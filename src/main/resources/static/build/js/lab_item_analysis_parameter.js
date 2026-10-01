/* ============================================================================================
 * Screen 156 "Item Analysis Parameter" — Architecture.WinApp.Lab/InvLabAnalysisItems.cs.
 * Line references (:n) are InvLabAnalysisItems.cs.
 *
 *   formvalidation             :88-103    two refusals, desktop order and text
 *   refresh                    :105-126   clear the fields, Save visible / Update hidden, gridfill
 *   Insert                     :128-182   one save path for both buttons; RecId decides which
 *   btnsave_Click              :184-195   RecId = 0, then Insert()
 *   btnupdate_Click            :197-207   Insert()
 *   btnnew_Click               :209-212   refresh()
 *   gridfill / grdfrmSetting   :214-271   grid + the Parent Parameter combo bound from the same table
 *   grdfrm_DoubleClick         :273-303   open the row, Save hidden / Update visible
 *   MasterParameters           :305-319   usp_getLabMasterParms
 *   InvLabAnalysisItems_Load   :339-355
 *   InvLabAnalysisItems_KeyDown:362-396   Enter = Tab, Ctrl+S / N / E / U, Esc
 *   ChkIsSub_CheckedChanged    :398-417   show / hide label6 + CmbParentParameter
 *   grdfrm_KeyDown             :431-454   Ctrl+Enter opens the current row
 *   txtMin/MaxValue_KeyPress   :456-478   CommonServices.OnlytextdecimelFunction (CommonServices.cs:2475)
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/lab/item-analysis-parameter';
    var EXIT = '/quality';

    /* gridfill :228-234 — the DataTable column names are the grid captions (RetrieveStructure, :244).
       Id and MasterParId are hidden (:259-260). Every column is a string column (Columns.Add(name)),
       so the numbers are shown as text, left-aligned. Widths :261-265. */
    var COLS = [['AnalysisParameter', 250, 'analysisParameter'], ['ParentParameter', 250, 'parentParameter'],
                ['MasterParameter', 250, 'masterParameter'], ['MinValue', 80, 'minValue'], ['MaxValue', 80, 'maxValue']];

    var st = { rows: [], recId: 0, updateMode: false, sel: null, filter: {}, sort: null, sortDir: 1 };

    function $(id) { return document.getElementById(id); }
    function esc(v) {
        return String(v === undefined || v === null ? '' : v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function text(v) { return v === undefined || v === null ? '' : String(v); }
    /** Conversion.ToInt — anything unparsable is 0. */
    function intOf(v) { var n = parseInt(text(v), 10); return isNaN(n) ? 0 : n; }
    /** Conversion.ToDouble — "" and anything unparsable is 0. */
    function num(v) {
        var s = text(v).replace(/,/g, '').trim();
        if (s === '') return 0;
        var n = Number(s);
        return isFinite(n) ? n : 0;
    }
    function msg(e) { return e && e.message ? e.message : String(e); }
    function show(id, on) { $(id).hidden = !on; }
    function focus(id) { var el = $(id); if (!el) return; var w = el.__dtcombo && el.__dtcombo.input; (w || el).focus(); }

    async function api(method, url, body) {
        var opt = { method: method, credentials: 'same-origin', headers: { Accept: 'application/json' } };
        if (body !== undefined) { opt.headers['Content-Type'] = 'application/json'; opt.body = JSON.stringify(body); }
        var res = await fetch(url, opt);
        var t = await res.text();
        var data = null;
        try { data = t ? JSON.parse(t) : null; } catch (e) { data = null; }
        if (res.status === 404) return null;
        if (!res.ok) throw new Error(data && (data.message || data.error) || ('The request failed (' + res.status + ').'));
        return data;
    }
    /** Busy panel + button lock (countx_purchase_request.js); a refusal is shown as the desktop's MessageBox. */
    function act(button, work) {
        var runner = window.PurchaseRequest ? window.PurchaseRequest.run.bind(window.PurchaseRequest) : function (b, w) { return w(); };
        return runner(button || null, async function () {
            try { await work(); } catch (e) { alert(msg(e)); }
        });
    }

    // ============================================================================ combos

    /** DropDownBind.BindDDL / BindDDLNew with ZeroIndex false: no default row and no selection. */
    function bind(id, rows, valueKey, textKey) {
        var el = $(id), html = '';
        (rows || []).forEach(function (r) { html += '<option value="' + esc(r[valueKey]) + '">' + esc(r[textKey]) + '</option>'; });
        el.innerHTML = html;
        el.selectedIndex = -1;
    }
    /** combo.Value = v — a value that is not in the list leaves no selection. */
    function setVal(id, v) {
        var el = $(id), s = String(v), i;
        for (i = 0; i < el.options.length; i++) { if (el.options[i].value === s) { el.selectedIndex = i; return; } }
        el.selectedIndex = -1;
    }
    function comboVal(id) { var el = $(id); return el.selectedIndex < 0 ? 0 : intOf(el.value); }
    function clearCombo(id) { $(id).selectedIndex = -1; }               // combo.Text = string.Empty

    /** ChkIsSub_CheckedChanged (:398-417) */
    function ChkIsSub_CheckedChanged() {
        var on = $('ChkIsSub').checked;
        show('CmbParentParameterBox', on);
        show('label6', on);
    }

    // ============================================================================ grid

    function visibleRows() {
        var rows = st.rows.filter(function (r) {
            return COLS.every(function (c) {
                var f = st.filter[c[0]];
                return !f || text(r[c[2]]).toLowerCase().indexOf(f.toLowerCase()) >= 0;
            });
        });
        if (st.sort) {
            var k = st.sort, d = st.sortDir;
            rows = rows.slice().sort(function (a, b) { return text(a[k]).localeCompare(text(b[k])) * d; });   // string columns
        }
        return rows;
    }
    function renderGrid() {
        var t = $('grdfrm');
        t.tHead.innerHTML = '<tr><th class="ind"></th>' + COLS.map(function (c) {
            var mark = st.sort === c[2] ? (st.sortDir > 0 ? ' ▲' : ' ▼') : '';
            return '<th data-col="' + c[2] + '" style="width:' + c[1] + 'px;min-width:' + c[1] + 'px;">' + esc(c[0]) + mark + '</th>';
        }).join('') + '</tr><tr class="flt"><td class="ind"></td>' + COLS.map(function (c) {
            return '<td><input type="text" data-f="' + c[0] + '" value="' + esc(st.filter[c[0]] || '') + '"></td>';
        }).join('') + '</tr>';
        t.tBodies[0].innerHTML = visibleRows().map(function (r) {
            return '<tr data-id="' + esc(r.id) + '"' + (String(r.id) === String(st.sel) ? ' class="is-sel"' : '') + '><td class="ind"></td>'
                + COLS.map(function (c) { return '<td>' + esc(r[c[2]]) + '</td>'; }).join('') + '</tr>';
        }).join('');
        /* TotalRow = True (:632) with no aggregate defined on any column: an empty total row. */
        t.tFoot.innerHTML = '<tr><td class="ind"></td>' + COLS.map(function () { return '<td></td>'; }).join('') + '</tr>';
        renderNav();
    }
    function dataTrs() { return Array.prototype.slice.call($('grdfrm').tBodies[0].querySelectorAll('tr[data-id]')); }
    /** RecordNavigator = True (:629). */
    function renderNav() {
        var trs = dataTrs(), pos = 0;
        trs.forEach(function (tr, i) { if (tr.getAttribute('data-id') === String(st.sel)) pos = i + 1; });
        $('navPos').value = pos ? pos : '';
        $('navCount').textContent = 'of ' + trs.length;
    }
    function select(id, scroll) {
        st.sel = id;
        dataTrs().forEach(function (tr) {
            var on = tr.getAttribute('data-id') === String(id);
            tr.classList.toggle('is-sel', on);
            if (on && scroll && tr.scrollIntoView) tr.scrollIntoView({ block: 'nearest' });
        });
        renderNav();
    }
    function move(where) {
        var trs = dataTrs();
        if (!trs.length) return;
        var i = -1;
        trs.forEach(function (tr, n) { if (tr.getAttribute('data-id') === String(st.sel)) i = n; });
        if (where === 'first') i = 0;
        else if (where === 'last') i = trs.length - 1;
        else i = Math.max(0, Math.min(trs.length - 1, i + where));
        select(trs[i].getAttribute('data-id'), true);
    }

    /** gridfill (:214-251) */
    async function gridfill() {
        st.rows = await api('GET', API + '/grid') || [];
        /* :239-242 — the Parent Parameter combo is bound from the grid's own table ("Id" /
           "AnalysisParameter"), and only when it has rows. */
        if (st.rows.length > 0) bind('CmbParentParameter', st.rows, 'id', 'analysisParameter');
        renderGrid();
    }

    /** MasterParameters (:305-319) — bound only when the procedure returns rows. */
    async function MasterParameters() {
        var rows = await api('GET', API + '/master-parameters') || [];
        if (rows.length > 0) bind('CmbMasterParameter', rows, 'id', 'description');
    }

    // ============================================================================ form

    /** refresh (:105-126) */
    async function refresh() {
        show('btnsave', true);
        show('btnupdate', false);
        /* The desktop leaves UpdateMode (and RecId) set here, so after one row was opened Ctrl+S is
           dead and Ctrl+U would overwrite the previously opened row with the new entry. That defect is
           NOT reproduced: the shortcuts follow the visible button, as on form 157 (refresh resets
           UpdateMode there). Listed in the report under "desktop quirks". */
        st.updateMode = false;
        $('txtdescription').value = '';
        clearCombo('CmbParentParameter');
        clearCombo('CmbMasterParameter');
        $('txtMinValue').value = '';
        $('txtMaxValue').value = '';
        $('ChkIsSub').checked = false;
        ChkIsSub_CheckedChanged();                  // :117-118 Visible = false
        try { await gridfill(); } catch (e) { alert(msg(e)); }
        focus('txtdescription');
    }

    /** formvalidation (:88-103) */
    function formvalidation() {
        if ($('txtdescription').value.trim() === '') { alert('Please Insert Description'); focus('txtdescription'); return false; }
        if ($('ChkIsSub').checked && $('CmbParentParameter').selectedIndex < 0) {       // ActiveRow == null
            alert('Parent Parameter Field is Required'); focus('CmbParentParameter'); return false;
        }
        return true;
    }

    /** Insert (:128-182) */
    function Insert(button) {
        if (!formvalidation()) return;
        var isUpdate = st.recId > 0;
        if (!confirm(isUpdate ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;      // :141 / :147
        var isSub = $('ChkIsSub').checked;
        var body = {
            id: st.recId,
            description: $('txtdescription').value,                          // :153-154 — Code and Description, as typed
            isSub: isSub,                                                    // :155
            masterParId: comboVal('CmbMasterParameter'),                     // :156
            minValue: num($('txtMinValue').value),                           // :157
            maxValue: num($('txtMaxValue').value),                           // :158
            parentParameterId: isSub ? comboVal('CmbParentParameter') : 0    // :159-166
        };
        return act(button, async function () {
            await api('POST', API + '/save', body);
            alert(isUpdate ? 'Update Successfully' : 'Save Successfully');   // :168-175
            await refresh();
        });
    }
    function btnsave_Click() { st.recId = 0; return Insert($('btnsave')); }  // :184-195
    function btnupdate_Click() { return Insert($('btnupdate')); }            // :197-207
    function btnnew_Click() { return refresh(); }                            // :209-212

    /** grdfrm_DoubleClick (:273-303) */
    function grdfrm_DoubleClick(id) {
        id = intOf(id);
        if (!id) return;
        return act(null, async function () {
            show('btnsave', false);
            show('btnupdate', true);
            st.recId = id;
            var r = await api('GET', API + '/' + id);
            if (r) {
                $('txtdescription').value = text(r.description);
                setVal('CmbParentParameter', intOf(r.parentParameterId));
                setVal('CmbMasterParameter', intOf(r.masterParId));
                $('ChkIsSub').checked = r.isSub === true;
                ChkIsSub_CheckedChanged();
                $('txtMinValue').value = text(r.minValue);
                $('txtMaxValue').value = text(r.maxValue);
                st.updateMode = true;
            }
        });
    }

    // ============================================================================ keys

    function comboInput(id) { var el = $(id); return (el.__dtcombo && el.__dtcombo.input) || el; }
    /** Enter = SendKeys("{TAB}") (:366) — TabIndex order: CmbMasterParameter 0, txtdescription 1, ChkIsSub 2,
        txtMinValue 3, txtMaxValue 4, CmbParentParameter 5 (when visible), then the grid. */
    function tabNext(from) {
        var order = [comboInput('CmbMasterParameter'), $('txtdescription'), $('ChkIsSub'), $('txtMinValue'), $('txtMaxValue')];
        if ($('ChkIsSub').checked) order.push(comboInput('CmbParentParameter'));
        order.push($('grdfrmWrap'));
        var i = order.indexOf(from);
        if (i < 0) return false;
        order[(i + 1) % order.length].focus();
        return true;
    }
    function onKey(e) {
        var k = e.key, ctrl = e.ctrlKey;
        if (k === 'Enter' && !ctrl && !e.altKey && !e.shiftKey) {
            if (tabNext(e.target)) e.preventDefault();             // an open drop-down stops the event itself
            return;
        }
        if (ctrl && (k === 's' || k === 'S')) { e.preventDefault(); if (!st.updateMode) btnsave_Click(); return; }
        if (ctrl && (k === 'n' || k === 'N')) { e.preventDefault(); btnnew_Click(); return; }
        if ((ctrl && (k === 'e' || k === 'E')) || k === 'Escape') {
            if (k === 'Escape' && e.defaultPrevented) return;      // Esc just closed an open drop-down
            e.preventDefault(); window.location.href = EXIT; return;   // Close()
        }
        if (ctrl && (k === 'u' || k === 'U')) { e.preventDefault(); if (st.updateMode) btnupdate_Click(); }
    }
    /** grdfrm_KeyDown (:431-454) */
    function onGridKey(e) {
        if (e.target && e.target.tagName === 'INPUT') return;      // the filter row
        if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); e.stopPropagation(); if (st.sel !== null) grdfrm_DoubleClick(st.sel); return; }
        if (e.ctrlKey) return;
        if (e.key === 'ArrowDown') { e.preventDefault(); move(1); }
        else if (e.key === 'ArrowUp') { e.preventDefault(); move(-1); }
        else if (e.key === 'Home') { e.preventDefault(); move('first'); }
        else if (e.key === 'End') { e.preventDefault(); move('last'); }
    }
    /** CommonServices.OnlytextdecimelFunction (CommonServices.cs:2475-2492): digits, control keys, one '.'. */
    function decimalGuard(e) {
        if (e.ctrlKey || e.metaKey || e.altKey || !e.key || e.key.length !== 1) return;
        if (e.key === '.') { if (e.target.value.indexOf('.') > -1) e.preventDefault(); return; }
        if (!/^[0-9]$/.test(e.key)) e.preventDefault();
    }

    function wire() {
        $('btnnew').addEventListener('click', btnnew_Click);
        $('btnsave').addEventListener('click', btnsave_Click);
        $('btnupdate').addEventListener('click', btnupdate_Click);
        $('ChkIsSub').addEventListener('change', ChkIsSub_CheckedChanged);
        $('txtMinValue').addEventListener('keydown', decimalGuard);
        $('txtMaxValue').addEventListener('keydown', decimalGuard);
        var t = $('grdfrm');
        t.addEventListener('click', function (e) {
            var th = e.target.closest('th[data-col]');
            if (th) {
                var c = th.getAttribute('data-col');
                if (st.sort === c) st.sortDir = -st.sortDir; else { st.sort = c; st.sortDir = 1; }
                renderGrid(); return;
            }
            var tr = e.target.closest('tr[data-id]');
            if (tr) select(tr.getAttribute('data-id'), false);
        });
        t.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tr[data-id]');
            if (tr) { select(tr.getAttribute('data-id'), false); grdfrm_DoubleClick(tr.getAttribute('data-id')); }
        });
        t.addEventListener('input', function (e) {
            var f = e.target.getAttribute && e.target.getAttribute('data-f');
            if (!f) return;
            st.filter[f] = e.target.value;
            var pos = e.target.selectionStart;
            renderGrid();
            var again = t.querySelector('input[data-f="' + f + '"]');
            if (again) { again.focus(); try { again.setSelectionRange(pos, pos); } catch (x) { } }
        });
        $('grdfrmWrap').addEventListener('keydown', onGridKey);
        $('navFirst').addEventListener('click', function () { move('first'); });
        $('navPrev').addEventListener('click', function () { move(-1); });
        $('navNext').addEventListener('click', function () { move(1); });
        $('navLast').addEventListener('click', function () { move('last'); });
        document.addEventListener('keydown', onKey);
    }

    /** InvLabAnalysisItems_Load (:339-355) */
    async function init() {
        wire();
        try {
            await api('GET', API + '/rights');                 // 403 without the View right
        } catch (e) {
            $('rightsNote').textContent = msg(e);
            $('rightsNote').hidden = false;
            document.querySelectorAll('.wf-root button, .wf-root input, .wf-root select').forEach(function (el) { el.disabled = true; });
            return;
        }
        $('ChkIsSub').checked = false;
        ChkIsSub_CheckedChanged();
        show('btnsave', true);
        show('btnupdate', false);
        try { await gridfill(); } catch (e) { alert(msg(e)); }
        try { await MasterParameters(); } catch (e) { alert(msg(e)); }
        focus('CmbMasterParameter');                           // TabIndex 0
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
}());
