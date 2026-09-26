/* ============================================================================
 * Purchase Sale Done Work (Dashboard) - "Tasks Done"
 * Ported from Architecture.WinApp.Dashboard\InventoryDashboardTasksInformation.cs (858 lines),
 * with InventoryTaskInfoCard1, InventoryTaskInfoCard2ForWages and
 * InventoryTaskInfoCard3ForWagesValues.
 *
 *   :93-108   Load                      Date Type list, first row selected
 *   :110-128  DateTypeFill              five hard-coded choices
 *   :130-140  btnshow_Click
 *   :142-336  GenerateCards             USP_TaskDoneStatus_Dashboard, TWO result sets
 *   :338-418  UserControl_Click         a task card opens its document screen
 *   :420-439  WagesControl_Click        a wages card opens the wages screen
 *   :441-478  CmbDateType_ValueChanged
 * ==========================================================================*/
(function () {
'use strict';

var API = '/api/dashboard/inventory-tasks';

var S = {
    purchaseTasks: [], saleTasks: [],
    purchaseWages: [], saleWages: [],
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

/* The card controls' own formats: "#,##0.####" on the task cards and the wages totals,
   "#,##0.###" on each wages line (:189-191, :257-265, :273-277). */
function f4(v) {
    return num(v).toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 4 });
}
function f3(v) {
    return num(v).toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 3 });
}

function msg(text, ok) {
    var m = el('itMessage');
    if (!m) return;
    m.className = ok ? 'ok' : 'err';
    m.textContent = text;
    if (ok) setTimeout(function () { if (m.textContent === text) m.className = ''; }, 6000);
}
function clearMsg() { var m = el('itMessage'); if (m) { m.className = ''; m.textContent = ''; } }

