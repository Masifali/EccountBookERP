/* ===========================================================================
 * Commission Trading - Purchase Order / Deal With Supplier (DocumentTypeId 1052)
 *
 * Ported from Architecture.WinApp.Cmagt/frmPurchaseOrderCmagt.cs. This screen is
 * NOT the general Purchase module's Purchase Order (doc type 41, dbo schema,
 * PurchsaeOrder.cs) - different model, different procedures, different tabs.
 *
 * Desktop arithmetic reproduced exactly:
 *
 *   CalculateWeight()     (frmPurchaseOrderCmagt.cs:1257-1270)
 *       weight = qty x PackUom.Equivalent
 *
 *   CalculateAmount()     (:1272-1286)
 *       amount = (weight > 0 && rateEquivalent > 0)
 *                    ? weight / rateEquivalent x rate
 *                    : 0
 *
 *   CalculateTaxAmount()  (:1372-1394)
 *       taxAmount   = amount x taxPercent / 100
 *       totalAmount = amount + taxAmount
 *       both rounded to 3 decimals, MidpointRounding.AwayFromZero
 *
 *   CalculateExpiryDate() (:3607-3624)
 *       expiry = deliveryDays == 0 ? deliveryStartDate
 *                                  : deliveryStartDate + deliveryDays
 *
 * Two rules this file holds to, because getting them wrong is what produced
 * silently wrong numbers before:
 *
 *   1. A UOM's database Id is never used as its conversion factor. `uomId` and
 *      `equivalent` are separate fields end to end.
 *   2. A missing or non-positive Equivalent is never quietly replaced with 1
 *      or 0. The row is refused, the UOM is named in the message, and Add is
 *      blocked - the desktop cannot produce such a row either.
 *
 * The server must recompute and verify these figures; nothing here is
 * authoritative.
 * =========================================================================== */

'use strict';

var API = '/api/commission/purchase-order';
var LOOKUP = '/api/commission/dropdowns';

/* Lookup contract. Each entry is one endpoint this screen needs, the desktop
 * source that establishes it, and where its rows land. `value`/`text` name the
 * columns the desktop's ValueList binds; `extra` names any further column the
 * screen reads (Equivalent is the important one). */
var LOOKUPS = [
    { key: 'companies',        url: LOOKUP + '/companies',         el: 'cmbCompany',          value: 'Id', text: 'CompName',      desktop: 'CommonServices.CompanyServiceBind()' },
    { key: 'branches',         url: LOOKUP + '/branches',          el: 'cmbBranch',           value: 'BranchId', text: 'BranchName', desktop: 'BranchesFill -> USP_GetBranchsAllocatedToUser; value member BranchId (:674)' },
    { key: 'commissionAgents', url: LOOKUP + '/commission-agents', el: 'cmbCommissionAgent',  value: 'Id', text: 'CompanyName',   desktop: 'CommonBindings.SupplierBind(CmbCommissionAgent, ...)' },
    { key: 'suppliers',        url: LOOKUP + '/suppliers',         el: 'cmbSupplierName',     value: 'Id', text: 'CompanyName',   desktop: 'CommonBindings.SupplierBind(CmbSupplierName, ...)' },
    { key: 'deliveryParties',  url: LOOKUP + '/suppliers',         el: 'cmbDeliveryToParty',  value: 'Id', text: 'CompanyName',   desktop: 'CmbDeliveryToParty' },
    { key: 'shipToAddresses',  url: LOOKUP + '/ship-to-addresses', el: 'cmbShipToAddress',    value: 'Id', text: 'AddressLine1',  extra: ['SupplierCustomerId'], desktop: 'SupplierCustomerShipToAddress.GetAll_Combo' },
    { key: 'deliveryTerms',    url: LOOKUP + '/delivery-terms',    el: 'cmbDeliveryTerm',     value: 'Id', text: 'Description',   desktop: 'clsGlobalVariables.globalDeliveryTermList' },
    { key: 'paymentTerms',     url: LOOKUP + '/payment-terms',     el: 'cmbPaymentTerm',      value: 'Id', text: 'TermsDescription', desktop: 'clsGlobalVariables.globalPaymentTerm' },
    { key: 'parentItems',      url: LOOKUP + '/parent-categories', el: 'cmbParentItem',       value: 'Id', text: 'InvParentCateDescription', desktop: 'CommonBindings.ParentCategoryBindFromGlobal' },
    { key: 'items',            url: LOOKUP + '/items',             el: 'cmbItemName',         value: 'Id', text: 'ItemName',      extra: ['InventoryParentCategoriesId'], desktop: 'CommonBindings.ItemdtFillFromGlobal' },
    { key: 'cropYears',        url: LOOKUP + '/crop-years',        el: 'cmbCropYear',         value: 'Id', text: 'CropYear',      desktop: 'CommonBindings.CropDtFillFromGlobalAndBind' },
    { key: 'packingTypes',     url: LOOKUP + '/packing-types',     el: 'cmbPackingType',      value: 'Id', text: 'PackTypeDesc',  desktop: 'CommonBindings.PackingTypeDtFillFromGlobalAndBind' },
    /* CmbCommissionAc and CmbBrokeryAc are bound to the SAME party table as the other combos
     * (SupplierBind, frmPurchaseOrderCmagt.cs:904-905), so the display column is CompanyName.
     * There is no AccountTitle here and no chart-of-accounts lookup: what is saved is a party
     * id (comm.commissionAgentId = CmbCommissionAc.Value, :3140/:3151). */
    { key: 'accounts',         url: LOOKUP + '/accounts',          el: 'cmbCommissionAc',     value: 'Id', text: 'CompanyName',   alsoInto: ['cmbBrokeryAc'], desktop: 'SupplierBind(CmbCommissionAc / CmbBrokeryAc, dtSupplier) (:904-905)' },
    { key: 'emptyBagItems',    url: LOOKUP + '/empty-bag-items',   el: null,                  value: 'ItemId', text: 'ItemName',  desktop: 'grdEmptyBagsPm EmptyBagItem ValueList, dtItemForEmptyBag (:2250)' },
    { key: 'otherItems',       url: LOOKUP + '/other-items',       el: null,                  value: 'Id', text: 'OtherItemName', desktop: 'grdInvExp ItemId ValueList, ComboBind(..., "Id", "OtherItemName") (:2033)' },
    { key: 'viewCombos',       url: LOOKUP + '/view-combos',       el: null,                  value: 'Id', text: 'ReferenceName', desktop: 'BindViewCombos() (:716-749) - one call, split by Activity' }
];

/* BindViewCombos (frmPurchaseOrderCmagt.cs:716-749) fills four combos from ONE
 * result set, keyed on its Activity column. These are configuration reference
 * lists, not masters - in particular AllocatedPackingType is what seeds the
 * empty-bags weight-cut grid, and CommissionRateUom's ReferenceName is itself
 * the divisor for a Weight-type commission. */
var VIEW_ACTIVITIES = {
    CommissionType:       ['cmbCommType', 'cmbBrokeryType'],
    CommissionRateUom:    ['cmbCommUom', 'cmbBrokeryRateUom'],
    PaymentBaseDate:      [],        /* used by the payment schedule grid */
    AllocatedPackingType: []         /* seeds the empty-bags weight-cut grid */
};

function viewRows(activity) {
    return (lookupData.viewCombos || []).filter(function (r) { return r.Activity === activity; });
}

function bindViewCombos() {
    Object.keys(VIEW_ACTIVITIES).forEach(function (act) {
        var rows = viewRows(act);
        VIEW_ACTIVITIES[act].forEach(function (elId) {
            fillSelect(elId, rows, 'Id', 'ReferenceName');
        });
    });
}

/* Item-scoped UOM lists (Pack Uom / Rate Uom) come from the item's own UOM
 * schedule, exactly as CommonBindings.ItemUomFromGlobalBind does on the
 * desktop - they are not a global list, so they load when an item is picked. */
/* The tax combo is item-scoped AND date-scoped, not a master list: CmbItemName_Leave
 * (frmPurchaseOrderCmagt.cs:1202) calls TaxTypeDbCall(ItemId, datDocDate.Value), which reads
 * the item's tax SCHEDULE as at that date. It is empty until an item is chosen, it reloads on
 * every item change, and changing the document date can change the percent - so it is fetched
 * on demand and never cached across items. */
var TAX_URL      = LOOKUP + '/taxes';          /* ?itemId=&docDate= -> [{TaxNameId, TaxName, TaxPercent}] */
var ITEM_UOM_URL = LOOKUP + '/item-uoms';      /* ?itemId= -> [{Id, UOMCode, Equivalent, BaseRateUom, BasePackUom}] */

var lookupData   = {};     /* key -> rows */
var missingLookups = [];   /* endpoints that returned nothing or failed */
var detailRows   = [];
var expenseRows  = [];
var emptyBagRows   = [];   /* grdEmptyBags   - entryTypeId 1 */
var emptyBagPmRows = [];   /* grdEmptyBagsPm - entryTypeId 2 */
var paymentRows  = [];
var editingIdx   = -1;
var selectedIdx  = 0;
var busy         = false;

/* ---------------------------------------------------------------- helpers */

function $(id) { return document.getElementById(id); }
function val(id) { var e = $(id); return e ? e.value : ''; }
function num(id) { var n = parseFloat(val(id)); return isNaN(n) ? 0 : n; }
function intOf(id) { var n = parseInt(val(id), 10); return isNaN(n) ? 0 : n; }
function esc(s) {
    return String(s === undefined || s === null ? '' : s)
        .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
}

/* 3 decimals, half away from zero - the .NET MidpointRounding.AwayFromZero the
 * desktop uses. JavaScript's Math.round is half-up, which differs for negative
 * midpoints, so the sign is handled explicitly. */
function r3(x) {
    if (!isFinite(x)) return 0;
    var s = x < 0 ? -1 : 1;
    return s * Math.round(Math.abs(x) * 1000 + 1e-9) / 1000;
}

function fmt(x) { return r3(x).toFixed(3).replace(/\.?0+$/, '') || '0'; }

/* Local-calendar yyyy-MM-dd. toISOString() converts to UTC first, which at the site's UTC+5
 * turns local midnight into the previous day - every computed date (expiry, due date, the new
 * document's date) came out one day early. Same defect and fix as Buyer Inquiry (1050). */
function ymd(d) {
    var m = d.getMonth() + 1, day = d.getDate();
    return d.getFullYear() + '-' + (m < 10 ? '0' : '') + m + '-' + (day < 10 ? '0' : '') + day;
}
function addDays(dateStr, days) {
    if (!dateStr) return '';
    var d = new Date(dateStr + 'T00:00:00');
    d.setDate(d.getDate() + (parseInt(days, 10) || 0));
    return ymd(d);
}
/* A date as the API returns it: a DATE column arrives as "yyyy-MM-dd", a DATETIME column as an
 * ISO instant in UTC ("...T19:00:00.000+00:00" for local midnight), sometimes epoch millis.
 * Instants are converted to the local calendar day; a bare date is taken as is. */
function dstr(v) {
    if (v === undefined || v === null || v === '') return '';
    if (typeof v === 'number') return ymd(new Date(v));
    var t = String(v);
    if (/^\d{4}-\d{2}-\d{2}$/.test(t)) return t;
    if (/T.*(Z|[+-]\d{2}:?\d{2})$/.test(t)) { var d = new Date(t); return isNaN(d.getTime()) ? t.slice(0, 10) : ymd(d); }
    return t.slice(0, 10);
}
/* Read a column whatever casing the procedure gave it (DeliveryToPartyId vs deliveryToPartyId). */
function col(row, name) {
    if (!row) return undefined;
    if (row[name] !== undefined) return row[name];
    var lower = name.toLowerCase();
    for (var k in row) { if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === lower) return row[k]; }
    return undefined;
}
function colNum(row, name) { var n = parseFloat(col(row, name)); return isNaN(n) ? 0 : n; }
function colInt(row, name) { var n = parseInt(col(row, name), 10); return isNaN(n) ? 0 : n; }
function colStr(row, name) { var v = col(row, name); return v === undefined || v === null ? '' : String(v); }

function message(text, isError) {
    var box = $('poMessage');
    if (!box) return;
    box.textContent = text || '';
    box.className = 'cmagt-status' + (isError ? ' error' : '');
    box.style.display = text ? 'block' : 'none';
}

/*
 * The five button rules. (1)/(4) every action button is disabled for the whole request and its
 * prior disabled state is remembered so Update/Delete do not come back enabled on a new
 * document. (2) the form is marked busy and the button that started it carries a spinner -
 * previously the buttons simply greyed out with no indicator at all. (3) setBusy is called
 * synchronously at the top of each action and busyGuard() holds a named lock. (5) every caller
 * restores through .finally(), so a failed request re-enables exactly like a successful one.
 */
function setBusy(on, btnId) {
    busy = on;
    /* This page has no single form wrapper, so the busy class goes on the toolbar that holds
       the action buttons rather than on <body>, which would dim the entire page. */
    var bar = ($('btnSave') && $('btnSave').parentElement) || null;
    if (bar && bar.classList) bar.classList.toggle('is-busy', !!on);
    if (on && btnId && $(btnId)) { $(btnId).classList.add('btn-busy'); }
    if (!on) {
        Array.prototype.forEach.call(document.querySelectorAll('.btn-busy'),
            function (b) { b.classList.remove('btn-busy'); });
    }
    ['btnNew', 'btnRefresh', 'btnSave', 'btnSaveAs', 'btnUpdate', 'btnDelete', 'btnLoadSo', 'btnPrint', 'btnHistory', 'btnAddDetail']
        .forEach(function (id) {
            var b = $(id);
            if (!b) return;
            if (on) { b.dataset.wasDisabled = b.disabled ? '1' : '0'; b.disabled = true; }
            else    { b.disabled = b.dataset.wasDisabled === '1'; }
        });
}

/* Rule 3 - a named lock, so a repeat keypress or a programmatic call cannot re-enter an
   action that is already running. */
var poInFlight = {};
function poGuard(name) {
    if (poInFlight[name]) return false;
    poInFlight[name] = true;
    return true;
}
function poRelease(name) { poInFlight[name] = false; }

function getJson(url) {
    return fetch(url, { headers: { 'Accept': 'application/json' } })
        .then(function (r) { return r.ok ? r.json() : Promise.reject(new Error(r.status + ' ' + url)); });
}

/* ------------------------------------------------------------- lookups */

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

function fillSelect(elId, rows, valueKey, textKey, placeholder) {
    var sel = $(elId);
    if (!sel) return;
    var keep = sel.value;
    sel.innerHTML = '<option value="0">' + esc(placeholder || '-- Select --') + '</option>';
    (rows || []).forEach(function (r) {
        var o = document.createElement('option');
        o.value = r[valueKey];
        o.textContent = r[textKey];
        applyComboColumns(o, r);        /* the extra grid columns, see below */
        sel.appendChild(o);
    });
    if (keep) sel.value = keep;                 /* a saved selection survives a refresh */
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
        bindViewCombos();
        if (missingLookups.length) {
            message('These lookups returned no rows, so their dropdowns are empty: '
                + missingLookups.join(', ')
                + '. The screen will not invent values for them.', true);
        } else {
            message('');
        }
    });
}

