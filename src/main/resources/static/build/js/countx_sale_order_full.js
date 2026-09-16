/**
 * Sale Order Master JS Module — WinForms Golden Master Replica
 * Handles dropdown population, item filtering, detail line calculations,
 * Customer Expense tab, Payment Detail tab, Stock Reports, Toolbar actions,
 * and History search.
 */

var masterLookupsData = {};
var currentLineItems = [];
var currentExpenseItems = [];
var currentPaymentSchedules = [];

$(document).ready(function () {
    var today = new Date().toISOString().split('T')[0];
    $('#txtDocDate').val(today);
    $('#txtDueDate').val(today);
    $('#txtDeliveryStartDate').val(today);
    $('#histFromDate').val(today);
    $('#histToDate').val(today);

    loadMasterLookups();
    setupEventListeners();
});

// =========================================================
// 1. MASTER LOOKUPS & DROPDOWN BINDING
// =========================================================
function loadMasterLookups() {
    $.get('/sale/sale-order/api/master-lookups', function (data) {
        if (!data) return;
        masterLookupsData = data;

        // Location Type - usp_getLocationType (Id, Location). Real DB lookup, not hardcoded.
        bindCombo('#cmbLocationType', data.locationTypes, 'Id', 'Location');

        // Order Categories - Sp_InvOrderCategory_GetAllMethod (Id, OrderCategoryName)
        bindCombo('#cmbOrderCategory', data.orderCategories, 'Id', 'OrderCategoryName');

        // Category-I & Category-II - Sp_InvLookup_GetAllMethod (Id, LookupName)
        bindCombo('#cmbCategoryI', data.categoriesI, 'Id', 'LookupName', '...Select Category-I...');
        bindCombo('#cmbCategoryII', data.categoriesII, 'Id', 'LookupName', '...Select Category-II...');

        // Customers / Parties - USP_GetVendorsAndCustomersWithCityName or Sp_SupplierCustomer_GetAllMethod (Id, CompanyName, PartyCode)
        bindCustomerCombo(data.customers);

        // Booking Persons - Sp_ReferenceParties_GetAllMethod (Id, ReferencePartyName)
        bindCombo('#cmbBookingPerson', data.bookingPersons, 'Id', 'ReferencePartyName', '...Select Person...');

        // Payment Terms - Sp_InvDueTerms_GetAllMethod (Id, TermsDescription)
        bindCombo('#cmbPaymentTerm', data.paymentTerms, 'Id', 'TermsDescription');
        // Delivery Terms - hardcoded in the desktop itself: Load / Ponch (Id, Description)
        bindCombo('#cmbDeliveryTerm', data.deliveryTerms, 'Id', 'Description');

        // Order Status - hardcoded in the desktop itself (Id, Description)
        bindCombo('#cmbOrderStatus', data.orderStatuses, 'Id', 'Description');

        // Commission Agents & Salesmen - SAME customer/supplier list as the Party combo
        // (desktop's CompanyNameBind binds CmbCustomerName, combsalesman and
        // CmbOtherCommissionAgent against the identical dtsuppcus DataTable).
        bindCombo('#cmbSalesMan', data.salesMen, 'Id', 'CompanyName', '...Select Any Value...');
        bindCombo('#cmbOtherSalesMan', data.otherCommissionAgents, 'Id', 'CompanyName', '...Select Any Value...');

        // Commission Types (hardcoded: Flat/Percent/Comm Weight) & Commission UOMs (SpStaticColumnNames)
        bindCombo('#cmbCommType', data.commissionTypes, 'Id', 'Description');
        bindCombo('#cmbOtherCommType', data.commissionTypes, 'Id', 'Description');
        bindCombo('#cmbCommUom', data.commissionUoms, 'Id', 'type');
        bindCombo('#cmbOtherCommUom', data.commissionUoms, 'Id', 'type');

        // Detail Line Items - USP_Item_AllItemsWithModal (Id, ItemName, ItemCode)
        bindItemCombo(data.items);

        // Crop Years - Sp_InvCropYear_GetAllMethod (Id, CropYear)
        if (data.cropYears && data.cropYears.length > 0) {
            $('#lineCropYear').val(data.cropYears[0].CropYear);
        }

        // Job/Lots - USP_GetJobLotsAllocatedToBranch (Id, JobLotDescription), scoped to the current branch
        bindCombo('#lineJobLot', data.jobLots, 'Id', 'JobLotDescription');

        // Packing Types - Sp_InvPackingType_GetAllMethod (Id, PackTypeDesc)
        bindCombo('#linePackType', data.packingTypes, 'Id', 'PackTypeDesc');

        // Cities - SP_City_GetAllMethod (Id, CityName) & Warehouses - USP_GetWarehousesAllocatedToBranch (Id, WareHouseName), scoped to the current branch
        bindCombo('#lineCityArea', data.cities, 'Id', 'CityName');
        bindCombo('#lineWarehouse', data.warehouses, 'Id', 'WareHouseName');

        // Payment Detail dropdown - Sp_InvDueTerms_GetAllMethod (Id, TermsDescription). Real, correct source.
        bindCombo('#payTerm', data.paymentTerms, 'Id', 'TermsDescription', '-- Select Term --');

        // Customer Expense "Other Item" dropdown - KNOWN WRONG, BLOCKED (see SALE-ORDER-PROGRESS.md Pass 3).
        // Desktop's real source is InventoryItemsOther.GetAll() -> Sp_InventoryItemsOther_GetAllMethod
        // (Id, OtherItemName) - a completely separate master table from the main Item list. The Java
        // backend (SaleOrderService/SaleCommonService) could not be reached this session (deeply-nested
        // file staging blocked - see progress doc), so no /sale/sale-order/api/other-items endpoint exists
        // yet and this still falls back to the main Item list as a stopgap. Do not treat this dropdown as
        // fixed; swap this line for a real other-items endpoint the moment backend access is restored.
        bindCombo('#expItem', data.items, 'Id', 'ItemName', '-- Select Item --');

        // History Customer Search Combo
        bindCombo('#histCustomerCombo', data.customers, 'Id', 'CompanyName', '...Select Customer...');
    });
}

