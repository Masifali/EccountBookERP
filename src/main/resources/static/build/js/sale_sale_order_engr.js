/*
 * Screen 536  SaleOrderWithQty  (Architecture.WinApp.SaleTrading.SaleOrderWithQty, document type 1605)
 * Page script. Desktop methods are named in the comments (SaleOrderWithQty.cs). Server: /sale/engr/sale-order/api
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/engr/sale-order/api', DOC = 1605;
    var S = {
        id: 0, rights: {}, remove: [], updateIdx: -1, updateExtraIdx: -1, files: [], removedAtt: [], existing: [],
        branches: [], projects: [], cust: [], terms: [], items: [], jobs: [], cities: [], curr: [], multi: false, taxEditable: false,
        fmt: { amountRound: 0, amount: 0, rate: 2, fcy: 0 }, def: {}, baseCurrency: 0, baseRate: 0, branchId: 0,
        uoms: [], masterItems: [], masterUoms: [], histRows: []
    };
    var cb = {}, G = {}, tabs, tabs2;

    function fail(e) { return SE.dbError(e); }
    function msg(t, c) { return SE.alert(t, c); }
    function field(id) { return ($(id).value || '').trim(); }
    function enable(c, on) { c.el.disabled = !on; var w = c.el.__dtcombo; if (w && w.syncFromSelect) w.syncFromSelect(); }
    function isShown(id) { var b = $(id); return !!b && b.style.display !== 'none' && !b.hidden; }
    function show(id, on) { var b = $(id); if (!b) return; b.style.display = on ? '' : 'none'; if (b.hasAttribute('hidden')) b.hidden = !on; }
    function vid(c) { return +c.value() || 0; }

    /* ------------------------------------------------------------------ number formats (clsGlobalVariables) */
    function fmtAmt(v) { var d = S.fmt.amount; return SE.num(v, d, d); }                      // stringFormatsingle
    function fmtRate(v) { var d = S.fmt.rate; return SE.num(v, d, d); }                       // DecimalRateFormate
    function fmtFcy(v) { var d = S.fmt.fcy; return SE.num(v, d, d); }                         // stringFormatsingleForFcy
    function f3(v) { return SE.num(v, 3, 0); }                                               // "#,##0.###"
    function f4(v) { return SE.num(v, 4, 0); }                                               // "#,##0.####"
    function f2(v) { return SE.num(v, 2, 0); }                                               // "#,##0.##"
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
        cb.branch = XCombo('cmbbranch1', { columns: [{ key: 'BranchName', caption: 'Branch Name' }], textKey: 'BranchName', popupWidth: 300 });
        cb.project = XCombo('combproject1', { columns: [{ key: 'ProjectName', caption: 'Project Name' }], textKey: 'ProjectName', popupWidth: 300 });
        cb.cust = XCombo('combsuppname', { columns: [{ key: 'CompanyName', caption: 'Customer Name' }], textKey: 'CompanyName', popupWidth: 420 });
        cb.term = XCombo('combpttrm', { columns: [{ key: 'TermsDescription', caption: 'Payment Term' }], textKey: 'TermsDescription', onSelect: termChanged });
        cb.dterm = XCombo('combdeliverytrm', { columns: [{ key: 'DeliveryTerm', caption: 'Delivery Term' }], textKey: 'DeliveryTerm' });
        cb.status = XCombo('CmbStatus', { columns: [{ key: 'Status', caption: 'Order Status' }], textKey: 'Status' });
        cb.bill = XCombo('CmbBillType', { columns: [{ key: 'Type', caption: 'Bill Type' }], textKey: 'Type' });
        cb.cur = XCombo('cmbCurrency', { columns: [{ key: 'CurrencyCode', caption: 'Currency' }], textKey: 'CurrencyCode', onSelect: currencyLeave });
        cb.item = XCombo('CmbItem', { columns: [{ key: '_t', caption: 'Item Name' }, { key: '_o', caption: 'Item Code' }], textKey: '_t', popupWidth: 460, onSelect: itemLeave });
        cb.uom = XCombo('CmbPackUom', { columns: [{ key: 'UOMCode', caption: 'Uom' }, { key: 'Equivalent', caption: 'Equivalent' }], textKey: 'UOMCode', onSelect: calcAll });
        cb.rate = XCombo('CmbRateUom', { columns: [{ key: 'UOMCode', caption: 'RateUom' }, { key: 'Equivalent', caption: 'Equivalent' }], textKey: 'UOMCode', onSelect: calcAmountTax });
        cb.job = XCombo('CmbJobLot', { columns: [{ key: 'JobLotDescription', caption: 'Job Lot' }], textKey: 'JobLotDescription', popupWidth: 300 });
        cb.disc = XCombo('CmbDiscountType', { columns: [{ key: 'Name', caption: 'Discount Type' }], textKey: 'Name', onSelect: calcAmountTax });
        cb.tax = XCombo('CmbTaxType', { columns: [{ key: 'TaxType', caption: 'TaxType' }], textKey: 'TaxType', onSelect: calcTax });
        cb.city = XCombo('CmbCity', { columns: [{ key: 'CityName', caption: 'City Name' }], textKey: 'CityName', popupWidth: 320 });
        cb.mItem = XCombo('CmbMasterItemInExtra', { columns: [{ key: '_t', caption: 'Item Name' }, { key: '_o', caption: 'Item Code' }], textKey: '_t', popupWidth: 460, onSelect: masterItemLeave });
        cb.mUom = XCombo('CmbMasterPackUomInExtra', { columns: [{ key: 'MasterPackUom', caption: 'MasterUom' }], valueKey: 'MasterPackUomId', textKey: 'MasterPackUom' });
        cb.xItem = XCombo('CmbItemInExtra', { columns: [{ key: '_t', caption: 'Item Name' }, { key: '_o', caption: 'Item Code' }], textKey: '_t', popupWidth: 460, onSelect: extraItemLeave });
        cb.xUom = XCombo('CmbPackuomInExtra', { columns: [{ key: 'UOMCode', caption: 'Uom' }, { key: 'Equivalent', caption: 'Equivalent' }], textKey: 'UOMCode', onSelect: calcWeightExtra });
        cb.xJob = XCombo('CmbJobLotInExtra', { columns: [{ key: 'JobLotDescription', caption: 'Job Lot' }], textKey: 'JobLotDescription', popupWidth: 300 });
        cb.hCust = XCombo('CmbCustomerHistory', { columns: [{ key: 'Customer', caption: 'Party' }], valueKey: 'Id', textKey: 'Customer', popupWidth: 380 });
    }

    /* ------------------------------------------------------------------ fills (CompanyFill ... CurrencyFill) */
    function keepFill(c, rows, keepId) { c.setData(rows); if (keepId > 0 && !c.setValue(keepId)) c.clear(); }
    function fillBranches() { cb.branch.setData(S.branches); if (S.branches.length) cb.branch.setValue(S.branches[0].Id); }          // Rows[0].Activate()
    function fillProjects() { cb.project.setData(S.projects); if (S.projects.length) cb.project.setValue(S.projects[0].Id); }
    function fillCustomers(history) {
        var a = vid(cb.cust), b = vid(cb.hCust);
        keepFill(cb.cust, S.cust, a);
        if (history) cb.hCust.setData(S.cust.map(function (r) { return { Id: r.Id, Customer: r.CompanyName }; })), b > 0 && !cb.hCust.setValue(b) && cb.hCust.clear();
    }
    function fillTerms() {
        var id = vid(cb.term);
        cb.term.setData(S.terms);
        if (S.terms.length) cb.term.setValue(S.terms[0].Id);                                   // Rows[1] (Rows[0] is the "Select" row)
        if (id > 0 && S.terms.some(function (t) { return +t.Id === id; })) cb.term.setValue(id);
        termChanged();
    }
    function bindItems() {
        var byName = $('rdbtnItemName').checked;
        cb.item.setData(S.items.map(function (r) { return { Id: r.Id, _t: byName ? r.ItemName : r.ItemCode, _o: byName ? r.ItemCode : r.ItemName, ItemName: r.ItemName, ItemCode: r.ItemCode }; }));
    }
    function bindExtraItems() {
        var byName = $('RadExtraItemName').checked;
        cb.xItem.setData(S.items.map(function (r) { return { Id: r.Id, _t: byName ? r.ItemName : r.ItemCode, _o: byName ? r.ItemCode : r.ItemName, ItemName: r.ItemName, ItemCode: r.ItemCode }; }));
    }
    function fillItems() {                                                                   // ItemDetailFill
        var id = vid(cb.item);
        cb.item.clear(); cb.xItem.clear();
        bindItems(); bindExtraItems();
        if (id > 0 && !cb.item.setValue(id)) cb.item.clear();
    }
    function fillJobLots() {
        var a = vid(cb.job), b = vid(cb.xJob);
        keepFill(cb.job, S.jobs, a); keepFill(cb.xJob, S.jobs, b);
    }
    function fillCities() { keepFill(cb.city, S.cities, vid(cb.city)); }
    function fillCurrencies() { keepFill(cb.cur, S.curr, vid(cb.cur)); }
    /* MultiCurrencyFeature */
    function multiCurrency() {
        ['cmbCurrency', 'txtExchangeRate', 'txtFcyAmount', 'label35', 'label32', 'label29'].forEach(function (id) { var e = $(id); if (e) e.style.visibility = S.multi ? '' : 'hidden'; });
        var c = $('cmbCurrency'); if (c && c.__dtcombo && c.__dtcombo.wrap) c.__dtcombo.wrap.style.visibility = S.multi ? '' : 'hidden';
        $('txtremarks').style.width = S.multi ? '614px' : '827px';
        var col = G.grd && G.grd.spec.cols.filter(function (x) { return x.k === 'FcyAmount'; })[0]; if (col) col.hide = !S.multi;
        var dcol = G.hd && G.hd.spec.cols.filter(function (x) { return x.k === 'FcyAmount'; })[0]; if (dcol) dcol.hide = !S.multi;
        if (G.grd) G.grd.refresh(); if (G.hd) G.hd.refresh();
    }
    /* ConfigurationDefault */
    function configDefault() {
        if (S.def.cityId > 0 && !(vid(cb.city) > 0)) cb.city.setValue(S.def.cityId);
        if (S.def.jobLotId > 0 && !(vid(cb.job) > 0)) cb.job.setValue(S.def.jobLotId);
        S.baseCurrency = S.def.baseCurrency || 0; S.baseRate = S.def.baseRate || 0;
        if (S.baseCurrency > 0 && !(vid(cb.cur) > 0)) cb.cur.setValue(S.baseCurrency);
        if (S.baseRate > 0 && num('txtExchangeRate') === 0) $('txtExchangeRate').value = fmtRate(S.baseRate);
    }
    function applyLists(d) {
        S.branches = d.branches || []; S.projects = d.projects || []; S.cust = d.customers || []; S.terms = d.terms || []; S.items = d.items || [];
        S.jobs = d.jobLots || []; S.cities = d.cities || []; S.curr = d.currencies || []; S.multi = !!d.multiCurrency; S.taxEditable = !!d.taxEditable;
        S.def = d.defaults || {}; S.fmt = d.fmt || S.fmt;
        G.grd.spec.dec = S.fmt.amount; G.hist.spec.dec = S.fmt.amount; G.hd.spec.dec = S.fmt.amount;
    }

    /* ------------------------------------------------------------------ calculations */
    function selUom(c) { var r = c.row(); return r && (+c.value() > 0) ? r : null; }
    function calcWeight() {                                                                  // CalculateWeight
        var r = selUom(cb.uom), q = num('txtqty');
        $('txtweight').value = r && q > 0 ? f3((+r.Equivalent || 0) * q) : '0';
    }
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
    function calcAll() { calcWeight(); calcAmount(); calcTax(); }
    function calcWeightExtra() {                                                             // CalculateWeightExtra
        var r = selUom(cb.xUom), q = num('txtQtyInExtra');
        $('txtWeightInExtra').value = r && q > 0 ? f3((+r.Equivalent || 0) * q) : '0';
    }

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

    /* ------------------------------------------------------------------ item / UOM / tax (combitem_Leave_1, bindRateUomAndItemPackUom, GetTaxTypeIdAndTaxPercent) */
    function byCode(rows, code) { for (var i = 0; i < rows.length; i++) if (String(rows[i].UOMCode) === String(code)) return rows[i]; return null; }
    function bindUoms() {
        var pu = cb.uom.text(), ru = cb.rate.text();
        cb.uom.clear(); cb.rate.clear();
        return SE.api(API + '/uoms' + SE.q({ itemId: vid(cb.item) })).then(function (rows) {
            S.uoms = rows;
            cb.uom.setData(rows); cb.rate.setData(rows);
            var a = byCode(rows, pu), b = byCode(rows, ru);
            if (rows.length && a) cb.uom.setValue(a.Id); else cb.uom.clear();
            if (rows.length && b) cb.rate.setValue(b.Id); else cb.rate.clear();
        });
    }
    function itemLeave() {
        return bindUoms().then(function () {
            if (S.def.jobLotId > 0) cb.job.setValue(S.def.jobLotId);                           // Job/Lot configuration
            return taxForItem();
        }).catch(fail);
    }
    function taxForItem() {
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
        if ($('txtRate').value === '') return stop('Item Rate Field is Required', function () { $('txtRate').focus(); });
        if (!cb.rate.row() || !(vid(cb.rate) > 0)) return stop('Rate UOM Field is Required', function () { cb.rate.focus(); });
        if (!cb.city.row() || !(vid(cb.city) > 0)) return stop('City Field is Required', function () { cb.city.focus(); });
        return true;
    }
    function newRowFromEntry(id) {
        var ir = cb.item.row(), amount = num('txtItemAmount'), ex = num('txtExchangeRate');
        return {
            Id: id, ItemId: vid(cb.item), ItemCode: ir.ItemCode, ItemName: ir.ItemName, ItemDescription: $('txtItemDescriptions').value, Remarks: $('txtremarksdetail').value,
            JobLotId: vid(cb.job), JobLot: cb.job.text(), PackUomId: vid(cb.uom), PackUom: cb.uom.text(), ItemQty: num('txtqty'), Weight: num('txtweight'), ItemRate: num('txtRate'),
            RateUomId: vid(cb.rate), RateUom: cb.rate.text(), DiscountType: vid(cb.disc), 'Discount%': num('txtDiscPercent'), DiscountAmount: num('txtDiscountAmount'), Amount: amount,
            FcyAmount: ex > 0 && amount > 0 ? amount / ex : 0, TaxNameId: vid(cb.tax), TaxName: cb.tax.text(), TaxPercent: num('txtTaxPercent'), TaxAmount: num('txtTaxAmount'),
            TotalAmount: num('txtTotalAmount'), CityId: vid(cb.city), CityName: cb.city.text()
        };
    }
    function clearDetailEntry() {
        cb.item.clear(); $('txtItemDescriptions').value = ''; $('txtremarksdetail').value = ''; cb.uom.clear(); $('txtqty').value = ''; $('txtweight').value = '';
        cb.rate.clear(); $('txtRate').value = ''; $('txtItemAmount').value = ''; cb.tax.clear(); $('txtTaxPercent').value = ''; $('txtTaxAmount').value = ''; $('txtTotalAmount').value = '';
    }
    function plus() {
        if (!detailValid()) return;
        G.grd.addRow(newRowFromEntry(0));
        clearDetailEntry(); cb.item.focus();
        exchangeChanged(); fillMasterItems();
    }
    function editRow(i) {                                                                    // grd_DoubleClick
        var r = G.grd.rows()[i]; if (!r) return;
        S.updateIdx = i;
        cb.item.setValue(+r.ItemId);
        bindUoms().then(function () {
            cb.job.setValue(+r.JobLotId || 0);                                               // desktop: Job/Lot default first, then the row's job lot
            return taxForItem();
        }).then(function () {
            $('txtItemDescriptions').value = r.ItemDescription || '';
            cb.job.setValue(+r.JobLotId); cb.uom.setValue(+r.PackUomId);
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
        }).catch(fail);
    }
    function updateDetail() {                                                                // btnUpdateDetail_Click
        if (!detailValid()) return;
        var old = G.grd.rows()[S.updateIdx]; if (!old) return;
        var n = newRowFromEntry(old.Id), rows = G.grd.rows();
        rows[S.updateIdx] = n; G.grd.refresh();
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        clearDetailEntry(); S.updateIdx = -1; cb.item.focus();
        exchangeChanged(); fillMasterItems();
    }
    function cancelUpdateDetail() {                                                          // btnCancelUpdateDetial_Click
        clearDetailEntry(); cb.item.focus();
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
    }
    function lineOf(r) {
        return { id: +r.Id || 0, itemId: +r.ItemId || 0, itemName: r.ItemName, itemDescription: r.ItemDescription == null ? '' : String(r.ItemDescription),
            remarks: r.Remarks == null ? '' : String(r.Remarks), jobLotId: +r.JobLotId || 0, packUomId: +r.PackUomId || 0, itemQty: +r.ItemQty || 0, weight: +r.Weight || 0,
            itemRate: +r.ItemRate || 0, rateUomId: +r.RateUomId || 0, discountType: +r.DiscountType || 0, discountPercent: +r['Discount%'] || 0, discountAmount: +r.DiscountAmount || 0,
            amount: +r.Amount || 0, taxNameId: +r.TaxNameId || 0, taxPercent: +r.TaxPercent || 0, taxAmount: +r.TaxAmount || 0, totalAmount: +r.TotalAmount || 0,
            cityId: +r.CityId || 0, cityName: r.CityName == null ? '' : String(r.CityName) };
    }
    /* ValidateDetailItemExistInExtra */
    function validateExtra(itemId) {
        var ex = G.ext.rows();
        if (ex.length > 0 && itemId > 0 && ex.some(function (r) { return +r.MasterItemId === itemId; })) {
            return SE.ask('The item you are trying to delete exists in the Extra Items grid as a master item.\nif You Delete this row, The rows containing that item in Extra Item Grid will also be deleted.\nAre you sure you want to delete it?', 'Confirm').then(function (yes) {
                if (!yes) return false;
                for (var i = ex.length - 1; i >= 0; i--) if (+ex[i].MasterItemId === itemId) G.ext.removeAt(i);
                return true;
            });
        }
        return Promise.resolve(true);
    }
    function deleteRow(i) {                                                                  // grd_ColumnButtonClick "Delete"
        var r = G.grd.rows()[i]; if (!r) return;
        return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
            if (!yes) return;
            return validateExtra(+r.ItemId).then(function (ok) {
                if (!ok) return;
                if ((+r.Id || 0) > 0) S.remove.push(lineOf(r));
                G.grd.removeAt(G.grd.rows().indexOf(r));
                calcTotals(); fillMasterItems();
            });
        });
    }

    /* ------------------------------------------------------------------ extra items (FillMasterItemsFromDetail, btnAddInExtra_Click ...) */
    function fillMasterItems() {
        if (G.grd.count() === 0) return;
        var id = vid(cb.mItem), seenI = {}, seenU = {};
        S.masterItems = []; S.masterUoms = [];
        G.grd.rows().forEach(function (r) {
            if (!seenI[r.ItemId]) { seenI[r.ItemId] = 1; S.masterItems.push({ Id: +r.ItemId, ItemName: r.ItemName, ItemCode: r.ItemCode }); }
            var k = r.PackUomId + '_' + r.ItemId;
            if (!seenU[k]) { seenU[k] = 1; S.masterUoms.push({ MasterPackUomId: +r.PackUomId, MasterPackUom: r.PackUom, MasterItemId: +r.ItemId }); }
        });
        bindMasterItems();
        if (id > 0) { if (S.masterItems.some(function (x) { return x.Id === id; })) cb.mItem.setValue(id); else cb.mItem.clear(); }
    }
    function bindMasterItems() {
        var byName = $('RadMasterItemName').checked;
        cb.mItem.setData(S.masterItems.map(function (r) { return { Id: r.Id, _t: byName ? r.ItemName : r.ItemCode, _o: byName ? r.ItemCode : r.ItemName }; }));
    }
    function masterItemLeave() {                                                             // CmbMasterItemInExtra_Leave
        if (!cb.mItem.row() || vid(cb.mItem) <= 0) return;
        var prev = cb.mUom.text(); cb.mUom.clear();
        var rows = S.masterUoms.filter(function (x) { return x.MasterItemId === vid(cb.mItem); });
        if (S.masterUoms.length > 0 && rows.length > 0) {
            cb.mUom.setData(rows);
            var hit = rows.filter(function (x) { return x.MasterPackUom === prev; })[0];
            if (hit) cb.mUom.setValue(hit.MasterPackUomId); else cb.mUom.clear();
        } else { cb.mUom.clear(); cb.mUom.setData([]); }
    }
    function extraItemLeave() {                                                              // CmbItemInExtra_Leave -> bindPackUomInExtra
        var prev = cb.xUom.text(); cb.xUom.clear();
        return SE.api(API + '/uoms' + SE.q({ itemId: vid(cb.xItem) })).then(function (rows) {
            if (rows.length) { cb.xUom.setData(rows); var h = byCode(rows, prev); if (h) cb.xUom.setValue(h.Id); else cb.xUom.clear(); }
            else { cb.xUom.clear(); cb.xUom.setData([]); }
        }).catch(fail);
    }
    function extraValid() {                                                                  // FormValidationExtraDetail
        function stop(m, f) { msg(m); if (f) f(); return false; }
        var mi = cb.mItem.row() && vid(cb.mItem) !== 0, mu = vid(cb.mUom);
        if (mi && mu === 0) return stop('Master Uom Field is Required when Master Item Is Selected', function () { cb.mUom.focus(); });
        if (!mi && mu !== 0) return stop('Master Item Field is Required when Master Uom Is Selected', function () { cb.mItem.focus(); });
        if (!cb.xItem.row() || vid(cb.xItem) === 0) return stop('Item Field is Required', function () { cb.xItem.focus(); });
        if (!cb.xUom.row() || vid(cb.xUom) === 0) return stop('Pack Uom Field is Required', function () { cb.xUom.focus(); });
        if (num('txtQtyInExtra') === 0) return stop('Item Qty Field is Required', function () { $('txtQtyInExtra').focus(); });
        if (!cb.xJob.row() || vid(cb.xJob) === 0) return stop('JobLot Field is Required', function () { cb.xJob.focus(); });
        return true;
    }
    function extraRowFromEntry(id, remarks) {
        var mn = '', mc = '';
        if (vid(cb.mItem) > 0) { var mr = cb.mItem.row(); mn = mr.ItemName || ''; mc = mr.ItemCode || ''; }
        var xr = cb.xItem.row();
        return { Id: id, MasterItemId: vid(cb.mItem), MasterItemCode: mc, MasterItemName: mn, MasterPackUomId: vid(cb.mUom), MasterPackUom: cb.mUom.text(), ItemId: vid(cb.xItem),
            ItemCode: xr.ItemCode, ItemName: xr.ItemName, ItemDescription: $('txtItemDescriptionInExtra').value, Remarks: remarks, JobLotId: vid(cb.xJob), JobLot: cb.xJob.text(),
            PackUomId: vid(cb.xUom), PackUom: cb.xUom.text(), ItemQty: num('txtQtyInExtra'), Weight: num('txtWeightInExtra') };
    }
    function resetExtra() {                                                                  // ResetExtraDetail
        cb.mItem.clear(); cb.mUom.clear(); cb.xItem.clear(); cb.xUom.clear(); $('txtQtyInExtra').value = ''; $('txtWeightInExtra').value = ''; cb.xJob.clear();
        show('btnAddInExtra', true); show('BtnUpdateInExtra', false); show('BtnCancelInExtra', false);
        cb.mItem.focus();
    }
    function addExtra() {
        if (!extraValid()) return;
        G.ext.addRow(extraRowFromEntry(0, $('txtRemarksInExtra').value));
        resetExtra();
    }
    function editExtra(i) {                                                                  // grdExtraItems_DoubleClick
        var r = G.ext.rows()[i]; if (!r) return;
        S.updateExtraIdx = i;
        fillMasterItems();
        cb.mItem.setValue(+r.MasterItemId || 0); masterItemLeave();
        cb.mUom.setValue(+r.MasterPackUomId || 0);
        cb.xItem.setValue(+r.ItemId);
        extraItemLeave().then(function () {
            cb.xUom.setValue(+r.PackUomId);
            $('txtItemDescriptionInExtra').value = r.ItemDescription || ''; $('txtRemarksInExtra').value = r.Remarks || '';
            cb.xJob.setValue(+r.JobLotId);
            $('txtQtyInExtra').value = f3(r.ItemQty); $('txtWeightInExtra').value = f3(r.Weight);
            show('btnAddInExtra', false); show('BtnUpdateInExtra', true); show('BtnCancelInExtra', true);
            cb.mItem.focus();
        });
    }
    function updateExtra() {                                                                 // BtnUpdateInExtra_Click (Remarks are read from txtremarksdetail, as the desktop does)
        if (!extraValid()) return;
        var rows = G.ext.rows(), old = rows[S.updateExtraIdx]; if (!old) return;
        rows[S.updateExtraIdx] = extraRowFromEntry(old.Id, $('txtremarksdetail').value);
        G.ext.refresh();
        resetExtra();
    }
    function deleteExtra(i) {                                                                // grdExtraItems "Delete"
        var r = G.ext.rows()[i]; if (!r) return;
        if ((+r.Id || 0) > 0) return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) { if (yes) G.ext.removeAt(i); });
        G.ext.removeAt(i);
    }

    /* ------------------------------------------------------------------ payment grid */
    function addPaymentRow() {                                                               // AddRowInPaymentGrid
        G.pay.addRow({ PaymentTermId: 0, '%ofTotal': 0, Amount: 0, DueDays: 0, DueDate: SE.today(), Remarks: '' });
    }
    function payEdit(r, k, v) {                                                              // grdPaymentTerm_UpdatingCell / _CellUpdated
        try {
            if (k === '%ofTotal' && !/^\s*-?\d*\.?\d*\s*$/.test(String(v))) { return msg('Please Type Only Numeric Value', 'Warning!'); }
            if (k === 'PaymentTermId') { r[k] = +v || 0; return; }
            if (k === 'Remarks') { r[k] = v; return; }
            if (k === 'DueDate') {
                var d = readDate(v); if (!d) return;
                if (d < $('DocDate').value) { r.DueDate = $('DocDate').value; r.DueDays = 0; return msg("Due Date Can't less Than DocDate"); }
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
                r['%ofTotal'] = roundAway(n * 100 / bill, 8);
            } else if (k === 'DueDays') {
                r.DueDays = Math.trunc(n); r.DueDate = addDays($('DocDate').value, Math.trunc(n));
            }
        } catch (e) { msg(e.message); }
    }
    function deletePay(i) {
        var rows = G.pay.rows(); if (!rows[i]) return;
        G.pay.removeAt(i);
        if (G.pay.count() === 0) addPaymentRow();
    }

    /* ------------------------------------------------------------------ grids */
    function makeGrids() {
        G.grd = SE.grid('grd', { footer: true, dec: 0, frozen: 1, onDbl: function (r, i) { editRow(i); },
            onBtn: function (k, r, i) { if (k === 'Delete') deleteRow(i); },
            cols: [
                { k: 'Delete', t: 'X', w: 28, btn: 'X' }, { k: 'Id', hide: true }, { k: 'ItemId', hide: true }, { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'ItemName', t: 'ItemName', w: 200 },
                { k: 'ItemDescription', t: 'ItemDescription', w: 200 }, { k: 'Remarks', t: 'Specification/Remarks', w: 200 }, { k: 'JobLotId', hide: true }, { k: 'JobLot', t: 'JobLot', w: 120 },
                { k: 'PackUomId', hide: true }, { k: 'PackUom', t: 'PackUom', w: 70 }, { k: 'ItemQty', t: 'Qty', w: 80, f: 'n3', sum: true }, { k: 'Weight', t: 'Weight', w: 80, f: 'n3', sum: true },
                { k: 'ItemRate', t: 'ItemRate', w: 80, render: function (v) { return v == null ? '' : fmtRate(v); } }, { k: 'RateUomId', hide: true }, { k: 'RateUom', t: 'RateUom', w: 70 },
                { k: 'DiscountType', t: 'DiscountType', w: 60, list: function () { return [{ Id: 1, Name: 'Flat' }, { Id: 2, Name: 'Percent' }]; }, lk: 'Id', lt: 'Name' },
                { k: 'Discount%', t: 'Discount%', w: 80, render: function (v) { return v == null ? '' : fmtRate(v); } },
                { k: 'DiscountAmount', t: 'DiscountAmount', w: 100, f: 'amt', sum: true }, { k: 'Amount', t: 'Amount', w: 100, f: 'amt', sum: true },
                { k: 'FcyAmount', t: 'FcyAmount', w: 100, f: 'amt', sum: true, hide: true }, { k: 'TaxNameId', hide: true }, { k: 'TaxName', t: 'TaxName', w: 90 },
                { k: 'TaxPercent', t: 'TaxPercent', w: 80, render: function (v) { return v == null ? '' : f3(v); } }, { k: 'TaxAmount', t: 'TaxAmount', w: 90, f: 'amt', sum: true },
                { k: 'TotalAmount', t: 'TotalAmount', w: 100, f: 'amt', sum: true }, { k: 'CityId', hide: true }, { k: 'CityName', t: 'CityName', w: 120 }] });
        G.ext = SE.grid('grdExtraItems', { footer: true, dec: 0, onDbl: function (r, i) { editExtra(i); },
            onBtn: function (k, r, i) { if (k === 'Delete') deleteExtra(i); else if (k === 'Edit') editExtra(i); },
            cols: [
                { k: 'Delete', t: 'X', w: 28, btn: 'X' }, { k: 'Edit', t: 'Edit', w: 50, btn: 'Edit' }, { k: 'Id', hide: true }, { k: 'MasterItemId', hide: true },
                { k: 'MasterItemCode', t: 'MasterItemCode', w: 100 }, { k: 'MasterItemName', t: 'MasterItemName', w: 180 }, { k: 'MasterPackUomId', hide: true }, { k: 'MasterPackUom', t: 'MasterPackUom', w: 90 },
                { k: 'ItemId', hide: true }, { k: 'ItemCode', t: 'ItemCode', w: 100 }, { k: 'ItemName', t: 'ItemName', w: 180 }, { k: 'ItemDescription', t: 'ItemDescription', w: 200 },
                { k: 'Remarks', t: 'Specification/Remarks', w: 200 }, { k: 'JobLotId', hide: true }, { k: 'JobLot', t: 'JobLot', w: 120 }, { k: 'PackUomId', hide: true }, { k: 'PackUom', t: 'PackUom', w: 80 },
                { k: 'ItemQty', t: 'Qty', w: 80, f: 'n3', sum: true }, { k: 'Weight', t: 'Weight', w: 80, f: 'n3', sum: true }] });
        G.pay = SE.grid('grdPaymentTerm', { footer: true, dec: 4, frozen: 2, onEdit: function (r, k, v) { payEdit(r, k, v); G.pay.refresh(); },
            onBtn: function (k, r, i) { if (k === 'Delete') deletePay(i); else if (k === 'Add') addPaymentRow(); },
            cols: [
                { k: 'Add', t: '+', w: 28, btn: '+' }, { k: 'Delete', t: 'X', w: 28, btn: 'X' },
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
                { k: 'DocNo', t: 'DocNo', w: 60 }, { k: 'DocDate', t: 'DocDate', w: 85, f: 'sdate' }, { k: 'CustomerName', t: 'CustomerName', w: 220 },
                { k: 'ItemNetAmount', t: 'ItemNetAmount', w: 100, f: 'amt', sum: true }, { k: 'TaxAmount', t: 'TaxAmount', w: 90, f: 'amt', sum: true },
                { k: 'DiscountAmount', t: 'DiscountAmount', w: 100, f: 'amt', sum: true }, { k: 'OrderNetAmount', t: 'OrderNetAmount', w: 110, f: 'amt', sum: true },
                { k: 'PaymentTerms', t: 'PaymentTerms', w: 100 }, { k: 'DueDays', t: 'DueDays', w: 60 }, { k: 'DueDate', t: 'DueDate', w: 85, f: 'sdate' }, { k: 'DeliveryTerm', t: 'DeliveryTerm', w: 90 },
                { k: 'DeliveryDays', t: 'DeliveryDays', w: 80 }, { k: 'OrderStatus', t: 'OrderStatus', w: 80 }, { k: 'OrderExpiryDate', t: 'OrderExpiryDate', w: 100, f: 'sdate' },
                { k: 'RemarksHeader', t: 'RemarksHeader', w: 200 }, { k: 'EntryUser', t: 'EntryUser', w: 100 }, { k: 'EntryDate', t: 'EntryDate', w: 130, f: 'dt12' },
                { k: 'ModifyUser', t: 'ModifyUser', w: 100 }, { k: 'ModifyDate', t: 'ModifyDate', w: 130, f: 'dt12' }, { k: 'ApprovedUser', t: 'ApprovedUser', w: 100 },
                { k: 'ApprovedDate', t: 'ApprovedDate', w: 130, f: 'dt12' }, { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 110, link: true }] });
        G.hd = SE.grid('grdDetail', { footer: true, dec: 0,
            cols: [
                { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'ItemDescription', t: 'ItemDescription', w: 200 }, { k: 'Specification/Remarks', t: 'Specification/Remarks', w: 200 },
                { k: 'JobLot', t: 'JobLot', w: 120 }, { k: 'PackUom', t: 'Uom', w: 70 }, { k: 'ItemQty', t: 'Qty', w: 80, f: 'n3', sum: true }, { k: 'Weight', t: 'Weight', w: 80, f: 'n3', sum: true },
                { k: 'ItemRate', t: 'ItemRate', w: 80, render: function (v) { return v == null ? '' : fmtRate(v); } }, { k: 'RateUom', t: 'RateUom', w: 70 }, { k: 'DiscountType', t: 'DiscountType', w: 80 },
                { k: 'Discount%', t: 'Discount%', w: 80, render: function (v) { return v == null ? '' : fmtRate(v); } }, { k: 'DiscountAmount', t: 'DiscountAmount', w: 100, f: 'n3', sum: true },
                { k: 'Amount', t: 'ItemAmount', w: 100, f: 'amt', sum: true }, { k: 'FcyAmount', t: 'FcyAmount', w: 100, f: 'amt', sum: true, hide: true }, { k: 'TaxName', t: 'TaxName', w: 90 },
                { k: 'TaxPercent', t: 'TaxPercent', w: 80, render: function (v) { return v == null ? '' : f3(v); } }, { k: 'TaxAmount', t: 'TaxAmount', w: 90, f: 'amt', sum: true },
                { k: 'CityName', t: 'CityName', w: 120 }] });
    }

    /* ------------------------------------------------------------------ Load / Reset / Refresh */
    function staticCombos() {
        cb.dterm.setData([{ Id: 1, DeliveryTerm: 'Load' }, { Id: 2, DeliveryTerm: 'Ponch' }]); cb.dterm.setValue(1);              // DeliveryTerm(): Rows[0].Activate()
        cb.status.setData([{ Id: 1, Status: 'Open' }, { Id: 2, Status: 'Complete' }, { Id: 3, Status: 'Cancel' }]); cb.status.setValue(1);  // OrderStatus()
        cb.bill.setData([{ Id: 1, Type: 'On Weight' }, { Id: 2, Type: 'On Qty' }]);                                              // BillTypeComboFill (no row activated)
        cb.disc.setData([{ Id: 1, Name: 'Flat' }, { Id: 2, Name: 'Percent' }]); cb.disc.setValue(2);                             // DiscountTypeFill: Rows[1].Activate()
    }
    function load() {
        makeCombos();
        tabs = SE.tabs('tabControl1', function (id) { if (id === 'tabPage2') $('FromDateHistory').focus(); else $('DocDate').focus(); });
        tabs2 = SE.tabs('tabControl2');
        ['txtduedays', 'txtdeliverydays', 'txtDiscPercent', 'FromDocNoHistory', 'ToDocNoHistory'].forEach(function (id) { SE.digitsOnly($(id)); });
        ['txtqty', 'txtRate', 'txtExchangeRate', 'txtItemAmount'].forEach(function (id) {
            $(id).addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[\d.]/.test(e.key) && !e.ctrlKey) e.preventDefault(); });
        });
        makeGrids();
        staticCombos();
        SE.api(API + '/initial').then(function (d) {
            applyLists(d);
            S.rights = d.rights || {}; S.branchId = d.branchId || 0;
            multiCurrency();
            fillCurrencies(); fillBranches(); fillProjects(); fillCustomers(false);
            cb.hCust.setData((d.history && d.history.customers) || []);
            fillTerms(); fillItems(); fillJobLots(); fillCities(); configDefault();
            $('txtdocno').value = d.nextNo > 0 ? String(d.nextNo) : '';
            $('txtTaxPercent').readOnly = !S.taxEditable;
            addPaymentRow();
            $('btnsave').disabled = !S.rights.save; $('btnprint').disabled = !S.rights.print; $('btnupdate').disabled = !S.rights.update;
            show('btnupdate', false); show('btnsave', true); show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false); show('BtnSaveAs', false);
            enable(cb.status, false);
            $('ChkBoxPrint').checked = true;
            var t = SE.today();
            $('DocDate').value = t; $('duedate').value = t; $('deliverystartdate').value = t; $('validate').value = t;
            $('FromDateHistory').value = SE.addDays(-3); $('ToDateHistory').value = t;
            $('txtduedays').value = ''; $('txtdeliverydays').value = '';
            $('DocDate').focus();
            var rec = SE.param('record'); if (rec) { S.id = +rec; edit(+rec); }
        }).catch(fail);
        wire();
    }

    function resetEntryFields() {
        ['txtsupprefno', 'txtduedays', 'txtItemNetAmountHeader', 'txtDiscountHeader', 'txtTaxAmountHeader', 'txtDiscPercent', 'txtDiscountAmount', 'txtqty', 'txtweight', 'txtRate',
            'txtItemAmount', 'txtTaxPercent', 'txtTaxAmount', 'txtTotalAmount', 'txtFcyAmount', 'txtOrderQty', 'txtOrderWeight', 'txtOrderAmountHeader'].forEach(function (id) { $(id).value = ''; });
    }
    function reset() {                                                                       // Reset()
        S.files = []; S.removedAtt = []; S.existing = []; S.remove = []; S.id = 0; S.updateExtraIdx = -1; S.updateIdx = -1;
        cb.cust.clear(); cb.bill.clear(); cb.item.clear(); cb.uom.clear(); cb.rate.clear(); cb.tax.clear(); cb.job.clear();
        resetEntryFields();
        show('btnsave', true); show('btnupdate', false); show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        configDefault();
        G.grd.setRows([]);
        G.pay.setRows([]); addPaymentRow();
        resetExtra(); G.ext.setRows([]);
        show('BtnSaveAs', false);
        enable(cb.status, false);
        $('DocDate').focus();
        return SE.api(API + '/next-no').then(function (r) { if (r.nextNo > 0) $('txtdocno').value = String(r.nextNo); }).catch(fail);
    }
    function refresh() {                                                                     // toolStripButton1_Click
        return SE.api(API + '/refresh').then(function (d) {
            applyLists(d);
            multiCurrency();
            fillBranches(); fillProjects(); fillCustomers(true); fillTerms(); fillItems(); fillJobLots(); fillCities(); fillCurrencies(); configDefault();
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ Insert */
    function formValid() {                                                                   // FormValidation()
        function stop(m, f) { return msg(m).then(function () { if (f) f(); return false; }); }
        if (!cb.branch.row() || !(vid(cb.branch) > 0)) return stop('branch field is required', function () { cb.branch.focus(); });
        if (!cb.project.row() || !(vid(cb.project) > 0)) return stop('Project Field is Required', function () { cb.project.focus(); });
        if (!cb.cust.row() || !(vid(cb.cust) > 0)) return stop('Supplier Name Field is Required', function () { cb.cust.focus(); });
        if (!cb.term.row()) return stop('Payment Term Field is Required', function () { cb.term.focus(); });
        if ((cb.term.text() || '').trim() === 'Credit' && SE.toInt($('txtduedays').value) === 0) return stop('Due Days Field is Required', function () { $('txtduedays').focus(); });
        if (!cb.dterm.row()) return stop('Delivery Term Field is Required', function () { cb.dterm.focus(); });
        if (S.multi) {
            if (!cb.cur.row() || !(vid(cb.cur) > 0)) return stop('Fcy Code Field is Required', function () { cb.cur.focus(); });
            if (field('txtExchangeRate') === '' || field('txtExchangeRate') === '0') return stop('Exchange Rate Field is Required', function () { $('txtExchangeRate').focus(); });
            if (field('txtFcyAmount') === '' || field('txtFcyAmount') === '0') return stop('Fcy Amount Rate Field is Required', function () { $('txtFcyAmount').focus(); });
        } else {
            if (!cb.cur.row() || !(vid(cb.cur) > 0)) return stop('Please Configure Your Base Currency In configurations', function () { cb.cur.focus(); });
            if (field('txtExchangeRate') === '' || field('txtExchangeRate') === '0') return stop('Please Configure Your Base Currency Rate In configurations', function () { $('txtExchangeRate').focus(); });
        }
        if (!cb.bill.row()) return stop('Bill Type Field is Required', function () { cb.bill.focus(); });
        return Promise.resolve(true);
    }
    function paymentChecks() {                                                               // the grid / payment checks of Insert(), in order
        var rows = G.grd.rows(), n = 0;
        for (var i = 0; i < rows.length; i++) {
            var r = rows[i], dt = +r.DiscountType || 0;
            if (dt > 0 && !(+r['Discount%'] > 0) && !(+r.DiscountAmount > 0)) dt = 0;
            if (dt > 0 && +r['Discount%'] > 0 && !(+r.DiscountAmount > 0)) return 'Discount Amount Required In Row#' + (i + 1) + ' In Detail Grid';
            if (+r.TaxNameId > 0 && +r.TaxPercent > 0 && !(+r.TaxAmount > 0)) return 'TaxAmount Required In Row#' + (i + 1) + ' In Detail Grid';
        }
        var pays = G.pay.rows(), amt = 0, pct = 0;
        if (pays.length <= 0) { tabs2.select('tabPagePaymentDetail'); return 'Payment Detail Record Not Found'; }
        for (var k = 0; k < pays.length; k++) {
            var p = pays[k];
            if (+p.Amount > 0) {
                pct += +p['%ofTotal'] || 0; amt += +p.Amount || 0;
                if (!(+p.PaymentTermId > 0)) return 'Payment Term Required in row#' + (k + 1);
                if (+p.PaymentTermId === 2 && !(+p.DueDays > 0)) { tabs2.select('tabPagePaymentDetail'); return 'Due Days Required In case Of Credit row in row#' + (k + 1); }
            }
        }
        var order = num('txtOrderAmountHeader');
        if (Math.abs(amt - order) > 1e-9) { tabs2.select('tabPagePaymentDetail'); return 'Payment Detail Amount ' + f4(amt) + ' Not Equal to Total Order Amount ' + f4(order); }
        if (Math.abs(pct - 100) > 1e-9) return 'Payment Detail Total% not equal to 100';
        return '';
    }
    function insert() {
        var first = G.grd.count() === 0 ? msg('Grid Record Not Found') : Promise.resolve();
        return first.then(formValid).then(function (ok) {
            if (!ok) return;
            return SE.ask(S.id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
                if (!yes) return;
                var bad = paymentChecks();
                if (bad) return msg(bad, /^Payment Detail (Record|Amount|Total)|^Payment Term|^Due Days/.test(bad) ? 'Warning Message' : 'Message');
                var body = {
                    id: S.id, docDate: $('DocDate').value, docNo: field('txtdocno'), branchId: vid(cb.branch), projectId: vid(cb.project), customerId: vid(cb.cust),
                    supplierRefNo: $('txtsupprefno').value, remarks: $('txtremarks').value, paymentTermId: vid(cb.term), dueDays: SE.toInt($('txtduedays').value),
                    dueDate: $('duedate').value, expiryDate: $('validate').value, deliveryTerm: cb.dterm.text(), deliveryStartDate: $('deliverystartdate').value,
                    deliveryDays: SE.toInt($('txtdeliverydays').value), status: cb.status.text(), billTypeId: vid(cb.bill), currencyId: vid(cb.cur),
                    exchangeRateText: field('txtExchangeRate'), fcyAmountText: field('txtFcyAmount'), exchangeRate: num('txtExchangeRate'), discountHeader: num('txtDiscountHeader'),
                    lines: G.grd.rows().map(lineOf), removed: S.remove,
                    extras: G.ext.rows().map(function (r) { return { id: +r.Id || 0, masterItemId: +r.MasterItemId || 0, masterPackUomId: +r.MasterPackUomId || 0, itemId: +r.ItemId || 0,
                        itemDescription: r.ItemDescription == null ? '' : String(r.ItemDescription), remarks: r.Remarks == null ? '' : String(r.Remarks), jobLotId: +r.JobLotId || 0,
                        packUomId: +r.PackUomId || 0, itemQty: +r.ItemQty || 0, weight: +r.Weight || 0 }; }),
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
        return SE.api(API + '/by-doc-no' + SE.q({ docNo: SE.toInt($('txtdocno').value.trim()) })).then(function (r) {
            if (!(r.id > 0)) return msg('Record Not Exist against this Number');
            return insert();
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ ReadById */
    function edit(id) {
        S.id = id;
        return SE.api(API + '/' + id).then(function (d) {
            var h = d.head;
            S.remove = [];
            tabs.select('tabPage1');
            cb.branch.setValue(+h.BranchesId); cb.project.setValue(+h.ProjectsId);
            $('DocDate').value = SE.dateInput(h.DocDate); $('txtdocno').value = h.DocNo;
            cb.cust.setValue(+h.OrderSupCustId); $('txtsupprefno').value = h.SupplierRefNo || ''; $('txtremarks').value = h.RemarksHeader || '';
            cb.cur.setValue(+h.CurrencyId || 0);
            $('txtOrderQty').value = f2(+h.OrderQty || 0); $('txtOrderWeight').value = f2(+h.OrderWeight || 0);
            $('txtItemNetAmountHeader').value = fmtAmt(+h.ItemAmountHeader || 0); $('txtTaxAmountHeader').value = fmtAmt(+h.TaxAmountHeader || 0);
            $('txtDiscountAmount').value = fmtAmt(+h.DiscountAmountHeader || 0);                // desktop writes the header discount into the detail box
            $('txtOrderAmountHeader').value = fmtAmt(+h.OrderAmount || 0); $('txtFcyAmount').value = fmtFcy(+h.FcyAmount || 0);
            cb.term.setValue(+h.PaymentTermsId || 0); termChanged();
            $('txtduedays').value = h.OrderDueDays == null ? '' : String(h.OrderDueDays); dueDaysChanged();
            $('duedate').value = SE.dateInput(h.OrderDueDate) || $('duedate').value; dueDateChanged();
            $('validate').value = SE.dateInput(h.OrderExpiryDate) || $('validate').value;
            var dt = (h.DeliveryTerm || '').trim(); (S.dtRows || [{ Id: 1, DeliveryTerm: 'Load' }, { Id: 2, DeliveryTerm: 'Ponch' }]).forEach(function (x) { if (x.DeliveryTerm === dt) cb.dterm.setValue(x.Id); });
            $('deliverystartdate').value = SE.dateInput(h.DeliveryStartDate) || $('deliverystartdate').value;
            $('txtdeliverydays').value = h.DeliveryDays == null ? '' : String(h.DeliveryDays);
            var stt = (h.OrderStatus || '').trim(); [{ Id: 1, Status: 'Open' }, { Id: 2, Status: 'Complete' }, { Id: 3, Status: 'Cancel' }].forEach(function (x) { if (x.Status === stt) cb.status.setValue(x.Id); });
            cb.bill.setValue(+h.BillCalculateTypeId || 0);
            enable(cb.status, true);
            if (+h.CurrencyId > 0) $('txtExchangeRate').value = String(h.ExchangeRate == null ? '' : h.ExchangeRate); else configDefault();
            G.grd.setRows((d.lines || []).map(function (l) {
                return { Id: l.Id, ItemId: l.OrderItemId, ItemCode: l.ItemCodeNew, ItemName: l.ItemName, ItemDescription: l.ItemDiscription, Remarks: l.OrderRemarks, JobLotId: l.JobLotId,
                    JobLot: l.JobLotDescription, PackUomId: l.OrderItemUOMId, PackUom: l.UOMCode, ItemQty: l.OrderItemQty, Weight: l.NetWeight, ItemRate: l.OrderItemRate,
                    RateUomId: l.OrderItemRateUOMId, RateUom: l.RateUom, DiscountType: l.DiscountTypeId, 'Discount%': l.ItemDiscount, DiscountAmount: l.ItemDiscountAmount, Amount: l.Amount,
                    FcyAmount: l.FcyAmount, TaxNameId: l.TaxNameId, TaxName: l.TaxName, TaxPercent: l.TaxPercent, TaxAmount: l.TaxAmount, TotalAmount: l.TotalAmount, CityId: l.CityId, CityName: l.CityArea };
            }));
            G.ext.setRows((d.extras || []).map(function (e) {
                return { Id: e.Id, MasterItemId: e.MasterItemId, MasterItemCode: e.MasterItemCode, MasterItemName: e.MasterItemName, MasterPackUomId: e.MasterPackUomId, MasterPackUom: e.MasterPackUomCode,
                    ItemId: e.ItemId, ItemCode: e.ItemCode, ItemName: e.ItemName, ItemDescription: e.ItemDiscription, Remarks: e.Remarks, JobLotId: e.JobLotId, JobLot: e.JobLotDescription,
                    PackUomId: e.PackUomId, PackUom: e.PackUomCode, ItemQty: e.ItemQty, Weight: e.NetWeight };
            }));
            G.pay.setRows((d.payments || []).map(function (p) {
                return { PaymentTermId: p.PaymentTermId, '%ofTotal': p.PrcntOfTotal, Amount: p.Amount, DueDays: p.DueDays, DueDate: SE.dateInput(p.DueDate), Remarks: p.PaymentRemarks };
            }));
            if (G.pay.count() === 0) addPaymentRow();
            S.existing = d.attachments || []; S.files = []; S.removedAtt = [];
            show('btnsave', false); show('BtnSaveAs', false); show('btnupdate', true);
            exchangeChanged(); fillMasterItems();
        }).catch(fail);
    }
    function histEdit(r) { return edit(+r.Id); }
    function histSaveAs(r) {                                                                 // grdhistory "SaveAs"
        return edit(+r.Id).then(function () {
            show('btnsave', false); show('btnupdate', false); show('BtnSaveAs', true);
            G.grd.rows().forEach(function (x) { x.Id = 0; }); G.grd.refresh();
        });
    }

    /* ------------------------------------------------------------------ history (gridhistoryfill, grdhistory_SelectionChanged) */
    function showHistory() {
        var dt = $('rdentrydate').checked ? 'entry' : ($('rdmodifydate').checked ? 'modify' : ($('rdapproveddate').checked ? 'approved' : 'document'));
        if ($('drdocdate').checked) dt = 'document';
        var q = { dateType: dt, fromDate: $('FromDateHistory_chk').checked ? $('FromDateHistory').value : '', toDate: $('ToDateHistory_chk').checked ? $('ToDateHistory').value : '',
            fromNo: SE.toInt($('FromDocNoHistory').value), toNo: SE.toInt($('ToDocNoHistory').value), customerId: vid(cb.hCust) };
        return SE.api(API + '/history' + SE.q(q)).then(function (rows) {
            if (!rows.length) return;                                                        // the desktop leaves the grid as it is
            G.hist.setRows(rows.map(function (r) {
                return { Id: r.Id, DocNo: r.DocNo, DocDate: r.DocDate, CustomerName: r.CustomerName, ItemNetAmount: r.ItemAmountHeader, TaxAmount: r.TaxAmountHeader, DiscountAmount: r.DiscountAmountHeader,
                    OrderNetAmount: r.OrderAmount, PaymentTerms: r.TermsDescription, DueDays: r.DueDays, DueDate: r.DueDate, DeliveryTerm: r.DeliveryTerm, DeliveryDays: r.DeliveryDays,
                    OrderStatus: r.OrderStatus, OrderExpiryDate: r.OrderExpiryDate, RemarksHeader: r.RemarksHeader, EntryUser: r.UserName, EntryDate: r.EntryDate, ModifyUser: r.ModifyUserName,
                    ModifyDate: r.ModifyDate, ApprovedUser: r.ApprovedUser, ApprovedDate: r.PostDate, NoOfAttachments: r.NoOfAttachments };
            }));
        }).catch(function (e) { return msg(e.message, 'Error Message'); });
    }
    function resetHistory() {                                                                // btnResetHistory_Click
        $('FromDateHistory').value = SE.addDays(-3); $('ToDateHistory').value = SE.today();
        $('FromDocNoHistory').value = ''; $('ToDocNoHistory').value = '';
        cb.hCust.clear(); G.hist.setRows([]); $('drdocdate').checked = true;
    }
    function refreshHistoryCombos() {
        return SE.api(API + '/history-combos').then(function (h) { cb.hCust.setData(h.customers || []); }).catch(function (e) { return msg(e.message, 'Error Message'); });
    }
    function histSelected(item) {
        if (!item) { G.hd.setRows([]); return; }
        SE.api(API + '/' + item.Id + '/history-detail', { quiet: true }).then(function (rows) {
            if (!G.hist.cur() || +G.hist.cur().Id !== +item.Id) return;
            G.hd.setRows(rows.map(function (l) {
                return { ItemCode: l.ItemCodeNew, ItemName: l.ItemName, ItemDescription: l.ItemDiscription, 'Specification/Remarks': l.OrderRemarks, JobLot: l.JobLotDescription, PackUom: l.UOMCode,
                    ItemQty: l.OrderItemQty, Weight: l.NetWeight, ItemRate: l.OrderItemRate, RateUom: l.RateUom, DiscountType: l.DiscountType, 'Discount%': l.ItemDiscount,
                    DiscountAmount: l.ItemDiscountAmount, Amount: l.Amount, FcyAmount: l.FcyAmount, TaxName: l.TaxName, TaxPercent: l.TaxPercent, TaxAmount: l.TaxAmount, CityName: l.CityArea };
            }));
        }).catch(function () { G.hd.setRows([]); });
    }

    /* ------------------------------------------------------------------ print / attachments */
    function print(id) {                                                                     // GeneratePrint -> CommonServices.SaleOrderSlipEngr_1605
        if (!(id > 0)) return msg('No Record Selected');
        SE.printRpt('1605-SaleOrderSlipAndRegister_Engr.rpt', { id: id, branchesId: S.branchId, documentTypeIds: '1605' });
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

    /* ------------------------------------------------------------------ keys / wiring */
    var SHORT = [['Ctrl+S', 'For Save and When on Entry form and For Show Data when on History Form'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'],
        ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+D', 'For Adding an row in Focused Grid'], ['Ctrl+Delete', 'For Deleting an row of Focused Grid'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On on Detail Entry First Column'], ['Ctrl+ArrowRight', 'to change focus from one grid to another'],
        ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ["Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function gridDelete(e) {                                                                 // Ctrl+Delete on the entry grids
        var t = e.target && e.target.closest ? e.target.closest('.dgrid') : null; if (!t) return false;
        var g = t.id === 'grd' ? G.grd : (t.id === 'grdExtraItems' ? G.ext : (t.id === 'grdPaymentTerm' ? G.pay : null)); if (!g || g.curIndex() < 0) return false;
        e.preventDefault();
        if (g === G.grd) deleteRow(g.curIndex()); else if (g === G.ext) deleteExtra(g.curIndex());
        else SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) { if (yes) deletePay(g.curIndex()); });
        return true;
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
            if (e.ctrlKey && e.key === 'Delete') { gridDelete(e); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); if (isShown('btnsave') && !$('btnsave').disabled) btnSave(); }
            else if (e.ctrlKey && k === 'u') { e.preventDefault(); if (isShown('btnupdate') && !$('btnupdate').disabled) btnUpdate(); }
            else if (e.ctrlKey && k === 'p') { e.preventDefault(); if (!$('btnprint').disabled) print(S.id); }
            else if (e.ctrlKey && k === 'd') { var tg = t.closest ? t.closest('#grdPaymentTerm') : null; if (tg) { e.preventDefault(); addPaymentRow(); } }
            else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); $('DocDate').focus(); }
            else if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); openAttachmentDialog(); }
            else if (e.ctrlKey && e.key === 'F12') { e.preventDefault(); if (isShown('BtnSaveAs') && !$('BtnSaveAs').disabled) btnSaveAs(); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { if (tabs2.index() === 1) $('grdPaymentTerm').focus(); else $('grd').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowUp') { cb.item.focus(); }
            else if (e.ctrlKey && e.key === 'ArrowRight') {
                if (tabs2.index() === 0) { tabs2.select('tabExtraItems'); $('grdPaymentTerm').focus(); } else if (tabs2.index() === 1) { tabs2.select('tabPageDetail'); cb.item.focus(); }
            }
        } else if (tabs.index() === 1) {
            if (e.ctrlKey && e.key === 'ArrowDown') {
                if (document.activeElement === $('grdDetail')) $('grdhistory').focus(); else $('grdDetail').focus();
            } else if (e.ctrlKey && k === 's') { e.preventDefault(); showHistory(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); resetHistory(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refreshHistoryCombos(); }
            else if (e.ctrlKey && e.key === 'Enter' && G.hist.cur()) {
                e.preventDefault();
                if (!S.rights.update) return msg("ypu don't have updae rights...");
                histEdit(G.hist.cur());
            }
        }
    }
    function wire() {
        $('btnnew').onclick = reset; $('toolStripButton1').onclick = refresh; $('btnsave').onclick = btnSave; $('btnupdate').onclick = btnUpdate; $('BtnSaveAs').onclick = btnSaveAs;
        $('btnattachment').onclick = openAttachmentDialog; $('btnprint').onclick = function () { print(S.id); };
        $('btnshortcutkeys').onclick = function () { SE.shortcuts(SHORT); };
        $('btnCity').onclick = function () { window.open('/master-data/city', '_blank'); };                      // DefineCity
        $('btnplus').onclick = plus; $('btnUpdateDetail').onclick = updateDetail; $('btnCancelUpdateDetial').onclick = cancelUpdateDetail;
        $('btnAddInExtra').onclick = addExtra; $('BtnUpdateInExtra').onclick = updateExtra; $('BtnCancelInExtra').onclick = resetExtra;
        $('btnResetHistory').onclick = resetHistory; $('btnRefreshHistory').onclick = refreshHistoryCombos; $('btnShow').onclick = showHistory;
        $('rdbtnItemName').addEventListener('change', function () { var id = vid(cb.item); bindItems(); if (id > 0) { cb.item.setValue(id); cb.item.focus(); } });
        $('rdbtnItemCode').addEventListener('change', function () { var id = vid(cb.item); bindItems(); if (id > 0) { cb.item.setValue(id); cb.item.focus(); } });
        ['RadExtraItemName', 'RadExtraItemCode'].forEach(function (x) { $(x).addEventListener('change', function () { var id = vid(cb.xItem); bindExtraItems(); if (id > 0) { cb.xItem.setValue(id); cb.xItem.focus(); } }); });
        ['RadMasterItemName', 'RadMasterItemCode'].forEach(function (x) { $(x).addEventListener('change', function () { var id = vid(cb.mItem); bindMasterItems(); if (id > 0) { cb.mItem.setValue(id); cb.mItem.focus(); } }); });
        ['txtqty'].forEach(function (x) { $(x).addEventListener('input', calcAll); });
        ['txtRate', 'txtDiscPercent'].forEach(function (x) { $(x).addEventListener('input', calcAmountTax); });
        $('txtTaxPercent').addEventListener('input', calcTax);
        $('txtQtyInExtra').addEventListener('input', calcWeightExtra);
        $('txtExchangeRate').addEventListener('input', exchangeChanged);
        $('txtExchangeRate').addEventListener('blur', function () { $('txtExchangeRate').value = fmtRate(num('txtExchangeRate')); });
        $('txtDiscountHeader').addEventListener('input', exchangeChanged);
        $('txtduedays').addEventListener('input', dueDaysChanged);
        $('duedate').addEventListener('change', dueDateChanged);
        $('deliverystartdate').addEventListener('change', deliveryStartChanged);
        $('DocDate').addEventListener('change', docDateLeave);
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
        var hook = false;
        document.addEventListener('keydown', function (e) { if (e.ctrlKey && e.altKey && !hook) { hook = true; SE.shortcuts(SHORT); setTimeout(function () { hook = false; }, 400); } });
    }

    document.addEventListener('DOMContentLoaded', load);
})();
