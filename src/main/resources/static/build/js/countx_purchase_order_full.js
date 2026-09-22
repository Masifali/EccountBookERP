
/* Reads what a combo is actually showing.
 *
 * Deliberately native: selectedIndex + options, never jQuery .val(). jQuery routes a select
 * through valHooks.select, which the desktop-combo component hooks, and this helper is called
 * from inside change handlers - exactly the path that was blowing the stack. Reading the DOM
 * directly cannot re-enter anything. Returns '' only when nothing is chosen. */
function comboText(sel) {
    var el = document.querySelector(sel);
    if (!el) { return ''; }
    var i = el.selectedIndex;
    if (i >= 0 && el.options[i]) {
        var t = (el.options[i].textContent || '').trim();
        if (t && t !== '0') { return t; }
    }
    var v = (el.value || '').toString().trim();
    return (v && v !== '0') ? v : '';
}
/**
 * Purchase Order — Golden Master DitTo Copy Event Controller
 */

let lineItems = [];
let emptyBagItems = [];
let expenseItems = [];
let chargeToProductItems = [];
let isEditMode = false;
let paymentTermsDetailItems = [];

let allSuppliers = [];
let allBrokers = [];
let allCommAgents = [];
let allBookingPersons = [];
let allItems = [];
let allCities = [];

// Real per-item UOM schedule rows (Id, ItemId, UOMCode, Equivalent, BaseRateUom, BasePackUom), fetched
// once for the whole Organization/Company, ditto desktop's UOMFill() -> Sp_UOMSchedule_GetAllMethod
// @Activity='ReadByOrganizationCompanyId'. The Pack UOM / Rate UOM dropdowns filter this same list by
// the selected item's ItemId client-side, ditto desktop's bindRateUomAndItemPackUom().
let uomScheduleList = [];

let editingLineIdx = -1;
let editingEbIdx = -1;
let currentPoMasterId = 0;
let emptyBagTypes = [];
let emptyBagItemOptions = [];
let emptyBagPackingTypes = [];
let otherItemsForExpense = [];
let coaAccountsForCharge = [];
let paymentTermsOptions = [];

$(document).ready(function() {
    initForm();
    bindKeyboardShortcuts();
});

function initForm() {
    setWinDefaultDates();
    fetchNextDocNo();
    loadDropdowns();
    fetchComboDefaults();
    preloadSearchData();
    initSearchableDropdowns();

    // Ditto of desktop PurchsaeOrder_Load -> AddRowInvEmptyBagsGrid(): a freshly-opened form
    // (new Purchase Order) seeds the Jute Bags / PP Bags default rows immediately.
    loadDefaultEmptyBagRows();

    // Ditto of desktop PurchsaeOrder_Load's grid-seeding for the other three real-data tabs:
    // Supplier Expense seeds one row per real "Other Item" master row; Account Credit _Charge to
    // Product and Payment Detail each seed a single blank row.
    loadDefaultExpenseRows();
    loadDefaultChargeRows();
    loadDefaultPaymentRows();
}

/* Superseded by build/js/countx_desktop_combo.js, which renders the multi-column drop grids for
   every Sale and Purchase screen through one implementation. The bespoke 2-column and 4-column
   formatters that used to live here are gone: their column sets were wrong for this form anyway -
   the supplier combo showed two columns where dtSupplier shows four, and the 4-column header was
   hard-captioned "Commission Agent" for every combo that used it.

   This now only initialises the plain ones and wires the change listeners; DesktopCombo.init()
   runs afterwards and skips anything already initialised. */
function initSearchableDropdowns() {
    /* select2 initialisation removed - build/js/countx_desktop_combo.js now renders every combo
       on this screen, without jQuery or select2, so Purchase Order and Sale Order draw the same
       control. Leaving select2 on some of them would have produced two different-looking
       dropdowns on one form. The change listeners below are still wired here. */
    if (false) {
        $('.select2').each(function() {
            $(this).select2({
                width: '100%',
                dropdownAutoWidth: true
            });
        });

        $('.select2-2col').each(function() {
            $(this).select2({
                width: '100%',
                dropdownAutoWidth: true,
                templateResult: format2Col,
                templateSelection: format2ColSelection,
                escapeMarkup: function (m) { return m; }
            }).on('select2:open', function() {
                setTimeout(function() {
                    if ($('.select2-results .select2-2col-header').length === 0) {
                        $('.select2-results').prepend(
                            '<div class="select2-2col-header">' +
                                '<span>Name / Description</span>' +
                                '<span>Code / Type</span>' +
                            '</div>'
                        );
                    }
                }, 10);
            });
        });
        $('.select2-4col').each(function() {
            $(this).select2({
                width: '100%',
                dropdownAutoWidth: true,
                templateResult: format4Col,
                templateSelection: format4ColSelection,
                escapeMarkup: function (m) { return m; }
            }).on('select2:open', function() {
                setTimeout(function() {
                    if ($('.select2-results .select2-4col-header').length === 0) {
                        $('.select2-results').prepend(
                            '<div class="select2-4col-header">' +
                                '<span style="flex: 3;">Commission Agent</span>' +
                                '<span style="flex: 2; margin-left: 6px;">PartyCode</span>' +
                                '<span style="flex: 2; margin-left: 6px;">CityName</span>' +
                                '<span style="flex: 2; margin-left: 6px;">MobileNo</span>' +
                            '</div>'
                        );
                    }
                }, 10);
            });
        });
    }

    bindDropdownChangeListeners();
}

function format2Col(state) {
    if (!state.id) return state.text;
    const code = $(state.element).data('code') || $(state.element).attr('data-code') || '';
    const name = state.text;
    return `
        <div class="select2-2col-row">
            <span class="select2-2col-title">${escapeHtml(name)}</span>
            <span class="select2-2col-level">${escapeHtml(code)}</span>
        </div>
    `;
}

function format2ColSelection(state) {
    if (!state.id) return state.text;
    const code = $(state.element).data('code') || $(state.element).attr('data-code') || '';
    if (code) {
        return `${state.text} (${code})`;
    }
    return state.text;
}

function format4Col(state) {
    if (!state.id || state.id == '0') return state.text;
    const name = state.text;
    const code = $(state.element).data('code') || '';
    const city = $(state.element).data('city') || '';
    const mobile = $(state.element).data('mobile') || '';
    return `
        <div class="select2-4col-row">
            <span class="select2-4col-col1">${escapeHtml(name)}</span>
            <span class="select2-4col-col2">${escapeHtml(code)}</span>
            <span class="select2-4col-col3">${escapeHtml(city)}</span>
            <span class="select2-4col-col4">${escapeHtml(mobile)}</span>
        </div>
    `;
}

function format4ColSelection(state) {
    if (!state.id || state.id == '0') return state.text;
    const code = $(state.element).data('code') || '';
    if (code) {
        return `${state.text} (${code})`;
    }
    return state.text;
}

/* ============================================================
 * The four combo change handlers.
 *
 * WHY THESE ARE NAMED FUNCTIONS AND NOT ANONYMOUS ONES
 * ----------------------------------------------------
 * They used to be anonymous functions bound only as .on('change.sync', function(){...}),
 * and the template's inline onchange="onSupplierChange()" reached them by calling
 * $('#cmbSupplier').trigger('change.sync').
 *
 * That was an infinite recursion. countx_desktop_combo.js Combo.prototype.fire dispatches a
 * NATIVE change event, and a native change runs the element's inline onchange attribute.
 * jQuery's .trigger() also invokes the element's inline attribute during its bubble walk
 * (handle = ontype && cur[ontype]; handle.apply(cur, data)) - and it resolves the BASE type
 * "change" for that lookup, so the ".sync" namespace does not protect against it. So:
 *
 *     native change -> onSupplierChange() -> trigger('change.sync') -> onSupplierChange() -> ...
 *
 * until "Maximum call stack size exceeded".
 *
 * The cause is removed rather than guarded: each handler is a named function, jQuery binds
 * THAT function, and the template no longer carries an inline onchange at all. A native change
 * - whether from Combo.fire, from a real <select> if the combo failed to initialise, or from
 * the explicit dispatchEvent at clearForm - reaches the jQuery binding exactly once, which is
 * the same "exactly once each" contract countx_desktop_combo.js:531 already relies on.
 * ============================================================ */

/* combsuppname_Leave, PurchsaeOrder.cs :1750 */
function syncSupplierSelection() {
    const suppId = parseInt($('#cmbSupplier').val() || '0');
    if (suppId > 0) {
        const supp = allSuppliers.find(s => s.id === suppId);
        if (supp) {
            $('#hidSupplierId').val(supp.id);
            const displayText = $('#radSupCode').is(':checked')
                ? `${supp.partyCode} - ${supp.companyName}`
                : `${supp.companyName} (${supp.partyCode || 'N/A'})`;
            $('#txtSupplierDisplay').val(displayText);
            onSupplierSelectedEventChain(supp);
        }
    } else {
        $('#hidSupplierId').val('0');
        $('#txtSupplierDisplay').val('');
    }
}

/* CmbBrokeryAccount_Leave -> TotalBrokeryAmountCalculate(), :4536 / :4376 */
function syncBrokerAccountSelection() {
    const brokerId = parseInt($('#cmbBrokerAccount').val() || '0');
    if (brokerId > 0) {
        const broker = allBrokers.find(b => b.id === brokerId);
        if (broker) {
            $('#hidBrokerAccountId').val(broker.id);
            $('#txtBrokerAcDisplay').val(broker.companyName);
            calcBrokery();
        }
    } else {
        $('#hidBrokerAccountId').val('0');
        $('#txtBrokerAcDisplay').val('');
        calcBrokery();
    }
}

/* The desktop has no ValueChanged/Leave handler on the commission agent; it is read only at
   save (:3305). This keeps the hidden id and the commission total in step with the pick. */
function syncCommissionAgentSelection() {
    const agentId = parseInt($('#cmbCommissionAgent').val() || '0');
    if (agentId > 0) {
        const agent = allCommAgents.find(a => a.id === agentId);
        if (agent) {
            $('#hidCommissionAgentId').val(agent.id);
            $('#txtCommAgentDisplay').val(agent.companyName);
            calcCommission();
        }
    } else {
        $('#hidCommissionAgentId').val('0');
        $('#txtCommAgentDisplay').val('');
        calcCommission();
    }
}

/* Likewise the booking person - read only at save (:3140). */
function syncBookingPersonSelection() {
    const personId = parseInt($('#cmbBookingPerson').val() || '0');
    if (personId > 0) {
        const person = allBookingPersons.find(p => p.id === personId);
        if (person) {
            $('#hidBookingPersonId').val(person.id);
            $('#txtBookingPersonDisplay').val(person.partyName || person.description || person.name);
        }
    } else {
        $('#hidBookingPersonId').val('0');
        $('#txtBookingPersonDisplay').val('-- Select --');
    }
}

function bindDropdownChangeListeners() {
    $('#cmbSupplier').off('change.sync').on('change.sync', syncSupplierSelection);
    $('#cmbBrokerAccount').off('change.sync').on('change.sync', syncBrokerAccountSelection);
    $('#cmbCommissionAgent').off('change.sync').on('change.sync', syncCommissionAgentSelection);
    $('#cmbBookingPerson').off('change.sync').on('change.sync', syncBookingPersonSelection);
}

function setWinDefaultDates() {
    const today = new Date();
    const isoDate = ymdLocal(today);
    
    $('#txtDocDate').val(isoDate);
    $('#txtDeliveryStartDate').val(isoDate);
    $('#txtHistoryFromDate').val(isoDate);
    $('#txtHistoryToDate').val(isoDate);
    
    calculateExpiryDate();
    calculatePaymentDueDate();
}

function fetchNextDocNo() {
    fetchNextBranchSrNo();          /* BranchSrNoFill() runs alongside DocumentNoFill() */
    applyScreenDefaults();          /* defaultConfiquration() */
    $.ajax({
        url: '/api/purchase-order/next-doc-no?docType=41',   /* doc type 41 - this is the
             general Purchase module's Purchase Order (Architecture.WinApp.Purchase\PurchsaeOrder.cs,
             po.DocumentTypeId = 41 at :3295). 1052 is the Commission Trading document
             (Cmagt\frmPurchaseOrderCmagt.cs), a different table and procedure family. */
        type: 'GET',
        success: function(res) {
            const code = res ? (res.nextCode || res.docNo) : null;
            if (code) {
                $('#txtDocNo').val(code);
                $('#lblDocNoDisplay').text("PO-2026-" + String(code).padStart(4, '0'));
            }
        }
    });
}

/*
 * BranchSrNoFill() - PurchsaeOrder.cs:1205. The desktop fills the "Branch #" box as soon as the
 * form opens, from Sp_PurchaseOrder_GetAllMethod @Activity='GeneratePurchaseOrderBranchCodeByDocId'
 * (organization + company + document type + BRANCH + financial year). The web box was blank
 * because nothing generated it.
 */
/*
 * defaultConfiquration() - PurchsaeOrder.cs:631/:804/:1641/:1646.
 *
 * Three values the desktop reads from the company's CONFIGURATION on a new order. The page had
 * them written in as literals: Delivery Days 7, JuteBag Cut 0.00, PPBag Cut 0.00 - which is why
 * the desktop showed 1 / 2.25 / 1.25 and the web showed 7 / 0.00 / 0.00 for the same company.
 *
 * The desktop only writes the two cuts when the configured value is non-zero, and formats them
 * "#,##0.###". Both boxes are Enabled = false there, so they are read-only here too.
 */
function applyScreenDefaults() {
    $.ajax({
        url: '/api/purchase-order/screen-defaults',
        type: 'GET',
        success: function (cfg) {
            if (!cfg) { return; }

            var days = parseInt(cfg.orderDefaultDeliveryDays || '0', 10);
            if (!isNaN(days)) { $('#txtDeliveryDays').val(days); }

            var jute = parseFloat(cfg.weightCutForJuteBags || '0');
            if (jute) { $('#txtJuteBagCut').val(fmtCut(jute)); }      /* :1642 - only when non-zero */

            var pp = parseFloat(cfg.weightCutForPPBags || '0');
            if (pp) { $('#txtPPBagCut').val(fmtCut(pp)); }            /* :1647 */

            /* txtJuteBagCut.Enabled = false / txtPpBagCut.Enabled = false */
            $('#txtJuteBagCut, #txtPPBagCut').prop('readonly', true);
        }
    });
}

/* "#,##0.###" - up to three decimals, trailing zeros dropped, thousands separated. */
function fmtCut(n) {
    return n.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 3 });
}

function fetchNextBranchSrNo() {
    $.ajax({
        url: '/api/purchase-order/next-branch-sr-no?docType=41',
        type: 'GET',
        success: function (res) {
            if (res && res.branchSrNo) { $('#txtBranchNo').val(res.branchSrNo); }
        }
    });
}

/*
 * combordercat_Leave - PurchsaeOrder.cs:1663. The "Cat No" box is filled when the Category combo
 * changes, from a DIFFERENT procedure and table (Sp_InvOrderCategory_GetAllMethod
 * @Activity='GenerateOrderCategoryCodeById'). That cascade did not exist on the web at all.
 */
function fetchNextCategorySrNo() {
    var id = parseInt($('#cmbParentCategory').val() || '0');
    if (!id) { $('#txtCategoryNo').val(''); return; }
    $.ajax({
        url: '/api/purchase-order/next-category-sr-no?categoryId=' + id,
        type: 'GET',
        success: function (res) {
            if (res && res.categorySrNo) { $('#txtCategoryNo').val(res.categorySrNo); }
        }
    });
}

var comboDefaults = { jobLotId: 0, cropYearId: 0, cityId: 0 };

function loadDropdowns() {
    /* CropYear() :1584 - Sp_InvCropYear_GetAllMethod @Activity='ReadAll'. */
    $.get('/api/purchase-order/crop-years', function (data) {
        const sel = $('#cmbCropYear');
        sel.find('option:gt(0)').remove();
        (data || []).forEach(function (c) {
            sel.append(`<option value="${c.id}">${escapeHtml(c.cropYear)}</option>`);
        });
        applyComboDefaults();
    });

    // Parent Categories
    $.get('/api/purchase-order/parent-categories', function(data) {
        const sel = $('#cmbParentCategory');
        sel.find('option:gt(0)').remove();
        if (data) {
            data.forEach(c => sel.append(`<option value="${c.id}" data-code="${c.id}">${escapeHtml(c.description)}</option>`));
        }
        if ($.fn.select2) sel.trigger('change.select2');
        /* combordercat_Leave: generate the category serial whenever the category changes. */
        sel.off('change.catsr').on('change.catsr', fetchNextCategorySrNo);
    });

    // Payment Terms
    $.get('/api/purchase-order/payment-terms', function(data) {
        paymentTermsOptions = data || [];
        const sel = $('#cmbPaymentTerm');
        sel.find('option:gt(0)').remove();
        if (data) {
            data.forEach(t => sel.append(`<option value="${t.id}" data-days="${t.dueDays}" data-code="${t.dueDays ? t.dueDays + ' Days' : ''}">${escapeHtml(t.description)}</option>`));
        }
        if ($.fn.select2) sel.trigger('change.select2');
    });

    // Delivery Terms
    $.get('/api/purchase-order/delivery-terms', function(data) {
        const sel = $('#cmbDeliveryTerm');
        sel.find('option:gt(0)').remove();
        if (data) {
            data.forEach(t => sel.append(`<option value="${t.id}" data-code="${t.id}">${escapeHtml(t.description || t.name)}</option>`));
        }
        if ($.fn.select2) sel.trigger('change.select2');
    });

    // Job Lots
    $.get('/api/purchase-order/job-lots', function(data) {
        const sel = $('#cmbJobLot');
        sel.find('option:gt(0)').remove();
        if (data) {
            data.forEach(j => sel.append(`<option value="${j.id}" data-code="${j.id}">${escapeHtml(j.description)}</option>`));
        }
        if ($.fn.select2) sel.trigger('change.select2');
    });

    loadCommissionUoms();          /* CommissionUOMFill(), :1162 */
    loadEmptyBagDropdowns();
    loadSupplierExpenseDropdowns();
    loadChargeToProductDropdowns();
    loadPaymentTermsOptions();
    loadUomScheduleList();
    loadHistoryMeta();
    loadHistoryBranches();
    loadHistoryParties();
}

/* ============================================================================================
 * History tab - Supplier Name and Booking Person pickers.
 *
 * HistorySupplierComboFill (PurchsaeOrder.cs:4640-4695) fills BOTH from a single
 * USP_GetDataForDropDownFromPurchaseOrder call and splits the one result set on its Activity
 * column: "Supplier" rows go to one picker, "BookingPerson" rows to the other (:4676-4686).
 *
 * So these are NOT the party master - they are only the parties that actually appear on a
 * Purchase Order, which is why the desktop offers a handful of names. /api/purchase-order/
 * history-parties performs that one call and returns the two lists already split.
 *
 * The desktop also re-runs this whenever the History branch changes (the branch is a parameter
 * of the call), so the branch picker re-triggers it here too.
 * ============================================================================================ */
function loadHistoryParties() {
    /* The desktop passes the History tab's chosen branch; with none chosen the BLL omits the
       parameter, which the endpoint reproduces. No branch is not an error here. */
    const branchId = $('#cmbHistoryBranch').val() || '';
    const qs = (branchId && branchId !== '0') ? ('?branchesIds=' + encodeURIComponent(branchId)) : '';

    $.get('/api/purchase-order/history-parties' + qs, function (data) {
        fillHistoryPartySelect('#cmbHistorySupplier', (data && data.suppliers) || [],
                               '-- All Suppliers --');
        fillHistoryPartySelect('#cmbHistoryBookingPerson', (data && data.bookingPersons) || [],
                               '-- All Booking Persons --');
    }).fail(function () {
        /* Left empty rather than filled from the party master: showing every supplier here would
           be a different list from the desktop's, not a degraded version of it. */
        console.warn('history-parties failed; the History Supplier and Booking Person pickers '
                   + 'will stay empty rather than show the full party master.');
    });
}

function fillHistoryPartySelect(selector, rows, placeholder) {
    const sel = $(selector);
    if (!sel.length) return;
    const keep = sel.val();
    sel.empty().append(`<option value="0">${escapeHtml(placeholder)}</option>`);
    rows.forEach(r => {
        const id = r.Id !== undefined ? r.Id : r.id;
        const name = r.ReferenceName !== undefined ? r.ReferenceName : (r.name || r.description || '');
        if (id === undefined || id === null) return;
        sel.append(`<option value="${escapeHtml(id)}">${escapeHtml(name)}</option>`);
    });
    /* Keep the operator's selection across a refresh when it is still offered. */
    if (keep && sel.find(`option[value="${keep}"]`).length) sel.val(keep);
    /* Mirror into the hidden field the history query already reads (:2486) - both now and on
       every later pick. The template also carries an inline onchange doing this, but binding it
       here means the mirror survives a template edit and works if that attribute is ever
       dropped; the handler is namespaced so re-filling the list cannot stack duplicates. */
    if (selector === '#cmbHistorySupplier') {
        $('#hidHistorySupplierId').val(sel.val() || '0');
        sel.off('change.histmirror').on('change.histmirror', function () {
            $('#hidHistorySupplierId').val($(this).val() || '0');
        });
    }
}

function loadHistoryBranches() {
    /* The branch is a parameter of the history-party call, so changing it re-runs that call -
       HistorySupplierComboFill reads cmbBranchName every time (:4654-4667). */
    $('#cmbHistoryBranch').off('change.histparties').on('change.histparties', loadHistoryParties);
    $.get('/api/purchase-order/branches', function(data) {
        const sel = $('#cmbHistoryBranch');
        sel.find('option:gt(0)').remove();
        if (data) {
            data.forEach(b => {
                const id = b.id || b.Id || b.branchId || 0;
                const name = b.branchName || b.BranchName || b.description || b.Description || '';
                const code = b.branchCode || b.BranchCode || id;
                sel.append(`<option value="${id}" data-code="${escapeHtml(code)}">${escapeHtml(name)}</option>`);
            });
        }
        /* The desktop opens with a branch already chosen - cmbBranchName.Text is non-empty at
           load, which is why its history grid populates immediately. DDL.BindDDL's ZeroIndex
           argument decides exactly how, and DDL lives in a compiled library that is not in the
           recovered source, so its precise semantics are UNVERIFIED. The first option is
           selected here: for a user allocated a single branch - the case in the report - first,
           all and "index zero" are the same branch. Revisit if a multi-branch user sees the
           wrong default. */
        const firstReal = sel.find('option').filter(function () {
            return $(this).val() && $(this).val() !== '0';
        }).first();
        if (firstReal.length && !(sel.val() || []).length) sel.val([firstReal.val()]);

        if ($.fn.select2) sel.trigger('change.select2');
    });
}

