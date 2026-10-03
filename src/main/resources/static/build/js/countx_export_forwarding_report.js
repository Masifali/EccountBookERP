/* ============================================================================================
 * countx_export_forwarding_report.js - ExImForwardingHistory.cs, screen 269 "5012 Forwarding Report"
 * (Activity 'Detail', 46 grid columns, Print column -> 507 slip, 508 / 508_01 registers). The same filter panel and
 * events serve ExImForwardingCostingReport (913) through ExportFwdInit(cfg) from countx_export_forwarding_costing_report.js.
 * Data: /api/export/forwarding-report (Sp_ExImForwarding_SlipAndRegister_Rpt, USP_GetDataForDropDownFromGoodsForwarding,
 * Sp_ExImInvoice_GetAllMethod 'InvoiceNo').
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptB;
    var $id = H.$id, box = H.box;

    function init(cfg) {
        var API = cfg.api;
        var S = { rows: [], cur: -1, yearStart: '', suspend: true };

        function bindStatic(d) {
            ['customers', 'invoices', 'items', 'contracts', 'warehousesFrom', 'warehousesTo'].forEach(function (k) { if (d[k + 'Error']) box(d[k + 'Error']); });
            if (d.customers) H.bind('cmbSupplierName', d.customers);
            if (d.invoices) H.bind('cmbInvoiceNo', d.invoices);
            if (d.items) H.bind('CmbItem', d.items);
            if (d.contracts) H.bind('CmbContractNo', d.contracts);
            if (d.warehousesFrom) H.bind('CmbWarehouseFrom', d.warehousesFrom);
            if (d.warehousesTo) H.bind('CmbWarehouseTo', d.warehousesTo);
        }
        /** frmExportShipingLineBookingRpt_Load: all fills, grid cleared, Rows[0].Activate() (This Day) unless requested by another form. */
        function load() {
            return H.getJson(API + '/setup').then(function (d) {
                d = d || {}; S.yearStart = H.isoDate(d.yearStart);
                H.bind('cmbdatetype', d.dateTypes || [], 'Id', 'Parameters');
                ['txtdatefrom', 'txtdateto', 'GPfromdate', 'GPToDate'].forEach(function (id) { H.setText(id, H.today()); });
                bindStatic(d);
                S.suspend = false;
                var inv = H.netI(H.q('invoiceId'));
                if (inv > 0 || H.q('auto')) {
                    /* RequestedByOtherDocument: the caller set cmbInvoiceNo / txtdatefrom (FY start) / txtdateto (now) then btnShow_Click */
                    if (H.q('fromDate')) H.setText('txtdatefrom', H.isoDate(H.q('fromDate')));
                    if (H.q('toDate')) H.setText('txtdateto', H.isoDate(H.q('toDate')));
                    if (inv > 0) H.setVal('cmbInvoiceNo', inv);
                    return show($id('btnShow'));
                }
                H.setVal('cmbdatetype', '1'); dateType();
                H.focus('cmbdatetype');
            }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
        }
        function dateType() { if (!S.suspend) H.dateTypeChanged('cmbdatetype', 'txtdatefrom', 'txtdateto', S.yearStart, true); }
        /** cmbSupplierName_Leave -> InvoiceNoFill(customer). */
        function customerLeave() {
            H.getJson(API + '/invoices?supplierCustomerId=' + H.netI(H.val('cmbSupplierName'))).then(function (rows) { if (rows && rows.length) H.bind('cmbInvoiceNo', rows); }).catch(function (e) { box(e.message); });
        }
        function filters() {
            return { invoiceId: H.netI(H.val('cmbInvoiceNo')), contractId: H.netI(H.val('CmbContractNo')), supplierCustomerId: H.netI(H.val('cmbSupplierName')),
                itemId: H.netI(H.val('CmbItem')), fromWarehouseId: H.netI(H.val('CmbWarehouseFrom')), toWarehouseId: H.netI(H.val('CmbWarehouseTo')),
                fromDate: H.checked('txtdatefromChk') ? H.val('txtdatefrom') : '', toDate: H.checked('txtdatetoChk') ? H.val('txtdateto') : '',
                gpDateFrom: H.checked('GPfromdateChk') ? H.val('GPfromdate') : '', gpDateTo: H.checked('GPToDateChk') ? H.val('GPToDate') : '',
                gpSrNoFrom: H.netI(H.val('GpsNoFrom')), gpSrNoTo: H.netI(H.val('GpsNoTo')) };
        }
        function render() { H.drawGrid('grdfrm', cfg.cols, S.rows, cfg.gridOpts(S)); H.show('grdEmpty', S.rows.length === 0); }
        function show(btn) {
            return H.busy(btn, function () {
                return H.postJson(API + '/show', filters()).then(function (rows) { S.rows = rows || []; S.cur = -1; render(); }).catch(function (e) { box(e.message); });
            });
        }
        /** Reset(): Invoice / Contract / Customer emptied, focus Date From, grid cleared. */
        function reset() { H.setVal('cmbInvoiceNo', '0'); H.setVal('CmbContractNo', '0'); H.setVal('cmbSupplierName', '0'); H.focus('txtdatefrom'); S.rows = []; S.cur = -1; render(); }
        /** btnRefresh_Click: CustomerGetAll, ContractNoFill, ItemGetAll. */
        function refresh(btn) { return H.busy(btn, function () { return H.getJson(API + '/refresh').then(bindStatic).catch(function (e) { box(e.message); }); }); }
        /** The register prints on the shown data (dtData) - "Not Record Found For Display" when empty. */
        function print(key, btn) {
            if (!S.rows.length) { box('Not Record Found For Display'); return; }
            var f = filters();
            return H.print(key, { eximInvoiceId: f.invoiceId, fromDate: f.fromDate, toDate: f.toDate, gpDateFrom: f.gpDateFrom, gpDateTo: f.gpDateTo, gpSrNoFrom: f.gpSrNoFrom, gpSrNoTo: f.gpSrNoTo,
                supplierCustomerId: f.supplierCustomerId, itemId: f.itemId, contractId: f.contractId, warehouseId: f.fromWarehouseId, toWarehouseId: f.toWarehouseId, activity: cfg.activity }, btn);
        }
        /** PrintSlip(Id) - ExportPdfReport.ExImForwarding_Slip_507 (Sp_ExImForwarding_Rpt @Id) -> 507-ExImForwarding_Slip.rpt. */
        function printSlip(i) { var r = S.rows[i]; if (!r) return; if (!H.netI(r.Id)) { box('Not Record Found For Display'); return; } H.print('exp-507', { id: H.netI(r.Id) }); }
        function shortcuts() {
            H.shortcuts([['Ctrl+N', 'For New'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Date From'],
                ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
        }

        document.addEventListener('DOMContentLoaded', function () {
            H.fullscreenButtons();
            $id('cmbdatetype').addEventListener('change', dateType);
            $id('cmbSupplierName').addEventListener('change', customerLeave);
            H.gridEvents('grdfrm', { select: function (i) { S.cur = i; }, button: function (key, i) { if (key === 'Print' && cfg.hasPrintColumn) printSlip(i); },
                ctrlSpace: function (i) { if (cfg.hasPrintColumn) printSlip(i); } });
            document.addEventListener('keydown', function (e) {
                if (H.baseKeys(e, shortcuts)) return;
                var k = (e.key || '').toLowerCase();
                if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
                else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh($id('btnRefresh')); }
                else if (e.ctrlKey && k === 'p') { e.preventDefault(); print(cfg.ctrlPKey, $id(cfg.ctrlPButton)); }
                else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); H.focus('txtdatefrom'); }
                else if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grdfrm'); }
            });
            load();
        });

        return { show: show, reset: reset, refresh: refresh, print: print, shortcuts: shortcuts, state: S };
    }

    /* the 269 grid: GridSetting captions, formats, Print button column first, hidden Id (+ the columns hidden when grouped) */
    var COLS_269 = [
        { key: 'Print', caption: 'Print', ctr: true, html: function () { return '<button type="button" class="win-btn-cell" data-btn="Print">Print</button>'; } },
        { key: 'Id', hidden: true }, { key: 'ContractNo', caption: 'Contract No' }, { key: 'InvoiceNo', caption: 'Invoice No' }, { key: 'DocNo', caption: 'Doc No', ctr: true, fmt: 'i' },
        { key: 'DocDate', caption: 'Doc Date', fmt: 'dmy' }, { key: 'CustomerName', caption: 'Customer Name' }, { key: 'DeliveryTerm', caption: 'Delivery Term' }, { key: 'FcyCode', caption: 'Fcy Code' },
        { key: 'WarehouseFrom', caption: 'Warehouse From' }, { key: 'WarehouseTo', caption: 'Warehouse To' }, { key: 'ItemName', caption: 'Item Name' }, { key: 'CropYear', caption: 'Crop Year' },
        { key: 'JobLot', caption: 'Job Lot' }, { key: 'PackingType', caption: 'Packing Type' }, { key: 'ItemQty', caption: 'Item Qty', num: true, sum: true, fmt: 'o2' }, { key: 'UOM', caption: 'Out Uom' },
        { key: 'InnerQty', caption: 'Inner Qty', num: true, fmt: 'o2' }, { key: 'InnerUomCode', caption: 'Inner Uom' }, { key: 'InnerEbUnit', caption: 'Inner Eb Unit', num: true, fmt: 'o2' },
        { key: 'InnerEbTotal', caption: 'Inner Eb Total', num: true, fmt: 'o2' }, { key: 'GrossWeight', caption: 'Gross Weight', num: true, sum: true, fmt: 'o2' },
        { key: 'EbUnit', caption: 'Eb Unit', num: true, sum: true, fmt: 'o2' }, { key: 'EbTotal', caption: 'Eb Total', num: true, sum: true, fmt: 'o2' },
        { key: 'AdLs', caption: 'Add Less Amount', num: true, sum: true, fmt: 'o2' }, { key: 'NetWeight', caption: 'Net Weight', num: true, sum: true, fmt: 'o2' },
        { key: 'StockWeight', caption: 'Stock Weight', num: true, sum: true, fmt: 'o2' }, { key: 'MTon', caption: 'M Ton', num: true, sum: true, fmt: 'o2' },
        { key: 'CostingContribution', caption: 'Costing Contribution', num: true, sum: true, fmt: 'n2' }, { key: 'ProfitLoss', caption: 'Profit Loss', num: true, sum: true, fmt: 'n2' },
        { key: 'NoOfContainer', caption: 'FCL', ctr: true, fmt: 'i' }, { key: 'LoadingName', caption: 'Loading Name' }, { key: 'DestinationName', caption: 'Destination Name' },
        { key: 'Continent', caption: 'Continent' }, { key: 'Container', caption: 'Container' }, { key: 'SealNo', caption: 'Seal No' }, { key: 'InspectionLotNo', caption: 'Inspection Lot No' },
        { key: 'InspectionSubLotNo', caption: 'Inspection Sub Lot No' }, { key: 'GpDate', caption: 'Gp Date', fmt: 'dmy' }, { key: 'GpNo', caption: 'Gp No', ctr: true }, { key: 'DoNo', caption: 'Do No', ctr: true },
        { key: 'DoDate', caption: 'Do Date', fmt: 'd' }, { key: 'VehicleNo', caption: 'Vehicle No' }, { key: 'BiltyNo', caption: 'Bilty No' }, { key: 'TransporterName', caption: 'Transporter Name' },
        { key: 'Freight', caption: 'Freight Amount', num: true, sum: true, fmt: 'n2' }, { key: 'DriverName', caption: 'Driver Name' }, { key: 'DriverCellNo', caption: 'Driver Cell No' },
        { key: 'DriverCnicNo', caption: 'Driver CNIC No' }
    ];

    global.ExportFwdInit = init;
    if (document.body && document.body.getAttribute('data-export-page') === 'rptb-269') {
        global.ExportFwd = init({ api: '/api/export/forwarding-report', cols: COLS_269, activity: 'Detail', hasPrintColumn: true,
            ctrlPKey: 'exp-508', ctrlPButton: 'toolStripButton2', gridOpts: function (S) { return { cur: S.cur }; } });
    }
}(window));
