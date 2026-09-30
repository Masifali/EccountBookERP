/* ============================================================================================
 * countx_pp_grn_register.js - 692 / 693 GRN / GDN Party Processing registers (?mode=grn|gdn)
 * Architecture.WinApp.PartyProcessingReports.frmGRNGDNPartyProcessing (Tag -> DocumentTypeId 49 / 89).
 * API /api/party-processing/reports/grn-register/{setup|combos|search}?mode=
 * Prints: pp-332, pp-337, pp-338 (GRN), pp-338gdn (GDN) with the last Show's filters.
 * ============================================================================================ */
(function () {
    'use strict';
    var MODE = document.body.getAttribute('data-mode') === 'gdn' ? 'gdn' : 'grn';
    var API = '/api/party-processing/reports/grn-register';
    var P = {};
    window.PpGrnGdn = P;
    var fyStart = null, lastRows = [], lastArgs = null, documentTypeId = MODE === 'gdn' ? 89 : 49, dateTypes = [];
    var fromTime = HRM.nowTime() + ':00', toTime = fromTime;     // DateTimePicker.Value keeps its time of day

    HRM.close = function () {
        window.close();
        setTimeout(function () { if (!window.closed) { if (history.length > 1) history.back(); else location.href = '/party-processing'; } }, 150);
    };

    function n3(v) { if (v === null || v === undefined || v === '') return ''; return HRM.esc(HRM.fmtNum(v, 3).replace(/\.?0+$/, '')); }

    var grd = new HRM.Grid('grdfrm', {
        columns: [
            { key: 'Id', hidden: true },
            { key: 'DocNo', caption: 'DocNo', type: 'int', width: 60 },
            { key: 'DocDate', caption: 'DocDate', type: 'date', width: 90 },
            { key: 'StockParty', caption: 'StockParty', width: 160 },
            { key: 'ReferenceParty', caption: 'ReferenceParty', width: 150 },
            { key: 'GpSrNo', caption: 'GpSrNo', width: 60 },
            { key: 'WareHouse', caption: 'WareHouse', width: 120 },
            { key: 'ItemName', caption: 'ItemName', width: 160 },
            { key: 'CropYear', caption: 'CropYear', width: 70 },
            { key: 'JobLot', caption: 'JobLot', width: 100 },
            { key: 'PackingType', caption: 'PackingType', width: 90 },
            { key: 'PackUom', caption: 'PackUom', width: 70 },
            { key: 'ItemQty', caption: 'ItemQty', width: 80, sum: true, render: n3, align: 'right' },
            { key: 'StockWeight', caption: 'StockWeight', width: 100, sum: true, render: n3, align: 'right' },
            { key: 'EBWPerUnit', caption: 'EBWPerUnit', width: 80, render: n3, align: 'right' },
            { key: 'EBWTotal', caption: 'EBWTotal', width: 80, sum: true, render: n3, align: 'right' },
            { key: 'AdLsWeight', caption: 'AdLsWeight', width: 80, sum: true, render: n3, align: 'right' },
            { key: 'NetBillWeight', caption: 'NetBillWeight', width: 100, sum: true, render: n3, align: 'right' },
            { key: 'GrossWeight', caption: 'GrossWeight', width: 100, sum: true, render: n3, align: 'right' },
            { key: 'Transporter', caption: 'Transporter', width: 120 },
            { key: 'VehicleNo', caption: 'VehicleNo', width: 90 },
            { key: 'BiltyNo', caption: 'BiltyNo', width: 80 },
            { key: 'CityName', caption: 'CityName', width: 90 },
            { key: 'Freight', caption: 'Freight', type: 'num', decimals: 2, width: 90, sum: true },
            { key: 'RemarksHeader', caption: 'RemarksHeader', width: 160 },
            { key: 'Comments', caption: 'Comments', width: 160 }
        ],
        filterRow: true, totals: true
    });

    function combos(d) {
        HRM.fill('CmbStockParty', d.stockParties || [], 'Id', 'name', { zero: '', keep: true });
        HRM.fill('cmbSupplierName', d.refParties || [], 'Id', 'name', { zero: '', keep: true });
        HRM.fill('CmbItemName', d.items || [], 'Id', 'name', { zero: '', keep: true });
    }

    function args() {
        return {
            documentTypeId: documentTypeId,
            fromDate: HRM.val('txtFromDate') ? HRM.val('txtFromDate') + 'T' + fromTime : '',
            toDate: HRM.val('txtToDate') ? HRM.val('txtToDate') + 'T' + toTime : '',
            docNoFrom: HRM.int(HRM.val('txtFromDocNo')), docNoTo: HRM.int(HRM.val('txtToDocNo')),
            supplierCustomerId: HRM.comboVal('cmbSupplierName'), stockPartyId: HRM.comboVal('CmbStockParty'), itemId: HRM.comboVal('CmbItemName')
        };
    }

    /** GridFill(): GrnAndGdnSlipAndRegister rows -> grid (none -> empty). They back the register prints. */
    function GridFill(btn) {
        var a = args();
        return HRM.busy(btn || 'btnshow', function () {
            return HRM.post(API + '/search?mode=' + MODE, a).then(function (rows) {
                lastRows = rows || []; lastArgs = a;
                grd.set(lastRows);
                HRM.text('lblStatus', lastRows.length + ' record(s)');
            }).catch(HRM.fail);
        });
    }

    function dateType(v) {
        var t = HRM.today();
        if (v === 1) { HRM.setVal('txtFromDate', t); fromTime = '00:00:00'; }
        else if (v === 2) { HRM.setVal('txtFromDate', HRM.addDays(t, -7)); fromTime = '00:00:00'; }
        else if (v === 3) { var u = new Date(); HRM.setVal('txtFromDate', u.getUTCFullYear() + '-' + String(u.getUTCMonth() + 1).padStart(2, '0') + '-01'); fromTime = '00:00:00'; HRM.setVal('txtToDate', t); toTime = '00:00:00'; }
        else if (v === 4) { HRM.setVal('txtFromDate', new Date().getFullYear() + '-01-01'); fromTime = '00:00:00'; HRM.setVal('txtToDate', t); toTime = '00:00:00'; }
        else if (v === 5) { if (fyStart) { HRM.setVal('txtFromDate', fyStart); fromTime = '00:00:00'; } }
        else return Promise.resolve();
        return GridFill();
    }
    HRM.$('cmbDateType').addEventListener('change', function () { dateType(HRM.comboVal('cmbDateType')); });

    function Datetypefill() {
        HRM.fill('cmbDateType', dateTypes, 'Id', 'Parameters', { zero: '' });
        HRM.setCombo('cmbDateType', 2);
        return dateType(2);
    }

    P.btnshow = function (btn) { return GridFill(btn); };
    /** btnReset_Click: doc numbers, parties and item cleared, Datetypefill (which searches again). */
    P.btnReset = function () {
        HRM.setVal('txtFromDocNo', ''); HRM.setVal('txtToDocNo', '');
        ['cmbSupplierName', 'CmbStockParty', 'CmbItemName'].forEach(function (id) { HRM.setCombo(id, 0); });
        Datetypefill();
        HRM.focus('cmbDateType');
    };
    P.btnRefresh = function (btn) {
        return HRM.busy(btn || 'btnRefresh', function () { return HRM.get(API + '/combos', { mode: MODE }).then(combos).catch(HRM.fail); });
    };
    /** GenerateRegister(dtGrid, rpt): the last Show's rows, else "Not Record Found For Display". */
    P.print = function (btn, key) {
        if (!lastRows.length) { HRM.box('Not Record Found For Display'); return; }
        return CrystalPrint.open(key, lastArgs, btn);
    };
    P.exportGrid = function () { exportGrid('grdfrm', MODE === 'gdn' ? 'GdnPartyProcessing' : 'GrnPartyProcessing'); };
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
    HRM.keys({                                                  // frmGRNGDNPartyProcessing_KeyDown
        'ctrl+p': function () { P.print(HRM.$('btnPrint'), 'pp-332'); },
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

    /** frmGatePassReport_Load: Tag -> DocumentTypeId, Datetypefill ("This Week" -> search), ComboFill. */
    HRM.setVal('txtFromDate', HRM.today());
    HRM.setVal('txtToDate', HRM.today());
    ['CmbStockParty', 'cmbSupplierName', 'CmbItemName'].forEach(function (id) { HRM.fill(id, [], 'Id', 'name', { zero: '' }); });
    HRM.loading(HRM.get(API + '/setup', { mode: MODE })).then(function (d) {
        fyStart = d.financialYearStart || null;
        documentTypeId = HRM.int(d.documentTypeId) || documentTypeId;
        dateTypes = d.dateTypes || [];
        var p = Datetypefill();
        combos(d);
        HRM.focus('cmbDateType');
        return p;
    }).catch(HRM.fail);
})();
