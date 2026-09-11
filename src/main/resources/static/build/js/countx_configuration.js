/*
 * Ditto of the desktop Configuration.cs screen's generic load/save infrastructure.
 *
 * Desktop equivalent (verified against Architecture.WinApp.Configurations.Configuration.cs):
 *   - Load:  ConfigrationsAllocation.HistoryConfiquration(new ConfigrationsAllocation{
 *              OrganizationId = ..., CompanyId = ... })  ->  FindControlByAccessibleName(...)
 *            populates every bound control from the returned list, matched by AccessibleName.
 *   - Save:  every bound control's own change/leave event (CheckedChanged / RadioButton
 *            CheckedChanged / ComboBox SelectedIndexChanged / TextBox Leave) calls one of the
 *            desktop's six generic Fire*() handlers, which all do the same thing:
 *              GetByKey(AccessibleName, OrgId, CompanyId)
 *                -> exists: keep Id + ConfigrationsDefinitionId, UPDATE
 *                -> not found: resolve ConfigrationsDefinitionId via
 *                   ActivityLog.ReadByConfigDescription(AccessibleName), Id = 0, INSERT
 *            ConfigValue is always saved as "" and IsActive always true by every Fire*()
 *            body - the actual persisted value lives in ConfigKey - see
 *            ConfigurationServiceImpl#saveControl (Java ditto of this).
 *
 * This file provides that same generic contract for every HTML control carrying a
 * data-config-description attribute (== the control's desktop AccessibleName, used
 * verbatim as the database key), regardless of which of the 18 tabs it lives on -
 * so every future tab only needs to add markup with the right data-config-description /
 * data-config-type, not new save logic.
 */

