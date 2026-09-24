/* =============================================================================
 * Contractor Wages - Wages Rate Schedule Contractor Wise
 * Ditto Architecture.WinApp.Contractor_Wages\frmContractWiseWagesSchedule.cs.
 * Line references are into that file.
 * ============================================================================= */

var API = '/api/contractor-wages/schedule';
var cwsRecId = 0;
var cwsRows = [];
var cwsBusy = false;

/* ---------------------------------------------------------------- utilities */

function cwsEsc(s) {
    return String(s === null || s === undefined ? '' : s)
        .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
}

/* Local-calendar formatter - toISOString() would shift the date at a positive UTC offset. */
function cwsYmd(d) {
    if (!d || isNaN(d.getTime())) return '';
    var m = d.getMonth() + 1, day = d.getDate();
    return d.getFullYear() + '-' + (m < 10 ? '0' + m : m) + '-' + (day < 10 ? '0' + day : day);
}

function cwsDate(v) {
    if (!v) return '';
    var d = new Date(v);
    if (isNaN(d.getTime())) return String(v).substring(0, 10);
    var mon = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'][d.getMonth()];
    var day = d.getDate();
    return (day < 10 ? '0' + day : day) + '-' + mon + '-' + d.getFullYear();
}

/* FormatString "dd-MMM-yyyy hh:mm tt" (:639-641) */
function cwsDateTime(v) {
    if (!v) return '';
    var d = new Date(v);
    if (isNaN(d.getTime())) return String(v);
    var h = d.getHours(), ap = h >= 12 ? 'PM' : 'AM';
    h = h % 12; if (h === 0) h = 12;
    var mi = d.getMinutes();
    return cwsDate(v) + ' ' + (h < 10 ? '0' + h : h) + ':' + (mi < 10 ? '0' + mi : mi) + ' ' + ap;
}

function cwsNum(v, dp) {
    var n = parseFloat(v);
    if (!isFinite(n)) n = 0;
    return n.toLocaleString(undefined, { minimumFractionDigits: dp, maximumFractionDigits: dp });
}

function cwsCol(row, name) {
    if (!row) return null;
    if (row[name] !== undefined) return row[name];
    for (var k in row) {
        if (row.hasOwnProperty(k) && k.toLowerCase() === name.toLowerCase()) return row[k];
    }
    return null;
}

function cwsMessage(text, ok) {
    var $m = $('#cwMessage');
    $m.removeClass('ok err').addClass(ok ? 'ok' : 'err').text(text);
    if (ok) { setTimeout(function () { $m.removeClass('ok err').text(''); }, 4000); }
}

function cwsRun($btn, work) {
    if (cwsBusy) return;
    cwsBusy = true;
    $btn.prop('disabled', true).addClass('btn-busy');
    var done = function () { cwsBusy = false; $btn.prop('disabled', false).removeClass('btn-busy'); };
    try {
        var p = work();
        if (p && typeof p.always === 'function') { p.always(done); } else { done(); }
    } catch (e) {
        done();
        cwsMessage(e.message || String(e), false);
    }
}

/* ---------------------------------------------------------------- start-up */

$(document).ready(function () {
    var today = cwsYmd(new Date());
    $('#txtdate').val(today);
    $('#txtdateto').val(today);
    $('#histFromDate').val(today);
    $('#histToDate').val(today);

    $('.cws-select2').select2({ width: '100%', dropdownAutoWidth: true });

    cwsLoadContractors();
    cwsLoadAccounts();
    cwsLoadHistoryDropdowns();

    $('#grdWagesScheduleBody').on('dblclick', 'tr[data-id]', function () {
        cwsReadById(parseInt($(this).attr('data-id'), 10));
    });

    $(document).on('keydown', function (e) {
        if (!e.ctrlKey) return;
        var k = (e.key || '').toLowerCase();
        if (k === 's') { e.preventDefault(); cwsSave(); }
        else if (k === 'u') { e.preventDefault(); if ($('#btnUpdate').is(':visible')) cwsUpdate(); }
        else if (k === 'n') { e.preventDefault(); cwsNew(); }
    });
});

/* ---------------------------------------------------------------- lookups */

/* ContractorFill(), :221-256 */
function cwsLoadContractors() {
    return $.get(API + '/contractors', function (rows) {
        var $c = $('#Cmbcontractor').empty();
        $c.append('<option value="">...Select Any Value...</option>');
        (rows || []).forEach(function (r) {
            $c.append($('<option>', { value: cwsCol(r, 'Id'), text: cwsCol(r, 'CompanyName') }));
        });
        $c.trigger('change.select2');
    }).fail(function () { cwsMessage('Could not load Contractors.', false); });
}