/* Pack Uom / Rate Uom for the chosen item, with their Equivalent factors. */
function loadItemUoms(itemId) {
    if (!itemId) {
        fillSelect('cmbPackUom', [], 'Id', 'UOMCode');
        fillSelect('cmbRateUom', [], 'Id', 'UOMCode');
        lookupData.itemUoms = [];
        return Promise.resolve();
    }
    return getJson(ITEM_UOM_URL + '?itemId=' + encodeURIComponent(itemId))
        .then(function (rows) {
            lookupData.itemUoms = rows || [];
            fillSelect('cmbPackUom', rows, 'Id', 'UOMCode');
            fillSelect('cmbRateUom', rows, 'Id', 'UOMCode');
            /* desktop pre-selects the item's base pack/rate UOM */
            (rows || []).forEach(function (r) {
                if (r.BasePackUom) $('cmbPackUom').value = r.Id;
                if (r.BaseRateUom) $('cmbRateUom').value = r.Id;
            });
            poCalcRow();
        })
        .catch(function () {
            lookupData.itemUoms = [];
            fillSelect('cmbPackUom', [], 'Id', 'UOMCode');
            fillSelect('cmbRateUom', [], 'Id', 'UOMCode');
            message('Could not load the UOM schedule for this item (' + ITEM_UOM_URL + '). '
                  + 'Weight and Amount cannot be calculated until it loads.', true);
        });
}

/* Resolve an Equivalent by UOM id. Returns null when it cannot be resolved -
 * never 0 and never 1. */
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

/* --------------------------------------------------- desktop calculations */

/* weight = qty x PackUom.Equivalent   (CalculateWeight, :1257) */
function poCalcWeight() {
    var qty = num('txtQty');
    var packId = intOf('cmbPackUom');
    var eq = equivalentOf(packId);
    if (eq === null) {
        if (packId) {
            message('Pack Uom "' + uomCode(packId) + '" has no usable conversion factor (Equivalent). '
                  + 'Weight cannot be calculated; the row cannot be added.', true);
        }
        $('txtWeight').value = '';
        return null;
    }
    var w = r3(qty * eq);
    $('txtWeight').value = w;
    return w;
}

/* amount = weight / RateUom.Equivalent x rate   (CalculateAmount, :1272) */
function poCalcAmount() {
    var weight = num('txtWeight');
    var rate = num('txtRate');
    var rateId = intOf('cmbRateUom');
    var eq = equivalentOf(rateId);

    if (eq === null) {
        if (rateId) {
            message('Rate Uom "' + uomCode(rateId) + '" has no usable conversion factor (Equivalent). '
                  + 'Amount cannot be calculated; the row cannot be added.', true);
        }
        $('txtAmount').value = '';
        return null;
    }
    /* the desktop's own guard: a non-positive weight or factor yields 0, and a
       zero rate legitimately yields 0 */
    var amt = (weight > 0 && eq > 0) ? r3(weight / eq * rate) : 0;
    $('txtAmount').value = amt;
    return amt;
}

/* taxAmount = amount x taxPercent / 100 ; total = amount + tax  (:1372) */
function poCalcTax() {
    var amount = num('txtAmount');
    var pct = num('txtTaxPercnt');
    var tax = r3(amount * pct / 100);
    $('txtTaxAmount').value = tax;
    $('txtTotalAmount').value = r3(amount + tax);
}

/* One entry point for the chain, mirroring the desktop's event order:
 *   txtQty / CmbPackUom  -> CalculateWeight -> CalculateAmount -> tax
 *   txtWeight / txtRate / CmbRateUom -> CalculateAmount -> tax          */
function poCalcRow(skipWeight, isUomChange) {
    if (!skipWeight) poCalcWeight();
    if (!isUomChange) poCalcAmount();
    poCalcTax();
}

/* CmbTaxName_Leave (:1360-1390). The percent is read from the selected row - the desktop takes
 * SelectedRow.Cells[2], which is the TaxPercent column - and an already-typed percent wins over
 * the schedule's, which is why txtTaxPercnt is only overwritten when it is empty or zero (:1382). */
function poTaxNameChanged() {
    var id = intOf('cmbTaxName');
    var rows = lookupData.taxNames || [];
    for (var i = 0; i < rows.length; i++) {
        if (parseInt(rows[i].TaxNameId, 10) === id) {
            var typed = parseFloat($('txtTaxPercnt').value);
            if (!(typed > 0)) {
                var p = parseFloat(rows[i].TaxPercent);
                $('txtTaxPercnt').value = isNaN(p) ? '' : p;
            }
            break;
        }
    }
    poCalcTax();
}

/* TaxTypeDbCall + TaxTypeBind (:1218-1255). No item means the combo is emptied and the two tax
 * boxes cleared, exactly as :1206-1209 does - it is not left showing the previous item's tax. */
function loadTaxesForItem(itemId) {
    var sel = $('cmbTaxName');
    if (!itemId) {
        lookupData.taxNames = [];
        if (sel) sel.innerHTML = '<option value="0"></option>';
        $('txtTaxPercnt').value = '';
        $('txtTaxAmount').value = '';
        poCalcTax();
        return;
    }
    var url = TAX_URL + '?itemId=' + encodeURIComponent(itemId)
            + '&docDate=' + encodeURIComponent(val('datDocDate') || '');
    fetch(url, { headers: { 'Accept': 'application/json' } })
        .then(function (r) { return r.ok ? r.json() : []; })
        .then(function (rows) {
            lookupData.taxNames = rows || [];
            fillSelect('cmbTaxName', lookupData.taxNames, 'TaxNameId', 'TaxName');
            poTaxNameChanged();
        })
        .catch(function (e) {
            console.warn('tax schedule lookup failed for item ' + itemId, e);
            lookupData.taxNames = [];
            if (sel) sel.innerHTML = '<option value="0"></option>';
        });
}

/* expiry = start + delivery days, or = start when days is 0  (:3607) */
function poCalculateExpiryDate() {
    var start = val('datDeliveryStartDate');
    if (!start) { $('datExpiryDate').value = ''; return; }
    var days = intOf('txtDeliveryDays');
    $('datExpiryDate').value = days === 0 ? start : addDays(start, days);
}

/* payment term 1 or 3 -> due days 0 and locked; term 2 -> default 2  (:769 in
 * the sibling Buyer Inquiry form, same rule in this one's ValueChanged) */
function poPaymentTermChanged() {
    var t = intOf('cmbPaymentTerm');
    var due = $('txtDueDays');
    due.disabled = false;
    if (t === 1 || t === 3) { due.value = 0; due.disabled = true; }
    else if (t === 2 && num('txtDueDays') === 0) { due.value = 2; }
}

/* commission / brokerage: Flat = rate, Percent = detail amount x rate / 100,
 * Weight = total weight / uom equivalent x rate   (TotalCommissionAmount) */
function calcAgentAmount(typeSelId, rateId, uomSelId, outId) {
    /* TotalCommissionAmount(), frmPurchaseOrderCmagt.cs:1468-1510, and
       TotalBrokeryAmountCalculate(). Three modes, keyed on the type combo's text:
         Flat    -> amount = rate
         Percent -> rate is clamped to 100, amount = Sum(detail Amount) x rate / 100,
                    rounded to a whole number
         Weight  -> amount = Sum(detail Weight) / UOM x rate, rounded to a whole
                    number, where UOM is the commission rate-UOM combo's own
                    DISPLAYED TEXT parsed as a number (:1501). That list is a
                    configuration reference list whose text is the divisor - it is
                    NOT the item's UOM schedule and must not be resolved through it. */
    var typeText = ($(typeSelId) && $(typeSelId).selectedOptions.length)
        ? $(typeSelId).selectedOptions[0].textContent.trim() : '';
    var rate = num(rateId);
    var totals = detailTotals();

    if (rate <= 0) { $(outId).value = 0; return; }

    if (typeText === 'Flat') {
        $(outId).value = r3(rate);
        return;
    }

    if (typeText === 'Percent') {
        if (rate > 100) { rate = 100; $(rateId).value = 100; }
        $(outId).value = Math.round(totals.amount * rate / 100);
        return;
    }

    if (typeText === 'Weight') {
        var uomText = ($(uomSelId) && $(uomSelId).selectedOptions.length)
            ? $(uomSelId).selectedOptions[0].textContent.trim() : '';
        var uom = parseFloat(uomText);
        if (!isFinite(uom) || uom <= 0) {
            message('The commission / brokerage Rate Uom "' + uomText + '" is not a usable divisor. '
                  + 'On the desktop this list holds the divisor as its text; the amount is left blank '
                  + 'rather than guessed.', true);
            $(outId).value = '';
            return;
        }
        $(outId).value = Math.round(totals.weight / uom * rate);
        return;
    }

    $(outId).value = 0;
}

function poCalcCommission() { calcAgentAmount('cmbCommType', 'txtCommRate', 'cmbCommUom', 'txtCommAmount'); }
function poCalcBrokery()    { calcAgentAmount('cmbBrokeryType', 'txtBrokeryRate', 'cmbBrokeryRateUom', 'txtBrokeryAmount'); }

/* ------------------------------------------------------- header behaviour */

function poDeliveryPartyChanged() {
    var partyId = intOf('cmbDeliveryToParty');
    var rows = (lookupData.shipToAddresses || []).filter(function (r) {
        return !partyId || parseInt(r.SupplierCustomerId, 10) === partyId;
    });
    fillSelect('cmbShipToAddress', rows, 'Id', 'AddressLine1');
    if (!partyId) $('txtShipToAddress').value = '';
}

/* picking an address back-fills its party, as CmbShipToAddress_Leave does */
function poShipToAddressChanged() {
    var id = intOf('cmbShipToAddress');
    var rows = lookupData.shipToAddresses || [];
    for (var i = 0; i < rows.length; i++) {
        if (parseInt(rows[i].Id, 10) === id) {
            if (rows[i].SupplierCustomerId) $('cmbDeliveryToParty').value = rows[i].SupplierCustomerId;
            $('txtShipToAddress').value = rows[i].AddressLine1 || '';
            break;
        }
    }
}

function poParentItemChanged() {
    var pid = intOf('cmbParentItem');
    var rows = (lookupData.items || []).filter(function (r) {
        return !pid || parseInt(r.InventoryParentCategoriesId, 10) === pid;
    });
    fillSelect('cmbItemName', rows, 'Id', 'ItemName');
    loadTaxesForItem(0);
    loadItemUoms(0);
}

function poItemChanged() {
    var itemId = intOf('cmbItemName');
    /* the desktop also back-fills the parent category from the item */
    var rows = lookupData.items || [];
    for (var i = 0; i < rows.length; i++) {
        if (parseInt(rows[i].Id, 10) === itemId && rows[i].InventoryParentCategoriesId) {
            $('cmbParentItem').value = rows[i].InventoryParentCategoriesId;
            break;
        }
    }
    /* CmbItemName_Leave does both: the tax schedule THEN the UOMs (:1202, :1211). */
    loadTaxesForItem(itemId);
    loadItemUoms(itemId);
}

/* ------------------------------------------------------------ detail grid */

function textOf(selId) {
    var s = $(selId);
    return (s && s.selectedOptions.length && s.value !== '0') ? s.selectedOptions[0].textContent : '';
}

function poAddOrUpdateDetail() {
    message('');

    var itemId = intOf('cmbItemName');
    if (!itemId) { message('Item Field Required', true); return; }
    if (!intOf('cmbParentItem')) { message('Parent Item Field Required', true); return; }
    if (!intOf('cmbCropYear')) { message('Crop Year Field Required', true); return; }
    if (!intOf('cmbPackingType')) { message('Packing Type Field Required', true); return; }

    var packUomId = intOf('cmbPackUom');
    var rateUomId = intOf('cmbRateUom');
    var packEq = equivalentOf(packUomId);
    var rateEq = equivalentOf(rateUomId);

    if (!packUomId) { message('Pack Uom Field is Required', true); return; }
    if (!rateUomId) { message('Rate Uom Field is Required', true); return; }
    if (packEq === null) { message('Pack Uom "' + uomCode(packUomId) + '" has no usable Equivalent - the row cannot be added.', true); return; }
    if (rateEq === null) { message('Rate Uom "' + uomCode(rateUomId) + '" has no usable Equivalent - the row cannot be added.', true); return; }

    var qty = num('txtQty');
    if (qty <= 0) { message('Qty Field Required', true); return; }

    var weight = r3(qty * packEq);
    var rate = num('txtRate');
    var amount = (weight > 0 && rateEq > 0) ? r3(weight / rateEq * rate) : 0;
    var taxPct = num('txtTaxPercnt');
    var taxAmount = r3(amount * taxPct / 100);

    var row = {
        purchaseOrderDetailId: editingIdx >= 0 ? (detailRows[editingIdx].purchaseOrderDetailId || 0) : 0,
        actionTypeId: 0,                                   /* set at save time */
        inventoryParentCategoryId: intOf('cmbParentItem'),
        parentItemName: textOf('cmbParentItem'),
        itemId: itemId,
        itemName: textOf('cmbItemName'),
        cropYearId: intOf('cmbCropYear'),
        cropYear: textOf('cmbCropYear'),
        packingTypeId: intOf('cmbPackingType'),
        packingType: textOf('cmbPackingType'),
        packUomId: packUomId,
        packUomCode: uomCode(packUomId),
        packUomEquivalent: packEq,                         /* factor, never the id */
        itemQty: qty,
        itemWeight: weight,
        itemRate: rate,
        rateUomId: rateUomId,
        rateUomCode: uomCode(rateUomId),
        rateUomEquivalent: rateEq,
        itemAmount: amount,
        taxNameId: intOf('cmbTaxName'),
        taxName: textOf('cmbTaxName'),
        taxPercent: taxPct,
        taxAmount: taxAmount,
        totalAmount: r3(amount + taxAmount),
        remarks: val('txtRemarksDetail')
    };

    if (editingIdx >= 0) { detailRows[editingIdx] = row; editingIdx = -1; }
    else { detailRows.push(row); }

    $('btnAddDetail').textContent = '+';
    $('btnCancelDetail').style.display = 'none';
    renderDetail();
    clearDetailEntry();
    poCalcCommission();
    poCalcBrokery();
    poPaymentAmountReCalculate();
}

function clearDetailEntry() {
    ['txtQty', 'txtWeight', 'txtRate', 'txtAmount', 'txtTaxPercnt', 'txtTaxAmount', 'txtTotalAmount']
        .forEach(function (id) { $(id).value = 0; });
    $('txtRemarksDetail').value = '';
}

