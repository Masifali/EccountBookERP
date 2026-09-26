/* ============================================================================
 * Purchase Analytics Dashboard
 * Ported from Architecture.WinApp.AnalyticDashboard\PurchaseAnalytiicsDashBoard.cs (2,850 lines).
 *
 *   :209-252   frmAnalyticsDashboard_Load            (the Load handler was never renamed)
 *   :255-280   BranchesFill
 *   :282-318   FilterData                            the four date-range radios
 *   :320-347   GetSeasonScheduleDates                "This Season"
 *   :349-399   ParentCategoryFill                    filtered by the form's Tag
 *   :402-466   AllComboBind                          one call, six dropdowns
 *   :468-494   cmbparentcategory_Leave               the cascade
 *   :496-515   btnshow_Click
 *   :575-727   GetComparisonDataAgainstSummary       both grids and both card sets
 *   :729-870   ItemWiseGridSettings
 *   :872-916   grdItemWise_ColumnButtonClick         "Party Wise" and "Date Wise"
 *   :919-969   grdItemWise_LinkClicked               Item Name and Moisture
 *   :971-1046  CustomerWiseGridSettings
 *   :1048-1098 grdCustomerWise_ColumnButtonClick / _LinkClicked
 *   :1100-1115 GetTotals
 *   :1165-1192 btnRefresh_Click
 *   :1241-1283 txtPreviousDays_TextChanged, chkShortWeight_CheckedChanged
 *
 * Dates are plain yyyy-mm-dd strings throughout - never toISOString(), which shifts a day at
 * UTC+5.
 * ==========================================================================*/
