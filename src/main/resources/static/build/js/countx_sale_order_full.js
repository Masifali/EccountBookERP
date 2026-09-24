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
    /* desktop duedate is Enabled=false and only ever set from Due Days (SaleOrder.cs :749,
       :1643-1655); a blank form leaves it empty, so nothing is defaulted here. */
    $('#txtDeliveryStartDate').val(today);
    $('#histFromDate').val(today);
    $('#histToDate').val(today);
    $('#histUseFrom, #histUseTo').prop('checked', true);

    initMultiColumnSelect2();
    loadMasterLookups();
    setupEventListeners();
});

function escapeHtml(str) {
    if (str === null || str === undefined) return '';
    return String(str).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
}

/* The desktop's UltraCombo dropdowns are grids: BindDDL shows every column of the bound table
   except the value member, with the display column recaptioned. attachColumnHeader reproduces the
   header band. It always replaces whatever header is there, because select2 reuses one dropdown
   container for every combo on the page. */
function attachColumnHeader(headerHtml) {
    setTimeout(function () {
        var $r = $('.select2-results');
        $r.find('.select2-col-header').remove();
        $r.prepend(headerHtml);
    }, 10);
}

/* SUPERSEDED by build/js/countx_desktop_combo.js.
 *
 * The column sets this function hard-coded are now declared on the elements themselves
 * (data-dtcombo / data-dtcombo-caption) and rendered by the shared component, so Sale Order and
 * Purchase Order draw their drop grids through one implementation instead of two that disagreed
 * about markup, CSS and captions.
 *
 * Kept as a no-op rather than deleted: it is called from this file's ready handler, and the
 * shared component skips anything already initialised. Re-initialising a select2 instance
 * destroys the previous one and silently drops its templateResult - which is exactly how these
 * combos lost their columns once before (see SALE-ORDER-81-DROPDOWN-GRIDS-AND-ITEM-FILTER.md).
 */
function initMultiColumnSelect2() { return; }

function initMultiColumnSelect2_superseded() {
    /* Item combo - desktop combitem, ItemDetailFill() :1305-1325. dtitem columns are
       Id, ItemName, ItemCode, InventoryParentCategoriesId, ItemCategoryId, ItemCategory,
       ItemTypeId, ItemType, ProductionStageId, ProductionStage; columns 3, 4, 6 and 8 are
       hidden, so five stay visible: Item | ItemCode | ItemCategory | ItemType | ProductionStage. */
    $('#lineItem').select2({
        width: '100%',
        dropdownAutoWidth: true,
        templateResult: formatItem5Col,
        templateSelection: function (state) { return state.text; },
        escapeMarkup: function (m) { return m; }
    }).on('select2:open', function () {
        attachColumnHeader(
            '<div class="select2-col-header select2-item-header">' +
                '<span style="flex: 4;">Item</span>' +
                '<span style="flex: 2;">ItemCode</span>' +
                '<span style="flex: 3;">ItemCategory</span>' +
                '<span style="flex: 2;">ItemType</span>' +
                '<span style="flex: 2;">ProductionStage</span>' +
            '</div>');
    });

    /* Item Category / Item Type combo - desktop CmbCategory, :1263-1266. One visible column whose
       caption follows the radio pair ("Item Category" / "ItemType"). */
    $('#lineItemCategory').select2({
        width: '100%',
        dropdownAutoWidth: true
    }).on('select2:open', function () {
        attachColumnHeader(
            '<div class="select2-col-header select2-item-header">' +
                '<span style="flex: 1;">' +
                ($('#radType').is(':checked') ? 'ItemType' : 'Item Category') +
                '</span></div>');
    });

    /* Rate UOM - desktop combrateuom, bound from the SAME dtUom as Pack Size (:1353), so it shows
       the same four columns, only the first one recaptioned. It had no column header at all. */
    $('#lineRateUom').select2({
        width: '100%',
        dropdownAutoWidth: true,
        templateResult: formatUom4Col,
        templateSelection: function (state) { return state.text; },
        escapeMarkup: function (m) { return m; }
    }).on('select2:open', function () {
        attachColumnHeader(
            '<div class="select2-col-header select2-uom-header">' +
                '<span style="flex: 2;">RateUOM</span>' +
                '<span style="flex: 2; text-align: right;">Equivalent</span>' +
                '<span style="flex: 2; text-align: right;">QtyEquivalent</span>' +
                '<span style="flex: 2; text-align: center;">BaseRateUom</span>' +
            '</div>');
    });

    /* Comm Type - desktop combcommtype / CmbOtherCommissionType, CommissionTypeFill() :1191-1194,
       bound with caption "CommissionType" and ZeroIndex: false. Comm Uom - combruom /
       CmbOtherCommissionUom, CommissionUOMFill() :1209-1212, caption "UOM", ZeroIndex: false.
       Neither had a column header band before. */
    $('#cmbCommType, #cmbOtherCommType').select2({ width: '100%', dropdownAutoWidth: true })
        .on('select2:open', function () {
            attachColumnHeader('<div class="select2-col-header select2-item-header">' +
                '<span style="flex: 1;">CommissionType</span></div>');
        });
    $('#cmbCommUom, #cmbOtherCommUom').select2({ width: '100%', dropdownAutoWidth: true })
        .on('select2:open', function () {
            attachColumnHeader('<div class="select2-col-header select2-item-header">' +
                '<span style="flex: 1;">UOM</span></div>');
        });

    // Pack Size / Pack Uom 4-Column Dropdown
    $('#linePackUom').select2({
        width: '100%',
        dropdownAutoWidth: true,
        templateResult: formatUom4Col,
        templateSelection: function (state) { return state.text; },
        escapeMarkup: function (m) { return m; }
    }).on('select2:open', function () {
        attachColumnHeader(
            '<div class="select2-col-header select2-uom-header">' +
                '<span style="flex: 2;">PackUOM</span>' +
                '<span style="flex: 2; text-align: right;">Equivalent</span>' +
                '<span style="flex: 2; text-align: right;">QtyEquivalent</span>' +
                '<span style="flex: 2; text-align: center;">BaseRateUom</span>' +
            '</div>');
    });

    // Party / SalesMan / Booking Person 3-Column Dropdown
    $('#cmbCustomer, #cmbSalesMan, #cmbOtherSalesMan, #cmbBookingPerson').select2({
        width: '100%',
        dropdownAutoWidth: true,
        templateResult: formatParty3Col,
        templateSelection: function (state) { return state.text; },
        escapeMarkup: function (m) { return m; }
    }).on('select2:open', function () {
        /* Desktop CompanyNameBind(:1078-1104) uses ONE binding for all three party combos and only
           the first column's caption changes: "Customer Name"/"Customer Code", "SalesMan Name",
           "OtherCommissionAgent". Columns 3 (GlAccountId) and 4 (CityId) are hidden, leaving
           Name | PartyCode | CityName. */
        var id = $(this).attr('id');
        var titleHeader = id === 'cmbCustomer' ? ($('#radPartyCode').is(':checked') ? 'Customer Code' : 'Customer Name')
                        : id === 'cmbBookingPerson' ? 'Booking Person'
                        : id === 'cmbOtherSalesMan' ? 'OtherCommissionAgent'
                        : 'SalesMan Name';
        attachColumnHeader(
            '<div class="select2-col-header select2-party-header">' +
                '<span style="flex: 3;">' + titleHeader + '</span>' +
                '<span style="flex: 2; padding-left: 6px;">PartyCode</span>' +
                '<span style="flex: 2; padding-left: 6px;">CityName</span>' +
            '</div>');
    });
}

function formatItem5Col(state) {
    if (!state.id) return state.text;
    var $el = $(state.element);
    var a = function (n) { return $el.attr(n) || ''; };
    return '<div class="select2-item-row">' +
        '<span style="flex: 4; font-weight: 500; overflow: hidden; text-overflow: ellipsis; white-space: nowrap;">' + escapeHtml(state.text) + '</span>' +
        '<span style="flex: 2; color: #555;">' + escapeHtml(a('data-code')) + '</span>' +
        '<span style="flex: 3; color: #555; overflow: hidden; text-overflow: ellipsis; white-space: nowrap;">' + escapeHtml(a('data-category')) + '</span>' +
        '<span style="flex: 2; color: #555;">' + escapeHtml(a('data-type')) + '</span>' +
        '<span style="flex: 2; color: #555;">' + escapeHtml(a('data-stage')) + '</span>' +
    '</div>';
}

