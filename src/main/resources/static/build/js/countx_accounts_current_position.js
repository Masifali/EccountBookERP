/* ============================================================================
 * Accounts Current Postition (Dashboard)
 * Ported from Architecture.WinApp.Account_Reports\AcFrmDashboard.cs (1,580 lines),
 * with AccountsDashboardCard, FcyPayablesReceivablesHeaderCard and
 * FcyPayablesReceivablesInfoCard.
 *
 *   :492-517  Load                             features, lists, then both card sets
 *   :533-568  cmbperemeter_SelectedIndexChanged
 *   :519-531  btnsearch_Click
 *   :316-354  GenerateCards                    Sp_ExectiveDashboard, ActivityId 1
 *   :194-265  GenerateFcyPayablesReceivablesCards
 *   :357-424  UserControl_Click                a card opens its account report
 *   :267-314  FcyPayablesRcvablesControl_Click
 * ==========================================================================*/
(function () {
'use strict';

var API = '/api/dashboard/accounts-position';

var S = { cards: [], fcy: [], branchFeature: false, busy: false };

/* ---------------------------------------------------------------- helpers */
function el(id)  { return document.getElementById(id); }
function val(id) { var e = el(id); return e ? e.value : ''; }
function setVal(id, v) { var e = el(id); if (e) e.value = v === null || v === undefined ? '' : v; }
function txt(v)  { return v === null || v === undefined ? '' : String(v); }
function esc(v)  { return txt(v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
                                .replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
function num(v)  { var n = parseFloat(v); return isNaN(n) ? 0 : n; }

/* AccountsDashboardCard formats every figure "#,#;(#,#);0" (:339-345) - thousands, no
   decimals, NEGATIVES IN PARENTHESES, a bare 0 for zero. */
function fInt(v) {
    var n = num(v);
    if (n === 0) return '0';
    var a = Math.abs(n).toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 0 });
    return n < 0 ? '(' + a + ')' : a;
}
/* The FCY cards use the global two-decimal formats, and the rate "#,##0.####" (:246). */
function f2(v) {
    return num(v).toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}
function f4(v) {
    return num(v).toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 4 });
}

function f(row, name) {
    if (!row) return null;
    if (row[name] !== undefined) return row[name];
    var lower = String(name).toLowerCase();
    for (var k in row) { if (k.toLowerCase() === lower) return row[k]; }
    return null;
}

function msg(text, ok) {
    var m = el('apMessage');
    if (!m) return;
    m.className = ok ? 'ok' : 'err';
    m.textContent = text;
    if (ok) setTimeout(function () { if (m.textContent === text) m.className = ''; }, 6000);
}
function clearMsg() { var m = el('apMessage'); if (m) { m.className = ''; m.textContent = ''; } }

function busy(btn, on) {
    S.busy = !!on;
    if (btn) { btn.disabled = !!on; btn.classList.toggle('btn-busy', !!on); }
    ['btnSearch', 'btnNew'].forEach(function (id) {
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
        if (d && d.companyError) msg('Companies: ' + d.companyError, false);
        if (d && d.branchError)  msg('Branches: ' + d.branchError, false);

        S.branchFeature = !!(d && d.branchFeature);

        setVal('fromDate', (d && d.fromDate) || '');
        setVal('toDate',   (d && d.toDate) || '');

        var comp = el('companies');
        if (comp) {
            comp.innerHTML = ((d && d.companies) || []).map(function (c) {
                return '<option value="' + esc(c.id) + '">' + esc(c.name) + '</option>';
            }).join('');
            /* :452-462 - only a head-office user gets the checked list; everyone else is fixed
               to their own company with the control disabled. The signed-in company is the one
               the server reports. */
            var mine = d && d.companyId;
            var headOffice = ((d && d.companies) || []).length > 1;
            if (!headOffice) {
                Array.prototype.forEach.call(comp.options, function (o) {
                    o.selected = (String(o.value) === String(mine));
                });
                comp.disabled = true;
            }
        }

        var br = el('branches');
        if (br) {
            br.innerHTML = ((d && d.branches) || []).map(function (b) {
                return '<option value="' + esc(f(b, 'BranchId')) + '">'
                     + esc(f(b, 'BranchName')) + '</option>';
            }).join('');
        }
        /* :499-504 - with the branch feature off the picker is removed entirely. */
        var wrap = el('branchWrap');
        if (wrap) wrap.style.display = S.branchFeature ? '' : 'none';

        if (window.jQuery && jQuery.fn.select2) {
            jQuery('.ap-select2').filter(function () { return !jQuery(this).data('select2'); })
                                 .select2({ width: '100%' });
        }
    }).catch(function (e) {
        msg('Setup could not be read: ' + e.message, false);
    });
}

/* cmbperemeter_SelectedIndexChanged, :533-568 - only choices 3 and 4 touch the To date. */
window.apParameterChanged = function () {
    var id = num(val('parameter')) || 1;
    getJson(API + '/parameter?id=' + id + '&toDate=' + encodeURIComponent(val('toDate')))
    .then(function (d) {
        setVal('fromDate', (d && d.fromDate) || '');
        setVal('toDate',   (d && d.toDate) || '');
    }).catch(function (e) { msg('Parameter failed: ' + e.message, false); });
};

function picked(id) {
    var sel = el(id);
    if (!sel) return '';
    var out = [];
    Array.prototype.forEach.call(sel.options, function (o) { if (o.selected) out.push(o.value); });
    return out.join(',');
}