/* ============================================================
 * PACK UOM / RATE UOM - real per-item UOM schedule, ditto desktop's
 * UOMFill() (fetch once for the whole Organization/Company) + bindRateUomAndItemPackUom()
 * (client-side filter by selected ItemId, applied to both the Pack UOM and Rate UOM combos).
 * ============================================================ */
function loadUomScheduleList() {
    $.get('/api/purchase-order/uom-schedules', function(data) {
        uomScheduleList = data || [];
    });
}

/**
 * Populates #cmbPackUom and #cmbRateUom with the real UOM rows for the given itemId, ditto
 * desktop's bindRateUomAndItemPackUom(). When isNewRow is true (adding a fresh line, not editing an
 * already-saved one) the Rate UOM is auto-defaulted to the schedule's BaseRateUom row, falling back
 * to whichever row has Equivalent==40 ("40Kg"), ditto desktop's own default-to-40kg behavior;
 * when false (editing an existing saved row) no auto-default is applied so the saved selection is
 * left for the caller to restore via .val(...).
 */
/* The desktop reports these through MessageBox.Show; this file already uses alert()
   for the same purpose, so keep one convention. */
function showPoMessage(text, isError) { alert(text); }

/* Local-calendar yyyy-MM-dd. toISOString() converts to UTC first, which east of
   Greenwich (this site runs at UTC+5) reports the PREVIOUS day for a local date. */
function ymdLocal(d) {
    if (!d || isNaN(d.getTime())) return '';
    const m = d.getMonth() + 1, day = d.getDate();
    return d.getFullYear() + '-' + (m < 10 ? '0' + m : m) + '-' + (day < 10 ? '0' + day : day);
}

function bindItemUom(itemId, isNewRow) {
    const packSel = $('#cmbPackUom');
    const rateSel = $('#cmbRateUom');
    packSel.empty();
    rateSel.empty();

    let rows = (uomScheduleList || []).filter(u => {
        const uItem = parseInt(u.itemId || u.ItemId || 0);
        return uItem === parseInt(itemId) || uItem === 0;
    });

    if (!rows || rows.length === 0) {
        rows = uomScheduleList || [];
    }

    /* No invented UOM schedule. The desktop binds combitempck/combrateuom strictly from
       the item's UOM schedule (CommonServices.GetUomScheduleByItemId); when that returns
       nothing there is no factor, and CalculateWeight() then zeroes Qty and Weight
       (:4262-4266) rather than assuming one. A hard-coded 40Kg/Kg/Bag/Ton list with
       invented equivalents used to stand here and would silently produce plausible but
       wrong weights and amounts. */
    if (!rows || rows.length === 0) {
        packSel.append('<option value="0">-- No UOM schedule for this item --</option>');
        rateSel.append('<option value="0">-- No UOM schedule for this item --</option>');
        if ($.fn.select2) { packSel.trigger('change.select2'); rateSel.trigger('change.select2'); }
        showPoMessage('This item has no UOM schedule, so Qty, Weight and Amount cannot be calculated.', true);
        return;
    }

    rows.forEach(u => {
        const id = u.id || u.Id || 1;
        /* The factor is the schedule's Equivalent column (combitempck.SelectedRow.Cells[2],
           :4259). A row without one gets NO data-eq, so the calculation can tell
           "no factor" apart from "a factor that happens to be 1". */
        const rawEq = (u.equivalent !== undefined && u.equivalent !== null && u.equivalent !== '')
            ? u.equivalent
            : ((u.Equivalent !== undefined && u.Equivalent !== null && u.Equivalent !== '') ? u.Equivalent : null);
        const eqNum = (rawEq === null) ? NaN : parseFloat(rawEq);
        const eqAttr = (isFinite(eqNum) && eqNum > 0) ? ` data-eq="${eqNum}"` : '';
        const code = u.uomCode || u.UOMCode || u.UomCode || '';
        /* data-base drives the BaseRateUom checkbox column - the second visible column of
           dtUOM once Equivalent is hidden (:1396, :1398). Read as stored; a row without the
           flag renders an unticked box rather than a guess. */
        const baseRaw = (u.baseRateUom !== undefined) ? u.baseRateUom
                      : ((u.BaseRateUom !== undefined) ? u.BaseRateUom : '');
        const baseAttr = ` data-base="${escapeHtml(String(baseRaw === true ? 1 : baseRaw === false ? 0 : baseRaw))}"`;
        packSel.append(`<option value="${id}"${eqAttr}${baseAttr}>${escapeHtml(code)}</option>`);
        rateSel.append(`<option value="${id}"${eqAttr}${baseAttr}>${escapeHtml(code)}</option>`);
    });

    if (isNewRow) {
        let baseRateRow = rows.find(u => u.baseRateUom === true || u.baseRateUom === 1 || String(u.uomCode).toLowerCase().includes('40kg'));
        if (!baseRateRow) {
            baseRateRow = rows.find(u => parseFloat(u.equivalent || 1.0) === 40.0) || rows[0];
        }
        if (baseRateRow) {
            rateSel.val(baseRateRow.id || baseRateRow.Id);
        }
        let basePackRow = rows.find(u => u.basePackUom === true || u.basePackUom === 1 || String(u.uomCode).toLowerCase().includes('bag'));
        if (!basePackRow) {
            basePackRow = rows[0];
        }
        if (basePackRow) {
            packSel.val(basePackRow.id || basePackRow.Id);
        }
    }

    if ($.fn.select2 && packSel.data('select2')) {
        packSel.trigger('change.select2');
        rateSel.trigger('change.select2');
    }
}

/* ============================================================
 * PACKING MATERIAL (EMPTY BAGS) - real dropdowns
 * ============================================================ */
function loadEmptyBagDropdowns() {
    // Type - real vEmptyBagTypes via SpStaticColumnNames (Id, type)
    $.get('/api/purchase-order/empty-bags/types', function(data) {
        emptyBagTypes = data || [];
        const sel = $('#cmbEbType');
        sel.find('option:gt(0)').remove();
        emptyBagTypes.forEach(t => sel.append(`<option value="${t.Id}">${escapeHtml(t.type)}</option>`));
        renderEbGrid();
    });

    // Item - real USP_PackingMaterialItemsAllocateToTransactionFlow_GetForCombo (ItemId, ItemName, ItemCode)
    $.get('/api/purchase-order/empty-bags/items', function(data) {
        emptyBagItemOptions = data || [];
        const sel = $('#cmbEbItem');
        sel.find('option:gt(0)').remove();
        emptyBagItemOptions.forEach(i => sel.append(`<option value="${i.ItemId}">${escapeHtml(i.ItemName)}</option>`));
        renderEbGrid();
    });

    // Packing Type - real Sp_InvPackingType_GetAllMethod ReadAll (Id, PackTypeDesc, MinEbWeight, MaxEbWeight, ...)
    $.get('/api/purchase-order/empty-bags/packing-types', function(data) {
        emptyBagPackingTypes = data || [];
        const sel = $('#cmbEbPackingType');
        sel.find('option:gt(0)').remove();
        emptyBagPackingTypes.forEach(p => sel.append(`<option value="${p.Id}">${escapeHtml(p.PackTypeDesc)}</option>`));
        renderEbGrid();
    });
}

function loadDefaultEmptyBagRows(callback) {
    $.get('/api/purchase-order/empty-bags/defaults', function(data) {
        emptyBagItems = data || [];
        renderEbGrid();
        if (typeof callback === 'function') callback();
    });
}

function preloadSearchData() {
    $.get('/api/purchase-order/suppliers?mode=name', function(data) {
        allSuppliers = data || [];
        populateSupplierDropdowns();
    });

    $.get('/api/purchase-order/brokers', function(data) {
        allBrokers = data || [];
        populateBrokerDropdown();
    });

    $.get('/api/purchase-order/commission-agents', function(data) {
        allCommAgents = data || [];
        populateCommissionAgentDropdown();
    });

    loadBookingPersons();

    $.get('/api/purchase-order/items?mode=name', function(data) {
        allItems = data || [];
        rebuildItemCategoryList();
        const ebSel = $('#cmbEbItem');
        ebSel.find('option:gt(0)').remove();
        allItems.forEach(i => ebSel.append(`<option value="${i.id}">${escapeHtml(i.itemName)}</option>`));
    });

    $.get('/api/purchase-order/cities', function(data) {
        allCities = data || [];
    });
}

function populateSupplierDropdowns() {
    const selForm = $('#cmbSupplier');
    selForm.find('option:gt(0)').remove();

    /* data-city-id carries the supplier's CityId so combsuppname_Leave's cascade
       (PurchsaeOrder.cs :1754-1757) can be reproduced without a second round trip.
       The history supplier is chosen through its own modal, not a <select>.

       The Name/Code radio changes only what is DISPLAYED: RdPartyByName_CheckedChanged
       (:1877-1884) re-binds the very same dtSupplier rows, swapping the display member
       between CompanyName and PartyCode. The row set never changes, so this is decided
       here rather than by re-querying. */
    const byCode = $('#radSupCode').is(':checked');
    const keepId = parseInt(selForm.val() || '0', 10);

    allSuppliers.forEach(s => {
        const code = s.partyCode || s.code || '';
        const name = s.companyName || s.name || '';
        const cityId = (s.cityId !== undefined && s.cityId !== null) ? s.cityId : '';
        const label = byCode ? (code || name) : (name || code);
        /* data-city / data-mobile feed the 4-column drop grid (Name | PartyCode | CityName |
           MobileNo), which is what dtSupplier shows once GlAccountId and CityId are hidden
           (PurchsaeOrder.cs:1031-1032). Absent values render as an empty cell, never a
           placeholder. data-city-id is separate and still drives the city cascade. */
        const cityName = s.cityName || s.city || '';
        const mobile = s.mobileNo || s.mobile || s.mobilePersonal || '';
        selForm.append(
            `<option value="${s.id}" data-code="${escapeHtml(code)}" data-city-id="${escapeHtml(String(cityId))}"`
            + ` data-city="${escapeHtml(cityName)}" data-mobile="${escapeHtml(mobile)}">${escapeHtml(label)}</option>`
        );
    });

    /* combsuppname.Value = Id after the re-bind (:1886-1889) */
    if (keepId > 0) selForm.val(String(keepId));
    if ($.fn.select2) selForm.trigger('change.select2');
}

function populateBrokerDropdown() {
    const sel = $('#cmbBrokerAccount');
    sel.find('option:gt(0)').remove();

    allBrokers.forEach(b => {
        const code = b.partyCode || b.glAccountId || '';
        const name = b.companyName || b.name || '';
        sel.append(`<option value="${b.id}" data-code="${escapeHtml(code)}">${escapeHtml(name)}</option>`);
    });

    if ($.fn.select2) {
        sel.trigger('change.select2');
    }
}

function populateCommissionAgentDropdown() {
    const sel = $('#cmbCommissionAgent');
    sel.find('option:gt(0)').remove();

    allCommAgents.forEach(a => {
        const name = a.companyName || a.name || '';
        const code = a.partyCode || a.glAccountId || '';
        const city = a.cityName || '';
        const mobile = a.mobileNo || '';
        sel.append(`<option value="${a.id}" data-code="${escapeHtml(code)}" data-city="${escapeHtml(city)}" data-mobile="${escapeHtml(mobile)}">${escapeHtml(name)}</option>`);
    });

    if ($.fn.select2) {
        sel.trigger('change.select2');
    }
}

/* ============================================================
 * 1. SUPPLIER AUTOCOMPLETE & SELECTION EVENT CHAIN
 * ============================================================ */
function onSupplierSearchModeChange() {
    const mode = $('#radSupCode').is(':checked') ? 'code' : 'name';
    $.get('/api/purchase-order/suppliers?mode=' + mode, function(data) {
        allSuppliers = data || [];
        populateSupplierDropdowns();
        renderSupplierModalGrid(allSuppliers);
    });
}

/* Which field the supplier search modal writes into: the form header, or the history
   filter. Any entry point other than openHistorySupplierModal() means the form. */
let supplierModalTarget = 'form';

function openSupplierSearchModal() {
    if (supplierModalTarget !== 'history') supplierModalTarget = 'form';
    const mode = $('#radSupCode').is(':checked') ? 'code' : 'name';
    $.get('/api/purchase-order/suppliers?mode=' + mode, function(data) {
        allSuppliers = data || [];
        populateSupplierDropdowns();
        renderSupplierModalGrid(allSuppliers);
        $('#modalSupplierSearch').modal('show');
        setTimeout(() => $('#txtModalSupQuery').focus(), 300);
    });
}

function filterSupplierSearchGrid() {
    const q = $('#txtModalSupQuery').val().toLowerCase();
    const filtered = allSuppliers.filter(s => 
        (s.companyName && s.companyName.toLowerCase().includes(q)) ||
        (s.partyCode && s.partyCode.toLowerCase().includes(q)) ||
        (s.cityName && s.cityName.toLowerCase().includes(q)) ||
        (s.mobileNo && s.mobileNo.toLowerCase().includes(q))
    );
    renderSupplierModalGrid(filtered);
}

function renderSupplierModalGrid(list) {
    const tbody = $('#tblSupplierModalTbody');
    tbody.empty();
    if (!list || list.length === 0) {
        tbody.html('<tr><td colspan="5" style="text-align:center; padding: 15px;">No matching suppliers found.</td></tr>');
        return;
    }
    list.forEach(s => {
        tbody.append(`
            <tr onclick="selectSupplier(${s.id})">
                <td><strong style="color: #004d40;">${escapeHtml(s.companyName)}</strong></td>
                <td>${escapeHtml(s.partyCode)}</td>
                <td>${escapeHtml(s.cityName)}</td>
                <td>${escapeHtml(s.mobileNo)}</td>
                <td style="text-align: center;"><button type="button" class="win-btn-action" style="padding: 1px 6px;">Select</button></td>
            </tr>
        `);
    });
}

function selectSupplier(suppId) {
    const supp = allSuppliers.find(s => s.id === suppId);
    if (!supp) return;

    /* The history branch of this modal is gone: that filter is now #cmbHistorySupplier, fed by
       /api/purchase-order/history-parties. Routing a pick from the party-master modal into it
       would put a supplier there that has no Purchase Order, which the desktop never offers. */
    if (supplierModalTarget === 'history') {
        supplierModalTarget = 'form';
        $('#modalSupplierSearch').modal('hide');
        return;
    }

    $('#hidSupplierId').val(supp.id);
    $('#cmbSupplier').val(supp.id).trigger('change.select2');
    const displayText = $('#radSupCode').is(':checked') 
        ? `${supp.partyCode} - ${supp.companyName}`
        : `${supp.companyName} (${supp.partyCode || 'N/A'})`;
    $('#txtSupplierDisplay').val(displayText);
    $('#modalSupplierSearch').modal('hide');

    // Execute Supplier Selection Event Chain
    onSupplierSelectedEventChain(supp);
}

/* combsuppname_Leave, PurchsaeOrder.cs :1750-1764.
 *
 * The desktop does exactly two things when a supplier is chosen: it copies the
 * supplier row's CityId into the Loading Location (City) control when that id is
 * greater than zero, and it re-binds the Factory Sample / Standard list.
 *
 * It does NOT default the Payment Term and it does NOT copy the supplier into the
 * Commission Agent. A previous build did both; that invented business logic silently
 * overwrote whatever the user had chosen and put the wrong party on the commission
 * line, so it is removed rather than kept alongside the desktop behaviour. */
function onSupplierSelectedEventChain(supp) {
    const cityId = parseInt(
        $('#cmbSupplier').find('option:selected').attr('data-city-id') ||
        (supp && supp.cityId) || '0', 10);

    if (cityId > 0) {
        if (typeof selectCity === 'function') {
            selectCity(cityId);                       /* keeps display + hidden id in step */
        } else {
            $('#hidLoadingCityId').val(cityId);
        }
    }
    /* FactorySampleOrStandardbind(FactorySampleOrStandardDbCall()), :1758 */
    if (typeof bindFactorySampleOrStandard === 'function') bindFactorySampleOrStandard();
}

/* ============================================================
 * Inline handlers the template calls by name. Each one delegates to the
 * already-bound programmatic logic so there is a single implementation, and each
 * cites the desktop handler it stands for.
 * ============================================================ */

/* The four combo onchange names the template USED to carry inline.
 *
 * They are deliberately empty. The handler for each combo is bound with jQuery in
 * bindDropdownChangeListeners() and a native change event already runs it; these called
 * .trigger('change.sync'), and jQuery's trigger re-invokes the element's own inline onchange
 * attribute - by BASE type, so the namespace did not protect it - which called back into here.
 * That was the "Maximum call stack size exceeded" on this page.
 *
 * purchase_order.html no longer sets onchange on these four selects. These stubs remain only so
 * that a browser holding an older cached copy of the template degrades to doing nothing rather
 * than throwing ReferenceError on every keystroke in the combo. Do not put work back in them:
 * anything added here runs in ADDITION to the jQuery handler, which is the double-execution
 * countx_desktop_combo.js:531 documents. */
function onSupplierChange()        { /* bound in bindDropdownChangeListeners - see above */ }
function onCommissionAgentChange() { /* bound in bindDropdownChangeListeners - see above */ }
function onBookingPersonChange()   { /* bound in bindDropdownChangeListeners - see above */ }
function onBrokerAccountChange()   { /* bound in bindDropdownChangeListeners - see above */ }

/* rdSearchByName_CheckedChanged, :1820-1846.
 *
 * The desktop re-binds combitem's display member between ItemName and ItemCode and
 * KEEPS the selected item (combitem.Value = ItemId). Here the item is picked through
 * a search modal rather than a combo, so the same rule applies to the two places the
 * mode is visible: the display text of the item already chosen, and the modal grid.
 * Like the desktop, this re-renders what is already loaded and does not re-query. */
function onItemSearchModeChange() {
    const byCode = $('#radItemCode').is(':checked');

    const keepId = parseInt($('#hidItemId').val() || '0', 10);
    if (keepId > 0 && allItems && allItems.length) {
        const item = allItems.find(i => i.id === keepId);
        if (item) {
            $('#txtItemDisplay').val(byCode
                ? `${item.itemCode} - ${item.itemName}`
                : `${item.itemName} (${item.itemCode || 'N/A'})`);
        }
    }
    if (allItems && allItems.length && $('#modalItemSearch').hasClass('in')) {
        renderItemModalGrid(itemsForPicker());
    }
}

/* FromDateHistory / ToDateHistory each carry their own enable checkbox, :4715-4756 */
function toggleHistoryDatePickers() {
    $('#txtHistoryFromDate').prop('disabled', !$('#chkHistoryEnableFromDate').is(':checked'));
    $('#txtHistoryToDate').prop('disabled', !$('#chkHistoryEnableToDate').is(':checked'));
}

/* btnNewHistory / the history Reset button: clears the filters back to their defaults.
   It clears filters only - it does not fetch, matching the desktop, where the grid is
   refreshed by the Show button. */
function resetHistoryFilters() {
    $('#chkHistoryEnableFromDate').prop('checked', true);
    $('#chkHistoryEnableToDate').prop('checked', true);
    $('#txtHistoryFromDate').val('');
    $('#txtHistoryToDate').val('');
    $('#txtHistoryFromDocNo').val('');
    $('#txtHistoryToDocNo').val('');
    $('#cmbHistoryBranch').val('0');
    $('#cmbHistoryBookingPerson').val('0');
    $('#cmbHistorySupplier').val('0');
    $('input[name="radHistoryDateType"][value="DocDate"]').prop('checked', true);
    clearHistorySupplier();
    toggleHistoryDatePickers();
    if ($.fn.select2) $('#cmbHistoryBranch, #cmbHistoryBookingPerson').trigger('change.select2');
}

/* cmbSupplierNameHistory, bound by HistorySupplierComboFill() :4640-4688, which binds the
   SAME supplier rows as the form combo. So the history picker reuses the one supplier
   search modal rather than duplicating it, with a target flag deciding where the pick
   lands. One modal, one data source, no second copy to drift. */
/* SUPERSEDED. The history Supplier filter is now #cmbHistorySupplier, a dropdown fed by
   /api/purchase-order/history-parties - which is what the desktop has (cmbSupplierNameHistory,
   PurchsaeOrder.cs:4688) and, more importantly, the right LIST: only parties that actually
   appear on a Purchase Order, not the party master this modal searches.

   Kept as a no-op because the template may still reference it from an older cached copy; it
   now does nothing rather than opening a modal that writes into a field that no longer exists. */
function openHistorySupplierModal() { /* replaced by #cmbHistorySupplier */ }

/* Clearing the history supplier means "all suppliers", which the desktop expresses as
   SupplierCustomerId = 0 (:4759). */
function clearHistorySupplier() {
    $('#hidHistorySupplierId').val('0');
    /* Now a dropdown; 0 is its "-- All Suppliers --" row. The old display textbox is gone. */
    const sel = $('#cmbHistorySupplier');
    if (sel.length) { sel.val('0'); sel[0].dispatchEvent(new Event('change', { bubbles: true })); }
}

/* ============================================================
 * BROKER AC, COMM AGENT & BOOKING PERSON SEARCH & SELECTION
 * ============================================================ */
function openBrokerSearchModal() {
    $.get('/api/purchase-order/brokers', function(data) {
        allBrokers = data || [];
        populateBrokerDropdown();
        renderBrokerModalGrid(allBrokers);
        $('#modalBrokerSearch').modal('show');
        setTimeout(() => $('#txtModalBrokerQuery').focus(), 300);
    });
}

