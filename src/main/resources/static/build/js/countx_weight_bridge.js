/* ============================================================================================
 * Screen 411 "Weigh Bridge" — Architecture.WinApp.WeightBridge.frmWeightbridge, DocTypeId 102.
 *
 * Each function below names the desktop method it reproduces. The order of operations inside
 * each is the desktop's, including where the desktop reads a combo's TEXT rather than its value
 * (the reference type is decided almost everywhere by its caption, "GatePass Inward",
 * "GatePass OutWard", "Move Order", "General", ...).
 *
 * THE INFRAGISTICS TEXT/VALUE MODEL
 * The desktop's Party, Item, Vehicle Type and Wb Type combos let the operator type free text
 * (LimitToList = false). Their saved value is the TEXT; their Value is the matching row's Id, or
 * nothing when the text matches no row; and FormValidation tests "ActiveRow != null", i.e. "does
 * the text match a row". They are therefore <input list=…> fields here, resolved against their
 * rows the same way — never a select that would forbid the free text the desktop allows.
 *
 * WEIGHTS ARE READ, NOT TYPED
 * txtFirstWeight / txtsecondWeight are disabled and read-only. F5 runs
 * ReadWeightFromWeightBridge():4181, which opens the indicator's serial port with the settings of
 * the first weighbridge scale and parses the text by the scale's ModelName. A web page reaches a
 * serial port only through the browser's Web Serial API (Chrome / Edge, on the PC the indicator
 * is plugged into), so that is what is used. The "Enable Weights" checkbox that unlocks manual
 * entry is hidden exactly as on the desktop, and revealed by the same Shift+Z,A,I,N sequence.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/weighbridge';

    if (window.DesktopCombo) {
        /* GetGatePassOutward:923 binds Id, GP, VehicleNo, GatepassType with every column shown
           but the value column. The other gate-pass lists show one column. */
        window.DesktopCombo.define('wbGatePass', [
            { caption: 'GpNo',         flex: 2 },
            { caption: 'VehicleNo',    flex: 2, key: 'vehicle' },
            { caption: 'GatepassType', flex: 2, key: 'gptype' }
        ]);
    }

    /* ------------------------------------------------------------------ state */

    var L = null;                       // lookups
    var rights = { canSave: false, canUpdate: false, canPrint: false, canViewAllRecord: false };
    var cfg = {};
    var RECID = 0;
    var UpdateMode = false;
    var Approved = false;
    var RefDocumentTypeId = 0;          // the gate pass's RefDocumentTypeId (41, 105, 106, 110 …)
    var SupplierFirstWeight = 0;        // the gate pass's supplier weights, for the Save confirm
    var SupplierSecondWeight = 0;
    var dtPartiesAndItemsFromDeliveryOrder = [];
    var firstDate = new Date();         // txtFirstDate.Value
    var secondDate = new Date();        // txtSecondDate.Value
    var keySequence = '';
    var loadedRow = null;
    /* UniqFileName..UniqFileName3 / UniqFileName4..6 — the picture names Save / Update write. */
    var pics = { first: ['', '', ''], second: ['', '', ''] };
    var liveBoxes = [];            // boxes showing the cameras live (HVUtility.Preview)
    var liveTimer = null;

    /* combo sources — {Id, <display>, …} */
    var src = {
        wbType:      { rows: [], key: 'GpTypeDescription', limit: false },
        vehicleType: { rows: [], key: 'VehicleDescription', limit: false },
        party:       { rows: [], key: 'PartyName', limit: false },
        item:        { rows: [], key: 'ItemName', limit: false }
    };
    var refDocRows = [];
    var invoiceRows = [];
    var pendingSecondRows = [], pendingFirstRows = [], historyRows = [];

    /* ------------------------------------------------------------------ dom helpers */

    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : String(v); }
    function enable(id, on) { var e = $id(id); if (e) e.disabled = !on; }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    /** Focus a control — for an enhanced combo, its visible text box, not the hidden select. */
    function focusEl(id) {
        var e = $id(id);
        if (!e) return;
        var w = e.closest ? e.closest('.dtcombo-wrap') : null;
        var target = w ? w.querySelector('input') : e;
        if (target && target.focus) target.focus();
    }
    function box(m) { window.alert(m); }
    function ask(m) { return window.confirm(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function ci(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var lower = name.toLowerCase();
        for (var k in row) {
            if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === lower) return row[k];
        }
        return '';
    }
    function str(v) { return (v === null || v === undefined) ? '' : String(v); }
    /** Conversion.ToDouble — 0 for anything that does not parse. */
    function num(v) {
        var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, ''));
        return isNaN(n) ? 0 : n;
    }
    /** Conversion.ToInt. */
    function intOf(v) { return Math.round(num(v)); }
    /** double.ToString() — no trailing ".0". */
    function dstr(n) {
        if (!isFinite(n)) return '0';
        var r = Math.round(n * 1e10) / 1e10;
        return String(r);
    }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function isoDay(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function isoLocal(d) { return isoDay(d) + 'T' + pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds()); }
    function timeOf(d) { return pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds()); }
    function daysAgo(n) { var d = new Date(); d.setDate(d.getDate() - n); return d; }
    function parseDate(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2})(?::(\d{2}))?)?/.exec(str(v));
        if (!m) return null;
        return new Date(+m[1], +m[2] - 1, +m[3], +(m[4] || 0), +(m[5] || 0), +(m[6] || 0));
    }
    /** ToShortDateString on a Pakistani-culture PC: dd/MM/yyyy. */
    function shortDate(v) {
        var d = parseDate(v);
        return d ? pad(d.getDate()) + '/' + pad(d.getMonth() + 1) + '/' + d.getFullYear() : str(v);
    }
    /** "#,##0.###" */
    function fmt(v, decimals) {
        if (v === '' || v === null || v === undefined) return '';
        var n = num(v);
        return n.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: decimals === undefined ? 3 : decimals });
    }

    function fetchJson(url, opts) {
        return fetch(url, Object.assign({ credentials: 'same-origin', headers: { 'Accept': 'application/json' } }, opts || {}))
            .then(function (r) {
                return r.text().then(function (t) {
                    var body = null;
                    try { body = t ? JSON.parse(t) : null; } catch (e) { body = null; }
                    if (!r.ok) {
                        var m = body && body.message ? body.message : ('Request failed (' + r.status + ')');
                        var err = new Error(m); err.status = r.status; throw err;
                    }
                    return body;
                });
            });
    }
    function get(path, params) {
        var q = params ? ('?' + new URLSearchParams(params).toString()) : '';
        return fetchJson(api + path + q);
    }
    function post(path, body) {
        return fetchJson(api + path, {
            method: 'POST',
            headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' },
            body: JSON.stringify(body)
        });
    }
    function busy(id, on) { var b = $id(id); if (b) { b.classList.toggle('is-busy', on); b.disabled = on || b.dataset.denied === '1'; } }

    /* ------------------------------------------------------------------ select helpers */

    /** Bind a <select> the way BindAndRetainSelection / DDL.BindDDL do. blankFirst = the "no
     *  active row" state an UltraCombo has before a row is picked. */
    function bindSelect(id, rows, valueKey, textKey, opts) {
        opts = opts || {};
        var sel = $id(id);
        if (!sel) return;
        var html = '';
        if (opts.blank) html += '<option value=""></option>';
        if (opts.defaultRow) html += '<option value="0">-- Select --</option>';
        (rows || []).forEach(function (r) {
            var extra = '';
            if (opts.attrs) opts.attrs.forEach(function (a) { extra += ' data-' + a[0] + '="' + esc(ci(r, a[1])) + '"'; });
            html += '<option value="' + esc(ci(r, valueKey)) + '"' + extra + '>' + esc(ci(r, textKey)) + '</option>';
        });
        sel.innerHTML = html;
    }
    function selectedText(id) {
        var sel = $id(id);
        if (!sel || sel.selectedIndex < 0) return '';
        var o = sel.options[sel.selectedIndex];
        return o && o.value !== '' ? o.textContent : '';
    }
    function hasActiveRow(id) {
        var sel = $id(id);
        return !!(sel && sel.selectedIndex >= 0 && sel.options[sel.selectedIndex] && sel.options[sel.selectedIndex].value !== '');
    }
    /** Combo.Text = x — first option with that caption, else no active row. */
    function setSelectByText(id, text) {
        var sel = $id(id);
        if (!sel) return;
        var t = str(text);
        for (var i = 0; i < sel.options.length; i++) {
            if (sel.options[i].value !== '' && sel.options[i].textContent === t) { sel.selectedIndex = i; return; }
        }
        sel.value = '';
        if (sel.value !== '') sel.selectedIndex = -1;
    }
    /** Combo.Text = x on a combo whose text is saved even when no row matches (cmbPackingType):
     *  keep the text as a value-0 option so it is what gets saved, as the desktop does. */
    function setSelectTextKeep(id, text) {
        setSelectByText(id, text);
        if (!hasActiveRow(id) && str(text) !== '') {
            var sel = $id(id);
            var o = document.createElement('option');
            o.value = '0'; o.textContent = str(text); o.setAttribute('data-free', '1');
            sel.insertBefore(o, sel.firstChild);
            sel.value = '0';
        }
    }
    function selectValue(id, v) {
        var sel = $id(id);
        if (!sel) return;
        sel.value = str(v);
        if (sel.value !== str(v)) sel.value = '';
    }

    /* ------------------------------------------------------------------ free-text combos */

    function bindText(which, inputId, listId, rows, opts) {
        opts = opts || {};
        var s = src[which];
        s.rows = (rows || []).slice();
        if (opts.defaultRow) {
            var d = { Id: 0 }; d[s.key] = '-- Select --';
            s.rows.unshift(d);
        }
        s.limit = !!opts.limitToList;
        var dl = $id(listId);
        if (dl) {
            dl.innerHTML = s.rows.map(function (r) {
                var label = opts.labelKey ? str(ci(r, opts.labelKey)) : '';
                return '<option value="' + esc(ci(r, s.key)) + '"' + (label ? ' label="' + esc(label) + '"' : '') + '></option>';
            }).join('');
        }
    }
    /** The row whose caption equals the text — Infragistics matches case-insensitively. */
    function textRow(which, inputId) {
        var t = val(inputId);
        var s = src[which];
        if (t === '') return null;
        var lower = t.toLowerCase();
        for (var i = 0; i < s.rows.length; i++) {
            if (str(ci(s.rows[i], s.key)).toLowerCase() === lower) return s.rows[i];
        }
        return null;
    }
    function textValue(which, inputId) { var r = textRow(which, inputId); return r ? intOf(ci(r, 'Id')) : 0; }
    function setTextById(which, inputId, id) {
        var s = src[which];
        for (var i = 0; i < s.rows.length; i++) {
            if (intOf(ci(s.rows[i], 'Id')) === intOf(id)) { setVal(inputId, ci(s.rows[i], s.key)); return true; }
        }
        return false;
    }
    /** A LimitToList combo refuses text that is not one of its rows. */
    function enforceLimit(which, inputId) {
        if (src[which].limit && val(inputId) !== '' && !textRow(which, inputId)) setVal(inputId, '');
    }

    // ===================================================================================== load

    function init() {
        var today = new Date();
        setVal('FromDate', isoDay(daysAgo(3)));            // :705 — DefaultDaysToLess… is never set, so 3
        setVal('ToDate', isoDay(today));
        setVal('FromGpDatePendingFor2nd', isoDay(today));  // designer default: DateTime.Now
        setVal('ToGpDatePendingFor2nd', isoDay(today));
        setVal('FromDatePendingFor1st', isoDay(today));
        setVal('ToDatePendingFor1st', isoDay(today));
        stampTimes();
        wireEvents();

        get('/lookups').then(function (d) {
            L = d;
            rights.canSave = !!d.canSave;
            rights.canUpdate = !!d.canUpdate;
            rights.canPrint = !!d.canPrint;
            rights.canViewAllRecord = !!d.canViewAllRecord;
            cfg = d;

            /* :688-690 */
            applyRights();

            if (RECID === 0) setVal('txtTickectNo', d.ticketNo);
            /* GatePassReferenceTypeBind:785 — no default row, no row activated. */
            bindSelect('cmbReferenceType', d.referenceTypes, 'Id', 'Name', { blank: true });
            bindSelect('CmbGatePassTypeHistory', d.referenceTypes, 'Id', 'Name', { blank: true });
            /* VehicleTypesBind:815 — insertDefaultRow: true. */
            bindText('vehicleType', 'cmbVehicleType', 'dlVehicleType', d.vehicleTypes, { defaultRow: true });
            setVal('cmbVehicleType', '-- Select --');
            /* WbType:1197 — no default row; row 0 is activated on the form, none on History. */
            bindText('wbType', 'CmbWeighBridgeType', 'dlWbType', d.wbTypes, {});
            if (src.wbType.rows.length) setVal('CmbWeighBridgeType', ci(src.wbType.rows[0], 'GpTypeDescription'));
            bindSelect('CmbWbTypeHistory', d.wbTypes, 'Id', 'GpTypeDescription', { blank: true });
            /* ItemDtFillFromGlobal / PackingTypeBindFromGlobal / PartyBind / ItemBind */
            packingTypeBind(d.packingTypes);
            partyBind(false);
            itemBind();
            /* GetConfigurationsFromGlobal:748 / :778 */
            if (intOf(d.defaultPackingTypeId) !== 0) selectValue('cmbPackingType', d.defaultPackingTypeId);
            show('GroupInwardLabInfo', !!d.showInwardLabInfo);
            resetPictures();
            initCameras(false, FIRST_BOXES);        // frmWeightbridge_Load → LoadCamera()

            /* InwardGatePass.WBForm(GPId) opens this form with the reference type set to 51,
               runs cmbReferenceType_Leave, and puts the new gate pass in cmbRefDocNo. */
            var qs = new URLSearchParams(window.location.search);
            var gp = intOf(qs.get('gatePassId'));
            if (qs.has('gatePassId')) {
                selectValue('cmbReferenceType', 51);
                cmbReferenceType_Leave().then(function () {
                    if (gp > 0) {
                        selectValue('cmbRefDocNo', gp);
                        if (hasActiveRow('cmbRefDocNo')) cmbRefDocNo_Leave();
                    }
                });
            }
            focusEl('cmbReferenceType');
        }).catch(function (e) { box(e.message); });
    }

    function applyRights() {
        var s = $id('btnsave'), u = $id('btnUpdate'), p = $id('btnPrint');
        s.disabled = !rights.canSave;   s.dataset.denied = rights.canSave ? '' : '1';
        u.disabled = !rights.canUpdate; u.dataset.denied = rights.canUpdate ? '' : '1';
        p.disabled = !rights.canPrint;  p.dataset.denied = rights.canPrint ? '' : '1';
        var missing = [];
        if (!rights.canSave) missing.push('Save');
        if (!rights.canUpdate) missing.push('Update');
        if (!rights.canPrint) missing.push('Print');
        var note = $id('rightsNote');
        if (missing.length) {
            note.textContent = 'Your user rights on Weigh Bridge do not include: ' + missing.join(', ') + '.';
            show('rightsNote', true);
        }
    }

    /** "btnsave.Visible && btnsave.Enabled" — the form is in Save mode AND the Save right is held. */
    function saveActive()   { return !$id('btnsave').classList.contains('is-hidden') && !$id('btnsave').disabled; }
    function updateActive() { return !$id('btnUpdate').classList.contains('is-hidden') && !$id('btnUpdate').disabled; }

    function stampTimes() {
        firstDate = new Date();
        secondDate = new Date();
        setVal('txtFirstDate', timeOf(firstDate));
        setVal('txtSecondDate', timeOf(secondDate));
    }

    function packingTypeBind(rows) {
        var prev = val('cmbPackingType');
        bindSelect('cmbPackingType', rows, 'Id', 'PackTypeDesc', { defaultRow: true });
        if (prev !== '' && prev !== '0') selectValue('cmbPackingType', prev);
        if (!hasActiveRow('cmbPackingType')) selectValue('cmbPackingType', '0');
    }

    /** PartyBind(limittolist):848 — dtParty with the "-- Select --" row, retaining the value. */
    function partyBind(limitToList, rows) {
        var prev = textValue('party', 'CmbPartyName');
        bindText('party', 'CmbPartyName', 'dlParty', rows || (L ? L.wbParties : []), { defaultRow: true });
        if (src.party.rows.length > 1) src.party.limit = !!limitToList;
        if (!setTextById('party', 'CmbPartyName', prev)) setVal('CmbPartyName', '-- Select --');
    }

    /** ItemBind():880 — the global items (Id, ItemName, ItemCode, PartyId 0). */
    function itemBind() {
        var prev = textValue('item', 'CmbItem');
        var rows = (L ? L.items : []).map(function (r) {
            return { Id: ci(r, 'Id'), ItemName: ci(r, 'ItemName'), ItemCode: ci(r, 'ItemCode'), PartyId: 0 };
        });
        bindText('item', 'CmbItem', 'dlItem', rows, { defaultRow: true, labelKey: 'ItemCode' });
        if (!setTextById('item', 'CmbItem', prev)) setVal('CmbItem', '-- Select --');
    }

    /** ItemLookupsBind():864 — the weighbridge item lookups instead of the global items. */
    function itemLookupsBind() {
        var prev = textValue('item', 'CmbItem');
        bindText('item', 'CmbItem', 'dlItem', (L ? L.wbItemLookups : []), { defaultRow: true });
        if (!setTextById('item', 'CmbItem', prev)) setVal('CmbItem', '-- Select --');
    }

    /** DDL.BindDDL(dt, CmbItem, "Id", "ItemName", …, ZeroIndex: true) for the order-scoped item
     *  lists, keeping the current item when it is still in the list (:1234-1244). */
    function itemsBindKeep(rows, limitToList) {
        var prev = textValue('item', 'CmbItem');
        bindText('item', 'CmbItem', 'dlItem', rows, { defaultRow: true, labelKey: 'ItemCode' });
        src.item.limit = !!limitToList;
        if (prev > 0) {
            if (!setTextById('item', 'CmbItem', prev)) setVal('CmbItem', '');
        } else {
            setVal('CmbItem', '-- Select --');
        }
    }

    // ================================================================ reference type (bending)

    /** cmbReferenceType_Leave():3929 and its SelectionChangeCommitted twin. */
    function cmbReferenceType_Leave() {
        var text = selectedText('cmbReferenceType').trim();
        var value = intOf(val('cmbReferenceType'));
        if (text === 'Move Order' || value === 218) {
            selectValue('cmbPackingType', '0');              // Rows[0].Activate() — the default row
            enable('txtWorkingReportNo', true);
        }
        return GatePassBending();
    }

    /** GatePassBending():3816. */
    function GatePassBending() {
        clearInvoices();
        if (!saveActive()) return Promise.resolve();
        var text = selectedText('cmbReferenceType').trim();
        var value = intOf(val('cmbReferenceType'));
        function resetRefFields() {
            clearRefDoc();
            setVal('txtBilityNo', '');
            setVal('txtVehicleNo', '');
            setVal('txtItemqty', '');
            setVal('cmbVehicleType', '');
            refDocVisible(true);
        }
        if (text === 'GatePass Inward') {
            resetRefFields();
            enable('cmbRefDocNo', true);
            enable('txtContainerNo', true);
            return loadRefDocs('lab', 0, true);
        } else if (value === 1601 && text === 'GatePass Inward') {
            /* Unreachable as written — the branch above already takes this text. Kept so the
               order of the desktop's tests is visible. */
            resetRefFields();
            return loadRefDocs('engr', 0, true);
        } else if (text === 'GatePass OutWard') {
            resetRefFields();
            enable('txtContainerNo', true);
            return loadRefDocs('outward', 0, false);
        } else if (text === 'Move Order' || value === 218) {
            setVal('txtItemqty', '');
            enable('txtItemqty', true);
            enable('txtWorkingReportNo', true);
            refDocVisible(false);
            enable('cmbRefDocNo', false);
            setVal('txtrefdocNumberofMoveOrder', val('txtTickectNo'));
            enable('txtrefdocNumberofMoveOrder', false);
        } else if (text === 'General') {
            setVal('txtItemqty', '');
            enable('txtItemqty', true);
            enable('txtWorkingReportNo', true);
            refDocVisible(false);
            enable('cmbRefDocNo', false);
            setVal('txtrefdocNumberofMoveOrder', val('txtTickectNo'));
            enable('txtrefdocNumberofMoveOrder', false);
        } else if (text === 'General GatePass Inward') {
            resetRefFields();
            enable('txtWorkingReportNo', true);
            enable('txtContainerNo', true);
            return loadRefDocs('general', 52, false);
        } else if (text === 'General GatePass Outward') {
            resetRefFields();
            enable('txtWorkingReportNo', true);
            enable('txtContainerNo', true);
            return loadRefDocs('general', 92, false);
        } else if (text === 'GatePassInwardPartyProcessing' || text === 'GatePassOutwardPartyProcessing') {
            resetRefFields();
            enable('txtWorkingReportNo', true);
            enable('txtContainerNo', true);
            return loadRefDocs('party', value, false);
        }
        return Promise.resolve();
    }

    /** DesktopCombo moves the <select> inside its own wrapper, so it is the wrapper that is
     *  shown or hidden. */
    function refDocVisible(on) {
        var sel = $id('cmbRefDocNo');
        var wrap = sel && sel.closest ? sel.closest('.dtcombo-wrap') : null;
        (wrap || sel).classList.toggle('is-hidden', !on);
        show('txtrefdocNumberofMoveOrder', !on);
    }

    function clearRefDoc() {
        refDocRows = [];
        bindSelect('cmbRefDocNo', [], 'Id', 'GpNo', { blank: true });
    }

    /** GpNoBind / GetGatePassOutward / RefDocNoGatePassGeneral(Outward) / GatePassPartyProcessing
     *  — each enables cmbRefDocNo and binds its list, or empties it when there is nothing. */
    function loadRefDocs(mode, documentTypeId, singleColumn) {
        enable('cmbRefDocNo', true);
        return get('/ref-docs', { mode: mode, documentTypeId: documentTypeId }).then(function (d) {
            refDocRows = d.rows || [];
            $id('cmbRefDocNo').setAttribute('data-dtcombo', mode === 'outward' ? 'wbGatePass' : 'single');
            $id('cmbRefDocNo').setAttribute('data-dtcombo-caption',
                mode === 'outward' ? 'GatePass' : (mode === 'lab' || mode === 'engr' ? 'GpNo' : 'GatePassNo'));
            bindSelect('cmbRefDocNo', refDocRows, 'Id', 'GpNo', {
                blank: true,
                attrs: [['vehicle', 'VehicleNo'], ['gptype', 'GatepassType']]
            });
        }).catch(function (e) { box(e.message); });
    }

    function refDocRow() {
        var id = val('cmbRefDocNo');
        if (id === '') return null;
        for (var i = 0; i < refDocRows.length; i++) if (str(ci(refDocRows[i], 'Id')) === id) return refDocRows[i];
        return null;
    }
    function refDocGatepassType() { return str(ci(refDocRow(), 'GatepassType')); }

    function clearInvoices() {
        invoiceRows = [];
        bindSelect('CmbInvoiceNo', [], 'Id', 'InvoiceNo', { blank: true });
    }

    // ========================================================== gate pass chosen (Leave fill)

    /** DisableFileds():1499. */
    function DisableFileds() {
        enable('cmbVehicleType', false);
        enable('txtVehicleNo', false);
        enable('txtBilityNo', false);
        enable('CmbPartyName', false);
    }
    /** EnableTrueFileds():1514. */
    function EnableTrueFileds() {
        enable('cmbVehicleType', true);
        enable('txtVehicleNo', true);
        enable('txtBilityNo', true);
        enable('CmbItem', true);
        enable('txtItemqty', true);
        enable('CmbPartyName', true);
    }

    /** cmbRefDocNo_Leave():3946. */
    function cmbRefDocNo_Leave() {
        SupplierFirstWeight = 0;
        SupplierSecondWeight = 0;
        if (!saveActive()) return Promise.resolve();
        itemBind();
        var text = selectedText('cmbReferenceType');
        var value = intOf(val('cmbReferenceType'));
        var refDocId = intOf(val('cmbRefDocNo'));
        var chain = Promise.resolve();

        if (text === 'GatePass Inward') {
            if (value === 1601) {
                chain = chain.then(function () {
                    return get('/gate-pass', { kind: 'inward', documentTypeId: 1601, id: refDocId }).then(function (d) {
                        var r = d.row;
                        if (!r) return;
                        setVal('txtBilityNo', ci(r, 'BiltyNo'));
                        setVal('txtVehicleNo', ci(r, 'VehicleNo'));
                        setVal('cmbVehicleType', ci(r, 'VehicleType'));
                        setVal('txtItemqty', ci(r, 'ItemQty'));
                        setVal('CmbItem', ci(r, 'VarietyName'));
                        setVal('CmbWeighBridgeType', str(ci(r, 'GatepassType')));
                        setVal('txtRemarks', str(ci(r, 'OtherRemarks')));
                        DisableFileds();
                    });
                });
            } else {
                chain = chain.then(function () {
                    return get('/gate-pass', { kind: 'inward', documentTypeId: 51, id: refDocId }).then(function (d) {
                        var r = d.row;
                        if (!r) return;
                        RefDocumentTypeId = intOf(ci(r, 'RefDocumentTypeId'));
                        setVal('txtBilityNo', ci(r, 'BiltyNo'));
                        setVal('txtVehicleNo', ci(r, 'VehicleNo'));
                        setVal('cmbVehicleType', ci(r, 'VehicleType'));
                        setVal('CmbPartyName', ci(r, 'CompanyName'));
                        setVal('txtItemqty', ci(r, 'NoOfPackages'));
                        setVal('CmbItem', ci(r, 'VarietyName'));
                        setVal('txtRemarks', str(ci(r, 'OtherRemarks')));
                        setVal('CmbWeighBridgeType', str(ci(r, 'GatepassType')));
                        selectValue('cmbPackingType', intOf(ci(r, 'PackingTypeId')));
                        var po = intOf(ci(r, 'PurchaseOrderId')) > 0;
                        SupplierFirstWeight = (RefDocumentTypeId === 41 && po) ? num(ci(r, 'SupplierFirstWeight')) : 0;
                        SupplierSecondWeight = (RefDocumentTypeId === 41 && po) ? num(ci(r, 'SupplierSecondWeight')) : 0;
                        if (d.isGatePassEntryUser) {
                            setVal('txtSupplierFirstWeight', str(ci(r, 'SupplierFirstWeight')));
                            setVal('txtSupplierSecondWeight', str(ci(r, 'SupplierSecondWeight')));
                            setVal('txtSupplierNetWeight', str(ci(r, 'SupplierWeight')));
                        } else {
                            setVal('txtSupplierFirstWeight', '0');
                            setVal('txtSupplierSecondWeight', '0');
                            setVal('txtSupplierNetWeight', '0');
                        }
                        CalculateSupplierNetWeight();
                        DisableFileds();
                        if ((cfg.labCompulsoryBeforFirstWeight && (RefDocumentTypeId === 105 || RefDocumentTypeId === 106 || RefDocumentTypeId === 110))
                                || RefDocumentTypeId === 41) {
                            return labItemsBind(1);
                        }
                    });
                });
            }
        }

        if (text === 'GatePass OutWard') {
            chain = chain.then(function () {
                return get('/gate-pass', { kind: 'outward', id: refDocId }).then(function (d) {
                    var r = d.row;
                    if (!r) return;
                    DisableFileds();
                    setVal('txtBilityNo', ci(r, 'BiltyNo'));
                    setVal('txtVehicleNo', ci(r, 'VehicleNo'));
                    setVal('cmbVehicleType', ci(r, 'VehicleType'));
                    setVal('txtItemqty', ci(r, 'NoOfPackages'));
                    setVal('CmbWeighBridgeType', str(ci(r, 'GatepassType')));
                    setVal('txtRemarks', str(ci(r, 'OtherRemarks')));
                    var orderType = str(ci(r, 'OtherSupCust'));
                    var orderId = intOf(ci(r, 'SaleOrderId'));
                    return outwardItems(orderType, orderId, str(ci(r, 'VarietyName')), true).then(function () {
                        setVal('CmbPartyName', ci(r, 'CompanyName'));
                        enable('CmbPartyName', !!cfg.weighBridgeForOutwardPartyAndItemWise);
                        if (cfg.weighBridgeForOutwardPartyAndItemWise && orderType === 'DeliverOrder' && textValue('party', 'CmbPartyName') > 0) {
                            ItemsAgainstPartyForOutward(textValue('party', 'CmbPartyName'));
                        }
                    });
                });
            });
        }

        if (text === 'General GatePass Inward' || text === 'General GatePass Outward') {
            var docType = text === 'General GatePass Inward' ? 52 : 92;
            chain = chain.then(function () {
                return get('/gate-pass', { kind: 'general', documentTypeId: docType, gpSrNo: intOf(selectedText('cmbRefDocNo').trim()) }).then(function (d) {
                    var r = d.row;
                    if (!r) return;
                    setVal('txtBilityNo', ci(r, 'BiltyNo'));
                    setVal('txtVehicleNo', ci(r, 'VehicleNo'));
                    setVal('cmbVehicleType', ci(r, 'VehicleType'));
                    setVal('txtRemarks', ci(r, 'OtherRemarks'));
                    setVal('txtItemqty', ci(r, 'NoOfPackages'));
                    setVal('CmbItem', ci(r, 'VarietyName'));
                    setVal('CmbPartyName', ci(r, 'PartyName'));
                    DisableFileds();
                });
            });
        }

        if (value === 54 || value === 55) {
            chain = chain.then(function () {
                return get('/gate-pass', { kind: 'party', documentTypeId: value, gpSrNo: intOf(selectedText('cmbRefDocNo').trim()) }).then(function (d) {
                    var r = d.row;
                    if (!r) return;
                    setVal('txtBilityNo', ci(r, 'BiltyNo'));
                    setVal('txtVehicleNo', ci(r, 'VehicleNo'));
                    setVal('cmbVehicleType', ci(r, 'VehicleType'));
                    setVal('txtRemarks', ci(r, 'OtherRemarks'));
                    setVal('txtItemqty', ci(r, 'ItemQty'));
                    setVal('CmbItem', ci(r, 'VarietyName'));
                    setVal('CmbPartyName', ci(r, 'CompanyName'));
                    setVal('CmbWeighBridgeType', ci(r, 'GatepassType'));
                    DisableFileds();
                });
            });
        }

        return chain.then(function () {
            recalcPacking();
        }).catch(function (e) {
            /* :4169-4178 — a failure blanks the fields it was filling. */
            setVal('CmbPartyName', ''); setVal('CmbItem', ''); setVal('txtRemarks', '');
            setVal('txtBilityNo', ''); setVal('txtVehicleNo', ''); setVal('cmbVehicleType', '');
            setVal('txtItemqty', ''); setVal('CmbWeighBridgeType', '');
            box(e.message);
        });
    }

    /** The OtherSupCust switch shared by cmbRefDocNo_Leave (:4040) and ReadById (:2665). */
    function outwardItems(orderType, orderId, varietyName, fromLeave) {
        var gpId = intOf(val('cmbRefDocNo'));
        var gpType = refDocGatepassType();
        var active = hasActiveRow('cmbRefDocNo') && gpId > 0;
        switch (orderType) {
            case 'DeliverOrder': {
                var p = Promise.resolve();
                if (active && gpType === 'Export') p = p.then(function () { return getInvoiceNoFromDeliveryOrderByGpId(gpId); });
                if (cfg.weighBridgeForOutwardPartyAndItemWise && active && gpType !== 'Export') {
                    return p.then(function () {
                        return get('/delivery-order-parties-items', { gpId: gpId, recId: RECID }).then(function (rows) {
                            dtPartiesAndItemsFromDeliveryOrder = rows || [];
                            PartyNameBindforOutward();
                            bindText('item', 'CmbItem', 'dlItem', [], {});
                            setVal('CmbItem', '');
                            CmbPartyName_Leave();
                        });
                    });
                }
                return p.then(function () {
                    return ItemsFromDeliveryOrder(orderId).then(function () {
                        if (fromLeave) setVal('CmbItem', varietyName);
                    });
                });
            }
            case 'SaleOrder':
            case 'SaleOrderStoreAndPm':
                return ItemsFromSaleOrder(orderId);
            default:
                itemBind();
                partyBind(false);
                if (fromLeave) setVal('CmbItem', varietyName);
                return Promise.resolve();
        }
    }

    /** getInvoiceNoFromDeliveryOrderByGpId:5177 — row 0 is activated. */
    function getInvoiceNoFromDeliveryOrderByGpId(gpId) {
        enable('CmbInvoiceNo', true);
        var prev = intOf(val('CmbInvoiceNo'));
        return get('/delivery-order-invoices', { gpId: gpId }).then(function (rows) {
            invoiceRows = rows || [];
            if (invoiceRows.length) {
                bindSelect('CmbInvoiceNo', invoiceRows, 'Id', 'InvoiceNo', { blank: true });
                $id('CmbInvoiceNo').selectedIndex = 1;
                if (prev > 0) {
                    if (invoiceRows.some(function (r) { return intOf(ci(r, 'Id')) === prev; })) selectValue('CmbInvoiceNo', prev);
                    else selectValue('CmbInvoiceNo', '');
                }
            } else {
                clearInvoices();
            }
        }).catch(function (e) { box(e.message); });
    }

    /** ItemsFromDeliveryOrder:1259. */
    function ItemsFromDeliveryOrder(deliveryOrderId, gpId) {
        return get('/delivery-order-items', { deliveryOrderId: deliveryOrderId || 0, gpId: gpId || 0 }).then(function (rows) {
            rows = rows || [];
            if (rows.length) {
                itemsBindKeep(rows.map(function (r) { return { Id: ci(r, 'Id'), ItemName: ci(r, 'ItemName'), ItemCode: '', PartyId: 0 }; }), false);
            } else if (src.item.rows.length === 0) {
                setVal('CmbItem', '');
            }
        }).catch(function (e) { box(e.message); });
    }

    /** ItemsFromSaleOrder:1301. */
    function ItemsFromSaleOrder(saleOrderId) {
        return get('/sale-order-items', { saleOrderId: saleOrderId }).then(function (rows) {
            rows = rows || [];
            if (rows.length) {
                itemsBindKeep(rows.map(function (r) { return { Id: ci(r, 'Id'), ItemName: ci(r, 'ItemName'), ItemCode: '', PartyId: 0 }; }), false);
            }
        }).catch(function (e) { box(e.message); });
    }

    /** LabItemsBindByGpId + DDL.BindDDLNew(…, "ItemId", "ItemName", …) with row 0 activated. */
    function labItemsBind(actionId, fallback) {
        return get('/lab-items', { gpId: intOf(val('cmbRefDocNo')), actionId: actionId || 0 }).then(function (rows) {
            rows = rows || [];
            if (rows.length) {
                setVal('CmbItem', '');
                bindText('item', 'CmbItem', 'dlItem',
                    rows.map(function (r) { return { Id: ci(r, 'ItemId'), ItemName: ci(r, 'ItemName'), PartyId: 0 }; }), {});
                setVal('CmbItem', ci(src.item.rows[0], 'ItemName'));
            } else if (fallback) {
                fallback();
            }
        }).catch(function (e) { box(e.message); });
    }

    /** PartyNameBindforOutward():896 — distinct parties of the delivery order. */
    function PartyNameBindforOutward() {
        var rows = [];
        if (dtPartiesAndItemsFromDeliveryOrder.length) {
            var seen = {};
            dtPartiesAndItemsFromDeliveryOrder.forEach(function (r) {
                var id = intOf(ci(r, 'SupplierCustomerId'));
                if (!seen[id]) { rows.push({ Id: id, PartyName: ci(r, 'CustomerName') }); seen[id] = true; }
            });
            partyBind(true, rows);
        } else {
            partyBind(true, src.party.rows.filter(function (r) { return intOf(ci(r, 'Id')) !== 0; }));
        }
    }

    /** ItemsAgainstPartyForOutward(PartyId):1210. */
    function ItemsAgainstPartyForOutward(partyId) {
        var prev = textValue('item', 'CmbItem');
        var rows = [];
        if (dtPartiesAndItemsFromDeliveryOrder.length) {
            var seen = {};
            dtPartiesAndItemsFromDeliveryOrder.forEach(function (r) {
                var id = intOf(ci(r, 'ItemId'));
                var party = intOf(ci(r, 'SupplierCustomerId'));
                if (!seen[id] && party === partyId) {
                    rows.push({ Id: id, ItemName: ci(r, 'ItemName'), ItemCode: ci(r, 'ItemCode'), PartyId: party });
                    seen[id] = true;
                }
            });
        }
        if (rows.length) {
            bindText('item', 'CmbItem', 'dlItem', rows, { defaultRow: true });
            src.item.limit = true;
            if (prev > 0) {
                if (!setTextById('item', 'CmbItem', prev)) setVal('CmbItem', '');
            } else {
                setVal('CmbItem', '-- Select --');
            }
        } else {
            setVal('CmbItem', '');
            bindText('item', 'CmbItem', 'dlItem', [], {});
        }
    }

    /** CmbPartyName_Leave():5260. */
    function CmbPartyName_Leave() {
        enforceLimit('party', 'CmbPartyName');
        var id = textValue('party', 'CmbPartyName');
        if (cfg.weighBridgeForOutwardPartyAndItemWise && selectedText('cmbReferenceType') === 'GatePass OutWard' && id > 0) {
            ItemsAgainstPartyForOutward(id);
        } else {
            itemBind();
        }
    }

    // ============================================================================ calculations

    /** CalculateNetWeight():3742 — |first − second|, only once both boxes hold something. */
    function CalculateNetWeight() {
        var a = val('txtFirstWeight'), b = val('txtsecondWeight');
        if (a !== '' && b !== '') {
            var x = parseFloat(a), y = parseFloat(b);
            if (isNaN(x) || isNaN(y)) { box('Input string was not in a correct format.'); return; }
            if (x > y) setVal('txtNetWeight', dstr(x - y));
            if (y > x) setVal('txtNetWeight', dstr(y - x));
            if (x === y) setVal('txtNetWeight', '0');
        }
        CalculateDifferenceWeights();
    }

    /** CalculateDifferenceWeights():3793. */
    function CalculateDifferenceWeights() {
        setVal('txtFirstWtDifference', dstr(Math.abs(num(val('txtFirstWeight')) - num(val('txtSupplierFirstWeight')))));
        setVal('txtSecondWtDifference', dstr(Math.abs(num(val('txtsecondWeight')) - num(val('txtSupplierSecondWeight')))));
        setVal('txtNetWtDifference', dstr(Math.abs(num(val('txtNetWeight')) - num(val('txtSupplierNetWeight')))));
    }

    /** PackingWeightCalculateAndNetWeight():5307 — Math.Round(qty × ebUnit, 0, AwayFromZero). */
    function recalcPacking() {
        var qty = num(val('txtItemqty')), ebunit = num(val('txtEbUnit')), net = num(val('txtNetWeight'));
        var t = qty * ebunit;
        var total = (t < 0 ? -1 : 1) * Math.round(Math.abs(t));
        setVal('txtEbTotalWeight', dstr(total));
        setVal('txtWeightAfterPakingWeight', dstr(net - num(val('txtEbTotalWeight'))));
    }

    /** CalculateSupplierNetWeight():5350. */
    function CalculateSupplierNetWeight() {
        var a = val('txtSupplierFirstWeight'), b = val('txtSupplierSecondWeight');
        if (a !== '' && b !== '') {
            var x = parseFloat(a), y = parseFloat(b);
            if (isNaN(x) || isNaN(y)) { box('Input string was not in a correct format.'); return; }
            setVal('txtSupplierNetWeight', dstr(Math.abs(x - y)));
        } else {
            setVal('txtSupplierNetWeight', '0');
        }
        CalculateDifferenceWeights();
    }

    // ================================================================= indicator (F5 / serial)

    /**
     * ReadWeightFromWeightBridge():4181 — done by the server, which runs on the PC the indicator
     * is wired to: it opens the scale's COM port, waits 500 ms, reads, parses by ModelName and,
     * for a reading above 0, takes the three camera pictures into the CameraPictureBox folder.
     */
    function readWeight() {
        var btn = $id('btnReadIndicator');
        btn.classList.add('is-busy');
        post('/device/read-weight', {
            ticketNo: val('txtTickectNo'),
            updateMode: UpdateMode,
            approved: Approved,
            firstTags: pics.first,
            secondTags: pics.second
        }).then(function (d) {
            $id('labelSerialPortData').textContent = str(d.machineData);
            applyReading(str(d.reading));
            var names = d.pictures || {};
            if (d.target === 'first' && names.FirstWtPicReading !== undefined) {
                pics.first = [names.FirstWtPicReading, names.FirstWtUpperPic, names.FirstWtFrontPic];
                markCaptured(FIRST_BOXES, pics.first);
            }
            if (d.target === 'second' && names.SecondPicReading !== undefined) {
                pics.second = [names.SecondPicReading, names.SecondWtUpperPic, names.SecondWtFrontPic];
                markCaptured(SECOND_BOXES, pics.second);
            }
            (d.warnings || []).forEach(function (w) { box(w); });   // CreateJPG's message boxes
        }).catch(function (e) {
            box(e && e.message ? e.message : String(e));
        }).then(function () { btn.classList.remove('is-busy'); });
    }

    /** :4284-4337 — the reading goes to the second weight on an open, unapproved ticket, and to
     *  the first weight on a new one. */
    function applyReading(b) {
        setVal('txtWeightReading', b + '  Kg');
        if (UpdateMode && !Approved) {
            setVal('txtsecondWeight', b);
            onSecondWeightChanged();
        }
        if (!UpdateMode) {
            setVal('txtFirstWeight', b);
            CalculateNetWeight();
        }
    }

    function onSecondWeightChanged() { CalculateNetWeight(); recalcPacking(); }

    // =================================================================================== refresh

    /** formRefresh():1386. */
    function formRefresh() {
        RECID = 0;
        SupplierFirstWeight = 0;
        SupplierSecondWeight = 0;
        Approved = false;
        loadedRow = null;
        weightsEditable(false);
        show('lblEnableWeights', false);
        setVal('txtWeightReading', '0');
        setVal('txtsecondWeight', '');
        setVal('txtTickectNo', '');
        setVal('txtBilityNo', '');
        setVal('txtWorkingReportNo', '');
        setVal('txtWeightDiffComments', '');
        setVal('CmbWeighBridgeType', '');
        enable('txtWorkingReportNo', false);
        setVal('txtFirstWeight', '');
        setVal('CmbItem', '');
        setVal('txtItemqty', '');
        setVal('txtRemarks', '');
        setVal('txtVehicleNo', '');
        setVal('txtWbCharges', '');
        setVal('CmbPartyName', '');
        setVal('txtContainerNo', '');
        resetPictures();
        initCameras(false, FIRST_BOXES);            // :1485 CameraDisconnect + LoadCamera()
        setVal('txtNetWeight', '');
        setVal('txtrefdocNumberofMoveOrder', '');
        setVal('txtEbUnit', '');
        setVal('txtEbTotalWeight', '');
        setVal('txtWeightAfterPakingWeight', '');
        clearInvoices();
        clearRefDoc();
        refDocVisible(true);
        EnableTrueFileds();
        show('lblMultiItem', false);
        $id('ChkMultiItem').checked = false;
        stampTimes();
        show('btnsave', true);
        show('btnUpdate', false);
        selectValue('cmbReferenceType', '');
        setVal('txtSupplierFirstWeight', '');
        setVal('txtSupplierSecondWeight', '');
        setVal('txtSupplierNetWeight', '');
        UpdateMode = false;
        enable('cmbReferenceType', true);
        src.party.limit = false; src.item.limit = false;
        CalculateDifferenceWeights();
        return get('/refresh-lists').then(function (d) {
            L.wbParties = d.wbParties;
            L.wbItemLookups = d.wbItemLookups;
            itemBind();
            partyBind(false);
            show('GroupInwardLabInfo', !!cfg.showInwardLabInfo);
        }).then(function () {
            return get('/ticket-no').then(function (d) { setVal('txtTickectNo', d.ticketNo); });
        }).catch(function (e) { box(e.message); }).then(function () {
            focusEl('cmbReferenceType');
        });
    }

    function weightsEditable(on) {
        ['txtFirstWeight', 'txtsecondWeight'].forEach(function (id) {
            var e = $id(id); e.disabled = !on; e.readOnly = !on;
        });
    }

    var FIRST_BOXES = ['PicFirstReading1stTime', 'picFirstUpper', 'PicVehicleFront1stTime'];
    var SECOND_BOXES = ['PicSecondReading2ndtime', 'PicSecondUpper', 'PicSecondVehicle2ndTime'];
    var FIRST_FIELDS = ['FirstWtPicReading', 'FirstWtUpperPic', 'FirstWtFrontPic'];
    var SECOND_FIELDS = ['SecondPicReading', 'SecondWtUpperPic', 'SecondWtFrontPic'];

    function camerasOn() { return !!(L && L.cameraPathConfigured); }

    function boxPlaceholder(id, i) {
        $id(id).removeAttribute('data-captured');
        $id(id).innerHTML = '';
        $id(id).textContent = 'Camera 0' + (i + 1);
        $id(id).title = '';
    }

    function boxImage(id, url, title) {
        var e = $id(id);
        var img = e.querySelector('img');
        if (!img) { e.textContent = ''; img = document.createElement('img'); img.style.width = '100%'; img.style.height = '100%'; img.style.objectFit = 'cover'; e.appendChild(img); }
        img.onerror = function () { e.innerHTML = ''; e.textContent = title || 'No picture'; };
        img.src = url;
        e.title = title || '';
    }

    /** HVUtility.Preview — the page shows a frame from the server every second. */
    function startLive(ids) {
        stopLive();
        if (!camerasOn()) return;
        liveBoxes = ids.slice();
        var tick = function () {
            liveBoxes.forEach(function (id, i) {
                var taken = $id(id).getAttribute('data-captured');
                boxImage(id, api + '/device/camera/' + (i + 1) + '/snapshot?t=' + Date.now(),
                         'Camera 0' + (i + 1) + ' (live)' + (taken ? ' - captured: ' + taken : ''));
            });
        };
        tick();
        liveTimer = setInterval(tick, 1000);
    }

    function stopLive() {
        if (liveTimer) clearInterval(liveTimer);
        liveTimer = null;
        liveBoxes = [];
    }

    /** WeightBridge_Helper.CameraInitilization — the desktop's message per camera that fails. */
    function initCameras(second, boxes) {
        if (!camerasOn()) return Promise.resolve();
        return get('/device/cameras', { second: second }).then(function (d) {
            (d.cameras || []).forEach(function (c) { if (!c.ok && c.message) box(c.message); });
            startLive(boxes);
        }).catch(function (e) { box(e.message); });
    }

    /** The captured names go on the boxes' titles (the desktop keeps them in PictureBox.Tag). */
    function markCaptured(ids, names) {
        ids.forEach(function (id, i) {
            if (names[i]) { $id(id).setAttribute('data-captured', names[i]); $id(id).title = 'Captured: ' + names[i]; }
        });
    }

    /** Opening a ticket: the stored pictures from the camera folder (File.Exists, else empty). */
    function showStoredPictures(r) {
        stopLive();
        FIRST_BOXES.concat(SECOND_BOXES).forEach(function (id, i) {
            var field = (FIRST_FIELDS.concat(SECOND_FIELDS))[i];
            var name = str(ci(r, field));
            if (name && camerasOn()) boxImage(id, api + '/device/ticket/' + RECID + '/picture/' + field + '?t=' + Date.now(), name);
            else boxPlaceholder(id, i % 3);
        });
    }

    function resetPictures() {
        pics = { first: ['', '', ''], second: ['', '', ''] };
        stopLive();
        FIRST_BOXES.forEach(boxPlaceholder);
        SECOND_BOXES.forEach(boxPlaceholder);
    }

    /** btnRefresh_Click:4629. */
    function refresh() {
        get('/refresh-lists').then(function (d) {
            bindText('vehicleType', 'cmbVehicleType', 'dlVehicleType', d.vehicleTypes, { defaultRow: true });
            packingTypeBind(d.packingTypes);
            cfg.weighBridgeForOutwardPartyAndItemWise = !!d.weighBridgeForOutwardPartyAndItemWise;
            if (cfg.weighBridgeForOutwardPartyAndItemWise && selectedText('cmbReferenceType') === 'GatePass OutWard') {
                PartyNameBindforOutward();
                var id = textValue('party', 'CmbPartyName');
                if (id > 0) ItemsAgainstPartyForOutward(id);
            } else {
                L.items = d.items;
                L.wbParties = d.wbParties;
                L.wbItemLookups = d.wbItemLookups;
                partyBind(false);
                itemBind();
            }
        }).catch(function (e) { box(e.message); });
    }

    // =================================================================================== payload

    function payload() {
        return {
            id: RECID,
            ticketNo: intOf(val('txtTickectNo')),
            biltyNo: val('txtBilityNo'),
            supplierCustomerId: textValue('party', 'CmbPartyName'),
            partyName: val('CmbPartyName'),
            partySelected: !!textRow('party', 'CmbPartyName'),
            invoiceId: intOf(val('CmbInvoiceNo')),
            invoiceSelected: hasActiveRow('CmbInvoiceNo'),
            wbCharges: num(val('txtWbCharges')),
            vehicleNo: val('txtVehicleNo'),
            savedVehicleNo: val('cmbVehicleNo'),
            vehicleType: val('cmbVehicleType'),
            weighBridgeType: val('CmbWeighBridgeType'),
            weighBridgeTypeSelected: !!textRow('wbType', 'CmbWeighBridgeType'),
            packingType: selectedText('cmbPackingType'),
            packingTypeId: intOf(val('cmbPackingType')),
            packingTypeSelected: hasActiveRow('cmbPackingType'),
            itemQty: num(val('txtItemqty')),
            itemId: textValue('item', 'CmbItem'),
            itemDescription: val('CmbItem'),
            itemSelected: !!textRow('item', 'CmbItem'),
            itemPartyId: intOf(ci(textRow('item', 'CmbItem'), 'PartyId')),
            remarks: val('txtRemarks'),
            firstWeight: num(val('txtFirstWeight')),
            secondWeight: num(val('txtsecondWeight')),
            netWeight: num(val('txtNetWeight')),
            firstDateTime: isoLocal(firstDate),
            secondDateTime: isoLocal(secondDate),
            packingUnitWeight: num(val('txtEbUnit')),
            packingTotalWeight: num(val('txtEbTotalWeight')),
            weightAfterPackingWeight: num(val('txtWeightAfterPakingWeight')),
            refTypeId: intOf(val('cmbReferenceType')),
            refTypeText: selectedText('cmbReferenceType'),
            refTypeSelected: hasActiveRow('cmbReferenceType'),
            refDocId: intOf(val('cmbRefDocNo')),
            refDocSelected: hasActiveRow('cmbRefDocNo'),
            refDocGatepassType: refDocGatepassType(),
            moveOrderRefNo: val('txtrefdocNumberofMoveOrder'),
            containerNo: val('txtContainerNo'),
            workingReportNo: val('txtWorkingReportNo'),
            weightDiffComments: val('txtWeightDiffComments'),
            multiItem: $id('ChkMultiItem').checked,
            supplierFirstWeight: num(val('txtSupplierFirstWeight')),
            supplierSecondWeight: num(val('txtSupplierSecondWeight')),
            supplierNetWeight: num(val('txtSupplierNetWeight')),
            firstWtPicReading: pics.first[0], firstWtUpperPic: pics.first[1], firstWtFrontPic: pics.first[2],
            secondPicReading: pics.second[0], secondWtUpperPic: pics.second[1], secondWtFrontPic: pics.second[2]
        };
    }

    /** FormValidation():1531 — the page's copy, so the offending control can take focus. The
     *  server runs the same checks again. Returns true to continue. */
    function FormValidation(p, update) {
        function no(msg, focusId) { box(msg); focusEl(focusId); return false; }
        var text = p.refTypeText;
        if (!p.refTypeSelected) return no('ReferenceType Field is Required', 'cmbReferenceType');
        if (p.refTypeId === 51) {
            if (p.supplierFirstWeight === 0) return no('Supplier First Weight Field is Required', 'txtSupplierFirstWeight');
            if (update) {
                if (p.supplierSecondWeight === 0) return no('Supplier Second Weight Field is Required', 'txtSupplierSecondWeight');
                if (p.supplierNetWeight === 0) return no('Supplier Net Weight Field is Required', 'txtSupplierNetWeight');
            }
        }
        var mog = text === 'Move Order' || text === 'General';
        var m = str(p.moveOrderRefNo).trim();
        if (mog && (m === '' || m === '0')) return no('GatePass No Field is Required', 'cmbRefDocNo');
        if (!mog && !p.refDocSelected) return no('GatePass No Field is Required', 'cmbRefDocNo');
        if (!p.weighBridgeTypeSelected) return no('WeighBridge Type Field is Required', 'CmbWeighBridgeType');
        if (p.refTypeId === 91 && p.refDocSelected && p.refDocId > 0 && p.refDocGatepassType === 'Export'
                && (!p.invoiceSelected || p.invoiceId === 0)) return no('Invoice number Field is Required', 'CmbInvoiceNo');
        if ((p.refTypeId === 74 || p.refTypeId === 75) && (!p.packingTypeSelected || p.packingTypeId === 0)) {
            return no('PackingType Field is Required', 'cmbPackingType');
        }
        if (update) {
            if (p.secondWeight === 0) return no('Second Weight  Field is Required', 'txtsecondWeight');
            var w = str(p.workingReportNo).trim();
            if ((text === 'Move Order' || text === 'General GatePass Inward' || text === 'General GatePass Outward') && (w === '' || w === '0')) {
                enable('txtWorkingReportNo', true);
                return no('WorkingReportNo Field is Required', 'txtWorkingReportNo');
            }
            if (text === 'GatePass OutWard' && (w === '' || w === '0')) {
                /* The StockTransfer test needs the delivery order type — the server makes it. */
                enable('txtWorkingReportNo', true);
            }
        }
        if (!update && p.firstWeight === 0) return no('First Weight  Field is Required', 'txtFirstWeight');
        if (p.refTypeId === 74 && cfg.lstIsSavedVehicle) {
            if (str(p.savedVehicleNo).trim() === '') return no('Vehicle Number Field is Required', 'txtVehicleNo');
        } else if (str(p.vehicleNo).trim() === '') {
            return no('Vehicle Number Field is Required', 'txtVehicleNo');
        }
        if (p.ticketNo === 0) return no('Ticket No Field is Required', 'txtTickectNo');
        if (cfg.weighBridgeForOutwardPartyAndItemWise && text === 'GatePass OutWard' && p.refDocGatepassType !== 'Export') {
            if (!p.partySelected || p.supplierCustomerId === 0) return no('PartyName Field is Required', 'CmbPartyName');
            if (!p.itemSelected || p.itemId === 0) return no('ItemName Field is Required', 'CmbItem');
            if (p.supplierCustomerId !== p.itemPartyId) { setVal('CmbPartyName', ''); return no('PartyName Field is Required', 'CmbPartyName'); }
        }
        if (str(p.remarks).trim() === '') return no('Remarks Field is Required', 'txtRemarks');
        return true;
    }

    // ================================================================================ save/update

    /** btnsave_Click:2347. */
    function save() {
        if (!saveActive()) return;
        enable('txtsecondWeight', false);
        var p = payload();
        if (!FormValidation(p, false)) return;
        if (p.refTypeText === 'GatePass Inward') {
            var load = p.supplierFirstWeight, tare = p.supplierSecondWeight;
            if (SupplierFirstWeight > 0 && SupplierFirstWeight !== load
                    && !ask('Supplier First Weight in Gate Pass does not match the Weighbridge First Weight.\n'
                          + 'Gate Pass Weight: ' + dstr(SupplierFirstWeight) + '\nWeighbridge Weight: ' + dstr(load)
                          + '\n\nDo you want to continue?')) return;
            if (SupplierSecondWeight > 0 && tare > 0 && SupplierSecondWeight !== tare
                    && !ask('Supplier Second Weight in Gate Pass does not match the Weighbridge Second Weight.\n'
                          + 'Gate Pass Weight: ' + dstr(SupplierSecondWeight) + '\nWeighbridge Weight: ' + dstr(tare)
                          + '\n\nDo you want to continue?')) return;
        }
        if (!ask('Are you sure to Save?')) return;
        var gpId = p.refDocId;
        busy('btnsave', true);
        post('/save', p).then(function (res) {
            busy('btnsave', false);
            if (!res || !res.success) return;
            box(res.message);
            return afterWrite(res.id, gpId);
        }).catch(function (e) { busy('btnsave', false); box(e.message); });
    }

    /** btnUpdate_Click:2951. */
    function update() {
        if (!updateActive()) return;
        if (Approved) { box('Record cannot be Updated because Record has Approved'); return; }
        var p = payload();
        if (!FormValidation(p, true)) return;
        if (p.refTypeText === 'GatePass Inward') {
            var tol = num(cfg.weighbridgeTolerance);
            if (Math.abs(p.netWeight - p.supplierNetWeight) > tol && str(p.weightDiffComments).trim() === '') {
                box('Supplier Net Weight in Weigh Bridge does not match the Weighbridge Net Weight.\n'
                  + 'Supplier Net Weight: ' + dstr(p.supplierNetWeight) + '\nWeighbridge Net Weight: ' + dstr(p.netWeight)
                  + '\n\nWeighbridge Tolerance is : ' + dstr(tol) + '\n\nDifference Weight Remarks are required');
                return;
            }
        }
        if (!ask('Are you sure to Update?')) return;
        var gpId = p.refDocId;
        busy('btnUpdate', true);
        post('/update', p).then(function (res) {
            busy('btnUpdate', false);
            if (res && res.success) box(res.message);
            return afterWrite(res ? res.id : 0, gpId);
        }).catch(function (e) { busy('btnUpdate', false); box(e.message); });
    }

    /** The shared tail of both buttons: refresh, then the three optional previews. */
    function afterWrite(id, gpId) {
        return formRefresh().then(function () {
            if ($id('chkPrintWithLab').checked) slip257For(gpId);
            if ($id('ChkBox').checked) printSlip(id, false);
            if ($id('ChkBoxPrintImages').checked) printSlip(id, true);
        });
    }

    // ================================================================================ read by id

    /** ReadById(ID):2587. */
    function readById(id) {
        return get('/' + id).then(function (d) {
            var r = d.row;
            loadedRow = r;
            show('lblMultiItem', true);
            show('btnsave', false);
            show('btnUpdate', true);
            enable('cmbReferenceType', false);
            RECID = id;
            tab('tabForm');
            setVal('txtTickectNo', ci(r, 'TicketNo'));
            setVal('txtFirstWeight', str(ci(r, 'FirstWeight')));
            var fd = parseDate(ci(r, 'FirstDateTime'));
            if (fd) { firstDate = fd; setVal('txtFirstDate', timeOf(fd)); }
            setVal('txtsecondWeight', str(ci(r, 'SecondWeight')));
            setVal('txtNetWeight', str(ci(r, 'NetWbWeight')));
            setVal('txtWorkingReportNo', str(ci(r, 'WorkingReportNo')));
            RefDocumentTypeId = intOf(ci(r, 'RefDocumentTypeId'));
            if (val('txtNetWeight') === '') CalculateNetWeight();
            if (val('txtFirstWeight') !== '' && intOf(val('txtsecondWeight')) > 0) {
                enable('txtsecondWeight', false);
                enable('txtFirstWeight', false);
                enable('txtBilityNo', false);
                enable('txtVehicleNo', false);
                enable('cmbRefDocNo', false);
            }
            setVal('txtBilityNo', ci(r, 'BiltyNo'));
            setVal('CmbPartyName', ci(r, 'PartyName'));
            setVal('txtVehicleNo', ci(r, 'VehicleNo'));
            setVal('cmbVehicleType', str(ci(r, 'VehicleType')));
            setVal('CmbWeighBridgeType', str(ci(r, 'WeighBridgeType')));
            setSelectTextKeep('cmbPackingType', str(ci(r, 'PackingType')));
            setVal('txtEbUnit', str(ci(r, 'PackingUnitWeight')));
            setVal('txtEbTotalWeight', str(ci(r, 'PackingTotalWeight')));
            setVal('txtWeightAfterPakingWeight', str(ci(r, 'WeightAfterPackingWeight')));
            setVal('txtWbCharges', ci(r, 'WbCharges'));
            setVal('txtItemqty', ci(r, 'ItemQty'));
            setVal('txtRemarks', ci(r, 'WbRemarks'));
            refDocVisible(true);

            var reftypeid = intOf(ci(r, 'ReferenceDocTypeId'));
            var oneRow = function (withType) {
                var o = { Id: intOf(ci(r, 'ReferenceDocNoId')), GpNo: intOf(ci(r, 'GpSrNo')), VehicleNo: intOf(ci(r, 'VehicleNo')) };
                if (withType) o.GatepassType = str(ci(r, 'GatepassType'));
                refDocRows = [o];
                $id('cmbRefDocNo').setAttribute('data-dtcombo', withType ? 'wbGatePass' : 'single');
                bindSelect('cmbRefDocNo', refDocRows, 'Id', 'GpNo', { blank: true, attrs: [['vehicle', 'VehicleNo'], ['gptype', 'GatepassType']] });
            };
            var chain = Promise.resolve();
            if (reftypeid === 51 || reftypeid === 1601) {
                oneRow(false);
                setSelectByText('cmbReferenceType', 'GatePass Inward');
            }
            enable('cmbRefDocNo', false);
            if (reftypeid === 91) {
                setSelectByText('cmbReferenceType', 'GatePass OutWard');
                oneRow(true);
                setVal('txtContainerNo', ci(r, 'ContainerNo'));
                setVal('txtWeightDiffComments', str(ci(r, 'WeightDiffComments')));
                $id('cmbRefDocNo').selectedIndex = 1;       // Rows[0].Activate()
                DisableFileds();
                var orderType = str(ci(r, 'OtherSupCust'));
                var orderId = intOf(ci(r, 'OrderId'));
                chain = chain.then(function () { return outwardItems(orderType, orderId, '', false); }).then(function () {
                    setVal('CmbPartyName', ci(r, 'PartyName'));
                    enable('CmbPartyName', !!cfg.weighBridgeForOutwardPartyAndItemWise);
                    if (cfg.weighBridgeForOutwardPartyAndItemWise && orderType === 'DeliverOrder' && textValue('party', 'CmbPartyName') > 0) {
                        ItemsAgainstPartyForOutward(textValue('party', 'CmbPartyName'));
                    }
                });
            }
            if (reftypeid === 52 || reftypeid === 92) {
                setSelectByText('cmbReferenceType', reftypeid === 52 ? 'General GatePass Inward' : 'General GatePass Outward');
                oneRow(false);
                setVal('txtContainerNo', ci(r, 'ContainerNo'));
            }
            if (reftypeid === 74) {
                refDocVisible(false);
                setSelectByText('cmbReferenceType', 'Move Order');
                setVal('txtrefdocNumberofMoveOrder', ci(r, 'ReferenceDocNoId'));
            }
            if (reftypeid === 75) {
                itemLookupsBind();
                partyBind(false);
                refDocVisible(false);
                setSelectByText('cmbReferenceType', 'General');
                setVal('txtrefdocNumberofMoveOrder', ci(r, 'ReferenceDocNoId'));
                /* PartyBind() after CmbPartyName.Text was set: a party name that is not one of
                   the weighbridge parties has no Value, Conversion.ToInt gives 0, and 0 is the
                   "-- Select --" row, which is what the desktop then shows. Reproduced. */
            }
            if (reftypeid === 54 || reftypeid === 55) {
                selectValue('cmbReferenceType', ci(r, 'ReferenceDocTypeId'));
                oneRow(false);
            }
            /* :2737 */
            Approved = intOf(val('txtsecondWeight')) > 0 ? true : !!d.approved;
            selectValue('cmbRefDocNo', ci(r, 'ReferenceDocNoId'));

            return chain.then(function () {
                var itemId = intOf(ci(r, 'ItemId'));
                var setItem = function () {
                    if (itemId > 0) { if (!setTextById('item', 'CmbItem', itemId)) setVal('CmbItem', ''); }
                    else setVal('CmbItem', ci(r, 'ItemDescription'));
                };
                if (reftypeid === 51 && (RefDocumentTypeId === 105 || RefDocumentTypeId === 106 || RefDocumentTypeId === 110 || RefDocumentTypeId === 41)) {
                    setVal('CmbItem', '');
                    return labItemsBind(0, setItem);
                }
                setItem();
            }).then(function () {
                setVal('txtSupplierFirstWeight', dstr(num(ci(r, 'LoadWeight'))));
                setVal('txtSupplierSecondWeight', dstr(num(ci(r, 'TearWeight'))));
                setVal('txtSupplierNetWeight', dstr(num(ci(r, 'SupplierWbNetWeight'))));
                CalculateDifferenceWeights();
                UpdateMode = true;
                enable('txtFirstWeight', false);
                enable('txtsecondWeight', false);
                pics.first = FIRST_FIELDS.map(function (f) { return str(ci(r, f)); });    // UniqFileName..3
                pics.second = SECOND_FIELDS.map(function (f) { return str(ci(r, f)); });  // UniqFileName4..6
                showStoredPictures(r);
                if (!Approved) initCameras(true, SECOND_BOXES);                         // :2331 Camera 04..06
                focusEl('txtsecondWeight');
            });
        }).catch(function (e) { box(e.message); });
    }

    // =============================================================================== pending grids

    function pickDate(chkId, id) { return $id(chkId).checked ? val(id) : ''; }

    /** bindGridPendingForSecond():1975 */
    function showPending2nd() {
        return get('/pending-second', {
            fromDate: pickDate('chkFromGpDatePendingFor2nd', 'FromGpDatePendingFor2nd'),
            toDate: pickDate('chkToGpDatePendingFor2nd', 'ToGpDatePendingFor2nd'),
            gpNoFrom: intOf(val('txtFromGpNoPendingFor2nd')),
            gpNoTo: intOf(val('txtToGpNoPendingFor2nd'))
        }).then(function (rows) {
            pendingSecondRows = rows || [];
            renderGrid('grdPendingForSecond', pendingSecondRows, PENDING2_COLS, {
                buttons: [{ key: 'Edit', text: 'Edit' }],
                onButton: function (key, row) { if (key === 'Edit') readById(intOf(ci(row, 'Id'))); },
                onDblClick: function (row) { readById(intOf(ci(row, 'Id'))); },
                rowStyle: labStatusStyle
            });
        }).catch(function (e) { box(e.message); });
    }
    function resetPending2nd() {
        setVal('FromGpDatePendingFor2nd', isoDay(daysAgo(3)));
        setVal('ToGpDatePendingFor2nd', isoDay(new Date()));
        setVal('txtToGpNoPendingFor2nd', '');
        setVal('txtFromGpNoPendingFor2nd', '');
        $id('grdPendingForSecond').innerHTML = '';
    }

    /** BindPendingFor1st():1729 */
    function showPending1st() {
        return get('/pending-first', {
            fromDate: pickDate('chkFromDatePendingFor1st', 'FromDatePendingFor1st'),
            toDate: pickDate('chkToDatePendingFor1st', 'ToDatePendingFor1st'),
            gpNoFrom: intOf(val('txtFromGpNoPendingFor1st')),
            gpNoTo: intOf(val('txtToGpNoPendingFor1st'))
        }).then(function (rows) {
            pendingFirstRows = rows || [];
            renderGrid('grdPendingFirst', pendingFirstRows, PENDING1_COLS, { rowStyle: pending1Style });
            InwardLabStatusCountCalculate();
        }).catch(function (e) { box(e.message); });
    }
    function resetPending1st() {
        setVal('FromDatePendingFor1st', isoDay(daysAgo(3)));
        setVal('ToDatePendingFor1st', isoDay(new Date()));
        setVal('txtFromGpNoPendingFor1st', '');
        setVal('txtToGpNoPendingFor1st', '');
        $id('grdPendingFirst').innerHTML = '';
    }

    /** InwardLabStatusCountCalculate():1918 — gate passes of DocumentTypeId 51 only. */
    function InwardLabStatusCountCalculate() {
        if (!pendingFirstRows.length) return;
        var inward = pendingFirstRows.filter(function (r) { return intOf(ci(r, 'DocumentTypeId')) === 51; });
        $id('labInwardfirstWtTotalCount').textContent = String(inward.length);
        $id('labInwardfirstWtCompleteCount').textContent = String(inward.filter(function (r) { return str(ci(r, 'LabStatus')) === 'Accepted'; }).length);
        $id('labInwardfirstWtPendingCount').textContent = String(inward.filter(function (r) { return str(ci(r, 'LabStatus')) === 'Pending'; }).length);
    }

    /* grdfill_FormattingRow:2041 */
    function labStatusStyle(col, v) {
        if (col === 'LabStatus') return v === 'Pending' ? 'c-red' : (v === 'Accepted' ? 'c-green' : '');
        if (col === 'LabApprovalStatus') return v === 'Not Approved' ? 'c-red' : (v === 'Approved' ? 'c-green' : '');
        return '';
    }
    /* grdPendingGpForWeighBridge_FormattingRow:1827 */
    function pending1Style(col, v) {
        if (col === 'LabStatus' || col === 'LabApprovalStatus') return labStatusStyle(col, v);
        if (col === 'AccessWeight') return num(v) === 0 ? 'c-green' : (num(v) > 0 ? 'c-red' : '');
        if (col === 'GpApprovalStatus') return v === 'Not Approved' ? 'c-red' : (v === 'Approved' ? 'c-green' : '');
        return '';
    }

    /* Column sets — the DataTables the desktop builds, in its order, with its source columns.
       {k: grid key, s: source column, t: type, sum, hidden, cap} */
    var PENDING2_COLS = [
        { k: 'Id', hidden: true }, { k: 'DocTypeId', hidden: true }, { k: 'ReferenceDocTypeId', hidden: true },
        { k: 'LabStatus' }, { k: 'LabApprovalStatus' },
        { k: 'DocDate', t: 'date' }, { k: 'DocumentType', s: 'DocumentTypeDescription' },
        { k: 'TicketNo', t: 'int' }, { k: 'GpSrNo', t: 'int' }, { k: 'VehicleNo' }, { k: 'BiltyNo' },
        { k: 'PartyName' }, { k: 'ItemName', s: 'ItemDescription' },
        { k: 'ItemQty', t: 'num', sum: true }, { k: 'WbCharges', t: 'num' }, { k: 'WbRemarks' },
        { k: 'FirstWeight', t: 'num' }, { k: 'FirstWtWbStatus' }, { k: 'SecondWeight', t: 'num' },
        { k: 'EntryUser', s: 'UserName' }
    ];
    var PENDING1_COLS = [
        { k: 'Id', hidden: true }, { k: 'DocumentTypeId', hidden: true },
        { k: 'LabStatus' }, { k: 'LabApprovalStatus' }, { k: 'DocumentType' },
        { k: 'GpDate', t: 'date' }, { k: 'GpSrNo', t: 'int', cap: 'Gp No' },
        { k: 'PartyName' }, { k: 'ItemName' }, { k: 'ItemQty', t: 'num', sum: true },
        { k: 'VehicleType' }, { k: 'VehicleNo' }, { k: 'BiltyNo' }, { k: 'Remarks' }, { k: 'GpStatus' },
        { k: 'EntryUser', s: 'UserName', hidden: true }, { k: 'GpApprovalStatus' },
        { k: 'AccessWeight', t: 'num', sum: true }
    ];
    /* BindHistoryGrid():3537 + HistoryGridSettings — doubles are summed ("#,##0.##") except EbUnit. */
    var HISTORY_COLS = [
        { k: 'RecordNo', t: 'int' }, { k: 'Id', hidden: true }, { k: 'DocDate', t: 'date' },
        { k: 'DocumentType', s: 'DocumentTypeDescription' }, { k: 'WeighBridgeType' },
        { k: 'ReferenceDocNoId', hidden: true }, { k: 'GpSrNo', t: 'int' }, { k: 'TicketNo', t: 'int' },
        { k: 'PartyName' }, { k: 'VehicleNo' }, { k: 'BiltyNo' }, { k: 'ItemDescription' },
        { k: 'ReferenceDocTypeId', hidden: true },
        { k: 'ItemQty', t: 'num', sum: true, d: 2 }, { k: 'WbCharges', t: 'num', sum: true, d: 2 },
        { k: 'WorkingReportNo' },
        { k: 'FirstWeight', t: 'num', sum: true, d: 2 }, { k: 'FirstWtWbStatus' },
        { k: 'SecondWeight', t: 'num', sum: true, d: 2 }, { k: 'SecondWtWbStatus' },
        { k: 'NetWbWeight', t: 'num', sum: true, d: 2 },
        { k: 'EbUnit', s: 'PackingUnitWeight', t: 'num', d: 2 },
        { k: 'EbTotal', s: 'PackingTotalWeight', t: 'num', sum: true, d: 2 },
        { k: 'WeightAfterPackingWeight', t: 'num', sum: true, d: 2 },
        { k: 'WbRemarks' }, { k: 'EntryDate', t: 'date' }, { k: 'EntryUser', s: 'UserName' }
    ];

    function renderGrid(hostId, rows, cols, opts) {
        opts = opts || {};
        var host = $id(hostId);
        if (!rows.length) { host.innerHTML = ''; return; }
        var visible = cols.filter(function (c) { return !c.hidden; });
        var buttons = opts.buttons || [];
        var h = '<table class="win-grid"><thead><tr>';
        buttons.forEach(function (b) { h += '<th>' + esc(b.cap || b.text) + '</th>'; });
        visible.forEach(function (c) { h += '<th>' + esc(c.cap || c.k) + '</th>'; });
        h += '</tr></thead><tbody>';
        var sums = {};
        rows.forEach(function (r, i) {
            h += '<tr data-i="' + i + '">';
            buttons.forEach(function (b) { h += '<td><button type="button" class="grid-btn" data-btn="' + esc(b.key) + '">' + esc(b.text) + '</button></td>'; });
            visible.forEach(function (c) {
                var v = ci(r, c.s || c.k);
                var cls = c.t === 'num' ? 'num' : (c.t === 'int' ? 'int' : '');
                var style = opts.rowStyle ? opts.rowStyle(c.k, str(v)) : '';
                var shown = c.t === 'num' ? fmt(v, c.d) : (c.t === 'date' ? shortDate(v) : str(v));
                if (c.sum) sums[c.k] = (sums[c.k] || 0) + num(v);
                h += '<td class="' + cls + ' ' + style + '">' + esc(shown) + '</td>';
            });
            h += '</tr>';
        });
        h += '</tbody>';
        if (Object.keys(sums).length) {
            h += '<tfoot><tr>';
            buttons.forEach(function () { h += '<td></td>'; });
            visible.forEach(function (c) { h += '<td class="num">' + (c.sum ? esc(fmt(sums[c.k], c.d)) : '') + '</td>'; });
            h += '</tr></tfoot>';
        }
        h += '</table>';
        host.innerHTML = h;
        var tbody = host.querySelector('tbody');
        tbody.addEventListener('click', function (e) {
            var tr = e.target.closest('tr'); if (!tr) return;
            host.querySelectorAll('tr.is-current').forEach(function (x) { x.classList.remove('is-current'); });
            tr.classList.add('is-current');
            var b = e.target.closest('[data-btn]');
            if (b && opts.onButton) opts.onButton(b.getAttribute('data-btn'), rows[+tr.getAttribute('data-i')]);
        });
        if (opts.onDblClick) {
            tbody.addEventListener('dblclick', function (e) {
                var tr = e.target.closest('tr'); if (!tr) return;
                opts.onDblClick(rows[+tr.getAttribute('data-i')]);
            });
        }
    }
    function currentRow(hostId, rows) {
        var tr = $id(hostId).querySelector('tr.is-current');
        return tr ? rows[+tr.getAttribute('data-i')] : null;
    }

    // ==================================================================================== history

    function showHistory() {
        var mode = (document.querySelector('input[name=histDate]:checked') || {}).value || 'doc';
        return get('/history', {
            dateMode: mode,
            fromDate: pickDate('chkFromDate', 'FromDate'),
            toDate: pickDate('chkToDate', 'ToDate'),
            ticketNoFrom: intOf(val('txtTicketNoFrom')),
            ticketNoTo: intOf(val('txtTicketNoTo')),
            gpNoFrom: intOf(val('GPNoFrom')),
            gpNoTo: intOf(val('GpNoTo')),
            vehicleNo: val('TxtVehicleNumber').trim(),
            refDocumentTypeId: intOf(val('CmbGatePassTypeHistory')),
            wbTypeId: intOf(val('CmbWbTypeHistory')),
            wbTypeText: selectedText('CmbWbTypeHistory')
        }).then(function (rows) {
            historyRows = rows || [];
            $id('lblHistoryCount').textContent = historyRows.length ? '(' + historyRows.length + ')' : '';
            renderGrid('DataGridHistory', historyRows, HISTORY_COLS, {
                buttons: [
                    { key: 'Edit', text: 'Edit' }, { key: 'Print', text: 'Print' },
                    { key: 'PrintWithImage', text: 'PrintImage' }, { key: 'PrintWithLab', text: 'PrintWithLab' }
                ],
                onButton: historyButton,
                onDblClick: function (row) { readById(intOf(ci(row, 'Id'))); }
            });
        }).catch(function (e) { box(e.message); });
    }

    /** DataGridHistory_ColumnButtonClick:4515 */
    function historyButton(key, row) {
        var id = intOf(ci(row, 'Id'));
        if (key === 'Edit') readById(id);
        if (key === 'Print') printSlip(id, false);
        if (key === 'PrintWithImage') printSlip(id, true);
        if (key === 'PrintWithLab') {
            if (intOf(ci(row, 'ReferenceDocTypeId')) !== 51 || intOf(ci(row, 'ReferenceDocNoId')) <= 0) {
                box("This Print Is Only For 'Inward GatePass'");
                return;
            }
            slip257For(intOf(ci(row, 'ReferenceDocNoId')));
        }
    }

    /** BtnNewHistory_Click:3688 */
    function newHistory() {
        setVal('txtTicketNoFrom', '0');
        setVal('txtTicketNoTo', '');
        setVal('TxtVehicleNumber', '');
        setVal('GPNoFrom', '');
        setVal('GpNoTo', '');
        setVal('FromDate', isoDay(daysAgo(3)));
        setVal('ToDate', isoDay(new Date()));
    }

    /** BtnRefreshHistory_Click:3706 — binds the reference types into BOTH combos, the W.B Type
     *  one included, exactly as written. */
    function refreshHistoryLists() {
        get('/refresh-history-lists').then(function (d) {
            bindSelect('CmbWbTypeHistory', d.wbTypesFromReferenceTypes, 'Id', 'Name', { blank: true });
            bindSelect('cmbReferenceType', d.referenceTypes, 'Id', 'Name', { blank: true });
            bindSelect('CmbGatePassTypeHistory', d.referenceTypes, 'Id', 'Name', { blank: true });
        }).catch(function (e) { box(e.message); });
    }

    // ===================================================================================== prints

    function printSlip(id, withImages) {
        if (!id) { box('Not Record Found For Display'); return; }
        get('/slip/' + id, { withImages: withImages }).then(function (rows) {
            if (!rows || !rows.length) { box('Not Record Found For Display'); return; }
            var extra = '';
            if (withImages && camerasOn()) {
                extra = '<h2>Pictures</h2><div style="display:grid;grid-template-columns:repeat(3,1fr);gap:4px;">'
                    + FIRST_FIELDS.concat(SECOND_FIELDS).map(function (f) {
                        return '<div style="border:1px solid #444;padding:2px;font-size:9px;">' + esc(f)
                            + '<img style="width:100%;display:block;" onerror="this.remove()" src="'
                            + location.origin + api + '/device/ticket/' + id + '/picture/' + f + '"></div>';
                    }).join('') + '</div>';
            }
            openPrint(withImages ? '281 - Weigh Bridge Slip With Pictures' : '280 - Weigh Bridge Slip',
                      [{ title: '', rows: rows }], extra);
        }).catch(function (e) { box(e.message); });
    }

    /** GenerateReport():4379 — Print button and Ctrl+P on an open ticket. */
    function print280() {
        if (!rights.canPrint) return;
        printSlip(RECID, false);
    }

    /** btnSlip257_Click:5144 */
    function slip257() {
        if (intOf(val('cmbReferenceType')) === 51 && selectedText('cmbReferenceType').trim() === 'GatePass Inward') {
            if (intOf(val('cmbRefDocNo')) > 0) { slip257For(intOf(val('cmbRefDocNo'))); return; }
            box('Please Select the GatePass No First...');
            return;
        }
        box('Please Select the GatePass Type To Inward...');
    }

    /** CommonServices.InwardGatePassWithWbAndLabSlip:14268 */
    function slip257For(gpId) {
        if (!(gpId > 0)) { box('No Record Found For Display'); return; }
        get('/gate-pass-slip/' + gpId).then(function (d) {
            openPrint('257 - Inward Gate Pass With Weigh Bridge And Lab Slip', [
                { title: 'Gate Pass', rows: d.main || [] },
                { title: 'Lab Analysis', rows: d.lab || [], table: true },
                { title: 'Weigh Bridge', rows: d.weighBridge || [], table: true }
            ]);
        }).catch(function (e) { box(e.message); });
    }

    function openPrint(title, sections, extraHtml) {
        var w = window.open('', '_blank');
        if (!w) { box('The browser blocked the print window. Allow pop-ups for this site.'); return; }
        var h = '<!DOCTYPE html><html><head><meta charset="utf-8"><title>' + esc(title) + '</title>'
              + '<style>body{font-family:Verdana,sans-serif;font-size:11px;margin:16px;}h1{font-size:15px;margin:0 0 2px;}'
              + 'h2{font-size:12px;margin:14px 0 4px;border-bottom:1px solid #000;}.co{font-weight:bold;font-size:13px;}'
              + 'table{border-collapse:collapse;width:100%;}td,th{border:1px solid #444;padding:3px 5px;text-align:left;}'
              + 'th{background:#eee;}.kv td:first-child{width:32%;font-weight:bold;background:#f6f6f6;}'
              + '@media print{button{display:none}}</style></head><body>'
              + '<div class="co">' + esc(L ? L.companyName : '') + '</div><h1>' + esc(title) + '</h1>'
              + '<button onclick="window.print()">Print</button>';
        sections.forEach(function (s) {
            if (s.title) h += '<h2>' + esc(s.title) + '</h2>';
            if (!s.rows.length) { h += '<div>No rows.</div>'; return; }
            if (s.table || s.rows.length > 1) {
                var keys = Object.keys(s.rows[0]);
                h += '<table><thead><tr>' + keys.map(function (k) { return '<th>' + esc(k) + '</th>'; }).join('') + '</tr></thead><tbody>';
                s.rows.forEach(function (r) { h += '<tr>' + keys.map(function (k) { return '<td>' + esc(r[k]) + '</td>'; }).join('') + '</tr>'; });
                h += '</tbody></table>';
            } else {
                h += '<table class="kv">';
                Object.keys(s.rows[0]).forEach(function (k) { h += '<tr><td>' + esc(k) + '</td><td>' + esc(s.rows[0][k]) + '</td></tr>'; });
                h += '</table>';
            }
        });
        h += (extraHtml || '') + '</body></html>';
        w.document.open(); w.document.write(h); w.document.close();
    }

    // ============================================================================= misc buttons

    function openPurchaseLab() { window.open('/quality/purchase-analysis', '_blank'); }
    function notPorted(what) { box(what + ' has not been built on the web yet.'); }

    /** MakeShortCutKeys():4947 — the desktop's own list, both "Ctrl+F1" rows included. */
    function shortcutKeys() {
        var rows = [
            ['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'],
            ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Ctrl+F1', 'For Weight Bridge Lookup '],
            ['Ctrl+F1', 'For Lookup Vehicle Weight'], ['Ctrl+F5', 'For Getting Weight From Indicator'],
            ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', "To Show ShortCut Keys Form"],
            ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus on GatePass Type'],
            ['Ctrl+Enter', 'For Update Record When Focus On Any Grid '],
            ['Ctrl+Space', "To Call Function's On Button Or Link When Focus On Any Grid "]
        ];
        $id('shortcutTable').innerHTML = '<thead><tr><th>KeyCombination</th><th>Description</th></tr></thead><tbody>'
            + rows.map(function (r) { return '<tr><td>' + esc(r[0]) + '</td><td>' + esc(r[1]) + '</td></tr>'; }).join('') + '</tbody>';
        $id('shortcutModal').classList.add('is-open');
    }
    function closeShortcutKeys() { $id('shortcutModal').classList.remove('is-open'); }

    function tab(id) {
        document.querySelectorAll('#tabs1 .win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
        ['tabForm', 'tabHistory'].forEach(function (p) { $id(p).classList.toggle('is-active', p === id); });
        if (id === 'tabHistory') focusEl('FromDate');      // tabControl1_SelectedIndexChanged:4481
    }
    function tab2(id) {
        document.querySelectorAll('#tabs2 .win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab2') === id); });
        ['tabPending2nd', 'tabPending1st'].forEach(function (p) { $id(p).classList.toggle('is-active', p === id); });
    }
    function activeTab() { return $id('tabHistory').classList.contains('is-active') ? 1 : 0; }

    // ===================================================================================== events

    function digitsOnly(id, allowDot) {
        var e = $id(id);
        if (!e) return;
        e.addEventListener('keypress', function (ev) {
            if (ev.ctrlKey || ev.metaKey || ev.key.length !== 1) return;
            if (/\d/.test(ev.key)) return;
            if (allowDot && ev.key === '.' && e.value.indexOf('.') < 0) return;
            ev.preventDefault();
        });
    }

    function wireEvents() {
        $id('cmbReferenceType').addEventListener('change', function () { cmbReferenceType_Leave(); });
        $id('cmbRefDocNo').addEventListener('change', function () { cmbRefDocNo_Leave(); });
        $id('txtrefdocNumberofMoveOrder').addEventListener('blur', function () { cmbRefDocNo_Leave(); });
        $id('CmbPartyName').addEventListener('blur', CmbPartyName_Leave);
        $id('CmbItem').addEventListener('blur', function () { enforceLimit('item', 'CmbItem'); });

        $id('txtFirstWeight').addEventListener('input', CalculateNetWeight);
        $id('txtsecondWeight').addEventListener('input', onSecondWeightChanged);
        $id('txtSupplierFirstWeight').addEventListener('input', CalculateSupplierNetWeight);
        $id('txtSupplierSecondWeight').addEventListener('input', CalculateSupplierNetWeight);
        ['txtEbUnit', 'txtItemqty'].forEach(function (id) { $id(id).addEventListener('input', recalcPacking); });
        $id('txtVehicleNo').addEventListener('input', function () {
            var e = $id('txtVehicleNo'); var p = e.selectionStart; e.value = e.value.toUpperCase(); e.setSelectionRange(p, p);
        });
        $id('chkEnableWeights').addEventListener('change', function () { weightsEditable($id('chkEnableWeights').checked); });

        digitsOnly('txtWorkingReportNo', false);
        digitsOnly('txtWbCharges', false);
        digitsOnly('txtSupplierFirstWeight', true);
        digitsOnly('txtSupplierSecondWeight', true);
        ['txtTicketNoFrom', 'txtTicketNoTo', 'GPNoFrom', 'GpNoTo'].forEach(function (id) { digitsOnly(id, false); });

        document.addEventListener('keydown', onKeyDown);
    }

    /** frmWeightbridge_KeyDown:4793. Browsers keep Ctrl+N and Ctrl+T for themselves (new window,
     *  new tab); those two cannot be taken over by a page. */
    function onKeyDown(e) {
        var k = e.key;
        if (k === 'Enter' && !e.ctrlKey && !e.altKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName)
                && e.target.type !== 'checkbox' && e.target.type !== 'radio') {
            var f = Array.prototype.filter.call(document.querySelectorAll('input,select,button,textarea'), function (x) {
                return !x.disabled && x.offsetParent !== null && x.tabIndex >= 0;
            });
            var i = f.indexOf(e.target);
            if (i >= 0 && i < f.length - 1) { e.preventDefault(); f[i + 1].focus(); }
        }
        if (e.ctrlKey && (k === 't' || k === 'T')) { e.preventDefault(); tab(activeTab() === 1 ? 'tabForm' : 'tabHistory'); }
        if (e.ctrlKey && (k === 'e' || k === 'E')) { e.preventDefault(); window.location.href = '/weighbridge'; return; }

        /* the Shift+Z,A,I,N sequence that reveals "Enable Weights" (:4821) */
        if (e.shiftKey && !e.ctrlKey) {
            var up = k.length === 1 ? k.toUpperCase() : '';
            if (up === 'Z' || up === 'A' || up === 'I' || up === 'N') {
                keySequence += up;
                if (keySequence.length > 10) keySequence = keySequence.substring(keySequence.length - 10);
                if (/ZAIN$/.test(keySequence)) { show('lblEnableWeights', true); keySequence = ''; }
            }
        } else if (k !== 'Shift') {
            keySequence = '';
        }
        if (e.ctrlKey && e.altKey && (k === 'Control' || k === 'Alt')) shortcutKeys();

        if (activeTab() === 1) {
            if (e.ctrlKey && (k === 'p' || k === 'P')) {
                e.preventDefault();
                var r = currentRow('DataGridHistory', historyRows);
                if (r) printSlip(intOf(ci(r, 'Id')), false);
            }
            return;
        }
        if (e.ctrlKey && (k === 's' || k === 'S') && saveActive()) { e.preventDefault(); save(); }
        if (e.ctrlKey && (k === 'u' || k === 'U') && updateActive()) { e.preventDefault(); update(); }
        if (e.ctrlKey && (k === 'n' || k === 'N')) { e.preventDefault(); formRefresh(); }
        if (k === 'F5') { e.preventDefault(); readWeight(); }
        if (e.ctrlKey && (k === 'p' || k === 'P') && updateActive()) { e.preventDefault(); print280(); }
        if (e.ctrlKey && k === 'F1') { e.preventDefault(); window.open('/weighbridge/weigh-bridge-general-lookups', '_blank'); }
        if (e.ctrlKey && k === 'F2') { e.preventDefault(); window.open('/weighbridge/vehicle-weight-lookup', '_blank'); }
        if (e.ctrlKey && k === 'ArrowUp') { e.preventDefault(); focusEl('cmbReferenceType'); }
        if (e.ctrlKey && k === 'ArrowDown') { e.preventDefault(); $id('grdPendingForSecond').focus(); }
        if (e.ctrlKey && k === 'ArrowRight') {
            e.preventDefault();
            if ($id('tabPending1st').classList.contains('is-active')) { tab2('tabPending2nd'); $id('grdPendingForSecond').focus(); }
            else { tab2('tabPending1st'); $id('grdPendingFirst').focus(); }
        }
        if (e.ctrlKey && k === 'Enter' && document.activeElement === $id('grdPendingForSecond')) {
            var pr = currentRow('grdPendingForSecond', pendingSecondRows);
            if (pr) readById(intOf(ci(pr, 'Id')));
        }
    }

    window.WeighBridge = {
        newForm: function () { formRefresh(); },
        save: save,
        update: update,
        refresh: refresh,
        print280: print280,
        slip257: slip257,
        openPurchaseLab: openPurchaseLab,
        notPorted: notPorted,
        shortcutKeys: shortcutKeys,
        closeShortcutKeys: closeShortcutKeys,
        readWeight: readWeight,
        tab: tab,
        tab2: tab2,
        showPending2nd: showPending2nd,
        resetPending2nd: resetPending2nd,
        showPending1st: showPending1st,
        resetPending1st: resetPending1st,
        showHistory: showHistory,
        newHistory: newHistory,
        refreshHistoryLists: refreshHistoryLists,
        /* exposed for the unit check */
        _test: { dstr: dstr }
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init);
    else init();
})();
