/* ============================================================================================
 * countx_pp_stock_report.js - 694 Stock Report (Party Processing)
 * Architecture.WinApp.PartyProcessingReports.frmStockReportPartyProcessing. Built on countx_hrm.js (window.HRM).
 * API /api/party-processing/reports/stock-report/{setup|combos|search}
 * Prints: pp-466_01 .. pp-466_06 (one per report type) with the last Show's filters.
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/party-processing/reports/stock-report';
    var P = {};
    window.PpStockRpt = P;
    var fyStart = null, dateTypes = [], lastRows = [], lastArgs = null, lastType = '', grd = null;
    var fromTime = HRM.nowTime() + ':00', toTime = fromTime;     // DateTimePicker.Value keeps its time of day

    HRM.close = function () {
        window.close();
        setTimeout(function () { if (!window.closed) { if (history.length > 1) history.back(); else location.href = '/party-processing'; } }, 150);
    };

    /** PrintButtonManage(): the template of each report type. */
    var PRINTS = {
        ItemStockSummary: ['pp-466_01', '466_01-ItemStockSummaryPartyProcessing'],
        ItemandWarehouseStockSummary: ['pp-466_02', '466_02-ItemandWarehouseStockSummaryPartyProcessing'],
        ItemandCropYearStockSummary: ['pp-466_03', '466_03-ItemandCropYearStockSummaryPartyProcessing'],
        ItemandCropYearandWarehouseStockSummary: ['pp-466_05', '466_05-ItemandCropYearandWarehouseStockSummaryPartyProcessing'],
        JobLotandItemStockSummary: ['pp-466_04', '466_04-JobLotandItemStockSummaryPartyProcessing'],
        WarehouseandJoblotandItemStockSummary: ['pp-466_06', '466_06-WarehouseandJoblotandItemStockSummaryStockSummaryPartyProcessing']
    };
    /** The form's DataTable per report type (key columns before the eight quantity / weight columns) and its GridEX groups. */
    var LAYOUT = {
        ItemStockSummary: { cols: ['StockParty', 'ItemName', 'PackUom', 'PackingType'], groups: ['StockParty'], bill: true },
        ItemandWarehouseStockSummary: { cols: ['StockParty', 'ItemName', 'WareHouseName', 'PackUom', 'PackingType'], groups: ['ItemName'] },
        ItemandCropYearStockSummary: { cols: ['StockParty', 'ItemName', 'CropYear', 'PackUom', 'PackingType'], groups: ['CropYear'] },
        JobLotandItemStockSummary: { cols: ['StockParty', 'ItemName', 'JobLot', 'PackUom', 'PackingType'], groups: ['JobLot'] },
        ItemandCropYearandWarehouseStockSummary: { cols: ['StockParty', 'ItemName', 'WareHouseName', 'CropYear', 'PackUom', 'PackingType'], groups: ['CropYear', 'WareHouseName'] },
        WarehouseandJoblotandItemStockSummary: { cols: ['StockParty', 'ItemName', 'WareHouseName', 'JobLot', 'PackUom', 'PackingType'], groups: ['WareHouseName', 'JobLot'] }
    };
    var NUMS = ['OpQty', 'QtyIn', 'QtyOut', 'BalQty', 'OpWeight', 'WeightIn', 'WeightOut', 'BalWeight'];
    var BILL = ['OpBillWeight', 'BillWeightIn', 'BillWeightOut', 'BalBillWeight'];

    function n0(v) { return v === null || v === undefined || v === '' ? '' : HRM.esc(HRM.fmtNum(v, 0)); }

    /** GridSettings(): the layout of the report type; groups hidden, group totals, Bill Weight columns per the tick. */
    function render() {
        var old = HRM.$('grdfrm'), t = old.cloneNode(false);
        old.parentNode.replaceChild(t, old);
        var L = LAYOUT[lastType];
        if (!L || !lastRows.length) { t.innerHTML = ''; grd = null; return; }
        var cols = [{ key: 'ItemId', hidden: true }];
        var firstShown = null;
        L.cols.forEach(function (k) {
            var hidden = L.groups.indexOf(k) >= 0;
            var c = { key: k, caption: k, width: k === 'ItemName' ? 200 : 110, hidden: hidden };
            if (k === 'ItemName') c.cls = 'pp-link';
            cols.push(c);
            if (!hidden && !firstShown) firstShown = c;
        });
        NUMS.concat(L.bill ? BILL : []).forEach(function (k) {
            cols.push({ key: k, caption: k, width: 90, sum: true, decimals: 2, align: 'right', hidden: BILL.indexOf(k) >= 0 && !HRM.checked('chkBillWeightColumnsShow'),
                render: function (v, r) { return r._gt ? '<b>' + HRM.esc(HRM.fmtNum(r._t[k], 2).replace(/\.?0+$/, '')) + '</b>' : n0(v); } });
        });
        cols.forEach(function (c) {
            if (c !== firstShown) return;
            c.render = function (v, r) {
                if (r._g) return '<b>' + HRM.esc(r._g) + '</b>';
                if (r._gt) return '<i>Total</i>';
                return c.key === 'ItemName' ? '<a href="#" class="pp-item-link">' + HRM.esc(v) + '</a>' : HRM.esc(v);
            };
        });
        grd = new HRM.Grid(t, { columns: cols, filterRow: true, totals: true,
            rowClass: function (r) { return r._g ? 'pp-group' : (r._gt ? 'pp-group-total' : ''); } });
        grd.set(grouped(lastRows, L.groups));
    }

    /** The rows sorted by their group keys with a caption row before and a total row after each group. */
    function grouped(rows, groups) {
        if (!groups.length) return rows.slice();
        var keyOf = function (r) { return groups.map(function (g) { return HRM.str(r[g]); }).join('\u0001'); };
        var order = [], byKey = {};
        rows.forEach(function (r) { var k = keyOf(r); if (!byKey[k]) { byKey[k] = []; order.push(k); } byKey[k].push(r); });
        order.sort();
        var out = [], nums = NUMS.concat(BILL);
        order.forEach(function (k) {
            var list = byKey[k], label = groups.map(function (g) { return g + ': ' + HRM.str(list[0][g]); }).join('  /  ');
            out.push({ _g: label });
            var tot = {};
            nums.forEach(function (n) { tot[n] = 0; });
            list.forEach(function (r) { out.push(r); nums.forEach(function (n) { tot[n] += HRM.num(r[n]); }); });
            out.push({ _gt: 1, _t: tot });
        });
        return out;
    }

    /** grdfrm_LinkClicked(ItemName): opens InventoryTransactionPartyProcessingReport - no web page yet. */
    HRM.$('grdWrap').addEventListener('click', function (e) {
        var a = e.target.closest('a.pp-item-link'); if (!a) return;
        e.preventDefault();
        HRM.box('The Inventory Transaction (Party Processing) report is not available on the web yet.');
    });

    function combos(d) {
        HRM.fill('cmbItemType', d.itemTypes || [], 'Id', 'name', { zero: '', keep: true });
        HRM.fill('cmbCategory', d.categories || [], 'Id', 'name', { zero: '', keep: true });
        HRM.fill('cmbItem', d.items || [], 'Id', 'name', { zero: '', keep: true });
        HRM.fill('cmbWareHouse', d.warehouses || [], 'Id', 'name', { zero: '', keep: true });
        HRM.fill('cmbJobLot', d.jobLots || [], 'Id', 'name', { zero: '', keep: true });
        HRM.fill('cmbCropYear', d.cropYears || [], 'Id', 'name', { zero: '', keep: true });
        HRM.fill('CmbStockParty', d.stockParties || [], 'Id', 'name', { zero: '', keep: true });
    }

    function reportType() { var s = HRM.$('cmbReportType'); return s && s.value && s.value !== '0' ? s.value : ''; }

    /** PrintButtonManage(). */
    function PrintButtonManage() {
        var t = reportType();
        HRM.show('Register', !!t);
        if (t && PRINTS[t]) HRM.$('RegisterText').textContent = PRINTS[t][1];
    }

    function args() {
        var itemWise = HRM.checked('ChkItemWiseStock');
        return {
            fromDate: HRM.val('txtDateFrom') ? HRM.val('txtDateFrom') + 'T' + fromTime : '',
            toDate: HRM.val('txtDateTo') ? HRM.val('txtDateTo') + 'T' + toTime : '',
            activity: reportType(), itemTypeId: HRM.comboVal('cmbItemType'), itemCategoryId: HRM.comboVal('cmbCategory'),
            itemId: HRM.comboVal('cmbItem'), warehouseId: HRM.comboVal('cmbWareHouse'), jobLotId: HRM.comboVal('cmbJobLot'),
            stockPartyId: HRM.comboVal('CmbStockParty'), itemWise: itemWise, isPackSizeOn: itemWise ? 0 : 1, isPackTypeOn: itemWise ? 0 : 1,
            skipZero: HRM.checked('chkSikeZero') ? 1 : 0, cropYear: HRM.comboText('cmbCropYear')
        };
    }

    /** GridBind() + PrintButtonManage(). */
    function GridBind(btn) {
        var a = args();
        return HRM.busy(btn || 'btnshow', function () {
            return HRM.post(API + '/search', a).then(function (rows) {
                lastRows = rows || []; lastArgs = a; lastType = a.activity;
                render();
                HRM.text('lblStatus', lastRows.length + ' record(s)');
                PrintButtonManage();
            }).catch(HRM.fail);
        });
    }

    /** cmbDateType_ValueChanged: the range only (this form does not search on it). */
    function dateType(v) {
        var t = HRM.today();
        if (v === 1) { HRM.setVal('txtDateFrom', t); fromTime = '00:00:00'; }
        else if (v === 2) { HRM.setVal('txtDateFrom', HRM.addDays(t, -7)); fromTime = '00:00:00'; }
        else if (v === 3) { var u = new Date(); HRM.setVal('txtDateFrom', u.getUTCFullYear() + '-' + String(u.getUTCMonth() + 1).padStart(2, '0') + '-01'); fromTime = '00:00:00'; HRM.setVal('txtDateTo', t); toTime = '00:00:00'; }
        else if (v === 4) { HRM.setVal('txtDateFrom', new Date().getFullYear() + '-01-01'); fromTime = '00:00:00'; HRM.setVal('txtDateTo', t); toTime = '00:00:00'; }
        else if (v === 5) { if (fyStart) { HRM.setVal('txtDateFrom', fyStart); fromTime = '00:00:00'; } }
    }
    HRM.$('cmbDateType').addEventListener('change', function () { dateType(HRM.comboVal('cmbDateType')); });
    HRM.$('chkBillWeightColumnsShow').addEventListener('change', function () { render(); });   // chkBillWeightColumnsShow_CheckedChanged

    function Datetypefill() {
        HRM.fill('cmbDateType', dateTypes, 'Id', 'Parameters', { zero: '' });
        HRM.setCombo('cmbDateType', 2);
        dateType(2);
    }

    P.btnshow = function (btn) { return GridBind(btn); };
    /** btnReset_Click: every filter combo (and the report type) cleared, Datetypefill. */
    P.btnReset = function () {
        ['cmbCategory', 'cmbCropYear', 'cmbItem', 'cmbItemType', 'cmbJobLot', 'cmbReportType', 'CmbStockParty', 'cmbWareHouse'].forEach(function (id) { HRM.setCombo(id, 0); });
        Datetypefill();
        HRM.focus('cmbDateType');
    };
    P.btnRefresh = function () {
        return HRM.busy('btnRefresh', function () { return HRM.get(API + '/combos').then(combos).catch(HRM.fail); });
    };
    /** Register_Click: the template of the last Show's report type; no rows -> "Record Not Found For Display". */
    P.Register = function (btn) {
        var p = PRINTS[lastType];
        if (!p) return;
        if (!lastRows.length) { HRM.box('Record Not Found For Display'); return; }
        return CrystalPrint.open(p[0], lastArgs, btn || 'Register');
    };
    P.exportGrid = function () { exportGrid('grdfrm', 'StockReportPartyProcessing'); };
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
    HRM.keys({                                                  // frmStockReportPartyProcessing_KeyDown
        'ctrl+p': function () { P.Register(); },
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
    HRM.footer(function (b) { return GridBind(b).then(function () { HRM.$('historyBox').scrollIntoView({ block: 'start', behavior: 'smooth' }); }); });

    /** frmStockReportPartyProcessing_Load: Datetypefill, ComboFill, ReportTypeFill, first report type, GridBind, PrintButtonManage. */
    HRM.setVal('txtDateFrom', HRM.today());
    HRM.setVal('txtDateTo', HRM.today());
    HRM.loading(HRM.get(API + '/setup')).then(function (d) {
        fyStart = d.financialYearStart || null;
        dateTypes = d.dateTypes || [];
        Datetypefill();
        combos(d);
        HRM.fill('cmbReportType', d.reportTypes || [], 'Id', 'name', { zero: '' });
        HRM.$('cmbReportType').value = 'ItemStockSummary'; HRM.refreshCombos();
        HRM.focus('cmbDateType');
        PrintButtonManage();
        return GridBind();
    }).catch(HRM.fail);
})();
