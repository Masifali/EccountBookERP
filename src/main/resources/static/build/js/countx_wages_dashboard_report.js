/* ============================================================================
 * Wages Dashboard Report
 * Ported from Architecture.WinApp.Inventory_Reports\frmWagesDashboardReport.cs (2,101 lines).
 *
 *   :191-271  frmEvaulationDetailSalesReports_Load   (the form's Load, name never updated)
 *   :345-523  AllGridFill                            five result sets, seven grids
 *   :525-688  the seven Grid*Setting methods         hidden columns, captions, links, grouping
 *   :689-807  the seven *_LinkClicked handlers
 *   :808-852  HandleLinkClicked
 *   :856-895  OpenWagesReportScreen
 *   :897-907  btnShow_Click
 *   :947-960  btnNew_Click "Refresh"
 *
 * Dates are plain yyyy-mm-dd strings throughout - never toISOString(), which shifts a day at
 * UTC+5.
 * ==========================================================================*/
(function () {
'use strict';

var API = '/api/dashboard/wages-report';

var S = { data: {}, busy: false, decimals: 2, rateDecimals: 2 };

/* Column sets, each matching what its Grid*Setting leaves visible.

   GridEX_Helper.GridColumnSettings picks a format by COLUMN-NAME SUBSTRING:
     a key containing "Amount"      -> stringFormatsingle "#,##0.NN", summed
     a key containing "Rate"        -> DecimalRateFormate "#,##0.NN", NOT summed
     anything else numeric          -> "#,##0.##", summed
   So Weight and Qty are summed, Amount is summed, and AvgRate/40Kg and WageRate are NOT - the
   desktop deliberately leaves a rate out of the totals.

   type: s text, n amount, q quantity, r rate, d date                                       */
var GRIDS = {
    /* GridSummaryByDocTypeSetting :525-540 - hides ActivityDiscription and RefDocumentTypeId. */
    docType: {
        table: 'gridDocType',
        cols: [
            { key: 'RefDocumentType', cap: 'RefDocumentType', type: 's', link: true },
            { key: 'Weight',          cap: 'Weight',          type: 'q', sum: true },
            { key: 'Qty',             cap: 'Qty',             type: 'q', sum: true },
            { key: 'Amount',          cap: 'Amount',          type: 'n', sum: true },
            { key: 'AvgRate/40Kg',    cap: 'AvgRate / 40Kg',  type: 'r' }
        ]
    },
    /* GridSummaryByWagesAcSetting :556-575 - hides ActivityDiscription and WagesAccountId.
       "Labour / WAges Activity" is the desktop's own spelling (:562). */
    wagesAc: {
        table: 'gridWagesAc',
        cols: [
            { key: 'WagesAccount',  cap: 'Labour / WAges Activity', type: 's', link: true },
            { key: 'Weight',        cap: 'Weight',        type: 'q', sum: true },
            { key: 'Qty',           cap: 'Qty',           type: 'q', sum: true },
            { key: 'Amount',        cap: 'Amount',        type: 'n', sum: true },
            { key: 'AvgRate/40Kg',  cap: 'AvgRate / 40Kg', type: 'r' }
        ]
    },
    /* GridSummaryByContractorSetting :542-554 passes NO hidden-column list, so unlike the other
       two summaries this grid really does show ActivityDiscription and ContractorNameId.
       Reproduced. And ContractorName carries the DOCUMENT TYPE - see the service (:381). */
    contractor: {
        table: 'gridContractor',
        cols: [
            { key: 'ActivityDiscription', cap: 'ActivityDiscription', type: 's' },
            { key: 'ContractorNameId',    cap: 'ContractorNameId',    type: 's' },
            { key: 'ContractorName',      cap: 'ContractorName',      type: 's', link: true },
            { key: 'Weight', cap: 'Weight', type: 'q', sum: true },
            { key: 'Qty',    cap: 'Qty',    type: 'q', sum: true },
            { key: 'Amount', cap: 'Amount', type: 'n', sum: true }
        ]
    },
    /* grdActivityAndDocumentTypeSetting :577-604 - grouped by WagesAccount, with
       HideColumnsWhenGrouped, so the grouped column leaves the row body. */
    activityDoc: {
        table: 'gridActivityDoc',
        group: 'WagesAccount',
        cols: [
            { key: 'RefDocumentType',  cap: 'RefDocumentType',  type: 's' },
            { key: 'WagesAccount',     cap: 'Labour / Wages Activity', type: 's', link: true },
            { key: 'RateEffectedFrom', cap: 'RateEffectedFrom', type: 'd' },
            { key: 'PackSize',         cap: 'PackSize',         type: 's' },
            { key: 'WageRate',         cap: 'WageRate',         type: 's' },
            { key: 'Weight', cap: 'Weight', type: 'q', sum: true },
            { key: 'Qty',    cap: 'Qty',    type: 'q', sum: true },
            { key: 'Amount', cap: 'Amount', type: 'n', sum: true },
            { key: 'AvgRate/40Kg', cap: 'AvgRate / 40Kg', type: 'r' }
        ]
    },
    /* grdDocumentTypeAndActivitySetting :606-634 - same columns, grouped by RefDocumentType. */
    docActivity: {
        table: 'gridDocActivity',
        group: 'RefDocumentType',
        cols: [
            { key: 'RefDocumentType',  cap: 'RefDocumentType',  type: 's' },
            { key: 'WagesAccount',     cap: 'Labour / Wages Activity', type: 's', link: true },
            { key: 'RateEffectedFrom', cap: 'RateEffectedFrom', type: 'd' },
            { key: 'PackSize',         cap: 'PackSize',         type: 's' },
            { key: 'WageRate',         cap: 'WageRate',         type: 's' },
            { key: 'Weight', cap: 'Weight', type: 'q', sum: true },
            { key: 'Qty',    cap: 'Qty',    type: 'q', sum: true },
            { key: 'Amount', cap: 'Amount', type: 'n', sum: true },
            { key: 'AvgRate/40Kg', cap: 'AvgRate / 40Kg', type: 'r' }
        ]
    },
    /* grdDetailSummaryByContractorSetting :636-660 - grouped by ContractorName; hides
       ContractorNameId, RefDocumentTypeId and WagesAccountId. */
    detSumContractor: {
        table: 'gridDetSumContractor',
        group: 'ContractorName',
        cols: [
            { key: 'RefDocumentType', cap: 'RefDocumentType', type: 's', link: true },
            { key: 'WagesAccount',    cap: 'Labour / Wages Activity', type: 's' },
            { key: 'ContractorName',  cap: 'ContractorName',  type: 's' },
            { key: 'Weight', cap: 'Weight', type: 'q', sum: true },
            { key: 'Qty',    cap: 'Qty',    type: 'q', sum: true },
            { key: 'Amount', cap: 'Amount', type: 'n', sum: true }
        ]
    },
    /* grdDetailByContractorSetting :662-687 - grouped by ContractorName; WageRate is a real
       number in this table, so it takes the rate format and stays out of the totals. */
    detContractor: {
        table: 'gridDetContractor',
        group: 'ContractorName',
        cols: [
            { key: 'RefDocumentType', cap: 'RefDocumentType', type: 's', link: true },
            { key: 'WagesAccount',    cap: 'Labour / Wages Activity', type: 's' },
            { key: 'ContractorName',  cap: 'ContractorName',  type: 's' },
            { key: 'PackSize',        cap: 'PackSize',        type: 's' },
            { key: 'WageRate',        cap: 'WageRate',        type: 'r' },
            { key: 'Weight', cap: 'Weight', type: 'q', sum: true },
            { key: 'Qty',    cap: 'Qty',    type: 'q', sum: true },
            { key: 'Amount', cap: 'Amount', type: 'n', sum: true }
        ]
    }
};

/* The server key that feeds each grid. */
var SOURCE = {
    docType: 'summaryByDocType',
    wagesAc: 'summaryByWagesAc',
    contractor: 'summaryByContractor',
    activityDoc: 'activityDocumentType',
    docActivity: 'documentTypeActivity',
    detSumContractor: 'detailSummaryByContractor',
    detContractor: 'detailByContractor'
};

var MONTHS = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];

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

/* stringFormatsingle "#,##0.NN" for an Amount column. */
function fAmt(v) {
    return num(v).toLocaleString(undefined,
            { minimumFractionDigits: S.decimals, maximumFractionDigits: S.decimals });
}
/* DecimalRateFormate "#,##0.NN" at the rate decimals. */
function fRate(v) {
    return num(v).toLocaleString(undefined,
            { minimumFractionDigits: S.rateDecimals, maximumFractionDigits: S.rateDecimals });
}
/* The fall-through numeric format, "#,##0.##". */
function fQty(v) {
    return num(v).toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 2 });
}
function fDate(v) {
    var s = txt(v);
    if (!s) return '';
    var y = parseInt(s.substr(0, 4), 10),
        m = parseInt(s.substr(5, 2), 10),
        d = parseInt(s.substr(8, 2), 10);
    if (!y || !m || !d || y <= 1900) return '';
    return (d < 10 ? '0' + d : d) + '-' + MONTHS[m - 1] + '-' + String(y).slice(-2);
}
function fmt(v, type) {
    if (type === 'n') return fAmt(v);
    if (type === 'r') return fRate(v);
    if (type === 'q') return fQty(v);
    if (type === 'd') return fDate(v);
    return esc(txt(v));
}
function isNumeric(type) { return type === 'n' || type === 'r' || type === 'q'; }

