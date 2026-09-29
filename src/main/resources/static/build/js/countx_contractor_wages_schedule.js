/* ============================================================================================
 * Wages Rate Schedule                 - Architecture.WinApp.Contractor_Wages/frmContractWagesSchedule.cs (2,248 lines)
 * Wages Rate Schedule Contractor Wise - Architecture.WinApp.Contractor_Wages/frmContractWiseWagesSchedule.cs (2,396 lines)
 *
 * One script, two forms: the page sets window.WS_MODE = 'plain' | 'contractor'. The two desktop
 * forms are the same class copied, so every difference is a flag here and is cited at its use.
 * Line numbers: plain form unless prefixed CW:.
 * ============================================================================================ */
(function () {
    'use strict';

    var CW = window.WS_MODE === 'contractor';
    var API = '/api/contractor-wages/schedule';
    var SCREEN = CW ? 'frmContractWiseWagesSchedule' : 'frmContractWagesSchedule';
    var GRID_ACTION = CW ? 1 : 2;            /* BindgrdWagesSchedule / BindGridHistory ActionId (CW:584, :654 / CW:975, :832) */
    var K = window.ReportKit;

    var RecId = 0, UpdateMode = false;
    var rights = { save: true, update: true };
    var fmtRate = 2;
    var dtFormHistory = [], dtHistory = [];   /* the rows behind the two grids (also what 06_Print prints) */
    var formArgs = null, histArgs = null;
    var tab = 0;

    function $id(id) { return document.getElementById(id); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return null;
    }
    function toI(v) { var n = parseInt(v, 10); return isNaN(n) ? 0 : n; }
    function toD(v) { if (v === null || v === undefined || v === '') return 0; var n = Number(String(v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function todayYmd() { var d = new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function shortDate(v) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || '')); return m ? m[3] + '/' + m[2] + '/' + m[1] : (v == null ? '' : String(v)); }
    var MON = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    /* FormatString "dd-MMM-yyyy hh:mm tt" */
    function dMMMyyyyhm(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})[T ](\d{2}):(\d{2})/.exec(String(v || ''));
        if (!m) return v == null ? '' : String(v);
        var h = +m[4], tt = h >= 12 ? 'PM' : 'AM'; h = h % 12; if (h === 0) h = 12;
        return m[3] + '-' + MON[+m[2] - 1] + '-' + m[1] + ' ' + pad(h) + ':' + m[5] + ' ' + tt;
    }
    function isTrue(v) { return v === true || v === 1 || String(v).toLowerCase() === 'true' || String(v) === '1'; }

    /* ------------------------------------------------------------------ MessageBox */
    function msg(text, title) {
        return dlg(text, title || '', ['OK']).then(function () {});
    }
    function confirmYesNo(text, title) {
        return dlg(text, title || 'Confirm', ['Yes', 'No']).then(function (b) { return b === 'Yes'; });
    }
    function dlg(text, title, buttons) {
        return new Promise(function (resolve) {
            var m = document.createElement('div');
            m.className = 'wr-modal';
            m.innerHTML = '<div class="wr-dlg" role="dialog"><div class="cap">' + esc(title) + '</div><div class="msg">' + esc(text)
                + '</div><div class="btns">' + buttons.map(function (b) { return '<button type="button" data-b="' + b + '">' + b + '</button>'; }).join(' ') + '</div></div>';
            document.body.appendChild(m);
            var first = m.querySelector('button'); first.focus();
            function done(v) { document.removeEventListener('keydown', key, true); m.remove(); resolve(v); }
            function key(e) { if (e.key === 'Escape') { e.preventDefault(); e.stopPropagation(); done(buttons[buttons.length - 1]); } }
            document.addEventListener('keydown', key, true);
            m.addEventListener('click', function (e) { var b = e.target.closest('button[data-b]'); if (b) done(b.getAttribute('data-b')); });
        });
    }
    function modalOpen() { return !!document.querySelector('.wr-modal, .rk-modal.is-open'); }

    /* ------------------------------------------------------------------ requests */
    function headers(h) {
        var t = document.querySelector('meta[name="_csrf"]'), hn = document.querySelector('meta[name="_csrf_header"]');
        if (t && hn && t.getAttribute('content')) h[hn.getAttribute('content')] = t.getAttribute('content');
        return h;
    }
    function handle(r) {
        return r.text().then(function (t) {
            var body = null;
            try { body = t ? JSON.parse(t) : null; } catch (e) { body = null; }
            if (!r.ok) throw new Error(body && body.message ? body.message : (t || ('Request failed (' + r.status + ')')));
            return body;
        });
    }
    function getJson(url) { return fetch(url, { credentials: 'same-origin', headers: { 'Accept': 'application/json' } }).then(handle); }
    function postJson(url, body) {
        return fetch(url, { method: 'POST', credentials: 'same-origin',
            headers: headers({ 'Content-Type': 'application/json', 'Accept': 'application/json' }),
            body: JSON.stringify(body) }).then(handle);
    }
    function q(o) { return Object.keys(o).filter(function (k) { return o[k] !== null && o[k] !== undefined && o[k] !== ''; })
        .map(function (k) { return encodeURIComponent(k) + '=' + encodeURIComponent(o[k]); }).join('&'); }
    function busy(id, fn) {
        var b = $id(id);
        if (b && b.classList.contains('is-busy')) return Promise.resolve();
        var was = b ? b.disabled : false;
        if (b) { b.classList.add('is-busy'); b.disabled = true; }
        var done = function () { if (b) { b.classList.remove('is-busy'); b.disabled = was; } };
        var p; try { p = Promise.resolve(fn()); } catch (e) { p = Promise.reject(e); }
        return p.then(function (v) { done(); return v; }, function (e) { done(); throw e; });
    }

    /* ------------------------------------------------------------------ combos (DDL.BindDDL, ZeroIndex:true) */
    function bind(sel, rows, idKey, textKey, keepValue) {
        var old = sel.value;
        var h = '<option value="0">...Select Any Value...</option>';
        (rows || []).forEach(function (r) { h += '<option value="' + esc(col(r, idKey)) + '">' + esc(col(r, textKey)) + '</option>'; });
        sel.innerHTML = h;
        /* accountName :252-262 / ComboBindFromWagesSchedule :330-338 - re-select the old Id if it still exists, else Text = "" */
        sel.value = (keepValue && toI(old) > 0 && sel.querySelector('option[value="' + old + '"]')) ? old : '0';
        fire(sel);
    }
    function unbind(sel) { sel.innerHTML = '<option value="0">...Select Any Value...</option>'; sel.value = '0'; fire(sel); }
    function fire(sel) { sel.dispatchEvent(new Event('change', { bubbles: true })); }
    function val(id) { return toI($id(id).value); }
    function numText(id) { return toD($id(id).value); }

    /* ================================================================================ Load :189-227 / CW:181-218 */
    function Load() {
        return getJson(API + '/rights?screen=' + SCREEN).catch(function () { return rights; }).then(function (r) {
            rights = r || rights;
            $id('btnsave').disabled = !rights.save;              /* btnsave.Enabled = formright.DoHaveSaveRight */
            $id('btnUpdate').disabled = !rights.update;          /* btnUpdate.Enabled = formright.DoHaveUpdateRights */
            var chain = CW ? ContractorFill().then(ComboBindFromWagesSchedule)
                           : accountName().then(ComboBindFromWagesSchedule);
            return chain;
        }).then(function () {
            if (CW) {
                if (val('combAccountName') > 0 || val('Cmbcontractor') > 0) return BindgrdWagesSchedule();
            } else if (val('combAccountName') > 0) {
                return BindgrdWagesSchedule();
            }
        }).then(function () {
            $id('btnsave').style.display = '';
            $id('btnUpdate').style.display = 'none';
            if (CW) $id('txtCompanyRate').disabled = true;        /* CW:214 txtCompanyRate.Enabled = false */
            $id('txtdate').focus();
        }).catch(function (e) { return msg(e.message); });
    }

    /** accountName() :229-281 - Sp_InvConractorWagesAccounts_GetAllMethod, ActionId 1 (plain form only). */
    function accountName() {
        return getJson(API + '/wages-accounts').then(function (rows) {
            if (!rows || !rows.length) return;                       /* :239 - returns, combo untouched */
            bind($id('combAccountName'), rows, 'Id', 'WagesAccountName', true);
        });
    }
    /** CW ContractorFill() :221-256 - SupplierCustomer.ReadByOrganizationCompanyIdForContractorWages. */
    function ContractorFill() {
        return getJson(API + '/contractors').then(function (rows) {
            var sel = $id('Cmbcontractor');
            if (rows && rows.length) bind(sel, rows, 'Id', 'CompanyName', true);
            else unbind(sel);                                         /* CW:245-246 Text = "", DataSource = null */
        });
    }
    /** ComboBindFromWagesSchedule() :291-391 / CW:260-377 - USP_GetDataForDropDownFromWagesSchedule split on Activity.
        On the Contractor Wise form the FORM's Wages Account combo is bound from this list too (CW:331). */
    function ComboBindFromWagesSchedule() {
        return getJson(API + '/history-dropdowns').then(function (d) {
            var c = (d && d.contractors) || [], w = (d && d.wagesAccounts) || [];
            if (!c.length && !w.length) {
                unbind($id('CmbContractoryHistory')); unbind($id('CmbWagesAccountHistory'));
                if (CW) unbind($id('combAccountName'));
                return;
            }
            if (c.length) bind($id('CmbContractoryHistory'), c, 'Id', 'name', true); else unbind($id('CmbContractoryHistory'));
            if (w.length) {
                bind($id('CmbWagesAccountHistory'), w, 'Id', 'name', true);
                if (CW) bind($id('combAccountName'), w, 'Id', 'name', true);
            } else {
                unbind($id('CmbWagesAccountHistory'));
                if (CW) unbind($id('combAccountName'));
            }
        });
    }

    /* ================================================================================ form grid */
    /** BindgrdWagesSchedule :643-704 (ActionId 2, by account) / CW:570-629 (ActionId 1, account and/or contractor). */
    function BindgrdWagesSchedule() {
        var acc = val('combAccountName'), con = CW ? val('Cmbcontractor') : 0;
        formArgs = { wagesAccountId: acc || null, contractorId: con || null, actionId: GRID_ACTION };
        return getJson(API + '/history?' + q({ wagesAccountId: acc, contractorId: con, actionId: GRID_ACTION })).then(function (rows) {
            dtFormHistory = rows || [];
            renderGrid('grdWagesSchedule', dtFormHistory, 'Form');
        }).catch(function (e) { dtFormHistory = []; renderGrid('grdWagesSchedule', [], 'Form'); return msg(e.message); });
    }
    function clearFormGrid() { dtFormHistory = []; renderGrid('grdWagesSchedule', [], 'Form'); }   /* grdWagesSchedule.ClearStructure() */

    /* grdScheduleSetings :706-735 / HistorySetings :887-912 (same list on CW with ContractName instead of WagesType/ActivityNature) */
    function columns() {
        var c = [{ k: 'Edit', btn: 'Edit', w: CW ? 40 : 50 },
                 { k: 'EffectedDate', t: 'd', w: 80 }, { k: 'EffectedDateTo', t: 'd', w: 80 },
                 { k: 'WagesAccountName', cap: 'Labour / Wages Activity', w: 180 }];
        if (CW) c.push({ k: 'ContractName', w: 150 });
        else c.push({ k: 'WagesType', w: 100 }, { k: 'ActivityNature', w: 100 });
        c.push({ k: 'PackUomFrom', w: 80 }, { k: 'PackUomTo', w: 80 },
               { k: 'WagesRate', t: 'rate', w: 90, sum: !CW },        /* :724-727 AggregateFunction Sum on the plain form only */
               { k: 'EnteredBy', w: 100 }, { k: 'EntryDate', t: 'dt', w: 150 },
               { k: 'ModifyBy', w: 100 }, { k: 'ModifyDate', t: 'dt', w: 150 },
               { k: 'ApprovedBy', w: 100 }, { k: 'ApprovedDate', t: 'dt', w: 150 },
               { k: 'Approve/UnApprove', btn: 'approve', w: 70 });
        return c;
    }
    function cell(c, r) {
        var v;
        switch (c.k) {
            case 'EnteredBy': v = col(r, 'EntryUser'); break;
            case 'ModifyBy': v = col(r, 'ModifyUser'); break;
            case 'ApprovedBy': v = col(r, 'ApprovedUser'); break;
            case 'WagesRate': v = col(r, 'WageRate'); break;
            default: v = col(r, c.k);
        }
        if (v === null || v === undefined || v === '') return '';
        if (c.t === 'd') return shortDate(v);
        if (c.t === 'dt') return dMMMyyyyhm(v);
        if (c.t === 'rate') return K.fixed(toD(v), fmtRate);
        return String(v);
    }
    function renderGrid(tableId, rows, which) {
        var tb = $id(tableId);
        if (!rows.length) { tb.innerHTML = ''; return; }            /* ClearStructure() */
        var cols = columns();
        var h = '<colgroup>' + cols.map(function (c) { return '<col style="width:' + c.w + 'px">'; }).join('') + '</colgroup><thead><tr>'
            + cols.map(function (c) { return '<th>' + esc(c.cap || (c.btn ? (c.btn === 'Edit' ? 'Edit' : 'Approve') : c.k)) + '</th>'; }).join('') + '</tr></thead><tbody>';
        rows.forEach(function (r, i) {
            var approved = isTrue(col(r, 'IsApproved'));
            h += '<tr data-i="' + i + '">' + cols.map(function (c) {
                if (c.btn === 'Edit') return '<td class="frz"><button type="button" class="cb" data-edit>Edit</button></td>';
                /* gridhistory_FormattingRow :1243-1272 - the button reads UnApprove once the row is approved */
                if (c.btn === 'approve') return '<td><button type="button" class="cb" data-approve>' + (approved ? 'UnApprove' : 'Approve') + '</button></td>';
                var t = cell(c, r);
                return '<td' + (c.t === 'rate' ? ' class="num"' : '') + ' title="' + esc(t) + '">' + esc(t) + '</td>';
            }).join('') + '</tr>';
        });
        h += '</tbody><tfoot><tr>' + cols.map(function (c) {
            if (!c.sum) return '<td></td>';
            var s = 0; rows.forEach(function (r) { s += toD(col(r, 'WageRate')); });
            return '<td class="num">' + esc(K.fixed(s, fmtRate)) + '</td>';
        }).join('') + '</tr></tfoot>';
        tb.innerHTML = h;
        tb.setAttribute('data-which', which);
    }

    /* grdWagesSchedule_ColumnButtonClick :615-641 / gridhistory_ColumnButtonClick :932-958 / DoubleClick :737, :914 */
    function gridClick(e, rows, which) {
        var tr = e.target.closest('tbody tr[data-i]');
        if (!tr) return;
        var r = rows[toI(tr.getAttribute('data-i'))];
        if (e.target.closest('button[data-edit]')) { RecId = toI(col(r, 'Id')); return ReadbyId(RecId); }
        if (e.target.closest('button[data-approve]')) {
            if (!isTrue(col(r, 'IsApproved'))) return ApproveUnApprove(toI(col(r, 'Id')), 'Approve', toI(col(r, 'EntryUserId')), which);
        }
    }
    function gridDblClick(e, rows) {
        var tr = e.target.closest('tbody tr[data-i]');
        if (!tr || e.target.closest('button')) return;
        RecId = toI(col(rows[toI(tr.getAttribute('data-i'))], 'Id'));
        return ReadbyId(RecId);
    }

    /** ReadbyId :588-613 / CW:827-854 */
    function ReadbyId(id) {
        return getJson(API + '/' + id).then(function (o) {
            if (!o || !Object.keys(o).length) return;
            RecId = id;
            selectTab(0);
            $id('txtdate').value = String(col(o, 'EffectedDate') || '').substring(0, 10);
            $id('txtdateto').value = String(col(o, 'EffectedDateTo') || '').substring(0, 10);
            setVal('combAccountName', col(o, 'InvConractorWagesAccountsId'));
            if (CW) setVal('Cmbcontractor', col(o, 'ContractorId'));
            $id('txtPackUOMFrom').value = String(toD(col(o, 'PackUomFrom')));
            $id('txtPackUOMTO').value = String(toD(col(o, 'PackUomTo')));
            if (CW) $id('txtCompanyRate').value = String(toD(col(o, 'CompanyRate')));
            $id('txtwagesrate').value = String(toD(col(o, 'WageRate')));
            UpdateMode = true;
            $id('btnsave').style.display = 'none';
            $id('btnUpdate').style.display = '';
            return BindgrdWagesSchedule();
        }).catch(function (e) { return msg(e.message); });
    }
    function setVal(id, v) { var s = $id(id); s.value = String(toI(v)); if (!s.querySelector('option[value="' + s.value + '"]')) s.value = '0'; fire(s); }

    /** ApproveUnApprove :960-1003 */
    function ApproveUnApprove(id, reqType, entryUserId, which) {
        return confirmYesNo('Are you sure to ' + reqType + '?', 'Confirm').then(function (yes) {
            if (!yes) return;
            return postJson(API + '/' + id + '/approve', { entryUserId: entryUserId, reqType: reqType }).then(function (res) {
                if (!res || !res.success) throw new Error((res && res.message) || 'Approve failed.');
                return msg('Record Updated Successfully!').then(function () {
                    return which === 'History' ? BindGridHistory() : BindgrdWagesSchedule();
                });
            });
        }).catch(function (e) { return msg(e.message); });
    }

    /* ================================================================================ validation / save */
    /** FormValidation :393-420 / CW:446-479 (CW starts with the contractor, lower-case "c" is the desktop's) */
    function FormValidation() {
        if (CW && val('Cmbcontractor') === 0) return msg('contractor Field is Required').then(function () { $id('Cmbcontractor').focus(); return false; });
        if (val('combAccountName') === 0) return msg('Account Field is Required').then(function () { $id('combAccountName').focus(); return false; });
        if (numText('txtPackUOMFrom') === 0) return msg('PackUOMFrom Field Required').then(function () { $id('txtPackUOMFrom').focus(); return false; });
        if (numText('txtPackUOMTO') === 0) return msg('PackUOMTo Field Required').then(function () { $id('txtPackUOMTO').focus(); return false; });
        if (numText('txtwagesrate') === 0) return msg('Wages Rate Field Required').then(function () { $id('txtwagesrate').focus(); return false; });
        return Promise.resolve(true);
    }
    /** Insert :510-561 / CW:748-800 */
    function Insert() {
        return FormValidation().then(function (ok) {
            if (!ok) return;
            return confirmYesNo(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
                if (!yes) return;
                var body = {
                    id: RecId,
                    effectedDate: $id('txtdate').value, effectedDateTo: $id('txtdateto').value,
                    wagesAccountId: val('combAccountName'),
                    packUomFrom: numText('txtPackUOMFrom'), packUomTo: numText('txtPackUOMTO'),
                    wagesRate: numText('txtwagesrate')
                };
                if (CW) { body.contractorId = val('Cmbcontractor'); body.companyRate = numText('txtCompanyRate'); }
                return postJson(API + (CW ? '/contractor-wise/save' : '/save'), body).then(function (res) {
                    if (!res || !res.success) throw new Error((res && res.message) || 'Save failed.');
                    return msg(RecId > 0 ? 'Data Update Successfully.' : 'Data Save Successfully.').then(FormRest);
                });
            });
        }).catch(function (e) { return msg(e.message, 'Database Error'); });
    }
    function btnsave_Click() { if (!rights.save) return; RecId = 0; return busy('btnsave', Insert); }        /* :563-574 */
    function btnUpdate_Click() { if (!rights.update) return; return busy('btnUpdate', Insert); }              /* :576-586 */

    /** FormRest :422-444 (binds the grid, then ClearStructure - the grid ends EMPTY on this form) /
        CW:541-563 (ClearStructure first, then binds - the grid ends FILLED). Reproduced as written. */
    function FormRest() {
        RecId = 0;
        $id('txtPackUOMTO').value = '0';
        $id('txtwagesrate').value = '0';
        $id('txtPackUOMFrom').value = '0';
        UpdateMode = false;
        $id('btnUpdate').style.display = 'none';
        $id('btnsave').style.display = '';
        if (CW) {
            $id('Cmbcontractor').focus();
            clearFormGrid();
            if (val('combAccountName') > 0 || val('Cmbcontractor') > 0) return BindgrdWagesSchedule();
            return Promise.resolve();
        }
        var p = val('combAccountName') > 0 ? BindgrdWagesSchedule() : Promise.resolve();
        $id('combAccountName').focus();
        return p.then(clearFormGrid);
    }

    /* ================================================================================ history tab */
    /** BindGridHistory :819-885 / CW:956-1017 - dates only when the picker is ticked (a blank date input = unticked). */
    function BindGridHistory() {
        histArgs = { fromDate: $id('FromDateHistory').value || null, toDate: $id('txtToDateHistory').value || null,
                     contractorId: val('CmbContractoryHistory') || null, wagesAccountId: val('CmbWagesAccountHistory') || null,
                     actionId: GRID_ACTION };
        return getJson(API + '/history?' + q(histArgs)).then(function (rows) {
            dtHistory = rows || [];
            renderGrid('gridhistory', dtHistory, 'History');
        }).catch(function (e) { dtHistory = []; renderGrid('gridhistory', [], 'History'); return msg(e.message); });
    }
    /** Resethistory :767-781 */
    function Resethistory() {
        $id('FromDateHistory').value = '';
        $id('txtToDateHistory').value = '';
        $id('CmbContractoryHistory').value = '0'; fire($id('CmbContractoryHistory'));
        $id('CmbWagesAccountHistory').value = '0'; fire($id('CmbWagesAccountHistory'));
        $id('FromDateHistory').focus();
    }

    /* ================================================================================ prints (plain form only) */
    function printWith(rows, args, btn) {
        if (!rows.length) return msg('No Record Found For Display');
        return window.CrystalPrint.open('ws-06', args || {}, btn);
    }
    function btnPrintForm_Click() { return printWith(dtFormHistory, formArgs, 'btnPrintForm'); }   /* :1296-1323 */
    function btnPrint_Click() { return printWith(dtHistory, histArgs, 'btnPrint'); }               /* :1274-1294 */

    /* ================================================================================ tabs / keys */
    function selectTab(i) {
        tab = i;
        $id('tabPage1').classList.toggle('is-hidden', i !== 0);
        $id('tabPage2').classList.toggle('is-hidden', i !== 1);
        $id('tabBtn1').classList.toggle('act', i === 0);
        $id('tabBtn2').classList.toggle('act', i === 1);
        (i === 1 ? $id('FromDateHistory') : $id('txtdate')).focus();     /* tabWagesSchedule_SelectedIndexChanged :755-765 */
    }
    function MakeShortCutKeys() {
        K.shortcuts([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'],
                     ['Ctrl+N', 'For New'], ['Alt+3', 'For Print Voucher 103'], ['Ctrl+F5', 'For Focus on DocDate'],
                     ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
                     ['Ctrl+D', 'For Adding an row in Focused Grid'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'],
                     ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }
    function closeForm() {
        if (window.opener && !window.opener.closed) { window.close(); return; }
        window.location.href = '/accounts/vouchers/contractor-wages-dashboard';
    }
    /** btnRefresh_Click :468-478 (accountName only) / CW:511-522 (ContractorFill + ComboBindFromWagesSchedule) */
    function btnRefresh_Click() {
        return busy('btnRefresh', function () { return CW ? ContractorFill().then(ComboBindFromWagesSchedule) : accountName(); })
            .catch(function (e) { return msg(e.message); });
    }
    /** btnWagesAccount_Click :1231-1241 - new frmContractorWagesAccount(UserAccount).Show() */
    function btnWagesAccount_Click() { window.open('/accounts/vouchers/wages-account', '_blank'); }

    /* CW txtPackUOMFrom_TextChanged :379-400 / combAccountName_Leave :418-444 - Company Rate auto-fill, only when > 0 */
    function lookupCompanyRate() {
        if (!(val('combAccountName') > 0 || numText('txtPackUOMFrom') > 0)) return Promise.resolve();
        return getJson(API + '/wages-rate?' + q({ effectedDate: $id('txtdate').value, packUomFrom: numText('txtPackUOMFrom'),
                                                  wagesAccountId: val('combAccountName'), contractorId: val('Cmbcontractor') }))
            .then(function (d) { var r = d ? toD(d.wagesRate) : 0; if (r > 0) $id('txtCompanyRate').value = K.fixed(r, fmtRate); })
            .catch(function (e) { return msg(e.message); });
    }

    function onlyDecimal(e) {                                            /* CommonServices.OnlytextdecimelFunction */
        if (e.ctrlKey || e.altKey || e.key.length > 1) return;
        if (!/[0-9.]/.test(e.key) || (e.key === '.' && e.target.value.indexOf('.') >= 0)) e.preventDefault();
    }

    function wire() {
        $id('btnnew').addEventListener('click', function () { FormRest(); });
        $id('btnRefresh').addEventListener('click', btnRefresh_Click);
        $id('btnsave').addEventListener('click', btnsave_Click);
        $id('btnUpdate').addEventListener('click', btnUpdate_Click);
        $id('btnshortcutkeys').addEventListener('click', MakeShortCutKeys);
        $id('btnWagesAccount').addEventListener('click', btnWagesAccount_Click);
        $id('BtnNewHistory').addEventListener('click', Resethistory);
        $id('BtnRefreshHistory').addEventListener('click', function () { busy('BtnRefreshHistory', ComboBindFromWagesSchedule).catch(function (e) { msg(e.message); }); });
        $id('btnshowHistory').addEventListener('click', function () { busy('btnshowHistory', BindGridHistory); });
        if ($id('btnPrintForm')) $id('btnPrintForm').addEventListener('click', btnPrintForm_Click);
        if ($id('btnPrint')) $id('btnPrint').addEventListener('click', btnPrint_Click);
        $id('tabBtn1').addEventListener('click', function () { selectTab(0); });
        $id('tabBtn2').addEventListener('click', function () { selectTab(1); });

        ['txtPackUOMFrom', 'txtPackUOMTO', 'txtwagesrate'].concat(CW ? ['txtCompanyRate'] : []).forEach(function (id) {
            $id(id).addEventListener('keydown', onlyDecimal);
        });
        /* txtwagesrate_Leave :495-508 - an empty box sends the focus back to the date */
        $id('txtwagesrate').addEventListener('blur', function () { if (this.value === '') setTimeout(function () { $id('txtdate').focus(); }, 0); });

        if (CW) {
            $id('txtPackUOMFrom').addEventListener('input', function () { lookupCompanyRate(); });
            $id('Cmbcontractor').addEventListener('blur', function () {
                if (val('combAccountName') > 0 || val('Cmbcontractor') > 0) BindgrdWagesSchedule();
            });
            $id('combAccountName').addEventListener('blur', function () {
                lookupCompanyRate().then(function () { if (val('combAccountName') > 0 || val('Cmbcontractor') > 0) return BindgrdWagesSchedule(); });
            });
        } else {
            /* combAccountName_Leave :283-289 */
            $id('combAccountName').addEventListener('blur', function () { if (val('combAccountName') > 0) BindgrdWagesSchedule(); });
        }

        $id('grdWagesSchedule').addEventListener('click', function (e) { gridClick(e, dtFormHistory, 'Form'); });
        $id('grdWagesSchedule').addEventListener('dblclick', function (e) { gridDblClick(e, dtFormHistory); });
        $id('gridhistory').addEventListener('click', function (e) { gridClick(e, dtHistory, 'History'); });
        $id('gridhistory').addEventListener('dblclick', function (e) { gridDblClick(e, dtHistory); });

        /* frmContractWagesSchedule_KeyDown :1071-1168 */
        K.enterToTab();
        document.addEventListener('keydown', function (e) {
            if (e.defaultPrevented || modalOpen()) return;
            var k = e.key;
            if (e.ctrlKey && (k === 't' || k === 'T')) { e.preventDefault(); selectTab(tab === 1 ? 0 : 1); return; }
            if ((e.ctrlKey && (k === 'e' || k === 'E')) || k === 'Escape') { e.preventDefault(); closeForm(); return; }
            if ((e.ctrlKey && k === 'Alt') || (e.altKey && k === 'Control')) { e.preventDefault(); MakeShortCutKeys(); return; }
            if (tab === 0) {
                if (e.ctrlKey && (k === 's' || k === 'S') && $id('btnsave').style.display !== 'none' && !$id('btnsave').disabled) { e.preventDefault(); btnsave_Click(); }
                if (e.ctrlKey && (k === 'u' || k === 'U') && $id('btnUpdate').style.display !== 'none' && !$id('btnUpdate').disabled) { e.preventDefault(); btnUpdate_Click(); }
                if (e.ctrlKey && (k === 'n' || k === 'N')) { e.preventDefault(); FormRest(); }
                if (e.ctrlKey && (k === 'r' || k === 'R')) { e.preventDefault(); btnRefresh_Click(); }
                if (e.ctrlKey && k === 'F5') { e.preventDefault(); $id('txtdate').focus(); }
                if (e.ctrlKey && k === 'ArrowDown') { e.preventDefault(); $id('wrapGrid1').focus(); }
                if (e.ctrlKey && k === 'ArrowUp') { e.preventDefault(); $id(CW ? 'Cmbcontractor' : 'combAccountName').focus(); }
            } else {
                if (e.ctrlKey && (k === 's' || k === 'S')) { e.preventDefault(); busy('btnshowHistory', BindGridHistory); }
                if (e.ctrlKey && (k === 'F5' || k === 'ArrowUp')) { e.preventDefault(); $id('FromDateHistory').focus(); }
                if (e.ctrlKey && k === 'ArrowDown') { e.preventDefault(); $id('wrapGrid2').focus(); }
            }
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        $id('txtdate').value = todayYmd();                 /* DateTimePicker designer default: now */
        $id('txtdateto').value = todayYmd();
        ['txtPackUOMFrom', 'txtPackUOMTO', 'txtwagesrate'].forEach(function (id) { $id(id).value = '0'; });
        wire();
        selectTab(0);
        Load();
    });
}());
