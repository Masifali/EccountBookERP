/* ============================================================================
 * Purchase Analytics (Periodic)
 * Ported from Architecture.WinApp.AnalyticDashboard\PurchaseAnalyticPeriodic.cs (512 lines),
 * its drill-down PurchaseAnalyticPeriodicPartyAndItemWise.cs (210 lines), and the card control
 * Architecture.WinApp.DynamicCards\PurchaseAnalyticCard.cs, which is where the column
 * visibility and the number formats come from.
 *
 *   :71-113   PurchaseAnalyticPeriodic_Load -> GetSeasonScheduleDates()
 *   :115-148  DateFrom_ValueChanged   - clamped to the season, with the form's own messages
 *   :150-168  DateTo_ValueChanged     - same, clamped to the season END on both failures
 *   :170-252  btnshow_Click           - one card per IpcId, "This Season" -> "Current Season"
 *   :254-300  UserControl_Click       - caption -> item-wise, grid row -> party + item wise
 * ==========================================================================*/
(function () {
'use strict';

var API = '/api/analytics/purchase-periodic';

var S = {
    scheduleFound: false,     // isScheduleFound on the form
    seasonStart: '',
    seasonEnd: '',
    cards: [],
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

/* The desktop's Janus format strings, reproduced:
     "#,##0"      -> no decimals
     "#,##0.####" -> up to four, trailing zeros dropped                       */
function f0(v) {
    return num(v).toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 0 });
}
function f4(v) {
    return num(v).toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 4 });
}
function f2(v) {
    return num(v).toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}

/* Dates are compared as plain yyyy-mm-dd strings. Nothing goes through Date/toISOString: at
   UTC+5 that turns a local midnight into the previous day, which would move a season boundary. */
function cmp(a, b) { return String(a) < String(b) ? -1 : (String(a) > String(b) ? 1 : 0); }

function msg(text, ok) {
    var m = el('paMessage');
    if (!m) return;
    m.className = ok ? 'ok' : 'err';
    m.textContent = text;
    if (ok) setTimeout(function () { if (m.textContent === text) m.className = ''; }, 5000);
}
function clearMsg() { var m = el('paMessage'); if (m) { m.className = ''; m.textContent = ''; } }

/* Click -> disabled + loader at once, re-enabled on success or failure, second click dropped. */
function busy(btn, on) {
    S.busy = !!on;
    if (btn) { btn.disabled = !!on; btn.classList.toggle('btn-busy', !!on); }
    var other = el('btnNew');
    if (other && other !== btn) other.disabled = !!on;
}

function getJson(url) {
    return fetch(url, { headers: { 'Accept': 'application/json' } })
        .then(function (r) {
            if (!r.ok) throw new Error('HTTP ' + r.status + ' on ' + url);
            return r.json();
        });
}

/* ---------------------------------------------------------------- season schedule
 * GetSeasonScheduleDates(), :85-113. No row -> Show stays disabled and the form's own warning
 * is shown. Nothing is defaulted in place of a missing schedule.
 */
function loadSeason() {
    return getJson(API + '/season').then(function (d) {
        S.scheduleFound = !!(d && d.found);
        if (!S.scheduleFound) {
            var b = el('btnShow'); if (b) b.disabled = true;
            msg((d && d.message) || 'No Schedule found. Please Make a season year schedule first!', false);
            showEmpty('No season year schedule, so there is nothing to report on.');
            return;
        }
        S.seasonStart = d.seasonStartDate;
        S.seasonEnd   = d.seasonEndDate;
        setVal('seasonStart', d.seasonStartDate);
        setVal('seasonEnd',   d.seasonEndDate);
        /* :100-101 - From is the season start, To is today. */
        setVal('dateFrom', d.fromDate);
        setVal('dateTo',   d.toDate);
        var b2 = el('btnShow'); if (b2) b2.disabled = false;
        clearMsg();
        showEmpty('Press Show to load.');
    }).catch(function (e) {
        S.scheduleFound = false;
        var b = el('btnShow'); if (b) b.disabled = true;
        msg('Season schedule could not be read: ' + e.message, false);
    });
}

/* DateFrom_ValueChanged, :115-148. Both failures reset From to the season START. */
window.paDateFromChanged = function () {
    if (!S.scheduleFound) {
        msg('No Schedule found. Please Make a season year schedule first!', false);
        return;
    }
    var v = val('dateFrom');
    if (!v) return;
    if (cmp(v, S.seasonStart) < 0) {
        setVal('dateFrom', S.seasonStart);
        msg("FromDate can't be less than season start date!", false);
    } else if (cmp(v, S.seasonEnd) > 0) {
        setVal('dateFrom', S.seasonStart);
        msg("FromDate can't be greater than season end date!", false);
    } else {
        clearMsg();
    }
};

/* DateTo_ValueChanged, :150-168. Note the asymmetry: BOTH failures reset To to the season END,
   including the "less than season start" one. Reproduced as written. */