function formatUom4Col(state) {
    if (!state.id) return state.text;
    var $el = $(state.element);
    var uom = $el.attr('data-uom') || $el.data('uom') || state.text;
    var eq = $el.attr('data-equivalent') || $el.data('equivalent') || '1';
    var qtyEq = $el.attr('data-qty-eq') || $el.data('qty-eq') || '1';
    var isBase = $el.attr('data-base-rate') === '1' || $el.data('base-rate') === '1' || $el.data('base-rate') === 1;
    var chk = isBase ? '<i class="fa fa-check-square-o text-success"></i>' : '<i class="fa fa-square-o text-muted"></i>';
    return '<div class="select2-uom-row">' +
        '<span style="flex: 2; font-weight: 500;">' + escapeHtml(uom) + '</span>' +
        '<span style="flex: 2; text-align: right;">' + escapeHtml(String(eq)) + '</span>' +
        '<span style="flex: 2; text-align: right;">' + escapeHtml(String(qtyEq)) + '</span>' +
        '<span style="flex: 2; text-align: center;">' + chk + '</span>' +
    '</div>';
}

function formatParty3Col(state) {
    if (!state.id) return state.text;
    var $el = $(state.element);
    var name = state.text;
    var code = $el.attr('data-code') || $el.data('code') || '';
    var city = $el.attr('data-city') || $el.data('city') || '';
    return '<div class="select2-party-row">' +
        '<span style="flex: 3; font-weight: 500; overflow: hidden; text-overflow: ellipsis; white-space: nowrap;">' + escapeHtml(name) + '</span>' +
        '<span style="flex: 2; color: #555; text-align: left; padding-left: 6px;">' + escapeHtml(code) + '</span>' +
        '<span style="flex: 2; color: #555; text-align: left; padding-left: 6px;">' + escapeHtml(city) + '</span>' +
    '</div>';
}

// =========================================================
// 1. MASTER LOOKUPS & DROPDOWN BINDING
// =========================================================
function loadMasterLookups() {
    $.get('/sale/sale-order/api/master-lookups', function (data) {
        if (!data) return;
        masterLookupsData = data;

        // Location Type
        bindCombo('#cmbLocationType', data.locationTypes, 'Id', 'Location');

        // Order Categories
        /* OrderCatagoryfill (SaleOrder.cs:980): BindDDLNew(..., ZeroIndex true) puts "...Select Any Value..."
           at row 0, then Rows[3] is activated - the THIRD real category, not the first. */
        bindCombo('#cmbOrderCategory', data.orderCategories, 'Id', 'OrderCategoryName', '...Select Any Value...');
        var ocOpts = $('#cmbOrderCategory option');
        if (ocOpts.length > 3) { $('#cmbOrderCategory').val(ocOpts.eq(3).val()).trigger('change'); }
        generateOrderCategoryNo();   /* PurchsaeOrder_Load :712 */

        /* OrderCategory_I/_II (SaleOrder.cs:934-972): BindDDLNew(..., false) - no placeholder row - then
           SetComboValue(..., ActivateRow: true) activates Rows[0], the first real lookup. */
        bindCombo('#cmbCategoryI', data.categoriesI, 'Id', 'LookupName');
        bindCombo('#cmbCategoryII', data.categoriesII, 'Id', 'LookupName');
        ['#cmbCategoryI', '#cmbCategoryII'].forEach(function (sel) {
            var first = $(sel + ' option').first();
            if (first.length) { $(sel).val(first.val()).trigger('change'); }
        });

        // Customers / Parties with PartyCode & CityName
        bindCustomerCombo(data.customers);

        // Booking Persons with PartyCode & CityName
        bindCombo('#cmbBookingPerson', data.bookingPersons, 'Id', 'ReferencePartyName', '...Select Person...', 'PartyCode', 'CityName');

        // Payment Terms
        bindCombo('#cmbPaymentTerm', data.paymentTerms, 'Id', 'TermsDescription');
        /* desktop binds with insertDefaultRow: true (SaleOrder.cs :1136), which puts a blank row at
           index 0, then NewEntry() activates Rows[2] - the SECOND real term (:744-748). Reproduced
           exactly here rather than defaulting to whatever happens to be first. */
        $('#cmbPaymentTerm').prepend($('<option>', { value: '0', text: '' }));
        var ptOpts = $('#cmbPaymentTerm option');
        $('#cmbPaymentTerm').val(ptOpts.length > 2 ? ptOpts.eq(2).val()
                               : (ptOpts.length > 1 ? ptOpts.eq(1).val() : '0'));
        applyPaymentTermDueDaysRule();
        // Delivery Terms
        /* DeliveryTerms(), SaleOrder.cs :1144-1158, builds this list IN CODE as a fixed
           two-row table: (1,"Load") (2,"Ponch"), with row 0 activated. The endpoint is used when
           it supplies rows, but the desktop's own pair is the fallback - the markup previously
           hard-coded "Load"/"Unload", and "Unload" is not a delivery term this product has. */
        if (data.deliveryTerms && data.deliveryTerms.length) {
            bindCombo('#cmbDeliveryTerm', data.deliveryTerms, 'Id', 'Description');
            /* DeliveryTerms() activates Rows[0], SaleOrder.cs :1154 */
            var dtOpts = $('#cmbDeliveryTerm option');
            if (dtOpts.length) { $('#cmbDeliveryTerm').val(dtOpts.eq(0).val()); }
        } else {
            bindCombo('#cmbDeliveryTerm',
                      [{ Id: 1, Description: 'Load' }, { Id: 2, Description: 'Ponch' }],
                      'Id', 'Description');
            $('#cmbDeliveryTerm').val('1');
        }

        // Order Status
        bindCombo('#cmbOrderStatus', data.orderStatuses, 'Id', 'Description');

        // Commission Agents & Salesmen
        bindCombo('#cmbSalesMan', data.salesMen, 'Id', 'CompanyName', '...Select Any Value...', 'PartyCode', 'CityName');
        bindCombo('#cmbOtherSalesMan', data.otherCommissionAgents, 'Id', 'CompanyName', '...Select Any Value...', 'PartyCode', 'CityName');

        // Commission Types & Commission UOMs
        bindCombo('#cmbCommType', data.commissionTypes, 'Id', 'Description');
        bindCombo('#cmbOtherCommType', data.commissionTypes, 'Id', 'Description');
        bindCombo('#cmbCommUom', data.commissionUoms, 'Id', 'type');
        bindCombo('#cmbOtherCommUom', data.commissionUoms, 'Id', 'type');

        // Detail Line Items
        bindItemCategoryCombo();
        bindItemCombo(data.items);

        // Crop Years
        bindCombo('#lineCropYear', data.cropYears, 'Id', 'CropYear');

        // Job/Lots
        bindCombo('#lineJobLot', data.jobLots, 'Id', 'JobLotDescription');

        // Packing Types
        bindCombo('#linePackType', data.packingTypes, 'Id', 'PackTypeDesc');

        // Cities & Warehouses
        bindCombo('#lineCityArea', data.cities, 'Id', 'CityName');
        bindCombo('#lineWarehouse', data.warehouses, 'Id', 'WareHouseName');

        // Payment Detail & Expense dropdowns
        bindCombo('#payTerm', data.paymentTerms, 'Id', 'TermsDescription', '-- Select Term --');
        bindCombo('#expItem', data.otherItems, 'Id', 'OtherItemName', '-- Select Item --');
        /* CmbCustomerHistory is NOT the party master: HistoryCombosFill (SaleOrder.cs:4012) lists only the
           parties that appear on Sale Orders (doc type 81) in the history branch, from
           USP_GetDataForDropDownFromSaleOrder rows where Activity='Customer'. */
        loadHistoryBranches();

        /* ConfigurationDefault (SaleOrder.cs:786-826) runs once when the form opens and pre-selects the
           configured City/Area, Job/Lot, Packing Type, Crop Year and Warehouse on the detail line. */
        $.get('/sale/sale-order/api/config-defaults', function (c) {
            if (!c) return;
            saleOrderConfig = c;
            /* ERP feature 6: show and fill the currency controls (CurrencyFill :3462). */
            if (c.hasMultiCurrencyFeature) {
                $('.so-fcy').prop('hidden', false);
                $.get('/sale/sale-order/api/currencies', function (rows) {
                    bindCombo('#cmbCurrency', rows || [], 'Id', 'CurrencyCode', '...Select Any Value...');
                });
            }
            /* ItemSearchByCode (:621-629) chooses the item radio; the item combo rebinds on the radio change. */
            if (c.itemSearchByCode) { $('#radCode').prop('checked', true).trigger('change'); }
            /* :751-754 - History From date = today minus DefaultDaysToLessFromHistoryFromDate, else minus 3 days. */
            var back = c.defaultDaysToLessFromHistoryFromDate > 0 ? c.defaultDaysToLessFromHistoryFromDate : 3;
            var fromD = new Date(); fromD.setDate(fromD.getDate() - back);
            $('#histFromDate').val(fromD.toISOString().split('T')[0]);
            [['#lineCityArea', 'cityAreaId'], ['#lineJobLot', 'jobLotId'], ['#linePackType', 'packingTypeId'],
             ['#lineCropYear', 'cropYearId'], ['#lineWarehouse', 'warehouseId']].forEach(function (p) {
                var v = c[p[1]];
                if (v && $(p[0] + ' option[value="' + v + '"]').length) { $(p[0]).val(String(v)).trigger('change'); }
            });
        });
    });
}