function poEditDetail(i) {
    var r = detailRows[i];
    if (!r) return;
    editingIdx = i;
    $('cmbParentItem').value = r.inventoryParentCategoryId || 0;
    poParentItemChanged();
    $('cmbItemName').value = r.itemId || 0;
    loadItemUoms(r.itemId).then(function () {
        $('cmbPackUom').value = r.packUomId || 0;
        $('cmbRateUom').value = r.rateUomId || 0;
        $('txtQty').value = r.itemQty;
        $('txtWeight').value = r.itemWeight;
        $('txtRate').value = r.itemRate;
        $('txtAmount').value = r.itemAmount;
        $('txtTaxPercnt').value = r.taxPercent;
        $('txtTaxAmount').value = r.taxAmount;
        $('txtTotalAmount').value = r.totalAmount;
    });
    $('cmbCropYear').value = r.cropYearId || 0;
    $('cmbPackingType').value = r.packingTypeId || 0;
    $('cmbTaxName').value = r.taxNameId || 0;
    $('txtRemarksDetail').value = r.remarks || '';
    $('btnAddDetail').textContent = 'Update';
    $('btnCancelDetail').style.display = '';
}

function poCancelDetailEdit() {
    editingIdx = -1;
    $('btnAddDetail').textContent = '+';
    $('btnCancelDetail').style.display = 'none';
    clearDetailEntry();
}

/* The desktop soft-deletes a saved row (actionTypeId = 3) and simply drops an
 * unsaved one, so a removed row that already has an id is kept for the save. */
var removedDetailRows = [];

function poDeleteDetail(i) {
    var r = detailRows[i];
    if (!r) return;
    if (!confirm('Are you sure to delete?')) return;
    if (r.purchaseOrderDetailId) { r.actionTypeId = 3; removedDetailRows.push(r); }
    detailRows.splice(i, 1);
    if (editingIdx === i) poCancelDetailEdit();
    renderDetail();
    poCalcCommission();
    poCalcBrokery();
    poPaymentAmountReCalculate();
}

/* PaymentAmountReCalculate (:1573-1592), called after every detail add/update/delete
 * (:1737, :1913, :1989): each schedule row's Amount = %OfTotal x Sum(detail TotalAmount) / 100. */
function poPaymentAmountReCalculate() {
    var total = detailTotals().total;
    if (!(total > 0) || !paymentRows.length) return;
    paymentRows.forEach(function (r) { r.dueAmount = (+r.pctOfTotal || 0) * total / 100; });
    renderPayment();
}

function detailTotals() {
    return detailRows.reduce(function (t, r) {
        t.qty += +r.itemQty || 0;
        t.weight += +r.itemWeight || 0;
        t.amount += +r.itemAmount || 0;
        t.tax += +r.taxAmount || 0;
        t.total += +r.totalAmount || 0;
        return t;
    }, { qty: 0, weight: 0, amount: 0, tax: 0, total: 0 });
}

function renderDetail() {
    var body = $('grdDetailBody');
    body.innerHTML = '';

    if (!detailRows.length) {
        body.innerHTML = '<tr><td colspan="17">No detail rows yet.</td></tr>';
    } else {
        detailRows.forEach(function (r, i) {
            var tr = document.createElement('tr');
            if (i === selectedIdx) tr.className = 'selected';
            tr.onclick = function () { selectedIdx = i; renderDetail(); };
            tr.innerHTML =
                '<td><button type="button" class="danger" onclick="event.stopPropagation();poDeleteDetail(' + i + ')">X</button></td>' +
                '<td><button type="button" onclick="event.stopPropagation();poEditDetail(' + i + ')">Edit</button></td>' +
                '<td>' + esc(r.parentItemName) + '</td>' +
                '<td>' + esc(r.itemName) + '</td>' +
                '<td>' + esc(r.cropYear) + '</td>' +
                '<td>' + esc(r.packingType) + '</td>' +
                '<td>' + esc(r.packUomCode) + '</td>' +
                '<td style="text-align:right;">' + fmt(r.itemQty) + '</td>' +
                '<td style="text-align:right;">' + fmt(r.itemWeight) + '</td>' +
                '<td style="text-align:right;">' + fmt(r.itemRate) + '</td>' +
                '<td>' + esc(r.rateUomCode) + '</td>' +
                '<td style="text-align:right;">' + fmt(r.itemAmount) + '</td>' +
                '<td>' + esc(r.taxName) + '</td>' +
                '<td style="text-align:right;">' + fmt(r.taxPercent) + '</td>' +
                '<td style="text-align:right;">' + fmt(r.taxAmount) + '</td>' +
                '<td style="text-align:right;">' + fmt(r.totalAmount) + '</td>' +
                '<td>' + esc(r.remarks) + '</td>';
            body.appendChild(tr);
        });
    }

    var t = detailTotals();
    $('totQty').textContent = fmt(t.qty);
    $('totWeight').textContent = fmt(t.weight);
    $('totAmount').textContent = fmt(t.amount);
    $('totTaxAmount').textContent = fmt(t.tax);
    $('totTotalAmount').textContent = fmt(t.total);

    if (selectedIdx >= detailRows.length) selectedIdx = Math.max(0, detailRows.length - 1);
    $('poNavTotal').textContent = detailRows.length;
    $('poNavCurrent').value = detailRows.length ? selectedIdx + 1 : 0;
}

function poNav(what, value) {
    if (!detailRows.length) return;
    if (what === 'first') selectedIdx = 0;
    else if (what === 'prev') selectedIdx = Math.max(0, selectedIdx - 1);
    else if (what === 'next') selectedIdx = Math.min(detailRows.length - 1, selectedIdx + 1);
    else if (what === 'last') selectedIdx = detailRows.length - 1;
    else if (what === 'goto') {
        var i = parseInt(value, 10) - 1;
        if (!isNaN(i) && i >= 0 && i < detailRows.length) selectedIdx = i;
    }
    renderDetail();
}

/* ------------------------------------------------- the other three tabs */

function poShowTab(ev, id) {
    if (ev) ev.preventDefault();
    ['tabDetail', 'tabExpense', 'tabEmptyBags', 'tabPayment'].forEach(function (t) {
        $(t).style.display = (t === id) ? '' : 'none';
    });
    Array.prototype.forEach.call(document.querySelectorAll('#poTabs a'), function (a) {
        a.classList.toggle('active', a.dataset.tab === id);
    });
}

/* grdInvExp (frmPurchaseOrderCmagt.cs:2029-2036): the ItemId column is a combo
   bound to the Other Item master but set EditType = 0 (NoEdit) - the grid is
   SEEDED with one row per other item (BARDANA, OTHER CHARGES, MUNSHIANA, SOTRI,
   LABOUR ...) and the user edits only Qty, Rate, Amount and Remarks. */
function seedExpenseRows() {
    var items = lookupData.otherItems || [];
    var existing = {};
    expenseRows.forEach(function (r) { existing[r.itemId] = r; });
    expenseRows = items.map(function (it) {
        var e = existing[it.Id];
        /* ReadById (:3485-3500) writes the saved row's Id, SoId and SoExpenseId back onto the
           seeded row, and Insert() re-sends SoId / SoExpenseId (:3053-3054). */
        return {
            purchaseOrderSupplierExpenseDetailId: e ? (e.purchaseOrderSupplierExpenseDetailId || 0) : 0,
            saleOrderMasterId: e ? (e.saleOrderMasterId || 0) : 0,
            saleOrderBuyerOtherExpenseDetailId: e ? (e.saleOrderBuyerOtherExpenseDetailId || 0) : 0,
            saleOrderNo: e ? (e.saleOrderNo || '') : '',
            itemId: it.Id,
            otherItemName: it.OtherItemName || it.ItemName,
            qty: e ? e.qty : 0,
            rate: e ? e.rate : 0,
            amount: e ? e.amount : 0,
            remarks: e ? e.remarks : ''
        };
    });
    renderExpense();
}

function renderExpense() {
    var body = $('grdInvExpBody');
    body.innerHTML = '';
    if (!expenseRows.length) { body.innerHTML = '<tr><td colspan="6">No other-item master rows returned.</td></tr>'; $('totExpense').textContent = '0'; return; }
    expenseRows.forEach(function (r, i) {
        var tr = document.createElement('tr');
        tr.innerHTML =
            '<td>' + esc(r.saleOrderNo || '') + '</td>' +
            '<td>' + esc(r.otherItemName) + '</td>' +
            '<td><input type="number" value="' + (r.qty || 0) + '" oninput="expenseRows[' + i + '].qty=parseFloat(this.value)||0;recalcExpense(' + i + ')"></td>' +
            '<td><input type="number" value="' + (r.rate || 0) + '" oninput="expenseRows[' + i + '].rate=parseFloat(this.value)||0;recalcExpense(' + i + ')"></td>' +
            '<td><input type="number" value="' + (r.amount || 0) + '" oninput="expenseRows[' + i + '].amount=parseFloat(this.value)||0;renderExpense()"></td>' +
            '<td><input type="text" value="' + esc(r.remarks) + '" oninput="expenseRows[' + i + '].remarks=this.value"></td>';
        body.appendChild(tr);
    });
    $('totExpense').textContent = fmt(expenseRows.reduce(function (s2, r) { return s2 + (+r.amount || 0); }, 0));
}

/* the persistent Sale Order Mapping panel (desktop panel17, :6523). Columns per
 * grdSaleOrderMappingSettings (:2597-2624); CurrentAllocationWeight is the only editable
 * cell and is what the save sends as itemNetWeight (:3254). */
function renderSaleOrderMapping() {
    var body = $('grdSaleOrderMappingBody');
    if (!body) return;
    body.innerHTML = '';
    if (!saleOrderMappings.length) {
        body.innerHTML = '<tr><td colspan="9">No sale orders mapped. Use Load So.</td></tr>';
        return;
    }
    saleOrderMappings.forEach(function (m, i) {
        var tr = document.createElement('tr');
        tr.innerHTML =
            '<td><button type="button" class="danger" onclick="poRemoveMapping(' + i + ')">X</button></td>' +
            '<td>' + esc(m.saleOrderNo || m.saleOrderMasterId) + '</td>' +
            '<td>' + esc(m.buyerName || m.buyerId) + '</td>' +
            '<td>' + esc(m.itemName || m.itemId) + '</td>' +
            '<td class="num">' + fmt(m.itemWeight) + '</td>' +
            '<td class="num">' + fmt(m.bookedWeight) + '</td>' +
            '<td class="num">' + fmt(m.balanceWeight) + '</td>' +
            '<td class="num"><input type="number" step="any" class="map-alloc" value="' + r3(m.itemNetWeight) +
                '" onchange="poMappingAllocChanged(' + i + ', this)"></td>' +
            '<td>' + esc(fmtDMY(m.validityDate)) + '</td>';
        body.appendChild(tr);
    });
}

/* grdSaleOrderMapping_CellUpdated (:2682-2704). Only enforced when the row knows its balance
 * (a row read back from a saved PO may not carry BalanceWeight). */
function poMappingAllocChanged(i, input) {
    var m = saleOrderMappings[i];
    if (!m) return;
    var v = parseFloat(input.value); if (isNaN(v)) v = 0;
    var bal = +m.balanceWeight || 0;
    if (m.hasBalance && v > bal) {
        alert('Current Allocation Weight:' + fmt(v) + " can't be greater than Balance weight:" + bal);
        v = bal;
        input.value = r3(v);
    }
    m.itemNetWeight = v;
}

var MONTHS = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
/* dd-MMM-yyyy [hh:mm tt] (desktop FormatString) in local time - never toISOString */
function fmtDMY(v, withTime) {
    if (v === undefined || v === null || v === '') return '';
    var raw = String(v), d = null;
    if (typeof v === 'number' || /T.*(Z|[+-]\d{2}:?\d{2})$/.test(raw)) d = new Date(v);
    else {
        var p = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2}))?/.exec(raw);
        if (p) d = new Date(+p[1], +p[2] - 1, +p[3], +(p[4] || 0), +(p[5] || 0));
    }
    if (!d || isNaN(d.getTime())) return raw;
    var out = ('0' + d.getDate()).slice(-2) + '-' + MONTHS[d.getMonth()] + '-' + d.getFullYear();
    if (withTime) {
        var h = d.getHours(), ap = h >= 12 ? 'PM' : 'AM'; h = h % 12 || 12;
        out += ' ' + ('0' + h).slice(-2) + ':' + ('0' + d.getMinutes()).slice(-2) + ' ' + ap;
    }
    return out;
}

/* DeleteSoMappingRow (:2644-2680): an unsaved row is simply dropped; a saved one is queued with
 * actionTypeId = 3 (lstRemoveRecordMappingDetail) so the procedure soft-deletes it. Splicing it
 * away, as before, left the stale mapping in the database after Update. */
var removedMappings = [];
function poRemoveMapping(i) {
    var m = saleOrderMappings[i];
    if (!m) return;
    if (!confirm('Are you sure to Delete?')) return;
    if (+m.purchaseOrderSaleOrderMappingId > 0) {
        removedMappings.push({
            purchaseOrderSaleOrderMappingId: +m.purchaseOrderSaleOrderMappingId,
            actionTypeId: 3,
            saleOrderMasterId: +m.saleOrderMasterId || 0,
            saleOrderDetailId: +m.saleOrderDetailId || 0,
            buyerId: +m.buyerId || 0,
            itemId: +m.itemId || 0,
            itemNetWeight: +m.itemNetWeight || 0
        });
    }
    saleOrderMappings.splice(i, 1);
    renderSaleOrderMapping();
}

/* amount = qty x rate on this grid only - that is what grdInvExp does on the
 * desktop (UpdateAmount), and it is a charge line, not a weight-priced item */
function recalcExpense(i) {
    var r = expenseRows[i];
    if (!r) return;
    r.amount = r3((+r.qty || 0) * (+r.rate || 0));
    renderExpense();
}

/* grdEmptyBags (frmPurchaseOrderCmagt.cs:2155-2168, AddRowsInvEmptyBagsGrid):
   the grid is SEEDED with one row per AllocatedPackingType - there is no add or
   delete - its PackingType column is NoEdit (:2200) because it identifies the
   seeded row, and only Weight Cut is editable. */
function seedEmptyBagRows() {
    var types = viewRows('AllocatedPackingType');
    var existing = {};
    emptyBagRows.forEach(function (r) { existing[r.packingTypeId] = r; });
    emptyBagRows = types.map(function (t) {
        var e = existing[t.Id];
        return {
            purchaseOrderEmptyBagDetailId: e ? e.purchaseOrderEmptyBagDetailId : 0,
            entryTypeId: 1,
            packingTypeId: t.Id,
            packingType: t.ReferenceName,
            weightCutKg: e ? e.weightCutKg : 0,
            saleOrderMasterId: e ? e.saleOrderMasterId : 0,
            saleOrderNo: e ? e.saleOrderNo : ''
        };
    });
    renderEmptyBags();
}

function poAddEmptyBagPmRow() {
    /* grdEmptyBagsPm: packing type + material item + rate, entryTypeId = 2,
       with its own X / + buttons (:2282-2283) */
    emptyBagPmRows.push({ purchaseOrderEmptyBagDetailId: 0, entryTypeId: 2, packingTypeId: 0, packingType: '', emptyBagPackingMaterialItemId: 0, emptyBagItem: '', rate: 0 });
    renderEmptyBagsPm();
}

/* Min/Max weight-cut range for a packing type, as InvPackingType carries it and
   the desktop checks it at save time (:3095-3105). */
