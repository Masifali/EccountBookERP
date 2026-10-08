/*
 * Screen 535  SaleInvoiceTrading_Engr  (Architecture.WinApp.SaleTrading.SaleInvoiceTrading_Engr, document type 1609)
 * Page script. Desktop methods are named in the comments (siT.cs). Server: /sale/engr/sale-invoice/api
 * The invoice arithmetic lives on the server (POST /calc, the same code Save runs); this page runs the cell logic of the small grids.
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/engr/sale-invoice/api', DOC = 1609;
    var S = {
        id: 0, rights: {}, files: [], removedAtt: [], existing: [], comm: [], headComm: 0, voucherHeadId: 0, deliveryTypeId: 0,
        cust: [], terms: [], other: [], fr: [], taxAc: [], discAc: [], commAc: [], curr: [], taxTypes: [], dterms: [],
        sub: false, multi: false, taxEditable: false, commDebit: false, fmt: { amountRound: 0, amount: 0, rate: 2, fcy: 0 }, def: {}, branchId: 0, fyStart: ''
    };
    var cb = {}, G = {}, tabs, seq = 0;

    function fail(e) { return SE.dbError(e); }
    function msg(t, c) { return SE.alert(t, c); }
    function ci(r, k) { if (!r) return null; if (k in r) return r[k]; var l = String(k).toLowerCase(); for (var x in r) if (x.toLowerCase() === l) return r[x]; return null; }
    function pick(r, ks) { for (var i = 0; i < ks.length; i++) { var v = ci(r, ks[i]); if (v != null) return v; } return null; }
    function enable(c, on) { c.el.disabled = !on; var w = c.el.__dtcombo; if (w && w.syncFromSelect) w.syncFromSelect(); }
    function isShown(id) { var b = $(id); return !!b && b.style.display !== 'none' && !b.hidden; }
    function show(id, on) { var b = $(id); if (!b) return; b.style.display = on ? '' : 'none'; if (b.hasAttribute('hidden')) b.hidden = !on; }
    function vid(c) { return +c.value() || 0; }
    function num(id) { return SE.toNum($(id).value); }
    function fmtAmt(v) { var d = S.fmt.amount; return SE.num(v, d, d); }
    function fmtRate(v) { var d = S.fmt.rate; return SE.num(v, d, d); }
    function fmtFcy(v) { var d = S.fmt.fcy; return SE.num(v, d, d); }
    function f2(v) { return SE.num(v, 2, 0); }
    function f3(v) { return SE.num(v, 3, 0); }
    function f4(v) { return SE.num(v, 4, 0); }
    function rnd(v, dec) { var p = Math.pow(10, Math.max(0, dec)); return (v < 0 ? -1 : 1) * Math.round(Math.abs(v) * p + 1e-9) / p; }
    function iso(d) { return d.getFullYear() + '-' + SE.pad2(d.getMonth() + 1) + '-' + SE.pad2(d.getDate()); }
    function parseIso(s) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(s || ''); return m ? new Date(+m[1], +m[2] - 1, +m[3]) : null; }
    function addDays(s, n) { var d = parseIso(s) || new Date(); d.setDate(d.getDate() + n); return iso(d); }
    function dayDiff(a, b) { var x = parseIso(a), y = parseIso(b); return x && y ? Math.round((x - y) / 86400000) : 0; }
    var MON = { jan: 1, feb: 2, mar: 3, apr: 4, may: 5, jun: 6, jul: 7, aug: 8, sep: 9, oct: 10, nov: 11, dec: 12 };
    function readDate(t) {
        t = String(t || '').trim(); var m;
        if ((m = /^(\d{4})-(\d{1,2})-(\d{1,2})/.exec(t))) return m[1] + '-' + SE.pad2(+m[2]) + '-' + SE.pad2(+m[3]);
        if ((m = /^(\d{1,2})[-\/ ]([A-Za-z]{3})[A-Za-z]*[-\/ ](\d{4})/.exec(t)) && MON[m[2].toLowerCase()]) return m[3] + '-' + SE.pad2(MON[m[2].toLowerCase()]) + '-' + SE.pad2(+m[1]);
        if ((m = /^(\d{1,2})[-\/](\d{1,2})[-\/](\d{4})/.exec(t))) return m[3] + '-' + SE.pad2(+m[2]) + '-' + SE.pad2(+m[1]);
        return '';
    }
    function isNumeric(t) { return /^\s*-?\d+(\.\d+)?\s*$/.test(String(t).replace(/,/g, '')); }

    /* ------------------------------------------------------------------ combos */
    function makeCombos() {
        cb.cust = XCombo('cmbsuppliername', { columns: [{ key: 'CompanyName', caption: 'Customer Name' }], textKey: 'CompanyName', popupWidth: 420, onSelect: recalc });
        cb.dterm = XCombo('cmbDeliveryTerm', { columns: [{ key: 'DeliveryTerm', caption: 'Delivery Term' }], textKey: 'DeliveryTerm' });
        cb.term = XCombo('CmbPaymentTerm', { columns: [{ key: 'TermsDescription', caption: 'Payment Term' }], textKey: 'TermsDescription', onSelect: termChanged });
        cb.tax = XCombo('CmbTaxAccount', { columns: [{ key: 'AccountTitle', caption: 'Account Title' }], textKey: 'AccountTitle', popupWidth: 360 });
        cb.frCr = XCombo('CmbFreightAcCr', { columns: [{ key: 'AccountTitle', caption: 'Account Title' }], valueKey: 'K', textKey: 'AccountTitle', popupWidth: 360, onSelect: function () { freightChanged('cr'); } });
        cb.frDr = XCombo('cmbFreightAcDr', { columns: [{ key: 'AccountTitle', caption: 'Account Title' }], valueKey: 'K', textKey: 'AccountTitle', popupWidth: 360, onSelect: function () { freightChanged('dr'); } });
        cb.disc = XCombo('CmbDiscountAccount', { columns: [{ key: 'AccountTitle', caption: 'Account Title' }], textKey: 'AccountTitle', popupWidth: 360 });
        cb.cur = XCombo('cmbCurrency', { columns: [{ key: 'CurrencyCode', caption: 'Currency' }], textKey: 'CurrencyCode', onSelect: currencyLeave });
        cb.commDr = XCombo('CmbCommissionDebitAc', { columns: [{ key: 'AccountTitle', caption: 'Account Title' }], textKey: 'AccountTitle', popupWidth: 360 });
        cb.hCust = XCombo('CmbCustomerHistory', { columns: [{ key: 'CustomerName', caption: 'Customer Name' }], valueKey: 'Id', textKey: 'CustomerName', popupWidth: 380 });
    }
    function keepFill(c, rows, keepId) { c.setData(rows); if (keepId > 0 && !c.setValue(keepId)) c.clear(); }
    function applyLists(d) {
        S.sub = !!d.subsidiary; S.multi = !!d.multiCurrency; S.taxEditable = !!d.taxEditable; S.commDebit = !!d.commissionDebitToExpenses;
        S.cust = d.customers || []; S.terms = d.terms || []; S.other = d.otherItems || []; S.taxAc = d.taxAccounts || []; S.discAc = d.discountAccounts || [];
        S.commAc = d.commissionDebitAccounts || []; S.curr = d.currencies || []; S.taxTypes = d.taxTypes || []; S.dterms = d.deliveryTerms || [];
        S.fr = (d.freightAccounts || []).map(function (r) { var o = {}; for (var k in r) o[k] = r[k]; o.K = S.sub ? r.SupplierCustomerId : r.Id; return o; });
        S.def = d.defaults || {}; S.fmt = d.fmt || S.fmt;
        G.grd.spec.dec = S.fmt.amount;
        G.hist.spec.dec = S.fmt.amount; G.hd.spec.dec = S.fmt.amount;
    }
    function fillAll(first) {
        var a = vid(cb.cust), t = vid(cb.term), tx = vid(cb.tax), fc = vid(cb.frCr), fd = vid(cb.frDr), dc = vid(cb.disc), cu = vid(cb.cur), cd = vid(cb.commDr);
        keepFill(cb.cust, S.cust, a);
        cb.term.setData(S.terms); if (t > 0 && !cb.term.setValue(t)) cb.term.clear();
        cb.dterm.setData(S.dterms);
        keepFill(cb.tax, S.taxAc, tx); keepFill(cb.frCr, S.fr, fc); keepFill(cb.frDr, S.fr, fd); keepFill(cb.disc, S.discAc, dc);
        keepFill(cb.cur, S.curr, cu); keepFill(cb.commDr, S.commAc, cd);
        G.exp.refresh(); G.gl.refresh(); G.pay.refresh(); G.grd.refresh();
    }
    function multiCurrency() {
        ['txtExchangeRate', 'txtFcyAmount', 'label43', 'label44', 'label45'].forEach(function (id) { var e = $(id); if (e) e.style.visibility = S.multi ? '' : 'hidden'; });
        var c = $('cmbCurrency'); if (c) { c.style.visibility = S.multi ? '' : 'hidden'; if (c.__dtcombo && c.__dtcombo.wrap) c.__dtcombo.wrap.style.visibility = S.multi ? '' : 'hidden'; }
        var col = G.grd.spec.cols.filter(function (x) { return x.k === 'FcyAmount'; })[0]; if (col) col.hide = !S.multi;
        var hc = G.hd.spec.cols.filter(function (x) { return x.k === 'FcyAmount'; })[0]; if (hc) hc.hide = !S.multi;
        G.grd.refresh(); G.hd.refresh();
    }
    function commissionDebitUi() {
        var c = $('CmbCommissionDebitAc'); if (c) { c.style.visibility = S.commDebit ? '' : 'hidden'; if (c.__dtcombo && c.__dtcombo.wrap) c.__dtcombo.wrap.style.visibility = S.commDebit ? '' : 'hidden'; }
        var l = $('lblDebitAccount'); if (l) l.style.visibility = S.commDebit ? '' : 'hidden';
    }
    function configDefault() {                                                               // ConfigurationDefault
        if (S.def.baseCurrency > 0 && !(vid(cb.cur) > 0)) cb.cur.setValue(S.def.baseCurrency);
        if (S.def.baseRate > 0 && num('txtExchangeRate') === 0) $('txtExchangeRate').value = fmtRate(S.def.baseRate);
    }
    function custRow() { var id = vid(cb.cust); for (var i = 0; i < S.cust.length; i++) if (+S.cust[i].Id === id) return S.cust[i]; return null; }

    /* ------------------------------------------------------------------ server calculation */
    function reqBody(withAtt) {
        var cr = custRow();
        var b = {
            id: S.id, docNo: $('txtdocno').value, docDate: $('DocDate').value, customerId: vid(cb.cust), customerGlId: cr ? +cr.GlAccountId || 0 : 0,
            deliveryTerm: cb.dterm.text() || '', paymentTermId: vid(cb.term), dueDays: $('txtDueDays').value, dueDate: $('DueDate').value, currencyId: vid(cb.cur),
            exchangeRate: num('txtExchangeRate'), taxAccountId: vid(cb.tax), freightCrId: vid(cb.frCr), freightDrId: vid(cb.frDr), freightAmount: num('txtFreightAmount'),
            freightRemarks: $('txtFreightRemarks').value, discountAccountId: vid(cb.disc), discountAmount: num('txtDiscountAmountHeader'), manualBillNo: $('txtbillno').value,
            refNo: $('txtrefno').value, remarks: $('txtRemarks').value, deliveryTypeId: S.deliveryTypeId, headCommAmount: S.headComm,
            lines: G.grd.rows(), expenses: G.exp.rows(), journals: G.gl.rows(), payments: G.pay.rows(), comm: S.comm
        };
        if (withAtt) b.attachments = { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removedAtt };
        return b;
    }
    function applyCalc(r) {
        S.comm = r.comm || S.comm;
        G.grd.setRows(r.lines || []);
        $('txtBillAmount').value = f4(+r.billAmount || 0);
        $('txtFcyAmount').value = fmtFcy(+r.fcyAmount || 0);
        G.grd.refresh();
    }
    /* CalculateBillAmount + proportions + txtExchangeRate_TextChanged, as Save runs them */
    function recalc() {
        var my = ++seq;
        return SE.api(API + '/calc', { method: 'POST', body: reqBody(false), quiet: true }).then(function (r) { if (my === seq) applyCalc(r); }).catch(function (e) { if (my === seq) fail(e); });
    }
    function billAmount() { return num('txtBillAmount'); }
    function itemTotal() { return G.grd.rows().reduce(function (a, r) { return a + (+r.ItemAmount || 0); }, 0); }

    /* ------------------------------------------------------------------ header events */
    function termChanged() {                                                                 // CmbPaymentTerm change
        if (vid(cb.term) === 1) { $('txtDueDays').disabled = true; $('txtDueDays').value = '0'; $('DueDate').value = $('DocDate').value; }
        else $('txtDueDays').disabled = false;
    }
    function dueDaysChanged() { var t = $('txtDueDays').value; $('DueDate').value = addDays($('DocDate').value, SE.toInt(t)); }
    function currencyLeave() {                                                               // cmbCurrency_Leave
        if (vid(cb.cur) === 0 || num('txtExchangeRate') !== 0) return;
        if (vid(cb.cur) !== S.def.baseCurrency) {
            return SE.api(API + '/last-rate' + SE.q({ currencyId: vid(cb.cur) })).then(function (dt) {
                $('txtExchangeRate').value = dt.length ? fmtRate(+pick(dt[0], ['LastExchRate']) || 0) : '0'; recalc();
            }).catch(fail);
        }
        $('txtExchangeRate').value = fmtRate(S.def.baseRate || 0); recalc();
    }
    function freightChanged(side) {
        var cr = vid(cb.frCr), dr = vid(cb.frDr);
        if (cr > 0 && cr === dr) {
            (side === 'cr' ? cb.frCr : cb.frDr).clear();
            msg("Freight Credit And Freight Debit Account Can't be same");
        }
        recalc();
    }
    function typeUi() {                                                                      // columns and header controls that depend on DeliveryTypeId
        var t = S.deliveryTypeId, one = t === 1;
        G.grd.spec.cols.forEach(function (c) {
            if (c.k === 'SaleOrder' || c.k === 'OrderDate') c.hide = !one;
            if (c.k === 'ItemRate' || c.k === 'RateUOM') c.edit = !one;
            if (c.k === 'TaxNameId' || c.k === 'TaxPrcnt') c.edit = S.taxEditable;
        });
        G.grd.refresh();
        enable(cb.dterm, t !== 1 && t !== 0); enable(cb.term, t !== 1 && t !== 0);
        if (t === 0) { enable(cb.dterm, false); enable(cb.term, false); }
    }

    /* ------------------------------------------------------------------ grids */
    function listCol(list, key, text) { return function () { return list(); }; }
    function makeGrids() {
        G.grd = SE.grid('grd', { frozen: 0, dec: 0, onEdit: gridEdit, cols: [
            { k: 'SaleOrder', t: 'SaleOrder', w: 70, hide: true }, { k: 'OrderDate', t: 'OrderDate', w: 85, f: 'date', hide: true },
            { k: 'Id', hide: true }, { k: 'GdnId', hide: true }, { k: 'GdnDetailId', hide: true }, { k: 'SaleOrderId', hide: true }, { k: 'SaleOrderDetailId', hide: true }, { k: 'WarehouseId', hide: true },
            { k: 'ItemCode', t: 'ItemCode', w: 80 }, { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'PackUomId', hide: true }, { k: 'PackUom', t: 'Uom', w: 60 },
            { k: 'PackEquivalent', hide: true }, { k: 'JobLotId', hide: true },
            { k: 'ItemQty', t: 'ItemQty', w: 80, f: 'n2', sum: true, render: function (v) { return v == null ? '' : f2(v); } },
            { k: 'ItemRate', t: 'ItemRate', w: 80, edit: true, f: 'n2', render: function (v) { return v == null ? '' : fmtRate(v); } },
            { k: 'RateUomId', hide: true }, { k: 'RateUOM', t: 'RateUOM', w: 70, edit: true }, { k: 'RateEquivalent', hide: true },
            { k: 'AddLessRate', t: 'AddLessRate', w: 80, edit: true, f: 'n2', render: function (v) { return v == null ? '' : fmtRate(v); } },
            { k: 'NetRate', t: 'NetRate', w: 80, f: 'n2', render: function (v) { return v == null ? '' : fmtRate(v); } },
            { k: 'DiscPrcnt', t: 'DiscPrcnt', w: 70, edit: true, f: 'n3' }, { k: 'DiscAmount', t: 'DiscAmount', w: 80, edit: true, f: 'amt', sum: true },
            { k: 'ItemAmount', t: 'ItemAmount', w: 90, f: 'amt', sum: true }, { k: 'FcyAmount', t: 'FcyAmount', w: 90, f: 'amt', sum: true, hide: true },
            { k: 'TaxNameId', t: 'Tax Name', w: 60, list: function () { return S.taxTypes; }, lk: 'TaxNameId', lt: 'TaxName' },
            { k: 'TaxPrcnt', t: 'TaxPrcnt', w: 70, f: 'n3' }, { k: 'TaxAmount', t: 'TaxAmount', w: 80, f: 'amt', sum: true },
            { k: 'Freights', t: 'Freights', w: 80, f: 'n3', sum: true }, { k: 'Expense', t: 'Expense', w: 80, f: 'n3', sum: true },
            { k: 'Journal', hide: true }, { k: 'Commission', t: 'Commission', w: 80, f: 'n3', sum: true, hide: true },
            { k: 'BillAmount', t: 'Item Net Amount', w: 100, f: 'amt', sum: true },
            { k: 'VehicleNo', t: 'VehicleNo', w: 90, edit: true }, { k: 'GpNo', t: 'GpNo', w: 60, edit: true },
            { k: 'PaymentTermId', hide: true }, { k: 'DueDays', hide: true }, { k: 'DueDate', hide: true }, { k: 'BillTypeId', hide: true }, { k: 'BillType', t: 'BillType', w: 80 }] });
        G.exp = SE.grid('grdInvExp', { footer: false, dec: 0, onBtn: expBtn, onEdit: expEdit, cols: [
            { k: '_del', t: '', w: 26, btn: 'X' }, { k: '_add', t: '', w: 26, btn: '+' },
            { k: 'ItemId', t: 'Item', w: 200, edit: true, list: function () { return S.other; }, lk: 'Id', lt: 'OtherItemName' },
            { k: 'Qty', t: 'Qty', w: 70, edit: true, f: 'n3' }, { k: 'Rate', t: 'Rate', w: 80, edit: true, f: 'n3' },
            { k: 'Amount', t: 'Amount', w: 90, f: 'n2' }, { k: 'Remarks', t: 'Remarks', w: 180, edit: true }] });
        G.pay = SE.grid('grdPaymentTerm', { footer: false, dec: 0, onBtn: payBtn, onEdit: payEdit, cols: [
            { k: '_add', t: '', w: 26, btn: '+' }, { k: '_del', t: '', w: 26, btn: 'X' },
            { k: 'PaymentTermId', t: 'Payment Term', w: 120, edit: true, list: function () { return S.terms; }, lk: 'Id', lt: 'TermsDescription' },
            { k: '%ofTotal', t: '%ofTotal', w: 70, edit: true, f: 'n3' }, { k: 'Amount', t: 'Amount', w: 90, edit: true, f: 'n2' },
            { k: 'DueDays', t: 'DueDays', w: 60, edit: true, f: 'n0' }, { k: 'DueDate', t: 'DueDate', w: 100, edit: true, f: 'date' },
            { k: 'Remarks', t: 'Remarks', w: 300, edit: true }] });
        G.gl = SE.grid('grdGLedger', { footer: false, dec: 0, onBtn: glBtn, onEdit: glEdit, cols: [
            { k: '_add', t: '', w: 26, btn: '+' }, { k: '_del', t: '', w: 26, btn: 'X' },
            { k: 'AccountId', t: 'Account', w: 250, edit: true, list: function () { return S.fr; }, lk: 'K', lt: 'AccountTitle' }, { k: 'GlAccountId', hide: true },
            { k: 'Remarks', t: 'Remarks', w: 140, edit: true }, { k: 'Percentage', t: 'Percentage', w: 70, edit: true, f: 'n3' },
            { k: 'Qty', t: 'Qty', w: 60, edit: true, f: 'n3' }, { k: 'Rate', t: 'Rate', w: 60, edit: true, f: 'n3' },
            { k: 'Debit', t: 'Debit', w: 80, edit: true, f: 'n3' }, { k: 'Credit', t: 'Credit', w: 80, edit: true, f: 'n3' }] });
        G.comm = null;                                                                       // grdComm is hidden on the desktop (panel18): kept as page state in S.comm
        G.hist = SE.grid('grdHistory', { footer: false, dec: 0, frozen: 4, onDbl: function (r) { histEdit(r); }, onBtn: histBtn, onLink: histLink, onSel: histSelected, cols: [
            { k: 'Edit', t: '', w: 44, btn: 'Edit' }, { k: 'View', t: '1609-Slip', w: 70, btn: 'View' }, { k: 'Print1', t: '1609A-Slip', w: 80, btn: 'Print' }, { k: 'Voucher', t: 'Voucher', w: 70, btn: 'Voucher' },
            { k: 'Id', hide: true }, { k: 'VoucherHeadId', hide: true }, { k: 'DocNo', t: 'InvoiceNo', w: 70 }, { k: 'DocDate', t: 'InvoiceDate', w: 90, f: 'date' },
            { k: 'CustomerName', t: 'CustomerName', w: 200 }, { k: 'ManualBillNo', t: 'ManualBillNo', w: 90 }, { k: 'DeliveryTerm', t: 'DeliveryTerm', w: 90 }, { k: 'PaymentTerm', t: 'PaymentTerm', w: 90 },
            { k: 'TransporterCreditAc', t: 'FreightAccountCr', w: 150 }, { k: 'TransporterDebitAc', t: 'FreightAccountDr', w: 150 }, { k: 'FreightAmount', t: 'FreightAmount', w: 90, f: 'amt' },
            { k: 'TaxAccount', t: 'TaxAccount', w: 120 }, { k: 'BillAmount', t: 'BillAmount', w: 100, f: 'amt' },
            { k: 'EntryUser', t: 'EntryUser', w: 100 }, { k: 'EntryDate', t: 'EntryDate', w: 130, f: 'dt12' }, { k: 'ModifyUser', t: 'ModifyUser', w: 100 }, { k: 'ModifyDate', t: 'ModifyDate', w: 130, f: 'dt12' },
            { k: 'ApprovedUser', t: 'ApprovedUser', w: 100 }, { k: 'ApprovedDate', t: 'ApprovedDate', w: 130, f: 'dt12' }, { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 90, link: true },
            { k: 'Remarks', t: 'Remarks', w: 200 }] });
        G.hd = SE.grid('grdDetail', { dec: 0, cols: [
            { k: 'SaleOrder', t: 'SaleOrder', w: 70 }, { k: 'WareHouseName', t: 'WareHouseName', w: 120 }, { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'ItemUOM', t: 'Uom', w: 60 }, { k: 'JobLot', t: 'JobLot', w: 90 },
            { k: 'ItemQty', t: 'ItemQty', w: 80, f: 'n2' }, { k: 'ItemRate', t: 'ItemRate', w: 80, f: 'n2' }, { k: 'RateUOM', t: 'RateUOM', w: 70 }, { k: 'AddLessRate', t: 'AddLessRate', w: 80, f: 'n2' },
            { k: 'NetRate', t: 'NetRate', w: 80, f: 'n2' }, { k: 'DiscPrcnt', t: 'DiscPrcnt', w: 70, f: 'n3' }, { k: 'DiscAmount', t: 'DiscAmount', w: 80, f: 'amt' },
            { k: 'ItemAmount', t: 'ItemAmount', w: 90, f: 'amt' }, { k: 'FcyAmount', t: 'FcyAmount', w: 90, f: 'amt', hide: true }, { k: 'TaxName', t: 'TaxName', w: 90 },
            { k: 'TaxPrcnt', t: 'TaxPrcnt', w: 70, f: 'n3' }, { k: 'TaxAmount', t: 'TaxAmount', w: 80, f: 'amt' }, { k: 'ExpenseAmount', t: 'ExpenseAmount', w: 90, f: 'n3' },
            { k: 'FreightAmount', t: 'FreightAmount', w: 90, f: 'n3' }, { k: 'BillAmount', t: 'Item Net Amount', w: 100, f: 'amt' }, { k: 'GpNo', t: 'GpNo', w: 60 }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 },
            { k: 'PaymentTerm', t: 'PaymentTerm', w: 100 }, { k: 'DueDays', t: 'DueDays', w: 60 }, { k: 'DueDate', t: 'DueDate', w: 90, f: 'date' }, { k: 'BillType', t: 'BillType', w: 80 }] });
    }

    /* grd_CellUpdated: the edited cell sets the sticky calc flag, the server runs the formulas */
    function gridEdit(row, key, val) {
        if (key === 'VehicleNo') { row.VehicleNo = val; return; }
        if (key === 'GpNo') { row.GpNo = SE.toInt(val); return; }
        if (key === 'RateUOM') { if (val !== '' && !isNumeric(val)) return msg('Please Type Only Numeric Value'); row.RateUOM = val; row.calc = 'rate'; }
        else if (key === 'TaxNameId') {
            var id = +val || 0, t = null; S.taxTypes.forEach(function (x) { if (+x.TaxNameId === id) t = x; });
            row.TaxNameId = id; if (t) row.TaxPrcnt = +t.TaxPercent || 0; else row.TaxPrcnt = 0; row.calc = row.calc || 'other';
        } else {
            if (val !== '' && !isNumeric(val)) return msg('Please Type Only Numeric Value');
            var n = SE.toNum(val); row[key] = n;
            if (key === 'DiscAmount') row.calc = 'discAmt';
            else if (key === 'TaxPrcnt') { if (n > 100) { row.TaxPrcnt = 100; msg("Tax % Can't be greater than 100..."); } row.calc = row.calc || 'other'; }
            else row.calc = 'rate';
        }
        G.grd.refresh(); recalc();
    }

    /* grdInvExp */
    function expBlank() { return { ItemId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' }; }
    function expBtn(k, row, i) {
        if (k === '_add') { G.exp.addRow(expBlank()); }
        else if (G.exp.count() > 1) { G.exp.removeAt(i); recalc(); }
        else { G.exp.setRows([expBlank()]); recalc(); }
    }
    function expEdit(row, key, val) {
        if (key === 'ItemId') row.ItemId = +val || 0;
        else if (key === 'Remarks') row.Remarks = val;
        else {
            if (val !== '' && !isNumeric(val)) return msg('Please Type Only Numeric Value');
            row[key] = SE.toNum(val); row.Amount = rnd((+row.Qty || 0) * (+row.Rate || 0), S.fmt.amountRound);
        }
        G.exp.refresh(); recalc();
    }

    /* grdPaymentTerm_CellUpdated */
    function payBlank() { return { PaymentTermId: 0, '%ofTotal': 0, Amount: 0, DueDays: 0, DueDate: $('DocDate').value, Remarks: '' }; }
    function payBtn(k, row, i) {
        if (k === '_add') { G.pay.addRow(payBlank()); return; }
        if (G.pay.count() > 1) G.pay.removeAt(i); else G.pay.setRows([payBlank()]);
    }
    function payEdit(row, key, val) {
        var bill = billAmount();
        if (key === 'Remarks') { row.Remarks = val; G.pay.refresh(); return; }
        if (key === 'PaymentTermId') {
            row.PaymentTermId = +val || 0;
            if (row.PaymentTermId === 3) {
                var so = G.grd.count() ? +G.grd.rows()[0].SaleOrderId || 0 : 0;
                if (so > 0) {
                    return SE.api(API + '/remaining' + SE.q({ orderId: so, recId: S.id })).then(function (r) {
                        row.Amount = +r.balanceAmount || 0; row['%ofTotal'] = bill > 0 ? rnd(row.Amount * 100 / bill, 8) : 0; G.pay.refresh();
                    }).catch(fail);
                }
                row.Amount = 0; row['%ofTotal'] = 0;
            }
            G.pay.refresh(); return;
        }
        if (key === '%ofTotal') {
            if (!isNumeric(val)) { G.pay.refresh(); return msg('Please Type Only Numeric Value'); }
            var p = SE.toNum(val);
            if (p > 100) { p = 100; msg("%of Total Can't Greater than 100"); }
            row['%ofTotal'] = p; row.Amount = rnd(bill * p / 100, 4);
        } else if (key === 'Amount') {
            if (!isNumeric(val)) { G.pay.refresh(); return msg('Please Type Only Numeric Value'); }
            var a = SE.toNum(val);
            if (a > bill) { a = bill; msg('Amount Cant be Greater than BillAmount'); }
            row.Amount = a; row['%ofTotal'] = bill > 0 ? rnd(a * 100 / bill, 8) : 0;
        } else if (key === 'DueDays') {
            if (val !== '' && !isNumeric(val)) { G.pay.refresh(); return msg('Please Type Only Numeric Value'); }
            row.DueDays = SE.toInt(val); row.DueDate = addDays($('DocDate').value, row.DueDays);
        } else if (key === 'DueDate') {
            var dd = readDate(val);
            if (!dd) { G.pay.refresh(); return; }
            if (dd < $('DocDate').value) { G.pay.refresh(); return msg("Due Date Can't less Than DocDate"); }
            row.DueDate = dd; row.DueDays = dayDiff(dd, $('DocDate').value);
        }
        G.pay.refresh();
    }

    /* grdGLedger_CellUpdated */
    function glBlank() { return { AccountId: 0, GlAccountId: 0, Remarks: '', Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0 }; }
    function glBtn(k, row, i) {
        if (k === '_add') { G.gl.addRow(glBlank()); return; }
        if (G.gl.count() > 1) G.gl.removeAt(i); else G.gl.setRows([glBlank()]);
        recalc();
    }
    function glEdit(row, key, val) {
        var r = S.fmt.amountRound;
        if (key === 'Remarks') { row.Remarks = val; G.gl.refresh(); return; }
        if (key === 'AccountId') {
            var id = +val || 0, a = null; S.fr.forEach(function (x) { if (+x.K === id) a = x; });
            if (id > 0 && a) {
                if (S.sub ? id === vid(cb.cust) : (custRow() && +a.Id === +custRow().GlAccountId)) {
                    G.gl.refresh();
                    return msg(S.sub ? 'Selected Account Can not be Same As Supplier Account' : 'Customer Account Not select');
                }
            }
            row.AccountId = id; row.GlAccountId = a ? +a.Id : 0;
        } else {
            if (!isNumeric(val)) { G.gl.refresh(); return msg('Please Type Only Numeric Value'); }
            var n = SE.toNum(val);
            if (key === 'Qty' || key === 'Rate') {
                row[key] = n; row.Credit = rnd((+row.Qty || 0) * (+row.Rate || 0), r); row.Debit = 0; row.Percentage = 0;
            } else if (key === 'Percentage') {
                var tot = n; G.gl.rows().forEach(function (x) { if (x !== row) tot += +x.Percentage || 0; });
                if (tot > 100) { G.gl.refresh(); return msg('TotalPercentage Can not be Greater than 100...'); }
                row.Percentage = n; var amt = rnd(itemTotal() * n / 100, r);
                if ((+row.Debit || 0) > 0) row.Debit = amt; else row.Credit = amt;
                row.Qty = 0; row.Rate = 0;
            } else if (key === 'Credit') {
                if ((+row.Debit || 0) > 0 && n > 0) { G.gl.refresh(); return msg('Debit Side is aleady added'); }
                row.Credit = n;
            } else if (key === 'Debit') {
                if ((+row.Credit || 0) > 0 && n > 0) { G.gl.refresh(); return msg('Credit Side is aleady added'); }
                row.Debit = n;
            }
        }
        G.gl.refresh(); recalc();
    }

    /* ------------------------------------------------------------------ Load Gdn (toolStripButton3 -> LoadGdnTrading_Engr) */
    function openLoader() {
        var h = '<div class="lbar"><button type="button" id="ldNew">New</button><button type="button" id="ldSearch">Search</button><button type="button" id="ldLoad">Load</button></div>' +
            '<div class="lf" style="position:relative;height:40px"><span class="dl" style="left:7px;top:12px">From Date</span><input class="f" type="date" id="ldFrom" style="left:75px;top:8px;width:130px;height:22px">' +
            '<span class="dl" style="left:225px;top:12px">To Date</span><input class="f" type="date" id="ldTo" style="left:285px;top:8px;width:130px;height:22px"></div>' +
            '<div class="lgrid" id="ldGrd" style="height:240px"></div><div class="lgrid" id="ldDet" style="height:120px"></div>';
        var pop = SE.pop('Load Goods Dispatch Notes', h, [{ t: 'Close' }], { wide: true });
        pop.open();
        var $$ = function (id) { return pop.body.querySelector('#' + id); };
        var g = SE.grid($$('ldGrd'), { footer: false, frozen: 1, onSel: loaderSel, cols: [
            { k: 'Sel', t: 'Select', w: 40, sel: true }, { k: 'Id', hide: true }, { k: 'SaleOrderId', hide: true }, { k: 'DocDate', t: 'DocDate', w: 85, f: 'date' }, { k: 'DocNo', t: 'DocNo', w: 60 },
            { k: 'SupplierCustomerId', hide: true }, { k: 'CustomerName', t: 'CustomerName', w: 220 }, { k: 'GpNO', t: 'GpNO', w: 70 }, { k: 'BiltyNo', t: 'BiltyNo', w: 90 },
            { k: 'VehicleNo', t: 'VehicleNo', w: 90 }, { k: 'ItemQty', t: 'ItemQty', w: 80, f: 'n3' }, { k: 'DeliveryTypeId', hide: true }, { k: 'DeliveryType', t: 'DeliveryType', w: 110 }] });
        var gd = SE.grid($$('ldDet'), { footer: false, cols: [
            { k: 'OrderNo', t: 'OrderNo', w: 70 }, { k: 'ItemName', t: 'Item', w: 240 }, { k: 'JobLot', t: 'JobLot', w: 100 }, { k: 'UOM', t: 'UOM', w: 70 },
            { k: 'Qty', t: 'Qty', w: 80, f: 'n3' }, { k: 'WareHouse', t: 'WareHouse', w: 120 }] });
        function loaderSel(r) {
            if (!r || !gd) { if (gd) gd.setRows([]); return; }
            SE.api(API + '/loader/detail' + SE.q({ gdnId: r.Id }), { quiet: true }).then(function (rows) {
                if (!g.cur() || +g.cur().Id !== +r.Id) return;
                gd.setRows(rows.map(function (x) { return { OrderNo: ci(x, 'SaleOrderNo'), ItemName: ci(x, 'ItemName'), JobLot: ci(x, 'JobLot'), UOM: ci(x, 'UOMCode'), Qty: ci(x, 'ItemQty'), WareHouse: ci(x, 'WareHouseCode') }; }));
            }).catch(function () { gd.setRows([]); });
        }
        function search() {
            return SE.api(API + '/loader/rows' + SE.q({ fromDate: $$('ldFrom').value, toDate: $$('ldTo').value })).then(function (rows) { g.setRows(rows); }).catch(function (e) { msg('Error occurred during database call: ' + e.message); });
        }
        $$('ldFrom').value = SE.dateInput(S.fyStart) || SE.today(); $$('ldTo').value = SE.today();
        $$('ldSearch').onclick = search;
        $$('ldNew').onclick = function () { $$('ldFrom').value = SE.dateInput(S.fyStart) || SE.today(); $$('ldTo').value = SE.today(); search(); };
        $$('ldLoad').onclick = function () {                                                 // btnLoad_Click
            var picked = g.checked();
            if (!picked.length) return msg('Chek the row first');
            var t0 = +picked[0].DeliveryTypeId, c0 = +picked[0].SupplierCustomerId, o0 = +picked[0].SaleOrderId;
            for (var i = 1; i < picked.length; i++) {
                if (+picked[i].DeliveryTypeId !== t0) return msg("Sorry!. The Selected Gdn's are not of same Delivery Type");
                if (+picked[i].SupplierCustomerId !== c0) return msg("Sorry!. The Selected Gdn's are not of same Customer");
                if (t0 === 1 && +picked[i].SaleOrderId !== o0) return msg("Sorry!. The Selected Gdn's are not of same Sale Order");
            }
            if (S.deliveryTypeId > 0 && G.grd.count() > 0 && S.deliveryTypeId !== t0) return msg("Already Loaded Rows Have Different Delivery Type. So you can't Load New Rows");
            if (vid(cb.cust) > 0 && G.grd.count() > 0 && vid(cb.cust) !== c0) return msg("You Can't Load GDN Of Different Customer At Same Time");
            loadGdn(picked.map(function (r) { return +r.Id; }), t0, c0).then(function (ok) { if (ok) pop.close(); });
        };
        search();
    }
    /* LoadInGridDetail: LoadDataDetailGridAgainstGP, LoadExpensesFromGdn, LoadPaymentDetailBySaleOrderIds */
    function loadGdn(ids, typeId, custId) {
        return SE.api(API + '/load-gdn' + SE.q({ gdnIds: ids.join(','), deliveryTypeId: typeId, customerId: G.grd.count() > 0 ? vid(cb.cust) : 0 })).then(function (d) {
            if (!d.lines || !d.lines.length) return true;
            var h = d.head || {};
            S.deliveryTypeId = typeId;
            cb.cust.setValue(+h.SupplierCustomerId || custId); enable(cb.cust, false);
            var dtm = (h.DeliveryTerm || '').trim(); S.dterms.forEach(function (x) { if (x.DeliveryTerm === dtm) cb.dterm.setValue(x.Id); });
            cb.term.setValue(+h.PaymentTermsId || 0); termChanged();
            if (h.OrderDueDays != null && h.OrderDueDays !== '' && vid(cb.term) !== 1) { $('txtDueDays').value = String(h.OrderDueDays); dueDaysChanged(); }
            var ttl = (h.AccountTitle || '').trim(); S.fr.forEach(function (x) { if (ttl && String(x.AccountTitle).trim() === ttl) cb.frCr.setValue(+x.K); });
            $('txtFreightAmount').value = f3(+h.CarriageAmount || 0);
            if (h.RemarksHeader) $('txtRemarks').value = h.RemarksHeader;
            var have = {}; G.grd.rows().forEach(function (r) { have[+r.GdnDetailId] = 1; });
            d.lines.forEach(function (l) { if (!have[+l.GdnDetailId]) { have[+l.GdnDetailId] = 1; G.grd.rows().push(l); } });
            var seen = {}; S.comm.forEach(function (k) { seen[k.OrderId] = 1; });
            (d.comm || []).forEach(function (k) { if (!seen[k.OrderId]) { seen[k.OrderId] = 1; S.comm.push(k); } });
            var expTotal = G.exp.rows().reduce(function (a, r) { return a + (+r.Amount || 0); }, 0);
            if (expTotal === 0 && d.expenses && d.expenses.length) G.exp.setRows(d.expenses);
            if (typeId === 1 && d.payments && d.payments.length) G.pay.setRows(d.payments.map(function (p) { p.DueDate = SE.dateInput(p.DueDate) || $('DocDate').value; return p; }));
            typeUi();
            return recalc().then(function () { return true; });
        }).catch(function (e) { msg(e.message); return false; });
    }

    /* ------------------------------------------------------------------ Save / Update / Delete */
    function insert(update) {
        if (G.grd.count() === 0) return msg('Grid Record Not Found');
        return SE.ask(update ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
            if (!yes) return;
            return SE.api(API, { method: 'POST', body: reqBody(true) }).then(function (r) {
                return msg(r.message || 'Record Saved Successfully').then(function () {
                    var pv = $('ChkBok').checked, ps = $('ChkPrintSlip').checked;
                    return reset().then(function () {
                        if (pv && r.voucherHeadId > 0) printVoucher(r.voucherHeadId);
                        if (ps) printSlip(r.id);
                    });
                });
            }).catch(function (e) { return msg(e.message, 'Message'); });
        });
    }
    function btnSave() { S.id = 0; return insert(false); }
    function btnUpdate() { if (!(S.id > 0)) return msg('RecordId Not Found.....'); return insert(true); }
    function btnDelete() {
        if (!(S.id > 0)) return msg('RecordId Not Found.....');
        return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
            if (!yes) return;
            return SE.api(API + '/' + S.id, { method: 'DELETE' }).then(function (r) { return msg(r.message || 'Delete Record Successfully').then(reset); }).catch(function (e) { return msg(e.message, 'Message'); });
        });
    }
    function setButtons() {
        var editing = S.id > 0;
        show('btnSave', !editing); show('btnUpdate', editing); show('btnDelete', editing);
        $('btnSave').disabled = !S.rights.save; $('btnUpdate').disabled = !S.rights.update; $('btnDelete').disabled = !S.rights['delete'];
        ['toolStripButton1', 'btnSlipFormat2', 'btnPrint'].forEach(function (id) { $(id).disabled = !S.rights.print; });
    }

    /* ------------------------------------------------------------------ Reset / Refresh */
    function reset() {
        S.id = 0; S.files = []; S.removedAtt = []; S.existing = []; S.comm = []; S.headComm = 0; S.voucherHeadId = 0; S.deliveryTypeId = 0;
        cb.cust.clear(); enable(cb.cust, true); cb.dterm.clear(); cb.term.clear(); cb.tax.clear(); cb.frCr.clear(); cb.frDr.clear(); cb.disc.clear(); cb.commDr.clear(); cb.cur.clear();
        ['txtrefno', 'txtbillno', 'txtRemarks', 'txtFreightAmount', 'txtDiscountAmountHeader', 'txtBillAmount', 'txtFcyAmount', 'txtExchangeRate', 'txtDueDays'].forEach(function (id) { $(id).value = ''; });
        $('txtDueDays').disabled = false;
        var t = SE.today(); $('DocDate').value = t; $('DueDate').value = t;
        G.grd.setRows([]); G.exp.setRows([expBlank()]); G.gl.setRows([glBlank()]); G.pay.setRows([payBlank()]);
        configDefault(); typeUi(); setButtons();
        if (S.lastDiscount > 0) cb.disc.setValue(S.lastDiscount);
        return SE.api(API + '/next-no').then(function (r) { if (r.nextNo > 0) $('txtdocno').value = String(r.nextNo); }).catch(fail);
    }
    function refresh() {
        return SE.api(API + '/refresh').then(function (d) { applyLists(d); multiCurrency(); commissionDebitUi(); fillAll(); configDefault(); }).catch(fail);
    }

    /* ------------------------------------------------------------------ ReadById */
    function edit(id) {
        return SE.api(API + '/' + id).then(function (d) {
            var h = d.head;
            tabs.select('tabForm');
            S.id = id; S.files = []; S.removedAtt = []; S.existing = d.attachments || []; S.voucherHeadId = +d.voucherHeadId || 0;
            S.deliveryTypeId = +d.deliveryTypeId || 2; S.comm = d.commissions || []; S.headComm = +h.CommAmount || 0;
            $('txtdocno').value = h.DocNo; $('DocDate').value = SE.dateInput(h.DocDate);
            cb.cust.setValue(+h.SupplierCustomerId || 0); enable(cb.cust, false);
            $('txtrefno').value = h.SupplierReferenceNo || ''; $('txtbillno').value = h.ManualBillNo || '';
            cb.tax.setValue(+h.TaxAccountId || 0);
            cb.frCr.setValue(S.sub ? (+h.TransporterCreditPartyId || 0) : (+h.TransporterId || 0));
            cb.frDr.setValue(S.sub ? (+h.TransporterDebitPartyId || 0) : (+h.TransporterDebitGLId || 0));
            $('txtFreightAmount').value = f3(+h.FreightAmount || 0); $('txtFreightRemarks').value = h.FreightRemark || '';
            $('txtRemarks').value = h.RemarksHeader || '';
            var dtm = (h.DeliveryTerm || '').trim(); cb.dterm.clear(); S.dterms.forEach(function (x) { if (x.DeliveryTerm === dtm) cb.dterm.setValue(x.Id); });
            cb.term.setValue(+h.PaymentTermId || 0); termChanged();
            if (vid(cb.term) !== 1) $('txtDueDays').value = h.DueDays == null ? '' : String(h.DueDays);
            $('DueDate').value = SE.dateInput(h.DueDate) || $('DocDate').value;
            cb.cur.setValue(+h.CurrencyId || 0);
            if (+h.CurrencyId > 0) $('txtExchangeRate').value = h.ExchangeRate == null ? '' : String(h.ExchangeRate); else configDefault();
            $('txtDiscountAmountHeader').value = f3(+h.DiscountAmount || 0); cb.disc.setValue(+h.DiscountAccountId || 0);
            if (S.commDebit) cb.commDr.setValue(+h.CommissionDebitAcGLId || 0);
            $('txtBillAmount').value = f4(+h.BillAmount || 0); $('txtFcyAmount').value = fmtFcy(+h.FcyAmount || 0);
            G.grd.setRows(d.lines || []);
            G.exp.setRows((d.expenses && d.expenses.length) ? d.expenses : [expBlank()]);
            G.gl.setRows((d.journals && d.journals.length) ? d.journals : [glBlank()]);
            G.pay.setRows((d.payments && d.payments.length) ? d.payments.map(function (p) { p.DueDate = SE.dateInput(p.DueDate) || $('DocDate').value; return p; }) : [payBlank()]);
            typeUi(); setButtons();
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ history */
    function showHistory() {
        var dt = $('rdentrydate').checked ? 'entry' : ($('rdmodifydate').checked ? 'modify' : ($('rdapproveddate').checked ? 'approved' : 'document'));
        var q = { dateType: dt, fromDate: $('txtFromdateHistory_chk').checked ? $('txtFromdateHistory').value : '', toDate: $('txtToDateHistory_chk').checked ? $('txtToDateHistory').value : '',
            fromNo: SE.toInt($('txtFromNoHistory').value), toNo: SE.toInt($('txtToDocNoHistory').value), customerId: vid(cb.hCust) };
        return SE.api(API + '/history' + SE.q(q)).then(function (rows) {
            G.hist.setRows(rows.map(function (r) {
                return { Id: ci(r, 'Id'), VoucherHeadId: ci(r, 'VoucherHeadId'), DocNo: ci(r, 'DocNo'), DocDate: ci(r, 'DocDate'), CustomerName: pick(r, ['CustomerName', 'SupplierCustomer', 'Customer']),
                    ManualBillNo: ci(r, 'ManualBillNo'), DeliveryTerm: ci(r, 'DeliveryTerm'), PaymentTerm: pick(r, ['PaymentTerm', 'TermsDescription']),
                    TransporterCreditAc: pick(r, ['FreightAccountCr', 'TransporterCreditAc', 'TransporterCredit']), TransporterDebitAc: pick(r, ['FreightAccountDr', 'TransporterDebitAc', 'TransporterDebit']),
                    FreightAmount: ci(r, 'FreightAmount'), TaxAccount: pick(r, ['TaxAccount', 'TaxAccountTitle']), BillAmount: ci(r, 'BillAmount'),
                    EntryUser: pick(r, ['UserName', 'EntryUser']), EntryDate: ci(r, 'EntryDate'), ModifyUser: pick(r, ['ModifyUserName', 'ModifyUser']), ModifyDate: ci(r, 'ModifyDate'),
                    ApprovedUser: pick(r, ['ApprovedUserName', 'ApprovedUser']), ApprovedDate: pick(r, ['ApprovedDate', 'PostDate']), NoOfAttachments: ci(r, 'NoOfAttachments'),
                    Remarks: pick(r, ['RemarksHeader', 'Remarks']) };
            }));
        }).catch(function (e) { return msg(e.message, 'Error Message'); });
    }
    function resetHistory() {
        $('txtFromdateHistory').value = SE.dateInput(S.fyStart) || SE.today(); $('txtToDateHistory').value = SE.today();
        $('txtFromNoHistory').value = ''; $('txtToDocNoHistory').value = ''; cb.hCust.clear(); G.hist.setRows([]); $('drdocdate').checked = true;
    }
    function refreshHistoryCombos() { return SE.api(API + '/history-combos').then(function (h) { cb.hCust.setData(h.customers || []); }).catch(function (e) { return msg(e.message, 'Error Message'); }); }
    function histSelected(item) {
        if (!item) { G.hd.setRows([]); return; }
        SE.api(API + '/' + item.Id + '/history-detail', { quiet: true }).then(function (rows) {
            if (!G.hist.cur() || +G.hist.cur().Id !== +item.Id) return;
            G.hd.setRows(rows);
        }).catch(function () { G.hd.setRows([]); });
    }
    function histEdit(r) { if (!S.rights.update) return msg("ypu don't have updae rights..."); return edit(+r.Id); }
    function histBtn(k, r) {
        if (k === 'Edit') histEdit(r);
        else if (k === 'View') printSlip(+r.Id, 'No Record Found For Display');
        else if (k === 'Print1') printSlip2(+r.Id, 'Record Not Found For Display');
        else if (k === 'Voucher') printVoucher(+r.VoucherHeadId, 'No Record Found For Display');
    }
    function histLink(k, r) { if (k === 'NoOfAttachments') showAttachments(+r.Id); }

    /* ------------------------------------------------------------------ print / attachments */
    function printSlip(id, m) {
        if (!(id > 0)) return msg(m || 'No Record Found For Display');
        SE.printRpt('1609-InvRepSaleBillCustomer.rpt', { id: id });
    }
    function printSlip2(id, m) {
        if (!(id > 0)) return msg(m || 'Record Not Found For Display');
        SE.printRpt('1609A-InvRepSaleBillCustomer-Format-II.rpt', { id: id });
    }
    function printVoucher(vhId, m) {
        if (!(vhId > 0)) return msg(m || 'No Record Found For Display');
        SE.printRpt('103-AcRptPurchaseSalesVoucherSlip.rpt', { id: vhId, documentTypeId: DOC });
    }
    function showAttachments(id) { return SE.api(API + '/' + id + '/attachments').then(function (rows) { renderAttachments(rows, id, true); }).catch(fail); }
    function openAttachmentDialog() {
        var rows = (S.existing || []).filter(function (r) { return S.removedAtt.indexOf(r.Id) < 0; });
        renderAttachments(rows, S.id, false);
    }
    function renderAttachments(rows, id, readOnly) {
        var h = '<div class="dgrid" style="max-height:50vh"><table class="jg"><thead><tr><th>Attachment</th><th style="width:90px">Action</th></tr></thead><tbody>';
        rows.forEach(function (r) {
            h += '<tr><td>' + SE.esc(r.Attachment) + '</td><td>' + (id > 0 ? '<a class="lnk" href="' + API + '/' + id + '/attachments/' + r.Id + '" target="_blank">Open</a>' : '') +
                (readOnly ? '' : ' <a class="lnk" data-rm="' + r.Id + '" style="cursor:pointer">Remove</a>') + '</td></tr>';
        });
        S.files.forEach(function (f, i) { if (!readOnly) h += '<tr><td>' + SE.esc(f.name) + ' (new)</td><td><a class="lnk" data-nf="' + i + '" style="cursor:pointer">Remove</a></td></tr>'; });
        h += '</tbody></table></div>';
        if (!readOnly) h += '<div style="padding:6px"><input type="file" id="atFile" multiple> <span class="se-note">Up to 5 MB each. Files are stored when the invoice is saved.</span></div>';
        var pop = SE.pop('Attachments', h, [{ t: 'Close' }]);
        pop.open();
        pop.body.addEventListener('click', function (e) {
            var rm = e.target.getAttribute && e.target.getAttribute('data-rm');
            if (rm) { S.removedAtt.push(+rm); pop.close(); openAttachmentDialog(); }
            if (e.target.hasAttribute && e.target.hasAttribute('data-nf')) { S.files.splice(+e.target.getAttribute('data-nf'), 1); pop.close(); openAttachmentDialog(); }
        });
        var fi = pop.body.querySelector('#atFile');
        if (fi) fi.addEventListener('change', function () {
            var list = Array.prototype.slice.call(fi.files), left = list.length;
            if (!left) return;
            list.forEach(function (f) {
                if (f.size > 5 * 1024 * 1024) { SE.alert(f.name + ' exceeds 5 MB'); left--; return; }
                var fr = new FileReader();
                fr.onload = function () { S.files.push({ name: f.name, base64: String(fr.result).split(',')[1] || '' }); if (--left <= 0) { pop.close(); openAttachmentDialog(); } };
                fr.readAsDataURL(f);
            });
        });
    }

    /* ------------------------------------------------------------------ keys / wiring */
    var SHORT = [['Ctrl+S', 'For Save in form tab and for show data in history tab'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+E', 'For Close'],
        ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Alt+1', 'For Print Slip 1608'], ['Alt+2', 'For Print Slip 1608A'], ['Alt+3', 'For Print Voucher 103'],
        ['Ctrl+F5', 'For Focus on Customer Name'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+D', 'For Adding an row in Focused Grid'], ['Ctrl+Delete', 'For Deleting an row of Focused Grid'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'],
        ['Ctrl+ArrowUp', 'For Focus On Customer Name in Detail Box'], ['Ctrl+ArrowRight', 'For Change Focus from one Grid To another Grid'],
        ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    var ORDER = ['grd', 'grdInvExp', 'grdGLedger', 'grdPaymentTerm'];
    function gridOf(id) { return id === 'grd' ? G.grd : id === 'grdInvExp' ? G.exp : id === 'grdGLedger' ? G.gl : id === 'grdPaymentTerm' ? G.pay : null; }
    function onKey(e) {
        var t = e.target;
        if (e.key === 'Enter' && !e.ctrlKey && t && (t.tagName === 'INPUT' || t.tagName === 'SELECT') && t.type !== 'checkbox' && t.type !== 'radio' && t.type !== 'button') {
            var f = Array.prototype.filter.call(document.querySelectorAll('.dform input:not([type=hidden]):not([disabled]), .dform select:not([disabled]), .dform .dtcombo-input'), function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(t); if (i >= 0 && f[i + 1]) { e.preventDefault(); f[i + 1].focus(); }
        }
        var k = (e.key || '').toLowerCase();
        if (e.altKey && !e.ctrlKey && tabs.index() === 0) {
            if (e.key === '1') { e.preventDefault(); if (!$('toolStripButton1').disabled) printSlip(S.id); }
            else if (e.key === '2') { e.preventDefault(); if (!$('btnSlipFormat2').disabled) printSlip2(S.id); }
            else if (e.key === '3') { e.preventDefault(); if (!$('btnPrint').disabled) printVoucher(S.voucherHeadId); }
            return;
        }
        if (!e.ctrlKey) return;
        if (k === 't') { e.preventDefault(); if (tabs.index() === 1) { tabs.select('tabForm'); cb.cust.focus(); } else { tabs.select('tabHistory'); $('txtFromdateHistory').focus(); } return; }
        if (tabs.index() === 0) {
            if (e.key === 'Delete' && e.shiftKey) { e.preventDefault(); if (isShown('btnDelete') && !$('btnDelete').disabled) btnDelete(); }
            else if (e.key === 'Delete') {
                var tg = t.closest ? t.closest('.dgrid') : null, g = tg ? gridOf(tg.id) : null;
                if (g && g.curIndex() >= 0) { e.preventDefault(); if (g === G.grd) { G.grd.removeAt(g.curIndex()); recalc(); } else if (g === G.exp) expBtn('_del', g.cur(), g.curIndex()); else if (g === G.gl) glBtn('_del', g.cur(), g.curIndex()); else payBtn('_del', g.cur(), g.curIndex()); }
            }
            else if (k === 'n') { e.preventDefault(); reset(); }
            else if (k === 'r') { e.preventDefault(); refresh(); }
            else if (k === 's') { e.preventDefault(); if (isShown('btnSave') && !$('btnSave').disabled) btnSave(); }
            else if (k === 'u') { e.preventDefault(); if (isShown('btnUpdate') && !$('btnUpdate').disabled) btnUpdate(); }
            else if (k === 'p') { e.preventDefault(); if (!$('toolStripButton1').disabled) printSlip(S.id); }
            else if (k === 'd') { var tg2 = t.closest ? t.closest('.dgrid') : null; if (tg2 && tg2.id !== 'grd') { e.preventDefault(); if (tg2.id === 'grdInvExp') G.exp.addRow(expBlank()); else if (tg2.id === 'grdGLedger') G.gl.addRow(glBlank()); else if (tg2.id === 'grdPaymentTerm') G.pay.addRow(payBlank()); } }
            else if (e.key === 'F5') { e.preventDefault(); cb.cust.focus(); }
            else if (e.key === 'F10') { e.preventDefault(); openAttachmentDialog(); }
            else if (e.key === 'ArrowDown') { e.preventDefault(); $('grd').focus(); }
            else if (e.key === 'ArrowUp') { e.preventDefault(); cb.cust.focus(); }
            else if (e.key === 'ArrowRight') {
                e.preventDefault();
                var cur = t.closest ? t.closest('.dgrid') : null, ix = cur ? ORDER.indexOf(cur.id) : -1;
                $(ORDER[(ix + 1) % ORDER.length]).focus();
            }
        } else {
            if (k === 's') { e.preventDefault(); showHistory(); }
            else if (k === 'n') { e.preventDefault(); resetHistory(); }
            else if (k === 'r') { e.preventDefault(); refreshHistoryCombos(); }
            else if (e.key === 'F5') { e.preventDefault(); $('txtFromdateHistory').focus(); }
        }
    }
    function wire() {
        $('btnNew').onclick = reset; $('btnRefresh').onclick = refresh; $('btnSave').onclick = btnSave; $('btnUpdate').onclick = btnUpdate; $('btnDelete').onclick = btnDelete;
        $('btnAttachment').onclick = openAttachmentDialog;
        $('toolStripButton1').onclick = function () { printSlip(S.id); };
        $('btnSlipFormat2').onclick = function () { printSlip2(S.id); };
        $('btnPrint').onclick = function () { printVoucher(S.voucherHeadId); };
        $('toolStripButton3').onclick = openLoader;
        $('btnshortcutkeys').onclick = function () { SE.shortcuts(SHORT); };
        $('BtnNewHistory').onclick = resetHistory; $('BtnRefreshHistory').onclick = refreshHistoryCombos; $('btnshow').onclick = showHistory;
        $('txtdocno').readOnly = true;
        ['txtDueDays', 'txtFromNoHistory', 'txtToDocNoHistory'].forEach(function (id) { SE.digitsOnly($(id)); });
        ['txtFreightAmount', 'txtDiscountAmountHeader', 'txtExchangeRate'].forEach(function (id) {
            $(id).addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[\d.]/.test(e.key) && !e.ctrlKey) e.preventDefault(); });
            $(id).addEventListener('input', recalc);
        });
        $('txtExchangeRate').addEventListener('blur', function () { if ($('txtExchangeRate').value !== '') $('txtExchangeRate').value = fmtRate(num('txtExchangeRate')); });
        $('txtDueDays').addEventListener('input', function () { dueDaysChanged(); });
        $('DocDate').addEventListener('change', function () { if (vid(cb.term) === 1) $('DueDate').value = $('DocDate').value; else dueDaysChanged(); });
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
        var hook = false;
        document.addEventListener('keydown', function (e) { if (e.ctrlKey && e.altKey && !hook) { hook = true; SE.shortcuts(SHORT); setTimeout(function () { hook = false; }, 400); } });
    }

    function load() {
        makeCombos(); makeGrids();
        tabs = SE.tabs('tabControl1', function (id) { if (id === 'tabHistory') $('txtFromdateHistory').focus(); else cb.cust.focus(); });
        wire();
        SE.api(API + '/initial').then(function (d) {
            S.rights = d.rights || {}; S.branchId = d.branchId || 0; S.fyStart = d.fyStart || ''; S.lastDiscount = +d.lastDiscountAcId || 0;
            applyLists(d); multiCurrency(); commissionDebitUi(); fillAll();
            cb.hCust.setData((d.history && d.history.customers) || []);
            $('txtdocno').value = d.nextNo > 0 ? String(d.nextNo) : '';
            $('ChkPrintSlip').checked = true; $('ChkBok').checked = false;
            var t = SE.today(); $('DocDate').value = t; $('DueDate').value = t;
            $('txtFromdateHistory').value = SE.dateInput(S.fyStart) || t; $('txtToDateHistory').value = t;
            G.exp.setRows([expBlank()]); G.gl.setRows([glBlank()]); G.pay.setRows([payBlank()]);
            configDefault(); typeUi(); setButtons();
            if (S.lastDiscount > 0) cb.disc.setValue(S.lastDiscount);
            cb.cust.focus();
            var rec = SE.param('record'); if (rec) edit(+rec);
        }).catch(fail);
    }
    document.addEventListener('DOMContentLoaded', load);
})();
