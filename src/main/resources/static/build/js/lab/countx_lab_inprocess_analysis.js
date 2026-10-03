/* ============================================================================================
 * Screen 162 "In-Process Analysis" — InvLabAnalysisInProcess.cs (Architecture.WinApp.Lab),
 * DocumentTypeId 306. Line references (:n) are InvLabAnalysisInProcess.cs.
 *
 * The page is the form, event for event: the combos bind the same lists (no default row — every
 * BindDDL / BindDDLNew call passes ZeroIndex false), the plant fills the Step Analysis grid, the
 * analysis group fills the Analysis Group grid, the job order fills the items, and the last five
 * results are matched into Caption1..5 exactly as TopFiveStepAnalysisResultsAgainstPlantJobOrderAndItem
 * (:755) does. The server re-validates everything it saves; the Doc No shown here is display only.
 *
 * Desktop "Leave" events are wired to the combo's change (the reference port does the same), plus a
 * focus-out on the plant / analysis-group combo that fills an EMPTY grid — so a re-visit without a
 * change does not wipe typed results, which the desktop's Leave would.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/lab/inprocess-analysis';
    var PRINT_RPT = '659-RptInvLabInProcessAnalysisSlip.rpt';       // CommonServices.LabAnalysisReport659 (CommonServices.cs:10959)
    var EXIT_URL = '/quality';
    var MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    if (window.DesktopCombo) {
        /* ItemNameBind (:513) — BindAndRetainSelection AllColumns: Id (hidden), ItemName ("Item Name"), ItemCode. */
        window.DesktopCombo.define('labIpaItem', [
            { caption: 'Item Name', flex: 4 },
            { caption: 'ItemCode', flex: 2, key: 'item-code' }
        ]);
    }

    var L = null;                       // lookups
    var st = {
        recId: 0,
        steps: [],                      // GetByPlantdt (:305)
        groups: [],                     // GetByAnalysisGroupdt (:293)
        caps: defaultCaps(),            // grdStepAnalysis Caption1..5 header captions
        analysisDate: '',               // date part of txtanalysistime.Value (the picker shows the time only)
        pics: { analysis: blankPic(), cooking: blankPic() },
        hist: [], histSel: -1, hFilter: {},
        histSteps: null, histGroups: null
    };

    function defaultCaps() { return ['Caption1', 'Caption2', 'Caption3', 'Caption4', 'Caption5']; }
    /** state: none | keep (the stored picture of the edited record) | new (browsed, not yet saved). */
    function blankPic() { return { state: 'none', fileName: '', data: '' }; }

    // ============================================================================ helpers

    function $(id) { return document.getElementById(id); }
    function esc(v) {
        return String(v === undefined || v === null ? '' : v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    /** Conversion.ToDouble */
    function num(v) {
        if (v === null || v === undefined) return 0;
        if (typeof v === 'number') return isFinite(v) ? v : 0;
        var s = String(v).replace(/,/g, '').trim();
        if (s === '') return 0;
        var n = Number(s);
        return isFinite(n) ? n : 0;
    }
    function intOf(v) { var n = parseInt(String(v === undefined || v === null ? '' : v).replace(/,/g, ''), 10); return isNaN(n) ? 0 : n; }
    /** FormatString "#,##0.####" */
    function fmt4(v) {
        if (v === null || v === undefined || v === '') return '';
        return num(v).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 4 });
    }
    function pad(n) { return String(n).padStart(2, '0'); }
    function isoOf(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function today() { return isoOf(new Date()); }
    function nowTime() { var d = new Date(); return pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds()); }
    function parts(v) { return /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2})(?::(\d{2}))?)?/.exec(String(v || '')); }
    /** "dd-MMM-yyyy" */
    function dMMMyyyy(v) { var m = parts(v); return m ? m[3] + '-' + MONTHS[+m[2] - 1] + '-' + m[1] : ''; }
    function h12(h) { h = h % 12; return h === 0 ? 12 : h; }
    /** "dd-MMM-yyyy hh:mm tt" */
    function dMMMyyyyTime(v) {
        var m = parts(v);
        if (!m) return '';
        if (m[4] === undefined) return dMMMyyyy(v);
        return m[3] + '-' + MONTHS[+m[2] - 1] + '-' + m[1] + ' ' + pad(h12(+m[4])) + ':' + m[5] + ' ' + (+m[4] >= 12 ? 'PM' : 'AM');
    }
    /** "hh:mmm:ss tt" (:1536) — hh:mm:ss tt */
    function timeTt(v) {
        var m = parts(v);
        if (!m || m[4] === undefined) return '';
        return pad(h12(+m[4])) + ':' + m[5] + ':' + (m[6] || '00') + ' ' + (+m[4] >= 12 ? 'PM' : 'AM');
    }
    /** DateTime.Now.ToString("dd-MMM-yyyy HH:mm tt") (:801) — 24-hour clock WITH the AM/PM designator, as written. */
    function nowCaption() {
        var d = new Date();
        return pad(d.getDate()) + '-' + MONTHS[d.getMonth()] + '-' + d.getFullYear() + ' ' + pad(d.getHours()) + ':' + pad(d.getMinutes())
            + ' ' + (d.getHours() >= 12 ? 'PM' : 'AM');
    }
    function msg(e) { return e && e.message ? e.message : String(e); }
    function rights() { return (L && L.rights) || {}; }
    function show(el, on) { if (typeof el === 'string') el = $(el); if (el) el.hidden = !on; }

    async function api(method, url, body) {
        var opt = { method: method, credentials: 'same-origin', headers: { Accept: 'application/json' } };
        if (body !== undefined) { opt.headers['Content-Type'] = 'application/json'; opt.body = JSON.stringify(body); }
        var res = await fetch(url, opt);
        var text = await res.text();
        var data = null;
        try { data = text ? JSON.parse(text) : null; } catch (e) { data = null; }
        if (!res.ok) {
            var err = new Error(data && (data.message || data.error) || ('The request failed (' + res.status + ').'));
            err.status = res.status;
            err.data = data;
            throw err;
        }
        return data;
    }
    /**
     * Busy panel + button lock (countx_purchase_request.js); any refusal is shown as the desktop's MessageBox.
     * Acceptance rule 5: the control is locked at once, shows a spinner (class btn-busy), further clicks are
     * ignored while the request is in flight, and it is released in finally on success AND on failure.
     */
    function act(button, work) {
        if (button && (button.disabled || button.classList.contains('btn-busy'))) return Promise.resolve();
        var runner = window.PurchaseRequest ? window.PurchaseRequest.run.bind(window.PurchaseRequest) : function (b, w) { return w(); };
        if (button) button.classList.add('btn-busy');
        return runner(button || null, async function () {
            try { await work(); } catch (e) { alert(msg(e)); }
            finally { if (button) button.classList.remove('btn-busy'); }
        });
    }

    // ============================================================================ combos

    function opt(value, text, attrs) {
        var a = '';
        if (attrs) Object.keys(attrs).forEach(function (k) { a += ' data-' + k + '="' + esc(attrs[k]) + '"'; });
        return '<option value="' + esc(value) + '"' + a + '>' + esc(text) + '</option>';
    }
    function has(id, v) { return Array.prototype.some.call($(id).options, function (o) { return o.value === String(v); }); }
    /** combo.Value = v — a value that is not in the list leaves the combo without a selection. */
    function setVal(id, v) {
        var el = $(id);
        if (v !== null && v !== undefined && v !== '' && has(id, v)) el.value = String(v); else el.selectedIndex = -1;
    }
    function comboVal(id) { var el = $(id); return el.selectedIndex < 0 ? 0 : intOf(el.value); }
    function comboText(id) { var el = $(id); return el.selectedIndex < 0 ? '' : el.options[el.selectedIndex].textContent; }
    function clearCombo(id) { $(id).selectedIndex = -1; }                               // combo.Text = ""
    /** DataSource rebound; the current value is kept when it is still in the list (no default row: ZeroIndex false). */
    function bind(id, rows, valueKey, textKey, attrFn) {
        var el = $(id), prev = el.selectedIndex < 0 ? null : el.value, html = '';
        (rows || []).forEach(function (r) { html += opt(r[valueKey], r[textKey], attrFn ? attrFn(r) : null); });
        el.innerHTML = html;
        setVal(id, prev);
    }
    function focus(id) { var el = $(id); if (!el) return; var w = el.__dtcombo && el.__dtcombo.input; try { (w || el).focus(); } catch (e) { } }
    /** The element whose focus-out is the desktop's Leave. */
    function leaveTarget(id) { var el = $(id); return (el.__dtcombo && el.__dtcombo.wrap) || el; }

    function bindFormLists(d) {
        if (d.plants) bind('cmbPlantName', d.plants, 'Id', 'Description');                          // PlantFill (:436)
        if (d.jobOrders) bind('cmbproductionNo', d.jobOrders, 'Id', 'PlanCode');                    // JobOrderNoFill (:475)
        if (d.analysisGroups) bind('cmbanalysisGroup', d.analysisGroups, 'Id', 'AnalysisGroupDescription');   // AnalysisGroup (:554)
        if (d.cropYears) bind('cmbCropYear', d.cropYears, 'Id', 'CropYear');                        // CropYearFill (:528)
    }
    /** ItemNameBind (:509) — previous value retained when still listed; an empty list clears the combo (InfragisticsHelper :17). */
    function bindItems(rows) {
        bind('cmbitem', rows || [], 'Id', 'ItemName', function (r) { return { 'item-code': r.ItemCode }; });
    }
    /** HistoryComboFill (:393-396) */
    function bindHistoryCombos(h) {
        h = h || {};
        bind('CmbItemHistory', h.items, 'Id', 'name');
        bind('CmbAnalystHistory', h.analysts, 'Id', 'name');
        bind('CmbJobOrderHistory', h.jobOrders, 'Id', 'name');
        bind('CmbPlantHistory', h.plants, 'Id', 'name');
    }
    /** GetConfigurationsFromGlobalAndBindValuesInColumns (:332) — "Default Crop Year". */
    function applyDefaultCropYear(id) {
        if (intOf(id) !== 0 && $('cmbCropYear').options.length > 0) setVal('cmbCropYear', intOf(id));
    }

    // ============================================================================ Step Analysis grid

    /** grdStepAnalysis.RootTable.Groups.Add("StepName") (:718) — rows are shown (and saved) group by group. */
    function sortSteps(rows) {
        return rows.map(function (r, i) { return { r: r, i: i }; }).sort(function (a, b) {
            var x = String(a.r.StepName).toLowerCase(), y = String(b.r.StepName).toLowerCase();
            return x < y ? -1 : x > y ? 1 : a.i - b.i;
        }).map(function (o) { return o.r; });
    }
    function setSteps(rows) {
        st.steps = sortSteps(rows || []);
        st.steps.forEach(function (r) { r.ResultValue = r.ResultValue === null || r.ResultValue === undefined ? '' : String(r.ResultValue); });
        st.caps = defaultCaps();                                                        // RetrieveStructure: captions back to the column names
    }
    /**
     * grdStepAnalysisSetting (:694). Visible columns in table order: SortNo (60), AnalysisParameter (220),
     * ResultValue (60, editable), Caption1..5, Average (60), RemarksDetail (230, editable); StepName is the
     * group (HideColumnsWhenGrouped); Id, PlantId, StepId, PlantName, AnalysisParameterId hidden.
     * No rows = ClearStructure (:1179 / :1244): an empty grid without headers.
     */
    function renderSteps() {
        var t = $('grdStepAnalysis');
        if (!st.steps.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; return; }
        t.tHead.innerHTML = '<tr><th style="width:60px;">SortNo</th><th style="width:220px;">AnalysisParameter</th><th style="width:60px;">ResultValue</th>'
            + st.caps.map(function (c) { return '<th style="width:100px;">' + esc(c) + '</th>'; }).join('')
            + '<th style="width:60px;">Average</th><th style="width:230px;">RemarksDetail</th><th style="width:26px;"></th></tr>';
        var html = '', prev = null;
        st.steps.forEach(function (r, i) {
            if (r.StepName !== prev) {
                prev = r.StepName;
                html += '<tr class="grp"><td colspan="11">StepName: ' + esc(r.StepName) + '</td></tr>';
            }
            html += '<tr data-i="' + i + '"><td>' + esc(r.SortNo) + '</td><td>' + esc(r.AnalysisParameter) + '</td>'
                + '<td><input type="text" class="c" data-k="ResultValue" data-i="' + i + '" value="' + esc(r.ResultValue === '' ? '' : fmt4(r.ResultValue)) + '"></td>'
                + [1, 2, 3, 4, 5].map(function (n) { return '<td class="c">' + esc(fmt4(r['Caption' + n])) + '</td>'; }).join('')
                + '<td class="c">' + esc(fmt4(r.Average)) + '</td>'
                + '<td><input type="text" data-k="RemarksDetail" data-i="' + i + '" value="' + esc(r.RemarksDetail) + '"></td>'
                /* grdStepAnalysis.AllowDelete = True (designer :3101): the Delete key removes the row; Insert() (:949) saves the rows left. */
                + '<td class="c"><button type="button" class="gbtn" data-del="' + i + '" title="Delete this row (Delete key on the desktop grid)">&times;</button></td></tr>';
        });
        t.tBodies[0].innerHTML = html;
        t.tFoot.innerHTML = '<tr>' + new Array(12).join('<td></td>') + '</tr>';         // TotalRow with no aggregate
    }
    /**
     * TopFiveStepAnalysisResultsAgainstPlantJobOrderAndItem (:755). With rows: the (StepId, AnalysisParameterId)
     * matches fill Caption1..5 / Average (a value that is not > 0 becomes 0), and the five header captions are
     * the first row's dtm_01..05 as "dd-MMM-yyyy hh:mm tt" — a missing one is 01-Jan-1900 to Conversion.ToDateTime,
     * which CheckDateTimeNull rejects, so the caption becomes the current time (:801). Without rows: Caption1..5
     * back to 0 and the captions back to "Caption1".."Caption5" (:808-823; Average is left as it is).
     */
    function applyTop(top) {
        top = top || [];
        if (top.length > 0) {
            st.steps.forEach(function (row) {
                top.forEach(function (tr) {
                    if (intOf(row.StepId) === intOf(tr.StepId) && intOf(row.AnalysisParameterId) === intOf(tr.AnalysisParameterId)) {
                        for (var n = 1; n <= 5; n++) row['Caption' + n] = num(tr['Value_0' + n]) > 0 ? num(tr['Value_0' + n]) : 0;
                        row.Average = num(tr.Average) > 0 ? num(tr.Average) : 0;
                    }
                });
            });
            st.caps = [1, 2, 3, 4, 5].map(function (n) {
                var v = top[0]['dtm_0' + n], m = parts(v);
                if (!m || +m[1] <= 1900) return nowCaption();
                return dMMMyyyyTime(v);
            });
        } else {
            st.steps.forEach(function (row) { for (var n = 1; n <= 5; n++) row['Caption' + n] = 0; });
            st.caps = defaultCaps();
        }
        renderSteps();
    }

    // ============================================================================ Analysis Group grid

    function setGroups(rows) {
        st.groups = rows || [];
        st.groups.forEach(function (r) { r.ResultValue = r.ResultValue === null || r.ResultValue === undefined ? '' : String(r.ResultValue); });
    }
    /**
     * grdAnalysisGroupSetting (:609). Visible: AnalysisParameter (150), MinValue (65), MaxValue (65),
     * ResultValue (65, editable), RemarksDetail (100, editable); Id, AnalysisGroupId, AnalysisParameterId,
     * AnalysisGroupName hidden. At form load the grid is bound to the empty table (:302), so the headers show.
     */
    function renderGroups(cleared) {
        var t = $('grdAnalysisGroup');
        if (cleared) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; return; }
        t.tHead.innerHTML = '<tr><th style="width:150px;">AnalysisParameter</th><th style="width:65px;">MinValue</th><th style="width:65px;">MaxValue</th>'
            + '<th style="width:65px;">ResultValue</th><th style="width:100px;">RemarksDetail</th></tr>';
        t.tBodies[0].innerHTML = st.groups.map(function (r, i) {
            return '<tr data-i="' + i + '"><td>' + esc(r.AnalysisParameter) + '</td><td class="n">' + esc(fmt4(r.MinValue)) + '</td><td class="n">' + esc(fmt4(r.MaxValue)) + '</td>'
                + '<td><input type="text" class="r" data-k="ResultValue" data-i="' + i + '" value="' + esc(r.ResultValue === '' ? '' : fmt4(r.ResultValue)) + '"></td>'
                + '<td><input type="text" data-k="RemarksDetail" data-i="' + i + '" value="' + esc(r.RemarksDetail) + '"></td></tr>';
        }).join('');
        t.tFoot.innerHTML = '<tr><td></td><td></td><td></td><td></td><td></td></tr>';    // TotalRow with no aggregate
    }

    /** grd_CellUpdated (:655) — the typed value goes straight to the table. ResultValue is a double column. */
    function onGridInput(list) {
        return function (e) {
            var k = e.target.getAttribute('data-k'), i = e.target.getAttribute('data-i');
            if (!k || i === null || !list()[+i]) return;
            list()[+i][k] = e.target.value;
        };
    }
    function onGridChange(list) {
        return function (e) {
            var k = e.target.getAttribute('data-k'), i = e.target.getAttribute('data-i');
            if (k !== 'ResultValue' || i === null || !list()[+i]) return;
            var raw = e.target.value.replace(/,/g, '').trim();
            if (raw !== '' && !isFinite(Number(raw))) {                                  // a double column refuses text
                alert('Input string was not in a correct format.');
                raw = '';
            }
            list()[+i].ResultValue = raw;
            e.target.value = raw === '' ? '' : fmt4(raw);
        };
    }

    // ============================================================================ dependent loads

    var seq = { plant: 0, job: 0, group: 0 };

    /** cmbPlantName_Leave (:660) */
    async function plantLeave() {
        var my = ++seq.plant;
        var d = await api('GET', API + '/plant-steps?plantId=' + comboVal('cmbPlantName') + '&jobOrderId=' + comboVal('cmbproductionNo'));
        if (my !== seq.plant) return;                     // a later Leave already answered
        setSteps(d.rows);
        applyTop(d.top);
    }
    /** cmbproductionNo_Leave (:563) */
    async function jobOrderLeave() {
        var my = ++seq.job;
        var d = await api('GET', API + '/job-order?jobOrderId=' + comboVal('cmbproductionNo') + '&plantId=' + comboVal('cmbPlantName'));
        if (my !== seq.job) return;
        bindItems(d.items);
        if (st.steps.length) applyTop(d.top);             // with no Step Analysis structure the desktop has no columns to caption
    }
    /** cmbanalysisGroup_Leave (:576) */
    async function analysisGroupLeave() {
        var my = ++seq.group;
        var rows = await api('GET', API + '/group-parameters?analysisGroupId=' + comboVal('cmbanalysisGroup'));
        if (my !== seq.group) return;
        setGroups(rows);
        renderGroups(false);
    }

    // ============================================================================ pictures

    var PIC = {
        analysis: { box: 'sampleanalysispicture', img: 'sampleanalysispictureImg', file: 'fileAnalysisPic' },
        cooking: { box: 'cookingpic', img: 'cookingpicImg', file: 'fileCookingPic' }
    };
    /** sampleanalysispicture.Image = null (:1298 / :1316 / :1231) */
    function clearPic(which) {
        st.pics[which] = blankPic();
        var img = $(PIC[which].img);
        img.removeAttribute('src'); img.hidden = true;
        $(PIC[which].box).classList.remove('zoom');
        $(PIC[which].file).value = '';
    }
    /** btnimg1_Click / btnimage2_Click (:1730 / :1767) — the choice is dropped first; a cancelled dialog leaves no picture. */
    function browsePic(which) {
        clearPicSelection(which);
        $(PIC[which].file).click();
    }
    function clearPicSelection(which) {
        st.pics[which].fileName = ''; st.pics[which].data = '';
        $(PIC[which].file).value = '';
    }
    function pickedPic(which) {
        var f = $(PIC[which].file).files[0];
        if (!f) { clearPic(which); return; }
        if (!/\.(jpg|jpeg|gif|png)$/i.test(f.name)) { clearPic(which); alert('Select an image (*.jpg; *.jpeg; *.gif; *.png)'); return; }
        var rd = new FileReader();
        rd.onload = function () {
            st.pics[which] = { state: 'new', fileName: f.name, data: String(rd.result) };
            var img = $(PIC[which].img);
            img.src = String(rd.result); img.hidden = false;
            $(PIC[which].box).classList.add('zoom');                                     // SizeMode = Zoom (:1753)
        };
        rd.onerror = function () { clearPic(which); alert('The selected image could not be read.'); };
        rd.readAsDataURL(f);
    }
    /** ReadById (:1111-1156) — the stored picture of the record. */
    function storedPic(which, id, exists) {
        clearPic(which);
        if (!exists) return;
        st.pics[which] = { state: 'keep', fileName: '', data: '' };
        var img = $(PIC[which].img);
        img.onerror = function () { img.hidden = true; img.removeAttribute('src'); };   // File.Exists false: no image shown
        img.src = API + '/' + id + '/picture/' + which + '?t=' + Date.now();
        img.hidden = false;
    }
    /** LabImagesPreview (:1804 / :1811 / :1409) */
    function preview(src) {
        var img = $('dlgPreviewImg');
        if (src) { img.src = src; img.hidden = false; } else { img.removeAttribute('src'); img.hidden = true; }
        show('dlgPreview', true);
    }
    function picPayload(which) {
        var p = st.pics[which];
        return p.state === 'new' ? { state: 'new', fileName: p.fileName, data: p.data } : { state: p.state };
    }

    // ============================================================================ new / refresh / read

    /** refresh() (:1210) */
    async function formNew() {
        var d = await api('GET', API + '/new');
        st.recId = 0;
        seq.plant++; seq.job++; seq.group++;
        $('txtdocno').value = d.docNo;                                                   // GenerateCode
        show('btnSave', true); show('btnUpdate', false);
        clearCombo('cmbitem'); clearCombo('cmbproductionNo');
        $('txtAnalystPerson').value = '';
        clearCombo('cmbanalysisGroup'); clearCombo('cmbCropYear');
        $('txtremarks').value = '';
        st.steps = []; st.groups = []; st.caps = defaultCaps();
        clearPic('analysis'); clearPic('cooking');
        $('cmbanalysisGroup').disabled = false;
        bindFormLists(d);                                                                // JobOrderNoFill, AnalysisGroup, CropYearFill, PlantFill
        renderGroups(true); renderSteps();                                               // ClearStructure x 2
        applyDefaultCropYear(d.defaultCropYear);
        focus('cmbPlantName');
        /* The plant, the Doc Date and the Analysis Time are NOT reset by refresh() — they stay as they are. */
    }
    /** Refresh1() (:1255) */
    async function formRefresh() {
        var d = await api('GET', API + '/refresh?recId=' + st.recId);
        bindFormLists(d);
    }
    /** ReadById (:1081) */
    async function readById(id) {
        var d = await api('GET', API + '/' + intOf(id));
        if (!d || !d.found) return;                                                      // :1092
        st.recId = intOf(id);
        seq.plant++; seq.job++; seq.group++;
        selectTab('tabForm');
        $('txtdocno').value = d.DocNo;
        var dd = parts(d.DocDate);
        if (dd) $('txtdocdate').value = dd[1] + '-' + dd[2] + '-' + dd[3];
        bind('cmbproductionNo', d.jobOrders, 'Id', 'PlanCode');                          // JobOrderNoFill with RecId (:1201)
        setVal('cmbCropYear', d.CropYearId);
        setVal('cmbproductionNo', d.JobOrderId);
        bindItems(d.items);                                                              // :1101
        setVal('cmbanalysisGroup', d.AnalysisGroupId);
        setVal('cmbPlantName', d.PlantId);
        setVal('cmbitem', d.ItemId);
        var at = parts(d.AnalysisTime);
        if (at && at[4] !== undefined) {
            st.analysisDate = at[1] + '-' + at[2] + '-' + at[3];
            $('txtanalysistime').value = at[4] + ':' + at[5] + ':' + (at[6] || '00');
        }
        $('txtremarks').value = d.RemarksHeader;
        $('txtAnalystPerson').value = d.AnalystPerson;
        show('btnSave', false); show('btnUpdate', true);
        $('cmbanalysisGroup').disabled = true;                                           // :1110
        storedPic('analysis', st.recId, d.hasAnalysisPic);
        storedPic('cooking', st.recId, d.hasCookingPic);
        setSteps(d.stepRows);
        setGroups(d.groupRows);
        renderGroups(st.groups.length === 0);                                            // :1190-1199
        applyTop(d.top);                                                                 // :1202 (renders the step grid)
    }

    // ============================================================================ save

    function payload(confirmed) {
        return {
            id: st.recId,
            confirm: confirmed,
            docDate: $('txtdocdate').value,
            analysisTime: (st.analysisDate || today()) + 'T' + ($('txtanalysistime').value || nowTime()),
            plantId: comboVal('cmbPlantName'),
            jobOrderId: comboVal('cmbproductionNo'),
            itemId: comboVal('cmbitem'),
            cropYearId: comboVal('cmbCropYear'),
            analysisGroupId: comboVal('cmbanalysisGroup'),
            analystPerson: $('txtAnalystPerson').value,
            remarksHeader: $('txtremarks').value,
            groupRows: st.groups.map(function (r) {
                return { AnalysisGroupId: r.AnalysisGroupId, AnalysisParameterId: r.AnalysisParameterId, AnalysisParameter: r.AnalysisParameter,
                    ResultValue: r.ResultValue, RemarksDetail: r.RemarksDetail };
            }),
            stepRows: st.steps.map(function (r) {
                return { StepId: r.StepId, StepName: r.StepName, AnalysisParameterId: r.AnalysisParameterId,
                    ResultValue: r.ResultValue, RemarksDetail: r.RemarksDetail };
            }),
            analysisPic: picPayload('analysis'),
            cookingPic: picPayload('cooking')
        };
    }
    /** formvalidation (:831) — the control each message focuses. */
    var FOCUS_AFTER = {
        'Please Select Plant Name': 'cmbPlantName', 'Please Select Job Order No': 'cmbproductionNo', 'Please Select Item': 'cmbitem',
        'Please Select Crop Year': 'cmbCropYear', 'Please Select Analysis Group': 'cmbanalysisGroup', 'Please Enter Analyst Person': 'txtAnalystPerson'
    };
    /**
     * Insert() (:872). The server runs formvalidation, then answers 409 with the desktop's Yes/No text
     * ("Are you sure to Save?" / "Are you sure to Update?"); Yes re-posts with confirm = true.
     */
    async function save(button) {
        return act(button, async function () {
            var d;
            try {
                d = await api('POST', API + '/save', payload(false));
            } catch (e) {
                if (e.status === 409 && e.data && e.data.confirm === true) {
                    if (!confirm(e.data.message)) return;                                // DialogResult.No
                    try { d = await api('POST', API + '/save', payload(true)); }
                    catch (e2) { alert(msg(e2)); if (FOCUS_AFTER[msg(e2)]) focus(FOCUS_AFTER[msg(e2)]); return; }
                } else {
                    alert(msg(e));
                    if (FOCUS_AFTER[msg(e)]) focus(FOCUS_AFTER[msg(e)]);
                    return;
                }
            }
            alert(d.message);                                                            // "Save Successfully" / "Update Successfully"
            await formNew();                                                             // refresh() (:1039)
            if ($('chkPreview').checked && rights().print) await print(d.id);            // :1040
        });
    }
    function btnSaveClick() { st.recId = 0; return save($('btnSave')); }                // btnSave_Click (:1056): RecId = 0
    function btnUpdateClick() { return save($('btnUpdate')); }                          // btnUpdate_Click (:1069)

    /** CommonServices.LabAnalysisReport659(id) — the Jasper print of 659-RptInvLabInProcessAnalysisSlip.rpt. */
    async function print(id) {
        try {
            await api('GET', API + '/' + intOf(id) + '/print-check');
            window.open('/api/print/by-template/' + encodeURIComponent(PRINT_RPT) + '/pdf?id=' + intOf(id), '_blank');
        } catch (e) { alert(msg(e)); }
    }

    // ============================================================================ history

    /**
     * historygridfill (:1465-1487) + grdhistorysetting (:1508). Print (position 0) and Edit (position 1) buttons,
     * then the table's visible columns in order; RecordNo, Id, DocumentTypeId, AnalysisPicValue, CookingPicValue
     * hidden; AnalysisPic / CookingPic / NoOfAttachments are link columns; "Add Attachment" button last.
     * [key, caption, kind, width] — kind: d date, t "hh:mm:ss tt", dt "dd-MMM-yyyy hh:mm tt", pic, att, n number.
     */
    var HIST_COLS = [
        ['DocNo', 'DocNo', 'n', 60], ['DocDate', 'DocDate', 'd', 73], ['PlantName', 'PlantName', '', 100], ['JobOrder', 'JobOrder', '', 120],
        ['ItemName', 'ItemName', '', 170], ['CropYear', 'CropYear', '', 73], ['AnalysisGroup', 'AnalysisGroup', '', 85],
        ['AnalystPerson', 'AnalystPerson', '', 100], ['AnalysisTime', 'AnalysisTime', 't', 110], ['EntryDate', 'EntryDate', 'dt', 145],
        ['EntryUser', 'EntryUser', '', 100], ['ModifyDate', 'ModifyDate', 'dt', 145], ['ModifyUser', 'ModifyUser', '', 100],
        ['AnalysisPic', 'AnalysisPic', 'pic', 60], ['CookingPic', 'CookingPic', 'pic', 60], ['NoOfAttachments', 'NoOfAttachments', 'att', 90],
        ['RemarksHeader', 'RemarksHeader', '', 150]
    ];
    function histText(c, v) {
        if (c[2] === 'd') return dMMMyyyy(v);
        if (c[2] === 't') return timeTt(v);
        if (c[2] === 'dt') return dMMMyyyyTime(v);
        return v === null || v === undefined ? '' : String(v);
    }
    /** FilterMode Automatic, DefaultFilterRowComparison Contains (GridEX_Helper.cs:249). */
    function histVisible() {
        var out = [];
        st.hist.forEach(function (r, i) {
            var ok = HIST_COLS.every(function (c) {
                var f = (st.hFilter[c[0]] || '').toLowerCase();
                return f === '' || histText(c, r[c[0]]).toLowerCase().indexOf(f) >= 0;
            });
            if (ok) out.push(i);
        });
        return out;
    }
    function renderHistory() {
        var t = $('grdhistory');
        if (!st.hist.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; return; }   // ClearStructure (:1499)
        t.tHead.innerHTML = '<tr><th style="width:40px;">Print</th><th style="width:40px;">Edit</th>'
            + HIST_COLS.map(function (c) { return '<th style="min-width:' + c[3] + 'px;">' + esc(c[1]) + '</th>'; }).join('')
            + '<th style="width:105px;">Add Attachment</th></tr>'
            + '<tr class="flt"><td></td><td></td>' + HIST_COLS.map(function (c) {
                return '<td><input type="text" data-f="' + c[0] + '" value="' + esc(st.hFilter[c[0]] || '') + '"></td>';
            }).join('') + '<td></td></tr>';
        t.tBodies[0].innerHTML = histVisible().map(function (i) {
            var r = st.hist[i];
            return '<tr data-i="' + i + '"' + (i === st.histSel ? ' class="is-sel"' : '') + '>'
                + '<td><button type="button" class="gbtn" data-act="Print">Print</button></td>'
                + '<td><button type="button" class="gbtn" data-act="Edit">Edit</button></td>'
                + HIST_COLS.map(function (c) {
                    if (c[2] === 'pic') return '<td class="n"><a class="lnk" data-pic="' + (c[0] === 'CookingPic' ? 'cooking' : 'analysis') + '">' + esc(r[c[0]]) + '</a></td>';
                    if (c[2] === 'att') return '<td class="n"><a class="lnk" data-att="1">' + esc(r[c[0]]) + '</a></td>';
                    if (c[0] === 'DocNo') return '<td class="n"><a class="lnk doc" data-open="1" title="Open this record in the form">' + esc(histText(c, r[c[0]])) + '</a></td>';
                    return '<td' + (c[2] === 'n' ? ' class="n"' : '') + '>' + esc(histText(c, r[c[0]])) + '</td>';
                }).join('')
                + '<td><button type="button" class="gbtn" disabled title="Attachments (DMS) are not ported to the web page">Add Attachment</button></td></tr>';
        }).join('');
        t.tFoot.innerHTML = '<tr>' + new Array(HIST_COLS.length + 4).join('<td></td>') + '</tr>';
    }
    /** A read-only history sub-grid: [key, caption, kind]; null rows = ClearStructure. */
    function renderSub(id, cols, rows) {
        var t = $(id);
        if (!rows || !rows.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; return; }
        t.tHead.innerHTML = '<tr>' + cols.map(function (c) { return '<th' + (c[3] ? ' style="min-width:' + c[3] + 'px;"' : '') + '>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = rows.map(function (r) {
            return '<tr>' + cols.map(function (c) {
                var v = r[c[0]];
                return c[2] === 'n' ? '<td class="n">' + esc(fmt4(v)) + '</td>' : '<td>' + esc(v === null || v === undefined ? '' : v) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        t.tFoot.innerHTML = '<tr>' + new Array(cols.length + 1).join('<td></td>') + '</tr>';
    }
    /** HistoryStepAnalysisGridFill (:1600): Id, PlantId, StepId, ParameterId, SortNo hidden; "Process Step" caption; ResultValue "#,##0.####". */
    var SUB_STEP_COLS = [['PlantName', 'PlantName'], ['ProcessStepName', 'Process Step'], ['AnalysisParameterCode', 'AnalysisParameterCode'],
        ['ResultValue', 'ResultValue', 'n'], ['RemarkDetail', 'RemarkDetail']];
    /** HistoryGroupAnalysisGridFill (:1639): Id, InvLabGroupAnalysisStandardsId, AnalysisParameterID hidden; AnalysisParameterDescription 150. */
    var SUB_GROUP_COLS = [['AnalysisGroupDescription', 'AnalysisGroupDescription'], ['AnalysisParameterDescription', 'AnalysisParameterDescription', '', 150],
        ['MinValue', 'MinValue', 'n'], ['MaxValue', 'MaxValue', 'n'], ['InAnalysisResult', 'InAnalysisResult', 'n'], ['RemarksDetail', 'RemarksDetail']];
    function renderHistoryDetail(d) {
        renderSub('grdStepAnalysisHistory', SUB_STEP_COLS, d ? d.steps : null);
        renderSub('grdGroupAnalysisHistory', SUB_GROUP_COLS, d ? d.groups : null);
    }
    /** grdhistory_SelectionChanged (:1374) */
    async function selectHistoryRow(i) {
        st.histSel = i;
        $('grdhistory').querySelectorAll('tbody tr').forEach(function (tr) { tr.classList.toggle('is-sel', +tr.getAttribute('data-i') === i); });
        if (i < 0 || !st.hist[i]) { renderHistoryDetail(null); return; }
        var d = await api('GET', API + '/' + intOf(st.hist[i].Id) + '/history-detail');
        if (st.histSel === i) renderHistoryDetail(d);
    }
    /** btnShowHistory_Click (:1362) -> historygridfill (:1432) */
    function showHistory() {
        return act($('btnShowHistory'), async function () {
            var q = '?fromChecked=' + $('chkFromDateHistory').checked + '&fromDate=' + encodeURIComponent($('FromDateHistory').value)
                + '&toChecked=' + $('chkToDateHistory').checked + '&toDate=' + encodeURIComponent($('ToDateHistory').value)
                + '&fromDocNo=' + encodeURIComponent(intOf($('FromDocNoHistory').value)) + '&toDocNo=' + encodeURIComponent(intOf($('ToDocNoHistory').value))
                + '&jobOrderId=' + comboVal('CmbJobOrderHistory') + '&plantId=' + comboVal('CmbPlantHistory') + '&itemId=' + comboVal('CmbItemHistory')
                + '&analyst=' + encodeURIComponent(comboText('CmbAnalystHistory'));
            st.hist = await api('GET', API + '/history' + q) || [];
            st.hFilter = {};
            st.histSel = -1;
            renderHistory();
            renderHistoryDetail(null);
            if (st.hist.length) await selectHistoryRow(0);                               // the bound grid selects its first row
        });
    }
    /** btnNewHistory_Click (:1328) — DateTimePicker.Text = "" resets the picker to now and unchecks it. */
    function newHistory() {
        $('FromDateHistory').value = today(); $('chkFromDateHistory').checked = false;
        $('ToDateHistory').value = today(); $('chkToDateHistory').checked = false;
        $('FromDocNoHistory').value = ''; $('ToDocNoHistory').value = '';
        ['CmbJobOrderHistory', 'CmbPlantHistory', 'CmbItemHistory', 'CmbAnalystHistory'].forEach(clearCombo);
        st.hist = []; st.histSel = -1; st.hFilter = {};
        renderHistory(); renderHistoryDetail(null);
    }
    /** btnRefreshHistory_Click (:1350) */
    function refreshHistory() {
        return act($('btnRefreshHistory'), async function () { bindHistoryCombos(await api('GET', API + '/history-combos')); });
    }
    /** CommonServices.GetNoofAttachmentsByRefDocumentTypeID (CommonServices.cs:4552) — AttachmentView, only when the record has attachments. */
    async function showAttachments(id) {
        var rows = await api('GET', API + '/' + intOf(id) + '/attachments');
        if (!rows || !rows.length) return;
        $('grdAttachments').tBodies[0].innerHTML = rows.map(function (r) {
            return '<tr><td>' + esc(r.AttachmentName) + '</td><td>' + esc(r.CustomName) + '</td><td>' + esc(dMMMyyyyTime(r.EntryDate)) + '</td></tr>';
        }).join('');
        show('dlgAttachments', true);
    }
    function onHistoryClick(e) {
        var tr = e.target.closest('tbody tr[data-i]');
        if (!tr) return;
        var i = +tr.getAttribute('data-i'), r = st.hist[i], b = e.target.closest('button[data-act]'), a = e.target.closest('a[data-pic]');
        var open = e.target.closest('a[data-open]'), att = e.target.closest('a[data-att]');
        if (!r) return;
        if (st.histSel !== i) act(null, function () { return selectHistoryRow(i); });
        if (open) { act(open, function () { return readById(r.Id); }); return; }           // web rule 4: the DocNo opens the record (= Edit)
        if (att) { act(att, function () { return showAttachments(r.Id); }); return; }      // grdhistory_LinkClicked "NoOfAttachments" (:1399)
        if (b) {                                                                          // grdhistory_ColumnButtonClick (:1568)
            if (b.getAttribute('data-act') === 'Edit') act(b, function () { return readById(r.Id); });
            else act(b, function () { return print(r.Id); });
        } else if (a) {                                                                   // grdhistory_LinkClicked (:1392)
            preview(API + '/' + intOf(r.Id) + '/picture/' + a.getAttribute('data-pic') + '?t=' + Date.now());
        }
    }

    // ============================================================================ tabs / keys

    function selectTab(id) {
        ['tabForm', 'tabHistory'].forEach(function (t) { $(t).classList.toggle('is-active', t === id); });
        $('tabBtnForm').classList.toggle('is-active', id === 'tabForm');
        $('tabBtnHistory').classList.toggle('is-active', id === 'tabHistory');
        $('btnHistory').innerHTML = id === 'tabHistory' ? '<i class="fa fa-pencil-square-o"></i> Form' : '<i class="fa fa-history"></i> History';
        if (id === 'tabHistory') $('FromDateHistory').focus();                           // tabControl1_SelectedIndexChanged (:1424)
    }
    /** Footer History button (acceptance rule 4): Form <-> History; the first visit runs Show so the list is there. */
    function historyButton() {
        if (!formTab()) { selectTab('tabForm'); return; }
        selectTab('tabHistory');
        if (!st.hist.length) return showHistory();
    }
    function closeFullscreen() {
        var full = document.querySelector('.wf-block.is-full');
        if (full) full.classList.remove('is-full');
        return !!full;
    }
    function formTab() { return $('tabForm').classList.contains('is-active'); }
    function usable(id) { var b = $(id); return !b.hidden && !b.disabled; }

    /** SendKeys.Send("{TAB}") on Enter (:1668) */
    function tabForward(from) {
        var page = $(formTab() ? 'tabForm' : 'tabHistory');
        var list = Array.prototype.filter.call(page.querySelectorAll('input, select, textarea, button'), function (el) {
            return !el.disabled && !el.hidden && el.type !== 'file' && el.offsetParent !== null && el.tabIndex >= 0;
        });
        var i = list.indexOf(from);
        if (i >= 0 && list[i + 1]) list[i + 1].focus();
    }
    /** InvLabPurchaseAnalysis_KeyDown (:1666) */
    function onKey(e) {
        var c = e.ctrlKey, k = e.key;
        if (!$('dlgPreview').hidden) { if (k === 'Escape') show('dlgPreview', false); return; }
        if (!$('dlgAttachments').hidden) { if (k === 'Escape') show('dlgAttachments', false); return; }
        if (k === 'Escape' && closeFullscreen()) { e.preventDefault(); return; }
        if (k === 'Enter' && !c && !e.altKey) {
            var el = e.target;
            /* An open combo grid keeps Enter for itself (countx_desktop_combo.js stops the event there). */
            if (el && /^(INPUT|SELECT|TEXTAREA)$/.test(el.tagName)) { e.preventDefault(); tabForward(el); }
            return;
        }
        if (k === 'Delete' && formTab() && st.stepSel >= 0 && st.steps[st.stepSel] && !/^(INPUT|SELECT|TEXTAREA)$/.test((e.target && e.target.tagName) || '')) {
            st.steps.splice(st.stepSel, 1); st.stepSel = -1; renderSteps(); return;     // Delete key on the current Step Analysis row
        }
        if (c && (k === 's' || k === 'S')) { e.preventDefault(); if (formTab() && usable('btnSave')) btnSaveClick(); }
        else if (c && (k === 'n' || k === 'N')) { if (formTab()) { e.preventDefault(); act($('btnNew'), formNew); } }
        else if (c && (k === 't' || k === 'T')) { e.preventDefault(); selectTab(formTab() ? 'tabHistory' : 'tabForm'); }
        else if ((c && (k === 'e' || k === 'E')) || k === 'Escape') { e.preventDefault(); window.location.href = EXIT_URL; }
        else if (c && (k === 'u' || k === 'U')) { e.preventDefault(); if (formTab() && usable('btnUpdate')) btnUpdateClick(); }
        else if (c && (k === 'p' || k === 'P')) { e.preventDefault(); if (rights().print) act($('btnprint'), function () { return print(st.recId); }); }
    }

    // ============================================================================ wiring

    function wire() {
        $('tabBtnForm').addEventListener('click', function () { selectTab('tabForm'); });
        $('tabBtnHistory').addEventListener('click', function () { selectTab('tabHistory'); });

        $('btnNew').addEventListener('click', function () { act(this, formNew); });                 // btnNew_Click (:1275)
        $('BtnRefresh').addEventListener('click', function () { act(this, formRefresh); });         // BtnRefresh_Click (:1270)
        $('btnSave').addEventListener('click', btnSaveClick);
        $('btnUpdate').addEventListener('click', btnUpdateClick);
        $('btnprint').addEventListener('click', function () { act(this, function () { return print(st.recId); }); });   // btnprint_Click (:1823)
        $('btnHistory').addEventListener('click', function () { var b = this; if (b.classList.contains('btn-busy')) return; b.classList.add('btn-busy'); Promise.resolve(historyButton()).finally(function () { b.classList.remove('btn-busy'); }); });
        document.querySelectorAll('[data-full]').forEach(function (b) {                             // rule 6: grid fullscreen toggle
            b.addEventListener('click', function () { $(b.getAttribute('data-full')).classList.toggle('is-full'); });
        });
        $('dlgAttachmentsClose').addEventListener('click', function () { show('dlgAttachments', false); });

        $('cmbPlantName').addEventListener('change', function () { act(null, plantLeave); });
        $('cmbproductionNo').addEventListener('change', function () { act(null, jobOrderLeave); });
        $('cmbanalysisGroup').addEventListener('change', function () { act(null, analysisGroupLeave); });
        /* Leave with nothing in the grid yet (e.g. the plant kept after New): fill it, as the desktop's Leave does. */
        document.addEventListener('focusout', function (e) {
            if (!L) return;
            if (leaveTarget('cmbPlantName').contains(e.target)) {
                if (!st.steps.length && comboVal('cmbPlantName') > 0) act(null, plantLeave);
            } else if (leaveTarget('cmbanalysisGroup').contains(e.target)) {
                if (!st.groups.length && comboVal('cmbanalysisGroup') > 0 && !$('cmbanalysisGroup').disabled) act(null, analysisGroupLeave);
            }
        });

        document.addEventListener('mousedown', function (e) { if (!(e.target.closest && e.target.closest('#grdStepAnalysis'))) st.stepSel = -1; });
        $('grdStepAnalysis').addEventListener('click', function (e) {                               // row delete (AllowDelete :3101)
            var b = e.target.closest('button[data-del]'), tr = e.target.closest('tbody tr[data-i]');
            if (tr) st.stepSel = +tr.getAttribute('data-i');
            if (!b) return;
            st.steps.splice(+b.getAttribute('data-del'), 1);
            st.stepSel = -1;
            renderSteps();
        });
        $('grdStepAnalysis').addEventListener('input', onGridInput(function () { return st.steps; }));
        $('grdStepAnalysis').addEventListener('change', onGridChange(function () { return st.steps; }));
        $('grdAnalysisGroup').addEventListener('input', onGridInput(function () { return st.groups; }));
        $('grdAnalysisGroup').addEventListener('change', onGridChange(function () { return st.groups; }));

        $('btnimg1').addEventListener('click', function () { browsePic('analysis'); });
        $('btnimage2').addEventListener('click', function () { browsePic('cooking'); });
        $('fileAnalysisPic').addEventListener('change', function () { pickedPic('analysis'); });
        $('fileCookingPic').addEventListener('change', function () { pickedPic('cooking'); });
        $('fileAnalysisPic').addEventListener('cancel', function () { clearPic('analysis'); });     // :1755
        $('fileCookingPic').addEventListener('cancel', function () { clearPic('cooking'); });       // :1792
        $('btnAnalysisPictureClear').addEventListener('click', function () { clearPic('analysis'); });   // :1292
        $('btnCookingPicClear').addEventListener('click', function () { clearPic('cooking'); });         // :1310
        $('btnprv').addEventListener('click', function () { var i = $('sampleanalysispictureImg'); preview(i.hidden ? '' : i.src); });
        $('btnimgprevcookingpic').addEventListener('click', function () { var i = $('cookingpicImg'); preview(i.hidden ? '' : i.src); });
        $('dlgPreviewClose').addEventListener('click', function () { show('dlgPreview', false); });
        $('dlgPreview').addEventListener('mousedown', function (e) { if (e.target === this) show('dlgPreview', false); });

        $('btnShowHistory').addEventListener('click', showHistory);
        $('btnNewHistory').addEventListener('click', function () { act(this, async function () { newHistory(); }); });
        $('btnRefreshHistory').addEventListener('click', refreshHistory);
        $('grdhistory').addEventListener('click', onHistoryClick);
        $('grdhistory').addEventListener('dblclick', function (e) {                                 // grdhistory_DoubleClick (:1555)
            var tr = e.target.closest('tbody tr[data-i]');
            if (tr && !e.target.closest('input,button,a')) { var r = st.hist[+tr.getAttribute('data-i')]; if (r) act(null, function () { return readById(r.Id); }); }
        });
        $('grdhistory').addEventListener('input', function (e) {
            var f = e.target.getAttribute('data-f');
            if (!f) return;
            st.hFilter[f] = e.target.value;
            var pos = e.target.selectionStart;
            renderHistory();
            var again = $('grdhistory').querySelector('input[data-f="' + f + '"]');
            if (again) { again.focus(); try { again.setSelectionRange(pos, pos); } catch (x) { } }
        });
        document.addEventListener('keydown', onKey);
    }

    // ============================================================================ load

    /** InvLabPurchaseAnalysis_Load (:268) */
    async function init() {
        wire();
        try {
            L = await api('GET', API + '/lookups');
        } catch (e) {
            $('rightsNote').textContent = msg(e);
            $('rightsNote').hidden = false;
            document.querySelectorAll('.wf-root button, .wf-root input, .wf-root select, .wf-root textarea').forEach(function (el) { el.disabled = true; });
            return;
        }
        if (L.printTemplate) PRINT_RPT = L.printTemplate;
        var r = rights();
        $('btnSave').disabled = !r.save;                                                 // :276
        $('btnprint').disabled = !r.print;                                               // :277
        $('btnUpdate').disabled = !r.update;                                             // :278
        bindFormLists(L);                                                                // PlantFill, JobOrderNoFill, AnalysisGroup, CropYearFill
        ['cmbPlantName', 'cmbproductionNo', 'cmbanalysisGroup', 'cmbCropYear', 'cmbitem'].forEach(clearCombo);
        $('txtdocno').value = L.docNo;                                                   // GenerateCode
        show('btnSave', true); show('btnUpdate', false);                                 // :290-291
        $('chkPreview').checked = true;                                                  // :292
        /* Designer defaults: both pickers start at Now. */
        var now = parts(L.serverNow) || parts(today() + 'T' + nowTime());
        $('txtdocdate').value = now[1] + '-' + now[2] + '-' + now[3];
        st.analysisDate = now[1] + '-' + now[2] + '-' + now[3];
        $('txtanalysistime').value = now[4] + ':' + now[5] + ':' + (now[6] || '00');
        setGroups([]); renderGroups(false);                                              // :302-304 — bound to the empty table
        renderSteps();                                                                   // grdStepAnalysis has no DataSource yet
        applyDefaultCropYear(L.defaultCropYear);                                         // :321
        var d = new Date(); d.setDate(d.getDate() - 7);
        $('FromDateHistory').value = isoOf(d);                                           // :322
        $('ToDateHistory').value = today();                                              // :323
        bindHistoryCombos(L.history);                                                    // :324
        ['CmbJobOrderHistory', 'CmbPlantHistory', 'CmbItemHistory', 'CmbAnalystHistory'].forEach(clearCombo);
        if (window.DesktopCombo && window.DesktopCombo.refresh) window.DesktopCombo.refresh();
        /* ?id=<record id> — the report pages link here; the record is opened as the History "Edit" button does. */
        var open = intOf(new URLSearchParams(window.location.search).get('id'));
        if (open > 0) await act(null, function () { return readById(open); });
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
}());
