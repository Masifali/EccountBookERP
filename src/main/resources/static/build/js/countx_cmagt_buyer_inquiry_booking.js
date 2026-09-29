/* ===========================================================================
 * Commission Trading - Buyer Inquiry Booking (DocumentTypeId 1050)
 *
 * Ported from Architecture.WinApp.CommissionAgent.Transactions/frmBuyerInquiryBooking.cs.
 * Field, procedure and validation contract:
 *   claude/CMAGT-BUYER-INQUIRY-BOOKING-1050-CONTRACT.md
 *
 * Desktop behaviour reproduced here, with the source line that establishes each:
 *
 *   CalculateWeight()            :1792   weight = qty x PackUom.Equivalent
 *   Insert() line amounts        :1591   buyerAmount    = itemWeight / RateUom.Equivalent x buyerRate
 *                                :1593   SupplierAmount = itemWeight / RateUom.Equivalent x supplierRate
 *   txtValidityDays_TextChanged  :1899   validityDate = inquiryDate + validityDays
 *   datValidityUpto_ValueChanged :1912   validityDays = validityDate - inquiryDate   (two-way)
 *   CalculateExpiryDate()        :1868   expiry = deliveryDays == 0 ? start : start + days
 *   datDeliveryStartDate_...     :1848   a start date before the document date is rejected and snapped back
 *   CmbPaymentTerm_ValueChanged  :769    term 1 or 3 -> due days 0 and disabled; term 2 -> default 2
 *   CmbDeliveryToParty_Leave     :664    ship-to list filtered by party; clearing the party clears the address
 *   CmbShipToAddress_Leave       :681    picking an address back-fills its party
 *   CmbItemName_Leave            :825    item -> parent category, analysis group, last analysis, UOMs
 *   UpdateAmount()               :1230   sub-party Amount = ItemQty x Rate
 *   DeleteDetailRow()            :1171   a saved sub-party row is kept with actionTypeId = 3
 *   grdSubParty_KeyDown          :1237   Ctrl+D adds a row, Ctrl+Delete removes one
 *   Insert() quality rules       :1547   both range values or neither, from <= to, no negatives;
 *                                        only rows with both > 0 are persisted and they build
 *                                        QualitySpecifications as "[Param, from,to]"
 *   formvalidation()             :1379   the 18 messages below, in this order and wording
 *
 * Rules this file will not break:
 *   1. A UOM id is never used as its conversion factor - id and Equivalent are
 *      separate fields everywhere.
 *   2. A missing or non-positive Equivalent is never replaced with 1 or 0. The
 *      derived field is blanked, the UOM is named, and Save is refused.
 *   3. Nothing here is authoritative - the server recomputes and re-validates.
 *
 * Two defects in the desktop/procedure layer that this screen deliberately does
 * NOT copy, both recorded in the contract document:
 *   - RateUomEquivalent returned by the read procedures is joined on packUomId,
 *     so it is the PACK factor. The rate factor is resolved here from rateUomId.
 *   - DeleteDetailRow() collects removed sub-party rows and never sends them, so
 *     deleting a saved row does nothing on the desktop. They are sent here with
 *     actionTypeId = 3, which the procedure fully supports.
 * =========================================================================== */

'use strict';

var API = '/api/commission/buyer-inquiry-booking';
var LOOKUP = '/api/commission/dropdowns';

/* Every dropdown on this screen, its endpoint, and the desktop binding it comes
 * from. Nothing on this screen is hard-coded - an endpoint that returns nothing
 * leaves its dropdown empty and is reported by URL. */
var LOOKUPS = [
    { key: 'companies',        url: LOOKUP + '/companies',         el: 'cmbCompanyName',      value: 'Id', text: 'CompName',                 desktop: 'CommonServices.CompanyServiceBind (:579)' },
    { key: 'commissionAgents', url: LOOKUP + '/commission-agents', el: 'cmbCommissionAgent',  value: 'Id', text: 'CompanyName',              extra: ['NickName'], desktop: 'CommonBindings.SupplierBind "Commission Agent" (:653)' },
    { key: 'buyers',           url: LOOKUP + '/buyers',            el: 'cmbBuyerName',        value: 'Id', text: 'CompanyName',              extra: ['NickName'], desktop: 'CommonBindings.SupplierBind "Buyer Name" (:654)' },
    { key: 'deliveryParties',  url: LOOKUP + '/buyers',            el: 'cmbDeliveryToParty',  value: 'Id', text: 'CompanyName',              extra: ['NickName'], desktop: 'CommonBindings.SupplierBind "Deliver To Party" (:655)' },
    { key: 'shipToAddresses',  url: LOOKUP + '/ship-to-addresses', el: 'cmbShipToAddress',    value: 'Id', text: 'AddressLine1',             extra: ['SupplierCustomerId', 'CompanyName'], desktop: 'SupplierCustomerShipToAddress.GetAll_Combo (:703)' },
    { key: 'paymentTerms',     url: LOOKUP + '/payment-terms',     el: 'cmbPaymentTerm',      value: 'Id', text: 'TermsDescription',         desktop: 'globalPaymentTerm (:752)' },
    { key: 'deliveryTerms',    url: LOOKUP + '/delivery-terms',    el: 'cmbDeliveryTerm',     value: 'Id', text: 'Description',              desktop: 'globalDeliveryTermList (:790)' },
    { key: 'parentItems',      url: LOOKUP + '/parent-categories', el: 'cmbParentItem',       value: 'Id', text: 'InvParentCateDescription', alsoInto: ['cmbHistParentItem'], desktop: 'ParentCategoryBindFromGlobal (:446)' },
    { key: 'items',            url: LOOKUP + '/items',             el: 'cmbItemName',         value: 'Id', text: 'ItemName',                 extra: ['InventoryParentCategoriesId'], alsoInto: ['cmbHistItem'], desktop: 'ItemdtFillFromGlobal / ItemNameBind (:807)' },
    { key: 'packingTypes',     url: LOOKUP + '/packing-types',     el: 'cmbPackingType',      value: 'Id', text: 'PackTypeDesc',             desktop: 'PackingTypeDtFillFromGlobalAndBind (:444)' },
    { key: 'cropYears',        url: LOOKUP + '/crop-years',        el: 'cmbCropYear',         value: 'Id', text: 'CropYear',                 desktop: 'CropDtFillFromGlobalAndBind (:445)' }
];

/* Analysis groups depend on the parent category (GetAnalysisGroup, :888) and the
 * item's UOM schedule depends on the item (ItemUomFromGlobalBind, :841), so both
 * load on demand rather than up front. */
var ANALYSIS_GROUP_URL = LOOKUP + '/analysis-groups';      /* ?parentCategoryId= */
var GROUP_PARAMS_URL   = LOOKUP + '/analysis-group-parameters'; /* ?analysisGroupId= */
var ITEM_UOM_URL       = LOOKUP + '/item-uoms';            /* ?itemId= -> [{Id,UOMCode,Equivalent,BasePackUom,BaseRateUom}] */
var LAST_ANALYSIS_URL  = API + '/last-analysis';           /* ?parentCategoryId= (GetByParentCategoryId_Last...) */

var lookupData = {};
var missingLookups = [];
var subPartyRows = [];
var removedSubPartyRows = [];
var paramRows = [];
var historyRows = [];
var subSelectedIdx = 0;
var paramSelectedIdx = 0;
var inFlight = {};

/* ------------------------------------------------------------- helpers */

function $(id) { return document.getElementById(id); }
function val(id) { var e = $(id); return e ? e.value : ''; }
function num(id) { var n = parseFloat(val(id)); return isNaN(n) ? 0 : n; }
function intOf(id) { var n = parseInt(val(id), 10); return isNaN(n) ? 0 : n; }
/* Local-calendar yyyy-MM-dd. toISOString() converts to UTC first, which in any
 * timezone east of Greenwich (the site runs at UTC+5) reports the PREVIOUS day
 * for a local-midnight Date, so every computed date would be one day early. */
function ymd(d) {
    if (!d || isNaN(d.getTime())) return '';
    var m = d.getMonth() + 1, day = d.getDate();
    return d.getFullYear() + '-' + (m < 10 ? '0' + m : m) + '-' + (day < 10 ? '0' + day : day);
}

function esc(s) {
    return String(s === undefined || s === null ? '' : s)
        .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
}
function r3(x) {
    if (!isFinite(x)) return 0;
    var s = x < 0 ? -1 : 1;
    return s * Math.round(Math.abs(x) * 1000 + 1e-9) / 1000;
}
function fmt(x) { return r3(x).toFixed(3).replace(/\.?0+$/, '') || '0'; }
function textOf(id) {
    var s = $(id);
    return (s && s.selectedOptions && s.selectedOptions.length && s.value !== '0') ? s.selectedOptions[0].textContent : '';
}

function message(text, isError) {
    var box = $('bibMessage');
    if (!box) return;
    box.textContent = text || '';
    box.className = 'cmagt-status' + (isError ? ' error' : '');
    box.style.display = text ? 'block' : 'none';
    if (text) box.scrollIntoView({ block: 'nearest' });
}

/* A click disables its own button, shows a spinner, blocks a second click of the
 * same action while one is in flight, and restores the button on success or
 * failure alike. */
function withButton(btnId, work) {
    if (inFlight[btnId]) return Promise.resolve();
    var b = $(btnId);
    var wasDisabled = b ? b.disabled : false;
    inFlight[btnId] = true;
    if (b) { b.disabled = true; b.classList.add('btn-busy'); }
    var done = function () {
        inFlight[btnId] = false;
        if (b) { b.classList.remove('btn-busy'); b.disabled = wasDisabled; }
        applyButtonState();
    };
    var result;
    try { result = work(); } catch (e) { done(); throw e; }
    if (result && typeof result.then === 'function') return result.then(function (v) { done(); return v; }, function (e) { done(); throw e; });
    done();
    return Promise.resolve(result);
}

/* Save / Update / Delete availability follows the desktop: Save on a new
 * document, Update and Delete once one is loaded (btnsave.Visible etc., :1328). */
function applyButtonState() {
    var loaded = intOf('inquiryBookingMasterId') > 0;
    /* btnsave / btnUpdate / btnDelete are shown and hidden (Visible), not greyed out */
    [['btnSave', !loaded], ['btnUpdate', loaded], ['btnDelete', loaded]].forEach(function (b) {
        var el = $(b[0]);
        if (!el) return;
        el.style.display = b[1] ? '' : 'none';
        if (!inFlight[b[0]]) el.disabled = !b[1];
    });
}

