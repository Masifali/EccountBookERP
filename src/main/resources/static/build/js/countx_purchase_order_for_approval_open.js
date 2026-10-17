/* ============================================================================================
 * Compatibility opener for older dashboard pages. Navigate directly because this page cannot be framed.
 *
 *   PoaOpen({ variant, branchId, fromDate, toDate })
 *     variant    "po" = ApprovalDashboard.PurchaseOrderForApproval (TypeID 20)
 *                "fm" = FeedMill.Approval.PurchaseOrderForApproval (TypeID 1100)
 *     branchId   BranchesId           - TypeID 20 only
 *     fromDate   RequestedFromDate    - TypeID 20, only when the dashboard's From Date tick box is set
 *     toDate     RequestedToDate      - TypeID 20, always the dashboard's To Date value
 *   PoaClose()   closes it (the page calls it on Esc / Ctrl+E / the close button)
 * ==========================================================================================*/
(function (w, d) {
    'use strict';
    var host = null;
    w.PoaOpen = function (o) {
        o = o || {};
        w.PoaClose();
        var q = new URLSearchParams();
        q.set('variant', o.variant === 'fm' ? 'fm' : 'po');
        if (o.variant !== 'fm') {
            q.set('branchId', String(parseInt(o.branchId, 10) || 0));
            if (o.fromDate) q.set('fromDate', o.fromDate);
            if (o.toDate) q.set('toDate', o.toDate);
        }
        w.location.assign('/dashboard/purchase-order-for-approval?' + q.toString());
    };
    w.PoaClose = function () {
        if (host && host.parentNode) host.parentNode.removeChild(host);
        host = null;
    };
}(window, document));
