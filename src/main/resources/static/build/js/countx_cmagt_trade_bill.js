/* ============================================================================================
 * Commission Agent Trade Bill Against GDN (DocumentTypeId 1056)
 * Desktop: Architecture.WinApp.Cmagt/frmCommissionAgentTradeBillAgainstGdn.cs (+ TradeBill_Helper,
 * frmPendingGdnLoader). Every function below names the desktop method it ports.
 *
 * The page keeps the desktop's DataTables as arrays of rows keyed by the desktop column names
 * (TradeBill_Helper.Initialize*Table), recalculates with the desktop formulas on every
 * CellUpdated / TextChanged equivalent, and posts the control values + grids to
 * /api/commission/trade-bill-against-gdn/save, where Insert(), MakeVoucher and DAL SetData run.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/commission/trade-bill-against-gdn';
    var DD = '/api/commission/dropdowns';

    /* ------------------------------------------------------------------ state */
    var S = {
        recId: 0,
        rows: [], removed: [], ds: null,
        pOther: [], sOther: [], pPm: [], sPm: [],
        pComm: [], sComm: [], pBrk: [], sBrk: [], pPay: [], sPay: [],
        showFreight: true, showFreightLess: false,
        L: { parties: [], partyById: {}, partyByGl: {}, payTerms: [], delTerms: [], otherItems: [],
             ebTerms: [], packTypes: [], baseDate: [], commType: [], commUom: [], pmPurchase: [], pmSale: [],
             acc: { trading: [], tax: [], whtPurchase: [], whtSale: [], purchasePm: [], salePm: [], commissionDr: [] },
             taxTypes: [], saleTaxTypes: [], partiesNick: [], items: [] },
        cfg: {}, dec: 2
    };
    var loaderState = { sets: null, master: [], selectedId: 0 };
    /* TaxTypeDbCallByItemId builds Id, TaxType, TaxPrcnt and TaxTypeBind binds AllColumns (Id hidden by the combo). */
    if (window.DesktopCombo && window.DesktopCombo.define) {
        window.DesktopCombo.define('taxType1056', [{ caption: 'Tax Name', flex: 3 }, { caption: 'TaxPrcnt', flex: 1, key: 'tax-percent', type: 'num' }]);
    }

    /* ------------------------------------------------------------------ small helpers */
    function $id(id) { return document.getElementById(id); }
    function esc(v) {
        return String(v === undefined || v === null ? '' : v).replace(/[&<>"']/g, function (c) {
            return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c];
        });
    }
    function pick(o, k) {
        if (!o) return undefined;
        if (k in o) return o[k];
        var lk = k.toLowerCase();
        for (var p in o) if (Object.prototype.hasOwnProperty.call(o, p) && p.toLowerCase() === lk) return o[p];
        return undefined;
    }
    /* Conversion.ToDecimal / ToDouble of a text or a cell: thousands separators allowed, "(x)" is not a number. */
    function num(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isFinite(v) ? v : 0;
        var t = String(v).replace(/,/g, '').trim();
        if (t === '' || !/^[-+]?\d*\.?\d+(e[-+]?\d+)?$/i.test(t)) return 0;
        return parseFloat(t);
    }
    function int(v) { return Math.trunc(num(v)); }
    /* Math.Round(v, d, MidpointRounding.AwayFromZero) */
    function rnd(v, d) {
        var m = Math.pow(10, d);
        return Math.sign(v) * Math.round((Math.abs(v) + Number.EPSILON) * m) / m;
    }
    function groupInt(s) { return s.replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
    /* .NET custom format "#,##0.###" style: up to maxD decimals, at least minD. */
    function fmtN(v, maxD, minD) {
        v = num(v);
        var r = rnd(v, maxD);
        var neg = r < 0; r = Math.abs(r);
        var s = r.toFixed(maxD);
        var parts = s.split('.');
        var frac = parts[1] || '';
        while (frac.length > (minD || 0) && frac.charAt(frac.length - 1) === '0') frac = frac.slice(0, -1);
        var out = groupInt(parts[0]) + (frac ? '.' + frac : '');
        return (neg && out !== '0' ? '-' : '') + out;
    }
    function f3(v) { return fmtN(v, 3, 0); }
    function f4(v) { return fmtN(v, 4, 0); }
    /* clsGlobalVariables.stringFormatboth: "#,##0.00;(0,0.00); 0" with the configured decimals. */
    function fmtBoth(v) {
        v = rnd(num(v), S.dec);
        if (v === 0) return '0';
        var a = Math.abs(v).toFixed(S.dec).split('.');
        var s = groupInt(a[0]) + (S.dec > 0 ? '.' + a[1] : '');
        return v < 0 ? '(' + s + ')' : s;
    }
    /* stringFormatsingle: "#,##0.00" */
    function fmtSingle(v) {
        v = rnd(num(v), S.dec);
        var a = Math.abs(v).toFixed(S.dec).split('.');
        return (v < 0 ? '-' : '') + groupInt(a[0]) + (S.dec > 0 ? '.' + a[1] : '');
    }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function selText(id) { var e = $id(id); if (!e || e.selectedIndex < 0) return ''; var o = e.options[e.selectedIndex]; return o && o.value !== '' && o.value !== '0' ? o.textContent : ''; }
    function ymd(d) { return d.getFullYear() + '-' + ('0' + (d.getMonth() + 1)).slice(-2) + '-' + ('0' + d.getDate()).slice(-2); }
    function d10(v) { return v ? String(v).substring(0, 10) : ''; }
    function addDays(s, n) {
        if (!s) return '';
        var p = s.split('-'); var d = new Date(+p[0], +p[1] - 1, +p[2]);
        d.setDate(d.getDate() + n); return ymd(d);
    }
    function dayDiff(a, b) {
        var pa = a.split('-'), pb = b.split('-');
        return Math.round((new Date(+pa[0], +pa[1] - 1, +pa[2]) - new Date(+pb[0], +pb[1] - 1, +pb[2])) / 86400000);
    }
    function dmy(v) {
        var s = d10(v); if (!s) return '';
        var p = s.split('-'); var m = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
        return p[2] + '-' + m[(+p[1]) - 1] + '-' + p[0].slice(2);
    }
    function sum(rows, k) { var t = 0; (rows || []).forEach(function (r) { t += num(r[k]); }); return t; }
    function sumWhere(rows, k, wk, wv) { var t = 0; (rows || []).forEach(function (r) { if (int(r[wk]) === int(wv)) t += num(r[k]); }); return t; }
    function glOf(partyId) { var p = S.L.partyById[int(partyId)]; return p ? int(p.GlAccountId) : 0; }
    function fetchJson(url, opt) {
        return fetch(url, Object.assign({ credentials: 'same-origin', headers: { 'Accept': 'application/json' } }, opt || {}))
            .then(function (r) {
                if (!r.ok) return r.text().then(function (t) { throw new Error('Server error ' + r.status + (t ? ': ' + t.substring(0, 300) : '')); });
                return r.json();
            });
    }
    function getJson(url) { return fetchJson(url).catch(function (e) { console.error(url, e); return null; }); }
    function fill(sel, list, idKey, nameKey, blank, extra) {
        var e = typeof sel === 'string' ? $id(sel) : sel; if (!e) return;
        var cur = e.value;
        var h = blank === false ? '' : '<option value="0"></option>';
        (list || []).forEach(function (d) {
            h += '<option value="' + esc(pick(d, idKey)) + '"' + (extra ? extra(d) : '') + '>' + esc(pick(d, nameKey)) + '</option>';
        });
        e.innerHTML = h;
        if (cur) e.value = cur;
        /* the DesktopCombo MutationObserver re-reads the options; no refresh call is needed */
    }
    function partyAttrs(d) {
        return ' data-code="' + esc(d.PartyCode || '') + '" data-city="' + esc(d.CityName || '') + '" data-mobile="' + esc(d.MobileNo || '') + '"';
    }
    function setSel(id, v) {
        var e = $id(id); if (!e) return;
        e.value = String(v === null || v === undefined ? 0 : v);
        if (e.selectedIndex < 0) e.value = '0';
    }
    function setSelByText(id, text) {
        var e = $id(id); if (!e) return;
        for (var i = 0; i < e.options.length; i++) if (e.options[i].textContent === text) { e.selectedIndex = i; return; }
    }
    function nameIn(list, id, idKey, nameKey) {
        for (var i = 0; i < (list || []).length; i++) if (int(pick(list[i], idKey || 'id')) === int(id)) return pick(list[i], nameKey || 'name');
        return '';
    }

    /* ------------------------------------------------------------------ busy buttons (withButton) */
    var inFlight = {};
    /* formright (SetRightsValueInRightsObject) from GET /rights; until it answers nothing is granted */
    var RIGHTS = { save: false, update: false, delete: false, print: false, canViewAllRecords: false };
    function loadRights() {
        return getJson(API + '/rights').then(function (r) { if (r) RIGHTS = Object.assign(RIGHTS, r); applyButtonState(); });
    }
    /* disables its button, shows the busy spinner, blocks a second click, restores on success or failure */
    function withButton(btnId, work) {
        if (inFlight[btnId]) return Promise.resolve();
        var b = $id(btnId);
        inFlight[btnId] = true;
        if (b) { b.disabled = true; b.classList.add('btn-busy'); }
        var done = function () {
            inFlight[btnId] = false;
            if (b) b.classList.remove('btn-busy');
            if (b && ['btnSave', 'btnUpdate', 'btnDelete'].indexOf(btnId) < 0) b.disabled = false;
            applyButtonState();
        };
        var res;
        try { res = work(); } catch (e) { done(); alert(e.message); return Promise.resolve(); }
        if (res && typeof res.then === 'function') return res.then(function (v) { done(); return v; }, function (e) { done(); alert(e && e.message ? e.message : e); });
        done(); return Promise.resolve(res);
    }
    /* ReadById: btnsave hidden, Update + Delete shown; ResetForm the reverse. */
    function applyButtonState() {
        var loaded = S.recId > 0;
        /* ResetForm (1602-1604) / ReadById (5695-5697): Visible, not Enabled, is what flips. */
        $id('btnSave').hidden = loaded; $id('btnUpdate').hidden = !loaded; $id('btnDelete').hidden = !loaded;
        /* :951-953 btnsave.Enabled = DoHaveSaveRight, btnUpdate.Enabled = DoHaveUpdateRights, BtnDelete.Enabled = DoHaveCanDelete */
        if (!inFlight.btnSave) $id('btnSave').disabled = loaded || !RIGHTS.save;
        if (!inFlight.btnUpdate) $id('btnUpdate').disabled = !loaded || !RIGHTS.update;
        if (!inFlight.btnDelete) $id('btnDelete').disabled = !loaded || !RIGHTS.delete;
    }

    /* ------------------------------------------------------------------ delivery terms (FormHelper) */
    function isPonch(id, t) { id = int(id); return id === 2 || id === 5 || id === 6 || t === 'Ponch' || t === 'Ponch & PartyWeight' || t === 'Ponch & FactoryWeight'; }
    function isLoad(id, t) { id = int(id); return id === 1 || id === 3 || id === 4 || t === 'Load' || t === 'Load & PartyWeight' || t === 'Load & FactoryWeight'; }
    function purchaseDelivered() { return isPonch(val('CmbDeliveryTermPurchase'), selText('CmbDeliveryTermPurchase')); }
    function purchaseXFactory() { return isLoad(val('CmbDeliveryTermPurchase'), selText('CmbDeliveryTermPurchase')); }
    function saleDelivered() { return isPonch(val('CmbDeliveryTermSale'), selText('CmbDeliveryTermSale')); }
    function saleXFactory() { return isLoad(val('CmbDeliveryTermSale'), selText('CmbDeliveryTermSale')); }

    /* ================================================================== CALCULATIONS */

    /* CalculateEbTotalThenBillWeight (8677) */
    function calculateEbTotalThenBillWeight() {
        S.rows.forEach(function (r) {
            var pNet = num(r.PurchaseGrossWeight) - num(r.PurchaseEbCutTotal) + num(r.PurchaseAddLessWeight);
            r.PurchaseBillWeight = pNet;
            var eq = num(r.PurchaseRateEquivalent);
            var pa = eq > 0 ? pNet / eq * num(r.PurchaseNetRate) : 0;
            pa = rnd(pa, S.dec); r.PurchaseAmount = pa;
            r.PurchaseTaxAmount = pa * num(r['PurchaseTax%']) / 100;
            var sNet = num(r.SaleGrossWeight) - num(r.SaleEbCutTotal) + num(r.SaleAddLess);
            r.SaleBillWeight = sNet;
            var seq = num(r.SaleRateEquivalent);
            var sa = seq > 0 ? sNet / seq * num(r.SaleNetRate) : 0;
            sa = rnd(sa, S.dec); r.SaleAmount = sa;
            r.SaleTaxAmount = sa * num(r['SaleTax%']) / 100;
        });
    }
    /* ApplyTax (2616) */
    function applyTax(r, amountCol, pctCol, taxCol) {
        var t = num(r[amountCol]) * num(r[pctCol]) / 100;
        t = rnd(t, S.dec); r[taxCol] = t; return t;
    }
    /* CalculateRowAmountsPurchase (2566) */
    function calcRowPurchase(r) {
        var net = num(r.PurchaseRateWithoutAddLess) + num(r.PurchaseRateAddLess);
        r.PurchaseNetRate = net;
        var w = num(r.PurchaseGrossWeight) - num(r.PurchaseEbCutTotal) + num(r.PurchaseAddLessWeight);
        r.PurchaseBillWeight = w;
        var eq = num(r.PurchaseRateEquivalent);
        var a = rnd(eq > 0 ? w / eq * net : 0, S.dec); r.PurchaseAmount = a;
        var tax = applyTax(r, 'PurchaseAmount', 'PurchaseTax%', 'PurchaseTaxAmount');
        r.PurchaseNetAmount = rnd(a + tax + num(r.PurchaseFreight) - num(r.PurchaseFreightLess) + num(r.PurchaseExpenses) + num(r.PurchaseCommission) + num(r.PurchaseBrokery), S.dec);
    }
    /* CalculateRowAmountsSale (2592) */
    function calcRowSale(r) {
        var net = num(r.SaleRateWithoutAddLess) + num(r.SaleRateAddLess);
        r.SaleNetRate = net;
        var w = num(r.SaleGrossWeight) - num(r.SaleEbCutTotal) + num(r.SaleAddLess);
        r.SaleBillWeight = w;
        var eq = num(r.SaleRateEquivalent);
        var a = rnd(eq > 0 ? w / eq * net : 0, S.dec); r.SaleAmount = a;
        var tax = applyTax(r, 'SaleAmount', 'SaleTax%', 'SaleTaxAmount');
        r.SaleNetAmount = rnd(a + tax - num(r.SaleExpenses) - num(r.SaleCommission) - num(r.SaleBrokery), S.dec);
    }
    /* ExpenseProportionForPurchase (7473) */
    function expenseProportionForPurchase() {
        var pa = sum(S.rows, 'PurchaseAmount');
        var exp = sum(S.pOther, 'Amount');
        S.rows.forEach(function (r) { r.PurchaseExpenses = pa > 0 ? Math.round(exp) / pa * num(r.PurchaseAmount) : 0; });
        billProportionForPurchase();
    }
    /* ExpenseProportionForSale (7508) */
    function expenseProportionForSale() {
        if (!S.rows.length) return;
        var sa = sum(S.rows, 'SaleAmount');
        if (sa > 0) {
            var exp = sum(S.sOther, 'Amount');
            var pack = sum(S.sPm, 'Amount');
            if (int(val('CmbTradingAccount')) > 0) pack = sumWhere(S.sPm, 'Amount', 'AccountId', val('CmbTradingAccount'));
            var tot = Math.abs(exp + pack);
            S.rows.forEach(function (r) { r.SaleExpenses = tot / sa * num(r.SaleAmount); });
        } else S.rows.forEach(function (r) { r.SaleExpenses = 0; });
        billProportionForSale();
    }
    /* BillProportionForPurchase (7555) */
    function billProportionForPurchase() {
        if (!purchaseDelivered()) {
            S.rows.forEach(function (r) { r.PurchaseNetAmount = num(r.PurchaseAmount) + num(r.PurchaseExpenses) + num(r.PurchaseFreight) + num(r.PurchaseCommission) + num(r.PurchaseBrokery); });
        } else {
            var fr = int(val('CmbTradingAccount')) > 0 ? sumWhere(S.pPm, 'Amount', 'AccountId', val('CmbTradingAccount')) : 0;
            S.rows.forEach(function (r) { r.PurchaseNetAmount = num(r.PurchaseAmount) + num(r.PurchaseExpenses) - fr + num(r.PurchaseCommission) + num(r.PurchaseBrokery); });
        }
        plAmountProportion(); calculateHeaderRates();
    }
    /* BillProportionForSale (7596) */
    function billProportionForSale() {
        S.rows.forEach(function (r) { r.SaleNetAmount = num(r.SaleAmount) + num(r.SaleExpenses) + num(r.SaleCommission) + num(r.SaleBrokery); });
        plAmountProportion(); calculateHeaderRates();
    }
    /* PLAmountProportion (7619) */
    function plAmountProportion() { S.rows.forEach(function (r) { r.PLAmount = num(r.PurchaseNetAmount) - num(r.SaleNetAmount); }); }
    /* FreightProportion (7650) */
    function freightProportion() {
        var nw = sum(S.rows, 'PurchaseBillWeight');
        var fr = sum(S.pPm, 'Amount');
        if (fr > 0) {
            if (!purchaseDelivered()) {
                S.rows.forEach(function (r) { r.PurchaseFreight = nw > 0 ? fr / nw * num(r.PurchaseBillWeight) : 0; });
                S.showFreight = true; S.showFreightLess = false;
            } else {
                fr = sumWhere(S.pPm, 'Amount', 'AccountId', val('CmbTradingAccount'));
                S.rows.forEach(function (r) { r.PurchaseFreightLess = nw > 0 ? fr / nw * num(r.PurchaseBillWeight) : 0; });
                S.showFreight = false; S.showFreightLess = true;
            }
        } else S.rows.forEach(function (r) { r.PurchaseFreight = 0; });
        billProportionForPurchase();
    }
    /* CalculateHeaderRates (7852) - GetColumnAvg */
    function calculateHeaderRates() {
        var n = S.rows.length;
        var s = n ? sum(S.rows, 'SaleNetRate') / n : 0, p = n ? sum(S.rows, 'PurchaseNetRate') / n : 0;
        setVal('txtSaleAvgNetRate', f3(s)); setVal('txtPurchaseAvgNetRate', f3(p));
        setVal('txtDiffAvgNetRate', f3(s - p));
    }

    /* ApplyCommissionOrBrokery (9594) */
    function applyCommission(grid, agentCol, amountCol, partyId, bill, isCustomer) {
        partyId = int(partyId); if (partyId <= 0) return bill;
        var pg = glOf(partyId);
        grid.forEach(function (r) {
            var ag = int(r[agentCol]), dr = int(r.DebitAccountId), a = num(r[amountCol]);
            if (isCustomer) { if (partyId === ag) bill -= a; if (dr > 0 && pg === dr) bill += a; }
            else { if (partyId === ag) bill += a; if (dr > 0 && pg === dr) bill -= a; }
        });
        return bill;
    }
    /* CustomerBillAmount (7205) */
    function customerBillAmount() {
        var cid = int(val('CmbCustomer')), cg = glOf(cid);
        var sa = sum(S.rows, 'SaleAmount'), tax = sum(S.rows, 'SaleTaxAmount');
        if (cid > 0 && S.pPm.length > 0) { var fa = sumWhere(S.pPm, 'Amount', 'AccountId', cg); if (fa !== 0) sa -= fa; }
        var ex = sum(S.sOther, 'Amount'); if (ex !== 0) sa += ex;
        sa += tax;
        var del = saleDelivered();
        if (saleXFactory()) sa += sum(S.sPm, 'Amount');
        else if (del) { var pm = sumWhere(S.sPm, 'Amount', 'AccountId', cg); if (pm !== 0) sa -= pm; }
        sa = applyCommission(S.pComm, 'CommissionAgentId', 'CommissionAmount', cid, sa, true);
        sa = applyCommission(S.sComm, 'CommissionAgentId', 'CommissionAmount', cid, sa, true);
        sa = applyCommission(S.pBrk, 'BrokeryAgentId', 'BrokeryAmount', cid, sa, true);
        sa = applyCommission(S.sBrk, 'BrokeryAgentId', 'BrokeryAmount', cid, sa, true);
        if (del) sa -= num(val('txtTotalFreightSale'));
        var wp = num(val('txtWhtTaxPercentSale')), wa = wp > 0 ? sa * wp / 100 : 0;
        setVal('txtwhtTaxAmountSale', fmtBoth(wa));
        sa -= wa;
        setBillAmount('txtSaleBillAmount', fmtBoth(sa));
        billProportionForSale();
        calculateProfitLoss();
    }
    /* SupplierBillAmount (7275) */
    function supplierBillAmount() {
        var sid = int(val('CmbSupplier')), sg = glOf(sid);
        var pa = sum(S.rows, 'PurchaseAmount') + sum(S.pOther, 'Amount') + sum(S.rows, 'PurchaseTaxAmount');
        if (sid > 0 && S.pPm.length > 0) {
            if (purchaseXFactory()) pa += sumWhere(S.pPm, 'Amount', 'AccountId', sg);
            else if (purchaseDelivered()) pa -= sum(S.pPm, 'Amount');
        }
        if (sid > 0 && S.sPm.length > 0) pa += sumWhere(S.sPm, 'Amount', 'AccountId', sg);
        pa = applyCommission(S.pComm, 'CommissionAgentId', 'CommissionAmount', sid, pa, false);
        pa = applyCommission(S.sComm, 'CommissionAgentId', 'CommissionAmount', sid, pa, false);
        pa = applyCommission(S.pBrk, 'BrokeryAgentId', 'BrokeryAmount', sid, pa, false);
        pa = applyCommission(S.sBrk, 'BrokeryAgentId', 'BrokeryAmount', sid, pa, false);
        if (purchaseDelivered()) pa -= num(val('txtTotalFreightPurchase'));
        var wp = num(val('txtWhtTaxPercentPurchase')), wa = wp > 0 ? weightForPurchaseTax() * wp : 0;
        setVal('txtWhtTaxAmountPurchase', fmtBoth(wa));
        pa -= wa;
        setBillAmount('txtPurchaseBillAmount', fmtBoth(pa));
        billProportionForPurchase();
        calculateProfitLoss();
    }
    /* txtPurchaseBillAmount_TextChanged / txtSaleBillAmount_TextChanged -> CalculatePaymentDetailGrid */
    function setBillAmount(id, text) {
        var changed = val(id) !== text;
        setVal(id, text);
        if (changed) calculatePaymentDetailGrid(id === 'txtPurchaseBillAmount');
    }
    /* GetWeightForPurchaseTaxCalculations (7342) */
    function weightForPurchaseTax() {
        var t = 0; S.rows.forEach(function (r) { var e = num(r.PurchaseRateEquivalent); if (e > 0) t += num(r.PurchaseBillWeight) / e; }); return t;
    }
    /* ApplyCommissionOrBrokeryForPL (9626) */
    function applyCommissionPL(grid, agentCol, amountCol, trading, acc) {
        grid.forEach(function (r) {
            var a = num(r[amountCol]), ag = glOf(r[agentCol]), dr = int(r.DebitAccountId);
            if (ag > 0 && trading === ag) acc.cr += a;
            if (dr === 0 || trading === dr) acc.dr += a;
        });
    }
    /* CalculateProfitLoss (7358) */
    function calculateProfitLoss() {
        var trading = int(val('CmbTradingAccount'));
        var acc = { dr: 0, cr: 0 };
        acc.dr += sum(S.rows, 'PurchaseAmount'); acc.cr += sum(S.rows, 'SaleAmount');
        acc.dr += sum(S.pOther, 'Amount');
        if (!purchaseDelivered()) acc.dr += sum(S.pPm, 'Amount');
        else acc.cr += sumWhere(S.pPm, 'Amount', 'AccountId', trading);
        acc.dr += num(val('txtTotalFreightSale')); acc.cr += num(val('txtTotalFreightPurchase'));
        acc.cr += sum(S.sOther, 'Amount');
        if (saleDelivered()) acc.dr += sum(S.sPm, 'Amount');
        else acc.cr += sumWhere(S.sPm, 'Amount', 'AccountId', trading);
        applyCommissionPL(S.pComm, 'CommissionAgentId', 'CommissionAmount', trading, acc);
        applyCommissionPL(S.sComm, 'CommissionAgentId', 'CommissionAmount', trading, acc);
        applyCommissionPL(S.pBrk, 'BrokeryAgentId', 'BrokeryAmount', trading, acc);
        applyCommissionPL(S.sBrk, 'BrokeryAgentId', 'BrokeryAmount', trading, acc);
        var pl = acc.dr - acc.cr;
        setVal('txtProfitLoss', fmtSingle(pl));
        $id('txtProfitLoss').className = pl < 0 ? 'tb-pl-pos' : (pl > 0 ? 'tb-pl-neg' : '');
    }

    /* InitCommissionConfigs (9666) - the desktop reads column "PurcahseAmount" (typo), so the
       purchase configs' TotalAmount is always 0. Reproduced. */
    var configs = null;
    function initCommissionConfigs() {
        var totP = 0; /* GetColumnSum(grd, "PurcahseAmount") -> no such column -> 0 */
        var totS = sum(S.rows, 'SaleAmount');
        configs = [
            { grid: function () { return S.pComm; }, order: 'PoId', amount: 'PurchaseAmount', weight: 'PurchaseBillWeight', target: 'PurchaseCommission', rate: 'CommissionRate', uom: 'CommissionUomId', type: 'CommissionTypeId', calc: 'CommissionAmount', total: totP },
            { grid: function () { return S.pBrk; }, order: 'PoId', amount: 'PurchaseAmount', weight: 'PurchaseBillWeight', target: 'PurchaseBrokery', rate: 'BrokeryRate', uom: 'BrokeryUomId', type: 'BrokeryTypeId', calc: 'BrokeryAmount', total: totP },
            { grid: function () { return S.sComm; }, order: 'SoId', amount: 'SaleAmount', weight: 'SaleBillWeight', target: 'SaleCommission', rate: 'CommissionRate', uom: 'CommissionUomId', type: 'CommissionTypeId', calc: 'CommissionAmount', total: totS },
            { grid: function () { return S.sBrk; }, order: 'SoId', amount: 'SaleAmount', weight: 'SaleBillWeight', target: 'SaleBrokery', rate: 'BrokeryRate', uom: 'BrokeryUomId', type: 'BrokeryTypeId', calc: 'BrokeryAmount', total: totS }
        ];
    }
    function typeText(id) { return nameIn(S.L.commType, id); }
    function uomText(id) { return nameIn(S.L.commUom, id); }
    /* CalculateCommissionGrid (9468) */
    function calculateCommissionGrid(c) {
        var g = c.grid(); if (!g.length) return;
        g.forEach(function (it) {
            var oid = int(it[c.order]), ta = 0, tw = 0;
            S.rows.forEach(function (r) { if (oid <= 0 || int(r[c.order]) === oid) { ta += num(r[c.amount]); tw += num(r[c.weight]); } });
            var rate = num(it[c.rate]), uom = num(uomText(it[c.uom])), type = typeText(it[c.type]);
            it[c.calc] = type === 'Percent' ? (ta !== 0 ? ta * rate / 100 : 0) : (type === 'Weight' ? (uom !== 0 ? tw / uom * rate : 0) : rate);
        });
    }
    /* ProportionateCommissionAndBrokery (9499) + ApplyProportionateValues (9528) */
    function proportionateCommissionAndBrokery() {
        S.rows.forEach(function (r) { r.PurchaseCommission = 0; r.PurchaseBrokery = 0; r.SaleCommission = 0; r.SaleBrokery = 0; });
        if (!configs) return;
        configs.forEach(function (c) {
            c.grid().forEach(function (it) {
                var oid = int(it[c.order]), rate = num(it[c.rate]), uom = num(uomText(it[c.uom])), type = typeText(it[c.type]);
                var orderTotal = oid > 0 ? sumWhere(S.rows, c.amount, c.order, oid) : c.total;
                S.rows.forEach(function (d) {
                    if (oid <= 0 || int(d[c.order]) === oid) {
                        var a = num(d[c.amount]), w = num(d[c.weight]);
                        var v = type === 'Percent' ? a * rate / 100 : (type === 'Weight' ? (uom !== 0 ? w / uom * rate : 0) : (orderTotal !== 0 ? rate / orderTotal * a : 0));
                        d[c.target] = num(d[c.target]) + v;
                    }
                });
            });
        });
    }
    /* EnforcePercentCap (9430) */
    function enforcePercentCap(c, it) {
        if (typeText(it[c.type]) !== 'Percent') return;
        var oid = int(it[c.order]), rate = num(it[c.rate]);
        if (oid > 0) {
            var tot = 0;
            c.grid().forEach(function (x) { if (typeText(x[c.type]) === 'Percent' && int(x[c.order]) === oid) tot += num(x[c.rate]); });
            if (tot > 100) { it[c.rate] = rate - (tot - 100); alert('Total Percentage for Order #' + oid + ' cannot be greater than 100.'); }
        } else if (rate > 100) { it[c.rate] = 100; alert('Percentage cannot be greater than 100.'); }
    }

    /* CalculatePaymentDetailGrid (9186) */
    function calculatePaymentDetailGrid(isPurchase) {
        var grid = isPurchase ? S.pPay : S.sPay;
        if (!grid.length) return;
        var orderCol = isPurchase ? 'PoId' : 'SoId', amountCol = isPurchase ? 'PurchaseAmount' : 'SaleAmount';
        var partyId = int(val(isPurchase ? 'CmbSupplier' : 'CmbCustomer')), pg = glOf(partyId);
        var totals = {}, grand = 0;
        S.rows.forEach(function (r) { var k = int(r[orderCol]); totals[k] = (totals[k] || 0) + num(r[amountCol]); grand += num(r[amountCol]); });
        var adj = 0;
        if (isPurchase) {
            var del = purchaseDelivered();
            adj += sum(S.pOther, 'Amount') + sum(S.rows, 'PurchaseTaxAmount');
            if (partyId > 0 && S.pPm.length > 0) { if (!del) adj += sumWhere(S.pPm, 'Amount', 'AccountId', pg); else adj -= sum(S.pPm, 'Amount'); }
            if (partyId > 0 && S.sPm.length > 0) adj += sumWhere(S.sPm, 'Amount', 'AccountId', pg);
            if (del) adj -= num(val('txtTotalFreightPurchase'));
            adj -= num(val('txtWhtTaxAmountPurchase'));
        } else {
            var load = saleXFactory(), sdel = saleDelivered();
            if (partyId > 0 && S.pPm.length > 0) { var f = sumWhere(S.pPm, 'Amount', 'AccountId', pg); if (f !== 0) adj -= f; }
            var ex = sum(S.sOther, 'Amount'); if (ex !== 0) adj += ex;
            if (load) adj += sum(S.sPm, 'Amount');
            else if (sdel && partyId > 0) { var pm = sumWhere(S.sPm, 'Amount', 'AccountId', pg); if (pm !== 0) adj -= pm; }
            adj += sum(S.rows, 'SaleTaxAmount');
            if (sdel) adj -= num(val('txtTotalFreightSale'));
            adj -= num(val('txtwhtTaxAmountSale'));
        }
        var spec = {}, noOrder = 0;
        [[S.pComm, 'CommissionAgentId', 'CommissionAmount', 'PoId'], [S.pBrk, 'BrokeryAgentId', 'BrokeryAmount', 'PoId'],
         [S.sComm, 'CommissionAgentId', 'CommissionAmount', 'SoId'], [S.sBrk, 'BrokeryAgentId', 'BrokeryAmount', 'SoId']].forEach(function (t) {
            if (partyId <= 0) return;
            var hasOrderCol = t[3] === orderCol;
            t[0].forEach(function (r) {
                var ag = int(r[t[1]]), dr = int(r.DebitAccountId), a = num(r[t[2]]);
                if (a <= 0) return;
                var c = 0;
                if (!isPurchase) { if (partyId === ag) c -= a; if (dr > 0 && pg === dr) c += a; }
                else { if (partyId === ag) c += a; if (dr > 0 && pg === dr) c -= a; }
                if (c === 0) return;
                var oid = hasOrderCol ? int(r[orderCol]) : 0;
                if (oid > 0) spec[oid] = (spec[oid] || 0) + c; else noOrder += c;
            });
        });
        grid.forEach(function (it) {
            var oid = int(it[orderCol]);
            var base = oid <= 0 ? grand : (totals[oid] || 0);
            var total = grand !== 0 ? rnd((grand + adj) * base / grand, 4) : base + adj;
            if (oid > 0 && spec[oid] !== undefined) total += spec[oid];
            if (grand !== 0 && noOrder !== 0) total += rnd(noOrder * base / grand, 4);
            else if (grand === 0) total += noOrder;
            var pct = num(it['%OfTotal']);
            it.Amount = total !== 0 ? rnd(total * pct / 100, 4) : 0;
        });
    }

    /* the desktop's recalculation cascades */
    function recalcPurchase(r) {
        calcRowPurchase(r); expenseProportionForPurchase(); freightProportion();
        initCommissionConfigs(); calculateCommissionGrid(configs[0]); calculateCommissionGrid(configs[1]);
        proportionateCommissionAndBrokery(); supplierBillAmount(); plAmountProportion(); calculateHeaderRates();
    }
    function recalcSale(r) {
        calcRowSale(r); expenseProportionForSale();
        initCommissionConfigs(); calculateCommissionGrid(configs[2]); calculateCommissionGrid(configs[3]);
        proportionateCommissionAndBrokery(); customerBillAmount(); plAmountProportion(); calculateHeaderRates();
    }
    /* CmbDeliveryTerms_TextChanged (7454) / CmbDeliveryTermSale_TextChanged (7434) */
    function deliveryTermChanged(isSale) {
        freightHandling(num(val('txtFreightAmountPurchase')), num(val('txtFreightAmountSale')));
        customerBillAmount(); supplierBillAmount();
        expenseProportionForPurchase(); freightProportion(); expenseProportionForSale();
        if (isSale) billProportionForSale();
        plAmountProportion(); calculateHeaderRates();
        renderAll();
    }
    /* txtTotalFreightPurchase_TextChanged / txtTotalFreightSale_TextChanged */
    function totalFreightChanged() {
        customerBillAmount(); supplierBillAmount();
        expenseProportionForPurchase(); freightProportion(); expenseProportionForSale(); plAmountProportion();
    }
    /* CalculateTotalPurchaseFreight / CalculateTotalSaleFreight (7706/7733) */
    function calculateTotalFreight(side) {
        var p = side === 'P';
        var t = num(val(p ? 'txtFreightAmountPurchase' : 'txtFreightAmountSale')) + num(val(p ? 'txtFreightAddLessPurchase' : 'txtFreightAddLessSale'));
        var id = p ? 'txtTotalFreightPurchase' : 'txtTotalFreightSale';
        var txt = f3(t);
        if (val(id) !== txt) { setVal(id, txt); totalFreightChanged(); }
    }
    /* FreightHandling (8622) */
    function freightHandling(pf, sf) {
        var pd = purchaseDelivered(), sd = saleDelivered();
        setVal('txtFreightAmountPurchase', '0'); setVal('txtFreightAmountSale', '0');
        if (pd && sd) { setVal('txtFreightAmountPurchase', f3(pf)); setVal('txtFreightAmountSale', f3(sf)); }
        else if (pd) setVal('txtFreightAmountPurchase', f3(pf));
        else if (sd) setVal('txtFreightAmountSale', f3(sf));
        calculateTotalFreight('P'); calculateTotalFreight('S');
    }
    /* weight differences (6498-6540) and FormHelper.SetDifference(txtBillWeightGrn, txtBillWeightGdn, txtBillWeightDiff) */
    function weights() {
        setVal('txtFirstWtDifference', f3(Math.abs(num(val('txtBuyerFirstWeight')) - num(val('txtSupplierFirstWeight')))));
        setVal('txtSecondWtDifference', f3(Math.abs(num(val('txtBuyerSecondWeight')) - num(val('txtSupplierSecondWeight')))));
        setVal('txtNetWeightDifference', f3(Math.abs(num(val('txtBuyerWeight')) - num(val('txtSupplierWeight')))));
        setVal('txtBillWeightDiff', f3(num(val('txtBillWeightGrn')) - num(val('txtBillWeightGdn'))));
    }
    /* CalculateSupplierNetWeight / CalculateBuyerNetWeight (6468/6483) */
    function netFromFirstSecond(side) {
        var p = side === 'P';
        var n = Math.abs(num(val(p ? 'txtSupplierFirstWeight' : 'txtBuyerFirstWeight')) - num(val(p ? 'txtSupplierSecondWeight' : 'txtBuyerSecondWeight')));
        setVal(p ? 'txtSupplierWeight' : 'txtBuyerWeight', f3(n));
        weights();
    }

    /* ================================================================== GRIDS */

    function hasGdn() { return S.rows.some(function (r) { return int(r.GdnId) > 0; }); }
    function anyPo() { return S.rows.some(function (r) { return int(r.PoId) > 0; }); }
    function anySo() { return S.rows.some(function (r) { return int(r.SoId) > 0; }); }

    /* TradeBill_Helper.DetailGridCommonSetting + grdSettings (2360): hidden ids, edit types. */
    function detailCols() {
        var g = hasGdn();
        var editSaleAddLess = !g || S.cfg.enableSaleAddLess;
        var c = [];
        if (!g) c.push({ btn: 'Delete', cap: '', label: 'X' });
        c.push({ k: 'GdnNo', cap: 'GdnNo', hide: !g }, { k: 'GdnDate', cap: 'GdnDate', date: 1, hide: !g },
            { k: 'GrnNo', cap: 'GrnNo', hide: !g }, { k: 'GrnDate', cap: 'GrnDate', date: 1, hide: !g },
            { k: 'PoNo', cap: 'PoNo', hide: !g }, { k: 'PoDate', cap: 'PoDate', date: 1, hide: !g },
            { k: 'SoNo', cap: 'SoNo', hide: !g }, { k: 'SoDate', cap: 'SoDate', date: 1, hide: !g },
            { k: 'ItemCode', cap: 'ItemCode' }, { k: 'ItemName', cap: 'ItemName' }, { k: 'CropYear', cap: 'CropYear' },
            { k: 'PackTypeDesc', cap: 'Packing Type' }, { k: 'UomCode', cap: 'UomCode' },
            { k: 'ItemQty', cap: 'ItemQty', n: 3, sum: 1 },
            { k: 'RefDocNo', cap: 'RefDocNo', edit: g },          /* grdSettings :2389 - only on a GDN bill */ { k: 'VehicleNo', cap: 'VehicleNo' }, { k: 'BiltyNo', cap: 'BiltyNo' },
            { k: 'PurchaseGrossWeight', cap: 'PurchaseGrossWeight', n: 3, sum: 1 }, { k: 'PurchaseEbCut', cap: 'PurchaseEbCut', n: 3 },
            { k: 'PurchaseEbCutTotal', cap: 'PurchaseEbCutTotal', n: 3, sum: 1 },
            { k: 'PurchaseAddLessWeight', cap: 'PurchaseAddLessWeight', n: 3, edit: true, sum: 1 },
            { k: 'PurchaseBillWeight', cap: 'PurchaseBillWeight', n: 3, sum: 1 },
            { k: 'PurchaseRateWithoutAddLess', cap: 'PurchaseRateWithoutAddLess', n: 4, edit: g && !anyPo() },
            { k: 'PurchaseRateAddLess', cap: 'PurchaseRateAddLess', n: 4, edit: true },
            { k: 'PurchaseNetRate', cap: 'PurchaseNetRate', n: 4 }, { k: 'PurchaseRateUom', cap: 'PurchaseRateUom' },
            { k: 'PurchaseAmount', cap: 'PurchaseAmount', n: S.dec, sum: 1 },
            { k: 'PurchaseTaxName', cap: 'PurchaseTaxName' }, { k: 'PurchaseTax%', cap: 'PurchaseTax%', n: 4 },
            { k: 'PurchaseTaxAmount', cap: 'PurchaseTaxAmount', n: S.dec, sum: 1 },
            { k: 'PurchaseFreight', cap: 'Purchase Freight Add', n: S.dec, sum: 1, hide: !S.showFreight },
            { k: 'PurchaseFreightLess', cap: 'PurchaseFreightLess', n: S.dec, sum: 1, hide: !S.showFreightLess },
            { k: 'PurchaseExpenses', cap: 'PurchaseExpenses', n: S.dec, sum: 1 },
            { k: 'PurchaseCommission', cap: 'PurchaseCommission', n: S.dec, sum: 1 },
            { k: 'PurchaseBrokery', cap: 'PurchaseBrokery', n: S.dec, sum: 1 },
            { k: 'PurchaseNetAmount', cap: 'PurchaseNetAmount', n: S.dec, sum: 1 },
            { k: 'SaleGrossWeight', cap: 'SaleGrossWeight', n: 3, sum: 1 }, { k: 'SaleEbCut', cap: 'SaleEbCut', n: 3 },
            { k: 'SaleEbCutTotal', cap: 'SaleEbCutTotal', n: 3, sum: 1 },
            { k: 'SaleAddLess', cap: 'SaleAddLess', n: 3, edit: editSaleAddLess, sum: 1 },
            { k: 'SaleBillWeight', cap: 'SaleBillWeight', n: 3, sum: 1 },
            { k: 'SaleRateWithoutAddLess', cap: 'SaleRateWithoutAddLess', n: 4, edit: g && !anySo() },
            { k: 'SaleRateAddLess', cap: 'SaleRateAddLess', n: 4, edit: true },
            { k: 'SaleNetRate', cap: 'SaleNetRate', n: 4 }, { k: 'SaleRateUom', cap: 'SaleRateUom' },
            { k: 'SaleAmount', cap: 'SaleAmount', n: S.dec, sum: 1 },
            { k: 'SaleTaxName', cap: 'SaleTaxName' }, { k: 'SaleTax%', cap: 'SaleTax%', n: 4 },
            { k: 'SaleTaxAmount', cap: 'SaleTaxAmount', n: S.dec, sum: 1 },
            { k: 'SaleExpenses', cap: 'SaleExpenses', n: S.dec, sum: 1 },
            { k: 'SaleCommission', cap: 'SaleCommission', n: S.dec, sum: 1 },
            { k: 'SaleBrokery', cap: 'SaleBrokery', n: S.dec, sum: 1 },
            { k: 'SaleNetAmount', cap: 'SaleNetAmount', n: S.dec, sum: 1 },
            { k: 'RemarksDetail', cap: 'RemarksDetail', edit: g });
        return c;
    }

    function cellText(c, r) {
        var v = r[c.k];
        if (c.combo) return esc(nameIn(c.combo(), v, c.idKey, c.nameKey));
        if (c.date) return esc(dmy(v));
        if (c.n !== undefined) return (v === null || v === undefined || v === '') ? '' : esc(fmtN(v, c.n, 0));
        return esc(v);
    }
    /* Generic GridEX: columns, editable cells, buttons, totals. */
    function renderGrid(tableId, cols, rows) {
        var t = $id(tableId); if (!t) return;
        cols = cols.filter(function (c) { return !c.hide; });
        var h = '<thead><tr>' + cols.map(function (c) { return '<th' + (c.n !== undefined ? ' class="num"' : '') + '>' + esc(c.cap) + '</th>'; }).join('') + '</tr></thead><tbody>';
        rows.forEach(function (r, i) {
            h += '<tr>';
            cols.forEach(function (c) {
                if (c.btn) { h += '<td><button type="button" data-g="' + tableId + '" data-btn="' + c.btn + '" data-r="' + i + '">' + esc(c.label) + '</button></td>'; return; }
                var editable = typeof c.edit === 'function' ? c.edit(r) : c.edit;
                var attrs = ' data-g="' + tableId + '" data-k="' + esc(c.k) + '" data-r="' + i + '"';
                if (editable && c.combo) {
                    var opts = '<option value="0"></option>' + c.combo().map(function (o) {
                        var id = pick(o, c.idKey || 'id');
                        return '<option value="' + esc(id) + '"' + (int(id) === int(r[c.k]) ? ' selected' : '') + '>' + esc(pick(o, c.nameKey || 'name')) + '</option>';
                    }).join('');
                    h += '<td><select' + attrs + ' aria-label="' + esc(c.cap) + '">' + opts + '</select></td>';
                } else if (editable) {
                    var v = r[c.k];
                    if (c.date) h += '<td><input type="date"' + attrs + ' aria-label="' + esc(c.cap) + '" value="' + esc(d10(v)) + '"></td>';
                    else {
                        var shown = (v === null || v === undefined) ? '' : (c.n !== undefined && v !== '' ? fmtN(v, Math.max(c.n, 4), 0).replace(/,/g, '') : v);
                        h += '<td' + (c.n !== undefined ? ' class="num"' : '') + '><input' + attrs + ' aria-label="' + esc(c.cap) + '" value="' + esc(shown) + '"' + (c.n !== undefined ? ' inputmode="decimal" style="text-align:right"' : '') + '></td>';
                    }
                } else {
                    h += '<td' + (c.n !== undefined ? ' class="num"' : '') + '>' + (c.link ? '<button type="button" class="tb-link" data-g="' + tableId + '" data-btn="' + c.link + '" data-r="' + i + '">' + cellText(c, r) + '</button>' : cellText(c, r)) + '</td>';
                }
            });
            h += '</tr>';
        });
        h += '</tbody>';
        if (cols.some(function (c) { return c.sum; })) {
            h += '<tfoot><tr>' + cols.map(function (c) { return '<td' + (c.n !== undefined ? ' class="num"' : '') + '>' + (c.sum ? esc(fmtN(sum(rows, c.k), c.n || 3, 0)) : '') + '</td>'; }).join('') + '</tr></tfoot>';
        }
        t.innerHTML = h;
    }

    function orderless(k) { return function (r) { return int(r[k]) <= 0; }; }
    /* TradeBill_Helper.PurchaseExpenseGridCommonSetting / SaleExpenseGridCommonSetting */
    function expenseCols(side) {
        var docK = side === 'P' ? 'GrnNo' : 'GdnNo', idK = side === 'P' ? 'GrnId' : 'GdnId';
        var rows = side === 'P' ? S.pOther : S.sOther;
        return [{ k: docK, cap: docK, hide: !rows.some(function (r) { return int(r[idK]) > 0; }) }, { k: 'ItemName', cap: 'ItemName' },
            { k: '%OfTotal', cap: '%OfTotal', n: 4, edit: true }, { k: 'Qty', cap: 'Qty', n: 3, edit: true }, { k: 'Rate', cap: 'Rate', n: 4, edit: true },
            { k: 'Amount', cap: 'Amount', n: 3, edit: true, sum: 1 }, { k: 'Remarks', cap: 'Remarks', edit: true }];
    }
    /* FreightGridCommonSetting / PackingMaterialGridCommonSetting (+ ComboBind) */
    function pmCols(side) {
        var docK = side === 'P' ? 'GrnNo' : 'GdnNo', idK = side === 'P' ? 'GrnId' : 'GdnId';
        var rows = side === 'P' ? S.pPm : S.sPm;
        return [{ btn: 'Add', label: '+' }, { btn: 'Delete', label: 'X' },
            { k: docK, cap: docK, hide: !rows.some(function (r) { return int(r[idK]) > 0; }) },
            { k: 'AccountId', cap: 'Account', edit: true, combo: function () { return side === 'P' ? S.L.acc.purchasePm : S.L.acc.salePm; }, idKey: 'Id', nameKey: 'AccountTitle' },
            { k: 'PackingTypeId', cap: 'Packing Type', edit: true, combo: function () { return S.L.packTypes; } },
            { k: 'PmItemId', cap: 'Empty Bag Item', edit: true, combo: function () { return side === 'P' ? S.L.pmPurchase : S.L.pmSale; } },
            { k: 'Qty', cap: 'Qty', n: 3, edit: true }, { k: 'Rate', cap: 'Rate', n: 4, edit: true },
            { k: 'Amount', cap: 'Credit', n: 3, edit: true, sum: 1 }, { k: 'Remarks', cap: 'Remarks', edit: true },
            { k: 'EmptyBagTerm', cap: 'EmptyBagTerm' }];
    }
    /* *CommissionGridCommonSetting / *BrokeryGridCommonSetting + PurchaseSaleCommissionGridsColumnsEdit */
    function commCols(orderK, docK, agentK, typeK, uomK, rateK, amtK) {
        var e = orderless(orderK), brk = agentK.indexOf('Brokery') === 0;
        return [{ btn: 'Add', label: '+' }, { btn: 'Delete', label: 'X' }, { k: docK, cap: docK },
            { k: agentK, cap: brk ? 'Broker' : 'Commission Agent', edit: e, combo: function () { return S.L.parties; }, idKey: 'Id', nameKey: 'CompanyName' },
            { k: typeK, cap: brk ? 'Brokery Type' : 'Commission Type', edit: e, combo: function () { return S.L.commType; } },
            { k: uomK, cap: brk ? 'Brokery Uom' : 'Commission Uom', edit: e, combo: function () { return S.L.commUom; } },
            { k: rateK, cap: 'Rate', n: 4, edit: e },
            { k: amtK, cap: 'Amount', n: 3, sum: 1 },
            { k: 'DebitAccountId', cap: brk ? 'Brokery Dr Account' : 'Commission Dr Account', edit: true, combo: function () { return S.L.acc.commissionDr; }, idKey: 'Id', nameKey: 'AccountTitle' }];
    }
    /* Purchase/SalePaymentGridCommonSetting */
    function payCols(side) {
        var docK = side === 'P' ? 'PoNo' : 'SoNo';
        return [{ btn: 'Add', label: '+' }, { btn: 'Delete', label: 'X' }, { k: docK, cap: docK },
            { k: 'PaymentTermId', cap: 'Payment Term', edit: true, combo: function () { return S.L.payTerms; } },
            { k: 'DueDays', cap: 'DueDays', n: 0, edit: true }, { k: '%OfTotal', cap: '%OfTotal', n: 4, edit: true },
            { k: 'Amount', cap: 'Amount', n: 3, sum: 1 },
            { k: 'BaseDateTypeId', cap: 'Base Date Type', edit: true, combo: function () { return S.L.baseDate; } },
            { k: 'DueDate', cap: 'DueDate', date: 1, edit: true }];
    }

    function renderDetail() {
        renderGrid('grd', detailCols(), S.rows);
        var p = $id('PanelDetailInfo'); if (p) p.hidden = hasGdn();               /* grdSettings :2386 */
    }
    function renderChildren() {
        renderGrid('grdPurchaseOtherExpenses', expenseCols('P'), S.pOther);
        renderGrid('grdSaleOtherExpense', expenseCols('S'), S.sOther);
        renderGrid('grdPurchasePmExpense', pmCols('P'), S.pPm);
        renderGrid('grdSalePmExpenseSale', pmCols('S'), S.sPm);
        renderGrid('grdPurchaseCommission', commCols('PoId', 'PoNo', 'CommissionAgentId', 'CommissionTypeId', 'CommissionUomId', 'CommissionRate', 'CommissionAmount'), S.pComm);
        renderGrid('grdSaleCommission', commCols('SoId', 'SoNo', 'CommissionAgentId', 'CommissionTypeId', 'CommissionUomId', 'CommissionRate', 'CommissionAmount'), S.sComm);
        renderGrid('grdPurchaseBrokery', commCols('PoId', 'PoNo', 'BrokeryAgentId', 'BrokeryTypeId', 'BrokeryUomId', 'BrokeryRate', 'BrokeryAmount'), S.pBrk);
        renderGrid('grdSaleBrokery', commCols('SoId', 'SoNo', 'BrokeryAgentId', 'BrokeryTypeId', 'BrokeryUomId', 'BrokeryRate', 'BrokeryAmount'), S.sBrk);
        renderGrid('grdPaymentDetailPurchase', payCols('P'), S.pPay);
        renderGrid('grdPaymentDetailSale', payCols('S'), S.sPay);
    }
    function renderAll() { renderDetail(); renderChildren(); weights(); }

    /* ------------------------------------------------------------------ default rows (AddRow*) */
    function otherExpenseRows() {
        return S.L.otherItems.map(function (o) {
            return { GrnId: null, GrnNo: null, GdnId: null, GdnNo: null, ItemId: int(o.id), ItemName: o.name, '%OfTotal': null, Qty: null, Rate: null, Amount: null, Remarks: null };
        });
    }
    function emptyPm(side) {
        var r = { AccountId: null, PackingTypeId: null, PmItemId: null, Qty: null, Rate: null, Amount: null, Debit: null, Remarks: null, EmptyBagTermId: null, EmptyBagTerm: null };
        if (side === 'P') { r.GrnId = null; r.GrnNo = null; } else { r.GdnId = null; r.GdnNo = null; }
        return r;
    }
    function emptyComm(order, doc, agent, type, uom, rate, amt) {
        var r = {}; [order, doc, agent, type, uom, rate, amt, 'DebitAccountId'].forEach(function (k) { r[k] = 0; }); return r;
    }
    function emptyPay(side) {
        var r = { Id: 0, PaymentTermId: 0, DueDays: 0, '%OfTotal': 0, Amount: 0, BaseDateTypeId: 0, DueDate: null };
        if (side === 'P') { r.PoId = 0; r.PoNo = 0; } else { r.SoId = 0; r.SoNo = 0; }
        return r;
    }
    function newComm(side, brokery) {
        var p = side === 'P';
        return brokery ? emptyComm(p ? 'PoId' : 'SoId', p ? 'PoNo' : 'SoNo', 'BrokeryAgentId', 'BrokeryTypeId', 'BrokeryUomId', 'BrokeryRate', 'BrokeryAmount')
                       : emptyComm(p ? 'PoId' : 'SoId', p ? 'PoNo' : 'SoNo', 'CommissionAgentId', 'CommissionTypeId', 'CommissionUomId', 'CommissionRate', 'CommissionAmount');
    }
    /* BindGrids (2074): every empty grid gets its default row(s). */
    function bindGrids() {
        if (!S.pOther.length) S.pOther = otherExpenseRows();
        if (!S.sOther.length) S.sOther = otherExpenseRows();
        if (!S.pPm.length) S.pPm = [emptyPm('P')];
        if (!S.sPm.length) S.sPm = [emptyPm('S')];
        if (!S.pPay.length) S.pPay = [emptyPay('P')];
        if (!S.sPay.length) S.sPay = [emptyPay('S')];
        if (!S.pComm.length) S.pComm = [newComm('P', false)];
        if (!S.pBrk.length) S.pBrk = [newComm('P', true)];
        if (!S.sComm.length) S.sComm = [newComm('S', false)];
        if (!S.sBrk.length) S.sBrk = [newComm('S', true)];
    }

    /* ------------------------------------------------------------------ grid events */
    function gridRows(g) {
        return { grd: S.rows, grdPurchaseOtherExpenses: S.pOther, grdSaleOtherExpense: S.sOther, grdPurchasePmExpense: S.pPm,
            grdSalePmExpenseSale: S.sPm, grdPurchaseCommission: S.pComm, grdSaleCommission: S.sComm, grdPurchaseBrokery: S.pBrk,
            grdSaleBrokery: S.sBrk, grdPaymentDetailPurchase: S.pPay, grdPaymentDetailSale: S.sPay }[g];
    }
    function onCellChange(e) {
        var el = e.target; var g = el.getAttribute('data-g'), k = el.getAttribute('data-k');
        if (!g || !k) return;
        var i = +el.getAttribute('data-r'), rows = gridRows(g), r = rows && rows[i];
        if (!r) return;
        var v = el.value;
        if (el.tagName === 'SELECT') v = int(v);
        else if (el.type !== 'date' && !/Remarks|RefDocNo/.test(k)) v = v === '' ? null : num(v);
        r[k] = v;
        try { cellUpdated(g, k, r, i); } catch (x) { alert(x.message); }
        renderAll();
    }
    function cellUpdated(g, k, r, i) {
        if (g === 'grd') {                                       /* grd_CellUpdated (2522) */
            if (k.indexOf('Purchase') === 0) recalcPurchase(r);
            else if (k.indexOf('Sale') === 0) recalcSale(r);
            return;
        }
        if (g === 'grdPurchaseOtherExpenses' || g === 'grdSaleOtherExpense') {   /* 2823 / 3183 */
            var rows = gridRows(g), p = g === 'grdPurchaseOtherExpenses';
            var total = sum(S.rows, p ? 'PurchaseNetAmount' : 'SaleNetAmount');
            if (k === '%OfTotal') {
                var used = 0; rows.forEach(function (x, j) { if (j !== i) used += num(x['%OfTotal']); });
                var rem = 100 - used, pc = num(r['%OfTotal']);
                if (pc > rem) { alert("%of Total Can't be Greater than Remaining: " + rem + '%'); pc = rem; r['%OfTotal'] = pc; }
                r.Qty = 0; r.Rate = 0; r.Amount = rnd(total * pc / 100, 4);
            }
            if ((k === 'Qty' || k === 'Rate') && num(r.Qty) > 0 && num(r.Rate) > 0) r.Amount = num(r.Qty) * num(r.Rate);
            if (k === 'Amount') { r.Qty = 0; r.Rate = 0; }
            if (p) { expenseProportionForPurchase(); supplierBillAmount(); } else { expenseProportionForSale(); customerBillAmount(); }
            return;
        }
        if (g === 'grdPurchasePmExpense') {                      /* 3037 */
            if ((k === 'Qty' || k === 'Rate') && num(r.Qty) > 0 && num(r.Rate) > 0) r.Amount = num(r.Qty) * num(r.Rate);
            if (k === 'Amount') { if (num(r.Debit) > 0) { r.Amount = 0; alert('Debit Side is aleady added'); } else { r.Qty = 0; r.Rate = 0; } }
            if (k === 'AccountId' && int(val('CmbSupplier')) > 0 && purchaseDelivered() && int(r.AccountId) === glOf(val('CmbSupplier'))) {
                r.AccountId = 0; alert("Freight Account And Supplier Gl Account Can't be Same"); return;
            }
            freightProportion(); expenseProportionForSale(); supplierBillAmount();
            return;
        }
        if (g === 'grdSalePmExpenseSale') {                      /* 3393 */
            if (k === 'Qty' || k === 'Rate') r.Amount = num(r.Qty) * num(r.Rate);
            else if (k === 'Amount') { r.Qty = 0; r.Rate = 0; }
            else if (k === 'AccountId' && int(val('CmbCustomer')) > 0 && saleXFactory() && int(r.AccountId) === glOf(val('CmbCustomer'))) {
                r.AccountId = 0; alert("PM Sale Expense Account And Customer Gl Account Can't be Same"); return;
            }
            expenseProportionForSale(); customerBillAmount();
            return;
        }
        var ci = { grdPurchaseCommission: 0, grdPurchaseBrokery: 1, grdSaleCommission: 2, grdSaleBrokery: 3 }[g];
        if (ci !== undefined) {                                  /* HandleCommissionGridCellUpdated (9409) */
            initCommissionConfigs();
            var c = configs[ci];
            if ((k === c.rate || k === c.type) && typeText(r[c.type]) === 'Percent') enforcePercentCap(c, r);
            calculateCommissionGrid(c); proportionateCommissionAndBrokery(); supplierBillAmount(); customerBillAmount();
            return;
        }
        if (g === 'grdPaymentDetailPurchase' || g === 'grdPaymentDetailSale') {   /* 3617 / 3858 */
            var pp = g === 'grdPaymentDetailPurchase', grid = gridRows(g), oc = pp ? 'PoId' : 'SoId';
            if (!S.rows.length) return;
            var tot = num(val(pp ? 'txtPurchaseBillAmount' : 'txtSaleBillAmount'));
            if (S.rows.some(function (x) { return int(x[pp ? 'GrnId' : 'GdnId']) > 0; })) tot = sumWhere(S.rows, pp ? 'PurchaseAmount' : 'SaleAmount', oc, r[oc]);
            if (k === '%OfTotal') {
                var used2 = 0; grid.forEach(function (x, j) { if (j !== i && int(x[oc]) === int(r[oc])) used2 += num(x['%OfTotal']); });
                var rem2 = 100 - used2, pc2 = num(r['%OfTotal']);
                if (pc2 > rem2) { alert("%of Total Can't be Greater than Remaining: " + rem2 + '%'); pc2 = rem2; r['%OfTotal'] = pc2; }
                r.Amount = rnd(tot * pc2 / 100, 4);
                calculatePaymentDetailGrid(pp);
            } else if (k === 'DueDays') {
                r.DueDate = addDays(val('txtDocDate'), int(r.DueDays));
            } else if (k === 'DueDate') {
                var dd = d10(r.DueDate);
                if (dd && dd < val('txtDocDate')) { r.DueDate = val('txtDocDate'); throw new Error("Due Date Can't be Less Than DocDate"); }
                if (dd) r.DueDays = dayDiff(dd, val('txtDocDate'));
            }
        }
    }
    function onGridButton(e) {
        var b = e.target.closest('button[data-btn]'); if (!b) return;
        var g = b.getAttribute('data-g'), act = b.getAttribute('data-btn'), i = +b.getAttribute('data-r');
        if (g === 'GrdHistory') { historyButton(act, i); return; }
        if (g === 'ldMaster') { loaderButton(act, i); return; }
        var rows = gridRows(g), r = rows && rows[i];
        if (!r) return;
        if (g === 'grd' && act === 'Delete') { deleteDetailRow(i); return; }
        if (g === 'grdPurchasePmExpense' || g === 'grdSalePmExpenseSale') {        /* 3100 / 3445 */
            var p = g === 'grdPurchasePmExpense';
            if (act === 'Delete') { if (!confirm('Are you sure to Delete?')) return; rows.splice(i, 1); if (!rows.length) rows.push(emptyPm(p ? 'P' : 'S')); }
            if (act === 'Add') rows.push(emptyPm(p ? 'P' : 'S'));
            if (p) { freightProportion(); supplierBillAmount(); } else { expenseProportionForSale(); customerBillAmount(); }
        } else if (/Commission|Brokery/.test(g)) {                                  /* 4073 / 4260 / 4447 / 4634 */
            var purchase = g.indexOf('Purchase') > 0, oc = purchase ? 'PoId' : 'SoId', isB = g.indexOf('Brokery') > 0;
            if (act === 'Delete') {
                if (int(r[oc]) > 0) { alert(purchase ? 'You Can not Delete Row Because The Entry Is From Purcahse Order' : 'You Can not Delete Row Because The Entry Is From Sale Order'); return; }
                rows.splice(i, 1); if (!rows.length) rows.push(newComm(purchase ? 'P' : 'S', isB));
            }
            if (act === 'Add') rows.push(newComm(purchase ? 'P' : 'S', isB));
            proportionateCommissionAndBrokery();
            if (purchase) supplierBillAmount(); else customerBillAmount();
        } else if (g === 'grdPaymentDetailPurchase' || g === 'grdPaymentDetailSale') { /* 3580 / 3821 */
            var pp = g === 'grdPaymentDetailPurchase', oc2 = pp ? 'PoId' : 'SoId';
            if (act === 'Delete') {
                var oid = int(r[oc2]);
                if (oid > 0 && rows.filter(function (x) { return int(x[oc2]) === oid; }).length <= 1) {
                    alert('At least one payment row must exist per ' + (pp ? 'Purchase Order.' : 'Sale Order.')); return;
                }
                rows.splice(i, 1); if (!rows.length) rows.push(emptyPay(pp ? 'P' : 'S'));
            }
            if (act === 'Add') rows.push(emptyPay(pp ? 'P' : 'S'));
        }
        renderAll();
    }
    /* DeleteDetailGridRow (2482) - rows with an Id go to lstRemoveRecord (ActionTypeId 3). */
    function deleteDetailRow(i) {
        var r = S.rows[i];
        if (int(r.Id) > 0) { if (!confirm('Are you sure to Delete?')) return; S.removed.push(Object.assign({}, r)); }
        S.rows.splice(i, 1);
        plAmountProportion(); calculateHeaderRates(); renderAll();
    }

    /* ================================================================== MANUAL ROW ENTRY (PanelDetailInfo, 2208-2465, 6667-7200) */

    var D = { upd: -1, ebUnitP: false, ebTotP: false, ebUnitS: false, ebTotS: false, uoms: [], taxes: [] };
    function optAttr(id, attr) {
        var e = $id(id); if (!e || e.selectedIndex < 0) return null;
        var o = e.options[e.selectedIndex]; return o ? o.getAttribute(attr) : null;
    }
    function uomEq(id) { return int(val(id)) > 0 ? num(optAttr(id, 'data-eq')) : 0; }
    function itemAttrs(d) { return ' data-item-code="' + esc(d.ItemCode || '') + '" data-item-category="' + esc(d.ItemCategory || '') + '"'; }
    function uomAttrs(d) {
        return ' data-eq="' + esc(d.Equivalent === null || d.Equivalent === undefined ? '' : d.Equivalent) + '" data-base="' + (d.BaseRateUom ? 'true' : 'false')
            + '" data-base-pack="' + (d.BasePackUom ? 'true' : 'false') + '"';
    }
    function taxAttrs(d) { return ' data-tax-percent="' + esc(d.TaxPercent === null || d.TaxPercent === undefined ? '' : d.TaxPercent) + '"'; }
    /* ItemNameBind / rdSearchByName_CheckedChanged (1413): the same dtItem shown by ItemName or ItemCode. */
    function itemBind() {
        var cur = val('CmbItemName');
        fill('CmbItemName', S.L.items, 'Id', $id('rdSearchByCode').checked ? 'ItemCode' : 'ItemName', true, itemAttrs);
        setSel('CmbItemName', cur);
    }
    /* CmbItemName_Leave (1442): TaxTypeBind(TaxTypeDbCallByItemId) + ItemUomFromGlobalBind(pack, rate) + (rate sale). */
    function itemLeave(keepSelections) {
        var itemId = int(val('CmbItemName'));
        var taxReq = itemId > 0
            ? getJson(DD + '/taxes?itemId=' + itemId + '&docDate=' + encodeURIComponent(val('txtDocDate')))
            : Promise.resolve(null);
        var uomReq = getJson(DD + '/item-uoms?itemId=' + itemId);
        return Promise.all([taxReq, uomReq]).then(function (a) {
            if (itemId > 0) {
                /* DatatableHelper.TaxTypeDbCallByItemId keeps Rows[0] only; bound without a default row */
                D.taxes = (a[0] || []).slice(0, 1).map(function (r) { return { Id: pick(r, 'TaxNameId'), TaxName: pick(r, 'TaxName'), TaxPercent: pick(r, 'TaxPercent') }; });
                fill('CmbTaxNamePurchase', D.taxes, 'Id', 'TaxName', false, taxAttrs);
                fill('CmbTaxNameSale', D.taxes, 'Id', 'TaxName', false, taxAttrs);
                if (D.upd > -1 && S.rows[D.upd]) {
                    var r = S.rows[D.upd];
                    if (int(r.PurchaseTaxNameId) > 0) setSel('CmbTaxNamePurchase', r.PurchaseTaxNameId);
                    if (int(r.SaleTaxNameId) > 0) setSel('CmbTaxNameSale', r.SaleTaxNameId);
                }
                calcTaxP(); calcTaxS();                                           /* TaxTypeBind -> *_Leave */
            } else {
                D.taxes = [];
                ['CmbTaxNamePurchase', 'CmbTaxNameSale'].forEach(function (id) { $id(id).innerHTML = ''; });
                ['txtTaxPercntPurchase', 'txtTaxAmountPurchase', 'txtTaxPercntSale', 'txtTaxAmountSale'].forEach(function (id) { setVal(id, ''); });
            }
            /* BindAndRetainSelection by text, then the base pack / rate uom (setBaseValues) */
            D.uoms = a[1] || [];
            var pt = selText('CmbPackUom'), rp = selText('CmbRateUomPurchase'), rs = selText('CmbRateUomSale');
            fill('CmbPackUom', D.uoms, 'Id', 'UOMCode', true, uomAttrs);
            fill('CmbRateUomPurchase', D.uoms, 'Id', 'UOMCode', true, uomAttrs);
            fill('CmbRateUomSale', D.uoms, 'Id', 'UOMCode', true, uomAttrs);
            if (pt) setSelByText('CmbPackUom', pt); else setSel('CmbPackUom', 0);
            if (rp) setSelByText('CmbRateUomPurchase', rp); else setSel('CmbRateUomPurchase', 0);
            if (rs) setSelByText('CmbRateUomSale', rs); else setSel('CmbRateUomSale', 0);
            if (!keepSelections) {
                var bp = D.uoms.find(function (u) { return u.BasePackUom === true; }), br = D.uoms.find(function (u) { return u.BaseRateUom === true; });
                if (bp) setSel('CmbPackUom', bp.Id);
                if (br) { setSel('CmbRateUomPurchase', br.Id); setSel('CmbRateUomSale', br.Id); }
            }
            detailChain({ amounts: true });
        });
    }
    /* CalculateGrossWeight (6667) */
    function calcGross() {
        var q = num(val('txtQuantity')), eq = uomEq('CmbPackUom');
        if (q > 0 && int(val('CmbPackUom')) > 0) { var w = q * eq; setVal('txtGrossWeightPurchase', fmtN(w, 2, 0)); setVal('txtGrossWeightSale', fmtN(w, 2, 0)); }
        else { setVal('txtGrossWeightPurchase', '0'); setVal('txtGrossWeightSale', '0'); }
    }
    /* PurchaseEbCalculations (6691) / SaleEbCalculations (6740); active = the field being typed in */
    function ebCalc(side, active) {
        var p = side === 'P', u = p ? 'txtEBUnitPurchase' : 'txtEBUnitSale', t = p ? 'txtEBTotalPurchase' : 'txtEBTotalSale';
        var fu = p ? 'ebUnitP' : 'ebUnitS', ft = p ? 'ebTotP' : 'ebTotS';
        var unit = num(val(u)), tot = num(val(t)), f = num(val('txtQuantity'));
        if (f <= 0) { setVal(u, '0'); setVal(t, '0'); return; }
        if (active === 'unit') { setVal(t, f3(unit > 0 ? rnd(unit * f, 3) : 0)); D[fu] = true; D[ft] = false; }
        else if (active === 'total') { setVal(u, f3(tot > 0 ? rnd(tot / f, 3) : 0)); D[fu] = false; D[ft] = true; }
        else if (D[ft]) setVal(u, f3(tot > 0 ? rnd(tot / f, 3) : 0));
        else if (D[fu]) setVal(t, f3(unit > 0 ? rnd(unit * f, 3) : 0));
    }
    /* CalculateBillWeightPurchase (6804) / Sale (6789) */
    function billWeight(side) {
        var p = side === 'P';
        var g = num(val(p ? 'txtGrossWeightPurchase' : 'txtGrossWeightSale')), eb = num(val(p ? 'txtEBTotalPurchase' : 'txtEBTotalSale')),
            al = num(val(p ? 'txtAddlessWtPurchase' : 'txtAddlessWtSales'));
        setVal(p ? 'txtBillWeightPurchase' : 'txtBillWeightSale', f3(g - eb + al));
    }
    /* txtSupplierRate_TextChanged / txtPurchaseRateAddLess_TextChanged (6989-7026) and the sale pair (7118-7155) */
    function netRate(side) {
        var p = side === 'P';
        setVal(p ? 'txtPurchaseNetRate' : 'txtSaleNetRate',
            f3(num(val(p ? 'txtPurchaseRateWithoutAddLess' : 'txtSaleRateWithoutAddLess')) + num(val(p ? 'txtPurchaseRateAddLess' : 'txtSaleRateAddLess'))));
    }
    /* CalculatePurchaseAmount (6819) / CalculateSaleAmount (6842) */
    function itemAmount(side) {
        var p = side === 'P', rate = num(val(p ? 'txtPurchaseNetRate' : 'txtSaleNetRate')), bw = num(val(p ? 'txtBillWeightPurchase' : 'txtBillWeightSale'));
        var uid = p ? 'CmbRateUomPurchase' : 'CmbRateUomSale';
        setVal(p ? 'txtItemAmountPurchase' : 'txtItemAmountSale',
            (rate > 0 && bw > 0 && int(val(uid)) > 0) ? fmtSingle(bw / uomEq(uid) * rate) : '0');
    }
    /* CalculateTaxAmountPurchase (6865): percent from the tax combo's third column */
    function calcTaxP() {
        var a = num(val('txtItemAmountPurchase')), pct = $id('CmbTaxNamePurchase').options.length ? num(optAttr('CmbTaxNamePurchase', 'data-tax-percent')) : 0;
        var t = a * pct / 100;
        setVal('txtTaxPercntPurchase', f3(pct)); setVal('txtTaxAmountPurchase', f3(rnd(t, 3))); setVal('txtTotalAmountPurchase', f3(rnd(a + t, 3)));
    }
    /* CalculateTaxAmountSale (6889): a typed percent wins over the combo's */
    function calcTaxS() {
        var a = num(val('txtItemAmountSale')), pct = 0;
        if ($id('CmbTaxNameSale').options.length) pct = num(val('txtTaxPercntSale')) > 0 ? num(val('txtTaxPercntSale')) : num(optAttr('CmbTaxNameSale', 'data-tax-percent'));
        var t = a * pct / 100;
        setVal('txtTaxPercntSale', f3(pct)); setVal('txtTaxAmountSale', f3(rnd(t, 3))); setVal('txtTotalAmountSale', f3(rnd(a + t, 3)));
    }
    /* the TextChanged cascade of the panel, in the desktop's order */
    function detailChain(o) {
        o = o || {};
        if (o.qty) { ebCalc('P', null); ebCalc('S', null); calcGross(); }
        if (o.gross) calcGross();
        if (o.ebP) ebCalc('P', o.ebP);
        if (o.ebS) ebCalc('S', o.ebS);
        if (o.rateP) netRate('P');
        if (o.rateS) netRate('S');
        billWeight('P'); billWeight('S');
        itemAmount('P'); itemAmount('S');
        calcTaxP(); calcTaxS();
    }
    /* FormValidationDetail (1929) */
    function formValidationDetail() {
        var chk = [
            [int(val('CmbItemName')) === 0, 'Item Field Required', 'CmbItemName'],
            [int(val('CmbCropYear')) === 0, 'Crop Year Field Required', 'CmbCropYear'],
            [int(val('CmbPackType')) === 0, 'Pack Type Field Required', 'CmbPackType'],
            [int(val('CmbPackUom')) === 0, 'Pack Uom Field Required', 'CmbPackUom'],
            [int(val('txtQuantity').trim()) === 0, 'Quantity Field Required', 'txtQuantity'],
            [num(val('txtGrossWeightPurchase')) === 0, 'Gross Weight Purchase Field is Required', 'txtGrossWeightPurchase'],
            [num(val('txtBillWeightPurchase')) === 0, ' Purchase Bill Weight Field is Required', 'txtGrossWeightPurchase'],
            [num(val('txtGrossWeightSale')) === 0, 'Gross Weight Sale Field is Required', 'txtGrossWeightSale'],
            [num(val('txtBillWeightSale')) === 0, ' Sale Bill Weight Field is Required', 'txtGrossWeightPurchase'],
            [num(val('txtPurchaseNetRate')) === 0, 'Rate Purchase Field is Required', 'txtPurchaseRateWithoutAddLess'],
            [int(val('CmbRateUomPurchase')) === 0, 'RateUom Field is Required', 'CmbRateUomPurchase'],
            [num(val('txtSaleNetRate')) === 0, 'Rate Sale Field is Required', 'txtSaleRateWithoutAddLess'],
            [int(val('CmbRateUomSale')) === 0, 'RateUom Sale Field is Required', 'CmbRateUomSale'],
            [num(val('txtItemAmountPurchase')) === 0, 'Purchase Amount Field is Required', 'txtItemAmountPurchase'],
            [num(val('txtItemAmountSale')) === 0, 'Sale Amount Field is Required', 'txtItemAmountSale'],
            [val('txtVehicleNo').trim() === '', 'Vehicle No Field is Required', 'txtVehicleNo'],
            [val('txtbiltyno').trim() === '', 'Bilty No Field is Required', 'txtbiltyno']];
        for (var i = 0; i < chk.length; i++) if (chk[i][0]) { alert(chk[i][1]); var f = $id(chk[i][2]); if (f) f.focus(); return false; }
        return true;
    }
    /* FillDetailRow (2208) */
    function fillDetailRow(dr) {
        var item = S.L.items.find(function (x) { return int(x.Id) === int(val('CmbItemName')); }) || {};
        Object.assign(dr, {
            ItemId: int(val('CmbItemName')), ItemCode: item.ItemCode, ItemName: item.ItemName,
            CropYearId: int(val('CmbCropYear')), CropYear: selText('CmbCropYear').trim(),
            PackTypeId: int(val('CmbPackType')), PackTypeDesc: selText('CmbPackType').trim(),
            UomId: int(val('CmbPackUom')), UomCode: selText('CmbPackUom').trim(), PackEquivalent: uomEq('CmbPackUom'),
            ItemQty: num(val('txtQuantity').trim()), RefDocNo: val('txtRefDocNo'), VehicleNo: val('txtVehicleNo'), BiltyNo: val('txtbiltyno'),
            PurchaseGrossWeight: num(val('txtGrossWeightPurchase')), PurchaseEbCut: num(val('txtEBUnitPurchase')), PurchaseEbCutTotal: num(val('txtEBTotalPurchase')),
            PurchaseAddLessWeight: num(val('txtAddlessWtPurchase')), PurchaseBillWeight: num(val('txtBillWeightPurchase')),
            PurchaseRateWithoutAddLess: num(val('txtPurchaseRateWithoutAddLess')), PurchaseRateAddLess: num(val('txtPurchaseRateAddLess')),
            PurchaseNetRate: num(val('txtPurchaseNetRate')), PurchaseRateUomId: int(val('CmbRateUomPurchase')), PurchaseRateUom: selText('CmbRateUomPurchase'),
            PurchaseRateEquivalent: uomEq('CmbRateUomPurchase'), PurchaseAmount: num(val('txtItemAmountPurchase')),
            PurchaseTaxNameId: int(val('CmbTaxNamePurchase')), PurchaseTaxName: selText('CmbTaxNamePurchase'),
            'PurchaseTax%': num(val('txtTaxPercntPurchase')), PurchaseTaxAmount: num(val('txtTaxAmountPurchase')), PurchaseNetAmount: num(val('txtTotalAmountPurchase')),
            SaleGrossWeight: num(val('txtGrossWeightSale')), SaleEbCut: num(val('txtEBUnitSale')), SaleEbCutTotal: num(val('txtEBTotalSale')),
            SaleAddLess: num(val('txtAddlessWtSales')), SaleBillWeight: num(val('txtBillWeightSale')),
            SaleRateWithoutAddLess: num(val('txtSaleRateWithoutAddLess')), SaleRateAddLess: num(val('txtSaleRateAddLess')),
            SaleNetRate: num(val('txtSaleNetRate')), SaleRateUomId: int(val('CmbRateUomSale')), SaleRateUom: selText('CmbRateUomSale'),
            SaleRateEquivalent: uomEq('CmbRateUomSale'), SaleAmount: num(val('txtItemAmountSale')),
            SaleTaxNameId: int(val('CmbTaxNameSale')), SaleTaxName: selText('CmbTaxNameSale'),
            'SaleTax%': num(val('txtTaxPercntSale')), SaleTaxAmount: num(val('txtTaxAmountSale')), SaleNetAmount: num(val('txtTotalAmountSale')),
            RemarksDetail: val('txtDetailRemarks')
        });
    }
    function newDetailRow() {
        return { Id: 0, GdnId: 0, GdnDetailId: 0, GdnNo: null, GdnDate: null, GrnId: 0, GrnDetailId: 0, GrnNo: null, GrnDate: null,
            PoId: 0, PoDetailId: 0, PoNo: null, PoDate: null, SoId: 0, SoDetailId: 0, SoNo: null, SoDate: null,
            PurchaseFreight: null, PurchaseFreightLess: null, PurchaseExpenses: null, PurchaseCommission: null, PurchaseBrokery: null,
            SaleExpenses: null, SaleCommission: null, SaleBrokery: null, PLAmount: null };
    }
    /* the recalculation btnAddDetail_Click / btnUpdateDetail_Click run after the row changes */
    function afterDetailRowChange() {
        expenseProportionForPurchase(); expenseProportionForSale(); freightProportion();
        initCommissionConfigs(); configs.forEach(calculateCommissionGrid); proportionateCommissionAndBrokery();
        supplierBillAmount(); customerBillAmount(); plAmountProportion(); calculateHeaderRates();
        renderAll();
    }
    /* btnAddDetail_Click (2261) */
    function addDetail() {
        if (!formValidationDetail()) return;
        var dr = newDetailRow(); fillDetailRow(dr); S.rows.push(dr);
        detailFormReset(); afterDetailRowChange();
    }
    /* btnUpdateDetail_Click (2430) */
    function updateDetail() {
        if (!formValidationDetail() || D.upd < 0 || D.upd >= S.rows.length) return;
        fillDetailRow(S.rows[D.upd]);
        detailFormReset(); detailButtons(false); D.upd = -1;
        afterDetailRowChange();
    }
    /* btnCancelDetail_Click (2415) */
    function cancelDetail() { detailButtons(false); detailFormReset(); D.upd = -1; }
    function detailButtons(editing) { $id('btnAddDetail').hidden = editing; $id('btnUpdateDetail').hidden = !editing; $id('btnCancelDetail').hidden = !editing; }
    /* DetailFormReset (1674) - the fields it clears, and only those */
    function detailFormReset() {
        ['txtQuantity', 'txtGrossWeightPurchase', 'txtGrossWeightSale', 'txtEBUnitPurchase', 'txtEBTotalPurchase', 'txtBillWeightPurchase',
            'txtEBUnitSale', 'txtEBTotalSale', 'txtAddlessWtSales', 'txtBillWeightSale', 'txtPurchaseRateWithoutAddLess', 'txtPurchaseNetRate',
            'txtSaleRateWithoutAddLess', 'txtSaleNetRate', 'txtItemAmountPurchase', 'txtItemAmountSale', 'txtVehicleNo', 'txtRefDocNo', 'txtDetailRemarks']
            .forEach(function (id) { setVal(id, ''); });
        var f = $id('CmbItemName'); if (f && !$id('PanelDetailInfo').hidden) try { f.focus(); } catch (x) { /* ignore */ }
    }
    /* grdDetail_DoubleClick (2296): only a manual row (GdnId <= 0) is edited in the panel */
    function editDetailRow(i) {
        var r = S.rows[i]; if (!r || int(r.GdnId) > 0) return;
        D.upd = i;
        setSel('CmbItemName', r.ItemId);
        setSel('CmbCropYear', r.CropYearId); setSel('CmbPackType', r.PackTypeId);
        var f3s = function (v) { return fmtN(num(v), 3, 0); };
        return itemLeave(true).then(function () {
            setSel('CmbPackUom', r.UomId);
            setVal('txtQuantity', r.ItemQty); setVal('txtVehicleNo', r.VehicleNo || ''); setVal('txtRefDocNo', r.RefDocNo || '');
            setVal('txtbiltyno', r.BiltyNo || '');
            setVal('txtGrossWeightPurchase', f3s(r.PurchaseGrossWeight)); setVal('txtEBUnitPurchase', f3s(r.PurchaseEbCut)); setVal('txtEBTotalPurchase', f3s(r.PurchaseEbCutTotal));
            setVal('txtAddlessWtPurchase', f3s(r.PurchaseAddLessWeight)); setVal('txtBillWeightPurchase', f3s(r.PurchaseBillWeight));
            setSel('CmbRateUomPurchase', r.PurchaseRateUomId);
            setVal('txtPurchaseRateWithoutAddLess', f3s(r.PurchaseRateWithoutAddLess)); setVal('txtPurchaseRateAddLess', f3s(r.PurchaseRateAddLess)); setVal('txtPurchaseNetRate', f3s(r.PurchaseNetRate));
            if (int(r.PurchaseTaxNameId) > 0) setSel('CmbTaxNamePurchase', r.PurchaseTaxNameId);
            setVal('txtTaxPercntPurchase', f3s(r['PurchaseTax%'])); setVal('txtTaxAmountPurchase', f3s(r.PurchaseTaxAmount)); setVal('txtItemAmountPurchase', f3s(r.PurchaseAmount));
            setVal('txtGrossWeightSale', f3s(r.SaleGrossWeight)); setVal('txtEBUnitSale', f3s(r.SaleEbCut)); setVal('txtEBTotalSale', f3s(r.SaleEbCutTotal));
            setVal('txtAddlessWtSales', f3s(r.SaleAddLess)); setVal('txtBillWeightSale', f3s(r.SaleBillWeight));
            setSel('CmbRateUomSale', r.SaleRateUomId);
            setVal('txtSaleRateWithoutAddLess', f3s(r.SaleRateWithoutAddLess)); setVal('txtSaleRateAddLess', f3s(r.SaleRateAddLess)); setVal('txtSaleNetRate', f3s(r.SaleNetRate));
            if (int(r.SaleTaxNameId) > 0) setSel('CmbTaxNameSale', r.SaleTaxNameId);
            setVal('txtTaxPercntSale', f3s(r['SaleTax%'])); setVal('txtTaxAmountSale', f3s(r.SaleTaxAmount)); setVal('txtItemAmountSale', f3s(r.SaleAmount));
            setVal('txtDetailRemarks', r.RemarksDetail || '');
            /* the TextChanged events those assignments raise on the desktop */
            calcTaxP(); calcTaxS();
            detailButtons(true);
            expenseProportionForPurchase(); expenseProportionForSale(); renderAll();
            try { $id('CmbItemName').focus(); } catch (x) { /* ignore */ }
        });
    }
    function wireDetailPanel() {
        var on = function (id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); };
        on('rdSearchByName', 'change', itemBind); on('rdSearchByCode', 'change', itemBind);
        on('CmbItemName', 'change', function () { itemLeave(false); });
        on('CmbPackUom', 'change', function () { detailChain({ gross: true }); });                     /* CmbPackUom_Leave */
        on('txtQuantity', 'input', function () { detailChain({ qty: true }); });
        on('txtGrossWeightPurchase', 'input', function () { detailChain({}); });
        on('txtGrossWeightSale', 'input', function () { detailChain({}); });
        on('txtEBUnitPurchase', 'input', function () { detailChain({ ebP: 'unit' }); });
        on('txtEBTotalPurchase', 'input', function () { detailChain({ ebP: 'total' }); });
        on('txtEBUnitSale', 'input', function () { detailChain({ ebS: 'unit' }); });
        on('txtEBTotalSale', 'input', function () { detailChain({ ebS: 'total' }); });
        on('txtAddlessWtPurchase', 'input', function () { detailChain({}); });
        on('txtAddlessWtSales', 'input', function () { detailChain({}); });
        on('txtPurchaseRateWithoutAddLess', 'input', function () { detailChain({ rateP: true }); });
        on('txtPurchaseRateAddLess', 'input', function () { detailChain({ rateP: true }); });
        on('txtSaleRateWithoutAddLess', 'input', function () { detailChain({ rateS: true }); });
        on('txtSaleRateAddLess', 'input', function () { detailChain({ rateS: true }); });
        on('CmbRateUomPurchase', 'change', function () { detailChain({}); });
        on('CmbRateUomSale', 'change', function () { detailChain({}); });
        on('CmbTaxNamePurchase', 'change', calcTaxP);
        on('CmbTaxNameSale', 'change', calcTaxS);
        on('btnAddDetail', 'click', addDetail);
        on('btnUpdateDetail', 'click', updateDetail);
        on('btnCancelDetail', 'click', cancelDetail);
        /* grdDetail_DoubleClick */
        $id('grd').addEventListener('dblclick', function (e) {
            if (e.target.closest('input,select,button')) return;
            var tr = e.target.closest('tbody tr'); if (!tr) return;
            editDetailRow(Array.prototype.indexOf.call(tr.parentNode.children, tr));
        });
    }

    /* ================================================================== LOADER (frmPendingGdnLoader) */

    /* btnLoadGdn_Click (8449): a NEW frmPendingGdnLoader each time (using ... ShowDialog), so every open
       starts from the designer state: FromDate = ToDate = today (DateTimePicker default), empty doc nos and
       combos; InitializeComponentMethod (:129) = ComboDbCall/CombosFill, PendingDataDbCall + GrdDataBind with
       that state, THEN FromDate.Value = Now - 7 (:143). */
    function openLoader() {
        if (S.rows.length) { alert("You Can't Load Because Entry Already Exist"); return; }
        var t = new Date();
        setVal('ldFromDate', ymd(t)); setVal('ldToDate', ymd(t)); setVal('ldDocNoFrom', ''); setVal('ldDocNoTo', '');
        ['ldCommissionAgent', 'ldSupplier', 'ldBuyer', 'ldItem', 'ldDeliveryToParty', 'ldShipToAddress'].forEach(function (id) { var e = $id(id); if (e) { e.innerHTML = ''; } });
        loaderState.sets = []; loaderState.master = []; loaderState.selectedId = 0; loaderState.dPos = 0;
        $id('loaderModal').classList.remove('fs-detail');
        renderLoader();
        $id('loaderModal').classList.add('open');
        loaderCombos().then(loaderSearch).then(function () {
            setVal('ldFromDate', ymd(new Date(t.getFullYear(), t.getMonth(), t.getDate() - 7)));
            try { $id('ldFromDate').focus(); } catch (x) { /* ignore */ }
        });
    }
    /* ComboDbCall + CombosFill (:158 / :186) - also btnRefresh_Click (:644), which only refills the combos */
    function loaderCombos() {
        return getJson(API + '/loader/dropdowns').then(function (rows) {
            /* CombosFill: split the flat table by Activity, distinct by Id (address by Id|Name) */
            var b = { CommissionAgent: [], SupplierName: [], BuyerName: [], Item: [], DeliveryToParty: [], DeliverToAddress: [] }, seen = {};
            (rows || []).forEach(function (r) {
                var a = pick(r, 'Activity'), id = pick(r, 'Id'), n = pick(r, 'ReferenceName');
                if (!b[a]) return;
                var key = a + '|' + id + (a === 'DeliverToAddress' ? '|' + n : '');
                if (seen[key]) return; seen[key] = 1;
                b[a].push({ id: id, name: n });
            });
            fill('ldCommissionAgent', b.CommissionAgent, 'id', 'name');
            fill('ldSupplier', b.SupplierName, 'id', 'name');
            fill('ldBuyer', b.BuyerName, 'id', 'name');
            fill('ldItem', b.Item, 'id', 'name');
            fill('ldDeliveryToParty', b.DeliveryToParty, 'id', 'name');
            fill('ldShipToAddress', b.DeliverToAddress, 'name', 'name', true);
        });
    }
    /* btnReset_Click (:626): FromDate = ActiveYr.Start_Period, doc nos cleared, only CmbItemName cleared, then search */
    function loaderReset() {
        return withButton('btnLoaderReset', function () {
            setVal('ldDocNoFrom', ''); setVal('ldDocNoTo', '');
            var it = $id('ldItem'); if (it) it.value = '0';
            return getJson(API + '/lookups/year-start').then(function (r) {
                if (r && r.financialYearStart) setVal('ldFromDate', String(r.financialYearStart).substring(0, 10));
                try { $id('ldFromDate').focus(); } catch (x) { /* ignore */ }
                return loaderSearch();
            });
        });
    }
    function loaderRefresh() { return withButton('btnLoaderRefresh', loaderCombos); }
    /* MakeShortCutKeys (:660) / main form MakeShortCutKeys (:8240) -> ShortCutKeyPopUp */
    var LD_KEYS = [['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+L', 'To Press Load Button'], ['Ctrl+S', 'For Search'],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    var MAIN_KEYS = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
        ['Alt+1', 'For Print Slip 168'], ['Alt+2', 'For Print Slip 170'], ['Alt+3', 'For Print Slip 171'], ['Alt+4', 'For Print Slip 172'], ['Alt+5', 'For Print Slip 1706'],
        ['Alt+6', 'For Print Voucher 118'], ['Ctrl+F5', 'For Focus on Supplier Name'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+D', 'For Adding an row in Focused Grid'], ['Ctrl+Delete', 'For Deleting an row of Focused Grid'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On Customer Name in Detail Box'],
        ['Ctrl+ArrowRight', 'For Change Focus from one Grid To another Grid'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function showKeys(list) {
        $id('keysGrid').innerHTML = '<thead><tr><th>KeyCombination</th><th>Description</th></tr></thead><tbody>'
            + list.map(function (k) { return '<tr><td>' + esc(k[0]) + '</td><td>' + esc(k[1]) + '</td></tr>'; }).join('') + '</tbody>';
        $id('keysModal').classList.add('open');
    }
    /* PendingDataDbCall + GrdDataBind (290 / 322) */
    function loaderSearch() {
        return withButton('btnLoaderSearch', function () {
            var q = ['fromDate=' + encodeURIComponent(val('ldFromDate')), 'toDate=' + encodeURIComponent(val('ldToDate')),
                'fromDocNo=' + int(val('ldDocNoFrom')), 'toDocNo=' + int(val('ldDocNoTo')), 'commissionAgentId=' + int(val('ldCommissionAgent')),
                'supplierId=' + int(val('ldSupplier')), 'buyerId=' + int(val('ldBuyer')), 'itemId=' + int(val('ldItem')),
                'deliverToPartyId=' + int(val('ldDeliveryToParty'))];
            var ship = val('ldShipToAddress'); if (ship && ship !== '0') q.push('shipToAddress=' + encodeURIComponent(ship));
            return fetchJson(API + '/loader/pending?' + q.join('&')).then(function (sets) {
                if (sets && sets.status === 'ERROR') throw new Error(sets.message);
                loaderState.sets = Array.isArray(sets) ? sets : [];
                var seen = {}; loaderState.master = [];
                (loaderState.sets[0] || []).forEach(function (r) {
                    var id = int(pick(r, 'gdnBuyerDispatchMasterId'));
                    if (seen[id]) return; seen[id] = 1; loaderState.master.push(r);
                });
                /* grd.CurrentRow after binding = first row -> SelectionChanged binds grdDetail */
                loaderState.selectedId = loaderState.master.length ? int(pick(loaderState.master[0], 'gdnBuyerDispatchMasterId')) : 0;
                loaderState.dPos = 0;
                renderLoader();
            });
        });
    }
    var LD_MASTER = [['docNo', 'DocNo'], ['docDate', 'DocDate', 'd'], ['CommissionAgentName', 'CommissionAgentName'], ['BuyerName', 'BuyerName'],
        ['DeliveryToPartyName', 'DeliveryToPartyName'], ['DeliverToAddress', 'DeliverToAddress'], ['LoadingCityName', 'LoadingCityName'],
        ['UnloadingCityName', 'UnloadingCityName'], ['DeliveryTerm', 'DeliveryTerm'], ['transporterName', 'TransporterName'],
        ['totalFreight', 'TotalFreight', 'n'], ['vehicleNo', 'VehicleNo'], ['biltyNo', 'BiltyNo'], ['biltyDate', 'BiltyDate', 'd'], ['biltyQty', 'BiltyQty', 'n'],
        ['BuyerLoadWeight', 'BuyerLoadWeight', 'n'], ['BuyerTareWeight', 'BuyerTareWeight', 'n'], ['BuyerScaleNetWeight', 'BuyerScaleNetWeight', 'n'],
        ['GrnNo', 'GrnNo'], ['GrnDate', 'GrnDate', 'd'], ['SupplierName', 'SupplierName'], ['GrnDeliveryTerm', 'GrnDeliveryTerm'],
        ['GrnTransporterName', 'GrnTransporterName'], ['GrnTotalFreight', 'GrnTotalFreight', 'n'], ['GrnVehicleNo', 'GrnVehicleNo'], ['GrnBiltyNo', 'GrnBiltyNo'],
        ['GrnBiltyDate', 'GrnBiltyDate', 'd'], ['GrnBiltyQty', 'GrnBiltyQty', 'n'], ['SupplierLoadWeight', 'SupplierLoadWeight', 'n'],
        ['SupplierTareWeight', 'SupplierTareWeight', 'n'], ['SupplierScaleNetWeight', 'SupplierScaleNetWeight', 'n'], ['warningRemarksHeader', 'WarningRemarks'],
        ['entryDate', 'EntryDate', 'd'], ['EntryUserName', 'EntryUserName'], ['modifyDate', 'ModifyDate', 'd'], ['ModifyUserName', 'ModifyUserName'], ['NoOfAttachments', 'NoOfAttachments']];
    var LD_DETAIL = [['SaleOrderNo', 'SaleOrderNo'], ['SaleOrderDate', 'SaleOrderDate', 'd'], ['PurchaseOrderNo', 'PurchaseOrderNo'], ['PurchaseOrderDate', 'PurchaseOrderDate', 'd'],
        ['inventoryParentCategory', 'InventoryParentCategory'], ['ItemName', 'ItemName'], ['PackUomCode', 'PackUom'], ['cropYear', 'CropYear'], ['PackingType', 'PackingType'],
        ['loadingQty', 'LoadingQty', 'n'], ['wbGrossWeight', 'GrossWeight', 'n'], ['ebwPerUnit', 'EbUnit', 'n'], ['ebwTotal', 'EbTotal', 'n'], ['addLessWeight', 'AddLessWeight', 'n'],
        ['netBillWeight', 'NetBillWeight', 'n'], ['GrnGrossWeight', 'GrnGrossWeight', 'n'], ['GrnEbUnit', 'GrnEbUnit', 'n'], ['GrnEbTotal', 'GrnEbTotal', 'n'],
        ['GrnAddLessWeight', 'GrnAddLessWeight', 'n'], ['GrnNetBillWeight', 'GrnNetBillWeight', 'n'], ['SaleRate', 'SaleRate', 'n'], ['SaleRateUomCode', 'SaleRateUom'],
        ['SaleAmount', 'SaleAmount', 'n'], ['TaxNameSale', 'TaxNameSale'], ['TaxPercentSale', 'TaxPercentSale', 'n'], ['TotalAmountSale', 'TotalAmountSale', 'n'],
        ['PurchaseRate', 'PurchaseRate', 'n'], ['PurchaseRateUomCode', 'PurchaseRateUom'], ['PurchaseAmount', 'PurchaseAmount', 'n'], ['TaxNamePurchase', 'TaxNamePurchase'],
        ['TaxPercentPurchase', 'TaxPercentPurchase', 'n'], ['TotalAmountPurchase', 'TotalAmountPurchase', 'n'], ['warningRemarks', 'WarningRemarks']];
    function ldCell(r, c) {
        var v = pick(r, c[0]);
        if (c[2] === 'd') return '<td>' + esc(dmy(v)) + '</td>';
        if (c[2] === 'n') return '<td class="num">' + esc(v === null || v === undefined || v === '' ? '' : f3(v)) + '</td>';
        return '<td>' + esc(v === null || v === undefined ? '' : v) + '</td>';
    }
    function ldRows() {
        return ((loaderState.sets && loaderState.sets[0]) || []).filter(function (r) { return int(pick(r, 'gdnBuyerDispatchMasterId')) === loaderState.selectedId; });
    }
    /* Janus RecordNavigator: |< < Record n of N > >| */
    function ldNav(id, pos, count) {
        var dis = function (b) { return b ? ' disabled' : ''; };
        $id(id).innerHTML = '<button type="button" data-nav="first"' + dis(pos <= 0) + '>|&lt;</button><button type="button" data-nav="prev"' + dis(pos <= 0) + '>&lt;</button>'
            + '<span class="pos">Record ' + (count ? pos + 1 : 0) + ' of ' + count + '</span>'
            + '<button type="button" data-nav="next"' + dis(pos >= count - 1) + '>&gt;</button><button type="button" data-nav="last"' + dis(pos >= count - 1) + '>&gt;|</button>';
    }
    function renderLoader() {
        var h = '<thead><tr><th>Load</th>' + LD_MASTER.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr></thead><tbody>';
        var mPos = -1;
        loaderState.master.forEach(function (r, i) {
            var id = int(pick(r, 'gdnBuyerDispatchMasterId')); if (id === loaderState.selectedId) mPos = i;
            h += '<tr data-ld="' + i + '"' + (id === loaderState.selectedId ? ' class="sel"' : '') + '><td><button type="button" data-g="ldMaster" data-btn="Load" data-r="' + i + '">Load This</button></td>'
                + LD_MASTER.map(function (c) { return ldCell(r, c); }).join('') + '</tr>';
        });
        $id('ldMaster').innerHTML = h + '</tbody>';
        ldNav('ldNavMaster', mPos, loaderState.master.length);
        var rows = ldRows();
        var d = '<thead><tr>' + LD_DETAIL.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr></thead><tbody>';
        rows.forEach(function (r, i) { d += '<tr' + (i === (loaderState.dPos || 0) ? ' class="sel"' : '') + '>' + LD_DETAIL.map(function (c) { return ldCell(r, c); }).join('') + '</tr>'; });
        $id('ldDetail').innerHTML = d + '</tbody>';
        ldNav('ldNavDetail', rows.length ? (loaderState.dPos || 0) : -1, rows.length);
    }
    function ldNavigate(grid, how) {
        if (grid === 'M') {
            var n = loaderState.master.length; if (!n) return;
            var i = loaderState.master.findIndex(function (m) { return int(pick(m, 'gdnBuyerDispatchMasterId')) === loaderState.selectedId; });
            i = how === 'first' ? 0 : how === 'last' ? n - 1 : how === 'prev' ? Math.max(0, i - 1) : Math.min(n - 1, i + 1);
            loaderState.selectedId = int(pick(loaderState.master[i], 'gdnBuyerDispatchMasterId')); loaderState.dPos = 0;
        } else {
            var c = ldRows().length; if (!c) return; var p = loaderState.dPos || 0;
            loaderState.dPos = how === 'first' ? 0 : how === 'last' ? c - 1 : how === 'prev' ? Math.max(0, p - 1) : Math.min(c - 1, p + 1);
        }
        renderLoader();
    }
    /* grd_ColumnButtonClick "Load" (772) -> dtLoader = rows of that GDN */
    function loaderButton(act, i) {
        var m = loaderState.master[i]; if (!m || act !== 'Load') return;
        var id = int(pick(m, 'gdnBuyerDispatchMasterId'));
        if (id <= 0) { alert('No valid row selected.'); return; }
        var rows = (loaderState.sets[0] || []).filter(function (r) { return int(pick(r, 'gdnBuyerDispatchMasterId')) === id; });
        if (!rows.length) { alert('No matching records found.'); return; }
        S.ds = loaderState.sets;
        $id('loaderModal').classList.remove('open');
        loadInGridDetail(rows);
    }

    /* LoadInGridDetail (8472) */
    function loadInGridDetail(dt) {
        if (!dt.length) return;
        var ex = S.rows.find(function (r) { return int(r.GdnId) > 0; });
        if (ex && int(ex.GdnId) !== int(pick(dt[0], 'gdnBuyerDispatchMasterId'))) { alert('Already loaded rows belong to a different Gdn. You cannot load rows from another Gdn.'); return; }
        var row = dt[0], lines = [], hdr = pick(row, 'warningRemarksHeader');
        if (hdr && String(hdr).trim()) lines.push('1. ' + hdr);
        var counter = lines.length ? 2 : 1;
        dt.forEach(function (r) { var w = pick(r, 'warningRemarks'); if (w && String(w).trim()) { lines.push(counter + '. ' + w); counter++; } });
        setVal('txtMainRemarks', lines.join('\r\n').trim());
        setSel('CmbSupplier', int(pick(row, 'supplierId')));
        setSel('CmbDeliveryTermPurchase', int(pick(row, 'GrnDeliveryTermId')));
        setVal('txtSupplierFirstWeight', f3(pick(row, 'SupplierLoadWeight')));
        setVal('txtSupplierSecondWeight', f3(pick(row, 'SupplierTareWeight')));
        setVal('txtSupplierWeight', f3(pick(row, 'SupplierScaleNetWeight')));
        setVal('txtBillWeightGrn', f3(pick(row, 'GrnNetBillWeightSum')));
        setSel('CmbFreightAccountPurchase', int(pick(row, 'supplierId')));
        $id('txtBuyerWeight').readOnly = true; $id('txtBuyerWeight').disabled = true;
        setSel('CmbCustomer', int(pick(row, 'BuyerId')));
        setSel('CmbDeliveryTermSale', int(pick(row, 'deliveryTermId')));
        setVal('txtBuyerFirstWeight', f3(pick(row, 'BuyerLoadWeight')));
        setVal('txtBuyerSecondWeight', f3(pick(row, 'BuyerTareWeight')));
        setVal('txtBuyerWeight', f3(pick(row, 'BuyerScaleNetWeight')));
        setVal('txtBillWeightGdn', f3(pick(row, 'BillWeight')));
        setSel('CmbFreightAccountSale', int(pick(row, 'BuyerId')));
        var pTerm = int(pick(row, 'GrnDeliveryTermId')), sTerm = int(pick(row, 'deliveryTermId'));
        var pd = isPonch(pTerm, ''), sd = isPonch(sTerm, '');
        if ((pd && sd) || (!pd && sd)) setVal('txtFreightRemarksSale', pick(row, 'FreightRemarks') || '');
        freightHandling(num(pick(row, 'GrnTotalFreight')), num(pick(row, 'totalFreight')));
        enableAndDisableFields(false);
        var existing = {}; S.rows.forEach(function (r) { existing[int(r.GdnDetailId)] = 1; });
        dt.forEach(function (d) {
            var detId = int(pick(d, 'DetailId'));
            if (existing[detId]) return; existing[detId] = 1;
            var itemId = int(pick(d, 'itemId'));
            var g = function (k) { return pick(d, k); };
            S.rows.push({
                Id: 0, GdnId: int(g('gdnBuyerDispatchMasterId')), GdnDetailId: detId, GdnNo: g('docNo'), GdnDate: g('docDate'),
                GrnId: int(g('grnSupplierLoadingMasterId')), GrnDetailId: g('grnSupplierLoadingDetailId'), GrnNo: g('GrnNo'), GrnDate: g('GrnDate'),
                PoId: int(g('purchaseOrderMasterId')), PoDetailId: g('purchaseOrderDetailId'), PoNo: g('PurchaseOrderNo'), PoDate: g('PurchaseOrderDate'),
                SoId: int(g('saleOrderMasterId')), SoDetailId: g('saleOrderDetailId'), SoNo: g('SaleOrderNo'), SoDate: g('SaleOrderDate'),
                ItemId: itemId, ItemName: itemId > 0 ? g('ItemName') : '', ItemCode: itemId > 0 ? g('ItemCode') : '',
                CropYearId: g('cropYearId'), CropYear: g('cropYear'), PackTypeId: g('packingTypeId'), PackTypeDesc: g('PackingType'),
                UomId: g('packUomId'), UomCode: g('PackUomCode'), PackEquivalent: g('PackUomEquivalent'), ItemQty: g('loadingQty'),
                RefDocNo: g('BuyerRefDocNo'), VehicleNo: g('vehicleNo'), BiltyNo: g('biltyNo'),
                PurchaseGrossWeight: g('GrnGrossWeight'), PurchaseEbCut: g('GrnEbUnit'), PurchaseEbCutTotal: g('GrnEbTotal'),
                PurchaseAddLessWeight: g('GrnAddLessWeight'), PurchaseBillWeight: g('GrnNetBillWeight'),
                PurchaseRateWithoutAddLess: g('PurchaseRate'), PurchaseRateAddLess: null, PurchaseNetRate: g('PurchaseRate'),
                PurchaseRateUomId: g('PurchaseRateUomId'), PurchaseRateUom: g('PurchaseRateUomCode'), PurchaseRateEquivalent: g('PurchaseRateUomEquivalent'),
                PurchaseAmount: g('PurchaseAmount'), PurchaseTaxNameId: g('TaxNameIdPurchase'), PurchaseTaxName: g('TaxNamePurchase'),
                'PurchaseTax%': g('TaxPercentPurchase'), PurchaseTaxAmount: g('TaxAmountPurchase'),
                PurchaseFreight: null, PurchaseFreightLess: null, PurchaseExpenses: null, PurchaseCommission: null, PurchaseBrokery: null, PurchaseNetAmount: null,
                SaleGrossWeight: g('wbGrossWeight'), SaleEbCut: g('ebwPerUnit'), SaleEbCutTotal: g('ebwTotal'), SaleAddLess: g('addLessWeight'),
                SaleBillWeight: g('netBillWeight'), SaleRateWithoutAddLess: g('SaleRate'), SaleRateAddLess: null, SaleNetRate: g('SaleRate'),
                SaleRateUomId: g('SaleRateUomId'), SaleRateUom: g('SaleRateUomCode'), SaleRateEquivalent: g('SaleRateUomEquivalent'),
                SaleAmount: g('SaleAmount'), SaleTaxNameId: g('TaxNameIdSale'), SaleTaxName: g('TaxNameSale'),
                'SaleTax%': g('TaxPercentSale'), SaleTaxAmount: g('TaxAmountSale'),
                SaleExpenses: null, SaleCommission: null, SaleBrokery: null, SaleNetAmount: null, PLAmount: null, RemarksDetail: null
            });
        });
        calculationsCallForDetail();
        loadSaleTaxTypes();
        renderAll();
    }
    /* CalculationsCallForDetail (8646) */
    function calculationsCallForDetail() {
        calculateEbTotalThenBillWeight();
        getSaleExpensesOnBaseOfGdn(); getEmptyBagOnBaseOf('S'); getEmptyBagOnBaseOf('P'); getPurchaseExpensesOnBaseOfGrn();
        getPaymentOnBaseOfOrder('S'); getPaymentOnBaseOfOrder('P');
        getCommissionOnBaseOfOrder('S'); getCommissionOnBaseOfOrder('P');
        initCommissionConfigs(); configs.forEach(calculateCommissionGrid);
        expenseProportionForPurchase(); expenseProportionForSale(); freightProportion();
        supplierBillAmount(); customerBillAmount(); proportionateCommissionAndBrokery();
    }
    function table(i) { return (S.ds && S.ds[i]) || []; }
    /* GetPurchaseExpensesOnBaseOfGrn (8723, Tables[4]) / GetSaleExpensesOnBaseOfGdn (8851, Tables[3]) */
    function applyExpenses(t, idKey, masterKey, noKey, rows) {
        var ids = {}; S.rows.forEach(function (r) { ids[int(r[idKey])] = 1; });
        t.filter(function (x) { return ids[int(pick(x, masterKey))]; }).forEach(function (x) {
            rows.forEach(function (r) {
                if (int(r.ItemId) !== int(pick(x, 'ItemId'))) return;
                r[idKey] = int(pick(x, masterKey)); r[noKey] = pick(x, noKey);
                r.Qty = pick(x, 'Qty'); r.Rate = pick(x, 'rate'); r.Amount = pick(x, 'amount'); r.Remarks = pick(x, 'remarks');
            });
        });
    }
    function getPurchaseExpensesOnBaseOfGrn() { applyExpenses(table(4), 'GrnId', 'grnSupplierLoadingMasterId', 'GrnNo', S.pOther); }
    function getSaleExpensesOnBaseOfGdn() { applyExpenses(table(3), 'GdnId', 'gdnBuyerDispatchMasterId', 'GdnNo', S.sOther); }
    /* GetEmptyBagDetailOnBaseOfGrn (8769, Tables[2]) / ...Gdn (8810, Tables[1]) */
    function getEmptyBagOnBaseOf(side) {
        var p = side === 'P', t = table(p ? 2 : 1), idKey = p ? 'GrnId' : 'GdnId', mk = p ? 'grnSupplierLoadingMasterId' : 'gdnBuyerDispatchMasterId';
        var rows = p ? S.pPm : S.sPm;
        var ids = {}; S.rows.forEach(function (r) { ids[int(r[idKey])] = 1; });
        var f = t.filter(function (x) { return ids[int(pick(x, mk))]; });
        if (!f.length) return;
        if (!rows.some(function (r) { return num(r.Amount) > 0; })) rows.length = 0;
        var have = {}; rows.forEach(function (r) { have[int(r[idKey])] = 1; });
        var qty = sum(S.rows, 'ItemQty');
        f.forEach(function (x) {
            var id = int(pick(x, mk)); if (have[id]) return; have[id] = 1;
            var r = emptyPm(side);
            r[idKey] = id; r[p ? 'GrnNo' : 'GdnNo'] = pick(x, p ? 'GrnNo' : 'GdnNo'); r.AccountId = 0;
            r.PackingTypeId = pick(x, 'PackingTypeId'); r.PmItemId = pick(x, 'emptyBagPackingMaterialItemId');
            r.Qty = qty; r.Rate = pick(x, 'Rate'); r.Amount = qty * num(pick(x, 'Rate')); r.Debit = 0; r.Remarks = '';
            r.EmptyBagTermId = pick(x, 'EBWeightDeductionTermId'); r.EmptyBagTerm = pick(x, 'EmptyBagTerm');
            rows.push(r);
        });
    }
    /* GetSalePaymentDetailOnBaseOfSaleOrder (8897, Tables[6]) / Purchase (8938, Tables[8]) */
    function getPaymentOnBaseOfOrder(side) {
        var p = side === 'P', t = table(p ? 8 : 6), oc = p ? 'PoId' : 'SoId', mk = p ? 'purchaseOrderMasterId' : 'saleOrderMasterId';
        var rows = p ? S.pPay : S.sPay;
        var ids = {}; S.rows.forEach(function (r) { ids[int(r[oc])] = 1; });
        var f = t.filter(function (x) { return ids[int(pick(x, mk))]; });
        if (!f.length) return;
        if (!rows.some(function (r) { return num(r.Amount) > 0; })) rows.length = 0;
        var have = {}; rows.forEach(function (r) { have[int(r[oc]) + '|' + int(r.PaymentTermId)] = 1; });
        f.forEach(function (x) {
            var oid = int(pick(x, mk)), pt = int(pick(x, 'PaymentTermId')), key = oid + '|' + pt;
            if (have[key]) return; have[key] = 1;
            var r = emptyPay(side);
            r[oc] = oid; r[p ? 'PoNo' : 'SoNo'] = pick(x, p ? 'PurchaseOrderNo' : 'SaleOrderNo'); r.PaymentTermId = pt;
            r.DueDays = pick(x, 'DueDays'); r['%OfTotal'] = pick(x, 'pctOfTotal'); r.Amount = pick(x, 'dueAmount');
            r.BaseDateTypeId = pick(x, 'BaseDueDateTypeId'); r.DueDate = d10(pick(x, 'DueDate')) || null;
            rows.push(r);
        });
    }
    /* GetSaleCommissionAndBrokeryOnBaseOfSaleOrder (9043, Tables[5]) / Purchase (8979, Tables[7]) */
    function getCommissionOnBaseOfOrder(side) {
        var p = side === 'P', t = table(p ? 7 : 5), oc = p ? 'PoId' : 'SoId', dk = p ? 'PoNo' : 'SoNo', mk = p ? 'purchaseOrderMasterId' : 'saleOrderMasterId';
        var ids = {}; S.rows.forEach(function (r) { ids[int(r[oc])] = 1; });
        var f = t.filter(function (x) { return ids[int(pick(x, mk))]; });
        if (!f.length) return;
        [[1, p ? 'pComm' : 'sComm', 'CommissionAgentId', 'CommissionTypeId', 'CommissionUomId', 'CommissionRate', 'CommissionAmount'],
         [2, p ? 'pBrk' : 'sBrk', 'BrokeryAgentId', 'BrokeryTypeId', 'BrokeryUomId', 'BrokeryRate', 'BrokeryAmount']].forEach(function (spec) {
            var part = f.filter(function (x) { return int(pick(x, 'agentTypeId')) === spec[0]; });
            if (!part.length) return;
            var rows = S[spec[1]];
            if (!rows.some(function (r) { return num(r[spec[6]]) > 0; })) rows.length = 0;
            var have = {}; rows.forEach(function (r) { have[int(r[oc])] = 1; });
            part.forEach(function (x) {
                var oid = int(pick(x, mk)); if (have[oid]) return; have[oid] = 1;
                var r = {}; r[oc] = oid; r[dk] = pick(x, p ? 'PurchaseOrderNo' : 'SaleOrderNo');
                r[spec[2]] = pick(x, 'commissionAgentId'); r[spec[3]] = pick(x, 'commissionTypeId'); r[spec[4]] = pick(x, 'rateUomId');
                r[spec[5]] = pick(x, 'commissionRate'); r[spec[6]] = pick(x, 'commissionAmount'); r.DebitAccountId = 0;
                rows.push(r);
            });
        });
    }
    /* EnableAndDisableFields (9107) */
    function enableAndDisableFields(flag) {
        ['CmbDeliveryTermSale', 'CmbCustomer', 'CmbSupplier', 'CmbDeliveryTermPurchase', 'CmbEmptyBagTerm', 'CmbEmptyBagTermPurchase', 'txtBuyerWeight']
            .forEach(function (id) { $id(id).disabled = !flag; });
        var ef = flag || !S.cfg.disableFreight;
        ['CmbFreightAccountPurchase', 'txtFreightAmountPurchase', 'txtTotalFreightPurchase', 'txtFreightAmountSale', 'txtTotalFreightSale', 'CmbFreightAccountSale']
            .forEach(function (id) { $id(id).disabled = !ef; });
        $id('txtBillWeightGdn').disabled = !(flag || S.cfg.enableSaleAddLess);
    }

    /* ================================================================== SAVE / UPDATE / DELETE */

    /* the control values Insert() (4928) reads */
    function header() {
        var h = {};
        ['TxtDocNo', 'txtBranchSrNo', 'txtDocDate', 'txtMainRemarks', 'txtDueDaysPurchase', 'txtDueDatePurchase', 'txtPurchaseBillAmount',
            'txtDueDaysSale', 'txtDueDateSale', 'txtSaleBillAmount', 'txtSupplierFirstWeight', 'txtSupplierSecondWeight', 'txtSupplierWeight',
            'txtBillWeightGrn', 'txtBillWeightGdn', 'txtBuyerFirstWeight', 'txtBuyerSecondWeight', 'txtBuyerWeight', 'txtProfitLoss',
            'txtTotalFreightPurchase', 'txtFreightAmountPurchase', 'txtFreightAddLessPurchase', 'txtFreightRemarksPurchase',
            'txtTotalFreightSale', 'txtFreightAmountSale', 'txtFreightAddLessSale', 'txtFreightRemarksSale',
            'txtWhtTaxPercentPurchase', 'txtWhtTaxAmountPurchase', 'txtWhtTaxPercentSale', 'txtwhtTaxAmountSale'].forEach(function (id) { h[id] = val(id); });
        ['CmbTradingAccount', 'CmbSupplier', 'CmbDeliveryTermPurchase', 'CmbPaymentTermPurchase', 'CmbTaxAccountPurchase', 'CmbCustomer',
            'CmbDeliveryTermSale', 'CmbPaymentTermSale', 'CmbTaxAccountSale', 'CmbFreightAccountPurchase', 'CmbFreightAccountSale',
            'CmbWhtAccountPurchase', 'CmbWhtTaxTypePurchase', 'CmbWhtAccountSale', 'CmbWhtTaxTypeSale'].forEach(function (id) { h[id] = int(val(id)); });
        ['CmbDeliveryTermPurchase', 'CmbDeliveryTermSale', 'CmbPaymentTermPurchase', 'CmbPaymentTermSale', 'CmbWhtAccountPurchase',
            'CmbWhtTaxTypePurchase', 'CmbWhtAccountSale', 'CmbWhtTaxTypeSale'].forEach(function (id) { h[id + 'Text'] = selText(id); });
        h.RecId = S.recId;
        return h;
    }
    /* grid text of the combo columns the desktop reads with Cells[x].Text */
    function payload() {
        var pm = function (rows, side) {
            return rows.map(function (r) {
                return Object.assign({}, r, {
                    AccountTitle: nameIn(side === 'P' ? S.L.acc.purchasePm : S.L.acc.salePm, r.AccountId, 'Id', 'AccountTitle'),
                    PackingType: nameIn(S.L.packTypes, r.PackingTypeId), PmItemName: nameIn(side === 'P' ? S.L.pmPurchase : S.L.pmSale, r.PmItemId)
                });
            });
        };
        var comm = function (rows, agentK, typeK, uomK) {
            return rows.map(function (r) {
                return Object.assign({}, r, { AgentText: nameIn(S.L.parties, r[agentK], 'Id', 'CompanyName'), TypeText: typeText(r[typeK]),
                    UomText: uomText(r[uomK]), DebitAccountText: nameIn(S.L.acc.commissionDr, r.DebitAccountId, 'Id', 'AccountTitle') });
            });
        };
        var pay = function (rows) {
            return rows.map(function (r) { return Object.assign({}, r, { PaymentTermText: nameIn(S.L.payTerms, r.PaymentTermId), BaseDateTypeText: nameIn(S.L.baseDate, r.BaseDateTypeId) }); });
        };
        return {
            header: header(), rows: S.rows, removedRows: S.removed,
            purchaseOtherExpenses: S.pOther, saleOtherExpenses: S.sOther,
            purchasePmExpenses: pm(S.pPm, 'P'), salePmExpenses: pm(S.sPm, 'S'),
            purchaseCommission: comm(S.pComm, 'CommissionAgentId', 'CommissionTypeId', 'CommissionUomId'),
            saleCommission: comm(S.sComm, 'CommissionAgentId', 'CommissionTypeId', 'CommissionUomId'),
            purchaseBrokery: comm(S.pBrk, 'BrokeryAgentId', 'BrokeryTypeId', 'BrokeryUomId'),
            saleBrokery: comm(S.sBrk, 'BrokeryAgentId', 'BrokeryTypeId', 'BrokeryUomId'),
            purchasePayment: pay(S.pPay), salePayment: pay(S.sPay)
        };
    }
    /* CommissionAgentAccoutValidation (1785) - the desktop only SHOWS these (it catches its own
       exception), so they are warnings here too; SupplierBillAmount/CustomerBillAmount rerun. */
    function commissionAccountWarnings() {
        var w = [];
        if (int(val('CmbSupplier')) > 0 && int(val('CmbSupplier')) === int(val('CmbCustomer'))) { setSel('CmbCustomer', 0); w.push('Supplier Ac and Customer  Ac cannot be same'); }
        [[S.pComm, 'CommissionAgentId', 'CommissionAmount', 'In Purchase Commission Grid: Commission Agent cannot be the same as the Debit Account'],
         [S.sComm, 'CommissionAgentId', 'CommissionAmount', 'In Sale Commission Grid: Commission Agent cannot be the same as the Debit Account'],
         [S.pBrk, 'BrokeryAgentId', 'BrokeryAmount', 'In Purchase Brokery Grid: Broker cannot be the same as the Debit Account'],
         [S.sBrk, 'BrokeryAgentId', 'BrokeryAmount', 'In Sale Brokery Grid: Broker cannot be the same as the Debit Account']].forEach(function (t) {
            if (w.length) return;
            for (var i = 0; i < t[0].length; i++) {
                var r = t[0][i]; if (num(r[t[2]]) <= 0) continue;
                var pid = S.L.partyByGl[int(r.DebitAccountId)] || 0;
                if (pid === int(r[t[1]])) { w.push('Row ' + (i + 1) + ': ' + t[3]); return; }
            }
        });
        if (w.length) { alert(w[0]); return; }
        supplierBillAmount(); customerBillAmount();
    }
    /* btnsave_Click (4865) / btnUpdate_Click (5827) -> Insert() */
    function save(isUpdate) {
        return withButton(isUpdate ? 'btnUpdate' : 'btnSave', function () {
            if (isUpdate && S.recId === 0) throw new Error('RecId not Found');
            if (!S.rows.length) { alert('Grid Record Not Found'); return; }
            commissionAccountWarnings();
            renderAll();
            if (!confirm(S.recId === 0 ? 'Are you sure to Save' : 'Are you sure to Update')) return;
            return fetchJson(API + '/save', { method: 'POST', headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' }, body: JSON.stringify(payload()) })
                .then(function (res) {
                    if (!res || res.status !== 'SUCCESS') { alert((res && res.message) || 'Save failed.'); return; }
                    alert(res.message);
                    resetForm();
                    return formResetDBCallsAndDefault();
                });
        });
    }
    /* BtnDelete_Click (5843) */
    function del() {
        return withButton('btnDelete', function () {
            if (!S.recId) throw new Error('Record Id Not Found');
            if (!confirm('Are you sure to Delete?')) return;
            return fetchJson(API + '/' + S.recId, { method: 'DELETE' }).then(function (res) {
                if (res && res.status === 'SUCCESS') { alert('Delete Record Successfully'); resetForm(); return formResetDBCallsAndDefault(); }
                alert((res && res.message) || 'Delete failed.');
            });
        });
    }

    /* ================================================================== READ BY ID (5684) */

    function mapDetail(d) {
        var g = function (k) { return pick(d, k); };
        return { Id: g('Id'), GrnId: g('grnSupplierLoadingMasterId'), GrnDetailId: g('grnSupplierLoadingDetailId'), GrnNo: g('GrnNo'), GrnDate: g('GrnDate'),
            GdnId: g('gdnBuyerDispatchMasterId'), GdnDetailId: g('gdnBuyerDispatchDetailId'), GdnNo: g('GdnNo'), GdnDate: g('GdnDate'),
            PoId: g('purchaseOrderMasterId'), PoDetailId: g('purchaseOrderDetailId'), PoNo: g('PurchaseOrderNo'), PoDate: g('PurchaseOrderDate'),
            SoId: g('saleOrderMasterId'), SoDetailId: g('saleOrderDetailId'), SoNo: g('SaleOrderNo'), SoDate: g('SaleOrderDate'),
            ItemId: g('ItemId'), ItemCode: g('ItemCode'), ItemName: g('ItemName'), CropYearId: g('Crop'), CropYear: g('CropYear'),
            PackTypeId: g('PackingTypeId'), PackTypeDesc: g('PackTypeDesc'), UomId: g('PackUomId'), UomCode: g('PackUom'), PackEquivalent: g('PackUomEquivalent'),
            ItemQty: g('Qty'), RefDocNo: g('RefDocNo'), VehicleNo: g('VehicleNo'), BiltyNo: g('BiltyNo'),
            PurchaseGrossWeight: g('GrossWeight'), PurchaseEbCut: g('EbCut'), PurchaseEbCutTotal: g('EbCutTotal'), PurchaseAddLessWeight: g('AddLessPurchase'),
            PurchaseBillWeight: g('BillWeight'), PurchaseRateWithoutAddLess: g('PurchaseRateWithoutAddLess'), PurchaseRateAddLess: g('PurchaseRateAddLess'),
            PurchaseNetRate: g('RatePurchase'), PurchaseRateUomId: g('RateUomId'), PurchaseRateUom: g('RateUom'), PurchaseRateEquivalent: g('RateUomEquivalent'),
            PurchaseAmount: g('ItemAmount'), PurchaseTaxNameId: g('TaxNameIdPurchase'), PurchaseTaxName: g('TaxNamePurchase'), 'PurchaseTax%': g('TaxPercentPurchase'),
            PurchaseTaxAmount: g('TaxAmountPurchase'), PurchaseFreight: g('PurchaseFreight'), PurchaseFreightLess: g('PurchaseFreightLess'),
            PurchaseExpenses: g('PurchaseExpenses'), PurchaseCommission: g('PurchaseCommission'), PurchaseBrokery: g('PurchaseBrokery'), PurchaseNetAmount: g('PurchaseNetAmount'),
            SaleGrossWeight: g('SaleGrossWeight'), SaleEbCut: g('SaleEbCut'), SaleEbCutTotal: g('SaleEbCutTotal'), SaleAddLess: g('AddLessSale'), SaleBillWeight: g('SaleBillWeight'),
            SaleRateWithoutAddLess: g('SaleRateWithoutAddLess'), SaleRateAddLess: g('SaleRateAddLess'), SaleNetRate: g('RateSale'), SaleRateUomId: g('SaleRateUomId'),
            SaleRateUom: g('SaleRateUom'), SaleRateEquivalent: g('SaleRateUomEquivalent'), SaleAmount: g('SaleAmount'), SaleTaxNameId: g('TaxNameIdSale'),
            SaleTaxName: g('TaxNameSale'), 'SaleTax%': g('TaxPercentSale'), SaleTaxAmount: g('TaxAmountSale'), SaleExpenses: g('SaleExpenses'),
            SaleCommission: g('SaleCommission'), SaleBrokery: g('SaleBrokery'), SaleNetAmount: g('SaleNetAmount'), PLAmount: 0, RemarksDetail: g('RemarksDetail') };
    }
    /* TradeBill_Helper.Fill*TableFromListCommonForReadById: the ReadById lists as the page's grid rows */
    function recordGrids(o, T) {
        T.rows = (pick(o, 'InvCommAgentTradeBillDetailslist') || []).map(mapDetail);
        T.pOther = otherExpenseRows();
        (pick(o, 'InvCommAgentTradePurchaseExpList') || []).forEach(function (d) {
            var r = T.pOther.find(function (x) { return int(x.ItemId) === int(pick(d, 'InvExpItemId')); });
            if (r) Object.assign(r, { GrnId: pick(d, 'grnSupplierLoadingMasterId'), GrnNo: pick(d, 'GrnNo'), ItemName: pick(d, 'OtherItemName'), Qty: pick(d, 'Qty'), Rate: pick(d, 'Rate'), Amount: pick(d, 'Amount'), Remarks: pick(d, 'Remarks') });
        });
        T.sOther = otherExpenseRows();
        (pick(o, 'InvCommAgentTradeSaleExpList') || []).forEach(function (d) {
            var r = T.sOther.find(function (x) { return int(x.ItemId) === int(pick(d, 'InvExpItemId')); });
            if (r) Object.assign(r, { GdnId: pick(d, 'gdnBuyerDispatchMasterId'), GdnNo: pick(d, 'GdnNo'), ItemName: pick(d, 'OtherItemName'), Qty: pick(d, 'Qty'), Rate: pick(d, 'Rate'), Amount: pick(d, 'Amount'), Remarks: pick(d, 'Remarks') });
        });
        var pmMap = function (d, side) {
            var r = emptyPm(side);
            if (side === 'P') { r.GrnId = pick(d, 'grnSupplierLoadingMasterId'); r.GrnNo = pick(d, 'GrnNo'); } else { r.GdnId = pick(d, 'gdnBuyerDispatchMasterId'); r.GdnNo = pick(d, 'GdnNo'); }
            Object.assign(r, { AccountId: pick(d, 'AccountId'), PackingTypeId: pick(d, 'PackingTypeId'), PmItemId: pick(d, 'PmItemId'), Qty: pick(d, 'Qty'), Rate: pick(d, 'Rate'),
                Amount: pick(d, 'Amount'), Debit: 0, Remarks: pick(d, 'Remarks'), EmptyBagTermId: pick(d, 'EBWeightDeductionTermId'), EmptyBagTerm: pick(d, 'EBWeightDeductionTerm') });
            return r;
        };
        T.pPm = (pick(o, 'InvCommAgentTradeFreightExpList') || []).map(function (d) { return pmMap(d, 'P'); });
        T.sPm = (pick(o, 'CommisionAgentBillSaleExpenseCreditToReleventAcsList') || []).map(function (d) { return pmMap(d, 'S'); });
        var comm = pick(o, 'CommisionAgentBillCommissionDetailList') || [];
        var cm = function (side, type, oc, dk, a, t, u, rt, am) {
            return comm.filter(function (d) { return int(pick(d, 'EntrySideId')) === side && int(pick(d, 'agentTypeId')) === type; }).map(function (d) {
                var r = {}; r[oc] = pick(d, side === 1 ? 'purchaseOrderMasterId' : 'saleOrderMasterId'); r[dk] = pick(d, side === 1 ? 'PurchaseOrderNo' : 'SaleOrderNo');
                r[a] = pick(d, 'commissionAgentId'); r[t] = pick(d, 'commissionTypeId'); r[u] = pick(d, 'rateUomId'); r[rt] = pick(d, 'commissionRate');
                r[am] = pick(d, 'commissionAmount'); r.DebitAccountId = pick(d, 'debitAccountId'); return r;
            });
        };
        T.pComm = cm(1, 1, 'PoId', 'PoNo', 'CommissionAgentId', 'CommissionTypeId', 'CommissionUomId', 'CommissionRate', 'CommissionAmount');
        T.pBrk = cm(1, 2, 'PoId', 'PoNo', 'BrokeryAgentId', 'BrokeryTypeId', 'BrokeryUomId', 'BrokeryRate', 'BrokeryAmount');
        T.sComm = cm(2, 1, 'SoId', 'SoNo', 'CommissionAgentId', 'CommissionTypeId', 'CommissionUomId', 'CommissionRate', 'CommissionAmount');
        T.sBrk = cm(2, 2, 'SoId', 'SoNo', 'BrokeryAgentId', 'BrokeryTypeId', 'BrokeryUomId', 'BrokeryRate', 'BrokeryAmount');
        var pays = pick(o, 'CommisionAgentBillPaymentDetailList') || [];
        var pm2 = function (side) {
            return pays.filter(function (d) { return int(pick(d, 'EntrySideId')) === side; }).map(function (d) {
                var r = emptyPay(side === 1 ? 'P' : 'S');
                if (side === 1) { r.PoId = pick(d, 'purchaseOrderMasterId'); r.PoNo = pick(d, 'PurchaseOrderNo'); } else { r.SoId = pick(d, 'saleOrderMasterId'); r.SoNo = pick(d, 'SaleOrderNo'); }
                Object.assign(r, { Id: pick(d, 'Id'), PaymentTermId: pick(d, 'PaymentTermId'), DueDays: pick(d, 'DueDays'), '%OfTotal': pick(d, 'pctOfTotal'),
                    Amount: pick(d, 'dueAmount'), BaseDateTypeId: pick(d, 'BaseDueDateTypeId'), DueDate: d10(pick(d, 'DueDate')) || null });
                return r;
            });
        };
        T.pPay = pm2(1); T.sPay = pm2(2);
        return T;
    }
    function loadRecord(id) {
        return fetchJson(API + '/' + id).then(function (res) {
            if (!res || res.status !== 'SUCCESS') { alert((res && res.message) || 'Record not found'); return; }
            var o = res.data;
            resetForm();
            S.recId = int(pick(o, 'Id'));
            setVal('TxtDocNo', pick(o, 'DocNo')); setVal('txtBranchSrNo', pick(o, 'BranchSrNo')); setVal('txtDocDate', d10(pick(o, 'DocDate')));
            setSel('CmbTradingAccount', pick(o, 'TradingGlAccountId')); setVal('txtMainRemarks', pick(o, 'RemarksHeader') || '');
            setSel('CmbSupplier', pick(o, 'SupplierId'));
            if (int(pick(o, 'DeliveryTermPurchaseId')) > 0) setSel('CmbDeliveryTermPurchase', pick(o, 'DeliveryTermPurchaseId')); else setSelByText('CmbDeliveryTermPurchase', pick(o, 'DeliveryTerm'));
            if (int(pick(o, 'PurchaseTaxAccountId')) > 0) setSel('CmbTaxAccountPurchase', pick(o, 'PurchaseTaxAccountId'));
            setSel('CmbPaymentTermPurchase', pick(o, 'PaymentTermId')); setVal('txtDueDaysPurchase', pick(o, 'DueDays')); setVal('txtDueDatePurchase', d10(pick(o, 'DueDate')));
            setVal('txtPurchaseBillAmount', fmtBoth(pick(o, 'PurchaseBillAmount')));
            setVal('txtSupplierFirstWeight', f3(pick(o, 'SupplierFirstWeight'))); setVal('txtSupplierSecondWeight', f3(pick(o, 'SupplierSecondWeight')));
            setVal('txtSupplierWeight', f3(pick(o, 'SupplierNetWeight'))); setVal('txtBillWeightGrn', f3(pick(o, 'SupplierBillWeight')));
            setSel('CmbCustomer', pick(o, 'CustomerId'));
            if (int(pick(o, 'DeliveryTermSaleId')) > 0) setSel('CmbDeliveryTermSale', pick(o, 'DeliveryTermSaleId')); else setSelByText('CmbDeliveryTermSale', pick(o, 'DeliveryTermSale'));
            if (int(pick(o, 'SaleTaxAccountId')) > 0) setSel('CmbTaxAccountSale', pick(o, 'SaleTaxAccountId'));
            setSel('CmbPaymentTermSale', pick(o, 'SalePaymentTermId')); setVal('txtDueDaysSale', pick(o, 'SaleDueDays')); setVal('txtDueDateSale', d10(pick(o, 'SaleDueDate')));
            setVal('txtSaleBillAmount', fmtBoth(pick(o, 'SaleBillAmount')));
            setVal('txtBuyerFirstWeight', f3(pick(o, 'BuyerFirstWeight'))); setVal('txtBuyerSecondWeight', f3(pick(o, 'BuyerSecondWeight')));
            setVal('txtBuyerWeight', f3(pick(o, 'BuyerNetWeight'))); setVal('txtBillWeightGdn', f3(pick(o, 'BuyerBillWeight')));
            setVal('txtProfitLoss', pick(o, 'PLAmount'));
            recordGrids(o, S);
            enableAndDisableFields(!S.rows.some(function (r) { return int(r.GdnId) > 0; }));
            bindGrids();
            var fr = pick(o, 'CommisionAgentBillFreightDetailList') || [];
            var fp = fr.find(function (x) { return int(pick(x, 'EntrySideId')) === 1; }), fs = fr.find(function (x) { return int(pick(x, 'EntrySideId')) === 2; });
            if (fp) { setVal('txtFreightAmountPurchase', f3(pick(fp, 'Amount'))); setVal('txtFreightAddLessPurchase', f3(pick(fp, 'AddLessAmount'))); setVal('txtTotalFreightPurchase', f3(pick(fp, 'NetAmount'))); setVal('txtFreightRemarksPurchase', pick(fp, 'Remarks') || ''); setSel('CmbFreightAccountPurchase', pick(fp, 'AccountId')); }
            if (fs) { setVal('txtFreightAmountSale', f3(pick(fs, 'Amount'))); setVal('txtFreightAddLessSale', f3(pick(fs, 'AddLessAmount'))); setVal('txtTotalFreightSale', f3(pick(fs, 'NetAmount'))); setVal('txtFreightRemarksSale', pick(fs, 'Remarks') || ''); setSel('CmbFreightAccountSale', pick(fs, 'AccountId')); }
            var tx = pick(o, 'CommisionAgentBillTaxDetailList') || [];
            var tp = tx.find(function (x) { return int(pick(x, 'EntrySideId')) === 1; }), ts = tx.find(function (x) { return int(pick(x, 'EntrySideId')) === 2; });
            if (tp) { setVal('txtWhtTaxPercentPurchase', f3(pick(tp, 'TaxPercantage'))); setVal('txtWhtTaxAmountPurchase', f3(pick(tp, 'TaxAmount'))); setSel('CmbWhtAccountPurchase', pick(tp, 'TaxAccountId')); setSel('CmbWhtTaxTypePurchase', pick(tp, 'TaxTypeId')); }
            return loadSaleTaxTypes(true).then(function () {
                if (ts) { setVal('txtWhtTaxPercentSale', f3(pick(ts, 'TaxPercantage'))); setVal('txtwhtTaxAmountSale', f3(pick(ts, 'TaxAmount'))); setSel('CmbWhtAccountSale', pick(ts, 'TaxAccountId')); setSel('CmbWhtTaxTypeSale', pick(ts, 'TaxTypeId')); }
                expenseProportionForPurchase(); expenseProportionForSale(); freightProportion();
                initCommissionConfigs(); configs.forEach(calculateCommissionGrid); proportionateCommissionAndBrokery();
                supplierBillAmount(); customerBillAmount(); plAmountProportion(); calculateHeaderRates();
                renderAll(); applyButtonState(); showTab('mainTabs', 'paneForm');
            });
        });
    }

    /* ================================================================== HISTORY (HistoryGridFill 5943) */

    var histRows = [], histSel = -1;
    var HIST = [{ k: 'DocNo', cap: 'DocNo', link: 'Edit' }, { k: 'BranchSrNo', cap: 'BranchSrNo' }, { k: 'DocDate', cap: 'DocDate', date: 1 },
        { k: 'SupplierName', cap: 'Supplier' }, { k: 'CustomerName', cap: 'Customer' }, { k: 'RefDocNos', cap: 'RefDocNos' }, { k: 'VehicleNos', cap: 'VehicleNos' },
        { k: 'DeliveryTerm', cap: 'PurDeliveryTerm' }, { k: 'DeliveryTermSale', cap: 'SaleDeliveryTerm' },
        { k: 'PurchaseBillAmount', cap: 'PurchaseBillAmount', n: 2, sum: 1 }, { k: 'SaleBillAmount', cap: 'SaleBillAmount', n: 2, sum: 1 },
        { k: 'PLAmount', cap: 'PLAmount', n: 2, sum: 1 }, { k: 'RemarksHeader', cap: 'Remarks' }, { k: 'EntryUserName', cap: 'EntryUser' },
        { k: 'EnteryDate', cap: 'EntryDate' }, { k: 'ModifyUserName', cap: 'ModifyUser' }, { k: 'ModifyDate', cap: 'ModifyDate' }, { k: 'NoOfAttachments', cap: 'NoOfAttachments' }];
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    /* FormatString "dd-MMM-yyyy hh:mm tt" (HistoryGridSettings 6059/6061) */
    function dmyhm(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})[T ](\d{2}):(\d{2})/.exec(String(v || ''));
        if (!m) return v ? String(v) : '';
        var h = +m[4], ap = h >= 12 ? 'PM' : 'AM'; h = h % 12 || 12;
        return m[3] + '-' + MON[+m[2] - 1] + '-' + m[1] + ' ' + ('0' + h).slice(-2) + ':' + m[5] + ' ' + ap;
    }
    /* HistoryGridFill (5943): the date pair goes to Doc / Entry / Modify date by the radio; an
       unticked picker (ShowCheckBox) is not sent. */
    function showHistory() {
        return withButton('btnShowHistory', function () {
            var q = [], kind = $id('rdentrydate').checked ? 'entry' : ($id('rdmodifydate').checked ? 'modify' : 'doc');
            q.push('dateKind=' + kind);
            if ($id('chkHistFrom').checked && val('histFromDate')) q.push('fromDate=' + val('histFromDate'));
            if ($id('chkHistTo').checked && val('histToDate')) q.push('toDate=' + val('histToDate'));
            if (int(val('histDocNoFrom'))) q.push('docNoFrom=' + int(val('histDocNoFrom')));
            if (int(val('histDocNoTo'))) q.push('docNoTo=' + int(val('histDocNoTo')));
            if (int(val('cmbtradAcHistory'))) q.push('tradingAccountId=' + int(val('cmbtradAcHistory')));
            if (int(val('cmbsupplierhistory'))) q.push('supplierId=' + int(val('cmbsupplierhistory')));
            if (int(val('cmbcusthistory'))) q.push('customerId=' + int(val('cmbcusthistory')));
            return fetchJson(API + '/history?' + q.join('&')).then(function (rows) {
                histRows = (rows || []).map(function (r) {
                    var o = {}; HIST.forEach(function (c) { o[c.k] = pick(r, c.k); }); o.Id = pick(r, 'Id');
                    o.EnteryDate = dmyhm(o.EnteryDate); o.ModifyDate = dmyhm(o.ModifyDate);
                    return o;
                });
                histSel = -1;
                renderGrid('GrdHistory', [{ btn: 'Edit', label: 'Edit' }].concat(HIST), histRows);
                if (!histRows.length) {
                    $id('GrdHistory').innerHTML = '';                                   /* GrdHistory.ClearStructure() */
                    clearHistoryDetail();
                }
            });
        });
    }
    function historyButton(act, i) { var r = histRows[i]; if (r && act === 'Edit') loadRecord(int(r.Id)); }
    var HIST_GRIDS = ['GrdHistoryDetail', 'GrdHistoryPurchaseExpenseDetail', 'GrdHistoryPurchaseFreightDetail', 'GrdHistorySaleExpenseDetail',
        'GrdHistorySalePmDetail', 'GrdHistoryPurchaseCommissionDetail', 'GrdHistorySaleCommissionDetail', 'GrdHistoryPurchaseBrokeryDetail',
        'GrdHistorySaleBrokeryDetail', 'GrdHistoryPaymentDetailPurchase', 'GrdHistoryPaymentDetailSale'];
    function clearHistoryDetail() { HIST_GRIDS.forEach(function (id) { var t = $id(id); if (t) t.innerHTML = ''; }); }
    /* the same column sets, read-only (the history grids are not edited) */
    function ro(cols) { return cols.filter(function (c) { return !c.btn; }).map(function (c) { return Object.assign({}, c, { edit: false, link: null }); }); }
    /* GrdHistory_SelectionChanged (6329): GetByID, then every child grid of the selected bill */
    var histDetailSeq = 0;
    function historySelect(i) {
        var r = histRows[i]; if (!r) return;
        histSel = i;
        var trs = $id('GrdHistory').querySelectorAll('tbody tr');
        trs.forEach(function (tr, j) { tr.classList.toggle('sel', j === i); });
        var seq = ++histDetailSeq;
        return fetchJson(API + '/' + int(r.Id)).then(function (res) {
            if (seq !== histDetailSeq) return;
            if (!res || res.status !== 'SUCCESS') { clearHistoryDetail(); return; }
            var T = recordGrids(res.data, {});
            var g = !!T.rows.some(function (x) { return int(x.GdnId) > 0; });
            var saved = { rows: S.rows, pOther: S.pOther, sOther: S.sOther, pPm: S.pPm, sPm: S.sPm };
            /* detailCols/expenseCols/pmCols read S.* to decide hidden doc columns - swap in the history rows while building */
            S.rows = T.rows; S.pOther = T.pOther; S.sOther = T.sOther; S.pPm = T.pPm; S.sPm = T.sPm;
            try {
                renderGrid('GrdHistoryDetail', ro(detailCols()).filter(function (c) { return g || !/^(GdnNo|GrnNo|PoNo|SoNo)$/.test(c.k); }), T.rows);
                renderGrid('GrdHistoryPurchaseExpenseDetail', ro(expenseCols('P')), T.pOther);
                renderGrid('GrdHistorySaleExpenseDetail', ro(expenseCols('S')), T.sOther);
                renderGrid('GrdHistoryPurchaseFreightDetail', ro(pmCols('P')), T.pPm);
                renderGrid('GrdHistorySalePmDetail', ro(pmCols('S')), T.sPm);
            } finally { S.rows = saved.rows; S.pOther = saved.pOther; S.sOther = saved.sOther; S.pPm = saved.pPm; S.sPm = saved.sPm; }
            renderGrid('GrdHistoryPurchaseCommissionDetail', ro(commCols('PoId', 'PoNo', 'CommissionAgentId', 'CommissionTypeId', 'CommissionUomId', 'CommissionRate', 'CommissionAmount')), T.pComm);
            renderGrid('GrdHistorySaleCommissionDetail', ro(commCols('SoId', 'SoNo', 'CommissionAgentId', 'CommissionTypeId', 'CommissionUomId', 'CommissionRate', 'CommissionAmount')), T.sComm);
            renderGrid('GrdHistoryPurchaseBrokeryDetail', ro(commCols('PoId', 'PoNo', 'BrokeryAgentId', 'BrokeryTypeId', 'BrokeryUomId', 'BrokeryRate', 'BrokeryAmount')), T.pBrk);
            renderGrid('GrdHistorySaleBrokeryDetail', ro(commCols('SoId', 'SoNo', 'BrokeryAgentId', 'BrokeryTypeId', 'BrokeryUomId', 'BrokeryRate', 'BrokeryAmount')), T.sBrk);
            renderGrid('GrdHistoryPaymentDetailPurchase', ro(payCols('P')), T.pPay);
            renderGrid('GrdHistoryPaymentDetailSale', ro(payCols('S')), T.sPay);
        });
    }
    /* ParameterFill -> InfragisticsHelper.BindComboDateType; cmbperemeter_ValueChanged (6158) */
    function parameterFill() {
        fill('cmbperemeter', [{ id: 1, name: 'This Day' }, { id: 2, name: 'This Week' }, { id: 3, name: 'This Month' }, { id: 4, name: 'This Year' }, { id: 5, name: 'Financial Year' }], 'id', 'name');
        setSel('cmbperemeter', 0);
    }
    function dateTypeChanged() {
        var v = int(val('cmbperemeter')), t = new Date();
        if (v === 1) setVal('histFromDate', ymd(t));
        else if (v === 2) setVal('histFromDate', ymd(new Date(t.getFullYear(), t.getMonth(), t.getDate() - 7)));
        else if (v === 3) { setVal('histFromDate', ymd(new Date(t.getFullYear(), t.getMonth(), 1))); setVal('histToDate', ymd(t)); }
        else if (v === 4) { setVal('histFromDate', ymd(new Date(t.getFullYear(), 0, 1))); setVal('histToDate', ymd(t)); }
        else if (v === 5) {
            getJson(API + '/lookups/year-start').then(function (r) { if (r && r.financialYearStart) setVal('histFromDate', String(r.financialYearStart).substring(0, 10)); setVal('histToDate', ymd(new Date())); });
        }
    }
    /* btnNewHistory_Click (6196) */
    function newHistory() {
        parameterFill();
        ['histDocNoFrom', 'histDocNoTo'].forEach(function (id) { setVal(id, ''); });
        ['cmbtradAcHistory', 'cmbsupplierhistory', 'cmbcusthistory'].forEach(function (id) { setSel(id, 0); });
        try { $id('cmbperemeter').focus(); } catch (x) { /* ignore */ }
    }

    /* ================================================================== RESET / INIT */

    /* ResetForm (1589) */
    function resetForm() {
        S.recId = 0; S.rows = []; S.removed = []; S.ds = null;
        S.pOther = []; S.sOther = []; S.pPm = []; S.sPm = []; S.pComm = []; S.sComm = []; S.pBrk = []; S.sBrk = []; S.pPay = []; S.sPay = [];
        enableAndDisableFields(true);
        $id('txtBuyerWeight').readOnly = false; $id('txtBuyerWeight').disabled = false;
        setVal('TxtDocNo', '0'); setVal('txtBranchSrNo', '0');
        ['txtBuyerFirstWeight', 'txtBuyerSecondWeight', 'txtSupplierFirstWeight', 'txtSupplierSecondWeight', 'txtBillWeightGrn', 'txtBillWeightGdn',
            'txtPurchaseBillAmount', 'txtSaleBillAmount', 'txtProfitLoss', 'txtMainRemarks', 'txtFreightRemarksPurchase', 'txtFreightRemarksSale',
            'txtSupplierWeight', 'txtBuyerWeight'].forEach(function (id) { setVal(id, ''); });
        ['txtFreightAmountPurchase', 'txtFreightAddLessPurchase', 'txtTotalFreightPurchase', 'txtFreightAmountSale', 'txtFreightAddLessSale', 'txtTotalFreightSale',
            'txtWhtTaxPercentPurchase', 'txtWhtTaxAmountPurchase', 'txtWhtTaxPercentSale', 'txtwhtTaxAmountSale'].forEach(function (id) { setVal(id, '0'); });
        ['CmbFreightAccountPurchase', 'CmbFreightAccountSale', 'CmbWhtAccountPurchase', 'CmbWhtTaxTypePurchase', 'CmbWhtAccountSale', 'CmbWhtTaxTypeSale'].forEach(function (id) { setSel(id, 0); });
        S.showFreight = true; S.showFreightLess = false;
        D.upd = -1; detailButtons(false); detailFormReset();                       /* ResetForm 1597-1600, 1637 */
        $id('PanelDetailInfo').hidden = false;
        bindGrids();
        renderAll(); applyButtonState();
    }
    /* FormResetDBCallsAndDefault (1659) */
    function formResetDBCallsAndDefault() {
        return getJson(API + '/generate-no').then(function (r) {
            if (r) { setVal('TxtDocNo', r.docNo); setVal('txtBranchSrNo', r.branchSrNo); applyButtonState(); }
            applyCommissionDefaults();
        });
    }
    /* GetCommissionAgentConfigurationsFromGlobalandBind (1077) */
    function applyCommissionDefaults() {
        var c = S.cfg.raw || {};
        var g = function (k) { return int(c[k]); };
        if (g('DefaultTradingAccountIdForCommissionAgentPortal') > 0) setSel('CmbTradingAccount', g('DefaultTradingAccountIdForCommissionAgentPortal'));
        if (g('DefaultTaxAccountIdForCommissionAgentPortal') > 0) { setSel('CmbTaxAccountPurchase', g('DefaultTaxAccountIdForCommissionAgentPortal')); setSel('CmbTaxAccountSale', g('DefaultTaxAccountIdForCommissionAgentPortal')); }
        if (g('DefaultPaymentTermIdForCommissionAgentPortal') > 0) { setSel('CmbPaymentTermSale', g('DefaultPaymentTermIdForCommissionAgentPortal')); setSel('CmbPaymentTermPurchase', g('DefaultPaymentTermIdForCommissionAgentPortal')); }
        paymentTermChanged('P'); paymentTermChanged('S');
        var dt = g('DefaultDeliveryTermIdForCommissionAgentPortal');
        if (dt > 0) { if (!int(val('CmbDeliveryTermPurchase'))) setSel('CmbDeliveryTermPurchase', dt); if (!int(val('CmbDeliveryTermSale'))) setSel('CmbDeliveryTermSale', dt); }
        if (g('DefaultCropYearIdForCommissionAgentPortal') > 0) setSel('CmbCropYear', g('DefaultCropYearIdForCommissionAgentPortal'));      /* 1110 */
        if (g('DefaultPackingTypeIdForCommissionAgentPortal') > 0) setSel('CmbPackType', g('DefaultPackingTypeIdForCommissionAgentPortal')); /* 1115 */
        if (g('DefaultWhtAccountPurchaseIdForCommissionAgentPortal') > 0) setSel('CmbWhtAccountPurchase', g('DefaultWhtAccountPurchaseIdForCommissionAgentPortal'));
        if (g('DefaultWhtAccountSaleIdForCommissionAgentPortal') > 0) setSel('CmbWhtAccountSale', g('DefaultWhtAccountSaleIdForCommissionAgentPortal'));
        if (g('DefaultTaxTypePurchaseeIdForCommissionAgentPortal') > 0) { setSel('CmbWhtTaxTypePurchase', g('DefaultTaxTypePurchaseeIdForCommissionAgentPortal')); purchaseTaxCalculation(); }
        if (g('DefaultTaxTypeSaleIdForCommissionAgentPortal') > 0) setSel('CmbWhtTaxTypeSale', g('DefaultTaxTypeSaleIdForCommissionAgentPortal'));
        $id('txtBillWeightGdn').disabled = !(!hasGdn() || S.cfg.enableSaleAddLess);
    }
    /* CmbPaymentTermPurchase_ValueChanged (7868) / CmbPaymentTermSale_TextChanged (7908) */
    function paymentTermChanged(side) {
        var p = side === 'P', t = int(val(p ? 'CmbPaymentTermPurchase' : 'CmbPaymentTermSale')), d = p ? 'txtDueDaysPurchase' : 'txtDueDaysSale';
        $id(d).disabled = false;
        if (t === 1 || t === 3) { setVal(d, '0'); $id(d).disabled = true; }
        else if (t === 2 && int(val(d)) === 0) setVal(d, '2');
        dueDaysChanged(side);
    }
    /* txtDueDaysPurchase_TextChanged (7889) / txtDueDaysSale_TextChanged (7929) */
    function dueDaysChanged(side) {
        var p = side === 'P', d = val(p ? 'txtDueDaysPurchase' : 'txtDueDaysSale').trim();
        setVal(p ? 'txtDueDatePurchase' : 'txtDueDateSale', d !== '' ? addDays(val('txtDocDate'), num(d)) : val('txtDocDate'));
    }
    /* PurchaseTaxCalculation (9747) */
    function purchaseTaxCalculation() {
        var id = int(val('CmbWhtTaxTypePurchase'));
        return getJson(API + '/lookups/wht-percent?taxNameId=' + id + '&docDate=' + encodeURIComponent(val('txtDocDate'))).then(function (r) {
            setVal('txtWhtTaxPercentPurchase', f4(r ? r.taxPercent : 0));
            supplierBillAmount(); renderAll();
        });
    }
    /* SaleTaxTypesBind (1244) -> CmbWhtTaxTypeSale_Leave -> SaleTaxCalculation (9780) */
    function loadSaleTaxTypes(skipCalc) {
        return getJson(API + '/lookups/sale-tax-types?customerId=' + int(val('CmbCustomer')) + '&docDate=' + encodeURIComponent(val('txtDocDate'))).then(function (rows) {
            S.L.saleTaxTypes = rows || [];
            fill('CmbWhtTaxTypeSale', S.L.saleTaxTypes, 'Id', 'TaxName');
            if (!skipCalc) saleTaxCalculation();
        });
    }
    function saleTaxCalculation() {
        var id = int(val('CmbWhtTaxTypeSale')), pct = 0;
        if (id > 0) { var r = S.L.saleTaxTypes.find(function (x) { return int(pick(x, 'Id')) === id; }); pct = r ? num(pick(r, 'TaxPercent')) : 0; }
        setVal('txtWhtTaxPercentSale', f4(pct));
        customerBillAmount(); renderAll();
    }
    /* CheckSupplierAndFreightGlAccount (1758) / CheckCustomerAndPMSaleGlAccount (2036) */
    function checkSupplierGl() {
        if (!(S.pPm.length > 0 && int(val('CmbSupplier')) > 0 && purchaseDelivered())) return;
        var g = glOf(val('CmbSupplier'));
        for (var i = 0; i < S.pPm.length; i++) if (int(S.pPm[i].AccountId) === g) { setSel('CmbSupplier', 0); alert("Freight Account And Supplier Gl Account Can't be Same. First Delete Freight Grid Row#" + (i + 1) + ' Or Change the Account in Freight Grid'); break; }
    }
    function checkCustomerGl() {
        if (!(S.sPm.length > 0 && int(val('CmbCustomer')) > 0 && saleXFactory())) return;
        var g = glOf(val('CmbCustomer'));
        for (var i = 0; i < S.sPm.length; i++) if (int(S.sPm[i].AccountId) === g) { setSel('CmbCustomer', 0); alert("PM Sale Account And Customer Gl Account Can't be Same"); break; }
    }
    function showTab(stripId, paneId) {
        $id(stripId).querySelectorAll('button[data-pane]').forEach(function (b) {
            var on = b.getAttribute('data-pane') === paneId; b.classList.toggle('active', on);
            $id(b.getAttribute('data-pane')).classList.toggle('active', on);
        });
    }

    /* RadBusinessNamePurchase_CheckedChanged (9801) / RadBusinessNameSale_CheckedChanged (9807):
       SupplierDtFillFromGlobal(dt, businessName, radPartyCode) then SupplierBind / BuyerBind. Code
       forces business names and shows PartyCode in the party combo; the freight combo keeps names. */
    function bindParties(side) {
        var p = side === 'P';
        var nick = $id(p ? 'RadNickNamePurchase' : 'RadNickNameSale').checked, code = $id(p ? 'radPartyCodePurchase' : 'radPartyCodeSale').checked;
        var list = (nick && !code) ? (S.L.partiesNick.length ? S.L.partiesNick : S.L.parties) : S.L.parties;
        var main = p ? 'CmbSupplier' : 'CmbCustomer', fr = p ? 'CmbFreightAccountPurchase' : 'CmbFreightAccountSale';
        var mv = val(main), fv = val(fr);
        fill(main, list, 'Id', code ? 'PartyCode' : 'CompanyName', true, partyAttrs);
        fill(fr, list, 'Id', 'CompanyName', true, partyAttrs);
        setSel(main, mv); setSel(fr, fv);
        var cap = $id(main); if (cap) cap.setAttribute('data-dtcombo-caption', code ? 'Party Code' : (p ? 'Supplier Name' : 'Buyer Name'));
    }
    /* InitializeComponentMethod (918): every combo from its desktop source */
    function loadLookups() {
        var t = new Date();
        setVal('txtDocDate', ymd(t)); setVal('histToDate', ymd(t)); setVal('histFromDate', ymd(new Date(t.getFullYear(), t.getMonth(), t.getDate() - 3)));
        return Promise.all([
            getJson(DD + '/suppliers?useBusinessName=true'), getJson(DD + '/payment-terms'), getJson(DD + '/delivery-terms'),
            getJson(DD + '/other-items'), getJson(DD + '/view-combos'), getJson(DD + '/empty-bag-items?transactionFlowId=1'),
            getJson(DD + '/empty-bag-items?transactionFlowId=2'), getJson(API + '/lookups/accounts'), getJson(API + '/lookups/tax-types'),
            getJson(API + '/lookups/configs'), getJson(DD + '/companies'), getJson(DD + '/branches'),
            getJson(DD + '/suppliers?useBusinessName=false'), getJson(DD + '/items'), getJson(DD + '/crop-years'), getJson(DD + '/packing-types'),
            getJson(API + '/lookups/history-combos')
        ]).then(function (a) {
            var L = S.L;
            L.parties = a[0] || []; L.partyById = {}; L.partyByGl = {};
            L.parties.forEach(function (p) { L.partyById[int(p.Id)] = p; if (int(p.GlAccountId) && !L.partyByGl[int(p.GlAccountId)]) L.partyByGl[int(p.GlAccountId)] = int(p.Id); });
            L.payTerms = a[1] || []; L.delTerms = a[2] || []; L.otherItems = a[3] || [];
            L.packTypes = []; L.ebTerms = []; L.baseDate = []; L.commType = []; L.commUom = [];
            (a[4] || []).forEach(function (r) {
                var o = { id: pick(r, 'Id'), name: pick(r, 'ReferenceName') };
                switch (pick(r, 'Activity')) {
                    case 'AllocatedPackingType': L.packTypes.push(o); break;
                    case 'EmptyBagTypes': L.ebTerms.push(o); break;
                    case 'PaymentBaseDate': L.baseDate.push(o); break;
                    case 'CommissionType': L.commType.push(o); break;
                    case 'CommissionRateUom': L.commUom.push(o); break;
                    default: break;
                }
            });
            L.pmPurchase = a[5] || []; L.pmSale = a[6] || [];
            L.acc = a[7] || L.acc; L.taxTypes = a[8] || [];
            var c = a[9] || {};
            S.cfg = { raw: c, disableFreight: /^(1|true)$/i.test(String(c.DisableFreightFieldsOnBillFromGdn || '')),
                enableSaleAddLess: /^(1|true)$/i.test(String(c.EnableSaleAddLessWeightOnBillFromGdn || '')) };
            S.dec = int(c['Default NoofDecimal Points For Amount']) || 2;
            var dh = int(c.DefaultDaysToLessFromHistoryFromDate);
            if (dh > 0) setVal('histFromDate', ymd(new Date(t.getFullYear(), t.getMonth(), t.getDate() - dh)));
            L.partiesNick = a[12] || [];
            bindParties('P'); bindParties('S');
            /* HistoryComboBind (6245): USP_DropDownFillFromInvCommAgentTradeBill split by Activity */
            var hc = { TradingAccount: [], Supplier: [], Customer: [] };
            (a[16] || []).forEach(function (r) { var k = pick(r, 'Activity'); if (hc[k]) hc[k].push({ id: pick(r, 'Id'), name: pick(r, 'ReferenceName') }); });
            fill('cmbsupplierhistory', hc.Supplier, 'id', 'name'); fill('cmbcusthistory', hc.Customer, 'id', 'name');
            fill('cmbtradAcHistory', hc.TradingAccount, 'id', 'name');
            /* ItemNameBind / CropDtFillFromGlobalAndBind / PackingTypeDtFillFromGlobalAndBind (1341-1385) */
            L.items = a[13] || []; itemBind();
            fill('CmbCropYear', a[14] || [], 'id', 'name'); fill('CmbPackType', a[15] || [], 'id', 'name');
            fill('CmbPaymentTermPurchase', L.payTerms, 'id', 'name'); fill('CmbPaymentTermSale', L.payTerms, 'id', 'name');
            fill('CmbDeliveryTermPurchase', L.delTerms, 'id', 'name'); fill('CmbDeliveryTermSale', L.delTerms, 'id', 'name');
            fill('CmbEmptyBagTerm', L.ebTerms, 'id', 'name', false); fill('CmbEmptyBagTermPurchase', L.ebTerms, 'id', 'name', false);
            var codeAttr = function (d) { return ' data-code="' + esc(d.AccountCode || '') + '"'; };
            fill('CmbTradingAccount', L.acc.trading, 'Id', 'AccountTitle', false, codeAttr);
            fill('CmbTaxAccountPurchase', L.acc.tax, 'Id', 'AccountTitle', false); fill('CmbTaxAccountSale', L.acc.tax, 'Id', 'AccountTitle', false);
            fill('CmbWhtAccountPurchase', L.acc.whtPurchase, 'Id', 'AccountTitle'); fill('CmbWhtAccountSale', L.acc.whtSale, 'Id', 'AccountTitle');
            fill('CmbWhtTaxTypePurchase', L.taxTypes, 'Id', 'TaxName');
            fill('CmbCompanyName', a[10] || [], 'id', 'name', false); fill('CmbBranch', a[11] || [], 'id', 'name', false);
        });
    }

    /* frmPendingGdnLoader_KeyDown (:696) while the loader is open, else CommissionAgentTradeBill_162_KeyDown (:8052) */
    function onShortcut(e) {
        if (!e.ctrlKey) return;
        var k = (e.key || '').toLowerCase();
        if (e.altKey && (k === 'alt' || k === 'control')) { e.preventDefault(); showKeys($id('loaderModal').classList.contains('open') ? LD_KEYS : MAIN_KEYS); return; }
        if ($id('keysModal').classList.contains('open')) return;
        if ($id('loaderModal').classList.contains('open')) {
            var map = { e: function () { $id('loaderModal').classList.remove('open'); }, s: loaderSearch, n: loaderReset, r: loaderRefresh, l: function () { /* btnLoadOnInvoice_Click_1 is empty */ } };
            if (map[k]) { e.preventDefault(); map[k](); }
            else if (k === 'f5' || k === 'arrowup') { e.preventDefault(); $id('ldFromDate').focus(); }
            return;
        }
        var main = $id('paneForm').classList.contains('active');
        var vis = function (id) { var b = $id(id); return b && !b.hidden && !b.disabled; };
        if (k === 't') { e.preventDefault(); if ($id('paneHistory').classList.contains('active')) showTab('mainTabs', 'paneForm'); else { showTab('mainTabs', 'paneHistory'); showHistory(); } return; }
        if (!main) return;
        if (k === 's' && !e.shiftKey && vis('btnSave')) { e.preventDefault(); $id('btnSave').click(); }
        else if (k === 'u' && vis('btnUpdate')) { e.preventDefault(); $id('btnUpdate').click(); }
        else if (k === 'delete' && e.shiftKey && vis('btnDelete')) { e.preventDefault(); $id('btnDelete').click(); }
        else if (k === 'n') { e.preventDefault(); $id('btnNew').click(); }
        else if (k === 'r') { e.preventDefault(); $id('btnRefresh').click(); }
        else if (k === 'f5') { e.preventDefault(); var sp = $id('CmbSupplier'); if (sp) sp.focus(); }
    }

    function wire() {
        document.addEventListener('change', function (e) { if (e.target.closest('table.tb-grid')) onCellChange(e); });
        document.addEventListener('click', function (e) { if (e.target.closest('table.tb-grid button[data-btn]')) onGridButton(e); });
        document.addEventListener('click', function (e) {
            var tr = e.target.closest('#ldMaster tbody tr[data-ld]');
            if (tr && !e.target.closest('button')) { var m = loaderState.master[+tr.getAttribute('data-ld')]; loaderState.selectedId = int(pick(m, 'gdnBuyerDispatchMasterId')); loaderState.dPos = 0; renderLoader(); }
            var dtr = e.target.closest('#ldDetail tbody tr');
            if (dtr) { loaderState.dPos = Array.prototype.indexOf.call(dtr.parentNode.children, dtr); renderLoader(); }
            var nb = e.target.closest('.ld56-nav button[data-nav]');
            if (nb && !nb.disabled) ldNavigate(nb.parentNode.getAttribute('data-grid'), nb.getAttribute('data-nav'));
        });
        ['mainTabs', 'childTabs', 'histTabs'].forEach(function (s) {
            $id(s).addEventListener('click', function (e) { var b = e.target.closest('button[data-pane]'); if (b) showTab(s, b.getAttribute('data-pane')); });
        });
        $id('btnNew').onclick = function () { withButton('btnNew', function () { resetForm(); return formResetDBCallsAndDefault(); }); };
        $id('btnSave').onclick = function () { save(false); };
        $id('btnUpdate').onclick = function () { save(true); };
        $id('btnDelete').onclick = del;
        $id('btnRefresh').onclick = function () { withButton('btnRefresh', function () { return loadLookups().then(renderAll); }); };
        $id('btnHistory').onclick = function () { showTab('mainTabs', 'paneHistory'); showHistory(); };
        $id('btnShowHistory').onclick = showHistory;
        $id('btnLoadGdn').onclick = openLoader;
        $id('btnLoaderClose').onclick = function () { $id('loaderModal').classList.remove('open'); };
        $id('btnLoaderSearch').onclick = loaderSearch;
        $id('btnLoaderRefresh').onclick = loaderRefresh;          /* btnRefresh_Click (:644) = CombosFill(ComboDbCall()) */
        $id('btnLoaderShortcut').onclick = function () { showKeys(LD_KEYS); };
        $id('btnShortcutKeys').onclick = function () { showKeys(MAIN_KEYS); };
        $id('btnKeysClose').onclick = function () { $id('keysModal').classList.remove('open'); };
        $id('btnLdFs').onclick = function () { $id('loaderModal').classList.toggle('fs-detail'); };   /* ctrlGrdBar6 */
        ['ldDocNoFrom', 'ldDocNoTo'].forEach(function (id) { $id(id).addEventListener('input', function () { var v = this.value.replace(/[^0-9]/g, ''); if (v !== this.value) this.value = v; }); });  /* OnlytextNumberFunction */
        /* BtnLoadSelectedRows: the selected (highlighted) GDN, as the row's own Load button does */
        $id('btnLoaderLoad').onclick = function () {
            var i = loaderState.master.findIndex(function (m) { return int(pick(m, 'gdnBuyerDispatchMasterId')) === loaderState.selectedId; });
            if (i < 0) { alert('No valid row selected.'); return; }
            loaderButton('Load', i);
        };
        /* history: row click = SelectionChanged (detail grids), double click = ReadById */
        $id('GrdHistory').addEventListener('click', function (e) {
            if (e.target.closest('button')) return;
            var tr = e.target.closest('tbody tr'); if (!tr || !histRows.length) return;
            historySelect(Array.prototype.indexOf.call(tr.parentNode.children, tr));
        });
        $id('GrdHistory').addEventListener('dblclick', function (e) {
            if (e.target.closest('button')) return;
            var tr = e.target.closest('tbody tr'); if (!tr) return;
            var r = histRows[Array.prototype.indexOf.call(tr.parentNode.children, tr)]; if (r) loadRecord(int(r.Id));
        });
        $id('cmbperemeter').addEventListener('change', dateTypeChanged);
        $id('btnNewHistory').onclick = newHistory;
        $id('btnGrnFormHistory').onclick = function () { window.open('/commission/reports/agent-trade-bill-register', '_blank'); };   /* CommissionAgentTrade_Register */
        ['RadBusinessNamePurchase', 'RadNickNamePurchase', 'radPartyCodePurchase'].forEach(function (id) { $id(id).addEventListener('change', function () { bindParties('P'); }); });
        ['RadBusinessNameSale', 'RadNickNameSale', 'radPartyCodeSale'].forEach(function (id) { $id(id).addEventListener('change', function () { bindParties('S'); }); });
        wireDetailPanel();
        $id('btnLoaderReset').onclick = loaderReset;
        var fs = function (btn, wrap) { $id(btn).onclick = function () { $id(wrap).classList.toggle('tb-fs'); }; };
        fs('btnFsDetail', 'wrapDetail'); fs('btnFsHistory', 'wrapHistory');
        parameterFill();
        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape') {
                document.querySelectorAll('.tb-fs').forEach(function (x) { x.classList.remove('tb-fs'); });
                if ($id('keysModal').classList.contains('open')) { $id('keysModal').classList.remove('open'); return; }
                $id('loaderModal').classList.remove('open'); return;
            }
            onShortcut(e);
        });

        var on = function (id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); };
        on('txtDocDate', 'change', function () { purchaseTaxCalculation(); loadSaleTaxTypes(); dueDaysChanged('P'); dueDaysChanged('S'); });   /* txtDocDate_ValueChanged */
        on('CmbSupplier', 'change', function () { checkSupplierGl(); commissionAccountWarnings(); renderAll(); });                         /* Leave + TextChanged */
        on('CmbCustomer', 'change', function () { checkCustomerGl(); expenseProportionForSale(); commissionAccountWarnings(); loadSaleTaxTypes(); });
        on('CmbTradingAccount', 'change', function () { expenseProportionForSale(); freightProportion(); calculateProfitLoss(); renderAll(); });
        on('CmbDeliveryTermPurchase', 'change', function () { deliveryTermChanged(false); });
        on('CmbDeliveryTermSale', 'change', function () { deliveryTermChanged(true); });
        on('CmbPaymentTermPurchase', 'change', function () { paymentTermChanged('P'); });
        on('CmbPaymentTermSale', 'change', function () { paymentTermChanged('S'); });
        on('txtDueDaysPurchase', 'input', function () { dueDaysChanged('P'); });
        on('txtDueDaysSale', 'input', function () { dueDaysChanged('S'); });
        on('CmbWhtTaxTypePurchase', 'change', purchaseTaxCalculation);
        on('CmbWhtTaxTypeSale', 'change', saleTaxCalculation);
        on('txtFreightAmountPurchase', 'input', function () { calculateTotalFreight('P'); renderAll(); });
        on('txtFreightAddLessPurchase', 'input', function () { calculateTotalFreight('P'); renderAll(); });
        on('txtFreightAmountSale', 'input', function () { calculateTotalFreight('S'); renderAll(); });
        on('txtFreightAddLessSale', 'input', function () { calculateTotalFreight('S'); renderAll(); });
        on('txtTotalFreightPurchase', 'input', function () { totalFreightChanged(); renderAll(); });
        on('txtTotalFreightSale', 'input', function () { totalFreightChanged(); renderAll(); });
        ['txtFreightAmountPurchase', 'txtFreightAddLessPurchase', 'txtTotalFreightPurchase', 'txtFreightAmountSale', 'txtFreightAddLessSale', 'txtTotalFreightSale',
            'txtBuyerFirstWeight', 'txtBuyerSecondWeight', 'txtSupplierFirstWeight', 'txtSupplierSecondWeight', 'txtBillWeightGdn', 'txtBillWeightGrn']
            .forEach(function (id) { on(id, 'blur', function () { setVal(id, f4(val(id))); }); });    /* *_Leave: "#,##0.####" */
        on('txtSupplierFirstWeight', 'input', function () { netFromFirstSecond('P'); });
        on('txtSupplierSecondWeight', 'input', function () { netFromFirstSecond('P'); });
        on('txtBuyerFirstWeight', 'input', function () { netFromFirstSecond('S'); });
        on('txtBuyerSecondWeight', 'input', function () { netFromFirstSecond('S'); });
        ['txtSupplierWeight', 'txtBuyerWeight', 'txtBillWeightGrn', 'txtBillWeightGdn'].forEach(function (id) { on(id, 'input', weights); });
        /* CmbEmptyBagTerm_Leave / CmbEmptyBagTermPurchase_Leave -> AddEmptyBagTermInPurchaseSaleGrid */
        on('CmbEmptyBagTerm', 'change', function () { S.sPm.forEach(function (r) { r.EmptyBagTermId = int(val('CmbEmptyBagTerm')); r.EmptyBagTerm = selText('CmbEmptyBagTerm'); }); renderAll(); });
        on('CmbEmptyBagTermPurchase', 'change', function () { S.pPm.forEach(function (r) { r.EmptyBagTermId = int(val('CmbEmptyBagTermPurchase')); r.EmptyBagTerm = selText('CmbEmptyBagTermPurchase'); }); renderAll(); });
    }

    document.addEventListener('DOMContentLoaded', function () {
        wire();
        loadRights();
        loadLookups().then(function () {
            resetForm();
            return formResetDBCallsAndDefault();
        }).then(function () {
            var m = /[?&]id=(\d+)/.exec(location.search);
            if (m) loadRecord(+m[1]);
        });
    });
})();
