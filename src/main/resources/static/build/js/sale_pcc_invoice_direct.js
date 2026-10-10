/*
 * Screen 546  SaleInvoiceDirectConcrete  (Architecture.WinApp.pcc.Sale.SaleInvoiceDirectConcrete, document type 1856)
 * Page script. Desktop methods are named in the comments (SaleInvoiceDirectConcrete.cs). Server: /sale/pcc/sale-invoice-direct/api
 * The detail rows come from the entry panel (manual), from "Load Order" (SaleOrderExist) or from "Load Gdn" (GdnExist); the grid cells that are editable depend on that mode.
 * The server repeats the proportions / net amounts / bill totals on save (SalePccInvoiceGdnCalc).
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/pcc/sale-invoice-direct/api', PRINT = '/sale/pcc/print/sale-invoice-direct-slip', IPRINT = '/sale/pcc/print/sale-invoice-direct-item-slip',
        VPRINT = '/sale/pcc/print/sale-invoice-direct-voucher';
    var F = { amtRound: 0, amt: 0, rateRound: 2, rate: 2, fcy: 0 };
    var S = { id: 0, approved: false, rights: {}, files: [], removedAtt: [], existing: [], voucherHeadId: 0, multi: false, subsidiary: false, partyByCode: false, itemByCode: false, def: {}, L: {}, hist: [],
        fyStart: '', dateTypes: [], accounts: null, supGl: 0, hc: { customers: [], agents: [] }, suppliers: [], commDebit: [], items: [], orderExist: false, gdnExist: false, credit: null, busy: false,
        remove: [], updateIdx: -1, packs: [], freightAcId: 0 };
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
        cb.wh = mk('comWarehouse', 'WareHouseName', 'WareHouse Name', { onSelect: stockLabel });
        cb.item = XCombo('comItem', { columns: [{ key: '_t', caption: 'Item Name' }], textKey: '_t', popupWidth: 420, onSelect: itemLeave });
        cb.pack = XCombo('CmbPackUom', { columns: [{ key: 'UOMCode', caption: 'PackUOM' }, { key: 'Equivalent', caption: 'Equivalent' }], textKey: 'UOMCode', popupWidth: 220, onSelect: packLeave });
        cb.varient = XCombo('CmbAttributeVarient', { columns: [{ key: 'ItemAttribute', caption: 'Varient' }], valueKey: 'ItemAttributeVarientId', textKey: 'ItemAttribute', onSelect: varientLeave });
        cb.disc = mk('CmbDiscountType', 'DiscountType', 'Discount Type', { onSelect: calcDisc });
        cb.job = mk('comjobLot', 'JobLotDescription', 'Job Lot', { onSelect: stockLabel });
        cb.vehicle = mk('CmbVehicleNo', 'VehicleNo', 'Vehicle No');
        cb.city = mk('txtCity', 'CityName', 'City Name');
        cb.wact = mk('CmbActivityforContractorUpdateInWagesGrid', 'ServiceActivity', 'Activity', { popupWidth: 260 });
        cb.wcon = mk('CmbContractorForUpdateInWagesGrid', 'CompanyName', 'Contractor', { popupWidth: 320 });
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
    function itemRows() {
        var byCode = $('rdbtnItemCode').checked;
        return S.items.map(function (r) { return Object.assign({}, r, { _t: byCode ? r.ItemCode : r.ItemName }); });
    }
    function histCustRows() {
        var byCode = $('RdCustomerHistorySearchByCode').checked;
        return S.hc.customers.map(function (r) { return Object.assign({}, r, { _t: byCode ? r.PartyCode : r.PartyName }); });
    }
    function commDebitRows() { return (S.commDebit || []).filter(function (r) { return +r.Id !== +S.supGl; }); }
    function fillLists(d) {                                                                 // btnRefresh_Click: CurrencyFill, BindVehicles, suppliercustomer, ... item, JobLotBind, BinCity, dtForGridComboFill, ConfigurationDefault
        var L = S.L;
        ['currencies', 'customers', 'visitedBy', 'terms', 'deliveryTerms', 'refSalesMan', 'commTypes', 'commUoms', 'commDebit', 'labourAccounts', 'warehouses', 'items', 'jobLots', 'discTypes',
            'cities', 'vehicles', 'otherItems', 'contractors', 'projects', 'branches'].forEach(function (k) { if (d[k]) L[k] = d[k]; });
        S.suppliers = L.customers || []; S.commDebit = L.commDebit || []; S.items = L.items || [];
        keep(cb.cur, L.currencies); keep(cb.sup, supRows()); keep(cb.agent, S.suppliers); keep(cb.visited, L.visitedBy); keep(cb.term, L.terms);
        keep(cb.salesman, L.refSalesMan); keep(cb.cdebit, commDebitRows()); keep(cb.settle, S.commDebit);
        keep(cb.trans, L.labourAccounts); keep(cb.owages, L.labourAccounts);
        keep(cb.wh, L.warehouses);
        var itemId = iv(cb.item); cb.item.setData(itemRows()); if (itemId > 0 && !cb.item.setValue(itemId)) cb.item.clear();
        keep(cb.job, L.jobLots); keep(cb.city, L.cities); keep(cb.vehicle, L.vehicles);
        keep(cb.wcon, L.contractors);
        if (!cb.dterm.rows().length) { cb.dterm.setData(L.deliveryTerms); cb.dterm.setValue(1); }
        if (!cb.ctype.rows().length) cb.ctype.setData(L.commTypes);
        if (!cb.cuom.rows().length) { cb.cuom.setData(L.commUoms); cb.cuom.setValue(1); }
        if (!cb.disc.rows().length) cb.disc.setData(L.discTypes);
        keep(cb.proj, L.projects); keep(cb.branch, L.branches);
        G.exp.refresh(); G.wg.refresh(); G.grd.refresh();
        fillWagesCombo();
    }
    function configDefault() {                                                              // ConfigurationDefault
        var d = S.def;
        if (+d.warehouseId > 0) cb.wh.setValue(d.warehouseId);
        if (+d.cityId > 0 && iv(cb.city) === 0) cb.city.setValue(d.cityId);
        if (+d.jobLotId > 0 && iv(cb.job) === 0) cb.job.setValue(d.jobLotId);
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
        $('txtBillAmountHeader').value = fa(bill); $('txtManualTotalBillAmount').value = fa(bill);
        $('txtInvoiceQty').value = f2(rnd(qty, 2)); $('txtInvoiceWeight').value = f2(rnd(wt, 2));
        var ex = N('txtExchangeRate');
        $('txtFcyAmount').value = ex > 0 && wd > 0 ? ff(wd / ex) : ff(0);
    }
    function recalc() {                                                                     // every grid / header change: the chain, the bill, the commission, the bill again (the commission reads the bill without commission)
        chain(); billAmount(); totalComm(); billAmount();
    }

    /* ------------------------------------------------------------------ grids */

    /* ------------------------------------------------------------------ entry panel calculations */
    function itemWithVariant() { var r = cb.item.row(); return !!r && (r.WithVariant === true || r.WithVariant === 1 || String(r.WithVariant).toLowerCase() === 'true'); }
    function packEq() { var r = cb.pack.row(); return r && iv(cb.pack) > 0 ? SE.toNum(r.Equivalent) : 0; }
    /* CalculateDetailAmount: an item without variants prices Qty x Rate and weighs Qty x PackUom equivalent; a variant item uses VarientUnit x Qty (x ItemWeight / x Rate) */
    function calcDetail() {
        var qty = N('txtQty'), rate = N('txtRate'), unit = N('txtVarientEquivalent'), w = N('txtItemWeight'), net, a;
        if (qty > 0 && iv(cb.item) > 0) {
            if (!itemWithVariant()) {
                unit = iv(cb.pack) <= 0 ? 0 : packEq();
                net = rnd(unit * qty, 2);
                a = amt(qty * rate);
            } else {
                net = rnd(unit * qty * w, 2);
                a = amt(unit * qty * rate);
            }
            $('txtNetWeight').value = net === 0 ? '' : f2(net);                              // ToString("##,#.##")
            $('txtAmount').value = fa(a); $('txtTotalAmount').value = fa(a);
        } else { $('txtAmount').value = '0'; $('txtTotalAmount').value = '0'; }
    }
    /* CalculateDiscountAndTotalAmount (ActiveControl.Tag: txtDiscountRate = DiscPercent, txtDiscountAmount = DiscAmount) */
    function calcDisc() {
        var type = cb.disc.text(), act = (document.activeElement && document.activeElement.id) || '';
        var pct = N('txtDiscountRate'), itemAmt = N('txtAmount'), disc = N('txtDiscountAmount');
        if (type === 'Percent') {
            if (act === 'txtDiscountRate') {
                if (pct > 0) { disc = amt(itemAmt * pct / 100); $('txtDiscountAmount').value = fa(disc); } else $('txtDiscountAmount').value = '0';
            } else if (act === 'txtDiscountAmount') {
                pct = itemAmt === 0 ? 0 : rnd(disc * 100 / itemAmt, F.rateRound);
                if (pct > 99) {
                    pct = 99; $('txtDiscountRate').value = SE.num(pct, 3, 0);
                    disc = amt(itemAmt * pct / 100); $('txtDiscountAmount').value = fa(disc);
                    $('txtTotalAmount').value = String(itemAmt - disc);
                    return;
                }
                $('txtDiscountRate').value = SE.num(pct, 3, 0);
            } else { disc = amt(itemAmt * pct / 100); $('txtDiscountAmount').value = fa(disc); }
        } else if (type === 'Flat') {
            if (act === 'txtDiscountRate') {
                pct = N('txtDiscountRate'); disc = pct; $('txtDiscountAmount').value = fa(pct);
            } else if (act === 'txtDiscountAmount') {
                disc = N('txtDiscountAmount'); $('txtDiscountRate').value = SE.num(disc, 3, 0);
            }
        }
        if (type === 'Percent' && N('txtDiscountRate') > 99) {
            msg('Discount Percentage Cannot greater than 99');
            $('txtDiscountRate').value = fa(99);
        }
        $('txtTotalAmount').value = String(itemAmt - disc);
    }
    /* NetRateCalculation (Tag NetRate = txtRate, AddLess = txtAddLess) */
    function netRate() {
        var price = N('txtItemPrice'), rate = N('txtRate');
        if (price > 0 || rate > 0) {
            var add = N('txtAddLess'), act = (document.activeElement && document.activeElement.id) || '';
            if (act === 'txtRate') $('txtAddLess').value = fr(rate - price);
            else if (act === 'txtAddLess') $('txtRate').value = String(+Number(price + add).toPrecision(15));
            else $('txtRate').value = SE.num(price + add, 4, 0);
        }
    }
    function onQty() { calcDetail(); calcDisc(); }
    function onRate() { netRate(); calcDetail(); calcDisc(); }

    /* comItem_Leave */
    function itemLeave() {
        var id = iv(cb.item), r = cb.item.row();
        bindPackUom(id);
        if (id > 0 && r) {
            $('txtItemWeight').value = r.ItemWeight == null ? '' : String(r.ItemWeight);
            var price = +r.ItemRate || 0;
            $('txtItemPrice').value = SE.num(price, 4, 0); $('txtRate').value = SE.num(price, 4, 0);
            bindVarient(id);
        } else $('txtItemWeight').value = '0';
        stockLabel();
    }
    /* bindPackUom: CommonServices.GetUomScheduleByItemId; the previous Pack Uom is kept when the new item has the same code */
    function bindPackUom(itemId) {
        var prev = cb.pack.text();
        if (!(itemId > 0)) { cb.pack.setData([]); cb.pack.clear(); return Promise.resolve(); }
        return SE.api(API + '/pack-uoms' + SE.q({ itemId: itemId })).then(function (rows) {
            S.packs = rows || [];
            cb.pack.setData(S.packs);
            var hit = null;
            S.packs.forEach(function (r) { if (prev && String(r.UOMCode) === prev) hit = r; });
            if (hit) cb.pack.setValue(hit.Id); else cb.pack.clear();
            calcDetail(); calcDisc();
        }).catch(fail);
    }
    function packLeave() { calcDetail(); calcDisc(); }                                      // CmbPackUom_Leave
    /* bindvarientunit */
    function bindVarient(itemId) {
        var prev = cb.varient.text();
        return SE.api(API + '/varients' + SE.q({ itemId: itemId })).then(function (rows) {
            cb.varient.setData(rows || []);
            var hit = null;
            (rows || []).forEach(function (r) { if (prev && String(r.ItemAttribute) === prev) hit = r; });
            if (hit) cb.varient.setValue(hit.ItemAttributeVarientId); else cb.varient.clear();
            $('txtVarientEquivalent').value = hit ? String(hit.VarientEquivalent == null ? 0 : hit.VarientEquivalent) : '';
            calcDetail(); calcDisc();
        }).catch(fail);
    }
    /* CmbAttributeVarient_Leave */
    function varientLeave() {
        var r = cb.varient.row();
        $('txtVarientEquivalent').value = r && iv(cb.varient) > 0 ? String(r.VarientEquivalent == null ? 0 : r.VarientEquivalent) : '0';
        calcDetail(); calcDisc();
    }
    /* AvailableStockGetByItem */
    function stockLabel() {
        var item = iv(cb.item);
        if (item <= 0) { $('lblBalance').textContent = '0'; return Promise.resolve(); }
        return SE.api(API + '/stock' + SE.q({ itemId: item, docDate: $('DocDate').value, warehouseId: iv(cb.wh), jobLotId: iv(cb.job), varientId: iv(cb.varient) }), { quiet: true }).then(function (r) {
            $('lblBalance').textContent = String(r.qty == null ? 0 : r.qty);
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ detail entry: validation / Add / Edit / Update / Cancel / Delete */
    function detailValid() {                                                                // FormValidationDetail
        function stop(m, f) { msg(m); if (f) f(); return false; }
        if (!cb.wh.row() || iv(cb.wh) === 0) return stop('Warehouse Field is Required', function () { cb.wh.focus(); });
        if (!cb.item.row() || iv(cb.item) === 0) return stop('Item Name Field is Required', function () { cb.item.focus(); });
        if (!cb.pack.row() || iv(cb.pack) === 0) return stop('Pack Uom field is required', function () { cb.pack.focus(); });
        if (itemWithVariant() && iv(cb.varient) === 0) return stop('Attribute Variant field is required', function () { cb.varient.focus(); });
        if (!itemWithVariant() && iv(cb.varient) > 0) cb.varient.clear();
        if (T('txtQty') === '' || T('txtQty') === '0') return stop('Qty Field is Required', function () { $('txtQty').focus(); });
        if (T('txtItemWeight') === '' || T('txtItemWeight') === '0') return stop('Item Weight Field is Required', function () { $('txtItemWeight').focus(); });
        if (T('txtRate') === '' || T('txtRate') === '0') return stop('Rate Field is Required', function () { $('txtRate').focus(); });
        if ($('txtAmount').value === '' || $('txtAmount').value === '0') { msg('Please Check Item Amount'); return false; }
        if ($('txtTotalAmount').value === '' || $('txtTotalAmount').value === '0') { msg('Please Check Total Amount'); return false; }
        if (!cb.job.row() || iv(cb.job) === 0) return stop('Job/Lot Field is Required', function () { cb.job.focus(); });
        if (!cb.city.row() || iv(cb.city) === 0) return stop('City Field is Required', function () { cb.city.focus(); });
        return true;
    }
    var DUP = 'Duplicate items are not added to the grid. An item with the same warehouse, job lot, and AttributeVarient already exists';
    function isDuplicate(skip, withFoc) {
        var rows = G.grd.rows();
        for (var i = 0; i < rows.length; i++) {
            var r = rows[i];
            if (skip !== undefined && i === skip) continue;
            if (iv(cb.wh) === +r.WarehouseId && iv(cb.item) === +r.ItemId && iv(cb.varient) === +r.AttributeVarientId && iv(cb.job) === +r.JobLotId && (!withFoc || $('chkisFreeOfCost').checked === !!r.IsFOC)) return true;
        }
        return false;
    }
    function entryRow() {
        var ir = cb.item.row() || {}, wv = itemWithVariant(), pk = cb.pack.row() || {};
        return { WarehouseId: iv(cb.wh), Warehouse: cb.wh.text(), ItemId: iv(cb.item), ItemName: ir.ItemName || '', ItemCode: ir.ItemCode || '', ScheduleId: ir.ScheduleId || 0,
            PackUomId: iv(cb.pack), PackUom: pk.UOMCode || '', PackUomEquivalent: SE.toNum(pk.Equivalent),
            AttributeVarientId: wv ? iv(cb.varient) : 0, AttributeVarient: wv ? cb.varient.text().trim() : '', VarientUnit: wv ? N('txtVarientEquivalent') : 0, JobLotId: iv(cb.job), JobLot: cb.job.text(),
            ItemQty: N('txtQty'), BalQty: N('txtQty'), ItemWeight: N('txtItemWeight'), NetWeight: N('txtNetWeight'), BalWeight: N('txtNetWeight'), ItemPrice: N('txtItemPrice'),
            AddLessRate: N('txtAddLess'), Rate: N('txtRate'), ItemAmount: N('txtAmount'), DiscountTypeId: iv(cb.disc), DiscountType: cb.disc.text(), DiscRate: N('txtDiscountRate'),
            DiscAmount: N('txtDiscountAmount'), ItemAmountWithDisc: N('txtTotalAmount'), FcyAmount: N('txtTotalAmount'), GpDate: $('txtgpdate').value || SE.today(), GpNo: SE.toInt($('txtgatepassno').value),
            VehicleNo: cb.vehicle.text().trim(), CityId: iv(cb.city), CityName: cb.city.text(), Expense: 0, SettleDisc: 0, Commission: 0, Freight: 0, Wages: 0, ItemNetAmount: 0,
            IsFOC: $('chkisFreeOfCost').checked, ItemWithVarient: wv };
    }
    function afterDetailChange() {                                                          // TotalCommissionAmount, proportions, txtExchangeRate_TextChanged, WagesItemFillFromDetail
        recalc(); wagesFromDetail();
    }
    function resetDetail() {                                                                // ResetDetail
        $('chkisFreeOfCost').checked = false;
        ['txtQty', 'txtItemWeight', 'txtNetWeight', 'txtItemPrice', 'txtAddLess', 'txtRate', 'txtAmount'].forEach(function (id) { $(id).value = ''; });
        if ($('ChkResetOnSave').checked) optionReset();
        cb.item.focus();
    }
    function optionReset() {                                                                // OptionResetFields
        cb.item.clear(); cb.job.clear(); cb.wh.clear(); $('txtgatepassno').value = ''; cb.vehicle.clear(); cb.city.clear(); cb.item.focus();
    }
    function btnAdd() {
        if (G.grd.count() > 0 && S.orderExist) { msg('You can not add manual Record beacause record against order exist in Grid'); return; }
        if (G.grd.count() > 0 && S.gdnExist) { msg('You can not add manual Record beacause record against Gdn exist in Grid'); return; }
        if (isDuplicate(undefined, true)) { msg(DUP); return; }
        if (!detailValid()) return;
        G.grd.addRow(Object.assign({ Id: 0, OrderId: 0, OrderDetailId: 0, OrderNo: 0, GdnId: 0, GdnDetailId: 0, GdnNo: 0 }, entryRow()));
        gridMode(); afterDetailChange(); resetDetail();
    }
    /* grd_DoubleClick (only for manual rows) */
    function editRow(i) {
        var r = G.grd.rows()[i]; if (!r || S.orderExist || S.gdnExist) return;
        S.updateIdx = i;
        cb.wh.setValue(+r.WarehouseId);
        cb.item.setValue(+r.ItemId);
        bindVarientKeep(+r.ItemId, +r.AttributeVarientId, r.VarientUnit);
        bindPackKeep(+r.ItemId, +r.PackUomId);
        cb.job.setValue(+r.JobLotId);
        $('txtQty').value = f2(+r.ItemQty || 0); $('txtItemWeight').value = f2(+r.ItemWeight || 0); $('txtNetWeight').value = f2(+r.NetWeight || 0);
        $('txtItemPrice').value = fr(+r.ItemPrice || 0); $('txtAddLess').value = fr(+r.AddLessRate || 0); $('txtRate').value = fr(+r.Rate || 0);
        $('txtAmount').value = fa(+r.ItemAmount || 0);
        cb.disc.setValue(+r.DiscountTypeId || 0);
        $('txtDiscountRate').value = fr(+r.DiscRate || 0); $('txtDiscountAmount').value = fa(+r.DiscAmount || 0); $('txtTotalAmount').value = fa(+r.ItemAmountWithDisc || 0);
        $('txtgpdate').value = parseDate(r.GpDate) || SE.today(); $('txtgatepassno').value = String(r.GpNo == null ? '' : r.GpNo);
        if (!cb.vehicle.setValue(vehicleId(r.VehicleNo))) cb.vehicle.clear();
        cb.city.setValue(+r.CityId || 0); $('chkisFreeOfCost').checked = !!r.IsFOC;
        show('btnAdd', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
        cb.item.focus(); stockLabel();
    }
    function vehicleId(no) {
        var hit = 0; (S.L.vehicles || []).forEach(function (v) { if (String(v.VehicleNo).trim() === String(no == null ? '' : no).trim()) hit = v.Id; });
        return hit;
    }
    function bindVarientKeep(itemId, varId, unit) {
        return SE.api(API + '/varients' + SE.q({ itemId: itemId })).then(function (rows) {
            cb.varient.setData(rows || []); if (varId > 0) cb.varient.setValue(varId); else cb.varient.clear();
            $('txtVarientEquivalent').value = String(unit == null ? '' : unit);
        }).catch(fail);
    }
    function bindPackKeep(itemId, packId) {
        return SE.api(API + '/pack-uoms' + SE.q({ itemId: itemId })).then(function (rows) {
            S.packs = rows || []; cb.pack.setData(S.packs); if (packId > 0) cb.pack.setValue(packId); else cb.pack.clear();
        }).catch(fail);
    }
    function btnUpdateDetail() {
        if (!detailValid()) return;
        if (G.grd.count() > 0 && S.orderExist) { msg('You can not Update manual Record beacause record against order exist in Grid'); return; }
        if (G.grd.count() > 0 && S.gdnExist) { msg('You can not Update manual Record beacause record against Gdn exist in Grid'); return; }
        if (isDuplicate(S.updateIdx, false)) { msg(DUP); return; }
        var r = G.grd.rows()[S.updateIdx]; if (!r) return;
        var n = entryRow();
        ['WarehouseId', 'Warehouse', 'ItemId', 'ItemName', 'ItemCode', 'ScheduleId', 'PackUomId', 'PackUom', 'PackUomEquivalent', 'AttributeVarientId', 'AttributeVarient', 'VarientUnit', 'JobLotId', 'JobLot',
            'ItemQty', 'ItemWeight', 'NetWeight', 'ItemPrice', 'AddLessRate', 'Rate', 'ItemAmount', 'DiscountTypeId', 'DiscountType', 'DiscRate', 'DiscAmount', 'ItemAmountWithDisc', 'GpDate', 'GpNo',
            'VehicleNo', 'CityId', 'CityName', 'IsFOC', 'ItemWithVarient'].forEach(function (k) { r[k] = n[k]; });
        show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        S.updateIdx = -1;
        G.grd.refresh(); resetDetail(); afterDetailChange();
    }
    function btnCancelUpdateDetail() {                                                      // btnCancelUpdateDetial_Click
        show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false); S.updateIdx = -1;
    }
    /* grd_ColumnButtonClick "Delete" (also Ctrl+Delete when no Gdn is loaded) */
    function deleteRow(i) {
        var r = G.grd.rows()[i]; if (!r) return;
        function finish() { G.grd.removeAt(i); if (!G.grd.count()) { S.orderExist = false; S.gdnExist = false; unlockHeader(); } gridMode(); afterDetailChange(); }
        if ((+r.Id || 0) > 0) {
            return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
                if (!yes) return;
                S.remove.push({ id: +r.Id });                                               // the server re-reads the stored row (ActionTypeId 3)
                finish();
            });
        }
        finish();
    }
    function addRowAgain(i) {                                                               // grd_ColumnButtonClick "Add" (order mode): the row is copied with Id = 0
        var r = G.grd.rows()[i]; if (!r) return;
        G.grd.addRow(Object.assign({}, r, { Id: 0 }));
        afterDetailChange();
    }

    /* ------------------------------------------------------------------ Update Same Discount / manual bill total */
    function updateSameDiscount() {                                                         // btnUpdateDiscountInGrid_Click
        var id = iv(cb.disc), rate = N('txtDiscountRate');
        if (id === 0) { msg('DiscountType Required'); cb.disc.focus(); return; }
        if (rate === 0) { msg('Discount Rate Required'); $('txtDiscountRate').focus(); return; }
        if (G.grd.count() <= 0) return;
        if (id === 2 && rate >= 100) { msg('Discount Rate Cannot be greater than 99 when Discount Type is Percent'); return; }
        var nm = cb.disc.text();
        G.grd.rows().forEach(function (r) { r.DiscountTypeId = id; r.DiscountType = nm; r.DiscRate = rate; discRecalc(r); });
        G.grd.refresh(); afterDetailChange();
    }
    function discRecalc(r) {                                                                // the discount block of grd_CellUpdated / UpdateDiscountTypeAndRateInGrid
        var amtNo = +r.ItemAmount || 0, rate = +r.DiscRate || 0, da = 0, t = +r.DiscountTypeId || 0;
        if (t !== 0) {
            if (t === 1) da = rate; else if (t === 2) da = amtNo * rate / 100;
            r.DiscAmount = da;
        } else r.DiscAmount = 0;
        if (amtNo > 0 && da > 0) r.ItemAmountWithDisc = amtNo - da; else if (amtNo > 0) r.ItemAmountWithDisc = amtNo; else r.ItemAmountWithDisc = 0;
    }
    function manualTotal(fromInput) {                                                       // txtManualTotalBillAmount_TextChanged / _Leave (CalculateWithManualOrderAmountTotal)
        var manual = N('txtManualTotalBillAmount');
        if (fromInput && !(manual > 0)) return;
        var rows = G.grd.rows(); if (!rows.length) return;
        var gia = 0; rows.forEach(function (r) { gia += +r.ItemAmount || 0; });
        $('txtItemAmountHeader').value = fa(gia);
        var j = jvSums(), actual = gia + N('txtOtherWagesHeader') + N('txtFreightAmountHeader') + (j.c - j.d);
        $('txtBillamountWithoutCommission').value = fa(actual);
        if (iv(cb.sup) === iv(cb.agent)) actual -= N('CommNetAmtHeader');
        actual -= N('txtSettlementDiscountHeader');
        if (manual === 0) {
            $('txtDiscountHeader').value = fa(0); $('txtBillAmountHeader').value = fa(actual); $('txtManualTotalBillAmount').value = fa(actual);
        } else if (manual > actual) {
            msg('Manual Bill Amount ' + manual + ' Can\'t be Greater Than Expected Bill Amount ' + actual);
            $('txtDiscountHeader').value = fa(0); $('txtBillAmountHeader').value = fa(actual); $('txtManualTotalBillAmount').value = fa(actual);
        } else {
            $('txtDiscountHeader').value = fa(actual - manual); $('txtBillAmountHeader').value = fa(manual);
        }
        $('txtBillamountWithoutCommission').value = fa(N('txtBillamountWithoutCommission') - N('txtDiscountHeader'));
        proportionate();
        var net = 0; G.grd.rows().forEach(function (r) { net += +r.ItemNetAmount || 0; });
        $('txtItemNetAmountHeader').value = fa(net);
    }
    function rowNet(r) {                                                                    // CalculateRowNetAmount (the same rule as the chain)
        var wd = rnd(+r.ItemAmountWithDisc || 0, F.amtRound), sd = rnd(+r.SettleDisc || 0, F.amtRound), e = rnd(+r.Expense || 0, F.amtRound), setAcc = iv(cb.settle) > 0;
        if (S.credit) r.ItemNetAmount = setAcc ? wd : rnd(wd - sd, F.amtRound);
        else r.ItemNetAmount = setAcc ? rnd(wd + e, F.amtRound) : rnd(wd + e - sd, F.amtRound);
    }
    function proportionate() {                                                              // PropotionateDiscountByManualOrderAmountInGrid
        var rows = G.grd.rows(); if (!rows.length) return;
        var foot = N('txtDiscountHeader'), total = 0, done = 0;
        rows.forEach(function (r) { total += +r.ItemAmount || 0; });
        rows.forEach(function (r) {
            var ra0 = +r.ItemAmount || 0, rd = (total > 0 ? (ra0 / total * 100) : 0) * foot / 100, dr = 0;
            r.DiscAmount = rnd(rd, 2); r.ItemAmountWithDisc = ra0 - rd;
            if ((+r.DiscountTypeId || 0) === 0) { r.DiscountTypeId = 2; r.DiscountType = 'Percent'; }
            if (+r.DiscountTypeId === 1) { dr = rd; r.DiscRate = rnd(dr, 2); }
            else if (+r.DiscountTypeId === 2) { if (foot > 0 && rd > 0) { dr = ra0 > 0 ? rd / ra0 * 100 : 0; r.DiscRate = rnd(dr, 2); } else { r.DiscRate = 0; } }
            else r.DiscRate = 0;
            if (ra0 > 0 && rd > 0) r.ItemAmountWithDisc = ra0 - rd; else if (ra0 > 0) r.ItemAmountWithDisc = ra0; else r.ItemAmountWithDisc = 0;
            rowNet(r);
            done += rd;
        });
        if (Math.abs(done - foot) > 0.01 && done !== 0) {
            var f = foot / done;
            rows.forEach(function (r) {
                var ra0 = +r.ItemAmount || 0, ud = rnd((+r.DiscAmount || 0) * f, 2), dr = 0;
                r.DiscAmount = ud; r.ItemAmountWithDisc = ra0 - ud;
                if (+r.DiscountTypeId === 1) { dr = ud; r.DiscRate = rnd(dr, 2); }
                else if (+r.DiscountTypeId === 2) { if (ra0 > 0 && ud > 0) { dr = ud / ra0 * 100; r.DiscRate = rnd(dr, 2); } else r.DiscRate = 0; }
                else r.DiscRate = 0;
                r.ItemAmountWithDisc = (ra0 > 0 && ud > 0) ? ra0 - ud : (ra0 > 0 ? ra0 : 0);
                rowNet(r);
            });
        }
        G.grd.refresh();
        var ex = N('txtExchangeRate');
        G.grd.rows().forEach(function (r) { r.FcyAmount = ex > 0 ? (+r.ItemAmountWithDisc || 0) / ex : 0; });
        G.grd.refresh();
    }



    /* ------------------------------------------------------------------ grids */
    function expBlank() { return { Id: 0, ItemId: 0, ItemName: '', Qty: 0, Rate: 0, Amount: 0, Remarks: '' }; }
    function jvBlank() { return { AccountId: 0, GlAccountId: 0, AccountTitle: '', Remarks: '', Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0 }; }
    function wgBlank() {                                                                    // AddRowInContractorWagesGrid
        return { Id: 0, GdnId: 0, GdnWagesId: 0, OrderId: 0, OrderWagesId: 0, ItemId: 0, ItemName: '', AttributeVarientId: 0, AttributeVarient: '', VarientEquivalent: 0, ContractorId: 0,
            ContractorName: '', ContractorWagesRateScheduleId: 0, ServiceActivity: '', ParentUomId: 0, Qty: 0, ItemNetWeight: 0, Rate: 0, Amount: 0, AddLess: 0, NetAmount: 0, Remarks: '' };
    }
    function makeGrids() {
        /* grd (grdSettings): the editable cells depend on SaleOrderExist / GdnExist (gridMode) */
        G.grd = SE.grid('grd', { footer: true, dec: 0, onDbl: function (r, i) { editRow(i); },
            onBtn: function (k, r, i) {
                if (k === 'Delete') deleteRow(i); else if (k === 'Edit') editRow(i); else if (k === 'Add') addRowAgain(i);
            },
            onEdit: function (r, k, v) { grdEdit(r, k, v); },
            cols: [
                { k: 'Delete', t: 'X', w: 24, btn: 'X' }, { k: 'Edit', t: 'Edit', w: 50, btn: 'Edit' }, { k: 'Add', t: '+', w: 24, btn: '+', hide: true },
                { k: 'Id', hide: true }, { k: 'GdnId', hide: true }, { k: 'GdnDetailId', hide: true }, { k: 'GdnNo', t: 'GdnNo', w: 70, hide: true },
                { k: 'OrderId', hide: true }, { k: 'OrderDetailId', hide: true }, { k: 'OrderNo', t: 'OrderNo', w: 70, hide: true },
                { k: 'WarehouseId', t: 'Warehouse', w: 130, list: function () { return S.L.warehouses; }, lk: 'Id', lt: 'WareHouseName' }, { k: 'Warehouse', hide: true },
                { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'ScheduleId', hide: true },
                { k: 'PackUomId', hide: true }, { k: 'PackUom', t: 'PackUom', w: 80 }, { k: 'PackUomEquivalent', hide: true },
                { k: 'AttributeVarientId', hide: true }, { k: 'AttributeVarient', t: 'AttributeVarient', w: 100 },
                { k: 'VarientUnit', t: 'VarientUnit', w: 80, render: d3, sum: true, cls: 'num' },
                { k: 'JobLotId', t: 'JobLot', w: 120, list: function () { return S.L.jobLots; }, lk: 'Id', lt: 'JobLotDescription' }, { k: 'JobLot', hide: true },
                { k: 'ItemQty', t: 'ItemQty', w: 80, render: d3, sum: true, cls: 'num' }, { k: 'BalQty', hide: true },
                { k: 'ItemWeight', t: 'ItemWeight', w: 80, render: d3, sum: true, cls: 'num' }, { k: 'NetWeight', t: 'NetWeight', w: 90, render: d3, sum: true, cls: 'num' }, { k: 'BalWeight', hide: true },
                { k: 'ItemPrice', t: 'ItemPrice', w: 80, render: rr, cls: 'num' }, { k: 'AddLessRate', t: 'AddLessRate', w: 85, render: rr, cls: 'num' }, { k: 'Rate', t: 'Rate', w: 80, render: rr, cls: 'num' },
                { k: 'ItemAmount', t: 'ItemAmount', w: 95, render: ra, sum: true, cls: 'num' },
                { k: 'DiscountTypeId', t: 'DiscountType', w: 90, edit: true, list: function () { return S.L.discTypes; }, lk: 'Id', lt: 'DiscountType' }, { k: 'DiscountType', hide: true },
                { k: 'DiscRate', t: 'DiscRate', w: 70, render: ra, cls: 'num' }, { k: 'DiscAmount', t: 'DiscAmount', w: 85, render: ra, sum: true, cls: 'num' },
                { k: 'ItemAmountWithDisc', t: 'ItemAmountWithDisc', w: 110, render: ra, sum: true, cls: 'num' }, { k: 'FcyAmount', t: 'FcyAmount', w: 90, render: rf, sum: true, cls: 'num', hide: true },
                { k: 'GpDate', t: 'GpDate', w: 85, f: 'sdate' }, { k: 'GpNo', t: 'GpNo', w: 60 }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 },
                { k: 'CityId', t: 'CityName', w: 110, list: function () { return S.L.cities; }, lk: 'Id', lt: 'CityName' }, { k: 'CityName', hide: true },
                { k: 'Expense', t: 'Expense', w: 80, render: ra, sum: true, cls: 'num' }, { k: 'SettleDisc', t: 'SettleDisc', w: 80, render: ra, sum: true, cls: 'num' },
                { k: 'Commission', hide: true }, { k: 'Freight', hide: true }, { k: 'Wages', hide: true },
                { k: 'ItemNetAmount', t: 'ItemNetAmount', w: 100, render: ra, sum: true, cls: 'num' },
                { k: 'IsFOC', t: 'IsFOC', w: 50, f: 'chk', render: function (v) { return '<input type="checkbox" data-foc' + (v ? ' checked' : '') + '>'; } }, { k: 'ItemWithVarient', hide: true }] });
        $('grd').addEventListener('click', function (e) {                                  // grd_Click: IsFOC is a check-box column (EditType 8 only when SaleOrderExist)
            if (e.target && e.target.hasAttribute && e.target.hasAttribute('data-foc')) {
                var tr = e.target.closest('tr[data-i]'), r = tr && G.grd.rows()[+tr.getAttribute('data-i')];
                if (!r) return;
                if (!S.orderExist) { e.target.checked = !!r.IsFOC; return; }
                r.IsFOC = e.target.checked; G.grd.refresh(); recalc();
            }
        });
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
            onBtn: function (k, r, i) { if (k === 'Delete') wgDelete(i); else if (k === 'DeleteAll') wgDeleteAll(r); else if (k === 'Add') wgAdd(); },
            onEdit: function (r, k, v) { wgCell(r, k, v); },
            onDbl: function () { wgPick(); },
            cols: [{ k: 'Delete', t: 'X', w: 30, btn: 'X' }, { k: 'DeleteAll', t: 'DeleteAll', w: 70, btn: 'DeleteAll' }, { k: 'Add', t: '+', w: 30, btn: '+' },
                { k: 'Id', hide: true }, { k: 'GdnId', hide: true }, { k: 'GdnWagesId', hide: true }, { k: 'OrderId', hide: true }, { k: 'OrderWagesId', hide: true }, { k: 'ItemId', hide: true },
                { k: 'ServiceActivity', t: 'ServiceActivity', w: 150 }, { k: 'ItemName', t: 'ItemName', w: 180 }, { k: 'AttributeVarientId', hide: true }, { k: 'AttributeVarient', t: 'Varient', w: 90 },
                { k: 'VarientEquivalent', t: 'Unit', w: 60, render: d3, cls: 'num' }, { k: 'ContractorId', hide: true }, { k: 'ContractorName', t: 'ContractorName', w: 150 },
                { k: 'ContractorWagesRateScheduleId', hide: true }, { k: 'ParentUomId', hide: true },
                { k: 'Qty', t: 'Qty', w: 80, edit: true, render: d3, sum: true, cls: 'num' }, { k: 'ItemNetWeight', t: 'Weight', w: 80, render: d3, sum: true, cls: 'num' },
                { k: 'Rate', t: 'Rate', w: 80, render: rr, cls: 'num' }, { k: 'Amount', t: 'Amount', w: 95, render: ra, sum: true, cls: 'num' },
                { k: 'AddLess', t: 'AddLess', w: 80, edit: true, render: ra, sum: true, cls: 'num' }, { k: 'NetAmount', t: 'NetAmount', w: 95, render: ra, sum: true, cls: 'num' },
                { k: 'Remarks', t: 'Remarks', w: 260 }] });
        $('grdContractorWages').addEventListener('dblclick', function () { wgPick(); });

        /* History (HistoryGridSettings / DetailGridSetting) */
        G.hist = SE.grid('grdHistory', { frozen: 4, dec: 0, onDbl: function (r) { getUpdate(+r.Id, false); },
            onBtn: function (k, r) {
                if (k === 'Edit') getUpdate(+r.Id, true); else if (k === 'Print') printSlip(+r.Id, false); else if (k === 'Voucher') printVoucher(+r.Id); else if (k === 'ItemSlip') printSlip(+r.Id, true);
            },
            onLink: function (k, r) { if (k === 'NoOfAttachments') showAttachments(+r.Id); },
            onSel: histSelected,
            cols: [{ k: 'Edit', t: 'Edit', w: 50, btn: 'Edit' }, { k: 'Voucher', t: 'Voucher', w: 70, btn: 'Voucher' }, { k: 'Print', t: 'Customer Slip', w: 110, btn: 'Customer Slip' }, { k: 'ItemSlip', t: 'Item Slip', w: 90, btn: 'Item Slip' },
                { k: 'Id', hide: true }, { k: 'DocumentTypeId', hide: true }, { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 100, link: true }, { k: 'DocNo', t: 'DocNo', w: 70 },
                { k: 'DocDate', t: 'DocDate', w: 90, f: 'sdate' }, { k: 'SupplierCustomerId', hide: true }, { k: 'CustomerName', t: 'PartyName', w: 180 }, { k: 'PartyCode', t: 'PartyCode', w: 90 },
                { k: 'RefrenenceNo', t: 'RefrenenceNo', w: 100 }, { k: 'Distance', t: 'Distance', w: 70 }, { k: 'ManualBillNo', t: 'ManualBillNo', w: 100 }, { k: 'VisitedByName', t: 'VisitedByName', w: 110 },
                { k: 'ReferencPartyName', t: 'ReferencPartyName', w: 120 }, { k: 'ReferencPartyAddress', t: 'ReferencPartyAddress', w: 150 }, { k: 'ReferencPartyCellNo', t: 'ReferencPartyCellNo', w: 110 },
                { k: 'CommissionAgent', t: 'CommissionAgent', w: 110 }, { k: 'RefSalesMan', t: 'RefSalesMan', w: 100 }, { k: 'CommissionAmount', t: 'CommissionAmount', w: 100, render: ra, sum: true, cls: 'num' },
                { k: 'PaymentTerm', t: 'PaymentTerm', w: 100 }, { k: 'DueDays', t: 'DueDays', w: 60 }, { k: 'DeliveryTerm', t: 'DeliveryTerm', w: 100 }, { k: 'DeliveryStartDate', t: 'DeliveryStartDate', w: 100, f: 'sdate' },
                { k: 'IsApproved', t: 'IsApproved', w: 80, f: 'chk' }, { k: 'EntryDate', t: 'EntryDate', w: 130, f: 'dt12' }, { k: 'EntryUserName', t: 'EntryUserName', w: 110 },
                { k: 'ModifyDate', t: 'ModifyDate', w: 130, f: 'dt12' }, { k: 'ModifyUserName', t: 'ModifyUserName', w: 110 }, { k: 'ApprovedDate', t: 'ApprovedDate', w: 130, f: 'dt12' },
                { k: 'ApprovedUserName', t: 'ApprovedUserName', w: 110 }, { k: 'InvoiceQty', t: 'InvoiceQty', w: 80, render: d3, sum: true, cls: 'num' },
                { k: 'InvoiceWeight', t: 'InvoiceWeight', w: 90, render: d3, sum: true, cls: 'num' }, { k: 'TransporterName', t: 'TransporterName', w: 110 },
                { k: 'FrieghtAmountHeader', t: 'FrieghtAmountHeader', w: 110, render: ra, sum: true, cls: 'num' }, { k: 'OtherWagesAccount', t: 'OtherWagesAccount', w: 110 },
                { k: 'OtherWagesHeader', t: 'OtherWagesHeader', w: 110, render: ra, sum: true, cls: 'num' }, { k: 'BillAmount', t: 'BillAmount', w: 100, render: ra, sum: true, cls: 'num' }] });
        G.hd = SE.grid('grdDetailHistory', { dec: 0, cols: [{ k: 'Id', hide: true }, { k: 'GdnId', hide: true }, { k: 'GdnNo', t: 'GdnNo', w: 70 }, { k: 'OrderId', hide: true }, { k: 'OrderNo', t: 'OrderNo', w: 70 },
            { k: 'Warehouse', t: 'Warehouse', w: 120 }, { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 190 }, { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'AttributeVarient', t: 'AttributeVarient', w: 100 },
            { k: 'VarientUnit', t: 'VarientUnit', w: 80, render: d3, sum: true, cls: 'num' }, { k: 'JobLot', t: 'JobLot', w: 110 }, { k: 'ItemQty', t: 'ItemQty', w: 80, render: d3, sum: true, cls: 'num' },
            { k: 'ItemWeight', t: 'ItemWeight', w: 80, render: d3, sum: true, cls: 'num' }, { k: 'NetWeight', t: 'NetWeight', w: 90, render: d3, sum: true, cls: 'num' },
            { k: 'ItemPrice', t: 'ItemPrice', w: 80, render: rr, cls: 'num' }, { k: 'AddLessRate', t: 'AddLessRate', w: 80, render: rr, cls: 'num' }, { k: 'Rate', t: 'NetRate', w: 80, render: rr, cls: 'num' },
            { k: 'ItemAmount', t: 'ItemAmount', w: 95, render: ra, sum: true, cls: 'num' }, { k: 'DiscountType', t: 'DiscountType', w: 90 }, { k: 'DiscRate', t: 'DiscRate', w: 70, render: ra, cls: 'num' },
            { k: 'DiscAmount', t: 'DiscAmount', w: 85, render: ra, sum: true, cls: 'num' }, { k: 'ItemAmountWithDisc', t: 'ItemAmountWithDisc', w: 110, render: ra, sum: true, cls: 'num' },
            { k: 'FcyAmount', t: 'FcyAmount', w: 90, render: rf, sum: true, cls: 'num', hide: true }, { k: 'GpDate', t: 'GpDate', w: 85, f: 'sdate' }, { k: 'GpNo', t: 'GpNo', w: 60 }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 },
            { k: 'CityName', t: 'CityName', w: 100 }, { k: 'Expense', t: 'Expense', w: 80, render: ra, sum: true, cls: 'num' }, { k: 'Journal', t: 'Journal', w: 80, render: ra, sum: true, cls: 'num' },
            { k: 'Commission', t: 'Commission', w: 80, render: ra, sum: true, cls: 'num' }, { k: 'Freight', t: 'Freight', w: 80, render: ra, sum: true, cls: 'num' }, { k: 'Wages', t: 'Wages', w: 80, render: ra, sum: true, cls: 'num' },
            { k: 'ItemNetAmount', t: 'ItemNetAmount', w: 100, render: ra, sum: true, cls: 'num' }, { k: 'IsFOC', t: 'IsFOC', w: 50, f: 'chk' }] });
        G.focHd = SPC.focRows($('grdDetailHistory'), G.hd);
        wgReset(); gridMode();
    }
    function setPlaces() { [G.grd, G.exp, G.jv, G.wg, G.hist, G.hd].forEach(function (g) { if (g) { g.spec.dec = F.amt; g.refresh(); } }); }

    /* grdSettings: Delete / Add / Edit buttons and the editable cells follow SaleOrderExist / GdnExist */
    function gridMode() {
        var m = S.orderExist ? 'order' : S.gdnExist ? 'gdn' : 'manual', cols = G.grd.spec.cols;
        function set(k, p, v) { cols.forEach(function (c) { if (c.k === k) { if (v === undefined) delete c[p]; else c[p] = v; } }); }
        set('Delete', 'hide', m === 'gdn'); set('Add', 'hide', m !== 'order'); set('Edit', 'hide', m !== 'manual');
        set('OrderNo', 'hide', !S.orderExist); set('GdnNo', 'hide', !S.gdnExist);
        set('ItemQty', 'edit', m === 'order'); set('GpDate', 'edit', m === 'order'); set('GpNo', 'edit', m === 'order');
        set('DiscRate', 'edit', m !== 'manual'); set('DiscAmount', 'edit', m !== 'manual'); set('AddLessRate', 'edit', m === 'gdn');
        set('WarehouseId', 'edit', m !== 'gdn'); set('JobLotId', 'edit', m !== 'gdn'); set('CityId', 'edit', m !== 'gdn'); set('DiscountTypeId', 'edit', true);
        set('VehicleNo', 'edit', m !== 'gdn');
        cols.forEach(function (c) {                                                          // VehicleNo: typed on an order invoice, F1 list otherwise
            if (c.k !== 'VehicleNo') return;
            if (m === 'manual') { c.list = function () { return S.L.vehicles; }; c.lk = 'VehicleNo'; c.lt = 'VehicleNo'; } else { delete c.list; delete c.lk; delete c.lt; }
        });
        G.grd.refresh();
    }
    function unlockHeader() { lock('comsupplier', false); lock('combcommAgent', false); }

    /* ---- grd_CellUpdated (+ the F1 pop-ups of grd_KeyDown) ---- */
    function qtyAmounts(r) {                                                                // the amount / discount block of the ItemQty and AddLessRate branches
        var qty = +r.ItemQty || 0, unit = +r.VarientUnit || 0, price = +r.ItemPrice || 0, add = +r.AddLessRate || 0, ia = 0, da = 0;
        if (qty > 0) { var nr = price + add; r.Rate = nr; ia = r.ItemWithVarient ? qty * unit * nr : qty * nr; }
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
    function grdEdit(r, k, v) {
        if (k === 'WarehouseId') { r.WarehouseId = +v || 0; r.Warehouse = nameIn(S.L.warehouses, r.WarehouseId, 'WareHouseName'); G.grd.refresh(); return; }
        if (k === 'JobLotId') { r.JobLotId = +v || 0; r.JobLot = nameIn(S.L.jobLots, r.JobLotId, 'JobLotDescription'); G.grd.refresh(); return; }
        if (k === 'CityId') { r.CityId = +v || 0; r.CityName = nameIn(S.L.cities, r.CityId, 'CityName'); G.grd.refresh(); return; }
        if (k === 'DiscountTypeId') {                                                       // grd_KeyDown F1 on DiscountType
            r.DiscountTypeId = +v || 0; r.DiscountType = nameIn(S.L.discTypes, r.DiscountTypeId, 'DiscountType');
            if (r.DiscountTypeId > 0) discRecalc(r); else { r.DiscAmount = 0; r.ItemAmountWithDisc = 0; }
            G.grd.refresh(); recalc(); wagesFromDetail(); return;
        }
        if (k === 'VehicleNo') { r.VehicleNo = v; if (!S.orderExist && !S.gdnExist) { G.grd.refresh(); return; } }
        if (!S.orderExist && !S.gdnExist) return;                                           // grd_CellUpdated: if (!SaleOrderExist && !GdnExist) return;
        if (k === 'GpDate') { var p = parseDate(v); if (p) r.GpDate = p; G.grd.refresh(); return; }
        if (k === 'GpNo') { r.GpNo = SE.toInt(v); G.grd.refresh(); return; }
        if (k === 'ItemQty') {
            var qty = SE.toNum(v); r.ItemQty = qty;
            var bal = +r.BalQty || 0, balWt = +r.BalWeight || 0, wt = +r.ItemWeight || 0, unit = +r.VarientUnit || 0, pack = +r.PackUomEquivalent || 0, netWt = 0;
            if (qty > bal) {
                msg('Item Qty Cannot be greater than Balance Qty ' + bal + ' Of this row');
                r.ItemQty = qty = bal; netWt = balWt; r.NetWeight = netWt;
            } else {
                if (pack > 0 && unit > 0) { netWt = unit * qty * pack * wt; r.NetWeight = netWt; }
                else if (pack > 0 && unit === 0) { netWt = qty * pack * wt; r.NetWeight = netWt; }
                else if (pack === 0 && unit > 0) { netWt = qty * unit * wt; r.NetWeight = netWt; }
                if (netWt > balWt) { netWt = balWt; r.NetWeight = netWt; }
            }
            qtyAmounts(r);
        } else if (k === 'AddLessRate') {
            r.AddLessRate = SE.toNum(v); qtyAmounts(r);
        } else if (k === 'DiscRate') {
            r.DiscRate = SE.toNum(v); discRecalc(r);
        } else if (k === 'DiscAmount') {
            r.DiscAmount = SE.toNum(v);                                                     // the desktop has no CellUpdated branch for DiscAmount
        } else return;
        G.grd.refresh(); recalc(); wagesFromDetail();
    }

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
    function wgReset() { G.wg.setRows([wgBlank()]); }
    function wgAdd() { G.wg.addRow(wgBlank()); }
    function wgAfter() { G.wg.refresh(); recalc(); fillWagesCombo(); }
    function wgAmount(r, p, rate, qty, weight, truncW) {                                    // ParentUomId 2: Rate * Qty; 3: Weight / 1000 * Rate; anything else 0
        if (p === 2) return rate * qty;
        if (p === 3) return (truncW ? Math.trunc(weight) : weight) / 1000 * rate;
        return 0;
    }
    function wgCell(r, k, v) {                                                              // grdContractorWages_CellUpdated
        if (v !== '' && isNaN(Number(String(v).replace(/,/g, '')))) { msg('Please Type Only Numeric Value', 'Warning!'); return; }
        if (k === 'Qty') {
            if (v === '' || r.Rate === '' || r.Rate == null) { msg('Qty And Rate Should be greater than zero...'); return; }
            r.Qty = SE.toNum(v);
            var found = null; S.items.forEach(function (x) { if (!found && +x.Id === +r.ItemId) found = x; });
            var weight = (found ? +found.ItemWeight || 0 : 0) * r.Qty * (+r.VarientEquivalent || 0);
            r.ItemNetWeight = weight;
            var rate = +r.Rate || 0, add = +r.AddLess || 0, p = +r.ParentUomId || 0;
            if (p === 2 || p === 3) { r.Amount = wgAmount(r, p, rate, r.Qty, weight, false); r.NetAmount = r.Amount + add; }
            else { r.Amount = 0; r.NetAmount = add; G.wg.refresh(); msg('ParentUom of RangeUom of Selected Wages Activity is not Defined...Please Check'); return; }
        } else if (k === 'AddLess') {
            r.AddLess = SE.toNum(v); r.NetAmount = (+r.Amount || 0) + r.AddLess;
        }
        G.wg.refresh(); recalc();
    }
    function wgEmptyCheck() { if (G.wg.count() === 0) wgReset(); }
    function wgDelete(i) { G.wg.removeAt(i); wgEmptyCheck(); wgAfter(); }
    function wgDeleteAll(r) {
        var act = String(r.ServiceActivity || '');
        SE.ask('Are you sure to Delete All Rows With Activity ' + act + '?', 'Confirm').then(function (yes) {
            if (!yes) return;
            G.wg.setRows(G.wg.rows().filter(function (x) { return String(x.ServiceActivity || '') !== act; }));
            wgEmptyCheck(); wgAfter();
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


    /* WagesItemFillFromDetail: dtItemForWages / dtVarientForWages (needs a current detail row, as the desktop) */
    function wagesFromDetail() {
        if (!G.grd.cur()) return;
        var items = [], vars = [], rows = G.grd.rows();
        rows.forEach(function (r) {
            var iid = +r.ItemId || 0, aid = +r.AttributeVarientId || 0;
            if (!items.some(function (x) { return x.Id === iid; })) items.push({ Id: iid, ItemName: String(r.ItemName || ''), ItemCode: String(r.ItemCode || '') });
            if (!vars.some(function (x) { return x.ItemAttributeId === aid && x.ItemId === iid; })) {
                var q = 0, w = 0;
                rows.forEach(function (x) { if ((+x.AttributeVarientId || 0) === aid && (+x.ItemId || 0) === iid) { q += +x.ItemQty || 0; w += +x.NetWeight || 0; } });
                vars.push({ ItemAttributeId: aid, ItemAttribute: String(r.AttributeVarient || ''), ItemId: iid, ItemName: String(r.ItemName || ''), VarientEquivalent: SE.toNum(r.VarientUnit), Qty: q, NetWeight: w });
            }
        });
        S.wItems = items; S.wVars = vars;
    }
    function fillWagesCombo() {                                                             // FillWagesGridFilterCombo
        var act = iv(cb.wact), txt = cb.wact.text(), rows = G.wg.rows();
        if (rows.length > 0) {
            var seen = {}, dist = [];
            rows.forEach(function (r) { var a = String(r.ServiceActivity || ''); if (!seen[a]) { seen[a] = 1; dist.push({ Id: +r.ContractorWagesRateScheduleId || 0, ServiceActivity: a }); } });
            cb.wcon.setData(S.L.contractors || []);
            cb.wact.setData(dist);
            var hit = dist.filter(function (x) { return x.ServiceActivity === txt; })[0];
            if (txt && hit) cb.wact.setValue(hit.Id); else cb.wact.clear();
        } else { cb.wcon.clear(); cb.wact.setData([]); cb.wact.clear(); }
    }
    function btnGenerateWages() {                                                           // BtnGenerateWagesRows_Click: WagesItemFillFromDetail, GenerateRowsInWagesGrid, FillWagesGridFilterCombo
        wagesFromDetail();
        if (G.grd.count() <= 0 || !(S.wVars || []).length) { fillWagesCombo(); return Promise.resolve(); }
        var date = $('DocDate').value, vars = S.wVars;
        return Promise.all(vars.map(function (d) { return SE.api(API + '/wages-rates' + SE.q({ itemId: d.ItemId, docDate: date, contractorId: 0 }), { quiet: true }); })).then(function (all) {
            var cur = G.wg.rows();
            if (cur.length === 1 && !(+cur[0].ItemId) && !(+cur[0].Amount) && !(+cur[0].ContractorWagesRateScheduleId) && !(+cur[0].AttributeVarientId)) cur = [];   // the blank first row stays on the desktop; it is kept below
            var rows = G.wg.rows().slice();
            vars.forEach(function (d, n) {
                (all[n] || []).forEach(function (r) {
                    var sid = cn(r, 'ContractorWagesRateScheduleId'), p = cn(r, 'ParentUomId'), rate = cn(r, 'Rate'), hit = null;
                    rows.forEach(function (x) { if (!hit && (+x.ItemId || 0) === d.ItemId && (+x.AttributeVarientId || 0) === d.ItemAttributeId && (+x.ContractorWagesRateScheduleId || 0) === sid) hit = x; });
                    var amount = wgAmount(null, p, rate, d.Qty, d.NetWeight, false);
                    if (hit) { hit.Qty = d.Qty; hit.ItemNetWeight = d.NetWeight; hit.Amount = amount; hit.NetAmount = amount + (+hit.AddLess || 0); }
                    else {
                        var nw = wgBlank();
                        nw.ItemId = d.ItemId; nw.ItemName = d.ItemName; nw.AttributeVarientId = d.ItemAttributeId; nw.AttributeVarient = d.ItemAttribute; nw.VarientEquivalent = d.VarientEquivalent;
                        nw.ContractorWagesRateScheduleId = sid; nw.ServiceActivity = cs(r, 'WagesActivity'); nw.ParentUomId = p; nw.Qty = d.Qty; nw.ItemNetWeight = d.NetWeight; nw.Rate = rate;
                        nw.Amount = amount; nw.AddLess = 0; nw.NetAmount = amount;
                        rows.push(nw);
                    }
                });
            });
            G.wg.setRows(wgSort(rows)); recalc(); fillWagesCombo();
        }).catch(fail);
    }
    function btnUpdateContractor() {                                                        // BtnUpdateConctratorInWages_Click
        var rows = G.wg.rows();
        if (!rows.length) return Promise.resolve();
        var con = iv(cb.wcon), conName = cb.wcon.text(), actId = iv(cb.wact), actName = cb.wact.text(), date = $('DocDate').value;
        var targets, cid, cname;
        if (con === 0 && actId === 0) {
            if (!(+rows[0].ContractorId)) return msg('ContractorName Field is Empty in Wages Grid Row No: 01', 'Error Message');
            targets = rows; cid = +rows[0].ContractorId; cname = String(rows[0].ContractorName || '');
        } else if (con !== 0 && actId === 0) { targets = rows; cid = con; cname = conName; }
        else if (con === 0 && actId !== 0) { cb.wcon.focus(); return msg('ContractorName Field is Empty', 'Error Message'); }
        else { targets = rows.filter(function (r) { return String(r.ServiceActivity || '') === actName; }); cid = con; cname = conName; }
        if (!targets.length) return msg('Wages Grid Record not Found', 'Error Message');
        return Promise.all(targets.map(function (r) { return SE.api(API + '/wages-rates' + SE.q({ itemId: +r.ItemId || 0, docDate: date, contractorId: cid }), { quiet: true }); })).then(function (all) {
            targets.forEach(function (r, n) {
                r.ContractorId = cid; r.ContractorName = cname;
                var add = Math.trunc(+r.AddLess || 0), list = all[n] || [];
                if (!list.length) return;
                var hit = null; list.forEach(function (x) { if (!hit && cs(x, 'WagesActivity') === String(r.ServiceActivity || '')) hit = x; });
                var rate = hit ? cn(hit, 'Rate') : 0, p = hit ? cn(hit, 'ParentUomId') : 0;
                r.ContractorWagesRateScheduleId = hit ? cn(hit, 'ContractorWagesRateScheduleId') : 0; r.Rate = rate; r.ParentUomId = p;
                if (p === 2 || p === 3) r.Amount = wgAmount(r, p, rate, +r.Qty || 0, +r.ItemNetWeight || 0, true);
                else { r.ParentUomId = 0; r.Rate = 0; r.Amount = 0; }
                r.NetAmount = r.Amount + add;
            });
            G.wg.setRows(wgSort(G.wg.rows())); recalc(); fillWagesCombo();
        }).catch(fail);
    }

    /* grdContractorWages_KeyDown F1: Contractor / Item / Varient / Activity pop-ups */
    function simplePick(title, list, idKey, nameKey, done) {
        var h = '<div style="padding:4px"><input id="spFind" class="f" style="width:100%;position:static" placeholder="Search"></div><div class="dgrid" style="max-height:50vh;overflow:auto"><table class="jg"><thead><tr><th>' +
            SE.esc(title) + '</th></tr></thead><tbody id="spBody"></tbody></table></div>';
        var pop = SE.pop(title, h, [{ t: 'Close' }]); pop.open();
        var body = pop.body.querySelector('#spBody'), find = pop.body.querySelector('#spFind'), view = [];
        function draw() {
            var q = find.value.toLowerCase();
            view = (list || []).filter(function (x) { return !q || String(x[nameKey]).toLowerCase().indexOf(q) >= 0; }).slice(0, 300);
            body.innerHTML = view.map(function (x, i) { return '<tr data-p="' + i + '" style="cursor:pointer"><td>' + SE.esc(x[nameKey]) + '</td></tr>'; }).join('');
        }
        draw(); find.addEventListener('input', draw); find.focus();
        body.addEventListener('click', function (e) {
            var tr = e.target.closest('tr[data-p]'); if (!tr) return;
            var x = view[+tr.getAttribute('data-p')]; pop.close(); done(+x[idKey] || 0, String(x[nameKey] == null ? '' : x[nameKey]));
        });
    }
    function wgPick() {
        var r = G.wg.cur(), k = G.wg.curKey();
        if (!r) return;
        var date = $('DocDate').value;
        if (k === 'ContractorId' || k === 'ContractorName') {
            simplePick('Contractor', S.L.contractors, 'Id', 'CompanyName', function (id, name) {
                r.ContractorId = id; r.ContractorName = name;
                SE.api(API + '/wages-rates' + SE.q({ itemId: +r.ItemId || 0, docDate: date, contractorId: id }), { quiet: true }).then(function (list) {
                    if (list && list.length) {
                        var hit = null; list.forEach(function (x) { if (!hit && cs(x, 'WagesActivity') === String(r.ServiceActivity || '')) hit = x; });
                        var rate = hit ? cn(hit, 'Rate') : 0, p = hit ? cn(hit, 'ParentUomId') : 0, add = Math.trunc(+r.AddLess || 0);
                        r.ContractorWagesRateScheduleId = hit ? cn(hit, 'ContractorWagesRateScheduleId') : 0; r.Rate = rate; r.ParentUomId = p;
                        if (p === 2 || p === 3) { r.Amount = wgAmount(r, p, rate, +r.Qty || 0, +r.ItemNetWeight || 0, true); r.NetAmount = r.Amount + add; }
                        else { r.ParentUomId = 0; r.Rate = 0; r.Amount = 0; r.NetAmount = add; G.wg.refresh(); msg('ParentUom of RangeUom of Selected Wages Activity is not Defined...Please Check'); return; }
                    }
                    wgAfter();
                }).catch(fail);
            });
        } else if (k === 'ItemId' || k === 'ItemName') {
            simplePick('Item', S.wItems || [], 'Id', 'ItemName', function (id, name) {
                var prev = +r.ItemId || 0;
                r.ItemId = id; r.ItemName = name;
                if (id !== prev) {
                    r.AttributeVarientId = 0; r.AttributeVarient = ''; r.VarientEquivalent = 0; r.Qty = 0; r.ItemNetWeight = 0; r.ContractorWagesRateScheduleId = 0; r.ServiceActivity = '';
                    r.ParentUomId = 0; r.Rate = 0; r.Amount = 0;
                }
                wgAfter();
            });
        } else if (k === 'AttributeVarientId' || k === 'AttributeVarient') {
            if ((+r.ItemId || 0) <= 0) { msg('Please Select an Item First'); return; }
            var vs = (S.wVars || []).filter(function (x) { return x.ItemId === +r.ItemId; });
            if (!vs.length) { msg('Please Select an Item First'); return; }
            simplePick('Varient', vs, 'ItemAttributeId', 'ItemAttribute', function (id, name) {
                r.AttributeVarientId = id; r.AttributeVarient = name;
                if (id > 0) {
                    var f = vs.filter(function (x) { return x.ItemAttributeId === id; })[0];
                    var q = f ? f.Qty : 0, w = f ? f.NetWeight : 0, add = Math.trunc(+r.AddLess || 0), rate = Math.trunc(+r.Rate || 0);
                    r.Qty = q; r.ItemNetWeight = w; r.VarientEquivalent = f ? f.VarientEquivalent : 0;
                    var p = +r.ParentUomId || 0;
                    if (p === 2) r.Amount = rate * q; else if (p === 3) r.Amount = w / 1000 * rate; else { r.Amount = 0; r.NetAmount = add; }
                } else { r.Qty = 0; r.ItemNetWeight = 0; r.Amount = 0; r.NetAmount = 0; }
                wgAfter();
            });
        } else if (k === 'ServiceActivity' || k === 'ContractorWagesRateScheduleId') {
            if ((+r.ItemId || 0) <= 0) { msg('Please Select an Item First'); return; }
            SE.api(API + '/wages-rates' + SE.q({ itemId: +r.ItemId, docDate: date, contractorId: +r.ContractorId || 0 }), { quiet: true }).then(function (list) {
                var rows = (list || []).map(function (x) { return { Id: cn(x, 'ContractorWagesRateScheduleId'), WagesActivity: cs(x, 'WagesActivity'), _x: x }; });
                simplePick('Activity', rows, 'Id', 'WagesActivity', function (id, name) {
                    r.ContractorWagesRateScheduleId = id; r.ServiceActivity = name;
                    var add = Math.trunc(+r.AddLess || 0);
                    if (id > 0) {
                        var f = rows.filter(function (x) { return x.Id === id; })[0], rate = f ? cn(f._x, 'Rate') : 0, p = f ? cn(f._x, 'ParentUomId') : 0;
                        r.Rate = rate; r.ParentUomId = p;
                        if (p === 2 || p === 3) { r.Amount = wgAmount(r, p, rate, +r.Qty || 0, +r.ItemNetWeight || 0, true); r.NetAmount = r.Amount + add; }
                        else { r.ContractorWagesRateScheduleId = 0; r.ServiceActivity = ''; r.ParentUomId = 0; r.Rate = 0; r.Amount = 0; r.NetAmount = add; G.wg.refresh(); msg('ParentUom of RangeUom of Selected Wages Activity is not Defined...Please Check'); return; }
                    } else { r.ContractorWagesRateScheduleId = 0; r.ServiceActivity = ''; r.ParentUomId = 0; r.Rate = 0; r.Amount = 0; r.NetAmount = add; }
                    wgAfter();
                });
            }).catch(fail);
        }
    }
    /* WagesValidationWithDetail */
    function wagesValid() {
        if (G.grd.count() <= 0 || G.wg.count() <= 0) return null;
        wagesFromDetail();
        if (!(S.wVars || []).length) return null;
        var bad = null;
        G.wg.rows().forEach(function (r) {
            var iid = +r.ItemId || 0;
            if (!bad && iid !== 0 && !G.grd.rows().some(function (x) { return (+x.ItemId || 0) === iid; })) bad = 'Item ' + String(r.ItemName || '') + ' in Contractor Wages Grid  does not exist in Detail Grid ';
        });
        return bad;
    }


    /* ------------------------------------------------------------------ Load Order (BtnLoadOrder_Click -> LoadSaleOrderConcrete) / Load Gdn (btnLoadGdn_Click -> LoadPendingGDNDirectConcrete) */
    var LD = null;
    function btnLoadOrder() {
        var cur = G.grd.cur();
        if (G.grd.count() > 0 && cur && (+cur.OrderId || 0) === 0) { msg('You cannot load Order because a record exists without Order in the grid'); return; }
        SE.api(API + '/loader/order-combos').then(function (c) { openLoader('order', c); }).catch(fail);
    }
    function btnLoadGdn() {
        var cur = G.grd.cur();
        if (G.grd.count() > 0 && cur && (+cur.GdnId || 0) === 0) { msg('You cannot load Gdn because a record exists without Gdn in the grid'); return; }
        SE.api(API + '/loader/combos').then(function (c) { openLoader('gdn', c); }).catch(fail);
    }
    function openLoader(kind, c) {
        var isO = kind === 'order', st = 'position:static;';
        var h = '<div style="padding:6px;display:flex;flex-wrap:wrap;gap:8px 14px;align-items:center;font-size:12px">' +
            '<label>From Date <input type="date" id="ldFrom" class="f" style="' + st + '"></label><label>To Date <input type="date" id="ldTo" class="f" style="' + st + '"></label>' +
            '<label>No From <input id="ldNoFrom" class="f" style="' + st + 'width:70px"></label><label>To <input id="ldNoTo" class="f" style="' + st + 'width:70px"></label>' +
            '<label>Parent Category <select id="ldParent"></select></label><label>Item Category <select id="ldCat"></select></label><label>Item Type <select id="ldType"></select></label>' +
            '<label>Item Name <select id="ldItem"></select></label><label>Customer Name <select id="ldCust"></select></label>' +
            '<button type="button" id="ldShow">Show</button><button type="button" id="ldLoad">Load</button><button type="button" id="ldReset">Reset</button></div>' +
            '<div class="dgrid" id="ldMain" style="height:' + (isO ? '200' : '230') + 'px"></div>' +
            (isO ? '<div id="ldTot" style="display:flex;flex-wrap:wrap;gap:6px 12px;padding:4px 6px;font-size:12px">' +
                ['OrderQty:Order Qty', 'OrderWeight:Order Weight', 'OrderAmount:Order Amount', 'DispatchQty:Dispatch Qty', 'DispatchWeight:Dispatch Weight', 'DispatchAmount:Dispatch Amount',
                    'BalanceQty:Balance Qty', 'BalanceWeight:Balance Weight', 'BalanceAmount:Balance Amount'].map(function (p) {
                    var a = p.split(':'); return '<label>' + a[1] + ' <input id="ldT' + a[0] + '" class="f" readonly style="' + st + 'width:90px;text-align:right" value="0"></label>';
                }).join('') + '</div>' : '') +
            '<div class="dgrid" id="ldDet" style="height:' + (isO ? '180' : '200') + 'px;margin-top:6px"></div>';
        var pop = SE.pop(isO ? 'Pending Sale Order For Invoice' : 'Pending Gdn For Invoice', h, [{ t: 'Close' }], { wide: true });
        pop.open();
        var q = function (id) { return pop.body.querySelector('#' + id); };
        LD = { pop: pop, all: [], c: {}, kind: kind };
        function lc(id, key, cap, rows, w) { return XCombo(id, { columns: [{ key: key, caption: cap }], textKey: key, popupWidth: w || 260 }).setData(rows || []); }
        LD.c.parent = lc(q('ldParent'), 'name', 'Parent Category', c.parentCategories); LD.c.cat = lc(q('ldCat'), 'name', 'Category', c.categories);
        LD.c.type = lc(q('ldType'), 'name', 'Item Type', c.itemTypes); LD.c.item = lc(q('ldItem'), 'name', 'Item', c.items, 340); LD.c.cust = lc(q('ldCust'), 'name', 'Customer', c.customers, 340);
        q('ldFrom').value = parseDate(S.fyStart) || SE.today(); q('ldTo').value = SE.today();
        SE.digitsOnly(q('ldNoFrom')); SE.digitsOnly(q('ldNoTo'));
        var hc = isO ? [
            { k: 'NoOfAttachments', t: 'Attached', w: 70 }, { k: 'DocNo', t: 'DocNo', w: 70 }, { k: 'DocDate', t: 'DocDate', w: 90, f: 'sdate' }, { k: 'CustomerName', t: 'CustomerName', w: 180 },
            { k: 'RefrenenceNo', t: 'RefrenenceNo', w: 100 }, { k: 'Distance', t: 'Distance', w: 70 }, { k: 'VisitedByName', t: 'VisitedByName', w: 110 }, { k: 'ReferencPartyName', t: 'ReferenceParty', w: 120 },
            { k: 'ReferencPartyCellNo', t: 'RefPartyCellNo', w: 100 }, { k: 'CommissionAgent', t: 'CommissionAgent', w: 110 }, { k: 'RefSalesMan', t: 'RefSalesMan', w: 100 },
            { k: 'CommissionAmount', t: 'CommAmount', w: 90, render: ra, cls: 'num' }, { k: 'TermsDescription', t: 'PaymentTerm', w: 100 }, { k: 'OrderDueDays', t: 'OrderDueDays', w: 80 },
            { k: 'OrderDueDate', t: 'OrderDueDate', w: 90, f: 'sdate' }, { k: 'DeliveryTerm', t: 'DeliveryTerm', w: 100 }, { k: 'DeliveryStartDate', t: 'DeliveryStartDate', w: 100, f: 'sdate' },
            { k: 'TransporterName', t: 'TransporterName', w: 110 }, { k: 'FrieghtAmountHeader', t: 'FrieghtAmountHeader', w: 100, render: ra, cls: 'num' }, { k: 'OtherWagesAccount', t: 'WagesAccount', w: 100 },
            { k: 'OtherWagesHeader', t: 'WagesAmountHeader', w: 100, render: ra, cls: 'num' }, { k: 'OrderStatus', t: 'Status', w: 80 }, { k: 'IsApproved', t: 'IsApproved', w: 80 },
            { k: 'EntryDate', t: 'EntryDate', w: 130, f: 'dt12' }, { k: 'EntryUserName', t: 'EntryUserName', w: 110 }, { k: 'ModifyDate', t: 'ModifyDate', w: 130, f: 'dt12' },
            { k: 'ModifyUserName', t: 'ModifyUserName', w: 110 }, { k: 'ApprovedDate', t: 'ApprovedDate', w: 130, f: 'dt12' }, { k: 'ApprovedUserName', t: 'ApprovedUserName', w: 110 },
            { k: 'RemarksHeader', t: 'RemarksHeader', w: 160 }] : [
            { k: 'Select', t: '', w: 40, sel: true }, { k: 'NoOfAttachments', t: 'Attached', w: 70 }, { k: 'DocNo', t: 'DocNo', w: 70 }, { k: 'DocDate', t: 'DocDate', w: 90, f: 'sdate' },
            { k: 'CustomerName', t: 'CustomerName', w: 180 }, { k: 'ReferenceNo', t: 'ReferenceNo', w: 100 }, { k: 'ReferencPartyName', t: 'ReferenceParty', w: 120 },
            { k: 'ReferencPartyCellNo', t: 'RefPartyCellNo', w: 100 }, { k: 'DeliveryTerm', t: 'DeliveryTerm', w: 100 }, { k: 'TransporterName', t: 'TransporterName', w: 110 },
            { k: 'FrieghtAmountHeader', t: 'FrieghtAmountHeader', w: 100, render: ra, cls: 'num' }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 }, { k: 'GpNo', t: 'GpNo', w: 60 },
            { k: 'GpDate', t: 'GpDate', w: 90, f: 'sdate' }, { k: 'IsApproved', t: 'IsApproved', w: 80 }, { k: 'EntryDate', t: 'EntryDate', w: 130, f: 'dt12' }, { k: 'EntryUserName', t: 'EntryUserName', w: 110 },
            { k: 'ModifyDate', t: 'ModifyDate', w: 130, f: 'dt12' }, { k: 'ModifyUserName', t: 'ModifyUserName', w: 110 }, { k: 'ApprovedDate', t: 'ApprovedDate', w: 130, f: 'dt12' },
            { k: 'ApprovedUserName', t: 'ApprovedUserName', w: 110 }, { k: 'RemarksHeader', t: 'RemarksHeader', w: 160 }];
        LD.main = SE.grid(q('ldMain'), { dec: 0, onSel: function (r) { loaderDetail(r); }, cols: [{ k: 'Id', hide: true }].concat(hc) });
        var dc = isO ? [
            { k: 'Select', t: '', w: 40, sel: true }, { k: 'ItemCode', t: 'ItemCode', w: 80 }, { k: 'ItemName', t: 'ItemName', w: 180 }, { k: 'ItemArrtibuteVarient', t: 'Varient', w: 90 },
            { k: 'VarientEquivalent', t: 'VarientUnit', w: 70, render: d3, cls: 'num' }, { k: 'JobLotDescription', t: 'JobLot', w: 100 }, { k: 'ItemQty', t: 'ItemQty', w: 70, render: d3, sum: true, cls: 'num' },
            { k: 'DispatchQty', t: 'DispatchQty', w: 80, render: d3, sum: true, cls: 'num' }, { k: 'BalanceQty', t: 'BalanceQty', w: 80, render: d3, sum: true, cls: 'num' },
            { k: 'ItemWeight', t: 'ItemWeight', w: 80, render: d3, cls: 'num' }, { k: 'NetWeight', t: 'NetWeight', w: 80, render: d3, sum: true, cls: 'num' },
            { k: 'DispatchWeight', t: 'DispatchWeight', w: 90, render: d3, sum: true, cls: 'num' }, { k: 'BalanceWeight', t: 'BalanceWeight', w: 90, render: d3, sum: true, cls: 'num' },
            { k: 'ItemRateWithOutAddLess', t: 'Item Price', w: 80, render: rr, cls: 'num' }, { k: 'RateAddLess', t: 'RateAddLess', w: 80, render: rr, cls: 'num' }, { k: 'ItemRate', t: 'Net Rate', w: 80, render: rr, cls: 'num' },
            { k: 'ItemAmount', t: 'ItemAmount', w: 90, render: ra, sum: true, cls: 'num' }, { k: 'ItemDiscountType', t: 'ItemDiscountType', w: 90 }, { k: 'ItemDiscountRate', t: 'DiscRate', w: 70, render: ra, cls: 'num' },
            { k: 'ItemDiscountAmount', t: 'ItemDiscountAmount', w: 100, render: ra, sum: true, cls: 'num' }, { k: 'ItemNetAmount', t: 'ItemNetAmount', w: 100, render: ra, sum: true, cls: 'num' },
            { k: 'DispatchAmount', t: 'DispatchAmount', w: 90, render: ra, sum: true, cls: 'num' }, { k: 'BalanceAmount', t: 'BalanceAmount', w: 90, render: ra, sum: true, cls: 'num' },
            { k: 'CityName', t: 'CityName', w: 90 }, { k: 'RemarksDetail', t: 'RemarksDetail', w: 140 }] : [
            { k: 'WareHouseName', t: 'WareHouseName', w: 110 }, { k: 'ItemCode', t: 'ItemCode', w: 80 }, { k: 'ItemName', t: 'ItemName', w: 180 }, { k: 'ScheduleId', t: 'ScheduleId', w: 70 },
            { k: 'PackUom', t: 'PackUom', w: 80 }, { k: 'PackEquivalent', t: 'PackEquivalent', w: 80, render: d3, cls: 'num' }, { k: 'ItemArrtibuteVarient', t: 'Varient', w: 90 },
            { k: 'VarientEquivalent', t: 'VarientUnit', w: 70, render: d3, cls: 'num' }, { k: 'JobLotDescription', t: 'JobLot', w: 100 }, { k: 'ItemQty', t: 'ItemQty', w: 70, render: d3, sum: true, cls: 'num' },
            { k: 'ItemWeight', t: 'ItemWeight', w: 80, render: d3, cls: 'num' }, { k: 'NetWeight', t: 'NetWeight', w: 80, render: d3, sum: true, cls: 'num' }, { k: 'CityName', t: 'CityName', w: 90 },
            { k: 'IsFOC', t: 'IsFOC', w: 50, f: 'chk' }, { k: 'RemarksDetail', t: 'RemarksDetail', w: 140 }];
        LD.det = SE.grid(q('ldDet'), { dec: 0, cols: dc });
        q('ldShow').onclick = ldShow; q('ldLoad').onclick = ldLoad; q('ldReset').onclick = ldReset;
        pop.el.addEventListener('keydown', function (e) { if (e.ctrlKey && e.key.toLowerCase() === 'l') { e.preventDefault(); ldLoad(); } });
    }
    function lq(id) { return LD.pop.body.querySelector('#' + id); }
    function ldTotals(r) {                                                                  // CalculateTotal
        ['OrderQty', 'OrderWeight', 'OrderAmount', 'DispatchQty', 'DispatchWeight', 'DispatchAmount', 'BalanceQty', 'BalanceWeight', 'BalanceAmount'].forEach(function (k) {
            var el = lq('ldT' + k); if (!el) return;
            var v = r ? cn(r, k + 'Header') : 0;
            el.value = /Amount$/.test(k) ? fa(v) : f2(v);
        });
    }
    function ldReset() {                                                                    // btnReset_Click
        lq('ldFrom').value = parseDate(S.fyStart) || SE.today(); lq('ldTo').value = SE.today(); lq('ldNoFrom').value = ''; lq('ldNoTo').value = '';
        ['parent', 'cat', 'type', 'item', 'cust'].forEach(function (k) { LD.c[k].clear(); });
        LD.all = []; LD.main.setRows([]); LD.det.setRows([]); ldTotals(null);
    }
    function ldShow() {                                                                     // PendingPurchaseOrderRegularForLoad / PendingGDNForInvoice: one header row per Id, all detail rows kept
        var p = { fromDate: lq('ldFrom').value, toDate: lq('ldTo').value, fromNo: SE.toInt(lq('ldNoFrom').value), toNo: SE.toInt(lq('ldNoTo').value),
            customerId: +LD.c.cust.value() || 0, categoryId: +LD.c.cat.value() || 0, itemId: +LD.c.item.value() || 0, typeId: +LD.c.type.value() || 0, parentId: +LD.c.parent.value() || 0 };
        return SE.api(API + (LD.kind === 'order' ? '/loader/orders' : '/loader/pending') + SE.q(p)).then(function (rows) {
            LD.all = rows || [];
            var seen = {}, heads = [];
            LD.all.forEach(function (r) { var id = cn(r, 'Id'); if (!seen[id]) { seen[id] = 1; heads.push(r); } });
            LD.main.setRows(heads); LD.det.setRows([]); ldTotals(null);
            if (heads.length) LD.main.select(0);
        }).catch(fail);
    }
    function loaderDetail(r) {                                                              // grdMain_SelectionChanged -> CalculateTotal, DetailGridBind, CheckedAllDetailRows
        if (!r) { LD.det.setRows([]); ldTotals(null); return; }
        var id = cn(r, 'Id'), isO = LD.kind === 'order';
        if (isO) ldTotals(r);
        LD.det.setRows(LD.all.filter(function (x) { return cn(x, 'Id') === id; }).map(function (x) { return isO ? Object.assign({ _chk: true }, x) : x; }));
    }
    function ldLoad() {                                                                     // btnLoadOnInvoice_Click_1 / btnLoad_Click
        if (LD.kind === 'order') {
            var chk = LD.det.checked();
            if (!chk.length) { msg('Check the Row first'); return; }
            var oid = 0;
            for (var i = 0; i < chk.length; i++) {
                var id = cn(chk[i], 'Id');
                if (oid === 0) oid = id;
                if (oid !== id) { msg('You Can Only Select Rows Of Same Order'); return; }
            }
            LD.pop.close();
            loadOrderRows(chk);
            return;
        }
        var checked = LD.main.checked();
        if (!checked.length) { msg('Check the row first', 'Error Message'); return; }
        var cust = 0, ids = [];
        for (var j = 0; j < checked.length; j++) {
            var cu = cn(checked[j], 'SupplierCustomerId');
            if (cust === 0) cust = cu;
            if (cust !== cu) { msg("Sorry! The selected Row's are not of the same Customer."); return; }
            ids.push(cn(checked[j], 'Id'));
        }
        var rows = LD.all.filter(function (x) { return ids.indexOf(cn(x, 'Id')) >= 0; });
        LD.pop.close();
        loadGdnRows(rows);
    }

    /* LoadInGridDetail (order) */
    function loadOrderRows(rows) {
        if (!rows.length) return Promise.resolve();
        var h = rows[0], orderId = 0;
        rows.forEach(function (r) { if (!orderId && cn(r, 'Id') > 0) orderId = cn(r, 'Id'); });
        cb.sup.setValue(cn(h, 'SupplierCustomerId')); supplierRefresh();
        $('txtDistance').value = cs(h, 'Distance'); $('txtSupplierReference').value = cs(h, 'RefrenenceNo');
        cb.visited.setValue(cn(h, 'VisitedById'));
        cb.term.setValue(cn(h, 'PaymentTermsId')); $('txtduedays').disabled = iv(cb.term) === 1;
        $('txtduedays').value = cs(h, 'OrderDueDays'); $('duedate').value = parseDate(cs(h, 'OrderDueDate')) || $('duedate').value;
        setByText(cb.dterm, cs(h, 'DeliveryTerm'));
        $('txtDeliveryStartDate').value = parseDate(cs(h, 'DeliveryStartDate')) || $('txtDeliveryStartDate').value; $('txtDeliverydays').value = cs(h, 'DeliveryDays');
        $('txtRefPartyName').value = cs(h, 'ReferencPartyName'); $('txtRefPartyAddress').value = cs(h, 'ReferencPartyAddress'); $('txtRefPartyCellNo').value = cs(h, 'ReferencPartyCellNo');
        $('txtremarks').value = cs(h, 'RemarksHeader');
        cb.agent.setValue(cn(h, 'CommissionAgentId')); cb.salesman.setValue(cn(h, 'RefSalesManId'));
        setByText(cb.ctype, cs(h, 'CommissionType')); $('txtcommrate').value = fr(cn(h, 'CommissionRate')); $('txtcommamount').value = fa(cn(h, 'CommissionAmount'));
        cb.cur.setValue(cn(h, 'CurrencyIdHeader')); $('txtExchangeRate').value = String(cn(h, 'ExchangeRateHeader'));
        cb.owages.setValue(cn(h, 'OtherWagesAccountId')); $('txtOtherWagesHeader').value = fa(cn(h, 'OtherWagesHeader'));
        $('txtInvoiceQty').value = f2(cn(h, 'OrderQtyHeader')); $('txtInvoiceWeight').value = f2(cn(h, 'OrderWeightHeader'));
        cb.trans.setValue(cn(h, 'TransporterId')); $('txtFreightAmountHeader').value = fa(cn(h, 'FrieghtAmountHeader'));
        $('txtSettlementDiscountHeader').value = fa(cn(h, 'AdjustDiscountAmount'));
        lock('comsupplier', true);
        S.orderExist = true;                                                                // LoadDataDetailfromPurchaseInvoivce: SaleOrderExist = true before the checks
        var stop = false, have = {};
        G.grd.rows().forEach(function (x) { have[x.OrderDetailId] = 1; });
        rows.forEach(function (r) {
            if (stop) return;
            var did = cn(r, 'DetailId'), oid = cn(r, 'Id');
            if (G.grd.rows().some(function (x) { return (+x.OrderId || 0) !== oid; })) { stop = true; msg('Data against another OrderNo Already Exist in Detail'); return; }
            if (have[did]) return;
            have[did] = 1;
            G.grd.addRow({ Id: 0, GdnId: 0, GdnDetailId: 0, GdnNo: 0, OrderId: oid, OrderDetailId: did, OrderNo: cs(r, 'DocNo'), WarehouseId: 0, Warehouse: '', ItemId: cn(r, 'OrderItemId'),
                ItemName: cs(r, 'ItemName'), ItemCode: cs(r, 'ItemCode'), ScheduleId: cn(r, 'ScheduleId'), PackUomId: 0, PackUom: '', PackUomEquivalent: 0,
                AttributeVarientId: cn(r, 'ItemAttributeVarientId'), AttributeVarient: cs(r, 'ItemArrtibuteVarient'), VarientUnit: cn(r, 'VarientEquivalent'), JobLotId: cn(r, 'JobLotId'),
                JobLot: cs(r, 'JobLotDescription'), ItemQty: cn(r, 'BalanceQty'), BalQty: cn(r, 'BalanceQty'), ItemWeight: cn(r, 'ItemWeight'), NetWeight: cn(r, 'BalanceWeight'),
                BalWeight: cn(r, 'BalanceWeight'), ItemPrice: cn(r, 'ItemRateWithOutAddLess'), AddLessRate: cn(r, 'RateAddLess'), Rate: cn(r, 'ItemRate'), ItemAmount: cn(r, 'ItemAmount'),
                DiscountTypeId: cn(r, 'ItemDiscountTypeId'), DiscountType: cs(r, 'ItemDiscountType'), DiscRate: cn(r, 'ItemDiscountRate'), DiscAmount: cn(r, 'ItemDiscountAmount'),
                ItemAmountWithDisc: cn(r, 'BalanceAmount'), FcyAmount: cn(r, 'BalanceAmount'), GpDate: SE.today(), GpNo: 0, VehicleNo: '', CityId: cn(r, 'CityId'), CityName: cs(r, 'CityName'),
                Expense: 0, SettleDisc: 0, Commission: 0, Freight: 0, Wages: 0, ItemNetAmount: 0, IsFOC: false, ItemWithVarient: cb2(r, 'ItemWithVarient') });
        });
        gridMode();
        return SE.api(API + '/loader/order-wages' + SE.q({ ids: String(orderId) })).then(function (w) { loadExpData(w || [], true); recalc(); wagesFromDetail(); }).catch(fail);
    }

    /* LoadInGridDetailFromGdn */
    function loadGdnRows(rows) {
        if (!rows.length) return Promise.resolve();
        var first = 0, refs = [];
        rows.forEach(function (r) { if (!first && cn(r, 'Id') > 0) first = cn(r, 'Id'); });
        lock('comsupplier', true);
        S.gdnExist = true;
        var have = {};
        G.grd.rows().forEach(function (x) { have[x.GdnDetailId] = 1; });
        var existing = G.grd.rows().length > 0, err = null;
        rows.forEach(function (r) {
            if (err) return;
            var rn = cs(r, 'ReferenceNo');
            if (rn !== '' && refs.join(',').indexOf(rn) < 0) refs.push(rn);
            if (existing && iv(cb.sup) > 0 && iv(cb.sup) !== cn(r, 'SupplierCustomerId')) { err = 'Data against another Customer Already Exist'; return; }
            var did = cn(r, 'DetailId');
            if (have[did]) return;
            have[did] = 1;
            G.grd.addRow({ Id: 0, GdnId: cn(r, 'Id'), GdnDetailId: did, GdnNo: cs(r, 'DocNo'), OrderId: 0, OrderDetailId: 0, OrderNo: 0, WarehouseId: cn(r, 'WarehouseId'), Warehouse: cs(r, 'WareHouseName'),
                ItemId: cn(r, 'ItemId'), ItemName: cs(r, 'ItemName'), ItemCode: cs(r, 'ItemCode'), ScheduleId: cn(r, 'ScheduleId'), PackUomId: cn(r, 'ItemUomId'), PackUom: cs(r, 'PackUom'),
                PackUomEquivalent: cn(r, 'PackEquivalent'), AttributeVarientId: cn(r, 'ItemAttributeVarientId'), AttributeVarient: cs(r, 'ItemArrtibuteVarient'), VarientUnit: cn(r, 'VarientEquivalent'),
                JobLotId: cn(r, 'JobLotId'), JobLot: cs(r, 'JobLotDescription'), ItemQty: cn(r, 'ItemQty'), BalQty: cn(r, 'ItemQty'), ItemWeight: cn(r, 'ItemWeight'), NetWeight: cn(r, 'NetWeight'),
                BalWeight: cn(r, 'NetWeight'), ItemPrice: 0, AddLessRate: 0, Rate: 0, ItemAmount: 0, DiscountTypeId: 0, DiscountType: '', DiscRate: 0, DiscAmount: 0, ItemAmountWithDisc: 0, FcyAmount: 0,
                GpDate: parseDate(cs(r, 'GpDate')), GpNo: cs(r, 'GpNo'), VehicleNo: cs(r, 'VehicleNo'), CityId: cn(r, 'CityId'), CityName: cs(r, 'CityName'),
                Expense: 0, SettleDisc: 0, Commission: 0, Freight: 0, Wages: 0, ItemNetAmount: 0, IsFOC: cb2(r, 'IsFOC'), ItemWithVarient: cb2(r, 'WithVariant') });
        });
        gridMode();
        if (err) return msg(err, 'Error Message');
        var h = rows[0];
        return priceFromSchedule().then(function () {
            cb.sup.setValue(cn(h, 'SupplierCustomerId')); supplierRefresh();
            $('txtDistance').value = cs(h, 'Distance');
            var seen = {}, dist = refs.filter(function (x) { if (!x || seen[x]) return false; seen[x] = 1; return true; });
            $('txtSupplierReference').value = dist.join(',');
            setByText(cb.dterm, cs(h, 'DeliveryTerm'));
            $('txtRefPartyName').value = cs(h, 'ReferencPartyName'); $('txtRefPartyAddress').value = cs(h, 'ReferencPartyAddress'); $('txtRefPartyCellNo').value = cs(h, 'ReferencPartyCellNo');
            $('txtremarks').value = cs(h, 'RemarksHeader');
            cb.trans.setValue(cn(h, 'TransporterId')); $('txtFreightAmountHeader').value = fa(cn(h, 'FrieghtAmountHeader'));
            return SE.api(API + '/loader/wages' + SE.q({ ids: String(first) }));
        }).then(function (w) { loadExpData(w || [], false); recalc(); wagesFromDetail(); }).catch(fail);
    }
    function priceFromSchedule() {                                                          // GridItemPriceUpdateFromSchedule: item() with the DocDate rates, then every row
        return SE.api(API + '/items' + SE.q({ docDate: $('DocDate').value })).then(function (items) {
            S.items = items || [];
            var id = iv(cb.item); cb.item.setData(itemRows()); if (id > 0 && !cb.item.setValue(id)) cb.item.clear();
            if (!S.items.length || G.grd.count() <= 0) return;
            G.grd.rows().forEach(function (r) {
                var f = null; S.items.forEach(function (x) { if (!f && +x.Id === +r.ItemId) f = x; });
                if (!f) return;
                r.ItemPrice = cn(f, 'ItemRate'); r.Rate = r.ItemPrice + (+r.AddLessRate || 0);
                qtyAmounts(r);
            });
            G.grd.refresh();
        });
    }
    function loadExpData(items, fromOrder) {                                                // LoadExpData / LoadExpDataFromGdn
        if (!items.length) return;
        var rows = G.wg.rows();
        if (rows.length && !(+rows[0].ItemId) && !(+rows[0].Amount)) rows = [];
        items.forEach(function (r) {
            var dup = rows.some(function (x) { return fromOrder ? (+x.OrderWagesId === cn(r, 'Id')) : (+x.GdnWagesId === cn(r, 'Id')); });
            if (dup) return;
            var w = wgBlank();
            w.ItemId = cn(r, 'ItemId'); w.ItemName = cs(r, 'ItemName'); w.ContractorId = cn(r, 'ContractorId'); w.ContractorName = cs(r, 'ContractorName');
            w.ContractorWagesRateScheduleId = cn(r, 'ContractorWagesRateScheduleId'); w.ServiceActivity = cs(r, 'ServiceActivity'); w.Rate = cn(r, 'Rate'); w.Amount = cn(r, 'Amount'); w.Remarks = cs(r, 'Remarks');
            if (fromOrder) { w.OrderId = cn(r, 'SaleOrderId'); w.OrderWagesId = cn(r, 'Id'); w.Qty = cn(r, 'Qty'); }
            else {
                w.GdnId = cn(r, 'InvGdnId'); w.GdnWagesId = cn(r, 'Id'); w.AttributeVarientId = cn(r, 'ItemAttributeVarientId'); w.AttributeVarient = cs(r, 'ItemAttributeVarient');
                w.VarientEquivalent = cn(r, 'VarientEquivalent'); w.ParentUomId = cn(r, 'ParentUomId'); w.Qty = cn(r, 'Qty'); w.ItemNetWeight = cn(r, 'ItemNetWeight');
                w.AddLess = cn(r, 'AddLess'); w.NetAmount = cn(r, 'NetAmount');
            }
            rows.push(w);
        });
        if (!rows.length) rows = [wgBlank()];
        G.wg.setRows(wgSort(rows));
    }


    /* ------------------------------------------------------------------ Reset / Refresh / Load */
    function reset() {                                                                      // btnNew_Click -> Reset (the reference party, visited by, terms and doc date are not cleared, as on the desktop)
        S.files = []; S.removedAtt = []; S.existing = []; S.id = 0; S.voucherHeadId = 0; S.approved = false; S.orderExist = false; S.gdnExist = false; S.remove = []; S.updateIdx = -1;
        unlockHeader();
        cb.sup.clear(); cb.agent.clear(); cb.ctype.clear(); cb.cuom.clear(); cb.cdebit.clear(); cb.trans.clear(); cb.owages.clear(); cb.settle.clear(); cb.item.clear();
        $('chkisFreeOfCost').checked = false;
        ['txtDistance', 'txtSupplierReference', 'txtbillno', 'txtInvoiceQty', 'txtInvoiceWeight', 'txtExchangeRate', 'txtCommissionRemarks', 'txtcommrate', 'txtcommamount', 'txtremarks',
            'txtItemAmountHeader', 'txtDiscountHeader', 'txtItemNetAmountHeader', 'txtBillamountWithoutCommission', 'txtBillAmountHeader', 'txtFreightAmountHeader', 'txtOtherWagesHeader',
            'txtSettlementDiscountHeader', 'txtQty', 'txtItemPrice', 'txtAddLess', 'txtRate', 'txtAmount', 'txtManualTotalBillAmount'].forEach(function (id) { $(id).value = ''; });
        $('txtduedays').value = '0'; dueGen();
        G.grd.setRows([]); wgReset(); expReset(); jvReset(); gridMode();
        if ($('ChkResetOnSave').checked) optionReset();
        show('btnSave', true); show('btnUpdate', false); show('btnDelete', false);
        show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        configDefault();
        fillWagesCombo();
        $('DocDate').focus();
        return SE.api(API + '/next-no').then(function (r) { $('txtdocno').value = r.nextNo > 0 ? String(r.nextNo) : $('txtdocno').value; }).catch(fail);
    }
    function refresh() { return SE.api(API + '/lists' + SE.q({ docDate: $('DocDate').value })).then(function (d) { fillLists(d); S.accounts = null; configDefault(); fillWagesCombo(); }).catch(fail); }
    function applyInitial(d) {
        S.rights = d.rights || {}; S.fyStart = d.fyStart || ''; S.dateTypes = d.dateTypes || [];
        var s = d.settings || {};
        S.partyByCode = !!s.partyByCode; S.itemByCode = !!s.itemByCode; S.multi = !!s.multiCurrency; S.subsidiary = !!s.subsidiary; S.freightAcId = +s.freightAcId || 0;
        S.credit = s.creditInSaleGl ? true : null;                                          // 546: ToBool(CreditAmountInItemSaleGL); absent / false takes the Expense branches
        S.def = d.defaults || {};
        var f = d.fmt || {};
        F.amtRound = f.amountRound == null ? 0 : f.amountRound; F.amt = f.amount == null ? 0 : f.amount; F.rateRound = f.rateRound == null ? 2 : f.rateRound;
        F.rate = f.rate == null ? 2 : f.rate; F.fcy = f.fcy == null ? 0 : f.fcy;
        setPlaces();
        $('txtdocno').value = d.nextNo > 0 ? String(d.nextNo) : $('txtdocno').value;
        if (S.partyByCode) { $('RdPartyByCode').checked = true; $('RdCustomerHistorySearchByCode').checked = true; }
        else { $('RdPartyByName').checked = true; $('RdCustomerHistorySearchByName').checked = true; }
        if (S.itemByCode) $('rdbtnItemCode').checked = true; else $('rdbtnItemName').checked = true;
        cb.dateType.setData(S.dateTypes);
        S.hc = d.history || S.hc;
        keep(cb.hCust, histCustRows()); keep(cb.hAgent, S.hc.agents);
    }
    function load() {
        makeCombos();
        tabs = SE.tabs('tabControl1', function (id) { if (id === 'tabPage2') tabHistory(); else $('DocDate').focus(); });
        tabs2 = SE.tabs('tabControl2', function () { });
        makeGrids();
        ['txtExchangeRate', 'txtcommrate', 'txtFreightAmountHeader', 'txtOtherWagesHeader', 'txtSettlementDiscountHeader', 'txtDeliverydays', 'txtDistance', 'txtQty', 'txtItemWeight', 'txtNetWeight',
            'txtItemPrice', 'txtRate', 'txtDiscountRate', 'txtDiscountAmount', 'txtManualTotalBillAmount'].forEach(function (id) {
            var e = $(id); if (e) e.addEventListener('keypress', function (ev) { if (ev.key.length === 1 && !/[\d.]/.test(ev.key) && !ev.ctrlKey) ev.preventDefault(); });      // OnlytextdecimelFunction
        });
        $('txtAddLess').addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[\d.\-]/.test(e.key) && !e.ctrlKey) e.preventDefault(); });
        SE.digitsOnly($('txtduedays')); SE.digitsOnly($('txtgatepassno')); SE.digitsOnly($('txtFromNoHistory')); SE.digitsOnly($('txtToDocNoHistory'));
        $('txtQty').addEventListener('input', onQty);
        $('txtRate').addEventListener('input', onRate);
        $('txtItemPrice').addEventListener('input', onRate);
        $('txtAddLess').addEventListener('input', onRate);
        $('txtDiscountRate').addEventListener('input', calcDisc); $('txtDiscountAmount').addEventListener('input', calcDisc);
        $('txtduedays').addEventListener('input', dueGen);
        $('DocDate').addEventListener('change', function () { dueGen(); updateWagesRates(); });
        $('txtcommrate').addEventListener('input', commChanged);
        ['txtFreightAmountHeader', 'txtOtherWagesHeader', 'txtSettlementDiscountHeader', 'txtExchangeRate'].forEach(function (id) { $(id).addEventListener('input', recalc); });
        $('txtExchangeRate').addEventListener('change', function () { $('txtExchangeRate').value = fr(N('txtExchangeRate')); });
        $('txtManualTotalBillAmount').addEventListener('input', function () { manualTotal(true); });
        $('txtManualTotalBillAmount').addEventListener('change', function () { manualTotal(false); });
        $('RdPartyByName').addEventListener('change', partyRdb); $('RdPartyByCode').addEventListener('change', partyRdb);
        $('rdbtnItemName').addEventListener('change', itemRdb); $('rdbtnItemCode').addEventListener('change', itemRdb);
        $('RdCustomerHistorySearchByName').addEventListener('change', histRdb); $('RdCustomerHistorySearchByCode').addEventListener('change', histRdb);
        Promise.all([SE.api(API + '/initial'), SE.api(API + '/lists' + SE.q({ docDate: SE.today() }))]).then(function (a) {
            applyInitial(a[0]); fillLists(a[1]);
            var r = S.rights;
            $('btnSave').disabled = !r.save; $('btnUpdate').disabled = !r.update; $('btnDelete').disabled = !r.delete;
            ['btnCustomerSlip', 'btn1856APrint', 'btnPrint', 'ChkCustomerPrintslip', 'ChkBok'].forEach(function (id) { $(id).disabled = !r.print; });
            $('ChkCustomerPrintslip').checked = !!r.print; $('ChkBok').checked = !!r.print;
            multiCurrency();
            show('btnSave', true); show('btnUpdate', false); show('btnDelete', false); show('CommNetAmtHeader', false); show('label84', false);
            show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
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
    function itemRdb() {
        if (!S.items.length) return;
        var id = iv(cb.item);
        cb.item.setData(itemRows()); if (id > 0) cb.item.setValue(id);
        cb.item.focus();
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
            warehouseId: +r.WarehouseId || 0, itemId: +r.ItemId || 0, scheduleId: SE.toInt(r.ScheduleId), packUomId: +r.PackUomId || 0, attributeVarientId: +r.AttributeVarientId || 0, jobLotId: +r.JobLotId || 0,
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
        return { id: +r.Id || 0, gdnId: +r.GdnId || 0, gdnWagesId: +r.GdnWagesId || 0, orderId: +r.OrderId || 0, orderWagesId: +r.OrderWagesId || 0, itemId: +r.ItemId || 0,
            attributeVarientId: +r.AttributeVarientId || 0, contractorId: +r.ContractorId || 0,
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
        if (T('txtRefPartyName') === '') return stop('Ref Party Name Field is Required', function () { $('txtRefPartyName').focus(); });
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
                var wv = wagesValid();
                if (wv) return stop(wv);
                recalc();
                if (N('txtItemAmountHeader') === 0 || N('txtItemNetAmountHeader') === 0) return stop('ItemAmount and ItemNetAmount can not equal to zero Please check');
                var wages = G.wg.rows().filter(function (r) { return (+r.ContractorWagesRateScheduleId || 0) > 0 || (+r.ContractorId || 0) > 0 || (+r.ItemId || 0) > 0 || (+r.AttributeVarientId || 0) > 0; });
                var body = { id: S.id, docDate: $('DocDate').value, docNo: T('txtdocno'), referenceNo: $('txtSupplierReference').value, manualBillNo: $('txtbillno').value, remarks: $('txtremarks').value,
                    dueDays: T('txtduedays'), dueDate: $('duedate').value, distance: $('txtDistance').value, deliveryStartDate: $('txtDeliveryStartDate').value, deliveryDays: T('txtDeliverydays'),
                    deliveryTerm: cb.dterm.text(), refPartyName: T('txtRefPartyName'), refPartyAddress: $('txtRefPartyAddress').value, refPartyCellNo: $('txtRefPartyCellNo').value,
                    supplierCustomerId: iv(cb.sup), visitedById: iv(cb.visited), paymentTermId: iv(cb.term), commissionAgentId: iv(cb.agent), refSalesManId: iv(cb.salesman),
                    commissionDebitAccountId: iv(cb.cdebit), currencyId: iv(cb.cur),
                    transporterId: iv(cb.trans), otherWagesAccountId: iv(cb.owages), settlementDiscountAccountId: iv(cb.settle), commissionType: cb.ctype.text(), commissionUom: cb.cuom.text(),
                    commissionRemarks: $('txtCommissionRemarks').value, commissionRate: N('txtcommrate'), commissionAmount: N('txtcommamount'), exchangeRate: N('txtExchangeRate'),
                    freightAmount: N('txtFreightAmountHeader'), otherWages: N('txtOtherWagesHeader'), settlementDiscount: N('txtSettlementDiscountHeader'),
                    rows: G.grd.rows().map(lineOf), removed: S.remove.slice(), expenses: G.exp.rows().filter(function (r) { return (+r.ItemId || 0) > 0 || (+r.Amount || 0) > 0; }).map(expOf),
                    journals: jv.map(jvOf), wages: wages.map(wgOf),
                    attachments: { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removedAtt } };
                return SE.api(API, { method: 'POST', body: body }).then(function (r) {
                    return msg(r.message).then(function () {
                        var vch = $('ChkBok').checked, slip = $('ChkCustomerPrintslip').checked;
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
        return p.then(function () { S.id = id; S.remove = []; return SE.api(API + '/' + id); }).then(function (d) {
            var h = d.head, lines = d.lines || [];
            tabs.select('tabPage1');
            $('DocDate').value = SE.dateInput(ci(h, 'DocDate')); $('txtdocno').value = cs(h, 'DocNo');
            $('txtbillno').value = cs(h, 'ManualBillNo'); $('txtSupplierReference').value = cs(h, 'RefrenenceNo');
            cb.sup.setValue(cn(h, 'SupplierCustomerId')); supplierRefresh();
            cb.visited.setValue(cn(h, 'VisitedById')); $('txtremarks').value = cs(h, 'RemarksHeader');
            cb.term.setValue(cn(h, 'PaymentTermsId'));
            $('duedate').value = parseDate(cs(h, 'DueDate')) || $('duedate').value;
            $('txtduedays').value = cs(h, 'DueDays'); $('txtduedays').disabled = iv(cb.term) === 1;
            setByText(cb.dterm, cs(h, 'DeliveryTerm'));
            $('txtDeliveryStartDate').value = parseDate(cs(h, 'DeliveryStartDate')); $('txtDeliverydays').value = cs(h, 'DeliveryDays'); $('txtDistance').value = cs(h, 'Distance');
            $('txtRefPartyName').value = cs(h, 'ReferencPartyName'); $('txtRefPartyAddress').value = cs(h, 'ReferencPartyAddress'); $('txtRefPartyCellNo').value = cs(h, 'ReferencPartyCellNo');
            cb.agent.setValue(cn(h, 'CommissionAgentId')); cb.salesman.setValue(cn(h, 'RefSalesManId'));
            setByText(cb.ctype, cs(h, 'CommissionType'));
            $('txtcommrate').value = fr(cn(h, 'CommissionRate'));
            var uom = cn(h, 'CommissionUom'), hit = null;
            cb.cuom.rows().forEach(function (r) { if (SE.toNum(r.UOM) === uom) hit = r; });
            if (hit) cb.cuom.setValue(hit.Id); else cb.cuom.clear();
            $('txtcommamount').value = fa(cn(h, 'CommissionAmount')); $('txtCommissionRemarks').value = cs(h, 'CommissionRemarks');
            $('txtInvoiceQty').value = f2(cn(h, 'InvoiceQty')); $('txtInvoiceWeight').value = f2(cn(h, 'InvoiceWeight'));
            $('txtExchangeRate').value = fr(cn(h, 'ExchangeRate'));
            cb.cur.setValue(cn(h, 'CurrencyId')); $('txtFcyAmount').value = ff(cn(h, 'FcyAmount'));
            cb.trans.setValue(cn(h, 'TransporterId')); cb.owages.setValue(cn(h, 'OtherWagesAccountId')); cb.settle.setValue(cn(h, 'SettlementDiscountAccountId'));
            cb.cdebit.setValue(cn(h, 'CommissionDebitAcId'));
            $('txtItemAmountHeader').value = fa(cn(h, 'ItemAmountHeader')); $('txtDiscountHeader').value = fa(cn(h, 'DiscountAmountHeader')); $('txtItemNetAmountHeader').value = fa(cn(h, 'ItemNetAmountHeader'));
            $('txtOtherWagesHeader').value = fa(cn(h, 'OtherWagesHeader')); $('txtFreightAmountHeader').value = fa(cn(h, 'FrieghtAmountHeader'));
            $('txtSettlementDiscountHeader').value = fa(cn(h, 'SettlementDiscountHeader')); $('txtBillAmountHeader').value = fa(cn(h, 'BillAmount'));
            $('txtManualTotalBillAmount').value = fa(cn(h, 'BillAmount'));
            cb.branch.setValue(cn(h, 'BranchesId')); cb.proj.setValue(cn(h, 'ProjectsId'));
            S.approved = cb2(h, 'IsApproved');
            if (cn(h, 'CurrencyId') > 0) $('txtExchangeRate').value = String(cn(h, 'ExchangeRate')); else configDefault();
            S.voucherHeadId = +d.voucherHeadId || 0;
            G.grd.setRows(lines.map(function (l) {
                return { Id: cn(l, 'Id'), GdnId: cn(l, 'InvGdnId'), GdnDetailId: cn(l, 'InvGdnDetailId'), GdnNo: cs(l, 'GdnNo'), OrderId: cn(l, 'SaleOrderId'), OrderDetailId: cn(l, 'SaleOrderDetailId'),
                    OrderNo: cs(l, 'SaleOrderNo'), WarehouseId: cn(l, 'WarehouseId'), Warehouse: cs(l, 'WareHouseName'), ItemId: cn(l, 'ItemId'), ItemName: cs(l, 'ItemName'), ItemCode: cs(l, 'ItemCode'),
                    ScheduleId: cn(l, 'ScheduleId'), PackUomId: cn(l, 'ItemUomId'), PackUom: cs(l, 'PackUom'), PackUomEquivalent: cn(l, 'PackEquivalent'),
                    AttributeVarientId: cn(l, 'ItemAttributeVarientId'), AttributeVarient: cs(l, 'ItemAttributeVarient'), VarientUnit: cn(l, 'VarientEquivalent'),
                    JobLotId: cn(l, 'JobLotId'), JobLot: cs(l, 'JobLotDescription'), ItemQty: cn(l, 'ItemQty'), BalQty: cn(l, 'ItemQty'), ItemWeight: cn(l, 'ItemWeight'), NetWeight: cn(l, 'ItemNetWeight'),
                    BalWeight: cn(l, 'ItemNetWeight'), ItemPrice: cn(l, 'ItemRateWithOutAddLess'), AddLessRate: cn(l, 'RateAddLess'), Rate: cn(l, 'ItemRate'), ItemAmount: cn(l, 'ItemAmount'),
                    DiscountTypeId: cn(l, 'ItemDiscountTypeId'), DiscountType: cs(l, 'ItemDiscountType'), DiscRate: cn(l, 'ItemDiscountRate'), DiscAmount: cn(l, 'ItemDiscountAmount'),
                    ItemAmountWithDisc: cn(l, 'ItemAmountWithDisc'), FcyAmount: cn(l, 'FcyAmount'), GpDate: parseDate(cs(l, 'GpDate')), GpNo: cs(l, 'GpNo'), VehicleNo: cs(l, 'VehicleNo'),
                    CityId: cn(l, 'CityId'), CityName: cs(l, 'CityName'), Expense: cn(l, 'ExpenseAmount'), SettleDisc: cn(l, 'JournalAmount'), Commission: cn(l, 'CommissionAmount'),
                    Freight: cn(l, 'FreightAmount'), Wages: cn(l, 'WagesAmount'), ItemNetAmount: cn(l, 'ItemNetAmount'), IsFOC: cb2(l, 'IsFOC'), ItemWithVarient: cb2(l, 'ItemWithVarient') };
            }));
            if (lines.length) { S.orderExist = cn(lines[lines.length - 1], 'SaleOrderId') > 0; S.gdnExist = cn(lines[lines.length - 1], 'InvGdnId') > 0; }   // ReadById: the last row decides
            gridMode();
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
            var wl = (d.wages || []).map(function (w) {
                return { Id: cn(w, 'Id'), GdnId: cn(w, 'InvGdnId'), GdnWagesId: cn(w, 'InvGdnWagesId'), OrderId: cn(w, 'SaleOrderId'), OrderWagesId: cn(w, 'SaleOrderWagesId'), ItemId: cn(w, 'ItemId'),
                    ItemName: cs(w, 'ItemName'), AttributeVarientId: cn(w, 'ItemAttributeVarientId'), AttributeVarient: cs(w, 'ItemAttributeVarient'), VarientEquivalent: cn(w, 'VarientEquivalent'),
                    ContractorId: cn(w, 'ContractorId'), ContractorName: cs(w, 'ContractorName'), ContractorWagesRateScheduleId: cn(w, 'ContractorWagesRateScheduleId'), ServiceActivity: cs(w, 'ServiceActivity'),
                    ParentUomId: cn(w, 'ParentUomId'), Qty: cn(w, 'Qty'), ItemNetWeight: cn(w, 'ItemNetWeight'), Rate: cn(w, 'Rate'), Amount: cn(w, 'Amount'), AddLess: cn(w, 'AddLess'),
                    NetAmount: cn(w, 'NetAmount'), Remarks: cs(w, 'Remarks') };
            });
            G.wg.setRows(wl.length ? wgSort(wl) : [wgBlank()]);
            lock('comsupplier', true);
            S.existing = d.attachments || []; S.files = []; S.removedAtt = [];
            show('btnSave', false); show('btnUpdate', true); show('btnDelete', true);
            recalc();                                                                       // the ReadById tail: proportions, BillAmount, exchange
            fillWagesCombo();
        }).catch(fail);
    }
    function del() {                                                                        // btnDelete_Click
        if (S.id <= 0) return msg('Record Not Found');
        if (S.approved) return msg('Record Not Delete beacause Record has approved');
        return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
            if (!yes) return;
            return SE.api(API + '/' + S.id, { method: 'DELETE' }).then(function (r) { return msg(r.message || 'Delete Record Successfully').then(reset); })
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
            var rows = (d.lines || []).map(function (l) {
                return { Id: cn(l, 'Id'), GdnId: cn(l, 'InvGdnId'), GdnNo: cs(l, 'GdnNo'), OrderId: cn(l, 'SaleOrderId'), OrderNo: cs(l, 'SaleOrderNo'), Warehouse: cs(l, 'WareHouseName'),
                    ItemId: cn(l, 'ItemId'), ItemName: cs(l, 'ItemName'), ItemCode: cs(l, 'ItemCode'),
                    AttributeVarient: cs(l, 'ItemAttributeVarient'), VarientUnit: cn(l, 'VarientEquivalent'), JobLot: cs(l, 'JobLotDescription'), ItemQty: cn(l, 'ItemQty'), ItemWeight: cn(l, 'ItemWeight'),
                    NetWeight: cn(l, 'ItemNetWeight'), ItemPrice: cn(l, 'ItemRateWithOutAddLess'), AddLessRate: cn(l, 'RateAddLess'), Rate: cn(l, 'ItemRate'), ItemAmount: cn(l, 'ItemAmount'),
                    DiscountType: cs(l, 'ItemDiscountType'), DiscRate: cn(l, 'ItemDiscountRate'), DiscAmount: cn(l, 'ItemDiscountAmount'), ItemAmountWithDisc: cn(l, 'ItemAmountWithDisc'),
                    FcyAmount: cn(l, 'FcyAmount'), GpDate: cs(l, 'GpDate'), GpNo: cs(l, 'GpNo'), VehicleNo: cs(l, 'VehicleNo'), CityName: cs(l, 'CityName'), Expense: cn(l, 'ExpenseAmount'),
                    Journal: cn(l, 'JournalAmount'), Commission: cn(l, 'CommissionAmount'), Freight: cn(l, 'FreightAmount'), Wages: cn(l, 'WagesAmount'), ItemNetAmount: cn(l, 'ItemNetAmount'), IsFOC: cb2(l, 'IsFOC') };
            });
            G.hd.spec.cols.forEach(function (c) {                                           // DetailGridSetting: FcyAmount follows the multi currency feature; OrderNo / GdnNo are hidden when the first row has none
                if (c.k === 'FcyAmount') c.hide = !S.multi;
                if (c.k === 'OrderNo') c.hide = !(rows.length && rows[0].OrderId > 0);
                if (c.k === 'GdnNo') c.hide = !(rows.length && rows[0].GdnId > 0);
            });
            G.hd.setRows(rows);
        }).catch(function () { G.hd.setRows([]); });
    }

    /* ------------------------------------------------------------------ print / attachments */
    function printSlip(id, item) {                                                          // 1856-SaleInvoiceDirect_Slip (customer slip) / 1856A-SaleInvoiceDirect_Slip (item slip)
        if (!(id > 0)) return msg('No Record Found For Display');
        return SPC.openPdf((item ? IPRINT : PRINT) + SE.q({ id: id }));
    }
    function printVoucher(id) {                                                             // AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 1856)
        if (!(id > 0)) return msg('No Record Found For Display');
        return SPC.openPdf(VPRINT + SE.q({ id: id }));
    }
    function showAttachments(id) { return SE.api(API + '/' + id + '/attachments').then(function (rows) { SPC.attachments(S, API, id, true, rows); }).catch(fail); }
    function openAttachments() { SPC.attachments(S, API, S.id, false); }

    /* ------------------------------------------------------------------ shortcut keys */
    var SHORT = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
        ['Ctrl+P', 'For Print'], ['Alt+1', 'For Print Slip 1856'], ['Alt+2', 'For Print Slip 1856A'], ['Alt+3', 'For Print Voucher 103'],
        ['Ctrl+F5', 'For Focus on Customer'], ['Ctrl+ArrowUp', 'For Focus on Warehouse'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+D', 'For Adding an row in Focused Grid'], ['Ctrl+Delete', 'For Deleting an row of Focused Grid'], ['Ctrl+Enter', 'For Editing the focused detail row'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowRight', 'For Change Focus from one Grid To another Grid'], ['F1', 'List of the focused grid cell'],
        ["Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function onKey(e) {
        var t = e.target, k = e.key.toLowerCase();
        if (LD && LD.pop && LD.pop.el.classList.contains('on')) return;
        if (e.key === 'Enter' && !e.ctrlKey && t && (t.tagName === 'INPUT' || t.tagName === 'SELECT') && t.type !== 'checkbox' && t.type !== 'radio' && t.type !== 'button' && !t.classList.contains('cell')) {
            var f = Array.prototype.filter.call(document.querySelectorAll('.dform input:not([type=hidden]):not([disabled]), .dform select:not([disabled]), .dform .dtcombo-input'), function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(t); if (i >= 0 && f[i + 1]) { e.preventDefault(); f[i + 1].focus(); }
        }
        if (e.key === 'F1' && $('grdPartyAddLs').contains(document.activeElement)) {
            var cur = G.jv.cur(); if (cur && /^(AccountTitle|AccountId|GlAccountId)$/.test(G.jv.curKey() || '')) { e.preventDefault(); pickAccount(cur); }
        }
        if (e.key === 'F1' && $('grdContractorWages').contains(document.activeElement)) { e.preventDefault(); wgPick(); }
        if (e.ctrlKey && k === 't') { e.preventDefault(); if (tabs.index() === 1) { tabs.select('tabPage1'); cb.sup.focus(); } else { tabs.select('tabPage2'); tabHistory(); $('grdHistory').focus(); } return; }
        if (tabs.index() === 0) {
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh(); }
            else if (e.ctrlKey && e.shiftKey && e.key === 'Delete') { e.preventDefault(); if (isShown('btnDelete') && !$('btnDelete').disabled) del(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); if (isShown('btnSave') && !$('btnSave').disabled) btnSave(); }
            else if (e.ctrlKey && k === 'u') { e.preventDefault(); if (isShown('btnUpdate') && !$('btnUpdate').disabled) btnUpdate(); }
            else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); cb.sup.focus(); }
            else if (e.ctrlKey && e.key === 'ArrowUp') { e.preventDefault(); cb.wh.focus(); }
            else if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); openAttachments(); }
            else if (e.ctrlKey && k === 'p') { e.preventDefault(); printSlip(S.id, false); }
            else if (e.altKey && e.key === '1') { e.preventDefault(); printSlip(S.id, false); }
            else if (e.altKey && e.key === '2') { e.preventDefault(); printSlip(S.id, true); }
            else if (e.altKey && e.key === '3') { e.preventDefault(); printVoucher(S.id); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { $('grd').focus(); }
            else if (e.ctrlKey && e.key === 'Enter' && $('grd').contains(document.activeElement)) { var ci0 = G.grd.curIndex(); if (ci0 >= 0) editRow(ci0); }
            else if (e.ctrlKey && e.key === 'ArrowRight') {
                var order = ['grd', 'grdChargeToProduct', 'grdPartyAddLs', 'grdContractorWages'], tabOf = { grd: 'tabPage3', grdChargeToProduct: 'tabPage4', grdPartyAddLs: 'tabPage5', grdContractorWages: 'tabPage6' };
                var at = -1; order.forEach(function (id, n) { if ($(id).contains(document.activeElement)) at = n; });
                if (at >= 0) { var nx = order[(at + 1) % order.length]; tabs2.select(tabOf[nx]); $(nx).focus(); }
            }
            else if (e.ctrlKey && e.key === 'Delete') {
                if ($('grd').contains(document.activeElement)) { var gi = G.grd.curIndex(); if (gi >= 0 && !S.gdnExist) deleteRow(gi); }
                else if ($('grdChargeToProduct').contains(document.activeElement)) { var ei = G.exp.curIndex(); if (ei >= 0) expDelete(ei); }
                else if ($('grdPartyAddLs').contains(document.activeElement)) { var ji = G.jv.curIndex(); if (ji >= 0) jvDelete(ji); }
                else if ($('grdContractorWages').contains(document.activeElement)) { var wi = G.wg.curIndex(); if (wi >= 0) wgDelete(wi); }
            }
            else if (e.ctrlKey && k === 'd') {
                if ($('grdChargeToProduct').contains(document.activeElement)) { e.preventDefault(); expAdd(); }
                else if ($('grdPartyAddLs').contains(document.activeElement)) { e.preventDefault(); jvAdd(); }
                else if ($('grdContractorWages').contains(document.activeElement)) { e.preventDefault(); wgAdd(); }
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
        $('btnAttachment').onclick = openAttachments; $('btnCustomerSlip').onclick = function () { printSlip(S.id, false); }; $('btn1856APrint').onclick = function () { printSlip(S.id, true); };
        $('btnPrint').onclick = function () { printVoucher(S.id); }; $('BtnLoadOrder').onclick = btnLoadOrder; $('btnLoadGdn').onclick = btnLoadGdn;
        $('btnshortcutkeys').onclick = function () { SE.shortcuts(SHORT); };
        $('btnDefinePlant').onclick = function () { window.open('/party-processing/reference-parties', '_blank'); };                // DefineReferenceParties
        $('btnAdd').onclick = btnAdd; $('btnUpdateDetail').onclick = btnUpdateDetail; $('btnCancelUpdateDetial').onclick = btnCancelUpdateDetail;
        $('btnUpdateDiscountInGrid').onclick = updateSameDiscount;
        $('BtnGenerateWagesRows').onclick = btnGenerateWages; $('BtnUpdateConctratorInWages').onclick = btnUpdateContractor;
        $('BtnNewHistory').onclick = resetHistory; $('BtnRefreshHistory').onclick = histCombos; $('btnshow').onclick = showHistory;
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
        var hook = false;
        document.addEventListener('keydown', function (e) { if (e.ctrlKey && e.altKey && !hook) { hook = true; SE.shortcuts(SHORT); setTimeout(function () { hook = false; }, 400); } });
    }

    document.addEventListener('DOMContentLoaded', load);
})();