function bindCombo(selector, items, valueAttr, textAttr, defaultText) {
    var $el = $(selector).empty();
    if (defaultText) {
        $el.append($('<option>', { value: '', text: defaultText }));
    }
    if (items && items.length > 0) {
        items.forEach(function (item) {
            $el.append($('<option>', {
                value: item[valueAttr],
                text: item[textAttr] || item[valueAttr]
            }));
        });
    }
}

function bindCustomerCombo(customers) {
    var $el = $('#cmbCustomer').empty();
    $el.append('<option value="">...Select Any Value...</option>');
    if (!customers) return;
    var searchByCode = $('#radPartyCode').is(':checked');
    customers.forEach(function (c) {
        var label = searchByCode ? ((c.PartyCode || '') + ' - ' + c.CompanyName) : c.CompanyName;
        $el.append($('<option>', { value: c.Id, text: label }));
    });
}

function bindItemCombo(items) {
    var $el = $('#lineItem').empty();
    $el.append('<option value="">-- Select Item --</option>');
    if (!items) return;

    var searchByCode = $('#radCode').is(':checked');

    items.forEach(function (i) {
        var label = searchByCode ? (i.ItemCode + ' - ' + i.ItemName) : i.ItemName;
        $el.append($('<option>', { value: i.Id, text: label, 'data-code': i.ItemCode, 'data-name': i.ItemName }));
    });
}

// =========================================================
// 2. EVENT LISTENERS & DEPENDENT CALCULATIONS
// =========================================================
function setupEventListeners() {
    // Party Code vs Name radio toggle
    $('input[name="radParty"]').change(function () {
        if (masterLookupsData.customers) bindCustomerCombo(masterLookupsData.customers);
    });

    // Item Code vs Name radio toggle
    $('input[name="radItemSelect"], input[name="radItemFilter"]').change(function () {
        if (masterLookupsData.items) bindItemCombo(masterLookupsData.items);
    });

    // Customer Selection -> Fetch Balance Summary
    $('#cmbCustomer').change(function () {
        var custId = $(this).val();
        if (!custId) {
            resetCustomerBalance();
            return;
        }
        $.get('/sale/sale-order/api/customer-balance/' + custId, function (data) {
            if (data) {
                $('#txtPartyGlAmount').val(parseFloat(data.partyGlAmount || 0).toFixed(2));
                $('#txtOutstandingOrder').val(parseFloat(data.outstandingOrders || 0).toFixed(2));
                $('#txtPartyLimit').val(parseFloat(data.partyLimit || 0).toFixed(2));
                recalcNetRecoverable();
            }
        });
    });

    // Item Selection -> Fetch Rate UOM Schedule (Sp_UOMSchedule_GetAllMethod @Activity='ReadByItemID').
    // BaseRateUom=true marks the row the desktop auto-selects by default (CommonServices.GetBaseRateUomId).
    $('#lineItem').change(function () {
        var itemId = $(this).val();
        if (!itemId) return;
        $.get('/sale/sale-order/api/item-uoms/' + itemId, function (uoms) {
            var $uom = $('#lineRateUom').empty();
            if (uoms && uoms.length > 0) {
                var baseUomCode = null;
                uoms.forEach(function (u) {
                    $uom.append($('<option>', { value: u.Id, text: u.UOMCode }));
                    if (u.BaseRateUom === true || u.BaseRateUom === 1) {
                        baseUomCode = u.Id;
                    }
                });
                if (baseUomCode !== null) {
                    $uom.val(baseUomCode);
                }
            }
        });
    });

    // Due Days -> Due Date Calculation
    $('#txtDueDays').on('input', function () {
        var days = parseInt($(this).val()) || 0;
        var docDateVal = $('#txtDocDate').val();
        if (docDateVal) {
            var d = new Date(docDateVal);
            d.setDate(d.getDate() + days);
            $('#txtDueDate').val(d.toISOString().split('T')[0]);
        }
    });

    // Delivery Days -> Delivery Start Date Calculation
    $('#txtDeliveryDays').on('input', function () {
        var days = parseInt($(this).val()) || 0;
        var dDateVal = $('#txtDocDate').val();
        if (dDateVal) {
            var d = new Date(dDateVal);
            d.setDate(d.getDate() + days);
            $('#txtDeliveryStartDate').val(d.toISOString().split('T')[0]);
        }
    });

    // -------- Customer Expense entry bar: Qty/Rate -> Amount, Amount(manual) resets Qty/Rate --------
    // Ports desktop grdInvExp_CellUpdated verbatim: editing Qty or Rate recomputes Amount = round(Qty*Rate);
    // editing Amount directly makes Amount authoritative and zeroes Qty/Rate.
    $('#expQty, #expRate').on('input', function () {
        var qty = parseFloat($('#expQty').val()) || 0;
        var rate = parseFloat($('#expRate').val()) || 0;
        $('#expAmount').val((qty * rate).toFixed(2));
    });
    $('#expAmount').on('input', function () {
        $('#expQty').val('0');
        $('#expRate').val('0');
    });

    // -------- Payment Detail entry bar: two-way %OfTotal <-> Amount, DueDays <-> DueDate --------
    // Ports desktop grdPaymentDetail_CellUpdated verbatim (see SALE-ORDER-PROGRESS.md Pass 3).
    $('#payPercent').on('input', function () {
        var pct = parseFloat($(this).val()) || 0;
        if (pct > 100) { pct = 100; $(this).val(100); }
        var total = getDetailOrderTotal();
        var amt = Math.round((pct * total / 100) * 10000) / 10000;
        if (total > 0 && amt > total) { amt = total; }
        $('#payAmount').val(amt.toFixed(2));
    });
    $('#payAmount').on('input', function () {
        var total = getDetailOrderTotal();
        var amt = parseFloat($(this).val()) || 0;
        if (total > 0 && amt > total) { amt = total; $(this).val(amt.toFixed(2)); }
        if (total > 0) {
            var pct = Math.round((amt / total * 100) * 10000) / 10000;
            $('#payPercent').val(pct);
        }
    });
    $('#payDueDays').on('input', function () {
        var days = parseInt($(this).val()) || 0;
        var docDateVal = $('#txtDocDate').val();
        if (docDateVal) {
            var d = new Date(docDateVal);
            d.setDate(d.getDate() + days);
            $('#payDueDate').val(d.toISOString().split('T')[0]);
        }
    });
    $('#payDueDate').on('change', function () {
        var docDateVal = $('#txtDocDate').val();
        var dueDateVal = $(this).val();
        if (!docDateVal || !dueDateVal) return;
        var docDate = new Date(docDateVal);
        var dueDate = new Date(dueDateVal);
        if (dueDate < docDate) {
            alert("Due Date Can't less Than DocDate");
            $(this).val(docDateVal);
            $('#payDueDays').val(0);
            return;
        }
        var days = Math.round((dueDate - docDate) / (1000 * 60 * 60 * 24));
        $('#payDueDays').val(days);
    });
}