function packingTypeRange(packingTypeId) {
    var rows = lookupData.packingTypes || [];
    for (var i = 0; i < rows.length; i++) {
        if (parseInt(rows[i].Id, 10) === parseInt(packingTypeId, 10)) {
            var mn = parseFloat(rows[i].MinEbWeight), mx = parseFloat(rows[i].MaxEbWeight);
            if (isFinite(mn) && isFinite(mx) && mx > 0) return { min: mn, max: mx, name: rows[i].PackTypeDesc };
            return null;
        }
    }
    return null;
}

function packingTypeOptions(selectedId) {
    var pt = lookupData.packingTypes || [];
    return '<option value="0">-- Select --</option>' + pt.map(function (p) {
        return '<option value="' + p.Id + '"' + (parseInt(p.Id, 10) === parseInt(selectedId, 10) ? ' selected' : '') + '>' + esc(p.PackTypeDesc) + '</option>';
    }).join('');
}

function viewComboOptions(activity, selectedId) {
    return '<option value="0">-- Select --</option>' + viewRows(activity).map(function (r) {
        return '<option value="' + r.Id + '"' + (parseInt(r.Id, 10) === parseInt(selectedId, 10) ? ' selected' : '') + '>' + esc(r.ReferenceName) + '</option>';
    }).join('');
}

function renderEmptyBags() {
    var body = $('grdEmptyBagsBody');
    if (!body) return;
    body.innerHTML = '';
    if (!emptyBagRows.length) { body.innerHTML = '<tr><td colspan="3">No packing types configured (AllocatedPackingType).</td></tr>'; return; }
    var showSo = emptyBagRows.some(function (r) { return r.saleOrderMasterId > 0; });
    emptyBagRows.forEach(function (r, i) {
        var tr = document.createElement('tr');
        /* packing type is read-only here: the row IS the packing type */
        tr.innerHTML =
            (showSo ? '<td>' + esc(r.saleOrderNo || '') + '</td>' : '') +
            '<td>' + esc(r.packingType) + '</td>' +
            '<td><input type="number" step="0.001" value="' + (r.weightCutKg || 0) + '" oninput="emptyBagRows[' + i + '].weightCutKg=parseFloat(this.value)||0"></td>';
        body.appendChild(tr);
    });
    var head = document.querySelector('#grdEmptyBags thead tr');
    if (head) {
        var hasSoCol = head.firstElementChild && head.firstElementChild.dataset.so === '1';
        if (showSo && !hasSoCol) {
            var th = document.createElement('th');
            th.textContent = 'Sale Order No'; th.dataset.so = '1'; th.style.width = '110px';
            head.insertBefore(th, head.firstElementChild);
        } else if (!showSo && hasSoCol) {
            head.removeChild(head.firstElementChild);
        }
    }
}

function renderEmptyBagsPm() {
    var body = $('grdEmptyBagsPmBody');
    if (!body) return;
    body.innerHTML = '';
    if (!emptyBagPmRows.length) { body.innerHTML = '<tr><td colspan="4">No packing-material rate rows.</td></tr>'; return; }
    var items = lookupData.emptyBagItems || [];
    emptyBagPmRows.forEach(function (r, i) {
        /* EmptyBagItem is a ValueList combo on the desktop (:2246-2250) */
        var itemOpts = '<option value="0">-- Select --</option>' + items.map(function (it) {
            return '<option value="' + it.ItemId + '"' + (parseInt(it.ItemId, 10) === parseInt(r.emptyBagPackingMaterialItemId, 10) ? ' selected' : '') + '>' + esc(it.ItemName) + '</option>';
        }).join('');
        var tr = document.createElement('tr');
        tr.innerHTML =
            '<td><button type="button" class="danger" onclick="emptyBagPmRows.splice(' + i + ',1);renderEmptyBagsPm();">X</button></td>' +
            '<td><select onchange="emptyBagPmRows[' + i + '].packingTypeId=parseInt(this.value,10)||0;emptyBagPmRows[' + i + '].packingType=this.selectedOptions[0].textContent">' + viewComboOptions('AllocatedPackingType', r.packingTypeId) + '</select></td>' +
            '<td><input type="number" step="0.001" value="' + (r.rate || 0) + '" oninput="emptyBagPmRows[' + i + '].rate=parseFloat(this.value)||0"></td>' +
            '<td><select onchange="emptyBagPmRows[' + i + '].emptyBagPackingMaterialItemId=parseInt(this.value,10)||0;emptyBagPmRows[' + i + '].emptyBagItem=this.selectedOptions[0].textContent">' + itemOpts + '</select></td>';
        body.appendChild(tr);
    });
}

/* NOTE: a SECOND copy of renderEmptyBagsPm() used to follow here and, being later in the
   file, was the one that actually ran. It drew Empty Bag Item as a free-text box and never
   set emptyBagPackingMaterialItemId, so the id stayed 0 - and Insert():3111 only keeps a
   packing-material row when `Conversion.ToInt(r4.Cells["EmptyBagItem"].Value) > 0`. Every
   packing-material rate row was therefore dropped at save time while the screen reported
   success. The duplicate is removed; the ValueList version above is the desktop's (:2246-2250). */

function poAddPaymentRow() {
    paymentRows.push({ purchaseOrderPaymentDetailId: 0, paymentTermId: 0, paymentTerm: '', dueDays: 0,
                       baseDueDateTypeId: 1, dueDate: '', pctOfTotal: 0, dueAmount: 0 });
    renderPayment();
}

function renderPayment() {
    var body = $('grdPaymentTermBody');
    body.innerHTML = '';
    if (!paymentRows.length) {
        body.innerHTML = '<tr><td colspan="7">No payment schedule rows. The header term is sent as one 100% row when this grid is empty.</td></tr>';
        $('totPct').textContent = '0'; $('totDue').textContent = '0';
        refreshScheduleDescription();
        return;
    }
    var terms = lookupData.paymentTerms || [];
    paymentRows.forEach(function (r, i) {
        var opts = '<option value="0">-- Select --</option>' + terms.map(function (t) {
            return '<option value="' + t.Id + '"' + (parseInt(t.Id, 10) === parseInt(r.paymentTermId, 10) ? ' selected' : '') + '>' + esc(t.TermsDescription) + '</option>';
        }).join('');
        /* BaseDueDateTypeId drives the saved due date: 1 = document date + due days,
           4 = the date typed in the row, anything else none (Insert():3176) */
        var baseOpts = viewComboOptions('PaymentBaseDate', r.baseDueDateTypeId);
        var tr = document.createElement('tr');
        tr.innerHTML =
            '<td><button type="button" class="danger" onclick="paymentRows.splice(' + i + ',1);renderPayment();">X</button></td>' +
            '<td><select onchange="paymentRows[' + i + '].paymentTermId=parseInt(this.value,10)||0;paymentRows[' + i + '].paymentTerm=this.selectedOptions[0].textContent;renderPayment()">' + opts + '</select></td>' +
            '<td><input type="number" value="' + (r.dueDays || 0) + '" onchange="poPayCell(' + i + ',\'DueDays\',this.value)"></td>' +
            '<td><input type="date" value="' + esc(r.dueDate || '') + '" ' + (parseInt(r.baseDueDateTypeId, 10) === 4 ? '' : 'readonly') + ' onchange="poPayCell(' + i + ',\'DueDate\',this.value)"></td>' +
            '<td><select onchange="paymentRows[' + i + '].baseDueDateTypeId=parseInt(this.value,10)||0;renderPayment()">' + baseOpts + '</select></td>' +
            '<td><input type="number" step="0.001" value="' + (r.pctOfTotal || 0) + '" onchange="poPayCell(' + i + ',\'%OfTotal\',this.value)"></td>' +
            '<td><input type="number" step="0.001" value="' + (r.dueAmount || 0) + '" onchange="poPayCell(' + i + ',\'Amount\',this.value)"></td>';
        body.appendChild(tr);
    });
    $('totPct').textContent = fmt(paymentRows.reduce(function (s2, r) { return s2 + (+r.pctOfTotal || 0); }, 0));
    $('totDue').textContent = fmt(paymentRows.reduce(function (s2, r) { return s2 + (+r.dueAmount || 0); }, 0));
    refreshScheduleDescription();
}

/* grdPaymentTerm_CellUpdated (:2479-2546), cell by cell. The base is Sum(detail TotalAmount),
 * tax included. %OfTotal is capped at 100 and drives Amount (rounded 4); Amount may not exceed
 * the order and drives %OfTotal (rounded 8); DueDays sets DueDate = doc date + days; a DueDate
 * before the doc date is reset to it, otherwise it sets DueDays. */
function poPayCell(i, key, value) {
    var r = paymentRows[i];
    if (!r) return;
    if (!detailRows.length) { message('No Detail Record Found', true); renderPayment(); return; }
    var total = detailTotals().total;
    var doc = val('datDocDate');
    if (key === '%OfTotal') {
        var pc = parseFloat(value) || 0;
        if (pc > 100) { message("%of Total Can't Greater than 100", true); pc = 100; }
        r.pctOfTotal = pc;
        r.dueAmount = Math.round(total * pc / 100 * 10000) / 10000;
    } else if (key === 'Amount') {
        var a = parseFloat(value) || 0;
        if (total < a) { message('Amount Cant be Greater than Order Amount:' + total, true); a = 0; }
        r.dueAmount = a;
        r.pctOfTotal = total ? Math.round(a * 100 / total * 1e8) / 1e8 : 0;
    } else if (key === 'DueDays') {
        r.dueDays = parseInt(value, 10) || 0;
        r.dueDate = addDays(doc, r.dueDays);
    } else if (key === 'DueDate') {
        if (value && doc && value < doc) {
            r.dueDate = doc;
            message("Due Date Can't less Than DocDate", true);
        } else if (value) {
            r.dueDate = value;
            r.dueDays = Math.round((new Date(value + 'T00:00:00') - new Date(doc + 'T00:00:00')) / 86400000);
        }
    }
    renderPayment();
}

function recalcPaymentDueDate(i) {
    var r = paymentRows[i];
    if (!r) return;
    if (parseInt(r.baseDueDateTypeId, 10) === 1) r.dueDate = addDays(val('datDocDate'), r.dueDays);
    renderPayment();
}

/* obj.paymentScheduleDescription, frmPurchaseOrderCmagt.cs:3209 - a generated
   summary of the rows. The desktop's box is disabled and filled from this, so it
   is never user input. */
function refreshScheduleDescription() {
    var rows = paymentRows.length ? paymentRows : buildPaymentRows();
    $('txtPaymentScheduleRemarks').value = rows.map(function (x) {
        return '[Term:' + (x.paymentTermId || 0) + ':' + (x.paymentTerm || '') +
               ',DueDays:' + (x.dueDays || 0) +
               ',%OfTotal:' + (x.pctOfTotal || 0) +
               ',DueAmount:' + (x.dueAmount || 0) +
               ',BaseDateType:' + (x.baseDueDateTypeId || 1) + ']';
    }).join(', ');
}

/* ------------------------------------------------------------ validation */

/* formvalidation(), frmPurchaseOrderCmagt.cs:2845-2887, in the desktop's order
 * and with the desktop's exact messages (including the "Paymrnt" typo). */
function ebTermId() {
    return parseInt((document.querySelector('input[name="ebTerm"]:checked') || {}).value || '0', 10) || 0;
}

function poValidate() {
    if (!intOf('cmbCommissionAgent')) return 'Please Select Commission Agent / Broker';
    if (!intOf('cmbSupplierName')) return 'Please Select Supplier Name';

    var scheduleTotal = paymentRows.reduce(function (s2, r) { return s2 + (+r.dueAmount || 0); }, 0);
    if (scheduleTotal === 0 && !intOf('cmbPaymentTerm')) return 'Please Select Paymrnt Term';
    if (intOf('cmbPaymentTerm') === 2 && intOf('txtDueDays') === 0) return 'Due Days field is required';

    if (!intOf('cmbDeliveryTerm')) return 'Please Select Delivery Term';
    if (intOf('txtDeliveryDays') === 0) return 'Delivery Days field is required';
    if (!detailRows.length) return 'Detail record not found. Please check!';

    /* per-detail-row required fields, :3020-3033 */
    for (var i = 0; i < detailRows.length; i++) {
        var d = detailRows[i], n = i + 1;
        if (!d.itemRate)    return 'Rate Field Required in row#' + n;
        if (!d.itemAmount)  return 'Amount Field Required in row#' + n;
        if (!d.totalAmount) return 'Total Amount Field Required in row#' + n;
    }

    /* Insert():3088-3105. Skipped entirely under "Bag FOC and Weight Cut Not Apply" (:3091) -
       which is the default term, so the old unconditional range check refused a new document
       with untouched zero weight cuts that the desktop saves. Otherwise the cut must be > 0 and
       within the range of a SUBSTITUTED packing type: 1, 2 and 5 validate against themselves,
       every other type against 2 (:3097-3102). */
    if (ebTermId() !== 3) {
        for (var j = 0; j < emptyBagRows.length; j++) {
            var e = emptyBagRows[j];
            var cut = +e.weightCutKg || 0;
            if (cut <= 0) return 'WeightCut filed required In Empty bags Grid...';
            var pt = parseInt(e.packingTypeId, 10) || 0;
            var range = packingTypeRange((pt === 1 || pt === 2 || pt === 5) ? pt : 2);
            if (range && (cut < range.min || cut > range.max)) {
                return 'Weight Cut Should be in Range of: ' + range.min + ' to ' + range.max + '\nFor Packing Type:' + range.name;
            }
        }
    }

    /* :3108-3123 - a packing-material row counts only when BOTH its item and its packing type
       are chosen (others are skipped, not refused); a counted row needs a rate. */
    for (var k = 0; k < emptyBagPmRows.length; k++) {
        var pm = emptyBagPmRows[k];
        if (!((+pm.emptyBagPackingMaterialItemId || 0) > 0 && (+pm.packingTypeId || 0) > 0)) continue;
        if (!((+pm.rate || 0) > 0)) return 'Rate filed required In Empty bags Pm Grid...';
    }

    /* per payment row, :3175-3190 */
    for (var m = 0; m < paymentRows.length; m++) {
        if (!paymentRows[m].paymentTermId) return 'Payment Term Required in row#' + (m + 1);
        if (parseInt(paymentRows[m].paymentTermId, 10) === 2 && !(paymentRows[m].dueDays > 0)) {
            return 'Due Days Required In case Of Credit row in row#' + (m + 1);
        }
    }

    /* schedule reconciliation, :3210-3223 - tolerances are the desktop's own */
    var rows = buildPaymentRows();
    var paid = rows.reduce(function (s2, r) { return s2 + (+r.dueAmount || 0); }, 0);
    var pct = Math.round(rows.reduce(function (s2, r) { return s2 + (+r.pctOfTotal || 0); }, 0) * 10000) / 10000;
    var detailSum = detailTotals().total;   /* DetailSumAmount += vd.TotalAmount (:3045) - tax included */
    if (Math.abs(paid - detailSum) > 0.3) {
        return 'Payment Detail Amount:' + fmt(paid) + ' Not Equal to Total Amount:' + fmt(detailSum);
    }
    if (Math.abs(100 - pct) > 0.01) return 'Payment Detail Total% not near to 100';

    return null;
}

