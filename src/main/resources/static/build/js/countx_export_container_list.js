/* ============================================================================================
 * countx_export_container_list.js - ExImContainerList.cs, screen 268 "Container List" (Container Register).
 * Data: /api/export/container-list (SpExportDeliveryOrderInvoiceGatepassRegister_rpt). No print on the desktop form.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptA, N = global.ExportRptN, $id = H.$id, box = H.box;
    var API = '/api/export/container-list';
    var S = { rows: [] };

    /* RetrieveStructure keeps every procedure column in order; GridSetting widths + DoWeight Σ ("0,0" / "#,##0"). */
    var COLS = [
        { key: 'Customer', caption: 'Customer', width: 140 }, { key: 'ContractNo', caption: 'ContractNo' },
        { key: 'InvoiceNo', caption: 'InvoiceNo', width: 150 }, { key: 'InvoiceDate', caption: 'InvoiceDate', fmt: 'd' },
        { key: 'DoNo', caption: 'DoNo' }, { key: 'DoDate', caption: 'DoDate', fmt: 'd' }, { key: 'ItemName', caption: 'ItemName', width: 180 },
        { key: 'LotNo', caption: 'LotNo' },
        { key: 'DoWeight', caption: 'DoWeight', num: true, sum: true, fmt: N.cellTotal(N.fmt00, function (v) { return H.fmtFixed(v, 0); }) },
        { key: 'GpNo', caption: 'GpNo' }, { key: 'GpDate', caption: 'GpDate', fmt: 'd' }, { key: 'VehicleNo', caption: 'VehicleNo' },
        { key: 'BiltyNo', caption: 'BiltyNo' }, { key: 'ContainerNo1', caption: 'ContainerNo1' }, { key: 'Container2', caption: 'Container2' }
    ];

    /** InvoiceNoGetAll / ContractNoGetAll: BindDDLNew only when the source has rows (an empty source leaves the old list). */
    function combos() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.invoicesError) box(d.invoicesError);
            if (d.contractsError) box(d.contractsError);
            if ((d.invoices || []).length) H.bind('cmbInvoiceNo', d.invoices, 'Id', 'name');
            if ((d.contracts || []).length) H.bind('cmbcontractNo', d.contracts, 'Id', 'name');
        }).catch(function (e) { box(e.message); });
    }
    /** GridBind + GridSetting. */
    function gridBind() {
        return H.postJson(API + '/show', { invoiceId: N.intVal('cmbInvoiceNo'), contractId: N.intVal('cmbcontractNo') }).then(function (d) {
            S.rows = (d && d.rows) || [];
            H.drawGrid('grdfrm', COLS, S.rows, {});
            H.show('grdfrmEmpty', !S.rows.length);
        }).catch(function (e) { box(e.message); });
    }
    function show(btn) { return H.busy(btn || 'btnShow', gridBind); }
    /** btnnew_Click: InvoiceNoGetAll, ContractNoGetAll, GridBind, then Reset (both combos cleared, focus Invoice No, GridBind again). */
    function btnNew(btn) {
        return H.busy(btn || 'btnnew', function () {
            return combos().then(gridBind).then(function () {
                H.setVal('cmbcontractNo', '0'); H.setVal('cmbInvoiceNo', '0'); H.focus('cmbInvoiceNo');
                return gridBind();
            });
        });
    }
    document.addEventListener('DOMContentLoaded', function () {
        N.embedFooter('cntFooter');
        H.fullscreenButtons();
        H.gridEvents('grdfrm', {});
        document.addEventListener('keydown', function (e) {
            if (H.baseKeys(e)) return;
            if (e.ctrlKey && (e.key || '').toLowerCase() === 'n') { e.preventDefault(); btnNew(); }
        });
        combos().then(function () { return show($id('btnShow')); });
    });
    global.ExportCnt = { btnNew: btnNew, show: show };
}(window));
