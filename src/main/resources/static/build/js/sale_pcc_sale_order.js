/*
 * Screen 548  SaleOrderConcrete  (Architecture.WinApp.pcc.Sale.SaleOrderConcrete, document type 1852)
 * Page script. Desktop methods are named in the comments (SaleOrderConcrete.cs). Server: /sale/pcc/sale-order/api
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/pcc/sale-order/api', PRINT = '/sale/pcc/print/sale-order-slip';
    var S = {
        id: 0, approved: false, rights: {}, remove: [], updateIdx: -1, files: [], removedAtt: [], existing: [], depth: 0, saveAs: false,
        fmt: { amountRound: 0, amount: 0, rate: 2, fcy: 0, rateRound: 2 }, def: {}, multi: false, subsidiary: false, baseCurrency: 0, baseRate: 0,
        cust: [], items: [], terms: [], refs: [], contractors: [], jobs: [], cities: [], gl: {}, hCust: [], hist: [], search: {}
    };
    var cb = {}, G = {}, tabs;

    function fail(e) { return SE.dbError(e); }
    function msg(t, c) { return SE.alert(t, c); }
    function field(id) { return ($(id).value || '').trim(); }
    function vid(c) { return +c.value() || 0; }
    function num(id) { return SE.toNum($(id).value); }
    function n(v) { return SE.toNum(v); }
    function isShown(id) { var b = $(id); return !!b && b.style.display !== 'none' && !b.hidden; }
    function show(id, on) { var b = $(id); if (!b) return; b.style.display = on ? '' : 'none'; if (b.hasAttribute('hidden')) b.hidden = !on; }
    function enable(c, on) { c.el.disabled = !on; var w = c.el.__dtcombo; if (w && w.syncFromSelect) w.syncFromSelect(); }
    function setEnabled(id, on) { $(id).disabled = !on; }
    /* WinForms TextChanged: fires only when the text really changes (and for programmatic assignments too) */
    function setv(id, t) {
        var e = $(id); t = t == null ? '' : String(t);
        if (e.value !== t) { e.value = t; e.dispatchEvent(new Event('input', { bubbles: true })); }
    }
    function tag() { var a = document.activeElement; return a && a.getAttribute ? (a.getAttribute('data-tag') || '') : ''; }   // base.ActiveControl.Tag
    /* every desktop handler wraps its body in try / catch (MessageBox.Show(ex.Message)); a depth guard stands in for WinForms' re-entrancy */
    function guard(fn) {
        return function () {
            if (S.depth > 30) return;
            S.depth++;
            try { return fn.apply(this, arguments); } catch (x) { msg(x.message); } finally { S.depth--; }
        };
    }

    /* ------------------------------------------------------------------ number formats (clsGlobalVariables) */
    function fmtAmt(v) { var d = S.fmt.amount; return SE.num(v, d, d); }                       // stringFormatsingle
    function fmtRate(v) { var d = S.fmt.rate; return SE.num(v, d, d); }                        // DecimalRateFormate
    function fmtFcy(v) { var d = S.fmt.fcy; return SE.num(v, d, d); }                          // stringFormatsingleForFcy
    function f2(v) { return SE.num(v, 2, 0); }                                                // "#,##0.##"
    function f2z(v) { return v === 0 ? '' : SE.num(v, 2, 0); }                                // "##,#.##" (zero prints nothing)
    function f3(v) { return SE.num(v, 3, 0); }                                                // "#,##0.###"
    function f4(v) { return SE.num(v, 4, 0); }                                                // "#,##0.####"
    function roundAway(v, dec) { var p = Math.pow(10, Math.max(0, dec)); return (v < 0 ? -1 : 1) * Math.round(Math.abs(v) * p + 1e-9) / p; }
    function roundEven(v, dec) {                                                              // decimal Math.Round(v, dec)
        var p = Math.pow(10, Math.max(0, dec)), x = Math.abs(v) * p, fl = Math.floor(x + 1e-9), d = x - fl, r;
        if (Math.abs(d - 0.5) < 1e-9) r = (fl % 2 === 0) ? fl : fl + 1; else r = Math.round(x);
        return (v < 0 ? -1 : 1) * r / p;
    }
    function plain(v) { return String(+Number(v).toFixed(10)); }
    function iso(d) { return d.getFullYear() + '-' + SE.pad2(d.getMonth() + 1) + '-' + SE.pad2(d.getDate()); }
    function parseIso(s) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(s || ''); return m ? new Date(+m[1], +m[2] - 1, +m[3]) : null; }
    function addDays(s, k) { var d = parseIso(s) || new Date(); d.setDate(d.getDate() + k); return iso(d); }

    /* ------------------------------------------------------------------ combos */
    function makeCombos() {
        cb.cust = XCombo('combsuppname', { columns: [{ key: '_t', caption: 'Party Name' }], textKey: '_t', popupWidth: 420 });
        cb.visited = XCombo('CmbVisitedbyName', { columns: [{ key: 'ReferencePartyName', caption: 'Visited By' }], textKey: 'ReferencePartyName' });
        cb.storey = XCombo('Cmbstoreys', { columns: [{ key: 'LookupName', caption: 'Building Storey' }], textKey: 'LookupName', onSelect: termLeave });
        cb.height = XCombo('Cmbheight', { columns: [{ key: 'LookupName', caption: 'Building Height' }], textKey: 'LookupName', onSelect: termLeave });
        cb.term = XCombo('combpttrm', { columns: [{ key: 'TermsDescription', caption: 'Payment Term' }], textKey: 'TermsDescription', onSelect: termLeave });
        cb.dterm = XCombo('combdeliverytrm', { columns: [{ key: 'DeliveryTerm', caption: 'Delivery Term' }], textKey: 'DeliveryTerm' });
        cb.agent = XCombo('combsalesman', { columns: [{ key: 'CompanyName', caption: 'SalesMan Name' }], textKey: 'CompanyName', popupWidth: 380, onSelect: agentLeave });
        cb.refsm = XCombo('CmbRefSalesMan', { columns: [{ key: 'ReferencePartyName', caption: 'RefSalesMan' }], textKey: 'ReferencePartyName' });
        cb.ctype = XCombo('combcommtype', { columns: [{ key: 'CommissionType', caption: 'CommissionType' }], textKey: 'CommissionType' });
        cb.freight = XCombo('CmbFrieghtAccount', { columns: [{ key: 'AccountTitle', caption: 'Account Title' }], textKey: 'AccountTitle', popupWidth: 380, onSelect: accountSelected });
        cb.otherw = XCombo('CmbOtherWagesAccount', { columns: [{ key: 'AccountTitle', caption: 'Account Title' }], textKey: 'AccountTitle', popupWidth: 380, onSelect: accountSelected });
        cb.item = XCombo('combitem', { columns: [{ key: '_t', caption: 'Item Name' }], textKey: '_t', popupWidth: 420, onSelect: guard(itemLeave) });
        cb.varient = XCombo('CmbAttributeVarient', { columns: [{ key: 'ItemAttribute', caption: 'Varient' }], valueKey: 'ItemAttributeVarientId', textKey: 'ItemAttribute', onSelect: guard(varientLeave) });
        cb.disc = XCombo('CmbDiscountType', { columns: [{ key: 'DiscountType', caption: 'DiscountType' }], textKey: 'DiscountType', onSelect: guard(function () { calcDisc(); }) });
        cb.job = XCombo('combjob', { columns: [{ key: 'JobLotDescription', caption: 'Job Lot' }], textKey: 'JobLotDescription', popupWidth: 300 });
        cb.city = XCombo('combcityarea', { columns: [{ key: 'CityName', caption: 'City Name' }], textKey: 'CityName', popupWidth: 300 });
        cb.cur = XCombo('cmbCurrency', { columns: [{ key: 'CurrencyCode', caption: 'Currency' }], textKey: 'CurrencyCode', onSelect: currencyLeave });
        cb.hStatus = XCombo('cmbApproveStatusHistory', { columns: [{ key: 'Status', caption: 'Status' }], textKey: 'Status' });
        cb.hAppr = XCombo('CmbIsApprovedHistory', { columns: [{ key: 'Status', caption: 'Status' }], textKey: 'Status' });
        cb.hCust = XCombo('CmbSupplierHistory', { columns: [{ key: '_t', caption: 'Party Name' }], textKey: '_t', popupWidth: 380 });
        cb.hItem = XCombo('CmbItemHistory', { columns: [{ key: 'ItemName', caption: 'Item Name' }], textKey: 'ItemName', popupWidth: 380 });
    }
    /* desktop fill pattern: rebind, then restore the previous Value if it is still in the list, else blank */
    function keep(c, rows, id) {
        var k = id === undefined ? vid(c) : id;
        c.setData(rows || []);
        if (k > 0 && !c.setValue(k)) c.clear();
    }
    function firstOf(c, rows) { if (rows && rows.length) c.setValue(rows[0].Id); }          // Rows[1].Activate() (Rows[0] is the blank ZeroIndex row)
    function custRows(list, byName) { return (list || []).map(function (r) { return Object.assign({ _t: byName ? r.CompanyName : r.PartyCode }, r); }); }
    function itemRows(list, byName) { return (list || []).map(function (r) { return Object.assign({ _t: byName ? r.ItemName : r.ItemCode }, r); }); }
    function partyByName() { return $('RdPartyByName').checked; }
    function itemByName() { return $('rdbtnItemName').checked; }

    function fillCustomers() { keep(cb.cust, custRows(S.cust, partyByName())); }                       // SupplierNameFill
    function fillItems() { keep(cb.item, itemRows(S.items, itemByName())); }                           // ItemDetailFill
    function fillTerms() {                                                                              // PaymentTerms
        var id = vid(cb.term);
        cb.term.setData(S.terms); firstOf(cb.term, S.terms);
        if (id > 0 && !cb.term.setValue(id)) cb.term.clear();
    }
    function fillStatic() {
        var dt = [{ Id: 1, DeliveryTerm: 'Factory Loading' }, { Id: 2, DeliveryTerm: 'Delivery' }];     // DeliveryTerms
        var id = vid(cb.dterm); cb.dterm.setData(dt); firstOf(cb.dterm, dt); if (id > 0 && !cb.dterm.setValue(id)) cb.dterm.clear();
        var ct = [{ Id: 1, CommissionType: 'Flat' }, { Id: 2, CommissionType: 'Percent' }];            // CommissionTypeFill
        id = vid(cb.ctype); cb.ctype.setData(ct); firstOf(cb.ctype, ct); if (id > 0 && !cb.ctype.setValue(id)) cb.ctype.clear();
        id = vid(cb.disc); cb.disc.setData([{ Id: 1, DiscountType: 'Flat' }, { Id: 2, DiscountType: 'Percent' }]);   // DiscountTypefill: blank until chosen
        if (id > 0 && !cb.disc.setValue(id)) cb.disc.clear();
    }
    /* RefSalesManFill */
    function fillRefSalesMen() {
        var id = vid(cb.refsm), a = vid(cb.agent);
        return SE.api(API + '/ref-salesmen' + SE.q({ agentId: a })).then(function (rows) {
            if (rows.length) keep(cb.refsm, rows, id); else { cb.refsm.clear(); cb.refsm.setData([]); }
        }).catch(fail);
    }
    function applyLists(d) {
        S.refs = d.refParties || S.refs; S.cust = d.customers || S.cust; S.items = d.items || S.items; S.terms = d.terms || S.terms;
        S.contractors = d.contractors || S.contractors; S.jobs = d.jobLots || S.jobs; S.cities = d.cities || S.cities; S.def = d.defaults || S.def;
        SPC.fillDatalist('CmbReferenceParty', S.refs, 'ReferencePartyName');                           // ReferencePartyFill
        keep(cb.storey, d.storeys); keep(cb.height, d.heights); keep(cb.visited, d.visitedBy);
        fillCustomers(); keep(cb.agent, d.agents); fillItems();
        keep(cb.job, S.jobs); keep(cb.city, S.cities);
        keep(cb.freight, d.freightAccounts); keep(cb.otherw, d.otherWagesAccounts);
        keep(cb.cur, d.currencies);
        if (d.terms) fillTerms();
        configDefault();
    }
    /* ConfigurationDefault */
    function configDefault() {
        if (S.def.cityId > 0) cb.city.setValue(S.def.cityId);
        if (S.def.jobLotId > 0) cb.job.setValue(S.def.jobLotId);
        S.baseCurrency = S.def.baseCurrency || 0; S.baseRate = S.def.baseRate || 0;
        if (S.baseCurrency > 0 && !(vid(cb.cur) > 0)) cb.cur.setValue(S.baseCurrency);
        if (S.baseRate > 0 && num('txtExchangeRate') === 0) setv('txtExchangeRate', fmtRate(S.baseRate));
    }
    /* MultiCurrencyFeature */
    function multiCurrency() {
        ['cmbCurrency', 'txtExchangeRate', 'txtFcyAmount', 'label15', 'label16', 'label48'].forEach(function (id) { var e = $(id); if (e) e.style.visibility = S.multi ? '' : 'hidden'; });
        var c = $('cmbCurrency'); if (c && c.__dtcombo && c.__dtcombo.wrap) c.__dtcombo.wrap.style.visibility = S.multi ? '' : 'hidden';
        $('CmbReferenceParty').style.width = (S.multi ? 193 : 360) + 'px';
        $('txtRefPartyAddress').style.width = (S.multi ? 193 : 390) + 'px';
        $('txtRefPartyCellNo').style.width = (S.multi ? 193 : 390) + 'px';
        $('BtnDefineRefParties').style.left = (S.multi ? 789 : 986) + 'px';
        [G.grd, G.hd].forEach(function (g) { var col = g && g.spec.cols.filter(function (x) { return x.k === 'FcyAmount'; })[0]; if (col) col.hide = !S.multi; if (g) g.refresh(); });
    }

    /* ------------------------------------------------------------------ detail entry calculations */
    /* CalculateDetailAmount */
    function calcDetail() {
        var qty = num('txtqty'), rate = num('txtRate'), ve = num('txtVarientEquivalent'), w = num('txtItemWeight');
        setv('txtNetWeight', f2z(ve * qty * w));
        var amount = ve * qty * rate;
        if (amount < 0) { setv('txtamount', ''); throw new Error('Amount can not be Less than zero'); }
        setv('txtamount', fmtAmt(amount));
        setv('txtTotalAmount', fmtAmt(amount));
    }
    /* CalculateDiscountAndTotalAmount (depends on the focused control's Tag) */
    function calcDisc() {
        var pct = num('txtDiscountRate'), amt = num('txtamount'), disc = num('txtDiscountAmount'), type = cb.disc.text(), t = tag();
        if (type === 'Percent') {
            if (t === 'DiscPercent') {
                if (pct > 0) { disc = roundAway(amt * pct / 100, S.fmt.amountRound); setv('txtDiscountAmount', fmtAmt(disc)); }
                else setv('txtDiscountAmount', '0');
            } else if (t === 'DiscAmount') {
                pct = roundAway(disc * 100 / amt, S.fmt.rateRound); if (isNaN(pct)) pct = 0;
                if (pct > 99) {
                    pct = 99; setv('txtDiscountRate', SE.num(pct, 3, 0));
                    disc = roundAway(amt * pct / 100, S.fmt.amountRound); setv('txtDiscountAmount', fmtAmt(disc));
                    return;
                }
                setv('txtDiscountRate', SE.num(pct, 3, 0));
            } else {
                disc = roundAway(amt * pct / 100, S.fmt.amountRound); setv('txtDiscountAmount', fmtAmt(disc));
            }
        } else if (type === 'Flat') {
            if (t === 'DiscPercent') {
                pct = roundAway(num('txtDiscountRate'), S.fmt.amountRound); disc = pct; setv('txtDiscountAmount', fmtAmt(pct));
            } else if (t === 'DiscAmount') {
                setv('txtDiscountAmount', SE.num(roundAway(num('txtDiscountAmount'), S.fmt.amountRound), 3, 0));
                disc = num('txtDiscountAmount'); pct = disc; setv('txtDiscountRate', SE.num(disc, 3, 0));
            } else {
                pct = roundAway(num('txtDiscountRate'), S.fmt.amountRound); disc = pct; setv('txtDiscountAmount', fmtAmt(pct));
            }
        }
        if (type === 'Percent' && num('txtDiscountRate') > 100) {
            msg('Discount Percentage Cannot greater than 100');
            var p = num('txtDiscountRate'), i = p - 100; setv('txtDiscountRate', fmtAmt(p - i));
        }
        if (amt < 0 || disc < 0) { setv('txtamount', ''); setv('txtDiscountAmount', ''); throw new Error('Amounts can not be Less than zero'); }
        setv('txtTotalAmount', plain(amt - disc));
    }
    /* NetRateCalculation */
    function netRate() {
        if (num('txtItemPrice') > 0 || num('txtRate') > 0) {
            var ip = num('txtItemPrice'), al = num('txtAddLessRate'), nr = num('txtRate'), t = tag();
            if (t === 'NetRate') setv('txtAddLessRate', fmtRate(nr - ip));
            else if (t === 'AddLess') setv('txtRate', plain(ip + al));
            else setv('txtRate', f4(ip + al));
        }
    }
    /* combitem_Leave */
    function itemLeave() {
        var id = vid(cb.item), r = cb.item.row();
        if (id > 0 && r) {
            setv('txtItemWeight', r.ItemWeight == null ? '' : String(r.ItemWeight));
            var price = n(r.ItemRate);
            setv('txtItemPrice', f4(price)); setv('txtRate', f4(price));
            bindVarient(id);
        } else { setv('txtItemPrice', '0'); setv('txtItemWeight', '0'); setv('txtRate', '0'); }
    }
    /* bindvarientunit */
    function bindVarient(itemId) {
        var prev = cb.varient.text();
        return SE.api(API + '/varients' + SE.q({ itemId: itemId })).then(function (rows) {
            if (rows.length) {
                cb.varient.setData(rows);
                var hit = prev ? rows.filter(function (x) { return String(x.ItemAttribute) === prev; })[0] : null;
                if (hit) cb.varient.setValue(hit.ItemAttributeVarientId); else cb.varient.clear();
                setv('txtVarientEquivalent', '');
            } else { cb.varient.clear(); cb.varient.setData([]); }
        }).catch(fail);
    }
    /* combitempck_Leave */
    function varientLeave() {
        var r = cb.varient.row(), eq = 0;
        if (vid(cb.varient) > 0 && r) eq = n(r.VarientEquivalent);
        setv('txtVarientEquivalent', plain(eq));
        calcDetail(); calcDisc();
    }
    /* combpttrm_Leave (also wired to Cmbheight / Cmbstoreys / CmbReferenceParty) */
    function termLeave() {
        setEnabled('txtduedays', true);
        if (vid(cb.term) === 1) { setv('txtduedays', ''); setEnabled('txtduedays', false); }
    }
    /* EnableDiableCommissionFields */
    function commFields() {
        var on = vid(cb.agent) > 0;
        enable(cb.ctype, on); setEnabled('txtCommrate', on); setEnabled('txtcommamount', on); setEnabled('txtcommremarks', on);
        if (on) cb.ctype.focus();
    }
    /* TotalCommissionAmount */
    function totalComm() {
        if ($('txtCommrate').value === '') return;
        var t = cb.ctype.text(), rate = num('txtCommrate');
        if (t === 'Flat') setv('txtcommamount', fmtAmt(rate));
        if (t === 'Percent') { var oa = num('txtOrderAmount'); if (oa !== 0) setv('txtcommamount', fmtAmt(oa * rate / 100)); }
    }
    /* combsalesman_Leave */
    function agentLeave() { fillRefSalesMen().then(function () { totalComm(); calcTotals(); commFields(); }); }
    /* DueDateGenerate */
    function dueDate() {
        var d = field('txtduedays');
        $('duedate').value = d !== '' ? addDays($('DocDate').value, SE.toNum(d)) : $('DocDate').value;
    }
    /* cmbCurrency_Leave */
    function currencyLeave() {
        if (vid(cb.cur) === 0 || num('txtExchangeRate') !== 0) return;
        if (vid(cb.cur) !== S.baseCurrency) {
            SE.api(API + '/last-rate' + SE.q({ currencyId: vid(cb.cur) })).then(function (rows) {
                setv('txtExchangeRate', rows.length ? fmtRate(n(rows[0].LastExchRate)) : '0');
            }).catch(fail);
        } else setv('txtExchangeRate', fmtRate(S.baseRate));
    }
    /* txtExchangeRate_TextChanged */
    function exchangeChanged() {
        var ex = num('txtExchangeRate'), rows = G.grd.rows();
        if (rows.length > 0 && ex > 0) rows.forEach(function (r) { r.FcyAmount = roundEven(n(r.Amount) / ex, S.fmt.fcy); });
        else rows.forEach(function (r) { r.FcyAmount = 0; });
        G.grd.refresh();
        calcTotals();
    }

    /* ------------------------------------------------------------------ footer totals */
    function accountSelected() {
        var ids = [vid(cb.freight), vid(cb.otherw)].filter(function (x) { return x > 0 && S.gl[x] === undefined; });
        if (ids.length) fetchGl(ids);
    }
    function fetchGl(ids) {
        return Promise.all(ids.map(function (id) {
            return SE.api(API + '/supplier-gl' + SE.q({ glAccountId: id })).then(function (r) { S.gl[id] = +r.supplierCustomerId || 0; }).catch(function () { S.gl[id] = 0; });
        }));
    }
    function sum(rows, k) { return rows.reduce(function (a, r) { return a + n(r[k]); }, 0); }
    /* CalculateTotalInformation */
    function calcTotals() {
        var fid = vid(cb.freight), oid = vid(cb.otherw);
        var miss = [fid, oid].filter(function (x) { return x > 0 && S.gl[x] === undefined; });
        if (miss.length) { fetchGl(miss).then(function () { calcTotals(); }); return; }
        guard(function () {
            var rows = G.grd.rows(), wr = G.wg.rows();
            var tq = sum(rows, 'QTY'), tw = sum(rows, 'Weight'), ia = sum(rows, 'Amount'), da = sum(rows, 'DiscAmount'), it = sum(rows, 'TotalAmount'), tf = sum(rows, 'FcyAmount');
            var wf = sum(wr, 'Amount'), adj = num('txtAdjustDiscamount');
            setv('txtOrderQty', f2(roundEven(tq, 2)));
            setv('txtTotalWagesFooter', f2(roundEven(wf, 2)));
            setv('txtOrderWeight', f2(roundEven(tw, 2)));
            setv('txtItemAmountFooter', fmtAmt(ia));
            setv('txtDiscountFooter', fmtAmt(da));
            it += adj;
            setv('txtItemNetAmountFooter', fmtAmt(it));
            var fr = num('txtFrieghtFooter'), oa;
            if (fid > 0) {
                var g1 = S.gl[fid];
                oa = g1 > 0 ? (fid !== g1 ? it + fr : it - fr) : it + fr;
            } else oa = it + fr;
            var ow = num('txtOtherWagesFooter');
            if (oid > 0) {
                var g2 = S.gl[oid];
                if (g2 > 0) { if (oid === g2) oa -= ow; else oa += ow; } else oa += ow;
            } else oa += ow;
            oa += wf;
            setv('txtOrderAmount', fmtAmt(oa));
            setv('txtManualTotalOrderAmount', fmtAmt(oa));
            setv('txtFcyAmount', fmtFcy(tf));
        })();
    }
    /* CalculateWithManualOrderAmountTotal */
    function calcManual() {
        var man = num('txtManualTotalOrderAmount'), fr = num('txtFrieghtFooter'), adj = num('txtAdjustDiscamount'), ow = num('txtOtherWagesFooter');
        var rows = G.grd.rows();
        if (rows.length > 0) {
            var wf = sum(G.wg.rows(), 'Amount'), oa = sum(rows, 'Amount') - adj + ow + fr + wf;
            if (man === 0) {
                setv('txtDiscountFooter', fmtAmt(0)); setv('txtOrderAmount', fmtAmt(oa)); setv('txtManualTotalOrderAmount', fmtAmt(oa));
            } else if (man > oa) {
                msg('Manual Order Amount ' + plain(man) + " Can't be Greater Than Expected Order Amount " + plain(oa));
                setv('txtDiscountFooter', fmtAmt(0)); setv('txtOrderAmount', fmtAmt(oa)); setv('txtManualTotalOrderAmount', fmtAmt(oa));
            } else {
                setv('txtDiscountFooter', fmtAmt(oa - man)); setv('txtOrderAmount', fmtAmt(man));
            }
            propManual();
            setv('txtItemNetAmountFooter', fmtAmt(sum(G.grd.rows(), 'TotalAmount')));
        }
    }
    function rowDisc(r, disc, amount, footer, positive) {            // the shared tail of the two loops of PropotionateDiscountByManualOrderAmountInGrid
        var rate = 0;
        if (+r.DiscountId === 1) { rate = disc; r.DiscRate = roundEven(rate, 2); }                // the desktop writes the missing cell "DisRate" here
        else if (+r.DiscountId === 2) {
            if (positive) { rate = amount > 0 ? disc / amount * 100 : 0; r.DiscRate = roundEven(rate, 2); } else { rate = 0; r.DiscRate = 0; }
        } else { rate = 0; r.DiscRate = 0; }
        if (amount > 0 && disc > 0) r.TotalAmount = amount - disc; else if (amount > 0) r.TotalAmount = amount; else r.TotalAmount = 0;
    }
    /* PropotionateDiscountByManualOrderAmountInGrid */
    function propManual() {
        var rows = G.grd.rows();
        if (rows.length <= 0) return;
        var footer = num('txtDiscountFooter'), total = sum(rows, 'Amount'), calc = 0;
        rows.forEach(function (r) {
            var ra = n(r.Amount), rd = (total > 0 ? (ra / total * 100) : 0) * footer / 100;
            r.DiscAmount = roundEven(rd, 2); r.TotalAmount = ra - rd;
            if (+r.DiscountId === 0) { r.DiscountId = 2; r.DiscountType = 'Percent'; }
            rowDisc(r, rd, ra, footer, footer > 0 && rd > 0);
            calc += rd;
        });
        G.grd.refresh();
        if (!(Math.abs(calc - footer) > 0.01)) return;
        var factor = footer / calc;
        rows.forEach(function (r) {
            var ra = n(r.Amount), up = roundEven(n(r.DiscAmount) * factor, 2);
            r.DiscAmount = up; r.TotalAmount = ra - up;
            rowDisc(r, up, ra, footer, ra > 0 && up > 0);
        });
        G.grd.refresh();
        totalComm(); calcTotals();
    }
    /* UpdateDiscountTypeAndRateInGrid (btnUpdateDiscountInGrid) */
    function updateDiscountInGrid() {
        if (vid(cb.disc) === 0) { msg('DiscountType Required'); cb.disc.focus(); }
        else if (num('txtDiscountRate') === 0) { msg('Discount Rate Required'); $('txtDiscountRate').focus(); }
        else if (G.grd.count() > 0) {
            var id = vid(cb.disc), type = cb.disc.text(), rate = num('txtDiscountRate');
            if (id === 2 && rate >= 100) { msg('Discount Rate Cannot be greater than 99 when Discount Type is Percent'); return; }
            G.grd.rows().forEach(function (r) {
                r.DiscountId = id; r.DiscountType = type; r.DiscRate = rate;
                var amt = n(r.Amount), da = 0;
                if (+r.DiscountId !== 0) {
                    if (+r.DiscountId === 1) { da = rate; r.DiscAmount = da; } else if (+r.DiscountId === 2) { da = amt * rate / 100; r.DiscAmount = da; }
                } else { r.DiscRate = 0; r.DiscAmount = 0; }
                if (amt > 0 && da > 0) r.TotalAmount = amt - da; else if (amt > 0) r.TotalAmount = amt; else r.TotalAmount = 0;
            });
            G.grd.refresh();
            totalComm(); calcTotals();
        } else msg('Atlest one Record Required in Grid');
    }

    /* ------------------------------------------------------------------ grids */
    function nameOr(v) { return v == null ? '' : String(v); }
    function rate0(v) { return fmtRate(n(v)); }
    function makeGrids() {
        G.grd = SE.grid('grd', { footer: true, frozen: 2, dec: 0, onDbl: function (r, i) { editRow(i); },
            onBtn: function (k, r, i) { if (k === 'Delete') deleteRow(i); else if (k === 'Edit') editRow(i); },
            cols: [
                { k: 'Delete', t: 'X', w: 50, btn: 'X' }, { k: 'Edit', t: 'Edit', w: 50, btn: 'Edit' },
                { k: 'Id', hide: true }, { k: 'ItemId', hide: true }, { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'Item', t: 'Item', w: 200 },
                { k: 'ScheduleId', hide: true }, { k: 'AttributeVarientId', hide: true }, { k: 'AttributeVarient', t: 'AttributeVarient', w: 100 },
                { k: 'VarientUnit', t: 'VarientUnit', w: 80, render: f2z, sum: true, cls: 'num' }, { k: 'QTY', t: 'QTY', w: 80, render: f2z, sum: true, cls: 'num' },
                { k: 'Weight', t: 'Weight', w: 80, render: f2z, sum: true, cls: 'num' }, { k: 'NetWeight', t: 'NetWeight', w: 90, render: f2z, sum: true, cls: 'num' },
                { k: 'ItemPrice', t: 'ItemPrice', w: 85, render: rate0, cls: 'num' }, { k: 'AddLessRate', t: 'AddLessRate', w: 85, render: rate0, cls: 'num' },
                { k: 'NetRate', t: 'NetRate', w: 85, render: rate0, cls: 'num' },
                { k: 'Amount', t: 'Amount', w: 100, f: 'amt', sum: true, cls: 'num' }, { k: 'DiscountId', hide: true }, { k: 'DiscountType', t: 'DiscountType', w: 90 },
                { k: 'DiscRate', t: 'DiscRate', w: 80, render: f4, cls: 'num' }, { k: 'DiscAmount', t: 'DiscAmount', w: 95, f: 'amt', sum: true, cls: 'num' },
                { k: 'TotalAmount', t: 'TotalAmount', w: 105, f: 'amt', sum: true, cls: 'num' }, { k: 'JobLotId', hide: true }, { k: 'JobLot', t: 'JobLot', w: 110 },
                { k: 'CityID', hide: true }, { k: 'City', t: 'City', w: 110 }, { k: 'Remarks', t: 'Remarks', w: 160 },
                { k: 'FcyAmount', t: 'FcyAmount', w: 100, render: function (v) { return fmtFcy(n(v)); }, sum: true, cls: 'num', hide: true }, { k: 'CommOnSale', hide: true }] });
        $('grd').addEventListener('keydown', function (e) { guard(gridKey)(e); });

        G.wg = SE.grid('grdContractorWages', { footer: true, frozen: 2, dec: 0,
            onBtn: function (k, r, i) { if (k === 'Delete') delWage(i); else if (k === 'Add') addWage(); },
            onEdit: function (r, k, v) { if (k === 'Qty') wageQty(r, v); else r[k] = v; },
            onDbl: function (r, i) { if (G.wg.curKey() && /^(ItemName|ContractorName|ServiceActivity)$/.test(G.wg.curKey())) pickWage(r, G.wg.curKey()); },
            cols: [
                { k: 'Delete', t: 'X', w: 40, btn: 'X' }, { k: 'Add', t: '+', w: 40, btn: '+' },
                { k: 'Id', hide: true }, { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 170 }, { k: 'ContractorId', hide: true },
                { k: 'ContractorName', t: 'ContractorName', w: 170 }, { k: 'ContractorWagesRateScheduleId', hide: true }, { k: 'ServiceActivity', t: 'ServiceActivity', w: 150 },
                { k: 'Qty', t: 'Qty', w: 80, edit: true, render: f2z, sum: true, cls: 'num' }, { k: 'Rate', t: 'Rate', w: 80, render: function (v) { return n(v) === 0 ? '' : SE.num(n(v), 3, 0); }, cls: 'num' },
                { k: 'Amount', t: 'Amount', w: 100, f: 'amt', sum: true, cls: 'num' }, { k: 'Remarks', t: 'Remarks', w: 130, edit: true }] });
        G.wg.setRows([blankWage()]);
        $('grdContractorWages').addEventListener('keydown', function (e) { guard(wageKey)(e); });

        G.hist = SE.grid('grdhistory', { frozen: 3, dec: 0, onDbl: function (r) { histRead(r, false, false); },
            onBtn: function (k, r) { if (k === 'Edit') histRead(r, true, false); else if (k === 'SaveAs') histRead(r, true, true); else if (k === 'Print') print(+r.Id); },
            onLink: function (k, r) { if (k === 'NoOfAttachments') showAttachments(+r.Id); },
            onSel: histSelected,
            cols: [{ k: 'Edit', t: 'Edit', w: 50, btn: 'Edit' }, { k: 'SaveAs', t: 'SaveAs', w: 80, btn: 'SaveAs' }, { k: 'Print', t: 'Print', w: 50, btn: 'Print' },
                { k: 'Id', hide: true }, { k: 'DocumentTypeId', hide: true }, { k: 'DocNo', t: 'DocNo', w: 70 }, { k: 'DocDate', t: 'DocDate', w: 90, f: 'sdate' },
                { k: 'SupplierCustomerId', hide: true }, { k: 'CustomerName', t: 'CustomerName', w: 150 }, { k: 'CustomerCode', t: 'CustomerCode', w: 90 },
                { k: 'RefrenenceNo', t: 'RefrenenceNo', w: 100 }, { k: 'OrderStatus', t: 'OrderStatus', w: 80 }, { k: 'RemarksHeader', t: 'RemarksHeader', w: 160 },
                { k: 'VisitedByName', t: 'VisitedByName', w: 110 }, { k: 'ReferencePartyName', t: 'ReferencePartyName', w: 150 }, { k: 'ReferencePartyCellNo', t: 'ReferencePartyCellNo', w: 110 },
                { k: 'ReferencePartyAddress', t: 'ReferencePartyAddress', w: 200 }, { k: 'CommissionAgent', t: 'CommissionAgent', w: 110 },
                { k: 'CommissionRate', t: 'Comm Rate', w: 80, render: rate0, cls: 'num' },
                { k: 'CommissionAmount', t: 'Comm Amount', w: 95, f: 'amt', cls: 'num' }, { k: 'PaymentTerm', t: 'PaymentTerm', w: 100 }, { k: 'OrderDueDays', t: 'OrderDueDays', w: 80 },
                { k: 'OrderDueDate', t: 'OrderDueDate', w: 90, f: 'sdate' }, { k: 'OrderExpiryDate', t: 'OrderExpiryDate', w: 100, f: 'sdate' }, { k: 'DeliveryTerm', t: 'DeliveryTerm', w: 110 },
                { k: 'DeliveryStartDate', t: 'DeliveryStartDate', w: 100, f: 'sdate' }, { k: 'DeliveryDays', t: 'DeliveryDays', w: 80 }, { k: 'EntryDate', t: 'EntryDate', w: 130, f: 'dt12' },
                { k: 'EntryUserName', t: 'EntryUserName', w: 100 }, { k: 'ModifyDate', t: 'ModifyDate', w: 130, f: 'dt12' }, { k: 'ModifyUserName', t: 'ModifyUserName', w: 100 },
                { k: 'OrderQty', t: 'OrderQty', w: 80, render: f2z, sum: true, cls: 'num' }, { k: 'OrderWeight', t: 'OrderWeight', w: 90, render: f2z, sum: true, cls: 'num' },
                { k: 'OrderItemAmount', hide: true }, { k: 'OrderDiscountAmount', hide: true },
                { k: 'OrderItemNetAmount', t: 'Item Net Amount', w: 110, f: 'amt', sum: true, cls: 'num' }, { k: 'FrieghtAmount', t: 'FrieghtAmount', w: 100, f: 'amt', sum: true, cls: 'num' },
                { k: 'WageNetAmount', t: 'WageNetAmount', w: 100, f: 'amt', sum: true, cls: 'num' }, { k: 'OtherWagesAmount', t: 'OtherWagesAmount', w: 100, f: 'amt', sum: true, cls: 'num' },
                { k: 'OrderAmount', t: 'OrderAmount', w: 110, f: 'amt', sum: true, cls: 'num' }, { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 90, link: true },
                { k: 'Distance', t: 'Distance', w: 70 }] });
        $('grdhistory').addEventListener('keydown', function (e) { guard(histKey)(e); });
        G.hd = SE.grid('DataGridHistoryDetail', { footer: true, dec: 0, cols: [
            { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'Item', t: 'Item', w: 200 }, { k: 'AttributeVarient', t: 'AttributeVarient', w: 100 }, { k: 'VarientUnit', t: 'VarientUnit', w: 80 },
            { k: 'QTY', t: 'QTY', w: 80, render: f3, sum: true, cls: 'num' }, { k: 'Weight', t: 'Weight', w: 80, render: f3, sum: true, cls: 'num' }, { k: 'NetWeight', t: 'NetWeight', w: 90, render: f3, sum: true, cls: 'num' },
            { k: 'ItemPrice', t: 'ItemPrice', w: 85, render: rate0, cls: 'num' }, { k: 'AddLessRate', t: 'AddLessRate', w: 85, render: rate0, cls: 'num' },
            { k: 'NetRate', t: 'NetRate', w: 85, render: rate0, cls: 'num' }, { k: 'Amount', t: 'Amount', w: 100, f: 'amt', sum: true, cls: 'num' },
            { k: 'Discount', t: 'Discount', w: 80 }, { k: 'DiscRate', t: 'DiscRate', w: 80, render: f4, cls: 'num' }, { k: 'DiscAmount', t: 'DiscAmount', w: 95, f: 'amt', cls: 'num' },
            { k: 'TotalAmount', t: 'TotalAmount', w: 105, f: 'amt', sum: true, cls: 'num' }, { k: 'FcyAmount', t: 'FcyAmount', w: 100, render: function (v) { return fmtFcy(n(v)); }, sum: true, cls: 'num', hide: true },
            { k: 'JobLot', t: 'JobLot', w: 110 }, { k: 'City', t: 'City', w: 110 }, { k: 'Remarks', t: 'Remarks', w: 160 }, { k: 'CommOnSale', t: 'CommOnSale', w: 80, f: 'chk' }] });
    }

    /* ------------------------------------------------------------------ detail entry: btnplus / btnUpdateDetail / grd_DoubleClick / delete */
    function detailValid() {
        function stop(m, f) { msg(m); if (f) f(); return false; }
        if (!cb.item.row() || vid(cb.item) === 0) return stop('Item Name Field is Required', function () { cb.item.focus(); });
        if (!cb.varient.row() || vid(cb.varient) === 0) return stop('Attribute Varient Field is Required', function () { cb.varient.focus(); });
        var q = field('txtqty'); if (q === '' || q === '0') return stop('Qty Field is Required', function () { $('txtqty').focus(); });
        var r = field('txtRate'); if (r === '' || r === '0') return stop('Item Rate Field is Required', function () { $('txtRate').focus(); });
        if (vid(cb.disc) !== 0 && (field('txtDiscountRate') === '' || field('txtDiscountRate') === '0')) return stop('Discount Rate Field is Required when Discount Type is Selected', function () { $('txtDiscountRate').focus(); });
        if (vid(cb.disc) !== 0 && (field('txtDiscountAmount') === '' || field('txtDiscountAmount') === '0')) return stop('Discount Amount Field is Required when Discount Type is Selected', function () { $('txtDiscountAmount').focus(); });
        if (!cb.job.row() || vid(cb.job) === 0) return stop('Job Lot Field is Required', function () { cb.job.focus(); });
        if (!cb.city.row() || vid(cb.city) === 0) return stop('City Field is Required', function () { cb.city.focus(); });
        return true;
    }
    function entryRow() {
        var ir = cb.item.row();
        return { ItemId: vid(cb.item), ItemCode: nameOr(ir.ItemCode), Item: nameOr(ir.ItemName), ScheduleId: +ir.ScheduleId || 0, AttributeVarientId: vid(cb.varient),
            AttributeVarient: cb.varient.text(), VarientUnit: num('txtVarientEquivalent'), QTY: num('txtqty'), Weight: num('txtItemWeight'), NetWeight: num('txtNetWeight'),
            ItemPrice: num('txtItemPrice'), AddLessRate: num('txtAddLessRate'), NetRate: num('txtRate'), Amount: num('txtamount'), DiscountId: vid(cb.disc), DiscountType: cb.disc.text(),
            DiscRate: num('txtDiscountRate'), DiscAmount: num('txtDiscountAmount'), TotalAmount: num('txtTotalAmount'), JobLotId: vid(cb.job), JobLot: cb.job.text().trim(),
            CityID: vid(cb.city), City: cb.city.text().trim(), Remarks: field('txtremarksdetail'), CommOnSale: $('chkCommissionOnSale').checked };
    }
    /* ResetDetail */
    function resetDetail() {
        setv('txtDiscountAmount', ''); setv('txtDiscountRate', '');
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        cb.item.focus();
    }
    var plus = guard(function () {
        if (!detailValid()) return;
        G.grd.addRow(Object.assign({ Id: 0, FcyAmount: 0 }, entryRow()));          // the desktop shifts the CommOnSale flag into FcyAmount here (see report)
        exchangeChanged();
        totalComm(); calcTotals();
        resetDetail();
    });
    var updateDetail = guard(function () {
        if (!detailValid()) return;
        var r = G.grd.rows()[S.updateIdx]; if (!r) return;
        var e = entryRow();
        Object.keys(e).forEach(function (k) { r[k] = e[k]; });
        $('chkCommissionOnSale').checked = false;
        exchangeChanged();
        totalComm(); calcTotals();
        resetDetail();
    });
    function cancelUpdateDetail() { show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false); }
    /* grd_DoubleClick */
    var editRow = guard(function (i) {
        var r = G.grd.rows()[i]; if (!r) return;
        S.updateIdx = i;
        cb.item.setValue(+r.ItemId);
        bindVarient(+r.ItemId).then(guard(function () {
            cb.varient.setValue(+r.AttributeVarientId || 0);
            setv('txtVarientEquivalent', String(r.VarientUnit)); setv('txtItemWeight', String(r.Weight)); setv('txtNetWeight', String(r.NetWeight)); setv('txtqty', String(r.QTY));
            setv('txtItemPrice', String(r.ItemPrice)); setv('txtAddLessRate', String(r.AddLessRate)); setv('txtRate', String(r.NetRate)); setv('txtamount', String(r.Amount));
            if (+r.DiscountId > 0) cb.disc.setValue(+r.DiscountId); else cb.disc.clear();
            setv('txtDiscountRate', String(r.DiscRate)); setv('txtDiscountAmount', String(r.DiscAmount)); setv('txtTotalAmount', String(r.TotalAmount));
            cb.job.setValue(+r.JobLotId || 0); cb.city.setValue(+r.CityID || 0); setv('txtremarksdetail', nameOr(r.Remarks)); $('chkCommissionOnSale').checked = !!r.CommOnSale;
            show('btnplus', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
            calcDetail(); calcDisc();
            cb.item.focus();
        }));
    });
    /* grd_ColumnButtonClick "Delete" / grd_KeyDown Ctrl+Delete */
    function deleteRow(i) {
        var r = G.grd.rows()[i]; if (!r) return;
        function finish() { G.grd.removeAt(i); totalComm(); exchangeChanged(); }
        if ((+r.Id || 0) > 0) {
            return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
                if (!yes) return;
                S.remove.push(lineOf(r));
                finish();
            });
        }
        finish();
    }
    function lineOf(r) {
        return { id: +r.Id || 0, itemId: +r.ItemId || 0, scheduleId: +r.ScheduleId || 0, attributeVarientId: +r.AttributeVarientId || 0, varientUnit: n(r.VarientUnit), qty: n(r.QTY),
            weight: n(r.Weight), netWeight: n(r.NetWeight), itemPrice: n(r.ItemPrice), addLessRate: n(r.AddLessRate), netRate: n(r.NetRate), amount: n(r.Amount), discountId: +r.DiscountId || 0,
            discRate: n(r.DiscRate), discAmount: n(r.DiscAmount), totalAmount: n(r.TotalAmount), jobLotId: +r.JobLotId || 0, cityId: +r.CityID || 0, remarks: nameOr(r.Remarks),
            fcyAmount: n(r.FcyAmount), commOnSale: !!r.CommOnSale };
    }
    function gridKey(e) {
        var i = G.grd.curIndex();
        if (e.ctrlKey && e.key === 'Delete' && i >= 0) { e.preventDefault(); deleteRow(i); }
        else if (e.ctrlKey && e.key === 'Enter' && i >= 0) { e.preventDefault(); editRow(i); }
    }

    /* ------------------------------------------------------------------ contractor wages grid */
    function blankWage() { return { Id: 0, ItemId: 0, ItemName: '', ContractorId: 0, ContractorName: '', ContractorWagesRateScheduleId: 0, ServiceActivity: '', Qty: 0, Rate: 0, Amount: 0, Remarks: '' }; }
    function addWage() { G.wg.addRow(blankWage()); }                                    // AddRowInContractorWagesGrid
    function delWage(i) { G.wg.removeAt(i); if (G.wg.count() === 0) addWage(); }
    /* grdContractorWages_CellUpdated (Qty) */
    function wageQty(r, v) {
        r.Qty = n(v);
        if (n(r.Rate) === 0) { msg('Qty And Rate Should be greater than zero...'); return; }
        r.Amount = n(r.Qty) * n(r.Rate);
        G.wg.refresh();
        calcTotals();
    }
    /* GrdPopUp(dt, idKey, nameKey): a modal list with a search box; closing without a choice returns 0 / "" */
    function pick(title, rows, idKey, nameKey) {
        return new Promise(function (resolve) {
            var done = false;
            function finish(v) { if (done) return; done = true; resolve(v || { id: 0, name: '' }); }
            var pop = SE.pop(title, '<input class="f" id="wpq" placeholder="Search" style="position:static;width:100%;margin-bottom:4px"><div class="dgrid" style="max-height:50vh"><table class="jg"><thead><tr><th>' +
                SE.esc(nameKey) + '</th></tr></thead><tbody id="wpb"></tbody></table></div>', [{ t: 'Close' }]);
            var tb = pop.body.querySelector('#wpb'), q = pop.body.querySelector('#wpq'), sel = 0, shown = [];
            function paint() {
                var t = q.value.toLowerCase();
                shown = rows.filter(function (r) { return String(r[nameKey]).toLowerCase().indexOf(t) >= 0; });
                if (sel >= shown.length) sel = 0;
                tb.innerHTML = shown.map(function (r, i) { return '<tr data-i="' + i + '"' + (i === sel ? ' class="sel"' : '') + '><td>' + SE.esc(r[nameKey]) + '</td></tr>'; }).join('');
            }
            function choose(i) { var r = shown[i]; if (!r) return; finish({ id: +r[idKey] || 0, name: String(r[nameKey]), row: r }); pop.close(); }
            q.addEventListener('input', paint);
            q.addEventListener('keydown', function (e) {
                if (e.key === 'ArrowDown' && sel < shown.length - 1) { sel++; paint(); } else if (e.key === 'ArrowUp' && sel > 0) { sel--; paint(); }
                else if (e.key === 'Enter') { e.preventDefault(); e.stopPropagation(); choose(sel); } else if (e.key === 'Escape') { e.stopPropagation(); pop.close(); }
            });
            tb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) choose(+tr.getAttribute('data-i')); });
            tb.addEventListener('click', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) { sel = +tr.getAttribute('data-i'); paint(); } });
            pop.open(); paint(); q.focus();
            var mo = new MutationObserver(function () { if (!document.body.contains(pop.el)) { mo.disconnect(); finish(null); } });
            mo.observe(document.body, { childList: true });
        });
    }
    /* grdContractorWages_KeyDown F1 (and double-click) */
    function pickWage(r, key) {
        if (key === 'ItemName' || key === 'ItemId') {
            var prev = +r.ItemId || 0;
            var items = G.grd.rows().map(function (x) { return { Id: x.ItemId, ItemName: x.Item, ItemCode: x.ItemCode }; });       // WagesItemFillFromDetail
            return pick('Item', items, 'Id', 'ItemName').then(function (p) {
                r.ItemId = p.id; r.ItemName = p.name;
                if (p.id !== prev) { r.ContractorWagesRateScheduleId = 0; r.ServiceActivity = 0; r.Rate = 0; r.Amount = 0; }
                G.wg.refresh(); calcTotals();
            });
        }
        if (key === 'ContractorName' || key === 'ContractorId') {
            return pick('Contractor', S.contractors, 'Id', 'CompanyName').then(function (p) { r.ContractorId = p.id; r.ContractorName = p.name; G.wg.refresh(); calcTotals(); });
        }
        if (key === 'ServiceActivity') {
            if ((+r.ItemId || 0) <= 0) { msg('Please Select an Item First'); return; }
            return SE.api(API + '/service-activities' + SE.q({ itemId: +r.ItemId, date: $('DocDate').value })).then(function (rows) {
                return pick('Service Activity', rows, 'ContractorWagesRateScheduleId', 'WagesActivity').then(function (p) {
                    r.ContractorWagesRateScheduleId = p.id; r.ServiceActivity = p.name;
                    if (p.id > 0) {
                        var f = rows.filter(function (x) { return +x.ContractorWagesRateScheduleId === p.id; })[0];
                        var qty = f ? n(f.Qty) : 0;
                        if (qty === 0) throw new Error('Attempted to divide by zero.');
                        var per = (f ? n(f.Rate) : 0) / qty;
                        r.Rate = per; r.Amount = per * Math.trunc(n(r.Qty));
                    } else { r.Rate = 0; r.Amount = 0; }
                    G.wg.refresh(); calcTotals();
                });
            }).catch(function (x) { return msg(x.message); });
        }
    }
    function wageKey(e) {
        var i = G.wg.curIndex(), r = G.wg.cur(), k = G.wg.curKey();
        if (!r || !k) return;
        if (e.ctrlKey && e.key === 'd') { e.preventDefault(); addWage(); }
        else if (e.ctrlKey && e.key === 'Delete') { e.preventDefault(); delWage(i); calcTotals(); }
        else if (e.key === 'F1') { e.preventDefault(); pickWage(r, k); }
    }

    /* ------------------------------------------------------------------ Reset / Refresh / Load */
    /* DocumentNoFill */
    function docNoFill() { return SE.api(API + '/next-no').then(function (r) { if (r.nextNo > 0) $('txtdocno').value = String(r.nextNo); }).catch(fail); }
    /* Reset() - clears only the controls the desktop clears */
    function reset() {
        guard(function () {
            S.files = []; S.removedAtt = []; S.existing = []; S.remove = []; S.approved = false; S.id = 0; S.saveAs = false; S.updateIdx = -1;
            G.grd.setRows([]);
            $('txtArea').value = ''; cb.item.clear(); cb.freight.clear(); cb.varient.clear();
            ['txtVarientEquivalent', 'txtItemWeight', 'txtqty', 'txtItemPrice', 'txtAddLessRate', 'txtRate', 'txtamount'].forEach(function (id) { setv(id, ''); });
            cb.disc.clear();
            ['txtDiscountAmount', 'txtDiscountRate', 'txtTotalAmount'].forEach(function (id) { setv(id, ''); });
            cb.job.clear(); cb.city.clear(); setv('txtremarks', '');
            show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
            G.wg.setRows([blankWage()]);
            cb.cust.clear(); cb.agent.clear();
            ['txtsupprefno', 'txtduedays', 'txtdeliverydays'].forEach(function (id) { setv(id, ''); });
            firstOf(cb.ctype, cb.ctype.rows());
            ['txtCommrate', 'txtcommamount', 'txtcommremarks'].forEach(function (id) { setv(id, ''); });
            setv('CmbReferenceParty', ''); setv('txtRefPartyAddress', ''); setv('txtRefPartyCellNo', '');
            ['txtOrderQty', 'txtOrderWeight', 'txtDiscountFooter', 'txtItemAmountFooter', 'txtItemNetAmountFooter', 'txtOrderAmount', 'txtFrieghtFooter', 'txtManualTotalOrderAmount'].forEach(function (id) { setv(id, ''); });
            show('btnsave', true); show('btnupdate', false); show('btnSaveAs', false);
            termLeave();
            commFields();
            $('DocDate').focus();
        })();
        return docNoFill();                                                          // DocumentNoFill
    }
    function resetAll() { return reset(); }
    /* btnRefresh_Click */
    function refresh() {
        return SE.api(API + '/lists' + SE.q({ docDate: $('DocDate').value })).then(function (d) {
            applyLists(d);
            keep(cb.refsm, cb.refsm.rows());
            return fillRefSalesMen();
        }).catch(fail);
    }
    /* GetDataFromSaleOrdersForComboBind + StatusFill + ApproveStatusFill */
    function histCombos(h) {
        S.hCust = (h && h.customers) || S.hCust;
        var byName = $('RdCustomerHistorySearchByName').checked;
        keep(cb.hCust, S.hCust.map(function (r) { return Object.assign({ _t: byName ? r.PartyName : r.PartyCode }, r); }));
        keep(cb.hItem, (h && h.items) || cb.hItem.rows());
        var st = [{ Id: 1, Status: 'Open' }, { Id: 2, Status: 'Cancel' }, { Id: 3, Status: 'Complete' }];
        cb.hStatus.setData(st); cb.hStatus.setValue(1);
        var ap = [{ Id: 1, Status: 'UnApproved' }, { Id: 2, Status: 'Approve' }, { Id: 3, Status: 'All' }];
        cb.hAppr.setData(ap); cb.hAppr.setValue(1);
    }

    /* ------------------------------------------------------------------ ReadById */
    function dateOf(v) { return SE.dateInput(v); }
    function readById(id, saveAs) {
        var rp = reset();
        return Promise.all([SE.api(API + '/' + id), rp]).then(function (a) {
            var d = a[0], h = d.head;
            return fetchGl([+h.TransporterId, +h.OtherWagesAccountId].filter(function (x) { return x > 0 && S.gl[x] === undefined; })).then(function () { return d; });
        }).then(guard(function (d) {
            var h = d.head;
            S.id = id;
            tabs.select('tabPage1');
            $('DocDate').value = dateOf(h.DocDate); dueDate();
            setv('txtdocno', nameOr(h.DocNo));
            cb.cust.setValue(+h.SupplierCustomerId || 0);
            setv('txtsupprefno', nameOr(h.RefrenenceNo));
            cb.visited.setValue(+h.VisitedById || 0);
            setv('txtremarks', nameOr(h.RemarksHeader)); setv('txtDistance', nameOr(h.Distance));
            cb.term.setValue(+h.PaymentTermsId || 0);
            cb.freight.setValue(+h.TransporterId || 0);
            setv('txtduedays', nameOr(h.OrderDueDays));
            $('duedate').value = dateOf(h.OrderDueDate);
            var dt = cb.dterm.rows().filter(function (r) { return r.DeliveryTerm === h.DeliveryTerm; })[0];
            if (dt) cb.dterm.setValue(dt.Id); else cb.dterm.clear();
            $('deliverystartdate').value = dateOf(h.DeliveryStartDate); setv('txtdeliverydays', nameOr(h.DeliveryDays));
            cb.height.setValue(+h.BuildingHeightId || 0); cb.storey.setValue(+h.BuildingStoreyId || 0); setv('txtArea', nameOr(h.BuildingArea));
            setv('CmbReferenceParty', nameOr(h.ReferencPartyName)); setv('txtRefPartyAddress', nameOr(h.ReferencPartyAddress)); setv('txtRefPartyCellNo', nameOr(h.ReferencPartyCellNo));
            cb.otherw.setValue(+h.OtherWagesAccountId || 0);
            cb.agent.setValue(+h.CommissionAgentId || 0);
            cb.refsm.setValue(+h.RefSalesManId || 0);
            var ct = cb.ctype.rows().filter(function (r) { return r.CommissionType === h.CommissionType; })[0];
            if (ct) cb.ctype.setValue(ct.Id); else cb.ctype.clear();
            setv('txtCommrate', plain(n(h.CommissionRate))); setv('txtcommamount', fmtAmt(n(h.CommissionAmount))); setv('txtcommremarks', nameOr(h.CommissionRemarks));
            setv('txtOrderQty', f2(n(h.OrderQty))); setv('txtOrderWeight', f2(n(h.OrderWeight)));
            setv('txtItemAmountFooter', fmtAmt(n(h.OrderItemAmount))); setv('txtDiscountFooter', fmtAmt(n(h.OrderDiscountAmount))); setv('txtItemNetAmountFooter', fmtAmt(n(h.OrderItemNetAmount)));
            setv('txtTotalWagesFooter', fmtAmt(n(h.WageNetAmount))); setv('txtOtherWagesFooter', fmtAmt(n(h.OtherWages))); setv('txtFrieghtFooter', fmtAmt(n(h.FrieghtAmount)));
            setv('txtOrderAmount', fmtAmt(n(h.OrderAmount))); setv('txtManualTotalOrderAmount', fmtAmt(n(h.OrderAmount))); setv('txtAdjustDiscamount', fmtAmt(n(h.AdjustDiscountAmount)));
            S.approved = !!h.IsApproved;
            if (+h.CurrencyId > 0) { cb.cur.setValue(+h.CurrencyId); setv('txtExchangeRate', nameOr(h.ExchangeRate)); } else { cb.cur.clear(); configDefault(); }
            setv('txtFcyAmount', fmtFcy(n(h.FcyAmount)));
            G.grd.setRows((d.lines || []).map(function (l) {
                return { Id: l.Id, ItemId: l.OrderItemId, ItemCode: l.ItemCode, Item: l.ItemName, ScheduleId: l.ScheduleId, AttributeVarientId: l.ItemAttributeVarientId, AttributeVarient: l.ItemAttributeVarient,
                    VarientUnit: l.VarientEquivalent, QTY: l.OrderItemQty, Weight: l.ItemWeight, NetWeight: l.ItemNetWeight, ItemPrice: l.ItemRateWithOutAddLess, AddLessRate: l.RateAddLess,
                    NetRate: l.OrderItemRate, Amount: l.OrderItemAmount, DiscountId: l.ItemDiscountTypeId, DiscountType: l.ItemDiscountType, DiscRate: l.ItemDiscountRate, DiscAmount: l.ItemDiscountAmount,
                    TotalAmount: l.ItemNetAmount, JobLotId: l.JobLotId, JobLot: l.JobLotDescription, CityID: l.CityId, City: l.CityName, Remarks: l.RemarksDetail, FcyAmount: l.FcyAmount, CommOnSale: !!l.CommOnSale };
            }));
            if ((d.wages || []).length) G.wg.setRows(d.wages.map(function (w) {
                return { Id: w.Id, ItemId: w.ItemId, ItemName: w.ItemName, ContractorId: w.ContractorId, ContractorName: w.ContractorName, ContractorWagesRateScheduleId: w.ContractorWagesRateScheduleId,
                    ServiceActivity: w.ServiceActivity, Qty: w.Qty, Rate: w.Rate, Amount: w.Amount, Remarks: nameOr(w.Remarks) };
            })); else G.wg.setRows([blankWage()]);
            S.existing = d.attachments || [];
            show('btnsave', false); show('btnupdate', true); setEnabled('btnupdate', !!S.rights.update);
            if (saveAs) { show('btnsave', false); show('btnupdate', false); show('btnSaveAs', true); S.saveAs = true; }
            exchangeChanged();
        })).catch(fail);
    }

    /* ------------------------------------------------------------------ Insert (btnsave / btnupdate / btnSaveAs) */
    function formValid() {
        function stop(m, f) { return SE.alert(m).then(function () { if (f) f(); return false; }); }
        var no = field('txtdocno');
        if (no === '' || no === '0') return stop('Doc No Field is Required', function () { $('txtdocno').focus(); });
        if (vid(cb.cust) === 0) return stop('Customer Name Field is Required', function () { cb.cust.focus(); });
        if (vid(cb.visited) === 0) return stop('Visitedby Name Field is Required', function () { cb.visited.focus(); });
        if (vid(cb.term) === 0) return stop('Payment Term Field is Required', function () { cb.term.focus(); });
        if (cb.term.text() === 'Credit' && field('txtduedays') === '') return stop('Due Days Field is Required', function () { $('txtduedays').focus(); });
        if (vid(cb.dterm) === 0) return stop('Delivery Term Field is Required', function () { cb.dterm.focus(); });
        if (field('CmbReferenceParty') === '') return stop('Ref Party Name Field is Required', function () { $('CmbReferenceParty').focus(); });
        if (field('txtRefPartyAddress') === '') return stop('Ref Party Address Field is Required', function () { $('txtRefPartyAddress').focus(); });
        if (field('txtRefPartyCellNo') === '') return stop('Ref Party CellNo Field is Required', function () { $('txtRefPartyCellNo').focus(); });
        if (S.multi) {
            if (vid(cb.cur) === 0) return stop('Fcy Code Field is Required', function () { cb.cur.focus(); });
            if (num('txtExchangeRate') === 0) return stop('Exchange Rate Field is Required', function () { $('txtExchangeRate').focus(); });
            if (num('txtFcyAmount') === 0) return stop('Fcy Amount Rate Field is Required', function () { $('txtFcyAmount').focus(); });
        } else {
            if (vid(cb.cur) === 0) return stop('Please Configure Your Base Currency In configurations');
            if (num('txtExchangeRate') === 0) return stop('Please Configure Your Base Currency Rate In configurations');
        }
        return Promise.resolve(true);
    }
    function refPartyId() { var t = field('CmbReferenceParty'); for (var i = 0; i < S.refs.length; i++) if (String(S.refs[i].ReferencePartyName) === t) return +S.refs[i].Id || 0; return 0; }
    function insert(ignore) {
        if (G.grd.count() === 0) return SE.alert('Grid Record not found', 'Database Error');
        return formValid().then(function (ok) {
            if (!ok) return;
            var go = ignore ? Promise.resolve(true) : SE.ask(S.id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm');
            return go.then(function (yes) {
                if (!yes) return;
                if (num('txtFrieghtFooter') > 0 && !(vid(cb.freight) > 0)) { cb.freight.focus(); return SE.alert('Frieght Account field Required', 'Database Error'); }
                var body = { id: S.id, docDate: $('DocDate').value, docNo: field('txtdocno'), refNo: $('txtsupprefno').value, remarks: $('txtremarks').value, distance: $('txtDistance').value,
                    buildingArea: $('txtArea').value, referencePartyName: $('CmbReferenceParty').value, referencePartyAddress: $('txtRefPartyAddress').value, referencePartyCellNo: $('txtRefPartyCellNo').value,
                    dueDays: $('txtduedays').value, dueDate: $('duedate').value, deliveryDays: $('txtdeliverydays').value, deliveryStartDate: $('deliverystartdate').value,
                    commissionType: cb.ctype.text(), commissionRemarks: $('txtcommremarks').value, supplierCustomerId: vid(cb.cust), visitedById: vid(cb.visited), buildingHeightId: vid(cb.height),
                    buildingStoreyId: vid(cb.storey), referencePartyId: refPartyId(), paymentTermId: vid(cb.term), deliveryTermId: vid(cb.dterm), refSalesManId: vid(cb.refsm),
                    otherWagesAccountId: vid(cb.otherw), freightAccountId: vid(cb.freight), currencyId: vid(cb.cur), commissionAgentId: vid(cb.agent), exchangeRate: num('txtExchangeRate'),
                    fcyAmount: num('txtFcyAmount'), orderQty: num('txtOrderQty'), orderWeight: num('txtOrderWeight'), itemAmount: num('txtItemAmountFooter'), adjustDisc: num('txtAdjustDiscamount'),
                    discount: num('txtDiscountFooter'), itemNet: num('txtItemNetAmountFooter'), wageNet: num('txtTotalWagesFooter'), otherWages: num('txtOtherWagesFooter'), freight: num('txtFrieghtFooter'),
                    orderAmount: num('txtOrderAmount'), commissionRate: num('txtCommrate'), commissionAmount: num('txtcommamount'), ignoreLimit: !!ignore,
                    lines: G.grd.rows().map(lineOf), removed: S.remove,
                    wages: G.wg.rows().map(function (w) { return { id: +w.Id || 0, itemId: +w.ItemId || 0, contractorId: +w.ContractorId || 0, contractorWagesRateScheduleId: +w.ContractorWagesRateScheduleId || 0,
                        qty: n(w.Qty), rate: n(w.Rate), amount: n(w.Amount), remarks: nameOr(w.Remarks) }; }),
                    attachments: { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removedAtt } };
                return SE.api(API, { method: 'POST', body: body }).then(function (r) {
                    if (r.limitWarning) return SE.ask(r.limitWarning, 'Confirm').then(function (y) { if (y) return insert(true); });
                    return SE.alert(r.message).then(function () {
                        var p = $('ChkBox').checked;
                        return resetAll().then(function () { if (p) print(r.id); });
                    });
                }).catch(function (e) { return SE.alert(e.message, 'Database Error'); });
            });
        });
    }
    function btnSave() { S.approved = false; S.id = 0; return insert(false); }
    function btnUpdate() {
        if (S.id === 0) return SE.alert('Record Not Update because RecId Not Found', 'Database Error');
        return insert(false);
    }
    function btnSaveAs() { S.approved = false; S.id = 0; return insert(false); }
    /* btnDelete_Click */
    function del() {
        return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
            if (!yes) return;
            return SE.api(API + '/' + S.id, { method: 'DELETE' }).then(function (r) { return SE.alert(r.message || 'Delete Record Seccessfully').then(resetAll); })
                .catch(function (e) { return SE.alert(e.message, 'Message'); });
        });
    }

    /* ------------------------------------------------------------------ history (tabPage2) */
    function showHistory() {                                                              // gridhistoryfill
        var q = { fromDate: $('fromdate').value, toDate: $('ToDate').value, customerId: vid(cb.hCust), status: cb.hStatus.text(), approved: cb.hAppr.text() };
        return SE.api(API + '/history' + SE.q(q)).then(function (rows) {
            S.hist = rows; G.hist.setRows(rows);
            if (!rows.length) G.hd.setRows([]);
        }).catch(fail);
    }
    function histRead(r, resetFirst, saveAs) { return readById(+r.Id, !!saveAs); }
    /* grdhistory_SelectionChanged -> GridDetailBind (SaleOrder.GetByID). The desktop also overwrites the form's Id here; the web does not (see report). */
    function histSelected(item) {
        if (!item) { G.hd.setRows([]); return; }
        SE.api(API + '/' + item.Id).then(function (d) {
            G.hd.setRows((d.lines || []).map(function (l) {
                return { ItemCode: l.ItemCode, Item: l.ItemName, AttributeVarient: l.ItemAttributeVarient, VarientUnit: l.VarientEquivalent, QTY: l.OrderItemQty, Weight: l.ItemWeight, NetWeight: l.ItemNetWeight,
                    ItemPrice: l.ItemRateWithOutAddLess, AddLessRate: l.RateAddLess, NetRate: l.OrderItemRate, Amount: l.OrderItemAmount, Discount: l.ItemDiscountType, DiscRate: l.ItemDiscountRate,
                    DiscAmount: l.ItemDiscountAmount, TotalAmount: l.ItemNetAmount, FcyAmount: l.FcyAmount, JobLot: l.JobLotDescription, City: l.CityName, Remarks: l.RemarksDetail, CommOnSale: !!l.CommOnSale };
            }));
        }).catch(fail);
    }
    function histKey(e) {
        var r = G.hist.cur(); if (!r) return;
        if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); if (!S.rights.update) { msg("you don't have update rights...", 'Message'); return; } histRead(r, true, false); }
        else if (e.ctrlKey && e.key.toLowerCase() === 'p') { e.preventDefault(); print(+r.Id); }
    }

    /* ------------------------------------------------------------------ print / attachments */
    function print(id) {                                                                  // CommonServices.SaleOrderSlipConcrete
        if (!(id > 0)) return SE.alert('No Record Found For Display', 'Message');
        return SPC.openPdf(PRINT + SE.q({ id: id }));
    }
    function showAttachments(id) {                                                        // CommonServices.GetNoofAttachmentsByRefDocumentTypeID(id, 1852)
        return SE.api(API + '/' + id + '/attachments').then(function (rows) { SPC.attachments(S, API, id, true, rows); }).catch(fail);
    }
    function openAttachments() { SPC.attachments(S, API, S.id, false); }

    /* ------------------------------------------------------------------ keys */
    var SHORT = [['Ctrl+S', 'For Save When on Entry form and For Show Data when on History Form'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+E', 'For Close'],
        ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+F12', 'For Save As'],
        ['F1', 'For Combo Lookup on Current Column of any Focused Grid'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+D', 'For Adding an row in Focused Grid'],
        ['Ctrl+Delete', 'For Deleting an row of Focused Grid'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On on Item Combo'],
        ['Ctrl+ArrowRight', 'to change focus from one grid to another'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
        ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function onKey(e) {
        var t = e.target, k = (e.key || '').toLowerCase();
        if (document.querySelector('.se-pop.on') && e.key !== 'Escape') return;                // a pop-up (lookup / attachments / shortcut keys) owns the keyboard
        if (e.key === 'Enter' && t && (t.tagName === 'INPUT' || t.tagName === 'SELECT') && t.type !== 'checkbox' && t.type !== 'radio' && t.type !== 'button') {
            var f = Array.prototype.filter.call(document.querySelectorAll('.dform input:not([type=hidden]):not([disabled]), .dform select:not([disabled]), .dform .dtcombo-input'), function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(t); if (i >= 0 && f[i + 1]) { e.preventDefault(); f[i + 1].focus(); }
        }
        if (e.ctrlKey && k === 't') { e.preventDefault(); if (tabs.index() === 1) { tabs.select('tabPage1'); $('DocDate').focus(); } else { tabs.select('tabPage2'); $('fromdate').focus(); } return; }
        if (e.ctrlKey && e.altKey) { SE.shortcuts(SHORT); return; }
        if (tabs.index() === 0) {
            if (e.ctrlKey && k === 's') { e.preventDefault(); if (isShown('btnsave') && !$('btnsave').disabled) btnSave(); }
            else if (e.ctrlKey && e.key === 'F12') { e.preventDefault(); if (isShown('btnSaveAs') && !$('btnSaveAs').disabled) btnSaveAs(); }
            else if (e.ctrlKey && k === 'u') { e.preventDefault(); if (isShown('btnupdate') && !$('btnupdate').disabled) btnUpdate(); }
            else if (e.ctrlKey && e.shiftKey && e.key === 'Delete') { e.preventDefault(); if (S.rights.delete && !$('btnDelete').disabled) del(); }
            else if (e.ctrlKey && k === 'p') { e.preventDefault(); if (!$('btnprint').disabled) print(S.id); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); resetAll(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh(); }
            else if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); openAttachments(); }
            else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); $('DocDate').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowUp') { cb.item.focus(); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { $('grd').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowRight') { if (document.activeElement === $('grd')) $('grdContractorWages').focus(); else $('grd').focus(); }
        } else {
            if (e.ctrlKey && e.key === 'ArrowDown') { $('DataGridHistoryDetail').focus(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); showHistory(); }
            else if (e.ctrlKey && e.key === 'ArrowUp') { $('grdhistory').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowRight') { if (document.activeElement === $('grdhistory')) $('DataGridHistoryDetail').focus(); else $('grdhistory').focus(); }
        }
    }
    /* CommonServices.OnlytextdecimelFunction / ...WithMinus */
    function decimalOnly(id, minus) {
        $(id).addEventListener('keypress', function (e) {
            if (e.key.length !== 1 || e.ctrlKey || e.metaKey) return;
            var ok = /[0-9]/.test(e.key) || (e.key === '.' && this.value.indexOf('.') < 0) || (minus && e.key === '-');
            if (!ok) e.preventDefault();
        });
    }

    /* ------------------------------------------------------------------ wiring / load */
    function on(id, ev, fn) { $(id).addEventListener(ev, fn); }
    function wire() {
        var detail = guard(function () { calcDetail(); calcDisc(); });
        on('txtqty', 'input', detail); on('txtVarientEquivalent', 'input', detail);
        on('txtRate', 'input', guard(function () { netRate(); calcDetail(); calcDisc(); }));
        on('txtItemPrice', 'input', guard(netRate)); on('txtAddLessRate', 'input', guard(netRate));
        on('txtDiscountRate', 'input', guard(function () {
            if (vid(cb.disc) === 2 && num('txtDiscountRate') > 99) { msg("The Discount Percent Can't Be Greater Than 99", 'WARNING!'); setv('txtDiscountRate', '99'); }
            calcDisc();
        }));
        on('txtDiscountAmount', 'input', guard(function () { calcDisc(); }));
        on('txtFrieghtFooter', 'input', function () { calcTotals(); }); on('txtOtherWagesFooter', 'input', function () { calcTotals(); }); on('txtAdjustDiscamount', 'input', function () { calcTotals(); });
        on('txtCommrate', 'input', guard(function () { totalComm(); calcTotals(); }));
        on('txtduedays', 'input', guard(dueDate)); on('DocDate', 'input', guard(dueDate)); on('DocDate', 'change', guard(dueDate));
        on('txtExchangeRate', 'input', guard(exchangeChanged));
        on('txtExchangeRate', 'change', function () { $('txtExchangeRate').value = fmtRate(num('txtExchangeRate')); });
        on('txtManualTotalOrderAmount', 'input', guard(function () { if (num('txtManualTotalOrderAmount') > 0) calcManual(); }));
        on('txtManualTotalOrderAmount', 'change', guard(calcManual));
        on('CmbReferenceParty', 'blur', termLeave);
        ['txtCommrate', 'txtdeliverydays', 'txtduedays', 'txtExchangeRate', 'txtFrieghtFooter', 'txtOtherWagesFooter'].forEach(function (id) { decimalOnly(id, false); });
        ['txtRate', 'txtqty', 'txtDiscountRate', 'txtAddLessRate', 'txtAdjustDiscamount'].forEach(function (id) { decimalOnly(id, true); });
        $('txtManualTotalOrderAmount').addEventListener('keypress', function (e) {
            if (G.grd.count() > 0) { if (e.key.length === 1 && !e.ctrlKey && !(/[0-9]/.test(e.key) || (e.key === '.' && this.value.indexOf('.') < 0) || e.key === '-')) e.preventDefault(); }
        });
        on('RdPartyByName', 'change', rdParty); on('RdPartyByCode', 'change', rdParty);
        on('rdbtnItemName', 'change', rdItem); on('rdbtnItemCode', 'change', rdItem);
        on('RdCustomerHistorySearchByName', 'change', rdHist); on('RdCustomerHistorySearchByCode', 'change', rdHist);
        $('btnnew').onclick = resetAll; $('btnRefresh').onclick = refresh; $('btnsave').onclick = btnSave; $('btnupdate').onclick = btnUpdate; $('btnDelete').onclick = del;
        $('btnSaveAs').onclick = btnSaveAs; $('btnattachment').onclick = openAttachments; $('btnprint').onclick = function () { print(S.id); };
        $('btnshortcutkeys').onclick = function () { SE.shortcuts(SHORT); };
        $('btnplus').onclick = plus; $('btnUpdateDetail').onclick = updateDetail; $('btnCancelUpdateDetial').onclick = cancelUpdateDetail;
        $('btnUpdateDiscountInGrid').onclick = guard(updateDiscountInGrid);
        $('BtnRefreshHistory').onclick = function () { SE.api(API + '/history-combos').then(histCombos).catch(fail); };
        $('btnshowHistory').onclick = showHistory;
        $('BtnDefineBuildingHeights').onclick = function () { window.open('/inventory/lookup-definitions', '_blank'); };
        $('BtnDefineBuildingStoreys').onclick = function () { window.open('/inventory/lookup-definitions', '_blank'); };
        $('BtnDefineRefParties').onclick = function () { window.open('/party-processing/reference-parties', '_blank'); };
        $('btnDefinePlant').onclick = function () { window.open('/party-processing/reference-parties', '_blank'); };
        $('BtnDefineCity').onclick = function () { window.open('/master-data/city', '_blank'); };
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
    }
    function rdParty() { var id = vid(cb.cust); fillCustomers(); if (id > 0) cb.cust.setValue(id); cb.cust.focus(); }                 // RdPartyByName_CheckedChanged
    function rdItem() { var id = vid(cb.item); fillItems(); if (id > 0) cb.item.setValue(id); cb.item.focus(); }                        // rdbtnItemName_CheckedChanged
    function rdHist() { var id = vid(cb.hCust); histCombos(null); if (id > 0) cb.hCust.setValue(id); cb.hCust.focus(); }
    function load() {
        makeCombos();
        tabs = SE.tabs('tabControl1', function (id) { if (id === 'tabPage2') $('fromdate').focus(); else $('DocDate').focus(); });
        makeGrids();
        wire();
        SE.api(API + '/initial').then(guard(function (d) {
            S.rights = d.rights || {}; S.fmt = d.fmt || S.fmt; S.multi = !!d.multiCurrency; S.subsidiary = !!d.subsidiary; S.search = d.search || {};
            [G.grd, G.wg, G.hist, G.hd].forEach(function (g) { g.spec.dec = S.fmt.amount; });
            $('rdbtnItemCode').checked = !!S.search.itemByCode; $('rdbtnItemName').checked = !S.search.itemByCode;
            $('RdPartyByCode').checked = !!S.search.partyByCode; $('RdPartyByName').checked = !S.search.partyByCode;
            $('RdCustomerHistorySearchByCode').checked = !!S.search.partyByCode; $('RdCustomerHistorySearchByName').checked = !S.search.partyByCode;
            if (d.nextNo > 0) $('txtdocno').value = String(d.nextNo);
            fillStatic();
            S.terms = d.terms || [];
            applyLists(d);
            multiCurrency();
            histCombos(d.history || {});
            var r = S.rights;
            $('btnsave').disabled = !r.save; $('btnupdate').disabled = !r.update; $('btnDelete').disabled = !r.delete; $('btnprint').disabled = !r.print; $('ChkBox').checked = !!r.print;
            show('btnupdate', false); show('btnDelete', false); show('btnSaveAs', false); show('btnsave', true);
            show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
            $('DocDate').value = SE.today(); $('deliverystartdate').value = SE.today(); $('fromdate').value = SE.today(); $('ToDate').value = SE.today();
            dueDate(); termLeave(); commFields();
            $('DocDate').focus();
            var rec = SE.param('record'); if (rec) readById(+rec, false);
        })).catch(fail);
    }
    document.addEventListener('DOMContentLoaded', load);
})();
