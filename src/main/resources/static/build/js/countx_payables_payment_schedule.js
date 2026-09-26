/* ============================================================================
 * Payables And Payment Schedule
 * Ported from Architecture.WinApp.Account_Reports\PayablesAndPaymentSchedule.cs (1,864 lines).
 *
 *   :146-179  PayablesAndPaymentSchedule_Load
 *   :182-197  btnshow_Click
 *   :199-355  gridHistory                  detail rows plus the aging summary
 *   :357-436  GridSetting                  via GridEX_Helper.GridWrappingAndColumnSettings
 *   :624-649  btnNew_Click -> Reset
 *   :668-714  PayablesAndPaymentSchedule_KeyDown
 *   :754-776  print_Click
 *   :801-812  txtIntervalDays_TextChanged
 *   :813-897  grdPayablesAndSchedule_LinkClicked
 *
 * Dates are plain yyyy-mm-dd strings throughout - never toISOString(), which shifts a day at
 * UTC+5.
 * ==========================================================================*/
(function () {
'use strict';

var API = '/api/dashboard/payables-schedule';

var S = {
    rows: [], summary: [], buckets: [],
    busy: false, branchFeature: false, decimals: 2,
    ranFrom: '', ranTo: '', ranPurFrom: '', ranPurTo: ''
};

/* GridSetting hides these (:361-364):
   BranchesId, AccountClass, AccountTypeId, AccountId, ParentAccount, LastBillDate,
   LastBillsAmount, BillDays, ReceivedQty, OrderWeight, PurchaseWeight, ReceivedWeight,
   BalWeight, FromDate, ToDate, DueDateFrom, DueDateTo, PurchaseFromDate, PurchaseToDate,
   OrderQty.

   Formats come from GridEX_Helper.GridColumnSettings, which picks by column-name substring, and
   then GridSetting overrides seven of them to stringFormatboth (:392-398):

     Opening, CurrDebit, CurrCredit, Closing, DueBalance, NotYetDue, Short/Excess
                                          -> stringFormatboth  "#,##0.NN;(0,0.NN); 0"
     LastPaymentAmount, PurchaseAmount    -> "Amount" rule, stringFormatsingle "#,##0.NN"
     PaymentDays                          -> the "Payment" rule, "#,#;(#,#.##);0"
     PayToday, PurchaseQty, BalQty        -> the fall-through rule, "#,##0.##"

   Every numeric column is summed - nothing is passed in ExcludeColumnsFromTotals.

   type: s text, b stringFormatboth, a stringFormatsingle, p "#,#;(#,#.##);0",
         q "#,##0.##", d dd-MMM-yy                                                           */
var COLS = [
    { key: 'BranchName',        cap: 'Branch',                type: 's', branchOnly: true },
    { key: 'AccountCode',       cap: 'Account Code',          type: 's', link: 'account' },
    { key: 'AccountTitle',      cap: 'Account Title / Party', type: 's' },
    { key: 'AccountType',       cap: 'Account Type',          type: 's' },
    { key: 'Opening',           cap: 'Opening Balance',       type: 'b', sum: true },
    { key: 'CurrDebit',         cap: 'Debit',                 type: 'b', sum: true },
    { key: 'CurrCredit',        cap: 'Credit',                type: 'b', sum: true },
    { key: 'Closing',           cap: 'Closing Balance',       type: 'b', sum: true },
    { key: 'DueBalance',        cap: 'DueBalance',            type: 'b', sum: true },
    { key: 'NotYetDue',         cap: 'NotYetDue',             type: 'b', sum: true },
    { key: 'PayToday',          cap: 'PayToday',              type: 'q', sum: true, edit: true },
    { key: 'Short/Excess',      cap: 'Short/Excess',          type: 'b', sum: true },
    { key: 'Increase/Decrease', cap: 'Increase/Decrease',     type: 's', center: true },
    { key: 'LastPaymentDate',   cap: 'LastPaymentDate',       type: 'd' },
    { key: 'LastPaymentAmount', cap: 'LastPaymentAmount',     type: 'a', sum: true },
    { key: 'PaymentDays',       cap: 'PaymentDays',           type: 'p', sum: true },
    { key: 'PurchaseQty',       cap: 'PurchaseQty',           type: 'q', sum: true },
    { key: 'PurchaseAmount',    cap: 'PurchaseAmount',        type: 'a', sum: true, link: 'purchase' },
    { key: 'BalQty',            cap: 'Order Bal Qty',         type: 'q', sum: true, link: 'balqty' }
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

/* stringFormatboth "#,##0.NN;(0,0.NN); 0" - negatives in parentheses, a bare 0 at zero. */
function fBoth(v) {
    var n = num(v);
    if (n === 0) return '0';
    var a = Math.abs(n).toLocaleString(undefined,
            { minimumFractionDigits: S.decimals, maximumFractionDigits: S.decimals });
    return n < 0 ? '(' + a + ')' : a;
}
/* stringFormatsingle "#,##0.NN" - a minus sign, no parentheses. */
function fSingle(v) {
    return num(v).toLocaleString(undefined,
            { minimumFractionDigits: S.decimals, maximumFractionDigits: S.decimals });
}
/* "#,#;(#,#.##);0" - whole numbers, parentheses for negatives, a bare 0 at zero. */
function fPay(v) {
    var n = num(v);
    if (n === 0) return '0';
    var a = Math.abs(n).toLocaleString(undefined, { maximumFractionDigits: 0 });
    return n < 0 ? '(' + a + ')' : a;
}
/* "#,##0.##" - up to two decimals, none forced. */
function fQty(v) {
    return num(v).toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 2 });
}
function fDate(v) {
    var s = txt(v);
    if (!s) return '';
    var y = parseInt(s.substr(0, 4), 10),
        m = parseInt(s.substr(5, 2), 10),
        d = parseInt(s.substr(8, 2), 10);
    if (!y || !m || !d || y <= 1900) return '';
    return (d < 10 ? '0' + d : d) + '-' + MONTHS[m - 1] + '-' + String(y).slice(-2);
}

function fmt(v, type) {
    if (type === 'b') return fBoth(v);
    if (type === 'a') return fSingle(v);
    if (type === 'p') return fPay(v);
    if (type === 'q') return fQty(v);
    if (type === 'd') return fDate(v);
    return esc(txt(v));
}
function isNumeric(type) { return type === 'b' || type === 'a' || type === 'p' || type === 'q'; }

function msg(text, ok) {
    var m = el('psMessage');
    if (!m) return;
    m.className = ok ? 'ok' : 'err';
    m.textContent = text;
    if (ok) setTimeout(function () { if (m.textContent === text) m.className = ''; }, 6000);
}
function clearMsg() { var m = el('psMessage'); if (m) { m.className = ''; m.textContent = ''; } }

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

        setVal('fromDate', d.fromDate);         setVal('toDate',     d.toDate);
        setVal('dueFrom',  d.dueFrom);          setVal('dueUpto',    d.dueUpto);
        setVal('purchaseFrom', d.purchaseFrom); setVal('purchaseTo', d.purchaseTo);
        setVal('intervalDays', d.intervalDays); setVal('agingDays',  d.agingDays);

        setChk('chkFrom',        d.chkFrom);        setChk('chkTo',         d.chkTo);
        setChk('chkDueFrom',     d.chkDueFrom);     setChk('chkDueUpto',    d.chkDueUpto);
        setChk('chkPurchaseFrom', d.chkPurchaseFrom); setChk('chkPurchaseTo', d.chkPurchaseTo);
        ['From', 'To', 'DueFrom', 'DueUpto', 'PurchaseFrom', 'PurchaseTo'].forEach(applyDateEnabled);

        fillSelect('parentCategory',  d.parentCategories, 'Parent Category');
        fillSelect('controlAccounts', d.controlAccounts,  null);
        fillSelect('customGroup',     d.customGroups,     'Custom Group');
        fillSelect('customerGroup',   d.customerGroups,   'Customer Group');

        var wrap = el('branchWrap');
        if (wrap) wrap.style.display = S.branchFeature ? '' : 'none';
        if (S.branchFeature) {
            fillSelect('branches', d.branches, 'Branch Name');
            if (d.currentBranchId) {
                var b = el('branches');
                if (b) {
                    Array.prototype.forEach.call(b.options, function (o) {
                        if (String(o.value) === String(d.currentBranchId)) o.selected = true;
                    });
                }
            }
        }

        /* Every dropdown is DB-driven and searchable; none carries hard-coded values. */
        if (window.jQuery && jQuery.fn.select2) {
            jQuery('.ps-select2').each(function () {
                var $s = jQuery(this);
                if ($s.data('select2')) $s.select2('destroy');
                $s.select2({ width: '100%', dropdownAutoWidth: true });
            });
        }
    }).catch(function (e) {
        msg('Setup could not be read: ' + e.message, false);
    });
}