/* Desktop combos restored with `.Text = value` select the row whose DISPLAY text equals the value.
   Numeric texts ("40" vs "40.00") are compared as numbers. No match leaves the combo empty, as the
   desktop's Text assignment does. */
function selectOptionByText(selector, value) {
    var want = $.trim(value == null ? '' : String(value));
    var wantNum = parseFloat(want);
    var hit = $(selector + ' option').filter(function () {
        var t = $.trim($(this).text());
        return t === want || (want !== '' && isFinite(wantNum) && isFinite(parseFloat(t)) && parseFloat(t) === wantNum && /^[0-9.\-]+$/.test(t));
    }).first();
    $(selector).val(hit.length ? hit.val() : null).trigger('change');
}

var saleOrderConfig = {};

/* txtExchangeRate_TextChanged (:3566) + CalculateTotalInformation (:3548): each row's FcyAmount is
   Math.Round(Amount / ExchangeRate, DefaultNoOfDecimalPointsForFcyAmount) - banker's rounding - or 0 when the rate
   is not positive; txtFcyAmount is their total. The server recomputes the same figures on save. */
function roundHalfEven(value, decimals) {
    var f = Math.pow(10, decimals), x = value * f, r = Math.round(x);
    if (Math.abs(x % 1) === 0.5) { r = 2 * Math.round(x / 2); }
    return r / f;
}
function recalcFcyAmount() {
    var rate = parseFloat($('#txtExchangeRate').val()) || 0;
    var n = saleOrderConfig.fcyDecimals || 0, total = 0;
    if (rate > 0) {
        currentLineItems.forEach(function (it) {
            if (it.actionTypeId === 3) return;
            total += roundHalfEven((parseFloat(it.amount) || 0) / rate, n);
        });
    }
    $('#txtFcyAmount').val(total.toFixed(n));
}

/* GenerateOrderCategoryNo (SaleOrder.cs:1012): next CatagorySrNo for the chosen Order Category and financial year;
   txtcatsr is only overwritten when the answer is > 0. Called on load (:712), on leaving the category combo (:1597)
   and on reset (:3430). */
function generateOrderCategoryNo() {
    var catId = parseInt($('#cmbOrderCategory').val(), 10) || 0;
    if (!catId) return;
    $.get('/sale/sale-order/api/category-sr-no', { categoryId: catId }, function (r) {
        if (r && r.catagorySrNo > 0) { $('#txtCatNo').val(r.catagorySrNo); }
    });
}

/* combitem_ValueChanged (SaleOrder.cs:1665-1672): Comm On Sale from the item's commission schedule, and the
   available stock weight shown in lblStockWeight ("#,##0.###", "0" when not positive). */
function refreshItemInfo(itemId) {
    if (!itemId) { $('#lblStockWeight').text('0'); return; }
    $.get('/sale/sale-order/api/item-info/' + itemId, { docDate: $('#txtDocDate').val() || '' }, function (r) {
        if (!r) return;
        $('#lineCommOnSale').prop('checked', !!r.commissionOnSale);
        var w = parseFloat(r.stockWeight) || 0;
        $('#lblStockWeight').text(w > 0 ? w.toLocaleString('en-US', { maximumFractionDigits: 3 }) : '0')
                            .attr('data-stock', w);
    });
}

/* HistoryBranchComboFill (:3968): the History tab's branch list, the user's own branch ticked. */
function loadHistoryBranches() {
    $.get('/sale/sale-order/api/history-branches', function (rows) {
        var $box = $('#histBranches').empty();
        (rows || []).forEach(function (b) {
            var id = 'histBranch_' + b.Id;
            $box.append($('<label class="win-radio-inline me-2">').append(
                $('<input type="checkbox" class="hist-branch">').attr('id', id).val(b.Id).prop('checked', b.UserBranch === true),
                ' ' + (b.BranchName || b.Id)));
        });
        loadHistoryCombos();
    });
}
function selectedHistoryBranchIds() {
    return $('#histBranches .hist-branch:checked').map(function () { return this.value; }).get().join(',');
}
/* HistoryCombosFill (:4012): Customer and Booking Person lists for the ticked branches. */
function loadHistoryCombos() {
    $.get('/sale/sale-order/api/history-lookups', { branchIds: selectedHistoryBranchIds() }, function (h) {
        bindCombo('#histCustomerCombo', (h && h.customers) || [], 'Id', 'ReferenceName', '...Select Customer...');
        bindCombo('#histBookingPersonCombo', (h && h.bookingPersons) || [], 'Id', 'ReferenceName', '...Select Booking Person...');
    });
}

function bindCombo(selector, items, valueAttr, textAttr, defaultText, codeAttr, cityAttr) {
    var $el = $(selector).empty();
    if (defaultText) {
        $el.append($('<option>', { value: '', text: defaultText }));
    }
    if (items && items.length > 0) {
        items.forEach(function (item) {
            var opt = $('<option>', {
                value: item[valueAttr],
                text: item[textAttr] || item[valueAttr]
            });
            if (codeAttr && item[codeAttr]) opt.attr('data-code', item[codeAttr]);
            if (cityAttr && item[cityAttr]) opt.attr('data-city', item[cityAttr]);
            $el.append(opt);
        });
    }
    $el.trigger('change');
}

/* combpttrm_Leave, SaleOrder.cs :1643-1655. Due Days is enabled for every payment term except
   PaymentTermsId 1 and 3, where the desktop clears it AND disables it. Term 2 additionally
   requires a non-zero Due Days at save time (:1766), which saveSaleOrder already enforces. */
/* ((UltraGridBase)combruom).Rows[0].Activate(), SaleOrder.cs :3831 / :1210. */
function resetCommUomToFirstRow(selector) {
    var $opts = $(selector + ' option');
    if ($opts.length) { $(selector).val($opts.eq(0).val()).trigger('change.select2'); }
    recalcCommissionAmounts();
}

function applyPaymentTermDueDaysRule() {
    var termId = parseInt($('#cmbPaymentTerm').val(), 10) || 0;
    if (termId === 1 || termId === 3) {
        $('#txtDueDays').val('').prop('disabled', true);
    } else {
        $('#txtDueDays').prop('disabled', false);
    }
}

function bindCustomerCombo(customers) {
    var $el = $('#cmbCustomer').empty();
    $el.append('<option value="">...Select Any Value...</option>');
    if (customers) {
        var searchByCode = $('#radPartyCode').is(':checked');
        customers.forEach(function (c) {
            var label = searchByCode ? ((c.PartyCode || '') + ' - ' + c.CompanyName) : c.CompanyName;
            var opt = $('<option>', { value: c.Id, text: label });
            opt.attr('data-code', c.PartyCode || '');
            opt.attr('data-city', c.CityName || '');
            $el.append(opt);
        });
    }
    $el.trigger('change');
}

/* Case-tolerant field read: these rows come straight from USP_Item_AllItemsWithModal, and the
   proc's own alias for the production stage is lower-cased ("productionStageName" on
   Architecture.Model.Main.getGlobalAllItems). No value is invented - a missing column reads ''. */
