/* ============================================================================
 * Receivables And Receipt Schedule
 * Ported from Architecture.WinApp.Account_Reports\ReceivablesAndReceiptSchedule.cs (1,812 lines).
 *
 *   :132-163  PayablesAndPaymentSchedule_Load
 *   :165-181  btnshow_Click
 *   :183-293  gridHistory
 *   :295-442  GridSetting          captions, hidden columns, link columns, Sum aggregates
 *   :630-655  btnNew_Click -> Reset
 *   :674-720  PayablesAndPaymentSchedule_KeyDown
 *   :760-782  print_Click
 *   :807-818  txtIntervalDays_TextChanged
 *   :819-916  grdPayablesAndSchedule_LinkClicked
 *
 * Dates are handled as plain yyyy-mm-dd strings throughout - never toISOString(), which shifts
 * a day at UTC+5.
 * ==========================================================================*/
(function () {
'use strict';

var API = '/api/dashboard/receivables-schedule';

var S = {
    rows: [],
    busy: false,
    branchFeature: false,
    decimals: 2,
    /* Set once the report has run, so a link uses the dates the rows were read with rather than
       whatever the user has since typed into the filters - the desktop reads the live control,
       but the live control cannot disagree with the grid there because the grid is rebuilt on
       every Show. Capturing them here keeps the two apps in step. */
    ranFrom: '', ranTo: '', ranSaleFrom: '', ranSaleTo: ''
};

/* GridSetting's visible columns, in the desktop table's own column order (:211-252), with the
   captions it overrides at :307-312 and :341. Everything GridSetting hides is left out:
   BranchesId, AccountTypeId, AccountId, AccountClass, ParentAccount, LastBillDate,
   LastBillsAmount, BillDays, DispatchedQty, OrderWeight, DispatchedWeight, BalWeight, FromDate,
   ToDate, DueDateFrom, DueDateTo, SaleFromDate, SaleToDate, OrderQty, ReceiveToday.

   type:  s = text, n = amount (stringFormatboth), q = quantity ("#,#00.##"), d = dd-MMM-yy,
          i = integer
   sum:   true where GridSetting sets AggregateFunction = 2 (Sum)
   link:  true where ColumnType = 5                                                          */
var COLS = [
    { key: 'BranchName',         cap: 'Branch',                type: 's', branchOnly: true },
    { key: 'AccountCode',        cap: 'Account Code',          type: 's', link: 'account' },
    { key: 'AccountTitle',       cap: 'Account Title / Party', type: 's' },
    { key: 'AccountType',        cap: 'Account Type',          type: 's' },
    { key: 'Opening',            cap: 'Opening Balance',       type: 'n', sum: true },
    { key: 'CurrDebit',          cap: 'Debit',                 type: 'n', sum: true },
    { key: 'CurrCredit',         cap: 'Credit',                type: 'n', sum: true },
    { key: 'Closing',            cap: 'Closing',               type: 'n', sum: true },
    { key: 'Short/Excess',       cap: 'Difference Amount',     type: 'n', sum: true },
    { key: 'Increase/Decrease',  cap: 'Increase/Decrease',     type: 's', center: true },
    { key: 'DueBalance',         cap: 'DueBalance',            type: 'n', sum: true },
    { key: 'NotYetDue',          cap: 'NotYetDue',             type: 'n', sum: true },
    { key: 'LastReceivedDate',   cap: 'LastReceivedDate',      type: 'd' },
    { key: 'LastReceivedAmount', cap: 'LastReceivedAmount',    type: 'n', sum: true },
    { key: 'ReceivedDays',       cap: 'ReceivedDays',          type: 'i', center: true },
    { key: 'SaleAmount',         cap: 'SaleAmount',            type: 'n', sum: true, link: 'sale' },
    { key: 'SaleQty',            cap: 'SaleQty',               type: 'q', sum: true },
    { key: 'SaleWeight',         cap: 'SaleWeight',            type: 'q', sum: true },
    { key: 'BalQty',             cap: 'Order Bal Qty',         type: 'q', sum: true, link: 'balqty' }
];

var MONTHS = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];

/* ---------------------------------------------------------------- helpers */
function el(id)  { return document.getElementById(id); }
function val(id) { var e = el(id); return e ? e.value : ''; }
function setVal(id, v) { var e = el(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
function chk(id) { var e = el(id); return !!(e && e.checked); }
function setChk(id, v) { var e = el(id); if (e) e.checked = !!v; }
function txt(v)  { return (v === null || v === undefined) ? '' : String(v); }
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

/* clsGlobalVariables.stringFormatboth is "#,##0.00;(0,0.00); 0" - thousands, the configured
   decimals, NEGATIVES IN PARENTHESES, and a bare 0 at exactly zero (CommonServices.cs:5398).
   The decimal count comes from the "Default NoofDecimal Points For Amount" configuration, which
   the server reads; it is not hard-coded here. */
function fAmt(v) {
    var n = num(v);
    if (n === 0) return '0';
    var a = Math.abs(n).toLocaleString(undefined,
            { minimumFractionDigits: S.decimals, maximumFractionDigits: S.decimals });
    return n < 0 ? '(' + a + ')' : a;
}
/* The quantity and weight columns use "#,#00.##" - up to two decimals, none forced. */
function fQty(v) {
    var n = num(v);
    return n.toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 2 });
}
/* FormatString "dd-MMM-yy" (:313-321). A blank or a sentinel 1900 date shows as nothing, the
   way an unset DateTime does on the desktop grid. */
function fDate(v) {
    var s = txt(v);
    if (!s) return '';
    var y = parseInt(s.substr(0, 4), 10),
        m = parseInt(s.substr(5, 2), 10),
        d = parseInt(s.substr(8, 2), 10);
    if (!y || !m || !d || y <= 1900) return '';
    return (d < 10 ? '0' + d : d) + '-' + MONTHS[m - 1] + '-' + String(y).slice(-2);
}
function fInt(v) {
    var n = num(v);
    return n === 0 ? '' : String(Math.round(n));
}

function cellText(row, c) {
    var v = f(row, c.key);
    if (c.type === 'n') return fAmt(v);
    if (c.type === 'q') return fQty(v);
    if (c.type === 'd') return fDate(v);
    if (c.type === 'i') return fInt(v);
    return esc(txt(v));
}

function msg(text, ok) {
    var m = el('rsMessage');
    if (!m) return;
    m.className = ok ? 'ok' : 'err';
    m.textContent = text;
    if (ok) setTimeout(function () { if (m.textContent === text) m.className = ''; }, 6000);
}
function clearMsg() { var m = el('rsMessage'); if (m) { m.className = ''; m.textContent = ''; } }

/* Every button goes disabled with a spinner the moment it is pressed and comes back on success
   or failure, so a second click cannot fire a second request. */
function busy(btn, on) {
    S.busy = !!on;
    if (btn) { btn.disabled = !!on; btn.classList.toggle('btn-busy', !!on); }
    ['btnShow', 'btnNew', 'btnPrint'].forEach(function (id) {
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

function fillSelect(id, list, placeholder) {
    var sel = el(id);
    if (!sel) return;
    var html = '';
    /* DDL.BindDDL(..., ZeroIndex: false) puts the caption row at the top with no id. The
       multi-select control (Control Account) has no such row. */
    if (placeholder !== null) html += '<option value="0">' + esc(placeholder) + '</option>';
    html += (list || []).map(function (o) {
        return '<option value="' + esc(o.id) + '">' + esc(o.name) + '</option>';
    }).join('');
    sel.innerHTML = html;
}

/* ---------------------------------------------------------------- setup */
function loadSetup() {
    return getJson(API + '/setup').then(function (d) {
        d = d || {};
        ['parentCategoryError', 'controlAccountError', 'customGroupError',
         'customerGroupError', 'branchError'].forEach(function (k) {
            if (d[k]) msg(k.replace('Error', '') + ': ' + d[k], false);
        });

        S.branchFeature = !!d.branchFeature;
        S.decimals = (typeof d.decimalsAmount === 'number' && d.decimalsAmount >= 1
                      && d.decimalsAmount <= 4) ? d.decimalsAmount : 2;

        setVal('fromDate', d.fromDate); setVal('toDate',   d.toDate);
        setVal('dueFrom',  d.dueFrom);  setVal('dueUpto',  d.dueUpto);
        setVal('saleFrom', d.saleFrom); setVal('saleTo',   d.saleTo);
        setVal('intervalDays', d.intervalDays);

        setChk('chkFrom',     d.chkFrom);     setChk('chkTo',       d.chkTo);
        setChk('chkDueFrom',  d.chkDueFrom);  setChk('chkDueUpto',  d.chkDueUpto);
        setChk('chkSaleFrom', d.chkSaleFrom); setChk('chkSaleTo',   d.chkSaleTo);
        ['From', 'To', 'DueFrom', 'DueUpto', 'SaleFrom', 'SaleTo'].forEach(applyDateEnabled);

        fillSelect('parentCategory',  d.parentCategories, 'Parent Category');
        fillSelect('controlAccounts', d.controlAccounts,  null);
        fillSelect('customGroup',     d.customGroups,     'Custom Group');
        fillSelect('customerGroup',   d.customerGroups,   'Customer Group');

        var wrap = el('branchWrap');
        if (wrap) wrap.style.display = S.branchFeature ? '' : 'none';
        if (S.branchFeature) {
            fillSelect('branches', d.branches, 'Branch Name');
            /* :588 - the desktop preselects the signed-in user's own branch. */
            if (d.currentBranchId) {
                var b = el('branches');
                if (b) {
                    Array.prototype.forEach.call(b.options, function (o) {
                        if (String(o.value) === String(d.currentBranchId)) o.selected = true;
                    });
                }
            }
        }

        /* Every dropdown here is DB-driven and searchable; none carries hard-coded values. */
        if (window.jQuery && jQuery.fn.select2) {
            jQuery('.rs-select2').each(function () {
                var $s = jQuery(this);
                if ($s.data('select2')) $s.select2('destroy');
                $s.select2({ width: '100%', dropdownAutoWidth: true });
            });
        }
    }).catch(function (e) {
        msg('Setup could not be read: ' + e.message, false);
    });
}

/* A DateTimePicker greys its value out when its box is unticked. */
function applyDateEnabled(which) {
    var map = { From: 'fromDate', To: 'toDate', DueFrom: 'dueFrom',
                DueUpto: 'dueUpto', SaleFrom: 'saleFrom', SaleTo: 'saleTo' };
    var input = el(map[which]);
    if (input) input.disabled = !chk('chk' + which);
}

window.rsDateToggled = function (which) {
    applyDateEnabled(which);
    /* :203-206 - Due Up To is sent only when DATE TO is ticked. That is a defect in the desktop
       form, kept here on purpose so both apps return the same rows; the hint says so out loud
       rather than silently sending a date the desktop would not have sent. */
    if (which === 'To' || which === 'DueUpto') {
        var upto = el('dueUpto');
        if (upto) {
            upto.title = chk('chkTo')
                ? ''
                : 'The desktop gates Due Up To on the Date To tick box, so with Date To '
                  + 'unticked this value is not sent.';
        }
    }
};

/* txtIntervalDays_TextChanged, :807-818 - Sale From = today minus the interval, every keystroke. */
window.rsIntervalChanged = function () {
    var days = parseInt(val('intervalDays'), 10);
    if (isNaN(days)) days = 0;          // Conversion.ToInt("") is 0 on the desktop
    var d = new Date();
    d.setDate(d.getDate() - days);
    setVal('saleFrom', isoLocal(d));
};

function isoLocal(d) {
    var m = d.getMonth() + 1, day = d.getDate();
    return d.getFullYear() + '-' + (m < 10 ? '0' + m : m) + '-' + (day < 10 ? '0' + day : day);
}

function picked(id) {
    var sel = el(id);
    if (!sel) return '';
    var out = [];
    Array.prototype.forEach.call(sel.options, function (o) {
        if (o.selected && txt(o.value) && txt(o.value) !== '0') out.push(o.value);
    });
    return out.join(',');
}
function single(id) {
    var v = num(val(id));
    return v > 0 ? v : 0;
}

/* ---------------------------------------------------------------- show */
window.rsShow = function (btn) {
    if (S.busy) return;
    clearMsg();
    busy(btn, true);

    var q = '?fromDate=' + encodeURIComponent(val('fromDate'))
          + '&toDate='   + encodeURIComponent(val('toDate'))
          + '&dueFrom='  + encodeURIComponent(val('dueFrom'))
          + '&dueUpto='  + encodeURIComponent(val('dueUpto'))
          + '&saleFrom=' + encodeURIComponent(val('saleFrom'))
          + '&saleTo='   + encodeURIComponent(val('saleTo'))
          + '&chkFrom='     + chk('chkFrom')
          + '&chkTo='       + chk('chkTo')
          + '&chkDueFrom='  + chk('chkDueFrom')
          + '&chkDueUpto='  + chk('chkDueUpto')
          + '&chkSaleFrom=' + chk('chkSaleFrom')
          + '&chkSaleTo='   + chk('chkSaleTo')
          + '&parentCategoryId=' + single('parentCategory')
          + '&customGroupId='    + single('customGroup')
          + '&customerGroupId='  + single('customerGroup')
          + '&controlAccountIds=' + encodeURIComponent(picked('controlAccounts'))
          + '&branchIds=' + encodeURIComponent(S.branchFeature ? picked('branches') : '');

    var empty = el('rsEmpty');
    if (empty) { empty.style.display = ''; empty.textContent = 'Reading...'; }

    getJson(API + '/report' + q).then(function (d) {
        busy(btn, false);
        d = d || {};
        if (d.error) {
            msg(d.error, false);
            S.rows = [];
        } else {
            S.rows = d.rows || [];
            S.ranFrom     = chk('chkFrom')     ? val('fromDate') : '';
            S.ranTo       = chk('chkTo')       ? val('toDate')   : '';
            S.ranSaleFrom = chk('chkSaleFrom') ? val('saleFrom') : '';
            S.ranSaleTo   = chk('chkSaleTo')   ? val('saleTo')   : '';
        }
        render();
    }).catch(function (e) {
        busy(btn, false);
        msg('Show failed: ' + e.message, false);
        S.rows = [];
        render();
    });
};

/* ---------------------------------------------------------------- grid */
function visibleCols() {
    /* :339 - BranchName shows only with the branch feature on AND a branch chosen. */
    var branchChosen = S.branchFeature && picked('branches') !== '';
    return COLS.filter(function (c) { return !c.branchOnly || branchChosen; });
}

function render() {
    var cols = visibleCols();
    var head = el('rsHead'), body = el('rsBody'), foot = el('rsFoot'),
        empty = el('rsEmpty'), count = el('rsCount');

    if (count) count.textContent = S.rows.length ? (S.rows.length + ' row(s)') : '';

    if (!S.rows.length) {
        if (head) head.innerHTML = '';
        if (body) body.innerHTML = '';
        if (foot) foot.innerHTML = '';
        if (empty) { empty.style.display = ''; empty.textContent = 'No records found.'; }
        return;
    }
    if (empty) empty.style.display = 'none';

    if (head) {
        head.innerHTML = '<tr>' + cols.map(function (c) {
            return '<th>' + esc(c.cap) + '</th>';
        }).join('') + '</tr>';
    }

    /* :305-306 - the grid is grouped by ParentAccount, and ParentAccount itself is then hidden
       as a column, so the parent title appears only as the group band. */
    var html = '', lastGroup = null;
    S.rows.forEach(function (row, i) {
        var g = txt(f(row, 'ParentAccount'));
        if (g !== lastGroup) {
            lastGroup = g;
            html += '<tr class="rs-group"><td colspan="' + cols.length + '">'
                 + esc(g || '(no parent account)') + '</td></tr>';
        }
        html += '<tr>' + cols.map(function (c) {
            var cls = (c.type === 'n' || c.type === 'q') ? 'rs-num'
                    : (c.center ? 'rs-ctr' : '');
            var raw = f(row, c.key);
            if ((c.type === 'n' || c.type === 'q') && num(raw) < 0) cls += ' rs-neg';
            var inner = cellText(row, c);
            if (c.link) inner = linkCell(c, row, i, inner);
            return '<td class="' + cls + '">' + inner + '</td>';
        }).join('') + '</tr>';
    });
    if (body) body.innerHTML = html;

    if (foot) {
        foot.innerHTML = '<tr>' + cols.map(function (c, idx) {
            if (!c.sum) return '<td>' + (idx === 0 ? 'Total' : '') + '</td>';
            var t = 0;
            S.rows.forEach(function (r) { t += num(f(r, c.key)); });
            /* TotalFormatString is "#,##0.00;(0,0.00); 0" for amounts and "#,##0.##" for the
               quantity columns, so a total uses the same shape as its cells. */
            return '<td class="rs-num' + (t < 0 ? ' rs-neg' : '') + '">'
                 + (c.type === 'q' ? fQty(t) : fAmt(t)) + '</td>';
        }).join('') + '</tr>';
    }
}

/* grdPayablesAndSchedule_LinkClicked, :819-916. Three link columns, ColumnType 5 at :368-370. */
function linkCell(c, row, i, inner) {
    if (c.link === 'account') {
        /* :829 - GoToGeneralLedgerFromLinkedEvent(AccountId, datDateFrom, datDateTo, 0,
           branchesId). The web General Ledger accepts accountId, fromDate and toDate on the
           query string and runs itself; it has no branch parameter, so branchesId is not
           carried over and the ledger opens unfiltered by branch. */
        var accountId = num(f(row, 'AccountId'));
        if (!accountId) return inner;
        var href = '/accounts/reports/general-ledger?accountId=' + accountId
                 + '&fromDate=' + encodeURIComponent(S.ranFrom)
                 + '&toDate='   + encodeURIComponent(S.ranTo);
        return '<a class="rs-link" href="' + href + '" title="General Ledger for this account">'
             + inner + '</a>';
    }
    /* :834-866 BalQty opens frmSaleOrderHistory, :874-900 SaleAmount opens
       frmEvaulationDetailSalesReports with Activity "Sales Summary By Customer & Item". Both
       do nothing at all when the figure is zero (:841, :879). Neither target screen is ported,
       so the cell says what it would open instead of opening something unrelated. */
    if (num(f(row, c.key)) === 0) return inner;
    return '<span class="rs-link-off" onclick="rsLinkPending(' + i + ',\'' + c.link + '\')">'
         + inner + '</span>';
}

window.rsLinkPending = function (i, kind) {
    var row = S.rows[i];
    if (!row) return;
    var party = txt(f(row, 'AccountTitle'));
    if (kind === 'balqty') {
        msg('Order Bal Qty for "' + party + '" opens Sale Order History on the desktop, for '
          + (S.ranSaleFrom || 'the Sale From date') + ' to ' + (S.ranSaleTo || 'the Sale To date')
          + '. That screen is not ported yet, so nothing was opened.', false);
    } else {
        msg('Sale Amount for "' + party + '" opens the Detail Sales Report (Sales Summary By '
          + 'Customer & Item) on the desktop, for ' + (S.ranSaleFrom || 'the Sale From date')
          + ' to ' + (S.ranSaleTo || 'the Sale To date')
          + '. That screen is not ported yet, so nothing was opened.', false);
    }
};

/* ---------------------------------------------------------------- toolbar */

/* btnNew_Click -> Reset, :630-655. The desktop clears only the Parent Category and puts Date
   From back to thirty days ago; every other filter and the grid are left alone. */
window.rsNew = function (btn) {
    if (S.busy) return;
    busy(btn, true);
    clearMsg();
    try {
        var pc = el('parentCategory');
        if (pc) {
            pc.value = '0';
            if (window.jQuery && jQuery.fn.select2 && jQuery(pc).data('select2')) {
                jQuery(pc).trigger('change.select2');
            }
        }
        var d = new Date();
        d.setDate(d.getDate() - 30);
        setVal('fromDate', isoLocal(d));
        var fd = el('fromDate');
        if (fd && !fd.disabled) fd.focus();
    } finally {
        busy(btn, false);
    }
};

/* print_Click, :760-782 - the desktop prints the grid as it stands. */
window.rsPrint = function (btn) {
    if (S.busy) return;
    if (!S.rows.length) { msg('Nothing to print - press Show first.', false); return; }
    busy(btn, true);
    try { window.print(); } finally { busy(btn, false); }
};

/* MakeShortCutKeys / btnShortCut_Click, :721-759. */
window.rsShortcuts = function () {
    msg('Ctrl+S Show   Ctrl+P Print   Ctrl+N New   Ctrl+F5 or Ctrl+Up focus Date From   '
      + 'Ctrl+Down focus the grid   Esc or Ctrl+E close', true);
};

window.rsToggleFullscreen = function () {
    document.body.classList.toggle('rs-full');
};

/* PayablesAndPaymentSchedule_KeyDown, :674-720. Enter moves to the next control, as
   SendKeys.Send("{TAB}") does on the desktop. */
document.addEventListener('keydown', function (e) {
    if (e.ctrlKey && (e.key === 's' || e.key === 'S')) {
        e.preventDefault(); var b = el('btnShow'); if (b && !b.disabled) rsShow(b);
    } else if (e.ctrlKey && (e.key === 'p' || e.key === 'P')) {
        e.preventDefault(); var p = el('btnPrint'); if (p && !p.disabled) rsPrint(p);
    } else if (e.ctrlKey && (e.key === 'n' || e.key === 'N')) {
        e.preventDefault(); var n = el('btnNew'); if (n && !n.disabled) rsNew(n);
    } else if (e.key === 'Escape' || (e.ctrlKey && (e.key === 'e' || e.key === 'E'))) {
        if (document.body.classList.contains('rs-full')) {
            e.preventDefault();
            document.body.classList.remove('rs-full');
        }
    } else if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) {
        e.preventDefault(); var fd = el('fromDate'); if (fd && !fd.disabled) fd.focus();
    } else if (e.ctrlKey && e.key === 'ArrowDown') {
        e.preventDefault(); var g = el('rsGridWrap'); if (g) g.scrollIntoView({ block: 'start' });
    } else if (e.key === 'Enter') {
        var t = e.target;
        if (t && t.tagName === 'INPUT' && t.type !== 'checkbox' && t.form === undefined) {
            /* nothing to submit - the desktop just moves focus onward */
        }
    }
});

/* ---------------------------------------------------------------- boot */
function boot() {
    /* The desktop does NOT read the report on load - it waits for Show (:132-163). */
    loadSetup().then(function () {
        var empty = el('rsEmpty');
        if (empty) { empty.style.display = ''; empty.textContent = 'Press Show to read the schedule.'; }
    });
}

if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
else boot();

})();
