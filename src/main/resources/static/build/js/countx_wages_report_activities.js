/* =============================================================================
 * Wages Report (With Activities)
 * Ditto Architecture.WinApp.Pcc.Reports\WagesReportWithActivities.cs.
 * ============================================================================= */

var WRA_API = '/accounts/api/reports/wages-report-activities';
var wraColumns = [];
var wraRows = [];
var wraBusy = false;

/* Columns the desktop right-aligns and totals - Qty, AvgRate, Amount, PrcntOfTotal are the
   numeric columns every one of the twelve result shapes carries (form :528, :540 and the ten
   summary branches). Anything else is rendered as text. */
var WRA_NUMERIC = ['qty', 'avgrate', 'amount', 'prcntoftotal', 'rate', 'weight', 'bags'];

function wraEsc(s) {
    return String(s === null || s === undefined ? '' : s)
        .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
}

function wraYmd(d) {
    if (!d || isNaN(d.getTime())) return '';
    var m = d.getMonth() + 1, day = d.getDate();
    return d.getFullYear() + '-' + (m < 10 ? '0' + m : m) + '-' + (day < 10 ? '0' + day : day);
}

function wraIsNumeric(col) {
    return WRA_NUMERIC.indexOf(String(col || '').toLowerCase()) !== -1;
}

function wraFmt(col, v) {
    if (v === null || v === undefined || v === '') return '';
    if (wraIsNumeric(col)) {
        var n = parseFloat(v);
        if (isFinite(n)) return n.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    }
    /* ISO timestamps come back from date columns; show the date part only, as the desktop does
       with ToShortDateString(). */
    if (typeof v === 'string' && /^\d{4}-\d{2}-\d{2}T/.test(v)) return v.substring(0, 10);
    return v;
}

function wraMessage(text, ok) {
    var $m = $('#wraMessage');
    if (!text) { $m.hide().text(''); return; }
    $m.css({
        display: 'block',
        background: ok ? '#e6f4ea' : '#fdecea',
        color: ok ? '#0a6b2e' : '#a3160b',
        border: '1px solid ' + (ok ? '#a8d5b5' : '#f0b3ad')
    }).text(text);
}

function wraRun($btn, work) {
    if (wraBusy) return;
    wraBusy = true;
    var label = $btn.html();
    $btn.prop('disabled', true).html('<i class="fa fa-spinner fa-spin"></i>');
    var done = function () { wraBusy = false; $btn.prop('disabled', false).html(label); };
    try {
        var p = work();
        if (p && typeof p.always === 'function') { p.always(done); } else { done(); }
    } catch (e) { done(); wraMessage(e.message || String(e), false); }
}

/* ---------------------------------------------------------------- start-up */

$(document).ready(function () {
    $('.wra-select2').select2({ width: '100%', dropdownAutoWidth: true });
    wraLoadLookups();
});

function wraLoadLookups() {
    return $.get(WRA_API + '/lookups', function (d) {
        if (!d) return;

        var fill = function (sel, rows, blank) {
            var $s = $(sel).empty();
            if (blank) $s.append('<option value="">...Select Any Value...</option>');
            (rows || []).forEach(function (r) {
                $s.append($('<option>', { value: r.Id, text: r.Name }));
            });
            $s.trigger('change.select2');
        };
        fill('#Cmbplant', d.plants, true);
        fill('#cmbItem', d.items, true);
        fill('#cmbContractor', d.contractors, true);
        fill('#cmbServiceActivity', d.serviceActivities, true);

        /* ReportTypeFill() binds with ZeroIndex false and activates Rows[0] (:271, :353),
           so there is no blank row and the first type is selected. */
        var $rt = $('#cmbReportType').empty();
        (d.reportTypes || []).forEach(function (t) {
            $rt.append($('<option>', { value: t, text: t }));
        });
        $rt.trigger('change.select2');

        /* DateTypeFill() binds the five in-code rows and activates Rows[2] = "This Month". */
        var $dt = $('#cmbDateType').empty();
        (d.dateTypes || []).forEach(function (t, i) {
            $dt.append($('<option>', { value: i + 1, text: t }));
        });
        var def = (d.defaultDateTypeIndex === undefined || d.defaultDateTypeIndex === null)
                  ? 2 : d.defaultDateTypeIndex;
        $dt.val(String(def + 1));
        wraApplyDateType();
    }).fail(function () {
        wraMessage('Could not load the report filters.', false);
    });
}

/* The Date Type combo drives the two date boxes. Five options, matching
   CommonServices.DateType() (CommonServices.cs :15850-15863). "Financial Year" is left to the
   user because the active year's start date is a server-side value, not something to guess here. */
function wraApplyDateType() {
    var v = parseInt($('#cmbDateType').val() || '0', 10) || 0;
    var now = new Date();
    var from = null, to = new Date(now.getFullYear(), now.getMonth(), now.getDate());
    if (v === 1) {                                    // This Day
        from = new Date(now.getFullYear(), now.getMonth(), now.getDate());
    } else if (v === 2) {                             // This Week
        from = new Date(now.getFullYear(), now.getMonth(), now.getDate() - now.getDay());
    } else if (v === 3) {                             // This Month
        from = new Date(now.getFullYear(), now.getMonth(), 1);
    } else if (v === 4) {                             // This Year
        from = new Date(now.getFullYear(), 0, 1);
    }
    if (from) {
        $('#txtDateFrom').val(wraYmd(from));
        $('#txtDateTo').val(wraYmd(to));
    }
}