// Detail tab order total (desktop: GridEX_Helper.GetColumnSum(grd, "Amount")) - the base that
// Payment Detail's %OfTotal / Amount two-way calc and the Save-time reconciliation both use.
function getDetailOrderTotal() {
    var total = 0;
    currentLineItems.forEach(function (item) { total += (item.amount || 0); });
    return total;
}

// Desktop PaymentAmountReCalculate(): when exactly one Payment Detail row exists and the Detail
// tab total changes, that row's Amount is kept in sync from its %OfTotal against the fresh total.
function paymentAmountReCalculate() {
    var total = getDetailOrderTotal();
    if (total > 0 && currentPaymentSchedules.length === 1) {
        var row = currentPaymentSchedules[0];
        row.amount = Math.round((row.percentOfTotal * total / 100) * 10000) / 10000;
        renderPaymentGrid();
    }
}

function resetCustomerBalance() {
    $('#txtPartyGlAmount').val('0.00');
    $('#txtOutstandingOrder').val('0.00');
    $('#txtCurrentOrder').val('0.00');
    $('#txtPartyLimit').val('0.00');
    $('#txtNetRecoverable').val('0.00');
}

function recalcNetRecoverable() {
    var gl = parseFloat($('#txtPartyGlAmount').val()) || 0;
    var out = parseFloat($('#txtOutstandingOrder').val()) || 0;
    var cur = parseFloat($('#txtCurrentOrder').val()) || 0;
    var lim = parseFloat($('#txtPartyLimit').val()) || 0;
    var net = gl + out + cur - lim;
    $('#txtNetRecoverable').val(net.toFixed(2));
}

// =========================================================
// 3. DETAIL LINE ITEM ENTRY & GRID
// =========================================================
function calcLine() {
    var qty = parseFloat($('#lineQty').val()) || 0;
    var rate = parseFloat($('#lineRate').val()) || 0;
    var weight = qty * 40; // Standard 40KG equivalent
    $('#lineWeight').val(weight.toFixed(2));
    $('#lineAmount').val((qty * rate).toFixed(2));
}