function msg(text, ok) {
    var m = el('wdMessage');
    if (!m) return;
    m.className = ok ? 'ok' : 'err';
    m.textContent = text;
    if (ok) setTimeout(function () { if (m.textContent === text) m.className = ''; }, 6000);
}
function clearMsg() { var m = el('wdMessage'); if (m) { m.className = ''; m.textContent = ''; } }

function busy(btn, on) {
    S.busy = !!on;
    if (btn) { btn.disabled = !!on; btn.classList.toggle('btn-busy', !!on); }
    ['btnShow', 'btnNew', 'btnPrint'].forEach(function (id) {
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
}

/* ---------------------------------------------------------------- setup */
function loadSetup(keepDates) {
    return getJson(API + '/setup').then(function (d) {
        d = d || {};
        ['documentTypeError', 'wagesAccountError', 'contractorError'].forEach(function (k) {
            if (d[k]) msg(k.replace('Error', '') + ': ' + d[k], false);
        });

        S.decimals = (typeof d.decimalsAmount === 'number' && d.decimalsAmount >= 1
                      && d.decimalsAmount <= 4) ? d.decimalsAmount : 2;
        S.rateDecimals = (typeof d.decimalsRate === 'number' && d.decimalsRate >= 1
                          && d.decimalsRate <= 4) ? d.decimalsRate : 2;

        /* btnNew "Refresh" (:947-960) re-reads the three dropdowns and does NOT touch the
           dates, so Refresh keeps whatever the user has typed. */
        if (!keepDates) {
            setVal('fromDate', d.fromDate);
            setVal('toDate',   d.toDate);
        }

        /* Every dropdown is DB-driven and searchable; none carries hard-coded values. */
        fillSelect('documentType', d.documentTypes, 'Reference Document Type');
        fillSelect('wagesAccount', d.wagesAccounts, 'Wages Account');
        fillSelect('contractor',   d.contractors,   'Wages Contractor');

        if (window.jQuery && jQuery.fn.select2) {
            jQuery('.wd-select2').each(function () {
                var $s = jQuery(this);
                if ($s.data('select2')) $s.select2('destroy');
                $s.select2({ width: '100%', dropdownAutoWidth: true });
            });
        }
    }).catch(function (e) {
        msg('Setup could not be read: ' + e.message, false);
    });
}

function single(id) {
    var v = num(val(id));
    return v > 0 ? v : 0;
}

/* ---------------------------------------------------------------- show */
window.wdShow = function (btn) {
    if (S.busy) return;
    clearMsg();
    busy(btn, true);

    var q = '?fromDate=' + encodeURIComponent(val('fromDate'))
          + '&toDate='   + encodeURIComponent(val('toDate'))
          + '&contractorId='      + single('contractor')
          + '&wagesAccountId='    + single('wagesAccount')
          + '&refDocumentTypeId=' + single('documentType');

    getJson(API + '/report' + q).then(function (d) {
        busy(btn, false);
        d = d || {};
        if (d.error) { msg(d.error, false); S.data = {}; }
        else S.data = d;
        renderAll();
    }).catch(function (e) {
        busy(btn, false);
        msg('Show failed: ' + e.message, false);
        S.data = {};
        renderAll();
    });
};

function renderAll() {
    Object.keys(GRIDS).forEach(function (name) {
        render(name, (S.data && S.data[SOURCE[name]]) || []);
    });
}

/* ---------------------------------------------------------------- grids */
function render(name, rows) {
    var g = GRIDS[name], table = el(g.table);
    if (!table) return;

    if (!rows.length) {
        table.innerHTML = '<tbody><tr><td class="wd-empty">No records.</td></tr></tbody>';
        return;
    }

    /* HideColumnsWhenGrouped = 1 on every grouped grid, so the grouped column is not repeated
       inside the band. */
    var cols = g.cols.filter(function (c) { return c.key !== g.group; });

    var head = '<thead><tr>' + cols.map(function (c) {
        return '<th>' + esc(c.cap) + '</th>';
    }).join('') + '</tr></thead>';

    var body = '', lastGroup = null, bucket = [];

    function flush() {
        /* GroupTotals = 2 - each band carries its own totals row. */
        if (!g.group || !bucket.length) { bucket = []; return; }
        body += '<tr class="wd-subtotal">' + cols.map(function (c, idx) {
            if (!c.sum) return '<td>' + (idx === 0 ? 'Group Total' : '') + '</td>';
            var t = 0;
            bucket.forEach(function (r) { t += num(f(r, c.key)); });
            return '<td class="wd-num' + (t < 0 ? ' wd-neg' : '') + '">' + fmt(t, c.type) + '</td>';
        }).join('') + '</tr>';
        bucket = [];
    }

    rows.forEach(function (row, i) {
        if (g.group) {
            var key = txt(f(row, g.group));
            if (key !== lastGroup) {
                flush();
                lastGroup = key;
                body += '<tr class="wd-group"><td colspan="' + cols.length + '">'
                     + esc(key || '(blank)') + '</td></tr>';
            }
            bucket.push(row);
        }
        body += '<tr>' + cols.map(function (c) {
            var raw = f(row, c.key);
            var cls = isNumeric(c.type) ? 'wd-num' : '';
            if (isNumeric(c.type) && num(raw) < 0) cls += ' wd-neg';
            var inner = fmt(raw, c.type);
            /* ColumnType 5 - every link on this form funnels into HandleLinkClicked. */
            if (c.link) {
                inner = '<span class="wd-link" onclick="wdLink(\'' + name + '\',' + i + ')">'
                      + inner + '</span>';
            }
            return '<td class="' + cls + '">' + inner + '</td>';
        }).join('') + '</tr>';
    });
    flush();

    var foot = '<tfoot><tr>' + cols.map(function (c, idx) {
        if (!c.sum) return '<td>' + (idx === 0 ? 'Total' : '') + '</td>';
        var t = 0;
        rows.forEach(function (r) { t += num(f(r, c.key)); });
        return '<td class="wd-num' + (t < 0 ? ' wd-neg' : '') + '">' + fmt(t, c.type) + '</td>';
    }).join('') + '</tr></tfoot>';

    table.innerHTML = head + '<tbody>' + body + '</tbody>' + foot;
}

/**
 * HandleLinkClicked :808-852 reads RefDocumentTypeId, ContractorNameId and WagesAccountId off
 * the clicked row where the grid has them, checks the screen right for
 * "frmEvaulationDetailWagesReports", and then OpenWagesReportScreen (:856-895) opens that form
 * with the dashboard's dates and, for any id the row did not supply, the filter's own value.
 *
 * frmEvaulationDetailWagesReports is not ported, so the click reports exactly which record it
 * would open rather than opening something unrelated.
 */
window.wdLink = function (name, i) {
    var rows = (S.data && S.data[SOURCE[name]]) || [];
    var row = rows[i];
    if (!row) return;

    var refDoc = num(f(row, 'RefDocumentTypeId')) || single('documentType');
    var cont   = num(f(row, 'ContractorNameId'))  || single('contractor');
    var wages  = num(f(row, 'WagesAccountId'))    || single('wagesAccount');

    msg('This opens the Detail Wages Report on the desktop for '
      + val('fromDate') + ' to ' + val('toDate')
      + ' with Document Type ' + (refDoc || 'any')
      + ', Contractor ' + (cont || 'any')
      + ' and Wages Account ' + (wages || 'any')
      + '. That screen is not ported yet, so nothing was opened.', false);
};

/* ---------------------------------------------------------------- tabs */
window.wdTab = function (which) {
    ['AD', 'DA', 'CON'].forEach(function (k) {
        var tab = el('tab' + k), pane = el('pane' + k);
        if (tab)  tab.classList.toggle('active', k === which);
        if (pane) pane.style.display = (k === which) ? '' : 'none';
    });
};

/* ---------------------------------------------------------------- toolbar */

/* btnNew_Click, :947-960 - "Refresh" only re-reads the three dropdowns and puts focus back on
   From Date. It does not clear the dates and does not re-read the grids. */
window.wdRefresh = function (btn) {
    if (S.busy) return;
    busy(btn, true);
    clearMsg();
    loadSetup(true).then(function () {
        busy(btn, false);
        var fd = el('fromDate');
        if (fd) fd.focus();
    });
};

/* btnPrint_Click is an empty stub on the desktop (:909-912); here the button prints the page. */
window.wdPrint = function (btn) {
    if (S.busy) return;
    busy(btn, true);
    try { window.print(); } finally { busy(btn, false); }
};

window.wdToggleFullscreen = function () {
    document.body.classList.toggle('wd-full');
};

document.addEventListener('keydown', function (e) {
    if (e.key === 'Escape' && document.body.classList.contains('wd-full')) {
        e.preventDefault();
        document.body.classList.remove('wd-full');
    }
});

/* ---------------------------------------------------------------- boot */
function boot() {
    /* :265-270 - the desktop calls AllGridFill() during Load, BEFORE the dropdowns are filled,
       so the first read always runs with no filter. Same order here. */
    getJson(API + '/setup').then(function (d) {
        d = d || {};
        S.decimals = (typeof d.decimalsAmount === 'number' && d.decimalsAmount >= 1
                      && d.decimalsAmount <= 4) ? d.decimalsAmount : 2;
        S.rateDecimals = (typeof d.decimalsRate === 'number' && d.decimalsRate >= 1
                          && d.decimalsRate <= 4) ? d.decimalsRate : 2;
        setVal('fromDate', d.fromDate);
        setVal('toDate',   d.toDate);
        fillSelect('documentType', d.documentTypes, 'Reference Document Type');
        fillSelect('wagesAccount', d.wagesAccounts, 'Wages Account');
        fillSelect('contractor',   d.contractors,   'Wages Contractor');
        ['documentTypeError', 'wagesAccountError', 'contractorError'].forEach(function (k) {
            if (d[k]) msg(k.replace('Error', '') + ': ' + d[k], false);
        });
        if (window.jQuery && jQuery.fn.select2) {
            jQuery('.wd-select2').each(function () {
                var $s = jQuery(this);
                if ($s.data('select2')) $s.select2('destroy');
                $s.select2({ width: '100%', dropdownAutoWidth: true });
            });
        }
        var b = el('btnShow');
        if (b) wdShow(b);
    }).catch(function (e) {
        msg('Setup could not be read: ' + e.message, false);
    });
}

if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
else boot();

})();
