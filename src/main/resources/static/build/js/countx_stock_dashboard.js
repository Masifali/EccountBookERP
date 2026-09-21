/* ============================================================================
 * Stock DashBoard
 * Ported from Architecture.WinApp.Dashboard\frmStockDashboard.cs (2,938 lines).
 *
 *   :379-393    frmPendingWorksRpt_Load   (the Load handler was never renamed)
 *   :395-420    BranchesFill
 *   :422-708    GridBind                  one read, six buckets summed on the client
 *   :710-804    btnRefresh_Click -> Reset
 *   :806-816    btnsearch_Click
 *   :818-1038   linkLabel1..6_LinkClicked
 *
 * Dates are plain yyyy-mm-dd strings throughout - never toISOString(), which shifts a day at
 * UTC+5.
 * ==========================================================================*/
(function () {
'use strict';

var API = '/api/dashboard/stock';

var S = { panels: [], busy: false, branchFeature: false };

/* ---------------------------------------------------------------- helpers */
function el(id)  { return document.getElementById(id); }
function val(id) { var e = el(id); return e ? e.value : ''; }
function setVal(id, v) { var e = el(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
function chk(id) { var e = el(id); return !!(e && e.checked); }
function txt(v)  { return (v === null || v === undefined) ? '' : String(v); }
function esc(v)  { return txt(v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
                                .replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
function num(v)  { var n = parseFloat(v); return isNaN(n) ? 0 : n; }

/* Every figure on this screen uses "#,#;(#,#);0" (:627 onwards) - thousands, NO decimals,
   NEGATIVES IN PARENTHESES, and a bare 0 at exactly zero. */
function fNum(v) {
    var n = num(v);
    if (n === 0) return '0';
    var a = Math.abs(n).toLocaleString(undefined, { maximumFractionDigits: 0 });
    return n < 0 ? '(' + a + ')' : a;
}

function msg(text, ok) {
    var m = el('sdMessage');
    if (!m) return;
    m.className = ok ? 'ok' : 'err';
    m.textContent = text;
    if (ok) setTimeout(function () { if (m.textContent === text) m.className = ''; }, 6000);
}
function clearMsg() { var m = el('sdMessage'); if (m) { m.className = ''; m.textContent = ''; } }

function busy(btn, on) {
    S.busy = !!on;
    if (btn) { btn.disabled = !!on; btn.classList.toggle('btn-busy', !!on); }
    ['btnShow', 'btnRefresh'].forEach(function (id) {
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

function pickedIds(id) {
    var sel = el(id);
    if (!sel) return '';
    var out = [];
    Array.prototype.forEach.call(sel.options, function (o) {
        if (o.selected && txt(o.value) && txt(o.value) !== '0') out.push(o.value);
    });
    return out.join(',');
}

/* ---------------------------------------------------------------- setup */
function loadSetup() {
    return getJson(API + '/setup').then(function (d) {
        d = d || {};
        if (d.branchError) msg('Branches: ' + d.branchError, false);

        S.branchFeature = !!d.branchFeature;
        setVal('fromDate', d.fromDate);
        setVal('toDate',   d.toDate);

        var wrap = el('branchWrap');
        if (wrap) wrap.style.display = S.branchFeature ? '' : 'none';
        if (S.branchFeature) {
            var sel = el('branches');
            if (sel) {
                /* DB-driven and searchable; nothing here is hard-coded. */
                sel.innerHTML = (d.branches || []).map(function (b) {
                    return '<option value="' + esc(b.id) + '">' + esc(b.name) + '</option>';
                }).join('');
            }
            if (window.jQuery && jQuery.fn.select2) {
                jQuery('.sd-select2').each(function () {
                    var $s = jQuery(this);
                    if ($s.data('select2')) $s.select2('destroy');
                    $s.select2({ width: '100%', dropdownAutoWidth: true });
                });
            }
        }
    }).catch(function (e) {
        msg('Setup could not be read: ' + e.message, false);
    });
}

/* ---------------------------------------------------------------- show */
window.sdShow = function (btn) {
    if (S.busy) return;
    clearMsg();
    busy(btn, true);

    var q = '?fromDate=' + encodeURIComponent(val('fromDate'))
          + '&toDate='   + encodeURIComponent(val('toDate'))
          + '&saleValue=' + chk('rdSaleValue')
          + '&branchIds=' + encodeURIComponent(S.branchFeature ? pickedIds('branches') : '');

    getJson(API + '/report' + q).then(function (d) {
        busy(btn, false);
        d = d || {};
        if (d.error) msg(d.error, false);
        S.panels = d.panels || [];
        render();
    }).catch(function (e) {
        busy(btn, false);
        msg('Show failed: ' + e.message, false);
        S.panels = [];
        render();
    });
};

/* ---------------------------------------------------------------- panels */
function render() {
    var wrap = el('sdPanels');
    if (!wrap) return;
    if (!S.panels.length) { wrap.innerHTML = ''; return; }

    wrap.innerHTML = S.panels.map(function (p, i) {
        function cell(v) {
            var n = num(v);
            return '<td' + (n < 0 ? ' class="sd-neg"' : '') + '>' + fNum(v) + '</td>';
        }
        function row(label, op, inn, out, bal) {
            return '<tr><td>' + label + '</td>'
                 + cell(p[op]) + cell(p[inn]) + cell(p[out]) + cell(p[bal]) + '</tr>';
        }
        return '<div class="col-lg-6 col-xl-4"><div class="sd-panel">'
             + '<div class="sd-head"><a onclick="sdOpenReport(' + i + ')" '
             + 'title="Stock Report With Values">' + esc(p.title) + '</a></div>'
             + '<div class="sd-body"><table class="sd-grid">'
             + '<thead><tr><th>Description</th><th>Opening</th><th>In</th><th>Out</th>'
             + '<th>Balance</th></tr></thead><tbody>'
             + row('Qty',    'opQty',    'inQty',    'outQty',    'balQty')
             + row('Weight', 'opWeight', 'inWeight', 'outWeight', 'balWeight')
             + row('Amount', 'opAmount', 'inAmount', 'outAmount', 'balAmount')
             + '</tbody></table></div></div></div>';
    }).join('');
}

/**
 * linkLabel1..6_LinkClicked, :818-1038.
 *
 * Each heading opens frmStockReportWithValues with its own pair fixed - cmbMainType and
 * ParentCategoryIds take the panel's parent category, CmbClassGroup its class group - plus the
 * two dates and, when the branch feature is on, the chosen branch list. That screen is not
 * ported, so the link reports exactly which report it would open rather than opening something
 * unrelated.
 */
window.sdOpenReport = function (i) {
    var p = S.panels[i];
    if (!p) return;
    msg('"' + txt(p.title) + '" opens Stock Report With Values on the desktop for '
      + val('fromDate') + ' to ' + val('toDate')
      + ', Class Group ' + num(p.classGroupId)
      + ' and Parent Category ' + num(p.parentCategoryId)
      + '. That screen is not ported yet, so nothing was opened.', false);
};

/* ---------------------------------------------------------------- toolbar */

/* btnRefresh_Click -> Reset, :710-804 - every figure back to zero. It does NOT re-read. */
window.sdRefresh = function (btn) {
    if (S.busy) return;
    busy(btn, true);
    clearMsg();
    try {
        S.panels = S.panels.map(function (p) {
            var z = { key: p.key, title: p.title,
                      classGroupId: p.classGroupId, parentCategoryId: p.parentCategoryId };
            ['opQty', 'inQty', 'outQty', 'balQty',
             'opWeight', 'inWeight', 'outWeight', 'balWeight',
             'opAmount', 'inAmount', 'outAmount', 'balAmount'].forEach(function (k) { z[k] = 0; });
            return z;
        });
        render();
    } finally {
        busy(btn, false);
    }
};

window.sdToggleFullscreen = function () {
    document.body.classList.toggle('sd-full');
};

document.addEventListener('keydown', function (e) {
    if (e.key === 'Escape' && document.body.classList.contains('sd-full')) {
        e.preventDefault();
        document.body.classList.remove('sd-full');
    }
});

/* ---------------------------------------------------------------- boot */
function boot() {
    /* :387 - the desktop calls GridBind() during Load, so the screen arrives already filled. */
    loadSetup().then(function () {
        var b = el('btnShow');
        if (b) sdShow(b);
    });
}

if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
else boot();

})();
