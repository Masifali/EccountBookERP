/* =====================================================================================
 * Contractor Wages - Wages Account
 *
 * Ported from Architecture.WinApp.Contractor_Wages\frmContractorWagesAccount.cs.
 * Line references below are that file.
 *
 *   Load (:107)        GetLookupsByTypeIdDt -> FillGrid -> focus -> GLAcoountBind -> GetActivityNature
 *   Validation (:182)  three checks, in order, with the product's own wording
 *   Save (:205)        Insert; Update (:304) is the same payload with Id set
 *   reset (:248)       Save shown, Update hidden, fields cleared, IsActive checked AND disabled
 *   Row edit (:362)    Save hidden, Update shown, fields filled, IsActive enabled
 *   Refresh (:462)     FillGrid + focus + GLAcoountBind  (NOT a full reset)
 *   Shortcuts (:403)   Ctrl+S save (only when not editing), Ctrl+N new, Ctrl+U update
 *
 * The desktop grid is read-only (AllowEdit / AllowDelete = False, :436-437), so there is no
 * delete and no in-grid editing here either.
 * ===================================================================================== */

var API = '/api/contractor-wages';

var wagesAccounts = [];      /* the rows behind the grid - the desktop's dtgetAll */
var selectedIdx = -1;
var recId = 0;               /* the desktop's recId */
var updateMode = false;      /* the desktop's UpdateMode */
var inFlight = {};

/* ------------------------------------------------------------------ helpers */

function $id(id) { return document.getElementById(id); }
function val(id) { var e = $id(id); return e ? e.value : ''; }
function intOf(id) { var n = parseInt(val(id), 10); return isNaN(n) ? 0 : n; }

function esc(s) {
    return String(s === undefined || s === null ? '' : s)
        .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;');
}

function message(text, isError) {
    var el = $id('cwMessage');
    if (!el) return;
    if (!text) { el.className = ''; el.textContent = ''; el.style.display = 'none'; return; }
    el.className = isError ? 'err' : 'ok';
    el.textContent = text;
}

/* The button contract: disable immediately, show a loader, block duplicate requests, keep it
   disabled while in flight, then stop the loader and restore state on success OR failure. */
function withButton(btnId, work) {
    if (inFlight[btnId]) return Promise.resolve();
    var b = $id(btnId);
    var wasDisabled = b ? b.disabled : false;
    inFlight[btnId] = true;
    if (b) { b.disabled = true; b.classList.add('btn-busy'); }

    var done = function () {
        inFlight[btnId] = false;
        if (b) { b.classList.remove('btn-busy'); b.disabled = wasDisabled; }
    };

    var result;
    try { result = work(); } catch (e) { done(); throw e; }
    if (result && typeof result.then === 'function') {
        return result.then(function (v) { done(); return v; },
                           function (e) { done(); throw e; });
    }
    done();
    return Promise.resolve(result);
}

function getJson(url) {
    return fetch(url, { headers: { 'Accept': 'application/json' } })
        .then(function (r) {
            if (!r.ok) throw new Error(url + ' returned ' + r.status);
            return r.json();
        });
}

/* ------------------------------------------------------------------ dropdowns */

/* Every option comes from the database. Nothing is hard-coded: when a list comes back empty the
   control says so rather than offering an invented value. */
function fillSelect(id, rows, placeholder, emptyText) {
    var sel = $id(id);
    if (!sel) return;
    sel.innerHTML = '';

    if (!rows || !rows.length) {
        sel.innerHTML = '<option value="0">' + esc(emptyText) + '</option>';
    } else {
        var html = '<option value="0">' + esc(placeholder) + '</option>';
        rows.forEach(function (r) {
            html += '<option value="' + r.id + '">' + esc(r.text) + '</option>';
        });
        sel.innerHTML = html;
    }
    if (window.jQuery && jQuery.fn.select2) jQuery(sel).trigger('change.select2');
}

function makeSearchable() {
    if (!(window.jQuery && jQuery.fn.select2)) return;
    jQuery('select.cw-searchable').each(function () {
        if (!jQuery(this).data('select2')) {
            jQuery(this).select2({ width: '100%', dropdownAutoWidth: true });
        }
    });
}

