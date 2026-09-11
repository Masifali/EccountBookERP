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

        // Customer Expense & Payment Detail Dropdowns
        bindCombo('#expItem', data.items, 'Id', 'ItemName', '-- Select Item --');
        bindCombo('#payTerm', data.paymentTerms, 'Id', 'TermsDescription', '-- Select Term --');

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
    var cityArea = $('#lineCityArea option:selected').text() || 'Khudian Khas';
    var warehouse = $('#lineWarehouse option:selected').text() || 'RICE ROOM G-1';

    if (!itemId || qty <= 0) {
        alert('Please select an Item and enter Quantity.');
        return;
    }

    var itemObj = {
        itemId: itemId,
        itemName: itemName,
        itemCode: 'ITM-' + itemId,
        cropYear: cropYear,
        jobLot: jobLot,
        packingType: packType,
        packSize: packSize,
        quantity: qty,
        weight: weight,
        rateUom: rateUom,
        rate: rate,
        amount: amount,
        bagPrice: bagPrice,
        weightCut: wtCut,
        cityArea: cityArea,
        warehouse: warehouse
    };

    currentLineItems.push(itemObj);
    renderDetailGrid();
    clearLineEntry();
}

function renderDetailGrid() {
    var tbody = $('#tblDetail tbody').empty();
    if (currentLineItems.length === 0) {
        tbody.append('<tr><td colspan="15" class="text-center text-muted" style="padding: 12px;">No order line items added yet. Record: 0 of 0</td></tr>');
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
            <td>${item.cityArea}</td>
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
}

// =========================================================
// 4. CUSTOMER EXPENSE TAB LOGIC
// =========================================================
function btnAddExpenseRow_Click() {
    var itemId = $('#expItem').val();
    var itemName = $('#expItem option:selected').text();
    var qty = parseFloat($('#expQty').val()) || 0;
    var rate = parseFloat($('#expRate').val()) || 0;
    var remarks = $('#expRemarks').val() || '';

    if (!itemId || qty <= 0) {
        alert('Select Item and enter Quantity for Expense.');
        return;
    }

    var amt = qty * rate;
    currentExpenseItems.push({
        itemId: itemId,
        itemName: itemName,
        quantity: qty,
        rate: rate,
        amount: amt,
        remarks: remarks
    });

    renderExpenseGrid();
    $('#expItem').val(''); $('#expQty').val(''); $('#expRate').val(''); $('#expRemarks').val('');
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
    $('#payTerm').val(''); $('#payDueDays').val('0'); $('#payPercent').val('0'); $('#payAmount').val('0.00'); $('#payRemarks').val('');
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
        expenseItems: currentExpenseItems,
        paymentSchedules: currentPaymentSchedules
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
    alert('Generating Print Preview for Report #' + reportCode);
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
                    packingType: l.PackingType || '',
                    packSize: '',
                    quantity: parseFloat(l.OrderItemQty) || 0,
                    weight: parseFloat(l.NetWeight) || 0,
                    rateUom: l.RateUom || l.UOMCode || '',
                    rate: parseFloat(l.OrderItemRate) || 0,
                    amount: parseFloat(l.Amount) || 0,
                    bagPrice: parseFloat(l.BagPrice) || 0,
                    weightCut: parseFloat(l.BagWeight) || 0,
                    cityArea: l.CityArea || '',
                    warehouse: l.WarehouseName || ''
                };
            });
            renderDetailGrid();
        } else {
            currentLineItems = [];
            renderDetailGrid();
        }

        // Customer Expense / Payment Detail tabs are still placeholders in the HTML (separate
        // pending tasks) - real rows are already being fetched here so nothing is lost, they are
        // just not rendered into a grid yet.
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
    });
}