function itemField(row, names) {
    for (var i = 0; i < names.length; i++) {
        if (row[names[i]] !== undefined && row[names[i]] !== null) return String(row[names[i]]);
    }
    var keys = Object.keys(row);
    for (var j = 0; j < names.length; j++) {
        for (var k = 0; k < keys.length; k++) {
            if (keys[k].toLowerCase() === names[j].toLowerCase() &&
                row[keys[k]] !== undefined && row[keys[k]] !== null) return String(row[keys[k]]);
        }
    }
    return '';
}

/* ItemdtFillFromAll / CategoryOrTypeFill, SaleOrder.cs :1229 and :1278 - both start from the global
   item list with ItemTypeOfTypeId 14 and 17 excluded. The web was binding every item, unfiltered. */
function saleOrderSelectableItems() {
    var items = (masterLookupsData && masterLookupsData.items) || [];
    return items.filter(function (i) {
        var t = parseInt(itemField(i, ['ItemTypeOfTypeId']) || '0', 10) || 0;
        return t !== 14 && t !== 17;
    });
}

/* CategoryOrTypeFill(), SaleOrder.cs :1225-1270: distinct (ItemCategoryId, ItemCategory) when the
   Category radio is on, distinct (ItemTypeId, ItemType) when Type is on, no blank row
   (insertDefaultRow: false). This combo did not exist on the web at all. */
function bindItemCategoryCombo() {
    var byType = $('#radType').is(':checked');
    var idKey = byType ? 'ItemTypeId' : 'ItemCategoryId';
    var txtKey = byType ? 'ItemType' : 'ItemCategory';
    var previous = $('#lineItemCategory').val();
    var seen = {}, rows = [];
    saleOrderSelectableItems().forEach(function (i) {
        var text = itemField(i, [txtKey]);
        if (!text) return;
        if (seen[text]) return;
        seen[text] = true;
        rows.push({ id: itemField(i, [idKey]), text: text });
    });
    var $el = $('#lineItemCategory').empty();
    $el.append($('<option>', { value: '', text: '' }));
    rows.forEach(function (r) { $el.append($('<option>', { value: r.id, text: r.text })); });
    if (previous && $el.find('option[value="' + previous + '"]').length) { $el.val(previous); }
    $el.trigger('change');
}

function bindItemCombo(items) {
    var $el = $('#lineItem').empty();
    /* ZeroIndex: true on the desktop binding (:1314) - the first row is the filter/blank row. */
    $el.append('<option value="">...Select Any Value...</option>');
    var searchByCode = $('#radCode').is(':checked');
    var byType = $('#radType').is(':checked');
    var filterId = parseInt($('#lineItemCategory').val() || '0', 10) || 0;
    var source = (items && items.length) ? items : saleOrderSelectableItems();
    source.forEach(function (i) {
        var typeOfType = parseInt(itemField(i, ['ItemTypeOfTypeId']) || '0', 10) || 0;
        if (typeOfType === 14 || typeOfType === 17) return;               /* :1229 */
        if (filterId > 0) {                                                /* :1293-1296 */
            var own = parseInt(itemField(i, [byType ? 'ItemTypeId' : 'ItemCategoryId']) || '0', 10) || 0;
            if (own !== filterId) return;
        }
        var itemName = itemField(i, ['ItemName']);
        var itemCode = itemField(i, ['ItemCode']);
        /* The desktop swaps the DISPLAY MEMBER between ItemName and ItemCode (:1314 vs :1318);
           it never concatenates the two into one label. */
        $el.append($('<option>', {
            value: itemField(i, ['Id']),
            text: searchByCode ? itemCode : itemName,
            'data-code': itemCode,
            'data-name': itemName,
            'data-category': itemField(i, ['ItemCategory']),
            'data-type': itemField(i, ['ItemType']),
            'data-stage': itemField(i, ['productionStageName', 'ProductionStage', 'ProductionStageName'])
        }));
    });
    $el.trigger('change'); // refresh select2 (".so-select2") after rebuilding options - see bindCombo()
}

