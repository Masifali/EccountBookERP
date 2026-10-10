/*
 * Screen 847  frmSaleInvoiceEngr  (Architecture.WinApp.Mfg.Sale.frmSaleInvoiceEngr, Sale Invoice Against Gdn, document type 1660)
 * Page script. Desktop methods are named in the comments. Server: /sale/engr/mfg/sale-invoice-gdn/api
 * The invoice arithmetic lives on the server (POST /calc, the same code Save runs); this page runs the cell logic of the small grids.
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/engr/mfg/sale-invoice-gdn/api', DOC = 1660;
    var S = {
        id: 0, rights: {}, files: [], removedAtt: [], existing: [], voucherHeadId: 0, saleOrderId: 0, taxRefresh: false, ledger: 0,
        cust: [], terms: [], other: [], fr: [], taxAc: [], glAc: [], discAc: [], curr: [], taxTypes: [], dterms: [], cities: [],
        sub: false, multi: false, taxEditable: false, showBranchSrNo: false, fmt: { amountRound: 0, amount: 0, rate: 2, fcy: 0 }, def: {}, branchId: 0, fyStart: '',
        lastDiscount: 0, lastTax: 0, histBranches: []
    };
    var DTYPES = [{ Id: 1, Name: 'Flat' }, { Id: 2, Name: 'Percent' }];
    var cb = {}, G = {}, tabs, tabs2, seq = 0, hBranch = null;

    function fail(e) { return SE.dbError(e); }
    function msg(t, c) { return SE.alert(t, c); }
    function ci(r, k) { if (!r) return null; if (k in r) return r[k]; var l = String(k).toLowerCase(); for (var x in r) if (x.toLowerCase() === l) return r[x]; return null; }
    function pick(r, ks) { for (var i = 0; i < ks.length; i++) { var v = ci(r, ks[i]); if (v != null) return v; } return null; }
    function isShown(id) { var b = $(id); return !!b && b.style.display !== 'none' && !b.hidden; }
    function show(id, on) { var b = $(id); if (!b) return; b.style.display = on ? '' : 'none'; }
    function vid(c) { return +c.value() || 0; }
    function num(id) { return SE.toNum($(id).value); }
    function fmtAmt(v) { var d = S.fmt.amount; return SE.num(v, d, d); }
    function fmtRate(v) { var d = S.fmt.rate; return SE.num(v, d, d); }
    function fmtFcy(v) { var d = S.fmt.fcy; return SE.num(v, d, d); }
    function f2(v) { return SE.num(v, 2, 0); }
    function f3(v) { return SE.num(v, 3, 0); }
    function rnd(v, dec) { var p = Math.pow(10, Math.max(0, dec)); return (v < 0 ? -1 : 1) * Math.round(Math.abs(v) * p + 1e-9) / p; }
    function iso(d) { return d.getFullYear() + '-' + ('0' + (d.getMonth() + 1)).slice(-2) + '-' + ('0' + d.getDate()).slice(-2); }
    function parseIso(s) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(s || ''); return m ? new Date(+m[1], +m[2] - 1, +m[3]) : null; }
    function addDays(s, n) { var d = parseIso(s) || new Date(); d.setDate(d.getDate() + n); return iso(d); }
    function dayDiff(a, b) { var x = parseIso(a), y = parseIso(b); return x && y ? Math.round((x - y) / 86400000) : 0; }
    function p2(n) { return ('0' + n).slice(-2); }
    var MON = { jan: 1, feb: 2, mar: 3, apr: 4, may: 5, jun: 6, jul: 7, aug: 8, sep: 9, oct: 10, nov: 11, dec: 12 };
    function readDate(t) {
        t = String(t || '').trim(); var m;
        if ((m = /^(\d{4})-(\d{1,2})-(\d{1,2})/.exec(t))) return m[1] + '-' + p2(+m[2]) + '-' + p2(+m[3]);
        if ((m = /^(\d{1,2})[-\/ ]([A-Za-z]{3})[A-Za-z]*[-\/ ](\d{4})/.exec(t)) && MON[m[2].toLowerCase()]) return m[3] + '-' + p2(MON[m[2].toLowerCase()]) + '-' + p2(+m[1]);
        if ((m = /^(\d{1,2})[-\/](\d{1,2})[-\/](\d{4})/.exec(t))) return m[3] + '-' + p2(+m[2]) + '-' + p2(+m[1]);
        return '';
    }
    function isNumeric(t) { return /^\s*-?\d+(\.\d+)?\s*$/.test(String(t).replace(/,/g, '')); }
    function fmtBal(v) { v = Math.round(v); if (v === 0) return '0'; return v < 0 ? '(' + SE.num(-v, 0) + ')' : SE.num(v, 0); }   // #,#;(#,#);0
    function addCalc(row, flag) { var s = row.calc ? String(row.calc).split(',') : []; if (s.indexOf(flag) < 0) s.push(flag); row.calc = s.join(','); }

    /* ------------------------------------------------------------------ combos */
    function makeCombos() {
        cb.cust = XCombo('cmbsuppliername', { columns: [{ key: 'CompanyName', caption: 'Customer Name' }, { key: 'CityName', caption: 'City' }], textKey: 'CompanyName', popupWidth: 460, onSelect: custChanged });
        cb.dterm = XCombo('cmbDeliveryTerm', { columns: [{ key: 'DeliveryTerm', caption: 'Delivery Term' }], textKey: 'DeliveryTerm' });
        cb.term = XCombo('CmbPaymentTerm', { columns: [{ key: 'TermsDescription', caption: 'Payment Term' }], textKey: 'TermsDescription', onSelect: termChanged });
        cb.tax = XCombo('CmbTaxAccount', { columns: [{ key: 'AccountTitle', caption: 'Account Title' }], textKey: 'AccountTitle', popupWidth: 360 });
        cb.frCr = XCombo('CmbFreightAcCr', { columns: [{ key: 'AccountTitle', caption: 'Account Title' }], valueKey: 'K', textKey: 'AccountTitle', popupWidth: 360, onSelect: recalc });
        cb.frDr = XCombo('cmbFreightAcDr', { columns: [{ key: 'AccountTitle', caption: 'Account Title' }], valueKey: 'K', textKey: 'AccountTitle', popupWidth: 360, onSelect: recalc });
        cb.disc = XCombo('CmbDiscountAccount', { columns: [{ key: 'AccountTitle', caption: 'Account Title' }], textKey: 'AccountTitle', popupWidth: 360 });
        cb.cur = XCombo('cmbCurrency', { columns: [{ key: 'CurrencyCode', caption: 'Currency' }], textKey: 'CurrencyCode', onSelect: currencyLeave });
        cb.hCust = XCombo('CmbCustomerHistory', { columns: [{ key: 'Customer', caption: 'Customer' }], valueKey: 'Id', textKey: 'Customer', popupWidth: 380 });
        cb.hTerm = XCombo('CmbPaymentTermHistory', { columns: [{ key: 'PaymentTerm', caption: 'Payment Term' }], valueKey: 'Id', textKey: 'PaymentTerm', popupWidth: 300 });
    }
    function keepFill(c, rows, keepId) { c.setData(rows); if (keepId > 0 && !c.setValue(keepId)) c.clear(); }
    function applyLists(d) {
        S.sub = !!d.subsidiary; S.multi = !!d.multiCurrency; S.taxEditable = !!d.taxEditable;
        S.cust = d.customers || []; S.terms = d.terms || []; S.other = d.otherItems || []; S.taxAc = d.taxAccounts || []; S.glAc = d.glAccounts || []; S.discAc = d.discountAccounts || [];
        S.curr = d.currencies || []; S.taxTypes = d.taxTypes || []; S.dterms = d.deliveryTerms || []; S.cities = d.cities || [];
        S.fr = (d.freightAccounts || []).map(function (r) { var o = {}; for (var k in r) o[k] = r[k]; o.K = S.sub ? r.SupplierCustomerId : r.Id; return o; });
        S.def = d.defaults || {}; S.fmt = d.fmt || S.fmt;
        G.grd.spec.dec = S.fmt.amount; G.hist.spec.dec = S.fmt.amount; G.hd.spec.dec = S.fmt.amount;
    }
    function fillAll() {
        var a = vid(cb.cust), t = vid(cb.term), dt = vid(cb.dterm), tx = vid(cb.tax), fc = vid(cb.frCr), fd = vid(cb.frDr), dc = vid(cb.disc), cu = vid(cb.cur);
        keepFill(cb.cust, S.cust, a); keepFill(cb.term, S.terms, t); keepFill(cb.dterm, S.dterms, dt);
        keepFill(cb.tax, S.taxAc, tx); keepFill(cb.frCr, S.fr, fc); keepFill(cb.frDr, S.fr, fd); keepFill(cb.disc, S.discAc, dc); keepFill(cb.cur, S.curr, cu);
        G.exp.refresh(); G.gl.refresh(); G.pay.refresh(); G.grd.refresh();
    }
    function multiCurrency() {
        ['txtExchangeRate', 'txtFcyAmount'].forEach(function (id) { $(id).style.visibility = S.multi ? '' : 'hidden'; });
        Array.prototype.forEach.call(document.querySelectorAll('#groupBox1 > .dl'), function (l) { if (/^(Fcy Code|Echange Rate|Fcy Amount)$/.test(l.textContent)) l.style.visibility = S.multi ? '' : 'hidden'; });
        var c = $('cmbCurrency'); c.style.visibility = S.multi ? '' : 'hidden'; if (c.__dtcombo && c.__dtcombo.wrap) c.__dtcombo.wrap.style.visibility = S.multi ? '' : 'hidden';
        [G.grd, G.hd].forEach(function (g) { g.spec.cols.forEach(function (x) { if (x.k === 'FcyAmount') x.hide = !S.multi; }); g.refresh(); });
    }
    function configDefault() {                                                               // ConfigurationDefault
        if (S.def.baseCurrency > 0 && !(vid(cb.cur) > 0)) cb.cur.setValue(S.def.baseCurrency);
        if (S.def.baseRate > 0 && num('txtExchangeRate') === 0) $('txtExchangeRate').value = fmtRate(S.def.baseRate);
    }
    function custRow() { var id = vid(cb.cust); for (var i = 0; i < S.cust.length; i++) if (+S.cust[i].Id === id) return S.cust[i]; return null; }

    /* ------------------------------------------------------------------ server calculation */
    function reqBody(withAtt) {
        var b = {
            id: S.id, docNo: $('txtdocno').value, docDate: $('DocDate').value, customerId: vid(cb.cust),
            deliveryTerm: cb.dterm.text() || '', paymentTermId: vid(cb.term), dueDays: $('txtDueDays').value, dueDate: $('DueDate').value, currencyId: vid(cb.cur),
            exchangeRate: num('txtExchangeRate'), taxAccountId: vid(cb.tax), freightCrId: vid(cb.frCr), freightDrId: vid(cb.frDr), freightAmount: num('txtFreightAmount'),
            freightRemarks: $('txtFreightRemarks').value, discountAccountId: vid(cb.disc), discountAmount: num('txtDiscountAmountHeader'), manualBillNo: $('txtbillno').value,
            refNo: $('txtrefno').value, remarks: $('txtRemarks').value, taxRefresh: S.taxRefresh && S.id === 0,
            lines: G.grd.rows(), expenses: G.exp.rows(), journals: G.gl.rows(), payments: G.pay.rows()
        };
        if (withAtt) b.attachments = { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removedAtt };
        return b;
    }
    function showBill(bill, fcy) {
        $('txtBillAmount').value = fmtAmt(bill);
        $('txtFcyAmount').value = fmtFcy(fcy);
        $('txtAfterBillBalance').value = fmtBal(S.ledger + bill);
    }
    function applyCalc(r) {
        G.grd.setRows(r.lines || []);
        if (r.payments) G.pay.setRows(r.payments);
        showBill(+r.billAmount || 0, +r.fcyAmount || 0);
        G.grd.refresh();
    }
    /* BillAmount + proportions + txtExchangeRate_TextChanged + PaymentTermAmountCalculateFromPercent, as Save runs them */
    function recalc() {
        var my = ++seq;
        return SE.api(API + '/calc', { method: 'POST', body: reqBody(false), quiet: true }).then(function (r) { if (my === seq) applyCalc(r); }).catch(function (e) { if (my === seq) fail(e); });
    }
    function billAmount() { return num('txtBillAmount'); }
    function itemTotal() { return G.grd.rows().reduce(function (a, r) { return a + (+r.ItemAmount || 0); }, 0); }

    /* ------------------------------------------------------------------ header events */
    function custChanged() {                                                                 // cmbsuppliername_ValueChanged
        var c = custRow();
        $('txtCustomerCity').value = c ? c.CityName || '' : ''; $('txtCustomerCell').value = c ? c.MobileNo || '' : '';
        $('txtSupplierGLId').value = c ? c.GlAccountId || '' : '';
        ledger();
        recalc();
    }
    function ledger() {
        var id = vid(cb.cust);
        if (!(id > 0)) { S.ledger = 0; $('txtLedgerBalance').value = ''; $('txtAfterBillBalance').value = ''; return; }
        SE.api(API + '/ledger-balance' + SE.q({ customerId: id, docDate: $('DocDate').value }), { quiet: true }).then(function (r) {
            if (id !== vid(cb.cust)) return;
            S.ledger = Math.round(+r.balance || 0); $('txtLedgerBalance').value = fmtBal(S.ledger); $('txtAfterBillBalance').value = fmtBal(S.ledger + billAmount());
        }).catch(function () { });
    }
    function isCash() { return /^\s*cash\s*$/i.test(cb.term.text() || ''); }
    function termChanged() {                                                                 // CmbPaymentTerm_TextChanged
        if (isCash()) { $('txtDueDays').disabled = true; $('txtDueDays').value = ''; $('DueDate').value = $('DocDate').value; }
        else $('txtDueDays').disabled = false;
    }
    function dueDaysChanged() { $('DueDate').value = addDays($('DocDate').value, SE.toInt($('txtDueDays').value)); }
    function dueDateChanged() {
        var dd = $('DueDate').value, dc = $('DocDate').value;
        if (!dd) return;
        if (dd < dc) { $('DueDate').value = dc; $('txtDueDays').value = '0'; return; }
        $('txtDueDays').value = String(dayDiff(dd, dc));
    }
    function docDateLeave() {                                                                // DocDate_Leave
        if (S.id === 0 && G.grd.count() > 0) {
            var ids = G.grd.rows().map(function (r) { return +r.ItemId || 0; }).join(',');
            SE.api(API + '/tax-by-date' + SE.q({ itemIds: ',' + ids, docDate: $('DocDate').value }), { quiet: true }).then(function (dt) {
                G.grd.rows().forEach(function (m) {
                    var f = null; dt.forEach(function (t) { if (+t.ItemId === +m.ItemId) f = t; });
                    if (f) { m.TaxTypeId = +f.TaxNameId || 0; m.TaxPct = +f.TaxPercent || 0; } else { m.TaxTypeId = 0; m.TaxPct = 0; }
                    addCalc(m, 'other');
                });
                S.taxRefresh = true; G.grd.refresh(); recalc();
            }).catch(fail);
        }
        if (isCash()) $('DueDate').value = $('DocDate').value; else dueDaysChanged();
        ledger(); recalc();
    }
    function currencyLeave() {                                                               // cmbCurrency_Leave
        if (vid(cb.cur) === 0 || num('txtExchangeRate') !== 0) return;
        if (vid(cb.cur) !== S.def.baseCurrency) {
            return SE.api(API + '/last-rate' + SE.q({ currencyId: vid(cb.cur) })).then(function (dt) {
                $('txtExchangeRate').value = dt.length ? fmtRate(+pick(dt[0], ['LastExchRate']) || 0) : '0'; recalc();
            }).catch(fail);
        }
        $('txtExchangeRate').value = fmtRate(S.def.baseRate || 0); recalc();
    }
    function taxUi() {
        G.grd.spec.cols.forEach(function (c) { if (c.k === 'TaxTypeId' || c.k === 'TaxPct') c.edit = S.taxEditable; });
        G.grd.refresh();
    }

    /* ------------------------------------------------------------------ grids */
    function rf(f) { return function (v) { return v == null || v === '' ? '' : f(v); }; }
    function makeGrids() {
        G.grd = SE.grid('grd', { frozen: 0, dec: 0, footer: false, onEdit: gridEdit, cols: [
            { k: 'Id', hide: true }, { k: 'GdnId', hide: true }, { k: 'GdnDetailId', hide: true }, { k: 'GdnNo', t: 'GdnNo', w: 80 }, { k: 'WarehouseId', hide: true },
            { k: 'WarehouseName', t: 'WarehouseName', w: 150 }, { k: 'ItemId', hide: true }, { k: 'ItemCode', t: 'ItemCode', w: 80 }, { k: 'ItemName', t: 'ItemName', w: 150 },
            { k: 'PackUomId', hide: true }, { k: 'PackUom', t: 'PackUom', w: 60 }, { k: 'PackEquivalent', hide: true }, { k: 'VariantId', hide: true },
            { k: 'VariantDescription', t: 'Modal Description', w: 100 }, { k: 'CastingTypeId', hide: true }, { k: 'CastingType', t: 'CastingType', w: 90 },
            { k: 'ProductionStageId', hide: true }, { k: 'ProductionStage', t: 'ProductionStage', w: 120 },
            { k: 'Remarks', t: 'Specification/Remarks', w: 100, edit: true },
            { k: 'ItemQty', t: 'Qty', w: 70, f: 'n3' }, { k: 'NetWeight', t: 'Item FG Weight', w: 80, f: 'n3' },
            { k: 'ItemRate', t: 'ItemRate', w: 70, render: rf(fmtRate) }, { k: 'RateUomId', hide: true }, { k: 'RateUom', t: 'RateUom', w: 60 }, { k: 'RateEquivalent', hide: true },
            { k: 'AddLessRate', t: 'AddLessRate', w: 80, edit: true, render: rf(fmtRate) }, { k: 'NetRate', t: 'NetRate', w: 70, render: rf(fmtRate) },
            { k: 'DiscountType', t: 'DiscountType', w: 60, edit: true, list: function () { return DTYPES; }, lk: 'Id', lt: 'Name' },
            { k: 'DiscPct', t: 'Discount%', w: 70, edit: true, f: 'n3' }, { k: 'DiscountAmount', t: 'DiscountAmount', w: 90, f: 'amt' },
            { k: 'ItemAmount', t: 'ItemAmount', w: 90, f: 'amt' }, { k: 'FcyAmount', t: 'FcyAmount', w: 90, f: 'amt', hide: true },
            { k: 'TaxTypeId', t: 'Tax Type', w: 60, list: function () { return S.taxTypes; }, lk: 'TaxNameId', lt: 'TaxName' },
            { k: 'TaxPct', t: 'Tax%', w: 70, f: 'n3' }, { k: 'TaxAmount', t: 'TaxAmount', w: 90, f: 'amt' },
            { k: 'Expense', t: 'Expense', w: 90, f: 'n3' }, { k: 'Freights', t: 'Freights', w: 90, f: 'n3', hide: true }, { k: 'BillAmount', t: 'BillAmount', w: 110, f: 'amt' },
            { k: 'GpDate', t: 'GpDate', w: 73, f: 'date', edit: true }, { k: 'GpNo', t: 'GpNo', w: 45, edit: true }, { k: 'VehicleNo', t: 'VehicleNo', w: 80, edit: true },
            { k: 'CityId', t: 'City Name', w: 90, edit: true, list: function () { return S.cities; }, lk: 'Id', lt: 'CityName' }] });
        G.exp = SE.grid('grdInvExp', { footer: false, dec: 0, onBtn: expBtn, onEdit: expEdit, cols: [
            { k: '_add', t: '', w: 26, btn: '+' }, { k: '_del', t: '', w: 26, btn: 'X' },
            { k: 'ItemId', t: 'Item', w: 200, edit: true, list: function () { return S.other; }, lk: 'Id', lt: 'OtherItemName' },
            { k: 'Qty', t: 'Qty', w: 70, edit: true, f: 'n3' }, { k: 'Rate', t: 'Rate', w: 80, edit: true, render: rf(fmtRate) },
            { k: 'Amount', t: 'Amount', w: 90, f: 'n2' }, { k: 'Remarks', t: 'Remarks', w: 80, edit: true }] });
        G.pay = SE.grid('grdPaymentTerm', { footer: false, dec: 0, onEdit: payEdit, cols: [
            { k: 'PaymentTermId', t: 'Payment Term', w: 140, edit: true, list: function () { return S.terms; }, lk: 'Id', lt: 'TermsDescription' },
            { k: '%ofTotal', t: '%ofTotal', w: 80, edit: true, f: 'n3' }, { k: 'Amount', t: 'Amount', w: 100, edit: true, f: 'n2' },
            { k: 'DueDays', t: 'DueDays', w: 70, edit: true, f: 'n0' }, { k: 'DueDate', t: 'DueDate', w: 100, edit: true, f: 'date' },
            { k: 'Remarks', t: 'Remarks', w: 300, edit: true }] });
        G.gl = SE.grid('grdGLedger', { footer: false, dec: 0, onBtn: glBtn, onEdit: glEdit, cols: [
            { k: '_add', t: '', w: 26, btn: '+' }, { k: '_del', t: '', w: 26, btn: 'X' },
            { k: 'AccountId', t: 'Account', w: 220, edit: true, list: function () { return S.glAc; }, lk: 'Id', lt: 'AccountTitle' }, { k: 'GlAccountId', hide: true },
            { k: 'Remarks', t: 'Remarks', w: 100, edit: true }, { k: 'Percentage', t: 'Percentage', w: 70, edit: true, f: 'n3' },
            { k: 'Qty', t: 'Qty', w: 55, edit: true, f: 'n3' }, { k: 'Rate', t: 'Rate', w: 55, edit: true, f: 'n3' },
            { k: 'Debit', t: 'Debit', w: 80, edit: true, f: 'n3' }, { k: 'Credit', t: 'Credit', w: 80, edit: true, f: 'n3' }] });
        G.hist = SE.grid('grdHistory', { footer: false, dec: 0, frozen: 4, onDbl: function (r) { histEdit(r); }, onBtn: histBtn, onLink: histLink, onSel: histSelected, cols: [
            { k: 'Edit', t: '', w: 40, btn: 'Edit' }, { k: 'Print1', t: 'Print-1660', w: 80, btn: 'Print' }, { k: 'Print2', t: 'Print-1660A', w: 80, btn: 'Print' }, { k: 'Voucher', t: 'Voucher', w: 60, btn: 'Voucher' },
            { k: 'Id', hide: true }, { k: 'VoucherHeadId', hide: true }, { k: 'DocNo', t: 'DocNo', w: 60 }, { k: 'DocDate', t: 'DocDate', w: 73, f: 'date' }, { k: 'DueDate', t: 'DueDate', w: 73, f: 'date' },
            { k: 'ManualBillNo', t: 'ManualBillNo', w: 90 }, { k: 'SupplierCustomerId', hide: true }, { k: 'CustomerName', t: 'CustomerName', w: 170 }, { k: 'BillAmount', t: 'BillAmount', w: 90, f: 'amt' },
            { k: 'EntryUser', t: 'EntryUser', w: 80 }, { k: 'EntryDate', t: 'EntryDate', w: 135, f: 'dt12' }, { k: 'ModifyUser', t: 'ModifyUser', w: 80 }, { k: 'ModifyDate', t: 'ModifyDate', w: 135, f: 'dt12' },
            { k: 'ApprovedUser', t: 'ApprovedUser', w: 65 }, { k: 'ApprovedDate', t: 'ApprovedDate', w: 135, f: 'dt12' }, { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 100, link: true },
            { k: 'Remarks', t: 'Remarks', w: 100 }] });
        G.hd = SE.grid('grdDetail', { footer: false, dec: 0, cols: [
            { k: 'GdnNo', t: 'GdnNo', w: 80 }, { k: 'Warehouse', t: 'Warehouse', w: 150 }, { k: 'ItemCode', t: 'ItemCode', w: 80 }, { k: 'ItemName', t: 'ItemName', w: 150 }, { k: 'PackUom', t: 'Uom', w: 60 },
            { k: 'VariantDescription', t: 'Modal Description', w: 100 }, { k: 'CastingType', t: 'CastingType', w: 90 }, { k: 'ProductionStage', t: 'ProductionStage', w: 120 },
            { k: 'Remarks', t: 'Specification/Remarks', w: 100 }, { k: 'ItemQty', t: 'Qty', w: 70, f: 'n3' }, { k: 'NetWeight', t: 'Item FG Weight', w: 80, f: 'n3' },
            { k: 'ItemRate', t: 'ItemRate', w: 70, render: rf(fmtRate) }, { k: 'RateUom', t: 'RateUom', w: 60 }, { k: 'AddLessRate', t: 'AddLessRate', w: 150, render: rf(fmtRate) },
            { k: 'NetRate', t: 'NetRate', w: 70, render: rf(fmtRate) }, { k: 'DiscountType', t: 'DiscountType', w: 60 }, { k: 'DiscPct', t: 'Discount%', w: 70, f: 'n3' },
            { k: 'DiscountAmount', t: 'DiscountAmount', w: 90, f: 'amt' }, { k: 'ItemAmount', t: 'ItemAmount', w: 90, f: 'amt' }, { k: 'FcyAmount', t: 'FcyAmount', w: 90, f: 'amt', hide: true },
            { k: 'TaxType', t: 'TaxType', w: 80 }, { k: 'TaxPct', t: 'Tax%', w: 70, f: 'n3' }, { k: 'TaxAmount', t: 'TaxAmount', w: 90, f: 'amt' }, { k: 'Expense', t: 'Expense', w: 90, f: 'n3' },
            { k: 'BillAmount', t: 'BillAmount', w: 110, f: 'amt' }, { k: 'GpDate', t: 'GpDate', w: 73, f: 'date' }, { k: 'GpNo', t: 'GpNo', w: 45 }, { k: 'VehicleNo', t: 'VehicleNo', w: 80 },
            { k: 'CityName', t: 'City Name', w: 90 }] });
    }

    /* grd_CellUpdated: the edited cell sets the sticky calc flag, the server runs the formulas */
    function gridEdit(row, key, val) {
        if (key === 'Remarks' || key === 'GpNo' || key === 'VehicleNo') { row[key] = val; G.grd.refresh(); return; }
        if (key === 'GpDate') { var gd = readDate(val); if (gd) row.GpDate = gd; G.grd.refresh(); return; }
        if (key === 'CityId') { row.CityId = +val || 0; G.grd.refresh(); return; }
        if (key === 'DiscountType') { row.DiscountType = +val || 0; addCalc(row, 'rate'); }
        else if (key === 'TaxTypeId') {
            var id = +val || 0, t = null; S.taxTypes.forEach(function (x) { if (+x.TaxNameId === id) t = x; });
            row.TaxTypeId = id; row.TaxPct = t ? +t.TaxPercent || 0 : 0; S.taxRefresh = false; addCalc(row, 'other');
        } else {
            if (val !== '' && !isNumeric(val)) { G.grd.refresh(); return msg('Please Type Only Numeric Value'); }
            var n = SE.toNum(val);
            if (key === 'TaxPct') { if (n > 100) { n = 100; msg("Tax % Can't be greater than 100..."); } row.TaxPct = n; S.taxRefresh = false; addCalc(row, 'other'); }
            else { row[key] = n; addCalc(row, 'rate'); }                                      // AddLessRate, Discount%
        }
        G.grd.refresh(); recalc();
    }

    /* grdInvExp */
    function expBlank() { return { ItemId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' }; }
    function expBtn(k, row, i) {
        if (k === '_add') { G.exp.addRow(expBlank()); return; }
        if (G.exp.count() > 1) { G.exp.removeAt(i); recalc(); } else { G.exp.setRows([expBlank()]); recalc(); }
    }
    function expEdit(row, key, val) {
        if (key === 'ItemId') row.ItemId = +val || 0;
        else if (key === 'Remarks') row.Remarks = val;
        else {
            if (val !== '' && !isNumeric(val)) { G.exp.refresh(); return msg('Please Type Only Numeric Value'); }
            row[key] = SE.toNum(val); row.Amount = rnd((+row.Qty || 0) * (+row.Rate || 0), S.fmt.amountRound);
        }
        G.exp.refresh(); recalc();
    }

    /* grdPaymentTerm_CellUpdated */
    function payBlank(t) { return { PaymentTermId: t ? +t.Id : 0, '%ofTotal': 0, Amount: 0, DueDays: 0, DueDate: iso(new Date()), Remarks: '' }; }
    function payInit() { return S.terms.length ? S.terms.map(payBlank) : [payBlank(null)]; }          // one row per payment term
    function payEdit(row, key, val) {
        var bill = billAmount();
        if (key === 'Remarks') { row.Remarks = val; G.pay.refresh(); return; }
        if (key === 'PaymentTermId') {
            row.PaymentTermId = +val || 0;
            if (row.PaymentTermId === 3 && S.saleOrderId > 0) {                               // the desktop passes the loaded GDN id as SaleOrderId
                return SE.api(API + '/remaining' + SE.q({ orderId: S.saleOrderId, recId: S.id })).then(function (r) {
                    row.Amount = +r.balanceAmount || 0; row['%ofTotal'] = bill > 0 ? rnd(row.Amount * 100 / bill, 8) : 0; G.pay.refresh();
                }).catch(fail);
            }
            row.Amount = 0; G.pay.refresh(); return;
        }
        if (key === '%ofTotal') {
            if (!isNumeric(val)) { G.pay.refresh(); return msg('Please Type Only Numeric Value'); }
            var p = SE.toNum(val);
            if (p > 100) { p = 100; msg('%of Total Can\'t Greater than 100'); }
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

    /* grdGLedger_CellUpdated (Party Add/Less) */
    function glBlank() { return { AccountId: 0, GlAccountId: 0, Remarks: '', Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0 }; }
    function glBtn(k, row, i) {
        if (k === '_add') { G.gl.addRow(glBlank()); return; }
        SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
            if (!yes) return;
            if (G.gl.count() > 1) G.gl.removeAt(i); else G.gl.setRows([glBlank()]);
            recalc();
        });
    }
    function glEdit(row, key, val) {
        var r = S.fmt.amountRound;
        if (key === 'Remarks') { row.Remarks = val; G.gl.refresh(); return; }
        if (key === 'AccountId') {
            var id = +val || 0, c = custRow();
            if (!S.sub && id > 0 && c && +c.GlAccountId === id) {
                var i = G.gl.rows().indexOf(row); if (i >= 0) G.gl.rows()[i] = glBlank();
                G.gl.refresh(); return msg('Customer Account Not select');
            }
            row.AccountId = id; row.GlAccountId = id;
        } else {
            if (!isNumeric(val)) { G.gl.refresh(); return msg('Please Type Only Numeric Value'); }
            var n = SE.toNum(val);
            if (key === 'Qty' || key === 'Rate') {
                row[key] = n; row.Credit = rnd((+row.Qty || 0) * (+row.Rate || 0), r); row.Debit = 0; row.Percentage = 0;
            } else if (key === 'Percentage') {
                var tot = n; G.gl.rows().forEach(function (x) { if (x !== row) tot += +x.Percentage || 0; });
                if (tot > 100) { row.Percentage = n - (tot - 100); G.gl.refresh(); return msg('TotalPercentage Can not be Greater than 100...'); }
                row.Percentage = n; var amt = rnd(itemTotal() * n / 100, r);
                if (amt > 0) { row.Credit = amt; row.Debit = 0; } else { row.Debit = Math.abs(amt); row.Credit = 0; }
                row.Qty = 0; row.Rate = 0;
            } else if (key === 'Credit') {
                n = rnd(n, r); row.Credit = n;
                if ((+row.Debit || 0) > 0) { row.Credit = 0; G.gl.refresh(); return msg('Debit Side is aleady added'); }
            } else if (key === 'Debit') {
                n = rnd(n, r); row.Debit = n;
                if ((+row.Credit || 0) > 0) { row.Debit = 0; G.gl.refresh(); return msg('Credit Side is aleady added'); }
            }
        }
        G.gl.refresh(); recalc();
    }

    /* ------------------------------------------------------------------ multi-select combo (checked list) */
    function multi(id, onChange) {
        var el = $(id), box = el.querySelector('.msbox'), list = el.querySelector('.mslist'), items = [];
        function ids() { return items.filter(function (x) { return x.on; }).map(function (x) { return x.Id; }); }
        function paint() {
            box.textContent = items.filter(function (x) { return x.on; }).map(function (x) { return x.BranchName; }).join(', ');
            list.innerHTML = items.map(function (x, i) { return '<label><input type="checkbox" data-i="' + i + '"' + (x.on ? ' checked' : '') + '> ' + SE.esc(x.BranchName) + '</label>'; }).join('');
        }
        box.addEventListener('click', function () { el.classList.toggle('on'); });
        list.addEventListener('change', function (e) { var i = e.target.getAttribute('data-i'); if (i != null) { items[+i].on = e.target.checked; paint(); if (onChange) onChange(); } });
        document.addEventListener('click', function (e) { if (!el.contains(e.target)) el.classList.remove('on'); });
        return {
            setItems: function (rows) { items = (rows || []).map(function (r) { return { Id: +r.Id, BranchName: r.BranchName, on: true }; }); paint(); },
            ids: ids, csv: function () { var a = ids(); return a.length ? ',' + a.join(',') : ''; }
        };
    }

    /* ------------------------------------------------------------------ Load Gdn (toolStripButton -> frmLoadGdnEngr) */
    function openLoader() {
        var h = '<div class="lbar"><button type="button" id="ldNew">New</button><button type="button" id="ldRefresh">Refresh</button><button type="button" id="ldKeys">ShortCut Keys</button></div>' +
            '<div class="dbar" style="position:relative;height:24px;"><span class="dl white" style="left:6px;top:3px;">Pending Gdn For Invoice</span></div>' +
            '<div class="lf" style="position:relative;height:55px">' +
            '<span class="dl" style="left:5px;top:6px">Branch Name</span><div class="msel" id="ldBranch" style="left:5px;top:24px;width:204px;height:24px"><div class="msbox"></div><div class="mslist"></div></div>' +
            '<span class="dl" style="left:211px;top:6px">From Date</span><input class="f" type="date" id="ldFrom" style="left:211px;top:26px;width:125px;height:22px">' +
            '<span class="dl" style="left:338px;top:6px">To Date</span><input class="f" type="date" id="ldTo" style="left:338px;top:26px;width:125px;height:22px">' +
            '<span class="dl" style="left:465px;top:6px">Doc No From</span><input class="f" id="ldFromNo" style="left:465px;top:26px;width:88px;height:22px">' +
            '<span class="dl" style="left:555px;top:6px">Doc No To</span><input class="f" id="ldToNo" style="left:555px;top:26px;width:88px;height:22px">' +
            '<span class="dl" style="left:646px;top:6px">Customer Name</span><select class="f" id="ldCust" style="left:646px;top:24px;width:194px;height:26px"></select>' +
            '<button type="button" class="dbtn" id="ldSearch" style="left:841px;top:24px;width:60px;height:24px">Search</button>' +
            '<button type="button" class="dbtn" id="ldLoad" style="left:906px;top:24px;width:50px;height:24px">Load</button></div>' +
            '<div class="dbar" style="position:relative;height:20px;"><span class="dl white" style="left:6px;top:2px;">Records</span></div>' +
            '<div class="lgrid" id="ldGrd" style="height:230px"></div>' +
            '<div class="dbar" style="position:relative;height:20px;"><span class="dl white" style="left:6px;top:2px;">Selected Row Detail</span></div>' +
            '<div class="lgrid" id="ldDet" style="height:140px"></div>';
        var pop = SE.pop('Load Gdn', h, [{ t: 'Close' }], { wide: true });
        pop.open();
        var $$ = function (id) { return pop.body.querySelector('#' + id); };
        var bsel = multi_in(pop.body.querySelector('#ldBranch'), function () { fillCust(); });
        var lc = XCombo('ldCust', { columns: [{ key: 'ReferenceName', caption: 'Customer Name' }], valueKey: 'Id', textKey: 'ReferenceName', popupWidth: 300 });
        var g = SE.grid($$('ldGrd'), { footer: false, frozen: 1, onSel: loaderSel, cols: [
            { k: 'Sel', t: '', w: 30, sel: true }, { k: 'Id', hide: true }, { k: 'DocDate', t: 'DocDate', w: 73, f: 'date' }, { k: 'DocNo', t: 'DocNo', w: 60 }, { k: 'SupplierCustomerId', hide: true },
            { k: 'CustomerName', t: 'CustomerName', w: 170 }, { k: 'CustomerRefNo', t: 'CustomerRefNo', w: 100 }, { k: 'PaymentTerm', t: 'PaymentTerm', w: 70 }, { k: 'DueDays', t: 'DueDays', w: 60 },
            { k: 'OrderDueDate', t: 'OrderDueDate', w: 73, f: 'date' }, { k: 'DeliveryTerm', t: 'DeliveryTerm', w: 90 }, { k: 'GpDate', t: 'GpDate', w: 73, f: 'date' }, { k: 'GpNo', t: 'GpNo', w: 45 },
            { k: 'VehicleType', t: 'VehicleType', w: 80 }, { k: 'VehicleNo', t: 'VehicleNo', w: 80 }, { k: 'RemarksHeader', t: 'RemarksHeader', w: 100 }] });
        var gd = SE.grid($$('ldDet'), { footer: false, cols: [
            { k: 'WareHouseName', t: 'WareHouseName', w: 110 }, { k: 'ItemId', hide: true }, { k: 'ItemCode', t: 'ItemCode', w: 70 }, { k: 'ItemName', t: 'ItemName', w: 150 },
            { k: 'VariantDescription', t: 'VariantDescription', w: 100 }, { k: 'PackUom', t: 'PackUom', w: 60 }, { k: 'CastingType', t: 'CastingType', w: 80 }, { k: 'ProductionStage', t: 'ProductionStage', w: 100 },
            { k: 'ItemQty', t: 'ItemQty', w: 70, f: 'n3' }, { k: 'DispatchQty', t: 'DispatchQty', w: 70, f: 'n3' }, { k: 'BalQty', t: 'BalQty', w: 70, f: 'n3' }, { k: 'ItemRate', t: 'ItemRate', w: 70, f: 'n2' },
            { k: 'RateUom', t: 'RateUom', w: 60 }, { k: 'DiscountPrcnt', t: 'DiscountPrcnt', w: 80, f: 'n3' }, { k: 'DiscountAmount', t: 'DiscountAmount', w: 90, f: 'n2' }, { k: 'ItemAmount', t: 'ItemAmount', w: 90, f: 'n2' },
            { k: 'TaxType', t: 'TaxType', w: 70 }, { k: 'TaxPrcnt', t: 'TaxPrcnt', w: 70, f: 'n3' }, { k: 'TaxAmount', t: 'TaxAmount', w: 80, f: 'n2' }, { k: 'ItemNetAmount', t: 'ItemNetAmount', w: 90, f: 'n2' },
            { k: 'DetailRemarks', t: 'DetailRemarks', w: 100 }, { k: 'DeliveryCity', t: 'DeliveryCity', w: 90 }] });
        function loaderSel(r) {
            if (!r) { gd.setRows([]); return; }
            SE.api(API + '/loader/detail' + SE.q({ gdnId: r.Id }), { quiet: true }).then(function (rows) {
                if (!g.cur() || +g.cur().Id !== +r.Id) return; gd.setRows(rows);
            }).catch(function () { gd.setRows([]); });
        }
        function fillCust() {
            return SE.api(API + '/loader/customers' + SE.q({ branchIds: bsel.csv() }), { quiet: true }).then(function (rows) { lc.setData(rows); }).catch(fail);
        }
        function search() {
            return SE.api(API + '/loader/rows' + SE.q({ branchIds: bsel.csv(), fromDate: $$('ldFrom').value, toDate: $$('ldTo').value, fromNo: SE.toInt($$('ldFromNo').value),
                toNo: SE.toInt($$('ldToNo').value), customerId: vid(lc) })).then(function (rows) { g.setRows(rows); gd.setRows([]); if (rows.length) loaderSel(rows[0]); })
                .catch(function (e) { msg(e.message); });
        }
        function init() {
            return SE.api(API + '/loader/init').then(function (d) {
                bsel.setItems(d.branches); lc.setData(d.customers || []); lc.clear();
                $$('ldFrom').value = SE.dateInput(d.fyStart || S.fyStart) || SE.today(); $$('ldTo').value = SE.today(); $$('ldFromNo').value = ''; $$('ldToNo').value = '';
                g.setRows([]); gd.setRows([]);
            }).catch(fail);
        }
        SE.digitsOnly($$('ldFromNo')); SE.digitsOnly($$('ldToNo'));
        $$('ldSearch').onclick = search;
        $$('ldNew').onclick = init;
        $$('ldRefresh').onclick = fillCust;
        $$('ldKeys').onclick = function () { SE.shortcuts([['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+S', 'For Search'], ['Ctrl+L', 'For Load']]); };
        $$('ldLoad').onclick = function () {                                                 // btnLoadOnInvoice_Click_1
            var picked = g.checked();
            if (G.grd.count() > 0 && !(+G.grd.rows()[0].GdnId > 0)) return msg("You Can't Load Sale Order Because Direct Entry Already Exist");
            if (!picked.length) return msg('Check the row first');
            var c0 = +picked[0].SupplierCustomerId;
            for (var i = 1; i < picked.length; i++) if (+picked[i].SupplierCustomerId !== c0) return msg('Sorry! Check Rows Which Have Same Customer');
            if (G.grd.count() > 0 && vid(cb.cust) > 0 && vid(cb.cust) !== c0) return msg("Already Loaded Row's Have Different Customer. So you Can't Load Rows Of Different Customer!");
            loadGdn(picked.map(function (r) { return +r.Id; })).then(function (ok) { if (ok) pop.close(); });
        };
        init();
    }
    function multi_in(el, onChange) {                                                        // the same checked list, for an element inside the dialog
        var id = 'ms' + Math.floor(Math.random() * 1e9); el.id = id; return multi(id, onChange);
    }
    /* LoadInGridDetail + LoadPaymentDetailBySaleOrderIds */
    function loadGdn(ids) {
        return SE.api(API + '/load-gdn' + SE.q({ gdnIds: ids.join(','), customerId: G.grd.count() > 0 ? vid(cb.cust) : 0 })).then(function (d) {
            if (!d.lines || !d.lines.length) return true;
            var h = d.head || {};
            cb.cust.setValue(+h.SupplierCustomerId || 0); custChanged();
            $('cmbsuppliername').disabled = true; var w = $('cmbsuppliername').__dtcombo; if (w && w.syncFromSelect) w.syncFromSelect();
            $('txtrefno').value = h.ReferenceDocNo || '';
            var tm = (h.TermsDescription || '').trim(); S.terms.forEach(function (x) { if (String(x.TermsDescription).trim() === tm) cb.term.setValue(x.Id); }); termChanged();
            if (h.OrderDueDays != null && h.OrderDueDays !== '' && !isCash()) { $('txtDueDays').value = String(h.OrderDueDays); dueDaysChanged(); }
            var dtm = (h.DeliveryTerm || '').trim(); S.dterms.forEach(function (x) { if (String(x.DeliveryTerm).trim() === dtm) cb.dterm.setValue(x.Id); });
            if (h.RemarksHeader) $('txtRemarks').value = h.RemarksHeader;
            S.saleOrderId = +h.SaleOrderId || 0;
            var have = {}; G.grd.rows().forEach(function (r) { have[+r.GdnDetailId] = 1; });
            d.lines.forEach(function (l) { if (!have[+l.GdnDetailId]) { have[+l.GdnDetailId] = 1; l.GpDate = SE.dateInput(l.GpDate); G.grd.rows().push(l); } });
            var pay = d.payments || [];
            if (pay.length) {
                var sum = G.pay.rows().reduce(function (a, r) { return a + (+r.Amount || 0); }, 0);
                pay.forEach(function (p) { p.DueDate = SE.dateInput(p.DueDate) || $('DocDate').value; });
                if (sum === 0) G.pay.setRows(pay);
                else pay.forEach(function (p) { G.pay.rows().forEach(function (r) { if (+r.PaymentTermId === +p.PaymentTermId) { r['%ofTotal'] = p['%ofTotal']; r.Amount = p.Amount; r.DueDays = p.DueDays; r.DueDate = p.DueDate; r.Remarks = p.Remarks; } }); });
            }
            G.grd.refresh(); G.pay.refresh();
            return recalc().then(function () { return true; });
        }).catch(function (e) { msg(e.message); return false; });
    }

    /* ------------------------------------------------------------------ Save / Update / Delete */
    function insert(update) {
        return SE.ask(update ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
            if (!yes) return;
            return SE.api(API, { method: 'POST', body: reqBody(true) }).then(function (r) {
                return msg(r.message || 'Record Save Successfully').then(function () {
                    var slip = $('ChkBok').checked, vch = $('ChkVoucherPreview').checked;
                    return reset().then(function () {
                        if (slip) printSlip(r.id);
                        if (vch && r.voucherHeadId > 0) printVoucher(r.voucherHeadId);
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
        ['btnPrint1660', 'btnPrint1660A', 'btnPrint103'].forEach(function (id) { $(id).disabled = !S.rights.print; });
    }

    /* ------------------------------------------------------------------ Reset / Refresh */
    function reset() {                                                                       // Reset(): reference no, payment/delivery term, due days, tax and discount account and DocDate stay
        S.id = 0; S.files = []; S.removedAtt = []; S.existing = []; S.voucherHeadId = 0; S.saleOrderId = 0; S.taxRefresh = false; S.ledger = 0;
        cb.cust.clear(); $('cmbsuppliername').disabled = false; var w = $('cmbsuppliername').__dtcombo; if (w && w.syncFromSelect) w.syncFromSelect();
        cb.frCr.clear(); cb.frDr.clear();
        ['txtbillno', 'txtRemarks', 'txtFreightAmount', 'txtFreightRemarks', 'txtBillAmount', 'txtFcyAmount', 'txtExchangeRate', 'txtCustomerCity', 'txtCustomerCell', 'txtLedgerBalance', 'txtAfterBillBalance', 'txtSupplierGLId']
            .forEach(function (id) { $(id).value = ''; });
        G.grd.setRows([]); G.exp.setRows([expBlank()]); G.gl.setRows([glBlank()]); G.pay.setRows(payInit());
        configDefault(); taxUi(); setButtons();
        return nextNo();
    }
    function nextNo() {
        return SE.api(API + '/next-no').then(function (r) { if (r.nextNo > 0) $('txtdocno').value = String(r.nextNo); $('txtBranchSrNo').value = r.branchSrNo > 0 ? String(r.branchSrNo) : ''; }).catch(fail);
    }
    function refresh() {                                                                     // btnFrmRefresh_Click
        return SE.api(API + '/refresh').then(function (d) { applyLists(d); multiCurrency(); fillAll(); configDefault(); taxUi(); }).catch(fail);
    }

    /* ------------------------------------------------------------------ ReadById */
    function edit(id) {
        return SE.api(API + '/' + id).then(function (d) {
            var h = d.head;
            tabs.select('tabForm');
            S.id = id; S.files = []; S.removedAtt = []; S.existing = d.attachments || []; S.voucherHeadId = +d.voucherHeadId || 0; S.saleOrderId = +d.saleOrderId || 0; S.taxRefresh = false;
            $('txtdocno').value = h.DocNo; $('txtBranchSrNo').value = h.BranchSrNo > 0 ? String(h.BranchSrNo) : ''; $('DocDate').value = SE.dateInput(h.DocDate);
            cb.cust.setValue(+h.SupplierCustomerId || 0); $('cmbsuppliername').disabled = true; var w = $('cmbsuppliername').__dtcombo; if (w && w.syncFromSelect) w.syncFromSelect();
            var c = custRow(); $('txtCustomerCity').value = c ? c.CityName || '' : ''; $('txtCustomerCell').value = c ? c.MobileNo || '' : ''; $('txtSupplierGLId').value = c ? c.GlAccountId || '' : '';
            $('txtrefno').value = h.SupplierReferenceNo || ''; $('txtbillno').value = h.ManualBillNo || '';
            cb.tax.setValue(+h.TaxAccountId || 0);
            cb.frCr.setValue(S.sub ? (+h.TransporterCreditPartyId || 0) : (+h.TransporterId || 0));
            cb.frDr.setValue(S.sub ? (+h.TransporterDebitPartyId || 0) : (+h.TransporterDebitGLId || 0));
            $('txtFreightAmount').value = f3(+h.FreightAmount || 0); $('txtFreightRemarks').value = h.FreightRemark || '';
            $('txtRemarks').value = h.RemarksHeader || '';
            var dtm = (h.DeliveryTerm || '').trim(); cb.dterm.clear(); S.dterms.forEach(function (x) { if (x.DeliveryTerm === dtm) cb.dterm.setValue(x.Id); });
            cb.term.setValue(+h.PaymentTermId || 0); termChanged();
            if (!isCash()) $('txtDueDays').value = h.DueDays == null ? '' : String(h.DueDays);
            $('DueDate').value = SE.dateInput(h.DueDate) || $('DocDate').value;
            cb.cur.setValue(+h.CurrencyId || 0);
            if (+h.CurrencyId > 0) $('txtExchangeRate').value = h.ExchangeRate == null ? '' : String(h.ExchangeRate); else configDefault();
            $('txtDiscountAmountHeader').value = f3(+h.DiscountAmount || 0); cb.disc.setValue(+h.DiscountAccountId || 0);
            G.grd.setRows((d.lines || []).map(function (l) { l.GpDate = SE.dateInput(l.GpDate); return l; }));
            G.exp.setRows((d.expenses && d.expenses.length) ? d.expenses : [expBlank()]);
            G.gl.setRows((d.journals && d.journals.length) ? d.journals : [glBlank()]);
            G.pay.setRows((d.payments && d.payments.length) ? d.payments.map(function (p) { p.DueDate = SE.dateInput(p.DueDate) || $('DocDate').value; return p; }) : payInit());
            S.ledger = 0; ledger();
            showBill(+h.BillAmount || 0, +h.FcyAmount || 0);
            taxUi(); setButtons();
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ history */
    function showHistory() {
        var dt = $('rdentrydate').checked ? 'entry' : ($('rdmodifydate').checked ? 'modify' : ($('rdapproveddate').checked ? 'approved' : 'document'));
        var q = { branchIds: hBranch.csv(), dateType: dt, fromDate: $('txtFromdateHistory_chk').checked ? $('txtFromdateHistory').value : '', toDate: $('txtToDateHistory_chk').checked ? $('txtToDateHistory').value : '',
            fromNo: SE.toInt($('txtFromNoHistory').value), toNo: SE.toInt($('txtToDocNoHistory').value), customerId: vid(cb.hCust), paymentTermId: vid(cb.hTerm) };
        return SE.api(API + '/history' + SE.q(q)).then(function (rows) { G.hist.setRows(rows); G.hd.setRows([]); if (rows.length) histSelected(rows[0]); }).catch(function (e) { return msg(e.message, 'Error Message'); });
    }
    function resetHistory() {
        $('txtFromdateHistory').value = SE.dateInput(S.fyStart) || SE.today(); $('txtToDateHistory').value = SE.today();
        $('txtFromNoHistory').value = ''; $('txtToDocNoHistory').value = ''; cb.hCust.clear(); cb.hTerm.clear(); G.hist.setRows([]); G.hd.setRows([]); $('drdocdate').checked = true;
    }
    function refreshHistoryCombos() {                                                        // HistoryComboFill
        return SE.api(API + '/history-combos' + SE.q({ branchIds: hBranch.csv() })).then(function (h) { cb.hCust.setData(h.customers || []); cb.hTerm.setData(h.paymentTerms || []); })
            .catch(function (e) { return msg(e.message, 'Error Message'); });
    }
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
        else if (k === 'Print1') printSlip(+r.Id, 'No Record Found For Display');
        else if (k === 'Print2') printSlip2(+r.Id, 'Record Not Found For Display');
        else if (k === 'Voucher') printVoucher(+r.VoucherHeadId, 'No Record Found For Display');
    }
    function histLink(k, r) { if (k === 'NoOfAttachments') showAttachments(+r.Id); }

    /* ------------------------------------------------------------------ print / attachments */
    function printSlip(id, m) {
        if (!(id > 0)) return msg(m || 'No Record Found For Display');
        SE.printRpt('1660_SaleInvoiceSlipEngr.rpt', { id: id });
    }
    function printSlip2(id, m) {
        if (!(id > 0)) return msg(m || 'Record Not Found For Display');
        SE.printRpt('1660A_SaleInvoiceSlipEngr.rpt', { id: id });
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
        ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Alt+1', 'Print Slip 1660'], ['Alt+2', 'Print Slip 1660A'], ['Alt+3', 'Print Voucher 103'],
        ['Ctrl+F5', 'Focus on Doc Date Name'], ['Ctrl+F10', 'Attachments'], ['Ctrl+T', 'Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+D', 'For Adding an row in Focused Grid'], ['Ctrl+Delete', 'For Deleting an row of Focused Grid'], ['Ctrl+ArrowDown', 'For Focus Grids'],
        ['Ctrl+ArrowUp', 'For Focus On Customer Name in Detail Box'], ['Ctrl+ArrowRight', 'For Change Focus from one Grid To another Grid'],
        ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    var ORDER = ['grd', 'grdInvExp', 'grdGLedger', 'grdPaymentTerm'];
    function gridOf(id) { return id === 'grd' ? G.grd : id === 'grdInvExp' ? G.exp : id === 'grdGLedger' ? G.gl : id === 'grdPaymentTerm' ? G.pay : null; }
    function onKey(e) {
        var t = e.target;
        if (e.key === 'Enter' && !e.ctrlKey && t && (t.tagName === 'INPUT' || t.tagName === 'SELECT') && t.type !== 'checkbox' && t.type !== 'radio' && t.type !== 'button' && !(t.classList && t.classList.contains('cell'))) {
            var f = Array.prototype.filter.call(document.querySelectorAll('#tabForm input:not([type=hidden]):not([disabled]), #tabForm select:not([disabled]), #tabForm .dtcombo-input'), function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(t); if (i >= 0 && f[i + 1]) { e.preventDefault(); f[i + 1].focus(); }
        }
        var k = (e.key || '').toLowerCase();
        if (e.altKey && !e.ctrlKey && tabs.index() === 0) {
            if (e.key === '1') { e.preventDefault(); if (!$('btnPrint1660').disabled) printSlip(S.id); }
            else if (e.key === '2') { e.preventDefault(); if (!$('btnPrint1660A').disabled) printSlip2(S.id); }
            else if (e.key === '3') { e.preventDefault(); if (!$('btnPrint103').disabled) printVoucher(S.voucherHeadId); }
            return;
        }
        if (!e.ctrlKey) return;
        if (k === 't') { e.preventDefault(); if (tabs.index() === 1) { tabs.select('tabForm'); cb.cust.focus(); } else { tabs.select('tabHistory'); $('txtFromdateHistory').focus(); } return; }
        if (tabs.index() === 0) {
            if (e.key === 'Delete' && e.shiftKey) { e.preventDefault(); if (isShown('btnDelete') && !$('btnDelete').disabled) btnDelete(); }
            else if (e.key === 'Delete') {
                var tg = t.closest ? t.closest('.dgrid') : null, g = tg ? gridOf(tg.id) : null;
                if (g && g.curIndex() >= 0) { e.preventDefault(); if (g === G.grd) { G.grd.removeAt(g.curIndex()); recalc(); } else if (g === G.exp) expBtn('_del', g.cur(), g.curIndex()); else if (g === G.gl) glBtn('_del', g.cur(), g.curIndex()); }
            }
            else if (k === 'n') { e.preventDefault(); reset(); }
            else if (k === 'r') { e.preventDefault(); refresh(); }
            else if (k === 's') { e.preventDefault(); if (isShown('btnSave') && !$('btnSave').disabled) btnSave(); }
            else if (k === 'u') { e.preventDefault(); if (isShown('btnUpdate') && !$('btnUpdate').disabled) btnUpdate(); }
            else if (k === 'p') { e.preventDefault(); if (!$('btnPrint1660').disabled) printSlip(S.id); }
            else if (k === 'd') { var tg2 = t.closest ? t.closest('.dgrid') : null; if (tg2 && tg2.id !== 'grd' && tg2.id !== 'grdPaymentTerm') { e.preventDefault(); if (tg2.id === 'grdInvExp') G.exp.addRow(expBlank()); else if (tg2.id === 'grdGLedger') G.gl.addRow(glBlank()); } }
            else if (e.key === 'F5') { e.preventDefault(); $('DocDate').focus(); }
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
        $('btnPrint1660').onclick = function () { printSlip(S.id); };
        $('btnPrint1660A').onclick = function () { printSlip2(S.id); };
        $('btnPrint103').onclick = function () { printVoucher(S.voucherHeadId); };
        $('btnLoadGdn').onclick = openLoader;
        $('btnshortcutkeys').onclick = function () { SE.shortcuts(SHORT); };
        $('BtnNewHistory').onclick = resetHistory; $('BtnRefreshHistory').onclick = refreshHistoryCombos; $('btnshow').onclick = showHistory;
        ['txtDueDays', 'txtFromNoHistory', 'txtToDocNoHistory'].forEach(function (id) { SE.digitsOnly($(id)); });
        ['txtFreightAmount', 'txtDiscountAmountHeader', 'txtExchangeRate'].forEach(function (id) {
            $(id).addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[\d.]/.test(e.key) && !e.ctrlKey) e.preventDefault(); });
            $(id).addEventListener('input', recalc);
        });
        $('txtExchangeRate').addEventListener('blur', function () { if ($('txtExchangeRate').value !== '') $('txtExchangeRate').value = fmtRate(num('txtExchangeRate')); });
        $('txtDueDays').addEventListener('input', dueDaysChanged);
        $('DueDate').addEventListener('change', dueDateChanged);
        $('DocDate').addEventListener('change', docDateLeave);
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
        var hook = false;
        document.addEventListener('keydown', function (e) { if (e.ctrlKey && e.altKey && !hook) { hook = true; SE.shortcuts(SHORT); setTimeout(function () { hook = false; }, 400); } });
    }

    function load() {
        makeCombos(); makeGrids();
        hBranch = multi('cmbBranchHistory', refreshHistoryCombos);
        tabs = SE.tabs('tabControl1', function (id) { if (id === 'tabHistory') $('txtFromdateHistory').focus(); else cb.cust.focus(); });
        tabs2 = SE.tabs('tabControl2');
        wire();
        SE.api(API + '/initial').then(function (d) {
            S.rights = d.rights || {}; S.branchId = d.branchId || 0; S.fyStart = d.fyStart || ''; S.lastDiscount = +d.lastDiscountAcId || 0; S.lastTax = +d.lastSalesTaxAcId || 0;
            S.showBranchSrNo = !!d.showBranchSrNo;
            applyLists(d); multiCurrency(); fillAll();
            $('txtBranchSrNo').style.display = S.showBranchSrNo ? '' : 'none';
            hBranch.setItems(d.historyBranches || []);
            $('txtdocno').value = d.nextNo > 0 ? String(d.nextNo) : ''; $('txtBranchSrNo').value = d.branchSrNo > 0 ? String(d.branchSrNo) : '';
            var t = SE.today(); $('DocDate').value = t; $('DueDate').value = t;
            $('txtFromdateHistory').value = SE.dateInput(S.fyStart) || t; $('txtToDateHistory').value = t;
            G.exp.setRows([expBlank()]); G.gl.setRows([glBlank()]); G.pay.setRows(payInit());
            if (S.terms.length > 1) cb.term.setValue(S.terms[1].Id); else if (S.terms.length) cb.term.setValue(S.terms[0].Id);   // Payment Term activated at Rows[1]
            if (S.dterms.length) cb.dterm.setValue(S.dterms[0].Id);                                                                   // Delivery Term activated at Rows[0]
            termChanged();
            if (S.lastDiscount > 0) cb.disc.setValue(S.lastDiscount);
            if (S.lastTax > 0) cb.tax.setValue(S.lastTax);
            configDefault(); taxUi(); setButtons();
            refreshHistoryCombos();
            cb.cust.focus();
            var rec = SE.param('record'); if (rec) edit(+rec);
        }).catch(fail);
    }
    document.addEventListener('DOMContentLoaded', load);
})();
