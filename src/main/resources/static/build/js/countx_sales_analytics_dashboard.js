/* ============================================================================
 * Sales Analytics Dashboard
 * Ported from Architecture.WinApp.AnalyticDashboard\SalesAnalyticsDashBoard.cs (2,375 lines).
 *
 *   :160-192   frmAnalyticsDashboard_Load        (the handler name was never updated)
 *   :195-220   BranchesFill
 *   :222-254   ParentCategoryFill                no Tag filter on this form
 *   :256-320   AllComboBind                      one call, six dropdowns
 *   :322-348   cmbparentcategory_Leave           the cascade
 *   :350-368   btnshow_Click
 *   :371-393   GetComparisonData                 both summaries AND both break-up sets
 *   :443-563   GetComparisonDataAgainstSummary
 *   :566-654   ItemWiseGridSettings
 *   :656-686   grdItemWise_LinkClicked           Item Name, Sales Account
 *   :688-729   grdItemWise_ColumnButtonClick     filters the in-memory break-up
 *   :732-816   CustomerWiseGridSettings
 *   :818-876   grdCustomerWise_ColumnButtonClick / _LinkClicked
 *   :878-914   GetTotals                         including the Avg Rate 40Kg total
 *
 * Dates are plain yyyy-mm-dd strings throughout - never toISOString(), which shifts a day at
 * UTC+5.
 * ==========================================================================*/