/* ------------------------------------------------------------ persistence */

/* ---------------------------------------------------------------------------
 * The four *Description strings the master carries.
 *
 * frmPurchaseOrderCmagt.cs builds them inside the child loops - :3069 (supplier
 * expense), :3129/:3134 (empty bags, one per entry type) and :3209 (payment
 * schedule) - and they are real columns on the master, not decoration: a desktop
 * row always has them populated. The page was sending none, so every web-written
 * order had four empty columns a desktop-written one does not.
 *
 * The formats below are the desktop's own interpolated strings, character for
 * character, so the stored text matches what the desktop would have stored.
 * --------------------------------------------------------------------------- */
function expenseDescription(rows) {
    return rows.map(function (x) {
        return '[Item:' + (x.ItemId || x.itemId || 0) + ':' + (x.OtherItemName || x.otherItemName || '')
             + ', Qty:' + (x.Qty || x.qty || 0) + ',Rate:' + (x.rate || 0) + ',Amount:' + (x.amount || 0) + ']';
    }).join(', ');
}
function emptyBagDescription(rows) {
    return rows.map(function (x) {
        return '[PackingType:' + (x.packingTypeId || 0) + ':' + (x.packingType || '')
             + ', WeightCut:' + (x.weightCutKg || 0) + ']';
    }).join(', ');
}
function emptyBagPmDescription(rows) {
    return rows.map(function (x) {
        return '[PackingType:' + (x.packingTypeId || 0) + ':' + (x.packingType || '')
             + ',Rate:' + (x.rate || 0) + '],EmptyBagItem:' + (x.emptyBagItem || '');
    }).join(', ');
}
function paymentScheduleDescription(rows) {
    return rows.map(function (x) {
        return '[Term:' + (x.paymentTermId || 0) + ':' + (x.paymentTerm || '')
             + ',DueDays:' + (x.dueDays || 0)
             + ',%OfTotal:' + (x.pctOfTotal || 0)
             + ',DueAmount:' + (x.dueAmount || 0)
             + ',BaseDateType:' + (x.baseDueDateTypeId || 0) + ']';
    }).join(', ');
}

function buildPayload() {
    var recId = intOf('purchaseOrderMasterId');
    var docDate = val('datDocDate');

    /* FillDetailListCommonForInsertAndDelete (:2911-2932) - exactly the fields the desktop row
       carries. Removed saved rows go FIRST with actionTypeId 3 and only on an update (:3009-3015);
       grid rows follow, id kept only when RecId != 0 (:3019), actionTypeId = id <= 0 ? 1 : 2. */
    function detailOut(r, keepId) {
        var id = keepId ? (+r.purchaseOrderDetailId || 0) : 0;
        return {
            purchaseOrderDetailId: id,
            actionTypeId: id <= 0 ? 1 : 2,
            inventoryParentCategoryId: +r.inventoryParentCategoryId || 0,
            itemId: +r.itemId || 0,
            itemName: r.itemName || '',
            cropYearId: +r.cropYearId || 0,
            cropYear: r.cropYear || '',
            packingTypeId: +r.packingTypeId || 0,
            packUomId: +r.packUomId || 0,
            itemQty: +r.itemQty || 0,
            itemWeight: +r.itemWeight || 0,
            itemRate: +r.itemRate || 0,
            rateUomId: +r.rateUomId || 0,
            itemAmount: +r.itemAmount || 0,
            taxNameId: +r.taxNameId || 0,
            taxPercent: +r.taxPercent || 0,
            taxAmount: +r.taxAmount || 0,
            totalAmount: +r.totalAmount || 0,
            remarks: r.remarks || ''
        };
    }
    var details = [];
    if (recId > 0) {
        removedDetailRows.forEach(function (r) {
            var d = detailOut(r, true); d.actionTypeId = 3; details.push(d);
        });
    }
    detailRows.forEach(function (r) { details.push(detailOut(r, recId !== 0)); });

    /* :2995 then :3037-3040 - a blank header remark takes the detail rows' remarks in order. */
    var remarksHeader = val('txtRemarks');
    detailRows.forEach(function (r) { if (!String(remarksHeader || '').trim()) remarksHeader = r.remarks || ''; });

    /* grdInvExp (:3048-3065): ItemId != 0 AND Amount > 0; a blank or "0" remark becomes
       "Expense : <item>  Qty<qty>  @<rate>". */
    var expenses = expenseRows.filter(function (r) { return (+r.itemId || 0) !== 0 && (+r.amount || 0) > 0; })
        .map(function (r) {
            var rem = String(r.remarks || '').trim();
            if (rem === '0' || rem === '') rem = 'Expense : ' + (r.otherItemName || '') + '  Qty' + (+r.qty || 0) + '  @' + (+r.rate || 0);
            return {
                saleOrderMasterId: +r.saleOrderMasterId || 0,
                saleOrderBuyerOtherExpenseDetailId: +r.saleOrderBuyerOtherExpenseDetailId || 0,
                itemId: +r.itemId || 0,
                otherItemName: r.otherItemName || '',
                qty: +r.qty || 0,
                rate: +r.rate || 0,
                amount: +r.amount || 0,
                remarks: rem
            };
        });

    /* grdEmptyBags (:3088-3107): every row, entryTypeId 1. grdEmptyBagsPm (:3108-3125): only rows
       with both item and packing type, entryTypeId 2. */
    var ebRows = emptyBagRows.map(function (r) {
        return { packingTypeId: +r.packingTypeId || 0, packingType: r.packingType || '',
                 weightCutKg: +r.weightCutKg || 0, entryTypeId: 1 };
    });
    var ebPmRows = emptyBagPmRows.filter(function (r) {
        return (+r.emptyBagPackingMaterialItemId || 0) > 0 && (+r.packingTypeId || 0) > 0;
    }).map(function (r) {
        return { packingTypeId: +r.packingTypeId || 0, packingType: r.packingType || '', rate: +r.rate || 0,
                 emptyBagPackingMaterialItemId: +r.emptyBagPackingMaterialItemId || 0,
                 emptyBagItem: r.emptyBagItem || '', entryTypeId: 2 };
    });

    /* :3170-3177 - DueDate = doc date + DueDays for base type 1, the row's own date for type 4,
       otherwise none. */
    var payments = buildPaymentRows().map(function (r) {
        var base = parseInt(r.baseDueDateTypeId, 10) || 0;
        return {
            paymentTermId: +r.paymentTermId || 0,
            paymentTerm: r.paymentTerm || '',
            dueDays: +r.dueDays || 0,
            pctOfTotal: +r.pctOfTotal || 0,
            dueAmount: +r.dueAmount || 0,
            baseDueDateTypeId: base,
            dueDate: base === 1 ? addDays(docDate, r.dueDays) : (base === 4 ? (r.dueDate || null) : null)
        };
    });

    /* grdSaleOrderMapping (:3226-3272), then the removed saved rows appended after validation
       (:3302-3308). The server sets actionTypeId and, as the DAL does, the header and detail ids. */
    var mappings = saleOrderMappings.map(function (m) {
        return {
            purchaseOrderSaleOrderMappingId: +m.purchaseOrderSaleOrderMappingId || 0,
            saleOrderMasterId: +m.saleOrderMasterId || 0,
            saleOrderDetailId: +m.saleOrderDetailId || 0,
            buyerId: +m.buyerId || 0,
            itemId: +m.itemId || 0,
            itemName: m.itemName || '',
            itemNetWeight: +m.itemNetWeight || 0,
            validityDate: m.validityDate || ''
        };
    });
    if (recId > 0) mappings = mappings.concat(removedMappings);

    return {
        purchaseOrderMasterId: recId,
        documentTypeId: 1052,
        docNo: intOf('txtDocNo'),
        docDate: docDate,
        validityDate: val('datExpiryDate'),          /* desktop saves the computed expiry here */
        statusId: 1,
        commissionAgentId: intOf('cmbCommissionAgent'),
        supplierId: intOf('cmbSupplierName'),
        deliveryToPartyId: intOf('cmbDeliveryToParty'),
        shipToAddressId: intOf('cmbShipToAddress'),
        shipToAddress: val('txtShipToAddress'),
        deliveryTermId: intOf('cmbDeliveryTerm'),
        deliveryDays: intOf('txtDeliveryDays'),
        deliveryStartDate: val('datDeliveryStartDate'),
        remarksHeader: remarksHeader,
        companyId: intOf('cmbCompany'),
        branchId: intOf('cmbBranch'),
        isApproved: false,
        isWhtApplied: $('chkWithHoldingTaxApplied').checked,
        isSupplierOtherChargesAllowed: $('chkOtherExpenseAllowed').checked,
        ebWeightDeductionTermId: ebTermId(),         /* :3005 - 0 when no radio is set */

        paymentScheduleDescription: paymentScheduleDescription(payments),
        purchaseOrderSupplierExpenseDetailDescription: expenseDescription(expenses),
        purchaseOrderEmptyBagDetailDescription:   emptyBagDescription(ebRows),
        purchaseOrderEmptyBagDetailDescriptionII: emptyBagPmDescription(ebPmRows),

        purchaseOrderDetailList: details,
        purchaseOrderSupplierExpenseDetailList: expenses,
        purchaseOrderEmptyBagDetailList: ebRows.concat(ebPmRows),
        purchaseOrderCommissionDetailList: buildCommissionRows(),
        purchaseOrderPaymentDetailList: payments,
        purchaseOrderSaleOrderMappingList: mappings
    };
}

/* commission row agentTypeId = 1, brokerage row agentTypeId = 2; only sent
 * when an amount was produced and an account chosen (desktop :3135-3157) */
function buildCommissionRows() {
    var out = [];
    /* frmPurchaseOrderCmagt.cs:3136-3158 - commissionAgentId is the ACCOUNT combo's
       id (CmbCommissionAc / CmbBrokeryAc), not the header Commission Agent, and the
       row carries no separate account field. */
    if (num('txtCommAmount') > 0 && intOf('cmbCommissionAc')) {
        out.push({
            agentTypeId: 1,
            commissionAgentId: intOf('cmbCommissionAc'),
            commissionTypeId: intOf('cmbCommType'),
            commissionRate: num('txtCommRate'),
            rateUomId: intOf('cmbCommUom'),
            commissionAmount: num('txtCommAmount')
        });
    }
    if (num('txtBrokeryAmount') > 0 && intOf('cmbBrokeryAc')) {
        out.push({
            agentTypeId: 2,
            commissionAgentId: intOf('cmbBrokeryAc'),
            commissionTypeId: intOf('cmbBrokeryType'),
            commissionRate: num('txtBrokeryRate'),
            rateUomId: intOf('cmbBrokeryRateUom'),
            commissionAmount: num('txtBrokeryAmount')
        });
    }
    return out;
}

/* the desktop falls back to one synthesized 100% row from the header term when
 * the schedule grid is empty (:3158-3208) */
function buildPaymentRows() {
    if (paymentRows.length) return paymentRows;
    if (!intOf('cmbPaymentTerm')) return [];
    /* :3193-3208 - one synthesized row: base type 1, due date = doc date + due days,
       100% of total, amount = the detail sum */
    var t = detailTotals();
    var due = addDays(val('datDocDate'), intOf('txtDueDays'));
    return [{
        purchaseOrderPaymentDetailId: 0,
        paymentTermId: intOf('cmbPaymentTerm'),
        paymentTerm: textOf('cmbPaymentTerm'),
        dueDays: intOf('txtDueDays'),
        baseDueDateTypeId: 1,
        dueDate: due,
        pctOfTotal: 100,
        dueAmount: t.total           /* psD.dueAmount = DetailSumAmount = Sum(TotalAmount) (:3202) */
    }];
}

/* Sale Order mapping - populated by the Load So picker. Empty until that
 * endpoint exists; the rows carry the real source ids, never a typed number. */
var saleOrderMappings = [];

function poSave() {
    if (busy || !poGuard('save')) return;
    var err = poValidate();
    if (err) { poRelease('save'); message(err, true); return; }

    var isUpdate = intOf('purchaseOrderMasterId') > 0;
    if (!confirm(isUpdate ? 'Are you sure to Update?' : 'Are you sure to Save?')) { poRelease('save'); return; }

    setBusy(true, isUpdate ? 'btnUpdate' : (poMode === 'saveas' ? 'btnSaveAs' : 'btnSave'));
    message('');
    fetch(API + '/save', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(buildPayload())
    })
        .then(function (r) { return r.json().catch(function () { return { success: r.ok }; }); })
        .then(function (data) {
            if (data && (data.success === true || data.status === 'SUCCESS')) {
                if (data.id) $('purchaseOrderMasterId').value = data.id;
                message(isUpdate ? 'Update Successfully' : 'Save Successfully');
                removedDetailRows = []; removedMappings = [];
                if ($('chkPreview') && $('chkPreview').checked) window.print();   /* chkPrint, :3332 */
                poLoad(data.id || intOf('purchaseOrderMasterId'));
            } else {
                message((data && (data.message || data.error)) || 'The server rejected the save.', true);
            }
        })
        .catch(function (e) { message('Save failed: ' + e.message, true); })
        .finally(function () { setBusy(false); poRelease('save'); });
}