function filterBrokerSearchGrid() {
    const q = $('#txtModalBrokerQuery').val().toLowerCase();
    const filtered = allBrokers.filter(b =>
        (b.companyName && b.companyName.toLowerCase().includes(q)) ||
        (b.glAccountId && String(b.glAccountId).includes(q)) ||
        (b.partyCode && b.partyCode.toLowerCase().includes(q)) ||
        (b.cityName && b.cityName.toLowerCase().includes(q)) ||
        (b.mobileNo && b.mobileNo.toLowerCase().includes(q))
    );
    renderBrokerModalGrid(filtered);
}

function renderBrokerModalGrid(list) {
    const tbody = $('#tblBrokerModalTbody');
    tbody.empty();
    if (!list || list.length === 0) {
        tbody.html('<tr><td colspan="7" style="text-align:center; padding: 15px;">No matching broker accounts found.</td></tr>');
        return;
    }
    list.forEach(b => {
        tbody.append(`
            <tr onclick="selectBroker(${b.id})">
                <td><strong style="color: #004d40;">${escapeHtml(b.companyName)}</strong></td>
                <td>${b.glAccountId || 0}</td>
                <td>${escapeHtml(b.partyCode || '')}</td>
                <td>${b.cityId || 0}</td>
                <td>${escapeHtml(b.cityName || '')}</td>
                <td>${escapeHtml(b.mobileNo || '')}</td>
                <td style="text-align: center;"><button type="button" class="win-btn-action" style="padding: 1px 6px;">Select</button></td>
            </tr>
        `);
    });
}

function selectBroker(brokerId) {
    const broker = allBrokers.find(b => b.id === brokerId);
    if (!broker) return;
    $('#hidBrokerAccountId').val(broker.id);
    $('#cmbBrokerAccount').val(broker.id).trigger('change.select2');
    $('#txtBrokerAcDisplay').val(broker.companyName);
    $('#modalBrokerSearch').modal('hide');
    calcBrokery();
}

function openCommAgentSearchModal() {
    $.get('/api/purchase-order/commission-agents', function(data) {
        allCommAgents = data || [];
        populateCommissionAgentDropdown();
        renderCommAgentModalGrid(allCommAgents);
        $('#modalCommAgentSearch').modal('show');
        setTimeout(() => $('#txtModalCommAgentQuery').focus(), 300);
    });
}

function filterCommAgentSearchGrid() {
    const q = $('#txtModalCommAgentQuery').val().toLowerCase();
    const filtered = allCommAgents.filter(a =>
        (a.companyName && a.companyName.toLowerCase().includes(q)) ||
        (a.glAccountId && String(a.glAccountId).includes(q)) ||
        (a.partyCode && a.partyCode.toLowerCase().includes(q)) ||
        (a.cityName && a.cityName.toLowerCase().includes(q)) ||
        (a.mobileNo && a.mobileNo.toLowerCase().includes(q))
    );
    renderCommAgentModalGrid(filtered);
}

function renderCommAgentModalGrid(list) {
    const tbody = $('#tblCommAgentModalTbody');
    tbody.empty();
    if (!list || list.length === 0) {
        tbody.html('<tr><td colspan="6" style="text-align:center; padding: 15px;">No matching commission agents found.</td></tr>');
        return;
    }
    list.forEach(a => {
        tbody.append(`
            <tr onclick="selectCommAgent(${a.id})">
                <td><strong style="color: #004d40;">${escapeHtml(a.companyName)}</strong></td>
                <td>${a.glAccountId || 0}</td>
                <td>${escapeHtml(a.partyCode || '')}</td>
                <td>${escapeHtml(a.cityName || '')}</td>
                <td>${escapeHtml(a.mobileNo || '')}</td>
                <td style="text-align: center;"><button type="button" class="win-btn-action" style="padding: 1px 6px;">Select</button></td>
            </tr>
        `);
    });
}

function selectCommAgent(agentId) {
    const agent = allCommAgents.find(a => a.id === agentId);
    if (!agent) return;
    $('#hidCommissionAgentId').val(agent.id);
    $('#cmbCommissionAgent').val(agent.id).trigger('change.select2');
    $('#txtCommAgentDisplay').val(agent.companyName);
    $('#modalCommAgentSearch').modal('hide');
    calcCommission();
}

function loadBookingPersons(callback) {
    $.get('/api/purchase-order/booking-persons', function(data) {
        allBookingPersons = data || [];

        const selForm = $('#cmbBookingPerson');
        const selHist = $('#cmbHistoryBookingPerson');
        selForm.find('option:gt(0)').remove();
        selHist.find('option:gt(0)').remove();

        /* The endpoint returns the procedure's own rows, so the keys are the SQL column
           spellings (ReferencePartyName / ReferencePartyType), not camelCase. Reading only the
           camelCase names left every option's text empty and every type literal. */
        allBookingPersons.forEach(p => {
            const name = p.ReferencePartyName || p.partyName || p.description || p.name || '';
            const type = p.ReferencePartyType || p.referencePartyType || p.partyTypeName || '';
            selForm.append(`<option value="${p.Id || p.id}" data-code="${escapeHtml(type)}">${escapeHtml(name)}</option>`);
            selHist.append(`<option value="${p.Id || p.id}" data-code="${escapeHtml(type)}">${escapeHtml(name)}</option>`);
        });

        if ($.fn.select2) {
            selForm.trigger('change.select2');
            selHist.trigger('change.select2');
        }

        if (typeof callback === 'function') callback();
    });
}

function openBookingPersonSearchModal() {
    loadBookingPersons(function() {
        renderBookingPersonModalGrid(allBookingPersons);
        $('#modalBookingPersonSearch').modal('show');
        setTimeout(() => $('#txtModalBookingPersonQuery').focus(), 300);
    });
}

function filterBookingPersonSearchGrid() {
    const q = $('#txtModalBookingPersonQuery').val().toLowerCase();
    const filtered = allBookingPersons.filter(p => {
        const name = p.ReferencePartyName || p.partyName || p.description || p.name || '';
        return name.toLowerCase().includes(q);
    });
    renderBookingPersonModalGrid(filtered);
}

function renderBookingPersonModalGrid(list) {
    const tbody = $('#tblBookingPersonModalTbody');
    tbody.empty();
    tbody.append(`
        <tr onclick="selectBookingPerson(0)">
            <td><em>-- Select --</em></td>
            <td><em>Booking Person</em></td>
            <td style="text-align: center;"><button type="button" class="win-btn-action" style="padding: 1px 6px;">Clear</button></td>
        </tr>
    `);
    if (!list || list.length === 0) {
        return;
    }
    list.forEach(p => {
        const pName = p.ReferencePartyName || p.partyName || p.description || p.name || '';
        const pType = p.ReferencePartyType || p.referencePartyType || p.partyTypeName || '';
        tbody.append(`
            <tr onclick="selectBookingPerson(${p.id})">
                <td><strong style="color: #004d40;">${escapeHtml(pName)}</strong></td>
                <td>${escapeHtml(pType)}</td>
                <td style="text-align: center;"><button type="button" class="win-btn-action" style="padding: 1px 6px;">Select</button></td>
            </tr>
        `);
    });
}

function selectBookingPerson(personId) {
    if (!personId || personId <= 0) {
        $('#hidBookingPersonId').val('0');
        $('#cmbBookingPerson').val('0').trigger('change.select2');
        $('#txtBookingPersonDisplay').val('-- Select --');
        $('#modalBookingPersonSearch').modal('hide');
        return;
    }
    const person = allBookingPersons.find(p => p.id === personId);
    if (!person) return;
    $('#hidBookingPersonId').val(person.id);
    $('#cmbBookingPerson').val(person.id).trigger('change.select2');
    $('#txtBookingPersonDisplay').val(person.partyName || person.description || person.name);
    $('#modalBookingPersonSearch').modal('hide');
}

/* ============================================================
 * 2. PAYMENT TERMS & DUE DATES DYNAMIC CALCULATIONS
 * ============================================================ */
function onContractDateChange() {
    calculatePaymentDueDate();
    calculateExpiryDate();
}

function onPaymentTermChange() {
    const val = $('#cmbPaymentTerm').val();
    const dueDaysInput = $('#txtDueDays');

    if (val === '1' || val === '3') { // Cash or Advance
        dueDaysInput.val('0').prop('disabled', true);
    } else {
        dueDaysInput.prop('disabled', false);
        if (parseFloat(dueDaysInput.val() || '0') === 0) {
            dueDaysInput.val('2');
        }
    }
    calculatePaymentDueDate();
}

function onDueDaysChange() {
    calculatePaymentDueDate();
}

function calculatePaymentDueDate() {
    const docDateStr = $('#txtDocDate').val();
    const dueDays = parseInt($('#txtDueDays').val() || '0');
    if (!docDateStr) return;

    const dt = new Date(docDateStr);
    dt.setDate(dt.getDate() + dueDays);
    $('#txtPaymentDueDate').val(ymdLocal(dt));
}

/* ============================================================
 * 3. DELIVERY START DATE & DAYS DYNAMIC CALCULATIONS
 * ============================================================ */
function onDeliveryStartDateChange() {
    calculateExpiryDate();
}

function onDeliveryDaysChange() {
    calculateExpiryDate();
}

function calculateExpiryDate() {
    const startStr = $('#txtDeliveryStartDate').val();
    const delDays = parseInt($('#txtDeliveryDays').val() || '0');
    if (!startStr) return;

    const dt = new Date(startStr);
    dt.setDate(dt.getDate() + delDays);
    $('#txtExpiryDate').val(ymdLocal(dt));
}

/* ============================================================
 * 4. COMMISSION & BROKERY CALCULATIONS
 * ============================================================ */
/* The desktop reads the rate UOM's divisor from the COMBO'S OWN TEXT -
   Conversion.ToDecimal(combruom.Text.Trim()) at :4364 and CmbBrokeryRateUom.Text at :4398 -
   because the "type" column of GetCommissionUom is the number itself.
   The old code did `val() === 1 ? 40 : 1000`, which mapped every option other than the
   first to 1000, so "100 KG" divided by 1000. Returns null when the text has no number,
   so a missing divisor is never silently treated as 1. */
function commissionUomDivisor(selectId) {
    const opt = $('#' + selectId + ' option:selected');
    if (!opt.length) return null;
    const m = /-?\d+(?:\.\d+)?/.exec(opt.text() || '');
    if (!m) return null;
    const n = parseFloat(m[0]);
    return (isFinite(n) && n > 0) ? n : null;
}

/* Desktop rounds commission and brokery to 4 decimals (:4367, :4394/:4400). */
function r4(n) { return Math.round((n + Number.EPSILON) * 10000) / 10000; }

/* TotalCommissionAmount(), PurchsaeOrder.cs :4343-4373 */
function calcCommission() {
    const type = $('#cmbCommType').val();
    const rate = parseFloat($('#txtCommRate').val() || '0');
    let commAmt = 0.0;

    if (type === 'Flat') {
        commAmt = rate;                                              /* :4356 */
    } else if (type === 'Percent') {
        commAmt = calculateGrandTotalLineAmount() * rate / 100.0;    /* :4360 */
    } else if (type === 'Weight' || type === 'Comm Weight') {
        const uom = commissionUomDivisor('cmbCommUom');              /* :4364 */
        if (uom === null) { $('#txtCommAmount').val('0'); return; }
        commAmt = calculateGrandTotalWeight() / uom * rate;
    }
    $('#txtCommAmount').val(r4(commAmt));
}

/* TotalBrokeryAmountCalculate(), PurchsaeOrder.cs :4376-4409 */
function calcBrokery() {
    const type = $('#cmbBrokeryType').val();
    const rate = parseFloat($('#txtBrokeryRate').val() || '0');
    let brokeryAmt = 0.0;

    if (type === 'Flat') {
        brokeryAmt = rate;                                           /* :4389 */
    } else if (type === 'Percent') {
        brokeryAmt = calculateGrandTotalLineAmount() * rate / 100.0; /* :4393 */
    } else if (type === 'Weight' || type === 'Comm Weight') {
        const uom = commissionUomDivisor('cmbBrokeryRateUom');       /* :4398 */
        if (uom === null) { $('#txtBrokeryAmount').val('0'); return; }
        brokeryAmt = calculateGrandTotalWeight() / uom * rate;
    }
    $('#txtBrokeryAmount').val(r4(brokeryAmt));
}

/* CommissionUOMFill() binds BOTH combos from StaticColumnNames "GetCommissionUom" (:1162-1172)
   and activates row 1 on each. No hard-coded list. */
function loadCommissionUoms() {
    $.get('/api/purchase-order/commission-uoms', function (data) {
        const rows = data || [];
        const targets = ['cmbCommUom', 'cmbBrokeryRateUom'];
        targets.forEach(function (id) {
            const sel = $('#' + id);
            sel.empty();
            if (!rows.length) {
                sel.append('<option value="0">-- No commission UOM configured --</option>');
                return;
            }
            rows.forEach(function (r) {
                const val = r.Id !== undefined ? r.Id : (r.id !== undefined ? r.id : 0);
                const txt = (r.type !== undefined && r.type !== null) ? r.type : (r.Type || '');
                sel.append(`<option value="${val}">${escapeHtml(String(txt))}</option>`);
            });
            if ($.fn.select2) sel.trigger('change.select2');
        });
        calcCommission();
        calcBrokery();
    });
}

function calculateGrandTotalLineAmount() {
    return lineItems.reduce((acc, item) => acc + item.itemAmount, 0.0);
}

function calculateGrandTotalWeight() {
    return lineItems.reduce((acc, item) => acc + item.itemWeight, 0.0);
}

/* ============================================================
 * 5. ITEM AUTOCOMPLETE & SELECTION EVENT CHAIN
 * ============================================================ */
/*
 * combordercat_Leave - PurchsaeOrder.cs:1676-1683.
 *
 * The header "Category" combo holds an InvOrderCategory id (1 General, 4 Paddy, 5 Rice,
 * 6 By Product, 8 Govt Purchase). The ITEM list is filtered on InventoryParentCategoriesId,
 * which is a different key, and the desktop maps between them with an explicit switch:
 *
 *     int CategoryId = val switch { 6 => 4, 1 => 3, 5 => 2, 4 => 1, _ => 0 };
 *     ItemCategoryOrTypeBind(CategoryId);
 *     ItemdtFillFromAll(CategoryId, ...);
 *
 * This page was sending the raw order-category id straight through as parentCategoryId, so
 * General asked for parent 1 instead of 3, Paddy for 4 instead of 1, Rice for 5 instead of 2.
 * Those are real parent categories, so the item list came back populated - with the wrong items.
 * Govt Purchase (8) falls to 0, which means "no filter", exactly as the desktop's default arm.
 *
 * The switch is the desktop's own; it is reproduced, not inferred from the names.
 */
/*
 * defaultConfiquration() :1617 - Job/Lot, Default Crop Year and City Area are configuration ids
 * the desktop assigns to combjob.Value, CmbCropyr.Value and combcityarea.Value on a new order.
 * Each is applied only when it is > 0 and the list actually offers it, which is the desktop's own
 * guard; nothing falls back to "the first row".
 */
function fetchComboDefaults() {
    $.get('/api/purchase-order/combo-defaults', function (d) {
        comboDefaults = {
            jobLotId:   parseInt((d && d.jobLotId)   || 0),
            cropYearId: parseInt((d && d.cropYearId) || 0),
            cityId:     parseInt((d && d.cityId)     || 0)
        };
        applyComboDefaults();
    });
}

function applyComboDefaults() {
    if (comboDefaults.cropYearId > 0) {
        const cy = $('#cmbCropYear');
        if (cy.find(`option[value="${comboDefaults.cropYearId}"]`).length) {
            cy.val(String(comboDefaults.cropYearId)).trigger('change');
        }
    }
    if (comboDefaults.jobLotId > 0) {
        const jl = $('#cmbJobLot');
        if (jl.find(`option[value="${comboDefaults.jobLotId}"]`).length) {
            jl.val(String(comboDefaults.jobLotId)).trigger('change');
        }
    }
}

function orderCategoryToParentCategory(orderCategoryId) {
    switch (parseInt(orderCategoryId || '0')) {
        case 6:  return 4;
        case 1:  return 3;
        case 5:  return 2;
        case 4:  return 1;
        default: return 0;
    }
}

function onParentCategoryChange() {
    const parentId = orderCategoryToParentCategory($('#cmbParentCategory').val());
    $.get('/api/purchase-order/items?mode=name&parentCategoryId=' + parentId, function(data) {
        allItems = data || [];
        /* combordercat_Leave (:1663-1686) runs ItemCategoryOrTypeBind THEN ItemdtFillFromAll THEN
           ItemNameBind - the category list is rebuilt from the newly narrowed items every time the
           header category changes, not just once at load. */
        rebuildItemCategoryList();
        onItemCategoryChange();
    });
}

/* Item Code / Item Name for a grid row, taken from the item itself.

   Order of preference, and why:
     1. the loaded item list - the direct equivalent of the desktop reading
        combitem.SelectedRow.Cells["ItemCode"] / ["ItemName"] (:2635);
     2. the row currently being edited - so re-saving an existing line whose item has since been
        filtered out of the list does not blank or corrupt its code;
     3. the display text, unchanged, as a last resort. It is NOT split here: in Name mode the
        display is "Name (CODE)" and splitting it is exactly the bug this replaces. */
function selectedItemField(itemId, field, displayText) {
    if (itemId > 0 && allItems && allItems.length) {
        const it = allItems.find(i => i.id === itemId);
        if (it && it[field] !== undefined && it[field] !== null && it[field] !== '') return it[field];
    }
    if (editingLineIdx >= 0 && lineItems[editingLineIdx] && lineItems[editingLineIdx][field]) {
        return lineItems[editingLineIdx][field];
    }
    return displayText || '';
}

/* ------------------------------------------------------------------------------------------
 * Item Category / Item Type filter - CmbCategory plus the RadCategory / RadType pair.
 *
 * ItemCategoryOrTypeBind, PurchsaeOrder.cs:1230-1283.
 *   - the list is DERIVED from the items already loaded, not fetched from a master;
 *   - it is distinct on the NAME, skipping blanks, and carries that row's own id;
 *   - the caption is "Item Category" when Category is checked and "ItemType" when Type is
 *     (:1271), which is why the label and the combo caption are both swapped below;
 *   - when the derived list is empty the desktop clears the combo rather than leaving stale
 *     values standing (:1277-1279).
 *
 * ItemdtFillFromAll, :1289-1310, then narrows the item list itself:
 *     CategoryOrTypeId == 0                       -> everything
 *     RadCategory.Checked && == ItemCategoryId    -> that category
 *     RadType.Checked     && == ItemTypeId        -> that type
 * and ItemNameBind (:1313) re-binds the item picker afterwards.
 *
 * The parent-category narrowing is already done server-side by the items endpoint, so what is
 * left here is exactly the second stage.
 * ---------------------------------------------------------------------------------------- */
function itemCatModeIsCategory() { return $('#radItemCatCategory').is(':checked'); }

function rebuildItemCategoryList() {
    const byCategory = itemCatModeIsCategory();
    const idKey   = byCategory ? 'itemCategoryId' : 'itemTypeId';
    const nameKey = byCategory ? 'itemCategory'   : 'itemType';
    const caption = byCategory ? 'Item Category'  : 'ItemType';

    $('#lblItemCategory').text(caption);
    const sel = $('#cmbItemCategory');
    sel.attr('data-dtcombo-caption', caption);

    const keep = parseInt(sel.val() || '0', 10);
    const seen = new Map();
    (allItems || []).forEach(function (i) {
        const name = (i[nameKey] || '').toString().trim();
        if (!name) return;                       /* !string.IsNullOrEmpty(row.ItemCategory) */
        if (!seen.has(name)) seen.set(name, parseInt(i[idKey] || '0', 10));
    });

    sel.empty().append('<option value="0">-- All --</option>');
    Array.from(seen.keys()).sort(function (a, b) { return a.localeCompare(b); })
        .forEach(function (name) {
            sel.append(`<option value="${seen.get(name)}">${escapeHtml(name)}</option>`);
        });

    /* Retain the selection when it is still offered - BindAndRetainSelection's whole purpose. */
    if (keep > 0 && sel.find(`option[value="${keep}"]`).length) sel.val(String(keep));
    else sel.val('0');
}

/* The items the picker should show: everything the endpoint returned, narrowed by the
   category/type selection. Kept separate from allItems so switching the filter back to
   "-- All --" restores the full list without re-querying, ditto the desktop. */
function itemsForPicker() {
    const catOrTypeId = parseInt($('#cmbItemCategory').val() || '0', 10);
    if (!catOrTypeId) return allItems || [];
    const key = itemCatModeIsCategory() ? 'itemCategoryId' : 'itemTypeId';
    return (allItems || []).filter(function (i) { return parseInt(i[key] || '0', 10) === catOrTypeId; });
}

function onItemCategoryModeChange() {
    /* Switching Category <-> Type rebuilds the list from the SAME loaded items and drops the old
       selection, because an ItemCategoryId and an ItemTypeId are not comparable. */
    $('#cmbItemCategory').val('0');
    rebuildItemCategoryList();
    onItemCategoryChange();
}

function onItemCategoryChange() {
    /* Narrowing the item list can orphan the item already chosen. The desktop's
       CommonServices.SetComboValue(combitem, "Id", ID, dtitem) restores the selection only when
       the id is still present, so a no-longer-offered item is cleared here rather than left
       displayed over a list it is not in. */
    const chosen = parseInt($('#hidItemId').val() || '0', 10);
    if (chosen > 0 && !itemsForPicker().some(function (i) { return i.id === chosen; })) {
        $('#hidItemId').val('0');
        $('#txtItemDisplay').val('');
    }
    if ($('#modalItemSearch').hasClass('in')) renderItemModalGrid(itemsForPicker());
}

