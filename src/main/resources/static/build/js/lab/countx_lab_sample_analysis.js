/* ============================================================================================
 * Screen 159 "Sample Analysis" — Architecture.WinApp.Lab/InvLabSampleAnalysis.cs (ScreenName
 * "InvLabSampleAnalysis"), DocumentTypeId 302. Line references (:n) are InvLabSampleAnalysis.cs.
 *
 * The page follows the form event for event: the combo chain (Analysis Type -> Analysis Group ->
 * detail grid + items; Supplier / Item -> Purchase Order + Sample Log No; Purchase Order -> items),
 * Insert() with its validations, ReadById, refresh(), the History tab and Print657. The server
 * repeats every validation, generates / keeps the Doc No and never takes tenancy from this page.
 *
 * The desktop's "Leave" events are wired to "change" (the combos are drawn by
 * countx_desktop_combo.js over a hidden <select>, which never receives focus).
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/lab/sample-analysis';
    var PRINT_RPT = '657-RptInvLabSampleAnalysisSlipA.rpt';            // Print657 (:1920)
    var MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var DEFAULT_TEXT = '-- Select --';                                  // InfragisticsHelper.InsertDefaultRow

    /* Form-specific combo columns (countx_desktop_combo.js). DropDownBind.BindDDL binds the whole
       table and hides column 0 only, so the other columns of the procedure show in the drop-down. */
    if (window.DesktopCombo) {
        /* cmbsupplierfill (:467): Id | CompanyName | GlAccountId | PartyCode */
        window.DesktopCombo.define('lsaParty', [
            { caption: 'Supplier Customer', flex: 4 },
            { caption: 'GlAccountId', flex: 2, key: 'gl' },
            { caption: 'PartyCode', flex: 2, key: 'code' }
        ]);
        /* ItemFill (:600): Id | ItemName | ItemCategory | ItemCode | InventoryParentCategoriesId */
        window.DesktopCombo.define('lsaItem', [
            { caption: 'Item', flex: 4 },
            { caption: 'ItemCategory', flex: 3, key: 'item-category' },
            { caption: 'ItemCode', flex: 2, key: 'item-code' },
            { caption: 'InventoryParentCategoriesId', flex: 2, key: 'parent-category' }
        ]);
        /* AnalysisGroupBind (:584, AllColumns): Id | AnalysisGroupDescription | GroupTypeId | GroupType */
        window.DesktopCombo.define('lsaGroup', [
            { caption: 'Analysis Group', flex: 4 },
            { caption: 'GroupTypeId', flex: 1, key: 'group-type-id' },
            { caption: 'GroupType', flex: 3, key: 'group-type' }
        ]);
    }

    var L = null;                                                       // lookups
    var st = {
        recId: 0,                 // RecId
        updateMode: false,        // UpdateMode
        details: [],              // detaillst
        subOf: -1,                // index of the detail row whose sub list grdSubItems shows
        hist: [], histSel: 0, histFilter: {},
        histDetail: null,
        pic: {
            analysis: { mode: 'none', name: '', data: '' },             // fileSavePath / ofd / PicBAnalysisImage
            cooking: { mode: 'none', name: '', data: '' }               // fileSavePathcookingpic / ofdcookingpic / PicBCookingImage
        }
    };

    // ============================================================================ helpers

    function $(id) { return document.getElementById(id); }
    function esc(v) {
        return String(v === undefined || v === null ? '' : v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    /** Conversion.ToDouble on text — thousands separators allowed, anything else 0. */
    function num(v) {
        if (v === null || v === undefined) return 0;
        if (typeof v === 'number') return isFinite(v) ? v : 0;
        var s = String(v).replace(/,/g, '').trim();
        if (s === '') return 0;
        var n = Number(s);
        return isFinite(n) ? n : 0;
    }
    function intOf(v) { var n = parseInt(String(v === undefined || v === null ? '' : v).replace(/,/g, ''), 10); return isNaN(n) ? 0 : n; }
    function roundAway(x, n) {
        var f = Math.pow(10, n || 0);
        return Math.sign(x) * Math.round(Math.abs(x) * f + 1e-9) / f;
    }
    /** double.ToString() */
    function netStr(v) {
        if (v === null || v === undefined || v === '' || !isFinite(v)) return '';
        return String(parseFloat(Number(v).toPrecision(15)));
    }
    /** "#,##0.###" (GridEX_Helper.GridColumnSettings, DecimalCount 3 — :957) */
    function fmt3(v) { return roundAway(num(v), 3).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 3 }); }
    /** "#,#.##" (:992, :1721) — no digit is forced: 0 is an empty text, 0.5 is ".5" */
    function fmtHash2(v) {
        var x = roundAway(num(v), 2);
        if (x === 0) return '';
        var s = Math.abs(x).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 2 });
        if (s.charAt(0) === '0' && s.charAt(1) === '.') s = s.substring(1);
        return (x < 0 ? '-' : '') + s;
    }
    function pad(n) { return String(n).padStart(2, '0'); }
    function isoOf(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function today() { return isoOf(new Date()); }
    function addDaysIso(days) { var d = new Date(); d.setDate(d.getDate() + days); return isoOf(d); }
    /** "dd-MMM-yyyy" */
    function dMMMyyyy(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || ''));
        return m ? m[3] + '-' + MONTHS[+m[2] - 1] + '-' + m[1] : '';
    }
    /** "dd-MMM-yyyy hh:mm tt" (:1569-1570) */
    function dMMMyyyyTime(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})[T ](\d{2}):(\d{2})/.exec(String(v || ''));
        if (!m) return dMMMyyyy(v);
        var h = +m[4], ap = h >= 12 ? 'PM' : 'AM';
        h = h % 12; if (h === 0) h = 12;
        return m[3] + '-' + MONTHS[+m[2] - 1] + '-' + m[1] + ' ' + pad(h) + ':' + m[5] + ' ' + ap;
    }
    function msg(e) { return e && e.message ? e.message : String(e); }
    function rights() { return (L && L.rights) || {}; }
    function show(el, on) { if (typeof el === 'string') el = $(el); if (el) el.hidden = !on; }
    function enc(v) { return encodeURIComponent(v === undefined || v === null ? '' : v); }

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
     * countx_purchase_request.js busy panel + button lock; any refusal is shown as the desktop's MessageBox.
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
    function fill(id, rows, valueKey, textKey, withDefault, attrFn) {
        var el = $(id), html = withDefault ? opt('0', DEFAULT_TEXT) : '';
        (rows || []).forEach(function (r) { html += opt(r[valueKey], r[textKey], attrFn ? attrFn(r) : null); });
        el.innerHTML = html;
    }
    function has(id, v) { return Array.prototype.some.call($(id).options, function (o) { return !o.hasAttribute('data-adhoc') && o.value === String(v); }); }
    /** combo.Value = v — a value that is not in the list leaves the combo without a selection. */
    function setVal(id, v) {
        var el = $(id);
        dropAdhoc(id);
        if (v !== null && v !== undefined && has(id, v)) el.value = String(v); else el.selectedIndex = -1;
    }
    /** Conversion.ToInt(combo.Value) */
    function comboVal(id) {
        var el = $(id);
        if (el.selectedIndex < 0) return 0;
        if (el.options[el.selectedIndex].hasAttribute('data-adhoc')) return 0;
        return intOf(el.value);
    }
    /** combo.Text — exactly what the box shows ("-- Select --" included: formvalidation :817 / :823 tests this text). */
    function comboText(id) {
        var el = $(id);
        return el.selectedIndex < 0 ? '' : el.options[el.selectedIndex].textContent;
    }
    function dropAdhoc(id) { Array.prototype.slice.call($(id).querySelectorAll('option[data-adhoc]')).forEach(function (o) { o.remove(); }); }
    /** combo.Text = string.Empty */
    function clearCombo(id) { dropAdhoc(id); $(id).selectedIndex = -1; }
    /** combo.DataSource = null; combo.Text = string.Empty */
    function unbindCombo(id) { $(id).innerHTML = ''; $(id).selectedIndex = -1; }
    /** combo.Text = text — the row with that display text; a text that matches no row stays in the box. */
    function setText(id, text) {
        var el = $(id), t = String(text === undefined || text === null ? '' : text).trim();
        dropAdhoc(id);
        if (t === '') { el.selectedIndex = -1; return; }
        for (var i = 0; i < el.options.length; i++) {
            if (el.options[i].textContent.trim().toLowerCase() === t.toLowerCase()) { el.selectedIndex = i; return; }
        }
        el.insertAdjacentHTML('beforeend', '<option value="' + esc(t) + '" data-adhoc="1">' + esc(t) + '</option>');
        el.value = t;
    }
    function selectedAttr(id, attr) {
        var el = $(id);
        if (el.selectedIndex < 0) return '';
        return el.options[el.selectedIndex].getAttribute('data-' + attr) || '';
    }
    /** Re-bind a list and keep the value it had when that value is still in the new list. */
    function rebind(id, rows, valueKey, textKey, attrFn) {
        var prev = $(id).selectedIndex < 0 ? null : $(id).value;
        fill(id, rows, valueKey, textKey, false, attrFn);
        setVal(id, prev);
    }

    function partyAttrs(r) { return { gl: r.GlAccountId, code: r.PartyCode }; }
    function itemAttrs(r) { return { 'item-category': r.ItemCategory, 'item-code': r.ItemCode, 'parent-category': r.InventoryParentCategoriesId }; }

    /** ItemFill (:600) — only when the table has rows. */
    function itemFill(rows) {
        if (!rows || !rows.length) return;
        $('CmbItemName').setAttribute('data-dtcombo', 'lsaItem');
        rebind('CmbItemName', rows, 'Id', 'ItemName', itemAttrs);
    }
    /** cmbsupplierfill (:467) — Supplier and Commission Agent share the table; only when it has rows. */
    function supplierFill() {
        if (!L.suppliers || !L.suppliers.length) return;
        rebind('CmbSupplierName', L.suppliers, 'Id', 'CompanyName', partyAttrs);
        rebind('CmbCommissionAgent', L.suppliers, 'Id', 'CompanyName', partyAttrs);
    }
    /** AnalysisTypeFill (:521) — BindAndRetainSelection: two columns, "-- Select --" row, previous value kept, else row 0. */
    function analysisTypeFill() {
        var prev = comboVal('CmbAnalysisType');
        if (!L.analysisTypes || !L.analysisTypes.length) { unbindCombo('CmbAnalysisType'); return; }
        fill('CmbAnalysisType', L.analysisTypes, 'Id', 'Name', true);
        if (prev !== 0 && has('CmbAnalysisType', prev)) $('CmbAnalysisType').value = String(prev);
        else $('CmbAnalysisType').selectedIndex = 0;
    }
    /** LabstatusFill (:645) — Accepted / Rejected, Rows[0].Activate(). */
    function statusFill() {
        fill('CmbStatus', L.statuses, 'Id', 'Name', false);
        $('CmbStatus').selectedIndex = 0;
    }
    function bindLists() {
        itemFill(L.items);
        analysisTypeFill();
        supplierFill();
        rebind('CmbCropYear', L.cropYears, 'Id', 'Name');                           // CropYearFill (:616)
        if (L.jobLots && L.jobLots.length) rebind('CmbJobLot', L.jobLots, 'Id', 'Name');   // combojoblotfill (:629)
        statusFill();
    }
    /** GetConfigurationsFromGlobalAndBindValuesInColumns (:390). */
    function applyConfigs(c) {
        if (intOf(c.defaultJobLot) !== 0) setVal('CmbJobLot', intOf(c.defaultJobLot));
        if (intOf(c.defaultCropYear) !== 0 && $('CmbCropYear').options.length) setVal('CmbCropYear', intOf(c.defaultCropYear));
    }
    /** HistoryComboFill (:324) — six Id / name lists, no default row, nothing selected. */
    function historyComboFill(c) {
        [['CmbSupplierCustomerHistory', c.historySuppliers], ['CmbItemHistory', c.historyItems],
         ['CmbCropYearHistory', c.historyCropYears], ['CmbJobLotHistory', c.historyJobLots],
         ['CmbStatusHistory', c.historyStatuses], ['CmbAnalysisGroupHistory', c.historyAnalysisGroups]].forEach(function (p) {
            var prev = $(p[0]).selectedIndex < 0 ? null : $(p[0]).value;
            fill(p[0], p[1], 'Id', 'Name', false);
            setVal(p[0], prev);
        });
    }

    // ================================================================= dependent combos

    /** CmbAnalysisType_Leave (:678) — also fired by txtdocdate_ValueChanged (:804). */
    async function analysisTypeLeave() {
        var typeId = comboVal('CmbAnalysisType');
        var rows = await api('GET', API + '/analysis-groups?typeId=' + typeId + '&docDate=' + enc($('txtdocdate').value));
        await analysisGroupBind(rows);
    }
    /** AnalysisGroupBind (:584) — AllColumns, "-- Select --" row, previous value kept; CmbAnalysisGroup_Leave when there are rows. */
    async function analysisGroupBind(rows) {
        var prev = comboVal('CmbAnalysisGroup');
        if (!rows || !rows.length) { unbindCombo('CmbAnalysisGroup'); return; }     // BindAndRetainSelection: Text "", DataSource null
        fill('CmbAnalysisGroup', rows, 'Id', 'AnalysisGroupDescription', true, function (r) {
            return { 'group-type-id': r.GroupTypeId, 'group-type': r.GroupType };
        });
        if (prev !== 0 && has('CmbAnalysisGroup', prev)) $('CmbAnalysisGroup').value = String(prev);
        else $('CmbAnalysisGroup').selectedIndex = 0;
        await analysisGroupLeave();
    }
    /** CmbAnalysisGroup_Leave (:691) — DetailGridFillByGroup(GetDtForDetailGrid(type)) then ItemBind(). */
    async function analysisGroupLeave() {
        var typeId = comboVal('CmbAnalysisType');
        var rows = await api('GET', API + '/parameters?typeId=' + typeId + '&groupId=' + comboVal('CmbAnalysisGroup')
            + '&docDate=' + enc($('txtdocdate').value));
        st.details = rows || [];
        bindGrid();
        await itemBind();
    }
    /** ItemBind (:705). */
    async function itemBind() {
        var orderId = comboVal('CmbPurchaseOrder'), groupId = comboVal('CmbAnalysisGroup');
        var groupTypeId = groupId > 0 ? intOf(selectedAttr('CmbAnalysisGroup', 'group-type-id')) : 0;
        await bindItems(groupId, groupTypeId, orderId);
    }
    /**
     * BindItemsByAnalysisGroupandOrderId (:736) / BindItemsByAnalysisGroup (:757) / ItemFill (:600).
     * No rows for a group: the combo is emptied, the focus goes to Analysis Group and the desktop's
     * message is shown.
     */
    async function bindItems(groupId, groupTypeId, orderId) {
        var res = await api('GET', API + '/items?groupId=' + groupId + '&groupTypeId=' + groupTypeId + '&orderId=' + orderId);
        var rows = res.items || [];
        if (res.mode === 'all') { itemFill(rows); return; }
        if (rows.length) {
            $('CmbItemName').setAttribute('data-dtcombo', 'single');
            rebind('CmbItemName', rows, 'Id', 'ItemName');
            return;
        }
        unbindCombo('CmbItemName');
        focusCombo('CmbAnalysisGroup');
        alert(res.mode === 'order'
            ? 'Please Select another Group Analysis,because Items not found against selected Order and analysis Group'
            : 'Please Select another Group Analysis,because Items not found against selected analysis Group');
    }
    /** cmbPurchaseOrder_Leave (:663). */
    async function purchaseOrderLeave() {
        if (comboVal('CmbPurchaseOrder') > 0 && comboVal('CmbAnalysisGroup') > 0) {
            await bindItems(comboVal('CmbAnalysisGroup'), intOf(selectedAttr('CmbAnalysisGroup', 'group-type-id')), comboVal('CmbPurchaseOrder'));
        }
    }
    /**
     * PurchaseOrderFill (:484) and SampleLogNumberFill (:435) — one request.
     * Purchase Order: rows -> bound, first row active; no rows -> emptied. Supplier or item 0 -> untouched.
     * Sample Log No: rows -> bound, first row active; otherwise left as it is.
     */
    async function partyItemFill(purchaseOrder, sampleLog) {
        var supplierId = comboVal('CmbSupplierName'), itemId = comboVal('CmbItemName');
        if (supplierId === 0 || itemId === 0) return;
        var res = await api('GET', API + '/by-party-item?supplierId=' + supplierId + '&itemId=' + itemId);
        if (purchaseOrder && res.purchaseOrderApplies) {
            if (res.purchaseOrders.length) {
                fill('CmbPurchaseOrder', res.purchaseOrders, 'Id', 'Name', false);
                $('CmbPurchaseOrder').selectedIndex = 0;                            // Rows[0].Activate() (:506)
            } else {
                unbindCombo('CmbPurchaseOrder');                                    // :510-511
            }
        }
        if (sampleLog && res.sampleLogApplies && res.sampleLogs.length) {
            fill('CmbSampleLogNo', res.sampleLogs, 'Id', 'Name', false);
            $('CmbSampleLogNo').selectedIndex = 0;                                  // Rows[0].Activate() (:457)
        } else if (sampleLog && res.sampleLogApplies) {
            /* Dependent combo: the desktop leaves the PREVIOUS supplier/item's sample logs in the list here
               (a stale Id would then be saved); the web list always belongs to the current pair. */
            unbindCombo('CmbSampleLogNo');
        }
    }
    function focusCombo(id) {
        /* countx_desktop_combo.js moves the <select> into a .dtcombo-wrap beside its text box */
        var el = $(id), wrap = el.closest ? el.closest('.dtcombo-wrap') : null, box = wrap ? wrap.querySelector('.dtcombo-input') : null;
        try { (box || el).focus(); } catch (e) { /* not focusable */ }
    }

    // ================================================================= detail grids (Form)

    /** BindGrid (:923) + Grdsetting (:937): visible Analysis Parameter (link, 150), MinValue, MaxValue, ResultValue (55 each), RemarksDetail; ResultValue and RemarksDetail editable. */
    function bindGrid() {
        var t = $('grd');
        t.tHead.innerHTML = '<tr><th style="width:150px;">Analysis Parameter</th><th style="width:55px;">MinValue</th>'
            + '<th style="width:55px;">MaxValue</th><th style="width:55px;">ResultValue</th><th>RemarksDetail</th></tr>';
        var html = '';
        st.details.forEach(function (r, i) {
            html += '<tr data-i="' + i + '">'
                + '<td><a class="lnk" data-act="sub">' + esc(r.AnalysisParameterDescription) + '</a></td>'
                + '<td class="n">' + fmt3(r.MinValue) + '</td>'
                + '<td class="n">' + fmt3(r.MaxValue) + '</td>'
                + '<td><input type="text" class="r" data-f="ResultValue" value="' + esc(fmt3(r.ResultValue)) + '"></td>'
                + '<td><input type="text" data-f="RemarksDetail" value="' + esc(r.RemarksDetail === null || r.RemarksDetail === undefined ? '' : r.RemarksDetail) + '"></td>'
                + '</tr>';
        });
        t.tBodies[0].innerHTML = html;
        gridFooter();
        /* The sub grid shows the list of a row of the PREVIOUS detaillst on the desktop until another
           link is clicked; values typed there would be lost, so it is emptied here. */
        st.subOf = -1;
        clearGrid('grdSubItems');
    }
    /** TotalRow: the numeric columns are summed (GridColumnSettings), "#,##0.###". */
    function gridFooter() {
        var a = 0, b = 0, c = 0;
        st.details.forEach(function (r) { a += num(r.MinValue); b += num(r.MaxValue); c += num(r.ResultValue); });
        $('grd').tFoot.innerHTML = st.details.length
            ? '<tr><td></td><td class="n">' + fmt3(a) + '</td><td class="n">' + fmt3(b) + '</td><td class="n">' + fmt3(c) + '</td><td></td></tr>' : '';
    }
    function clearGrid(id) { var t = $(id); t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; }

    /** grd_LinkClicked (:1020) + GrdSubsetting (:971): Parent Parameter, Sub Parameter (read-only), ResultValue ("#,#.##", summed). */
    function showSubItems(i) {
        var r = st.details[i];
        if (!r) return;
        st.subOf = i;
        var t = $('grdSubItems');
        t.tHead.innerHTML = '<tr><th>Parent Parameter</th><th>Sub Parameter</th><th>ResultValue</th></tr>';
        var html = '';
        (r.Subs || []).forEach(function (s, j) {
            html += '<tr data-j="' + j + '"><td>' + esc(s.ParentParameterName) + '</td><td>' + esc(s.SubParameterName) + '</td>'
                + '<td><input type="text" class="r" data-f="SubResultValue" value="' + esc(fmtHash2(s.ResultValue)) + '"></td></tr>';
        });
        t.tBodies[0].innerHTML = html;
        subFooter();
        Array.prototype.forEach.call($('grd').tBodies[0].rows, function (tr) { tr.classList.toggle('is-sel', intOf(tr.getAttribute('data-i')) === i); });
    }
    function subFooter() {
        var r = st.details[st.subOf], total = 0;
        ((r && r.Subs) || []).forEach(function (s) { total += num(s.ResultValue); });
        $('grdSubItems').tFoot.innerHTML = r ? '<tr><td></td><td></td><td class="n">' + fmtHash2(total) + '</td></tr>' : '';
    }

    // ============================================================================ pictures

    function picEls(which) {
        return which === 'analysis'
            ? { img: $('PicBAnalysisImage'), file: $('fileAnalysisPic') }
            : { img: $('PicBCookingImage'), file: $('fileCookingPic') };
    }
    /** PictureBox.Image = null */
    function picClear(which) {
        var e = picEls(which);
        st.pic[which] = { mode: 'none', name: '', data: '' };
        e.img.removeAttribute('src');
        e.img.hidden = true;
        e.img.classList.remove('zoom');
        e.file.value = '';
    }
    /** ReadById (:1285-1330): the stored picture when its file exists, otherwise none. */
    function picStored(which, id, exists) {
        picClear(which);
        if (!exists) return;
        var e = picEls(which);
        st.pic[which] = { mode: 'keep', name: '', data: '' };
        e.img.src = API + '/' + id + '/picture/' + which + '?t=' + Date.now();
        e.img.hidden = false;
    }
    /** btnimg1_Click (:1731) / btnimage2_Click (:1768): OK -> the chosen image, SizeMode Zoom; Cancel -> the box is emptied. */
    function picChosen(which) {
        var e = picEls(which), f = e.file.files && e.file.files[0];
        if (!f) { picClear(which); return; }
        var reader = new FileReader();
        reader.onload = function () {
            st.pic[which] = { mode: 'new', name: f.name, data: String(reader.result) };
            e.img.src = String(reader.result);
            e.img.hidden = false;
            e.img.classList.add('zoom');                                            // PictureBoxSizeMode.Zoom (:1754)
        };
        reader.onerror = function () { picClear(which); alert('The selected image could not be read.'); };
        reader.readAsDataURL(f);
    }
    /** btnViewAnalysisPic_Click (:1841) / btnViewCookingPic_Click (:1858) — LabImagesPreview, only when there is an image. */
    function picView(which) {
        var e = picEls(which);
        if (e.img.hidden || !e.img.getAttribute('src')) return;
        preview(e.img.src);
    }
    function preview(src) {
        var img = $('pictureBox1');
        img.onerror = function () { show('dlgPreview', false); };                   // FormHelper.ShowLabImagesPreview: nothing when the file is missing
        img.src = src;
        show('dlgPreview', true);
    }

    // ================================================================================ form

    /** formvalidation (:809) — desktop order, text and focus. */
    function formvalidation() {
        if ($('txtManaulReportNo').value.trim() === '') { alert('Please Report No'); $('txtManaulReportNo').focus(); return false; }
        if (comboText('CmbAnalysisType').trim() === '') { alert('Please select Analysis Type'); focusCombo('CmbAnalysisType'); return false; }
        if (comboText('CmbAnalysisGroup').trim() === '') { alert('Please select Analysis Standard Group'); focusCombo('CmbAnalysisGroup'); return false; }
        if (comboVal('CmbSupplierName') === 0 && comboVal('CmbCommissionAgent') === 0) {
            alert('Please select Supplier or Commission Agent'); focusCombo('CmbSupplierName'); return false;
        }
        return true;
    }

    function payload() {
        return {
            id: st.recId,
            docDate: $('txtdocdate').value,
            reportNo: $('txtManaulReportNo').value,
            partyLotRefNo: $('txtPartyLotRefNo').value,
            qty: $('txtQty').value,
            sampleLogId: comboVal('CmbSampleLogNo'),
            supplierId: comboVal('CmbSupplierName'),
            commissionAgentId: comboVal('CmbCommissionAgent'),
            orderId: comboVal('CmbPurchaseOrder'),
            analysisTypeId: comboVal('CmbAnalysisType'),
            analysisTypeText: comboText('CmbAnalysisType'),
            analysisGroupId: comboVal('CmbAnalysisGroup'),
            analysisGroupText: comboText('CmbAnalysisGroup'),
            itemId: comboVal('CmbItemName'),
            cropText: comboText('CmbCropYear'),
            jobLotId: comboVal('CmbJobLot'),
            statusText: comboText('CmbStatus'),
            remarks: $('txtremarks').value,
            details: st.details.map(function (r) {
                return {
                    id: intOf(r.Id), invLabAnalysisItemsId: intOf(r.InvLabAnalysisItemsId), resultValue: num(r.ResultValue),
                    remarksDetail: r.RemarksDetail === undefined ? null : r.RemarksDetail,
                    subs: (r.Subs || []).map(function (s) { return { id: intOf(s.Id), subParameterId: intOf(s.SubParameterId), resultValue: num(s.ResultValue) }; })
                };
            }),
            analysisPic: st.pic.analysis,
            cookingPic: st.pic.cooking,
            confirm: false
        };
    }

    /** Insert() (:1035). The Yes/No question comes back from the server as 409 {confirm:true,message}. */
    async function insert() {
        if (!formvalidation()) return;
        var body = payload(), res;
        try {
            res = await api('POST', API + '/save', body);
        } catch (e) {
            if (!e.confirm) throw e;
            if (!window.confirm(e.message)) return;                                 // DialogResult.No (:1048, :1054)
            body.confirm = true;
            res = await api('POST', API + '/save', body);
        }
        alert(res.message);                                                         // "Update Successfully" / "Save Successfully"
        await refresh();                                                            // :1192
        if ($('IsPrint').checked) await print657(res.id);                           // :1193
    }
    /** Save_Click (:1204). */
    function saveClick() { st.recId = 0; return insert(); }
    /** Update_Click (:1217). */
    function updateClick() { return insert(); }

    /** ReadById (:1229). */
    async function readById(id) {
        var h = await api('GET', API + '/' + intOf(id));
        show('BtnSave', false);
        show('BtnUpdate', true);
        st.updateMode = true;
        st.recId = intOf(id);
        if (!h.Details || h.Details.length <= 0) return;                            // :1245
        st.details = [];
        selectTab('Form');                                                          // tabControl1.SelectedIndex = 0
        $('txtdocno').value = String(h.DocNo);
        $('txtdocdate').value = h.DocDate || today();
        $('txtManaulReportNo').value = h.ReportNo || '';
        $('txtPartyLotRefNo').value = h.PartyLotRefNo || '';
        $('txtQty').value = netStr(h.NoOfBagsInspected);
        if (intOf(h.SupplierCustomerId) > 0) setVal('CmbSupplierName', h.SupplierCustomerId);       // :1256
        if (intOf(h.CommissionAgentId) > 0) setVal('CmbCommissionAgent', h.CommissionAgentId);      // :1260
        setText('CmbCropYear', h.Crop);                                             // :1264
        setVal('CmbAnalysisType', intOf(h.AnalysisTypeId));                         // :1265
        try { await analysisTypeLeave(); } catch (e) { alert(msg(e)); }             // :1266
        setVal('CmbAnalysisGroup', intOf(h.InvLabAnalysisGroupId));                 // :1267
        try { await itemBind(); } catch (e) { alert(msg(e)); }                      // :1268
        setVal('CmbItemName', intOf(h.ItemId));                                     // :1269
        try { await partyItemFill(true, true); } catch (e) { alert(msg(e)); }       // PurchaseOrderFill (:1270) + the Sample Log list
        setVal('CmbPurchaseOrder', intOf(h.OrderId));                               // :1271
        /* Edit-load preselect of the dependent Sample Log No. The desktop leaves CmbSampleLogNo as it was
           (so its Update saves InvLabSampleLogRegisterId 0); here the stored link is shown and kept. */
        setVal('CmbSampleLogNo', intOf(h.InvLabSampleLogRegisterId) > 0 ? intOf(h.InvLabSampleLogRegisterId) : null);
        setVal('CmbJobLot', intOf(h.JobLotId));                                     // :1272
        setText('CmbStatus', h.IsAccepted ? 'Accepted' : 'Rejected');               // :1273-1280
        $('txtremarks').value = h.Remarks || '';
        st.details = h.Details;                                                     // :1282
        bindGrid();
        picStored('analysis', st.recId, !!h.AnalysisPicExists);
        picStored('cooking', st.recId, !!h.CookingPicExists);
    }

    /** refresh() (:1343) — New_Click and after a save. RecId is not reset here on the desktop either. */
    async function refresh() {
        $('txtdocdate').focus();
        st.updateMode = false;
        show('BtnSave', true);
        show('BtnUpdate', false);
        clearCombo('CmbItemName');
        clearCombo('CmbJobLot');
        clearCombo('CmbAnalysisGroup');
        clearCombo('CmbSupplierName');
        clearCombo('CmbCommissionAgent');
        $('txtQty').value = '';
        clearCombo('CmbCropYear');
        $('txtdocno').value = '';
        unbindCombo('CmbPurchaseOrder');
        unbindCombo('CmbSampleLogNo');                                              // web: no stale sample log on a new document
        $('txtremarks').value = '';
        $('txtManaulReportNo').value = '';
        $('txtPartyLotRefNo').value = '';
        st.details = [];
        clearGrid('grdSubItems');
        st.subOf = -1;
        clearGrid('grd');                                                           // grd.DataSource = null
        picClear('analysis');
        picClear('cooking');
        var n = await api('GET', API + '/numbers');
        if (intOf(n.docNo) > 0) $('txtdocno').value = String(n.docNo);              // itemcode (:411)
        applyConfigs(n);
    }

    /** toolStripButton2_Click "Refresh" (:1385). */
    async function refreshLists() {
        var r = await api('GET', API + '/refresh');
        if (intOf(r.docNo) > 0) $('txtdocno').value = String(r.docNo);
        L.items = r.items; L.suppliers = r.suppliers; L.cropYears = r.cropYears; L.jobLots = r.jobLots;
        L.statuses = r.statuses; L.analysisTypes = r.analysisTypes;
        itemFill(L.items);
        supplierFill();
        rebind('CmbCropYear', L.cropYears, 'Id', 'Name');
        if (L.jobLots && L.jobLots.length) rebind('CmbJobLot', L.jobLots, 'Id', 'Name');
        statusFill();
        analysisTypeFill();
    }

    /**
     * Print657 (:1892) — 657-RptInvLabSampleAnalysisSlipA.rpt through the Jasper print endpoint.
     * The server first runs the report procedure: no rows is "No Record Found For Display".
     * Id 0 (657-Print on a new form) sends no @Id, as the BLL does.
     */
    async function print657(id) {
        id = intOf(id);
        await api('GET', API + '/' + id + '/print-check');
        window.open('/reports/print/by-template/' + encodeURIComponent(PRINT_RPT) + '/pdf' + (id > 0 ? '?id=' + id : ''), '_blank');
    }

    // ============================================================================= history

    var HIST_COLS = [
        { k: 'Print', btn: 'Print', w: 40 },                                        // AddButton(..., 40, 0) (:1575)
        { k: 'Edit', btn: 'Edit', w: 40 },                                          // AddButton(..., 40, 1) (:1576)
        { k: 'DocNo', w: 60, cls: 'c', link: true, doc: true },                     // web: the document number opens the record (= Edit)
        { k: 'DocDate', w: 73, f: dMMMyyyy },
        { k: 'ReportNo', w: 60 },
        { k: 'PartyLotNo', w: 100 },
        { k: 'NoOfBags', w: 70, cls: 'n', f: fmt3, sum: true },                     // "#,##0.###", Sum (:1571-1574)
        { k: 'PartyName', w: 190 },
        { k: 'CommissionAgent', w: 190 },
        { k: 'ItemName', w: 170 },
        { k: 'CropYear', w: 73 },
        { k: 'JobLot', w: 80 },
        { k: 'Status', w: 75 },
        { k: 'AnalysisGroup', w: 110 },
        { k: 'EntryDate', w: 145, f: dMMMyyyyTime },
        { k: 'EntryUser', w: 100 },
        { k: 'ModifyDate', w: 145, f: dMMMyyyyTime },
        { k: 'ModifyUser', w: 100 },
        { k: 'AnalysisPic', w: 60, link: true, cls: 'c' },                          // ColumnType Link (:1580)
        { k: 'CookingPic', w: 60, link: true, cls: 'c' },
        { k: 'NoOfAttachments', w: 90, link: true, cls: 'c' },                      // ColumnType Link (:1579)
        { k: 'AddAttachment', btn: 'Add Attachment', w: 105, off: 'Adding attachments from History (AttachmentAddingFromHistory) is not ported' }
    ];
    function histText(c, r) { var v = r[c.k]; return c.f ? c.f(v) : (v === null || v === undefined ? '' : String(v)); }
    function histVisible() {
        return st.hist.filter(function (r) {
            return HIST_COLS.every(function (c) {
                var q = (st.histFilter[c.k] || '').trim().toLowerCase();
                return c.btn || q === '' || histText(c, r).toLowerCase().indexOf(q) >= 0;    // DefaultFilterRowComparison Contains
            });
        });
    }
    /** grdhistorySetting (:1544). */
    function renderHistoryHead() {
        var h = '<tr>', f = '<tr class="flt">';
        HIST_COLS.forEach(function (c) {
            h += '<th style="min-width:' + c.w + 'px;">' + esc(c.btn || c.k) + '</th>';
            f += '<th>' + (c.btn ? '' : '<input type="text" data-k="' + c.k + '" value="' + esc(st.histFilter[c.k] || '') + '">') + '</th>';
        });
        $('grdhistory').tHead.innerHTML = h + '</tr>' + f + '</tr>';
    }
    function renderHistoryBody() {
        var rows = histVisible(), html = '', total = 0;
        rows.forEach(function (r) {
            total += num(r.NoOfBags);
            html += '<tr data-id="' + r.Id + '"' + (r.Id === st.histSel ? ' class="is-sel"' : '') + '>';
            HIST_COLS.forEach(function (c) {
                if (c.btn) html += '<td class="c"><button type="button" class="gbtn" data-act="' + c.k + '"' + (c.off ? ' disabled title="' + esc(c.off) + '"' : '') + '>' + esc(c.btn) + '</button></td>';
                else if (c.link) html += '<td class="' + (c.cls || '') + '"><a class="lnk' + (c.doc ? ' doc' : '') + '" data-act="' + c.k + '"' + (c.doc ? ' title="Open this record in the form"' : '') + '>' + esc(histText(c, r)) + '</a></td>';
                else html += '<td class="' + (c.cls || '') + '">' + esc(histText(c, r)) + '</td>';
            });
            html += '</tr>';
        });
        $('grdhistory').tBodies[0].innerHTML = html;
        var foot = '';
        if (rows.length) {
            foot = '<tr>';
            HIST_COLS.forEach(function (c) { foot += '<td class="' + (c.cls || '') + '">' + (c.sum ? fmt3(total) : '') + '</td>'; });
            foot += '</tr>';
        }
        $('grdhistory').tFoot.innerHTML = foot;
    }
    /** gridhistoryfill (:1475). From / To Doc No are read by the form but never reach the procedure (BLL GetAll). */
    async function gridhistoryfill() {
        var q = '?fromChecked=' + $('chkFromDateHistory').checked + '&fromDate=' + enc($('FromDateHistory').value)
            + '&toChecked=' + $('chkToDateHistory').checked + '&toDate=' + enc($('ToDateHistory').value)
            + '&supplierId=' + comboVal('CmbSupplierCustomerHistory') + '&itemId=' + comboVal('CmbItemHistory')
            + '&jobLotId=' + comboVal('CmbJobLotHistory') + '&analysisGroupId=' + comboVal('CmbAnalysisGroupHistory')
            + '&cropYear=' + enc(comboText('CmbCropYearHistory')) + '&status=' + enc(comboText('CmbStatusHistory'));
        var rows = await api('GET', API + '/history' + q);
        st.hist = rows || [];
        st.histFilter = {};
        st.histSel = 0;
        if (!st.hist.length) { clearGrid('grdhistory'); return; }                   // grdhistory.DataSource = null (:1540)
        renderHistoryHead();
        renderHistoryBody();
        await historySelect(st.hist[0].Id);                                         // the bound grid's first row becomes current
    }
    /** grdhistory_SelectionChanged (:1674) + GrdHistoryDetailSetting (:1710). */
    async function historySelect(id) {
        id = intOf(id);
        if (id <= 0) return;
        st.histSel = id;
        Array.prototype.forEach.call($('grdhistory').tBodies[0].rows, function (tr) { tr.classList.toggle('is-sel', intOf(tr.getAttribute('data-id')) === id); });
        var h = await api('GET', API + '/' + id);
        if (st.histSel !== id) return;
        var t = $('grdHistoryDetail');
        t.tHead.innerHTML = '<tr><th style="width:125px;">AnalysisGroup</th><th style="width:125px;">AnalysisParameter</th><th style="width:70px;">MinValue</th>'
            + '<th style="width:70px;">MaxValue</th><th style="width:90px;">ResultValue</th><th>RemarksDetail</th></tr>';
        var html = '', total = 0;
        (h.Details || []).forEach(function (d) {
            total += num(d.ResultValue);
            html += '<tr><td>' + esc(d.AnalysisGroupDescription) + '</td><td>' + esc(d.AnalysisParameterDescription) + '</td>'
                + '<td>' + esc(netStr(d.MinValue)) + '</td><td>' + esc(netStr(d.MaxValue)) + '</td>'
                + '<td class="n">' + fmtHash2(d.ResultValue) + '</td><td>' + esc(d.RemarksDetail === null || d.RemarksDetail === undefined ? '' : d.RemarksDetail) + '</td></tr>';
        });
        t.tBodies[0].innerHTML = html;
        t.tFoot.innerHTML = '<tr><td></td><td></td><td></td><td></td><td class="n">' + fmtHash2(total) + '</td><td></td></tr>';
    }
    /** btnNewHistory_Click (:1428). DateTimePicker.Text = "" resets the picker to today and unticks its box. */
    function historyNew() {
        $('FromDateHistory').value = today(); $('chkFromDateHistory').checked = false;
        $('ToDateHistory').value = today(); $('chkToDateHistory').checked = false;
        $('FromDocNoHistory').value = '';
        $('ToDocNoHistory').value = '';
        ['CmbSupplierCustomerHistory', 'CmbItemHistory', 'CmbCropYearHistory', 'CmbJobLotHistory', 'CmbStatusHistory', 'CmbAnalysisGroupHistory'].forEach(clearCombo);
        st.hist = []; st.histSel = 0; st.histFilter = {};
        clearGrid('grdhistory');
        clearGrid('grdHistoryDetail');
    }
    /** grdhistory_LinkClicked "NoOfAttachments" (:1652) — AttachmentView, only when the record has attachments. */
    async function showAttachments(id) {
        var rows = await api('GET', API + '/' + intOf(id) + '/attachments');
        if (!rows || !rows.length) return;
        var html = '';
        rows.forEach(function (r) { html += '<tr><td>' + esc(r.AttachmentName) + '</td><td>' + esc(r.CustomName) + '</td><td>' + esc(dMMMyyyyTime(r.EntryDate)) + '</td></tr>'; });
        $('grdAttachments').tBodies[0].innerHTML = html;
        show('dlgAttachments', true);
    }

    // ================================================================================ wiring

    function selectTab(name) {
        ['Form', 'History'].forEach(function (p) { $(p).classList.toggle('is-active', p === name); });
        $('tabBtnForm').classList.toggle('is-active', name === 'Form');
        $('tabBtnHistory').classList.toggle('is-active', name === 'History');
        $('btnHistory').innerHTML = name === 'History' ? '<i class="fa fa-pencil-square-o"></i> Form' : '<i class="fa fa-history"></i> History';
    }
    function onHistoryTab() { return $('History').classList.contains('is-active'); }
    /** Footer History button (acceptance rule 4): Form <-> History; the first visit runs Show so the list is there. */
    async function historyButton() {
        if (onHistoryTab()) { selectTab('Form'); return; }
        selectTab('History');
        if (!st.hist.length) await gridhistoryfill();
    }
    function closeFullscreen() {
        var full = document.querySelector('.wf-block.is-full');
        if (full) full.classList.remove('is-full');
        return !!full;
    }

    function wire() {
        // ---- toolStrip2
        $('BtnNew').addEventListener('click', function () { act(this, refresh); });                         // New_Click (:1338)
        $('BtnRefresh').addEventListener('click', function () { act(this, refreshLists); });
        $('BtnSave').addEventListener('click', function () { act(this, saveClick); });
        $('BtnUpdate').addEventListener('click', function () { act(this, updateClick); });
        $('BtnPrint').addEventListener('click', function () { act(this, function () { return print657(st.recId); }); });   // Print_Click (:1880)

        // ---- combo chain
        $('txtdocdate').addEventListener('change', function () { act(null, analysisTypeLeave); });          // txtdocdate_ValueChanged (:804)
        $('CmbAnalysisType').addEventListener('change', function () { act(null, analysisTypeLeave); });     // :678
        $('CmbAnalysisGroup').addEventListener('change', function () { act(null, analysisGroupLeave); });   // :691
        $('CmbItemName').addEventListener('change', function () { act(null, function () { return partyItemFill(true, true); }); });       // cmbitem_Leave (:778)
        $('CmbSupplierName').addEventListener('change', function () { act(null, function () { return partyItemFill(true, true); }); });   // cmbsupplier_Leave (:791)
        $('CmbPurchaseOrder').addEventListener('change', function () { act(null, purchaseOrderLeave); });   // :663

        // ---- grd / grdSubItems
        $('grd').addEventListener('click', function (e) {
            var a = e.target.closest('a[data-act="sub"]');
            if (a) showSubItems(intOf(a.closest('tr').getAttribute('data-i')));
        });
        $('grd').addEventListener('change', function (e) {
            var f = e.target.getAttribute && e.target.getAttribute('data-f');
            if (!f) return;
            var r = st.details[intOf(e.target.closest('tr').getAttribute('data-i'))];
            if (!r) return;
            if (f === 'ResultValue') { r.ResultValue = num(e.target.value); e.target.value = fmt3(r.ResultValue); gridFooter(); }
            else if (f === 'RemarksDetail') r.RemarksDetail = e.target.value;
        });
        $('grdSubItems').addEventListener('change', function (e) {
            if (!e.target.getAttribute || e.target.getAttribute('data-f') !== 'SubResultValue') return;
            var r = st.details[st.subOf], s = r && r.Subs ? r.Subs[intOf(e.target.closest('tr').getAttribute('data-j'))] : null;
            if (!s) return;
            s.ResultValue = num(e.target.value);
            e.target.value = fmtHash2(s.ResultValue);
            subFooter();
        });

        // ---- pictures
        $('BtnBrowseAnaylsisPic').addEventListener('click', function () { $('fileAnalysisPic').value = ''; $('fileAnalysisPic').click(); });
        $('BtnBrowseCookingPic').addEventListener('click', function () { $('fileCookingPic').value = ''; $('fileCookingPic').click(); });
        $('fileAnalysisPic').addEventListener('change', function () { picChosen('analysis'); });
        $('fileCookingPic').addEventListener('change', function () { picChosen('cooking'); });
        $('fileAnalysisPic').addEventListener('cancel', function () { picClear('analysis'); });             // dialog cancelled (:1756)
        $('fileCookingPic').addEventListener('cancel', function () { picClear('cooking'); });               // dialog cancelled (:1793)
        $('btnClearAnalysisPic').addEventListener('click', function () { picClear('analysis'); });          // :1805
        $('btnClearCookingPic').addEventListener('click', function () { picClear('cooking'); });            // :1823
        $('BtnViewAnalysisPic').addEventListener('click', function () { picView('analysis'); });
        $('btnViewCookingPic').addEventListener('click', function () { picView('cooking'); });
        $('dlgPreviewClose').addEventListener('click', function () { show('dlgPreview', false); $('pictureBox1').removeAttribute('src'); });
        $('dlgAttachmentsClose').addEventListener('click', function () { show('dlgAttachments', false); });

        // ---- tabs
        $('tabBtnForm').addEventListener('click', function () { selectTab('Form'); });
        $('tabBtnHistory').addEventListener('click', function () { selectTab('History'); });
        $('btnHistory').addEventListener('click', function () { act(this, historyButton); });
        Array.prototype.forEach.call(document.querySelectorAll('[data-full]'), function (b) {                // rule 6: grid fullscreen toggle
            b.addEventListener('click', function () { $(b.getAttribute('data-full')).classList.toggle('is-full'); });
        });

        // ---- History
        $('btnNewHistory').addEventListener('click', function () { act(this, async function () { historyNew(); }); });
        $('btnRefreshHistory').addEventListener('click', function () {                                       // :1451
            act(this, async function () { historyComboFill(await api('GET', API + '/history-combos')); });
        });
        $('btnShowHistory').addEventListener('click', function () { act(this, gridhistoryfill); });          // :1463
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
            var el = e.target.closest('[data-act]'), k = el ? el.getAttribute('data-act') : '';
            act(el || null, async function () {
                if (id !== st.histSel) await historySelect(id);
                if (k === 'Edit' || k === 'DocNo') await readById(id);                                       // :1601 (DocNo link: web rule 4)
                else if (k === 'Print') await print657(id);                                                  // :1606
                else if (k === 'NoOfAttachments') await showAttachments(id);                                 // :1652
                else if (k === 'AnalysisPic') preview(API + '/' + id + '/picture/analysis?t=' + Date.now()); // :1656
                else if (k === 'CookingPic') preview(API + '/' + id + '/picture/cooking?t=' + Date.now());   // :1661
            });
        });
        $('grdhistory').addEventListener('dblclick', function (e) {                                          // grdhistory_DoubleClick (:1624)
            var tr = e.target.closest('tbody tr');
            if (!tr || e.target.closest('[data-act]')) return;
            act(null, function () { return readById(intOf(tr.getAttribute('data-id'))); });
        });

        // ---- InvLabSampleAnalysis_KeyDown (:1946)
        document.addEventListener('keydown', function (e) {
            if (e.defaultPrevented) return;                        // an open combo drop-down handled the key
            var c = e.ctrlKey || e.metaKey, k = e.key;
            if (k === 'Enter' && !c && !e.altKey) {
                var a = document.activeElement;
                if (a && a.tagName === 'INPUT' && (a.classList.contains('wf-txt') || a.classList.contains('wf-dt') || a.closest('.win-grid tbody'))) {
                    e.preventDefault();
                    focusNext(a);                                                                            // SendKeys.Send("{TAB}")
                }
                return;
            }
            if (c && (k === 's' || k === 'S')) { e.preventDefault(); if (!st.updateMode) act($('BtnSave'), saveClick); }
            else if (c && (k === 'n' || k === 'N')) { e.preventDefault(); act($('BtnNew'), refresh); }
            else if ((c && (k === 'e' || k === 'E')) || k === 'Escape') {
                if (!$('dlgPreview').hidden) { show('dlgPreview', false); return; }
                if (!$('dlgAttachments').hidden) { show('dlgAttachments', false); return; }
                if (closeFullscreen()) { e.preventDefault(); return; }
                e.preventDefault(); window.location.href = '/quality';                                       // Close()
            }
            else if (c && (k === 'u' || k === 'U')) { e.preventDefault(); if (st.updateMode) act($('BtnUpdate'), updateClick); }
        });
    }
    function focusNext(a) {
        var page = a.closest('.lsa-page') || document;
        var list = Array.prototype.filter.call(page.querySelectorAll('input:not([type=hidden]):not([type=file]):not([disabled]), button.wf-btn:not([disabled])'),
            function (el) { return el.offsetParent !== null; });
        var i = list.indexOf(a);
        if (i >= 0 && i + 1 < list.length) list[i + 1].focus();
    }

    /** InvLabSampleAnalysis_Load (:285). */
    async function load() {
        $('txtdocdate').value = today();
        try {
            L = await api('GET', API + '/lookups');
        } catch (e) {
            $('rightsNote').textContent = msg(e);
            show('rightsNote', true);
            Array.prototype.forEach.call(document.querySelectorAll('button.wf-tool, button.wf-btn'), function (b) { b.disabled = true; });
            return;
        }
        var r = rights();
        $('BtnSave').disabled = !r.save;                                                                     // :290
        $('BtnPrint').disabled = !r.print;                                                                   // :291
        $('BtnUpdate').disabled = !r.update;                                                                 // :292
        show('BtnSave', true);
        show('BtnUpdate', false);
        if (intOf(L.docNo) > 0) $('txtdocno').value = String(L.docNo);                                       // itemcode (:296)
        bindLists();
        applyConfigs(L);                                                                                     // :303
        clearGrid('grd');
        $('FromDateHistory').value = addDaysIso(-7);                                                         // :314
        $('ToDateHistory').value = today();                                                                  // :315
        historyComboFill(L);                                                                                 // :316
        wire();
        $('txtdocdate').focus();
        /* ?id=<record id> — the report pages link here; the record is opened as the History "Edit" button does. */
        var open = intOf(new URLSearchParams(window.location.search).get('id'));
        if (open > 0) await act(null, function () { return readById(open); });
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', load); else load();
}());
