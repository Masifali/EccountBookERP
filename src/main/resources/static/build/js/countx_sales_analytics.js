/* ============================================================================
 * Sales Analytics Dashboard
 * Ported from Architecture.WinApp.Dashboard\SalesAnalytics.cs (523 lines).
 *
 *   :95-108   Load        From = financial year start, To = today, then Show
 *   :130-163  OrganizationTotalSaleComparisonByLocation()
 *   :199-209  btnshow_Click
 *   :211-238  GridSettings()  captions, hidden columns, formats, sums
 *   :79-93    KeyDown  Ctrl+P / Ctrl+E / Esc / Ctrl+N
 *
 * The chart is one series, so it carries no legend - the heading names it. Bars carry the
 * series hue; every label stays in text ink. Each bar has a hover tooltip, and the grid below
 * is the chart's table view.
 * ==========================================================================*/
(function () {
'use strict';

var API = '/api/dashboard/sales-analytics';

var S = { rows: [], busy: false };

/* ---------------------------------------------------------------- helpers */
function el(id)  { return document.getElementById(id); }
function val(id) { var e = el(id); return e ? e.value : ''; }
function setVal(id, v) { var e = el(id); if (e) e.value = v === null || v === undefined ? '' : v; }
function txt(v)  { return v === null || v === undefined ? '' : String(v); }
function esc(v)  { return txt(v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
                                .replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
function num(v)  { var n = parseFloat(v); return isNaN(n) ? 0 : n; }

/* GridSettings :224-227 - "#,#" is thousands with no decimals. */
function fInt(v) {
    return num(v).toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 0 });
}
/* GridSettings :228 - "#.##" is up to two decimals and NO thousands separator. */
function fPct(v) {
    var n = num(v);
    var s = (Math.round(n * 100) / 100).toString();
    return s;
}

function msg(text, ok) {
    var m = el('saMessage');
    if (!m) return;
    m.className = ok ? 'ok' : 'err';
    m.textContent = text;
    if (ok) setTimeout(function () { if (m.textContent === text) m.className = ''; }, 5000);
}
function clearMsg() { var m = el('saMessage'); if (m) { m.className = ''; m.textContent = ''; } }

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

/* ---------------------------------------------------------------- columns
 * GridSettings :211-238. OrgTotalAmount and AvgRate are hidden (:213-214), and the four
 * remaining measures are re-captioned.
 */
var COLS = [
    { k: 'LocationName',        c: 'Location',    fmt: null },
    { k: 'LocationAmountTotal', c: 'Value',       fmt: fInt, sum: true },
    { k: 'LocationQtyTotal',    c: 'Qty Sold',    fmt: fInt, sum: true },
    { k: 'LocationWeightTotal', c: 'Weight Sold', fmt: fInt, sum: true },
    { k: 'OrgLocationPrcnt',    c: 'Percentage',  fmt: fPct }
];

/* ---------------------------------------------------------------- load */
window.saShow = function (btn) {
    if (S.busy) return;
    clearMsg();
    busy(btn, true);

    var q = '?fromDate=' + encodeURIComponent(val('fromDate'))
          + '&toDate='   + encodeURIComponent(val('toDate'));

    getJson(API + '/by-location' + q).then(function (d) {
        busy(btn, false);
        if (d && d.error) msg(d.error, false);
        S.rows = (d && d.rows) || [];
        renderGrid();
        renderChart();
        var r = el('saRange');
        if (r) r.textContent = val('fromDate') + '  to  ' + val('toDate');
    }).catch(function (e) {
        busy(btn, false);
        msg('Sales analytics failed: ' + e.message, false);
    });
};

function renderGrid() {
    var head = el('saHead'), body = el('saBody'), foot = el('saFoot');
    if (!head || !body) return;

    head.innerHTML = COLS.map(function (c) {
        return '<th' + (c.fmt ? ' style="text-align:right;"' : '') + '>' + esc(c.c) + '</th>';
    }).join('');

    if (!S.rows.length) {
        body.innerHTML = '<tr><td colspan="' + COLS.length
            + '" style="text-align:center; padding:16px; color:#777;">No records.</td></tr>';
        if (foot) foot.innerHTML = '';
        var c0 = el('saRowCount'); if (c0) c0.textContent = '0';
        return;
    }

    body.innerHTML = S.rows.map(function (r) {
        return '<tr>' + COLS.map(function (c) {
            var v = r[c.k];
            return '<td class="' + (c.fmt ? 'num' : '') + '">'
                 + (c.fmt ? c.fmt(v) : esc(v)) + '</td>';
        }).join('') + '</tr>';
    }).join('');

    /* AggregateFunction 2 = Sum, on the three measures (:229-231). Percentage is NOT summed
       on the desktop and is not summed here. */
    if (foot) {
        foot.innerHTML = COLS.map(function (c) {
            if (!c.sum) return '<td></td>';
            var s = S.rows.reduce(function (a, r) { return a + num(r[c.k]); }, 0);
            return '<td class="num">' + fInt(s) + '</td>';
        }).join('');
    }
    var c1 = el('saRowCount'); if (c1) c1.textContent = S.rows.length;
}

/* ---------------------------------------------------------------- chart
 * ChartSalesAnalytics (:150-155): X = LocationName, Y = OrgLocationPrcnt, one series.
 *
 * Drawn as inline SVG so the page needs no charting library. One series means no legend;
 * the card heading names it. Bars are the only thing carrying the hue - every label uses text
 * ink. A 2px gap is left between adjacent bars, and the bars' top corners are rounded while
 * they stay anchored to the baseline.
 */
function renderChart() {
    var svg = el('saChart');
    if (!svg) return;

    var rows = S.rows;
    if (!rows.length) { svg.innerHTML = ''; svg.setAttribute('height', 0); return; }

    var barW = 54, gap = 18, padL = 54, padR = 16, padT = 18, padB = 64;
    var plotH = 240;
    var w = Math.max(padL + padR + rows.length * (barW + gap), 640);
    var h = plotH + padT + padB;

    var max = rows.reduce(function (a, r) { return Math.max(a, num(r.OrgLocationPrcnt)); }, 0);
    if (max <= 0) max = 1;
    /* A round-ish top so the axis reads cleanly. */
    var step = Math.pow(10, Math.floor(Math.log(max) / Math.LN10));
    var top = Math.ceil(max / (step / 2)) * (step / 2);
    if (!isFinite(top) || top <= 0) top = max;

    var parts = [];

    /* Recessive gridlines and their value ticks. */
    var lines = 4;
    for (var g = 0; g <= lines; g++) {
        var v = top * g / lines;
        var y = padT + plotH - (v / top) * plotH;
        parts.push('<line class="grid" x1="' + padL + '" y1="' + y + '" x2="' + (w - padR) + '" y2="' + y + '"/>');
        parts.push('<text class="tick" x="' + (padL - 6) + '" y="' + (y + 3) + '" text-anchor="end">'
                 + fPct(v) + '</text>');
    }
    /* Baseline. */
    parts.push('<line class="axis" x1="' + padL + '" y1="' + (padT + plotH)
             + '" x2="' + (w - padR) + '" y2="' + (padT + plotH) + '"/>');

    /* Direct value labels only when there are few enough bars to stay legible; otherwise the
       hover tooltip and the grid below carry the numbers. */
    var labelAll = rows.length <= 8;

    rows.forEach(function (r, i) {
        var v = num(r.OrgLocationPrcnt);
        var bh = Math.max((v / top) * plotH, v > 0 ? 2 : 0);
        var x = padL + gap / 2 + i * (barW + gap);
        var y = padT + plotH - bh;
        var rx = Math.min(4, bh);

        /* Rounded top, square foot on the baseline. */
        var d = 'M' + x + ',' + (padT + plotH)
              + 'V' + (y + rx)
              + 'q0,' + (-rx) + ' ' + rx + ',' + (-rx)
              + 'h' + (barW - 2 * rx)
              + 'q' + rx + ',0 ' + rx + ',' + rx
              + 'V' + (padT + plotH) + 'Z';
        parts.push('<path class="bar" d="' + d + '"/>');

        if (labelAll && v > 0) {
            parts.push('<text class="vallbl" x="' + (x + barW / 2) + '" y="' + (y - 5)
                     + '" text-anchor="middle">' + fPct(v) + '</text>');
        }

        /* Category label, wrapped onto a second line when it is long. */
        var name = txt(r.LocationName);
        var l1 = name, l2 = '';
        if (name.length > 12) {
            var cut = name.lastIndexOf(' ', 12);
            if (cut < 4) cut = 12;
            l1 = name.slice(0, cut);
            l2 = name.slice(cut).trim();
            if (l2.length > 12) l2 = l2.slice(0, 11) + '…';
        }
        parts.push('<text class="catlbl" x="' + (x + barW / 2) + '" y="' + (padT + plotH + 16)
                 + '" text-anchor="middle">' + esc(l1) + '</text>');
        if (l2) {
            parts.push('<text class="catlbl" x="' + (x + barW / 2) + '" y="' + (padT + plotH + 28)
                     + '" text-anchor="middle">' + esc(l2) + '</text>');
        }

        /* Hit target is the full column, taller and wider than the bar itself. */
        parts.push('<rect class="hit" x="' + (x - gap / 2) + '" y="' + padT
                 + '" width="' + (barW + gap) + '" height="' + plotH
                 + '" data-i="' + i + '"/>');
    });

    svg.setAttribute('width', w);
    svg.setAttribute('height', h);
    svg.setAttribute('viewBox', '0 0 ' + w + ' ' + h);
    svg.innerHTML = parts.join('');

    bindTooltip(svg);
}

function bindTooltip(svg) {
    var tip = el('saTip');
    if (!tip) return;
    Array.prototype.forEach.call(svg.querySelectorAll('.hit'), function (hit) {
        hit.addEventListener('mousemove', function (ev) {
            var r = S.rows[parseInt(hit.getAttribute('data-i'), 10)];
            if (!r) return;
            tip.innerHTML = '<b>' + esc(r.LocationName) + '</b><br>'
                          + 'Percentage: ' + fPct(r.OrgLocationPrcnt) + '<br>'
                          + 'Value: ' + fInt(r.LocationAmountTotal) + '<br>'
                          + 'Qty Sold: ' + fInt(r.LocationQtyTotal) + '<br>'
                          + 'Weight Sold: ' + fInt(r.LocationWeightTotal);
            tip.style.display = 'block';
            var card = tip.parentElement.getBoundingClientRect();
            tip.style.left = (ev.clientX - card.left + 12) + 'px';
            tip.style.top  = (ev.clientY - card.top + 12) + 'px';
        });
        hit.addEventListener('mouseleave', function () { tip.style.display = 'none'; });
    });
}

/* ---------------------------------------------------------------- toolbar */
window.saRefresh = function (btn) {
    /* btnNew_Click -> reset() -> btnshow_Click (:110-113, :125-128). It re-runs the report; it
       does not clear the dates. */
    if (S.busy) return;
    saShow(btn);
};

window.saToggleFullscreen = function () {
    document.body.classList.toggle('sa-full');
};

/* ---------------------------------------------------------------- boot */
function boot() {
    /* VoucherValidation_KeyDown, :79-93. Ctrl+P calls print_Click_1 -> ShowReport(), which is
       an empty method on the desktop, so it is bound and does nothing here too rather than
       inventing a print the desktop does not have. */
    document.addEventListener('keydown', function (e) {
        if (!e.ctrlKey && e.key !== 'Escape') return;
        var k = String(e.key || '').toLowerCase();
        if (e.ctrlKey && k === 'n') { e.preventDefault(); saRefresh(el('btnNew')); }
        if (e.ctrlKey && k === 'p') { e.preventDefault(); /* ShowReport() is empty on the desktop */ }
        if (e.key === 'Escape' || (e.ctrlKey && k === 'e')) {
            e.preventDefault();
            if (document.body.classList.contains('sa-full')) saToggleFullscreen();
        }
    });

    window.addEventListener('resize', function () { renderChart(); });

    getJson(API + '/defaults').then(function (d) {
        setVal('fromDate', (d && d.fromDate) || '');
        setVal('toDate',   (d && d.toDate) || '');
    }).catch(function () { /* the Show below will report any real failure */ })
      .then(function () {
        /* :102 - the desktop runs the report as soon as the form loads. */
        var b = el('btnShow');
        if (b) saShow(b);
      });
}

if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
else boot();

})();
