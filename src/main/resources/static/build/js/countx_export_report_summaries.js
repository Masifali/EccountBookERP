/* ============================================================================================
 * countx_export_report_summaries.js - ExportReportSummaries.cs "Export Summary" (no ScreenDefinition row).
 * Data: /api/export/report-summaries (SpExImInvoice_ExportsSummery_Reports). Print: exp-o-538 (538-ExportSummariesReport.rpt)
 * with the filters of the last GridBind (the desktop prints dtdetail, the DataTable of the last bind).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptO, $id = H.$id, box = H.box;
    var API = '/api/export/report-summaries';
    var S = { rows: [], last: null };
    function cols(reportType) {
        /* GridSetting(): Description caption by CmbReportType.Value 1 / 3 / 2, width 350; three "#,#" sums */
        var cap = reportType === 1 ? 'Customer Name' : reportType === 3 ? 'Item Name' : reportType === 2 ? 'Port Name' : 'Description';
        return [
            { key: 'Description', caption: cap, width: 350 },
            { key: 'FcyCode', caption: 'FcyCode' },
            { key: 'M_Tons', caption: 'M_Tons', num: true, sum: true, fmt: 'h0' },
            { key: 'FcyAmount', caption: 'FcyAmount', num: true, sum: true, fmt: 'h0' },
            { key: 'PKR_Amount', caption: 'PKR_Amount', num: true, sum: true, fmt: 'h0' },
            { key: 'CompCountry', caption: 'CompCountry' }, { key: 'CompContactPerson', caption: 'CompContactPerson' },
            { key: 'CompMobileA', caption: 'CompMobileA' }, { key: 'CompMobileB', caption: 'CompMobileB' }, { key: 'CompMobileC', caption: 'CompMobileC' },
            { key: 'CompEmailA', caption: 'CompEmailA' }, { key: 'CompEmailB', caption: 'CompEmailB' },
            { key: 'CompanyWebsite', caption: 'CompanyWebsite' }, { key: 'CompanyFaxNo', caption: 'CompanyFaxNo' },
            { key: 'OrgReportingRemarks', caption: 'OrgReportingRemarks' }
        ];
    }
    function filters() {
        return { fromDate: H.val('datFromDate'), toDate: H.val('datToDate'), itemId: H.netI(H.val('CmbItemName')),
            customerId: H.netI(H.val('CmbCustomerName')), contractId: H.netI(H.val('CmbContractNo')), portId: H.netI(H.val('CmbSeaPort')),
            reportType: H.netI(H.val('CmbReportType')) ? H.val('CmbReportType') : '' };
    }
    /** GridBind(): rows -> RetrieveStructure + GridSetting; none -> ClearStructure. */
    function gridBind() {
        var f = filters();
        return H.postJson(API + '/show', f).then(function (rows) {
            S.rows = rows || []; S.last = f;
            if (S.rows.length) { H.drawGrid('grdfrm', cols(H.netI(f.reportType)), S.rows, {}); H.show('grdfrmEmpty', false); }
            else { H.clearGrid('grdfrm'); $id('grdfrm').querySelector('thead').innerHTML = ''; H.show('grdfrmEmpty', true); }
        }).catch(function (e) { box(e.message); });
    }
    function load() {
        H.setText('datFromDate', H.today()); H.setText('datToDate', H.today());
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            (d.errors || []).forEach(function (m) { box(m); });
            if ((d.reportTypes || []).length) H.bind('CmbReportType', d.reportTypes, 'Id', 'name', true);
            if ((d.customers || []).length) H.bind('CmbCustomerName', d.customers, 'Id', 'name', true);
            if ((d.contracts || []).length) H.bind('CmbContractNo', d.contracts, 'Id', 'name', true);
            if ((d.items || []).length) H.bind('CmbItemName', d.items, 'Id', 'name', true);
            if ((d.ports || []).length) H.bind('CmbSeaPort', d.ports, 'Id', 'name', true);
            $id('datToDate').focus();
            return gridBind();
        }).catch(function (e) { box(e.message); });
    }
    function show(btn) { return H.busy(btn, gridBind); }
    /** Reset(): the five combo texts blank, focus To Date, GridBind. */
    function btnNew(btn) {
        ['CmbCustomerName', 'CmbItemName', 'CmbContractNo', 'CmbSeaPort', 'CmbReportType'].forEach(function (id) { H.setVal(id, '0'); });
        $id('datToDate').focus();
        return H.busy(btn, gridBind);
    }
    /** btnReport_Click: "Not Record Found For Display" without rows, else 538 of the last bind. */
    function btnReport(btn) {
        if (!S.rows.length || !S.last) { box('Not Record Found For Display'); return; }
        var f = S.last;
        return H.print('exp-o-538', { fromDate: f.fromDate, toDate: f.toDate, itemId: f.itemId, supplierCustomerId: f.customerId,
            lcOrderId: f.contractId, destinationPortId: f.portId, reportType: f.reportType }, btn);
    }
    document.addEventListener('DOMContentLoaded', function () {
        H.fullscreenButtons();
        H.gridEvents('grdfrm', {});
        document.addEventListener('keydown', function (e) {
            if (H.baseKeys(e, null)) return;
            if (e.ctrlKey && (e.key || '').toLowerCase() === 'n') { e.preventDefault(); btnNew($id('btnnew')); }
        });
        load();
    });
    global.ExportSum = { show: show, btnNew: btnNew, btnReport: btnReport };
}(window));
