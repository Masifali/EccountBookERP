/* ============================================================================================
 * Cost Center picker — shared by the vouchers that carry one.
 *
 * Desktop source, traced from ContraVoucher.cs:
 *
 *   CostCenterFill:1172 -> Projects.GetSubCostCenters(OrganizationId, CompanyId, UserId,
 *                          AppId, ParentCostCenterId: 0)          (BLL 0078:126)
 *                       -> usp_getCostCenters
 *       @OrganizationId, @CompanyId, @UserId   always
 *       @AppId              only when non-zero - the form passes UserAccount.AppId
 *       @ParentCostCenterId only when non-zero - the form passes 0, so it is never sent
 *
 *   Bound as Id / CostCenterName with ZeroIndex:false, so there is NO blank row, and the
 *   drop grid's columns 2 and 3 are hidden.
 *
 * AND THE CONTROL IS NOT ALWAYS THERE (ContraVoucher.cs:893):
 *
 *   ApplicationsAllocateToCompany obj = ...Find(row => row.AppId == 5
 *                                                   && row.CompanyId == UserAccount.CompanyId);
 *   IsBookingOffice = obj != null && obj.IsActive;
 *   if (!IsBookingOffice) { CostCenterPanel.Visible = false; }
 *
 * So a company without an active Booking Office allocation sees no Cost Center at all. The
 * endpoint returns that user's cost centres for their own company and app, so an empty list is
 * the same condition, and the control is hidden rather than left showing an empty picker.
 *
 * This replaces a hardcoded <option value="1"> that five voucher templates carried. That option
 * named the company, not a cost centre, and it posted the literal id 1 on save.
 * ============================================================================================ */
(function () {
    'use strict';

    var ENDPOINT = '/accounts/api/reports/cost-centers';

    function ci(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        for (var k in row) {
            if (Object.prototype.hasOwnProperty.call(row, k)
                && k.toLowerCase() === name.toLowerCase()) return row[k];
        }
        return '';
    }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }

    /** The desktop hides the whole panel, so hide the label and the control together. */
    function hidePanel(sel) {
        var node = sel;
        for (var i = 0; i < 6 && node && node.parentElement; i++) {
            node = node.parentElement;
            if (node.classList && node.classList.contains('row')) { node.style.display = 'none'; return; }
        }
        sel.style.display = 'none';
    }

    function fill(sel, rows) {
        /* ZeroIndex:false - no blank row is added. */
        sel.innerHTML = rows.map(function (r) {
            return '<option value="' + esc(ci(r, 'Id')) + '">'
                 + esc(ci(r, 'CostCenterName')) + '</option>';
        }).join('');
    }

    function boot() {
        var sel = document.getElementById('cmbCostCenter');
        if (!sel) return;
        /* Nothing is posted until the real list arrives - the hardcoded option is gone from the
           markup, so an unanswered request leaves the control empty rather than wrong. */
        sel.innerHTML = '';
        fetch(ENDPOINT, { headers: { 'Accept': 'application/json' }, credentials: 'same-origin' })
            .then(function (r) { return r.ok ? r.json() : []; })
            .then(function (rows) {
                if (!rows || !rows.length) { hidePanel(sel); return; }
                fill(sel, rows);
            })
            .catch(function () { hidePanel(sel); });
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
    else boot();
}());
