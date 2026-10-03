/* ============================================================================================
 * Screen 168 "Export Pre Shipment Analysis" — Architecture.WinApp.Export/EximPreProductionLab.cs.
 * Line references (:n) are EximPreProductionLab.cs.
 *
 *   FrmPreProductionLotInspection_Load :93-107   both combos, Save visible / Update hidden, ReadAll
 *   LabInspectionNameFill              :109-129  BindDDLNew(Id, LookUpName, caption "LookUpName", no zero row)
 *   ItemNameFill                       :131-145  BindDDLNew(Id, ItemName, caption "Ite mName", no zero row)
 *   Reset                              :147-165  RecId 0, the fields cleared (not the date), Save visible
 *   Insert                             :167-201  save or update, message, Reset, ReadAll — no validation
 *   grd_DoubleClick                    :203-207  ReadById of the current row
 *   btnnew / btnsave / btnUpdate       :209-222
 *   ReadById                           :224-250  Update visible / Save hidden, the seven fields
 *   ReadAll / grdSetting               :252-299  grid = the procedure's own columns
 *   btnRefresh_Click                   :301-305  ItemNameFill, LabInspectionNameFill
 *
 * The form has no KeyDown handler; the only keys are the tool strip mnemonics ("&New", "&Save", "&Print").
 * The desktop form no longer works against its procedures (see ExportPreShipmentAnalysisService): Save
 * and Update are refused with the desktop's own error text, which this page shows as the desktop does.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/lab/export-pre-shipment-analysis';

    /* grdSetting (:282-299) */
    var HIDDEN = { Id: 1, InspectedByLabId: 1, ItemId: 1 };
    var WIDTH = { LookUpName: 100, ItemName: 120, ReportRefNo: 80, LotRefNo: 80, InspectionRemarks: 220 };
    /* The document number of the row is its link (a click loads the record, like a double-click on the row). */
    var LINK_COL = 'ReportDocNo';

    var st = { columns: [], rows: [], recId: 0, sel: -1, sort: null, sortDir: 1 };

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
    function pad(n) { return n < 10 ? '0' + n : '' + n; }
    function today() { var d = new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }

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

    // ============================================================================ combos

    /** combo.Value — null (here 0, Convert.ToInt32(null)) when no row is selected. */
    function comboValue(id) { var el = $(id); return el.selectedIndex < 0 ? 0 : intOf(el.value); }
    /** combo.Value = v — LimitToList: a value that is not in the list leaves no selection. */
    function setComboValue(id, v) {
        var el = $(id), s = String(v), i;
        for (i = 0; i < el.options.length; i++) { if (el.options[i].value === s) { el.selectedIndex = i; return; } }
        el.selectedIndex = -1;
    }
    /**
     * DropDownBind.BindDDLNew(dt, combo, valueMember, displayMember, caption, false): no "...Select Any
     * Value..." row and no initial value. The callers bind only "if (dt.Rows.Count > 0)" (:119, :136), so
     * an empty answer leaves the combo as it was. A selection that is still listed is kept.
     */
    function bind(id, rows, valueMember, displayMember) {
        rows = rows || [];
        if (rows.length === 0) return;
        var el = $(id), keep = el.selectedIndex < 0 ? null : el.value, html = '';
        rows.forEach(function (r) { html += '<option value="' + esc(r[valueMember]) + '">' + esc(r[displayMember]) + '</option>'; });
        el.innerHTML = html;
        el.selectedIndex = -1;
        if (keep !== null) setComboValue(id, keep);
    }
    function LabInspectionNameFill(rows) { bind('cmbInspectionLabName', rows, 'Id', 'LookUpName'); }
    function ItemNameFill(rows) { bind('CmbItem', rows, 'Id', 'ItemName'); }

    // ============================================================================ grid

    function visibleCols() { return st.columns.filter(function (c) { return !HIDDEN[c]; }); }
    function isDateText(v) { return typeof v === 'string' && /^\d{4}-\d{2}-\d{2}(T\d{2}:\d{2}(:\d{2}(\.\d+)?)?)?$/.test(v); }
    /** A date cell: dd/MM/yyyy, with the time when it is not midnight. */
    function dateText(v) {
        var s = v.substring(8, 10) + '/' + v.substring(5, 7) + '/' + v.substring(0, 4);
        var t = v.length > 10 ? v.substring(11, 19) : '';
        return t && t !== '00:00:00' && t !== '00:00' ? s + ' ' + t : s;
    }
    function cell(c, v, i) {
        if (typeof v === 'boolean') return '<td class="c"><input type="checkbox" disabled' + (v ? ' checked' : '') + '></td>';   // a bit column is a check box
        var cls = typeof v === 'number' ? ' class="n"' : (WIDTH[c] && WIDTH[c] >= 200 ? ' class="wrap"' : '');
        var shown = isDateText(v) ? dateText(v) : text(v);
        if (c === LINK_COL) return '<td' + cls + '><a href="#" class="lnk" data-open="' + i + '">' + esc(shown === '' ? '(open)' : shown) + '</a></td>';
        return '<td' + cls + '>' + esc(shown) + '</td>';
    }
    function order() {
        var idx = st.rows.map(function (r, i) { return i; });
        if (!st.sort) return idx;
        var k = st.sort, d = st.sortDir;
        return idx.sort(function (a, b) {
            var x = st.rows[a][k], y = st.rows[b][k];
            if (typeof x === 'number' && typeof y === 'number') return (x - y) * d;
            return text(x).localeCompare(text(y)) * d;
        });
    }
    function renderGrid() {
        var t = $('grd'), cols = visibleCols();
        /* grd.DataSource = null (:273): no rows = a grid without columns. */
        if (!st.rows.length || !cols.length) {
            t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = '';
            renderNav();
            return;
        }
        var linkCol = cols.indexOf(LINK_COL) >= 0;
        t.tHead.innerHTML = '<tr><th class="ind"></th>' + cols.map(function (c) {
            var mark = st.sort === c ? (st.sortDir > 0 ? ' ▲' : ' ▼') : '';
            var w = WIDTH[c] ? ' style="min-width:' + WIDTH[c] + 'px;"' : '';
            return '<th data-col="' + esc(c) + '"' + w + '>' + esc(c) + mark + '</th>';
        }).join('') + '</tr>';
        var html = '';
        order().forEach(function (i) {
            var r = st.rows[i];
            html += '<tr data-i="' + i + '"' + (i === st.sel ? ' class="is-sel"' : '') + '><td class="ind">'
                + (linkCol ? '' : '<a href="#" class="lnk" data-open="' + i + '" title="Open">&#9658;</a>') + '</td>'
                + cols.map(function (c) { return cell(c, r[c], i); }).join('') + '</tr>';
        });
        t.tBodies[0].innerHTML = html;
        /* TotalRow = True with no aggregate defined on any column: an empty total row. */
        t.tFoot.innerHTML = '<tr><td class="ind"></td>' + cols.map(function () { return '<td></td>'; }).join('') + '</tr>';
        renderNav();
    }
    function dataTrs() { return Array.prototype.slice.call($('grd').tBodies[0].querySelectorAll('tr[data-i]')); }
    /** RecordNavigator = True. */
    function renderNav() {
        var trs = dataTrs(), pos = 0;
        trs.forEach(function (tr, n) { if (intOf(tr.getAttribute('data-i')) === st.sel) pos = n + 1; });
        $('navPos').value = pos ? pos : '';
        $('navCount').textContent = 'of ' + trs.length;
    }
    function select(i, scroll) {
        st.sel = i;
        dataTrs().forEach(function (tr) {
            var on = intOf(tr.getAttribute('data-i')) === i;
            tr.classList.toggle('is-sel', on);
            if (on && scroll && tr.scrollIntoView) tr.scrollIntoView({ block: 'nearest' });
        });
        renderNav();
    }
    function move(where) {
        var trs = dataTrs();
        if (!trs.length) return;
        var n = -1;
        trs.forEach(function (tr, k) { if (intOf(tr.getAttribute('data-i')) === st.sel) n = k; });
        if (where === 'first') n = 0;
        else if (where === 'last') n = trs.length - 1;
        else n = Math.max(0, Math.min(trs.length - 1, n + where));
        select(intOf(trs[n].getAttribute('data-i')), true);
    }
    function applyGrid(g) {
        st.columns = (g && g.columns) || [];
        st.rows = (g && g.rows) || [];
        st.sel = -1;
        renderGrid();
    }
    /** ReadAll (:252-280) */
    async function ReadAll() {
        applyGrid(await api('GET', API + '/grid'));
    }
    /**
     * grd.AllowDelete = True and the form handles no DeletingRecord event: the Delete key takes the current
     * row out of the grid's table only. Nothing is deleted in the database; the next ReadAll shows it again.
     */
    function deleteCurrentRowFromView() {
        if (st.sel < 0 || st.sel >= st.rows.length) return;
        st.rows.splice(st.sel, 1);
        st.sel = -1;
        renderGrid();
    }

    // ============================================================================ form

    /** Reset (:147-165) — datDate is not touched. */
    function Reset() {
        st.recId = 0;
        $('cmbInspectionLabName').selectedIndex = -1;          // .Text = string.Empty
        $('txtReportRefNo').value = '';
        $('txtLotNo').value = '';
        $('txtQty').value = '';
        $('CmbItem').selectedIndex = -1;
        $('txtRemarks').value = '';
        show('btnsave', true);
        show('btnUpdate', false);
    }

    /** Insert (:167-201). The values go as the form reads them; the server answers what the desktop shows. */
    async function Insert() {
        var recId = st.recId;
        var r = await api('POST', API + '/save', {
            id: recId,                                              // :173-176
            inspectedByLabId: comboValue('cmbInspectionLabName'),   // Convert.ToInt32(cmbInspectionLabName.Value)
            reportDate: $('datDate').value,                         // datDate.Value
            reportRefNo: $('txtReportRefNo').value,                 // .Trim() on the server
            lotRefNo: $('txtLotNo').value,
            itemId: comboValue('CmbItem'),                          // Convert.ToInt32(CmbItem.Value)
            qtyKgs: $('txtQty').value,                              // Convert.ToDouble(txtQty.Text.Trim())
            inspectionRemarks: $('txtRemarks').value
        });
        alert((r && r.message) || (recId === 0 ? 'Record Save Successfully' : 'Record Update Successfully'));   // :185-192
        Reset();
        await ReadAll();
    }

    function btnnew_Click() { return withBusy($('btnnew'), async function () { Reset(); }); }
    function btnsave_Click() { return withBusy($('btnsave'), Insert); }
    function btnUpdate_Click() { return withBusy($('btnUpdate'), Insert); }

    /** btnRefresh_Click (:301-305) */
    function btnRefresh_Click() {
        return withBusy($('btnRefresh'), async function () {
            var c = await api('GET', API + '/combos');
            ItemNameFill(c.items);
            LabInspectionNameFill(c.labs);
        });
    }

    /** ReadById (:224-250) */
    async function ReadById(id) {
        st.recId = id;                                              // :228 — set before the read, as on the desktop
        var o = await api('GET', API + '/' + encodeURIComponent(id));
        if (!o) return;
        show('btnsave', false);
        show('btnUpdate', true);
        setComboValue('cmbInspectionLabName', o.InspectedByLabId);
        if (o.ReportDate) $('datDate').value = String(o.ReportDate).substring(0, 10);
        $('txtReportRefNo').value = text(o.ReportRefNo);
        $('txtLotNo').value = text(o.LotRefNo);
        setComboValue('CmbItem', o.ItemId);
        $('txtQty').value = text(o.QtyKgs);
        $('txtRemarks').value = text(o.InspectionRemarks);
    }

    /**
     * grd_DoubleClick (:203-207) and the click on a row's link. The desktop reads Cells[0], which in today's
     * result set is the text column "ReportCriteria" and makes Convert.ToInt32 fail; the page uses the
     * row's Id column, which is what the form was written for.
     */
    function grd_DoubleClick(i) {
        var r = st.rows[i];
        if (!r) return;
        return withBusy(null, async function () { await ReadById(intOf(r.Id)); });
    }

    /** Footer History button: ReadAll() and the grid is brought into view. */
    function btnHistory_Click() {
        return withBusy($('btnHistory'), async function () {
            await ReadAll();
            var w = $('grdWrap');
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

    /** Tool strip mnemonics only: "&New" Alt+N, "&Save" Alt+S (while Save is shown), "&Print" Alt+P (no handler). */
    function onKey(e) {
        var k = (e.key || '').toLowerCase();
        if (k === 'escape' && $('gridBox').classList.contains('is-full')) { e.preventDefault(); toggleFull(); return; }
        if (!e.altKey || e.ctrlKey) return;
        if (k === 'n') { e.preventDefault(); btnnew_Click(); }
        else if (k === 's' && visible('btnsave')) { e.preventDefault(); btnsave_Click(); }
        else if (k === 'p') { e.preventDefault(); }
    }
    function onGridKey(e) {
        if (e.ctrlKey || e.altKey) return;
        if (e.key === 'ArrowDown') { e.preventDefault(); move(1); }
        else if (e.key === 'ArrowUp') { e.preventDefault(); move(-1); }
        else if (e.key === 'Home') { e.preventDefault(); move('first'); }
        else if (e.key === 'End') { e.preventDefault(); move('last'); }
        else if (e.key === 'Delete') { e.preventDefault(); deleteCurrentRowFromView(); }
    }

    function wire() {
        $('btnnew').addEventListener('click', btnnew_Click);
        $('btnsave').addEventListener('click', btnsave_Click);
        $('btnUpdate').addEventListener('click', btnUpdate_Click);
        $('btnRefresh').addEventListener('click', btnRefresh_Click);
        $('btnHistory').addEventListener('click', btnHistory_Click);
        $('btnFull').addEventListener('click', toggleFull);
        /* "Print" has no Click handler in the designer — the button does nothing, here as there. */
        var t = $('grd');
        t.addEventListener('click', function (e) {
            var a = e.target.closest('a[data-open]');
            if (a) { e.preventDefault(); var i = intOf(a.getAttribute('data-open')); select(i, false); grd_DoubleClick(i); return; }
            var th = e.target.closest('th[data-col]');
            if (th) {
                var c = th.getAttribute('data-col');
                if (st.sort === c) st.sortDir = -st.sortDir; else { st.sort = c; st.sortDir = 1; }
                renderGrid(); return;
            }
            var tr = e.target.closest('tr[data-i]');
            if (tr) select(intOf(tr.getAttribute('data-i')), false);
        });
        t.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tr[data-i]');
            if (tr) { var i = intOf(tr.getAttribute('data-i')); select(i, false); grd_DoubleClick(i); }
        });
        $('grdWrap').addEventListener('keydown', onGridKey);
        $('navFirst').addEventListener('click', function () { move('first'); });
        $('navPrev').addEventListener('click', function () { move(-1); });
        $('navNext').addEventListener('click', function () { move(1); });
        $('navLast').addEventListener('click', function () { move('last'); });
        document.addEventListener('keydown', onKey);
    }

    /** FrmPreProductionLotInspection_Load (:93-107) */
    async function init() {
        wire();
        $('datDate').value = today();          // a DateTimePicker starts on today
        var L;
        try {
            L = await api('GET', API + '/init');
        } catch (e) {
            $('rightsNote').textContent = msg(e);
            $('rightsNote').hidden = false;
            document.querySelectorAll('.wf-root button, .wf-root input, .wf-root select, .wf-root textarea').forEach(function (el) { el.disabled = true; });
            return;
        }
        LabInspectionNameFill(L.labs);
        ItemNameFill(L.items);
        show('btnsave', true);
        show('btnUpdate', false);
        applyGrid(L.grid);                     // ReadAll()
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
}());
