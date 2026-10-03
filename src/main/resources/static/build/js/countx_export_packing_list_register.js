/* countx_export_packing_list_register.js - PackingListRegister_Export.cs, screen 243 "Export Packing List Register".
 * Data: /api/export/packing-list-register. */
(function (global) {
    'use strict';
    var H = global.ExRptC, API = '/api/export/packing-list-register';
    var YEAR_START = '', ROWS = [], CUR = -1;
    var COLS = [
        { key: 'ExImInvoiceId', hidden: true }, { key: 'DocDate', cap: 'Doc Date', kind: 'date' }, { key: 'DocCode', cap: 'Doc Code' }, { key: 'ContractNos', cap: 'Contract Nos' },
        { key: 'InvoiceNo', cap: 'Invoice No' }, { key: 'Customer', cap: 'Customer' }, { key: 'DestinationPort', cap: 'Destination Port' }, { key: 'ItemName', cap: 'Item Name' },
        { key: 'CropYear', cap: 'Crop Year' }, { key: 'JobLot', cap: 'Job Lot' }, { key: 'PackingType', cap: 'Packing Type' }, { key: 'WareHouse', cap: 'Ware House' },
        { key: 'ContainerNo', cap: 'Container No' }, { key: 'SealNo', cap: 'Seal No' }, { key: 'ItemQty', cap: 'Item Qty', kind: 'q2', sum: true }, { key: 'PackUom', cap: 'Pack Uom' },
        { key: 'GrossWeight', cap: 'Gross Weight', kind: 'q2', sum: true }, { key: 'PackingWeight', cap: 'Packing Weight', kind: 'q2', sum: true },
        { key: 'PackingWeightTotal', cap: 'Packing Weight Total', kind: 'q2', sum: true }, { key: 'NetWeight', cap: 'Net Weight', kind: 'q2', sum: true },
        { key: 'RatePrice', cap: 'Rate Price', kind: 'rate' }, { key: 'RateUom', cap: 'Rate Uom' }
    ];
    var COMBOS = [['Cmbware', 'warehouses'], ['cmbItemName', 'items'], ['cmbCustomer', 'customers'], ['cmbpackingtype', 'packingTypes'], ['cmbcropyear', 'crops'],
        ['cmbJobLotSumm', 'jobLots'], ['cmbDestination', 'destinations'], ['cmbInvoice', 'invoices']];

    function render() { H.drawGrid('DataGridHistory', ROWS, COLS, { cur: CUR }); H.show('DataGridHistoryEmpty', !ROWS.length); }
    function bindCombos(d) { COMBOS.forEach(function (c) { H.bind(c[0], d[c[1]] || [], 'Id', 'Name'); }); }
    /* CmbDateTypeSumm_ValueChanged - "Financial Year" also sets To = today on this form. */
    function dateType() { H.applyDateType(H.val('CmbDateTypeSumm'), 'txtFromDateSumm', 'txtToDateSumm', YEAR_START, true); }
    function filters() {
        return { fromDate: H.val('txtFromDateSumm'), toDate: H.val('txtToDateSumm'), invoiceId: H.netI(H.val('cmbInvoice')), customerId: H.netI(H.val('cmbCustomer')),
            itemId: H.netI(H.val('cmbItemName')), cropYearId: H.netI(H.val('cmbcropyear')), jobLotId: H.netI(H.val('cmbJobLotSumm')), warehouseId: H.netI(H.val('Cmbware')),
            packingTypeId: H.netI(H.val('cmbpackingtype')), destinationPortId: H.netI(H.val('cmbDestination')) };
    }
    function show(btn) {
        return H.busy(btn, function () {
            return H.postJson(API + '/show', filters()).then(function (rows) { ROWS = rows || []; CUR = -1; render(); });
        });
    }
    /** LoadInvoices_Load: ComboFill, ParameterFill (Rows[1] = "This Week"). */
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            YEAR_START = d.yearStart || '';
            H.setText('txtFromDateSumm', H.today()); H.setText('txtToDateSumm', H.today());
            bindCombos(d);
            H.bindDateTypes('CmbDateTypeSumm', d.dateTypes);
            H.setVal('CmbDateTypeSumm', 2); dateType();
            H.$id('footerInfo').textContent = 'PackingListRegister_Export  -  Print 563';
            H.focus('CmbDateTypeSumm');
        }).catch(function (e) { H.box(e.message || 'Error occurred during database call.'); });
    }
    /** BtnNewSummary_Click: Customer / Warehouse / Item / Invoice / Packing Type / Job Lot cleared (Crop Year and Destination stay), Show, focus Date Type. */
    function reset(btn) {
        ['cmbCustomer', 'Cmbware', 'cmbItemName', 'cmbInvoice', 'cmbpackingtype', 'cmbJobLotSumm'].forEach(function (id) { H.setVal(id, 0); });
        return show(btn).then(function () { H.focus('CmbDateTypeSumm'); });
    }
    function refresh(btn) { return H.busy(btn, function () { return H.getJson(API + '/combos').then(bindCombos); }); }
    /** btnPrintSumm_Click: "Record Not Found For Display" without data, else 563-PackingListRegister_Export.rpt. */
    function print(btn) {
        if (!ROWS.length) { H.box('Record Not Found For Display'); return; }
        var f = filters();
        return H.print('563-PackingListRegister_Export.rpt', { fromDate: f.fromDate, toDate: f.toDate, itemId: f.itemId || undefined, exImInvoiceNoId: f.invoiceId || undefined,
            supplierCustomerId: f.customerId || undefined, cropYearId: f.cropYearId || undefined, jobLotId: f.jobLotId || undefined, warehouseId: f.warehouseId || undefined,
            packingTypeId: f.packingTypeId || undefined, destinationPortId: f.destinationPortId || undefined }, btn);
    }
    function shortcuts() {
        H.shortcuts([['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+S', 'For Show'], ['Ctrl+Up', 'For Focus Date Type Combo'],
            ['Ctrl+F5', 'For Focus Date Type Combo'], ['Ctrl+Down', 'For Focus Grid'], ['Ctrl+alt', 'To Show ShortCut Keys Form']]);
    }
    function toggleHistory() { var b = H.$id('DataGridHistoryBox'); if (b) b.scrollIntoView({ behavior: 'smooth' }); H.focusGrid('DataGridHistory'); }

    document.addEventListener('DOMContentLoaded', function () {
        H.wireFullscreen();
        H.$id('CmbDateTypeSumm').addEventListener('change', dateType);
        H.wireGrid('DataGridHistory', { select: function (i) { CUR = i; } });
        H.wireKeys({ shortcuts: shortcuts, handle: function (e, k) {
            if (!e.ctrlKey) return;
            if (k === 's') { e.preventDefault(); show(H.$id('BtnShowSumm')); }
            else if (k === 'n') { e.preventDefault(); reset(H.$id('BtnNewSummary')); }
            else if (k === 'p') { e.preventDefault(); print(H.$id('btnPrintSumm')); }
            else if (k === 'r') { e.preventDefault(); refresh(H.$id('btnRefreshSummary')); }
            else if (e.key === 'F5' || e.key === 'ArrowUp') { e.preventDefault(); H.focus('CmbDateTypeSumm'); }
            else if (e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('DataGridHistory'); }
        } });
        load();
    });

    global.ExportPackingListRegister = { show: show, reset: reset, refresh: refresh, print: print, shortcuts: shortcuts, toggleHistory: toggleHistory };
}(window));
