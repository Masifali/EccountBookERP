/* ============================================================================
 * Monthly Purchase / Sale (Graph) Report
 * Ported from Architecture.WinApp.Graph\MonthWisePurchaseReport.cs (1,156 lines) and
 * MonthWiseSaleReport.cs (1,161 lines).
 *
 *   :111-126  Load           Month(), the combos, then ShowReport() at once
 *   :128-152  Month()        twelve hard-coded months
 *   :156-213  the combos     one flat table split by its Activity column
 *   :215-225  btnshow_Click
 *   :227-330  ShowReport()   Tables[2] months -> bar chart + cards; Tables[1] quarters -> pie
 *   :402-414  btnNew_Click
 *
 * Charts are inline SVG so the page needs no charting library.
 *   - The bar chart is one series, so it carries no legend; its heading names it.
 *   - The quarters are an ORDERED set, so the pie uses a single hue stepped light to dark -
 *     a sequential ramp, not four unrelated colours - and carries a legend, because more than
 *     one slice means identity must not rest on colour alone. Both grids below double as the
 *     charts' table view.
 * ==========================================================================*/
(function () {
'use strict';

var API = '/api/dashboard/month-wise';
var MODE = window.MW_MODE || 'purchase';

var S = { months: [], quarters: [], busy: false };

/* ---------------------------------------------------------------- helpers */
function el(id)  { return document.getElementById(id); }
function val(id) { var e = el(id); return e ? e.value : ''; }
function setVal(id, v) { var e = el(id); if (e) e.value = v === null || v === undefined ? '' : v; }
function txt(v)  { return v === null || v === undefined ? '' : String(v); }
function esc(v)  { return txt(v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
                                .replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
function num(v)  { var n = parseFloat(v); return isNaN(n) ? 0 : n; }

/* The chart tooltips are "#VALY{#,##0.##}" on both forms (:262, :275) - up to two decimals. */
function f2(v) {
    return num(v).toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 2 });
}
/* A compact axis tick, so large rupee amounts do not collide. */
function fAxis(v) {
    var n = Math.abs(num(v));
    if (n >= 1e9) return (num(v) / 1e9).toFixed(1) + 'B';
    if (n >= 1e6) return (num(v) / 1e6).toFixed(1) + 'M';
    if (n >= 1e3) return (num(v) / 1e3).toFixed(0) + 'K';
    return String(Math.round(num(v)));
}

function msg(text, ok) {
    var m = el('mwMessage');
    if (!m) return;
    m.className = ok ? 'ok' : 'err';
    m.textContent = text;
    if (ok) setTimeout(function () { if (m.textContent === text) m.className = ''; }, 5000);
}
function clearMsg() { var m = el('mwMessage'); if (m) { m.className = ''; m.textContent = ''; } }

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

/* ---------------------------------------------------------------- tabs */
window.mwTab = function (which) {
    ['month', 'quarter'].forEach(function (t) {
        var v = el('view' + t.charAt(0).toUpperCase() + t.slice(1));
        var b = el('tab'  + t.charAt(0).toUpperCase() + t.slice(1));
        if (v) v.style.display = (t === which) ? '' : 'none';
        if (b) b.classList.toggle('active', t === which);
    });
    /* An SVG sized while its tab was hidden measures zero, so it is redrawn on the way in. */
    if (which === 'month') renderBar(); else renderPie();
};

/* ---------------------------------------------------------------- combos */
function loadCombos() {
    return getJson(API + '/combos?mode=' + encodeURIComponent(MODE)).then(function (d) {
        if (d && d.error) msg('Filter lists: ' + d.error, false);

        var months = (d && d.months) || [];
        var sel = el('cmbMonth');
        if (sel) {
            sel.innerHTML = '<option value="0">Month</option>'
                + months.map(function (m, i) {
                      return '<option value="' + (i + 1) + '">' + esc(m) + '</option>';
                  }).join('');
        }
        fill('cmbCategory',   (d && d.categories) || [],  'Category');
        fill('cmbClassGroup', (d && d.classGroups) || [], 'Class Group');
    }).catch(function (e) {
        msg('Filter lists could not be read: ' + e.message, false);
    });
}

function fill(id, rows, caption) {
    var sel = el(id);
    if (!sel) return;
    sel.innerHTML = '<option value="0">' + esc(caption) + '</option>'
        + rows.map(function (r) {
              return '<option value="' + esc(r.id) + '">' + esc(r.name) + '</option>';
          }).join('');
}

/* ---------------------------------------------------------------- report */
window.mwShow = function (btn) {
    if (S.busy) return;
    clearMsg();
    busy(btn, true);

    var q = '?mode=' + encodeURIComponent(MODE)
          + '&month=' + (num(val('cmbMonth')) || 0)
          + '&categoryId=' + (num(val('cmbCategory')) || 0)
          + '&classGroupId=' + (num(val('cmbClassGroup')) || 0);

    getJson(API + '/report' + q).then(function (d) {
        busy(btn, false);
        if (d && d.error) msg(d.error, false);

        S.months   = (d && d.months) || [];
        S.quarters = (d && d.quarters) || [];

        renderMonthGrid(num(d && d.monthTotal));
        renderQuarterGrid(num(d && d.quarterTotal));
        renderBar();
        renderPie();

        var p = el('mwPeriod');
        if (p && d) p.textContent = txt(d.fromDate) + '  to  ' + txt(d.toDate);
    }).catch(function (e) {
        busy(btn, false);
        msg('Report failed: ' + e.message, false);
    });
};

function renderMonthGrid(total) {
    var body = el('mwMonthBody'), foot = el('mwMonthFoot');
    if (!body) return;
    if (!S.months.length) {
        body.innerHTML = '<tr><td colspan="2" style="text-align:center; padding:16px; color:#777;">'
                       + 'No months for this selection.</td></tr>';
        if (foot) foot.innerHTML = '';
    } else {
        body.innerHTML = S.months.map(function (m) {
            return '<tr><td>' + esc(m.month) + '</td>'
                 + '<td class="num">' + f2(m.amount) + '</td></tr>';
        }).join('');
        if (foot) foot.innerHTML = '<td>Total</td><td class="num">' + f2(total) + '</td>';
    }
    var c = el('mwMonthCount'); if (c) c.textContent = S.months.length;
    var t = el('mwMonthTotal'); if (t) t.textContent = f2(total);
}

function renderQuarterGrid(total) {
    var body = el('mwQuarterBody'), foot = el('mwQuarterFoot');
    if (!body) return;
    if (!S.quarters.length) {
        body.innerHTML = '<tr><td colspan="4" style="text-align:center; padding:16px; color:#777;">'
                       + 'No quarters for this selection.</td></tr>';
        if (foot) foot.innerHTML = '';
    } else {
        body.innerHTML = S.quarters.map(function (q) {
            return '<tr><td>' + esc(q.startDate) + '</td><td>' + esc(q.endDate) + '</td>'
                 + '<td>' + esc(q.year) + '</td>'
                 + '<td class="num">' + f2(q.amount) + '</td></tr>';
        }).join('');
        if (foot) foot.innerHTML = '<td colspan="3">Total</td><td class="num">'
                                 + f2(total) + '</td>';
    }
    var c = el('mwQuarterCount'); if (c) c.textContent = S.quarters.length;
    var t = el('mwQuarterTotal'); if (t) t.textContent = f2(total);
}

/* ---------------------------------------------------------------- bar chart
 * chart1: X = the month label, Y = the amount, ChartType Column, X gridlines off and Y
 * gridlines dotted (:253-263). One series, so no legend.
 */
function renderBar() {
    var svg = el('mwBar');
    if (!svg) return;
    var rows = S.months;
    if (!rows.length) { svg.innerHTML = ''; svg.setAttribute('height', 0); return; }

    var barW = 46, gap = 16, padL = 62, padR = 16, padT = 16, padB = 44;
    var plotH = 230;
    var w = Math.max(padL + padR + rows.length * (barW + gap), 640);
    var h = plotH + padT + padB;

    var max = rows.reduce(function (a, r) { return Math.max(a, num(r.amount)); }, 0);
    if (max <= 0) max = 1;
    var step = Math.pow(10, Math.floor(Math.log(max) / Math.LN10));
    var top = Math.ceil(max / (step / 2)) * (step / 2);
    if (!isFinite(top) || top <= 0) top = max;

    var parts = [];
    var lines = 4;
    for (var g = 0; g <= lines; g++) {
        var v = top * g / lines;
        var y = padT + plotH - (v / top) * plotH;
        /* AxisY.MajorGrid dotted (:249) - AxisX gridlines are off (:248). */
        parts.push('<line class="grid" x1="' + padL + '" y1="' + y + '" x2="' + (w - padR)
                 + '" y2="' + y + '"/>');
        parts.push('<text class="tick" x="' + (padL - 6) + '" y="' + (y + 3)
                 + '" text-anchor="end">' + fAxis(v) + '</text>');
    }
    parts.push('<line class="axis" x1="' + padL + '" y1="' + (padT + plotH)
             + '" x2="' + (w - padR) + '" y2="' + (padT + plotH) + '"/>');

    rows.forEach(function (r, i) {
        var v = num(r.amount);
        var bh = Math.max((v / top) * plotH, v > 0 ? 2 : 0);
        var x = padL + gap / 2 + i * (barW + gap);
        var y = padT + plotH - bh;
        var rx = Math.min(4, bh);
        var d = 'M' + x + ',' + (padT + plotH) + 'V' + (y + rx)
              + 'q0,' + (-rx) + ' ' + rx + ',' + (-rx)
              + 'h' + (barW - 2 * rx)
              + 'q' + rx + ',0 ' + rx + ',' + rx
              + 'V' + (padT + plotH) + 'Z';
        parts.push('<path class="bar" d="' + d + '"/>');
        /* AxisX.LabelStyle.Font is bold on the desktop (:250). */
        parts.push('<text class="catlbl" x="' + (x + barW / 2) + '" y="' + (padT + plotH + 16)
                 + '" text-anchor="middle">' + esc(r.month) + '</text>');
        parts.push('<rect class="hit" x="' + (x - gap / 2) + '" y="' + padT
                 + '" width="' + (barW + gap) + '" height="' + plotH + '" data-i="' + i + '"/>');
    });

    svg.setAttribute('width', w);
    svg.setAttribute('height', h);
    svg.setAttribute('viewBox', '0 0 ' + w + ' ' + h);
    svg.innerHTML = parts.join('');

    var tip = el('mwBarTip');
    Array.prototype.forEach.call(svg.querySelectorAll('.hit'), function (hit) {
        hit.addEventListener('mousemove', function (ev) {
            var r = S.months[parseInt(hit.getAttribute('data-i'), 10)];
            if (!r || !tip) return;
            tip.innerHTML = '<b>' + esc(r.month) + '</b><br>' + f2(r.amount);
            tip.style.display = 'block';
            var card = tip.parentElement.getBoundingClientRect();
            tip.style.left = (ev.clientX - card.left + 12) + 'px';
            tip.style.top  = (ev.clientY - card.top + 12) + 'px';
        });
        hit.addEventListener('mouseleave', function () { if (tip) tip.style.display = 'none'; });
    });
}

/* ---------------------------------------------------------------- pie chart
 * chart2: X = "MMM To MMM yy" built from the quarter's start and end (:269), Y = the amount,
 * with PieLabelStyle disabled (:274) - so the slices carry no inline labels on the desktop
 * either, and the legend plus the grid below identify them.
 *
 * The quarters are ordered, so the slices step through one hue from light to dark rather than
 * using four unrelated colours.
 */
var PIE_RAMP = ['#8fc7c1', '#5aa8a0', '#2e8079', '#165e57', '#0f4a44', '#0b3a35'];

function quarterLabel(q) {
    /* "MMM To MMM  yy" - :269 formats the two ends as MMM and the start's year as yy. */
    var MON = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    function mon(d) {
        var m = String(d || '').match(/^\d{4}-(\d{2})-\d{2}/);
        return m ? MON[parseInt(m[1], 10) - 1] : String(d || '');
    }
    var yy = String(q.startDate || '').slice(2, 4);
    return mon(q.startDate) + ' To ' + mon(q.endDate) + '  ' + yy;
}

function renderPie() {
    var svg = el('mwPie'), legend = el('mwPieLegend');
    if (!svg) return;
    var rows = S.quarters;
    if (!rows.length) {
        svg.innerHTML = ''; svg.setAttribute('height', 0);
        if (legend) legend.innerHTML = '';
        return;
    }

    var size = 240, r = 100, cx = size / 2, cy = size / 2;
    var total = rows.reduce(function (a, q) { return a + Math.abs(num(q.amount)); }, 0);
    var parts = [];

    if (total <= 0) {
        parts.push('<circle cx="' + cx + '" cy="' + cy + '" r="' + r
                 + '" fill="#eef2f4" stroke="#cfd8dc"/>');
        parts.push('<text class="tick" x="' + cx + '" y="' + cy
                 + '" text-anchor="middle">No amounts</text>');
    } else {
        var a0 = -Math.PI / 2;
        rows.forEach(function (q, i) {
            var frac = Math.abs(num(q.amount)) / total;
            var a1 = a0 + frac * Math.PI * 2;
            var large = (a1 - a0) > Math.PI ? 1 : 0;
            var x0 = cx + r * Math.cos(a0), y0 = cy + r * Math.sin(a0);
            var x1 = cx + r * Math.cos(a1), y1 = cy + r * Math.sin(a1);
            var colour = PIE_RAMP[i % PIE_RAMP.length];
            /* A full circle cannot be drawn as one arc, so a lone quarter is a circle. */
            var d = (rows.length === 1)
                ? null
                : 'M' + cx + ',' + cy + ' L' + x0 + ',' + y0
                  + ' A' + r + ',' + r + ' 0 ' + large + ' 1 ' + x1 + ',' + y1 + ' Z';
            if (d) {
                parts.push('<path class="slice" d="' + d + '" fill="' + colour
                         + '" data-i="' + i + '"/>');
            } else {
                parts.push('<circle class="slice" cx="' + cx + '" cy="' + cy + '" r="' + r
                         + '" fill="' + colour + '" data-i="' + i + '"/>');
            }
            a0 = a1;
        });
    }

    svg.setAttribute('width', size);
    svg.setAttribute('height', size);
    svg.setAttribute('viewBox', '0 0 ' + size + ' ' + size);
    svg.innerHTML = parts.join('');

    if (legend) {
        legend.innerHTML = rows.map(function (q, i) {
            var pct = total > 0 ? (Math.abs(num(q.amount)) / total * 100) : 0;
            return '<span class="k"><i style="background:' + PIE_RAMP[i % PIE_RAMP.length]
                 + '"></i>' + esc(quarterLabel(q)) + ' &mdash; ' + f2(q.amount)
                 + ' (' + pct.toFixed(1) + '%)</span>';
        }).join('');
    }

    var tip = el('mwPieTip');
    Array.prototype.forEach.call(svg.querySelectorAll('.slice'), function (sl) {
        sl.addEventListener('mousemove', function (ev) {
            var q = S.quarters[parseInt(sl.getAttribute('data-i'), 10)];
            if (!q || !tip) return;
            tip.innerHTML = '<b>' + esc(quarterLabel(q)) + '</b><br>' + f2(q.amount);
            tip.style.display = 'block';
            var card = tip.parentElement.getBoundingClientRect();
            tip.style.left = (ev.clientX - card.left + 12) + 'px';
            tip.style.top  = (ev.clientY - card.top + 12) + 'px';
        });
        sl.addEventListener('mouseleave', function () { if (tip) tip.style.display = 'none'; });
    });
}

/* ---------------------------------------------------------------- toolbar */
window.mwRefresh = function (btn) {
    /* btnNew_Click :402-414 - the desktop clears the three combos and re-runs the report. */
    if (S.busy) return;
    setVal('cmbMonth', '0');
    setVal('cmbCategory', '0');
    setVal('cmbClassGroup', '0');
    clearMsg();
    mwShow(btn);
};

window.mwToggleFullscreen = function () {
    document.body.classList.toggle('mw-full');
    renderBar();
    renderPie();
};

/* ---------------------------------------------------------------- boot */
function boot() {
    window.addEventListener('resize', function () { renderBar(); renderPie(); });
    /* :117-121 - the desktop fills the lists and runs the report on load. */
    loadCombos().then(function () {
        var b = el('btnShow');
        if (b) mwShow(b);
    });
}

if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
else boot();

})();
