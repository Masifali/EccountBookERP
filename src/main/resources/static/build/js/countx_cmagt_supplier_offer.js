/* ===========================================================================
 * Commission Trading - Supplier Offer (DocumentTypeId 1051)
 *
 * Ported from Architecture.WinApp.Cmagt/frmSupplierOfferCmagt.cs.
 *
 * This screen builds the SAME model as Purchase Order / Deal With Supplier
 * (Architecture.Model.CommissionAgent.purchaseOrderMaster) and saves through the
 * same BLL and procedures - :3357 is literally `purchaseOrderMaster.Save(obj)`.
 * Only DocumentTypeId (1051 vs 1052) and the mapping grid differ.
 *
 * ---------------------------------------------------------------------------
 * Desktop arithmetic, reproduced exactly
 * ---------------------------------------------------------------------------
 *   CalculateWeight()        (:1307-1319)
 *       weight = qty x PackUom.Equivalent
 *
 *   CalculateAmount()        (:1321-1335)
 *       amount = (weight > 0 && rateEquivalent > 0)
 *                    ? weight / rateEquivalent x rate
 *                    : 0
 *
 *   CalculateTaxAmount()     (:1425-1442)
 *       taxPercent  = txtTaxPercnt if > 0, else the selected tax row's percent
 *       taxAmount   = amount x taxPercent / 100      rounded 3, away from zero
 *       totalAmount = amount + taxAmount             rounded 3, away from zero
 *
 *   TotalCommissionAmount()  (:1518-1568)   - only when an ACCOUNT is chosen
 *       Flat        -> amount = rate
 *       Percent     -> rate capped at 100; amount = SUM(detail.Amount) x rate/100
 *                      rounded with Math.Round (whole number)
 *       Comm Weight -> amount = SUM(detail.Weight) / UOM_TEXT x rate
 *                      where UOM_TEXT is the combo's DISPLAYED TEXT parsed as a
 *                      number, not its id
 *       otherwise, or rate <= 0 -> amount = 0
 *
 *   TotalBrokeryAmountCalculate() (:1570-1616) - identical shape, own controls
 *
 *   PaymentAmountReCalculate()    (:1621-1640)
 *       every schedule row's Amount = %OfTotal x SUM(detail.TotalAmount) / 100
 *
 * ---------------------------------------------------------------------------
 * Three rules this file holds to
 * ---------------------------------------------------------------------------
 *   1. A UOM's database Id is never used as its conversion factor. `uomId` and
 *      `equivalent` are separate fields end to end.
 *   2. A missing or non-positive Equivalent is never quietly replaced with 1 or
 *      0. The row is refused and the UOM named, because the desktop cannot
 *      produce such a row either.
 *   3. For "Comm Weight" the divisor is the UOM combo's TEXT
 *      (Conversion.ToDouble(CmbCommUom.Text), :1546) - the reference NAME is
 *      itself the number. Using the id would silently divide by the wrong value.
 *
 * Nothing computed here is authoritative: the server re-runs every refusal in
 * PurchaseOrderMasterCmagtValidator before the procedure is called.
 * =========================================================================== */

'use strict';

var API    = '/api/commission/supplier-offer';
var LOOKUP = '/api/commission/dropdowns';

/* DocumentTypeId is fixed by the FORM, never inferred from the screen name or
   taken from the page - frmSupplierOfferCmagt.cs:546. The server overrides it
   anyway; it is sent so the payload is self-describing. */
var DOCUMENT_TYPE_ID = 1051;

/* ---------------------------------------------------------------- lookups */
/* Each entry names the endpoint, the desktop source that establishes it, and
 * the columns the desktop's ValueList binds. */
var LOOKUPS = [
    { key: 'companies',        url: LOOKUP + '/companies',         el: 'cmbCompany',         value: 'Id', text: 'CompName',      desktop: 'CompanyBind <- CommonServices.CompanyServiceBind() (:830)' },
    { key: 'branches',         url: LOOKUP + '/branches',          el: 'cmbBranch',          value: 'BranchId', text: 'BranchName', desktop: 'BranchesFill <- GetBranchsAllocatedToUser (:762)' },
    { key: 'commissionAgents', url: LOOKUP + '/commission-agents', el: 'cmbCommissionAgent', value: 'Id', text: 'CompanyName',   desktop: 'SupplierBind(CmbCommissionAgent, dtSupplier) (:900)' },
    { key: 'suppliers',        url: LOOKUP + '/suppliers',         el: 'cmbSupplierName',    value: 'Id', text: 'CompanyName',   desktop: 'SupplierBind(CmbSupplierName, dtSupplier) (:901)' },
    { key: 'deliveryParties',  url: LOOKUP + '/suppliers',         el: 'cmbDeliveryToParty', value: 'Id', text: 'CompanyName',   desktop: 'SupplierBind(CmbDeliveryToParty, dtSupplier) (:902)' },
    { key: 'shipToAddresses',  url: LOOKUP + '/ship-to-addresses', el: 'cmbShipToAddress',   value: 'Id', text: 'AddressLine1',  extra: ['SupplierCustomerId'], desktop: 'AllShipToAddress <- SupplierCustomerShipToAddress.GetAll_Combo (:1076)' },
    { key: 'deliveryTerms',    url: LOOKUP + '/delivery-terms',    el: 'cmbDeliveryTerm',    value: 'Id', text: 'Description',   desktop: 'DeliveryTermFill <- DeliveryTerm.FormHistory() (:1112)' },
    { key: 'paymentTerms',     url: LOOKUP + '/payment-terms',     el: 'cmbPaymentTerm',     value: 'Id', text: 'TermsDescription', desktop: 'PaymentTermsFill <- clsGlobalVariables.globalPaymentTerm (:1085)' },
    { key: 'parentItems',      url: LOOKUP + '/parent-categories', el: 'cmbParentItem',      value: 'Id', text: 'InvParentCateDescription', desktop: 'ParentCategoryBindFromGlobal (:1124)' },
    { key: 'items',            url: LOOKUP + '/items',             el: 'cmbItemName',        value: 'Id', text: 'ItemName',      extra: ['InventoryParentCategoriesId'], desktop: 'ItemdtFillFromGlobal / ItemNameBind (:1165/:1185)' },
    { key: 'cropYears',        url: LOOKUP + '/crop-years',        el: 'cmbCropYear',        value: 'Id', text: 'CropYear',      desktop: 'CropDtFillFromGlobalAndBind (:1203)' },
    { key: 'packingTypes',     url: LOOKUP + '/packing-types',     el: 'cmbPackingType',     value: 'Id', text: 'PackTypeDesc',  desktop: 'PackingTypeDtFillFromGlobalAndBind (:1218)' },
    /* CmbCommissionAc / CmbBrokeryAc are bound to the SAME party table as the other
       combos (SupplierBind :903-904), so the display column is CompanyName. What is
       saved is a party id (comm.commissionAgentId = CmbCommissionAc.Value, :3182). */
    { key: 'accounts',         url: LOOKUP + '/accounts',          el: 'cmbCommissionAc',    value: 'Id', text: 'CompanyName', alsoInto: ['cmbBrokeryAc'], desktop: 'SupplierBind(CmbCommissionAc / CmbBrokeryAc, dtSupplier) (:903-904)' },
    { key: 'emptyBagItems',    url: LOOKUP + '/empty-bag-items',   el: null, value: 'ItemId', text: 'ItemName',      desktop: 'dtItemForEmptyBag <- GetPackingMaterialItemsAllocateToFlow (:643)' },
    { key: 'otherItems',       url: LOOKUP + '/other-items',       el: null, value: 'Id',     text: 'OtherItemName', desktop: 'OtherItemsBind <- InventoryItemsOther.GetAll (:846)' },
    { key: 'viewCombos',       url: LOOKUP + '/view-combos',       el: null, value: 'Id',     text: 'ReferenceName', desktop: 'AllComboServices_FromViews, split by Activity (:735-762)' }
];

/* BindViewCombos (:716-762) fills four combos from ONE result set keyed on its
   Activity column. AllocatedPackingType seeds the empty-bags weight-cut grid;
   PaymentBaseDate feeds the schedule grid. */
var VIEW_ACTIVITIES = {
    CommissionType:       ['cmbCommType', 'cmbBrokeryType'],
    CommissionRateUom:    ['cmbCommUom', 'cmbBrokeryRateUom'],
    PaymentBaseDate:      [],
    AllocatedPackingType: []
};

var TAX_URL      = LOOKUP + '/taxes';           /* ?itemId=&docDate= */
var ITEM_UOM_URL = LOOKUP + '/item-uoms';       /* ?itemId= */
var CONFIG_URL   = LOOKUP + '/config-defaults';

/* ------------------------------------------------------------------ state */
var lookupData        = {};
var missingLookups    = [];
var portalDefaults    = {};
var detailRows        = [];
var removedDetailRows = [];      /* actionTypeId 3, kept so the server deletes them */
var expenseRows       = [];
var emptyBagRows      = [];      /* grdEmptyBags   - entryTypeId 1 */
var emptyBagPmRows    = [];      /* grdEmptyBagsPm - entryTypeId 2 */
var paymentRows       = [];
var inquiryMappings   = [];      /* grdInquiryMapping, filled only by Load Inquiry */
var removedMappings   = [];
var editingIdx        = -1;
var selectedIdx       = 0;
var busy              = false;
var soInFlight        = {};

/* ---------------------------------------------------------------- helpers */

function $(id) { return document.getElementById(id); }
function val(id) { var e = $(id); return e ? e.value : ''; }
function num(id) { var n = parseFloat(val(id)); return isNaN(n) ? 0 : n; }
function intOf(id) { var n = parseInt(val(id), 10); return isNaN(n) ? 0 : n; }

function esc(s) {
    return String(s === null || s === undefined ? '' : s)
        .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
}

/* Math.Round(x, 3, MidpointRounding.AwayFromZero) - JS Math.round is
   half-up-toward-positive-infinity, which differs for negatives. */
function r3(x) {
    var n = Number(x);
    if (!isFinite(n)) return 0;
    var f = Math.pow(10, 3);
    return (n < 0 ? -1 : 1) * Math.round(Math.abs(n) * f) / f;
}
/* Local-calendar yyyy-mm-dd. toISOString() converts to UTC first, so at the site's
   UTC+5 a local-midnight date came back as the PREVIOUS day (expiry/ValidityDate,
   payment DueDate and the new-document date were all one day early). */
function ymd(d) {
    if (!(d instanceof Date) || isNaN(d.getTime())) return '';
    var m = d.getMonth() + 1, day = d.getDate();
    return d.getFullYear() + '-' + (m < 10 ? '0' : '') + m + '-' + (day < 10 ? '0' : '') + day;
}