function applyDateEnabled(which) {
    var map = { From: 'fromDate', To: 'toDate', DueFrom: 'dueFrom', DueUpto: 'dueUpto',
                PurchaseFrom: 'purchaseFrom', PurchaseTo: 'purchaseTo' };
    var input = el(map[which]);
    if (input) input.disabled = !chk('chk' + which);
}

window.psDateToggled = function (which) {
    applyDateEnabled(which);
    /* :222-225 - Due Up To is gated on the DATE TO tick box on the desktop. Kept, and said out
       loud on the control rather than silently sending a date the desktop would not send. */
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

/* txtIntervalDays_TextChanged, :801-812 - Purchase From = today minus the interval. */
window.psIntervalChanged = function () {
    var days = parseInt(val('intervalDays'), 10);
    if (isNaN(days)) days = 0;
    var d = new Date();
    d.setDate(d.getDate() - days);
    setVal('purchaseFrom', isoLocal(d));
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
window.psShow = function (btn) {
    if (S.busy) return;
    clearMsg();
    busy(btn, true);

    var q = '?fromDate=' + encodeURIComponent(val('fromDate'))
          + '&toDate='       + encodeURIComponent(val('toDate'))
          + '&dueFrom='      + encodeURIComponent(val('dueFrom'))
          + '&dueUpto='      + encodeURIComponent(val('dueUpto'))
          + '&purchaseFrom=' + encodeURIComponent(val('purchaseFrom'))
          + '&purchaseTo='   + encodeURIComponent(val('purchaseTo'))
          + '&chkFrom='         + chk('chkFrom')
          + '&chkTo='           + chk('chkTo')
          + '&chkDueFrom='      + chk('chkDueFrom')
          + '&chkDueUpto='      + chk('chkDueUpto')
          + '&chkPurchaseFrom=' + chk('chkPurchaseFrom')
          + '&chkPurchaseTo='   + chk('chkPurchaseTo')
          + '&parentCategoryId=' + single('parentCategory')
          + '&customGroupId='    + single('customGroup')
          + '&customerGroupId='  + single('customerGroup')
          + '&agingDays='        + (parseInt(val('agingDays'), 10) || 0)
          + '&controlAccountIds=' + encodeURIComponent(picked('controlAccounts'))
          + '&branchIds=' + encodeURIComponent(S.branchFeature ? picked('branches') : '');

    var empty = el('psEmpty');
    if (empty) { empty.style.display = ''; empty.textContent = 'Reading...'; }

    getJson(API + '/report' + q).then(function (d) {
        busy(btn, false);
        d = d || {};
        if (d.error) {
            msg(d.error, false);
            S.rows = []; S.summary = []; S.buckets = [];
        } else {
            S.rows    = d.rows || [];
            S.summary = d.summary || [];
            S.buckets = d.summaryBuckets || [];
            S.ranFrom    = chk('chkFrom')         ? val('fromDate')     : '';
            S.ranTo      = chk('chkTo')           ? val('toDate')       : '';
            S.ranPurFrom = chk('chkPurchaseFrom') ? val('purchaseFrom') : '';
            S.ranPurTo   = chk('chkPurchaseTo')   ? val('purchaseTo')   : '';
            /* :313 - the desktop says so outright when the detail set comes back empty. */
            if (!S.rows.length) msg('No Record Found For Display', false);
        }
        render();
        renderSummary();
    }).catch(function (e) {
        busy(btn, false);
        msg('Show failed: ' + e.message, false);
        S.rows = []; S.summary = []; S.buckets = [];
        render();
        renderSummary();
    });
};

/* ---------------------------------------------------------------- detail grid */
function visibleCols() {
    /* :371 - BranchName shows only with the branch feature on AND a branch chosen. */
    var branchChosen = S.branchFeature && picked('branches') !== '';
    return COLS.filter(function (c) { return !c.branchOnly || branchChosen; });
}

function render() {
    var cols = visibleCols();
    var head = el('psHead'), body = el('psBody'), foot = el('psFoot'),
        empty = el('psEmpty'), count = el('psCount');

    if (count) count.textContent = S.rows.length ? (S.rows.length + ' row(s)') : '';

    if (!S.rows.length) {
        if (head) head.innerHTML = '';
        if (body) body.innerHTML = '';
        if (foot) foot.innerHTML = '';
        if (empty) { empty.style.display = ''; empty.textContent = 'No Record Found For Display'; }
        return;
    }
    if (empty) empty.style.display = 'none';

    if (head) {
        head.innerHTML = '<tr>' + cols.map(function (c) {
            return '<th>' + esc(c.cap) + '</th>';
        }).join('') + '</tr>';
    }

    /* :372-374 - grouped by ParentAccount, which is then hidden as a column. */
    var html = '', lastGroup = null;
    S.rows.forEach(function (row, i) {
        var g = txt(f(row, 'ParentAccount'));
        if (g !== lastGroup) {
            lastGroup = g;
            html += '<tr class="ps-group"><td colspan="' + cols.length + '">'
                 + esc(g || '(no parent account)') + '</td></tr>';
        }
        html += '<tr>' + cols.map(function (c) {
            var raw = f(row, c.key);
            var cls = isNumeric(c.type) ? 'ps-num' : (c.center ? 'ps-ctr' : '');
            if (isNumeric(c.type) && num(raw) < 0) cls += ' ps-neg';
            var inner;
            if (c.edit) {
                /* PayToday, EditType 1 on the desktop. Nothing writes it back to the database
                   there, so this only re-totals the column. */
                inner = '<input type="number" step="any" class="ps-edit" value="'
                      + esc(num(raw)) + '" onchange="psPayTodayChanged(' + i + ', this.value)">';
            } else {
                inner = fmt(raw, c.type);
                if (c.link) inner = linkCell(c, row, i, inner);
            }
            return '<td class="' + cls + '">' + inner + '</td>';
        }).join('') + '</tr>';
    });
    if (body) body.innerHTML = html;
    renderFoot(cols);
}

function renderFoot(cols) {
    var foot = el('psFoot');
    if (!foot) return;
    foot.innerHTML = '<tr>' + cols.map(function (c, idx) {
        if (!c.sum) return '<td>' + (idx === 0 ? 'Total' : '') + '</td>';
        var t = 0;
        S.rows.forEach(function (r) { t += num(f(r, c.key)); });
        return '<td class="ps-num' + (t < 0 ? ' ps-neg' : '') + '">' + fmt(t, c.type) + '</td>';
    }).join('') + '</tr>';
}

window.psPayTodayChanged = function (i, v) {
    var row = S.rows[i];
    if (!row) return;
    row['PayToday'] = num(v);
    renderFoot(visibleCols());
};

/* grdPayablesAndSchedule_LinkClicked, :813-897. Three link columns, ColumnType 5 at :433-435. */
function linkCell(c, row, i, inner) {
    if (c.link === 'account') {
        /* :831 - GoToGeneralLedgerFromLinkedEvent(AccountId, datDateFrom, datDateTo, 0,
           branchesId). The web General Ledger takes accountId, fromDate and toDate on the query
           string and runs itself; it has no branch parameter, so branchesId is not carried. */
        var accountId = num(f(row, 'AccountId'));
        if (!accountId) return inner;
        var href = '/accounts/reports/general-ledger?accountId=' + accountId
                 + '&fromDate=' + encodeURIComponent(S.ranFrom)
                 + '&toDate='   + encodeURIComponent(S.ranTo);
        return '<a class="ps-link" href="' + href + '" title="General Ledger for this account">'
             + inner + '</a>';
    }
    /* :834-863 BalQty opens PurchaseOrderHistory, :869-890 PurchaseAmount opens
       PurchaseRegisterNew. Both do nothing when the figure is zero (:841, :875). Neither target
       screen is ported, so the cell says what it would open. */
    if (num(f(row, c.key)) === 0) return inner;
    return '<span class="ps-link-off" onclick="psLinkPending(' + i + ',\'' + c.link + '\')">'
         + inner + '</span>';
}

window.psLinkPending = function (i, kind) {
    var row = S.rows[i];
    if (!row) return;
    var party = txt(f(row, 'AccountTitle'));
    var range = (S.ranPurFrom || 'the Purchase From date') + ' to '
              + (S.ranPurTo || 'the Purchase To date');
    if (kind === 'balqty') {
        msg('Order Bal Qty for "' + party + '" opens Purchase Order History on the desktop, for '
          + range + ', with all approved and unapproved and all open, completed and cancelled '
          + 'detail ticked. That screen is not ported yet, so nothing was opened.', false);
    } else {
        msg('Purchase Amount for "' + party + '" opens the Purchase Register on the desktop, for '
          + range + '. That screen is not ported yet, so nothing was opened.', false);
    }
};

/* ---------------------------------------------------------------- aging summary */
function renderSummary() {
    var sect = el('psSumSect'), wrap = el('psSumWrap'),
        head = el('psSumHead'), body = el('psSumBody'), foot = el('psSumFoot');

    var show = S.summary.length > 0;
    if (sect) sect.style.display = show ? '' : 'none';
    if (wrap) wrap.style.display = show ? '' : 'none';
    if (!show) {
        if (head) head.innerHTML = '';
        if (body) body.innerHTML = '';
        if (foot) foot.innerHTML = '';
        return;
    }

    /* :322-327 - the four bucket headings are DATA, taken from the first summary row. */
    var b = S.buckets || [];
    if (head) {
        head.innerHTML = '<tr><th style="text-align:left;">Description</th><th>Amount</th>'
                       + '<th>' + esc(b[0]) + '</th><th>' + esc(b[1]) + '</th>'
                       + '<th>' + esc(b[2]) + '</th><th>' + esc(b[3]) + '</th></tr>';
    }
    if (body) {
        body.innerHTML = S.summary.map(function (r) {
            return '<tr><td>' + esc(r.description) + '</td>'
                 + '<td class="ps-num">' + fBoth(r.amount) + '</td>'
                 + '<td class="ps-num">' + fBoth(r.value1) + '</td>'
                 + '<td class="ps-num">' + fBoth(r.value2) + '</td>'
                 + '<td class="ps-num">' + fBoth(r.value3) + '</td>'
                 + '<td class="ps-num">' + fBoth(r.value4) + '</td></tr>';
        }).join('');
    }
    /* grdSummary.TotalRow = 2 (:349) - the summary grid carries its own total row. */
    if (foot) {
        var keys = ['amount', 'value1', 'value2', 'value3', 'value4'];
        var totals = keys.map(function (k) {
            var t = 0;
            S.summary.forEach(function (r) { t += num(r[k]); });
            return t;
        });
        foot.innerHTML = '<tr><td>Total</td>' + totals.map(function (t) {
            return '<td class="ps-num' + (t < 0 ? ' ps-neg' : '') + '">' + fBoth(t) + '</td>';
        }).join('') + '</tr>';
    }
}

/* ---------------------------------------------------------------- toolbar */

/* btnNew_Click -> Reset, :624-649 - only Parent Category and Date From; nothing else changes. */
window.psNew = function (btn) {
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

/* print_Click, :754-776. */
window.psPrint = function (btn) {
    if (S.busy) return;
    if (!S.rows.length) { msg('Nothing to print - press Show first.', false); return; }
    busy(btn, true);
    try { window.print(); } finally { busy(btn, false); }
};

window.psShortcuts = function () {
    msg('Ctrl+S Show   Ctrl+P Print   Ctrl+N New   Ctrl+F5 or Ctrl+Up focus Date From   '
      + 'Ctrl+Down focus the grid   Esc or Ctrl+E close', true);
};

window.psToggleFullscreen = function () {
    document.body.classList.toggle('ps-full');
};

/* PayablesAndPaymentSchedule_KeyDown, :668-714. */
document.addEventListener('keydown', function (e) {
    if (e.ctrlKey && (e.key === 's' || e.key === 'S')) {
        e.preventDefault(); var b = el('btnShow'); if (b && !b.disabled) psShow(b);
    } else if (e.ctrlKey && (e.key === 'p' || e.key === 'P')) {
        e.preventDefault(); var p = el('btnPrint'); if (p && !p.disabled) psPrint(p);
    } else if (e.ctrlKey && (e.key === 'n' || e.key === 'N')) {
        e.preventDefault(); var n = el('btnNew'); if (n && !n.disabled) psNew(n);
    } else if (e.key === 'Escape' || (e.ctrlKey && (e.key === 'e' || e.key === 'E'))) {
        if (document.body.classList.contains('ps-full')) {
            e.preventDefault();
            document.body.classList.remove('ps-full');
        }
    } else if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) {
        e.preventDefault(); var fd = el('fromDate'); if (fd && !fd.disabled) fd.focus();
    } else if (e.ctrlKey && e.key === 'ArrowDown') {
        e.preventDefault(); var g = el('psGridWrap'); if (g) g.scrollIntoView({ block: 'start' });
    }
});

/* ---------------------------------------------------------------- boot */
function boot() {
    /* The desktop does NOT read the report on load - it waits for Show (:146-179). */
    loadSetup().then(function () {
        var empty = el('psEmpty');
        if (empty) { empty.style.display = ''; empty.textContent = 'Press Show to read the schedule.'; }
    });
}

if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
else boot();

})();
