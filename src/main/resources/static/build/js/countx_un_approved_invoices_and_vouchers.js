/* ============================================================================
 * Undo Approval (Dashboard) - "UnApproved Vouchers"
 * Ported from Architecture.WinApp.ApprovalDashboard\UnApprovedInvoicesAndVouchers.cs (788 lines).
 *
 *   :85-99    Load       From = today - 7, To = today, then the cards
 *   :101-161  DynamicallyGenerateCards   two procedures, three panels
 *   :163-481  UserControl_Click          TypeID decides which approval screen opens
 *   :483-486  btnNew_Click               Refresh
 *   :500-519  BtnShow_Click
 * ==========================================================================*/
(function () {
'use strict';

var API = '/api/dashboard/un-approved';

var S = { accounts: [], inventory: [], commission: [], busy: false };

/* ---------------------------------------------------------------- helpers */
function el(id)  { return document.getElementById(id); }
function val(id) { var e = el(id); return e ? e.value : ''; }
function setVal(id, v) { var e = el(id); if (e) e.value = v === null || v === undefined ? '' : v; }
function txt(v)  { return v === null || v === undefined ? '' : String(v); }
function esc(v)  { return txt(v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
                                .replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
function num(v)  { var n = parseFloat(v); return isNaN(n) ? 0 : n; }

function msg(text, ok) {
    var m = el('uaMessage');
    if (!m) return;
    m.className = ok ? 'ok' : 'err';
    m.textContent = text;
    if (ok) setTimeout(function () { if (m.textContent === text) m.className = ''; }, 6000);
}
function clearMsg() { var m = el('uaMessage'); if (m) { m.className = ''; m.textContent = ''; } }

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

window.uaFromToggled = function () {
    var f = el('fromDate'); if (f) f.disabled = !(el('chkFrom') && el('chkFrom').checked);
};
window.uaToToggled = function () {
    var t = el('toDate'); if (t) t.disabled = !(el('chkTo') && el('chkTo').checked);
};

/* ---------------------------------------------------------------- cards */
window.uaShow = function (btn) {
    if (S.busy) return;
    clearMsg();
    busy(btn, true);
    var empty = el('uaEmpty');
    if (empty) { empty.style.display = ''; empty.textContent = 'Loading...'; }

    var q = [];
    /* :120-127 - each date is copied only when its tick box is set; otherwise the parameter is
       omitted entirely, which is not the same as sending NULL. */
    if (el('chkFrom') && el('chkFrom').checked && val('fromDate')) {
        q.push('fromDate=' + encodeURIComponent(val('fromDate')));
    }
    if (el('chkTo') && el('chkTo').checked && val('toDate')) {
        q.push('toDate=' + encodeURIComponent(val('toDate')));
    }

    getJson(API + '/cards' + (q.length ? '?' + q.join('&') : '')).then(function (d) {
        busy(btn, false);
        if (d && d.accountsError)  msg('Accounts: ' + d.accountsError, false);
        if (d && d.inventoryError) msg('Inventory: ' + d.inventoryError, false);

        S.accounts   = (d && d.accounts) || [];
        S.inventory  = (d && d.inventory) || [];
        S.commission = (d && d.commission) || [];

        render('uaAccounts',   S.accounts,   'accounts');
        render('uaInventory',  S.inventory,  'inventory');
        render('uaCommission', S.commission, 'commission');

        /* PanelExportAndCommissionAgent.Visible = commissionRows.Any()  (:143) */
        var show = !!(d && d.showCommission);
        var sect = el('uaCommissionSect'), wrap = el('uaCommission');
        if (sect) sect.style.display = show ? '' : 'none';
        if (wrap) wrap.style.display = show ? '' : 'none';

        var total = S.accounts.length + S.inventory.length + S.commission.length;
        if (empty) {
            empty.style.display = total ? 'none' : '';
            empty.textContent = 'No approved documents waiting to be un-approved for this range.';
        }
    }).catch(function (e) {
        busy(btn, false);
        msg('Dashboard failed: ' + e.message, false);
        if (empty) { empty.style.display = ''; empty.textContent = 'Could not be read.'; }
    });
};

function render(wrapId, cards, group) {
    var wrap = el(wrapId);
    if (!wrap) return;
    if (!cards.length) { wrap.innerHTML = ''; return; }
    wrap.innerHTML = cards.map(function (c, i) {
        var zero = num(c.count) <= 0;
        return '<div class="ua-card' + (zero ? ' zero' : '') + '"'
             + ' onclick="uaCardClick(\'' + group + '\',' + i + ')"'
             + ' title="TypeID ' + esc(c.id) + '">'
             + '<div class="t">' + esc(c.title) + '</div>'
             + '<div class="n">' + esc(c.count) + '</div>'
             + '</div>';
    }).join('');
}

/* ---------------------------------------------------------------- the card click
 * UserControl_Click, :163-481. TypeID decides which approval screen opens, and several of them
 * also fix a DocumentTypeId before showing. The whole mapping, read out of that switch:
 *
 *   0-9, 34, 35  PendingApprovalVouchersHistory        flagAproved
 *   23           PendingApprovalPurchaseInvoice        DocumentTypeId 56
 *   24           PendingApprovalPurchaseInvoice        DocumentTypeId 57
 *   32           PendingApprovalSaleInvoice            DocumentTypeId 95
 *   33           PendingApprovalSaleInvoice            DocumentTypeId 99
 *   36           PendingApprovalPurchaseInvoice        DocumentTypeId 98
 *   37           PendingGRNForApproval_Engr
 *   38           PendingGDNForApproval_Engr
 *   39           PendingApprovalPurchaseInvoice        DocumentTypeId 702
 *   62           PendingJobOrderForApproval
 *   90           frmDayBookApproval
 *   112          frmProductionOutPutApprovalOrUnApproval
 *   138          PendingApprovalPurchaseInvoice        DocumentTypeId 138
 *   141, 188     PendingStorePurchaseDemandForApproval
 *   142          PendingFreightVoucherForApproval      DocumentTypeId 28
 *   145, 146     PendingApprovalSaleInvoice            DocumentTypeId 145
 *   245          PendingApprovalPurchaseInvoice        DocumentTypeId 245
 *   1052         frmApprovalPurchaseOrderCmagt         DocumentTypeId 1052
 *   1053         frmApprovalSaleOrderCmagt             DocumentTypeId 1053
 *   1056         frmApprovalCommissionTradeBill        DocumentTypeId 1056
 *   1870         frmWorkOrderApproval                  DocumentTypeId 1870
 *
 * None of those screens has been ported. Un-approving a document is not something to approximate,
 * so the card names its screen instead of opening anything.
 */
var TARGETS = {
    23:   ['PendingApprovalPurchaseInvoice.cs', 56],
    24:   ['PendingApprovalPurchaseInvoice.cs', 57],
    32:   ['PendingApprovalSaleInvoice.cs', 95],
    33:   ['PendingApprovalSaleInvoice.cs', 99],
    36:   ['PendingApprovalPurchaseInvoice.cs', 98],
    37:   ['PendingGRNForApproval_Engr.cs', null],
    38:   ['PendingGDNForApproval_Engr.cs', null],
    39:   ['PendingApprovalPurchaseInvoice.cs', 702],
    62:   ['PendingJobOrderForApproval.cs', null],
    90:   ['frmDayBookApproval.cs', null],
    112:  ['frmProductionOutPutApprovalOrUnApproval.cs', null],
    138:  ['PendingApprovalPurchaseInvoice.cs', 138],
    141:  ['PendingStorePurchaseDemandForApproval.cs', null],
    142:  ['PendingFreightVoucherForApproval.cs', 28],
    145:  ['PendingApprovalSaleInvoice.cs', 145],
    146:  ['PendingApprovalSaleInvoice.cs', 145],
    188:  ['PendingStorePurchaseDemandForApproval.cs', null],
    245:  ['PendingApprovalPurchaseInvoice.cs', 245],
    1052: ['frmApprovalPurchaseOrderCmagt.cs', 1052],
    1053: ['frmApprovalSaleOrderCmagt.cs', 1053],
    1056: ['frmApprovalCommissionTradeBill.cs', 1056],
    1870: ['frmWorkOrderApproval.cs', 1870]
};

function targetFor(id) {
    /* :166 - the first branch is a RANGE, not a case list. */
    if ((id >= 0 && id <= 9) || id === 34 || id === 35) {
        return ['PendingApprovalVouchersHistory.cs', null];
    }
    return TARGETS[id] || null;
}

window.uaCardClick = function (group, i) {
    var list = group === 'accounts' ? S.accounts
             : group === 'inventory' ? S.inventory
             : S.commission;
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
      + ', which has not been ported yet. Nothing was un-approved.', false);
};

/* ---------------------------------------------------------------- toolbar */
window.uaRefresh = function (btn) {
    /* btnNew_Click :483-486 - the desktop re-runs the cards; it does not clear the dates. */
    if (S.busy) return;
    uaShow(btn);
};

window.uaToggleFullscreen = function () {
    document.body.classList.toggle('ua-full');
};

/* ---------------------------------------------------------------- boot */
function boot() {
    getJson(API + '/defaults').then(function (d) {
        setVal('fromDate', (d && d.fromDate) || '');
        setVal('toDate',   (d && d.toDate) || '');
    }).catch(function () { /* the Show below reports any real failure */ })
      .then(function () {
        uaFromToggled();
        uaToToggled();
        /* :95 - the desktop builds the cards as soon as the form loads. */
        var b = el('btnShow');
        if (b) uaShow(b);
      });
}

if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
else boot();

})();
