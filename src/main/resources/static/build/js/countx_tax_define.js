/* ============================================================================================
 * countx_tax_define.js - the four Taxation master-data forms (Architecture.WinApp.Taxation).
 * The page says which one it is with <body data-tax-page="...">:
 *
 *   tax-type      InvfrmAddTaxType.cs (178)                     "Add Tax Type"
 *   tax-schedule  InvfrmAddTaxSchedule.cs (177)                 "Add Tax Schedule"
 *   gl-maping     frmTaxNotesAndGLMaping.cs (172)               "Tax Notes And GL Maping"
 *   exemption     frmSupplierCustomerExemptionSchedule.cs (173) "Supplier Customer Exemption Schedule"
 *
 * Every form has the same shape: a toolstrip (New / [Refresh] / Update(hidden) / Save), a caption, one
 * row of entry fields, a "History" caption and a GridEX whose double-click loads the row for Update.
 * Save/Update confirm first on 178, 172 and 173 ("Are you sure to Save?" / "Are you sure to Update?");
 * 177 saves straight away. Ctrl+N New, Ctrl+S Save, Ctrl+U Update, Ctrl+E / Esc Close - as each
 * form's KeyDown.
 * ============================================================================================ */
(function () {
    'use strict';

    function $id(id) { return document.getElementById(id); }
    function box(m) { window.alert(m); }
    function ask(m) { return window.confirm(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return null;
    }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    function netI(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isFinite(v) ? Math.round(v) : 0;
        var s = String(v).trim();
        return /^[+-]?\d+$/.test(s) ? parseInt(s, 10) : 0;
    }
    function netD(v) { var n = parseFloat(v); return isFinite(n) ? n : 0; }
    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) {
            if (b.disabled || b.classList.contains('is-busy')) return;
            b.disabled = true; b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } };
        var p;
        try { p = fn(); } catch (e) { done(); throw e; }
        if (p && typeof p.then === 'function') p.then(done, done); else done();
        return p;
    }
    function parse(r) {
        return r.text().then(function (t) {
            var b = null;
            try { b = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
            if (!r.ok) throw new Error((b && b.message) || ('Request failed (' + r.status + ')'));
            return b;
        });
    }
    function getJson(url) { return fetch(url, { headers: { 'Accept': 'application/json' }, credentials: 'same-origin' }).then(parse); }
    function postJson(url, data) {
        var h = { 'Accept': 'application/json', 'Content-Type': 'application/json' };
        var t = document.querySelector('meta[name="_csrf"]'), n = document.querySelector('meta[name="_csrf_header"]');
        if (t && n && t.getAttribute('content') && n.getAttribute('content')) h[n.getAttribute('content')] = t.getAttribute('content');
        return fetch(url, { method: 'POST', headers: h, credentials: 'same-origin', body: JSON.stringify(data) }).then(parse);
    }
    function refreshCombos() { if (window.DesktopCombo) window.DesktopCombo.refresh(); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function hasSel(id) { var s = $id(id); return !!s && s.value !== '0' && s.value !== ''; }
    /** DDL.BindDDL(dt, cmb, valueMember, displayMember, ..., ZeroIndex:false) - options from rows; the empty slot stays first. */
    function bind(id, rows, valueCol, textCol) {
        var s = $id(id); if (!s) return;
        var html = '<option value="0"></option>';
        (rows || []).forEach(function (r) { html += '<option value="' + esc(col(r, valueCol)) + '">' + esc(col(r, textCol)) + '</option>'; });
        s.innerHTML = html;
        s.value = '0';
    }
    function setVal(id, v) {
        var s = $id(id); if (!s) return;
        s.value = str(v);
        if (s.value !== str(v)) s.value = '0';        // UltraCombo.Value with no matching row: nothing active
    }
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    /** yyyy-MM-dd from whatever the server sent (ISO date / date-time text). */
    function isoDate(v) {
        var s = str(v).trim();
        if (!s) return '';
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(s);
        if (m) return m[1] + '-' + m[2] + '-' + m[3];
        var d = new Date(s);
        return isNaN(d.getTime()) ? '' : d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate());
    }
    /** "dd-MMM-yy" (HistoryGridFill on 177). */
    function ddMMMyy(v) {
        var s = isoDate(v); if (!s) return '';
        return s.substring(8, 10) + '-' + MON[parseInt(s.substring(5, 7), 10) - 1] + '-' + s.substring(2, 4);
    }
    /** DateTime.ToShortDateString() under the desktop's en-US culture: M/d/yyyy. */
    function shortDate(v) {
        var s = isoDate(v); if (!s) return '';
        return parseInt(s.substring(5, 7), 10) + '/' + parseInt(s.substring(8, 10), 10) + '/' + s.substring(0, 4);
    }
    function today() { var d = new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function num(v) { var n = netD(v); return String(n); }

    // ------------------------------------------------------------------------------ shared page state

    var PAGE = document.body.getAttribute('data-tax-page');
    var RecId = 0;
    var GRID = [];
    var CUR = -1;
    var PERM = { Save: true, Update: true };

    function saveMode(save) {
        show('btnsave', save && PERM.Save);
        show('btnupdate', !save && PERM.Update);
    }
    function drawRows(cells) {
        var body = $id('gridBody');
        body.innerHTML = GRID.map(function (r, i) {
            return '<tr data-i="' + i + '"' + (i === CUR ? ' class="is-current"' : '') + '>' + cells(r).map(function (c) {
                return c.hidden ? '<td style="display:none;">' + esc(c.v) + '</td>'
                     : '<td' + (c.right ? ' class="num"' : '') + '>' + esc(c.v) + '</td>';
            }).join('') + '</tr>';
        }).join('');
    }
    function cancel() {
        window.close();
        setTimeout(function () { if (!window.closed) { if (history.length > 1) history.back(); else location.href = '/taxation'; } }, 150);
    }
    function wireGrid(onDouble) {
        var gb = $id('gridBody');
        gb.addEventListener('click', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            CUR = +tr.getAttribute('data-i');
            gb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
        });
        gb.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (tr) onDouble(+tr.getAttribute('data-i'));
        });
    }
    function wireKeys(p) {
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'n') { e.preventDefault(); p.btnnew(); }
            if (e.ctrlKey && k === 's' && !$id('btnsave').classList.contains('is-hidden')) { e.preventDefault(); p.btnsave(); }
            if (e.ctrlKey && k === 'u' && !$id('btnupdate').classList.contains('is-hidden')) { e.preventDefault(); p.btnupdate(); }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); cancel(); }
        });
    }
    function applyPermissions(d) {
        if (d && d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false };
    }

    // ============================================================================== 178 Add Tax Type

    var TaxType = (function () {
        var api = '/api/taxation/tax-type';

        /* DataGridAddTaxTypeFill :  dt(Id, TaxName, AccountTitle) - only when rows came back. */
        function fillGrid(rows) {
            if (!rows || !rows.length) return;
            GRID = rows.map(function (r) { return { Id: col(r, 'Id'), TaxName: str(col(r, 'TaxName')), AccountTitle: str(col(r, 'AccountTitle')) }; });
            CUR = -1;
            drawRows(function (r) { return [{ v: r.Id, hidden: true }, { v: r.TaxName }, { v: r.AccountTitle }]; });
        }
        function gridFill() { return getJson(api + '/list').then(fillGrid).catch(function (e) { box(e.message); }); }

        /* AcfrmAddTaxType_Load : CmbTaxTypeFill (Rows[0].Activate), CmbAccountFill, DataGridAddTaxTypeFill. */
        function load() {
            return getJson(api + '/setup').then(function (d) {
                d = d || {};
                applyPermissions(d);
                bind('cmbType', d.types || [], 'Id', 'TaxType');
                if ($id('cmbType').options.length > 1) $id('cmbType').selectedIndex = 1;
                if (d.accountsError) box(d.accountsError);
                else if ((d.accounts || []).length) bind('cmbaccount', d.accounts, 'Id', 'AccountTitle');
                if (d.taxTypesError) box(d.taxTypesError); else fillGrid(d.taxTypes || []);
                refreshCombos();
            }).catch(function (e) { box(e.message); }).then(function () {
                saveMode(true);
                $id('txtName').focus();
            });
        }
        /* FormValidation. */
        function formValidation() {
            if ($id('txtName').value === '') { box('Name Field Required'); $id('txtName').focus(); return false; }
            if (!hasSel('cmbType')) { box('Type Field Required'); $id('cmbType').focus(); return false; }
            if (!hasSel('cmbaccount')) { box('Account Field Required'); $id('cmbaccount').focus(); return false; }
            return true;
        }
        /* FormReset. */
        function formReset() {
            $id('txtName').value = '';
            setVal('cmbaccount', '0');
            saveMode(true);
            $id('txtName').focus();
            if ($id('cmbType').options.length > 1) $id('cmbType').selectedIndex = 1;
            refreshCombos();
            return gridFill();
        }
        /* Insert() - confirm, save, "Record Save Successfully.." / "Record Update Successfully.", refill, reset. */
        function insert(btn) {
            if (!formValidation()) return;
            if (RecId > 0) { if (!ask('Are you sure to Update?')) return; }
            else if (!ask('Are you sure to Save?')) return;
            return busy(btn, function () {
                return postJson(api + '/save', {
                    id: RecId, name: $id('txtName').value.trim(), type: netI($id('cmbType').value), accountId: netI($id('cmbaccount').value)
                }).then(function () {
                    box(RecId > 0 ? 'Record Update Successfully.' : 'Record Save Successfully..');
                    RecId = 0;
                    return gridFill().then(formReset);
                }).catch(function (e) { box(e.message); });
            });
        }
        function btnsave(btn) { RecId = 0; return insert(btn || 'btnsave'); }
        function btnupdate(btn) { return insert(btn || 'btnupdate'); }
        function btnnew() { RecId = 0; return formReset(); }
        /* btnRefresh_Click : CmbTaxTypeFill + CmbAccountFill. */
        function btnrefresh(btn) {
            return busy(btn || 'btnRefresh', function () {
                return getJson(api + '/setup').then(function (d) {
                    d = d || {};
                    bind('cmbType', d.types || [], 'Id', 'TaxType');
                    if ($id('cmbType').options.length > 1) $id('cmbType').selectedIndex = 1;
                    if (d.accountsError) box(d.accountsError);
                    else if ((d.accounts || []).length) bind('cmbaccount', d.accounts, 'Id', 'AccountTitle');
                    refreshCombos();
                }).catch(function (e) { box(e.message); });
            });
        }
        /* GridAddTaxType_DoubleClick : GetByID -> Name, Account, Type; Save hidden, Update shown. */
        function rowDoubleClick(i) {
            var r = GRID[i]; if (!r) return;
            RecId = netI(r.Id);
            return getJson(api + '/by-id?id=' + encodeURIComponent(RecId)).then(function (rows) {
                var row = (rows || [])[0];
                if (!row) throw new Error('Record not found.');
                $id('txtName').value = str(col(row, 'TaxName'));
                setVal('cmbaccount', col(row, 'TaxGLAccountId'));
                setVal('cmbType', col(row, 'Type'));
                refreshCombos();
                saveMode(false);
            }).catch(function (e) { box(e.message); });
        }
        return { load: load, btnsave: btnsave, btnupdate: btnupdate, btnnew: btnnew, btnrefresh: btnrefresh, rowDoubleClick: rowDoubleClick };
    }());

    // ============================================================================== 177 Add Tax Schedule

    var TaxSchedule = (function () {
        var api = '/api/taxation/tax-schedule';

        /* HistoryGridFill : dt(Id, TaxName, EffectedDate dd-MMM-yy, TaxPercent) - the grid is rebound even when empty. */
        function fillGrid(rows) {
            GRID = (rows || []).map(function (r) {
                return { Id: col(r, 'Id'), TaxName: str(col(r, 'TaxName')), EffectedDate: ddMMMyy(col(r, 'EffectedDate')), TaxPercent: netD(col(r, 'TaxPercent')) };
            });
            CUR = -1;
            drawRows(function (r) { return [{ v: r.Id, hidden: true }, { v: r.TaxName }, { v: r.EffectedDate }, { v: r.TaxPercent, right: true }]; });
        }
        function gridFill() { return getJson(api + '/list').then(fillGrid).catch(function (e) { box(e.message); }); }

        /* InvfrmAddTaxSchedule_Load : Is Active checked; CmbTaxTypeFill (Id, TaxDescription); HistoryGridFill. */
        function load() {
            return getJson(api + '/setup').then(function (d) {
                d = d || {};
                applyPermissions(d);
                if (d.taxTypesError) box(d.taxTypesError);
                else if ((d.taxTypes || []).length) bind('cmbTaxType', d.taxTypes, 'Id', 'TaxDescription');
                if (d.schedulesError) box(d.schedulesError); else fillGrid(d.schedules || []);
                refreshCombos();
            }).catch(function (e) { box(e.message); }).then(function () {
                $id('ChkBoxIsActive').checked = true;
                if (!$id('dtpEffectedDate').value) $id('dtpEffectedDate').value = today();
                saveMode(true);
            });
        }
        /* FormValidation - in the desktop's order. */
        function formValidation() {
            var t = $id('txtTaxPercent').value.trim();
            if (!hasSel('cmbTaxType')) { box('Tax Type Field Required'); $id('cmbTaxType').focus(); return false; }
            if (netD(t) > 100) { box('Tax Percent cannot be greater than 100'); $id('txtTaxPercent').focus(); return false; }
            if (t === '' || t === '0') { box('Tax Percent Field Required'); $id('txtTaxPercent').focus(); return false; }
            return true;
        }
        /* reset(). */
        function reset() {
            setVal('cmbTaxType', '0');
            $id('txtTaxPercent').value = '';
            saveMode(true);
            $id('ChkBoxIsActive').checked = true;
            refreshCombos();
            return gridFill();
        }
        function payload(id) {
            return { id: id, taxNameId: netI($id('cmbTaxType').value), effectedDate: $id('dtpEffectedDate').value,
                     taxPercent: $id('txtTaxPercent').value.trim(), isActive: $id('ChkBoxIsActive').checked };
        }
        /* BtnSave_Click : "Record Save Successfully...[n]". */
        function btnsave(btn) {
            if (!formValidation()) return;
            return busy(btn || 'btnsave', function () {
                return postJson(api + '/save', payload(0)).then(function (d) {
                    var n = netI(d && d.id);
                    if (n >= 1) box('Record Save Successfully...[' + n + ']');
                    return reset();
                }).catch(function (e) { box(e.message); });
            });
        }
        /* BtnEdit_Click : "Record Update Successfully...[n]". */
        function btnupdate(btn) {
            if (!formValidation()) return;
            return busy(btn || 'btnupdate', function () {
                return postJson(api + '/save', payload(RecId)).then(function (d) {
                    var n = netI(d && d.id);
                    if (n >= 1) box('Record Update Successfully...[' + n + ']');
                    RecId = 0;
                    return reset();
                }).catch(function (e) { box(e.message); });
            });
        }
        function btnnew() { RecId = 0; return reset(); }
        /* HistoryGrd_DoubleClick_1 : GetByID -> Tax Type, Effected Date, Tax Percent; Save hidden, Update shown. */
        function rowDoubleClick(i) {
            var r = GRID[i]; if (!r) return;
            RecId = netI(r.Id);
            return getJson(api + '/by-id?id=' + encodeURIComponent(RecId)).then(function (rows) {
                var row = (rows || [])[0];
                if (!row) throw new Error('Record not found.');
                setVal('cmbTaxType', col(row, 'TaxNameId'));
                $id('dtpEffectedDate').value = isoDate(col(row, 'EffectedDate'));
                $id('txtTaxPercent').value = num(col(row, 'TaxPercent'));
                $id('ChkBoxIsActive').checked = col(row, 'PostState') === true || str(col(row, 'PostState')) === '1' || str(col(row, 'PostState')).toLowerCase() === 'true';
                refreshCombos();
                saveMode(false);
            }).catch(function (e) { box(e.message); });
        }
        /* btnDefineBusinessType_Click : frmTaxTypeAndBusinessTypeMapping - not in this port. */
        function btnBusinessType() { box('frmTaxTypeAndBusinessTypeMapping (Tax Mapping With Business type) is not available in the web application yet.'); }
        return { load: load, btnsave: btnsave, btnupdate: btnupdate, btnnew: btnnew, rowDoubleClick: rowDoubleClick, btnBusinessType: btnBusinessType };
    }());

    // ============================================================================== 172 Tax Notes and GL Maping

    var GlMaping = (function () {
        var api = '/api/taxation/gl-maping';

        /* GridBind : dt(Id, TaxLookUpsId, LookUpsTaxInOutId, GLAccountId, SortNo, EntryDate, EntryUserId, ModifyUserId,
           ModifyDate, saletaxnotes, inputoutput, AccountTitle) - only when rows came back; Id, the three
           foreign keys and EntryUserId hidden (GridSetting). */
        function fillGrid(rows) {
            if (!rows || !rows.length) return;
            GRID = rows.map(function (r) {
                return { Id: col(r, 'Id'), TaxLookUpsId: netI(col(r, 'TaxLookUpsId')), LookUpsTaxInOutId: netI(col(r, 'LookUpsTaxInOutId')),
                         GLAccountId: netI(col(r, 'GLAccountId')), SortNo: netI(col(r, 'SortNo')), EntryDate: shortDate(col(r, 'EntryDate')),
                         EntryUserId: netI(col(r, 'EntryUserId')), ModifyUserId: netI(col(r, 'ModifyUserId')), ModifyDate: shortDate(col(r, 'ModifyDate')),
                         saletaxnotes: str(col(r, 'saletaxnotes')), inputoutput: netI(col(r, 'inputoutput')), AccountTitle: str(col(r, 'AccountTitle')) };
            });
            CUR = -1;
            drawRows(function (r) {
                return [{ v: r.Id, hidden: true }, { v: r.TaxLookUpsId, hidden: true }, { v: r.LookUpsTaxInOutId, hidden: true }, { v: r.GLAccountId, hidden: true },
                        { v: r.SortNo, right: true }, { v: r.EntryDate }, { v: r.EntryUserId, hidden: true }, { v: r.ModifyUserId, right: true }, { v: r.ModifyDate },
                        { v: r.saletaxnotes }, { v: r.inputoutput, right: true }, { v: r.AccountTitle }];
            });
        }
        function gridFill() { return getJson(api + '/list').then(fillGrid).catch(function (e) { box(e.message); }); }

        function fillCombos(d) {
            if (d.saleTaxNotesError) box(d.saleTaxNotesError);
            else if ((d.saleTaxNotes || []).length) bind('TaxLookUpsId', d.saleTaxNotes, 'Id', 'LookUpName');
            if (d.inputOutputError) box(d.inputOutputError);
            else if ((d.inputOutput || []).length) bind('LookUpsTaxInOutId', d.inputOutput, 'Id', 'LookUpName');
        }
        /* frmTaxNotesAndGLMaping_Load : GridBind, SaleTaxFill, InputOutputFill, GLAccountFill. */
        function load() {
            return getJson(api + '/setup').then(function (d) {
                d = d || {};
                applyPermissions(d);
                if (d.mapingsError) box(d.mapingsError); else fillGrid(d.mapings || []);
                fillCombos(d);
                if (d.glAccountsError) box(d.glAccountsError);
                else if ((d.glAccounts || []).length) bind('GLAccountId', d.glAccounts, 'Id', 'AccountTitle');
                refreshCombos();
            }).catch(function (e) { box(e.message); }).then(function () { saveMode(true); });
        }
        /* FormValidation. */
        function formValidation() {
            if (!hasSel('TaxLookUpsId')) { box('Sale Tax Notes Required'); $id('TaxLookUpsId').focus(); return false; }
            if (!hasSel('LookUpsTaxInOutId')) { box('Input / Output Required'); $id('LookUpsTaxInOutId').focus(); return false; }
            if (!hasSel('GLAccountId')) { box('GL Account Required'); $id('GLAccountId').focus(); return false; }
            return true;
        }
        /* Reset(). */
        function reset() {
            setVal('TaxLookUpsId', '0'); setVal('LookUpsTaxInOutId', '0'); setVal('GLAccountId', '0');
            refreshCombos();
            $id('TaxLookUpsId').focus();
            saveMode(true);
            return gridFill();
        }
        /* Insert(). */
        function insert(btn) {
            if (!formValidation()) return;
            if (RecId > 0) { if (!ask('Are you sure to Update?')) return; }
            else if (!ask('Are you sure to Save?')) return;
            return busy(btn, function () {
                return postJson(api + '/save', {
                    id: RecId, taxLookUpsId: netI($id('TaxLookUpsId').value), lookUpsTaxInOutId: netI($id('LookUpsTaxInOutId').value), glAccountId: netI($id('GLAccountId').value)
                }).then(function () {
                    box(RecId > 0 ? 'Record Update Successfully.' : 'Record Save Successfully..');
                    RecId = 0;
                    return reset();
                }).catch(function (e) { box(e.message); });
            });
        }
        function btnsave(btn) { RecId = 0; return insert(btn || 'btnsave'); }
        function btnupdate(btn) { return insert(btn || 'btnupdate'); }
        function btnnew() { RecId = 0; return reset(); }
        /* btnRefresh_Click : GridBind, SaleTaxFill, InputOutputFill. */
        function btnrefresh(btn) {
            return busy(btn || 'btnRefresh', function () {
                return getJson(api + '/setup').then(function (d) {
                    d = d || {};
                    if (d.mapingsError) box(d.mapingsError); else fillGrid(d.mapings || []);
                    fillCombos(d);
                    refreshCombos();
                }).catch(function (e) { box(e.message); });
            });
        }
        /* RetrivedData(Id). */
        function rowDoubleClick(i) {
            var r = GRID[i]; if (!r) return;
            RecId = netI(r.Id);
            return getJson(api + '/by-id?id=' + encodeURIComponent(RecId)).then(function (rows) {
                var row = (rows || [])[0];
                if (!row) throw new Error('Record not found.');
                setVal('TaxLookUpsId', col(row, 'TaxLookUpsId'));
                setVal('GLAccountId', col(row, 'GLAccountId'));
                setVal('LookUpsTaxInOutId', col(row, 'LookUpsTaxInOutId'));
                refreshCombos();
                saveMode(false);
            }).catch(function (e) { box(e.message); });
        }
        /* btnTaxNotes_Click : TaxLookup form (screen 428 "Tax Lookup") - not in this port. */
        function btnTaxNotes() { box('TaxLookup (Tax Lookup, screen 428) is not available in the web application yet.'); }
        return { load: load, btnsave: btnsave, btnupdate: btnupdate, btnnew: btnnew, btnrefresh: btnrefresh, rowDoubleClick: rowDoubleClick, btnTaxNotes: btnTaxNotes };
    }());

    // ============================================================================== 173 Supplier Customer Exemption

    var Exemption = (function () {
        var api = '/api/taxation/exemption';

        /* GridBind : dt(Id, FromDate, ToDate, SupplierCustomerId, CompanyName, TaxTypeId, TaxName, TaxPrcnt) - only when
           rows came back; Id, SupplierCustomerId, TaxTypeId hidden (grdSetting). */
        function fillGrid(rows) {
            if (!rows || !rows.length) return;
            GRID = rows.map(function (r) {
                return { Id: col(r, 'Id'), FromDate: shortDate(col(r, 'FromDate')), ToDate: shortDate(col(r, 'ToDate')), SupplierCustomerId: netI(col(r, 'SupplierCustomerId')),
                         CompanyName: str(col(r, 'CompanyName')), TaxTypeId: netI(col(r, 'TaxTypeId')), TaxName: str(col(r, 'TaxName')), TaxPrcnt: netD(col(r, 'TaxPrcnt')) };
            });
            CUR = -1;
            drawRows(function (r) {
                return [{ v: r.Id, hidden: true }, { v: r.FromDate }, { v: r.ToDate }, { v: r.SupplierCustomerId, hidden: true }, { v: r.CompanyName },
                        { v: r.TaxTypeId, hidden: true }, { v: r.TaxName }, { v: r.TaxPrcnt, right: true }];
            });
        }
        function gridFill() { return getJson(api + '/list').then(fillGrid).catch(function (e) { box(e.message); }); }

        function fillCombos(d) {
            if (d.supplierCustomersError) box(d.supplierCustomersError);
            else if ((d.supplierCustomers || []).length) bind('SupplierCustomerId', d.supplierCustomers, 'Id', 'CustomerName');
            if (d.taxTypesError) box(d.taxTypesError);
            else if ((d.taxTypes || []).length) bind('TaxTypeId', d.taxTypes, 'Id', 'TaxName');
        }
        /* frmSupplierCustomerExemptionSchedule_Load : SupplierCustomerFill, TaxTypeFill, GridBind. */
        function load() {
            $id('FromDate').value = today(); $id('ToDate').value = today();
            return getJson(api + '/setup').then(function (d) {
                d = d || {};
                applyPermissions(d);
                fillCombos(d);
                if (d.schedulesError) box(d.schedulesError); else fillGrid(d.schedules || []);
                refreshCombos();
            }).catch(function (e) { box(e.message); }).then(function () { saveMode(true); });
        }
        /* FormValidation. The desktop's third check converts the TextBox object (always 0) - the typed value is checked here. */
        function formValidation() {
            if (!hasSel('SupplierCustomerId')) { box('Supplier Customer Required'); $id('SupplierCustomerId').focus(); return false; }
            if (!hasSel('TaxTypeId')) { box('Tax Type Required'); $id('TaxTypeId').focus(); return false; }
            if (netD($id('TaxPrcnt').value) > 100) { box('Tax Percent must be less than 100'); $id('TaxPrcnt').focus(); return false; }
            return true;
        }
        /* Reset(). */
        function reset() {
            $id('FromDate').value = today(); $id('ToDate').value = today();
            setVal('SupplierCustomerId', '0'); setVal('TaxTypeId', '0');
            $id('TaxPrcnt').value = '';
            refreshCombos();
            $id('FromDate').focus();
            saveMode(true);
            return gridFill();
        }
        /* Insert(). */
        function insert(btn) {
            if (!formValidation()) return;
            if (RecId > 0) { if (!ask('Are you sure to Update?')) return; }
            else if (!ask('Are you sure to Save?')) return;
            return busy(btn, function () {
                return postJson(api + '/save', {
                    id: RecId, fromDate: $id('FromDate').value, toDate: $id('ToDate').value,
                    supplierCustomerId: netI($id('SupplierCustomerId').value), taxTypeId: netI($id('TaxTypeId').value), taxPrcnt: $id('TaxPrcnt').value.trim()
                }).then(function () {
                    box(RecId > 0 ? 'Record Update Successfully.' : 'Record Save Successfully..');
                    RecId = 0;
                    return reset();
                }).catch(function (e) { box(e.message); });
            });
        }
        function btnsave(btn) { RecId = 0; return insert(btn || 'btnsave'); }
        function btnupdate(btn) { return insert(btn || 'btnupdate'); }
        function btnnew() { RecId = 0; return reset(); }
        /* btnRefresh_Click_1 : SupplierCustomerFill, TaxTypeFill. */
        function btnrefresh(btn) {
            return busy(btn || 'btnRefresh', function () {
                return getJson(api + '/setup').then(function (d) { fillCombos(d || {}); refreshCombos(); }).catch(function (e) { box(e.message); });
            });
        }
        /* RetrivedData(Id). */
        function rowDoubleClick(i) {
            var r = GRID[i]; if (!r) return;
            RecId = netI(r.Id);
            return getJson(api + '/by-id?id=' + encodeURIComponent(RecId)).then(function (rows) {
                var row = (rows || [])[0];
                if (!row) throw new Error('Record not found.');
                $id('FromDate').value = isoDate(col(row, 'FromDate'));
                $id('ToDate').value = isoDate(col(row, 'ToDate'));
                setVal('SupplierCustomerId', col(row, 'SupplierCustomerId'));
                setVal('TaxTypeId', col(row, 'TaxTypeId'));
                $id('TaxPrcnt').value = num(col(row, 'TaxPrcnt'));
                refreshCombos();
                saveMode(false);
            }).catch(function (e) { box(e.message); });
        }
        return { load: load, btnsave: btnsave, btnupdate: btnupdate, btnnew: btnnew, btnrefresh: btnrefresh, rowDoubleClick: rowDoubleClick };
    }());

    // ------------------------------------------------------------------------------ boot

    var PAGES = { 'tax-type': TaxType, 'tax-schedule': TaxSchedule, 'gl-maping': GlMaping, 'exemption': Exemption };
    var page = PAGES[PAGE];
    if (!page) return;
    page.cancel = cancel;
    window.TaxPage = page;

    function boot() {
        wireGrid(page.rowDoubleClick);
        wireKeys(page);
        page.load();
    }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
    else boot();
}());
