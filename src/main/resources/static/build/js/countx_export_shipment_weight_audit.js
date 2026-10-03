/* ============================================================================================
 * countx_export_shipment_weight_audit.js - frmAuditByWeightReport.cs (Architecture.WinApp.Audit_Dashboard), screen 261
 * "5016 Export Shipment Weight Audit". Data: /api/export/shipment-weight-audit. The report rows (one per gate pass)
 * are kept as dtHistory; the main grid is the first row per InvoiceNo plus GpCount, the detail grid the rows of the
 * selected invoice - exactly GridBind / grd_SelectionChanged. Prints: exp-618, exp-618-01, exp-258, exp-396.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptB, API = '/api/export/shipment-weight-audit';
    var $id = H.$id, box = H.box;
    var LINKS = { deliveryOrderHistory: '/export/delivery-order-report' };

    var S = { hist: [], main: [], detail: [], cur: -1, yearStart: '', suspend: true };

    var status = function (v) { return v === 'Completed' ? 'rptb-green' : (v === 'In Process' ? 'rptb-red' : ''); };
    var MAIN = [
        { key: 'SupplierCustomerId', hidden: true }, { key: 'CustomerName', caption: 'Customer Name', width: 170 }, { key: 'ContractNo', caption: 'Contract Number', width: 130 },
        { key: 'InvoiceId', hidden: true }, { key: 'InvoiceNo', caption: 'Invoice No', link: true }, { key: 'InvoiceDate', caption: 'Invoice Date', fmt: 'dmy' },
        { key: 'Containers', caption: 'FCL', num: true, sum: true, fmt: 'o0' }, { key: 'InvoiceGrossWeight', caption: 'Invoice Gross Weight', num: true, sum: true, fmt: 'p0' },
        { key: 'DoGrossWeight', caption: 'Do Gross Weight', num: true, sum: true, fmt: 'p0', link: true }, { key: 'WbWeight', caption: 'Weigh Bridge Weight', num: true, sum: true, fmt: 'p0' },
        { key: 'ExcessWeight', caption: 'Less / Excess DO - WB', num: true, sum: true, fmt: 'p3' }, { key: 'LessExcess', caption: 'Less Excess' },
        { key: 'AvgLessExcess', caption: 'Avg Ls/Exc Per FCL', num: true, sum: true, fmt: 'o0' }, { key: 'LessExcessAmount', caption: 'Less Excess Amount', num: true, sum: true, fmt: 'o0' },
        { key: 'InvoiceNetWeight', caption: 'Invoice Net Weight', num: true, sum: true, fmt: 'p0' }, { key: 'DoNetWeight', caption: 'Do Net Weight', num: true, sum: true, fmt: 'p0' },
        { key: 'DiffWeight', caption: 'Less / Excess Invoice - DO', num: true, sum: true, fmt: 'p0' }, { key: 'GpCount', caption: 'No Of GatePass', num: true, sum: true, fmt: 'o0' },
        { key: 'InvoiceStatus', caption: 'Dispatch Status', cls: status }
    ];
    var DETAIL = [
        { key: 'GpDate', caption: 'Gp Date', fmt: 'dmy' }, { key: 'GpId', hidden: true }, { key: 'GpNo', caption: 'Gp No', num: true, link: true }, { key: 'DOId', hidden: true },
        { key: 'DONo', caption: 'DO No', num: true, link: true }, { key: 'Container1', caption: 'Container1' }, { key: 'Container2', caption: 'Container2' },
        { key: 'DoNetWeight', caption: 'Do Net Weight', num: true, sum: true, fmt: 'p3' }, { key: 'DoPackingWeight', caption: 'Do Packing Weight', num: true, sum: true, fmt: 'p3' },
        { key: 'DoGrossWeight', caption: 'Do Gross Weight', num: true, sum: true, fmt: 'p3' }, { key: 'DoOtherWeight', caption: 'Do Other Weight', num: true, sum: true, fmt: 'p3' },
        { key: 'WbWeight', caption: 'Wb Weight', num: true, sum: true, fmt: 'p3' }, { key: 'Less/Excess', caption: 'WB-DO Difference', num: true, sum: true, fmt: 'p3' },
        { key: 'LessExcess', caption: 'Less Excess' }
    ];

    /** frmExportShipingLineBookingRpt_Load: ParameterFill, ComboFill, Rows[1].Activate() (This Week -> From = today - 7). */
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {}; S.yearStart = H.isoDate(d.yearStart);
            H.bind('cmbperemeter', d.dateTypes || [], 'Id', 'Parameters');
            H.setText('txtdatefrom', H.today()); H.setText('txtdateto', H.today());
            var c = d.combos || {};
            if (d.combosError) box(d.combosError);
            H.bind('CmbInvoiceNo', c.invoices || []); H.bind('CmbContractNo', c.contracts || []);
            S.suspend = false;
            H.setVal('cmbperemeter', '2'); dateType();
            H.focus('cmbperemeter');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    function dateType() { if (!S.suspend) H.dateTypeChanged('cmbperemeter', 'txtdatefrom', 'txtdateto', S.yearStart, false); }
    function filters() {
        return { fromDate: H.val('txtdatefrom'), toDate: H.val('txtdateto'), invoiceId: H.netI(H.val('CmbInvoiceNo')), contractId: H.netI(H.val('CmbContractNo')), status: H.radio('status') };
    }
    /** GridBind: dt = first row per InvoiceNo (+ GpCount), then GridSetting / GridColumnsColourChange. */
    function gridBind() {
        return H.postJson(API + '/show', filters()).then(function (rows) {
            S.hist = rows || []; S.main = []; S.detail = []; S.cur = -1;
            var seen = {};
            S.hist.forEach(function (r) {
                var inv = H.str(r.InvoiceNo);
                if (seen[inv]) return;
                seen[inv] = true;
                var gp = S.hist.filter(function (x) { return H.str(x.InvoiceNo) === inv; }).length;
                S.main.push({ SupplierCustomerId: r.SupplierCustomerId, CustomerName: r.CustomerName, ContractNo: r.LcOrderNo, InvoiceId: r.InvoiceId, InvoiceNo: r.InvoiceNo,
                    InvoiceDate: r.InvoiceDate, Containers: r.Containers, InvoiceGrossWeight: r.InvoiceGwTotal, DoGrossWeight: r.DoGwTotal, WbWeight: r.WbGwTotal,
                    ExcessWeight: r.ExcessWtTotal, LessExcess: r.LessExcess, AvgLessExcess: r.AvgLessExcess, LessExcessAmount: r.LessExcessAmount,
                    InvoiceNetWeight: r.InvoiceNwTotal, DoNetWeight: r.DoNwTotal, DiffWeight: r.InvNwDoNwDiff, GpCount: gp, InvoiceStatus: r.InvoiceStatus });
            });
            render();
        });
    }
    function render() {
        H.drawGrid('grd', MAIN, S.main, { cur: S.cur });
        H.show('grdEmpty', S.main.length === 0);
        H.drawGrid('grdDetail', DETAIL, S.detail, {});
    }
    /** grd_SelectionChanged - the rows of the selected InvoiceNo into the detail grid. */
    function select(i) {
        S.cur = i;
        var m = S.main[i]; if (!m) return;
        S.detail = S.hist.filter(function (x) { return H.str(x.InvoiceNo) === H.str(m.InvoiceNo); }).map(function (r) {
            return { GpDate: r.GpDate, GpId: r.GpId, GpNo: r.GpSrNo, DOId: r.DOId, DONo: r.DONo, Container1: r.Container1, Container2: r.Container2,
                DoNetWeight: r.doNetWeight, DoPackingWeight: r.doPackingWeight, DoGrossWeight: r.doGrossWeight, DoOtherWeight: r.doOtherWeight,
                WbWeight: r.WbWeight, 'Less/Excess': r.ExcessWt, LessExcess: r.LessExcessDoWise };
        });
        H.drawGrid('grdDetail', DETAIL, S.detail, {});
    }
    function show(btn) { return H.busy(btn, function () { return gridBind().catch(function (e) { box(e.message); }); }); }
    /** Reset(): focus Date From, GridBind (filters are NOT cleared on this form). */
    function reset(btn) { H.focus('txtdatefrom'); return show(btn); }
    function printAll(key, btn) {
        if (!S.hist.length) { box('Not Record Found For Display'); return; }
        var f = filters();
        return H.print(key, { eximInvoiceId: f.invoiceId, exImLcOrderId: f.contractId, fromDate: f.fromDate, toDate: f.toDate, status: f.status }, btn);
    }
    function print618(btn) { return printAll('exp-618', btn); }
    function print618_01(btn) { return printAll('exp-618-01', btn); }
    /** grd_LinkClicked. */
    function mainLink(key, i) {
        var r = S.main[i]; if (!r) return;
        var invoiceId = H.netI(r.InvoiceId);
        if (key === 'InvoiceNo') {
            H.getJson(API + '/invoice-exists?invoiceId=' + invoiceId).then(function (d) {
                if (!d || !d.found) box('Not Record Found For Display'); else H.print('exp-618', { eximInvoiceId: invoiceId });
            }).catch(function (e) { box(e.message); });
        } else if (key === 'DoGrossWeight' && H.netD(r.DoGrossWeight) > 0) {
            /* DeliveryOrderHistory: DateTypeId 5, DOType Export, FY start .. Date To, InvoiceId, RadWithSecondWeight */
            global.open(LINKS.deliveryOrderHistory + '?dateTypeId=5&doType=Export&invoiceId=' + invoiceId + '&fromDate=' + (S.yearStart || H.today()) + '&toDate=' + H.val('txtdateto') + '&weight=second', '_blank');
        }
    }
    /** grdDetail_LinkClicked: GpNo -> 258 gate pass with WB, DONo -> 396 export DO slip (DocumentTypeId 84). */
    function detailLink(key, i) {
        var r = S.detail[i]; if (!r) return;
        if (key === 'GpNo') H.print('exp-258', { id: H.netI(r.GpId) });
        if (key === 'DONo') { if (!H.netI(r.DOId)) { box('No Record Found For Display'); return; } H.print('exp-396', { id: H.netI(r.DOId), documentTypeId: 84 }); }
    }
    function shortcuts() {
        H.shortcuts([['Ctrl+N', 'For New'], ['Ctrl+E', 'For Close'], ['Ctrl+F5', 'For Focus on Date Type'], ['Ctrl+S', 'For Showing Data'], ['Ctrl+P', 'For Print'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    document.addEventListener('DOMContentLoaded', function () {
        H.fullscreenButtons();
        $id('cmbperemeter').addEventListener('change', dateType);
        H.gridEvents('grd', { select: select, link: mainLink, ctrlSpace: function (i) { mainLink('InvoiceNo', i); } });
        H.gridEvents('grdDetail', { link: detailLink, ctrlSpace: function (i) { detailLink('GpNo', i); } });
        document.addEventListener('keydown', function (e) {
            if (H.baseKeys(e, shortcuts)) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset($id('btnnew')); }
            else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); H.focus('cmbperemeter'); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); show($id('btnShow')); }
            else if (e.ctrlKey && k === 'p') { e.preventDefault(); print618($id('btnPrint')); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grd'); }
        });
        load();
    });

    global.ExportWgt = { show: show, reset: reset, print618: print618, print618_01: print618_01, shortcuts: shortcuts };
}(window));
