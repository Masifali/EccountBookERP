/* ============================================================================
 * Sales Comparison (Graph) Report
 * Ported from Architecture.WinApp.Graph\SalesComparisonReportWithGraph.cs (1,582 lines).
 *
 *   :157-200  Load          dates, Top rows = 10, the three lists, then ShowReport()
 *   :145-155  btnshow_Click
 *   :280-540  ShowReport    one procedure, one flat table split four ways on DescriptionTitle
 *
 * Each of the four blocks is one series, so none carries a legend - its heading names it - and
 * every bar has a hover tooltip. The grid under each chart is that chart's table view.
 * ==========================================================================*/
(function () {
'use strict';

var API = '/api/dashboard/sales-comparison';

var S = {
    customers: [], items: [], cities: [], packSizes: [],
    appId: 0, lockCostCenter: false, busy: false
};

/* The four blocks, with the name column each grid captions (:334-338, :389, :443, :497). */
var BLOCKS = [
    { key: 'customers', suffix: 'Customers', nameCol: 'CustomerName', title: 'Customers' },
    { key: 'items',     suffix: 'Items',     nameCol: 'ItemName',     title: 'Items' },
    { key: 'cities',    suffix: 'Cities',    nameCol: 'CityName',     title: 'Cities' },
    { key: 'packSizes', suffix: 'PackSizes', nameCol: 'PackUom',      title: 'Pack Size' }
];

/* ---------------------------------------------------------------- helpers */
function el(id)  { return document.getElementById(id); }
function val(id) { var e = el(id); return e ? e.value : ''; }
function setVal(id, v) { var e = el(id); if (e) e.value = v === null || v === undefined ? '' : v; }
function txt(v)  { return v === null || v === undefined ? '' : String(v); }
function esc(v)  { return txt(v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
                                .replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
function num(v)  { var n = parseFloat(v); return isNaN(n) ? 0 : n; }

/* The grids use "#,##0.##" throughout (:348-361). */
function f2(v) {
    return num(v).toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 2 });
}
function fAxis(v) {
    var n = Math.abs(num(v));
    if (n >= 1e9) return (num(v) / 1e9).toFixed(1) + 'B';
    if (n >= 1e6) return (num(v) / 1e6).toFixed(1) + 'M';
    if (n >= 1e3) return (num(v) / 1e3).toFixed(0) + 'K';
    return String(Math.round(num(v)));
}

function msg(text, ok) {
    var m = el('scMessage');
    if (!m) return;
    m.className = ok ? 'ok' : 'err';
    m.textContent = text;
    if (ok) setTimeout(function () { if (m.textContent === text) m.className = ''; }, 5000);
}
function clearMsg() { var m = el('scMessage'); if (m) { m.className = ''; m.textContent = ''; } }

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

/* ---------------------------------------------------------------- setup */
function loadSetup() {
    return getJson(API + '/setup').then(function (d) {
        if (d && d.listError) msg('Filter lists: ' + d.listError, false);
        if (d && d.costCenterError) msg('Cost centres: ' + d.costCenterError, false);

        S.appId = num(d && d.appId);
        S.lockCostCenter = !!(d && d.lockCostCenter);

        setVal('fromDate', (d && d.fromDate) || '');
        setVal('toDate',   (d && d.toDate) || '');
        setVal('topRows',  (d && d.topRows) || 10);

        fill('cmbCategory',   (d && d.categories) || [],  'Parent Category');
        fill('cmbItemType',   (d && d.itemTypes) || [],   'Item Type');
        fill('cmbCostCenter', (d && d.costCenters) || [], 'Cost Center');

        /* :181-186 - for AppId 5 the first cost centre is selected and the combo is locked. */
        if (S.lockCostCenter) {
            var sel = el('cmbCostCenter');
            if (sel && sel.options.length > 1) sel.selectedIndex = 1;
            if (sel) sel.disabled = true;
        }
    }).catch(function (e) {
        msg('Setup could not be read: ' + e.message, false);
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
window.scShow = function (btn) {
    if (S.busy) return;
    clearMsg();
    busy(btn, true);

    var topRows = num(val('topRows')) || 0;
    var flag = (el('radBottom') && el('radBottom').checked) ? 'Bottom' : 'Top';

    var q = '?fromDate=' + encodeURIComponent(val('fromDate'))
          + '&toDate='   + encodeURIComponent(val('toDate'))
          + '&topRows='  + topRows
          + '&orderByFlag=' + encodeURIComponent(flag)
          + '&categoryId='   + (num(val('cmbCategory')) || 0)
          + '&itemTypeId='   + (num(val('cmbItemType')) || 0)
          + '&costCenterId=' + (num(val('cmbCostCenter')) || 0);

    getJson(API + '/report' + q).then(function (d) {
        busy(btn, false);
        if (d && d.error) {
            /* Includes the desktop's own "Cost Center Not Found" (:303-306). */
            msg(d.error, false);
            S.customers = []; S.items = []; S.cities = []; S.packSizes = [];
        } else {
            S.customers = (d && d.customers) || [];
            S.items     = (d && d.items) || [];
            S.cities    = (d && d.cities) || [];
            S.packSizes = (d && d.packSizes) || [];
        }
        renderAll(topRows, flag);
    }).catch(function (e) {
        busy(btn, false);
        msg('Report failed: ' + e.message, false);
    });
};

function renderAll(topRows, flag) {
    BLOCKS.forEach(function (b) {
        /* :325 - the chart title is "Top N <thing>", built from the Top-rows box. */
        var head = el('hd' + b.suffix);
        if (head) head.textContent = flag + ' ' + topRows + ' ' + b.title;
        renderGrid(b);
        renderChart(b);
    });
}

function renderGrid(b) {
    var rows = S[b.key] || [];
    var head = el('th' + b.suffix), body = el('tb' + b.suffix), foot = el('tf' + b.suffix);
    if (!head || !body) return;

    head.innerHTML = '<th>' + esc(b.nameCol) + '</th>'
                   + '<th style="text-align:right;">SaleQty</th>'
                   + '<th style="text-align:right;">SaleWeight</th>'
                   + '<th style="text-align:right;">SaleAmount</th>'
                   + '<th style="text-align:right;">%OfTop</th>';

    if (!rows.length) {
        body.innerHTML = '<tr><td colspan="5" class="sc-empty">No rows.</td></tr>';
        if (foot) foot.innerHTML = '';
        return;
    }

    body.innerHTML = rows.map(function (r) {
        return '<tr><td>' + esc(r.name) + '</td>'
             + '<td class="num">' + f2(r.saleQty) + '</td>'
             + '<td class="num">' + f2(r.saleWeight) + '</td>'
             + '<td class="num">' + f2(r.saleAmount) + '</td>'
             + '<td class="num">' + f2(r.pctOfTop) + '</td></tr>';
    }).join('');

    /* All four measures carry AggregateFunction 2 - Sum (:346-361). */
    if (foot) {
        function sum(k) { return rows.reduce(function (a, r) { return a + num(r[k]); }, 0); }
        foot.innerHTML = '<td>Total</td>'
                       + '<td class="num">' + f2(sum('saleQty')) + '</td>'
                       + '<td class="num">' + f2(sum('saleWeight')) + '</td>'
                       + '<td class="num">' + f2(sum('saleAmount')) + '</td>'
                       + '<td class="num">' + f2(sum('pctOfTop')) + '</td>';
    }
}

/* X = GroupTitle, Y = SaleAmount, ChartType Column, X gridlines off and Y gridlines dotted
   (:322-333). One series per block, so no legend. */
function renderChart(b) {
    var svg = el('ch' + b.suffix);
    if (!svg) return;
    var rows = S[b.key] || [];
    if (!rows.length) { svg.innerHTML = ''; svg.setAttribute('height', 0); return; }

    var barW = 40, gap = 14, padL = 58, padR = 12, padT = 12, padB = 52;
    var plotH = 180;
    var w = Math.max(padL + padR + rows.length * (barW + gap), 500);
    var h = plotH + padT + padB;

    var max = rows.reduce(function (a, r) { return Math.max(a, num(r.saleAmount)); }, 0);
    if (max <= 0) max = 1;
    var step = Math.pow(10, Math.floor(Math.log(max) / Math.LN10));
    var top = Math.ceil(max / (step / 2)) * (step / 2);
    if (!isFinite(top) || top <= 0) top = max;

    var parts = [];
    for (var g = 0; g <= 4; g++) {
        var v = top * g / 4;
        var y = padT + plotH - (v / top) * plotH;
        parts.push('<line class="grid" x1="' + padL + '" y1="' + y + '" x2="' + (w - padR)
                 + '" y2="' + y + '"/>');
        parts.push('<text class="tick" x="' + (padL - 6) + '" y="' + (y + 3)
                 + '" text-anchor="end">' + fAxis(v) + '</text>');
    }
    parts.push('<line class="axis" x1="' + padL + '" y1="' + (padT + plotH)
             + '" x2="' + (w - padR) + '" y2="' + (padT + plotH) + '"/>');

    rows.forEach(function (r, i) {
        var v = num(r.saleAmount);
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

        /* Names here are long, so the label is rotated rather than overlapping its neighbour. */
        var name = txt(r.name);
        var shown = name.length > 18 ? name.slice(0, 17) + '…' : name;
        var lx = x + barW / 2, ly = padT + plotH + 8;
        parts.push('<text class="catlbl" transform="rotate(-40 ' + lx + ' ' + ly + ')" x="' + lx
                 + '" y="' + ly + '" text-anchor="end">' + esc(shown) + '</text>');

        parts.push('<rect class="hit" x="' + (x - gap / 2) + '" y="' + padT
                 + '" width="' + (barW + gap) + '" height="' + plotH + '" data-i="' + i + '"/>');
    });

    svg.setAttribute('width', w);
    svg.setAttribute('height', h);
    svg.setAttribute('viewBox', '0 0 ' + w + ' ' + h);
    svg.innerHTML = parts.join('');

    var tip = el('tip' + b.suffix);
    Array.prototype.forEach.call(svg.querySelectorAll('.hit'), function (hit) {
        hit.addEventListener('mousemove', function (ev) {
            var r = (S[b.key] || [])[parseInt(hit.getAttribute('data-i'), 10)];
            if (!r || !tip) return;
            tip.innerHTML = '<b>' + esc(r.name) + '</b><br>'
                          + 'SaleAmount: ' + f2(r.saleAmount) + '<br>'
                          + 'SaleQty: ' + f2(r.saleQty) + '<br>'
                          + 'SaleWeight: ' + f2(r.saleWeight) + '<br>'
                          + '%OfTop: ' + f2(r.pctOfTop);
            tip.style.display = 'block';
            var card = tip.parentElement.getBoundingClientRect();
            tip.style.left = (ev.clientX - card.left + 12) + 'px';
            tip.style.top  = (ev.clientY - card.top + 12) + 'px';
        });
        hit.addEventListener('mouseleave', function () { if (tip) tip.style.display = 'none'; });
    });
}

/* ---------------------------------------------------------------- toolbar */
window.scRefresh = function (btn) {
    if (S.busy) return;
    busy(btn, true);
    clearMsg();
    loadSetup().then(function () {
        busy(btn, false);
        var b = el('btnShow');
        if (b) scShow(b);
    });
};

window.scToggleFullscreen = function () {
    document.body.classList.toggle('sc-full');
    BLOCKS.forEach(renderChart);
};

/* ---------------------------------------------------------------- boot */
function boot() {
    window.addEventListener('resize', function () { BLOCKS.forEach(renderChart); });
    /* :188 - the desktop runs the report as soon as the form loads. */
    loadSetup().then(function () {
        var b = el('btnShow');
        if (b) scShow(b);
    });
}

if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
else boot();

})();
