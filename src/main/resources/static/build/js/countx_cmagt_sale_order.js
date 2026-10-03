/* =============================================================================
 * Commission Trading - Sale Order / Deal With Buyer   (DocumentTypeId = 1053)
 *
 * Port of Architecture.WinApp.Cmagt/frmSaleOrderCmagt.cs.
 *
 * Every calculation, validation message, default and event wiring below is taken
 * from that file. Where the desktop has a quirk (its own typos, its rounding
 * choices, the fact that editing an expense Amount back-solves the Rate rather than
 * the Qty) the quirk is reproduced rather than "improved", so the two screens
 * behave identically for the people who use both.
 *
 * Desktop event -> web event map:
 *   txtQty.TextChanged            -> #txtQty  input
 *   txtWeight.TextChanged         -> #txtWeight input
 *   txtRate.TextChanged           -> #txtRate input
 *   CmbPackUom.Leave              -> #cmbPackUom change
 *   CmbRateUom.TextChanged        -> #cmbRateUom change
 *   txtAmount.TextChanged         -> #txtAmount input/programmatic
 *   CmbTaxName.Leave              -> #cmbTaxName change
 *   cmbParentItem.Leave           -> #cmbParentItem change
 *   CmbItemName.Leave             -> #cmbItemName change
 *   CmbDeliveryToParty.Leave      -> #cmbDeliveryToParty change
 *   CmbShipToAddress.Leave        -> #cmbShipToAddress change
 *   CmbPaymentTerm.ValueChanged   -> #cmbPaymentTerm change
 *   txtDeliveryDays.TextChanged   -> #txtDeliveryDays input
 *   datDeliveryStartDate.ValueChanged -> #datDeliveryStartDate change
 *   grdDetail.DoubleClick         -> Edit cell button / row double click
 *   grdDetail_ColumnButtonClick   -> X and Edit cell buttons
 * ============================================================================= */
