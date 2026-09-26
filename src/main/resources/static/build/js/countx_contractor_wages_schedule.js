/* =============================================================================
 * Contractor Wages - Wages Rate Schedule
 * Ditto Architecture.WinApp.Contractor_Wages\frmContractWagesSchedule.cs.
 * Line references below are into that file.
 * ============================================================================= */

var API = '/api/contractor-wages/schedule';
var wsRecId = 0;                 /* the desktop's RecId */
var wsRows = [];                 /* current grdWagesSchedule rows */
var wsBusy = false;

/* ---------------------------------------------------------------- utilities */

function wsEsc(s) {
    return String(s === null || s === undefined ? '' : s)
        .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
}

/* Local-calendar formatter. toISOString() converts to UTC first, so at UTC+5 a local
   midnight date comes out as the previous day. */
function wsYmd(d) {
    if (!d || isNaN(d.getTime())) return '';
    var m = d.getMonth() + 1, day = d.getDate();
    return d.getFullYear() + '-' + (m < 10 ? '0' + m : m) + '-' + (day < 10 ? '0' + day : day);
}

function wsDate(v) {
    if (!v) return '';
    var d = new Date(v);
    if (isNaN(d.getTime())) return String(v).substring(0, 10);
    var mon = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'][d.getMonth()];
    var day = d.getDate();
    return (day < 10 ? '0' + day : day) + '-' + mon + '-' + d.getFullYear();
}

/* EntryDate / ModifyDate / ApprovedDate carry FormatString "dd-MMM-yyyy hh:mm tt" (:715-717) */
function wsDateTime(v) {
    if (!v) return '';
    var d = new Date(v);
    if (isNaN(d.getTime())) return String(v);
    var h = d.getHours(), ap = h >= 12 ? 'PM' : 'AM';
    h = h % 12; if (h === 0) h = 12;
    var mi = d.getMinutes();
    return wsDate(v) + ' ' + (h < 10 ? '0' + h : h) + ':' + (mi < 10 ? '0' + mi : mi) + ' ' + ap;
}

function wsNum(v, dp) {
    var n = parseFloat(v);
    if (!isFinite(n)) n = 0;
    return n.toLocaleString(undefined, { minimumFractionDigits: dp, maximumFractionDigits: dp });
}

/* Case-tolerant column read - these rows come straight out of the stored procedure. */
function wsCol(row, name) {
    if (!row) return null;
    if (row[name] !== undefined) return row[name];
    for (var k in row) {
        if (row.hasOwnProperty(k) && k.toLowerCase() === name.toLowerCase()) return row[k];
    }
    return null;
}

function wsMessage(text, ok) {
    var $m = $('#cwMessage');
    $m.removeClass('ok err').addClass(ok ? 'ok' : 'err').text(text);
    if (ok) { setTimeout(function () { $m.removeClass('ok err').text(''); }, 4000); }
}

/* One button at a time: disabled immediately, spinner on, re-enabled on success OR failure. */
function wsRun($btn, work) {
    if (wsBusy) return;
    wsBusy = true;
    $btn.prop('disabled', true).addClass('btn-busy');
    var done = function () {
        wsBusy = false;
        $btn.prop('disabled', false).removeClass('btn-busy');
    };
    try {
        var p = work();
        if (p && typeof p.always === 'function') { p.always(done); }
        else { done(); }
    } catch (e) {
        done();
        wsMessage(e.message || String(e), false);
    }
}

/* ---------------------------------------------------------------- start-up */

$(document).ready(function () {
    var today = wsYmd(new Date());
    $('#txtdate').val(today);
    $('#txtdateto').val(today);
    $('#histFromDate').val(today);
    $('#histToDate').val(today);

    $('.ws-select2').select2({ width: '100%', dropdownAutoWidth: true });

    wsLoadAccounts();
    wsLoadHistoryDropdowns();

    $('#grdWagesScheduleBody').on('dblclick', 'tr[data-id]', function () {
        wsReadById(parseInt($(this).attr('data-id'), 10));      /* :737-748 */
    });

    /* btnshortcutkeys (:1893) - the desktop's own three. */
    $(document).on('keydown', function (e) {
        if (!e.ctrlKey) return;
        var k = (e.key || '').toLowerCase();
        if (k === 's') { e.preventDefault(); wsSave(); }
        else if (k === 'u') { e.preventDefault(); if ($('#btnUpdate').is(':visible')) wsUpdate(); }
        else if (k === 'n') { e.preventDefault(); wsNew(); }
    });
});