function getJson(url) {
    return fetch(url, { headers: { 'Accept': 'application/json' } })
        .then(function (r) { return r.ok ? r.json() : Promise.reject(new Error(r.status + ' ' + url)); });
}

/* --------------------------------------------------------- dropdowns */

/* The drop grids rendered by build/js/countx_desktop_combo.js read their extra columns from
   data- attributes on each <option>. The column SET is declared on the <select>
   (data-dtcombo="party4" | "itemCmagt" | "uomCmagt" | …); this just publishes whatever the row
   carries, under the names those families expect.
 *
 * Every key below is a real column of the row the API returns - the commission dropdown endpoints
 * pass the desktop's own column names straight through - so nothing is invented. A row without a
 * given column simply gets no attribute, and the grid renders that cell EMPTY rather than
 * substituting anything. */
function applyComboColumns(opt, row) {
    if (!row) return;
    function pick() {
        for (var i = 0; i < arguments.length; i++) {
            var v = row[arguments[i]];
            if (v !== undefined && v !== null && v !== '') return v;
        }
        return null;
    }
    function put(attr, v) {
        if (v === null || v === undefined) return;
        opt.setAttribute('data-' + attr, String(v));
    }
    /* party4 / party3 - CommonBindings.SupplierBind, Columns[3] (GlAccountId) hidden */
    put('code',   pick('PartyCode', 'partyCode'));
    put('city',   pick('CityName', 'cityName'));
    put('mobile', pick('MobileNo', 'mobileNo', 'MobilePersonal'));
    /* itemCmagt - ItemNameBind, Columns[3] and [4] hidden */
    put('item-code',     pick('ItemCode', 'itemCode'));
    put('item-category', pick('ItemCategory', 'itemCategory', 'InvParentCateDescription'));
    /* uomCmagt - ItemUomFromGlobalBind hides nothing, so Equivalent and both base flags show.
       Equivalent is published EXACTLY as stored and is never defaulted to 1. */
    var eq = pick('Equivalent', 'equivalent');
    if (eq !== null) put('eq', eq);
    var br = row.BaseRateUom !== undefined ? row.BaseRateUom : row.baseRateUom;
    var bp = row.BasePackUom !== undefined ? row.BasePackUom : row.basePackUom;
    if (br !== undefined && br !== null) put('base', br === true ? 1 : br === false ? 0 : br);
    if (bp !== undefined && bp !== null) put('base-pack', bp === true ? 1 : bp === false ? 0 : bp);

    /* uomCmagt5 / shipTo2 / analysisGroup3 / taxCmagt (countx_desktop_combo.js) */
    var bs = row.BaseSecondaryUom !== undefined ? row.BaseSecondaryUom : row.baseSecondaryUom;
    if (bs !== undefined && bs !== null) put('base-secondary', bs === true ? 1 : bs === false ? 0 : bs);
    if (row.AddressLine1 !== undefined) put('party-name', pick('CompanyName', 'PartyName'));
    if (row.AnalysisGroupDescription !== undefined) {
        put('group-type', pick('GroupType'));
        put('parent-category', pick('InvParentCateDescription'));
    }
    if (row.TaxNameId !== undefined) {
        put('tax-schedule-id', pick('TaxScheduleId'));
        var ed = pick('EffectedDate');
        if (ed !== null) put('effected-date', typeof ed === 'number' ? localYmd(new Date(ed)) : String(ed).slice(0, 10));
        put('tax-percent', pick('TaxPercent'));
        put('tax-gl-account', pick('TaxGLAccountId'));
    }
    function localYmd(d) { return d.getFullYear() + '-' + ('0' + (d.getMonth() + 1)).slice(-2) + '-' + ('0' + d.getDate()).slice(-2); }
}

/* Refreshes a select2 caption WITHOUT running the element's inline onchange.
   jQuery's trigger('change.select2') still calls elem.onchange (the namespace only filters
   jQuery-bound handlers), so refilling Item Name ran bibItemChanged, which set Parent Item and
   ran bibParentItemChanged, which refilled Item Name ... -> "Maximum call stack size exceeded".
   Every caller here sets a value programmatically and calls any follow-up handler itself. */
function bibRefreshSelect2(sel) {
    if (!sel || !(window.jQuery && jQuery.fn.select2)) return;
    var h = sel.onchange;
    sel.onchange = null;
    try { jQuery(sel).trigger('change.select2'); } finally { sel.onchange = h; }
}

function fillSelect(elId, rows, valueKey, textKey) {
    var sel = $(elId);
    if (!sel) return;
    var keep = sel.value;
    sel.innerHTML = '<option value="0">-- Select --</option>';
    (rows || []).forEach(function (r) {
        var o = document.createElement('option');
        o.value = r[valueKey];
        o.textContent = r[textKey];
        applyComboColumns(o, r);        /* the extra grid columns, see below */
        sel.appendChild(o);
    });
    /* a saved selection is restored even when it is not in the first page of
       results, because the whole list is in the DOM */
    if (keep) sel.value = keep;
    if (sel.classList.contains('searchable')) bibRefreshSelect2(sel);
}

function makeSearchable() {
    if (!(window.jQuery && jQuery.fn.select2)) return;
    jQuery('select.searchable').each(function () {
        /* never put select2 on a select the desktop-combo already replaced: select2 then draws a
           second, hidden-select-sized box next to the combo that spills over the next label
           ("Cre"/"Loa" behind "Due Days"/"Delivery Days") */
        if (this.__dtcombo || this.classList.contains('dtcombo-native')) return;
        if (!jQuery(this).data('select2')) {
            jQuery(this).select2({ width: 'resolve', dropdownAutoWidth: true });
            /* select2 replaces the control, so the element's own change handler
               still fires - no handler is lost */
        }
    });
}

function loadLookups() {
    missingLookups = [];
    var jobs = LOOKUPS.map(function (L) {
        return getJson(L.url)
            .then(function (rows) {
                if (!rows || !rows.length) { missingLookups.push(L.url); rows = []; }
                lookupData[L.key] = rows;
                if (L.el) fillSelect(L.el, rows, L.value, L.text);
                (L.alsoInto || []).forEach(function (other) { fillSelect(other, rows, L.value, L.text); });
            })
            .catch(function () { missingLookups.push(L.url); lookupData[L.key] = []; });
    });
    return Promise.all(jobs).then(function () {
        fillSelect('cmbHistCommissionAgent', lookupData.commissionAgents, 'Id', 'CompanyName');
        fillSelect('cmbHistBuyer', lookupData.buyers, 'Id', 'CompanyName');
        makeSearchable();
        message(missingLookups.length
            ? 'These lookups returned no rows, so their dropdowns are empty: ' + missingLookups.join(', ')
              + '. No values are invented for them.'
            : '', missingLookups.length > 0);
    });
}

/* ---------------------------------------------- item-scoped UOM schedule */

function loadItemUoms(itemId) {
    if (!itemId) {
        lookupData.itemUoms = [];
        fillSelect('cmbPackUom', [], 'Id', 'UOMCode');
        fillSelect('cmbRateUom', [], 'Id', 'UOMCode');
        return Promise.resolve();
    }
    return getJson(ITEM_UOM_URL + '?itemId=' + encodeURIComponent(itemId))
        .then(function (rows) {
            lookupData.itemUoms = rows || [];
            fillSelect('cmbPackUom', rows, 'Id', 'UOMCode');
            fillSelect('cmbRateUom', rows, 'Id', 'UOMCode');
            (rows || []).forEach(function (r) {
                if (r.BasePackUom) { $('cmbPackUom').value = r.Id; }
                if (r.BaseRateUom) { $('cmbRateUom').value = r.Id; }
            });
            makeSearchable();
            bibCalculateWeight();
        })
        .catch(function () {
            lookupData.itemUoms = [];
            fillSelect('cmbPackUom', [], 'Id', 'UOMCode');
            fillSelect('cmbRateUom', [], 'Id', 'UOMCode');
            message('Could not load the UOM schedule for this item (' + ITEM_UOM_URL + '). '
                  + 'Weight and the two amounts cannot be calculated until it loads.', true);
        });
}

/* The conversion factor, by UOM id. Returns null when it cannot be resolved -
   never 0 and never 1. */
function equivalentOf(uomId) {
    if (!uomId) return null;
    var rows = lookupData.itemUoms || [];
    for (var i = 0; i < rows.length; i++) {
        if (parseInt(rows[i].Id, 10) === parseInt(uomId, 10)) {
            var e = parseFloat(rows[i].Equivalent);
            return (isNaN(e) || e <= 0) ? null : e;
        }
    }
    return null;
}

function uomCode(uomId) {
    var rows = lookupData.itemUoms || [];
    for (var i = 0; i < rows.length; i++) {
        if (parseInt(rows[i].Id, 10) === parseInt(uomId, 10)) return rows[i].UOMCode;
    }
    return String(uomId || '');
}

/* ------------------------------------------------- desktop calculations */

/* weight = qty x PackUom.Equivalent   (CalculateWeight, :1792) */
function bibCalculateWeight() {
    var qty = num('txtQty');
    var packId = intOf('cmbPackUom');
    var eq = equivalentOf(packId);
    if (eq === null) {
        if (packId) {
            message('Pack Uom "' + uomCode(packId) + '" has no usable conversion factor (Equivalent). '
                  + 'Weight cannot be calculated and the document cannot be saved.', true);
        }
        $('txtWeight').value = '';
    } else {
        $('txtWeight').value = r3(qty * eq);
    }
    bibCalculateAmounts();
}

/* buyerAmount / SupplierAmount = weight / RateUom.Equivalent x price  (:1591, :1593) */
function bibCalculateAmounts() {
    var weight = num('txtWeight');
    var rateId = intOf('cmbRateUom');
    var eq = equivalentOf(rateId);
    if (eq === null) {
        if (rateId) {
            message('Rate Uom "' + uomCode(rateId) + '" has no usable conversion factor (Equivalent). '
                  + 'The buyer and supplier amounts cannot be calculated.', true);
        }
        bibState.buyerAmount = null;
        bibState.supplierAmount = null;
        return;
    }
    bibState.buyerAmount = (weight > 0) ? r3(weight / eq * num('txtBuyerTargetPrice')) : 0;
    bibState.supplierAmount = (weight > 0) ? r3(weight / eq * num('txtTargetPurchasePrice')) : 0;
}

var bibState = { buyerAmount: 0, supplierAmount: 0 };

