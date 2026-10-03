/* ============================================================================================
 * countx_export_return_invoice.js - ExportReturnInvoice.cs (Architecture.WinApp.Export), screen 191
 * "Export Return Invoice", DocumentTypeId 242, with the popup frmLoadCommercialInvoiceForReturn.
 * Every button, TextChanged, grid edit / button / key, history action and shortcut of the desktop form has its
 * counterpart here with the desktop's messages and order. Data: /api/export/return-invoice (ExportReturnController ->
 * ExportReturnInvoiceService -> the desktop's procedures). Prints: CrystalPrint 'exp-216' (216-ExportReturnInvoiceSlip,
 * USp_ExportReturnInvoice_SlipAndRegister @Id) and 'exp-102' (102-ExportReturnInvoiceVoucherSlip,
 * SpVouchers_ExportReturnInvoiceVoucherSlipNew_Rpt @Id = voucher head id).
 * ============================================================================================ */
(function (global) {
    'use strict';

    var X = global.ExM;
    var $id = X.$id, box = X.box, ask = X.ask, netD = X.netD, netI = X.netI, str = X.str;
    var API = '/api/export/return-invoice';

    var PERM = { Save: true, Update: true, Print: true, Delete: true };
    var CFG = { enableExportReturnReverseFlow: false, defaultDaysToLessFromHistoryFromDate: 0, decAmount: 0, decFcy: 0 };
    var L = { customers: [], currencies: [], creditAccounts: [], warehouses: [] };
    var S = { recId: 0, updateMode: false, detail: [], pay: [], pending: [], checked: {}, removed: [], exImInvoiceId: 0, customerGlAcId: 0 };
    var H = { rows: [], cur: -1, detail: [], pay: [] };
    var LD = { rows: [], main: [], detail: [], checked: {}, mainCur: -1 };

    function fA(v) { return X.fixed(v, CFG.decAmount); }      /* clsGlobalVariables.stringFormatsingle */
    function fF(v) { return X.fixed(v, CFG.decFcy); }         /* clsGlobalVariables.stringFormatsingleForFcy */
    function roundEven(v) { var r = Math.round(v); if (Math.abs(v % 1) === 0.5 && r % 2 !== 0) r -= 1; return r; }   /* Math.Round(decimal) */
    function reverse() { return !!CFG.enableExportReturnReverseFlow; }

    // ------------------------------------------------------------------------------ columns

    var DETAIL_COLS = [
        { k: 'Id', hide: true }, { k: 'ExImInvoiceId', hide: true }, { k: 'ExImInvoiceDetailId', hide: true }, { k: 'GrnId', hide: true },
        { k: 'GrnDetailId', hide: true }, { k: 'GrnNo', t: 'GrnNo' }, { k: 'WarehouseId', hide: true }, { k: 'Warehouse', t: 'Warehouse' },
        { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 160 }, { k: 'ItemCode', t: 'ItemCode' }, { k: 'CropYearId', hide: true },
        { k: 'CropYear', t: 'CropYear' }, { k: 'JobLotId', hide: true }, { k: 'JobLot', t: 'JobLot' }, { k: 'PackingTypeId', hide: true },
        { k: 'PackingType', t: 'PackingType' }, { k: 'PackUomId', hide: true }, { k: 'PackUomCode', t: 'Pack Uom' }, { k: 'PackUomEquivalent', hide: true },
        { k: 'Qty', t: 'Qty', num: 3 }, { k: 'NetWeight', t: 'NetWeight', num: 3 }, { k: 'StockWeight', t: 'StockWeight', num: 3 }, { k: 'Mton', t: 'Mton', num: 3 },
        { k: 'RatePrice', t: 'RatePrice', num: 4, edit: 'num' }, { k: 'RateUomId', hide: true }, { k: 'RateUomCode', t: 'Rate Uom' }, { k: 'RateUomEquivalent', hide: true },
        { k: 'FcyAmountWithOutAddLess', t: 'FcyAmountWithOutAddLess', num: 2 }, { k: 'AddLessAmount', t: 'AddLessAmount', num: 2 },
        { k: 'FcyAmount', t: 'FcyAmount', num: 2 }, { k: 'LcyAmount', t: 'LcyAmount', num: 2 }, { k: 'RemarksDetail', t: 'RemarksDetail', w: 160 }
    ];
    function detailCols(forHistory) {
        return DETAIL_COLS.map(function (c) {
            var o = {}; for (var k in c) o[k] = c[k];
            if (forHistory) { delete o.edit; }
            if (reverse() && o.k === 'GrnNo') o.hide = true;
            if (!forHistory && reverse() && o.k === 'Qty') o.edit = 'num';
            if (!forHistory && reverse() && o.k === 'Warehouse') o.code = true;   /* F1 picker on the warehouse cell */
            return o;
        });
    }
    var PAY_COLS = [
        { k: 'Id', hide: true }, { k: 'InvoiceId', hide: true }, { k: 'InvoiceDetailId', hide: true }, { k: 'PaymentTermId', hide: true },
        { k: 'PaymentTerm', t: 'PaymentTerm', w: 350 }, { k: 'PrcntOfTotal', t: '%OfTotal', num: 2 }, { k: 'FcyAmount', t: 'FcyAmount', num: 2 },
        { k: 'LcyAmount', t: 'LcyAmount', num: 2 }, { k: 'DueDays', t: 'DueDays' }, { k: 'AdjustmentAmount', t: 'AdjustmentAmount', num: 2, edit: 'num' },
        { k: 'Remarks', t: 'Remarks', edit: 'text', w: 160 }
    ];
    var PENDING_COLS = [
        { k: 'GrnId', hide: true }, { k: 'GrnDetailId', hide: true }, { k: 'GrnNo', t: 'GrnNo' }, { k: 'GrnDate', t: 'GrnDate', date: 'short' },
        { k: 'ExImInvoiceId', hide: true }, { k: 'InvoiceDetailId', hide: true }, { k: 'InvoiceNo', t: 'InvoiceNo' }, { k: 'InvoiceDate', hide: true },
        { k: 'LCOrderId', hide: true }, { k: 'LcOrderNo', t: 'LcOrderNo' }, { k: 'LcOrderDate', hide: true }, { k: 'SupplierCustomerId', hide: true },
        { k: 'CustomerName', t: 'CustomerName', w: 150 }, { k: 'LoadingPortId', hide: true }, { k: 'LoadingPort', t: 'LoadingPort' },
        { k: 'DestinationPortId', hide: true }, { k: 'DestinationPort', t: 'DestinationPort' }, { k: 'WarehouseId', hide: true },
        { k: 'WareHouseName', t: 'WareHouseName' }, { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 150 }, { k: 'ItemCode', t: 'ItemCode' },
        { k: 'CropYearId', hide: true }, { k: 'CropYear', t: 'CropYear' }, { k: 'JobLotId', hide: true }, { k: 'JobLotDescription', t: 'JobLotDescription' },
        { k: 'PackingTypeId', hide: true }, { k: 'PackingType', t: 'PackingType' }, { k: 'PackUomId', hide: true }, { k: 'PackUomCode', t: 'Pack Uom' },
        { k: 'PackUomEquivalent', hide: true }, { k: 'ItemQty', t: 'ItemQty', num: 3 }, { k: 'NetWeight', t: 'NetWeight', num: 3 }, { k: 'MTon', t: 'MTon', num: 3 },
        { k: 'StockWeight', t: 'StockWeight', num: 3 }, { k: 'RatePrice', t: 'RatePrice', num: 4 }, { k: 'RateUomId', hide: true }, { k: 'RateUomCode', t: 'Rate Uom' },
        { k: 'RateUomEquivalent', hide: true }, { k: 'FcyAmount', t: 'FcyAmount', num: 2 }, { k: 'RemarksDetail', t: 'RemarksDetail' },
        { k: 'EntryUser', t: 'EntryUser' }, { k: 'ModifyUser', t: 'ModifyUser' }, { k: 'ApprovedUser', t: 'ApprovedUser' }
    ];
    var HIST_COLS = [
        { k: 'Id', hide: true }, { k: 'DocNo', t: 'DocNo', code: true }, { k: 'DocDate', t: 'DocDate', date: 'short' }, { k: 'DocumentTypeId', hide: true },
        { k: 'SupplierCustomerId', hide: true }, { k: 'CustomerName', t: 'CustomerName', w: 150 }, { k: 'DebitAccountId', hide: true },
        { k: 'DebitAccountTitle', t: 'DebitAccountTitle' }, { k: 'CurrencyId', hide: true }, { k: 'CurrencyName', t: 'CurrencyName' },
        { k: 'FcyAmountWithOutAddLess', t: 'FcyAmountWithOutAddLess', num: 2 }, { k: 'AddLessAmount', t: 'AddLessAmount', num: 2 },
        { k: 'FcyAmount', t: 'FcyAmount', num: 2 }, { k: 'ExchangeRate', t: 'ExchangeRate', num: 4 }, { k: 'LcyAmount', t: 'LcyAmount', num: 2 },
        { k: 'NetWeight', t: 'NetWeight', num: 3 }, { k: 'RemarksHeader', t: 'RemarksHeader' }, { k: 'EntryDate', t: 'EntryDate', date: 'dt' },
        { k: 'EntryUser', t: 'EntryUser' }, { k: 'ModifyUser', t: 'ModifyUser' }, { k: 'ModifyDate', t: 'ModifyDate', date: 'dt' },
        { k: 'ApprovedUser', t: 'ApprovedUser' }, { k: 'ApprovedDate', t: 'ApprovedDate', date: 'dt' }, { k: 'IsApproved', t: 'IsApproved', bool: true }
    ];

    // ------------------------------------------------------------------------------ tabs / load

    function onHistory() { return $id('riHistory').classList.contains('is-active'); }
    function tabChanged(p) {
        var hist = p === 'riHistory';
        var fb = $id('btnRiFooterHistory');
        fb.querySelector('span').textContent = hist ? 'Form' : 'History';
        fb.querySelector('i').className = hist ? 'fa fa-file-text-o' : 'fa fa-history';
        /* tabControl1_SelectedIndexChanged */
        X.focus(hist ? 'fromdateHistory' : 'datVoucherDate');
    }
    function toggleHistory() { var p = onHistory() ? 'riForm' : 'riHistory'; X.selectTab('ri', p); tabChanged(p); }

    function bindGlobals(d) {
        if (d.customers) { L.customers = d.customers; X.bind('CmbRefAccountId', L.customers, 'Id', 'CompanyName', ['PartyCode', 'CityName', 'MobileNo']); }
        if (d.currencies) { L.currencies = d.currencies; X.bind('CmbCurrencyCode', L.currencies, 'Id', 'CurrencyCode'); }
        if (d.creditAccounts) { L.creditAccounts = d.creditAccounts; X.bind('CmbCreditAccount', L.creditAccounts, 'Id', 'AccountTitle', ['AccountCode', 'ParentAccountTitle', 'AccountClass']); }
        if (d.warehouses) L.warehouses = d.warehouses;
    }
    /* BindingForConfigurations. */
    function applyConfig() {
        X.show('btnInvoiceLoader', reverse());
        X.show('panelLabelPending', !reverse());
        $id('riDetailNote').textContent = reverse() ? 'Edit Qty / RatePrice in the grid - X deletes - click Warehouse = F1 picker' : 'Edit RatePrice in the grid';
    }
    function load() {
        return X.getJson(API + '/setup').then(function (d) {
            d = d || {};
            PERM = d.permissions || PERM;
            CFG = d.config || CFG;
            $id('BtnSave').disabled = !PERM.Save;
            $id('btnUpdate').disabled = !PERM.Update;
            $id('BtnPrint').disabled = !PERM.Print;
            $id('ChkBox').disabled = !PERM.Print;
            $id('ChkBox').checked = !!PERM.Print;
            $id('btnDelete').disabled = !PERM.Delete;
            if (S.recId === 0) X.setText('txtVoucherCode', d.docNo);
            applyConfig();
            bindGlobals(d);
            if (!reverse()) { S.pending = d.pending || []; S.checked = {}; drawPending(); }
            drawDetail(); drawPay();
            historyCombos(d.history || {});
            X.setText('fromdateHistory', CFG.defaultDaysToLessFromHistoryFromDate > 0 ? X.daysAgo(CFG.defaultDaysToLessFromHistoryFromDate) : X.daysAgo(3));
            X.setText('ToDateHistory', X.today());
            X.setText('datVoucherDate', X.today());
            X.setText('datInvoiceDate', X.today());
            showMode(false);
            $id('riFooterInfo').textContent = 'ExportReturnInvoice  -  Document Type 242';
            X.focus('txtVoucherCode');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    function showMode(update) {
        S.updateMode = update;
        X.show('BtnSave', !update);
        X.show('btnUpdate', update);
        X.show('btnDelete', update);
    }

    // ------------------------------------------------------------------------------ grids

    function drawDetail() {
        X.grid('grd', {
            cols: detailCols(false), rows: S.detail,
            lead: reverse() ? [{ t: 'X', html: function () { return X.btnHtml('Delete', 'X', 'win-x'); } }] : [],
            onButton: function (i, act) { if (act === 'Delete') deleteDetailRow(i); },
            onEdit: function (i, k, v) { detailCellUpdated(i, k, v); },
            onCode: function (i, k) { if (k === 'Warehouse') warehousePicker(i); },
            onKey: function (i, e) { if (e.ctrlKey && e.key === 'Delete') { e.preventDefault(); deleteDetailRow(i); } }
        });
    }
    function drawPay() {
        X.grid('grdpaymentdetail', { cols: PAY_COLS, rows: S.pay, onEdit: function (i, k, v) { payCellUpdated(i, k, v); } });
    }
    function drawPending() {
        X.grid('grdPending', {
            cols: PENDING_COLS, rows: S.pending, empty: 'grdPendingEmpty',
            lead: [{ t: 'Load', html: function () { return X.btnHtml('Load', 'Load'); } },
                   { t: 'Select', html: function (r, i) { return X.checkHtml('Select', !!S.checked[i]); } }],
            onButton: function (i, act) { if (act === 'Load') loadFromPending(); }
        });
        var tb = $id('grdPending').tBodies[0];
        if (tb) tb.addEventListener('change', function (e) {
            if (e.target.getAttribute('data-act') !== 'Select') return;
            var tr = e.target.closest('tr[data-i]'); S.checked[parseInt(tr.getAttribute('data-i'), 10)] = e.target.checked;
        });
    }

    // ------------------------------------------------------------------------------ calculations

    function setHeaderText(id, v) {
        var e = $id(id); if (!e) return;
        var changed = e.value !== str(v);
        e.value = str(v);
        /* txtTotalWeight.TextChanged is wired to txtExchangeRate_TextChanged. */
        if (changed && id === 'txtTotalWeight') exchangeRateChanged();
    }
    /* txtExchangeRate_TextChanged. */
    function exchangeRateChanged() {
        var rate = netD(X.val('txtExchangeRate')), fcy = netD(X.val('txtFcyAmount'));
        if (rate > 0 && fcy > 0) X.setText('txtRsAmount', fA(fcy * rate)); else X.setText('txtRsAmount', '0');
        addLessProportionInGrid();
    }
    /* NetFcyAmountCalculation. */
    function netFcyAmountCalculation() {
        var fcy = netD(X.val('txtFcyamountWithoutAddLess'));
        if (fcy > 0) X.setText('txtFcyAmount', fF(fcy + netD(X.val('txtAddlessAmount')))); else X.setText('txtFcyAmount', fF(fcy));
        exchangeRateChanged();
    }
    /* AddLessProportionInGrid: only a positive header add/less is spread by NetWeight; otherwise every row gets 0. */
    function addLessProportionInGrid() {
        var al = netD(X.val('txtAddlessAmount')), rate = netD(X.val('txtExchangeRate'));
        var net = 0; S.detail.forEach(function (r) { net += netD(r.NetWeight); });
        S.detail.forEach(function (r) {
            var cur = al > 0 ? al / net * netD(r.NetWeight) : 0;
            r.AddLessAmount = cur;
            r.FcyAmount = netD(r.FcyAmountWithOutAddLess) + cur;
            r.LcyAmount = r.FcyAmount * rate;
        });
        drawDetail();
    }
    /* AmountCalculation. */
    function amountCalculation() {
        var rate = netD(X.val('txtExchangeRate'));
        if (S.detail.length > 0) {
            var wo = 0, fcy = 0, lcy = 0, wt = 0;
            S.detail.forEach(function (r) {
                var amount = netD(r.FcyAmount) * rate;
                r.LcyAmount = amount;
                wo += netD(r.FcyAmountWithOutAddLess); fcy += netD(r.FcyAmount); wt += netD(r.NetWeight); lcy += amount;
            });
            X.setText('txtFcyamountWithoutAddLess', fF(wo));
            X.setText('txtFcyAmount', fF(fcy));
            X.setText('txtRsAmount', fA(lcy));
            setHeaderText('txtTotalWeight', X.opt(wt, 0, true));           /* "#,#" */
        } else {
            X.setText('txtFcyamountWithoutAddLess', '');
            X.setText('txtFcyAmount', '');
        }
        drawDetail();
    }
    /* grd_CellUpdated (Qty in the reverse flow, RatePrice always). */
    function detailCellUpdated(i, k, v) {
        var r = S.detail[i]; if (!r) return;
        r[k] = netD(v);
        var rate = netD(X.val('txtExchangeRate'));
        if (k === 'Qty' || k === 'RatePrice') {
            var net;
            if (k === 'Qty') {
                net = netD(r.Qty) * netD(r.PackUomEquivalent);
                r.NetWeight = roundEven(net); r.StockWeight = roundEven(net); r.Mton = roundEven(net / 1000);
            } else net = netD(r.NetWeight);
            var price = netD(r.RatePrice), uom = netD(r.RateUomEquivalent), fcy = 0;
            if (net > 0 && price > 0 && uom > 0) {
                var wo = net / uom * price;
                r.FcyAmountWithOutAddLess = wo;
                fcy = wo + netD(r.AddLessAmount);
                r.FcyAmount = fcy;
            } else { r.FcyAmountWithOutAddLess = 0; r.FcyAmount = 0; }
            r.LcyAmount = fcy * rate;
        }
        amountCalculation();
        /* UpdatePaymentTerms(grdpaymentdetail, ...) has an empty body on the desktop. */
    }
    /* grdpaymentdetail_CellUpdated. */
    function payCellUpdated(i, k, v) {
        var r = S.pay[i]; if (!r) return;
        if (k === 'Remarks') { r.Remarks = v; return; }
        r.AdjustmentAmount = netD(v);
        var fcy = netD(r.FcyAmount);
        if (netI(r.PaymentTermId) === 1) { r.AdjustmentAmount = fcy; drawPay(); box('you Cannot Change AdjustmentAmount For PaymentType Advance'); }
        if (netD(r.AdjustmentAmount) > fcy) { r.AdjustmentAmount = fcy; drawPay(); box("AdjustmentAmount Cannot greater Than Invoice's FcyAmount"); }
        drawPay();
    }
    /* DeleteDetailRow. */
    function deleteDetailRow(i) {
        var r = S.detail[i]; if (!r) return;
        if (netI(r.Id) > 0) {
            if (!ask('Are you sure to Delete?')) return;
            S.removed.push(r);
        }
        S.detail.splice(i, 1);
        amountCalculation();
    }
    function warehousePicker(i) {
        if (!reverse()) return;
        X.pick('WareHouseName', L.warehouses, 'Id', 'WareHouseName').then(function (p) {
            var r = S.detail[i]; if (!r) return;
            r.WarehouseId = p ? netI(p.Id) : 0; r.Warehouse = p ? str(p.WareHouseName) : '';
            drawDetail();
        });
    }

    // ------------------------------------------------------------------------------ pending GRN (normal flow)

    /* grdPending_ColumnButtonClick "Load" -> LoadDataFronGridPending. */
    function loadFromPending() {
        var idx = Object.keys(S.checked).filter(function (k) { return S.checked[k]; }).map(Number);
        if (!idx.length) { box('Please Check the Row First'); return; }
        var grnIds = [], invoiceId = 0;
        for (var j = 0; j < idx.length; j++) {
            var r = S.pending[idx[j]], cur = netI(r.ExImInvoiceId);
            if (invoiceId === 0) invoiceId = cur;
            if (invoiceId !== cur) { box('You Can Only Select Rows Of Same Invoice'); return; }
            grnIds.push(netI(r.GrnId));
        }
        if (grnIds.length) pendingDataLoadInDetail(grnIds);
        var after = function () { netFcyAmountCalculation(); addLessProportionInGrid(); amountCalculation(); };
        if (invoiceId !== 0) bindPaymentDetailByInvoiceId(invoiceId).then(after); else after();
    }
    function pendingDataLoadInDetail(grnIds) {
        if (S.detail.length > 0) {
            if (!ask('Grid Detail Already have Record,Do You Want To Reset and Load Again???')) return;
            S.detail.forEach(function (r) { if (netI(r.Id) > 0) S.removed.push(r); });
            S.detail = [];
        }
        var match = S.pending.filter(function (r) { return grnIds.indexOf(netI(r.GrnId)) >= 0; });
        if (!match.length) return;
        S.detail = [];
        var m0 = match[0];
        S.exImInvoiceId = netI(m0.ExImInvoiceId);
        X.setText('txtInvoiceNo', m0.InvoiceNo);
        X.setText('datInvoiceDate', X.isoDate(m0.InvoiceDate));
        X.setVal('CmbCurrencyCode', netI(m0.FcurrencyId));
        X.setText('txtExchangeRate', X.raw(m0.ConversionRate));
        exchangeRateChanged();
        X.setVal('CmbRefAccountId', netI(m0.SupplierCustomerId));
        S.customerGlAcId = netI(m0.CustomerGlAcId);
        X.setVal('CmbCreditAccount', netI(m0.CreditAccountId));
        match.forEach(function (r) {
            S.detail.push({ Id: 0, ExImInvoiceId: r.ExImInvoiceId, ExImInvoiceDetailId: r.InvoiceDetailId, GrnId: r.GrnId, GrnDetailId: r.GrnDetailId,
                GrnNo: r.GrnNo, WarehouseId: r.WarehouseId, Warehouse: r.WareHouseName, ItemId: r.ItemId, ItemName: r.ItemName, ItemCode: r.ItemCode,
                CropYearId: r.CropYearId, CropYear: r.CropYear, JobLotId: r.JobLotId, JobLot: r.JobLotDescription, PackingTypeId: r.PackingTypeId,
                PackingType: r.PackingType, PackUomId: r.PackUomId, PackUomCode: r.PackUomCode, PackUomEquivalent: r.PackUomEquivalent,
                Qty: r.ItemQty, NetWeight: r.NetWeight, StockWeight: r.StockWeight, Mton: r.MTon, RatePrice: r.RatePrice, RateUomId: r.RateUomId,
                RateUomCode: r.RateUomCode, RateUomEquivalent: r.RateUomEquivalent, FcyAmountWithOutAddLess: r.FcyAmount, AddLessAmount: 0,
                FcyAmount: r.FcyAmount, LcyAmount: 0, RemarksDetail: str(r.RemarksDetail) });
        });
        amountCalculation();
    }
    /* BindPaymentDetailByInvoiceId. */
    function bindPaymentDetailByInvoiceId(invoiceId) {
        S.pay = [];
        drawPay();
        return X.getJson(API + '/payment-terms?invoiceId=' + invoiceId).then(function (rows) {
            rows = rows || [];
            if (!rows.length) return;
            var rate = netD(X.val('txtExchangeRate'));
            rows.forEach(function (r) {
                S.pay.push({ Id: 0, InvoiceId: r.ExImInvoiceId, InvoiceDetailId: r.Id, PaymentTermId: r.PaymentTermId, PaymentTerm: r.PaymentTerm,
                    PrcntOfTotal: r.PrcntOfTotal, FcyAmount: r.FcyAmount, LcyAmount: netD(r.FcyAmount) * rate, DueDays: r.DueDays,
                    AdjustmentAmount: netI(r.PaymentTermId) === 1 ? r.FcyAmount : 0, Remarks: r.PaymentRemarks });
            });
            if (S.pay.length === 1) {
                var total = netD(X.val('txtFcyAmount'));
                if (total <= netD(S.pay[0].FcyAmount)) S.pay[0].AdjustmentAmount = total;
            }
            drawPay();
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ header events

    function wire() {
        X.tabs('ri', tabChanged);
        X.tabs('riDet');
        X.tabs('riHistDet');
        X.fullscreen();
        X.on('txtExchangeRate', 'input', exchangeRateChanged);
        /* txtAddlessAmount_TextChanged (KeyPress: OnlytextdecimelFunctionWithMinus). */
        X.on('txtAddlessAmount', 'input', function () {
            var e = $id('txtAddlessAmount'), v = e.value.replace(/[^0-9.\-]/g, '');
            if (v !== e.value) e.value = v;
            netFcyAmountCalculation(); addLessProportionInGrid();
        });
        document.addEventListener('keydown', keyDown);
    }

    // ------------------------------------------------------------------------------ reset / new / refresh

    /* Reset(). */
    function reset() {
        S.removed = []; S.recId = 0; showMode(false);
        S.exImInvoiceId = 0; S.customerGlAcId = 0;
        X.setText('txtInvoiceNo', ''); X.setText('datInvoiceDate', X.today());
        X.setText('txtFcyamountWithoutAddLess', ''); X.setText('txtAddlessAmount', ''); X.setText('txtFcyAmount', '');
        X.setVal('CmbCurrencyCode', 0); X.setText('txtExchangeRate', ''); X.setText('txtRsAmount', ''); X.setText('txtTotalWeight', '');
        X.setVal('CmbCreditAccount', 0); X.setText('txtRemarks', '');
        S.detail = []; S.pay = []; drawDetail(); drawPay();
        return X.getJson(API + '/reset').then(function (d) {
            X.setText('txtVoucherCode', d.docNo);
            if (!reverse()) { S.pending = d.pending || []; S.checked = {}; drawPending(); }
            X.focus('datVoucherDate');
        });
    }
    function btnNew(btn) { return X.busy(btn, function () { return reset().then(function () { X.setVal('CmbRefAccountId', 0); }); }); }
    /* btnRefresh_Click: the global services, configuration and combos (the pending grid re-binds its cached rows). */
    function btnRefresh(btn) {
        return X.busy(btn, function () {
            return X.getJson(API + '/refresh').then(function (d) {
                CFG = d.config || CFG; applyConfig(); bindGlobals(d);
                if (!reverse()) drawPending();
                drawDetail();
            });
        });
    }

    // ------------------------------------------------------------------------------ save / update / delete

    /* FormValidation. */
    function formValidation() {
        var fail = function (m, id) { box(m); X.focus(id); return false; };
        if (!X.val('txtInvoiceNo')) return fail('Invoice No Field is Required', 'txtInvoiceNo');
        if (X.vint('CmbCurrencyCode') === 0) return fail('Currency Code Field is Required', 'CmbCurrencyCode');
        if (X.val('txtExchangeRate') === '' || X.val('txtExchangeRate') === '0' || netD(X.val('txtExchangeRate')) === 0) return fail('ExchangeRate Field is Required', 'txtExchangeRate');
        if (X.val('txtFcyAmount') === '' || X.val('txtFcyAmount') === '0' || netD(X.val('txtFcyAmount')) === 0) return fail('FcyAmount Field is Required', 'txtFcyAmount');
        if (X.val('txtRsAmount') === '' || X.val('txtRsAmount') === '0' || netD(X.val('txtRsAmount')) === 0) return fail('RsAmount Field is Required', 'txtRsAmount');
        if (X.vint('CmbRefAccountId') === 0) return fail('Customer Field is Required', 'CmbRefAccountId');
        return true;
    }
    function fieldCheck(v, name, i, grid) {
        var bad = v === null || v === undefined || (typeof v === 'number' && v <= 0) || (typeof v === 'string' && v.trim() === '');
        if (bad) throw new Error(name + ' is required in ' + (grid || 'Detail Grid') + ' at row No: ' + (i + 1));
    }
    /* Insert(). */
    function insert(btn) {
        if (!S.detail.length) { box('Please Check Detail Grid'); return Promise.resolve(); }
        if (!formValidation()) return Promise.resolve();
        if (!ask(S.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return Promise.resolve();
        try {
            S.detail.forEach(function (r, i) {
                fieldCheck(netI(r.ExImInvoiceDetailId), 'Invoice Detail ID', i); fieldCheck(netI(r.ExImInvoiceId), 'Invoice ID', i);
                if (!reverse()) { fieldCheck(netI(r.GrnId), 'GRN ID', i); fieldCheck(netI(r.GrnDetailId), 'GRN Detail ID', i); }
                fieldCheck(netI(r.WarehouseId), 'Warehouse', i); fieldCheck(netI(r.ItemId), 'Item Name', i); fieldCheck(netI(r.CropYearId), 'Crop Year', i);
                fieldCheck(netI(r.JobLotId), 'Job Lot', i); fieldCheck(netI(r.PackingTypeId), 'Packing Type', i); fieldCheck(netI(r.PackUomId), 'Pack UOM', i);
                fieldCheck(netD(r.Qty), 'Qty', i); fieldCheck(netD(r.NetWeight), 'Net Weight', i); fieldCheck(netD(r.Mton), 'M.Ton', i);
                fieldCheck(netD(r.StockWeight), 'Stock Weight', i); fieldCheck(netD(r.RatePrice), 'Rate Price', i); fieldCheck(netI(r.RateUomId), 'Rate UOM', i);
                fieldCheck(netD(r.FcyAmount), 'FCY Amount', i);
            });
            if (S.pay.length) {
                var adj = 0; S.pay.forEach(function (p) { adj += netD(p.AdjustmentAmount); });
                adj = roundEven(adj); var inv = roundEven(netD(X.val('txtFcyAmount')));
                if (adj !== inv) throw new Error('Total InvoiceAmount ' + inv + ' and Total Payment UtilizeAmount ' + adj + ' Not equal Please Check');
                S.pay.forEach(function (p, i) { fieldCheck(netI(p.PaymentTermId), 'PaymentTerm', i, 'PaymentTerm Detail'); fieldCheck(netD(p.FcyAmount), 'FcyAmount', i, 'PaymentTerm Detail'); });
            }
        } catch (e) { box(e.message); return Promise.resolve(); }
        var body = {
            recId: S.recId,
            header: {
                DocNo: X.val('txtVoucherCode'), DocDate: X.val('datVoucherDate'), ExImInvoiceId: S.exImInvoiceId, CurrencyId: X.vint('CmbCurrencyCode'),
                CurrencyName: X.text('CmbCurrencyCode'), ExchangeRate: X.val('txtExchangeRate'), FcyAmountWithOutAddLess: X.val('txtFcyamountWithoutAddLess'),
                AddLessAmount: X.val('txtAddlessAmount'), FcyAmount: X.val('txtFcyAmount'), LcyAmount: X.val('txtRsAmount'), RemarksHeader: X.val('txtRemarks'),
                NetWeight: X.val('txtTotalWeight'), SupplierCustomerId: X.vint('CmbRefAccountId'), DebitAccounId: X.vint('CmbCreditAccount'), InvoiceNo: X.val('txtInvoiceNo')
            },
            rows: S.detail, removed: S.removed, payments: S.pay
        };
        var win = $id('ChkBox').checked && global.CrystalPrint ? global.CrystalPrint.reserve() : null;
        return X.busy(btn, function () {
            return X.postJson(API + '/save', body).then(function (d) {
                box(d.message);
                return reset().then(function () {
                    if (win) global.CrystalPrint.open('exp-216', { id: d.id }, null, win);
                });
            }).catch(function (e) { if (win) global.CrystalPrint.release(win); throw e; });
        });
    }
    function save(btn) { if ($id('BtnSave').classList.contains('is-hidden')) return; S.recId = 0; return insert(btn); }
    function update(btn) { return insert(btn); }
    function btnDelete(btn) {
        if (S.recId === 0) { box('Record Not Delete because RecId No Found'); return; }
        if (!ask('Are you sure to Delete?')) return;
        return X.busy(btn, function () { return X.postJson(API + '/delete', { recId: S.recId }).then(function (d) { box(d.message); return reset(); }); });
    }

    // ------------------------------------------------------------------------------ read

    /* ReadById - nothing at all without the Update right (formright.DoHaveUpdateRights). */
    function readById(id) {
        if (!PERM.Update) return Promise.resolve();
        return reset().then(function () {
            return X.getJson(API + '/by-id?edit=true&id=' + id);
        }).then(function (d) {
            if (!d || d.noRight) return;
            S.recId = id;
            var h = d.header;
            X.selectTab('ri', 'riForm'); tabChanged('riForm');
            showMode(true);
            X.setText('txtVoucherCode', h.DocNo);
            X.setText('datVoucherDate', X.isoDate(h.DocDate));
            S.exImInvoiceId = netI(h.ExImInvoiceId);
            X.setText('txtInvoiceNo', h.InvoiceNo);
            X.setText('datInvoiceDate', X.isoDate(h.InvoiceDate));
            X.setText('txtFcyamountWithoutAddLess', fF(h.FcyAmountWithOutAddLess));
            X.setText('txtAddlessAmount', fF(h.AddLessAmount));
            X.setText('txtFcyAmount', fF(h.FcyAmount));
            X.setVal('CmbCurrencyCode', h.CurrencyId);
            X.setText('txtExchangeRate', X.opt(h.ExchangeRate, 4, true));     /* "#,#.####" */
            X.setText('txtRsAmount', fA(h.LcyAmount));
            X.setText('txtRemarks', h.RemarksHeader);
            X.setText('txtTotalWeight', X.opt(h.NetWeight, 4, true));         /* "##,#.####" */
            X.setVal('CmbRefAccountId', h.SupplierCustomerId);
            X.setVal('CmbCreditAccount', h.DebitAccounId);
            S.detail = d.detail || []; S.pay = d.payments || [];
            drawDetail(); drawPay();
            exchangeRateChanged();
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ prints

    function noRecord(id) { if (netI(id) === 0) { box('Not Record Found For Display'); return true; } return false; }
    function print216(btn, id) {
        id = id === undefined ? S.recId : id;
        if (noRecord(id)) return;
        if (!global.CrystalPrint) { box('Print is not available.'); return; }
        return global.CrystalPrint.open('exp-216', { id: id }, btn);
    }
    /* ExportReturnVoucherSlip(PrintId). */
    function print102(btn, id) {
        id = id === undefined ? S.recId : id;
        if (noRecord(id)) return;
        var win = global.CrystalPrint ? global.CrystalPrint.reserve() : null;
        return X.busy(btn, function () {
            return X.getJson(API + '/voucher-head?id=' + id).then(function (d) {
                if (!netI(d.voucherHeadId) || !win) { if (win) global.CrystalPrint.release(win); box('Not Record Found For Display'); return; }
                return global.CrystalPrint.open('exp-102', { id: d.voucherHeadId }, null, win);
            }).catch(function (e) { if (win) global.CrystalPrint.release(win); throw e; });
        });
    }
    function attachment() { box('Attachments are not available in the web version.'); }

    // ------------------------------------------------------------------------------ history

    function historyCombos(d) {
        X.bind('CmbCustomerHistory', d.customers || [], 'Id', 'Name');
        X.bind('CmbDebitAccountHistory', d.debitAccounts || [], 'Id', 'Name');
        X.bind('CmbInvoiceNoHistory', d.invoices || [], 'Id', 'Name');
    }
    function historyShow(btn) {
        return X.busy(btn, function () {
            return X.postJson(API + '/history', {
                fromChecked: $id('fromdateHistoryChk').checked, fromDate: X.val('fromdateHistory'),
                toChecked: $id('ToDateHistoryChk').checked, toDate: X.val('ToDateHistory'),
                fromDocNo: X.val('txtFromNoHistory'), toDocNo: X.val('txtToDocNoHistory'),
                debitAccountId: X.vint('CmbDebitAccountHistory'), exImInvoiceId: X.vint('CmbInvoiceNoHistory'), supplierCustomerId: X.vint('CmbCustomerHistory')
            }).then(function (rows) {
                H.rows = rows || []; H.cur = -1;
                drawHistory();
                if (!H.rows.length) { H.detail = []; H.pay = []; drawHistoryDetail(); }
            });
        });
    }
    function drawHistory() {
        var cols = HIST_COLS.slice();
        X.grid('DataGridHistory', {
            cols: cols, rows: H.rows, current: H.cur, empty: 'DataGridHistoryEmpty',
            lead: [{ t: 'Edit', html: function () { return X.btnHtml('Edit', 'Edit'); } },
                   { t: 'Print', html: function () { return X.btnHtml('Print', 'Print'); } },
                   { t: 'Voucher', html: function () { return X.btnHtml('Voucher', 'Voucher'); } },
                   { t: 'Attached', html: function (r) { return '<a class="win-code" data-code="NoOfAttachments">' + X.esc(r.NoOfAttachments) + '</a>'; } }],
            onRow: function (i) { H.cur = i; historySelect(i); },
            onDbl: function (i) { readById(netI(H.rows[i].Id)); },
            onCode: function (i, k) { if (k === 'NoOfAttachments') attachment(); else readById(netI(H.rows[i].Id)); },
            onButton: function (i, act, b) {
                var id = netI(H.rows[i].Id);
                if (act === 'Edit') readById(id);
                if (act === 'Print') print216(b, id);
                if (act === 'Voucher') print102(b, id);
            }
        });
    }
    /* DataGridHistory_SelectionChanged -> GetDetailGrdByHeadId. */
    function historySelect(i) {
        var id = netI(H.rows[i].Id);
        return X.getJson(API + '/by-id?id=' + id).then(function (d) {
            H.detail = (d && d.detail) || []; H.pay = (d && d.payments) || [];
            drawHistoryDetail();
        }).catch(function (e) { box(e.message); });
    }
    function drawHistoryDetail() {
        X.grid('DataGridHistoryDetail', { cols: detailCols(true), rows: H.detail });
        X.grid('grdPaymentHistory', { cols: PAY_COLS.map(function (c) { var o = {}; for (var k in c) o[k] = c[k]; delete o.edit; return o; }), rows: H.pay });
    }
    /* ResetHistory (From = today - 3, not the configured days). */
    function historyReset() {
        X.setText('fromdateHistory', X.daysAgo(3)); X.setText('ToDateHistory', X.today());
        X.setText('txtFromNoHistory', ''); X.setText('txtToDocNoHistory', '');
        X.setVal('CmbInvoiceNoHistory', 0); X.setVal('CmbDebitAccountHistory', 0); X.setVal('CmbCustomerHistory', 0);
        H.rows = []; H.detail = []; H.pay = []; drawHistory(); drawHistoryDetail();
    }
    function historyRefresh(btn) { return X.busy(btn, function () { return X.getJson(API + '/history-combos').then(historyCombos); }); }

    // ------------------------------------------------------------------------------ frmLoadCommercialInvoiceForReturn

    var LD_MAIN_COLS = [
        { k: 'ExImInvoiceId', hide: true }, { k: 'InvoiceNo', t: 'InvoiceNo' }, { k: 'InvoiceDate', t: 'InvoiceDate', date: 'short' }, { k: 'SupplierCustomerId', hide: true },
        { k: 'CustomerName', t: 'CustomerName', w: 160 }, { k: 'InvoiceQty', t: 'InvoiceQty', num: 3 }, { k: 'UsedQty', t: 'UsedQty', num: 3 }, { k: 'BalQty', t: 'BalQty', num: 3 },
        { k: 'InvoiceWeight', t: 'InvoiceWeight', num: 3 }, { k: 'UsedWeight', t: 'UsedWeight', num: 3 }, { k: 'BalWeight', t: 'BalWeight', num: 3 },
        { k: 'InvoiceAmount', t: 'InvoiceAmount', num: 3 }, { k: 'UsedAmount', t: 'UsedAmount', num: 3 }, { k: 'BalAmount', t: 'BalAmount', num: 3 }
    ];
    var LD_DETAIL_COLS = [
        { k: 'ExImInvoiceId', hide: true }, { k: 'InvoiceDetailId', hide: true }, { k: 'InvoiceNo', t: 'InvoiceNo' }, { k: 'InvoiceDate', t: 'InvoiceDate', date: 'short' },
        { k: 'SupplierCustomerId', hide: true }, { k: 'CustomerName', t: 'CustomerName', w: 160 }, { k: 'ContractId', hide: true }, { k: 'ContractNo', t: 'ContractNo' },
        { k: 'ContractNoShipmentWise', t: 'ContractNoShipmentWise' }, { k: 'JobLotId', hide: true }, { k: 'JobLotDescription', t: 'JobLotDescription' },
        { k: 'CropYearId', hide: true }, { k: 'CropYear', t: 'CropYear' }, { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 150 },
        { k: 'PackingMaterialTypeId', hide: true }, { k: 'PackingType', t: 'PackingType' }, { k: 'PackUomId', hide: true }, { k: 'PackUom', t: 'PackUom' },
        { k: 'PackEquivalent', hide: true }, { k: 'InvoiceQty', t: 'InvoiceQty', num: 3 }, { k: 'UsedQty', t: 'UsedQty', num: 3 }, { k: 'BalQty', t: 'BalQty', num: 3 },
        { k: 'InvoiceWeight', t: 'InvoiceWeight', num: 3 }, { k: 'UsedWeight', t: 'UsedWeight', num: 3 }, { k: 'BalWeight', t: 'BalWeight', num: 3 },
        { k: 'InvoiceAmount', t: 'InvoiceAmount', num: 3 }, { k: 'UsedAmount', t: 'UsedAmount', num: 3 }, { k: 'BalAmount', t: 'BalAmount', num: 3 }
    ];
    function loaderOpen(btn) {
        if (!reverse()) return;
        return X.busy(btn, function () {
            return X.getJson(API + '/loader/setup').then(function (d) {
                X.setText('ldFromDate', X.isoDate(d.fromDate) || X.today());
                X.setText('ldTodate', X.today());
                loaderCombos(d.combos || {});
                X.modal('loaderModal', true);
                return loaderRows();
            });
        });
    }
    function loaderCombos(c) {
        X.bind('ldCmbInvoiceNo', c.Invoice || [], 'Id', 'name'); X.bind('ldCmbContractNo', c.ContractNo || [], 'Id', 'name');
        X.bind('ldCmbCustomer', c.Customer || [], 'Id', 'name'); X.bind('ldCmbItem', c.Items || [], 'Id', 'name');
        X.bind('ldCmbJobLot', c.JobLot || [], 'Id', 'name'); X.bind('ldCmbCrop', c.Crop || [], 'Id', 'name');
    }
    /* GridRecordsDBCall + GridRecordsFill (grouped by ExImInvoiceId, sums per group). */
    function loaderRows() {
        return X.postJson(API + '/loader/rows', {
            fromDate: X.val('ldFromDate'), toDate: X.val('ldTodate'), exImInvoiceId: X.vint('ldCmbInvoiceNo'), contractId: X.vint('ldCmbContractNo'),
            supplierCustomerId: X.vint('ldCmbCustomer'), itemId: X.vint('ldCmbItem'), jobLotId: X.vint('ldCmbJobLot'), cropYearId: X.vint('ldCmbCrop')
        }).then(function (rows) {
            LD.rows = rows || []; LD.main = []; LD.detail = []; LD.checked = {}; LD.mainCur = -1;
            var map = {};
            LD.rows.forEach(function (r) {
                var k = netI(r.ExImInvoiceId), g = map[k];
                if (!g) { g = map[k] = { ExImInvoiceId: k, InvoiceNo: r.InvoiceNo, InvoiceDate: r.InvoiceDate, SupplierCustomerId: r.SupplierCustomerId, CustomerName: r.CustomerName,
                    InvoiceQty: 0, UsedQty: 0, BalQty: 0, InvoiceWeight: 0, UsedWeight: 0, BalWeight: 0, InvoiceAmount: 0, UsedAmount: 0, BalAmount: 0 }; LD.main.push(g); }
                ['InvoiceQty', 'UsedQty', 'BalQty', 'InvoiceWeight', 'UsedWeight', 'BalWeight', 'InvoiceAmount', 'UsedAmount', 'BalAmount'].forEach(function (c) { g[c] += netD(r[c]); });
            });
            drawLoader();
        });
    }
    function drawLoader() {
        X.grid('ldGrdMain', { cols: LD_MAIN_COLS, rows: LD.main, current: LD.mainCur, onRow: function (i) { LD.mainCur = i; loaderMainSelect(i); } });
        X.grid('ldGrdDetail', {
            cols: LD_DETAIL_COLS, rows: LD.detail,
            lead: [{ t: 'Select', html: function (r, i) { return X.checkHtml('Select', !!LD.checked[i]); } }]
        });
        var tb = $id('ldGrdDetail').tBodies[0];
        if (tb) tb.addEventListener('change', function (e) {
            if (e.target.getAttribute('data-act') !== 'Select') return;
            var tr = e.target.closest('tr[data-i]'); LD.checked[parseInt(tr.getAttribute('data-i'), 10)] = e.target.checked; loaderTotals();
        });
        loaderTotals();
    }
    /* HandleMainRowSelectionOrCheck: DetailGridBind(Id) + CheckedAllDetailRows. */
    function loaderMainSelect(i) {
        var id = netI(LD.main[i].ExImInvoiceId);
        LD.detail = LD.rows.filter(function (r) { return netI(r.ExImInvoiceId) === id; }).map(function (r) {
            var o = {}; for (var k in r) o[k] = r[k]; o.PackingType = r.PackTypeDesc; return o;
        });
        LD.checked = {}; LD.detail.forEach(function (r, j) { LD.checked[j] = true; });
        drawLoader();
    }
    /* CalculateTotal over the checked detail rows. */
    function loaderTotals() {
        var t = { InvoiceQty: 0, InvoiceWeight: 0, InvoiceAmount: 0, UsedQty: 0, UsedWeight: 0, UsedAmount: 0, BalQty: 0, BalWeight: 0, BalAmount: 0 };
        LD.detail.forEach(function (r, i) { if (LD.checked[i]) for (var k in t) t[k] += netD(r[k]); });
        X.setText('txtOrderQtyHeader', fA(t.InvoiceQty)); X.setText('txtQrderWeightHeader', fA(t.InvoiceWeight)); X.setText('txtQrderAmountHeader', fA(t.InvoiceAmount));
        X.setText('txtUsedQtyHeader', fA(t.UsedQty)); X.setText('txtUsedWeightHeader', fA(t.UsedWeight)); X.setText('txtUsedAmountHeader', fA(t.UsedAmount));
        X.setText('txtBalanceQtyHeader', fA(t.BalQty)); X.setText('txtBalanceWeightHeader', fA(t.BalWeight)); X.setText('txtBalanceAmountHeader', fA(t.BalAmount));
    }
    function loaderShow(btn) { return X.busy(btn, loaderRows); }
    function loaderReset(btn) {
        ['ldCmbCustomer', 'ldCmbInvoiceNo', 'ldCmbContractNo', 'ldCmbItem', 'ldCmbJobLot', 'ldCmbCrop'].forEach(function (c) { X.setVal(c, 0); });
        return X.busy(btn, loaderRows);
    }
    function loaderRefresh(btn) { return X.busy(btn, function () { return X.getJson(API + '/loader/setup').then(function (d) { loaderCombos(d.combos || {}); }); }); }
    function loaderClose() { X.modal('loaderModal', false); }      /* Esc / Ctrl+E: dtLoader = null -> nothing loaded */
    /* btnLoadOnInvoice_Click_1 -> LoadInGridDetail. */
    function loaderLoad() {
        var picked = LD.detail.filter(function (r, i) { return LD.checked[i]; });
        if (!picked.length) { box('Please check at least one row.'); return; }
        var inv = {}; picked.forEach(function (r) { inv[netI(r.ExImInvoiceId)] = 1; });
        if (Object.keys(inv).length > 1) { box('You can only select rows from the same Invoice.'); return; }
        var loader = picked.map(function (r) { return LD.rows.filter(function (x) { return netI(x.InvoiceDetailId) === netI(r.InvoiceDetailId); })[0]; }).filter(Boolean);
        X.modal('loaderModal', false);
        loadInGridDetail(loader);
    }
    function loadInGridDetail(rows) {
        if (!rows.length) return;
        var dr = rows[0], invoiceId = netI(dr.ExImInvoiceId);
        if (S.exImInvoiceId > 0 && S.exImInvoiceId !== invoiceId) { box('Data Against Another Invoice Already Loaded'); return; }
        S.exImInvoiceId = invoiceId;
        X.setText('txtInvoiceNo', dr.InvoiceNo);
        X.setText('datInvoiceDate', X.isoDate(dr.InvoiceDate));
        X.setText('txtAddlessAmount', X.fixed(dr.AddLessAmount, 0));          /* "#,#0" */
        X.setVal('CmbCurrencyCode', netI(dr.FCurrencyId));
        X.setText('txtExchangeRate', X.opt(dr.ExchRate, 2));                   /* "#,#0.##" */
        X.setVal('CmbRefAccountId', netI(dr.SupplierCustomerId));
        var rate = netD(X.val('txtExchangeRate'));
        rows.forEach(function (r) {
            var exists = S.detail.some(function (d) { return netI(d.ExImInvoiceDetailId) === netI(r.InvoiceDetailId); });
            if (exists) return;
            S.detail.push({ Id: 0, ExImInvoiceId: r.ExImInvoiceId, ExImInvoiceDetailId: r.InvoiceDetailId, GrnId: 0, GrnDetailId: 0, GrnNo: 0, WarehouseId: 0, Warehouse: '',
                ItemId: r.ItemId, ItemName: r.ItemName, ItemCode: r.ItemCode, CropYearId: r.CropYearId, CropYear: r.CropYear, JobLotId: r.JobLotId, JobLot: r.JobLotDescription,
                PackingTypeId: r.PackingMaterialTypeId, PackingType: r.PackTypeDesc, PackUomId: r.PackUomId, PackUomCode: r.PackUom, PackUomEquivalent: r.PackEquivalent,
                Qty: r.BalQty, NetWeight: r.BalWeight, StockWeight: r.BalWeight, Mton: netD(r.BalWeight) / 1000, RatePrice: r.RatePrice, RateUomId: r.RateUomId,
                RateUomCode: r.RateUomCode, RateUomEquivalent: r.RateUomEquivalent, FcyAmountWithOutAddLess: r.BalAmount, AddLessAmount: 0, FcyAmount: r.BalAmount,
                LcyAmount: netD(r.BalAmount) * rate, RemarksDetail: '' });
        });
        drawDetail();
        bindPaymentDetailByInvoiceId(invoiceId).then(function () { netFcyAmountCalculation(); addLessProportionInGrid(); amountCalculation(); });
    }

    // ------------------------------------------------------------------------------ shortcuts

    var SHORTCUTS = [['Ctrl+S', 'For Save and for Show on history'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+E', 'For Close'],
        ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print 242 Slip'], ['Alt+1', 'For Print 242 Slip'], ['Ctrl+F5', 'For Focus on DocDate'],
        ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+D', 'For Adding an row in Focused Grid'],
        ['Ctrl+Delete', 'For Deleting an row of Focused Grid'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On Exchange Rate'],
        ['Ctrl+ArrowRight', "For Change Focus from one Grid To another Grid"], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function shortcuts() { X.shortcuts(SHORTCUTS); }
    function loaderShortcuts() {
        X.shortcuts([['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+L', 'To Press Load Button'], ['Ctrl+S', 'For Search'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }
    function vis(id) { var e = $id(id); return e && !e.classList.contains('is-hidden') && !e.disabled; }
    /* ExportReturnInvoice_KeyDown (and the loader's LoadPurchaseOrder_KeyDown while it is open). */
    function keyDown(e) {
        var k = (e.key || '').toLowerCase();
        if (!$id('loaderModal').classList.contains('is-hidden')) {
            if (k === 'escape' || (e.ctrlKey && k === 'e')) { e.preventDefault(); loaderClose(); }
            else if (e.ctrlKey && e.altKey) loaderShortcuts();
            else if (e.ctrlKey && k === 's') { e.preventDefault(); loaderShow($id('ldBtnShow')); }
            else if (e.ctrlKey && k === 'l') { e.preventDefault(); loaderLoad(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); loaderReset($id('ldBtnNew')); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); loaderRefresh($id('ldBtnRefresh')); }
            return;
        }
        if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); return; }
        if (k === 'escape' || (e.ctrlKey && k === 'e')) { e.preventDefault(); X.cancel(); return; }
        if (e.ctrlKey && e.altKey) { shortcuts(); return; }
        if (!onHistory()) {
            if (e.ctrlKey && e.shiftKey && k === 'delete' && vis('btnDelete')) { e.preventDefault(); btnDelete($id('btnDelete')); }
            else if (e.ctrlKey && k === 's' && vis('BtnSave')) { e.preventDefault(); save($id('BtnSave')); }
            else if (e.ctrlKey && k === 'u' && vis('btnUpdate')) { e.preventDefault(); update($id('btnUpdate')); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew($id('BtnNew')); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); btnRefresh($id('btnRefresh')); }
            else if (e.ctrlKey && k === 'f5') { e.preventDefault(); X.focus('datVoucherDate'); }
            else if (e.ctrlKey && k === 'arrowup') { e.preventDefault(); X.focus('txtExchangeRate'); }
            else if (e.ctrlKey && k === 'f10') { e.preventDefault(); attachment(); }
            else if ((e.ctrlKey && k === 'p') || (e.altKey && k === '1')) { e.preventDefault(); print216($id('BtnPrint')); }
            return;
        }
        if (e.ctrlKey && k === 's') { e.preventDefault(); historyShow($id('btnshowHistory')); }
        else if (e.ctrlKey && k === 'f5') { e.preventDefault(); X.focus('fromdateHistory'); }
        else if (e.ctrlKey && k === 'n') { e.preventDefault(); historyReset(); }
        else if (e.ctrlKey && k === 'r') { e.preventDefault(); historyRefresh($id('BtnRefreshHistory')); }
        else if (e.ctrlKey && k === 'enter' && H.cur >= 0) { e.preventDefault(); readById(netI(H.rows[H.cur].Id)); }
        else if (e.ctrlKey && k === 'p' && H.cur >= 0 && PERM.Print) { e.preventDefault(); print216(null, netI(H.rows[H.cur].Id)); }
    }

    global.ExportReturnInvoice = {
        btnNew: btnNew, btnRefresh: btnRefresh, save: save, update: update, btnDelete: btnDelete, attachment: attachment,
        print216: function (b) { return print216(b); }, print102: function (b) { return print102(b); }, shortcuts: shortcuts, toggleHistory: toggleHistory,
        historyShow: historyShow, historyReset: historyReset, historyRefresh: historyRefresh,
        loaderOpen: loaderOpen, loaderClose: loaderClose, loaderShow: loaderShow, loaderReset: loaderReset, loaderRefresh: loaderRefresh,
        loaderLoad: loaderLoad, loaderShortcuts: loaderShortcuts
    };

    document.addEventListener('DOMContentLoaded', function () { wire(); load(); });
}(window));
