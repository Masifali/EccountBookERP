/* ============================================================================================
 * countx_export_sale_comparisons_report.js - ExportCustomerWiseComparisonsSummeryReport.cs, screen 759 "5007 Sale Comparisons Report".
 * Data: /api/export/sale-comparisons-report (SpStaticColumnNames, USP_GetDataForDropDownFromExportInvoiceVoucher,
 * usp_ExportSaleDetail_report for "Detail", SpExImInvoice_ExportComparisonsSummery_Report otherwise).
 * Prints: exp-534 / exp-535 (two-group comparisons), exp-536 (other comparisons), exp-536-01 (Detail), links exp-521 / exp-560.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptB, API = '/api/export/sale-comparisons-report';
    var $id = H.$id, box = H.box;
    var S = { rows: [], cur: -1, reportType: '', hasSecond: false, yearStart: '' };

    var TWO_GROUP = ['Customer And Item Wise Comparison', 'Item And Customer Wise Comparison'];
    function isTwoGroup(t) { return TWO_GROUP.indexOf(t) >= 0; }

    /* GridSettingDetail: hidden InvoiceDocTypeId, ExImInvoiceId, ContractId, ItemId, JobLot, PackingType; links InvoiceNo, ContractNo, ContainerNos, NoOfAttachments. */
    var DETAIL = [
        { key: 'VoucherDate', caption: 'Voucher Date', fmt: 'd' }, { key: 'VoucherNo', caption: 'Voucher No', fmt: 'i' }, { key: 'InvoiceDocTypeId', hidden: true }, { key: 'ExImInvoiceId', hidden: true },
        { key: 'InvoiceNo', caption: 'Invoice No', link: true }, { key: 'InvoiceDate', caption: 'Invoice Date', fmt: 'd' }, { key: 'ContractId', hidden: true }, { key: 'ContractNo', caption: 'Contract No', link: true },
        { key: 'ContractDate', caption: 'Contract Date', fmt: 'd' }, { key: 'EFormNo', caption: 'E Form No' }, { key: 'EFormDate', caption: 'E Form Date', fmt: 'd' }, { key: 'CustomerName', caption: 'Customer Name' },
        { key: 'ItemId', hidden: true }, { key: 'ItemName', caption: 'Item Name' }, { key: 'JobLot', hidden: true }, { key: 'PackingType', hidden: true },
        { key: 'Qty', caption: 'Qty', num: true, sum: true, fmt: 'o3' }, { key: 'MTons', caption: 'M Tons', num: true, sum: true, fmt: 'o3' }, { key: 'NoOfContainers', caption: 'No Of Containers', num: true, sum: true, fmt: 'i' },
        { key: 'PackingSize', caption: 'Packing Size' }, { key: 'RatePrice', caption: 'Rate Price', num: true, fmt: 'o3' }, { key: 'FcyCode', caption: 'Fcy Code' }, { key: 'RateUom', caption: 'Rate Uom' },
        { key: 'FcyAmount', caption: 'Fcy Amount', num: true, sum: true, fmt: 'n2' }, { key: 'ExchangeRate', caption: 'Exchange Rate', num: true, fmt: 'n4' }, { key: 'LcyAmount', caption: 'Lcy Amount', num: true, sum: true, fmt: 'n2' },
        { key: 'PaymentTerms', caption: 'Payment Terms' }, { key: 'DestinationPort', caption: 'Destination Port' }, { key: 'BLNumber', caption: 'BL Number' }, { key: 'ContainerNos', caption: 'Container Nos', link: true },
        { key: 'DocStatus', caption: 'Document Status' }, { key: 'PaymentStatus', caption: 'Payment Status' }, { key: 'ExImFarmingTypes', caption: 'Farming Types' }, { key: 'NoOfAttachments', caption: 'No Of Attachments', num: true, fmt: 'i', link: true }
    ];
    /* GridSetting (summary): GroupName captioned with the first row's GroupCaption; two-group types show SecondGroupName grouped by GroupName. */
    function summaryCols() {
        var first = S.rows[0] || {};
        var cols = [{ key: 'GroupCaption', hidden: true }, { key: 'GroupName', caption: H.str(first.GroupCaption) || 'Group Name', hidden: S.hasSecond }];
        if (S.hasSecond) cols.push({ key: 'SecondGroupCaption', hidden: true }, { key: 'SecondGroupName', caption: H.str(first.SecondGroupCaption) || 'Second Group Name' });
        return cols.concat([
            { key: 'MTons', caption: 'M Tons', num: true, sum: true, fmt: 'o3' }, { key: 'PrctExport', caption: 'Prct Export', num: true, sum: true, fmt: 'o2' }, { key: 'FcyCode', caption: 'Fcy Code' },
            { key: 'AvgPrice', caption: 'Avg Price', num: true, fmt: 'n2' }, { key: 'Fcy_Amount', caption: 'Fcy Amount', num: true, sum: true, fmt: 'n2' }, { key: 'FcrAvgRate', caption: 'Fcr Avg Rate', num: true, fmt: 'n4' },
            { key: 'PKR_Amount', caption: 'PKR Amount', num: true, sum: true, fmt: 'n2' }]);
    }

    function bindCombos(d) {
        var c = (d && d.combos) || {};
        if (d && d.combosError) box(d.combosError);
        H.bind('CmbItemCategory', c.ItemCategory || []); H.bind('CmbItemType', c.ItemType || []); H.bind('CmbItemName', c.Items || []); H.bind('CmbCustomerName', c.Customer || []);
        H.bind('CmbSalePerson', c.SalesPerson || []); H.bind('CmbContractNo', c.ContractNo || []); H.bind('CmbInvoiceNo', c.Invoice || []); H.bind('CmbSeaPort', c.DestinationPort || []);
    }
    /** ReportTypeBind: Rows[0].Activate() -> "Detail". */
    function bindReportTypes(d) {
        if (d.reportTypesError) box(d.reportTypesError);
        H.bind('CmbReportType', d.reportTypes || []);
        var s = $id('CmbReportType'); if (s.options.length > 1) { s.selectedIndex = 1; H.refreshCombos(); }
    }
    /** Load: From = FY start then today - 15 (no calling form), To = today, ReportTypeBind, ComboFill, GridBind, focus From. */
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {}; S.yearStart = H.isoDate(d.yearStart);
            H.setText('datFromDate', H.addDays(H.today(), -15)); H.setText('datToDate', H.today());
            bindReportTypes(d); bindCombos(d);
            var sa = H.netI(H.q('stockAccountId')), it = H.netI(H.q('itemId'));
            if (H.q('fromDate')) { H.setText('datFromDate', H.isoDate(H.q('fromDate'))); H.setText('datToDate', H.isoDate(H.q('toDate')) || H.today()); }
            if (sa > 0) { var so = $id('CmbItemStockAccountHistory'); so.innerHTML = '<option value="0"></option><option value="' + sa + '" selected>' + sa + '</option>'; H.refreshCombos(); }
            if (it > 0) H.setVal('CmbItemName', it);
            return gridBind().catch(function (e) { box(e.message); }).then(function () { H.focus('datFromDate'); });
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    function filters() {
        return { reportTypeId: H.netI(H.val('CmbReportType')), reportType: H.selText('CmbReportType').trim(),
            fromDate: H.checked('datFromDateChk') ? H.val('datFromDate') : '', toDate: H.checked('datToDateChk') ? H.val('datToDate') : '',
            itemCategoryId: H.netI(H.val('CmbItemCategory')), itemTypeId: H.netI(H.val('CmbItemType')), itemId: H.netI(H.val('CmbItemName')),
            supplierCustomerId: H.netI(H.val('CmbCustomerName')), salePersonId: H.netI(H.val('CmbSalePerson')), contractId: H.netI(H.val('CmbContractNo')),
            invoiceId: H.netI(H.val('CmbInvoiceNo')), destinationPortId: H.netI(H.val('CmbSeaPort')), stockAccountId: H.netI(H.val('CmbItemStockAccountHistory')) };
    }
    /** GridBind: the print buttons hide first, then show for the chosen report type; "Report Type filed is required" without one. */
    function gridBind() {
        ['btnReport', 'btnFormat2', 'btn536Report', 'btnPrint536_01'].forEach(function (id) { H.show(id, false); });
        S.rows = []; S.cur = -1;
        var f = filters();
        if (f.reportTypeId === 0 || !f.reportType) { render(); return Promise.reject(new Error('Report Type filed is required')); }
        return H.postJson(API + '/show', f).then(function (d) {
            S.reportType = (d && d.reportType) || f.reportType; S.hasSecond = !!(d && d.hasSecondGroup); S.rows = (d && d.rows) || [];
            if (S.reportType === 'Detail') H.show('btnPrint536_01', true);
            else if (isTwoGroup(S.reportType)) { H.show('btnReport', true); H.show('btnFormat2', true); }
            else H.show('btn536Report', true);
            render();
        });
    }
    function render() {
        if (!S.rows.length) { H.clearGrid('grdfrm'); var t = $id('grdfrm').querySelector('thead'); if (t) t.innerHTML = ''; H.show('grdEmpty', true); return; }
        H.show('grdEmpty', false);
        if (S.reportType === 'Detail') H.drawGrid('grdfrm', DETAIL, S.rows, { cur: S.cur });
        else H.drawGrid('grdfrm', summaryCols(), S.rows, S.hasSecond ? { cur: S.cur, group: 'GroupName', groupCaption: H.str((S.rows[0] || {}).GroupCaption), groupTotals: true } : { cur: S.cur });
    }
    function show(btn) { return H.busy(btn, function () { return gridBind().catch(function (e) { box(e.message); }); }); }
    /** Reset(): six combos emptied, focus To Date, report type back to the first row. */
    function reset() {
        ['CmbItemCategory', 'CmbItemType', 'CmbCustomerName', 'CmbItemName', 'CmbContractNo', 'CmbSeaPort'].forEach(function (id) { H.setVal(id, '0'); });
        H.focus('datToDate');
        var s = $id('CmbReportType'); if (s.options.length > 1) { s.selectedIndex = 1; H.refreshCombos(); }
    }
    /** btnRefreshSummary_Click: ComboFill, ReportTypeBind. */
    function refresh(btn) { return H.busy(btn, function () { return H.getJson(API + '/setup').then(function (d) { bindCombos(d || {}); bindReportTypes(d || {}); }).catch(function (e) { box(e.message); }); }); }
    /** The four register prints on dtdetail (the shown filters + report type). */
    function print(key, btn) {
        if (!S.rows.length) { box('Not Record Found For Display'); return; }
        var f = filters();
        return H.print(key, { fromDate: f.fromDate, toDate: f.toDate, supplierCustomerId: f.supplierCustomerId, destinationPortId: f.destinationPortId, itemId: f.itemId,
            itemTypeId: f.itemTypeId, itemCategoryId: f.itemCategoryId, lcContractId: f.contractId, exImInvoiceId: f.invoiceId, salePersonId: f.salePersonId,
            itemStockAccountId: f.stockAccountId, reportType: S.reportType }, btn);
    }
    /** Ctrl+P / Alt+1 / Alt+2 follow the report type as the KeyDown does. */
    function printPrimary() { if (isTwoGroup(S.reportType)) print('exp-534', $id('btnReport')); else if (S.reportType !== 'Detail') print('exp-536', $id('btn536Report')); else print('exp-536-01', $id('btnPrint536_01')); }
    /** grdfrm_LinkClicked (Detail grid). */
    function link(key, i) {
        var r = S.rows[i]; if (!r) return;
        var inv = H.netI(r.ExImInvoiceId);
        if (key === 'InvoiceNo') { if (!inv) { box('No Record Found For Display'); return; } H.print('exp-521', { eximInvoiceId: inv }); }
        else if (key === 'ContractNo') H.contractDetail(H.netI(r.ContractId), 759);
        else if (key === 'ContainerNos') {
            if (!inv) { box('No Record Found For Display'); return; }
            H.getJson(API + '/cro-exists?invoiceId=' + inv).then(function (d) { if (!d || !d.found) box('No Record Found For Display'); else H.print('exp-560', { exImInvoiceId: inv }); }).catch(function (e) { box(e.message); });
        }
        else if (key === 'NoOfAttachments') H.attachments(inv, H.netI(r.InvoiceDocTypeId));
    }
    function shortcuts() {
        H.shortcuts([['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+P', 'For Print'], ['Ctrl+S', 'For Show'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    document.addEventListener('DOMContentLoaded', function () {
        H.fullscreenButtons();
        H.gridEvents('grdfrm', { select: function (i) { S.cur = i; }, link: link, ctrlSpace: function (i) { if (S.reportType === 'Detail') link('InvoiceNo', i); } });
        document.addEventListener('keydown', function (e) {
            if (H.baseKeys(e, shortcuts)) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh($id('btnRefreshSummary')); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); show($id('BtnShow')); }
            else if ((e.ctrlKey && k === 'p') || (e.altKey && (e.key === '1' || e.code === 'Numpad1'))) { e.preventDefault(); printPrimary(); }
            else if (e.altKey && (e.key === '2' || e.code === 'Numpad2')) { e.preventDefault(); if (isTwoGroup(S.reportType)) print('exp-535', $id('btnFormat2')); }
            else if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); H.focus('datFromDate'); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grdfrm'); }
        });
        load();
    });

    global.ExportCmp = { show: show, reset: reset, refresh: refresh, print: print, shortcuts: shortcuts };
}(window));