/* ---------------------------------------------------------------- search */
window.apSearch = function (btn) {
    if (S.busy) return;
    clearMsg();
    busy(btn, true);

    var q = '?fromDate=' + encodeURIComponent(val('fromDate'))
          + '&toDate='   + encodeURIComponent(val('toDate'))
          + '&companyIds=' + encodeURIComponent(picked('companies'))
          + '&branchIds='  + encodeURIComponent(S.branchFeature ? picked('branches') : '');

    var empty = el('apEmpty');
    if (empty) { empty.style.display = ''; empty.textContent = 'Loading...'; }

    /* :514-515 - the desktop builds both card sets from the same click. */
    Promise.all([
        getJson(API + '/cards' + q).catch(function (e) { return { error: e.message }; }),
        getJson(API + '/fcy' + q).catch(function (e) { return { error: e.message }; })
    ]).then(function (res) {
        busy(btn, false);
        var a = res[0] || {}, b = res[1] || {};
        if (a.error) msg('Accounts: ' + a.error, false);
        if (b.error) msg('FCY: ' + b.error, false);

        S.cards = a.cards || [];
        S.fcy   = b.cards || [];
        renderCards();
        renderFcy();

        if (empty) {
            empty.style.display = S.cards.length ? 'none' : '';
            empty.textContent = 'No account balances for this period.';
        }
    });
};

function renderCards() {
    var wrap = el('apCards');
    if (!wrap) return;
    if (!S.cards.length) { wrap.innerHTML = ''; return; }
    wrap.innerHTML = S.cards.map(function (c, i) {
        function row(label, v, cls) {
            var n = num(v);
            return '<div class="ap-row' + (cls ? ' ' + cls : '') + '"><span>' + esc(label)
                 + '</span><span' + (n < 0 ? ' class="neg"' : '') + '>' + fInt(v) + '</span></div>';
        }
        /* DiffStatus is the desktop's own text; it is shown as text, not encoded in colour. */
        var status = txt(c.diffStatus);
        return '<div class="ap-card" onclick="apCardClick(' + i + ')">'
             + '<div class="t">' + esc(c.title) + '</div>'
             + '<div class="b">'
             + row('Opening', c.opening)
             + row('Current Dr', c.currDr)
             + row('Current Cr', c.currCr)
             + row('Difference', c.diffAmount)
             + row('Closing', c.closing, 'closing')
             + '</div>'
             + (status ? '<div class="ap-status"><i></i>' + esc(status) + '</div>' : '')
             + '</div>';
    }).join('');
}

function renderFcy() {
    var wrap = el('apFcy'), sect = el('apFcySect');
    if (!wrap) return;
    /* :210 and :258-262 - the heading appears only when there are rows. */
    var show = S.fcy.length > 0;
    if (sect) sect.style.display = show ? '' : 'none';
    if (!show) { wrap.innerHTML = ''; return; }

    wrap.innerHTML = S.fcy.map(function (c, i) {
        var lines = (c.lines || []).map(function (l) {
            return '<tr><td>' + esc(l.currencyCode) + '</td>'
                 + '<td class="num">' + f2(l.fcyAmount) + '</td>'
                 + '<td class="num">' + f4(l.lastExRate) + '</td>'
                 + '<td class="num">' + f2(l.lcyAmount) + '</td></tr>';
        }).join('');
        return '<div class="ap-fcard" onclick="apFcyClick(' + i + ')">'
             + '<div class="t">' + esc(c.typeName) + '</div>'
             + '<table class="ap-grid">'
             + '<thead><tr><th style="text-align:left;">Currency</th><th>FCY Amount</th>'
             + '<th>Rate</th><th>PKR Amount</th></tr></thead>'
             + '<tbody>' + lines + '</tbody>'
             + '<tfoot><tr><td>Total</td><td class="num">' + f2(c.totalFcy) + '</td>'
             + '<td></td><td class="num">' + f2(c.totalLcy) + '</td></tr></tfoot>'
             + '</table></div>';
    }).join('');
}

/* UserControl_Click :357-424 opens the account report behind the card, and
   FcyPayablesRcvablesControl_Click :267-314 re-reads the FCY figures for one type. Neither of
   those target screens is ported, so the card reports what it stands for rather than opening
   something unrelated. */
window.apCardClick = function (i) {
    var c = S.cards[i];
    if (!c) return;
    msg('"' + txt(c.title) + '" (SortingNo ' + num(c.id) + ') opens its account report on the '
      + 'desktop. That screen has not been ported yet, so nothing was opened.', false);
};

window.apFcyClick = function (i) {
    var c = S.fcy[i];
    if (!c) return;
    msg('"' + txt(c.typeName) + '" (TypeId ' + num(c.typeId) + ') opens the FCY payables and '
      + 'receivables detail on the desktop. That screen has not been ported yet.', false);
};

/* ---------------------------------------------------------------- toolbar */
window.apRefresh = function (btn) {
    if (S.busy) return;
    busy(btn, true);
    clearMsg();
    loadSetup().then(function () {
        busy(btn, false);
        var b = el('btnSearch');
        if (b) apSearch(b);
    });
};

window.apToggleFullscreen = function () {
    document.body.classList.toggle('ap-full');
};

/* ---------------------------------------------------------------- boot */
function boot() {
    /* :512-515 - the desktop builds both card sets as soon as the form loads. */
    loadSetup().then(function () {
        var b = el('btnSearch');
        if (b) apSearch(b);
    });
}

if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
else boot();

})();