/* accountName() - same source as the plain schedule form */
function cwsLoadAccounts() {
    return $.get(API + '/wages-accounts', function (rows) {
        var $a = $('#combAccountName').empty();
        $a.append('<option value="">...Select Any Value...</option>');
        (rows || []).forEach(function (r) {
            $a.append($('<option>', { value: cwsCol(r, 'Id'), text: cwsCol(r, 'WagesAccountName') }));
        });
        $a.trigger('change.select2');
    }).fail(function () { cwsMessage('Could not load Wages Accounts.', false); });
}

/* The history tab's two combos come from USP_GetDataForDropDownFromWagesSchedule, split on the
   Activity column server-side. They are filled ONLY here, so nothing else can wipe them. */
function cwsLoadHistoryDropdowns() {
    return $.get(API + '/history-dropdowns', function (data) {
        var $c = $('#CmbContractoryHistory').empty();
        var $w = $('#CmbWagesAccountHistory').empty();
        $c.append('<option value="">...Select Any Value...</option>');
        $w.append('<option value="">...Select Any Value...</option>');
        ((data && data.contractors) || []).forEach(function (r) {
            $c.append($('<option>', { value: r.Id, text: r.name }));
        });
        ((data && data.wagesAccounts) || []).forEach(function (r) {
            $w.append($('<option>', { value: r.Id, text: r.name }));
        });
        $c.trigger('change.select2');
        $w.trigger('change.select2');
    });
}

/* ---------------------------------------------------------------- grid */

/* The desktop rebinds as soon as EITHER combo has a value (:206, :407, :435). */
function cwsFilterChanged() {
    var acc = parseInt($('#combAccountName').val() || '0', 10) || 0;
    var con = parseInt($('#Cmbcontractor').val() || '0', 10) || 0;
    if (acc > 0 || con > 0) { cwsBindGrid(acc, con); }
    else { cwsRows = []; cwsRenderGrid('Select a Contractor or a Wages Account to load the schedule.'); }
    cwsLookupRate();
}

function cwsBindGrid(acc, con) {
    return $.get(API + '/contractor-wise', { wagesAccountId: acc, contractorId: con }, function (rows) {
        cwsRows = rows || [];
        cwsRenderGrid('No schedule rows for this selection.');
    }).fail(function () {
        cwsRows = [];
        cwsRenderGrid('Could not load the schedule.');
        cwsMessage('Could not load the schedule.', false);
    });
}

function cwsRenderGrid(emptyText) {
    var $b = $('#grdWagesScheduleBody').empty();
    if (!cwsRows.length) {
        $b.append('<tr><td colspan="15" style="text-align:center; padding:14px; color:#777;">' +
                  cwsEsc(emptyText) + '</td></tr>');
        return;
    }
    cwsRows.forEach(function (r) {
        var id = cwsCol(r, 'Id');
        var approved = cwsCol(r, 'IsApproved') === true || cwsCol(r, 'IsApproved') === 1;
        var entryUserId = parseInt(cwsCol(r, 'EntryUserId'), 10) || 0;
        $b.append(
            '<tr data-id="' + cwsEsc(id) + '" title="Double-click to edit">' +
            '<td><button type="button" class="win-btn" style="padding:1px 6px;" onclick="cwsReadById(' + id + ')">Edit</button></td>' +
            '<td>' + cwsEsc(cwsDate(cwsCol(r, 'EffectedDate'))) + '</td>' +
            '<td>' + cwsEsc(cwsDate(cwsCol(r, 'EffectedDateTo'))) + '</td>' +
            '<td>' + cwsEsc(cwsCol(r, 'WagesAccountName')) + '</td>' +
            '<td>' + cwsEsc(cwsCol(r, 'ContractName')) + '</td>' +
            '<td>' + cwsEsc(cwsCol(r, 'PackUomFrom')) + '</td>' +
            '<td>' + cwsEsc(cwsCol(r, 'PackUomTo')) + '</td>' +
            '<td style="text-align:right;">' + cwsNum(cwsCol(r, 'WageRate'), 2) + '</td>' +
            '<td>' + cwsEsc(cwsCol(r, 'EntryUser')) + '</td>' +
            '<td>' + cwsEsc(cwsDateTime(cwsCol(r, 'EntryDate'))) + '</td>' +
            '<td>' + cwsEsc(cwsCol(r, 'ModifyUser')) + '</td>' +
            '<td>' + cwsEsc(cwsDateTime(cwsCol(r, 'ModifyDate'))) + '</td>' +
            '<td>' + cwsEsc(cwsCol(r, 'ApprovedUser')) + '</td>' +
            '<td>' + cwsEsc(cwsDateTime(cwsCol(r, 'ApprovedDate'))) + '</td>' +
            '<td>' + (approved
                ? '<span style="color:#0a6b2e; font-weight:bold;">Approved</span>'
                : '<button type="button" class="win-btn" style="padding:1px 6px;" ' +
                  'onclick="cwsApprove(this,' + id + ',' + entryUserId + ')">Approve</button>') +
            '</td></tr>');
    });
}

