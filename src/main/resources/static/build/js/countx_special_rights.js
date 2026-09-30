/*
 * "Special Rights" toolbar button = desktop SpecialSreenRightsUserWise (Architecture.WinApp.Special_Rights.Configurations),
 * opened by the voucher forms' btnSpecialRights_Click with the form's ScreenId (screen combo disabled) and the
 * logged-in user (user combo disabled unless RoleName == "Admin").  Backend: /api/special-rights (SpecialScreenRightsService).
 *
 *   SpecialRights.open(screenId)                 - the dialog (Save / Update / New / Refresh, history grid, double-click edit)
 *   SpecialRights.mine(screenId) -> Promise rows - SpecialRightsImplement: [{RightId, IsActive, ...}] for this user
 */
(function (w, $) {
    'use strict';
    var API = '/api/special-rights';
    var st = { screenId: 0, recId: 0, isAdmin: false, userId: 0, busy: false };

    function esc(v) { return $('<div>').text(v == null ? '' : String(v)).html(); }
    function csrf() {
        var t = $('meta[name="_csrf"]').attr('content'), h = $('meta[name="_csrf_header"]').attr('content');
        var o = {}; if (t && h) o[h] = t; return o;
    }
    function v(r, k) { if (!r) return null; if (r[k] !== undefined) return r[k]; var l = k.toLowerCase(); for (var x in r) if (x.toLowerCase() === l) return r[x]; return null; }
    function err(x) { return (x && x.responseJSON && x.responseJSON.message) || ('Request failed (' + (x ? x.status : '?') + ')'); }
    function opts(rows, val, txt, blank) {
        var h = blank ? '<option value="0"></option>' : '';
        (rows || []).forEach(function (r) { h += '<option value="' + esc(v(r, val)) + '">' + esc(v(r, txt)) + '</option>'; });
        return h;
    }
    function busy(on) {
        st.busy = on;
        $('#srSave, #srUpdate').prop('disabled', on);
        $('#srSpin').toggle(on);
    }

    function ensureDom() {
        if ($('#srModal').length) return;
        var html =
            '<div id="srModal" style="display:none;position:fixed;inset:0;z-index:3000;background:rgba(0,0,0,.35);">' +
            '<div style="position:absolute;left:50%;top:60px;transform:translateX(-50%);width:640px;max-width:96vw;background:#fff;border:1px solid #008080;box-shadow:0 4px 16px rgba(0,0,0,.3);font-family:Tahoma,Verdana,sans-serif;font-size:11px;">' +
            '<div style="background:#00796B;color:#fff;padding:5px 8px;font-weight:bold;display:flex;justify-content:space-between;">' +
            '<span><i class="fa fa-key"></i> Special Screen Rights User Wise</span><a href="javascript:void(0)" id="srClose" style="color:#fff;">&#x2715;</a></div>' +
            '<div style="padding:4px 6px;border-bottom:1px solid #ccc;background:#f3f3f3;">' +
            '<button type="button" class="win-btn" id="srNew"><i class="fa fa-file-o"></i> New</button> ' +
            '<button type="button" class="win-btn" id="srRefresh"><i class="fa fa-refresh"></i> Refresh</button> ' +
            '<button type="button" class="win-btn" id="srSave"><i class="fa fa-save"></i> Save</button> ' +
            '<button type="button" class="win-btn" id="srUpdate" style="display:none;"><i class="fa fa-save"></i> Update</button> ' +
            '<span id="srSpin" style="display:none;"><i class="fa fa-spinner fa-spin"></i></span></div>' +
            '<div style="padding:8px 10px;display:grid;grid-template-columns:90px 1fr;gap:6px 8px;align-items:center;">' +
            '<label>Screen Name</label><select id="srScreen" class="win-input" disabled style="width:100%;"></select>' +
            '<label>User Name</label><select id="srUser" class="win-input" style="width:100%;"></select>' +
            '<label>Right Name</label><select id="srRight" class="win-input" style="width:100%;"></select>' +
            '<span></span><label style="font-weight:normal;"><input type="checkbox" id="srActive" checked/> Is Active</label></div>' +
            '<div style="background:#00796B;color:#fff;padding:3px 8px;font-weight:bold;">Detail</div>' +
            '<div style="max-height:300px;overflow:auto;"><table id="srGrid" style="width:100%;border-collapse:collapse;">' +
            '<thead><tr style="background:#dbe6f2;"><th style="padding:3px;border:1px solid #aaa;">ScreenName</th><th style="padding:3px;border:1px solid #aaa;">UserName</th>' +
            '<th style="padding:3px;border:1px solid #aaa;">RightsType</th><th style="padding:3px;border:1px solid #aaa;">RightsName</th><th style="padding:3px;border:1px solid #aaa;">IsActive</th></tr></thead>' +
            '<tbody></tbody></table></div></div></div>';
        $('body').append(html);
        $('#srClose').on('click', close);
        $('#srNew').on('click', reset);
        $('#srRefresh').on('click', refresh);
        $('#srSave').on('click', function () { st.recId = 0; save(); });
        $('#srUpdate').on('click', save);
        $('#srGrid').on('dblclick', 'tbody tr[data-id]', function () { edit(parseInt($(this).data('id'), 10)); });
        $(document).on('keydown.sr', function (e) {
            if (!$('#srModal').is(':visible')) return;
            if (e.key === 'Escape' || (e.ctrlKey && (e.key === 'e' || e.key === 'E'))) { e.preventDefault(); close(); }
            else if (e.ctrlKey && (e.key === 'n' || e.key === 'N')) { e.preventDefault(); reset(); }
            else if (e.ctrlKey && (e.key === 's' || e.key === 'S') && $('#srSave').is(':visible')) { e.preventDefault(); st.recId = 0; save(); }
            else if (e.ctrlKey && (e.key === 'u' || e.key === 'U') && $('#srUpdate').is(':visible')) { e.preventDefault(); save(); }
        });
    }

    function renderHistory(rows) {
        var h = '';
        (rows || []).forEach(function (r) {
            var a = v(r, 'IsActive'); a = (a === true || a === 1 || a === '1' || String(a).toLowerCase() === 'true') ? 'True' : 'False';
            h += '<tr data-id="' + esc(v(r, 'Id')) + '" style="cursor:pointer;"><td style="padding:2px 4px;border:1px solid #ddd;">' + esc(v(r, 'ScreenName')) +
                 '</td><td style="padding:2px 4px;border:1px solid #ddd;">' + esc(v(r, 'UserName')) +
                 '</td><td style="padding:2px 4px;border:1px solid #ddd;">' + esc(v(r, 'RightsType')) +
                 '</td><td style="padding:2px 4px;border:1px solid #ddd;">' + esc(v(r, 'RightsName')) +
                 '</td><td style="padding:2px 4px;border:1px solid #ddd;">' + a + '</td></tr>';
        });
        $('#srGrid tbody').html(h);
    }

    /* SreenRightsUserWise_Load: ScreenNameFill (value = ScreenId), UserNameFill (value = UserId), RightNameFill, FormHistory */
    function load() {
        return $.getJSON(API + '/lookups', { screenId: st.screenId }).then(function (L) {
            st.isAdmin = !!L.isAdmin; st.userId = L.userId;
            $('#srScreen').html(opts(L.screens, 'Id', 'ScreenName', true)).val(String(st.screenId));
            $('#srUser').html(opts(L.users, 'ID', 'UserName', true)).val(String(st.userId)).prop('disabled', !st.isAdmin);
            $('#srRight').html(opts(L.rights, 'Id', 'RightsType', true));
            renderHistory(L.history);
        }, function (x) { alert(err(x)); });
    }
    function refresh() {
        var u = $('#srUser').val(), r = $('#srRight').val();
        load().then(function () { if (u) $('#srUser').val(u); if (r) $('#srRight').val(r); });
    }
    /* Reset(): clear User / Right, Save visible, history re-read. The user combo keeps the logged-in user
       when it is disabled (non-Admin), because the desktop's disabled combo cannot be changed either. */
    function reset() {
        st.recId = 0;
        if (st.isAdmin) $('#srUser').val('0'); else $('#srUser').val(String(st.userId));
        $('#srRight').val('0'); $('#srActive').prop('checked', true);
        $('#srSave').show(); $('#srUpdate').hide();
        $.getJSON(API + '/history', { screenId: st.screenId }).then(renderHistory, function (x) { alert(err(x)); });
    }
    /* grdDetail_DoubleClick -> GetByID, Update mode */
    function edit(id) {
        $.getJSON(API + '/' + id).then(function (r) {
            st.recId = id;
            $('#srScreen').val(String(v(r, 'ScreenId')));
            $('#srUser').val(String(v(r, 'UserId')));
            $('#srRight').val(String(v(r, 'RightId')));
            var a = v(r, 'IsActive'); $('#srActive').prop('checked', a === true || a === 1 || String(a).toLowerCase() === 'true');
            $('#srSave').hide(); $('#srUpdate').show();
        }, function (x) { alert(err(x)); });
    }
    /* Insert(): FormValidation, confirm, Save, message, Reset */
    function save() {
        if (st.busy) return;
        if (!parseInt($('#srScreen').val() || 0, 10)) { alert('Screen Name Field is Required'); return; }
        if (!parseInt($('#srRight').val() || 0, 10)) { alert('Right Name Field is Required'); $('#srRight').focus(); return; }
        if (!parseInt($('#srUser').val() || 0, 10)) { alert('User Name Field is Required'); $('#srUser').focus(); return; }
        if (!confirm(st.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        busy(true);
        $.ajax({ url: API + '/save', type: 'POST', contentType: 'application/json', headers: csrf(),
                 data: JSON.stringify({ id: st.recId, screenId: parseInt($('#srScreen').val(), 10), rightId: parseInt($('#srRight').val(), 10),
                                        userId: parseInt($('#srUser').val(), 10), isActive: $('#srActive').is(':checked') }) })
            .done(function (res) { alert(res.message); reset(); })
            .fail(function (x) { alert(err(x)); })
            .always(function () { busy(false); });
    }

    function open(screenId) {
        ensureDom();
        st.screenId = parseInt(screenId, 10) || 0; st.recId = 0;
        $('#srSave').show(); $('#srUpdate').hide(); $('#srActive').prop('checked', true);
        $('#srModal').show();
        load();
    }
    function close() { $('#srModal').hide(); if (window.PRVSpecialRights) window.PRVSpecialRights(); }   // voucher re-applies its print defaults

    function mine(screenId) {
        return $.getJSON(API + '/mine', { screenId: screenId });
    }

    w.SpecialRights = { open: open, mine: mine };
})(window, jQuery);