(function () {
'use strict';

var API = '/api/dashboard/sales-analytics';

var S = {
    itemRows: [], custRows: [],
    breakItem: [], breakCust: [],
    totalAmount: 0, avgRate40: 0,
    branchFeature: false, busy: false
};

/* ItemWiseGridSettings, :566-654.
   Hidden: BranchesId, GroupCaption, GroupId, ParentCategory, TotalAmount, SalesAccountId;
   BranchName only with the branch feature on AND a branch chosen (:578).
   One BUTTON column "Party Wise" (key BreakUp) at position 0, FrozenColumns = 1.
   Two LINK columns (ColumnType 5): GroupName and SalesAccount.
   Sums: PrctOfTotalAmount, Qty, WeightKg, ItemAmount, AddExpenses, LessExpenses, Amount.
   AvgRateExp is AggregateFunction 3 = AVERAGE.
   AvgRate has NO aggregate of its own - GetTotals injects Amount/Weight x 40 into its total cell
   and renames the column to "Avg Rate 40Kg" (:884-899).

   type: d0 "#,#"   d2 "#,#.##"   s text                                                      */
var ITEM_COLS = [
    { key: '__BreakUp',         cap: 'BreakUp',            button: true, text: 'Party Wise' },
    { key: 'BranchName',        cap: 'BranchName',         type: 's', branchOnly: true },
    { key: 'GroupName',         cap: 'Item Name',          type: 's', link: 'item' },
    { key: 'CropYear',          cap: 'CropYear',           type: 's' },
    { key: 'PrctOfTotalAmount', cap: '% Of Total Amount',  type: 'd2', sum: true },
    { key: 'Qty',               cap: 'Qty',                type: 'd0', sum: true },
    { key: 'WeightKg',          cap: 'WeightKg',           type: 'd0', sum: true },
    { key: 'AvgRate',           cap: 'Avg Rate 40Kg',      type: 'd2', avgRate40: true },
    { key: 'ItemAmount',        cap: 'ItemAmount',         type: 'd2', sum: true },
    { key: 'AddExpenses',       cap: 'AddExpenses',        type: 'd2', sum: true },
    { key: 'LessExpenses',      cap: 'LessExpenses',       type: 'd2', sum: true },
    { key: 'Amount',            cap: 'Amount',             type: 'd2', sum: true },
    { key: 'AvgRateExp',        cap: 'Avg Rate + Expenses', type: 'd2', avg: true },
    { key: 'SalesAccount',      cap: 'SalesAccount',       type: 's', link: 'account' }
];

/* CustomerWiseGridSettings, :732-816 - same shape, "Item Wise" button, GroupName is "Party Name",
   and no Sales Account columns. */
var CUST_COLS = [
    { key: '__BreakUp',         cap: 'Break Up',           button: true, text: 'Item Wise' },
    { key: 'BranchName',        cap: 'BranchName',         type: 's', branchOnly: true },
    { key: 'GroupName',         cap: 'Party Name',         type: 's', link: 'party' },
    { key: 'CropYear',          cap: 'CropYear',           type: 's' },
    { key: 'PrctOfTotalAmount', cap: '% Of Total Amount',  type: 'd2', sum: true },
    { key: 'Qty',               cap: 'Qty',                type: 'd0', sum: true },
    { key: 'WeightKg',          cap: 'WeightKg',           type: 'd0', sum: true },
    { key: 'AvgRate',           cap: 'Avg Rate 40Kg',      type: 'd2', avgRate40: true },
    { key: 'ItemAmount',        cap: 'ItemAmount',         type: 'd2', sum: true },
    { key: 'AddExpenses',       cap: 'AddExpenses',        type: 'd2', sum: true },
    { key: 'LessExpenses',      cap: 'LessExpenses',       type: 'd2', sum: true },
    { key: 'Amount',            cap: 'Amount',             type: 'd2', sum: true },
    { key: 'AvgRateExp',        cap: 'Avg Rate + Expenses', type: 'd2', avg: true }
];

/* ---------------------------------------------------------------- helpers */
function el(id)  { return document.getElementById(id); }
function val(id) { var e = el(id); return e ? e.value : ''; }
function setVal(id, v) { var e = el(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
function txt(v)  { return (v === null || v === undefined) ? '' : String(v); }
function esc(v)  { return txt(v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
                                .replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
function num(v)  { var n = parseFloat(v); return isNaN(n) ? 0 : n; }

function f(row, name) {
    if (!row) return null;
    if (row[name] !== undefined) return row[name];
    var lower = String(name).toLowerCase();
    for (var k in row) { if (k.toLowerCase() === lower) return row[k]; }
    return null;
}

function dec(v, places) {
    return num(v).toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: places });
}
/* GetTotals formats the three header boxes "#,#;(#,#);0" (:882-887). */
function fTot(v) {
    var n = num(v);
    if (n === 0) return '0';
    var a = Math.abs(n).toLocaleString(undefined, { maximumFractionDigits: 0 });
    return n < 0 ? '(' + a + ')' : a;
}
function fmt(v, type) {
    if (type === 'd0') return dec(v, 0);
    if (type === 'd2') return dec(v, 2);
    return esc(txt(v));
}
function isNumeric(type) { return type === 'd0' || type === 'd2'; }

function msg(text, ok) {
    var m = el('saMessage');
    if (!m) return;
    m.className = ok ? 'ok' : 'err';
    m.textContent = text;
    if (ok) setTimeout(function () { if (m.textContent === text) m.className = ''; }, 6000);
}
function clearMsg() { var m = el('saMessage'); if (m) { m.className = ''; m.textContent = ''; } }

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

function fillSelect(id, list, placeholder) {
    var sel = el(id);
    if (!sel) return;
    sel.innerHTML = '<option value="0">' + esc(placeholder) + '</option>'
        + (list || []).map(function (o) {
            return '<option value="' + esc(o.id) + '">' + esc(o.name) + '</option>';
        }).join('');
    if (window.jQuery && jQuery.fn.select2 && jQuery(sel).data('select2')) {
        jQuery(sel).trigger('change.select2');
    }
}
function clearSelect(id) { fillSelect(id, [], ''); }

function single(id) { var v = num(val(id)); return v > 0 ? v : 0; }

function pickedIds(id) {
    var sel = el(id);
    if (!sel) return '';
    var out = [];
    Array.prototype.forEach.call(sel.options, function (o) {
        if (o.selected && txt(o.value) && txt(o.value) !== '0') out.push(o.value);
    });
    return out.join(',');
}
function selectedText(id) {
    var sel = el(id);
    if (!sel || sel.selectedIndex < 0) return '';
    return (sel.value === '0') ? '' : sel.options[sel.selectedIndex].text;
}

/* ---------------------------------------------------------------- setup */
function loadSetup() {
    return getJson(API + '/setup').then(function (d) {
        d = d || {};
        if (d.parentCategoryError) msg('Parent Category: ' + d.parentCategoryError, false);
        if (d.branchError)         msg('Branches: ' + d.branchError, false);

        S.branchFeature = !!d.branchFeature;
        setVal('fromDate', d.fromDate);
        setVal('toDate',   d.toDate);

        fillSelect('parentCategory', d.parentCategories, 'Parent Category');
        /* :247 - Rows[0].Activate(), so the first category is selected. */
        var pc = el('parentCategory');
        if (pc && pc.options.length > 1) pc.selectedIndex = 1;

        var bw = el('branchWrap');
        if (bw) bw.style.display = S.branchFeature ? '' : 'none';
        if (S.branchFeature) fillSelect('branches', d.branches, 'Branch Name');

        if (window.jQuery && jQuery.fn.select2) {
            jQuery('.sa-select2').each(function () {
                var $s = jQuery(this);
                if ($s.data('select2')) $s.select2('destroy');
                $s.select2({ width: '100%', dropdownAutoWidth: true });
            });
            if (pc) jQuery(pc).trigger('change.select2');
        }

        if (!(d.parentCategories || []).length) {
            msg('No Parent Category was returned for this user.', false);
            return null;
        }
        return loadCombos();
    }).catch(function (e) {
        msg('Setup could not be read: ' + e.message, false);
    });
}

/* AllComboBind, :256-320. All six are DB-driven and searchable; none is hard-coded. */
function loadCombos() {
    return getJson(API + '/combos?parentCategoryId=' + single('parentCategory')).then(function (d) {
        d = d || {};
        if (d.error) msg('Dropdowns: ' + d.error, false);
        fillSelect('itemType',     d.itemTypes,      'Item Type');
        fillSelect('itemCategory', d.itemCategories, 'Item Category');
        fillSelect('party',        d.parties,        'Party Name');
        fillSelect('itemName',     d.items,          'Item Name');
        fillSelect('jobLot',       d.jobLots,        'JobLot');
        fillSelect('cropYear',     d.cropYears,      'CropYear');
    }).catch(function (e) {
        msg('Dropdowns could not be read: ' + e.message, false);
    });
}

/* cmbparentcategory_Leave, :322-348. */
window.saParentChanged = function () {
    if (single('parentCategory') > 0) { loadCombos(); return; }
    ['itemCategory', 'itemType', 'itemName', 'cropYear', 'jobLot', 'party'].forEach(clearSelect);
};

/* ---------------------------------------------------------------- show */
window.saShow = function (btn) {
    if (S.busy) return;
    clearMsg();
    /* :354-358 - the desktop refuses before reading anything. */
    if (single('parentCategory') <= 0) {
        msg('Please Select Parent Category First...', false);
        return;
    }
    busy(btn, true);

    var q = '?fromDate=' + encodeURIComponent(val('fromDate'))
          + '&toDate='   + encodeURIComponent(val('toDate'))
          + '&parentCategoryId=' + single('parentCategory')
          + '&itemCategoryId='   + single('itemCategory')
          + '&itemTypeId='       + single('itemType')
          + '&jobLotId='         + single('jobLot')
          + '&partyId='          + single('party')
          + '&itemId='           + single('itemName')
          /* CropYear is posted as the combo's TEXT, not its id (:475). */
          + '&cropYear='  + encodeURIComponent(selectedText('cropYear'))
          + '&branchIds=' + encodeURIComponent(S.branchFeature ? pickedIds('branches') : '');

    getJson(API + '/report' + q).then(function (d) {
        busy(btn, false);
        d = d || {};
        ['itemWiseError', 'customerWiseError', 'breakupItemWiseError', 'breakupCustomerWiseError']
            .forEach(function (k) { if (d[k]) msg(k.replace('Error', '') + ': ' + d[k], false); });
        if (d.noRecord) msg(d.noRecord, false);

        S.itemRows  = d.itemWise || [];
        S.custRows  = d.customerWise || [];
        S.breakItem = d.breakupItemWise || [];
        S.breakCust = d.breakupCustomerWise || [];
        computeTotals();
        renderAll();
    }).catch(function (e) {
        busy(btn, false);
        msg('Show failed: ' + e.message, false);
        S.itemRows = []; S.custRows = []; S.breakItem = []; S.breakCust = [];
        computeTotals();
        renderAll();
    });
};

/**
 * GetTotals, :878-914.
 *
 * Qty and Weight are sums of the PARTY grid; Total Amount is TotalAmount off the first party row,
 * not a sum. The Avg Rate total is then Amount / Weight x 40 and is injected into BOTH grids'
 * total row, with the column renamed "Avg Rate 40Kg".
 *
 * The desktop divides without guarding, so a zero weight yields Infinity there. Here the cell is
 * left blank instead - no substitute factor is invented, and the figure is simply not shown when
 * it cannot be computed.
 */
function computeTotals() {
    var qty = 0, weight = 0;
    S.custRows.forEach(function (r) {
        qty += num(f(r, 'Qty'));
        weight += num(f(r, 'WeightKg'));
    });
    S.totalAmount = S.custRows.length ? num(f(S.custRows[0], 'TotalAmount')) : 0;
    S.avgRate40 = weight !== 0 ? (S.totalAmount / weight) * 40 : null;

    var q = el('totalQty'), w = el('totalWeight'), a = el('totalAmount');
    if (q) q.textContent = fTot(qty);
    if (w) w.textContent = fTot(weight);
    if (a) a.textContent = fTot(S.totalAmount);
}

function renderAll() {
    renderGrid('gridItemWise',     itemCols(), S.itemRows, 'item');
    renderGrid('gridCustomerWise', custCols(), S.custRows, 'cust');
}

function branchChosen() { return S.branchFeature && pickedIds('branches') !== ''; }
function itemCols() { return ITEM_COLS.filter(function (c) { return !c.branchOnly || branchChosen(); }); }
function custCols() { return CUST_COLS.filter(function (c) { return !c.branchOnly || branchChosen(); }); }

/* ---------------------------------------------------------------- grids */
function renderGrid(tableId, cols, rows, which) {
    var table = el(tableId);
    if (!table) return;
    if (!rows.length) {
        table.innerHTML = '<tbody><tr><td class="sa-empty">No Record Found!</td></tr></tbody>';
        return;
    }

    var head = '<thead><tr>' + cols.map(function (c) {
        return '<th>' + esc(c.cap) + '</th>';
    }).join('') + '</tr></thead>';

    var body = rows.map(function (row, i) {
        return '<tr>' + cols.map(function (c) {
            if (c.button) {
                return '<td><button type="button" class="sa-cellbtn" '
                     + 'onclick="saBreakUp(\'' + which + '\',' + i + ')">'
                     + esc(c.text) + '</button></td>';
            }
            var raw = f(row, c.key);
            var cls = isNumeric(c.type) ? 'sa-num' : '';
            if (isNumeric(c.type) && num(raw) < 0) cls += ' sa-neg';
            var inner = fmt(raw, c.type);
            if (c.link) {
                if (c.link === 'account') {
                    /* :678 - GoToGeneralLedgerFromLinkedEvent(SalesAccountId, fromdate, todate).
                       The web General Ledger takes accountId, fromDate and toDate on the query
                       string and runs itself. */
                    var accId = num(f(row, 'SalesAccountId'));
                    if (accId) {
                        inner = '<a class="sa-link" href="/accounts/reports/general-ledger?accountId='
                              + accId + '&fromDate=' + encodeURIComponent(val('fromDate'))
                              + '&toDate=' + encodeURIComponent(val('toDate'))
                              + '" title="General Ledger for this sales account">' + inner + '</a>';
                    }
                } else {
                    inner = '<span class="sa-link" onclick="saLink(\'' + which + '\',\''
                          + c.link + '\',' + i + ')">' + inner + '</span>';
                }
            }
            return '<td class="' + cls + '">' + inner + '</td>';
        }).join('') + '</tr>';
    }).join('');

    var foot = '<tfoot><tr>' + cols.map(function (c, idx) {
        /* AvgRate carries the injected Amount/Weight x 40 rather than a column aggregate. */
        if (c.avgRate40) {
            return '<td class="sa-num">' + (S.avgRate40 === null ? '' : dec(S.avgRate40, 2)) + '</td>';
        }
        if (!c.sum && !c.avg) return '<td>' + (idx === 0 ? 'Total' : '') + '</td>';
        var t = 0;
        rows.forEach(function (r) { t += num(f(r, c.key)); });
        if (c.avg) t = rows.length ? t / rows.length : 0;   // AggregateFunction 3 = Average
        return '<td class="sa-num' + (t < 0 ? ' sa-neg' : '') + '">' + fmt(t, c.type) + '</td>';
    }).join('') + '</tr></tfoot>';

    table.innerHTML = head + '<tbody>' + body + '</tbody>' + foot;
}

/**
 * grdItemWise_ColumnButtonClick :688-729 and grdCustomerWise_ColumnButtonClick :818-855.
 *
 * Both filter the break-up set ALREADY IN MEMORY - matching FirstGroupName to the clicked row's
 * GroupName, and additionally BranchesId when the clicked row carries one. Neither re-queries.
 */
window.saBreakUp = function (which, i) {
    var rows   = which === 'item' ? S.itemRows  : S.custRows;
    var source = which === 'item' ? S.breakItem : S.breakCust;
    var row = rows[i];
    if (!row) return;

    var groupName = txt(f(row, 'GroupName'));
    var rowBranch = txt(f(row, 'BranchesId'));

    var filtered = source.filter(function (b) {
        if (txt(f(b, 'FirstGroupName')) !== groupName) return false;
        if (rowBranch !== '') return txt(f(b, 'BranchesId')) === rowBranch;
        return true;
    });

    openModal((which === 'item' ? 'Party Wise Break Up - ' : 'ItemWise BreakUp - ') + groupName,
              filtered);
};

/**
 * grdItemWise_LinkClicked :656-686 and grdCustomerWise_LinkClicked :857-876.
 * Item Name and Party Name both open frmEvaulationDetailSalesReports, which is not ported, so the
 * click reports what it would open rather than opening something unrelated.
 */
window.saLink = function (which, kind, i) {
    var rows = which === 'item' ? S.itemRows : S.custRows;
    var row = rows[i];
    if (!row) return;
    var name = txt(f(row, 'GroupName'));
    msg('"' + name + '" opens the Detail Sales Report on the desktop for '
      + val('fromDate') + ' to ' + val('toDate')
      + ' filtered by ' + (kind === 'item' ? 'this item' : 'this party')
      + '. That screen is not ported yet, so nothing was opened.', false);
};

function openModal(title, rows) {
    var modal = el('saModal'), head = el('saModalTitle'), grid = el('saModalGrid');
    if (!modal || !grid) return;
    if (head) head.textContent = title;
    modal.classList.add('open');

    if (!rows.length) {
        grid.innerHTML = '<tbody><tr><td class="sa-empty">No Data Found</td></tr></tbody>';
        return;
    }
    /* The pop-up shows whatever the break-up procedure returned, so the columns come from the
       first row rather than being fixed here. */
    var keys = Object.keys(rows[0]);
    grid.innerHTML =
        '<thead><tr>' + keys.map(function (k) { return '<th>' + esc(k) + '</th>'; }).join('')
      + '</tr></thead><tbody>'
      + rows.map(function (r) {
            return '<tr>' + keys.map(function (k) {
                var v = r[k];
                var isNum = (typeof v === 'number');
                return '<td class="' + (isNum ? 'sa-num' : '') + (isNum && v < 0 ? ' sa-neg' : '')
                     + '">' + (isNum ? dec(v, 2) : esc(txt(v))) + '</td>';
            }).join('') + '</tr>';
        }).join('')
      + '</tbody>';
}

window.saCloseModal = function () {
    var modal = el('saModal');
    if (modal) modal.classList.remove('open');
};

/* ---------------------------------------------------------------- tabs, toolbar */
window.saTab = function (which) {
    ['IW', 'CW'].forEach(function (k) {
        var tab = el('tab' + k), pane = el('pane' + k);
        if (tab)  tab.classList.toggle('active', k === which);
        if (pane) pane.style.display = (k === which) ? '' : 'none';
    });
};

/* btnRefresh_Click, :964 - re-reads Parent Category and the dependents; it does NOT re-read the
   grids. */
window.saRefresh = function (btn) {
    if (S.busy) return;
    busy(btn, true);
    clearMsg();
    loadSetup().then(function () { busy(btn, false); });
};

window.saToggleFullscreen = function () {
    document.body.classList.toggle('sa-full');
};

document.addEventListener('keydown', function (e) {
    if (e.key !== 'Escape') return;
    var modal = el('saModal');
    if (modal && modal.classList.contains('open')) { e.preventDefault(); saCloseModal(); return; }
    if (document.body.classList.contains('sa-full')) {
        e.preventDefault();
        document.body.classList.remove('sa-full');
    }
});

/* ---------------------------------------------------------------- boot */
function boot() {
    /* :180-186 - the desktop reads as soon as a Parent Category is active. */
    loadSetup().then(function () {
        if (single('parentCategory') > 0) {
            var b = el('btnShow');
            if (b) saShow(b);
        }
    });
}

if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
else boot();

})();