window.paDateToChanged = function () {
    if (!S.scheduleFound) {
        msg('No Schedule found. Please Make a season year schedule first!', false);
        return;
    }
    var v = val('dateTo');
    if (!v) return;
    if (cmp(v, S.seasonStart) < 0) {
        setVal('dateTo', S.seasonEnd);
        msg("ToDate can't be less than season start date!", false);
    } else if (cmp(v, S.seasonEnd) > 0) {
        setVal('dateTo', S.seasonEnd);
        msg("ToDate can't be greater than season end date!", false);
    } else {
        clearMsg();
    }
};

/* ---------------------------------------------------------------- the cards
 * btnshow_Click, :170-252. The desktop returns immediately when no schedule was found (:181).
 */
window.paShow = function (btn) {
    if (S.busy) return;
    if (!S.scheduleFound) return;
    clearMsg();
    busy(btn, true);

    var q = '?fromDate=' + encodeURIComponent(val('dateFrom'))
          + '&toDate='   + encodeURIComponent(val('dateTo'))
          + '&seasonStart=' + encodeURIComponent(S.seasonStart)
          + '&seasonEnd='   + encodeURIComponent(S.seasonEnd);

    getJson(API + '/report' + q).then(function (d) {
        busy(btn, false);
        if (d && d.error) { msg(d.error, false); showEmpty('The report could not be read.'); return; }
        S.cards = (d && d.cards) || [];
        renderCards();
        var r = el('paRange');
        if (r) r.textContent = val('dateFrom') + '  to  ' + val('dateTo');
    }).catch(function (e) {
        busy(btn, false);
        msg('Report failed: ' + e.message, false);
        showEmpty('The report could not be read.');
    });
};

function showEmpty(text) {
    var wrap = el('paCards'), empty = el('paEmpty');
    if (wrap) wrap.innerHTML = '';
    if (empty) { empty.style.display = ''; empty.textContent = text; }
    var c = el('paCardCount'); if (c) c.textContent = '0';
}

/* Card grid, RequestFor = "Product_Wise_Summ" (PurchaseAnalyticCard :210-225).
   Hidden there and so not rendered: Supplier, Exp+Amount, Moisture, Broken, AnalysisQty, and
   Empty Shell / Trash, Dust / Stone, AGL, Damage (:161-164). */
var SUMM_COLS = [
    { k: 'SortNo',       c: 'SortNo',       w: 45,  fmt: null },
    { k: 'Descriptions', c: 'Descriptions', w: 110, fmt: null },
    { k: 'Qty',          c: 'Qty',          w: 60,  fmt: f0 },
    { k: 'Weight',       c: 'Weight',       w: 75,  fmt: f0 },
    { k: 'ItemRate',     c: 'ItemRate',     w: 75,  fmt: f4 },
    { k: 'Exp40Kg',      c: 'Exp40Kg',      w: 75,  fmt: f4 },
    { k: 'AvgRate',      c: 'AvgRate',      w: 80,  fmt: f4 }
];

/* Card grid, RequestFor = "ProductAndPartyWise" (PurchaseAnalyticCard :227-233).
   Descriptions is hidden and a total row is switched on; Broken is hidden because ParentIds is
   empty on that card, which takes the else branch at :206-210, leaving Moisture visible.
   Exp+Amount carries AggregateFunction 2 - Sum - so that is the footer total. */
var PARTY_COLS = [
    { k: 'SortNo',      c: 'SortNo',      w: 45,  fmt: null },
    { k: 'Supplier',    c: 'Supplier',    w: 190, fmt: null },
    { k: 'Qty',         c: 'Qty',         w: 60,  fmt: f0 },
    { k: 'Weight',      c: 'Weight',      w: 75,  fmt: f0 },
    { k: 'ItemRate',    c: 'ItemRate',    w: 75,  fmt: f4 },
    { k: 'Exp40Kg',     c: 'Exp40Kg',     w: 75,  fmt: f4 },
    { k: 'AvgRate',     c: 'AvgRate',     w: 80,  fmt: f4 },
    { k: 'ExpAmount',   c: 'Exp+Amount',  w: 90,  fmt: f2, sum: true },
    { k: 'AnalysisQty', c: 'AnalysisQty', w: 65,  fmt: f0 },
    { k: 'Moisture',    c: 'Moisture',    w: 55,  fmt: f0 }
];

function gridHtml(cols, rows, withTotal, clickFn, cardId) {
    var head = cols.map(function (c) {
        return '<th style="width:' + c.w + 'px;">' + esc(c.c) + '</th>';
    }).join('');

    var body = rows.map(function (r, i) {
        var tds = cols.map(function (c) {
            var v = r[c.k];
            var shown = c.fmt ? c.fmt(v) : esc(v);
            return '<td class="' + (c.fmt ? 'num' : '') + '">' + shown + '</td>';
        }).join('');
        var attr = clickFn ? ' onclick="' + clickFn + '(' + cardId + ',' + i + ')"' : '';
        return '<tr' + attr + '>' + tds + '</tr>';
    }).join('');

    var foot = '';
    if (withTotal) {
        foot = '<tfoot><tr>' + cols.map(function (c) {
            if (!c.sum) return '<td></td>';
            var s = rows.reduce(function (a, r) { return a + num(r[c.k]); }, 0);
            return '<td class="num">' + f2(s) + '</td>';
        }).join('') + '</tr></tfoot>';
    }

    return '<div class="pa-scroll"><table class="pa-grid"><thead><tr>' + head + '</tr></thead>'
         + '<tbody>' + body + '</tbody>' + foot + '</table></div>';
}