// =========================================================
// 2. EVENT LISTENERS & DEPENDENT CALCULATIONS
// =========================================================
function setupEventListeners() {
    /* combordercat_Leave (:1597) regenerates the category serial number. */
    $('#cmbOrderCategory').on('change', generateOrderCategoryNo);
    $('#txtExchangeRate').on('input change', recalcFcyAmount);
    /* Ticking branches re-reads the history party / booking-person lists (btnRefreshHistory_Click :4090). */
    $(document).on('change', '#histBranches .hist-branch', loadHistoryCombos);
    // Party Code vs Name radio toggle
    $('input[name="radParty"]').change(function () {
        if (masterLookupsData.customers) bindCustomerCombo(masterLookupsData.customers);
    });

    /* Category/Type radio rebuilds the category combo first, then the item list it filters
       (desktop RadCategory/RadType -> CategoryOrTypeFill() then ItemdtFillFromAll()). The
       Name/Code radio only re-labels the item list. */
    $('input[name="radItemFilter"]').change(function () {
        bindItemCategoryCombo();
        bindItemCombo(null);
    });
    $('input[name="radItemSelect"]').change(function () {
        bindItemCombo(null);
    });
    $('#lineItemCategory').on('change', function () {
        bindItemCombo(null);
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
        refreshItemInfo(itemId);
        if (!itemId) return;
        /* bindRateUomAndItemPackUom (SaleOrder.cs:1339) keeps the Pack and Rate UOM TEXT the user had
           before the item changed, and re-selects it when the new item's schedule has the same code. */
        var prevRateText = $.trim($('#lineRateUom option:selected').text());
        var prevPackText = $.trim($('#linePackUom option:selected').text());
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
                var uomsWithoutFactor = [];
                uoms.forEach(function (u) {
                    // The desktop's conversion factor is the UOM schedule's **Equivalent**
                    // column, not QtyEquivalent. CommonServices.GetUomScheduleByItemId
                    // (Architecture.WinApp.Common/CommonServices.cs:5239) builds the bound
                    // table as Id, UOMCode, Equivalent, QtyEquivalent, BaseRateUom - and
                    // SaleOrder.cs's CalculateWeight()/TotalAmount() read Cells[2], i.e.
                    // Equivalent. dbo.UOMSchedule carries both columns (view
                    // V_UomScheduleAndUom), so they are different numbers and using
                    // QtyEquivalent here silently produced wrong weights and amounts.
                    // A missing Equivalent is never replaced with QtyEquivalent, 1 or 0 -
                    // the option carries no factor and the screen says so.
                    var eq = (u.Equivalent === undefined || u.Equivalent === null || u.Equivalent === '')
                        ? null : parseFloat(u.Equivalent);
                    if (eq === null || !isFinite(eq) || eq <= 0) {
                        uomsWithoutFactor.push(u.UOMCode);
                        eq = null;
                    }
                    var attrs = {
                        value: u.Id,
                        text: u.UOMCode,
                        'data-uom': u.UOMCode || '',
                        /* No data-equivalent at all when the schedule has no usable Equivalent.
                           This used to fall back to the string '1', which parseFloat turned into a
                           real factor of 1 - exactly what the comment above says must never happen,
                           and what CalculateWeight() :3620 refuses to do (it zeroes Qty and Weight
                           instead). QtyEquivalent is a DIFFERENT column and is never a substitute
                           for Equivalent, so it is carried for display only. */
                        'data-qty-eq': u.QtyEquivalent != null ? u.QtyEquivalent : '',
                        /* Both dropdowns are bound to the SAME dtUom, so both show the schedule's
                           BaseRateUom column - the desktop's Pack Size dropdown is headed
                           "BaseRateUom" too. BasePackUom only drives which row Pack Size defaults
                           to (below); it is not what this column displays. */
                        'data-base-rate': (u.BaseRateUom === true || u.BaseRateUom === 1) ? '1' : '0'
                    };
                    if (eq !== null) { attrs['data-equivalent'] = eq; }   /* absent when null */
                    $uom.append($('<option>', attrs));
                    $packUom.append($('<option>', $.extend({}, attrs)));
                    if ((u.BaseRateUom === true || u.BaseRateUom === 1) && baseUomCode === null) {
                        baseUomCode = u.Id;   /* GetBaseRateUomId returns the FIRST such row (CommonServices:5316, break) */
                    }
                    if (u.BasePackUom === true || u.BasePackUom === 1) {
                        basePackUomCode = u.Id;
                    }
                });
                if (uomsWithoutFactor.length) {
                    var warn = 'These UOMs have no usable Equivalent from '
                        + '/sale/sale-order/api/item-uoms/' + itemId + ': '
                        + uomsWithoutFactor.join(', ')
                        + '. Weight and Amount cannot be calculated for them, and no '
                        + 'substitute factor is used.';
                    if (typeof console !== 'undefined' && console.warn) { console.warn(warn); }
                    if (typeof showMessage === 'function') { showMessage(warn, 'error'); }
                    else { alert(warn); }
                }
                /* Desktop defaults, SaleOrder.cs:1355-1387 - ported rule for rule:
                   1. Pack and Rate UOM keep the previous text if that code exists for the new item,
                      otherwise they are blank. Pack UOM is NEVER auto-defaulted (the old web code
                      defaulted it to BasePackUom/BaseRateUom; the desktop has no such rule).
                   2. On a new line (not an edit of a grid row), when Rate UOM is empty or is not a
                      40-Equivalent row: take GetBaseRateUomId (first BaseRateUom row) if THAT row is
                      40-Equivalent, else the first 40-Equivalent row.
                   3. A Rate UOM whose Equivalent is not 40 is cleared. */
                function optionByText($sel, txt) {
                    if (!txt) return null;
                    var hit = $sel.find('option').filter(function () { return $.trim($(this).text()) === txt; }).first();
                    return hit.length ? hit.val() : null;
                }
                function eqOf(id) {
                    var u = uoms.filter(function (x) { return String(x.Id) === String(id); })[0];
                    return u ? parseFloat(u.Equivalent) : NaN;
                }
                var keptPack = optionByText($packUom, prevPackText);
                var keptRate = optionByText($uom, prevRateText);
                $packUom.val(keptPack !== null ? keptPack : null);
                $uom.val(keptRate !== null ? keptRate : null);
                if (!pendingLineUomSelection) {
                    var uom40Ids = uoms.filter(function (x) { return parseFloat(x.Equivalent) === 40; })
                                       .map(function (x) { return String(x.Id); });
                    var rateVal = $uom.val();
                    if (!rateVal || uom40Ids.indexOf(String(rateVal)) < 0) {
                        if (baseUomCode !== null && uom40Ids.indexOf(String(baseUomCode)) >= 0) {
                            $uom.val(baseUomCode);
                        } else if (uom40Ids.length > 0) {
                            $uom.val(uom40Ids[0]);
                        }
                    }
                    if ($uom.val() && eqOf($uom.val()) !== 40) {
                        $uom.val(null);
                    }
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

    $('#cmbPaymentTerm').on('change', applyPaymentTermDueDaysRule);

    /* combcommtype_Leave, SaleOrder.cs :3827-3838, does TWO things: recalculate, and
       ((UltraGridBase)combruom).Rows[0].Activate() - changing Comm Type snaps the Uom back to the
       first row. Only the recalculation was ported before. */
    $('#cmbCommType').on('change', function () { resetCommUomToFirstRow('#cmbCommUom'); });
    $('#cmbOtherCommType').on('change', function () { resetCommUomToFirstRow('#cmbOtherCommUom'); });

    /* combsalesman_Leave (:1613-1617) and CmbOtherCommissionAgent_Leave (:3932-3936) enable the
       whole commission block once an agent is chosen. Only txtcommamount (:8417) and
       txtOtherCommissionAmount (:8380) start disabled, so only those two are toggled here. */
    $('#cmbSalesMan').on('change', function () {
        $('#txtCommAmount').prop('disabled', !parseInt($(this).val() || '0', 10));
    });
    $('#cmbOtherSalesMan').on('change', function () {
        $('#txtOtherCommAmount').prop('disabled', !parseInt($(this).val() || '0', 10));
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
/* Returns a combo's Equivalent, or null when it has none. Never substitutes 1 or 0:
   CalculateWeight() :3617 reads Cells[2] only when a row is active and the value is > 0, and
   treats anything else as "no factor". */
function uomFactor(selectorId) {
    var opt = $(selectorId + ' option:selected');
    if (!opt.length || !opt.val()) return null;
    var raw = opt.attr('data-equivalent');
    if (raw === undefined || raw === null || raw === '') return null;
    var n = parseFloat(raw);
    return (isFinite(n) && n > 0) ? n : null;
}

/* .NET's Math.Round(x, 0, MidpointRounding.AwayFromZero) - JS Math.round is half-UP, which
   differs from half-away-from-zero on negatives. TotalAmount() :3674. */
function roundAwayFromZero(n) {
    return n < 0 ? -Math.round(-n) : Math.round(n);
}

/* CalculateWeight(), SaleOrder.cs :3611-3652, and TotalAmount(), :3660-3676.
 *
 * Qty and Net Weight are two views of one line joined by the Pack UOM's Equivalent. When there is
 * no usable factor the desktop ZEROES BOTH (:3620-3624) rather than leaving numbers that were
 * computed from an assumption. */
function calcLine(source) {
    var packEquivalent = uomFactor('#linePackUom');

    if (packEquivalent === null) {
        $('#lineQty').val('0');          /* :3622 */
        $('#lineWeight').val('0');       /* :3623 */
        $('#lineAmount').val('0');
        return;
    }

    var qty = parseFloat($('#lineQty').val()) || 0;
    var weight = parseFloat($('#lineWeight').val()) || 0;

    if (source === 'weight') {
        qty = weight > 0 ? weight / packEquivalent : 0;      /* :3635 */
        $('#lineQty').val(qty.toFixed(3));
    } else {
        weight = qty > 0 ? qty * packEquivalent : 0;         /* :3627 */
        $('#lineWeight').val(weight.toFixed(3));
    }

    var rate = parseFloat($('#lineRate').val()) || 0;
    var rateEquivalent = uomFactor('#lineRateUom');

    /* TotalAmount() :3667 - all three must be present, else the amount stays 0 */
    var amount = (weight > 0 && rateEquivalent !== null && rate > 0)
        ? roundAwayFromZero((weight / rateEquivalent) * rate)
        : 0;
    $('#lineAmount').val(amount.toFixed(4));

    recalcCommissionAmounts();
}

function btnAddRow_Click() {
    /* btnplus_Click (:1905-1908) + FormValidationDetail (:1885-1890): with config
       CheckStockAndGiveWarningMessageOnSaleOrder on, an item with no stock and no detail remarks asks to continue,
       and then the detail validation still insists on remarks. */
    var stockNow = parseFloat($('#lblStockWeight').attr('data-stock') || $('#lblStockWeight').text().replace(/,/g, '')) || 0;
    if (saleOrderConfig.checkStockAndGiveWarningMessageOnSaleOrder && stockNow <= 0 && !($('#lineRemarks').val() || '').trim()) {
        if (!confirm('No stock is available for this item. Do you want to continue adding it?')) return;
        alert('Remarks Field is Required');
        $('#lineRemarks').focus();
        return;
    }
    var itemId = $('#lineItem').val();
    var itemName = $('#lineItem option:selected').text();
    var itemCode = $('#lineItem option:selected').attr('data-code') || '';
    var cropYearId = $('#lineCropYear').val() || '';
    var cropYear = $('#lineCropYear option:selected').text() || '';
    var jobLot = $('#lineJobLot option:selected').text() || '';
    var packType = $('#linePackType option:selected').text() || '';
    var qty = parseFloat($('#lineQty').val()) || 0;
    var weight = parseFloat($('#lineWeight').val()) || 0;
    var packUomId = $('#linePackUom').val() || '';
    var packUom = $('#linePackUom option:selected').text() || '';
    /* desktop has ONE control here: combitempck, label36 "Pack Size"; the grid column it fills
       is PackUom with caption "Pack Size" (SaleOrder.cs :2066). Pack Size is that combo's text,
       so it is read AFTER packUom - never before it. */
    var packSize = packUom;
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

    var jobLotIdVal = $('#lineJobLot').val() || '';
    var packTypeIdVal = $('#linePackType').val() || '';

    /* FormValidationDetail(), SaleOrder.cs :1817-1893 - ten checks, in this order, with the
       product's own wording. The previous version merged Item with Qty, reworded the rest using
       the SAVE-time messages (":2932 ... not found in detail grid"), dropped the Item Rate and
       Rate UOM checks entirely, and made the Amount check conditional on rate > 0. */
    if (!itemId || parseInt(itemId, 10) === 0) { alert('Item Name Field is Required'); return; }   /* :1819 */
    if (!cropYearId)    { alert('Crop Year Field is Required'); return; }                          /* :1825 */
    if (!jobLotIdVal)   { alert('Job Lot Field is Required'); return; }                            /* :1831 */
    if (!packTypeIdVal) { alert('Pack Type Field is Required'); return; }                          /* :1837 */
    if (!packUomId)     { alert('Pack Uom Field is Required'); return; }                           /* :1843 */
    if (qty <= 0)       { alert('Qty Field is Required'); return; }                                /* :1849 */
    if (weight <= 0)    { alert('Net Weight Field is Required'); return; }                         /* :1855 */
    if (rate <= 0)      { alert('Item Rate Field is Required'); return; }                          /* :1861 */
    if (!rateUomId)     { alert('Rate UOM Field is Required'); return; }                           /* :1867 */
    if (amount <= 0)    { alert('Item amount Field is Required'); return; }                        /* :1873 */

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
        tbody.append('<tr><td colspan="18" class="text-center text-muted" style="padding: 12px;">No order line items added yet. Record: 0 of 0</td></tr>');
        recalcTotals();
    renderDetailTotals();
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
            <td>${item.rateUom}</td>
            <td class="text-end">${item.rate.toFixed(2)}</td>
            <td class="text-end amt-val">${item.amount.toFixed(2)}</td>
            <td class="text-end">${item.bagPrice.toFixed(2)}</td>
            <td class="text-end">${item.weightCut.toFixed(2)}</td>
            <td>${item.cityArea || ''}</td>
            <td>${item.warehouse || ''}</td>
            <td>${item.labSample || ''}</td>
            <td>${item.remarks || ''}</td>
        </tr>`;
        tbody.append(tr);
    });

    recalcTotals();
    renderDetailTotals();
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

/* Grid totals the commission formulas read, matching grd.GetTotal(..., AggregateFunction.Sum). */
function gridTotal(field) {
    var t = 0;
    currentLineItems.forEach(function (item) { t += (parseFloat(item[field]) || 0); });
    return t;
}

function gridAverage(field) {
    if (!currentLineItems.length) return 0;
    return gridTotal(field) / currentLineItems.length;
}

/* Detail grid aggregate row. SaleOrder.cs :2067-2091 sets AggregateFunction 2 on QTY, Weight,
   BagRate, BagWeight and Amount, and AggregateFunction 3 on Rate. Across this code base only two
   numeric values are ever used and the named forms appear as AggregateFunction.Sum (8,475 uses)
   and AggregateFunction.Average (251), matching the numeric counts for 2 and 3 exactly - and the
   columns given 3 are Rate / AvgRate / AvgRate40kg. So 2 = Sum, 3 = Average.
   The web grid had no total row at all. */
function renderDetailTotals() {
    var f2 = function (n) { return (isFinite(n) ? n : 0).toLocaleString(undefined,
        { minimumFractionDigits: 2, maximumFractionDigits: 2 }); };
    $('#totQty').text(f2(gridTotal('quantity')));
    $('#totNetWeight').text(f2(gridTotal('weight')));
    $('#totRate').text(f2(gridAverage('rate')));
    $('#totAmount').text(f2(gridTotal('amount')));
    $('#totBagRate').text(f2(gridTotal('bagPrice')));
    $('#totBagWeight').text(f2(gridTotal('weightCut')));
}

/* TotalCommissionAmount(), SaleOrder.cs :3758-3796.
 *
 * Three branches keyed on the combo's TEXT, not its id. Note two details that are easy to lose:
 *   - an empty Rate returns EARLY and leaves the amount untouched (:3762) - it does not zero it;
 *   - "Comm Weight" divides by the Comm Uom combo's TEXT parsed as a number (:3781), because the
 *     GetCommissionUom list's text is the divisor itself, not a lookup id.
 * None of this existed on the web: the Amount box was free-typed and never recomputed. */
function commissionAmountFor(typeSelector, rateSelector, uomSelector, amountSelector, zeroWhenNoRate) {
    var rateText = String($(rateSelector).val() === undefined ? '' : $(rateSelector).val()).trim();

    if (rateText === '') {
        /* TotalOtherCommissionAmount has an explicit else that zeroes (:3895); the regular one
           simply returns (:3762-3765). */
        if (zeroWhenNoRate) $(amountSelector).val('0');
        return;
    }

    var type = ($(typeSelector + ' option:selected').text() || '').trim();
    var rate = parseFloat(rateText) || 0;

    if (type === 'Flat') {                                          /* :3766 */
        $(amountSelector).val(String(rate));
        return;
    }
    if (type === 'Percent') {                                       /* :3771 */
        var amt = gridTotal('amount') * rate / 100.0;
        $(amountSelector).val(String(roundAwayFromZero(amt)));
        return;
    }
    if (type === 'Comm Weight') {                                   /* :3781 */
        var uomText = ($(uomSelector + ' option:selected').text() || '').trim();
        var m = /-?\d+(?:\.\d+)?/.exec(uomText);
        var uom = m ? parseFloat(m[0]) : 0;
        var weight = gridTotal('weight');
        if (weight > 0 && rate > 0 && uom > 0) {
            $(amountSelector).val(String(roundAwayFromZero(weight / uom * rate)));
        } else {
            $(amountSelector).val('0');                              /* :3791 */
        }
    }
}

function recalcCommissionAmounts() {
    commissionAmountFor('#cmbCommType', '#txtCommRate', '#cmbCommUom', '#txtCommAmount', false);
    commissionAmountFor('#cmbOtherCommType', '#txtOtherCommRate', '#cmbOtherCommUom', '#txtOtherCommAmount', true);
}

/* CalculateTotalInformation(), SaleOrder.cs :3529-3557 */
function recalcTotals() {
    var totalQty = gridTotal('quantity');
    var totalWt = gridTotal('weight');
    var totalAmt = gridTotal('amount');

    /* Commission must be recomputed from the new grid totals BEFORE the deduction below reads it. */
    recalcCommissionAmounts();

    /* :3539-3546 - when the customer IS the commission agent, the order amount is taken NET of
       that commission. Absent from the web before, so the order amount (and therefore the net
       recoverable) was overstated whenever a party acted as its own agent. */
    var customerId = parseInt($('#cmbCustomer').val() || '0', 10);

    var salesManId = parseInt($('#cmbSalesMan').val() || '0', 10);
    var commAmount = parseFloat($('#txtCommAmount').val()) || 0;
    if (salesManId > 0 && commAmount > 0 && customerId > 0 && customerId === salesManId) {
        totalAmt -= commAmount;
    }

    var otherAgentId = parseInt($('#cmbOtherSalesMan').val() || '0', 10);
    var otherAmount = parseFloat($('#txtOtherCommAmount').val()) || 0;
    if (otherAgentId > 0 && otherAmount > 0 && customerId > 0 && customerId === otherAgentId) {
        totalAmt -= otherAmount;
    }

    $('#txtOrderQty').val(totalQty.toFixed(2));      /* :3547 Math.Round(,2) */
    $('#txtOrderWeight').val(totalWt.toFixed(2));    /* :3548 */
    $('#txtCurrentOrder').val(totalAmt.toFixed(2));  /* :3549 txtOrderAmount */
    recalcFcyAmount();

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

    /* FormValidation(), SaleOrder.cs :1726-1813 - in this order, with the product's own wording.
       The web previously had only two checks here, both reworded. The multi-currency checks
       (:1780-1811) are NOT reproduced: this screen has no Fcy Code / Exchange Rate / Fcy Amount
       controls at all, so there is nothing to validate - see the note in the progress document. */
    if (!$('#txtDocNo').val() || parseInt($('#txtDocNo').val(), 10) === 0) {
        alert('Doc No Field is Required'); return;                                    /* :1728 */
    }
    if (!$('#cmbOrderCategory').val() || parseInt($('#cmbOrderCategory').val(), 10) === 0) {
        alert('Order Category Field is Required'); return;                            /* :1734 */
    }
    /* :1741 / :1748 - required only when the combo actually has rows to choose from. */
    if ($('#cmbCategoryI option').length > 1 && !$('#cmbCategoryI').val()) {
        alert('Category-I Field is Required'); return;
    }
    if ($('#cmbCategoryII option').length > 1 && !$('#cmbCategoryII').val()) {
        alert('Category-II Field is Required'); return;
    }
    if (!custId || parseInt(custId, 10) === 0) {
        alert('Customer Name Field is Required'); return;                             /* :1754 */
    }
    if (!$('#cmbPaymentTerm').val() || parseInt($('#cmbPaymentTerm').val(), 10) === 0) {
        alert('Payment Term Field is Required'); return;                              /* :1760 */
    }
    /* :1766 - payment term 2 is Credit; due days are then mandatory. */
    if (parseInt($('#cmbPaymentTerm').val(), 10) === 2 && (parseInt($('#txtDueDays').val(), 10) || 0) === 0) {
        alert('Due Days Field is Required'); return;
    }
    if (!$('#cmbDeliveryTerm').val() || parseInt($('#cmbDeliveryTerm').val(), 10) === 0) {
        alert('Delivery Term Field is Required'); return;                             /* :1772 */
    }

    if (currentLineItems.length === 0) {
        alert('Grid Record Not Found'); return;                                       /* Insert() :2772 */
    }

    /* Insert() :2799-2828 - the commission cross-checks, in the desktop's order. */
    var commAgent = parseInt($('#cmbSalesMan').val() || '0', 10);
    var commRateV = parseFloat($('#txtCommRate').val()) || 0;
    var commAmtV = parseFloat($('#txtCommAmount').val()) || 0;
    if (commAgent === 0 && (commAmtV > 0 || commRateV > 0)) {
        alert('Please Select Commission Agent Required when Commission Amount or Rate is Present'); return;
    }
    if (commAgent > 0 && commAmtV <= 0) {
        alert('Commission Amount Required when Commission Agent is Selected'); return;
    }
    if (commAgent > 0 && commRateV <= 0) {
        alert('Commission Rate Required when Commission Agent is Selected'); return;
    }

    var oAgent = parseInt($('#cmbOtherSalesMan').val() || '0', 10);
    var oRateV = parseFloat($('#txtOtherCommRate').val()) || 0;
    var oAmtV = parseFloat($('#txtOtherCommAmount').val()) || 0;
    if (oAgent === 0 && (oAmtV > 0 || oRateV > 0)) {
        alert('Please Select OtherCommission Agent when OtherCommission Amount or Rate is Present'); return;
    }
    if (oAgent > 0 && oAmtV <= 0) {
        alert('OtherCommission Amount Required when OtherCommission Agent is Selected'); return;
    }
    if (oAgent > 0 && oRateV <= 0) {
        alert('OtherCommission Rate Required when OtherCommission Agent is Selected'); return;
    }

    /* Insert() :2988 - a detail row with no rate is refused outright. The web used to accept
       rate-0 rows straight into the payload. */
    for (var ri = 0; ri < currentLineItems.length; ri++) {
        if (!(parseFloat(currentLineItems[ri].rate) > 0)) {
            alert('Rate Field Required'); return;
        }
    }

    /* FormValidation (:1778-1797) with ERP feature 6. */
    if (saleOrderConfig.hasMultiCurrencyFeature) {
        if (!(parseInt($('#cmbCurrency').val(), 10) > 0)) { alert('Fcy Code Field is Required'); $('#cmbCurrency').focus(); return; }
        var xr = ($('#txtExchangeRate').val() || '').trim();
        if (xr === '' || xr === '0') { alert('Exchange Rate Field is Required'); $('#txtExchangeRate').focus(); return; }
        var fa = ($('#txtFcyAmount').val() || '').trim();
        if (fa === '' || parseFloat(fa) === 0) { alert('Fcy Amount Rate Field is Required'); $('#txtFcyAmount').focus(); return; }
    }

    /* Insert() :3138 - the party-limit confirmation. The desktop blocks the save unless the user
       agrees; the web used to save silently over the limit. */
    var netRec = parseFloat($('#txtNetRecoverable').val()) || 0;
    if (netRec > 0) {
        var msg = 'Party Balance Exceeds Define Limit After This Order\n'
                + 'GlAmount value (Before this Order) is ' + ($('#txtPartyGlAmount').val() || '0') + '\n'
                + 'OutstandingOrderAmount value is ' + ($('#txtOutstandingOrder').val() || '0') + '\n'
                + 'Current OrderAmount value is ' + ($('#txtCurrentOrder').val() || '0') + '\n'
                + 'Party limit value is ' + ($('#txtPartyLimit').val() || '0') + '\n\n'
                + 'And NetRecoverableAmount value Will be ' + netRec + '\n'
                + 'are you Sure to Proceed';
        if (!confirm(msg)) return;
    }

    /* Insert() :3142-3144 - SaleOrder.CheckOrderExist: USP_SaleOrderValidations on the FIRST detail line (the
       procedure always returns a row, so the BLL's loop stops there). A warning must be confirmed to save. */
    var firstLine = currentLineItems[0];
    if (firstLine) {
        var warn = '';
        $.ajax({
            url: '/sale/sale-order/api/order-exists-warning', type: 'POST', async: false,
            contentType: 'application/json',
            data: JSON.stringify({ id: currentEditingOrderId || 0, docDate: $('#txtDocDate').val(),
                                   customerId: parseInt(custId, 10) || 0, itemId: parseInt(firstLine.itemId, 10) || 0,
                                   qty: firstLine.quantity, rate: firstLine.rate, amount: firstLine.amount }),
            success: function (r) { warn = (r && r.warning) || ''; },
            error: function (xhr) { warn = '\u0000' + (xhr.responseText || 'Order validation failed'); }
        });
        if (warn.charAt(0) === '\u0000') { alert(warn.substring(1)); return; }
        if (warn && !confirm(warn)) return;
    }

    /* Insert() :2783 / :2790 */
    if (!confirm(currentEditingOrderId ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;

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
        deliveryDays: ($('#txtDeliveryDays').val() || '').trim() === ''
                      ? null : (parseInt($('#txtDeliveryDays').val(), 10) || 0),
        orderCategoryId: parseInt($('#cmbOrderCategory').val()) || null,
        categoryI_Id: parseInt($('#cmbCategoryI').val()) || null,
        categoryII_Id: parseInt($('#cmbCategoryII').val()) || null,
        customerId: parseInt(custId),
        partyRefNo: $('#txtPartyRefNo').val(),
        bookingPersonId: parseInt($('#cmbBookingPerson').val()) || null,
        paymentTermId: parseInt($('#cmbPaymentTerm').val()) || null,
        deliveryTermId: parseInt($('#cmbDeliveryTerm').val()) || null,
        /* Insert() :2901 - po.OrderStatus = "Open" is a hard constant; the desktop's CmbStatus
           combo is a filter control the save ignores. This line used to read #cmbOrderStatus,
           an element that does not exist on this page, and land on 1 by accident. */
        orderStatus: 'Open',
        orderStatusId: 1,
        /* BranchesId comes from the signed-in user server-side (desktop BranchSrNoFill uses
           UserAccount.BranchesId, SaleOrder.cs :917). A client-supplied branch is not trusted. */
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

        /* Header fields the desktop writes in Insert() that this payload did not carry.
           Sending them is what makes the saved row match the desktop's. */
        catagorySrNo: parseInt($('#txtCatNo').val(), 10) || 0,        /* :2845 txtcatsr */
        orderCategoryNo: parseInt($('#txtCatNo').val(), 10) || 0,     /* the DTO field the service writes to @CatagorySrNo */
        /* :2861/:2865 - sent only with ERP feature 6; without it the server applies GetBaseCurrencyAndRate. */
        currencyId: saleOrderConfig.hasMultiCurrencyFeature ? (parseInt($('#cmbCurrency').val(), 10) || null) : null,
        exchangeRate: saleOrderConfig.hasMultiCurrencyFeature ? (parseFloat($('#txtExchangeRate').val()) || null) : null,
        isValidate: $('#chkValidateDO').is(':checked'),               /* :2860 ChkIsValidateOrder */
        locationTypeId: parseInt($('#cmbLocationType').val(), 10) || 0, /* :2980 per detail row */
        orderQty: parseFloat($('#txtOrderQty').val()) || 0,           /* :2862 */
        orderWeight: parseFloat($('#txtOrderWeight').val()) || 0,     /* :2863 */
        orderAmount: parseFloat($('#txtCurrentOrder').val()) || 0,    /* :2864 txtOrderAmount */
        /* :2857 stores the delivery term's TEXT, not its id - both are sent so the server can
           write whichever column the procedure expects. */
        deliveryTerm: $('#cmbDeliveryTerm option:selected').text() || '',
        /* :2873 UomScheduleIdCmRate is the Comm Uom combo's TEXT parsed as a number, not its id. */
        commUomValue: parseFloat(($('#cmbCommUom option:selected').text() || '').replace(/[^0-9.\-]/g, '')) || 0,
        /* :2886-2893 the Other Commission Uom is written ONLY for "Comm Weight", else 0. */
        otherCommUomValue: (($('#cmbOtherCommType option:selected').text() || '').trim() === 'Comm Weight')
            ? (parseFloat(($('#cmbOtherCommUom option:selected').text() || '').replace(/[^0-9.\-]/g, '')) || 0)
            : 0,

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
    /* gridhistoryfill (SaleOrder.cs:4122-4246): one date pair chosen by the radio, each end only when its box is
       ticked; doc-no range; customer; booking person; the ticked branches (required - "Select branch first"). */
    var branchIds = selectedHistoryBranchIds();
    if (!branchIds) { alert('Select branch first'); return; }
    var query = $.param({
        dateMode: $('input[name="histDateMode"]:checked').val() || 'doc',
        fromDate: $('#histUseFrom').is(':checked') ? ($('#histFromDate').val() || '') : '',
        toDate: $('#histUseTo').is(':checked') ? ($('#histToDate').val() || '') : '',
        fromDocNo: $('#histFromDocNo').val() || '',
        toDocNo: $('#histToDocNo').val() || '',
        customerId: $('#histCustomerCombo').val() || '',
        bookingPersonId: $('#histBookingPersonCombo').val() || '',
        branchIds: branchIds
    });
    var tbody = $('#tblHistory tbody').empty();
    $.get('/sale/sale-order/api/history-search?' + query, function (data) {
        if (!data || data.length === 0) {
            tbody.append('<tr><td colspan="30" class="text-center text-muted" style="padding:15px;">No history records found.</td></tr>');
            return;
        }
        data.forEach(function (item) {
            function c(v) { return '<td>' + escapeHtml(v == null ? '' : String(v)) + '</td>'; }
            function n(v) { return '<td class="text-end">' + (v == null || v === '' ? '' : (parseFloat(v) || 0).toFixed(2)) + '</td>'; }
            var tr = '<tr data-id="' + item.Id + '" ondblclick="loadOrderIntoForm(' + item.Id + ')">'
                + '<td class="text-center"><button class="btn btn-sm btn-primary p-0 px-2" onclick="loadOrderIntoForm(' + item.Id + ')">Edit</button></td>'
                + '<td class="text-center"><button class="btn btn-sm btn-warning p-0 px-2" onclick="currentEditingOrderId=' + item.Id + ';btnPrintReport(\'273\')">Print</button></td>'
                + '<td><button type="button" class="voucher-link" onclick="loadOrderIntoForm(' + item.Id + ')">' + escapeHtml(String(item.DocNo || '')) + '</button></td>'
                + c(item.BranchSrNo) + c(item.BranchName) + c(formatDateValue(item.DocDate)) + c(item.CustomerName)
                + c(item.BookingPerson) + c(item.DueDays) + c(formatDateValue(item.DueDate)) + c(formatDateValue(item.OrderExpiryDate))
                + c(item.CommissionType) + n(item.CommRate) + n(item.CommAmount) + c(item.CommissionRemarks)
                + c(item.DeliveryTerm) + c(item.DeliveryDays) + c(item.TermsDescription) + c(item.OrderStatus)
                + c(item.OrderType) + c(item.OtherCategory)
                + c((item.IsAproved === true || item.IsAproved === 1) ? 'Approved' : 'Not Approved')
                + c(item.UserName) + c(formatDateValue(item.EntryDate)) + c(item.ModifyUserName) + c(formatDateValue(item.ModifyDate))
                + c(item.ApprovedUser) + c(formatDateValue(item.PostDate)) + c(item.NoOfAttachments) + c(item.RemarksHeader)
                + '</tr>';
            tbody.append(tr);
        });
    }).fail(function (xhr) {
        tbody.append('<tr><td colspan="30" class="text-center text-danger">' + escapeHtml(xhr.responseText || 'History search failed') + '</td></tr>');
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
        $('#txtCatNo').val(data.CatagorySrNo != null ? data.CatagorySrNo : '');   /* ReadById :3225 */
        if (saleOrderConfig.hasMultiCurrencyFeature) {                              /* :3238 */
            if (data.CurrencyId != null) $('#cmbCurrency').val(String(data.CurrencyId)).trigger('change');
            $('#txtExchangeRate').val(data.ExchangeRate != null ? data.ExchangeRate : '');
        }
        /* cmbOrderCategory1.Value = po.OrderTypeId / cmbOrderCategory2.Value = po.OtherCategoryId (SaleOrder.cs:3219-3223) -
           the load never set them, so an edited order showed the first lookup instead of its own. */
        if (data.OrderTypeId != null) { $('#cmbCategoryI').val(String(data.OrderTypeId)).trigger('change'); }
        if (data.OtherCategoryId != null) { $('#cmbCategoryII').val(String(data.OtherCategoryId)).trigger('change'); }
        $('#txtBranchSrNo').val(data.BranchSrNo != null ? data.BranchSrNo : '');
        $('#cmbBookingPerson').val(data.BookingPersonId || '');
        $('#cmbPaymentTerm').val(data.PaymentTermsId || '');
        $('#txtDueDays').val(data.OrderDueDays != null ? data.OrderDueDays : '');
        $('#txtDueDate').val(data.OrderDueDate ? data.OrderDueDate.substring(0, 10) : '');
        /* The desktop stores and restores Delivery Term as TEXT ("Load"/"Ponch"): combdeliverytrm.Text =
           po.DeliveryTerm (SaleOrder.cs:3244). .val() against option values 1/2 never matched. */
        selectOptionByText('#cmbDeliveryTerm', data.DeliveryTerm);
        $('#txtDeliveryStartDate').val(data.DeliveryStartDate ? data.DeliveryStartDate.substring(0, 10) : '');
        $('#txtDeliveryDays').val(data.DeliveryDays != null ? data.DeliveryDays : '');
        $('#cmbSalesMan').val(data.BrokerAgentSupCustId || '');
        /* An order loaded WITH an agent must arrive with its Amount box already enabled -
           the desktop reaches the same state through combsalesman_Leave (:1616). */
        $('#txtCommAmount').prop('disabled', !parseInt(data.BrokerAgentSupCustId || '0', 10));
        $('#cmbCommType').val(data.CommissionType || '');
        /* combruom.Text = po.UomScheduleIdCmRate (SaleOrder.cs:3250) - restored by text, as the desktop does.
           Deferred so the cmbCommType change handler's reset-to-first-row runs first. */
        var loadedCommUom = data.UomScheduleIdCmRate, loadedOtherCommUom = data.OtherCommissionUom;
        $('#txtCommRate').val(data.CommRate != null ? data.CommRate : '');
        $('#txtCommAmount').val(data.CommAmount != null ? data.CommAmount : '');
        $('#txtCommRemarks').val(data.CommissionRemarks || '');
        $('#cmbOtherSalesMan').val(data.OtherCommissionAgentId || '');
        $('#txtOtherCommAmount').prop('disabled', !parseInt(data.OtherCommissionAgentId || '0', 10));
        $('#cmbOtherCommType').val(data.OtherCommissionType || '');
        $('#txtOtherCommRate').val(data.OtherCommissionRate != null ? data.OtherCommissionRate : '');
        $('#txtOtherCommAmount').val(data.OtherCommissionAmount != null ? data.OtherCommissionAmount : 0);
        $('#txtOtherCommRemarks').val(data.OtherCommissionRemarks || '');
        if (loadedCommUom != null && loadedCommUom !== '') { selectOptionByText('#cmbCommUom', loadedCommUom); }
        /* CmbOtherCommissionUom.Text is set only when po.OtherCommissionUom > 0 (SaleOrder.cs:3259-3261). */
        if (parseFloat(loadedOtherCommUom) > 0) { selectOptionByText('#cmbOtherCommUom', loadedOtherCommUom); }
        /* cmbLocationType.Value = SaleOrderDetailList[0].LocationTypeId (SaleOrder.cs:3284). */
        if (data.lineItems && data.lineItems.length && data.lineItems[0].LocationTypeId != null) {
            $('#cmbLocationType').val(String(data.lineItems[0].LocationTypeId)).trigger('change');
        }

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