/* GLAcoountBind(), :126 - Sp_COAAllocation_GetAllMethod, AccountTypeIds 11,12,13,20,21 */
function loadGlAccounts() {
    return getJson(API + '/gl-accounts')
        .then(function (rows) {
            fillSelect('cmbGlAccount', rows, '-- Select Account Name --',
                       '-- No GL accounts available --');
        })
        .catch(function (e) { message('Could not load GL accounts: ' + e.message, true); });
}

/* GetLookupsByTypeIdDt(10), :142 - Sp_InvLookup_GetAllMethod */
function loadWagesTypes() {
    return getJson(API + '/wages-types')
        .then(function (rows) {
            fillSelect('cmbWagesType', rows, '-- Select Wages Type --',
                       '-- No wages types configured --');
        })
        .catch(function (e) { message('Could not load wages types: ' + e.message, true); });
}

/* GetActivityNature(), :158 - USP_DropDownActivityWages */
function loadActivityNatures() {
    return getJson(API + '/activity-natures')
        .then(function (rows) {
            fillSelect('cmbActivityNature', rows, '-- Select Activity Nature --',
                       '-- No activity natures configured --');
        })
        .catch(function (e) { message('Could not load activity natures: ' + e.message, true); });
}

/* ------------------------------------------------------------------ grid */

/* FillGrid(), :270 */
function fillGrid() {
    return getJson(API + '/accounts')
        .then(function (rows) {
            wagesAccounts = rows || [];
            renderGrid();
        })
        .catch(function (e) {
            wagesAccounts = [];
            renderGrid();
            message('Could not load wages accounts: ' + e.message, true);
        });
}

function renderGrid() {
    var body = $id('grdWagesAccountBody');
    if (!body) return;

    if (!wagesAccounts.length) {
        body.innerHTML = '<tr><td colspan="5" style="text-align:center; padding:14px; color:#777;">'
                       + 'No wages accounts found.</td></tr>';
    } else {
        body.innerHTML = wagesAccounts.map(function (r, i) {
            return '<tr' + (i === selectedIdx ? ' class="selected"' : '')
                 + ' ondblclick="cwEditRow(' + i + ')" onclick="cwSelectRow(' + i + ')">'
                 + '<td>' + esc(r.accountName) + '</td>'
                 + '<td>' + esc(r.activityType) + '</td>'
                 + '<td>' + esc(r.activityNature) + '</td>'
                 + '<td>' + esc(r.glAccount) + '</td>'
                 + '<td style="text-align:center;">' + (r.isActive ? 'true' : 'false') + '</td>'
                 + '</tr>';
        }).join('');
    }

    $id('lblRecordCount').textContent =
        (selectedIdx >= 0 ? (selectedIdx + 1) : 0) + ' of ' + wagesAccounts.length;
}

function cwSelectRow(i) { selectedIdx = i; renderGrid(); }

/* grdWagesAccount_DoubleClick, :362 */
function cwEditRow(i) {
    var r = wagesAccounts[i];
    if (!r) return;

    selectedIdx = i;
    recId = r.id;
    updateMode = true;

    $id('btnSave').style.display = 'none';       /* btnsave.Visible = false, :366 */
    $id('btnUpdate').style.display = '';         /* btnUpdate.Visible = true,  :367 */

    $id('txtWagesAccount').value = r.accountName || '';
    setSelect('cmbGlAccount', r.glAccountId);
    setSelect('cmbWagesType', r.wagesLookupId);

    /* The desktop assigns the activity NAME to this value-bound combo (:380), which works there
       only because an UltraCombo also matches on text. Prefer the id when the procedure supplies
       one, and fall back to matching the displayed name so behaviour is the same either way. */
    if (r.wagesActivityId > 0) setSelect('cmbActivityNature', r.wagesActivityId);
    else setSelectByText('cmbActivityNature', r.activityNature);

    var chk = $id('chkIsActive');
    chk.checked = !!r.isActive;
    chk.disabled = false;                        /* chkIsActive.Enabled = true, :383 */

    message('');
    renderGrid();
}

function setSelect(id, value) {
    var sel = $id(id);
    if (!sel) return;
    sel.value = String(value || 0);
    if (window.jQuery && jQuery.fn.select2) jQuery(sel).trigger('change.select2');
}

function setSelectByText(id, text) {
    var sel = $id(id);
    if (!sel) return;
    var wanted = String(text || '').trim().toLowerCase();
    sel.value = '0';
    for (var i = 0; i < sel.options.length; i++) {
        if (sel.options[i].text.trim().toLowerCase() === wanted) { sel.selectedIndex = i; break; }
    }
    if (window.jQuery && jQuery.fn.select2) jQuery(sel).trigger('change.select2');
}

