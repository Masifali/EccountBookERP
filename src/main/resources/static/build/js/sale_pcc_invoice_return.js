/*
 * Screen 553  SaleInvoiceReturnConcrete  (Architecture.WinApp.pcc.Sale.SaleInvoiceReturnConcrete, document type 1862)
 * Page script. Desktop methods are named in the comments (SaleInvoiceReturnConcrete.cs). Server: /sale/pcc/sale-invoice-return/api
 * All amounts are re-calculated by the server on save (CalculateDetailAmount / grd_CellUpdated / CalculateRowNetAmount / BillAmount).
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/pcc/sale-invoice-return/api', PRINT = '/sale/pcc/print/sale-invoice-return-slip', VPRINT = '/sale/pcc/print/sale-invoice-return-voucher';
    var F = { amtRound: 0, amt: 0, rateRound: 2, rate: 2, fcy: 0 };                       // clsGlobalVariables decimal settings (CommonServices.GetDecimalConfiguration)
    var S = { id: 0, approved: false, rights: {}, remove: [], updateIdx: -1, files: [], removedAtt: [], existing: [], voucherHeadId: 0, multi: false, subsidiary: false,
        itemByCode: false, partyByCode: false, def: {}, L: {}, hist: [], fyStart: '', dateTypes: [], accounts: null, supGl: 0, hc: { customers: [], agents: [] }, items: [], suppliers: [], commDebit: [] };
    var cb = {}, G = {}, tabs, tabs2;

    function fail(e) { return SE.dbError(e); }
    function msg(t, c) { return SE.alert(t, c); }
    function iv(c) { return +c.value() || 0; }
    function N(id) { return SE.toNum($(id).value); }
    function T(id) { return ($(id).value || '').trim(); }
    function isShown(id) { var b = $(id); return !!b && b.style.display !== 'none' && !b.hidden; }
    function show(id, on) { var b = $(id); if (!b) return; b.style.display = on ? '' : 'none'; if (b.hasAttribute('hidden')) b.hidden = !on; }
    function showCombo(id, on) { var w = $(id).closest('.dtcombo-wrap'); (w || $(id)).style.display = on ? '' : 'none'; }
    function ci(o, k) {                                                                     // case-insensitive column read
        if (!o) return undefined;
        if (o[k] !== undefined) return o[k];
        var lk = String(k).toLowerCase();
        for (var p in o) if (p.toLowerCase() === lk) return o[p];
        return undefined;
    }
    function cn(o, k) { var v = ci(o, k); return v == null || v === '' ? 0 : SE.toNum(v); }
    function cs(o, k) { var v = ci(o, k); return v == null ? '' : String(v); }

    /* ------------------------------------------------------------------ number rules */
    function rnd(v, dp) {                                                                   // Math.Round(v, dp, MidpointRounding.AwayFromZero)
        v = +v || 0;
        var s = v < 0 ? -1 : 1, a = Math.abs(v), t = String(a);
        if (t.indexOf('e') >= 0) return s * Math.round(a * Math.pow(10, dp)) / Math.pow(10, dp);
        return s * Number(Math.round(Number(t + 'e' + dp)) + 'e-' + dp);
    }
    function amt(v) { return rnd(rnd(v, F.amtRound), F.amt); }                               // Math.Round(.., DefaultNoofDecimalPointsForAmount) then stringFormatsingle
    function fa(v) { return SE.num(amt(v), F.amt, F.amt); }                                  // ToString(stringFormatsingle)
    function fr(v) { return SE.num(rnd(v, F.rate), F.rate, F.rate); }                        // ToString(DecimalRateFormate)
    function ff(v) { return SE.num(rnd(v, F.fcy), F.fcy, F.fcy); }                           // ToString(stringFormatsingleForFcy)
    function f2(v) { return SE.num(v, 2, 0); }                                               // "#,##0.##"
    function f3(v) { return SE.num(v, 3, 0); }                                               // "#,##0.###"
    function dbl(v) { return String(+Number(v).toPrecision(15)); }                          // double.ToString()
    function nameIn(list, id, key, idKey) { for (var i = 0; i < (list || []).length; i++) if (+list[i][idKey || 'Id'] === +id) return list[i][key]; return ''; }

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
        cb.dterm = mk('combdeliverytrm', 'DeliveryTerm', 'Delivery Term');
        cb.cur = mk('cmbCurrency', 'CurrencyCode', 'Currency', { onSelect: currencyLeave });
        cb.branch = mk('combranches', 'BranchName', 'Branch');
        cb.agent = mk('combcommAgent', 'CompanyName', 'SalesMan Name', { popupWidth: 420, onSelect: commChanged });
        cb.salesman = mk('CmbRefSalesMan', 'ReferencePartyName', 'Ref Sales Man');
        cb.ctype = mk('combCommType', 'CommissionType', 'Commission Type', { onSelect: commChanged });
        cb.proj = mk('comproject', 'ProjectName', 'Project');
        cb.wh = mk('comWarehouse', 'WareHouseName', 'WareHouse Name', { onSelect: stockLabel });
        cb.item = XCombo('comItem', { columns: [{ key: '_t', caption: 'Item Name' }], textKey: '_t', popupWidth: 420, onSelect: itemLeave });
        cb.varient = XCombo('CmbAttributeVarient', { columns: [{ key: 'ItemAttribute', caption: 'Varient' }], valueKey: 'ItemAttributeVarientId', textKey: 'ItemAttribute', onSelect: varientLeave });
        cb.disc = mk('CmbDiscountType', 'DiscountType', 'Discount Type', { onSelect: calcDisc });
        cb.job = mk('comjobLot', 'JobLotDescription', 'Job Lot', { onSelect: stockLabel });
        cb.city = mk('txtCity', 'CityName', 'City Name');
        cb.cuom = mk('combcommUOM', 'UOM', 'UOM', { onSelect: commChanged });
        cb.cdebit = mk('CmbCommissionDebitAccount', 'AccountTitle', 'CommissionDebit Account Title', { popupWidth: 420 });
        cb.dateType = mk('cmbDateTypeHistory', 'Parameters', 'Parameters', { onSelect: dateTypeChanged });
        cb.hCust = mk('CmbCustomerHistory', '_t', 'Party Name', { popupWidth: 380 });
        cb.hAgent = mk('CmbCommissionAgentHistory', 'CommissionAgent', 'CommissionAgent', { popupWidth: 300 });
    }
    function keep(c, rows, fn) {                                                            // desktop pattern: rebind, restore the previous Value when it is still in the list
        var k = iv(c);
        c.setData(rows || []);
        if (k > 0 && !c.setValue(k)) c.clear();
        if (fn) fn();
    }
    function supRows() {                                                                    // RdPartyByName_CheckedChanged: PartyCode or CompanyName
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
    function commDebitRows() {                                                              // CommissionDebitAccountFill: the supplier's own GL account is left out
        return (S.commDebit || []).filter(function (r) { return +r.Id !== +S.supGl; });
    }
    function fillLists(d) {                                                                 // CurrencyFill, suppliercustomer, VisitedByFill, PaymentTerms, RefSalesManFill, CommissionDebitAccountFill, bindWareHouse, item, JobLotBind, BinCity
        var L = S.L;
        ['currencies', 'customers', 'visitedBy', 'terms', 'deliveryTerms', 'refSalesMan', 'commTypes', 'commUoms', 'commDebit', 'warehouses', 'items', 'jobLots', 'discTypes', 'cities', 'projects', 'branches']
            .forEach(function (k) { if (d[k]) L[k] = d[k]; });
        S.suppliers = L.customers || []; S.items = L.items || []; S.commDebit = L.commDebit || [];
        keep(cb.cur, L.currencies); keep(cb.sup, supRows()); keep(cb.agent, S.suppliers); keep(cb.visited, L.visitedBy); keep(cb.term, L.terms);
        keep(cb.salesman, L.refSalesMan); keep(cb.cdebit, commDebitRows()); keep(cb.wh, L.warehouses);
        var itemId = iv(cb.item); cb.item.setData(itemRows()); if (itemId > 0 && !cb.item.setValue(itemId)) cb.item.clear();
        keep(cb.job, L.jobLots); keep(cb.city, L.cities);
        if (!cb.dterm.rows().length) { cb.dterm.setData(L.deliveryTerms); cb.dterm.setValue(1); }
        if (!cb.ctype.rows().length) cb.ctype.setData(L.commTypes);
        if (!cb.cuom.rows().length) { cb.cuom.setData(L.commUoms); cb.cuom.setValue(1); }
        if (!cb.disc.rows().length) cb.disc.setData(L.discTypes);
        keep(cb.proj, L.projects); keep(cb.branch, L.branches);
        G.grd.refresh();
    }
    function configDefault() {                                                              // ConfigurationDefault
        var d = S.def;
        if (+d.warehouseId > 0) cb.wh.setValue(d.warehouseId);
        if (+d.cityId > 0 && iv(cb.city) === 0) cb.city.setValue(d.cityId);
        if (+d.jobLotId > 0 && iv(cb.job) === 0) cb.job.setValue(d.jobLotId);
        if (+d.baseCurrency > 0 && iv(cb.cur) === 0) cb.cur.setValue(d.baseCurrency);
        if (+d.baseRate && N('txtExchangeRate') === 0) { $('txtExchangeRate').value = fr(+d.baseRate); }
    }
    function multiCurrency() {                                                              // MultiCurrencyFeature
        var on = S.multi;
        showCombo('cmbCurrency', on);
        ['txtExchangeRate', 'txtFcyAmount', 'label59', 'label60', 'label61'].forEach(function (id) { show(id, on); });
        $('txtRefPartyAddress').style.width = on ? '198px' : '384px';
        $('txtremarks').style.width = on ? '422px' : '608px';
    }

    /* ------------------------------------------------------------------ entry panel calculations */
    /* CalculateDetailAmount */
    function calcDetail() {
        var qty = N('txtQty'), rate = N('txtRate'), unit = N('txtVarientEquivalent'), w = N('txtItemWeight');
        if (unit > 0 && qty > 0) {
            var net = rnd(unit * qty * w, 2);
            $('txtNetWeight').value = net === 0 ? '' : f2(net);                              // ToString("##,#.##")
            var a = amt(unit * qty * rate);
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
                    pct = 99; $('txtDiscountRate').value = f3(pct);
                    disc = amt(itemAmt * pct / 100); $('txtDiscountAmount').value = fa(disc);
                    $('txtTotalAmount').value = String(itemAmt - disc);
                    return;
                }
                $('txtDiscountRate').value = f3(pct);
            } else { disc = amt(itemAmt * pct / 100); $('txtDiscountAmount').value = fa(disc); }
        } else if (type === 'Flat') {
            if (act === 'txtDiscountRate') {
                $('txtDiscountRate').value = f3(rnd(N('txtDiscountRate'), F.amtRound));
                pct = N('txtDiscountRate'); disc = pct; $('txtDiscountAmount').value = fa(pct);
            } else if (act === 'txtDiscountAmount') {
                $('txtDiscountAmount').value = f3(rnd(N('txtDiscountAmount'), F.amtRound));
                disc = N('txtDiscountAmount'); $('txtDiscountRate').value = f3(disc);
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
            else if (act === 'txtAddLess') $('txtRate').value = dbl(price + add);
            else $('txtRate').value = SE.num(price + add, 4, 0);
        }
    }
    function onQty() { calcDetail(); calcDisc(); }
    function onRate() { netRate(); calcDetail(); calcDisc(); }

    /* comItem_Leave */
    function itemLeave() {
        var id = iv(cb.item), r = cb.item.row();
        if (id > 0 && r) {
            $('txtItemWeight').value = r.ItemWeight == null ? '' : String(r.ItemWeight);
            var price = +r.ItemRate || 0;
            $('txtItemPrice').value = SE.num(price, 4, 0); $('txtRate').value = SE.num(price, 4, 0);
            bindVarient(id);
        } else $('txtItemWeight').value = '0';
        stockLabel();
    }
    /* bindvarientunit */
    function bindVarient(itemId) {
        var prev = cb.varient.text();
        return SE.api(API + '/varients' + SE.q({ itemId: itemId })).then(function (rows) {
            cb.varient.setData(rows || []);
            var hit = null;
            (rows || []).forEach(function (r) { if (prev && String(r.ItemAttribute) === prev) hit = r; });
            if (hit) cb.varient.setValue(hit.ItemAttributeVarientId); else cb.varient.clear();
            $('txtVarientEquivalent').value = hit ? String(hit.VarientEquivalent == null ? 0 : hit.VarientEquivalent) : '';
            if (hit) { calcDetail(); calcDisc(); }
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

    /* ------------------------------------------------------------------ header: terms / due date / currency / supplier / commission */
    function termChanged() {                                                                // combpttrm_ValueChanged
        var one = iv(cb.term) === 1;
        if (one) { $('txtduedays').value = ''; $('txtduedays').disabled = true; } else $('txtduedays').disabled = false;
        dueGen();
    }
    function dueGen() {                                                                     // DueDateGenerate
        var d = SE.parts ? $('DocDate').value : '';
        if (!d) { $('duedate').value = ''; return; }
        var p = d.split('-'), dt = new Date(+p[0], +p[1] - 1, +p[2]);
        var days = T('txtduedays');
        if (days !== '') dt.setDate(dt.getDate() + (+days || 0));
        $('duedate').value = dt.getFullYear() + '-' + ('0' + (dt.getMonth() + 1)).slice(-2) + '-' + ('0' + dt.getDate()).slice(-2);
    }
    function supplierChanged() {                                                            // comsupplier_ValueChanged
        billAmount();
        var r = cb.sup.row();
        if (!r || iv(cb.sup) <= 0) return;
        S.supGl = cn(r, 'GlAccountId'); $('txtSupplierGLId').value = String(S.supGl);
        keep(cb.cdebit, commDebitRows());                                                   // CommissionDebitAccountFill
    }
    function currencyLeave() {                                                              // cmbCurrency_Leave
        if (iv(cb.cur) === 0 || N('txtExchangeRate') !== 0) return;
        if (iv(cb.cur) !== +S.def.baseCurrency) {
            SE.api(API + '/last-rate' + SE.q({ currencyId: iv(cb.cur) })).then(function (r) {
                $('txtExchangeRate').value = r.rate ? fr(+r.rate) : '0'; exchChanged();
            }).catch(fail);
        } else { $('txtExchangeRate').value = fr(+S.def.baseRate || 0); exchChanged(); }
    }
    /* txtExchangeRate_TextChanged */
    function exchChanged() {
        var ex = N('txtExchangeRate');
        G.grd.rows().forEach(function (r) { r.FcyAmount = ex > 0 ? (+r.ItemAmountWithDisc || 0) / ex : 0; });
        billAmount();
    }
    /* TotalCommissionAmount */
    function totalComm() {
        if ($('txtcommrate').value === '') return;
        var t = cb.ctype.text(), rate = N('txtcommrate');
        if (t === 'Flat') $('txtcommamount').value = fa(rate);
        if (t === 'Percent' || t === 'Percentage') $('txtcommamount').value = fa(N('txtBillamountWithoutCommission') * rate / 100);
    }
    function commChanged() { totalComm(); exchChanged(); }

    /* ------------------------------------------------------------------ BillAmount */
    function gridNet() {                                                                    // GridItemNetAmountCalculate / CalculateRowNetAmount
        G.grd.rows().forEach(function (r) { r.ItemNetAmount = amt(amt(+r.ItemAmountWithDisc || 0) + amt(+r.Commission || 0)); });
        G.grd.refresh();
    }
    function jvSums() {
        var d = 0, c = 0;
        G.jv.rows().forEach(function (r) { if ((+r.AccountId || 0) > 0 || (+r.GlAccountId || 0) > 0) { d += +r.Debit || 0; c += +r.Credit || 0; } });
        return { d: d, c: c };
    }
    function billAmount() {
        var rows = G.grd.rows();
        if (rows.length > 0) {
            var qty = 0, wt = 0, ia = 0, da = 0, wd = 0;
            rows.forEach(function (r) {
                qty += +r.ItemQty || 0; wt += +r.NetWeight || 0;
                if (!r.IsFOC) { ia += +r.ItemAmount || 0; da += +r.DiscAmount || 0; wd += +r.ItemAmountWithDisc || 0; }
            });
            var j = jvSums(), bill = wd + j.c - j.d;
            $('txtItemAmountHeader').value = fa(ia); $('txtDiscountHeader').value = fa(da); $('txtItemNetAmountHeader').value = fa(wd);
            $('txtBillamountWithoutCommission').value = fa(bill);
            var ca = N('txtcommamount');
            if (iv(cb.agent) > 0 && ca > 0 && iv(cb.sup) === iv(cb.agent)) {
                bill -= ca; $('CommNetAmtHeader').value = fa(ca); show('CommNetAmtHeader', true); show('label84', true);
            } else { $('CommNetAmtHeader').value = '0'; show('CommNetAmtHeader', false); show('label84', false); }
            $('txtBillAmountHeader').value = fa(bill); $('txtManualTotalBillAmount').value = fa(bill);
            $('txtInvoiceQty').value = f2(rnd(qty, 2)); $('txtInvoiceWeight').value = f2(rnd(wt, 2));
            var ex = N('txtExchangeRate');
            $('txtFcyAmount').value = ex > 0 && wd > 0 ? ff(wd / ex) : ff(0);
        } else {
            $('txtInvoiceQty').value = '0'; $('txtInvoiceWeight').value = '0'; $('txtFcyAmount').value = '0';
        }
        gridNet();
    }

    /* ------------------------------------------------------------------ grids */
    function d3(v) { return v === '' || v == null ? '' : f2(v); }
    function ra(v) { return v === '' || v == null ? '' : fa(v); }
    function rr(v) { return v === '' || v == null ? '' : fr(v); }
    function rf(v) { return v === '' || v == null ? '' : ff(v); }
    function parseDate(v) {                                                                 // GpDate cell: yyyy-mm-dd or dd/mm/yyyy
        v = String(v || '').trim();
        var m = /^(\d{4})-(\d{1,2})-(\d{1,2})/.exec(v);
        if (m) return m[1] + '-' + ('0' + m[2]).slice(-2) + '-' + ('0' + m[3]).slice(-2);
        m = /^(\d{1,2})[\/.-](\d{1,2})[\/.-](\d{4})$/.exec(v);
        if (m) return m[3] + '-' + ('0' + m[2]).slice(-2) + '-' + ('0' + m[1]).slice(-2);
        return '';
    }
    function discRecalc(r) {                                                                // grd_CellUpdated body / F1 DiscountType body
        var amtNo = +r.ItemAmount || 0, rate = +r.DiscRate || 0, da = 0, t = +r.DiscountTypeId || 0;
        if (t !== 0) {
            if (t === 1) da = rate; else if (t === 2) da = amtNo * rate / 100;
            r.DiscAmount = da;
        } else r.DiscAmount = 0;
        if (amtNo > 0 && da > 0) r.ItemAmountWithDisc = amtNo - da; else if (amtNo > 0) r.ItemAmountWithDisc = amtNo; else r.ItemAmountWithDisc = 0;
    }
    function afterGridChange() { totalComm(); exchChanged(); }
    function makeGrids() {
        G.grd = SE.grid('grd', { footer: true, dec: 0, onDbl: function (r, i) { editRow(i); },
            onBtn: function (k, r, i) { if (k === 'Delete') deleteRow(i); else if (k === 'Edit') editRow(i); },
            onEdit: function (r, k, v) {                                                    // grd_CellUpdated
                if (k === 'GpDate') { var p = parseDate(v); if (p) r.GpDate = p; }
                else if (k === 'GpNo') r.GpNo = SE.toInt(v);
                else if (k === 'VehicleNo') r.VehicleNo = v;
                else if (k === 'WarehouseId') { r.WarehouseId = +v || 0; r.Warehouse = nameIn(S.L.warehouses, r.WarehouseId, 'WareHouseName'); }
                else if (k === 'JobLotId') { r.JobLotId = +v || 0; r.JobLot = nameIn(S.L.jobLots, r.JobLotId, 'JobLotDescription'); }
                else if (k === 'CityId') { r.CityId = +v || 0; r.CityName = nameIn(S.L.cities, r.CityId, 'CityName'); }
                else if (k === 'DiscountTypeId') {                                          // grd_KeyDown F1 on DiscountType
                    r.DiscountTypeId = +v || 0; r.DiscountType = nameIn(S.L.discTypes, r.DiscountTypeId, 'DiscountType');
                    if (r.DiscountTypeId > 0) discRecalc(r); else { r.DiscAmount = 0; r.ItemAmountWithDisc = 0; }
                }
                if (k === 'GpDate' || k === 'GpNo' || k === 'VehicleNo') discRecalc(r);
                G.grd.refresh(); afterGridChange();
            },
            cols: [
                { k: 'Delete', t: 'X', w: 24, btn: 'X' }, { k: 'Edit', t: 'Edit', w: 50, btn: 'Edit' },
                { k: 'Id', hide: true },
                { k: 'WarehouseId', t: 'Warehouse', w: 130, edit: true, list: function () { return S.L.warehouses; }, lk: 'Id', lt: 'WareHouseName' }, { k: 'Warehouse', hide: true },
                { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'ScheduleId', hide: true },
                { k: 'AttributeVarientId', hide: true }, { k: 'AttributeVarient', t: 'AttributeVarient', w: 100 },
                { k: 'VarientUnit', t: 'VarientUnit', w: 80, render: d3, sum: true, cls: 'num' },
                { k: 'JobLotId', t: 'JobLot', w: 120, edit: true, list: function () { return S.L.jobLots; }, lk: 'Id', lt: 'JobLotDescription' }, { k: 'JobLot', hide: true },
                { k: 'ItemQty', t: 'ItemQty', w: 80, render: d3, sum: true, cls: 'num' }, { k: 'BalQty', hide: true },
                { k: 'ItemWeight', t: 'ItemWeight', w: 80, render: d3, sum: true, cls: 'num' }, { k: 'NetWeight', t: 'NetWeight', w: 90, render: d3, sum: true, cls: 'num' }, { k: 'BalWeight', hide: true },
                { k: 'ItemPrice', t: 'ItemPrice', w: 80, render: rr, cls: 'num' }, { k: 'AddLessRate', t: 'AddLessRate', w: 80, render: rr, cls: 'num' }, { k: 'Rate', t: 'Rate', w: 80, render: rr, cls: 'num' },
                { k: 'ItemAmount', t: 'ItemAmount', w: 95, render: ra, sum: true, cls: 'num' },
                { k: 'DiscountTypeId', t: 'DiscountType', w: 90, edit: true, list: function () { return S.L.discTypes; }, lk: 'Id', lt: 'DiscountType' }, { k: 'DiscountType', hide: true },
                { k: 'DiscRate', t: 'DiscRate', w: 70, render: ra, cls: 'num' }, { k: 'DiscAmount', t: 'DiscAmount', w: 85, render: ra, sum: true, cls: 'num' },
                { k: 'ItemAmountWithDisc', t: 'ItemAmountWithDisc', w: 110, render: ra, sum: true, cls: 'num' },
                { k: 'FcyAmount', t: 'FcyAmount', w: 90, render: rf, sum: true, cls: 'num' },
                { k: 'GpDate', t: 'GpDate', w: 85, edit: true, f: 'sdate' }, { k: 'GpNo', t: 'GpNo', w: 60, edit: true }, { k: 'VehicleNo', t: 'VehicleNo', w: 90, edit: true },
                { k: 'CityId', t: 'CityName', w: 110, edit: true, list: function () { return S.L.cities; }, lk: 'Id', lt: 'CityName' }, { k: 'CityName', hide: true },
                { k: 'Commission', t: 'Commission', w: 80, render: ra, sum: true, cls: 'num' }, { k: 'ItemNetAmount', t: 'ItemNetAmount', w: 100, render: ra, sum: true, cls: 'num' },
                { k: 'IsFOC', t: 'IsFOC', w: 50, f: 'chk', render: function (v) { return '<input type="checkbox" data-foc' + (v ? ' checked' : '') + '>'; } }] });
        $('grd').addEventListener('click', function (e) {                                  // grd_Click: IsFOC is a check-box column
            if (e.target && e.target.hasAttribute && e.target.hasAttribute('data-foc')) {
                var tr = e.target.closest('tr[data-i]'), r = tr && G.grd.rows()[+tr.getAttribute('data-i')];
                if (r) { r.IsFOC = e.target.checked; G.grd.refresh(); exchChanged(); }
            }
        });
        G.foc = SPC.focRows($('grd'), G.grd);

        /* grdPartyAddLs (gridGLSettings) */
        G.jv = SE.grid('grdPartyAddLs', { footer: true, dec: 0,
            onBtn: function (k, r, i) { if (k === 'Delete') jvDelete(i); else if (k === 'Add') jvAdd(); },
            onDbl: function (r, i) { pickAccount(r); },
            onEdit: function (r, k, v) { jvCell(r, k, v); },
            cols: [{ k: 'Delete', t: 'X', w: 30, btn: 'X' }, { k: 'Add', t: '+', w: 30, btn: '+' }, { k: 'AccountId', hide: true }, { k: 'GlAccountId', hide: true },
                { k: 'AccountTitle', t: 'AccountTitle', w: 260 }, { k: 'Remarks', t: 'Remarks', w: 260, edit: true },
                { k: 'Percentage', t: 'Percentage', w: 90, edit: true, render: d3, sum: true, cls: 'num' }, { k: 'Qty', t: 'Qty', w: 90, edit: true, render: d3, sum: true, cls: 'num' },
                { k: 'Rate', t: 'Rate', w: 90, edit: true, render: rr, cls: 'num' },
                { k: 'Debit', t: 'Debit', w: 110, edit: true, render: ra, sum: true, cls: 'num' }, { k: 'Credit', t: 'Credit', w: 110, edit: true, render: ra, sum: true, cls: 'num' }] });
        jvReset();

        /* History (HistoryGridSettings / DetailGridSetting) */
        G.hist = SE.grid('grdHistory', { frozen: 3, dec: 0, onDbl: function (r) { getUpdate(+r.Id, false); },
            onBtn: function (k, r) { if (k === 'Edit') getUpdate(+r.Id, true); else if (k === 'Print') printSlip(+r.Id); else if (k === 'Voucher') printVoucher(+r.Id); },
            onLink: function (k, r) { if (k === 'NoOfAttachments') showAttachments(+r.Id); },
            onSel: histSelected,
            cols: [{ k: 'Edit', t: 'Edit', w: 50, btn: 'Edit' }, { k: 'Voucher', t: 'Voucher', w: 70, btn: 'Voucher' }, { k: 'Print', t: 'Customer Slip', w: 110, btn: 'Customer Slip' },
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
        G.hd = SE.grid('grdDetailHistory', { dec: 0, cols: [{ k: 'Id', hide: true }, { k: 'Warehouse', t: 'Warehouse', w: 120 }, { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 190 },
            { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'AttributeVarient', t: 'AttributeVarient', w: 100 }, { k: 'VarientUnit', t: 'VarientUnit', w: 80, render: d3, sum: true, cls: 'num' },
            { k: 'JobLot', t: 'JobLot', w: 110 }, { k: 'ItemQty', t: 'ItemQty', w: 80, render: d3, sum: true, cls: 'num' }, { k: 'ItemWeight', t: 'ItemWeight', w: 80, render: d3, sum: true, cls: 'num' },
            { k: 'NetWeight', t: 'NetWeight', w: 90, render: d3, sum: true, cls: 'num' }, { k: 'ItemPrice', t: 'ItemPrice', w: 80, render: rr, cls: 'num' },
            { k: 'AddLessRate', t: 'AddLessRate', w: 80, render: rr, cls: 'num' }, { k: 'Rate', t: 'NetRate', w: 80, render: rr, cls: 'num' }, { k: 'ItemAmount', t: 'ItemAmount', w: 95, render: ra, sum: true, cls: 'num' },
            { k: 'DiscountType', t: 'DiscountType', w: 90 }, { k: 'DiscRate', t: 'DiscRate', w: 70, render: ra, cls: 'num' }, { k: 'DiscAmount', t: 'DiscAmount', w: 85, render: ra, sum: true, cls: 'num' },
            { k: 'ItemAmountWithDisc', t: 'ItemAmountWithDisc', w: 110, render: ra, sum: true, cls: 'num' }, { k: 'FcyAmount', t: 'FcyAmount', w: 90, render: rf, sum: true, cls: 'num' },
            { k: 'GpDate', t: 'GpDate', w: 85, f: 'sdate' }, { k: 'GpNo', t: 'GpNo', w: 60 }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 }, { k: 'CityName', t: 'CityName', w: 100 },
            { k: 'Commission', t: 'Commission', w: 80, render: ra, sum: true, cls: 'num' }, { k: 'ItemNetAmount', t: 'ItemNetAmount', w: 100, render: ra, sum: true, cls: 'num' },
            { k: 'IsFOC', t: 'IsFOC', w: 50, f: 'chk' }] });
        G.focHd = SPC.focRows($('grdDetailHistory'), G.hd);
    }
    function setPlaces() { [G.grd, G.jv, G.hist, G.hd].forEach(function (g) { if (g) { g.spec.dec = F.amt; g.refresh(); } }); }

    /* ---- Party Add/Less grid ---- */
    function jvBlank() { return { AccountId: 0, GlAccountId: 0, AccountTitle: '', Remarks: '', Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0 }; }
    function jvReset() { G.jv.setRows([jvBlank()]); }                                         // AddRowInGLGrid
    function jvAdd() { G.jv.addRow(jvBlank()); billAmount(); }
    function jvDelete(i) {                                                                  // grdGLedger_ColumnButtonClick "Delete"
        SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
            if (!yes) return;
            G.jv.removeAt(i);
            if (G.jv.count() === 0) G.jv.addRow(jvBlank());
            billAmount();
        });
    }
    function jvCell(r, k, v) {                                                              // grdGLedger_UpdatingCell / _CellUpdated
        if (k === 'Remarks') { r.Remarks = v; G.jv.refresh(); return; }
        if (v !== '' && isNaN(Number(String(v).replace(/,/g, '')))) { msg('Please Type Only Numeric Value', 'Warning!'); return; }
        var n = SE.toNum(v);
        r[k] = n;
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
        G.jv.refresh(); billAmount();
    }
    /* grdGLedger_KeyDown F1 */
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
                G.jv.refresh(); billAmount();
            });
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ detail entry: validation / Add / Edit / Update / Cancel / Delete */
    function detailValid() {
        function stop(m, f) { msg(m); if (f) f(); return false; }
        if (!cb.wh.row() || iv(cb.wh) === 0) return stop('Warehouse Field is Required', function () { cb.wh.focus(); });
        if (!cb.item.row() || iv(cb.item) === 0) return stop('Item Name Field is Required', function () { cb.item.focus(); });
        if (!cb.varient.row() || iv(cb.varient) === 0) return stop('Attribute Varient Field is Required', function () { cb.varient.focus(); });
        if (T('txtQty') === '' || T('txtQty') === '0') return stop('Qty Field is Required', function () { $('txtQty').focus(); });
        if (T('txtItemWeight') === '' || T('txtItemWeight') === '0') return stop('Item Weight Field is Required', function () { $('txtItemWeight').focus(); });
        if (T('txtRate') === '' || T('txtRate') === '0') return stop('Rate Field is Required', function () { $('txtRate').focus(); });
        if ($('txtAmount').value === '' || $('txtAmount').value === '0') { msg('Please Check Item Amount'); return false; }
        if ($('txtTotalAmount').value === '' || $('txtTotalAmount').value === '0') { msg('Please Check Total Amount'); return false; }
        if (!cb.job.row() || iv(cb.job) === 0) return stop('Job/Lot Field is Required', function () { cb.job.focus(); });
        if (!cb.city.row() || iv(cb.city) === 0) return stop('City Field is Required', function () { cb.city.focus(); });
        return true;
    }
    function isDuplicate(skip) {
        var rows = G.grd.rows();
        for (var i = 0; i < rows.length; i++) {
            var r = rows[i];
            if (skip !== undefined && i === skip) continue;
            if (iv(cb.wh) === +r.WarehouseId && iv(cb.item) === +r.ItemId && iv(cb.varient) === +r.AttributeVarientId && iv(cb.job) === +r.JobLotId) return true;
        }
        return false;
    }
    var DUP = 'Duplicate items are not added to the grid. An item with the same warehouse, job lot, and AttributeVarient already exists';
    function entryRow() {
        var ir = cb.item.row() || {};
        return { WarehouseId: iv(cb.wh), Warehouse: cb.wh.text(), ItemId: iv(cb.item), ItemName: ir.ItemName || '', ItemCode: ir.ItemCode || '', ScheduleId: ir.ScheduleId || 0,
            AttributeVarientId: iv(cb.varient), AttributeVarient: cb.varient.text().trim(), VarientUnit: N('txtVarientEquivalent'), JobLotId: iv(cb.job), JobLot: cb.job.text(),
            ItemQty: N('txtQty'), BalQty: N('txtQty'), ItemWeight: N('txtItemWeight'), NetWeight: N('txtNetWeight'), BalWeight: N('txtNetWeight'), ItemPrice: N('txtItemPrice'),
            AddLessRate: N('txtAddLess'), Rate: N('txtRate'), ItemAmount: N('txtAmount'), DiscountTypeId: iv(cb.disc), DiscountType: cb.disc.text(), DiscRate: N('txtDiscountRate'),
            DiscAmount: N('txtDiscountAmount'), ItemAmountWithDisc: N('txtTotalAmount'), FcyAmount: 0, GpDate: $('txtgpdate').value || SE.today(), GpNo: SE.toInt($('txtgatepassno').value),
            VehicleNo: T('txtvehicleno'), CityId: iv(cb.city), CityName: cb.city.text(), Commission: 0, ItemNetAmount: 0, IsFOC: $('chkisFreeOfCost').checked };
    }
    function resetDetail() {                                                                // ResetDetail
        $('chkisFreeOfCost').checked = false;
        ['txtQty', 'txtItemWeight', 'txtNetWeight', 'txtItemPrice', 'txtAddLess', 'txtRate', 'txtAmount'].forEach(function (id) { $(id).value = ''; });
        if ($('ChkResetOnSave').checked) optionReset();
        cb.item.focus();
    }
    function optionReset() {                                                                // OptionResetFields
        cb.item.clear(); cb.job.clear(); cb.wh.clear(); $('txtgatepassno').value = ''; $('txtvehicleno').value = ''; cb.city.clear(); cb.item.focus();
    }
    function btnAdd() {
        if (isDuplicate()) { msg(DUP); return; }
        if (!detailValid()) return;
        G.grd.addRow(Object.assign({ Id: 0 }, entryRow()));
        totalComm(); exchChanged(); resetDetail();
    }
    /* grd_DoubleClick */
    function editRow(i) {
        var r = G.grd.rows()[i]; if (!r) return;
        S.updateIdx = i;
        cb.wh.setValue(+r.WarehouseId);
        cb.item.setValue(+r.ItemId);
        bindVarientKeep(+r.ItemId, +r.AttributeVarientId, r.VarientUnit);
        cb.job.setValue(+r.JobLotId);
        $('txtQty').value = f2(+r.ItemQty || 0); $('txtItemWeight').value = f2(+r.ItemWeight || 0); $('txtNetWeight').value = f2(+r.NetWeight || 0);
        $('txtItemPrice').value = fr(+r.ItemPrice || 0); $('txtAddLess').value = fr(+r.AddLessRate || 0); $('txtRate').value = fr(+r.Rate || 0);
        $('txtAmount').value = fa(+r.ItemAmount || 0);
        cb.disc.setValue(+r.DiscountTypeId || 0);
        $('txtDiscountRate').value = fr(+r.DiscRate || 0); $('txtDiscountAmount').value = fa(+r.DiscAmount || 0); $('txtTotalAmount').value = fa(+r.ItemAmountWithDisc || 0);
        $('txtgpdate').value = parseDate(r.GpDate) || SE.today(); $('txtgatepassno').value = String(r.GpNo == null ? '' : r.GpNo); $('txtvehicleno').value = r.VehicleNo || '';
        cb.city.setValue(+r.CityId || 0); $('chkisFreeOfCost').checked = !!r.IsFOC;
        show('btnAdd', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
        cb.item.focus(); stockLabel();
    }
    function bindVarientKeep(itemId, varId, unit) {
        return SE.api(API + '/varients' + SE.q({ itemId: itemId })).then(function (rows) {
            cb.varient.setData(rows || []); cb.varient.setValue(varId);
            $('txtVarientEquivalent').value = String(unit == null ? '' : unit);
        }).catch(fail);
    }
    function btnUpdateDetail() {
        if (!detailValid()) return;
        if (isDuplicate(S.updateIdx)) { msg(DUP); return; }
        var r = G.grd.rows()[S.updateIdx]; if (!r) return;
        var n = entryRow();
        ['WarehouseId', 'Warehouse', 'ItemId', 'ItemName', 'ItemCode', 'ScheduleId', 'AttributeVarientId', 'AttributeVarient', 'VarientUnit', 'JobLotId', 'JobLot', 'ItemQty', 'ItemWeight', 'NetWeight',
            'ItemPrice', 'AddLessRate', 'Rate', 'ItemAmount', 'DiscountTypeId', 'DiscountType', 'DiscRate', 'DiscAmount', 'ItemAmountWithDisc', 'GpDate', 'GpNo', 'VehicleNo', 'CityId', 'CityName', 'IsFOC']
            .forEach(function (k) { r[k] = n[k]; });
        show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        resetDetail(); totalComm(); exchChanged();
    }
    function btnCancelUpdateDetail() {                                                      // btnCancelUpdateDetial_Click
        show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false); S.updateIdx = -1;
    }
    /* grd_ColumnButtonClick "Delete" */
    function deleteRow(i) {
        var r = G.grd.rows()[i]; if (!r) return;
        function finish() { G.grd.removeAt(i); afterGridChange(); }
        if ((+r.Id || 0) > 0) {
            return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
                if (!yes) return;
                S.remove.push({ id: +r.Id });                                               // the server re-reads the stored row (ActionTypeId 3)
                finish();
            });
        }
        finish();
    }

    /* ------------------------------------------------------------------ Update Same Discount / manual bill total */
    function updateSameDiscount() {                                                         // btnUpdateDiscountInGrid_Click
        var id = iv(cb.disc), rate = N('txtDiscountRate');
        if (id === 0) { msg('DiscountType Required'); cb.disc.focus(); return; }
        if (rate === 0) { msg('Discount Rate Required'); $('txtDiscountRate').focus(); return; }
        if (G.grd.count() <= 0) { msg('Atlest one Record Required in Grid'); return; }
        if (id === 2 && rate >= 100) { msg('Discount Rate Cannot be greater than 99 when Discount Type is Percent'); return; }
        var nm = cb.disc.text();
        G.grd.rows().forEach(function (r) { r.DiscountTypeId = id; r.DiscountType = nm; r.DiscRate = rate; discRecalc(r); });
        G.grd.refresh(); totalComm(); exchChanged();
    }
    function manualTotal(fromInput) {                                                       // txtManualTotalBillAmount_TextChanged / _Leave
        var manual = N('txtManualTotalBillAmount');
        if (fromInput && !(manual > 0)) return;
        var rows = G.grd.rows(); if (!rows.length) return;
        var gia = 0; rows.forEach(function (r) { gia += +r.ItemAmount || 0; });
        $('txtItemAmountHeader').value = fa(gia);
        var j = jvSums(), actual = gia + (j.c - j.d);
        $('txtBillamountWithoutCommission').value = fa(actual);
        if (iv(cb.sup) === iv(cb.agent)) actual -= N('CommNetAmtHeader');
        if (manual === 0) {
            $('txtDiscountHeader').value = fa(0); $('txtBillAmountHeader').value = fa(actual); $('txtManualTotalBillAmount').value = fa(actual);
        } else if (manual > actual) {
            msg('Manual Bill Amount ' + manual + ' Can\'t be Greater Than Expected Bill Amount ' + actual);
            $('txtDiscountHeader').value = fa(0); $('txtBillAmountHeader').value = fa(actual); $('txtManualTotalBillAmount').value = fa(actual);
        } else {
            $('txtDiscountHeader').value = fa(actual - manual); $('txtBillAmountHeader').value = fa(manual);
        }
        proportionate();
        $('txtBillamountWithoutCommission').value = fa(N('txtBillamountWithoutCommission') - N('txtDiscountHeader'));
        var net = 0; rows.forEach(function (r) { net += +r.ItemNetAmount || 0; });
        $('txtItemNetAmountHeader').value = fa(net);
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
            r.ItemNetAmount = amt(amt(+r.ItemAmountWithDisc || 0) + amt(+r.Commission || 0));
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
                r.ItemNetAmount = amt(amt(+r.ItemAmountWithDisc || 0) + amt(+r.Commission || 0));
            });
        }
        G.grd.refresh();
        exchChanged();
    }

    /* ------------------------------------------------------------------ Reset / Refresh / Load */
    function reset() {                                                                      // btnNew_Click -> Reset
        S.files = []; S.removedAtt = []; S.existing = []; S.remove = []; S.id = 0; S.voucherHeadId = 0; S.approved = false; S.updateIdx = -1;
        $('chkisFreeOfCost').checked = false;
        cb.sup.clear(); cb.agent.clear(); cb.ctype.clear(); cb.cuom.clear(); cb.cdebit.clear(); cb.item.clear();
        ['txtSupplierReference', 'txtbillno', 'txtInvoiceQty', 'txtInvoiceWeight', 'txtExchangeRate', 'txtCommissionRemarks', 'txtcommrate', 'txtcommamount', 'txtremarks',
            'txtItemAmountHeader', 'txtDiscountHeader', 'txtItemNetAmountHeader', 'txtBillamountWithoutCommission', 'txtQty', 'txtItemPrice', 'txtAddLess', 'txtRate', 'txtAmount',
            'txtBillAmountHeader', 'txtManualTotalBillAmount'].forEach(function (id) { $(id).value = ''; });
        $('txtduedays').value = '0'; dueGen();
        G.grd.setRows([]); jvReset();
        if ($('ChkResetOnSave').checked) optionReset();
        show('btnSave', true); show('btnUpdate', false); show('btnDelete', false); show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        configDefault();
        billAmount();
        $('DocDate').focus();
        return SE.api(API + '/next-no').then(function (r) { $('txtdocno').value = r.nextNo > 0 ? String(r.nextNo) : $('txtdocno').value; }).catch(fail);
    }
    function refresh() {                                                                    // btnRefresh_Click
        return SE.api(API + '/lists' + SE.q({ docDate: $('DocDate').value })).then(function (d) { fillLists(d); S.accounts = null; configDefault(); }).catch(fail);
    }
    function applyInitial(d) {
        S.rights = d.rights || {}; S.fyStart = d.fyStart || ''; S.dateTypes = d.dateTypes || [];
        var s = d.settings || {};
        S.itemByCode = !!s.itemByCode; S.partyByCode = !!s.partyByCode; S.multi = !!s.multiCurrency; S.subsidiary = !!s.subsidiary;
        S.def = d.defaults || {};
        var f = d.fmt || {};
        F.amtRound = f.amountRound == null ? 0 : f.amountRound; F.amt = f.amount == null ? 0 : f.amount; F.rateRound = f.rateRound == null ? 2 : f.rateRound;
        F.rate = f.rate == null ? 2 : f.rate; F.fcy = f.fcy == null ? 0 : f.fcy;
        setPlaces();
        $('txtdocno').value = d.nextNo > 0 ? String(d.nextNo) : $('txtdocno').value;
        if (S.itemByCode) $('rdbtnItemCode').checked = true; else $('rdbtnItemName').checked = true;               // ItemSearchWithCode
        if (S.partyByCode) { $('RdPartyByCode').checked = true; $('RdCustomerHistorySearchByCode').checked = true; }
        else { $('RdPartyByName').checked = true; $('RdCustomerHistorySearchByName').checked = true; }              // PartySearchWithCode
        cb.dateType.setData(S.dateTypes);
        S.hc = d.history || S.hc;
        keep(cb.hCust, histCustRows()); keep(cb.hAgent, S.hc.agents);
    }
    function load() {
        makeCombos();
        tabs = SE.tabs('tabControl1', function (id) { if (id === 'tabPage2') tabHistory(); else $('DocDate').focus(); });
        tabs2 = SE.tabs('tabControl2', function () { });
        makeGrids();
        SE.upper($('txtvehicleno'));
        ['txtQty', 'txtItemWeight', 'txtRate', 'txtExchangeRate', 'txtManualTotalBillAmount', 'txtcommrate', 'txtDiscountRate', 'txtDiscountAmount'].forEach(function (id) {
            $(id).addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[\d.]/.test(e.key) && !e.ctrlKey) e.preventDefault(); });     // OnlytextdecimelFunction
        });
        $('txtAddLess').addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[\d.\-]/.test(e.key) && !e.ctrlKey) e.preventDefault(); });   // ...WithMinus
        SE.digitsOnly($('txtduedays')); SE.digitsOnly($('txtgatepassno')); SE.digitsOnly($('txtFromNoHistory')); SE.digitsOnly($('txtToDocNoHistory'));
        $('txtQty').addEventListener('input', onQty);
        $('txtRate').addEventListener('input', onRate);
        $('txtAddLess').addEventListener('input', netRate);
        $('txtDiscountRate').addEventListener('input', calcDisc); $('txtDiscountAmount').addEventListener('input', calcDisc);
        $('txtduedays').addEventListener('input', dueGen); $('DocDate').addEventListener('change', dueGen);
        $('txtcommrate').addEventListener('input', commChanged);
        $('txtExchangeRate').addEventListener('input', exchChanged);
        $('txtExchangeRate').addEventListener('change', function () { $('txtExchangeRate').value = fr(N('txtExchangeRate')); });   // txtExchangeRate_Leave
        $('txtManualTotalBillAmount').addEventListener('input', function () { manualTotal(true); });
        $('txtManualTotalBillAmount').addEventListener('change', function () { manualTotal(false); });
        $('RdPartyByName').addEventListener('change', partyRdb); $('RdPartyByCode').addEventListener('change', partyRdb);
        $('rdbtnItemName').addEventListener('change', itemRdb); $('rdbtnItemCode').addEventListener('change', itemRdb);
        $('RdCustomerHistorySearchByName').addEventListener('change', histRdb); $('RdCustomerHistorySearchByCode').addEventListener('change', histRdb);
        Promise.all([SE.api(API + '/initial'), SE.api(API + '/lists')]).then(function (a) {
            applyInitial(a[0]); fillLists(a[1]);
            var r = S.rights;
            $('btnSave').disabled = !r.save; $('btnUpdate').disabled = !r.update; $('btnDelete').disabled = !r.delete;
            $('btnCustomerSlip').disabled = !r.print; $('btnPrint').disabled = !r.print; $('ChkCustomerPrintslip').disabled = !r.print; $('ChkBok').disabled = !r.print;
            $('ChkCustomerPrintslip').checked = !!r.print; $('ChkBok').checked = !!r.print;
            multiCurrency();
            show('btnSave', true); show('btnUpdate', false); show('btnDelete', false); show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
            show('CommNetAmtHeader', false); show('label84', false);
            $('DocDate').value = SE.today(); dueGen();
            $('txtgpdate').value = SE.today();
            $('txtFromdateHistory').value = SE.today(); $('txtToDateHistory').value = SE.today();
            if (!iv(cb.dateType) && cb.dateType.rows().length) cb.dateType.setValue(cb.dateType.rows()[0].Id);
            configDefault();
            $('DocDate').focus();
            var rec = SE.param('record'); if (rec) getUpdate(+rec, true);
        }).catch(fail);
        wire();
    }
    function partyRdb() {                                                                   // RdPartyByName_CheckedChanged
        if (!S.suppliers.length) return;
        var id = iv(cb.sup);
        cb.sup.setData(supRows()); if (id > 0) cb.sup.setValue(id);
        cb.sup.focus();
    }
    function itemRdb() {                                                                    // ItemSearchWithCode switch
        if (!S.items.length) return;
        var id = iv(cb.item);
        cb.item.setData(itemRows()); if (id > 0) cb.item.setValue(id);
        cb.item.focus();
    }
    function histRdb() {                                                                    // RdCustomerHistorySearchByName_CheckedChanged
        if (!S.hc.customers.length) return;
        var id = iv(cb.hCust);
        cb.hCust.setData(histCustRows()); if (id > 0) cb.hCust.setValue(id);
        cb.hCust.focus();
    }

    /* ------------------------------------------------------------------ Insert / btnSave / btnUpdate */
    function lineOf(r) {
        return { id: +r.Id || 0, warehouseId: +r.WarehouseId || 0, itemId: +r.ItemId || 0, scheduleId: SE.toInt(r.ScheduleId), attributeVarientId: +r.AttributeVarientId || 0,
            jobLotId: +r.JobLotId || 0, gpNo: SE.toInt(r.GpNo), cityId: +r.CityId || 0, discountTypeId: +r.DiscountTypeId || 0, varientUnit: SE.toNum(r.VarientUnit), qty: SE.toNum(r.ItemQty),
            itemWeight: SE.toNum(r.ItemWeight), netWeight: SE.toNum(r.NetWeight), itemPrice: SE.toNum(r.ItemPrice), addLessRate: SE.toNum(r.AddLessRate), rate: SE.toNum(r.Rate),
            discRate: SE.toNum(r.DiscRate), discAmount: SE.toNum(r.DiscAmount), gpDate: parseDate(r.GpDate), vehicleNo: r.VehicleNo == null ? '' : String(r.VehicleNo), isFoc: !!r.IsFOC };
    }
    function jvOf(r) {
        return { accountId: +r.AccountId || 0, glAccountId: +r.GlAccountId || 0, remarks: r.Remarks == null ? '' : String(r.Remarks), percentage: +r.Percentage || 0, qty: +r.Qty || 0,
            rate: +r.Rate || 0, debit: +r.Debit || 0, credit: +r.Credit || 0 };
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
            if (N('txtcommamount') > 0) {
                var stop = function (m, f) { return msg(m).then(function () { if (f) f(); }); };
                if (iv(cb.agent) === 0) return stop('Please Select Commission Agent Account First', function () { cb.agent.focus(); });
                if (iv(cb.salesman) === 0) return stop('Please Select Ref Sales Man First', function () { cb.salesman.focus(); });
                if (iv(cb.cdebit) === 0) return stop('CommissionCreditAccount Account field Required', function () { cb.cdebit.focus(); });
            }
            return SE.ask(S.id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
                if (!yes) return;
                var jv = G.jv.rows(), bad = -1;
                jv.forEach(function (r, i) { if (bad < 0 && ((+r.Credit || 0) > 0 || (+r.Debit || 0) > 0) && !(+r.AccountId > 0) && !(+r.GlAccountId > 0)) bad = i; });
                if (bad >= 0) return msg('Please Select an Account Party Addless Grid in Row no: ' + (bad + 1));
                var body = { id: S.id, docDate: $('DocDate').value, docNo: T('txtdocno'), referenceNo: $('txtSupplierReference').value, manualBillNo: $('txtbillno').value, remarks: $('txtremarks').value,
                    dueDays: T('txtduedays'), refPartyName: $('txtRefPartyName').value, refPartyAddress: $('txtRefPartyAddress').value, refPartyCellNo: $('txtRefPartyCellNo').value,
                    supplierCustomerId: iv(cb.sup), visitedById: iv(cb.visited), paymentTermId: iv(cb.term), deliveryTermId: iv(cb.dterm), commissionAgentId: iv(cb.agent), refSalesManId: iv(cb.salesman),
                    commissionDebitAccountId: iv(cb.cdebit), currencyId: iv(cb.cur), commissionType: cb.ctype.text(), commissionUom: cb.cuom.text(), commissionRemarks: $('txtCommissionRemarks').value,
                    commissionRate: N('txtcommrate'), commissionAmount: N('txtcommamount'), exchangeRate: N('txtExchangeRate'),
                    rows: G.grd.rows().map(lineOf), removed: S.remove, journals: jv.map(jvOf),
                    attachments: { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removedAtt } };
                return SE.api(API, { method: 'POST', body: body }).then(function (r) {
                    return msg(r.message).then(function () {
                        var vch = $('ChkBok').checked, slip = $('ChkCustomerPrintslip').checked;
                        return reset().then(function () { if (vch) printVoucher(r.id); if (slip) printSlip(r.id); });
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
            $('txtduedays').value = cs(h, 'DueDays'); $('txtduedays').disabled = iv(cb.term) === 1; dueGen();
            setByText(cb.dterm, cs(h, 'DeliveryTerm'));
            $('txtRefPartyName').value = cs(h, 'ReferencPartyName'); $('txtRefPartyAddress').value = cs(h, 'ReferencPartyAddress'); $('txtRefPartyCellNo').value = cs(h, 'ReferencPartyCellNo');
            cb.agent.setValue(cn(h, 'CommissionAgentId')); cb.salesman.setValue(cn(h, 'RefSalesManId'));
            setByText(cb.ctype, cs(h, 'CommissionType'));
            $('txtcommrate').value = fr(cn(h, 'CommissionRate'));
            var uom = cn(h, 'CommissionUom'), hit = null;
            cb.cuom.rows().forEach(function (r) { if (SE.toNum(r.UOM) === uom) hit = r; });
            if (hit) cb.cuom.setValue(hit.Id); else cb.cuom.clear();
            $('txtcommamount').value = fa(cn(h, 'CommissionAmount')); $('txtCommissionRemarks').value = cs(h, 'CommissionRemarks');
            $('txtInvoiceQty').value = cs(h, 'InvoiceQty'); $('txtInvoiceWeight').value = cs(h, 'InvoiceWeight');
            cb.cur.setValue(cn(h, 'CurrencyId'));
            $('txtFcyAmount').value = ff(cn(h, 'FcyAmount'));
            cb.cdebit.setValue(cn(h, 'CommissionDebitAcId'));
            $('txtItemAmountHeader').value = fa(cn(h, 'ItemAmountHeader')); $('txtDiscountHeader').value = fa(cn(h, 'DiscountAmountHeader'));
            $('txtItemNetAmountHeader').value = fa(cn(h, 'ItemNetAmountHeader'));
            $('txtBillAmountHeader').value = fa(cn(h, 'BillAmount')); $('txtManualTotalBillAmount').value = fa(cn(h, 'BillAmount'));
            cb.branch.setValue(cn(h, 'BranchesId')); cb.proj.setValue(cn(h, 'ProjectsId'));
            S.approved = !!ci(h, 'IsApproved') && String(ci(h, 'IsApproved')).toLowerCase() !== 'false' && ci(h, 'IsApproved') !== 0;
            if (cn(h, 'CurrencyId') > 0) $('txtExchangeRate').value = String(cn(h, 'ExchangeRate')); else configDefault();
            S.voucherHeadId = +d.voucherHeadId || 0;
            G.grd.setRows(lines.map(function (l) {
                return { Id: cn(l, 'Id'), WarehouseId: cn(l, 'WarehouseId'), Warehouse: cs(l, 'WareHouseName'), ItemId: cn(l, 'ItemId'), ItemName: cs(l, 'ItemName'), ItemCode: cs(l, 'ItemCode'),
                    ScheduleId: cn(l, 'ScheduleId'), AttributeVarientId: cn(l, 'ItemAttributeVarientId'), AttributeVarient: cs(l, 'ItemAttributeVarient'), VarientUnit: cn(l, 'VarientEquivalent'),
                    JobLotId: cn(l, 'JobLotId'), JobLot: cs(l, 'JobLotDescription'), ItemQty: cn(l, 'ItemQty'), BalQty: cn(l, 'ItemQty'), ItemWeight: cn(l, 'ItemWeight'), NetWeight: cn(l, 'ItemNetWeight'),
                    BalWeight: cn(l, 'ItemNetWeight'), ItemPrice: cn(l, 'ItemRateWithOutAddLess'), AddLessRate: cn(l, 'RateAddLess'), Rate: cn(l, 'ItemRate'), ItemAmount: cn(l, 'ItemAmount'),
                    DiscountTypeId: cn(l, 'ItemDiscountTypeId'), DiscountType: cs(l, 'ItemDiscountType'), DiscRate: cn(l, 'ItemDiscountRate'), DiscAmount: cn(l, 'ItemDiscountAmount'),
                    ItemAmountWithDisc: cn(l, 'ItemAmountWithDisc'), FcyAmount: cn(l, 'FcyAmount'), GpDate: parseDate(cs(l, 'GpDate')), GpNo: cn(l, 'GpNo'), VehicleNo: cs(l, 'VehicleNo'),
                    CityId: cn(l, 'CityId'), CityName: cs(l, 'CityName'), Commission: cn(l, 'CommissionAmount'), ItemNetAmount: cn(l, 'ItemNetAmount'), IsFOC: !!ci(l, 'IsFOC') && ci(l, 'IsFOC') !== 0 };
            }));
            var jr = (d.journals || []).map(function (j) {
                var acc = cn(j, 'TransporterSupCustId'), gl = cn(j, 'ChartofAccountId');
                return { AccountId: acc, GlAccountId: gl, AccountTitle: acc > 0 ? cs(j, 'SupplierCustomer') : (gl > 0 ? cs(j, 'AccountTitle') : ''), Remarks: cs(j, 'JvRemarks'), Percentage: cn(j, 'JvPrcnt'),
                    Qty: cn(j, 'JvQty'), Rate: cn(j, 'JvRate'), Debit: cn(j, 'JvDebit'), Credit: cn(j, 'JvCredit') };
            });
            G.jv.setRows(jr.length ? jr : [jvBlank()]);
            S.existing = d.attachments || []; S.files = []; S.removedAtt = [];
            show('btnSave', false); show('btnUpdate', true); show('btnDelete', true);
            exchChanged();
        }).catch(fail);
    }
    function supplierRefresh() {                                                            // comsupplier_ValueChanged side effects without BillAmount
        var r = cb.sup.row();
        if (!r || iv(cb.sup) <= 0) return;
        S.supGl = cn(r, 'GlAccountId'); $('txtSupplierGLId').value = String(S.supGl);
        keep(cb.cdebit, commDebitRows());
    }

    /* btnDelete_Click */
    function del() {
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
    function histCombos() {                                                                 // HistoryComboBind
        return SE.api(API + '/history-combos').then(function (h) {
            S.hc = h; keep(cb.hCust, histCustRows()); keep(cb.hAgent, h.agents);
        }).catch(fail);
    }
    function showHistory() {                                                                // FillHistory
        var q = { fromDate: $('txtFromdateHistory').value, toDate: $('txtToDateHistory').value, fromNo: SE.toInt($('txtFromNoHistory').value), toNo: SE.toInt($('txtToDocNoHistory').value),
            customerId: iv(cb.hCust), agentId: iv(cb.hAgent) };
        return SE.api(API + '/history' + SE.q(q)).then(function (rows) {
            S.hist = rows;
            G.hist.setRows(rows);
            if (!rows.length) G.hd.setRows([]);
        }).catch(fail);
    }
    function resetHistory() {                                                               // Resethistory
        cb.dateType.setValue(3); dateTypeChanged();
        $('txtFromNoHistory').value = ''; $('txtToDocNoHistory').value = '';
        cb.hCust.clear(); cb.hAgent.clear(); cb.dateType.focus();
    }
    /* grdHistory_SelectionChanged -> GetDetailGrdByHeadId */
    function histSelected(item) {
        if (!item) { G.hd.setRows([]); return; }
        SE.api(API + '/' + item.Id, { quiet: true }).then(function (d) {
            G.hd.setRows((d.lines || []).map(function (l) {
                return { Id: cn(l, 'Id'), Warehouse: cs(l, 'WareHouseName'), ItemId: cn(l, 'ItemId'), ItemName: cs(l, 'ItemName'), ItemCode: cs(l, 'ItemCode'), AttributeVarient: cs(l, 'ItemAttributeVarient'),
                    VarientUnit: cn(l, 'VarientEquivalent'), JobLot: cs(l, 'JobLotDescription'), ItemQty: cn(l, 'ItemQty'), ItemWeight: cn(l, 'ItemWeight'), NetWeight: cn(l, 'ItemNetWeight'),
                    ItemPrice: cn(l, 'ItemRateWithOutAddLess'), AddLessRate: cn(l, 'RateAddLess'), Rate: cn(l, 'ItemRate'), ItemAmount: cn(l, 'ItemAmount'), DiscountType: cs(l, 'ItemDiscountType'),
                    DiscRate: cn(l, 'ItemDiscountRate'), DiscAmount: cn(l, 'ItemDiscountAmount'), ItemAmountWithDisc: cn(l, 'ItemAmountWithDisc'), FcyAmount: cn(l, 'FcyAmount'), GpDate: cs(l, 'GpDate'),
                    GpNo: cn(l, 'GpNo'), VehicleNo: cs(l, 'VehicleNo'), CityName: cs(l, 'CityName'), Commission: cn(l, 'CommissionAmount'), ItemNetAmount: cn(l, 'ItemNetAmount'),
                    IsFOC: !!ci(l, 'IsFOC') && ci(l, 'IsFOC') !== 0 };
            }));
        }).catch(function () { G.hd.setRows([]); });
    }

    /* ------------------------------------------------------------------ print / attachments */
    function printSlip(id) {                                                                // CommonServices.SaleInvoiceReturnConcreteCustomerSlip
        if (!(id > 0)) return msg('No Record Found For Display');
        return SPC.openPdf(PRINT + SE.q({ id: id }));
    }
    function printVoucher(id) {                                                             // CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 1862)
        if (!(id > 0)) return msg('No Record Found For Display');
        return SPC.openPdf(VPRINT + SE.q({ id: id }));
    }
    function showAttachments(id) {
        return SE.api(API + '/' + id + '/attachments').then(function (rows) { SPC.attachments(S, API, id, true, rows); }).catch(fail);
    }
    function openAttachments() { SPC.attachments(S, API, S.id, false); }                     // btnAttachment_Click -> AT.Show()

    /* ------------------------------------------------------------------ shortcut keys (InvfrmPurchasedirectInvoice_KeyDown_1 / MakeShortCutKeys) */
    var SHORT = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
        ['Ctrl+P', 'For Print'], ['Alt+1', 'For Print Slip 1862'], ['Alt+2', 'For Print Voucher 103'], ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'],
        ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+D', 'For Adding an row in Focused Grid'], ['Ctrl+Delete', 'For Deleting an row of Focused Grid'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On WareHouse in Detail Box'], ['Ctrl+ArrowRight', 'For Change Focus from one Grid To another Grid'],
        ["Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function onKey(e) {
        var t = e.target, k = e.key.toLowerCase();
        if (e.key === 'Enter' && t && (t.tagName === 'INPUT' || t.tagName === 'SELECT') && t.type !== 'checkbox' && t.type !== 'radio' && t.type !== 'button' && !t.classList.contains('cell')) {
            var f = Array.prototype.filter.call(document.querySelectorAll('.dform input:not([type=hidden]):not([disabled]), .dform select:not([disabled]), .dform .dtcombo-input'), function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(t); if (i >= 0 && f[i + 1]) { e.preventDefault(); f[i + 1].focus(); }
        }
        if (e.key === 'F1' && document.activeElement === $('grdPartyAddLs') || (e.key === 'F1' && $('grdPartyAddLs').contains(document.activeElement))) {
            var cur = G.jv.cur(); if (cur && /^(AccountTitle|AccountId|GlAccountId)$/.test(G.jv.curKey() || '')) { e.preventDefault(); pickAccount(cur); }
        }
        if (e.ctrlKey && k === 't') { e.preventDefault(); if (tabs.index() === 1) { tabs.select('tabPage1'); cb.sup.focus(); } else { tabs.select('tabPage2'); tabHistory(); $('grdHistory').focus(); } return; }
        if (tabs.index() === 0) {
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh(); }
            else if (e.ctrlKey && e.shiftKey && e.key === 'Delete') { e.preventDefault(); if (isShown('btnDelete') && !$('btnDelete').disabled) del(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); if (isShown('btnSave') && !$('btnSave').disabled) btnSave(); }
            else if (e.ctrlKey && k === 'u') { e.preventDefault(); if (isShown('btnUpdate') && !$('btnUpdate').disabled) btnUpdate(); }
            else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); cb.sup.focus(); }
            else if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); openAttachments(); }
            else if (e.ctrlKey && k === 'p') { e.preventDefault(); printSlip(S.id); }
            else if (e.altKey && e.key === '1') { e.preventDefault(); printSlip(S.id); }
            else if (e.altKey && e.key === '2') { e.preventDefault(); printVoucher(S.id); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { $('grd').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowUp') { cb.wh.focus(); }
            else if (e.ctrlKey && e.key === 'ArrowRight') {
                if (document.activeElement === $('grd')) { tabs2.select('tabPage5'); $('grdPartyAddLs').focus(); } else if (document.activeElement === $('grdPartyAddLs')) { tabs2.select('tabPage3'); $('grd').focus(); }
            }
            else if (e.ctrlKey && e.key === 'Delete') {
                if (document.activeElement === $('grd')) { var gi = G.grd.curIndex(); if (gi >= 0) deleteRow(gi); }
                else if (document.activeElement === $('grdPartyAddLs')) { var ji = G.jv.curIndex(); if (ji >= 0) jvDelete(ji); }
            }
            else if (e.ctrlKey && k === 'd') { if (document.activeElement === $('grdPartyAddLs')) { e.preventDefault(); jvAdd(); } }
        } else {
            if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); $('grdHistory').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { $('grdDetailHistory').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowUp') { $('grdHistory').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowRight') { if (document.activeElement === $('grdDetailHistory')) $('grdHistory').focus(); else $('grdDetailHistory').focus(); }
        }
    }
    function wire() {
        $('btnNew').onclick = reset; $('btnRefresh').onclick = refresh; $('btnSave').onclick = btnSave; $('btnUpdate').onclick = btnUpdate; $('btnDelete').onclick = del;
        $('btnAttachment').onclick = openAttachments; $('btnCustomerSlip').onclick = function () { printSlip(S.id); }; $('btnPrint').onclick = function () { printVoucher(S.id); };
        $('btnshortcutkeys').onclick = function () { SE.shortcuts(SHORT); };
        $('btnDefinePlant').onclick = function () { window.open('/party-processing/reference-parties', '_blank'); };                // DefineReferenceParties
        $('btnAdd').onclick = btnAdd; $('btnUpdateDetail').onclick = btnUpdateDetail; $('btnCancelUpdateDetial').onclick = btnCancelUpdateDetail;
        $('btnUpdateDiscountInGrid').onclick = updateSameDiscount;
        $('BtnNewHistory').onclick = resetHistory; $('BtnRefreshHistory').onclick = histCombos; $('btnshow').onclick = showHistory;
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
        var hook = false;
        document.addEventListener('keydown', function (e) { if (e.ctrlKey && e.altKey && !hook) { hook = true; SE.shortcuts(SHORT); setTimeout(function () { hook = false; }, 400); } });
    }

    document.addEventListener('DOMContentLoaded', load);
})();
