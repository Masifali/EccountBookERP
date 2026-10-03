/* ============================================================================================
 * Screen 158 "Group Analysis Standards" - Architecture.WinApp.Lab/InvLabGroupAnalysisStandards.cs.
 * Line references (:n) are that file. BLL 0406_Architecture.BLL.Lab.InvLabGroupAnalysisStandards.cs.
 *
 * The form: two combos, two numeric boxes, one check box, a toolbar (New, Refresh, Update, Save) and
 * one read-only Janus grid grouped by AnalysisGroup. No delete, no print, no history, no prompts.
 * The server re-validates everything it saves and takes organization / company from the session.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/lab/group-analysis-standards';
    var EXIT_URL = '/quality';

    /* grdfrmSetting (:316): column name, Width, numeric. RetrieveStructure (:307) captions a column with
       its DataTable column name. Id is hidden (:341); AnalysisGroup is the group column and
       HideColumnsWhenGrouped is True (:321-322), so it shows only in the group rows. */
    var COLS = [
        { key: 'AnalysisItem', width: 200, open: true },            // rule 4: opens the record (= grdfrm_DoubleClick)
        { key: 'MinValue', width: 65, num: true },
        { key: 'MaxValue', width: 65, num: true },
        { key: 'IsEditableAfterApproval', width: 80, check: true }   // rule 4: boolean column drawn as a read-only check box
    ];

    var L = null;                       // lookups
    var st = {
        recId: 0,                       // RecId (:24)
        all: [],                        // dtgetAll (:30)
        view: [],                       // the DataTable currently bound to grdfrm
        filter: {},                     // filter row texts
        collapsed: {},                  // collapsed group rows
        selId: 0                        // grdfrm.CurrentRow
    };

    // ============================================================================ helpers

    function $(id) { return document.getElementById(id); }
    function esc(v) {
        return String(v === undefined || v === null ? '' : v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    /** Conversion.ToString - null is "". */
    function str(v) { return v === undefined || v === null ? '' : String(v); }
    /** Conversion.ToDouble on text - anything that does not convert is 0. */
    function num(v) {
        var s = String(v === undefined || v === null ? '' : v).replace(/,/g, '').trim();
        if (s === '') return 0;
        var n = Number(s);
        return isFinite(n) ? n : 0;
    }
    /** Conversion.ToBool - "1" counts as true. */
    function bool(v) { return v === true || v === 1 || v === '1' || String(v).toLowerCase() === 'true'; }
    function msg(e) { return e && e.message ? e.message : String(e); }
    function rights() { return (L && L.rights) || {}; }
    function show(id, on) { $(id).hidden = !on; }

    async function api(method, url, body) {
        var opt = { method: method, credentials: 'same-origin', headers: { Accept: 'application/json' } };
        if (body !== undefined) { opt.headers['Content-Type'] = 'application/json'; opt.body = JSON.stringify(body); }
        var res = await fetch(url, opt);
        var text = await res.text();
        var data = null;
        try { data = text ? JSON.parse(text) : null; } catch (e) { data = null; }
        if (!res.ok) throw new Error(data && (data.message || data.error) || ('The request failed (' + res.status + ').'));
        return data;
    }
    /** Busy panel + button lock; any refusal is shown as the desktop's MessageBox (catch -> MessageBox.Show(ex.Message)). */
    function act(button, work) {
        /* Rule 5 (countx_lab_page_kit.js): the button is disabled at once, carries a spinner (btn-busy), ignores
           further clicks while the request is in flight and is re-enabled in finally - success or failure. */
        if (window.LabKit) return window.LabKit.act(button || null, work);
        var runner = window.PurchaseRequest ? window.PurchaseRequest.run.bind(window.PurchaseRequest) : function (b, w) { return w(); };
        return runner(button || null, async function () {
            try { await work(); } catch (e) { alert(msg(e)); }
        });
    }

    // ============================================================================ combos

    function fill(id, rows, valueKey, textKey) {
        var html = '';
        rows.forEach(function (r) { html += '<option value="' + esc(r[valueKey]) + '">' + esc(r[textKey]) + '</option>'; });
        $(id).innerHTML = html;
        $(id).selectedIndex = -1;
    }
    /** combo.Text */
    function comboText(id) {
        var el = $(id);
        return el.selectedIndex < 0 ? '' : el.options[el.selectedIndex].textContent;
    }
    /** Conversion.ToInt(combo.Value) */
    function comboVal(id) {
        var el = $(id), n = el.selectedIndex < 0 ? 0 : parseInt(el.value, 10);
        return isNaN(n) ? 0 : n;
    }
    /** combo.Text = text - selects the row with that display text; LimitToList, so no other text can stay. */
    function setComboText(id, text) {
        var el = $(id), t = str(text), idx = -1;
        if (t !== '') {
            for (var i = 0; i < el.options.length; i++) {
                if (el.options[i].textContent.toLowerCase() === t.toLowerCase()) { idx = i; break; }
            }
        }
        el.selectedIndex = idx;
    }
    function focusEl(id) {
        var el = $(id), w = el.closest ? el.closest('.dtcombo-wrap') : null, i = w && w.querySelector('.dtcombo-input');
        (i || el).focus();
    }
    /** cmbgroup.Text = text, raising cmbgroup_TextChanged when the text really changes. */
    function setGroupText(text) {
        var before = comboText('cmbgroup');
        setComboText('cmbgroup', text);
        if (comboText('cmbgroup') !== before) cmbgroup_TextChanged();
    }

    /** GroupFill (:177) - BindDDL(dtGroup, cmbgroup, "Id", "Group", "Analysis Group", false); only when rows exist (:197). */
    function GroupFill(rows) {
        if (!rows || rows.length <= 0) return;
        var before = comboText('cmbgroup');
        fill('cmbgroup', rows, 'Id', 'AnalysisGroupDescription');
        setComboText('cmbgroup', before);
    }
    /** ItemFill (:151) - BindDDLNew(dt, cmbitem, "Id", "AnalysisParameterDescription", "Analysis Parameter Item", false); only when rows exist (:166). */
    function ItemFill(rows) {
        if (!rows || rows.length <= 0) return;
        var before = comboText('cmbitem');
        fill('cmbitem', rows, 'Id', 'AnalysisParameterDescription');
        setComboText('cmbitem', before);
    }

    // ============================================================================ grid

    /** One grid row of the table built in gridfill (:292-305) / cmbgroup_TextChanged (:482-498). */
    function tableRow(r) {
        return {
            Id: r.Id,
            AnalysisGroup: str(r.AnalysisGroupDescription),
            AnalysisItem: str(r.AnalysisParameterDescription),
            MinValue: r.MinValue === null || r.MinValue === undefined ? '' : r.MinValue,
            MaxValue: r.MaxValue === null || r.MaxValue === undefined ? '' : r.MaxValue,
            IsEditableAfterApproval: bool(r.IsEditableAfterApproval) ? 'true' : 'false'
        };
    }
    /** gridfill (:282) - every row of dtgetAll. */
    function gridfill(rows) {
        st.all = rows || [];
        st.view = st.all.map(tableRow);
        renderGrid();
    }
    /**
     * cmbgroup_TextChanged (:474). Nothing happens while dtgetAll is empty (:478); otherwise the grid is
     * rebuilt with only the rows whose AnalysisGroupDescription equals the combo text (:494). An empty
     * text matches no row, so clearing the combo (New, or the refresh() after Save / Update) leaves the
     * grid without rows until Refresh or the next pick - that is what the desktop does.
     */
    function cmbgroup_TextChanged() {
        if (st.all.length <= 0) return;
        var text = comboText('cmbgroup');
        st.view = st.all.filter(function (r) { return text === str(r.AnalysisGroupDescription); }).map(tableRow);
        renderGrid();
    }

    function visibleRows() {
        return st.view.filter(function (r) {
            return COLS.every(function (c) {
                var f = st.filter[c.key];
                return !f || String(r[c.key]).toLowerCase().indexOf(f.toLowerCase()) >= 0;
            });
        });
    }
    function renderGrid() {
        var t = $('grdfrm');
        var active = document.activeElement && document.activeElement.getAttribute ? document.activeElement.getAttribute('data-f') : null;
        t.tHead.innerHTML = '<tr>' + COLS.map(function (c) {
            return '<th style="width:' + c.width + 'px;min-width:' + c.width + 'px;">' + esc(c.key) + '</th>';
        }).join('') + '<th></th></tr><tr class="flt">' + COLS.map(function (c) {
            return '<td><input type="text" data-f="' + c.key + '" value="' + esc(st.filter[c.key] || '') + '"></td>';
        }).join('') + '<td></td></tr>';

        var rows = visibleRows(), groups = [], byName = {};
        rows.forEach(function (r) {
            if (!byName.hasOwnProperty(r.AnalysisGroup)) { byName[r.AnalysisGroup] = []; groups.push(r.AnalysisGroup); }
            byName[r.AnalysisGroup].push(r);
        });
        groups.sort(function (a, b) { return a.toLowerCase() < b.toLowerCase() ? -1 : a.toLowerCase() > b.toLowerCase() ? 1 : 0; });
        var html = '';
        groups.forEach(function (g) {
            var closed = !!st.collapsed[g];
            html += '<tr class="grp" data-g="' + esc(g) + '"><td colspan="' + (COLS.length + 1) + '"><span class="tw">'
                + (closed ? '+' : '&minus;') + '</span> AnalysisGroup: ' + esc(g) + '</td></tr>';
            if (closed) return;
            byName[g].forEach(function (r) {
                html += '<tr data-id="' + esc(r.Id) + '"' + (String(r.Id) === String(st.selId) ? ' class="is-sel"' : '') + '>'
                    + COLS.map(function (c) {
                        if (c.check) return '<td class="chk" title="' + esc(r[c.key]) + '"><input type="checkbox" disabled' + (r[c.key] === 'true' ? ' checked' : '') + '></td>';
                        if (c.open) return '<td><a class="lab-open" data-open="1" title="Open this record">' + esc(r[c.key]) + '</a></td>';
                        return '<td' + (c.num ? ' class="n"' : '') + '>' + esc(r[c.key]) + '</td>';
                    }).join('')
                    + '<td></td></tr>';
            });
        });
        t.tBodies[0].innerHTML = html;
        /* TotalRow = True (:724) with no aggregate defined on any column: an empty total row. */
        t.tFoot.innerHTML = '<tr>' + COLS.map(function () { return '<td></td>'; }).join('') + '<td></td></tr>';
        updateNav(rows);
        if (active) {
            var again = t.querySelector('input[data-f="' + active + '"]');
            if (again) { again.focus(); var n = again.value.length; again.setSelectionRange(n, n); }
        }
    }
    function updateNav(rows) {
        rows = rows || visibleRows();
        var pos = 0;
        rows.forEach(function (r, i) { if (String(r.Id) === String(st.selId)) pos = i + 1; });
        $('grdfrmNav').textContent = pos + ' of ' + rows.length;
    }
    function selectRow(id) {
        st.selId = id;
        Array.prototype.forEach.call($('grdfrm').tBodies[0].rows, function (tr) {
            tr.classList.toggle('is-sel', tr.hasAttribute('data-id') && tr.getAttribute('data-id') === String(id));
        });
        updateNav();
    }

    // ============================================================================ form

    /** formvalidation (:92) - the desktop's texts and focus targets, in the desktop's order. */
    function formvalidation() {
        if (comboText('cmbgroup').trim() === '') { alert('Please select Analysis Group'); focusEl('cmbgroup'); return false; }
        if (comboText('cmbitem').trim() === '') { alert('Please select Analysis Parameter'); focusEl('cmbitem'); return false; }
        if ($('txtminvalue').value.trim() === '') { alert('Min Value Required'); $('txtminvalue').focus(); return false; }
        if ($('txtmaxvalue').value.trim() === '') { alert('Max Value Required'); $('txtmaxvalue').focus(); return false; }
        if (num($('txtminvalue').value) === num($('txtmaxvalue').value)) { alert('Min Value cant equal to Max Value'); $('txtmaxvalue').focus(); return false; }
        if (num($('txtminvalue').value) > num($('txtmaxvalue').value)) { alert('Min Value cant Greater than Max Value'); $('txtmaxvalue').focus(); return false; }
        return true;
    }

    /** refresh() (:133). chkEditableAfterApproval is not touched by the desktop's refresh and is not touched here. */
    function refresh() {
        focusEl('cmbgroup');
        show('btnsave', true);
        show('btnupdate', false);
        setGroupText('');
        setComboText('cmbitem', '');
        $('txtminvalue').value = '';
        $('txtmaxvalue').value = '';
    }

    function payload(id) {
        return {
            id: id,
            groupId: comboVal('cmbgroup'),                              // Conversion.ToInt(cmbgroup.Value)
            itemId: comboVal('cmbitem'),                                // Conversion.ToInt(cmbitem.Value)
            minValue: $('txtminvalue').value,
            maxValue: $('txtmaxvalue').value,
            editableAfterApproval: $('chkEditableAfterApproval').checked
        };
    }

    /** btnsave_Click (:208) - Id is not set, so the BLL inserts. */
    function btnsave_Click() {
        return act($('btnsave'), async function () {
            if (!formvalidation()) return;
            var res = await api('POST', API + '/save', payload(0));
            alert(res.message);                                         // "Save Successfully"
            gridfill(await api('GET', API + '/rows'));
            refresh();
        });
    }
    /** btnupdate_Click (:244) - Id = RecId. */
    function btnupdate_Click() {
        return act($('btnupdate'), async function () {
            if (!formvalidation()) return;
            var res = await api('POST', API + '/save', payload(st.recId));
            alert(res.message);                                         // "Update Successfully"
            gridfill(await api('GET', API + '/rows'));
            refresh();
        });
    }
    /** btnnew_Click (:399) */
    function btnnew_Click() { return act($('btnnew'), async function () { refresh(); }); }

    /** btnRefresh_Click (:463): refresh(); buttons; gridfill(); ItemFill(); GroupFill(); focus. */
    function btnRefresh_Click() {
        return act($('btnRefresh'), async function () {
            refresh();
            show('btnupdate', false);
            show('btnsave', true);
            var d = await api('GET', API + '/refresh');
            (d.errors || []).forEach(function (m) { alert(m); });
            gridfill(d.rows);
            ItemFill(d.items);
            GroupFill(d.groups);
            focusEl('cmbgroup');
        });
    }

    /** grdfrm_DoubleClick (:366) - reads the row again by Id and loads the entry controls. */
    function grdfrm_DoubleClick() {
        var id = parseInt(st.selId, 10);
        if (!id) return;                                                // no current record row
        return act(null, async function () {
            var d = await api('GET', API + '/' + id);
            var r = d && d.row;
            if (!r) return;                                             // dtgetById.Rows.Count > 0 (:381)
            st.recId = id;
            setGroupText(str(r.AnalysisGroupDescription));
            setComboText('cmbitem', str(r.AnalysisParameterDescription));
            $('txtminvalue').value = str(r.MinValue);
            $('txtmaxvalue').value = str(r.MaxValue);
            $('chkEditableAfterApproval').checked = bool(r.IsEditableAfterApproval);
            show('btnsave', false);
            show('btnupdate', true);
        });
    }

    // ============================================================================ keys

    /** CommonServices.OnlytextdecimelFunction (CommonServices.cs:2475) - digits and one '.'. */
    function decimalGuard(el) {
        el.addEventListener('keypress', function (e) {
            if (e.ctrlKey || e.metaKey || e.altKey || !e.key || e.key.length !== 1) return;
            if (!/[0-9.]/.test(e.key)) { e.preventDefault(); return; }
            if (e.key === '.' && el.value.indexOf('.') > -1) e.preventDefault();
        });
    }
    /** SendKeys.Send("{TAB}") */
    function tabNext(from) {
        var list = Array.prototype.filter.call(
            document.querySelectorAll('.gas-page input, .gas-page select, .gas-page button, .gas-page [tabindex]'),
            function (el) { return !el.disabled && !el.hidden && el.offsetParent !== null && el.getAttribute('tabindex') !== '-1'; });
        var i = list.indexOf(from);
        if (i >= 0 && i + 1 < list.length) list[i + 1].focus();
    }
    /**
     * InvLabGroupAnalysisStandards_KeyDown (:422), KeyPreview. The desktop tests a field, UpdateMode,
     * that nothing on the form ever sets, so it is always false: Ctrl+S always runs btnsave_Click and
     * Ctrl+U never runs anything. Reproduced as written.
     */
    function onKey(e) {
        if (e.defaultPrevented) return;
        var key = (e.key || '').toLowerCase(), ctrl = e.ctrlKey || e.metaKey;
        if (key === 'enter' && !ctrl && !e.altKey && !e.shiftKey) {
            var t = e.target;
            if (t && t.tagName !== 'BUTTON' && t.tagName !== 'A') { e.preventDefault(); tabNext(t); }
            return;
        }
        if (ctrl && key === 's') { e.preventDefault(); btnsave_Click(); return; }
        if (ctrl && key === 'n') { e.preventDefault(); btnnew_Click(); return; }
        if ((ctrl && key === 'e') || key === 'escape') { e.preventDefault(); window.location.href = EXIT_URL; return; }
        if (ctrl && key === 'u') { e.preventDefault(); }                // UpdateMode is never true on the desktop
    }

    function wire() {
        $('btnnew').addEventListener('click', btnnew_Click);
        $('btnRefresh').addEventListener('click', btnRefresh_Click);
        $('btnupdate').addEventListener('click', btnupdate_Click);
        $('btnsave').addEventListener('click', btnsave_Click);
        $('cmbgroup').addEventListener('change', cmbgroup_TextChanged);
        decimalGuard($('txtminvalue'));
        decimalGuard($('txtmaxvalue'));

        var t = $('grdfrm');
        t.addEventListener('input', function (e) {
            var f = e.target.getAttribute && e.target.getAttribute('data-f');
            if (!f) return;
            st.filter[f] = e.target.value;
            renderGrid();
        });
        t.addEventListener('click', function (e) {
            var g = e.target.closest('tr.grp');
            if (g) { var name = g.getAttribute('data-g'); st.collapsed[name] = !st.collapsed[name]; renderGrid(); return; }
            var tr = e.target.closest('tr[data-id]');
            if (tr) {
                selectRow(tr.getAttribute('data-id'));
                if (e.target.closest('a.lab-open')) grdfrm_DoubleClick();
            }
        });
        t.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tr[data-id]');
            if (!tr) return;
            selectRow(tr.getAttribute('data-id'));
            grdfrm_DoubleClick();
        });
        /* grdfrm_KeyDown (:545) - Ctrl+Enter on a record row is the double click. */
        $('grdfrmWrap').addEventListener('keydown', function (e) {
            if (e.target.tagName === 'INPUT') return;
            if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); e.stopPropagation(); grdfrm_DoubleClick(); return; }
            if (e.key !== 'ArrowDown' && e.key !== 'ArrowUp') return;
            var ids = Array.prototype.map.call(t.querySelectorAll('tr[data-id]'), function (tr) { return tr.getAttribute('data-id'); });
            if (!ids.length) return;
            var i = ids.indexOf(String(st.selId));
            i = e.key === 'ArrowDown' ? Math.min(ids.length - 1, i + 1) : Math.max(0, i - 1);
            e.preventDefault();
            selectRow(ids[i]);
        });
        document.addEventListener('keydown', onKey);
    }

    // ============================================================================ load

    /** InvLabAnalysisGroup_Load (:349): gridfill(); GroupFill(); ItemFill(); Update hidden, Save shown; focus. */
    async function init() {
        wire();
        try {
            L = await api('GET', API + '/lookups');
        } catch (e) {
            $('rightsNote').textContent = msg(e);
            $('rightsNote').hidden = false;
            document.querySelectorAll('.wf-root button, .wf-root input, .wf-root select').forEach(function (el) { el.disabled = true; });
            return;
        }
        var r = rights();
        $('btnsave').disabled = !r.save;
        $('btnupdate').disabled = !r.update;
        if (!r.save) $('btnsave').title = 'You do not have the Save right for this screen.';
        if (!r.update) $('btnupdate').title = 'You do not have the Update right for this screen.';
        (L.errors || []).forEach(function (m) { alert(m); });      // each desktop fill has its own catch -> MessageBox
        gridfill(L.rows);
        GroupFill(L.groups);
        ItemFill(L.items);
        show('btnupdate', false);
        show('btnsave', true);
        focusEl('cmbgroup');
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();
