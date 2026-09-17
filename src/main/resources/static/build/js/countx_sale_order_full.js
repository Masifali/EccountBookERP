/**
 * Sale Order Master JS Module — WinForms Golden Master Replica
 * Handles dropdown population, item filtering, detail line calculations,
 * Customer Expense tab, Payment Detail tab, Stock Reports, Toolbar actions,
 * and History search.
 */

var masterLookupsData = {};
var currentLineItems = [];
var removedLineItems = [];
var editingLineIndex = -1;
var pendingLineUomSelection = null;
var preBookingRows = [];
var currentExpenseItems = [];
var currentPaymentSchedules = [];
// Real desktop RecId equivalent: 0 = New (btnsave_Click path -> Sp_SaleOrder_Insert), >0 = an
// existing order currently loaded for edit (btnupdate_Click path -> Sp_SaleOrder_Update). Set by
// loadOrderIntoForm(); btnSave_Click() must send this as the payload's `id` so Update actually
// reaches Sp_SaleOrder_Update instead of always inserting a new order.
var currentEditingOrderId = 0;

function setSaleOrderBusy(on) {
    $('#saleOrderLoader').prop('hidden', !on);
    $('button').each(function () {
        if (on) {
            this.dataset.saleOrderWasDisabled = this.disabled ? '1' : '0';
            this.disabled = true;
        } else {
            this.disabled = this.dataset.saleOrderWasDisabled === '1';
            delete this.dataset.saleOrderWasDisabled;
        }
    });
}
$(document).ajaxStart(function () { setSaleOrderBusy(true); });
$(document).ajaxStop(function () { setSaleOrderBusy(false); });

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

        // CmbCropyr stores both its selected Id and its displayed CropYear text in each detail row.
        bindCombo('#lineCropYear', data.cropYears, 'Id', 'CropYear');

        // Job/Lots - USP_GetJobLotsAllocatedToBranch (Id, JobLotDescription), scoped to the current branch
        bindCombo('#lineJobLot', data.jobLots, 'Id', 'JobLotDescription');

        // Packing Types - Sp_InvPackingType_GetAllMethod (Id, PackTypeDesc)
        bindCombo('#linePackType', data.packingTypes, 'Id', 'PackTypeDesc');

        // Cities - SP_City_GetAllMethod (Id, CityName) & Warehouses - USP_GetWarehousesAllocatedToBranch (Id, WareHouseName), scoped to the current branch
        bindCombo('#lineCityArea', data.cities, 'Id', 'CityName');
        bindCombo('#lineWarehouse', data.warehouses, 'Id', 'WareHouseName');

        // Payment Detail dropdown - Sp_InvDueTerms_GetAllMethod (Id, TermsDescription). Real, correct source.
        bindCombo('#payTerm', data.paymentTerms, 'Id', 'TermsDescription', '-- Select Term --');

        // Desktop OtherItemsBind(): InventoryItemsOther.GetAll -> Sp_InventoryItemsOther_GetAllMethod.
        bindCombo('#expItem', data.otherItems, 'Id', 'OtherItemName', '-- Select Item --');

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
    // select2 (".so-select2", see sale_order.html) does not notice raw DOM <option> changes made
    // via jQuery .empty()/.append() - it only refreshes its rendered list/selection on the
    // underlying <select>'s native 'change' event. Every dropdown on this screen is searchable
    // now (matching the real desktop's 30 AutoCompleteMode/AutoSuggestFilterMode UltraCombo
    // controls - see SaleOrder.cs), so this is required after every rebuild, not just cosmetic.
    $el.trigger('change');
}

function bindCustomerCombo(customers) {
    var $el = $('#cmbCustomer').empty();
    $el.append('<option value="">...Select Any Value...</option>');
    if (customers) {
        var searchByCode = $('#radPartyCode').is(':checked');
        customers.forEach(function (c) {
            var label = searchByCode ? ((c.PartyCode || '') + ' - ' + c.CompanyName) : c.CompanyName;
            $el.append($('<option>', { value: c.Id, text: label }));
        });
    }
    $el.trigger('change'); // refresh select2 (".so-select2") after rebuilding options - see bindCombo()
}

