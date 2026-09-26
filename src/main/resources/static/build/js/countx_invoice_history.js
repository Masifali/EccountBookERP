/* Form History for the seven Sale / Purchase invoice screens.
 *
 * One file, because the seven desktop forms share one history panel design: the same four
 * mutually exclusive date radios, the same From/To document-number pair, the same party filter and
 * the same grid-with-clickable-document-code. What differs between them - the DocumentTypeId and
 * which stored procedure runs - is decided SERVER-side from the screen key, so nothing here can
 * ask one screen for another screen's documents.
 *
 * The page sets window.INVOICE_HISTORY_SCREEN to its key before loading this file, e.g.
 *     <script>window.INVOICE_HISTORY_SCREEN = 'purchase-invoice';</script>
 *
 * Deliberately absent: any userId or "view all records" parameter. The desktop takes both from the
 * signed-in user and the screen's grant grid (InvfrmPurchaseInvoice.cs:4943-4947), and so does the
 * endpoint. Sending them from here would let a caller read other users' invoices.
 */
(function () {
    'use strict';

    var SCREEN = window.INVOICE_HISTORY_SCREEN || '';
    var URL_BASE = '/api/invoice-history/';
    var rows = [];
    var busy = false;

    function $(id) { return document.getElementById(id); }

    function esc(s) {
        return String(s === undefined || s === null ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }

    /* Dates are handled as plain yyyy-mm-dd strings throughout. toISOString() converts to UTC
       first, which at UTC+5 reports the PREVIOUS day for a local-midnight date. */
    function val(id) { var e = $(id); return e ? e.value : ''; }
    function intOf(id) { var n = parseInt(val(id), 10); return isNaN(n) ? 0 : n; }

    function dateMode() {
        var r = document.querySelector('input[name="ihDateMode"]:checked');
        return r ? r.value : 'doc';
    }

    function msg(text, isError) {
        var el = $('ihMessage');
        if (!el) return;
        el.textContent = text || '';
        el.style.color = isError ? '#b00020' : '#004d40';
    }

    /* Button guard: disabled immediately, loader shown, duplicate request blocked, and re-enabled
       in every exit path including failure - a button must never be left stuck disabled. */
    function setBusy(on) {
        busy = on;
        var b = $('ihSearchBtn');
        if (b) {
            b.disabled = on;
            b.innerHTML = on
                ? '<i class="fa fa-spinner fa-spin"></i> Searching...'
                : '<i class="fa fa-search"></i> Search';
        }
    }

    function buildQuery() {
        var p = [];
        function add(k, v) {
            if (v === '' || v === 0 || v === null || v === undefined) return;
            p.push(encodeURIComponent(k) + '=' + encodeURIComponent(v));
        }
        add('dateMode', dateMode());
        /* An unticked end is omitted, never defaulted to today - the desktop treats
           "from 1 Jan, no to-date" as a real query. */
        add('fromDate', val('ihFromDate'));
        add('toDate', val('ihToDate'));
        add('fromDocNo', intOf('ihFromDocNo'));
        add('toDocNo', intOf('ihToDocNo'));
        add('supplierCustomerId', intOf('ihParty'));
        add('paymentTermId', intOf('ihPaymentTerm'));
        add('noOfRecords', intOf('ihNoOfRecords'));
        var br = $('ihBranches');
        if (br && br.value) add('branchesIds', br.value);
        return p.length ? ('?' + p.join('&')) : '';
    }

    function search() {
        if (busy) return;                     /* duplicate request blocked */
        if (!SCREEN) { msg('This screen has no history key configured.', true); return; }
        setBusy(true);
        msg('');
        fetch(URL_BASE + encodeURIComponent(SCREEN) + buildQuery(),
              { headers: { 'Accept': 'application/json' } })
            .then(function (r) { return r.json().then(function (j) { return { ok: r.ok, body: j }; }); })
            .then(function (res) {
                rows = (res.body && res.body.rows) || [];
                if (!res.ok || res.body.success === false) {
                    msg(res.body && res.body.message ? res.body.message : 'History failed.', true);
                } else if (!rows.length) {
                    msg('No records matched.');
                } else {
                    msg(rows.length + ' record' + (rows.length === 1 ? '' : 's')
                        + (res.body.canViewAllRecords === false ? ' (your own records only)' : ''));
                }
                render();
            })
            .catch(function (e) {
                rows = [];
                render();
                msg('History request failed: ' + e, true);
            })
            .then(function () { setBusy(false); });   /* always re-enabled */
    }

    /* Columns follow the desktop history grid's own order and names
       (InvfrmPurchaseInvoice.cs:5011-5041). Whatever the procedure returns is shown - the header
       is built from the first row rather than from a hard-coded list, so a column the procedure
       adds is not silently dropped and one it omits does not render an empty column. */
    var LEAD = ['DocNo', 'DocDate', 'BranchName', 'BranchSrNo'];
    var HIDE = { Id: 1, VoucherHeadId: 1, DocumentTypeId: 1 };

    function columns() {
        if (!rows.length) return [];
        var keys = Object.keys(rows[0]).filter(function (k) { return !HIDE[k]; });
        var lead = LEAD.filter(function (k) { return keys.indexOf(k) >= 0; });
        var rest = keys.filter(function (k) { return lead.indexOf(k) < 0; });
        return lead.concat(rest);
    }

    function render() {
        var head = $('ihHead'), body = $('ihBody');
        if (!head || !body) return;
        var cols = columns();
        head.innerHTML = cols.length
            ? '<tr>' + cols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') + '</tr>'
            : '';
        if (!rows.length) {
            body.innerHTML = '<tr><td colspan="' + Math.max(cols.length, 1)
                + '" style="text-align:center;padding:8px;">No records.</td></tr>';
            return;
        }
        body.innerHTML = rows.map(function (r, i) {
            return '<tr>' + cols.map(function (c) {
                var v = r[c];
                if (v && typeof v === 'string' && /^\d{4}-\d{2}-\d{2}T/.test(v)) v = v.slice(0, 10);
                /* The document code is the clickable one, as on the desktop grid. */
                if (c === 'DocNo') {
                    return '<td><a href="#" onclick="return invoiceHistoryOpen(' + i + ');">'
                        + esc(v) + '</a></td>';
                }
                return '<td>' + esc(v) + '</td>';
            }).join('') + '</tr>';
        }).join('');
    }

    /* Clicking the document code hands the row to the screen. A screen that can load a document
       implements window.loadInvoiceFromHistory(row); one that cannot says so rather than pretending
       the click worked. */
    window.invoiceHistoryOpen = function (i) {
        var r = rows[i];
        if (!r) return false;
        if (typeof window.loadInvoiceFromHistory === 'function') {
            try { window.loadInvoiceFromHistory(r); }
            catch (e) { msg('Could not load that document: ' + e, true); }
        } else {
            msg('Document ' + (r.DocNo || '') + ' selected. Loading a document into this screen '
                + 'is not implemented yet, so nothing was opened.', true);
        }
        return false;
    };

    window.invoiceHistorySearch = search;

    /* The party filter is optional - history runs without it - but an empty dropdown is a dead
       control, so it is filled from the list the screen's own module already exposes. Sale screens
       read customers, purchase screens read suppliers: two different endpoints, because they are
       two different lists on the desktop too. A failure leaves "-- All --" standing rather than
       inventing names. */
    function fillSelect(id, url, idKeys, nameKeys) {
        var sel = $(id);
        if (!sel) return;
        fetch(url, { headers: { 'Accept': 'application/json' } })
            .then(function (r) { return r.ok ? r.json() : []; })
            .then(function (data) {
                var list = Array.isArray(data) ? data : (data && data.data) || [];
                list.forEach(function (row) {
                    var v = pick(row, idKeys), t = pick(row, nameKeys);
                    if (v === null || v === undefined || v === '') return;
                    var o = document.createElement('option');
                    o.value = v;
                    o.textContent = t === null || t === undefined ? String(v) : t;
                    sel.appendChild(o);
                });
            })
            .catch(function (e) { console.warn('history filter ' + id + ' not loaded', e); });
    }

    function pick(row, keys) {
        for (var i = 0; i < keys.length; i++) {
            if (row[keys[i]] !== undefined && row[keys[i]] !== null) return row[keys[i]];
        }
        return null;
    }

    function loadFilters() {
        var isSale = SCREEN.indexOf('sale-') === 0;
        fillSelect('ihParty',
            isSale ? '/api/sale/customers' : '/api/purchase/suppliers',
            ['id', 'Id'],
            isSale ? ['customerName', 'CompanyName', 'name']
                   : ['supplierName', 'CompanyName', 'name']);
        /* Sale only - the Purchase procedure has no @PaymentTermId parameter. */
        if (isSale) {
            fillSelect('ihPaymentTerm', '/api/sale/payment-terms',
                       ['id', 'Id'], ['termName', 'TermsDescription', 'name']);
        }
    }

    document.addEventListener('DOMContentLoaded', function () {
        var b = $('ihSearchBtn');
        if (b) b.addEventListener('click', search);
        loadFilters();
        render();
    });
}());