function btnAddRow_Click() {
    var itemId = $('#lineItem').val();
    var itemName = $('#lineItem option:selected').text();
    var cropYear = $('#lineCropYear').val() || '2025-26';
    var jobLot = $('#lineJobLot option:selected').text() || 'General';
    var packType = $('#linePackType option:selected').text() || 'PP Bags';
    var packSize = $('#linePackSize').val() || '40 KG';
    var qty = parseFloat($('#lineQty').val()) || 0;
    var weight = parseFloat($('#lineWeight').val()) || 0;
    var rateUom = $('#lineRateUom').val() || '40KG';
    var rate = parseFloat($('#lineRate').val()) || 0;
    var amount = parseFloat($('#lineAmount').val()) || 0;
    var bagPrice = parseFloat($('#lineBagPrice').val()) || 0;
    var wtCut = parseFloat($('#lineWtCut').val()) || 0;
    var cityId = $('#lineCityArea').val() || '';
    var cityArea = $('#lineCityArea option:selected').text() || '';
    var warehouseId = $('#lineWarehouse').val() || '';
    var warehouse = $('#lineWarehouse option:selected').text() || '';
    var labSample = $('#lineLabSample').val() || '';
    var remarks = $('#lineRemarks').val() || '';
    var commOnSale = $('#lineCommOnSale').is(':checked');

    if (!itemId || qty <= 0) {
        alert('Please select an Item and enter Quantity.');
        return;
    }

    var jobLotIdVal = $('#lineJobLot').val() || '';
    var packTypeIdVal = $('#linePackType').val() || '';

    // Real desktop per-row required fields (SaleOrder.cs Insert(), see SALE-ORDER-PROGRESS.md
    // Pass 3): Crop Year, Job Lot, Pack Type, Pack Uom, Net Weight are all mandatory per row.
    if (!cropYear) { alert('Crop Year not found in detail grid.'); return; }
    if (!jobLotIdVal) { alert('Job Lot not found in detail grid.'); return; }
    if (!packTypeIdVal) { alert('Pack Type not found in detail grid.'); return; }
    if (weight <= 0) { alert('New Weight not found in detail grid.'); return; }
    if (rate > 0 && amount <= 0) { alert('Amount not found in detail grid.'); return; }

    var itemObj = {
        itemId: itemId,
        itemName: itemName,
        itemCode: 'ITM-' + itemId,
        cropYear: cropYear,
        jobLot: jobLot,
        jobLotId: jobLotIdVal,
        packingType: packType,
        packingTypeId: packTypeIdVal,
        packSize: packSize,
        quantity: qty,
        weight: weight,
        rateUom: rateUom,
        rate: rate,
        amount: amount,
        bagPrice: bagPrice,
        weightCut: wtCut,
        cityId: cityId,
        cityArea: cityArea,
        warehouseId: warehouseId,
        warehouse: warehouse,
        labSample: labSample,
        remarks: remarks,
        commOnSale: commOnSale
    };

    currentLineItems.push(itemObj);
    renderDetailGrid();
    clearLineEntry();
}

function renderDetailGrid() {
    var tbody = $('#tblDetail tbody').empty();
    if (currentLineItems.length === 0) {
        tbody.append('<tr><td colspan="19" class="text-center text-muted" style="padding: 12px;">No order line items added yet. Record: 0 of 0</td></tr>');
        recalcTotals();
        return;
    }

    currentLineItems.forEach(function (item, index) {
        var tr = `<tr>
            <td class="text-center"><button class="btn btn-sm btn-danger p-0 px-1" onclick="removeLineItem(${index})">&times;</button></td>
            <td>${item.itemCode}</td>
            <td>${item.itemName}</td>
            <td>${item.cropYear}</td>
            <td>${item.jobLot}</td>
            <td>${item.packingType}</td>
            <td>${item.packSize}</td>
            <td class="text-end qty-val">${item.quantity.toFixed(2)}</td>
            <td class="text-end wt-val">${item.weight.toFixed(2)}</td>
            <td>${item.rateUom}</td>
            <td class="text-end">${item.rate.toFixed(2)}</td>
            <td class="text-end amt-val">${item.amount.toFixed(2)}</td>
            <td class="text-end">${item.bagPrice.toFixed(2)}</td>
            <td class="text-end">${item.weightCut.toFixed(2)}</td>
            <td>${item.cityArea || ''}</td>
            <td>${item.warehouse || ''}</td>
            <td>${item.labSample || ''}</td>
            <td>${item.remarks || ''}</td>
            <td class="text-center">${item.commOnSale ? '<i class="fa fa-check text-success"></i>' : ''}</td>
        </tr>`;
        tbody.append(tr);
    });

    recalcTotals();
}

function removeLineItem(index) {
    currentLineItems.splice(index, 1);
    renderDetailGrid();
}

function clearLineEntry() {
    $('#lineItem').val('');
    $('#lineQty').val('');
    $('#lineWeight').val('');
    $('#lineRate').val('');
    $('#lineAmount').val('0.00');
    $('#lineBagPrice').val('');
    $('#lineWtCut').val('');
    $('#lineLabSample').val('');
    $('#lineRemarks').val('');
    $('#lineCommOnSale').prop('checked', false);
}

function recalcTotals() {
    var totalQty = 0, totalAmt = 0, totalWt = 0;
    currentLineItems.forEach(function (item) {
        totalQty += item.quantity;
        totalWt += item.weight;
        totalAmt += item.amount;
    });
    $('#txtOrderQty').val(totalQty.toFixed(2));
    $('#txtOrderWeight').val(totalWt.toFixed(2));
    $('#txtCurrentOrder').val(totalAmt.toFixed(2));
    recalcNetRecoverable();
    paymentAmountReCalculate();
}

// =========================================================
// 4. CUSTOMER EXPENSE TAB LOGIC
// =========================================================
function btnAddExpenseRow_Click() {
    var itemId = $('#expItem').val();
    var itemName = $('#expItem option:selected').text();
    var qty = parseFloat($('#expQty').val()) || 0;
    var rate = parseFloat($('#expRate').val()) || 0;
    var amt = parseFloat($('#expAmount').val()) || 0;
    var remarks = $('#expRemarks').val() || '';

    if (!itemId || amt <= 0) {
        alert('Select Item and enter Qty/Rate or Amount for Expense.');
        return;
    }

    // Desktop auto-fills Remarks when left blank: "OtherItemName : {name}  {qty}  @{rate}"
    if (!remarks || remarks === '0') {
        remarks = 'OtherItemName : ' + itemName + '  ' + qty + '  @' + rate;
    }

    currentExpenseItems.push({
        itemId: itemId,
        itemName: itemName,
        quantity: qty,
        rate: rate,
        amount: amt,
        remarks: remarks
    });

    renderExpenseGrid();
    $('#expItem').val(''); $('#expQty').val(''); $('#expRate').val(''); $('#expAmount').val('0.00'); $('#expRemarks').val('');
}