/* ------------------------------------------------- Company Rate auto-fill */

/* txtPackUOMFrom_TextChanged (:379-400) and Cmbcontractor_Leave (:421-432). The desktop writes the
   looked-up rate into Company Rate ONLY when it is greater than zero (:391) - a miss leaves the
   box exactly as it was. Nothing is zeroed and no substitute rate is invented. */
function cwsLookupRate() {
    var acc = parseInt($('#combAccountName').val() || '0', 10) || 0;
    var pack = parseFloat($('#txtPackUOMFrom').val()) || 0;
    if (!(acc > 0 || pack > 0)) return;
    var con = parseInt($('#Cmbcontractor').val() || '0', 10) || 0;
    $.get(API + '/wages-rate', {
        effectedDate: $('#txtdate').val() || '',
        packUomFrom: pack, wagesAccountId: acc, contractorId: con
    }, function (d) {
        var rate = d && d.wagesRate !== null && d.wagesRate !== undefined ? parseFloat(d.wagesRate) : 0;
        if (isFinite(rate) && rate > 0) { $('#txtCompanyRate').val(rate); }
    });
}

/* ---------------------------------------------------------------- read one */

function cwsReadById(id) {
    if (!id) return;
    $.get(API + '/' + id, function (o) {
        if (!o || Object.keys(o).length === 0) { cwsMessage('Record not found.', false); return; }
        cwsTab('form');
        cwsRecId = id;
        $('#txtdate').val(String(cwsCol(o, 'EffectedDate') || '').substring(0, 10));
        $('#txtdateto').val(String(cwsCol(o, 'EffectedDateTo') || '').substring(0, 10));
        $('#Cmbcontractor').val(String(cwsCol(o, 'ContractorId') || '')).trigger('change.select2');
        $('#combAccountName').val(String(cwsCol(o, 'InvConractorWagesAccountsId') || '')).trigger('change.select2');
        $('#txtPackUOMFrom').val(cwsCol(o, 'PackUomFrom'));
        $('#txtPackUOMTO').val(cwsCol(o, 'PackUomTo'));
        $('#txtwagesrate').val(cwsCol(o, 'WageRate'));
        $('#txtCompanyRate').val(cwsCol(o, 'CompanyRate'));
        $('#btnSave').hide();
        $('#btnUpdate').show();
        $('#lblEditing').text('Editing schedule #' + id);
    }).fail(function () { cwsMessage('Could not open that schedule row.', false); });
}

/* ---------------------------------------------------------------- save */

function cwsCollect() {
    return {
        id: cwsRecId,
        contractorId: parseInt($('#Cmbcontractor').val() || '0', 10) || 0,
        wagesAccountId: parseInt($('#combAccountName').val() || '0', 10) || 0,
        packUomFrom: $('#txtPackUOMFrom').val(),
        packUomTo: $('#txtPackUOMTO').val(),
        wagesRate: $('#txtwagesrate').val(),
        companyRate: $('#txtCompanyRate').val(),
        effectedDate: $('#txtdate').val(),
        effectedDateTo: $('#txtdateto').val()
    };
}

/* FormValidation(), :446-479 - same order, same wording (including the lower-case "contractor"). */
function cwsValidate(p) {
    if (!p.contractorId)                return 'contractor Field is Required';
    if (!p.wagesAccountId)              return 'Account Field is Required';
    if (!parseFloat(p.packUomFrom))     return 'PackUOMFrom Field Required';
    if (!parseFloat(p.packUomTo))       return 'PackUOMTo Field Required';
    if (!parseFloat(p.wagesRate))       return 'Wages Rate Field Required';
    return null;
}

function cwsPost(payload) {
    return $.ajax({
        url: API + '/contractor-wise/save', type: 'POST', contentType: 'application/json',
        data: JSON.stringify(payload)
    }).done(function (res) {
        if (res && res.success) { cwsMessage(res.message, true); cwsFormRest(); }
        else { cwsMessage((res && res.message) || 'Save failed.', false); }
    }).fail(function (xhr) {
        cwsMessage('Save failed: ' + (xhr.responseText || xhr.statusText), false);
    });
}

/* btnsave_Click zeroes RecId first (:802-808), so Save is always an insert. */
function cwsSave() {
    cwsRun($('#btnSave'), function () {
        cwsRecId = 0;
        var p = cwsCollect();
        var err = cwsValidate(p);
        if (err) { cwsMessage(err, false); return null; }
        if (!confirm('Are you sure to Save?')) { return null; }
        return cwsPost(p);
    });
}

