/* ============================================================================
 * Approvals (Dashboard)
 * Ported from Architecture.WinApp.ApprovalDashboard\ApprovalDashboard.cs (1,812 lines).
 *
 *   :105-130   Load                      feature 11, branches, From = today - 7, To = today
 *   :132-157   BranchesBind              defaults to the signed-in user's own branch
 *   :159-242   DynamicallyGenerateCards  two procedures, four panels, split by ActionId
 *   :244-1330  UserControl_Click         84 cases plus a range branch
 *   :1331-1344 Refresh_Click
 *   :1370-1389 BtnShow_Click
 * ==========================================================================*/
(function () {
'use strict';

var API = '/api/dashboard/approvals';

var S = {
    accounts: [], inventory: [], export_: [], commission: [],
    branchFeature: false, busy: false
};

/* ---------------------------------------------------------------- helpers */
function el(id)  { return document.getElementById(id); }
function val(id) { var e = el(id); return e ? e.value : ''; }
function setVal(id, v) { var e = el(id); if (e) e.value = v === null || v === undefined ? '' : v; }
function txt(v)  { return v === null || v === undefined ? '' : String(v); }
function esc(v)  { return txt(v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
                                .replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
function num(v)  { var n = parseFloat(v); return isNaN(n) ? 0 : n; }

function f(row, name) {
    if (!row) return null;
    if (row[name] !== undefined) return row[name];
    var lower = String(name).toLowerCase();
    for (var k in row) { if (k.toLowerCase() === lower) return row[k]; }
    return null;
}

function msg(text, ok) {
    var m = el('adMessage');
    if (!m) return;
    m.className = ok ? 'ok' : 'err';
    m.textContent = text;
    if (ok) setTimeout(function () { if (m.textContent === text) m.className = ''; }, 6000);
}
function clearMsg() { var m = el('adMessage'); if (m) { m.className = ''; m.textContent = ''; } }

function busy(btn, on) {
    S.busy = !!on;
    if (btn) { btn.disabled = !!on; btn.classList.toggle('btn-busy', !!on); }
    ['btnShow', 'btnNew'].forEach(function (id) {
        var b = el(id); if (b && b !== btn) b.disabled = !!on;
    });
}

function getJson(url) {
    return fetch(url, { headers: { 'Accept': 'application/json' } })
        .then(function (r) {
            if (!r.ok) throw new Error('HTTP ' + r.status + ' on ' + url);
            return r.json();
        });
}

window.adFromToggled = function () {
    var e = el('fromDate'); if (e) e.disabled = !(el('chkFrom') && el('chkFrom').checked);
};
window.adToToggled = function () {
    var e = el('toDate'); if (e) e.disabled = !(el('chkTo') && el('chkTo').checked);
};

/* ---------------------------------------------------------------- setup */
function loadSetup() {
    return getJson(API + '/setup').then(function (d) {
        if (d && d.branchError) msg('Branches: ' + d.branchError, false);
        S.branchFeature = !!(d && d.branchFeature);

        setVal('fromDate', (d && d.fromDate) || '');
        setVal('toDate',   (d && d.toDate) || '');

        var sel = el('branch');
        if (sel) {
            sel.innerHTML = '<option value="0">Branch Name</option>'
                + ((d && d.branches) || []).map(function (b) {
                      return '<option value="' + esc(f(b, 'BranchId')) + '">'
                           + esc(f(b, 'BranchName')) + '</option>';
                  }).join('');
            /* :153 - the desktop selects the signed-in user's own branch. */
            if (d && d.currentBranchId) sel.value = String(d.currentBranchId);
            /* :112-119 - the combo is enabled only when feature 11 is on. */
            sel.disabled = !S.branchFeature;
        }
    }).catch(function (e) {
        msg('Setup could not be read: ' + e.message, false);
    });
}

/* ---------------------------------------------------------------- cards */
window.adShow = function (btn) {
    if (S.busy) return;
    clearMsg();
    busy(btn, true);
    var empty = el('adEmpty');
    if (empty) { empty.style.display = ''; empty.textContent = 'Loading...'; }

    var q = ['branchId=' + (num(val('branch')) || 0)];
    if (el('chkFrom') && el('chkFrom').checked && val('fromDate')) {
        q.push('fromDate=' + encodeURIComponent(val('fromDate')));
    }
    if (el('chkTo') && el('chkTo').checked && val('toDate')) {
        q.push('toDate=' + encodeURIComponent(val('toDate')));
    }

    getJson(API + '/cards?' + q.join('&')).then(function (d) {
        busy(btn, false);
        if (d && d.error) {
            /* Includes the desktop's own "Please Select Branch First" (:176-180). */
            msg(d.error, false);
            S.accounts = []; S.inventory = []; S.export_ = []; S.commission = [];
        } else {
            if (d && d.accountsError)  msg('Accounts: ' + d.accountsError, false);
            if (d && d.inventoryError) msg('Inventory: ' + d.inventoryError, false);
            S.accounts   = (d && d.accounts) || [];
            S.inventory  = (d && d.inventory) || [];
            S.export_    = (d && d.export) || [];
            S.commission = (d && d.commission) || [];
        }

        render('adAccounts',   S.accounts,   'accounts');
        render('adInventory',  S.inventory,  'inventory');
        render('adExport',     S.export_,    'export_');
        render('adCommission', S.commission, 'commission');

        show('adExportSect',     'adExport',     !!(d && d.showExport));
        show('adCommissionSect', 'adCommission', !!(d && d.showCommission));

        var total = S.accounts.length + S.inventory.length
                  + S.export_.length + S.commission.length;
        if (empty) {
            empty.style.display = total ? 'none' : '';
            empty.textContent = 'Nothing is waiting for approval in this range.';
        }
    }).catch(function (e) {
        busy(btn, false);
        msg('Dashboard failed: ' + e.message, false);
        if (empty) { empty.style.display = ''; empty.textContent = 'Could not be read.'; }
    });
};

function show(sectId, wrapId, on) {
    var s = el(sectId), w = el(wrapId);
    if (s) s.style.display = on ? '' : 'none';
    if (w) w.style.display = on ? '' : 'none';
}

function render(wrapId, cards, group) {
    var wrap = el(wrapId);
    if (!wrap) return;
    if (!cards.length) { wrap.innerHTML = ''; return; }
    wrap.innerHTML = cards.map(function (c, i) {
        var zero = num(c.count) <= 0;
        return '<div class="ad-card' + (zero ? ' zero' : '') + '"'
             + ' onclick="adCardClick(\'' + group + '\',' + i + ')"'
             + ' title="TypeID ' + esc(c.id) + '">'
             + '<div class="t">' + esc(c.title) + '</div>'
             + '<div class="n">' + esc(c.count) + '</div>'
             + '</div>';
    }).join('');
}

/* ---------------------------------------------------------------- the card click
 * UserControl_Click, :244-1330. The whole switch, read out of the form - 84 cases, several of
 * which also fix a DocumentTypeId before showing. None of these approval screens is ported, and
 * approving a document is not something to approximate, so a card names its target rather than
 * opening anything.
 *
 * Note the first branch is a RANGE, not a case list: 0-9, plus 18, 34 and 35 (:248).
 */
var TARGETS = {
    19:   ['CommercialInvoiceApproval.cs', null],
    20:   ['PurchaseOrderForApproval.cs', null],
    21:   ['PurchaseOrderStoreForApproval.cs', 42],
    23:   ['PendingApprovalPurchaseInvoice.cs', 56],
    24:   ['PendingApprovalPurchaseInvoice.cs', 57],
    26:   ['PendingApprovalPurchaseInvoice.cs', 59],
    28:   ['frmSaleOrderForApproval.cs', 81],
    31:   ['PendingGDNForApproval_Engr.cs', null],
    32:   ['PendingApprovalSaleInvoice.cs', 95],
    33:   ['PendingApprovalSaleInvoice.cs', 99],
    36:   ['PendingApprovalPurchaseInvoice.cs', 98],
    37:   ['DeliveryOrderForApproval.cs', 84],
    38:   ['DeliveryOrderForApproval.cs', 85],
    39:   ['PendingApprovalGatePassOutward.cs', null],
    40:   ['PendingApprovalGatePassOutwardGeneral.cs', 92],
    54:   ['frmSaleOrderForApproval.cs', 127],
    56:   ['PendingApprovalGatePassOutward.cs', null],
    57:   ['LabPurchaseAnalysisForApproval.cs', null],
    58:   ['DepartmentRequestApproval.cs', null],
    59:   ['PendingApprovalSaleInvoice.cs', 171],
    60:   ['PendingApprovalPurchaseInvoice.cs', 172],
    62:   ['PendingJobOrderForApproval.cs', null],
    63:   ['DeliveryOrderForApproval.cs', 84],
    64:   ['DeliveryOrderForApproval.cs', 84],
    65:   ['PendingApprovalPurchaseInvoice.cs', 64],
    68:   ['PendingApprovalStockTransfer.cs', 68],
    70:   ['SaleOrderForApproval_Engr.cs', 1605],
    71:   ['DeliveryOrderApproval_Engr.cs', 1606],
    72:   ['PendingApprovalPurchaseInvoice.cs', 702],
    73:   ['frmDayBookApproval.cs', null],
    74:   ['StoreIssuanceFinancialOnApproval.cs', null],
    76:   ['PendingGRNForApproval.cs', 46],
    77:   ['SaleOrderForApproval_Concrete.cs', 1852],
    78:   ['DeliveryOrderForApproval_Concrete.cs', 1853],
    79:   ['SaleInvoiceForApproval_Concrete.cs', 1856],
    80:   ['SaleInvoiceForApproval_Concrete.cs', 1861],
    81:   ['PurchaseOrderForApproval_Concrete.cs', 1863],
    82:   ['PurchaseInvoiceForApproval_Concrete.cs', 1859],
    83:   ['PurchaseInvoiceForApproval_Concrete.cs', 1865],
    84:   ['SaleInvoiceForApproval_Concrete.cs', 1862],
    90:   ['frmSaleOrderForApproval.cs', 90],
    101:  ['PendingApprovalLabourWages.cs', 101],
    112:  ['frmProductionOutPutApprovalOrUnApproval.cs', null],
    126:  ['PendingApprovalSaleInvoice.cs', 126],
    127:  ['PendingApprovalSaleInvoice.cs', 126],
    138:  ['PendingApprovalPurchaseInvoice.cs', 138],
    141:  ['PendingStorePurchaseDemandForApproval.cs', null],
    142:  ['PendingFreightVoucherForApproval.cs', 28],
    145:  ['PendingApprovalSaleInvoice.cs', 145],
    155:  ['frmApprovalLogisticPurchaseServicesBill.cs', 155],
    184:  ['PendingApprovalSaleInvoice.cs', 184],
    185:  ['PendingApprovalSaleInvoice.cs', 1609],
    186:  ['PendingApprovalSaleInvoice.cs', 1608],
    187:  ['PendingApprovalSaleInvoice.cs', 1611],
    188:  ['PendingStorePurchaseDemandForApproval.cs', null],
    189:  ['PendingApprovalGatePassOutwardGeneral.cs', 52],
    202:  ['frmExportSalesContractApproval.cs', null],
    242:  ['frmExportReturnInvoiceApproval.cs', null],
    245:  ['PendingApprovalPurchaseInvoice.cs', 245],
    301:  ['LabPurchaseAnalysisForApproval.cs', null],
    303:  ['LabPurchaseAnalysisForApproval.cs', null],
    700:  ['PurchaseOrderStoreForApproval.cs', 700],
    806:  ['PendingApprovalStockTransfer.cs', 806],
    810:  ['PendingApprovalLabourWages.cs', 810],
    910:  ['DeliveryOrderForApproval.cs', 910],
    999:  ['frmThirdPartyLotInspectionApproval.cs', null],
    1000: ['frmPayrollPosting.cs', null],
    1052: ['frmApprovalPurchaseOrderCmagt.cs', 1052],
    1053: ['frmApprovalSaleOrderCmagt.cs', 1053],
    1056: ['frmApprovalCommissionTradeBill.cs', 1056],
    1100: ['PurchaseOrderForApproval.cs', null],
    1200: ['DeliveryOrderApproval_Engr.cs', 1200],
    1300: ['frmApprovalLogisticRateNegotiation.cs', 1300],
    1301: ['frmApprovalLogisticAgreement.cs', 1301],
    1302: ['frmApprovalLogisticRateNegotiation.cs', 1302],
    1303: ['frmApprovalLogisticPurchaseOrder.cs', 1303],
    1304: ['frmApprovalLogisticPurchaseServicesBill.cs', 1304],
    1600: ['PurchaseOrderStoreForApproval.cs', 1600],
    1618: ['PendingGRNForApproval_Engr.cs', null],
    1656: ['SaleOrderForApproval_Engr.cs', 1656],
    1657: ['DeliveryOrderApproval_Engr.cs', 1657],
    1660: ['PendingApprovalSaleInvoice.cs', 1660],
    1661: ['PendingApprovalSaleInvoice.cs', 1661],
    1870: ['frmWorkOrderApproval.cs', 1870]
};

function targetFor(id) {
    /* :248 - a RANGE plus three extras, not a case list. */
    if ((id >= 0 && id <= 9) || id === 18 || id === 34 || id === 35) {
        return ['PendingApprovalVouchersHistory.cs', null];
    }
    return TARGETS[id] || null;
}

window.adCardClick = function (group, i) {
    var list = S[group] || [];
    var c = list[i];
    if (!c) return;

    var id = num(c.id);
    var t = targetFor(id);
    if (!t) {
        msg('"' + txt(c.title) + '" (TypeID ' + id + ') has no branch in the desktop form\'s own '
          + 'switch either, so it opens nothing there.', false);
        return;
    }
    msg('"' + txt(c.title) + '" opens the desktop screen ' + t[0]
      + (t[1] !== null ? ' with DocumentTypeId ' + t[1] : '')
      + ', which has not been ported yet. Nothing was approved.', false);
};

/* BtnSpecialApprovalDashBoard_Click, :1358-1368 - opens SpecialApprovalDashboard, a separate
   desktop form that is not one of the DashBoard screens and has not been ported. */
window.adSpecial = function () {
    msg('Special Approval DashBoard is the desktop form '
      + 'Architecture.WinApp.SpecialApprovalDashboard.SpecialApprovalDashboard, which has not '
      + 'been ported yet.', false);
};

/* ---------------------------------------------------------------- toolbar */
window.adRefresh = function (btn) {
    /* Refresh_Click :1331-1344 - re-reads; it does not clear the filters. */
    if (S.busy) return;
    adShow(btn);
};

window.adToggleFullscreen = function () {
    document.body.classList.toggle('ad-full');
};

/* ---------------------------------------------------------------- boot */
function boot() {
    loadSetup().then(function () {
        adFromToggled();
        adToToggled();
        /* :128 - the desktop builds the cards as soon as the form loads. */
        var b = el('btnShow');
        if (b) adShow(b);
    });
}

if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
else boot();

})();
