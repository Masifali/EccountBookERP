/* ============================================================================================
 * Screen 868 "Stock In Transit" — frmSupplierDispatchPreBill.cs (Architecture.WinApp.Purchase),
 * DocumentTypeId 251. Line references (:n) are frmSupplierDispatchPreBill.cs.
 *
 * The arithmetic is the form's own, event for event: the detail-entry calculations (CalculateWeight,
 * Total, TotalAmount), the grid-edit calculations (grd_CellUpdated / UpdateCalculatedFields), the
 * commission / brokery amounts, the expense and commission proportions and the bill footers. The
 * server recomputes every total, amount and FcyAmount it saves (it never trusts the disabled boxes)
 * and re-validates every reference; the Doc No shown here is display only.
 *
 * .NET Math.Round(double) is banker's rounding (roundEven); a custom numeric format string rounds
 * away from zero (roundAway).
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/purchase/stock-in-transit';
    var PRINT_RPT = '251_SupplierDispatchPreBillSlip.rpt';
    var MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var DEFAULT_TEXT = '-- Select --';

    /* Form-specific combo columns (countx_prod_combo.js exposes the same DesktopCombo.define API). */
    if (window.DesktopCombo) {
        /* ItemNameBind (:1113) — AllColumns over dtitem (Id, ItemName, ItemCode); display ItemName or ItemCode. */
        window.DesktopCombo.define('sitItem', [
            { caption: 'Item Name', flex: 4 },
            { caption: 'ItemName', flex: 4, key: 'item-name' },
            { caption: 'ItemCode', flex: 2, key: 'item-code' }
        ]);
        /* PackUomFromGlobalBind (:1160) — the item's UOM schedule: UOMCode, Equivalent. */
        window.DesktopCombo.define('sitUom', [
            { caption: 'UOM', flex: 3 },
            { caption: 'Equivalent', flex: 2, key: 'eq', type: 'num' }
        ]);
    }

    var L = null;                       // lookups
    var st = {
        recId: 0, approved: false, rows: [], exp: [], updateIdx: -1,
        uoms: [], hist: [], histSel: -1, statusMode: false, formInitialized: false
    };

    // ============================================================================ helpers

    function $(id) { return document.getElementById(id); }
    function esc(v) {
        return String(v === undefined || v === null ? '' : v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    /** Conversion.ToDouble on text — thousands separators allowed, anything else 0. */
    function num(v) {
        if (v === null || v === undefined) return 0;
        if (typeof v === 'number') return isFinite(v) ? v : 0;
        var s = String(v).replace(/,/g, '').trim();
        if (s === '') return 0;
        var n = Number(s);
        return isFinite(n) ? n : 0;
    }
    function intOf(v) { var n = parseInt(String(v === undefined || v === null ? '' : v).replace(/,/g, ''), 10); return isNaN(n) ? 0 : n; }
    function roundEven(x, n) {
        var f = Math.pow(10, n || 0), y = x * f, r = Math.round(y);
        if (Math.abs(y % 1) === 0.5) r = 2 * Math.round(y / 2);
        return r / f;
    }
    function roundAway(x, n) {
        var f = Math.pow(10, n || 0);
        return Math.sign(x) * Math.round(Math.abs(x) * f + 1e-9) / f;
    }
    /** double.ToString() */
    function netStr(v) {
        if (!isFinite(v)) return '';
        return String(parseFloat(Number(v).toPrecision(15)));
    }
    function group(x, minD, maxD) {
        return x.toLocaleString('en-US', { minimumFractionDigits: minD, maximumFractionDigits: maxD });
    }
    function dec() { return (L && L.decimals) || { amount: 0, rate: 2, fcy: 0 }; }
    /** x.ToString(clsGlobalVariables.stringFormatsingle) — "#,##0." + N zeros. */
    function fmtAmt(v) { var n = dec().amount; return group(roundAway(num(v), n), n, n); }
    /** clsGlobalVariables.DecimalRateFormate — "#,#0." + N zeros. */
    function fmtRate(v) { var n = dec().rate; return group(roundAway(num(v), n), n, n); }
    /** stringFormatsingleForFcy — the FCY decimal count. */
    function fmtFcy(v) { var n = dec().fcy; return group(roundAway(num(v), n), n, n); }
    /** "#,##0.##" / "#,##0.###" / "#,##0.####" */
    function fmtHash(v, n) { return group(roundAway(num(v), n), 0, n); }
    function pad(n) { return String(n).padStart(2, '0'); }
    function isoOf(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function today() { return isoOf(new Date()); }
    function addDaysIso(iso, days) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(iso || '');
        var d = m ? new Date(+m[1], +m[2] - 1, +m[3]) : new Date();
        d.setDate(d.getDate() + days);
        return isoOf(d);
    }
    function daysBetween(a, b) {
        var ma = /^(\d{4})-(\d{2})-(\d{2})/.exec(a || ''), mb = /^(\d{4})-(\d{2})-(\d{2})/.exec(b || '');
        if (!ma || !mb) return 0;
        return Math.round((Date.UTC(+mb[1], +mb[2] - 1, +mb[3]) - Date.UTC(+ma[1], +ma[2] - 1, +ma[3])) / 86400000);
    }
    function isoDay(v) { var m = /^(\d{4}-\d{2}-\d{2})/.exec(String(v || '')); return m ? m[1] : ''; }
    /** "dd-MMM-yyyy" */
    function dMMMyyyy(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || ''));
        return m ? m[3] + '-' + MONTHS[+m[2] - 1] + '-' + m[1] : '';
    }
    /** "dd-MMM-yyyy hh:mm tt" */
    function dMMMyyyyTime(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})[T ](\d{2}):(\d{2})/.exec(String(v || ''));
        if (!m) return dMMMyyyy(v);
        var h = +m[4], ap = h >= 12 ? 'PM' : 'AM';
        h = h % 12; if (h === 0) h = 12;
        return m[3] + '-' + MONTHS[+m[2] - 1] + '-' + m[1] + ' ' + pad(h) + ':' + m[5] + ' ' + ap;
    }
    function sum(list, key) { var t = 0; (list || []).forEach(function (r) { t += num(r[key]); }); return t; }
    function msg(e) { return e && e.message ? e.message : String(e); }
    function rights() { return (L && L.rights) || {}; }
    function show(el, on) { if (typeof el === 'string') el = $(el); if (el) el.hidden = !on; }

    async function api(method, url, body) {
        var opt = { method: method, credentials: 'same-origin', headers: { Accept: 'application/json' } };
        if (body !== undefined) { opt.headers['Content-Type'] = 'application/json'; opt.body = JSON.stringify(body); }
        var res = await fetch(url, opt);
        var text = await res.text();
        var data = null;
        try { data = text ? JSON.parse(text) : null; } catch (e) { data = null; }
        if (!res.ok) {
            var m = data && (data.message || data.error) || ('The request failed (' + res.status + ').');
            throw new Error(m);
        }
        return data;
    }
    /* The Attachment form (btnattachment_Click :3924 AT.Show()); saved with Insert() (:2267-2278). */
    var AT = window.PurchaseDocAttachments ? PurchaseDocAttachments.create({
        type: 251, getId: function () { return st.recId; },
        canEdit: function () { return st.recId > 0 ? !!rights().update : !!rights().save; },
        message: function (m) { alert(m); }
    }) : null;
    /** countx_purchase_request.js busy panel + button lock; any refusal is shown as the desktop's MessageBox. */
    function act(button, work) {
        var runner = window.PurchaseRequest ? window.PurchaseRequest.run.bind(window.PurchaseRequest) : function (b, w) { return w(); };
        return runner(button || null, async function () {
            try { await work(); } catch (e) { alert(msg(e)); }
        });
    }

    // ============================================================================ combos

    function opt(value, text, attrs) {
        var a = '';
        if (attrs) Object.keys(attrs).forEach(function (k) { a += ' data-' + k + '="' + esc(attrs[k]) + '"'; });
        return '<option value="' + esc(value) + '"' + a + '>' + esc(text) + '</option>';
    }
    function fill(id, rows, valueKey, textKey, withDefault, attrFn) {
        var el = $(id), prev = el.value, html = withDefault ? opt('0', DEFAULT_TEXT) : '';
        (rows || []).forEach(function (r) { html += opt(r[valueKey], r[textKey], attrFn ? attrFn(r) : null); });
        el.innerHTML = html;
        return prev;
    }
    function has(id, v) { return Array.prototype.some.call($(id).options, function (o) { return o.value === String(v); }); }
    /** combo.Value = v — a value not in the list leaves the combo without a selection. */
    function setVal(id, v) {
        var el = $(id);
        if (v !== null && v !== undefined && has(id, v)) el.value = String(v); else el.selectedIndex = -1;
    }
    function comboVal(id) { var el = $(id); return el.selectedIndex < 0 ? 0 : intOf(el.value); }
    function comboRaw(id) { var el = $(id); return el.selectedIndex < 0 ? '' : el.value; }
    /** combo.Text — the default row counts as empty; an unmatched text keeps what was typed. */
    function comboText(id) {
        var el = $(id);
        if (el.selectedIndex < 0) return '';
        var o = el.options[el.selectedIndex];
        if (!o.hasAttribute('data-adhoc') && o.value === '0' && o.textContent === DEFAULT_TEXT) return '';
        return o.textContent;
    }
    function active(id) { return $(id).selectedIndex >= 0; }                      // ActiveRow != null
    function clearCombo(id) { dropAdhoc(id); $(id).selectedIndex = -1; }          // combo.Text = string.Empty
    function dropAdhoc(id) { Array.prototype.slice.call($(id).querySelectorAll('option[data-adhoc]')).forEach(function (o) { o.remove(); }); }
    /**
     * combo.Text = text — selects the row whose display text matches; a text that matches no row stays
     * in the box on the desktop (and becomes its Value), so it is kept here as an ad-hoc entry.
     */
    function setText(id, text) {
        var el = $(id), t = String(text === undefined || text === null ? '' : text).trim();
        dropAdhoc(id);
        if (t === '') { el.selectedIndex = -1; return; }
        for (var i = 0; i < el.options.length; i++) {
            if (el.options[i].textContent.trim().toLowerCase() === t.toLowerCase()) { el.selectedIndex = i; return; }
        }
        el.insertAdjacentHTML('beforeend', '<option value="' + esc(t) + '" data-adhoc="1">' + esc(t) + '</option>');
        el.value = t;
    }
    function selectedAttr(id, attr) {
        var el = $(id);
        if (el.selectedIndex < 0) return '';
        return el.options[el.selectedIndex].getAttribute('data-' + attr) || '';
    }

    function partyAttrs(r) { return { code: r.PartyCode, city: r.CityName, mobile: r.MobileNo }; }

    function bindSuppliers() {                                                      // BindSupplierName (:978)
        ['CmbSupplierDetail', 'CmbCommisionAgent', 'CmbBrokeryAc', 'ldrSupplier'].forEach(function (id) {
            var prev = comboRaw(id);
            fill(id, L.suppliers, 'Id', 'CompanyName', false, partyAttrs);
            setVal(id, prev || null);                                               // insertDefaultRow false, ActivateRow false
        });
    }
    /** ItemNameBind (:1107) / rdSearchByName_CheckedChanged (:1213) — display ItemName or ItemCode. */
    function bindItems(activateFirst) {
        var byCode = $('rdSearchByCode').checked, prev = comboVal('CmbItemNameDetail');
        var el = $('CmbItemNameDetail');
        el.setAttribute('data-dtcombo-caption', 'Item Name');
        var html = opt('0', DEFAULT_TEXT);
        (L.items || []).forEach(function (r) {
            html += opt(r.Id, byCode ? r.ItemCode : r.ItemName, { 'item-name': r.ItemName, 'item-code': r.ItemCode });
        });
        el.innerHTML = html;
        if (prev > 0 && has('CmbItemNameDetail', prev)) el.value = String(prev);
        else if (activateFirst && el.options.length > 1) el.selectedIndex = 1;      // ActivateRow true, index 1
        else el.selectedIndex = -1;
    }
    function bindLists(first) {
        bindSuppliers();
        var ref = comboVal('CmbRefPartyDetail');                                    // ReferancePartyBind: activate row 1
        fill('CmbRefPartyDetail', L.referenceParties, 'Id', 'Name', true);
        if (ref > 0 && has('CmbRefPartyDetail', ref)) $('CmbRefPartyDetail').value = String(ref);
        else if ($('CmbRefPartyDetail').options.length > 1) $('CmbRefPartyDetail').selectedIndex = 1;
        else $('CmbRefPartyDetail').selectedIndex = -1;

        var dt = comboText('CmbDeliveryTerm');                                      // DeliveryTerm (:1027): Rows[2]
        fill('CmbDeliveryTerm', L.deliveryTerms, 'Id', 'Name', true);
        if (first) $('CmbDeliveryTerm').selectedIndex = $('CmbDeliveryTerm').options.length > 2 ? 2 : -1;
        else setText('CmbDeliveryTerm', dt);

        var cur = comboVal('CmbCurrencyMain');                                      // CurrencyBind (:1073)
        fill('CmbCurrencyMain', L.currencies, 'Id', 'Name', true);
        setVal('CmbCurrencyMain', cur || null);

        bindItems(first);

        var crop = comboVal('CmbCropYearDetail');
        fill('CmbCropYearDetail', L.cropYears, 'Id', 'Name', true);
        setVal('CmbCropYearDetail', crop || null);
        var pack = comboVal('CmbPackingTypeDetail');
        fill('CmbPackingTypeDetail', L.packingTypes, 'Id', 'Name', true);
        setVal('CmbPackingTypeDetail', pack || null);
        var city = comboVal('CmbCityNameMain');
        fill('CmbCityNameMain', L.cities, 'Id', 'Name', true);
        setVal('CmbCityNameMain', city || null);
        var br = comboVal('CmbBranchDetail');
        fill('CmbBranchDetail', L.receiverLocations, 'Id', 'Name', true);
        setVal('CmbBranchDetail', br || null);
    }
    /** CommissionTypeFill (:915) / CommissionUOMFill (:936) — fixed lists, no default row. */
    function bindFixedCombos() {
        var types = [{ Id: 1, Name: 'Flat' }, { Id: 2, Name: 'Percent' }, { Id: 3, Name: 'Comm Weight' }];
        var uoms = [{ Id: 1, Name: '40' }, { Id: 2, Name: '50' }, { Id: 3, Name: '60' }, { Id: 4, Name: '100' }];
        fill('CmbCommType', types, 'Id', 'Name', false);
        fill('CmbBrokeryType', types, 'Id', 'Name', false);
        $('CmbCommType').selectedIndex = 1;                                          // Rows[1].Activate()
        $('CmbBrokeryType').selectedIndex = 1;
        fill('CmbCommUOM', uoms, 'Id', 'Name', false);
        fill('CmbBrokeryRateUom', uoms, 'Id', 'Name', false);
        $('CmbCommUOM').selectedIndex = 0;                                           // Rows[0].Activate()
        $('CmbBrokeryRateUom').selectedIndex = 0;
    }
    /** GetConfigurationsFromGlobalAndBindValuesInColumns (:815). */
    function applyColumnDefaults() {
        if (L.defaultCropYear) setVal('CmbCropYearDetail', L.defaultCropYear);
        if (L.defaultPackingType) setVal('CmbPackingTypeDetail', L.defaultPackingType);
        if (L.cityArea > 0) setVal('CmbCityNameMain', L.cityArea);
    }
    /** GetBaseCurrencyAndRate (:2626). */
    function baseCurrencyAndRate() {
        if (comboVal('CmbCurrencyMain') === 0) setVal('CmbCurrencyMain', L.baseCurrency);
        if (num($('txtExchangeRateMain').value) === 0) $('txtExchangeRateMain').value = netStr(num(L.baseCurrencyRate));
        exchangeRateChanged();
    }

    /** PackUomFromGlobalBind (:1152) — the item's UOMs, pack/rate UOM retained by their text. */
    async function packUomBind(itemId) {
        var packText = comboText('CmbPackUomDetail'), rateText = comboText('CmbRateUomDetail');
        var list = itemId > 0 ? await api('GET', API + '/uoms?itemId=' + itemId) : [];
        st.uoms = list || [];
        ['CmbPackUomDetail', 'CmbRateUomDetail'].forEach(function (id, i) {
            fill(id, st.uoms, 'Id', 'UOMCode', true, function (r) { return { eq: r.Equivalent }; });
            var want = i === 0 ? packText : rateText, el = $(id), hit = -1;
            for (var k = 1; k < el.options.length; k++) if (el.options[k].textContent === want && want !== '') { hit = k; break; }
            el.selectedIndex = hit >= 0 ? hit : 0;                                   // ActivateRow true, index 0
        });
        totalAmount();                                                              // CmbRateUomDetail.TextChanged -> combrateuom_TextChanged (:2867)
    }
    function uomEq(id) { return active(id) ? num(selectedAttr(id, 'eq')) : 0; }

    // ======================================================================= feature ui

    function handleFeatures() {
        var branchCol = L.branchFeature && !L.branchImplemented;                     // HandleBranchFeature (:732)
        show('label63', branchCol); show('label8', branchCol);
        showCombo('CmbBranchDetail', branchCol);
        $('txtRemarksdetail').style.width = branchCol ? '602px' : '797px';
        var mc = !!L.multiCurrency;                                                  // HandleMultiCurrencyFeature (:744)
        showCombo('CmbCurrencyMain', mc);
        show('txtExchangeRateMain', mc); show('txtFcyAmountMain', mc);
        show('label15', mc); show('label16', mc); show('label48', mc); show('label43', mc);
        $('txtExchangeRateMain').tabIndex = mc ? 0 : -1;
        if (!mc) baseCurrencyAndRate();
    }
    /** An enhanced combo draws a wrapper beside the hidden select; hide both. */
    function showCombo(id, on) {
        var el = $(id);
        el.hidden = !on;
        var w = el.__dtcombo && el.__dtcombo.wrap;
        if (w) w.style.display = on ? '' : 'none';
    }
    function setComboEnabled(id, on) { $(id).disabled = !on; }

    // ======================================================================= calculations

    /** CalculateWeight (:2720) */
    function calculateWeight() {
        var qty = num($('txtQtyDetail').value);
        if (active('CmbPackUomDetail') && qty > 0) $('txtGrossWeight').value = fmtHash(uomEq('CmbPackUomDetail') * qty, 4);
    }
    /** Total (:2737) */
    function total() {
        var qty = num($('txtQtyDetail').value), gross = num($('txtGrossWeight').value), ebUnit = num($('txtEBUnit').value);
        var ebTotal = 0;
        if (ebUnit * qty > 0) { ebTotal = ebUnit * qty; $('txtEBTotal').value = netStr(ebTotal); }
        else $('txtEBTotal').value = '0';
        var net = gross - ebTotal + num($('txtAddLss').value);
        $('txtNetBillWeightDetail').value = netStr(roundEven(net, 2));
    }
    /** TotalAmount (:2765) — Math.Round(x, 0, AwayFromZero), "#,##0.####". */
    function totalAmount() {
        var net = num($('txtNetBillWeightDetail').value), rate = num($('txtRateDetail').value);
        if (net > 0 && active('CmbRateUomDetail') && rate > 0) {
            var eq = uomEq('CmbRateUomDetail');
            var amt = net / eq * rate;
            $('txtAmountDetail').value = isFinite(amt) ? fmtHash(roundAway(amt, 0), 4) : '';
        }
    }
    function detailCalcAll() { calculateWeight(); total(); totalAmount(); }

    /** txtExchangeRate_TextChanged (:2674) */
    function exchangeRateChanged() {
        var ex = num($('txtExchangeRateMain').value);
        st.rows.forEach(function (r) { r.FcyAmount = ex > 0 ? roundEven(num(r.Amount) / ex, dec().fcy) : 0; });
        billAmount();
        calculateTotalInformation();
        renderGrid();
    }
    /** CalculateTotalInformation (:2648) */
    function calculateTotalInformation() {
        if (st.rows.length > 0) {
            $('txtQtyMain').value = fmtHash(roundEven(sum(st.rows, 'Qty'), 2), 2);
            $('txtTotalWeightMain').value = fmtHash(roundEven(sum(st.rows, 'Weight'), 2), 2);
            $('txtFcyAmountMain').value = fmtFcy(sum(st.rows, 'FcyAmount'));
        } else {
            $('txtQtyMain').value = '0'; $('txtTotalWeightMain').value = '0'; $('txtFcyAmountMain').value = '0';
        }
    }
    /** BillAmount (:2903) */
    function billAmount() {
        var adv = num($('txtAdvanceFreight').value), item = sum(st.rows, 'Amount'), exp = sum(st.exp, 'Amount');
        var bill = item + exp;
        if (comboVal('CmbSupplierDetail') === comboVal('CmbCommisionAgent')) bill += num($('txtcommamount').value);
        if (active('CmbBrokeryAc')) {
            if (num($('txtBrokeryAmount').value) > 0) bill -= num($('txtBrokeryAmount').value);
            else $('txtBrokeryAmount').value = '0';
        } else $('txtBrokeryAmount').value = '0';
        bill += adv;
        $('txtItemAmountFooter').value = fmtAmt(item);
        $('txtAdvanceFreightFooter').value = fmtAmt(adv);
        $('txtExpenseAmountFooter').value = fmtAmt(exp);
        $('txtTotalAmountMain').value = fmtAmt(bill);
        $('txtNetBillAmountFooter').value = fmtAmt(bill);
        billProportion();
    }
    /** ExpProportion (:2946) */
    function expProportion() {
        var t = sum(st.exp, 'Amount'), w = sum(st.rows, 'Weight');
        st.rows.forEach(function (r) { r.Expense = t > 0 ? t / w * num(r.Weight) : 0; });
        billProportion();
    }
    /** BillProportion (:2979) */
    function billProportion() { st.rows.forEach(function (r) { r.BillAmount = num(r.Amount) + num(r.Expense) + num(r.Commission); }); }
    /** CommissionProportion (:2997) */
    function commissionProportion() {
        var t = num($('txtcommamount').value), pct = num($('txtcommRate').value), w = sum(st.rows, 'Weight');
        st.rows.forEach(function (r) {
            if (t > 0) r.Commission = comboText('CmbCommType') === 'Percent' ? num(r.Amount) * pct / 100 : t / w * num(r.Weight);
            else r.Commission = 0;
        });
        billProportion();
    }
    /** TotalCommissionAmount (:3042) */
    function totalCommissionAmount() {
        var rateText = $('txtcommRate').value, type = comboText('CmbCommType'), rate = num(rateText);
        var out = $('txtcommamount');
        if (rateText !== '') out.value = type === 'Flat' ? fmtAmt(rate) : '0';
        if (rateText !== '') {
            if (type === 'Percent' || type === 'Percentage') out.value = fmtAmt(sum(st.rows, 'Amount') * rate / 100);
        } else out.value = '0';
        if (rateText !== '') {
            if (type === 'Comm Weight') {
                var v = sum(st.rows, 'Weight') / num(comboText('CmbCommUOM')) * rate;
                out.value = isFinite(v) ? fmtAmt(v) : '';
            }
        } else out.value = '0';
        commissionProportion();
    }
    /** TotalBrokeryAmount (:3136) — ends with BillAmount. */
    function totalBrokeryAmount() {
        var rateText = $('txtBrokeryRate').value, type = comboText('CmbBrokeryType'), rate = num(rateText);
        var out = $('txtBrokeryAmount');
        if (rateText !== '') { if (type === 'Flat') out.value = fmtAmt(rate); } else out.value = '0';
        if (rateText !== '') { if (type === 'Percent') out.value = fmtAmt(sum(st.rows, 'Amount') * rate / 100); } else out.value = '0';
        if (rateText !== '') {
            if (type === 'Comm Weight') {
                var v = sum(st.rows, 'Weight') / num(comboText('CmbBrokeryRateUom')) * rate;
                out.value = isFinite(v) ? fmtAmt(v) : '';
            }
        } else out.value = '0';
        billAmount();
    }
    /** The five calls every grid change makes (:1496-1500, :1584-1588, :1733-1737). */
    function recalcAll() {
        totalCommissionAmount();
        totalBrokeryAmount();
        commissionProportion();
        expProportion();
        exchangeRateChanged();
    }

    // ========================================================================= main grid

    function orderExists() { return st.rows.some(function (r) { return intOf(r.OrderId) > 0; }); }
    function listName(list, id) { for (var i = 0; i < (list || []).length; i++) if (+list[i].Id === +id) return list[i].Name; return ''; }
    function selectHtml(list, value, attrs, textKey) {
        var h = '<select data-dtcombo="single" ' + attrs + '>' + opt('0', '');
        var found = false;
        (list || []).forEach(function (r) {
            var sel = +r.Id === +value; if (sel) found = true;
            h += '<option value="' + esc(r.Id) + '"' + (sel ? ' selected' : '') + '>' + esc(r[textKey || 'Name']) + '</option>';
        });
        return h.replace('<option value="0">', '<option value="0"' + (found ? '' : ' selected') + '>') + '</select>';
    }

    /** countx_prod_combo: enhance the cell selects of a re-rendered grid at once (not on its 16 ms rescan),
        so the caret can be put back into the combo the operator was in. */
    function enhanceCombos(tableId) {
        if (window.DesktopCombo && DesktopCombo.init) DesktopCombo.init('#' + tableId + ' select[data-dtcombo]');
    }
    /** The data-k / data-act of a focused cell control; a combo's field answers for its hidden select. */
    function cellKey(a) {
        if (!a || !a.getAttribute) return null;
        var w = a.closest && a.closest('.dtcombo-wrap'), s = w && w.querySelector('select');
        if (s) return s.getAttribute('data-k');
        return a.getAttribute('data-k') || a.getAttribute('data-act');
    }
    function focusCell(el) { if (!el) return; var c = el.__dtcombo && el.__dtcombo.input; (c || el).focus(); }

    /** grdSettings (:1599): hidden ids, captions, editable Qty/GrossWeight/EBUnit/AddLss/Remarks, combo columns. */
    function gridColumns() {
        var oe = orderExists();
        var cols = [];
        cols.push({ k: 'Delete', c: 'X', btn: 'X' });
        if (!oe) cols.push({ k: 'Edit', c: 'Edit', btn: 'Edit' });
        if (oe) cols.push({ k: 'OrderNo', c: 'OrderNo' });
        cols.push({ k: 'ItemCode', c: 'ItemCode' }, { k: 'ItemName', c: 'ItemName' },
            { k: 'CropYearId', c: 'Crop Year', combo: 'crop' }, { k: 'PackingTypeId', c: 'Packing Type', combo: 'pack' },
            { k: 'PackUom', c: 'PackUom', combo: 'uom' },
            { k: 'Qty', c: 'Qty', n: 'q', edit: 1, tot: 1 }, { k: 'GrossWeight', c: 'GrossWeight', n: 'q', edit: 1, tot: 1 },
            { k: 'EBUnit', c: 'EBUnit', n: 'q', edit: 1, tot: 1 }, { k: 'EBTotal', c: 'EBTotal', n: 'q', tot: 1 },
            { k: 'AddLss', c: 'AddLss', n: 's', edit: 1 }, { k: 'Weight', c: 'Net Bill Weight', n: 'q', tot: 1 },
            { k: 'RateUom', c: 'RateUom' }, { k: 'Rate', c: 'Rate', n: 'r' }, { k: 'Amount', c: 'Amount', n: 'a', tot: 1 });
        if (L.multiCurrency) cols.push({ k: 'FcyAmount', c: 'FcyAmount', n: 'a', tot: 1 });
        cols.push({ k: 'Remarks', c: 'Remarks', edit: 1 });
        if (L.branchFeature && !L.branchImplemented) cols.push({ k: 'ReceiverLocationId', c: 'Receiver Location', combo: 'loc' });
        cols.push({ k: 'Expense', c: 'Expense', n: 'q', tot: 1 }, { k: 'Commission', c: 'Commission', n: 'q', tot: 1 },
            { k: 'BillAmount', c: 'BillAmount', n: 'a', tot: 1 });
        return cols;
    }
    function numText(kind, v) {
        if (kind === 'a') return fmtAmt(v);
        if (kind === 'r') return fmtRate(v);
        if (kind === 's') return v === null || v === undefined ? '' : netStr(num(v));
        return fmtHash(v, 2);
    }
    function renderGrid() {
        var cols = gridColumns(), t = $('grd');
        t.tHead.innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + esc(c.c) + '</th>'; }).join('') + '</tr>';
        var html = '';
        st.rows.forEach(function (r, i) {
            html += '<tr data-i="' + i + '">' + cols.map(function (c) {
                if (c.btn) return '<td><button type="button" class="gbtn" data-act="' + c.k + '">' + c.btn + '</button></td>';
                if (c.combo === 'crop') return '<td>' + selectHtml(L.cropYears, r.CropYearId, 'data-k="CropYearId"') + '</td>';
                if (c.combo === 'pack') return '<td>' + selectHtml(L.packingTypes, r.PackingTypeId, 'data-k="PackingTypeId"') + '</td>';
                if (c.combo === 'loc') return '<td>' + selectHtml(L.receiverLocations, r.ReceiverLocationId, 'data-k="ReceiverLocationId"') + '</td>';
                if (c.combo === 'uom') {
                    var list = r._uoms || [{ Id: r.PackUomId, UOMCode: r.PackUom, Equivalent: r.PackUomEquivalent }];
                    return '<td>' + selectHtml(list, r.PackUomId, 'data-k="PackUomId" title="F1 on the desktop: pick the pack UOM from the item\'s schedule"', 'UOMCode') + '</td>';
                }
                if (c.edit) {
                    var v = c.n ? (c.n === 's' ? netStr(num(r[c.k])) : netStr(num(r[c.k]))) : (r[c.k] || '');
                    return '<td><input type="text" data-k="' + c.k + '" value="' + esc(v) + '"' + (c.n ? ' class="r" data-decimal' : '') + '></td>';
                }
                if (c.n) return '<td class="n">' + esc(numText(c.n, r[c.k])) + '</td>';
                if (c.k === 'OrderNo') return '<td class="n">' + esc(r.OrderNo || '') + '</td>';
                return '<td>' + esc(r[c.k]) + '</td>';
            }).join('') + '</tr>';
        });
        t.tBodies[0].innerHTML = html;
        t.tFoot.innerHTML = st.rows.length ? '<tr>' + cols.map(function (c) {
            return c.tot ? '<td class="n">' + esc(numText(c.n, sum(st.rows, c.k))) + '</td>' : '<td></td>';
        }).join('') + '</tr>' : '';
        enhanceCombos('grd');
    }

    /** UpdateCalculatedFields (:1809) */
    function updateCalculatedFields(r) {
        var qty = num(r.Qty), gross = qty * num(r.PackUomEquivalent);
        r.GrossWeight = gross;
        var ebUnit = num(r.EBUnit), ebTotal = num(r.EBTotal);
        if (ebTotal === 0 && ebUnit > 0) ebTotal = qty * ebUnit;
        else if (ebUnit === 0 && ebTotal > 0) ebUnit = ebTotal / qty;
        r.EBUnit = ebUnit; r.EBTotal = ebTotal;
        r.Weight = gross - ebTotal + num(r.AddLss);
        var eq = num(r.RateUomEquivalent), amount = eq > 0 ? num(r.Weight) / eq * num(r.Rate) : 0;
        r.Amount = amount;
        r.FcyAmount = num($('txtExchangeRateMain').value) > 0 ? amount / num($('txtExchangeRateMain').value) : 0;
    }
    /** grd_CellUpdated (:1753) — no footer refresh follows, as on the desktop. */
    function gridCellUpdated(r, key) {
        switch (key) {
            case 'Qty': case 'PackUomEquivalent': case 'EBUnit': case 'EBTotal': case 'AddLss':
                updateCalculatedFields(r); break;
            case 'GrossWeight':
                r.Weight = num(r.GrossWeight) - num(r.EBTotal) + num(r.AddLss); break;
            default: break;
        }
        var eq = num(r.RateUomEquivalent), amount = eq > 0 ? num(r.Weight) / eq * num(r.Rate) : 0;
        r.Amount = amount;
        r.FcyAmount = num($('txtExchangeRateMain').value) > 0 ? amount / num($('txtExchangeRateMain').value) : 0;
    }

    function onGridChange(e) {
        var tr = e.target.closest('tr[data-i]'), k = e.target.getAttribute('data-k');
        if (!tr || !k) return;
        var r = st.rows[+tr.getAttribute('data-i')];
        if (!r) return;
        if (k === 'PackUomId') {                                                     // grd_KeyDown F1 on PackUom (:4328)
            var id = intOf(e.target.value), list = r._uoms || [], hit = null;
            list.forEach(function (u) { if (+u.Id === id) hit = u; });
            r.PackUomId = id; r.PackUom = hit ? hit.UOMCode : ''; r.PackUomEquivalent = hit ? num(hit.Equivalent) : 0;
            updateCalculatedFields(r);
        } else if (k === 'CropYearId' || k === 'PackingTypeId' || k === 'ReceiverLocationId') {
            r[k] = intOf(e.target.value);
            gridCellUpdated(r, k);
        } else if (k === 'Remarks') {
            r.Remarks = e.target.value;
            gridCellUpdated(r, k);
        } else {
            r[k] = k === 'AddLss' ? e.target.value : num(e.target.value);
            gridCellUpdated(r, k);
        }
        /* Re-render after the focus has moved, then put the caret back where the operator went. */
        setTimeout(function () {
            var a = document.activeElement, ai = -1, ak = null;
            var atr = a && a.closest ? a.closest('#grd tbody tr[data-i]') : null;
            if (atr) { ai = +atr.getAttribute('data-i'); ak = cellKey(a); }
            renderGrid();
            if (ai >= 0 && ak) {
                var back = $('grd').querySelector('tbody tr[data-i="' + ai + '"] [data-k="' + ak + '"], tbody tr[data-i="' + ai + '"] [data-act="' + ak + '"]');
                focusCell(back);
            }
        }, 0);
    }
    async function onGridFocusUom(e) {
        var sel = e.target, w = sel.closest && sel.closest('.dtcombo-wrap');
        if (w) sel = w.querySelector('select');
        if (!sel || sel.getAttribute('data-k') !== 'PackUomId') return;
        var tr = sel.closest('tr[data-i]'), r = tr && st.rows[+tr.getAttribute('data-i')];
        if (!r || r._uoms) return;
        if (intOf(r.ItemId) <= 0) { alert('Please Select an Item First'); return; }
        try {
            r._uoms = await api('GET', API + '/uoms?itemId=' + intOf(r.ItemId));
            var keep = r.PackUomId;
            sel.innerHTML = opt('0', '') + r._uoms.map(function (u) { return '<option value="' + esc(u.Id) + '"' + (+u.Id === +keep ? ' selected' : '') + '>' + esc(u.UOMCode) + '</option>'; }).join('');
        } catch (err) { alert(msg(err)); }
    }
    function onGridClick(e) {
        var b = e.target.closest('button[data-act]');
        if (!b) return;
        var tr = b.closest('tr[data-i]'), i = +tr.getAttribute('data-i');
        if (b.getAttribute('data-act') === 'Delete') deleteDetailRow(i);
        else gridDoubleClick(i);
    }

    /**
     * DeleteDetailRow (:1533). A saved row (Id > 0) fails on the desktop: after "Are you sure to Delete?"
     * it reads r.Cells["CommissionAgentId"], a column the grid does not have (:1556), and the exception
     * ends the method before the row is removed. Reproduced (see the report / service note W5).
     */
    function deleteDetailRow(i) {
        var r = st.rows[i];
        if (!r) return;
        if (st.updateIdx !== -1) { alert('Please Reset the Detail First...'); return; }
        if (intOf(r.Id) > 0) {
            if (!confirm('Are you sure to Delete?')) return;
            alert('Object reference not set to an instance of an object.');
            return;
        }
        st.rows.splice(i, 1);
        recalcAll();
    }

    /** grd_DoubleClick (:1662) — only while no row comes from an order. */
    async function gridDoubleClick(i) {
        var r = st.rows[i];
        if (!r || orderExists()) return;
        st.updateIdx = i;
        await supplierLeave();
        $('CmbItemNameDetail').value = String(r.ItemId);
        await packUomBind(intOf(r.ItemId));
        setVal('CmbRateUomDetail', r.RateUomId);
        setVal('CmbPackUomDetail', r.PackUomId);
        setVal('CmbCropYearDetail', r.CropYearId);
        setVal('CmbPackingTypeDetail', r.PackingTypeId);
        $('txtQtyDetail').value = fmtHash(r.Qty, 3);
        $('txtGrossWeight').value = fmtHash(r.GrossWeight, 3);
        $('txtEBUnit').value = fmtHash(r.EBUnit, 3);
        $('txtEBTotal').value = fmtHash(r.EBTotal, 3);
        $('txtAddLss').value = fmtHash(r.AddLss, 3);
        $('txtNetBillWeightDetail').value = fmtHash(r.Weight, 3);
        $('txtRateDetail').value = fmtHash(r.Rate, 3);
        setVal('CmbRateUomDetail', r.RateUomId);
        $('txtAmountDetail').value = fmtHash(r.Amount, 3);
        $('txtRemarksdetail').value = r.Remarks || '';
        setVal('CmbBranchDetail', r.ReceiverLocationId);
        show('btnplus', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
        $('CmbItemNameDetail').focus();
    }

    // ============================================================================ detail

    /** FormValidationDetail (:1392) — messages verbatim. */
    function formValidationDetail() {
        var checks = [
            [!active('CmbSupplierDetail') || comboVal('CmbSupplierDetail') === 0, 'Supplier Name Field is Required', 'CmbSupplierDetail'],
            [!active('CmbItemNameDetail') || comboVal('CmbItemNameDetail') === 0, 'Item Name Field is Required', 'CmbItemNameDetail'],
            [!active('CmbCropYearDetail') || comboVal('CmbCropYearDetail') === 0, 'Crop Year Field is Required', 'CmbCropYearDetail'],
            [!active('CmbPackingTypeDetail') || comboVal('CmbPackingTypeDetail') === 0, 'Pack Type Field is Required', 'CmbPackingTypeDetail'],
            [!active('CmbPackUomDetail') || comboVal('CmbPackUomDetail') === 0, 'Item Pack Unit Field is Required', 'CmbPackUomDetail'],
            [emptyOrZero('txtQtyDetail'), 'Qty Field is Required', 'txtQtyDetail'],
            [emptyOrZero('txtGrossWeight'), 'Gross Weight Field is Required', 'txtGrossWeight'],
            [emptyOrZero('txtNetBillWeightDetail'), 'Net Bill Weight Field is Required', 'txtNetBillWeightDetail'],
            [emptyOrZero('txtRateDetail'), 'Item Rate Field is Required', 'txtRateDetail'],
            [!active('CmbRateUomDetail') || comboVal('CmbRateUomDetail') === 0, 'Rate UOM Field is Required', 'CmbRateUomDetail'],
            [emptyOrZero('txtAmountDetail'), 'Item amount Field is Required', 'txtAmountDetail'],
            /* BranchFeature only — also when SupplierDispatchBranchWise hides the combo (desktop quirk). */
            [L.branchFeature && (!active('CmbBranchDetail') || comboVal('CmbBranchDetail') === 0), "Receiver's Location Field is Required", 'CmbBranchDetail']
        ];
        for (var i = 0; i < checks.length; i++) {
            if (checks[i][0]) { alert(checks[i][1]); focus(checks[i][2]); return false; }
        }
        return true;
    }
    function emptyOrZero(id) { var t = $(id).value.trim(); return t === '' || t === '0' || num(t) === 0; }
    function focus(id) { var el = $(id); if (!el) return; var w = el.__dtcombo && el.__dtcombo.input; (w || el).focus(); }

    function itemRow() {
        var id = comboVal('CmbItemNameDetail'), it = null;
        (L.items || []).forEach(function (x) { if (+x.Id === id) it = x; });
        return it || { ItemName: '', ItemCode: '' };
    }
    /** btnplus_Click (:1469) */
    function btnPlus() {
        if (orderExists()) { alert('Grid has records from Purchase Order. Manual Entry is not allowed!'); return; }
        if (!formValidationDetail()) return;
        var it = itemRow(), ex = num($('txtExchangeRateMain').value), amount = num($('txtAmountDetail').value);
        st.rows.push({
            Id: 0, OrderId: 0, OrderNo: 0, OrderDetailId: 0, ItemId: comboVal('CmbItemNameDetail'),
            ItemCode: it.ItemCode, ItemName: it.ItemName, CropYearId: comboVal('CmbCropYearDetail'),
            PackingTypeId: comboVal('CmbPackingTypeDetail'), PackUomId: comboVal('CmbPackUomDetail'),
            PackUom: comboText('CmbPackUomDetail').trim(), PackUomEquivalent: uomEq('CmbPackUomDetail'),
            Qty: num($('txtQtyDetail').value), GrossWeight: num($('txtGrossWeight').value), EBUnit: num($('txtEBUnit').value),
            EBTotal: num($('txtEBTotal').value), AddLss: netStr(num($('txtAddLss').value)), Weight: num($('txtNetBillWeightDetail').value),
            RateUomId: comboVal('CmbRateUomDetail'), RateUom: comboText('CmbRateUomDetail'), RateUomEquivalent: uomEq('CmbRateUomDetail'),
            Rate: num($('txtRateDetail').value), Amount: amount, FcyAmount: ex > 0 && amount > 0 ? amount / ex : 0,
            Remarks: $('txtRemarksdetail').value.trim(), ReceiverLocationId: comboVal('CmbBranchDetail'),
            Expense: 0, Commission: 0, BillAmount: 0
        });
        /* :1486-1494 — cleared in this order; clearing Qty runs Total() once more (EB Total -> "0"). */
        clearItem(); clearCombo('CmbPackUomDetail'); clearCombo('CmbPackingTypeDetail');
        $('txtQtyDetail').value = ''; $('txtEBTotal').value = '0';
        $('txtNetBillWeightDetail').value = ''; clearCombo('CmbRateUomDetail');
        $('txtRateDetail').value = ''; $('txtAmountDetail').value = ''; $('txtRemarksdetail').value = '';
        focus('CmbItemNameDetail');
        recalcAll();
    }
    /** btnUpdateDetail_Click (:1702) */
    function btnUpdateDetail() {
        if (!formValidationDetail()) return;
        var r = st.rows[st.updateIdx];
        if (!r) return;
        var it = itemRow();
        r.ItemId = comboVal('CmbItemNameDetail'); r.ItemCode = it.ItemCode; r.ItemName = it.ItemName;
        r.CropYearId = comboVal('CmbCropYearDetail'); r.PackingTypeId = comboVal('CmbPackingTypeDetail');
        r.PackUomId = comboVal('CmbPackUomDetail'); r.PackUom = comboText('CmbPackUomDetail'); r.PackUomEquivalent = uomEq('CmbPackUomDetail');
        r.Qty = num($('txtQtyDetail').value); r.GrossWeight = num($('txtGrossWeight').value); r.EBUnit = num($('txtEBUnit').value);
        r.EBTotal = num($('txtEBTotal').value); r.AddLss = netStr(num($('txtAddLss').value)); r.Weight = num($('txtNetBillWeightDetail').value);
        r.RateUomId = comboVal('CmbRateUomDetail'); r.RateUom = comboText('CmbRateUomDetail'); r.RateUomEquivalent = uomEq('CmbRateUomDetail');
        r.Rate = num($('txtRateDetail').value); r.Amount = num($('txtAmountDetail').value);
        r.Remarks = $('txtRemarksdetail').value; r.ReceiverLocationId = comboVal('CmbBranchDetail');
        r._uoms = null;
        recalcAll();
        resetDetail();
        focus('CmbItemNameDetail');
    }
    /** Item text cleared: combitem_ValueChanged rebinds the (now empty) UOM lists before the next line runs. */
    function clearItem() {
        clearCombo('CmbItemNameDetail');
        packUomBind(0).catch(function () { });                                      // no request for item 0: runs synchronously
    }
    /** ResetDetail (:2574) */
    function resetDetail() {
        st.updateIdx = -1;
        clearItem(); clearCombo('CmbPackUomDetail');
        ['txtQtyDetail', 'txtGrossWeight', 'txtEBUnit', 'txtEBTotal', 'txtAddLss', 'txtNetBillWeightDetail'].forEach(function (id) { $(id).value = ''; });
        clearCombo('CmbRateUomDetail');
        $('txtRateDetail').value = ''; $('txtAmountDetail').value = '';
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
    }

    // ======================================================================== expense grid

    function blankExp() { return { Id: 0, OrderId: 0, OrderExpId: 0, ItemId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' }; }   // AddRowInvExpGrid (:1837)
    /** grdInvExpSettings (:1843): X at 0, + at 1, "Other Item" combo, Qty / Rate / Amount / Remarks. */
    function renderExp() {
        var t = $('grdInvExp');
        t.tHead.innerHTML = '<tr><th>X</th><th>+</th><th>Other Item</th><th>Qty</th><th>Rate</th><th>Amount</th><th>Remarks</th></tr>';
        t.tBodies[0].innerHTML = st.exp.map(function (e, i) {
            return '<tr data-i="' + i + '"><td><button type="button" class="gbtn" data-act="Delete">X</button></td>'
                + '<td><button type="button" class="gbtn" data-act="Add">+</button></td>'
                + '<td>' + selectHtml(L.otherItems, e.ItemId, 'data-k="ItemId"') + '</td>'
                + '<td><input type="text" class="r" data-decimal data-k="Qty" value="' + esc(netStr(num(e.Qty))) + '"></td>'
                + '<td><input type="text" class="r" data-decimal data-k="Rate" value="' + esc(netStr(num(e.Rate))) + '"></td>'
                + '<td><input type="text" class="r" data-decimal data-k="Amount" value="' + esc(netStr(num(e.Amount))) + '"></td>'
                + '<td><input type="text" data-k="Remarks" value="' + esc(e.Remarks || '') + '"></td></tr>';
        }).join('');
        t.tFoot.innerHTML = '<tr><td></td><td></td><td></td><td class="n">' + esc(fmtHash(sum(st.exp, 'Qty'), 2)) + '</td><td></td><td class="n">'
            + esc(fmtAmt(sum(st.exp, 'Amount'))) + '</td><td></td></tr>';
        enhanceCombos('grdInvExp');
    }
    function onExpChange(e) {
        var tr = e.target.closest('tr[data-i]'), k = e.target.getAttribute('data-k');
        if (!tr || !k) return;
        var r = st.exp[+tr.getAttribute('data-i')];
        if (k === 'Remarks') r.Remarks = e.target.value;
        else if (k === 'ItemId') r.ItemId = intOf(e.target.value);
        else r[k] = num(e.target.value);
        if ((k === 'Qty' || k === 'Rate')) r.Amount = num(r.Qty) * num(r.Rate);            // grdInvExp_CellUpdated (:1936)
        expProportion();
        exchangeRateChanged();
        setTimeout(function () {
            var a = document.activeElement, ai = -1, ak = null;
            var atr = a && a.closest ? a.closest('#grdInvExp tbody tr[data-i]') : null;
            if (atr) { ai = +atr.getAttribute('data-i'); ak = cellKey(a); }
            renderExp();
            if (ai >= 0 && ak) { var back = $('grdInvExp').querySelector('tbody tr[data-i="' + ai + '"] [data-k="' + ak + '"]'); focusCell(back); }
        }, 0);
    }
    function onExpClick(e) {                                                          // grdInvExp_ColumnButtonClick (:1897)
        var b = e.target.closest('button[data-act]');
        if (!b) return;
        var i = +b.closest('tr[data-i]').getAttribute('data-i');
        if (b.getAttribute('data-act') === 'Delete') { st.exp.splice(i, 1); if (!st.exp.length) st.exp.push(blankExp()); }
        else st.exp.push(blankExp());
        expProportion();
        exchangeRateChanged();
        renderExp();
    }

    // ============================================================================ header

    /** EnableDisableFeilds (:2612) */
    function enableDisableFields(on) {
        show('PanelDetail', on);
        setComboEnabled('CmbSupplierDetail', on);
        setComboEnabled('CmbDeliveryTerm', on);
    }
    /** ETACalculations (:4503) */
    function etaCalculations() {
        var before = $('txtETAdestinationMain').value;
        $('txtETAdestinationMain').value = addDaysIso($('txtDepartureDateMain').value || today(), intOf($('txtTransitDaysMain').value));
        if ($('txtETAdestinationMain').value !== before) etaChanged();              // ValueChanged fires only on a change
    }
    /** txtETAdestinationMain_ValueChanged (:4530) */
    function etaChanged() {
        $('txtTransitDaysMain').value = String(Math.abs(daysBetween($('txtDepartureDateMain').value, $('txtETAdestinationMain').value)));
    }
    function docDateChanged() { $('txtDepartureDateMain').value = $('txtDocDateMain').value; etaCalculations(); }   // :4490

    /** CmbSupplierDetail_Leave (:1266) — the supplier's last saved dispatch fills empty defaults. */
    async function supplierLeave() {
        var sid = comboVal('CmbSupplierDetail');
        if (!active('CmbSupplierDetail') || sid <= 0) return;
        var r = await api('GET', API + '/last-saved?supplierId=' + sid);
        if (!r || r.CityId === undefined) return;
        var isNew = st.recId === 0;
        if (intOf(r.CityId) > 0 && (isNew || comboVal('CmbCityNameMain') === 0)) setVal('CmbCityNameMain', r.CityId);
        if (r.DeliveryTerm && (isNew || comboText('CmbDeliveryTerm') === '')) setText('CmbDeliveryTerm', r.DeliveryTerm);
        if (r.CommType && (isNew || comboText('CmbCommType') === '')) { setText('CmbCommType', r.CommType); billAmount(); totalCommissionAmount(); }
        if (num(r.CommRate) > 0 && (isNew || num($('txtcommRate').value) === 0)) { $('txtcommRate').value = netStr(num(r.CommRate)); totalCommissionAmount(); billAmount(); }
        if (num(r.CommUom) > 0 && (isNew || num(comboText('CmbCommUOM')) === 0)) setText('CmbCommUOM', netStr(num(r.CommUom)));
        if (r.BrokeryType && (isNew || comboText('CmbBrokeryType') === '')) setText('CmbBrokeryType', r.BrokeryType);
        if (num(r.BrokeryRate) > 0 && (isNew || num($('txtBrokeryRate').value) === 0)) { $('txtBrokeryRate').value = netStr(num(r.BrokeryRate)); totalBrokeryAmount(); }
        if (num(r.BrokeryUom) > 0 && (isNew || num(comboText('CmbBrokeryRateUom')) === 0)) setText('CmbBrokeryRateUom', netStr(num(r.BrokeryUom)));
        renderGrid();
    }

    /** FormValidation (:1321) — messages verbatim, in order. */
    function formValidation() {
        var t;
        t = $('txtDocNoMain').value.trim();
        if (t === '' || t === '0') { alert('Doc No Field is Required'); return false; }
        if (!active('CmbSupplierDetail') || comboVal('CmbSupplierDetail') === 0) { alert('Supplier Name Field is Required'); focus('CmbSupplierDetail'); return false; }
        t = $('txtVehicleNoMain').value.trim();
        if (t === '' || t === '0') { alert('Vehicle No Field is Required'); focus('txtVehicleNoMain'); return false; }
        t = $('txtTransitDaysMain').value.trim();
        if (t === '' || intOf(t) === 0) { alert('TransitDays Field is Required'); focus('txtTransitDaysMain'); return false; }
        if (!active('CmbCityNameMain') || comboVal('CmbCityNameMain') === 0) { alert('CityName Field is Required'); focus('CmbCityNameMain'); return false; }
        var cur = active('CmbCurrencyMain') && comboVal('CmbCurrencyMain') !== 0, ex = $('txtExchangeRateMain').value.trim();
        if (L.multiCurrency) {
            if (!cur) { alert('Fcy Code Field is Required'); focus('CmbCurrencyMain'); return false; }
            if (ex === '' || ex === '0') { alert('Exchange Rate Field is Required'); focus('txtExchangeRateMain'); return false; }
            t = $('txtFcyAmountMain').value.trim();
            if (t === '' || t === '0') { alert('Fcy Amount Rate Field is Required'); return false; }
        } else {
            if (!cur) { alert('Please Configure Your Base Currency In configurations'); return false; }
            if (ex === '' || ex === '0') { alert('Please Configure Your Base Currency Rate In configurations'); return false; }
        }
        return true;
    }

    function payload() {
        return {
            Id: st.recId,
            DocDate: $('txtDocDateMain').value, DepartureDate: $('txtDepartureDateMain').value,
            ETAdestination: $('txtETAdestinationMain').value, TransitDays: $('txtTransitDaysMain').value,
            VehicleNo: $('txtVehicleNoMain').value, BiltyNo: $('txtBiltyNoMain').value, ManualBillNo: $('txtManualBillNo').value,
            RemarksHeader: $('txtRemarksMain').value, Freight: $('txtFreightMain').value, OtherExpense: $('txtOtherExpenseMain').value,
            AdvanceFreight: $('txtAdvanceFreight').value, SupplierWbWeight: $('txtSupplierWbWtMain').value.trim(),
            SupplierId: comboVal('CmbSupplierDetail'), ReferencePartyId: comboVal('CmbRefPartyDetail'), CityId: comboVal('CmbCityNameMain'),
            DeliveryTerm: comboText('CmbDeliveryTerm'), CurrencyId: comboVal('CmbCurrencyMain'), ExchangeRate: $('txtExchangeRateMain').value,
            CommissionAgentId: comboVal('CmbCommisionAgent'), CommTypeValue: comboRaw('CmbCommType'), CommTypeText: comboText('CmbCommType'),
            CommUomText: comboText('CmbCommUOM'), CommRate: $('txtcommRate').value,
            BrokeryAcId: comboVal('CmbBrokeryAc'), BrokeryTypeValue: comboRaw('CmbBrokeryType'), BrokeryTypeText: comboText('CmbBrokeryType'),
            BrokeryUomValue: comboRaw('CmbBrokeryRateUom'), BrokeryUomText: comboText('CmbBrokeryRateUom'), BrokeryRate: $('txtBrokeryRate').value,
            BrokeryAmountText: $('txtBrokeryAmount').value,
            rows: st.rows.map(function (r) {
                return {
                    Id: intOf(r.Id), OrderId: intOf(r.OrderId), OrderDetailId: intOf(r.OrderDetailId), ItemId: intOf(r.ItemId),
                    CropYearId: intOf(r.CropYearId), PackingTypeId: intOf(r.PackingTypeId), PackUomId: intOf(r.PackUomId),
                    Qty: num(r.Qty), GrossWeight: num(r.GrossWeight), EBUnit: num(r.EBUnit), EBTotal: num(r.EBTotal), AddLss: num(r.AddLss),
                    Weight: num(r.Weight), RateUomId: intOf(r.RateUomId), Rate: num(r.Rate), Amount: num(r.Amount),
                    Remarks: r.Remarks || '', ReceiverLocationId: intOf(r.ReceiverLocationId)
                };
            }),
            expenses: st.exp.map(function (e) {
                return { Id: intOf(e.Id), OrderId: intOf(e.OrderId), OrderExpId: intOf(e.OrderExpId), ItemId: intOf(e.ItemId),
                    Qty: num(e.Qty), Rate: num(e.Rate), Amount: num(e.Amount), Remarks: e.Remarks || '' };
            }),
            attachments: AT ? AT.payload() : undefined
        };
    }

    /** Insert (:1999) — client-side checks up to the prompt, then the server runs every check again. */
    function insert(button) {
        if (st.rows.length === 0) { alert('Grid Record Not Found'); return; }
        if (!formValidation()) return;
        if (!confirm(st.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        recalcAll();
        return act(button, async function () {
            var res = await api('POST', API + '/save', payload());
            alert(res.message);
            var savedId = res.id;
            await reset();
            if ($('ChkBox').checked) await print(savedId);                           // :2286
        });
    }
    function btnSave() { st.approved = false; st.recId = 0; insert($('btnsave')); }  // :2298
    function btnUpdate() {                                                            // :2312
        if (st.recId === 0) { alert('Record Not Update because RecId Not Found'); return; }
        insert($('btnupdate'));
    }
    function btnDelete() {                                                            // :1957
        if (st.approved) { alert('Record Not Delete because Record has approved'); return; }
        if (st.recId <= 0) { alert('Record Not Found'); return; }
        if (!confirm('Are you sure to Delete?')) return;
        return act($('btnDelete'), async function () {
            await api('POST', API + '/' + st.recId + '/delete');
            alert('Delete Record Successfully');
            await reset();
        });
    }
    /** CommonServices.SupplierDispatchPreBillSlip251(id) — the Jasper print of 251_SupplierDispatchPreBillSlip.rpt. */
    async function print(id) {
        if (!rights().print) return;
        var w = window.open('about:blank', '_blank');                                   // opened now so a pop-up blocker lets it through
        try {
            await api('GET', API + '/' + intOf(id) + '/print-check');
            var url = '/reports/print/by-template/' + encodeURIComponent(PRINT_RPT) + '/pdf?id=' + intOf(id);
            if (w) w.location.href = url; else window.open(url, '_blank');
        } catch (e) { if (w) w.close(); alert(msg(e)); }
    }

    /** Reset (:2492) */
    async function reset() {
        st.recId = 0; st.approved = false; st.updateIdx = -1;
        if (AT) AT.reset();
        show('btnsave', true); show('btnupdate', false); show('btnDelete', false); show('btnSaveAs', false);
        clearCombo('CmbSupplierDetail'); clearCombo('CmbRefPartyDetail');
        ['txtManualBillNo', 'txtRemarksMain', 'txtVehicleNoMain', 'txtBiltyNoMain', 'txtTransitDaysMain', 'txtFreightMain',
            'txtOtherExpenseMain', 'txtAdvanceFreight', 'txtSupplierWbWtMain', 'txtQtyMain', 'txtTotalWeightMain', 'txtFcyAmountMain',
            'txtcommRate', 'txtcommamount', 'txtBrokeryRate', 'txtBrokeryAmount', 'txtItemAmountFooter', 'txtExpenseAmountFooter',
            'txtAdvanceFreightFooter', 'txtNetBillAmountFooter', 'txtTotalAmountMain'].forEach(function (id) { $(id).value = ''; });
        ['CmbCommisionAgent', 'CmbCommType', 'CmbCommUOM', 'CmbBrokeryAc', 'CmbBrokeryRateUom', 'CmbBrokeryType'].forEach(clearCombo);
        clearItem(); clearCombo('CmbPackUomDetail');
        ['txtQtyDetail', 'txtGrossWeight', 'txtEBUnit', 'txtEBTotal', 'txtAddLss', 'txtNetBillWeightDetail', 'txtRateDetail', 'txtAmountDetail']
            .forEach(function (id) { $(id).value = ''; });
        clearCombo('CmbRateUomDetail');
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        st.rows = []; st.exp = [blankExp()];
        renderGrid(); renderExp();
        var n = await api('GET', API + '/numbers');
        $('txtDocNoMain').value = n.docNo; $('txtBranchSrNo').value = n.branchSrNo;
        ['branchImplemented', 'itemSearchByCode', 'defaultDaysToLessFromHistoryFromDate', 'defaultCropYear', 'defaultPackingType', 'cityArea',
            'baseCurrency', 'baseCurrencyRate'].forEach(function (k) { L[k] = n[k]; });
        applyColumnDefaults();
        enableDisableFields(true);
        $('txtDocDateMain').focus();
    }

    /** btnRefresh_Click (:2450) */
    function refresh() {
        return act($('btnRefresh'), async function () {
            var r = await api('GET', API + '/refresh');
            Object.keys(r).forEach(function (k) { L[k] = r[k]; });
            bindLists(false);
            await packUomBind(comboVal('CmbItemNameDetail'));
            baseCurrencyAndRate();
            applyColumnDefaults();
            renderGrid(); renderExp();
        });
    }

    /** ReadById (:2328) */
    async function readById(id) {
        var po = await api('GET', API + '/' + id);
        if (!po || !po.rows || !po.rows.length) return;
        st.recId = id;
        tab('tabPage1');
        $('txtDocDateMain').value = isoDay(po.DocDate); docDateChanged();
        $('txtDocNoMain').value = po.DocNo; $('txtBranchSrNo').value = po.BranchSrNo;
        $('txtManualBillNo').value = po.ManualBillNo || ''; $('txtVehicleNoMain').value = po.VehicleNo || ''; $('txtBiltyNoMain').value = po.BiltyNo || '';
        $('txtDepartureDateMain').value = isoDay(po.DepartureDate);
        $('txtTransitDaysMain').value = String(intOf(po.TransitDays));
        $('txtETAdestinationMain').value = isoDay(po.ETAdestination); etaChanged();
        $('txtFreightMain').value = fmtHash(po.Freight, 2); $('txtOtherExpenseMain').value = fmtHash(po.OtherExpense, 2);
        $('txtAdvanceFreight').value = fmtHash(po.AdvanceFreight, 2);
        setVal('CmbCityNameMain', po.CityId);
        $('txtSupplierWbWtMain').value = fmtHash(po.SupplierWbWeight, 2);
        setText('CmbDeliveryTerm', po.DeliveryTerm);
        $('txtQtyMain').value = fmtHash(po.TotalQty, 2); $('txtTotalWeightMain').value = fmtHash(po.TotalWeight, 2);
        $('txtTotalAmountMain').value = fmtAmt(po.TotalAmount);
        setVal('CmbCurrencyMain', po.CurrencyId);
        $('txtExchangeRateMain').value = fmtHash(po.ExchangeRate, 2);
        $('txtFcyAmountMain').value = fmtFcy(po.FcyAmount);
        $('txtRemarksMain').value = po.RemarksHeader || '';
        st.approved = !!po.IsApproved;
        if (intOf(po.CurrencyId) === 0) baseCurrencyAndRate();
        setVal('CmbSupplierDetail', po.SupplierId);
        setVal('CmbRefPartyDetail', po.ReferencePartyId);
        st.rows = po.rows.map(function (d) {
            return Object.assign({}, d, { AddLss: netStr(num(d.AddLss)), Expense: 0, Commission: 0, BillAmount: 0 });
        });
        if (orderExists()) enableDisableFields(false);
        (po.comm || []).forEach(function (c) {
            if (+c.TypeId === 1) {
                setVal('CmbCommisionAgent', c.CommissionAgentId);
                dropAdhoc('CmbCommType'); if (has('CmbCommType', c.CommType)) $('CmbCommType').value = String(c.CommType); else $('CmbCommType').selectedIndex = -1;
                $('txtcommRate').value = netStr(num(c.CommRate));
                setText('CmbCommUOM', netStr(num(c.RateUom)));
                $('txtcommamount').value = netStr(num(c.CommAmount));
            }
            if (+c.TypeId === 2) {
                setVal('CmbBrokeryAc', c.CommissionAgentId);
                dropAdhoc('CmbBrokeryType'); if (has('CmbBrokeryType', c.CommType)) $('CmbBrokeryType').value = String(c.CommType); else $('CmbBrokeryType').selectedIndex = -1;
                $('txtBrokeryRate').value = netStr(num(c.CommRate));
                setText('CmbBrokeryRateUom', netStr(num(c.RateUom)));                   // D3: shows the stored Id 1..4
                $('txtBrokeryAmount').value = netStr(num(c.CommAmount));
            }
        });
        if (comboText('CmbBrokeryType') === '') $('CmbBrokeryType').selectedIndex = 1;
        if (comboText('CmbCommType') === '') $('CmbCommType').selectedIndex = 1;
        st.exp = (po.expenses || []).map(function (e) { return Object.assign({}, e); });
        if (!st.exp.length) st.exp.push(blankExp());
        renderExp();
        recalcAll();
        show('btnsave', false); show('btnupdate', true); show('btnDelete', true);
    }

    // =========================================================================== loader

    function openOrderLoader() {                                                     // btnPurchaseOrderLoader_Click (:4544)
        if (st.rows.length > 0 && !orderExists()) { alert("You cannot load an order because no valid 'OrderId' exists in the grid."); return; }
        $('ldrGrd').tHead.innerHTML = ''; $('ldrGrd').tBodies[0].innerHTML = '';
        setVal('ldrSupplier', comboVal('CmbSupplierDetail') || null);
        show('dlgOrder', true);
    }
    var LDR_COLS = [['DocNo', 'DocNo'], ['DocDate', 'DocDate'], ['SupplierName', 'SupplierName'], ['ItemName', 'ItemName'],
        ['ItemCode', 'ItemCode'], ['ItemQty', 'ItemQty'], ['ItemWeight', 'ItemWeight'], ['PackUom', 'PackUom'], ['UOMCode', 'Rate Uom'],
        ['OrderItemRate', 'OrderItemRate'], ['DeliveryTerm', 'DeliveryTerm'], ['JobLotDescription', 'JobLot']];
    var ldrRows = [];
    function loaderShow() {
        return act($('ldrShow'), async function () {
            var q = '?supplierId=' + comboVal('ldrSupplier') + '&fromDate=' + encodeURIComponent($('ldrFromDate').value)
                + '&toDate=' + encodeURIComponent($('ldrToDate').value) + '&fromDocNo=' + encodeURIComponent($('ldrFromDocNo').value)
                + '&toDocNo=' + encodeURIComponent($('ldrToDocNo').value);
            ldrRows = await api('GET', API + '/order-loader' + q) || [];
            var t = $('ldrGrd');
            t.tHead.innerHTML = '<tr><th><input type="checkbox" id="ldrAll"></th>' + LDR_COLS.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = ldrRows.map(function (r, i) {
                return '<tr data-i="' + i + '"><td><input type="checkbox" data-pick></td>' + LDR_COLS.map(function (c) {
                    var v = r[c[0]];
                    if (c[0] === 'DocDate') v = dMMMyyyy(v);
                    else if (c[0] === 'ItemQty' || c[0] === 'ItemWeight') v = fmtHash(v, 2);
                    else if (c[0] === 'OrderItemRate') v = fmtRate(v);
                    return '<td' + (typeof r[c[0]] === 'number' ? ' class="n"' : '') + '>' + esc(v) + '</td>';
                }).join('') + '</tr>';
            }).join('');
            $('ldrAll').addEventListener('change', function () {
                var on = this.checked;
                t.querySelectorAll('input[data-pick]').forEach(function (c) { c.checked = on; });
            });
        });
    }
    function loaderLoad() {
        var picked = [];
        $('ldrGrd').querySelectorAll('tbody tr').forEach(function (tr) {
            if (tr.querySelector('input[data-pick]').checked) picked.push(ldrRows[+tr.getAttribute('data-i')]);
        });
        show('dlgOrder', false);
        return act($('btnPurchaseOrderLoader'), async function () { await loadInGridDetail(picked); });
    }
    /** LoadInGridDetail (:4568) */
    async function loadInGridDetail(dt) {
        if (!dt.length) return;
        var poIds = dt.map(function (r) { return r.Id; }).join(',');
        var first = dt[0];
        var prevSupplier = comboVal('CmbSupplierDetail'), rowSupplier = intOf(first.SupplierCustomerId);
        var prevTerm = comboText('CmbDeliveryTerm'), rowTerm = first.DeliveryTerm == null ? '' : String(first.DeliveryTerm);
        if (active('CmbSupplierDetail') && prevSupplier > 0 && rowSupplier !== prevSupplier) throw new Error("You can't load an order from a different party at the same time.");
        if (active('CmbDeliveryTerm') && prevTerm.trim() !== '' && rowTerm !== prevTerm) throw new Error("You can't load an order with a different delivery term at the same time.");
        setVal('CmbSupplierDetail', rowSupplier);
        setText('CmbDeliveryTerm', rowTerm);
        setVal('CmbBrokeryAc', first.BrokerAgentId);
        setText('CmbBrokeryType', first.BrokeryType);
        $('txtBrokeryRate').value = first.BrokeryRate == null ? '' : netStr(num(first.BrokeryRate));
        setText('CmbBrokeryRateUom', first.BrokeryUom == null ? '' : netStr(num(first.BrokeryUom)));
        $('txtBrokeryAmount').value = first.BrokeryAmount == null ? '' : netStr(num(first.BrokeryAmount));
        setVal('CmbCommisionAgent', first.BrokerAgentSupCustId);
        setText('CmbCommType', first.CommissionType);
        $('txtcommRate').value = first.CommRate == null ? '' : netStr(num(first.CommRate));
        setText('CmbCommUOM', first.UomScheduleIdCmRate == null ? '' : String(first.UomScheduleIdCmRate));
        $('txtcommamount').value = first.CommAmount == null ? '' : netStr(num(first.CommAmount));
        $('txtRemarksMain').value = first.RemarksHeader || '';
        enableDisableFields(false);
        var existingDetailIds = {}, orderIds = {}, anyOrder = false;
        st.rows.forEach(function (r) { existingDetailIds[intOf(r.OrderDetailId)] = 1; orderIds[intOf(r.OrderId)] = 1; anyOrder = true; });
        var ex = num($('txtExchangeRateMain').value);
        for (var i = 0; i < dt.length; i++) {
            var row = dt[i];
            if (anyOrder && !orderIds[intOf(row.Id)]) { renderGrid(); alert('Data against another OrderNo Already Exist in Detail'); return; }
            if (existingDetailIds[intOf(row.PODetailId)]) continue;
            var net = num(row.GrossWeight) - num(row.EmptyBagsTotal), eq = num(row.Equivalent);
            var amount = eq === 0 ? 0 : net / eq * num(row.OrderItemRate);
            st.rows.push({
                Id: 0, OrderId: intOf(row.Id), OrderNo: row.DocNo, OrderDetailId: intOf(row.PODetailId), ItemId: intOf(row.OrderItemId),
                ItemCode: row.ItemCode, ItemName: row.ItemName, CropYearId: intOf(row.CropYearId), PackingTypeId: 0,
                PackUomId: intOf(row.PackUomId), PackUom: row.PackUom, PackUomEquivalent: num(row.PackEquivalent),
                Qty: num(row.ItemQty), GrossWeight: num(row.GrossWeight), EBUnit: num(row.EmptyBags), EBTotal: num(row.EmptyBagsTotal),
                AddLss: '0', Weight: net, RateUomId: intOf(row.RateUomId), RateUom: row.UOMCode, RateUomEquivalent: eq,
                Rate: num(row.OrderItemRate), Amount: amount, FcyAmount: ex > 0 ? amount / ex : 0, Remarks: '', ReceiverLocationId: 0,
                Expense: 0, Commission: 0, BillAmount: 0
            });
        }
        renderGrid();
        /* LoadExpData (:4650) */
        var exps = await api('GET', API + '/order-expenses?orderIds=' + encodeURIComponent(poIds)) || [];
        if (exps.length) {
            var expIds = {}, expOrders = {}, anyExpOrder = false;
            st.exp.forEach(function (e) { expIds[intOf(e.OrderExpId)] = 1; if (intOf(e.OrderId) > 0) { expOrders[intOf(e.OrderId)] = 1; anyExpOrder = true; } });
            for (var k = 0; k < exps.length; k++) {
                var x = exps[k], oid = intOf(x.PurchaseOrderId), xid = intOf(x.Id);
                if (expIds[xid]) continue;
                if (anyExpOrder && !expOrders[oid]) { renderExp(); throw new Error('Data against another OrderNo already exists in Expense Grid.'); }
                st.exp.push({ Id: 0, OrderId: oid, OrderExpId: xid, ItemId: intOf(x.InvRevExpItemId), Qty: num(x.Qty), Rate: num(x.Rate),
                    Amount: num(x.Amount), Remarks: x.Remarks == null ? '' : String(x.Remarks) });
                expIds[xid] = 1; expOrders[oid] = 1; anyExpOrder = true;
            }
        }
        renderExp();
        recalcAll();
    }

    // =========================================================================== history

    function historyBranchFill() {                                                   // HistoryBranchComboFill (:787)
        /* checked countx_prod_combo (<select multiple data-dtcombo-checked>); Text = UserAccount.BranchName checks the user's branch */
        var sel = $('cmbBranchName'), rows = L.historyBranches || [];
        sel.innerHTML = rows.map(function (b) {
            return '<option value="' + esc(b.Id) + '"' + (+b.Id === +L.userBranchId ? ' selected' : '') + '>' + esc(b.Name) + '</option>';
        }).join('');
    }
    function branchPicked() { return Array.prototype.filter.call($('cmbBranchName').options, function (o) { return o.selected; }); }
    function branchText() { return branchPicked().map(function (o) { return o.textContent; }).join(','); }
    function branchIds() { return branchPicked().map(function (o) { return o.value; }).join(','); }
    function historyDefaults() {
        var d = intOf(L.defaultDaysToLessFromHistoryFromDate);
        $('FromDateHistory').value = addDaysIso(today(), -(d > 0 ? d : 3));
        $('ToDateHistory').value = today();
    }

    var H_COLS = [
        ['DocNo', 'DocNo', 'i'], ['BranchSrNo', 'BranchSrNo', 'i', 'branch'], ['DocDate', 'DocDate', 'd'], ['Supplier', 'Supplier'],
        ['ReferenceParty', 'ReferenceParty'], ['ManualBillNo', 'ManualBillNo'], ['VehicleNo', 'VehicleNo'], ['BiltyNo', 'BiltyNo'],
        ['DepartureDate', 'DepartureDate', 'd'], ['TransitDays', 'TransitDays', 'i'], ['ETADestination', 'ETADestination', 'd'],
        ['SupplierWbWeight', 'SupplierWbWeight', 'q'], ['TotalQty', 'TotalQty', 'q'], ['TotalWeight', 'TotalWeight', 'q'],
        ['TotalAmount', 'TotalAmount', 'a'], ['FcyAmount', 'FcyAmount', 'a', 'fcy'], ['Freight', 'Freight', 'q'],
        ['OtherExpense', 'OtherExpense', 'q'], ['CityName', 'CityName'], ['RemarksHeader', 'RemarksHeader'], ['DeliveryTerm', 'DeliveryTerm'],
        ['Status', 'Status', 's'], ['EntryDate', 'EntryDate', 't'], ['EntryUser', 'EntryUser'], ['ModifyDate', 'ModifyDate', 't'],
        ['ModifyUser', 'ModifyUser'], ['IsApproved', 'IsApproved'], ['ApprovedDate', 'ApprovedDate', 't'], ['ApprovedUser', 'ApprovedUser'],
        ['NoOfAttachments', 'NoOfAttachments', 'l']
    ];
    var hFilter = {};
    function histCols() {
        return H_COLS.filter(function (c) {
            if (c[3] === 'branch') return L.branchFeature && !L.branchImplemented;
            if (c[3] === 'fcy') return !!L.multiCurrency;
            return true;
        });
    }
    function histCell(c, v) {
        switch (c[2]) {
            case 'd': return dMMMyyyy(v);
            case 't': return dMMMyyyyTime(v);
            case 'q': return fmtHash(v, 2);
            case 'a': return fmtAmt(v);
            default: return v === null || v === undefined ? '' : v;
        }
    }
    function histVisibleRows() {
        var cols = histCols();
        return st.hist.map(function (r, i) { return i; }).filter(function (i) {
            var r = st.hist[i];
            return cols.every(function (c) {
                var f = hFilter[c[0]];
                return !f || String(histCell(c, r[c[0]])).toLowerCase().indexOf(f.toLowerCase()) >= 0;
            });
        });
    }
    /** HistoryGridSettings (:3496) + GridHistoryStatusUpdate (:3468) */
    function renderHistory() {
        var cols = histCols(), t = $('grdhistory'), sm = st.statusMode;
        t.tHead.innerHTML = '<tr>' + (sm ? '<th><input type="checkbox" id="histAll"></th>' : '') + '<th>Edit</th><th>Print</th>'
            + cols.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>'
            + '<tr class="flt">' + (sm ? '<td></td>' : '') + '<td></td><td></td>' + cols.map(function (c) {
                return '<td><input type="text" data-f="' + c[0] + '" value="' + esc(hFilter[c[0]] || '') + '"></td>';
            }).join('') + '</tr>';
        var idx = histVisibleRows();
        t.tBodies[0].innerHTML = idx.map(function (i) {
            var r = st.hist[i];
            return '<tr data-i="' + i + '"' + (i === st.histSel ? ' class="is-sel"' : '') + '>'
                + (sm ? '<td><input type="checkbox" data-pick></td>' : '')
                + '<td><button type="button" class="gbtn" data-act="Edit">Edit</button></td>'
                + '<td><button type="button" class="gbtn" data-act="Print">Print</button></td>'
                + cols.map(function (c) {
                    if (c[2] === 's' && sm) return '<td>' + selectHtml(L.statuses, statusId(r.Status), 'data-status') + '</td>';
                    if (c[2] === 'l') return '<td class="n"><a class="lnk" tabindex="0" data-attach="' + intOf(r.Id) + '" title="Attachments">' + esc(r[c[0]]) + '</a></td>';
                    var numeric = c[2] === 'q' || c[2] === 'a' || c[2] === 'i';
                    return '<td' + (numeric ? ' class="n"' : '') + '>' + esc(histCell(c, r[c[0]])) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        t.tFoot.innerHTML = idx.length ? '<tr>' + (sm ? '<td></td>' : '') + '<td></td><td></td>' + cols.map(function (c) {
            if (c[2] === 'q' || c[2] === 'a') {
                var s = 0; idx.forEach(function (i) { s += num(st.hist[i][c[0]]); });
                return '<td class="n">' + esc(histCell(c, s)) + '</td>';
            }
            return '<td></td>';
        }).join('') + '</tr>' : '';
        enhanceCombos('grdhistory');
        var all = $('histAll');
        if (all) all.addEventListener('change', function () { var on = this.checked; t.querySelectorAll('input[data-pick]').forEach(function (c) { c.checked = on; }); });
    }
    function statusId(text) { var id = 0; (L.statuses || []).forEach(function (s) { if (s.Name === text) id = s.Id; }); return id; }

    /** gridhistoryfill (:3330) */
    function showHistory() {
        return act($('btnShow'), async function () {
            var ids = branchIds();
            if (branchText() === '' || !ids) { focus('cmbBranchName'); throw new Error('Select branch first'); }
            var dateType = document.querySelector('input[name="rdHist"]:checked').value;
            var q = '?branchIds=' + encodeURIComponent(ids) + '&dateType=' + dateType
                + '&fromChecked=' + $('chkFromDateHistory').checked + '&fromDate=' + encodeURIComponent($('FromDateHistory').value)
                + '&toChecked=' + $('chkToDateHistory').checked + '&toDate=' + encodeURIComponent($('ToDateHistory').value)
                + '&fromDocNo=' + encodeURIComponent($('FromDocNoHistory').value) + '&toDocNo=' + encodeURIComponent($('ToDocNoHistory').value);
            st.hist = await api('GET', API + '/history' + q) || [];
            st.histSel = -1;
            renderHistory();
            renderHistoryDetail(null);
        });
    }
    /** GridDetailBind (:3690) + DataGridHistoryDetailSetting (:3744) */
    function renderHistoryDetail(po) {
        var t = $('DataGridHistoryDetail');
        if (!po) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; return; }
        var rows = po.rows || [], oe = rows.some(function (r) { return intOf(r.OrderId) > 0; });
        var cols = [];
        if (oe) cols.push(['OrderNo', 'OrderNo', 'i']);
        cols.push(['ItemCode', 'ItemCode'], ['ItemName', 'ItemName'], ['CropYear', 'CropYear'], ['PackingType', 'PackingType'], ['PackUom', 'PackUom'],
            ['Qty', 'Qty', 'q'], ['GrossWeight', 'GrossWeight', 'q'], ['EBUnit', 'EBUnit', 'q'], ['EBTotal', 'EBTotal', 'q'], ['AddLss', 'AddLss', 's'],
            ['Weight', 'Net Bill Weight', 'q'], ['RateUom', 'RateUom'], ['Rate', 'Rate', 'r'], ['Amount', 'Amount', 'a']);
        if (L.multiCurrency) cols.push(['FcyAmount', 'FcyAmount', 'a']);
        cols.push(['Remarks', 'Remarks']);
        if (L.branchFeature && !L.branchImplemented) cols.push(['ReceiverLocation', 'ReceiverLocation']);
        function cell(c, v) { return c[2] === 'q' ? fmtHash(v, 2) : c[2] === 'a' ? fmtAmt(v) : c[2] === 'r' ? fmtRate(v) : c[2] === 's' ? netStr(num(v)) : (v == null ? '' : v); }
        t.tHead.innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = rows.map(function (r) {
            return '<tr>' + cols.map(function (c) {
                var n = c[2] === 'q' || c[2] === 'a' || c[2] === 'r' || c[2] === 'i';
                return '<td' + (n ? ' class="n"' : '') + '>' + esc(cell(c, r[c[0]])) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        t.tFoot.innerHTML = rows.length ? '<tr>' + cols.map(function (c) {
            return (c[2] === 'q' || c[2] === 'a') ? '<td class="n">' + esc(cell(c, sum(rows, c[0]))) + '</td>' : '<td></td>';
        }).join('') + '</tr>' : '';
    }
    async function selectHistoryRow(i) {
        st.histSel = i;
        $('grdhistory').querySelectorAll('tbody tr').forEach(function (tr) { tr.classList.toggle('is-sel', +tr.getAttribute('data-i') === i); });
        try { renderHistoryDetail(await api('GET', API + '/' + st.hist[i].Id)); } catch (e) { alert(msg(e)); }
    }
    /** Edit (:3612) and Ctrl+Enter: Reset + ReadById. Double-click does the same here (see the report). */
    function editHistory(i) {
        var id = st.hist[i] && st.hist[i].Id;
        if (!id) return;
        return act(null, async function () { await reset(); await readById(id); });
    }
    function onHistoryClick(e) {
        var b = e.target.closest('button[data-act]'), tr = e.target.closest('tbody tr[data-i]');
        if (!tr) return;
        var i = +tr.getAttribute('data-i');
        if (b) {
            if (b.getAttribute('data-act') === 'Edit') editHistory(i);
            else print(st.hist[i].Id);
            return;
        }
        var link = e.target.closest('a[data-attach]');
        if (link) { if (AT) AT.view(intOf(link.getAttribute('data-attach'))); return; }   // NoOfAttachments link (GetNoofAttachmentsByScreenName)
        if (e.target.closest('input,select,a')) return;
        if (i !== st.histSel) selectHistoryRow(i);
    }
    /** btnUpdateStatusHistory_Click (:3789) */
    function updateStatus() {
        if (!rights().update) { alert("You Don't have right for Update"); return; }
        var picked = [];
        $('grdhistory').querySelectorAll('tbody tr').forEach(function (tr) {
            var c = tr.querySelector('input[data-pick]');
            if (c && c.checked) {
                var s = tr.querySelector('select[data-status]');
                picked.push({ Id: st.hist[+tr.getAttribute('data-i')].Id, Status: s && s.selectedIndex >= 0 ? s.options[s.selectedIndex].textContent : '' });
            }
        });
        if (!picked.length) { alert('Please select check box first'); return; }
        if (!confirm('Are you sure to Update Selected Records?')) return;
        return act($('btnUpdateStatusHistory'), async function () {
            await api('POST', API + '/status', picked);
            alert('Record Approved Successfully');
            await showHistory();
        });
    }
    /** btnResetHistory_Click (:3287) — From Date back to Now-3. */
    function resetHistory() {
        $('FromDateHistory').value = addDaysIso(today(), -3);
        $('ToDateHistory').value = today();
        $('FromDocNoHistory').value = ''; $('ToDocNoHistory').value = '';
        st.hist = []; st.histSel = -1; hFilter = {};
        $('grdhistory').tHead.innerHTML = ''; $('grdhistory').tBodies[0].innerHTML = ''; $('grdhistory').tFoot.innerHTML = '';
        renderHistoryDetail(null);
        $('drdocdate').checked = true;
    }
    function refreshHistory() {
        return act($('btnRefreshHistory'), async function () {
            var r = await api('GET', API + '/history-refresh');
            L.historyBranches = r.historyBranches; L.userBranchId = r.userBranchId; L.userBranchName = r.userBranchName;
            historyBranchFill();
        });
    }
    /** Ctrl+H (:4220) */
    function enableStatusUpdate() {
        if ($('grpUpdateHistoryStatus').hidden) return;
        $('grpUpdateHistoryStatus').classList.remove('sit-disabled');
        $('btnUpdateStatusHistory').disabled = false;
        st.statusMode = true;
        if (st.hist.length) renderHistory();
    }

    // ===================================================================== tabs / keys

    function tab(id) {
        ['tabPage1', 'tabPage2'].forEach(function (p) { $(p).classList.toggle('is-active', p === id); });
        $('tabBtnForm').classList.toggle('is-active', id === 'tabPage1');
        $('tabBtnHistory').classList.toggle('is-active', id === 'tabPage2');
        if (id === 'tabPage2') $('FromDateHistory').focus(); else $('txtDocDateMain').focus();   // tabControl1_SelectedIndexChanged
    }
    function currentTab() { return $('tabPage2').classList.contains('is-active') ? 1 : 0; }
    function visibleEnabled(id) { var b = $(id); return !b.hidden && !b.disabled; }

    var KEYS = [
        ['Ctrl+S', 'For Save in Form Tab and For Show History in History Tab'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'],
        ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print 273'], ['Alt+1', 'For Print 273'],
        ['Ctrl+F1', 'For Open Reference Party Form'], ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'],
        ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid when focus in form tab and for focus on history grid when in history tab'],
        ['Ctrl+ArrowUp', 'For Focus on Item in Detail Grid when in Form tab and For focus on FromDate in history tab'],
        ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]
    ];
    function shortcutKeys() {                                                        // MakeShortCutKeys (:4247)
        $('grdKeys').tBodies[0].innerHTML = KEYS.map(function (k) { return '<tr><td>' + esc(k[0]) + '</td><td>' + esc(k[1]) + '</td></tr>'; }).join('');
        show('dlgKeys', true);
    }
    /** SupplierDispatch_KeyDown (:4104). Keys the browser reserves (Ctrl+N / Ctrl+T) may not reach the page. */
    function onKey(e) {
        if (!L) return;
        var k = e.key, c = e.ctrlKey, handled = true;
        if (c && e.altKey && (k === 'Control' || k === 'Alt')) { shortcutKeys(); e.preventDefault(); return; }
        if (c && (k === 't' || k === 'T')) { tab(currentTab() === 1 ? 'tabPage1' : 'tabPage2'); e.preventDefault(); return; }
        if (currentTab() === 0) {
            if (c && (k === 'n' || k === 'N')) act($('btnnew'), reset);
            else if (c && (k === 's' || k === 'S')) { if (visibleEnabled('btnsave')) btnSave(); }
            else if (c && (k === 'r' || k === 'R')) refresh();
            else if (c && (k === 'u' || k === 'U')) { if (visibleEnabled('btnupdate')) btnUpdate(); }
            else if (c && k === 'F5') $('txtDocDateMain').focus();
            else if (c && k === 'F1') window.open('/party-processing/reference-parties', '_blank');     // BtnRefPartyForm_Click
            else if (c && k === 'F10') { if (AT) AT.open($('btnattachment')); }                        // btnattachment_Click
            else if (c && k === 'F12') { if (visibleEnabled('btnSaveAs')) btnSave(); }                  // btnSaveAs_Click
            else if (c && k === 'ArrowDown') { var g = $('grd').querySelector('input,select,button'); if (g) g.focus(); }
            else if (c && k === 'ArrowUp') focus('CmbItemNameDetail');
            else if (c && (k === 'p' || k === 'P')) { if (rights().print) print(st.recId); }
            else if (e.altKey && k === '1') { if (rights().print) print(st.recId); }
            else if (c && e.shiftKey && k === 'Delete') { if (visibleEnabled('btnDelete')) btnDelete(); }
            else handled = false;
        } else {
            if (c && (k === 's' || k === 'S')) showHistory();
            else if (c && (k === 'u' || k === 'U')) { if (rights().update && !$('btnUpdateStatusHistory').disabled) updateStatus(); }
            else if (c && (k === 'n' || k === 'N')) resetHistory();
            else if (c && (k === 'r' || k === 'R')) refreshHistory();
            else if (c && k === 'F5') $('FromDateHistory').focus();
            else if (c && k === 'ArrowDown') { var h = $('grdhistory').querySelector('tbody button'); if (h) h.focus(); }
            else if (c && k === 'ArrowUp') $('FromDateHistory').focus();
            else if (c && (k === 'h' || k === 'H')) enableStatusUpdate();
            else if (c && k === 'Enter') { var tr = e.target.closest && e.target.closest('#grdhistory tbody tr[data-i]'); if (tr) editHistory(+tr.getAttribute('data-i')); }
            else handled = false;
        }
        if (handled) e.preventDefault();
    }

    // ============================================================================= wiring

    function numericGuards() {
        document.addEventListener('input', function (e) {
            var t = e.target;
            if (!t || t.tagName !== 'INPUT') return;
            if (t.hasAttribute('data-integer')) { var v = t.value.replace(/[^0-9]/g, ''); if (v !== t.value) t.value = v; }
            else if (t.hasAttribute('data-decimal')) {
                var w = t.value.replace(/[^0-9.\-]/g, '');                               // OnlytextdecimelFunction
                if (w !== t.value) t.value = w;
            }
        }, true);
    }

    function wire() {
        $('btnnew').addEventListener('click', function () { act(this, reset); });
        $('btnRefresh').addEventListener('click', refresh);
        $('btnsave').addEventListener('click', btnSave);
        $('btnSaveAs').addEventListener('click', btnSave);                           // btnSaveAs_Click == btnsave_Click (:1985)
        $('btnupdate').addEventListener('click', btnUpdate);
        $('btnDelete').addEventListener('click', btnDelete);
        $('btnprint').addEventListener('click', function () { var b = this; act(b, function () { return print(st.recId); }); });
        $('btnattachment').addEventListener('click', function () { if (AT) AT.open(this); });             // btnattachment_Click :3924
        $('BtnRefPartyForm').addEventListener('click', function () { window.open('/party-processing/reference-parties', '_blank'); });  // BtnRefPartyForm_Click :4092 DefineReferenceParties.Show()
        $('btnPurchaseOrderLoader').addEventListener('click', openOrderLoader);
        $('BtnShortCutkeys').addEventListener('click', shortcutKeys);
        $('dlgKeysClose').addEventListener('click', function () { show('dlgKeys', false); });
        $('dlgOrderClose').addEventListener('click', function () { show('dlgOrder', false); });
        $('ldrShow').addEventListener('click', loaderShow);
        $('ldrLoad').addEventListener('click', loaderLoad);
        $('tabBtnForm').addEventListener('click', function () { tab('tabPage1'); });
        $('tabBtnHistory').addEventListener('click', function () { tab('tabPage2'); });

        $('txtDocDateMain').addEventListener('change', docDateChanged);
        $('txtTransitDaysMain').addEventListener('blur', docDateChanged);            // Leave -> txtTransitDaysMain_TextChanged (:7754)
        $('txtETAdestinationMain').addEventListener('change', etaChanged);
        $('txtExchangeRateMain').addEventListener('input', exchangeRateChanged);
        $('txtExchangeRateMain').addEventListener('blur', function () {              // txtExchangeRate_Leave (:2708)
            this.value = fmtRate(num(this.value)); exchangeRateChanged();
        });
        $('txtAdvanceFreight').addEventListener('input', exchangeRateChanged);
        $('CmbSupplierDetail').addEventListener('change', function () { act(null, supplierLeave); });
        $('CmbCommisionAgent').addEventListener('change', function () {           // TextChanged (:3120) + Leave (:3244)
            if (comboVal('CmbSupplierDetail') === comboVal('CmbCommisionAgent')) { totalCommissionAmount(); billAmount(); }
            totalCommissionAmount(); renderGrid();
        });
        $('CmbCommType').addEventListener('change', function () { billAmount(); totalCommissionAmount(); renderGrid(); });
        $('txtcommRate').addEventListener('input', function () { totalCommissionAmount(); billAmount(); renderGrid(); });
        $('CmbCommUOM').addEventListener('change', function () { totalCommissionAmount(); billAmount(); renderGrid(); });
        ['CmbBrokeryAc', 'CmbBrokeryType', 'CmbBrokeryRateUom'].forEach(function (id) {
            $(id).addEventListener('change', function () { totalBrokeryAmount(); renderGrid(); });
        });
        $('txtBrokeryRate').addEventListener('input', function () { totalBrokeryAmount(); renderGrid(); });

        $('rdSearchByName').addEventListener('change', function () { bindItems(false); focus('CmbItemNameDetail'); });
        $('rdSearchByCode').addEventListener('change', function () { bindItems(false); focus('CmbItemNameDetail'); });
        $('CmbItemNameDetail').addEventListener('change', function () { act(null, function () { return packUomBind(comboVal('CmbItemNameDetail')); }); });
        $('CmbPackUomDetail').addEventListener('change', detailCalcAll);
        $('txtQtyDetail').addEventListener('input', detailCalcAll);
        ['txtGrossWeight', 'txtEBUnit', 'txtAddLss'].forEach(function (id) { $(id).addEventListener('input', function () { total(); totalAmount(); }); });
        $('txtRateDetail').addEventListener('input', totalAmount);
        $('CmbRateUomDetail').addEventListener('change', totalAmount);
        $('btnplus').addEventListener('click', btnPlus);
        $('btnUpdateDetail').addEventListener('click', btnUpdateDetail);
        $('btnCancelUpdateDetial').addEventListener('click', resetDetail);

        $('grd').addEventListener('change', onGridChange);
        $('grd').addEventListener('click', onGridClick);
        $('grd').addEventListener('focusin', onGridFocusUom);
        $('grd').addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tbody tr[data-i]');
            if (tr && !e.target.closest('input,select,button')) gridDoubleClick(+tr.getAttribute('data-i'));
        });
        $('grdInvExp').addEventListener('change', onExpChange);
        $('grdInvExp').addEventListener('click', onExpClick);

        $('btnShow').addEventListener('click', showHistory);
        $('btnResetHistory').addEventListener('click', resetHistory);
        $('btnRefreshHistory').addEventListener('click', refreshHistory);
        $('btnUpdateStatusHistory').addEventListener('click', updateStatus);
        $('grdhistory').addEventListener('click', onHistoryClick);
        $('grdhistory').addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tbody tr[data-i]');
            if (tr && !e.target.closest('input,select,button')) editHistory(+tr.getAttribute('data-i'));
        });
        $('grdhistory').addEventListener('input', function (e) {
            var f = e.target.getAttribute('data-f');
            if (!f) return;
            hFilter[f] = e.target.value;
            var pos = e.target.selectionStart;
            renderHistory();
            var again = $('grdhistory').querySelector('input[data-f="' + f + '"]');
            if (again) { again.focus(); try { again.setSelectionRange(pos, pos); } catch (x) { } }
        });
        document.addEventListener('keydown', onKey);
        /* grd_KeyDown :4280 / grdInvExp_KeyDown :4364 - Ctrl+Space on a focused X / + button runs it, Ctrl+Delete
           deletes the current row, Ctrl+D adds an expense row, Ctrl+Enter edits a detail row. */
        document.addEventListener('keydown', function (e) {
            if (!e.ctrlKey || !e.target.closest) return;
            var tr = e.target.closest('#grd tbody tr[data-i], #grdInvExp tbody tr[data-i]');
            if (!tr) return;
            var i = +tr.getAttribute('data-i'), exp = !!tr.closest('#grdInvExp'), btn = e.target.closest('button[data-act]');
            if (e.code === 'Space' && btn) { e.preventDefault(); btn.click(); return; }
            if (e.key === 'Delete') {
                e.preventDefault();
                if (exp) { st.exp.splice(i, 1); if (!st.exp.length) st.exp.push(blankExp()); expProportion(); exchangeRateChanged(); renderExp(); }
                else deleteDetailRow(i);
            } else if ((e.key === 'd' || e.key === 'D') && exp) {
                e.preventDefault(); st.exp.push(blankExp()); expProportion(); exchangeRateChanged(); renderExp();
            } else if (e.key === 'Enter' && !exp) { e.preventDefault(); gridDoubleClick(i); }
        });
        /* grdhistory_KeyDown - Ctrl+Space (or Enter) on a focused link cell runs the link. */
        document.addEventListener('keydown', function (e) {
            var a = e.target.closest && e.target.closest('a.lnk[tabindex]');
            if (a && ((e.ctrlKey && e.code === 'Space') || (e.key === 'Enter' && !e.ctrlKey))) { e.preventDefault(); a.click(); return; }
            var hb = e.ctrlKey && e.code === 'Space' && e.target.closest && e.target.closest('#grdhistory button[data-act]');
            if (hb) { e.preventDefault(); hb.click(); }
        });
        numericGuards();
        if (window.PurchaseChrome) {
            PurchaseChrome.footer({ isHistory: function () { return currentTab() === 1; }, toggle: function () { tab(currentTab() === 1 ? 'tabPage1' : 'tabPage2'); }, watch: $('tabPage2') });
            ['grdWrap'].forEach(function (id) { PurchaseChrome.fullscreen($(id)); });
            ['grdhistory', 'DataGridHistoryDetail', 'grdInvExp'].forEach(function (id) { var t = $(id); if (t && t.parentElement) PurchaseChrome.fullscreen(t.parentElement); });
        }
    }

    // =============================================================================== load

    /** frmSupplierDispatchPreBill constructor + InitializeComponentMethod (:631). */
    async function init() {
        wire();
        try {
            L = await api('GET', API + '/lookups');
        } catch (e) {
            $('rightsNote').textContent = msg(e);
            $('rightsNote').hidden = false;
            document.querySelectorAll('.wf-root button, .wf-root input, .wf-root select').forEach(function (el) { el.disabled = true; });
            return;
        }
        var r = rights();
        $('btnsave').disabled = !r.save; $('btnSaveAs').disabled = !r.save; $('btnprint').disabled = !r.print;
        $('btnupdate').disabled = !r.update; $('btnDelete').disabled = !r.delete;
        $('ChkBox').checked = !!r.print;
        show('grpUpdateHistoryStatus', !!r.update);                                  // Visible = update right, Enabled = false
        if (L.itemSearchByCode) $('rdSearchByCode').checked = true; else $('rdSearchByName').checked = true;
        $('txtDocNoMain').value = L.docNo; $('txtBranchSrNo').value = L.branchSrNo;
        $('txtDocDateMain').value = today();
        bindFixedCombos();
        bindLists(true);
        handleFeatures();
        applyColumnDefaults();
        st.exp = [blankExp()];
        renderGrid(); renderExp();
        await packUomBind(comboVal('CmbItemNameDetail')).catch(function (e) { alert(msg(e)); });   // combitem_Leave (:1114)
        /* Designer defaults: Departure and ETA pickers start at Now; Transit Days is empty (no ValueChanged at load). */
        $('txtDepartureDateMain').value = today();
        $('txtETAdestinationMain').value = today();
        historyBranchFill();
        historyDefaults();
        $('ldrFromDate').value = ''; $('ldrToDate').value = '';
        st.formInitialized = true;
        $('txtDocDateMain').focus();
        /* ?id= deep link (Stock In Transit Report DocNo, Inward Gate Pass transit vehicle): open the record after the lookups. */
        var qid = parseInt(new URLSearchParams(location.search).get('id') || '0', 10);
        if (qid > 0) { try { await readById(qid); } catch (e) { alert(msg(e)); } }
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
}());