function poLoad(id) {
    if (!id) return Promise.resolve();
    setBusy(true);
    return getJson(API + '/' + id)
        .then(function (resp) {
            /* GET /{id} answers { status, data } - the header and its seven lists are under data.
               The page used to read the wrapper itself, so every field loaded blank. */
            var po = resp && resp.data ? resp.data : null;
            if (!po || (resp && resp.status === 'ERROR')) { message('Record Not Found', true); return; }
            /* Keys are the procedure's column names (DeliveryToPartyId, ValidityDate,
               EBWeightDeductionTermId, ItemAmount ...), so every read goes through col(). */
            $('purchaseOrderMasterId').value = colInt(po, 'purchaseOrderMasterId') || id;
            $('txtDocNo').value = colStr(po, 'docNo');
            $('datDocDate').value = dstr(col(po, 'docDate'));
            $('cmbCommissionAgent').value = colInt(po, 'commissionAgentId');
            $('cmbSupplierName').value = colInt(po, 'supplierId');
            $('cmbDeliveryToParty').value = colInt(po, 'DeliveryToPartyId');
            poDeliveryPartyChanged();
            if (colInt(po, 'shipToAddressId') > 0) $('cmbShipToAddress').value = colInt(po, 'shipToAddressId');
            $('txtShipToAddress').value = colStr(po, 'ShipToAddress');
            $('cmbDeliveryTerm').value = colInt(po, 'deliveryTermId');
            $('txtDeliveryDays').value = colInt(po, 'deliveryDays');
            $('datDeliveryStartDate').value = dstr(col(po, 'deliveryStartDate'));
            $('datExpiryDate').value = dstr(col(po, 'ValidityDate'));
            $('txtRemarks').value = colStr(po, 'remarksHeader');
            $('chkWithHoldingTaxApplied').checked = !!col(po, 'isWhtApplied');
            $('chkOtherExpenseAllowed').checked = !!col(po, 'isSupplierOtherChargesAllowed');

            /* ReadById :3421-3448 - commission (agentTypeId 1) and brokery (2) rows go back into
               their header controls. Not restoring them meant an Update re-sent nothing, and
               the procedure (which deletes the commission rows on update) lost them. */
            ['txtCommRate', 'txtCommAmount', 'txtBrokeryRate', 'txtBrokeryAmount'].forEach(function (x) { $(x).value = 0; });
            (col(po, 'purchaseOrderCommissionDetailList') || []).forEach(function (c) {
                var t = colInt(c, 'agentTypeId');
                var ids = t === 1 ? ['cmbCommissionAc', 'cmbCommType', 'txtCommRate', 'cmbCommUom', 'txtCommAmount']
                        : t === 2 ? ['cmbBrokeryAc', 'cmbBrokeryType', 'txtBrokeryRate', 'cmbBrokeryRateUom', 'txtBrokeryAmount'] : null;
                if (!ids) return;
                $(ids[0]).value = colInt(c, 'commissionAgentId');
                $(ids[1]).value = colInt(c, 'commissionTypeId');
                $(ids[2]).value = colNum(c, 'commissionRate');
                if (colInt(c, 'rateUomId') > 0) $(ids[3]).value = colInt(c, 'rateUomId');
                $(ids[4]).value = colNum(c, 'commissionAmount');
            });

            var eb = colInt(po, 'EBWeightDeductionTermId');
            Array.prototype.forEach.call(document.querySelectorAll('input[name="ebTerm"]'), function (rb) {
                rb.checked = parseInt(rb.value, 10) === eb;
            });

            detailRows = (col(po, 'purchaseOrderDetailList') || []).map(function (d) {
                return {
                    purchaseOrderDetailId: colInt(d, 'purchaseOrderDetailId'),
                    actionTypeId: 0,
                    inventoryParentCategoryId: colInt(d, 'inventoryParentCategoryId'),
                    parentItemName: colStr(d, 'inventoryParentCategory'),
                    itemId: colInt(d, 'itemId'),
                    itemName: colStr(d, 'ItemName'),
                    cropYearId: colInt(d, 'cropYearId'),
                    cropYear: colStr(d, 'cropYear'),
                    packingTypeId: colInt(d, 'packingTypeId'),
                    packingType: colStr(d, 'PackingType'),
                    packUomId: colInt(d, 'packUomId'),
                    packUomCode: colStr(d, 'PackUomCode'),
                    packUomEquivalent: colNum(d, 'PackUomEquivalent'),
                    itemQty: colNum(d, 'itemQty'),
                    itemWeight: colNum(d, 'itemWeight'),
                    itemRate: colNum(d, 'itemRate'),
                    rateUomId: colInt(d, 'rateUomId'),
                    rateUomCode: colStr(d, 'RateUomCode'),
                    itemAmount: colNum(d, 'ItemAmount'),
                    taxNameId: colInt(d, 'TaxNameId'),
                    taxName: colStr(d, 'TaxName'),
                    taxPercent: colNum(d, 'TaxPercent'),
                    taxAmount: colNum(d, 'TaxAmount'),
                    totalAmount: colNum(d, 'TotalAmount'),
                    remarks: colStr(d, 'remarks')
                };
            });
            expenseRows = (col(po, 'purchaseOrderSupplierExpenseDetailList') || []).map(function (x) {
                return {
                    purchaseOrderSupplierExpenseDetailId: colInt(x, 'purchaseOrderSupplierExpenseDetailId'),
                    saleOrderMasterId: colInt(x, 'saleOrderMasterId'),
                    saleOrderBuyerOtherExpenseDetailId: colInt(x, 'saleOrderBuyerOtherExpenseDetailId'),
                    saleOrderNo: colStr(x, 'SaleOrderNo'),
                    itemId: colInt(x, 'ItemId'),
                    otherItemName: colStr(x, 'OtherItemName'),
                    qty: colNum(x, 'Qty'),
                    rate: colNum(x, 'rate'),
                    amount: colNum(x, 'amount'),
                    remarks: colStr(x, 'remarks')
                };
            });
            /* :3469-3484 - the two empty-bag grids show the SAVED rows, split by entryTypeId; they
               are not re-seeded from the allocated packing types. */
            var allBags = (col(po, 'purchaseOrderEmptyBagDetailList') || []).map(function (b) {
                return {
                    purchaseOrderEmptyBagDetailId: colInt(b, 'purchaseOrderEmptyBagDetailId'),
                    entryTypeId: colInt(b, 'entryTypeId'),
                    packingTypeId: colInt(b, 'PackingTypeId'),
                    packingType: colStr(b, 'PackingType'),
                    weightCutKg: colNum(b, 'weightCutKg'),
                    rate: colNum(b, 'Rate'),
                    emptyBagPackingMaterialItemId: colInt(b, 'emptyBagPackingMaterialItemId'),
                    emptyBagItem: colStr(b, 'EmptyBagItem'),
                    saleOrderMasterId: colInt(b, 'saleOrderMasterId'),
                    saleOrderNo: colStr(b, 'SaleOrderNo')
                };
            });
            emptyBagRows   = allBags.filter(function (b) { return b.entryTypeId === 1; });
            emptyBagPmRows = allBags.filter(function (b) { return b.entryTypeId === 2; });
            paymentRows = (col(po, 'purchaseOrderPaymentDetailList') || []).map(function (x) {
                return {
                    purchaseOrderPaymentDetailId: colInt(x, 'purchaseOrderPaymentDetailId'),
                    paymentTermId: colInt(x, 'PaymentTermId'),
                    paymentTerm: colStr(x, 'PaymentTerm'),
                    dueDays: colInt(x, 'DueDays'),
                    pctOfTotal: colNum(x, 'pctOfTotal'),
                    dueAmount: colNum(x, 'dueAmount'),
                    baseDueDateTypeId: colInt(x, 'BaseDueDateTypeId'),
                    dueDate: dstr(col(x, 'DueDate'))
                };
            });
            /* :3505-3510 - a single schedule row also goes back into the header term / due days. */
            if (paymentRows.length === 1) {
                $('cmbPaymentTerm').value = paymentRows[0].paymentTermId;
                $('txtDueDays').value = paymentRows[0].dueDays;
            }
            saleOrderMappings = (col(po, 'purchaseOrderSaleOrderMappingList') || []).map(function (m) {
                return {
                    purchaseOrderSaleOrderMappingId: colInt(m, 'purchaseOrderSaleOrderMappingId'),
                    saleOrderMasterId: colInt(m, 'saleOrderMasterId'),
                    saleOrderDetailId: colInt(m, 'saleOrderDetailId'),
                    saleOrderNo: colStr(m, 'SaleOrderNo'),
                    buyerId: colInt(m, 'buyerId'),
                    buyerName: colStr(m, 'BuyerName'),
                    itemId: colInt(m, 'itemId'),
                    itemName: colStr(m, 'ItemName'),
                    itemNetWeight: colNum(m, 'itemNetWeight'),
                    itemWeight: colNum(m, 'ItemWeight'),
                    bookedWeight: colNum(m, 'BookedWeight'),
                    balanceWeight: colNum(m, 'BalanceWeight'),
                    hasBalance: col(m, 'BalanceWeight') !== undefined && col(m, 'BalanceWeight') !== null,
                    validityDate: dstr(col(m, 'ValidityDate'))
                };
            });
            removedDetailRows = [];
            removedMappings = [];

            seedExpenseRows();
            renderDetail(); renderEmptyBags(); renderEmptyBagsPm(); renderPayment(); renderSaleOrderMapping();
            $('btnUpdate').disabled = false;
            $('btnDelete').disabled = false;
            $('btnSave').disabled = true;
            poSetMode('edit');
            poMainTab(null, 'tabForm');
        })
        .catch(function (e) { message('Load failed: ' + e.message, true); })
        .finally(function () { setBusy(false); });
}

function poDelete() {
    if (busy || !poGuard('delete')) return;
    var id = intOf('purchaseOrderMasterId');
    if (!id) { poRelease('delete'); message('No record found to Delete', true); return; }
    if (!confirm('Are you sure to Delete?')) { poRelease('delete'); return; }
    setBusy(true, 'btnDelete');
    fetch(API + '/' + id, { method: 'DELETE' })
        .then(function (r) { return r.json().catch(function () { return { success: r.ok }; }); })
        .then(function (d) {
            if (d && d.success) { message('Delete Record Successfully'); poNew(); }
            else message((d && d.message) || 'Delete was refused.', true);
        })
        .catch(function (e) { message('Delete failed: ' + e.message, true); })
        .finally(function () { setBusy(false); poRelease('delete'); });
}

/* ----------------------------------------------------------------- history
 * HistoryFill (:3919-4056). The From/To pair applies to the date the radio names
 * (drdocdate / rdentrydate / rdmodifydate); Validity From/To only when filled (their
 * check boxes default off); parent categories as a comma list of ids; the rest by id,
 * except Deliver To Address which the desktop sends as the combo TEXT (:3990). */
function poHistSelected(id) {
    var e = $(id); if (!e) return '';
    return Array.prototype.filter.call(e.options, function (o) { return o.selected && o.value !== '0'; })
        .map(function (o) { return o.value; }).join(',');
}
function poLoadHistory() {
    if (busy || !poGuard('history')) return;
    setBusy(true, 'btnHistory');
    var dt = (document.querySelector('input[name="histDateType"]:checked') || {}).value || 'doc';
    var shipText = intOf('histDeliverToAddress') ? textOf('histDeliverToAddress') : '';
    var parents = poHistSelected('histParentItems');
    var q = '?fromDate=' + encodeURIComponent(val('histFromDate')) + '&toDate=' + encodeURIComponent(val('histToDate'))
        + '&dateType=' + dt
        + '&validityFrom=' + encodeURIComponent(val('histValidityFrom'))
        + '&validityTo=' + encodeURIComponent(val('histValidityTo'))
        + (intOf('histCommissionAgent') ? '&commissionAgentId=' + intOf('histCommissionAgent') : '')
        + (intOf('histSupplier') ? '&supplierId=' + intOf('histSupplier') : '')
        + (intOf('histItem') ? '&itemId=' + intOf('histItem') : '')
        + (parents ? '&parentItemIds=' + encodeURIComponent(parents + ',') : '')
        + (intOf('histDeliveryToParty') ? '&deliveryToPartyId=' + intOf('histDeliveryToParty') : '')
        + (shipText ? '&shipToAddress=' + encodeURIComponent(shipText) : '');
    getJson(API + '/history' + q)
        .then(function (rows) {
            var body = $('grdHistoryBody');
            body.innerHTML = '';
            if (!rows || !rows.length) { body.innerHTML = '<tr><td colspan="20">No documents in this range.</td></tr>'; return; }
            rows.forEach(function (r) {
                var id = colInt(r, 'purchaseOrderMasterId');
                var wht = col(r, 'isWhtApplied');
                var tr = document.createElement('tr');
                tr.ondblclick = function () { poLoad(id); };
                tr.onclick = function () {
                    Array.prototype.forEach.call(body.querySelectorAll('tr.hsel'), function (x) { x.classList.remove('hsel'); });
                    tr.classList.add('hsel');
                    poHistShowChildren(id);
                };
                tr.innerHTML =
                    '<td><button type="button" onclick="poLoad(' + id + ')">Edit</button></td>' +
                    '<td><button type="button" onclick="poHistSaveAs(' + id + ')">SaveAs</button></td>' +
                    '<td><button type="button" onclick="poHistPrint(' + id + ')">Print</button></td>' +
                    '<td><a href="#" onclick="event.preventDefault();poLoad(' + id + ')">' + esc(colStr(r, 'docNo')) + '</a></td>' +
                    '<td>' + esc(fmtDMY(col(r, 'docDate'))) + '</td>' +
                    '<td>' + esc(colStr(r, 'CommissionAgentName')) + '</td>' +
                    '<td>' + esc(colStr(r, 'SupplierName')) + '</td>' +
                    '<td>' + esc(colStr(r, 'DeliveryToPartyName')) + '</td>' +
                    '<td>' + esc(colStr(r, 'ShipToAddress')) + '</td>' +
                    '<td>' + esc(colStr(r, 'DeliveryTerm')) + '</td>' +
                    '<td>' + esc(fmtDMY(col(r, 'deliveryStartDate'))) + '</td>' +
                    '<td class="num">' + colInt(r, 'deliveryDays') + '</td>' +
                    '<td>' + esc(fmtDMY(col(r, 'ValidityDate'))) + '</td>' +
                    '<td style="text-align:center;"><input type="checkbox" disabled' + ((wht === true || wht === 1 || wht === '1' || wht === 'true') ? ' checked' : '') + '></td>' +
                    '<td>' + esc(colStr(r, 'Status')) + '</td>' +
                    '<td>' + esc(colStr(r, 'EntryUserName')) + '</td>' +
                    '<td>' + esc(fmtDMY(col(r, 'EntryDate'), true)) + '</td>' +
                    '<td>' + esc(colStr(r, 'ModifyUserName')) + '</td>' +
                    '<td>' + esc(fmtDMY(col(r, 'ModifyDate'), true)) + '</td>' +
                    '<td class="num">' + colInt(r, 'NoOfAttachments') + '</td>';
                body.appendChild(tr);
            });
        })
        .catch(function (e) { message('History failed: ' + e.message, true); })
        .finally(function () { setBusy(false); poRelease('history'); });
}

/* DataGridHistory_SaveAs (:4058-4078): read, then present as new - doc date today, Sale Order
 * mapping cleared; btnSaveAs_Click saves with RecId = 0. */
function poHistSaveAs(id) {
    poLoad(id).then(function () {
        if (intOf('purchaseOrderMasterId') !== id) return;
        $('purchaseOrderMasterId').value = 0;
        $('datDocDate').value = ymd(new Date());
        saleOrderMappings = []; removedMappings = []; removedDetailRows = [];
        renderSaleOrderMapping();
        $('btnSave').disabled = false; $('btnUpdate').disabled = true; $('btnDelete').disabled = true;
        poSetMode('saveas');
        message('Save As: press Save to create a new document from this one.');
    });
}
function poHistPrint(id) { poLoad(id).then(function () { if (intOf('purchaseOrderMasterId') === id) window.print(); }); }