/* Jackson binds by the SETTER-derived name (setItemAmount -> "itemAmount",
   setEBWeightDeductionTermId -> "ebweightDeductionTermId"). The @JsonAlias
   annotations in PurchaseOrderMasterCmagtDto sit on private leading-capital fields,
   which Jackson treats as a separate, invisible property - so a key posted in the
   desktop model's spelling ("ItemAmount", "PaymentTermId", "ValidityDate", ...) was
   silently dropped (verified against jackson-databind). Every leading-capital key is
   therefore ALSO sent under its bean name; the original key is kept, and a key the
   object already carries is never overwritten. Recurses into the child lists. */
function jacksonName(k) {
    return k.replace(/^[A-Z]+/, function (m) { return m.toLowerCase(); });
}
function jacksonKeys(v) {
    if (Array.isArray(v)) return v.map(jacksonKeys);
    if (!v || typeof v !== 'object') return v;
    var out = {};
    Object.keys(v).forEach(function (k) { out[k] = jacksonKeys(v[k]); });
    Object.keys(v).forEach(function (k) {
        if (!/^[A-Z]/.test(k)) return;
        var bean = jacksonName(k);
        if (!(bean in out)) out[bean] = out[k];
    });
    return out;
}

function fmt(x) {
    var n = r3(x);
    return n.toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 3 });
}

function message(text, isError) {
    var box = $('soMessage');
    if (!box) return;
    if (!text) { box.style.display = 'none'; box.textContent = ''; return; }
    box.textContent = text;
    box.className = 'cmagt-status ' + (isError ? 'error' : 'ok');
    box.style.display = '';
    if (!isError) setTimeout(function () { if (box.textContent === text) message(''); }, 6000);
}

/* Button discipline: disable on click, show the loader, block duplicate
   requests, re-enable on BOTH success and failure. */
function setBusy(on, btnId) {
    busy = !!on;
    var form = document.body;
    if (form) form.classList.toggle('is-busy', busy);
    var ids = ['btnNew', 'btnRefresh', 'btnSave', 'btnUpdate', 'btnDelete',
               'btnLoadInquiry', 'btnAttachments', 'btnPrint', 'btnHistory', 'btnAddDetail'];
    ids.forEach(function (id) {
        var b = $(id);
        if (!b) return;
        if (busy) {
            if (b.dataset.wasDisabled === undefined) b.dataset.wasDisabled = b.disabled ? '1' : '0';
            b.disabled = true;
        } else {
            b.disabled = b.dataset.wasDisabled === '1';
            delete b.dataset.wasDisabled;
        }
        b.classList.toggle('btn-busy', busy && id === btnId);
    });
}

function soGuard(name) {
    if (soInFlight[name]) return false;
    soInFlight[name] = true;
    return true;
}
function soRelease(name) { soInFlight[name] = false; }

function getJson(url) {
    return fetch(url, { headers: { 'Accept': 'application/json' } }).then(function (r) {
        if (!r.ok) {
            return r.text().then(function (t) {
                var msg = t;
                try { var j = JSON.parse(t); msg = j.message || j.error || t; } catch (e) { /* plain text */ }
                throw new Error(msg || (r.status + ' ' + r.statusText));
            });
        }
        return r.json();
    });
}

/* BindAndRetainSelection with AllColumns:true shows every column but column 0.
   The desktop-combo widget reads these data-* attributes. */
function applyComboColumns(opt, row) {
    if (!row) return;
    var cols = [];
    Object.keys(row).forEach(function (k) {
        if (k === 'Id' || k === 'id') return;
        var v = row[k];
        if (v === null || v === undefined || v === '') return;
        if (typeof v === 'object') return;
        cols.push(String(v));
    });
    if (cols.length > 1) opt.setAttribute('data-cols', cols.slice(0, 5).join('\u001f'));
}

function fillSelect(elId, rows, valueKey, textKey, placeholder) {
    var sel = $(elId);
    if (!sel) return;
    var keep = sel.value;
    sel.innerHTML = '';
    var ph = document.createElement('option');
    ph.value = '0';
    ph.textContent = placeholder || '-- Select --';
    sel.appendChild(ph);
    (rows || []).forEach(function (r) {
        var o = document.createElement('option');
        o.value = r[valueKey];
        o.textContent = r[textKey] === null || r[textKey] === undefined ? '' : r[textKey];
        applyComboColumns(o, r);
        sel.appendChild(o);
    });
    if (keep) sel.value = keep;
    if (sel.dataset.dtcombo && window.CountxDesktopCombo && window.CountxDesktopCombo.refresh) {
        window.CountxDesktopCombo.refresh(sel);
    }
}

function loadLookups() {
    missingLookups = [];
    var jobs = LOOKUPS.map(function (L) {
        return getJson(L.url)
            .then(function (rows) {
                lookupData[L.key] = rows || [];
                if (!rows || !rows.length) missingLookups.push(L.key);
                if (L.el) fillSelect(L.el, rows, L.value, L.text);
                (L.alsoInto || []).forEach(function (other) { fillSelect(other, rows, L.value, L.text); });
            })
            .catch(function (e) {
                lookupData[L.key] = [];
                missingLookups.push(L.key + ' (' + e.message + ')');
            });
    });
    jobs.push(
        getJson(CONFIG_URL)
            .then(function (d) { portalDefaults = d || {}; })
            .catch(function () { portalDefaults = {}; })
    );
    return Promise.all(jobs).then(function () {
        bindViewCombos();
        if (missingLookups.length) {
            message('These lookups returned nothing and their dropdowns are empty: '
                  + missingLookups.join(', ')
                  + '. They are not defaulted - the desktop reads them from the database.', true);
        }
    });
}

function viewRows(activity) {
    return (lookupData.viewCombos || []).filter(function (r) { return r.Activity === activity; });
}

function bindViewCombos() {
    Object.keys(VIEW_ACTIVITIES).forEach(function (act) {
        var rows = viewRows(act);
        VIEW_ACTIVITIES[act].forEach(function (elId) { fillSelect(elId, rows, 'Id', 'ReferenceName'); });
    });
}

/* GetCommissionAgentConfigurationsFromGlobalandBind (:1057-1093): each id is
   applied ONLY when > 0 - a zero means "not configured", and the control keeps
   whatever it has. Nothing is substituted. */
function applyPortalDefaults() {
    var map = [
        ['commissionAgentId',   'cmbCommissionAgent'],
        ['commissionAccountId', 'cmbCommissionAc'],
        ['brokeryAccountId',    'cmbBrokeryAc'],
        ['paymentTermId',       'cmbPaymentTerm'],
        ['deliveryTermId',      'cmbDeliveryTerm'],
        ['cropYearId',          'cmbCropYear'],
        ['packingTypeId',       'cmbPackingType']
    ];
    map.forEach(function (pair) {
        var id = parseInt(portalDefaults[pair[0]], 10);
        if (id > 0) {
            var sel = $(pair[1]);
            if (sel) sel.value = id;
        }
    });
    /* :686 - the form opens on Payment Term 2 when the list is non-empty and no
       configured term overrode it. */
    var pt = $('cmbPaymentTerm');
    if (pt && intOf('cmbPaymentTerm') === 0 && (lookupData.paymentTerms || []).length) {
        pt.value = portalDefaults.fallbackPaymentTermId || 2;
    }
    soPaymentTermChanged();
}

/* ----------------------------------------------------- item-scoped lookups */

var itemUoms = [];

function loadItemUoms(itemId) {
    if (!itemId) { itemUoms = []; fillSelect('cmbPackUom', [], 'Id', 'UOMCode'); fillSelect('cmbRateUom', [], 'Id', 'UOMCode'); return Promise.resolve(); }
    return getJson(ITEM_UOM_URL + '?itemId=' + encodeURIComponent(itemId))
        .then(function (rows) {
            itemUoms = rows || [];
            fillSelect('cmbPackUom', itemUoms, 'Id', 'UOMCode');
            fillSelect('cmbRateUom', itemUoms, 'Id', 'UOMCode');
            /* CommonBindings.ItemUomFromGlobalBind pre-selects the item's base
               pack and base rate UOM when the schedule marks them. */
            itemUoms.forEach(function (u) {
                if (u.BasePackUom === true || u.BasePackUom === 1) $('cmbPackUom').value = u.Id;
                if (u.BaseRateUom === true || u.BaseRateUom === 1) $('cmbRateUom').value = u.Id;
            });
        })
        .catch(function (e) {
            itemUoms = [];
            message('UOM list for this item could not be read: ' + e.message, true);
        });
}

/* The Equivalent is the conversion factor; the Id is not. A missing or
   non-positive factor is returned as null so the caller refuses the row. */
function equivalentOf(uomId) {
    var id = parseInt(uomId, 10);
    if (!id) return null;
    for (var i = 0; i < itemUoms.length; i++) {
        if (parseInt(itemUoms[i].Id, 10) === id) {
            var eq = parseFloat(itemUoms[i].Equivalent);
            return (isFinite(eq) && eq > 0) ? eq : null;
        }
    }
    return null;
}

function uomCode(uomId) {
    var id = parseInt(uomId, 10);
    for (var i = 0; i < itemUoms.length; i++) {
        if (parseInt(itemUoms[i].Id, 10) === id) return itemUoms[i].UOMCode || '';
    }
    return '';
}

function loadTaxesForItem(itemId) {
    if (!itemId) {
        fillSelect('cmbTaxName', [], 'TaxNameId', 'TaxName');
        $('txtTaxPercnt').value = 0; $('txtTaxAmount').value = 0;
        soCalcTax();
        return Promise.resolve();
    }
    var q = '?itemId=' + encodeURIComponent(itemId) + '&docDate=' + encodeURIComponent(val('datDocDate'));
    return getJson(TAX_URL + q)
        .then(function (rows) {
            lookupData.taxes = rows || [];
            fillSelect('cmbTaxName', rows, 'TaxNameId', 'TaxName');
            soTaxNameChanged();
        })
        .catch(function () { fillSelect('cmbTaxName', [], 'TaxNameId', 'TaxName'); soCalcTax(); });
}

/* ------------------------------------------------------------ calculations */

/* CalculateWeight (:1307) - weight = qty x PackUom.Equivalent */
function soCalcWeight() {
    var qty = num('txtQty');
    var packId = intOf('cmbPackUom');
    if (!packId) { $('txtWeight').value = 0; return; }
    var eq = equivalentOf(packId);
    if (eq === null) {
        $('txtWeight').value = 0;
        message('Pack Uom "' + (uomCode(packId) || packId) + '" has no usable conversion factor, '
              + 'so the weight cannot be computed. It is not assumed to be 1.', true);
        return;
    }
    $('txtWeight').value = r3(qty * eq);
}

/* CalculateAmount (:1321) - amount = weight / rateEquivalent x rate, else 0 */
function soCalcAmount() {
    var weight = num('txtWeight');
    var rate = num('txtRate');
    var rateId = intOf('cmbRateUom');
    if (!rateId) { $('txtAmount').value = 0; soCalcTax(); return; }
    var eq = equivalentOf(rateId);
    if (eq === null) {
        $('txtAmount').value = 0;
        message('Rate Uom "' + (uomCode(rateId) || rateId) + '" has no usable conversion factor, '
              + 'so the amount cannot be computed. It is not assumed to be 1.', true);
        soCalcTax();
        return;
    }
    $('txtAmount').value = (weight > 0 && eq > 0) ? r3(weight / eq * rate) : 0;
    soCalcTax();
}

