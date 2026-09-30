/* ============================================================================================
 * countx_pp_gate_pass_report.js - 691 Gate Pass Party Processing (report)
 * Architecture.WinApp.PartyProcessingReports.frmGatePassPartyProcessing. Built on countx_hrm.js (window.HRM).
 * API /api/party-processing/reports/gate-pass/{setup|combos|search}
 * Print: pp-300 (300-Register, the last Show's filters).
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/party-processing/reports/gate-pass';
    var P = {};
    window.PpGatePassRpt = P;
    var fyStart = null, lastRows = [], lastArgs = null;
    var fromTime = HRM.nowTime() + ':00', toTime = fromTime;     // DateTimePicker.Value keeps its time of day

    HRM.close = function () {
        window.close();
        setTimeout(function () { if (!window.closed) { if (history.length > 1) history.back(); else location.href = '/party-processing'; } }, 150);
    };

    function n2(v) { if (v === null || v === undefined || v === '') return ''; return HRM.esc(HRM.fmtNum(v, 2).replace(/\.?0+$/, '')); }

    var grd = new HRM.Grid('grdfrm', {
        columns: [
            { key: 'Id', hidden: true }, { key: 'DocumentTypeId', hidden: true },
            { key: 'GpDate', caption: 'GpDate', type: 'date', width: 90 },
            { key: 'GpSrNo', caption: 'GpSrNo', width: 60 },
            { key: 'GpType', caption: 'GpType', width: 80 },
            { key: 'GatePassType', caption: 'GatePassType', width: 110 },
            { key: 'GpTypeSrNo', caption: 'GpTypeSrNo', width: 80 },
            { key: 'StockParty', caption: 'StockParty', width: 160 },
            { key: 'ReferenceParty', caption: 'ReferenceParty', width: 150 },
            { key: 'VarietyName', caption: 'VarietyName', width: 120 },
            { key: 'VehicleType', caption: 'VehicleType', width: 90 },
            { key: 'VehicleNo', caption: 'VehicleNo', width: 90 },
            { key: 'BiltyNo', caption: 'BiltyNo', width: 80 },
            { key: 'ItemQty', caption: 'ItemQty', width: 80, sum: true, render: n2, align: 'right' },
            { key: 'OtherRemarks', caption: 'OtherRemarks', width: 160 },
            { key: 'InDateTimeStamp', caption: 'InDateTimeStamp', type: 'datetime', width: 130 },
            { key: 'OutDateTimeStamp', caption: 'OutDateTimeStamp', type: 'datetime', width: 130 },
            { key: 'Status', caption: 'Status', width: 80 },
            { key: 'SupplierWeight', caption: 'SupplierWeight', width: 100, sum: true, render: n2, align: 'right' },
            { key: 'FactoryWeight', caption: 'FactoryWeight', width: 100, sum: true, render: n2, align: 'right' },
            { key: 'DifferenceWeight', caption: 'DifferenceWeight', width: 100, sum: true, render: n2, align: 'right' },
            { key: 'WBStatus', caption: 'WBStatus', width: 90 },
            { key: 'IsWeighable', caption: 'IsWeighable', type: 'check', width: 80 }
        ],
        filterRow: true, totals: true
    });

    function combos(d) {
        HRM.fill('CmbStockParty', d.stockParties || [], 'Id', 'name', { zero: '', keep: true });
        HRM.fill('cmbReferenceParty', d.refParties || [], 'Id', 'name', { zero: '', keep: true });
        HRM.fill('cmbDocumentType', d.documentTypes || [], 'Id', 'name', { zero: '', keep: true });
        HRM.fill('cmbGatePassType', d.gatePassTypes || [], 'Id', 'name', { zero: '', keep: true });
    }

    function args() {
        return {
            fromDate: HRM.val('gpFromDate') ? HRM.val('gpFromDate') + 'T' + fromTime : '',
            toDate: HRM.val('gpToDate') ? HRM.val('gpToDate') + 'T' + toTime : '',
            docNoFrom: HRM.int(HRM.val('txtGpFrom')), docNoTo: HRM.int(HRM.val('txtGPTo')),
            stockPartyId: HRM.comboVal('CmbStockParty'), refPartyId: HRM.comboVal('cmbReferenceParty'),
            documentTypeId: HRM.comboVal('cmbDocumentType'), gatePassType: HRM.comboText('cmbGatePassType'), status: HRM.comboText('cmbIsAccept')
        };
    }

    /** GridFill(). */
    function GridFill(btn) {
        var a = args();
        return HRM.busy(btn || 'btnshow', function () {
            return HRM.post(API + '/search', a).then(function (rows) {
                lastRows = rows || []; lastArgs = a;
                grd.set(lastRows);
                HRM.text('lblStatus', lastRows.length + ' record(s)');
            }).catch(HRM.fail);
        });
    }

    /** cmbDateType_ValueChanged: the date range of the choice, then the search. */
    function dateType(v) {
        var t = HRM.today();
        if (v === 1) { HRM.setVal('gpFromDate', t); fromTime = '00:00:00'; }
        else if (v === 2) { HRM.setVal('gpFromDate', HRM.addDays(t, -7)); fromTime = '00:00:00'; }
        else if (v === 3) { var u = new Date(); HRM.setVal('gpFromDate', u.getUTCFullYear() + '-' + String(u.getUTCMonth() + 1).padStart(2, '0') + '-01'); fromTime = '00:00:00'; HRM.setVal('gpToDate', t); toTime = '00:00:00'; }
        else if (v === 4) { HRM.setVal('gpFromDate', new Date().getFullYear() + '-01-01'); fromTime = '00:00:00'; HRM.setVal('gpToDate', t); toTime = '00:00:00'; }
        else if (v === 5) { if (fyStart) { HRM.setVal('gpFromDate', fyStart); fromTime = '00:00:00'; } }
        else return Promise.resolve();
        return GridFill();
    }
    HRM.$('cmbDateType').addEventListener('change', function () { dateType(HRM.comboVal('cmbDateType')); });

    P.btnshow = function (btn) { return GridFill(btn); };
    /** btnReset_Click: Status, Reference Party, GP numbers, GatePass Type cleared; Document Type back to its first row. */
    P.btnReset = function () {
        HRM.setCombo('cmbIsAccept', 0); HRM.setCombo('cmbReferenceParty', 0); HRM.setCombo('cmbGatePassType', 0);
        HRM.setVal('txtGpFrom', ''); HRM.setVal('txtGPTo', '');
        var s = HRM.$('cmbDocumentType'); if (s && s.options.length > 1) { s.selectedIndex = 1; HRM.refreshCombos(); }
        HRM.focus('cmbDateType');
    };
    P.btnRefresh = function (btn) {
        return HRM.busy(btn || 'btnRefresh', function () { return HRM.get(API + '/combos').then(combos).catch(HRM.fail); });
    };
    /** toolStripButton1_Click: the last Show's rows, else "Not Record Found For Display"; 300 prints them. */
    P.btnPrint = function (btn) {
        if (!lastRows.length) { HRM.box('Not Record Found For Display'); return; }
        return CrystalPrint.open('pp-300', lastArgs, btn || 'btnPrint');
    };
    P.exportGrid = function () { exportGrid('grdfrm', 'GatePassPartyProcessing'); };
    P.shortCuts = function () {
        shortCuts([['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For print'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus On DateType in Filter']]);
    };

    function shortCuts(rows) {
        HRM.modal({ title: 'ShortCut Keys', width: 'min(520px, 96vw)', html: '<table class="win-grid hrm-grid"><thead><tr><th>KeyCombination</th><th>Description</th></tr></thead><tbody>' +
            rows.map(function (r) { return '<tr><td>' + HRM.esc(r[0]) + '</td><td>' + HRM.esc(r[1]) + '</td></tr>'; }).join('') + '</tbody></table>' });
    }
    function exportGrid(tableId, fileName) {
        var t = HRM.$(tableId);
        if (!t || !t.tBodies[0] || !t.tBodies[0].querySelector('tr[data-i]')) { HRM.box('Not Record Found For Display'); return; }
        var html = '';
        Array.prototype.forEach.call(t.rows, function (tr) {
            if (tr.classList.contains('hrm-filter-row') || tr.classList.contains('hrm-empty')) return;
            var cells = '';
            Array.prototype.forEach.call(tr.cells, function (c) {
                if (c.style.display === 'none' || c.querySelector('button')) return;
                var box = c.querySelector('input[type=checkbox]');
                cells += '<td>' + HRM.esc(box ? (box.checked ? 'True' : 'False') : c.textContent) + '</td>';
            });
            html += '<tr>' + cells + '</tr>';
        });
        var blob = new Blob(['﻿<html><head><meta charset="utf-8"></head><body><table border="1">' + html + '</table></body></html>'], { type: 'application/vnd.ms-excel' });
        var a = document.createElement('a');
        a.href = URL.createObjectURL(blob); a.download = fileName + '.xls';
        document.body.appendChild(a); a.click();
        setTimeout(function () { URL.revokeObjectURL(a.href); a.remove(); }, 500);
    }
    HRM.keys({                                                  // frmGatePassPartyProcessing_KeyDown
        'ctrl+p': function () { P.btnPrint(); },
        'ctrl+s': function () { P.btnshow(); },
        'ctrl+r': function () { P.btnRefresh(); },
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+n': function () { P.btnReset(); },
        'ctrl+f5': function () { HRM.focus('cmbDateType'); },
        'ctrl+arrowup': function () { HRM.focus('cmbDateType'); },
        'ctrl+arrowdown': function () { var t = HRM.$('grdfrm').querySelector('tbody tr[data-i]'); if (t) t.focus(); },
        'ctrl+alt+control': P.shortCuts, 'ctrl+alt+alt': P.shortCuts,
        enterTab: false
    });
    HRM.footer(function (b) { return GridFill(b).then(function () { HRM.$('historyBox').scrollIntoView({ block: 'start', behavior: 'smooth' }); }); });

    /** frmGatePassReport_Load: Datetypefill ("This Week" -> search with empty filters), ComboFill, IsAcceptFill. */
    HRM.setVal('gpFromDate', HRM.today());
    HRM.setVal('gpToDate', HRM.today());
    ['CmbStockParty', 'cmbReferenceParty', 'cmbDocumentType', 'cmbGatePassType', 'cmbIsAccept'].forEach(function (id) { HRM.fill(id, [], 'Id', 'name', { zero: '' }); });
    HRM.loading(HRM.get(API + '/setup')).then(function (d) {
        fyStart = d.financialYearStart || null;
        HRM.fill('cmbDateType', d.dateTypes || [], 'Id', 'Parameters', { zero: '' });
        HRM.setCombo('cmbDateType', 2);
        var p = dateType(2);
        combos(d);
        HRM.fill('cmbIsAccept', d.statuses || [], 'Id', 'Status', { zero: '' });
        HRM.focus('cmbDateType');
        return p;
    }).catch(HRM.fail);
})();
