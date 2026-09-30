/* ============================================================================================
 * countx_pp_grn_info.js - 690 Grn History / GRN Info (Party Processing reports)
 * Architecture.WinApp.PartyProcessingReports.frmPartyProcessingGrnInfo. Built on countx_hrm.js (window.HRM).
 * API /api/party-processing/reports/grn-info/{setup|combos|search|slip-check}
 * Prints: pp-169 (169-Register, the last Show's filters), pp-333 (grid Print, the row's GRN).
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/party-processing/reports/grn-info';
    var P = {};
    window.PpGrnInfo = P;
    var fyStart = null, lastRows = [], lastArgs = null;
    var fromTime = HRM.nowTime() + ':00', toTime = fromTime;     // DateTimePicker.Value keeps its time of day

    HRM.close = function () {
        window.close();
        setTimeout(function () { if (!window.closed) { if (history.length > 1) history.back(); else location.href = '/party-processing'; } }, 150);
    };

    function btnCell(text) { return '<button type="button" class="win-btn-action pp-cell-btn pp-print">' + text + '</button>'; }
    function n2(v) { if (v === null || v === undefined || v === '') return ''; return HRM.esc(HRM.fmtNum(v, 2).replace(/\.?0+$/, '')); }

    var grd = new HRM.Grid('DataGridHistory', {
        columns: [
            { key: '_print', caption: 'Print', width: 50, render: function () { return btnCell('Print'); } },
            { key: 'Id', hidden: true }, { key: 'DocumentTypeId', hidden: true },
            { key: 'GrnNo', caption: 'GrnNo', type: 'int', width: 60 },
            { key: 'GrnDate', caption: 'GrnDate', type: 'date', width: 90 },
            { key: 'StockPartyName', caption: 'StockPartyName', width: 160 },
            { key: 'ReferencePartyName', caption: 'ReferencePartyName', width: 150 },
            { key: 'GpDate', caption: 'GpDate', type: 'date', width: 90 },
            { key: 'GpNo', caption: 'GpNo', width: 60 },
            { key: 'VehicleNo', caption: 'VehicleNo', width: 90 },
            { key: 'BiltyNo', caption: 'BiltyNo', width: 80 },
            { key: 'TransporterName', caption: 'TransporterName', width: 130 },
            { key: 'CarriageAmount', caption: 'CarriageAmount', width: 100, sum: true, render: n2, align: 'right' },
            { key: 'IsApproved', caption: 'IsApproved', type: 'check', width: 70 },
            { key: 'GrnEntryDate', caption: 'GrnEntryDate', type: 'datetime', width: 130 },
            { key: 'GrnEntryUser', caption: 'GrnEntryUser', width: 100 },
            { key: 'GrnModifyDate', caption: 'GrnModifyDate', type: 'datetime', width: 130 },
            { key: 'GrnModifyUser', caption: 'GrnModifyUser', width: 100 },
            { key: 'GrnApprovedDate', caption: 'GrnApprovedDate', type: 'datetime', width: 130 },
            { key: 'GrnApprovedUser', caption: 'GrnApprovedUser', width: 100 },
            { key: 'JobOrderNo', caption: 'JobOrderNo', width: 80 },
            { key: 'WareHouseName', caption: 'WareHouseName', width: 130 },
            { key: 'ItemName', caption: 'ItemName', width: 160 },
            { key: 'CropYear', caption: 'CropYear', width: 70 },
            { key: 'JobLot', caption: 'JobLot', width: 100 },
            { key: 'PackType', caption: 'PackType', width: 90 },
            { key: 'UOM', caption: 'UOM', width: 70 },
            { key: 'ItemQty', caption: 'ItemQty', width: 80, sum: true, render: n2, align: 'right' },
            { key: 'GrossWeight', caption: 'GrossWeight', width: 90, sum: true, render: n2, align: 'right' },
            { key: 'EBWPerUnit', caption: 'EBWPerUnit', type: 'num', decimals: 3, width: 80 },
            { key: 'EBWTotal', caption: 'EBWTotal', width: 80, sum: true, render: n2, align: 'right' },
            { key: 'AdLsWeight', caption: 'AdLsWeight', width: 80, sum: true, render: n2, align: 'right' },
            { key: 'NetBillWeight', caption: 'NetBillWeight', width: 100, sum: true, render: n2, align: 'right' },
            { key: 'StockWeight', caption: 'StockWeight', width: 100, sum: true, render: n2, align: 'right' },
            { key: 'GrnType', caption: 'GrnType', width: 90 }
        ],
        filterRow: true, totals: true
    });
    /** DataGridHistory_ColumnButtonClick(Print) -> GenerateReport(Id): 333 slip. */
    HRM.$('DataGridHistory').addEventListener('click', function (e) {
        var b = e.target.closest('button.pp-print'); if (!b) return;
        var tr = b.closest('tr[data-i]'); if (!tr) return;
        var id = HRM.int(grd.rows()[+tr.getAttribute('data-i')].Id);
        var win = window.CrystalPrint ? CrystalPrint.reserve() : null;
        HRM.busy(b, function () {
            return HRM.get(API + '/slip-check', { id: id }).then(function () {
                return CrystalPrint.open('pp-333', { id: id }, null, win);
            }).catch(function (x) { if (win) CrystalPrint.release(win); HRM.fail(x); });
        }, 'pp-333');
    });

    function combos(d) {
        HRM.fill('CmbSupplier', d.stockParties || [], 'Id', 'name', { zero: '', keep: true });
        HRM.fill('cmbRefParty', d.refParties || [], 'Id', 'name', { zero: '', keep: true });
        HRM.fill('CmbItem', d.items || [], 'Id', 'name', { zero: '', keep: true });
        HRM.fill('CmbWarehouseName', d.warehouses || [], 'Id', 'name', { zero: '', keep: true });
        HRM.fill('CmbJobLotName', d.jobLots || [], 'Id', 'name', { zero: '', keep: true });
    }

    function args() {
        return {
            fromDate: HRM.val('GRNfromdate') ? HRM.val('GRNfromdate') + 'T' + fromTime : '',
            toDate: HRM.val('GRNToDate') ? HRM.val('GRNToDate') + 'T' + toTime : '',
            docNoFrom: HRM.int(HRM.val('txtGrnNoFrom').trim()), docNoTo: HRM.int(HRM.val('txtGrnNoTo').trim()),
            gpNoFrom: HRM.int(HRM.val('GpsNoFrom').trim()), gpNoTo: HRM.int(HRM.val('GpsNoTo').trim()),
            stockPartyId: HRM.comboVal('CmbSupplier'), refPartyId: HRM.comboVal('cmbRefParty'), itemId: HRM.comboVal('CmbItem'),
            warehouseId: HRM.comboVal('CmbWarehouseName'), jobLotId: HRM.comboVal('CmbJobLotName'), grnTypeId: HRM.comboVal('CmbGrnType')
        };
    }

    /** gridHisory(): rows -> grid; none -> empty grid (ClearStructure). The rows back the 169 print. */
    function gridHisory(btn) {
        var a = args();
        return HRM.busy(btn || 'btnshow', function () {
            return HRM.post(API + '/search', a).then(function (rows) {
                lastRows = rows || []; lastArgs = a;
                grd.set(lastRows);
                HRM.text('lblStatus', lastRows.length + ' record(s)');
            }).catch(HRM.fail);
        });
    }

    /** cmbDateType_ValueChanged: the date range of the choice, then gridHisory(). */
    function dateType(v, search) {
        var t = HRM.today();
        if (v === 1) { HRM.setVal('GRNfromdate', t); fromTime = '00:00:00'; }
        else if (v === 2) { HRM.setVal('GRNfromdate', HRM.addDays(t, -7)); fromTime = '00:00:00'; }
        else if (v === 3) { var u = new Date(); HRM.setVal('GRNfromdate', u.getUTCFullYear() + '-' + String(u.getUTCMonth() + 1).padStart(2, '0') + '-01'); fromTime = '00:00:00'; HRM.setVal('GRNToDate', t); toTime = '00:00:00'; }
        else if (v === 4) { HRM.setVal('GRNfromdate', new Date().getFullYear() + '-01-01'); fromTime = '00:00:00'; HRM.setVal('GRNToDate', t); toTime = '00:00:00'; }
        else if (v === 5) { if (fyStart) { HRM.setVal('GRNfromdate', fyStart); fromTime = '00:00:00'; } }
        else return Promise.resolve();
        return search === false ? Promise.resolve() : gridHisory();
    }
    HRM.$('cmbDateType').addEventListener('change', function () { dateType(HRM.comboVal('cmbDateType')); });

    /** Datetypefill(): the list with its zero row, "This Week" active (which fires the search). */
    function Datetypefill(list) {
        HRM.fill('cmbDateType', list, 'Id', 'Parameters', { zero: '' });
        HRM.setCombo('cmbDateType', 2);
        return dateType(2);
    }
    var dateTypes = [];

    P.btnshow = function (btn) { return gridHisory(btn); };
    /** Reset(): filters cleared (Item kept, as the form), Datetypefill. */
    P.btnNew = function () {
        ['CmbSupplier', 'cmbRefParty', 'CmbWarehouseName', 'CmbJobLotName', 'CmbGrnType'].forEach(function (id) { HRM.setCombo(id, 0); });
        ['txtGrnNoFrom', 'txtGrnNoTo', 'GpsNoFrom', 'GpsNoTo'].forEach(function (id) { HRM.setVal(id, ''); });
        Datetypefill(dateTypes);
        HRM.focus('cmbDateType');
    };
    P.btnRefresh = function (btn) {
        return HRM.busy(btn || 'btnRefresh', function () { return HRM.get(API + '/combos').then(combos).catch(HRM.fail); });
    };
    /** btn334Register_Click: the last Show's rows, else "Not Record Found For Display"; 169 prints them. */
    P.btn334Register = function (btn) {
        if (!lastRows.length) { HRM.box('Not Record Found For Display'); return; }
        return CrystalPrint.open('pp-169', lastArgs, btn || 'btn334Register');
    };
    P.exportGrid = function () { exportGrid('DataGridHistory', 'GrnHistory'); };
    P.shortCuts = function () {
        shortCuts([['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl + P', 'For print'],
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

    HRM.keys({                                                  // frmGRNHistory_KeyDown
        'ctrl+p': function () { P.btn334Register(); },
        'ctrl+s': function () { P.btnshow(); },
        'ctrl+r': function () { P.btnRefresh(); },
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+n': function () { P.btnNew(); },
        'ctrl+f5': function () { HRM.focus('cmbDateType'); },
        'ctrl+arrowup': function () { HRM.focus('cmbDateType'); },
        'ctrl+arrowdown': function () { var t = HRM.$('DataGridHistory').querySelector('tbody tr[data-i]'); if (t) t.focus(); },
        'ctrl+alt+control': P.shortCuts, 'ctrl+alt+alt': P.shortCuts,
        enterTab: false
    });
    HRM.footer(function (b) { return gridHisory(b).then(function () { HRM.$('historyBox').scrollIntoView({ block: 'start', behavior: 'smooth' }); }); });

    /** frmGRNHistory_Load: ComboFill, Datetypefill (searches with no Grn Type yet), GrnTypeBind ("Processed" active). */
    HRM.setVal('GRNfromdate', HRM.today());
    HRM.setVal('GRNToDate', HRM.today());
    HRM.loading(HRM.get(API + '/setup')).then(function (d) {
        fyStart = d.financialYearStart || null;
        dateTypes = d.dateTypes || [];
        combos(d);
        HRM.fill('CmbGrnType', [], 'Id', 'Name', { zero: '' });
        var p = Datetypefill(dateTypes);
        HRM.fill('CmbGrnType', d.grnTypes || [], 'Id', 'Name', { zero: '' });
        HRM.setCombo('CmbGrnType', 2);
        HRM.focus('cmbDateType');
        return p;
    }).catch(HRM.fail);
})();