/* CalculateTaxAmount (:1425) */
function soCalcTax() {
    var amount = num('txtAmount');
    var pct = num('txtTaxPercnt');
    if (!(pct > 0)) {
        /* fall back to the selected tax row's own percent, as the desktop does */
        var sel = $('cmbTaxName');
        if (sel && parseInt(sel.value, 10) > 0) {
            var rows = lookupData.taxes || [];
            for (var i = 0; i < rows.length; i++) {
                if (parseInt(rows[i].TaxNameId, 10) === parseInt(sel.value, 10)) {
                    pct = parseFloat(rows[i].TaxPercent) || 0;
                    break;
                }
            }
        }
    }
    var taxAmt = amount * pct / 100;
    $('txtTaxPercnt').value = r3(pct);
    $('txtTaxAmount').value = r3(taxAmt);
    $('txtTotalAmount').value = r3(amount + taxAmt);
}

function soCalcRow(skipWeight) {
    if (!skipWeight) soCalcWeight();
    soCalcAmount();
}

function soTaxNameChanged() {
    var sel = $('cmbTaxName');
    var rows = lookupData.taxes || [];
    var pct = 0;
    if (sel && parseInt(sel.value, 10) > 0) {
        for (var i = 0; i < rows.length; i++) {
            if (parseInt(rows[i].TaxNameId, 10) === parseInt(sel.value, 10)) {
                pct = parseFloat(rows[i].TaxPercent) || 0;
                break;
            }
        }
    }
    $('txtTaxPercnt').value = r3(pct);
    soCalcTax();
}

/* datDeliveryStartDate_ValueChanged (:3635) / txtDeliveryDays_TextChanged (:3674)
   expiry = deliveryDays == 0 ? start : start + deliveryDays. That value is what
   the master saves as ValidityDate (Insert():2996). */
function soCalculateExpiryDate() {
    var start = val('datDeliveryStartDate');
    if (!start) { $('datExpiryDate').value = ''; return; }
    var days = intOf('txtDeliveryDays');
    var d = new Date(start + 'T00:00:00');
    if (days > 0) d.setDate(d.getDate() + days);
    $('datExpiryDate').value = ymd(d);
}

/* CmbPaymentTerm_ValueChanged (:1101-1117) - terms 1 and 3 force 0 due days and
   disable the box; term 2 seeds 2 when the box is still zero. */
function soPaymentTermChanged() {
    var t = intOf('cmbPaymentTerm');
    var box = $('txtDueDays');
    if (!box) return;
    box.disabled = false;
    if (t === 1 || t === 3) {
        box.value = 0;
        box.disabled = true;
    } else if (t === 2 && intOf('txtDueDays') === 0) {
        box.value = 2;
    }
}

/* The document date is part of the tax schedule lookup key (:1234), so changing
   it re-reads the tax for the item currently in the entry row. */
function soDocDateChanged() {
    var itemId = intOf('cmbItemName');
    if (itemId) loadTaxesForItem(itemId);
    recalcAllPaymentDueDates();
}

/* TotalCommissionAmount (:1518) and TotalBrokeryAmountCalculate (:1570) are the
   same shape over different controls, so they share one implementation - the
   desktop's two methods are line-for-line equivalent apart from the control
   names and which grid total they read. */
function calcAgentAmount(accountSelId, typeSelId, rateId, uomSelId, outId) {
    var out = $(outId);
    if (!out) return;

    /* :1520 / :1576 - nothing is computed unless an ACCOUNT row is active. */
    if (!intOf(accountSelId)) { out.value = 0; return; }

    var rate = num(rateId);
    if (!(rate > 0)) { out.value = 0; return; }

    var typeText = textOf(typeSelId).trim();
    var t = detailTotals();

    if (typeText === 'Flat') {
        out.value = r3(rate);
        return;
    }
    if (typeText === 'Percent') {
        if (rate > 100) { rate = 100; $(rateId).value = 100; }    /* :1531 / :1589 */
        out.value = Math.round(t.amount * rate / 100);            /* Math.Round -> whole */
        return;
    }
    if (typeText === 'Comm Weight') {
        /* :1546 / :1606 - the divisor is the UOM combo's TEXT parsed as a number.
           The reference NAME is the number (e.g. "40"); its Id is not. */
        var uomText = parseFloat(textOf(uomSelId).trim());
        if (!isFinite(uomText) || uomText === 0) {
            out.value = 0;
            message('Commission type "Comm Weight" divides the total weight by the Rate Uom\'s own '
                  + 'value, and "' + textOf(uomSelId) + '" is not a number. No divisor is assumed.', true);
            return;
        }
        out.value = Math.round(t.weight / uomText * rate);
        return;
    }
    /* any other type: the desktop leaves the box at its previous value only when
       rate <= 0; for an unrecognised type it never assigns, so 0 stands. */
    out.value = 0;
}

function soCalcCommission() {
    calcAgentAmount('cmbCommissionAc', 'cmbCommType', 'txtCommRate', 'cmbCommUom', 'txtCommAmount');
}
function soCalcBrokery() {
    calcAgentAmount('cmbBrokeryAc', 'cmbBrokeryType', 'txtBrokeryRate', 'cmbBrokeryRateUom', 'txtBrokeryAmount');
}

function textOf(selId) {
    var s = $(selId);
    if (!s || s.selectedIndex < 0) return '';
    return s.options[s.selectedIndex].textContent || '';
}

/* ------------------------------------------------------------ header events */

/* CmbDeliveryToParty_Leave (:1019) - re-filter ship-to addresses for the party,
   and clear the address text when no party is chosen. */
function soDeliveryPartyChanged() {
    var partyId = intOf('cmbDeliveryToParty');
    var all = lookupData.shipToAddresses || [];
    var rows = partyId ? all.filter(function (r) {
        return parseInt(r.SupplierCustomerId, 10) === partyId;
    }) : all;
    fillSelect('cmbShipToAddress', rows, 'Id', 'AddressLine1');
    if (!partyId) { $('cmbShipToAddress').value = 0; $('txtShipToAddress').value = ''; }
    /* BindShipToAddressAgainstBuyer activates the row when exactly one matches (:1105). */
    if (rows.length === 1) { $('cmbShipToAddress').value = rows[0].Id; soShipToAddressChanged(); }
}

/* CmbShipToAddress_Leave (:1036) - picking an address sets the party back. */
function soShipToAddressChanged() {
    var id = intOf('cmbShipToAddress');
    if (!id) return;
    var rows = lookupData.shipToAddresses || [];
    for (var i = 0; i < rows.length; i++) {
        if (parseInt(rows[i].Id, 10) === id) {
            var partyId = parseInt(rows[i].SupplierCustomerId, 10) || 0;
            if (partyId > 0) $('cmbDeliveryToParty').value = partyId;
            $('txtShipToAddress').value = rows[i].AddressLine1 || '';
            break;
        }
    }
}

/* cmbParentItem_Leave (:1152) - re-filter the item list by parent category. */
function soParentItemChanged() {
    var parentId = intOf('cmbParentItem');
    var all = lookupData.items || [];
    var rows = parentId ? all.filter(function (r) {
        return parseInt(r.InventoryParentCategoriesId, 10) === parentId;
    }) : all;
    fillSelect('cmbItemName', rows, 'Id', 'ItemName');
    soItemChanged();
}

/* CmbItemName_Leave (:1234) - tax schedule for the item as at the doc date, then
   the item's own UOM schedule. */
function soItemChanged() {
    var itemId = intOf('cmbItemName');
    loadTaxesForItem(itemId);
    loadItemUoms(itemId).then(function () { soCalcRow(); });
}

/* The empty-bags weight-cut grid is skipped entirely under the FOC term (:3091). */
function soEbTermChanged() {
    renderEmptyBags();
    var note = $('ebFocNote');
    if (!note) return;
    note.textContent = ebTermId() === 3
        ? 'Bag FOC & Weight Cut Not Apply is selected, so the weight-cut rows are not validated '
        + 'and may stay at zero (frmSupplierOfferCmagt.cs:3091).'
        : '';
}

function ebTermId() {
    var r = document.querySelector('input[name="ebTerm"]:checked');
    return r ? parseInt(r.value, 10) : 0;
}

/* ------------------------------------------------------------- detail grid */

/* FormValidationDetail (:2936-2956) - the entry row's own required fields,
   in the desktop's order and with its own field labels. */
function validateDetailEntry() {
    if (!intOf('cmbParentItem'))  return 'Parent Item Field Required';
    if (!intOf('cmbItemName'))    return 'Item Name Field Required';
    if (!intOf('cmbCropYear'))    return 'Crop Year Field Required';
    if (!intOf('cmbPackingType')) return 'Packing Type Field Required';
    if (!intOf('cmbPackUom'))     return 'Pack Uom Field Required';
    if (!(num('txtQty') > 0))     return 'Qty Field Required';
    if (!(num('txtWeight') > 0))  return 'Weight Field Required';
    if (!(num('txtRate') > 0))    return 'NetWeight Field Required';   /* desktop's label (:2945) */
    if (!intOf('cmbRateUom'))     return 'Rate Uom Field Required';
    if (!(num('txtAmount') > 0))  return 'NetWeight Field Required';   /* desktop's label (:2947) */
    if (!(num('txtTotalAmount') > 0)) return 'Tax + Amount Field Required';
    return null;
}

/* BtnAdd_Click (:1932) / BtnUpdateDetail_Click (:2011) - one row per item. */
function soAddOrUpdateDetail() {
    var err = validateDetailEntry();
    if (err) { message(err, true); return; }

    var itemId = intOf('cmbItemName');
    var clash = detailRows.some(function (r, idx) {
        return idx !== editingIdx && parseInt(r.itemId, 10) === itemId;
    });
    if (clash) {
        message(textOf('cmbItemName') + ' already in detail. So can\'t add this item', true);
        $('cmbItemName').focus();
        return;
    }

    var row = {
        purchaseOrderDetailId: editingIdx >= 0 ? (detailRows[editingIdx].purchaseOrderDetailId || 0) : 0,
        inventoryParentCategoryId: intOf('cmbParentItem'),
        parentItem:   textOf('cmbParentItem'),
        itemId:       itemId,
        ItemName:     textOf('cmbItemName'),
        cropYearId:   intOf('cmbCropYear'),
        cropYear:     textOf('cmbCropYear'),
        packingTypeId: intOf('cmbPackingType'),
        PackingType:  textOf('cmbPackingType'),
        packUomId:    intOf('cmbPackUom'),
        packUom:      textOf('cmbPackUom'),
        itemQty:      r3(num('txtQty')),
        itemWeight:   r3(num('txtWeight')),
        itemRate:     r3(num('txtRate')),
        rateUomId:    intOf('cmbRateUom'),
        RateUomCode:  textOf('cmbRateUom'),
        ItemAmount:   r3(num('txtAmount')),
        TaxNameId:    intOf('cmbTaxName'),
        TaxName:      intOf('cmbTaxName') ? textOf('cmbTaxName') : '',
        TaxPercent:   r3(num('txtTaxPercnt')),
        TaxAmount:    r3(num('txtTaxAmount')),
        TotalAmount:  r3(num('txtTotalAmount')),
        remarks:      val('txtRemarksDetail')
    };

    if (editingIdx >= 0) detailRows[editingIdx] = row; else detailRows.push(row);

    clearDetailEntry();
    renderDetail();
    /* the three recalculations the desktop runs after every add/update/delete
       (:1958-1961, :2035-2038, :1793-1796) */
    soCalcCommission();
    soCalcBrokery();
    paymentAmountReCalculate();
}