/* validityDate = inquiryDate + validityDays  (:1899) */
function bibValidityDaysChanged() {
    var base = val('datInquiryDate');
    if (!base) return;
    var d = new Date(base + 'T00:00:00');
    d.setDate(d.getDate() + intOf('txtValidityDays'));
    $('datValidityUpto').value = ymd(d);
}

/* the reverse: validityDays = validityDate - inquiryDate  (:1912) */
function bibValidityUptoChanged() {
    var a = val('datInquiryDate'), b = val('datValidityUpto');
    if (!a || !b) return;
    var days = Math.round((new Date(b + 'T00:00:00') - new Date(a + 'T00:00:00')) / 86400000);
    $('txtValidityDays').value = days;
}

/* expiry = start + deliveryDays, or = start when days is 0  (:1868) */
function bibCalculateExpiryDate() {
    var start = val('datDeliveryStartDate');
    if (!start) { $('datExpiryDate').value = ''; return; }
    var days = intOf('txtDeliveryDays');
    var d = new Date(start + 'T00:00:00');
    if (days > 0) d.setDate(d.getDate() + days);
    $('datExpiryDate').value = ymd(d);
}

/* a delivery start before the document date is rejected and snapped back (:1848) */
function bibDeliveryStartChanged() {
    var doc = val('datInquiryDate'), start = val('datDeliveryStartDate');
    if (doc && start && start < doc) {
        message("Delivery Start Date Can't Be Less Than Doc Date", true);
        $('datDeliveryStartDate').value = doc;
    }
    bibCalculateExpiryDate();
}

/* term 1 or 3 -> due days 0 and locked; term 2 -> default 2  (:769) */
function bibPaymentTermChanged() {
    var t = intOf('cmbPaymentTerm');
    var due = $('txtDueDays');
    due.disabled = false;
    if (t === 1 || t === 3) { due.value = 0; due.disabled = true; }
    else if (t === 2 && num('txtDueDays') === 0) { due.value = 2; }
}

/* ----------------------------------------------------- party behaviour */

function bibPartyNameModeChanged() {
    var mode = (document.querySelector('input[name="partyNameMode"]:checked') || {}).value || 'business';
    var field = (mode === 'nick') ? 'NickName' : 'CompanyName';
    [['cmbCommissionAgent', 'commissionAgents'], ['cmbBuyerName', 'buyers'], ['cmbDeliveryToParty', 'deliveryParties']]
        .forEach(function (pair) {
            var sel = $(pair[0]); if (!sel) return;
            var keep = sel.value;
            var rows = (lookupData[pair[1]] || []).map(function (r) {
                return { Id: r.Id, Label: r[field] || r.CompanyName };
            });
            fillSelect(pair[0], rows, 'Id', 'Label');
            sel.value = keep;
            bibRefreshSelect2(sel);
        });
}

/* ship-to list filtered by the chosen party; clearing the party clears the
   address, exactly as CmbDeliveryToParty_Leave does (:664) */
function bibDeliveryPartyChanged() {
    var partyId = intOf('cmbDeliveryToParty');
    var rows = (lookupData.shipToAddresses || []).filter(function (r) {
        return !partyId || parseInt(r.SupplierCustomerId, 10) === partyId;
    });
    fillSelect('cmbShipToAddress', rows, 'Id', 'AddressLine1');
    makeSearchable();
    if (!partyId) { $('cmbShipToAddress').value = 0; $('txtShipToAddressText').value = ''; }
    /* BindAndRetainSelection(..., selectSingle: dt.Rows.Count == 1) (:740) */
    else if (rows.length === 1) { $('cmbShipToAddress').value = rows[0].Id; bibShipToAddressChanged(); }
}

/* picking an address back-fills its party (:681) */
function bibShipToAddressChanged() {
    var id = intOf('cmbShipToAddress');
    var rows = lookupData.shipToAddresses || [];
    for (var i = 0; i < rows.length; i++) {
        if (parseInt(rows[i].Id, 10) === id) {
            if (rows[i].SupplierCustomerId) {
                $('cmbDeliveryToParty').value = rows[i].SupplierCustomerId;
                bibRefreshSelect2($('cmbDeliveryToParty'));
            }
            $('txtShipToAddressText').value = rows[i].AddressLine1 || '';
            break;
        }
    }
}

/* ------------------------------------------------------ item behaviour */

function bibParentItemChanged() {
    var pid = intOf('cmbParentItem');
    var rows = (lookupData.items || []).filter(function (r) {
        return !pid || parseInt(r.InventoryParentCategoriesId, 10) === pid;
    });
    fillSelect('cmbItemName', rows, 'Id', 'ItemName');
    makeSearchable();
    loadItemUoms(0);
    loadAnalysisGroups(pid).then(function () { return loadLastAnalysis(pid, true); });
}

/* item -> parent category, analysis group, last analysis, UOMs  (:825) */
function bibItemChanged() {
    var itemId = intOf('cmbItemName');
    var rows = lookupData.items || [];
    var parentId = 0;
    for (var i = 0; i < rows.length; i++) {
        if (parseInt(rows[i].Id, 10) === itemId) { parentId = parseInt(rows[i].InventoryParentCategoriesId, 10) || 0; break; }
    }
    if (parentId) {
        $('cmbParentItem').value = parentId;
        bibRefreshSelect2($('cmbParentItem'));
        loadAnalysisGroups(parentId).then(function () { return loadLastAnalysis(parentId, true); });
    }
    loadItemUoms(itemId);
}

function loadAnalysisGroups(parentCategoryId) {
    if (!parentCategoryId) { fillSelect('cmbAnalysisGroup', [], 'Id', 'AnalysisGroupDescription'); return Promise.resolve(); }
    return getJson(ANALYSIS_GROUP_URL + '?parentCategoryId=' + encodeURIComponent(parentCategoryId))
        .then(function (rows) {
            lookupData.analysisGroups = rows || [];
            fillSelect('cmbAnalysisGroup', rows, 'Id', 'AnalysisGroupDescription');
            makeSearchable();
        })
        .catch(function () { message('Could not load analysis groups (' + ANALYSIS_GROUP_URL + ').', true); });
}

/* BindLastAnalysisByParentItem (:1019): when the parent item has a previous
   inquiry, its analysis group and ranges are offered - and the desktop asks
   first when the grid already holds parameters. */
function loadLastAnalysis(parentCategoryId, ask) {
    if (!parentCategoryId) return Promise.resolve();
    return getJson(LAST_ANALYSIS_URL + '?parentCategoryId=' + encodeURIComponent(parentCategoryId))
        .then(function (rows) {
            if (!rows || !rows.length) return bibAnalysisGroupChanged();
            var groupId = rows[0].analysisGroupId || rows[0].AnalysisGroupId;
            if (ask && intOf('cmbAnalysisGroup') !== parseInt(groupId, 10) && paramRows.length
                && !confirm('Do you want to fetch last analysis for selected parent item?')) {
                return;
            }
            $('cmbAnalysisGroup').value = groupId;
            bibRefreshSelect2($('cmbAnalysisGroup'));
            return bibAnalysisGroupChanged().then(function () {
                var byId = {};
                paramRows.forEach(function (p) { byId[p.qualityParameterId] = p; });
                rows.forEach(function (r) {
                    var p = byId[r.qualityParameterId || r.QualityParameterId];
                    if (p) {
                        p.rangeFrom = parseFloat(r.rangeFrom !== undefined ? r.rangeFrom : r.RangeFrom) || 0;
                        p.rangeTo = parseFloat(r.rangeTo !== undefined ? r.rangeTo : r.RangeTo) || 0;
                    }
                });
                renderParams();
            });
        })
        .catch(function () { /* no previous analysis is not an error */ });
}

/* BindQualityParamsAgainstAnalysisGroup (:932)
 *
 * The desktop clears ParamList only on a NEW document (RecId == 0) or when the
 * selected group differs from the one the LOADED record was saved under
 * (CmbAnalysisGroup.Tag, :948-954). Otherwise it APPENDS the group's standard
 * parameters to the rows already in the list and never removes one
 * (existingIds.Add, :966-980). Rebuilding the list from the endpoint instead
 * would silently drop a saved specification whose parameter has since left the
 * group standards, and Update would then delete that row from the database. */
function bibAnalysisGroupChanged() {
    var groupId = intOf('cmbAnalysisGroup');
    var recId = intOf('inquiryBookingMasterId');
    var loadedGroup = parseInt($('cmbAnalysisGroup').getAttribute('data-loaded-group') || '0', 10) || 0;

    if (recId === 0 || loadedGroup !== groupId) paramRows = [];

    if (!groupId) { renderParams(); return Promise.resolve(); }

    return getJson(GROUP_PARAMS_URL + '?analysisGroupId=' + encodeURIComponent(groupId))
        .then(function (rows) {
            var seen = {};
            paramRows.forEach(function (p) { seen[p.qualityParameterId] = true; });
            (rows || []).forEach(function (r) {
                var id = r.InvLabAnalysisItemsId || r.qualityParameterId || r.Id;
                if (!id || seen[id]) return;          /* existingIds.Add, :970 */
                seen[id] = true;
                paramRows.push({
                    inquiryBookingQualitySpecificationId: 0,
                    qualityParameterId: id,
                    QualityParameter: r.AnalysisParameterDescription || r.QualityParameter || '',
                    rangeFrom: 0,
                    rangeTo: 0
                });
            });
            renderParams();
        })
        .catch(function () { message('Could not load the analysis group parameters (' + GROUP_PARAMS_URL + ').', true); });
}

function renderParams() {
    var body = $('grdParametersBody');
    body.innerHTML = '';
    if (!paramRows.length) {
        body.innerHTML = '<tr><td colspan="3">No quality parameters for this analysis group.</td></tr>';
    } else {
        paramRows.forEach(function (p, i) {
            var tr = document.createElement('tr');
            if (i === paramSelectedIdx) tr.className = 'selected';
            tr.onclick = function () { paramSelectedIdx = i; renderParams(); };
            /* only the two range cells are editable, ditto grdParamsSetting (:1053) */
            tr.innerHTML =
                '<td>' + esc(p.QualityParameter) + '</td>' +
                '<td><input type="number" step="0.01" value="' + (p.rangeFrom || 0) + '" onclick="event.stopPropagation()" oninput="paramRows[' + i + '].rangeFrom=parseFloat(this.value)||0"></td>' +
                '<td><input type="number" step="0.01" value="' + (p.rangeTo || 0) + '" onclick="event.stopPropagation()" oninput="paramRows[' + i + '].rangeTo=parseFloat(this.value)||0"></td>';
            body.appendChild(tr);
        });
    }
    if (paramSelectedIdx >= paramRows.length) paramSelectedIdx = Math.max(0, paramRows.length - 1);
    $('paramNavTotal').textContent = paramRows.length;
    $('paramNavCurrent').value = paramRows.length ? paramSelectedIdx + 1 : 0;
}