function renderExpenseGrid() {
    var tbody = $('#tblCustomerExpense tbody').empty();
    if (currentExpenseItems.length === 0) {
        tbody.append('<tr><td colspan="6" class="text-center text-muted" style="padding: 10px;">No customer expenses added.</td></tr>');
        return;
    }
    currentExpenseItems.forEach(function (item, index) {
        tbody.append(`<tr>
            <td class="text-center"><button class="btn btn-sm btn-danger p-0 px-1" onclick="removeExpenseItem(${index})">&times;</button></td>
            <td>${item.itemName}</td>
            <td class="text-end">${item.quantity.toFixed(2)}</td>
            <td class="text-end">${item.rate.toFixed(2)}</td>
            <td class="text-end">${item.amount.toFixed(2)}</td>
            <td>${item.remarks}</td>
        </tr>`);
    });
}

function removeExpenseItem(index) {
    currentExpenseItems.splice(index, 1);
    renderExpenseGrid();
}

// =========================================================
// 5. PAYMENT DETAIL TAB LOGIC
// =========================================================
function btnAddPaymentRow_Click() {
    var termId = $('#payTerm').val();
    var termName = $('#payTerm option:selected').text();
    var dueDays = parseInt($('#payDueDays').val()) || 0;
    var dueDate = $('#payDueDate').val();
    var percent = parseFloat($('#payPercent').val()) || 0;
    var amt = parseFloat($('#payAmount').val()) || 0;
    var remarks = $('#payRemarks').val() || '';

    if (!termId) {
        alert('Select Payment Term.');
        return;
    }

    currentPaymentSchedules.push({
        paymentTermId: termId,
        paymentTerm: termName,
        dueDays: dueDays,
        dueDate: dueDate,
        percentOfTotal: percent,
        amount: amt,
        remarks: remarks
    });

    renderPaymentGrid();
    $('#payTerm').val(''); $('#payDueDays').val('0'); $('#payDueDate').val(''); $('#payPercent').val('0'); $('#payAmount').val('0.00'); $('#payRemarks').val('');
}

// Real desktop save-time validation + fallback (Sale Order Insert(), see SALE-ORDER-PROGRESS.md
// Pass 3). Returns {ok:true} or {ok:false, message} - never silently drops a validation.
// If the grid was never touched (sum of Amount == 0), synthesizes a single implicit 100% row
// from the header's own Payment Term/Due Days, exactly like desktop does at save time - the
// Payment Detail tab is never a hard requirement to visit before saving.
function buildPaymentTermsForSave() {
    var detailTotal = getDetailOrderTotal();
    var sumAmount = 0, sumPercent = 0;
    currentPaymentSchedules.forEach(function (p) { sumAmount += (p.amount || 0); });

    var rows;
    if (sumAmount > 0) {
        for (var i = 0; i < currentPaymentSchedules.length; i++) {
            var p = currentPaymentSchedules[i];
            if (!p.paymentTermId) {
                return { ok: false, message: 'Payment Term Required in row#' + (i + 1) };
            }
            // Desktop hardcodes PaymentTermId==2 as the "Credit" term requiring Due Days.
            if (parseInt(p.paymentTermId) === 2 && (!p.dueDays || p.dueDays <= 0)) {
                return { ok: false, message: 'Due Days Required In case Of Credit row in row#' + (i + 1) };
            }
            sumPercent += (p.percentOfTotal || 0);
        }
        rows = currentPaymentSchedules;
    } else {
        var termId = parseInt($('#cmbPaymentTerm').val()) || 0;
        var dueDays = parseInt($('#txtDueDays').val()) || 0;
        var termText = $('#cmbPaymentTerm option:selected').text() || '';
        rows = [{
            paymentTermId: termId,
            paymentTerm: termText,
            dueDays: dueDays,
            dueDate: $('#txtDueDate').val(),
            percentOfTotal: 100,
            amount: detailTotal,
            remarks: ''
        }];
        sumAmount = detailTotal;
        sumPercent = 100;
    }

    if (Math.abs(sumAmount - detailTotal) > 0.3) {
        return { ok: false, message: 'Payment Detail Amount:' + sumAmount.toFixed(2) + ' Not Equal to Total Amount:' + detailTotal.toFixed(2) };
    }
    if (Math.abs(100 - sumPercent) > 0.01) {
        return { ok: false, message: 'Payment Detail Total% not near to 100' };
    }
    return { ok: true, rows: rows };
}

