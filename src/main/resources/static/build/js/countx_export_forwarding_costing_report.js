/* ============================================================================================
 * countx_export_forwarding_costing_report.js - ExImForwardingCostingReport.cs, screen 913 "Export Forwarding (Costing Report)".
 * The filter panel / events are ExImForwardingHistory's (ExportFwdInit in countx_export_forwarding_report.js); this file is the
 * 913 grid: @Activity 'Summary', grouped by InvoiceNo, DocDate as month, the five weight/costing columns at the end, the
 * hiddenColumnsSummary list hidden, and the single print 508_02_ForwardingSummary_Register.rpt (exp-508-02).
 * ============================================================================================ */
(function (global) {
    'use strict';
    /* GridSetting: visible = InvoiceNo, DocDate("Month", MMM), CustomerName, DeliveryTerm, FcyCode, ItemName, JobLot, DestinationName, Continent,
       then StockWeight, NetWeight, MTon, CostingContribution, ProfitLoss (positions Count-5..Count-1). */
    var COLS_913 = [
        { key: 'Id', hidden: true }, { key: 'ContractNo', hidden: true }, { key: 'InvoiceNo', caption: 'Invoice No' }, { key: 'DocNo', hidden: true },
        { key: 'DocDate', caption: 'Month', fmt: 'mmm' }, { key: 'CustomerName', caption: 'Customer Name' }, { key: 'DeliveryTerm', caption: 'Delivery Term' }, { key: 'FcyCode', caption: 'Fcy Code' },
        { key: 'WarehouseFrom', hidden: true }, { key: 'WarehouseTo', hidden: true }, { key: 'ItemName', caption: 'Item Name' }, { key: 'CropYear', hidden: true },
        { key: 'JobLot', caption: 'Job Lot' }, { key: 'PackingType', hidden: true }, { key: 'UOM', hidden: true }, { key: 'ItemQty', hidden: true }, { key: 'GrossWeight', hidden: true },
        { key: 'EbUnit', hidden: true }, { key: 'EbTotal', hidden: true }, { key: 'AdLs', hidden: true }, { key: 'NoOfContainer', hidden: true }, { key: 'LoadingName', hidden: true },
        { key: 'DestinationName', caption: 'Destination Name' }, { key: 'Continent', caption: 'Continent' }, { key: 'Container', hidden: true }, { key: 'SealNo', hidden: true },
        { key: 'GpDate', hidden: true }, { key: 'GpNo', hidden: true }, { key: 'DoNo', hidden: true }, { key: 'DoDate', hidden: true }, { key: 'VehicleNo', hidden: true },
        { key: 'BiltyNo', hidden: true }, { key: 'TransporterName', hidden: true }, { key: 'Freight', hidden: true }, { key: 'DriverName', hidden: true }, { key: 'DriverCellNo', hidden: true },
        { key: 'DriverCnicNo', hidden: true },
        { key: 'StockWeight', caption: 'Stock Weight', num: true, sum: true, fmt: 'o2' }, { key: 'NetWeight', caption: 'Net Weight', num: true, sum: true, fmt: 'o2' },
        { key: 'MTon', caption: 'M Ton', num: true, sum: true, fmt: 'o2' }, { key: 'CostingContribution', caption: 'Costing Contribution', num: true, sum: true, fmt: 'n2' },
        { key: 'ProfitLoss', caption: 'Profit Loss', num: true, sum: true, fmt: 'n2' }
    ];
    global.ExportFwd = global.ExportFwdInit({ api: '/api/export/forwarding-costing-report', cols: COLS_913, activity: 'Summary', hasPrintColumn: false,
        ctrlPKey: 'exp-508-02', ctrlPButton: 'btnPrint508_02',
        gridOpts: function (S) {
            /* grdfrm.RootTable.Groups.Add("InvoiceNo") - the grid groups the rows by invoice (stable order inside a group) */
            S.rows = S.rows.map(function (r, i) { r.__i = i; return r; }).sort(function (a, b) { var x = String(a.InvoiceNo || ''), y = String(b.InvoiceNo || ''); return x < y ? -1 : (x > y ? 1 : a.__i - b.__i); });
            return { cur: S.cur, group: 'InvoiceNo', groupCaption: 'Invoice No', groupTotals: true };
        } });
}(window));