/* ---------------------------------------------------------------- run */

/* GridBind(), form :490-520 */
function wraShow() {
    wraRun($('#btnShow'), function () {
        var reportType = $('#cmbReportType').val();
        if (!reportType) { wraMessage('Select a Report Type.', false); return null; }
        wraMessage('', true);

        var req = {
            reportType: reportType,
            fromDate: $('#txtDateFrom').val(),
            toDate: $('#txtDateTo').val(),
            fromDocNo: $('#txtFromDoc').val(),
            toDocNo: $('#txtDocTo').val(),
            plantId: $('#Cmbplant').val(),
            itemId: $('#cmbItem').val(),
            contractorId: $('#cmbContractor').val(),
            serviceActivityId: $('#cmbServiceActivity').val(),
            actionId: $('input[name="radSource"]:checked').val()      /* 2 Sales, 1 Production, 0 Both */
        };

        $('#tblBody').html('<tr><td style="text-align:center; padding:16px;"><i class="fa fa-spinner fa-spin"></i> Loading…</td></tr>');

        return $.ajax({
            url: WRA_API, type: 'POST', contentType: 'application/json',
            data: JSON.stringify(req)
        }).done(function (res) {
            if (!res || res.success === false) {
                wraMessage((res && res.message) || 'The report could not be run.', false);
                wraRender([], []);
                return;
            }
            wraRender(res.columns || [], res.rows || []);
            if (!(res.rows || []).length) {
                wraMessage('No records found for the selected criteria.', true);
            }
        }).fail(function (xhr) {
            wraMessage('The report could not be run: ' + (xhr.responseText || xhr.statusText), false);
            wraRender([], []);
        });
    });
}

function wraRender(columns, rows) {
    wraColumns = columns;
    wraRows = rows;

    var $head = $('#tblHead').empty();
    var $body = $('#tblBody').empty();
    var $foot = $('#tblFoot').empty();

    if (!columns.length) {
        $head.append('<th>Choose a Report Type and press Show.</th>');
        $body.append('<tr><td style="text-align:center; padding:16px; color:#64748b;">No data.</td></tr>');
        $('#lblTotalRecords').text('0');
        $('#lblTotalQty').text('0.00');
        $('#lblTotalAmount').text('0.00');
        return;
    }

    columns.forEach(function (c) {
        $head.append('<th' + (wraIsNumeric(c) ? ' style="text-align:right;"' : '') + '>' + wraEsc(c) + '</th>');
    });

    var totals = {};
    columns.forEach(function (c) { if (wraIsNumeric(c)) totals[c] = 0; });

    rows.forEach(function (r) {
        var tds = columns.map(function (c) {
            var v = r[c];
            if (wraIsNumeric(c)) {
                var n = parseFloat(v);
                if (isFinite(n)) totals[c] += n;
                return '<td style="text-align:right;">' + wraEsc(wraFmt(c, v)) + '</td>';
            }
            return '<td>' + wraEsc(wraFmt(c, v)) + '</td>';
        }).join('');
        $body.append('<tr>' + tds + '</tr>');
    });

    /* Total band across the numeric columns the result actually carries. PrcntOfTotal is a
       percentage, so it is not summed. */
    var first = true;
    columns.forEach(function (c) {
        if (wraIsNumeric(c) && String(c).toLowerCase() !== 'prcntoftotal') {
            $foot.append('<td style="text-align:right; font-weight:bold;">' +
                         wraEsc(totals[c].toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 })) +
                         '</td>');
        } else if (first) {
            $foot.append('<td style="font-weight:bold;">Total</td>');
            first = false;
        } else {
            $foot.append('<td></td>');
        }
    });

    $('#lblTotalRecords').text(rows.length);
    var qtyKey = columns.filter(function (c) { return String(c).toLowerCase() === 'qty'; })[0];
    var amtKey = columns.filter(function (c) { return String(c).toLowerCase() === 'amount'; })[0];
    $('#lblTotalQty').text(qtyKey ? totals[qtyKey].toFixed(2) : '0.00');
    $('#lblTotalAmount').text(amtKey ? totals[amtKey].toFixed(2) : '0.00');
}

/* ---------------------------------------------------------------- misc */

function wraNew() {
    $('#txtFromDoc').val('');
    $('#txtDocTo').val('');
    $('#Cmbplant, #cmbItem, #cmbContractor, #cmbServiceActivity').val('').trigger('change.select2');
    $('#RadBoth').prop('checked', true);
    wraApplyDateType();
    wraMessage('', true);
    wraRender([], []);
}

function wraExportCsv() {
    if (!wraColumns.length || !wraRows.length) { wraMessage('Run the report first.', false); return; }
    var esc = function (v) {
        var s = (v === null || v === undefined) ? '' : String(v);
        return /[",\n]/.test(s) ? '"' + s.replace(/"/g, '""') + '"' : s;
    };
    var lines = [wraColumns.map(esc).join(',')];
    wraRows.forEach(function (r) {
        lines.push(wraColumns.map(function (c) { return esc(r[c]); }).join(','));
    });
    var blob = new Blob([lines.join('\n')], { type: 'text/csv;charset=utf-8;' });
    var a = document.createElement('a');
    a.href = URL.createObjectURL(blob);
    a.download = 'wages-report-with-activities.csv';
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(a.href);
}

function wraShortcuts() {
    alert('ShortCut Keys\n\nShow runs the report for the chosen Report Type.\nNew clears the filters.\nPrint prints the current result.');
}