function renderPaymentGrid() {
    var tbody = $('#tblPaymentDetail tbody').empty();
    if (currentPaymentSchedules.length === 0) {
        tbody.append('<tr><td colspan="7" class="text-center text-muted" style="padding: 10px;">No payment schedules added.</td></tr>');
        return;
    }
    currentPaymentSchedules.forEach(function (p, index) {
        tbody.append(`<tr>
            <td class="text-center"><button class="btn btn-sm btn-danger p-0 px-1" onclick="removePaymentSchedule(${index})">&times;</button></td>
            <td>${p.paymentTerm}</td>
            <td class="text-center">${p.dueDays}</td>
            <td>${p.dueDate || ''}</td>
            <td class="text-end">${p.percentOfTotal.toFixed(2)}%</td>
            <td class="text-end">${p.amount.toFixed(2)}</td>
            <td>${p.remarks}</td>
        </tr>`);
    });
}

function removePaymentSchedule(index) {
    currentPaymentSchedules.splice(index, 1);
    renderPaymentGrid();
}

// =========================================================
// 6. TABS & FOOTER NAVIGATION
// =========================================================
function switchTab(tabId) {
    $('.po-tab-pane').hide();
    $('#' + tabId).show();
    $('.win-nav-tabs li').removeClass('active');
    $(event.target).closest('li').addClass('active');
}

// Programmatic tab switch (no click event available) - used when Save-time validation needs to
// focus a tab, matching desktop's tabControl2.SelectedIndex = 2 / ((Control)grdPaymentDetail).Focus().
var TAB_IDS = ['tabDetail', 'tabCustomerExpense', 'tabPaymentDetail'];
function switchTabById(tabId) {
    $('.po-tab-pane').hide();
    $('#' + tabId).show();
    $('.win-nav-tabs li').removeClass('active');
    var idx = TAB_IDS.indexOf(tabId);
    if (idx >= 0) { $('#poTabControl li').eq(idx).addClass('active'); }
}

function showFormTab() {
    $('#btnFooterForm').addClass('active');
    $('#btnFooterHistory').removeClass('active');
    $('#viewFormContainer').show();
    $('#viewHistoryContainer').hide();
}

function showHistoryTab() {
    $('#btnFooterHistory').addClass('active');
    $('#btnFooterForm').removeClass('active');
    $('#viewFormContainer').hide();
    $('#viewHistoryContainer').show();
    loadHistoryData();
}

// =========================================================
// 7. STOCK REPORT MODALS
// =========================================================
function openStockReport() {
    var itemId = $('#lineItem').val() || '';
    $.get('/sale/sale-order/api/stock-report?itemId=' + itemId, function (data) {
        var tbody = $('#tblStockReportModal tbody').empty();
        if (!data || data.length === 0) {
            tbody.append('<tr><td colspan="4" class="text-center text-muted" style="padding:15px;">No stock records found.</td></tr>');
        } else {
            data.forEach(function (row) {
                tbody.append(`<tr>
                    <td>${row.itemCode || ''}</td>
                    <td>${row.itemName || ''}</td>
                    <td class="text-end font-weight-bold">${parseFloat(row.stockQty || 0).toFixed(2)}</td>
                    <td class="text-end">${parseFloat(row.stockWeight || 0).toFixed(2)}</td>
                </tr>`);
            });
        }
        $('#modalStockReport').modal('show');
    });
}

function openStockReportWithValue() {
    var itemId = $('#lineItem').val() || '';
    $.get('/sale/sale-order/api/stock-report-with-value?itemId=' + itemId, function (data) {
        var tbody = $('#tblStockReportWithValueModal tbody').empty();
        if (!data || data.length === 0) {
            tbody.append('<tr><td colspan="6" class="text-center text-muted" style="padding:15px;">No stock valuation records found.</td></tr>');
        } else {
            data.forEach(function (row) {
                tbody.append(`<tr>
                    <td>${row.itemCode || ''}</td>
                    <td>${row.itemName || ''}</td>
                    <td class="text-end">${parseFloat(row.stockQty || 0).toFixed(2)}</td>
                    <td class="text-end">${parseFloat(row.stockWeight || 0).toFixed(2)}</td>
                    <td class="text-end">${parseFloat(row.avgRate || 0).toFixed(2)}</td>
                    <td class="text-end font-weight-bold">${parseFloat(row.totalValue || 0).toFixed(2)}</td>
                </tr>`);
            });
        }
        $('#modalStockReportWithValue').modal('show');
    });
}

function openPreBookingModal() {
    $.get('/sale/sale-order/api/pre-booking-orders', function (data) {
        var tbody = $('#tblPreBookingModal tbody').empty();
        if (!data || data.length === 0) {
            tbody.append('<tr><td colspan="5" class="text-center text-muted" style="padding:15px;">No pre-booking orders available.</td></tr>');
        } else {
            data.forEach(function (row) {
                tbody.append(`<tr>
                    <td>BO-${row.id}</td>
                    <td>${row.date || ''}</td>
                    <td>${row.customerName || ''}</td>
                    <td class="text-end">${(parseFloat(row.totalQty) || 0).toFixed(2)}</td>
                    <td class="text-center"><button class="btn btn-sm btn-primary" onclick="loadPreBooking(${row.id})">Load</button></td>
                </tr>`);
            });
        }
        $('#modalPreBooking').modal('show');
    });
}

function openAttachmentsModal() {
    $('#modalAttachments').modal('show');
}

// =========================================================
// 8. SAVE & HISTORY SEARCH
// =========================================================
function btnNew_Click() {
    if (confirm('Create new Sale Order? Unsaved changes will be lost.')) {
        location.reload();
    }
}

