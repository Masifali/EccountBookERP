/**
 * Purchase Order — Golden Master DitTo Copy Event Controller
 */

let lineItems = [];
let emptyBagItems = [];
let expenseItems = [];
let chargeToProductItems = [];
let paymentTermsDetailItems = [];

let allSuppliers = [];
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

function initSearchableDropdowns() {
    if ($.fn.select2) {
        $('select').each(function() {
            $(this).select2({
                width: '100%',
                dropdownAutoWidth: true
            });
        });
    }
}

function setWinDefaultDates() {
    const today = new Date();
    const isoDate = today.toISOString().split('T')[0];
    
    $('#txtDocDate').val(isoDate);
    $('#txtDeliveryStartDate').val(isoDate);
    
    calculateExpiryDate();
    calculatePaymentDueDate();
}

function fetchNextDocNo() {
    $.ajax({
        url: '/api/purchase-order/next-doc-no?docType=1052',
        type: 'GET',
        success: function(res) {
            const docNo = res ? res.docNo : null;
            const displayCode = res ? (res.displayCode || res.nextCode || ("PO-" + docNo)) : '';
            const branchNo = res ? (res.branchNo || docNo) : '';
            if (displayCode) {
                $('#txtDocNo').val(displayCode);
                if (docNo) $('#txtDocNo').data('docNo', docNo);
                if (branchNo) $('#txtBranchNo').val(branchNo);
                $('#lblDocNoDisplay').text(displayCode);
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
            data.forEach(c => sel.append(`<option value="${c.id}">${escapeHtml(c.description)}</option>`));
        }
    });

    // Payment Terms
    $.get('/api/purchase-order/payment-terms', function(data) {
        const sel = $('#cmbPaymentTerm');
        sel.find('option:gt(0)').remove();
        if (data && data.length > 0) {
            data.forEach(t => {
                const id = t.id != null ? t.id : (t.Id != null ? t.Id : 0);
                const desc = t.description || t.TermsDescription || t.TermsName || t.Name;
                const dueDays = t.dueDays != null ? t.dueDays : (t.DueDays != null ? t.DueDays : 0);
                if (desc) {
                    sel.append(`<option value="${id}" data-days="${dueDays}">${escapeHtml(desc)}</option>`);
                }
            });
        }
    });

    // Delivery Terms
    $.get('/api/purchase-order/delivery-terms', function(data) {
        const sel = $('#cmbDeliveryTerm');
        sel.find('option:gt(0)').remove();
        if (data && data.length > 0) {
            data.forEach(d => {
                const id = d.id != null ? d.id : d.Id;
                const desc = d.description || d.DeliveryTermDescription || d.DeliveryTerm || d.Description || d.DeliveryTermName || d.TermName;
                if (desc) {
                    sel.append(`<option value="${id}">${escapeHtml(desc)}</option>`);
                }
            });
        }
    });

    // Job Lots
    $.get('/api/purchase-order/job-lots', function(data) {
        const sel = $('#cmbJobLot');
        sel.find('option:gt(0)').remove();
        if (data) {
            data.forEach(j => sel.append(`<option value="${j.id}">${escapeHtml(j.description)}</option>`));
        }
    });

    loadEmptyBagDropdowns();
    loadSupplierExpenseDropdowns();
    loadChargeToProductDropdowns();
    loadPaymentTermsOptions();
    loadUomScheduleList();
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
function bindItemUom(itemId, isNewRow) {
    const packSel = $('#cmbPackUom');
    const rateSel = $('#cmbRateUom');
    packSel.empty();
    rateSel.empty();

    const rows = uomScheduleList.filter(u => parseInt(u.itemId) === parseInt(itemId));
    if (!rows || rows.length === 0) {
        packSel.append('<option value="0" data-eq="1.0">-- No UOM Defined For This Item --</option>');
        rateSel.append('<option value="0" data-eq="1.0">-- No UOM Defined For This Item --</option>');
        return;
    }

    rows.forEach(u => {
        const eq = parseFloat(u.equivalent || 1.0);
        packSel.append(`<option value="${u.id}" data-eq="${eq}">${escapeHtml(u.uomCode)}</option>`);
        rateSel.append(`<option value="${u.id}" data-eq="${eq}">${escapeHtml(u.uomCode)}</option>`);
    });

    if (isNewRow) {
        let baseRateRow = rows.find(u => u.baseRateUom === true || u.baseRateUom === 1);
        if (!baseRateRow) {
            baseRateRow = rows.find(u => parseFloat(u.equivalent) === 40.0);
        }
        if (baseRateRow) {
            rateSel.val(baseRateRow.id);
        }
        let basePackRow = rows.find(u => u.basePackUom === true || u.basePackUom === 1);
        if (basePackRow) {
            packSel.val(basePackRow.id);
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
        // Populate Commission Agent, Broker Ac & Booking Person dropdowns
        const commSel = $('#cmbCommissionAgent');
        const brokerSel = $('#cmbBrokerAc');
        const bookingSel = $('#cmbBookingPerson');
        commSel.find('option:gt(0)').remove();
        brokerSel.find('option:gt(0)').remove();
        bookingSel.find('option:gt(0)').remove();

        allSuppliers.forEach(s => {
            commSel.append(`<option value="${s.id}">${escapeHtml(s.companyName)}</option>`);
            brokerSel.append(`<option value="${s.id}">${escapeHtml(s.companyName)}</option>`);
            bookingSel.append(`<option value="${s.id}">${escapeHtml(s.companyName)}</option>`);
        });

        // Load real Booking Persons from database if available
        $.get('/api/purchase-order/booking-persons', function(bpData) {
            if (bpData && bpData.length > 0) {
                bookingSel.find('option:gt(0)').remove();
                bpData.forEach(b => {
                    const id = b.id != null ? b.id : b.Id;
                    const name = b.partyName || b.ReferencePartyName || b.description || b.Description;
                    if (name) {
                        bookingSel.append(`<option value="${id}">${escapeHtml(name)}</option>`);
                    }
                });
            }
        });
    });

    $.get('/api/purchase-order/items?mode=name', function(data) {
        allItems = data || [];
        const ebSel = $('#cmbEbItem');
        ebSel.find('option:gt(0)').remove();
        allItems.forEach(i => ebSel.append(`<option value="${i.id}">${escapeHtml(i.itemName)}</option>`));
    });

    $.get('/api/purchase-order/cities', function(data) {
        allCities = data || [];
    });
}

/* ============================================================
 * 1. SUPPLIER AUTOCOMPLETE & SELECTION EVENT CHAIN
 * ============================================================ */
function onSupplierSearchModeChange() {
    const mode = $('#radSupCode').is(':checked') ? 'code' : 'name';
    $.get('/api/purchase-order/suppliers?mode=' + mode, function(data) {
        allSuppliers = data || [];
        renderSupplierModalGrid(allSuppliers);
    });
}

function openSupplierSearchModal() {
    const mode = $('#radSupCode').is(':checked') ? 'code' : 'name';
    $.get('/api/purchase-order/suppliers?mode=' + mode, function(data) {
        allSuppliers = data || [];
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

    $('#hidSupplierId').val(supp.id);
    const displayText = $('#radSupCode').is(':checked') 
        ? `${supp.partyCode} - ${supp.companyName}`
        : `${supp.companyName} (${supp.partyCode || 'N/A'})`;
    $('#txtSupplierDisplay').val(displayText);
    $('#modalSupplierSearch').modal('hide');

    // Execute Supplier Selection Event Chain
    onSupplierSelectedEventChain(supp);
}

function onSupplierSelectedEventChain(supp) {
    // 1. Set default payment term if credit
    if ($('#cmbPaymentTerm option[value="2"]').length > 0) {
        $('#cmbPaymentTerm').val('2');
        onPaymentTermChange();
    }
    // 2. Set default Commission Agent if not set
    if ($('#cmbCommissionAgent').val() == '0' && supp.id > 0) {
        $('#cmbCommissionAgent').val(supp.id);
    }
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
    $('#txtPaymentDueDate').val(dt.toISOString().split('T')[0]);
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
    $('#txtExpiryDate').val(dt.toISOString().split('T')[0]);
}

/* ============================================================
 * 4. COMMISSION & BROKERY CALCULATIONS
 * ============================================================ */
function calcCommission() {
    const type = $('#cmbCommType').val();
    const rate = parseFloat($('#txtCommRate').val() || '0');
    const uomEq = parseFloat($('#cmbCommUom option:selected').val() || '1') === 1 ? 40 : 1000;
    
    let commAmt = 0.0;
    const totalLineAmt = calculateGrandTotalLineAmount();
    const totalLineWeight = calculateGrandTotalWeight();

    if (type === 'Flat') {
        commAmt = rate;
    } else if (type === 'Percent') {
        commAmt = (totalLineAmt * rate) / 100.0;
    } else if (type === 'Weight') {
        commAmt = uomEq > 0 ? (totalLineWeight / uomEq) * rate : 0;
    }
    $('#txtCommAmount').val(commAmt.toFixed(2));
}

function calcBrokery() {
    const type = $('#cmbBrokeryType').val();
    const rate = parseFloat($('#txtBrokeryRate').val() || '0');
    const uomEq = parseFloat($('#cmbBrokeryRateUom option:selected').val() || '1') === 1 ? 40 : 1000;
    
    let brokeryAmt = 0.0;
    const totalLineAmt = calculateGrandTotalLineAmount();
    const totalLineWeight = calculateGrandTotalWeight();

    if (type === 'Flat') {
        brokeryAmt = rate;
    } else if (type === 'Percent') {
        brokeryAmt = (totalLineAmt * rate) / 100.0;
    } else if (type === 'Weight') {
        brokeryAmt = uomEq > 0 ? (totalLineWeight / uomEq) * rate : 0;
    }
    $('#txtBrokeryAmount').val(brokeryAmt.toFixed(2));
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
    });
}

function openItemSearchModal() {
    const mode = $('#radItemCode').is(':checked') ? 'code' : 'name';
    const parentId = parseInt($('#cmbParentCategory').val() || '0');
    $.get('/api/purchase-order/items?mode=' + mode + '&parentCategoryId=' + parentId, function(data) {
        allItems = data || [];
        renderItemModalGrid(allItems);
        $('#modalItemSearch').modal('show');
        setTimeout(() => $('#txtModalItemQuery').focus(), 300);
    });
}

function filterItemSearchGrid() {
    const q = $('#txtModalItemQuery').val().toLowerCase();
    const filtered = allItems.filter(i => 
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
function calcLineWeight() {
    const qty = parseFloat($('#txtQty').val() || '0');
    const packEq = parseFloat($('#cmbPackUom option:selected').attr('data-eq') || '1.0');
    const weight = qty * packEq;
    $('#txtWeight').val(weight.toFixed(2));
    calcLineAmount();
}

function calcLineAmount() {
    const weight = parseFloat($('#txtWeight').val() || '0');
    const rate = parseFloat($('#txtRate').val() || '0');
    const rateEq = parseFloat($('#cmbRateUom option:selected').attr('data-eq') || '1.0');

    const amount = (weight > 0 && rateEq > 0) ? (weight / rateEq) * rate : 0.0;
    $('#txtAmount').val(amount.toFixed(2));
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
        itemCode: itemDisplayText.split(' - ')[0] || '',
        itemName: itemDisplayText.includes(' - ') ? itemDisplayText.split(' - ')[1] : itemDisplayText,
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
    $('#cmbPackUom').empty().append('<option value="0" data-eq="1.0">-- Select Item First --</option>');
    $('#cmbRateUom').empty().append('<option value="0" data-eq="1.0">-- Select Item First --</option>');
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
    const itemName = itemId > 0 ? $('#cmbEbItem option:selected').text() : '';
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
    if (!b.packingTypeName && b.packingTypeId) {
        const p = emptyBagPackingTypes.find(x => parseInt(x.Id) === parseInt(b.packingTypeId));
        if (p) b.packingTypeName = p.PackTypeDesc;
    }
    return b;
}

function renderEbGrid() {
    const tbody = $('#tblEbTbody');
    tbody.empty();
    if (!emptyBagItems || emptyBagItems.length === 0) {
        tbody.html('<tr><td colspan="7" style="text-align: center; padding: 15px; color: #777;">No empty bag packing material specified.</td></tr>');
        return;
    }
    emptyBagItems.forEach((b, idx) => {
        resolveEmptyBagDisplayNames(b);
        tbody.append(`
            <tr>
                <td>${idx + 1}</td>
                <td>${escapeHtml(b.typeName || '')}</td>
                <td>${escapeHtml(b.itemName || '')}</td>
                <td>${escapeHtml(b.packingTypeName || '')}</td>
                <td style="text-align: right;">${(b.rate || 0).toFixed(2)}</td>
                <td style="text-align: right;">${(b.weightCut || 0).toFixed(3)}</td>
                <td>
                    <button type="button" class="btn btn-default btn-xs" onclick="editEmptyBagRow(${idx})"><i class="fa fa-pencil"></i></button>
                    <button type="button" class="btn btn-danger btn-xs" onclick="removeEmptyBagRow(${idx})">&times;</button>
                </td>
            </tr>
        `);
    });
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
        const options = coaAccountsForCharge.map(a =>
            `<option value="${a.Id}" ${a.Id == row.accountId ? 'selected' : ''}>${escapeHtml(a.AccountTitle)}</option>`
        ).join('');
        tbody.append(`
            <tr>
                <td><select class="win-combo" onchange="onChargeAccountChange(${idx}, this.value)"><option value="0">-- Select --</option>${options}</select></td>
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
    $.get('/api/purchase-order/payment-terms', function(data) {
        paymentTermsOptions = data || [];
        renderSchedGrid();
    });
}

function addDefaultPaymentRow() {
    const today = $('#txtDocDate').val() || new Date().toISOString().split('T')[0];
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

    let totalPct = 0, totalAmt = 0;
    paymentTermsDetailItems.forEach((row, idx) => {
        totalPct += (row.prcntOfTotal || 0);
        totalAmt += (row.amount || 0);
        const options = paymentTermsOptions.map(t =>
            `<option value="${t.id}" ${t.id == row.paymentTermId ? 'selected' : ''}>${escapeHtml(t.description)}</option>`
        ).join('');
        tbody.append(`
            <tr>
                <td><select class="win-combo" onchange="onSchedTermChange(${idx}, this.value)"><option value="0">-- Select --</option>${options}</select></td>
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
    paymentTermsDetailItems[idx].dueDate = due.toISOString().split('T')[0];
    renderSchedGrid();
}

function onSchedDueDateChange(idx, val) {
    const docDate = getDocDateAsDate();
    const due = new Date(val + 'T00:00:00');
    if (due < docDate) {
        alert("Due Date Can't less Than DocDate");
        paymentTermsDetailItems[idx].dueDate = docDate.toISOString().split('T')[0];
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
function getDocNoFromInput() {
    let stored = $('#txtDocNo').data('docNo');
    if (stored && !isNaN(stored) && parseInt(stored, 10) > 0) return parseInt(stored, 10);
    let val = ($('#txtDocNo').val() || '').trim();
    if (!val) return 0;
    let parts = val.split('-');
    let lastPart = parts[parts.length - 1];
    let num = parseInt(lastPart, 10);
    if (!isNaN(num) && num > 0) return num;
    num = parseInt(val.replace(/\D/g, ''), 10);
    return (!isNaN(num) && num > 0) ? num : 0;
}

function buildPayload() {
    return {
        purchaseOrderMasterId: currentPoMasterId,
        documentTypeId: 1052,
        docNo: getDocNoFromInput(),
        branchNo: $('#txtBranchNo').val() ? parseInt($('#txtBranchNo').val(), 10) : getDocNoFromInput(),
        docDate: $('#txtDocDate').val(),
        supplierId: parseInt($('#hidSupplierId').val() || '0'),
        deliveryStartDate: $('#txtDeliveryStartDate').val(),
        deliveryDays: parseInt($('#txtDeliveryDays').val() || '7'),
        expiryDate: $('#txtExpiryDate').val(),
        paymentTermId: parseInt($('#cmbPaymentTerm').val() || '0'),
        dueDays: parseInt($('#txtDueDays').val() || '0'),
        paymentDueDate: $('#txtPaymentDueDate').val(),
        deliveryTermId: parseInt($('#cmbDeliveryTerm').val() || '0'),
        bookingPersonId: parseInt($('#cmbBookingPerson').val() || '0'),
        commissionAgentId: parseInt($('#cmbCommissionAgent').val() || '0'),
        commType: $('#cmbCommType').val(),
        commRate: parseFloat($('#txtCommRate').val() || '0'),
        commUomId: parseInt($('#cmbCommUom').val() || '1'),
        commAmount: parseFloat($('#txtCommAmount').val() || '0'),
        brokerAccountId: parseInt($('#cmbBrokerAc').val() || '0'),
        brokeryType: $('#cmbBrokeryType').val(),
        brokeryRate: parseFloat($('#txtBrokeryRate').val() || '0'),
        brokeryRateUomId: parseInt($('#cmbBrokeryRateUom').val() || '1'),
        brokeryAmount: parseFloat($('#txtBrokeryAmount').val() || '0'),
        juteBagCut: parseFloat($('#txtJuteBagCut').val() || '0'),
        ppBagCut: parseFloat($('#txtPPBagCut').val() || '0'),
        cashFreight: parseFloat($('#txtCashFreight').val() || '0'),
        creditFreight: parseFloat($('#txtCreditFreight').val() || '0'),
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
    $('#txtRemarksHeader').val('');
    $('#txtBranchNo').val('');

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
        const docNo = po.docNo || 1;
        const displayCode = po.displayCode || po.voucherCode || ("PO-" + docNo);
        const branchNo = po.branchNo || docNo;
        $('#txtDocNo').val(displayCode);
        $('#txtDocNo').data('docNo', docNo);
        $('#txtBranchNo').val(branchNo);
        $('#lblDocNoDisplay').text(displayCode);
        $('#txtDocDate').val(po.docDate);
        $('#cmbDeliveryTerm').val(po.deliveryTermId || po.deliveryTerm || '0');
        $('#cmbBookingPerson').val(po.bookingPersonId || po.bookingPerson || '0');
        $('#hidSupplierId').val(po.supplierId);
        $('#txtSupplierDisplay').val(po.supplierName);
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
 * DEFINE LOOKUP PARTIES (Booking Person / Reference Parties)
 * ============================================================ */
function openDefineLookUpPartiesModal() {
    const agentSel = $('#cmbLookupAgent');
    agentSel.find('option:gt(0)').remove();
    if (allSuppliers && allSuppliers.length > 0) {
        allSuppliers.forEach(s => agentSel.append(`<option value="${s.id}">${escapeHtml(s.companyName)}</option>`));
    }
    btnNewLookupParty_Click();
    loadLookupPartiesGrid();
    $('#modalDefineLookUpParties').modal('show');
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
            tbody.html('<tr><td colspan="4" style="text-align: center; padding: 20px;">No lookup parties found.</td></tr>');
            return;
        }
        data.forEach(p => {
            const partyType = p.partyTypeName || (p.partyTypeId === 5 ? 'Booking Person' : 'Reference Party');
            const partyName = p.partyName || p.ReferencePartyName || '';
            const isActive = p.isActive ? '<i class="fa fa-check text-success"></i>' : '<i class="fa fa-times text-danger"></i>';
            const supplierCust = p.supplierCustomerName || '';
            tbody.append(`
                <tr>
                    <td>${escapeHtml(partyType)}</td>
                    <td><strong>${escapeHtml(partyName)}</strong></td>
                    <td style="text-align: center;">${isActive}</td>
                    <td>${escapeHtml(supplierCust)}</td>
                </tr>
            `);
        });
    });
}

function saveLookupParty_Click() {
    const partyName = ($('#txtLookupPartyName').val() || '').trim();
    if (!partyName) {
        alert("PartyName Field is Required");
        $('#txtLookupPartyName').focus();
        return;
    }

    const payload = {
        partyName: partyName,
        partyTypeId: parseInt($('#cmbLookupPartyType').val() || '5', 10),
        supplierCustomerId: parseInt($('#cmbLookupAgent').val() || '0', 10),
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
                btnNewLookupParty_Click();
                loadLookupPartiesGrid();
                $.get('/api/purchase-order/booking-persons', function(data) {
                    const bookingSel = $('#cmbBookingPerson');
                    bookingSel.find('option:gt(0)').remove();
                    if (data && data.length > 0) {
                        data.forEach(b => {
                            const id = b.id != null ? b.id : b.Id;
                            const name = b.partyName || b.ReferencePartyName || b.description || b.Description;
                            if (name) {
                                bookingSel.append(`<option value="${id}">${escapeHtml(name)}</option>`);
                            }
                        });
                        if (res.id) bookingSel.val(res.id);
                    }
                });
            } else {
                alert("Error saving party: " + (res ? res.message : "Unknown error"));
            }
        },
        error: function() {
            alert("Error saving lookup party.");
        }
    });
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

function escapeHtml(str) {
    if (!str) return '';
    return String(str).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#039;');
}