function openItemSearchModal() {
    const mode = $('#radItemCode').is(':checked') ? 'code' : 'name';
    /* Same mapping as onParentCategoryChange: the combo holds an InvOrderCategory id, the item
       filter wants an InventoryParentCategoriesId. On the desktop this modal's list is the
       already-filtered dtitem (rdSearchByName_CheckedChanged:1820 rebinds it rather than
       re-querying), so it must be narrowed by the same key the header last applied. */
    const parentId = orderCategoryToParentCategory($('#cmbParentCategory').val());
    $.get('/api/purchase-order/items?mode=' + mode + '&parentCategoryId=' + parentId, function(data) {
        allItems = data || [];
        rebuildItemCategoryList();
        renderItemModalGrid(itemsForPicker());
        $('#modalItemSearch').modal('show');
        setTimeout(() => $('#txtModalItemQuery').focus(), 300);
    });
}

function filterItemSearchGrid() {
    const q = $('#txtModalItemQuery').val().toLowerCase();
    /* Text search runs over the CATEGORY-FILTERED list, not the whole one - otherwise typing
       could surface an item the chosen Item Category has excluded. */
    const filtered = itemsForPicker().filter(i => 
        (i.itemName && i.itemName.toLowerCase().includes(q)) ||
        (i.itemCode && i.itemCode.toLowerCase().includes(q)) ||
        (i.itemCategory && i.itemCategory.toLowerCase().includes(q))
    );
    renderItemModalGrid(filtered);
}

function renderItemModalGrid(list) {
    const tbody = $('#tblItemModalTbody');
    tbody.empty();
    if (!list || list.length === 0) {
        tbody.html('<tr><td colspan="5" style="text-align:center; padding: 15px;">No matching items found.</td></tr>');
        return;
    }
    list.forEach(i => {
        tbody.append(`
            <tr onclick="selectItem(${i.id})">
                <td><strong style="color: #008080;">${escapeHtml(i.itemCode)}</strong></td>
                <td>${escapeHtml(i.itemName)}</td>
                <td>${escapeHtml(i.itemCategory)}</td>
                <td style="text-align: right;">${parseFloat(i.purchasePrice || 0).toFixed(2)}</td>
                <td style="text-align: center;"><button type="button" class="win-btn-action" style="padding: 1px 6px;">Select</button></td>
            </tr>
        `);
    });
}

function selectItem(itemId) {
    const item = allItems.find(i => i.id === itemId);
    if (!item) return;

    $('#hidItemId').val(item.id);
    const displayText = $('#radItemCode').is(':checked')
        ? `${item.itemCode} - ${item.itemName}`
        : `${item.itemName} (${item.itemCode || 'N/A'})`;
    $('#txtItemDisplay').val(displayText);
    $('#modalItemSearch').modal('hide');

    // Real per-item UOM schedule, ditto desktop's bindRateUomAndItemPackUom() fired on the Item
    // combo's Leave/selection-committed event - populates both Pack UOM and Rate UOM from the same
    // database-backed UOMSchedule rows for this item (isNewRow=true only when not mid-edit of an
    // already-saved row, so editing an existing line never clobbers its saved UOM selection).
    bindItemUom(item.id, editingLineIdx < 0);

    if (item.purchasePrice && parseFloat(item.purchasePrice) > 0) {
        $('#txtRate').val(parseFloat(item.purchasePrice).toFixed(2));
    }
    calcLineWeight();
    calcLineAmount();
}

/* ============================================================
 * 6. ITEM WEIGHT & AMOUNT CALCULATIONS
 * ============================================================ */
/* Returns the selected UOM's Equivalent, or null when there is none.
   Never substitutes 1 or 0 - CalculateWeight() :4259 reads Cells[2] only when a row is
   active and the value is > 0, and treats anything else as "no factor". */
function uomFactor(selectorId) {
    const opt = $('#' + selectorId + ' option:selected');
    if (!opt.length || parseInt(opt.val() || '0', 10) <= 0) return null;
    const raw = opt.attr('data-eq');
    if (raw === undefined || raw === null || raw === '') return null;
    const n = parseFloat(raw);
    return (isFinite(n) && n > 0) ? n : null;
}

/* Desktop rounds these to 3 decimals and displays #,##0.### (:4270, :4278). */
function r3(n) { return Math.round((n + Number.EPSILON) * 1000) / 1000; }

/* CalculateWeight(), PurchsaeOrder.cs :4253-4299.
 *
 * Qty and Weight are two views of the same line, linked by the Pack UOM's Equivalent:
 * editing Qty computes Weight (qty x Uom), editing Weight computes Qty (weight / Uom).
 * When there is no usable factor the desktop ZEROES BOTH fields rather than assuming
 * one - that is the whole point of the Uom <= 0 branch at :4262.
 *
 * `source` is 'qty' or 'weight', standing in for the desktop's ActiveControl test. */
function calcLineWeight(source) {
    const uom = uomFactor('cmbPackUom');

    if (uom === null) {
        /* :4262-4266 - no factor, so neither figure can be trusted */
        $('#txtQty').val('0');
        $('#txtWeight').val('0');
        calcLineAmount();
        return;
    }

    if (source === 'weight') {
        const weight = parseFloat($('#txtWeight').val() || '0');
        $('#txtQty').val(weight > 0 ? r3(weight / uom) : 0);      /* :4277 */
    } else {
        const qty = parseFloat($('#txtQty').val() || '0');
        $('#txtWeight').val(qty > 0 ? r3(qty * uom) : 0);         /* :4269 */
    }
    calcLineAmount();
}

/* Amount = Weight / RateUom.Equivalent x Rate.
   combrateuom.SelectedRow.Cells[2] is that Equivalent (:2620 compares it to 40.0).
   With no factor there is no amount - it is left at 0 and the user is not shown a
   number that was computed from an assumption. */
function calcLineAmount() {
    const weight = parseFloat($('#txtWeight').val() || '0');
    const rate = parseFloat($('#txtRate').val() || '0');
    const rateEq = uomFactor('cmbRateUom');

    if (rateEq === null || !(weight > 0)) { $('#txtAmount').val('0'); return; }
    $('#txtAmount').val(r3(weight / rateEq * rate));
}

/* ============================================================
 * 7. DETAIL GRID & ADD/EDIT/DELETE
 * ============================================================ */
function btnAddDetailRow_Click() {
    const itemId = parseInt($('#hidItemId').val() || '0');
    const itemDisplayText = $('#txtItemDisplay').val();
    const qty = parseFloat($('#txtQty').val() || '0');
    const weight = parseFloat($('#txtWeight').val() || '0');
    const rate = parseFloat($('#txtRate').val() || '0');
    const amount = parseFloat($('#txtAmount').val() || '0');
    /* The desktop saves the combo's TEXT into Crop and its id into CropYearId (:3289). */
    const cropYearId = parseInt($('#cmbCropYear').val() || '0');
    const cropYear = cropYearId > 0 ? $('#cmbCropYear option:selected').text() : '';
    /* No '|| 1' fallback: FormDetailValidation treats 0 as "not chosen" and refuses the row.
       Defaulting to 1 made the UOM checks below unreachable and could post a wrong factor. */
    const packUomId = parseInt($('#cmbPackUom').val() || '0');
    const packUomCode = $('#cmbPackUom option:selected').text();
    const rateUomId = parseInt($('#cmbRateUom').val() || '0');
    const rateUomCode = $('#cmbRateUom option:selected').text();
    const jobLotId = parseInt($('#cmbJobLot').val() || '0');
    const jobLotName = jobLotId > 0 ? $('#cmbJobLot option:selected').text() : '';
    const loadingCityId = parseInt($('#hidLoadingCityId').val() || '0');
    const loadingCityName = $('#txtLoadingCityDisplay').val() || '';
    const moisture = parseFloat($('#txtMoisture').val() || '0');
    const remarks = $('#txtLineRemarks').val() || '';

    /* ------------------------------------------------------------------------------------
     * FormDetailValidation() - PurchsaeOrder.cs:1999-2050.
     *
     * The desktop runs EIGHT checks, in this order, before btnplus_Click will add a row, and
     * each one shows its own message and focuses its own control. The web had only three, worded
     * differently, and was missing Crop Year, Job/Lot, Item Rate and Amount entirely - which is
     * why a line could be added with Item Rate 0.00 and Amount 0.00, a row the desktop refuses.
     *
     * Messages are the desktop's own strings, not paraphrases, so the two apps say the same thing.
     * Loading Location, Moisture and Remarks are deliberately NOT validated - the desktop does
     * not validate them either.
     * ------------------------------------------------------------------------------------ */
    if (itemId <= 0) {
        alert("Item Field is Required");
        $('#txtItemDisplay').focus();
        return;
    }
    if (cropYearId <= 0) {
        alert("CropYear Field is Required");
        $('#cmbCropYear').focus();
        return;
    }
    if (jobLotId <= 0) {
        alert("JobLot Field is Required");
        $('#cmbJobLot').focus();
        return;
    }
    if (packUomId <= 0) {
        alert("UOM Field is Required");
        $('#cmbPackUom').focus();
        return;
    }
    if (!(qty > 0)) {
        alert("Item Qty Field is Required");
        $('#txtQty').focus();
        return;
    }
    if (!(rate > 0)) {
        alert("Item Rate Field is Required");
        $('#txtRate').focus();
        return;
    }
    if (rateUomId <= 0) {
        alert("Rate UOM Field is Required");
        $('#cmbRateUom').focus();
        return;
    }
    if (!(amount > 0)) {
        alert("Amount Field is Required");
        $('#txtAmount').focus();
        return;
    }

    // Preserve the real, already-saved PurchaseOrderDetail.Id (round-tripped from
    // getPurchaseOrderById()) when this Add/Update is actually editing an existing row - ditto
    // desktop's btnsave_Click() reading the grid's own "Id" cell. A brand-new row keeps this at 0 so
    // the backend inserts it instead of updating a row that doesn't belong to it.
    const existingDetailId = (editingLineIdx >= 0 && lineItems[editingLineIdx]) ? (lineItems[editingLineIdx].purchaseOrderDetailId || 0) : 0;

    const line = {
        purchaseOrderDetailId: existingDetailId,
        itemId: itemId,
        /* The desktop reads these as two separate cells of the selected combo row -
           combitem.SelectedRow.Cells["ItemCode"] and Cells["ItemName"], :2635 - so they are taken
           from the item here too.

           They used to be RE-PARSED out of the display textbox, which has two different shapes:
               Code mode: "CODE - Name"      split(' - ') worked
               Name mode: "Name (CODE)"      no " - " at all
           so in Name mode - the default - split(' - ')[0] returned the WHOLE string and both grid
           columns showed the same text: "B1 1509 White Process (732)" under Item Code as well as
           Item Name. Even in Code mode an item name containing " - " split in the wrong place.

           Falling back to the row being edited (not to the parse) keeps an existing line intact if
           its item is no longer in the loaded list. */
        itemCode: selectedItemField(itemId, 'itemCode', itemDisplayText),
        itemName: selectedItemField(itemId, 'itemName', itemDisplayText),
        cropYear: cropYear,
        cropYearId: cropYearId,
        packUomId: packUomId,
        packUomCode: packUomCode,
        itemQty: qty,
        itemWeight: weight,
        itemRate: rate,
        rateUomId: rateUomId,
        rateUomCode: rateUomCode,
        itemAmount: amount,
        jobLotId: jobLotId,
        jobLotName: jobLotName,
        loadingLocationCityId: loadingCityId,
        loadingLocationCityName: loadingCityName,
        moisturePercent: moisture,
        remarks: remarks
    };

    // Duplicate-item guard, ditto desktop's btnplus_Click() ("Duplicate Item Not Add in Grid") -
    // only enforced when adding a brand-new row, not when editing the row that already holds this item.
    const dup = lineItems.some((li, i) => i !== editingLineIdx && parseInt(li.itemId) === itemId);
    if (dup) {
        alert("Duplicate Item Not Add in Grid");
        return;
    }

    /* btnplus_Click:2625 - when the chosen Rate UOM's Equivalent is not 40, the desktop asks
       before adding. The factor is the schedule's Equivalent (combrateuom.SelectedRow.Cells[2]),
       which is what data-eq carries. A row with no factor at all is not second-guessed here: the
       desktop's test reads the cell and a missing one is not 40 either, so it asks too. */
    const rateEqForConfirm = uomFactor('cmbRateUom');
    if (rateEqForConfirm !== 40 &&
        !confirm("Are you sure to add record because your RateUom not 40Kg?")) {
        $('#cmbRateUom').focus();
        return;
    }

    if (editingLineIdx >= 0) {
        lineItems[editingLineIdx] = line;
        editingLineIdx = -1;
    } else {
        lineItems.push(line);
    }

    renderDetailGrid();
    clearItemInputs();
    calcCommission();
    calcBrokery();
}

function clearItemInputs() {
    // Ditto desktop's ResetDetail(): Item / Pack UOM / Qty / Weight / Rate UOM / Rate / Amount /
    // Remarks are cleared after each Add; Job/Lot, Crop Year and Loading City are deliberately left
    // as-is since those tend to repeat across consecutive line items on the same Purchase Order.
    $('#hidItemId').val('0');
    $('#txtItemDisplay').val('');
    $('#txtQty').val('');
    $('#txtWeight').val('');
    $('#txtRate').val('');
    $('#txtAmount').val('');
    $('#txtLineRemarks').val('');
    $('#cmbCropYear').val(String(comboDefaults.cropYearId || 0));
    $('#cmbPackUom').empty().append('<option value="0">-- Select Item First --</option>');
    $('#cmbRateUom').empty().append('<option value="0">-- Select Item First --</option>');
}

function editDetailRow(idx) {
    const item = lineItems[idx];
    if (!item) return;

    editingLineIdx = idx;
    $('#hidItemId').val(item.itemId);
    $('#txtItemDisplay').val(`${item.itemCode} - ${item.itemName}`);
    $('#cmbCropYear').val(item.cropYearId || '0');
    if (parseInt(item.cropYearId || '0') <= 0 && item.cropYear) {
        /* An older row saved before the combo existed carries the text but no id. */
        $('#cmbCropYear option').filter(function () { return $(this).text() === item.cropYear; }).prop('selected', true);
    }

    // Re-populate the real, item-specific UOM list before restoring the saved selection - ditto
    // desktop's bindRateUomAndItemPackUom(detailId) called with a non-zero detailId when double-
    // clicking an existing grid row to edit it, which skips the auto-default-to-40kg logic so the
    // row's already-saved Rate UOM is preserved instead of being overwritten.
    bindItemUom(item.itemId, false);
    $('#cmbPackUom').val(item.packUomId);
    $('#cmbRateUom').val(item.rateUomId);

    $('#txtQty').val(item.itemQty);
    $('#txtWeight').val(item.itemWeight);
    $('#txtRate').val(item.itemRate);
    $('#txtAmount').val(item.itemAmount);
    $('#cmbJobLot').val(item.jobLotId || 0);
    $('#hidLoadingCityId').val(item.loadingLocationCityId || 0);
    $('#txtLoadingCityDisplay').val(item.loadingLocationCityName || '');
    $('#txtMoisture').val(item.moisturePercent || 14.0);
    $('#txtLineRemarks').val(item.remarks || '');
}

function removeDetailRow(idx) {
    lineItems.splice(idx, 1);
    renderDetailGrid();
    calcCommission();
    calcBrokery();
}

function renderDetailGrid() {
    const tbody = $('#tblItemsTbody');
    tbody.empty();

    if (lineItems.length === 0) {
        tbody.html('<tr><td colspan="14" style="text-align: center; padding: 15px; color: #777;">No transaction detail items added yet.</td></tr>');
        updateDetailTotals(0, 0, 0);
        return;
    }

    let totQty = 0, totWt = 0, totAmt = 0;

    lineItems.forEach((item, idx) => {
        totQty += item.itemQty;
        totWt += item.itemWeight;
        totAmt += item.itemAmount;

        tbody.append(`
            <tr>
                <td style="text-align: center; font-weight: bold;">${idx + 1}</td>
                <td><strong style="color: #008080;">${escapeHtml(item.itemCode)}</strong></td>
                <td>${escapeHtml(item.itemName)}</td>
                <td>${escapeHtml(item.cropYear)}</td>
                <td style="text-align: right;">${item.itemQty.toFixed(2)}</td>
                <td style="text-align: right;">${item.itemWeight.toFixed(2)}</td>
                <td style="text-align: right;">${item.itemRate.toFixed(2)}</td>
                <td>${escapeHtml(item.rateUomCode)}</td>
                <td style="text-align: right; font-weight: bold; color: #004d40;">${item.itemAmount.toFixed(2)}</td>
                <td>${escapeHtml(item.jobLotName)}</td>
                <td>${escapeHtml(item.loadingLocationCityName)}</td>
                <td style="text-align: right;">${item.moisturePercent}%</td>
                <td>${escapeHtml(item.remarks)}</td>
                <td style="text-align: center;">
                    <button type="button" class="btn btn-warning btn-xs" onclick="editDetailRow(${idx})" style="padding: 0 4px; font-size: 10px;"><i class="fa fa-edit"></i></button>
                    <button type="button" class="btn btn-danger btn-xs" onclick="removeDetailRow(${idx})" style="padding: 0 4px; font-size: 10px;"><i class="fa fa-times"></i></button>
                </td>
            </tr>
        `);
    });

    updateDetailTotals(totQty, totWt, totAmt);
    renderSchedGrid();
}

function updateDetailTotals(qty, wt, amt) {
    $('#lblTotalQty').text(qty.toFixed(2));
    $('#lblTotalWeight').text(wt.toFixed(2) + " KG");
    $('#lblGrandTotal').text(amt.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
}

/* ============================================================
 * 8. CITY SEARCH & DEFINE CITY
 * ============================================================ */
/**
 * Real, DB-backed City search, ditto desktop's cmbCityFill() -> Architecture.BLL.City.GetAll() ->
 * SP_City_GetAllMethod (org/company scoped). Renders a searchable modal grid (consistent with the
 * Supplier/Item search modals) instead of a native browser prompt() - a prompt() cannot show more
 * than a flat comma-joined string and blocks the page while open, which is what made the previous
 * implementation feel "not working" even though the underlying /api/purchase-order/cities endpoint
 * was already real and DB-backed.
 */
function openCitySearchModal() {
    $.get('/api/purchase-order/cities', function(data) {
        allCities = (data || []).map(c => ({ id: c.id, name: (c.cityName || '').trim() }))
            .filter(c => c.name.length > 0);
        renderCityModalGrid(allCities);
        $('#modalCitySearch').modal('show');
        setTimeout(() => $('#txtModalCityQuery').focus(), 300);
    });
}

function filterCitySearchGrid() {
    const q = $('#txtModalCityQuery').val().toLowerCase();
    const filtered = allCities.filter(c => c.name.toLowerCase().includes(q));
    renderCityModalGrid(filtered);
}

function renderCityModalGrid(list) {
    const tbody = $('#tblCityModalTbody');
    tbody.empty();
    if (!list || list.length === 0) {
        tbody.html('<tr><td colspan="2" style="text-align:center; padding: 15px;">No matching cities found. Use "Define City" above to add one.</td></tr>');
        return;
    }
    list.forEach(c => {
        tbody.append(`
            <tr onclick="selectCity(${c.id})">
                <td>${escapeHtml(c.name)}</td>
                <td style="text-align: center;"><button type="button" class="win-btn-action" style="padding: 1px 6px;">Select</button></td>
            </tr>
        `);
    });
}

function selectCity(cityId) {
    const city = allCities.find(c => c.id === cityId);
    if (!city) return;
    $('#hidLoadingCityId').val(city.id);
    $('#txtLoadingCityDisplay').val(city.name);
    $('#modalCitySearch').modal('hide');
}

/** Real "Define City" popup, ditto desktop's DefineCity.cs form (opened from the same toolbar
 *  action). Kept as its own small modal (city name only) rather than replicating the desktop's full
 *  Tehsil-assignment grid screen, which is out of scope for the Purchase Order Detail tab fix this
 *  serves - see the Desktop-vs-Java report for that documented gap. */
function openDefineCityModal() {
    $('#modalDefineCity').modal('show');
    setTimeout(() => $('#txtNewCityName').focus(), 300);
}

function saveNewCity_Click() {
    const cityName = $('#txtNewCityName').val();
    if (!cityName || !cityName.trim()) {
        alert("CityName Field is Required");
        return;
    }
    $.ajax({
        url: '/api/purchase-order/save-city',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ cityName: cityName.trim() }),
        success: function(res) {
            if (res && res.success) {
                $('#hidLoadingCityId').val(res.id);
                $('#txtLoadingCityDisplay').val(res.cityName);
                $('#txtNewCityName').val('');
                $('#modalDefineCity').modal('hide');
                // Refresh the City Search modal's list immediately, ditto desktop's
                // CityDefineGridFill()/cmbCityFill() re-bind after a successful Insert(), so the
                // newly-defined city is selectable without a page reload.
                $.get('/api/purchase-order/cities', function(data) {
                    allCities = (data || []).map(c => ({ id: c.id, name: (c.cityName || '').trim() }))
                        .filter(c => c.name.length > 0);
                });
                alert(res.message);
            } else {
                alert(res && res.message ? res.message : "Error saving city.");
            }
        },
        error: function() {
            alert("Error saving city.");
        }
    });
}

/* ============================================================
 * 9. PACKING MATERIAL (EMPTY BAGS) & EXPENSES
 * Ditto of desktop PurchsaeOrder.cs grdEmptyBags (Type / Item / PackingType / Rate / WeightCut -
 * there is no Qty or Total Amount on the real desktop screen or in the real database table).
 * ============================================================ */
function clearEmptyBagInputs() {
    $('#cmbEbType').val('0');
    $('#cmbEbItem').val('0');
    $('#cmbEbPackingType').val('0');
    $('#txtEbRate').val('');
    $('#txtEbWeightCut').val('');
    editingEbIdx = -1;
    $('#btnAddEmptyBag').html('<i class="fa fa-plus"></i> Add Bag');
    $('#btnCancelEmptyBagEdit').hide();
}

function cancelEmptyBagEdit() {
    clearEmptyBagInputs();
}

