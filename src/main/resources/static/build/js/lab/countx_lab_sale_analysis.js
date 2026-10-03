/* ============================================================================================
 * Screen 161 "Sale Analysis" — Architecture.WinApp.Lab/InvLabSaleAnalysis.cs (ScreenName
 * "InvLabSaleAnalysis"), DocumentTypeId 304. Line references (:n) are InvLabSaleAnalysis.cs.
 *
 * The page follows the form event for event: Load, the gate pass Leave (item list + gate pass
 * boxes), the analysis group Leave (grid rows), New / Refresh / Save / Update (Insert), ReadById,
 * the History tab (Detail / Print / Edit buttons, double click), the two pictures, the attachment
 * dialog, 658-Print and the form's shortcut keys. The server repeats every validation, generates /
 * keeps the Document No and never takes tenancy from this page.
 *
 * The desktop's "Leave" events are wired to "change" (the combos are drawn by
 * countx_desktop_combo.js over a hidden <select>, which never receives focus).
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/lab/sale-analysis';
    var PRINT_RPT = '658-RptInvLabSaleAnalysisSlip.rpt';               // GenerateReport (:1154)
    var EXIT_URL = '/quality';

    /* GatepassNofill (:270): Id (hidden) | GpSrNo | VehicleNo */
    if (window.DesktopCombo) {
        window.DesktopCombo.define('lslGatePass', [
            { caption: 'GpSrNo', flex: 3 },
            { caption: 'VehicleNo', flex: 3, key: 'vehicle' }
        ]);
    }

    var L = null;                                                       // lookups
    var st = {
        recId: 0,                 // RecId
        updateMode: false,        // UpdateMode
        rows: [],                 // grdtable as grd shows it
        hist: [], histFilter: {}, histSel: 0,
        pic: {
            analysis: { mode: 'none', name: '', data: '' },             // fileSavePath / ofd / sampleanalysispicture
            cooking: { mode: 'none', name: '', data: '' }               // fileSavePathcookingpic / ofdcookingpic / cookingpic
        },
        att: { stored: [], files: [] }                                  // the Attachment dialog's list
    };

    // ============================================================================ helpers

    function $(id) { return document.getElementById(id); }
    function esc(v) {
        return String(v === undefined || v === null ? '' : v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function intOf(v) { var n = parseInt(String(v === undefined || v === null ? '' : v).replace(/,/g, ''), 10); return isNaN(n) ? 0 : n; }
    function pad(n) { return String(n).padStart(2, '0'); }
    function today() { var d = new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function msg(e) { return e && e.message ? e.message : String(e); }
    function rights() { return (L && L.rights) || {}; }
    function show(el, on) { if (typeof el === 'string') el = $(el); if (el) el.hidden = !on; }
    function visible(id) { return !$(id).hidden; }

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
            err.confirm = !!(data && data.confirm);
            throw err;
        }
        return data;
    }

    /**
     * Rule 5: the button is disabled at once, shows a spinner (class btn-busy), further clicks are
     * ignored while the request is in flight, and it is re-enabled on success AND on failure.
     * Any refusal is shown as the desktop's MessageBox.
     */
    async function withBusy(btn, work) {
        if (btn) {
            if (btn.getAttribute('data-busy') === '1' || btn.disabled) return;
            btn.setAttribute('data-busy', '1');
            btn.disabled = true;
            btn.classList.add('btn-busy');
        }
        try { await work(); }
        catch (e) { alert(msg(e)); }
        finally {
            if (btn) {
                btn.removeAttribute('data-busy');
                btn.classList.remove('btn-busy');
                btn.disabled = false;
                applyRights();
            }
        }
    }
    /** btnSave / btnUpdate / btnprint .Enabled = the screen rights (:226-228). */
    function applyRights() {
        var r = rights();
        if ($('btnSave').getAttribute('data-busy') !== '1') $('btnSave').disabled = !r.save;
        if ($('btnUpdate').getAttribute('data-busy') !== '1') $('btnUpdate').disabled = !r.update;
        if ($('btnprint').getAttribute('data-busy') !== '1') $('btnprint').disabled = !r.print;
    }

    // ============================================================================ combos

    function opt(value, text, attrs) {
        var a = '';
        if (attrs) Object.keys(attrs).forEach(function (k) { a += ' data-' + k + '="' + esc(attrs[k]) + '"'; });
        return '<option value="' + esc(value) + '"' + a + '>' + esc(text) + '</option>';
    }
    function fill(id, rows, valueKey, textKey, attrFn) {
        var html = '';
        (rows || []).forEach(function (r) { html += opt(r[valueKey], r[textKey], attrFn ? attrFn(r) : null); });
        $(id).innerHTML = html;
        $(id).selectedIndex = -1;                                       // ZeroIndex: false
    }
    function has(id, v) { return Array.prototype.some.call($(id).options, function (o) { return !o.hasAttribute('data-adhoc') && o.value === String(v); }); }
    function dropAdhoc(id) { Array.prototype.slice.call($(id).querySelectorAll('option[data-adhoc],option[data-stored]')).forEach(function (o) { o.remove(); }); }
    /** Conversion.ToInt(combo.Value) — a text that matches no row is 0. */
    function comboVal(id) {
        var el = $(id);
        if (el.selectedIndex < 0 || el.options[el.selectedIndex].hasAttribute('data-adhoc')) return 0;
        return intOf(el.value);
    }
    /** combo.Text */
    function comboText(id) { var el = $(id); return el.selectedIndex < 0 ? '' : el.options[el.selectedIndex].textContent; }
    /** combo.Text = "" */
    function clearCombo(id) { dropAdhoc(id); $(id).selectedIndex = -1; }
    /** combo.DataSource = null; combo.Text = "" */
    function unbindCombo(id) { $(id).innerHTML = ''; $(id).selectedIndex = -1; }
    /** combo.Text = text — the row with that display text; a text that matches no row stays in the box. */
    function setText(id, text) {
        var el = $(id), t = String(text === undefined || text === null ? '' : text).trim();
        dropAdhoc(id);
        if (t === '') { el.selectedIndex = -1; return; }
        for (var i = 0; i < el.options.length; i++) {
            if (el.options[i].textContent.trim().toLowerCase() === t.toLowerCase()) { el.selectedIndex = i; return; }
        }
        el.insertAdjacentHTML('beforeend', '<option value="" data-adhoc="1">' + esc(t) + '</option>');
        el.selectedIndex = el.options.length - 1;
    }
    /** combo.Value = id; combo.Text = text (ReadById :733-739) — the record's own value is shown even when the bound list no longer has it. */
    function setStored(id, value, text) {
        var el = $(id);
        dropAdhoc(id);
        if (intOf(value) === 0 && String(text || '').trim() === '') { el.selectedIndex = -1; return; }
        if (has(id, value)) { el.value = String(value); return; }
        el.insertAdjacentHTML('beforeend', '<option value="' + esc(value) + '" data-stored="1">' + esc(text) + '</option>');
        el.selectedIndex = el.options.length - 1;
    }
    function focusCombo(id) {
        var el = $(id);
        try { if (el.__dtcombo && el.__dtcombo.input) el.__dtcombo.input.focus(); else if (!el.disabled) el.focus(); } catch (e) { /* not focusable */ }
    }

    /** GatepassNofill (:270) — rebound only when the procedure returned rows (:279). */
    function gatepassNofill(rows) {
        if (!rows || !rows.length) return;
        var prev = $('txtgatepassno').selectedIndex < 0 ? null : { v: $('txtgatepassno').value, t: comboText('txtgatepassno'), adhoc: comboVal('txtgatepassno') === 0 };
        fill('txtgatepassno', rows, 'Id', 'GpSrNo', function (r) { return { vehicle: r.VehicleNo }; });
        if (prev) { if (!prev.adhoc && has('txtgatepassno', prev.v)) $('txtgatepassno').value = prev.v; else setText('txtgatepassno', prev.t); }
    }
    /** AnalysisGroup (:379) — rebound only when there are rows (:398). */
    function analysisGroupFill(rows) {
        if (!rows || !rows.length) return;
        var prev = $('cmbanalysisgroup').selectedIndex < 0 ? null : { v: $('cmbanalysisgroup').value, t: comboText('cmbanalysisgroup') };
        fill('cmbanalysisgroup', rows, 'Id', 'Name');
        if (prev) setStored('cmbanalysisgroup', prev.v, prev.t);
    }
    /** ItemFill (:451): rows -> bound, nothing selected; no rows -> Text "", DataSource null. */
    function itemFill(rows) {
        if (rows && rows.length) fill('cmbitem', rows, 'Id', 'Name');
        else unbindCombo('cmbitem');
    }
    /** CropYearFill (:470): bound and Rows[0].Activate() when there are rows. */
    function cropYearFill(rows) {
        if (!rows || !rows.length) return;
        fill('txtcrop', rows, 'Id', 'Name');
        $('txtcrop').selectedIndex = 0;
    }

    // ===================================================================== form events

    /** txtgatepassno_Leave (:305). */
    async function gatepassLeave() {
        var el = $('txtgatepassno');
        var activeRow = el.selectedIndex >= 0 && !el.options[el.selectedIndex].hasAttribute('data-adhoc');
        if (!activeRow) {                                               // ActiveRow == null (:332)
            $('txtsupcode').value = ''; $('txtbiltyno').value = ''; $('txtvehicleno').value = '';
            return;
        }
        var id = comboVal('txtgatepassno');
        var res = await api('GET', API + '/gate-pass?id=' + id);
        if (comboVal('txtgatepassno') !== id) return;                   // the selection changed meanwhile
        itemFill(res.items);                                            // ItemFill() (:311)
        if (res.found) {
            $('txtsupcode').value = res.SupplierContractCode || '';
            $('txtbiltyno').value = res.BiltyNo || '';
            $('txtvehicleno').value = res.VehicleNo || '';
            $('txtcontainerno').value = res.Container || '';
        } else {                                                        // :324-329
            $('txtsupcode').value = ''; $('txtbiltyno').value = ''; $('txtvehicleno').value = '';
        }
    }
    /** cmbanalysisgroup_Leave (:412) — grdtable.Clear(), then one row per parameter of the group. */
    async function analysisGroupLeave() {
        var id = comboVal('cmbanalysisgroup');
        var rows = await api('GET', API + '/parameters?groupId=' + id);
        if (comboVal('cmbanalysisgroup') !== id) return;
        st.rows = rows || [];
        bindGrid();
    }

    // ============================================================================= grd

    var GRD_COLS = ['AnalysisPerameter', 'Min', 'MaxValue', 'AnalysisResult', 'Remarks'];   // LabstandardanalysisId hidden (:849)

    /** grd.DataSource = grdtable; RetrieveStructure(); grdsetting() (:840): the first three columns are read-only. */
    function bindGrid() {
        var t = $('grd');
        if (!st.rows.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; return; }
        t.tHead.innerHTML = '<tr><th style="width:34%;">AnalysisPerameter</th><th style="width:9%;">Min</th><th style="width:9%;">MaxValue</th>'
            + '<th style="width:14%;">AnalysisResult</th><th>Remarks</th></tr>';
        var html = '';
        st.rows.forEach(function (r, i) {
            html += '<tr data-i="' + i + '"><td class="ro">' + esc(r.AnalysisPerameter) + '</td><td class="ro">' + esc(r.Min) + '</td><td class="ro">' + esc(r.MaxValue) + '</td>'
                + '<td><input type="text" data-f="AnalysisResult" value="' + esc(r.AnalysisResult) + '"></td>'
                + '<td><input type="text" data-f="Remarks" maxlength="250" value="' + esc(r.Remarks) + '"></td></tr>';
        });
        t.tBodies[0].innerHTML = html;
    }
    function readOnlyGrid(id, rows) {
        var t = $(id);
        if (!rows || !rows.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; return; }
        t.tHead.innerHTML = '<tr><th style="width:34%;">AnalysisPerameter</th><th style="width:9%;">Min</th><th style="width:9%;">MaxValue</th>'
            + '<th style="width:14%;">AnalysisResult</th><th>Remarks</th></tr>';
        var html = '';
        rows.forEach(function (r) {
            html += '<tr>';
            GRD_COLS.forEach(function (c) { html += '<td' + (c === 'Remarks' ? ' class="wrap"' : '') + '>' + esc(r[c]) + '</td>'; });
            html += '</tr>';
        });
        t.tBodies[0].innerHTML = html;
    }

    // ======================================================================== pictures

    function picEls(which) {
        return which === 'analysis'
            ? { img: $('sampleanalysispicture'), file: $('fileAnalysisPic') }
            : { img: $('cookingpic'), file: $('fileCookingPic') };
    }
    /** PictureBox.Image = null; Invalidate() */
    function picClear(which) {
        var e = picEls(which);
        st.pic[which] = { mode: 'none', name: '', data: '' };
        e.img.removeAttribute('src');
        e.img.hidden = true;
        e.file.value = '';
    }
    /** ReadById (:743-784): the stored picture, otherwise none. */
    function picStored(which, id, exists) {
        picClear(which);
        if (!exists) return;
        var e = picEls(which);
        e.img.onerror = function () { picClear(which); };              // File.Exists false (:749 / :769)
        st.pic[which] = { mode: 'keep', name: '', data: '' };
        e.img.src = API + '/' + id + '/picture/' + which + '?t=' + Date.now();
        e.img.hidden = false;
    }
    /** btnimg1_Click (:1043) / btnimage2_Click (:1070): definition() first, then the file dialog. */
    function picBrowse(which) {
        if (!L || !L.attachmentPathSet) { alert('Please Map the Path in Configration'); return; }   // :1034
        var e = picEls(which);
        e.file.value = '';
        e.file.click();
    }
    function picChosen(which) {
        var e = picEls(which), f = e.file.files && e.file.files[0];
        if (!f) return;                                                 // dialog cancelled: nothing changes
        if (!/\.(jpg|jpeg|png)$/i.test(f.name)) { alert('Only .jpg, .jpeg and .png pictures can be attached'); return; }
        var reader = new FileReader();
        reader.onload = function () {
            st.pic[which] = { mode: 'new', name: f.name, data: String(reader.result) };
            e.img.onerror = null;
            e.img.src = String(reader.result);
            e.img.hidden = false;
        };
        reader.onerror = function () { alert('The selected image could not be read.'); };
        reader.readAsDataURL(f);
    }
    /** btnprv_Click (:1114) / btnimgprevcookingpic_Click (:1121) — LabImagesPreview with the box's image. */
    function picView(which) {
        var e = picEls(which);
        var img = $('pictureBox1');
        if (e.img.hidden || !e.img.getAttribute('src')) img.removeAttribute('src'); else img.src = e.img.src;
        show('dlgPreview', true);
    }

    // ===================================================================== attachments

    function attachCount() {
        var n = st.att.stored.length + st.att.files.length;
        $('attachmentCount').textContent = n ? '(' + n + ')' : '';
    }
    function attachRender() {
        var html = '';
        st.att.stored.forEach(function (a, i) {
            html += '<tr><td class="wrap">' + esc(a.Name) + '</td><td>' + esc(a.EntryDate || '') + '</td><td class="c"><button type="button" class="gbtn" data-rm="s' + i + '">Remove</button></td></tr>';
        });
        st.att.files.forEach(function (a, i) {
            html += '<tr><td class="wrap">' + esc(a.name) + '</td><td>(new)</td><td class="c"><button type="button" class="gbtn" data-rm="f' + i + '">Remove</button></td></tr>';
        });
        $('grdAttachments').tBodies[0].innerHTML = html || '<tr><td colspan="3" class="lsl-empty">No attachment</td></tr>';
        attachCount();
    }
    function attachBrowse() {
        if (!L || !L.attachmentPathSet) { alert('Please Map the Path in Configration'); return; }
        $('fileAttachments').value = '';
        $('fileAttachments').click();
    }
    function attachChosen() {
        var files = Array.prototype.slice.call($('fileAttachments').files || []);
        files.forEach(function (f) {
            var reader = new FileReader();
            reader.onload = function () { st.att.files.push({ name: f.name, base64: String(reader.result) }); attachRender(); };
            reader.onerror = function () { alert('The attachment could not be read'); };
            reader.readAsDataURL(f);
        });
    }

    // ============================================================================ form

    /** formvalidation (:495). */
    function formvalidation() {
        if (comboText('txtgatepassno').trim() === '') { alert('Please Select GatePass No'); focusCombo('txtgatepassno'); return false; }
        if ($('txtAnalystName').value.trim() === '') { alert('Please Select Analyst Name'); $('txtAnalystName').focus(); return false; }
        if (comboText('cmbanalysisgroup').trim() === '') { alert('Please Select Analysis Group No'); focusCombo('cmbanalysisgroup'); return false; }
        return true;
    }
    function payload(id) {
        function pic(which) { var p = st.pic[which]; return p.mode === 'new' ? { name: p.name, base64: p.data } : null; }
        return {
            id: id,
            docDate: $('txtdocdate').value,
            gatePassId: comboVal('txtgatepassno'),
            gatePassText: comboText('txtgatepassno'),
            itemId: comboVal('cmbitem'),
            cropText: comboText('txtcrop'),
            analysisGroupId: comboVal('cmbanalysisgroup'),
            analysisGroupText: comboText('cmbanalysisgroup'),
            analystName: $('txtAnalystName').value,
            analyzedBags: $('txtAnalyzedBags').value,
            containerNo: $('txtcontainerno').value,
            remarks: $('txtremarks').value,
            accepted: $('chkisaccepted').checked,
            details: st.rows.map(function (r) { return { standardsId: intOf(r.LabstandardanalysisId), result: String(r.AnalysisResult === null || r.AnalysisResult === undefined ? '' : r.AnalysisResult), remarks: r.Remarks || '' }; }),
            analysisPic: pic('analysis'),
            keepAnalysisPic: st.pic.analysis.mode === 'keep',
            cookingPic: pic('cooking'),
            keepCookingPic: st.pic.cooking.mode === 'keep',
            files: id > 0 || st.att.files.length ? st.att.files.map(function (f) { return { name: f.name, base64: f.base64 }; }) : [],
            keepAttachmentIds: id > 0 ? st.att.stored.map(function (a) { return intOf(a.Id); }) : [],
            confirm: true
        };
    }
    /** Insert() (:584). */
    async function insert() {
        if (!formvalidation()) return;
        var recId = st.recId;
        if (!window.confirm(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;      // :600 / :607
        if (!st.rows.length) { alert('Grid Record Not Found'); return; }                                   // :671
        var res = await api('POST', API + '/save', payload(recId));
        alert(res.message);                                                                                // :663
        if ($('chkprint').checked) {                                                                       // :681
            try { await generateReport(res.id, false); } catch (e) { alert(msg(e)); }
        }
        await formNew();                                                                                   // :685
        focusCombo('txtgatepassno');
        await historygridfill();                                                                           // :687
        st.updateMode = true;                                                                              // :688
    }
    /** btnSave_Click (:573). */
    function saveClick() { st.recId = 0; return insert(); }
    /** btnUpdate_Click (:700). */
    function updateClick() {
        if (st.recId === 0) throw new Error('Record Not Update beacause Record Id not found');
        return insert();
    }

    /** New() (:518). RecId is left as it is, as on the desktop; Save resets it. */
    async function formNew() {
        var res = await api('GET', API + '/new');
        $('txtdocno').value = String(res.docNo);                                                           // GenerateCode
        st.updateMode = false;
        st.rows = [];
        bindGrid();
        show('btnSave', true);
        show('btnUpdate', false);
        clearCombo('txtgatepassno');
        $('txtbiltyno').value = '';
        $('txtvehicleno').value = '';
        $('txtsupcode').value = '';
        clearCombo('cmbitem');
        clearCombo('cmbanalysisgroup');
        $('txtAnalyzedBags').value = '';
        $('txtcontainerno').value = '';
        $('txtremarks').value = '';
        $('txtAnalystName').value = '';
        picClear('cooking');
        picClear('analysis');
        st.att = { stored: [], files: [] };
        attachCount();
        $('cmbitem').disabled = false;
        itemFill(res.items);                                                                               // ItemFill()
        cropYearFill(res.cropYears);                                                                       // CropYearFill()
        $('cmbanalysisgroup').disabled = false;
        $('txtgatepassno').disabled = false;
    }
    /** Refresh1 (:554). */
    async function refresh1() {
        var res = await api('GET', API + '/refresh');
        analysisGroupFill(res.analysisGroups);
        st.hist = res.history || [];
        renderHistory();
        gatepassNofill(res.gatePasses);
        var crop = comboText('txtcrop');
        cropYearFill(res.cropYears);
        if (visible('btnUpdate')) setText('txtcrop', crop);             // an opened record keeps its own boxes (W3)
        else await gatepassLeave();                                     // txtgatepassno_Leave(null, null)
    }

    /** ReadById (:714). */
    async function readById(id) {
        id = intOf(id);
        if (id <= 0) return;
        var h = await api('GET', API + '/' + id);
        show('btnSave', false);
        show('btnUpdate', true);
        st.recId = id;
        selectTab('tabForm', true);                                                                        // tabControl1.SelectedIndex = 0
        $('txtdocno').value = h.DocNo;
        if (h.DocDate) $('txtdocdate').value = h.DocDate;                                                  // W4
        $('txtbiltyno').value = h.BiltyNo;
        $('txtvehicleno').value = h.VehicleNo;
        setText('txtgatepassno', h.GpSrNo);
        $('txtsupcode').value = h.SupCustCode;
        $('chkisaccepted').checked = !!h.IsAccepted;
        itemFill(h.items);
        setStored('cmbitem', h.ItemId, h.ItemName);
        setText('txtcrop', h.CropYear);
        $('txtcontainerno').value = h.ContainerNo1;
        setStored('cmbanalysisgroup', h.AnalysisGroupId, h.AnalysisGroupDescription);
        $('txtremarks').value = h.RemarksHeader;
        $('txtAnalystName').value = h.AnalystName;
        $('txtAnalyzedBags').value = h.AnalyzedBags;
        $('txtgatepassno').disabled = true;                                                                // :741
        picStored('analysis', id, h.HasAnalysisPic);
        picStored('cooking', id, h.HasCookingPic);
        st.rows = h.rows || [];
        bindGrid();
        st.att = { stored: h.attachments || [], files: [] };                                               // W6
        attachCount();
    }

    /**
     * GenerateReport (:1133) — 658-RptInvLabSaleAnalysisSlip.rpt through the Jasper print endpoint.
     * The server answers "Not Record Found For Display" when the slip procedure has no row.
     */
    async function generateReport(id, toolbar) {
        id = intOf(id);
        if (id <= 0) throw new Error('Not Record Found For Display');                                      // W7
        await api('GET', API + '/' + id + '/print-check' + (toolbar ? '?toolbar=true' : ''));
        window.open('/reports/print/by-template/' + encodeURIComponent(PRINT_RPT) + '/pdf?id=' + id, '_blank');
    }

    // ========================================================================== history

    /* grdhistorysetting (:907): Detail, Print, Edit buttons first, then the columns with their widths; Id hidden. */
    var HIST_COLS = [
        { k: 'Detail', btn: 'Detail', w: 60 }, { k: 'Print', btn: 'Print', w: 50 }, { k: 'Edit', btn: 'Edit', w: 50 },
        { k: 'DocNo', w: 60, link: true }, { k: 'AnalysisItem', w: 250 }, { k: 'AnlysisGroup', w: 250 }, { k: 'GatePass#', w: 80 },
        { k: 'CustomerCode', w: 130 }, { k: 'BiltyNo', w: 70 }, { k: 'VehicleNo', w: 100 }, { k: 'CropYear', w: 120 },
        { k: 'EntryUser', w: 150 }, { k: 'Remarks', w: 300 }
    ];
    function histVisible() {
        return st.hist.filter(function (r) {
            return HIST_COLS.every(function (c) {
                var f = (st.histFilter[c.k] || '').trim().toLowerCase();
                return c.btn || f === '' || String(r[c.k] === null || r[c.k] === undefined ? '' : r[c.k]).toLowerCase().indexOf(f) >= 0;
            });
        });
    }
    function renderHistoryHead() {
        var h = '<tr>', f = '<tr class="flt">';
        HIST_COLS.forEach(function (c) {
            h += '<th style="min-width:' + c.w + 'px;">' + esc(c.btn || c.k) + '</th>';
            f += '<th>' + (c.btn ? '' : '<input type="text" data-k="' + esc(c.k) + '" value="' + esc(st.histFilter[c.k] || '') + '">') + '</th>';
        });
        $('grdhistory').tHead.innerHTML = h + '</tr>' + f + '</tr>';
    }
    function renderHistoryBody() {
        var html = '';
        histVisible().forEach(function (r) {
            html += '<tr data-id="' + intOf(r.Id) + '"' + (intOf(r.Id) === st.histSel ? ' class="is-sel"' : '') + '>';
            HIST_COLS.forEach(function (c) {
                if (c.btn) html += '<td class="c"><button type="button" class="gbtn" data-act="' + c.k + '">' + esc(c.btn) + '</button></td>';
                else if (c.link) html += '<td><a class="lnk" data-act="Open" title="Open this document">' + esc(r[c.k]) + '</a></td>';
                else html += '<td' + (c.k === 'Remarks' ? ' class="wrap"' : '') + '>' + esc(r[c.k]) + '</td>';
            });
            html += '</tr>';
        });
        $('grdhistory').tBodies[0].innerHTML = html;
    }
    function renderHistory() { st.histFilter = {}; renderHistoryHead(); renderHistoryBody(); }
    /** historygridfill (:860) — the last 50 documents. */
    async function historygridfill() {
        st.hist = (await api('GET', API + '/history')) || [];
        renderHistory();
    }
    /** grdhistory_ColumnButtonClick "Detail" (:818). */
    async function historyDetail(id) {
        var h = await api('GET', API + '/' + id);
        readOnlyGrid('grdHistoryDetail', h.rows);
    }

    // ================================================================================ wiring

    /** tabControl1.SelectedIndex = n; tabControl1_SelectedIndexChanged (:1109) refills the history on every change. */
    function selectTab(name, quiet) {
        var changed = !$(name).classList.contains('is-active');
        ['tabForm', 'tabHistory'].forEach(function (p) { $(p).classList.toggle('is-active', p === name); });
        $('tabBtnForm').classList.toggle('is-active', name === 'tabForm');
        $('tabBtnHistory').classList.toggle('is-active', name === 'tabHistory');
        $('btnHistory').innerHTML = name === 'tabHistory' ? '<i class="fa fa-pencil-square-o"></i> Form' : '<i class="fa fa-history"></i> History';
        if (!changed) return Promise.resolve();
        var p = historygridfill();
        if (quiet) p = p.catch(function () { /* the record is already on screen */ });
        return p;
    }
    function onFormTab() { return $('tabForm').classList.contains('is-active'); }
    function closeForm() { window.location.href = EXIT_URL; }

    function wire() {
        // ---- btn (ToolStrip)
        $('btnNew').addEventListener('click', function () { withBusy(this, formNew); });                    // :563
        $('txtRefresh').addEventListener('click', function () { withBusy(this, refresh1); });               // :568
        $('btnSave').addEventListener('click', function () { withBusy(this, saveClick); });                 // :573
        $('btnUpdate').addEventListener('click', function () { withBusy(this, updateClick); });             // :700
        $('btnprint').addEventListener('click', function () { withBusy(this, function () { return generateReport(st.recId, true); }); });   // :1097
        $('btnattachment').addEventListener('click', function () { attachRender(); show('dlgAttachments', true); });                         // :1128

        // ---- combos
        $('txtgatepassno').addEventListener('change', function () { withBusy(null, gatepassLeave); });      // :305
        $('cmbanalysisgroup').addEventListener('change', function () { withBusy(null, analysisGroupLeave); });   // :412

        // ---- grd_CellUpdated (:579) — grd.UpdateData()
        $('grd').addEventListener('input', function (e) {
            var f = e.target.getAttribute && e.target.getAttribute('data-f');
            if (!f) return;
            var r = st.rows[intOf(e.target.closest('tr').getAttribute('data-i'))];
            if (r) r[f] = e.target.value;
        });

        // ---- pictures
        $('btnimg1').addEventListener('click', function () { picBrowse('analysis'); });
        $('btnimage2').addEventListener('click', function () { picBrowse('cooking'); });
        $('fileAnalysisPic').addEventListener('change', function () { picChosen('analysis'); });
        $('fileCookingPic').addEventListener('change', function () { picChosen('cooking'); });
        $('btnprv').addEventListener('click', function () { picView('analysis'); });
        $('btnimgprevcookingpic').addEventListener('click', function () { picView('cooking'); });
        $('dlgPreviewClose').addEventListener('click', function () { show('dlgPreview', false); $('pictureBox1').removeAttribute('src'); });

        // ---- Attachment dialog
        $('dlgAttachmentsClose').addEventListener('click', function () { show('dlgAttachments', false); });
        $('btnAttachOk').addEventListener('click', function () { show('dlgAttachments', false); });
        $('btnAttachBrowse').addEventListener('click', attachBrowse);
        $('fileAttachments').addEventListener('change', attachChosen);
        $('grdAttachments').addEventListener('click', function (e) {
            var b = e.target.closest('button[data-rm]');
            if (!b) return;
            var k = b.getAttribute('data-rm'), i = intOf(k.substring(1));
            if (k.charAt(0) === 's') st.att.stored.splice(i, 1); else st.att.files.splice(i, 1);
            attachRender();
        });

        // ---- tabs + footer History button
        $('tabBtnForm').addEventListener('click', function () { withBusy(null, function () { return selectTab('tabForm'); }); });
        $('tabBtnHistory').addEventListener('click', function () { withBusy(null, function () { return selectTab('tabHistory'); }); });
        $('btnHistory').addEventListener('click', function () {
            withBusy(this, function () { return selectTab(onFormTab() ? 'tabHistory' : 'tabForm'); });
        });

        // ---- fullscreen toggles on the grid captions
        Array.prototype.forEach.call(document.querySelectorAll('button[data-full]'), function (b) {
            b.addEventListener('click', function () { $(b.getAttribute('data-full')).classList.toggle('is-full'); });
        });

        // ---- grdhistory: filter row, Detail / Print / Edit, the DocNo link, double click
        $('grdhistory').addEventListener('input', function (e) {
            var k = e.target.getAttribute && e.target.getAttribute('data-k');
            if (!k) return;
            st.histFilter[k] = e.target.value;
            renderHistoryBody();
        });
        $('grdhistory').addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr');
            if (!tr) return;
            var id = intOf(tr.getAttribute('data-id'));
            st.histSel = id;
            Array.prototype.forEach.call($('grdhistory').tBodies[0].rows, function (x) { x.classList.toggle('is-sel', x === tr); });
            var el = e.target.closest('[data-act]'), k = el ? el.getAttribute('data-act') : '';
            if (!k) return;
            withBusy(el.tagName === 'BUTTON' ? el : null, async function () {
                if (k === 'Edit' || k === 'Open') await readById(id);                                       // :810
                else if (k === 'Print') await generateReport(id, false);                                    // :815
                else if (k === 'Detail') await historyDetail(id);                                           // :818
            });
        });
        $('grdhistory').addEventListener('dblclick', function (e) {                                         // grdhistory_DoubleClick (:709)
            var tr = e.target.closest('tbody tr');
            if (!tr || e.target.closest('[data-act]')) return;
            withBusy(null, function () { return readById(intOf(tr.getAttribute('data-id'))); });
        });

        // ---- InvLabPurchaseAnalysis_KeyDown (:971)
        document.addEventListener('keydown', function (e) {
            if (e.defaultPrevented) return;                        // an open combo drop-down handled the key
            var c = e.ctrlKey || e.metaKey, k = e.key;
            if (k === 'Enter' && !c && !e.altKey) {
                var a = document.activeElement;
                if (a && a.tagName === 'INPUT' && a.type !== 'file') { e.preventDefault(); focusNext(a); }  // SendKeys.Send("{TAB}")
                return;
            }
            if (c && (k === 's' || k === 'S')) { e.preventDefault(); if (visible('btnSave') && onFormTab()) withBusy($('btnSave'), saveClick); }
            else if (c && (k === 'n' || k === 'N')) { e.preventDefault(); if (onFormTab()) withBusy($('btnNew'), formNew); }
            else if (c && (k === 't' || k === 'T')) { e.preventDefault(); withBusy(null, function () { return selectTab(onFormTab() ? 'tabHistory' : 'tabForm'); }); }
            else if ((c && (k === 'e' || k === 'E')) || k === 'Escape') {
                if (!$('dlgPreview').hidden) { show('dlgPreview', false); return; }
                if (!$('dlgAttachments').hidden) { show('dlgAttachments', false); return; }
                var full = document.querySelector('.lsl-block.is-full');
                if (full) { full.classList.remove('is-full'); return; }
                e.preventDefault(); closeForm();                                                            // Close()
            }
            else if (c && (k === 'u' || k === 'U')) { e.preventDefault(); if (visible('btnUpdate')) withBusy($('btnUpdate'), updateClick); }
            else if (c && (k === 'p' || k === 'P')) { e.preventDefault(); withBusy($('btnprint'), function () { return generateReport(st.recId, true); }); }
        });
    }
    function focusNext(a) {
        var page = a.closest('.lsl-page') || document;
        var list = Array.prototype.filter.call(page.querySelectorAll('input:not([type=hidden]):not([type=file]):not([disabled]), button.wf-btn:not([disabled])'),
            function (el) { return el.offsetParent !== null; });
        var i = list.indexOf(a);
        if (i >= 0 && i + 1 < list.length) list[i + 1].focus();
    }

    /** InvLabPurchaseAnalysis_Load (:221). */
    async function load() {
        $('txtdocdate').value = today();
        try {
            L = await api('GET', API + '/lookups');
        } catch (e) {
            $('rightsNote').textContent = msg(e);
            show('rightsNote', true);
            Array.prototype.forEach.call(document.querySelectorAll('button.wf-tool, button.wf-btn, button.lsl-foot-btn'), function (b) { b.disabled = true; });
            return;
        }
        applyRights();                                                                                      // :226-228
        show('btnSave', true);                                                                              // :232
        show('btnUpdate', false);
        itemFill(L.items);                                                                                  // ItemFill
        $('txtdocno').value = String(L.docNo);                                                              // GenerateCode
        analysisGroupFill(L.analysisGroups);                                                                // AnalysisGroup
        st.hist = L.history || [];                                                                          // historygridfill
        renderHistory();
        gatepassNofill(L.gatePasses);                                                                       // GatepassNofill
        cropYearFill(L.cropYears);                                                                          // CropYearFill
        bindGrid();
        attachCount();
        wire();
        /* ?id=<record id> - the Lab Sale Analysis Report links here; opened as History "Edit" does. */
        var openId = parseInt(new URLSearchParams(window.location.search).get('id'), 10);
        if (openId > 0) withBusy(null, function () { return readById(openId); });
        focusCombo('txtgatepassno');                                                                        // :230
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', load); else load();
}());
