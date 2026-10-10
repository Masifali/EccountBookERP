/* ============================================================================================
 * Group E2 (Sale Engr reports 555 / 554 / 556 / 848) - small shared helpers on top of window.AcRpt1 and window.SaRpt1.
 *   window.SaEngr = { api, attachments, ledger, digits, fmtDefaultDate, nonCommaInt }
 * Nothing of the shared engines is edited.
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1;
    var E = w.SaEngr = {};

    /* CommonServices.GetNoofAttachmentsByRefDocumentTypeID(id, documentTypeId): AttachmentView lists name / custom name / entry date; nothing opens when there are none */
    E.attachments = function (api, id, documentTypeId) {
        return A.getJson(api + '/attachments?' + S.qs({ id: id, documentTypeId: documentTypeId })).then(function (rows) {
            if (!rows || !rows.length) return;
            var tb = A.el('attachRows'); tb.innerHTML = '';
            rows.forEach(function (r) {
                var tr = d.createElement('tr');
                var a = d.createElement('a'); a.textContent = r.AttachmentName == null ? '' : String(r.AttachmentName);
                a.href = api + '/attachments/download?' + S.qs({ id: id, documentTypeId: documentTypeId, attachmentId: r.Id });
                a.target = '_blank'; a.rel = 'noopener';
                var c1 = d.createElement('td'); c1.appendChild(a);
                var c2 = d.createElement('td'); c2.textContent = r.CustomName == null ? '' : String(r.CustomName);
                var c3 = d.createElement('td'); c3.textContent = S.fmtDate(r.EntryDate, 'dd/MM/yyyy hh:mm:ss tt');
                tr.appendChild(c1); tr.appendChild(c2); tr.appendChild(c3); tb.appendChild(tr);
            });
            A.el('attachDialog').showModal();
        });
    };
    (function () {
        var b = A.el('closeAttach'); if (b) b.addEventListener('click', function () { A.el('attachDialog').close(); });
    }());

    /* frmApprovalCommentory (RefDocumentTypeId, RefDocNoId): the approval history of one document. q = the report's filters + id, documentTypeId, idColumn, typeColumn */
    var APPROVAL_COLS = [['DocumentName', 'DocumentName'], ['ApproveLevel', 'ApprovalLevel'], ['UserName', 'UserName'], ['UserComments', 'UserComments'],
        ['Approved_Status', 'Approved_Status'], ['ApprovalDate', 'ApproveDate'], ['Rejected_Status', 'Rejected_Status'], ['RejectedDate', 'RejectedDate'],
        ['IsFinalApprover', 'IsFinalApprover'], ['IsMandatory', 'IsMandatory']];
    E.approvalHistory = function (api, q) {
        return A.getJson(api + '/approval-history?' + S.qs(q)).then(function (rows) {
            var head = A.el('approvalHead'), body = A.el('approvalRows');
            head.innerHTML = ''; body.innerHTML = '';
            APPROVAL_COLS.forEach(function (c) { var th = d.createElement('th'); th.textContent = c[1]; head.appendChild(th); });
            (rows || []).forEach(function (r) {
                var tr = d.createElement('tr');
                APPROVAL_COLS.forEach(function (c) {
                    var v = A.ci(r, c[0]), td = d.createElement('td');
                    if (typeof v === 'boolean') td.textContent = v ? '\u2611' : '\u2610';
                    else if (/Date$/.test(c[0])) td.textContent = v ? S.fmtDate(v, 'dd/MM/yyyy hh:mm:ss tt') : '';
                    else td.textContent = v == null ? '' : String(v);
                    tr.appendChild(td);
                });
                body.appendChild(tr);
            });
            A.el('approvalDialog').showModal();
        });
    };
    (function () {
        var b = A.el('closeApproval'); if (b) b.addEventListener('click', function () { A.el('approvalDialog').close(); });
    }());

    /* CommonServices.GoToGeneralLedgerFromLinkedEvent(GetGlAccountIdBySupplierCustomerId(id), from, to) */
    E.ledger = function (api, supplierCustomerId, from, to) {
        return A.getJson(api + '/gl-account?' + S.qs({ supplierCustomerId: A.toInt(supplierCustomerId) })).then(function (o) {
            var acc = A.toInt(o && o.glAccountId);
            if (acc <= 0) { alert('No ledger account is linked to this customer.'); return; }
            w.open('/accounts/reports/general-ledger?' + S.qs({ accountId: acc, fromDate: from, toDate: to }), '_blank');
        });
    };

    /* CommonServices.OnlytextNumberFunction: digits only */
    E.digits = function (id) { S.digitsOnly(id, false); };

    /* Convert.ToInt32 on a decimal (banker's rounding), Conversion.ToInt of a DataTable.Compute sum */
    E.toInt32 = function (v) {
        var n = Number(v); if (!isFinite(n)) return 0;
        var f = Math.floor(n), diff = n - f;
        if (diff < 0.5) return f; if (diff > 0.5) return f + 1; return f % 2 === 0 ? f : f + 1;
    };

    /* Janus default DateTime cell (no FormatString): short date, with the time only when it is not midnight */
    E.dateDefault = function (v) {
        if (v === null || v === undefined || v === '') return '';
        var m = /T(\d{2}):(\d{2}):(\d{2})/.exec(String(v));
        if (m && (m[1] !== '00' || m[2] !== '00' || m[3] !== '00')) return S.fmtDate(v, 'dd/MM/yyyy hh:mm:ss tt');
        return S.fmtDate(v, 'dd/MM/yyyy');
    };

    /* the date cells hold the already formatted text (date: 'x' columns call fmtDate with the pattern, so a default cell is pre-formatted here) */
    E.fixDates = function (rows, keys) {
        rows.forEach(function (r) { keys.forEach(function (k) { r[k] = E.dateDefault(r[k]); }); });
        return rows;
    };
}(window, document));