/**
 * Ditto of PurchsaeOrder.cs's per-row Empty Bags validation (run before Save/Update):
 * EmptyBagsType required, PackingType required, WeightCut required unless Type=2
 * ('Purchase Against Weight'), and WeightCut must fall inside the selected Packing Type's
 * MinEbWeight/MaxEbWeight range (same message text as the desktop). The authoritative check
 * also runs server-side (PurchaseOrderFullService.persistEmptyBags) at Save time.
 */
function validateEmptyBagRow(type, packingTypeId, weightCut) {
    if (!type || type === 0) {
        return "EmptyBagsType filed required In Empty bags Grid...";
    }
    if (!packingTypeId || packingTypeId === 0) {
        return "PackingType filed required In Empty bags Grid...";
    }
    if (type !== 2 && weightCut <= 0) {
        return "WeightCut filed required In Empty bags Grid...";
    }
    if (weightCut > 0) {
        const validationTypeId = (packingTypeId === 1 || packingTypeId === 2 || packingTypeId === 5) ? packingTypeId : 2;
        const pt = emptyBagPackingTypes.find(p => parseInt(p.Id) === validationTypeId);
        if (pt) {
            const min = parseFloat(pt.MinEbWeight || 0);
            const max = parseFloat(pt.MaxEbWeight || 0);
            if (weightCut < min || weightCut > max) {
                return `Weight Cut Should be in Range of: ${min} to ${max} For Packing Type:${pt.PackTypeDesc || ''}`;
            }
        }
    }
    return null;
}

function btnAddEmptyBag_Click() {
    const type = parseInt($('#cmbEbType').val() || '0');
    const typeName = $('#cmbEbType option:selected').text();
    const itemId = parseInt($('#cmbEbItem').val() || '0');
    // Ditto of desktop grdEmptyBags "ItemId" ValueList column (PurchsaeOrder.cs GrdEmptyBagsRefresh():
    // HasValueList/LimitToList bound to dtEmptyBagsItem "ItemId"->"ItemName"). No real Item ever has
    // Id=0, so when nothing is selected Janus GridEX can't resolve the ValueList entry and falls back
    // to showing the raw underlying cell value as text - i.e. the real desktop screen literally shows
    // "0" here, not a blank cell. Match that instead of showing blank.
    const itemName = itemId > 0 ? $('#cmbEbItem option:selected').text() : String(itemId);
    const packingTypeId = parseInt($('#cmbEbPackingType').val() || '0');
    const packingTypeName = $('#cmbEbPackingType option:selected').text();
    const rate = parseFloat($('#txtEbRate').val() || '0');
    const weightCut = parseFloat($('#txtEbWeightCut').val() || '0');

    const validationError = validateEmptyBagRow(type, packingTypeId, weightCut);
    if (validationError) {
        alert(validationError);
        return;
    }

    const row = { type, typeName, itemId, itemName, packingTypeId, packingTypeName, rate, weightCut };

    if (editingEbIdx >= 0) {
        emptyBagItems[editingEbIdx] = row;
    } else {
        emptyBagItems.push(row);
    }

    renderEbGrid();
    clearEmptyBagInputs();
}

function editEmptyBagRow(idx) {
    const row = emptyBagItems[idx];
    if (!row) return;
    editingEbIdx = idx;
    $('#cmbEbType').val(row.type || 0);
    $('#cmbEbItem').val(row.itemId || 0);
    $('#cmbEbPackingType').val(row.packingTypeId || 0);
    $('#txtEbRate').val(row.rate);
    $('#txtEbWeightCut').val(row.weightCut);
    $('#btnAddEmptyBag').html('<i class="fa fa-check"></i> Update Bag');
    $('#btnCancelEmptyBagEdit').show();
}

function removeEmptyBagRow(idx) {
    if (!confirm("Remove this Packing Material (Empty Bags) row?")) {
        return;
    }
    emptyBagItems.splice(idx, 1);
    if (editingEbIdx === idx) {
        clearEmptyBagInputs();
    }
    renderEbGrid();
}

function resolveEmptyBagDisplayNames(b) {
    if (!b.typeName && b.type) {
        const t = emptyBagTypes.find(x => parseInt(x.Id) === parseInt(b.type));
        if (t) b.typeName = t.type;
    }
    if (!b.itemName && b.itemId) {
        const i = emptyBagItemOptions.find(x => parseInt(x.ItemId) === parseInt(b.itemId));
        if (i) b.itemName = i.ItemName;
    }
    // Ditto of desktop grdEmptyBags "ItemId" ValueList column (PurchsaeOrder.cs GrdEmptyBagsRefresh():
    // HasValueList/LimitToList bound to dtEmptyBagsItem "ItemId"->"ItemName"). Real default rows (and
    // any legacy/orphaned row) carry ItemId=0 or an Id with no matching Item master row, and no real
    // Item ever has Id=0 - so Janus GridEX cannot resolve a ValueList entry and displays the raw
    // underlying cell value as text instead of blank. The real desktop screen literally shows "0" (or
    // the stored, unmatched Id) here, never an empty cell - match that exactly rather than showing blank.
    if (!b.itemName) {
        b.itemName = (b.itemId !== undefined && b.itemId !== null) ? String(b.itemId) : '0';
    }
    if (!b.packingTypeName && b.packingTypeId) {
        const p = emptyBagPackingTypes.find(x => parseInt(x.Id) === parseInt(b.packingTypeId));
        if (p) b.packingTypeName = p.PackTypeDesc;
    }
    return b;
}

var selectedEbIdx = 0;

function selectEbRow(idx) {
    if (!emptyBagItems || idx < 0 || idx >= emptyBagItems.length) return;
    selectedEbIdx = idx;
    $('#tblEbTbody tr').removeClass('selected-row').css('background-color', '');
    $(`#tblEbTbody tr[data-idx="${idx}"]`).addClass('selected-row').css('background-color', '#cbe2f7');
    updateEbNavigator(selectedEbIdx + 1, emptyBagItems.length);
}

function updateEbNavigator(current, total) {
    $('#ebNavCurrent').val(total > 0 ? current : 0);
    $('#ebNavCurrent').attr('max', total);
    $('#ebNavTotal').text(total);

    $('#btnEbNavFirst, #btnEbNavPrev').prop('disabled', current <= 1 || total === 0);
    $('#btnEbNavNext, #btnEbNavLast').prop('disabled', current >= total || total === 0);
}

function ebNavFirst() {
    if (emptyBagItems && emptyBagItems.length > 0) selectEbRow(0);
}

function ebNavPrev() {
    if (selectedEbIdx > 0) selectEbRow(selectedEbIdx - 1);
}

function ebNavNext() {
    if (emptyBagItems && selectedEbIdx < emptyBagItems.length - 1) selectEbRow(selectedEbIdx + 1);
}

function ebNavLast() {
    if (emptyBagItems && emptyBagItems.length > 0) selectEbRow(emptyBagItems.length - 1);
}

function ebNavGoTo(val) {
    var idx = parseInt(val) - 1;
    if (!isNaN(idx) && idx >= 0 && emptyBagItems && idx < emptyBagItems.length) {
        selectEbRow(idx);
    }
}

function updateEbCell(idx, field, val) {
    if (!emptyBagItems || !emptyBagItems[idx]) return;
    var row = emptyBagItems[idx];

    // ValueList (dropdown) columns hold an integer Id, not a decimal - parse them as int and
    // keep the paired display name in step so Save/History/reopen keep working unchanged.
    if (field === 'type') {
        row.type = parseInt(val, 10) || 0;
        var t = emptyBagTypes.find(function (x) { return parseInt(x.Id, 10) === row.type; });
        row.typeName = t ? t.type : String(row.type);
        return;
    }
    if (field === 'itemId') {
        row.itemId = parseInt(val, 10) || 0;
        var i = emptyBagItemOptions.find(function (x) { return parseInt(x.ItemId, 10) === row.itemId; });
        row.itemName = i ? i.ItemName : String(row.itemId);
        return;
    }

    var num = parseFloat(String(val).replace(/,/g, ''));
    row[field] = isNaN(num) ? 0 : num;
}

/**
 * Builds the <option> list for an in-grid ValueList column, ditto of Janus GridEX
 * Column.ValueList.PopulateValueList(view, valueMember, displayMember) with LimitToList = true
 * (PurchsaeOrder.cs GrdEmptyBagsRefresh()). When the stored cell value matches no ValueList
 * entry - e.g. the seeded default rows that carry ItemId = 0, and no real Item ever has Id 0 -
 * GridEX falls back to painting the RAW underlying value as text rather than blanking the cell,
 * so the raw value is prepended as the selected option to reproduce that exactly.
 */
function ebValueListOptions(list, valueKey, textKey, currentVal) {
    var cur = parseInt(currentVal, 10);
    if (isNaN(cur)) cur = 0;
    var html = '';
    var matched = false;
    (list || []).forEach(function (o) {
        var v = parseInt(o[valueKey], 10);
        var isSel = (v === cur);
        if (isSel) matched = true;
        var text = (o[textKey] === undefined || o[textKey] === null) ? '' : String(o[textKey]);
        html += '<option value="' + v + '"' + (isSel ? ' selected' : '') + '>' + escapeHtml(text) + '</option>';
    });
    if (!matched) {
        html = '<option value="' + cur + '" selected>' + escapeHtml(String(cur)) + '</option>' + html;
    }
    return html;
}

function renderEbGrid() {
    const tbody = $('#tblEbTbody');
    tbody.empty();
    if (!emptyBagItems || emptyBagItems.length === 0) {
        tbody.html('<tr><td colspan="5" style="text-align: center; padding: 15px; color: #777;">No empty bag packing material specified.</td></tr>');
        updateEbNavigator(0, 0);
        return;
    }
    emptyBagItems.forEach((b, idx) => {
        resolveEmptyBagDisplayNames(b);
        const rateDisplay = (b.rate === undefined || b.rate === null) ? 0 : b.rate;
        const weightCutDisplay = (b.weightCut === undefined || b.weightCut === null) ? 0 : b.weightCut;
        const isSelected = selectedEbIdx === idx;
        const bgStyle = isSelected ? 'background-color: #cbe2f7;' : '';

        // Type and Item Name are in-cell dropdowns, ditto PurchsaeOrder.cs GrdEmptyBagsRefresh():
        // both columns are EditType 4 (Combo) with HasValueList/LimitToList = true.
        // PackingType is deliberately NOT a dropdown here: gridEmptyBagsSettings() runs
        // GrdEmptyBagsRefresh() and then immediately overrides
        // grdEmptyBags.RootTable.Columns["PackingType"].EditType = (EditType)0 (NoEdit), so on the
        // real screen it shows its ValueList text (PackTypeDesc) but cannot be edited.
        tbody.append(`
            <tr data-idx="${idx}" onclick="selectEbRow(${idx})" style="${bgStyle} cursor: pointer;">
                <td><select class="win-grid-cell-combo" onchange="updateEbCell(${idx}, 'type', this.value)">${ebValueListOptions(emptyBagTypes, 'Id', 'type', b.type)}</select></td>
                <td><select class="win-grid-cell-combo" onchange="updateEbCell(${idx}, 'itemId', this.value)">${ebValueListOptions(emptyBagItemOptions, 'ItemId', 'ItemName', b.itemId)}</select></td>
                <td style="text-align: right;" contenteditable="true" onblur="updateEbCell(${idx}, 'rate', this.innerText)">${rateDisplay}</td>
                <td>${escapeHtml(b.packingTypeName || '')}</td>
                <td style="text-align: right;" contenteditable="true" onblur="updateEbCell(${idx}, 'weightCut', this.innerText)">${weightCutDisplay}</td>
            </tr>
        `);
    });

    if (selectedEbIdx < 0 || selectedEbIdx >= emptyBagItems.length) {
        selectedEbIdx = 0;
    }
    updateEbNavigator(selectedEbIdx + 1, emptyBagItems.length);
}

/* ============================================================
 * 9A. SUPPLIER EXPENSE (Credit To Supplier & Debit To Product) - real desktop ditto.
 * Real table [dbo].[PurchaseOrderSupplierExpense]: Id, PurchaseOrderId, InvRevExpItemId, Qty, Rate,
 * Amount, Remarks. One row per real "Other Item" master row (Sp_InventoryItemsOther_GetAllMethod),
 * ditto PurchsaeOrder.cs's grdInvExp/dtInvExp seeding loop. Amount = Qty x Rate (auto-computed);
 * editing Amount directly resets Qty/Rate to 0, ditto grdInvExp_CellUpdated().
 * ============================================================ */
function loadSupplierExpenseDropdowns() {
    $.get('/api/purchase-order/supplier-expense/other-items', function(data) {
        otherItemsForExpense = data || [];
        renderExpGrid();
    });
}

function loadDefaultExpenseRows() {
    $.get('/api/purchase-order/supplier-expense/other-items', function(data) {
        otherItemsForExpense = data || [];
        expenseItems = otherItemsForExpense.map(function(it) {
            return { invRevExpItemId: it.Id, otherItemName: it.OtherItemName, qty: 0, rate: 0, amount: 0, remarks: '' };
        });
        renderExpGrid();
    });
}

function renderExpGrid() {
    const tbody = $('#tblExpTbody');
    tbody.empty();
    if (!expenseItems || expenseItems.length === 0) {
        tbody.html('<tr><td colspan="7" style="text-align: center; padding: 15px; color: #777;">No supplier expenses recorded.</td></tr>');
        return;
    }
    expenseItems.forEach((row, idx) => {
        const options = otherItemsForExpense.map(it =>
            `<option value="${it.Id}" ${it.Id == row.invRevExpItemId ? 'selected' : ''}>${escapeHtml(it.OtherItemName)}</option>`
        ).join('');
        tbody.append(`
            <tr>
                <td style="text-align: center;">${idx + 1}</td>
                <td><select class="win-combo" onchange="onExpItemChange(${idx}, this.value)"><option value="0">${placeholder}</option>${options}</select></td>
                <td><input type="number" step="0.01" class="win-textbox" style="text-align: right;" value="${row.qty}" onchange="onExpQtyRateChange(${idx}, 'qty', this.value)"/></td>
                <td><input type="number" step="0.01" class="win-textbox" style="text-align: right;" value="${row.rate}" onchange="onExpQtyRateChange(${idx}, 'rate', this.value)"/></td>
                <td><input type="number" step="0.01" class="win-textbox" style="text-align: right;" value="${(row.amount || 0).toFixed(2)}" onchange="onExpAmountChange(${idx}, this.value)"/></td>
                <td><input type="text" class="win-textbox" value="${escapeHtml(row.remarks || '')}" onchange="expenseItems[${idx}].remarks = this.value;"/></td>
                <td style="text-align: center;"><button type="button" class="btn btn-danger btn-xs" onclick="removeExpRow(${idx})">&times;</button></td>
            </tr>
        `);
    });
}

function onExpItemChange(idx, val) {
    const id = parseInt(val || '0');
    const found = otherItemsForExpense.find(it => it.Id == id);
    expenseItems[idx].invRevExpItemId = id;
    expenseItems[idx].otherItemName = found ? found.OtherItemName : '';
}

function onExpQtyRateChange(idx, field, val) {
    const num = parseFloat(val || '0') || 0;
    expenseItems[idx][field] = num;
    const qty = expenseItems[idx].qty || 0;
    const rate = expenseItems[idx].rate || 0;
    expenseItems[idx].amount = (qty > 0 && rate > 0) ? Math.round(qty * rate * 100) / 100 : 0;
    renderExpGrid();
}

function onExpAmountChange(idx, val) {
    const num = parseFloat(val || '0') || 0;
    expenseItems[idx].amount = Math.round(num * 100) / 100;
    expenseItems[idx].qty = 0;
    expenseItems[idx].rate = 0;
    renderExpGrid();
}

function removeExpRow(idx) {
    expenseItems.splice(idx, 1);
    renderExpGrid();
}

function btnAddExpenseRow_Click() {
    expenseItems.push({ invRevExpItemId: 0, otherItemName: '', qty: 0, rate: 0, amount: 0, remarks: '' });
    renderExpGrid();
}

/* ============================================================
 * 9B. ACCOUNT CREDIT _CHARGE TO PRODUCT - real desktop ditto.
 * Real table [dbo].[PurchaseOrderExpensesChargeToProduct]: Id, PurchaseOrderId, AccountId,
 * Percentage, Qty, Rate, Amount, Remarks, SupplierCustomerId. AccountTitle dropdown is the real
 * Chart of Account (4th-level/Detail) list (Sp_COAAllocation_GetAllMethod). Editing ItemQty/ItemRate
 * computes Amount = Qty x Rate (resets Percentage to 0); editing Percentage computes
 * Amount = round(TotalOrderAmount / 100 * Percentage) (resets ItemQty/ItemRate to 0), ditto
 * grdExpensesChargeToProduct_CellUpdated().
 * ============================================================ */
let chargeAccountsError = '';

function loadChargeToProductDropdowns() {
    $.get('/api/purchase-order/charge-to-product/accounts', function(data) {
        coaAccountsForCharge = data || [];
        chargeAccountsError = coaAccountsForCharge.length ? ''
            : 'The Account Title list came back empty.';
        renderChargeGrid();
    }).fail(function (xhr) {
        /* There was no .fail() here. A failed load left coaAccountsForCharge as [] and the
           Account Title cell rendered with nothing but "-- Select --" - indistinguishable, on
           screen, from a company that simply has no accounts configured. The operator's report
           was "onChargeAccountChange not working", which is what a silently empty dropdown
           looks like from the outside. */
        coaAccountsForCharge = [];
        chargeAccountsError = (xhr && xhr.responseJSON && (xhr.responseJSON.message || xhr.responseJSON.error))
            || (xhr && xhr.status ? ('the account list request failed with HTTP ' + xhr.status) : 'unknown error');
        console.error('[PO] Account Title list failed to load:', chargeAccountsError);
        renderChargeGrid();
    });
}

/* Case-insensitive column read. The server normalises both branches now, but a procedure's
   column casing is not something to depend on from the page. */
function acol(row, name) {
    if (!row) return undefined;
    if (row[name] !== undefined) return row[name];
    const k = Object.keys(row).find(x => x.toLowerCase() === String(name).toLowerCase());
    return k === undefined ? undefined : row[k];
}

/** The value member the desktop binds this list on - :3150 / :3155. */
function chargeAccountValue(a) {
    const v = (acol(a, 'bindOn') === 'SupplierCustomerId')
        ? acol(a, 'SupplierCustomerId') : acol(a, 'Id');
    const n = parseInt(v, 10);
    return isNaN(n) ? 0 : n;
}

function loadDefaultChargeRows() {
    chargeToProductItems = [{ accountId: 0, accountTitle: '', percentage: 0, qty: 0, rate: 0, amount: 0, remarks: '' }];
    renderChargeGrid();
}

function renderChargeGrid() {
    const tbody = $('#tblChargesTbody');
    tbody.empty();
    if (!chargeToProductItems || chargeToProductItems.length === 0) {
        tbody.html('<tr><td colspan="7" style="text-align: center; padding: 15px; color: #777;">No product account charges specified.</td></tr>');
        return;
    }
    chargeToProductItems.forEach((row, idx) => {
        /* data-code is the account code, the second column of the drop grid. Published only
           when the procedure returns one - absent renders an empty cell, not a placeholder. */
        /* grdChargeToProductRefresh binds this value list on SupplierCustomerId when
           SubsidiaryAccountAllownOnVouchers is on and on Id when it is off (:3150 / :3155).
           The server says which by returning bindOn, so the option value follows the desktop
           rather than always being the account Id. */
        /* An account whose value member or title did not resolve is DROPPED rather than drawn
           as an <option value="undefined"> with empty text - a blank row in the drop grid that
           selects NaN is worse than a shorter list, and it is what made this cell look broken. */
        const options = coaAccountsForCharge.map(a => {
            const v = chargeAccountValue(a);
            const title = acol(a, 'AccountTitle');
            if (!v || !title) return '';
            const code = acol(a, 'AccountCode') || '';
            return `<option value="${v}" data-code="${escapeHtml(code)}"`
                 + ` ${v == row.accountId ? 'selected' : ''}>${escapeHtml(title)}</option>`;
        }).join('');

        const placeholder = chargeAccountsError
            ? `-- ${escapeHtml(chargeAccountsError)} --`
            : '-- Select --';
        tbody.append(`
            <tr data-charge-row="${idx}">
                <td><select class="win-combo dtcombo" data-dtcombo="account2" data-dtcombo-caption="Account Title" onchange="onChargeAccountChange(${idx}, this.value)"><option value="0">-- Select --</option>${options}</select></td>
                <td><input type="number" step="0.01" class="win-textbox" data-charge-cell="percentage" style="text-align: right;" value="${row.percentage}" onchange="onChargePercentageChange(${idx}, this.value)"/></td>
                <td><input type="number" step="0.01" class="win-textbox" data-charge-cell="qty" style="text-align: right;" value="${row.qty}" onchange="onChargeQtyRateChange(${idx}, 'qty', this.value)"/></td>
                <td><input type="number" step="0.01" class="win-textbox" data-charge-cell="rate" style="text-align: right;" value="${row.rate}" onchange="onChargeQtyRateChange(${idx}, 'rate', this.value)"/></td>
                <td><input type="number" step="0.01" class="win-textbox win-textbox-readonly" data-charge-cell="amount" style="text-align: right;" value="${(row.amount || 0).toFixed(2)}" readonly/></td>
                <td><input type="text" class="win-textbox" data-charge-cell="remarks" value="${escapeHtml(row.remarks || '')}" onchange="chargeToProductItems[${idx}].remarks = this.value;"/></td>
                <td style="text-align: center;"><button type="button" class="btn btn-danger btn-xs" onclick="removeChargeRow(${idx})">&times;</button></td>
            </tr>
        `);
    });
}

/* :2945 - CellUpdated's AccountId branch. The row keeps BOTH the picked value and the title,
   because the title is what the desktop matches on in dtAccounts (:2896) and what the grid
   shows after a re-render. parseInt on an unresolved value used to yield NaN, which JSON
   serialises as null and the save path then refuses as "AccountTitle Field Required" - a
   refusal whose stated reason had nothing to do with the actual cause. */
