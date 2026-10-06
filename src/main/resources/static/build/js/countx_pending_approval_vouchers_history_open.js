/* ============================================================================================
 * Opens Approvals - vouchers (PendingApprovalVouchersHistory) over a dashboard, the way the desktop
 * ShowDialog()s it from ApprovalDashboard.UserControl_Click (:248-259) and
 * UnApprovedInvoicesAndVouchers.UserControl_Click (:166-176).
 *
 *   PavhOpen({ id, fromDate, toDate, unApprove })
 *     id         card TypeID (PendingApprovalVouchersHistory.Id)
 *     fromDate   RequestedFromDate - pass only when the dashboard's From Date tick box is set
 *     toDate     RequestedToDate   - always the dashboard's To Date value
 *     unApprove  true from Undo Approval (obj.flagAproved = true)
 *   PavhClose()  closes it (the page calls it on Esc / Ctrl+E / the close button)
 * ==========================================================================================*/
(function (w, d) {
    'use strict';
    var host = null;
    w.PavhOpen = function (o) {
        o = o || {};
        w.PavhClose();
        var q = new URLSearchParams();
        q.set('id', String(parseInt(o.id, 10) || 0));
        if (o.fromDate) q.set('fromDate', o.fromDate);
        if (o.toDate) q.set('toDate', o.toDate);
        if (o.unApprove) q.set('unApprove', '1');
        q.set('popup', '1');
        host = d.createElement('div');
        host.id = 'pavhHost';
        host.setAttribute('role', 'dialog');
        host.setAttribute('aria-label', 'Approvals');
        host.style.cssText = 'position:fixed;inset:0;z-index:3500;background:rgba(0,0,0,.35);display:flex;';
        var f = d.createElement('iframe');
        f.title = 'Approvals';
        f.src = '/dashboard/pending-approval-vouchers-history?' + q.toString();
        f.style.cssText = 'flex:1 1 auto;border:0;margin:0;background:#fff;width:100%;height:100%;';
        host.appendChild(f);
        d.body.appendChild(host);
        f.addEventListener('load', function () { try { f.contentWindow.focus(); } catch (e) { /* ignore */ } });
    };
    w.PavhClose = function () {
        if (host && host.parentNode) host.parentNode.removeChild(host);
        host = null;
    };
}(window, document));
