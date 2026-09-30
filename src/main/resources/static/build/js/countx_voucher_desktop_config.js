/*
 * Configuration-screen settings used by the Accounts voucher pages, the way the desktop forms
 * use them. Backed by VoucherDesktopConfigController (/accounts/api/vouchers/desktop-config,
 * /detail-accounts, /outstanding-cheques).
 *
 *   PaymentVoucherNew.cs  DefaultConfigurations():3294  CheqNoFill():2205  CheckingChequeNoSerialWise():651
 *                         grd_ColumnButtonClick():2782  cbPrintOnSave.Visible = ChequePrintingEnable
 *                         ReadById():2574/2619 (which remark text the edit reload shows)
 *   ReceiptsVoucherNew.cs / ContraVoucher.cs / VoucherEntry.cs / JournalVoucher.cs / ExpenseVoucherNew.cs
 */
(function (w, $) {
    "use strict";

    var D = w.VoucherDesk = {
        flags: {},
        cheques: [],          // dtCheq: [{id, cheqNo}]
        ready: null
    };

    D.ready = $.getJSON('/accounts/api/vouchers/desktop-config')
        .then(function (f) { D.flags = f || {}; return D.flags; },
              function () { D.flags = {}; return D.flags; });

    /** DetailAccountFill() of a form: payment | receipt | journal | journalEntry | expense */
    D.detailAccountsUrl = function (form, query) {
        return '/accounts/api/vouchers/detail-accounts?form=' + encodeURIComponent(form) +
               '&query=' + encodeURIComponent(query || '');
    };

    /**
     * CheqNoFill(): only when the voucher is a BPV (or the form always uses it) and
     * "CheqBook Enabled" is on. Fills the <datalist> tied to $input and keeps dtCheq.
     * LimitToList = ChequeNoCompulsoryOnBpv - when off, any typed text is still accepted.
     */
    D.loadCheques = function (bankId, recId, $input) {
        D.cheques = [];
        var listId = $input.attr('list');
        var $dl = listId ? $('#' + listId) : $();
        $dl.empty();
        if (!D.flags.chequeBookEnabled || !(bankId > 0)) return $.Deferred().resolve([]).promise();
        return $.getJSON('/accounts/api/vouchers/outstanding-cheques', { bankId: bankId, recId: recId || 0 })
            .then(function (rows) {
                D.cheques = rows || [];
                D.cheques.forEach(function (c) {
                    $dl.append($('<option>').attr('value', c.cheqNo));
                });
                return D.cheques;
            });
    };

    /** CmbCheqNo.Value for the typed text (0 when it is not an outstanding leaf). */
    D.chequeIdFor = function (text) {
        var t = (text || '').trim();
        for (var i = 0; i < D.cheques.length; i++) {
            if (String(D.cheques[i].cheqNo).trim() === t) return parseInt(D.cheques[i].id, 10) || 0;
        }
        return 0;
    };

    function toInt(v) { var n = parseInt(v, 10); return isNaN(n) ? 0 : n; }
    function minOf(list, msg) {
        if (!list.length) throw new Error(msg || 'Sequence contains no elements');
        return Math.min.apply(null, list);
    }

    /**
     * CheckingChequeNoSerialWise() - throws Error(message) exactly where the desktop throws.
     * @param chequeText   CmbCheqNo.Text being added
     * @param gridCheques  ChequeNo of every row already in the grid
     */
    D.checkSerialWise = function (chequeText, gridCheques) {
        if (D.cheques.length <= 0) return;
        var entered = toInt(chequeText);
        var leaves = D.cheques.map(function (c) { return toInt(c.cheqNo); });
        if (gridCheques.length === 0) {
            if (entered !== minOf(leaves)) throw new Error('Please Insert Cheque No In Detail Grid Serial Wise');
            return;
        }
        var distinct = [];
        gridCheques.forEach(function (c) { var n = toInt(c); if (distinct.indexOf(n) < 0) distinct.push(n); });
        if (distinct.length === 1) {
            var others = leaves.filter(function (n) { return n !== distinct[0]; });
            if (entered > minOf(others)) throw new Error('Please Insert Cheque No In Detail Grid Serial Wise');
            return;
        }
        var min = Math.min.apply(null, distinct), max = Math.max.apply(null, distinct);
        if (entered < max) {
            throw new Error('Please Insert Cheque No In Detail Grid Serial Wise! You Inserting ' + chequeText +
                            ' ChequeNo And in Grid ' + max + ' ChequeNo Is Present');
        }
        var outside = D.cheques.filter(function (c) {
            var t = (c.cheqNo || '').toString();
            if (!t.trim() || isNaN(parseInt(t, 10))) return true;
            var r = parseInt(t, 10);
            return r < min || r > max;
        }).map(function (c) { return toInt(c.cheqNo); });
        var minValue3 = minOf(outside);
        if (entered >= min && entered < max) throw new Error('Please Insert Cheque No In Detail Grid Serial Wise');
        if (entered > minValue3) throw new Error('Please Insert Cheque No In Detail Grid Serial Wise');
    };

    /** grd_ColumnButtonClick "Delete" with ChequeBookSerialWiseOnBpv: only the highest cheque row goes. */
    D.checkSerialDelete = function (rowCheque, gridCheques) {
        var max = Math.max.apply(null, gridCheques.map(toInt));
        if (toInt(rowCheque) !== max) {
            throw new Error("You can't Delete This Row Beacuse This Entry Is Against Serial Wise Cheque No");
        }
    };

    /** ReadById(): header remarks text the desktop puts back in txtremarksmain. */
    D.editHeaderRemarks = function (form, docTypeId, h) {
        var auto = !!D.flags.autoRemarksForPaymentThroughBank;
        var rem = h.remarks, other = h.remarksOtherLingo;
        if (form === 'PaymentVoucherNew') {
            if (docTypeId !== 2) return rem || '';
            return (auto ? other : (other != null ? other : rem)) || '';
        }
        if (form === 'ContraVoucher') {
            var has = (rem || '').indexOf('CHEQUE NO') >= 0;
            return (auto ? other : (has ? other : rem)) || '';
        }
        return rem || '';
    };

    /** ReadById(): detail remarks text shown in the grid on edit. */
    D.editRowRemarks = function (form, docTypeId, d) {
        var auto = !!D.flags.autoRemarksForPaymentThroughBank;
        if (form === 'PaymentVoucherNew') {
            if (docTypeId !== 2) return d.comments || '';
            return (!auto ? (d.commentsOtherLingo != null ? d.commentsOtherLingo : d.comments)
                          : d.commentsOtherLingo) || '';
        }
        if (form === 'ContraVoucher') {
            return (auto ? d.commentsOtherLingo
                         : (d.commentsOtherLingo != null ? d.commentsOtherLingo : d.comments)) || '';
        }
        return d.comments || '';
    };

    /**
     * Button guard: disable immediately, show a spinner, ignore repeat clicks, re-enable when the
     * request settles (success or failure). fn must return a promise/jqXHR (or nothing).
     */
    D.busy = function (btn, fn) {
        var $b = $(btn);
        if ($b.data('vd-busy')) return;
        $b.data('vd-busy', true).prop('disabled', true).attr('aria-busy', 'true');
        var $sp = $('<i class="fa fa-spinner fa-spin vd-spin" style="margin-right:4px"></i>').prependTo($b);
        var done = function () { $sp.remove(); $b.data('vd-busy', false).prop('disabled', false).removeAttr('aria-busy'); };
        var p;
        try { p = fn(); } catch (e) { done(); throw e; }
        if (p && typeof p.always === 'function') p.always(done);
        else if (p && typeof p.finally === 'function') p.finally(done);
        else done();
        return p;
    };
})(window, jQuery);