function btnSave_Click() {
    var custId = $('#cmbCustomer').val();
    if (!custId) {
        alert('Please select a Customer / Party.');
        return;
    }
    if (currentLineItems.length === 0) {
        alert('At least one line item must be added in the Detail tab.');
        return;
    }

    // Real desktop Payment Detail validation/fallback (Sp_SaleOrder_Insert flow) - see
    // SALE-ORDER-PROGRESS.md Pass 3. Must run before submit; a failure focuses that tab.
    var paymentResult = buildPaymentTermsForSave();
    if (!paymentResult.ok) {
        alert(paymentResult.message);
        switchTabById('tabPaymentDetail');
        return;
    }

    // Real desktop save-time filter for Customer Expense rows: only ItemId!=0 && Amount>0 rows
    // are actually submitted (SaleOrderCustomerExpenseslist in Insert()).
    var expenseRowsForSave = currentExpenseItems.filter(function (e) {
        return e.itemId && parseFloat(e.amount) > 0;
    });

    var payload = {
        voucherCode: parseInt($('#txtDocNo').val()) || 0,
        orderDate: $('#txtDocDate').val(),
        dueDate: $('#txtDueDate').val(),
        dueDays: parseInt($('#txtDueDays').val()) || 0,
        deliveryStartDate: $('#txtDeliveryStartDate').val(),
        deliveryDays: parseInt($('#txtDeliveryDays').val()) || 7,
        orderCategoryId: parseInt($('#cmbOrderCategory').val()) || null,
        categoryI_Id: parseInt($('#cmbCategoryI').val()) || null,
        categoryII_Id: parseInt($('#cmbCategoryII').val()) || null,
        customerId: parseInt(custId),
        partyRefNo: $('#txtPartyRefNo').val(),
        bookingPersonId: parseInt($('#cmbBookingPerson').val()) || null,
        paymentTermId: parseInt($('#cmbPaymentTerm').val()) || null,
        deliveryTermId: parseInt($('#cmbDeliveryTerm').val()) || null,
        orderStatusId: parseInt($('#cmbOrderStatus').val()) || 1,
        branchId: parseInt($('#cmbBranch').val()) || null,
        salesManId: parseInt($('#cmbSalesMan').val()) || null,
        commType: $('#cmbCommType').val(),
        commRate: parseFloat($('#txtCommRate').val()) || 0,
        commUomId: parseInt($('#cmbCommUom').val()) || null,
        commAmount: parseFloat($('#txtCommAmount').val()) || 0,
        commRemarks: $('#txtCommRemarks').val(),
        otherSalesManId: parseInt($('#cmbOtherSalesMan').val()) || null,
        otherCommType: $('#cmbOtherCommType').val(),
        otherCommRate: parseFloat($('#txtOtherCommRate').val()) || 0,
        otherCommUomId: parseInt($('#cmbOtherCommUom').val()) || null,
        otherCommAmount: parseFloat($('#txtOtherCommAmount').val()) || 0,
        otherCommRemarks: $('#txtOtherCommRemarks').val(),
        remarks: $('#txtRemarks').val(),
        lineItems: currentLineItems,
        expenseItems: expenseRowsForSave,
        paymentSchedules: paymentResult.rows
    };

    $.ajax({
        url: '/sale/sale-order/api/save',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(payload),
        success: function (res) {
            if (res.success) {
                alert(res.message || 'Sale Order saved successfully!');
                location.reload();
            } else {
                alert('Error: ' + (res.message || 'Failed to save Sale Order.'));
            }
        },
        error: function (xhr) {
            alert('Server error saving Sale Order: ' + xhr.responseText);
        }
    });
}

function btnPrintReport(reportCode) {
    window.print();
}

function loadHistoryData() {
    $.get('/sale/sale-order/api/history', function (data) {
        var tbody = $('#tblHistory tbody').empty();
        if (!data || data.length === 0) {
            tbody.append('<tr><td colspan="9" class="text-center text-muted" style="padding:15px;">No history records found.</td></tr>');
            return;
        }
        data.forEach(function (item) {
            var tr = `<tr data-id="${item.id}" ondblclick="loadOrderIntoForm(${item.id})">
                <td class="text-center"><button class="btn btn-sm btn-primary p-0 px-2" onclick="loadOrderIntoForm(${item.id})">Edit</button></td>
                <td class="text-center"><button class="btn btn-sm btn-warning p-0 px-2" onclick="btnPrintReport(${item.id})">Print</button></td>
                <td>SO-${item.voucherCode || ''}</td>
                <td>${item.orderDate || ''}</td>
                <td>${item.customerCode || 'CUST-01'}</td>
                <td>${item.customerName || 'N/A'}</td>
                <td class="text-end font-weight-bold">${(parseFloat(item.totalAmount) || 0).toFixed(2)}</td>
                <td>${item.entryDate || item.orderDate || ''}</td>
                <td class="text-center"><span class="badge bg-success">Approved</span></td>
            </tr>`;
            tbody.append(tr);
        });
    });
}

