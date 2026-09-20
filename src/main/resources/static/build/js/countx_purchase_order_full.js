/**
 * Purchase Order — Golden Master DitTo Copy Event Controller
 */

let lineItems = [];
let emptyBagItems = [];
let expenseItems = [];
let chargeToProductItems = [];
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

function bindDropdownChangeListeners() {
    $('#cmbSupplier').off('change.sync').on('change.sync', function() {
        const suppId = parseInt($(this).val() || '0');
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
    });

    $('#cmbBrokerAccount').off('change.sync').on('change.sync', function() {
        const brokerId = parseInt($(this).val() || '0');
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
    });

    $('#cmbCommissionAgent').off('change.sync').on('change.sync', function() {
        const agentId = parseInt($(this).val() || '0');
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
    });

    $('#cmbBookingPerson').off('change.sync').on('change.sync', function() {
        const personId = parseInt($(this).val() || '0');
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
    });
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

function loadDropdowns() {
    // Parent Categories
    $.get('/api/purchase-order/parent-categories', function(data) {
        const sel = $('#cmbParentCategory');
        sel.find('option:gt(0)').remove();
        if (data) {
            data.forEach(c => sel.append(`<option value="${c.id}" data-code="${c.id}">${escapeHtml(c.description)}</option>`));
        }
        if ($.fn.select2) sel.trigger('change.select2');
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

/* combsuppname_Leave, :1750 */
function onSupplierChange() { $('#cmbSupplier').trigger('change.sync'); }

/* The desktop has no ValueChanged/Leave handler on the commission agent or the
   booking person; both are read only at save (:3305, :3140). These exist because the
   template binds them, and they keep the hidden ids and the totals in step. */
function onCommissionAgentChange() { $('#cmbCommissionAgent').trigger('change.sync'); }
function onBookingPersonChange()   { $('#cmbBookingPerson').trigger('change.sync'); }

/* CmbBrokeryAccount_Leave -> TotalBrokeryAmountCalculate(), :4536 / :4376 */
function onBrokerAccountChange()   { $('#cmbBrokerAccount').trigger('change.sync'); }

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

        allBookingPersons.forEach(p => {
            const name = p.partyName || p.description || p.name || '';
            const type = p.referencePartyType || p.partyTypeName || 'Booking Person';
            selForm.append(`<option value="${p.id}" data-code="${escapeHtml(type)}">${escapeHtml(name)}</option>`);
            selHist.append(`<option value="${p.id}" data-code="${escapeHtml(type)}">${escapeHtml(name)}</option>`);
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
        const name = p.partyName || p.description || p.name || '';
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
        const pName = p.partyName || p.description || p.name || '';
        const pType = p.referencePartyType || p.partyTypeName || 'Booking Person';
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
function onParentCategoryChange() {
    const parentId = parseInt($('#cmbParentCategory').val() || '0');
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
    const parentId = parseInt($('#cmbParentCategory').val() || '0');
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
    const cropYear = $('#txtCropYear').val() || '';
    const packUomId = parseInt($('#cmbPackUom').val() || '1');
    const packUomCode = $('#cmbPackUom option:selected').text();
    const rateUomId = parseInt($('#cmbRateUom').val() || '1');
    const rateUomCode = $('#cmbRateUom option:selected').text();
    const jobLotId = parseInt($('#cmbJobLot').val() || '0');
    const jobLotName = jobLotId > 0 ? $('#cmbJobLot option:selected').text() : '';
    const loadingCityId = parseInt($('#hidLoadingCityId').val() || '0');
    const loadingCityName = $('#txtLoadingCityDisplay').val() || '';
    const moisture = parseFloat($('#txtMoisture').val() || '0');
    const remarks = $('#txtLineRemarks').val() || '';

    if (itemId <= 0) {
        alert("Please select an Inventory Item.");
        openItemSearchModal();
        return;
    }
    if (qty <= 0) {
        alert("Item Quantity must be greater than zero.");
        $('#txtQty').focus();
        return;
    }

    if (packUomId <= 0 || rateUomId <= 0) {
        alert("Please select a valid Pack UOM and Rate UOM for this Item (loaded from the database).");
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
    $('#cmbPackUom').empty().append('<option value="0">-- Select Item First --</option>');
    $('#cmbRateUom').empty().append('<option value="0">-- Select Item First --</option>');
}

function editDetailRow(idx) {
    const item = lineItems[idx];
    if (!item) return;

    editingLineIdx = idx;
    $('#hidItemId').val(item.itemId);
    $('#txtItemDisplay').val(`${item.itemCode} - ${item.itemName}`);
    $('#txtCropYear').val(item.cropYear);

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
                <td><select class="win-combo" onchange="onExpItemChange(${idx}, this.value)"><option value="0">-- Select --</option>${options}</select></td>
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
function loadChargeToProductDropdowns() {
    $.get('/api/purchase-order/charge-to-product/accounts', function(data) {
        coaAccountsForCharge = data || [];
        renderChargeGrid();
    });
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
        const options = coaAccountsForCharge.map(a => {
            const code = a.AccountCode || a.accountCode || '';
            return `<option value="${a.Id}" data-code="${escapeHtml(code)}"`
                 + ` ${a.Id == row.accountId ? 'selected' : ''}>${escapeHtml(a.AccountTitle)}</option>`;
        }).join('');
        tbody.append(`
            <tr>
                <td><select class="win-combo dtcombo" data-dtcombo="account2" data-dtcombo-caption="Account Title" onchange="onChargeAccountChange(${idx}, this.value)"><option value="0">-- Select --</option>${options}</select></td>
                <td><input type="number" step="0.01" class="win-textbox" style="text-align: right;" value="${row.percentage}" onchange="onChargePercentageChange(${idx}, this.value)"/></td>
                <td><input type="number" step="0.01" class="win-textbox" style="text-align: right;" value="${row.qty}" onchange="onChargeQtyRateChange(${idx}, 'qty', this.value)"/></td>
                <td><input type="number" step="0.01" class="win-textbox" style="text-align: right;" value="${row.rate}" onchange="onChargeQtyRateChange(${idx}, 'rate', this.value)"/></td>
                <td><input type="number" step="0.01" class="win-textbox" style="text-align: right;" value="${(row.amount || 0).toFixed(2)}"/></td>
                <td><input type="text" class="win-textbox" value="${escapeHtml(row.remarks || '')}" onchange="chargeToProductItems[${idx}].remarks = this.value;"/></td>
                <td style="text-align: center;"><button type="button" class="btn btn-danger btn-xs" onclick="removeChargeRow(${idx})">&times;</button></td>
            </tr>
        `);
    });
}

function onChargeAccountChange(idx, val) {
    const id = parseInt(val || '0');
    const found = coaAccountsForCharge.find(a => a.Id == id);
    chargeToProductItems[idx].accountId = id;
    chargeToProductItems[idx].accountTitle = found ? found.AccountTitle : '';
}

function onChargeQtyRateChange(idx, field, val) {
    const num = parseFloat(val || '0') || 0;
    chargeToProductItems[idx][field] = num;
    chargeToProductItems[idx].amount = (chargeToProductItems[idx].qty || 0) * (chargeToProductItems[idx].rate || 0);
    chargeToProductItems[idx].percentage = 0;
    renderChargeGrid();
}

function onChargePercentageChange(idx, val) {
    const pct = parseFloat(val || '0') || 0;
    chargeToProductItems[idx].percentage = pct;
    const totalOrderAmt = calculateGrandTotalLineAmount();
    const amt = (totalOrderAmt / 100.0) * pct;
    chargeToProductItems[idx].amount = amt > 0 ? Math.round(amt) : 0;
    chargeToProductItems[idx].qty = 0;
    chargeToProductItems[idx].rate = 0;
    renderChargeGrid();
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
        deliveryDays: parseInt($('#txtDeliveryDays').val() || '7'),
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

function btnUpdate_Click() {
    if (currentPoMasterId <= 0) {
        alert("No existing Purchase Order loaded for update. Use Load Order first.");
        openLoadOrderModal();
        return;
    }
    btnSave_Click();
}

function btnNew_Click() {
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

function loadSelectedOrder(poId) {
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
                percentage: c.Percentage,
                qty: c.Qty,
                rate: c.Rate,
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

        $('#modalLoadOrder').modal('hide');
    });
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
    const branchId = $('#cmbHistoryBranch').val() || '';
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

    const query = $.param({
        fromDate: fromDate,
        toDate: toDate,
        fromDocNo: fromDocNo,
        toDocNo: toDocNo,
        supplierId: supplierId,
        bookingPersonId: bookingPersonId,
        branchId: branchId,
        dateType: dateType
    });

    $('#tblHistoryTbody').html('<tr><td colspan="15" style="text-align: center; padding: 20px; color: #555;"><i class="fa fa-spinner fa-spin"></i> Loading Purchase Order history...</td></tr>');
    $('#tblHistoryDetailTbody').html('<tr><td colspan="11" style="text-align: center; padding: 15px; color: #777;">Select a history row above to view line item details.</td></tr>');

    $.get(`/api/purchase-order/history?${query}`, function(data) {
        currentHistoryRecords = data || [];
        selectedHistoryRecordIdx = -1;
        renderHistoryMasterGrid();
    }).fail(function() {
        $('#tblHistoryTbody').html('<tr><td colspan="15" style="text-align: center; padding: 20px; color: #d32f2f;">Failed to load history records.</td></tr>');
    });
}

function renderHistoryMasterGrid() {
    const tbody = $('#tblHistoryTbody');
    tbody.empty();

    if (!currentHistoryRecords || currentHistoryRecords.length === 0) {
        tbody.html('<tr><td colspan="15" style="text-align: center; padding: 20px; color: #777;">No matching Purchase Order history records found.</td></tr>');
        $('#lblHistoryRecordCount').text('0 of 0');
        return;
    }

    currentHistoryRecords.forEach((po, idx) => {
        const poId = po.id || po.Id || po.PurchaseOrderMasterId || po.purchaseOrderMasterId || 0;
        const branch = po.branchName || po.BranchName || '';
        const docNo = po.docNo || po.DocNo || '';
        const docDate = po.docDate || po.DocDate ? String(po.docDate || po.DocDate).split('T')[0] : '';
        const party = po.supplierName || po.SupplierName || '';
        const deliveryStart = po.deliveryStartDate || po.DeliveryStartDate ? String(po.deliveryStartDate || po.DeliveryStartDate).split('T')[0] : '';
        const expiry = po.expiryDate || po.ExpiryDate ? String(po.expiryDate || po.ExpiryDate).split('T')[0] : '';
        const remarks = po.remarks || po.Remarks || po.remarksHeader || '';
        const status = po.status || po.Status || 'Active';
        const entryUser = po.entryUser || po.EntryUser || 'Admin';
        const isApproved = po.isApproved === 1 || po.isApproved === true || po.IsApproved === 1;

        const isSelected = (idx === selectedHistoryRecordIdx);
        const rowBg = isSelected ? '#b2dfdb' : '';

        tbody.append(`
            <tr id="histRow_${idx}" onclick="onHistoryRowClick(${idx})" ondblclick="loadSelectedOrderFromHistory(${poId})" style="cursor: pointer; background-color: ${rowBg};">
                <td style="text-align: center;" class="action-col">
                    <button type="button" class="win-btn-action" onclick="event.stopPropagation(); loadSelectedOrderFromHistory(${poId})" title="Edit Order"><i class="fa fa-pencil text-primary"></i> Edit</button>
                    <button type="button" class="win-btn-action" onclick="event.stopPropagation(); loadSelectedOrderFromHistory(${poId})" title="Save As"><i class="fa fa-copy text-info"></i> SaveAs</button>
                    <button type="button" class="win-btn-action" onclick="event.stopPropagation(); btnPrintReport('history_${poId}')" title="Print"><i class="fa fa-print"></i> Print</button>
                </td>
                <td>${escapeHtml(branch)}</td>
                <td><strong style="color: #008080;">${escapeHtml(String(docNo))}</strong></td>
                <td>${escapeHtml(docDate)}</td>
                <td><strong style="color: #004d40;">${escapeHtml(party)}</strong></td>
                <td>${escapeHtml(deliveryStart)}</td>
                <td>${escapeHtml(expiry)}</td>
                <td>${escapeHtml(remarks)}</td>
                <td>${escapeHtml(status)}</td>
                <td>${escapeHtml(entryUser)}</td>
                <td style="text-align: center;">${isApproved ? '<i class="fa fa-check text-success"></i>' : '<i class="fa fa-times text-muted"></i>'}</td>
            </tr>
        `);
    });

    updateHistoryNavDisplay();

    // Auto-select first row if available
    if (currentHistoryRecords.length > 0 && selectedHistoryRecordIdx < 0) {
        onHistoryRowClick(0);
    }
}

function onHistoryRowClick(idx) {
    if (idx < 0 || idx >= currentHistoryRecords.length) return;

    selectedHistoryRecordIdx = idx;
    $('#tblHistoryTbody tr').css('background-color', '');
    $(`#histRow_${idx}`).css('background-color', '#b2dfdb');

    updateHistoryNavDisplay();

    const po = currentHistoryRecords[idx];
    const poId = po.id || po.Id || po.PurchaseOrderMasterId || po.purchaseOrderMasterId || 0;

    loadHistoryDetailGrid(poId);
}

function loadHistoryDetailGrid(poId) {
    const tbody = $('#tblHistoryDetailTbody');
    tbody.html('<tr><td colspan="11" style="text-align: center; padding: 15px; color: #555;"><i class="fa fa-spinner fa-spin"></i> Loading item details...</td></tr>');

    $.get('/api/purchase-order/' + poId, function(po) {
        tbody.empty();
        const items = (po && po.lineItems) ? po.lineItems : [];
        if (items.length === 0) {
            tbody.html('<tr><td colspan="11" style="text-align: center; padding: 15px; color: #777;">No detail line items found for this Purchase Order.</td></tr>');
            return;
        }

        items.forEach((item, i) => {
            const srNo = i + 1;
            const category = item.parentCategoryDesc || item.parentCategory || '';
            const code = item.itemCode || item.code || '';
            const name = item.itemName || item.itemDescription || '';
            const qty = parseFloat(item.qty || 0).toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
            const packQty = parseFloat(item.packQty || 0).toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
            const packUom = item.packUomName || item.packUom || '';
            const rateUom = item.rateUomName || item.rateUom || '';
            const rate = parseFloat(item.rate || 0).toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
            const gross = parseFloat(item.grossAmount || (item.qty * item.rate) || 0).toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });

            tbody.append(`
                <tr>
                    <td style="text-align: center;">${srNo}</td>
                    <td>${escapeHtml(category)}</td>
                    <td>${escapeHtml(code)}</td>
                    <td><strong style="color: #004d40;">${escapeHtml(name)}</strong></td>
                    <td style="text-align: right;">${qty}</td>
                    <td style="text-align: right;">${packQty}</td>
                    <td>${escapeHtml(packUom)}</td>
                    <td>${escapeHtml(rateUom)}</td>
                    <td style="text-align: right;">${rate}</td>
                    <td style="text-align: right;">${gross}</td>
                    <td>${escapeHtml(item.remarks || '')}</td>
                </tr>
            `);
        });
    }).fail(function() {
        tbody.html('<tr><td colspan="11" style="text-align: center; padding: 15px; color: #d32f2f;">Failed to load item details.</td></tr>');
    });
}

function updateHistoryNavDisplay() {
    const total = currentHistoryRecords.length;
    const current = total > 0 ? (selectedHistoryRecordIdx + 1) : 0;
    $('#lblHistoryRecordCount').text(`${current} of ${total}`);
}

function navHistory(action) {
    if (!currentHistoryRecords || currentHistoryRecords.length === 0) return;
    if (action === 'first') onHistoryRowClick(0);
    else if (action === 'prev' && selectedHistoryRecordIdx > 0) onHistoryRowClick(selectedHistoryRecordIdx - 1);
    else if (action === 'next' && selectedHistoryRecordIdx < currentHistoryRecords.length - 1) onHistoryRowClick(selectedHistoryRecordIdx + 1);
    else if (action === 'last') onHistoryRowClick(currentHistoryRecords.length - 1);
}

function loadSelectedOrderFromHistory(poId) {
    loadSelectedOrder(poId);
    switchMainView('form');
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
