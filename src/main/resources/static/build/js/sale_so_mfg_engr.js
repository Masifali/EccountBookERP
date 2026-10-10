/*
 * Screen 779  frmSaleOrderEngr  (Architecture.WinApp.Mfg.Sale.frmSaleOrderEngr, Sale Engr module 134, document type 1656)
 * Page script. Desktop methods are named in the comments (frmSaleOrderEngr.cs). Server: /sale/engr/mfg/sale-order/api
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/engr/mfg/sale-order/api', DOC = 1656;
    var S = {
        id: 0, rights: {}, remove: [], updateIdx: -1, files: [], removedAtt: [], existing: [],
        cust: [], cats: [], casting: [], terms: [], modals: [], items: [], cities: [], curr: [], multi: false, taxEditable: false, noRate: false,
        fmt: { amountRound: 0, amount: 0, rate: 2, fcy: 0 }, def: {}, baseCurrency: 0, baseRate: 0, branchId: 0, bill: 2,
        hb: [], hbChecked: {}, branchImplemented: false, branchFeature: false, userBranchName: '', historyDays: 0
    };
    var cb = {}, G = {}, tabs, tabs2;
    var DTERMS = [{ Id: 1, DeliveryTerm: 'Load' }, { Id: 2, DeliveryTerm: 'Ponch' }];
    var STATUS = [{ Id: 1, Status: 'Open' }, { Id: 2, Status: 'Complete' }, { Id: 3, Status: 'Cancel' }];

    function fail(e) { return SE.dbError(e); }
    function msg(t, c) { return SE.alert(t, c); }
    function field(id) { return ($(id).value || '').trim(); }
    function enable(c, on) { c.el.disabled = !on; var w = c.el.__dtcombo; if (w && w.syncFromSelect) w.syncFromSelect(); }
    function isShown(id) { var b = $(id); return !!b && b.style.display !== 'none' && !b.hidden; }
    function show(id, on) { var b = $(id); if (!b) return; b.style.display = on ? '' : 'none'; if (b.hasAttribute('hidden')) b.hidden = !on; }
    function vid(c) { return +c.value() || 0; }
    function ctrlEnable(id, on) { $(id).disabled = !on; }

    /* ------------------------------------------------------------------ number formats (clsGlobalVariables) */
    function fmtAmt(v) { var d = S.fmt.amount; return SE.num(v, d, d); }                      // stringFormatsingle
    function fmtRate(v) { var d = S.fmt.rate; return SE.num(v, d, d); }                       // DecimalRateFormate
    function fmtFcy(v) { var d = S.fmt.fcy; return SE.num(v, d, d); }                         // stringFormatsingleForFcy
    function f3(v) { return SE.num(v, 3, 0); }                                               // "#,##0.###"
    function f4(v) { return SE.num(v, 4, 0); }                                               // "#,##0.####"
    function f2(v) { return SE.num(v, 2, 0); }                                               // "#,##0.##"
    /* FillItemFinishWeight: ToString("#,##.##") - no digit placeholder before the point, so 0 gives '' and 0.5 gives '.5' */
    function fWeight(v) { v = +v || 0; if (v === 0) return ''; var s = SE.num(v, 2, 0); return s.indexOf('0.') === 0 ? s.substring(1) : s; }
    function roundAway(v, dec) { var p = Math.pow(10, Math.max(0, dec)); return (v < 0 ? -1 : 1) * Math.round(Math.abs(v) * p + 1e-9) / p; }
    function num(id) { return SE.toNum($(id).value); }
    function iso(d) { return d.getFullYear() + '-' + SE.pad2(d.getMonth() + 1) + '-' + SE.pad2(d.getDate()); }
    function parseIso(s) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(s || ''); return m ? new Date(+m[1], +m[2] - 1, +m[3]) : null; }
    function addDays(s, n) { var d = parseIso(s) || new Date(); d.setDate(d.getDate() + n); return iso(d); }
    function dayDiff(a, b) { var x = parseIso(a), y = parseIso(b); return x && y ? Math.round((x - y) / 86400000) : 0; }
    var MON = { jan: 1, feb: 2, mar: 3, apr: 4, may: 5, jun: 6, jul: 7, aug: 8, sep: 9, oct: 10, nov: 11, dec: 12 };
    /* DateTime cell editor input -> yyyy-mm-dd ('' when it is not a date) */
    function readDate(t) {
        t = String(t || '').trim(); var m;
        if ((m = /^(\d{4})-(\d{1,2})-(\d{1,2})/.exec(t))) return m[1] + '-' + SE.pad2(+m[2]) + '-' + SE.pad2(+m[3]);
        if ((m = /^(\d{1,2})[-\/ ]([A-Za-z]{3})[A-Za-z]*[-\/ ](\d{4})/.exec(t)) && MON[m[2].toLowerCase()]) return m[3] + '-' + SE.pad2(MON[m[2].toLowerCase()]) + '-' + SE.pad2(+m[1]);
        if ((m = /^(\d{1,2})[-\/](\d{1,2})[-\/](\d{4})/.exec(t))) return m[3] + '-' + SE.pad2(+m[2]) + '-' + SE.pad2(+m[1]);
        return '';
    }

    /* ------------------------------------------------------------------ combos */
    function makeCombos() {
        cb.cust = XCombo('combsuppname', { onSelect: customerLeave, columns: [{ key: 'CompanyName', caption: 'Customer Name' }, { key: 'PartyCode', caption: 'PartyCode' }, { key: 'CityName', caption: 'CityName' }], textKey: 'CompanyName', popupWidth: 460 });
        cb.term = XCombo('combpttrm', { columns: [{ key: 'TermsDescription', caption: 'Payment Term' }], textKey: 'TermsDescription', onSelect: termChanged });
        cb.dterm = XCombo('combdeliverytrm', { columns: [{ key: 'DeliveryTerm', caption: 'Delivery Term' }], textKey: 'DeliveryTerm' });
        cb.status = XCombo('CmbStatus', { columns: [{ key: 'Status', caption: 'Order Status' }], textKey: 'Status' });
        cb.cur = XCombo('cmbCurrency', { columns: [{ key: 'CurrencyCode', caption: 'Currency' }], textKey: 'CurrencyCode', onSelect: currencyLeave });
        cb.cat = XCombo('CmbCategory', { columns: [{ key: 'CategoryDescription', caption: 'ItemCategory' }], textKey: 'CategoryDescription', popupWidth: 300, onSelect: categoryLeave });
        cb.modal = XCombo('CmbSampleModal', { columns: [{ key: 'SampleNo', caption: 'SampleNo' }, { key: 'ItemModal', caption: 'ItemModal' }], textKey: 'SampleNo', popupWidth: 380, onSelect: modalLeave });
        cb.item = XCombo('CmbItem', { columns: [{ key: '_t', caption: 'Item Name' }, { key: '_o', caption: 'Item Code' }], textKey: '_t', popupWidth: 460, onSelect: itemLeave });
        cb.uom = XCombo('CmbPackUom', { columns: [{ key: 'UOMCode', caption: 'Uom' }, { key: 'Equivalent', caption: 'Equivalent' }], textKey: 'UOMCode', onSelect: calcAmountTax });
        cb.variant = XCombo('CmbVariantDescription', { columns: [{ key: 'VariantDescription', caption: 'VariantDescription' }], valueKey: 'VariantId', textKey: 'VariantDescription', popupWidth: 300 });
        cb.casting = XCombo('CmbCastingTypeDetail', { columns: [{ key: 'LookupName', caption: 'Casting Type' }], textKey: 'LookupName', popupWidth: 350 });
        cb.rate = XCombo('CmbRateUom', { columns: [{ key: 'UOMCode', caption: 'RateUom' }, { key: 'Equivalent', caption: 'Equivalent' }], textKey: 'UOMCode', onSelect: calcAmountTax });
        cb.disc = XCombo('CmbDiscountType', { columns: [{ key: 'Name', caption: 'Discount Type' }], textKey: 'Name', onSelect: calcAmountTax });
        cb.tax = XCombo('CmbTaxType', { columns: [{ key: 'TaxType', caption: 'TaxType' }], textKey: 'TaxType', onSelect: calcTax });
        cb.city = XCombo('CmbCity', { columns: [{ key: 'CityName', caption: 'City Name' }], textKey: 'CityName', popupWidth: 320 });
        cb.hCust = XCombo('CmbCustomerHistory', { columns: [{ key: 'Customer', caption: 'Party' }], valueKey: 'Id', textKey: 'Customer', popupWidth: 380 });
    }

    /* ------------------------------------------------------------------ fills (SupplierNameFill ... CurrencyFill) */
    function keepFill(c, rows, keepId) { c.setData(rows); if (keepId > 0 && !c.setValue(keepId)) c.clear(); }
    function fillCustomers() {                                                               // SupplierNameFill: the previous customer is kept
        var id = vid(cb.cust);
        cb.cust.setData(S.cust);
        if (id > 0) cb.cust.setValue(id);
    }
    function fillCategories() { keepFill(cb.cat, S.cats, vid(cb.cat)); }                      // GetItemCategory
    function fillCasting() { var id = vid(cb.casting); cb.casting.setData(S.casting); if (id > 0) cb.casting.setValue(id); }   // CastingType
    function fillTerms() {                                                                   // PaymentTerms: BindDDLNew(..., false) + Rows[1].Activate()
        var id = vid(cb.term);
        cb.term.setData(S.terms);
        if (S.terms.length > 1) cb.term.setValue(S.terms[1].Id);
        if (id > 0 && S.terms.some(function (t) { return +t.Id === id; })) cb.term.setValue(id);
        termChanged();
    }
    function fillModals(cat, itemId) {                                                       // ItemModalBind(CategoryId, itemId)
        var id = vid(cb.modal), rows;
        if (itemId > 0) rows = S.modals.filter(function (m) { return +m.ItemId === itemId; });
        else if (cat > 0) rows = S.modals.filter(function (m) { return +m.ItemCategoryId === cat; });
        else rows = S.modals;
        if (rows.length) {
            cb.modal.setData(rows);
            if (id > 0 && rows.some(function (m) { return +m.Id === id; })) cb.modal.setValue(id); else cb.modal.setValue(+rows[0].Id);     // Rows[1].Activate()
        } else { cb.modal.clear(); cb.modal.setData([]); }
    }
    function itemRows(rows) {
        var byName = $('rdbtnItemName').checked;
        return rows.map(function (r) { return { Id: r.Id, _t: byName ? r.ItemName : r.ItemCode, _o: byName ? r.ItemCode : r.ItemName, ItemName: r.ItemName, ItemCode: r.ItemCode, ItemCategoryId: r.ItemCategoryId, itemModalId: r.itemModalId, FinishWeight: r.FinishWeight }; });
    }
    function fillItems(cat, modal) {                                                         // ItemDetailBind(CategoryId, itemModalId)
        var id = vid(cb.item), rows;
        if (modal > 0) rows = S.items.filter(function (i) { return +i.itemModalId === modal; });
        else if (cat > 0) rows = S.items.filter(function (i) { return +i.ItemCategoryId === cat; });
        else rows = S.items;
        if (rows.length) {
            cb.item.setData(itemRows(rows));
            if (id > 0 && rows.some(function (r) { return +r.Id === id; })) cb.item.setValue(id); else cb.item.setValue(+rows[0].Id);       // Rows[1].Activate()
        } else { cb.item.clear(); cb.item.setData([]); }
    }
    function fillCities() { keepFill(cb.city, S.cities, vid(cb.city)); }                      // CityFill
    function fillCurrencies() { keepFill(cb.cur, S.curr, vid(cb.cur)); }                      // CurrencyFill
    /* MultiCurrencyFeature */
    function multiCurrency() {
        ['cmbCurrency', 'txtExchangeRate', 'txtFcyAmount', 'label35', 'label32', 'label29'].forEach(function (id) { var e = $(id); if (e) e.style.visibility = S.multi ? '' : 'hidden'; });
        var c = $('cmbCurrency'); if (c && c.__dtcombo && c.__dtcombo.wrap) c.__dtcombo.wrap.style.visibility = S.multi ? '' : 'hidden';
        [G.grd, G.hd].forEach(function (g) {
            var col = g && g.spec.cols.filter(function (x) { return x.k === 'FcyAmount'; })[0]; if (col) col.hide = !S.multi;
            if (g) g.refresh();
        });
    }
    /* ConfigurationDefault (the Job/Lot part is not needed: the job lot combo is hidden and JobLotId is 0 on every line) */
    function configDefault() {
        S.baseCurrency = S.def.baseCurrency || 0; S.baseRate = S.def.baseRate || 0;
        if (S.baseCurrency > 0 && !(vid(cb.cur) > 0)) cb.cur.setValue(S.baseCurrency);
        if (S.baseRate > 0 && num('txtExchangeRate') === 0) $('txtExchangeRate').value = fmtRate(S.baseRate);
    }
    function applyLists(d) {
        S.cust = d.customers || []; S.cats = d.categories || []; S.casting = d.castingTypes || []; S.terms = d.terms || []; S.modals = d.modals || []; S.items = d.items || [];
        S.cities = d.cities || []; S.curr = d.currencies || []; S.multi = !!d.multiCurrency; S.taxEditable = !!d.taxEditable; S.noRate = !!d.insertWithoutRate;
        S.def = d.defaults || {}; S.fmt = d.fmt || S.fmt;
        G.grd.spec.dec = S.fmt.amount; G.hist.spec.dec = S.fmt.amount; G.hd.spec.dec = S.fmt.amount;
    }

    /* ------------------------------------------------------------------ calculations */
    function calcTax() {                                                                     // CalculateTaxAmount
        var amt = num('txtItemAmount'), tr = cb.tax.row();
        if (tr && vid(cb.tax) > 0) {
            var pct;
            if (!(num('txtTaxPercent') > 0)) { pct = +tr.TaxPrcnt || 0; $('txtTaxPercent').value = f3(pct); } else pct = num('txtTaxPercent');
            var t = amt * pct / 100;
            $('txtTaxAmount').value = f4(t);
            $('txtTotalAmount').value = f4(amt + t);
        } else if (isShown('btnsave') && !$('btnsave').disabled) {
            $('txtTaxPercent').value = '0'; $('txtTaxAmount').value = '0'; $('txtTotalAmount').value = f4(num('txtItemAmount'));
        }
    }
    function calcAmount() {                                                                  // CalculateAmount
        var ru = cb.rate.row();
        if (ru && (+cb.rate.value() > 0)) {
            var eq = +ru.Equivalent || 0, qty = num('txtqty'), rate = num('txtRate'), pct = num('txtDiscPercent'), da = 0;
            if (qty > 0 && rate > 0 && eq > 0) {
                var amount = qty / eq * rate, dt = vid(cb.disc);
                if (dt === 1) da = pct > 0 ? pct : 0;
                else if (dt === 2) {
                    if (pct > 100) { msg("Discount Percent Can't Greater Than 100.", 'WARNING!!!'); $('txtDiscPercent').value = '100'; pct = 100; }
                    da = pct > 0 ? amount * pct / 100 : 0;
                } else { da = 0; $('txtDiscPercent').value = '0'; $('txtDiscountAmount').value = '0'; }
                $('txtDiscountAmount').value = f3(da);
                amount -= da;
                var r = fmtAmt(roundAway(amount, S.fmt.amountRound));
                $('txtItemAmount').value = r; $('txtTotalAmount').value = r;
            } else { $('txtItemAmount').value = '0'; $('txtTotalAmount').value = '0'; }
        } else { $('txtItemAmount').value = '0'; $('txtTotalAmount').value = '0'; }
    }
    function calcAmountTax() { calcAmount(); calcTax(); }

    /* CalculateTotalInformation + txtExchangeRate_TextChanged */
    function calcTotals() {
        var rows = G.grd.rows();
        if (rows.length > 0) {
            var q = 0, w = 0, a = 0, t = 0, f = 0;
            rows.forEach(function (r) { q += +r.ItemQty || 0; w += +r.Weight || 0; a += +r.Amount || 0; t += +r.TaxAmount || 0; f += +r.FcyAmount || 0; });
            $('txtOrderQty').value = f2(roundAway(q, 2)); $('txtOrderWeight').value = f2(roundAway(w, 2));
            $('txtItemNetAmountHeader').value = fmtAmt(a); $('txtTaxAmountHeader').value = fmtAmt(t);
            $('txtOrderAmountHeader').value = fmtAmt(a - num('txtDiscountHeader'));
            $('txtFcyAmount').value = fmtFcy(f);
        } else {
            ['txtOrderQty', 'txtOrderWeight', 'txtItemNetAmountHeader', 'txtTaxAmountHeader', 'txtOrderAmountHeader', 'txtFcyAmount'].forEach(function (id) { $(id).value = '0'; });
        }
    }
    function exchangeChanged() {
        var rate = num('txtExchangeRate');
        G.grd.rows().forEach(function (r) { r.FcyAmount = rate > 0 ? (+r.Amount || 0) / rate : 0; });
        G.grd.refresh();
        calcTotals();
    }
    /* PaymentTermAmountCalculateFromPercent */
    function payFromPercent() {
        var order = num('txtOrderAmountHeader'), rows = G.pay.rows();
        if (order > 0 && rows.length) {
            rows.forEach(function (r) { var p = +r['%ofTotal'] || 0; r.Amount = p > 0 ? roundAway(order * p / 100, 2) : 0; });
            G.pay.refresh();
        }
    }
    function discountHeaderChanged() { exchangeChanged(); payFromPercent(); }                 // txtDiscountHeader_TextChanged

    /* ------------------------------------------------------------------ header events */
    function termChanged() {                                                                 // combpttrm_TextChanged
        var t = (cb.term.text() || '').trim();
        if (t === 'Cash') { $('txtduedays').disabled = true; $('duedate').disabled = true; $('txtduedays').value = ''; $('duedate').value = $('DocDate').value; }
        else { $('txtduedays').disabled = false; $('duedate').disabled = false; }
    }
    function dueDaysChanged() {                                                              // txtduedays_TextChanged
        var t = $('txtduedays').value;
        $('duedate').value = t !== '' ? addDays($('DocDate').value, SE.toInt(t)) : $('DocDate').value;
    }
    function dueDateChanged() {                                                              // duedate_ValueChanged
        if (!$('duedate').value) return;
        if ($('duedate').value < $('DocDate').value) $('duedate').value = $('DocDate').value;
        $('txtduedays').value = String(dayDiff($('duedate').value, $('DocDate').value));
    }
    function deliveryStartChanged() {                                                        // deliverystartdate_ValueChanged
        if ($('deliverystartdate').value < $('DocDate').value) { $('deliverystartdate').value = $('DocDate').value; return msg("Delivery Start Date Can't Be Less Than Doc Date"); }
    }
    function docDateLeave() {                                                                // DocDate_Leave
        var p = Promise.resolve();
        if (G.grd.count() > 0) {
            var ids = ''; G.grd.rows().forEach(function (r) { ids += ',' + r.ItemId; });
            p = SE.api(API + '/tax-by-items' + SE.q({ itemIds: ids, date: $('DocDate').value })).then(function (dt) {
                G.grd.rows().forEach(function (it) {
                    var hit = null; dt.forEach(function (x) { if (+x.ItemId === +it.ItemId) hit = x; });
                    var taxAmount = 0, pct = 0;
                    if (hit) {
                        it.TaxNameId = +hit.TaxNameId || 0; it.TaxName = hit.TaxName == null ? '' : String(hit.TaxName); pct = +hit.TaxPercent || 0; it.TaxPercent = pct;
                        var ia = +it.Amount || 0;
                        if (ia > 0 && pct > 0) { taxAmount = ia * pct / 100; it.TaxAmount = taxAmount; }
                        it.TotalAmount = ia + taxAmount;
                    } else { it.TaxNameId = 0; it.TaxName = '0'; it.TaxPercent = 0; it.TaxAmount = 0; it.TotalAmount = it.Amount; }
                });
                G.grd.refresh(); calcTotals();
            });
        }
        return p.then(function () {
            var warn = $('deliverystartdate').value < $('DocDate').value;
            $('deliverystartdate').value = $('DocDate').value;
            dueDaysChanged();
            if (warn) return msg("Delivery Start Date Can't Be Less Than Doc Date");
        }).catch(fail);
    }
    function currencyLeave() {                                                               // cmbCurrency_Leave
        if (vid(cb.cur) === 0 || num('txtExchangeRate') !== 0) return;
        if (vid(cb.cur) !== S.baseCurrency) {
            return SE.api(API + '/last-rate' + SE.q({ currencyId: vid(cb.cur) })).then(function (dt) {
                $('txtExchangeRate').value = dt.length ? fmtRate(+dt[0].LastExchRate || 0) : '0';
                exchangeChanged();
            }).catch(fail);
        }
        $('txtExchangeRate').value = fmtRate(S.baseRate); exchangeChanged();
    }
    /* combsuppname_Leave -> BindCustomerCity */
    function customerLeave() {
        $('txtCustomerCity').value = '';
        var r = cb.cust.row();
        if (r && vid(cb.cust) > 0) {
            var cn = r.CityName == null ? '' : String(r.CityName);
            $('txtCustomerCity').value = cn;
            if (cn && !(vid(cb.city) > 0)) { var hit = S.cities.filter(function (c) { return String(c.CityName) === cn; })[0]; if (hit) cb.city.setValue(+hit.Id); }
        }
    }

    /* ------------------------------------------------------------------ item / category / modal / UOM / tax */
    function byCode(rows, code) { for (var i = 0; i < rows.length; i++) if (String(rows[i].UOMCode) === String(code)) return rows[i]; return null; }
    function bindRateUomAndPackUom(itemId) {                                                  // bindRateUomAndItemPackUom (ZeroIndex false, Rows[0])
        var pu = cb.uom.text(), ru = cb.rate.text();
        cb.uom.clear(); cb.rate.clear();
        return SE.api(API + '/uoms' + SE.q({ itemId: itemId })).then(function (rows) {
            if (rows.length) {
                cb.uom.setData(rows); cb.rate.setData(rows);
                var a = pu ? byCode(rows, pu) : null, b = ru ? byCode(rows, ru) : null;
                cb.uom.setValue(+(a || rows[0]).Id); cb.rate.setValue(+(b || rows[0]).Id);
            } else { cb.uom.clear(); cb.rate.clear(); cb.uom.setData([]); cb.rate.setData([]); }
        });
    }
    function bindVariants(itemId) {                                                          // bindvarientunit (ZeroIndex true, Rows[1])
        var prev = cb.variant.text();
        return SE.api(API + '/variants' + SE.q({ itemId: itemId })).then(function (rows) {
            if (rows.length) {
                cb.variant.setData(rows);
                var hit = prev ? rows.filter(function (v) { return String(v.VariantDescription) === prev; })[0] : null;
                cb.variant.setValue(+(hit || rows[0]).VariantId);
            } else { cb.variant.clear(); cb.variant.setData([]); }
        });
    }
    function itemLeave() {                                                                   // combitem_Leave_1
        var itemId = vid(cb.item), ir = cb.item.row();
        return bindRateUomAndPackUom(itemId).then(function () { return bindVariants(itemId); }).then(function () {
            if (ir && itemId > 0) { var fw = fWeight(ir.FinishWeight); $('txtweight').value = fw; }   // FillItemFinishWeight
            return taxForItem();
        }).then(function () {
            fillModals(0, itemId);
            if (ir && itemId > 0 && +ir.ItemCategoryId > 0) cb.cat.setValue(+ir.ItemCategoryId);
        }).catch(fail);
    }
    function categoryLeave() { var c = vid(cb.cat); fillItems(c, 0); fillModals(c, 0); }       // CmbCategory_Leave
    function modalLeave() {                                                                  // CmbSampleModal_Leave
        var r = cb.modal.row();
        if (r && vid(cb.modal) > 0 && +r.ItemCategoryId > 0) cb.cat.setValue(+r.ItemCategoryId);
        fillItems(vid(cb.cat), vid(cb.modal));
    }
    function taxForItem() {                                                                  // GetTaxTypeIdAndTaxPercent
        return SE.api(API + '/item-tax' + SE.q({ itemId: vid(cb.item), date: $('DocDate').value })).then(function (dt) {
            if (dt.length) cb.tax.setData([{ Id: dt[0].TaxNameId, TaxType: dt[0].TaxName, TaxPrcnt: dt[0].TaxPercent }]);
            else { cb.tax.clear(); cb.tax.setData([]); $('txtTaxPercent').value = '0'; $('txtTaxAmount').value = '0'; }
            if (S.updateIdx > -1) { var cur = G.grd.rows()[S.updateIdx]; if (cur && +cur.TaxNameId > 0) cb.tax.setValue(+cur.TaxNameId); }
            calcTax();
        });
    }

    /* ------------------------------------------------------------------ detail grid (btnplus_Click / FormValidationDetails ...) */
    function detailValid() {
        function stop(m, f) { msg(m); if (f) f(); return false; }
        if (!cb.item.row() || !(vid(cb.item) > 0)) return stop('Item Name Field is Required', function () { cb.item.focus(); });
        if (!cb.uom.row() || !(vid(cb.uom) > 0)) return stop('Pack Uom Field is Required', function () { cb.uom.focus(); });
        if (num('txtqty') === 0) return stop('Item Qty Field is Required', function () { $('txtqty').focus(); });
        if (!S.noRate && num('txtRate') === 0) return stop('Item Rate Field is Required', function () { $('txtRate').focus(); });
        if (!cb.rate.row() || !(vid(cb.rate) > 0)) return stop('Rate UOM Field is Required', function () { cb.rate.focus(); });
        if (!cb.city.row() || !(vid(cb.city) > 0)) return stop('City Field is Required', function () { cb.city.focus(); });
        return true;
    }
    function newRowFromEntry(id, old) {
        var ir = cb.item.row(), amount = num('txtItemAmount'), ex = num('txtExchangeRate');
        return {
            Id: id, ItemId: vid(cb.item), ItemCode: ir.ItemCode, ItemName: ir.ItemName, PackUomId: vid(cb.uom), PackUom: cb.uom.text(),
            VariantId: vid(cb.variant), VariantDescription: cb.variant.text(), CastingTypeId: vid(cb.casting), CastingType: cb.casting.text(),
            Remarks: $('txtremarksdetail').value, ItemQty: num('txtqty'), Weight: num('txtweight'), ItemRate: num('txtRate'), RateUomId: vid(cb.rate), RateUom: cb.rate.text(),
            DiscountType: vid(cb.disc), 'Discount%': num('txtDiscPercent'), DiscountAmount: num('txtDiscountAmount'), Amount: amount,
            FcyAmount: ex > 0 && amount > 0 ? amount / ex : 0, TaxNameId: vid(cb.tax), TaxName: cb.tax.text(), TaxPercent: num('txtTaxPercent'), TaxAmount: num('txtTaxAmount'),
            TotalAmount: num('txtTotalAmount'), CityId: vid(cb.city), CityName: cb.city.text(),
            ReferredInGDN: old ? !!old.ReferredInGDN : false, ReferredInInvoice: old ? !!old.ReferredInInvoice : false
        };
    }
    function clearAfterRow() {                                                               // the fields btnplus_Click / btnUpdateDetail_Click clear
        cb.item.clear(); $('txtremarksdetail').value = ''; cb.uom.clear(); $('txtqty').value = ''; $('txtweight').value = '';
        cb.rate.clear(); $('txtRate').value = ''; $('txtItemAmount').value = ''; cb.tax.clear(); $('txtTaxPercent').value = ''; $('txtTaxAmount').value = ''; $('txtTotalAmount').value = '';
    }
    function plus() {
        if (!detailValid()) return;
        G.grd.addRow(newRowFromEntry(0));
        clearAfterRow(); cb.item.focus();
        exchangeChanged(); payFromPercent();
    }
    function editRow(i) {                                                                    // grd_DoubleClick
        var r = G.grd.rows()[i]; if (!r) return;
        S.updateIdx = i;
        cb.cat.clear(); categoryLeave();
        cb.item.setValue(+r.ItemId);
        itemLeave().then(function () {
            cb.uom.setValue(+r.PackUomId); cb.variant.setValue(+r.VariantId || 0); cb.casting.setValue(+r.CastingTypeId || 0);
            $('txtqty').value = f3(r.ItemQty); $('txtweight').value = f3(r.Weight);
            $('txtRate').value = fmtRate(r.ItemRate);
            cb.rate.setValue(+r.RateUomId);
            cb.disc.setValue(+r.DiscountType || 0);
            $('txtDiscPercent').value = f3(r['Discount%']); $('txtDiscountAmount').value = fmtAmt(r.DiscountAmount);
            $('txtItemAmount').value = fmtAmt(r.Amount);
            cb.tax.setValue(+r.TaxNameId || 0);
            $('txtTaxPercent').value = f4(r.TaxPercent); $('txtTaxAmount').value = fmtAmt(r.TaxAmount); $('txtTotalAmount').value = fmtAmt(r.TotalAmount);
            $('txtremarksdetail').value = r.Remarks || '';
            cb.city.setValue(+r.CityId || 0);
            show('btnplus', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
            cb.item.focus();
            fieldsDisable(!!r.ReferredInGDN, !!r.ReferredInInvoice);
        }).catch(fail);
    }
    function updateDetail() {                                                                // btnUpdateDetail_Click
        if (!detailValid()) return;
        var rows = G.grd.rows(), old = rows[S.updateIdx]; if (!old) return;
        rows[S.updateIdx] = newRowFromEntry(old.Id, old); G.grd.refresh();
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        clearAfterRow(); S.updateIdx = -1; cb.item.focus();
        exchangeChanged(); payFromPercent();
    }
    function cancelUpdateDetail() {                                                          // btnCancelUpdateDetial_Click (updateDetailIndex is left as it is, as on the desktop)
        cb.item.clear(); $('txtremarksdetail').value = ''; cb.uom.clear(); cb.variant.clear(); cb.casting.clear(); $('txtqty').value = ''; $('txtweight').value = '';
        cb.rate.clear(); $('txtRate').value = ''; $('txtItemAmount').value = ''; cb.tax.clear(); $('txtTaxPercent').value = ''; $('txtTaxAmount').value = ''; $('txtTotalAmount').value = '';
        cb.item.focus();
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
    }
    function lineOf(r) {
        return { id: +r.Id || 0, itemId: +r.ItemId || 0, packUomId: +r.PackUomId || 0, variantId: +r.VariantId || 0, castingTypeId: +r.CastingTypeId || 0,
            remarks: r.Remarks == null ? '' : String(r.Remarks), itemQty: +r.ItemQty || 0, weight: +r.Weight || 0,
            itemRate: +r.ItemRate || 0, rateUomId: +r.RateUomId || 0, discountType: +r.DiscountType || 0, discountPercent: +r['Discount%'] || 0, discountAmount: +r.DiscountAmount || 0,
            amount: +r.Amount || 0, taxNameId: +r.TaxNameId || 0, taxPercent: +r.TaxPercent || 0, taxAmount: +r.TaxAmount || 0, totalAmount: +r.TotalAmount || 0,
            cityId: +r.CityId || 0, cityName: r.CityName == null ? '' : String(r.CityName) };
    }
    function deleteRow(i) {                                                                  // grd_ColumnButtonClick "Delete" -> DeleteDetailRow
        var r = G.grd.rows()[i]; if (!r) return;
        if (S.updateIdx !== -1) return msg('Please Reset the Detail First...');
        return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
            if (!yes) return;
            if ((+r.Id || 0) > 0) S.remove.push(lineOf(r));
            G.grd.removeAt(G.grd.rows().indexOf(r));
            calcTotals(); payFromPercent();
        });
    }
    /* HeaderFieldsDisableWhenOrderDetailReferred / FieldsDisableWhenOrderDetailReferred */
    function headerDisable(gdn, inv) {
        var on = gdn || inv;
        enable(cb.cust, !on); enable(cb.dterm, !on); ctrlEnable('txtdeliverydays', !on); ctrlEnable('deliverystartdate', !on);
        ctrlEnable('txtDiscountHeader', on ? !inv : true);
    }
    function fieldsDisable(gdn, inv) {
        var on = gdn || inv;
        [cb.cat, cb.modal, cb.item, cb.uom, cb.variant, cb.casting].forEach(function (c) { enable(c, !on); });
        ctrlEnable('txtremarksdetail', !on); ctrlEnable('txtqty', !on);
        var rateOff = on && inv;
        ctrlEnable('txtRate', !rateOff); enable(cb.disc, !rateOff); ctrlEnable('txtDiscPercent', !rateOff); enable(cb.tax, !rateOff);
        $('txtTaxPercent').disabled = rateOff ? true : !S.taxEditable;
        enable(cb.rate, !on); enable(cb.city, !on);
    }

    /* ------------------------------------------------------------------ payment grid */
    function addPaymentRows() {                                                              // AddPaymentRowsRowsInPaymentGrid: one row per payment term
        var rows = G.pay.rows();
        S.terms.forEach(function (t) {
            if (!rows.some(function (r) { return +r.PaymentTermId === +t.Id; })) G.pay.addRow({ PaymentTermId: +t.Id, '%ofTotal': 0, Amount: 0, DueDays: 0, DueDate: SE.today(), Remarks: '' });
        });
    }
    function payEdit(r, k, v) {                                                              // grdPaymentTerm_UpdatingCell / _CellUpdated
        try {
            if (k === '%ofTotal' && !/^\s*-?\d*\.?\d*\s*$/.test(String(v))) { return msg('Please Type Only Numeric Value'); }
            if (k === 'PaymentTermId') { r[k] = +v || 0; return; }
            if (k === 'Remarks') { r[k] = v; return; }
            if (k === 'DueDate') {
                var d = readDate(v); if (!d) return;
                if (d < $('DocDate').value) { r.DueDate = $('DocDate').value; return msg("Due Date Can't less Than DocDate"); }
                r.DueDate = d; r.DueDays = dayDiff(d, $('DocDate').value); return;
            }
            var n = SE.toNum(v);
            if (k === '%ofTotal') {
                r[k] = n;
                if (G.grd.count() > 0) {
                    var total = num('txtOrderAmountHeader');
                    if (n > 100) { msg("%of Total Can't Greater than 100", 'Warning Message'); r[k] = 100; n = 100; }
                    r.Amount = roundAway(total * n / 100, 4);
                }
            } else if (k === 'Amount') {
                r[k] = n;
                if (G.grd.count() <= 0) { return msg('No Detail Record Found'); }
                var bill = num('txtOrderAmountHeader');
                if (bill < n) { msg('Amount Cant be Greater than Order Amount', 'Warning Message'); r.Amount = bill; n = bill; }
                r['%ofTotal'] = bill > 0 ? roundAway(n * 100 / bill, 8) : 0;
            } else if (k === 'DueDays') {
                r.DueDays = Math.trunc(n); r.DueDate = addDays($('DocDate').value, Math.trunc(n));
            }
        } catch (e) { msg(e.message); }
    }

    /* ------------------------------------------------------------------ grids */
    function makeGrids() {
        G.grd = SE.grid('grd', { footer: true, dec: 0, frozen: 1, onDbl: function (r, i) { editRow(i); },
            onBtn: function (k, r, i) { if (k === 'Delete') deleteRow(i); },
            cols: [
                { k: 'Delete', t: 'X', w: 28, btn: 'X' }, { k: 'Id', hide: true }, { k: 'ItemId', hide: true }, { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'ItemName', t: 'ItemName', w: 200 },
                { k: 'PackUomId', hide: true }, { k: 'PackUom', t: 'PackUom', w: 70 }, { k: 'VariantId', hide: true }, { k: 'VariantDescription', t: 'Modal Description', w: 150 },
                { k: 'CastingTypeId', hide: true }, { k: 'CastingType', t: 'CastingType', w: 130 }, { k: 'Remarks', t: 'Specification/Remarks', w: 200 },
                { k: 'ItemQty', t: 'Qty', w: 80, f: 'n3', sum: true }, { k: 'Weight', t: 'Weight', w: 80, f: 'n3', sum: true },
                { k: 'ItemRate', t: 'ItemRate', w: 80, render: function (v) { return v == null ? '' : fmtRate(v); } }, { k: 'RateUomId', hide: true }, { k: 'RateUom', t: 'RateUom', w: 70 },
                { k: 'DiscountType', t: 'DiscountType', w: 60, list: function () { return [{ Id: 1, Name: 'Flat' }, { Id: 2, Name: 'Percent' }]; }, lk: 'Id', lt: 'Name' },
                { k: 'Discount%', t: 'Discount%', w: 80, render: function (v) { return v == null ? '' : fmtRate(v); } },
                { k: 'DiscountAmount', t: 'DiscountAmount', w: 100, f: 'amt', sum: true }, { k: 'Amount', t: 'Amount', w: 100, f: 'amt', sum: true },
                { k: 'FcyAmount', t: 'FcyAmount', w: 100, f: 'amt', sum: true, hide: true }, { k: 'TaxNameId', hide: true }, { k: 'TaxName', t: 'TaxName', w: 90 },
                { k: 'TaxPercent', t: 'TaxPercent', w: 80, render: function (v) { return v == null ? '' : f3(v); } }, { k: 'TaxAmount', t: 'TaxAmount', w: 90, f: 'amt', sum: true },
                { k: 'TotalAmount', t: 'TotalAmount', w: 100, f: 'amt', sum: true }, { k: 'CityId', hide: true }, { k: 'CityName', t: 'CityName', w: 120 },
                { k: 'ReferredInGDN', hide: true }, { k: 'ReferredInInvoice', hide: true }] });
        G.pay = SE.grid('grdPaymentTerm', { footer: true, dec: 4, frozen: 0, onEdit: function (r, k, v) { payEdit(r, k, v); G.pay.refresh(); },
            cols: [
                { k: 'PaymentTermId', t: 'Payment Term', w: 120, edit: true, list: function () { return S.terms; }, lk: 'Id', lt: 'TermsDescription' },
                { k: '%ofTotal', t: '%ofTotal', w: 90, f: 'amt', sum: true, edit: true, render: function (v) { return v == null ? '' : f4(v); } },
                { k: 'Amount', t: 'Amount', w: 120, f: 'amt', sum: true, edit: true, render: function (v) { return v == null ? '' : fmtFcy(v); } },
                { k: 'DueDays', t: 'DueDays', w: 80, f: 'n3', sum: true, edit: true }, { k: 'DueDate', t: 'DueDate', w: 110, f: 'date', edit: true }, { k: 'Remarks', t: 'Remarks', w: 250, edit: true }] });
        G.hist = SE.grid('grdhistory', { frozen: 3, dec: 0, onDbl: function (r) { histEdit(r); },
            onBtn: function (k, r) { if (k === 'Edit') histEdit(r); else if (k === 'Print') print(+r.Id); else if (k === 'SaveAs') histSaveAs(r); },
            onLink: function (k, r) { if (k === 'NoOfAttachments') showAttachments(+r.Id); },
            onSel: histSelected,
            cols: [
                { k: 'Edit', t: 'Edit', w: 40, btn: 'Edit' }, { k: 'Print', t: 'Print', w: 40, btn: 'Print' }, { k: 'SaveAs', t: 'SaveAs', w: 60, btn: 'SaveAs' }, { k: 'Id', hide: true },
                { k: 'DocNo', t: 'DocNo', w: 60 }, { k: 'BranchSrNo', t: 'BranchSrNo', w: 60, hide: true }, { k: 'BranchName', t: 'BranchName', w: 120, hide: true },
                { k: 'DocDate', t: 'DocDate', w: 85, f: 'sdate' }, { k: 'CustomerName', t: 'CustomerName', w: 220 },
                { k: 'ItemNetAmount', t: 'ItemNetAmount', w: 100, f: 'amt', sum: true }, { k: 'TaxAmount', t: 'TaxAmount', w: 90, f: 'amt', sum: true },
                { k: 'DiscountAmount', t: 'DiscountAmount', w: 100, f: 'amt', sum: true }, { k: 'OrderNetAmount', t: 'OrderNetAmount', w: 110, f: 'amt', sum: true },
                { k: 'PaymentTerms', t: 'PaymentTerms', w: 100 }, { k: 'DueDays', t: 'DueDays', w: 60 }, { k: 'DueDate', t: 'DueDate', w: 85, f: 'sdate' }, { k: 'DeliveryTerm', t: 'DeliveryTerm', w: 90 },
                { k: 'DeliveryDays', t: 'DeliveryDays', w: 80 }, { k: 'OrderStatus', t: 'OrderStatus', w: 80 }, { k: 'OrderExpiryDate', t: 'OrderExpiryDate', w: 100, f: 'sdate' },
                { k: 'RemarksHeader', t: 'RemarksHeader', w: 200 }, { k: 'EntryUser', t: 'EntryUser', w: 100 }, { k: 'EntryDate', t: 'EntryDate', w: 130, f: 'dt12' },
                { k: 'ModifyUser', t: 'ModifyUser', w: 100 }, { k: 'ModifyDate', t: 'ModifyDate', w: 130, f: 'dt12' }, { k: 'ApprovedUser', t: 'ApprovedUser', w: 100 },
                { k: 'ApprovedDate', t: 'ApprovedDate', w: 130, f: 'dt12' }, { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 110, link: true }] });
        G.hd = SE.grid('grdDetail', { footer: true, dec: 0,
            cols: [
                { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'PackUom', t: 'PackUom', w: 70 }, { k: 'VariantDescription', t: 'Modal Description', w: 150 },
                { k: 'CastingType', t: 'CastingType', w: 130 }, { k: 'Remarks', t: 'Specification/Remarks', w: 200 },
                { k: 'ItemQty', t: 'Qty', w: 80, f: 'n3', sum: true }, { k: 'Weight', t: 'Weight', w: 80, f: 'n3', sum: true },
                { k: 'ItemRate', t: 'ItemRate', w: 80, render: function (v) { return v == null ? '' : fmtRate(v); } }, { k: 'RateUom', t: 'RateUom', w: 70 }, { k: 'DiscountType', t: 'DiscountType', w: 80 },
                { k: 'Discount%', t: 'Discount%', w: 80, render: function (v) { return v == null ? '' : fmtRate(v); } }, { k: 'DiscountAmount', t: 'DiscountAmount', w: 100, f: 'amt', sum: true },
                { k: 'Amount', t: 'Amount', w: 100, f: 'amt', sum: true }, { k: 'FcyAmount', t: 'FcyAmount', w: 100, f: 'amt', sum: true, hide: true }, { k: 'TaxName', t: 'TaxName', w: 90 },
                { k: 'TaxPercent', t: 'TaxPercent', w: 80, render: function (v) { return v == null ? '' : f3(v); } }, { k: 'TaxAmount', t: 'TaxAmount', w: 90, f: 'amt', sum: true },
                { k: 'TotalAmount', t: 'TotalAmount', w: 100, f: 'amt', sum: true }, { k: 'CityName', t: 'CityName', w: 120 }] });
    }

    /* ------------------------------------------------------------------ Load / Reset / Refresh */
    function load() {
        makeCombos();
        tabs = SE.tabs('tabControl1', function (id) { if (id === 'tabPage2') $('FromDateHistory').focus(); else $('DocDate').focus(); });
        tabs2 = SE.tabs('tabControl2');
        ['txtduedays', 'txtdeliverydays', 'FromDocNoHistory', 'ToDocNoHistory'].forEach(function (id) { SE.digitsOnly($(id)); });
        ['txtqty', 'txtRate', 'txtExchangeRate', 'txtItemAmount', 'txtDiscPercent'].forEach(function (id) {
            $(id).addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[\d.]/.test(e.key) && !e.ctrlKey) e.preventDefault(); });
        });
        makeGrids();
        cb.dterm.setData(DTERMS); cb.dterm.setValue(1);                                    // DeliveryTerm(): Rows[0].Activate()
        cb.status.setData(STATUS); cb.status.setValue(1);                                  // OrderStatus(): Rows[0].Activate()
        cb.disc.setData([{ Id: 1, Name: 'Flat' }, { Id: 2, Name: 'Percent' }]); cb.disc.setValue(2);   // DiscountTypeFill: Rows[1].Activate()
        S.bill = 2;                                                                       // BillTypeComboFill: Rows[1] = "On Qty"
        SE.api(API + '/initial').then(function (d) {
            applyLists(d);
            S.rights = d.rights || {}; S.branchId = d.branchId || 0;
            var h = d.history || {};
            S.hb = h.branches || []; S.branchImplemented = !!h.branchImplemented; S.branchFeature = !!h.branchFeature; S.userBranchName = h.userBranchName || ''; S.historyDays = d.historyDays || 0;
            multiCurrency();
            fillCurrencies(); fillCasting(); fillCustomers(); fillCategories(); fillTerms();
            fillModals(0, 0); fillItems(0, 0); fillCities(); configDefault();
            $('txtdocno').value = d.nextNo > 0 ? String(d.nextNo) : '';
            $('txtTaxPercent').disabled = !S.taxEditable;
            addPaymentRows();
            $('btnsave').disabled = !S.rights.save; $('btnprint').disabled = !S.rights.print; $('btnupdate').disabled = !S.rights.update;
            show('btnupdate', false); show('btnsave', true); show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false); show('BtnSaveAs', false);
            enable(cb.status, false);
            $('ChkBoxPrint').checked = true;
            var t = SE.today();
            $('DocDate').value = t; $('duedate').value = t; $('deliverystartdate').value = t; $('validate').value = t;
            $('FromDateHistory').value = SE.addDays(S.historyDays > 0 ? -S.historyDays : -3); $('ToDateHistory').value = t;
            $('txtduedays').value = ''; $('txtdeliverydays').value = '';
            historyBranchFill();
            historyCombosFill(false).catch(function () { });
            $('DocDate').focus();
            var rec = SE.param('record'); if (rec) { S.id = +rec; edit(+rec); }
        }).catch(fail);
        wire();
    }

    function reset() {                                                                       // Reset()
        S.files = []; S.removedAtt = []; S.existing = []; S.remove = []; S.id = 0; S.updateIdx = -1;
        cb.cust.clear(); S.bill = 0;                                                         // CmbBillType.Text = '' (BillCalculateTypeId becomes 0)
        ['txtsupprefno', 'txtduedays', 'txtItemNetAmountHeader', 'txtDiscountHeader', 'txtTaxAmountHeader', 'txtDiscPercent', 'txtDiscountAmount', 'txtqty', 'txtweight', 'txtRate',
            'txtItemAmount', 'txtTaxPercent', 'txtTaxAmount', 'txtTotalAmount', 'txtFcyAmount', 'txtOrderQty', 'txtOrderWeight', 'txtOrderAmountHeader', 'txtCustomerCity'].forEach(function (id) { $(id).value = ''; });
        cb.item.clear(); cb.uom.clear(); cb.rate.clear(); cb.tax.clear(); cb.casting.clear();
        show('btnsave', true); show('btnupdate', false); show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        configDefault();
        G.grd.setRows([]);
        G.pay.setRows([]); addPaymentRows();
        headerDisable(false, false); fieldsDisable(false, false);
        show('BtnSaveAs', false);
        enable(cb.status, false);
        $('DocDate').focus();
        return SE.api(API + '/next-no').then(function (r) { if (r.nextNo > 0) $('txtdocno').value = String(r.nextNo); }).catch(fail);
    }
    function refresh() {                                                                     // toolStripButton1_Click
        return SE.api(API + '/lists').then(function (d) {
            applyLists(d);
            multiCurrency();
            fillCustomers(); fillCategories(); fillModals(0, 0); fillItems(0, 0); fillCities(); fillCurrencies(); fillCasting(); configDefault(); addPaymentRows();
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ Insert */
    function formValid() {                                                                   // FormValidation()
        function stop(m, f) { return msg(m).then(function () { if (f) f(); return false; }); }
        if (!cb.cust.row() || !(vid(cb.cust) > 0)) return stop('Customer Name Field is Required', function () { cb.cust.focus(); });
        if (!cb.term.row() || !(vid(cb.term) > 0)) return stop('Payment Term Field is Required', function () { cb.term.focus(); });
        if ((cb.term.text() || '').trim() === 'Credit' && SE.toInt($('txtduedays').value) === 0) return stop('Due Days Field is Required', function () { $('txtduedays').focus(); });
        if (!cb.dterm.row() || !cb.dterm.text()) return stop('Delivery Term Field is Required', function () { cb.dterm.focus(); });
        if (S.multi) {
            if (!cb.cur.row() || !(vid(cb.cur) > 0)) return stop('Fcy Code Field is Required', function () { cb.cur.focus(); });
            if (field('txtExchangeRate') === '' || field('txtExchangeRate') === '0') return stop('Exchange Rate Field is Required', function () { $('txtExchangeRate').focus(); });
            if (!S.noRate && (field('txtFcyAmount') === '' || field('txtFcyAmount') === '0')) return stop('Fcy Amount Rate Field is Required', function () { $('txtFcyAmount').focus(); });
            return Promise.resolve(true);
        }
        if (!cb.cur.row() || !(vid(cb.cur) > 0)) return stop('Please Configure Your Base Currency In configurations', function () { cb.cur.focus(); });
        if (field('txtExchangeRate') === '' || field('txtExchangeRate') === '0') return stop('Please Configure Your Base Currency Rate In configurations', function () { $('txtExchangeRate').focus(); });
        return Promise.resolve(true);
    }
    function gridChecks() {                                                                  // the per-row checks of Insert()
        var rows = G.grd.rows();
        for (var i = 0; i < rows.length; i++) {
            var r = rows[i], dt = +r.DiscountType || 0;
            if (dt > 0 && !(+r['Discount%'] > 0) && !(+r.DiscountAmount > 0)) dt = 0;
            if (dt > 0 && +r['Discount%'] > 0 && !(+r.DiscountAmount > 0)) return 'Discount Amount Required In Row#' + (i + 1) + ' In Detail Grid';
            if (+r.TaxNameId > 0 && +r.TaxPercent > 0 && !(+r.TaxAmount > 0)) return 'TaxAmount Required In Row#' + (i + 1) + ' In Detail Grid';
        }
        return '';
    }
    function paymentChecks() {                                                               // the payment checks of Insert()
        var pays = G.pay.rows(), amt = 0, pct = 0, sum = 0, order = num('txtOrderAmountHeader');
        if (pays.length <= 0) { tabs2.select('tabPagePaymentDetail'); return 'Payment Detail Record Not Found'; }
        pays.forEach(function (p) { sum += +p.Amount || 0; });
        if (sum > 0) {
            for (var k = 0; k < pays.length; k++) {
                var p = pays[k];
                if (+p.Amount > 0) {
                    pct += +p['%ofTotal'] || 0; amt += +p.Amount || 0;
                    if (!(+p.PaymentTermId > 0)) return 'Payment Term Required in row#' + (k + 1);
                    if (+p.PaymentTermId === 2 && !(+p.DueDays > 0)) { tabs2.select('tabPagePaymentDetail'); return 'Due Days Required In case Of Credit row in row#' + (k + 1); }
                }
            }
        } else { pct = 100; amt = roundAway(order, 4); }
        if (roundAway(amt, 2) !== order) { tabs2.select('tabPagePaymentDetail'); return 'Payment Detail Amount ' + f4(amt) + ' Not Equal to Total Order Amount ' + f4(order); }
        if (roundAway(pct, 2) !== 100) { tabs2.select('tabPagePaymentDetail'); return 'Payment Detail Total% not equal to 100'; }
        return '';
    }
    function insert() {
        if (G.grd.count() === 0) return msg('Grid Record Not Found');
        return formValid().then(function (ok) {
            if (!ok) return;
            return SE.ask(S.id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
                if (!yes) return;
                var bad = gridChecks() || paymentChecks();
                if (bad) return msg(bad, /^Payment Detail|^Payment Term|^Due Days/.test(bad) ? 'Warning Message' : 'Message');
                var body = {
                    id: S.id, docDate: $('DocDate').value, docNo: field('txtdocno'), customerId: vid(cb.cust),
                    supplierRefNo: $('txtsupprefno').value, remarks: $('txtremarks').value, paymentTermId: vid(cb.term), dueDays: SE.toInt($('txtduedays').value),
                    dueDate: $('duedate').value, expiryDate: $('validate').value, deliveryTerm: cb.dterm.text(), deliveryStartDate: $('deliverystartdate').value,
                    deliveryDays: SE.toInt($('txtdeliverydays').value), status: cb.status.text(), billTypeId: S.bill, currencyId: vid(cb.cur),
                    exchangeRateText: field('txtExchangeRate'), fcyAmountText: field('txtFcyAmount'), exchangeRate: num('txtExchangeRate'), discountHeader: num('txtDiscountHeader'),
                    lines: G.grd.rows().map(lineOf), removed: S.id > 0 ? S.remove : [],
                    payments: G.pay.rows().map(function (r) { return { paymentTermId: +r.PaymentTermId || 0, percent: +r['%ofTotal'] || 0, amount: +r.Amount || 0, dueDays: +r.DueDays || 0,
                        dueDate: r.DueDate, remarks: r.Remarks == null ? '' : String(r.Remarks) }; }),
                    attachments: { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removedAtt }
                };
                return SE.api(API, { method: 'POST', body: body }).then(function (r) {
                    return msg(r.message).then(function () {
                        var preview = $('ChkBoxPrint').checked;
                        return reset().then(function () { if (preview) print(r.id); });
                    });
                }).catch(function (e) { return msg(e.message, 'Message'); });
            });
        });
    }
    function btnSave() { S.id = 0; return insert(); }
    function btnSaveAs() { S.id = 0; return insert(); }
    function btnUpdate() {                                                                   // btnupdate_Click
        if (S.id === 0) return msg('Record Id not found for update....');
        return insert();
    }

    /* ------------------------------------------------------------------ ReadById */
    function edit(id) {
        S.id = id;
        return SE.api(API + '/' + id).then(function (d) {
            var h = d.head;
            S.remove = []; S.updateIdx = -1;
            tabs.select('tabPage1');
            $('DocDate').value = SE.dateInput(h.DocDate); $('txtdocno').value = h.DocNo;
            cb.cust.setValue(+h.OrderSupCustId); customerLeave();
            $('txtsupprefno').value = h.SupplierRefNo || ''; $('txtremarks').value = h.RemarksHeader || '';
            cb.cur.setValue(+h.CurrencyId || 0);
            $('txtOrderQty').value = f2(+h.OrderQty || 0); $('txtOrderWeight').value = f2(+h.OrderWeight || 0);
            $('txtItemNetAmountHeader').value = fmtAmt(+h.ItemAmountHeader || 0); $('txtTaxAmountHeader').value = fmtAmt(+h.TaxAmountHeader || 0);
            $('txtDiscountHeader').value = fmtAmt(+h.DiscountAmountHeader || 0);
            $('txtOrderAmountHeader').value = fmtAmt(+h.OrderAmount || 0); $('txtFcyAmount').value = fmtFcy(+h.FcyAmount || 0);
            cb.term.setValue(+h.PaymentTermsId || 0); termChanged();
            $('txtduedays').value = h.OrderDueDays == null ? '' : String(h.OrderDueDays); dueDaysChanged();
            $('duedate').value = SE.dateInput(h.OrderDueDate) || $('duedate').value; dueDateChanged();
            $('validate').value = SE.dateInput(h.OrderExpiryDate) || $('validate').value;
            var dt = (h.DeliveryTerm || '').trim(); DTERMS.forEach(function (x) { if (x.DeliveryTerm === dt) cb.dterm.setValue(x.Id); });
            $('deliverystartdate').value = SE.dateInput(h.DeliveryStartDate) || $('deliverystartdate').value;
            $('txtdeliverydays').value = h.DeliveryDays == null ? '' : String(h.DeliveryDays);
            var stt = (h.OrderStatus || '').trim(); STATUS.forEach(function (x) { if (x.Status === stt) cb.status.setValue(x.Id); });
            S.bill = +h.BillCalculateTypeId || 0;
            enable(cb.status, true);
            if (+h.CurrencyId > 0) $('txtExchangeRate').value = String(h.ExchangeRate == null ? '' : h.ExchangeRate); else configDefault();
            var any = { gdn: false, inv: false };
            G.grd.setRows((d.lines || []).map(function (l) {
                if (l.ReferredInGDN) any.gdn = true; if (l.ReferredInInvoice) any.inv = true;
                return { Id: l.Id, ItemId: l.OrderItemId, ItemCode: l.ItemCodeNew, ItemName: l.ItemName, PackUomId: l.OrderItemUOMId, PackUom: l.UOMCode,
                    VariantId: l.ItemVariantId, VariantDescription: l.VariantDescription, CastingTypeId: l.CastingTypeId, CastingType: l.CastingType, Remarks: l.OrderRemarks,
                    ItemQty: l.OrderItemQty, Weight: l.NetWeight, ItemRate: l.OrderItemRate, RateUomId: l.OrderItemRateUOMId, RateUom: l.RateUom,
                    DiscountType: l.DiscountTypeId, 'Discount%': l.ItemDiscount, DiscountAmount: l.ItemDiscountAmount, Amount: l.Amount, FcyAmount: l.FcyAmount,
                    TaxNameId: l.TaxNameId, TaxName: l.TaxName, TaxPercent: l.TaxPercent, TaxAmount: l.TaxAmount, TotalAmount: l.TotalAmount, CityId: l.CityId, CityName: l.CityArea,
                    ReferredInGDN: !!l.ReferredInGDN, ReferredInInvoice: !!l.ReferredInInvoice };
            }));
            G.pay.setRows((d.payments || []).map(function (p) {
                return { PaymentTermId: p.PaymentTermId, '%ofTotal': p.PrcntOfTotal, Amount: p.Amount, DueDays: p.DueDays, DueDate: SE.dateInput(p.DueDate), Remarks: p.PaymentRemarks };
            }));
            addPaymentRows();
            S.existing = d.attachments || []; S.files = []; S.removedAtt = [];
            show('btnsave', false); show('BtnSaveAs', false); show('btnupdate', true);
            exchangeChanged();
            headerDisable(any.gdn, any.inv);
        }).catch(fail);
    }
    function histEdit(r) { return edit(+r.Id); }
    function histSaveAs(r) {                                                                 // grdhistory "SaveAs"
        return edit(+r.Id).then(function () {
            show('btnsave', false); show('btnupdate', false); show('BtnSaveAs', true);
            G.grd.rows().forEach(function (x) { x.Id = 0; }); G.grd.refresh();
        });
    }

    /* ------------------------------------------------------------------ history (HistoryBranchComboFill, HistoryCombosFill, gridhistoryfill, grdhistory_SelectionChanged) */
    function branchText() { return S.hb.filter(function (b) { return S.hbChecked[b.Id]; }).map(function (b) { return b.BranchName; }).join(','); }
    function branchIds() { return S.hb.filter(function (b) { return S.hbChecked[b.Id]; }).map(function (b) { return ',' + b.Id; }).join(''); }
    function renderBranches() {
        var box = $('branchList'), h = '';
        S.hb.forEach(function (b) { h += '<label style="display:block;padding:2px 6px;white-space:nowrap"><input type="checkbox" data-b="' + b.Id + '"' + (S.hbChecked[b.Id] ? ' checked' : '') + '> ' + SE.esc(b.BranchName) + '</label>'; });
        box.innerHTML = h;
        $('cmbBranchName').textContent = branchText();
        var hide = !(S.branchFeature && !S.branchImplemented);
        ['BranchSrNo', 'BranchName'].forEach(function (k) { var c = G.hist.spec.cols.filter(function (x) { return x.k === k; })[0]; if (c) c.hide = hide; });
        G.hist.refresh();
    }
    function historyBranchFill() {                                                           // HistoryBranchComboFill: the user's own branch is checked
        S.hbChecked = {};
        S.hb.forEach(function (b) { if (b.BranchName === S.userBranchName) S.hbChecked[b.Id] = true; });
        renderBranches();
    }
    function historyCombosFill(validate) {
        var ids = branchIds();
        if (!ids && validate) return Promise.reject(new Error('Select branch first'));
        return SE.api(API + '/history-customers' + SE.q({ branchIds: ids, validate: validate ? 'true' : 'false' })).then(function (rows) { cb.hCust.setData(rows || []); });
    }
    function branchLeave() {                                                                 // cmbBranchName_Leave
        if (branchText() !== '') historyCombosFill(true).catch(function (e) { msg(e.message, 'Error Message'); });
        else { cb.hCust.clear(); cb.hCust.setData([]); }
    }
    function showHistory() {
        if (!branchIds()) { $('cmbBranchName').focus(); return msg('Select branch first', 'Message'); }
        var dt = $('rdentrydate').checked ? 'entry' : ($('rdmodifydate').checked ? 'modify' : ($('rdapproveddate').checked ? 'approved' : 'document'));
        if ($('drdocdate').checked) dt = 'document';
        var q = { dateType: dt, fromDate: $('FromDateHistory_chk').checked ? $('FromDateHistory').value : '', toDate: $('ToDateHistory_chk').checked ? $('ToDateHistory').value : '',
            fromNo: SE.toInt($('FromDocNoHistory').value), toNo: SE.toInt($('ToDocNoHistory').value), customerId: vid(cb.hCust), branchIds: branchIds() };
        return SE.api(API + '/history' + SE.q(q)).then(function (rows) {
            if (!rows.length) { G.hist.setRows([]); G.hd.setRows([]); return; }              // grdhistory.DataSource = null; ClearStructure()
            G.hist.setRows(rows.map(function (r) {
                return { Id: r.Id, DocNo: r.DocNo, BranchSrNo: r.BranchSrNo, BranchName: r.BranchName, DocDate: r.DocDate, CustomerName: r.CustomerName, ItemNetAmount: r.ItemAmountHeader,
                    TaxAmount: r.TaxAmountHeader, DiscountAmount: r.DiscountAmountHeader, OrderNetAmount: r.OrderAmount, PaymentTerms: r.TermsDescription, DueDays: r.DueDays, DueDate: r.DueDate,
                    DeliveryTerm: r.DeliveryTerm, DeliveryDays: r.DeliveryDays, OrderStatus: r.OrderStatus, OrderExpiryDate: r.OrderExpiryDate, RemarksHeader: r.RemarksHeader,
                    EntryUser: r.UserName, EntryDate: r.EntryDate, ModifyUser: r.ModifyUserName, ModifyDate: r.ModifyDate, ApprovedUser: r.ApprovedUser, ApprovedDate: r.PostDate,
                    NoOfAttachments: r.NoOfAttachments };
            }));
        }).catch(function (e) { return msg(e.message, 'Message'); });
    }
    function resetHistory() {                                                                // btnResetHistory_Click
        $('FromDateHistory').value = SE.addDays(-3); $('ToDateHistory').value = SE.today();
        $('FromDocNoHistory').value = ''; $('ToDocNoHistory').value = '';
        cb.hCust.clear(); G.hist.setRows([]); $('drdocdate').checked = true;
    }
    function refreshHistoryCombos() {                                                        // btnRefreshHistory_Click
        return SE.api(API + '/history-branches').then(function (h) {
            S.hb = h.branches || []; S.branchImplemented = !!h.branchImplemented; S.branchFeature = !!h.branchFeature; S.userBranchName = h.userBranchName || '';
            historyBranchFill();
            return historyCombosFill(true);
        }).catch(function (e) { return msg(e.message, 'Message'); });
    }
    function histSelected(item) {
        if (!item) { G.hd.setRows([]); return; }
        SE.api(API + '/' + item.Id + '/history-detail', { quiet: true }).then(function (rows) {
            if (!G.hist.cur() || +G.hist.cur().Id !== +item.Id) return;
            G.hd.setRows(rows.map(function (l) {
                return { ItemCode: l.ItemCodeNew, ItemName: l.ItemName, PackUom: l.UOMCode, VariantDescription: l.VariantDescription, CastingType: l.CastingType, Remarks: l.OrderRemarks,
                    ItemQty: l.OrderItemQty, Weight: l.NetWeight, ItemRate: l.OrderItemRate, RateUom: l.RateUom, DiscountType: l.DiscountType, 'Discount%': l.ItemDiscount,
                    DiscountAmount: l.ItemDiscountAmount, Amount: l.Amount, FcyAmount: l.FcyAmount, TaxName: l.TaxName, TaxPercent: l.TaxPercent, TaxAmount: l.TaxAmount,
                    TotalAmount: l.TotalAmount, CityName: l.CityArea };
            }));
        }).catch(function () { G.hd.setRows([]); });
    }

    /* ------------------------------------------------------------------ print / attachments */
    function print(id) {                                                                     // GeneratePrint -> CommonServices.SaleOrderSlipEngr_1656
        if (!(id > 0)) return msg('No Record Selected');
        return SE.api(API + '/' + id + '/slip-check', { quiet: true }).then(function (r) {
            if (!r || !r.rows) return msg('No Record Found For Display');
            SE.printRpt('1656_SaleOrderSlip_Engr.rpt', { id: id, branchesId: S.branchId, documentTypeIds: String(DOC) });
        }).catch(fail);
    }
    function showAttachments(id) {
        return SE.api(API + '/' + id + '/attachments').then(function (rows) { renderAttachments(rows, id, true); }).catch(fail);
    }
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
        if (!readOnly) h += '<div style="padding:6px"><input type="file" id="atFile" multiple> <span class="se-note">Up to 5 MB each. Files are stored when the sale order is saved.</span></div>';
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

    /* ------------------------------------------------------------------ keys / wiring (PurchsaeOrder_KeyDown, MakeShortCutKeys) */
    var SHORT = [['Ctrl+S', 'For Save and When on Entry form and For Show Data when on History Form'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'],
        ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+D', 'For Adding an row in Focused Grid'], ['Ctrl+Delete', 'For Deleting an row of Focused Grid'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On on Detail Entry First Column'], ['Ctrl+ArrowRight', 'to change focus from one grid to another'],
        ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ["Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function gridKey(e) {                                                                    // grd_KeyDown: Ctrl+Delete / Ctrl+Space on the Delete column
        var t = e.target && e.target.closest ? e.target.closest('.dgrid') : null; if (!t || t.id !== 'grd' || G.grd.curIndex() < 0) return false;
        e.preventDefault(); deleteRow(G.grd.curIndex()); return true;
    }
    function onKey(e) {
        var t = e.target;
        if (e.key === 'Enter' && t && (t.tagName === 'INPUT' || t.tagName === 'SELECT') && t.type !== 'checkbox' && t.type !== 'radio' && t.type !== 'button' && !e.ctrlKey) {
            var f = Array.prototype.filter.call(document.querySelectorAll('.dform input:not([type=hidden]):not([disabled]), .dform select:not([disabled]), .dform .dtcombo-input'),
                function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(t); if (i >= 0 && f[i + 1]) { e.preventDefault(); f[i + 1].focus(); }
        }
        if (e.ctrlKey && (e.key === 't' || e.key === 'T')) { e.preventDefault(); if (tabs.index() === 1) { tabs.select('tabPage1'); $('DocDate').focus(); } else { tabs.select('tabPage2'); $('grdhistory').focus(); } return; }
        var k = e.key.toLowerCase();
        if (tabs.index() === 0) {
            if (e.ctrlKey && e.key === 'Delete') { gridKey(e); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); if (isShown('btnsave') && !$('btnsave').disabled) btnSave(); }
            else if (e.ctrlKey && k === 'u') { e.preventDefault(); if (isShown('btnupdate') && !$('btnupdate').disabled) btnUpdate(); }
            else if (e.ctrlKey && k === 'p') { e.preventDefault(); if (!$('btnprint').disabled) print(S.id); }
            else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); $('DocDate').focus(); }
            else if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); openAttachmentDialog(); }
            else if (e.ctrlKey && e.key === 'F12') { e.preventDefault(); btnSaveAs(); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { if (tabs2.index() === 1) $('grdPaymentTerm').focus(); else $('grd').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowUp') { cb.item.focus(); }
            else if (e.ctrlKey && e.key === 'ArrowRight') {
                if (tabs2.index() === 0) { tabs2.select('tabPagePaymentDetail'); $('grdPaymentTerm').focus(); } else if (tabs2.index() === 1) { tabs2.select('tabPageDetail'); cb.item.focus(); }
            }
        } else if (tabs.index() === 1) {
            if (e.ctrlKey && e.key === 'ArrowDown') {
                if (document.activeElement === $('grd')) $('grdDetail').focus(); else if (document.activeElement === $('grdDetail')) $('grdhistory').focus();
            } else if (e.ctrlKey && k === 's') { e.preventDefault(); showHistory(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); resetHistory(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refreshHistoryCombos(); }
            else if (e.ctrlKey && e.key === 'Enter' && G.hist.cur()) {
                e.preventDefault();
                if (!S.rights.update) return msg("ypu don't have update rights...");
                histEdit(G.hist.cur());
            }
        }
    }
    function wire() {
        $('btnnew').onclick = reset; $('toolStripButton1').onclick = refresh; $('btnsave').onclick = btnSave; $('btnupdate').onclick = btnUpdate; $('BtnSaveAs').onclick = btnSaveAs;
        $('btnattachment').onclick = openAttachmentDialog; $('btnprint').onclick = function () { if (S.id > 0) print(S.id); else msg('No Record Selected'); };
        $('btnshortcutkeys').onclick = function () { SE.shortcuts(SHORT); };
        $('btnCity').onclick = function () { window.open('/master-data/city', '_blank'); };                      // DefineCity
        $('BtnDefineItem').onclick = function () { msg('Define Item (frmAddItem_Engr) has no web screen yet.'); };
        $('BtnDefineVariant').onclick = function () { msg('Define Variant (ItemAttributeVarient_Engr) has no web screen yet.').then(function () { if (vid(cb.item) > 0) bindVariants(vid(cb.item)); }); };
        $('btnplus').onclick = plus; $('btnUpdateDetail').onclick = updateDetail; $('btnCancelUpdateDetial').onclick = cancelUpdateDetail;
        $('btnResetHistory').onclick = resetHistory; $('btnRefreshHistory').onclick = refreshHistoryCombos; $('btnShow').onclick = showHistory;
        function rebindItems() { var id = vid(cb.item); cb.item.setData(itemRows(S.items)); if (id > 0) { cb.item.setValue(id); cb.item.focus(); } }        // rdbtnItemName_CheckedChanged
        $('rdbtnItemName').addEventListener('change', rebindItems);
        $('rdbtnItemCode').addEventListener('change', rebindItems);
        $('txtqty').addEventListener('input', calcAmountTax);
        ['txtRate', 'txtDiscPercent'].forEach(function (x) { $(x).addEventListener('input', calcAmountTax); });
        $('txtTaxPercent').addEventListener('input', calcTax);
        $('txtExchangeRate').addEventListener('input', exchangeChanged);
        $('txtExchangeRate').addEventListener('blur', function () { $('txtExchangeRate').value = fmtRate(num('txtExchangeRate')); });
        $('txtDiscountHeader').addEventListener('input', discountHeaderChanged);
        $('txtduedays').addEventListener('input', dueDaysChanged);
        $('duedate').addEventListener('change', dueDateChanged);
        $('deliverystartdate').addEventListener('change', deliveryStartChanged);
        $('DocDate').addEventListener('change', docDateLeave);
        var bl = $('branchList'), bt = $('cmbBranchName');
        bt.addEventListener('click', function () { bl.style.display = bl.style.display === 'none' ? 'block' : 'none'; });
        bl.addEventListener('change', function (e) {
            var b = e.target && e.target.getAttribute && e.target.getAttribute('data-b'); if (!b) return;
            S.hbChecked[b] = e.target.checked; bt.textContent = branchText();
        });
        document.addEventListener('click', function (e) {
            if (bl.style.display !== 'none' && !bl.contains(e.target) && e.target !== bt) { bl.style.display = 'none'; branchLeave(); }
        });
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
        var hook = false;
        document.addEventListener('keydown', function (e) { if (e.ctrlKey && e.altKey && !hook) { hook = true; SE.shortcuts(SHORT); setTimeout(function () { hook = false; }, 400); } });
    }

    document.addEventListener('DOMContentLoaded', load);
})();
