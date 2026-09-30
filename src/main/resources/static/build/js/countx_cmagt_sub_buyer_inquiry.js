/* ===========================================================================
 * Sub Buyer Inquiry Booking - frmSubBuyerInquiryBooking.cs (DocumentTypeId 1050)
 *
 * Loaded AFTER countx_cmagt_buyer_inquiry_booking.js (window.BIB_SUB_MODE stops that file
 * from starting itself) and reuses its load / payload / save code, because the desktop sub
 * form calls the very same BLL (InquiryBookingMaster.ReadById / Save, 0492 -> DAL 0544) with
 * the very same object built by the very same Insert() - verified line by line:
 * Insert :1561-1711 is identical to frmBuyerInquiryBooking.Insert apart from the tail
 * (no ChkLink / openForm; Reset() then chkPrint -> 1050A slip).
 *
 * Desktop event handlers and where they live here:
 *   frmSubBuyerInquiryBooking_Load / InitializeComponentMethod (:364/:406) -> start-up below
 *   RadBusinessName_CheckedChanged (:668)     -> bibPartyNameModeChanged (header disabled)
 *   CmbDeliveryToParty_Leave / CmbShipToAddress_Leave (:674/:691) -> shared, run by bibLoad
 *   cmbParentItem_Leave / CmbItemName_Leave / CmbAnalysisGroup_Leave -> shared, run by bibLoad
 *   CmbPackUom_Leave / txtQty_TextChanged (CalculateWeight)          -> shared
 *   datDeliveryStartDate_ValueChanged / txtDeliveryDays_TextChanged  -> shared
 *   txtValidityDays_TextChanged / datValidityUpto_ValueChanged       -> shared
 *   grdSubParty_ColumnButtonClick / CellUpdated / KeyDown (:1219/:1280/:1309) -> shared grid
 *   btnnew_Click / Reset (:1354/:1366)        -> bibNew (portal defaults NOT applied: the sub
 *                                                form has no GetCommissionAgentConfigurations)
 *   BtnRefresh_Click (:1410)                  -> bibRefresh
 *   btnUpdate_Click (:1726)                   -> bibSave guard below ("Record not update because Id not found")
 *   btnPrint_Click (:1893)                    -> bibPrint (1050A slip)
 *   btnAttachments_Click (:1849)              -> bibAttachments (not ported, as on the main page)
 *   frmSubBuyerInquiryBooking_KeyDown (:2448) -> keydown handler below
 *   BtnShortCutkeys_Click / MakeShortCutKeys (:2572/:2540) -> subShortcutKeys
 *   History tab handlers (:1987-2433, :2584)  -> none: tabPage2 is removed at start-up (:432)
 *   CmbPaymentTerm_ValueChanged               -> does not exist on the sub form; disabled here
 * =========================================================================== */
'use strict';

/* no CmbPaymentTerm_ValueChanged on this form: the loaded Due Days are kept as saved */
bibPaymentTermChanged = function () { };

/* Reset() here never calls GetCommissionAgentConfigurationsFromGlobalandBind */
applyPortalDefaults = function () { return Promise.resolve(); };

/* grdSubParty is always visible; BindGrids() seeds one empty row (AddRowsInSubPartyeGrid) */
bibToggleSubParties = function () {
    $('subPartiesBlock').style.display = '';
    if (!subPartyRows.length) bibAddSubParty();
};

/* btnUpdate.Visible = true only after ReadById; there is no Save and no Delete */
applyButtonState = function () {
    var loaded = intOf('inquiryBookingMasterId') > 0;
    var u = $('btnUpdate');
    if (u) { u.style.display = loaded ? '' : 'none'; if (!inFlight.btnUpdate) u.disabled = !loaded; }
};

bibShowTab = function () { };

/* btnUpdate_Click (:1726): RecId == 0 -> "Record not update because Id not found".
 * The shared bibSave then runs Insert(): same validation, confirm, payload and save;
 * on success it Resets and prints when Preview is ticked - exactly the desktop tail. */
var subBaseSave = bibSave;
bibSave = function () {
    if (intOf('inquiryBookingMasterId') <= 0) { message('Record not update because Id not found', true); return Promise.resolve(); }
    return subBaseSave();
};

/* PanelHeaderInfo.Enabled = false (:4255), CmbCompanyName.Enabled = false (:4966) */
function subLockHeader() {
    Array.prototype.forEach.call(
        document.querySelectorAll('#PanelHeaderInfo input, #PanelHeaderInfo select, #PanelHeaderInfo textarea, #cmbCompanyName'),
        function (el) { el.disabled = true; });
}

function subShortcutKeys() {
    alert([
        'Ctrl+U  For Update', 'Ctrl+E / Esc  For Close', 'Ctrl+R  For Refresh', 'Ctrl+N  For New',
        'Ctrl+P  For Print', 'Ctrl+F5  For Focus on Inquiry Date', 'Ctrl+F10  For Open Attachments',
        'Ctrl+Alt  To Show ShortCut Keys Form', 'Ctrl+D  Add a sub party row',
        'Ctrl+Delete  Remove the current sub party row'
    ].join('\n'));
}

/* frmSubBuyerInquiryBooking_KeyDown (:2448) */
document.addEventListener('keydown', function (e) {
    if (!e.ctrlKey && e.key === 'Escape') { window.close(); return; }
    if (!e.ctrlKey) return;
    if (e.altKey && (e.key === 'Control' || e.key === 'Alt')) { subShortcutKeys(); return; }
    var k = (e.key || '').toLowerCase();
    if (k === 'u') { e.preventDefault(); var u = $('btnUpdate'); if (u && u.style.display !== 'none' && !u.disabled) bibSave(); }
    else if (k === 'p') { e.preventDefault(); bibPrint(); }
    else if (k === 'n') { e.preventDefault(); bibNew(); }
    else if (k === 'r') { e.preventDefault(); bibRefresh(); }
    else if (k === 'e') { e.preventDefault(); window.close(); }
    else if (e.key === 'F10') { e.preventDefault(); bibAttachments(); }
});

document.addEventListener('DOMContentLoaded', function () {
    subLockHeader();
    var id = parseInt(window.SUB_BIB_ID, 10) || 0;
    if (!id) {
        var q = new URLSearchParams(window.location.search);
        id = parseInt(q.get('id'), 10) || 0;
    }
    loadLookups()
        .then(function () { return bibNew(); })
        .then(function () { if (id > 0) return bibLoad(id); })
        .then(function () {
            if (!subPartyRows.length) bibAddSubParty();
            renderSubParties();
            subLockHeader();
            applyButtonState();
        });
});