function clearDetailEntry() {
    editingIdx = -1;
    ['txtQty', 'txtWeight', 'txtRate', 'txtAmount', 'txtTaxPercnt', 'txtTaxAmount', 'txtTotalAmount']
        .forEach(function (id) { $(id).value = 0; });
    $('txtRemarksDetail').value = '';
    $('cmbItemName').value = 0;
    $('cmbTaxName').value = 0;
    $('btnAddDetail').textContent = '+';
    $('btnCancelDetail').style.display = 'none';
}

/* grdDetail_DoubleClick (:1799) - load the row back into the entry strip. */
function soEditDetail(i) {
    var r = detailRows[i];
    if (!r) return;
    editingIdx = i;
    $('cmbParentItem').value = r.inventoryParentCategoryId || 0;
    soParentItemChanged();
    $('cmbItemName').value = r.itemId || 0;
    loadTaxesForItem(r.itemId);
    loadItemUoms(r.itemId).then(function () {
        $('cmbPackUom').value = r.packUomId || 0;
        $('cmbRateUom').value = r.rateUomId || 0;
    });
    $('cmbCropYear').value = r.cropYearId || 0;
    $('cmbPackingType').value = r.packingTypeId || 0;
    $('txtQty').value = r.itemQty || 0;
    $('txtWeight').value = r.itemWeight || 0;
    $('txtRate').value = r.itemRate || 0;
    $('txtAmount').value = r.ItemAmount || 0;
    $('cmbTaxName').value = r.TaxNameId || 0;
    $('txtTaxPercnt').value = r.TaxPercent || 0;
    $('txtTaxAmount').value = r.TaxAmount || 0;
    $('txtTotalAmount').value = r.TotalAmount || 0;
    $('txtRemarksDetail').value = r.remarks || '';
    $('btnAddDetail').textContent = 'Update';
    $('btnCancelDetail').style.display = '';
    $('cmbParentItem').focus();
}

function soCancelDetailEdit() { clearDetailEntry(); }

/* DeleteDetailRow (:1748) - a saved row is queued with actionTypeId 3 so the
   server deletes it; an unsaved row just disappears. An edit in progress blocks
   the delete, exactly as the desktop does (:1756). */
function soDeleteDetail(i) {
    if (editingIdx !== -1) { message('Please Reset the Detail first..', true); return; }
    var r = detailRows[i];
    if (!r) return;
    if (r.purchaseOrderDetailId > 0) {
        if (!confirm('Are you sure to Delete?')) return;
        var gone = Object.assign({}, r);
        gone.actionTypeId = 3;
        removedDetailRows.push(gone);
    }
    detailRows.splice(i, 1);
    renderDetail();
    soCalcCommission();
    soCalcBrokery();
    paymentAmountReCalculate();
}

function detailTotals() {
    var t = { qty: 0, weight: 0, amount: 0, tax: 0, total: 0 };
    detailRows.forEach(function (r) {
        t.qty    += +r.itemQty    || 0;
        t.weight += +r.itemWeight || 0;
        t.amount += +r.ItemAmount || 0;
        t.tax    += +r.TaxAmount  || 0;
        t.total  += +r.TotalAmount || 0;
    });
    return t;
}

/* ---------------------------------------------------------------------------
 * Display text for a saved detail row.
 *
 * ItemName, PackingType, RateUomCode and TaxName are VIRTUAL on
 * Architecture.Model.CommissionAgent.purchaseOrderDetail (:65, :73, :75, :79), so
 * GenericProvider.SetProc never sends them and the DTO correctly omits them. A row
 * that comes back from ReadById therefore carries ids, and may or may not carry the
 * display strings depending on what the read procedure selects.
 *
 * Rather than leave those grid cells blank after a reload, the text is resolved from
 * the SAME lookup lists the desktop binds these combos to. Nothing is invented: when
 * the id is not in the list the cell stays empty, which is what the desktop shows for
 * an id it cannot resolve either.
 * --------------------------------------------------------------------------- */
function lookupText(listKey, idValue, valueKey, textKey) {
    var id = parseInt(idValue, 10);
    if (!id) return '';
    var rows = lookupData[listKey] || [];
    for (var i = 0; i < rows.length; i++) {
        if (parseInt(rows[i][valueKey], 10) === id) return rows[i][textKey] || '';
    }
    return '';
}

function rowParentItem(r) {
    return r.parentItem || lookupText('parentItems', r.inventoryParentCategoryId, 'Id', 'InvParentCateDescription');
}
function rowItemName(r) {
    return r.ItemName || r.itemName || lookupText('items', r.itemId, 'Id', 'ItemName');
}
function rowCropYear(r) {
    return r.cropYear || lookupText('cropYears', r.cropYearId, 'Id', 'CropYear');
}
function rowPackingType(r) {
    return r.PackingType || r.packingType || lookupText('packingTypes', r.packingTypeId, 'Id', 'PackTypeDesc');
}
/* Pack/Rate UOM lists are item-scoped and are only loaded for the item in the entry
   row, so a saved row's UOM code is taken from the row when the read supplied it and
   left blank otherwise - guessing from another item's schedule would be wrong. */
function rowPackUom(r)  { return r.packUom || r.PackUomCode || ''; }
function rowRateUom(r)  { return r.RateUomCode || r.rateUomCode || ''; }
function rowTaxName(r)  { return r.TaxName || r.taxName || ''; }

function renderDetail() {
    var body = $('grdDetailBody');
    if (!body) return;
    body.innerHTML = '';
    if (!detailRows.length) {
        body.innerHTML = '<tr><td colspan="17">No detail rows. Fill the entry row above and press +.</td></tr>';
    } else {
        detailRows.forEach(function (r, i) {
            var tr = document.createElement('tr');
            if (i === selectedIdx) tr.className = 'selected';
            tr.innerHTML =
                '<td><button type="button" class="danger" onclick="soDeleteDetail(' + i + ')">X</button></td>' +
                '<td><button type="button" onclick="soEditDetail(' + i + ')">Edit</button></td>' +
                '<td>' + esc(rowParentItem(r)) + '</td>' +
                '<td>' + esc(rowItemName(r)) + '</td>' +
                '<td>' + esc(rowCropYear(r)) + '</td>' +
                '<td>' + esc(rowPackingType(r)) + '</td>' +
                '<td>' + esc(rowPackUom(r)) + '</td>' +
                '<td style="text-align:right;">' + fmt(r.itemQty) + '</td>' +
                '<td style="text-align:right;">' + fmt(r.itemWeight) + '</td>' +
                '<td style="text-align:right;">' + fmt(r.itemRate) + '</td>' +
                '<td>' + esc(rowRateUom(r)) + '</td>' +
                '<td style="text-align:right;">' + fmt(r.ItemAmount) + '</td>' +
                '<td>' + esc(rowTaxName(r)) + '</td>' +
                '<td style="text-align:right;">' + fmt(r.TaxPercent) + '</td>' +
                '<td style="text-align:right;">' + fmt(r.TaxAmount) + '</td>' +
                '<td style="text-align:right;">' + fmt(r.TotalAmount) + '</td>' +
                '<td>' + esc(r.remarks) + '</td>';
            tr.ondblclick = function () { soEditDetail(i); };
            body.appendChild(tr);
        });
    }
    var t = detailTotals();
    $('totQty').textContent = fmt(t.qty);
    $('totWeight').textContent = fmt(t.weight);
    $('totAmount').textContent = fmt(t.amount);
    $('totTaxAmount').textContent = fmt(t.tax);
    $('totTotalAmount').textContent = fmt(t.total);
    $('soNavTotal').textContent = detailRows.length;
    $('soNavCurrent').value = detailRows.length ? (selectedIdx + 1) : 0;

    /* EnableDisableColumns (:1789) - the doc date is locked once any row carries tax. */
    var hasTax = detailRows.some(function (r) { return (+r.TaxAmount || 0) > 0; });
    $('datDocDate').disabled = hasTax;
}

function soNav(what, value) {
    if (!detailRows.length) return;
    if (what === 'first') selectedIdx = 0;
    else if (what === 'last') selectedIdx = detailRows.length - 1;
    else if (what === 'prev') selectedIdx = Math.max(0, selectedIdx - 1);
    else if (what === 'next') selectedIdx = Math.min(detailRows.length - 1, selectedIdx + 1);
    else if (what === 'goto') {
        var n = parseInt(value, 10);
        if (isFinite(n) && n >= 1 && n <= detailRows.length) selectedIdx = n - 1;
    }
    renderDetail();
}

function soShowTab(ev, id) {
    if (ev) ev.preventDefault();
    ['tabDetail', 'tabExpense', 'tabEmptyBags', 'tabPayment'].forEach(function (t) {
        var p = $(t);
        if (p) p.style.display = (t === id) ? '' : 'none';
    });
    Array.prototype.forEach.call(document.querySelectorAll('#soTabs a'), function (a) {
        a.classList.toggle('active', a.dataset.tab === id);
    });
}

/* Full-width tables: only the table container scrolls, never the page. */
function soToggleFullscreen(wrapId, btn) {
    var w = $(wrapId);
    if (!w) return;
    var on = w.classList.toggle('fs');
    if (btn) btn.textContent = on ? 'Exit Fullscreen' : 'Fullscreen';
    document.body.style.overflow = on ? 'hidden' : '';
}

/* ---------------------------------------------------- supplier other charges */

/* AddRowsInExpenseGrid (:2049) - one row per InventoryItemsOther, all zeros. */
function seedExpenseRows() {
    var items = lookupData.otherItems || [];
    var existing = {};
    expenseRows.forEach(function (r) { existing[r.ItemId] = r; });
    expenseRows = items.map(function (it) {
        var e = existing[it.Id];
        return {
            purchaseOrderSupplierExpenseDetailId: e ? e.purchaseOrderSupplierExpenseDetailId : 0,
            saleOrderMasterId: e ? e.saleOrderMasterId : 0,
            saleOrderBuyerOtherExpenseDetailId: e ? e.saleOrderBuyerOtherExpenseDetailId : 0,
            ItemId: it.Id,
            OtherItemName: it.OtherItemName,
            Qty:  e ? e.Qty : 0,
            rate: e ? e.rate : 0,
            amount: e ? e.amount : 0,
            remarks: e ? e.remarks : ''
        };
    });
    renderExpense();
}