function onChargeAccountChange(idx, val) {
    if (!chargeToProductItems[idx]) return;
    const id = parseInt(val, 10);
    const safeId = isNaN(id) ? 0 : id;
    const found = coaAccountsForCharge.find(a => chargeAccountValue(a) === safeId);
    chargeToProductItems[idx].accountId = safeId;
    chargeToProductItems[idx].accountTitle = found ? (acol(found, 'AccountTitle') || '') : '';
}

/* ============================================================
 * grdExpensesChargeToProduct_CellUpdated - PurchsaeOrder.cs :2528-2564.
 *
 * ---------------------------------------------------------------------------------------
 * WHAT WAS ACTUALLY WRONG, AND WHAT WAS NOT
 * ---------------------------------------------------------------------------------------
 * Reported: "click on Item Qty and it sets 0 automatically". The row read Percentage 0,
 * Item Qty 230, Item Rate 0, Amount 0.00.
 *
 * NOT A DEFECT - that row is what the desktop shows too. dtChargeToProduct seeds a new row
 * with Rows.Add(0, 0, 0, 0, 0, 0, "") (:2444) and declares ItemQty/ItemRate as
 * typeof(double) (:589-590), so those two cells are "0" and never empty. The :2540 guard
 * therefore passes, and the desktop likewise writes Amount = 230 x 0 = 0 and resets
 * Percentage to 0. Entering a Qty before a Rate is simply an incomplete row on both sides.
 *
 * THE REAL DEFECT - renderChargeGrid() was called from inside both change handlers. The
 * desktop assigns three cell values in place (:2543-2545 / :2553-2556) and never rebuilds
 * the grid. A change event fires on blur, so rebuilding the tbody destroys and replaces the
 * very <input> the operator is clicking into: the click lands on an element that no longer
 * exists, focus is lost, and anything typed into the replacement is discarded. That is the
 * behaviour being reported - the field is not zeroed by the click, it is replaced
 * mid-gesture. The rebuild also discarded every .dtcombo wrapper in the account column.
 * These now write the cells in place instead.
 *
 * A SECOND, SEPARATE DEFECT - :2553 is `if (TotlaPercentage > 0.0) { Amount = ... }` with no
 * else, so a percentage computing to zero or less leaves the existing Amount standing. This
 * had `amt > 0 ? Math.round(amt) : 0`, inventing a zero the desktop never writes.
 *
 * The :2540 and :2547 empty-cell guards are reproduced anyway. :2547 is reachable -
 * Percentage is an untyped (string) column (:588) and can genuinely be cleared - and :2540
 * costs nothing and keeps the ported condition honest against its source.
 * ============================================================ */

/** The raw text of one cell of one row - the web equivalent of item.Cells[k].Value.ToString(). */
function chargeCellText(idx, cell) {
    const el = $(`#tblChargesTbody tr[data-charge-row="${idx}"] input[data-charge-cell="${cell}"]`);
    return el.length ? String(el.val()) : '';
}

/** Writes one cell in place, model and DOM together, without rebuilding the row. */
function setChargeCell(idx, cell, value) {
    chargeToProductItems[idx][cell] = value;
    const el = $(`#tblChargesTbody tr[data-charge-row="${idx}"] input[data-charge-cell="${cell}"]`);
    if (el.length) el.val(cell === 'amount' ? (value || 0).toFixed(2) : value);
}

function onChargeQtyRateChange(idx, field, val) {
    /* The edited cell itself is always taken, ditto grdExpensesChargeToProduct.UpdateData(). */
    chargeToProductItems[idx][field] = parseFloat(val || '0') || 0;

    /* :2540 - both cells must be non-empty or the desktop does nothing at all. */
    const qtyText  = field === 'qty'  ? String(val == null ? '' : val) : chargeCellText(idx, 'qty');
    const rateText = field === 'rate' ? String(val == null ? '' : val) : chargeCellText(idx, 'rate');
    if (qtyText.trim() === '' || rateText.trim() === '') return;

    const qty  = parseFloat(qtyText  || '0') || 0;
    const rate = parseFloat(rateText || '0') || 0;
    setChargeCell(idx, 'amount', qty * rate);   /* :2543 */
    setChargeCell(idx, 'percentage', 0);        /* :2544 */
}

function onChargePercentageChange(idx, val) {
    const pctText = String(val == null ? '' : val);
    chargeToProductItems[idx].percentage = parseFloat(pctText || '0') || 0;

    /* :2547 - Percentage must be non-empty. */
    if (pctText.trim() === '') return;

    /* :2549 - grd.GetTotal(Amount, Sum) on the ITEM DETAIL grid, which is what
       calculateGrandTotalLineAmount() sums. Not the charge grid's own total. */
    const totalOrderAmt = calculateGrandTotalLineAmount();
    const amt = (totalOrderAmt / 100.0) * (parseFloat(pctText || '0') || 0);

    /* :2553 - Amount is written ONLY when the computed share is positive. No else branch. */
    if (amt > 0) setChargeCell(idx, 'amount', Math.round(amt));
    setChargeCell(idx, 'qty', 0);    /* :2555 */
    setChargeCell(idx, 'rate', 0);   /* :2556 */
}

function removeChargeRow(idx) {
    chargeToProductItems.splice(idx, 1);
    if (chargeToProductItems.length === 0) {
        chargeToProductItems.push({ accountId: 0, accountTitle: '', percentage: 0, qty: 0, rate: 0, amount: 0, remarks: '' });
    }
    renderChargeGrid();
}

function btnAddChargeRow_Click() {
    chargeToProductItems.push({ accountId: 0, accountTitle: '', percentage: 0, qty: 0, rate: 0, amount: 0, remarks: '' });
    renderChargeGrid();
}

/* ============================================================
 * 9C. PAYMENT DETAIL - real desktop ditto.
 * Real table [dbo].[PurchaseOrderPaymentTermsDetail]: Id, PurchaseOrderId, PaymentTermId,
 * PrcntOfTotal, Amount, DueDays, PaymentRemarks, SortNo, DueDate. Payment Term dropdown is the real
 * InvDueTerms master (Id=1 Cash, Id=2 Credit). Editing Due Days sets Due Date = DocDate + DueDays;
 * editing Due Date back-computes Due Days (must not be before DocDate). Editing %Of Total computes
 * Amount = round(TotalOrderAmount x Pct/100, 4) (max 100%); editing Amount back-computes %Of Total
 * (must not exceed TotalOrderAmount) - ditto grdPaymentDetail_CellUpdated().
 * ============================================================ */
function loadPaymentTermsOptions() {
    /* Payment terms come from Sp_InvDueTerms_GetAllMethod (@Activity='GetAll'), the same source
       the desktop's clsGlobalVariables.globalPaymentTerm is filled from. An empty read used to
       be replaced here with an invented Cash/Credit/Advance list carrying invented ids and
       dueDays - which would have let a purchase order be saved against a payment term id that
       does not exist in the database, and fed a made-up 30 into the due-date arithmetic. An
       empty read now stays empty and says so. */
    $.get('/api/purchase-order/payment-terms', function(data) {
        paymentTermsOptions = (data && data.length) ? data : [];
        if (!paymentTermsOptions.length) {
            console.warn('[PO] /api/purchase-order/payment-terms returned no rows - the payment '
                       + 'term list is empty; nothing is substituted for it.');
        }
        renderSchedGrid();
    });
}

function addDefaultPaymentRow() {
    const today = $('#txtDocDate').val() || ymdLocal(new Date());
    paymentTermsDetailItems.push({ paymentTermId: 0, paymentTerm: '', dueDays: 0, dueDate: today, prcntOfTotal: 0, amount: 0, remarks: '' });
}

function loadDefaultPaymentRows() {
    paymentTermsDetailItems = [];
    addDefaultPaymentRow();
    renderSchedGrid();
}

function getDocDateAsDate() {
    const v = $('#txtDocDate').val();
    return v ? new Date(v + 'T00:00:00') : new Date();
}

