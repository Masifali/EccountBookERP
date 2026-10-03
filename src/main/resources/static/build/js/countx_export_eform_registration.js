/* ============================================================================================
 * countx_export_eform_registration.js - two desktop forms of Architecture.WinApp.Export that share one
 * BLL (ExImEFormRegistration) and almost all of their code:
 *   217 ExpfrmEformRegistration   (body data-variant="ef")  /api/export/eform-registration
 *   218 ExpfrmFinancialInsturment (body data-variant="fi")  /api/export/financial-instrument
 * Every button, Leave, TextChanged, grid double-click, tab change and shortcut of the desktop forms has its
 * counterpart here with the desktop's messages and order. The ids are the desktop control names; where the two
 * forms name the same control differently, ID[] maps the logical name to the form's own control.
 *
 * Button contract: disabled + spinner while a request runs, duplicates ignored, re-enabled on success/failure.
 * ============================================================================================ */
(function (global) {
    'use strict';

    var VARIANT = (document.body && document.body.getAttribute('data-variant')) || 'ef';
    var FI = VARIANT === 'fi';
    var API = FI ? '/api/export/financial-instrument' : '/api/export/eform-registration';
    var ID = FI ? {
        customer: 'cmbConsignee', bank: 'cmbBank', formNo: 'txtfiuniqueNo', formDate: 'txtfidate', delivery: 'cmbDeliverymode',
        currency: 'cmbFICode', valueFc: 'txtfivalue', country: 'cmbconsgneeocuntry', destPort: 'cmbPortOfDischarge'
    } : {
        customer: 'cmbCustomer', bank: 'cmbIssueBank', formNo: 'txtFormNo', formDate: 'txtFormdate', delivery: 'cmbDeliveryTerm',
        currency: 'cmbCurrencyCode', valueFc: 'txtValueFc', country: 'cmbCountry', destPort: 'cmbDestinationPort'
    };

    // ------------------------------------------------------------------------------ helpers
    function $id(id) { return document.getElementById(id); }
    function box(m) { global.alert(m); }
    function esc(s) { return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    function netD(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isFinite(n) ? n : 0; }
    function netI(v) { var n = parseInt(String(v === null || v === undefined ? '' : v), 10); return isFinite(n) ? n : 0; }
    /* Conversion.ToInt(text) = Convert.ToInt32(string), failures 0: "12.5" -> 0 */
    function toIntText(v) { var s = str(v).trim(); return /^[+-]?\d+$/.test(s) ? parseInt(s, 10) : 0; }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function today() { var d = new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function fmt(v) { var n = netD(v); var s = String(Math.round(n * 1000) / 1000); var p = s.split('.'); p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ','); return p.join('.'); }
    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) { if (b.disabled || b.classList.contains('is-busy')) return Promise.resolve(); b.disabled = true; b.classList.add('is-busy'); }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } };
        var p;
        try { p = fn(); } catch (e) { done(); box(e.message); return Promise.resolve(); }
        if (p && typeof p.then === 'function') return p.then(done, function (e) { done(); if (e && e.message) box(e.message); });
        done(); return Promise.resolve(p);
    }
    function parse(r) {
        return r.text().then(function (t) {
            var b = null; try { b = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
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
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function bind(id, rows, v, t) {
        var s = $id(id); if (!s) return;
        var h = '<option value="0"></option>';
        (rows || []).forEach(function (r) { h += '<option value="' + esc(r[v]) + '">' + esc(r[t]) + '</option>'; });
        s.innerHTML = h; s.value = '0'; refreshCombos();
    }
    function setVal(id, v) { var s = $id(id); if (!s) return; s.value = str(v); if (s.value !== str(v)) s.value = '0'; refreshCombos(); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setText(id, v) { var e = $id(id); if (e) e.value = str(v); }
    /* combo .Text - the shown text of the chosen row ("" when nothing chosen) */
    function comboText(id) { var s = $id(id); if (!s || s.selectedIndex < 0) return ''; var o = s.options[s.selectedIndex]; return o && o.value !== '0' ? o.text : ''; }
    function setComboText(id, text) {
        var s = $id(id); if (!s) return;
        s.value = '0';
        for (var i = 0; i < s.options.length; i++) if (s.options[i].text === str(text)) { s.selectedIndex = i; break; }
        refreshCombos();
    }
    function focus(id) { var e = $id(id); if (!e) return; var w = e.closest && e.closest('.dtcombo-wrap'); var t = w ? w.querySelector('.dtcombo-input') : e; try { (t || e).focus(); } catch (x) { /* ignore */ } }
    function cancel() { global.close(); setTimeout(function () { if (!global.closed) { if (history.length > 1) history.back(); else location.href = '/export'; } }, 150); }

    // ------------------------------------------------------------------------------ state
    var PERM = { Save: true, Update: true, Print: true };
    var S = {
        recId: 0, updateMode: false, advanceUtilizationGrid: 0,
        com: [], pay: [], util: [], payBound: false, utilBound: false, comBound: false,
        comIdx: -1, payIdx: -1, utilIdx: -1, getFcBankUtlizedAmount: 0
    };
    var HIST = [];

    // ------------------------------------------------------------------------------ tabs
    function tab(panelId) {
        document.querySelectorAll('.win-tabs .win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === panelId); });
        ['efForm', 'efHistory'].forEach(function (p) { $id(p).classList.toggle('is-active', p === panelId); });
        var onHist = panelId === 'efHistory';
        var fb = $id('btnFooterHistory');
        fb.querySelector('span').textContent = onHist ? 'Form' : 'History';
        /* tabControl1_SelectedIndexChanged: History -> HistoryFill */
        if (onHist) historyFill();
    }
    function onHistory() { return $id('efHistory').classList.contains('is-active'); }
    function toggleHistory() { tab(onHistory() ? 'efForm' : 'efHistory'); }

    // ------------------------------------------------------------------------------ load / combos
    function bindCombos(d) {
        ['customersError', 'paymentTermsError', 'currenciesError', 'countriesError', 'banksError', 'portsError', 'deliveryTermsError'].forEach(function (k) { if (d[k]) box(d[k]); });
        if (d.customers && d.customers.length) bind(ID.customer, d.customers, 'Id', 'name');
        if (d.paymentTerms && d.paymentTerms.length) { bind('cmbPaymentTerm', d.paymentTerms, 'Id', 'name'); if (FI) bind('cmbpaymentmode', d.paymentTerms, 'Id', 'name'); }
        if (d.currencies && d.currencies.length) bind(ID.currency, d.currencies, 'Id', 'name');
        if (d.countries && d.countries.length) bind(ID.country, d.countries, 'Id', 'name');
        if (d.banks && d.banks.length) bind(ID.bank, d.banks, 'Id', 'name');
        if (d.ports && d.ports.length) { bind('cmbLoadingPort', d.ports, 'Id', 'name'); bind(ID.destPort, d.ports, 'Id', 'name'); }
        if (d.deliveryTerms && d.deliveryTerms.length) bind(ID.delivery, d.deliveryTerms, 'Id', 'name');
    }
    function load() {
        return getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = d.permissions;
            $id('btnSave').disabled = !PERM.Save;
            $id('btnUpdate').disabled = !PERM.Update;
            bindCombos(d);
            if (FI) { bind('cmbTradeType', [{ Id: 1, name: 'Import' }, { Id: 2, name: 'Export' }], 'Id', 'name'); setVal('cmbTradeType', 1); }
            setText(ID.formDate, today());
            if (FI) setText('txtexpirydate', today());
            renderAll();
            $id('efFooterInfo').textContent = FI ? 'ExpfrmFinancialInsturment  -  Document Type 207' : 'ExpfrmEformRegistration';
            focus(FI ? ID.bank : ID.customer);
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    function reloadCombos() { return getJson(API + '/combos').then(function (d) { bindCombos(d || {}); }).catch(function (e) { box(e.message); }); }

    // ------------------------------------------------------------------------------ Leave events
    /** cmbCustomer_Leave: GetDataBySuppliercustomerId(SupplierCustomerId) -> cmbBankReerence and the header currency combo (rows only). */
    function customerLeave() {
        return getJson(API + '/receipts-by-customer?supplierCustomerId=' + netI(val(ID.customer))).then(function (rows) {
            if (rows && rows.length) {
                bind('cmbBankReerence', rows, 'Id', 'BankFbpNo');
                var keep = val(ID.currency);
                bind(ID.currency, rows, 'MultiCurrencyId', 'CurrencyCode');
                setVal(ID.currency, keep);
            }
        }).catch(function (e) { box(e.message); });
    }
    /** cmbBankReerence_Leave: GetDataBySuppliercustomerId(Id) -> Fc Amount, Utilized Amount, Balance, currency; then GetTotalFcAmount. */
    function bankRefLeave() {
        return getJson(API + '/receipt-by-id?id=' + netI(val('cmbBankReerence'))).then(function (rows) {
            if (rows && rows.length) {
                setText('txtFcAmountPaymentUtilization', String(netD(rows[0].FcNetAmount)));
                setText('txtUtilizeAmount', String(netD(rows[0].FcUtilizedAmount)));
                setText('txtBalance', String(netD(val('txtFcAmountPaymentUtilization')) - netD(val('txtUtilizeAmount'))));
                var keep = val('cmbcurrency');
                bind('cmbcurrency', rows, 'MultiCurrencyId', 'CurrencyCode');
                setVal('cmbcurrency', keep);
            }
            getTotalFcAmount();
        }).catch(function (e) { box(e.message); });
    }
    /** GetTotalFcAmount: the utilisation grid rows of the chosen bank reference REPLACE the Utilized Amount (desktop behaviour). */
    function getTotalFcAmount() {
        var ref = netI(val('cmbBankReerence'));
        S.util.forEach(function (r) { if (netI(r.BankReferenceId) === ref) S.getFcBankUtlizedAmount += Math.fround(netD(r.ThisUtilize)); });
        setText('txtUtilizeAmount', String(S.getFcBankUtlizedAmount));
        setText('txtBalance', String(netD(val('txtFcAmountPaymentUtilization')) - S.getFcBankUtlizedAmount));
        S.getFcBankUtlizedAmount = 0;
    }

    // ------------------------------------------------------------------------------ TextChanged checks
    /** txtExchangeRate_TextChanged: Amount Rs = ToInt(Value FC) * ToInt(rate) when the rate (as int) > 0. */
    function exchangeRateChanged() {
        var rate = toIntText(val('txtExchangeRate'));
        if (rate > 0) setText('txtAmountRs', String(toIntText(val(ID.valueFc)) * rate));
    }
    /** checkpaymentamount (txtPaymentPercent / txtFcAmountPayment TextChanged). */
    function checkPaymentAmount() {
        var paymentamount = 0, headerfcvalue = 0, pct = 0;
        if (val(ID.valueFc) !== '' && val('txtPaymentPercent') !== '') {
            headerfcvalue = netD(val(ID.valueFc));
            pct = netD(val('txtPaymentPercent'));
            setText('txtFcAmountPayment', String(headerfcvalue * pct / 100.0));
            paymentamount = netD(val('txtFcAmountPayment'));
        }
        if (pct > 100.0) { box('Payment Percent Is Greater Than 100 Plese Check'); focus('txtPaymentPercent'); $id('btnAddPayment').disabled = true; }
        else if (paymentamount > headerfcvalue) { box('Your PaymentFc Amount Is Greater Than Fc Value Please Check'); focus('txtFcAmountPayment'); $id('btnAddPayment').disabled = true; }
        else $id('btnAddPayment').disabled = false;
    }
    /** checkCommodityamountWeight (txtNetweightDetail / txtFCAmount TextChanged; also before an Add). */
    function checkCommodity() {
        var headerfcvalue = netD(val(ID.valueFc)), comFc = netD(val('txtFCAmount')), wHeader = netD(val('txtNetWeightkg'));
        if (netD(val('txtNetweightDetail')) > wHeader) { box('Weight Is Greater Than Net Weight :Please Check'); focus('txtNetweightDetail'); $id('btnCommodityAdd').disabled = true; }
        else if (comFc > headerfcvalue) { box('Your Fc Amount Is Greater Than Fc Value :Please Check'); focus('txtFCAmount'); $id('btnCommodityAdd').disabled = true; }
        else $id('btnCommodityAdd').disabled = false;
    }
    /** checkutlizedamount (txtThisUtilize TextChanged / Leave, grid cell events, after an Add). */
    function checkUtilized() {
        if (netD(val('txtThisUtilize')) > netD(val('txtBalance'))) { box('Your Utlized Amount Is Greater Than Balance Please Check'); focus('txtThisUtilize'); $id('btnAddPaymentUtilization').disabled = true; }
        else $id('btnAddPaymentUtilization').disabled = false;
    }

    // ------------------------------------------------------------------------------ grids
    function drawGrid(bodyId, footId, rows, cols, numCols, cur) {
        var sums = {};
        $id(bodyId).innerHTML = rows.map(function (r, i) {
            var h = '<tr data-i="' + i + '"' + (i === cur ? ' class="is-current"' : '') + '>';
            cols.forEach(function (c) {
                var v = r[c];
                if (numCols[c]) { sums[c] = (sums[c] || 0) + netD(v); h += '<td class="num">' + esc(fmt(v)) + '</td>'; }
                else h += '<td>' + esc(str(v)) + '</td>';
            });
            return h + '</tr>';
        }).join('');
        var foot = $id(footId);
        if (!rows.length) { foot.innerHTML = ''; return; }
        foot.innerHTML = '<tr>' + cols.map(function (c, k) { return numCols[c] ? '<td>' + esc(fmt(sums[c] || 0)) + '</td>' : '<td class="lbl">' + (k === 0 ? '&Sigma;' : '') + '</td>'; }).join('') + '</tr>';
    }
    var COM_COLS = ['Commodity', 'NetWeight', 'FcAmount'];
    var PAY_COLS = ['PaymentTerm', 'PaymentPercent', 'FcAmount', 'DueDays'];
    var UTIL_COLS = ['BankReference', 'CurrencyCode', 'ThisUtilize', 'Remarks'];
    function renderAll() {
        drawGrid('comBody', 'comFoot', S.com, COM_COLS, { NetWeight: 1, FcAmount: 1 }, S.comIdx);
        drawGrid('payBody', 'payFoot', S.pay, PAY_COLS, { PaymentPercent: 1, FcAmount: 1 }, S.payIdx);
        drawGrid('utilBody', 'utilFoot', S.util, UTIL_COLS, { ThisUtilize: 1 }, S.utilIdx);
    }

    // ------------------------------------------------------------------------------ commodity
    function btnCommodityAdd() {
        if (val('txtCommodity').trim() === '') { box('Please Insert Commodity'); focus('txtCommodity'); return; }
        if (val('txtNetweightDetail').trim() === '') { box('Please Insert Weight'); focus('txtNetweightDetail'); return; }
        if (val('txtFCAmount').trim() === '') { box('Please Insert FcAmount'); focus('txtFCAmount'); return; }
        checkCommodity();
        S.com.push({ Commodity: val('txtCommodity'), NetWeight: netD(val('txtNetweightDetail')), FcAmount: netD(val('txtFCAmount')) });
        S.comBound = true;
        setText('txtCommodity', ''); setText('txtNetweightDetail', ''); setText('txtFCAmount', '');
        renderAll(); focus('txtCommodity');
    }
    function comDblClick(i) {
        var r = S.com[i]; if (!r) return;
        S.comIdx = i;
        setText('txtCommodity', r.Commodity); setText('txtFCAmount', fmt(r.FcAmount)); setText('txtNetweightDetail', fmt(r.NetWeight));
        show('btnUpdateComodity', true); show('btnCancelComodity', true); show('btnCommodityAdd', false);
        focus('txtCommodity');
    }
    /** btnUpdateComodity_Click - the desktop leaves Update / Cancel visible afterwards. */
    function btnUpdateComodity() {
        var r = S.com[S.comIdx]; if (!r) { box('There is no row at position ' + S.comIdx + '.'); return; }
        r.Commodity = val('txtCommodity'); r.FcAmount = netD(val('txtFCAmount')); r.NetWeight = netD(val('txtNetweightDetail'));
        renderAll();
        setText('txtCommodity', ''); setText('txtNetweightDetail', ''); setText('txtFCAmount', '');
    }
    function formCommodityReset() {
        focus('txtCommodity');
        setText('txtCommodity', ''); setText('txtNetweightDetail', ''); setText('txtFCAmount', '');
        show('btnUpdateComodity', false); show('btnCancelComodity', false); show('btnCommodityAdd', true);
    }

    // ------------------------------------------------------------------------------ payment terms
    function btnAddPayment() {
        if (comboText('cmbPaymentTerm').trim() === '') { box('Please Select PaymentTerm'); focus('cmbPaymentTerm'); return; }
        if (val('txtPaymentPercent').trim() === '') { box('Please Insert Payment Percent '); focus('txtPaymentPercent'); return; }
        if (val('txtFcAmountPayment').trim() === '') { box('Please Insert FcAmount'); focus('txtFcAmountPayment'); return; }
        S.pay.push({ PaymentTermId: netI(val('cmbPaymentTerm')), PaymentTerm: comboText('cmbPaymentTerm'), PaymentPercent: netD(val('txtPaymentPercent')), FcAmount: netD(val('txtFcAmountPayment')), DueDays: val('txtDueDays') });
        S.payBound = true;
        setVal('cmbPaymentTerm', '0'); setText('txtPaymentPercent', ''); setText('txtFcAmountPayment', ''); setText('txtDueDays', '');
        renderAll(); focus('cmbPaymentTerm');
    }
    function payDblClick(i) {
        var r = S.pay[i]; if (!r) return;
        S.payIdx = i;
        setVal('cmbPaymentTerm', r.PaymentTermId); setText('txtPaymentPercent', fmt(r.PaymentPercent)); setText('txtFcAmountPayment', fmt(r.FcAmount)); setText('txtDueDays', r.DueDays);
        renderAll();
        show('btnUpdatePayment', true); show('btnCancelPayment', true); show('btnAddPayment', false);
    }
    function btnUpdatePayment() {
        var r = S.pay[S.payIdx]; if (!r) { box('There is no row at position ' + S.payIdx + '.'); return; }
        r.PaymentTermId = netI(val('cmbPaymentTerm')); r.PaymentTerm = comboText('cmbPaymentTerm'); r.PaymentPercent = netD(val('txtPaymentPercent'));
        r.FcAmount = netD(val('txtFcAmountPayment')); r.DueDays = val('txtDueDays');
        renderAll(); formPayment();
    }
    function formPayment() {
        focus('cmbPaymentTerm');
        setVal('cmbPaymentTerm', '0'); setText('txtPaymentPercent', ''); setText('txtFcAmountPayment', ''); setText('txtDueDays', '');
        show('btnUpdatePayment', false); show('btnCancelPayment', false); show('btnAddPayment', true);
    }

    // ------------------------------------------------------------------------------ advance payment utilization
    function btnAddPaymentUtilization() {
        if (comboText('cmbBankReerence').trim() === '') { box('Please Select BankReference'); focus('cmbBankReerence'); return; }
        if (comboText('cmbcurrency').trim() === '') { box('Please Select Currency'); focus('cmbcurrency'); return; }
        if (val('txtThisUtilize').trim() === '') { box('Please Insert ThisUtlized Amount'); focus('txtThisUtilize'); return; }
        if (S.pay.length > 0) {
            $id('btnAddPaymentUtilization').disabled = false;
            S.util.push({ BankReferenceId: netI(val('cmbBankReerence')), BankReference: comboText('cmbBankReerence'), CurrencyCodeId: netI(val('cmbcurrency')), CurrencyCode: comboText('cmbcurrency'), ThisUtilize: netD(val('txtThisUtilize')), Remarks: val('txtremarks') });
            S.utilBound = true;
            renderAll();
            checkUtilized();
            setText('txtThisUtilize', ''); setText('txtremarks', '');
            getTotalFcAmount();
        } else {
            box('Please Add Payment Terms First');
            $id('btnAddPaymentUtilization').disabled = true;
            focus('cmbPaymentTerm');
        }
    }
    function utilDblClick(i) {
        var r = S.util[i]; if (!r) return;
        S.utilIdx = i;
        setVal('cmbBankReerence', r.BankReferenceId); setVal('cmbcurrency', r.CurrencyCodeId); setText('txtThisUtilize', fmt(r.ThisUtilize)); setText('txtremarks', r.Remarks);
        bankRefLeave();
        renderAll();
        show('btnUpdatePaymentUtilization', true); show('btnCancelPaymentUtilization', true); show('btnAddPaymentUtilization', false);
    }
    function btnUpdatePaymentUtilization() {
        var r = S.util[S.utilIdx]; if (!r) { box('There is no row at position ' + S.utilIdx + '.'); return; }
        r.BankReferenceId = netI(val('cmbBankReerence')); r.BankReference = comboText('cmbBankReerence'); r.CurrencyCodeId = netI(val('cmbcurrency'));
        r.CurrencyCode = comboText('cmbcurrency'); r.ThisUtilize = netD(val('txtThisUtilize')); r.Remarks = val('txtremarks');
        renderAll();
        /* calculations() */
        var tot = 0; S.util.forEach(function (x) { tot += netD(x.ThisUtilize); });
        setText('txtBalance', String(netD(val('txtFcAmountPaymentUtilization')) - tot));
        formPaymentUtilization();
    }
    function formPaymentUtilization() {
        setVal('cmbBankReerence', '0'); setText('txtBalance', ''); setText('txtThisUtilize', ''); setText('txtFcAmountPaymentUtilization', '');
        setVal('cmbcurrency', '0'); setText('txtUtilizeAmount', '');
        show('btnUpdatePaymentUtilization', false); show('btnCancelPaymentUtilization', false); show('btnAddPaymentUtilization', true);
    }

    // ------------------------------------------------------------------------------ reset / read / save
    function formReset() {
        if (FI) {
            focus(ID.bank);
            setVal(ID.bank, '0'); setText('txtIBAN', ''); setVal('cmbTradeType', '0'); setVal('cmbpaymentmode', '0');
            setText(ID.formDate, today()); setText(ID.formNo, ''); $id('RBIsActive').checked = true;
            setVal(ID.customer, '0'); setText('txtconsigneeIban', ''); setVal(ID.country, '0'); setVal(ID.destPort, '0');
            setVal(ID.delivery, '0'); setVal(ID.currency, '0'); setText(ID.valueFc, '0'); setText('txtBalance', '0');
        } else {
            setVal(ID.customer, '0'); setVal(ID.bank, '0'); setVal(ID.delivery, '0'); setVal(ID.currency, '0'); setVal(ID.country, '0');
            setText(ID.formNo, ''); setText(ID.formDate, today()); setText(ID.valueFc, ''); setText('txtExchangeRate', '');
            setText('txtAmountRs', ''); setText('txtNetWeightkg', '');
        }
        S.getFcBankUtlizedAmount = 0;
        show('btnSave', true); show('btnUpdate', false);
        S.updateMode = false;
        S.pay = []; S.payBound = false; S.com = []; S.comBound = false; S.util = []; S.utilBound = false;
        setText('txtCommodity', ''); setText('txtNetweightDetail', ''); setText('txtFCAmount', '');
        show('btnUpdateComodity', false); show('btnCancelComodity', false); show('btnCommodityAdd', true);
        setVal('cmbPaymentTerm', '0'); setText('txtPaymentPercent', ''); setText('txtFcAmountPayment', ''); setText('txtDueDays', '');
        show('btnUpdatePayment', false); show('btnCancelPayment', false); show('btnAddPayment', true);
        setVal('cmbBankReerence', '0'); setText('txtBalance', FI ? '0' : ''); setText('txtThisUtilize', ''); setText('txtFcAmountPaymentUtilization', '');
        setVal('cmbcurrency', '0'); setText('txtUtilizeAmount', '');
        show('btnUpdatePaymentUtilization', false); show('btnCancelPaymentUtilization', false); show('btnAddPaymentUtilization', true);
        renderAll();
        return reloadCombos();
    }
    /** BtnNew_Click: FormReset, the three entry resets, then the combo reloads again. */
    function btnNew() {
        return formReset().then(function () { formCommodityReset(); formPayment(); formPaymentUtilization(); return reloadCombos(); });
    }
    /** ReadById (DataGridHistory_DoubleClick with Cells[1] = Id). */
    function readById(id) {
        show('btnSave', false); show('btnUpdate', true);
        S.recId = id;
        return getJson(API + '/by-id?id=' + id).then(function (d) {
            var h = d.header || {};
            tab('efForm');
            setVal(ID.customer, h.ConsigneeId); setVal(ID.bank, h.IssuingBankId);
            setText(ID.formNo, h.EFormNo); setText(ID.formDate, h.EFormDate || today()); setVal(ID.delivery, h.DeliveryTermId);
            if (FI) { setVal('cmbpaymentmode', h.PaymenttermId); setComboText('cmbTradeType', h.TradeType); }
            setVal(ID.currency, h.FcurrencyId); setText(ID.valueFc, String(h.EFormValueTotal));
            setText('txtExchangeRate', String(h.ExchangeRate)); setText('txtAmountRs', String(h.AmountRs));
            if (FI) {
                /* eFormRegistration.ConsigneeIban.ToString() throws on a NULL column - the rest of ReadById is skipped */
                if (h.ConsigneeIban === null || h.ConsigneeIban === undefined) { box('Object reference not set to an instance of an object.'); return; }
                setText('txtconsigneeIban', h.ConsigneeIban); setText('txtIBAN', h.IbanNo || '');
            }
            setText('txtNetWeightkg', String(h.NetWeightTotal));
            setVal(ID.country, h.CountryId); setVal('cmbLoadingPort', h.LoadingPortId); setVal(ID.destPort, h.DestinationPortId);
            S.com = d.commodities || []; S.pay = d.paymentTerms || []; S.util = d.utilization || [];
            S.comBound = S.payBound = S.utilBound = true;
            S.comIdx = S.payIdx = S.utilIdx = -1;
            renderAll(); focus('cmbPaymentTerm');
            S.updateMode = true;
            return customerLeave();
        }).catch(function (e) { box(e.message); });
    }
    function headerPayload() {
        var h = {
            customerText: comboText(ID.customer), bankText: comboText(ID.bank), deliveryTermText: comboText(ID.delivery), valueFc: val(ID.valueFc),
            consigneeId: netI(val(ID.customer)), bankId: netI(val(ID.bank)), eFormNo: val(ID.formNo), eFormDate: val(ID.formDate),
            deliveryTermId: netI(val(ID.delivery)), currencyId: netI(val(ID.currency)), countryId: netI(val(ID.country)),
            destinationPortId: netI(val(ID.destPort)), loadingPortId: netI(val('cmbLoadingPort')),
            exchangeRate: val('txtExchangeRate'), amountRs: val('txtAmountRs'), netWeightKg: val('txtNetWeightkg')
        };
        if (FI) {
            h.ibanNo = val('txtIBAN'); h.tradeTypeText = comboText('cmbTradeType'); h.paymentModeId = netI(val('cmbpaymentmode'));
            h.status = $id('RBIsActive').checked; h.consigneeIban = val('txtconsigneeIban'); h.consigneeAddress = val('txtconsigneeAddress');
            h.balanceAmount = val('txtBalnaceAmt'); h.expiryDate = val('txtexpirydate');
        }
        return h;
    }
    function focusForMessage(m) {
        var map = { 'Customer Field Required': ID.customer, 'Issue Bank Field Required': ID.bank, 'Delivery Term Field Required': ID.delivery, 'ValueFc Field Required': ID.valueFc };
        if (map[m]) focus(map[m]);
    }
    /** btnSave_Click (btnUpdate_Click calls it too). */
    function btnSave(btn) {
        return busy(btn, function () {
            var body = {
                recId: S.recId, updateMode: S.updateMode, header: headerPayload(),
                commodities: S.com, paymentTerms: S.pay, utilization: S.util,
                paymentGridBound: S.payBound, utilizationGridBound: S.utilBound, advanceUtilizationGrid: S.advanceUtilizationGrid
            };
            return postJson(API + '/save', body).then(function (d) {
                S.advanceUtilizationGrid = (d && d.advanceUtilizationGrid) || S.advanceUtilizationGrid;
                if (d && d.success) { box(d.message); return formReset(); }
            }).catch(function (e) {
                /* AdvanceUtilizationGrid is set inside the payment loop before these two messages */
                if (/Please Utilize Advance Amount|Your Payment Term Is Advance/.test(e.message) && S.pay.some(function (r) { return netI(r.PaymentTermId) === 1; })) S.advanceUtilizationGrid = 1;
                box(e.message); focusForMessage(e.message);
            });
        });
    }
    function btnUpdate(btn) { return btnSave(btn); }

    // ------------------------------------------------------------------------------ history
    var HIDE = { Id: 1, RecordNo: 1 };
    function historyFill() {
        return getJson(API + '/history').then(function (rows) {
            HIST = rows || [];
            var cols = HIST.length ? Object.keys(HIST[0]).filter(function (k) { return !HIDE[k]; }) : [];
            $id('histHead').innerHTML = '<tr>' + cols.map(function (c) { return '<th data-col="' + esc(c) + '">' + esc(c) + '</th>'; }).join('') + '</tr>';
            var codeCol = cols.indexOf('FIUniqueNumber') >= 0 ? 'FIUniqueNumber' : cols[0];
            $id('histBody').innerHTML = HIST.map(function (r, i) {
                return '<tr data-i="' + i + '">' + cols.map(function (c) {
                    var v = r[c];
                    if (typeof v === 'string' && /^\d{4}-\d{2}-\d{2}T/.test(v)) v = v.replace('T', ' ').substring(0, 16);
                    if (c === codeCol) return '<td><a class="win-code" href="javascript:void(0)" data-i="' + i + '">' + esc(str(v)) + '</a></td>';
                    if (typeof v === 'number') return '<td class="num">' + esc(fmt(v)) + '</td>';
                    if (typeof v === 'boolean') return '<td class="ctr"><input type="checkbox" disabled' + (v ? ' checked' : '') + '/></td>';
                    return '<td>' + esc(str(v)) + '</td>';
                }).join('') + '</tr>';
            }).join('');
            show('histEmpty', HIST.length === 0);
        }).catch(function (e) { box(e.message); });
    }
    function historyOpen(i) { var r = HIST[i]; if (r) readById(netI(r.Id)); }

    // ------------------------------------------------------------------------------ wiring
    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }
    function gridDbl(bodyId, fn) {
        var b = $id(bodyId);
        b.addEventListener('click', function (e) { var tr = e.target.closest('tr[data-i]'); if (!tr) return; b.querySelectorAll('tr').forEach(function (x) { x.classList.toggle('is-current', x === tr); }); });
        b.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) fn(+tr.getAttribute('data-i')); });
    }
    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.win-tabs .win-tab').forEach(function (b) { b.addEventListener('click', function () { tab(b.getAttribute('data-tab')); }); });
        document.querySelectorAll('button[data-fullscreen]').forEach(function (b) { b.addEventListener('click', function () { var bx = $id(b.getAttribute('data-fullscreen')); if (bx) bx.classList.toggle('ex-fullscreen'); }); });
        on(ID.customer, 'change', customerLeave);
        on('cmbBankReerence', 'change', bankRefLeave);
        on('txtExchangeRate', 'input', exchangeRateChanged);
        on('txtPaymentPercent', 'input', checkPaymentAmount);
        on('txtFcAmountPayment', 'input', checkPaymentAmount);
        on('txtNetweightDetail', 'input', checkCommodity);
        on('txtFCAmount', 'input', checkCommodity);
        on('txtThisUtilize', 'input', checkUtilized);
        on('txtThisUtilize', 'blur', checkUtilized);
        gridDbl('comBody', comDblClick);
        gridDbl('payBody', payDblClick);
        gridDbl('utilBody', utilDblClick);
        var hb = $id('histBody');
        hb.addEventListener('click', function (e) { var a = e.target.closest('a.win-code'); if (a) historyOpen(+a.getAttribute('data-i')); });
        hb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) historyOpen(+tr.getAttribute('data-i')); });
        /* Form_KeyDown */
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox') {
                var f = Array.prototype.filter.call(document.querySelectorAll('input:not([type=hidden]):not(:disabled), select:not(:disabled), button:not(:disabled), textarea:not(:disabled)'), function (x) { return x.offsetParent !== null; });
                var i = f.indexOf(e.target); if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); }
                return;
            }
            if (e.ctrlKey && k === 's' && !onHistory() && !S.updateMode) { e.preventDefault(); btnSave($id('btnSave')); }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew(); }
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); cancel(); }
            if (e.ctrlKey && k === 'u' && S.updateMode) { e.preventDefault(); btnUpdate($id('btnUpdate')); }
        });
        load();
    });

    global.ExportEform = {
        btnNew: btnNew, btnSave: btnSave, btnUpdate: btnUpdate, toggleHistory: toggleHistory,
        btnCommodityAdd: btnCommodityAdd, btnUpdateComodity: btnUpdateComodity, btnCancelComodity: formCommodityReset,
        btnAddPayment: btnAddPayment, btnUpdatePayment: btnUpdatePayment, btnCancelPayment: formPayment,
        btnAddPaymentUtilization: btnAddPaymentUtilization, btnUpdatePaymentUtilization: btnUpdatePaymentUtilization, btnCancelPaymentUtilization: formPaymentUtilization,
        attachment: function () { box('Attachments are not available on the web page for this form.'); },
        historyNew: function () { /* toolStripButton1 "New" on the History strip has no Click handler on the desktop */ },
        historyPrint: function () { /* toolStripButton6 "Print" on the History strip has no Click handler on the desktop */ }
    };
}(window));
