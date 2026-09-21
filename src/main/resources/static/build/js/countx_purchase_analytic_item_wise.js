/* ============================================================================
 * Purchase Analytic Periodic ItemWise
 * Ported from Architecture.WinApp.AnalyticDashboard\PurchaseAnalyticPeriodicItemWise.cs
 * (715 lines), with the card control PurchaseAnalyticCard.cs.
 *
 *   :95-124   Load       season dates, parent categories, and the RequestedByOtherDocument path
 *   :126-152  ParentCategoryComboFill  a CHECKED list - multi-select
 *   :154-187  GetSeasonScheduleDates
 *   :189-215  DateFrom_ValueChanged
 *   :217-236  DateTo_ValueChanged
 *   :238-341  btnshow_Click  USP_PurchaseAnalyticsAB_Report @Activity='Product_Wise'
 * ==========================================================================*/
(function () {
'use strict';

var API = '/api/analytics/purchase-item-wise';

var S = {
    scheduleFound: false,
    seasonStart: '',
    seasonEnd: '',
    cards: [],
    hideMoisture: false,
    fromOtherDocument: false,
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

/* PurchaseAnalyticCard's own formats: "#,##0" for counts and weights, "#,##0.####" for rates. */
function f0(v) {
    return num(v).toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 0 });
}
function f4(v) {
    return num(v).toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 4 });
}

function cmp(a, b) { return String(a) < String(b) ? -1 : (String(a) > String(b) ? 1 : 0); }

function f(row, name) {
    if (!row) return null;
    if (row[name] !== undefined) return row[name];
    var lower = String(name).toLowerCase();
    for (var k in row) { if (k.toLowerCase() === lower) return row[k]; }
    return null;
}

function msg(text, ok) {
    var m = el('piMessage');
    if (!m) return;
    m.className = ok ? 'ok' : 'err';
    m.textContent = text;
    if (ok) setTimeout(function () { if (m.textContent === text) m.className = ''; }, 5000);
}
function clearMsg() { var m = el('piMessage'); if (m) { m.className = ''; m.textContent = ''; } }

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

/* ---------------------------------------------------------------- season */
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
        setVal('dateFrom', d.fromDate);
        setVal('dateTo',   d.toDate);
        var b2 = el('btnShow'); if (b2) b2.disabled = false;
        clearMsg();
    }).catch(function (e) {
        S.scheduleFound = false;
        var b = el('btnShow'); if (b) b.disabled = true;
        msg('Season schedule could not be read: ' + e.message, false);
    });
}

