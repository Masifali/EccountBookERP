/* ============================================================================================
 * Screen 157 "Analysis Group" — Architecture.WinApp.Lab/InvLabAnalysisGroup.cs.
 * Line references (:n) are InvLabAnalysisGroup.cs.
 *
 *   formvalidation             :81-102    three refusals, desktop order and text
 *   btnsave_Click              :104-133   validate, "Are you sure to Save?", Save, gridfill, refresh
 *   btnupdate_Click            :135-166   the same with Id = RecId and "Are you sure to Update?"
 *   gridfill / grdfrmSetting   :168-218   Id | GroupType | GroupCode | Description, grouped by GroupType
 *   btnnew_Click               :220-223   refresh()
 *   grdfrm_DoubleClick         :225-252   open the row, Update visible / Save hidden
 *   refresh                    :254-263
 *   InvLabAnalysisGroup_Load_1 :283-297   GroupTypeFill, gridfill, focus CmbGroupType
 *   GroupTypeFill / Bind       :299-338
 *   InvLabAnalysisGroup_KeyDown:340-382   Enter = Tab, Ctrl+S / N / E / U / Up / Down, Esc
 *   grdfrm_KeyDown             :401-424   Ctrl+Enter opens the current row
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/lab/analysis-group';
    var EXIT = '/quality';

    /* gridfill :182-185 — DataTable column names are the grid captions (RetrieveStructure, :193).
       Id is hidden (:211); GroupType is the group (:206) and hidden while grouped (:207). */
    var COLS = [['GroupCode', 200], ['Description', 200]];

    var st = { rows: [], recId: 0, updateMode: false, sel: null, filter: {}, collapsed: {}, sort: null, sortDir: 1 };

    function $(id) { return document.getElementById(id); }
    function esc(v) {
        return String(v === undefined || v === null ? '' : v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function text(v) { return v === undefined || v === null ? '' : String(v); }
    function intOf(v) { var n = parseInt(text(v), 10); return isNaN(n) ? 0 : n; }
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
        if (!res.ok) throw new Error(data && (data.message || data.error) || ('The request failed (' + res.status + ').'));
        return data;
    }
    /** Busy panel + button lock (countx_purchase_request.js); a refusal is shown as the desktop's MessageBox. */
    function act(button, work) {
        /* Rule 5 (countx_lab_page_kit.js): the button is disabled at once, carries a spinner (btn-busy), ignores
           further clicks while the request is in flight and is re-enabled in finally - success or failure. */
        if (window.LabKit) return window.LabKit.act(button || null, work);
        var runner = window.PurchaseRequest ? window.PurchaseRequest.run.bind(window.PurchaseRequest) : function (b, w) { return w(); };
        return runner(button || null, async function () {
            try { await work(); } catch (e) { alert(msg(e)); }
        });
    }

    // ============================================================================ combo

    /** GroupTypeBind (:328-338): value member "Id", display member "Name". */
    function bindGroupType(rows) {
        var el = $('CmbGroupType'), html = '';
        (rows || []).forEach(function (r) { html += '<option value="' + esc(r.Id) + '">' + esc(r.Name) + '</option>'; });
        el.innerHTML = html;
        /* InfragisticsHelper.BindAndRetainSelection(..., previousValue = Conversion.ToInt(CmbGroupType.Value))
           — 0 at load, which matches no row, so RetainComboSelection activates row 0
           (InfragisticsHelper.cs:125-128). An empty list leaves the combo empty (:17-22). */
        el.selectedIndex = el.options.length ? 0 : -1;
    }
    /** CmbGroupType.Value = v — a value that is not in the list leaves no selection. */
    function setGroupType(v) {
        var el = $('CmbGroupType'), s = String(v), i;
        for (i = 0; i < el.options.length; i++) { if (el.options[i].value === s) { el.selectedIndex = i; return; } }
        el.selectedIndex = -1;
    }
    function groupTypeText() { var el = $('CmbGroupType'); return el.selectedIndex < 0 ? '' : el.options[el.selectedIndex].text; }
    function groupTypeValue() { var el = $('CmbGroupType'); return el.selectedIndex < 0 ? 0 : intOf(el.value); }

    // ============================================================================ grid

    function visibleRows() {
        var rows = st.rows.filter(function (r) {
            return COLS.every(function (c) {
                var f = st.filter[c[0]];
                return !f || text(r[c[0]]).toLowerCase().indexOf(f.toLowerCase()) >= 0;
            });
        });
        if (st.sort) {
            var k = st.sort, d = st.sortDir;
            rows = rows.slice().sort(function (a, b) { return text(a[k]).localeCompare(text(b[k])) * d; });
        }
        return rows;
    }
    /** RootTable.Groups.Add("GroupType") (:206) — rows under their group, groups in ascending order. */
    function grouped(rows) {
        var map = {}, keys = [];
        rows.forEach(function (r) {
            var k = text(r.GroupType);
            if (!map[k]) { map[k] = []; keys.push(k); }
            map[k].push(r);
        });
        keys.sort(function (a, b) { return a.localeCompare(b); });
        return keys.map(function (k) { return { key: k, rows: map[k] }; });
    }
    function renderGrid() {
        var t = $('grdfrm');
        t.tHead.innerHTML = '<tr><th class="ind"></th>' + COLS.map(function (c) {
            var mark = st.sort === c[0] ? (st.sortDir > 0 ? ' ▲' : ' ▼') : '';
            return '<th data-col="' + c[0] + '" style="width:' + c[1] + 'px;min-width:' + c[1] + 'px;">' + esc(c[0]) + mark + '</th>';
        }).join('') + '</tr><tr class="flt"><td class="ind"></td>' + COLS.map(function (c) {
            return '<td><input type="text" data-f="' + c[0] + '" value="' + esc(st.filter[c[0]] || '') + '"></td>';
        }).join('') + '</tr>';
        var rows = visibleRows(), html = '';
        grouped(rows).forEach(function (g) {
            var closed = !!st.collapsed[g.key];
            html += '<tr class="grp" data-grp="' + esc(g.key) + '"><td colspan="' + (COLS.length + 1) + '"><span class="tw">'
                + (closed ? '+' : '−') + '</span>GroupType: ' + esc(g.key) + '</td></tr>';
            if (closed) return;
            g.rows.forEach(function (r) {
                html += '<tr data-id="' + esc(r.Id) + '"' + (String(r.Id) === String(st.sel) ? ' class="is-sel"' : '') + '><td class="ind"></td>'
                    + COLS.map(function (c) {
                        /* Rule 4: the code column is a link that opens the record (= grdfrm_DoubleClick). */
                        if (c[0] === 'GroupCode') return '<td><a class="lab-open" data-open="1" title="Open this record">' + esc(r[c[0]]) + '</a></td>';
                        return '<td>' + esc(r[c[0]]) + '</td>';
                    }).join('') + '</tr>';
            });
        });
        t.tBodies[0].innerHTML = html;
        /* TotalRow = True (:549) with no aggregate defined on any column: an empty total row. */
        t.tFoot.innerHTML = '<tr><td class="ind"></td>' + COLS.map(function () { return '<td></td>'; }).join('') + '</tr>';
        renderNav();
    }
    function dataTrs() { return Array.prototype.slice.call($('grdfrm').tBodies[0].querySelectorAll('tr[data-id]')); }
    /** RecordNavigator = True (:546). */
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

    /** gridfill (:168-200) */
    async function gridfill() {
        st.rows = await api('GET', API + '/grid') || [];
        renderGrid();
    }

    // ============================================================================ form

    /** refresh (:254-263) */
    function refresh() {
        show('btnupdate', false);
        show('btnsave', true);
        st.updateMode = false;
        focus('txtcode');
        $('txtcode').value = '';
        $('CmbGroupType').selectedIndex = -1;          // CmbGroupType.Text = string.Empty
        $('txtdescription').value = '';
    }

    /** formvalidation (:81-102) */
    function formvalidation() {
        if (groupTypeText().trim() === '') { alert('Please select Group type'); focus('CmbGroupType'); return false; }
        if ($('txtcode').value.trim() === '') { alert('Please Insert Code'); focus('txtcode'); return false; }
        if ($('txtdescription').value.trim() === '') { alert('Please Insert Description'); focus('txtdescription'); return false; }
        return true;
    }

    function post(id) {
        return api('POST', API + '/save', {
            id: id,
            groupType: groupTypeValue(),                         // Conversion.ToInt(CmbGroupType.Value)
            analysisGroupCode: $('txtcode').value,               // Conversion.ToString(txtcode.Text) — as typed
            analysisGroupDescription: $('txtdescription').value
        });
    }

    /** btnsave_Click (:104-133) */
    function btnsave_Click() {
        if (!formvalidation()) return;
        if (!confirm('Are you sure to Save?')) return;
        return act($('btnsave'), async function () {
            await post(0);
            alert('Save Successfully');
            await gridfill();
            refresh();
        });
    }

    /** btnupdate_Click (:135-166) */
    function btnupdate_Click() {
        if (!formvalidation()) return;
        if (!confirm('Are you sure to Update?')) return;
        return act($('btnupdate'), async function () {
            await post(st.recId);
            alert('Update Successfully');
            await gridfill();
            refresh();
        });
    }

    /** grdfrm_DoubleClick (:225-252) */
    function grdfrm_DoubleClick(id) {
        id = intOf(id);
        if (!id) return;
        return act(null, async function () {
            show('btnupdate', true);
            show('btnsave', false);
            st.recId = id;
            var r = await api('GET', API + '/' + id);
            if (r && r.found) {
                setGroupType(intOf(r.GroupType));
                $('txtcode').value = text(r.AnalysisGroupCode);
                $('txtdescription').value = text(r.AnalysisGroupDescription);
                st.updateMode = true;
                focus('CmbGroupType');
            }
        });
    }

    // ============================================================================ keys

    function comboOpen(el) { return !!el && el.getAttribute && el.getAttribute('aria-expanded') === 'true'; }
    /** Enter = SendKeys("{TAB}") (:344) — TabIndex order CmbGroupType 0, txtcode 1, txtdescription 2, then the grid. */
    function tabNext(from) {
        var order = [($('CmbGroupType').__dtcombo && $('CmbGroupType').__dtcombo.input) || $('CmbGroupType'), $('txtcode'), $('txtdescription'), $('grdfrmWrap')];
        var i = order.indexOf(from);
        if (i < 0) return false;
        order[(i + 1) % order.length].focus();
        return true;
    }
    function onKey(e) {
        var k = e.key, ctrl = e.ctrlKey;
        if (k === 'Enter' && !ctrl && !e.altKey && !e.shiftKey) {
            if (comboOpen(e.target)) return;                       // the open drop-down takes Enter to pick a row
            if (tabNext(e.target)) e.preventDefault();
            return;
        }
        if (ctrl && (k === 's' || k === 'S')) { e.preventDefault(); if (!st.updateMode) btnsave_Click(); return; }
        if (ctrl && (k === 'n' || k === 'N')) { e.preventDefault(); refresh(); return; }
        if ((ctrl && (k === 'e' || k === 'E')) || k === 'Escape') {
            if (k === 'Escape' && e.defaultPrevented) return;      // Esc just closed an open drop-down
            e.preventDefault(); window.location.href = EXIT; return;   // Close()
        }
        if (ctrl && (k === 'u' || k === 'U')) { e.preventDefault(); if (st.updateMode) btnupdate_Click(); return; }
        if (ctrl && k === 'ArrowUp') { e.preventDefault(); focus('CmbGroupType'); return; }
        if (ctrl && k === 'ArrowDown') {
            e.preventDefault();
            var c = $('CmbGroupType').__dtcombo;
            if (c && c.open) c.closePop();
            $('grdfrmWrap').focus();
        }
    }
    /** grdfrm_KeyDown (:401-424) */
    function onGridKey(e) {
        if (e.target && e.target.tagName === 'INPUT') return;      // the filter row
        if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); e.stopPropagation(); if (st.sel !== null) grdfrm_DoubleClick(st.sel); return; }
        if (e.ctrlKey) return;
        if (e.key === 'ArrowDown') { e.preventDefault(); move(1); }
        else if (e.key === 'ArrowUp') { e.preventDefault(); move(-1); }
        else if (e.key === 'Home') { e.preventDefault(); move('first'); }
        else if (e.key === 'End') { e.preventDefault(); move('last'); }
    }

    function wire() {
        $('btnnew').addEventListener('click', function () { act($('btnnew'), async function () { refresh(); }); });   // btnnew_Click (:220-223)
        $('btnsave').addEventListener('click', btnsave_Click);
        $('btnupdate').addEventListener('click', btnupdate_Click);
        var t = $('grdfrm');
        t.addEventListener('click', function (e) {
            var th = e.target.closest('th[data-col]');
            if (th) {
                var c = th.getAttribute('data-col');
                if (st.sort === c) st.sortDir = -st.sortDir; else { st.sort = c; st.sortDir = 1; }
                renderGrid(); return;
            }
            var g = e.target.closest('tr.grp');
            if (g) { var k = g.getAttribute('data-grp'); st.collapsed[k] = !st.collapsed[k]; renderGrid(); return; }
            var tr = e.target.closest('tr[data-id]');
            if (tr) {
                select(tr.getAttribute('data-id'), false);
                if (e.target.closest('a.lab-open')) grdfrm_DoubleClick(tr.getAttribute('data-id'));
            }
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

    /** InvLabAnalysisGroup_Load_1 (:283-297) */
    async function init() {
        wire();
        var L;
        try {
            L = await api('GET', API + '/init');
        } catch (e) {
            $('rightsNote').textContent = msg(e);
            $('rightsNote').hidden = false;
            document.querySelectorAll('.wf-root button, .wf-root input, .wf-root select').forEach(function (el) { el.disabled = true; });
            return;
        }
        show('btnupdate', false);
        show('btnsave', true);
        bindGroupType(L.groupTypes);          // GroupTypeFill()
        st.rows = L.rows || [];               // gridfill()
        renderGrid();
        focus('CmbGroupType');
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
}());