function renderExpense() {
    var body = $('grdInvExpBody');
    if (!body) return;
    body.innerHTML = '';
    if (!expenseRows.length) {
        body.innerHTML = '<tr><td colspan="5">No other-charge items are configured (InventoryItemsOther).</td></tr>';
        $('totExpense').textContent = '0';
        return;
    }
    var tot = 0;
    expenseRows.forEach(function (r, i) {
        tot += +r.amount || 0;
        var tr = document.createElement('tr');
        tr.innerHTML =
            '<td>' + esc(r.OtherItemName) + '</td>' +
            '<td><input type="number" step="0.001" value="' + (r.Qty || 0) + '" oninput="expenseRows[' + i + '].Qty=parseFloat(this.value)||0;recalcExpense(' + i + ')"></td>' +
            '<td><input type="number" step="0.001" value="' + (r.rate || 0) + '" oninput="expenseRows[' + i + '].rate=parseFloat(this.value)||0;recalcExpense(' + i + ')"></td>' +
            '<td><input type="number" step="0.001" value="' + (r.amount || 0) + '" oninput="expenseRows[' + i + '].amount=parseFloat(this.value)||0;renderExpenseTotal()"></td>' +
            '<td><input type="text" value="' + esc(r.remarks) + '" oninput="expenseRows[' + i + '].remarks=this.value"></td>';
        body.appendChild(tr);
    });
    $('totExpense').textContent = fmt(tot);
}

function renderExpenseTotal() {
    var tot = expenseRows.reduce(function (s, r) { return s + (+r.amount || 0); }, 0);
    $('totExpense').textContent = fmt(tot);
}

function recalcExpense(i) {
    var r = expenseRows[i];
    if (!r) return;
    r.amount = r3((+r.Qty || 0) * (+r.rate || 0));
    renderExpense();
}

/* ----------------------------------------------------------- empty bags */

/* The weight-cut grid is one row per AllocatedPackingType reference value
   (BindViewCombos :758 feeds GrdEmptyBagsRefresh). */
function seedEmptyBagRows() {
    var types = viewRows('AllocatedPackingType');
    var existing = {};
    emptyBagRows.forEach(function (r) { existing[r.PackingTypeId] = r; });
    emptyBagRows = types.map(function (t) {
        var e = existing[t.Id];
        return {
            purchaseOrderEmptyBagDetailId: e ? e.purchaseOrderEmptyBagDetailId : 0,
            entryTypeId: 1,
            PackingTypeId: t.Id,
            PackingType: t.ReferenceName,
            weightCutKg: e ? e.weightCutKg : 0
        };
    });
    renderEmptyBags();
}

/* Insert():3097-3102 - ids 1, 2 and 5 validate against themselves; everything
   else validates against packing type 2. That substitution is the desktop's. */