(function () {
'use strict';

var API = '/api/dashboard/purchase-analytics';

var S = {
    tag: 'frmPaddyPurchaseAnalytics',
    tagFixed: false,
    branchFeature: false,
    itemRows: [], custRows: [],
    totalAmount: 0,
    busy: false
};

/* ItemWiseGridSettings, :729-870.

   Hidden there: BranchesId, GroupCaption, GroupId, ParentCategory, TotalAmount (:784-789), and
   BranchName unless the branch feature is on AND a branch is chosen (:757-758).

   Two BUTTON columns are added at positions 0 and 1 (:735-750): "Party Wise" (key BreakUp) and
   "Date Wise" (key DateWise), and FrozenColumns = 2 keeps them pinned.
   Two LINK columns (ColumnType 5): GroupName (:813) and Moisture (:752).

   Sums (AggregateFunction 2): PrctOfTotalAmount, Qty, WeightKg, Expenses, Amount, ShortWeight,
   NetWeight.  AVERAGES (AggregateFunction 3): AvgRate and AvgRateExp - the desktop deliberately
   averages the two rate columns rather than adding them.

   Formats: "#,#" for Qty and WeightKg, "#,#.##" for the money and rate columns, "#,##0.###" for
   ShortWeight, NetWeight and NetRate.                                                        */
var ITEM_COLS = [
    { key: '__BreakUp',  cap: 'BreakUp',    button: 'breakup',  text: 'Party Wise', frozen: true },
    { key: '__DateWise', cap: 'Item Rates', button: 'datewise', text: 'Date Wise',  frozen: true },
    { key: 'BranchName',        cap: 'BranchName',  type: 's', branchOnly: true },
    { key: 'GroupName',         cap: 'Item Name',   type: 's', link: 'item' },
    { key: 'CropYear',          cap: 'CropYear',    type: 's' },
    { key: 'PrctOfTotalAmount', cap: '% Of Total',  type: 'd2', sum: true },
    { key: 'Qty',               cap: 'Qty',         type: 'd0', sum: true },
    { key: 'WeightKg',          cap: 'WeightKg',    type: 'd0', sum: true },
    { key: 'AvgRate',           cap: 'AvgRate',     type: 'd2', avg: true },
    { key: 'Expenses',          cap: 'Expenses',    type: 'd2', sum: true },
    { key: 'Exp40Kg',           cap: 'Exp40Kg',     type: 'd2' },
    { key: 'AvgRateExp',        cap: 'AvgRateExp',  type: 'd2', avg: true },
    { key: 'Amount',            cap: 'Amount',      type: 'd2', sum: true },
    { key: 'AnalysisQty',       cap: 'AnalysisQty', type: 'd2' },
    { key: 'Moisture',          cap: 'Moisture',    type: 'd2', link: 'moisture' },
    { key: 'EmptyShell/Trash',  cap: 'EmptyShell/Trash', type: 'd2', hideFor: 'Rice' },
    { key: 'Dust/Stone',        cap: 'Dust/Stone',  type: 'd2', hideFor: 'Rice' },
    { key: 'Broken',            cap: 'Broken',      type: 'd2', hideFor: 'Paddy' },
    { key: 'AGL',               cap: 'AGL',         type: 'd2', hideFor: 'Paddy' },
    { key: 'Damage',            cap: 'Damage',      type: 'd2', hideFor: 'Paddy' },
    { key: 'ShortWeight',       cap: 'ShortWeight', type: 'd3', sum: true, shortWeight: true },
    { key: 'NetWeight',         cap: 'NetWeight',   type: 'd3', sum: true, shortWeight: true },
    { key: 'NetRate',           cap: 'NetRate',     type: 'd3',            shortWeight: true }
];

/* CustomerWiseGridSettings, :971-1046. One button column ("Item Wise", key BreakUp) at position
   0 with FrozenColumns = 1, and GroupName as the only link. Hidden: BranchesId, GroupCaption,
   GroupId, ParentCategory, TotalAmount. AvgRate and AvgRateExp are averaged here too. */
var CUST_COLS = [
    { key: '__BreakUp', cap: 'Break Up', button: 'breakup', text: 'Item Wise', frozen: true },
    { key: 'BranchName',        cap: 'BranchName',    type: 's', branchOnly: true },
    { key: 'GroupName',         cap: 'Party Name',    type: 's', link: 'party' },
    { key: 'CropYear',          cap: 'CropYear',      type: 's' },
    { key: 'PrctOfTotalAmount', cap: '% Of T.Amount', type: 'd2', sum: true },
    { key: 'Qty',               cap: 'Qty',           type: 'd0', sum: true },
    { key: 'WeightKg',          cap: 'WeightKg',      type: 'd0', sum: true },
    { key: 'AvgRate',           cap: 'AvgRate',       type: 'd2', avg: true },
    { key: 'Expenses',          cap: 'Expenses',      type: 'd2', sum: true },
    { key: 'Exp40Kg',           cap: 'Exp40Kg',       type: 'd2' },
    { key: 'AvgRateExp',        cap: 'AvgRateExp',    type: 'd2', avg: true },
    { key: 'Amount',            cap: 'Amount',        type: 'd2', sum: true }
];

/* ---------------------------------------------------------------- helpers */
function el(id)  { return document.getElementById(id); }
function val(id) { var e = el(id); return e ? e.value : ''; }
function setVal(id, v) { var e = el(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
function chk(id) { var e = el(id); return !!(e && e.checked); }
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
    var n = num(v);
    return n.toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: places });
}
/* GetTotals uses "#,#;(#,#);0" - parentheses for negatives, a bare 0 at zero (:1104-1110). */
function fTot(v) {
    var n = num(v);
    if (n === 0) return '0';
    var a = Math.abs(n).toLocaleString(undefined, { maximumFractionDigits: 0 });
    return n < 0 ? '(' + a + ')' : a;
}
function fmt(v, type) {
    if (type === 'd0') return dec(v, 0);
    if (type === 'd2') return dec(v, 2);
    if (type === 'd3') return dec(v, 3);
    return esc(txt(v));
}
function isNumeric(type) { return type === 'd0' || type === 'd2' || type === 'd3'; }

function msg(text, ok) {
    var m = el('paMessage');
    if (!m) return;
    m.className = ok ? 'ok' : 'err';
    m.textContent = text;
    if (ok) setTimeout(function () { if (m.textContent === text) m.className = ''; }, 6000);
}
function clearMsg() { var m = el('paMessage'); if (m) { m.className = ''; m.textContent = ''; } }

function busy(btn, on) {
    S.busy = !!on;
    if (btn) { btn.disabled = !!on; btn.classList.toggle('btn-busy', !!on); }
    ['btnShow', 'btnRefresh', 'btnPrintItem', 'btnPrintSupplier'].forEach(function (id) {
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

function isoLocal(d) {
    var m = d.getMonth() + 1, day = d.getDate();
    return d.getFullYear() + '-' + (m < 10 ? '0' + m : m) + '-' + (day < 10 ? '0' + day : day);
}

function single(id) {
    var v = num(val(id));
    return v > 0 ? v : 0;
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
function selectedText(id) {
    var sel = el(id);
    if (!sel || sel.selectedIndex < 0) return '';
    var t = sel.options[sel.selectedIndex].text;
    return (sel.value === '0') ? '' : t;
}
function parentCategoryText() { return selectedText('parentCategory'); }

/** The filter bar, in the query-string shape every endpoint takes. */
function filterQuery() {
    return 'fromDate=' + encodeURIComponent(val('fromDate'))
         + '&toDate='  + encodeURIComponent(val('toDate'))
         + '&parentCategoryId=' + single('parentCategory')
         + '&itemCategoryId='   + single('itemCategory')
         + '&itemTypeId='       + single('itemType')
         + '&jobLotId='         + single('jobLot')
         + '&partyId='          + single('party')
         + '&itemId='           + single('itemName')
         /* CropYear is posted as the combo's TEXT, not its id (:606, :557). */
         + '&cropYear='  + encodeURIComponent(selectedText('cropYear'))
         + '&branchIds=' + encodeURIComponent(S.branchFeature ? pickedIds('branches') : '');
}

/* ---------------------------------------------------------------- setup */
function loadSetup() {
    return getJson(API + '/setup?tag=' + encodeURIComponent(S.tag)).then(function (d) {
        d = d || {};
        if (d.parentCategoryError) msg('Parent Category: ' + d.parentCategoryError, false);
        if (d.branchError)         msg('Branches: ' + d.branchError, false);

        S.branchFeature = !!d.branchFeature;

        setVal('fromDate', d.fromDate);
        setVal('toDate',   d.toDate);
        setVal('previousDays', d.previousDays);

        fillSelect('parentCategory', d.parentCategories, 'Parent Category');
        /* :392-395 - the desktop activates the first row, so the first category is selected. */
        var pc = el('parentCategory');
        if (pc && pc.options.length > 1) pc.selectedIndex = 1;

        var bw = el('branchWrap');
        if (bw) bw.style.display = S.branchFeature ? '' : 'none';
        if (S.branchFeature) fillSelect('branches', d.branches, 'Branch Name');

        /* :226-233 - the Short Weight tick box exists only on the Paddy screen. */
        var sw = el('shortWeightWrap');
        if (sw) sw.style.display = d.showShortWeight ? '' : 'none';

        if (window.jQuery && jQuery.fn.select2) {
            jQuery('.pa-select2').each(function () {
                var $s = jQuery(this);
                if ($s.data('select2')) $s.select2('destroy');
                $s.select2({ width: '100%', dropdownAutoWidth: true });
            });
            if (pc) jQuery(pc).trigger('change.select2');
        }

        if (!(d.parentCategories || []).length) {
            msg('No Parent Category is allocated to this analytics screen.', false);
            return null;
        }
        return loadCombos();
    }).catch(function (e) {
        msg('Setup could not be read: ' + e.message, false);
    });
}

/* AllComboBind, :402-466 - every remaining dropdown comes from one call, keyed by the chosen
   Parent Category. All six are DB-driven and searchable; none carries hard-coded values. */
function loadCombos() {
    var pid = single('parentCategory');
    return getJson(API + '/combos?parentCategoryId=' + pid).then(function (d) {
        d = d || {};
        if (d.error) msg('Dropdowns: ' + d.error, false);
        fillSelect('itemType',     d.itemTypes,      'Item Type');
        fillSelect('itemCategory', d.itemCategories, 'Item Category');
        fillSelect('party',        d.parties,        'Party Name');
        fillSelect('itemName',     d.items,          'Item Name');
        fillSelect('cropYear',     d.cropYears,      'CropYear');
        fillSelect('jobLot',       d.jobLots,        'JobLot');
    }).catch(function (e) {
        msg('Dropdowns could not be read: ' + e.message, false);
    });
}

/* cmbparentcategory_Leave, :468-494 - with a category chosen the six dependents are re-read;
   with none, every one of them is emptied. */
window.paParentChanged = function () {
    if (single('parentCategory') > 0) { loadCombos(); return; }
    ['itemCategory', 'itemType', 'itemName', 'cropYear', 'jobLot', 'party'].forEach(clearSelect);
};

/* ---------------------------------------------------------------- date range */

/* FilterData, :282-318. Only the four radios below set the dates; "This Season" has its own
   handler that reads the season schedule from the database. */
window.paRangeChanged = function () {
    var now = new Date(), start, end;
    if (chk('rdToday')) {
        start = new Date(); end = new Date();
    } else if (chk('rdThisWeek')) {
        /* :291 - the desktop offsets by (DayOfWeek - 1), so its week starts on Monday and a
           Sunday walks back six days. Same arithmetic here. */
        var diff = now.getDay() - 1;
        start = new Date(now.getFullYear(), now.getMonth(), now.getDate() - diff);
        end = new Date(start.getFullYear(), start.getMonth(), start.getDate() + 6);
    } else if (chk('rdThisMonth')) {
        start = new Date(now.getFullYear(), now.getMonth(), 1);
        end = new Date(now.getFullYear(), now.getMonth() + 1, 0);
    } else if (chk('rdPrevious')) {
        var days = num(val('previousDays'));
        start = new Date(); start.setDate(start.getDate() - days);
        end = new Date();
    } else {
        return;
    }
    setVal('fromDate', isoLocal(start));
    setVal('toDate',   isoLocal(end));
};

/* txtPreviousDays_TextChanged, :1241-1260 - only while "Previous Days" is the chosen radio. */
window.paPreviousDaysChanged = function () {
    if (!chk('rdPrevious')) return;
    var days = num(val('previousDays'));
    var start = new Date(); start.setDate(start.getDate() - days);
    setVal('fromDate', isoLocal(start));
    setVal('toDate',   isoLocal(new Date()));
};

/* rdSeason_Click -> GetSeasonScheduleDates, :320-347. */
window.paSeasonChanged = function () {
    if (!chk('rdSeason')) return;
    getJson(API + '/season').then(function (d) {
        d = d || {};
        if (d.error)   { msg('Season schedule: ' + d.error, false); return; }
        if (d.warning) { msg(d.warning, false); return; }
        setVal('fromDate', d.fromDate);
        setVal('toDate',   d.toDate);
    }).catch(function (e) { msg('Season schedule failed: ' + e.message, false); });
};

/* chkShortWeight_CheckedChanged, :1262-1283 - three columns come and go, no re-read. */
window.paShortWeightChanged = function () { renderItem(); };

window.paTagChanged = function () {
    S.tag = val('tagPicker') || S.tag;
    S.itemRows = []; S.custRows = [];
    renderAll();
    loadSetup();
};

/* ---------------------------------------------------------------- show */
window.paShow = function (btn) {
    if (S.busy) return;
    clearMsg();
    /* :500-504 - the desktop refuses before it reads anything. */
    if (single('parentCategory') <= 0) {
        msg('Please Select Parent Category First...', false);
        return;
    }
    busy(btn, true);

    getJson(API + '/report?' + filterQuery()).then(function (d) {
        busy(btn, false);
        d = d || {};
        if (d.itemWiseError)     msg('Item Wise: ' + d.itemWiseError, false);
        if (d.customerWiseError) msg('Supplier Wise: ' + d.customerWiseError, false);
        if (d.noRecord)          msg(d.noRecord, false);

        S.itemRows = d.itemWise || [];
        S.custRows = d.customerWise || [];
        /* :1108 - Total Amount is NOT a sum of the grid; it is TotalAmount off the first party
           row, which the procedure already carries. */
        S.totalAmount = S.custRows.length ? num(f(S.custRows[0], 'TotalAmount')) : 0;
        renderAll();
    }).catch(function (e) {
        busy(btn, false);
        msg('Show failed: ' + e.message, false);
        S.itemRows = []; S.custRows = []; S.totalAmount = 0;
        renderAll();
    });
};

function renderAll() {
    renderItem();
    renderCustomer();
    renderCards();
    renderTotals();
}

/* GetTotals, :1100-1115 - Qty and Weight are sums of the PARTY grid, and Total Amount comes
   from the procedure's own TotalAmount column. */
function renderTotals() {
    var qty = 0, weight = 0;
    S.custRows.forEach(function (r) {
        qty += num(f(r, 'Qty'));
        weight += num(f(r, 'WeightKg'));
    });
    var q = el('totalQty'), w = el('totalWeight'), a = el('totalAmount');
    if (q) q.textContent = fTot(qty);
    if (w) w.textContent = fTot(weight);
    if (a) a.textContent = fTot(S.totalAmount);
}

/* ---------------------------------------------------------------- grids */
function itemVisibleCols() {
    var branchChosen = S.branchFeature && pickedIds('branches') !== '';
    var cat = parentCategoryText();
    return ITEM_COLS.filter(function (c) {
        if (c.branchOnly && !branchChosen) return false;
        /* :791-812 - Paddy hides Broken, AGL and Damage; Rice hides EmptyShell/Trash,
           Dust/Stone and the three Short Weight columns. */
        if (c.hideFor && c.hideFor === cat) return false;
        if (c.shortWeight) {
            if (cat === 'Rice') return false;
            if (cat === 'Paddy') return chk('chkShortWeight');
        }
        return true;
    });
}

function custVisibleCols() {
    var branchChosen = S.branchFeature && pickedIds('branches') !== '';
    return CUST_COLS.filter(function (c) { return !c.branchOnly || branchChosen; });
}

function renderGrid(tableId, cols, rows, which) {
    var table = el(tableId);
    if (!table) return;
    if (!rows.length) {
        table.innerHTML = '<tbody><tr><td class="pa-empty">No Record Found!</td></tr></tbody>';
        return;
    }

    var head = '<thead><tr>' + cols.map(function (c) {
        return '<th>' + esc(c.cap) + '</th>';
    }).join('') + '</tr></thead>';

    var body = rows.map(function (row, i) {
        return '<tr>' + cols.map(function (c) {
            if (c.button) {
                return '<td><button type="button" class="pa-cellbtn" '
                     + 'onclick="paCellButton(\'' + which + '\',\'' + c.button + '\',' + i + ')">'
                     + esc(c.text) + '</button></td>';
            }
            var raw = f(row, c.key);
            var cls = isNumeric(c.type) ? 'pa-num' : '';
            if (isNumeric(c.type) && num(raw) < 0) cls += ' pa-neg';
            var inner = fmt(raw, c.type);
            if (c.link) {
                inner = '<span class="pa-link" onclick="paCellLink(\'' + which + '\',\''
                      + c.link + '\',' + i + ')">' + inner + '</span>';
            }
            return '<td class="' + cls + '">' + inner + '</td>';
        }).join('') + '</tr>';
    }).join('');

    /* AggregateFunction 2 is Sum and 3 is Average - the two rate columns are AVERAGED. */
    var foot = '<tfoot><tr>' + cols.map(function (c, idx) {
        if (!c.sum && !c.avg) return '<td>' + (idx === 0 ? 'Total' : '') + '</td>';
        var t = 0;
        rows.forEach(function (r) { t += num(f(r, c.key)); });
        if (c.avg) t = rows.length ? t / rows.length : 0;
        return '<td class="pa-num' + (t < 0 ? ' pa-neg' : '') + '">' + fmt(t, c.type) + '</td>';
    }).join('') + '</tr></tfoot>';

    table.innerHTML = head + '<tbody>' + body + '</tbody>' + foot;
}

function renderItem()     { renderGrid('gridItemWise',     itemVisibleCols(), S.itemRows, 'item'); }
function renderCustomer() { renderGrid('gridCustomerWise', custVisibleCols(), S.custRows, 'cust'); }

/* ItemAndSupplierWiseAnalyticalInfo, the card control (:652-668 and :714-726). The item cards
   format with "#,##0.###"; the party cards use "#,##.###", which is the desktop's own
   difference, and both come out the same for any real figure. */
function renderCards() {
    var branchChosen = S.branchFeature && pickedIds('branches') !== '';
    function cards(rows) {
        if (!rows.length) return '<div class="pa-empty">No Record Found!</div>';
        return rows.map(function (r) {
            function row(label, key) {
                return '<div class="pa-crow"><span>' + label + '</span><span>'
                     + dec(f(r, key), 3) + '</span></div>';
            }
            var branch = branchChosen ? txt(f(r, 'BranchName')) : '';
            return '<div class="pa-card">'
                 + '<div class="t">' + esc(txt(f(r, 'GroupName'))) + '</div>'
                 + '<div class="b">'
                 + row('Qty', 'Qty')
                 + row('Weight', 'WeightKg')
                 + row('Exp', 'Exp40Kg')
                 + row('AvgRate', 'AvgRate')
                 + row('E.AvgRate', 'AvgRateExp')
                 + '</div>'
                 + (branch ? '<div class="pa-cbranch">' + esc(branch) + '</div>' : '')
                 + '</div>';
        }).join('');
    }
    var a = el('cardsItemWise'), b = el('cardsCustomerWise');
    if (a) a.innerHTML = cards(S.itemRows);
    if (b) b.innerHTML = cards(S.custRows);
}

/* ---------------------------------------------------------------- drill-downs */

/**
 * grdItemWise_ColumnButtonClick :872-916 and grdCustomerWise_ColumnButtonClick :1048-1063.
 *   "Party Wise" -> the break-up of one item by party   ("Item And Customer Wise Comparison")
 *   "Item Wise"  -> the break-up of one party by item   ("Customer And Item Wise Comparison")
 *   "Date Wise"  -> the item's average rates by date
 * All three open a maximised pop-up on the desktop; here they fill the modal.
 */
window.paCellButton = function (which, kind, i) {
    var rows = which === 'item' ? S.itemRows : S.custRows;
    var row = rows[i];
    if (!row) return;
    var groupId = num(f(row, 'GroupId'));
    var rowBranch = txt(f(row, 'BranchesId'));
    var name = txt(f(row, 'GroupName'));

    if (kind === 'datewise') {
        openModal('Date Wise Item Rates - ' + name,
            API + '/avg-rate-by-item?' + filterQuery()
                + '&itemId=' + groupId + '&rowBranchId=' + encodeURIComponent(rowBranch));
        return;
    }
    var reportType = which === 'item'
        ? 'Item And Customer Wise Comparison'
        : 'Customer And Item Wise Comparison';
    openModal((which === 'item' ? 'Party Wise Break Up - ' : 'ItemWise BreakUp - ') + name,
        API + '/breakup?' + filterQuery()
            + '&reportType=' + encodeURIComponent(reportType)
            + '&groupId=' + groupId
            + '&rowBranchId=' + encodeURIComponent(rowBranch));
};

/**
 * grdItemWise_LinkClicked :919-969 and grdCustomerWise_LinkClicked :1065-1098.
 *   Item Name  -> average rates by supplier for that item, with the filter's own party
 *   Party Name -> average rates by supplier for that party, with the filter's own item
 *   Moisture   -> LabDataVehicleWiseByParent, which is NOT ported
 */
window.paCellLink = function (which, kind, i) {
    var rows = which === 'item' ? S.itemRows : S.custRows;
    var row = rows[i];
    if (!row) return;
    var groupId = num(f(row, 'GroupId'));
    var rowBranch = txt(f(row, 'BranchesId'));
    var name = txt(f(row, 'GroupName'));

    if (kind === 'moisture') {
        msg('Moisture opens Lab Data (Vehicle Wise By Parent) on the desktop for '
          + (name || 'this item') + '. That screen is not ported yet, so nothing was opened.',
          false);
        return;
    }
    if (kind === 'item') {
        /* :943-945 - the row supplies the item, the filter supplies the party. */
        openModal('Avg Rate By Supplier - ' + name,
            API + '/avg-rate-by-supplier?' + filterQuery()
                + '&itemId=' + groupId + '&partyId=' + single('party')
                + '&rowBranchId=' + encodeURIComponent(rowBranch));
        return;
    }
    /* :1084-1086 - the row supplies the party, the filter supplies the item. */
    openModal('Avg Rate By Item - ' + name,
        API + '/avg-rate-by-supplier?' + filterQuery()
            + '&itemId=' + single('itemName') + '&partyId=' + groupId
            + '&rowBranchId=' + encodeURIComponent(rowBranch));
};

function openModal(title, url) {
    var modal = el('paModal'), head = el('paModalTitle'), grid = el('paModalGrid');
    if (!modal || !grid) return;
    if (head) head.textContent = title;
    grid.innerHTML = '<tbody><tr><td class="pa-empty">Reading...</td></tr></tbody>';
    modal.classList.add('open');

    getJson(url).then(function (d) {
        d = d || {};
        if (d.error) {
            grid.innerHTML = '<tbody><tr><td class="pa-empty">' + esc(d.error) + '</td></tr></tbody>';
            return;
        }
        var rows = d.rows || [];
        /* :906 and :953 - the desktop says "No Data Found" and opens nothing. */
        if (!rows.length) {
            grid.innerHTML = '<tbody><tr><td class="pa-empty">No Data Found</td></tr></tbody>';
            return;
        }
        /* The pop-ups show whatever the procedure returns, so the columns are taken from the
           first row rather than fixed here. */
        var keys = Object.keys(rows[0]);
        grid.innerHTML =
            '<thead><tr>' + keys.map(function (k) { return '<th>' + esc(k) + '</th>'; }).join('')
          + '</tr></thead><tbody>'
          + rows.map(function (r) {
                return '<tr>' + keys.map(function (k) {
                    var v = r[k];
                    var isNum = (typeof v === 'number');
                    return '<td class="' + (isNum ? 'pa-num' : '') + (isNum && v < 0 ? ' pa-neg' : '')
                         + '">' + (isNum ? dec(v, 3) : esc(txt(v))) + '</td>';
                }).join('') + '</tr>';
            }).join('')
          + '</tbody>';
    }).catch(function (e) {
        grid.innerHTML = '<tbody><tr><td class="pa-empty">' + esc(e.message) + '</td></tr></tbody>';
    });
}

window.paCloseModal = function () {
    var modal = el('paModal');
    if (modal) modal.classList.remove('open');
};

/* ---------------------------------------------------------------- tabs, toolbar */
window.paTab = function (which) {
    ['IW', 'IC', 'SW', 'SC'].forEach(function (k) {
        var tab = el('tab' + k), pane = el('pane' + k);
        if (tab)  tab.classList.toggle('active', k === which);
        if (pane) pane.style.display = (k === which) ? '' : 'none';
    });
};

/* btnRefresh_Click, :1165-1192 - re-reads Parent Category, then either the six dependents or
   clears them. It does NOT re-read the grids. */
window.paRefresh = function (btn) {
    if (S.busy) return;
    busy(btn, true);
    clearMsg();
    loadSetup().then(function () { busy(btn, false); });
};

/* btnPrintItemWise :1285-1306 and btnPrintSupplierWise :1308-1338 refuse with "Not Record Found
   For Display" when their grid is empty. */
window.paPrint = function (btn, which) {
    if (S.busy) return;
    var rows = which === 'item' ? S.itemRows : S.custRows;
    if (!rows.length) { msg('Not Record Found For Display', false); return; }
    busy(btn, true);
    try {
        paTab(which === 'item' ? 'IW' : 'SW');
        window.print();
    } finally { busy(btn, false); }
};

window.paToggleFullscreen = function () {
    document.body.classList.toggle('pa-full');
};

document.addEventListener('keydown', function (e) {
    if (e.key !== 'Escape') return;
    var modal = el('paModal');
    if (modal && modal.classList.contains('open')) { e.preventDefault(); paCloseModal(); return; }
    if (document.body.classList.contains('pa-full')) {
        e.preventDefault();
        document.body.classList.remove('pa-full');
    }
});

/* ---------------------------------------------------------------- boot */
function boot() {
    S.tag = val('screenTag') || S.tag;
    S.tagFixed = (val('tagFixed') === 'true');

    var pickerRow = el('tagPickerRow');
    if (pickerRow) pickerRow.style.display = S.tagFixed ? 'none' : '';
    var picker = el('tagPicker');
    if (picker) picker.value = S.tag;

    loadSetup().then(function () {
        /* :242-250 - the desktop reads as soon as a Parent Category is active. */
        if (single('parentCategory') > 0) {
            var b = el('btnShow');
            if (b) paShow(b);
        }
    });
}

if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
else boot();

})();
