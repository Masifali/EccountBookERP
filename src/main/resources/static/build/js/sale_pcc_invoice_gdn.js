/*
 * Screen 547  SaleInvoiceAgainstGDNConcrete  (Architecture.WinApp.pcc.Sale.SaleInvoiceAgainstGDNConcrete, document type 1861)
 * Page script. Desktop methods are named in the comments (SaleInvoiceAgainstGDNConcrete.cs). Server: /sale/pcc/sale-invoice-against-gdn/api
 * The detail rows come only from the "Load Gdn" dialog (LoadPendingGDNConcrete); AddLessRate is the only editable detail cell.
 * The server repeats the proportions / net amounts / bill totals on save (SalePccInvoiceGdnCalc).
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/pcc/sale-invoice-against-gdn/api', PRINT = '/sale/pcc/print/sale-invoice-against-gdn-slip', IPRINT = '/sale/pcc/print/sale-invoice-against-gdn-item-slip',
        VPRINT = '/sale/pcc/print/sale-invoice-against-gdn-voucher';
    var F = { amtRound: 0, amt: 0, rateRound: 2, rate: 2, fcy: 0 };
    var S = { id: 0, approved: false, rights: {}, files: [], removedAtt: [], existing: [], voucherHeadId: 0, multi: false, subsidiary: false, partyByCode: false, def: {}, L: {}, hist: [],
        fyStart: '', dateTypes: [], accounts: null, supGl: 0, hc: { customers: [], agents: [] }, suppliers: [], commDebit: [], orderExist: false, orderIds: '', credit: null, busy: false, rateCache: {} };
    var cb = {}, G = {}, tabs, tabs2;

    function fail(e) { return SE.dbError(e); }
    function msg(t, c) { return SE.alert(t, c); }
    function iv(c) { return +c.value() || 0; }
    function N(id) { return SE.toNum($(id).value); }
    function T(id) { return ($(id).value || '').trim(); }
    function isShown(id) { var b = $(id); return !!b && b.style.display !== 'none' && !b.hidden; }
    function show(id, on) { var b = $(id); if (!b) return; b.style.display = on ? '' : 'none'; if (b.hasAttribute('hidden')) b.hidden = !on; }
    function showCombo(id, on) { var w = $(id).closest('.dtcombo-wrap'); (w || $(id)).style.display = on ? '' : 'none'; }
    function lock(id, on) { var s = $(id), w = s.closest('.dtcombo-wrap'); s.disabled = on; if (w) Array.prototype.forEach.call(w.querySelectorAll('input,button'), function (x) { x.disabled = on; }); }
    function ci(o, k) {
        if (!o) return undefined;
        if (o[k] !== undefined) return o[k];
        var lk = String(k).toLowerCase();
        for (var p in o) if (p.toLowerCase() === lk) return o[p];
        return undefined;
    }
    function cn(o, k) { var v = ci(o, k); return v == null || v === '' ? 0 : SE.toNum(v); }
    function cs(o, k) { var v = ci(o, k); return v == null ? '' : String(v); }
    function cb2(o, k) { var v = ci(o, k); return v === true || v === 1 || String(v).toLowerCase() === 'true' || v === '1'; }

    /* ------------------------------------------------------------------ number rules */
    function rnd(v, dp) {                                                                   // Math.Round(v, dp, MidpointRounding.AwayFromZero)
        v = +v || 0;
        var s = v < 0 ? -1 : 1, a = Math.abs(v), t = String(a);
        if (t.indexOf('e') >= 0) return s * Math.round(a * Math.pow(10, dp)) / Math.pow(10, dp);
        return s * Number(Math.round(Number(t + 'e' + dp)) + 'e-' + dp);
    }
    function amt(v) { return rnd(rnd(v, F.amtRound), F.amt); }
    function fa(v) { return SE.num(amt(v), F.amt, F.amt); }
    function fr(v) { return SE.num(rnd(v, F.rate), F.rate, F.rate); }
    function ff(v) { return SE.num(rnd(v, F.fcy), F.fcy, F.fcy); }
    function f2(v) { return SE.num(v, 2, 0); }
    function d3(v) { return v === '' || v == null ? '' : f2(v); }
    function ra(v) { return v === '' || v == null ? '' : fa(v); }
    function rr(v) { return v === '' || v == null ? '' : fr(v); }
    function rf(v) { return v === '' || v == null ? '' : ff(v); }
    function nameIn(list, id, key, idKey) { for (var i = 0; i < (list || []).length; i++) if (+list[i][idKey || 'Id'] === +id) return list[i][key]; return ''; }
    function parseDate(v) {
        v = String(v || '').trim();
        var m = /^(\d{4})-(\d{1,2})-(\d{1,2})/.exec(v);
        if (m) return m[1] + '-' + ('0' + m[2]).slice(-2) + '-' + ('0' + m[3]).slice(-2);
        m = /^(\d{1,2})[\/.-](\d{1,2})[\/.-](\d{4})$/.exec(v);
        if (m) return m[3] + '-' + ('0' + m[2]).slice(-2) + '-' + ('0' + m[1]).slice(-2);
        return '';
    }

    /* ------------------------------------------------------------------ combos */
    function mk(id, key, cap, extra) {
        var o = { columns: [{ key: key, caption: cap }], textKey: key };
        for (var k in (extra || {})) o[k] = extra[k];
        return XCombo(id, o);
    }
    function makeCombos() {
        cb.sup = mk('comsupplier', '_t', 'Party Name', { popupWidth: 420, onSelect: supplierChanged });
        cb.visited = mk('CmbVisitedbyName', 'ReferencePartyName', 'Visited By');
        cb.term = mk('combpttrm', 'TermsDescription', 'Payment Term', { onSelect: termChanged });
        cb.dterm = mk('combdeliverytrm', 'DeliveryTerm', 'Delivery Term', { onSelect: deliveryTermChanged });
        cb.cur = mk('cmbCurrency', 'CurrencyCode', 'Currency', { onSelect: currencyLeave });
        cb.branch = mk('combranches', 'BranchName', 'Branch');
        cb.agent = mk('combcommAgent', 'CompanyName', 'SalesMan Name', { popupWidth: 420, onSelect: commChanged });
        cb.salesman = mk('CmbRefSalesMan', 'ReferencePartyName', 'Ref Sales Man');
        cb.ctype = mk('combCommType', 'CommissionType', 'Commission Type', { onSelect: commChanged });
        cb.proj = mk('comproject', 'ProjectName', 'Project');
        cb.cuom = mk('combcommUOM', 'UOM', 'UOM', { onSelect: commChanged });
        cb.cdebit = mk('CmbCommissionDebitAccount', 'AccountTitle', 'CommissionDebit Account Title', { popupWidth: 420 });
        cb.settle = mk('cmbSettlementDiscountAccount', 'AccountTitle', 'Settlement Discount Account', { popupWidth: 420, onSelect: recalc });
        cb.trans = mk('CmbTransporterAc', 'AccountTitle', 'Transporter Account', { popupWidth: 420, onSelect: recalc });
        cb.owages = mk('CmbOtherWagesAccount', 'AccountTitle', 'Labour Account', { popupWidth: 420, onSelect: recalc });
        cb.height = mk('Cmbheight', 'LookupName', 'Building Height');
        cb.storey = mk('Cmbstoreys', 'LookupName', 'Storeys');
        cb.refp = mk('CmbReferenceParty', 'ReferencePartyName', 'Reference Party', { popupWidth: 320, onSelect: refPartyPicked });
        cb.dateType = mk('cmbDateTypeHistory', 'Parameters', 'Parameters', { onSelect: dateTypeChanged });
        cb.hCust = mk('CmbCustomerHistory', '_t', 'Party Name', { popupWidth: 380 });
        cb.hAgent = mk('CmbCommissionAgentHistory', 'CommissionAgent', 'CommissionAgent', { popupWidth: 300 });
    }
    function keep(c, rows, fn) {
        var k = iv(c);
        c.setData(rows || []);
        if (k > 0 && !c.setValue(k)) c.clear();
        if (fn) fn();
    }
    function supRows() {
        var byCode = $('RdPartyByCode').checked;
        return S.suppliers.map(function (r) { return Object.assign({}, r, { _t: byCode ? cs(r, 'PartyCode') : cs(r, 'CompanyName') }); });
    }
    function histCustRows() {
        var byCode = $('RdCustomerHistorySearchByCode').checked;
        return S.hc.customers.map(function (r) { return Object.assign({}, r, { _t: byCode ? r.PartyCode : r.PartyName }); });
    }
    function commDebitRows() { return (S.commDebit || []).filter(function (r) { return +r.Id !== +S.supGl; }); }
    function fillLists(d) {
        var L = S.L;
        ['currencies', 'customers', 'visitedBy', 'terms', 'deliveryTerms', 'refSalesMan', 'commTypes', 'commUoms', 'commDebit', 'labourAccounts', 'refParties', 'heights', 'storeys',
            'otherItems', 'contractors', 'projects', 'branches'].forEach(function (k) { if (d[k]) L[k] = d[k]; });
        S.suppliers = L.customers || []; S.commDebit = L.commDebit || [];
        keep(cb.cur, L.currencies); keep(cb.sup, supRows()); keep(cb.agent, S.suppliers); keep(cb.visited, L.visitedBy); keep(cb.term, L.terms);
        keep(cb.salesman, L.refSalesMan); keep(cb.cdebit, commDebitRows()); keep(cb.settle, S.commDebit);
        keep(cb.trans, L.labourAccounts); keep(cb.owages, L.labourAccounts);
        keep(cb.height, L.heights); keep(cb.storey, L.storeys); keepRefParty();
        if (!cb.dterm.rows().length) { cb.dterm.setData(L.deliveryTerms); cb.dterm.setValue(1); }
        if (!cb.ctype.rows().length) cb.ctype.setData(L.commTypes);
        if (!cb.cuom.rows().length) { cb.cuom.setData(L.commUoms); cb.cuom.setValue(1); }
        keep(cb.proj, L.projects); keep(cb.branch, L.branches);
        G.exp.refresh(); G.wg.refresh();
    }
    function keepRefParty() {                                                               // ReferencePartyFill; a name that is not in the list is kept as a temporary row (the desktop combo accepts free text)
        var id = iv(cb.refp), name = cb.refp.text(), rows = (S.L.refParties || []).slice();
        if (id < 0 && name) rows.push({ Id: -1, ReferencePartyName: name });
        cb.refp.setData(rows);
        if (id !== 0) cb.refp.setValue(id);
    }
    function setRefParty(id, name) {
        var rows = cb.refp.rows(), hit = null;
        rows.forEach(function (r) { if (+r.Id > 0 && ((+id > 0 && +r.Id === +id) || (!(+id > 0) && name && String(r.ReferencePartyName).trim() === String(name).trim()))) hit = r; });
        if (hit) { cb.refp.setValue(hit.Id); return; }
        if (!name) { cb.refp.clear(); return; }
        var list = (S.L.refParties || []).slice(); list.push({ Id: -1, ReferencePartyName: name });
        cb.refp.setData(list); cb.refp.setValue(-1);
    }
    function refPartyPicked() { }
    function configDefault() {
        var d = S.def;
        if (+d.baseCurrency > 0 && iv(cb.cur) === 0) cb.cur.setValue(d.baseCurrency);
        if (+d.baseRate && N('txtExchangeRate') === 0) $('txtExchangeRate').value = fr(+d.baseRate);
    }
    function multiCurrency() {
        var on = S.multi;
        showCombo('cmbCurrency', on);
        ['txtExchangeRate', 'txtFcyAmount', 'label59', 'label60', 'label61'].forEach(function (id) { show(id, on); });
        var c = G.grd.spec.cols.filter(function (x) { return x.k === 'FcyAmount'; })[0];
        if (c) { c.hide = !on; G.grd.refresh(); }
    }

    /* ------------------------------------------------------------------ header: terms / due date / currency / supplier / commission / freight */
    function termChanged() {                                                                // combpttrm_ValueChanged
        var one = iv(cb.term) === 1;
        if (one) { $('txtduedays').value = ''; $('txtduedays').disabled = true; } else $('txtduedays').disabled = false;
        dueGen();
    }
    function dueGen() {                                                                     // DueDateGenerate
        var d = $('DocDate').value;
        if (!d) { $('duedate').value = ''; return; }
        var p = d.split('-'), dt = new Date(+p[0], +p[1] - 1, +p[2]);
        var days = T('txtduedays');
        if (days !== '') dt.setDate(dt.getDate() + (+days || 0));
        $('duedate').value = dt.getFullYear() + '-' + ('0' + (dt.getMonth() + 1)).slice(-2) + '-' + ('0' + dt.getDate()).slice(-2);
    }
    function deliveryTermChanged() {                                                        // combdeliverytrm_TextChanged: "Ponch" = outward freight account; anything else clears the transporter and the freight
        if (cb.dterm.text() === 'Ponch') return;
        cb.trans.clear(); $('txtFreightAmountHeader').value = '0'; recalc();
    }
    function supplierChanged() {                                                            // comsupplier_ValueChanged
        supplierRefresh(); recalc();
    }
    function supplierRefresh() {
        var r = cb.sup.row();
        if (!r || iv(cb.sup) <= 0) return;
        S.supGl = cn(r, 'GlAccountId'); $('txtSupplierGLId').value = String(S.supGl);
        keep(cb.cdebit, commDebitRows());                                                   // CommissionDebitAccountFill
    }
    function currencyLeave() {
        if (iv(cb.cur) === 0 || N('txtExchangeRate') !== 0) return;
        if (iv(cb.cur) !== +S.def.baseCurrency) {
            SE.api(API + '/last-rate' + SE.q({ currencyId: iv(cb.cur) })).then(function (r) { $('txtExchangeRate').value = r.rate ? fr(+r.rate) : '0'; recalc(); }).catch(fail);
        } else { $('txtExchangeRate').value = fr(+S.def.baseRate || 0); recalc(); }
    }
    function totalComm() {                                                                  // TotalCommissionAmount (Flat = rate; Percent = BillAmountWithoutCommission * rate / 100)
        if ($('txtcommrate').value === '') return;
        var t = cb.ctype.text(), rate = N('txtcommrate');
        if (t === 'Flat') $('txtcommamount').value = fa(rate);
        if (t === 'Percent' || t === 'Percentage') $('txtcommamount').value = fa(N('txtBillamountWithoutCommission') * rate / 100);
    }
    function commChanged() { recalc(); }

    /* ------------------------------------------------------------------ proportions / GridItemNetAmountCalculate / BillAmount */
    function sumExp() { var t = 0; G.exp.rows().forEach(function (r) { t += +r.Amount || 0; }); return t; }
    function sumWages() { var t = 0; G.wg.rows().forEach(function (r) { t += +r.NetAmount || 0; }); return t; }
    function portion(total, q, qty) { return rnd(total / q * qty, F.amtRound); }
    function chain() {                                                                      // WagesProportion, SettlementDiscountProportion, OtherItemsProportion, FreightHeaderProportion, GridItemNetAmountCalculate, txtExchangeRate_TextChanged
        var rows = G.grd.rows(); if (!rows.length) return;
        var q = 0; rows.forEach(function (r) { q += +r.ItemQty || 0; });
        var wt = sumWages(), et = sumExp(), fr0 = N('txtFreightAmountHeader'), st = N('txtSettlementDiscountHeader'), ex = N('txtExchangeRate');
        var credit = S.credit, setAcc = iv(cb.settle) > 0;
        rows.forEach(function (r) {
            var qty = +r.ItemQty || 0;
            if (q !== 0) {
                r.Wages = portion(wt, q, qty); r.SettleDisc = st > 0 ? portion(st, q, qty) : 0; r.Expense = portion(et, q, qty); r.Freight = fr0 > 0 ? portion(fr0, q, qty) : 0;
            }
            var wd = rnd(+r.ItemAmountWithDisc || 0, F.amtRound), sd = rnd(+r.SettleDisc || 0, F.amtRound), e = rnd(+r.Expense || 0, F.amtRound);
            if (credit !== null) { if (!credit) r.ItemNetAmount = wd; else if (setAcc) r.ItemNetAmount = wd; else r.ItemNetAmount = rnd(wd - sd, F.amtRound); }
            else if (setAcc) r.ItemNetAmount = rnd(wd + e, F.amtRound);
            else r.ItemNetAmount = rnd(wd + e - sd, F.amtRound);
            r.FcyAmount = ex > 0 ? (+r.ItemAmountWithDisc || 0) / ex : 0;
        });
        G.grd.refresh();
    }
    function jvSums() {
        var d = 0, c = 0;
        G.jv.rows().forEach(function (r) { if ((+r.AccountId || 0) > 0 || (+r.GlAccountId || 0) > 0) { d += +r.Debit || 0; c += +r.Credit || 0; } });
        return { d: d, c: c };
    }
    function billAmount() {                                                                 // BillAmount
        var rows = G.grd.rows();
        if (!rows.length) { $('txtInvoiceQty').value = '0'; $('txtInvoiceWeight').value = '0'; $('txtFcyAmount').value = '0'; return; }
        var qty = 0, wt = 0, ia = 0, da = 0, wd = 0;
        rows.forEach(function (r) {
            qty += +r.ItemQty || 0; wt += +r.NetWeight || 0;
            if (!r.IsFOC) { ia += +r.ItemAmount || 0; da += +r.DiscAmount || 0; wd += +r.ItemAmountWithDisc || 0; }
        });
        var j = jvSums(), bill = wd + rnd(sumExp(), F.amtRound) + rnd(j.c, F.amtRound) - rnd(j.d, F.amtRound);
        $('txtItemAmountHeader').value = fa(rnd(ia, F.amtRound)); $('txtDiscountHeader').value = fa(da); $('txtItemNetAmountHeader').value = fa(wd);
        var fr0 = N('txtFreightAmountHeader'), ow = N('txtOtherWagesHeader'), tr = iv(cb.trans), lw = iv(cb.owages);
        bill = tr > 0 && S.supGl > 0 && S.supGl === tr ? bill - fr0 : bill + fr0;
        bill = lw > 0 && S.supGl > 0 && S.supGl === lw ? bill - ow : bill + ow;
        $('txtBillamountWithoutCommission').value = fa(bill);
        var ca = N('txtcommamount');
        if (iv(cb.agent) > 0 && ca > 0 && iv(cb.sup) === iv(cb.agent)) { bill -= ca; $('CommNetAmtHeader').value = fa(ca); show('CommNetAmtHeader', true); show('label84', true); }
        else { $('CommNetAmtHeader').value = '0'; show('CommNetAmtHeader', false); show('label84', false); }
        if (iv(cb.settle) > 0 && S.supGl > 0 && S.supGl === iv(cb.settle)) {
            if (!S.busy) { S.busy = true; msg('SettlementDiscountAccount can not be Equal to customer Account').then(function () { S.busy = false; }); }
            return;
        }
        bill -= N('txtSettlementDiscountHeader');
        $('txtBillAmountHeader').value = fa(bill);
        $('txtInvoiceQty').value = f2(rnd(qty, 2)); $('txtInvoiceWeight').value = f2(rnd(wt, 2));
        var ex = N('txtExchangeRate');
        $('txtFcyAmount').value = ex > 0 && wd > 0 ? ff(wd / ex) : ff(0);
    }
    function recalc() {                                                                     // every grid / header change: the chain, the bill, the commission, the bill again (the commission reads the bill without commission)
        chain(); billAmount(); totalComm(); billAmount();
    }

    /* ------------------------------------------------------------------ grids */
    function discTotals(r) {                                                                // grd_CellUpdated AddLessRate branch
        var qty = +r.ItemQty || 0, unit = +r.VarientUnit || 0, price = +r.ItemPrice || 0, add = +r.AddLessRate || 0, ia = 0, da = 0, rate;
        if (qty > 0) {
            if (r.ItemWithVarient) { rate = price - add; r.Rate = rate; ia = qty * unit * rate; }
            else { rate = price; r.Rate = rate; ia = qty * rate; }
        }
        r.ItemAmount = ia;
        var t = +r.DiscountTypeId || 0, dr = +r.DiscRate || 0;
        if (t !== 0) {
            if (t === 1) da = dr;
            else if (t === 2) {
                if (dr >= 100) { msg('DiscountRate Cannot be greater than 99 when Discount Type is Percent'); dr = 99; r.DiscRate = dr; }
                da = ia * dr / 100;
            }
            r.DiscAmount = da;
        } else r.DiscAmount = 0;
        if (ia > 0 && da > 0) r.ItemAmountWithDisc = ia - da; else if (ia > 0) r.ItemAmountWithDisc = ia; else r.ItemAmountWithDisc = 0;
    }
    function expBlank() { return { Id: 0, ItemId: 0, ItemName: '', Qty: 0, Rate: 0, Amount: 0, Remarks: '' }; }
    function jvBlank() { return { AccountId: 0, GlAccountId: 0, AccountTitle: '', Remarks: '', Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0 }; }
    function makeGrids() {
        /* grd (grdSettings): everything is read only except AddLessRate */
        G.grd = SE.grid('grd', { footer: true, dec: 0,
            onEdit: function (r, k, v) {
                if (k !== 'AddLessRate' || !S.orderExist) return;
                r.AddLessRate = SE.toNum(v); discTotals(r); recalc();
            },
            cols: [
                { k: 'Id', hide: true }, { k: 'OrderId', hide: true }, { k: 'OrderDetailId', hide: true }, { k: 'OrderNo', t: 'OrderNo', w: 70 },
                { k: 'GdnId', hide: true }, { k: 'GdnDetailId', hide: true }, { k: 'GdnNo', t: 'GdnNo', w: 70 }, { k: 'WarehouseId', hide: true }, { k: 'Warehouse', t: 'Warehouse', w: 120 },
                { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'ScheduleId', hide: true }, { k: 'AttributeVarientId', hide: true },
                { k: 'AttributeVarient', t: 'AttributeVarient', w: 100 }, { k: 'VarientUnit', t: 'VarientUnit', w: 80, render: d3, sum: true, cls: 'num' }, { k: 'JobLotId', hide: true },
                { k: 'JobLot', t: 'JobLot', w: 110 }, { k: 'ItemQty', t: 'ItemQty', w: 80, render: d3, sum: true, cls: 'num' }, { k: 'BalQty', hide: true },
                { k: 'ItemWeight', t: 'ItemWeight', w: 80, render: d3, sum: true, cls: 'num' }, { k: 'NetWeight', t: 'NetWeight', w: 90, render: d3, sum: true, cls: 'num' }, { k: 'BalWeight', hide: true },
                { k: 'ItemPrice', t: 'ItemPrice', w: 80, render: rr, cls: 'num' }, { k: 'AddLessRate', t: 'AddLessRate', w: 85, render: rr, cls: 'num', edit: true }, { k: 'Rate', t: 'Rate', w: 80, render: rr, cls: 'num' },
                { k: 'ItemAmount', t: 'ItemAmount', w: 95, render: ra, sum: true, cls: 'num' }, { k: 'DiscountTypeId', hide: true }, { k: 'DiscountType', t: 'DiscountType', w: 90 },
                { k: 'DiscRate', t: 'DiscRate', w: 70, render: ra, cls: 'num' }, { k: 'DiscAmount', t: 'DiscAmount', w: 85, render: ra, sum: true, cls: 'num' },
                { k: 'ItemAmountWithDisc', t: 'ItemAmountWithDisc', w: 110, render: ra, sum: true, cls: 'num' }, { k: 'FcyAmount', t: 'FcyAmount', w: 90, render: rf, sum: true, cls: 'num', hide: true },
                { k: 'GpDate', t: 'GpDate', w: 85, f: 'sdate' }, { k: 'GpNo', t: 'GpNo', w: 60 }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 }, { k: 'CityId', hide: true }, { k: 'CityName', t: 'CityName', w: 100 },
                { k: 'Expense', t: 'Expense', w: 80, render: ra, sum: true, cls: 'num' }, { k: 'SettleDisc', t: 'SettleDisc', w: 80, render: ra, sum: true, cls: 'num' },
                { k: 'Commission', hide: true }, { k: 'Freight', hide: true }, { k: 'Wages', hide: true },
                { k: 'ItemNetAmount', t: 'ItemNetAmount', w: 100, render: ra, sum: true, cls: 'num' },
                { k: 'IsFOC', t: 'IsFOC', w: 50, f: 'chk' }, { k: 'ItemWithVariant', hide: true }] });
        G.foc = SPC.focRows($('grd'), G.grd);

        /* grdChargeToProduct (grdInvExpSettings / grdInvExp_CellUpdated) */
        G.exp = SE.grid('grdChargeToProduct', { footer: true, dec: 0,
            onBtn: function (k, r, i) { if (k === 'Delete') expDelete(i); else if (k === 'Add') expAdd(); },
            onEdit: function (r, k, v) {
                if (k === 'ItemId') { r.ItemId = +v || 0; r.ItemName = nameIn(S.L.otherItems, r.ItemId, 'OtherItemName'); }
                else if (k === 'Remarks') r.Remarks = v;
                else {
                    if (v !== '' && isNaN(Number(String(v).replace(/,/g, '')))) { msg('Please Type Only Numeric Value', 'Warning!'); return; }
                    r[k] = SE.toNum(v);
                    if (k === 'Qty' || k === 'Rate') r.Amount = amt((+r.Qty || 0) * (+r.Rate || 0));
                }
                expKeepBlank(); G.exp.refresh(); recalc();
            },
            cols: [{ k: 'Delete', t: 'X', w: 30, btn: 'X' }, { k: 'Add', t: '+', w: 30, btn: '+' }, { k: 'Id', hide: true },
                { k: 'ItemId', t: 'ItemName', w: 260, edit: true, list: function () { return S.L.otherItems; }, lk: 'Id', lt: 'OtherItemName' }, { k: 'ItemName', hide: true },
                { k: 'Qty', t: 'Qty', w: 90, edit: true, render: d3, sum: true, cls: 'num' }, { k: 'Rate', t: 'Rate', w: 90, edit: true, render: rr, cls: 'num' },
                { k: 'Amount', t: 'Amount', w: 110, edit: true, render: ra, sum: true, cls: 'num' }, { k: 'Remarks', t: 'Remarks', w: 300, edit: true }] });
        expReset();

        /* grdPartyAddLs (gridGLSettings) */
        G.jv = SE.grid('grdPartyAddLs', { footer: true, dec: 0,
            onBtn: function (k, r, i) { if (k === 'Delete') jvDelete(i); else if (k === 'Add') jvAdd(); },
            onDbl: function (r) { pickAccount(r); },
            onEdit: function (r, k, v) { jvCell(r, k, v); },
            cols: [{ k: 'Delete', t: 'X', w: 30, btn: 'X' }, { k: 'Add', t: '+', w: 30, btn: '+' }, { k: 'AccountId', hide: true }, { k: 'GlAccountId', hide: true },
                { k: 'AccountTitle', t: 'AccountTitle', w: 260 }, { k: 'Remarks', t: 'Remarks', w: 260, edit: true },
                { k: 'Percentage', t: 'Percentage', w: 90, edit: true, render: d3, sum: true, cls: 'num' }, { k: 'Qty', t: 'Qty', w: 90, edit: true, render: d3, sum: true, cls: 'num' },
                { k: 'Rate', t: 'Rate', w: 90, edit: true, render: rr, cls: 'num' },
                { k: 'Debit', t: 'Debit', w: 110, edit: true, render: ra, sum: true, cls: 'num' }, { k: 'Credit', t: 'Credit', w: 110, edit: true, render: ra, sum: true, cls: 'num' }] });
        jvReset();

        /* grdContractorWages (grdContractorWagesGridSetting): grouped by ServiceActivity on the desktop - here sorted by it */
        G.wg = SE.grid('grdContractorWages', { footer: true, dec: 0,
            onBtn: function (k, r, i) { if (k === 'Delete') wgDelete(i); else if (k === 'DeleteAll') wgDeleteAll(r); },
            onEdit: function (r, k, v) { wgCell(r, k, v); },
            cols: [{ k: 'Delete', t: 'X', w: 30, btn: 'X' }, { k: 'DeleteAll', t: 'All', w: 40, btn: 'All' },
                { k: 'Id', hide: true }, { k: 'OrderId', hide: true }, { k: 'OrderWagesId', hide: true }, { k: 'InvGdnId', hide: true }, { k: 'InvGdnWagesId', hide: true }, { k: 'ItemId', hide: true },
                { k: 'ServiceActivity', t: 'ServiceActivity', w: 150 }, { k: 'ItemName', t: 'ItemName', w: 180 }, { k: 'AttributeVarientId', hide: true }, { k: 'AttributeVarient', t: 'Varient', w: 90 },
                { k: 'VarientEquivalent', t: 'Unit', w: 60, render: d3, cls: 'num' }, { k: 'ContractorId', hide: true }, { k: 'ContractorName', t: 'ContractorName', w: 150 },
                { k: 'ContractorWagesRateScheduleId', hide: true }, { k: 'ParentUomId', hide: true },
                { k: 'Qty', t: 'Qty', w: 80, edit: true, render: d3, sum: true, cls: 'num' }, { k: 'ItemNetWeight', t: 'Weight', w: 80, render: d3, sum: true, cls: 'num' },
                { k: 'Rate', t: 'Rate', w: 80, render: rr, cls: 'num' }, { k: 'Amount', t: 'Amount', w: 95, render: ra, sum: true, cls: 'num' },
                { k: 'AddLess', t: 'AddLess', w: 80, edit: true, render: ra, sum: true, cls: 'num' }, { k: 'NetAmount', t: 'NetAmount', w: 95, render: ra, sum: true, cls: 'num' },
                { k: 'Remarks', t: 'Remarks', w: 260 }] });

        /* History (HistoryGridSettings / DetailGridSetting) */
        G.hist = SE.grid('grdHistory', { frozen: 4, dec: 0, onDbl: function (r) { getUpdate(+r.Id, false); },
            onBtn: function (k, r) {
                if (k === 'Edit') getUpdate(+r.Id, true); else if (k === 'Print') printSlip(+r.Id, false); else if (k === 'Voucher') printVoucher(+r.Id); else if (k === 'ItemSlip') printSlip(+r.Id, true);
            },
            onLink: function (k, r) { if (k === 'NoOfAttachments') showAttachments(+r.Id); },
            onSel: histSelected,
            cols: [{ k: 'Edit', t: 'Edit', w: 50, btn: 'Edit' }, { k: 'Voucher', t: 'Voucher', w: 70, btn: 'Voucher' }, { k: 'Print', t: 'Customer Slip', w: 110, btn: 'Customer Slip' }, { k: 'ItemSlip', t: 'Item Slip', w: 90, btn: 'Item Slip' },
                { k: 'Id', hide: true }, { k: 'DocumentTypeId', hide: true }, { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 100, link: true }, { k: 'DocNo', t: 'DocNo', w: 70 },
                { k: 'DocDate', t: 'DocDate', w: 90, f: 'sdate' }, { k: 'SupplierCustomerId', hide: true }, { k: 'CustomerName', t: 'CustomerName', w: 180 }, { k: 'RefrenenceNo', t: 'RefrenenceNo', w: 100 },
                { k: 'ManualBillNo', t: 'ManualBillNo', w: 100 }, { k: 'VisitedByName', t: 'VisitedByName', w: 110 }, { k: 'ReferencPartyName', t: 'ReferencPartyName', w: 120 },
                { k: 'ReferencPartyAddress', t: 'ReferencPartyAddress', w: 150 }, { k: 'ReferencPartyCellNo', t: 'ReferencPartyCellNo', w: 110 }, { k: 'CommissionAgent', t: 'CommissionAgent', w: 110 },
                { k: 'RefSalesMan', t: 'RefSalesMan', w: 100 }, { k: 'CommissionAmount', t: 'CommissionAmount', w: 100, render: ra, sum: true, cls: 'num' }, { k: 'PaymentTerm', t: 'PaymentTerm', w: 100 },
                { k: 'DueDays', t: 'DueDays', w: 60 }, { k: 'DeliveryTerm', t: 'DeliveryTerm', w: 100 }, { k: 'DeliveryStartDate', t: 'DeliveryStartDate', w: 100, f: 'sdate' },
                { k: 'IsApproved', t: 'IsApproved', w: 80, f: 'chk' }, { k: 'EntryDate', t: 'EntryDate', w: 130, f: 'dt12' }, { k: 'EntryUserName', t: 'EntryUserName', w: 110 },
                { k: 'ModifyDate', t: 'ModifyDate', w: 130, f: 'dt12' }, { k: 'ModifyUserName', t: 'ModifyUserName', w: 110 }, { k: 'ApprovedDate', t: 'ApprovedDate', w: 130, f: 'dt12' },
                { k: 'ApprovedUserName', t: 'ApprovedUserName', w: 110 }, { k: 'InvoiceQty', t: 'InvoiceQty', w: 80, render: d3, sum: true, cls: 'num' },
                { k: 'InvoiceWeight', t: 'InvoiceWeight', w: 90, render: d3, sum: true, cls: 'num' }, { k: 'TransporterName', t: 'TransporterName', w: 110 },
                { k: 'FrieghtAmountHeader', t: 'FrieghtAmountHeader', w: 110, render: ra, sum: true, cls: 'num' }, { k: 'OtherWagesAccount', t: 'OtherWagesAccount', w: 110 },
                { k: 'OtherWagesHeader', t: 'OtherWagesHeader', w: 110, render: ra, sum: true, cls: 'num' }, { k: 'BillAmount', t: 'BillAmount', w: 100, render: ra, sum: true, cls: 'num' }] });
        G.hd = SE.grid('grdDetailHistory', { dec: 0, cols: [{ k: 'Id', hide: true }, { k: 'OrderNo', t: 'OrderNo', w: 70 }, { k: 'GdnNo', t: 'GdnNo', w: 70 }, { k: 'Warehouse', t: 'Warehouse', w: 120 },
            { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 190 }, { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'AttributeVarient', t: 'AttributeVarient', w: 100 },
            { k: 'VarientUnit', t: 'VarientUnit', w: 80, render: d3, sum: true, cls: 'num' }, { k: 'JobLot', t: 'JobLot', w: 110 }, { k: 'ItemQty', t: 'ItemQty', w: 80, render: d3, sum: true, cls: 'num' },
            { k: 'ItemWeight', t: 'ItemWeight', w: 80, render: d3, sum: true, cls: 'num' }, { k: 'NetWeight', t: 'NetWeight', w: 90, render: d3, sum: true, cls: 'num' },
            { k: 'ItemPrice', t: 'ItemPrice', w: 80, render: rr, cls: 'num' }, { k: 'AddLessRate', t: 'AddLessRate', w: 80, render: rr, cls: 'num' }, { k: 'Rate', t: 'NetRate', w: 80, render: rr, cls: 'num' },
            { k: 'ItemAmount', t: 'ItemAmount', w: 95, render: ra, sum: true, cls: 'num' }, { k: 'DiscountType', t: 'DiscountType', w: 90 }, { k: 'DiscRate', t: 'DiscRate', w: 70, render: ra, cls: 'num' },
            { k: 'DiscAmount', t: 'DiscAmount', w: 85, render: ra, sum: true, cls: 'num' }, { k: 'ItemAmountWithDisc', t: 'ItemAmountWithDisc', w: 110, render: ra, sum: true, cls: 'num' },
            { k: 'FcyAmount', t: 'FcyAmount', w: 90, render: rf, sum: true, cls: 'num' }, { k: 'GpDate', t: 'GpDate', w: 85, f: 'sdate' }, { k: 'GpNo', t: 'GpNo', w: 60 }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 },
            { k: 'CityName', t: 'CityName', w: 100 }, { k: 'Expense', t: 'Expense', w: 80, render: ra, sum: true, cls: 'num' }, { k: 'Commission', t: 'Commission', w: 80, render: ra, sum: true, cls: 'num' },
            { k: 'ItemNetAmount', t: 'ItemNetAmount', w: 100, render: ra, sum: true, cls: 'num' }, { k: 'IsFOC', t: 'IsFOC', w: 50, f: 'chk' }] });
        G.focHd = SPC.focRows($('grdDetailHistory'), G.hd);
    }
    function setPlaces() { [G.grd, G.exp, G.jv, G.wg, G.hist, G.hd].forEach(function (g) { if (g) { g.spec.dec = F.amt; g.refresh(); } }); }

    /* ---- expense grid (grdInvExp_CellUpdated / grdInvExp_KeyDown) ---- */
    function expReset() { G.exp.setRows([expBlank()]); }
    function expKeepBlank() { if (G.exp.count() === 0) G.exp.addRow(expBlank()); }
    function expAdd() { G.exp.addRow(expBlank()); }
    function expDelete(i) {
        SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
            if (!yes) return;
            G.exp.removeAt(i); expKeepBlank(); recalc();
        });
    }

    /* ---- Party Add/Less grid ---- */
    function jvReset() { G.jv.setRows([jvBlank()]); }
    function jvAdd() { G.jv.addRow(jvBlank()); recalc(); }
    function jvDelete(i) {
        SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
            if (!yes) return;
            G.jv.removeAt(i);
            if (G.jv.count() === 0) G.jv.addRow(jvBlank());
            recalc();
        });
    }
    function jvCell(r, k, v) {
        if (k === 'Remarks') { r.Remarks = v; G.jv.refresh(); return; }
        if (v !== '' && isNaN(Number(String(v).replace(/,/g, '')))) { msg('Please Type Only Numeric Value', 'Warning!'); return; }
        r[k] = SE.toNum(v);
        if (k === 'Qty' || k === 'Rate') { r.Credit = rnd((+r.Qty || 0) * (+r.Rate || 0), F.amtRound); r.Debit = 0; r.Percentage = 0; }
        if (k === 'Percentage') {
            var total = 0; G.grd.rows().forEach(function (x) { total += +x.ItemAmount || 0; });
            var tp = total / 100 * (+r.Percentage || 0);
            if (tp > 0) { r.Credit = rnd(tp, F.amtRound); r.Debit = 0; } else { r.Debit = Math.abs(rnd(tp, F.amtRound)); r.Credit = 0; }
            r.Qty = 0; r.Rate = 0;
        }
        if (k === 'Credit') {
            r.Credit = rnd(r.Credit, F.amtRound);
            if ((+r.Debit || 0) > 0) { r.Credit = 0; msg('Debit Side is aleady added'); }
        }
        if (k === 'Debit') {
            r.Debit = rnd(r.Debit, F.amtRound);
            if ((+r.Credit || 0) > 0) { r.Debit = 0; msg('Credit Side is aleady added'); }
        }
        G.jv.refresh(); recalc();
    }
    function accountList() {
        return S.accounts ? Promise.resolve(S.accounts) : SE.api(API + '/accounts').then(function (a) { S.accounts = a; return a; });
    }
    function pickAccount(row) {
        accountList().then(function (a) {
            var h = '<div style="padding:4px"><input id="acFind" class="f" style="width:100%;position:static" placeholder="Search"></div><div class="dgrid" style="max-height:50vh;overflow:auto"><table class="jg"><thead><tr><th>' +
                (a.subsidiary ? 'CompanyName' : 'AccountTitle') + '</th></tr></thead><tbody id="acBody"></tbody></table></div>';
            var pop = SE.pop('Select Account', h, [{ t: 'Close' }]);
            pop.open();
            var body = pop.body.querySelector('#acBody'), find = pop.body.querySelector('#acFind'), view = [];
            function draw() {
                var q = find.value.toLowerCase();
                view = (a.rows || []).filter(function (x) { return !q || String(x.Name).toLowerCase().indexOf(q) >= 0; }).slice(0, 300);
                body.innerHTML = view.map(function (x, i) { return '<tr data-p="' + i + '" style="cursor:pointer"><td>' + SE.esc(x.Name) + '</td></tr>'; }).join('');
            }
            draw(); find.addEventListener('input', draw); find.focus();
            body.addEventListener('click', function (e) {
                var tr = e.target.closest('tr[data-p]'); if (!tr) return;
                var x = view[+tr.getAttribute('data-p')];
                pop.close();
                if (a.subsidiary) {
                    row.AccountId = x.Id; row.AccountTitle = x.Name; row.GlAccountId = x.GlAccountId;
                    if (iv(cb.sup) === +row.AccountId) { msg('Selected Account Can not be Same As Supplier Account'); row.AccountId = 0; row.GlAccountId = 0; row.AccountTitle = ''; }
                } else {
                    row.GlAccountId = x.Id; row.AccountId = 0; row.AccountTitle = x.Name;
                    if (+S.supGl === +row.GlAccountId) { msg('Selected Account Can not be Same As Supplier Account'); row.GlAccountId = 0; row.AccountId = 0; row.AccountTitle = ''; }
                }
                if (row.AccountId > 0 || row.GlAccountId > 0) { if (!String(row.Remarks || '').trim()) row.Remarks = row.AccountTitle; }
                G.jv.refresh(); recalc();
            });
        }).catch(fail);
    }

    /* ---- contractor wages grid ---- */
    function wgSort(rows) {                                                                 // Groups.Add("ServiceActivity")
        return rows.map(function (r, i) { return { r: r, i: i }; }).sort(function (a, b) {
            var x = String(a.r.ServiceActivity || ''), y = String(b.r.ServiceActivity || '');
            return x < y ? -1 : x > y ? 1 : a.i - b.i;
        }).map(function (o) { return o.r; });
    }
    function wgCell(r, k, v) {                                                              // grdContractorWages_CellUpdated
        if (v !== '' && isNaN(Number(String(v).replace(/,/g, '')))) { msg('Please Type Only Numeric Value', 'Warning!'); return; }
        if (k === 'Qty') {
            if (v === '' || r.Rate === '' || r.Rate == null) { msg('Qty And Rate Should be greater than zero...'); return; }
            r.Qty = SE.toNum(v);
            var found = null; G.grd.rows().forEach(function (x) { if (!found && +x.ItemId === +r.ItemId) found = x; });
            var weight = (found ? +found.ItemWeight || 0 : 0) * r.Qty * (+r.VarientEquivalent || 0);
            r.ItemNetWeight = weight;
            var rate = +r.Rate || 0, add = +r.AddLess || 0, p = +r.ParentUomId || 0;
            if (p === 2) { r.Amount = rate * r.Qty; r.NetAmount = r.Amount + add; }
            else if (p === 3) { r.Amount = weight / 1000 * rate; r.NetAmount = r.Amount + add; }
            else { r.Amount = 0; r.NetAmount = add; G.wg.refresh(); msg('ParentUom of RangeUom of Selected Wages Activity is not Defined...Please Check'); return; }
        } else if (k === 'AddLess') {
            r.AddLess = SE.toNum(v); r.NetAmount = (+r.Amount || 0) + r.AddLess;
        }
        G.wg.refresh(); recalc();
    }
    function wgDelete(i) { G.wg.removeAt(i); recalc(); }
    function wgDeleteAll(r) {
        var act = String(r.ServiceActivity || '');
        SE.ask('Are you sure to Delete All Rows With Activity ' + act + '?', 'Confirm').then(function (yes) {
            if (!yes) return;
            G.wg.setRows(G.wg.rows().filter(function (x) { return String(x.ServiceActivity || '') !== act; }));
            recalc();
        });
    }
    function updateWagesRates() {                                                           // DocDate_ValueChanged -> UpdateWagesRateForAllRows
        var rows = G.wg.rows().filter(function (r) { return (+r.ItemId || 0) > 0; });
        if (!rows.length) return Promise.resolve();
        var date = $('DocDate').value, cache = {};
        return Promise.all(rows.map(function (r) {
            var key = r.ItemId + '|' + (r.ContractorId || 0);
            if (!cache[key]) cache[key] = SE.api(API + '/wages-rates' + SE.q({ itemId: r.ItemId, docDate: date, contractorId: +r.ContractorId || 0 }), { quiet: true });
            return cache[key];
        })).then(function (all) {
            rows.forEach(function (r, n) {
                var list = all[n] || [], add = Math.trunc(+r.AddLess || 0), hit = null;
                list.forEach(function (x) { if (!hit && cs(x, 'WagesActivity') === String(r.ServiceActivity)) hit = x; });
                if (list.length) {
                    var rate = hit ? cn(hit, 'Rate') : 0, p = hit ? cn(hit, 'ParentUOMId') : 0;
                    r.ContractorWagesRateScheduleId = hit ? cn(hit, 'ContractorWagesRateScheduleId') : 0; r.Rate = rate; r.ParentUomId = p;
                    if (p === 2) r.Amount = rate * (+r.Qty || 0);
                    else if (p === 3) r.Amount = Math.trunc(+r.ItemNetWeight || 0) / 1000 * rate;
                    else { r.ParentUomId = 0; r.Rate = 0; r.Amount = 0; }
                } else { r.ParentUomId = 0; r.Rate = 0; r.Amount = 0; }
                r.AddLess = add; r.NetAmount = r.Amount + add;
            });
            G.wg.refresh(); recalc();
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ Load Gdn (btnLoadGdn_Click -> LoadPendingGDNConcrete) */
    var LD = null;
    function btnLoadGdn() {
        if (!S.rights.save && !S.rights.update) { /* the desktop opens the dialog without a right check; the server checks view rights */ }
        SE.api(API + '/loader/combos').then(function (c) { openLoader(c); }).catch(fail);
    }
    function openLoader(c) {
        var st = 'position:static;', h = '<div style="padding:6px;display:flex;flex-wrap:wrap;gap:8px 14px;align-items:center;font-size:12px">' +
            '<label>From Date <input type="date" id="ldFrom" class="f" style="' + st + '"></label><label>To Date <input type="date" id="ldTo" class="f" style="' + st + '"></label>' +
            '<label>No From <input id="ldNoFrom" class="f" style="' + st + 'width:70px"></label><label>To <input id="ldNoTo" class="f" style="' + st + 'width:70px"></label>' +
            '<label>Parent Category <select id="ldParent"></select></label><label>Item Category <select id="ldCat"></select></label><label>Item Type <select id="ldType"></select></label>' +
            '<label>Item Name <select id="ldItem"></select></label><label>Customer Name <select id="ldCust"></select></label>' +
            '<button type="button" id="ldShow">Show</button><button type="button" id="ldLoad">Load</button><button type="button" id="ldReset">Reset</button></div>' +
            '<div class="dgrid" id="ldMain" style="height:230px"></div><div class="dgrid" id="ldDet" style="height:200px;margin-top:6px"></div>';
        var pop = SE.pop('Pending Gdn For Invoice', h, [{ t: 'Close' }], { wide: true });
        pop.open();
        var q = function (id) { return pop.body.querySelector('#' + id); };
        LD = { pop: pop, all: [], c: {} };
        function lc(id, key, cap, rows, w) { return XCombo(id, { columns: [{ key: key, caption: cap }], textKey: key, popupWidth: w || 260 }).setData(rows || []); }
        LD.c.parent = lc(q('ldParent'), 'name', 'Parent Category', c.parentCategories); LD.c.cat = lc(q('ldCat'), 'name', 'Category', c.categories);
        LD.c.type = lc(q('ldType'), 'name', 'Item Type', c.itemTypes); LD.c.item = lc(q('ldItem'), 'name', 'Item', c.items, 340); LD.c.cust = lc(q('ldCust'), 'name', 'Customer', c.customers, 340);
        q('ldFrom').value = parseDate(S.fyStart) || SE.today(); q('ldTo').value = SE.today();
        SE.digitsOnly(q('ldNoFrom')); SE.digitsOnly(q('ldNoTo'));
        LD.main = SE.grid(q('ldMain'), { dec: 0, onSel: function (r) { loaderDetail(r); }, onCheck: function () { },
            cols: [{ k: 'Select', t: '', w: 40, sel: true }, { k: 'Id', hide: true }, { k: 'NoOfAttachments', t: 'Attached', w: 70 }, { k: 'DocNo', t: 'DocNo', w: 70 }, { k: 'DocDate', t: 'DocDate', w: 90, f: 'sdate' },
                { k: 'DeliveryOrderId', hide: true }, { k: 'DoOrderDetailId', hide: true }, { k: 'DeliveryOrderNo', t: 'DeliveryOrderNo', w: 100 }, { k: 'OrderId', hide: true }, { k: 'OrderDetailId', hide: true },
                { k: 'SaleOrderNo', t: 'SaleOrderNo', w: 90 }, { k: 'CustomerName', t: 'CustomerName', w: 180 }, { k: 'ReferenceNo', t: 'ReferenceNo', w: 100 }, { k: 'VisitedByName', t: 'VisitedByName', w: 110 },
                { k: 'ReferencPartyName', t: 'ReferenceParty', w: 120 }, { k: 'ReferencPartyCellNo', t: 'RefPartyCellNo', w: 100 }, { k: 'CommissionAgent', t: 'CommissionAgent', w: 110 },
                { k: 'RefSalesMan', t: 'RefSalesMan', w: 100 }, { k: 'CommissionAmount', t: 'CommAmount', w: 90, render: ra, cls: 'num' }, { k: 'TermsDescription', t: 'PaymentTerm', w: 100 },
                { k: 'OrderDueDays', t: 'OrderDueDays', w: 80 }, { k: 'OrderDueDate', t: 'OrderDueDate', w: 90, f: 'sdate' }, { k: 'DeliveryTerm', t: 'DeliveryTerm', w: 100 },
                { k: 'DeliveryStartDate', t: 'DeliveryStartDate', w: 100, f: 'sdate' }, { k: 'TransporterName', t: 'TransporterName', w: 110 }, { k: 'FrieghtAmountHeader', t: 'FrieghtAmountHeader', w: 100, render: ra, cls: 'num' },
                { k: 'OtherWagesAccount', t: 'WagesAccount', w: 100 }, { k: 'OtherWagesHeader', t: 'WagesAmountHeader', w: 100, render: ra, cls: 'num' }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 },
                { k: 'GpNo', t: 'GpNo', w: 60 }, { k: 'GpDate', t: 'GpDate', w: 90, f: 'sdate' }, { k: 'RemarksHeader', t: 'RemarksHeader', w: 160 }] });
        LD.det = SE.grid(q('ldDet'), { dec: 0, cols: [{ k: 'WareHouseName', t: 'WareHouseName', w: 110 }, { k: 'ItemCode', t: 'ItemCode', w: 80 }, { k: 'ItemName', t: 'ItemName', w: 180 },
            { k: 'ItemArrtibuteVarient', t: 'Varient', w: 90 }, { k: 'VarientEquivalent', t: 'VarientUnit', w: 70, render: d3, cls: 'num' }, { k: 'JobLotDescription', t: 'JobLot', w: 100 },
            { k: 'ItemQty', t: 'ItemQty', w: 70, render: d3, sum: true, cls: 'num' }, { k: 'ConfirmQty', t: 'ConfirmQty', w: 80, render: d3, sum: true, cls: 'num' },
            { k: 'ItemWeight', t: 'ItemWeight', w: 80, render: d3, cls: 'num' }, { k: 'NetWeight', t: 'NetWeight', w: 80, render: d3, sum: true, cls: 'num' },
            { k: 'ItemRateWithOutAddLess', t: 'Item Price', w: 80, render: rr, cls: 'num' }, { k: 'RateAddLess', t: 'RateAddLess', w: 80, render: rr, cls: 'num' }, { k: 'ItemRate', t: 'Net Rate', w: 80, render: rr, cls: 'num' },
            { k: 'ItemAmount', t: 'ItemAmount', w: 90, render: ra, sum: true, cls: 'num' }, { k: 'ItemDiscountType', t: 'DiscountType', w: 90 }, { k: 'ItemDiscountRate', t: 'DiscRate', w: 70, render: ra, cls: 'num' },
            { k: 'ItemDiscountAmount', t: 'ItemDiscountAmount', w: 100, render: ra, sum: true, cls: 'num' }, { k: 'ItemNetAmount', t: 'ItemNetAmount', w: 100, render: ra, sum: true, cls: 'num' },
            { k: 'CityName', t: 'CityName', w: 90 }, { k: 'IsFOC', t: 'IsFOC', w: 50, f: 'chk' }] });
        q('ldShow').onclick = ldShow; q('ldLoad').onclick = ldLoad; q('ldReset').onclick = ldReset;
        pop.el.addEventListener('keydown', function (e) { if (e.ctrlKey && e.key.toLowerCase() === 'l') { e.preventDefault(); ldLoad(); } });
    }
    function lq(id) { return LD.pop.body.querySelector('#' + id); }
    function ldReset() {                                                                    // btnReset_Click
        lq('ldFrom').value = parseDate(S.fyStart) || SE.today(); lq('ldTo').value = SE.today(); lq('ldNoFrom').value = ''; lq('ldNoTo').value = '';
        ['parent', 'cat', 'type', 'item', 'cust'].forEach(function (k) { LD.c[k].clear(); });
        LD.all = []; LD.main.setRows([]); LD.det.setRows([]);
    }
    function ldShow() {                                                                     // PendingGDNForInvoice: one header row per GDN Id, the detail rows are kept for the lower grid
        var p = { fromDate: lq('ldFrom').value, toDate: lq('ldTo').value, fromNo: SE.toInt(lq('ldNoFrom').value), toNo: SE.toInt(lq('ldNoTo').value),
            customerId: +LD.c.cust.value() || 0, categoryId: +LD.c.cat.value() || 0, itemId: +LD.c.item.value() || 0, typeId: +LD.c.type.value() || 0, parentId: +LD.c.parent.value() || 0 };
        return SE.api(API + '/loader/pending' + SE.q(p)).then(function (rows) {
            LD.all = rows || [];
            var seen = {}, heads = [];
            LD.all.forEach(function (r) { var id = cn(r, 'Id'); if (!seen[id]) { seen[id] = 1; heads.push(r); } });
            LD.main.setRows(heads); LD.det.setRows([]);
            if (heads.length) { LD.main.select(0); }
        }).catch(fail);
    }
    function loaderDetail(r) {                                                              // grdMain_SelectionChanged -> DetailGridBind
        if (!r) { LD.det.setRows([]); return; }
        var id = cn(r, 'Id');
        LD.det.setRows(LD.all.filter(function (x) { return cn(x, 'Id') === id; }));
    }
    function ldLoad() {                                                                     // btnLoadOnInvoice_Click_1
        var checked = LD.main.checked();
        if (!checked.length) { msg('Check the row first', 'Error Message'); return; }
        var orderId = null, ids = [];
        for (var i = 0; i < checked.length; i++) {
            var r = checked[i], o = cn(r, 'OrderId'), id = cn(r, 'Id'), no = cn(r, 'DocNo');
            if (orderId === null) orderId = o;
            else if (orderId !== o) { msg('Sorry! The selected rows are not of the same Order.', 'Validation'); return; }
            var zero = LD.all.some(function (x) { return cn(x, 'Id') === id && cn(x, 'ConfirmQty') === 0; });
            if (zero) { msg('ConfirmQty is 0 in one of the rows of DocNo ' + no + '. Please update it first in GDN.', 'Error Message'); return; }
            ids.push(id);
        }
        var rows = LD.all.filter(function (x) { return ids.indexOf(cn(x, 'Id')) >= 0; });
        LD.pop.close();
        loadInGridDetail(rows);
    }
    function loadInGridDetail(rows) {                                                       // LoadInGridDetail + LoadExpData
        if (!rows.length) return;
        var h = rows[0];
        // the order of an already loaded row must be the same
        var existing = G.grd.rows();
        for (var i = 0; i < rows.length; i++) {
            if (existing.length && existing.some(function (x) { return +x.OrderId !== cn(rows[i], 'OrderId'); })) { msg('Data against another OrderNo Already Exist in Detail'); return; }
        }
        cb.sup.setValue(cn(h, 'SupplierCustomerId')); supplierRefresh();
        $('txtDistance').value = cs(h, 'Distance'); $('txtSupplierReference').value = cs(h, 'ReferenceNo');
        cb.visited.setValue(cn(h, 'VisitedById'));
        cb.term.setValue(cn(h, 'PaymentTermsId')); $('txtduedays').disabled = iv(cb.term) === 1;
        $('txtduedays').value = cs(h, 'OrderDueDays'); $('duedate').value = parseDate(cs(h, 'OrderDueDate')) || $('duedate').value;
        setByText(cb.dterm, cs(h, 'DeliveryTerm'));
        $('txtDeliveryStartDate').value = parseDate(cs(h, 'DeliveryStartDate')) || $('txtDeliveryStartDate').value; $('txtDeliverydays').value = cs(h, 'DeliveryDays');
        cb.height.setValue(cn(h, 'BuildingHeightId')); cb.storey.setValue(cn(h, 'BuildingStoreyId')); $('txtArea').value = cs(h, 'BuildingArea');
        setRefParty(cn(h, 'ReferencePartyId'), cs(h, 'ReferencPartyName'));
        $('txtRefPartyAddress').value = cs(h, 'ReferencPartyAddress'); $('txtRefPartyCellNo').value = cs(h, 'ReferencPartyCellNo'); $('txtremarks').value = cs(h, 'RemarksHeader');
        cb.agent.setValue(cn(h, 'CommissionAgentId')); cb.salesman.setValue(cn(h, 'RefSalesManId'));
        setByText(cb.ctype, cs(h, 'CommissionType')); $('txtcommrate').value = fr(cn(h, 'CommissionRate')); $('txtcommamount').value = fa(cn(h, 'CommissionAmount'));
        if (cn(h, 'CurrencyIdHeader') > 0) cb.cur.setValue(cn(h, 'CurrencyIdHeader'));
        if (cn(h, 'ExchangeRateHeader') > 0) $('txtExchangeRate').value = String(cn(h, 'ExchangeRateHeader'));
        cb.owages.setValue(cn(h, 'OtherWagesAccountId')); $('txtOtherWagesHeader').value = fa(cn(h, 'OtherWagesHeader'));
        $('txtInvoiceQty').value = f2(cn(h, 'OrderQtyHeader')); $('txtInvoiceWeight').value = f2(cn(h, 'OrderWeightHeader'));
        cb.trans.setValue(cn(h, 'TransporterId')); $('txtFreightAmountHeader').value = fa(cn(h, 'FrieghtAmountHeader'));
        lock('comsupplier', true); if (iv(cb.agent) > 0) lock('combcommAgent', true);
        S.orderExist = true;
        var have = {}; G.grd.rows().forEach(function (x) { have[x.GdnDetailId] = 1; });
        var ids = {}, list = [];
        rows.forEach(function (r) {
            var did = cn(r, 'DetailId'); ids[cn(r, 'Id')] = 1;
            if (have[did]) return;
            have[did] = 1;
            list.push({ Id: 0, OrderId: cn(r, 'OrderId'), OrderDetailId: cn(r, 'OrderDetailId'), OrderNo: cs(r, 'SaleOrderNo'), GdnId: cn(r, 'Id'), GdnDetailId: did, GdnNo: cs(r, 'DocNo'),
                WarehouseId: cn(r, 'WarehouseId'), Warehouse: cs(r, 'WareHouseName'), ItemId: cn(r, 'ItemId'), ItemName: cs(r, 'ItemName'), ItemCode: cs(r, 'ItemCode'), ScheduleId: cn(r, 'ScheduleId'),
                AttributeVarientId: cn(r, 'ItemAttributeVarientId'), AttributeVarient: cs(r, 'ItemArrtibuteVarient'), VarientUnit: cn(r, 'VarientEquivalent'), JobLotId: cn(r, 'JobLotId'),
                JobLot: cs(r, 'JobLotDescription'), ItemQty: cn(r, 'ConfirmQty'), BalQty: cn(r, 'ConfirmQty'), ItemWeight: cn(r, 'ItemWeight'), NetWeight: cn(r, 'NetWeight'), BalWeight: cn(r, 'NetWeight'),
                ItemPrice: cn(r, 'ItemRateWithOutAddLess'), AddLessRate: cn(r, 'RateAddLess'), Rate: cn(r, 'ItemRate'), ItemAmount: cn(r, 'ItemAmount'), DiscountTypeId: cn(r, 'ItemDiscountTypeId'),
                DiscountType: cs(r, 'ItemDiscountType'), DiscRate: cn(r, 'ItemDiscountRate'), DiscAmount: cn(r, 'ItemDiscountAmount'), ItemAmountWithDisc: cn(r, 'ItemNetAmount'), FcyAmount: cn(r, 'ItemNetAmount'),
                GpDate: parseDate(cs(r, 'GpDate')), GpNo: cs(r, 'GpNo'), VehicleNo: cs(r, 'VehicleNo'), CityId: cn(r, 'CityId'), CityName: cs(r, 'CityName'),
                Expense: 0, SettleDisc: 0, Commission: 0, Freight: 0, Wages: 0, ItemNetAmount: 0, IsFOC: cb2(r, 'IsFOC'), ItemWithVarient: cb2(r, 'ItemWithVarient') });
        });
        list.forEach(function (x) { G.grd.addRow(x); });
        S.orderIds = Object.keys(ids).reduce(function (a, k) { return a + ',' + k; }, S.orderIds || '');
        var gdnIds = Object.keys(ids).reduce(function (a, k) { return a + ',' + k; }, '');
        return SE.api(API + '/loader/wages' + SE.q({ ids: gdnIds })).then(function (w) { loadExpData(w || []); recalc(); }).catch(fail);
    }
    function loadExpData(items) {                                                           // LoadExpData
        if (!items.length) return;
        var rows = G.wg.rows();
        if (rows.length === 1 && !(+rows[0].ItemId) && !(+rows[0].Amount)) rows = [];
        var flag = false;
        items.forEach(function (r) {
            if (rows.length) { rows.forEach(function (x) { if (+x.InvGdnWagesId === cn(r, 'Id')) flag = true; }); }
            if (!flag) {                                                                    // the desktop never resets the flag once a duplicate was found
                rows.push({ Id: 0, OrderId: cn(r, 'SaleOrderId'), OrderWagesId: cn(r, 'SaleOrderWagesId'), InvGdnId: cn(r, 'InvGdnId'), InvGdnWagesId: cn(r, 'Id'), ItemId: cn(r, 'ItemId'),
                    ItemName: cs(r, 'ItemName'), AttributeVarientId: cn(r, 'ItemAttributeVarientId'), AttributeVarient: cs(r, 'ItemAttributeVarient'), VarientEquivalent: cn(r, 'VarientEquivalent'),
                    ContractorId: cn(r, 'ContractorId'), ContractorName: cs(r, 'ContractorName'), ContractorWagesRateScheduleId: cn(r, 'ContractorWagesRateScheduleId'), ServiceActivity: cs(r, 'ServiceActivity'),
                    ParentUomId: cn(r, 'ParentUomId'), Qty: cn(r, 'Qty'), ItemNetWeight: cn(r, 'ItemNetWeight'), Rate: cn(r, 'Rate'), Amount: cn(r, 'Amount'), AddLess: cn(r, 'AddLess'),
                    NetAmount: cn(r, 'NetAmount'), Remarks: cs(r, 'Remarks') });
            }
        });
        G.wg.setRows(wgSort(rows));
    }

    /* ------------------------------------------------------------------ Reset / Refresh / Load */
    function reset() {                                                                      // btnNew_Click -> Reset (the reference party, visited by, terms and doc date are not cleared, as on the desktop)
        S.files = []; S.removedAtt = []; S.existing = []; S.id = 0; S.voucherHeadId = 0; S.approved = false; S.orderExist = false; S.orderIds = '';
        lock('comsupplier', false); lock('combcommAgent', false);
        cb.sup.clear(); cb.agent.clear(); cb.ctype.clear(); cb.cuom.clear(); cb.cdebit.clear(); cb.trans.clear(); cb.owages.clear(); cb.settle.clear();
        ['txtSupplierReference', 'txtbillno', 'txtInvoiceQty', 'txtInvoiceWeight', 'txtExchangeRate', 'txtCommissionRemarks', 'txtcommrate', 'txtcommamount', 'txtremarks',
            'txtItemAmountHeader', 'txtDiscountHeader', 'txtItemNetAmountHeader', 'txtBillamountWithoutCommission', 'txtBillAmountHeader', 'txtFreightAmountHeader', 'txtOtherWagesHeader',
            'txtSettlementDiscountHeader'].forEach(function (id) { $(id).value = ''; });
        $('txtduedays').value = '0'; dueGen();
        G.grd.setRows([]); G.wg.setRows([]); expReset(); jvReset();
        show('btnSave', true); show('btnUpdate', false); show('btnDelete', false);
        configDefault();
        billAmount();
        $('DocDate').focus();
        return SE.api(API + '/next-no').then(function (r) { $('txtdocno').value = r.nextNo > 0 ? String(r.nextNo) : $('txtdocno').value; }).catch(fail);
    }
    function refresh() { return SE.api(API + '/lists').then(function (d) { fillLists(d); S.accounts = null; configDefault(); }).catch(fail); }
    function applyInitial(d) {
        S.rights = d.rights || {}; S.fyStart = d.fyStart || ''; S.dateTypes = d.dateTypes || [];
        var s = d.settings || {};
        S.partyByCode = !!s.partyByCode; S.multi = !!s.multiCurrency; S.subsidiary = !!s.subsidiary;
        S.credit = s.creditInSaleGl === '' || s.creditInSaleGl == null ? null : !!s.creditInSaleGl;
        S.def = d.defaults || {};
        var f = d.fmt || {};
        F.amtRound = f.amountRound == null ? 0 : f.amountRound; F.amt = f.amount == null ? 0 : f.amount; F.rateRound = f.rateRound == null ? 2 : f.rateRound;
        F.rate = f.rate == null ? 2 : f.rate; F.fcy = f.fcy == null ? 0 : f.fcy;
        setPlaces();
        $('txtdocno').value = d.nextNo > 0 ? String(d.nextNo) : $('txtdocno').value;
        if (S.partyByCode) { $('RdPartyByCode').checked = true; $('RdCustomerHistorySearchByCode').checked = true; }
        else { $('RdPartyByName').checked = true; $('RdCustomerHistorySearchByName').checked = true; }
        cb.dateType.setData(S.dateTypes);
        S.hc = d.history || S.hc;
        keep(cb.hCust, histCustRows()); keep(cb.hAgent, S.hc.agents);
    }
    function load() {
        makeCombos();
        tabs = SE.tabs('tabControl1', function (id) { if (id === 'tabPage2') tabHistory(); else $('DocDate').focus(); });
        tabs2 = SE.tabs('tabControl2', function () { });
        makeGrids();
        ['txtExchangeRate', 'txtcommrate', 'txtFreightAmountHeader', 'txtOtherWagesHeader', 'txtSettlementDiscountHeader', 'txtDeliverydays', 'txtDistance'].forEach(function (id) {
            var e = $(id); if (e) e.addEventListener('keypress', function (ev) { if (ev.key.length === 1 && !/[\d.]/.test(ev.key) && !ev.ctrlKey) ev.preventDefault(); });      // OnlytextdecimelFunction
        });
        SE.digitsOnly($('txtduedays')); SE.digitsOnly($('txtFromNoHistory')); SE.digitsOnly($('txtToDocNoHistory'));
        $('txtduedays').addEventListener('input', dueGen);
        $('DocDate').addEventListener('change', function () { dueGen(); updateWagesRates(); });
        $('txtcommrate').addEventListener('input', commChanged);
        ['txtFreightAmountHeader', 'txtOtherWagesHeader', 'txtSettlementDiscountHeader', 'txtExchangeRate'].forEach(function (id) { $(id).addEventListener('input', recalc); });
        $('txtExchangeRate').addEventListener('change', function () { $('txtExchangeRate').value = fr(N('txtExchangeRate')); });
        $('RdPartyByName').addEventListener('change', partyRdb); $('RdPartyByCode').addEventListener('change', partyRdb);
        $('RdCustomerHistorySearchByName').addEventListener('change', histRdb); $('RdCustomerHistorySearchByCode').addEventListener('change', histRdb);
        Promise.all([SE.api(API + '/initial'), SE.api(API + '/lists')]).then(function (a) {
            applyInitial(a[0]); fillLists(a[1]);
            var r = S.rights;
            $('btnSave').disabled = !r.save; $('btnUpdate').disabled = !r.update; $('btnDelete').disabled = !r.delete;
            $('btnPartySlip').disabled = !r.print; $('btnItemPrint').disabled = !r.print; $('btnPrint').disabled = !r.print; $('ChkPrintPartySlip').disabled = !r.print; $('ChkBok').disabled = !r.print;
            $('ChkPrintPartySlip').checked = !!r.print; $('ChkBok').checked = !!r.print;
            multiCurrency();
            show('btnSave', true); show('btnUpdate', false); show('btnDelete', false); show('CommNetAmtHeader', false); show('label84', false);
            $('DocDate').value = SE.today(); dueGen();
            $('txtDeliveryStartDate').value = SE.today();
            $('txtFromdateHistory').value = SE.today(); $('txtToDateHistory').value = SE.today();
            if (!iv(cb.dateType) && cb.dateType.rows().length) cb.dateType.setValue(cb.dateType.rows()[0].Id);
            configDefault();
            $('DocDate').focus();
            var rec = SE.param('record'); if (rec) getUpdate(+rec, true);
        }).catch(fail);
        wire();
    }
    function partyRdb() {
        if (!S.suppliers.length) return;
        var id = iv(cb.sup);
        cb.sup.setData(supRows()); if (id > 0) cb.sup.setValue(id);
        cb.sup.focus();
    }
    function histRdb() {
        if (!S.hc.customers.length) return;
        var id = iv(cb.hCust);
        cb.hCust.setData(histCustRows()); if (id > 0) cb.hCust.setValue(id);
        cb.hCust.focus();
    }

    /* ------------------------------------------------------------------ Insert / btnSave / btnUpdate */
    function lineOf(r) {
        return { id: +r.Id || 0, orderId: +r.OrderId || 0, orderDetailId: +r.OrderDetailId || 0, orderNo: SE.toInt(r.OrderNo), gdnId: +r.GdnId || 0, gdnDetailId: +r.GdnDetailId || 0,
            warehouseId: +r.WarehouseId || 0, itemId: +r.ItemId || 0, scheduleId: SE.toInt(r.ScheduleId), attributeVarientId: +r.AttributeVarientId || 0, jobLotId: +r.JobLotId || 0,
            gpNo: SE.toInt(r.GpNo), cityId: +r.CityId || 0, discountTypeId: +r.DiscountTypeId || 0, varientUnit: SE.toNum(r.VarientUnit), qty: SE.toNum(r.ItemQty), itemWeight: SE.toNum(r.ItemWeight),
            netWeight: SE.toNum(r.NetWeight), itemPrice: SE.toNum(r.ItemPrice), addLessRate: SE.toNum(r.AddLessRate), rate: SE.toNum(r.Rate), itemAmount: SE.toNum(r.ItemAmount),
            discRate: SE.toNum(r.DiscRate), discAmount: SE.toNum(r.DiscAmount), itemAmountWithDisc: SE.toNum(r.ItemAmountWithDisc), gpDate: parseDate(r.GpDate),
            vehicleNo: r.VehicleNo == null ? '' : String(r.VehicleNo), isFoc: !!r.IsFOC };
    }
    function expOf(r) { return { id: +r.Id || 0, itemId: +r.ItemId || 0, qty: SE.toNum(r.Qty), rate: SE.toNum(r.Rate), amount: SE.toNum(r.Amount), remarks: r.Remarks == null ? '' : String(r.Remarks) }; }
    function jvOf(r) {
        return { accountId: +r.AccountId || 0, glAccountId: +r.GlAccountId || 0, remarks: r.Remarks == null ? '' : String(r.Remarks), percentage: +r.Percentage || 0, qty: +r.Qty || 0,
            rate: +r.Rate || 0, debit: +r.Debit || 0, credit: +r.Credit || 0 };
    }
    function wgOf(r) {
        return { id: +r.Id || 0, orderId: +r.OrderId || 0, orderWagesId: +r.OrderWagesId || 0, itemId: +r.ItemId || 0, attributeVarientId: +r.AttributeVarientId || 0, contractorId: +r.ContractorId || 0,
            scheduleId: +r.ContractorWagesRateScheduleId || 0, parentUomId: +r.ParentUomId || 0, serviceActivity: r.ServiceActivity == null ? '' : String(r.ServiceActivity),
            remarks: r.Remarks == null ? '' : String(r.Remarks), itemName: r.ItemName == null ? '' : String(r.ItemName), varientUnit: SE.toNum(r.VarientEquivalent), qty: SE.toNum(r.Qty),
            itemNetWeight: SE.toNum(r.ItemNetWeight), rate: SE.toNum(r.Rate), amount: SE.toNum(r.Amount), addLess: SE.toNum(r.AddLess), netAmount: SE.toNum(r.NetAmount) };
    }
    function formValid() {                                                                  // FormValidation
        function stop(m, f) { return msg(m).then(function () { if (f) f(); return false; }); }
        var no = T('txtdocno');
        if (no === '' || no === '0') return stop('DocNo Field is Required', function () { $('txtdocno').focus(); });
        if (!cb.sup.row() || iv(cb.sup) === 0) return stop('CustomerName Field is Required', function () { cb.sup.focus(); });
        if (!cb.term.row() || iv(cb.term) === 0) return stop('Payment Term Field is Required', function () { cb.term.focus(); });
        if (!cb.dterm.row() || iv(cb.dterm) === 0) return stop('Delivery Term Field is Required', function () { cb.dterm.focus(); });
        if (cb.refp.text().trim() === '') return stop('Ref Party Name Field is Required', function () { cb.refp.focus(); });
        if (T('txtRefPartyAddress') === '') return stop('Ref Party Address Field is Required', function () { $('txtRefPartyAddress').focus(); });
        if (T('txtRefPartyCellNo') === '') return stop('Ref Party CellNo Field is Required', function () { $('txtRefPartyCellNo').focus(); });
        if (S.multi) {
            if (!cb.cur.row() || iv(cb.cur) === 0) return stop('Fcy Code Field is Required', function () { cb.cur.focus(); });
            if (T('txtExchangeRate') === '' || T('txtExchangeRate') === '0') return stop('Exchange Rate Field is Required', function () { $('txtExchangeRate').focus(); });
            if (T('txtFcyAmount') === '' || T('txtFcyAmount') === '0') return stop('Fcy Amount Rate Field is Required', function () { $('txtFcyAmount').focus(); });
        } else {
            if (!cb.cur.row() || iv(cb.cur) === 0) return stop('Please Configure Your Base Currency In configurations', function () { cb.cur.focus(); });
            if (T('txtExchangeRate') === '' || T('txtExchangeRate') === '0') return stop('Please Configure Your Base Currency Rate In configurations', function () { $('txtExchangeRate').focus(); });
        }
        if (cb.term.text() === 'Credit' && T('txtduedays') === '') return stop('Due Days Field is Required', function () { $('txtduedays').focus(); });
        return Promise.resolve(true);
    }
    function insert() {
        if (G.grd.count() === 0) return msg('Grid Record Not Found', 'Database Error');
        return formValid().then(function (ok) {
            if (!ok) return;
            function stop(m, f) { return msg(m, 'Database Error').then(function () { if (f) f(); }); }
            if (N('txtFreightAmountHeader') > 0) {
                if (iv(cb.trans) === 0) return stop('Transporter Account field Required', function () { cb.trans.focus(); });
                if (iv(cb.trans) === +S.supGl) return stop('Transporter Account can not be same as Customer Please check', function () { cb.trans.focus(); });
            }
            if (N('txtOtherWagesHeader') > 0) {
                if (iv(cb.owages) === 0) return stop('Labour Account field Required', function () { cb.owages.focus(); });
                if (iv(cb.owages) === +S.supGl) return stop('Labour Account can not be same as Customer Please check', function () { cb.owages.focus(); });
            }
            if (N('txtcommamount') > 0) {
                if (iv(cb.agent) === 0) return msg('Please Select Commission Agent Account First').then(function () { cb.agent.focus(); });
                if (iv(cb.salesman) === 0) return msg('Please Select Ref Sales Man First').then(function () { cb.salesman.focus(); });
                if (iv(cb.cdebit) === 0) return stop('CommissionDebitAccount Account field Required', function () { cb.cdebit.focus(); });
                if (iv(cb.cdebit) === iv(cb.agent)) return stop('CommissionDebit Account can not be same as Commission agent Please check');
            }
            return SE.ask(S.id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
                if (!yes) return;
                var jv = G.jv.rows(), bad = -1;
                jv.forEach(function (r, i) { if (bad < 0 && ((+r.Credit || 0) > 0 || (+r.Debit || 0) > 0) && !(+r.AccountId > 0) && !(+r.GlAccountId > 0)) bad = i; });
                if (bad >= 0) return msg('Please Select an Account Party Addless Grid in Row no: ' + (bad + 1));
                var eb = -1;
                G.exp.rows().forEach(function (r, i) { if (eb < 0 && (+r.Amount || 0) > 0 && !(+r.ItemId > 0)) eb = i; });
                if (eb >= 0) return msg('Please Select an Item Against Expense First');
                recalc();
                if (N('txtItemAmountHeader') === 0 || N('txtItemNetAmountHeader') === 0) return stop('ItemAmount and ItemNetAmount can not equal to zero Please check');
                var wages = G.wg.rows().filter(function (r) { return (+r.ContractorWagesRateScheduleId || 0) > 0 || (+r.ContractorId || 0) > 0 || (+r.ItemId || 0) > 0 || (+r.AttributeVarientId || 0) > 0; });
                var body = { id: S.id, docDate: $('DocDate').value, docNo: T('txtdocno'), referenceNo: $('txtSupplierReference').value, manualBillNo: $('txtbillno').value, remarks: $('txtremarks').value,
                    dueDays: T('txtduedays'), dueDate: $('duedate').value, distance: $('txtDistance').value, deliveryStartDate: $('txtDeliveryStartDate').value, deliveryDays: T('txtDeliverydays'),
                    deliveryTerm: cb.dterm.text(), buildingArea: $('txtArea').value, refPartyName: cb.refp.text().trim(), refPartyAddress: $('txtRefPartyAddress').value, refPartyCellNo: $('txtRefPartyCellNo').value,
                    supplierCustomerId: iv(cb.sup), visitedById: iv(cb.visited), paymentTermId: iv(cb.term), commissionAgentId: iv(cb.agent), refSalesManId: iv(cb.salesman),
                    commissionDebitAccountId: iv(cb.cdebit), currencyId: iv(cb.cur), buildingHeightId: iv(cb.height), buildingStoreyId: iv(cb.storey), referencePartyId: Math.max(0, iv(cb.refp)),
                    transporterId: iv(cb.trans), otherWagesAccountId: iv(cb.owages), settlementDiscountAccountId: iv(cb.settle), commissionType: cb.ctype.text(), commissionUom: cb.cuom.text(),
                    commissionRemarks: $('txtCommissionRemarks').value, commissionRate: N('txtcommrate'), commissionAmount: N('txtcommamount'), exchangeRate: N('txtExchangeRate'),
                    freightAmount: N('txtFreightAmountHeader'), otherWages: N('txtOtherWagesHeader'), settlementDiscount: N('txtSettlementDiscountHeader'),
                    rows: G.grd.rows().map(lineOf), expenses: G.exp.rows().filter(function (r) { return (+r.ItemId || 0) > 0 || (+r.Amount || 0) > 0; }).map(expOf), journals: jv.map(jvOf), wages: wages.map(wgOf),
                    attachments: { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removedAtt } };
                return SE.api(API, { method: 'POST', body: body }).then(function (r) {
                    return msg(r.message).then(function () {
                        var vch = $('ChkBok').checked, slip = $('ChkPrintPartySlip').checked;
                        return reset().then(function () { if (vch) printVoucher(r.id); if (slip) printSlip(r.id, false); });
                    });
                }).catch(function (e) { return msg(e.message, 'Database Error'); });
            });
        });
    }
    function btnSave() { S.id = 0; return insert(); }
    function btnUpdate() {
        if (S.approved) return msg('Record Not Update beacause Record has approved');
        return insert();
    }

    /* ------------------------------------------------------------------ ReadById */
    function setByText(c, text) {
        var rows = c.rows(), t = String(text == null ? '' : text).trim();
        for (var i = 0; i < rows.length; i++) { if (String(rows[i][Object.keys(rows[i]).filter(function (k) { return k !== 'Id'; })[0]]).trim() === t) { c.setValue(rows[i].Id); return true; } }
        c.clear(); return false;
    }
    function getUpdate(id, resetFirst) {
        var p = resetFirst ? reset() : Promise.resolve();
        return p.then(function () { S.id = id; return SE.api(API + '/' + id); }).then(function (d) {
            var h = d.head, lines = d.lines || [];
            tabs.select('tabPage1');
            $('DocDate').value = SE.dateInput(ci(h, 'DocDate')); $('txtdocno').value = cs(h, 'DocNo');
            $('txtbillno').value = cs(h, 'ManualBillNo'); $('txtSupplierReference').value = cs(h, 'RefrenenceNo');
            cb.sup.setValue(cn(h, 'SupplierCustomerId')); supplierRefresh();
            cb.visited.setValue(cn(h, 'VisitedById')); $('txtremarks').value = cs(h, 'RemarksHeader');
            cb.term.setValue(cn(h, 'PaymentTermsId'));
            $('txtduedays').value = cs(h, 'DueDays'); $('txtduedays').disabled = iv(cb.term) === 1; $('duedate').value = parseDate(cs(h, 'DueDate')) || $('duedate').value;
            setByText(cb.dterm, cs(h, 'DeliveryTerm'));
            $('txtDeliveryStartDate').value = parseDate(cs(h, 'DeliveryStartDate')); $('txtDeliverydays').value = cs(h, 'DeliveryDays'); $('txtDistance').value = cs(h, 'Distance');
            cb.height.setValue(cn(h, 'BuildingHeightId')); cb.storey.setValue(cn(h, 'BuildingStoreyId')); $('txtArea').value = cs(h, 'BuildingArea');
            setRefParty(cn(h, 'ReferencePartyId'), cs(h, 'ReferencPartyName'));
            $('txtRefPartyAddress').value = cs(h, 'ReferencPartyAddress'); $('txtRefPartyCellNo').value = cs(h, 'ReferencPartyCellNo');
            cb.agent.setValue(cn(h, 'CommissionAgentId')); cb.salesman.setValue(cn(h, 'RefSalesManId'));
            setByText(cb.ctype, cs(h, 'CommissionType'));
            $('txtcommrate').value = fr(cn(h, 'CommissionRate'));
            var uom = cn(h, 'CommissionUom'), hit = null;
            cb.cuom.rows().forEach(function (r) { if (SE.toNum(r.UOM) === uom) hit = r; });
            if (hit) cb.cuom.setValue(hit.Id); else cb.cuom.clear();
            $('txtcommamount').value = fa(cn(h, 'CommissionAmount')); $('txtCommissionRemarks').value = cs(h, 'CommissionRemarks');
            $('txtInvoiceQty').value = f2(cn(h, 'InvoiceQty')); $('txtInvoiceWeight').value = f2(cn(h, 'InvoiceWeight'));
            cb.cur.setValue(cn(h, 'CurrencyId')); $('txtFcyAmount').value = ff(cn(h, 'FcyAmount'));
            cb.trans.setValue(cn(h, 'TransporterId')); cb.owages.setValue(cn(h, 'OtherWagesAccountId')); cb.settle.setValue(cn(h, 'SettlementDiscountAccountId'));
            cb.cdebit.setValue(cn(h, 'CommissionDebitAcId'));
            $('txtItemAmountHeader').value = fa(cn(h, 'ItemAmountHeader')); $('txtDiscountHeader').value = fa(cn(h, 'DiscountAmountHeader')); $('txtItemNetAmountHeader').value = fa(cn(h, 'ItemNetAmountHeader'));
            $('txtOtherWagesHeader').value = fa(cn(h, 'OtherWagesHeader')); $('txtFreightAmountHeader').value = fa(cn(h, 'FrieghtAmountHeader'));
            $('txtSettlementDiscountHeader').value = fa(cn(h, 'SettlementDiscountHeader')); $('txtBillAmountHeader').value = fa(cn(h, 'BillAmount'));
            cb.branch.setValue(cn(h, 'BranchesId')); cb.proj.setValue(cn(h, 'ProjectsId'));
            S.approved = cb2(h, 'IsApproved');
            if (cn(h, 'CurrencyId') > 0) $('txtExchangeRate').value = String(cn(h, 'ExchangeRate')); else configDefault();
            S.voucherHeadId = +d.voucherHeadId || 0;
            G.grd.setRows(lines.map(function (l) {
                return { Id: cn(l, 'Id'), OrderId: cn(l, 'SaleOrderId'), OrderDetailId: cn(l, 'SaleOrderDetailId'), OrderNo: cs(l, 'SaleOrderNo'), GdnId: cn(l, 'InvGdnId'), GdnDetailId: cn(l, 'InvGdnDetailId'),
                    GdnNo: cs(l, 'GdnNo'), WarehouseId: cn(l, 'WarehouseId'), Warehouse: cs(l, 'WareHouseName'), ItemId: cn(l, 'ItemId'), ItemName: cs(l, 'ItemName'), ItemCode: cs(l, 'ItemCode'),
                    ScheduleId: cn(l, 'ScheduleId'), AttributeVarientId: cn(l, 'ItemAttributeVarientId'), AttributeVarient: cs(l, 'ItemAttributeVarient'), VarientUnit: cn(l, 'VarientEquivalent'),
                    JobLotId: cn(l, 'JobLotId'), JobLot: cs(l, 'JobLotDescription'), ItemQty: cn(l, 'ItemQty'), BalQty: cn(l, 'ItemQty'), ItemWeight: cn(l, 'ItemWeight'), NetWeight: cn(l, 'ItemNetWeight'),
                    BalWeight: cn(l, 'ItemNetWeight'), ItemPrice: cn(l, 'ItemRateWithOutAddLess'), AddLessRate: cn(l, 'RateAddLess'), Rate: cn(l, 'ItemRate'), ItemAmount: cn(l, 'ItemAmount'),
                    DiscountTypeId: cn(l, 'ItemDiscountTypeId'), DiscountType: cs(l, 'ItemDiscountType'), DiscRate: cn(l, 'ItemDiscountRate'), DiscAmount: cn(l, 'ItemDiscountAmount'),
                    ItemAmountWithDisc: cn(l, 'ItemAmountWithDisc'), FcyAmount: cn(l, 'FcyAmount'), GpDate: parseDate(cs(l, 'GpDate')), GpNo: cs(l, 'GpNo'), VehicleNo: cs(l, 'VehicleNo'),
                    CityId: cn(l, 'CityId'), CityName: cs(l, 'CityName'), Expense: cn(l, 'ExpenseAmount'), SettleDisc: cn(l, 'JournalAmount'), Commission: cn(l, 'CommissionAmount'),
                    Freight: cn(l, 'FreightAmount'), Wages: cn(l, 'WagesAmount'), ItemNetAmount: cn(l, 'ItemNetAmount'), IsFOC: cb2(l, 'IsFOC'), ItemWithVarient: cb2(l, 'ItemWithVarient') };
            }));
            S.orderExist = lines.length > 0 && cn(lines[lines.length - 1], 'SaleOrderId') > 0;
            var ex = (d.expenses || []).map(function (e) {
                return { Id: cn(e, 'Id'), ItemId: cn(e, 'InvOtherItemId'), ItemName: cs(e, 'OtherItemName'), Qty: cn(e, 'Qty'), Rate: cn(e, 'Rate'), Amount: cn(e, 'Amount'), Remarks: cs(e, 'Remarks') };
            });
            G.exp.setRows(ex.length ? ex : [expBlank()]);
            G.jv.setRows((d.journals || []).map(function (j) {
                var acc = cn(j, 'TransporterSupCustId'), gl = cn(j, 'ChartofAccountId');
                return { AccountId: acc, GlAccountId: gl, AccountTitle: acc > 0 ? cs(j, 'SupplierCustomer') : (gl > 0 ? cs(j, 'AccountTitle') : ''), Remarks: cs(j, 'JvRemarks'), Percentage: cn(j, 'JvPrcnt'),
                    Qty: cn(j, 'JvQty'), Rate: cn(j, 'JvRate'), Debit: cn(j, 'JvDebit'), Credit: cn(j, 'JvCredit') };
            }));
            if (!G.jv.count()) G.jv.setRows([jvBlank()]);
            G.wg.setRows(wgSort((d.wages || []).map(function (w) {                           // the desktop swaps (InvGdnId, InvGdnWagesId) with (SaleOrderId, SaleOrderWagesId) here; the web keeps the stored ids
                return { Id: cn(w, 'Id'), OrderId: cn(w, 'SaleOrderId'), OrderWagesId: cn(w, 'SaleOrderWagesId'), InvGdnId: cn(w, 'InvGdnId'), InvGdnWagesId: cn(w, 'InvGdnWagesId'), ItemId: cn(w, 'ItemId'),
                    ItemName: cs(w, 'ItemName'), AttributeVarientId: cn(w, 'ItemAttributeVarientId'), AttributeVarient: cs(w, 'ItemAttributeVarient'), VarientEquivalent: cn(w, 'VarientEquivalent'),
                    ContractorId: cn(w, 'ContractorId'), ContractorName: cs(w, 'ContractorName'), ContractorWagesRateScheduleId: cn(w, 'ContractorWagesRateScheduleId'), ServiceActivity: cs(w, 'ServiceActivity'),
                    ParentUomId: cn(w, 'ParentUomId'), Qty: cn(w, 'Qty'), ItemNetWeight: cn(w, 'ItemNetWeight'), Rate: cn(w, 'Rate'), Amount: cn(w, 'Amount'), AddLess: cn(w, 'AddLess'),
                    NetAmount: cn(w, 'NetAmount'), Remarks: cs(w, 'Remarks') };
            })));
            lock('comsupplier', true);
            S.existing = d.attachments || []; S.files = []; S.removedAtt = [];
            show('btnSave', false); show('btnUpdate', true); show('btnDelete', true);
            recalc();                                                                       // the ReadById tail: proportions, GridItemNetAmount, BillAmount, EnableDiableCommissionFields, exchange
        }).catch(fail);
    }
    function del() {                                                                        // btnDelete_Click
        if (S.id <= 0) return msg('Record Not Found');
        if (S.approved) return msg('Record Not Delete beacause Record has approved');
        return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
            if (!yes) return;
            return SE.api(API + '/' + S.id, { method: 'DELETE' }).then(function (r) { return msg(r.message || 'Delete Voucher Successfully').then(reset); })
                .catch(function (e) { return msg(e.message); });
        });
    }

    /* ------------------------------------------------------------------ history */
    function tabHistory() {
        if (!iv(cb.dateType) && cb.dateType.rows().length) cb.dateType.setValue(cb.dateType.rows()[0].Id);
        cb.dateType.focus();
    }
    function dateTypeChanged() { SPC.setRangeFromDateType(iv(cb.dateType), $('txtFromdateHistory'), $('txtToDateHistory'), S.fyStart); }
    function histCombos() {
        return SE.api(API + '/history-combos').then(function (h) { S.hc = h; keep(cb.hCust, histCustRows()); keep(cb.hAgent, h.agents); }).catch(fail);
    }
    function showHistory() {
        var q = { fromDate: $('txtFromdateHistory').value, toDate: $('txtToDateHistory').value, fromNo: SE.toInt($('txtFromNoHistory').value), toNo: SE.toInt($('txtToDocNoHistory').value),
            customerId: iv(cb.hCust), agentId: iv(cb.hAgent) };
        return SE.api(API + '/history' + SE.q(q)).then(function (rows) { S.hist = rows; G.hist.setRows(rows); if (!rows.length) G.hd.setRows([]); }).catch(fail);
    }
    function resetHistory() {
        cb.dateType.setValue(3); dateTypeChanged();
        $('txtFromNoHistory').value = ''; $('txtToDocNoHistory').value = '';
        cb.hCust.clear(); cb.hAgent.clear(); cb.dateType.focus();
    }
    function histSelected(item) {                                                           // grdHistory_SelectionChanged -> GetDetailGrdByHeadId
        if (!item) { G.hd.setRows([]); return; }
        SE.api(API + '/' + item.Id, { quiet: true }).then(function (d) {
            G.hd.setRows((d.lines || []).map(function (l) {
                return { Id: cn(l, 'Id'), OrderNo: cs(l, 'SaleOrderNo'), GdnNo: cs(l, 'GdnNo'), Warehouse: cs(l, 'WareHouseName'), ItemId: cn(l, 'ItemId'), ItemName: cs(l, 'ItemName'), ItemCode: cs(l, 'ItemCode'),
                    AttributeVarient: cs(l, 'ItemAttributeVarient'), VarientUnit: cn(l, 'VarientEquivalent'), JobLot: cs(l, 'JobLotDescription'), ItemQty: cn(l, 'ItemQty'), ItemWeight: cn(l, 'ItemWeight'),
                    NetWeight: cn(l, 'ItemNetWeight'), ItemPrice: cn(l, 'ItemRateWithOutAddLess'), AddLessRate: cn(l, 'RateAddLess'), Rate: cn(l, 'ItemRate'), ItemAmount: cn(l, 'ItemAmount'),
                    DiscountType: cs(l, 'ItemDiscountType'), DiscRate: cn(l, 'ItemDiscountRate'), DiscAmount: cn(l, 'ItemDiscountAmount'), ItemAmountWithDisc: cn(l, 'ItemAmountWithDisc'),
                    FcyAmount: cn(l, 'FcyAmount'), GpDate: cs(l, 'GpDate'), GpNo: cs(l, 'GpNo'), VehicleNo: cs(l, 'VehicleNo'), CityName: cs(l, 'CityName'), Expense: cn(l, 'ExpenseAmount'),
                    Commission: cn(l, 'CommissionAmount'), ItemNetAmount: cn(l, 'ItemNetAmount'), IsFOC: cb2(l, 'IsFOC') };
            }));
        }).catch(function () { G.hd.setRows([]); });
    }

    /* ------------------------------------------------------------------ print / attachments */
    function printSlip(id, item) {                                                          // 1861-SaleInvoice_Slip (customer slip) / 1861A-SaleInvoice_Slip (item slip)
        if (!(id > 0)) return msg('No Record Found For Display');
        return SPC.openPdf((item ? IPRINT : PRINT) + SE.q({ id: id }));
    }
    function printVoucher(id) {                                                             // AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 1861)
        if (!(id > 0)) return msg('No Record Found For Display');
        return SPC.openPdf(VPRINT + SE.q({ id: id }));
    }
    function showAttachments(id) { return SE.api(API + '/' + id + '/attachments').then(function (rows) { SPC.attachments(S, API, id, true, rows); }).catch(fail); }
    function openAttachments() { SPC.attachments(S, API, S.id, false); }

    /* ------------------------------------------------------------------ shortcut keys */
    var SHORT = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
        ['Ctrl+L', 'For Load Gdn'], ['Ctrl+P', 'For Print'], ['Alt+1', 'For Print Slip 1861'], ['Alt+2', 'For Print Slip 1861A'], ['Alt+3', 'For Print Voucher 103'],
        ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+D', 'For Adding an row in Focused Grid'], ['Ctrl+Delete', 'For Deleting an row of Focused Grid'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'],
        ['Ctrl+ArrowRight', 'For Change Focus from one Grid To another Grid'], ["Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function onKey(e) {
        var t = e.target, k = e.key.toLowerCase();
        if (LD && LD.pop && LD.pop.el.classList.contains('on')) return;
        if (e.key === 'Enter' && t && (t.tagName === 'INPUT' || t.tagName === 'SELECT') && t.type !== 'checkbox' && t.type !== 'radio' && t.type !== 'button' && !t.classList.contains('cell')) {
            var f = Array.prototype.filter.call(document.querySelectorAll('.dform input:not([type=hidden]):not([disabled]), .dform select:not([disabled]), .dform .dtcombo-input'), function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(t); if (i >= 0 && f[i + 1]) { e.preventDefault(); f[i + 1].focus(); }
        }
        if (e.key === 'F1' && $('grdPartyAddLs').contains(document.activeElement)) {
            var cur = G.jv.cur(); if (cur && /^(AccountTitle|AccountId|GlAccountId)$/.test(G.jv.curKey() || '')) { e.preventDefault(); pickAccount(cur); }
        }
        if (e.ctrlKey && k === 't') { e.preventDefault(); if (tabs.index() === 1) { tabs.select('tabPage1'); cb.sup.focus(); } else { tabs.select('tabPage2'); tabHistory(); $('grdHistory').focus(); } return; }
        if (tabs.index() === 0) {
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh(); }
            else if (e.ctrlKey && k === 'l') { e.preventDefault(); btnLoadGdn(); }
            else if (e.ctrlKey && e.shiftKey && e.key === 'Delete') { e.preventDefault(); if (isShown('btnDelete') && !$('btnDelete').disabled) del(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); if (isShown('btnSave') && !$('btnSave').disabled) btnSave(); }
            else if (e.ctrlKey && k === 'u') { e.preventDefault(); if (isShown('btnUpdate') && !$('btnUpdate').disabled) btnUpdate(); }
            else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); $('DocDate').focus(); }
            else if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); openAttachments(); }
            else if (e.ctrlKey && k === 'p') { e.preventDefault(); printSlip(S.id, false); }
            else if (e.altKey && e.key === '1') { e.preventDefault(); printSlip(S.id, false); }
            else if (e.altKey && e.key === '2') { e.preventDefault(); printSlip(S.id, true); }
            else if (e.altKey && e.key === '3') { e.preventDefault(); printVoucher(S.id); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { $('grd').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowRight') {
                var order = ['grd', 'grdChargeToProduct', 'grdPartyAddLs', 'grdContractorWages'], tabOf = { grd: 'tabPage3', grdChargeToProduct: 'tabPage4', grdPartyAddLs: 'tabPage5', grdContractorWages: 'tabPage6' };
                var at = -1; order.forEach(function (id, n) { if ($(id).contains(document.activeElement)) at = n; });
                if (at >= 0) { var nx = order[(at + 1) % order.length]; tabs2.select(tabOf[nx]); $(nx).focus(); }
            }
            else if (e.ctrlKey && e.key === 'Delete') {
                if ($('grdChargeToProduct').contains(document.activeElement)) { var ei = G.exp.curIndex(); if (ei >= 0) expDelete(ei); }
                else if ($('grdPartyAddLs').contains(document.activeElement)) { var ji = G.jv.curIndex(); if (ji >= 0) jvDelete(ji); }
                else if ($('grdContractorWages').contains(document.activeElement)) { var wi = G.wg.curIndex(); if (wi >= 0) wgDelete(wi); }
            }
            else if (e.ctrlKey && k === 'd') {
                if ($('grdChargeToProduct').contains(document.activeElement)) { e.preventDefault(); expAdd(); }
                else if ($('grdPartyAddLs').contains(document.activeElement)) { e.preventDefault(); jvAdd(); }
            }
        } else {
            if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); $('grdHistory').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { $('grdDetailHistory').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowUp') { $('grdHistory').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowRight') { if (document.activeElement === $('grdDetailHistory')) $('grdHistory').focus(); else $('grdDetailHistory').focus(); }
        }
    }
    function wire() {
        $('btnNew').onclick = reset; $('btnRefresh').onclick = refresh; $('btnSave').onclick = btnSave; $('btnUpdate').onclick = btnUpdate; $('btnDelete').onclick = del;
        $('btnAttachment').onclick = openAttachments; $('btnPartySlip').onclick = function () { printSlip(S.id, false); }; $('btnItemPrint').onclick = function () { printSlip(S.id, true); };
        $('btnPrint').onclick = function () { printVoucher(S.id); }; $('btnLoadGdn').onclick = btnLoadGdn;
        $('btnshortcutkeys').onclick = function () { SE.shortcuts(SHORT); };
        $('btnDefinePlant').onclick = function () { window.open('/party-processing/reference-parties', '_blank'); };                // DefineReferenceParties
        $('BtnNewHistory').onclick = resetHistory; $('BtnRefreshHistory').onclick = histCombos; $('btnshow').onclick = showHistory;
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
        var hook = false;
        document.addEventListener('keydown', function (e) { if (e.ctrlKey && e.altKey && !hook) { hook = true; SE.shortcuts(SHORT); setTimeout(function () { hook = false; }, 400); } });
    }

    document.addEventListener('DOMContentLoaded', load);
})();