function packingTypeRange(packingTypeId) {
    var declared = parseInt(packingTypeId, 10) || 0;
    var validationTypeId = (declared === 1 || declared === 2 || declared === 5) ? declared : 2;
    var rows = lookupData.packingTypes || [];
    for (var i = 0; i < rows.length; i++) {
        if (parseInt(rows[i].Id, 10) === validationTypeId) {
            var mn = parseFloat(rows[i].MinEbWeight), mx = parseFloat(rows[i].MaxEbWeight);
            if (isFinite(mn) && isFinite(mx) && mx > 0) {
                return { min: mn, max: mx, name: rows[i].PackTypeDesc };
            }
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
    if (!emptyBagRows.length) {
        body.innerHTML = '<tr><td colspan="3">No packing types configured (AllocatedPackingType).</td></tr>';
        return;
    }
    var foc = ebTermId() === 3;
    emptyBagRows.forEach(function (r, i) {
        var range = packingTypeRange(r.PackingTypeId);
        var tr = document.createElement('tr');
        tr.innerHTML =
            '<td>' + esc(r.PackingType) + '</td>' +
            '<td><input type="number" step="0.001" value="' + (r.weightCutKg || 0) + '" ' +
                 'oninput="emptyBagRows[' + i + '].weightCutKg=parseFloat(this.value)||0"></td>' +
            '<td style="font-size:11px;color:#5B6A69;">' +
                (foc ? 'not validated under the FOC term'
                     : (range ? (fmt(range.min) + ' to ' + fmt(range.max) + ' (' + esc(range.name) + ')')
                              : 'no range configured')) +
            '</td>';
        body.appendChild(tr);
    });
}

function soAddEmptyBagPmRow() {
    emptyBagPmRows.push({
        purchaseOrderEmptyBagDetailId: 0, entryTypeId: 2,
        PackingTypeId: 0, PackingType: '',
        emptyBagPackingMaterialItemId: 0, EmptyBagItem: '', Rate: 0
    });
    renderEmptyBagsPm();
}

/* grdEmptyBagsPm: packing type + material ITEM (a ValueList combo, :2264) + rate.
   The item must be an id, not typed text: Insert():3111 only keeps the row when
   Conversion.ToInt(EmptyBagItem) > 0. */
function renderEmptyBagsPm() {
    var body = $('grdEmptyBagsPmBody');
    if (!body) return;
    body.innerHTML = '';
    if (!emptyBagPmRows.length) {
        body.innerHTML = '<tr><td colspan="4">No packing-material rate rows.</td></tr>';
        return;
    }
    var items = lookupData.emptyBagItems || [];
    emptyBagPmRows.forEach(function (r, i) {
        var itemOpts = '<option value="0">-- Select --</option>' + items.map(function (it) {
            return '<option value="' + it.ItemId + '"' +
                   (parseInt(it.ItemId, 10) === parseInt(r.emptyBagPackingMaterialItemId, 10) ? ' selected' : '') +
                   '>' + esc(it.ItemName) + '</option>';
        }).join('');
        var tr = document.createElement('tr');
        tr.innerHTML =
            '<td><button type="button" class="danger" onclick="emptyBagPmRows.splice(' + i + ',1);renderEmptyBagsPm();">X</button></td>' +
            '<td><select onchange="emptyBagPmRows[' + i + '].PackingTypeId=parseInt(this.value,10)||0;emptyBagPmRows[' + i + '].PackingType=this.options[this.selectedIndex].textContent">' + viewComboOptions('AllocatedPackingType', r.PackingTypeId) + '</select></td>' +
            '<td><select onchange="emptyBagPmRows[' + i + '].emptyBagPackingMaterialItemId=parseInt(this.value,10)||0;emptyBagPmRows[' + i + '].EmptyBagItem=this.options[this.selectedIndex].textContent">' + itemOpts + '</select></td>' +
            '<td><input type="number" step="0.001" value="' + (r.Rate || 0) + '" oninput="emptyBagPmRows[' + i + '].Rate=parseFloat(this.value)||0"></td>';
        body.appendChild(tr);
    });
}

/* -------------------------------------------------------- payment schedule */

function soAddPaymentRow() {
    paymentRows.push({
        purchaseOrderPaymentDetailId: 0,
        PaymentTermId: intOf('cmbPaymentTerm') || 0,
        PaymentTerm: textOf('cmbPaymentTerm'),
        DueDays: intOf('txtDueDays') || 0,
        BaseDueDateTypeId: 1,
        DueDate: '',
        pctOfTotal: 0,
        dueAmount: 0
    });
    recalcPaymentDueDate(paymentRows.length - 1);
    renderPayment();
}

function paymentTermOptions(selectedId) {
    var rows = lookupData.paymentTerms || [];
    return '<option value="0">-- Select --</option>' + rows.map(function (p) {
        return '<option value="' + p.Id + '"' + (parseInt(p.Id, 10) === parseInt(selectedId, 10) ? ' selected' : '') + '>' + esc(p.TermsDescription) + '</option>';
    }).join('');
}

function renderPayment() {
    var body = $('grdPaymentTermBody');
    if (!body) return;
    body.innerHTML = '';
    if (!paymentRows.length) {
        body.innerHTML = '<tr><td colspan="7">No schedule rows. One 100% row is synthesized from the '
                       + 'header Payment Term at save time, as the desktop does (:3193).</td></tr>';
        $('totPct').textContent = '0';
        $('totDue').textContent = '0';
        refreshScheduleDescription();
        return;
    }
    var pct = 0, due = 0;
    paymentRows.forEach(function (r, i) {
        pct += +r.pctOfTotal || 0;
        due += +r.dueAmount || 0;
        /* DueDate is editable only under base type 4; type 1 derives it from the
           doc date + due days, any other type leaves it null (:3182-3184). */
        var editableDate = parseInt(r.BaseDueDateTypeId, 10) === 4;
        var tr = document.createElement('tr');
        tr.innerHTML =
            '<td><button type="button" class="danger" onclick="paymentRows.splice(' + i + ',1);renderPayment();">X</button></td>' +
            '<td><select onchange="paymentRows[' + i + '].PaymentTermId=parseInt(this.value,10)||0;paymentRows[' + i + '].PaymentTerm=this.options[this.selectedIndex].textContent;renderPayment()">' + paymentTermOptions(r.PaymentTermId) + '</select></td>' +
            '<td><input type="number" min="0" value="' + (r.DueDays || 0) + '" oninput="paymentRows[' + i + '].DueDays=parseInt(this.value,10)||0;recalcPaymentDueDate(' + i + ')"></td>' +
            '<td><select onchange="paymentRows[' + i + '].BaseDueDateTypeId=parseInt(this.value,10)||0;recalcPaymentDueDate(' + i + ');renderPayment()">' + viewComboOptions('PaymentBaseDate', r.BaseDueDateTypeId) + '</select></td>' +
            '<td><input type="date" value="' + esc((r.DueDate || '').slice(0, 10)) + '"' + (editableDate ? '' : ' readonly') + ' onchange="paymentRows[' + i + '].DueDate=this.value"></td>' +
            '<td><input type="number" step="0.0001" value="' + (r.pctOfTotal || 0) + '" oninput="paymentRows[' + i + '].pctOfTotal=parseFloat(this.value)||0;paymentAmountReCalculate()"></td>' +
            '<td><input type="number" step="0.001" value="' + (r.dueAmount || 0) + '" oninput="paymentRows[' + i + '].dueAmount=parseFloat(this.value)||0;renderPaymentTotals()"></td>';
        body.appendChild(tr);
    });
    $('totPct').textContent = fmt(pct);
    $('totDue').textContent = fmt(due);
    refreshScheduleDescription();
}

function renderPaymentTotals() {
    var pct = paymentRows.reduce(function (s, r) { return s + (+r.pctOfTotal || 0); }, 0);
    var due = paymentRows.reduce(function (s, r) { return s + (+r.dueAmount || 0); }, 0);
    $('totPct').textContent = fmt(pct);
    $('totDue').textContent = fmt(due);
    refreshScheduleDescription();
}

/* PaymentAmountReCalculate (:1621) - every row's Amount follows %OfTotal of the
   detail TotalAmount sum. */
function paymentAmountReCalculate() {
    var total = detailTotals().total;
    if (total > 0) {
        paymentRows.forEach(function (r) {
            r.dueAmount = r3((+r.pctOfTotal || 0) * total / 100);
        });
    }
    renderPayment();
}

/* pd.DueDate: base type 1 -> docDate + DueDays; base type 4 -> the typed date;
   anything else -> null (:3182-3184). */
function recalcPaymentDueDate(i) {
    var r = paymentRows[i];
    if (!r) return;
    var base = parseInt(r.BaseDueDateTypeId, 10);
    if (base === 1) {
        var doc = val('datDocDate');
        if (doc) {
            var d = new Date(doc + 'T00:00:00');
            d.setDate(d.getDate() + (parseInt(r.DueDays, 10) || 0));
            r.DueDate = ymd(d);
        } else r.DueDate = '';
    } else if (base !== 4) {
        r.DueDate = '';
    }
}

function recalcAllPaymentDueDates() {
    paymentRows.forEach(function (r, i) { recalcPaymentDueDate(i); });
    renderPayment();
}

/* paymentScheduleDescription (:3208) - rebuilt in the desktop's exact format so
   what the master stores matches what the desktop would have stored. */
function refreshScheduleDescription() {
    var rows = buildPaymentRows();
    var s = rows.map(function (x) {
        return '[Term:' + (x.PaymentTermId || 0) + ':' + (x.PaymentTerm || '')
             + ',DueDays:' + (x.DueDays || 0)
             + ',%OfTotal:' + (x.pctOfTotal || 0)
             + ',DueAmount:' + (x.dueAmount || 0)
             + ',BaseDateType:' + (x.BaseDueDateTypeId || 0) + ']';
    }).join(', ');
    var box = $('txtPaymentScheduleRemarks');
    if (box) box.value = s;
}

/* ------------------------------------------------------- inquiry mapping */

function renderInquiryMapping() {
    var body = $('grdInquiryMappingBody');
    if (!body) return;
    body.innerHTML = '';
    if (!inquiryMappings.length) {
        body.innerHTML = '<tr><td colspan="8">No buyer inquiries mapped. Use Load Inquiry.</td></tr>';
        return;
    }
    inquiryMappings.forEach(function (r, i) {
        var tr = document.createElement('tr');
        tr.innerHTML =
            '<td><button type="button" class="danger" onclick="soDeleteMapping(' + i + ')">X</button></td>' +
            '<td><a href="#" class="doc-code" onclick="event.preventDefault();soOpenInquiry(' + i + ')">'
                + esc(r.inquiryNo || r.InquiryBookingNo || r.inquiryBookingMasterId) + '</a></td>' +
            '<td>' + esc(r.buyerName || r.BuyerName || '') + '</td>' +
            '<td>' + esc(r.itemName || r.ItemName || '') + '</td>' +
            '<td style="text-align:right;">' + fmt(mapItemWeight(r)) + '</td>' +
            '<td style="text-align:right;">' + fmt(mapBookedWeight(r)) + '</td>' +
            '<td style="text-align:right;">' + fmt(mapBalanceWeight(r)) + '</td>' +
            '<td><input type="number" step="0.001" value="' + (r.itemNetWeight || 0) + '" ' +
                 'onchange="soMappingWeightChanged(' + i + ', this)"></td>';
        body.appendChild(tr);
    });
}

/* ReadById fills the mapping grid from the saved rows (:3556):
   ItemWeight <- InquiryBookingQty, BookedWeight <- UsedinquiryBookingWeightInPoOffer,
   BalanceWeight <- inquiryBookingBalWeight, CurrentAllocationWeight <- itemNetWeight. */
function pick(r, a, b) { return (r[a] !== undefined && r[a] !== null) ? r[a] : r[b]; }
function mapItemWeight(r)    { return pick(r, 'itemWeight', 'InquiryBookingQty'); }
function mapBookedWeight(r)  { return pick(r, 'bookedWeight', 'UsedinquiryBookingWeightInPoOffer'); }
function mapBalanceWeight(r) { return pick(r, 'balanceWeight', 'inquiryBookingBalWeight'); }

/* grdSaleOrderMapping_CellUpdated (:2728-2742, wired to grdInquiryMapping at :6603):
   an allocation above the row's BalanceWeight is refused and set back to it. */
function soMappingWeightChanged(i, box) {
    var r = inquiryMappings[i];
    if (!r) return;
    var cur = parseFloat(box.value) || 0;
    var bal = +mapBalanceWeight(r) || 0;
    if (cur > bal) {
        message('Current Allocation Weight:' + fmt(cur) + ' can\'t be greater than Balance weight:' + bal, true);
        cur = bal;
        box.value = bal;
    }
    r.itemNetWeight = cur;
}

function soDeleteMapping(i) {
    var r = inquiryMappings[i];
    if (!r) return;
    if (r.SupplierOfferBuyerInquiryMappingDetailId > 0) {
        if (!confirm('Are you sure to Delete?')) return;
        var gone = Object.assign({}, r);
        gone.actionTypeId = 3;
        removedMappings.push(gone);
    }
    inquiryMappings.splice(i, 1);
    renderInquiryMapping();
}

/* The document code on a mapping row is clickable and opens the Buyer Inquiry
   that produced it, by its real id. */
function soOpenInquiry(i) {
    var r = inquiryMappings[i];
    if (!r || !r.inquiryBookingMasterId) { message('This row carries no inquiry id.', true); return; }
    window.open('/commission/buyer-inquiry-booking?id=' + encodeURIComponent(r.inquiryBookingMasterId), '_blank');
}

/* btnLoadSo_Click (:4713) opens frmBuyerInquiryLoaderForOffer, which returns
   real inquiryBookingMasterId / inquiryBookingDetailId pairs. Nothing is faked
   here: a typed number would not create that link, and Insert():3233-3239
   refuses a row whose ids are zero. */
function soLoadInquiry() {
    message('The Buyer Inquiry picker for this screen is not built yet. The desktop opens '
          + 'frmBuyerInquiryLoaderForOffer and stores real inquiryBookingMasterId / '
          + 'inquiryBookingDetailId values - typing a document number would not create that '
          + 'link, so nothing is fabricated here.', true);
}

/* -------------------------------------------------------------- validation */

/* formvalidation (:2891) then Insert()'s per-row refusals, in the desktop's
   order and with its own message strings. The server runs all of these again in
   PurchaseOrderMasterCmagtValidator - this copy only saves a round trip. */
function soValidate() {
    if (!intOf('cmbCommissionAgent')) return 'Please Select Commission Agent / Broker';
    if (!intOf('cmbSupplierName'))    return 'Please Select Supplier Name';

    var scheduleTotal = paymentRows.reduce(function (s, r) { return s + (+r.dueAmount || 0); }, 0);
    if (scheduleTotal === 0) {
        if (!intOf('cmbPaymentTerm')) return 'Please Select Paymrnt Term';
        if (intOf('cmbPaymentTerm') === 2 && intOf('txtDueDays') === 0) return 'Due Days field is required';
    }
    if (!intOf('cmbDeliveryTerm')) return 'Please Select Delivery Term';
    if (intOf('txtDeliveryDays') === 0) return 'Delivery Days field is required';
    if (!detailRows.length) return 'Detail record not found. Please check!';

    for (var i = 0; i < detailRows.length; i++) {
        var d = detailRows[i], n = i + 1;
        if (!d.inventoryParentCategoryId) return 'Parent Category Field Required in row#' + n;
        if (!d.itemId)        return 'Item Name Field Required in row#' + n;
        if (!d.cropYearId)    return 'Crop Year Field Required in row#' + n;
        if (!d.packingTypeId) return 'Packing Type Field Required in row#' + n;
        if (!d.packUomId)     return 'Pack Uom Field Required in row#' + n;
        if (!(d.itemQty > 0))    return 'Qty Field Required in row#' + n;
        if (!(d.itemWeight > 0)) return 'Weight Field Required in row#' + n;
        if (!(d.itemRate > 0))   return 'Rate Field Required in row#' + n;
        if (!d.rateUomId)     return 'Rate Uom Field Required in row#' + n;
        if (!(d.ItemAmount > 0)) return 'Amount Field Required in row#' + n;
        var anyTax = d.TaxNameId > 0 || d.TaxPercent > 0 || d.TaxAmount > 0;
        if (anyTax) {
            if (!d.TaxNameId)      return 'Tax Name Field Required in row#' + n;
            if (!(d.TaxPercent > 0)) return 'Tax Percent Field Required in row#' + n;
            if (!(d.TaxAmount > 0))  return 'Tax Amount Field Required in row#' + n;
        }
        if (!(d.TotalAmount > 0)) return 'Total Amount Field Required in row#' + n;
    }

    /* empty bags - skipped entirely under the FOC term (:3091) */
    if (ebTermId() !== 3) {
        for (var j = 0; j < emptyBagRows.length; j++) {
            var e = emptyBagRows[j];
            if (!(e.weightCutKg > 0)) return 'WeightCut filed required In Empty bags Grid...';
            var range = packingTypeRange(e.PackingTypeId);
            if (range && (e.weightCutKg < range.min || e.weightCutKg > range.max)) {
                return 'Weight Cut Should be in Range of: ' + fmt(range.min) + ' to ' + fmt(range.max)
                     + '\nFor Packing Type:' + range.name;
            }
        }
    }
    for (var k = 0; k < emptyBagPmRows.length; k++) {
        var pm = emptyBagPmRows[k];
        if (!pm.emptyBagPackingMaterialItemId || !pm.PackingTypeId) continue;  /* :3111 */
        if (!(pm.Rate > 0)) return 'Rate filed required In Empty bags Pm Grid...';
    }

    for (var m = 0; m < paymentRows.length; m++) {
        if (!paymentRows[m].PaymentTermId) return 'Payment Term Required in row#' + (m + 1);
        if (parseInt(paymentRows[m].PaymentTermId, 10) === 2 && !(paymentRows[m].DueDays > 0)) {
            return 'Due Days Required In case Of Credit row in row#' + (m + 1);
        }
    }

    /* :3210-3220 - DetailSumAmount is the sum of TotalAmount (tax included). */
    var rows = buildPaymentRows();
    var paid = rows.reduce(function (s, r) { return s + (+r.dueAmount || 0); }, 0);
    var pct  = Math.round(rows.reduce(function (s, r) { return s + (+r.pctOfTotal || 0); }, 0) * 10000) / 10000;
    var detailSum = detailTotals().total;
    if (Math.abs(paid - detailSum) > 0.3) {
        return 'Payment Detail Amount:' + fmt(paid) + ' Not Equal to Total Amount:' + fmt(detailSum);
    }
    if (Math.abs(100 - pct) > 0.01) return 'Payment Detail Total% not near to 100';

    /* mapping (:3226-3300) */
    var supplierId = intOf('cmbSupplierName');
    var offerWeight = {};
    detailRows.forEach(function (d) {
        offerWeight[d.itemId] = (offerWeight[d.itemId] || 0) + (+d.itemWeight || 0);
    });
    var mappedWeight = {}, unknown = [];
    for (var p = 0; p < inquiryMappings.length; p++) {
        var mp = inquiryMappings[p];
        if (!mp.inquiryBookingMasterId) return 'Inquiry Id not found. please check';
        if (!mp.inquiryBookingDetailId) return 'Inquiry detail Id not found. please check';
        if (!mp.buyerId) return 'buyer not found. please check';
        if (!mp.itemId)  return 'Item not found. please check';
        if (!(mp.itemNetWeight > 0)) return 'Allocation weight should be greater than \'0\'. please check';
        if (mp.buyerId === supplierId) {
            return 'Row ' + (p + 1) + ': Supplier and buyer cannot be the same in mapping grid.';
        }
        if (!(mp.itemId in offerWeight)) unknown.push(mp.itemName || ('ItemId ' + mp.itemId));
        else mappedWeight[mp.itemId] = (mappedWeight[mp.itemId] || 0) + (+mp.itemNetWeight || 0);
    }
    if (unknown.length) {
        return 'Following item(s) are not present in Purchase Order Detail: '
             + unknown.filter(function (v, i2, a) { return a.indexOf(v) === i2; }).join(', ')
             + '. Please delete those rows which contain these items from mapping.';
    }
    for (var itemId in mappedWeight) {
        if (mappedWeight[itemId] > offerWeight[itemId]) {
            return 'Mapped weight (' + fmt(mappedWeight[itemId]) + ') cannot be greater than '
                 + 'Offer weight (' + fmt(offerWeight[itemId]) + ') for Item ' + itemId;
        }
    }
    return null;
}

/* ------------------------------------------------------------ persistence */

/* comm.agentTypeId 1 = commission, 2 = brokery; each row is sent only when an
   amount was produced AND an account chosen (:3176-3196). commissionAgentId is
   the ACCOUNT combo's id, not the header Commission Agent. */
function buildCommissionRows() {
    var out = [];
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

/* :3193-3207 - when the schedule grid is empty the desktop synthesizes ONE row
   from the header term: base type 1, due date = doc date + due days, 100%,
   amount = the detail TotalAmount sum. */
function buildPaymentRows() {
    if (paymentRows.length) return paymentRows;
    if (!intOf('cmbPaymentTerm')) return [];
    var t = detailTotals();
    var doc = val('datDocDate');
    var due = '';
    if (doc) {
        var d = new Date(doc + 'T00:00:00');
        d.setDate(d.getDate() + intOf('txtDueDays'));
        due = ymd(d);
    }
    return [{
        purchaseOrderPaymentDetailId: 0,
        PaymentTermId: intOf('cmbPaymentTerm'),
        PaymentTerm: textOf('cmbPaymentTerm'),
        DueDays: intOf('txtDueDays'),
        BaseDueDateTypeId: 1,
        DueDate: due,
        pctOfTotal: 100,
        dueAmount: r3(t.total)
    }];
}

/* The three *Description strings the master carries (:3115, :3167-3172, :3208).
   They are built in the desktop's exact format so the stored text matches. */
function expenseDescription(rows) {
    return rows.map(function (x) {
        return '[Item:' + x.ItemId + ':' + (x.OtherItemName || '')
             + ', Qty:' + (x.Qty || 0) + ',Rate:' + (x.rate || 0) + ',Amount:' + (x.amount || 0) + ']';
    }).join(', ');
}
function emptyBagDescription(rows) {
    return rows.map(function (x) {
        return '[PackingType:' + x.PackingTypeId + ':' + (x.PackingType || '')
             + ', WeightCut:' + (x.weightCutKg || 0) + ']';
    }).join(', ');
}
function emptyBagPmDescription(rows) {
    return rows.map(function (x) {
        return '[PackingType:' + x.PackingTypeId + ':' + (x.PackingType || '')
             + ',Rate:' + (x.Rate || 0) + '],EmptyBagItem:' + (x.EmptyBagItem || '');
    }).join(', ');
}

function buildPayload() {
    var recId = intOf('purchaseOrderMasterId');

    var details = detailRows.map(function (r) {
        var copy = Object.assign({}, r);
        copy.actionTypeId = (recId > 0 && copy.purchaseOrderDetailId > 0) ? 2 : 1;   /* :3064 */
        if (recId === 0) copy.purchaseOrderDetailId = 0;
        return copy;
    }).concat(recId > 0 ? removedDetailRows : []);

    var expenses = expenseRows.filter(function (r) {
        return r.ItemId && (+r.amount || 0) > 0;                                     /* :3093 */
    }).map(function (r) {
        /* :3102-3106 - remarks are trimmed, and a blank (or "0") remark becomes
           "Expense : <item>  Qty<qty>  @<rate>". */
        var c = Object.assign({}, r);
        var rm = (c.remarks === null || c.remarks === undefined) ? '' : String(c.remarks).trim();
        c.remarks = (rm === '' || rm === '0')
            ? 'Expense : ' + String(c.OtherItemName || '').trim() + '  Qty' + (+c.Qty || 0) + '  @' + (+c.rate || 0)
            : rm;
        return c;
    });
    var pmRows = emptyBagPmRows.filter(function (r) {
        return r.emptyBagPackingMaterialItemId > 0 && r.PackingTypeId > 0;           /* :3111 */
    });

    var mappings = inquiryMappings.map(function (r) {
        var copy = Object.assign({}, r);
        copy.actionTypeId = copy.SupplierOfferBuyerInquiryMappingDetailId > 0 ? 2 : 1;
        copy.supplierId = intOf('cmbSupplierName');                                  /* :3242 */
        return copy;
    }).concat(recId > 0 ? removedMappings : []);

    /* remarksHeader falls back to the first detail row's remarks when the header
       box is blank (:3085) - the desktop does this inside the detail loop. */
    var remarks = val('txtRemarks');
    if (!remarks || !remarks.trim()) {
        for (var i = 0; i < detailRows.length; i++) {
            if (detailRows[i].remarks) { remarks = detailRows[i].remarks; break; }
        }
    }

    return jacksonKeys({
        purchaseOrderMasterId: recId,
        documentTypeId: DOCUMENT_TYPE_ID,
        docNo: intOf('txtDocNo'),
        docDate: val('datDocDate'),
        ValidityDate: val('datExpiryDate'),          /* :2996 obj.ValidityDate = datExpiryDate */
        statusId: 1,                                 /* :2997 */
        commissionAgentId: intOf('cmbCommissionAgent'),
        supplierId: intOf('cmbSupplierName'),
        DeliveryToPartyId: intOf('cmbDeliveryToParty'),
        shipToAddressId: intOf('cmbShipToAddress'),
        ShipToAddress: val('txtShipToAddress'),
        deliveryTermId: intOf('cmbDeliveryTerm'),
        deliveryDays: intOf('txtDeliveryDays'),
        deliveryStartDate: val('datDeliveryStartDate'),
        remarksHeader: remarks,
        companyId: intOf('cmbCompany'),
        branchId: intOf('cmbBranch'),
        isApproved: false,                           /* :3046 - never from the client */
        isWhtApplied: $('chkWithHoldingTaxApplied').checked,
        isSupplierOtherChargesAllowed: $('chkOtherExpenseAllowed').checked,
        EBWeightDeductionTermId: ebTermId(),

        paymentScheduleDescription: val('txtPaymentScheduleRemarks'),
        purchaseOrderSupplierExpenseDetailDescription: expenseDescription(expenses),
        purchaseOrderEmptyBagDetailDescription:   emptyBagDescription(emptyBagRows),
        purchaseOrderEmptyBagDetailDescriptionII: emptyBagPmDescription(pmRows),

        purchaseOrderDetailList: details,
        purchaseOrderSupplierExpenseDetailList: expenses,
        purchaseOrderEmptyBagDetailList: emptyBagRows.concat(pmRows),
        purchaseOrderCommissionDetailList: buildCommissionRows(),
        purchaseOrderPaymentDetailList: buildPaymentRows(),
        supplierOfferBuyerInquiryMappingDetailList: mappings
    });
}

function soSave() {
    if (busy || !soGuard('save')) return;
    var err = soValidate();
    if (err) { message(err, true); soRelease('save'); return; }

    var isUpdate = intOf('purchaseOrderMasterId') > 0;
    if (!confirm(isUpdate ? 'Are you sure to Update?' : 'Are you sure to Save?')) {
        soRelease('save');
        return;
    }
    setBusy(true, isUpdate ? 'btnUpdate' : 'btnSave');
    fetch(API + '/save', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(buildPayload())
    })
        .then(function (r) {
            return r.text().then(function (t) {
                var d;
                try { d = JSON.parse(t); } catch (e) { d = { message: t }; }
                if (!r.ok) throw new Error(d.message || d.error || (r.status + ' ' + r.statusText));
                return d;
            });
        })
        .then(function (d) {
            var newId = d.id || d.purchaseOrderMasterId || d.Id;
            message(isUpdate ? 'Update Successfully' : 'Save Successfully');
            if (newId) soLoad(newId); else soNew();
        })
        .catch(function (e) { message(e.message, true); })
        .finally(function () { setBusy(false); soRelease('save'); });
}

function soLoad(id) {
    if (!id) return;
    setBusy(true);
    getJson(API + '/' + id)
        .then(function (res) {
            var so = (res && res.data) ? res.data : res;
            if (!so || (res && res.status === 'ERROR')) { message('Record Not Found', true); return; }

            $('purchaseOrderMasterId').value = so.purchaseOrderMasterId || id;
            $('txtDocNo').value = so.docNo || '';
            $('datDocDate').value = (so.docDate || '').slice(0, 10);
            $('datExpiryDate').value = (so.ValidityDate || so.validityDate || '').slice(0, 10);
            $('cmbCompany').value = so.companyId || 0;
            $('cmbBranch').value = so.branchId || 0;
            $('cmbCommissionAgent').value = so.commissionAgentId || 0;
            $('cmbSupplierName').value = so.supplierId || 0;
            $('cmbDeliveryToParty').value = so.DeliveryToPartyId || so.deliveryToPartyId || 0;
            soDeliveryPartyChanged();
            $('cmbShipToAddress').value = so.shipToAddressId || 0;
            $('txtShipToAddress').value = so.ShipToAddress || so.shipToAddress || '';
            $('cmbDeliveryTerm').value = so.deliveryTermId || 0;
            $('txtDeliveryDays').value = so.deliveryDays || 0;
            $('datDeliveryStartDate').value = (so.deliveryStartDate || '').slice(0, 10);
            $('txtRemarks').value = so.remarksHeader || '';
            $('chkWithHoldingTaxApplied').checked = !!(so.isWhtApplied);
            $('chkOtherExpenseAllowed').checked = !!(so.isSupplierOtherChargesAllowed);
            var ebId = so.EBWeightDeductionTermId || so.ebWeightDeductionTermId || 0;
            var rb = document.querySelector('input[name="ebTerm"][value="' + ebId + '"]');
            if (rb) rb.checked = true;

            detailRows  = so.purchaseOrderDetailList || [];
            expenseRows = so.purchaseOrderSupplierExpenseDetailList || [];
            var allBags = so.purchaseOrderEmptyBagDetailList || [];
            emptyBagRows   = allBags.filter(function (b) { return parseInt(b.entryTypeId, 10) !== 2; });
            emptyBagPmRows = allBags.filter(function (b) { return parseInt(b.entryTypeId, 10) === 2; });
            paymentRows = so.purchaseOrderPaymentDetailList || [];
            inquiryMappings = so.supplierOfferBuyerInquiryMappingDetailList || [];
            removedDetailRows = [];
            removedMappings = [];

            var comm = so.purchaseOrderCommissionDetailList || [];
            comm.forEach(function (c) {
                if (parseInt(c.agentTypeId, 10) === 1) {
                    $('cmbCommissionAc').value = c.commissionAgentId || 0;
                    $('cmbCommType').value = c.commissionTypeId || 0;
                    $('txtCommRate').value = c.commissionRate || 0;
                    $('cmbCommUom').value = c.rateUomId || 0;
                    $('txtCommAmount').value = c.commissionAmount || 0;
                } else if (parseInt(c.agentTypeId, 10) === 2) {
                    $('cmbBrokeryAc').value = c.commissionAgentId || 0;
                    $('cmbBrokeryType').value = c.commissionTypeId || 0;
                    $('txtBrokeryRate').value = c.commissionRate || 0;
                    $('cmbBrokeryRateUom').value = c.rateUomId || 0;
                    $('txtBrokeryAmount').value = c.commissionAmount || 0;
                }
            });

            seedExpenseRows();
            seedEmptyBagRows();
            renderDetail();
            renderEmptyBagsPm();
            renderPayment();
            renderInquiryMapping();
            soEbTermChanged();

            $('btnUpdate').disabled = false;
            $('btnDelete').disabled = false;
            $('btnSave').disabled = true;
            message('Loaded document ' + (so.docNo || id));
        })
        .catch(function (e) { message('Load failed: ' + e.message, true); })
        .finally(function () { setBusy(false); });
}

function soDelete() {
    if (busy || !soGuard('delete')) return;
    var id = intOf('purchaseOrderMasterId');
    if (!id) { soRelease('delete'); message('No record found to Delete', true); return; }
    if (!confirm('Are you sure to Delete?')) { soRelease('delete'); return; }
    setBusy(true, 'btnDelete');
    fetch(API + '/delete/' + id, { method: 'POST' })
        .then(function (r) {
            return r.text().then(function (t) {
                var d;
                try { d = JSON.parse(t); } catch (e) { d = {}; }
                if (!r.ok) throw new Error(d.message || (r.status + ' ' + r.statusText));
                return d;
            });
        })
        .then(function (d) {
            if (d.success === false) { message(d.message || 'Delete was refused.', true); return; }
            message('Delete Record Successfully');
            soNew();
        })
        .catch(function (e) { message('Delete failed: ' + e.message, true); })
        .finally(function () { setBusy(false); soRelease('delete'); });
}

/* FormHistory - the same columns the desktop's grdHistory shows. The Doc No is
   a link that loads that record, per the cross-cutting rule. */
function soLoadHistory() {
    if (busy || !soGuard('history')) return;
    setBusy(true, 'btnHistory');
    $('wrapHistory').style.display = '';
    var q = '?fromDate=' + encodeURIComponent(val('histFromDate'))
          + '&toDate=' + encodeURIComponent(val('histToDate'));
    getJson(API + '/history' + q)
        .then(function (rows) {
            var body = $('grdHistoryBody');
            body.innerHTML = '';
            if (!rows || !rows.length) {
                body.innerHTML = '<tr><td colspan="8">No documents in this range.</td></tr>';
                return;
            }
            rows.forEach(function (r) {
                var id = r.purchaseOrderMasterId || r.PurchaseOrderMasterId || r.Id;
                var docNo = r.docNo || r.DocNo || '';
                var tr = document.createElement('tr');
                tr.style.cursor = 'pointer';
                tr.ondblclick = function () { soLoad(id); };
                tr.innerHTML =
                    '<td><a href="#" class="doc-code" onclick="event.preventDefault();soLoad(' + id + ')">' + esc(docNo) + '</a></td>' +
                    '<td>' + esc(String(r.docDate || r.DocDate || '').slice(0, 10)) + '</td>' +
                    '<td>' + esc(r.SupplierName || r.supplierName || '') + '</td>' +
                    '<td>' + esc(r.CommissionAgentName || r.commissionAgentName || '') + '</td>' +
                    '<td>' + esc(r.DeliveryToPartyName || r.DeliveryToParty || r.deliveryToParty || '') + '</td>' +
                    '<td>' + esc(String(r.ValidityDate || r.validityDate || '').slice(0, 10)) + '</td>' +
                    '<td>' + esc(r.status || r.Status || '') + '</td>' +
                    '<td>' + esc(r.EntryUserName || r.entryUserName || '') + '</td>';
                body.appendChild(tr);
            });
        })
        .catch(function (e) { message('History failed: ' + e.message, true); })
        .finally(function () { setBusy(false); soRelease('history'); });
}

/* ---------------------------------------------------------------- toolbar */

function soNew() {
    $('purchaseOrderMasterId').value = 0;
    $('txtDocNo').value = '';
    detailRows = []; removedDetailRows = [];
    expenseRows = []; emptyBagRows = []; emptyBagPmRows = []; paymentRows = [];
    inquiryMappings = []; removedMappings = [];
    selectedIdx = 0; editingIdx = -1;

    ['txtShipToAddress', 'txtRemarks', 'txtPaymentScheduleRemarks', 'txtRemarksDetail']
        .forEach(function (id) { $(id).value = ''; });
    ['txtCommRate', 'txtCommAmount', 'txtBrokeryRate', 'txtBrokeryAmount']
        .forEach(function (id) { $(id).value = 0; });
    ['cmbCommissionAc', 'cmbCommType', 'cmbCommUom',
     'cmbBrokeryAc', 'cmbBrokeryType', 'cmbBrokeryRateUom']
        .forEach(function (id) { if ($(id)) $(id).value = 0; });

    clearDetailEntry();

    var today = ymd(new Date());
    $('datDocDate').value = today;
    $('datDocDate').disabled = false;
    $('datDeliveryStartDate').value = today;                  /* Reset():3941 */
    $('txtDeliveryDays').value = portalDefaults.defaultDeliveryDays || 1;  /* _Load:590 */
    soCalculateExpiryDate();

    applyPortalDefaults();

    seedExpenseRows();
    seedEmptyBagRows();
    renderDetail();
    renderEmptyBagsPm();
    renderPayment();
    renderInquiryMapping();
    soEbTermChanged();

    $('btnSave').disabled = false;
    $('btnUpdate').disabled = true;
    $('btnDelete').disabled = true;
    $('wrapHistory').style.display = 'none';
    message('');

    getJson(API + '/generate-no')
        .then(function (d) { $('txtDocNo').value = (d && (d.docNo || d.DocNo)) || ''; })
        .catch(function () { /* the procedure allocates the real number at save time anyway */ });
}

function soRefresh() {
    if (busy || !soGuard('refresh')) return;
    setBusy(true, 'btnRefresh');
    loadLookups().finally(function () {
        setBusy(false);
        soRelease('refresh');
        applyPortalDefaults();
        seedExpenseRows();
        seedEmptyBagRows();
        renderEmptyBagsPm();
        message('Lookups reloaded.');
    });
}

function soAttachments() {
    message('Attachments (Proc_DMSAttachments_Insert) are not built on this screen yet. '
          + 'The desktop stores them through FormHelper.UpdateAttachmentsForObject (:3308).', true);
}

function soPrint() {
    if (!intOf('purchaseOrderMasterId')) { message('No Data found to display', true); return; }
    window.print();
}

function soAddShipToAddress() {
    message('Defining a new ship-to address (BtnAddShiptoAddress_Click, :4801) is not built on '
          + 'this screen yet.', true);
}

/* RadBusinessName_CheckedChanged (:929) - re-bind what the party combos DISPLAY.
   A row that lacks NickName or PartyCode keeps its CompanyName rather than
   showing blank. */
function soPartyNameModeChanged() {
    var mode = (document.querySelector('input[name="partyNameMode"]:checked') || {}).value || 'business';
    var field = mode === 'nick' ? 'NickName' : (mode === 'code' ? 'PartyCode' : 'CompanyName');
    [['cmbCommissionAgent', 'commissionAgents'],
     ['cmbSupplierName',    'suppliers'],
     ['cmbDeliveryToParty', 'deliveryParties'],
     ['cmbCommissionAc',    'accounts'],
     ['cmbBrokeryAc',       'accounts']]
        .forEach(function (pair) {
            var sel = $(pair[0]);
            if (!sel) return;
            var keep = sel.value;
            var rows = (lookupData[pair[1]] || []).map(function (r) {
                return { Id: r.Id, Label: r[field] || r.CompanyName };
            });
            fillSelect(pair[0], rows, 'Id', 'Label');
            sel.value = keep;
        });
}

/* frmSupplierOfferCmagt_KeyDown (:4532) - the form's own shortcuts. */
function soKeyDown(e) {
    if (e.key === 'F2') { e.preventDefault(); soSave(); }
    else if (e.key === 'F3') { e.preventDefault(); soNew(); }
    else if (e.key === 'F5') { e.preventDefault(); soRefresh(); }
    else if (e.key === 'Escape' && editingIdx >= 0) { e.preventDefault(); soCancelDetailEdit(); }
    else if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); soAddOrUpdateDetail(); }
}

/* ---------------------------------------------------------------- start-up */

document.addEventListener('DOMContentLoaded', function () {
    Array.prototype.forEach.call(document.querySelectorAll('input[name="partyNameMode"]'), function (rb) {
        rb.addEventListener('change', soPartyNameModeChanged);
    });
    document.addEventListener('keydown', soKeyDown);

    /* Deep link: /commission/supplier-offer?id=123 opens that record, so a code
       clicked on another screen lands on the right document. */
    var wantId = new URLSearchParams(window.location.search).get('id');

    setBusy(true);
    loadLookups().finally(function () {
        setBusy(false);
        soNew();
        soShowTab(null, 'tabDetail');
        if (wantId) soLoad(parseInt(wantId, 10));
    });
});
