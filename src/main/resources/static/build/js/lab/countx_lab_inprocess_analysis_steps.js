/* ============================================================================================
 * Screen 163 "InProcess Analysis Steps Schedule" -
 * Architecture.WinApp.Lab/LabInProcessAnalysisStepAndParameterSchedule.cs. Line references (:n) are
 * that file; (dps:n) are Architecture.WinApp/DefineProcessStep.cs, the form BtnStep opens.
 * BLL 0396 InvProcessAnalysisStepSchedule / 0395 ProcessStep.
 *
 * The form: three combos with a "+" button each, a toolbar (New, Refresh, Update, Save) and one
 * read-only Janus grid. No delete, no print, no history. The desktop's Yes/No prompt comes after its
 * validations; the server answers HTTP 409 {confirm:true,message} and the request is resent with
 * confirm=true. The server re-validates everything and takes organization / company / user from the
 * session.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/lab/inprocess-analysis-steps';
    var EXIT_URL = '/quality';
    /* btnPlantDefine_Click (:471) -> DefineProductionPlant; btnDefineParameter_Click (:495) -> InvLabAnalysisItems. */
    var URL_DEFINE_PLANT = '/production/define-production-plant';
    var URL_DEFINE_PARAMETER = '/quality/item-analysis-parameter';
    var MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    /* gridfill (:330) builds Id, PlantId, LabInProcessAnalysisStepId, AnalysisParameterId, PlantName,
       ProcessStepName, AnalysisParameterDescription; grdfrmSetting (:372) hides the four ids and sets
       Width 250 on the three names. RetrieveStructure captions a column with its column name. */
    var COLS = [
        { key: 'PlantName', width: 250, open: true },              // rule 4: the first column opens the record (= grdfrm_DoubleClick)
        { key: 'ProcessStepName', width: 250 },
        { key: 'AnalysisParameterDescription', width: 250 }
    ];
    /* DefineProcessStep.bindGrid (dps:163): the 'FormHistory' columns as they come, Id hidden (dps:176). */
    var DPS_COLS = [
        { key: 'SortNo', num: true },
        { key: 'ProcessStepName', open: true },                     // rule 4: opens the step (= grdProcessStep_DoubleClick)
        { key: 'EntryUser' },
        { key: 'EntryDate', date: true }
    ];

    var L = null;
    var st = { recId: 0, rows: [], filter: {}, selId: 0 };
    var dps = { recId: 0, rows: [], filter: {}, selId: 0 };

    // ============================================================================ helpers

    function $(id) { return document.getElementById(id); }
    function esc(v) {
        return String(v === undefined || v === null ? '' : v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function str(v) { return v === undefined || v === null ? '' : String(v); }          // Conversion.ToString
    function intOf(v) { var n = parseInt(str(v), 10); return isNaN(n) ? 0 : n; }        // Conversion.ToInt
    function msg(e) { return e && e.message ? e.message : String(e); }
    function rights() { return (L && L.rights) || {}; }
    function show(id, on) { $(id).hidden = !on; }
    function usable(id) { var el = $(id); return !el.hidden && !el.disabled; }          // Visible && Enabled
    function dMMMyyyy(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(str(v));
        return m ? m[3] + '-' + MONTHS[+m[2] - 1] + '-' + m[1] : str(v);
    }

    async function call(method, url, body) {
        var opt = { method: method, credentials: 'same-origin', headers: { Accept: 'application/json' } };
        if (body !== undefined) { opt.headers['Content-Type'] = 'application/json'; opt.body = JSON.stringify(body); }
        var res = await fetch(url, opt);
        var text = await res.text();
        var data = null;
        try { data = text ? JSON.parse(text) : null; } catch (e) { data = null; }
        if (res.status === 409 && data && data.confirm) return data;                    // the desktop's Yes/No prompt
        if (!res.ok) throw new Error(data && (data.message || data.error) || ('The request failed (' + res.status + ').'));
        return data;
    }
    /** POST; when the server asks the desktop's Yes/No question, ask it and resend. null = the user said No. */
    async function postConfirmed(url, body) {
        var res = await call('POST', url, body);
        if (res && res.confirm) {
            if (!window.confirm(res.message)) return null;
            body.confirm = true;
            res = await call('POST', url, body);
        }
        return res;
    }
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

    /** DDL.BindDDLNew(dt, combo, "Id", display, caption, false) - called only when the table has rows. */
    function bind(id, rows, textKey) {
        if (!rows || rows.length <= 0) return;
        var el = $(id), before = el.selectedIndex < 0 ? null : el.value, html = '';
        rows.forEach(function (r) { html += '<option value="' + esc(r.Id) + '">' + esc(r[textKey]) + '</option>'; });
        el.innerHTML = html;
        setVal(id, before);
    }
    /** combo.Value = v - a value that is not in the list leaves the combo without an active row. */
    function setVal(id, v) {
        var el = $(id), idx = -1;
        if (v !== null && v !== undefined) {
            for (var i = 0; i < el.options.length; i++) { if (el.options[i].value === String(v)) { idx = i; break; } }
        }
        el.selectedIndex = idx;
    }
    function active(id) { return $(id).selectedIndex >= 0; }                               // ActiveRow != null
    function comboVal(id) { var el = $(id); return el.selectedIndex < 0 ? 0 : intOf(el.value); }
    function comboText(id) { var el = $(id); return el.selectedIndex < 0 ? '' : el.options[el.selectedIndex].textContent; }
    function focusEl(id) {
        var el = $(id), w = el.closest ? el.closest('.dtcombo-wrap') : null, i = w && w.querySelector('.dtcombo-input');
        (i || el).focus();
    }
    function BindPlantName(rows) { bind('cmbPlantName', rows, 'Description'); }                  // (:144) caption "Plant/Feader "
    function BindProcessStepName(rows) { bind('cmbProcessStepName', rows, 'ProcessStepName'); }   // (:195) caption "Process Step Name"
    function BindParameter(rows) { bind('CmbParentParameter', rows, 'AnalysisParameterDescription'); } // (:169) caption "Item"

    /** GenerateSortNo (:306) - the text box is set only when SortNo > 0. */
    function GenerateSortNo(sortNo) { if (intOf(sortNo) > 0) $('txtSortNo').value = String(intOf(sortNo)); }

    // ============================================================================ grids

    function visible(rows, cols, filter, cell) {
        return rows.filter(function (r) {
            return cols.every(function (c) {
                var f = filter[c.key];
                return !f || String(cell(c, r)).toLowerCase().indexOf(f.toLowerCase()) >= 0;
            });
        });
    }
    function cellText(c, r) { return c.date ? dMMMyyyy(r[c.key]) : str(r[c.key]); }

    /**
     * One renderer for grdfrm and for DefineProcessStep's grd: header, filter row (FilterMode Automatic),
     * rows, an empty total row (TotalRow True, no aggregates). With no rows the desktop calls
     * ClearStructure(), which leaves a grid without columns.
     */
    function render(tableId, s, cols, navId) {
        var t = $(tableId);
        var activeF = document.activeElement && t.contains(document.activeElement) ? document.activeElement.getAttribute('data-f') : null;
        if (s.rows.length <= 0) {
            t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = '';
            if (navId) $(navId).textContent = '0 of 0';
            return;
        }
        t.tHead.innerHTML = '<tr>' + cols.map(function (c) {
            return '<th' + (c.width ? ' style="width:' + c.width + 'px;min-width:' + c.width + 'px;"' : '') + '>' + esc(c.key) + '</th>';
        }).join('') + '<th></th></tr><tr class="flt">' + cols.map(function (c) {
            return '<td><input type="text" data-f="' + c.key + '" value="' + esc(s.filter[c.key] || '') + '"></td>';
        }).join('') + '<td></td></tr>';
        var rows = visible(s.rows, cols, s.filter, cellText);
        t.tBodies[0].innerHTML = rows.map(function (r) {
            return '<tr data-id="' + esc(r.Id) + '"' + (String(r.Id) === String(s.selId) ? ' class="is-sel"' : '') + '>'
                + cols.map(function (c) {
                    if (c.open) return '<td><a class="lab-open" data-open="1" title="Open this record">' + esc(cellText(c, r)) + '</a></td>';
                    return '<td' + (c.num ? ' class="n"' : '') + '>' + esc(cellText(c, r)) + '</td>';
                }).join('')
                + '<td></td></tr>';
        }).join('');
        t.tFoot.innerHTML = '<tr>' + cols.map(function () { return '<td></td>'; }).join('') + '<td></td></tr>';
        if (navId) nav(navId, s, rows);
        if (activeF) {
            var again = t.querySelector('input[data-f="' + activeF + '"]');
            if (again) { again.focus(); var n = again.value.length; again.setSelectionRange(n, n); }
        }
    }
    function nav(navId, s, rows) {
        var pos = 0;
        rows.forEach(function (r, i) { if (String(r.Id) === String(s.selId)) pos = i + 1; });
        $(navId).textContent = pos + ' of ' + rows.length;
    }
    function select(tableId, s, id, cols, navId) {
        s.selId = id;
        Array.prototype.forEach.call($(tableId).tBodies[0].rows, function (tr) {
            tr.classList.toggle('is-sel', tr.getAttribute('data-id') === String(id));
        });
        if (navId) nav(navId, s, visible(s.rows, cols, s.filter, cellText));
    }
    function wireGrid(tableId, s, cols, navId, onDouble) {
        var t = $(tableId);
        t.addEventListener('input', function (e) {
            var f = e.target.getAttribute && e.target.getAttribute('data-f');
            if (!f) return;
            s.filter[f] = e.target.value;
            render(tableId, s, cols, navId);
        });
        t.addEventListener('click', function (e) {
            var tr = e.target.closest('tr[data-id]');
            if (tr) {
                select(tableId, s, tr.getAttribute('data-id'), cols, navId);
                if (e.target.closest('a.lab-open')) onDouble();
            }
        });
        t.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tr[data-id]');
            if (!tr) return;
            select(tableId, s, tr.getAttribute('data-id'), cols, navId);
            onDouble();
        });
    }

    /** gridfill (:330) */
    function gridfill(rows) {
        st.rows = rows || [];
        render('grdfrm', st, COLS, 'grdfrmNav');
    }

    // ============================================================================ form

    /** formvalidation (:93) - ActiveRow == null on each combo, desktop texts and focus targets. */
    function formvalidation() {
        if (!active('cmbPlantName')) { alert('Plant Name Field is Required'); focusEl('cmbPlantName'); return false; }
        if (!active('cmbProcessStepName')) { alert('Process Step Field is Required'); focusEl('cmbProcessStepName'); return false; }
        if (!active('CmbParentParameter')) { alert('Parent Parameter Field is Required'); focusEl('CmbParentParameter'); return false; }
        return true;
    }

    /** refresh() (:116): buttons, the three combo texts, gridfill(), GenerateSortNo(). RecId is not reset there. */
    async function refresh() {
        show('btnsave', true);
        show('btnupdate', false);
        setVal('CmbParentParameter', null);
        setVal('cmbProcessStepName', null);
        setVal('cmbPlantName', null);
        var d = await call('GET', API + '/refresh');
        gridfill(d.rows);
        GenerateSortNo(d.sortNo);
    }

    /**
     * Insert() (:221). The grid check (:228-241) compares the combo TEXTS with the rows the grid is
     * showing (GetRows()), and it also runs on Update. Note the Plant combo shows the plant's Description
     * while the grid's PlantName is the plant's Code - the desktop compares the two as they are.
     */
    async function Insert(button) {
        if (!formvalidation()) return;
        var shown = visible(st.rows, COLS, st.filter, cellText);
        for (var i = 0; i < shown.length; i++) {
            if (comboText('CmbParentParameter') === str(shown[i].AnalysisParameterDescription)
                && comboText('cmbPlantName') === str(shown[i].PlantName)
                && comboText('cmbProcessStepName') === str(shown[i].ProcessStepName)) {
                alert('This Schedule Already Exits In Grid');
                return;
            }
        }
        var res = await postConfirmed(API + '/save', {
            id: st.recId,
            sortNo: intOf($('txtSortNo').value),                      // Conversion.ToInt(txtSortNo.Text) (:256)
            plantId: comboVal('cmbPlantName'),                        // (:257)
            stepId: comboVal('cmbProcessStepName'),                   // (:258)
            parameterId: comboVal('CmbParentParameter')               // (:259)
        });
        if (!res) return;                                             // DialogResult.No
        alert(res.message);                                           // "Save Successfully" / "Update Successfully"
        await refresh();
    }
    /** btnsave_Click (:281) - RecId = 0, Insert(). */
    function btnsave_Click() { return act($('btnsave'), async function () { st.recId = 0; await Insert(); }); }
    /** btnupdate_Click (:294) - Insert() with the RecId of the row that was opened. */
    function btnupdate_Click() { return act($('btnupdate'), async function () { await Insert(); }); }
    /** btnnew_Click (:134) */
    function btnnew_Click() { return act($('btnnew'), refresh); }
    /** btnRefresh_Click (:525) - rebinds the three combos only. */
    function btnRefresh_Click() {
        return act($('btnRefresh'), async function () {
            var d = await call('GET', API + '/combos');
            (d.errors || []).forEach(function (m) { alert(m); });
            BindPlantName(d.plants);
            BindProcessStepName(d.steps);
            BindParameter(d.parameters);
        });
    }

    /** grdfrm_DoubleClick (:393) - Save hidden / Update shown first, then the row is read again by Id. */
    function grdfrm_DoubleClick() {
        var id = intOf(st.selId);
        if (!id) return;
        return act(null, async function () {
            show('btnsave', false);
            show('btnupdate', true);
            st.recId = id;
            var d = await call('GET', API + '/' + id);
            var r = d && d.row;
            if (!r) return;                                           // dtgetbyId.Rows.Count > 0 (:408)
            $('txtSortNo').value = str(r.SortNo);
            setVal('cmbPlantName', intOf(r.PlantId));
            setVal('cmbProcessStepName', intOf(r.LabInProcessAnalysisStepId));
            setVal('CmbParentParameter', intOf(r.AnalysisParameterId));
        });
    }

    // ============================================================================ DefineProcessStep

    function dpsOpen() { return !$('dlgProcessStep').hidden; }
    function dpsRender() { render('dps_grd', dps, DPS_COLS, null); }

    /** DefineProcessStep_Load (dps:65): bindGrid(); Update hidden; focus; GenerateSortNo(). */
    function btnProcessStep_Click() {                                 // (:483)
        return act($('BtnStep'), async function () {
            var d = await call('GET', API + '/process-steps');
            dps.recId = 0; dps.selId = 0; dps.filter = {};
            dps.rows = d.rows || [];
            $('dps_txtProcessStepName').value = '';
            $('dps_txtSortNo').value = intOf(d.sortNo) > 0 ? String(intOf(d.sortNo)) : '';
            show('dps_btnsave', true);
            show('dps_btnUpdate', false);
            $('dlgProcessStep').hidden = false;
            dpsRender();
            $('dps_txtProcessStepName').focus();
        });
    }
    /** formReFresh (dps:237) */
    async function dpsFormReFresh() {
        dps.recId = 0;
        var d = await call('GET', API + '/process-steps');
        dps.rows = d.rows || [];
        dpsRender();
        if (intOf(d.sortNo) > 0) $('dps_txtSortNo').value = String(intOf(d.sortNo));
        $('dps_txtProcessStepName').value = '';
        $('dps_txtProcessStepName').focus();
        show('dps_btnsave', true);
        show('dps_btnUpdate', false);
    }
    /** DefineProcessStep.Insert (dps:104): FormValidation (dps:214), prompt, ProcessStep.Save, message, formReFresh. */
    async function dpsInsert() {
        if ($('dps_txtProcessStepName').value.trim() === '') {
            alert('Process Name Field Required');
            $('dps_txtProcessStepName').focus();
            return;
        }
        var res = await postConfirmed(API + '/process-steps/save', {
            id: dps.recId,
            processStepName: $('dps_txtProcessStepName').value.trim(),
            sortNo: intOf($('dps_txtSortNo').value)
        });
        if (!res) return;
        alert(res.message);                                           // "Record Save Successfully..." / "Record Update Successfully..."
        await dpsFormReFresh();
    }
    function dpsSave() { return act($('dps_btnsave'), async function () { dps.recId = 0; await dpsInsert(); }); }   // dps:150
    function dpsUpdate() { return act($('dps_btnUpdate'), async function () { await dpsInsert(); }); }              // dps:225
    function dpsNew() { return act($('dps_btnnew'), dpsFormReFresh); }                                              // dps:260
    /** grdProcessStep_DoubleClick (dps:193) */
    function dpsDoubleClick() {
        var id = intOf(dps.selId);
        if (!id) return;
        return act(null, async function () {
            show('dps_btnsave', false);
            show('dps_btnUpdate', true);
            dps.recId = id;
            var o = await call('GET', API + '/process-steps/' + id);
            $('dps_txtProcessStepName').value = str(o.ProcessStepName);
            $('dps_txtSortNo').value = str(o.SortNo);
        });
    }
    /**
     * Closing the dialog. On the desktop DefineProcessStep is a separate window and the user presses
     * Refresh (btnRefresh_Click :525) to see a newly defined step. Rule 3 (dependent combos reload): the
     * Process Step combo is rebound here from the same procedure, keeping the current selection (bind()).
     */
    function dpsClose() {
        $('dlgProcessStep').hidden = true;
        focusEl('cmbProcessStepName');
        return act(null, async function () {
            var d = await call('GET', API + '/combos');
            (d.errors || []).forEach(function (m) { alert(m); });
            BindProcessStepName(d.steps);
        });
    }

    // ============================================================================ keys

    /** SendKeys.Send("{TAB}") */
    function tabNext(from) {
        var list = Array.prototype.filter.call(
            document.querySelectorAll('.ips-page input, .ips-page select, .ips-page button, .ips-page [tabindex]'),
            function (el) { return !el.disabled && !el.hidden && el.type !== 'hidden' && el.offsetParent !== null && el.getAttribute('tabindex') !== '-1'; });
        var i = list.indexOf(from);
        if (i >= 0 && i + 1 < list.length) list[i + 1].focus();
    }
    function onKey(e) {
        if (e.defaultPrevented) return;
        var key = (e.key || '').toLowerCase(), ctrl = e.ctrlKey || e.metaKey;
        if (dpsOpen()) {                                               // DefineProcessStep_KeyDown (dps:265)
            if (ctrl && key === 'n') { e.preventDefault(); dpsNew(); return; }
            if (ctrl && key === 's') { e.preventDefault(); if (usable('dps_btnsave')) dpsSave(); return; }
            if (ctrl && key === 'u') { e.preventDefault(); if (usable('dps_btnUpdate')) dpsUpdate(); return; }
            if ((ctrl && key === 'e') || key === 'escape') { e.preventDefault(); dpsClose(); }
            return;
        }
        /* InvLabAnalysisItems_KeyDown (:440), KeyPreview */
        if (key === 'enter' && !ctrl && !e.altKey && !e.shiftKey) {
            var t = e.target;
            if (t && t.tagName !== 'BUTTON' && t.tagName !== 'A') { e.preventDefault(); tabNext(t); }
            return;
        }
        if (ctrl && key === 's') { e.preventDefault(); if (usable('btnsave')) btnsave_Click(); return; }
        if (ctrl && key === 'u') { e.preventDefault(); if (usable('btnupdate')) btnupdate_Click(); return; }
        if (ctrl && key === 'n') { e.preventDefault(); btnnew_Click(); return; }
        if ((ctrl && key === 'e') || key === 'escape') { e.preventDefault(); window.location.href = EXIT_URL; }
    }

    function wire() {
        $('btnnew').addEventListener('click', btnnew_Click);
        $('btnRefresh').addEventListener('click', btnRefresh_Click);
        $('btnupdate').addEventListener('click', btnupdate_Click);
        $('btnsave').addEventListener('click', btnsave_Click);
        /* The desktop opens the other definition forms as separate windows (.Show()). */
        $('btnPlant').addEventListener('click', function () { window.open(URL_DEFINE_PLANT, '_blank'); });
        $('BtnStep').addEventListener('click', btnProcessStep_Click);
        $('btnDefineParameter').addEventListener('click', function () { window.open(URL_DEFINE_PARAMETER, '_blank'); });
        wireGrid('grdfrm', st, COLS, 'grdfrmNav', grdfrm_DoubleClick);

        $('dps_btnnew').addEventListener('click', dpsNew);
        $('dps_btnUpdate').addEventListener('click', dpsUpdate);
        $('dps_btnsave').addEventListener('click', dpsSave);
        $('dpsClose').addEventListener('click', dpsClose);
        wireGrid('dps_grd', dps, DPS_COLS, null, dpsDoubleClick);

        document.addEventListener('keydown', onKey);
    }

    // ============================================================================ load

    /** InvLabAnalysisItems_Load (:422): buttons, gridfill, BindPlantName, BindProcessStepName, BindParameter, GenerateSortNo. */
    async function init() {
        wire();
        try {
            L = await call('GET', API + '/lookups');
        } catch (e) {
            $('rightsNote').textContent = msg(e);
            $('rightsNote').hidden = false;
            document.querySelectorAll('.wf-root button, .wf-root input, .wf-root select').forEach(function (el) { el.disabled = true; });
            return;
        }
        var r = rights();
        ['btnsave', 'dps_btnsave'].forEach(function (id) {
            $(id).disabled = !r.save;
            if (!r.save) $(id).title = 'You do not have the Save right for this screen.';
        });
        ['btnupdate', 'dps_btnUpdate'].forEach(function (id) {
            $(id).disabled = !r.update;
            if (!r.update) $(id).title = 'You do not have the Update right for this screen.';
        });
        show('btnsave', true);
        show('btnupdate', false);
        (L.errors || []).forEach(function (m) { alert(m); });      // each desktop fill has its own catch -> MessageBox
        gridfill(L.rows);
        BindPlantName(L.plants);
        BindProcessStepName(L.steps);
        BindParameter(L.parameters);
        GenerateSortNo(L.sortNo);
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();
