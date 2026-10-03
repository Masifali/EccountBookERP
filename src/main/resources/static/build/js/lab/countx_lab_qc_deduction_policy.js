/* ============================================================================================
 * Screen 164 "Lab Deduction Policy For Purchase" — Architecture.WinApp.QCL/frmQcDeductionPolicy.cs
 * (ScreenName frmQcDeductionPolicy). Method names in the comments are the form's own handlers.
 *
 * frmQcDeductionPolicy_Load, AnalysisGroup, AnalysisParamerterByGroup, UOM, CmbAnalysisGroup_Leave,
 * Add_Click, IsBetween, grdSettings, DeleteDetailRow, btnUpdateDetail_Click, btnCancelDetail_Click,
 * grd_DoubleClick, grd_ColumnButtonClick, FormValidation, FormValidationDetail, btnUpdate_Click,
 * Insert, btnSave_Click, ReadById, btnDelete_Click, btnNew_Click, btnRefresh_Click, Reset,
 * ResetDetail, the four KeyPress filters, tabControl1_SelectedIndexChanged, btnResetHistory_Click,
 * btnShow_Click, gridhistoryfill, HistoryGridSettings, grdhistory_ColumnButtonClick / LinkClicked /
 * DoubleClick / SelectionChanged, GridDetailBind, btnattachment_Click, frmQcDeductionPolicy_KeyDown,
 * MakeShortCutKeys, grd_KeyDown, grdhistory_KeyDown.
 * The server re-validates everything it saves.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/lab/qc-deduction-policy';
    var MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var MAX_BYTES = 5 * 1024 * 1024;                                        // DesktopAttachmentStore.MAX_BYTES

    /* AnalysisGroup(): DDL.BindDDL over dt(Id, AnalysisGroupDescription, GroupTypeId, GroupType) — column 0
       (Id) hidden, column 1 captioned "Analysis Group", Columns[2] (GroupTypeId) hidden, GroupType visible. */
    if (window.DesktopCombo) {
        window.DesktopCombo.define('labQdpGroup', [
            { caption: 'Analysis Group', flex: 3 },
            { caption: 'GroupType', flex: 2, key: 'grouptype' }
        ]);
    }

    var L = null;                       // lookups
    var st = {
        recId: 0,                       // RecId
        rows: [],                       // table (grd.DataSource)
        sel: -1,                        // grd.CurrentRow
        updateDetailIndex: -1,
        removed: [],                    // lstRemoveRecord (detail ids)
        hist: [], histSel: -1, histFilter: {},
        histDetail: [], histDetailFilter: {},
        att: [],                        // AT.lst rows that came from the record
        files: [],                      // AT.lst rows added with Browse: {name, base64, size}
        rateDecimals: 2
    };

    /* grd after grdSettings: "Delete" (caption X) at Position 0, "Edit" at Position 1, Id and
       AnalysisParameterId hidden. double columns: "Rate" key -> DecimalRateFormate, the rest "#,##0.##" + Sum. */
    var DETAIL_COLS = [
        { key: 'AnalysisParameter', caption: 'AnalysisParameter', width: 220 },
        { key: 'RangeFrom', caption: 'RangeFrom', type: 'num', sum: true },
        { key: 'RangeTo', caption: 'RangeTo', type: 'num', sum: true },
        { key: 'DeductionWeight', caption: 'DeductionWeight', type: 'num', sum: true },
        { key: 'Uom', caption: 'Uom', type: 'int' },
        { key: 'DeductionQty', caption: 'DeductionQty', type: 'num', sum: true },
        { key: 'DeductionRate', caption: 'DeductionRate', type: 'rate' }
    ];
    /* grdhistory after HistoryGridSettings: "Edit" at Position 0 (frozen), Id and
       InvLabGroupAnalysisStandardsId hidden, NoOfAttachments ColumnType Link. */
    var HIST_COLS = [
        { key: 'AnalysisGroup', caption: 'AnalysisGroup', width: 150 },
        { key: 'PolicyName', caption: 'PolicyName', width: 200, type: 'code' },
        { key: 'EffectiveFrom', caption: 'EffectiveFrom', type: 'date' },
        { key: 'EffectiveTo', caption: 'EffectiveTo', type: 'date' },
        { key: 'RemarksHeader', caption: 'RemarksHeader', width: 200 },
        { key: 'EntryDate', caption: 'EntryDate', type: 'datetime' },
        { key: 'EntryUser', caption: 'EntryUser', width: 120 },
        { key: 'ModifyDate', caption: 'ModifyDate', type: 'date' },
        { key: 'ModifyUser', caption: 'ModifyUser', width: 120 },
        { key: 'IsApproved', caption: 'IsApproved' },
        { key: 'ApprovedDate', caption: 'ApprovedDate', type: 'date' },
        { key: 'ApprovedUser', caption: 'ApprovedUser', width: 120 },
        { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'link', sum: true }
    ];
    /* MakeShortCutKeys — the desktop's own table. */
    var SHORTCUTS = [
        ['Ctrl+S', 'For Save in Form Tab and For Show History in History Tab'],
        ['Ctrl+U', 'For Update'],
        ['Ctrl+E', 'For Close'],
        ['Ctrl+R', 'For Refresh'],
        ['Ctrl+N', 'For New'],
        ['Ctrl+F5', 'For Focus on Policy Name'],
        ['Ctrl+F10', 'For Open Attachments'],
        ['Ctrl+T', 'For Tab Transfer'],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid when focus in form tab and for focus on history grid when in history tab'],
        ['Ctrl+ArrowUp', 'For Focus on Parameter in Detail Grid when in Form tab and For focus on FromDate in history tab'],
        ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
        ['Ctrl+Space', 'When Focus On Any Grid To Call Function\'s On Button Or Link']
    ];

    // ============================================================================ helpers

    function $(id) { return document.getElementById(id); }
    function esc(v) {
        return String(v === undefined || v === null ? '' : v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function intOf(v) { var n = parseInt(String(v === undefined || v === null ? '' : v), 10); return isNaN(n) ? 0 : n; }
    /** Conversion.ToDouble(text): a float, thousands separators allowed; anything else 0. */
    function netToDouble(v) {
        var s = String(v === undefined || v === null ? '' : v).trim();
        if (!/^[+-]?(\d[\d,]*\.?\d*|\.\d+)([eE][+-]?\d+)?$/.test(s)) return 0;
        var n = Number(s.replace(/,/g, ''));
        return isFinite(n) ? n : 0;
    }
    /** "#,##0.##" */
    function fmtNum(v) { return (Number(v) || 0).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 2 }); }
    /** clsGlobalVariables.DecimalRateFormate — the configured number of rate decimals. */
    function fmtRate(v) { return (Number(v) || 0).toLocaleString('en-US', { minimumFractionDigits: st.rateDecimals, maximumFractionDigits: st.rateDecimals }); }
    function pad(n) { return String(n).padStart(2, '0'); }
    function isoOf(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function today() { return isoOf(new Date()); }
    function addDays(n) { var d = new Date(); d.setDate(d.getDate() + n); return isoOf(d); }
    function isoDay(v) { var m = /^(\d{4}-\d{2}-\d{2})/.exec(String(v || '')); return m ? m[1] : ''; }
    function dMMMyyyy(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || ''));
        return m ? m[3] + '-' + MONTHS[+m[2] - 1] + '-' + m[1] : '';
    }
    /** "dd-MMM-yy hh:mm tt" (HistoryGridSettings) */
    function dMMMyyTime(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})[T ](\d{2}):(\d{2})/.exec(String(v || ''));
        if (!m) { var d = dMMMyyyy(v); return d; }
        var h = +m[4], ap = h >= 12 ? 'PM' : 'AM';
        h = h % 12; if (h === 0) h = 12;
        return m[3] + '-' + MONTHS[+m[2] - 1] + '-' + m[1].substring(2) + ' ' + pad(h) + ':' + m[5] + ' ' + ap;
    }
    function sizeMb(v) { return (Number(v) || 0).toFixed(3); }
    function msg(e) { return e && e.message ? e.message : String(e); }
    function rights() { return (L && L.rights) || {}; }
    function show(el, on) { if (typeof el === 'string') el = $(el); if (el) el.hidden = !on; }
    function visible(id) { var el = $(id); return !!el && !el.hidden; }

    async function api(method, url, body) {
        var opt = { method: method, credentials: 'same-origin', headers: { Accept: 'application/json' } };
        if (body !== undefined) { opt.headers['Content-Type'] = 'application/json'; opt.body = JSON.stringify(body); }
        var res = await fetch(url, opt);
        var text = await res.text();
        var data = null;
        try { data = text ? JSON.parse(text) : null; } catch (e) { data = null; }
        if (!res.ok) {
            var m = data && (data.message || data.error) || ('The request failed (' + res.status + ').');
            throw new Error(m);
        }
        return data;
    }

    /**
     * Button lock: the button is disabled at once, shows a spinner (class btn-busy), further clicks are
     * ignored while the request is in flight, and it is released on success AND on failure (finally).
     * Any refusal is shown as the desktop's MessageBox (catch -> MessageBox.Show(ex.Message)).
     */
    var busy = new WeakMap();
    async function withBusy(btn, asyncFn) {
        if (btn) {
            if (busy.get(btn)) return;
            busy.set(btn, { disabled: btn.disabled });
            btn.disabled = true;
            btn.classList.add('btn-busy');
            btn.setAttribute('aria-busy', 'true');
        }
        var track = window.PurchaseRequest && window.PurchaseRequest.run
            ? function (w) { return window.PurchaseRequest.run(null, w); }      // the house "Loading…" panel
            : function (w) { return w(); };
        try {
            await track(asyncFn);
        } catch (e) {
            alert(msg(e));
        } finally {
            if (btn) {
                var was = busy.get(btn);
                busy.delete(btn);
                btn.classList.remove('btn-busy');
                btn.removeAttribute('aria-busy');
                btn.disabled = was ? was.disabled : false;
            }
        }
    }
    /** A button whose Enabled state changes while it is busy (rights): remember the new state. */
    function setEnabled(id, on) {
        var b = $(id), lock = busy.get(b);
        if (lock) lock.disabled = !on; else b.disabled = !on;
    }

    // ============================================================================ combos

    function opt(value, text, attrs) {
        var a = '';
        if (attrs) Object.keys(attrs).forEach(function (k) { a += ' data-' + k + '="' + esc(attrs[k]) + '"'; });
        return '<option value="' + esc(value) + '"' + a + '>' + esc(text) + '</option>';
    }
    /** BindDDL with ZeroIndex false: no default row, nothing selected. */
    function fill(id, rows, valueKey, textKey, attrFn) {
        var el = $(id), html = '';
        (rows || []).forEach(function (r) { html += opt(r[valueKey], r[textKey], attrFn ? attrFn(r) : null); });
        el.innerHTML = html;
        el.selectedIndex = -1;
    }
    function has(id, v) { return Array.prototype.some.call($(id).options, function (o) { return !o.hasAttribute('data-adhoc') && o.value === String(v); }); }
    function dropAdhoc(id) { Array.prototype.slice.call($(id).querySelectorAll('option[data-adhoc]')).forEach(function (o) { o.remove(); }); }
    /** combo.Value = v — a value that is not in the list leaves the combo empty. */
    function setVal(id, v) {
        var el = $(id);
        dropAdhoc(id);
        if (v !== null && v !== undefined && has(id, v)) el.value = String(v); else el.selectedIndex = -1;
    }
    /** Conversion.ToInt(combo.Value) with ActiveRow != null */
    function comboVal(id) {
        var el = $(id);
        if (el.selectedIndex < 0) return 0;
        var o = el.options[el.selectedIndex];
        return o.hasAttribute('data-adhoc') ? 0 : intOf(o.value);
    }
    /** combo.Text */
    function comboText(id) { var el = $(id); return el.selectedIndex < 0 ? '' : el.options[el.selectedIndex].textContent; }
    function clearCombo(id) { dropAdhoc(id); $(id).selectedIndex = -1; }                 // combo.Text = string.Empty
    /** combo.Text = text — selects the row with that text; a text that matches no row stays in the box. */
    function setText(id, text) {
        var el = $(id), t = String(text === undefined || text === null ? '' : text);
        dropAdhoc(id);
        if (t.trim() === '') { el.selectedIndex = -1; return; }
        for (var i = 0; i < el.options.length; i++) {
            if (el.options[i].textContent.trim().toLowerCase() === t.trim().toLowerCase()) { el.selectedIndex = i; return; }
        }
        el.insertAdjacentHTML('beforeend', '<option value="' + esc(t) + '" data-adhoc="1">' + esc(t) + '</option>');
        el.value = t;
    }
    function focusCombo(id) {
        var el = $(id), wrap = el && el.parentNode;
        var input = wrap && wrap.classList && wrap.classList.contains('dtcombo-wrap') ? wrap.querySelector('.dtcombo-input') : null;
        try { (input || el).focus(); } catch (e) { /* not focusable yet */ }
    }
    function focusEl(id) { try { $(id).focus(); } catch (e) { /* hidden */ } }

    /** AnalysisGroup() — the previous value is kept when it is still in the list, otherwise the text is cleared. */
    function bindAnalysisGroup(rows) {
        var prev = comboVal('CmbAnalysisGroup');
        if (rows && rows.length > 0) {
            fill('CmbAnalysisGroup', rows, 'Id', 'AnalysisGroupDescription', function (r) { return { grouptype: r.GroupType === null || r.GroupType === undefined ? '' : r.GroupType }; });
            if (prev > 0) setVal('CmbAnalysisGroup', prev);
        } else {
            $('CmbAnalysisGroup').innerHTML = ''; $('CmbAnalysisGroup').selectedIndex = -1;   // Text = "", DataSource = null
        }
    }
    /** UOM() — value "Id", display "type", caption "Weight Kg". */
    function bindUom(rows) {
        var prev = comboVal('CmbDeductionWeightUom');
        if (rows && rows.length > 0) {
            fill('CmbDeductionWeightUom', rows, 'Id', 'type');
            if (prev > 0) setVal('CmbDeductionWeightUom', prev);
        } else {
            $('CmbDeductionWeightUom').innerHTML = ''; $('CmbDeductionWeightUom').selectedIndex = -1;
        }
    }
    /** AnalysisParamerterByGroup(GroupId) */
    function bindParameters(rows) {
        var prev = comboVal('CmbAnalysisParameter');
        if (rows && rows.length > 0) {
            fill('CmbAnalysisParameter', rows, 'AnalysisParameterId', 'AnalysisParameter');
            if (prev > 0) setVal('CmbAnalysisParameter', prev);
        } else {
            $('CmbAnalysisParameter').innerHTML = ''; $('CmbAnalysisParameter').selectedIndex = -1;
        }
    }
    /** CmbAnalysisGroup_Leave — the parameters of the chosen group (nothing happens without a group). */
    var groupSeq = 0;
    async function analysisGroupLeave() {
        var groupId = comboVal('CmbAnalysisGroup');
        if (groupId <= 0) return;
        var mine = ++groupSeq;
        var rows = await api('GET', API + '/parameters?groupId=' + groupId);
        if (mine === groupSeq) bindParameters(rows);
    }

    // ============================================================================ grd (detail grid)

    function head(cols, lead) {
        return '<tr>' + (lead || '') + cols.map(function (c) {
            return '<th' + (c.width ? ' style="min-width:' + c.width + 'px;"' : '') + '>' + esc(c.caption) + '</th>';
        }).join('') + '</tr>';
    }
    function shown(c, v) {
        if (c.type === 'date') return dMMMyyyy(v);
        if (c.type === 'datetime') return dMMMyyTime(v);
        if (c.type === 'num') return fmtNum(v);
        if (c.type === 'rate') return fmtRate(v);
        return String(v === null || v === undefined ? '' : v);
    }
    function cell(c, r) {
        var v = r[c.key], t = esc(shown(c, v));
        if (c.type === 'num' || c.type === 'rate' || c.type === 'int') return '<td class="n">' + t + '</td>';
        if (c.type === 'link') return '<td class="n"><a class="lnk" data-att="1" title="Attachments">' + t + '</a></td>';
        if (c.type === 'code') return '<td><a class="lnk" data-open="1" title="Open this record">' + t + '</a></td>';
        return '<td>' + t + '</td>';
    }
    function totals(cols, rows, lead) {
        return '<tr>' + (lead || '') + cols.map(function (c) {
            if (!c.sum) return '<td></td>';
            var s = 0;
            rows.forEach(function (r) { s += Number(r[c.key]) || 0; });
            return '<td class="n">' + esc(fmtNum(s)) + '</td>';
        }).join('') + '</tr>';
    }

    function renderGrd() {
        var t = $('grd');
        t.tHead.innerHTML = head(DETAIL_COLS, '<th style="width:30px;">X</th><th style="width:60px;">Edit</th>');
        t.tBodies[0].innerHTML = st.rows.map(function (r, i) {
            return '<tr data-i="' + i + '"' + (i === st.sel ? ' class="is-sel"' : '') + '>'
                + '<td class="c"><button type="button" class="gbtn" data-del="1" title="Delete this row">X</button></td>'
                + '<td class="c"><button type="button" class="gbtn" data-edit="1">Edit</button></td>'
                + DETAIL_COLS.map(function (c) { return cell(c, r); }).join('') + '</tr>';
        }).join('');
        t.tFoot.innerHTML = totals(DETAIL_COLS, st.rows, '<td></td><td></td>');
        $('grdNav').textContent = 'Record ' + (st.rows.length && st.sel >= 0 ? st.sel + 1 : 0) + ' of ' + st.rows.length;
    }
    function selectRow(i) {
        st.sel = i;
        Array.prototype.forEach.call($('grd').tBodies[0].rows, function (r) { r.classList.toggle('is-sel', intOf(r.getAttribute('data-i')) === i); });
        $('grdNav').textContent = 'Record ' + (st.rows.length && st.sel >= 0 ? st.sel + 1 : 0) + ' of ' + st.rows.length;
    }

    /** IsBetween (:459) — the desktop's own expression: item >= start || item <= end. */
    function isBetween(item, start, end) { return item >= start || item <= end; }

    /** FormValidationDetail — desktop order and texts. */
    function formValidationDetail() {
        if (comboVal('CmbAnalysisParameter') === 0) { alert('Parameter Field is Required'); focusCombo('CmbAnalysisParameter'); return false; }
        if (netToDouble($('txtRangeFrom').value) <= 0) { alert('Range From Field is Required'); focusEl('txtRangeFrom'); return false; }
        if (netToDouble($('txtrangeTo').value) <= 0) { alert('Range To Field is Required'); focusEl('txtrangeTo'); return false; }
        if (netToDouble($('txtRangeFrom').value) >= netToDouble($('txtrangeTo').value)) { alert('Range From Must Be less Then Range To'); focusEl('txtRangeFrom'); return false; }
        if (netToDouble($('txtDeductionWt').value) <= 0) { alert('Deduction Weight Field is Required'); focusEl('txtDeductionWt'); return false; }
        if (comboVal('CmbDeductionWeightUom') === 0) { alert('Deduction UOM Field is Required'); focusCombo('CmbDeductionWeightUom'); return false; }
        if (netToDouble($('txtDeductionQty').value) <= 0) { alert('Deduction Qty Field is Required'); focusEl('txtDeductionQty'); return false; }
        if (netToDouble($('TxtDecRate').value) <= 0) { alert('Deduction Value Field is Required'); focusEl('TxtDecRate'); return false; }
        return true;
    }
    /** The range check of Add_Click / btnUpdateDetail_Click over the other rows of the same parameter. */
    function rangeClash(skipIndex) {
        var par = comboVal('CmbAnalysisParameter');
        var from = netToDouble($('txtRangeFrom').value), to = netToDouble($('txtrangeTo').value);
        for (var i = 0; i < st.rows.length; i++) {
            var r = st.rows[i];
            if (intOf(r.AnalysisParameterId) !== par || i === skipIndex) continue;
            if (isBetween(from, Number(r.RangeFrom) || 0, Number(r.RangeTo) || 0)) return 'RangeFrom Already Exist in another row';
            if (isBetween(to, Number(r.RangeFrom) || 0, Number(r.RangeTo) || 0)) return 'RangeTo Already Exist in another row';
        }
        return null;
    }
    function entryValues() {
        return {
            AnalysisParameterId: comboVal('CmbAnalysisParameter'),
            AnalysisParameter: comboText('CmbAnalysisParameter'),
            RangeFrom: netToDouble($('txtRangeFrom').value),
            RangeTo: netToDouble($('txtrangeTo').value),
            DeductionWeight: netToDouble($('txtDeductionWt').value),
            Uom: comboVal('CmbDeductionWeightUom'),
            DeductionQty: netToDouble($('txtDeductionQty').value),
            DeductionRate: netToDouble($('TxtDecRate').value)
        };
    }

    /** Add_Click (btnAdd "+") */
    function addClick() {
        if (!formValidationDetail()) return;
        var clash = rangeClash(-1);
        if (clash) { alert(clash); return; }
        var v = entryValues();
        v.Id = 0;
        st.rows.push(v);
        if (st.sel < 0) st.sel = 0;
        renderGrd();
        /* Only these five are cleared (Deduction Wt and Qty stay for the next row). */
        clearCombo('CmbAnalysisParameter');
        $('txtRangeFrom').value = '';
        $('txtrangeTo').value = '';
        $('TxtDecRate').value = '';
        clearCombo('CmbDeductionWeightUom');
        focusCombo('CmbAnalysisParameter');
    }

    /** btnUpdateDetail_Click */
    function updateDetailClick() {
        if (!formValidationDetail()) return;
        var clash = rangeClash(st.updateDetailIndex);
        if (clash) { alert(clash); return; }
        var row = st.rows[st.updateDetailIndex];
        if (!row) { resetDetail(); return; }
        var v = entryValues();
        Object.keys(v).forEach(function (k) { row[k] = v[k]; });          // Id is kept
        renderGrd();
        resetDetail();
        focusCombo('CmbAnalysisParameter');
    }

    /** ResetDetail — the Uom combo is not cleared here. */
    function resetDetail() {
        st.updateDetailIndex = -1;
        clearCombo('CmbAnalysisParameter');
        $('txtRangeFrom').value = '';
        $('txtrangeTo').value = '';
        $('txtDeductionWt').value = '';
        $('txtDeductionQty').value = '';
        $('TxtDecRate').value = '';
        show('btnAdd', true);
        show('btnUpdateDetail', false);
        show('btnCancelDetail', false);
    }

    /** grd_DoubleClick — the current row goes back to the entry line. */
    function editRow(i) {
        var r = st.rows[i];
        if (!r) return;
        selectRow(i);
        st.updateDetailIndex = i;
        /* CmbAnalysisParameter.Value = id, then .Text = name: a parameter that is not in the current list
           stays as text only (no ActiveRow) and "Parameter Field is Required" is asked on Update. */
        if (has('CmbAnalysisParameter', r.AnalysisParameterId)) setVal('CmbAnalysisParameter', r.AnalysisParameterId);
        else setText('CmbAnalysisParameter', r.AnalysisParameter);
        $('txtRangeFrom').value = String(r.RangeFrom);
        $('txtrangeTo').value = String(r.RangeTo);
        $('txtDeductionWt').value = String(r.DeductionWeight);
        setText('CmbDeductionWeightUom', String(r.Uom));                   // .Text = Cells["Uom"].Value.ToString()
        $('txtDeductionQty').value = String(r.DeductionQty);
        $('TxtDecRate').value = String(r.DeductionRate);
        show('btnAdd', false);
        show('btnUpdateDetail', true);
        show('btnCancelDetail', true);
        focusCombo('CmbAnalysisParameter');
    }

    /** DeleteDetailRow — a stored row is confirmed and remembered for the save (ActionTypeId 3). */
    function deleteDetailRow(i) {
        var r = st.rows[i];
        if (!r) return;
        if (st.updateDetailIndex !== -1) { alert('Please Reset the Detail First...'); return; }
        if (intOf(r.Id) > 0) {
            if (!confirm('Are you sure to Delete?')) return;
            st.removed.push(intOf(r.Id));
        }
        st.rows.splice(i, 1);
        if (st.sel >= st.rows.length) st.sel = st.rows.length - 1;
        renderGrd();
    }

    // ============================================================================ form

    /** FormValidation — desktop order and texts. */
    function formValidation() {
        var p = $('txtPolicyName').value;
        if (p === '' || p.trim() === '0') { alert('PolicyName field is required'); focusEl('txtPolicyName'); return false; }
        if (comboVal('CmbAnalysisGroup') === 0) { alert('AnalysisGroup field is required'); focusCombo('CmbAnalysisGroup'); return false; }
        return true;
    }

    function payload() {
        return {
            id: st.recId,
            policyName: $('txtPolicyName').value,
            analysisGroupId: comboVal('CmbAnalysisGroup'),
            effectiveFrom: $('txtAffectiveDate').value || today(),
            effectiveTo: $('TxtAffectiveDateTo').value || today(),
            remarks: $('txtremarks').value,
            rows: st.rows.map(function (r) {
                return {
                    id: intOf(r.Id), analysisParameterId: intOf(r.AnalysisParameterId),
                    rangeFrom: Number(r.RangeFrom) || 0, rangeTo: Number(r.RangeTo) || 0,
                    deductionWeight: Number(r.DeductionWeight) || 0, uom: intOf(r.Uom),
                    deductionQty: Number(r.DeductionQty) || 0, deductionRate: Number(r.DeductionRate) || 0
                };
            }),
            removedDetailIds: st.recId > 0 ? st.removed.slice() : [],
            files: st.files.map(function (f) { return { name: f.name, base64: f.base64 }; }),
            keepAttachmentIds: st.att.map(function (a) { return intOf(a.Id); })
        };
    }

    /** Insert() — called by btnSave_Click (RecId = 0) and btnUpdate_Click. */
    function insert(button) {
        if (st.rows.length === 0) { alert('Grid Record Not Found'); return; }
        if (!formValidation()) return;
        if (!confirm(st.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        for (var i = 0; i < st.rows.length; i++) {
            if (intOf(st.rows[i].AnalysisParameterId) === 0) { alert('AnalysisParameter Field Required in Grid Row no : ' + (i + 1)); return; }
        }
        withBusy(button, async function () {
            var d = await api('POST', API + '/save', payload());
            alert(d.message);                       // "Data Save Successfully....  " / "Data Update Successfully....  " + PolicyName
            reset();
        });
    }
    /** btnSave_Click */
    function saveClick() {
        var b = $('btnSave');
        if (b.disabled || !visible('btnSave')) return;
        st.recId = 0;
        insert(b);
    }
    /** btnUpdate_Click */
    function updateClick() {
        var b = $('btnUpdate');
        if (b.disabled || !visible('btnUpdate')) return;
        if (st.recId === 0) { alert('Record cannot be updated because RecId not found'); return; }
        insert(b);
    }
    /** btnDelete_Click */
    function deleteClick() {
        var b = $('btnDelete');
        if (b.disabled || !visible('btnDelete')) return;
        if (!(st.recId > 0)) { alert('Record Not Found'); return; }
        if (!confirm('Are you sure to Delete?')) return;
        withBusy(b, async function () {
            var d = await api('POST', API + '/' + st.recId + '/delete');
            alert(d.message);                       // "Delete Record Successfully"
            reset();
        });
    }

    /** Reset() — the analysis group, the dates and the Uom keep their values. */
    function reset() {
        st.att = []; st.files = [];                 // AT = new Attachment()
        st.removed = [];
        st.recId = 0;
        show('btnSave', true);
        show('btnUpdate', false);
        show('btnDelete', false);
        $('txtPolicyName').value = '';
        $('txtremarks').value = '';
        st.rows = []; st.sel = -1;
        renderGrd();
        resetDetail();
        footer();
        focusEl('txtPolicyName');
    }

    /** ReadById */
    async function readById(id) {
        var d = await api('GET', API + '/' + intOf(id));
        st.recId = intOf(d.Id);
        selectTab('tabForm', true);                 // tabControl1.SelectedIndex = 0
        show('btnSave', false);
        show('btnUpdate', true);
        show('btnDelete', true);
        $('txtPolicyName').value = d.PolicyName || '';
        setVal('CmbAnalysisGroup', d.InvLabGroupAnalysisStandardsId);
        $('txtremarks').value = d.RemarksHeader || '';
        $('txtAffectiveDate').value = isoDay(d.EffectiveFrom) || today();
        $('TxtAffectiveDateTo').value = isoDay(d.EfffectiveTo) || today();
        if (comboVal('CmbAnalysisGroup') > 0) { groupSeq++; bindParameters(d.parameters); }   // CmbAnalysisGroup_Leave
        st.rows = (d.rows || []).slice();
        st.sel = st.rows.length ? 0 : -1;
        renderGrd();
        st.att = (d.attachments || []).slice();     // FormHelper.LoadAttachmentsForObject
        st.files = [];
        footer();
        focusEl('txtPolicyName');
    }
    /** grdhistory "Edit" / the PolicyName link / Ctrl+Enter: Reset() then ReadById. */
    function openRecord(id, button) {
        if (!intOf(id)) return;
        withBusy(button || null, async function () {
            reset();
            await readById(id);
        });
    }

    function footer() {
        $('footerInfo').textContent = st.recId > 0 ? 'Editing record ' + st.recId + ' — ' + $('txtPolicyName').value : 'New record';
    }

    /** btnRefresh_Click — AnalysisGroup() + UOM(). */
    function refreshClick() {
        withBusy($('btnRefresh'), async function () {
            var d = await api('GET', API + '/refresh');
            bindAnalysisGroup(d.analysisGroups);
            bindUom(d.uoms);
        });
    }

    // ============================================================================ history

    function histShown(c, r) { return shown(c, r[c.key]); }
    function visibleRows(rows, cols, filter) {
        return rows.map(function (r, i) { return { r: r, i: i }; }).filter(function (x) {
            return cols.every(function (c) {
                var f = (filter[c.key] || '').trim().toLowerCase();
                return !f || histShown(c, x.r).toLowerCase().indexOf(f) >= 0;
            });
        });
    }
    function filterRow(cols, filter, lead) {
        return '<tr class="flt">' + (lead || '') + cols.map(function (c) {
            return '<th><input type="text" data-flt="' + esc(c.key) + '" value="' + esc(filter[c.key] || '') + '"></th>';
        }).join('') + '</tr>';
    }
    function renderHistBody() {
        var t = $('grdhistory'), vis = visibleRows(st.hist, HIST_COLS, st.histFilter);
        t.tBodies[0].innerHTML = vis.map(function (x) {
            return '<tr data-i="' + x.i + '"' + (x.i === st.histSel ? ' class="is-sel"' : '') + '>'
                + '<td class="c frz"><button type="button" class="gbtn" data-edit="1">Edit</button></td>'
                + HIST_COLS.map(function (c) { return cell(c, x.r); }).join('') + '</tr>';
        }).join('');
        t.tFoot.innerHTML = totals(HIST_COLS, vis.map(function (x) { return x.r; }), '<td class="frz"></td>');
        var pos = vis.map(function (x) { return x.i; }).indexOf(st.histSel);
        $('grdhistoryNav').textContent = 'Record ' + (pos >= 0 ? pos + 1 : 0) + ' of ' + vis.length;
    }
    function renderHist() {
        var t = $('grdhistory');
        if (st.hist.length === 0) {                                     // grdhistory.ClearStructure()
            t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = '';
            $('grdhistoryNav').textContent = 'Record 0 of 0';
            return;
        }
        t.tHead.innerHTML = head(HIST_COLS, '<th class="frz" style="width:40px;">Edit</th>')
            + filterRow(HIST_COLS, st.histFilter, '<th class="frz"></th>');   // FilterMode Automatic
        renderHistBody();
    }
    function renderHistDetailBody() {
        var t = $('DataGridHistoryDetail'), vis = visibleRows(st.histDetail, DETAIL_COLS, st.histDetailFilter);
        t.tBodies[0].innerHTML = vis.map(function (x) {
            return '<tr>' + DETAIL_COLS.map(function (c) { return cell(c, x.r); }).join('') + '</tr>';
        }).join('');
        t.tFoot.innerHTML = totals(DETAIL_COLS, vis.map(function (x) { return x.r; }));
    }
    function renderHistDetail(has) {
        var t = $('DataGridHistoryDetail');
        if (!has) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; return; }   // ClearStructure / DataSource = null
        t.tHead.innerHTML = head(DETAIL_COLS) + filterRow(DETAIL_COLS, st.histDetailFilter);
        renderHistDetailBody();
    }

    /** grdhistory_SelectionChanged -> GridDetailBind */
    var detailSeq = 0;
    async function selectHist(i) {
        st.histSel = i;
        Array.prototype.forEach.call($('grdhistory').tBodies[0].rows, function (r) { r.classList.toggle('is-sel', intOf(r.getAttribute('data-i')) === i); });
        var vis = visibleRows(st.hist, HIST_COLS, st.histFilter), pos = vis.map(function (x) { return x.i; }).indexOf(i);
        $('grdhistoryNav').textContent = 'Record ' + (pos >= 0 ? pos + 1 : 0) + ' of ' + vis.length;
        var row = st.hist[i];
        if (!row) return;
        var mine = ++detailSeq;
        try {
            var rows = await api('GET', API + '/' + intOf(row.Id) + '/details');
            if (mine !== detailSeq) return;
            st.histDetail = rows || [];
            renderHistDetail(true);
        } catch (e) { alert(msg(e)); }
    }

    /** btnShow_Click -> gridhistoryfill */
    function showHistory() {
        withBusy($('btnShow'), async function () {
            var q = '?dateType=' + encodeURIComponent(document.querySelector('input[name="histDate"]:checked').value);
            if ($('FromDateHistoryChecked').checked) q += '&fromDate=' + encodeURIComponent($('FromDateHistory').value || today());
            if ($('ToDateHistoryChecked').checked) q += '&toDate=' + encodeURIComponent($('ToDateHistory').value || today());
            var rows = await api('GET', API + '/history' + q);
            st.hist = rows || [];
            st.histSel = -1;
            if (st.hist.length === 0) {
                renderHist();                                            // grdhistory.ClearStructure()
                st.histDetail = []; renderHistDetail(false);             // DataGridHistoryDetail.ClearStructure()
                return;
            }
            renderHist();
            await selectHist(0);                                         // the first row becomes current -> SelectionChanged
        });
    }
    /** btnResetHistory_Click */
    function resetHistory() {
        $('FromDateHistory').value = addDays(-3);
        $('ToDateHistory').value = today();
        st.hist = []; st.histSel = -1; st.histFilter = {};
        st.histDetail = []; st.histDetailFilter = {};
        detailSeq++;
        renderHist();
        renderHistDetail(false);
        $('rdentrydate').checked = true;
    }

    // ============================================================================ attachments

    function readFile(file) {
        return new Promise(function (resolve, reject) {
            var fr = new FileReader();
            fr.onload = function () {
                var s = String(fr.result || ''), i = s.indexOf(',');
                resolve({ name: file.name, base64: i >= 0 ? s.substring(i + 1) : '', size: file.size });
            };
            fr.onerror = function () { reject(new Error('The file could not be read.')); };
            fr.readAsDataURL(file);
        });
    }
    function renderAttachment() {
        var html = '';
        st.att.forEach(function (a, i) {
            html += '<tr><td>' + (st.recId ? '<a class="lnk" href="' + API + '/' + st.recId + '/attachments/' + intOf(a.Id) + '" target="_blank" rel="noopener">' + esc(a.Attachment) + '</a>' : esc(a.Attachment))
                + '</td><td class="n">' + esc(sizeMb(a.UploadedFileSizeMb)) + '</td><td><button type="button" class="gbtn" data-del-att="' + i + '">Delete</button></td></tr>';
        });
        st.files.forEach(function (f, i) {
            html += '<tr><td>' + esc(f.name) + '</td><td class="n">' + esc(sizeMb(f.size / 1048576)) + '</td><td><button type="button" class="gbtn" data-del-file="' + i + '">Delete</button></td></tr>';
        });
        $('grdAttachment').tBodies[0].innerHTML = html;
    }
    async function attachmentsChosen() {
        var list = Array.prototype.slice.call($('attFile').files || []);
        for (var i = 0; i < list.length; i++) {
            var f = list[i];
            var present = st.files.some(function (x) { return x.name === f.name; }) || st.att.some(function (x) { return x.Attachment === f.name; });
            if (present) { alert(f.name + ' File Already Present'); continue; }
            if (f.size === 0 || f.size > MAX_BYTES) { alert(f.name + ': Attachment must be between 1 byte and 5 MB'); continue; }
            if (st.files.length >= 10) { alert('At most ten attachments may be uploaded at once'); break; }
            try {
                var p = await readFile(f);
                st.files.push(p);
            } catch (e) { alert(msg(e)); }
        }
        $('attFile').value = '';
        renderAttachment();
    }
    /** grdhistory_LinkClicked — CommonServices.GetNoofAttachmentsByScreenName(Id, "frmQcDeductionPolicy"). */
    function viewAttachments(id) {
        withBusy(null, async function () {
            var rows = await api('GET', API + '/' + intOf(id) + '/attachments');
            var total = 0;
            $('grdAttachmentView').tBodies[0].innerHTML = (rows || []).map(function (a) {
                total += Number(a.UploadedFileSizeMb) || 0;
                return '<tr><td><a class="lnk" href="' + API + '/' + intOf(id) + '/attachments/' + intOf(a.Id) + '" target="_blank" rel="noopener">' + esc(a.Attachment) + '</a></td><td class="n">'
                    + esc(sizeMb(a.UploadedFileSizeMb)) + '</td><td>' + esc(a.EntryUserName || '') + '</td><td>' + esc(dMMMyyTime(a.EntryDate)) + '</td></tr>';
            }).join('');
            $('lblTotalFiles').textContent = String((rows || []).length);
            $('lblTotalSize').textContent = total.toFixed(3);
            show('dlgAttachmentView', true);
        });
    }

    // ============================================================================ tabs / keys

    function activeTab() { return $('tabHistory').classList.contains('is-active') ? 'tabHistory' : 'tabForm'; }
    /** tabControl1_SelectedIndexChanged — History: focus From Date; Form: focus Policy Name. */
    function selectTab(id, quiet) {
        ['tabForm', 'tabHistory'].forEach(function (t) { $(t).classList.toggle('is-active', t === id); });
        $('tabBtnForm').classList.toggle('is-active', id === 'tabForm');
        $('tabBtnHistory').classList.toggle('is-active', id === 'tabHistory');
        $('btnHistoryText').textContent = id === 'tabHistory' ? 'Form' : 'History';
        if (quiet) return;
        if (id === 'tabHistory') focusEl('FromDateHistory'); else focusEl('txtPolicyName');
    }
    function toggleTab() { selectTab(activeTab() === 'tabHistory' ? 'tabForm' : 'tabHistory'); }

    function showShortCutKeys() {
        $('grdShortCutKeys').tBodies[0].innerHTML = SHORTCUTS.map(function (r) { return '<tr><td>' + esc(r[0]) + '</td><td>' + esc(r[1]) + '</td></tr>'; }).join('');
        show('dlgShortCutKeys', true);
    }
    function openDialog() {
        return ['dlgAttachment', 'dlgAttachmentView', 'dlgShortCutKeys'].filter(function (id) { return !$(id).hidden; })[0] || null;
    }

    /** SendKeys.Send("{TAB}") — Enter moves to the next control in the form's tab order. */
    function focusNext(from) {
        var page = $(activeTab());
        var list = Array.prototype.filter.call(page.querySelectorAll('input, select, textarea, button, .wf-grid[tabindex]'), function (el) {
            return !el.disabled && el.type !== 'hidden' && el.type !== 'file' && el.tabIndex >= 0 && el.offsetParent !== null
                && !el.classList.contains('wf-tool') && !el.classList.contains('qdp-full') && !el.closest('table')
                && !(el.tagName === 'SELECT' && el.parentNode.classList && el.parentNode.classList.contains('dtcombo-wrap'));
        });
        var i = list.indexOf(from);
        if (i >= 0 && list.length > 1) list[(i + 1) % list.length].focus();
    }

    /** CommonServices.OnlytextdecimelFunction — digits and a single decimal point. */
    function decimalKeyPress(e) {
        if (e.ctrlKey || e.metaKey || e.altKey || !e.key || e.key.length !== 1) return;
        var el = e.target;
        if (/\d/.test(e.key)) return;
        if (e.key === '.' && el.value.indexOf('.') < 0) return;
        e.preventDefault();
    }
    function decimalPaste(e) {
        var el = e.target;
        setTimeout(function () {
            var v = el.value.replace(/[^\d.]/g, ''), i = v.indexOf('.');
            if (i >= 0) v = v.substring(0, i + 1) + v.substring(i + 1).replace(/\./g, '');
            if (v !== el.value) el.value = v;
        }, 0);
    }

    /** frmQcDeductionPolicy_KeyDown + grd_KeyDown + grdhistory_KeyDown */
    function keyDown(e) {
        var dlg = openDialog();
        if (dlg) {
            if (e.key === 'Escape') { show(dlg, false); e.preventDefault(); }
            return;
        }
        var ctrl = e.ctrlKey || e.metaKey, key = e.key, k = (key || '').toLowerCase();
        var t = e.target, inGrd = !!(t.closest && t.closest('#grdWrap')), inHist = !!(t.closest && t.closest('#grdhistoryWrap'));
        var onForm = activeTab() === 'tabForm';

        /* grid keys */
        if (inGrd && st.rows.length) {
            if (!ctrl && (key === 'ArrowDown' || key === 'ArrowUp')) {
                e.preventDefault();
                selectRow(Math.max(0, Math.min(st.rows.length - 1, st.sel + (key === 'ArrowDown' ? 1 : -1))));
                return;
            }
            if (ctrl && key === 'Enter') { e.preventDefault(); editRow(st.sel); return; }               // grd_DoubleClick
            if (ctrl && key === 'Delete' && !e.shiftKey) { e.preventDefault(); deleteDetailRow(st.sel); return; }
            if (ctrl && k === ' ' && t.getAttribute && t.getAttribute('data-del')) { e.preventDefault(); deleteDetailRow(st.sel); return; }
        }
        if (inHist && st.hist.length && !(t.tagName === 'INPUT')) {
            if (!ctrl && (key === 'ArrowDown' || key === 'ArrowUp')) {
                e.preventDefault();
                var vis = visibleRows(st.hist, HIST_COLS, st.histFilter).map(function (x) { return x.i; });
                var p = vis.indexOf(st.histSel), n = Math.max(0, Math.min(vis.length - 1, p + (key === 'ArrowDown' ? 1 : -1)));
                if (vis.length && vis[n] !== st.histSel) selectHist(vis[n]);
                return;
            }
            var cur = st.hist[st.histSel];
            if (cur && ctrl && key === 'Enter') { e.preventDefault(); openRecord(cur.Id); return; }
            if (cur && ctrl && k === ' ') {
                if (t.getAttribute && t.getAttribute('data-att')) { e.preventDefault(); viewAttachments(cur.Id); return; }
                e.preventDefault(); openRecord(cur.Id); return;                                         // the "Edit" column
            }
        }

        if (key === 'Enter' && !ctrl && !e.altKey && !e.shiftKey) {
            if (t.tagName === 'TEXTAREA' || t.tagName === 'BUTTON' || t.tagName === 'A') return;
            if (t.classList && t.classList.contains('dtcombo-input') && t.getAttribute('aria-expanded') === 'true') return;
            e.preventDefault();
            focusNext(t);
            return;
        }
        /* Escape first closes an open combo list (the combo's own key), it does not leave the page then. */
        if (key === 'Escape' && t.classList && t.classList.contains('dtcombo-input') && t.getAttribute('aria-expanded') === 'true') return;
        if ((ctrl && k === 'e') || key === 'Escape') { e.preventDefault(); window.location.href = $('lnkExit').getAttribute('href'); return; }   // Close()
        if (ctrl && e.altKey && (key === 'Control' || key === 'Alt')) { e.preventDefault(); showShortCutKeys(); return; }
        if (ctrl && k === 't') { e.preventDefault(); toggleTab(); return; }
        if (!ctrl) return;
        if (onForm) {
            if (k === 'n') { e.preventDefault(); newClick(); }
            else if (k === 's') { e.preventDefault(); saveClick(); }
            else if (k === 'r') { e.preventDefault(); refreshClick(); }
            else if (k === 'u') { e.preventDefault(); updateClick(); }
            else if (key === 'F5') { e.preventDefault(); focusEl('txtPolicyName'); }
            else if (key === 'F10') { e.preventDefault(); attachmentClick(); }
            else if (key === 'ArrowDown') { e.preventDefault(); focusEl('grdWrap'); if (st.sel < 0 && st.rows.length) selectRow(0); }
            else if (key === 'ArrowUp') { e.preventDefault(); focusEl('txtPolicyName'); }
            else if (e.shiftKey && key === 'Delete') { e.preventDefault(); deleteClick(); }
        } else {
            if (k === 's') { e.preventDefault(); showHistory(); }
            else if (k === 'n') { e.preventDefault(); resetHistory(); }
            else if (k === 'r') { e.preventDefault(); }                                                  // btnRefreshHistory_Click is empty
            else if (key === 'F5') { e.preventDefault(); focusEl('FromDateHistory'); }
            else if (key === 'ArrowDown') { e.preventDefault(); focusEl('grdhistoryWrap'); }
            else if (key === 'ArrowUp') { e.preventDefault(); focusEl('FromDateHistory'); }
        }
    }

    /** btnNew_Click */
    function newClick() { withBusy($('btnNew'), async function () { reset(); }); }
    /** btnattachment_Click — AT.Show() */
    function attachmentClick() { renderAttachment(); show('dlgAttachment', true); }

    // ============================================================================ wiring

    function wire() {
        $('btnNew').addEventListener('click', newClick);
        $('btnRefresh').addEventListener('click', refreshClick);
        $('btnSave').addEventListener('click', saveClick);
        $('btnUpdate').addEventListener('click', updateClick);
        $('btnDelete').addEventListener('click', deleteClick);
        $('btnattachment').addEventListener('click', attachmentClick);
        $('BtnShortCutkeys').addEventListener('click', showShortCutKeys);

        /* CmbAnalysisGroup_Leave: the parameter list follows the chosen group. */
        $('CmbAnalysisGroup').addEventListener('change', function () {
            withBusy(null, analysisGroupLeave);
        });

        $('btnAdd').addEventListener('click', addClick);
        $('btnUpdateDetail').addEventListener('click', updateDetailClick);
        $('btnCancelDetail').addEventListener('click', resetDetail);
        ['txtRangeFrom', 'txtrangeTo', 'txtDeductionWt', 'txtDeductionQty', 'TxtDecRate'].forEach(function (id) {
            $(id).addEventListener('keypress', decimalKeyPress);
            $(id).addEventListener('paste', decimalPaste);
        });

        /* grd_ColumnButtonClick / grd_DoubleClick */
        var grd = $('grd');
        grd.addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr');
            if (!tr) return;
            var i = intOf(tr.getAttribute('data-i'));
            selectRow(i);
            if (e.target.closest('[data-del]')) deleteDetailRow(i);
            else if (e.target.closest('[data-edit]')) editRow(i);
        });
        grd.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tbody tr');
            if (!tr || e.target.closest('[data-del]') || e.target.closest('[data-edit]')) return;
            editRow(intOf(tr.getAttribute('data-i')));
        });

        /* grdhistory_ColumnButtonClick / LinkClicked / DoubleClick / SelectionChanged + the filter row */
        var gh = $('grdhistory');
        gh.addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr');
            if (!tr) return;
            var i = intOf(tr.getAttribute('data-i')), row = st.hist[i];
            if (!row) return;
            var edit = e.target.closest('[data-edit]');
            if (edit) { st.histSel = i; openRecord(row.Id, edit); return; }
            if (e.target.closest('[data-open]')) { st.histSel = i; openRecord(row.Id); return; }
            if (i !== st.histSel) selectHist(i);
            if (e.target.closest('[data-att]')) viewAttachments(row.Id);
        });
        gh.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tbody tr');
            if (!tr || e.target.closest('[data-edit]') || e.target.closest('[data-open]') || e.target.closest('[data-att]')) return;
            var row = st.hist[intOf(tr.getAttribute('data-i'))];
            if (row) openRecord(row.Id);
        });
        gh.addEventListener('input', function (e) {
            var k = e.target.getAttribute && e.target.getAttribute('data-flt');
            if (!k) return;
            st.histFilter[k] = e.target.value;
            renderHistBody();
        });
        $('DataGridHistoryDetail').addEventListener('input', function (e) {
            var k = e.target.getAttribute && e.target.getAttribute('data-flt');
            if (!k) return;
            st.histDetailFilter[k] = e.target.value;
            renderHistDetailBody();
        });

        $('btnShow').addEventListener('click', showHistory);
        $('btnResetHistory').addEventListener('click', function () { withBusy($('btnResetHistory'), async function () { resetHistory(); }); });
        /* DateTimePicker.ShowCheckBox: an unchecked picker is greyed and its date is not sent. */
        [['FromDateHistoryChecked', 'FromDateHistory'], ['ToDateHistoryChecked', 'ToDateHistory']].forEach(function (p) {
            $(p[0]).addEventListener('change', function () { $(p[1]).disabled = !$(p[0]).checked; });
        });

        $('tabBtnForm').addEventListener('click', function () { selectTab('tabForm'); });
        $('tabBtnHistory').addEventListener('click', function () { selectTab('tabHistory'); });
        /* footer History button: Form <-> History */
        $('btnHistory').addEventListener('click', function () { withBusy($('btnHistory'), async function () { toggleTab(); }); });

        /* fullscreen toggle of a grid */
        Array.prototype.forEach.call(document.querySelectorAll('.qdp-full'), function (b) {
            b.addEventListener('click', function () {
                var box = $(b.getAttribute('data-full')), on = !box.classList.contains('is-full');
                box.classList.toggle('is-full', on);
                b.title = on ? 'Exit fullscreen' : 'Fullscreen';
                b.firstElementChild.className = on ? 'fa fa-compress' : 'fa fa-arrows-alt';
            });
        });

        /* dialogs */
        $('Choose').addEventListener('click', function () { $('attFile').value = ''; $('attFile').click(); });
        $('attFile').addEventListener('change', function () { withBusy($('Choose'), attachmentsChosen); });
        $('btnClose').addEventListener('click', function () { show('dlgAttachment', false); });
        $('dlgAttachmentX').addEventListener('click', function () { show('dlgAttachment', false); });
        $('grdAttachment').addEventListener('click', function (e) {
            var a = e.target.getAttribute('data-del-att'), f = e.target.getAttribute('data-del-file');
            if (a !== null) st.att.splice(intOf(a), 1);
            else if (f !== null) st.files.splice(intOf(f), 1);
            else return;
            renderAttachment();
        });
        $('dlgAttachmentViewX').addEventListener('click', function () { show('dlgAttachmentView', false); });
        $('dlgShortCutKeysX').addEventListener('click', function () { show('dlgShortCutKeys', false); });

        document.addEventListener('keydown', keyDown);
    }

    /** frmQcDeductionPolicy_Load */
    async function init() {
        wire();
        renderGrd();
        renderHist();
        renderHistDetail(false);
        $('txtAffectiveDate').value = today();                              // DateTimePicker default
        $('TxtAffectiveDateTo').value = today();
        $('FromDateHistory').value = addDays(-3);
        $('ToDateHistory').value = today();
        footer();
        try {
            L = await api('GET', API + '/lookups');
        } catch (e) {
            $('rightsNote').textContent = msg(e);
            show('rightsNote', true);
            ['btnNew', 'btnRefresh', 'btnSave', 'btnUpdate', 'btnDelete', 'btnattachment', 'btnAdd', 'btnShow', 'btnResetHistory'].forEach(function (id) { $(id).disabled = true; });
            return;
        }
        var r = rights();
        setEnabled('btnSave', !!r.save);                                    // DoHaveSaveRight
        setEnabled('btnUpdate', !!r.update);                                // DoHaveUpdateRights
        setEnabled('btnDelete', !!r['delete']);                             // DoHaveCanDelete
        st.rateDecimals = intOf(L.rateDecimals) || 2;
        bindAnalysisGroup(L.analysisGroups);                                // AnalysisGroup()
        bindUom(L.uoms);                                                    // UOM()
        renderGrd();
        $('FromDateHistory').value = isoDay(L.historyFrom) || addDays(-3);
        $('ToDateHistory').value = isoDay(L.today) || today();
        focusEl('txtPolicyName');
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init);
    else init();
})();