function bibParamNav(what, v) {
    if (!paramRows.length) return;
    if (what === 'first') paramSelectedIdx = 0;
    else if (what === 'prev') paramSelectedIdx = Math.max(0, paramSelectedIdx - 1);
    else if (what === 'next') paramSelectedIdx = Math.min(paramRows.length - 1, paramSelectedIdx + 1);
    else if (what === 'last') paramSelectedIdx = paramRows.length - 1;
    else if (what === 'goto') {
        var i = parseInt(v, 10) - 1;
        if (!isNaN(i) && i >= 0 && i < paramRows.length) paramSelectedIdx = i;
    }
    renderParams();
}

/* ------------------------------------------------------- sub parties */

/* grdSubParty is always on the desktop form (docked Fill under panel5 "Sub Parties");
   BindGrids() seeds one empty row when the table is empty (AddRowsInSubPartyeGrid, :1086). */
function bibToggleSubParties() {
    if ($('subPartiesBlock')) $('subPartiesBlock').style.display = '';
    if (!subPartyRows.length) bibAddSubParty();
}

function bibAddSubParty() {
    subPartyRows.push({ inquiryBookingPartyDetailId: 0, SubPartyId: 0, subPartyName: '', itemQty: 0, Rate: 0, Amount: 0, remarks: '', selected: false });
    renderSubParties();
}

/* a saved row is kept and sent with actionTypeId = 3; an unsaved one is dropped
   (DeleteDetailRow, :1171 - and see the header note on the desktop's own defect) */
function bibRemoveSubParty(i) {
    var r = subPartyRows[i];
    if (!r) return;
    if (r.inquiryBookingPartyDetailId && !confirm('Are you sure to delete?')) return;
    if (r.inquiryBookingPartyDetailId) {
        r.actionTypeId = 3;
        removedSubPartyRows.push(r);
    }
    subPartyRows.splice(i, 1);
    if (!subPartyRows.length) bibAddSubParty(); else renderSubParties();
}

/* Amount = ItemQty x Rate  (UpdateAmount, :1230) */
function bibSubCellChanged(i, field, value) {
    var r = subPartyRows[i];
    if (!r) return;
    if (field === 'SubPartyId') {
        r.SubPartyId = parseInt(value, 10) || 0;
        var rows = lookupData.buyers || [];
        r.subPartyName = '';
        for (var k = 0; k < rows.length; k++) {
            if (parseInt(rows[k].Id, 10) === r.SubPartyId) { r.subPartyName = rows[k].CompanyName; break; }
        }
    } else if (field === 'remarks') {
        r.remarks = value;
        return;
    } else {
        r[field] = parseFloat(value) || 0;
        r.Amount = r3((+r.itemQty || 0) * (+r.Rate || 0));
    }
    renderSubParties();
}

function bibToggleAllSubParties(on) {
    subPartyRows.forEach(function (r) { r.selected = !!on; });
    renderSubParties();
}

function renderSubParties() {
    var body = $('grdSubPartyBody');
    body.innerHTML = '';
    var parties = lookupData.buyers || [];
    if (!subPartyRows.length) {
        body.innerHTML = '<tr><td colspan="7">No sub parties.</td></tr>';
    } else {
        subPartyRows.forEach(function (r, i) {
            var opts = '<option value="0">-- Select --</option>' + parties.map(function (p) {
                return '<option value="' + p.Id + '"' + (parseInt(p.Id, 10) === parseInt(r.SubPartyId, 10) ? ' selected' : '') + '>' + esc(p.CompanyName) + '</option>';
            }).join('');
            var tr = document.createElement('tr');
            if (i === subSelectedIdx) tr.className = 'selected';
            tr.onclick = function () { subSelectedIdx = i; renderSubParties(); };
            tr.innerHTML =
                '<td><button type="button" class="danger" onclick="event.stopPropagation();bibRemoveSubParty(' + i + ')">X</button></td>' +
                '<td><button type="button" onclick="event.stopPropagation();bibAddSubParty()">+</button></td>' +
                '<td><select onclick="event.stopPropagation()" onchange="bibSubCellChanged(' + i + ',\'SubPartyId\',this.value)">' + opts + '</select></td>' +
                '<td><input type="number" step="0.001" value="' + (r.itemQty || 0) + '" onclick="event.stopPropagation()" oninput="bibSubCellChanged(' + i + ',\'itemQty\',this.value)"></td>' +
                '<td><input type="number" step="0.001" value="' + (r.Rate || 0) + '" onclick="event.stopPropagation()" oninput="bibSubCellChanged(' + i + ',\'Rate\',this.value)"></td>' +
                '<td style="text-align:right;">' + fmt(r.Amount) + '</td>' +
                '<td><input type="text" value="' + esc(r.remarks) + '" onclick="event.stopPropagation()" oninput="bibSubCellChanged(' + i + ',\'remarks\',this.value)"></td>';
            body.appendChild(tr);
        });
    }
    var totals = subPartyRows.reduce(function (t, r) {
        t.qty += +r.itemQty || 0; t.amount += +r.Amount || 0; return t;
    }, { qty: 0, amount: 0 });
    $('totSubQty').textContent = fmt(totals.qty);
    $('totSubAmount').textContent = fmt(totals.amount);

    if (subSelectedIdx >= subPartyRows.length) subSelectedIdx = Math.max(0, subPartyRows.length - 1);
    $('subNavTotal').textContent = subPartyRows.length;
    $('subNavCurrent').value = subPartyRows.length ? subSelectedIdx + 1 : 0;
}

function bibSubNav(what, v) {
    if (!subPartyRows.length) return;
    if (what === 'first') subSelectedIdx = 0;
    else if (what === 'prev') subSelectedIdx = Math.max(0, subSelectedIdx - 1);
    else if (what === 'next') subSelectedIdx = Math.min(subPartyRows.length - 1, subSelectedIdx + 1);
    else if (what === 'last') subSelectedIdx = subPartyRows.length - 1;
    else if (what === 'goto') {
        var i = parseInt(v, 10) - 1;
        if (!isNaN(i) && i >= 0 && i < subPartyRows.length) subSelectedIdx = i;
    }
    renderSubParties();
}

/* Ctrl+D adds a row, Ctrl+Delete removes the current one  (grdSubParty_KeyDown, :1237) */
document.addEventListener('keydown', function (e) {
    if (!e.ctrlKey) return;
    if (!$('subPartiesBlock') || $('subPartiesBlock').style.display === 'none') return;
    if (e.key === 'd' || e.key === 'D') { e.preventDefault(); bibAddSubParty(); }
    else if (e.key === 'Delete' && !e.shiftKey) { e.preventDefault(); bibRemoveSubParty(subSelectedIdx); }
});

/* ------------------------------------------------------- validation */

/* formvalidation() (:1379), in the desktop's order and wording, followed by the
   quality-range rules from Insert() (:1547). */
function bibValidate() {
    if (intOf('txtValidityDays') === 0) return 'Validity Days field is required';
    if (!intOf('cmbCommissionAgent')) return 'Please Select Commission Agent / Broker';
    if (!intOf('cmbBuyerName')) return 'Please Select Buyer';
    if (!intOf('cmbDeliveryToParty')) return 'Please Select DeliveryToParty';
    if (!intOf('cmbPaymentTerm')) return 'Please Select Paymrnt Term';
    if (intOf('cmbPaymentTerm') === 2 && intOf('txtDueDays') === 0) return 'Due Days field is required';
    if (!intOf('cmbDeliveryTerm')) return 'Please Select Delivery Term';
    if (intOf('txtDeliveryDays') === 0) return 'Delivery Days field is required';
    if (!intOf('cmbParentItem')) return 'Parent Item Field Required';
    if (!intOf('cmbItemName')) return 'Item Field Required';
    if (num('txtQty') === 0) return 'Qty Field Required';
    if (num('txtWeight') === 0) return 'Weight Field Required';
    if (!intOf('cmbPackingType')) return 'Packing Type Field is Required';
    if (!intOf('cmbPackUom')) return 'Pack Uom Field is Required';
    if (!intOf('cmbRateUom')) return 'Rate Uom Field is Required';
    if (!intOf('cmbCropYear')) return 'Please Select Crop Year';
    if (num('txtBuyerTargetPrice') === 0) return 'Buyer Target Price Field Required';
    if (num('txtTargetPurchasePrice') === 0) return 'Target Purchase Price Field Required';

    if (equivalentOf(intOf('cmbPackUom')) === null) return 'Pack Uom "' + uomCode(intOf('cmbPackUom')) + '" has no usable Equivalent.';
    if (equivalentOf(intOf('cmbRateUom')) === null) return 'Rate Uom "' + uomCode(intOf('cmbRateUom')) + '" has no usable Equivalent.';

    for (var i = 0; i < paramRows.length; i++) {
        var p = paramRows[i];
        var fromEntered = (+p.rangeFrom || 0) !== 0;
        var toEntered = (+p.rangeTo || 0) !== 0;
        if (fromEntered !== toEntered) return 'Please enter both Range From and Range To for parameter: ' + p.QualityParameter;
        if (fromEntered && toEntered && (+p.rangeFrom) > (+p.rangeTo)) return 'Range From cannot be greater than Range To for parameter: ' + p.QualityParameter;
        if ((+p.rangeFrom) < 0 || (+p.rangeTo) < 0) return 'Negative values are not allowed for parameter: ' + p.QualityParameter;
    }
    /* Insert() :1608-1615 - a sub-party row that is used (party chosen or Amount > 0) must have
       both; FormHelper.ValidateField wording "<field> is required in Detail Grid at row No: n" */
    for (var j = 0; j < subPartyRows.length; j++) {
        var sp = subPartyRows[j];
        if ((parseInt(sp.SubPartyId, 10) || 0) > 0 || (+sp.Amount || 0) > 0) {
            if (!(parseInt(sp.SubPartyId, 10) > 0)) return 'Sub Party Name is required in Detail Grid at row No: ' + (j + 1);
            if (!((+sp.Amount || 0) > 0)) return 'amount is required in Detail Grid at row No: ' + (j + 1);
        }
    }
    return null;
}

