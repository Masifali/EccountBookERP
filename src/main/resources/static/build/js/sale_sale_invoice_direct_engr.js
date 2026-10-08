/*
 * Screen 139  SaleInvoiceQtyWithTax  (Architecture.WinApp.SaleTrading.SaleInvoiceQtyWithTax, document type 1608)
 * Page script. Desktop methods are named in the comments (siqT.cs). Server: /sale/engr/sale-invoice-direct/api
 * The invoice arithmetic (grd_CellUpdated, BillAmount, Exp/Freight/Ledger/Bill proportions) lives on the server (POST /calc, the same code Save runs);
 * this page runs the entry panel (CalculateAmount / CalculateTaxAmount) and the cell logic of the small grids.
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/engr/sale-invoice-direct/api', DOC = 1608;
    var S = {
        id: 0, rights: {}, files: [], removedAtt: [], existing: [], voucherHeadId: 0, updateIdx: -1, custGl: 0,
        cust: [], terms: [], other: [], fr: [], taxAc: [], discAc: [], curr: [], taxTypes: [], dterms: [], dtypes: [], items: [], wh: [], jobs: [], cities: [], uoms: [],
        sub: false, multi: false, taxEditable: false, fmt: { amountRound: 0, amount: 0, rate: 2, fcy: 0 }, def: {}, branchId: 0, fyStart: '', lastDiscount: 0, lastTaxAc: 0
    };
    var cb = {}, G = {}, tabs, tabs2, seq = 0;

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
    function copyRow(r) { var c = JSON.parse(JSON.stringify(r)); delete c._chk; return c; }
    function saveMode() { return isShown('btnSave') && !$('btnSave').disabled; }

    /* ------------------------------------------------------------------ combos */
    function makeCombos() {
        cb.cust = XCombo('cmbsuppliername', { columns: [{ key: 'CompanyName', caption: 'Customer Name' }], textKey: 'CompanyName', popupWidth: 420, onSelect: custChanged });
        cb.dterm = XCombo('CmbDeliveryTerm', { columns: [{ key: 'DeliveryTerm', caption: 'Delivery Term' }], textKey: 'DeliveryTerm' });
        cb.term = XCombo('CmbPaymentTerm', { columns: [{ key: 'TermsDescription', caption: 'Payment Term' }], textKey: 'TermsDescription', onSelect: termChanged });
        cb.tax = XCombo('CmbTaxAccount', { columns: [{ key: 'AccountTitle', caption: 'Account Title' }], textKey: 'AccountTitle', popupWidth: 360 });
        cb.frCr = XCombo('CmbFreightAcCr', { columns: [{ key: 'AccountTitle', caption: 'Account Title' }], valueKey: 'K', textKey: 'AccountTitle', popupWidth: 360, onSelect: function () { freightChanged('cr'); } });
        cb.frDr = XCombo('cmbFreightAcDr', { columns: [{ key: 'AccountTitle', caption: 'Account Title' }], valueKey: 'K', textKey: 'AccountTitle', popupWidth: 360, onSelect: function () { freightChanged('dr'); } });
        cb.disc = XCombo('CmbDiscountAccount', { columns: [{ key: 'AccountTitle', caption: 'Account Title' }], textKey: 'AccountTitle', popupWidth: 360 });
        cb.cur = XCombo('cmbCurrency', { columns: [{ key: 'CurrencyCode', caption: 'Currency' }], textKey: 'CurrencyCode', onSelect: currencyLeave });
        cb.wh = XCombo('CmbWarehouse', { columns: [{ key: 'WareHouseName', caption: 'WareHouse' }], textKey: 'WareHouseName', popupWidth: 260 });
        cb.item = XCombo('CmbItem', { columns: [{ key: '_t', caption: 'Item Name' }, { key: '_o', caption: 'Item Code' }], textKey: '_t', popupWidth: 460, onSelect: itemLeave });
        cb.uom = XCombo('CmbPackUom', { columns: [{ key: 'UOMCode', caption: 'Uom' }, { key: 'Equivalent', caption: 'Equivalent' }], textKey: 'UOMCode', onSelect: calcAll });
        cb.job = XCombo('CmbJobLot', { columns: [{ key: 'JobLotDescription', caption: 'Job Lot' }], textKey: 'JobLotDescription', popupWidth: 300 });
        cb.rate = XCombo('CmbRateUom', { columns: [{ key: 'UOMCode', caption: 'RateUom' }, { key: 'Equivalent', caption: 'Equivalent' }], textKey: 'UOMCode', onSelect: calcAmountTax });
        cb.dtype = XCombo('CmbDiscountType', { columns: [{ key: 'Name', caption: 'Discount Type' }], textKey: 'Name', onSelect: calcAmountTax });
        cb.ttype = XCombo('CmbTaxType', { columns: [{ key: 'TaxName', caption: 'TaxType' }], valueKey: 'TaxNameId', textKey: 'TaxName', onSelect: taxTypePicked });
        cb.city = XCombo('CmbCity', { columns: [{ key: 'CityName', caption: 'City Name' }], textKey: 'CityName', popupWidth: 320 });
        cb.hCust = XCombo('CmbCustomerHistory', { columns: [{ key: 'CustomerName', caption: 'Customer Name' }], valueKey: 'Id', textKey: 'CustomerName', popupWidth: 380 });
        cb.hTerm = XCombo('CmbPaymentTermHistory', { columns: [{ key: 'TermsDescription', caption: 'Payment Term' }], valueKey: 'Id', textKey: 'TermsDescription', popupWidth: 260 });
    }
    function keepFill(c, rows, keepId) { c.setData(rows); if (keepId > 0 && !c.setValue(keepId)) c.clear(); }
    function applyLists(d) {
        S.sub = !!d.subsidiary; S.multi = !!d.multiCurrency; S.taxEditable = !!d.taxEditable;
        S.cust = d.customers || []; S.terms = d.terms || []; S.other = d.otherItems || []; S.taxAc = d.taxAccounts || []; S.discAc = d.discountAccounts || [];
        S.curr = d.currencies || []; S.taxTypes = d.taxTypes || []; S.dterms = d.deliveryTerms || []; S.dtypes = d.discountTypes || [];
        S.wh = d.warehouses || []; S.jobs = d.jobLots || []; S.cities = d.cities || [];
        S.items = (d.items || []).map(function (r) { return { Id: r.Id, ItemName: r.ItemName, ItemCode: r.ItemCode, _t: r.ItemName, _o: r.ItemCode }; });
        S.fr = (d.freightAccounts || []).map(function (r) { var o = {}; for (var k in r) o[k] = r[k]; o.K = S.sub ? r.SupplierCustomerId : r.Id; return o; });
        S.def = d.defaults || {}; S.fmt = d.fmt || S.fmt;
        G.grd.spec.dec = S.fmt.amount; G.hist.spec.dec = S.fmt.amount;
    }
    function fillAll() {
        var a = vid(cb.cust), t = vid(cb.term), tx = vid(cb.tax), fc = vid(cb.frCr), fd = vid(cb.frDr), dc = vid(cb.disc), cu = vid(cb.cur), dt = vid(cb.dterm);
        var wh = vid(cb.wh), jb = vid(cb.job), ct = vid(cb.city), dy = vid(cb.dtype), tt = vid(cb.ttype);
        keepFill(cb.cust, S.cust, a); keepFill(cb.term, S.terms, t); keepFill(cb.dterm, S.dterms, dt);
        keepFill(cb.tax, S.taxAc, tx); keepFill(cb.frCr, S.fr, fc); keepFill(cb.frDr, S.fr, fd); keepFill(cb.disc, S.discAc, dc); keepFill(cb.cur, S.curr, cu);
        keepFill(cb.wh, S.wh, wh); keepFill(cb.item, S.items, vid(cb.item)); keepFill(cb.job, S.jobs, jb); keepFill(cb.city, S.cities, ct);
        keepFill(cb.dtype, S.dtypes, dy); keepFill(cb.ttype, S.taxTypes, tt);
        G.exp.refresh(); G.gl.refresh(); G.pay.refresh(); G.grd.refresh();
    }
    function multiCurrency() {
        ['txtExchangeRate', 'txtFcyAmount', 'label43', 'label44', 'label45'].forEach(function (id) { var e = $(id); if (e) e.style.visibility = S.multi ? '' : 'hidden'; });
        var c = $('cmbCurrency'); if (c) { c.style.visibility = S.multi ? '' : 'hidden'; if (c.__dtcombo && c.__dtcombo.wrap) c.__dtcombo.wrap.style.visibility = S.multi ? '' : 'hidden'; }
        gridUi();
    }
    function configDefault() {                                                               // ConfigurationDefault
        if (S.def.cityId > 0 && !(vid(cb.city) > 0)) cb.city.setValue(S.def.cityId);
        if (S.def.jobLotId > 0 && !(vid(cb.job) > 0)) cb.job.setValue(S.def.jobLotId);
        if (S.def.baseCurrency > 0 && !(vid(cb.cur) > 0)) cb.cur.setValue(S.def.baseCurrency);
        if (S.def.baseRate > 0 && num('txtExchangeRate') === 0) $('txtExchangeRate').value = fmtRate(S.def.baseRate);
    }
    function defaultsAtLoad() {                                                              // Rows[1] / Rows[0] .Activate()
        if (S.dtypes.length > 1) cb.dtype.setValue(S.dtypes[1].Id);
        if (S.terms.length > 1) cb.term.setValue(S.terms[1].Id);
        if (S.dterms.length > 0) cb.dterm.setValue(S.dterms[0].Id);
        termChanged();
    }
    function custRow() { var id = vid(cb.cust); for (var i = 0; i < S.cust.length; i++) if (+S.cust[i].Id === id) return S.cust[i]; return null; }
    function orderId() { return G.grd.count() ? (+G.grd.rows()[0].OrderId || 0) : 0; }

    /* ------------------------------------------------------------------ server calculation */
    function reqBody(withAtt) {
        var b = {
            id: S.id, docNo: $('txtdocno').value, docDate: $('DocDate').value, customerId: vid(cb.cust),
            deliveryTerm: cb.dterm.text() || '', paymentTermId: vid(cb.term), dueDays: $('txtDueDays').value, dueDate: $('DueDate').value, currencyId: vid(cb.cur),
            exchangeRate: num('txtExchangeRate'), taxAccountId: vid(cb.tax), freightCrId: vid(cb.frCr), freightDrId: vid(cb.frDr), freightAmount: num('txtFreightAmount'),
            freightRemarks: $('txtFreightRemarks').value, discountAccountId: vid(cb.disc), discountAmount: num('txtDiscountAmountHeader'), manualBillNo: $('txtbillno').value,
            refNo: $('txtRefrenceNo').value, remarks: $('txtRemarks').value,
            lines: G.grd.rows(), expenses: G.exp.rows(), journals: G.gl.rows(), payments: G.pay.rows()
        };
        if (withAtt) b.attachments = { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removedAtt };
        return b;
    }
    function applyCalc(r) {
        var lines = r.lines || [];
        G.grd.setRows(lines);
        if (r.expenses) G.exp.setRows(r.expenses);
        if (lines.length) { $('txtBillAmount').value = fmtAmt(+r.billAmount || 0); $('txtFcyAmount').value = f4(+r.fcyAmount || 0); }
        else $('txtFcyAmount').value = '0';                                                  // BillAmount(): no rows -> txtFcyAmount "0", the bill text stays
        G.grd.refresh(); G.exp.refresh();
    }
    /* ExpProportion + FreightProportion + LedgerProportion + BillAmount + txtExchangeRate_TextChanged, as Save runs them */
    function recalc() {
        var my = ++seq;
        return SE.api(API + '/calc', { method: 'POST', body: reqBody(false), quiet: true }).then(function (r) { if (my === seq) applyCalc(r); }).catch(function (e) { if (my === seq) fail(e); });
    }
    function billAmount() { return num('txtBillAmount'); }
    function itemTotal() { return G.grd.rows().reduce(function (a, r) { return a + (+r.ItemAmount || 0); }, 0); }

    /* ------------------------------------------------------------------ header events */
    function custChanged() {                                                                 // cmbsuppliername_ValueChanged
        var r = custRow(); S.custGl = r ? +r.GlAccountId || 0 : 0; $('txtSupplierGLId').value = String(S.custGl);
        return recalc();
    }
    function termChanged() {                                                                 // CmbPaymentTerm text 'Cash'
        if ((cb.term.text() || '').trim() === 'Cash') { $('txtDueDays').disabled = true; $('txtDueDays').value = ''; $('DueDate').value = $('DocDate').value; }
        else $('txtDueDays').disabled = false;
    }
    function dueDaysChanged() {                                                              // txtDueDays_TextChanged
        var t = $('txtDueDays').value.trim();
        $('DueDate').value = t !== '' ? addDays($('DocDate').value, SE.toInt(t)) : $('DocDate').value;
    }
    function dueDateChanged() {                                                              // DueDate_ValueChanged
        if (!$('DueDate').value) return;
        if ($('DueDate').value < $('DocDate').value) $('DueDate').value = $('DocDate').value;
        $('txtDueDays').value = String(dayDiff($('DueDate').value, $('DocDate').value));
    }
    function docDateLeave() {                                                                // DocDate_Leave
        var p = Promise.resolve();
        if (G.grd.count() > 0 && saveMode()) {
            var ids = ''; G.grd.rows().forEach(function (r) { ids += ',' + r.ItemId; });
            p = SE.api(API + '/tax-by-items' + SE.q({ itemIds: ids, date: $('DocDate').value })).then(function (dt) {
                G.grd.rows().forEach(function (it) {
                    var hit = null; dt.forEach(function (x) { if (+ci(x, 'ItemId') === +it.ItemId) hit = x; });
                    if (hit) { it.TaxTypeId = +ci(hit, 'TaxNameId') || 0; it.TaxPct = +ci(hit, 'TaxPercent') || 0; }
                    else { it.TaxTypeId = 0; it.TaxPct = 0; }
                    it.calc = it.calc || 'tax';
                });
            });
        }
        return p.then(function () { dueDaysChanged(); return SE.api(API + '/tax-types' + SE.q({ date: $('DocDate').value })); })
            .then(function (tt) { S.taxTypes = tt; keepFill(cb.ttype, S.taxTypes, vid(cb.ttype)); G.grd.refresh(); return recalc(); }).catch(fail);
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
    function freightChanged(side) {                                                          // CmbFreightAcCr_Leave / cmbFreightAcDr_Leave
        var cr = vid(cb.frCr), dr = vid(cb.frDr);
        if (cr > 0 && cr === dr) {
            (side === 'cr' ? cb.frCr : cb.frDr).clear();
            msg("Freight Credit And Freight Debit Account Can't be same", 'Warning Message');
        }
        recalc();
    }
    function discountHeaderChanged() {                                                       // txtDiscountAmountHeader_TextChanged
        var v = num('txtDiscountAmountHeader');
        if (v !== 0 && v >= billAmount()) { $('txtDiscountAmountHeader').value = '0'; msg('Discount Amount Can not be Greater than or Equal to Bill Amount', 'Warning Message'); }
        recalc();
    }

    /* ------------------------------------------------------------------ entry panel */
    function calcWeight() {                                                                  // CalculateWeight (hidden boxes)
        var r = cb.uom.row(), q = num('txtItemQty');
        $('txtGrossWeight').value = r && vid(cb.uom) > 0 && q > 0 ? f3((+r.Equivalent || 0) * q) : '0';
        $('txtNetBillWeight').value = f3(num('txtGrossWeight') + num('txtAddLss'));
    }
    function calcTax() {                                                                     // CalculateTaxAmount
        var amt = num('txtAmount'), tr = cb.ttype.row();
        if (tr && vid(cb.ttype) > 0) {
            var pct = num('txtTaxPercent');
            if (!(pct > 0)) { pct = +tr.TaxPercent || 0; $('txtTaxPercent').value = f3(pct); }
            if (pct > 100) { pct = 100; $('txtTaxPercent').value = '100'; msg("Tax % Can't be greater than 100..."); }
            var t = amt * pct / 100;
            $('txtTaxAmount').value = fmtAmt(t); $('txtTotalAmount').value = fmtAmt(amt + t);
        } else { $('txtTaxAmount').value = '0'; $('txtTotalAmount').value = fmtAmt(amt); }
    }
    function calcAmount() {                                                                  // CalculateAmount
        var ru = cb.rate.row(), eq = ru && vid(cb.rate) > 0 ? (+ru.Equivalent || 0) : 0;
        var qty = num('txtItemQty'), rate = num('txtRate'), add = num('txtAddLessRate'), pct = num('txtDiscPercent'), da = 0;
        if (qty > 0 && rate > 0 && eq > 0) {
            var net = add !== 0 ? rate + add : rate;
            $('txtNetRate').value = fmtRate(net);
            var amount = qty / eq * net, dt = vid(cb.dtype);
            if (dt === 1) da = pct > 0 ? pct : 0;
            else if (dt === 2) {
                if (pct > 100) { msg("Discount Percent Can't Greater Than 100.", 'WARNING!!!'); $('txtDiscPercent').value = '100'; pct = 100; }
                da = pct > 0 ? amount * pct / 100 : 0;
            } else { da = 0; $('txtDiscPercent').value = '0'; }
            $('txtDiscountAmount').value = f3(da);
            amount -= da;
            var a = fmtAmt(rnd(amount, S.fmt.amountRound));
            $('txtAmount').value = a; $('txtTotalAmount').value = a;
        } else { $('txtAmount').value = '0'; $('txtNetRate').value = '0'; $('txtTotalAmount').value = '0'; }
    }
    function calcAmountTax() { calcAmount(); calcTax(); }
    function calcAll() { calcWeight(); calcAmount(); calcTax(); }
    function taxTypePicked() { $('txtTaxPercent').value = ''; calcTax(); }

    function byCode(rows, code) { for (var i = 0; i < rows.length; i++) if (String(rows[i].UOMCode) === String(code)) return rows[i]; return null; }
    function itemLeave() {                                                                   // CmbItem_Leave -> BindPackUomAndRateUom
        var pu = cb.uom.text(), ru = cb.rate.text(), id = vid(cb.item);
        cb.uom.clear(); cb.rate.clear();
        if (!(id > 0)) { cb.uom.setData([]); cb.rate.setData([]); return Promise.resolve(); }
        return SE.api(API + '/uoms' + SE.q({ itemId: id })).then(function (rows) {
            S.uoms = rows; cb.uom.setData(rows); cb.rate.setData(rows);
            var a = byCode(rows, pu), b = byCode(rows, ru);
            if (rows.length && a) cb.uom.setValue(a.Id); else cb.uom.clear();
            if (rows.length && b) cb.rate.setValue(b.Id); else cb.rate.clear();
            $('txtTaxPercent').disabled = !S.taxEditable;
            calcAll();
        }).catch(fail);
    }
    function resetDetail() {                                                                 // ResetDetail
        cb.item.clear(); cb.uom.clear(); cb.rate.clear(); cb.ttype.clear();
        ['txtItemQty', 'txtGrossWeight', 'txtAddLss', 'txtNetBillWeight', 'txtRate', 'txtAddLessRate', 'txtNetRate', 'txtAmount', 'txtTaxPercent', 'txtTaxAmount',
            'txtDiscPercent', 'txtDiscountAmount', 'txtgatepassno', 'txtvehicleno', 'txtTotalAmount'].forEach(function (id) { $(id).value = ''; });
    }
    function detailButtons(updating) { show('btnAdd', !updating); show('btnUpdateDetail', updating); show('btnCancelUpdateDetial', updating); }
    function detailValid() {                                                                 // FormValidationDetila
        function stop(m, f) { msg(m); if (f) f(); return false; }
        if (!cb.wh.row() || !(vid(cb.wh) > 0)) return stop('Warehouse Field is Required', function () { cb.wh.focus(); });
        if (!cb.item.row() || !(vid(cb.item) > 0)) return stop('Item Name Field is Required', function () { cb.item.focus(); });
        if (!cb.job.row() || !(vid(cb.job) > 0)) return stop('Job/Lot Field is Required', function () { cb.job.focus(); });
        if (!cb.uom.row() || !(vid(cb.uom) > 0)) return stop('Uom Field is Required', function () { cb.uom.focus(); });
        if (num('txtItemQty') === 0) return stop('Qty Field is Required', function () { $('txtItemQty').focus(); });
        if (num('txtRate') === 0) return stop('Item Rate Field is Required', function () { $('txtRate').focus(); });
        if (!cb.rate.row() || !(vid(cb.rate) > 0)) return stop('Rate UOM Field is Required', function () { cb.rate.focus(); });
        if (vid(cb.dtype) > 0 && num('txtDiscPercent') <= 0) return stop('Discount Rate Field is Required when Discount Type is Selected', function () { $('txtDiscPercent').focus(); });
        if (vid(cb.dtype) !== 0 && ($('txtDiscountAmount').value.trim() === '' || $('txtDiscountAmount').value.trim() === '0'))
            return stop('Discount Amount Field is Required when Discount Type is Selected', function () { $('txtDiscountAmount').focus(); });
        if (!cb.city.row() || !(vid(cb.city) > 0)) return stop('CityName Field is Required', function () { cb.city.focus(); });
        return true;
    }
    function panelValues(row) {                                                              // the columns btnAdd / btnUpdateDetail write
        var ir = cb.item.row(), pu = cb.uom.row(), ru = cb.rate.row();
        row.WarehouseId = vid(cb.wh); row.ItemId = vid(cb.item); row.ItemCode = ir ? ir.ItemCode : ''; row.ItemName = cb.item.text();
        row.PackUomId = vid(cb.uom); row.PackUom = cb.uom.text(); row.PackEquivalent = pu ? +pu.Equivalent || 0 : 0;
        row.JobLotId = vid(cb.job); row.JobLot = cb.job.text(); row.ItemQty = num('txtItemQty'); row.ItemRate = num('txtRate');
        row.RateUomId = vid(cb.rate); row.RateUom = cb.rate.text(); row.RateEquivalent = ru ? +ru.Equivalent || 0 : 0;
        row.AddLessRate = num('txtAddLessRate'); row.NetRate = num('txtNetRate'); row.DiscountType = vid(cb.dtype); row.DiscPct = num('txtDiscPercent');
        row.DiscountAmount = num('txtDiscountAmount'); row.ItemAmount = num('txtAmount'); row.CityId = vid(cb.city);
        row.GpDate = $('txtgpdate').value || SE.today(); row.GpNo = SE.toInt($('txtgatepassno').value); row.VehicleNo = $('txtvehicleno').value;
        row.TaxTypeId = vid(cb.ttype); row.TaxPct = num('txtTaxPercent'); row.TaxAmount = num('txtTaxAmount');
        row.calc = 'entry';
        return row;
    }
    function btnAdd() {                                                                      // btnAdd_Click
        if (!detailValid()) return;
        var r = panelValues({ Id: 0, OrderId: 0, OrderDetailId: 0, OrderNo: 0, OrderDate: SE.today(), FcyAmount: 0, Expense: 0, Freights: 0, Journal: 0, BillAmount: 0 });
        G.grd.addRow(r);
        gridUi();
        cb.item.clear(); $('txtRate').value = ''; $('txtItemQty').value = '';
        cb.item.focus();
        return recalc();
    }
    function gridDbl(row, i) {                                                               // grd_DoubleClick
        if (!row || +row.OrderId > 0) return;
        S.updateIdx = i;
        cb.wh.setValue(+row.WarehouseId || 0); cb.item.setValue(+row.ItemId || 0);
        itemLeave().then(function () {
            cb.uom.setValue(+row.PackUomId || 0); cb.job.setValue(+row.JobLotId || 0);
            $('txtItemQty').value = String(row.ItemQty); $('txtRate').value = fmtRate(+row.ItemRate || 0);
            cb.rate.setValue(+row.RateUomId || 0); $('txtAddLessRate').value = fmtRate(+row.AddLessRate || 0); $('txtNetRate').value = fmtRate(+row.NetRate || 0);
            cb.dtype.setValue(+row.DiscountType || 0); $('txtDiscPercent').value = f3(+row.DiscPct || 0); $('txtDiscountAmount').value = f3(+row.DiscountAmount || 0);
            $('txtAmount').value = fmtAmt(+row.ItemAmount || 0); cb.city.setValue(+row.CityId || 0);
            $('txtgpdate').value = SE.dateInput(row.GpDate) || SE.today(); $('txtgatepassno').value = String(row.GpNo == null ? '' : row.GpNo); $('txtvehicleno').value = row.VehicleNo || '';
            cb.ttype.setValue(+row.TaxTypeId || 0); $('txtTaxPercent').value = String(row.TaxPct == null ? '' : row.TaxPct); $('txtTaxAmount').value = String(row.TaxAmount == null ? '' : row.TaxAmount);
            detailButtons(true); cb.wh.focus();
        });
    }
    function btnUpdateDetail() {                                                             // btnUpdateDetail_Click
        if (!detailValid()) return;
        var row = G.grd.rows()[S.updateIdx];
        if (row) panelValues(row);
        detailButtons(false);
        resetDetail(); cb.item.focus(); S.updateIdx = -1;
        gridUi();
        return recalc();
    }
    function btnCancelUpdateDetail() { detailButtons(false); resetDetail(); S.updateIdx = -1; cb.item.focus(); }

    /* ------------------------------------------------------------------ grids */
    function rateRender(v) { return v == null || v === '' ? '' : fmtRate(+v); }
    function makeGrids() {
        G.grd = SE.grid('grd', { frozen: 2, dec: 0, onEdit: gridEdit, onBtn: gridBtn, onDbl: gridDbl, cols: [
            { k: '_add', t: '+', w: 26, btn: '+', hide: true }, { k: '_del', t: 'X', w: 26, btn: 'X' },
            { k: 'OrderNo', t: 'OrderNo', w: 70, hide: true }, { k: 'OrderDate', t: 'OrderDate', w: 85, f: 'date' },
            { k: 'WarehouseId', t: 'Ware House', w: 110, edit: true, list: function () { return S.wh; }, lk: 'Id', lt: 'WareHouseName' },
            { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'PackUom', t: 'Uom', w: 60 }, { k: 'JobLot', t: 'JobLot', w: 90 },
            { k: 'ItemQty', t: 'ItemQty', w: 80, f: 'n3', edit: true, sum: true },
            { k: 'ItemRate', t: 'ItemRate', w: 80, f: 'n2', render: rateRender }, { k: 'RateUom', t: 'RateUom', w: 70 },
            { k: 'AddLessRate', t: 'AddLessRate', w: 80, f: 'n2', edit: true, render: rateRender }, { k: 'NetRate', t: 'NetRate', w: 80, f: 'n2', render: rateRender },
            { k: 'DiscountType', t: 'DiscountType', w: 60, edit: true, list: function () { return S.dtypes; }, lk: 'Id', lt: 'Name' },
            { k: 'DiscPct', t: 'Discount%', w: 70, f: 'n2', edit: true, render: rateRender },
            { k: 'DiscountAmount', t: 'DiscountAmount', w: 80, f: 'n3', sum: true }, { k: 'ItemAmount', t: 'ItemAmount', w: 90, f: 'amt', sum: true },
            { k: 'FcyAmount', t: 'FcyAmount', w: 90, f: 'amt', sum: true, hide: true },
            { k: 'CityId', t: 'CityName', w: 100, edit: true, list: function () { return S.cities; }, lk: 'Id', lt: 'CityName' },
            { k: 'GpDate', t: 'GpDate', w: 85, f: 'date', edit: true }, { k: 'GpNo', t: 'GpNo', w: 60, edit: true }, { k: 'VehicleNo', t: 'VehicleNo', w: 90, edit: true },
            { k: 'TaxTypeId', t: 'Tax Type', w: 60, list: function () { return S.taxTypes; }, lk: 'TaxNameId', lt: 'TaxName' },
            { k: 'TaxPct', t: 'Tax%', w: 60, f: 'n3' }, { k: 'TaxAmount', t: 'TaxAmount', w: 80, f: 'amt', sum: true },
            { k: 'Expense', t: 'Expense', w: 80, f: 'n3', sum: true }, { k: 'Freights', t: 'Freights', w: 80, f: 'n3', sum: true },
            { k: 'Journal', t: 'Journal', w: 80, f: 'amt', sum: true }, { k: 'BillAmount', t: 'BillAmount', w: 100, f: 'amt', sum: true }] });
        G.exp = SE.grid('grdInvExp', { footer: false, frozen: 2, dec: 0, onBtn: expBtn, onEdit: expEdit, cols: [
            { k: '_add', t: '+', w: 26, btn: '+' }, { k: '_del', t: 'X', w: 26, btn: 'X' },
            { k: 'ItemId', t: 'Item', w: 200, edit: true, list: function () { return S.other; }, lk: 'Id', lt: 'OtherItemName' },
            { k: 'Qty', t: 'Qty', w: 70, edit: true, f: 'n3' }, { k: 'Rate', t: 'Rate', w: 80, edit: true, f: 'n3' },
            { k: 'Amount', t: 'Amount', w: 90, f: 'n2' }, { k: 'Remarks', t: 'Remarks', w: 180, edit: true }] });
        G.pay = SE.grid('grdPaymentTerm', { footer: false, frozen: 2, dec: 0, onBtn: payBtn, onEdit: payEdit, cols: [
            { k: '_add', t: '+', w: 26, btn: '+' }, { k: '_del', t: 'X', w: 26, btn: 'X' },
            { k: 'PaymentTermId', t: 'Payment Term', w: 120, edit: true, list: function () { return S.terms; }, lk: 'Id', lt: 'TermsDescription' },
            { k: '%ofTotal', t: '%ofTotal', w: 70, edit: true, f: 'n3' }, { k: 'Amount', t: 'Amount', w: 90, edit: true, f: 'n3' },
            { k: 'DueDays', t: 'DueDays', w: 60, edit: true, f: 'n0' }, { k: 'DueDate', t: 'DueDate', w: 100, edit: true, f: 'date' },
            { k: 'Remarks', t: 'Remarks', w: 300, edit: true }] });
        G.gl = SE.grid('grdGLedger', { footer: false, frozen: 2, dec: 0, onBtn: glBtn, onEdit: glEdit, cols: [
            { k: '_add', t: '+', w: 26, btn: '+' }, { k: '_del', t: 'X', w: 26, btn: 'X' },
            { k: 'AccountId', t: 'Account', w: 250, edit: true, list: function () { return S.fr; }, lk: 'K', lt: 'AccountTitle' }, { k: 'GlAccountId', hide: true },
            { k: 'Remarks', t: 'Remarks', w: 140, edit: true }, { k: 'Percentage', t: 'Percentage', w: 70, edit: true, f: 'n3' },
            { k: 'Qty', t: 'Qty', w: 60, edit: true, f: 'n3', sum: true }, { k: 'Rate', t: 'Rate', w: 60, edit: true, f: 'n3' },
            { k: 'Debit', t: 'Debit', w: 80, edit: true, f: 'n3', sum: true }, { k: 'Credit', t: 'Credit', w: 80, edit: true, f: 'n3', sum: true }] });
        G.hist = SE.grid('grdHistory', { frozen: 4, dec: 0, onDbl: function (r) { histEdit(r); }, onBtn: histBtn, onLink: histLink, onSel: histSelected, cols: [
            { k: 'Edit', t: 'Edit', w: 44, btn: 'Edit' }, { k: 'Print1', t: 'Print-1608', w: 84, btn: 'Print-1608' }, { k: 'Print2', t: 'Print-1608A', w: 90, btn: 'Print-1608A' },
            { k: 'Voucher', t: 'Voucher', w: 70, btn: 'Voucher' },
            { k: 'DocNo', t: 'DocNo', w: 70 }, { k: 'DocDate', t: 'DocDate', w: 90, f: 'sdate' }, { k: 'DueDate', t: 'DueDate', w: 90, f: 'sdate' },
            { k: 'ManualBillNo', t: 'ManualBillNo', w: 90 }, { k: 'CustomerName', t: 'CustomerName', w: 200 }, { k: 'BillAmount', t: 'BillAmount', w: 100, f: 'amt', sum: true },
            { k: 'EntryUser', t: 'EntryUser', w: 100 }, { k: 'EntryDate', t: 'EntryDate', w: 130, f: 'dt12' }, { k: 'ModifyUser', t: 'ModifyUser', w: 100 },
            { k: 'ModifyDate', t: 'ModifyDate', w: 130, f: 'dt12' }, { k: 'ApprovedUser', t: 'ApprovedUser', w: 100 }, { k: 'ApprovedDate', t: 'ApprovedDate', w: 130, f: 'dt12' },
            { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 90, link: true }, { k: 'Remarks', t: 'Remarks', w: 200 }] });
        G.hd = SE.grid('grdDetail', { dec: 0, cols: [
            { k: 'OrderNo', t: 'OrderNo', w: 70 }, { k: 'Warehouse', t: 'Warehouse', w: 120 }, { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'Uom', t: 'Uom', w: 60 }, { k: 'JobLot', t: 'JobLot', w: 90 },
            { k: 'ItemQty', t: 'ItemQty', w: 80, f: 'n3' }, { k: 'ItemRate', t: 'ItemRate', w: 80, f: 'n2' }, { k: 'RateUOM', t: 'RateUOM', w: 70 }, { k: 'AddLessRate', t: 'AddLessRate', w: 80, f: 'n2' },
            { k: 'NetRate', t: 'NetRate', w: 80, f: 'n2' }, { k: 'DiscountType', t: 'DiscountType', w: 80 }, { k: 'DiscPct', t: 'Discount%', w: 70, f: 'n2' },
            { k: 'DiscountAmount', t: 'DiscountAmount', w: 90, f: 'n3' }, { k: 'ItemAmount', t: 'ItemAmount', w: 90, f: 'amt' }, { k: 'FcyAmount', t: 'FcyAmount', w: 90, f: 'amt', hide: true },
            { k: 'CityName', t: 'CityName', w: 100 }, { k: 'GpDate', t: 'GpDate', w: 85, f: 'date' }, { k: 'GpNo', t: 'GpNo', w: 60 }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 },
            { k: 'TaxName', t: 'TaxName', w: 90 }, { k: 'TaxPct', t: 'Tax%', w: 60, f: 'n3' }, { k: 'TaxAmount', t: 'TaxAmount', w: 80, f: 'amt' }] });
    }
    function colOf(g, k) { return g.spec.cols.filter(function (x) { return x.k === k; })[0]; }
    function gridUi() {                                                                      // grdSettings: what depends on OrderId, Save and the configurations
        var order = orderId() > 0;
        var c;
        if ((c = colOf(G.grd, 'OrderNo'))) c.hide = !order;
        if ((c = colOf(G.grd, '_add'))) c.hide = !order;
        if ((c = colOf(G.grd, '_del'))) c.hide = !saveMode();
        if ((c = colOf(G.grd, 'FcyAmount'))) c.hide = !S.multi;
        if ((c = colOf(G.grd, 'TaxTypeId'))) c.edit = S.taxEditable;
        if ((c = colOf(G.grd, 'TaxPct'))) c.edit = S.taxEditable;
        if ((c = colOf(G.hd, 'FcyAmount'))) c.hide = !S.multi;
        $('DetailPanel').style.display = order ? 'none' : '';
        G.grd.refresh(); G.hd.refresh();
    }

    /* grd_CellUpdated / grd_UpdatingCell: the edited cell sets the sticky calc flag, the server runs the formulas */
    function taxPct(id) { var p = 0; S.taxTypes.forEach(function (x) { if (+x.TaxNameId === id) p = +x.TaxPercent || 0; }); return p; }
    function gridEdit(row, key, val) {
        var order = +row.OrderId > 0;
        if (key === 'WarehouseId' || key === 'CityId') { row[key] = +val || 0; row.calc = row.calc || 'tax'; }
        else if (key === 'GpNo' || key === 'GpDate' || key === 'VehicleNo') {
            if (!order) return;                                                              // editable only when OrderId > 0
            if (key === 'GpNo') { if (!/^\s*-?\d+\s*$/.test(String(val))) return msg('Please enter a valid number.'); row.GpNo = SE.toInt(val); }
            else if (key === 'GpDate') { var dd = readDate(val); if (!dd) return; row.GpDate = dd; }
            else row.VehicleNo = val;
            row.calc = row.calc || 'tax';
        } else if (key === 'DiscountType') { row.DiscountType = +val || 0; row.calc = 'amt'; }
        else if (key === 'TaxTypeId') {
            if (!S.taxEditable) return;
            var id = +val || 0; row.TaxTypeId = id; row.TaxPct = id === 0 ? 0 : taxPct(id); row.calc = row.calc || 'tax';
        } else {
            if (val !== '' && !isNumeric(val)) return;
            var n = SE.toNum(val);
            if (key === 'TaxPct') {
                if (!S.taxEditable) return;
                row.TaxPct = n;
                if (n === 0 && +row.TaxTypeId > 0) row.TaxPct = taxPct(+row.TaxTypeId);
                if (row.TaxPct > 100) { row.TaxPct = 100; msg("Tax % Can't be greater than 100..."); }
                row.calc = row.calc || 'tax';
            } else { row[key] = n; row.calc = 'amt'; }                                       // ItemQty, AddLessRate, Discount%
        }
        G.grd.refresh(); recalc();
    }
    function afterRowsChanged() { gridUi(); return recalc(); }
    function dupRow(row) { var c = copyRow(row); c.Id = 0; G.grd.addRow(c); return afterRowsChanged(); }
    function delRow(i) {
        return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) { if (yes) { G.grd.removeAt(i); return afterRowsChanged(); } });
    }
    function gridBtn(k, row, i) { if (k === '_add') return dupRow(row); return delRow(i); }

    /* grdInvExp */
    function expBlank() { return { ItemId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' }; }
    function expBtn(k, row, i) {
        if (k === '_add') { G.exp.addRow(expBlank()); }
        else if (G.exp.count() > 1) { G.exp.removeAt(i); }
        else { G.exp.setRows([expBlank()]); }
        recalc();
    }
    function expEdit(row, key, val) {
        if (key === 'ItemId') row.ItemId = +val || 0;
        else if (key === 'Remarks') row.Remarks = val;
        else {
            if (val !== '' && !isNumeric(val)) return msg('Please Type Only Numeric Value', 'Warning!');
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
            var so = orderId();
            if (row.PaymentTermId === 3 && so > 0) {
                return SE.api(API + '/remaining' + SE.q({ orderId: so, recId: S.id })).then(function (r) {
                    var a = +r.balanceAmount || 0;
                    if (a > bill) { a = bill; msg('Amount Cant be Greater than BillAmount'); }
                    row.Amount = a; row['%ofTotal'] = bill > 0 ? rnd(a * 100 / bill, 8) : 0; G.pay.refresh();
                }).catch(fail);
            }
            row.Amount = 0; G.pay.refresh(); return;
        }
        if (key === '%ofTotal') {
            if (!isNumeric(val)) { G.pay.refresh(); return msg('Please Type Only Numeric Value', 'Warning!'); }
            if (G.grd.count() === 0) { G.pay.refresh(); return; }
            var p = SE.toNum(val);
            if (p > 100) { p = 100; msg("%of Total Can't Greater than 100"); }
            row['%ofTotal'] = p; row.Amount = rnd(bill * p / 100, 4);
        } else if (key === 'Amount') {
            if (!isNumeric(val)) { G.pay.refresh(); return msg('Please Type Only Numeric Value', 'Warning!'); }
            if (G.grd.count() === 0) { G.pay.refresh(); return msg('No Detail Record Found'); }
            var a2 = SE.toNum(val);
            if (a2 > bill) { a2 = bill; msg('Amount Cant be Greater than BillAmount'); }
            row.Amount = a2; row['%ofTotal'] = bill > 0 ? rnd(a2 * 100 / bill, 8) : 0;
        } else if (key === 'DueDays') {
            if (val !== '' && !isNumeric(val)) { G.pay.refresh(); return msg('Please Type Only Numeric Value', 'Warning!'); }
            row.DueDays = SE.toInt(val); row.DueDate = addDays($('DocDate').value, row.DueDays);
        } else if (key === 'DueDate') {
            var dd = readDate(val);
            if (!dd) { G.pay.refresh(); return; }
            if (dd < $('DocDate').value) { row.DueDate = $('DocDate').value; G.pay.refresh(); return msg("Due Date Can't less Than DocDate"); }
            row.DueDate = dd; row.DueDays = dayDiff(dd, $('DocDate').value);
        }
        G.pay.refresh();
    }

    /* grdGLedger_CellUpdated */
    function glBlank() { return { AccountId: 0, GlAccountId: 0, Remarks: '', Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0 }; }
    function glBtn(k, row, i) {
        if (k === '_add') { G.gl.addRow(glBlank()); recalc(); return; }
        if (G.gl.count() > 1) G.gl.removeAt(i); else G.gl.setRows([glBlank()]);
        recalc();
    }
    function glEdit(row, key, val) {
        var r = S.fmt.amountRound;
        if (key === 'Remarks') { row.Remarks = val; G.gl.refresh(); return; }
        if (key === 'AccountId') {
            var id = +val || 0, a = null; S.fr.forEach(function (x) { if (+x.K === id) a = x; });
            if (id > 0 && a) {
                if (!S.sub && S.custGl > 0 && +a.Id === S.custGl) { G.gl.refresh(); return msg('Customer Account Not select'); }
                if (S.sub && id === vid(cb.cust)) { G.gl.refresh(); return msg('Selected Account Can not be Same As Supplier Account'); }
            }
            row.AccountId = id; row.GlAccountId = a ? +a.Id : 0;
        } else {
            if (!isNumeric(val)) { G.gl.refresh(); return msg('Please Type Only Numeric Value', 'Warning!'); }
            var n = SE.toNum(val);
            if (key === 'Qty' || key === 'Rate') {
                row[key] = n; row.Credit = rnd((+row.Qty || 0) * (+row.Rate || 0), r); row.Debit = 0; row.Percentage = 0;
            } else if (key === 'Percentage') {
                var tot = 0; G.gl.rows().forEach(function (x) { tot += x === row ? n : (+x.Percentage || 0); });
                if (tot > 100) { n = n - (tot - 100); msg('TotalPercentage Can not be Greater than 100...'); }
                row.Percentage = n;
                var amt = itemTotal() / 100 * n;
                if (amt > 0) { row.Credit = rnd(amt, r); row.Debit = 0; } else { row.Debit = Math.abs(rnd(amt, r)); row.Credit = 0; }
                row.Qty = 0; row.Rate = 0;
            } else if (key === 'Credit') {
                row.Credit = rnd(n, r);
                if ((+row.Debit || 0) > 0) { row.Credit = 0; msg('Debit Side is aleady added'); }
            } else if (key === 'Debit') {
                row.Debit = rnd(n, r);
                if ((+row.Credit || 0) > 0) { row.Debit = 0; msg('Credit Side is aleady added'); }
            }
        }
        G.gl.refresh(); recalc();
    }

    /* ------------------------------------------------------------------ Load Order (toolStripButton3 -> LoadSaleOrderWithQty) */
    function loadOrderClick() {
        if (G.grd.count() === 0 || orderId() > 0) return openLoader();
        return msg("You Can't Load Sale Order Because Direct Entry Already Exist");
    }
    function openLoader() {
        var h = '<div class="lbar"><button type="button" id="ldSearch">Search</button><button type="button" id="ldLoad">Load</button><button type="button" id="ldReset">Reset</button></div>' +
            '<div class="lf" style="position:relative;height:44px"><span class="dl" style="left:7px;top:14px">From Date</span><input class="f" type="date" id="ldFrom" style="left:70px;top:10px;width:120px;height:22px">' +
            '<span class="dl" style="left:200px;top:14px">To Date</span><input class="f" type="date" id="ldTo" style="left:255px;top:10px;width:120px;height:22px">' +
            '<span class="dl" style="left:390px;top:14px">Doc No From</span><input class="f" id="ldNoFrom" style="left:465px;top:10px;width:70px;height:22px">' +
            '<span class="dl" style="left:545px;top:14px">Doc No To</span><input class="f" id="ldNoTo" style="left:610px;top:10px;width:70px;height:22px">' +
            '<span class="dl" style="left:695px;top:14px">Customer Name</span><select class="f" id="ldCust" style="left:785px;top:8px;width:230px;height:26px"></select></div>' +
            '<div class="lgrid" id="ldGrd" style="height:380px"></div>';
        var pop = SE.pop('Sale Order Load', h, [{ t: 'Close' }], { wide: true });
        pop.open();
        var $$ = function (id) { return pop.body.querySelector('#' + id); };
        var cc = XCombo($$('ldCust'), { columns: [{ key: 'Customer', caption: 'Customer Name' }], valueKey: 'Id', textKey: 'Customer', popupWidth: 380 });
        SE.api(API + '/loader/combos', { quiet: true }).then(function (d) { cc.setData(d.customers || []); }).catch(function () { });
        SE.digitsOnly($$('ldNoFrom')); SE.digitsOnly($$('ldNoTo'));
        var g = SE.grid($$('ldGrd'), { footer: false, frozen: 1, dec: S.fmt.amount, cols: [
            { k: 'Sel', t: 'Select', w: 44, sel: true }, { k: 'DocDate', t: 'DocDate', w: 85, f: 'date' }, { k: 'DocNo', t: 'DocNo', w: 60 },
            { k: 'CustomerName', t: 'CustomerName', w: 200 }, { k: 'SupplierRefNo', t: 'SupplierRefNo', w: 90 }, { k: 'PaymentTerm', t: 'PaymentTerm', w: 90 }, { k: 'DueDays', t: 'DueDays', w: 60 },
            { k: 'OrderDueDate', t: 'OrderDueDate', w: 85, f: 'date' }, { k: 'DeliveryTerm', t: 'DeliveryTerm', w: 80 }, { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'JobLot', t: 'JobLot', w: 80 },
            { k: 'PackUom', t: 'PackUom', w: 60 }, { k: 'ItemQty', t: 'ItemQty', w: 80, f: 'n3' }, { k: 'DispatchQty', t: 'DispatchQty', w: 80, f: 'n3' }, { k: 'BalQty', t: 'BalQty', w: 80, f: 'n3' },
            { k: 'Weight', t: 'Weight', w: 80, f: 'n3' }, { k: 'DispatchWeight', t: 'DispatchWeight', w: 90, f: 'n3' }, { k: 'BalWeight', t: 'BalWeight', w: 80, f: 'n3' },
            { k: 'ItemRate', t: 'ItemRate', w: 80, f: 'n2' }, { k: 'RateUom', t: 'RateUom', w: 70 }, { k: 'CityName', t: 'CityName', w: 90 }, { k: 'TaxType', t: 'TaxType', w: 80 },
            { k: 'TaxPrcnt', t: 'TaxPrcnt', w: 70, f: 'n3' }, { k: 'TaxAmount', t: 'TaxAmount', w: 80, f: 'amt' }, { k: 'ItemAmount', t: 'ItemAmount', w: 90, f: 'amt' }] });
        function dates() { $$('ldFrom').value = SE.dateInput(S.fyStart) || SE.today(); $$('ldTo').value = SE.today(); }
        function search() {                                                                  // btngrnlod_Click -> PendingOrderLoad
            return SE.api(API + '/loader/rows' + SE.q({ fromDate: $$('ldFrom').value, toDate: $$('ldTo').value, fromNo: SE.toInt($$('ldNoFrom').value), toNo: SE.toInt($$('ldNoTo').value),
                customerId: +cc.value() || 0 })).then(function (rows) { g.setRows(rows); }).catch(function (e) { msg(e.message); });
        }
        dates();
        $$('ldSearch').onclick = search;
        $$('ldReset').onclick = function () { dates(); $$('ldNoFrom').value = ''; $$('ldNoTo').value = ''; cc.clear(); g.setRows([]); };
        $$('ldLoad').onclick = function () {                                                 // btnLoadOnInvoice_Click_1
            var picked = g.checked();
            if (!picked.length) return msg('Chek the row first');
            var sid = 0;
            for (var i = 0; i < picked.length; i++) {
                var oid = +picked[i].Id;
                if (oid !== 0) { if (sid === 0) sid = oid; if (sid !== oid) return msg('Sorry Check Rows Which Have Same Order No'); }
            }
            if (loadInGrid(picked)) pop.close();
        };
    }
    /* LoadInGridDetail + LoadPaymentDetailBySaleOrderIds */
    function loadInGrid(rows) {
        var o = rows[0], oid = +o.Id;
        if (G.grd.count() > 0 && orderId() !== oid) { msg("Already Loaded Row's Have Different Order. So you Can't Load Rows Of Different Order!"); return false; }
        cb.cust.setValue(+o.SupplierCustomerId || 0); enable(cb.cust, false); custChanged();
        $('txtRefrenceNo').value = o.SupplierRefNo || '';
        var pt = String(o.PaymentTerm || '').trim(); S.terms.forEach(function (x) { if (String(x.TermsDescription).trim() === pt) cb.term.setValue(+x.Id); });
        termChanged();
        if (o.DueDays != null && (cb.term.text() || '').trim() !== 'Cash') { $('txtDueDays').value = String(o.DueDays); dueDaysChanged(); }
        var dtm = String(o.DeliveryTerm || '').trim(); S.dterms.forEach(function (x) { if (x.DeliveryTerm === dtm) cb.dterm.setValue(+x.Id); });
        $('txtRemarks').value = o.RemarksHeader || '';
        var have = {}; G.grd.rows().forEach(function (r) { have[+r.OrderDetailId] = 1; });
        rows.forEach(function (r) { if (!have[+r.OrderDetailId]) { have[+r.OrderDetailId] = 1; G.grd.rows().push(copyRow(r.line)); } });
        gridUi();
        var paySum = G.pay.rows().reduce(function (a, r) { return a + (+r.Amount || 0); }, 0);
        var p = Promise.resolve();
        if (paySum === 0) {
            p = SE.api(API + '/order-payments' + SE.q({ orderIds: String(oid) })).then(function (pay) {
                var list = pay.map(function (x) { x.DueDate = SE.dateInput(x.DueDate) || $('DocDate').value; return x; });
                G.pay.setRows(list.length ? list : [payBlank()]);
            });
        }
        p.then(recalc).catch(fail);
        return true;
    }

    /* ------------------------------------------------------------------ Save / Update / Delete */
    function insert(update) {                                                                // Insert()
        return SE.api(API + '/validate', { method: 'POST', body: reqBody(false), quiet: true })
            .then(function () { return SE.ask(update ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm'); })
            .then(function (yes) {
                if (!yes) return;
                return SE.api(API, { method: 'POST', body: reqBody(true) }).then(function (r) {
                    return msg(r.message || 'Record Save Successfully').then(function () {
                        var slip = $('ChkBok').checked, vch = $('ChkVoucherPreview').checked;
                        return reset().then(function () {
                            if (slip) printSlip(r.id, r.docNo);
                            if (vch) printVoucher(r.voucherHeadId);
                        });
                    });
                });
            }).catch(function (e) { return msg(e.message, 'Message'); });
    }
    function btnSave() { S.id = 0; return insert(false); }
    function btnUpdate() { return insert(true); }
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
        ['btnSaleInvoice1', 'btnSaleinvoice2', 'btnPrint'].forEach(function (id) { $(id).disabled = !S.rights.print; });
        gridUi();
    }

    /* ------------------------------------------------------------------ Reset / Refresh */
    function reset() {                                                                       // Reset()
        S.id = 0; S.files = []; S.removedAtt = []; S.existing = []; S.voucherHeadId = 0; S.updateIdx = -1;
        enable(cb.cust, true); cb.cust.clear(); cb.frCr.clear(); cb.frDr.clear();
        ['txtbillno', 'txtFreightAmount', 'txtFreightRemarks', 'txtExchangeRate', 'txtRemarks', 'txtBillAmount'].forEach(function (id) { $(id).value = ''; });
        G.grd.setRows([]); G.exp.setRows([expBlank()]); G.gl.setRows([glBlank()]); G.pay.setRows([payBlank()]);
        detailButtons(false); setButtons(); configDefault();
        cb.cust.focus();
        return SE.api(API + '/next-no').then(function (r) { if (r.nextNo > 0) $('txtdocno').value = String(r.nextNo); }).catch(fail);
    }
    function btnNew() { return reset().then(resetDetail); }
    function refresh() {                                                                     // btnFrmRefresh_Click
        return SE.api(API + '/refresh' + SE.q({ date: $('DocDate').value })).then(function (d) { applyLists(d); multiCurrency(); fillAll(); configDefault(); }).catch(fail);
    }

    /* ------------------------------------------------------------------ ReadById */
    function edit(id) {
        return SE.api(API + '/' + id).then(function (d) {
            var h = d.head;
            tabs.select('tabForm');
            S.id = id; S.files = []; S.removedAtt = []; S.existing = d.attachments || []; S.voucherHeadId = +d.voucherHeadId || 0; S.updateIdx = -1;
            $('txtdocno').value = h.DocNo; $('DocDate').value = SE.dateInput(h.DocDate);
            cb.cust.setValue(+h.SupplierCustomerId || 0); enable(cb.cust, false);
            var cr = custRow(); S.custGl = cr ? +cr.GlAccountId || 0 : 0; $('txtSupplierGLId').value = String(S.custGl);
            $('txtbillno').value = h.ManualBillNo || '';
            var dtm = (h.DeliveryTerm || '').trim(); cb.dterm.clear(); S.dterms.forEach(function (x) { if (x.DeliveryTerm === dtm) cb.dterm.setValue(x.Id); });
            cb.term.setValue(+h.PaymentTermId || 0); termChanged();
            if ((cb.term.text() || '').trim() !== 'Cash') $('txtDueDays').value = h.DueDays == null ? '' : String(h.DueDays);
            $('DueDate').value = SE.dateInput(h.DueDate) || $('DocDate').value;
            if (+h.TaxAccountId > 0) cb.tax.setValue(+h.TaxAccountId);
            if (+h.TransporterId > 0) cb.frCr.setValue(S.sub ? (+h.TransporterCreditPartyId || 0) : (+h.TransporterId || 0));
            if (+h.TransporterDebitGLId > 0) cb.frDr.setValue(S.sub ? (+h.TransporterDebitPartyId || 0) : (+h.TransporterDebitGLId || 0));
            $('txtFreightAmount').value = f3(+h.FreightAmount || 0); $('txtFreightRemarks').value = h.FreightRemark || '';
            $('txtBillAmount').value = f4(+h.BillAmount || 0);
            cb.cur.setValue(+h.CurrencyId || 0);
            $('txtFcyAmount').value = fmtFcy(+h.FcyAmount || 0);
            $('txtRemarks').value = h.RemarksHeader || '';
            $('txtDiscountAmountHeader').value = f4(+h.DiscountAmount || 0);
            if (+h.DiscountAccountId > 0) cb.disc.setValue(+h.DiscountAccountId);
            if (+h.CurrencyId > 0) $('txtExchangeRate').value = h.ExchangeRate == null ? '' : String(h.ExchangeRate); else configDefault();
            G.grd.setRows(d.lines || []);
            G.exp.setRows((d.expenses && d.expenses.length) ? d.expenses : [expBlank()]);
            G.gl.setRows((d.journals && d.journals.length) ? d.journals : [glBlank()]);
            G.pay.setRows((d.payments && d.payments.length) ? d.payments.map(function (p) { p.DueDate = SE.dateInput(p.DueDate) || $('DocDate').value; return p; }) : [payBlank()]);
            detailButtons(false); setButtons();
            return recalc();                                                                 // FreightProportion, ExpProportion, LedgerProportion, BillAmount, txtExchangeRate_TextChanged
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ history */
    function showHistory() {                                                                 // GetAll
        var dt = $('rdentrydate').checked ? 'entry' : ($('rdmodifydate').checked ? 'modify' : ($('rdapproveddate').checked ? 'approved' : 'document'));
        var q = { dateType: dt, fromDate: $('FromDateHistory_chk').checked ? $('FromDateHistory').value : '', toDate: $('ToDateHistory_chk').checked ? $('ToDateHistory').value : '',
            fromNo: SE.toInt($('FromDocNoHistory').value), toNo: SE.toInt($('ToDocNoHistory').value), customerId: vid(cb.hCust), paymentTermId: vid(cb.hTerm) };
        return SE.api(API + '/history' + SE.q(q)).then(function (rows) {
            G.hist.setRows(rows.map(function (r) {
                return { Id: ci(r, 'Id'), VoucherHeadId: ci(r, 'VoucherHeadId'), DocNo: ci(r, 'DocNo'), DocDate: ci(r, 'DocDate'), DueDate: ci(r, 'DueDate'), ManualBillNo: ci(r, 'ManualBillNo'),
                    CustomerName: pick(r, ['CustomerName', 'SupplierCustomer', 'Customer']), BillAmount: ci(r, 'BillAmount'), EntryUser: pick(r, ['UserName', 'EntryUser']), EntryDate: ci(r, 'EntryDate'),
                    ModifyUser: pick(r, ['ModifyUserName', 'ModifyUser']), ModifyDate: ci(r, 'ModifyDate'), ApprovedUser: pick(r, ['ApprovedUserName', 'ApprovedUser']),
                    ApprovedDate: pick(r, ['ApprovedDate', 'PostDate']), NoOfAttachments: ci(r, 'NoOfAttachments'), Remarks: pick(r, ['RemarksHeader', 'Remarks']) };
            }));
        }).catch(function (e) { return msg(e.message); });
    }
    function resetHistory() {                                                                // btnResetHistory_Click
        var t = SE.today();
        $('FromDateHistory').value = t; $('ToDateHistory').value = t; $('FromDocNoHistory').value = ''; $('ToDocNoHistory').value = '';
        cb.hCust.clear(); cb.hTerm.clear(); G.hist.setRows([]); G.hd.setRows([]); $('drdocdate').checked = true;
    }
    function refreshHistoryCombos() {                                                        // BtnRefreshHistory_Click -> HistoryComboFill
        return SE.api(API + '/history-combos').then(function (h) { cb.hCust.setData(h.customers || []); cb.hTerm.setData(h.terms || []); }).catch(function (e) { return msg(e.message); });
    }
    function histSelected(item) {                                                            // grdHistory_SelectionChanged -> BindDetailOfHeaderId
        if (!item) { G.hd.setRows([]); return; }
        SE.api(API + '/' + item.Id + '/history-detail', { quiet: true }).then(function (rows) {
            if (!G.hist.cur() || +G.hist.cur().Id !== +item.Id) return;
            G.hd.setRows(rows);
        }).catch(function () { G.hd.setRows([]); });
    }
    function histEdit(r) { return edit(+r.Id); }
    function histBtn(k, r) {
        if (k === 'Edit') histEdit(r);
        else if (k === 'Print1') printSlip(+r.Id, +r.DocNo);
        else if (k === 'Print2') printSlip2(+r.Id, +r.DocNo);
        else if (k === 'Voucher') printVoucher(+r.VoucherHeadId);
    }
    function histLink(k, r) { if (k === 'NoOfAttachments') showAttachments(+r.Id); }

    /* ------------------------------------------------------------------ print / attachments */
    function slipArgs(id, no) { return { id: id, pbmId: id, documentTypeId: DOC, branchesId: S.branchId, fromDocNo: no || 0, toDocNo: no || 0 }; }
    function printSlip(id, no) {                                                             // GeneratePrint1608
        if (!(id > 0)) return msg('No Record Selected');
        SE.printRpt('1608-SaleInvoice_CustomerBillDirect_Engr.rpt', slipArgs(id, no));
    }
    function printSlip2(id, no) {                                                            // GeneratePrint1608A
        if (!(id > 0)) return msg('No Record Selected');
        SE.printRpt('1608A-SaleInvoice_CustomerBillDirect_Engr.rpt', slipArgs(id, no));
    }
    function printVoucher(vhId) {                                                            // AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 1608)
        if (!(vhId > 0)) return msg('No Record Found For Display');
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
    var SHORT = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
        ['Ctrl+P', 'For Print'], ['Alt+1', 'For Print Slip 1608'], ['Alt+2', 'For Print Slip 1608A'], ['Alt+3', 'For Print Voucher 103'], ['Ctrl+F5', 'For Focus on Doc Date Name'],
        ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+D', 'For Adding an row in Focused Grid'],
        ['Ctrl+Delete', 'For Deleting an row of Focused Grid'], ['Ctrl+ArrowDown', 'For Focus Grids'], ['Ctrl+ArrowUp', 'For Focus On Customer Name in Detail Box'],
        ['Ctrl+ArrowRight', 'For Change Focus from one Grid To another Grid'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    var ORDER = [['DetailGrid', 'grd'], ['tabPaymentDetail', 'grdPaymentTerm'], ['tabExpenses', 'grdInvExp'], ['tabPage2', 'grdGLedger']];
    function gridOf(id) { return id === 'grd' ? G.grd : id === 'grdInvExp' ? G.exp : id === 'grdGLedger' ? G.gl : id === 'grdPaymentTerm' ? G.pay : null; }
    function focusTab2(i) { var o = ORDER[i]; tabs2.select(o[0]); $(o[1]).focus(); }
    function curTab2() { var i = tabs2.index(); return i < 0 ? 0 : i; }
    function addRowIn(id) {
        if (id === 'grdInvExp') { G.exp.addRow(expBlank()); recalc(); }
        else if (id === 'grdGLedger') { G.gl.addRow(glBlank()); recalc(); }
        else if (id === 'grdPaymentTerm') G.pay.addRow(payBlank());
        else if (id === 'grd' && G.grd.cur()) dupRow(G.grd.cur());
    }
    function onKey(e) {
        var t = e.target;
        if (e.key === 'Enter' && !e.ctrlKey && t && (t.tagName === 'INPUT' || t.tagName === 'SELECT') && t.type !== 'checkbox' && t.type !== 'radio' && t.type !== 'button') {
            var f = Array.prototype.filter.call(document.querySelectorAll('.dform input:not([type=hidden]):not([disabled]), .dform select:not([disabled]), .dform .dtcombo-input'), function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(t); if (i >= 0 && f[i + 1]) { e.preventDefault(); f[i + 1].focus(); }
        }
        var k = (e.key || '').toLowerCase();
        if (e.altKey && !e.ctrlKey && tabs.index() === 0) {
            if (e.key === '1') { e.preventDefault(); if (!$('btnSaleInvoice1').disabled) printSlip(S.id, +$('txtdocno').value); }
            else if (e.key === '2') { e.preventDefault(); if (!$('btnSaleinvoice2').disabled) printSlip2(S.id, +$('txtdocno').value); }
            else if (e.key === '3') { e.preventDefault(); if (!$('btnPrint').disabled) printVoucher(S.voucherHeadId); }
            return;
        }
        if (!e.ctrlKey) return;
        if (k === 'e') { e.preventDefault(); try { window.history.back(); } catch (x) { } return; }
        if (k === 't') { e.preventDefault(); if (tabs.index() === 1) { tabs.select('tabForm'); $('DocDate').focus(); } else { tabs.select('tabHistory'); $('FromDateHistory').focus(); } return; }
        if (tabs.index() === 0) {
            var tg = t.closest ? t.closest('.dgrid') : null, gid = tg ? tg.id : '';
            if (e.key === 'Delete' && e.shiftKey) { e.preventDefault(); if (isShown('btnDelete') && !$('btnDelete').disabled) btnDelete(); }
            else if (e.key === 'Delete') {
                var g = gridOf(gid);
                if (g && g.curIndex() >= 0) {
                    e.preventDefault();
                    if (g === G.grd) { if (saveMode()) delRow(g.curIndex()); }
                    else if (g === G.exp) expBtn('_del', g.cur(), g.curIndex());
                    else if (g === G.gl) glBtn('_del', g.cur(), g.curIndex());
                    else payBtn('_del', g.cur(), g.curIndex());
                }
            }
            else if (k === 'n') { e.preventDefault(); btnNew(); }
            else if (k === 'r') { e.preventDefault(); refresh(); }
            else if (k === 's') { e.preventDefault(); if (isShown('btnSave') && !$('btnSave').disabled) btnSave(); }
            else if (k === 'u') { e.preventDefault(); if (isShown('btnUpdate') && !$('btnUpdate').disabled) btnUpdate(); }
            else if (k === 'p') { e.preventDefault(); if (!$('btnSaleInvoice1').disabled) printSlip(S.id, +$('txtdocno').value); }
            else if (k === 'd') { if (gid) { e.preventDefault(); addRowIn(gid); } }
            else if (e.key === 'F5') { e.preventDefault(); cb.cust.focus(); }
            else if (e.key === 'F10') { e.preventDefault(); openAttachmentDialog(); }
            else if (e.key === 'ArrowDown') { e.preventDefault(); $(ORDER[curTab2()][1]).focus(); }
            else if (e.key === 'ArrowUp') { e.preventDefault(); cb.wh.focus(); }
            else if (e.key === 'ArrowRight') { e.preventDefault(); focusTab2((curTab2() + 1) % ORDER.length); }
        } else {
            if (k === 's') { e.preventDefault(); showHistory(); }
            else if (e.key === 'ArrowDown') { e.preventDefault(); $('grdHistory').focus(); }
            else if (e.key === 'ArrowUp') { e.preventDefault(); $('FromDateHistory').focus(); }
            else if (e.key === 'ArrowRight') { e.preventDefault(); var tgh = t.closest ? t.closest('.dgrid') : null; if (tgh && tgh.id === 'grdHistory') $('grdDetail').focus(); else $('grdHistory').focus(); }
            else if (k === 'p') { e.preventDefault(); var hc = G.hist.cur(); if (hc && S.rights.print) printSlip(+hc.Id, +hc.DocNo); }
            else if (e.key === 'Enter') { var hr = G.hist.cur(), tg2 = t.closest ? t.closest('.dgrid') : null; if (hr && tg2 && tg2.id === 'grdHistory' && S.rights.update) { e.preventDefault(); histEdit(hr); } }
        }
    }
    function wire() {
        $('btnNew').onclick = btnNew; $('btnFrmRefresh').onclick = refresh; $('btnSave').onclick = btnSave; $('btnUpdate').onclick = btnUpdate; $('btnDelete').onclick = btnDelete;
        $('btnAttachment').onclick = openAttachmentDialog;
        $('btnSaleInvoice1').onclick = function () { printSlip(S.id, +$('txtdocno').value); };
        $('btnSaleinvoice2').onclick = function () { printSlip2(S.id, +$('txtdocno').value); };
        $('btnPrint').onclick = function () { printVoucher(S.voucherHeadId); };
        $('toolStripButton3').onclick = loadOrderClick;
        $('btnshortcutkeys').onclick = function () { SE.shortcuts(SHORT); };
        $('btnResetHistory').onclick = resetHistory; $('BtnRefreshHistory').onclick = refreshHistoryCombos; $('btnShow').onclick = showHistory;
        $('btnAdd').onclick = btnAdd; $('btnUpdateDetail').onclick = btnUpdateDetail; $('btnCancelUpdateDetial').onclick = btnCancelUpdateDetail;
        $('txtdocno').readOnly = true;
        ['txtDueDays', 'FromDocNoHistory', 'ToDocNoHistory', 'txtgatepassno'].forEach(function (id) { SE.digitsOnly($(id)); });
        ['txtFreightAmount', 'txtDiscountAmountHeader', 'txtExchangeRate', 'txtItemQty', 'txtRate', 'txtAddLessRate', 'txtDiscPercent', 'txtTaxPercent'].forEach(function (id) {
            $(id).addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[\d.\-]/.test(e.key) && !e.ctrlKey) e.preventDefault(); });
        });
        $('txtFreightAmount').addEventListener('input', recalc);
        $('txtDiscountAmountHeader').addEventListener('input', discountHeaderChanged);
        $('txtExchangeRate').addEventListener('input', recalc);
        $('txtExchangeRate').addEventListener('blur', function () { if ($('txtExchangeRate').value !== '') $('txtExchangeRate').value = fmtRate(num('txtExchangeRate')); });
        $('txtItemQty').addEventListener('input', calcAll);
        $('txtRate').addEventListener('input', calcAmountTax);
        $('txtRate').addEventListener('blur', function () { if ($('txtRate').value !== '') $('txtRate').value = fmtRate(num('txtRate')); });       // txtRate_Leave
        $('txtAddLessRate').addEventListener('input', function () { calcWeight(); calcAmountTax(); });
        $('txtDiscPercent').addEventListener('input', calcAmountTax);
        $('txtTaxPercent').addEventListener('input', calcTax);
        $('txtDueDays').addEventListener('input', dueDaysChanged);
        $('DueDate').addEventListener('change', dueDateChanged);
        $('DocDate').addEventListener('change', docDateLeave);
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
        var hook = false;
        document.addEventListener('keydown', function (e) { if (e.ctrlKey && e.altKey && !hook) { hook = true; SE.shortcuts(SHORT); setTimeout(function () { hook = false; }, 400); } });
    }

    function load() {
        makeCombos(); makeGrids();
        tabs = SE.tabs('tabControl1', function (id) { if (id === 'tabHistory') $('FromDateHistory').focus(); else cb.cust.focus(); });
        tabs2 = SE.tabs('tabControl2');
        wire();
        SE.api(API + '/initial').then(function (d) {
            S.rights = d.rights || {}; S.branchId = d.branchId || 0; S.fyStart = d.fyStart || ''; S.lastDiscount = +d.lastDiscountAcId || 0; S.lastTaxAc = +d.lastTaxAcId || 0;
            applyLists(d); multiCurrency(); fillAll();
            cb.hCust.setData((d.history && d.history.customers) || []); cb.hTerm.setData((d.history && d.history.terms) || []);
            $('txtdocno').value = d.nextNo > 0 ? String(d.nextNo) : '';
            $('ChkBok').checked = true; $('ChkVoucherPreview').checked = false;
            var t = SE.today(); $('DocDate').value = t; $('DueDate').value = t; $('txtgpdate').value = t;
            $('FromDateHistory').value = t; $('ToDateHistory').value = t;
            G.exp.setRows([expBlank()]); G.gl.setRows([glBlank()]); G.pay.setRows([payBlank()]);
            defaultsAtLoad(); configDefault(); detailButtons(false); setButtons();
            if (S.lastDiscount > 0) cb.disc.setValue(S.lastDiscount);
            if (S.lastTaxAc > 0) cb.tax.setValue(S.lastTaxAc);
            cb.cust.focus();
            var rec = SE.param('record'); if (rec) edit(+rec);
        }).catch(fail);
    }
    document.addEventListener('DOMContentLoaded', load);
})();
