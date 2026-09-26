/* ============================================================================
 * Stock Breakup In & Out Summary  -  the whole Audit Dashboard module.
 * Ported from Architecture.WinApp.Audit_Dashboard\StockBreakupInAndOutSummary.cs (625 lines).
 *
 *   :78-105    Reset() / Load()          From = financial year start but UNTICKED, To = today
 *   :107-155   GridFill()                usp_TotalStockBreakup_InAndOut_Summary
 *   :158-203   gridSetting()             hidden columns, grouping, formats, Select checkboxes
 *   :205-215   btnSearch_Click
 *   :259-282   grdfrm_LinkClicked        the Activity link, only when SortNo != 7
 *   :307-366   btnGenerate_Click         the shortage row, recalculated in memory only
 *   :368-378   txtShortPercent_KeyPress  digits and one decimal point
 * ==========================================================================*/
(function () {
'use strict';

var API = '/api/audit/stock-breakup';

var S = {
    rows: [],
    checked: {},      // row index -> true, from the Select column
    busy: false
};

/* ---------------------------------------------------------------- helpers */
function el(id)  { return document.getElementById(id); }
function val(id) { var e = el(id); return e ? e.value : ''; }
function setVal(id, v) { var e = el(id); if (e) e.value = v === null || v === undefined ? '' : v; }
function txt(v)  { return v === null || v === undefined ? '' : String(v); }
function esc(v)  { return txt(v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
                                .replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
function num(v)  { var n = parseFloat(v); return isNaN(n) ? 0 : n; }

/* Janus "#,#;(#,#);0" - thousands, no decimals, NEGATIVES IN PARENTHESES, and a bare 0
   for zero. Reproduced exactly, including the parentheses, because the desktop shows
   outgoing quantities that way and a plain minus sign would read differently. */
function fInt(v) {
    var n = num(v);
    if (n === 0) return '0';
    var a = Math.abs(n).toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 0 });
    return n < 0 ? '(' + a + ')' : a;
}
/* Janus "#,##0.##" */
function f2(v) {
    return num(v).toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 2 });
}

function msg(text, ok) {
    var m = el('sbMessage');
    if (!m) return;
    m.className = ok ? 'ok' : 'err';
    m.textContent = text;
    if (ok) setTimeout(function () { if (m.textContent === text) m.className = ''; }, 5000);
}
function clearMsg() { var m = el('sbMessage'); if (m) { m.className = ''; m.textContent = ''; } }

