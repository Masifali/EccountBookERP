/* ============================================================================
 * Purchase & Sales Pending Works (Dashboard)
 * the whole "Followups && Pending Works DashBoards" module
 * Ported from Architecture.WinApp.Dashboard\frmPendingWorksRpt.cs (753 lines)
 * and its drill-down frmPendingWorksDetail.cs.
 *
 *   :93-109   Load        -> ERP feature 11, BranchesFill(), AllCardsBind()
 *   :111-136  BranchesFill  USP_GetBranchsAllocatedToUser, a CHECKED list (multi-select)
 *   :139-282  AllCardsBind  Sp_Dashboards_PendingWork @ReportType='FiguresOnly', split into
 *                           five panels by PendingWorkDepartment and InSeparatePanel
 *   :284-337  UserControl_Click  three special cases, otherwise the detail window
 *   :339-349  btnshow_Click
 *   :376-379  datToDate_ValueChanged -> From follows To
 * ==========================================================================*/
(function () {
'use strict';

var API = '/api/dashboard/pending-works';

var S = {
    multiBranch: false,
    branches: [],       // { BranchId, BranchName }
    busy: false,
    loaded: false
};

/* ---------------------------------------------------------------- helpers */
function el(id)  { return document.getElementById(id); }
function val(id) { var e = el(id); return e ? e.value : ''; }
function setVal(id, v) { var e = el(id); if (e) e.value = v === null || v === undefined ? '' : v; }
function txt(v)  { return v === null || v === undefined ? '' : String(v); }
function esc(v)  { return txt(v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
                                .replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
function num(v)  { var n = parseFloat(v); return isNaN(n) ? 0 : n; }
function f2(v)   { return num(v).toLocaleString(undefined,
                          { minimumFractionDigits: 2, maximumFractionDigits: 2 }); }

/* Dates stay as yyyy-mm-dd strings; nothing goes through toISOString, which at UTC+5 would
   shift a local midnight to the previous day. */
function ymd(v) {
    if (!v) return '';
    var s = String(v);
    var m = s.match(/^(\d{4})-(\d{2})-(\d{2})/);
    if (m) return m[1] + '-' + m[2] + '-' + m[3];
    return s;
}
function today() {
    var d = new Date();
    return d.getFullYear() + '-' + ('0' + (d.getMonth() + 1)).slice(-2)
                           + '-' + ('0' + d.getDate()).slice(-2);
}
function f(row, name) {
    if (!row) return null;
    if (row[name] !== undefined) return row[name];
    var lower = String(name).toLowerCase();
    for (var k in row) { if (k.toLowerCase() === lower) return row[k]; }
    return null;
}

function msg(text, ok) {
    var m = el('pwMessage');
    if (!m) return;
    m.className = ok ? 'ok' : 'err';
    m.textContent = text;
    if (ok) setTimeout(function () { if (m.textContent === text) m.className = ''; }, 6000);
}
function clearMsg() { var m = el('pwMessage'); if (m) { m.className = ''; m.textContent = ''; } }

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

/* ---------------------------------------------------------------- setup */
function loadSetup() {
    return getJson(API + '/setup').then(function (d) {
        S.multiBranch = !!(d && d.multiBranch);
        S.branches = (d && d.branches) || [];

        var sel = el('branches');
        if (sel) {
            sel.innerHTML = S.branches.map(function (b) {
                return '<option value="' + esc(f(b, 'BranchId')) + '">'
                     + esc(f(b, 'BranchName')) + '</option>';
            }).join('');
        }
        /* AllCardsBind :176 - when the multi-branch feature is off the ids are sent as ""
           whatever the combo shows, so the control is hidden rather than left misleading. */
        var wrap = el('branchWrap');
        if (wrap) wrap.style.display = S.multiBranch ? '' : 'none';

        if (d && d.branchError) msg('Branches could not be read: ' + d.branchError, false);

        setVal('toDate', (d && d.toDate) || today());
        setVal('fromDate', (d && d.fromDate) || today());

        if (window.jQuery && jQuery.fn.select2 && sel) {
            jQuery(sel).filter(function () { return !jQuery(this).data('select2'); })
                       .select2({ width: '100%', placeholder: 'Branch Name' });
        }
    }).catch(function (e) {
        msg('Setup could not be read: ' + e.message, false);
    });
}

/* datToDate_ValueChanged, :376-379 - From follows To. Reproduced as written. */
window.pwToDateChanged = function () {
    setVal('fromDate', val('toDate'));
};

function branchIds() {
    if (!S.multiBranch) return '';
    var sel = el('branches');
    if (!sel) return '';
    var out = [];
    Array.prototype.forEach.call(sel.selectedOptions || [], function (o) { out.push(o.value); });
    /* The desktop builds ",id,id" - a leading comma from its own loop (:170). Kept the same so
       the procedure sees the same string it does. */
    return out.length ? ',' + out.join(',') : '';
}

function typeId() {
    return (el('rdPmOthers') && el('rdPmOthers').checked) ? 2 : 1;
}

/* ---------------------------------------------------------------- cards */
window.pwShow = function (btn) {
    if (S.busy) return;
    clearMsg();
    busy(btn, true);

    var q = '?fromDate=' + encodeURIComponent(val('fromDate'))
          + '&toDate='   + encodeURIComponent(val('toDate'))
          + '&typeId='   + typeId()
          + '&branchIds=' + encodeURIComponent(branchIds());

    getJson(API + '/cards' + q).then(function (d) {
        busy(btn, false);
        S.loaded = true;
        if (d && d.error) { msg(d.error, false); }
        renderGroup('pwPurchase',         (d && d.purchase) || [],         false);
        renderGroup('pwPurchaseSeparate', (d && d.purchaseSeparate) || [], true);
        renderGroup('pwSale',             (d && d.sale) || [],             false);
        renderGroup('pwSaleSeparate',     (d && d.saleSeparate) || [],     true);
        renderGroup('pwExport',           (d && d.export) || [],           false);

        var total = ((d && d.purchase) || []).length + ((d && d.purchaseSeparate) || []).length
                  + ((d && d.sale) || []).length + ((d && d.saleSeparate) || []).length
                  + ((d && d.export) || []).length;
        var empty = el('pwEmpty');
        if (empty) {
            empty.style.display = total ? 'none' : '';
            empty.textContent = 'No pending works for this date range.';
        }
    }).catch(function (e) {
        busy(btn, false);
        msg('Pending works failed: ' + e.message, false);
    });
};

function renderGroup(id, cards, separate) {
    var wrap = el(id);
    if (!wrap) return;
    if (!cards.length) { wrap.innerHTML = ''; return; }
    wrap.innerHTML = cards.map(function (c, i) {
        var zero = num(c.count) <= 0;
        return '<div class="pw-card' + (separate ? ' sep' : '') + (zero ? ' zero' : '') + '"'
             + (zero ? '' : ' onclick="pwOpenDetail(\'' + esc(id) + '\',' + i + ')"')
             + ' title="' + esc(c.department) + '">'
             + '<div class="t">' + esc(c.title) + '</div>'
             + '<div class="n">' + esc(c.count) + '</div>'
             + '<div class="c">' + esc(c.caption) + '</div>'
             + '</div>';
    }).join('');
    wrap.setAttribute('data-cards', JSON.stringify(cards));
}

/* ---------------------------------------------------------------- detail
 * UserControl_Click, :284-337.
 *
 * Three pending-work names do NOT open the detail window on the desktop - they open their own
 * forms instead. Neither of those forms has been ported, so rather than open something
 * unrelated the card says which desktop form it belongs to:
 *
 *   "Gate Purchase Rate Not Applied"                      LabPurchaseAnalysisForApproval.cs
 *   "Purchase Order WeightCut Not Applied"                LabPurchaseAnalysisForApproval.cs
 *   "Vehicles Pending Clearance And Not Fully Forwarded"  frmGatePassForStockInTrust.cs
 */
var SPECIAL = {
    'Gate Purchase Rate Not Applied': 'LabPurchaseAnalysisForApproval.cs',
    'Purchase Order WeightCut Not Applied': 'LabPurchaseAnalysisForApproval.cs',
    'Vehicles Pending Clearance And Not Fully Forwarded': 'frmGatePassForStockInTrust.cs'
};

window.pwOpenDetail = function (groupId, i) {
    var wrap = el(groupId);
    if (!wrap) return;
    var cards;
    try { cards = JSON.parse(wrap.getAttribute('data-cards') || '[]'); } catch (e) { cards = []; }
    var c = cards[i];
    if (!c) return;
    if (num(c.count) <= 0) return;      // :292 - a zero card does nothing

    if (SPECIAL[c.title] && c.department === 'Purchase Department') {
        msg('"' + c.title + '" opens the desktop form ' + SPECIAL[c.title]
          + ', which has not been ported yet. Nothing else was opened.', false);
        return;
    }

    var title = el('pwModalTitle');
    if (title) title.textContent = c.title + '  -  ' + c.department;
    var cnt = el('pwModalCount'); if (cnt) cnt.textContent = '';
    renderDetail([], 'Loading...');
    el('pwModal').style.display = 'block';

    var q = '?fromDate=' + encodeURIComponent(val('fromDate'))
          + '&toDate='   + encodeURIComponent(val('toDate'))
          + '&typeId='   + typeId()
          + '&branchIds=' + encodeURIComponent(branchIds())
          + '&department=' + encodeURIComponent(c.department)
          + '&pendingWorkName=' + encodeURIComponent(c.title);

    getJson(API + '/detail' + q).then(function (d) {
        if (d && d.error) { renderDetail([], d.error); return; }
        var rows = (d && d.rows) || [];
        renderDetail(rows, rows.length ? null : 'No detail rows.');
        if (cnt) cnt.textContent = rows.length + ' rows';
    }).catch(function (e) {
        renderDetail([], e.message);
    });
};

/* The detail columns differ per pending-work type - frmPendingWorksDetail builds a different
   DataTable per case - so whatever the procedure returns is shown, which is the same behaviour
   as the desktop's RetrieveStructure() off its own DataTable. */
function renderDetail(rows, note) {
    var head = el('pwDetailHead'), body = el('pwDetailBody');
    if (!head || !body) return;
    if (!rows.length) {
        head.innerHTML = '<th>' + esc(note || 'No records') + '</th>';
        body.innerHTML = '<tr><td style="text-align:center; padding:16px; color:#777;">'
                       + esc(note || 'No records.') + '</td></tr>';
        return;
    }
    var keys = Object.keys(rows[0]);
    head.innerHTML = keys.map(function (k) { return '<th>' + esc(k) + '</th>'; }).join('');
    body.innerHTML = rows.map(function (r) {
        return '<tr>' + keys.map(function (k) {
            var v = r[k];
            var kl = String(k).toLowerCase();
            if (kl.indexOf('date') >= 0) return '<td>' + esc(ymd(v)) + '</td>';
            /* Document numbers are clickable per the brief. There is no ported target for
               every document type yet, so the click reports the code rather than guessing a URL. */
            if (kl === 'docno' || kl === 'invoicedocno' || kl === 'invoiceno' || kl === 'labno') {
                return '<td><span class="doc-link" onclick="pwDocClick(\'' + esc(k) + '\',\''
                     + esc(v) + '\')">' + esc(v) + '</span></td>';
            }
            if (typeof v === 'number') return '<td class="num">' + f2(v) + '</td>';
            return '<td>' + esc(v) + '</td>';
        }).join('') + '</tr>';
    }).join('');
}

window.pwDocClick = function (column, value) {
    msg(column + ' ' + value + ' - the document screen this row belongs to has not been ported '
      + 'yet, so nothing was opened.', false);
};

window.pwCloseModal = function () {
    var m = el('pwModal'); if (m) m.style.display = 'none';
};

/* ---------------------------------------------------------------- toolbar */
window.pwRefresh = function (btn) {
    if (S.busy) return;
    busy(btn, true);
    clearMsg();
    ['pwPurchase', 'pwPurchaseSeparate', 'pwSale', 'pwSaleSeparate', 'pwExport']
        .forEach(function (id) { var w = el(id); if (w) { w.innerHTML = ''; w.removeAttribute('data-cards'); } });
    var empty = el('pwEmpty');
    if (empty) { empty.style.display = ''; empty.textContent = 'Press Show to load.'; }
    loadSetup().then(function () { busy(btn, false); });
};

window.pwToggleFullscreen = function () {
    document.body.classList.toggle('pw-full');
};

/* ---------------------------------------------------------------- boot */
function boot() {
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') pwCloseModal();
    });
    var m = el('pwModal');
    if (m) m.addEventListener('click', function (e) { if (e.target === m) pwCloseModal(); });

    /* frmPendingWorksRpt_Load :93-109 - the desktop binds the cards immediately on load. */
    loadSetup().then(function () {
        var b = el('btnShow');
        if (b) pwShow(b);
    });
}

if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
else boot();

})();