/* HistoryComboDbCall + HistoryComboBind / BindDropdownsAgainstParentCategory */
var historyComboRows = [];
function poHistRefreshCombos() {
    var b = $('btnHistRefresh'); if (b) b.disabled = true;
    return getJson(API + '/history-combos')
        .then(function (rows) {
            historyComboRows = rows || [];
            var parents = [], seen = {}, most = 0;
            historyComboRows.forEach(function (r) {
                var act = colStr(r, 'Activity');
                if (act === 'ParentCategories' && !seen[colInt(r, 'Id')]) { seen[colInt(r, 'Id')] = 1; parents.push({ Id: colInt(r, 'Id'), Name: colStr(r, 'ReferenceName') }); }
                if (act === 'GetMostUsedParentCategoryId' && !most) most = colInt(r, 'Id');
            });
            var ps = $('histParentItems');
            var keep = poHistSelected('histParentItems');
            ps.innerHTML = '';
            parents.forEach(function (p) {
                var o = document.createElement('option'); o.value = p.Id; o.textContent = p.Name;
                o.selected = keep ? (',' + keep + ',').indexOf(',' + p.Id + ',') >= 0 : p.Id === most;
                ps.appendChild(o);
            });
            poHistParentChanged();
        })
        .catch(function (e) { message('History combos failed: ' + e.message, true); })
        .finally(function () { if (b) b.disabled = false; });
}
function poHistParentChanged() {
    var sel = poHistSelected('histParentItems');
    var set = sel ? (',' + sel + ',') : null;
    var buckets = { CommissionAgent: [], SupplierName: [], Item: [], DeliveryToParty: [], DeliverToAddress: [] };
    var seen = {};
    historyComboRows.forEach(function (r) {
        var act = colStr(r, 'Activity');
        if (!buckets[act]) return;
        if (set && set.indexOf(',' + colInt(r, 'ParentCategoryId') + ',') < 0) return;
        var key = act + '|' + colInt(r, 'Id');
        if (seen[key]) return;
        seen[key] = 1;
        buckets[act].push({ Id: colInt(r, 'Id'), Name: colStr(r, 'ReferenceName') });
    });
    fillSelect('histCommissionAgent', buckets.CommissionAgent, 'Id', 'Name');
    fillSelect('histSupplier', buckets.SupplierName, 'Id', 'Name');
    fillSelect('histItem', buckets.Item, 'Id', 'Name');
    fillSelect('histDeliveryToParty', buckets.DeliveryToParty, 'Id', 'Name');
    fillSelect('histDeliverToAddress', buckets.DeliverToAddress, 'Id', 'Name');
}
/* btnNewHistory_Click (:3650): clears Item, Commission Agent and Supplier only */
function poHistNew() {
    ['histItem', 'histCommissionAgent', 'histSupplier'].forEach(function (id) { if ($(id)) $(id).value = '0'; });
}
function poToggleFull(id) { var e = $(id); if (e) e.classList.toggle('po-full'); }

/* runs an action with its button disabled until the promise settles (success or failure) */
function withButton(btnId, fn) {
    var b = $(btnId);
    if (b && b.disabled) return Promise.resolve();
    if (b) { b.disabled = true; b.classList.add('btn-busy'); }
    var p;
    try { p = Promise.resolve(fn()); } catch (e) { p = Promise.reject(e); }
    return p.catch(function (e) { message(e.message || String(e), true); })
        .finally(function () { if (b) { b.disabled = false; b.classList.remove('btn-busy'); } });
}

/* --------------------------------------------------------------- toolbar */

function poNew() {
    $('purchaseOrderMasterId').value = 0;
    $('txtDocNo').value = '';
    detailRows = []; expenseRows = []; emptyBagRows = []; emptyBagPmRows = []; paymentRows = [];
    saleOrderMappings = []; removedDetailRows = []; removedMappings = [];
    selectedIdx = 0; editingIdx = -1;
    ['txtShipToAddress', 'txtRemarks', 'txtPaymentScheduleRemarks', 'txtRemarksDetail'].forEach(function (id) { $(id).value = ''; });
    ['txtCommRate', 'txtCommAmount', 'txtBrokeryRate', 'txtBrokeryAmount'].forEach(function (id) { $(id).value = 0; });
    clearDetailEntry();
    var today = ymd(new Date());
    $('datDocDate').value = today;
    $('datDeliveryStartDate').value = today;
    $('txtDeliveryDays').value = 1;
    poCalculateExpiryDate();
    seedExpenseRows(); seedEmptyBagRows();
    renderDetail(); renderEmptyBagsPm(); renderPayment(); renderSaleOrderMapping();
    $('btnSave').disabled = false;
    $('btnUpdate').disabled = true;
    $('btnDelete').disabled = true;
    poSetMode('new');
    message('');
    getJson(API + '/generate-no')
        .then(function (d) { $('txtDocNo').value = (d && (d.docNo || d.DocNo)) || ''; })
        .catch(function () { /* the server allocates the real number at save time anyway */ });
}

function poRefresh() { setBusy(true); loadLookups().finally(function () { setBusy(false); }); }

/* ------------------------------------------------------ Load So (frmLoadSaleIrderForPO)
 * btnLoadSo_Click (:4651-4693) pre-sets the loader's Delivery To Party and Ship To text from
 * this form and passes the distinct items already in the detail grid; the loader lists
 * USP_saleOrderMaster_PendingDataLoader rows with a check column; Load refuses rows whose
 * item is not in that list (:447-496); LoadInGridDetail (:4695-4733) sets this form's
 * Delivery To Party from the first row and appends rows not already mapped (by SoDetailId),
 * CurrentAllocationWeight = BalanceWeight. */
var loadSoRows = [];
var loadSoCombosLoaded = false;
function poLoadSaleOrder() {
    withButton('btnLoadSo', function () {
        var d = new Date(), from = new Date(d.getFullYear(), d.getMonth(), d.getDate() - 7);   /* Load: Now - 7 days */
        $('lsFromDate').value = ymd(from);
        $('lsToDate').value = ymd(d);
        $('lsDocNoFrom').value = ''; $('lsDocNoTo').value = '';
        $('loadSoModal').style.display = 'flex';
        var chain = loadSoCombosLoaded ? Promise.resolve() : poLoadSoRefreshCombos();
        return chain.then(function () {
            var party = intOf('cmbDeliveryToParty');
            if (party > 0) $('lsDeliveryToParty').value = party;
            var ship = (intOf('cmbShipToAddress') ? textOf('cmbShipToAddress') : '') || val('txtShipToAddress');
            var ls = $('lsShipToAddress');
            ls.value = '0';
            if (ship) {
                var hit = false;
                Array.prototype.forEach.call(ls.options, function (o) { if (!hit && o.textContent === ship) { ls.value = o.value; hit = true; } });
                if (!hit) { var o = document.createElement('option'); o.value = '-1'; o.textContent = ship; ls.appendChild(o); ls.value = '-1'; }
            }
            return poLoadSoFetch();
        });
    });
}
function poLoadSoClose() { $('loadSoModal').style.display = 'none'; }
function poLoadSoRefreshCombos() {
    return getJson(API + '/load-so/combos').then(function (rows) {
        var b = { CommissionAgent: [], buyerName: [], Item: [], DeliveryToPartyName: [], DeliverToAddress: [] }, seen = {};
        (rows || []).forEach(function (r) {
            var act = colStr(r, 'Activity'); if (!b[act]) return;
            var key = act + '|' + colInt(r, 'Id') + (act === 'DeliverToAddress' ? '|' + colStr(r, 'ReferenceName') : '');
            if (seen[key]) return; seen[key] = 1;
            b[act].push({ Id: colInt(r, 'Id'), Name: colStr(r, 'ReferenceName') });
        });
        fillSelect('lsCommissionAgent', b.CommissionAgent, 'Id', 'Name');
        fillSelect('lsBuyer', b.buyerName, 'Id', 'Name');
        fillSelect('lsItem', b.Item, 'Id', 'Name');
        fillSelect('lsDeliveryToParty', b.DeliveryToPartyName, 'Id', 'Name');
        fillSelect('lsShipToAddress', b.DeliverToAddress, 'Id', 'Name', '');   /* matched by TEXT, no default row */
        loadSoCombosLoaded = true;
    });
}
function poLoadSoRefreshCombos_click() { return withButton('btnLsRefresh', poLoadSoRefreshCombos); }
function poLoadSoFetch() {
    var ls = $('lsShipToAddress');
    var ship = ls && ls.value !== '0' && ls.selectedIndex >= 0 ? textOf('lsShipToAddress') : '';
    var q = '?fromDate=' + encodeURIComponent(val('lsFromDate')) + '&toDate=' + encodeURIComponent(val('lsToDate'))
        + (intOf('lsDocNoFrom') ? '&fromDocNo=' + intOf('lsDocNoFrom') : '')
        + (intOf('lsDocNoTo') ? '&toDocNo=' + intOf('lsDocNoTo') : '')
        + (intOf('lsCommissionAgent') > 0 ? '&commissionAgentId=' + intOf('lsCommissionAgent') : '')
        + (intOf('lsBuyer') > 0 ? '&buyerId=' + intOf('lsBuyer') : '')
        + (intOf('lsItem') > 0 ? '&itemId=' + intOf('lsItem') : '')
        + (intOf('lsDeliveryToParty') > 0 ? '&deliveryToPartyId=' + intOf('lsDeliveryToParty') : '')
        + (ship ? '&shipToAddress=' + encodeURIComponent(ship) : '');
    return getJson(API + '/load-so/pending' + q).then(function (rows) {
        loadSoRows = rows || [];
        var body = $('grdLoadSoBody');
        body.innerHTML = '';
        $('lsCheckAll').checked = false;
        if (!loadSoRows.length) { body.innerHTML = '<tr><td colspan="22">No pending sale orders.</td></tr>'; return; }
        var today = ymd(new Date());
        loadSoRows.forEach(function (r, i) {
            var exp = col(r, 'ValidityDate');
            var tr = document.createElement('tr');
            tr.innerHTML =
                '<td><input type="checkbox" class="ls-chk" data-i="' + i + '"></td>' +
                '<td>' + esc(colStr(r, 'docNo')) + '</td>' +
                '<td>' + esc(fmtDMY(col(r, 'docDate'))) + '</td>' +
                '<td>' + esc(colStr(r, 'CommissionAgentName')) + '</td>' +
                '<td>' + esc(colStr(r, 'buyerName')) + '</td>' +
                '<td>' + esc(colStr(r, 'DeliveryToPartyName')) + '</td>' +
                '<td>' + esc(colStr(r, 'ShipToAddress')) + '</td>' +
                '<td' + (dstr(exp) === today ? ' class="expiry-today"' : '') + '>' + esc(fmtDMY(exp)) + '</td>' +
                '<td>' + esc(colStr(r, 'ItemName')) + '</td>' +
                '<td class="num">' + fmt(colNum(r, 'itemQty')) + '</td>' +
                '<td class="num">' + fmt(colNum(r, 'QtyUsedInPo')) + '</td>' +
                '<td class="num">' + fmt(colNum(r, 'BalanceQty')) + '</td>' +
                '<td class="num">' + fmt(colNum(r, 'itemWeight')) + '</td>' +
                '<td class="num">' + fmt(colNum(r, 'WeightUsedInPo')) + '</td>' +
                '<td class="num">' + fmt(colNum(r, 'BalanceWeight')) + '</td>' +
                '<td class="num">' + fmt(colNum(r, 'itemRate')) + '</td>' +
                '<td>' + esc(colStr(r, 'EntryUserName')) + '</td>' +
                '<td>' + esc(fmtDMY(col(r, 'EntryDate'), true)) + '</td>' +
                '<td>' + esc(fmtDMY(col(r, 'ModifyDate'), true)) + '</td>' +
                '<td>' + esc(colStr(r, 'ModifyUserName')) + '</td>' +
                '<td>' + esc(colStr(r, 'RemarksHeader')) + '</td>' +
                '<td class="num">' + colInt(r, 'NoOfAttachments') + '</td>';
            body.appendChild(tr);
        });
    });
}
function poLoadSoShow() { withButton('btnLsShow', poLoadSoFetch); }
/* btnReset_Click (:417): From = active year start, doc nos and buyer cleared, reload */
function poLoadSoReset() {
    withButton('btnLsReset', function () {
        /* btnReset_Click :422 - FromDate = ActiveYr.Start_Period, read from the server session */
        return getJson(API + '/financial-year-start').catch(function () { return {}; }).then(function (d) {
            var fy = String((d && d.financialYearStart) || window.ACTIVE_YEAR_START || '').slice(0, 10);
            if (fy) $('lsFromDate').value = fy;
            $('lsDocNoFrom').value = ''; $('lsDocNoTo').value = '';
            $('lsBuyer').value = '0';
            return poLoadSoFetch();
        });
    });
}
function poLoadSoCheckAll(on) {
    Array.prototype.forEach.call(document.querySelectorAll('#grdLoadSoBody .ls-chk'), function (c) { c.checked = on; });
}
function poLoadSoLoad() {
    var checked = Array.prototype.filter.call(document.querySelectorAll('#grdLoadSoBody .ls-chk'), function (c) { return c.checked; })
        .map(function (c) { return loadSoRows[+c.getAttribute('data-i')]; });
    if (!checked.length) { alert('Check the row first'); return; }
    var allowed = {}, names = [];
    detailRows.forEach(function (d) {
        if (!allowed[d.itemId]) { allowed[d.itemId] = 1; names.push(d.itemName || textOfItem(d.itemId)); }
    });
    if (names.length) {
        for (var i = 0; i < checked.length; i++) {
            if (!allowed[colInt(checked[i], 'itemId')]) {
                alert('Sorry! You can only check rows having these Item(s):\n' + names.join(', '));
                return;
            }
        }
    }
    poLoadSoClose();
    $('cmbDeliveryToParty').value = colInt(checked[0], 'DeliveryToPartyId');
    var have = {};
    saleOrderMappings.forEach(function (m) { have[m.saleOrderDetailId] = 1; });
    checked.forEach(function (r) {
        var did = colInt(r, 'DetailId');
        if (have[did]) return;
        have[did] = 1;
        saleOrderMappings.push({
            purchaseOrderSaleOrderMappingId: 0,
            saleOrderMasterId: colInt(r, 'saleOrderMasterId'),
            saleOrderDetailId: did,
            saleOrderNo: colStr(r, 'docNo'),
            buyerId: colInt(r, 'buyerId'),
            buyerName: colStr(r, 'buyerName'),
            itemId: colInt(r, 'itemId'),
            itemName: colStr(r, 'ItemName'),
            itemWeight: colNum(r, 'itemWeight'),
            bookedWeight: colNum(r, 'WeightUsedInPo'),
            balanceWeight: colNum(r, 'BalanceWeight'),
            hasBalance: true,
            itemNetWeight: colNum(r, 'BalanceWeight'),
            validityDate: dstr(col(r, 'ValidityDate'))
        });
    });
    renderSaleOrderMapping();
}
function textOfItem(itemId) {
    var it = (lookupData.items || []).filter(function (x) { return +x.Id === +itemId; })[0];
    return it ? it.ItemName : String(itemId);
}

/* --------------------------------------------- Ship To Address "+" (SupfrmShipToAddress)
 * BtnAddShiptoAddress_Click opens the definition form; saving goes through BLL 0602 Save
 * (Sp_SupplierCustomerShipToAddress_Insert / _Update). Afterwards the ship-to lookup is
 * reloaded so the new address can be picked. */