/* Click -> disabled + loader at once, re-enabled either way, second click dropped. */
function busy(btn, on) {
    S.busy = !!on;
    if (btn) { btn.disabled = !!on; btn.classList.toggle('btn-busy', !!on); }
    ['btnShow', 'btnNew', 'btnGenerate'].forEach(function (id) {
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

/* ---------------------------------------------------------------- columns
 * gridSetting(), :158-203. SortNo and ParentCategory are hidden (:160-161); TransType becomes
 * the group and is hidden as a column because HideColumnsWhenGrouped is on (:163-164).
 */
var COLS = [
    { k: 'ClassGroup',      c: 'ClassGroup',  w: 120, fmt: null },
    { k: 'Activity',        c: 'Activity',    w: 150, fmt: null, link: true },
    { k: 'ItemQty',         c: 'ItemQty',     w: 120, fmt: fInt, sum: true },
    { k: 'Weight',          c: 'Weight',      w: 120, fmt: fInt, sum: true },
    { k: 'Amount',          c: 'Amount',      w: 150, fmt: fInt, sum: true },
    { k: 'AvgRate',         c: 'AvgRate',     w: 120, fmt: f2 },
    { k: 'RecoveryPercent', c: 'Recovery%',   w: 100, fmt: f2 }
];

/* ---------------------------------------------------------------- load */
function loadDefaults() {
    return getJson(API + '/defaults').then(function (d) {
        /* :96-97 - From shows the financial year start, To shows today. The From box starts
           UNTICKED (:84), so the report initially runs without a from-date at all. */
        setVal('fromDate', (d && d.fromDate) || '');
        setVal('toDate',   (d && d.toDate) || '');
        var chk = el('chkFrom'); if (chk) chk.checked = false;
        var f = el('fromDate'); if (f) f.disabled = true;
    }).catch(function (e) {
        msg('Defaults could not be read: ' + e.message, false);
    });
}

window.sbFromToggled = function () {
    var chk = el('chkFrom'), f = el('fromDate');
    if (f) f.disabled = !(chk && chk.checked);
};

window.sbRateModeChanged = function () {
    /* Changing the radio only changes what the next Show sends (@ActionId 2 or omitted);
       the desktop does not re-run the report on the change either. */
    clearMsg();
};

/* ---------------------------------------------------------------- show */
window.sbShow = function (btn) {
    if (S.busy) return;
    clearMsg();
    busy(btn, true);

    var chk = el('chkFrom');
    var useFrom = !!(chk && chk.checked);
    var custom = !!(el('rdCustom') && el('rdCustom').checked);

    var q = '?toDate=' + encodeURIComponent(val('toDate'))
          + '&customRate=' + (custom ? 'true' : 'false');
    if (useFrom && val('fromDate')) q += '&fromDate=' + encodeURIComponent(val('fromDate'));

    getJson(API + '/report' + q).then(function (d) {
        busy(btn, false);
        if (d && d.error) { msg(d.error, false); render(); return; }
        S.rows = (d && d.rows) || [];
        S.checked = {};
        render();
        var r = el('sbRange');
        if (r) r.textContent = (useFrom && val('fromDate') ? val('fromDate') : 'opening')
                             + '  to  ' + val('toDate');
    }).catch(function (e) {
        busy(btn, false);
        msg('Report failed: ' + e.message, false);
    });
};

/* ---------------------------------------------------------------- render
 * RootTable.Groups.Add("TransType") with GroupTotals - each TransType gets a header row, its
 * rows, then its own totals; the grid then carries a grand total.
 */
function render() {
    var head = el('sbHead'), body = el('sbBody'), foot = el('sbFoot');
    if (!head || !body) return;

    /* The Select column: ActAsSelector + UseHeaderSelector (:196-200) - a checkbox per row and
       one in the header that toggles them all. */
    head.innerHTML = '<th style="width:30px; text-align:center;">'
                   + '<input type="checkbox" id="sbAll" onclick="sbToggleAll(this)"></th>'
                   + COLS.map(function (c) {
                         return '<th style="width:' + c.w + 'px;'
                              + (c.fmt ? ' text-align:right;' : '') + '">' + esc(c.c) + '</th>';
                     }).join('');

    if (!S.rows.length) {
        body.innerHTML = '<tr><td colspan="' + (COLS.length + 1)
            + '" style="text-align:center; padding:16px; color:#777;">No records.</td></tr>';
        if (foot) foot.innerHTML = '';
        count();
        return;
    }

    var html = '';
    var groupKey = null;
    var gSums = null;

    function groupTotalRow(sums) {
        return '<tr class="grp-total"><td></td>'
             + COLS.map(function (c) {
                   if (!c.sum) return '<td></td>';
                   return '<td class="num' + (sums[c.k] < 0 ? ' neg' : '') + '">'
                        + fInt(sums[c.k]) + '</td>';
               }).join('') + '</tr>';
    }

    S.rows.forEach(function (r, i) {
        var g = txt(r.TransType);
        if (g !== groupKey) {
            if (gSums) html += groupTotalRow(gSums);
            groupKey = g;
            gSums = { ItemQty: 0, Weight: 0, Amount: 0 };
            html += '<tr class="grp"><td colspan="' + (COLS.length + 1) + '">'
                  + esc(g) + '</td></tr>';
        }
        gSums.ItemQty += num(r.ItemQty);
        gSums.Weight  += num(r.Weight);
        gSums.Amount  += num(r.Amount);

        var tds = COLS.map(function (c) {
            var v = r[c.k];
            if (c.link) {
                /* :265-268 - the Activity link opens the item-wise breakdown, but NOT on the
                   shortage row (SortNo 7), which is computed here and has nothing behind it. */
                if (num(r.SortNo) !== 7) {
                    return '<td><a class="act-link" onclick="sbOpenItemWise(' + i + ')">'
                         + esc(v) + '</a></td>';
                }
                return '<td><span class="act-flat">' + esc(v) + '</span></td>';
            }
            var cls = 'num' + (c.fmt === fInt && num(v) < 0 ? ' neg' : '');
            return '<td class="' + (c.fmt ? cls : '') + '">'
                 + (c.fmt ? c.fmt(v) : esc(v)) + '</td>';
        }).join('');

        html += '<tr><td style="text-align:center;">'
              + '<input type="checkbox" class="sb-chk" data-i="' + i + '"'
              + (S.checked[i] ? ' checked' : '') + ' onclick="sbToggleRow(' + i + ',this)"></td>'
              + tds + '</tr>';
    });
    if (gSums) html += groupTotalRow(gSums);

    body.innerHTML = html;

    if (foot) {
        foot.innerHTML = '<td></td>' + COLS.map(function (c) {
            if (!c.sum) return '<td></td>';
            var s = S.rows.reduce(function (a, r) { return a + num(r[c.k]); }, 0);
            return '<td class="num' + (s < 0 ? ' neg' : '') + '">' + fInt(s) + '</td>';
        }).join('');
    }
    count();
}

function count() {
    var c1 = el('sbRowCount'); if (c1) c1.textContent = S.rows.length;
    var n = 0;
    for (var k in S.checked) if (S.checked[k]) n++;
    var c2 = el('sbCheckedCount'); if (c2) c2.textContent = n;
}

window.sbToggleRow = function (i, box) {
    S.checked[i] = !!box.checked;
    count();
};

window.sbToggleAll = function (box) {
    Array.prototype.forEach.call(document.querySelectorAll('.sb-chk'), function (c) {
        c.checked = box.checked;
        S.checked[parseInt(c.getAttribute('data-i'), 10)] = !!box.checked;
    });
    count();
};

/* ---------------------------------------------------------------- Generate
 * btnGenerate_Click, :307-366. This is a pure recalculation of the SortNo 7 row from the
 * checked rows - the desktop writes nothing to the database here, and neither does this.
 *
 *   AvgRate          = TotalAmount / TotalWeight
 *   TotalShortWeight = round(TotalWeight * ShortPercent / 100, 2)
 *   TotalQty         = TotalShortWeight / 100
 *   TotalShortAmount = round(TotalShortWeight * AvgRate, 2)
 *   ShortAvgRate     = round(TotalShortAmount / TotalShortWeight * 40, 2)
 *
 * and the row is written back NEGATED for quantity, weight and amount, with ShortAvgRate
 * positive. The divisor 100 and the multiplier 40 are the desktop's own - neither is inverted.
 */
window.sbGenerate = function (btn) {
    if (S.busy) return;
    clearMsg();

    var shortPercent = num(val('shortPercent'));
    if (shortPercent === 0) {
        var sp = el('shortPercent'); if (sp) sp.focus();
        msg('Short Percent Field is Required', false);
        return;
    }
    var idx = [];
    for (var k in S.checked) if (S.checked[k]) idx.push(parseInt(k, 10));
    if (!idx.length) { msg('Please check the row first...', false); return; }

    var totalWeight = 0, totalAmount = 0;
    idx.forEach(function (i) {
        totalWeight += num(S.rows[i].Weight);
        totalAmount += num(S.rows[i].Amount);
    });

    if (totalWeight > 0 && shortPercent > 0) {
        var avgRate          = totalAmount / totalWeight;
        var totalShortWeight = Math.round(totalWeight * shortPercent / 100 * 100) / 100;
        var totalQty         = totalShortWeight / 100;
        var totalShortAmount = Math.round(totalShortWeight * avgRate * 100) / 100;
        var shortAvgRate     = Math.round(totalShortAmount / totalShortWeight * 40 * 100) / 100;

        var hit = false;
        S.rows.forEach(function (r) {
            if (num(r.SortNo) === 7) {
                r.ItemQty = -totalQty;
                r.Weight  = -totalShortWeight;
                r.Amount  = -totalShortAmount;
                r.AvgRate = shortAvgRate;
                hit = true;
            }
        });
        render();
        msg(hit ? 'Shortage row recalculated (nothing was saved - the desktop does not save here either).'
                : 'No shortage row (SortNo 7) in this result, so there was nothing to update.', hit);
    }
};

/* txtShortPercent_KeyPress -> CommonServices.OnlytextdecimelFunction: digits and a single
   decimal point. Enforced on input rather than on keypress so a paste cannot get around it. */
function bindShortPercent() {
    var e = el('shortPercent');
    if (!e) return;
    e.addEventListener('input', function () {
        var v = e.value.replace(/[^0-9.]/g, '');
        var first = v.indexOf('.');
        if (first >= 0) {
            v = v.slice(0, first + 1) + v.slice(first + 1).replace(/\./g, '');
        }
        if (v !== e.value) e.value = v;
    });
}

/* ---------------------------------------------------------------- Activity link
 * :265-277 - opens PaddyStockBreakUpItemWise with this row's SortNo and Activity name.
 * That form is not one of the DashBoard screens and has not been ported, so rather than open
 * something unrelated the row's own parameters are shown and the form is named.
 */
window.sbOpenItemWise = function (i) {
    var r = S.rows[i];
    if (!r) return;
    msg('Item-wise breakdown for "' + txt(r.Activity) + '" (SortNo ' + num(r.SortNo)
      + ') comes from the desktop form PaddyStockBreakUpItemWise.cs, which has not been ported '
      + 'yet. Nothing else was opened.', false);
};

/* ---------------------------------------------------------------- toolbar */
window.sbReset = function (btn) {
    if (S.busy) return;
    busy(btn, true);
    S.rows = [];
    S.checked = {};
    setVal('shortPercent', '');
    var rs = el('rdSystem'); if (rs) rs.checked = true;
    clearMsg();
    render();
    var r = el('sbRange'); if (r) r.textContent = '';
    loadDefaults().then(function () { busy(btn, false); });
};

window.sbPrint = function () {
    if (!S.rows.length) { msg('No Record Found For Display', false); return; }
    /* The desktop runs the Crystal report 842-StockBreakUpInOutSummary.rpt, which this
       application has no equivalent of; the browser's own print is offered instead and the
       difference is stated rather than hidden. */
    msg('The desktop prints Crystal report 842-StockBreakUpInOutSummary.rpt, which is not part '
      + 'of this web application. Using the browser print instead.', true);
    setTimeout(function () { window.print(); }, 250);
};

window.sbToggleFullscreen = function () {
    document.body.classList.toggle('sb-full');
};

/* ---------------------------------------------------------------- boot */
function boot() {
    bindShortPercent();
    render();
    loadDefaults();
}

if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
else boot();

})();