function renderSchedGrid() {
    const tbody = $('#tblSchedTbody');
    tbody.empty();

    if (!paymentTermsDetailItems || paymentTermsDetailItems.length === 0) {
        tbody.html('<tr><td colspan="7" style="text-align: center; padding: 15px; color: #777;">No payment terms added yet.</td></tr>');
        $('#lblSchedTotalPercent').text('0.00%');
        $('#lblSchedTotalAmount').text('0.00');
        return;
    }

    /* No invented fallback here either - see the loader above. With no terms read, each row's
       dropdown shows only its placeholder, so a term cannot be picked and nothing bogus is
       saved. */
    let optsList = paymentTermsOptions || [];

    let totalPct = 0, totalAmt = 0;
    paymentTermsDetailItems.forEach((row, idx) => {
        totalPct += (row.prcntOfTotal || 0);
        totalAmt += (row.amount || 0);
        /* data-code carries the term's due days, the second column of the drop grid. */
        const options = optsList.map(t =>
            `<option value="${t.id}" data-code="${t.dueDays !== undefined && t.dueDays !== null ? escapeHtml(t.dueDays) : ''}"`
            + ` ${t.id == row.paymentTermId ? 'selected' : ''}>${escapeHtml(t.description)}</option>`
        ).join('');
        tbody.append(`
            <tr>
                <td><select class="win-combo dtcombo" data-dtcombo="term2" data-dtcombo-caption="Payment Term" onchange="onSchedTermChange(${idx}, this.value)"><option value="0">-- Select --</option>${options}</select></td>
                <td><input type="number" class="win-textbox" style="text-align: center;" value="${row.dueDays}" onchange="onSchedDueDaysChange(${idx}, this.value)"/></td>
                <td><input type="date" class="win-datepicker" value="${row.dueDate}" onchange="onSchedDueDateChange(${idx}, this.value)"/></td>
                <td><input type="number" step="0.01" class="win-textbox" style="text-align: right;" value="${row.prcntOfTotal}" onchange="onSchedPercentChange(${idx}, this.value)"/></td>
                <td><input type="number" step="0.0001" class="win-textbox" style="text-align: right;" value="${(row.amount || 0).toFixed(4)}" onchange="onSchedAmountChange(${idx}, this.value)"/></td>
                <td><input type="text" class="win-textbox" value="${escapeHtml(row.remarks || '')}" onchange="paymentTermsDetailItems[${idx}].remarks = this.value;"/></td>
                <td style="text-align: center;"><button type="button" class="btn btn-danger btn-xs" onclick="removeSchedRow(${idx})">&times;</button></td>
            </tr>
        `);
    });

    $('#lblSchedTotalPercent').text(totalPct.toFixed(2) + '%');
    $('#lblSchedTotalAmount').text(totalAmt.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
}

function onSchedTermChange(idx, val) {
    const id = parseInt(val || '0');
    const found = paymentTermsOptions.find(t => t.id == id);
    paymentTermsDetailItems[idx].paymentTermId = id;
    paymentTermsDetailItems[idx].paymentTerm = found ? found.description : '';
}

function onSchedDueDaysChange(idx, val) {
    const days = parseInt(val || '0') || 0;
    paymentTermsDetailItems[idx].dueDays = days;
    const due = getDocDateAsDate();
    due.setDate(due.getDate() + days);
    paymentTermsDetailItems[idx].dueDate = ymdLocal(due);
    renderSchedGrid();
}

function onSchedDueDateChange(idx, val) {
    const docDate = getDocDateAsDate();
    const due = new Date(val + 'T00:00:00');
    if (due < docDate) {
        alert("Due Date Can't less Than DocDate");
        paymentTermsDetailItems[idx].dueDate = ymdLocal(docDate);
        renderSchedGrid();
        return;
    }
    paymentTermsDetailItems[idx].dueDate = val;
    const diffDays = Math.round((due - docDate) / (1000 * 60 * 60 * 24));
    paymentTermsDetailItems[idx].dueDays = diffDays;
    renderSchedGrid();
}

function onSchedPercentChange(idx, val) {
    let pct = parseFloat(val || '0') || 0;
    if (pct > 100) {
        alert("%of Total Can't Greater than 100");
        pct = 100;
    }
    paymentTermsDetailItems[idx].prcntOfTotal = pct;
    const totalOrderAmt = calculateGrandTotalLineAmount();
    paymentTermsDetailItems[idx].amount = Math.round((totalOrderAmt * pct / 100.0) * 10000) / 10000;
    renderSchedGrid();
}

function onSchedAmountChange(idx, val) {
    let amt = parseFloat(val || '0') || 0;
    const totalOrderAmt = calculateGrandTotalLineAmount();
    if (totalOrderAmt < amt) {
        alert("Amount Cant be Greater than Order Amount:" + totalOrderAmt);
        amt = 0;
    }
    paymentTermsDetailItems[idx].amount = amt;
    paymentTermsDetailItems[idx].prcntOfTotal = totalOrderAmt > 0 ? Math.round((amt * 100.0 / totalOrderAmt) * 100000000) / 100000000 : 0;
    renderSchedGrid();
}

function removeSchedRow(idx) {
    paymentTermsDetailItems.splice(idx, 1);
    if (paymentTermsDetailItems.length === 0) {
        addDefaultPaymentRow();
    }
    renderSchedGrid();
}

function btnAddPaymentRow_Click() {
    addDefaultPaymentRow();
    renderSchedGrid();
}

/* ============================================================
 * 10. TABS SWITCHING
 * ============================================================ */
function switchTab(tabId) {
    $('#poTabControl li').removeClass('active');
    $('.po-tab-pane').hide();

    $(`#poTabControl a[onclick="switchTab('${tabId}')"]`).parent().addClass('active');
    $('#' + tabId).show();
}

/* ============================================================
 * 11. TOOLBAR OPERATIONS (SAVE / UPDATE / NEW / LOAD ORDER)
 * ============================================================ */
function buildPayload() {
    return {
        purchaseOrderMasterId: currentPoMasterId,
        documentTypeId: 41,          /* PurchsaeOrder.cs:3295 - NOT 1052 (Commission Trading) */
        docNo: parseInt($('#txtDocNo').val() || '0'),
        docDate: $('#txtDocDate').val(),
        supplierId: parseInt($('#hidSupplierId').val() || '0'),
        bookingPersonId: parseInt($('#hidBookingPersonId').val() || '0'),
        deliveryStartDate: $('#txtDeliveryStartDate').val(),
        deliveryDays: parseInt($('#txtDeliveryDays').val() || '0'),
        /* po.OrderExpiryDate = DateTime.Now (:3310) - the desktop has no expiry control on
           this form, so it stamps the current date. There is no #txtExpiryDate here either,
           and reading it sent undefined. */
        expiryDate: ymdLocal(new Date()),
        paymentTermId: parseInt($('#cmbPaymentTerm').val() || '0'),
        dueDays: parseInt($('#txtDueDays').val() || '0'),
        paymentDueDate: $('#txtPaymentDueDate').val(),
        commissionAgentId: parseInt($('#hidCommissionAgentId').val() || '0'),
        /* The DTO field is commissionTypeName (a String), matching po.CommissionType, which the
           desktop sets from the combo's TEXT - :3324. Sent as "commType" this was an unknown
           property and Jackson dropped it silently. */
        commissionTypeName: $('#cmbCommType option:selected').text(),
        commRate: parseFloat($('#txtCommRate').val() || '0'),
        commUomId: parseInt($('#cmbCommUom').val() || '1'),
        commAmount: parseFloat($('#txtCommAmount').val() || '0'),
        brokerAccountId: parseInt($('#hidBrokerAccountId').val() || '0'),
        /* Likewise brokeryTypeName <- po.BrokeryType, also the combo's TEXT - :3332. */
        brokeryTypeName: $('#cmbBrokeryType option:selected').text(),
        brokeryRate: parseFloat($('#txtBrokeryRate').val() || '0'),
        brokeryRateUomId: parseInt($('#cmbBrokeryRateUom').val() || '1'),
        brokeryAmount: parseFloat($('#txtBrokeryAmount').val() || '0'),
        juteBagCut: parseFloat($('#txtJuteBagCut').val() || '0'),
        ppBagCut: parseFloat($('#txtPPBagCut').val() || '0'),
        /* CashFreight and CreditFreight are BOOLEANS driven by the rdFreightCash /
           rdFreightCredit radio pair (PurchsaeOrder.cs :3337-3344) - exactly one is true.
           This used to send the freight AMOUNT as cashFreight and a constant 0 as
           creditFreight (its control does not exist), so the freight type was never
           saved and the amount landed in the wrong column. */
        cashFreight:   $('#radCashFreight').is(':checked'),
        creditFreight: $('#radCreditFreight').is(':checked'),
        /* freightAmount has NO desktop counterpart: Architecture.Model.FeedMill.Purchase.PurchaseOrder
           declares only the two booleans above, and PurchsaeOrder.cs has no freight-amount control
           at all - just rdFreightCash / rdFreightCredit. It is not a DTO field either, so it was
           being dropped by Jackson. Still sent so the screen's value is not silently invented into
           some other column; it is recorded as an open question rather than mapped by guesswork. */
        freightAmount: parseFloat($('#txtCashFreight').val() || '0'),
        /* ------------------------------------------------------------------------------
           Header fields the DESKTOP writes that this payload never sent, so they were lost
           on every save. PurchsaeOrder.cs:3300-3353.

           Two are mandatory in the procedure itself: Sp_PurchaseOrder_Insert raises
           "DeliveryTerm Field Required" and "OrderStatus Field Required" on an empty value.
           Both come from the combo's TEXT on the desktop, not its id (:3312, :3352).
           ------------------------------------------------------------------------------ */
        branchNo:         parseInt($('#txtBranchNo').val() || '0'),
        supplierRefNo:    $('#txtSupplierRefNo').val(),
        /* comboText(): the desktop sends the combo's TEXT (:3312, :3352) and the procedure
           RAISERRORs on an empty one. These selects are driven by the shared dtcombo component,
           so read the selected option, then fall back to the select's own value - never send an
           empty string just because the option element was not the source of truth. */
        orderStatus:      comboText('#cmbOrderStatus'),
        deliveryTermName: comboText('#cmbDeliveryTerm'),
        orderCategoryId:  parseInt($('#cmbParentCategory').val() || '0'),
        categorySrNo:     parseInt($('#txtCategoryNo').val() || '0'),
        orderQty:         parseFloat($('#txtHeaderTotalQty').val() || '0'),
        orderWeight:      parseFloat($('#txtHeaderTotalWeight').val() || '0'),
        orderAmount:      parseFloat($('#txtHeaderTotalAmount').val() || '0'),
        /* LocationTypeId has no control on this page. The desktop reads cmbLocationType; the
           procedure defaults a 0 to 1 itself, so 0 is sent rather than a guessed value. */
        locationTypeId:   0,
        remarksHeader: $('#txtRemarksHeader').val(),
        lineItems: lineItems,
        emptyBags: emptyBagItems.map(function(b) {
            return {
                type: b.type,
                itemId: b.itemId,
                packingTypeId: b.packingTypeId,
                rate: b.rate,
                weightCut: b.weightCut
            };
        }),
        supplierExpenses: expenseItems.map(function(e) {
            return {
                invRevExpItemId: e.invRevExpItemId,
                qty: e.qty,
                rate: e.rate,
                amount: e.amount,
                remarks: e.remarks
            };
        }),
        expensesChargeToProduct: chargeToProductItems.map(function(c) {
            return {
                accountId: c.accountId,
                percentage: c.percentage,
                qty: c.qty,
                rate: c.rate,
                amount: c.amount,
                remarks: c.remarks
            };
        }),
        paymentTermsDetail: paymentTermsDetailItems.map(function(p) {
            return {
                paymentTermId: p.paymentTermId,
                dueDays: p.dueDays,
                dueDate: p.dueDate,
                prcntOfTotal: p.prcntOfTotal,
                amount: p.amount,
                paymentRemarks: p.remarks
            };
        })
    };
}

function btnSave_Click() {
    const payload = buildPayload();
    
    if (!payload.supplierId || payload.supplierId <= 0) {
        alert("Please select a Supplier / Party.");
        openSupplierSearchModal();
        return;
    }
    if (!payload.lineItems || payload.lineItems.length === 0) {
        alert("At least one purchase order detail item is required.");
        return;
    }

    $.ajax({
        url: '/api/purchase-order/save',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(payload),
        success: function(res) {
            if (res && (res.success || res.voucherHeadId)) {
                alert(res.message || "Purchase Order saved successfully.");
                btnNew_Click();
                loadPurchaseOrderHistory();
            } else {
                alert("Error saving Purchase Order: " + (res.message || "Unknown error"));
            }
        },
        error: function(xhr) {
            let errMsg = "Error saving Purchase Order";
            try {
                const res = JSON.parse(xhr.responseText);
                if (res && res.message) errMsg += ": " + res.message;
            } catch(e) {}
            alert(errMsg);
        }
    });
}

/* :5085-5104 - by the time this is reachable, applyEditModeState(po,'saveas') has already
   zeroed every detail Id and currentPoMasterId, so the ordinary save path inserts a new
   document. That is what SaveAs means on the desktop: a copy, not an update. */
function btnSaveAs_Click() {
    if (currentPoMasterId > 0) {
        alert('Save As expects a copied order. Reload it from History with Save As.');
        return;
    }
    btnSave_Click();
}

function btnUpdate_Click() {
    if (currentPoMasterId <= 0) {
        alert("No existing Purchase Order loaded for update. Use Load Order first.");
        openLoadOrderModal();
        return;
    }
    btnSave_Click();
}

function btnNew_Click() {
    clearEditModeState();      /* Reset() :3962-3972 - Save back, Update gone, locks cleared */
    currentPoMasterId = 0;
    lineItems = [];
    expenseItems = [];
    chargeToProductItems = [];
    paymentTermsDetailItems = [];
    editingEbIdx = -1;
    clearEmptyBagInputs();

    renderDetailGrid();
    renderExpGrid();
    renderChargeGrid();
    renderSchedGrid();

    $('#hidSupplierId').val('0');
    $('#txtSupplierDisplay').val('');
    $('#hidBrokerAccountId').val('0');
    $('#txtBrokerAcDisplay').val('');
    $('#hidCommissionAgentId').val('0');
    $('#txtCommAgentDisplay').val('');
    $('#hidBookingPersonId').val('0');
    $('#txtBookingPersonDisplay').val('-- Select --');
    $('#txtRemarksHeader').val('');

    fetchNextDocNo();
    setWinDefaultDates();

    // Ditto of desktop AddRowInvEmptyBagsGrid(): a brand-new Purchase Order always seeds two
    // Empty Bags rows (Jute Bags / PP Bags, Type=Normal) with real config-driven Weight Cut defaults.
    loadDefaultEmptyBagRows();

    // Ditto of desktop's grid-seeding for the other three real-data tabs (see initForm()).
    loadDefaultExpenseRows();
    loadDefaultChargeRows();
    loadDefaultPaymentRows();
}

function btnRefresh_Click() {
    btnNew_Click();
}

function openLoadOrderModal() {
    $.get('/api/purchase-order/history', function(data) {
        const tbody = $('#tblLoadOrderTbody');
        tbody.empty();
        if (!data || data.length === 0) {
            tbody.html('<tr><td colspan="5" style="text-align: center; padding: 20px;">No orders found.</td></tr>');
            $('#modalLoadOrder').modal('show');
            return;
        }
        data.forEach(po => {
            const docCode = po.voucherCode || po.docNo;
            const partyName = po.customerName || po.supplierName;
            const orderDate = po.orderDate || po.docDate;
            tbody.append(`
                <tr>
                    <td><strong style="color: #008080;">${docCode}</strong></td>
                    <td>${orderDate}</td>
                    <td>${escapeHtml(partyName || 'N/A')}</td>
                    <td>${escapeHtml(po.remarks || '')}</td>
                    <td style="text-align: center;">
                        <button type="button" class="win-btn-action" onclick="loadSelectedOrder(${po.id})">Load</button>
                    </td>
                </tr>
            `);
        });
        $('#modalLoadOrder').modal('show');
    });
}

function loadSelectedOrder(poId, mode) {
    $.get('/api/purchase-order/' + poId, function(po) {
        if (!po) return;

        currentPoMasterId = po.purchaseOrderMasterId;
        $('#txtDocNo').val(po.docNo);
        $('#lblDocNoDisplay').text("PO-2026-" + String(po.docNo).padStart(4, '0'));
        $('#txtDocDate').val(po.docDate);
        $('#hidSupplierId').val(po.supplierId || 0);
        $('#txtSupplierDisplay').val(po.supplierName || '');
        
        $('#hidBrokerAccountId').val(po.brokerAccountId || 0);
        $('#txtBrokerAcDisplay').val(po.brokerAccountName || (po.brokerAccountId ? 'Broker Ac #' + po.brokerAccountId : ''));
        
        $('#hidCommissionAgentId').val(po.commissionAgentId || 0);
        $('#txtCommAgentDisplay').val(po.commissionAgentName || (po.commissionAgentId ? 'Agent #' + po.commissionAgentId : ''));
        
        $('#hidBookingPersonId').val(po.bookingPersonId || 0);
        $('#txtBookingPersonDisplay').val(po.bookingPersonName || '-- Select --');

        $('#txtRemarksHeader').val(po.remarksHeader);

        lineItems = po.lineItems || [];
        renderDetailGrid();

        // Packing Material (Empty Bags) - real rows returned by Sp_PurchaseOrder_GetAllMethod
        // (@Activity='ReadPurchaseOrderEmptyBagsDetailByHeaderId'), same order as the desktop query.
        editingEbIdx = -1;
        clearEmptyBagInputs();
        emptyBagItems = (po.emptyBags || []).map(function(b) {
            return {
                type: b.Type,
                typeName: b.EmptyBagsType,
                itemId: b.ItemId,
                itemName: b.ItemName,
                packingTypeId: b.PackingTypeId,
                packingTypeName: b.PackTypeDesc,
                rate: b.Rate,
                weightCut: b.WeightCut
            };
        });
        renderEbGrid();

        // Supplier Expense - real rows returned by Sp_PurchaseOrder_GetAllMethod
        // (@Activity='ReadPurchaseOrderSupplierExpenseByHeaderId').
        expenseItems = (po.supplierExpenses || []).map(function(e) {
            return {
                invRevExpItemId: e.InvRevExpItemId,
                otherItemName: e.OtherItemName,
                qty: e.Qty,
                rate: e.Rate,
                amount: e.Amount,
                remarks: e.Remarks
            };
        });
        if (expenseItems.length === 0) {
            loadDefaultExpenseRows();
        } else {
            renderExpGrid();
        }

        // Account Credit _Charge to Product - real rows returned by Sp_PurchaseOrder_GetAllMethod
        // (@Activity='ReadPurchaseOrderExpensesChargeToProductDetailByHeaderId').
        chargeToProductItems = (po.expensesChargeToProduct || []).map(function(c) {
            return {
                accountId: c.AccountId,
                accountTitle: c.AccountTitle,
                percentage: c.Percentage !== undefined ? c.Percentage : c.percentage,
                /* The table's columns are Qty/Rate; the desktop's in-memory grid calls the same
                   two ItemQty/ItemRate. Accept either so a rename on the procedure side cannot
                   silently load them as 0. */
                qty: (c.Qty !== undefined ? c.Qty : (c.ItemQty !== undefined ? c.ItemQty : 0)),
                rate: (c.Rate !== undefined ? c.Rate : (c.ItemRate !== undefined ? c.ItemRate : 0)),
                amount: c.Amount,
                remarks: c.Remarks
            };
        });
        if (chargeToProductItems.length === 0) {
            loadDefaultChargeRows();
        } else {
            renderChargeGrid();
        }

        // Payment Detail - real rows returned by Sp_PurchaseOrder_GetAllMethod
        // (@Activity='PurchaseOrderPaymentTermDetailByHeaderId').
        paymentTermsDetailItems = (po.paymentTermsDetail || []).map(function(p) {
            return {
                paymentTermId: p.PaymentTermId,
                paymentTerm: p.PaymentTerm,
                dueDays: p.DueDays,
                dueDate: p.DueDate ? String(p.DueDate).split('T')[0] : '',
                prcntOfTotal: p.PrcntOfTotal,
                amount: p.Amount,
                remarks: p.PaymentRemarks
            };
        });
        if (paymentTermsDetailItems.length === 0) {
            loadDefaultPaymentRows();
        } else {
            renderSchedGrid();
        }

        /* ReadById :3721-3757 - the header fields the port was not filling at all.
           Every id below was checked to exist in purchase_order.html; writing to a selector
           that matches nothing is a silent no-op, which is how a field stays blank and looks
           like missing data. The desktop fields with NO web control yet are listed at the end
           of this function rather than quietly skipped. */
        $('#txtBranchNo').val(po.branchSrNo);
        $('#cmbParentCategory').val(po.orderCategoryId || 0);       /* combordercat :3722 */
        $('#txtCategoryNo').val(po.categorySrNo);
        $('#txtSupplierRefNo').val(po.supplierRefNo || '');
        $('#cmbPaymentTerm').val(po.paymentTermsId || 0);
        $('#txtDueDays').val(po.orderDueDays);
        $('#txtPaymentDueDate').val(po.orderDueDate || '');
        $('#cmbDeliveryTerm').val(po.deliveryTermId || 0);
        $('#txtDeliveryStartDate').val(po.deliveryStartDate || '');
        $('#txtDeliveryDays').val(po.deliveryDays);
        $('#cmbCommType').val(po.commissionType || '');             /* combcommtype :3746 */
        $('#cmbCommUom').val(po.commissionRateUom || 0);            /* combruom     :3747 */
        $('#txtCommRate').val(po.commRate);
        $('#txtCommAmount').val(po.commAmount);
        $('#cmbBrokeryType').val(po.brokeryType || '');
        $('#txtBrokeryRate').val(po.brokeryRate);
        $('#cmbBrokeryRateUom').val(po.brokeryUom || 0);
        $('#txtBrokeryAmount').val(po.brokeryAmount);
        $('#cmbOrderStatus').val(po.orderStatus || '');             /* CmbStatus :3755 */
        $('#radCashFreight').prop('checked', !!po.cashFreight);
        $('#radCreditFreight').prop('checked', !!po.creditFreight);
        if ($.fn.select2) {
            $('#cmbParentCategory, #cmbPaymentTerm, #cmbDeliveryTerm, #cmbCommType, #cmbCommUom, '
            + '#cmbBrokeryType, #cmbBrokeryRateUom, #cmbOrderStatus').trigger('change.select2');
        }

        /* NOT FILLED, because this page has no control for them yet - the desktop sets all six
           in ReadById. Recorded so they are not mistaken for working:
             cmbCurrency (:3736)   txtExchangeRate (:3737)  txtFcyAmount (:3738)
             OrderExpiryDate       CommissionRemarks        cmbLocationType (:3774) */

        applyEditModeState(po, mode || 'edit');

        $('#modalLoadOrder').modal('hide');
    });
}

/* ============================================================
 * The control states an existing record puts the form into.
 *
 * grdhistory_ColumnButtonClick :5076-5083 - Edit is Reset(), then ReadById(id), then
 * btnsave.Visible = false / btnSaveAs.Visible = false / btnUpdate.Visible = true.
 * ReadById :3860-3865 repeats that and enables CmbStatus.
 *
 * WHICH FIELDS LOCK IS A PROPERTY OF THE RECORD, NOT OF "being in edit mode", so the three
 * flags are computed on the server from the record's own rows and simply applied here:
 *
 *   lockOrderCategory  the order has lines                    (:2076, always when rows exist)
 *   lockSupplier       a line carries a Lab Sample            (:2070)
 *   lockDeliveryTerm   a supplier dispatch exists             (:3783)
 *
 * The desktop disables Supplier at :3781 when a dispatch exists and then RE-ENABLES it at
 * :2074 when no line has a lab sample. That looks like an oversight; it is reproduced, because
 * this is a parity port and inventing a stricter rule would be a different application.
 *
 * Nothing else is locked. In particular Doc No, Doc Date and the detail grid stay editable -
 * the desktop does not touch them here, and locking them "because it is an edit" would be a
 * rule this application does not have.
 * ============================================================ */
function applyEditModeState(po, mode) {
    isEditMode = true;
    const saveAs = (mode === 'saveas');

    /* :5080-5082 Edit -> Update only. :5088-5091 SaveAs -> SaveAs only. */
    $('#btnSave').hide();
    $('#btnUpdate').toggle(!saveAs);
    $('#btnSaveAs').toggle(saveAs);

    /* :3865 / :5094 - CmbStatus.Enabled = btnUpdate.Visible, so SaveAs leaves it locked. */
    setFieldLocked('#cmbOrderStatus', saveAs);

    /* :5095-5103 - SaveAs writes the lines as NEW rows. */
    if (saveAs) {
        lineItems.forEach(function (l) { l.purchaseOrderDetailId = 0; });
        currentPoMasterId = 0;
        renderDetailGrid();
    }

    setFieldLocked('#cmbParentCategory', !!po.lockOrderCategory);   /* combordercat */
    setFieldLocked('#cmbSupplier',       !!po.lockSupplier);        /* combsuppname */
    setFieldLocked('#cmbDeliveryTerm',   !!po.lockDeliveryTerm);    /* combdeliverytrm */

    /* :3969-3972 Reset() disables these four; combsalesman_Leave :1654 is the only thing that
       enables them, so they are live exactly when a commission agent is set. */
    const commOn = !!po.commissionFieldsEnabled;
    ['#cmbCommType', '#txtCommRate', '#cmbCommUom', '#txtCommAmount']
        .forEach(sel => setFieldLocked(sel, !commOn));

    /* GAP, not silently skipped: :3784 makes the Supplier Loading Detail grid visible once a
       dispatch exists, and THIS PAGE HAS NO SUCH GRID. po.supplierDispatchDetail carries the
       rows and po.hasSupplierDispatch the flag, so building it is a UI job only. Until then
       the operator can see that Delivery Term is locked but not what dispatched against the
       order. Logged rather than shown, so it does not look like an error to the operator. */
    if (po.hasSupplierDispatch) {
        console.info('[PO] This order has supplier dispatch rows (' +
            (po.supplierDispatchDetail || []).length + '). Delivery Term is locked; the ' +
            'Supplier Loading Detail grid is not implemented on the web form yet.');
    }
}

/* Back to a blank form: btnnew_Click -> Reset() :3922-3999. */
function clearEditModeState() {
    isEditMode = false;
    $('#btnSave').show();
    $('#btnUpdate').hide();
    $('#btnSaveAs').hide();
    setFieldLocked('#cmbParentCategory', false);
    setFieldLocked('#cmbSupplier', false);
    setFieldLocked('#cmbDeliveryTerm', false);   /* :3939 combdeliverytrm.Enabled = true */
    /* :3969-3972 - Reset leaves the commission four DISABLED until an agent is picked. */
    ['#cmbCommType', '#txtCommRate', '#cmbCommUom', '#txtCommAmount']
        .forEach(sel => setFieldLocked(sel, true));
}

/* A <select> cannot be made read-only, only disabled - and a disabled control is not posted,
   which would silently drop the value on Update. So a locked select is disabled for the user
   and its value carried in a hidden twin that still posts. Text inputs use readonly, which
   keeps them in the form data. */
function setFieldLocked(selector, locked) {
    const el = $(selector);
    if (!el.length) return;
    const isSelect = el.is('select');
    if (isSelect) {
        el.prop('disabled', !!locked);
        const twinId = (el.attr('id') || '') + '_lockedTwin';
        $('#' + twinId).remove();
        if (locked) {
            el.after(`<input type="hidden" id="${twinId}" name="${el.attr('name') || el.attr('id')}" value="${el.val() || ''}">`);
        }
    } else if (el.is('input, textarea')) {
        el.prop('readonly', !!locked);
    } else {
        el.prop('disabled', !!locked);
    }
    el.css('background-color', locked ? '#ebebe4' : '');
    el.attr('title', locked ? 'Locked on this Purchase Order — see the record\'s own state' : '');
}

function btnPrintReport(reportType) {
    alert("Report printing generated for format: " + reportType + " (Doc No: " + $('#txtDocNo').val() + ")");
}

function openAttachmentsModal() {
    $('#modalAttachments').modal('show');
}

/* ============================================================
 * 12. KEYBOARD SHORTCUTS & HELPERS
 * ============================================================ */
function bindKeyboardShortcuts() {
    $(document).keydown(function(e) {
        if (e.altKey && e.keyCode === 78) { // Alt + N
            e.preventDefault();
            btnNew_Click();
        } else if (e.altKey && e.keyCode === 83) { // Alt + S
            e.preventDefault();
            btnSave_Click();
        } else if (e.altKey && e.keyCode === 85) { // Alt + U
            e.preventDefault();
            btnUpdate_Click();
        } else if (e.altKey && e.keyCode === 65) { // Alt + A
            e.preventDefault();
            btnAddDetailRow_Click();
        }
    });
}

function switchMainView(view) {
    if (view === 'form') {
        $('#tabBtnForm').addClass('active').css({ 'background': '#fff', 'color': '#000' });
        $('#tabBtnHistory').removeClass('active').css({ 'background': '#ece9d8', 'color': '#555' });
        $('#mainViewForm').show();
        $('#mainViewHistory').hide();
    } else if (view === 'history') {
        $('#tabBtnHistory').addClass('active').css({ 'background': '#fff', 'color': '#000' });
        $('#tabBtnForm').removeClass('active').css({ 'background': '#ece9d8', 'color': '#555' });
        $('#mainViewForm').hide();
        $('#mainViewHistory').show();
        loadPurchaseOrderHistory();
    }
}

let currentHistoryRecords = [];
let selectedHistoryRecordIdx = -1;

function loadPurchaseOrderHistory() {
    /* The Branch SrNo / Branch Name columns exist or not according to PurchaseOrderBranchWise,
       and the number formats come from configuration, so the grid cannot be drawn before
       history-meta has answered. */
    if (!historyMetaLoaded) { loadHistoryMeta(loadPurchaseOrderHistory); return; }

    const useFromDate = $('#chkHistoryEnableFromDate').is(':checked');
    const fromDate = useFromDate ? ($('#txtHistoryFromDate').val() || '') : '';
    const useToDate = $('#chkHistoryEnableToDate').is(':checked');
    const toDate = useToDate ? ($('#txtHistoryToDate').val() || '') : '';
    const fromDocNo = $('#txtHistoryFromDocNo').val() || '';
    const toDocNo = $('#txtHistoryToDocNo').val() || '';
    /* The template selects the history supplier through a search modal that stores the id in
       #hidHistorySupplierId (there is no #cmbHistorySupplier control). */
    const supplierId = $('#hidHistorySupplierId').val() || '';
    const bookingPersonId = $('#cmbHistoryBookingPerson').val() || '';

    /* Four mutually exclusive date-type radios, one radio GROUP named radHistoryDateType.
       Desktop: rddocdate / rdentrydate / rdmodifydate / rdapproveddate, PurchsaeOrder.cs :4715-4756. */
    let dateType = 'Doc Date';
    switch ($('input[name="radHistoryDateType"]:checked').val()) {
        case 'ModifyDate':   dateType = 'Modify Date';   break;
        case 'EntryDate':    dateType = 'Entry Date';    break;
        case 'ApprovedDate': dateType = 'Approved Date'; break;
        default:             dateType = 'Doc Date';      break;
    }

    /* PurchsaeOrder.cs :4761-4773 - the branch filter is a comma-separated STRING of branch
       ids (@BranchesIds), built by splitting the combo's TEXT and looking each name up in
       dtBranch. It is multi-select on the desktop. A single integer was being sent here, which
       the server then compared to po.BranchId - and an id that matches no stored Purchase
       Order returns nothing at all, which is why the grid was empty while the desktop showed
       three rows for the same filters.

       :4761 also wraps the entire query in `if (cmbBranchName.Text != string.Empty)`: with no
       branch chosen the desktop does not query. The server reproduces that. */
    const branchIds = $('#cmbHistoryBranch').val();
    const branchParam = Array.isArray(branchIds)
        ? branchIds.filter(v => v && v !== '0').join(',')
        : (branchIds && branchIds !== '0' ? String(branchIds) : '');

    const query = $.param({
        fromDate: fromDate,
        toDate: toDate,
        fromDocNo: fromDocNo,
        toDocNo: toDocNo,
        supplierId: supplierId,
        bookingPersonId: bookingPersonId,
        branchIds: branchParam,
        dateType: dateType
    });

    $('#tblHistoryTbody').html('<tr><td colspan="15" style="text-align: center; padding: 20px; color: #555;"><i class="fa fa-spinner fa-spin"></i> Loading Purchase Order history...</td></tr>');
    $('#tblHistoryDetailTbody').html('<tr><td colspan="11" style="text-align: center; padding: 15px; color: #777;">Select a history row above to view line item details.</td></tr>');

    if (!branchParam) {
        $('#tblHistoryTbody').html('<tr><td colspan="15" style="text-align: center; padding: 20px; '
            + 'color: #777;">Select a Branch Name to show history.</td></tr>');
        currentHistoryRecords = [];
        selectedHistoryRecordIdx = -1;
        return;
    }

    $.get(`/api/purchase-order/history?${query}`, function(data) {
        currentHistoryRecords = data || [];
        selectedHistoryRecordIdx = -1;
        renderHistoryMasterGrid();
    }).fail(function() {
        $('#tblHistoryTbody').html('<tr><td colspan="15" style="text-align: center; padding: 20px; color: #d32f2f;">Failed to load history records.</td></tr>');
    });
}

/* ============================================================
 * THE HISTORY GRIDS - PurchsaeOrder.cs :4777-4816 (dtHistory), :4838-4963
 * (HistoryGridSettings), :4971-5010 (getUpdateForHistory), :5021-5048 (grdDetailSettings).
 *
 * -----------------------------------------------------------------------------------------
 * WHAT WAS WRONG
 * -----------------------------------------------------------------------------------------
 * 1. THE HEADER AND THE BODY DISAGREED. The template's <thead> listed
 *    Action | Doc Date | Doc No | Branch Name | Supplier Name | Booking Person | Order Qty |
 *    Total Amount | Delivery Term | Status | Entry Date | Remarks   (12 cells)
 *    while renderHistoryMasterGrid emitted
 *    action | branch | docNo | docDate | party | deliveryStart | expiry | remarks | status |
 *    entryUser | approved                                          (11 cells)
 *    so every value sat one column away from its own heading and the last heading had no cell
 *    at all. On screen: the branch name under "Doc Date", the doc date under "Branch Name",
 *    a delivery date under "Booking Person", "Active" under "Delivery Term", and the raw
 *    EntryUser id 77 under "Status". Nothing was wrong with the DATA - the layout was.
 *
 * 2. IT WAS THE WRONG COLUMN SET. The desktop's history grid has 37 data columns; this had 11
 *    invented ones, several of which (Total Amount, Order Qty as separate notions) the desktop
 *    does not carry on this grid at all.
 *
 * 3. `EntryUser` WAS READ FROM THE WRONG FIELD. :4816 maps the EntryUser COLUMN from
 *    dt["UserName"], not from dt["EntryUser"] - the latter is the numeric user id, which is
 *    exactly the 77 that appeared on screen. Several columns have this shape: ApprovedDate
 *    comes from PostDate, PaymentTerm from TermsDescription, ItemQty from OrderQty,
 *    DeliveryTerm from DeliveryTermDB, ExpiryDate from OrderExpiryDate, CommAgent from
 *    CommissionAgentName, BrokeryAc from BrokerAgentName, Remarks from RemarksHeader. Every
 *    one of them is named in the table below beside the column it feeds.
 *
 * 4. THERE WAS NO TOTALS ROW. :4896 / :4903 / :4909 set AggregateFunction Sum on ItemQty,
 *    CommAmount and BrokeryAmount - the 810 under Item Qty in the desktop grid.
 *
 * Header, body and totals are now all generated from HISTORY_COLUMNS, so they cannot drift
 * apart again.
 * ============================================================ */

/* Two formats the desktop reads from configuration rather than hardcoding
   (CommonServices :5397 / :5420). Filled by /api/purchase-order/history-meta. */
let historyMeta = { branchImplemented: false, amountDecimals: 0, rateDecimals: 2 };
let historyMetaLoaded = false;

function loadHistoryMeta(done) {
    $.get('/api/purchase-order/history-meta', function (m) {
        if (m) historyMeta = m;
    }).always(function () { historyMetaLoaded = true; if (done) done(); });
}

/* num  - "#,##0.###"  (ItemQty, Weight)
   amt  - stringFormatsingle, "#,##0." + amountDecimals
   rate - DecimalRateFormate, "#,#0." + rateDecimals
   int  - plain integer, no grouping (DueDays, DeliveryDays, NoOfAttachments)
   date - ToShortDateString()
   dt   - "dd-MM-yyyy hh:mm tt" (:4888-4890) */
function fmtHistory(v, kind) {
    if (v === null || v === undefined || v === '') return '';
    if (kind === 'date') return String(v).split('T')[0];
    if (kind === 'dt') {
        const d = new Date(v);
        if (isNaN(d.getTime())) return String(v);
        const p = n => String(n).padStart(2, '0');
        let h = d.getHours(); const ap = h >= 12 ? 'PM' : 'AM'; h = h % 12 || 12;
        return `${p(d.getDate())}-${p(d.getMonth() + 1)}-${d.getFullYear()} ${p(h)}:${p(d.getMinutes())} ${ap}`;
    }
    const n = parseFloat(v);
    if (isNaN(n)) return String(v);
    if (kind === 'int')  return String(Math.round(n));
    if (kind === 'num')  return n.toLocaleString('en-US', { maximumFractionDigits: 3 });
    if (kind === 'amt')  return n.toLocaleString('en-US', { minimumFractionDigits: historyMeta.amountDecimals, maximumFractionDigits: historyMeta.amountDecimals });
    if (kind === 'rate') return n.toLocaleString('en-US', { minimumFractionDigits: historyMeta.rateDecimals,   maximumFractionDigits: historyMeta.rateDecimals });
    return String(v);
}

/** Case-insensitive column read - a procedure's column casing is not something to assume. */
function hcol(row, name) {
    if (!row) return null;
    if (row[name] !== undefined) return row[name];
    const k = Object.keys(row).find(x => x.toLowerCase() === String(name).toLowerCase());
    return k === undefined ? null : row[k];
}

/* dtHistory, :4777-4816. `caption` is what GridEX shows, `src` is the column of the procedure
   result the desktop copies into it, and they are NOT always the same word. `sum` marks the
   three columns HistoryGridSettings gives AggregateFunction Sum. `branchOnly` marks the two
   columns hidden when PurchaseOrderBranchWise is on (:4842-4846, :4891-4892). */
const HISTORY_COLUMNS = [
    { caption: 'Doc No',              src: 'DocNo',               kind: 'int',  w: 70,  align: 'right' },
    { caption: 'Branch SrNo',         src: 'BranchSrNo',          kind: 'int',  w: 80,  align: 'right', branchOnly: true },
    { caption: 'Branch Name',         src: 'BranchName',          kind: 'str',  w: 150,                 branchOnly: true },
    { caption: 'Doc Date',            src: 'DocDate',             kind: 'date', w: 90 },
    { caption: 'Item Qty',            src: 'OrderQty',            kind: 'num',  w: 90,  align: 'right', sum: true },
    { caption: 'Supplier Name',       src: 'SupplierName',        kind: 'str',  w: 235 },
    { caption: 'Booking Person',      src: 'BookingPerson',       kind: 'str',  w: 150 },
    { caption: 'Payment Term',        src: 'TermsDescription',    kind: 'str',  w: 90 },
    { caption: 'Due Days',            src: 'DueDays',             kind: 'int',  w: 70,  align: 'right' },
    { caption: 'Due Date',            src: 'DueDate',             kind: 'date', w: 90 },
    { caption: 'Delivery Term',       src: 'DeliveryTermDB',      kind: 'str',  w: 90 },
    { caption: 'Delivery Days',       src: 'DeliveryDays',        kind: 'int',  w: 80,  align: 'right' },
    { caption: 'Delivery Start Date', src: 'DeliveryStartDate',   kind: 'date', w: 100 },
    { caption: 'Expiry Date',         src: 'OrderExpiryDate',     kind: 'date', w: 90 },
    { caption: 'Order Status',        src: 'OrderStatus',         kind: 'str',  w: 90 },
    { caption: 'Comm Agent',          src: 'CommissionAgentName', kind: 'str',  w: 150 },
    { caption: 'Comm Type',           src: 'CommissionType',      kind: 'str',  w: 90 },
    { caption: 'Comm Rate',           src: 'CommRate',            kind: 'rate', w: 80,  align: 'right' },
    { caption: 'Comm Amount',         src: 'CommAmount',          kind: 'amt',  w: 100, align: 'right', sum: true },
    { caption: 'Comm Remarks',        src: 'CommissionRemarks',   kind: 'str',  w: 150 },
    { caption: 'Brokery Ac',          src: 'BrokerAgentName',     kind: 'str',  w: 150 },
    { caption: 'Brokery Type',        src: 'BrokeryType',         kind: 'str',  w: 90 },
    { caption: 'Brokery Rate',        src: 'BrokeryRate',         kind: 'rate', w: 80,  align: 'right' },
    { caption: 'Brokery Amount',      src: 'BrokeryAmount',       kind: 'amt',  w: 100, align: 'right', sum: true },
    { caption: 'Approved Status',     src: '__approved',          kind: 'str',  w: 90 },
    { caption: 'PreBill Nos',         src: 'PreBillNos',          kind: 'str',  w: 100 },
    { caption: 'PreBill Vehicle Nos', src: 'PreBillVehicleNos',   kind: 'str',  w: 100 },
    { caption: 'Entry User',          src: 'UserName',            kind: 'str',  w: 110 },
    { caption: 'Entry Date',          src: 'EntryDate',           kind: 'dt',   w: 130 },
    { caption: 'Modify User',         src: 'ModifyUserName',      kind: 'str',  w: 110 },
    { caption: 'Modify Date',         src: 'ModifyDate',          kind: 'dt',   w: 130 },
    { caption: 'Approved User',       src: 'ApprovedUserName',    kind: 'str',  w: 110 },
    { caption: 'Approved Date',       src: 'PostDate',            kind: 'dt',   w: 130 },
    { caption: 'No Of Attachments',   src: 'NoOfAttachments',     kind: 'int',  w: 80,  align: 'center' },
    { caption: 'Remarks',             src: 'RemarksHeader',       kind: 'str',  w: 180 },
    { caption: 'Location Type',       src: 'LocationType',        kind: 'str',  w: 100 }
];

function visibleHistoryColumns() {
    return HISTORY_COLUMNS.filter(c => !(c.branchOnly && historyMeta.branchImplemented));
}

/* :4911-4962 - six button columns, each inserted at DocNo.Position - 1 in this order, so they
   all end up to the LEFT of Doc No. Each is gated on its own right; the web has no per-right
   gate here yet, so all six are drawn and that is recorded as a gap rather than hidden. */
const HISTORY_BUTTONS = [
    { key: 'Edit',     label: 'Edit',     w: 40 },
    { key: 'SaveAs',   label: 'SaveAs',   w: 60 },
    { key: 'Print',    label: 'Print',    w: 40 },
    { key: 'Print-A',  label: 'Print-A',  w: 50 },
    { key: 'PrintII',  label: 'PrintII',  w: 50 },
    { key: 'PrintIII', label: 'PrintIII', w: 55 }
];

function renderHistoryMasterGrid() {
    const cols  = visibleHistoryColumns();
    const thead = $('#tblHistoryThead');
    const tbody = $('#tblHistoryTbody');
    const tfoot = $('#tblHistoryTfoot');
    const span  = cols.length + HISTORY_BUTTONS.length + 1;   /* +1 = Add Attachment (:4960) */

    thead.html('<tr style="background: linear-gradient(to bottom, #dceaf7 0%, #cbe2f7 100%); color: #000; font-weight: bold;">'
        + HISTORY_BUTTONS.map(b => `<th style="width:${b.w}px; text-align:center;">${escapeHtml(b.label)}</th>`).join('')
        + cols.map(c => `<th style="width:${c.w}px; text-align:${c.align || 'left'};">${escapeHtml(c.caption)}</th>`).join('')
        + '<th style="width:105px; text-align:center;">Add Attachment</th>'
        + '</tr>');

    tbody.empty();
    tfoot.empty();

    if (!currentHistoryRecords || currentHistoryRecords.length === 0) {
        tbody.html(`<tr><td colspan="${span}" style="text-align: center; padding: 20px; color: #777;">No matching Purchase Order history records found.</td></tr>`);
        updateHistoryNavDisplay();
        return;
    }

    const totals = {};
    cols.forEach(c => { if (c.sum) totals[c.src] = 0; });

    currentHistoryRecords.forEach((po, idx) => {
        const poId = parseInt(hcol(po, 'Id') || 0, 10) || 0;

        const cells = cols.map(c => {
            let raw;
            if (c.src === '__approved') {
                /* :4816 - Conversion.ToBool(IsAproved) ? "Approved" : "Not Approved".
                   Note the desktop's spelling of the column: IsAproved, one 'p'. */
                const v = hcol(po, 'IsAproved');
                raw = (v === true || v === 1 || v === '1' || String(v).toLowerCase() === 'true')
                    ? 'Approved' : 'Not Approved';
            } else {
                raw = hcol(po, c.src);
            }
            if (c.sum) {
                const n = parseFloat(raw);
                if (!isNaN(n)) totals[c.src] += n;
            }
            const text = (c.src === '__approved') ? raw : fmtHistory(raw, c.kind);
            return `<td style="text-align:${c.align || 'left'};">${escapeHtml(String(text))}</td>`;
        }).join('');

        const buttons = HISTORY_BUTTONS.map(b => `<td style="text-align:center;" class="action-col">`
            + `<button type="button" class="win-btn-action" onclick="event.stopPropagation(); onHistoryButtonClick('${b.key}', ${poId})">`
            + `${escapeHtml(b.label)}</button></td>`).join('');

        const isSelected = (idx === selectedHistoryRecordIdx);
        tbody.append(`
            <tr id="histRow_${idx}" onclick="onHistoryRowClick(${idx})" ondblclick="loadSelectedOrderFromHistory(${poId})" style="cursor: pointer; background-color: ${isSelected ? '#b2dfdb' : ''};">
                ${buttons}${cells}
                <td style="text-align:center;" class="action-col"><button type="button" class="win-btn-action" onclick="event.stopPropagation(); onHistoryButtonClick('AddAttachment', ${poId})"><i class="fa fa-paperclip"></i></button></td>
            </tr>
        `);
    });

    /* :4896 / :4903 / :4909 - Sum on ItemQty, CommAmount, BrokeryAmount. */
    tfoot.html('<tr style="background:#ece9d8; font-weight:bold; border-top:2px solid #7a9a9e;">'
        + HISTORY_BUTTONS.map(() => '<td></td>').join('')
        + cols.map(c => c.sum
            ? `<td style="text-align:right;">${escapeHtml(fmtHistory(totals[c.src], c.kind))}</td>`
            : '<td></td>').join('')
        + '<td></td></tr>');

    updateHistoryNavDisplay();

    if (currentHistoryRecords.length > 0 && selectedHistoryRecordIdx < 0) {
        onHistoryRowClick(0);
    }
}

/* :5063 grdhistory_ColumnButtonClick. Only Edit is wired; the five print variants and the
   attachment dialog are separate desktop report/dialog paths that are NOT built, and saying so
   is better than a button that silently does nothing. */
function onHistoryButtonClick(key, poId) {
    if (key === 'Edit')   { loadSelectedOrderFromHistory(poId, 'edit');   return; }
    if (key === 'SaveAs') { loadSelectedOrderFromHistory(poId, 'saveas'); return; }
    alert(key + ' is not implemented for the web Purchase Order yet.');
}

function onHistoryRowClick(idx) {
    if (idx < 0 || idx >= currentHistoryRecords.length) return;

    selectedHistoryRecordIdx = idx;
    $('#tblHistoryTbody tr').css('background-color', '');
    $(`#histRow_${idx}`).css('background-color', '#b2dfdb');

    updateHistoryNavDisplay();
    loadHistoryDetailGrid(parseInt(hcol(currentHistoryRecords[idx], 'Id') || 0, 10) || 0);
}

/* getUpdateForHistory() :4986-5000 - the fourteen columns in the desktop's own order.
   FcyAmount is declared and then hidden at :5021, so it is carried and not drawn. */
const HISTORY_DETAIL_COLUMNS = [
    { caption: 'Item Code',  src: 'ItemCode', kind: 'str',  w: 100 },
    { caption: 'Item Name',  src: 'ItemName', kind: 'str',  w: 220 },
    { caption: 'Crop Year',  src: 'CropYear', kind: 'str',  w: 90 },
    { caption: 'UOM',        src: 'UOM',      kind: 'str',  w: 80 },
    { caption: 'Item QTY',   src: 'ItemQTY',  kind: 'num',  w: 90,  align: 'right', sum: true },
    { caption: 'Weight',     src: 'Weight',   kind: 'num',  w: 90,  align: 'right', sum: true },
    { caption: 'Item Rate',  src: 'ItemRate', kind: 'rate', w: 90,  align: 'right' },
    { caption: 'Rate UOM',   src: 'RateUOM',  kind: 'str',  w: 80 },
    { caption: 'Amount',     src: 'Amount',   kind: 'amt',  w: 110, align: 'right', sum: true },
    { caption: 'Job/Lot',    src: 'JobLot',   kind: 'str',  w: 90 },
    { caption: 'City Name',  src: 'CityName', kind: 'str',  w: 130 },
    { caption: 'Moisture%',  src: 'Moisture', kind: 'str',  w: 80,  align: 'right' },
    { caption: 'Remarks',    src: 'Remarks',  kind: 'str',  w: 180 }
];

function loadHistoryDetailGrid(poId) {
    const thead = $('#tblHistoryDetailThead');
    const tbody = $('#tblHistoryDetailTbody');
    const tfoot = $('#tblHistoryDetailTfoot');
    const cols  = HISTORY_DETAIL_COLUMNS;

    thead.html('<tr style="background: linear-gradient(to bottom, #ece9d8 0%, #dbd6c6 100%); color: #000; font-weight: bold;">'
        + cols.map(c => `<th style="width:${c.w}px; text-align:${c.align || 'left'};">${escapeHtml(c.caption)}</th>`).join('')
        + '</tr>');
    tfoot.empty();
    tbody.html(`<tr><td colspan="${cols.length}" style="text-align: center; padding: 15px; color: #555;"><i class="fa fa-spinner fa-spin"></i> Loading item details...</td></tr>`);

    if (!poId) {
        tbody.html(`<tr><td colspan="${cols.length}" style="text-align: center; padding: 15px; color: #d32f2f;">This history row carried no Id, so its detail cannot be read.</td></tr>`);
        updateHistoryDetailNavDisplay(0);
        return;
    }

    /* Its OWN endpoint, not the form-load payload: that one returns null from a bare catch when
       its hand-written header joins fail, which became a 404 and the bare "Failed to load item
       details" with no reason anywhere. See PurchaseOrderFullService.historyDetail. */
    $.get('/api/purchase-order/' + poId + '/history-detail', function (rows) {
        tbody.empty();
        const items = rows || [];
        if (items.length === 0) {
            tbody.html(`<tr><td colspan="${cols.length}" style="text-align: center; padding: 15px; color: #777;">No detail line items found for this Purchase Order.</td></tr>`);
            updateHistoryDetailNavDisplay(0);
            return;
        }

        const totals = {};
        cols.forEach(c => { if (c.sum) totals[c.src] = 0; });

        items.forEach(function (it) {
            const cells = cols.map(c => {
                const raw = hcol(it, c.src);
                if (c.sum) {
                    const n = parseFloat(raw);
                    if (!isNaN(n)) totals[c.src] += n;
                }
                return `<td style="text-align:${c.align || 'left'};">${escapeHtml(String(fmtHistory(raw, c.kind)))}</td>`;
            }).join('');
            tbody.append(`<tr>${cells}</tr>`);
        });

        /* :5023 / :5027 / :5034 - Sum on ItemQTY, Weight and Amount. */
        tfoot.html('<tr style="background:#ece9d8; font-weight:bold; border-top:2px solid #7a9a9e;">'
            + cols.map(c => c.sum
                ? `<td style="text-align:right;">${escapeHtml(fmtHistory(totals[c.src], c.kind))}</td>`
                : '<td></td>').join('')
            + '</tr>');

        updateHistoryDetailNavDisplay(items.length);
    }).fail(function (xhr) {
        const why = (xhr && xhr.responseJSON && (xhr.responseJSON.message || xhr.responseJSON.error))
            || (xhr && xhr.status ? ('HTTP ' + xhr.status) : 'unknown error');
        tbody.html(`<tr><td colspan="${cols.length}" style="text-align: center; padding: 15px; color: #d32f2f;">Could not load item details — ${escapeHtml(String(why))}</td></tr>`);
        updateHistoryDetailNavDisplay(0);
    });
}

function updateHistoryDetailNavDisplay(count) {
    $('#lblHistoryDetailRecordInfo').text((count > 0 ? 1 : 0) + ' Of ' + count);
}

function updateHistoryNavDisplay() {
    const total = currentHistoryRecords.length;
    const current = total > 0 ? (selectedHistoryRecordIdx + 1) : 0;
    /* Two labels carry this, and only one was being written - the record navigator UNDER the
       grid, which is the one the desktop has, was left reading "0 Of 0" while the grid held
       three rows. The desktop's navigator reads "1 Of 3", capital Of. */
    $('#lblHistoryRecordCount').text(`${current} of ${total}`);
    $('#lblHistoryRecordInfo').text(`${current} Of ${total}`);
}

function navHistory(action) {
    if (!currentHistoryRecords || currentHistoryRecords.length === 0) return;
    if (action === 'first') onHistoryRowClick(0);
    else if (action === 'prev' && selectedHistoryRecordIdx > 0) onHistoryRowClick(selectedHistoryRecordIdx - 1);
    else if (action === 'next' && selectedHistoryRecordIdx < currentHistoryRecords.length - 1) onHistoryRowClick(selectedHistoryRecordIdx + 1);
    else if (action === 'last') onHistoryRowClick(currentHistoryRecords.length - 1);
}

/* grdhistory_ColumnButtonClick :5076-5083 (Edit) and :5085-5104 (SaveAs).
 *
 * BOTH begin with Reset() before ReadById - :5077 / :5086. Without it the previous record's
 * values survive in any field the new record does not overwrite, which is how a stale supplier
 * or remark silently rides along into a different document. btnNew_Click() is this page's
 * Reset().
 *
 * They differ only in what comes after:
 *   Edit   -> Save and SaveAs hidden, Update shown.
 *   SaveAs -> Save and Update hidden, SaveAs shown; Status forced to its second row and
 *             DISABLED (:5093-5094, Enabled = btnUpdate.Visible, and Update is hidden here);
 *             and every detail row's Id is zeroed (:5095-5103) so the lines are written as new
 *             rows rather than updating the ones belonging to the order being copied.
 */
function loadSelectedOrderFromHistory(poId, mode) {
    btnNew_Click();                       /* :5077 Reset() */
    switchMainView('form');
    loadSelectedOrder(poId, mode || 'edit');
}

/* ============================================================
 * DEFINE LOOKUP PARTIES MODAL & PERSISTENCE
 * ============================================================ */
function openDefineLookUpPartiesModal() {
    loadLookupPartiesGrid();
    $('#modalDefineLookUpParties').modal('show');
    setTimeout(() => $('#txtLookupPartyName').focus(), 300);
}

function btnNewLookupParty_Click() {
    $('#txtLookupPartyName').val('');
    $('#cmbLookupPartyType').val('5');
    $('#cmbLookupAgent').val('0');
    $('#chkLookupIsActive').prop('checked', true);
}

function loadLookupPartiesGrid() {
    $.get('/api/purchase-order/lookup-parties', function(data) {
        const tbody = $('#tblLookupPartiesTbody');
        tbody.empty();
        if (!data || data.length === 0) {
            tbody.html('<tr><td colspan="4" style="text-align: center; padding: 15px;">No lookup parties found.</td></tr>');
            return;
        }
        data.forEach(p => {
            const activeText = p.isActive === 1 || p.isActive === true ? 'Yes' : 'No';
            tbody.append(`
                <tr>
                    <td>${escapeHtml(p.partyTypeName)}</td>
                    <td><strong style="color: #004d40;">${escapeHtml(p.partyName)}</strong></td>
                    <td style="text-align: center;">${activeText}</td>
                    <td>${escapeHtml(p.supplierCustomerName || '')}</td>
                </tr>
            `);
        });
    });
}

function saveLookupParty_Click() {
    const partyName = $('#txtLookupPartyName').val();
    if (!partyName || !partyName.trim()) {
        alert("PartyName Field is Required");
        return;
    }
    const payload = {
        partyName: partyName.trim(),
        partyTypeId: parseInt($('#cmbLookupPartyType').val() || '5'),
        supplierCustomerId: parseInt($('#cmbLookupAgent').val() || '0'),
        isActive: $('#chkLookupIsActive').is(':checked')
    };

    $.ajax({
        url: '/api/purchase-order/save-lookup-party',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(payload),
        success: function(res) {
            if (res && res.success) {
                alert(res.message || "Record Saved Successfully.");
                const newId = res.id;
                const newName = res.partyName;
                $('#txtLookupPartyName').val('');
                $('#modalDefineLookUpParties').modal('hide');
                loadLookupPartiesGrid();
                
                // Auto-refresh Booking Persons and select newly created Booking Person
                loadBookingPersons(function() {
                    if (newId && newId > 0) {
                        $('#hidBookingPersonId').val(newId);
                        $('#txtBookingPersonDisplay').val(newName);
                    }
                });
            } else {
                alert(res ? res.message : "Error saving lookup party.");
            }
        },
        error: function() {
            alert("Error saving lookup party.");
        }
    });
}

function escapeHtml(str) {
    if (!str) return '';
    return String(str).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#039;');
}