var shipToLookupsLoaded = false;
function poAddShipToAddress() {
    withButton('btnAddShipTo', function () {
        $('shipToModal').style.display = 'flex';
        poShipToReset();
        var jobs = [];
        if (!shipToLookupsLoaded) {
            fillSelect('stParty', lookupData.deliveryParties || lookupData.suppliers || [], 'Id', 'CompanyName');
            jobs.push(getJson(API + '/ship-to/countries').then(function (rows) {
                fillSelect('stCountry', (rows || []).map(function (r) { return { Id: colInt(r, 'Id'), Name: colStr(r, 'Description') || colStr(r, 'CountryName') }; }), 'Id', 'Name');
            }));
            jobs.push(getJson(API + '/ship-to/cities').then(function (rows) {
                fillSelect('stCity', (rows || []).map(function (r) { return { Id: colInt(r, 'Id'), Name: colStr(r, 'CityName') }; }), 'Id', 'Name');
            }));
        }
        jobs.push(poShipToGrid());
        return Promise.all(jobs).then(function () {
            shipToLookupsLoaded = true;
            if (intOf('cmbDeliveryToParty')) $('stParty').value = intOf('cmbDeliveryToParty');
        });
    });
}
function poShipToClose() { $('shipToModal').style.display = 'none'; }
function poShipToReset() {
    $('stId').value = 0;
    ['stParty', 'stCountry', 'stCity'].forEach(function (id) { if ($(id)) $(id).value = '0'; });
    ['stTitle', 'stAddress', 'stContact', 'stPhone', 'stMobile', 'stWhatsApp'].forEach(function (id) { $(id).value = ''; });
    $('btnStSave').textContent = 'Save';
}
function poShipToGrid() {
    return getJson(API + '/ship-to/history').then(function (rows) {
        var body = $('grdShipToBody'); body.innerHTML = '';
        (rows || []).forEach(function (r) {
            var tr = document.createElement('tr');
            tr.style.cursor = 'pointer';
            tr.ondblclick = function () { poShipToEdit(colInt(r, 'Id')); };
            tr.innerHTML = '<td>' + esc(colStr(r, 'PartyName')) + '</td><td>' + esc(colStr(r, 'AddressTitle')) + '</td><td>' +
                esc(colStr(r, 'CountryName')) + '</td><td>' + esc(colStr(r, 'CityName')) + '</td><td>' + esc(colStr(r, 'ContactPerson')) +
                '</td><td>' + esc(colStr(r, 'PhoneNo')) + '</td><td>' + esc(colStr(r, 'MobileNo')) + '</td><td>' + esc(colStr(r, 'WhatsAppNo')) +
                '</td><td>' + esc(fmtDMY(col(r, 'EntryDate'), true)) + '</td><td>' + esc(colStr(r, 'EntryUser')) + '</td><td>' +
                esc(fmtDMY(col(r, 'ModifyDate'), true)) + '</td><td>' + esc(colStr(r, 'ModifyUser')) + '</td><td>' + esc(colStr(r, 'AddressLine1')) + '</td>';
            body.appendChild(tr);
        });
    });
}
function poShipToEdit(id) {
    getJson(API + '/ship-to/' + id).then(function (b) {
        $('stId').value = id;
        $('stParty').value = colInt(b, 'SupplierCustomerId');
        $('stCountry').value = colInt(b, 'CountryId');
        $('stCity').value = colInt(b, 'CityId');
        $('stAddress').value = colStr(b, 'AddressLine1');
        $('stTitle').value = colStr(b, 'AddressTitle');
        $('stWhatsApp').value = colStr(b, 'WhatsAppNo');
        $('stContact').value = colStr(b, 'ContactPerson');
        $('stMobile').value = colStr(b, 'MobileNo');
        $('stPhone').value = colStr(b, 'PhoneNo');
        $('btnStSave').textContent = 'Update';
    }).catch(function (e) { alert(e.message); });
}
function poShipToSave() {
    if (!intOf('stParty')) { alert('Please Select Supplier'); return; }
    if (!val('stAddress').trim()) { alert('Please Enter Address'); return; }
    if (!val('stTitle').trim()) { alert('Please Enter Address Title'); return; }
    if (!intOf('stCountry')) { alert('Please Select Country'); return; }
    if (!intOf('stCity')) { alert('Please Select City'); return; }
    withButton('btnStSave', function () {
        return fetch(API + '/ship-to/save', {
            method: 'POST', headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                Id: intOf('stId'), SupplierCustomerId: intOf('stParty'), CountryId: intOf('stCountry'), CityId: intOf('stCity'),
                AddressLine1: val('stAddress'), AddressTitle: val('stTitle'), PhoneNo: val('stPhone'), MobileNo: val('stMobile'),
                WhatsAppNo: val('stWhatsApp'), ContactPerson: val('stContact')
            })
        }).then(function (r) { return r.json(); }).then(function (d) {
            if (!d || !d.success) { alert((d && d.message) || 'Save refused.'); return; }
            alert(d.message);
            poShipToReset();
            return Promise.all([poShipToGrid(), getJson(LOOKUP + '/ship-to-addresses').then(function (rows) {
                lookupData.shipToAddresses = rows || [];
                poDeliveryPartyChanged();
            }).catch(function () {})]);
        });
    });
}

function poAttachments() { message('Attachments are not implemented on this screen yet.', true); }

function poPrint() {
    if (!intOf('purchaseOrderMasterId')) { message('No Data found to display', true); return; }
    window.print();
}


/* ------------------------------------------------------------- start-up */

/* Business Name / Nick Name / Code re-binds what the party combos display, as
   RadBusinessName_CheckedChanged does on the desktop (:879-926). The rows must
   carry NickName and PartyCode for the other two modes; when a row does not, that
   party keeps its CompanyName rather than showing blank. */
function poPartyNameModeChanged() {
    var mode = (document.querySelector('input[name="partyNameMode"]:checked') || {}).value || 'business';
    var field = mode === 'nick' ? 'NickName' : (mode === 'code' ? 'PartyCode' : 'CompanyName');
    [['cmbCommissionAgent', 'commissionAgents'], ['cmbSupplierName', 'suppliers'], ['cmbDeliveryToParty', 'deliveryParties']]
        .forEach(function (pair) {
            var sel = $(pair[0]); if (!sel) return;
            var keep = sel.value;
            var rows = (lookupData[pair[1]] || []).map(function (r) {
                return { Id: r.Id, Label: r[field] || r.CompanyName };
            });
            fillSelect(pair[0], rows, 'Id', 'Label');
            sel.value = keep;
        });
}

document.addEventListener('DOMContentLoaded', function () {
    Array.prototype.forEach.call(document.querySelectorAll('input[name="partyNameMode"]'), function (rb) {
        rb.addEventListener('change', poPartyNameModeChanged);
    });
    setBusy(true);
    loadLookups().finally(function () {
        setBusy(false);
        poNew();
        poShowTab(null, 'tabDetail');
        poHistRefreshCombos();
        /* report Doc No links open /commission/purchase-order?id=N */
        var qm = /[?&]id=(\d+)/.exec(location.search);
        if (qm) poLoad(+qm[1]);
    });
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


/* ===================== 2026-09-30 GUI parity (frmPurchaseOrderCmagt InitializeComponent) ===================== */

/* Button visibility as the desktop toggles it: New -> Save only (:2727), ReadById -> Update + Delete
   (:3400), history SaveAs -> SaveAs only (:4101). */
var poMode = 'new';
function poSetMode(mode) {
    poMode = mode;
    var show = { btnSave: mode === 'new', btnSaveAs: mode === 'saveas', btnUpdate: mode === 'edit', btnDelete: mode === 'edit' };
    Object.keys(show).forEach(function (id) { var b = $(id); if (b) b.style.display = show[id] ? '' : 'none'; });
    if (mode === 'saveas' && $('btnSaveAs')) $('btnSaveAs').disabled = false;
}

/* tabControl1: Form | History */
function poMainTab(ev, id) {
    if (ev) ev.preventDefault();
    ['tabForm', 'tabHistory'].forEach(function (t) { var e = $(t); if (e) e.style.display = (t === id) ? '' : 'none'; });
    Array.prototype.forEach.call(document.querySelectorAll('#poMainTabs a'), function (a) {
        a.classList.toggle('active', a.dataset.tab === id);
    });
}

/* tabControl3 on the History page */
function poHistTab(ev, id) {
    if (ev) ev.preventDefault();
    ['htDetail', 'htExpense', 'htEmptyBags', 'htPayment', 'htMapping'].forEach(function (t) {
        var e = $(t); if (e) e.style.display = (t === id) ? '' : 'none';
    });
    Array.prototype.forEach.call(document.querySelectorAll('#poHistTabs a'), function (a) {
        a.classList.toggle('active', a.dataset.tab === id);
    });
}

/* grdHistory_SelectionChanged -> GetDetailGrdByHeadId (:4110): ReadById of the selected row and
   the five child grids (detail, other charges, empty bags split by entryTypeId, payment terms,
   sale order mapping) with the desktop's column sets. Read-only; uses the same GET /{id}. */
var poHistChildSeq = 0;
function poHistShowChildren(id) {
    var seq = ++poHistChildSeq;
    var bodies = ['htDetailBody', 'htExpenseBody', 'htEbBody', 'htEbPmBody', 'htPaymentBody', 'htMappingBody'];
    bodies.forEach(function (b) { if ($(b)) $(b).innerHTML = ''; });
    if (!id) return;
    getJson(API + '/' + id).then(function (resp) {
        if (seq !== poHistChildSeq) return;
        var po = resp && resp.data ? resp.data : null;
        if (!po) return;
        function td(v, num) { return '<td' + (num ? ' class="num"' : '') + '>' + (num ? fmt(v) : esc(v == null ? '' : String(v))) + '</td>'; }
        function fill(bodyId, list, cells) {
            var b = $(bodyId); if (!b) return;
            b.innerHTML = (list || []).map(function (r) { return '<tr>' + cells(r) + '</tr>'; }).join('');
        }
        fill('htDetailBody', col(po, 'purchaseOrderDetailList'), function (d) {
            return td(colStr(d, 'inventoryParentCategory')) + td(colStr(d, 'ItemName')) + td(colStr(d, 'cropYear')) +
                td(colStr(d, 'PackingType')) + td(colStr(d, 'PackUomCode')) + td(colNum(d, 'itemQty'), 1) +
                td(colNum(d, 'itemWeight'), 1) + td(colNum(d, 'itemRate'), 1) + td(colStr(d, 'RateUomCode')) +
                td(colNum(d, 'ItemAmount'), 1) + td(colStr(d, 'TaxName')) + td(colNum(d, 'TaxPercent'), 1) +
                td(colNum(d, 'TaxAmount'), 1) + td(colNum(d, 'TotalAmount'), 1) + td(colStr(d, 'remarks'));
        });
        fill('htExpenseBody', col(po, 'purchaseOrderSupplierExpenseDetailList'), function (x) {
            return td(colStr(x, 'OtherItemName')) + td(colNum(x, 'Qty'), 1) + td(colNum(x, 'rate'), 1) +
                td(colNum(x, 'amount'), 1) + td(colStr(x, 'remarks'));
        });
        var bags = col(po, 'purchaseOrderEmptyBagDetailList') || [];
        fill('htEbBody', bags.filter(function (b) { return colInt(b, 'entryTypeId') === 1; }), function (b) {
            return td(colStr(b, 'PackingType')) + td(colNum(b, 'weightCutKg'), 1);
        });
        fill('htEbPmBody', bags.filter(function (b) { return colInt(b, 'entryTypeId') === 2; }), function (b) {
            return td(colStr(b, 'PackingType')) + td(colNum(b, 'Rate'), 1) + td(colStr(b, 'EmptyBagItem'));
        });
        fill('htPaymentBody', col(po, 'purchaseOrderPaymentDetailList'), function (x) {
            return td(colStr(x, 'PaymentTerm')) + td(colInt(x, 'DueDays'), 1) + td(colNum(x, 'pctOfTotal'), 1) +
                td(colNum(x, 'dueAmount'), 1) + td(colStr(x, 'BaseDateType')) + td(fmtDMY(col(x, 'DueDate')));
        });
        fill('htMappingBody', col(po, 'purchaseOrderSaleOrderMappingList'), function (m) {
            return td(colStr(m, 'SaleOrderNo')) + td(colStr(m, 'BuyerName')) + td(colStr(m, 'ItemName')) +
                td(colNum(m, 'SaleOrderWeight') || colNum(m, 'ItemWeight'), 1) +
                td(colNum(m, 'UsedSaleOrderWeightInPo') || colNum(m, 'BookedWeight'), 1) +
                td(colNum(m, 'SaleOrderBalWeight') || colNum(m, 'BalanceWeight'), 1) + td(colNum(m, 'itemNetWeight'), 1);
        });
    }).catch(function (e) { message('History detail failed: ' + e.message, true); });
}

/* BtnShortCutkeys_Click -> MakeShortCutKeys (:4600) */
var PO_SHORTCUTS = [
    ['Ctrl+S', 'For Save When on Entry form and For Show Data when on History Form'],
    ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+E', 'For Close'],
    ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
    ['Ctrl+F5', 'For Focus on Inquiry Date'], ['Ctrl+F10', 'For Open Attachments'],
    ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
    ['Ctrl+ArrowDown', 'For Focus On Parent Item'], ['Ctrl+ArrowUp', 'For Focus On on Inquiry Date'],
    ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
    ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]
];
function poShortCutKeys() {
    var b = $('sckBody'); if (!b) return;
    b.innerHTML = PO_SHORTCUTS.map(function (r) { return '<tr><td>' + esc(r[0]) + '</td><td>' + esc(r[1]) + '</td></tr>'; }).join('');
    $('sckModal').style.display = 'flex';
}

/* frmPurchaseOrderCmagt_KeyDown (:4470) - the browser-safe subset. A button only fires when it is
   visible and enabled, as on the desktop. */
function poVisibleEnabled(id) { var b = $(id); return !!b && !b.disabled && b.style.display !== 'none'; }
document.addEventListener('keydown', function (e) {
    if (!e.ctrlKey) return;
    var onForm = $('tabForm') && $('tabForm').style.display !== 'none';
    var k = e.key;
    var fire = function (id, fn) { e.preventDefault(); if (poVisibleEnabled(id)) fn(); };
    if (k === 't' || k === 'T') { e.preventDefault(); poMainTab(null, onForm ? 'tabHistory' : 'tabForm'); return; }
    if (!onForm) { if (k === 's' || k === 'S') fire('btnHistory', poLoadHistory); return; }
    if (e.shiftKey && k === 'Delete') fire('btnDelete', poDelete);
    else if (k === 's' || k === 'S') fire('btnSave', poSave);
    else if (k === 'F12') fire('btnSaveAs', poSave);
    else if (k === 'u' || k === 'U') fire('btnUpdate', poSave);
    else if (k === 'p' || k === 'P') fire('btnPrint', poPrint);
    else if (k === 'r' || k === 'R') fire('btnRefresh', poRefresh);
    else if (k === 'F5') { e.preventDefault(); $('datDocDate').focus(); }
    else if (k === 'F10') fire('btnAttachments', poAttachments);
    else if (k === 'ArrowDown') { e.preventDefault(); poShowTab(null, 'tabDetail'); $('cmbParentItem').focus(); }
    else if (k === 'ArrowUp') { e.preventDefault(); $('datDocDate').focus(); }
});