/* ------------------------------------------------------- persistence */

function validParamRows() {
    return paramRows.filter(function (p) { return (+p.rangeFrom || 0) > 0 && (+p.rangeTo || 0) > 0; });
}

function buildPayload() {
    var recId = intOf('inquiryBookingMasterId');
    var valid = validParamRows();

    /* one detail line - the desktop builds exactly one (:1577-1595) */
    var detailId = intOf('detailId');
    var detail = {
        inquiryBookingDetailId: detailId,
        actionTypeId: detailId > 0 ? 2 : 1,
        inventoryParentCategoryId: intOf('cmbParentItem'),
        itemId: intOf('cmbItemName'),
        itemQty: num('txtQty'),
        itemWeight: num('txtWeight'),
        packUomId: intOf('cmbPackUom'),
        packUomEquivalent: equivalentOf(intOf('cmbPackUom')),
        packingTypeId: intOf('cmbPackingType'),
        cropYearId: intOf('cmbCropYear'),
        cropYear: textOf('cmbCropYear'),
        rateUomId: intOf('cmbRateUom'),
        rateUomEquivalent: equivalentOf(intOf('cmbRateUom')),
        buyerRate: num('txtBuyerTargetPrice'),
        buyerAmount: bibState.buyerAmount,
        supplierRate: num('txtTargetPurchasePrice'),
        SupplierAmount: bibState.supplierAmount,
        QualitySpecifications: val('txtQualitySpecifications')
    };

    /* one payment schedule row from the header term, pctOfTotal 100 (:1596-1604) */
    var dueDays = intOf('txtDueDays');
    var dueBase = '';
    if (val('datInquiryDate')) {
        var d = new Date(val('datInquiryDate') + 'T00:00:00');
        d.setDate(d.getDate() + dueDays);
        dueBase = ymd(d);
    }

    var parties = subPartyRows
        .filter(function (r) { return (+r.SubPartyId || 0) > 0 || (+r.Amount || 0) > 0; })
        .map(function (r) {
            return {
                inquiryBookingPartyDetailId: r.inquiryBookingPartyDetailId || 0,
                actionTypeId: (r.inquiryBookingPartyDetailId || 0) > 0 ? 2 : 1,
                SubPartyId: r.SubPartyId,
                itemQty: r.itemQty,
                Rate: r.Rate,
                Amount: r.Amount,
                remarks: r.remarks
            };
        })
        .concat(removedSubPartyRows);

    return {
        inquiryBookingMasterId: recId,
        documentTypeId: 1050,
        inquiryBookingNo: intOf('txtInquiryNo'),
        inquiryBookingDate: val('datInquiryDate'),
        validityDays: intOf('txtValidityDays'),
        validityDate: val('datValidityUpto'),
        expiryDate: val('datExpiryDate'),
        inquiryStatusId: 1,
        commissionAgentId: intOf('cmbCommissionAgent'),
        buyerId: intOf('cmbBuyerName'),
        DeliveryToPartyId: intOf('cmbDeliveryToParty'),
        shipToAddressId: intOf('cmbShipToAddress'),
        ShipToAddress: val('txtShipToAddressText') || textOf('cmbShipToAddress'),
        deliveryTermId: intOf('cmbDeliveryTerm'),
        deliveryDays: intOf('txtDeliveryDays'),
        deliveryStartDate: val('datDeliveryStartDate'),
        remarksHeader: val('txtRemarks'),
        companyId: intOf('cmbCompanyName'),
        isApproved: false,
        analysisGroupId: valid.length ? intOf('cmbAnalysisGroup') : 0,
        QualitySpecifications: valid.map(function (p) {
            return '[' + p.QualityParameter + ', ' + p.rangeFrom + ',' + p.rangeTo + ']';
        }).join(', '),

        InquiryBookingDetailList: [detail],
        InquiryBookingPaymentScheduleList: [{
            inquiryBookingPaymentScheduleId: 0,
            paymentTermId: intOf('cmbPaymentTerm'),
            dueDays: dueDays,
            dueBaseDate: dueBase,
            pctOfTotal: 100,
            dueAmount: bibState.buyerAmount
        }],
        InquiryBookingQualitySpecificationList: valid.map(function (p) {
            return {
                inquiryBookingQualitySpecificationId: p.inquiryBookingQualitySpecificationId || 0,
                qualityParameterId: p.qualityParameterId,
                rangeFrom: p.rangeFrom,
                rangeTo: p.rangeTo
            };
        }),
        InquiryBookingPartyDetailList: parties
    };
}

