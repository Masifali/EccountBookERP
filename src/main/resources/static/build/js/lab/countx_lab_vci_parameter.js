/* ============================================================================================
 * Screen 809 "VCI Parameter" — Architecture.WinApp.Lookups/ExImVCIParameter.cs.
 * Line references (:n) are ExImVCIParameter.cs.
 *
 *   ExImVCIParameter_Load       :75-92    Save visible / Update hidden, ParameterCategoryfill, FormHistory, focus combo
 *   ParameterCategoryfill       :94-129   combo rows + "...Select Any Value..." (BindDDLNew, ZeroIndex true), keeps the selection
 *   Insert                      :131-177  FormValidation, confirm, save, message, Reset
 *   btnsave_Click               :179-190  RecId = 0, Insert
 *   btnUpdate_Click             :192-202  Insert
 *   FormValidation              :204-220  two refusals, desktop order and text
 *   btnnew_Click                :221-232  Reset, then the combo text is cleared
 *   Reset                       :234-249  RecId 0, Save visible, description cleared, FormHistory, focus combo
 *   FormHistory / GrdSetting    :251-303  Id | CategoryDescription | ParameterDescription, grouped by CategoryDescription
 *   grdDetail_DoubleClick       :323-343  open the row, Update visible / Save hidden
 *   ExImVCIParameter_KeyDown    :345-386  Ctrl+N / S / U / E / Down / Up / F5 / R, Esc
 *   btnRefresh_Click            :400-410  ParameterCategoryfill
 *   btnVCICategoryDefine_Click  :412-422  opens the form ExImVCICategory
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/lab/vci-parameter';
    var EXIT = '/quality';
    var SELECT_ANY = '...Select Any Value...';

    /* FormHistory :265-267 — DataTable column names are the grid captions (RetrieveStructure, :273).
       Id is hidden (:295); CategoryDescription is the group (:292) and hidden while grouped (:293). */
    var COLS = [['ParameterDescription', 250]];

    var st = { rows: [], recId: 0, sel: null, collapsed: {}, sort: null, sortDir: 1 };

    function $(id) { return document.getElementById(id); }
    function esc(v) {
        return String(v === undefined || v === null ? '' : v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function text(v) { return v === undefined || v === null ? '' : String(v); }
    function intOf(v) { var n = parseInt(text(v), 10); return isNaN(n) ? 0 : n; }
    function msg(e) { return e && e.message ? e.message : String(e); }
    function show(id, on) { $(id).hidden = !on; }
    function visible(id) { return !$(id).hidden; }
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

    /**
     * Button lock: the button is disabled and shows a spinner (class btn-busy) from the click until its
     * request has finished — on success and on failure (finally). Further clicks while it is in flight
     * are ignored. A refusal is shown as the desktop shows it: MessageBox(ex.Message).
     */
    async function withBusy(btn, work) {
        if (btn) {
            if (btn.disabled || btn.classList.contains('btn-busy')) return;
            btn.disabled = true;
            btn.classList.add('btn-busy');
            btn.setAttribute('aria-busy', 'true');
        }
        try {
            return await work();
        } catch (e) {
            alert(msg(e));
        } finally {
            if (btn) {
                btn.disabled = false;
                btn.classList.remove('btn-busy');
                btn.removeAttribute('aria-busy');
            }
        }
    }

    // ============================================================================ combo

    function categoryValue() { var el = $('cmbvciCategory'); return el.selectedIndex < 0 ? 0 : intOf(el.value); }   // Conversion.ToInt(Value)
    function categoryActiveRow() { return $('cmbvciCategory').selectedIndex >= 0; }
    function setCategoryValue(v) {
        var el = $('cmbvciCategory'), s = String(v), i;
        for (i = 0; i < el.options.length; i++) { if (el.options[i].value === s) { el.selectedIndex = i; return true; } }
        el.selectedIndex = -1;
        return false;
    }
    /** cmbvciCategory.Text = t (LimitToList): the first row whose display text matches; nothing otherwise. */
    function setCategoryText(t) {
        var el = $('cmbvciCategory'), i;
        if (t !== '') {
            for (i = 0; i < el.options.length; i++) { if (el.options[i].text === t) { el.selectedIndex = i; return; } }
        }
        el.selectedIndex = -1;
    }

    /** ParameterCategoryfill (:94-129) + DropDownBind.BindDDLNew(dt, cmb, "Id", "VciCategoryDescription", "VciCategory", true). */
    function bindCategories(rows) {
        var el = $('cmbvciCategory');
        var vciId = categoryValue();                                   // :104
        rows = rows || [];
        if (rows.length > 0) {                                          // :105 — an empty table leaves the combo as it is
            var html = '';
            var hasZero = rows.some(function (r) { return text(r.VciCategoryDescription) === SELECT_ANY; });
            if (!hasZero) html += '<option value="0">' + esc(SELECT_ANY) + '</option>';
            rows.forEach(function (r) { html += '<option value="' + esc(r.Id) + '">' + esc(r.VciCategoryDescription) + '</option>'; });
            el.innerHTML = html;
            setCategoryValue(0);                                        // DDL.Value = 0
        }
        if (vciId > 0) {                                                // :110-120
            var listed = rows.some(function (r) { return intOf(r.Id) === vciId; });
            if (listed) setCategoryValue(vciId); else el.selectedIndex = -1;
        }
    }

    // ============================================================================ grid

    function sortedRows() {
        if (!st.sort) return st.rows;
        var k = st.sort, d = st.sortDir;
        return st.rows.slice().sort(function (a, b) { return text(a[k]).localeCompare(text(b[k])) * d; });
    }
    /** RootTable.Groups.Add("CategoryDescription") (:292) — rows under their group, groups in ascending order. */
    function grouped(rows) {
        var map = {}, keys = [];
        rows.forEach(function (r) {
            var k = text(r.CategoryDescription);
            if (!map[k]) { map[k] = []; keys.push(k); }
            map[k].push(r);
        });
        keys.sort(function (a, b) { return a.localeCompare(b); });
        return keys.map(function (k) { return { key: k, rows: map[k] }; });
    }
    function renderGrid() {
        var t = $('grdDetail');
        t.tHead.innerHTML = '<tr><th class="ind"></th>' + COLS.map(function (c) {
            var mark = st.sort === c[0] ? (st.sortDir > 0 ? ' ▲' : ' ▼') : '';
            return '<th data-col="' + c[0] + '" style="min-width:' + c[1] + 'px;">' + esc(c[0]) + mark + '</th>';
        }).join('') + '</tr>';
        var html = '';
        grouped(sortedRows()).forEach(function (g) {
            var closed = !!st.collapsed[g.key];
            html += '<tr class="grp" data-grp="' + esc(g.key) + '"><td colspan="' + (COLS.length + 1) + '"><span class="tw">'
                + (closed ? '+' : '−') + '</span>CategoryDescription: ' + esc(g.key) + ' (' + g.rows.length + ')</td></tr>';
            if (closed) return;
            g.rows.forEach(function (r) {
                /* The parameter name is the record's link: a click loads the row into the form (grdDetail_DoubleClick). */
                html += '<tr data-id="' + esc(r.Id) + '"' + (String(r.Id) === String(st.sel) ? ' class="is-sel"' : '') + '><td class="ind"></td>'
                    + '<td><a href="#" class="lnk" data-open="' + esc(r.Id) + '">' + esc(r.ParameterDescription) + '</a></td></tr>';
            });
        });
        t.tBodies[0].innerHTML = html;
        /* TotalRow = True with no aggregate defined on any column: an empty total row. */
        t.tFoot.innerHTML = st.rows.length ? '<tr><td class="ind"></td>' + COLS.map(function () { return '<td></td>'; }).join('') + '</tr>' : '';
        renderNav();
    }
    function dataTrs() { return Array.prototype.slice.call($('grdDetail').tBodies[0].querySelectorAll('tr[data-id]')); }
    /** RecordNavigator = True. */
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
    function rowById(id) {
        var s = String(id), i;
        for (i = 0; i < st.rows.length; i++) { if (String(st.rows[i].Id) === s) return st.rows[i]; }
        return null;
    }

    /**
     * FormHistory (:251-286): ExImVCICartegoryId = Conversion.ToInt(cmbvciCategory.Value) — 0 lists every
     * category. The grid is rebound ONLY when rows come back (:262); an empty answer leaves the rows
     * already on screen, exactly as the desktop does.
     */
    async function FormHistory() {
        var rows = await api('GET', API + '/history?categoryId=' + encodeURIComponent(categoryValue())) || [];
        applyHistory(rows);
    }
    function applyHistory(rows) {
        if (rows && rows.length > 0) {
            st.rows = rows;
            st.sel = null;
            renderGrid();
        }
    }

    // ============================================================================ form

    /** Reset (:234-249) */
    async function Reset() {
        st.recId = 0;
        show('btnsave', true);
        show('btnUpdate', false);
        $('txtDescription').value = '';
        try { await FormHistory(); } catch (e) { alert(msg(e)); }      // Reset has its own try/catch (:246)
        focus('cmbvciCategory');
    }

    /** FormValidation (:204-220) */
    function FormValidation() {
        if ($('txtDescription').value === '') { alert('Description Field is Required'); focus('txtDescription'); return false; }
        if (!categoryActiveRow() || categoryValue() === 0) { alert('VCI Category Field is Required'); focus('txtDescription'); return false; }
        return true;
    }

    /** Insert (:131-177) */
    async function Insert() {
        if (!FormValidation()) return;
        var recId = st.recId;
        if (recId > 0) {
            if (!confirm('Are you sure to Update?')) return;
        } else if (!confirm('Are you sure to Save?')) {
            return;
        }
        await api('POST', API + '/save', {
            id: recId,
            categoryId: categoryValue(),                         // Conversion.ToInt(cmbvciCategory.Value)
            description: $('txtDescription').value                // Conversion.ToString(txtDescription.Text) — as typed
        });
        alert(recId > 0 ? 'Record Update Successfully' : 'Record Save Successfully');
        await Reset();
    }

    /** btnsave_Click (:179-190) */
    function btnsave_Click() {
        return withBusy($('btnsave'), async function () { st.recId = 0; await Insert(); });
    }
    /** btnUpdate_Click (:192-202) */
    function btnUpdate_Click() {
        return withBusy($('btnUpdate'), async function () { await Insert(); });
    }
    /** btnnew_Click (:221-232) */
    function btnnew_Click() {
        return withBusy($('btnnew'), async function () {
            await Reset();
            $('cmbvciCategory').selectedIndex = -1;               // cmbvciCategory.Text = string.Empty
        });
    }
    /** btnRefresh_Click (:400-410) */
    function btnRefresh_Click() {
        return withBusy($('btnRefresh'), async function () {
            bindCategories(await api('GET', API + '/categories'));
        });
    }
    /** btnVCICategoryDefine_Click (:412-422): new ExImVCICategory(UserAccount).Show() */
    function btnVCICategoryDefine_Click() {
        var url = document.querySelector('.vcp-root').getAttribute('data-vci-category-url') || '';
        if (url) window.open(url, '_blank');
    }

    /** grdDetail_DoubleClick (:323-343) — also the click on a row's link. Nothing is read from the server. */
    function grdDetail_DoubleClick(id) {
        var r = rowById(id);
        if (!r) return;
        st.recId = intOf(r.Id);
        setCategoryText(text(r.CategoryDescription));
        $('txtDescription').value = text(r.ParameterDescription);
        show('btnUpdate', true);
        show('btnsave', false);
        focus('cmbvciCategory');
    }

    /** Footer History button: FormHistory() and the History grid is brought into view. */
    function btnHistory_Click() {
        return withBusy($('btnHistory'), async function () {
            await FormHistory();
            var w = $('grdDetailWrap');
            if (w.scrollIntoView) w.scrollIntoView({ block: 'nearest' });
            w.focus();
        });
    }

    function toggleFull() {
        var on = $('gridBox').classList.toggle('is-full');
        $('btnFull').title = on ? 'Exit full screen' : 'Full screen';
        $('btnFull').innerHTML = on ? '<i class="fa fa-compress"></i>' : '<i class="fa fa-arrows-alt"></i>';
    }

    // ============================================================================ keys

    /** ExImVCIParameter_KeyDown (:345-386). The ifs are independent on the desktop; so are they here. */
    function onKey(e) {
        var k = e.key, ctrl = e.ctrlKey;
        if (k === 'Escape' && $('gridBox').classList.contains('is-full')) { e.preventDefault(); toggleFull(); return; }
        /* ToolStrip mnemonics: "&New", "&Refresh", "&Save", "&Update" (designer text) = Alt+N / R / S / U. */
        if (e.altKey && !ctrl) {
            var a = (k || '').toLowerCase();
            if (a === 'n') { e.preventDefault(); btnnew_Click(); }
            else if (a === 'r') { e.preventDefault(); btnRefresh_Click(); }
            else if (a === 's' && visible('btnsave')) { e.preventDefault(); btnsave_Click(); }
            else if (a === 'u' && visible('btnUpdate')) { e.preventDefault(); btnUpdate_Click(); }
            return;
        }
        if (ctrl && (k === 'n' || k === 'N')) { e.preventDefault(); btnnew_Click(); }
        if (ctrl && (k === 's' || k === 'S')) { e.preventDefault(); if (visible('btnsave') && !$('btnsave').disabled) btnsave_Click(); }
        if (ctrl && (k === 'u' || k === 'U')) { e.preventDefault(); if (visible('btnUpdate') && !$('btnUpdate').disabled) btnUpdate_Click(); }
        if ((ctrl && (k === 'e' || k === 'E')) || k === 'Escape') {
            if (k === 'Escape' && e.defaultPrevented) return;      // Esc just closed an open drop-down
            e.preventDefault(); window.location.href = EXIT; return;   // Close()
        }
        if (ctrl && k === 'ArrowDown') { e.preventDefault(); $('grdDetailWrap').focus(); }
        if (ctrl && k === 'ArrowUp') { e.preventDefault(); focus('txtDescription'); }
        if (ctrl && k === 'F5') { e.preventDefault(); focus('txtDescription'); }
        if (ctrl && (k === 'r' || k === 'R')) { e.preventDefault(); btnRefresh_Click(); }
    }
    function onGridKey(e) {
        if (e.ctrlKey) return;
        if (e.key === 'ArrowDown') { e.preventDefault(); move(1); }
        else if (e.key === 'ArrowUp') { e.preventDefault(); move(-1); }
        else if (e.key === 'Home') { e.preventDefault(); move('first'); }
        else if (e.key === 'End') { e.preventDefault(); move('last'); }
    }

    function wire() {
        $('btnnew').addEventListener('click', btnnew_Click);
        $('btnRefresh').addEventListener('click', btnRefresh_Click);
        $('btnsave').addEventListener('click', btnsave_Click);
        $('btnUpdate').addEventListener('click', btnUpdate_Click);
        $('btnVCICategoryDefine').addEventListener('click', btnVCICategoryDefine_Click);
        $('btnHistory').addEventListener('click', btnHistory_Click);
        $('btnFull').addEventListener('click', toggleFull);
        var t = $('grdDetail');
        t.addEventListener('click', function (e) {
            var a = e.target.closest('a[data-open]');
            if (a) { e.preventDefault(); select(a.getAttribute('data-open'), false); grdDetail_DoubleClick(a.getAttribute('data-open')); return; }
            var th = e.target.closest('th[data-col]');
            if (th) {
                var c = th.getAttribute('data-col');
                if (st.sort === c) st.sortDir = -st.sortDir; else { st.sort = c; st.sortDir = 1; }
                renderGrid(); return;
            }
            var g = e.target.closest('tr.grp');
            if (g) { var k = g.getAttribute('data-grp'); st.collapsed[k] = !st.collapsed[k]; renderGrid(); return; }
            var tr = e.target.closest('tr[data-id]');
            if (tr) select(tr.getAttribute('data-id'), false);
        });
        t.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tr[data-id]');        // :328 — only a record row, not a group header
            if (tr) { select(tr.getAttribute('data-id'), false); grdDetail_DoubleClick(tr.getAttribute('data-id')); }
        });
        $('grdDetailWrap').addEventListener('keydown', onGridKey);
        $('navFirst').addEventListener('click', function () { move('first'); });
        $('navPrev').addEventListener('click', function () { move(-1); });
        $('navNext').addEventListener('click', function () { move(1); });
        $('navLast').addEventListener('click', function () { move('last'); });
        document.addEventListener('keydown', onKey);
    }

    /** ExImVCIParameter_Load (:75-92) */
    async function init() {
        wire();
        var root = document.querySelector('.vcp-root');
        if (!(root.getAttribute('data-vci-category-url') || '')) {
            $('btnVCICategoryDefine').disabled = true;
            $('btnVCICategoryDefine').title = 'The VCI Category screen (ExImVCICategory) has no web page yet';
        }
        var L;
        try {
            L = await api('GET', API + '/init');
        } catch (e) {
            $('rightsNote').textContent = msg(e);
            $('rightsNote').hidden = false;
            document.querySelectorAll('.wf-root button, .wf-root input, .wf-root select').forEach(function (el) { el.disabled = true; });
            return;
        }
        show('btnsave', true);
        show('btnUpdate', false);
        bindCategories(L.categories);         // ParameterCategoryfill()
        st.rows = [];
        renderGrid();
        applyHistory(L.rows);                 // FormHistory()
        focus('cmbvciCategory');
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
}());