function renderCards() {
    var wrap = el('paCards'), empty = el('paEmpty');
    if (!wrap) return;
    if (!S.cards.length) { showEmpty('No rows for this date range.'); return; }
    if (empty) empty.style.display = 'none';

    wrap.innerHTML = S.cards.map(function (card, ci) {
        return '<div class="pa-card">'
             + '<div class="pa-card-head">'
             /* lblTitle is a link on the desktop and opens the item-wise screen (:290-299). */
             + '<a onclick="paOpenItemWise(' + ci + ')">' + esc(card.caption) + '</a>'
             + '<span>' + card.rows.length + '</span>'
             + '</div>'
             + gridHtml(SUMM_COLS, card.rows, false, 'paOpenPartyItem', ci)
             + '</div>';
    }).join('');

    var c = el('paCardCount'); if (c) c.textContent = S.cards.length;
}

/* ---------------------------------------------------------------- drill-downs */

/* Grid row -> PurchaseAnalyticPeriodicPartyAndItemWise (:270-288), carrying the card's IpcId as
   Ids and the clicked row's SortNo. The desktop opens it with ShowDialog(); here it is a modal. */
window.paOpenPartyItem = function (cardIndex, rowIndex) {
    var card = S.cards[cardIndex];
    if (!card) return;
    var row = card.rows[rowIndex];
    if (!row) return;

    var body = el('paModalBody');
    var title = el('paModalTitle');
    if (title) title.textContent = card.caption + '  -  ' + txt(row.Descriptions);
    if (body) body.innerHTML = '<div class="pa-empty" style="margin:0;">Loading...</div>';
    el('paModal').style.display = 'block';

    var q = '?fromDate=' + encodeURIComponent(val('dateFrom'))
          + '&toDate='   + encodeURIComponent(val('dateTo'))
          + '&seasonStart=' + encodeURIComponent(S.seasonStart)
          + '&seasonEnd='   + encodeURIComponent(S.seasonEnd)
          + '&ids='    + encodeURIComponent(card.id)
          + '&sortNo=' + num(row.SortNo);

    getJson(API + '/party-item' + q).then(function (d) {
        if (!body) return;
        if (d && d.error) {
            body.innerHTML = '<div class="pa-empty" style="margin:0;">' + esc(d.error) + '</div>';
            return;
        }
        var cards = (d && d.cards) || [];
        if (!cards.length) {
            body.innerHTML = '<div class="pa-empty" style="margin:0;">No detail for this row.</div>';
            return;
        }
        body.innerHTML = cards.map(function (c2) {
            return '<div class="pa-card">'
                 + '<div class="pa-card-head"><span>' + esc(c2.caption) + '</span>'
                 + '<span>' + c2.rows.length + '</span></div>'
                 + gridHtml(PARTY_COLS, c2.rows, true, null, 0)
                 + '</div>';
        }).join('');
    }).catch(function (e) {
        if (body) body.innerHTML = '<div class="pa-empty" style="margin:0;">'
                                 + esc(e.message) + '</div>';
    });
};

window.paCloseModal = function () {
    var m = el('paModal'); if (m) m.style.display = 'none';
};

/* Card caption -> PurchaseAnalyticPeriodicItemWise (:290-299), carrying this card's IpcId as
   the parent category and the same dates - which is what RequestedByOtherDocument does on the
   desktop (that form's :105-118: the category is fixed, the combo disabled, the report run). */
window.paOpenItemWise = function (cardIndex) {
    var card = S.cards[cardIndex];
    if (!card) return;
    window.location.href = '/dashboard/purchase-analytic-periodic-item-wise'
        + '?parentCategoryId=' + encodeURIComponent(card.id);
};

/* ---------------------------------------------------------------- toolbar */
window.paRefresh = function (btn) {
    if (S.busy) return;
    busy(btn, true);
    S.cards = [];
    showEmpty('Press Show to load.');
    var r = el('paRange'); if (r) r.textContent = '';
    loadSeason().then(function () { busy(btn, false); });
};

window.paToggleFullscreen = function () {
    document.body.classList.toggle('pa-full');
};

/* ---------------------------------------------------------------- boot */
function boot() {
    /* PurchaseAnalyticPeriodic_Load, :71-83 */
    loadSeason();

    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') paCloseModal();
    });
    var m = el('paModal');
    if (m) m.addEventListener('click', function (e) { if (e.target === m) paCloseModal(); });
}

if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
else boot();

})();