function busy(btn, on) {
    S.busy = !!on;
    if (btn) { btn.disabled = !!on; btn.classList.toggle('btn-busy', !!on); }
    ['btnShow', 'btnNew'].forEach(function (id) {
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

/* CmbDateType_ValueChanged, :441-478. Choice 5 sets only the From date. */
window.itDateTypeChanged = function () {
    var id = num(val('dateType')) || 1;
    getJson(API + '/date-type?id=' + id + '&toDate=' + encodeURIComponent(val('toDate')))
    .then(function (d) {
        setVal('fromDate', (d && d.fromDate) || '');
        setVal('toDate',   (d && d.toDate) || '');
    }).catch(function (e) { msg('Date type failed: ' + e.message, false); });
};

/* ---------------------------------------------------------------- show */
window.itShow = function (btn) {
    if (S.busy) return;
    clearMsg();
    busy(btn, true);
    var empty = el('itEmpty');
    if (empty) { empty.style.display = ''; empty.textContent = 'Loading...'; }

    var q = '?fromDate=' + encodeURIComponent(val('fromDate'))
          + '&toDate='   + encodeURIComponent(val('toDate'));

    getJson(API + '/cards' + q).then(function (d) {
        busy(btn, false);
        if (d && d.error) msg(d.error, false);

        S.purchaseTasks = (d && d.purchaseTasks) || [];
        S.saleTasks     = (d && d.saleTasks) || [];
        S.purchaseWages = (d && d.purchaseWages) || [];
        S.saleWages     = (d && d.saleWages) || [];

        renderTasks('itPurchaseTasks', S.purchaseTasks, 'purchaseTasks');
        renderTasks('itSaleTasks',     S.saleTasks,     'saleTasks');
        renderWages('itPurchaseWages', S.purchaseWages, 'purchaseWages');
        renderWages('itSaleWages',     S.saleWages,     'saleWages');

        var total = S.purchaseTasks.length + S.saleTasks.length
                  + S.purchaseWages.length + S.saleWages.length;
        if (empty) {
            empty.style.display = total ? 'none' : '';
            empty.textContent = 'No tasks recorded for this date range.';
        }
    }).catch(function (e) {
        busy(btn, false);
        msg('Tasks dashboard failed: ' + e.message, false);
        if (empty) { empty.style.display = ''; empty.textContent = 'Could not be read.'; }
    });
};

/* InventoryTaskInfoCard1 - Vehicles, Quantity, Weight under the title (:203-248). */
function renderTasks(wrapId, cards, group) {
    var wrap = el(wrapId);
    if (!wrap) return;
    if (!cards.length) { wrap.innerHTML = ''; return; }
    wrap.innerHTML = cards.map(function (c, i) {
        return '<div class="it-card" onclick="itTaskClick(\'' + group + '\',' + i + ')"'
             + ' title="' + esc(c.key) + '">'
             + '<div class="t">' + esc(c.title) + '</div>'
             + '<div class="b">'
             + '<div class="it-row"><span>Vehicles</span><span>' + f4(c.vehicleNos) + '</span></div>'
             + '<div class="it-row"><span>Quantity</span><span>' + f4(c.qty) + '</span></div>'
             + '<div class="it-row"><span>Weight</span><span>' + f4(c.weight) + '</span></div>'
             + '</div></div>';
    }).join('');
}

/* InventoryTaskInfoCard2ForWages with its InventoryTaskInfoCard3ForWagesValues lines
   (:236-278). The card's Avg Rate is the MEAN OF THE LINE RATES, not Amount / Weight - that is
   what :275 computes, and it is reproduced rather than "corrected". */
function renderWages(wrapId, cards, group) {
    var wrap = el(wrapId);
    if (!wrap) return;
    if (!cards.length) { wrap.innerHTML = ''; return; }
    wrap.innerHTML = cards.map(function (c, i) {
        var lines = (c.lines || []).map(function (l) {
            return '<tr>'
                 + '<td>' + esc(l.activity) + '</td>'
                 + '<td class="num">' + f3(l.qty) + '</td>'
                 + '<td class="num">' + f3(l.weight) + '</td>'
                 + '<td class="num">' + f3(l.rate) + '</td>'
                 + '<td class="num">' + f3(l.amount) + '</td>'
                 + '</tr>';
        }).join('');
        return '<div class="it-wcard" onclick="itWagesClick(\'' + group + '\',' + i + ')">'
             + '<div class="t">' + esc(c.title) + '</div>'
             + '<table class="it-grid">'
             + '<thead><tr><th style="text-align:left;">Activity</th><th>Qty</th><th>Weight</th>'
             + '<th>Avg Rate</th><th>Amount</th></tr></thead>'
             + '<tbody>' + lines + '</tbody>'
             + '<tfoot><tr><td>Total</td>'
             + '<td class="num">' + f4(c.qtyTotal) + '</td>'
             + '<td class="num">' + f4(c.weightTotal) + '</td>'
             + '<td class="num">' + f4(c.avgRate) + '</td>'
             + '<td class="num">' + f4(c.amountTotal) + '</td>'
             + '</tr></tfoot></table></div>';
    }).join('');
}

/* UserControl_Click :338-418 and WagesControl_Click :420-439 open the document screen behind
   the card. Those screens are the ordinary transaction forms, and which one a card opens is
   decided by its TransTypeKey / RefDocumentTypeId. None of that routing exists on the web side
   yet, so the card reports its own keys instead of opening something unrelated. */
window.itTaskClick = function (group, i) {
    var c = (S[group] || [])[i];
    if (!c) return;
    msg('"' + txt(c.title) + '" (key ' + txt(c.key) + ', ParentId ' + num(c.parentId)
      + ') opens its own document screen on the desktop. That routing has not been ported yet, '
      + 'so nothing was opened.', false);
};

window.itWagesClick = function (group, i) {
    var c = (S[group] || [])[i];
    if (!c) return;
    msg('"' + txt(c.title) + '" (RefDocumentTypeId ' + num(c.documentTypeId)
      + ') opens the wages screen on the desktop. That routing has not been ported yet, so '
      + 'nothing was opened.', false);
};

/* ---------------------------------------------------------------- toolbar */
window.itRefresh = function (btn) {
    if (S.busy) return;
    busy(btn, true);
    S.purchaseTasks = []; S.saleTasks = []; S.purchaseWages = []; S.saleWages = [];
    ['itPurchaseTasks', 'itSaleTasks', 'itPurchaseWages', 'itSaleWages'].forEach(function (id) {
        var w = el(id); if (w) w.innerHTML = '';
    });
    var empty = el('itEmpty');
    if (empty) { empty.style.display = ''; empty.textContent = 'Press Show to load.'; }
    clearMsg();
    setVal('dateType', '1');
    itDateTypeChanged();
    busy(btn, false);
};

window.itToggleFullscreen = function () {
    document.body.classList.toggle('it-full');
};

/* ---------------------------------------------------------------- boot */
function boot() {
    /* :101 - the desktop activates the first Date Type row on load, which fires its
       ValueChanged and sets the dates. It does NOT run the report until Show is pressed. */
    setVal('dateType', '1');
    itDateTypeChanged();
}

if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
else boot();

})();
