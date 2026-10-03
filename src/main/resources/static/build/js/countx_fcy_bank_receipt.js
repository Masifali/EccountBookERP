/* ============================================================================================
 * countx_fcy_bank_receipt.js - Acfrmfcbankreceipt.cs (Architecture.WinApp.Account_Definition), screens 707
 * "FCY Bank Receipt" (Banking Managment) and 43 "FCY Receipt" (Accounts Transaction), DocumentTypeId 203.
 * Page /accounts/vouchers/fcy-bank-receipt (templates/accounts/vouchers/fcy_bank_receipt.html, drawn at the
 * InitializeComponent geometry). Derived 2026-10-02 from countx_export_fcy_receipts.js (the 794 port of the same
 * form): every button, Leave, TextChanged, CheckedChanged, grid button, cell update, double-click and shortcut
 * of the desktop form has its counterpart here with the desktop's messages and order; data comes from
 * /accounts/api/banking/fcy-bank-receipt (FcyBankReceiptController -> FcyBankReceiptService rights ->
 * FcyBankReceiptsService -> the desktop's own procedures, MakeVoucher and DAL SetDate). Prints:
 * 102-AcRptPaymentReceiptsVoucherSlip.rpt (acc-102) and 516-ExImRptFCBankReceipts.rpt (exp-516) via CrystalPrint.
 *
 * Button contract on every action: disabled + spinner while the request runs, duplicates ignored,
 * re-enabled on success and on failure (busy()).
 * ============================================================================================ */