(function () {
    'use strict';

    var API = '/commission/sale-order/api';

    /* ---------------------------------------------------------------------
     * State - the web equivalent of the desktop's DataTables and fields
     * ------------------------------------------------------------------- */
    var LK = {};                 // master lookups
    var dtDetail = [];           // grdDetail rows
    var dtExpGrid = [];          // grdInvExp rows
    var dtEmptyBags = [];        // grdEmptyBags rows
    var dtPaymentTerm = [];      // grdPaymentTerm rows
    var removedDetailRows = [];  // lstRemoveRecordDetail - re-submitted with actionTypeId 3

    var RecId = 0;               // 0 = new order
    var updateDetailIndex = -1;  // -1 = not editing a row
    var currentUomRows = [];     // UOM schedule of the item currently in the entry bar
    var historyRows = [];
    var selectedHistoryId = 0;

    /* ---------------------------------------------------------------------
     * Formatting - the desktop's "#,##0.###"
     * ------------------------------------------------------------------- */
    function num(v) {
        if (v === null || v === undefined) return 0;
        var n = parseFloat(String(v).replace(/,/g, '').trim());
        return isNaN(n) ? 0 : n;
    }
    function int(v) { return Math.trunc(num(v)); }

    function fmt3(v) {
        var n = num(v);
        return n.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 3 });
    }
    function fmt4(v) {
        var n = num(v);
        return n.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 4 });
    }
    /** Math.Round(x, MidpointRounding.AwayFromZero) at a given number of decimals. */
    function roundAway(v, dp) {
        var f = Math.pow(10, dp || 0);
        var x = num(v) * f;
        return (x >= 0 ? Math.round(x) : -Math.round(-x)) / f;
    }
    function esc(v) {
        if (v === null || v === undefined) return '';
        return String(v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
                        .replace(/>/g, '&gt;').replace(/"/g, '&quot;');
    }
    /*
     * Calendar dates are handled in LOCAL time throughout. toISOString() converts to UTC first,
     * so at the site's UTC+5 a local midnight became the previous day: every Expiry Date and
     * payment Due Date was one day early, and every DATETIME loaded from the server
     * (deliveryStartDate, ValidityDate, DueDate - serialised as UTC instants) was shown one
     * day early and then saved back one day early on Update.
     */
    function ymd(d) {
        return d.getFullYear() + '-' + ('0' + (d.getMonth() + 1)).slice(-2) + '-' + ('0' + d.getDate()).slice(-2);
    }
    function today() { return ymd(new Date()); }
    function addDays(iso, days) {
        if (!iso) return '';
        var d = new Date(String(iso).slice(0, 10) + 'T00:00:00');
        if (isNaN(d.getTime())) return '';
        d.setDate(d.getDate() + int(days));
        return ymd(d);
    }
    function daysBetween(a, b) {
        if (!a || !b) return 0;
        return Math.round((new Date(b + 'T00:00:00') - new Date(a + 'T00:00:00')) / 86400000);
    }
    function fmtDate(v) {
        if (!v) return '';
        var d = new Date(v);
        if (isNaN(d.getTime())) return String(v);
        var m = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
        return ('0' + d.getDate()).slice(-2) + '-' + m[d.getMonth()] + '-' + d.getFullYear();
    }
    function toIsoDate(v) {
        if (!v) return '';
        // A bare calendar date, or a zone-less local date-time: take the calendar part as is.
        if (typeof v === 'string' && /^\d{4}-\d{2}-\d{2}(T[0-9:.]*)?$/.test(v)) return v.slice(0, 10);
        // An instant (epoch number or ISO string with a zone): read it in local time.
        var d = new Date(v);
        return isNaN(d.getTime()) ? '' : ymd(d);
    }
    /** C# Math.Round(double) - MidpointRounding.ToEven, the default the desktop uses. */
    function roundEven(v) {
        var x = num(v), r = Math.round(x);
        if (Math.abs(x % 1) === 0.5 && r % 2 !== 0) r -= 1;
        return r;
    }

    /* ---------------------------------------------------------------------
     * Busy state - every action button is disabled while a request is in
     * flight and re-enabled the moment the data has finished loading.
     * ------------------------------------------------------------------- */
    var busyDepth = 0;
    /* The five button rules. (1) every button in the form is disabled the moment a request
       starts and (4) stays disabled for as long as busyDepth > 0, so a nested call cannot
       re-enable the form early. (2) 'is-busy' now has a stylesheet behind it
       (build/css/countx_button_busy.css) and the button that was clicked carries the spinner -
       the class used to be toggled against no CSS at all, and the `message` argument every
       caller passes was accepted and thrown away. (3) busyGuard() below refuses a second call
       of the same action. (5) every caller restores through .always(), so a rejected request
       re-enables exactly like a successful one. */
    function busy(on, message) {
        busyDepth += on ? 1 : -1;
        if (busyDepth < 0) busyDepth = 0;
        var isBusy = busyDepth > 0;
        $('#soForm').toggleClass('is-busy', isBusy);
        $('#soForm').find('button').prop('disabled', isBusy);
        var $msg = $('#soBusyText');
        if ($msg.length) { $msg.text(isBusy && message ? message : ''); }
        if (!isBusy) { $('#soForm').find('button.btn-busy').removeClass('btn-busy'); }
        if (message !== undefined) status(message);
    }

    /* Rule 3 - prevent duplicate requests. Disabling the buttons already stops a second UI
       click, but a keyboard repeat or a programmatic call can still re-enter before the first
       response lands, so each action also holds a named lock. */
    var soInFlight = {};
    function busyGuard(name, work) {
        if (soInFlight[name]) return null;
        soInFlight[name] = true;
        var release = function () { soInFlight[name] = false; };
        var r;
        try { r = work(release); } catch (e) { release(); throw e; }
        if (r && typeof r.always === 'function') { r.always(release); }
        else if (r && typeof r.then === 'function') { r.then(release, release); }
        return r;
    }
    function status(msg, isError) {
        $('#formStatus').text(msg || '').toggleClass('err', !!isError);
    }
    function detailStatus(msg, isError) {
        $('#detailStatus').text(msg || '').toggleClass('err', !!isError);
    }

    /* ---------------------------------------------------------------------
     * Combo binding. Every dropdown on this screen is searchable, matching the
     * desktop, where all 30-odd UltraCombo controls are created with
     * AutoCompleteMode = SuggestAppend and AutoSuggestFilterMode = Contains.
     * ------------------------------------------------------------------- */
    /* The drop grids rendered by build/js/countx_desktop_combo.js read their extra columns from
       data- attributes on each <option>; the column SET is declared on the <select>
       (data-dtcombo="party4" | "itemCmagt" | "uomCmagt" | …).
     *
     * Every key below is a real column of the row the API returns - the commission dropdown
     * endpoints pass the desktop's own column names straight through - so nothing is invented. A
     * row without a column gets no attribute and the grid renders that cell EMPTY. Equivalent is
     * published exactly as stored and is never defaulted to 1. */
    function comboColumnAttrs(r) {
        if (!r) return '';
        var out = '';
        function pick() {
            for (var i = 0; i < arguments.length; i++) {
                var v = r[arguments[i]];
                if (v !== undefined && v !== null && v !== '') return v;
            }
            return null;
        }
        function add(attr, v) {
            if (v === null || v === undefined) return;
            out += ' data-' + attr + '="' + esc(v) + '"';
        }
        add('code',          pick('PartyCode', 'partyCode'));
        add('city',          pick('CityName', 'cityName'));
        add('mobile',        pick('MobileNo', 'mobileNo', 'MobilePersonal'));
        add('item-code',     pick('ItemCode', 'itemCode'));
        add('item-category', pick('ItemCategory', 'itemCategory', 'InvParentCateDescription'));
        var eq = pick('Equivalent', 'equivalent');
        if (eq !== null) add('eq', eq);
        var br = r.BaseRateUom !== undefined ? r.BaseRateUom : r.baseRateUom;
        var bp = r.BasePackUom !== undefined ? r.BasePackUom : r.basePackUom;
        if (br !== undefined && br !== null) add('base', br === true ? 1 : br === false ? 0 : br);
        if (bp !== undefined && bp !== null) add('base-pack', bp === true ? 1 : bp === false ? 0 : bp);

        /* uomCmagt5 / shipTo2 / analysisGroup3 / taxCmagt (countx_desktop_combo.js) */
        var bs = r.BaseSecondaryUom !== undefined ? r.BaseSecondaryUom : r.baseSecondaryUom;
        if (bs !== undefined && bs !== null) add('base-secondary', bs === true ? 1 : bs === false ? 0 : bs);
        if (r.AddressLine1 !== undefined) add('party-name', pick('CompanyName', 'PartyName'));
        if (r.AnalysisGroupDescription !== undefined) {
            add('group-type', pick('GroupType'));
            add('parent-category', pick('InvParentCateDescription'));
        }
        if (r.TaxNameId !== undefined) {
            add('tax-schedule-id', pick('TaxScheduleId'));
            var ed = pick('EffectedDate');
            if (ed !== null) add('effected-date', typeof ed === 'number' ? localYmd(new Date(ed)) : String(ed).slice(0, 10));
            add('tax-percent', pick('TaxPercent'));
            add('tax-gl-account', pick('TaxGLAccountId'));
        }
        function localYmd(d) { return d.getFullYear() + '-' + ('0' + (d.getMonth() + 1)).slice(-2) + '-' + ('0' + d.getDate()).slice(-2); }
        return out;
    }

    function bindCombo(sel, rows, valueField, textField, opts) {
        opts = opts || {};
        var $el = $(sel);
        var keep = opts.keepValue ? $el.val() : null;
        var html = opts.noBlank ? '' : '<option value=""></option>';
        (rows || []).forEach(function (r) {
            var v = r[valueField];
            var t = r[textField];
            if (v === null || v === undefined) return;
            html += '<option value="' + esc(v) + '"' + comboColumnAttrs(r) + '>' + esc(t) + '</option>';
        });
        $el.html(html);
        if (keep !== null && $el.find('option[value="' + keep + '"]').length) $el.val(keep);
        // select2 only re-renders on the native change event
        $el.trigger('change.select2');
    }

    function initSelect2() {
        $('.so-select2').each(function () {
            var $e = $(this);
            /* 2026-10-03: a checked combo (countx_prod_combo.js, data-dtcombo-checked) never gets select2 */
            if ($e.data('select2') || this.__dtcombo || this.classList.contains('dtcombo-native') || this.hasAttribute('data-dtcombo-checked')) return;
            $e.select2({ width: $e.css('width'), dropdownAutoWidth: true, placeholder: '' });
        });
    }

    /* =====================================================================
     * MASTER LOOKUPS
     * ===================================================================== */
    function loadLookups() {
        busy(true, 'Loading…');
        return $.getJSON(API + '/master-lookups')
            .done(function (d) {
                LK = d || {};
                bindPartyCombos();
                bindCombo('#cmbDeliveryTerm', LK.deliveryTerms, 'Id', 'Description');
                bindCombo('#cmbPaymentTerm', LK.paymentTerms, 'Id', 'TermsDescription');
                bindCombo('#cmbBranch', LK.branches, 'BranchId', 'BranchName');
                bindCombo('#cmbCommType', LK.commissionTypes, 'Id', 'Name');
                bindCombo('#cmbBrokeryType', LK.commissionTypes, 'Id', 'Name');
                bindCombo('#cmbCommUom', LK.commissionRateUoms, 'Id', 'Name');
                bindCombo('#cmbBrokeryRateUom', LK.commissionRateUoms, 'Id', 'Name');
                bindCombo('#cmbCropYear', LK.cropYears, 'Id', 'CropYear');
                bindCombo('#cmbPackingType', LK.packingTypes, 'Id', 'PackTypeDesc');
                bindCombo('#cmbParentItem', LK.parentItems, 'Id', 'Description');
                // CompanyBind(CompanyDropdownData()); CmbCompanyName.Value = UserAccount.CompanyId
                bindCombo('#cmbCompanyName', LK.companies, 'Id', 'CompName', { keepValue: true });
                /* the "-- Select --" row has value "0", so test the number, not the string */
                if (!int($('#cmbCompanyName').val()) && LK.currentCompanyId) {
                    $('#cmbCompanyName').val(String(LK.currentCompanyId)).trigger('change.select2');
                }
                /* desktop: CmbBranch.Value = UserAccount.BranchesId (login branch) */
                if (!int($('#cmbBranch').val()) && LK.currentBranchId) {
                    $('#cmbBranch').val(String(LK.currentBranchId)).trigger('change.select2');
                }
                bindItemCombo('#cmbItemName', 0);
                bindShipToAddresses(0);
                // HistoryComboBind(HistoryComboDbCall()) - USP_GetDataForDropDownFromsaleOrderMaster
                historyComboBind(LK.historyCombos);

                seedExpenseGrid();
                seedEmptyBagGrid();
                renderAllGrids();

                // A lookup that failed comes back empty, and says so - it is never
                // quietly padded with placeholder rows.
                var failed = Object.keys(LK.lookupErrors || {});
                if (failed.length) {
                    status('These dropdowns could not be loaded and are empty: '
                         + failed.join(', ') + ' — ' + LK.lookupErrors[failed[0]], true);
                } else {
                    status('');
                }
            })
            .fail(function (xhr) {
                status('Could not load the screen lookups: ' + (xhr.responseText || xhr.statusText), true);
            })
            .always(function () { busy(false); });
    }

    /**
     * The desktop binds ONE parties DataTable to five combos. The Business Name /
     * Nick Name radio pair switches which name column is displayed; sub-parties are
     * shown as "parent / child" under either mode (already composed server-side).
     */
    function bindPartyCombos() {
        var field = $('#radNickName').is(':checked') ? 'DisplayNick' : 'DisplayCompany';
        ['#cmbCommissionAgent', '#cmbBuyerName', '#cmbDeliveryToParty',
         '#cmbCommissionAc', '#cmbBrokeryAc']
            .forEach(function (sel) {
                bindCombo(sel, LK.parties, 'Id', field, { keepValue: true });
            });
    }

    /** ItemdtFillFromGlobal + ItemNameBind: the item list filtered by parent category. */
    function bindItemCombo(sel, parentCategoryId) {
        var rows = (LK.items || []).filter(function (r) {
            return !parentCategoryId || int(r.InventoryParentCategoriesId) === int(parentCategoryId);
        });
        bindCombo(sel, rows, 'Id', 'ItemName', { keepValue: true });
    }

    /** BindShipToAddressAgainstBuyer - addresses of the chosen Deliver/Ship To Party. */
    function bindShipToAddresses(partyId) {
        var rows = (LK.shipToAddresses || []).filter(function (r) {
            return !partyId || int(r.SupplierCustomerId) === int(partyId);
        });
        bindCombo('#cmbShipToAddress', rows, 'Id', 'AddressLine1', { keepValue: true });
    }

    /* =====================================================================
     * HISTORY COMBOS - HistoryComboBind (:3062) / BindDropdownsAgainstParentCategory (:3183)
     * Source: [cmagt].[USP_GetDataForDropDownFromsaleOrderMaster] rows
     * (Id, ReferenceName, ParentCategoryId, Activity). Parent Category is a multi-select
     * checked list defaulted to GetMostUsedParentCategoryId; the other five combos are
     * filtered to the checked parent categories.
     * ===================================================================== */
    var HISTORY_COMBO_MAP = [
        ['CommissionAgent',     '#cmbCommissionAgentHistory', true],
        ['buyerName',           '#cmbBuyerNameHistory',       true],
        ['Item',                '#cmbItemNameHistory',        false],
        ['DeliveryToPartyName', '#cmbDeliveryToPartyHistory', true],
        ['DeliverToAddress',    '#cmbDeliverToAddressHistory', true]
    ];
    function bindHistoryChildCombos(parentIds) {
        var rows = LK.historyCombos || [];
        var filter = parentIds && parentIds.length ? parentIds.map(String) : null;
        HISTORY_COMBO_MAP.forEach(function (m) {
            var seen = {}, out = [];
            rows.forEach(function (r) {
                if (String(r.Activity || '').toLowerCase() !== m[0].toLowerCase()) return;
                if (filter && filter.indexOf(String(r.ParentCategoryId)) < 0) return;
                if (m[2]) { if (seen[r.Id]) return; seen[r.Id] = 1; }
                out.push({ Id: r.Id, Name: r.ReferenceName });
            });
            bindCombo(m[1], out, 'Id', 'Name', { keepValue: true });
        });
    }
    function historyComboBind(rows) {
        LK.historyCombos = rows || [];
        var parents = [], seen = {}, mostUsed = null;
        LK.historyCombos.forEach(function (r) {
            if (r.Activity === 'ParentCategories' && !seen[r.Id]) { seen[r.Id] = 1; parents.push({ Id: r.Id, Name: r.ReferenceName }); }
            if (r.Activity === 'GetMostUsedParentCategoryId' && mostUsed === null) mostUsed = r.Id;
        });
        var $p = $('#cmbParentItemHistory');
        var html = '';
        parents.forEach(function (r) { html += '<option value="' + esc(r.Id) + '">' + esc(r.Name) + '</option>'; });
        $p.html(html);
        if (mostUsed !== null) $p.val([String(mostUsed)]);
        $p.trigger('change.select2');
        /* HistoryComboBind loads the five combos with rows of the most-used parent only */
        bindHistoryChildCombos(mostUsed !== null ? [mostUsed] : []);
    }
    function historyParentIds() {
        var v = $('#cmbParentItemHistory').val();
        return Array.isArray(v) ? v.filter(Boolean) : (v ? [v] : []);
    }

    /* =====================================================================
     * DETAIL ENTRY BAR - calculations, verbatim from the desktop
     * ===================================================================== */

    /**
     * CalculateWeight(): Weight = Qty * packEquivalent  (MULTIPLY).
     *
     * If the factor is unavailable the calculation is blocked and the user's Qty is left exactly
     * as typed - the computed fields are cleared rather than written as 0, so a missing factor
     * can never be mistaken for a legitimate zero weight.
     */
    function calculateWeight() {
        var f = uomFactor($('#cmbPackUom').val(), 'Pack Uom');
        if (!f.ok) { blockFactor(f); return; }
        clearFactorBlock();
        $('#txtWeight').val(fmt3(num($('#txtQty').val()) * f.value));
        calculateAmount();
    }

    /** CalculateAmount(): Amount = Weight / rateEquivalent * Rate  (DIVIDE, then multiply). */
    function calculateAmount() {
        var f = uomFactor($('#cmbRateUom').val(), 'Rate Uom');
        if (!f.ok) { blockFactor(f); return; }
        clearFactorBlock();
        var weight = num($('#txtWeight').val());
        var rate = num($('#txtRate').val());
        // The desktop's own guard: a zero weight or rate legitimately yields a zero amount.
        var amount = (weight > 0 && f.value > 0) ? (weight / f.value * rate) : 0;
        $('#txtAmount').val(fmt3(amount));
        calculateTaxAmount();
    }

    /**
     * A required factor could not be resolved. Blank the derived fields (never 0), keep every
     * value the user typed, say exactly what is wrong, and block Add/Update.
     */
    function blockFactor(f) {
        if (f.missing) {
            // Simply not chosen yet - not an error, just nothing to compute with.
            $('#txtWeight,#txtAmount,#txtTaxPercnt,#txtTaxAmount,#txtTotalAmount').val('');
            factorBlocked = false;
            detailStatus('');
            return;
        }
        $('#txtWeight,#txtAmount,#txtTaxPercnt,#txtTaxAmount,#txtTotalAmount').val('');
        factorBlocked = true;
        detailStatus(f.reason + ' This row cannot be calculated or added until the UOM schedule '
                   + 'is corrected. Your entries have been kept.', true);
    }

    function clearFactorBlock() {
        if (factorBlocked) { factorBlocked = false; detailStatus(''); }
    }

    /** CalculateTaxAmount(): Tax = Amount * Tax% / 100, rounded away from zero at 3dp. */
    function calculateTaxAmount() {
        var amount = num($('#txtAmount').val());
        var taxPercent = 0;
        var taxId = int($('#cmbTaxName').val());
        if (taxId > 0) {
            var row = (currentTaxRows || []).filter(function (r) { return int(r.TaxNameId) === taxId; })[0];
            if (row) taxPercent = num(row.TaxPercent);
        }
        var taxAmt = amount * taxPercent / 100;
        $('#txtTaxPercnt').val(fmt3(taxPercent));
        $('#txtTaxAmount').val(fmt3(roundAway(taxAmt, 3)));
        $('#txtTotalAmount').val(fmt3(roundAway(amount + taxAmt, 3)));
    }

    var currentTaxRows = [];

    /**
     * The UOM conversion factor.
     *
     * The server hands this screen a stable contract - uomId / uomCode / equivalent /
     * equivalentAvailable / basePackUom / baseRateUom - so the browser never guesses at raw
     * database column names. The factor itself was traced to
     * CommonServices.dtUomFromGloablUomScheduleByItemId, which builds the DataTable the
     * desktop combos bind to with column order Id, UOMCode, Equivalent, BaseRateUom,
     * BasePackUom, BaseSecondaryUom - so the desktop's positional Cells[2] is the
     * Equivalent column. QtyEquivalent is a different column and is NOT the factor.
     *
     * Returns {ok:true, value:Number} or {ok:false, reason:String}. There is no fallback to
     * 0 or 1: an unavailable factor is an error state, never a silent number.
     */
    function uomFactor(uomId, label) {
        var id = int(uomId);
        if (!id) return { ok: false, missing: true, reason: label + ' is not selected.' };
        var row = (currentUomRows || []).filter(function (r) { return int(r.uomId) === id; })[0];
        if (!row) {
            return { ok: false, reason: label + ' is not in this item\'s UOM schedule, so its '
                                      + 'conversion factor cannot be resolved.' };
        }
        if (!row.equivalentAvailable) {
            return { ok: false, reason: label + ' (' + (row.uomCode || id) + ') has no usable '
                                      + 'conversion factor in the UOM schedule'
                                      + (row.equivalent === null || row.equivalent === undefined
                                          ? ' (Equivalent is not set).'
                                          : ' (Equivalent is ' + row.equivalent + ').') };
        }
        return { ok: true, value: num(row.equivalent) };
    }

    /**
     * True while a required conversion factor cannot be resolved. While set, the row cannot be
     * added or updated - the desktop reaches the same outcome by leaving the computed fields at
     * zero, which its own required-field validation then rejects.
     */
    var factorBlocked = false;

    function loadItemUoms(itemId) {
        if (!itemId) { currentUomRows = []; bindCombo('#cmbPackUom', [], 'Id', 'UOMCode'); bindCombo('#cmbRateUom', [], 'Id', 'UOMCode'); return $.Deferred().resolve().promise(); }
        busy(true);
        return $.getJSON(API + '/item-uoms/' + itemId)
            .done(function (rows) {
                currentUomRows = rows || [];
                bindCombo('#cmbPackUom', currentUomRows, 'uomId', 'uomCode');
                bindCombo('#cmbRateUom', currentUomRows, 'uomId', 'uomCode');
                // ItemUomFromGlobalBind pre-selects the BasePackUom / BaseRateUom rows.
                // These use .trigger('change.select2'), which refreshes select2's rendering
                // WITHOUT firing the plain 'change' handlers - so initialising the lookup
                // never recalculates over values already loaded into the entry bar.
                var basePack = currentUomRows.filter(function (r) { return r.basePackUom; })[0];
                var baseRate = currentUomRows.filter(function (r) { return r.baseRateUom; })[0];
                if (basePack) $('#cmbPackUom').val(basePack.uomId).trigger('change.select2');
                if (baseRate) $('#cmbRateUom').val(baseRate.uomId).trigger('change.select2');

                var unusable = currentUomRows.filter(function (r) { return !r.equivalentAvailable; });
                if (currentUomRows.length && unusable.length === currentUomRows.length) {
                    factorBlocked = true;
                    detailStatus('No UOM on this item has a usable conversion factor (Equivalent). '
                               + 'Weight and Amount cannot be calculated for it.', true);
                }
            })
            .always(function () { busy(false); });
    }

    function loadItemTax(itemId) {
        if (!itemId) {
            currentTaxRows = [];
            bindCombo('#cmbTaxName', [], 'TaxNameId', 'TaxName');
            $('#txtTaxPercnt,#txtTaxAmount,#txtTotalAmount').val('');
            return $.Deferred().resolve().promise();
        }
        busy(true);
        return $.getJSON(API + '/item-tax/' + itemId, { docDate: $('#datDocDate').val() })
            .done(function (rows) {
                currentTaxRows = rows || [];
                bindCombo('#cmbTaxName', currentTaxRows, 'TaxNameId', 'TaxName');
            })
            .always(function () { busy(false); });
    }

    /* =====================================================================
     * COMMISSION / BROKERY - TotalCommissionAmount + TotalBrokeryAmountCalculate
     * ===================================================================== */
    function commissionBlock($ac, $type, $rate, $uom, $amount) {
        if (!int($ac.val())) { $amount.val('0'); return; }
        var rate = num($rate.val());
        if (!(rate > 0)) { $amount.val('0'); return; }

        var typeText = ($type.find('option:selected').text() || '').trim();

        if (typeText === 'Flat') {
            $amount.val(fmt3(rate));
        } else if (typeText === 'Percent') {
            if (rate > 100) { rate = 100; $rate.val('100'); }
            var totalAmount = sumDetail('Amount');
            $amount.val(fmt3(roundEven(totalAmount * rate / 100)));
        } else if (typeText === 'Weight') {
            var totalWeight = sumDetail('Weight');
            var uomText = ($uom.find('option:selected').text() || '').trim();
            var uomVal = num(uomText);
            if (uomVal > 0) $amount.val(fmt3(roundEven(totalWeight / uomVal * rate)));
            else $amount.val('0');
        }
    }

    function totalCommissionAmount() {
        commissionBlock($('#cmbCommissionAc'), $('#cmbCommType'), $('#txtCommRate'),
                        $('#cmbCommUom'), $('#txtCommAmount'));
    }
    function totalBrokeryAmount() {
        commissionBlock($('#cmbBrokeryAc'), $('#cmbBrokeryType'), $('#txtBrokeryRate'),
                        $('#cmbBrokeryRateUom'), $('#txtBrokeryAmount'));
    }

    function sumDetail(field) {
        return dtDetail.reduce(function (a, r) { return a + num(r[field]); }, 0);
    }

    /** PaymentAmountReCalculate(): re-spread the schedule over the new detail total. */
    function paymentAmountReCalculate() {
        var total = sumDetail('TotalAmount');
        if (total > 0 && dtPaymentTerm.length) {
            dtPaymentTerm.forEach(function (r) {
                r.Amount = num(r['%OfTotal']) * total / 100;
            });
            renderPaymentGrid();
            refreshPaymentScheduleText();
        }
    }

    function afterDetailChanged() {
        totalCommissionAmount();
        totalBrokeryAmount();
        paymentAmountReCalculate();
        // EnableDisableColumns(): the doc date is locked once any row carries tax.
        var hasTax = dtDetail.some(function (r) { return num(r.TaxAmount) > 0; });
        $('#datDocDate').prop('disabled', hasTax);
    }

    /* =====================================================================
     * DETAIL GRID
     *
     * Columns, order, captions and pixel widths are the desktop's own, from
     * SaleOrderCmagt_Helper.InitializeDetailtable / DetailGridCommonSetting and
     * Constants.InventoryConstants. Hidden id columns are hidden here too.
     * X (20px) and Edit (40px) are the GridEX_Helper.AddButton cell buttons at
     * positions 0 and 1; grdDetail.FrozenColumns = 2 keeps them pinned while the
     * grid scrolls sideways.
     * ===================================================================== */
    var DETAIL_COLUMNS = [
        { key: 'ParentItem',  caption: 'Parent Item',   w: 150 },
        { key: 'ItemName',    caption: 'Item Name',     w: 150 },
        { key: 'CropYear',    caption: 'Crop Year',     w: 73  },
        { key: 'PackingType', caption: 'Packing Type',  w: 115 },
        { key: 'PackUom',     caption: 'Pack Uom',      w: 60  },
        { key: 'Qty',         caption: 'Qty',           w: 70,  num: true, total: true },
        { key: 'Weight',      caption: 'Weight',        w: 80,  num: true, total: true },
        { key: 'Rate',        caption: 'Rate',          w: 70,  num: true },
        { key: 'RateUom',     caption: 'Rate Uom',      w: 60  },
        { key: 'Amount',      caption: 'Amount',        w: 90,  num: true, total: true },
        { key: 'TaxName',     caption: 'Tax Name',      w: 130 },
        { key: 'Tax%',        caption: 'Tax%',          w: 70,  num: true },
        { key: 'TaxAmount',   caption: 'Tax Amount',    w: 90,  num: true, total: true },
        // TotalAmount's caption really is "Tax + Amount" on the desktop.
        { key: 'TotalAmount', caption: 'Tax + Amount',  w: 90,  num: true, total: true },
        { key: 'Remarks',     caption: 'Remarks',       w: 100, flex: true }
    ];

    function renderDetailGrid() {
        var h = '<colgroup><col style="width:20px"><col style="width:40px">';
        DETAIL_COLUMNS.forEach(function (c) {
            h += '<col style="width:' + c.w + 'px' + (c.flex ? ';min-width:' + c.w + 'px' : '') + '">';
        });
        h += '</colgroup><thead><tr>';
        h += '<th class="frz" style="left:0">X</th><th class="frz" style="left:20px">Edit</th>';
        DETAIL_COLUMNS.forEach(function (c) { h += '<th>' + esc(c.caption) + '</th>'; });
        h += '</tr></thead><tbody>';

        if (!dtDetail.length) {
            h += '<tr><td colspan="' + (DETAIL_COLUMNS.length + 2) + '" class="ctr" style="color:#666">'
               + 'No detail rows</td></tr>';
        }
        dtDetail.forEach(function (r, i) {
            h += '<tr data-i="' + i + '">';
            h += '<td class="frz ctr" style="left:0"><button type="button" class="grid-cell-btn del" data-act="del" data-i="' + i + '">X</button></td>';
            h += '<td class="frz ctr" style="left:20px"><button type="button" class="grid-cell-btn" data-act="edit" data-i="' + i + '">Edit</button></td>';
            DETAIL_COLUMNS.forEach(function (c) {
                var v = r[c.key];
                h += '<td' + (c.num ? ' class="num"' : '') + ' title="' + esc(c.num ? fmt3(v) : v) + '">'
                   + esc(c.num ? fmt3(v) : (v === undefined || v === null ? '' : v)) + '</td>';
            });
            h += '</tr>';
        });
        h += '</tbody><tfoot><tr>';
        h += '<td class="frz" style="left:0"></td><td class="frz" style="left:20px"></td>';
        DETAIL_COLUMNS.forEach(function (c) {
            h += '<td' + (c.num ? ' class="num"' : '') + '>'
               + (c.total ? fmt3(sumDetail(c.key)) : '') + '</td>';
        });
        h += '</tr></tfoot>';
        $('#grdDetail').html(h);
    }

    /** BtnAdd_Click - with the desktop's duplicate-item guard. */
    function addDetailRow() {
        if (!validateDetailEntry()) return;
        var itemId = int($('#cmbItemName').val());
        if (dtDetail.some(function (r) { return int(r.ItemId) === itemId; })) {
            detailStatus($('#cmbItemName option:selected').text() + ' already in detail. So can\'t add this item', true);
            return;
        }
        dtDetail.push(readDetailEntry(0));
        renderDetailGrid();
        resetDetailEntry();
        afterDetailChanged();
        detailStatus('');
    }

    /** BtnUpdateDetail_Click - the duplicate guard skips the row being edited. */
    function updateDetailRow() {
        if (!validateDetailEntry()) return;
        if (updateDetailIndex < 0 || updateDetailIndex >= dtDetail.length) return;
        var itemId = int($('#cmbItemName').val());
        var dup = dtDetail.some(function (r, i) { return i !== updateDetailIndex && int(r.ItemId) === itemId; });
        if (dup) {
            detailStatus($('#cmbItemName option:selected').text() + ' already in detail. So can\'t add this item', true);
            return;
        }
        var existingId = dtDetail[updateDetailIndex].Id;
        dtDetail[updateDetailIndex] = readDetailEntry(existingId);
        renderDetailGrid();
        resetDetailEntry();
        afterDetailChanged();
        detailStatus('');
    }

    /**
     * DeleteDetailRow - a row that already exists in the database is NOT dropped: it is
     * moved to removedDetailRows so it is re-submitted with actionTypeId = 3 and the
     * procedure performs its soft delete. Only a row added during this session is
     * discarded outright.
     */
    function deleteDetailRow(i) {
        if (updateDetailIndex !== -1) {
            detailStatus('Please Reset the Detail first..', true);
            return;
        }
        var row = dtDetail[i];
        if (!row) return;
        if (int(row.Id) !== 0) {
            if (!window.confirm('Are you sure to Delete?')) return;
            var d = detailRowToDto(row);
            d.actionTypeId = 3;
            removedDetailRows.push(d);
        }
        dtDetail.splice(i, 1);
        renderDetailGrid();
        afterDetailChanged();
    }

    /** grdDetail_DoubleClick - load the row back into the entry bar for editing. */
    function editDetailRow(i) {
        var r = dtDetail[i];
        if (!r) return;
        updateDetailIndex = i;

        $('#cmbParentItem').val(r.ParentItemId).trigger('change.select2');
        bindItemCombo('#cmbItemName', r.ParentItemId);
        $('#cmbItemName').val(r.ItemId).trigger('change.select2');

        $.when(loadItemUoms(r.ItemId), loadItemTax(r.ItemId)).always(function () {
            $('#cmbCropYear').val(r.CropYearId).trigger('change.select2');
            $('#cmbPackingType').val(r.PackingTypeId).trigger('change.select2');
            $('#cmbPackUom').val(r.PackUomId).trigger('change.select2');
            $('#txtQty').val(fmt3(r.Qty));
            $('#txtWeight').val(fmt3(r.Weight));
            $('#txtRate').val(fmt3(r.Rate));
            $('#cmbRateUom').val(r.RateUomId).trigger('change.select2');
            $('#txtAmount').val(fmt3(r.Amount));
            $('#cmbTaxName').val(r.TaxNameId).trigger('change.select2');
            $('#txtTaxPercnt').val(fmt3(r['Tax%']));
            $('#txtTaxAmount').val(fmt3(r.TaxAmount));
            $('#txtTotalAmount').val(fmt3(r.TotalAmount));
            $('#txtRemarksDetail').val(r.Remarks || '');

            $('#btnAddRow').hide();
            $('#btnUpdateRow').show();
            $('#btnCancelRow').show();
        });
    }

    /** FormValidationDetail - the desktop's own required-field list for the entry bar. */
    function validateDetailEntry() {
        // A row whose conversion factor could not be resolved is never addable. The server
        // enforces the same rule independently, so this is a courtesy, not the control.
        if (factorBlocked) {
            detailStatus('This row cannot be added while a required UOM conversion factor is '
                       + 'unavailable. Fix the item\'s UOM schedule, then reselect the item.', true);
            return false;
        }
        var checks = [
            [int($('#cmbParentItem').val()), 'Parent Item'],
            [int($('#cmbItemName').val()), 'Item Name'],
            [int($('#cmbCropYear').val()), 'Crop Year'],
            [int($('#cmbPackingType').val()), 'Packing Type'],
            [int($('#cmbPackUom').val()), 'Pack Uom'],
            [num($('#txtQty').val()), 'Qty'],
            [num($('#txtWeight').val()), 'Weight'],
            [num($('#txtRate').val()), 'Rate'],
            [int($('#cmbRateUom').val()), 'Rate Uom'],
            [num($('#txtAmount').val()), 'Amount'],
            [num($('#txtTotalAmount').val()), 'Tax + Amount']
        ];
        for (var i = 0; i < checks.length; i++) {
            if (!(checks[i][0] > 0)) {
                detailStatus(checks[i][1] + ' is required', true);
                return false;
            }
        }
        return true;
    }

    /** FillCustomerDetailRow - entry bar -> grid row. */
    function readDetailEntry(existingId) {
        var amount = num($('#txtAmount').val());
        var taxAmount = num($('#txtTaxAmount').val());
        var taxId = int($('#cmbTaxName').val());
        return {
            Id: int(existingId),
            ParentItemId: int($('#cmbParentItem').val()),
            ParentItem: $('#cmbParentItem option:selected').text(),
            ItemId: int($('#cmbItemName').val()),
            ItemName: $('#cmbItemName option:selected').text(),
            CropYearId: int($('#cmbCropYear').val()),
            CropYear: $('#cmbCropYear option:selected').text(),
            PackingTypeId: int($('#cmbPackingType').val()),
            PackingType: $('#cmbPackingType option:selected').text(),
            PackUomId: int($('#cmbPackUom').val()),
            PackUom: $('#cmbPackUom option:selected').text(),
            Qty: num($('#txtQty').val()),
            Weight: num($('#txtWeight').val()),
            Rate: num($('#txtRate').val()),
            RateUomId: int($('#cmbRateUom').val()),
            RateUom: $('#cmbRateUom option:selected').text(),
            Amount: amount,
            TaxNameId: taxId,
            TaxName: taxId > 0 ? $('#cmbTaxName option:selected').text() : '',
            'Tax%': num($('#txtTaxPercnt').val()),
            TaxAmount: taxAmount,
            TotalAmount: amount + taxAmount,
            Remarks: $('#txtRemarksDetail').val()
        };
    }

    /** ResetDetail - clears the entry bar and returns it to "add" mode. */
    function resetDetailEntry() {
        $('#cmbItemName').val('').trigger('change.select2');
        $('#txtQty,#txtWeight,#txtRate,#txtAmount,#txtRemarksDetail').val('');
        $('#cmbPackUom').html('<option value=""></option>').trigger('change.select2');
        $('#cmbRateUom').html('<option value=""></option>').trigger('change.select2');
        $('#cmbTaxName').html('<option value=""></option>').trigger('change.select2');
        $('#txtTaxPercnt,#txtTaxAmount,#txtTotalAmount').val('');
        currentUomRows = [];
        currentTaxRows = [];
        factorBlocked = false;
        detailStatus('');
        updateDetailIndex = -1;
        $('#btnAddRow').show();
        $('#btnUpdateRow').hide();
        $('#btnCancelRow').hide();
    }

    /* =====================================================================
     * BUYER OTHER CHARGES GRID  (grdInvExp)
     *
     * AddRowsInExpenseGrid seeds ONE ROW PER OTHER-ITEM in the master list - not one
     * blank row - and only rows with an item and a positive amount are submitted.
     * ===================================================================== */
    var EXPENSE_COLUMNS = [
        { key: 'ItemId',  caption: 'Other Item Name', w: 200, combo: 'otherItems' },
        { key: 'Qty',     caption: 'Qty',             w: 70,  num: true, edit: true, total: true },
        { key: 'Rate',    caption: 'Rate',            w: 70,  num: true, edit: true },
        { key: 'Amount',  caption: 'Amount',          w: 90,  num: true, edit: true, total: true },
        { key: 'Remarks', caption: 'Remarks',         w: 350, edit: true, flex: true }
    ];

    function seedExpenseGrid() {
        dtExpGrid = (LK.otherItems || []).map(function (it) {
            return { Id: 0, ItemId: int(it.Id), Qty: 0, Rate: 0, Amount: 0, Remarks: '' };
        });
    }

    function renderExpenseGrid() {
        var h = '<colgroup>';
        EXPENSE_COLUMNS.forEach(function (c) { h += '<col style="width:' + c.w + 'px">'; });
        h += '</colgroup><thead><tr>';
        EXPENSE_COLUMNS.forEach(function (c) { h += '<th>' + esc(c.caption) + '</th>'; });
        h += '</tr></thead><tbody>';
        dtExpGrid.forEach(function (r, i) {
            h += '<tr data-i="' + i + '">';
            EXPENSE_COLUMNS.forEach(function (c) {
                if (c.combo) {
                    h += '<td>' + esc(otherItemName(r.ItemId)) + '</td>';
                } else if (c.edit) {
                    h += '<td class="edit"><input type="text" class="' + (c.num ? 'num' : '')
                       + '" data-grid="exp" data-i="' + i + '" data-k="' + c.key + '" value="'
                       + esc(c.num ? fmt3(r[c.key]) : (r[c.key] || '')) + '"></td>';
                } else {
                    h += '<td>' + esc(r[c.key]) + '</td>';
                }
            });
            h += '</tr>';
        });
        h += '</tbody><tfoot><tr>';
        EXPENSE_COLUMNS.forEach(function (c) {
            h += '<td' + (c.num ? ' class="num"' : '') + '>'
               + (c.total ? fmt3(dtExpGrid.reduce(function (a, r) { return a + num(r[c.key]); }, 0)) : '')
               + '</td>';
        });
        h += '</tr></tfoot>';
        $('#grdInvExp').html(h);
    }

    function otherItemName(id) {
        var r = (LK.otherItems || []).filter(function (x) { return int(x.Id) === int(id); })[0];
        return r ? r.OtherItemName : '';
    }

    /**
     * grdInvExp_CellUpdated: editing Qty or Rate recomputes Amount; editing Amount
     * back-solves the RATE (not the Qty) - the desktop's UpdateRate does amount/qty.
     */
    function expenseCellUpdated(i, key) {
        var r = dtExpGrid[i];
        if (!r) return;
        if (key === 'Qty' || key === 'Rate') {
            r.Amount = num(r.Qty) * num(r.Rate);
        } else if (key === 'Amount') {
            r.Rate = num(r.Qty) === 0 ? 0 : num(r.Amount) / num(r.Qty);
        }
        renderExpenseGrid();
    }

    /* =====================================================================
     * EMPTY BAGS GRID  (grdEmptyBags) - seeded with packing types 1 and 2
     * ===================================================================== */
    var EMPTYBAG_COLUMNS = [
        { key: 'PackingType',  caption: 'Packing Type',  w: 165, combo: true },
        { key: 'WeightCut',    caption: 'WeightCut',     w: 80,  num: true, edit: true },
        { key: 'PurchaseRate', caption: 'PurchaseRate',  w: 90,  num: true, edit: true }
    ];

    function seedEmptyBagGrid() {
        dtEmptyBags = [
            { PackingType: 1, WeightCut: 0, PurchaseRate: 0 },
            { PackingType: 2, WeightCut: 0, PurchaseRate: 0 }
        ];
    }

    function renderEmptyBagGrid() {
        var opts = LK.allocatedPackingTypes && LK.allocatedPackingTypes.length
                 ? LK.allocatedPackingTypes
                 : (LK.packingTypes || []).map(function (p) { return { Id: p.Id, Name: p.PackTypeDesc }; });

        var h = '<colgroup>';
        EMPTYBAG_COLUMNS.forEach(function (c) { h += '<col style="width:' + c.w + 'px">'; });
        h += '</colgroup><thead><tr>';
        EMPTYBAG_COLUMNS.forEach(function (c) { h += '<th>' + esc(c.caption) + '</th>'; });
        h += '</tr></thead><tbody>';
        dtEmptyBags.forEach(function (r, i) {
            h += '<tr data-i="' + i + '">';
            h += '<td class="edit"><select data-grid="eb" data-i="' + i + '" data-k="PackingType">';
            opts.forEach(function (o) {
                h += '<option value="' + esc(o.Id) + '"' + (int(o.Id) === int(r.PackingType) ? ' selected' : '')
                   + '>' + esc(o.Name) + '</option>';
            });
            h += '</select></td>';
            h += '<td class="edit"><input type="text" class="num" data-grid="eb" data-i="' + i
               + '" data-k="WeightCut" value="' + esc(fmt3(r.WeightCut)) + '"></td>';
            h += '<td class="edit"><input type="text" class="num" data-grid="eb" data-i="' + i
               + '" data-k="PurchaseRate" value="' + esc(fmt3(r.PurchaseRate)) + '"></td>';
            h += '</tr>';
        });
        h += '</tbody>';
        $('#grdEmptyBags').html(h);
    }

    function emptyBagPackName(id) {
        var opts = LK.allocatedPackingTypes && LK.allocatedPackingTypes.length
                 ? LK.allocatedPackingTypes
                 : (LK.packingTypes || []).map(function (p) { return { Id: p.Id, Name: p.PackTypeDesc }; });
        var r = opts.filter(function (x) { return int(x.Id) === int(id); })[0];
        return r ? r.Name : '';
    }

    /* =====================================================================
     * PAYMENT SCHEDULE GRID  (grdPaymentTerm)
     *
     * Two-way calculation from grdPaymentTerm_CellUpdated:
     *   %OfTotal -> Amount   (capped at 100%)
     *   Amount   -> %OfTotal (capped at the detail total)
     *   DueDays  -> DueDate  (DocDate + days)
     *   DueDate  -> DueDays  (rejects a date before DocDate)
     * X (20px) and + (20px) are the desktop's own cell buttons at positions 0 and 1.
     * ===================================================================== */
    var PAYMENT_COLUMNS = [
        { key: 'PaymentTerm',  caption: 'Payment Term',   w: 120, combo: 'paymentTerms' },
        { key: 'DueDays',      caption: 'DueDays',        w: 60,  num: true, edit: true },
        { key: '%OfTotal',     caption: '%OfTotal',       w: 90,  num: true, edit: true },
        { key: 'Amount',       caption: 'Amount',         w: 110, num: true, edit: true, total: true },
        { key: 'BaseDateType', caption: 'Base Date Type', w: 113, combo: 'paymentBaseDates' },
        { key: 'DueDate',      caption: 'DueDate',        w: 93,  date: true, edit: true }
    ];

    function addPaymentRow() {
        dtPaymentTerm.push({ PaymentTerm: 0, DueDays: 0, '%OfTotal': 0, Amount: 0, BaseDateType: 0, DueDate: '' });
        renderPaymentGrid();
    }

    function renderPaymentGrid() {
        // BindGrids (:1456) / delete (:2103): the desktop grid never shows zero rows.
        if (!dtPaymentTerm.length) dtPaymentTerm.push({ PaymentTerm: 0, DueDays: 0, '%OfTotal': 0, Amount: 0, BaseDateType: 0, DueDate: '' });
        var h = '<colgroup><col style="width:20px"><col style="width:20px">';
        PAYMENT_COLUMNS.forEach(function (c) { h += '<col style="width:' + c.w + 'px">'; });
        h += '</colgroup><thead><tr><th>X</th><th>+</th>';
        PAYMENT_COLUMNS.forEach(function (c) { h += '<th>' + esc(c.caption) + '</th>'; });
        h += '</tr></thead><tbody>';

        dtPaymentTerm.forEach(function (r, i) {
            h += '<tr data-i="' + i + '">';
            h += '<td class="ctr"><button type="button" class="grid-cell-btn del" data-act="pay-del" data-i="' + i + '">X</button></td>';
            h += '<td class="ctr"><button type="button" class="grid-cell-btn" data-act="pay-add" data-i="' + i + '">+</button></td>';

            h += '<td class="edit">' + selectCell('pay', i, 'PaymentTerm', LK.paymentTerms, 'Id', 'TermsDescription', r.PaymentTerm) + '</td>';
            h += '<td class="edit"><input type="text" class="num" data-grid="pay" data-i="' + i + '" data-k="DueDays" value="' + esc(int(r.DueDays)) + '"></td>';
            h += '<td class="edit"><input type="text" class="num" data-grid="pay" data-i="' + i + '" data-k="%OfTotal" value="' + esc(fmt3(r['%OfTotal'])) + '"></td>';
            h += '<td class="edit"><input type="text" class="num" data-grid="pay" data-i="' + i + '" data-k="Amount" value="' + esc(fmt3(r.Amount)) + '"></td>';
            h += '<td class="edit">' + selectCell('pay', i, 'BaseDateType', LK.paymentBaseDates, 'Id', 'Name', r.BaseDateType) + '</td>';
            h += '<td class="edit"><input type="date" data-grid="pay" data-i="' + i + '" data-k="DueDate" value="' + esc(toIsoDate(r.DueDate)) + '"></td>';
            h += '</tr>';
        });

        h += '</tbody><tfoot><tr><td></td><td></td>';
        PAYMENT_COLUMNS.forEach(function (c) {
            h += '<td' + (c.num ? ' class="num"' : '') + '>'
               + (c.total ? fmt3(dtPaymentTerm.reduce(function (a, r) { return a + num(r.Amount); }, 0)) : '')
               + '</td>';
        });
        h += '</tr></tfoot>';
        $('#grdPaymentTerm').html(h);
    }

    function selectCell(grid, i, key, rows, vf, tf, selected) {
        var h = '<select data-grid="' + grid + '" data-i="' + i + '" data-k="' + key + '"><option value="0"></option>';
        (rows || []).forEach(function (r) {
            h += '<option value="' + esc(r[vf]) + '"' + (int(r[vf]) === int(selected) ? ' selected' : '')
               + '>' + esc(r[tf]) + '</option>';
        });
        return h + '</select>';
    }

    function paymentCellUpdated(i, key) {
        var r = dtPaymentTerm[i];
        if (!r) return;
        if (!dtDetail.length) {
            status('No Detail Record Found', true);
            return;
        }
        var total = sumDetail('TotalAmount');
        var docDate = $('#datDocDate').val();

        if (key === '%OfTotal') {
            var pct = num(r['%OfTotal']);
            if (pct > 100) { window.alert("%of Total Can't Greater than 100"); pct = 100; r['%OfTotal'] = 100; }
            r.Amount = Math.round(total * pct / 100 * 10000) / 10000;
        } else if (key === 'Amount') {
            var amt = num(r.Amount);
            if (total < amt) {
                window.alert('Amount Cant be Greater than Order Amount:' + total);
                amt = 0; r.Amount = 0;
            }
            r['%OfTotal'] = total ? Math.round(amt * 100 / total * 1e8) / 1e8 : 0;
        } else if (key === 'DueDays') {
            r.DueDate = addDays(docDate, r.DueDays);
        } else if (key === 'DueDate') {
            if (r.DueDate && docDate && r.DueDate < docDate) {
                r.DueDate = docDate;
                window.alert("Due Date Can't less Than DocDate");
            }
            r.DueDays = daysBetween(docDate, r.DueDate);
        }
        renderPaymentGrid();
        refreshPaymentScheduleText();
    }

    /** The desktop mirrors the grid into txtPaymentScheduleRemarks in this exact format. */
    function refreshPaymentScheduleText() {
        var sum = dtPaymentTerm.reduce(function (a, r) { return a + num(r.Amount); }, 0);
        if (!dtPaymentTerm.length || sum <= 0) return;
        $('#txtPaymentScheduleRemarks').val(dtPaymentTerm.map(function (x) {
            return '[Term:' + int(x.PaymentTerm) + ',DueDays:' + int(x.DueDays)
                 + ',%OfTotal:' + num(x['%OfTotal'])
                 + ',DueAmount:' + num(x.Amount)
                 + ',BaseDateType:' + int(x.BaseDateType) + ']';
        }).join(', '));
    }

    function renderAllGrids() {
        renderDetailGrid();
        renderExpenseGrid();
        renderEmptyBagGrid();
        renderPaymentGrid();
    }

    /* =====================================================================
     * HISTORY
     *
     * Columns come from HistoryFill()'s own projection and GridSettingHistory()'s
     * widths. Two additions the user asked for and the desktop supports in its own
     * way: a row-select checkbox column, and DocNo (the voucher code) rendered as a
     * clickable link on every line that opens that order - the desktop reaches the
     * same place through its Edit cell button and row double-click.
     * ===================================================================== */
    var HISTORY_COLUMNS = [
        { key: 'DocNo',             caption: 'DocNo',              w: 70,  link: true },
        { key: 'DocDate',           caption: 'DocDate',            w: 88,  date: true },
        { key: 'CommissionAgent',   caption: 'CommissionAgent',    w: 170 },
        { key: 'BuyerName',         caption: 'BuyerName',          w: 170 },
        { key: 'DeliverToParty',    caption: 'DeliverToParty',     w: 170 },
        { key: 'DeliverToAddress',  caption: 'DeliverToAddress',   w: 170 },
        { key: 'DeliveryTerm',      caption: 'DeliveryTerm',       w: 70  },
        { key: 'DeliveryStartDate', caption: 'DeliveryStartDate',  w: 88,  date: true },
        { key: 'DeliveryDays',      caption: 'DeliveryDays',       w: 60,  num: true },
        { key: 'ValidityDate',      caption: 'Expiry Date',        w: 88,  date: true },
        { key: 'WhtApplied',        caption: 'WhtApplied',         w: 60,  check: true },
        { key: 'Status',            caption: 'Status',             w: 70  },
        { key: 'EntryUser',         caption: 'EntryUser',          w: 100 },
        { key: 'EntryDate',         caption: 'EntryDate',          w: 145, datetime: true },
        { key: 'ModifyUser',        caption: 'ModifyUser',         w: 100 },
        { key: 'ModifyDate',        caption: 'ModifyDate',         w: 145, datetime: true },
        { key: 'NoOfAttachments',   caption: 'NoOfAttachments',    w: 90,  num: true }
    ];

    /**
     * The service hands back a normalised, stable contract (see normalizeHistory server-side),
     * so this is a straight rename into the grid's column keys - no database column names and
     * no case guessing. The raw procedure projects DocNo / DocDate / BuyerName / Status with
     * capitals, which is exactly the trap this removes.
     */
    function mapHistoryRow(row) {
        return {
            Id:                row.id,
            DocumentTypeId:    row.documentTypeId,
            DocNo:             row.docNo,
            DocDate:           row.docDate,
            CommissionAgent:   row.commissionAgent,
            BuyerName:         row.buyerName,
            DeliverToParty:    row.deliverToParty,
            DeliverToAddress:  row.deliverToAddress,
            DeliveryTerm:      row.deliveryTerm,
            DeliveryStartDate: row.deliveryStartDate,
            DeliveryDays:      row.deliveryDays,
            ValidityDate:      row.validityDate,
            WhtApplied:        row.whtApplied,
            Status:            row.status,
            EntryUser:         row.entryUser,
            EntryDate:         row.entryDate,
            ModifyUser:        row.modifyUser,
            ModifyDate:        row.modifyDate,
            NoOfAttachments:   row.noOfAttachments
        };
    }

    function renderHistoryGrid() {
        var h = '<colgroup><col style="width:26px"><col style="width:40px"><col style="width:60px">';
        HISTORY_COLUMNS.forEach(function (c) { h += '<col style="width:' + c.w + 'px">'; });
        h += '</colgroup><thead><tr>';
        h += '<th class="frz ctr" style="left:0"><input type="checkbox" id="histCheckAll" title="Select all"></th>';
        h += '<th class="frz" style="left:26px">Edit</th>';
        h += '<th class="frz" style="left:66px">SaveAs</th>';
        HISTORY_COLUMNS.forEach(function (c) { h += '<th>' + esc(c.caption) + '</th>'; });
        h += '</tr></thead><tbody>';

        if (!historyRows.length) {
            h += '<tr><td colspan="' + (HISTORY_COLUMNS.length + 3) + '" class="ctr" style="color:#666">No records</td></tr>';
        }
        historyRows.forEach(function (r, i) {
            var selCls = int(r.Id) === selectedHistoryId ? ' class="sel"' : '';
            h += '<tr data-hi="' + i + '"' + selCls + '>';
            h += '<td class="frz ctr" style="left:0"><input type="checkbox" class="hist-check" data-id="' + esc(r.Id) + '"></td>';
            h += '<td class="frz ctr" style="left:26px"><button type="button" class="grid-cell-btn" data-act="hist-edit" data-id="' + esc(r.Id) + '">Edit</button></td>';
            h += '<td class="frz ctr" style="left:66px"><button type="button" class="grid-cell-btn" data-act="hist-saveas" data-id="' + esc(r.Id) + '">SaveAs</button></td>';

            HISTORY_COLUMNS.forEach(function (c) {
                var v = r[c.key];
                if (c.link) {
                    // the voucher code, clickable on every line
                    h += '<td class="ctr"><button type="button" class="grid-link" data-act="hist-open" data-id="'
                       + esc(r.Id) + '" title="Open Sale Order ' + esc(v) + '">' + esc(v) + '</button></td>';
                } else if (c.check) {
                    h += '<td class="ctr"><input type="checkbox" disabled' + (v === true || v === 1 ? ' checked' : '') + '></td>';
                } else if (c.date) {
                    h += '<td class="ctr">' + esc(fmtDate(v)) + '</td>';
                } else if (c.datetime) {
                    h += '<td class="ctr">' + esc(v ? fmtDate(v) + ' ' + new Date(v).toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit' }) : '') + '</td>';
                } else if (c.num) {
                    h += '<td class="num">' + esc(v === null || v === undefined ? '' : v) + '</td>';
                } else {
                    h += '<td title="' + esc(v) + '">' + esc(v === null || v === undefined ? '' : v) + '</td>';
                }
            });
            h += '</tr>';
        });
        h += '</tbody>';
        $('#grdHistory').html(h);
        fixFrozenLefts('#grdHistory');
    }
    $(window).on('resize', function () { if ($('#mainpanel-history').hasClass('active')) fixFrozenLefts('#grdHistory'); });

    /* Frozen (sticky) columns: the browser may widen Edit/SaveAs past their <col> width,
       so the hard-coded left offsets made SaveAs slide over DocNo. Re-derive each frozen
       column's left from the rendered header widths. */
    function fixFrozenLefts(sel) {
        var $t = $(sel), lefts = [], x = 0;
        $t.find('thead tr:first th.frz').each(function () {
            lefts.push(x); x += this.getBoundingClientRect().width;
        });
        $t.find('tr').each(function () {
            $(this).children('.frz').each(function (k) { if (k < lefts.length) this.style.left = lefts[k] + 'px'; });
        });
    }

    /* =====================================================================
     * SupfrmShipToAddress (desktop dialog opened by BtnAddShiptoAddress_Click :4006)
     * ===================================================================== */
    var PO_API = '/api/commission/purchase-order';
    var stCitiesLoaded = false, stCombosLoaded = false;
    var SHIPTO_COLUMNS = [   // gridFill(): Id (hidden) + 13 columns, captions = column names (RetrieveStructure)
        { key: 'PartyName', caption: 'CompanyName' }, { key: 'AddressTitle', caption: 'AddressTitle' },
        { key: 'CountryName', caption: 'Country' }, { key: 'CityName', caption: 'City' },
        { key: 'ContactPerson', caption: 'ContactPerson' }, { key: 'PhoneNo', caption: 'PhoneNo' },
        { key: 'MobileNo', caption: 'MobileNo' }, { key: 'WhatsAppNo', caption: 'WhatsAppNo' },
        { key: 'EntryDate', caption: 'EntryDate', dt: true }, { key: 'EntryUser', caption: 'EntryUser' },
        { key: 'ModifyDate', caption: 'ModifyDate', dt: true }, { key: 'ModifyUser', caption: 'ModifyUser' },
        { key: 'AddressLine1', caption: 'AddressLine1' }
    ];
    function stCol(r, k) {
        if (!r) return null;
        if (r[k] !== undefined) return r[k];
        var lk = k.toLowerCase();
        for (var p in r) { if (p.toLowerCase() === lk) return r[p]; }
        return null;
    }
    function stStatus(msg, isError) { $('#stStatus').text(msg || '').css('color', isError ? '#c00' : '#333'); }
    function openShipToDialog() {
        $('#soShipToDlg').addClass('open');
        shipToReset();
        $.when(stCombosLoaded ? null : shipToFillCombos(false), shipToGridFill());
        setTimeout(function () { $('#stSupplier').trigger('focus'); }, 50);
    }
    function closeShipToDialog() {
        $('#soShipToDlg').removeClass('open');
        // back on the Sale Order: re-read AllShipToAddress() so a new address is selectable
        $.getJSON(API + '/ship-to-addresses').done(function (rows) {
            LK.shipToAddresses = rows || [];
            bindShipToAddresses(int($('#cmbDeliveryToParty').val()));
        });
    }
    /* cmbsupplierfill (SupplierCustomerGetforComboServiceBind, CompanyName) + cmbcountryfill (Country.GetAll, Description) */
    function shipToFillCombos(withCities) {
        bindCombo('#stSupplier', (LK.parties || []).map(function (r) {
            return { Id: r.Id, CompanyName: r.CompanyName || r.DisplayCompany };
        }), 'Id', 'CompanyName', { keepValue: true });
        var jobs = [$.getJSON(PO_API + '/ship-to/countries').done(function (rows) {
            bindCombo('#stCountry', (rows || []).map(function (r) {
                return { Id: stCol(r, 'Id'), Name: stCol(r, 'Description') || stCol(r, 'CountryName') };
            }), 'Id', 'Name', { keepValue: true });
        })];
        if (withCities || !stCitiesLoaded) jobs.push(shipToLoadCities());
        return $.when.apply($, jobs).done(function () { stCombosLoaded = true; })
            .fail(function (xhr) { stStatus('Could not load the dropdowns: ' + (xhr.responseText || xhr.statusText), true); });
    }
    function shipToLoadCities() {
        return $.getJSON(PO_API + '/ship-to/cities').done(function (rows) {
            stCitiesLoaded = true;
            bindCombo('#stCity', (rows || []).map(function (r) {
                return { Id: stCol(r, 'Id'), Name: stCol(r, 'CityName') };
            }), 'Id', 'Name', { keepValue: true });
        });
    }
    /* gridFill(): SupplierCustomerShipToAddress.FormHistory(Org, Company) */
    function shipToGridFill() {
        return $.getJSON(PO_API + '/ship-to/history').done(function (rows) {
            var h = '<thead><tr>';
            SHIPTO_COLUMNS.forEach(function (c) { h += '<th>' + esc(c.caption) + '</th>'; });
            h += '</tr></thead><tbody>';
            (rows || []).forEach(function (r) {
                h += '<tr data-id="' + esc(stCol(r, 'Id')) + '">';
                SHIPTO_COLUMNS.forEach(function (c) {
                    var v = stCol(r, c.key);
                    if (c.dt) v = v ? fmtDate(v) + ' ' + new Date(v).toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit' }) : '';
                    h += '<td title="' + esc(v) + '">' + esc(v === null || v === undefined ? '' : v) + '</td>';
                });
                h += '</tr>';
            });
            $('#grdShipTo').html(h + '</tbody>');
        }).fail(function (xhr) { stStatus('Could not load the history: ' + (xhr.responseText || xhr.statusText), true); });
    }
    /* Reset() */
    function shipToReset() {
        $('#stRecId').val(0);
        $('#stSupplier,#stCity,#stCountry').val('').trigger('change');
        $('#stAddress,#stAddressTitle,#stContactPerson,#stWhatsApp,#stCell,#stTelephone').val('');
        $('#stBtnUpdate').hide(); $('#stBtnSave').show();
        stStatus('');
    }
    /* grdfrm_CellContentDoubleClick -> GetByID, Save hidden / Update shown */
    function shipToEdit(id) {
        if (!id) return;
        $.getJSON(PO_API + '/ship-to/' + id).done(function (b) {
            $('#stBtnSave').hide(); $('#stBtnUpdate').show();
            $('#stRecId').val(id);
            $('#stSupplier').val(String(int(stCol(b, 'SupplierCustomerId')))).trigger('change');
            $('#stCountry').val(String(int(stCol(b, 'CountryId')))).trigger('change');
            $('#stCity').val(String(int(stCol(b, 'CityId')))).trigger('change');
            $('#stAddress').val(stCol(b, 'AddressLine1') || '');
            $('#stAddressTitle').val(stCol(b, 'AddressTitle') || '');
            $('#stWhatsApp').val(stCol(b, 'WhatsAppNo') || '');
            $('#stContactPerson').val(stCol(b, 'ContactPerson') || '');
            $('#stCell').val(stCol(b, 'MobileNo') || '');
            $('#stTelephone').val(stCol(b, 'PhoneNo') || '');
        }).fail(function (xhr) { alert(xhr.responseText || xhr.statusText); });
    }
    /* FormValidation() + Insert(): same messages/order; RecId>0 -> update */
    function shipToInsert(btn) {
        var checks = [
            [!int($('#stSupplier').val()), 'Please Select Supplier', '#stSupplier'],
            [!$.trim($('#stAddress').val()), 'Please Enter Address', '#stAddress'],
            [!$.trim($('#stAddressTitle').val()), 'Please Enter Address Title', '#stAddressTitle'],
            [!int($('#stCountry').val()), 'Please Select Country', '#stCountry'],
            [!int($('#stCity').val()), 'Please Select City', '#stCity']
        ];
        for (var i = 0; i < checks.length; i++) {
            if (checks[i][0]) { alert(checks[i][1]); $(checks[i][2]).trigger('focus'); return; }
        }
        var $b = $(btn);
        if ($b.prop('disabled')) return;
        $b.prop('disabled', true);
        var recId = int($('#stRecId').val());
        $.ajax({
            url: PO_API + '/ship-to/save', type: 'POST', contentType: 'application/json',
            data: JSON.stringify({
                Id: recId, SupplierCustomerId: int($('#stSupplier').val()),
                CountryId: int($('#stCountry').val()), CityId: int($('#stCity').val()),
                AddressLine1: $.trim($('#stAddress').val()), AddressTitle: $.trim($('#stAddressTitle').val()),
                PhoneNo: $.trim($('#stTelephone').val()), MobileNo: $.trim($('#stCell').val()),
                WhatsAppNo: $.trim($('#stWhatsApp').val()), ContactPerson: $.trim($('#stContactPerson').val())
            })
        }).done(function (d) {
            if (!d || !d.success) { alert((d && d.message) || 'Save refused.'); return; }
            alert(recId === 0 ? 'Save Successfully' : 'Update Successfully');
            shipToGridFill();
            shipToReset();
        }).fail(function (xhr) {
            alert(xhr.responseText || xhr.statusText);
        }).always(function () { $b.prop('disabled', false); });
    }

    function loadHistory() {
        busy(true, 'Loading history…');
        var q = {
            dateMode: $('input[name=histDateMode]:checked').val(),
            fromDate: $('#fromDateHistory').val(),
            toDate: $('#toDateHistory').val(),
            validityFrom: $('#datValidityFromHistory').val(),
            validityTo: $('#datValidityToHistory').val(),
            commissionAgentId: int($('#cmbCommissionAgentHistory').val()) || '',
            buyerId: int($('#cmbBuyerNameHistory').val()) || '',
            itemId: int($('#cmbItemNameHistory').val()) || '',
            parentItemIds: historyParentIds().join(','),
            deliveryToPartyId: int($('#cmbDeliveryToPartyHistory').val()) || '',
            shipToAddress: $('#cmbDeliverToAddressHistory option:selected').text() || ''
        };
        return $.getJSON(API + '/history', q)
            .done(function (rows) {
                historyRows = (rows || []).map(mapHistoryRow);
                renderHistoryGrid();
                clearHistoryChildGrids();
                $('#historyStatus').text(historyRows.length + ' record(s)').removeClass('err');
            })
            .fail(function (xhr) {
                $('#historyStatus').text('History failed: ' + (xhr.responseText || xhr.statusText)).addClass('err');
            })
            .always(function () { busy(false); });
    }

    function clearHistoryChildGrids() {
        $('#grdHistoryDetail,#grdExpenseHistory,#grdEmptyBagHistory,#grdPaymentTermHistory').empty();
    }

    /** GetDetailGrdByHeadId - fill the four read-only child grids for the selected row. */
    function loadHistoryChildren(id) {
        selectedHistoryId = int(id);
        renderHistoryGrid();
        busy(true);
        $.getJSON(API + '/' + id)
            .done(function (d) {
                renderReadOnlyGrid('#grdHistoryDetail', DETAIL_COLUMNS,
                    (d.detail || []).map(mapDetailFromDb));

                renderReadOnlyGrid('#grdExpenseHistory', [
                    { key: 'OtherItemName', caption: 'OtherItemName', w: 150 },
                    { key: 'Qty',     caption: 'Qty',     w: 70, num: true, total: true },
                    { key: 'Rate',    caption: 'Rate',    w: 70, num: true },
                    { key: 'Amount',  caption: 'Amount',  w: 90, num: true, total: true },
                    { key: 'Remarks', caption: 'Remarks', w: 300 }
                ], (d.expenses || []).map(function (r) {
                    return { OtherItemName: r.OtherItemName, Qty: r.Qty, Rate: r.rate, Amount: r.amount, Remarks: r.remarks };
                }));

                renderReadOnlyGrid('#grdEmptyBagHistory', [
                    { key: 'PackingType',  caption: 'PackingType',  w: 150 },
                    { key: 'WeightCut',    caption: 'WeightCut',    w: 80, num: true },
                    { key: 'PurchaseRate', caption: 'PurchaseRate', w: 70, num: true }
                ], (d.emptyBags || []).map(function (r) {
                    return { PackingType: r.PackingType || emptyBagPackName(r.PackingTypeId),
                             WeightCut: r.weightCutKg, PurchaseRate: r.Rate };
                }));

                renderReadOnlyGrid('#grdPaymentTermHistory', [
                    { key: 'PaymentTerm',  caption: 'PaymentTerm',  w: 80 },
                    { key: 'DueDays',      caption: 'DueDays',      w: 70, num: true },
                    { key: '%OfTotal',     caption: '%OfTotal',     w: 70, num: true },
                    { key: 'Amount',       caption: 'Amount',       w: 90, num: true, total: true },
                    { key: 'BaseDateType', caption: 'BaseDateType', w: 130 },
                    { key: 'DueDate',      caption: 'DueDate',      w: 88, date: true }
                ], (d.payments || []).map(function (r) {
                    return { PaymentTerm: r.PaymentTerm, DueDays: r.DueDays, '%OfTotal': r.pctOfTotal,
                             Amount: r.dueAmount, BaseDateType: r.BaseDateType, DueDate: r.DueDate };
                }));
            })
            .always(function () { busy(false); });
    }

    function renderReadOnlyGrid(sel, columns, rows) {
        var h = '<colgroup>';
        columns.forEach(function (c) { h += '<col style="width:' + c.w + 'px">'; });
        h += '</colgroup><thead><tr>';
        columns.forEach(function (c) { h += '<th>' + esc(c.caption) + '</th>'; });
        h += '</tr></thead><tbody>';
        if (!rows.length) {
            h += '<tr><td colspan="' + columns.length + '" class="ctr" style="color:#666">No rows</td></tr>';
        }
        rows.forEach(function (r) {
            h += '<tr>';
            columns.forEach(function (c) {
                var v = r[c.key];
                if (c.date) h += '<td class="ctr">' + esc(fmtDate(v)) + '</td>';
                else if (c.num) h += '<td class="num">' + esc(fmt3(v)) + '</td>';
                else h += '<td title="' + esc(v) + '">' + esc(v === null || v === undefined ? '' : v) + '</td>';
            });
            h += '</tr>';
        });
        h += '</tbody><tfoot><tr>';
        columns.forEach(function (c) {
            h += '<td' + (c.num ? ' class="num"' : '') + '>'
               + (c.total ? fmt3(rows.reduce(function (a, r) { return a + num(r[c.key]); }, 0)) : '') + '</td>';
        });
        h += '</tr></tfoot>';
        $(sel).html(h);
    }

    /* =====================================================================
     * LOAD / SAVE / DELETE
     * ===================================================================== */

    /** FillDetailFromListCommonForReadById - db row -> grid row. */
    function mapDetailFromDb(r) {
        var amount = num(r.ItemAmount);
        var taxAmount = num(r.TaxAmount);
        return {
            Id: int(r.saleOrderDetailId),
            ParentItemId: int(r.inventoryParentCategoryId),
            ParentItem: r.inventoryParentCategory || '',
            ItemId: int(r.itemId),
            ItemName: int(r.itemId) > 0 ? (r.ItemName || '') : '',
            CropYearId: int(r.cropYearId),
            CropYear: r.cropYear || '',
            PackingTypeId: int(r.packingTypeId),
            PackingType: r.PackingType || '',
            PackUomId: int(r.packUomId),
            PackUom: r.PackUomCode || '',
            Qty: num(r.itemQty),
            Weight: num(r.itemWeight),
            Rate: num(r.itemRate),
            RateUomId: int(r.rateUomId),
            RateUom: r.RateUomCode || '',
            Amount: amount,
            TaxNameId: int(r.TaxNameId),
            TaxName: int(r.TaxNameId) > 0 ? (r.TaxName || '') : '',
            'Tax%': num(r.TaxPercent),
            TaxAmount: taxAmount,
            TotalAmount: amount + taxAmount,
            Remarks: r.remarks || ''
        };
    }

    /** The grid row shape -> the procedure's parameter shape. */
    function detailRowToDto(r) {
        return {
            saleOrderDetailId: int(r.Id),
            inventoryParentCategoryId: int(r.ParentItemId),
            itemId: int(r.ItemId),
            cropYearId: int(r.CropYearId),
            cropYear: r.CropYear || '',
            packingTypeId: int(r.PackingTypeId),
            packUomId: int(r.PackUomId),
            itemQty: num(r.Qty),
            itemWeight: num(r.Weight),
            itemRate: num(r.Rate),
            rateUomId: int(r.RateUomId),
            ItemAmount: num(r.Amount),
            TaxNameId: int(r.TaxNameId),
            TaxPercent: num(r['Tax%']),
            TaxAmount: num(r.TaxAmount),
            TotalAmount: num(r.TotalAmount),
            remarks: r.Remarks || '',
            ItemName: r.ItemName || '',
            PackingType: r.PackingType || '',
            RateUomCode: r.RateUom || '',
            TaxName: r.TaxName || ''
        };
    }

    /** ReadById - load an existing order into the form. */
    function loadOrder(id, asSaveAs) {
        busy(true, 'Loading order…');
        $.getJSON(API + '/' + id)
            .done(function (d) {
                resetForm(true);
                var h = d.header || {};
                RecId = int(h.saleOrderMasterId);

                $('#txtDocNo').val(h.docNo);
                $('#datDocDate').val(asSaveAs ? today() : toIsoDate(h.docDate));
                $('#cmbCompanyName').val(h.companyId).trigger('change.select2');
                $('#cmbCommissionAgent').val(h.commissionAgentId).trigger('change.select2');
                $('#cmbBuyerName').val(h.buyerId).trigger('change.select2');
                $('#cmbDeliveryToParty').val(h.deliveryToPartyId).trigger('change.select2');
                bindShipToAddresses(int(h.deliveryToPartyId));
                if (int(h.shipToAddressId) > 0) $('#cmbShipToAddress').val(h.shipToAddressId).trigger('change.select2');
                $('#txtShipToAddress').val(h.shipToAddress || '');
                $('#txtBuyerReference').val(h.buyerReferenceNo || '');
                $('#cmbDeliveryTerm').val(h.deliveryTermId).trigger('change.select2');
                $('#txtDeliveryDays').val(h.deliveryDays);
                $('#datDeliveryStartDate').val(toIsoDate(h.deliveryStartDate));
                $('#datExpiryDate').val(toIsoDate(h.validityDate));
                $('#txtRemarks').val(h.remarksHeader || '');
                $('#txtPaymentScheduleRemarks').val(h.paymentScheduleDescription || '');
                $('#chkOtherExpenseAllowed').prop('checked', !!h.isBuyerOtherChargesAllowed);
                $('#chkWithHoldingTaxApplied').prop('checked', !!h.isWhtApplied);
                $('input[name=ebPolicy][value="' + int(h.ebWeightDeductionTermId) + '"]').prop('checked', true);

                // commission blocks, split by agentTypeId (1 = commission, 2 = brokery)
                (d.commissions || []).forEach(function (c) {
                    if (int(c.agentTypeId) === 1) {
                        $('#cmbCommissionAc').val(c.commissionAgentId).trigger('change.select2');
                        $('#cmbCommType').val(c.commissionTypeId).trigger('change.select2');
                        $('#txtCommRate').val(c.commissionRate);
                        if (int(c.rateUomId) > 0) $('#cmbCommUom').val(c.rateUomId).trigger('change.select2');
                        $('#txtCommAmount').val(c.commissionAmount);
                    } else if (int(c.agentTypeId) === 2) {
                        $('#cmbBrokeryAc').val(c.commissionAgentId).trigger('change.select2');
                        $('#cmbBrokeryType').val(c.commissionTypeId).trigger('change.select2');
                        $('#txtBrokeryRate').val(c.commissionRate);
                        if (int(c.rateUomId) > 0) $('#cmbBrokeryRateUom').val(c.rateUomId).trigger('change.select2');
                        $('#txtBrokeryAmount').val(c.commissionAmount);
                    }
                });

                dtDetail = (d.detail || []).map(mapDetailFromDb);

                dtEmptyBags = (d.emptyBags || []).map(function (r) {
                    return { PackingType: int(r.PackingTypeId), WeightCut: num(r.weightCutKg), PurchaseRate: num(r.Rate) };
                });
                if (!dtEmptyBags.length) seedEmptyBagGrid();

                // The expense grid is re-seeded with every other item, then the saved rows
                // are merged in by ItemId - exactly as the desktop's ReadById does.
                seedExpenseGrid();
                (d.expenses || []).forEach(function (x) {
                    var row = dtExpGrid.filter(function (e) { return int(e.ItemId) === int(x.ItemId); })[0];
                    if (row) {
                        row.Id = int(x.saleOrderbuyerExpenseDetailId);
                        row.Qty = num(x.Qty);
                        row.Rate = num(x.rate);
                        row.Amount = num(x.amount);
                        row.Remarks = x.remarks || '';
                    }
                });

                dtPaymentTerm = (d.payments || []).map(function (p) {
                    return {
                        PaymentTerm: int(p.PaymentTermId), DueDays: int(p.DueDays),
                        '%OfTotal': num(p.pctOfTotal), Amount: num(p.dueAmount),
                        BaseDateType: int(p.BaseDueDateTypeId), DueDate: toIsoDate(p.DueDate)
                    };
                });
                if (dtPaymentTerm.length === 1) {
                    $('#cmbPaymentTerm').val(dtPaymentTerm[0].PaymentTerm).trigger('change.select2');
                    $('#txtDueDays').val(dtPaymentTerm[0].DueDays);
                }

                renderAllGrids();
                afterDetailChanged();

                if (asSaveAs) {
                    // DataGridHistory_SaveAs: keep the loaded content but save it as a NEW order.
                    RecId = 0;
                    dtDetail.forEach(function (r) { r.Id = 0; });
                    removedDetailRows = [];
                    renderDetailGrid();
                    $('#btnSave').hide(); $('#btnUpdate').hide(); $('#btnDelete').hide(); $('#btnSaveAs').show();
                    loadNextDocNo();
                } else {
                    $('#btnSave').hide(); $('#btnSaveAs').hide();
                    $('#btnUpdate').show(); $('#btnDelete').show();
                }

                $('.win-tab[data-maintab=form]').trigger('click');
                status('');
            })
            .fail(function (xhr) {
                status('Could not load that order: ' + (xhr.responseText || xhr.statusText), true);
            })
            .always(function () { busy(false); });
    }

    /** Insert() - build the payload exactly as the desktop builds its model. */
    function buildPayload() {
        var commissions = [];
        if (num($('#txtCommAmount').val()) > 0 && int($('#cmbCommissionAc').val()) > 0) {
            commissions.push({
                agentTypeId: 1,
                commissionAgentId: int($('#cmbCommissionAc').val()),
                commissionTypeId: int($('#cmbCommType').val()),
                commissionRate: num($('#txtCommRate').val()),
                rateUomId: int($('#cmbCommUom').val()),
                commissionAmount: num($('#txtCommAmount').val())
            });
        }
        if (num($('#txtBrokeryAmount').val()) > 0 && int($('#cmbBrokeryAc').val()) > 0) {
            commissions.push({
                agentTypeId: 2,
                commissionAgentId: int($('#cmbBrokeryAc').val()),
                commissionTypeId: int($('#cmbBrokeryType').val()),
                commissionRate: num($('#txtBrokeryRate').val()),
                rateUomId: int($('#cmbBrokeryRateUom').val()),
                commissionAmount: num($('#txtBrokeryAmount').val())
            });
        }

        return {
            saleOrderMasterId: RecId,
            docNo: int($('#txtDocNo').val()),
            docDate: $('#datDocDate').val(),
            ValidityDate: $('#datExpiryDate').val(),
            commissionAgentId: int($('#cmbCommissionAgent').val()),
            buyerId: int($('#cmbBuyerName').val()),
            DeliveryToPartyId: int($('#cmbDeliveryToParty').val()),
            buyerReferenceNo: $('#txtBuyerReference').val(),
            shipToAddressId: int($('#cmbShipToAddress').val()),
            ShipToAddress: $('#txtShipToAddress').val(),
            deliveryTermId: int($('#cmbDeliveryTerm').val()),
            deliveryDays: int($('#txtDeliveryDays').val()),
            deliveryStartDate: $('#datDeliveryStartDate').val(),
            remarksHeader: $('#txtRemarks').val(),
            companyId: int($('#cmbCompanyName').val()),
            isWhtApplied: $('#chkWithHoldingTaxApplied').is(':checked'),
            isbuyerOtherChargesAllowed: $('#chkOtherExpenseAllowed').is(':checked'),
            EBWeightDeductionTermId: int($('input[name=ebPolicy]:checked').val()),

            headerPaymentTermId: int($('#cmbPaymentTerm').val()),
            headerDueDays: int($('#txtDueDays').val()),

            saleOrderDetailList: dtDetail.map(detailRowToDto),
            removedDetailRows: removedDetailRows,

            saleOrderBuyerExpenseDetailList: dtExpGrid
                .filter(function (r) { return int(r.ItemId) !== 0 && num(r.Amount) > 0; })
                .map(function (r) {
                    return {
                        saleOrderbuyerExpenseDetailId: int(r.Id),
                        ItemId: int(r.ItemId),
                        OtherItemName: otherItemName(r.ItemId),
                        Qty: num(r.Qty),
                        rate: num(r.Rate),
                        amount: num(r.Amount),
                        remarks: r.Remarks || ''
                    };
                }),

            saleOrderEmptyBagDetailList: dtEmptyBags.map(function (r) {
                return {
                    PackingTypeId: int(r.PackingType),
                    PackingType: emptyBagPackName(r.PackingType),
                    weightCutKg: num(r.WeightCut),
                    Rate: num(r.PurchaseRate)
                };
            }),

            saleOrderCommissionDetailList: commissions,

            saleOrderPaymentDetailList: dtPaymentTerm.map(function (r) {
                return {
                    PaymentTermId: int(r.PaymentTerm),
                    PaymentTerm: paymentTermName(r.PaymentTerm),
                    DueDays: int(r.DueDays),
                    pctOfTotal: num(r['%OfTotal']),
                    dueAmount: num(r.Amount),
                    BaseDueDateTypeId: int(r.BaseDateType),
                    DueDate: r.DueDate || null
                };
            })
        };
    }

    function paymentTermName(id) {
        var r = (LK.paymentTerms || []).filter(function (x) { return int(x.Id) === int(id); })[0];
        return r ? r.TermsDescription : '';
    }

    function save() {
        if (!dtDetail.length) { status('Detail list not found', true); return; }
        if (!window.confirm(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;

        busy(true, 'Saving…');
        $.ajax({
            url: API + '/save',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(buildPayload())
        }).done(function (res) {
            if (res && res.success) {
                status(res.message || 'Save Successfully');
                window.alert(res.message || 'Save Successfully');
                resetForm(false);
            } else {
                var msg = (res && res.message) || 'Save failed';
                status(msg, true);
                window.alert(msg);
                if (res && res.tab) $('.win-tab[data-tab="' + res.tab + '"]').trigger('click');
            }
        }).fail(function (xhr) {
            status('Save failed: ' + (xhr.responseText || xhr.statusText), true);
        }).always(function () { busy(false); });
    }

    function remove() {
        if (RecId <= 0) { window.alert('No record found to Delete'); return; }
        if (!window.confirm('Are you sure to Delete?')) return;
        busy(true, 'Deleting…');
        $.ajax({ url: API + '/delete/' + RecId, method: 'POST' })
            .done(function (res) {
                window.alert((res && res.message) || 'Delete Record Successfully');
                if (res && res.success) resetForm(false);
            })
            .fail(function (xhr) { status('Delete failed: ' + (xhr.responseText || xhr.statusText), true); })
            .always(function () { busy(false); });
    }

    function loadNextDocNo() {
        busy(true);
        return $.getJSON(API + '/next-doc-no')
            .done(function (d) { $('#txtDocNo').val(d.docNo); })
            .always(function () { busy(false); });
    }

    /** Reset() - back to a blank new order. */
    function resetForm(skipDocNo) {
        RecId = 0;
        removedDetailRows = [];
        dtDetail = [];
        dtPaymentTerm = [];
        seedExpenseGrid();
        seedEmptyBagGrid();

        $('#btnSave').show(); $('#btnSaveAs').hide(); $('#btnUpdate').hide(); $('#btnDelete').hide();

        $('#cmbCommissionAgent,#cmbBuyerName,#cmbDeliveryToParty,#cmbShipToAddress,' +
          '#cmbPaymentTerm,#cmbCommissionAc,#cmbCommType,#cmbCommUom,' +
          '#cmbBrokeryAc,#cmbBrokeryType,#cmbBrokeryRateUom').val('').trigger('change.select2');

        $('#txtDeliveryDays').val('');     // Reset():2264 empties it (the designer's "1" is first-open only)
        $('#txtDueDays,#txtRemarks,#txtCommRate,#txtCommAmount,#txtBrokeryRate,' +
          '#txtBrokeryAmount,#txtPaymentScheduleRemarks,#txtBuyerReference,#txtShipToAddress').val('');
        $('#chkWithHoldingTaxApplied,#chkOtherExpenseAllowed').prop('checked', false);
        // Desktop Reset() never touches panel7's radios; designer default is rdBagFocAndWeightCutNotApply.Checked=true (frmSaleOrderCmagt.cs:5600)
        if (!$('input[name=ebPolicy]:checked').length) $('input[name=ebPolicy][value="3"]').prop('checked', true);

        $('#datDocDate').val(today()).prop('disabled', false);
        $('#datDeliveryStartDate').val(today());
        calculateExpiryDate();

        resetDetailEntry();
        renderAllGrids();
        if (!skipDocNo) {
            loadNextDocNo();
            applyPortalDefaults();         // Reset() -> GetCommissionAgentConfigurationsFromGlobalandBind
        }
        status('');
    }

    /**
     * GetCommissionAgentConfigurationsFromGlobalandBind (:4040): the seven Commission Agent
     * Portal configuration ids a NEW document pre-selects, each applied only when > 0.
     * Served by the shared /api/commission/dropdowns/config-defaults. This screen has no
     * static payment-term fallback, so fallbackPaymentTermId is deliberately not used.
     */
    var portalDefaults = null;
    function applyPortalDefaults() {
        var apply = function () {
            var d = portalDefaults || {};
            [['commissionAgentId', '#cmbCommissionAgent'], ['commissionAccountId', '#cmbCommissionAc'],
             ['brokeryAccountId', '#cmbBrokeryAc'], ['deliveryTermId', '#cmbDeliveryTerm'],
             ['cropYearId', '#cmbCropYear'], ['packingTypeId', '#cmbPackingType']]
                .forEach(function (p) {
                    if (int(d[p[0]]) > 0) $(p[1]).val(int(d[p[0]])).trigger('change.select2');
                });
            // Setting CmbPaymentTerm.Value raises CmbPaymentTerm_ValueChanged on the desktop.
            if (int(d.paymentTermId) > 0) $('#cmbPaymentTerm').val(int(d.paymentTermId)).trigger('change');
        };
        if (portalDefaults) { apply(); return; }
        $.getJSON('/api/commission/dropdowns/config-defaults')
            .done(function (d) { portalDefaults = d || {}; apply(); })
            .fail(function () { portalDefaults = null; });
    }

    /** CalculateExpiryDate(): Delivery Start Date + Delivery Days. */
    function calculateExpiryDate() {
        var start = $('#datDeliveryStartDate').val();
        var days = int($('#txtDeliveryDays').val());
        $('#datExpiryDate').val(days === 0 ? start : addDays(start, days));
    }

    /* MakeShortCutKeys (:3933) - the desktop's key list, shown in a popup. */
    var SHORTCUT_KEYS = [
        ['Ctrl+S', 'For Save When on Entry form and For Show Data when on History Form'],
        ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+E', 'For Close'],
        ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
        ['Ctrl+F5', 'For Focus on Inquiry Date'], ['Ctrl+F10', 'For Open Attachments'],
        ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+ArrowDown', 'For Focus On Parent Item'], ['Ctrl+ArrowUp', 'For Focus On on Inquiry Date'],
        ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
        ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]
    ];
    function showShortCutKeys() {
        $('#soShortcutBody').html(SHORTCUT_KEYS.map(function (r) {
            return '<tr><td>' + esc(r[0]) + '</td><td>' + esc(r[1]) + '</td></tr>';
        }).join(''));
        $('#soShortcutDlg').addClass('open');
    }

    /* =====================================================================
     * EVENT WIRING
     * ===================================================================== */
    $(function () {
        initSelect2();

        $('#datDocDate').val(today());
        $('#datDeliveryStartDate').val(today());
        calculateExpiryDate();

        loadLookups().always(function () {
            loadNextDocNo(); applyPortalDefaults();
            /* report Doc No links open /commission/sale-order?id=N */
            var qm = /[?&]id=(\d+)/.exec(location.search);
            if (qm) loadOrder(+qm[1]);
        });

        // ---- main tabs (Form / History) ----
        $(document).on('click', '.win-tab[data-maintab]', function () {
            var t = $(this).data('maintab');
            $('.win-tab[data-maintab]').removeClass('active');
            $(this).addClass('active');
            $('#mainpanel-form,#mainpanel-history').removeClass('active');
            $('#mainpanel-' + t).addClass('active');
            if (t === 'history' && !historyRows.length) loadHistory();
            else if (t === 'history') fixFrozenLefts('#grdHistory');
        });

        // ---- detail tabs ----
        $(document).on('click', '.win-tab[data-tab]', function () {
            var t = $(this).data('tab');
            $('.win-tab[data-tab]').removeClass('active');
            $(this).addClass('active');
            $('#panel-detail,#panel-expenses,#panel-emptybags,#panel-payment').removeClass('active');
            $('#panel-' + t).addClass('active');
        });

        // ---- history child tabs ----
        $(document).on('click', '.win-tab[data-htab]', function () {
            var t = $(this).data('htab');
            $('.win-tab[data-htab]').removeClass('active');
            $(this).addClass('active');
            $('#hpanel-detail,#hpanel-expenses,#hpanel-emptybags,#hpanel-payment').removeClass('active');
            $('#hpanel-' + t).addClass('active');
        });

        // ---- Business Name / Nick Name ----
        $('#radBusinessName,#radNickName').on('change', bindPartyCombos);

        // ---- detail entry calculations ----
        $('#txtQty').on('input', calculateWeight);
        $('#cmbPackUom').on('change', calculateWeight);
        $('#txtWeight').on('input', calculateAmount);
        $('#txtRate').on('input', calculateAmount);
        $('#cmbRateUom').on('change', calculateAmount);
        $('#txtAmount').on('input', calculateTaxAmount);
        $('#cmbTaxName').on('change', calculateTaxAmount);

        $('#cmbParentItem').on('change', function () {
            bindItemCombo('#cmbItemName', int($(this).val()));
        });

        $('#cmbItemName').on('change', function () {
            var itemId = int($(this).val());
            if (itemId > 0) {
                // CmbItemName_Leave also back-fills the Parent Item from the chosen item.
                var it = (LK.items || []).filter(function (r) { return int(r.Id) === itemId; })[0];
                if (it) $('#cmbParentItem').val(it.InventoryParentCategoriesId).trigger('change.select2');
            }
            loadItemUoms(itemId);
            loadItemTax(itemId);
        });

        $('#cmbParentItemHistory').on('change', function () {
            // CmbParentItemHistory_Leave -> BindDropdownsAgainstParentCategory
            bindHistoryChildCombos(historyParentIds());
        });

        // ---- commission / brokery recalculation ----
        $('#cmbCommissionAc,#cmbCommType,#cmbCommUom').on('change', totalCommissionAmount);
        $('#txtCommRate').on('input', totalCommissionAmount);
        $('#cmbBrokeryAc,#cmbBrokeryType,#cmbBrokeryRateUom').on('change', totalBrokeryAmount);
        $('#txtBrokeryRate').on('input', totalBrokeryAmount);

        // ---- delivery dates ----
        $('#txtDeliveryDays').on('input', calculateExpiryDate);
        $('#datDeliveryStartDate').on('change', function () {
            var start = $(this).val(), doc = $('#datDocDate').val();
            if (start && doc && start < doc) {
                window.alert("Delivery Start Date Can't Be Less Than Doc Date");
                $(this).val(doc);
            }
            calculateExpiryDate();
        });

        // ---- CmbPaymentTerm_ValueChanged ----
        $('#cmbPaymentTerm').on('change', function () {
            var v = int($(this).val());
            $('#txtDueDays').prop('disabled', false);
            if (v === 1 || v === 3) {
                $('#txtDueDays').val('0').prop('disabled', true);
            } else if (v === 2 && int($('#txtDueDays').val()) === 0) {
                $('#txtDueDays').val('2');
            }
        });

        // ---- ship-to-address <-> deliver-to-party ----
        // CmbDeliveryToParty_Leave (:841): rebind the addresses; a cleared party clears the
        // Ship To Address COMBO. The free-text txtShipToAddress is never touched by the desktop.
        $('#cmbDeliveryToParty').on('change', function () {
            var partyId = int($(this).val());
            bindShipToAddresses(partyId);
            if (partyId === 0) $('#cmbShipToAddress').val('').trigger('change.select2');
        });

        $('#cmbShipToAddress').on('change', function () {
            var id = int($(this).val());
            var row = (LK.shipToAddresses || []).filter(function (r) { return int(r.Id) === id; })[0];
            // CmbShipToAddress_Leave (:858) only back-fills the Deliver / Ship To Party. It does
            // not write the address into txtShipToAddress, which is saved as ShipToAddress.
            if (row) {
                if (int(row.SupplierCustomerId) > 0) {
                    $('#cmbDeliveryToParty').val(row.SupplierCustomerId).trigger('change.select2');
                }
            }
        });

        // ---- detail row buttons ----
        $('#btnAddRow').on('click', addDetailRow);
        $('#btnUpdateRow').on('click', updateDetailRow);
        $('#btnCancelRow').on('click', resetDetailEntry);

        $(document).on('click', '#grdDetail button[data-act]', function () {
            var i = int($(this).data('i'));
            if ($(this).data('act') === 'del') deleteDetailRow(i);
            else editDetailRow(i);
        });
        $(document).on('dblclick', '#grdDetail tbody tr[data-i]', function () {
            editDetailRow(int($(this).data('i')));
        });

        // ---- inline grid editing ----
        $(document).on('change', '#grdInvExp input[data-grid=exp]', function () {
            var i = int($(this).data('i')), k = $(this).data('k');
            dtExpGrid[i][k] = k === 'Remarks' ? $(this).val() : num($(this).val());
            expenseCellUpdated(i, k);
        });

        $(document).on('change', '#grdEmptyBags [data-grid=eb]', function () {
            var i = int($(this).data('i')), k = $(this).data('k');
            dtEmptyBags[i][k] = (k === 'PackingType') ? int($(this).val()) : num($(this).val());
        });

        $(document).on('change', '#grdPaymentTerm [data-grid=pay]', function () {
            var i = int($(this).data('i')), k = $(this).data('k');
            var v = $(this).val();
            dtPaymentTerm[i][k] = (k === 'DueDate') ? v
                                : (k === 'PaymentTerm' || k === 'BaseDateType' || k === 'DueDays') ? int(v)
                                : num(v);
            paymentCellUpdated(i, k);
        });

        $('#btnAddPaymentRow').on('click', addPaymentRow);
        $(document).on('click', '#grdPaymentTerm button[data-act]', function () {
            var i = int($(this).data('i'));
            if ($(this).data('act') === 'pay-del') {
                dtPaymentTerm.splice(i, 1);
                renderPaymentGrid();
                refreshPaymentScheduleText();
            } else {
                addPaymentRow();
            }
        });

        // ---- toolbar ----
        $('#btnNew').on('click', function () { resetForm(false); });
        $('#btnRefresh').on('click', function () { loadLookups(); });
        $('#btnSave,#btnUpdate,#btnSaveAs').on('click', save);
        $('#btnDelete').on('click', remove);
        // BtnShortCutkeys_Click -> MakeShortCutKeys (:3933): the desktop's own list.
        $('#btnShortCutKeys').on('click', showShortCutKeys);
        $('#soShortcutClose').on('click', function () { $('#soShortcutDlg').removeClass('open'); });
        $('#btnAttachments,#btnPrint').on('click', function () {
            status('Attachments and printing are not migrated for this screen yet.', true);
        });
        // BtnAddShiptoAddress_Click (:4006): using SupfrmShipToAddress obj; obj.ShowDialog().
        // The dialog is the desktop form; its data goes through the purchase-order ship-to
        // endpoints (FormHistory / GetByID / BLL Save = Sp_SupplierCustomerShipToAddress_*).
        $('#btnAddShipToAddress').on('click', openShipToDialog);
        $('#stClose').on('click', closeShipToDialog);
        $('#stBtnNew').on('click', shipToReset);
        $('#stBtnRefresh').on('click', function () { shipToFillCombos(true); });   // cmbcountryfill + cmbsupplierfill
        $('#stBtnSave,#stBtnUpdate').on('click', function () { shipToInsert(this); });
        // cmbcountry_Leave -> City.GetAll(Org, Company) (not filtered by country)
        $('#stCountry').on('change blur', function () { if (!stCitiesLoaded) shipToLoadCities(); });
        // txttelephone / txtcell KeyPress -> OnlytextNumberFunction
        $('#stTelephone,#stCell').on('keypress', function (e) {
            if (e.which && e.which >= 32 && !/[0-9]/.test(String.fromCharCode(e.which))) e.preventDefault();
        });
        $('#grdShipTo').on('dblclick', 'tbody tr[data-id]', function () { shipToEdit(int($(this).attr('data-id'))); });
        $('#soShipToDlg').on('keydown', function (e) {
            var k = (e.key || '').toUpperCase();
            if (e.key === 'Escape' || (e.ctrlKey && k === 'E')) { e.preventDefault(); closeShipToDialog(); }
            else if (e.ctrlKey && k === 'N') { e.preventDefault(); shipToReset(); }
            else if (e.ctrlKey && k === 'S' && $('#stBtnSave').is(':visible')) { e.preventDefault(); $('#stBtnSave').trigger('click'); }
            else if (e.ctrlKey && k === 'U' && $('#stBtnUpdate').is(':visible')) { e.preventDefault(); $('#stBtnUpdate').trigger('click'); }
        });

        // ---- history ----
        $('#btnShowHistory').on('click', function () {
            var $b = $(this);
            if ($b.prop('disabled')) return;
            $b.prop('disabled', true);
            var r = loadHistory();
            if (r && r.always) r.always(function () { $b.prop('disabled', false); }); else $b.prop('disabled', false);
        });
        // BtnRefreshHistory_Click (:3042): re-read the history combos only (no grid reload).
        $('#btnRefreshHistory').on('click', function () {
            var $b = $(this);
            if ($b.prop('disabled')) return;
            $b.prop('disabled', true);
            $.getJSON(API + '/history-combos').done(function (rows) {
                var keep = historyParentIds();
                LK.historyCombos = rows || [];
                if (keep.length) bindHistoryChildCombos(keep); else historyComboBind(rows);
            }).fail(function (xhr) {
                $('#historyStatus').text('Refresh failed: ' + (xhr.responseText || xhr.statusText)).addClass('err');
            }).always(function () { $b.prop('disabled', false); });
        });
        // btnNewHistory_Click (:3028): clears Item, Commission Agent and Buyer filters only.
        $('#btnNewHistory').on('click', function () {
            $('#cmbItemNameHistory,#cmbCommissionAgentHistory,#cmbBuyerNameHistory').val('').trigger('change.select2');
        });

        $(document).on('click', '#grdHistory button[data-act]', function () {
            var id = int($(this).data('id'));
            var act = $(this).data('act');
            if (act === 'hist-saveas') loadOrder(id, true);
            else loadOrder(id, false);
        });
        $(document).on('click', '#grdHistory tbody tr[data-hi]', function (e) {
            if ($(e.target).is('button, input')) return;
            loadHistoryChildren(int(historyRows[int($(this).data('hi'))].Id));
        });
        $(document).on('change', '#histCheckAll', function () {
            $('#grdHistory .hist-check').prop('checked', $(this).is(':checked'));
        });

        // ---- keyboard: frmPurchaseOrderCmagt_KeyDown (:3795, KeyPreview=true) ----
        function isVisibleEnabled(sel) { var $b = $(sel); return $b.length && $b.is(':visible') && !$b.prop('disabled'); }
        function onHistoryTab() { return $('#mainpanel-history').hasClass('active'); }
        function select2Open() { return $('.select2-container--open').length > 0; }
        function focusGrid(wrapSel) { var $w = $(wrapSel); if ($w.length) $w.trigger('focus'); }
        var TAB_ORDER = ['detail', 'expenses', 'emptybags', 'payment'];
        var GRID_OF = { detail: '#grdDetail', expenses: '#grdInvExp', emptybags: '#grdEmptyBags', payment: '#grdPaymentTerm' };
        function gridWrap(tab) { return $('#panel-' + tab + ' .win-grid-wrap'); }

        /* Current grid row (GridEX CurrentRow): the row last clicked / focused in each grid. */
        var curRow = {};
        $(document).on('mousedown focusin', '#grdDetail tbody tr[data-i], #grdPaymentTerm tbody tr[data-i], #grdHistory tbody tr[data-hi]', function () {
            var gid = $(this).closest('table').attr('id');
            curRow[gid] = $(this).is('[data-hi]') ? int($(this).data('hi')) : int($(this).data('i'));
        });
        function focusedGridId() {
            var $t = $(document.activeElement).closest('.win-grid-wrap').find('table.win-grid').first();
            return $t.length ? $t.attr('id') : null;
        }

        /* grdDetail_KeyDown (:1600), grdPaymentTerm_KeyDown (:2190), grdHistory_KeyDown (:3670). */
        function gridKeyDown(e, gid) {
            var i = curRow[gid];
            var ctrl = e.ctrlKey && !e.altKey;
            var btnInRow = $(document.activeElement).is('button') && $(document.activeElement).closest('#' + gid).length;
            if (gid === 'grdDetail') {
                if (i === undefined || i >= dtDetail.length) return false;
                if (ctrl && e.key === ' ') { if (btnInRow) $(document.activeElement).trigger('click'); return true; }
                if (ctrl && e.key === 'Delete') { deleteDetailRow(i); curRow[gid] = undefined; return true; }
                if (ctrl && e.key === 'Enter') { editDetailRow(i); return true; }
            } else if (gid === 'grdPaymentTerm') {
                if (i === undefined || i >= dtPaymentTerm.length) return false;
                if (ctrl && e.key === ' ') { if (btnInRow) $(document.activeElement).trigger('click'); return true; }
                if (ctrl && e.key === 'Delete') {
                    dtPaymentTerm.splice(i, 1); curRow[gid] = undefined;
                    renderPaymentGrid(); refreshPaymentScheduleText(); return true;
                }
                if (ctrl && (e.key === 'd' || e.key === 'D')) { addPaymentRow(); return true; }
            } else if (gid === 'grdHistory') {
                if (i === undefined || !historyRows[i]) return false;
                if (ctrl && e.key === ' ') { if (btnInRow) $(document.activeElement).trigger('click'); return true; }
                if (ctrl && e.key === 'Enter') { loadOrder(int(historyRows[i].Id), false); return true; }
            }
            return false;
        }

        function nextFocusable(from) {
            var $all = $('#soForm').find('input:not([type=hidden]):not([disabled]):not([readonly]), select:not([disabled]), textarea:not([disabled]):not([readonly]), button:not([disabled])')
                .filter(':visible').add($('#soForm .select2-selection').filter(':visible'));
            var list = $all.toArray().sort(function (a, b) {
                return (a.compareDocumentPosition(b) & Node.DOCUMENT_POSITION_FOLLOWING) ? -1 : 1;
            });
            var idx = list.indexOf(from);
            if (idx < 0) { var s2 = $(from).closest('.select2-container').find('.select2-selection')[0]; idx = list.indexOf(s2); }
            return idx >= 0 ? list[idx + 1] : null;
        }

        $(document).on('keydown', function (e) {
            if ($('#soShipToDlg').hasClass('open')) return;
            if ($('#soShortcutDlg').hasClass('open')) {
                if (e.key === 'Escape') { $('#soShortcutDlg').removeClass('open'); e.preventDefault(); }
                return;
            }
            // leave a full-screen grid first
            if (e.key === 'Escape' && $('.so-fs-host.so-fs').length) { $('.so-fs-host.so-fs').removeClass('so-fs'); e.preventDefault(); return; }
            if (select2Open()) return;           // the open combo owns the keys

            var gid = focusedGridId() || ($(e.target).closest('table.win-grid').attr('id'));
            if (gid && gridKeyDown(e, gid)) { e.preventDefault(); return; }

            var t = e.target, tag = (t.tagName || '').toLowerCase();
            // Keys.Return -> SendKeys("{TAB}") - not inside a textarea, a button or a grid
            if (e.key === 'Enter' && !e.ctrlKey && !e.altKey && !e.shiftKey) {
                if (tag === 'textarea' || tag === 'button' || gid) return;
                if ($(t).closest('#soForm').length) {
                    var nx = nextFocusable(t);
                    if (nx) { e.preventDefault(); $(nx).trigger('focus'); }
                }
                return;
            }
            if (e.ctrlKey && e.altKey && (e.key === 'Control' || e.key === 'Alt')) { e.preventDefault(); showShortCutKeys(); return; }
            if (!e.ctrlKey) {
                if (e.key === 'Escape' && tag !== 'input' && tag !== 'select' && tag !== 'textarea') { e.preventDefault(); window.close(); }
                return;
            }
            var k = e.key.length === 1 ? e.key.toUpperCase() : e.key;
            if (k === 'T') {
                e.preventDefault();
                if (onHistoryTab()) { $('.win-tab[data-maintab=form]').trigger('click'); $('#datDocDate').trigger('focus'); }
                else { $('.win-tab[data-maintab=history]').trigger('click'); focusGrid('#hpanel-grid .win-grid-wrap'); }
                return;
            }
            if (k === 'E') { e.preventDefault(); window.close(); return; }
            if (!onHistoryTab()) {
                if (e.shiftKey && k === 'Delete') { e.preventDefault(); if (!$('#btnDelete').prop('disabled')) $('#btnDelete').trigger('click'); }
                else if (k === 'S') { e.preventDefault(); if (isVisibleEnabled('#btnSave')) $('#btnSave').trigger('click'); else if (isVisibleEnabled('#btnUpdate')) $('#btnUpdate').trigger('click'); }
                else if (k === 'F12') { e.preventDefault(); if (isVisibleEnabled('#btnSaveAs')) $('#btnSaveAs').trigger('click'); }
                else if (k === 'U') { e.preventDefault(); if (isVisibleEnabled('#btnUpdate')) $('#btnUpdate').trigger('click'); }
                else if (k === 'P') { e.preventDefault(); if (isVisibleEnabled('#btnPrint')) $('#btnPrint').trigger('click'); }
                else if (k === 'N') { e.preventDefault(); if (!$('#btnNew').prop('disabled')) $('#btnNew').trigger('click'); }
                else if (k === 'R') { e.preventDefault(); if (!$('#btnRefresh').prop('disabled')) $('#btnRefresh').trigger('click'); }
                else if (k === 'F5') { e.preventDefault(); $('#datDocDate').trigger('focus'); }
                else if (k === 'F10') { e.preventDefault(); $('#btnAttachments').trigger('click'); }
                else if (k === 'ArrowDown') { e.preventDefault(); $('.win-tab[data-tab=detail]').trigger('click'); focusGrid(gridWrap('detail')); }
                else if (k === 'ArrowRight') {
                    var cur = null;
                    TAB_ORDER.forEach(function (tb) { if (gid === GRID_OF[tb].slice(1)) cur = tb; });
                    if (cur) {
                        e.preventDefault();
                        var nxt = TAB_ORDER[(TAB_ORDER.indexOf(cur) + 1) % TAB_ORDER.length];
                        $('.win-tab[data-tab=' + nxt + ']').trigger('click'); focusGrid(gridWrap(nxt));
                    }
                }
                else if (k === 'ArrowUp') { e.preventDefault(); $('#cmbParentItem').select2('focus'); }
            } else {
                if (k === 'ArrowDown') { e.preventDefault(); focusGrid('#hpanel-grid .win-grid-wrap'); }
                else if (k === 'S') { e.preventDefault(); if (!$('#btnShowHistory').prop('disabled')) $('#btnShowHistory').trigger('click'); }
                else if (k === 'ArrowUp') { e.preventDefault(); $('#fromDateHistory').trigger('focus'); }
            }
        });

        // ---- ctrlGrdBar : full-screen toggle for each grid ----
        $(document).on('click', '.so-fs-btn', function () {
            var $h = $(this).closest('.so-fs-host');
            var on = !$h.hasClass('so-fs');
            $('.so-fs-host.so-fs').removeClass('so-fs');
            $h.toggleClass('so-fs', on);
            $(this).attr('title', on ? 'Exit full screen (Esc)' : 'Full screen');
        });
    });
})();

/* History From/To: the desktop DateTimePickers (FromDateHistory / ToDateHistory) are never
   assigned in code, so they open on the designer default - today - with their check box ON
   (ShowCheckBox, Checked defaults true), i.e. the history filters today..today. Validity
   From/To are Checked=false in the designer and stay blank. Local calendar date, not UTC. */
(function () {
    function seed() {
        var d = new Date();
        var t = d.getFullYear() + '-' + ('0' + (d.getMonth() + 1)).slice(-2) + '-' + ('0' + d.getDate()).slice(-2);
        ['fromDateHistory', 'toDateHistory'].forEach(function (id) {
            var e = document.getElementById(id);
            if (e && !e.value) e.value = t;
        });
    }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', seed); else seed();
})();