/* DateFrom_ValueChanged, :189-215 - both failures reset From to the season START. */
window.piDateFromChanged = function () {
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

/* DateTo_ValueChanged, :217-236 - both failures reset To to the season END. */
window.piDateToChanged = function () {
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

/* ---------------------------------------------------------------- categories */
function loadCategories() {
    return getJson(API + '/parent-categories').then(function (rows) {
        rows = rows || [];
        var sel = el('parentCats');
        if (!sel) return;
        sel.innerHTML = rows.map(function (r) {
            return '<option value="' + esc(f(r, 'Id')) + '">'
                 + esc(f(r, 'InvParentCateDescription')) + '</option>';
        }).join('');

        /* RequestedByOtherDocument, :105-118: the category is fixed to the one handed over and
           the combo is disabled, then the report runs at once. */
        var fixedId = window.PI_PARENT_CATEGORY_ID;
        if (fixedId !== null && fixedId !== undefined && String(fixedId) !== '') {
            S.fromOtherDocument = true;
            Array.prototype.forEach.call(sel.options, function (o) {
                o.selected = (String(o.value) === String(fixedId));
            });
            sel.disabled = true;
        }

        if (window.jQuery && jQuery.fn.select2) {
            jQuery(sel).filter(function () { return !jQuery(this).data('select2'); })
                       .select2({ width: '100%', placeholder: 'Parent Categories' });
        }
    }).catch(function (e) {
        msg('Parent categories could not be read: ' + e.message, false);
    });
}

function selectedIds() {
    var sel = el('parentCats');
    if (!sel) return '';
    var out = [];
    Array.prototype.forEach.call(sel.options, function (o) { if (o.selected) out.push(o.value); });
    /* The desktop builds "id,id," - trailing comma from its own loop (:252). Kept identical so
       the procedure receives the same string. */
    return out.length ? out.join(',') + ',' : '';
}

/* ---------------------------------------------------------------- show */
window.piShow = function (btn) {
    if (S.busy) return;
    if (!S.scheduleFound) return;          // :244-247
    clearMsg();

    var ids = selectedIds();
    if (!ids) { msg('Please Select a Parent Category', false); return; }   // :259-262

    busy(btn, true);
    var q = '?fromDate=' + encodeURIComponent(val('dateFrom'))
          + '&toDate='   + encodeURIComponent(val('dateTo'))
          + '&seasonStart=' + encodeURIComponent(S.seasonStart)
          + '&seasonEnd='   + encodeURIComponent(S.seasonEnd)
          + '&ids=' + encodeURIComponent(ids);

    getJson(API + '/report' + q).then(function (d) {
        busy(btn, false);
        if (d && d.error) { msg(d.error, false); showEmpty('The report could not be read.'); return; }
        S.cards = (d && d.cards) || [];
        S.hideMoisture = !!(d && d.hideMoisture);
        render();
        var r = el('piRange');
        if (r) r.textContent = val('dateFrom') + '  to  ' + val('dateTo');
    }).catch(function (e) {
        busy(btn, false);
        msg('Report failed: ' + e.message, false);
        showEmpty('The report could not be read.');
    });
};

function showEmpty(text) {
    var wrap = el('piCards'), empty = el('piEmpty');
    if (wrap) wrap.innerHTML = '';
    if (empty) { empty.style.display = ''; empty.textContent = text; }
    var c = el('piCardCount'); if (c) c.textContent = '0';
}

/* Card grid for RequestFor = "Product_Wise" (PurchaseAnalyticCard :225-231).
   Hidden there: Supplier and Exp+Amount. Hidden globally (:161-164): Empty Shell / Trash,
   Dust / Stone, AGL, Damage. Moisture or Broken drops out depending on whether ParentIds
   contains "2" (:202-210). */
function columns() {
    var cols = [
        { k: 'SortNo',       c: 'SortNo',       w: 45,  fmt: null },
        { k: 'Descriptions', c: 'Descriptions', w: 110, link: true },
        { k: 'Qty',          c: 'Qty',          w: 60,  fmt: f0 },
        { k: 'Weight',       c: 'Weight',       w: 75,  fmt: f0 },
        { k: 'ItemRate',     c: 'ItemRate',     w: 75,  fmt: f4 },
        { k: 'Exp40Kg',      c: 'Exp40Kg',      w: 75,  fmt: f4 },
        { k: 'AvgRate',      c: 'AvgRate',      w: 80,  fmt: f4 },
        { k: 'AnalysisQty',  c: 'AnalysisQty',  w: 70,  fmt: f0 }
    ];
    if (S.hideMoisture) cols.push({ k: 'Broken',   c: 'Broken',   w: 60, fmt: f0 });
    else                cols.push({ k: 'Moisture', c: 'Moisture', w: 60, fmt: f0 });
    return cols;
}

function render() {
    var wrap = el('piCards'), empty = el('piEmpty');
    if (!wrap) return;
    if (!S.cards.length) { showEmpty('No rows for this selection.'); return; }
    if (empty) empty.style.display = 'none';

    var cols = columns();

    wrap.innerHTML = S.cards.map(function (card, ci) {
        var head = cols.map(function (c) {
            return '<th style="width:' + c.w + 'px;">' + esc(c.c) + '</th>';
        }).join('');

        var body = card.rows.map(function (r, ri) {
            return '<tr>' + cols.map(function (c) {
                var v = r[c.k];
                if (c.link) {
                    return '<td><span class="pi-link" onclick="piRowClick(' + ci + ',' + ri
                         + ')">' + esc(v) + '</span></td>';
                }
                return '<td class="' + (c.fmt ? 'num' : '') + '">'
                     + (c.fmt ? c.fmt(v) : esc(v)) + '</td>';
            }).join('') + '</tr>';
        }).join('');

        return '<div class="pi-card">'
             + '<div class="pi-card-head"><span>' + esc(card.caption) + '</span>'
             + '<span>' + card.rows.length + '</span></div>'
             + '<div class="pi-scroll"><table class="pi-grid">'
             + '<thead><tr>' + head + '</tr></thead><tbody>' + body + '</tbody>'
             + '</table></div></div>';
    }).join('');

    var c = el('piCardCount'); if (c) c.textContent = S.cards.length;
}

/* UserControl_Click, :343-389 - the Descriptions link drills into the party breakdown for that
   item and period. It is the same USP_PurchaseAnalyticsAB_Report the periodic screen's row
   click uses, so it is handed to that screen rather than duplicated here. */
window.piRowClick = function (ci, ri) {
    var card = S.cards[ci];
    if (!card) return;
    var row = card.rows[ri];
    if (!row) return;
    msg('Party breakdown for "' + txt(card.caption) + '" / ' + txt(row.Descriptions)
      + ' is the same drill-down as the Purchase Analytics (Periodic) grid rows - open it there '
      + 'for this item.', false);
};

/* ---------------------------------------------------------------- toolbar */
window.piRefresh = function (btn) {
    if (S.busy) return;
    busy(btn, true);
    S.cards = [];
    showEmpty('Press Show to load.');
    var r = el('piRange'); if (r) r.textContent = '';
    clearMsg();
    loadSeason().then(loadCategories).then(function () { busy(btn, false); });
};

window.piToggleFullscreen = function () {
    document.body.classList.toggle('pi-full');
};

/* ---------------------------------------------------------------- boot */
function boot() {
    showEmpty('Press Show to load.');
    loadSeason().then(loadCategories).then(function () {
        /* :117 - when the periodic screen opened this one, it runs the report immediately. */
        if (S.fromOtherDocument && S.scheduleFound) {
            var b = el('btnShow');
            if (b) piShow(b);
        }
    });
}

if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
else boot();

})();
