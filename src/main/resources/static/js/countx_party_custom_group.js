/* Party Custom Group - Architecture.WinApp.Account_Definition.frmPartyCustomGroup (screen 786).
   Event-for-event port; desktop line references are to frmPartyCustomGroup.cs. */
(function ($) {
    'use strict';
    var API = '/accounts/party-custom-group';
    /* PartyCustomGroupsAllocationToDocumentType has no web page yet (btnCustomGroupallocationToDocumentType_Click). Set to its route when it exists. */
    var DOC_TYPE_ROUTE = '';

    var RecId = 0, UpdateMood = false;

    function esc(v) { return CJG.esc(v); }
    function busy(btn, on) { var $b = $(btn); if (on) $b.prop('disabled', true).addClass('btn-busy'); else $b.removeClass('btn-busy').prop('disabled', false); }
    function isBusy(btn) { return $(btn).hasClass('btn-busy'); }
    function toInt(v) { var n = parseInt(v, 10); return isNaN(n) ? 0 : n; }
    function failText(x) {
        try { var j = x.responseJSON || JSON.parse(x.responseText); if (j && j.message) return j.message; } catch (e) { /* ignore */ }
        return x && x.status ? 'HTTP ' + x.status : 'Request failed';
    }
    function call(method, path, data, json) {
        return new Promise(function (resolve, reject) {
            var o = { url: API + path, type: method, dataType: 'json', cache: false };
            if (json) { o.contentType = 'application/json'; o.data = JSON.stringify(data); } else o.data = data || {};
            $.ajax(o).done(function (r) { if (r && r.success === false) reject(new Error(r.message || 'Error')); else resolve(r); })
                .fail(function (x) { reject(new Error(failText(x))); });
        });
    }
    function say(m) { if (m != null && m !== '') window.alert(m); }

    /* ---------------------------------------------------------------- grids (Janus GridEX) */
    var grdLookUp = CJG.create({
        table: '#grdLookUp', nav: '#navLookUp', groupBox: false,
        columns: [{ key: 'Id', hidden: true }, { key: 'Description', caption: 'Description', width: 300 }, { key: 'LookTypeId', hidden: true }],
        onDblClick: function (row) { grdLookUp_DoubleClick(row); }
    });
    function partyCols() {
        return [{ key: 'Select', selector: true, frozen: true, width: 30 }, { key: 'Id', hidden: true }, { key: 'SupplierCustomerId', hidden: true },
                { key: 'PartyCode', caption: 'PartyCode', width: 100 }, { key: 'PartyName', caption: 'PartyName', width: 270 }];
    }
    var grdAllocated = CJG.create({ table: '#grdAllocated', nav: '#navAllocated', groupBox: false, columns: partyCols() });
    var grdUnAllocated = CJG.create({ table: '#grdUnAllocated', nav: '#navUnAllocated', groupBox: false, columns: partyCols() });

    /* ---------------------------------------------------------------- GridBind() */
    function applyGrid(rows) {
        grdLookUp.setRows((rows || []).map(function (r) {
            return { Id: CJG.pick(r, 'Id'), Description: CJG.pick(r, 'AcLookUpsDescription'), LookTypeId: CJG.pick(r, 'AcLookUpTypesId') };
        }));
    }
    function gridBind() {
        return call('GET', '/grid').then(function (r) { applyGrid(r.gridGroups); }).catch(function (e) { say(e.message); });
    }

    /* ---------------------------------------------------------------- AccountTypeCombo() / LookUpBind() */
    function clearCombo(sel) { AccF.fillSelect(sel, [], 'Id', 'Description', ''); $(sel).val('').trigger('change'); }
    function hasId(rows, id) { return (rows || []).some(function (r) { return toInt(CJG.pick(r, 'Id')) === id; }); }
    function accountTypeCombo(rows) {
        var Id = toInt($('#cmbAccountTypes').val());
        if (rows && rows.length > 0) {
            AccF.fillSelect('#cmbAccountTypes', rows, 'Id', 'Description', '');            // BindDDL(dt, cmb, "Id", "Description", "Party Group", ZeroIndex false)
            if (Id > 0) {
                if (hasId(rows, Id)) $('#cmbAccountTypes').val(String(Id)).trigger('change');
                else $('#cmbAccountTypes').val('').trigger('change');                       // Text = string.Empty
            }
        } else {
            clearCombo('#cmbAccountTypes');                                                  // Text = ""; DataSource = null
        }
    }
    function lookUpBind(rows) {
        var Id = toInt($('#cmbGroupName').val());
        if (rows && rows.length > 0) {
            AccF.fillSelect('#cmbGroupName', rows, 'Id', 'AcLookUpsDescription', '');       // BindDDLNew(dtLookUp, cmb, "Id", "AcLookUpsDescription", "Account Group", false)
            if (Id > 0) {
                if (hasId(rows, Id)) $('#cmbGroupName').val(String(Id)).trigger('change');
                else $('#cmbAccountTypes').val('').trigger('change');                       // desktop clears cmbAccountTypes here (not cmbGroupName)
            }
        } else {
            clearCombo('#cmbAccountTypes');                                                  // desktop clears cmbAccountTypes here too
        }
    }

    /* BtnRefresh_Click: AccountTypeCombo(); LookUpBind(); */
    function btnRefresh() {
        return call('GET', '/refresh').then(function (r) { accountTypeCombo(r.partyGroups); lookUpBind(r.comboGroups); })
            .catch(function (e) { say(e.message); });
    }

    /* ---------------------------------------------------------------- Add / Update group */
    function setAddMode(update) {
        $('#btnAdd').text(update ? 'Update' : 'Add');
        $('#BtnSaveGroup span').html(update ? '<u>U</u>pdate' : '<u>S</u>ave');
    }
    /* Reset(): txtGroupName = "", UpdateMood = false, btnAdd "Add", BtnSaveGroup "Save", GridBind() */
    function resetAfterSave() { $('#txtGroupName').val(''); UpdateMood = false; setAddMode(false); return gridBind(); }

    function btnAdd_Click() {
        if (isBusy('#btnAdd')) return Promise.resolve();
        busy('#btnAdd', true);
        var update = $('#btnAdd').text() === 'Update';
        var step;
        var name = $('#txtGroupName').val();
        if (name.trim() === '') {                                          // Validation()
            say('Group Name Required'); $('#txtGroupName').focus();
            step = Promise.resolve();
        } else {
            step = call('POST', '/save', { id: update ? RecId : 0, update: update, name: name }).then(function (r) {
                if (r.saved) { say(r.message); return resetAfterSave(); }   // Success > 0
            }).catch(function (e) { say(e.message); });
        }
        /* LookUpBind(); then BtnRefresh_Click() (AccountTypeCombo + LookUpBind) - the desktop runs both even after a validation failure */
        return step.then(function () { return btnRefresh(); }).then(function () { busy('#btnAdd', false); });
    }
    /* btnClear_Click */
    function btnClear_Click() {
        setAddMode(false); $('#txtGroupName').val('');
        return gridBind();
    }
    /* grdLookUp_DoubleClick */
    function grdLookUp_DoubleClick(row) {
        if (!row) { say("Object reference not set to an instance of an object."); return; }
        RecId = toInt(row.Id);
        call('GET', '/group/' + RecId).then(function (r) {
            $('#txtGroupName').val(CJG.pick(r.group, 'AcLookUpsDescription') || '');
            setAddMode(true); UpdateMood = true;
        }).catch(function (e) { say(e.message); });
    }

    /* ---------------------------------------------------------------- Show / Allocate / Un-Allocate */
    function mapParty(rows) {
        return (rows || []).map(function (r) {
            return { Id: CJG.pick(r, 'CustomGroupId'), SupplierCustomerId: CJG.pick(r, 'SupplierCustomerId'), PartyCode: CJG.pick(r, 'PartyCode'), PartyName: CJG.pick(r, 'PartyName') };
        });
    }
    function btnShow_Click() {
        if (isBusy('#BtnShow')) return Promise.resolve();
        busy('#BtnShow', true);
        return call('GET', '/show', { groupId: toInt($('#cmbGroupName').val()), partyGroupId: toInt($('#cmbAccountTypes').val()) }).then(function (r) {
            /* UnAllocatedData() -> grdUnAllocated (USP_..._AllocatedData); AllocatedData() -> grdAllocated (USP_..._UnAllocatedData) */
            if (r.right && r.right.length > 0) grdUnAllocated.setRows(mapParty(r.right)); else grdUnAllocated.clear();
            if (r.left && r.left.length > 0) grdAllocated.setRows(mapParty(r.left)); else grdAllocated.clear();
        }).catch(function (e) { say(e.message); }).then(function () { busy('#BtnShow', false); });
    }
    function allocate() {
        var b = '#btnAddAc'; if (isBusy(b)) return Promise.resolve();
        var rows = grdAllocated.checked();
        if (rows.length === 0) { say('Checked row first'); return Promise.resolve(); }
        busy(b, true);
        return call('POST', '/allocate', { groupId: toInt($('#cmbGroupName').val()), ids: rows.map(function (r) { return toInt(r.SupplierCustomerId); }) }, true)
            .then(function (r) { say(r.message); })
            .catch(function (e) { say(e.message); })
            .then(function () { return btnShow_Click(); })
            .then(function () { busy(b, false); });
    }
    function unAllocate() {
        var b = '#btnDelete'; if (isBusy(b)) return Promise.resolve();
        var rows = grdUnAllocated.checked();
        if (rows.length === 0) { say('Checked row first'); return Promise.resolve(); }
        busy(b, true);
        return call('POST', '/unallocate', { rows: rows.map(function (r) { return { customGroupId: toInt(r.Id), supplierCustomerId: toInt(r.SupplierCustomerId) }; }) }, true)
            .then(function (r) { say(r.message); return btnShow_Click(); })
            .catch(function (e) { say(e.message); })
            .then(function () { busy(b, false); });
    }
    /* btnAcClear_Click */
    function btnAcClear_Click() {
        $('#cmbAccountTypes').val('').trigger('change'); $('#cmbGroupName').val('').trigger('change');
        grdAllocated.clear(); grdUnAllocated.clear();
    }

    /* ---------------------------------------------------------------- ShortCut keys */
    var SHORTCUTS = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+A', 'For Allocate'], ['Ctrl+Delete', 'For UnAllocate'], ['Ctrl+E', 'For Close'],
        ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+F5', 'For Focus on CustomGroup'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus On CustomGroup'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
        ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function shortcuts() {
        $('#scBody').html(SHORTCUTS.map(function (s) { return '<tr><td>' + esc(s[0]) + '</td><td>' + esc(s[1]) + '</td></tr>'; }).join(''));
        $('#scModal').css('display', 'flex');
    }
    function comboOpen() { return $('.dtcombo-pop').filter(function () { return this.style.display === 'block'; }).length > 0; }
    function closeForm() { window.location.href = '/accounts/dashboard'; }
    function focusCombo(sel) { AccF.focus(sel); }

    /* btnCustomGroupallocationToDocumentType_Click: new PartyCustomGroupsAllocationToDocumentType(UserAccount).Show() */
    function openDocType() {
        if (!DOC_TYPE_ROUTE) { say('Party Custom Groups Allocation To Document Type has no web page yet.'); return; }
        window.open(DOC_TYPE_ROUTE, '_blank');
    }

    /* ---------------------------------------------------------------- wiring */
    $('#btnAdd, #BtnSaveGroup').on('click', btnAdd_Click);
    $('#btnClear, #BtnResetGroup').on('click', btnClear_Click);
    $('#btnShortCutKeys').on('click', shortcuts);
    $('#BtnResetAllocatedUnAllocated, #btnAcClear').on('click', btnAcClear_Click);
    $('#BtnRefresh').on('click', btnRefresh);
    $('#BtnAllocate, #btnAddAc').on('click', allocate);
    $('#BtnDeAllocate, #btnDelete').on('click', unAllocate);
    $('#BtnShow').on('click', btnShow_Click);
    $('#btnCustomGroupallocationToDocumentType').on('click', openDocType);
    $('#scClose').on('click', function (e) { e.preventDefault(); $('#scModal').hide(); });

    /* frmPartyCustomGroup_KeyDown (KeyPreview) */
    $(document).on('keydown', function (e) {
        var k = e.key, K = (k || '').toUpperCase(), ctl = e.ctrlKey && !e.altKey;
        if ($('#scModal').is(':visible')) { if (k === 'Escape') { e.preventDefault(); $('#scModal').hide(); } return; }
        if (k === 'Enter' && !e.ctrlKey && !e.altKey && !e.shiftKey && !comboOpen() && !$(e.target).is('button, a, tr.flt input')) {
            /* SendKeys.Send("{TAB}") */
            var f = $('#frm').find('input:visible:not([readonly]):not([type=checkbox]), .dtcombo-input:visible, button:visible:enabled').toArray();
            var i = f.indexOf(e.target); if (i >= 0 && i < f.length - 1) { e.preventDefault(); f[i + 1].focus(); }
        }
        if (ctl && (K === 'S' || K === 'U')) { e.preventDefault(); btnAdd_Click(); }
        if (ctl && K === 'N') {                                  // grdAllocated.Focus() succeeds, so the desktop always takes the first branch
            e.preventDefault(); grdAllocated.focus(); btnAcClear_Click();
        }
        if ((ctl && K === 'E') || (k === 'Escape' && !comboOpen())) { e.preventDefault(); closeForm(); return; }
        if (e.ctrlKey && k === 'F5') { e.preventDefault(); grdAllocated.focus(); focusCombo('#cmbGroupName'); }
        if (e.ctrlKey && k === 'Delete') { e.preventDefault(); unAllocate(); }
        if (ctl && K === 'R') { e.preventDefault(); btnRefresh(); }
        if (e.ctrlKey && k === 'ArrowDown') {
            e.preventDefault();
            if ($('#panel4').has(document.activeElement).length) grdLookUp.focus(); else grdAllocated.focus();
        }
        if (e.ctrlKey && k === 'ArrowUp') { e.preventDefault(); grdAllocated.focus(); focusCombo('#cmbGroupName'); }
        if (e.ctrlKey && e.altKey && (k === 'Control' || k === 'Alt')) { e.preventDefault(); shortcuts(); }
    });

    /* frmPartyCustomGroup_Load: GridBind(); AccountTypeCombo(); LookUpBind(); */
    $(function () {
        call('GET', '/load').then(function (r) {
            applyGrid(r.gridGroups);
            accountTypeCombo(r.partyGroups);
            lookUpBind(r.comboGroups);
        }).catch(function (e) { say(e.message); });
        $('#txtGroupName').focus();
    });
})(window.jQuery);
