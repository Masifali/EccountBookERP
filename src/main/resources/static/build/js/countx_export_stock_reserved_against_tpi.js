/* ============================================================================================
 * countx_export_stock_reserved_against_tpi.js - frmStockReservedAgainstThirdPartyInspection.cs
 * (Architecture.WinApp.Export) "Stock Reserved Against third Party Analysis", pop-up of 857 / 858.
 * Data from /api/export/stock-reserved-against-tpi.
 *
 * Desktop behaviour kept: Ctrl+S runs the SEARCH (not the save); editing ItemQty recomputes NetWeight
 * pro-rata (BalanceWeight / BalanceQty x qty) with the desktop's two refusals; editing NetWeight raises
 * "Object reference not set to an instance of an object." (the handler reads columns "BalanceNetWeight" and
 * "Rate" that the grid does not have) and keeps the typed weight; the selected totals are
 * Math.Round(sum).ToString("0,0") (banker's rounding, at least two digits); Save needs a checked row and a
 * Third Party Analysis No, writes one InventoryStockReserved per checked row (RefDocumentTypeId 204) and then
 * resets (which searches again). Stock Reserved Register is a separate desktop report form (not ported).
 * ?trackingId= preselects the Third Party Analysis No.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var Q = global.ExportQ;
    var API = '/api/export/stock-reserved-against-tpi';
    var box = Q.box, netI = Q.netI, netD = Q.netD, str = Q.str, esc = Q.esc, $id = Q.$id;

    var S = { rows: [], checked: {}, cur: -1, yearStart: '' };
    /* dtGrid's visible columns, in order (the hidden ones stay in the row objects). */
    var COLS = [
        { key: 'RefDocumentType' }, { key: 'DocDate', fmt: 'date' }, { key: 'DocCodeNo', fmt: 'int', link: true }, { key: 'ManualNo' },
        { key: 'GrnNo', fmt: 'int' }, { key: 'PartyName' }, { key: 'GpNo' }, { key: 'VehicleNo' }, { key: 'BiltyNo' }, { key: 'Remarks', edit: true },
        { key: 'WareHouse' }, { key: 'ItemName' }, { key: 'ItemCode' }, { key: 'CropYear' }, { key: 'JobLot' }, { key: 'PackingType' }, { key: 'PackUom' },
        { key: 'ItemQty', fmt: 'num3', sum: true, edit: true }, { key: 'NetWeight', fmt: 'num3', sum: true, edit: true },
        { key: 'QtyIn', fmt: 'num3', sum: true }, { key: 'QtyOut', fmt: 'num3', sum: true }, { key: 'BalanceQty', fmt: 'num3', sum: true },
        { key: 'WeightIn', fmt: 'num3', sum: true }, { key: 'WeightOut', fmt: 'num3', sum: true }, { key: 'BalanceWeight', fmt: 'num3', sum: true }];

    function isNum(c) { return /^(int|num3)$/.test(c.fmt || ''); }
    function render() {
        var body = $id('grdBody');
        body.innerHTML = S.rows.map(function (r, i) {
            var on = !!S.checked[i];
            var h = '<tr data-i="' + i + '" class="' + (on ? 'exq-checked' : '') + (i === S.cur ? ' is-current' : '') + '">' +
                '<td class="exq-sel"><input type="checkbox" class="exf-cell-chk" data-i="' + i + '" data-k="Select"' + (on ? ' checked' : '') + '/></td>';
            COLS.forEach(function (c) {
                var v = r[c.key], inner;
                if (c.edit) inner = '<input class="exf-cell' + (isNum(c) ? ' num' : '') + '" data-i="' + i + '" data-k="' + c.key + '" value="' + esc(isNum(c) ? str(netD(v)) : str(v)) + '"/>';
                else {
                    var t = Q.cellText(c, v);
                    inner = c.link && t !== '' ? '<a class="win-code" data-i="' + i + '" href="javascript:void(0)">' + esc(t) + '</a>' : esc(t);
                }
                h += '<td' + (isNum(c) ? ' class="num"' : '') + '>' + inner + '</td>';
            });
            return h + '</tr>';
        }).join('');
        renderFoot();
        Q.show('grdEmpty', S.rows.length === 0);
        $id('chkSelectAll').checked = S.rows.length > 0 && S.rows.every(function (r, i) { return !!S.checked[i]; });
    }
    function renderFoot() {
        var foot = $id('grdFoot');
        if (!S.rows.length) { foot.innerHTML = ''; return; }
        var t = '<tr><td class="lbl">&Sigma;</td>';
        COLS.forEach(function (c) {
            if (!c.sum) { t += '<td class="lbl"></td>'; return; }
            var s = 0; S.rows.forEach(function (r) { s += netD(r[c.key]); });
            t += '<td>' + esc(Q.cellText(c, s)) + '</td>';
        });
        foot.innerHTML = t + '</tr>';
    }
    function setCell(i, key, v) {
        S.rows[i][key] = v;
        var inp = $id('grdBody').querySelector('input.exf-cell[data-i="' + i + '"][data-k="' + key + '"]');
        if (inp) inp.value = str(v);
    }

    /* Math.Round (MidpointRounding.ToEven) then ToString("0,0"). */
    function roundEven(n) {
        var f = Math.floor(n), d = n - f;
        if (Math.abs(d - 0.5) < 1e-9) return (f % 2 === 0) ? f : f + 1;
        return Math.round(n);
    }
    function fmt00(n) {
        var r = roundEven(n), neg = r < 0, s = String(Math.abs(r));
        if (s.length < 2) s = '0' + s;
        return (neg ? '-' : '') + s.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    }
    /* SelectedWeightCalculation. */
    function selectedTotals() {
        var w = 0, q = 0;
        S.rows.forEach(function (r, i) { if (S.checked[i]) { w += netD(r.NetWeight); q += netD(r.ItemQty); } });
        Q.setText('txtSelectedStock', fmt00(w));
        Q.setText('txtSelectedQty', fmt00(q));
    }

    /* StockComboFill / TackingNoBind. */
    function applyCombos(d) {
        Q.bind('CmbItemParentCategory', d.parentCategories, 'Id', 'name', []);
        Q.bind('CmbJobLot', d.jobLots, 'Id', 'name', []);
        Q.bind('CmbCropYear', d.cropYears, 'Id', 'name', []);
        Q.bind('CmbWarehouse', d.warehouses, 'Id', 'name', []);
        Q.bind('CmbReferenceDocument', d.documentTypes, 'Id', 'name', []);
        Q.bind('CmbSupplier', d.parties, 'Id', 'name', []);
        Q.bind('CmbItemName', d.items, 'Id', 'name', []);
        Q.bind('CmbLotTrackingNo', d.trackingNos, 'Id', 'name', []);
    }

    /* InitializeComponentCustom + InitializeComponentMethod. */
    function load() {
        Q.setText('Todate', Q.today());
        return Q.getJson(API + '/setup').then(function (d) {
            d = d || {};
            S.yearStart = d.yearStart || '';
            Q.setText('FromDate', S.yearStart || Q.today());
            if (d.setupError) { box(d.setupError); return; }
            applyCombos(d);
            var t = netI(Q.qs('trackingId'));
            if (t > 0) Q.setVal('CmbLotTrackingNo', t);
            Q.focus('FromDate');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }

    function filters() {
        var crop = Q.hasSel('CmbCropYear') ? Q.selText('CmbCropYear') : '';
        return {
            fromDate: Q.val('FromDate'), toDate: Q.val('Todate'),
            supplierId: netI(Q.val('CmbSupplier')), itemId: netI(Q.val('CmbItemName')), refDocumentTypeId: netI(Q.val('CmbReferenceDocument')),
            jobLotId: netI(Q.val('CmbJobLot')), warehouseId: netI(Q.val('CmbWarehouse')), parentCategoryId: netI(Q.val('CmbItemParentCategory')),
            cropYearText: crop
        };
    }
    /* PendingTransactions: rows -> grid; none -> grid cleared. */
    function pending() {
        return Q.postJson(API + '/search', filters()).then(function (rows) {
            S.rows = rows || []; S.checked = {}; S.cur = -1;
            render();
        }).catch(function (e) { box(e.message); });
    }
    /* btnShow_Click ("Search"). */
    function search(btn) { return Q.busy(btn, pending); }

    /* grd_CellUpdated. */
    function cellUpdated(i, key, value) {
        var r = S.rows[i]; if (!r) return;
        if (key === 'Select') { if (value) S.checked[i] = true; else delete S.checked[i]; render(); selectedTotals(); return; }
        if (key === 'Remarks') { r.Remarks = value; return; }
        if (key === 'ItemQty') {
            r.ItemQty = netD(value);
            var qtyOut = netD(r.ItemQty), balanceQty = netD(r.BalanceQty), balanceNetWeight = netD(r.BalanceWeight);
            if (balanceQty > 0 && qtyOut > balanceQty) {
                box('Qty cannot be greater than BalanceQty. Please check!');
                setCell(i, 'ItemQty', 0); setCell(i, 'NetWeight', 0);
                renderFoot(); selectedTotals();
                return;
            }
            var nw = balanceNetWeight / balanceQty * qtyOut;
            if (!isFinite(nw)) { box('Value was either too large or too small for a Decimal.'); renderFoot(); selectedTotals(); return; }
            if (nw > balanceNetWeight) {
                box('NetWeight cannot be greater than BalanceNetWeight. Please check!');
                setCell(i, 'NetWeight', 0);
                renderFoot(); selectedTotals();
                return;
            }
            setCell(i, 'NetWeight', nw);
            renderFoot(); selectedTotals();
            return;
        }
        if (key === 'NetWeight') {
            r.NetWeight = netD(value);
            renderFoot(); selectedTotals();   /* grd_CellValueChanged */
            box('Object reference not set to an instance of an object.');
        }
    }

    /* btnsave_Click. */
    function save(btn) {
        return Q.busy(btn, function () {
            var rows = S.rows.filter(function (r, i) { return !!S.checked[i]; });
            if (rows.length === 0) { box('Please select at least one record to save.'); return Promise.resolve(); }
            if (netI(Q.val('CmbLotTrackingNo')) === 0) { Q.focus('CmbLotTrackingNo'); box('Please select a third party analysis.'); return Promise.resolve(); }
            for (var i = 0; i < rows.length; i++) {
                var q = netD(rows[i].ItemQty), w = netD(rows[i].NetWeight);
                if (q <= 0 && w <= 0) { box('ItemQty or NetWeight must be greater than zero for selected records.'); return Promise.resolve(); }
                if (q > netD(rows[i].BalanceQty)) { box('ItemQty cannot be greater than BalanceQty. Please check!'); return Promise.resolve(); }
                if (w > netD(rows[i].BalanceWeight)) { box('NetWeight cannot be greater than BalanceWeight. Please check!'); return Promise.resolve(); }
            }
            return Q.postJson(API + '/save', { trackingId: netI(Q.val('CmbLotTrackingNo')), rows: rows }).then(function (r) {
                box((r && r.message) || 'Allocated Successfully');
                return reset();
            });
        });
    }
    /* btnNew_Click (Reset): party / item cleared, totals "0", grid cleared, PendingTransactions, focus From Date. */
    function reset(btn) {
        var run = function () {
            Q.setVal('CmbSupplier', '0'); Q.setVal('CmbItemName', '0');
            Q.setText('txtSelectedStock', '0'); Q.setText('txtSelectedQty', '0');
            S.rows = []; S.checked = {}; render();
            return pending().then(function () { Q.focus('FromDate'); });
        };
        return btn ? Q.busy(btn, run) : run();
    }
    /* btnRefresh_Click. */
    function refresh(btn) {
        var run = function () { return Q.getJson(API + '/refresh').then(function (d) { applyCombos(d || {}); }); };
        return btn ? Q.busy(btn, run) : run().catch(function (e) { box(e.message); });
    }
    /* BtnStockReservedRegisterForm_Click: frmStockReservedRegister (Architecture.WinApp.ExportReports) is not part of this port. */
    function register() { box('Stock Reserved Register (frmStockReservedRegister) is a desktop report form that is not available on the web yet.'); }
    function shortcuts() {
        Q.shortcutPopup([['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
            ['Ctrl+F5', 'For Focus On DateType '], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
            ['Ctrl+ArrowUp', 'For Focus On DateType in Filter'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }
    /* Footer History: the available-stock list for the current filters (the form's only grid), brought into view. */
    function history(btn) {
        return Q.busy(btn, function () { return pending().then(function () { var b = $id('gridBar'); if (b) b.scrollIntoView({ behavior: 'smooth' }); }); });
    }

    document.addEventListener('DOMContentLoaded', function () {
        Q.wireTabs(null, null);
        Q.wireGrid('grdBody', {
            select: function (i) { S.cur = i; },
            cell: cellUpdated,
            open: function (i) { var c = $id('grdBody').querySelector('input.exf-cell-chk[data-i="' + i + '"]'); if (c) { c.checked = !c.checked; cellUpdated(i, 'Select', c.checked); } },
            link: function (k, i) { var c = $id('grdBody').querySelector('input.exf-cell-chk[data-i="' + i + '"]'); if (c) { c.checked = !c.checked; cellUpdated(i, 'Select', c.checked); } }
        });
        /* grd_ColumnHeaderClick on the select column: check / uncheck all, then the totals. */
        Q.on('chkSelectAll', 'change', function () {
            var on = $id('chkSelectAll').checked;
            S.checked = {}; if (on) S.rows.forEach(function (r, i) { S.checked[i] = true; });
            render(); selectedTotals();
        });
        /* frmStockReservedAgainstThirdPartyInspection_KeyDown. */
        document.addEventListener('keydown', function (e) {
            if (e.target && e.target.classList && e.target.classList.contains('exf-cell') && e.key === 'Enter') { e.target.blur(); return; }
            if (Q.enterMoves(e)) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 's') { e.preventDefault(); search($id('btngrnlod')); }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh(); }
            if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); Q.focus('FromDate'); }
            if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); $id('grd').focus(); }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); Q.cancel(); }
            if (e.ctrlKey && e.altKey && (k === 'control' || k === 'alt')) shortcuts();
        });
        load();
    });

    global.ExportStockReservedTpi = { reset: reset, refresh: refresh, save: save, search: search, register: register, shortcuts: shortcuts, history: history };
}(window));