(function (global) {
    'use strict';

    var API = '/accounts/api/banking/fcy-bank-receipt';

    function $id(id) { return document.getElementById(id); }
    function box(m) { global.alert(m); }
    function ask(m) { return global.confirm(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    function netI(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isFinite(n) ? n : 0; }
    function netD(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isFinite(n) ? n : 0; }
    function group(s) { var p = s.split('.'); p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ','); return p.join('.'); }
    /* "#,##0.####"-style (dec optional digits) */
    function fmt(v, dec) { var n = netD(v); if (dec === undefined) dec = 4; var s = n.toFixed(dec); if (dec > 0) s = s.replace(/\.?0+$/, ''); return group(s); }
    /* "#,#.###" / "#,#.##" - an empty string for zero */
    function fmtH(v, dec) { var n = netD(v); if (n === 0) return ''; var s = fmt(n, dec); return s.replace(/^(-?)0\./, '$1.'); }
    /* "#,##0,####" (NetAmountFC's bank amount) - an integer with grouping */
    function fmtInt(v) { return group(Math.round(netD(v)).toFixed(0)); }
    /* "0,0" - grouped integer, at least two digits */
    function fmt00(v) { var n = Math.round(netD(v)); var neg = n < 0; var s = String(Math.abs(n)); if (s.length < 2) s = '0' + s; return (neg ? '-' : '') + group(s); }
    /* double.ToString() */
    function dstr(v) { var n = netD(v); return String(parseFloat(n.toPrecision(15))); }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function isoOf(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function today() { return isoOf(new Date()); }
    function daysAgo(n) { var d = new Date(); d.setDate(d.getDate() - n); return isoOf(d); }
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function isoDate(v) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(str(v).trim()); return m ? m[1] + '-' + m[2] + '-' + m[3] : ''; }
    function ddMMMyy(v) { var s = isoDate(v); if (!s) return ''; return s.substring(8, 10) + '-' + MON[parseInt(s.substring(5, 7), 10) - 1] + '-' + s.substring(2, 4); }
    function ddMMMyyHm(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})[T ](\d{2}):(\d{2})/.exec(str(v).trim());
        if (!m) return ddMMMyy(v);
        var h = parseInt(m[4], 10), tt = h >= 12 ? 'PM' : 'AM'; h = h % 12; if (h === 0) h = 12;
        return m[3] + '-' + MON[parseInt(m[2], 10) - 1] + '-' + m[1].substring(2) + ' ' + pad(h) + ':' + m[5] + ' ' + tt;
    }
    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) {
            if (b.disabled || b.classList.contains('is-busy')) return Promise.resolve();
            b.disabled = true; b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } };
        var p;
        try { p = fn(); } catch (e) { done(); box(e.message); return Promise.resolve(); }
        if (p && typeof p.then === 'function') return p.then(done, function (e) { done(); if (e && e.message) box(e.message); });
        done();
        return Promise.resolve(p);
    }
    function parse(r) {
        return r.text().then(function (t) {
            var b = null;
            try { b = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
            if (!r.ok) throw new Error((b && b.message) || ('Request failed (' + r.status + ')'));
            return b;
        });
    }
    function getJson(url) { return fetch(url, { headers: { 'Accept': 'application/json' }, credentials: 'same-origin' }).then(parse); }
    function postJson(url, data) {
        var h = { 'Accept': 'application/json', 'Content-Type': 'application/json' };
        var t = document.querySelector('meta[name="_csrf"]'), n = document.querySelector('meta[name="_csrf_header"]');
        if (t && n && t.getAttribute('content') && n.getAttribute('content')) h[n.getAttribute('content')] = t.getAttribute('content');
        return fetch(url, { method: 'POST', headers: h, credentials: 'same-origin', body: JSON.stringify(data) }).then(parse);
    }
    function refreshCombos() { if (global.DesktopCombo) global.DesktopCombo.refresh(); }
    function show(id, on) {
        var e = $id(id); if (!e) return;
        e.classList.toggle('is-hidden', !on);
        var w = e.tagName === 'SELECT' && e.closest ? e.closest('.dtcombo-wrap') : null;   /* countx_prod_combo wrap */
        if (w) w.classList.toggle('is-hidden', !on);
    }
    /** lblConsignee / CmbConsignee / labFiNo / txtFINo Visible together; txtRemarks.Width 234 when shown, 555 when hidden (PaymentTerm / RESETMAIN / ReadById). */
    function consigneeVisible(on) {
        ['lblConsignee', 'CmbConsignee', 'labFiNo', 'txtFINo'].forEach(function (id) { show(id, on); });
        var r = $id('txtRemarks'); if (r) r.style.width = (on ? 234 : 555) + 'px';
    }
    function visible(id) { var e = $id(id); return !!e && !e.classList.contains('is-hidden'); }
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return null;
    }
    function bind(id, rows, valueCol, textCol) {
        var s = $id(id); if (!s) return;
        var keep = s.value;
        var h = '<option value="0"></option>';
        (rows || []).forEach(function (r) { h += '<option value="' + esc(col(r, valueCol)) + '">' + esc(col(r, textCol)) + '</option>'; });
        s.innerHTML = h;
        s.value = keep;
        if (s.value !== keep) s.value = '0';
        refreshCombos();
    }
    function clearCombo(id) { var s = $id(id); if (!s) return; s.innerHTML = '<option value="0"></option>'; s.value = '0'; refreshCombos(); }
    function setVal(id, v) { var s = $id(id); if (!s) return; s.value = str(v); if (s.value !== str(v)) s.value = '0'; refreshCombos(); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function selText(id) { var s = $id(id); if (!s || s.selectedIndex < 0) return ''; var o = s.options[s.selectedIndex]; return o && o.value !== '0' ? o.text : ''; }
    function setText(id, v) { var e = $id(id); if (e) e.value = str(v); }
    function setEnabled(id, on) { var e = $id(id); if (e) { e.disabled = !on; refreshCombos(); } }
    function focus(id) { var e = $id(id); if (!e) return; var w = e.closest && e.closest('.dtcombo-wrap'); var t = w ? w.querySelector('.dtcombo-input') : e; try { (t || e).focus(); } catch (x) { /* ignore */ } }
    function radio(name) { var r = document.querySelector('input[name="' + name + '"]:checked'); return r ? r.value : ''; }
    function cancel() {
        global.close();
        setTimeout(function () { if (!global.closed) { if (history.length > 1) history.back(); else location.href = '/accounts/dashboard'; } }, 150);
    }
    function sumRow(cols, rows, numCols, lead, fmtFn) {
        if (!rows.length) return '';
        var sums = {};
        rows.forEach(function (r) { numCols.forEach(function (c) { sums[c] = (sums[c] || 0) + netD(r[c]); }); });
        var t = '<tr>' + (lead ? '<td class="lbl" colspan="' + lead + '">&Sigma;</td>' : '');
        cols.forEach(function (c, i) {
            var isNum = numCols.indexOf(c) >= 0;
            t += '<td' + (isNum ? '' : ' class="lbl"') + '>' + (isNum ? esc((fmtFn || fmt)(sums[c])) : (i === 0 && !lead ? '&Sigma;' : '')) + '</td>';
        });
        return t + '</tr>';
    }

    // ------------------------------------------------------------------------------ state

    var PERM = { Save: true, Update: true, Print: true };
    var ACCOUNTS = [], PAYMENT_TERMS = [];
    var S = {
        id: 0, previousBalance: 0, voucherHeadId: 0, branchReset: false, docSeq: 0,
        charges: [], updIndex: -1,
        breakups: [], brkGridCleared: false, updBrk: -1, gdBreakUpId: 0,
        party: [], terms: [],
        brkInvoices: [], gds: [], lastActive: ''
    };
    var HIST = [], CUR = { hist: -1 };

    // ------------------------------------------------------------------------------ tabs

    function tab(panelId) {
        document.querySelectorAll('.win-tabs[data-tabs="fcy"] .win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === panelId); });
        ['fcyForm', 'fcyHistory'].forEach(function (p) { $id(p).classList.toggle('is-active', p === panelId); });
        var onHist = panelId === 'fcyHistory';
        var fb = $id('btnFcyFooterHistory');
        fb.querySelector('span').textContent = onHist ? 'Form' : 'History';
        fb.querySelector('i').className = onHist ? 'fa fa-file-text-o' : 'fa fa-history';
        if (onHist) focus('FromDateHistory'); else focus('txtdate');
    }
    function onHistory() { return $id('fcyHistory').classList.contains('is-active'); }
    function toggleHistory() { tab(onHistory() ? 'fcyForm' : 'fcyHistory'); }

    // ------------------------------------------------------------------------------ helpers on the header

    function termText() { return selText('cmbPaymentTerm'); }
    function termId() { return netI(val('cmbPaymentTerm')); }
    function acctType() { return netI(val('cmbaccounttype')); }
    function breakupMode() { return termId() === 4 && acctType() === 1; }

    // ------------------------------------------------------------------------------ load

    function bindRefresh(d) {
        ACCOUNTS = d.accounts || ACCOUNTS;
        bind('cmbcurrency', d.currencies, 'Id', 'name');
        bind('cmbFcycodeThirdParty', d.currencies, 'Id', 'name');
        bind('CmbChargesAccountType', d.chargesAccountTypes, 'Id', 'name');
        $id('dlReferenceNo').innerHTML = (d.referenceNos || []).map(function (r) { return '<option value="' + esc(col(r, 'name')) + '"></option>'; }).join('');
    }

    /** Acfrmfcbankreceipt_Load. */
    function load() {
        return getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = d.permissions;
            $id('btnsave').disabled = !PERM.Save;
            $id('btnPrint').disabled = !PERM.Print;
            $id('btnUpdate').disabled = !PERM.Update;
            setText('txtdate', today());
            $id('chkpreview').checked = true;
            partyPanelVisible();
            if (netI(d.documentNo) > 0) setText('txtdocNumber', d.documentNo);
            bindRefresh(d);
            /* Accounttype(): a fixed DataTable, Rows[0] (Bank) active */
            var at = [{ Id: 1, name: 'Bank' }, { Id: 2, name: 'Parties' }, { Id: 3, name: 'Cash' }, { Id: 4, name: 'Expense' }];
            bind('cmbaccounttype', at, 'Id', 'name'); setVal('cmbaccounttype', 1);
            bind('CmbGainLossAc', d.gainLossAccounts, 'Id', 'name');
            PAYMENT_TERMS = d.paymentTerms || [];
            bind('cmbPaymentTerm', PAYMENT_TERMS, 'Id', 'name');
            if (PAYMENT_TERMS.length) setVal('cmbPaymentTerm', PAYMENT_TERMS[0].Id);
            /* ChargesTypeFill: Local / Foreign, Rows[0] active */
            bind('CmbChargesType', [{ Id: 1, name: 'Local' }, { Id: 2, name: 'Foreign' }], 'Id', 'name'); setVal('CmbChargesType', 1);
            bind('CmbFBCDebitAccount', d.fbcAccounts, 'Id', 'name');
            historyComboFill(d);
            setText('FromDateHistory', daysAgo(7));
            setText('ToDateHistory', today());
            S.party = [{ RealizedAmount: 0, FBC: 0, TotalAmount: 0 }];
            renderAll();
            focus('txtdate');
            /* ?id=<ExImFCBankReceipts.Id> opens that receipt, as a history Edit does (RESETMAIN + ReadById) */
            var qid = 0; try { qid = netI(new URLSearchParams(global.location.search).get('id')); } catch (x) { qid = 0; }
            if (qid > 0) { resetMain(); return readById(qid); }
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }

    // ------------------------------------------------------------------------------ PaymentTerm()

    /** cmbPaymentTerm_Leave: the customer is cleared unless "Other Payment", then PaymentTerm(). */
    function paymentTermLeave() {
        if (termText() !== 'Other Payment') clearCombo('comSuplier');
        return paymentTerm();
    }

    /** PaymentTerm(). */
    async function paymentTerm() {
        var t = termText(), tid = termId();
        consigneeVisible(false); show('grdPaymentTermsBox', false);
        try {
            if (t === 'Advanced Payment') {
                setEnabled('cmbLCOrder', true); setEnabled('cmbInvoiceNo', false); setEnabled('cmbcurrency', true);
                setVal('cmbInvoiceNo', '0');
                setText('txtInvoiceAmount', '0'); setText('txtBalanceAmount', '0'); setText('txtRecAmount', '0'); setText('txtExportvoucherRate', '0');
                consigneeVisible(true);
                show('RemainingBalanceOfBankDeviceBreakUp', false); show('btnRemaingBreakupBalance', false);
                var c = await getJson(API + '/customers?term=' + encodeURIComponent(t) + '&termId=' + tid);
                if ((c.customers || []).length) { bind('comSuplier', c.customers, 'Id', 'name'); bind('CmbConsignee', c.consignees, 'Id', 'name'); }
                await lcOrder();
            }
            if (t === 'Invoice Payment') {
                setEnabled('cmbInvoiceNo', true); setEnabled('cmbLCOrder', false); clearCombo('cmbLCOrder'); setEnabled('cmbcurrency', false);
                show('grdPaymentTermsBox', true);
                show('RemainingBalanceOfBankDeviceBreakUp', false); show('btnRemaingBreakupBalance', false);
                consigneeVisible(true);
                var c2 = await getJson(API + '/customers?term=' + encodeURIComponent(t) + '&termId=' + tid);
                if ((c2.customers || []).length) bind('comSuplier', c2.customers, 'Id', 'name'); else clearCombo('comSuplier');
                if ((c2.consignees || []).length) bind('CmbConsignee', c2.consignees, 'Id', 'name');
                await invoiceNo();
            }
            if (t === 'Other Payment') {
                setEnabled('cmbLCOrder', false); setEnabled('cmbInvoiceNo', false); setEnabled('cmbcurrency', true);
                setText('txtInvoiceAmount', '0'); setText('txtBalanceAmount', '0'); setText('txtRecAmount', '0'); setText('txtExportvoucherRate', '0');
                clearCombo('cmbLCOrder'); clearCombo('cmbInvoiceNo');
                await allExportCustomers(t, tid);
            }
            if (tid === 4) {
                setEnabled('cmbLCOrder', false); setEnabled('cmbInvoiceNo', false); setEnabled('cmbcurrency', true);
                setVal('cmbInvoiceNo', '0');
                setText('txtInvoiceAmount', '0'); setText('txtBalanceAmount', '0'); setText('txtRecAmount', '0'); setText('txtExportvoucherRate', '0');
                await allExportCustomers(t, tid);
                if (tid === 4 && acctType() === 1) {
                    await invoiceNoForBreakUp();
                    show('panelInvoiceWiseBreakup', true);
                    show('btnRemaingBreakupBalance', true);
                } else {
                    show('panelInvoiceWiseBreakup', false); clearBreakupGrid(); show('btnRemaingBreakupBalance', false);
                }
            } else {
                show('panelInvoiceWiseBreakup', false); clearBreakupGrid();
            }
        } catch (e) { box(e.message); }
    }
    async function allExportCustomers(t, tid) {
        var c = await getJson(API + '/customers?term=' + encodeURIComponent(t) + '&termId=' + tid);
        if ((c.customers || []).length) bind('comSuplier', c.customers, 'Id', 'name');
    }
    /** LcOrder(). */
    async function lcOrder() {
        var rows = await getJson(API + '/lc-orders?customerId=' + netI(val('comSuplier')));
        if ((rows || []).length) bind('cmbLCOrder', rows, 'Id', 'name');
    }
    /** InvoiceNo(). */
    async function invoiceNo() {
        if (termText() !== 'Invoice Payment') { clearCombo('cmbInvoiceNo'); return; }
        var rows = await getJson(API + '/invoices?customerId=' + netI(val('comSuplier')));
        if ((rows || []).length) bind('cmbInvoiceNo', rows, 'Id', 'name'); else clearCombo('cmbInvoiceNo');
    }
    /** InvoiceNoForBreakUp(). */
    async function invoiceNoForBreakUp() {
        if (!breakupMode()) { clearCombo('cmbInvoicenobreakup'); S.brkInvoices = []; return; }
        S.brkInvoices = await getJson(API + '/breakup-invoices') || [];
        if (S.brkInvoices.length) bind('cmbInvoicenobreakup', S.brkInvoices, 'Id', 'name'); else clearCombo('cmbInvoicenobreakup');
    }
    /** grdInvoiceWiseBreakUp.ClearStructure() - the grid shows nothing (and Save reads no rows) until it is bound again. */
    function clearBreakupGrid() { S.brkGridCleared = true; renderBreakups(); }

    /** cmbaccounttype_Leave. */
    async function accountTypeLeave() {
        try {
            if (breakupMode()) { await invoiceNoForBreakUp(); show('panelInvoiceWiseBreakup', true); }
            else { show('panelInvoiceWiseBreakup', false); clearBreakupGrid(); }
            var a = acctType();
            if (a === 1) bankAccountDr(15);
            if (a === 2) bankAccountDr(22);
            if (a === 3) bankAccountDr(2);
            if (a === 4) bankAccountDr(11);
        } catch (e) { box(e.message); }
    }
    /** BankAccountDr(accounttypeId) - filtered in memory from dtaccounts as the desktop does. */
    function bankAccountDr(t) {
        var rows = ACCOUNTS.filter(function (r) {
            var a = netI(r.AccountTypeId);
            if (t === 22) return a === 3 || a === 6 || a === 8 || a === 22;
            if (t === 11) return a === 11 || a === 13 || a === 14 || a === 20 || a === 21;
            return a === t;
        });
        if (rows.length) bind('cmbbankAcc', rows, 'Id', 'name'); else clearCombo('cmbbankAcc');
    }

    // ------------------------------------------------------------------------------ amounts

    /** InvoiceAmountGet() + GetExchangeRateandCurrencyFromVoucher(). */
    async function invoiceAmountGet() {
        setText('txtInvoiceAmount', '0'); setText('txtRecAmount', '0'); setText('txtBalanceAmount', '0');
        var bm = breakupMode();
        var inv = termId() === 4 ? netI(val('cmbInvoicenobreakup')) : netI(val('cmbInvoiceNo'));
        var d = await getJson(API + '/invoice-info?breakup=' + bm + '&invoiceId=' + (bm ? netI(val('cmbInvoicenobreakup')) : netI(val('cmbInvoiceNo')))
            + '&previousBalance=' + S.previousBalance + '&rateInvoiceId=' + inv);
        if (!bm && netI(val('cmbInvoiceNo')) <= 0) { d.InvoiceAmount = 0; d.ReceivedAmount = 0; d.BalanceAmount = 0; }
        setText('txtInvoiceAmount', fmtH(d.InvoiceAmount, 3));
        setText('txtRecAmount', fmtH(d.ReceivedAmount, 3));
        setText('txtBalanceAmount', fmtH(d.BalanceAmount, 3));
        /* GetExchangeRateandCurrencyFromVoucher uses the breakup invoice when term 4, else cmbInvoiceNo */
        if (inv !== (bm ? netI(val('cmbInvoicenobreakup')) : netI(val('cmbInvoiceNo')))) {
            d = await getJson(API + '/invoice-info?breakup=false&invoiceId=' + inv + '&previousBalance=0');
        }
        if (d.VoucherRateFound) { setText('txtExportvoucherRate', fmtH(d.VoucherRate, 2)); setVal('cmbcurrency', d.MultiCurrencyId); }
        else setText('txtExportvoucherRate', '0');
    }
    /** cmbInvoiceNo ValueChanged + Leave. */
    async function invoiceNoChanged() {
        try {
            await invoiceAmountGet();
            if (termText() === 'Invoice Payment') await paymentTermDetail(netI(val('cmbInvoiceNo')));
            else { S.terms = []; renderTerms(); }
        } catch (e) { box(e.message); }
    }
    /** getPaymentTermDetailByInvoiceId(InvoiceId) with Id. */
    async function paymentTermDetail(invoiceId) {
        S.terms = await getJson(API + '/payment-terms?invoiceId=' + invoiceId + '&recId=' + S.id) || [];
        renderTerms();
    }
    /** SalesContractAmountGet(). */
    async function contractAmount() {
        try {
            var d = await getJson(API + '/contract-amount?contractId=' + netI(val('cmbLCOrder')));
            if (d.found) { setText('txtInvoiceAmount', fmt00(d.InvoiceAmount)); setText('txtRecAmount', fmt00(d.ReceivedAmount)); setText('txtBalanceAmount', fmt00(d.BalanceAmount)); }
            else { setText('txtInvoiceAmount', '0'); setText('txtRecAmount', '0'); setText('txtBalanceAmount', '0'); S.previousBalance = 0; }
        } catch (e) { box(e.message); }
    }

    /** txtFCGrosamount_TextChanged / txtexchangeRate_TextChanged. */
    function grossOrRateChanged(fromRate) {
        var g = val('txtFCGrosamount'), r = val('txtexchangeRate');
        if (g !== '' && r !== '') {
            var c = netD(g) * netD(r);
            setText('txtNetAmount', fmt(c, 4)); setText('txtbankDrAmount', fmt(c, 4));
        } else if (fromRate) { setText('txtNetAmount', '0'); setText('txtbankDrAmount', '0'); }
        netAmountFC();
    }
    /** NetAmountFC(). */
    function netAmountFC() {
        var rateT = val('txtexchangeRate'), grossT = val('txtFCGrosamount');
        if (rateT === '' || grossT === '') return;
        var comm = 0;
        if (!S.brkGridCleared) S.breakups.forEach(function (r) { comm += netD(r.ForiegnAgencyCommAmount); });
        var a = netD(rateT), b = netD(grossT), c = a * b;
        setText('txtbankDrAmount', fmt(c, 4));
        setText('txtNetAmountFC', fmt(b, 4));
        setText('txtpkramount', fmt(netD(val('txtfcamount')) * a, 4));
        if (!S.charges.length) return;
        var fc = 0, rs = 0;
        S.charges.forEach(function (r) {
            if (r.Type === 'Foreign') fc += netD(r.FCAmount);
            if (netD(r.RsAmount) > 0) {
                var p = netD(r.Prcnt);
                if (p > 0) r.RsAmount = (b + comm) * a * p / 100;
                rs += netD(r.RsAmount);
            }
        });
        setText('txtNetAmountFC', fmt(b - fc, 4));
        setText('txtbankDrAmount', fmtInt(c - rs));
        renderCharges();
    }
    /** BankDebitAmountCalculate(). */
    function bankDebitAmountCalculate() {
        if (val('txtFCGrosamount') === '') return;
        var t = 0; S.charges.forEach(function (r) { t += netD(r.RsAmount); });
        setText('txtbankDrAmount', fmt(netD(val('txtNetAmount')) - t, 4));
    }
    /** ThirdExchRateByFcGross() - txtFCGrosamount_Leave. */
    function thirdExchRateByFcGross() {
        if (val('txtFCGrosamount') !== '' && val('txtfcyAmountThirdParty') !== '') {
            setText('txtFcyExchRthirdParty', dstr(Math.round(netD(val('txtfcyAmountThirdParty')) / netD(val('txtFCGrosamount')) * 1000) / 1000));
        } else setText('txtFcyExchRthirdParty', '');
    }
    /** FcGrossAmtByThirdParty() - Fcy Amount / Cust Ex.Rate typed: FCY Amount = third amount / rate (3 dp), which fires its own TextChanged. */
    function fcGrossAmtByThirdParty() {
        var r = netD(val('txtFcyExchRthirdParty')), t = netD(val('txtfcyAmountThirdParty'));
        if (r !== 0 && t !== 0) { setText('txtFCGrosamount', dstr(Math.round(t / r * 1000) / 1000)); grossOrRateChanged(false); }
    }
    /** ThirdPartyReceiverAmountCalculations(). */
    function receiverAmount() {
        setText('txtReceiverFcyAmount', '0');
        var t = netD(val('txtfcyAmountThirdParty')), r = netD(val('txtReceiverExchangeRate'));
        if (t !== 0 && r !== 0) setText('txtReceiverFcyAmount', dstr(Math.round(t / r * 1000) / 1000));
    }

    // ------------------------------------------------------------------------------ charges (groupBox2 / grdFCBank)

    /** CalculatePercntAmount() - Tag "Percent" (txtChargePercent) computes Rs Amount, Tag "PkrAmount" (txtpkramount) clears Prcnt. */
    function calculatePercntAmount(activeTag) {
        var ct = netI(val('CmbChargesType'));
        if (!ct) { setText('txtfcamount', ''); setText('txtpkramount', ''); return; }
        var local = selText('CmbChargesType') === 'Local';
        var pct = netD(val('txtChargePercent'));
        if (termId() === 4 && netD(val('txtexchangeRate')) > 0 && netI(val('CmbChargesFDBCNo')) !== 0) {
            var net = 0, fd = str(selText('CmbChargesFDBCNo')).toUpperCase();
            S.breakups.forEach(function (r) { if (str(r.FDBCNo).toUpperCase() === fd) net += (netD(r.RealizedAmount) + netD(r.ForiegnAgencyCommAmount)) * netD(val('txtexchangeRate')); });
            if (!local) return;
            if (activeTag === 'Percent') { if (pct > 0 && net > 0) setText('txtpkramount', fmt(net * pct / 100, 4)); }
            else if (activeTag === 'PkrAmount') setText('txtChargePercent', '');
            return;
        }
        var net2 = netD(val('txtNetAmount'));
        if (!local || !(net2 > 0)) return;
        if (activeTag === 'Percent') { if (pct > 0 && net2 > 0) setText('txtpkramount', fmt(net2 * pct / 100, 4)); }
        else if (activeTag === 'PkrAmount') setText('txtChargePercent', '');
    }
    function chargesTotals() {
        var f = 0, l = 0;
        S.charges.forEach(function (r) { if (r.Type === 'Foreign') f += netD(r.RsAmount); else l += netD(r.RsAmount); });
        return { foreign: f, local: l };
    }
    function chargeValidation() {
        if (breakupMode() && (selText('CmbChargesFDBCNo').trim() === '' || netI(val('CmbChargesFDBCNo')) === 0 && val('CmbChargesFDBCNo') === '0')) { box('Please Select FDBC #'); focus('CmbChargesFDBCNo'); return false; }
        if (selText('CmbChargesAccountType').trim() === '' || netI(val('CmbChargesAccountType')) === 0) { box('Please Select AccountTitle'); focus('CmbChargesAccountType'); return false; }
        if (val('txtpkramount').trim() === '') { box('Please Enter PKR Amount'); focus('txtpkramount'); return false; }
        if (selText('CmbChargesType').trim() === '' || netI(val('CmbChargesType')) === 0) { box('Please select Chargestype'); focus('CmbChargesType'); return false; }
        return true;
    }
    function chargeLimits(skipIndex, gratter) {
        var fc = netD(val('txtfcamount')), rs = netD(val('txtpkramount'));
        S.charges.forEach(function (r) { fc += netD(r.FCAmount); rs += netD(r.RsAmount); });
        if (fc > netD(val('txtNetAmountFC'))) throw new Error(gratter ? 'Fcy Bank Charges Amount not gratter than FcyNetAmount' : 'Fcy Bank Charges Amount not greater than FcyNetAmount');
        if (rs > netD(val('txtNetAmount'))) throw new Error(gratter ? 'Bank Charges Amount not gratter than Amount(PKR) Debit Amount' : 'Bank Charges Amount not greater than Amount(PKR) Debit Amount');
        if (selText('CmbChargesType') === 'Foreign' && (val('txtfcamount').trim() === '' || val('txtfcamount').trim() === '0')) throw new Error('FcAmount field required');
    }
    function panelCharge() {
        return {
            FDBCNo: selText('CmbChargesFDBCNo'), TypeId: netI(val('CmbChargesType')), Type: selText('CmbChargesType').trim(),
            ChargesTypeId: netI(val('CmbChargesAccountType')), ChargesType: selText('CmbChargesAccountType'),
            Prcnt: netD(val('txtChargePercent')), Remarks: val('txtRemarksDetail').trim(),
            FCAmount: netD(val('txtfcamount')), RsAmount: netD(val('txtpkramount'))
        };
    }
    /** btnAddinGrid_Click. */
    function btnAddinGrid() {
        try {
            if (!chargeValidation()) return;
            chargeLimits(-1, false);
            S.charges.push(panelCharge());
            renderCharges();
            var t = chargesTotals();
            setText('txtForeinchargesPkr', fmt(t.foreign, 4)); setText('txtLocalAmount', fmt(t.local, 4));
            setVal('CmbChargesAccountType', '0');
            ['txtChargePercent', 'txtfcamount', 'txtpkramount', 'txtRemarksDetail'].forEach(function (id) { setText(id, ''); });
            bankDebitAmountCalculate();
            focus(breakupMode() ? 'CmbChargesFDBCNo' : 'CmbChargesType');
            calculateRemaining();
        } catch (e) { box(e.message); }
    }
    /** btnUpdateDetail_Click. */
    function btnUpdateDetail() {
        try {
            if (breakupMode() && selText('CmbChargesFDBCNo').trim() === '') { box('Please Select FDBC #'); focus('CmbChargesFDBCNo'); return; }
            if (selText('CmbChargesAccountType').trim() === '') { box('Please Select AccountTitle'); focus('CmbChargesAccountType'); return; }
            if (val('txtpkramount').trim() === '') { box('Please Enter PKR Amount'); focus('txtpkramount'); return; }
            if (selText('CmbChargesType').trim() === '' || netI(val('CmbChargesType')) === 0) { box('Please select Chargestype'); focus('CmbChargesType'); return; }
            chargeLimits(S.updIndex, true);   /* the desktop counts the edited row twice (its old values stay in the grid sum) */
            var r = S.charges[S.updIndex]; if (!r) return;
            var n = panelCharge();
            for (var k in n) if (Object.prototype.hasOwnProperty.call(n, k)) r[k] = n[k];
            renderCharges();
            resetDetail();
            bankDebitAmountCalculate();
            show('btnAddinGrid', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
            var t = chargesTotals();
            setText('txtForeinchargesPkr', dstr(t.foreign)); setText('txtLocalAmount', dstr(t.local));
            S.updIndex = -1;
            focus(breakupMode() ? 'CmbChargesFDBCNo' : 'CmbChargesType');
            calculateRemaining();
        } catch (e) { box(e.message); }
    }
    function btnCancelUpdateDetial() {
        resetDetail();
        show('btnAddinGrid', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        S.updIndex = -1;
    }
    /** resetdetail. */
    function resetDetail() {
        ['txtChargePercent', 'txtRemarksDetail', 'txtfcamount', 'txtpkramount'].forEach(function (id) { setText(id, ''); });
        setVal('CmbChargesAccountType', '0');
    }
    /** grdFCBank_DoubleClick. */
    function chargeEdit(i) {
        var r = S.charges[i]; if (!r) return;
        S.updIndex = i;
        selectByText('CmbChargesFDBCNo', r.FDBCNo);
        setVal('CmbChargesType', r.TypeId);
        setVal('CmbChargesAccountType', r.ChargesTypeId);
        setText('txtChargePercent', fmt(r.Prcnt, 4));
        setText('txtRemarksDetail', r.Remarks);
        setText('txtfcamount', fmt(r.FCAmount, 4));
        setText('txtpkramount', fmt(r.RsAmount, 4));
        focus('CmbChargesAccountType');
        show('btnAddinGrid', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
        renderCharges();
    }
    function selectByText(id, t) { var s = $id(id); if (!s) return; for (var i = 0; i < s.options.length; i++) if (s.options[i].text === str(t)) { s.selectedIndex = i; refreshCombos(); return; } s.value = '0'; refreshCombos(); }
    /** grdFCBank_ColumnButtonClick (X). */
    function chargeDelete(i) {
        S.charges.splice(i, 1);
        renderCharges();
        bankDebitAmountCalculate();
        var t = chargesTotals();
        setText('txtForeinchargesPkr', fmt(t.foreign, 4)); setText('txtLocalAmount', fmt(t.local, 4));
        calculateRemaining();
    }
    /** CmbChargesAccountType_Leave: the automatic remark for Invoice Payment. */
    function chargesAccountLeave() {
        if (termText().trim() === 'Invoice Payment' && val('txtRemarksDetail').trim() === '') {
            var rmk = '', fc = parseFloat(val('txtfcamount'));
            if (isFinite(fc) && fc > 0) rmk += selText('cmbcurrency') + ' ' + dstr(fc) + ' @ ' + val('txtexchangeRate') + ' ';
            rmk = rmk + selText('CmbChargesAccountType').trim() + ' AGAINST INVOICE NO ' + selText('cmbInvoiceNo').trim();
            setText('txtRemarksDetail', rmk);
            try { $id('txtRemarksDetail').select(); } catch (e) { /* ignore */ }
        }
    }
    /** CalculateBankInvoiceBreakupRemaining(). */
    function calculateRemaining() {
        if (visible('panelInvoiceWiseBreakup') && !S.brkGridCleared && S.breakups.length) {
            show('RemainingBalanceOfBankDeviceBreakUp', true);
            var distinct = [];
            S.breakups.forEach(function (r) { var f = str(r.FDBCNo); if (distinct.indexOf(f) < 0) distinct.push(f); });
            if (!distinct.length) return;
            var rate = netD(val('txtexchangeRate'));
            var rows = distinct.map(function (f) {
                var dr = null;
                for (var i = 0; i < S.breakups.length; i++) if (f.indexOf(str(S.breakups[i].FDBCNo)) >= 0) { dr = S.breakups[i]; break; }   /* distinct[i].Contains(row FDBCNo) */
                var realized = dr ? netD(dr.RealizedAmount) : 0;
                var lcy = Math.round(realized * rate * 10000) / 10000;
                var exp = 0; S.charges.forEach(function (c) { if (str(c.FDBCNo) === f) exp += netD(c.RsAmount); });
                return { FDBCNo: f, Realized: realized, ExRate: rate, LcyAmount: lcy, Expense: exp, NetLcyAmount: lcy - exp };
            });
            var cols = ['FDBCNo', 'Realized', 'ExRate', 'LcyAmount', 'Expense', 'NetLcyAmount'];
            $id('remBody').innerHTML = rows.map(function (r) { return '<tr>' + cols.map(function (c) { return c === 'FDBCNo' ? '<td>' + esc(r[c]) + '</td>' : '<td class="num">' + esc(fmt(r[c], 4)) + '</td>'; }).join('') + '</tr>'; }).join('');
            $id('remFoot').innerHTML = sumRow(cols, rows, ['Realized', 'Expense', 'NetLcyAmount'], 0);
        } else {
            show('RemainingBalanceOfBankDeviceBreakUp', false);
            $id('remBody').innerHTML = ''; $id('remFoot').innerHTML = '';
        }
    }

    // ------------------------------------------------------------------------------ invoice-wise breakup

    function brkInvoiceRow() { var id = netI(val('cmbInvoicenobreakup')); for (var i = 0; i < S.brkInvoices.length; i++) if (netI(S.brkInvoices[i].Id) === id) return S.brkInvoices[i]; return null; }
    /** cmbInvoicenobreakup_Leave (also CmbChargesFDBCNo.Leave - the designer wires both to it). */
    async function breakupInvoiceLeave() {
        try {
            await invoiceAmountGet();
            var r = brkInvoiceRow();
            if (r && netI(val('cmbInvoicenobreakup')) > 0) {
                S.gds = await getJson(API + '/gds?invoiceId=' + netI(r.Id) + '&gdBreakUpId=' + S.gdBreakUpId + '&refDocumentTypeId=' + netI(r.DocumentTypeId)) || [];
                if (S.gds.length) bind('cmbGdNoBreakup', S.gds, 'Id', 'name'); else clearCombo('cmbGdNoBreakup');
                statusBind(str(r.InvoiceStatus));
            } else { S.gds = []; clearCombo('cmbGdNoBreakup'); }
        } catch (e) { box(e.message); }
    }
    /** StatusInvoiceWisebreakupBind(LastSavedStatus). */
    function statusBind(last) {
        var rows = [{ Id: 1, name: 'Final Part' }];
        if (last !== 'Final Part') {
            if (!last) rows.push({ Id: 2, name: '1 Part' });
            else {
                var n = parseInt(last.split(/\s+/)[0], 10);
                if (!isFinite(n)) { box('Input string was not in a correct format.'); return; }
                var saveMode = !$id('btnsave').classList.contains('is-hidden') && !$id('btnsave').disabled;
                rows.push({ Id: 2, name: (saveMode ? n + 1 : n) + ' Part' });
            }
        }
        bind('cmbStatusBreakup', rows, 'Id', 'name');
    }
    /** RealizedAmountAutoCalculate(). */
    function realizedAuto() {
        var t = netD(val('txtInvoiceAmountBreakup').trim()), c = netD(val('txtCommisionamtbreakUp').trim());
        if (t > 0 && c > 0) setText('txtRealizedAmtBreakup', dstr(t - c));
        else if (t > 0 && c === 0) setText('txtRealizedAmtBreakup', dstr(t));
    }
    /** FormValidationbreakUp(). */
    function breakupValidation() {
        if (selText('cmbInvoicenobreakup').trim() === '' || netI(val('cmbInvoicenobreakup')) === 0) { box('Please Select InvoiceNo'); focus('cmbInvoicenobreakup'); return false; }
        if (netI(val('cmbGdNoBreakup')) === 0) { box('Please Select GD No'); focus('cmbGdNoBreakup'); return false; }
        if (netD(val('txtInvoiceAmountBreakup')) === 0) { box('Invoice Amount required'); focus('txtInvoiceAmountBreakup'); return false; }
        if (netD(val('txtRealizedAmtBreakup')) === 0) { box('Realized Amount required'); focus('txtRealizedAmtBreakup'); return false; }
        var t = Math.round(netD(val('txtInvoiceAmountBreakup')) * 1e6), sum = Math.round((netD(val('txtRealizedAmtBreakup')) + netD(val('txtCommisionamtbreakUp'))) * 1e6);
        if (t !== sum) { box('RealizedAmount and CommissionAmount cannot be equal to TotalAmount.'); focus('txtInvoiceAmountBreakup'); return false; }
        if (val('txtFDBCNoBreakup').trim() === '') { box('FDBC No required'); focus('txtFDBCNoBreakup'); return false; }
        if (selText('cmbStatusBreakup').trim() === '' || netI(val('cmbStatusBreakup')) === 0) { box('Status required'); focus('cmbStatusBreakup'); return false; }
        return true;
    }
    function gdRow() { var id = netI(val('cmbGdNoBreakup')); for (var i = 0; i < S.gds.length; i++) if (netI(S.gds[i].Id) === id) return S.gds[i]; return null; }
    /** btnAddBreakup_Click. */
    function btnAddBreakup() {
        S.gdBreakUpId = 0;
        if (!breakupValidation()) return;
        var g = gdRow();
        S.breakups.push({
            Id: 0, InvoiceId: netI(val('cmbInvoicenobreakup')), InvoiceNo: selText('cmbInvoicenobreakup').trim(),
            GDId: netI(val('cmbGdNoBreakup')), GDNO: selText('cmbGdNoBreakup').trim(), GdRefDocTypeId: g ? netI(g.DocumentTypeId) : 0,
            InvoiceAmount: netD(val('txtInvoiceAmountBreakup')), RealizedAmount: netD(val('txtRealizedAmtBreakup').trim()),
            ForiegnAgencyCommAmount: netD(val('txtCommisionamtbreakUp').trim()), FDBCNo: val('txtFDBCNoBreakup').trim(), Status: selText('cmbStatusBreakup').trim()
        });
        S.brkGridCleared = false;
        renderBreakups();
        resetBreakUp();
        addTotalRealizedInHeader();
        focus('cmbInvoicenobreakup');
        gdBindInExpenses();
    }
    /** BtnUpdatebreakup_Click. */
    function btnUpdatebreakup() {
        if (!breakupValidation()) return;
        var r = S.breakups[S.updBrk]; if (!r) return;
        var g = gdRow();
        r.InvoiceId = netI(val('cmbInvoicenobreakup')); r.InvoiceNo = selText('cmbInvoicenobreakup');
        r.GDId = netI(val('cmbGdNoBreakup')); r.GDNO = selText('cmbGdNoBreakup'); r.GdRefDocTypeId = g ? netI(g.DocumentTypeId) : 0;
        r.InvoiceAmount = netD(val('txtInvoiceAmountBreakup')); r.RealizedAmount = netD(val('txtRealizedAmtBreakup'));
        r.ForiegnAgencyCommAmount = netD(val('txtCommisionamtbreakUp')); r.FDBCNo = val('txtFDBCNoBreakup'); r.Status = selText('cmbStatusBreakup');
        S.brkGridCleared = false;
        renderBreakups();
        resetBreakUp();
        addTotalRealizedInHeader();
        accountTypeLeave();
        focus('cmbInvoicenobreakup');
        gdBindInExpenses();
    }
    /** ResetBreakUp. */
    function resetBreakUp() {
        setVal('cmbInvoicenobreakup', '0'); setVal('cmbGdNoBreakup', '0');
        ['txtInvoiceAmountBreakup', 'txtRealizedAmtBreakup', 'txtCommisionamtbreakUp', 'txtFDBCNoBreakup'].forEach(function (id) { setText(id, ''); });
        setVal('cmbStatusBreakup', '0');
        show('btnAddBreakup', true); show('BtnUpdatebreakup', false); show('BtnCancelBreakUp', false);
        S.gdBreakUpId = 0; S.updBrk = -1;
    }
    /** AddTotalRealizedAmountInHeaderFcAmount() - FCY Amount = Σ Realized (fires its TextChanged). */
    function addTotalRealizedInHeader() {
        var t = 0; if (!S.brkGridCleared) S.breakups.forEach(function (r) { t += netD(r.RealizedAmount); });
        setText('txtFCGrosamount', S.breakups.length && !S.brkGridCleared ? fmt(t, 3) : '0');
        grossOrRateChanged(false);
    }
    /** GdBindinExpensesInvoice() - the FDBS # combo of the charges entry. */
    function gdBindInExpenses() {
        clearCombo('CmbChargesFDBCNo');
        if (S.breakups.length) bind('CmbChargesFDBCNo', S.breakups.map(function (r, i) { return { Id: i + 1, name: str(r.FDBCNo) }; }), 'Id', 'name');
    }
    /** grdInvoiceWiseBreakUp_DoubleClick. */
    async function breakupEdit(i) {
        var r = S.breakups[i]; if (!r) return;
        S.updBrk = i;
        S.brkInvoices = [{ Id: r.InvoiceId, name: r.InvoiceNo, EFormNo: r.GDNO, InvoiceStatus: r.Status, DocumentTypeId: 0 }];
        bind('cmbInvoicenobreakup', S.brkInvoices, 'Id', 'name');
        setVal('cmbInvoicenobreakup', r.InvoiceId);
        setText('txtRealizedAmtBreakup', dstr(r.RealizedAmount));
        setText('txtCommisionamtbreakUp', dstr(r.ForiegnAgencyCommAmount));
        setText('txtInvoiceAmountBreakup', dstr(netD(r.RealizedAmount) + netD(r.ForiegnAgencyCommAmount)));
        setText('txtFDBCNoBreakup', r.FDBCNo);
        S.gdBreakUpId = netI(r.Id);
        show('btnAddBreakup', false); show('BtnUpdatebreakup', true); show('BtnCancelBreakUp', true);
        await breakupInvoiceLeave();
        selectByText('cmbStatusBreakup', r.Status);
        setVal('cmbGdNoBreakup', r.GDId);
        focus('cmbInvoicenobreakup');
    }
    /** grdInvoiceWiseBreakUp_ColumnButtonClick (X): a saved row cannot be deleted. */
    function breakupDelete(i) {
        var r = S.breakups[i]; if (!r) return;
        if (netI(r.Id) > 0) { box('Saved Record Can not be Deleted...'); return; }
        S.breakups.splice(i, 1);
        renderBreakups();
        addTotalRealizedInHeader();
        calculateRemaining();
    }

    // ------------------------------------------------------------------------------ party payments

    function partyPanelVisible() { var t = radio('fcyTrans'); show('panel10', t === '1' || t === '3'); }
    function fbcCalculations() { var f = 0; S.party.forEach(function (r) { f += netD(r.FBC); }); setText('txtFBC', dstr(f)); }

    // ------------------------------------------------------------------------------ render

    function renderCharges() {
        /* grouped by FDBCNo with group totals (GroupTotals = Always) */
        var groups = {}, order = [];
        S.charges.forEach(function (r, i) { var k = str(r.FDBCNo); if (!groups[k]) { groups[k] = []; order.push(k); } groups[k].push(i); });
        var h = '';
        order.forEach(function (k) {
            var fcs = 0, rss = 0;
            h += '<tr class="fcy-group"><td colspan="7">FDBCNo: ' + esc(k) + '</td></tr>';
            groups[k].forEach(function (i) {
                var r = S.charges[i];
                fcs += netD(r.FCAmount); rss += netD(r.RsAmount);
                h += '<tr data-i="' + i + '"' + (i === S.updIndex ? ' class="is-current"' : '') + '><td class="win-cell-btn"><button type="button" class="win-x" data-cdel="' + i + '">X</button></td>'
                    + '<td><a class="win-code" data-cedit="' + i + '" href="javascript:void(0)">' + esc(r.Type) + '</a></td><td>' + esc(r.ChargesType) + '</td>'
                    + '<td class="num">' + esc(fmt(r.Prcnt, 4)) + '</td><td>' + esc(r.Remarks) + '</td><td class="num">' + esc(fmt(r.FCAmount, 3)) + '</td><td class="num">' + esc(fmt(r.RsAmount, 2)) + '</td></tr>';
            });
            h += '<tr class="fcy-group-total"><td colspan="5" class="lbl">&Sigma; ' + esc(k) + '</td><td class="num">' + esc(fmt(fcs, 3)) + '</td><td class="num">' + esc(fmt(rss, 2)) + '</td></tr>';
        });
        $id('chgBody').innerHTML = h;
        var f = 0, rs = 0; S.charges.forEach(function (r) { f += netD(r.FCAmount); rs += netD(r.RsAmount); });
        $id('chgFoot').innerHTML = S.charges.length ? '<tr><td class="lbl" colspan="5">&Sigma;</td><td>' + esc(fmt(f, 3)) + '</td><td>' + esc(fmt(rs, 2)) + '</td></tr>' : '';
    }
    function renderBreakups() {
        var rows = S.brkGridCleared ? [] : S.breakups;
        var cols = ['InvoiceNo', 'GDNO', 'InvoiceAmount', 'RealizedAmount', 'ForiegnAgencyCommAmount', 'FDBCNo', 'Status'];
        var nums = ['InvoiceAmount', 'RealizedAmount', 'ForiegnAgencyCommAmount'];
        $id('brkBody').innerHTML = rows.map(function (r, i) {
            return '<tr data-i="' + i + '"' + (i === S.updBrk ? ' class="is-current"' : '') + '><td class="win-cell-btn"><button type="button" class="win-x" data-bdel="' + i + '">X</button></td>'
                + cols.map(function (c) {
                    if (c === 'InvoiceNo') return '<td><a class="win-code" data-bedit="' + i + '" href="javascript:void(0)">' + esc(r[c]) + '</a></td>';
                    return nums.indexOf(c) >= 0 ? '<td class="num">' + esc(fmt(r[c], 3)) + '</td>' : '<td>' + esc(r[c]) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        $id('brkFoot').innerHTML = sumRow(cols, rows, nums, 1, function (v) { return fmt(v, 3); });
    }
    function renderParty() {
        $id('partyBody').innerHTML = S.party.map(function (r, i) {
            return '<tr data-i="' + i + '"><td class="win-cell-btn"><button type="button" class="win-x" data-pdel="' + i + '">X</button></td>'
                + '<td class="win-cell-btn"><button type="button" class="win-x" style="color:#004d40" data-padd="' + i + '">+</button></td>'
                + '<td class="win-editable"><input type="text" class="num" data-guard="decimal" data-pi="' + i + '" data-pc="RealizedAmount" value="' + esc(dstr(r.RealizedAmount)) + '"/></td>'
                + '<td class="win-editable"><input type="text" class="num" data-guard="decimal" data-pi="' + i + '" data-pc="FBC" value="' + esc(dstr(r.FBC)) + '"/></td>'
                + '<td class="num">' + esc(fmt(r.TotalAmount, 3)) + '</td></tr>';
        }).join('');
        $id('partyFoot').innerHTML = sumRow(['RealizedAmount', 'FBC', 'TotalAmount'], S.party, ['RealizedAmount', 'FBC', 'TotalAmount'], 2, function (v) { return fmt(v, 3); });
    }
    function renderTerms() {
        show('grdPaymentTermsBox', termText() === 'Invoice Payment' || visible('grdPaymentTermsBox'));
        $id('termsBody').innerHTML = S.terms.map(function (r, i) {
            return '<tr><td>' + esc(r.PaymentTerm) + '</td><td class="num">' + esc(fmtH(r.Amount, 3)) + '</td><td class="num">' + esc(fmtH(r.Received, 3)) + '</td>'
                + '<td class="num">' + esc(fmtH(r.Balance, 3)) + '</td><td class="win-editable"><input type="text" class="num" data-guard="decimal" data-ti="' + i + '" value="' + esc(dstr(r.ThisReceipt)) + '"/></td></tr>';
        }).join('');
        $id('termsFoot').innerHTML = sumRow(['PaymentTerm', 'Amount', 'Received', 'Balance', 'ThisReceipt'], S.terms, ['Amount', 'Received', 'Balance', 'ThisReceipt'], 0, function (v) { return fmtH(v, 3); });
    }
    function renderAll() { renderCharges(); renderBreakups(); renderParty(); renderTerms(); }

    // ------------------------------------------------------------------------------ RESETMAIN / Insert / ReadById

    /** RESETMAIN (+ resetdetail, ResetBreakUp as btnnew_Click adds). */
    function resetMain() {
        S.id = 0; S.previousBalance = 0; S.voucherHeadId = 0; S.branchReset = true;   /* BrancheId / ProjectId = 0 from here on (desktop quirk) */
        S.charges = []; S.updIndex = -1;
        /* GenerateDocumentNo - ignored if a ReadById started meanwhile (history Edit / ?id= run RESETMAIN then ReadById) */
        var docSeq = ++S.docSeq;
        getJson(API + '/document-no').then(function (d) { if (docSeq === S.docSeq && S.id === 0 && netI(d.documentNo) > 0) setText('txtdocNumber', d.documentNo); }).catch(function () { /* ignore */ });
        ['comSuplier', 'cmbLCOrder', 'cmbInvoiceNo', 'cmbbankAcc', 'cmbcurrency', 'CmbConsignee', 'CmbFBCDebitAccount', 'cmbFcycodeThirdParty', 'cmbaccounttype'].forEach(function (id) { setVal(id, '0'); });
        ['txtbankfbpNo', 'txtFINo', 'txtFBC', 'txtfcyAmountThirdParty', 'txtFcyExchRthirdParty', 'txtHcyExchRthirdParty', 'txtReceiverExchangeRate', 'txtReceiverFcyAmount',
         'txtFCGrosamount', 'txtForeinchargesPkr', 'txtNetAmountFC', 'txtexchangeRate', 'txtNetAmount', 'txtbankDrAmount', 'txtLocalAmount', 'txtRemarks', 'cmbReferenceNo'].forEach(function (id) { setText(id, ''); });
        setVal('CmbChargesType', 1);
        show('btnsave', true); show('btnUpdate', false);
        setText('txtExportvoucherRate', '0');
        if (termId() !== 2) consigneeVisible(false);
        S.breakups = []; S.brkGridCleared = true;
        clearCombo('CmbChargesFDBCNo');
        S.party = [{ RealizedAmount: 0, FBC: 0, TotalAmount: 0 }];
        partyPanelVisible();
        calculateRemaining();
        show('grdPaymentTermsBox', false);
        S.terms = [];
        renderAll();
        focus('txtdate');
    }
    /** btnnew_Click. */
    function btnnew() { resetMain(); resetDetail(); show('btnsave', true); show('btnUpdate', false); resetBreakUp(); }
    /** btnRefresh_Click. */
    function btnRefresh(btn) { return busy(btn, function () { return getJson(API + '/refresh').then(function (d) { bindRefresh(d || {}); }).catch(function (e) { box(e.message); }); }); }

    /** FormValidation repeated client-side (message before the confirm, as on the desktop; the server repeats it). */
    function formValidation() {
        var fbp = val('txtbankfbpNo').trim();
        var checks = [
            [fbp === '' || fbp === '0', 'FBP Bank Field Required', 'txtbankfbpNo'],
            [val('txtFCGrosamount').trim() === '' || netD(val('txtFCGrosamount')) === 0, 'Fc Gross Field Required', 'txtFCGrosamount'],
            [val('txtNetAmountFC').trim() === '' || netD(val('txtNetAmountFC')) === 0, 'Fc Net Amount Field Required', 'txtNetAmountFC'],
            [val('txtexchangeRate').trim() === '' || netD(val('txtexchangeRate')) === 0, 'Exchange Rate Field Required', 'txtexchangeRate'],
            [val('txtNetAmount').trim() === '' || netD(val('txtNetAmount')) === 0, 'Net Amount Field Required', 'txtNetAmount'],
            [netI(val('comSuplier')) === 0, 'Customer  Field Required', 'comSuplier'],
            [termId() === 0, 'Payment term Field Required', 'cmbPaymentTerm'],
            [termText() === 'Invoice Payment' && netI(val('CmbGainLossAc')) === 0, 'GainLossAc Field Required', 'CmbGainLossAc'],
            [termId() === 2 && netI(val('CmbConsignee')) === 0, 'Consignee Field Required', 'CmbConsignee'],
            [termId() === 2 && val('txtFINo') === '', 'FI No Field Required', 'txtFINo'],
            [termId() === 1 && selText('cmbInvoiceNo') === '', 'Invoice Field Required', 'cmbInvoiceNo'],
            [acctType() === 0, ' Account Type Field Required', 'cmbaccounttype'],
            [netD(val('txtFBC')) > 0 && netI(val('CmbFBCDebitAccount')) === 0, 'FBC Debit Account Field Required', 'CmbFBCDebitAccount'],
            [netI(val('cmbbankAcc')) === 0, ' Account (DR) Field Required', 'cmbbankAcc'],
            [netI(val('cmbcurrency')) === 0, 'Currency Code Field Required', 'cmbcurrency']
        ];
        for (var i = 0; i < checks.length; i++) if (checks[i][0]) { box(checks[i][1]); focus(checks[i][2]); return false; }
        return true;
    }
    function payload() {
        return {
            recId: S.id, branchReset: S.branchReset,
            paymentTerm: termText(), paymentTermId: termId(), accountTypeId: acctType(), transTypeId: netI(radio('fcyTrans')),
            DocumentNo: netI(val('txtdocNumber')), DocumentDate: val('txtdate'),
            SupplierCustomerId: netI(val('comSuplier')), ExImLcOrderId: netI(val('cmbLCOrder')), ExImInvoiceId: netI(val('cmbInvoiceNo')),
            InvoiceNoText: selText('cmbInvoiceNo'),
            BankGlDrAc: netI(val('cmbbankAcc')), BankAccountText: selText('cmbbankAcc'), BankFbpNo: val('txtbankfbpNo'),
            MultiCurrencyId: netI(val('cmbcurrency')), CurrencyCode: selText('cmbcurrency'),
            FcGrossAmount: netD(val('txtFCGrosamount')), FcFBCharges: netD(val('txtFBC')), FBCDebitAccountId: netI(val('CmbFBCDebitAccount')),
            FcNetAmount: netD(val('txtNetAmountFC')),
            ThirdCurrencyId: netI(val('cmbFcycodeThirdParty')), ThirdCurrencyCode: selText('cmbFcycodeThirdParty'),
            ThirdCurrencyHcyExchangeRate: netD(val('txtHcyExchRthirdParty')), ThirdCurrencyFcyExchangeRate: netD(val('txtFcyExchRthirdParty')),
            ThirdCurrencyAmount: netD(val('txtfcyAmountThirdParty')), ThirdCurrencyReceiverExchangeRate: netD(val('txtReceiverExchangeRate')),
            ThirdCurrencyReceiverFcyAmount: netD(val('txtReceiverFcyAmount')),
            ExchangeRate: netD(val('txtexchangeRate')), NetAmountRs: netD(val('txtNetAmount')), BankDrAmount: netD(val('txtbankDrAmount')),
            GainLossAccountId: netI(val('CmbGainLossAc')), VoucherExchangeRate: netD(val('txtExportvoucherRate')),
            Remarks: val('txtRemarks'), ReferenceNo: val('cmbReferenceNo'),
            BalanceAmount: netD(val('txtBalanceAmount')),
            ConsigneeId: netI(val('CmbConsignee')), FINo: val('txtFINo'),
            charges: S.charges, breakups: S.brkGridCleared ? [] : S.breakups, partyBreakups: S.party, paymentTerms: S.terms
        };
    }
    /** Insert(). */
    function insert(btn) {
        return busy(btn, function () {
            if (!formValidation()) return Promise.resolve();
            if (!ask(S.id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return Promise.resolve();
            var pre = null;
            if ($id('chkpreview').checked && global.CrystalPrint && global.CrystalPrint.reserve) pre = global.CrystalPrint.reserve();
            return postJson(API + '/save', payload()).then(function (d) {
                box(d.message);
                resetMain();
                S.voucherHeadId = netI(d.voucherHeadId);
                if ($id('chkpreview').checked && global.CrystalPrint) {
                    /* VoucherReports.VoucherReport(DocumentTypeId 203, Id = VoucherHeadId, ApprovedFilter "All") */
                    global.CrystalPrint.open('acc-102', { id: S.voucherHeadId, documentTypeId: 203 }, null, pre);
                }
                S.id = 0;
            }).catch(function (e) { if (pre && global.CrystalPrint.release) global.CrystalPrint.release(pre); box(e.message); });
        });
    }
    function btnsave(btn) { S.id = 0; return insert(btn); }
    function btnUpdate(btn) { return insert(btn); }
    /** btnPrint_Click -> GenerateReportVoucher102: VoucherHeadId, no ApprovedFilter (IsApproved = false is sent). */
    function voucher102(btn) {
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
        return global.CrystalPrint.open('acc-102', { id: S.voucherHeadId, documentTypeId: 203, isApproved: 0 }, btn);
    }
    /** btnSlip_Click -> CommonServices.ExImRptFCBankReceipts_516(Id). */
    function slip516(btn) {
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
        if (S.id === 0) { box('No Record Found For Display'); return; }
        return global.CrystalPrint.open('exp-516', { id: S.id }, btn);
    }
    function notPorted(name) { box(name + ' is a desktop pop-up form that is not available in the web version.'); }

    /** ReadById(ID) - the desktop does not reset first (history Edit does RESETMAIN before it, double-click does not). */
    async function readById(id) {
        S.docSeq++;
        try {
            var d = await getJson(API + '/by-id?id=' + id);
            var h = d.header || {};
            S.id = id; S.previousBalance = 0;
            show('btnsave', false); show('btnUpdate', true);
            S.voucherHeadId = netI(d.voucherHeadId);
            setText('txtdocNumber', h.DocumentNo);
            tab('fcyForm');
            setText('txtdate', isoDate(h.DocumentDate) || today());
            selectByText('cmbPaymentTerm', h.ExmLcPaymentTerm);
            setText('txtFCGrosamount', fmtH(h.FcGrossAmount, 3));
            if (h.ExmLcPaymentTerm === 'Advanced Payment' || netI(h.ExImLcOrderId) > 0) {
                var c = await getJson(API + '/customers?term=' + encodeURIComponent('Advanced Payment') + '&termId=2');
                if ((c.customers || []).length) { bind('comSuplier', c.customers, 'Id', 'name'); bind('CmbConsignee', c.consignees, 'Id', 'name'); }
                bind('cmbLCOrder', [{ Id: h.ExImLcOrderId, name: h.LcOrderNo }], 'Id', 'name'); setVal('cmbLCOrder', h.ExImLcOrderId);
                setEnabled('cmbLCOrder', true); setEnabled('cmbInvoiceNo', false);
                await paymentTermLeave();
                var sa = d.shipmentAdvances || [];
                if (sa.length) { consigneeVisible(true); setVal('CmbConsignee', sa[0].ConsigneeId); setText('txtFINo', sa[0].FINo); }
            }
            if (netI(h.ExImInvoiceId) > 0) {
                S.previousBalance = netD(val('txtFCGrosamount').trim());
                var c2 = await getJson(API + '/customers?term=' + encodeURIComponent('Invoice Payment') + '&termId=1');
                if ((c2.customers || []).length) bind('comSuplier', c2.customers, 'Id', 'name');
                if ((c2.consignees || []).length) bind('CmbConsignee', c2.consignees, 'Id', 'name');
                bind('cmbInvoiceNo', [{ Id: h.ExImInvoiceId, name: h.InvoiceNo }], 'Id', 'name'); setVal('cmbInvoiceNo', h.ExImInvoiceId);
                setEnabled('cmbInvoiceNo', true); setEnabled('cmbLCOrder', false);
                show('grdPaymentTermsBox', true);
                if (termText() === 'Invoice Payment') { await invoiceAmountGet(); await paymentTermDetail(netI(h.ExImInvoiceId)); }
                else { S.terms = []; renderTerms(); }
            }
            if (h.ExmLcPaymentTerm === 'Other Payment') await allExportCustomers('Other Payment', 3);
            if (netI(h.ExImInvoiceId) === 0 && netI(h.ExImLcOrderId) === 0) {
                var c3 = await getJson(API + '/customers?term=' + encodeURIComponent('Invoice Payment') + '&termId=1');
                if ((c3.customers || []).length) bind('comSuplier', c3.customers, 'Id', 'name');
            }
            setVal('comSuplier', h.SupplierCustomerId);
            setVal('cmbaccounttype', h.DebitAccountTypeId);
            var a = acctType();
            if (a === 1) bankAccountDr(15);
            if (a === 2) bankAccountDr(22);
            if (a === 3) bankAccountDr(2);
            setVal('cmbbankAcc', h.BankGlDrAc);
            setText('txtbankfbpNo', h.BankFbpNo);
            setVal('cmbcurrency', h.MultiCurrencyId);
            setText('txtNetAmountFC', fmtH(h.FcNetAmount, 3));
            setText('txtexchangeRate', dstr(h.ExchangeRate));
            setVal('cmbFcycodeThirdParty', h.ThirdCurrencyId);
            setText('txtHcyExchRthirdParty', dstr(h.ThirdCurrencyHcyExchangeRate));
            setText('txtFcyExchRthirdParty', dstr(h.ThirdCurrencyFcyExchangeRate));
            setText('txtfcyAmountThirdParty', dstr(h.ThirdCurrencyAmount));
            setText('txtReceiverExchangeRate', dstr(h.ThirdCurrencyReceiverExchangeRate));
            setText('txtReceiverFcyAmount', dstr(h.ThirdCurrencyReceiverFcyAmount));
            setText('txtNetAmount', fmtH(h.NetAmountRs, 2));
            setText('txtbankDrAmount', fmtH(h.BankDrAmount, 2));
            setVal('CmbGainLossAc', h.VoucherHeadId);
            setText('txtRemarks', h.Remarks);
            setText('cmbReferenceNo', h.ReferenceNo);
            setText('txtFBC', dstr(h.FcFBCharges));
            setVal('CmbFBCDebitAccount', h.FBCDebitAccountId);
            var tt = netI(h.TransTypeId);
            $id(tt === 1 ? 'RDBankSingleAndPartiesMulti' : tt === 3 ? 'rdMultiBankAndMultiParties' : 'RDMultiBankDevice').checked = true;
            partyPanelVisible();
            S.charges = (d.charges || []).map(function (r) { return { FDBCNo: r.FDBCNo, TypeId: r.TypeId, Type: r.Type, ChargesTypeId: r.ChargesTypeId, ChargesType: r.ChargesType, Prcnt: r.Prcnt, Remarks: r.Remarks, FCAmount: r.FCAmount, RsAmount: r.RsAmount }; });
            var t = chargesTotals();
            if (termId() === 4) {
                await paymentTerm();     /* comSuplier_Leave */
                show('panelInvoiceWiseBreakup', true);
                (d.breakups || []).forEach(function (b) { S.breakups.push(b); });   /* tablebreakup is not cleared by ReadById itself */
                gdBindInExpenses();
            }
            if (S.breakups.length) { S.brkGridCleared = false; renderBreakups(); }
            else show('panelInvoiceWiseBreakup', false);
            setText('txtForeinchargesPkr', fmt00(t.foreign));
            setText('txtLocalAmount', fmt00(t.local));
            renderCharges();
            S.party = (d.partyBreakups || []).length ? d.partyBreakups.map(function (p) { return { RealizedAmount: p.RealizedAmount, FBC: p.FBC, TotalAmount: p.TotalAmount }; })
                : [{ RealizedAmount: 0, FBC: 0, TotalAmount: 0 }];
            renderParty();
            calculateRemaining();
        } catch (e) { box(e.message); }
    }

    // ------------------------------------------------------------------------------ history

    var HCOLS = ['DocumentNo', 'DocumentDate', 'CustomerName', 'InvoiceNo', 'PaymentTerm', 'AccountType', 'BankGLDrAc', 'BankFbpNo', 'BankDrAmount',
        'CurrencyCode', 'FcGrossAmount', 'ReferenceNo', 'FBCAccount', 'FcFBCharges', 'FcNetAmount', 'ExchangeRate', 'NetAmountRs', 'EntryDate', 'UserName', 'NoOfAttachments'];
    var HNUM = { BankDrAmount: 1, FcGrossAmount: 1, FcFBCharges: 1, FcNetAmount: 1, NetAmountRs: 1 };
    function histRender() {
        $id('fcyHistBody').innerHTML = HIST.map(function (r, i) {
            var h = '<tr data-i="' + i + '"' + (i === CUR.hist ? ' class="is-current"' : '') + '>'
                + '<td class="win-cell-btn"><button type="button" class="win-edit" data-hv="' + i + '">Voucher</button></td>'
                + '<td class="win-cell-btn"><button type="button" class="win-edit" data-hs="' + i + '">Slip</button></td>'
                + '<td class="win-cell-btn"><button type="button" class="win-edit" data-he="' + i + '">Edit</button></td>'
                + '<td class="win-cell-btn"><button type="button" class="win-edit" data-hg="' + i + '">G&amp;LBreakup</button></td>';
            HCOLS.forEach(function (c) {
                var v = col(r, c), t;
                if (HNUM[c]) t = fmt(v, 3);
                else if (c === 'ExchangeRate') t = fmt(v, 3);
                else if (c === 'DocumentDate') t = ddMMMyy(v);
                else if (c === 'EntryDate') t = ddMMMyyHm(v);
                else t = str(v);
                if (c === 'DocumentNo') h += '<td class="num"><a class="win-code" data-he="' + i + '" href="javascript:void(0)">' + esc(t) + '</a></td>';
                else if (c === 'NoOfAttachments') h += '<td class="num"><a class="win-code" data-ha="' + i + '" href="javascript:void(0)">' + esc(netI(v)) + '</a></td>';
                else h += '<td' + (HNUM[c] || c === 'ExchangeRate' ? ' class="num"' : '') + '>' + esc(t) + '</td>';
            });
            return h + '<td class="win-cell-btn"><button type="button" class="win-edit" data-hatt="' + i + '">Add Attachment</button></td></tr>';
        }).join('');
        var cols = HCOLS;
        $id('fcyHistFoot').innerHTML = HIST.length ? sumRow(cols, HIST, Object.keys(HNUM), 4, function (v) { return fmt(v, 3); }).replace('</tr>', '<td></td></tr>') : '';
        show('fcyHistEmpty', HIST.length === 0);
    }
    /** btnShowHistory_Click -> bindHistory. */
    function historyShow(btn) {
        return busy(btn, function () {
            var f = {
                fromChecked: $id('FromDateHistoryChk').checked, fromDate: val('FromDateHistory'),
                toChecked: $id('ToDateHistoryChk').checked, toDate: val('ToDateHistory'),
                fromDocNo: netI(val('FromDocNoHistory')), toDocNo: netI(val('ToDocNoHistory')),
                customerId: netI(val('CmbCustomerHistory')), paymentTerm: selText('CmbPaymentTermHistory'),
                invoiceId: netI(val('CmbInvoiceNoHistory')), receiverAccountId: netI(val('CmbReceiverAcHistory'))
            };
            return postJson(API + '/history', f).then(function (rows) { HIST = rows || []; CUR.hist = -1; histRender(); }).catch(function (e) { box(e.message); });
        });
    }
    /** btnResetHistory_Click: From = today - 3, To = today, voucher nos and customer cleared, grid emptied. */
    function historyReset() {
        setText('FromDateHistory', daysAgo(3)); setText('ToDateHistory', today());
        setText('FromDocNoHistory', ''); setText('ToDocNoHistory', '');
        setVal('CmbCustomerHistory', '0');
        HIST = []; histRender(); show('fcyHistEmpty', false);
    }
    function historyComboFill(d) {
        if ((d.historyCustomers || []).length) bind('CmbCustomerHistory', d.historyCustomers, 'Id', 'name');
        if ((d.historyPaymentTerms || []).length) bind('CmbPaymentTermHistory', d.historyPaymentTerms, 'Id', 'name');
        if ((d.historyInvoices || []).length) bind('CmbInvoiceNoHistory', d.historyInvoices, 'Id', 'name');
        if ((d.historyReceiverAccounts || []).length) bind('CmbReceiverAcHistory', d.historyReceiverAccounts, 'Id', 'name');
    }
    function historyRefresh(btn) { return busy(btn, function () { return getJson(API + '/history-combos').then(function (d) { historyComboFill(d || {}); }).catch(function (e) { box(e.message); }); }); }
    /** G&LBreakup -> GainAndLossBreakUp (minimum): USP_GetGainAndLossBreackup rows. */
    function gainLoss(i) {
        var r = HIST[i]; if (!r) return;
        getJson(API + '/gain-loss?id=' + netI(r.Id)).then(function (rows) {
            rows = rows || [];
            var keys = rows.length ? Object.keys(rows[0]) : ['No record'];
            $id('fcyGlHead').innerHTML = '<tr>' + keys.map(function (k) { return '<th>' + esc(k) + '</th>'; }).join('') + '</tr>';
            $id('fcyGlBody').innerHTML = rows.map(function (x) { return '<tr>' + keys.map(function (k) { var v = x[k]; return typeof v === 'number' ? '<td class="num">' + esc(fmt(v, 4)) + '</td>' : '<td>' + esc(v) + '</td>'; }).join('') + '</tr>'; }).join('');
            show('fcyGl', true);
        }).catch(function (e) { box(e.message); });
    }
    function glClose() { show('fcyGl', false); }

    // ------------------------------------------------------------------------------ wiring

    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }

    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.win-tabs[data-tabs="fcy"] .win-tab').forEach(function (b) { b.addEventListener('click', function () { tab(b.getAttribute('data-tab')); }); });
        document.querySelectorAll('button[data-fullscreen]').forEach(function (b) {
            b.addEventListener('click', function () { var bx = $id(b.getAttribute('data-fullscreen')); if (bx) bx.classList.toggle('ex-fullscreen'); });
        });
        on('cmbPaymentTerm', 'change', paymentTermLeave);
        on('comSuplier', 'change', paymentTerm);                       /* comSuplier_Leave */
        on('cmbaccounttype', 'change', accountTypeLeave);
        on('cmbInvoiceNo', 'change', invoiceNoChanged);
        on('cmbLCOrder', 'change', contractAmount);
        on('txtFCGrosamount', 'input', function () { grossOrRateChanged(false); });
        on('txtFCGrosamount', 'blur', thirdExchRateByFcGross);
        on('txtexchangeRate', 'input', function () { grossOrRateChanged(true); });
        on('txtexchangeRate', 'blur', function () { if (val('txtFCGrosamount') !== '' && val('txtexchangeRate') !== '') setText('txtNetAmount', fmt(netD(val('txtFCGrosamount')) * netD(val('txtexchangeRate')), 4)); });
        on('txtfcamount', 'input', netAmountFC);
        on('txtpkramount', 'input', function () { calculatePercntAmount('PkrAmount'); });
        on('txtChargePercent', 'input', function () { calculatePercntAmount('Percent'); });
        on('CmbChargesType', 'change', function () { calculatePercntAmount(''); });
        on('CmbChargesAccountType', 'change', chargesAccountLeave);
        on('CmbChargesFDBCNo', 'change', breakupInvoiceLeave);         /* the designer wires its Leave to cmbInvoicenobreakup_Leave */
        on('txtfcyAmountThirdParty', 'input', fcGrossAmtByThirdParty);
        on('txtFcyExchRthirdParty', 'input', fcGrossAmtByThirdParty);
        on('txtReceiverExchangeRate', 'input', receiverAmount);
        on('cmbInvoicenobreakup', 'change', breakupInvoiceLeave);
        /* CmbGainLossAc_Leave - the designer wires CmbGainLossAc, CmbConsignee and CmbFBCDebitAccount Leave to it:
           CmbChargesType.Focus(). Reproduced when the field is left with Tab / Enter (a mouse click elsewhere keeps its target). */
        ['CmbGainLossAc', 'CmbConsignee', 'CmbFBCDebitAccount'].forEach(function (id) {
            var s0 = $id(id), w0 = s0 && s0.closest ? s0.closest('.dtcombo-wrap') : null;
            var host = w0 || s0; if (!host) return;
            host.addEventListener('keydown', function (e) {
                if ((e.key === 'Tab' && !e.shiftKey) || e.key === 'Enter') {
                    var pop = host.querySelector && host.querySelector('.dtcombo-pop');
                    if (pop && pop.style.display === 'block') return;
                    e.preventDefault(); e.stopPropagation(); focus('CmbChargesType');
                }
            });
        });
        on('txtCommisionamtbreakUp', 'input', realizedAuto);
        on('txtInvoiceAmountBreakup', 'input', realizedAuto);
        document.querySelectorAll('input[name="fcyTrans"]').forEach(function (r) { r.addEventListener('change', partyPanelVisible); });
        /* charges grid */
        var cb = $id('chgBody');
        cb.addEventListener('click', function (e) {
            var d = e.target.closest('button[data-cdel]'); if (d) { chargeDelete(+d.getAttribute('data-cdel')); return; }
            var ed = e.target.closest('a[data-cedit]'); if (ed) chargeEdit(+ed.getAttribute('data-cedit'));
        });
        cb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) chargeEdit(+tr.getAttribute('data-i')); });
        /* breakup grid */
        var bb = $id('brkBody');
        bb.addEventListener('click', function (e) {
            var d = e.target.closest('button[data-bdel]'); if (d) { breakupDelete(+d.getAttribute('data-bdel')); return; }
            var ed = e.target.closest('a[data-bedit]'); if (ed) breakupEdit(+ed.getAttribute('data-bedit'));
        });
        bb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) breakupEdit(+tr.getAttribute('data-i')); });
        /* party grid: X / + buttons, CellUpdated (Total = Realized + FBC when Realized > 0), FBCCalculations */
        var pb = $id('partyBody');
        pb.addEventListener('click', function (e) {
            var d = e.target.closest('button[data-pdel]');
            if (d) { S.party.splice(+d.getAttribute('data-pdel'), 1); if (!S.party.length) S.party.push({ RealizedAmount: 0, FBC: 0, TotalAmount: 0 }); renderParty(); fbcCalculations(); return; }
            var a = e.target.closest('button[data-padd]');
            if (a) { S.party.push({ RealizedAmount: 0, FBC: 0, TotalAmount: 0 }); renderParty(); fbcCalculations(); }
        });
        pb.addEventListener('change', function (e) {
            var inp = e.target.closest('input[data-pi]'); if (!inp) return;
            var r = S.party[+inp.getAttribute('data-pi')]; if (!r) return;
            r[inp.getAttribute('data-pc')] = netD(inp.value);
            if (netD(r.RealizedAmount) > 0) r.TotalAmount = netD(r.RealizedAmount) + netD(r.FBC);
            renderParty(); fbcCalculations();
        });
        /* payment terms grid: ThisReceipt edit with the Balance check */
        $id('termsBody').addEventListener('change', function (e) {
            var inp = e.target.closest('input[data-ti]'); if (!inp) return;
            var r = S.terms[+inp.getAttribute('data-ti')]; if (!r) return;
            var v = parseFloat(String(inp.value).replace(/,/g, ''));
            if (!isFinite(v)) { box('Invalid input in This Receipt field.'); inp.value = dstr(r.ThisReceipt); return; }
            r.ThisReceipt = v;
            if (v > netD(r.Balance)) { r.ThisReceipt = 0; box('This Receipt cannot be greater than the balance amount. Balance amount is ' + dstr(r.Balance) + ' and This Receipt is ' + dstr(v) + '.'); }
            renderTerms();
        });
        /* history grid */
        var hb = $id('fcyHistBody');
        hb.addEventListener('click', function (e) {
            var t;
            if ((t = e.target.closest('button[data-hv]'))) { var r1 = HIST[+t.getAttribute('data-hv')]; if (r1 && global.CrystalPrint) global.CrystalPrint.open('acc-102', { id: netI(r1.VoucherHeadId), documentTypeId: 203, isApproved: 0 }, t); return; }
            if ((t = e.target.closest('button[data-hs]'))) { var r2 = HIST[+t.getAttribute('data-hs')]; if (r2 && global.CrystalPrint) global.CrystalPrint.open('exp-516', { id: netI(r2.Id) }, t); return; }
            if ((t = e.target.closest('[data-he]'))) { var r3 = HIST[+t.getAttribute('data-he')]; if (r3) { resetMain(); readById(netI(r3.Id)); } return; }
            if ((t = e.target.closest('button[data-hg]'))) { gainLoss(+t.getAttribute('data-hg')); return; }
            if ((t = e.target.closest('a[data-ha]'))) { notPorted('Attachment view'); return; }
            if ((t = e.target.closest('button[data-hatt]'))) { notPorted('Add Attachment'); return; }
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            CUR.hist = +tr.getAttribute('data-i');
            hb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
        });
        hb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) { var r = HIST[+tr.getAttribute('data-i')]; if (r) readById(netI(r.Id)); } });
        /* Acfrmfcbankreceipt_KeyDown */
        document.addEventListener('keydown', function (e) {
            if (!$id('fcyGl').classList.contains('is-hidden')) { if (e.key === 'Escape') glClose(); return; }
            var k = (e.key || '').toLowerCase();
            if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox') {
                var f = Array.prototype.filter.call(document.querySelectorAll('input:not([type=hidden]):not(:disabled), select:not(:disabled), button:not(:disabled), textarea:not(:disabled)'), function (x) { return x.offsetParent !== null; });
                var i = f.indexOf(e.target);
                if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); }
                return;
            }
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); return; }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); cancel(); return; }
            if (!onHistory()) {
                if (e.ctrlKey && k === 's' && !$id('btnsave').classList.contains('is-hidden') && !$id('btnsave').disabled) { e.preventDefault(); btnsave($id('btnsave')); }
                if (e.ctrlKey && k === 'u' && !$id('btnUpdate').classList.contains('is-hidden') && !$id('btnUpdate').disabled) { e.preventDefault(); btnUpdate($id('btnUpdate')); }
                if (e.ctrlKey && k === 'r') { e.preventDefault(); btnRefresh($id('btnRefresh')); }
                if (e.ctrlKey && k === 'n') { e.preventDefault(); btnnew(); }
                if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); notPorted('Attachments'); }
                if (e.ctrlKey && e.key === 'ArrowLeft') { e.preventDefault(); focus('cmbInvoicenobreakup'); }
                if (e.ctrlKey && e.key === 'ArrowRight') { e.preventDefault(); focus(breakupMode() ? 'CmbChargesFDBCNo' : 'CmbChargesType'); }
                if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); focus('txtdate'); }
                return;
            }
            if (e.ctrlKey && k === 's') { e.preventDefault(); historyShow($id('btnShowHistory')); }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); historyReset(); }
            if (e.ctrlKey && k === 'r') { e.preventDefault(); historyRefresh($id('btnRefreshHistory')); }
            if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); focus('FromDateHistory'); }
        });
        load();
    });

    global.FcyBank = {
        btnnew: btnnew, btnsave: btnsave, btnUpdate: btnUpdate, btnRefresh: btnRefresh, voucher102: voucher102, slip516: slip516, notPorted: notPorted,
        btnAddinGrid: btnAddinGrid, btnUpdateDetail: btnUpdateDetail, btnCancelUpdateDetial: btnCancelUpdateDetial, remainingBalance: calculateRemaining,
        btnAddBreakup: btnAddBreakup, btnUpdatebreakup: btnUpdatebreakup, resetBreakUp: resetBreakUp,
        historyShow: historyShow, historyReset: historyReset, historyRefresh: historyRefresh, toggleHistory: toggleHistory, glClose: glClose
    };
}(window));