function bibSave() {
    return withButton(intOf('inquiryBookingMasterId') > 0 ? 'btnUpdate' : 'btnSave', function () {
        message('');
        var err = bibValidate();
        if (err) { message(err, true); return; }

        var isUpdate = intOf('inquiryBookingMasterId') > 0;
        if (!confirm(isUpdate ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;

        return fetch(API + '/save', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(buildPayload())
        })
            .then(function (r) { return r.json().catch(function () { return { success: r.ok }; }); })
            .then(function (data) {
                if (data && data.success) {
                    message(isUpdate ? 'Update Successfully' : 'Save Successfully');
                    removedSubPartyRows = [];
                    var id = data.id || intOf('inquiryBookingMasterId');
                    /* desktop Insert (:1621-1641): message, Reset(), then
                       new doc + ChkLink -> open frmSubBuyerInquiryBooking for it,
                       otherwise Preview -> print slip. The separate sub-party form is
                       not ported; the web opens the saved inquiry with the sub-party
                       grid switched on instead. */
                    /* new doc + ChkLink: Reset() then openForm(success) -> frmSubBuyerInquiryBooking */
                    if (!isUpdate && $('chkLink') && $('chkLink').checked && id) {
                        bibNew();
                        bibAddSubPartyTo(id, true);
                        return;
                    }
                    bibNew();
                    if ($('chkPrint').checked && id) bibPrintId(id);
                    return;
                }
                message((data && (data.message || data.error)) || 'The server rejected the save.', true);
            })
            .catch(function (e) { message('Save failed: ' + e.message, true); });
    });
}

function bibLoad(id) {
    if (!id) return Promise.resolve();
    return getJson(API + '/' + id)
        .then(function (o) {
            /* The service answers {success, header, details, paymentSchedule, qualitySpecs,
               partyDetails}. Reading the envelope as if it were the header left every field
               blank on Edit, so Update then posted an empty document. Unwrap it into the
               desktop model's shape (ReadById + the four ReadByHeaderId_* lists). */
            if (o && o.success === false) { message(o.message || 'Record Not Found', true); return; }
            if (o && o.header) {
                var h = o.header;
                h.InquiryBookingDetailList = o.details || [];
                h.InquiryBookingPaymentScheduleList = o.paymentSchedule || [];
                h.InquiryBookingQualitySpecificationList = o.qualitySpecs || [];
                h.InquiryBookingPartyDetailList = o.partyDetails || [];
                o = h;
            }
            if (!o) { message('Record Not Found', true); return; }
            bibResetFields();
            $('inquiryBookingMasterId').value = o.inquiryBookingMasterId || id;
            $('txtInquiryNo').value = o.inquiryBookingNo || '';
            $('datInquiryDate').value = (o.inquiryBookingDate || '').slice(0, 10);
            $('txtValidityDays').value = o.validityDays || 0;
            $('datValidityUpto').value = (o.validityDate || '').slice(0, 10);
            $('cmbCompanyName').value = o.companyId || 0;
            $('cmbCommissionAgent').value = o.commissionAgentId || 0;
            $('cmbBuyerName').value = o.buyerId || 0;
            $('cmbDeliveryToParty').value = o.DeliveryToPartyId || o.deliveryToPartyId || 0;
            bibDeliveryPartyChanged();
            if (o.shipToAddressId) $('cmbShipToAddress').value = o.shipToAddressId;
            $('txtShipToAddressText').value = o.ShipToAddress || o.shipToAddress || '';
            $('cmbDeliveryTerm').value = o.deliveryTermId || 0;
            $('txtDeliveryDays').value = o.deliveryDays || 0;
            $('datDeliveryStartDate').value = (o.deliveryStartDate || '').slice(0, 10);
            $('datExpiryDate').value = (o.expiryDate || '').slice(0, 10);
            $('txtRemarks').value = o.remarksHeader || '';

            var sched = (o.InquiryBookingPaymentScheduleList || o.inquiryBookingPaymentScheduleList || [])[0];
            if (sched) {
                $('cmbPaymentTerm').value = sched.paymentTermId || 0;
                $('txtDueDays').value = sched.dueDays || 0;
                bibPaymentTermChanged();
            }

            var details = o.InquiryBookingDetailList || o.inquiryBookingDetailList || [];
            var chain = Promise.resolve();
            if (details.length) {
                var d = details[0];
                $('detailId').value = d.inquiryBookingDetailId || 0;
                $('cmbParentItem').value = d.inventoryParentCategoryId || 0;
                chain = loadAnalysisGroups(d.inventoryParentCategoryId).then(function () {
                    var rows = (lookupData.items || []).filter(function (r) {
                        return parseInt(r.InventoryParentCategoriesId, 10) === parseInt(d.inventoryParentCategoryId, 10);
                    });
                    fillSelect('cmbItemName', rows, 'Id', 'ItemName');
                    $('cmbItemName').value = d.itemId || 0;
                    return loadItemUoms(d.itemId);
                }).then(function () {
                    $('txtQty').value = d.itemQty || 0;
                    $('txtWeight').value = d.itemWeight || 0;
                    $('cmbPackingType').value = d.packingTypeId || 0;
                    $('cmbCropYear').value = d.cropYearId || 0;
                    $('cmbPackUom').value = d.packUomId || 0;
                    $('cmbRateUom').value = d.rateUomId || 0;
                    $('txtQualitySpecifications').value = d.QualitySpecifications || d.qualitySpecifications || '';
                    $('txtBuyerTargetPrice').value = d.buyerRate || 0;
                    $('txtTargetPurchasePrice').value = d.supplierRate || 0;
                    /* the rate factor is resolved from rateUomId, never from the
                       RateUomEquivalent the read procedure returns - see header */
                    bibCalculateAmounts();
                });
            }

            return chain.then(function () {
                if (o.analysisGroupId) {
                    $('cmbAnalysisGroup').value = o.analysisGroupId;
                    /* CmbAnalysisGroup.Tag on the desktop (:952): the group this record was
                     * saved under. Only a change AWAY from it clears the parameter list. */
                    $('cmbAnalysisGroup').setAttribute('data-loaded-group', String(o.analysisGroupId));
                    return bibAnalysisGroupChanged();
                }
                $('cmbAnalysisGroup').removeAttribute('data-loaded-group');
            }).then(function () {
                var specs = o.InquiryBookingQualitySpecificationList || o.inquiryBookingQualitySpecificationList || [];
                if (specs.length) {
                    var byId = {};
                    paramRows.forEach(function (p) { byId[p.qualityParameterId] = p; });
                    specs.forEach(function (s) {
                        var p = byId[s.qualityParameterId];
                        if (p) {
                            p.inquiryBookingQualitySpecificationId = s.inquiryBookingQualitySpecificationId || 0;
                            p.rangeFrom = s.rangeFrom || 0;
                            p.rangeTo = s.rangeTo || 0;
                        } else {
                            /* The desktop loads ParamList from the saved rows FIRST (:1739) and
                             * only then appends group standards, so a saved parameter that is no
                             * longer a standard of the group stays on screen and is written back.
                             * Dropping it here would delete it from the database on Update. */
                            paramRows.push({
                                inquiryBookingQualitySpecificationId: s.inquiryBookingQualitySpecificationId || 0,
                                qualityParameterId: s.qualityParameterId,
                                QualityParameter: s.QualityParameter || s.qualityParameter || '',
                                rangeFrom: s.rangeFrom || 0,
                                rangeTo: s.rangeTo || 0
                            });
                        }
                    });
                    renderParams();
                }

                subPartyRows = (o.InquiryBookingPartyDetailList || o.inquiryBookingPartyDetailList || []).map(function (r) {
                    return {
                        inquiryBookingPartyDetailId: r.inquiryBookingPartyDetailId || 0,
                        SubPartyId: r.SubPartyId || r.subPartyId || 0,
                        subPartyName: r.SubPartyName || r.subPartyName || '',
                        itemQty: r.itemQty || 0,
                        Rate: r.Rate || r.rate || 0,
                        Amount: r.Amount || r.amount || 0,
                        remarks: r.remarks || '',
                        selected: false
                    };
                });
                removedSubPartyRows = [];
                bibToggleSubParties();
                renderSubParties();

                makeSearchable();
                applyButtonState();
                bibShowTab(null, 'tabForm');
            });
        })
        .catch(function (e) { message('Load failed: ' + e.message, true); });
}

function bibDelete() {
    return withButton('btnDelete', function () {
        var id = intOf('inquiryBookingMasterId');
        if (!id) { message('No record found to Delete', true); return; }
        if (!confirm('Are you sure to Delete?')) return;
        /* the controller maps POST /delete/{id}; DELETE /{id} was answered 405 */
        return fetch(API + '/delete/' + id, { method: 'POST' })
            .then(function (r) { return r.json().catch(function () { return { success: r.ok }; }); })
            .then(function (d) {
                if (d && d.success) { message('Delete Record Successfully'); return bibNew(); }
                message((d && d.message) || 'Delete was refused.', true);
            })
            .catch(function (e) { message('Delete failed: ' + e.message, true); });
    });
}

/* ---------------------------------------------------------- history */

function bibLoadHistory() {
    return withButton('btnShowHistory', function () {
        var mode = (document.querySelector('input[name="histDateMode"]:checked') || {}).value || 'doc';
        var q = [];
        if (val('histFromDate')) q.push((mode === 'entry' ? 'entryFromDate' : mode === 'modify' ? 'modifyFromDate' : 'fromDate') + '=' + val('histFromDate'));
        if (val('histToDate')) q.push((mode === 'entry' ? 'entryToDate' : mode === 'modify' ? 'modifyToDate' : 'toDate') + '=' + val('histToDate'));
        if (val('histValidityFrom')) q.push('validityDateFrom=' + val('histValidityFrom'));
        if (val('histValidityTo')) q.push('validityDateTo=' + val('histValidityTo'));
        if (intOf('cmbHistParentItem')) q.push('parentItemIds=' + intOf('cmbHistParentItem'));
        if (intOf('cmbHistCommissionAgent')) q.push('commissionAgentId=' + intOf('cmbHistCommissionAgent'));
        if (intOf('cmbHistBuyer')) q.push('buyerId=' + intOf('cmbHistBuyer'));
        if (intOf('cmbHistItem')) q.push('itemId=' + intOf('cmbHistItem'));
        if (num('txtRateFrom')) q.push('buyerRateFrom=' + num('txtRateFrom'));
        if (num('txtRateTo')) q.push('buyerRateTo=' + num('txtRateTo'));

        return getJson(API + '/history' + (q.length ? '?' + q.join('&') : ''))
            .then(function (rows) { historyRows = rows || []; renderHistory(); })
            .catch(function (e) { message('History failed: ' + e.message, true); });
    });
}

/* The procedure returns these column names exactly; JSON and JavaScript are
   case-sensitive where ADO.NET's DataTable indexer is not, which is what made
   the Sale Order history render blank. Each field is read through both spellings. */
function pick(row) {
    for (var i = 1; i < arguments.length; i++) {
        var k = arguments[i];
        if (row[k] !== undefined && row[k] !== null) return row[k];
    }
    return '';
}

function renderHistory() {
    var body = $('grdHistoryBody');
    body.innerHTML = '';
    if (!historyRows.length) {
        body.innerHTML = '<tr><td colspan="23">No documents match these filters.</td></tr>';
        if ($('totHistQty')) { $('totHistQty').textContent = ''; $('totHistWeight').textContent = ''; }
        bibRenderSubPartyHistory([]);
        $('lblOpenInquiries').textContent = '0';
        $('lblInProcessInquiries').textContent = '0';
        $('lblTotalOutstanding').textContent = '0';
        return;
    }
    var totQty = 0, totWeight = 0;
    historyRows.forEach(function (r, i) {
        var id = pick(r, 'inquiryBookingMasterId', 'InquiryBookingMasterId');
        var qty = parseFloat(pick(r, 'itemQty', 'ItemQty')) || 0, wt = parseFloat(pick(r, 'itemWeight', 'ItemWeight')) || 0;
        totQty += qty; totWeight += wt;
        var tr = document.createElement('tr');
        /* SelectionChanged -> GetDetailGrdByHeadId (:2331); DoubleClick / Ctrl+Enter -> ReadById */
        tr.onclick = function () {
            Array.prototype.forEach.call(document.querySelectorAll('#grdHistoryBody tr.selected'), function (x) { x.classList.remove('selected'); });
            tr.classList.add('selected');
            bibLoadSubPartyHistory(id);
        };
        tr.ondblclick = function () { bibLoad(id); bibShowTab(null, 'tabForm'); };
        /* column order = dt2 (:2162) with the Edit / Print / AddSubParties buttons inserted at
           positions 0, 2 and 3 (GridSettingHistory, :2245) */
        tr.innerHTML =
            '<td><button type="button" onclick="event.stopPropagation();bibLoad(' + id + ');bibShowTab(null,\'tabForm\')">Edit</button></td>' +
            '<td>' + esc(pick(r, 'CommissionAgentName', 'commissionAgentName')) + '</td>' +
            '<td><button type="button" onclick="event.stopPropagation();bibPrintId(' + id + ')">Print</button></td>' +
            '<td><button type="button" onclick="event.stopPropagation();bibAddSubPartyTo(' + id + ')">Add Sub Party</button></td>' +
            '<td>' + esc(pick(r, 'BuyerName', 'buyerName')) + '</td>' +
            '<td>' + esc(pick(r, 'DeliveryToParty', 'deliveryToParty')) + '</td>' +
            '<td>' + esc(pick(r, 'ShipToAddress', 'shipToAddress')) + '</td>' +
            /* the document code is clickable and opens its own record */
            '<td><a href="#" onclick="event.preventDefault();event.stopPropagation();bibLoad(' + id + ');bibShowTab(null,\'tabForm\')">' + esc(pick(r, 'inquiryBookingNo', 'InquiryBookingNo')) + '</a></td>' +
            '<td>' + esc(bibDmy(pick(r, 'inquiryBookingDate', 'InquiryBookingDate'))) + '</td>' +
            '<td>' + esc(bibDmy(pick(r, 'validityDate', 'ValidityDate'))) + '</td>' +
            /* PaymentTermNames is always empty from this procedure - see the contract */
            '<td>' + esc(pick(r, 'PaymentTermNames', 'paymentTermNames')) + '</td>' +
            '<td>' + esc(pick(r, 'DeliveryTerm', 'deliveryTerm')) + '</td>' +
            '<td>' + esc(pick(r, 'ItemName', 'itemName')) + '</td>' +
            '<td class="num">' + fmt(qty) + '</td>' +
            '<td class="num">' + fmt(wt) + '</td>' +
            '<td class="num">' + fmt(pick(r, 'buyerRate', 'BuyerRate')) + '</td>' +
            '<td class="num">' + fmt(pick(r, 'supplierRate', 'SupplierRate')) + '</td>' +
            '<td>' + esc(pick(r, 'QualitySpecifications', 'qualitySpecifications')) + '</td>' +
            '<td>' + esc(pick(r, 'EntryUserName', 'entryUserName')) + '</td>' +
            '<td>' + esc(bibDmyTime(pick(r, 'EntryDate', 'entryDate'))) + '</td>' +
            '<td>' + esc(pick(r, 'ModifyUserName', 'modifyUserName')) + '</td>' +
            '<td>' + esc(bibDmyTime(pick(r, 'ModifyDate', 'modifyDate'))) + '</td>' +
            '<td class="num">' + esc(pick(r, 'NoOfAttachments', 'noOfAttachments') || 0) + '</td>';
        body.appendChild(tr);
    });
    if ($('totHistQty')) $('totHistQty').textContent = fmt(totQty);
    if ($('totHistWeight')) $('totHistWeight').textContent = fmt(totWeight);

    var first = historyRows[0];
    var open = parseFloat(pick(first, 'TotalOpenRecords', 'totalOpenRecords')) || 0;
    var proc = parseFloat(pick(first, 'TotalInProcessRecords', 'totalInProcessRecords')) || 0;
    $('lblOpenInquiries').textContent = open;
    $('lblInProcessInquiries').textContent = proc;
    $('lblTotalOutstanding').textContent = open + proc;
}

function bibToggleAllHistory(on) {
    historyRows.forEach(function (r) { r.selected = !!on; });
    Array.prototype.forEach.call(document.querySelectorAll('#grdHistoryBody input[type=checkbox]'), function (c) { c.checked = !!on; });
}

/* btnNewHistory_Click (:1937): clears Item, Commission Agent, Buyer and the price range only -
   the dates, validity dates and parent item are kept, and the grid is not cleared. */
function bibClearHistoryFilters() {
    ['txtRateFrom', 'txtRateTo'].forEach(function (id) { $(id).value = ''; });
    ['cmbHistCommissionAgent', 'cmbHistBuyer', 'cmbHistItem'].forEach(function (id) {
        $(id).value = 0;
        bibRefreshSelect2($(id));
    });
}

/* BtnRefreshHistory_Click (:1953): reload the history combos; when a parent item is chosen
   the item list is re-narrowed to it (BindDropdownsAgainstParentCategory). */
function bibRefreshHistoryCombos() {
    return withButton('btnRefreshHistory', function () {
        var keep = { p: intOf('cmbHistParentItem'), a: intOf('cmbHistCommissionAgent'), b: intOf('cmbHistBuyer'), i: intOf('cmbHistItem') };
        return loadLookups().then(function () {
            $('cmbHistParentItem').value = keep.p; $('cmbHistCommissionAgent').value = keep.a; $('cmbHistBuyer').value = keep.b;
            if (keep.p) bibHistParentChanged();
            $('cmbHistItem').value = keep.i;
            ['cmbHistParentItem', 'cmbHistCommissionAgent', 'cmbHistBuyer', 'cmbHistItem'].forEach(function (id) { bibRefreshSelect2($(id)); });
        });
    });
}

/* dd-MMM-yyyy and dd-MMM-yy hh:mm tt (GridSettingHistory FormatString, :2232-2235) */
var BIB_MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
function bibParts(v) {
    var m = String(v || '').match(/^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2}))?/);
    return m ? { y: m[1], mo: BIB_MON[parseInt(m[2], 10) - 1], d: m[3], h: m[4] ? parseInt(m[4], 10) : null, mi: m[5] } : null;
}
function bibDmy(v) { var p = bibParts(v); return p ? p.d + '-' + p.mo + '-' + p.y : ''; }
function bibDmyTime(v) {
    var p = bibParts(v);
    if (!p) return '';
    var s = p.d + '-' + p.mo + '-' + p.y.slice(2);
    if (p.h === null) return s;
    var h12 = p.h % 12 || 12;
    return s + ' ' + (h12 < 10 ? '0' + h12 : h12) + ':' + p.mi + ' ' + (p.h < 12 ? 'AM' : 'PM');
}

