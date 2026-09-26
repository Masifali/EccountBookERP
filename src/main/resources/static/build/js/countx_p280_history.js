/* ============================================================================================
 * Screen 280 - the Transaction History tab page of FoodProductionWithValues.cs
 *   jobOrderTransactionHistoryCombobind:931, GridHistory:998, GridSetting:1060,
 *   DatagridHistory_ColumnButtonClick:1089, DatagridHistoryDetail:1130, btnNew/Refresh/Search:1257-1298
 * ============================================================================================ */
(function (global) {
    'use strict';

    var P = global.P280, K = global.ReportKit, doc = global.document;
    var $ = P.$, esc = P.esc, box = P.box;
    var TH = P.api + '/transaction-history';
    var C = P.api + '/consumption';

    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function today() { var d = new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function addDays(ymd, n) { var p = ymd.split('-'), d = new Date(+p[0], +p[1] - 1, +p[2] + n); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    var pickTime = (function () { var d = new Date(); return pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds()); }());
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    function num(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function toInt(v) { var n = parseInt(String(v === null || v === undefined ? '' : v), 10); return isNaN(n) ? 0 : n; }
    function ci(row, name) {
        if (!row) return undefined;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return undefined;
    }
    /* "#,#" - whole number, grouped, zero shows nothing. */
    function hash0(v) { var n = Math.round(num(v)); return n === 0 ? '' : K.num(n, 0); }
    function fill(id, rows, v, t) {
        var s = $(id);
        s.innerHTML = '<option value=""></option>' + rows.map(function (r) {
            return '<option value="' + esc(r[v]) + '">' + esc(r[t]) + '</option>';
        }).join('');
        s.selectedIndex = 0;
    }
    function clearList(id) { $(id).innerHTML = '<option value=""></option>'; }
    function selVal(id) { return toInt($(id).value); }

    var T = { rows: [], cur: -1, hidden: {} };
    var COLS = ['Id', 'DocSrNo', 'DocDate', 'JobOrderNo', 'EntryType', 'Remarks', 'JobOrderStatus', 'StartDate', 'EndDate',
                'EntryDate', 'EntryUser', 'ModifyDate', 'ModifyUser', 'PlanStatus', 'WagesId'];

    /** jobOrderTransactionHistoryCombobind:931 - DocumentTypeIds "80,112", split on Activity. */
    function combobind() {
        return P.getJson(TH + '/combos').then(function (rows) {
            rows = rows || [];
            if (!rows.length) { clearList('CmbPlantTransactionHistory'); return; }
            var job = [], plant = [];
            rows.forEach(function (r) {
                var a = str(ci(r, 'Activity'));
                if (a === 'JobOrder') job.push({ Id: ci(r, 'Id'), JobOrder: ci(r, 'ReferenceName') });
                else if (a === 'Plant') plant.push({ Id: ci(r, 'Id'), Plant: ci(r, 'ReferenceName') });
            });
            if (job.length) fill('CmbJobOrderTransactionHistory', job, 'Id', 'JobOrder'); else clearList('CmbJobOrderTransactionHistory');
            if (plant.length) fill('CmbPlantTransactionHistory', plant, 'Id', 'Plant'); else clearList('CmbPlantTransactionHistory');
        }).catch(function (e) { box(e.message); });
    }

    /** GridHistory:998 - a date is sent only when its check box is ticked. */
    function search() {
        return P.busy('btnSearchTransactionHistory', function () {
            return P.getJson(TH + P.qs({
                from: $('chkFromDateTransactionHistory').checked && $('txtfromDateTransactionHistory').value ? $('txtfromDateTransactionHistory').value + 'T' + pickTime : '',
                to: $('chkToDateTransactionHistory').checked && $('txtToDateTransactionHistory').value ? $('txtToDateTransactionHistory').value + 'T' + pickTime : '',
                docFrom: $('txtDocNoFrom').value, docTo: $('txtDocNumberTo').value,
                jobOrderId: selVal('CmbJobOrderTransactionHistory'), plantId: selVal('CmbPlantTransactionHistory')
            })).then(function (rows) {
                if (!rows || !rows.length) { T.rows = []; $('DatagridHistoryHost').innerHTML = ''; return; }
                T.rows = rows.map(function (x) {
                    return { Id: ci(x, 'Id'), DocSrNo: ci(x, 'DocCode'), DocDate: ci(x, 'DocDate'), JobOrderNo: ci(x, 'InvJobOrderNo'),
                        EntryType: ci(x, 'EntryType'), Remarks: ci(x, 'MainRemarks'), JobOrderStatus: ci(x, 'JobOrderApprovedStatus'),
                        StartDate: ci(x, 'StartDate'), EndDate: ci(x, 'EndDate'), EntryDate: ci(x, 'EntryDate'),
                        EntryUser: ci(x, 'EntryUser'), ModifyDate: ci(x, 'ModifyDate'), ModifyUser: ci(x, 'ModifyUser'),
                        PlanStatus: ci(x, 'PlanStatus'), WagesId: ci(x, 'WagesId') };
                });
                T.cur = -1;
                render();
            }).catch(function (e) { box(e.message); });
        });
    }

    function visible(c) {
        if (Object.prototype.hasOwnProperty.call(T.hidden, c)) return !T.hidden[c];
        return c !== 'Id' && c !== 'WagesId';
    }
    function cell(c, v) {
        if (c === 'DocDate' || c === 'StartDate' || c === 'EndDate') return v ? K.dMMMyyyy(v) : '';
        if (c === 'EntryDate' || c === 'ModifyDate') return v ? K.dmyhm(v) : '';
        return str(v);
    }
    /** GridSetting:1060 - Edit and WagesPrint are appended after the data columns. */
    function render() {
        var cols = COLS.filter(visible);
        var h = '<table class="gx alt" id="DatagridHistory"><thead><tr>';
        cols.forEach(function (c) { h += '<th>' + esc(c) + '</th>'; });
        h += '<th style="width:40px;">Edit</th><th style="width:60px;">WagesPrint</th></tr></thead><tbody>';
        T.rows.forEach(function (r, i) {
            h += '<tr data-i="' + i + '"' + (i === T.cur ? ' class="cur"' : '') + '>';
            cols.forEach(function (c) {
                if (c === 'DocSrNo') h += '<td><span class="doclink" data-open="' + i + '" title="Open this document">' + esc(str(r[c])) + '</span></td>';
                else h += '<td>' + esc(cell(c, r[c])) + '</td>';
            });
            h += '<td><button type="button" class="cellbtn" data-b="Edit" data-i="' + i + '">Edit</button></td>'
               + '<td><button type="button" class="cellbtn" data-b="WagesPrint" data-i="' + i + '">WagesPrint</button></td></tr>';
        });
        h += '</tbody></table>';
        $('DatagridHistoryHost').innerHTML = h;
        if (K && K.filterRow) K.filterRow($('DatagridHistory'));
    }

    /** DatagridHistory_SelectionChanged:1233 -> DatagridHistoryDetail:1130. */
    function select(i) {
        T.cur = i;
        var t = $('DatagridHistory');
        if (t) Array.prototype.forEach.call(t.tBodies[0].rows, function (tr) { tr.classList.toggle('cur', toInt(tr.getAttribute('data-i')) === i); });
        detail();
    }
    function detail() {
        var r = T.rows[T.cur];
        if (!r) return;
        P.getJson(C + '/' + toInt(r.Id)).then(function (d) {
            if (!d || !d.found) return;
            var cols = ['EntryType', 'WareHouse', 'Item', 'JobLot', 'CropBatch', 'PackType', 'Qty', 'Weight', 'Order', 'Remarks'];
            var rows = (d.details || []).map(function (x) {
                return { EntryType: ci(x, 'EntryType'), WareHouse: ci(x, 'WareHouseName'), Item: ci(x, 'ItemName'),
                    JobLot: ci(x, 'JobLotCode'), CropBatch: ci(x, 'CropBatch'), PackType: ci(x, 'PackTypeCode'),
                    Qty: num(ci(x, 'Qty')), Weight: num(ci(x, 'Weight')), Order: ci(x, 'InvProductionJobOrderNo'), Remarks: ci(x, 'Remarks') };
            });
            var h = '<table class="gx alt" id="grdDetail"><thead><tr>';
            cols.forEach(function (c) { h += '<th' + (c === 'Qty' || c === 'Weight' ? ' class="num"' : '') + '>' + c + '</th>'; });
            h += '</tr></thead><tbody>';
            var tq = 0, tw = 0;
            rows.forEach(function (x) {
                tq += x.Qty; tw += x.Weight;
                h += '<tr>';
                cols.forEach(function (c) {
                    var v = (c === 'Qty' || c === 'Weight') ? hash0(x[c]) : str(x[c]);
                    h += '<td' + (c === 'Qty' || c === 'Weight' ? ' class="num"' : '') + '>' + esc(v) + '</td>';
                });
                h += '</tr>';
            });
            h += '</tbody><tfoot><tr>';
            cols.forEach(function (c) {
                h += '<td class="num">' + (c === 'Qty' ? esc(hash0(tq)) : (c === 'Weight' ? esc(hash0(tw)) : '')) + '</td>';
            });
            h += '</tr></tfoot></table>';
            $('grdDetailHost').innerHTML = h;
            if (K && K.filterRow) K.filterRow($('grdDetail'));
        }).catch(function (e) { box(e.message); });
    }

    /** DatagridHistory_ColumnButtonClick:1089. */
    function button(b, i) {
        var r = T.rows[i];
        if (!r) return;
        if (b === 'Edit') {
            if (str(r.JobOrderStatus) === 'Approved') { box("You Can't Update Approved Record"); return; }
            /* Cells[4] is EntryType. Anything that is not "Input" goes to Output with RecIdOutPut,
               a field this form never assigns - so ReadByIdOutPut(0), exactly as the desktop calls it. */
            if (str(r.EntryType) === 'Input') P.openHosted(0, toInt(r.Id));
            else P.openHosted(1, 0);
        }
        if (b === 'WagesPrint') {
            var w = toInt(r.WagesId);
            if (w <= 0) { box('No Record Found For Display'); return; }
            global.CrystalPrint.open('wages-002', { id: w });
        }
    }

    /** btnNewTransactionHistory_Click:1257. */
    function reset() {
        $('txtfromDateTransactionHistory').value = addDays(today(), -3);
        $('txtToDateTransactionHistory').value = today();
        $('txtDocNoFrom').value = '';
        $('txtDocNumberTo').value = '';
        $('CmbJobOrderTransactionHistory').selectedIndex = 0;
        $('CmbPlantTransactionHistory').selectedIndex = 0;
        T.rows = [];
        $('DatagridHistoryHost').innerHTML = '';
        $('txtfromDateTransactionHistory').focus();
    }

    doc.addEventListener('DOMContentLoaded', function () {
        $('btnNewTransactionHistory').addEventListener('click', reset);
        $('btnRefreshTransactionHistory').addEventListener('click', function () { P.busy('btnRefreshTransactionHistory', combobind); });
        $('btnSearchTransactionHistory').addEventListener('click', search);
        var host = $('DatagridHistoryHost');
        host.addEventListener('click', function (e) {
            var b = e.target.getAttribute('data-b');
            if (b) { select(toInt(e.target.getAttribute('data-i'))); button(b, toInt(e.target.getAttribute('data-i'))); return; }
            var o = e.target.getAttribute('data-open');
            if (o !== null) { select(toInt(o)); button('Edit', toInt(o)); return; }
            var tr = e.target.closest('tbody tr');
            if (tr) select(toInt(tr.getAttribute('data-i')));
        });
        /* ctrlGrdBar1 over DatagridHistory: the shared GridBar (countx_grid_bar.js, data-gridbar on #DatagridHistoryHost). */
    });

    /* frmFoodProduction_Load:587-588 */
    P.ready.then(function () {
        $('txtfromDateTransactionHistory').value = addDays(today(), -3);
        $('txtToDateTransactionHistory').value = today();
        combobind();
    }).catch(function () { /* shown by the shell */ });

    /* KeyDown:816-826 - index 5: Ctrl+D reloads the detail (Ctrl+Enter's comparison is discarded). */
    P.onKey(function (e, index) {
        if (index !== 5 || !e.ctrlKey) return;
        if (e.key === 'd' || e.key === 'D') { e.preventDefault(); detail(); }
    });
    /* SelectedIndexChanged:908 - index 5 focuses Date From (hidden when index 5 is Summary). */
    P.onSelect(function (index) {
        if (index === 5) { var d = $('txtfromDateTransactionHistory'); if (d && d.offsetParent) d.focus(); }
    });
}(window));