(function () {
    "use strict";

    var API_BASE = "/api/configurations";
    var configMap = {}; // AccessibleName -> { id, configrationsDefinitionId, configKey, ... }

    // ---------------------------------------------------------------
    // Toast (ditto MessageBox.Show(...) on save success/failure)
    // ---------------------------------------------------------------
    function showToast(message, isError) {
        var $t = $("#cfgSaveToast");
        $t.text(message)
          .removeClass("success error")
          .addClass(isError ? "error" : "success")
          .stop(true, true)
          .fadeIn(120);
        clearTimeout(showToast._h);
        showToast._h = setTimeout(function () { $t.fadeOut(300); }, isError ? 4000 : 1800);
    }

    // ---------------------------------------------------------------
    // Tab switching (win-nav-tabs / cfg-tab-pane, same pattern as
    // countx_purchase_order_full.js's switchTab, renamed to avoid collision)
    // ---------------------------------------------------------------
    window.switchConfigTab = function (tabId) {
        $("#configTabControl li").removeClass("active");
        $(".cfg-tab-pane").removeClass("active");
        $("#configTabControl a[onclick=\"switchConfigTab('" + tabId + "')\"]").parent().addClass("active");
        $("#" + tabId).addClass("active");
    };

    // Nested sub-tab strip (ditto a top-level tab whose own Controls.Add contains a second
    // WinForms TabControl - first needed by Purchase's tabControl2/tabPage1-4). groupId is the
    // <ul class="win-sub-tabs"> id, subTabId the target <div class="cfg-subtab-pane"> id; only
    // siblings within that same group's parent tab pane are toggled, so unrelated tabs' own
    // sub-tab groups (if any) are never touched.
    window.switchConfigSubTab = function (groupId, subTabId) {
        var $group = $("#" + groupId);
        $group.find("li").removeClass("active");
        $group.find("a[onclick=\"switchConfigSubTab('" + groupId + "', '" + subTabId + "')\"]").parent().addClass("active");
        $group.closest(".cfg-tab-pane").find(".cfg-subtab-pane").removeClass("active");
        $("#" + subTabId).addClass("active");
    };

    // ---------------------------------------------------------------
    // Generic LOAD: GET /api/configurations/map once, then populate every
    // bound control on every tab (only Account tab has controls today).
    // ---------------------------------------------------------------
    function loadAllConfigurations() {
        $.ajax({
            url: API_BASE + "/map",
            type: "GET",
            success: function (map) {
                configMap = map || {};
                populateAllControls();
            },
            error: function () {
                showToast("Failed to load configuration values.", true);
            }
        });
    }

    function populateAllControls() {
        $("[data-config-description]").each(function () {
            var $el = $(this);
            var accessibleName = $el.attr("data-config-description");
            var type = $el.attr("data-config-type");
            var entry = configMap[accessibleName];
            var configKey = entry ? entry.configKey : null;

            switch (type) {
                case "checkbox":
                    $el.prop("checked", configKey === "True");
                    break;
                case "radio":
                    // Each radio in a WinForms RadioButton group is its own independently
                    // persisted ConfigrationsAllocation row (verified: desktop re-fires
                    // CheckedChanged - and therefore a save - for every sibling radio when
                    // one is clicked, not just the newly-checked one). Checked = "True".
                    $el.prop("checked", configKey === "True");
                    break;
                case "textbox":
                    // Ditto Configuration.cs's BindForm(): the load loop only ever assigns
                    // txtbx.Text when a matching saved ConfigrationsAllocation row exists
                    // (item.ConfigKey) - when a control has no saved row yet, BindForm's loop
                    // simply never touches it, so its WinForms-designer-authored default Text
                    // (InitializeComponent's own txtXxx.Text = "...") is what actually shows on
                    // first run. Overwriting to "" here when configKey is null (as this used to
                    // do) was a fidelity gap - discovered on Production tab's Weight Tolerance
                    // group, whose textboxes all have non-blank designer defaults (see the
                    // matching value="..." attributes in configuration.html). Corrected once,
                    // retroactively correct for all 8 prior tabs too (none of them had a
                    // non-blank default, so this was previously unobservable there).
                    if (configKey != null) {
                        $el.val(configKey);
                    }
                    break;
                case "select":
                    if (configKey != null && configKey !== "") {
                        $el.val(configKey);
                    }
                    break;
                case "radio-numeric":
                    // HRM tab (ditto Configuration.cs's BindForm(), lines ~2048-2062, and
                    // FireRadioButton's own special-case branch at lines ~3175-3197) - a pattern
                    // not used anywhere else on this screen: RadValueZero/radValueTwo/radValuethree
                    // ("Off"/"2 Days"/"3 Days" for DaysForAdjacentHolidayAbsenceRule) all three
                    // share the SAME AccessibleName/ConfigDescription - there is exactly ONE saved
                    // row for the whole group, whose ConfigKey is a literal numeral string ("0",
                    // "2", or "3"), not "True"/"False". Verified: the save path builds a throwaway
                    // TextBox and calls FireTextBox on it (not a boolean radio save) - ditto here,
                    // each radio's own data-radio-value is compared against the shared configKey.
                    // Guarded on configKey != null (ditto the textbox-default-preservation fix
                    // above) so the HTML-authored default (RadValueZero/"Off", matching
                    // InitializeComponent's own RadValueZero.Checked = true) survives until a
                    // value is actually saved, rather than every radio in the group being
                    // unconditionally unchecked when no row exists yet.
                    if (configKey != null) {
                        $el.prop("checked", configKey === $el.attr("data-radio-value"));
                    }
                    break;
                case "date":
                    // Deliberately NOT populated from configKey here - ditto a verified desktop
                    // quirk: Configuration.cs's BindForm() load loop has an if/else-if chain for
                    // CheckBox/TextBox/ComboBox/UltraCombo/RadioButton only - DateTimePicker
                    // (txtAsOnDateForDoCompulasoryOnGDN, the only one on this whole screen) has
                    // NO branch there at all, so its saved ConfigKey is written by FireDateTime()
                    // on change but never read back on screen load. The control's WinForms
                    // default (DateTime.Now at construction) is what's always shown - preserved
                    // here by defaulting the <input type=date> to today's date every load,
                    // never the saved value. See applyDateTimePickerDefaults() below.
                    break;
            }
        });

        applyRadioGroupDefaults();
        applyDateTimePickerDefaults();

        // Special-case UI coupling (verified: Configuration.cs ControlEventHandler,
        // "AutoRemarksForPaymentThroughBank" branch) - GroupBoxAutoRemarks.Enabled is
        // driven by chkAutoRemarksForPaymentThroughBank's own checked state, both on
        // initial load and on every subsequent change.
        applyAutoRemarksCoupling();

        // Special-case UI coupling (verified: Configuration.cs ControlEventHandler,
        // "IncludeBankAccountsInFreightVoucherCreditAccount" branch, line ~3149, calling
        // CmbDefaultFreightVoucherCreditAccountFillFromGlobal()) - the Default Freight
        // Voucher Credit Account list is re-filtered by this checkbox's own checked state,
        // both on initial load (using its just-loaded saved value) and on every subsequent
        // toggle.
        applyFreightVoucherCoupling(true);

        // Commission Agent tab (verified Configuration.cs constructor calling
        // CmbCustomGroupForWHTAccounts_Leave(null,null) TWICE on load and
        // CmbCustomGroupForWHTAccountsSale_Leave() never at all - see
        // IConfigurationService#getAccountsByCustomGroup's own javadoc for the full desktop-bug
        // writeup) - only the Purchase-side WHT account combo is refreshed on initial load.
        applySupplierCustomerNameDisplayMode();
        applyWhtAccountsCoupling("CmbCustomGroupForWHTAccounts", "CmbDefaultWhtAccountPurchaseIdForCommissionAgentPortal", true);
        applyWhtAccountsCoupling("CmbCustomGroupForWHTAccountsSale", "CmbDefaultWhtAccountSaleIdForCommissionAgentPortal", false);
    }

    // Ditto Configuration.cs's BindForm() load-time normalization: every one of these three
    // radio "groups" is really 2-3 independently-persisted rows, and BindForm re-derives a
    // single winner with a fixed tie-break priority every time it runs (verified source,
    // BindForm's RadioButton branch, lines ~2076-2110) rather than trusting all of a group's
    // saved rows to already be mutually exclusive:
    //   - Negative Balance:  Chk_IsMinusBlncAllow > RadDisplayWarningforNegativeBalance
    //                        > RadDisableBothNegativeBalanceRestrictions (default winner)
    //   - Ledger Default:    rdDetailLedger > rdDefaulLedgerSummaryOne
    //                        > rdDefaulLedgerSummaryTow (default winner)
    //   - Auto Remarks cheque style: rdAutoRemarksIncludeChequeDateAndNumber
    //                        > rdAutoRemarksIncludeChequeNumber (default winner)
    // On the real desktop this block only runs at all when the org/company's config list
    // contains at least one RadioButton-type row somewhere across the full 18-tab form; this
    // port applies it whenever the Account tab's own config map load completes, since a
    // database with any real usage history will always satisfy that condition in practice -
    // documented here rather than silently assumed.
    function applyRadioGroupDefaults() {
        function pickWinner(idsInPriorityOrder) {
            var winnerId = null;
            for (var i = 0; i < idsInPriorityOrder.length; i++) {
                if ($("#" + idsInPriorityOrder[i]).is(":checked")) {
                    winnerId = idsInPriorityOrder[i];
                    break;
                }
            }
            if (winnerId === null) {
                winnerId = idsInPriorityOrder[idsInPriorityOrder.length - 1]; // default winner
            }
            idsInPriorityOrder.forEach(function (id) {
                $("#" + id).prop("checked", id === winnerId);
            });
        }

        pickWinner(["Chk_IsMinusBlncAllow", "RadDisplayWarningforNegativeBalance", "RadDisableBothNegativeBalanceRestrictions"]);
        pickWinner(["rdDetailLedger", "rdDefaulLedgerSummaryOne", "rdDefaulLedgerSummaryTow"]);
        pickWinner(["rdAutoRemarksIncludeChequeDateAndNumber", "rdAutoRemarksIncludeChequeNumber"]);
        // QA tab (ditto Configuration.cs's BindForm(), lines ~2062-2075: rdLabCompulsoryBeforFirstWeight's
        // own saved value always wins, rdLabCompulsoryAfterFirstWeight is always forced to its exact
        // negation - never read from its own saved ConfigKey on load). Also a live
        // ControlEventHandler coupling (lines ~3201-3213) on change - covered for free by plain
        // HTML radio grouping (same name="labCompulsoryTiming") plus the existing generic radio
        // dual-fire save handler below, no new save-path code needed.
        pickWinner(["rdLabCompulsoryBeforFirstWeight", "rdLabCompulsoryAfterFirstWeight"]);

        // Production tab (ditto Configuration.cs's BindForm(), lines ~2114-2120 - NOT a simple
        // negation like the Lab Compulsory pair above. Verified: rdPackingMaterialCompulsoryOn-
        // StockConversionForWarning's AccessibleName is in BindForm's own generic-assignment
        // exclude list (line ~2064), so it NEVER reads its own saved ConfigKey and its .Checked
        // simply stays at the WinForms designer default of false (InitializeComponent never
        // sets it true - verified). ...ForStop's AccessibleName is NOT excluded, so its own saved
        // ConfigKey IS read correctly by the generic loop first - but then this same
        // normalization block unconditionally runs "if (Warning.Checked) Stop=false; else
        // Stop=true" every single time, and since Warning.Checked is always false (per above),
        // the else branch always fires and stomps Stop back to true regardless of what was just
        // correctly loaded for it. Net effect, verified: Stop always wins on every screen load no
        // matter what either radio's saved value actually is. This is force-applied here
        // unconditionally (ignoring both radios' just-populated values entirely), not via
        // pickWinner, because pickWinner would incorrectly let a saved Warning=True win - the
        // real desktop never lets that happen on load. The live ControlEventHandler coupling on
        // change (verified lines ~3252-3262) IS a normal, correct mutual-exclusion - covered for
        // free by plain HTML radio grouping (name="packingMaterialCompulsoryOnStockConversion")
        // plus the existing generic radio dual-fire save handler, no new save-path code needed.
        $("#rdPackingMaterialCompulsoryOnStockConversionForStop").prop("checked", true);
        $("#rdPackingMaterialCompulsoryOnStockConversionForWarning").prop("checked", false);

        // Export tab (ditto Configuration.cs's BindForm(), lines ~2186-2192) - a THIRD, distinct
        // radio pattern, different from both above. Verified: neither
        // rdIsInspectionLotMappedOnDeliveryOrderByInvoice nor ...ByItem is in BindForm's
        // generic-assignment exclude list, so both correctly read their own saved ConfigKey via
        // the plain "case radio" handling above - no pickWinner/force needed for the normal case.
        // The desktop's own extra normalization ("if (ByInvoice.Checked) ByItem=false; else
        // ByInvoice=false;") is a harmless anomaly-only tie-break: it only changes anything when
        // BOTH were somehow saved True (an inconsistent-data edge case), in which case ByInvoice
        // wins. Reproduced narrowly here rather than with pickWinner (which would incorrectly
        // supply an always-checked default winner even when neither radio was ever saved, unlike
        // the real desktop which leaves both unchecked in that case).
        if ($("#rdIsInspectionLotMappedOnDeliveryOrderByInvoice").is(":checked") &&
            $("#rdIsInspectionLotMappedOnDeliveryOrderByItem").is(":checked")) {
            $("#rdIsInspectionLotMappedOnDeliveryOrderByItem").prop("checked", false);
        }

        // Party Processing tab, groupBox11 "Mange Stock By" (ditto Configuration.cs's BindForm(),
        // lines ~2122-2153) - a 5-way priority chain, structurally like the Account tab's existing
        // 3-way/2-way pickWinner() groups above, EXCEPT its first member,
        // rdCheckStockPartyItemWareHouse, is itself in BindForm's generic-assignment exclude list
        // (AccessibleName "CheckStockPartyItemWareHouse", verified line ~2064) - so unlike every
        // pickWinner() group above (where every member reads its own real saved value), this one
        // can NEVER win on load no matter what its own ConfigKey says. Forced false here first,
        // ditto Production's Warning/Stop pair, so pickWinner()'s own "is it checked" test can
        // never see it as a candidate - the remaining 4 members (CropYear, CropYearJobLot,
        // CropYearJobLotPackUom, CropYearJobLotPackingTypeUom) all correctly read their own real
        // saved values via the generic radio case above, exactly matching the desktop's own
        // un-excluded assignment for them, with CropYearJobLotPackingTypeUom as the verified
        // final-else default winner when none of the other 4 (including the always-false first
        // member) are checked.
        $("#rdCheckStockPartyItemWareHouse").prop("checked", false);
        pickWinner([
            "rdCheckStockPartyItemWareHouse",
            "rdCheckStockPartyItemWareHouseCropYear",
            "rdCheckStockPartyItemWareHouseCropYearJobLot",
            "rdCheckStockPartyItemWareHouseCropYearJobLotPackUom",
            "rdCheckStockPartyItemWareHouseCropYearJobLotPackingTypeUom"
        ]);

        // Commission Agent tab, groupBox20 "GRN Loading Challan Doc Date And Bilty Date" (ditto
        // Configuration.cs's BindForm(), lines ~2194-2207) - a normal 3-way priority chain,
        // structurally identical to the Account tab's pickWinner() groups above: verified none of
        // the 3 AccessibleNames are in BindForm's generic-assignment exclude list, so all 3
        // correctly read their own saved ConfigKey, with radGrnLoadingAllowDifferentBiltyDate >
        // radGrnLoadingDocDateAndBiltyDateSame > radGrnLoadingDocDateAndBiltyDateNoValidation
        // (implicit default winner - the desktop's own final "else" branch never force-sets it,
        // it just never overwrites whatever was already loaded for it).
        pickWinner([
            "radGrnLoadingAllowDifferentBiltyDate",
            "radGrnLoadingDocDateAndBiltyDateSame",
            "radGrnLoadingDocDateAndBiltyDateNoValidation"
        ]);

        // Commission Agent tab, GBForLateVehicleArrivalAfterPoExpiry "Late Vehicle Arrival After
        // Po Expiry Date" (ditto Configuration.cs's BindForm(), lines ~2209-2219) - a 6th, distinct
        // radio pattern: verified RadShowWarningForLateVehicleArrivalAfterPoExpiryCommissionAgentPortal
        // and RadBlockEntryForLateVehicleArrivalAfterPoExpiryCommissionAgentPortal are BOTH
        // independently-persisted and correctly read their own saved ConfigKey via the plain "case
        // radio" handling above (neither is in BindForm's exclude list, just checked via an
        // explicit "if (radioButton.AccessibleName == '...')" branch instead of the generic
        // unconditional assignment - functionally identical outcome). RadNoValidationForLate-
        // VehicleArrivalAfterPoExpiryCommissionAgentPortal ALSO has its own saved row (also not
        // excluded) yet BindForm unconditionally forces it to Checked=true whenever NEITHER
        // Warning nor Block ends up checked - overriding its own just-loaded value, unlike
        // groupBox20's fallback member above which is never forced. Not a pickWinner() (there is
        // no priority order between Warning/Block themselves - the desktop reads both
        // independently with no tie-break; only the "neither is true" fallback is special).
        if (!$("#RadBlockEntryForLateVehicleArrivalAfterPoExpiryCommissionAgentPortal").is(":checked") &&
            !$("#RadShowWarningForLateVehicleArrivalAfterPoExpiryCommissionAgentPortal").is(":checked")) {
            $("#RadNoValidationForLateVehicleArrivalAfterPoExpiryCommissionAgentPortal").prop("checked", true);
        }

        // System tab, groupBox9 "Mange Stock By" (ditto Configuration.cs's BindForm(), lines
        // ~2153-2182) - structurally identical to Party Processing's groupBox11 block above: a
        // 5-way priority chain whose first member, rdItemWarehouse, is itself in BindForm's
        // generic-assignment exclude list (AccessibleName "CheckStockItemWareHouse", verified line
        // ~2064, right alongside "CheckStockPartyItemWareHouse") - so it can never win on load no
        // matter what its own ConfigKey says. Forced false here first, ditto groupBox11 above, so
        // pickWinner()'s own "is it checked" test can never see it as a candidate - the remaining 4
        // members (CropYear, CropYearJobLot, CropYearJobLotPackUom,
        // CropYearJobLotPackingTypeUom) all correctly read their own real saved values via the
        // generic radio case above, exactly matching the desktop's own un-excluded assignment for
        // them, with CropYearJobLotPackingTypeUom as the verified final-else default winner when
        // none of the other 4 (including the always-false first member) are checked. The only
        // ControlEventHandler coupling on the whole System tab (line ~3347) is this group's own
        // dual-fire save, already handled by the existing generic radio-save case - no additional
        // save-path JS needed.
        $("#rdItemWarehouse").prop("checked", false);
        pickWinner([
            "rdItemWarehouse",
            "rdItemWarehouseCropYear",
            "rdItemWarehouseCropYearJobLot",
            "rdItemWarhouseCropJobPackUOM",
            "rdCheckStockItemWareHouseCropYearJobLotPackingTypeUom"
        ]);
    }

    // Commission Agent tab, GBComm's RadNickName/RadBusinessName pair (ditto Configuration.cs's
    // RadBusinessName_CheckedChanged(), called from ControlEventHandler's own dedicated
    // "radioButton.Name == 'RadBusinessName' || radioButton.Name == 'RadNickName'" branch INSTEAD
    // of FireRadioButton() - verified NEITHER radio has an AccessibleName set anywhere in
    // InitializeComponent, so this pair is never persisted at all, unlike every other radio group
    // on this screen). Purely swaps the displayed option text of the 3 supplier/customer combos
    // between each option's precomputed data-business-text/data-nick-text (see
    // IConfigurationService#getSupplierCustomers()'s own javadoc for how those two strings,
    // including the sub-supplier "{parent} / {self}" hierarchy prefix, are computed server-side) -
    // no AJAX call, ditto the desktop's own in-memory-only re-bind.
    function applySupplierCustomerNameDisplayMode() {
        var useBusinessName = $("#RadBusinessName").is(":checked");
        $(".cfg-supplier-customer-select").each(function () {
            var $select = $(this);
            var currentVal = $select.val();
            $select.find("option[data-business-text]").each(function () {
                var $opt = $(this);
                $opt.text(useBusinessName ? $opt.attr("data-business-text") : $opt.attr("data-nick-text"));
            });
            if (currentVal) {
                $select.val(currentVal);
            }
        });
    }

    // Ditto WinForms DateTimePicker's own default Value (DateTime.Now at construction, never
    // overwritten by BindForm() for this control type - see the "date" case in
    // populateAllControls() above). Runs on every load, unconditionally.
    function applyDateTimePickerDefaults() {
        var today = new Date();
        var iso = today.getFullYear() + "-" +
            String(today.getMonth() + 1).padStart(2, "0") + "-" +
            String(today.getDate()).padStart(2, "0");
        $("input[data-config-type='date']").val(iso);
    }

    // Formats an <input type=date> value ("YYYY-MM-DD") to match .NET's
    // Conversion.ToDateTime(DateTimePicker.Value).ToString() - the exact expression
    // FireDateTime() uses to build ConfigKey. With no time-of-day component supplied by an
    // HTML date input, the DateTimePicker's own time-of-day (00:00:00, since the control's
    // Value is never time-adjusted anywhere in Configuration.cs) is assumed, giving .NET's
    // default (en-US, invariant-equivalent) short-date + midnight format: "M/d/yyyy 12:00:00 AM".
    // ConfigKey is a free-text nvarchar column (not schema/type-checked), so this is a
    // presentation-format choice, not a database change.
    function formatDateForConfigKey(isoDateStr) {
        var parts = isoDateStr.split("-");
        var year = parseInt(parts[0], 10);
        var month = parseInt(parts[1], 10);
        var day = parseInt(parts[2], 10);
        return month + "/" + day + "/" + year + " 12:00:00 AM";
    }

    // ---------------------------------------------------------------
    // Generic SAVE: POST /api/configurations/save-control immediately on
    // change/leave, exactly like the desktop's per-control Fire*() handlers -
    // never a full-form submit.
    // ---------------------------------------------------------------
    function saveControl($el, accessibleName, configKeyValue) {
        var controlType = $el.attr("data-config-type");

        $.ajax({
            url: API_BASE + "/save-control",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({ configDescription: accessibleName, configKey: configKeyValue }),
            success: function (res) {
                if (res && res.success) {
                    // Keep the in-memory map in sync so re-reading it later (e.g. a sibling
                    // radio's dual-fire, or re-populating after a tab switch) reflects the
                    // just-saved value without a round trip.
                    configMap[accessibleName] = configMap[accessibleName] || {};
                    configMap[accessibleName].configKey = configKeyValue;
                    showToast(res.message || "Configuration saved successfully!", false);
                } else {
                    // Ditto Fire(CheckBox)'s "Configuration Not Found" path, which explicitly
                    // resets chkbx.Checked = false. FireRadioButton/FireDDLInfaragastic/
                    // FireTextBox do NOT reset their control on this same failure - checkbox
                    // is the only type that reverts.
                    if (controlType === "checkbox") {
                        $el.prop("checked", false);
                    }
                    showToast((res && res.message) || "Configuration Not Found", true);
                }
            },
            error: function (xhr) {
                // Ditto every Fire*() catch(Exception) block: Fire(CheckBox) resets
                // chkbx.Checked = false on any exception too; the other four handlers just
                // show the message and leave the control's on-screen state as the user left it.
                if (controlType === "checkbox") {
                    $el.prop("checked", false);
                }
                var msg = "Error saving configuration";
                try {
                    var res = JSON.parse(xhr.responseText);
                    if (res && res.message) msg = res.message;
                } catch (e) { /* ignore */ }
                showToast(msg, true);
            }
        });
    }

    // Formats a control's current value the same way the desktop's Fire*() handlers do:
    // bool.ToString() ("True"/"False") for checkbox/radio, trimmed text for textboxes,
    // the raw selected value for dropdowns.
    function boolToConfigKey(checked) {
        return checked ? "True" : "False";
    }

    // ---------------------------------------------------------------
    // Generic wiring: bind every control once, dispatch by data-config-type -
    // ditto the desktop's single ControlEventHandler dispatching by control type.
    // ---------------------------------------------------------------
    function wireGenericControls() {
        // Checkbox: ditto Fire() on CheckedChanged.
        $(document).on("change", "input[type=checkbox][data-config-description]", function () {
            var $el = $(this);
            var accessibleName = $el.attr("data-config-description");
            saveControl($el, accessibleName, boolToConfigKey($el.is(":checked")));

            if (accessibleName === "AutoRemarksForPaymentThroughBank") {
                applyAutoRemarksCoupling();
            }

            if (accessibleName === "IncludeBankAccountsInFreightVoucherCreditAccount") {
                applyFreightVoucherCoupling(false);
            }
        });

        // Radio: ditto FireRadioButton() on CheckedChanged. WinForms fires CheckedChanged
        // for BOTH the newly-checked radio AND the previously-checked sibling in the same
        // group, so both get saved (one "True", the other "False") - replicate that by
        // saving every radio sharing the same `name` attribute whenever one changes.
        // Restricted to data-config-type="radio" (plain boolean-per-radio groups) - the HRM
        // tab's DaysForAdjacentHolidayAbsenceRule group uses data-config-type="radio-numeric"
        // instead and is wired separately below, since it saves one shared numeric-string
        // ConfigKey via a single FireTextBox-equivalent call, not a per-radio boolean dual-fire.
        $(document).on("change", "input[type=radio][data-config-type='radio'][data-config-description]", function () {
            var groupName = $(this).attr("name");
            $("input[type=radio][name='" + groupName + "']").each(function () {
                var $r = $(this);
                var accessibleName = $r.attr("data-config-description");
                if (accessibleName) {
                    saveControl($r, accessibleName, boolToConfigKey($r.is(":checked")));
                }
            });
        });

        // Radio (numeric group): ditto ControlEventHandler's "DaysForAdjacentHolidayAbsenceRule"
        // branch (lines ~3175-3197) - builds a throwaway TextBox carrying whichever radio in the
        // group is now checked and calls FireTextBox on it, i.e. a single save of the group's one
        // shared ConfigDescription with the checked radio's own numeral value, not a per-radio
        // boolean dual-fire like the plain "radio" type above.
        $(document).on("change", "input[type=radio][data-config-type='radio-numeric']", function () {
            var $checked = $(this);
            if (!$checked.is(":checked")) {
                return;
            }
            var accessibleName = $checked.attr("data-config-description");
            var value = $checked.attr("data-radio-value");
            saveControl($checked, accessibleName, value);
        });

        // Textbox: ditto FireTextBox() on Leave (blur), only when the value actually changed.
        // FireTextBox itself also returns immediately when TextBox.Text is empty - ditto that
        // guard here rather than saving/clearing a previously-set value with a blank one.
        $(document).on("focus", "input[type=text][data-config-description]", function () {
            $(this).data("cfg-prev-value", $(this).val());
        });
        $(document).on("blur", "input[type=text][data-config-description]", function () {
            var $el = $(this);
            var newVal = $.trim($el.val());
            var prevVal = $el.data("cfg-prev-value");
            if (newVal === prevVal) {
                return; // ditto "don't save until the value actually changed"
            }
            $el.val(newVal);
            if (newVal === "") {
                return; // ditto FireTextBox: "if (!(TextBox.Text != string.Empty) ...) return;"
            }
            saveControl($el, $el.attr("data-config-description"), newVal);
        });

        // Dropdown/UltraCombo: ditto FireDDLInfaragastic() / FireDDL() on SelectedIndexChanged.
        // FireDDLInfaragastic returns immediately when cmbBox.ActiveRow == null (nothing
        // selected) - ditto that guard by skipping the save entirely on an empty selection.
        $(document).on("change", "select[data-config-description]", function () {
            var $el = $(this);
            var val = $el.val();
            if (!val) {
                return;
            }
            saveControl($el, $el.attr("data-config-description"), val);
        });

        // DateTimePicker: ditto FireDateTime() on ValueChanged. FireDateTime returns
        // immediately when Conversion.CheckDateTimeNull(Value) is true - ditto that guard by
        // skipping the save entirely when the input has been cleared.
        $(document).on("change", "input[type=date][data-config-description]", function () {
            var $el = $(this);
            var val = $el.val();
            if (!val) {
                return;
            }
            saveControl($el, $el.attr("data-config-description"), formatDateForConfigKey(val));
        });
    }

    // GroupBoxAutoRemarks.Enabled = chkAutoRemarksForPaymentThroughBank.Checked
    // (verified: Configuration.cs ControlEventHandler, "AutoRemarksForPaymentThroughBank" branch)
    function applyAutoRemarksCoupling() {
        var enabled = $("#chkAutoRemarksForPaymentThroughBank").is(":checked");
        var $fieldset = $("#fieldsetAutoRemarks");
        $fieldset.find(".cfg-autoremarks-child").prop("disabled", !enabled);
        $fieldset.toggleClass("cfg-disabled-group", !enabled);
    }

    // Ditto Configuration.cs's CmbDefaultFreightVoucherCreditAccountFillFromGlobal(), called
    // from ControlEventHandler's "IncludeBankAccountsInFreightVoucherCreditAccount" branch:
    // rebuilds CmbDefaultFreightVoucherCreditAccountId's option list between AccountTypeId
    // {2} ("Cash Equivalent" only, unchecked) and {2,8,15} ("Cash Equivalent" + "Other
    // Liabilities" + "Bank Equivalent", checked), retaining the current/saved selection if
    // it's still present in the new list - ditto the desktop's own BindAndRetainSelection.
    //
    // The unchecked-state list (type {2}) is already server-rendered (Thymeleaf
    // freightCreditAccountsDefault, see ConfigurationViewController), so the unchecked branch
    // just re-applies the saved selection rather than re-fetching; only the checked branch
    // needs a live call to GET /api/configurations/global-accounts, since the wider {2,8,15}
    // list is never rendered server-side.
    function applyFreightVoucherCoupling(isInitialLoad) {
        var $checkbox = $("#chkIncludeBankAccountsInFreightVoucherCreditAccount");
        var $combo = $("#CmbDefaultFreightVoucherCreditAccountId");
        if ($checkbox.length === 0 || $combo.length === 0) {
            return;
        }

        var checked = $checkbox.is(":checked");
        var savedEntry = configMap["DefaultFreightVoucherCreditAccountId"];
        var targetVal = isInitialLoad && savedEntry ? savedEntry.configKey : $combo.val();

        if (!checked) {
            if (targetVal) {
                $combo.val(targetVal);
            }
            return;
        }

        $.ajax({
            url: API_BASE + "/global-accounts",
            type: "GET",
            data: { with: "2,8,15" },
            success: function (rows) {
                var $placeholder = $combo.find("option[value='']").first();
                $combo.empty();
                $combo.append($placeholder.length
                    ? $placeholder
                    : $("<option></option>").attr("value", "").text("-- Select --"));
                (rows || []).forEach(function (row) {
                    $combo.append($("<option></option>")
                        .attr("value", row.Id)
                        .text(row.AccountCode + " - " + row.AccountTitle));
                });
                if (targetVal) {
                    $combo.val(targetVal); // ditto BindAndRetainSelection
                }
            },
            error: function () {
                showToast("Failed to refresh Default Freight Voucher Credit Account list.", true);
            }
        });
    }

    // Commission Agent tab (ditto Configuration.cs's CmbCustomGroupForWHTAccounts_Leave()/
    // CmbCustomGroupForWHTAccountsSale_Leave()): re-filters GET /api/configurations/
    // accounts-by-custom-group?customGroupId=<the custom-group select's own current value> into
    // the paired WHT account combo, retaining the current selection if still present - ditto
    // BindAndRetainSelection. When the custom-group select has no value (0/blank), the account
    // combo is simply left/cleared to just its placeholder (ditto the desktop's own "else:
    // DataSource=null, Text=''" branch) - no AJAX call made.
    function refreshWhtAccountCombo($customGroupSelect, $accountSelect) {
        var customGroupId = parseInt($customGroupSelect.val(), 10);
        if (!customGroupId || customGroupId <= 0) {
            $accountSelect.find("option:not(:first-child)").remove();
            return;
        }
        var currentVal = $accountSelect.val();
        $.ajax({
            url: API_BASE + "/accounts-by-custom-group",
            type: "GET",
            data: { customGroupId: customGroupId },
            success: function (rows) {
                var $placeholder = $accountSelect.find("option[value='']").first();
                $accountSelect.empty();
                $accountSelect.append($placeholder.length
                    ? $placeholder
                    : $("<option></option>").attr("value", "").text("-- Select --"));
                (rows || []).forEach(function (row) {
                    $accountSelect.append($("<option></option>")
                        .attr("value", row.Id)
                        .text(row.AccountCode + " - " + row.AccountTitle));
                });
                if (currentVal) {
                    $accountSelect.val(currentVal);
                }
            },
            error: function () {
                showToast("Failed to refresh WHT Account list.", true);
            }
        });
    }

    function applyWhtAccountsCoupling(customGroupSelectId, accountSelectId, refreshOnLoad) {
        var $customGroupSelect = $("#" + customGroupSelectId);
        var $accountSelect = $("#" + accountSelectId);
        if ($customGroupSelect.length === 0 || $accountSelect.length === 0) {
            return;
        }
        $customGroupSelect.off("change.whtAccounts").on("change.whtAccounts", function () {
            refreshWhtAccountCombo($customGroupSelect, $accountSelect);
        });
        if (refreshOnLoad) {
            refreshWhtAccountCombo($customGroupSelect, $accountSelect);
        }
    }

    $(function () {
        wireGenericControls();
        loadAllConfigurations();

        // Commission Agent tab's non-persisted display-mode toggle (see
        // applySupplierCustomerNameDisplayMode()'s own comment above) - wired once here rather
        // than through wireGenericControls()'s generic data-config-description dispatch, since
        // this pair carries neither attribute (it is never saved).
        $(document).on("change", "input[name='commissionAgentNameDisplayMode']", applySupplierCustomerNameDisplayMode);
    });
})();