/* grdSubPartyHistory: GetDetailGrdByHeadId (:2349) - ReadById, then the party list with
   SubPartyName shown in place of the hidden SubPartyId (HistoryDetailGridSetting, :2371). */
var subHistReq = 0;
function bibLoadSubPartyHistory(id) {
    var mine = ++subHistReq;
    if (!id) { bibRenderSubPartyHistory([]); return Promise.resolve(); }
    return getJson(API + '/' + id)
        .then(function (o) {
            if (mine !== subHistReq) return;
            var rows = (o && (o.partyDetails || o.InquiryBookingPartyDetailList || o.inquiryBookingPartyDetailList)) || [];
            bibRenderSubPartyHistory(rows);
        })
        .catch(function (e) { if (mine === subHistReq) { bibRenderSubPartyHistory([]); message('Sub parties could not be read: ' + e.message, true); } });
}
function bibRenderSubPartyHistory(rows) {
    var body = $('grdSubPartyHistoryBody');
    if (!body) return;
    var parties = lookupData.buyers || [], q = 0, a = 0;
    body.innerHTML = rows.map(function (r) {
        var pid = parseInt(r.SubPartyId || r.subPartyId, 10) || 0;
        var name = r.SubPartyName || r.subPartyName || '';
        if (!name) for (var k = 0; k < parties.length; k++) if (parseInt(parties[k].Id, 10) === pid) { name = parties[k].CompanyName; break; }
        var qty = parseFloat(r.itemQty || r.ItemQty) || 0, amt = parseFloat(r.Amount || r.amount) || 0;
        q += qty; a += amt;
        return '<tr><td>' + esc(name) + '</td><td class="num">' + fmt(qty) + '</td><td class="num">' + fmt(r.Rate || r.rate || 0) +
            '</td><td class="num">' + fmt(amt) + '</td><td>' + esc(r.remarks || r.Remarks || '') + '</td></tr>';
    }).join('');
    if ($('totSubHistQty')) { $('totSubHistQty').textContent = rows.length ? fmt(q) : ''; $('totSubHistAmount').textContent = rows.length ? fmt(a) : ''; }
}

