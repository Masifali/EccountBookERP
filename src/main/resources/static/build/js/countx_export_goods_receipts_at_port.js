/* ============================================================================================
 * countx_export_goods_receipts_at_port.js - ExImGoodsReceiptsAtPort.cs "Goods Receipts At Port"
 * (Architecture.WinApp.Export) and its loader GetForwardingDataForGoodsReceiptsAsPort.cs.
 * Every event of the two desktop forms has its counterpart here (Load, tab change, New, Save, Update,
 * Load Forwarding Data, grid CellUpdated, history DoubleClick, Register-Print; loader Load, Search,
 * Load, Reset, grid SelectionChanged), with the desktop's messages and order.
 * Data: /api/export/goods-receipts-at-port (ExportShipmentFormsController -> ExportGoodsReceiptsAtPortService).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var G = global.ExportG, S = global.ExportSF;
    var API = '/api/export/goods-receipts-at-port';

    var RIGHTS = { Save: false, Update: false };
    var HIST = [];
    var UPDATE_MODE = false;
    var LOADER = { rows: [], master: [], supply: [] };

    /* grd: dtDetail columns (Id, ForwardingId, ForwardingDetailId, WarehouseId, PackEquivalent, BalQty, BalWeight hidden). */
    var grd = S.editGrid('grd', [
        { key: 'ForwardingNo' }, { key: 'ForwardingDate', date: true }, { key: 'CustomerName' }, { key: 'GpNo' }, { key: 'GpDate', date: true },
        { key: 'VehicleNo' }, { key: 'BiltyNo' }, { key: 'Warehouse' },
        { key: 'QtyReceived', edit: 'num', num: true }, { key: 'WeightReceived', edit: 'num', num: true }, { key: 'RemarksDetail', edit: 'text' }
    ], { onCellUpdated: cellUpdated });

    var grdHistory = G.grid('grdHistory', [
        { key: 'DocNo', link: true }, { key: 'DocDate', date: true }, { key: 'CustomerName' }, { key: 'GpNo' }, { key: 'GpDate', date: true },
        { key: 'VehicleNo' }, { key: 'BiltyNo' }, { key: 'Warehouse' }, { key: 'QtyReceived', num: true }, { key: 'WeightReceived', num: true },
        { key: 'Remarks' }, { key: 'EntryUser' }, { key: 'EntryDate', date: true }, { key: 'ModifyUser' }, { key: 'ModifyDate', date: true }
    ], { emptyId: 'grdHistoryEmpty', onDblClick: historyDoubleClick, onLink: function (i) { historyDoubleClick(i); } });

    var tabs = G.tabs('main', ['tabForm', 'tabHistory'], function (p) { if (footerLabel) footerLabel(); if (p === 'tabHistory') bindHistory(); });
    var footerLabel = S.footerToggle('btnFooterHistory', tabs, 'tabForm', 'tabHistory');

    // ------------------------------------------------------------------------------ load / history

    /** ExImGoodsReceiptsAtPort_Load. */
    function load() {
        return G.getJson(API + '/setup').then(function (d) {
            d = d || {};
            RIGHTS = d.rights || RIGHTS;
            G.$id('btnsave').disabled = !RIGHTS.Save;
            G.$id('btnupdate').disabled = !RIGHTS.Update;
            if (d.historyError) G.box(d.historyError);
            setHistory(d.history || []);
            grd.draw([]);
        }).catch(function (e) { G.box(e.message); });
    }
    /** BindHistory: the grid is only re-bound when rows came back (an empty result keeps the old grid, as on the desktop). */
    function setHistory(rows) {
        HIST = rows;
        if (rows.length > 0) grdHistory.draw(rows);
        else if (!grdHistory.rows.length) grdHistory.draw([]);
    }
    function bindHistory() {
        return G.getJson(API + '/history').then(function (rows) { setHistory(rows || []); }).catch(function (e) { G.box(e.message); });
    }

    // ------------------------------------------------------------------------------ form

    /** Reset (btnNew_Click): Save visible, Update hidden, UpdateMode off, grid cleared. */
    function reset() {
        G.show('btnsave', true); G.show('btnupdate', false);
        UPDATE_MODE = false;
        grd.draw([]);
    }

    /** grd_CellUpdated: Qty / Weight capped at the balance, the other recomputed through PackEquivalent. */
    function cellUpdated(i, key, row) {
        var balQty = G.netD(row.BalQty), qty = G.netD(row.QtyReceived), balWeight = G.netD(row.BalWeight), weight = G.netD(row.WeightReceived);
        var pe = Number(row.PackEquivalent);
        if (key === 'QtyReceived') {
            if (qty > balQty) {
                row.QtyReceived = balQty; row.WeightReceived = balQty * pe;
                grd.draw();
                G.box('Qty Rcvd Can not Greater Than Balance Qty Of ' + balQty);
            } else { row.WeightReceived = qty * pe; grd.draw(); }
        }
        if (key === 'WeightReceived') {
            if (weight > balWeight) {
                row.WeightReceived = balWeight; row.QtyReceived = balWeight / pe;
                grd.draw();
                G.box('Weight Rcvd Can not Greater Than Balance Weight Of ' + balWeight);
            } else { row.QtyReceived = weight / pe; grd.draw(); }
        }
    }

    /** btnsave_Click (also btnupdate_Click). */
    function save(btn) {
        return G.busy(btn, function () {
            if (!G.ask(UPDATE_MODE ? 'Are you sure to Update?' : 'Are you sure to Save?')) return Promise.resolve();
            return G.postJson(API + '/save', { update: UPDATE_MODE, rows: grd.rows }).then(function (d) {
                G.box((d && d.message) || (UPDATE_MODE ? 'Record Updated Successfully' : 'Record Saved Successfully'));
                reset();
                return bindHistory();
            }).catch(function (e) { G.box(e.message); });
        });
    }

    /** grdHistory_DoubleClick: every history row with the clicked SortNo back into the form, UpdateMode on. */
    function historyDoubleClick(i) {
        var item = HIST[i]; if (!item) return;
        var sortNo = G.netI(item.SortNo);
        var detail = HIST.filter(function (r) { return G.netI(r.SortNo) === sortNo; });
        if (detail.length > 0) {
            tabs.select('tabForm'); if (footerLabel) footerLabel();
            G.show('btnsave', false); G.show('btnupdate', true);
            UPDATE_MODE = true;
            grd.draw(detail.map(function (r) {
                return { Id: r.Id, ForwardingId: r.ForwardingId, ForwardingDetailId: r.ForwardingDetailId, ForwardingNo: r.DocNo, ForwardingDate: r.DocDate,
                    CustomerName: r.CustomerName, GpNo: r.GpNo, GpDate: r.GpDate, VehicleNo: r.VehicleNo, BiltyNo: r.BiltyNo, WarehouseId: r.WarehouseId,
                    Warehouse: r.Warehouse, PackEquivalent: G.netD(r.WeightReceived) / G.netD(r.QtyReceived), QtyReceived: r.QtyReceived, BalQty: r.QtyReceived,
                    WeightReceived: r.WeightReceived, BalWeight: r.WeightReceived, RemarksDetail: r.Remarks };
            }));
        } else {
            G.show('btnsave', true); G.show('btnupdate', false);
            grd.draw([]);
        }
    }

    /** PrintRegister_Click: 466 register of the history. */
    function printRegister(btn) {
        if (!HIST || HIST.length === 0) { G.box('No Record Found For Display'); return; }
        S.print('exp-466', {}, btn);
    }

    // ------------------------------------------------------------------------------ loader dialog

    var dlg = S.dialog('loaderModal');

    /** btnLoaderForm_Click: a fresh dialog (LoadInvoices_Load), ShowDialog, then LoadDataFromLoader. */
    function openLoader() {
        LOADER = { rows: [], master: [], supply: [] };
        G.bind('CmbPartyName', [], 'Id', 'Name'); G.bind('cmbCurrency', [], 'Id', 'Name'); G.bind('cmbItemName', [], 'Id', 'Name');
        G.setText('Todate', G.today());
        drawMaster(); drawDetail([]);
        dlg.open();
        G.focus('FromDate');
        return loaderSearch(null).then(loaderCombos);
    }
    function loaderCombos() {
        return G.getJson(API + '/loader/combos').then(function (d) {
            d = d || {};
            ['parties', 'currencies', 'items'].forEach(function (k) { if (d[k + 'Error']) G.box(d[k + 'Error']); });
            if ((d.parties || []).length) G.bind('CmbPartyName', d.parties, 'Id', 'Name');
            if ((d.currencies || []).length) G.bind('cmbCurrency', d.currencies, 'Id', 'Name');
            if ((d.items || []).length) G.bind('cmbItemName', d.items, 'Id', 'Name');
        }).catch(function (e) { G.box(e.message); });
    }
    /** ExportPreInvoicesLoad: the grid is only re-bound when rows came back. */
    function loaderSearch(btn) {
        return G.busy(btn, function () {
            var q = '?partyId=' + G.valI('CmbPartyName') + '&itemId=' + G.valI('cmbItemName') + '&currencyId=' + G.valI('cmbCurrency');
            return G.getJson(API + '/loader' + q).then(function (rows) {
                rows = rows || [];
                if (rows.length > 0) {
                    LOADER.rows = rows;
                    var seen = {};
                    LOADER.master = [];
                    rows.forEach(function (r) { var id = G.netI(r.Id); if (!seen[id]) { seen[id] = 1; LOADER.master.push(r); } });
                    drawMaster();
                }
            }).catch(function (e) { G.box(e.message); });
        });
    }
    function drawMaster() {
        var tb = G.$id('loaderGrd').tBodies[0];
        tb.innerHTML = LOADER.master.map(function (r, i) {
            return '<tr data-i="' + i + '"><td class="ctr"><input type="checkbox" class="win-rowcheck" data-i="' + i + '"/></td>'
                + '<td>' + G.esc(G.shortDate(r.DocDate)) + '</td><td>' + G.esc(r.DocNo) + '</td><td>' + G.esc(r.CustomerName) + '</td><td>' + G.esc(r.GpNo) + '</td>'
                + '<td>' + G.esc(G.shortDate(r.GPDate || r.GpDate)) + '</td><td>' + G.esc(r.VehicleNo) + '</td><td>' + G.esc(r.BiltyNo) + '</td><td>' + G.esc(r.EntryUser) + '</td>'
                + '<td>' + G.esc(G.shortDate(r.EntryDate)) + '</td><td>' + G.esc(r.ModifyUser) + '</td><td>' + G.esc(G.shortDate(r.ModifyDate)) + '</td></tr>';
        }).join('');
        G.$id('loaderAll').checked = false;
    }
    /** grd_SelectionChanged: the detail rows of the selected forwarding. */
    function loaderSelect(i) {
        var m = LOADER.master[i]; if (!m) return;
        G.$id('loaderGrd').querySelectorAll('tbody tr').forEach(function (tr) { if (G.netI(tr.getAttribute('data-i')) === i) tr.setAttribute('data-current', '1'); else tr.removeAttribute('data-current'); });
        drawDetail(LOADER.rows.filter(function (r) { return G.netI(r.Id) === G.netI(m.Id); }));
    }
    function drawDetail(rows) {
        var t = G.$id('grddetail'), q = 0, w = 0;
        t.tBodies[0].innerHTML = rows.map(function (r) {
            q += G.netD(r.NoOfBags); w += G.netD(r.NetWeight);
            return '<tr><td>' + G.esc(r.WareHouseName) + '</td><td>' + G.esc(r.ItemName) + '</td><td>' + G.esc(r.PackingType) + '</td><td>' + G.esc(r.CropYear) + '</td>'
                + '<td>' + G.esc(r.PackUom) + '</td><td class="num">' + G.esc(G.fmt(r.NoOfBags)) + '</td><td class="num">' + G.esc(G.fmt(r.NetWeight)) + '</td></tr>';
        }).join('');
        t.tFoot.innerHTML = rows.length ? '<tr><td colspan="5"></td><td class="num">&Sigma; ' + G.esc(G.fmt(q)) + '</td><td class="num">&Sigma; ' + G.esc(G.fmt(w)) + '</td></tr>' : '';
    }
    /** btnLoadOnInvoice_Click_1: every procedure row of the checked forwardings, then Hide and LoadDataFromLoader. */
    function loaderLoad() {
        var checked = [];
        G.$id('loaderGrd').querySelectorAll('input.win-rowcheck:checked').forEach(function (c) { checked.push(LOADER.master[G.netI(c.getAttribute('data-i'))]); });
        if (checked.length === 0) { G.box('Check the row first'); return; }
        LOADER.supply = [];
        checked.forEach(function (m) { LOADER.rows.forEach(function (r) { if (G.netI(r.Id) === G.netI(m.Id)) LOADER.supply.push(r); }); });
        dlg.close();
        loadDataFromLoader();
    }
    /** LoadDataFromLoader: rows not yet in the grid (by ForwardingDetailId) are added. */
    function loadDataFromLoader() {
        var dt = LOADER.supply;
        if (!dt || dt.length <= 0) return;
        dt.forEach(function (r) {
            var exists = grd.rows.some(function (x) { return G.netI(x.ForwardingDetailId) === G.netI(r.ForwardingDetailId); });
            if (exists) return;
            grd.rows.push({ Id: 0, ForwardingId: G.netI(r.Id), ForwardingDetailId: G.netI(r.ForwardingDetailId), ForwardingNo: G.netI(r.DocNo), ForwardingDate: G.isoDate(r.DocDate),
                CustomerName: G.str(r.CustomerName), GpNo: G.netI(r.GpNo), GpDate: G.isoDate(r.GPDate || r.GpDate), VehicleNo: G.str(r.VehicleNo), BiltyNo: G.str(r.BiltyNo),
                WarehouseId: G.netI(r.WarehouseToId), Warehouse: G.str(r.WareHouseName), PackEquivalent: G.netD(r.NetWeight) / G.netD(r.NoOfBags),
                QtyReceived: G.netD(r.NoOfBags), BalQty: G.netD(r.NoOfBags), WeightReceived: G.netD(r.NetWeight), BalWeight: G.netD(r.NetWeight), RemarksDetail: '' });
        });
        grd.draw();
    }
    /** btnReset_Click. */
    function loaderReset(btn) {
        G.setVal('CmbPartyName', '0');
        return G.busy(btn, function () { return loaderCombos().then(function () { return loaderSearch(null); }); });
    }

    // ------------------------------------------------------------------------------ wiring

    document.addEventListener('DOMContentLoaded', function () {
        G.initFullscreen();
        G.$id('btnNew').addEventListener('click', reset);
        G.$id('btnsave').addEventListener('click', function () { save(this); });
        G.$id('btnupdate').addEventListener('click', function () { save(this); });
        G.$id('btnLoaderForm').addEventListener('click', function () { openLoader(); });
        G.$id('PrintRegister').addEventListener('click', function () { printRegister(this); });
        G.$id('btngrnlod').addEventListener('click', function () { loaderSearch(this); });
        G.$id('btnLoadOnInvoice').addEventListener('click', loaderLoad);
        G.$id('btnReset').addEventListener('click', function () { loaderReset(this); });
        G.$id('loaderClose').addEventListener('click', function () { LOADER.supply = []; dlg.close(); });
        G.$id('loaderAll').addEventListener('change', function () { var on = this.checked; G.$id('loaderGrd').querySelectorAll('input.win-rowcheck').forEach(function (c) { c.checked = on; }); });
        G.$id('loaderGrd').addEventListener('click', function (e) {
            if (e.target.closest('input')) return;
            var tr = e.target.closest('tbody tr'); if (tr) loaderSelect(G.netI(tr.getAttribute('data-i')));
        });
        document.addEventListener('keydown', function (e) { S.enterAsTab(e); });
        load();
    });

    global.ExportGoodsReceiptsAtPort = { reset: reset, save: save, openLoader: openLoader };
})(window);