/* ------------------------------------------------------------------ commands */

function buildPayload() {
    return {
        id: recId,
        accountName: val('txtWagesAccount').trim(),
        glAccountId: intOf('cmbGlAccount'),
        wagesLookupId: intOf('cmbWagesType'),
        wagesActivityId: intOf('cmbActivityNature'),
        isActive: $id('chkIsActive').checked
    };
}

/* FormValidation(), :182-204 - the same three checks, in the same order, with the product's own
   wording (including "WagesType", which is how the desktop spells it). The server repeats these,
   so a payload can never bypass them. */
function validate() {
    if (!val('txtWagesAccount').trim()) return { msg: 'Account Name Field Required', focus: 'txtWagesAccount' };
    if (intOf('cmbGlAccount') <= 0)     return { msg: 'GL Account Field is Required', focus: 'cmbGlAccount' };
    if (intOf('cmbWagesType') <= 0)     return { msg: 'WagesType Field is Required', focus: 'cmbWagesType' };
    return null;
}

function postSave(btnId) {
    return withButton(btnId, function () {
        message('');
        var err = validate();
        if (err) {
            message(err.msg, true);
            var f = $id(err.focus);
            if (f) f.focus();
            return;
        }

        return fetch(API + '/accounts/save', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(buildPayload())
        })
            .then(function (r) { return r.json().catch(function () { return { success: r.ok }; }); })
            .then(function (data) {
                if (data && data.success) {
                    message(data.message || 'Record Saved Successfully', false);
                    return reset();                     /* the desktop resets after both, :239 / :340 */
                }
                message((data && data.message) || 'The server rejected the save.', true);
            })
            .catch(function (e) { message('Save failed: ' + e.message, true); });
    });
}

function cwSave()   { return postSave('btnSave'); }
function cwUpdate() { return postSave('btnUpdate'); }

/* reset(), :248-268 */
function reset() {
    $id('btnSave').style.display = '';
    $id('btnUpdate').style.display = 'none';

    $id('txtWagesAccount').value = '';
    setSelect('cmbGlAccount', 0);
    setSelect('cmbWagesType', 0);
    setSelect('cmbActivityNature', 0);

    updateMode = false;
    recId = 0;
    selectedIdx = -1;

    var chk = $id('chkIsActive');
    chk.checked = true;          /* chkIsActive.Checked = true,  :259 */
    chk.disabled = true;         /* chkIsActive.Enabled = false, :260 */

    return fillGrid().then(function () { $id('txtWagesAccount').focus(); });
}

/* btnnew_Click, :393 */
function cwNew() { message(''); return withButton('btnNew', function () { return reset(); }); }

/* btnRefreshForm_Click, :462-474 - FillGrid, focus, GLAcoountBind. Deliberately NOT a full reset:
   the desktop leaves whatever is being typed in place. */
function cwRefresh() {
    return withButton('btnRefresh', function () {
        message('');
        return Promise.all([fillGrid(), loadGlAccounts()])
            .then(function () {
                makeSearchable();
                $id('txtWagesAccount').focus();
            });
    });
}

/* ------------------------------------------------------------------ chrome */

function cwToggleFullscreen() {
    var box = $id('cwGridBox');
    if (box) box.classList.toggle('cw-fullscreen');
}

/* frmContractorWagesAccount_KeyDown, :403-421. Ctrl+E / Escape close the desktop window; a browser
   tab has no equivalent, so they are not bound. */
document.addEventListener('keydown', function (e) {
    if (!e.ctrlKey) return;
    var k = (e.key || '').toLowerCase();

    if (k === 's' && !updateMode) { e.preventDefault(); cwSave(); }
    else if (k === 'n')           { e.preventDefault(); cwNew(); }
    else if (k === 'u' && updateMode) { e.preventDefault(); cwUpdate(); }
});

/* frmContractorWagesAccount_Load, :107-124 - same order as the desktop. */
document.addEventListener('DOMContentLoaded', function () {
    Promise.all([loadWagesTypes(), fillGrid(), loadGlAccounts(), loadActivityNatures()])
        .then(function () {
            makeSearchable();
            /* reset() is not called on load: the desktop's Load leaves IsActive in its designer
               state (checked, disabled) and simply focuses the name field. */
            $id('txtWagesAccount').focus();
        });
});
