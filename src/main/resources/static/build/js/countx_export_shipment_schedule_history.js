/* ============================================================================================
 * countx_export_shipment_schedule_history.js - ExImShipmentScheduleHistory.cs "Shipment Schedule" (no ScreenDefinition row).
 * Data: /api/export/shipment-schedule-history (SpExImLcOrder_ShipmentSchedule_Rpt). Prints: exp-o-527
 * (527-ExImLcOrderShipmentScheduleDetailRegister.rpt) and exp-o-527-cw (...CustomerWise.rpt), see ExportReportsOReportSupport.
 * ContractNo opens the sales contract (/export/sales-contract?id=LcOrderId).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptO, $id = H.$id, box = H.box;
    var API = '/api/export/shipment-schedule-history';
    var S = { rows: [] };
    /* GridSetting(): LcOrderId / Id hidden, captions, Σ on the four weight columns ("0,0", total "#,##0.##"). */
    var COLS = [
        { key: 'LcOrderId', hidden: true }, { key: 'Id', hidden: true },
        { key: 'LcOrderDocNo', caption: 'LcOrderDocNo', num: true },
        { key: 'CustomerName', caption: 'Customer' },
        { key: 'ContractNo', caption: 'ContractNo', link: true },
        { key: 'ContractDate', caption: 'ContractDate', fmt: 'd' },
        { key: 'DeliveryTerms', caption: 'DeliveryTerms' },
        { key: 'ContractWeight', caption: 'Contract M.Ton', num: true, sum: true, fmt: 'z2', tfmt: 'o2' },
        { key: 'ShippedWeight', caption: 'Shipped M.Ton', num: true, sum: true, fmt: 'z2', tfmt: 'o2' },
        { key: 'BalanceWeight', caption: 'Balance M.Ton', num: true, sum: true, fmt: 'z2', tfmt: 'o2' },
        { key: 'LcStatus', caption: 'LcStatus' },
        { key: 'ItemName', caption: 'ItemName' },
        { key: 'NoOfCntr', caption: 'NoOfCntr', num: true },
        { key: 'ShpSchQtyMtons', caption: 'M.Ton', num: true, sum: true, fmt: 'z2', tfmt: 'o2' },
        { key: 'ShipmentSchDate', caption: 'ShipmentSchDate', fmt: 'd' },
        { key: 'ScheduleMonth', caption: 'Schedule Month' },
        { key: 'PackingRemarks', caption: 'Pack Remarks' },
        { key: 'LotNo', caption: 'Lot No' },
        { key: 'LineStatus', caption: 'LineStatus' },
        { key: 'DestinationPort', caption: 'Destination' }
    ];
    function filters() {
        return { fromDate: H.val('txtdatefrom'), toDate: H.val('txtdateto'),
            contractId: H.netI(H.val('cmbcontractNo')), customerId: H.netI(H.val('cmbSupplierName')) };
    }
    function bindCombos(d) {
        (d.errors || []).forEach(function (m) { box(m); });
        /* CustomerGetAll / ContractNoGetAll only rebind when rows came back */
        if ((d.customers || []).length) H.bind('cmbSupplierName', d.customers, 'Id', 'name', true);
        if ((d.contracts || []).length) H.bind('cmbcontractNo', d.contracts, 'Id', 'name', true);
    }
    /** GridBind(): the grid always shows the latest result (RetrieveStructure, even when empty). */
    function gridBind() {
        return H.postJson(API + '/show', filters()).then(function (rows) {
            S.rows = rows || [];
            H.drawGrid('grdfrm', COLS, S.rows, {});
            H.show('grdfrmEmpty', !S.rows.length);
        }).catch(function (e) { box(e.message); });
    }
    function load() {
        H.setText('txtdatefrom', H.today());
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            H.setText('txtdateto', d.yearEnd || H.today());     /* txtdateto.Value = ActiveYr.End_Period */
            bindCombos(d);
            $id('txtdatefrom').focus();
            return gridBind();
        }).catch(function (e) { box(e.message); });
    }
    function show(btn) { return H.busy(btn, gridBind); }
    /** btnnew_Click: ContractNoGetAll, CustomerGetAll, Reset() (texts blank, focus Date From, GridBind). */
    function btnNew(btn) {
        return H.busy(btn, function () {
            return H.getJson(API + '/combos').then(function (d) {
                bindCombos(d || {});
                H.setVal('cmbcontractNo', '0'); H.setVal('cmbSupplierName', '0');
                $id('txtdatefrom').focus();
                return gridBind();
            });
        });
    }
    /** ShowRegister527: ExpRptSalesContractExportRegister_518 - only the customer reaches the procedure (see the contract). */
    function print527(btn) {
        var f = filters();
        return H.print('exp-o-527', { supplierCustomerId: f.customerId }, btn);
    }
    /** ShowRegister527CustomerWise: ExpRptSalesContractExportRegister_523 - dates + customer. */
    function print527Cw(btn) {
        var f = filters();
        return H.print('exp-o-527-cw', { fromDate: f.fromDate, toDate: f.toDate, supplierCustomerId: f.customerId }, btn);
    }
    document.addEventListener('DOMContentLoaded', function () {
        H.fullscreenButtons();
        H.gridEvents('grdfrm', {
            link: function (key, i) {
                var r = S.rows[i]; if (!r) return;
                if (key === 'ContractNo' && H.netI(r.LcOrderId) > 0) global.open('/export/sales-contract?id=' + H.netI(r.LcOrderId), '_blank');
            }
        });
        document.addEventListener('keydown', function (e) {
            if (H.baseKeys(e, null)) return;
            if (e.ctrlKey && (e.key || '').toLowerCase() === 'n') { e.preventDefault(); btnNew($id('btnnew')); }
        });
        load();
    });
    global.ExportSsh = { show: show, btnNew: btnNew, print527: print527, print527Cw: print527Cw };
}(window));