/* MakeShortCutKeys() (:2505) */
var BIB_SHORTCUTS = [
    ['Ctrl+S', 'For Save When on Entry form and For Show Data when on History Form'], ['Ctrl+U', 'For Update'],
    ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
    ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Inquiry Date'], ['Ctrl+F10', 'For Open Attachments'],
    ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Parent Item'],
    ['Ctrl+ArrowUp', 'For Focus On on Inquiry Date'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record']
];
function bibShortcutKeys() {
    var m = $('bibShortcutModal');
    if (!m) return;
    $('bibShortcutBody').innerHTML = BIB_SHORTCUTS.map(function (k) { return '<tr><td>' + esc(k[0]) + '</td><td>' + esc(k[1]) + '</td></tr>'; }).join('');
    m.classList.add('open');
}

/* frmBuyerInquiryBooking_KeyDown (:2400) - main page only; the sub page has its own */
document.addEventListener('keydown', function (e) {
    if (window.BIB_SUB_MODE || !e.ctrlKey) return;
    var onHistory = $('tabHistory') && $('tabHistory').style.display !== 'none';
    var k = (e.key || '').toLowerCase();
    function vis(id) { var b = $(id); return b && b.style.display !== 'none' && !b.disabled; }
    if (e.altKey && (e.key === 'Control' || e.key === 'Alt')) { bibShortcutKeys(); return; }
    if (k === 's' && !e.shiftKey) { e.preventDefault(); if (onHistory) bibLoadHistory(); else if (vis('btnSave')) bibSave(); }
    else if (k === 'u') { e.preventDefault(); if (!onHistory && vis('btnUpdate')) bibSave(); }
    else if (e.key === 'Delete' && e.shiftKey) { e.preventDefault(); if (!onHistory && vis('btnDelete')) bibDelete(); }
    else if (k === 'r') { e.preventDefault(); if (onHistory) bibRefreshHistoryCombos(); else bibRefresh(); }
    else if (k === 'n') { e.preventDefault(); if (onHistory) bibClearHistoryFilters(); else bibNew(); }
    else if (k === 'p') { e.preventDefault(); bibPrint(); }
    else if (k === 't') { e.preventDefault(); bibShowTab(null, onHistory ? 'tabForm' : 'tabHistory'); }
    else if (k === 'e') { e.preventDefault(); window.location.href = '/commission'; }
    else if (e.key === 'F10') { e.preventDefault(); bibAttachments(); }
    else if (e.key === 'F5' || e.key === 'ArrowUp') { e.preventDefault(); if ($('datInquiryDate')) $('datInquiryDate').focus(); }
    else if (e.key === 'ArrowDown') { e.preventDefault(); if ($('cmbParentItem')) $('cmbParentItem').focus(); }
});

/* the history's Item list narrows with the chosen parent item, as
   BindDropdownsAgainstParentCategory does (:1973) */
function bibHistParentChanged() {
    var pid = intOf('cmbHistParentItem');
    var rows = (lookupData.items || []).filter(function (r) {
        return !pid || parseInt(r.InventoryParentCategoriesId, 10) === pid;
    });
    fillSelect('cmbHistItem', rows, 'Id', 'ItemName');
    makeSearchable();
}

/* ------------------------------------------------------------ chrome */

function bibShowTab(ev, id) {
    if (ev) ev.preventDefault();
    ['tabForm', 'tabHistory'].forEach(function (t) { if ($(t)) $(t).style.display = (t === id) ? '' : 'none'; });
    Array.prototype.forEach.call(document.querySelectorAll('#bibTabs a'), function (a) {
        a.classList.toggle('active', a.dataset.tab === id);
    });
}

function bibToggleFullscreen(wrapId, btn) {
    var w = $(wrapId);
    if (!w) return;
    var on = w.classList.toggle('grid-full');
    if (btn) btn.textContent = on ? 'Exit full screen' : 'Full screen';
}

function bibResetFields() {
    ['txtShipToAddressText', 'txtRemarks', 'txtQualitySpecifications'].forEach(function (id) { $(id).value = ''; });
    ['txtQty', 'txtWeight', 'txtBuyerTargetPrice', 'txtTargetPurchasePrice'].forEach(function (id) { $(id).value = 0; });
    bibState.buyerAmount = 0; bibState.supplierAmount = 0;
}

function bibNew() {
    return withButton('btnNew', function () {
        $('inquiryBookingMasterId').value = 0;
        $('detailId').value = 0;
        $('txtInquiryNo').value = '';
        subPartyRows = []; removedSubPartyRows = []; paramRows = [];
        subSelectedIdx = 0; paramSelectedIdx = 0;
        bibResetFields();
        var today = ymd(new Date());
        $('datInquiryDate').value = today;
        $('datDeliveryStartDate').value = today;
        $('txtValidityDays').value = 1;
        $('txtDeliveryDays').value = 1;
        bibValidityDaysChanged();
        bibCalculateExpiryDate();
        ['cmbCommissionAgent', 'cmbBuyerName', 'cmbDeliveryToParty', 'cmbShipToAddress', 'cmbParentItem',
         'cmbItemName', 'cmbPackingType', 'cmbCropYear', 'cmbPackUom', 'cmbRateUom', 'cmbAnalysisGroup']
            .forEach(function (id) { if ($(id)) $(id).value = 0; });
        $('cmbAnalysisGroup').removeAttribute('data-loaded-group');   /* RecId = 0 (:949) */
        bibToggleSubParties();
        renderParams(); renderSubParties();
        makeSearchable();
        message('');
        applyButtonState();
        return Promise.all([
            getJson(API + '/generate-no')
                .then(function (d) { $('txtInquiryNo').value = (d && (d.docNo || d.DocNo || d.documentNo)) || ''; })
                .catch(function () { /* the server allocates the real number at save time */ }),
            applyPortalDefaults()
        ]);
    });
}

/* Reset() ends with GetCommissionAgentConfigurationsFromGlobalandBind() (:615): the five
   Commission Agent Portal configuration ids, each applied only when > 0. */
function applyPortalDefaults() {
    return getJson(LOOKUP + '/config-defaults')
        .then(function (d) {
            if (!d) return;
            [['cmbCommissionAgent', 'commissionAgentId'], ['cmbPaymentTerm', 'paymentTermId'],
             ['cmbDeliveryTerm', 'deliveryTermId'], ['cmbCropYear', 'cropYearId'],
             ['cmbPackingType', 'packingTypeId']].forEach(function (p) {
                var v = parseInt(d[p[1]], 10);
                if (v > 0 && $(p[0])) {
                    $(p[0]).value = v;
                    bibRefreshSelect2($(p[0]));
                    if (p[0] === 'cmbPaymentTerm') bibPaymentTermChanged();
                }
            });
        })
        .catch(function () { /* no configuration is not an error */ });
}

function bibRefresh() { return withButton('btnRefresh', function () { return loadLookups(); }); }

function bibPrint() {
    var id = intOf('inquiryBookingMasterId');
    if (!id) { message('No Data found to display', true); return; }
    bibPrintId(id);
}
function bibPrintId(id) { window.open(API + '/print/' + id, '_blank'); }

function bibAttachments() { message('Attachments are not implemented on this screen yet.', true); }
/* ------------------------------------ Ship To Address "+" (SupfrmShipToAddress, :2603)
 * Same endpoints as the Purchase Order dialog (BLL SupplierCustomerShipToAddress:
 * FormHistory / GetByID / Save -> Sp_SupplierCustomerShipToAddress_Insert|_Update).
 * Load: cmbsupplierfill (SupplierCustomerGetforComboServiceBind), cmbcountryfill, gridFill.
 * cmbcountry_Leave binds City.GetAll(org, company). Double-click a row = edit (Save hidden,
 * Update shown). Insert(): FormValidation messages, then "Save Successfully" /
 * "Update Successfully", gridFill, Reset. Nothing is pre-selected (Reset clears all). */
var ST_API = '/api/commission/purchase-order/ship-to';
var stLookups = false;
function stStr(r, k) { if (!r) return ''; var v = r[k]; if (v == null) v = r[k.charAt(0).toLowerCase() + k.slice(1)]; return v == null ? '' : String(v); }
function stInt(r, k) { var n = parseInt(stStr(r, k), 10); return isNaN(n) ? 0 : n; }
function stDate(v) {
    if (!v) return '';
    var d = new Date(v); if (isNaN(d)) return String(v);
    var p = function (n) { return (n < 10 ? '0' : '') + n; };
    return p(d.getDate()) + '/' + p(d.getMonth() + 1) + '/' + d.getFullYear() + ' ' + p(d.getHours()) + ':' + p(d.getMinutes());
}
function bibShipToLoadLookups() {
    return Promise.all([
        getJson(LOOKUP + '/suppliers').then(function (rows) { fillSelect('stParty', rows || [], 'Id', 'CompanyName'); }),
        getJson(ST_API + '/countries').then(function (rows) {
            fillSelect('stCountry', (rows || []).map(function (r) { return { Id: stInt(r, 'Id'), Name: stStr(r, 'Description') || stStr(r, 'CountryName') }; }), 'Id', 'Name');
        })
    ]).then(function () { stLookups = true; });
}
function bibAddShipToAddress() {
    return withButton('btnAddShipTo', function () {
        $('shipToModal').classList.add('open');
        bibShipToReset();
        return Promise.all([stLookups ? null : bibShipToLoadLookups(), bibShipToGrid()])
            .catch(function (e) { alert(e.message); });
    });
}
function bibShipToClose() { $('shipToModal').classList.remove('open'); }
function bibShipToReset() {
    $('stId').value = 0;
    ['stParty', 'stCountry', 'stCity'].forEach(function (id) { if ($(id)) $(id).value = '0'; });
    ['stTitle', 'stAddress', 'stContact', 'stPhone', 'stMobile', 'stWhatsApp'].forEach(function (id) { $(id).value = ''; });
    $('btnStSave').style.display = ''; $('btnStUpdate').style.display = 'none';
}
function bibShipToRefresh() {
    return withButton('btnStRefresh', function () { return bibShipToLoadLookups().catch(function (e) { alert(e.message); }); });
}
/* cmbcountry_Leave: City.GetAll(org, company) - not filtered by the country */
var stCitiesLoaded = false;
function bibShipToCountryLeave() {
    if (stCitiesLoaded) return Promise.resolve();
    return getJson(ST_API + '/cities').then(function (rows) {
        var keep = intOf('stCity');
        fillSelect('stCity', (rows || []).map(function (r) { return { Id: stInt(r, 'Id'), Name: stStr(r, 'CityName') }; }), 'Id', 'Name');
        if (keep) $('stCity').value = keep;
        stCitiesLoaded = true;
    });
}
function bibShipToGrid() {
    return getJson(ST_API + '/history').then(function (rows) {
        var body = $('grdShipToBody'); body.innerHTML = '';
        rows = rows || [];
        rows.forEach(function (r) {
            var tr = document.createElement('tr');
            tr.onclick = function () { Array.prototype.forEach.call(body.children, function (x) { x.classList.remove('sel'); }); tr.classList.add('sel'); };
            tr.ondblclick = function () { bibShipToEdit(stInt(r, 'Id')); };
            tr.innerHTML = ['PartyName', 'AddressTitle', 'CountryName', 'CityName', 'ContactPerson', 'PhoneNo', 'MobileNo', 'WhatsAppNo']
                .map(function (k) { return '<td>' + esc(stStr(r, k)) + '</td>'; }).join('') +
                '<td>' + esc(stDate(r.EntryDate || r.entryDate)) + '</td><td>' + esc(stStr(r, 'EntryUser')) + '</td>' +
                '<td>' + esc(stDate(r.ModifyDate || r.modifyDate)) + '</td><td>' + esc(stStr(r, 'ModifyUser')) + '</td>' +
                '<td>' + esc(stStr(r, 'AddressLine1')) + '</td>';
            body.appendChild(tr);
        });
        $('stCount').textContent = rows.length + ' Records';
    });
}
/* grdfrm_CellContentDoubleClick (:369) */
function bibShipToEdit(id) {
    Promise.all([getJson(ST_API + '/' + id), bibShipToCountryLeave()]).then(function (res) {
        var b = res[0];
        $('stId').value = id;
        $('stParty').value = stInt(b, 'SupplierCustomerId');
        $('stCountry').value = stInt(b, 'CountryId');
        $('stCity').value = stInt(b, 'CityId');
        $('stAddress').value = stStr(b, 'AddressLine1');
        $('stTitle').value = stStr(b, 'AddressTitle');
        $('stWhatsApp').value = stStr(b, 'WhatsAppNo');
        $('stContact').value = stStr(b, 'ContactPerson');
        $('stMobile').value = stStr(b, 'MobileNo');
        $('stPhone').value = stStr(b, 'PhoneNo');
        $('btnStSave').style.display = 'none'; $('btnStUpdate').style.display = '';
    }).catch(function (e) { alert(e.message); });
}
/* Insert() / FormValidation() (:77-111, :279) */
function bibShipToSave(btnId) {
    var focus = function (id, msg) { alert(msg); $(id).focus(); };
    if (!intOf('stParty')) return focus('stParty', 'Please Select Supplier');
    if (!val('stAddress').trim()) return focus('stAddress', 'Please Enter Address');
    if (!val('stTitle').trim()) return focus('stTitle', 'Please Enter Address Title');
    if (!intOf('stCountry')) return focus('stCountry', 'Please Select Country');
    if (!intOf('stCity')) return focus('stCity', 'Please Select City');
    return withButton(btnId, function () {
        return fetch(ST_API + '/save', {
            method: 'POST', headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                Id: intOf('stId'), SupplierCustomerId: intOf('stParty'), CountryId: intOf('stCountry'), CityId: intOf('stCity'),
                AddressLine1: val('stAddress').trim(), AddressTitle: val('stTitle').trim(), PhoneNo: val('stPhone').trim(),
                MobileNo: val('stMobile').trim(), WhatsAppNo: val('stWhatsApp').trim(), ContactPerson: val('stContact').trim()
            })
        }).then(function (r) { return r.json(); }).then(function (d) {
            if (!d || !d.success) { alert((d && d.message) || 'Save refused.'); return; }
            alert(intOf('stId') ? 'Update Successfully' : 'Save Successfully');
            bibShipToReset();
            /* the new address becomes pickable in CmbShipToAddress without a page reload */
            return Promise.all([bibShipToGrid(), getJson(LOOKUP + '/ship-to-addresses').then(function (rows) {
                lookupData.shipToAddresses = rows || [];
                bibDeliveryPartyChanged && bibDeliveryPartyChanged();
            }).catch(function () {})]);
        }).catch(function (e) { alert(e.message); });
    });
}
function bibOpenSupplierOffer() { window.location.href = '/commission/supplier-offer'; }
/* openForm(Id) (:2635): opens frmSubBuyerInquiryBooking as its own window and ReadById(Id).
   The web opens /commission/sub-buyer-inquiry-booking?id= in a new tab; when a popup is
   blocked (after an async save) the current tab navigates there instead. */
function bibAddSubPartyTo(id, afterSave) {
    if (!id) { message('Please Save The Record First or Edit Any', true); return; }
    var url = '/commission/sub-buyer-inquiry-booking?id=' + encodeURIComponent(id);
    var w = window.open(url, '_blank');
    if (!w && afterSave) window.location.href = url;
    else if (!w) window.location.href = url;
}

/* ------------------------------------------------------------- start */

document.addEventListener('DOMContentLoaded', function () {
    /* the Sub Buyer Inquiry page reuses this file and starts itself */
    if (window.BIB_SUB_MODE) return;
    loadLookups().then(function () { return bibNew(); });
});

/* History From/To: the desktop DateTimePickers (FromDateHistory / ToDateHistory) are never
   assigned in code, so they open on the designer default - today - with their check box ON
   (ShowCheckBox, Checked defaults true), i.e. the history filters today..today. Validity
   From/To are Checked=false in the designer and stay blank. Local calendar date, not UTC. */
(function () {
    function seed() {
        var d = new Date();
        var t = d.getFullYear() + '-' + ('0' + (d.getMonth() + 1)).slice(-2) + '-' + ('0' + d.getDate()).slice(-2);
        ['histFromDate', 'histToDate'].forEach(function (id) {
            var e = document.getElementById(id);
            if (e && !e.value) e.value = t;
        });
    }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', seed); else seed();
})();
