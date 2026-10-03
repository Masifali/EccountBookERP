/* countx_export_third_party_inspection_lot_tracking_report.js - frmThirdPartyInspectionLotTrackingReport.cs, screen 859.
 * Data: /api/export/third-party-inspection-lot-tracking-report. */
(function (global) {
    'use strict';
    var H = global.ExRptC, API = '/api/export/third-party-inspection-lot-tracking-report';
    var YEAR_START = '', ROWS = [], CARDS = [], STAGES = [], CUR = -1;
    /* grdsetting: id columns hidden, captions "Total MTons" / "Invoice MTons" / "Balance MTons" / "Contract No", 10 dd-MMM-yy dates,
       GridWrappingAndColumnSettings totals except LotMTons / UsedMTons / BalMTons, Inspection + Lab buttons first (frozen). */
    var COLS = [
        { key: 'Id', hidden: true }, { key: 'ExImLcOrderId', hidden: true }, { key: 'LotCodeTrackingNo', cap: 'Lot Code Tracking No' }, { key: 'ItemName', cap: 'Item Name' },
        { key: 'InspectionAgency', cap: 'Inspection Agency' }, { key: 'TradeType', cap: 'Trade Type' }, { key: 'CustomerName', cap: 'Customer Name' }, { key: 'LcOrderNo', cap: 'Contract No' },
        { key: 'ScheduleCode', cap: 'Schedule Code' }, { key: 'CustomerContractNo', cap: 'Customer Contract No' }, { key: 'InvoiceNo', cap: 'Invoice No' },
        { key: 'LoadingScheduleDate', cap: 'Loading Schedule Date', kind: 'date' }, { key: 'InspectionScheduleDate', cap: 'Inspection Schedule Date', kind: 'date' },
        { key: 'MTons', cap: 'Invoice MTons', kind: 'q3', sum: true }, { key: 'LotMTons', cap: 'Total MTons', kind: 'q3' }, { key: 'UsedMTons', cap: 'Used MTons', kind: 'q3' },
        { key: 'BalMTons', cap: 'Balance MTons', kind: 'q3' }, { key: 'ContractScheduleId', hidden: true }, { key: 'InvoiceId', hidden: true }, { key: 'SupplierCustomerId', hidden: true },
        { key: 'ExporterLotRefNo', cap: 'Exporter Lot Ref No' }, { key: 'ItemId', hidden: true }, { key: 'SamplingResponsibility', cap: 'Sampling Responsibility' }, { key: 'ProcessStatus', cap: 'Process Status' },
        { key: 'StockReservedDate', cap: 'Stock Reserved Date', kind: 'date' }, { key: 'SampleTakenDate', cap: 'Sample Taken Date', kind: 'date' }, { key: 'StockSealedDate', cap: 'Stock Sealed Date', kind: 'date' },
        { key: 'SampleDispatchedDate', cap: 'Sample Dispatched Date', kind: 'date' }, { key: 'CourierNo', cap: 'Courier No' }, { key: 'LabCountry', cap: 'Lab Country' },
        { key: 'SampleETADestination', cap: 'Sample ETA Destination', kind: 'date' }, { key: 'SampleATADestination', cap: 'Sample ATA Destination', kind: 'date' },
        { key: 'ReportReferenceNo', cap: 'Report Reference No' }, { key: 'ReportDate', cap: 'Report Date', kind: 'date' }, { key: 'DateOfInspection', cap: 'Date Of Inspection', kind: 'date' },
        { key: 'RequiredAnalysis', cap: 'Required Analysis' }, { key: 'ResultStatus', cap: 'Result Status' }, { key: 'ResultRemarks', cap: 'Result Remarks' }, { key: 'SampleStatus', cap: 'Sample Status' },
        { key: 'InstructionsOrRemarks', cap: 'Instructions Or Remarks' }
    ];

    function render() {
        H.drawGrid('grd', ROWS, COLS, {
            cur: CUR,
            leadHead: '<th class="lead">Inspection</th><th class="lead">Lab</th>',
            lead: function (r, i) {
                return '<td class="lead win-cell-btn"><button type="button" class="win-edit" data-act="inspection" data-i="' + i + '">Inspection</button></td>' +
                    '<td class="lead win-cell-btn"><button type="button" class="win-edit" data-act="lab" data-i="' + i + '">Lab</button></td>';
            }
        });
        H.show('grdEmpty', !ROWS.length);
    }
    /* grdEtaDestination: Id (hidden) | LotInspectionStage | NoOfLots | MTons ("#,##.##", summed), sorted by Id. */
    function renderCards() {
        var box = H.$id('grdEtaDestination'), tot = 0, lots = 0;
        box.innerHTML = CARDS.map(function (c) {
            tot += H.netD(c.MTons); lots += H.netI(c.NoOfLots);
            return '<div class="exr-card"><b>' + H.esc(c.LotInspectionStage) + '</b><span>No Of Lots: ' + H.netI(c.NoOfLots) + '</span><span>MTons: ' + H.fmtOpt(c.MTons, 2) + '</span></div>';
        }).join('') + (CARDS.length ? '<div class="exr-card" style="border-left-color:#333;"><b>&Sigma;</b><span>No Of Lots: ' + lots + '</span><span>MTons: ' + H.fmtOpt(tot, 2) + '</span></div>' : '');
    }
    function bindCombos(d) {
        H.bind('CmbLotTrackingNo', d.trackingNos || [], 'Id', 'LotRefNo', ['LodgeDate', 'ItemName', 'MTons', 'FarmingNTrade']);
        H.bind('cmbsupplierCustomer', d.customers || [], 'Id', 'Name');
        H.bind('CmbItemName', d.items || [], 'Id', 'Name');
    }
    /* CmbDateType_Leave (this form applies the date type on Leave). */
    function dateType() { H.applyDateType(H.val('CmbDateType'), 'datFromDate', 'datToDate', YEAR_START); }
    function actionId() { var r = document.querySelector('input[name="lotAction"]:checked'); return r ? H.netI(r.value) : 0; }
    function filters() {
        return { fromDate: H.val('datFromDate'), fromChecked: H.checked('datFromDateChk'), toDate: H.val('datToDate'), toChecked: H.checked('datToDateChk'),
            customerId: H.netI(H.val('cmbsupplierCustomer')), itemId: H.netI(H.val('CmbItemName')), recId: H.netI(H.val('CmbLotTrackingNo')), actionId: actionId(), skipZero: H.checked('chkSkipZero') };
    }
    function show(btn) {
        return H.busy(btn, function () {
            return H.postJson(API + '/show', filters()).then(function (d) {
                d = d || {};
                ROWS = d.rows || []; CUR = -1; render();
                CARDS = ROWS.length ? (d.cards || []) : STAGES.map(function (s) { return { Id: s.Id, LotInspectionStage: s.Description, NoOfLots: 0, MTons: 0 }; });
                renderCards();
            });
        });
    }
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            YEAR_START = d.yearStart || ''; STAGES = d.stages || [];
            H.bindDateTypes('CmbDateType', d.dateTypes);
            H.setText('datFromDate', H.today()); H.setText('datToDate', H.today());
            H.setVal('CmbDateType', 2); dateType();
            CARDS = STAGES.map(function (s) { return { Id: s.Id, LotInspectionStage: s.Description, NoOfLots: 0, MTons: 0 }; });
            renderCards();
            bindCombos(d);
            H.$id('footerInfo').textContent = 'frmThirdPartyInspectionLotTrackingReport  -  Print 514_01';
            H.focus('CmbDateType');
        }).catch(function (e) { H.box(e.message || 'Error occurred during database call.'); });
    }
    /** Reset(): Lot Tracking No / Item / Customer cleared, focus Lot Tracking No. */
    function reset() { H.setVal('CmbLotTrackingNo', 0); H.setVal('CmbItemName', 0); H.setVal('cmbsupplierCustomer', 0); H.focus('CmbLotTrackingNo'); }
    function refresh(btn) { return H.busy(btn, function () { return H.getJson(API + '/combos').then(bindCombos); }); }
    function print(btn) {
        if (!ROWS.length) { H.box('No Record Found For Display'); return; }
        var f = filters();
        return H.print('514_01_ThirdPartyInspectionLotTrackingReport.rpt', { fromDate: f.fromChecked ? f.fromDate : undefined, toDate: f.toChecked ? f.toDate : undefined,
            recId: f.recId || undefined, supplierCustomerId: f.customerId || undefined, itemId: f.itemId || undefined, actionId: f.actionId || undefined }, btn);
    }
    /* frmThirdPartyInspection / frmLabAgainstThirdPartyInspection are separate desktop forms (not part of this port). */
    function openInspection(id) { H.box('The Third Party Inspection form is not available on this page' + (id ? ' (record ' + id + ').' : '.')); }
    function openLab(id) { H.box('The Lab Against Third Party Inspection form is not available on this page' + (id ? ' (record ' + id + ').' : '.')); }
    function shortcuts() { H.shortcuts([]); }
    function toggleHistory() { var b = H.$id('grdBox'); if (b) b.scrollIntoView({ behavior: 'smooth' }); H.focusGrid('grd'); }

    document.addEventListener('DOMContentLoaded', function () {
        H.wireFullscreen();
        H.$id('CmbDateType').addEventListener('change', dateType);
        H.wireGrid('grd', { select: function (i) { CUR = i; }, button: function (i, act) { var r = ROWS[i]; if (!r) return; var id = H.netI(H.col(r, 'Id')); if (act === 'inspection') openInspection(id); else openLab(id); } });
        H.wireKeys({ shortcuts: shortcuts, handle: function (e, k) {
            if (!e.ctrlKey) return;
            if (k === 's') { e.preventDefault(); show(H.$id('btnshow')); }
            else if (k === 'r') { e.preventDefault(); refresh(H.$id('btnRefresh')); }
            else if (k === 'n') { e.preventDefault(); reset(); }
            else if (k === 'p') { e.preventDefault(); print(H.$id('btnPrintCurrent')); }
            else if (e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grd'); }
            else if (e.key === 'ArrowUp' || e.key === 'F5') { e.preventDefault(); H.focus('cmbsupplierCustomer'); }
        } });
        load();
    });

    global.ExportLotTracking = { show: show, reset: reset, refresh: refresh, print: print, openInspection: openInspection, openLab: openLab, shortcuts: shortcuts, toggleHistory: toggleHistory };
}(window));