function bindItemCombo(items) {
    var $el = $('#lineItem').empty();
    $el.append('<option value="">-- Select Item --</option>');
    if (items) {
        var searchByCode = $('#radCode').is(':checked');
        items.forEach(function (i) {
            var label = searchByCode ? (i.ItemCode + ' - ' + i.ItemName) : i.ItemName;
            $el.append($('<option>', { value: i.Id, text: label, 'data-code': i.ItemCode, 'data-name': i.ItemName }));
        });
    }
    $el.trigger('change'); // refresh select2 (".so-select2") after rebuilding options - see bindCombo()
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
        var balanceUrl = '/sale/sale-order/api/customer-balance/' + custId +
            (currentEditingOrderId ? '?saleOrderId=' + currentEditingOrderId : '');
        $.get(balanceUrl, function (data) {
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
            // Pack Uom (OrderItemUOMId) is a real, required field distinct from Rate Uom - see
            // SALE-ORDER-PROGRESS.md Pass 3 ("Pack Uom not found in detail grid at row#N"). Both
            // are drawn from the same real per-item UOMSchedule rows, just auto-defaulted to a
            // different flag column (BaseRateUom vs BasePackUom), ditto desktop's
            // CommonServices.GetBaseRateUomId / GetBasePackUomId.
            var $packUom = $('#linePackUom').empty();
            if (uoms && uoms.length > 0) {
                var baseUomCode = null;
                var basePackUomCode = null;
                uoms.forEach(function (u) {
                    $uom.append($('<option>', { value: u.Id, text: u.UOMCode, 'data-equivalent': u.QtyEquivalent }));
                    $packUom.append($('<option>', { value: u.Id, text: u.UOMCode, 'data-equivalent': u.QtyEquivalent }));
                    if (u.BaseRateUom === true || u.BaseRateUom === 1) {
                        baseUomCode = u.Id;
                    }
                    if (u.BasePackUom === true || u.BasePackUom === 1) {
                        basePackUomCode = u.Id;
                    }
                });
                if (baseUomCode !== null) {
                    $uom.val(baseUomCode);
                }
                if (basePackUomCode !== null) {
                    $packUom.val(basePackUomCode);
                } else if (baseUomCode !== null) {
                    $packUom.val(baseUomCode); // fall back to the same schedule row desktop uses for Rate Uom
                }
            }
            // Refresh select2 (".so-select2") after rebuilding these two dropdowns' options and
            // selection - see bindCombo() for why trigger('change') is required.
            $uom.trigger('change');
            $packUom.trigger('change');
            if (pendingLineUomSelection) {
                $uom.val(pendingLineUomSelection.rateUomId).trigger('change');
                $packUom.val(pendingLineUomSelection.packUomId).trigger('change');
                pendingLineUomSelection = null;
            }
            calcLine('qty');
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
function calcLine(source) {
    var qty = parseFloat($('#lineQty').val()) || 0;
    var rate = parseFloat($('#lineRate').val()) || 0;
    var weight = parseFloat($('#lineWeight').val()) || 0;
    var packEquivalent = parseFloat($('#linePackUom option:selected').attr('data-equivalent')) || 0;
    var rateEquivalent = parseFloat($('#lineRateUom option:selected').attr('data-equivalent')) || 0;
    if (source === 'weight' && packEquivalent > 0) {
        qty = weight / packEquivalent;
        $('#lineQty').val(qty.toFixed(3));
    } else if (packEquivalent > 0) {
        weight = qty * packEquivalent;
        $('#lineWeight').val(weight.toFixed(3));
    }
    var amount = (weight > 0 && rateEquivalent > 0 && rate > 0)
        ? Math.round((weight / rateEquivalent) * rate)
        : 0;
    $('#lineAmount').val(amount.toFixed(4));
}

function btnAddRow_Click() {
    var itemId = $('#lineItem').val();
    var itemName = $('#lineItem option:selected').text();
    var itemCode = $('#lineItem option:selected').attr('data-code') || '';
    var cropYearId = $('#lineCropYear').val() || '';
    var cropYear = $('#lineCropYear option:selected').text() || '';
    var jobLot = $('#lineJobLot option:selected').text() || 'General';
    var packType = $('#linePackType option:selected').text() || 'PP Bags';
    var packSize = $('#linePackSize').val() || '40 KG';
    var qty = parseFloat($('#lineQty').val()) || 0;
    var weight = parseFloat($('#lineWeight').val()) || 0;
    var packUomId = $('#linePackUom').val() || '';
    var packUom = $('#linePackUom option:selected').text() || '';
    var rateUomId = $('#lineRateUom').val() || '';
    var rateUom = $('#lineRateUom option:selected').text() || '';
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
    if (!packUomId) { alert('Pack Uom not found in detail grid.'); return; }
    if (weight <= 0) { alert('New Weight not found in detail grid.'); return; }
    if (rate > 0 && amount <= 0) { alert('Amount not found in detail grid.'); return; }

    var itemObj = {
        itemId: itemId,
        itemName: itemName,
        cropYearId: cropYearId ? parseInt(cropYearId) : null,
        itemCode: itemCode,
        cropYear: cropYear,
        jobLot: jobLot,
        jobLotId: jobLotIdVal,
        packingType: packType,
        packingTypeId: packTypeIdVal,
        packSize: packSize,
        quantity: qty,
        weight: weight,
        packUomId: packUomId,
        packUom: packUom,
        rateUom: rateUom,
        rateUomId: rateUomId,
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

    if (editingLineIndex >= 0) {
        var persisted = currentLineItems[editingLineIndex];
        itemObj.id = persisted.id || 0;
        itemObj.actionTypeId = itemObj.id > 0 ? 2 : 1;
        currentLineItems[editingLineIndex] = itemObj;
    } else {
        itemObj.id = 0;
        itemObj.actionTypeId = 1;
        currentLineItems.push(itemObj);
    }
    editingLineIndex = -1;
    renderDetailGrid();
    clearLineEntry();
}

function renderDetailGrid() {
    var tbody = $('#tblDetail tbody').empty();
    if (currentLineItems.length === 0) {
        tbody.append('<tr><td colspan="20" class="text-center text-muted" style="padding: 12px;">No order line items added yet. Record: 0 of 0</td></tr>');
        recalcTotals();
        return;
    }

    currentLineItems.forEach(function (item, index) {
        var tr = `<tr ondblclick="editLineItem(${index})" title="Double-click to edit">
            <td class="text-center"><button class="btn btn-sm btn-danger p-0 px-1" onclick="removeLineItem(${index})">&times;</button></td>
            <td>${item.itemCode}</td>
            <td>${item.itemName}</td>
            <td>${item.cropYear}</td>
            <td>${item.jobLot}</td>
            <td>${item.packingType}</td>
            <td>${item.packSize}</td>
            <td class="text-end qty-val">${item.quantity.toFixed(2)}</td>
            <td class="text-end wt-val">${item.weight.toFixed(2)}</td>
            <td>${item.packUom || item.packUomId || ''}</td>
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

function editLineItem(index) {
    var item = currentLineItems[index];
    if (!item) return;
    editingLineIndex = index;
    pendingLineUomSelection = { packUomId: item.packUomId, rateUomId: item.rateUomId };
    $('#lineItem').val(item.itemId).trigger('change');
    $('#lineCropYear').val(item.cropYearId || '').trigger('change');
    $('#lineJobLot').val(item.jobLotId || '').trigger('change');
    $('#linePackType').val(item.packingTypeId || '').trigger('change');
    $('#linePackSize').val(item.packSize || '');
    $('#lineQty').val(item.quantity);
    $('#lineWeight').val(item.weight);
    $('#lineRate').val(item.rate);
    $('#lineAmount').val(item.amount);
    $('#lineBagPrice').val(item.bagPrice);
    $('#lineWtCut').val(item.weightCut);
    $('#lineCityArea').val(item.cityId || '').trigger('change');
    $('#lineWarehouse').val(item.warehouseId || '').trigger('change');
    $('#lineLabSample').val(item.labSample || '');
    $('#lineRemarks').val(item.remarks || '');
    $('#lineCommOnSale').prop('checked', !!item.commOnSale);
}

function removeLineItem(index) {
    var removed = currentLineItems.splice(index, 1)[0];
    if (removed && parseInt(removed.id) > 0) {
        removed.actionTypeId = 3;
        removedLineItems.push(removed);
    }
    renderDetailGrid();
}

function clearLineEntry() {
    editingLineIndex = -1;
    $('#lineItem').val('');
    $('#lineQty').val('');
    $('#lineWeight').val('');
    // NOTE: Pack Uom (like Rate Uom, see #lineRateUom below) is intentionally left
    // populated here. Both dropdowns are (re)populated together from the same
    // /api/item-uoms/{itemId} response inside the #lineItem change handler; emptying
    // only #linePackUom here (and not #lineRateUom) left it with zero <option>
    // elements after every successful "Add Row" click - the reported "UOM pack
    // dropdown is empty" bug - until the user re-selected an Item to repopulate it.
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
        preBookingRows = data || [];
        var tbody = $('#tblPreBookingModal tbody').empty();
        if (!data || data.length === 0) {
            tbody.append('<tr><td colspan="5" class="text-center text-muted" style="padding:15px;">No pre-booking orders available.</td></tr>');
        } else {
            var grouped = {};
            data.forEach(function (row) {
                var id = row.PreBookingOrderId;
                if (!grouped[id]) grouped[id] = { id: id, docNo: row.DocNo, date: row.DocDate,
                    customerName: row.CustomerName, totalQty: 0 };
                grouped[id].totalQty += parseFloat(row.BalQty) || 0;
            });
            Object.keys(grouped).forEach(function (key) {
                var row = grouped[key];
                tbody.append(`<tr>
                    <td><button type="button" class="voucher-link" onclick="loadPreBooking(${row.id})">BO-${row.docNo || ''}</button></td>
                    <td>${formatDateValue(row.date)}</td>
                    <td>${row.customerName || ''}</td>
                    <td class="text-end">${(parseFloat(row.totalQty) || 0).toFixed(2)}</td>
                    <td class="text-center"><button class="btn btn-sm btn-primary" onclick="loadPreBooking(${row.id})">Load</button></td>
                </tr>`);
            });
        }
        $('#modalPreBooking').modal('show');
    });
}

function loadPreBooking(id) {
    var rows = preBookingRows.filter(function (row) { return parseInt(row.PreBookingOrderId) === parseInt(id); });
    if (!rows.length) return;
    var first = rows[0];
    $('#cmbCustomer').val(first.OrderSupCustId || '').trigger('change');
    currentLineItems = rows.filter(function (row) { return (parseFloat(row.BalQty) || 0) > 0; }).map(function (row) {
        return {
            id: 0, actionTypeId: 1, refDocId: row.PreBookingOrderId, refDocDetailId: row.DetailId,
            itemId: row.OrderItemId, itemCode: row.ItemCode || '', itemName: row.ItemName || '',
            cropYearId: row.CropYearId || '', cropYear: row.CropYear || '',
            jobLotId: row.JobLotId || '', jobLot: row.JobLot || '',
            packingTypeId: $('#linePackType').val() || '', packingType: $('#linePackType option:selected').text() || '',
            packUomId: row.OrderItemUOMId || '', packUom: row.PackUom || '', packSize: row.PackUom || '',
            quantity: parseFloat(row.BalQty) || 0, weight: parseFloat(row.BalWeight) || 0,
            rateUomId: row.OrderItemRateUOMId || '', rateUom: row.RateUom || '',
            rate: parseFloat(row.OrderItemRate) || 0, amount: parseFloat(row.BalAmount) || 0,
            bagPrice: 0, weightCut: 0, cityId: $('#lineCityArea').val() || '',
            cityArea: $('#lineCityArea option:selected').text() || '', warehouseId: $('#lineWarehouse').val() || '',
            warehouse: $('#lineWarehouse option:selected').text() || '', labSample: '', remarks: '',
            commOnSale: false, costCenterId: row.CostCenterId || null
        };
    });
    renderDetailGrid();
    $('#modalPreBooking').modal('hide');
}

function openAttachmentsModal() {
    if (!currentEditingOrderId) {
        alert('Save or load a Sale Order before managing attachments.');
        return;
    }
    loadAttachments();
    $('#modalAttachments').modal('show');
}

function loadAttachments(rows) {
    var request = rows ? $.Deferred().resolve(rows).promise()
        : $.get('/sale/sale-order/api/' + currentEditingOrderId + '/attachments');
    request.done(function (data) {
        var list = $('#attachmentList').empty();
        if (!data || data.length === 0) {
            list.append('<div class="text-muted">No attachments.</div>');
            return;
        }
        data.forEach(function (row) {
            list.append('<label class="d-flex align-items-center gap-2 mb-1">' +
                '<input type="checkbox" class="remove-sale-order-attachment" value="' + row.Id + '"> Remove ' +
                '<a href="/sale/sale-order/api/' + currentEditingOrderId + '/attachments/' + row.Id + '">' +
                $('<div>').text(row.Attachment || 'Attachment').html() + '</a></label>');
        });
    });
}

function fileAsUpload(file) {
    return new Promise(function (resolve, reject) {
        var reader = new FileReader();
        reader.onload = function () { resolve({ name: file.name, base64: String(reader.result).split(',')[1] || '' }); };
        reader.onerror = reject;
        reader.readAsDataURL(file);
    });
}

function saveAttachments() {
    if (!currentEditingOrderId) return;
    var files = Array.from(document.getElementById('attachmentFile').files || []);
    var removeIds = $('.remove-sale-order-attachment:checked').map(function () { return parseInt(this.value); }).get();
    Promise.all(files.map(fileAsUpload)).then(function (uploads) {
        return $.ajax({
            url: '/sale/sale-order/api/' + currentEditingOrderId + '/attachments',
            method: 'POST', contentType: 'application/json',
            data: JSON.stringify({ files: uploads, removeAttachmentIds: removeIds })
        });
    }).then(function (rows) {
        $('#attachmentFile').val('');
        loadAttachments(rows);
    }).catch(function (error) {
        alert(error.responseJSON && error.responseJSON.message ? error.responseJSON.message : 'Unable to save attachments.');
    });
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
        id: currentEditingOrderId || null,
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
        lineItems: currentLineItems.concat(removedLineItems),
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
                currentEditingOrderId = res.id;
                removedLineItems = [];
                if (res.docNo != null) $('#txtDocNo').val(res.docNo);
                alert(res.message || 'Sale Order saved successfully!');
                loadOrderIntoForm(res.id);
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
    var id = currentEditingOrderId || 0;
    if (!id) {
        alert('Save or load a Sale Order before printing.');
        return;
    }
    window.print();
}

function formatDateValue(value) {
    if (!value) return '';
    var date = new Date(value);
    if (isNaN(date.getTime())) return String(value);
    return String(date.getDate()).padStart(2, '0') + '-' +
        date.toLocaleString('en-GB', { month: 'short' }) + '-' +
        String(date.getFullYear()).slice(-2);
}

function loadHistoryData() {
    var query = $.param({
        fromDate: $('#histFromDate').val() || '',
        toDate: $('#histToDate').val() || '',
        customerId: $('#histCustomerCombo').val() || ''
    });
    $.get('/sale/sale-order/api/history?' + query, function (data) {
        var tbody = $('#tblHistory tbody').empty();
        if (!data || data.length === 0) {
            tbody.append('<tr><td colspan="9" class="text-center text-muted" style="padding:15px;">No history records found.</td></tr>');
            return;
        }
        data.forEach(function (item) {
            var tr = `<tr data-id="${item.Id}" ondblclick="loadOrderIntoForm(${item.Id})">
                <td class="text-center"><button class="btn btn-sm btn-primary p-0 px-2" onclick="loadOrderIntoForm(${item.Id})">Edit</button></td>
                <td class="text-center"><button class="btn btn-sm btn-warning p-0 px-2" onclick="currentEditingOrderId=${item.Id};btnPrintReport('273')">Print</button></td>
                <td><button type="button" class="voucher-link" onclick="loadOrderIntoForm(${item.Id})">SO-${item.DocNo || ''}</button></td>
                <td>${formatDateValue(item.DocDate)}</td>
                <td>${item.PartyCode || item.CustomerCode || ''}</td>
                <td>${item.CustomerName || ''}</td>
                <td class="text-end font-weight-bold">${(parseFloat(item.OrderAmount || item.TotalAmount) || 0).toFixed(2)}</td>
                <td>${formatDateValue(item.EntryDate)}</td>
                <td class="text-center">${(item.IsAproved === true || item.IsAproved === 1) ? 'Approved' : 'Not Approved'}</td>
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
        currentEditingOrderId = data.Id || id;
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
            removedLineItems = [];
            currentLineItems = data.lineItems.map(function (l) {
                return {
                    itemId: l.OrderItemId,
                    itemName: l.ItemName || 'Item',
                    itemCode: l.ItemCodeNew || ('ITM-' + l.OrderItemId),
                    cropYear: l.Crop || '',
                    cropYearId: l.CropYearID || l.CropYearId || '',
                    jobLot: l.JobLotDescription || '',
                    jobLotId: l.JobLotId || '',
                    packingType: l.PackingType || '',
                    packingTypeId: l.PackingTypeID || '',
                    packSize: '',
                    quantity: parseFloat(l.OrderItemQty) || 0,
                    weight: parseFloat(l.NetWeight) || 0,
                    packUomId: l.OrderItemUOMId || '',
                    packUom: l.PackUom || l.PackUomCode || '',
                    rateUom: l.RateUom || l.UOMCode || '',
                    rateUomId: l.OrderItemRateUOMId || '',
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
                    // Sp_SaleOrderDetail_Insert branches on ActionTypeId; removed persisted rows
                    // are retained separately and posted back as ActionTypeId=3.
                    id: l.Id || 0,
                    actionTypeId: 2
                };
            });
            renderDetailGrid();
        } else {
            currentLineItems = [];
            removedLineItems = [];
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