function cwsUpdate() {
    cwsRun($('#btnUpdate'), function () {
        var p = cwsCollect();
        var err = cwsValidate(p);
        if (err) { cwsMessage(err, false); return null; }
        if (!p.id) { cwsMessage('Open a row first, then press Update.', false); return null; }
        if (!confirm('Are you sure to Update?')) { return null; }
        return cwsPost(p);
    });
}

function cwsFormRest() {
    cwsRecId = 0;
    $('#txtPackUOMTO').val('0');
    $('#txtPackUOMFrom').val('0');
    $('#txtwagesrate').val('0');
    $('#txtCompanyRate').val('0');
    $('#btnUpdate').hide();
    $('#btnSave').show();
    $('#lblEditing').text('');
    cwsFilterChanged();
}

function cwsNew()     { cwsFormRest(); }
function cwsRefresh() { cwsLoadContractors(); cwsLoadAccounts(); cwsFilterChanged(); }

/* ---------------------------------------------------------------- approve */

function cwsApprove(btn, id, entryUserId) {
    cwsRun($(btn), function () {
        if (!confirm('Approve this schedule row?')) { return null; }
        return $.ajax({
            url: API + '/' + id + '/approve', type: 'POST', contentType: 'application/json',
            data: JSON.stringify({ entryUserId: entryUserId })
        }).done(function (res) {
            if (res && res.success) {
                cwsMessage('Approved.', true);
                cwsFilterChanged();
            } else { cwsMessage((res && res.message) || 'Approve failed.', false); }
        }).fail(function (xhr) {
            cwsMessage('Approve failed: ' + (xhr.responseText || xhr.statusText), false);
        });
    });
}

/* ---------------------------------------------------------------- history */

function cwsTab(which) {
    var form = which === 'form';
    $('#viewForm').toggle(form);
    $('#viewHistory').toggle(!form);
    $('#tabForm').toggleClass('active', form);
    $('#tabHistory').toggleClass('active', !form);
}

function cwsShowHistory() {
    cwsRun($('#btnShowHistory'), function () {
        var acc = parseInt($('#CmbWagesAccountHistory').val() || '0', 10) || 0;
        var con = parseInt($('#CmbContractoryHistory').val() || '0', 10) || 0;
        cwsMessage('', true);
        return $.get(API + '/contractor-wise', {
            wagesAccountId: acc, contractorId: con,
            fromDate: $('#histFromDate').val(), toDate: $('#histToDate').val()
        }, function (rows) {
            var filtered = rows || [];
            var $b = $('#grdHistoryBody').empty();
            $('#histCount').text(filtered.length);
            if (!filtered.length) {
                $b.append('<tr><td colspan="13" style="text-align:center; padding:14px; color:#777;">No records.</td></tr>');
                return;
            }
            filtered.forEach(function (r) {
                $b.append('<tr>' +
                    '<td>' + cwsEsc(cwsDate(cwsCol(r, 'EffectedDate'))) + '</td>' +
                    '<td>' + cwsEsc(cwsDate(cwsCol(r, 'EffectedDateTo'))) + '</td>' +
                    '<td>' + cwsEsc(cwsCol(r, 'WagesAccountName')) + '</td>' +
                    '<td>' + cwsEsc(cwsCol(r, 'ContractName')) + '</td>' +
                    '<td>' + cwsEsc(cwsCol(r, 'PackUomFrom')) + '</td>' +
                    '<td>' + cwsEsc(cwsCol(r, 'PackUomTo')) + '</td>' +
                    '<td style="text-align:right;">' + cwsNum(cwsCol(r, 'WageRate'), 2) + '</td>' +
                    '<td>' + cwsEsc(cwsCol(r, 'EntryUser')) + '</td>' +
                    '<td>' + cwsEsc(cwsDateTime(cwsCol(r, 'EntryDate'))) + '</td>' +
                    '<td>' + cwsEsc(cwsCol(r, 'ModifyUser')) + '</td>' +
                    '<td>' + cwsEsc(cwsDateTime(cwsCol(r, 'ModifyDate'))) + '</td>' +
                    '<td>' + cwsEsc(cwsCol(r, 'ApprovedUser')) + '</td>' +
                    '<td>' + cwsEsc(cwsDateTime(cwsCol(r, 'ApprovedDate'))) + '</td>' +
                '</tr>');
            });
        }).fail(function () { cwsMessage('Could not load history.', false); });
    });
}

/* ---------------------------------------------------------------- misc */

function cwsToggleFullscreen() { $('#cwsGridCard').toggleClass('cw-fullscreen'); }

function cwsShortcuts() {
    alert('ShortCut Keys\n\nCtrl+S  Save\nCtrl+U  Update\nCtrl+N  New\n\nDouble-click a grid row to open it for editing.');
}
