/* ============================================================================================
 * Screen 166 "Lab Standard Policy For Deduction (Not Use)" —
 * Architecture.WinApp.Lab/InvLabStandardPolicyForDeduction.cs. Line references (:n) are that file.
 *
 * Every event below is the form's own: Lab_Load (:163), Add_Click (:296), IsBetween (:346),
 * grdSettings (:358), btnUpdateDetail_Click (:372), btnCancelDetail_Click (:396), grd_DoubleClick (:409),
 * FormValidation (:436), FormValidationDetail (:453), btnUpdate_Click (:500), Insert (:516),
 * btnSave_Click (:586), getUpdate (:600), DetailReset (:635), Reset (:656), btnNew_Click (:677),
 * the three KeyPress handlers (:689-702), tabControl1_SelectedIndexChanged (:704),
 * gridhistoryfill (:712), grdhistory_DoubleClick (:745).
 * The form has no Delete, no row delete on either grid (no DeletingRecord handler, grd AllowDelete is
 * never enabled, grdhistory AllowDelete is set False :730), no KeyDown handler, and btnprint has no
 * Click handler. The server re-validates what Insert() validates.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/lab/standard-policy-for-deduction';
    var MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    /* ItemBind (:192): DDL.BindDDL over the procedure's table — column 0 (Id) hidden, column 1 captioned
       "Item Name" (the form then sets its width to 300, :200), the other columns under their own names. */
    if (window.DesktopCombo) {
        window.DesktopCombo.define('labSpdItem', [
            { caption: 'Item Name', flex: 4 },
            { caption: 'ItemCategory', flex: 3, key: 'category' },
            { caption: 'ItemCode', flex: 2, key: 'code' },
            { caption: 'InventoryParentCategoriesId', flex: 2, key: 'parent', type: 'num' }
        ]);
    }

    var L = null;                       // lookups
    var st = {
        recId: 0,                       // RecId
        table: [],                      // the form's DataTable "table" (:177-183)
        structure: false,               // grd has columns (RetrieveStructure) — false after ClearStructure (:663)
        showPerameterId: false,         // grdSettings (:358) hides PerameterId; :384 retrieves the structure without it
        gridSel: -1,
        updateDetailIndex: 0,           // :31
        hist: [], histSel: -1
    };

    /* table columns (:177-183); PerameterId is hidden by grdSettings (:365). */
    var GRD_COLS = [
        { key: 'PerameterId', caption: 'PerameterId', hiddenBySettings: true },
        { key: 'AnalysisPerameter', caption: 'AnalysisPerameter' },
        { key: 'RangeFrom', caption: 'RangeFrom', type: 'num' },
        { key: 'RangeTo', caption: 'RangeTo', type: 'num' },
        { key: 'DeductionOn', caption: 'DeductionOn' },
        { key: 'DeductionValue', caption: 'DeductionValue', type: 'num' },
        { key: 'Weight', caption: 'Weight' }
    ];
    /* gridhistoryfill (:712): the 'ReadAll' table, Id hidden (:728). */
    var HIST_COLS = [
        { key: 'PolicyApplyOn', caption: 'PolicyApplyOn', type: 'link' },
        { key: 'ItemName', caption: 'ItemName' },
        { key: 'EffectiveFrom', caption: 'EffectiveFrom', type: 'date' },
        { key: 'EfffectiveTo', caption: 'EfffectiveTo', type: 'date' },
        { key: 'RemarksHeader', caption: 'RemarksHeader' }
    ];

    // ============================================================================ helpers

    function $(id) { return document.getElementById(id); }
    function esc(v) {
        return String(v === undefined || v === null ? '' : v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function intOf(v) { var n = parseInt(String(v === undefined || v === null ? '' : v), 10); return isNaN(n) ? 0 : n; }
    var NUM_RE = /^[+-]?(\d[\d,]*\.?\d*|\.\d+)([eE][+-]?\d+)?$/;
    /** Conversion.ToDouble(text) = Convert.ToDouble(string): a float, thousands separators allowed; anything else 0. */
    function netToDouble(v) {
        var s = String(v === undefined || v === null ? '' : v).trim();
        if (!NUM_RE.test(s)) return 0;
        var n = Number(s.replace(/,/g, ''));
        return isFinite(n) ? n : 0;
    }
    /** Conversion.ToInt(value) */
    function netToInt(v) {
        var s = String(v === undefined || v === null ? '' : v).trim();
        if (!/^[+-]?\d+$/.test(s)) return 0;
        var n = Number(s);
        return (n > 2147483647 || n < -2147483648) ? 0 : n;
    }
    /**
     * Storing a text in a typeof(double) DataColumn (:179, :180, :182): the text must be a number, else
     * the DataTable refuses it with this message (shown by the handler's MessageBox).
     */
    function toDoubleColumn(text, column) {
        var s = String(text === undefined || text === null ? '' : text).trim();
        if (!NUM_RE.test(s) || !isFinite(Number(s.replace(/,/g, '')))) {
            throw new Error('Input string was not in a correct format.Couldn\'t store <' + text + '> in ' + column + ' Column.  Expected type is Double.');
        }
        return Number(s.replace(/,/g, ''));
    }
    function pad(n) { return String(n).padStart(2, '0'); }
    function isoOf(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function today() { return isoOf(new Date()); }
    function isoDay(v) { var m = /^(\d{4}-\d{2}-\d{2})/.exec(String(v || '')); return m ? m[1] : ''; }
    function dMMMyyyy(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || ''));
        return m ? m[3] + '-' + MONTHS[+m[2] - 1] + '-' + m[1] : '';
    }
    function msg(e) { return e && e.message ? e.message : String(e); }
    function show(el, on) { if (typeof el === 'string') el = $(el); if (el) el.hidden = !on; }

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
     * Button lock: disabled + spinner at once, further clicks ignored while the work is in flight,
     * released on success AND on failure. Uses the house busy panel of countx_purchase_request.js.
     * Any refusal is shown the way the desktop shows it — a MessageBox with the message text.
     */
    var busy = new WeakSet();
    async function withBusy(button, work) {
        if (button) {
            if (busy.has(button) || button.disabled) return;
            busy.add(button);
            button.classList.add('btn-busy');
        }
        var runner = window.PurchaseRequest ? window.PurchaseRequest.run.bind(window.PurchaseRequest)
            : async function (b, w) {
                var was = b ? b.disabled : false;
                if (b) b.disabled = true;
                try { return await w(); } finally { if (b) b.disabled = was; }
            };
        try {
            await runner(button || null, async function () {
                try { await work(); } catch (e) { alert(msg(e)); }
            });
        } finally {
            if (button) { button.classList.remove('btn-busy'); busy.delete(button); }
        }
    }

    // ============================================================================ combos

    function opt(value, text, attrs) {
        var a = '';
        if (attrs) Object.keys(attrs).forEach(function (k) { a += ' data-' + k + '="' + esc(attrs[k]) + '"'; });
        return '<option value="' + esc(value) + '"' + a + '>' + esc(text) + '</option>';
    }
    /** BindDDL / BindDDLNew with ZeroIndex false: no "...Select Any Value..." row, nothing selected. */
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
    /** combo.ActiveRow != null — a row of the combo's own list is selected. */
    function activeRow(id) {
        var el = $(id);
        return el.selectedIndex >= 0 && !el.options[el.selectedIndex].hasAttribute('data-adhoc');
    }
    /** combo.Value — "" when no list row is selected. */
    function comboValue(id) { var el = $(id); return activeRow(id) ? el.options[el.selectedIndex].value : ''; }
    /** combo.Text */
    function comboText(id) { var el = $(id); return el.selectedIndex < 0 ? '' : el.options[el.selectedIndex].textContent; }
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

    function bindLookups() {
        /* ApplyOnFill (:235) — rows[0] is activated (:243) */
        if ((L.applyOn || []).length > 0) { fill('cmbApplyOn', L.applyOn, 'Id', 'type'); $('cmbApplyOn').selectedIndex = 0; }
        else $('cmbApplyOn').innerHTML = '';
        /* ItemBind (:192) — only when the procedure returned rows */
        if ((L.items || []).length > 0) {
            fill('cmbitem', L.items, 'Id', 'ItemName', function (r) {
                return { category: r.ItemCategory, code: r.ItemCode, parent: r.InventoryParentCategoriesId };
            });
        }
        /* LabItemPerameterFill (:209) */
        if ((L.parameters || []).length > 0) fill('cmbparameter', L.parameters, 'Id', 'AnalysisParameterDescription');
        /* DeductionTypeFill (:256) */
        if ((L.deductionOn || []).length > 0) fill('cmbdeduction', L.deductionOn, 'Id', 'type'); else $('cmbdeduction').innerHTML = '';
        /* UOM (:276) */
        if ((L.uom || []).length > 0) fill('cmbweight', L.uom, 'Id', 'type'); else $('cmbweight').innerHTML = '';
    }

    // ============================================================================ grids

    function shownCols() {
        return GRD_COLS.filter(function (c) { return !(c.hiddenBySettings && !st.showPerameterId); });
    }
    function txt(v) { return v === null || v === undefined ? '' : String(v); }

    function renderGrd() {
        var t = $('grd'), cols = shownCols();
        if (!st.structure) {                                                 // grd.ClearStructure() (:663)
            t.tHead.innerHTML = '';
            t.tBodies[0].innerHTML = '';
            $('grdNav').textContent = 'Record 0 of 0';
            return;
        }
        t.tHead.innerHTML = '<tr>' + cols.map(function (c) {
            return '<th' + (c.type === 'num' ? ' class="n"' : '') + '>' + esc(c.caption) + '</th>';
        }).join('') + '</tr>';
        t.tBodies[0].innerHTML = st.table.map(function (r, i) {
            return '<tr data-i="' + i + '"' + (i === st.gridSel ? ' class="is-sel"' : '') + '>' + cols.map(function (c) {
                return '<td' + (c.type === 'num' ? ' class="n"' : '') + '>' + esc(txt(r[c.key])) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        $('grdNav').textContent = 'Record ' + (st.table.length && st.gridSel >= 0 ? st.gridSel + 1 : 0) + ' of ' + st.table.length;
    }
    /** grd.DataSource = table; grd.RetrieveStructure(); [grdSettings()] */
    function bindGrd(withSettings) {
        st.structure = true;
        st.showPerameterId = !withSettings;
        if (st.gridSel >= st.table.length) st.gridSel = st.table.length - 1;
        if (st.gridSel < 0 && st.table.length) st.gridSel = 0;
        renderGrd();
    }

    function renderHist() {
        var t = $('grdhistory');
        t.tHead.innerHTML = '<tr>' + HIST_COLS.map(function (c) { return '<th>' + esc(c.caption) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = st.hist.map(function (r, i) {
            return '<tr data-i="' + i + '"' + (i === st.histSel ? ' class="is-sel"' : '') + '>' + HIST_COLS.map(function (c) {
                var v = r[c.key];
                if (c.type === 'date') return '<td>' + esc(dMMMyyyy(v)) + '</td>';
                if (c.type === 'link') return '<td><a class="lnk" href="#" data-open="1" title="Open this policy">' + esc(txt(v) || '(open)') + '</a></td>';
                return '<td>' + esc(txt(v)) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        $('grdhistoryNav').textContent = 'Record ' + (st.hist.length && st.histSel >= 0 ? st.histSel + 1 : 0) + ' of ' + st.hist.length;
    }

    // ============================================================================ detail entry

    /** IsBetween (:346) — written with "||" on the desktop: item >= start || item <= end. Reproduced as written. */
    function isBetween(item, start, end) { return item >= start || item <= end; }

    /** FormValidationDetail (:453) — desktop order and texts. */
    function formValidationDetail() {
        if (!activeRow('cmbparameter')) { alert('Parameter Field is Required'); focusCombo('cmbparameter'); return false; }
        if (netToDouble($('txtRangeFrom').value) >= netToDouble($('txtrangeTo').value)) {
            alert('Range From Must Be less Then Range To Thank You'); $('txtRangeFrom').focus(); return false;
        }
        if (netToDouble($('txtRangeFrom').value) <= 0) { alert('Range From Field is Required'); $('txtRangeFrom').focus(); return false; }
        if (netToDouble($('txtrangeTo').value) <= 0) { alert('Range To Field is Required'); $('txtrangeTo').focus(); return false; }
        if (!activeRow('cmbdeduction')) { alert('Deduction On Field is Required'); focusCombo('cmbdeduction'); return false; }
        if (netToDouble($('TxtDecValue').value) <= 0) { alert('Deduction Value Field is Required'); focusCombo('cmbdeduction'); return false; }
        if (!activeRow('cmbweight')) { alert('Weight Kg Field is Required'); focusCombo('cmbweight'); return false; }
        return true;
    }

    /** Add_Click (:296) */
    function addClick() {
        try {
            if (!formValidationDetail()) return;
            var pid = netToInt(comboValue('cmbparameter'));
            var ded = comboText('cmbdeduction').trim();
            var from = netToDouble($('txtRangeFrom').value), to = netToDouble($('txtrangeTo').value);
            for (var i = 0; i < st.table.length; i++) {
                var r = st.table[i];
                if (netToInt(r.PerameterId) === pid && txt(r.DeductionOn) !== ded) {
                    throw new Error('This parameter deduction on must be same please check');
                }
                if (netToInt(r.PerameterId) === pid) {
                    if (isBetween(from, netToDouble(r.RangeFrom), netToDouble(r.RangeTo))) throw new Error('Exist');
                    if (isBetween(to, netToDouble(r.RangeFrom), netToDouble(r.RangeTo))) throw new Error('Exist');
                }
            }
            st.table.push({
                PerameterId: comboValue('cmbparameter'),
                AnalysisPerameter: comboText('cmbparameter').trim(),
                RangeFrom: toDoubleColumn($('txtRangeFrom').value, 'RangeFrom'),
                RangeTo: toDoubleColumn($('txtrangeTo').value, 'RangeTo'),
                DeductionOn: ded,
                DeductionValue: toDoubleColumn($('TxtDecValue').value, 'DeductionValue'),
                Weight: comboText('cmbweight').trim()
            });
            bindGrd(true);                                                   // :328-330
            clearDetailBoxes();                                              // :331-336
            focusCombo('cmbparameter');
        } catch (e) { alert(msg(e)); }
    }

    function clearDetailBoxes() {
        setText('cmbparameter', '');
        $('txtRangeFrom').value = '';
        $('txtrangeTo').value = '';
        setText('cmbdeduction', '');
        $('TxtDecValue').value = '';
        setText('cmbweight', '');
    }

    /** DetailReset (:635) */
    function detailReset() {
        focusCombo('cmbparameter');
        clearDetailBoxes();
        show('btnUpdateDetail', false);
        show('btnAdd', true);
        show('btnCancelDetail', false);
    }

    /**
     * btnUpdateDetail_Click (:372) — no validation; the cells are assigned one after the other, so a text
     * that a double column refuses stops the handler there (the cells before it are already changed).
     * The structure is retrieved WITHOUT grdSettings (:384), so the PerameterId column shows until the
     * next Add / load. Reproduced as written.
     */
    function updateDetailClick() {
        try {
            var r = st.table[st.updateDetailIndex];
            if (!r) throw new Error('There is no row at position ' + st.updateDetailIndex + '.');
            try {
                r.PerameterId = comboValue('cmbparameter');
                r.AnalysisPerameter = comboText('cmbparameter');
                r.RangeFrom = toDoubleColumn($('txtRangeFrom').value, 'RangeFrom');
                r.RangeTo = toDoubleColumn($('txtrangeTo').value, 'RangeTo');
                r.DeductionOn = comboText('cmbdeduction');
                r.DeductionValue = toDoubleColumn($('TxtDecValue').value, 'DeductionValue');
                r.Weight = comboText('cmbweight');
            } catch (e) { renderGrd(); throw e; }
            bindGrd(false);
            show('btnAdd', true);
            show('btnUpdateDetail', false);
            show('btnCancelDetail', false);
            detailReset();
        } catch (e) { alert(msg(e)); }
    }

    /** btnCancelDetail_Click (:396) */
    function cancelDetailClick() {
        detailReset();
        show('btnUpdateDetail', false);
    }

    /** grd_DoubleClick (:409) */
    function grdDoubleClick(index) {
        var r = st.table[index];
        if (!r) return;
        st.updateDetailIndex = index;
        setVal('cmbparameter', txt(r.PerameterId));
        setText('cmbparameter', txt(r.AnalysisPerameter));
        $('txtRangeFrom').value = txt(r.RangeFrom);
        $('txtrangeTo').value = txt(r.RangeTo);
        setText('cmbdeduction', txt(r.DeductionOn));
        $('TxtDecValue').value = txt(r.DeductionValue);
        setText('cmbweight', txt(r.Weight));
        show('btnAdd', false);
        show('btnUpdateDetail', true);
        show('btnCancelDetail', true);
        Array.prototype.forEach.call($('grd').tBodies[0].rows, function (tr) { tr.classList.toggle('is-edit', intOf(tr.getAttribute('data-i')) === index); });
        focusCombo('cmbparameter');
    }

    // ============================================================================ header

    /** FormValidation (:436) */
    function formValidation() {
        if (!activeRow('cmbApplyOn')) { alert('ApplyOn field is required'); focusCombo('cmbApplyOn'); return false; }
        if (!activeRow('cmbitem')) { alert('ItemName Field is Required'); focusCombo('cmbitem'); return false; }
        return true;
    }

    /** Reset (:656) — Apply On and the two dates are left as they are. */
    function reset() {
        show('btnSave', true);
        show('btnUpdate', false);
        st.table = [];
        st.structure = false;
        st.gridSel = -1;
        renderGrd();
        focusCombo('cmbitem');
        setText('cmbitem', '');
        $('txtremarks').value = '';
        st.recId = 0;
        detailReset();
    }

    /** Insert (:516) */
    async function insert(button) {
        if (st.table.length === 0) { alert('Grid Record Not Found'); return; }                   // :520
        if (!formValidation()) return;
        var update = st.recId > 0;
        if (!confirm(update ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;      // :526 / :532
        await withBusy(button, async function () {
            var res = await api('POST', API + '/save', {
                id: update ? st.recId : 0,
                policyApplyOn: comboText('cmbApplyOn'),
                itemId: netToInt(comboValue('cmbitem')),
                effectiveFrom: $('txtAffectiveDate').value || today(),
                effectiveTo: $('TxtAffectiveDateTo').value || today(),
                remarksHeader: $('txtremarks').value,
                rows: st.table.map(function (r) {
                    return {
                        perameterId: txt(r.PerameterId), analysisPerameter: txt(r.AnalysisPerameter),
                        rangeFrom: txt(r.RangeFrom), rangeTo: txt(r.RangeTo), deductionOn: txt(r.DeductionOn),
                        deductionValue: txt(r.DeductionValue), weight: txt(r.Weight)
                    };
                })
            });
            alert((res && res.message) || (update ? 'Update SuccessFully' : 'Save SuccessFully'));   // :563 / :567
            reset();
        });
    }

    /** btnSave_Click (:586) */
    function saveClick() { st.recId = 0; insert($('btnSave')); }
    /** btnUpdate_Click (:500) */
    function updateClick() {
        if (st.recId === 0) { alert('Recod cannot be updated because RecId not found'); return; }
        insert($('btnUpdate'));
    }
    /** btnNew_Click (:677) */
    function newClick() { withBusy($('btnNew'), async function () { reset(); }); }

    /** getUpdate (:600) */
    function getUpdate(id, button) {
        id = intOf(id);
        st.recId = id;                                                       // :606
        return withBusy(button || null, async function () {
            var lab = await api('GET', API + '/' + id);
            if (!lab) return;
            show('btnSave', false);
            show('btnUpdate', true);
            selectTab('tabForm', true);                                      // tabControl1.SelectedIndex = 0
            setText('cmbApplyOn', lab.PolicyApplyOn);
            setVal('cmbitem', lab.ItemId);
            $('txtAffectiveDate').value = isoDay(lab.EffectiveFrom) || today();
            $('TxtAffectiveDateTo').value = isoDay(lab.EfffectiveTo) || today();
            $('txtremarks').value = txt(lab.RemarksHeader);
            st.table = (lab.rows || []).map(function (r) {
                return {
                    PerameterId: txt(r.PerameterId), AnalysisPerameter: txt(r.AnalysisPerameter),
                    RangeFrom: Number(r.RangeFrom) || 0, RangeTo: Number(r.RangeTo) || 0,
                    DeductionOn: txt(r.DeductionOn), DeductionValue: Number(r.DeductionValue) || 0, Weight: txt(r.Weight)
                };
            });
            st.gridSel = st.table.length ? 0 : -1;
            bindGrd(true);                                                   // :621-623
        });
    }

    // ============================================================================ history / tabs

    /** gridhistoryfill (:712) */
    function gridHistoryFill(button) {
        return withBusy(button || null, async function () {
            st.hist = (await api('GET', API + '/history')) || [];
            st.histSel = st.hist.length ? 0 : -1;
            renderHist();
        });
    }
    function selectTab(id, silent) {
        ['tabForm', 'tabHistory'].forEach(function (t) { $(t).classList.toggle('is-active', t === id); });
        $('tabBtnForm').classList.toggle('is-active', id === 'tabForm');
        $('tabBtnHistory').classList.toggle('is-active', id === 'tabHistory');
        if (id === 'tabHistory' && !silent) gridHistoryFill(null);           // tabControl1_SelectedIndexChanged :704
    }
    /** The footer History button: opens the History page (the same as choosing the History tab). */
    function historyClick() {
        ['tabForm', 'tabHistory'].forEach(function (t) { $(t).classList.toggle('is-active', t === 'tabHistory'); });
        $('tabBtnForm').classList.remove('is-active');
        $('tabBtnHistory').classList.add('is-active');
        gridHistoryFill($('btnHistory'));
    }

    function toggleFull(boxId, btnId) {
        var on = $(boxId).classList.toggle('is-full');
        $(btnId).innerHTML = '<i class="fa ' + (on ? 'fa-compress' : 'fa-expand') + '"></i>';
        $(btnId).title = on ? 'Exit full screen' : 'Full screen';
    }

    // ============================================================================ keys

    /** CommonServices.OnlytextdecimelFunction (CommonServices.cs:2475) — digits and one '.'. */
    function decimalGuard(el) {
        el.addEventListener('keypress', function (e) {
            if (e.ctrlKey || e.metaKey || e.altKey || !e.key || e.key.length !== 1) return;
            if (!/[0-9.]/.test(e.key)) { e.preventDefault(); return; }
            if (e.key === '.' && el.value.indexOf('.') > -1) e.preventDefault();
        });
    }

    /**
     * The form has no KeyDown handler; its only keys are the ToolStrip mnemonics &New, &Save, &Update
     * (Alt+N / Alt+S / Alt+U — a hidden ToolStrip button does not answer its mnemonic). &Print has no
     * Click handler and &Attachment is hidden with an empty handler. Escape leaves grid full screen.
     */
    function keyDown(e) {
        if (e.defaultPrevented) return;
        var k = (e.key || '').toLowerCase();
        if (k === 'escape') {
            ['grdBox', 'grdhistoryBox'].forEach(function (b, i) {
                if ($(b).classList.contains('is-full')) { toggleFull(b, i === 0 ? 'grdFull' : 'grdhistoryFull'); e.preventDefault(); }
            });
            return;
        }
        if (!e.altKey || e.ctrlKey || e.metaKey) return;
        if (!$('tabForm').classList.contains('is-active')) return;
        if (k === 'n') { e.preventDefault(); newClick(); }
        else if (k === 's') { e.preventDefault(); if (!$('btnSave').hidden) saveClick(); }
        else if (k === 'u') { e.preventDefault(); if (!$('btnUpdate').hidden) updateClick(); }
    }

    // ============================================================================ wiring

    function wire() {
        $('btnNew').addEventListener('click', newClick);
        $('btnSave').addEventListener('click', saveClick);
        $('btnUpdate').addEventListener('click', updateClick);

        $('btnAdd').addEventListener('click', addClick);
        $('btnUpdateDetail').addEventListener('click', updateDetailClick);
        $('btnCancelDetail').addEventListener('click', cancelDetailClick);

        decimalGuard($('txtRangeFrom'));                                     // :689
        decimalGuard($('txtrangeTo'));                                       // :694
        decimalGuard($('TxtDecValue'));                                      // :699

        var grd = $('grd');
        grd.addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr');
            if (!tr) return;
            st.gridSel = intOf(tr.getAttribute('data-i'));
            Array.prototype.forEach.call(grd.tBodies[0].rows, function (r) { r.classList.toggle('is-sel', r === tr); });
            $('grdNav').textContent = 'Record ' + (st.gridSel + 1) + ' of ' + st.table.length;
        });
        /* grd_DoubleClick (:409) works on grd.CurrentRow */
        grd.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tbody tr');
            if (tr) grdDoubleClick(intOf(tr.getAttribute('data-i')));
            else if (st.gridSel >= 0) grdDoubleClick(st.gridSel);
        });

        var gh = $('grdhistory');
        gh.addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr');
            if (!tr) return;
            st.histSel = intOf(tr.getAttribute('data-i'));
            Array.prototype.forEach.call(gh.tBodies[0].rows, function (r) { r.classList.toggle('is-sel', r === tr); });
            $('grdhistoryNav').textContent = 'Record ' + (st.histSel + 1) + ' of ' + st.hist.length;
            if (e.target.closest('a[data-open]')) {                          // the link = grdhistory_DoubleClick (:745)
                e.preventDefault();
                var row = st.hist[st.histSel];
                if (row) getUpdate(row.Id, null);
            }
        });
        gh.addEventListener('dblclick', function (e) {
            if (e.target.closest('a[data-open]')) return;                    // the link already opened it
            var tr = e.target.closest('tbody tr');
            var row = tr ? st.hist[intOf(tr.getAttribute('data-i'))] : st.hist[st.histSel];
            if (row) getUpdate(row.Id, null);
        });

        $('tabBtnForm').addEventListener('click', function () { selectTab('tabForm'); });
        $('tabBtnHistory').addEventListener('click', function () { selectTab('tabHistory'); });
        $('btnHistory').addEventListener('click', historyClick);

        $('grdFull').addEventListener('click', function () { toggleFull('grdBox', 'grdFull'); });
        $('grdhistoryFull').addEventListener('click', function () { toggleFull('grdhistoryBox', 'grdhistoryFull'); });

        document.addEventListener('keydown', keyDown);
    }

    /** Lab_Load (:163) */
    async function init() {
        wire();
        try {
            L = await api('GET', API + '/lookups');
        } catch (e) {
            $('rightsNote').textContent = msg(e);
            show('rightsNote', true);
            ['btnNew', 'btnSave', 'btnUpdate', 'btnAdd', 'btnUpdateDetail', 'btnCancelDetail', 'btnHistory'].forEach(function (id) { $(id).disabled = true; });
            return;
        }
        show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelDetail', false);    // :167-169
        show('btnUpdate', false); show('btnSave', true);                                         // :170-171
        $('txtAffectiveDate').value = isoDay(L.today) || today();            // DateTimePicker default
        $('TxtAffectiveDateTo').value = isoDay(L.today) || today();
        bindLookups();                                                       // :173-177
        renderGrd();
        focusCombo('cmbitem');                                               // :172
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init);
    else init();
})();
