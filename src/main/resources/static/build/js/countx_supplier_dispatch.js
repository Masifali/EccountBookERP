/* ============================================================================================
 * Screen 788 "Supplier Dispatch" - frmSupplierDispatch.cs (Architecture.WinApp.Purchase),
 * DocumentTypeId 246. Line references (:n) are frmSupplierDispatch.cs.
 *
 * Event for event the form's own logic: CmbSupplierDetail_Leave -> OrderBind (:1066 / :1218),
 * CmbOrderNoDetail_Leave -> BindCommissionAgent / BindItem (:1084 / :1121 / :1163), combitem_Leave (:1319),
 * CalculateWeight (:2486), TotalAmount (:2503), CalculateTotalInformation (:2412), the exchange-rate
 * recompute (:2441), ETACalculations (:3715), btnplus / btnUpdateDetail / DeleteDetailRow, history.
 * The server recomputes every total / amount / FcyAmount it saves and re-validates every reference;
 * the Doc No shown here is display only.
 *
 * .NET Math.Round(double) is banker's rounding (roundEven); a custom numeric format string and
 * MidpointRounding.AwayFromZero round away from zero (roundAway).
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/purchase/supplier-dispatch';
    var PRINT_RPT = '246_SupplierDispatchSlip.rpt';
    var MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var DEFAULT_TEXT = '-- Select --';

    if (window.DesktopCombo) {
        /* BindItem (:1163) / ItemWithoutOrderFill (:1263) - AllColumns over dtitem; display ItemName or ItemCode. */
        window.DesktopCombo.define('sdItem', [
            { caption: 'Item Name', flex: 4 },
            { caption: 'ItemName', flex: 4, key: 'item-name' },
            { caption: 'ItemCode', flex: 2, key: 'item-code' }
        ]);
        /* bindRateUomAndItemPackUom (:871) - the item's UOM schedule: UOMCode, Equivalent. */
        window.DesktopCombo.define('sdUom', [
            { caption: 'UOM', flex: 3 },
            { caption: 'Equivalent', flex: 2, key: 'eq', type: 'num' }
        ]);
    }

    var L = null;                       // lookups
    var st = {
        recId: 0, approved: false, rows: [], removed: [], updateIdx: -1,
        uoms: [], orders: [], hist: [], histSel: -1,
        vehPick: null, drvPick: null, hasVeh: false, hasDrv: false
    };

    // ============================================================================ helpers

    function $(id) { return document.getElementById(id); }
    function esc(v) {
        return String(v === undefined || v === null ? '' : v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    /** Conversion.ToDouble on text - thousands separators allowed, anything else 0. */
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
    /** x.ToString(clsGlobalVariables.stringFormatsingle) - "#,##0." + N zeros. */
    function fmtAmt(v) { var n = dec().amount; return group(roundAway(num(v), n), n, n); }
    /** clsGlobalVariables.DecimalRateFormate */
    function fmtRate(v) { var n = dec().rate; return group(roundAway(num(v), n), n, n); }
    /** clsGlobalVariables.stringFormatsingleForFcy */
    function fmtFcy(v) { var n = dec().fcy; return group(roundAway(num(v), n), n, n); }
    /** "#,##0.##.." - optional decimals up to n. */
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
    function dMMMyyyy(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || ''));
        return m ? m[3] + '-' + MONTHS[+m[2] - 1] + '-' + m[1] : '';
    }
    /** "dd-MMM-yy hh:mm tt" (HistoryGridSettings :2853) */
    function dMMMyyTime(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})[T ](\d{2}):(\d{2})/.exec(String(v || ''));
        if (!m) return dMMMyyyy(v);
        var h = +m[4], ap = h >= 12 ? 'PM' : 'AM';
        h = h % 12; if (h === 0) h = 12;
        return m[3] + '-' + MONTHS[+m[2] - 1] + '-' + m[1].slice(2) + ' ' + pad(h) + ':' + m[5] + ' ' + ap;
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
    /* The Attachment form (btnattachment_Click :3227 AT.Show()); saved with Insert(). */
    var AT = window.PurchaseDocAttachments ? PurchaseDocAttachments.create({
        type: 246, getId: function () { return st.recId; },
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
    /** combo.Value = v - a value not in the list leaves the combo without a selection. */
    function setVal(id, v) {
        var el = $(id);
        if (v !== null && v !== undefined && has(id, v)) el.value = String(v); else el.selectedIndex = -1;
    }
    function comboVal(id) { var el = $(id); return el.selectedIndex < 0 ? 0 : intOf(el.value); }
    function comboRaw(id) { var el = $(id); return el.selectedIndex < 0 ? '' : el.value; }
    /** combo.Text - the default row counts as empty. */
    function comboText(id) {
        var el = $(id);
        if (el.selectedIndex < 0) return '';
        var o = el.options[el.selectedIndex];
        if (o.value === '0' && o.textContent === DEFAULT_TEXT) return '';
        return o.textContent;
    }
    function active(id) { return $(id).selectedIndex >= 0; }                      // ActiveRow != null
    function clearCombo(id) { $(id).selectedIndex = -1; }                          // combo.Text = string.Empty
    function selectedAttr(id, attr) {
        var el = $(id);
        if (el.selectedIndex < 0) return '';
        return el.options[el.selectedIndex].getAttribute('data-' + attr) || '';
    }
    function emptyList(id) { $(id).innerHTML = ''; $(id).selectedIndex = -1; }     // DataSource = null; Text = ""
    function focus(id) { var el = $(id); if (!el) return; var w = el.__dtcombo && el.__dtcombo.input; (w || el).focus(); }

    function partyAttrs(r) { return { code: r.PartyCode, city: r.CityName, mobile: r.MobileNo }; }

    /** SupplierNameFill (:776) */
    function bindSuppliers() {
        var prev = comboRaw('CmbSupplierDetail');
        fill('CmbSupplierDetail', L.suppliers, 'Id', 'CompanyName', false, partyAttrs);
        setVal('CmbSupplierDetail', prev || null);
    }
    /** ItemWithoutOrderFill (:1263) / rdSearchByName_CheckedChanged (:1037) - ItemName or ItemCode shown. */
    function bindItemsAll() {
        var byCode = $('rdSearchByCode').checked, prev = comboVal('CmbItemNameDetail');
        var html = opt('0', DEFAULT_TEXT);
        (L.items || []).forEach(function (r) {
            html += opt(r.Id, byCode ? r.ItemCode : r.ItemName, { 'item-name': r.ItemName, 'item-code': r.ItemCode });
        });
        $('CmbItemNameDetail').innerHTML = html;
        if (prev > 0 && has('CmbItemNameDetail', prev)) $('CmbItemNameDetail').value = String(prev);
        else $('CmbItemNameDetail').selectedIndex = -1;
    }
    /** BindItem (:1163) - the order's lines. */
    function bindItemsOrder(orderId) {
        var byCode = $('rdSearchByCode').checked, prev = comboVal('CmbItemNameDetail'), html = '', n = 0;
        st.orders.forEach(function (o) {
            if (intOf(o.OrderId) !== orderId) return;
            n++;
            html += opt(o.OrderItemId, byCode ? o.ItemCode : o.ItemName, {
                'item-name': o.ItemName, 'item-code': o.ItemCode, crop: o.CropYearId, uom: o.OrderItemUOMId,
                qty: o.OrderItemQty, wt: o.NetWeight, rate: o.OrderItemRate, rateuom: o.OrderItemRateUOMId, odid: o.OrderDetailId
            });
        });
        if (n === 0) { emptyList('CmbItemNameDetail'); return; }
        $('CmbItemNameDetail').innerHTML = opt('0', DEFAULT_TEXT) + html;
        if (prev > 0 && has('CmbItemNameDetail', prev)) $('CmbItemNameDetail').value = String(prev);
        else $('CmbItemNameDetail').selectedIndex = -1;
    }
    function bindItems() {
        var oid = comboVal('CmbOrderNoDetail');
        if (active('CmbOrderNoDetail') && oid > 0 && st.orders.length) bindItemsOrder(oid); else bindItemsAll();
    }
    function bindLists() {
        bindSuppliers();
        var ref = comboVal('CmbRefPartyDetail');                                    // ReferencePartyFill (:837)
        fill('CmbRefPartyDetail', L.referenceParties, 'Id', 'Name', true);
        setVal('CmbRefPartyDetail', ref || null);
        var cur = comboVal('CmbCurrencyMain');                                      // CurrencyFill (:2348)
        fill('CmbCurrencyMain', L.currencies, 'Id', 'Name', true);
        setVal('CmbCurrencyMain', cur || null);
        bindItems();
        var crop = comboVal('CmbCropYearDetail');                                   // CropYear (:925)
        fill('CmbCropYearDetail', L.cropYears, 'Id', 'Name', true);
        setVal('CmbCropYearDetail', crop || null);
        var pack = comboVal('CmbPackingTypeDetail');                                // CmbPackingTypeFill (:992)
        fill('CmbPackingTypeDetail', L.packingTypes, 'Id', 'Name', true);
        setVal('CmbPackingTypeDetail', pack || null);
        var city = comboVal('CmbCityNameMain');                                     // cmbCityFill (:734)
        fill('CmbCityNameMain', L.cities, 'Id', 'Name', true);
        setVal('CmbCityNameMain', city || null);
        var br = comboVal('CmbBranchDetail');                                       // BranchDetailFill (:958)
        fill('CmbBranchDetail', L.receiverLocations, 'Id', 'Name', true);
        setVal('CmbBranchDetail', br || null);
    }
    /** ConfigurationDefault (:651) */
    function configurationDefault() {
        if (L.cityArea > 0) setVal('CmbCityNameMain', L.cityArea);
        if (L.defaultCropYear > 0) setVal('CmbCropYearDetail', L.defaultCropYear);
    }
    /** GetBaseCurrencyAndRate (:2390) */
    function baseCurrencyAndRate() {
        if (comboVal('CmbCurrencyMain') === 0) setVal('CmbCurrencyMain', L.baseCurrency);
        if (num($('txtExchangeRateMain').value) === 0) $('txtExchangeRateMain').value = netStr(num(L.baseCurrencyRate));
        exchangeRateChanged();
    }

    /** bindRateUomAndItemPackUom (:871) - the item's UOMs, pack/rate UOM retained by their text. */
    async function packUomBind(itemId) {
        var packText = comboText('CmbPackUomDetail'), rateText = comboText('CmbRateUomDetail');
        var list = itemId > 0 ? await api('GET', API + '/uoms?itemId=' + itemId) : [];
        st.uoms = list || [];
        ['CmbPackUomDetail', 'CmbRateUomDetail'].forEach(function (id, i) {
            fill(id, st.uoms, 'Id', 'UOMCode', true, function (r) { return { eq: r.Equivalent }; });
            var want = i === 0 ? packText : rateText, el = $(id), hit = -1;
            for (var k = 1; k < el.options.length; k++) if (el.options[k].textContent === want && want !== '') { hit = k; break; }
            el.selectedIndex = hit >= 0 ? hit : 0;
        });
        totalAmount();
    }
    function uomEq(id) { return active(id) ? num(selectedAttr(id, 'eq')) : 0; }

    // ======================================================================= feature ui

    function handleFeatures() {
        var branchCol = L.branchFeature && !L.branchImplemented;                     // HandleBranchFeature (:555)
        show('label63', branchCol); show('label8', branchCol);
        showCombo('CmbBranchDetail', branchCol);
        $('txtRemarksdetail').style.width = branchCol ? '343px' : '489px';
        var mc = !!L.multiCurrency;                                                  // HandleMultiCurrencyFeature (:566)
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

    // ======================================================================= calculations

    /** CalculateWeight (:2486) */
    function calculateWeight() {
        var qty = num($('txtQtyDetail').value);
        if (active('CmbPackUomDetail') && qty > 0) $('txtWeightDetail').value = fmtHash(uomEq('CmbPackUomDetail') * qty, 4);
    }
    /** TotalAmount (:2503) - Math.Round(x, 0, AwayFromZero), "#,##0.####". */
    function totalAmount() {
        var w = num($('txtWeightDetail').value), rate = num($('txtRateDetail').value);
        if (w > 0 && active('CmbRateUomDetail') && rate > 0) {
            var eq = uomEq('CmbRateUomDetail');
            var amt = w / eq * rate;
            $('txtAmountDetail').value = isFinite(amt) ? fmtHash(roundAway(amt, 0), 4) : '';
        }
    }
    function detailCalcAll() { calculateWeight(); totalAmount(); }

    /** txtExchangeRate_TextChanged (:2441) */
    function exchangeRateChanged() {
        var ex = num($('txtExchangeRateMain').value);
        st.rows.forEach(function (r) { r.FcyAmount = ex > 0 ? roundEven(num(r.Amount) / ex, dec().fcy) : 0; });
        calculateTotalInformation();
        renderGrid();
    }
    /** CalculateTotalInformation (:2412) */
    function calculateTotalInformation() {
        if (st.rows.length > 0) {
            $('txtQtyMain').value = fmtHash(roundEven(sum(st.rows, 'Qty'), 2), 2);
            $('txtTotalWeightMain').value = fmtHash(roundEven(sum(st.rows, 'Weight'), 2), 2);
            $('txtTotalAmountMain').value = fmtAmt(sum(st.rows, 'Amount'));
            $('txtFcyAmountMain').value = fmtFcy(sum(st.rows, 'FcyAmount'));
        } else {
            $('txtQtyMain').value = '0'; $('txtTotalWeightMain').value = '0';
            $('txtTotalAmountMain').value = '0'; $('txtFcyAmountMain').value = '0';
        }
    }

    // ============================================================================ order binding

    /** OrderBind (:1218) */
    async function orderBind(supplierId) {
        var keep = comboVal('CmbOrderNoDetail');
        st.orders = await api('GET', API + '/orders?supplierId=' + supplierId) || [];
        if (st.orders.length > 0) {
            var seen = {}, html = '';
            st.orders.forEach(function (o) {
                var k = intOf(o.OrderId);
                if (seen[k]) return;
                seen[k] = true;
                html += opt(o.OrderId, o.OrderNo);
            });
            $('CmbOrderNoDetail').innerHTML = html;
            if (keep > 0 && has('CmbOrderNoDetail', keep)) $('CmbOrderNoDetail').value = String(keep);
            else $('CmbOrderNoDetail').selectedIndex = -1;
        } else {
            emptyList('CmbOrderNoDetail');
        }
    }
    /** CmbSupplierDetail_Leave (:1066) */
    async function supplierLeave() {
        var sid = comboVal('CmbSupplierDetail');
        if (active('CmbSupplierDetail') && sid > 0) { await orderBind(sid); return; }
        st.orders = [];
        emptyList('CmbOrderNoDetail');
    }
    /** CmbOrderNoDetail_Leave (:1084) -> BindOtherCombosByOrder (:1099) */
    function orderLeave() {
        var oid = comboVal('CmbOrderNoDetail');
        if (!(active('CmbOrderNoDetail') && oid > 0)) return;
        if (st.orders.length > 0) {
            bindCommissionAgent(oid);
            bindItemsOrder(oid);
        } else {
            bindItemsAll();
            emptyList('CmbCommissionAgentDetail');
        }
    }
    /** BindCommissionAgent (:1121) */
    function bindCommissionAgent(orderId) {
        var keep = comboVal('CmbCommissionAgentDetail'), html = '', n = 0;
        st.orders.forEach(function (o) {
            if (intOf(o.OrderId) === orderId && intOf(o.CommissionAgentId) > 0) { n++; html += opt(o.CommissionAgentId, o.CommissionAgent); }
        });
        if (n === 0) { emptyList('CmbCommissionAgentDetail'); return; }
        $('CmbCommissionAgentDetail').innerHTML = opt('0', DEFAULT_TEXT) + html;
        if (keep > 0 && has('CmbCommissionAgentDetail', keep)) $('CmbCommissionAgentDetail').value = String(keep);
        else $('CmbCommissionAgentDetail').selectedIndex = -1;
    }
    /** combitem_Leave (:1319) */
    async function itemLeave() {
        await packUomBind(comboVal('CmbItemNameDetail'));
        if (active('CmbOrderNoDetail') && comboVal('CmbOrderNoDetail') > 0 && active('CmbItemNameDetail') && comboVal('CmbItemNameDetail') > 0) {
            var a = function (k) { return selectedAttr('CmbItemNameDetail', k); };
            if (a('odid') === '' && a('crop') === '') return;                       // not an order line (full item list)
            if (comboVal('CmbCropYearDetail') === 0) setVal('CmbCropYearDetail', intOf(a('crop')));
            if (comboVal('CmbPackUomDetail') === 0) { setVal('CmbPackUomDetail', intOf(a('uom'))); detailCalcAll(); }
            if (num($('txtQtyDetail').value) === 0) { $('txtQtyDetail').value = fmtHash(num(a('qty')), 2); detailCalcAll(); }
            if (num($('txtWeightDetail').value) === 0) { $('txtWeightDetail').value = fmtHash(num(a('wt')), 2); detailCalcAll(); }
            if (num($('txtRateDetail').value) === 0) { $('txtRateDetail').value = fmtHash(num(a('rate')), 2); totalAmount(); }
            if (comboVal('CmbRateUomDetail') === 0) { setVal('CmbRateUomDetail', intOf(a('rateuom'))); totalAmount(); }
        }
    }

    // ============================================================================ grid

    function gridColumns() {
        var cols = [{ k: 'Delete', c: 'X', btn: 'X', w: 30 }, { k: 'Edit', c: 'Edit', btn: 'Edit', w: 60 }];
        cols.push({ k: 'Supplier', c: 'Supplier' }, { k: 'OrderNo', c: 'OrderNo' }, { k: 'CommissionAgent', c: 'CommissionAgent' },
            { k: 'ReferenceParty', c: 'ReferenceParty' }, { k: 'ItemCode', c: 'ItemCode' }, { k: 'ItemName', c: 'ItemName' },
            { k: 'CropYear', c: 'CropYear' }, { k: 'PackingType', c: 'PackingType' }, { k: 'PackUom', c: 'PackUom' },
            { k: 'Qty', c: 'Qty', n: 'q', tot: 1 }, { k: 'Weight', c: 'Weight', n: 'q', tot: 1 },
            { k: 'RateUom', c: 'RateUom' }, { k: 'Rate', c: 'Rate', n: 'r' }, { k: 'Amount', c: 'Amount', n: 'a', tot: 1 });
        if (L.multiCurrency) cols.push({ k: 'FcyAmount', c: 'FcyAmount', n: 'a', tot: 1 });
        cols.push({ k: 'Remarks', c: 'Remarks' });
        if (L.branchFeature && !L.branchImplemented) cols.push({ k: 'ReceiverLocation', c: 'ReceiverLocation' });
        return cols;
    }
    function numText(kind, v) {
        if (kind === 'a') return fmtAmt(v);
        if (kind === 'r') return fmtRate(v);
        return fmtHash(v, 3);
    }
    function renderGrid() {
        var cols = gridColumns(), t = $('grd');
        t.tHead.innerHTML = '<tr>' + cols.map(function (c) { return '<th' + (c.w ? ' style="width:' + c.w + 'px"' : '') + '>' + esc(c.c) + '</th>'; }).join('') + '</tr>';
        var html = '';
        st.rows.forEach(function (r, i) {
            html += '<tr data-i="' + i + '">' + cols.map(function (c) {
                if (c.btn) return '<td><button type="button" class="gbtn" data-act="' + c.k + '">' + c.btn + '</button></td>';
                if (c.n) return '<td class="n">' + esc(numText(c.n, r[c.k])) + '</td>';
                return '<td>' + esc(r[c.k]) + '</td>';
            }).join('') + '</tr>';
        });
        t.tBodies[0].innerHTML = html;
        t.tFoot.innerHTML = st.rows.length ? '<tr>' + cols.map(function (c) {
            return c.tot ? '<td class="n">' + esc(numText(c.n, sum(st.rows, c.k))) + '</td>' : '<td></td>';
        }).join('') + '</tr>' : '';
    }
    function onGridClick(e) {
        var b = e.target.closest('button[data-act]');
        if (!b) return;
        var tr = b.closest('tr[data-i]'), i = +tr.getAttribute('data-i');
        if (b.getAttribute('data-act') === 'Delete') deleteDetailRow(i);
        else gridDoubleClick(i);
    }

    /**
     * DeleteDetailRow (:1583). A saved row (Id > 0) is asked about and queued in lstRemoveRecord
     * (ActionTypeId 3, sent as `removed`); an unsaved row is simply dropped. No footer refresh follows, as on the desktop.
     */
    function deleteDetailRow(i) {
        var r = st.rows[i];
        if (!r) return;
        if (st.updateIdx !== -1) { alert('Please Reset the Detail First...'); return; }
        if (intOf(r.Id) > 0) {
            if (!confirm('Are you sure to Delete?')) return;
            st.removed.push({ Id: intOf(r.Id) });
        }
        st.rows.splice(i, 1);
        renderGrid();
    }

    /** grd_DoubleClick (:1717) */
    async function gridDoubleClick(i) {
        var r = st.rows[i];
        if (!r) return;
        st.updateIdx = i;
        setVal('CmbSupplierDetail', r.SupplierId);
        await supplierLeave();
        setVal('CmbOrderNoDetail', r.OrderId);
        orderLeave();
        setVal('CmbCommissionAgentDetail', r.CommissionAgentId);
        setVal('CmbRefPartyDetail', r.ReferencePartyId);
        setVal('CmbItemNameDetail', r.ItemId);
        await itemLeave();
        setVal('CmbRateUomDetail', r.RateUomId);
        setVal('CmbPackUomDetail', r.PackUomId);
        setVal('CmbCropYearDetail', r.CropYearId);
        setVal('CmbPackingTypeDetail', r.PackingTypeId);
        $('txtQtyDetail').value = fmtHash(r.Qty, 3);
        $('txtWeightDetail').value = fmtHash(r.Weight, 3);
        $('txtRateDetail').value = fmtHash(r.Rate, 3);
        setVal('CmbRateUomDetail', r.RateUomId);
        $('txtAmountDetail').value = fmtHash(r.Amount, 3);
        $('txtRemarksdetail').value = r.Remarks || '';
        setVal('CmbBranchDetail', r.ReceiverLocationId);
        show('btnplus', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
        focus('CmbItemNameDetail');
    }

    // ============================================================================ detail

    function emptyOrZero(id) { var t = $(id).value.trim(); return t === '' || t === '0' || num(t) === 0; }
    /** FormValidationDetail - messages verbatim, in order. */
    function formValidationDetail() {
        var checks = [
            [!active('CmbSupplierDetail') || comboVal('CmbSupplierDetail') === 0, 'Supplier Name Field is Required', 'CmbSupplierDetail'],
            [!active('CmbItemNameDetail') || comboVal('CmbItemNameDetail') === 0, 'Item Name Field is Required', 'CmbItemNameDetail'],
            [!active('CmbCropYearDetail') || comboVal('CmbCropYearDetail') === 0, 'Crop Year Field is Required', 'CmbCropYearDetail'],
            [!active('CmbPackingTypeDetail') || comboVal('CmbPackingTypeDetail') === 0, 'Pack Type Field is Required', 'CmbPackingTypeDetail'],
            [!active('CmbPackUomDetail') || comboVal('CmbPackUomDetail') === 0, 'Item Pack Unit Field is Required', 'CmbPackUomDetail'],
            [emptyOrZero('txtQtyDetail'), 'Qty Field is Required', 'txtQtyDetail'],
            [emptyOrZero('txtWeightDetail'), 'Net Weight Field is Required', 'txtWeightDetail'],
            [emptyOrZero('txtRateDetail'), 'Item Rate Field is Required', 'txtRateDetail'],
            [!active('CmbRateUomDetail') || comboVal('CmbRateUomDetail') === 0, 'Rate UOM Field is Required', 'CmbRateUomDetail'],
            [emptyOrZero('txtAmountDetail'), 'Item amount Field is Required', 'txtAmountDetail'],
            [!!L.branchFeature && (!active('CmbBranchDetail') || comboVal('CmbBranchDetail') === 0), "Receiver's Location Field is Required", 'CmbBranchDetail']
        ];
        for (var i = 0; i < checks.length; i++) {
            if (checks[i][0]) { alert(checks[i][1]); focus(checks[i][2]); return false; }
        }
        return true;
    }
    function itemRow() {
        return { ItemName: selectedAttr('CmbItemNameDetail', 'item-name'), ItemCode: selectedAttr('CmbItemNameDetail', 'item-code'), OrderDetailId: intOf(selectedAttr('CmbItemNameDetail', 'odid')) };
    }
    function rowFromDetail(base) {
        var it = itemRow(), ex = num($('txtExchangeRateMain').value), amount = num($('txtAmountDetail').value);
        var oid = comboVal('CmbOrderNoDetail');
        var odid = oid > 0 && comboVal('CmbItemNameDetail') > 0 ? it.OrderDetailId : 0;
        return Object.assign(base, {
            SupplierId: comboVal('CmbSupplierDetail'), Supplier: comboText('CmbSupplierDetail').trim(),
            OrderId: oid, OrderNo: comboText('CmbOrderNoDetail').trim(),
            CommissionAgentId: comboVal('CmbCommissionAgentDetail'), CommissionAgent: comboText('CmbCommissionAgentDetail').trim(),
            ReferencePartyId: comboVal('CmbRefPartyDetail'), ReferenceParty: comboText('CmbRefPartyDetail').trim(),
            OrderDetailId: odid, ItemId: comboVal('CmbItemNameDetail'), ItemCode: it.ItemCode, ItemName: it.ItemName,
            CropYearId: comboVal('CmbCropYearDetail'), CropYear: comboText('CmbCropYearDetail').trim(),
            PackingTypeId: comboVal('CmbPackingTypeDetail'), PackingType: comboText('CmbPackingTypeDetail').trim(),
            PackUomId: comboVal('CmbPackUomDetail'), PackUom: comboText('CmbPackUomDetail').trim(), PackUomEquivalent: uomEq('CmbPackUomDetail'),
            Qty: num($('txtQtyDetail').value), Weight: num($('txtWeightDetail').value),
            RateUomId: comboVal('CmbRateUomDetail'), RateUom: comboText('CmbRateUomDetail'), RateUomEquivalent: uomEq('CmbRateUomDetail'),
            Rate: num($('txtRateDetail').value), Amount: amount, FcyAmount: ex > 0 ? amount / ex : 0,
            Remarks: $('txtRemarksdetail').value.trim(), ReceiverLocationId: comboVal('CmbBranchDetail'),
            ReceiverLocation: comboText('CmbBranchDetail')
        });
    }
    /** btnplus_Click (:1522) */
    function btnPlus() {
        if (!formValidationDetail()) return;
        st.rows.push(rowFromDetail({ Id: 0 }));
        clearItem(); clearCombo('CmbPackUomDetail'); clearCombo('CmbPackingTypeDetail');
        ['txtQtyDetail', 'txtWeightDetail', 'txtRateDetail', 'txtAmountDetail', 'txtRemarksdetail'].forEach(function (id) { $(id).value = ''; });
        clearCombo('CmbRateUomDetail');
        focus('CmbItemNameDetail');
        calculateTotalInformation();
        renderGrid();
    }
    /** btnUpdateDetail_Click (:1758) */
    function btnUpdateDetail() {
        if (!formValidationDetail()) return;
        var r = st.rows[st.updateIdx];
        if (!r) return;
        rowFromDetail(r);
        exchangeRateChanged();                                                       // txtExchangeRate_TextChanged(null, null)
        resetDetail();
        focus('CmbItemNameDetail');
    }
    function clearItem() {
        clearCombo('CmbItemNameDetail');
        packUomBind(0).catch(function () { });                                      // no request for item 0: runs synchronously
    }
    /** ResetDetail (:2314) */
    function resetDetail() {
        st.updateIdx = -1;
        clearItem(); clearCombo('CmbPackUomDetail');
        ['txtQtyDetail', 'txtWeightDetail', 'txtRateDetail', 'txtAmountDetail'].forEach(function (id) { $(id).value = ''; });
        clearCombo('CmbRateUomDetail');
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
    }

    // ======================================================================== ETA

    /** ETACalculations (:3715) */
    function etaCalculations() {
        var before = $('txtETAdestinationMain').value;
        $('txtETAdestinationMain').value = addDaysIso($('txtDepartureDateMain').value || today(), intOf($('txtTransitDaysMain').value));
        if ($('txtETAdestinationMain').value !== before) etaChanged();
    }
    /** txtETAdestinationMain_ValueChanged (:3742) */
    function etaChanged() {
        $('txtTransitDaysMain').value = String(Math.abs(daysBetween($('txtDepartureDateMain').value, $('txtETAdestinationMain').value)));
    }
    /** txtDocDateMain_ValueChanged (:3702) / txtTransitDaysMain_TextChanged (:3729) */
    function docDateChanged() { $('txtDepartureDateMain').value = $('txtDocDateMain').value; etaCalculations(); }

    // ======================================================================== pictures

    function pickPicture(file, which) {
        if (!file) return;
        if (!/\.(jpe?g|gif|png)$/i.test(file.name)) { alert('Select a jpg, jpeg, gif or png picture'); return; }
        var rd = new FileReader();
        rd.onload = function () {
            var url = String(rd.result), b64 = url.substring(url.indexOf(',') + 1);
            var pick = { name: file.name, base64: b64 };
            if (which === 'vehicle') st.vehPick = pick; else st.drvPick = pick;
            setPicture(which, url);
        };
        rd.readAsDataURL(file);
    }
    function setPicture(which, url) {
        var img = $(which === 'vehicle' ? 'PicVehicleImage' : 'PicDriverImage').querySelector('img');
        if (url) { img.src = url; img.hidden = false; } else { img.removeAttribute('src'); img.hidden = true; }
    }

    // ============================================================================ save

    /** FormValidation (:1321) - messages verbatim, in order. */
    function formValidation() {
        var t;
        t = $('txtDocNoMain').value.trim();
        if (t === '' || t === '0') { alert('Doc No Field is Required'); return false; }
        t = $('txtVehicleNoMain').value.trim();
        if (t === '' || t === '0') { alert('Vehicle No Field is Required'); focus('txtVehicleNoMain'); return false; }
        t = $('txtBiltyNoMain').value.trim();
        if (t === '' || t === '0') { alert('Bilty No Field is Required'); focus('txtBiltyNoMain'); return false; }
        t = $('txtDriverNameMain').value.trim();
        if (t === '' || t === '0') { alert('Driver Name Field is Required'); focus('txtDriverNameMain'); return false; }
        t = $('txtDriverCell1Main').value.trim();
        if (t === '' || t === '0') { alert('Driver CellNo Field is Required'); focus('txtDriverCell1Main'); return false; }
        t = $('txtTransitDaysMain').value.trim();
        if (t === '' || intOf(t) === 0) { alert('TransitDays Field is Required'); focus('txtTransitDaysMain'); return false; }
        t = $('txtFreightMain').value.trim();
        if (t === '' || intOf(t) === 0) { alert('Freight Field is Required'); focus('txtFreightMain'); return false; }
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
            VehicleNo: $('txtVehicleNoMain').value, BiltyNo: $('txtBiltyNoMain').value, DriverName: $('txtDriverNameMain').value,
            Driver1CellNo: $('txtDriverCell1Main').value, Driver2CellNo: $('txtDriverCell2Main').value,
            Freight: $('txtFreightMain').value, OtherExpense: $('txtOtherExpenseMain').value,
            CityId: comboVal('CmbCityNameMain'), SupplierWbWeight: $('txtSupplierWbWtMain').value.trim(),
            CurrencyId: comboVal('CmbCurrencyMain'), ExchangeRate: $('txtExchangeRateMain').value,
            RemarksHeader: $('txtRemarksMain').value,
            rows: st.rows.map(function (r) {
                return {
                    Id: intOf(r.Id), SupplierId: intOf(r.SupplierId), OrderId: intOf(r.OrderId), OrderDetailId: intOf(r.OrderDetailId),
                    CommissionAgentId: intOf(r.CommissionAgentId), ReferencePartyId: intOf(r.ReferencePartyId), ItemId: intOf(r.ItemId),
                    CropYearId: intOf(r.CropYearId), PackingTypeId: intOf(r.PackingTypeId), PackUomId: intOf(r.PackUomId),
                    Qty: num(r.Qty), Weight: num(r.Weight), RateUomId: intOf(r.RateUomId), Rate: num(r.Rate), Amount: num(r.Amount),
                    Remarks: r.Remarks || '', ReceiverLocationId: intOf(r.ReceiverLocationId)
                };
            }),
            removed: st.removed.map(function (x) { return { Id: intOf(x.Id) }; }),
            VehicleImagePick: st.vehPick || undefined, DriverImagePick: st.drvPick || undefined,
            attachments: AT ? AT.payload() : undefined
        };
    }

    /** Insert (:1877) - client-side checks up to the prompt, then the server runs every check again. */
    function insert(button) {
        if (st.rows.length === 0) { alert('Grid Record Not Found'); return; }
        if (!formValidation()) return;
        if (!confirm(st.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        return act(button, async function () {
            var res = await api('POST', API + '/save', payload());
            alert(res.message);
            var savedId = res.id;
            await reset();
            if ($('ChkBox').checked) await print(savedId);
        });
    }
    function btnSave() { st.approved = false; st.recId = 0; insert($('btnsave')); }   // :2088
    function btnUpdate() {                                                            // :2102
        if (st.recId === 0) { alert('Record Not Update because RecId Not Found'); return; }
        insert($('btnupdate'));
    }
    function btnDelete() {                                                            // :1835
        if (st.approved) { alert('Record Not Delete because Record has approved'); return; }
        if (st.recId <= 0) { alert('Record Not Found'); return; }
        if (!confirm('Are you sure to Delete?')) return;
        return act($('btnDelete'), async function () {
            await api('POST', API + '/' + st.recId + '/delete');
            alert('Delete Record Successfully');
            await reset();
        });
    }
    /** CommonServices.SupplierDispatchSlip246(id) - the Jasper print of the 246 slip. */
    async function print(id) {
        if (!rights().print) return;
        var w = window.open('about:blank', '_blank');                                   // opened now so a pop-up blocker lets it through
        try {
            await api('GET', API + '/' + intOf(id) + '/print-check');
            var url = '/reports/print/by-template/' + encodeURIComponent(PRINT_RPT) + '/pdf?id=' + intOf(id);
            if (w) w.location.href = url; else window.open(url, '_blank');
        } catch (e) { if (w) w.close(); alert(msg(e)); }
    }

    /** Reset (:2254) */
    async function reset() {
        st.recId = 0; st.approved = false; st.updateIdx = -1; st.removed = [];
        st.vehPick = null; st.drvPick = null;
        if (AT) AT.reset();
        show('btnsave', true); show('btnupdate', false); show('btnDelete', false); show('btnSaveAs', false);
        ['txtVehicleNoMain', 'txtBiltyNoMain', 'txtDriverNameMain', 'txtDriverCell1Main', 'txtDriverCell2Main', 'txtRemarksMain',
            'txtTransitDaysMain', 'txtFreightMain', 'txtOtherExpenseMain', 'txtSupplierWbWtMain', 'txtQtyMain', 'txtTotalWeightMain',
            'txtTotalAmountMain', 'txtFcyAmountMain'].forEach(function (id) { $(id).value = ''; });
        ['CmbSupplierDetail', 'CmbCommissionAgentDetail', 'CmbRefPartyDetail', 'CmbItemNameDetail', 'CmbPackUomDetail', 'CmbRateUomDetail']
            .forEach(clearCombo);
        ['txtQtyDetail', 'txtWeightDetail', 'txtRateDetail', 'txtAmountDetail'].forEach(function (id) { $(id).value = ''; });
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        st.rows = [];
        renderGrid();
        setPicture('vehicle', ''); setPicture('driver', '');
        var n = await api('GET', API + '/numbers');
        $('txtDocNoMain').value = n.docNo; $('txtBranchSrNo').value = n.branchSrNo;
        ['branchImplemented', 'itemSearchByCode', 'defaultDaysToLessFromHistoryFromDate', 'defaultCropYear', 'cityArea',
            'baseCurrency', 'baseCurrencyRate'].forEach(function (k) { L[k] = n[k]; });
        configurationDefault();
        $('txtDocDateMain').focus();
    }

    /** btnRefresh_Click (:2219) */
    function refresh() {
        return act($('btnRefresh'), async function () {
            var r = await api('GET', API + '/refresh');
            Object.keys(r).forEach(function (k) { L[k] = r[k]; });
            baseCurrencyAndRate();
            bindLists();
            configurationDefault();
        });
    }

    /** ReadById */
    async function readById(id) {
        var po = await api('GET', API + '/' + id);
        if (!po || !po.rows || !po.rows.length) return;
        st.recId = id;
        tab('tabPage1');
        $('txtDocDateMain').value = isoDay(po.DocDate); docDateChanged();
        $('txtDocNoMain').value = po.DocNo; $('txtBranchSrNo').value = po.BranchSrNo;
        $('txtVehicleNoMain').value = po.VehicleNo || ''; $('txtBiltyNoMain').value = po.BiltyNo || '';
        $('txtDriverNameMain').value = po.DriverName || '';
        $('txtDriverCell1Main').value = po.Driver1CellNo || ''; $('txtDriverCell2Main').value = po.Driver2CellNo || '';
        $('txtDepartureDateMain').value = isoDay(po.DepartureDate);
        $('txtTransitDaysMain').value = String(intOf(po.TransitDays));
        $('txtETAdestinationMain').value = isoDay(po.ETAdestination); etaChanged();
        $('txtFreightMain').value = fmtHash(po.Freight, 2); $('txtOtherExpenseMain').value = fmtHash(po.OtherExpense, 2);
        setVal('CmbCityNameMain', po.CityId);
        $('txtSupplierWbWtMain').value = fmtHash(po.SupplierWbWeight, 2);
        $('txtQtyMain').value = fmtHash(po.TotalQty, 2); $('txtTotalWeightMain').value = fmtHash(po.TotalWeight, 2);
        $('txtTotalAmountMain').value = fmtAmt(po.TotalAmount);
        setVal('CmbCurrencyMain', po.CurrencyId);
        $('txtExchangeRateMain').value = fmtHash(po.ExchangeRate, 2);
        $('txtFcyAmountMain').value = fmtFcy(po.FcyAmount);
        $('txtRemarksMain').value = po.RemarksHeader || '';
        st.approved = !!po.IsApproved;
        if (intOf(po.CurrencyId) === 0) baseCurrencyAndRate();
        st.removed = []; st.vehPick = null; st.drvPick = null;
        st.rows = po.rows.map(function (d) { return Object.assign({}, d); });
        renderGrid();
        setPicture('vehicle', po.HasVehicleImage ? API + '/' + id + '/image/vehicle?t=' + Date.now() : '');
        setPicture('driver', po.HasDriverImage ? API + '/' + id + '/image/driver?t=' + Date.now() : '');
        show('btnsave', false); show('btnupdate', true); show('btnDelete', true);
    }

    // ============================================================================ history

    function historyBranchFill() {                                                   // HistoryBranchComboFill (:2613)
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

    /* HistoryGridSettings (:2853) - Edit / Print first, Id and DocumentTypeId hidden. */
    var H_COLS = [
        ['DocNo', 'DocNo', 'i'], ['BranchSrNo', 'BranchSrNo', 'i', 'branch'], ['DocDate', 'DocDate', 'd'], ['VehicleNo', 'VehicleNo'],
        ['BiltyNo', 'BiltyNo'], ['DriverName', 'DriverName'], ['Driver1CellNo', 'Driver1CellNo'], ['Driver2CellNo', 'Driver2CellNo'],
        ['DepartureDate', 'DepartureDate', 'd'], ['TransitDays', 'TransitDays', 'i'], ['ETADestination', 'ETADestination', 'd'],
        ['TotalQty', 'TotalQty', 'q'], ['SupplierWbWeight', 'SupplierWbWeight', 'q'], ['TotalWeight', 'TotalWeight', 'q'],
        ['TotalAmount', 'TotalAmount', 'a'], ['FcyAmount', 'FcyAmount', 'a', 'fcy'], ['Freight', 'Freight', 'q'],
        ['OtherExpense', 'OtherExpense', 'q'], ['CityName', 'CityName'], ['RemarksHeader', 'RemarksHeader'],
        ['EntryDate', 'EntryDate', 't'], ['EntryUser', 'EntryUser'], ['ModifyDate', 'ModifyDate', 't'], ['ModifyUser', 'ModifyUser'],
        ['IsApproved', 'IsApproved'], ['ApprovedDate', 'ApprovedDate', 't'], ['ApprovedUser', 'ApprovedUser'],
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
            case 't': return dMMMyyTime(v);
            case 'q': return fmtHash(v, 3);
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
    function renderHistory() {
        var cols = histCols(), t = $('grdhistory');
        t.tHead.innerHTML = '<tr><th>Edit</th><th>Print</th>' + cols.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>'
            + '<tr class="flt"><td></td><td></td>' + cols.map(function (c) {
                return '<td><input type="text" data-f="' + c[0] + '" value="' + esc(hFilter[c[0]] || '') + '"></td>';
            }).join('') + '</tr>';
        var idx = histVisibleRows();
        t.tBodies[0].innerHTML = idx.map(function (i) {
            var r = st.hist[i];
            return '<tr data-i="' + i + '"' + (i === st.histSel ? ' class="is-sel"' : '') + '>'
                + '<td><button type="button" class="gbtn" data-act="Edit">Edit</button></td>'
                + '<td><button type="button" class="gbtn" data-act="Print">Print</button></td>'
                + cols.map(function (c) {
                    if (c[2] === 'l') return '<td class="n"><a class="lnk" tabindex="0" data-attach="' + intOf(r.Id) + '" title="Attachments">' + esc(r[c[0]]) + '</a></td>';
                    var numeric = c[2] === 'q' || c[2] === 'a' || c[2] === 'i';
                    return '<td' + (numeric ? ' class="n"' : '') + '>' + esc(histCell(c, r[c[0]])) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        t.tFoot.innerHTML = idx.length ? '<tr><td></td><td></td>' + cols.map(function (c) {
            if (c[2] === 'q' || c[2] === 'a') {
                var s = 0; idx.forEach(function (i) { s += num(st.hist[i][c[0]]); });
                return '<td class="n">' + esc(histCell(c, s)) + '</td>';
            }
            return '<td></td>';
        }).join('') + '</tr>' : '';
    }
    /** gridhistoryfill (:2718) */
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
    /** GridDetailBind (:3037) + DataGridHistoryDetailSetting (:3091) */
    function renderHistoryDetail(po) {
        var t = $('DataGridHistoryDetail');
        if (!po) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; return; }
        var rows = po.rows || [];
        var cols = [['Supplier', 'Supplier'], ['OrderNo', 'OrderNo'], ['CommissionAgent', 'CommissionAgent'], ['ReferenceParty', 'ReferenceParty'],
            ['ItemCode', 'ItemCode'], ['ItemName', 'ItemName'], ['CropYear', 'CropYear'], ['PackingType', 'PackingType'], ['PackUom', 'PackUom'],
            ['Qty', 'Qty', 'q'], ['Weight', 'Weight', 'q'], ['RateUom', 'RateUom'], ['Rate', 'Rate', 'r'], ['Amount', 'Amount', 'a']];
        if (L.multiCurrency) cols.push(['FcyAmount', 'FcyAmount', 'a']);
        cols.push(['Remarks', 'Remarks']);
        if (L.branchFeature && !L.branchImplemented) cols.push(['ReceiverLocation', 'ReceiverLocation']);
        function cell(c, v) { return c[2] === 'q' ? fmtHash(v, 3) : c[2] === 'a' ? fmtAmt(v) : c[2] === 'r' ? fmtRate(v) : (v == null ? '' : v); }
        t.tHead.innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = rows.map(function (r) {
            return '<tr>' + cols.map(function (c) {
                var n = c[2] === 'q' || c[2] === 'a' || c[2] === 'r';
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
    /** Edit button (:2950) and grdhistory_DoubleClick (:3002): Reset + ReadById. */
    function editHistory(i) {
        var id = st.hist[i] && st.hist[i].Id;
        if (!id) return;
        return act(null, async function () { await reset(); st.recId = id; await readById(id); });
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
        if (link) { if (AT) AT.view(intOf(link.getAttribute('data-attach'))); return; }   // grdhistory_LinkClicked (:2985)
        if (e.target.closest('input,select,a')) return;
        if (i !== st.histSel) selectHistoryRow(i);
    }
    /** btnResetHistory_Click (:2676) */
    function resetHistory() {
        $('FromDateHistory').value = addDaysIso(today(), -3);
        $('ToDateHistory').value = today();
        $('FromDocNoHistory').value = ''; $('ToDocNoHistory').value = '';
        st.hist = []; st.histSel = -1; hFilter = {};
        $('grdhistory').tHead.innerHTML = ''; $('grdhistory').tBodies[0].innerHTML = ''; $('grdhistory').tFoot.innerHTML = '';
        renderHistoryDetail(null);
        $('drdocdate').checked = true;
    }
    /** btnRefreshHistory_Click (:2694) */
    function refreshHistory() {
        return act($('btnRefreshHistory'), async function () {
            var r = await api('GET', API + '/history-refresh');
            L.historyBranches = r.historyBranches; L.userBranchId = r.userBranchId; L.userBranchName = r.userBranchName;
            historyBranchFill();
        });
    }

    // ============================================================================ tabs + keys

    function tab(id) {
        ['tabPage1', 'tabPage2'].forEach(function (p) { $(p).classList.toggle('is-active', p === id); });
        $('tabBtnForm').classList.toggle('is-active', id === 'tabPage1');
        $('tabBtnHistory').classList.toggle('is-active', id === 'tabPage2');
        if (id === 'tabPage2') $('FromDateHistory').focus(); else $('txtDocDateMain').focus();   // tabControl1_SelectedIndexChanged (:2657)
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
    function shortcutKeys() {                                                        // MakeShortCutKeys (:3504)
        $('grdKeys').tBodies[0].innerHTML = KEYS.map(function (k) { return '<tr><td>' + esc(k[0]) + '</td><td>' + esc(k[1]) + '</td></tr>'; }).join('');
        show('dlgKeys', true);
    }
    /** SupplierDispatch_KeyDown (:3371). Keys the browser reserves (Ctrl+N / Ctrl+T) may not reach the page. */
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
            else if (c && k === 'F1') window.open('/party-processing/reference-parties', '_blank');     // BtnRefPartyForm_Click (:3359)
            else if (c && k === 'F10') { if (AT) AT.open($('btnattachment')); }                        // btnattachment_Click
            else if (c && k === 'F12') { if (visibleEnabled('btnSaveAs')) btnSave(); }                  // btnSaveAs_Click
            else if (c && k === 'ArrowDown') { var g = $('grd').querySelector('button'); if (g) g.focus(); }
            else if (c && k === 'ArrowUp') focus('CmbItemNameDetail');
            else if (c && (k === 'p' || k === 'P')) { if (rights().print) print(st.recId); }
            else if (e.altKey && k === '1') { if (rights().print) print(st.recId); }
            else if (c && e.shiftKey && k === 'Delete') { if (visibleEnabled('btnDelete')) btnDelete(); }
            else handled = false;
        } else {
            if (c && (k === 's' || k === 'S')) showHistory();
            else if (c && (k === 'n' || k === 'N')) resetHistory();
            else if (c && (k === 'r' || k === 'R')) refreshHistory();
            else if (c && k === 'F5') $('FromDateHistory').focus();
            else if (c && k === 'ArrowDown') { var h = $('grdhistory').querySelector('tbody button'); if (h) h.focus(); }
            else if (c && k === 'ArrowUp') $('FromDateHistory').focus();
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
                var w = t.value.replace(/[^0-9.\-]/g, '');
                if (w !== t.value) t.value = w;
            }
        }, true);
    }

    function wire() {
        $('btnnew').addEventListener('click', function () { act(this, reset); });
        $('btnRefresh').addEventListener('click', refresh);
        $('btnsave').addEventListener('click', btnSave);
        $('btnSaveAs').addEventListener('click', btnSave);                           // btnSaveAs_Click (:1863)
        $('btnupdate').addEventListener('click', btnUpdate);
        $('btnDelete').addEventListener('click', btnDelete);
        $('btnprint').addEventListener('click', function () { var b = this; act(b, function () { return print(st.recId); }); });
        $('btnattachment').addEventListener('click', function () { if (AT) AT.open(this); });
        $('BtnRefPartyForm').addEventListener('click', function () { window.open('/party-processing/reference-parties', '_blank'); });
        $('BtnShortCutkeys').addEventListener('click', shortcutKeys);
        $('dlgKeysClose').addEventListener('click', function () { show('dlgKeys', false); });
        $('tabBtnForm').addEventListener('click', function () { tab('tabPage1'); });
        $('tabBtnHistory').addEventListener('click', function () { tab('tabPage2'); });

        $('txtDocDateMain').addEventListener('change', docDateChanged);
        $('txtTransitDaysMain').addEventListener('blur', docDateChanged);
        $('txtETAdestinationMain').addEventListener('change', etaChanged);
        $('txtExchangeRateMain').addEventListener('input', exchangeRateChanged);
        $('txtExchangeRateMain').addEventListener('blur', function () {              // txtExchangeRate_Leave (:2474)
            this.value = fmtRate(num(this.value)); exchangeRateChanged();
        });

        $('CmbSupplierDetail').addEventListener('change', function () { act(null, supplierLeave); });
        $('CmbOrderNoDetail').addEventListener('change', orderLeave);
        $('rdSearchByName').addEventListener('change', function () { bindItems(); focus('CmbItemNameDetail'); });
        $('rdSearchByCode').addEventListener('change', function () { bindItems(); focus('CmbItemNameDetail'); });
        $('CmbItemNameDetail').addEventListener('change', function () { act(null, itemLeave); });
        $('CmbPackUomDetail').addEventListener('change', detailCalcAll);
        $('txtQtyDetail').addEventListener('input', detailCalcAll);
        $('txtRateDetail').addEventListener('input', totalAmount);
        $('CmbRateUomDetail').addEventListener('change', totalAmount);
        $('btnplus').addEventListener('click', btnPlus);
        $('btnUpdateDetail').addEventListener('click', btnUpdateDetail);
        $('btnCancelUpdateDetial').addEventListener('click', resetDetail);

        $('BtnBrowseVehicleImage').addEventListener('click', function () { $('fileVehicleImage').click(); });
        $('BtnBrowseDriverImage').addEventListener('click', function () { $('fileDriverImage').click(); });
        $('fileVehicleImage').addEventListener('change', function () { pickPicture(this.files[0], 'vehicle'); this.value = ''; });
        $('fileDriverImage').addEventListener('change', function () { pickPicture(this.files[0], 'driver'); this.value = ''; });

        $('grd').addEventListener('click', onGridClick);
        $('grd').addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tbody tr[data-i]');
            if (tr && !e.target.closest('button')) gridDoubleClick(+tr.getAttribute('data-i'));
        });

        $('btnShow').addEventListener('click', showHistory);
        $('btnResetHistory').addEventListener('click', resetHistory);
        $('btnRefreshHistory').addEventListener('click', refreshHistory);
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
        /* grd_KeyDown :3537 - Ctrl+Space on a focused X / Edit button runs it, Ctrl+Enter edits the row. */
        document.addEventListener('keydown', function (e) {
            if (!e.ctrlKey || !e.target.closest) return;
            var tr = e.target.closest('#grd tbody tr[data-i]');
            if (!tr) return;
            var i = +tr.getAttribute('data-i'), btn = e.target.closest('button[data-act]');
            if (e.code === 'Space' && btn) { e.preventDefault(); if (btn.getAttribute('data-act') === 'Delete') deleteDetailRow(i); return; }
            if (e.key === 'Enter') { e.preventDefault(); gridDoubleClick(i); }
        });
        /* grdhistory_KeyDown :3573 - Ctrl+Space (or Enter) on a focused link cell runs the link. */
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
            ['grdhistory', 'DataGridHistoryDetail'].forEach(function (id) { var t = $(id); if (t && t.parentElement) PurchaseChrome.fullscreen(t.parentElement); });
        }
    }

    // =============================================================================== load

    /** SupplierDispatch_Load (:446) + InitializeDataAsync (:475) + InitializeComponentCustom (:584). */
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
        if (L.itemSearchByCode) $('rdSearchByCode').checked = true; else $('rdSearchByName').checked = true;
        $('txtDocNoMain').value = L.docNo; $('txtBranchSrNo').value = L.branchSrNo;
        $('txtDocDateMain').value = today();
        bindLists();
        handleFeatures();
        configurationDefault();
        renderGrid();
        await packUomBind(comboVal('CmbItemNameDetail')).catch(function (e) { alert(msg(e)); });
        /* Designer defaults: Departure and ETA pickers start at Now; Transit Days is empty (no ValueChanged at load). */
        $('txtDepartureDateMain').value = today();
        $('txtETAdestinationMain').value = today();
        historyBranchFill();
        historyDefaults();
        $('txtDocDateMain').focus();
        /* ?id= deep link: open the record after the lookups. */
        var qid = parseInt(new URLSearchParams(location.search).get('id') || '0', 10);
        if (qid > 0) { try { await readById(qid); } catch (e) { alert(msg(e)); } }
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
}());
