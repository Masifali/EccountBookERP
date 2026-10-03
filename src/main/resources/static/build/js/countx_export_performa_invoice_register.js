/* ============================================================================================
 * countx_export_performa_invoice_register.js - PerformaInvoiceRegister.cs "Performa Invoice Register" (no ScreenDefinition row).
 * Data: /api/export/performa-invoice-register (USP_GetProformaDataForInvoices, DocumentTypeIds "151").
 * Print: exp-o-546 (546-GetProformaDataForInvoices.rpt) with the filters of the last load (the desktop prints dtloadExIO).
 * DocNo opens /export/proforma-invoice?id=<Id>; NoOfAttachments opens the AttachmentView list (RefDocumentTypeId 151).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptO, $id = H.$id, box = H.box;
    var API = '/api/export/performa-invoice-register';
    var S = { raw: [], main: [], detailed: [], byId: {}, detail: [], last: null };

    /* GridSettingForMainDetail / GridDetailSettings: "#,##0" and "#,##0.##" sums (shared spec) */
    var Q = function (k) { return { key: k, caption: k, num: true, sum: true, fmt: 'n0' }; };
    var Q2 = function (k) { return { key: k, caption: k, num: true, sum: true, fmt: 'o2' }; };
    var N = function (k) { return { key: k, caption: k, num: true }; };
    var ITEM_COLS = [
        { key: 'ItemName', caption: 'ItemName' }, { key: 'PackingType', caption: 'PackingType' }, Q2('PackingWeight'), { key: 'CropYear', caption: 'CropYear' },
        Q2('NoOfContainers'), Q2('NoOfBags/Cntnr'), { key: 'PackUom', caption: 'PackUom' }, N('MTon'), N('ShipMTon'), N('BalMTon'),
        Q('NoOfBags'), Q('ShipBags'), Q('BalBags'), Q2('PackingTotalWeight'), Q('NetWeight'), Q('ShipWeight'), Q('BalWeight'), N('ItemRate'),
        Q('Amount'), Q('ShipAmount'), Q('BalAmount'), { key: 'PackingExpiryDate', caption: 'PackingExpiryDate' }, { key: 'ContainerSize', caption: 'ContainerSize' },
        { key: 'OtherDescription', caption: 'OtherDescription' }, { key: 'OuterPackUomDescription', caption: 'OuterPackUomDescription' }
    ];
    var HEAD_COLS = [
        { key: 'Id', hidden: true }, { key: 'DocumentTypeId', hidden: true }, { key: 'DocNo', caption: 'DocNo', num: true, link: true },
        { key: 'DocDate', caption: 'DocDate', fmt: 'd' }, { key: 'Pri_Ref#', caption: 'Pri_Ref#' }, { key: 'Pri_RefDate', caption: 'Pri_RefDate', fmt: 'd' },
        { key: 'SupplierCustomerId', hidden: true }, { key: 'PartyName', caption: 'PartyName' }, { key: 'SalesPersonId', hidden: true },
        { key: 'SalesMan', caption: 'SalesMan' }, { key: 'CommissionAgentId', hidden: true }, { key: 'CommissionAgent', caption: 'CommissionAgent' },
        N('Comm%'), N('CommAmount')
    ];
    var TAIL_COLS = [
        { key: 'InsuranceRemarks', caption: 'InsuranceRemarks' }, { key: 'RemarksHeader', caption: 'RemarksHeader' }, { key: 'EntryUser', caption: 'EntryUser' },
        { key: 'EntryDate', caption: 'EntryDate', fmt: 'd' }, { key: 'ModifyUser', caption: 'ModifyUser' }, { key: 'ModifyDate', caption: 'ModifyDate', fmt: 'd' },
        { key: 'NoOfAttachments', caption: 'NoOfAttachments', num: true, link: true }
    ];
    /* GridMainDetail ("Detailed"): Id, DocumentTypeId, SalesPersonId, CommissionAgentId, SupplierCustomerId, ItemId hidden */
    var DETAILED_COLS = HEAD_COLS.concat([
        { key: 'CurrencyCode', caption: 'CurrencyCode' }, { key: 'DeliveryTerm', caption: 'DeliveryTerm' }, { key: 'PaymentTerm', caption: 'PaymentTerm' },
        { key: 'LoadingPort', caption: 'LoadingPort' }, { key: 'DestinationPort', caption: 'DestinationPort' }, { key: 'ItemId', hidden: true }
    ]).concat(ITEM_COLS).concat(TAIL_COLS);
    /* grd ("Main"): the Id columns hidden, NoOfAttachments a link, no totals */
    var MAIN_COLS = HEAD_COLS.map(function (c) { var o = {}; for (var k in c) o[k] = c[k]; delete o.sum; return o; }).concat([
        { key: 'FCurrencyId', hidden: true }, { key: 'CurrencyCode', caption: 'CurrencyCode' }, { key: 'DeliveryTermId', hidden: true },
        { key: 'DeliveryTerm', caption: 'DeliveryTerm' }, { key: 'PaymentTermId', hidden: true }, { key: 'PaymentTerm', caption: 'PaymentTerm' },
        { key: 'LoadingPortId', hidden: true }, { key: 'LoadingPort', caption: 'LoadingPort' }, { key: 'DestinationPortId', hidden: true },
        { key: 'DestinationPort', caption: 'DestinationPort' }
    ]).concat(TAIL_COLS);
    /* grddetail: PerformaDetailId / ItemId hidden */
    var DETAIL_COLS = [{ key: 'PerformaDetailId', hidden: true }, { key: 'ItemId', hidden: true }].concat(ITEM_COLS);

    function filters() {
        return { partyId: H.netI(H.val('CmbPartyName')), itemId: H.netI(H.val('cmbItemName')), currencyId: H.netI(H.val('cmbCurrency')),
            skipZero: H.checked('ChboxSkipZero') };
    }
    function bindCombos(d) {
        (d.errors || []).forEach(function (m) { box(m); });
        if ((d.parties || []).length) H.bind('CmbPartyName', d.parties, 'Id', 'name', true);
        if ((d.currencies || []).length) H.bind('cmbCurrency', d.currencies, 'Id', 'name', true);
        if ((d.items || []).length) H.bind('cmbItemName', d.items, 'Id', 'name', true);
    }
    /** grd_SelectionChanged: the selected Id's lines into grddetail. */
    function selectMain(i) {
        var r = S.main[i]; if (!r) return;
        S.detail = S.byId[String(H.netI(r.Id))] || [];
        H.drawGrid('grddetail', DETAIL_COLS, S.detail, {});
    }
    /**
     * ExportInvoicesLoad(): the grids are rebuilt only when rows come back (an empty result leaves the previous grids
     * on screen, as the desktop does), while dtloadExIO - what Print-546 checks - always takes the new result.
     */
    function loadInvoices() {
        var f = filters();
        return H.postJson(API + '/show', f).then(function (d) {
            d = d || {};
            S.raw = d.detailed || []; S.last = f;
            if (!S.raw.length) return;
            S.detailed = S.raw; S.main = d.main || []; S.byId = d.detailById || {};
            H.drawGrid('GridMainDetail', DETAILED_COLS, S.detailed, {});
            H.drawGrid('grd', MAIN_COLS, S.main, { cur: 0 });
            selectMain(0);
        }).catch(function (e) { box(e.message); });
    }
    function load() {
        H.setText('Todate', H.today());
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            H.setText('FromDate', d.yearStart || H.today());      /* FromDate.Value = ActiveYr.Start_Period */
            $id('FromDate').focus();
            return loadInvoices().then(function () { bindCombos(d); });   /* Load order: ExportInvoicesLoad, then the combos */
        }).catch(function (e) { box(e.message); });
    }
    function search(btn) { return H.busy(btn, loadInvoices); }
    /** btnReset_Click: focus From Date, Party text blank, PartyNameFill / ItemFill / CurrencyFill, ExportInvoicesLoad. */
    function reset(btn) {
        $id('FromDate').focus();
        H.setVal('CmbPartyName', '0');
        return H.busy(btn, function () {
            return H.getJson(API + '/combos').then(function (d) { bindCombos(d || {}); return loadInvoices(); });
        });
    }
    /** btnPrint_Click: "No Record Found For Display" without rows, else 546. */
    function print(btn) {
        if (!S.raw.length || !S.last) { box('No Record Found For Display'); return; }
        var f = S.last;
        return H.print('exp-o-546', { supplierCustomerId: f.partyId, fcyId: f.currencyId, itemId: f.itemId, skipZero: f.skipZero ? 1 : 0 }, btn);
    }
    function link(rows) {
        return function (key, i) {
            var r = rows()[i]; if (!r) return;
            if (key === 'NoOfAttachments') H.attachments(API + '/attachments?id=' + H.netI(r.Id)).catch(function (e) { box(e.message); });
            else if (key === 'DocNo') global.open('/export/proforma-invoice?id=' + H.netI(r.Id), '_blank');
        };
    }
    document.addEventListener('DOMContentLoaded', function () {
        H.fullscreenButtons();
        H.tabs('tabControl1');
        H.gridEvents('grd', { select: selectMain, link: link(function () { return S.main; }) });
        H.gridEvents('GridMainDetail', { link: link(function () { return S.detailed; }) });
        H.gridEvents('grddetail', {});
        /* keyboard row moves on "Main" also change the selection (SelectionChanged) */
        $id('grd').addEventListener('keydown', function (e) {
            if (e.key !== 'ArrowDown' && e.key !== 'ArrowUp') return;
            var i = H.selectedRow('grd'), n = i + (e.key === 'ArrowDown' ? 1 : -1);
            if (n < 0 || n >= S.main.length) return;
            e.preventDefault();
            $id('grd').querySelectorAll('tbody tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', +x.getAttribute('data-i') === n); });
            selectMain(n);
        });
        /* the desktop form has no KeyDown handler */
        load();
    });
    global.ExportPir = { search: search, reset: reset, print: print };
}(window));
