/* countx_export_pending_forwarding_for_commercial_invoice.js - PendingForwardingForCommercialInvoice.cs, screen 244.
 * Data: /api/export/pending-forwarding-for-commercial-invoice. */
(function (global) {
    'use strict';
    var H = global.ExRptC, API = '/api/export/pending-forwarding-for-commercial-invoice';
    var ROWS = [], CUR = -1;
    /* grdSettings: grouped by PartyName (hidden when grouped), group totals, the 8 id columns hidden. */
    var COLS = [
        { key: 'ProformaNo', cap: 'Proforma No' }, { key: 'CommissionAgent', cap: 'Commission Agent' }, { key: 'CommRate', cap: 'Comm Rate', kind: 'q3' },
        { key: 'CurrencyCode', cap: 'Fcy Code' }, { key: 'ItemName', cap: 'Item Name' }, { key: 'PackingType', cap: 'Packing Type' }, { key: 'CropYear', cap: 'Crop Year' },
        { key: 'PackUom', cap: 'Pack Uom' }, { key: 'EbUnit', cap: 'Eb Unit', kind: 'q3' }, { key: 'EbTotal', cap: 'Eb Total', kind: 'q3', sum: true },
        { key: 'GrossWeight', cap: 'Gross Weight', kind: 'q3', sum: true }, { key: 'MTon', cap: 'M Ton', kind: 'int', sum: true }, { key: 'ShipMTon', cap: 'Ship M Ton', kind: 'int', sum: true },
        { key: 'BalMTon', cap: 'Bal M Ton', kind: 'int', sum: true }, { key: 'NoOfBags', cap: 'No Of Bags', kind: 'int', sum: true }, { key: 'ShipBags', cap: 'Ship Bags', kind: 'int', sum: true },
        { key: 'BalBags', cap: 'Bal Bags', kind: 'int', sum: true }, { key: 'NetWeight', cap: 'Net Weight', kind: 'q3', sum: true }, { key: 'ShipWeight', cap: 'Ship Weight', kind: 'q3', sum: true },
        { key: 'BalWeight', cap: 'Bal Weight', kind: 'q3', sum: true }, { key: 'ItemRate', cap: 'Item Rate', kind: 'q3' }, { key: 'RateUom', cap: 'Rate Uom' },
        { key: 'Amount', cap: 'Amount', kind: 'amt', sum: true }, { key: 'ShipAmount', cap: 'Ship Amount', kind: 'amt', sum: true }, { key: 'BalAmount', cap: 'Bal Amount', kind: 'amt', sum: true },
        { key: 'OtherRate', cap: 'Other Rate', kind: 'q3' }, { key: 'PackingExpiryDate', cap: 'Packing Expiry Date' }, { key: 'ProductionNo', cap: 'Production No' }
    ];

    /* Janus group rows: one header per PartyName (rows kept in the procedure's order), a total row per group and a grand total. */
    function render() {
        var t = H.$id('grd'), head = t.tHead, body = t.tBodies[0], foot = t.tFoot;
        head.innerHTML = '<tr>' + COLS.map(function (c) { var f = H.cell(c.kind, 0); return '<th' + (f.num ? ' class="num"' : '') + ' data-col="' + H.esc(c.key) + '">' + H.esc(c.cap) + '</th>'; }).join('') + '</tr>';
        var groups = [], byName = {};
        ROWS.forEach(function (r, i) {
            var k = H.str(H.col(r, 'PartyName'));
            if (!byName[k]) { byName[k] = { name: k, idx: [], sums: {} }; groups.push(byName[k]); }
            byName[k].idx.push(i);
            COLS.forEach(function (c) { if (c.sum) byName[k].sums[c.key] = (byName[k].sums[c.key] || 0) + H.netD(H.col(r, c.key)); });
        });
        var grand = {};
        var h = '';
        function totalRow(sums, label, cls) {
            var s = '<tr class="' + cls + '">';
            COLS.forEach(function (c, idx) {
                var f = H.cell(c.kind, 0);
                var v = c.sum ? H.cell(c.kind, sums[c.key] || 0).v : (idx === 0 ? label : '');
                s += '<td' + (f.num ? ' class="num"' : '') + '><b>' + H.esc(v) + '</b></td>';
            });
            return s + '</tr>';
        }
        groups.forEach(function (g) {
            h += '<tr class="exr-group"><td colspan="' + COLS.length + '" style="background:#e3eeee;font-weight:bold;">Party Name: ' + H.esc(g.name) + ' (' + g.idx.length + ')</td></tr>';
            g.idx.forEach(function (i) {
                var r = ROWS[i];
                h += '<tr data-i="' + i + '"' + (i === CUR ? ' class="is-current"' : '') + '>';
                COLS.forEach(function (c) { var f = H.cell(c.kind, H.col(r, c.key)); h += '<td' + (f.num ? ' class="num"' : '') + '>' + H.esc(f.v) + '</td>'; });
                h += '</tr>';
            });
            h += totalRow(g.sums, 'Total', 'exr-group-total');
            COLS.forEach(function (c) { if (c.sum) grand[c.key] = (grand[c.key] || 0) + (g.sums[c.key] || 0); });
        });
        body.innerHTML = h;
        foot.innerHTML = ROWS.length ? totalRow(grand, 'Σ', '') : '';
        H.show('grdEmpty', !ROWS.length);
    }
    function filters() {
        return { partyId: H.netI(H.val('CmbPartyName')), itemId: H.netI(H.val('cmbItemName')), currencyId: H.netI(H.val('cmbCurrency')), skipZero: H.checked('chkSkipZero'),
            packingExpiryDate: H.val('CmbPackingExpiryDate') === '0' ? '' : H.val('CmbPackingExpiryDate'), productionNo: H.val('CmbProductionNo') === '0' ? '' : H.val('CmbProductionNo'),
            proformaId: H.netI(H.val('CmbProformaNo')) };
    }
    /** btngrnlod_Click -> ExportPreInvoicesLoad (the two date pickers are not parameters - desktop quirk). */
    function search(btn) {
        return H.busy(btn, function () {
            return H.postJson(API + '/show', filters()).then(function (rows) { ROWS = rows || []; CUR = -1; render(); });
        });
    }
    /** LoadInvoices_Load: FromDate = ActiveYr.Start_Period, all combos. */
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            H.setText('FromDate', d.yearStart ? H.isoDate(d.yearStart) : H.today()); H.setText('Todate', H.today());
            H.bind('CmbProformaNo', d.proformas || [], 'Id', 'Name');
            H.bind('CmbPartyName', d.parties || [], 'Id', 'Name');
            H.bind('cmbItemName', d.items || [], 'Id', 'Name');
            H.bind('cmbCurrency', d.currencies || [], 'Id', 'Name');
            H.bind('CmbPackingExpiryDate', d.packingExpiryDates || [], 'Id', 'Name');
            H.bind('CmbProductionNo', d.productionNos || [], 'Id', 'Name');
            H.$id('footerInfo').textContent = 'PendingForwardingForCommercialInvoice  -  Print 389';
            H.focus('FromDate');
        }).catch(function (e) { H.box(e.message || 'Error occurred during database call.'); });
    }
    /** btnReset_Click: focus From Date, Party Name cleared (only that, as on the desktop). */
    function reset() { H.focus('FromDate'); H.setVal('CmbPartyName', 0); }
    function print(btn) {
        if (!ROWS.length) { H.box('Not Record Found For Display'); return; }
        var f = filters();
        return H.print('389-GetPreInvoicesAndForwardingDateForCommercialInvoices.rpt', { exImLcOrderId: f.proformaId || undefined, supplierCustomerId: f.partyId || undefined,
            fcyId: f.currencyId || undefined, itemId: f.itemId || undefined, packingExpiryDate: f.packingExpiryDate || undefined, productionNo: f.productionNo || undefined,
            zeroBalanceType: f.skipZero ? 1 : undefined }, btn);
    }
    function toggleHistory() { var b = H.$id('grdBox'); if (b) b.scrollIntoView({ behavior: 'smooth' }); H.focusGrid('grd'); }

    document.addEventListener('DOMContentLoaded', function () {
        H.wireFullscreen();
        H.wireGrid('grd', { select: function (i) { CUR = i; } });
        /* LoadForwardingForCommercialInvoice_KeyDown: Enter = Tab; Ctrl+Alt+L shows chkSkipZero (always visible here). */
        H.wireKeys({ shortcuts: function () { H.show('chkSkipZero', true); } });
        load();
    });

    global.ExportPendingForwarding = { search: search, reset: reset, print: print, toggleHistory: toggleHistory };
}(window));
