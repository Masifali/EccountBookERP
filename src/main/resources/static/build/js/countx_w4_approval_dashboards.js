/* ============================================================================
 * 364 PendingDocumentForMultipleApprovals (305 lines) and 368 ApprovalDashboard_St (333 lines).
 *
 *   364  :31-95   DynamicallyGenerateCards   USP_GetPendingDocumentApprovalsForDashboard, one card per row
 *        :97-118  UserControl_Click          TargetUrl empty -> "Target URL not Configured properly... <title>"
 *        :120-130 Refresh_Click
 *   368  :36-92   DynamicallyGenerateCards   Sp_Voucher_ReadDashboard + [ST].[USP_Inventory_ReadDashboardPendingForApproval]
 *        :94-156  UserControl_Click          0-9 -> PendingApprovalVouchersHistory; 20, 25, 27, 28, 30 -> steel forms
 *        :158-170 Refresh_Click
 * ==========================================================================*/
(function () {
'use strict';

var MODE = document.body.getAttribute('data-w4-mode') || 'multi';
var API = MODE === 'steel' ? '/api/dashboard/w4-approvals/steel' : '/api/dashboard/w4-approvals/pending-documents';
var S = { accounts: [], inventory: [], busy: false };

function el(id) { return document.getElementById(id); }
function txt(v) { return v === null || v === undefined ? '' : String(v); }
function esc(v) { return txt(v).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
function num(v) { var n = parseFloat(v); return isNaN(n) ? 0 : n; }

function msg(text, ok) { var m = el('w4Message'); m.className = ok ? 'ok' : 'err'; m.textContent = text; }
function clearMsg() { var m = el('w4Message'); m.className = ''; m.textContent = ''; }
function getJson(url) {
    return fetch(url, { headers: { 'Accept': 'application/json' } }).then(function (r) {
        if (!r.ok) throw new Error('HTTP ' + r.status + ' on ' + url);
        return r.json();
    });
}

/* ApprovalDashboard_St.UserControl_Click :94-156 - the targets the desktop opens for the non-voucher cards */
var STEEL_TARGETS = {
    20: ['PurchaseOrderForApproval_St.cs', null],
    25: ['SaleOrderForApproval_St.cs', 1505],
    27: ['SaleInvoiceForApproval_St.cs', 1509],
    28: ['SaleInvoiceForApproval_St.cs', 1510],
    30: ['DeliveryOrderForApproval_St.cs', 1506]
};

function render(wrapId, cards, group) {
    var wrap = el(wrapId);
    if (!cards.length) { wrap.innerHTML = ''; return; }
    wrap.innerHTML = cards.map(function (c, i) {
        var zero = num(c.count) <= 0;
        var href = (MODE === 'multi' && c.route) ? c.route + (c.route.indexOf('?') < 0 ? '?' : '&') + 'documentTypeId=' + encodeURIComponent(c.id) : null;
        var tag = href ? 'a' : 'div';
        return '<' + tag + ' class="w4-ac' + (zero ? ' zero' : '') + '"' + (href ? ' href="' + esc(href) + '"' : '')
            + ' data-g="' + group + '" data-i="' + i + '" title="' + (MODE === 'steel' ? 'TypeID ' : 'DocumentTypeId ') + esc(c.id) + '">'
            + '<div class="t">' + esc(c.title) + '</div><div class="n">' + esc(c.count) + '</div></' + tag + '>';
    }).join('');
}

function onCard(group, i, ev) {
    var c = (S[group] || [])[i];
    if (!c) return;
    clearMsg();
    if (MODE === 'multi') {
        if (!c.targetUrl) {                                   // :103-106
            if (ev) ev.preventDefault();
            msg('Target URL not Configured properly... ' + c.title, false);
            return;
        }
        if (c.route) return;                                  // real link, the browser follows it
        if (ev) ev.preventDefault();
        msg('"' + c.title + '" opens the desktop screen ' + c.targetUrl + ', which has no page in this application yet. Nothing was approved.', false);
        return;
    }
    /* steel */
    var id = num(c.id);
    if (id >= 0 && id <= 9) {                                 // :96-101 PendingApprovalVouchersHistory with Id = TypeID, no dates passed
        if (typeof window.PavhOpen === 'function') window.PavhOpen({ id: id });
        return;
    }
    var t = STEEL_TARGETS[id];
    if (!t) { msg('"' + c.title + '" (TypeID ' + id + ') has no branch in the desktop form\'s own switch either, so it opens nothing there.', false); return; }
    msg('"' + c.title + '" opens the desktop screen ' + t[0] + (t[1] !== null ? ' with DocumentTypeId ' + t[1] : '')
        + ', which has not been ported yet. Nothing was approved.', false);
}

function load() {
    if (S.busy) return;
    S.busy = true; clearMsg();
    var b = el('w4Refresh'); b.disabled = true; b.classList.add('w4-btn-busy');
    var empty = el('w4Empty'); empty.style.display = ''; empty.textContent = 'Loading...';
    getJson(API).then(function (d) {
        S.busy = false; b.disabled = false; b.classList.remove('w4-btn-busy');
        if (d && d.error) msg(d.error, false);
        if (d && d.accountsError) msg('Accounts: ' + d.accountsError, false);
        if (d && d.inventoryError) msg('Inventory: ' + d.inventoryError, false);
        S.accounts = (d && d.accounts) || [];
        S.inventory = (d && d.inventory) || [];
        render('w4Accounts', S.accounts, 'accounts');
        render('w4Inventory', S.inventory, 'inventory');
        var total = S.accounts.length + S.inventory.length;
        empty.style.display = total ? 'none' : '';
        empty.textContent = 'Nothing is waiting for approval.';
    }).catch(function (e) {
        S.busy = false; b.disabled = false; b.classList.remove('w4-btn-busy');
        msg('Dashboard failed: ' + e.message, false);
        empty.style.display = ''; empty.textContent = 'Could not be read.';
    });
}

document.addEventListener('DOMContentLoaded', function () {
    el('w4Refresh').addEventListener('click', load);          // Refresh_Click
    document.addEventListener('click', function (e) {
        var card = e.target.closest ? e.target.closest('.w4-ac') : null;
        if (card) onCard(card.getAttribute('data-g'), parseInt(card.getAttribute('data-i'), 10), e);
    });
    load();                                                    // ApprovalDashboard_Load -> DynamicallyGenerateCards
});

})();