/* ---------------------------------------------------------------- lookups */

/* accountName(), :229-268 */
function wsLoadAccounts() {
    return $.get(API + '/wages-accounts', function (rows) {
        /* Only the form combo is filled here. The history tab's account combo comes from
           USP_GetDataForDropDownFromWagesSchedule (wsLoadHistoryDropdowns) - touching it here too
           would let whichever request finished last wipe the other's options. */
        var $a = $('#combAccountName').empty();
        $a.append('<option value="">...Select Any Value...</option>');
        (rows || []).forEach(function (r) {
            var id = wsCol(r, 'Id');
            var name = wsCol(r, 'WagesAccountName');
            $a.append($('<option>', { value: id, text: name }));
        });
        $a.trigger('change.select2');
    }).fail(function () {
        wsMessage('Could not load Wages Accounts.', false);
    });
}

/* ComboBindFromWagesSchedule(), :291-360 - one call, split on the Activity column. */
function wsLoadHistoryDropdowns() {
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

function wsAccountChanged() {
    var id = parseInt($('#combAccountName').val() || '0', 10) || 0;
    if (id > 0) { wsBindGrid(id); }
    else {
        wsRows = [];
        wsRenderGrid('Select a Wages Account to load its schedule.');
    }
}

/* BindgrdWagesSchedule(WagesAccountId), :643-704 */
function wsBindGrid(wagesAccountId) {
    return $.get(API + '/by-account/' + wagesAccountId, function (rows) {
        wsRows = rows || [];
        wsRenderGrid('No schedule rows for this account.');
    }).fail(function () {
        wsRows = [];
        wsRenderGrid('Could not load the schedule.');
        wsMessage('Could not load the schedule for this account.', false);
    });
}

function wsRenderGrid(emptyText) {
    var $b = $('#grdWagesScheduleBody').empty();
    if (!wsRows.length) {
        $b.append('<tr><td colspan="16" style="text-align:center; padding:14px; color:#777;">' +
                  wsEsc(emptyText) + '</td></tr>');
        $('#totWagesRate').text('0.00');
        return;
    }
    var total = 0;
    wsRows.forEach(function (r) {
        var id = wsCol(r, 'Id');
        var approved = wsCol(r, 'IsApproved') === true || wsCol(r, 'IsApproved') === 1;
        var entryUserId = wsCol(r, 'EntryUserId');
        var rate = parseFloat(wsCol(r, 'WageRate'));
        if (isFinite(rate)) total += rate;
        var tr =
            '<tr data-id="' + wsEsc(id) + '" title="Double-click to edit">' +
            '<td><button type="button" class="win-btn" style="padding:1px 6px;" onclick="wsReadById(' + id + ')">Edit</button></td>' +
            '<td>' + wsEsc(wsDate(wsCol(r, 'EffectedDate'))) + '</td>' +
            '<td>' + wsEsc(wsDate(wsCol(r, 'EffectedDateTo'))) + '</td>' +
            '<td>' + wsEsc(wsCol(r, 'WagesAccountName')) + '</td>' +
            '<td>' + wsEsc(wsCol(r, 'WagesType')) + '</td>' +
            '<td>' + wsEsc(wsCol(r, 'ActivityNature')) + '</td>' +
            '<td>' + wsEsc(wsCol(r, 'PackUomFrom')) + '</td>' +
            '<td>' + wsEsc(wsCol(r, 'PackUomTo')) + '</td>' +
            '<td style="text-align:right;">' + wsNum(wsCol(r, 'WageRate'), 2) + '</td>' +
            '<td>' + wsEsc(wsCol(r, 'EntryUser')) + '</td>' +
            '<td>' + wsEsc(wsDateTime(wsCol(r, 'EntryDate'))) + '</td>' +
            '<td>' + wsEsc(wsCol(r, 'ModifyUser')) + '</td>' +
            '<td>' + wsEsc(wsDateTime(wsCol(r, 'ModifyDate'))) + '</td>' +
            '<td>' + wsEsc(wsCol(r, 'ApprovedUser')) + '</td>' +
            '<td>' + wsEsc(wsDateTime(wsCol(r, 'ApprovedDate'))) + '</td>' +
            /* :631 - the Approve button only acts when the row is NOT already approved. */
            '<td>' + (approved
                ? '<span style="color:#0a6b2e; font-weight:bold;">Approved</span>'
                : '<button type="button" class="win-btn" style="padding:1px 6px;" ' +
                  'onclick="wsApprove(this,' + id + ',' + (parseInt(entryUserId, 10) || 0) + ')">Approve</button>') +
            '</td></tr>';
        $b.append(tr);
    });
    $('#totWagesRate').text(wsNum(total, 2));
}

/* ---------------------------------------------------------------- read one */

/* ReadbyId(Id), :585-614 */
function wsReadById(id) {
    if (!id) return;
    $.get(API + '/' + id, function (o) {
        if (!o || Object.keys(o).length === 0) { wsMessage('Record not found.', false); return; }
        wsTab('form');
        wsRecId = id;
        $('#txtdate').val(String(wsCol(o, 'EffectedDate') || '').substring(0, 10));
        $('#txtdateto').val(String(wsCol(o, 'EffectedDateTo') || '').substring(0, 10));
        $('#combAccountName').val(String(wsCol(o, 'InvConractorWagesAccountsId') || '')).trigger('change.select2');
        $('#txtPackUOMFrom').val(wsCol(o, 'PackUomFrom'));
        $('#txtPackUOMTO').val(wsCol(o, 'PackUomTo'));
        $('#txtwagesrate').val(wsCol(o, 'WageRate'));
        /* FormRest()/ReadbyId leave Save hidden and Update shown while a row is open (:436-437). */
        $('#btnSave').hide();
        $('#btnUpdate').show();
        $('#lblEditing').text('Editing schedule #' + id);
    }).fail(function () {
        wsMessage('Could not open that schedule row.', false);
    });
}

/* ---------------------------------------------------------------- save */

function wsCollect() {
    return {
        id: wsRecId,
        wagesAccountId: parseInt($('#combAccountName').val() || '0', 10) || 0,
        packUomFrom: $('#txtPackUOMFrom').val(),
        packUomTo: $('#txtPackUOMTO').val(),
        wagesRate: $('#txtwagesrate').val(),
        effectedDate: $('#txtdate').val(),
        effectedDateTo: $('#txtdateto').val()
    };
}

/* FormValidation(), :393-420 - same order, same messages. */
function wsValidate(p) {
    if (!p.wagesAccountId)                            return 'Account Field is Required';
    if (!parseFloat(p.packUomFrom))                   return 'PackUOMFrom Field Required';
    if (!parseFloat(p.packUomTo))                     return 'PackUOMTo Field Required';
    if (!parseFloat(p.wagesRate))                     return 'Wages Rate Field Required';
    return null;
}

function wsPost(payload, $btn) {
    return $.ajax({
        url: API + '/save', type: 'POST', contentType: 'application/json',
        data: JSON.stringify(payload)
    }).done(function (res) {
        if (res && res.success) {
            wsMessage(res.message, true);          /* "Data Save Successfully." / "Data Update Successfully." */
            wsFormRest();
        } else {
            wsMessage((res && res.message) || 'Save failed.', false);
        }
    }).fail(function (xhr) {
        wsMessage('Save failed: ' + (xhr.responseText || xhr.statusText), false);
    });
}

/* btnsave_Click, :568-578 - RecId is zeroed first, so Save is always an insert. */
function wsSave() {
    var $btn = $('#btnSave');
    wsRun($btn, function () {
        wsRecId = 0;
        var p = wsCollect();
        var err = wsValidate(p);
        if (err) { wsMessage(err, false); return null; }
        if (!confirm('Are you sure to Save?')) { return null; }
        return wsPost(p, $btn);
    });
}

/* btnUpdate_Click, :580-590 - RecId is kept, so this is always an update. */
function wsUpdate() {
    var $btn = $('#btnUpdate');
    wsRun($btn, function () {
        var p = wsCollect();
        var err = wsValidate(p);
        if (err) { wsMessage(err, false); return null; }
        if (!p.id) { wsMessage('Open a row first, then press Update.', false); return null; }
        if (!confirm('Are you sure to Update?')) { return null; }
        return wsPost(p, $btn);
    });
}

/* FormRest(), :422-444 - the three numeric boxes go back to "0", the grid is rebound for the
   account still selected, Update hides and Save comes back. */
function wsFormRest() {
    wsRecId = 0;
    $('#txtPackUOMTO').val('0');
    $('#txtwagesrate').val('0');
    $('#txtPackUOMFrom').val('0');
    $('#btnUpdate').hide();
    $('#btnSave').show();
    $('#lblEditing').text('');
    var id = parseInt($('#combAccountName').val() || '0', 10) || 0;
    if (id > 0) { wsBindGrid(id); }
}

function wsNew()     { wsFormRest(); }                                   /* btnnew_Click :446 */
function wsRefresh() { wsLoadAccounts(); wsAccountChanged(); }           /* btnRefresh_Click :475 */

/* ---------------------------------------------------------------- approve */

function wsApprove(btn, id, entryUserId) {
    var $btn = $(btn);
    wsRun($btn, function () {
        if (!confirm('Approve this schedule row?')) { return null; }
        return $.ajax({
            url: API + '/' + id + '/approve', type: 'POST', contentType: 'application/json',
            data: JSON.stringify({ entryUserId: entryUserId })
        }).done(function (res) {
            if (res && res.success) {
                wsMessage('Approved.', true);
                var acc = parseInt($('#combAccountName').val() || '0', 10) || 0;
                if (acc > 0) wsBindGrid(acc);
            } else {
                wsMessage((res && res.message) || 'Approve failed.', false);
            }
        }).fail(function (xhr) {
            wsMessage('Approve failed: ' + (xhr.responseText || xhr.statusText), false);
        });
    });
}

/* ---------------------------------------------------------------- history tab */

function wsTab(which) {
    var form = which === 'form';
    $('#viewForm').toggle(form);
    $('#viewHistory').toggle(!form);
    $('#tabForm').toggleClass('active', form);
    $('#tabHistory').toggleClass('active', !form);
}

/* btnshowHistory_Click - the same GetAll the form grid uses, with the filter combos applied. */
function wsShowHistory() {
    var $btn = $('#btnShowHistory');
    wsRun($btn, function () {
        var acc = parseInt($('#CmbWagesAccountHistory').val() || '0', 10) || 0;
        if (!acc) {
            wsMessage('Select a Wages Account to show its history.', false);
            return null;
        }
        return $.get(API + '/by-account/' + acc, function (rows) {
            rows = rows || [];
            var contractor = parseInt($('#CmbContractoryHistory').val() || '0', 10) || 0;
            var from = $('#histFromDate').val();
            var to = $('#histToDate').val();
            var filtered = rows.filter(function (r) {
                if (contractor > 0 && (parseInt(wsCol(r, 'ContractorId'), 10) || 0) !== contractor) return false;
                var d = String(wsCol(r, 'EffectedDate') || '').substring(0, 10);
                if (from && d && d < from) return false;
                if (to && d && d > to) return false;
                return true;
            });
            var $b = $('#grdHistoryBody').empty();
            $('#histCount').text(filtered.length);
            if (!filtered.length) {
                $b.append('<tr><td colspan="14" style="text-align:center; padding:14px; color:#777;">No records.</td></tr>');
                return;
            }
            filtered.forEach(function (r) {
                $b.append('<tr>' +
                    '<td>' + wsEsc(wsDate(wsCol(r, 'EffectedDate'))) + '</td>' +
                    '<td>' + wsEsc(wsDate(wsCol(r, 'EffectedDateTo'))) + '</td>' +
                    '<td>' + wsEsc(wsCol(r, 'WagesAccountName')) + '</td>' +
                    '<td>' + wsEsc(wsCol(r, 'WagesType')) + '</td>' +
                    '<td>' + wsEsc(wsCol(r, 'ActivityNature')) + '</td>' +
                    '<td>' + wsEsc(wsCol(r, 'PackUomFrom')) + '</td>' +
                    '<td>' + wsEsc(wsCol(r, 'PackUomTo')) + '</td>' +
                    '<td style="text-align:right;">' + wsNum(wsCol(r, 'WageRate'), 2) + '</td>' +
                    '<td>' + wsEsc(wsCol(r, 'EntryUser')) + '</td>' +
                    '<td>' + wsEsc(wsDateTime(wsCol(r, 'EntryDate'))) + '</td>' +
                    '<td>' + wsEsc(wsCol(r, 'ModifyUser')) + '</td>' +
                    '<td>' + wsEsc(wsDateTime(wsCol(r, 'ModifyDate'))) + '</td>' +
                    '<td>' + wsEsc(wsCol(r, 'ApprovedUser')) + '</td>' +
                    '<td>' + wsEsc(wsDateTime(wsCol(r, 'ApprovedDate'))) + '</td>' +
                '</tr>');
            });
        }).fail(function () {
            wsMessage('Could not load history.', false);
        });
    });
}

/* ---------------------------------------------------------------- misc */

function wsToggleFullscreen() { $('#wsGridCard').toggleClass('cw-fullscreen'); }

function wsShortcuts() {
    alert('ShortCut Keys\n\nCtrl+S  Save\nCtrl+U  Update\nCtrl+N  New\n\nDouble-click a grid row to open it for editing.');
}