function loadOrderIntoForm(id) {
    // Real load-by-id: header via Sp_SaleOrder_GetAllMethod @Activity='ReadById', plus the same
    // four sub-list activities the desktop's own DAL loads (detail/payment/expense/extra-items).
    // Field names below are the exact raw columns those procedures return - see
    // SaleOrderService.getSaleOrderById() and SALE-ORDER-PROGRESS.md.
    $.get('/sale/sale-order/api/' + id, function (data) {
        if (!data) return alert('Failed to load order (not found).');
        showFormTab();
        $('#txtDocNo').val(data.DocNo != null ? data.DocNo : id);
        $('#txtDocDate').val(data.DocDate ? data.DocDate.substring(0, 10) : '');
        $('#cmbCustomer').val(data.OrderSupCustId || '').trigger('change');
        $('#txtRemarks').val(data.RemarksHeader || '');
        $('#cmbOrderCategory').val(data.OrderCatagoryId || '');
        $('#cmbBranch').val(data.BranchesId || '');
        $('#cmbBookingPerson').val(data.BookingPersonId || '');
        $('#cmbPaymentTerm').val(data.PaymentTermsId || '');
        $('#txtDueDays').val(data.OrderDueDays != null ? data.OrderDueDays : 0);
        $('#txtDueDate').val(data.OrderDueDate ? data.OrderDueDate.substring(0, 10) : '');
        $('#cmbDeliveryTerm').val(data.DeliveryTerm || '');
        $('#txtDeliveryStartDate').val(data.DeliveryStartDate ? data.DeliveryStartDate.substring(0, 10) : '');
        $('#txtDeliveryDays').val(data.DeliveryDays != null ? data.DeliveryDays : 0);
        $('#cmbSalesMan').val(data.BrokerAgentSupCustId || '');
        $('#cmbCommType').val(data.CommissionType || '');
        $('#txtCommRate').val(data.CommRate != null ? data.CommRate : '');
        $('#txtCommAmount').val(data.CommAmount != null ? data.CommAmount : '');
        $('#txtCommRemarks').val(data.CommissionRemarks || '');
        $('#cmbOtherSalesMan').val(data.OtherCommissionAgentId || '');
        $('#cmbOtherCommType').val(data.OtherCommissionType || '');
        $('#txtOtherCommRate').val(data.OtherCommissionRate != null ? data.OtherCommissionRate : '');
        $('#txtOtherCommAmount').val(data.OtherCommissionAmount != null ? data.OtherCommissionAmount : 0);
        $('#txtOtherCommRemarks').val(data.OtherCommissionRemarks || '');

        // Detail tab - real SaleOrderDetail rows (Sp_SaleOrder_GetAllMethod @Activity='ReadBySaleOrderHeaderId').
        // Mapped into the existing internal currentLineItems shape the Detail-tab grid code already uses
        // (that grid's own real-schema rebuild is a separate pending task - this only fixes what data feeds it).
        if (data.lineItems && data.lineItems.length > 0) {
            currentLineItems = data.lineItems.map(function (l) {
                return {
                    itemId: l.OrderItemId,
                    itemName: l.ItemName || 'Item',
                    itemCode: l.ItemCodeNew || ('ITM-' + l.OrderItemId),
                    cropYear: l.Crop || '',
                    jobLot: l.JobLotDescription || '',
                    jobLotId: l.JobLotId || '',
                    packingType: l.PackingType || '',
                    packingTypeId: l.PackingTypeID || '',
                    packSize: '',
                    quantity: parseFloat(l.OrderItemQty) || 0,
                    weight: parseFloat(l.NetWeight) || 0,
                    rateUom: l.RateUom || l.UOMCode || '',
                    rate: parseFloat(l.OrderItemRate) || 0,
                    amount: parseFloat(l.Amount) || 0,
                    bagPrice: parseFloat(l.BagPrice) || 0,
                    weightCut: parseFloat(l.BagWeight) || 0,
                    cityId: l.CityId || '',
                    cityArea: l.CityArea || '',
                    warehouseId: l.WarehouseId || '',
                    warehouse: l.WarehouseName || '',
                    labSample: l.LabSampleNo || '',
                    remarks: l.OrderRemarks || '',
                    commOnSale: l.CommOnSale === true || l.CommOnSale === 1,
                    // Row identity/soft-delete plumbing needed by the real save proc
                    // (Sp_SaleOrderDetail_Insert branches on ActionTypeId) - not yet consumed by
                    // the backend (still blocked), kept here so it's ready when unblocked.
                    id: l.Id || 0,
                    actionTypeId: 2
                };
            });
            renderDetailGrid();
        } else {
            currentLineItems = [];
            renderDetailGrid();
        }

        // Customer Expense / Payment Detail tabs - real rows fetched via the same load-by-id
        // activities the desktop DAL uses (SaleOrderCustomerExpensesByHeaderId /
        // SaleOrderPaymentTermDetailByHeaderId), now rendered into their real grids.
        currentExpenseItems = (data.customerExpenses || []).map(function (e) {
            return {
                itemId: e.InvRevExpItemId,
                itemName: e.OtherItemName || '',
                quantity: parseFloat(e.Qty) || 0,
                rate: parseFloat(e.Rate) || 0,
                amount: parseFloat(e.Amount) || 0,
                remarks: e.Remarks || ''
            };
        });
        renderExpenseGrid();
        currentPaymentSchedules = (data.paymentSchedules || []).map(function (p) {
            return {
                paymentTermId: p.PaymentTermId,
                paymentTerm: p.TermsDescription || '',
                dueDays: p.DueDays || 0,
                dueDate: p.DueDate ? p.DueDate.substring(0, 10) : '',
                percentOfTotal: parseFloat(p.PrcntOfTotal) || 0,
                amount: parseFloat(p.Amount) || 0,
                remarks: p.PaymentRemarks || ''
            };
        });
        renderPaymentGrid();
    });
}
