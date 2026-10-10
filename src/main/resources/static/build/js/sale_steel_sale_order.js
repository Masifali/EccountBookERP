/*
 * Screen 542  SaleOrder_St  (Architecture.WinApp.Steel.Sale.SaleOrder_St, document type 1505)
 * Page script. Desktop methods are named in the comments (SaleOrder_St.cs). Server: /sale/steel/sale-order/api
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/steel/sale-order/api', fail = SS.fail, DOC = 1505;
    var S = { id: 0, approved: false, rights: {}, fmt: { amountRound: 0, amount: 0, rate: 2, fcy: 0, fcyRound: 0 }, def: {}, remove: [], updateIdx: -1, detailId: 0,
              files: [], removed: [], existing: [], items: [], histSeq: 0, lists: {} };
    var cb = {}, G = {}, tabs, footObs;
    var COLS = ['Id', 'ItemId', 'ItemName', 'JobLotId', 'JobLot', 'PackingTypeId', 'PackingType', 'PackUomId', 'PackUom', 'ItemQty', 'Weight', 'ItemRate', 'RateUomId', 'RateUom',
                'ItemAmount', 'CityId', 'CityName', 'ExchangeRate', 'FcyAmount', 'Remarks'];            // the DataTable `table` of PurchsaeOrder_Load, in column order

    function field(id) { return ($(id).value || '').trim(); }
    function tn(v) { return SE.toNum(v); }                                               // Conversion.ToDecimal / ToDouble of a (formatted) text
    function ti(v) { var n = Number(String(v == null ? '' : v).replace(/,/g, '')); return isFinite(n) ? Math.trunc(n) : 0; }
    function msg(t, cap) { return SE.alert(t, cap); }
    function show(id, on) { var b = $(id); if (b) b.style.display = on ? '' : 'none'; }
    function visible(id) { return SS.visible(id); }
    function onBlur(c, fn) { var x = c.el.__dtcombo; if (x && x.input) x.input.addEventListener('blur', fn); }
    function vid(c) { return +c.value() || 0; }
    function errMsg(e) { return msg(e && e.message ? e.message : String(e), 'Error Message'); }
    /* Math.Round(decimal, d): banker's rounding */
    function bround(v, d) {
        var p = Math.pow(10, d), m = v * p, f = Math.floor(m), diff = m - f, r;
        if (Math.abs(diff - 0.5) < 1e-9) r = (f % 2 === 0) ? f : f + 1; else r = Math.round(m);
        return r / p;
    }
    function fmtSingle(v) { return SE.num(v, S.fmt.amount, S.fmt.amount); }              // ToString(clsGlobalVariables.stringFormatsingle)
    function fmtFcy(v) { return SE.num(v, S.fmt.fcy, S.fmt.fcy); }                       // stringFormatsingleForFcy
    function fmtRate(v) { return SE.num(v, S.fmt.rate, S.fmt.rate); }                    // DecimalRateFormate
    function f2(v) { return SE.num(v, 2, 0); }                                           // "#,##0.##"
    function iso(d) { return d.getFullYear() + '-' + SE.pad2(d.getMonth() + 1) + '-' + SE.pad2(d.getDate()); }
    function parseIso(s) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(s || ''); return m ? new Date(+m[1], +m[2] - 1, +m[3]) : null; }
    function addDaysTo(s, n) { var d = parseIso(s) || new Date(); d.setDate(d.getDate() + n); return iso(d); }
    function setCombo(c, id) { var k = c.el.getAttribute('data-vk') || 'Id', hit = c.rows().some(function (r) { return String(r[k]) === String(id); }); if (hit) c.setValue(id); else c.clear(); }
    function setRow(c, i) { var r = c.rows()[i]; if (!r) return false; c.setValue(r[c.el.getAttribute('data-vk') || 'Id']); return true; }

    /* ------------------------------------------------------------------ combos */
    function makeCombos() {
        cb.cat = XCombo('CmbOrderCategory', { columns: [{ key: 'OrderCategoryName', caption: 'OrderCategoryName' }], textKey: 'OrderCategoryName', onLeave: genCategoryNo });
        cb.cust = XCombo('CmbCustomerName', { columns: [{ key: 'CompanyName', caption: 'Customer Name' }], textKey: 'CompanyName', popupWidth: 420 });
        cb.agent = XCombo('CmbCommAgent', { columns: [{ key: 'CompanyName', caption: 'SalesMan Name' }], textKey: 'CompanyName', popupWidth: 420, onLeave: agentLeave });
        cb.dterm = XCombo('CmbDeliveryTerm', { columns: [{ key: 'DeliveryTerm', caption: 'Delivery Term' }], textKey: 'DeliveryTerm' });
        cb.term = XCombo('CmbPaymentTerm', { columns: [{ key: 'TermsDescription', caption: 'Payment Term' }], textKey: 'TermsDescription', onSelect: termChanged });
        cb.status = XCombo('CmbOrderStatus', { columns: [{ key: 'Status', caption: 'Order Status' }], textKey: 'Status' });
        cb.cur = XCombo('cmbCurrency', { columns: [{ key: 'CurrencyCode', caption: 'Currency' }], textKey: 'CurrencyCode' });
        cb.ctype = XCombo('CmbCommType', { columns: [{ key: 'CommissionType', caption: 'CommissionType' }], textKey: 'CommissionType', onSelect: commChanged });
        cb.cuom = XCombo('CmbCommRateUom', { columns: [{ key: 'UOM', caption: 'UOM' }], textKey: 'UOM', onSelect: commChanged });
        cb.item = XCombo('CmbItemName', { columns: [{ key: 'ItemName', caption: 'Item Name' }], textKey: 'ItemName', popupWidth: 520, onSelect: itemChanged });
        cb.job = XCombo('CmbJobLot', { columns: [{ key: 'JobLotDescription', caption: 'JobLot' }], textKey: 'JobLotDescription' });
        cb.pack = XCombo('cmbPackType', { columns: [{ key: 'PackTypeDesc', caption: 'Packing Type' }], textKey: 'PackTypeDesc' });
        cb.uom = XCombo('CmbPackUom', { columns: [{ key: 'UOMCode', caption: 'PackUOM' }, { key: 'Equivalent', caption: 'Equivalent' }], textKey: 'UOMCode', onSelect: packUomChanged });
        cb.rate = XCombo('CmbRateUom', { columns: [{ key: 'UOMCode', caption: 'RateUOM' }, { key: 'Equivalent', caption: 'Equivalent' }], textKey: 'UOMCode', onSelect: rateUomChanged });
        cb.city = XCombo('CmbCityName', { columns: [{ key: 'CityName', caption: 'Description' }], textKey: 'CityName' });
        cb.hcust = XCombo('CmbCustomerHistory', { columns: [{ key: 'Customer', caption: 'Party' }], textKey: 'Customer', popupWidth: 420 });
        onBlur(cb.uom, packUomChanged); onBlur(cb.rate, rateUomChanged); onBlur(cb.ctype, commChanged); onBlur(cb.cuom, commChanged); onBlur(cb.item, itemChanged);
        onBlur(cb.term, termChanged);
    }
    function bindLists(d, load) {
        var L = S.lists;
        if (d.categories) { L.cat = d.categories; cb.cat.setData(d.categories); setRow(cb.cat, 0); }                          // OrderCatagoryfill: Rows[0]
        if (d.customers) { L.cust = d.customers; cb.cust.setData(d.customers); cb.agent.setData(d.customers); if (!load) { cb.cust.clear(); cb.agent.clear(); } }
        if (d.deliveryTerms) { cb.dterm.setData(d.deliveryTerms); setRow(cb.dterm, 0); }                                     // DeliveryTerms: Rows[0]
        if (d.paymentTerms) { cb.term.setData(d.paymentTerms); setRow(cb.term, 1); termChanged(); }                           // PaymentTerms: Rows[1]
        if (d.currencies) { cb.cur.setData(d.currencies); if (!load) cb.cur.clear(); }
        if (d.items) { S.items = d.items; cb.item.setData(d.items); if (!load) cb.item.clear(); }
        if (d.jobLots) { cb.job.setData(d.jobLots); if (!load) cb.job.clear(); }
        if (d.packingTypes) { cb.pack.setData(d.packingTypes); if (!load) cb.pack.clear(); }
        if (d.cities) { cb.city.setData(d.cities); setRow(cb.city, 0); }                                                     // cmbCityFill: Rows[0]
    }

    /* GenerateOrderCategoryNo :843 (combordercat_Leave) */
    function genCategoryNo() {
        return SE.api(API + '/category-no' + SE.q({ categoryId: vid(cb.cat) })).then(function (d) { if (d.catSrNo > 0) $('txtcatsr').value = String(d.catSrNo); }).catch(errMsg);
    }
    /* combsalesman_Leave :934 */
    function agentLeave() {
        SS.dis(cb.ctype, false); $('txtCommRate').disabled = false; SS.dis(cb.cuom, false); $('txtCommAmount').disabled = false; $('txtCommRemarks').disabled = false;
        cb.ctype.focus();
        try { calcTotalInformation(); } catch (e) { errMsg(e); }
    }
    /* CmbPaymentTerm_ValueChanged :2609 */
    function termChanged() {
        try {
            if (cb.term.text() === 'Cash') { $('txtduedays').disabled = true; $('txtduedays').value = ''; dueDaysChanged(); $('duedate').value = $('DocDate').value; }
            else $('txtduedays').disabled = false;
        } catch (e) { errMsg(e); }
    }
    /* txtduedays_TextChanged :2360 */
    function dueDaysChanged() {
        try {
            var t = field('txtduedays');
            $('duedate').value = t !== '' ? addDaysTo(SE.today(), tn(t)) : SE.today();
        } catch (e) { errMsg(e); }
    }
    /* txtdeliverydays_TextChanged :2457 / CalculateExpiryDate :2469 */
    function calcExpiry() {
        if (ti($('txtDeliveryDays').value) === 0) $('datExpiryDate').value = $('deliverystartdate').value;
        else $('datExpiryDate').value = addDaysTo($('deliverystartdate').value, tn($('txtDeliveryDays').value));
    }
    function deliveryDaysChanged() { try { calcExpiry(); } catch (e) { errMsg(e); } }
    /* deliverystartdate_ValueChanged :2666 */
    function startDateChanged() {
        try {
            var s = parseIso($('deliverystartdate').value), d = parseIso($('DocDate').value);
            if (s && d && s < d) { $('deliverystartdate').value = $('DocDate').value; throw new Error("Delivery Start Date Can't Be Less Than Doc Date"); }
            calcExpiry();
        } catch (e) { errMsg(e); }
    }

    /* bindRateUomAndItemPackUom :999 (CmbItemName_ValueChanged :2063) */
    function itemChanged() {
        var itemId = vid(cb.item), pk = cb.uom.text(), rt = cb.rate.text();
        if (S.lastItem === itemId && S.lastItemSet) return Promise.resolve();
        S.lastItem = itemId; S.lastItemSet = true;
        cb.uom.clear(); cb.rate.clear();
        var p = itemId ? SE.api(API + '/uoms' + SE.q({ itemId: itemId })) : Promise.resolve([]);
        return p.then(function (rows) {
            if (rows.length > 0) {
                cb.uom.setData(rows); cb.rate.setData(rows);
                var a = rows.filter(function (r) { return r.UOMCode === pk; })[0], b = rows.filter(function (r) { return r.UOMCode === rt; })[0];
                if (a) cb.uom.setValue(a.Id); else cb.uom.clear();
                if (b) cb.rate.setValue(b.Id); else cb.rate.clear();
            } else { cb.uom.setData([]); cb.rate.setData([]); }
        }).catch(errMsg);
    }
    function eqOf(c) { var r = c.row(); if (!r) throw new Error('Object reference not set to an instance of an object.'); return tn(r.Equivalent); }

    /* CalculateWeight :1855 */
    function calcWeight() {
        if (cb.uom.text() !== '' && $('txtItemQty').value !== '') {
            var w = bround(eqOf(cb.uom) * tn($('txtItemQty').value), 2);
            $('txtweight').value = String(w);
        } else $('txtweight').value = '0';
    }
    /* TotalAmount :1891 */
    function totalAmount() {
        if ($('txtweight').value !== '' && cb.rate.text() !== '' && $('txtRate').value !== '') {
            var eq = eqOf(cb.rate);
            if (eq === 0) throw new Error('Attempted to divide by zero.');
            $('txtamount').value = String(bround(tn($('txtweight').value) / eq * tn($('txtRate').value), S.fmt.amountRound));
        } else $('txtamount').value = '0';
    }
    function gridTotal(k) { return G.grd.rows().reduce(function (a, r) { return a + tn(r[k]); }, 0); }
    /* TotalCommissionAmount :1918 */
    function totalCommission() {
        if ($('txtCommRate').value !== '') {
            var t = cb.ctype.text(), rate = tn($('txtCommRate').value.trim());
            if (t === 'Flat') $('txtCommAmount').value = fmtSingle(bround(rate, S.fmt.amountRound));
            else if (t === 'Percent') {
                var tot = gridTotal('ItemAmount');
                if (tot > 0) $('txtCommAmount').value = fmtSingle(bround(tot * rate / 100, S.fmt.amountRound));
            } else if (t === 'Comm Weight') {
                var w = gridTotal('Weight');
                if (w > 0) {
                    var u = tn(cb.cuom.text().trim());
                    if (u === 0) throw new Error('Attempted to divide by zero.');
                    $('txtCommAmount').value = fmtSingle(bround(w / u * rate, S.fmt.amountRound));
                }
            }
        } else $('txtCommAmount').value = '0';
    }
    /* CalculateTotalInformation :2550 */
    function calcTotalInformation() {
        if (G.grd.rows().length > 0) {
            var q = gridTotal('ItemQty'), w = gridTotal('Weight'), a = gridTotal('ItemAmount'), f = gridTotal('FcyAmount');
            if (cb.agent.row() && tn($('txtCommAmount').value) > 0 && vid(cb.cust) === vid(cb.agent)) a -= tn($('txtCommAmount').value);
            $('txtOrderQty').value = f2(bround(q, 2)); $('txtOrderWeight').value = f2(bround(w, 2));
            $('txtOrderAmount').value = fmtSingle(a); $('txtFcyAmount').value = fmtFcy(f);
        } else { $('txtOrderQty').value = '0'; $('txtOrderWeight').value = '0'; $('txtOrderAmount').value = '0'; $('txtFcyAmount').value = '0'; }
    }
    /* txtqty_TextChanged :1876 / txtRate_TextChanged :1984 / CmbPackUom_TextChanged :2535 */
    function qtyChanged() { try { calcWeight(); totalAmount(); totalCommission(); calcTotalInformation(); } catch (e) { errMsg(e); } }
    function rateChanged() { qtyChanged(); }
    function packUomChanged() { qtyChanged(); }
    /* combrateuom_TextChanged :2346 */
    function rateUomChanged() { try { totalAmount(); totalCommission(); calcTotalInformation(); } catch (e) { errMsg(e); } }
    /* txtCommrate_TextChanged :2193 / CmbCommType_TextChanged :2583 / CmbCommRateUom_TextChanged :2596 */
    function commChanged() { try { totalCommission(); calcTotalInformation(); } catch (e) { errMsg(e); } }
    /* txtExchangeRate_TextChanged :2500 */
    function exchangeChanged() {
        try {
            var er = tn($('txtExchangeRate').value), rows = G.grd.rows();
            if (rows.length > 0 && er > 0) rows.forEach(function (r) { r.ExchangeRate = er; r.FcyAmount = bround(tn(r.ItemAmount) / er, S.fmt.fcyRound); });
            else rows.forEach(function (r) { r.ExchangeRate = 0; r.FcyAmount = 0; });
            refreshGrid();
            calcTotalInformation();
        } catch (e) { errMsg(e); }
    }
    function decimalKeys(id) {                                                           // CommonServices.OnlytextdecimelFunction
        $(id).addEventListener('keypress', function (e) {
            if (e.key.length === 1 && !/[\d.]/.test(e.key) && !e.ctrlKey) e.preventDefault();
            else if (e.key === '.' && $(id).value.indexOf('.') >= 0) e.preventDefault();
        });
    }
    function digitKeys(id) { $(id).addEventListener('keypress', function (e) { if (e.key.length === 1 && !/\d/.test(e.key) && !e.ctrlKey) e.preventDefault(); }); }

    /* ------------------------------------------------------------------ grids */
    function makeGrids() {
        G.grd = SE.grid('grd', { footer: true, onDbl: function (r, i) { editRow(i); }, onBtn: function (k, r, i) { if (k === 'Delete') deleteRow(i); },
            cols: [
                { k: 'Id', hide: true }, { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 240 }, { k: 'JobLotId', hide: true }, { k: 'JobLot', t: 'JobLot', w: 120 },
                { k: 'PackingTypeId', hide: true }, { k: 'PackingType', t: 'PackingType', w: 110 }, { k: 'PackUomId', hide: true }, { k: 'PackUom', t: 'PackUom', w: 80 },
                { k: 'ItemQty', t: 'ItemQty', w: 90, f: 'n2', sum: true }, { k: 'Weight', t: 'Weight', w: 100, f: 'n2', sum: true },
                { k: 'ItemRate', t: 'ItemRate', w: 90, render: function (v) { return fmtRate(v); }, cls: 'num' }, { k: 'RateUomId', hide: true }, { k: 'RateUom', t: 'RateUom', w: 80 },
                { k: 'ItemAmount', t: 'ItemAmount', w: 110, f: 'amt', sum: true }, { k: 'CityId', hide: true }, { k: 'CityName', t: 'CityName', w: 110 },
                { k: 'ExchangeRate', t: 'ExchangeRate', w: 90, render: function (v) { return fmtRate(v); }, cls: 'num' },
                { k: 'FcyAmount', t: 'FcyAmount', w: 100, render: function (v) { return fmtFcy(v); }, cls: 'num', sum: true },
                { k: 'Remarks', t: 'Remarks', w: 200 }, { k: 'Delete', t: 'X', w: 20, btn: 'X' }] });
        G.hist = SE.grid('grdhistory', { frozen: 2, onDbl: function (r) { histDbl(r); }, onSel: histSelected, onBtn: function (k, r) { histButton(k, r); },
            onLink: function (k, r) { if (k === 'NoOfAttachments') SS.showAttachments(API, r.Id); },
            cols: [
                { k: 'Edit', t: 'Edit', w: 50, btn: 'Edit' }, { k: 'Print', t: 'Print', w: 50, btn: 'Print' }, { k: 'Id', hide: true },
                { k: 'DocNo', t: 'DocNo', w: 60 }, { k: 'DocDate', t: 'DocDate', w: 85, f: 'sdate' }, { k: 'OrderCategory', t: 'OrderCategory', w: 120 }, { k: 'PartyName', t: 'PartyName', w: 200 },
                { k: 'CommAgent', t: 'CommAgent', w: 150 }, { k: 'CommType', t: 'CommType', w: 90 }, { k: 'CommRate', t: 'CommRate', w: 80, render: function (v) { return fmtRate(v); }, cls: 'num' },
                { k: 'CommAmount', t: 'CommAmount', w: 100, f: 'amt', sum: true }, { k: 'CommRemarks', t: 'CommRemarks', w: 150 }, { k: 'OrderStatus', t: 'OrderStatus', w: 90 },
                { k: 'DeliveryTerm', t: 'DeliveryTerm', w: 90 }, { k: 'PaymentTerm', t: 'PaymentTerm', w: 100 }, { k: 'DueDays', t: 'DueDays', w: 70 }, { k: 'DueDate', t: 'DueDate', w: 85, f: 'sdate' },
                { k: 'ExpiryDate', t: 'ExpiryDate', w: 85, f: 'sdate' }, { k: 'DeliveryDays', t: 'DeliveryDays', w: 80 }, { k: 'DeliveryStartDate', t: 'DeliveryStartDate', w: 100, f: 'sdate' },
                { k: 'OrderQty', t: 'OrderQty', w: 90, f: 'n2', sum: true }, { k: 'OrderWeight', t: 'OrderWeight', w: 100, f: 'n2', sum: true }, { k: 'OrderAmount', t: 'OrderAmount', w: 110, f: 'amt', sum: true },
                { k: 'CurrencyName', t: 'CurrencyName', w: 90 }, { k: 'ExchangeRate', t: 'ExchangeRate', w: 90, render: function (v) { return fmtRate(v); }, cls: 'num' },
                { k: 'FcyAmount', t: 'FcyAmount', w: 100, render: function (v) { return fmtFcy(v); }, cls: 'num', sum: true },
                { k: 'EntryUser', t: 'EntryUser', w: 110 }, { k: 'EntryDate', t: 'EntryDate', w: 85, f: 'sdate' }, { k: 'ModifyUser', t: 'ModifyUser', w: 110 }, { k: 'ModifyDate', t: 'ModifyDate', w: 85, f: 'sdate' },
                { k: 'IsApproved', t: 'IsApproved', w: 100 }, { k: 'ApprovedUser', t: 'ApprovedUser', w: 110 }, { k: 'ApprovedDate', t: 'ApprovedDate', w: 85, f: 'sdate' },
                { k: 'Remarks', t: 'Remarks', w: 200 }, { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 110, link: true }] });
        G.hd = SE.grid('DataGridHistoryDetail', { footer: true,
            cols: [{ k: 'ItemName', t: 'ItemName', w: 240 }, { k: 'JobLot', t: 'JobLot', w: 110 }, { k: 'PackingType', t: 'PackingType', w: 110 }, { k: 'PackUom', t: 'PackUom', w: 80 },
                { k: 'ItemQty', t: 'ItemQty', w: 90, f: 'n2', sum: true }, { k: 'Weight', t: 'Weight', w: 90, f: 'n2', sum: true },
                { k: 'ItemRate', t: 'ItemRate', w: 90, render: function (v) { return fmtRate(v); }, cls: 'num' }, { k: 'RateUOM', t: 'RateUOM', w: 80 },
                { k: 'ItemAmount', t: 'ItemAmount', w: 110, f: 'amt', sum: true }, { k: 'City', t: 'City', w: 110 },
                { k: 'ExchangeRate', t: 'ExchangeRate', w: 90, render: function (v) { return fmtRate(v); }, cls: 'num' },
                { k: 'FcyAmount', t: 'FcyAmount', w: 100, render: function (v) { return fmtFcy(v); }, cls: 'num', sum: true }, { k: 'Remarks', t: 'Remarks', w: 200 }] });
        /* total-row formats the shared grid does not know ("#,##0.##" for quantity/weight, the Fcy format): patch the footer after every render */
        [G.grd, G.hd, G.hist].forEach(function (g) {
            var fixing = false;
            function fix() {
                if (fixing) return; fixing = true;
                try {
                    var vc = g.spec.cols.filter(function (c) { return !c.hide; }), tds = g.el.querySelectorAll('tfoot td');
                    vc.forEach(function (c, i) {
                        if (!c.sum || !tds[i]) return;
                        var t = g.rows().reduce(function (a, r) { return a + tn(r[c.k]); }, 0);
                        if (c.f === 'n2') tds[i].textContent = f2(t);
                        else if (c.k === 'FcyAmount') tds[i].textContent = fmtFcy(t);
                    });
                } finally { fixing = false; }
            }
            new MutationObserver(fix).observe(g.el, { childList: true });
            g.fix = fix;
        });
    }
    function refreshGrid() { G.grd.refresh(); }

    /* grd_ColumnButtonClick :1144 (Delete) */
    function deleteRow(i) {
        var r = G.grd.rows()[i]; if (!r) return;
        function done() { G.grd.removeAt(i); try { totalCommission(); calcTotalInformation(); } catch (e) { errMsg(e); } }
        if (ti(r.Id) > 0) {
            return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
                if (!yes) return;
                S.remove.push(toLine(r));
                done();
            });
        }
        done();
    }
    function toLine(r) {
        return { id: ti(r.Id), itemId: ti(r.ItemId), jobLotId: ti(r.JobLotId), packingTypeId: ti(r.PackingTypeId), packUomId: ti(r.PackUomId), rateUomId: ti(r.RateUomId), cityId: ti(r.CityId),
            itemQty: tn(r.ItemQty), weight: tn(r.Weight), itemRate: tn(r.ItemRate), itemAmount: tn(r.ItemAmount), exchangeRate: tn(r.ExchangeRate), fcyAmount: tn(r.FcyAmount),
            remarks: r.Remarks == null ? '' : String(r.Remarks) };
    }

    /* ------------------------------------------------------------------ detail entry */
    /* FormValidationDetail :450 */
    function validateDetail() {
        function stop(m, c) { return msg(m).then(function () { if (c) { if (c.focus) c.focus(); else $(c).focus(); } return false; }); }
        if (!cb.item.row()) return stop('Item Name Field is Required', cb.item);
        if (!cb.job.row()) return stop('Job Lot Field is Required', cb.job);
        if (!cb.pack.row()) return stop('Packing Type Field is Required', cb.pack);
        if (!cb.uom.row()) return stop('Pack Uom  Field is Required', cb.uom);
        var q = field('txtItemQty'); if (q === '' || q === '0') return stop('Qty Field is Required', 'txtItemQty');
        var r = field('txtRate'); if (r === '' || r === '0') return stop('Item Rate Field is Required', 'txtRate');
        if (!cb.rate.row()) return stop('Rate UOM Field is Required', cb.rate);
        var a = field('txtamount'); if (a === '' || a === '0') return stop('Item Amount Should Greater Than 0');
        if (!cb.city.row()) return stop('CityName Field is Required', cb.city);
        return Promise.resolve(true);
    }
    /* btnplus_Click :512 */
    function plus() {
        return validateDetail().then(function (ok) {
            if (!ok) return;
            try {
                var er = tn($('txtExchangeRate').value), amt = tn($('txtamount').value);
                G.grd.rows().push({ Id: 0, ItemId: cb.item.value(), ItemName: cb.item.text().trim(), JobLotId: cb.job.value(), JobLot: cb.job.text().trim(), PackingTypeId: cb.pack.value(),
                    PackingType: cb.pack.text().trim(), PackUomId: cb.uom.value(), PackUom: cb.uom.text().trim(), ItemQty: tn($('txtItemQty').value.trim()), Weight: tn($('txtweight').value.trim()),
                    ItemRate: tn($('txtRate').value.trim()), RateUomId: cb.rate.value(), RateUom: cb.rate.text().trim(), ItemAmount: amt, CityId: cb.city.value(), CityName: cb.city.text().trim(),
                    ExchangeRate: er > 0 ? er : 0, FcyAmount: er > 0 ? bround(amt / er, S.fmt.fcyRound) : 0, Remarks: $('txtRemarksDetail').value.trim() });
                refreshGrid();
                resetDetail();
                totalCommission();
                calcTotalInformation();
            } catch (e) { return errMsg(e); }
        });
    }
    /* ResetDetail :533 */
    function resetDetail() {
        cb.item.clear(); S.lastItemSet = false; itemChanged();
        cb.pack.clear(); cb.uom.clear(); $('txtItemQty').value = ''; $('txtweight').value = ''; $('txtRate').value = ''; cb.rate.clear(); $('txtamount').value = ''; $('txtRemarksDetail').value = '';
        cb.item.focus();
    }
    /* grd_DoubleClick :1999 */
    function editRow(i) {
        var r = G.grd.rows()[i]; if (!r) return Promise.resolve();
        S.updateIdx = i; S.detailId = ti(r.Id);
        setCombo(cb.item, r.ItemId); S.lastItemSet = false;
        return itemChanged().then(function () {
            setCombo(cb.job, r.JobLotId); setCombo(cb.pack, r.PackingTypeId); setCombo(cb.uom, r.PackUomId);
            $('txtItemQty').value = String(r.ItemQty); $('txtweight').value = String(r.Weight); $('txtRate').value = String(r.ItemRate);
            setCombo(cb.rate, r.RateUomId); $('txtamount').value = String(r.ItemAmount); setCombo(cb.city, r.CityId); $('txtRemarksDetail').value = r.Remarks == null ? '' : String(r.Remarks);
            show('btnplus', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
        }).catch(errMsg);
    }
    /* btnUpdateDetail_Click :2031 */
    function updateDetail() {
        return validateDetail().then(function (ok) {
            if (!ok) return;
            var r = G.grd.rows()[S.updateIdx]; if (!r) return;
            r.Id = S.detailId; r.ItemId = cb.item.value(); r.ItemName = cb.item.text(); r.JobLotId = cb.job.value(); r.JobLot = cb.job.text();
            r.PackingTypeId = cb.pack.value(); r.PackingType = cb.pack.text(); r.PackUomId = cb.uom.value(); r.PackUom = cb.uom.text();
            r.ItemQty = tn($('txtItemQty').value); r.Weight = tn($('txtweight').value); r.ItemRate = tn($('txtRate').value); r.RateUomId = cb.rate.value(); r.RateUom = cb.rate.text();
            r.ItemAmount = tn($('txtamount').value); r.CityId = cb.city.value(); r.CityName = cb.city.text(); r.Remarks = $('txtRemarksDetail').value;
            exchangeChanged();
            resetDetail();
            show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        });
    }
    function cancelDetail() { resetDetail(); show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false); }

    /* ------------------------------------------------------------------ ConfigurationDefault :2260 */
    function configDefault() {
        var d = S.def;
        if (d.cityId) setCombo(cb.city, d.cityId);
        if (d.jobLotId) setCombo(cb.job, d.jobLotId);
        if (d.currencyId) setCombo(cb.cur, d.currencyId);
        if (d.exchangeRate) { $('txtExchangeRate').value = String(d.exchangeRate); exchangeChanged(); }
    }

    /* ------------------------------------------------------------------ Reset :1290 */
    function reset() {
        S.files = []; S.removed = []; S.existing = []; S.remove = []; S.approved = false; S.id = 0; S.detailId = 0;
        show('btnSaveAs', false);
        cb.cust.clear(); $('txtsupprefno').value = ''; $('txtduedays').value = ''; dueDaysChanged(); $('txtDeliveryDays').value = ''; deliveryDaysChanged();
        cb.agent.clear(); cb.ctype.clear(); $('txtCommRate').value = ''; $('txtCommAmount').value = ''; $('txtCommRemarks').value = '';
        cb.item.clear(); cb.job.clear(); cb.uom.clear(); $('txtItemQty').value = ''; $('txtweight').value = ''; $('txtRate').value = ''; cb.rate.clear(); $('txtamount').value = '';
        cb.city.clear(); $('txtRemarksDetail').value = ''; $('txtRemarks').value = '';
        S.lastItemSet = false; itemChanged();
        show('btnsave', true); show('btnupdate', false); show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        SS.dis(cb.ctype, true); $('txtCommRate').disabled = true; SS.dis(cb.cuom, true); $('txtCommAmount').disabled = true; $('txtCommRemarks').disabled = true; SS.dis(cb.status, true);
        setRow(cb.status, 0); setRow(cb.cat, 1);
        $('DocDate').focus();
        return genCategoryNo().then(function () { return SE.api(API + '/next-no'); }).then(function (d) {
            if (d.nextNo > 0) $('txtdocno').value = String(d.nextNo);                    // DocumentNoFill
            configDefault();
            $('txtOrderQty').value = ''; $('txtOrderWeight').value = ''; $('txtOrderAmount').value = ''; $('txtFcyAmount').value = '';
            G.grd.setRows([]);
        }).catch(errMsg);
    }
    /* btnRefresh_Click :2239 */
    function refresh() {
        return SE.api(API + '/refresh').then(function (d) {
            bindLists(d, false);
            S.lastItemSet = false; return itemChanged();
        }).catch(errMsg);
    }

    /* ------------------------------------------------------------------ Save (Insert :554) */
    function formValidation() {
        function stop(m, c) { return msg(m).then(function () { if (c) { if (c.focus) c.focus(); else $(c).focus(); } return false; }); }
        var no = field('txtdocno'); if (no === '' || no === '0') return stop('Doc No Field is Required', 'txtdocno');
        if (!cb.cat.row()) return stop('Order Catagory Field is Required', cb.cat);
        if (!cb.cust.row()) return stop('Customer Name Field is Required', cb.cust);
        if (!cb.dterm.row()) return stop('Delivery Term Field is Required', cb.dterm);
        if (!cb.term.row()) return stop('Payment Term Field is Required', cb.term);
        var dd = field('txtduedays');
        if (vid(cb.term) === 2 && (dd === '' || dd === '0')) return stop('Due Days Field is Required', 'txtduedays');
        if (!cb.status.row()) return stop('Status Field is Required', cb.status);
        var oa = field('txtOrderAmount'); if (oa === '' || oa === '0') return stop('Order Amount Should Greater Than 0');
        if (!cb.cur.row()) return stop('Currency Field is Required', cb.cur);
        var er = field('txtExchangeRate'); if (er === '' || tn(er) === 0) return stop('Exchange Rate Field is Required', 'txtExchangeRate');
        if (tn($('txtCommAmount').value) > 0 && (!cb.agent.row() || cb.agent.text() === '')) return stop('Sales Man Agent Is Requird', cb.agent);
        if (cb.agent.text() !== '0' && cb.agent.row() && cb.agent.text() !== '' && tn($('txtCommAmount').value) === 0) return stop('Commission Amount Is Requird', 'txtCommAmount');
        var fa = field('txtFcyAmount'); if (fa === '' || fa === '0') return stop('Fcy Amount Should Greater Than 0');
        return Promise.resolve(true);
    }
    function insert(id) {
        return formValidation().then(function (ok) {
            if (!ok) return;
            return SE.ask(id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
                if (!yes) return;
                var hasAgent = !!cb.agent.row();
                if (hasAgent) { try { calcTotalInformation(); } catch (e) { return errMsg(e); } }
                var rows = G.grd.rows();
                if (rows.length === 0) return SE.alert('Grid Record Not Found', '');
                var keep = rows.filter(function (r) { return tn(r.ItemQty) > 0; });
                for (var i = 0; i < keep.length; i++) if (!(tn(keep[i].ItemRate) > 0)) return SE.alert('Rate Field Required', 'Database Error');
                if (keep.length === 0) return SE.alert('At least enter value/quantity in one of the rows', 'Database Error');
                var amount = tn($('txtOrderAmount').value);
                return SE.api(API + '/limit' + SE.q({ customerId: vid(cb.cust), amount: amount })).then(function (l) {
                    var lim = l.availableLimit;
                    if (lim > 0) return SE.ask('Debit Limit of Customer Exceeds Define Debit Limit & remaining limit value is   ' + String(lim), 'Confirm');
                    if (lim < 0) return SE.ask('Debit Limit of Customer already Exceeds Define Debit Limit', 'Confirm');
                    return true;
                }).then(function (go) {
                    if (!go) return;
                    var body = {
                        id: id, docDate: $('DocDate').value, docNo: field('txtdocno'), orderCategoryId: vid(cb.cat), catSrNo: field('txtcatsr'), customerId: vid(cb.cust),
                        partyReference: $('txtsupprefno').value, refPartyId: 0, remarks: $('txtRemarks').value, paymentTermId: vid(cb.term), dueDays: $('txtduedays').value,
                        dueDate: $('duedate').value, expiryDate: $('datExpiryDate').value, deliveryTerm: cb.dterm.text(), deliveryStartDate: $('deliverystartdate').value,
                        deliveryDays: $('txtDeliveryDays').value, orderStatus: cb.status.text(), currencyId: vid(cb.cur), orderQty: tn($('txtOrderQty').value),
                        orderWeight: tn($('txtOrderWeight').value), orderAmount: amount, exchangeRate: tn($('txtExchangeRate').value), fcyAmount: tn($('txtFcyAmount').value),
                        hasAgent: hasAgent, commAgentId: vid(cb.agent), commType: cb.ctype.text(), commRate: tn($('txtCommRate').value), commRateUom: cb.cuom.text(),
                        commAmount: tn($('txtCommAmount').value.trim()), commRemarks: $('txtCommRemarks').value,
                        lines: rows.map(toLine), removed: id > 0 ? S.remove : [],
                        attachments: { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removed }
                    };
                    return SE.api(API, { method: 'POST', body: body }).then(function (r) {
                        return SE.alert(r.message).then(function () {
                            var preview = $('ChkBox').checked;
                            return reset().then(function () { if (preview) print(r.id); });
                        });
                    });
                });
            }).catch(function (e) { return SE.alert(e.message, 'Database Error'); });
        });
    }
    function doSave() { S.approved = false; S.id = 0; return insert(0); }                 // btnsave_Click / btnSaveAs_Click
    function doUpdate() {                                                                  // btnupdate_Click :753
        if (S.id === 0) return SE.alert('Record Not Update because RecId Not Found', 'Database Error');
        return insert(S.id);
    }
    /* btnprint_Click :2222 / CommonServices.SaleOrderSlip_1511 */
    function print(id) {
        if (!(id > 0)) return SE.alert('No Record Selected', 'Error Message');
        return SE.printRpt('1511-SaleOrderSlipAndRegister.rpt', { id: id });
    }
    /* btnDelete_Click :2324 */
    function doDelete() {
        if (S.id <= 0) return SE.alert('No Record Found For Delete', 'Error Message');
        return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
            if (!yes) return;
            return SE.api(API + '/' + S.id, { method: 'DELETE' }).then(function (d) { return SE.alert(d.message).then(reset); });
        }).catch(function (e) { return SE.alert(e.message, 'Error Message'); });
    }

    /* ------------------------------------------------------------------ ReadById :1584 */
    function readById(id) {
        S.id = id; S.remove = [];
        return SE.api(API + '/' + id).then(function (d) {
            var h = d.head;
            tabs.select('tabPage1');
            show('btnSaveAs', true);
            $('DocDate').value = SE.dateInput(h.DocDate); $('txtdocno').value = h.DocNo == null ? '' : String(h.DocNo);
            setCombo(cb.cat, h.OrderCatagoryId); $('txtcatsr').value = h.CatagorySrNo == null ? '' : String(h.CatagorySrNo);
            setCombo(cb.cust, h.OrderSupCustId); $('txtsupprefno').value = h.PartyReference || ''; $('txtRemarks').value = h.RemarksHeader || '';
            setCombo(cb.term, h.PaymentTermsId); termChanged();
            $('txtduedays').value = h.OrderDueDays == null ? '' : String(h.OrderDueDays); dueDaysChanged();
            $('duedate').value = SE.dateInput(h.OrderDueDate); $('datExpiryDate').value = SE.dateInput(h.OrderExpiryDate);
            SS.byText(cb.dterm, 'DeliveryTerm', h.DeliveryTerm || '');
            $('deliverystartdate').value = SE.dateInput(h.DeliveryStartDate); startDateChanged();
            $('txtDeliveryDays').value = h.DeliveryDays == null ? '' : String(h.DeliveryDays); deliveryDaysChanged();
            setCombo(cb.agent, h.CommissionAgentId);
            SS.byText(cb.ctype, 'CommissionType', h.CommissionType || ''); commChanged();
            $('txtCommRate').value = h.CommRate == null ? '' : Number(h.CommRate).toFixed(4); commChanged();
            SS.byText(cb.cuom, 'UOM', h.CommRateUom == null ? '' : String(h.CommRateUom)); commChanged();
            $('txtCommAmount').value = h.CommAmount == null ? '' : Number(h.CommAmount).toFixed(6); $('txtCommRemarks').value = h.CommRemarks || '';
            SS.dis(cb.status, false); SS.byText(cb.status, 'Status', h.OrderStatus || '');
            S.approved = !!h.IsApproved;
            setCombo(cb.cur, h.CurrencyId); $('txtExchangeRate').value = h.ExchangeRate == null ? '' : Number(h.ExchangeRate).toFixed(4); exchangeChanged();
            var rows = (d.lines || []).map(function (l) {
                return { Id: l.Id, ItemId: l.OrderItemId, ItemName: l.ItemName, JobLotId: l.JobLotId, JobLot: l.JobLotDescription, PackingTypeId: l.PackingTypeId, PackingType: l.PackingType,
                    PackUomId: l.OrderItemUOMId, PackUom: l.UOMCode, ItemQty: l.OrderItemQty, Weight: l.NetWeight, ItemRate: l.OrderItemRate, RateUomId: l.OrderItemRateUOMId, RateUom: l.RateUom,
                    ItemAmount: l.ItemAmount, CityId: l.CityId, CityName: l.CityName, ExchangeRate: l.ExchangeRate, FcyAmount: l.FcyAmount, Remarks: l.RemarkDetail };
            });
            G.grd.setRows(rows);
            show('btnsave', false); show('btnupdate', true);
            S.existing = d.attachments || []; S.files = []; S.removed = [];
            totalCommission(); calcTotalInformation();
        }).catch(errMsg);
    }

    /* ------------------------------------------------------------------ history (gridhistoryfill :1644) */
    function showHistory() {
        var type = $('rdentrydate').checked ? 'entry' : $('rdmodifydate').checked ? 'modify' : $('rdapproveddate').checked ? 'approved' : 'document';
        var q = { dateType: type, fromDate: $('FromDateHistory_chk').checked ? $('FromDateHistory').value : '', toDate: $('ToDateHistory_chk').checked ? $('ToDateHistory').value : '',
                  fromNo: ti($('FromDocNoHistory').value), toNo: ti($('ToDocNoHistory').value), customerId: vid(cb.hcust) };
        return SE.api(API + '/history' + SE.q(q)).then(function (rows) {
            if (rows.length > 0) G.hist.setRows(rows); else { G.hist.setRows([]); G.hd.setRows([]); }
        }).catch(function (e) { return SE.alert(e.message); });
    }
    function resetHistory() {                                                              // btnResetHistory_Click :1520
        $('FromDateHistory').value = SE.addDays(-3); $('ToDateHistory').value = SE.today(); $('FromDocNoHistory').value = ''; $('ToDocNoHistory').value = '';
        cb.hcust.clear(); G.hist.setRows([]); G.hd.setRows([]);
    }
    function histCombos() {                                                                // HistoryCombosFill :1550
        return SE.api(API + '/history-customers').then(function (rows) { cb.hcust.setData(rows); }).catch(errMsg);
    }
    function histDbl(r) { S.id = r.Id; return readById(r.Id); }                            // grdhistory_DoubleClick
    function histButton(k, r) {                                                            // grdhistory_ColumnButtonClick
        if (k === 'Edit') return reset().then(function () { S.id = r.Id; return readById(r.Id); });
        if (k === 'Print') return print(r.Id);
    }
    function histSelected(r) {                                                             // grdhistory_SelectionChanged -> GridDetailBind (sets Id as the desktop does)
        var seq = ++S.histSeq;
        if (!r) { G.hd.setRows([]); return; }
        S.id = ti(r.Id);
        SE.api(API + '/' + r.Id + '/lines', { quiet: true }).then(function (rows) {
            if (seq !== S.histSeq) return;
            G.hd.setRows(rows.map(function (l) { return { ItemName: l.ItemName, JobLot: l.JobLotDescription, PackingType: l.PackingType, PackUom: l.UOMCode, ItemQty: l.OrderItemQty, Weight: l.NetWeight,
                ItemRate: l.OrderItemRate, RateUOM: l.RateUom, ItemAmount: l.ItemAmount, City: l.CityName, ExchangeRate: l.ExchangeRate, FcyAmount: l.FcyAmount, Remarks: l.RemarkDetail }; }));
        }).catch(function (e) { if (seq === S.histSeq) SE.alert(e.message, 'Error Message'); });
    }

    /* ------------------------------------------------------------------ shortcut keys (PurchsaeOrder_KeyDown :1432) */
    function onKey(e) {
        SS.enterTab(e);
        var k = (e.key || '').toLowerCase();
        if (e.ctrlKey && k === 't') { e.preventDefault(); if (tabs.index() === 1) tabs.select('tabPage1'); else tabs.select('tabPage2'); return; }
        if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { if (e.ctrlKey) e.preventDefault(); if (window.history.length > 1) window.history.back(); return; }
        if (e.ctrlKey && k === 's' && tabs.index() === 0) { e.preventDefault(); if (visible('btnsave')) doSave(); }
        else if (e.ctrlKey && k === 'n' && tabs.index() === 0) { e.preventDefault(); reset().then(genCategoryNo); }
        else if (e.ctrlKey && k === 'u') { e.preventDefault(); if (visible('btnupdate')) doUpdate(); }
        else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); cb.item.focus(); }
    }

    /* ------------------------------------------------------------------ load (PurchsaeOrder_Load :1201) */
    function load() {
        makeCombos();
        tabs = SE.tabs('tabControl1', function (id) { if (id === 'tabPage2') $('FromDateHistory').focus(); else $('DocDate').focus(); });
        SE.digitsOnly($('FromDocNoHistory')); SE.digitsOnly($('ToDocNoHistory')); digitKeys('txtduedays'); digitKeys('txtDeliveryDays');
        decimalKeys('txtRate'); decimalKeys('txtItemQty'); decimalKeys('txtCommRate'); decimalKeys('txtExchangeRate');
        makeGrids();
        SE.api(API + '/initial').then(function (d) {
            S.rights = d.rights || {}; S.fmt = d.fmt || S.fmt; S.def = d.defaults || {};
            G.grd.spec.dec = S.fmt.amount; G.hist.spec.dec = S.fmt.amount; G.hd.spec.dec = S.fmt.amount;
            cb.status.setData(d.statuses); cb.ctype.setData(d.commTypes); cb.cuom.setData(d.commUoms);
            bindLists(d, true);
            cb.hcust.setData(d.historyCustomers || []);
            var t = SE.today();
            ['DocDate', 'deliverystartdate', 'duedate', 'datExpiryDate'].forEach(function (id) { $(id).value = t; });
            if (d.nextNo > 0) $('txtdocno').value = String(d.nextNo);
            configDefault();
            $('btnsave').disabled = !S.rights.save; $('btnprint').disabled = !S.rights.print; $('btnupdate').disabled = !S.rights.update;
            show('btnDelete', !!S.rights['delete']);
            show('btnupdate', false); show('btnSaveAs', false); show('btnsave', true); show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
            SS.dis(cb.ctype, true); $('txtCommRate').disabled = true; SS.dis(cb.cuom, true); $('txtCommAmount').disabled = true; $('txtCommRemarks').disabled = true;
            setRow(cb.cat, 1); setRow(cb.status, 0); SS.dis(cb.status, true);
            genCategoryNo();
            $('ChkBox').checked = true;
            $('FromDateHistory').value = SE.addDays(-d.historyDays); $('ToDateHistory').value = t;
            $('txtRate').value = $('txtRate').value || '0';
            $('DocDate').focus();
        }).catch(fail);
        wire();
        var rec = SE.param('record'); if (rec) setTimeout(function () { readById(+rec); }, 600);
    }

    function wire() {
        $('btnnew').onclick = function () { reset().then(genCategoryNo); };                 // btnnew_Click: Reset(); combordercat_Leave()
        $('btnRefresh').onclick = refresh;
        $('btnsave').onclick = doSave; $('btnSaveAs').onclick = doSave; $('btnupdate').onclick = doUpdate; $('btnDelete').onclick = doDelete;
        $('btnattachment').onclick = function () { SS.attachmentDialog(S, API, function () { return S.id; }); };
        $('btnprint').onclick = function () { print(S.id); };
        $('SendEmail').onclick = function () { };                                           // SendEmail_Click saves a fixed e-mail history row on the desktop; the button is hidden there
        $('btnplus').onclick = plus; $('btnUpdateDetail').onclick = updateDetail; $('btnCancelUpdateDetial').onclick = cancelDetail;
        $('btnShow').onclick = showHistory; $('btnResetHistory').onclick = resetHistory; $('btnRefreshHistory').onclick = histCombos;
        $('txtItemQty').addEventListener('input', qtyChanged); $('txtRate').addEventListener('input', rateChanged);
        $('txtCommRate').addEventListener('input', commChanged); $('txtExchangeRate').addEventListener('input', exchangeChanged);
        $('txtduedays').addEventListener('input', dueDaysChanged); $('txtDeliveryDays').addEventListener('input', deliveryDaysChanged);
        $('deliverystartdate').addEventListener('change', startDateChanged);
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
    }

    document.addEventListener('DOMContentLoaded', load);
})();
