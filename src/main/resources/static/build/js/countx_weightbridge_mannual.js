/* ============================================================================================
 * Screen 410 "Weigh Bridge Manual" — Architecture.WinApp.WeightBridge.WeightbridgeMannual,
 * DocTypeId 113. Each function names the desktop method it reproduces.
 *
 * THE DESKTOP'S EVENT WIRING, AND WHAT IT MEANS HERE
 *   cmbReferenceType.TextChanged + Leave  -> cmbReferenceType_Leave -> GatePassBending
 *   CmbGatePassNo.TextChanged + Leave     -> BindingAgainstGatePass
 * Because both run on TEXT CHANGE, rebinding a list or activating a row re-runs them. The two
 * places where that visibly changes the result are reproduced: GatePassBending clearing a gate
 * pass that was showing re-runs the fill with nothing selected, and ReadById setting the
 * reference type and then the gate pass runs the bending and the fill before the saved ticket's
 * own values are written over them.
 *
 * Free-text combos (Wb Type, Party, Item) are <input list=…> resolved against their rows the
 * Infragistics way — Value is the matching row's Id, "ActiveRow" is "the text matches a row".
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/weighbridge-manual';
    var shared = '/api/weighbridge';

    if (window.DesktopCombo) {
        /* GetGatePassOutward:566 binds Id, GpSrNo, VehicleNo, GatepassType. */
        window.DesktopCombo.define('wbmGatePass', [
            { caption: 'GatePass No',  flex: 2 },
            { caption: 'VehicleNo',    flex: 2, key: 'vehicle' },
            { caption: 'GatepassType', flex: 2, key: 'gptype' }
        ]);
    }

    var L = null, cfg = {};
    var rights = { canSave: false, canUpdate: false, canPrint: false };
    var RECID = 0, Approved = false, RefDocumentTypeId = 0;
    var dtPartiesAndItemsFromDeliveryOrder = [];
    var firstDate = new Date(), secondDate = new Date();
    var refDocRows = [], invoiceRows = [], savedVehicleRows = [];
    var pendingSecondRows = [], pendingFirstRows = [], historyRows = [];
    var src = {
        wbType: { rows: [], key: 'GpTypeDescription', limit: false },
        party:  { rows: [], key: 'PartyName', limit: false },
        item:   { rows: [], key: 'ItemName', limit: false }
    };

    /* ------------------------------------------------------------------ helpers */

    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : String(v); }
    function enable(id, on) { var e = $id(id); if (e) e.disabled = !on; }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function box(m) { window.alert(m); }
    function ask(m) { return window.confirm(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function ci(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var lower = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === lower) return row[k];
        return '';
    }
    function str(v) { return (v === null || v === undefined) ? '' : String(v); }
    function num(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function intOf(v) { return Math.round(num(v)); }
    function dstr(n) { return String(Math.round(n * 1e10) / 1e10); }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function isoDay(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function isoLocal(d) { return isoDay(d) + 'T' + pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds()); }
    function timeOf(d) { return pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds()); }
    function daysAgo(n) { var d = new Date(); d.setDate(d.getDate() - n); return d; }
    function parseDate(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2})(?::(\d{2}))?)?/.exec(str(v));
        return m ? new Date(+m[1], +m[2] - 1, +m[3], +(m[4] || 0), +(m[5] || 0), +(m[6] || 0)) : null;
    }
    function shortDate(v) { var d = parseDate(v); return d ? pad(d.getDate()) + '/' + pad(d.getMonth() + 1) + '/' + d.getFullYear() : str(v); }
    function fmt(v, dec) {
        if (v === '' || v === null || v === undefined) return '';
        return num(v).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: dec === undefined ? 3 : dec });
    }
    function focusEl(id) {
        var e = $id(id); if (!e) return;
        var w = e.closest ? e.closest('.dtcombo-wrap') : null;
        var t = w ? w.querySelector('input') : e;
        if (t && t.focus) t.focus();
    }

    function fetchJson(url, opts) {
        return fetch(url, Object.assign({ credentials: 'same-origin', headers: { 'Accept': 'application/json' } }, opts || {}))
            .then(function (r) {
                return r.text().then(function (t) {
                    var body = null;
                    try { body = t ? JSON.parse(t) : null; } catch (e) { body = null; }
                    if (!r.ok) throw new Error(body && body.message ? body.message : ('Request failed (' + r.status + ')'));
                    return body;
                });
            });
    }
    function get(path, params, base) {
        return fetchJson((base || api) + path + (params ? '?' + new URLSearchParams(params).toString() : ''));
    }
    function post(path, body) {
        return fetchJson(api + path, { method: 'POST', headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' }, body: JSON.stringify(body) });
    }

    /* ------------------------------------------------------------------ selects */

    function bindSelect(id, rows, valueKey, textKey, opts) {
        opts = opts || {};
        var h = opts.blank === false ? '' : '<option value=""></option>';
        (rows || []).forEach(function (r) {
            var extra = '';
            (opts.attrs || []).forEach(function (a) { extra += ' data-' + a[0] + '="' + esc(ci(r, a[1])) + '"'; });
            h += '<option value="' + esc(ci(r, valueKey)) + '"' + extra + '>' + esc(ci(r, textKey)) + '</option>';
        });
        $id(id).innerHTML = h;
    }
    function selectedText(id) {
        var s = $id(id); if (!s || s.selectedIndex < 0) return '';
        var o = s.options[s.selectedIndex]; return o && o.value !== '' ? o.textContent : '';
    }
    function hasActiveRow(id) {
        var s = $id(id); return !!(s && s.selectedIndex >= 0 && s.options[s.selectedIndex] && s.options[s.selectedIndex].value !== '');
    }
    function selectValue(id, v) { var s = $id(id); if (!s) return; s.value = str(v); if (s.value !== str(v)) s.value = ''; }
    function activateFirst(id) { var s = $id(id); if (s && s.options.length > 1) s.selectedIndex = 1; }
    function setSelectByText(id, text) {
        var s = $id(id); if (!s) return;
        for (var i = 0; i < s.options.length; i++) {
            if (s.options[i].value !== '' && s.options[i].textContent === str(text)) { s.selectedIndex = i; return; }
        }
        s.value = '';
    }
    /** Text on a combo whose TEXT is what gets saved (Packing Type): keep unmatched text. */
    function setSelectTextKeep(id, text) {
        setSelectByText(id, text);
        if (!hasActiveRow(id) && str(text) !== '') {
            var s = $id(id), o = document.createElement('option');
            o.value = '0'; o.textContent = str(text);
            s.insertBefore(o, s.options[1] || null); s.value = '0';
        }
    }

    /* ------------------------------------------------------------------ free-text combos */

    function bindText(which, listId, rows, opts) {
        opts = opts || {};
        var s = src[which];
        s.rows = (rows || []).slice();
        if (opts.zeroRow) { var z = { Id: 0 }; z[s.key] = '...Select Any Value...'; s.rows.unshift(z); }
        s.limit = !!opts.limitToList;
        $id(listId).innerHTML = s.rows.map(function (r) { return '<option value="' + esc(ci(r, s.key)) + '"></option>'; }).join('');
    }
    function textRow(which, inputId) {
        var t = val(inputId).toLowerCase(), s = src[which];
        if (t === '') return null;
        for (var i = 0; i < s.rows.length; i++) if (str(ci(s.rows[i], s.key)).toLowerCase() === t) return s.rows[i];
        return null;
    }
    function textValue(which, inputId) { var r = textRow(which, inputId); return r ? intOf(ci(r, 'Id')) : 0; }
    function setTextById(which, inputId, id) {
        var s = src[which];
        for (var i = 0; i < s.rows.length; i++) if (intOf(ci(s.rows[i], 'Id')) === intOf(id)) { setVal(inputId, ci(s.rows[i], s.key)); return true; }
        return false;
    }
    function enforceLimit(which, inputId) {
        if (src[which].limit && val(inputId) !== '' && !textRow(which, inputId)) setVal(inputId, '');
    }

    /** DDL.BindDDL(dtitem, CmbItem, …, ZeroIndex: true) followed by the retain-by-id block —
     *  getAllitems / ItemsFromDeliveryOrder / ItemsFromSaleOrder / ItemsAgainstPartyForOutward. */
    function itemsBindRetain(rows, limitToList) {
        var prev = textValue('item', 'CmbItem');
        if (!rows.length) { setVal('CmbItem', ''); bindText('item', 'dlItem', [], {}); return; }
        bindText('item', 'dlItem', rows, { zeroRow: true, limitToList: limitToList });
        setVal('CmbItem', '...Select Any Value...');                  // DDL.Value = 0
        if (prev > 0 && !setTextById('item', 'CmbItem', prev)) setVal('CmbItem', '');
    }
    /** getAllitems():943. */
    function getAllitems() {
        itemsBindRetain((L.items || []).map(function (r) { return { Id: ci(r, 'Id'), ItemName: ci(r, 'ItemName'), PartyId: 0 }; }), false);
    }
    /** PartyNameBind():440 — no default row; the typed text stays unless it named a row that
     *  has gone. */
    function PartyNameBind(rows, limitToList) {
        var prev = textValue('party', 'CmbPartyName');
        rows = rows || L.parties || [];
        if (!rows.length) { bindText('party', 'dlParty', [], {}); setVal('CmbPartyName', ''); return; }
        bindText('party', 'dlParty', rows, { limitToList: !!limitToList });
        if (prev > 0 && !setTextById('party', 'CmbPartyName', prev)) setVal('CmbPartyName', '');
    }
    /** PartyNameBindforOutward():493. */
    function PartyNameBindforOutward() {
        var rows = [], seen = {};
        dtPartiesAndItemsFromDeliveryOrder.forEach(function (r) {
            var id = intOf(ci(r, 'SupplierCustomerId'));
            if (!seen[id]) { rows.push({ Id: id, PartyName: ci(r, 'CustomerName') }); seen[id] = true; }
        });
        if (!rows.length) rows = src.party.rows;
        PartyNameBind(rows, true);
    }
    /** ItemsAgainstPartyForOutward(PartyId):999. */
    function ItemsAgainstPartyForOutward(partyId) {
        var rows = [], seen = {};
        dtPartiesAndItemsFromDeliveryOrder.forEach(function (r) {
            var id = intOf(ci(r, 'ItemId')), party = intOf(ci(r, 'SupplierCustomerId'));
            if (!seen[id] && party === partyId) { rows.push({ Id: id, ItemName: ci(r, 'ItemName'), PartyId: party }); seen[id] = true; }
        });
        if (!rows.length) rows = src.item.rows.filter(function (r) { return intOf(ci(r, 'Id')) !== 0; });
        itemsBindRetain(rows, false);
    }
    /** ItemsFromDeliveryOrder:1082 / ItemsFromSaleOrder:1124 — an empty answer leaves dtitem as
     *  it was and binds that again. */
    function itemsFrom(path, params) {
        return get(path, params, shared).then(function (rows) {
            rows = (rows || []).map(function (r) { return { Id: ci(r, 'Id'), ItemName: ci(r, 'ItemName'), PartyId: 0 }; });
            if (!rows.length) rows = src.item.rows.filter(function (r) { return intOf(ci(r, 'Id')) !== 0; });
            itemsBindRetain(rows, false);
        }).catch(function (e) { box(e.message); });
    }
    /** CmbPartyName_Leave():1592. */
    function CmbPartyName_Leave() {
        enforceLimit('party', 'CmbPartyName');
        var id = textValue('party', 'CmbPartyName');
        if (cfg.weighBridgeForOutwardPartyAndItemWise && selectedText('cmbReferenceType') === 'GatePass OutWard' && id > 0) ItemsAgainstPartyForOutward(id);
        else getAllitems();
    }

    // ===================================================================================== load

    function init() {
        setVal('FromDate', isoDay(new Date()));      // designer default DateTime.Now; New History sets -3
        setVal('ToDate', isoDay(new Date()));
        stampTimes();
        wire();
        get('/lookups').then(function (d) {
            L = d; cfg = d;
            rights.canSave = !!d.canSave; rights.canUpdate = !!d.canUpdate; rights.canPrint = !!d.canPrint;
            $id('btnsave').disabled = !rights.canSave;
            $id('btnUpdate').disabled = !rights.canUpdate;
            $id('btnPrint').disabled = !rights.canPrint;
            var missing = [];
            if (!rights.canSave) missing.push('Save');
            if (!rights.canUpdate) missing.push('Update');
            if (!rights.canPrint) missing.push('Print');
            if (missing.length) { $id('rightsNote').textContent = 'Your user rights on Weigh Bridge Manual do not include: ' + missing.join(', ') + '.'; show('rightsNote', true); }

            /* VehicleTypesFill:908 — BindDDLNew, row 0 activated. */
            bindSelect('cmbVehicleType', d.vehicleTypes, 'Id', 'VehicleDescription');
            activateFirst('cmbVehicleType');
            getAllitems();
            /* WbType:1166 — nothing activated. */
            bindText('wbType', 'dlWbType', d.wbTypes, {});
            bindSelect('CmbWbTypeHistory', d.wbTypes, 'Id', 'GpTypeDescription');
            /* PackingTypeForWBFill:540 */
            bindSelect('cmbPackingType', d.packingTypes, 'Id', 'type');
            PartyNameBind();
            setVal('txtTickectNo', d.ticketNo > 0 ? d.ticketNo : '');
            /* GatePassTypeFill:925 — row 0 activated, which fires the reference combo's
               TextChanged, i.e. cmbReferenceType_Leave. */
            bindSelect('cmbReferenceType', d.referenceTypes, 'Id', 'Name');
            bindSelect('CmbGatePassTypeHistory', d.referenceTypes, 'Id', 'Name');
            activateFirst('cmbReferenceType');
            cmbReferenceType_Leave();
            loadPending();
            enable('txtsecondWeight', true);
            focusEl('cmbReferenceType');
        }).catch(function (e) { box(e.message); });
    }

    function stampTimes() {
        firstDate = new Date(); secondDate = new Date();
        setVal('txtFirstDate', timeOf(firstDate)); setVal('txtSecondDate', timeOf(secondDate));
    }
    function saveActive()   { return !$id('btnsave').classList.contains('is-hidden') && !$id('btnsave').disabled; }
    function updateActive() { return !$id('btnUpdate').classList.contains('is-hidden') && !$id('btnUpdate').disabled; }

    // ================================================================ reference type (bending)

    /** cmbReferenceType_Leave():1248. */
    function cmbReferenceType_Leave() {
        var p = GatePassBending();
        var v = intOf(val('cmbReferenceType'));
        if (cfg.lstIsSavedVehicle) {
            if (v === 74 || v === 218) return p.then(GetSavedVehicles);
            savedVehicleMode(false);
        } else {
            enable('txtsecondWeight', true);
        }
        return p;
    }

    function savedVehicleMode(on) {
        show('wrapVehicleNo', on); enable('cmbVehicleNo', on);
        show('txtVehicleNo', !on); enable('txtVehicleNo', !on);
    }

    /** GetSavedVehicles():875. */
    function GetSavedVehicles() {
        savedVehicleMode(true);
        return get('/saved-vehicles').then(function (rows) {
            savedVehicleRows = rows || [];
            bindSelect('cmbVehicleNo', savedVehicleRows, 'Id', 'VehicleNo');
        }).catch(function (e) { box(e.message); });
    }

    /** cmbVehicleNo_TextChanged:1276 — the saved vehicle's 4th column becomes the second weight. */
    function cmbVehicleNo_TextChanged() {
        if (!cfg.lstIsSavedVehicle || $id('cmbVehicleNo').disabled || !hasActiveRow('cmbVehicleNo')) return;
        var row = null, id = val('cmbVehicleNo');
        savedVehicleRows.forEach(function (r) { if (str(ci(r, 'Id')) === id) row = r; });
        if (!row) return;
        var keys = Object.keys(row);
        setVal('txtsecondWeight', keys.length > 3 ? str(row[keys[3]]) : '');
        enable('txtsecondWeight', false);
        CalculateNetWeight();
    }

    /** GatePassBending():1183. */
    function GatePassBending() {
        var text = selectedText('cmbReferenceType').trim(), v = intOf(val('cmbReferenceType'));
        var had = selectedText('CmbGatePassNo') !== '';
        function load(mode, docType, withCols) {
            return get('/ref-docs', { mode: mode, documentTypeId: docType || 0, recId: RECID }).then(function (rows) {
                refDocRows = rows || [];
                $id('CmbGatePassNo').setAttribute('data-dtcombo', withCols ? 'wbmGatePass' : 'single');
                bindSelect('CmbGatePassNo', refDocRows, 'Id', 'GpSrNo', { attrs: [['vehicle', 'VehicleNo'], ['gptype', 'GatepassType']] });
                /* Clearing a gate pass that was showing fires its TextChanged — the fill runs
                   once with nothing chosen. */
                if (had) return BindingAgainstGatePass();
            }).catch(function (e) { box(e.message); });
        }
        enable('CmbGatePassNo', true);
        if (text === 'GatePass OutWard') return load('outward', 0, true);
        if (text === 'GatePass OutWard Steel') return load('steel');
        if (v === 51 && text === 'GatePass Inward') return load('lab');
        if (v === 1601 && text === 'GatePass Inward') return load('engr');
        if (v === 52 && text === 'General GatePass Inward') return load('general', 52);
        if (v === 92 && text === 'General GatePass Outward') return load('general', 92);
        if (v === 74 || v === 75 || v === 218) {
            FieldsEnables();
            refDocRows = [{ Id: val('txtTickectNo'), GpSrNo: val('txtTickectNo') }];
            $id('CmbGatePassNo').setAttribute('data-dtcombo', 'single');
            bindSelect('CmbGatePassNo', refDocRows, 'Id', 'GpSrNo');
            activateFirst('CmbGatePassNo');
            return BindingAgainstGatePass();
        }
        if (v === 54 || v === 55) return load('party', v);
        return Promise.resolve();
    }

    function FieldsDisables() { enable('txtBilityNo', false); enable('txtVehicleNo', false); enable('cmbVehicleType', false); }
    function FieldsEnables() { enable('txtBilityNo', true); enable('txtVehicleNo', true); enable('cmbVehicleType', true); enable('txtItemqty', true); }

    /** ClearFormFields():1563. */
    function ClearFormFields() {
        setVal('txtBilityNo', ''); setVal('txtVehicleNo', ''); selectValue('cmbVehicleType', '');
        setVal('txtItemqty', ''); setVal('CmbItem', ''); RefDocumentTypeId = 0; setVal('CmbWeighBridgeType', '');
    }

    function refDocRow() {
        var id = val('CmbGatePassNo'), row = null;
        refDocRows.forEach(function (r) { if (str(ci(r, 'Id')) === id) row = r; });
        return row;
    }
    /** SelectedRow.Cells["GatepassType"] — "" when the list has no such column (:1616-1620). */
    function refDocGatepassType() { var r = refDocRow(); return r && 'GatepassType' in r ? str(r.GatepassType) : ''; }

    function clearInvoices() { invoiceRows = []; bindSelect('CmbInvoiceNo', [], 'Id', 'InvoiceNo'); }

    /** getInvoiceNoFromDeliveryOrderByGpId:1047 — row 0 activated. */
    function getInvoiceNo(gpId) {
        enable('CmbInvoiceNo', true);
        var prev = intOf(val('CmbInvoiceNo'));
        return get('/delivery-order-invoices', { gpId: gpId }, shared).then(function (rows) {
            invoiceRows = rows || [];
            if (!invoiceRows.length) { clearInvoices(); return; }
            bindSelect('CmbInvoiceNo', invoiceRows, 'Id', 'InvoiceNo');
            activateFirst('CmbInvoiceNo');
            if (prev > 0) {
                if (invoiceRows.some(function (r) { return intOf(ci(r, 'Id')) === prev; })) selectValue('CmbInvoiceNo', prev);
                else selectValue('CmbInvoiceNo', '');
            }
        }).catch(function (e) { box(e.message); });
    }

    /** LabItemsBindByGpId + BindDDLNew(…, "ItemId", "ItemName") with row 0 activated. */
    function labItems(actionId, orElse) {
        return get('/lab-items', { gpId: intOf(val('CmbGatePassNo')), actionId: actionId || 0 }, shared).then(function (rows) {
            rows = rows || [];
            if (rows.length) {
                setVal('CmbItem', '');
                bindText('item', 'dlItem', rows.map(function (r) { return { Id: ci(r, 'ItemId'), ItemName: ci(r, 'ItemName'), PartyId: 0 }; }), {});
                setVal('CmbItem', ci(src.item.rows[0], 'ItemName'));
            } else if (orElse) orElse();
        }).catch(function (e) { box(e.message); });
    }

    /** cmbRefDocNo_Leave():1292 -> BindingAgainstGatePass():1305; any failure clears the
     *  fields it was filling and shows the message. */
    function BindingAgainstGatePass() {
        return bindingInner().catch(function (e) { ClearFormFields(); box(e.message); });
    }

    function bindingInner() {
        ['label6', 'label16', 'label41'].forEach(function (id) { $id(id).classList.add('invisible-keep'); });
        enable('CmbItem', true); enable('CmbPartyName', true);
        clearInvoices();
        dtPartiesAndItemsFromDeliveryOrder = [];
        var text = selectedText('cmbReferenceType'), v = intOf(val('cmbReferenceType'));
        var gpId = intOf(val('CmbGatePassNo'));

        if (text === 'GatePass OutWard') {
            FieldsDisables();
            return get('/gate-pass', { kind: 'outward', id: gpId }).then(function (d) {
                var r = d.row; if (!r) return;
                setVal('txtBilityNo', ci(r, 'BiltyNo'));
                setVal('txtVehicleNo', ci(r, 'VehicleNo'));
                setSelectByText('cmbVehicleType', ci(r, 'VehicleType'));
                setVal('txtItemqty', ci(r, 'NoOfPackages'));
                setVal('CmbWeighBridgeType', str(ci(r, 'GatepassType')));
                var orderType = str(ci(r, 'OtherSupCust')), orderId = intOf(ci(r, 'SaleOrderId'));
                var active = hasActiveRow('CmbGatePassNo') && gpId > 0, gpType = refDocGatepassType();
                var step;
                if (orderType === 'DeliverOrder') {
                    step = (active && gpType === 'Export') ? getInvoiceNo(gpId) : Promise.resolve();
                    if (cfg.weighBridgeForOutwardPartyAndItemWise && active && gpType !== 'Export') {
                        step = step.then(function () {
                            return get('/delivery-order-parties-items', { gpId: gpId, recId: RECID }, shared).then(function (rows) {
                                dtPartiesAndItemsFromDeliveryOrder = rows || [];
                                PartyNameBindforOutward();
                                bindText('item', 'dlItem', [], {}); setVal('CmbItem', '');
                                CmbPartyName_Leave();
                            });
                        });
                    } else {
                        step = step.then(function () { return itemsFrom('/delivery-order-items', { deliveryOrderId: orderId, gpId: 0 }); })
                                   .then(function () { setVal('CmbItem', ci(r, 'VarietyName')); });
                    }
                } else if (orderType === 'SaleOrder' || orderType === 'SaleOrderStoreAndPm') {
                    step = itemsFrom('/sale-order-items', { saleOrderId: orderId });
                } else {
                    PartyNameBind(); getAllitems(); step = Promise.resolve();
                }
                return step.then(function () {
                    setVal('CmbItem', orderType !== 'DeliverOrder' ? str(ci(r, 'VarietyName')) : '');
                    setVal('CmbPartyName', ci(r, 'CompanyName'));
                    enable('CmbItem', true);
                    enable('CmbPartyName', !!cfg.weighBridgeForOutwardPartyAndItemWise);
                    if (cfg.weighBridgeForOutwardPartyAndItemWise && orderType === 'DeliverOrder' && textValue('party', 'CmbPartyName') > 0) {
                        ItemsAgainstPartyForOutward(textValue('party', 'CmbPartyName'));
                    }
                });
            });
        }
        if (text === 'GatePass OutWard Steel') {
            FieldsDisables();
            return get('/gate-pass', { kind: 'steel', id: gpId }).then(function (d) {
                var r = d.row; if (!r) return;
                setVal('txtBilityNo', ci(r, 'BiltyNo'));
                setVal('txtVehicleNo', ci(r, 'VehicleNo'));
                setSelectByText('cmbVehicleType', ci(r, 'VehicleType'));
                setVal('txtItemqty', ci(r, 'NoOfPackages'));
                setVal('CmbItem', ci(r, 'VarietyName'));
                setVal('CmbWeighBridgeType', str(ci(r, 'GatepassType')));
                PartyNameBind(); getAllitems();
                enable('CmbItem', false);
            });
        }
        if (text === 'GatePass Inward') {
            ['label6', 'label16', 'label41'].forEach(function (id) { $id(id).classList.remove('invisible-keep'); });
            PartyNameBind(); getAllitems();
            FieldsDisables();
            if (v === 1601) {
                return get('/gate-pass', { kind: 'inward', documentTypeId: 1601, id: gpId }).then(function (d) {
                    var r = d.row; if (!r) return;
                    setVal('txtBilityNo', ci(r, 'BiltyNo'));
                    setVal('txtVehicleNo', ci(r, 'VehicleNo'));
                    setSelectByText('cmbVehicleType', ci(r, 'VehicleType'));
                    setVal('txtItemqty', ci(r, 'ItemQty'));
                    if (!setTextById('item', 'CmbItem', ci(r, 'ItemId'))) setVal('CmbItem', '');
                    RefDocumentTypeId = intOf(ci(r, 'RefDocumentTypeId'));
                    setVal('CmbWeighBridgeType', str(ci(r, 'GatepassType')));
                });
            }
            return get('/gate-pass', { kind: 'inward', documentTypeId: 51, id: gpId }).then(function (d) {
                var r = d.row; if (!r) return;
                setVal('txtBilityNo', ci(r, 'BiltyNo'));
                setVal('txtVehicleNo', ci(r, 'VehicleNo'));
                setSelectByText('cmbVehicleType', ci(r, 'VehicleType'));
                setVal('txtItemqty', ci(r, 'NoOfPackages'));
                if (intOf(ci(r, 'ItemId')) === 0) setVal('CmbItem', str(ci(r, 'VarietyName')));
                else if (!setTextById('item', 'CmbItem', ci(r, 'ItemId'))) setVal('CmbItem', '');
                setVal('CmbPartyName', ci(r, 'CompanyName'));
                RefDocumentTypeId = intOf(ci(r, 'RefDocumentTypeId'));
                setVal('CmbWeighBridgeType', str(ci(r, 'GatepassType')));
                selectValue('cmbPackingType', intOf(ci(r, 'PackingTypeId')));
                if (d.isGatePassEntryUser) {
                    setVal('txtLoadWeight', str(ci(r, 'SupplierFirstWeight')));
                    setVal('txtTearWeight', str(ci(r, 'SupplierSecondWeight')));
                    setVal('txtSupplierWbNetWeight', str(ci(r, 'SupplierWeight')));
                } else {
                    setVal('txtLoadWeight', '0'); setVal('txtTearWeight', '0'); setVal('txtSupplierWbNetWeight', '0');
                }
                CalculateSupplierNetWeight();
                enable('CmbPartyName', false);
                if ((cfg.labCompulsoryBeforFirstWeight && (RefDocumentTypeId === 105 || RefDocumentTypeId === 106 || RefDocumentTypeId === 110))
                        || RefDocumentTypeId === 41) return labItems(1);
            });
        }
        if (v === 52 || v === 92) {
            getAllitems(); FieldsDisables();
            return get('/gate-pass', { kind: 'general', documentTypeId: v, gpSrNo: intOf(selectedText('CmbGatePassNo').trim()) }).then(function (d) {
                var r = d.row; if (!r) return;
                setVal('txtBilityNo', ci(r, 'BiltyNo'));
                setVal('txtVehicleNo', ci(r, 'VehicleNo'));
                setSelectByText('cmbVehicleType', ci(r, 'VehicleType'));
                setVal('txtRemarks', ci(r, 'OtherRemarks'));
                setVal('txtItemqty', ci(r, 'NoOfPackages'));
                setVal('CmbItem', ci(r, 'VarietyName'));
            });
        }
        if (v === 54 || v === 55) {
            getAllitems(); FieldsDisables();
            return get('/gate-pass', { kind: 'party', documentTypeId: v, id: gpId }).then(function (d) {
                var r = d.row; if (!r) return;
                setVal('txtBilityNo', ci(r, 'BiltyNo'));
                setVal('txtVehicleNo', ci(r, 'VehicleNo'));
                setSelectByText('cmbVehicleType', ci(r, 'VehicleType'));
                setVal('txtRemarks', ci(r, 'OtherRemarks'));
                setVal('txtItemqty', ci(r, 'ItemQty'));
                setVal('CmbItem', ci(r, 'VarietyName'));
                setVal('CmbPartyName', ci(r, 'CompanyName'));
                setVal('CmbWeighBridgeType', str(ci(r, 'GatepassType')));
            });
        }
        if (text === '') return Promise.reject(new Error('Please Select Gate pass Type'));
        return Promise.resolve();
    }

    // ============================================================================ calculations

    /** CalculateNetWeight():2872 — only once both weights hold something. */
    function CalculateNetWeight() {
        var a = val('txtFirstWeight'), b = val('txtsecondWeight');
        if (a === '' || b === '') return;
        var x = parseFloat(a), y = parseFloat(b);
        if (isNaN(x) || isNaN(y)) { box('Input string was not in a correct format.'); return; }
        if (x > y) setVal('txtNetWeight', dstr(x - y));
        if (y > x) setVal('txtNetWeight', dstr(y - x));
        if (x === y) setVal('txtNetWeight', '0');
    }
    /** CalculateSupplierNetWeight():3345. */
    function CalculateSupplierNetWeight() {
        var a = val('txtLoadWeight'), b = val('txtTearWeight');
        if (a !== '' && b !== '') {
            var x = parseFloat(a), y = parseFloat(b);
            if (isNaN(x) || isNaN(y)) { box('Input string was not in a correct format.'); return; }
            setVal('txtSupplierWbNetWeight', dstr(Math.abs(x - y)));
        } else {
            setVal('txtSupplierWbNetWeight', '0');
        }
    }

    // ================================================================================== refresh

    /** formRefresh():2086. Note what it does NOT do, as on the desktop: the reference type, the
     *  packing type, the vehicle type, Working Report No and Wt Diff Remarks keep their values,
     *  Bility / Vehicle / Vehicle Type stay disabled if a gate pass had disabled them, and the
     *  gate-pass list is reloaded with OUTWARD gate passes whatever type is chosen. */
    function formRefresh() {
        RECID = 0; Approved = false;
        ['txtsecondWeight', 'txtBilityNo', 'CmbWeighBridgeType', 'txtFirstWeight', 'CmbItem', 'txtItemqty', 'txtRemarks',
         'txtVehicleNo', 'txtWbCharges', 'txtNetWeight'].forEach(function (id) { setVal(id, ''); });
        clearInvoices();
        refDocRows = [];
        var p = Promise.all([
            get('/ticket-no').then(function (d) { setVal('txtTickectNo', d.ticketNo > 0 ? d.ticketNo : ''); }),
            get('/ref-docs', { mode: 'outward', recId: 0 }).then(function (rows) {
                refDocRows = rows || [];
                $id('CmbGatePassNo').setAttribute('data-dtcombo', 'wbmGatePass');
                bindSelect('CmbGatePassNo', refDocRows, 'Id', 'GpSrNo', { attrs: [['vehicle', 'VehicleNo'], ['gptype', 'GatepassType']] });
            }),
            loadPending()
        ]).catch(function (e) { box(e.message); });
        PartyNameBind();
        getAllitems();
        stampTimes();
        show('btnsave', true); show('btnUpdate', false);
        focusEl('cmbReferenceType');
        ['label16', 'label6', 'label41'].forEach(function (id) { $id(id).classList.add('invisible-keep'); });
        setVal('txtLoadWeight', ''); setVal('txtTearWeight', ''); setVal('txtSupplierWbNetWeight', '');
        enable('txtsecondWeight', true);
        savedVehicleMode(false);
        show('lblMultiItem', false); $id('ChkMultiItem').checked = false; enable('ChkMultiItem', true);
        enable('CmbPartyName', true);
        enable('CmbGatePassNo', true);
        return p;
    }

    /** btnRefresh_Click:2179. */
    function refresh() {
        get('/refresh-lists').then(function (d) {
            cfg.lstIsSavedVehicle = !!d.lstIsSavedVehicle;
            cfg.weighBridgeForOutwardPartyAndItemWise = !!d.weighBridgeForOutwardPartyAndItemWise;
            var prev = val('cmbPackingType');
            bindSelect('cmbPackingType', d.packingTypes, 'Id', 'type');
            if (intOf(prev) > 0) selectValue('cmbPackingType', prev);
            L.parties = d.parties; L.items = d.items;
            if (cfg.weighBridgeForOutwardPartyAndItemWise && selectedText('cmbReferenceType') === 'GatePass OutWard') {
                PartyNameBindforOutward();
                var id = textValue('party', 'CmbPartyName');
                if (id > 0) ItemsAgainstPartyForOutward(id);
            } else {
                PartyNameBind(); getAllitems();
            }
        }).catch(function (e) { box(e.message); });
    }

    // ================================================================================ payload

    function payload() {
        var itemRow = textRow('item', 'CmbItem');
        return {
            id: RECID,
            ticketNo: intOf(val('txtTickectNo')),
            biltyNo: val('txtBilityNo'),
            supplierCustomerId: textValue('party', 'CmbPartyName'),
            partyName: val('CmbPartyName'),
            partySelected: !!textRow('party', 'CmbPartyName'),
            wbCharges: val('txtWbCharges'),
            vehicleNo: val('txtVehicleNo'),
            savedVehicleNo: selectedText('cmbVehicleNo'),
            vehicleTypeValue: val('cmbVehicleType'),
            weighBridgeType: val('CmbWeighBridgeType'),
            weighBridgeTypeSelected: !!textRow('wbType', 'CmbWeighBridgeType'),
            packingType: selectedText('cmbPackingType'),
            packingTypeId: intOf(val('cmbPackingType')),
            packingTypeSelected: hasActiveRow('cmbPackingType'),
            itemQty: val('txtItemqty'),
            itemId: itemRow ? intOf(ci(itemRow, 'Id')) : 0,
            itemDescription: val('CmbItem'),
            itemSelected: !!itemRow,
            itemPartyId: intOf(ci(itemRow, 'PartyId')),
            remarks: val('txtRemarks'),
            firstWeight: val('txtFirstWeight'),
            secondWeight: val('txtsecondWeight'),
            netWeight: val('txtNetWeight'),
            firstDateTime: isoLocal(firstDate),
            secondDateTime: isoLocal(secondDate),
            refTypeId: intOf(val('cmbReferenceType')),
            refTypeText: selectedText('cmbReferenceType'),
            refTypeSelected: hasActiveRow('cmbReferenceType'),
            refDocId: intOf(val('CmbGatePassNo')),
            refDocSelected: hasActiveRow('CmbGatePassNo'),
            refDocGatepassType: refDocGatepassType(),
            invoiceId: intOf(val('CmbInvoiceNo')),
            invoiceSelected: hasActiveRow('CmbInvoiceNo'),
            workingReportNo: val('txtWorkingReportNo'),
            weightDiffComments: val('txtWeightDiffComments'),
            multiItem: $id('ChkMultiItem').checked,
            loadWeight: val('txtLoadWeight'),
            tearWeight: val('txtTearWeight'),
            supplierNetWeight: val('txtSupplierWbNetWeight')
        };
    }

    /** FormValidation():1612 — the page's copy, for focus; the server repeats it. */
    function FormValidation(p, update) {
        function no(m, id) { box(m); focusEl(id); return false; }
        var ref = p.refTypeId, gpType = p.refDocGatepassType;
        if (!p.refTypeSelected) return no('GatePassType No Field is Required', 'cmbReferenceType');
        if (!p.refDocSelected) return no('GatePass No Field is Required', 'CmbGatePassNo');
        if (!p.weighBridgeTypeSelected) return no('WeighBridge Type Field is Required', 'CmbWeighBridgeType');
        if (ref === 91 && p.refDocSelected && p.refDocId > 0 && gpType === 'Export' && (!p.invoiceSelected || p.invoiceId === 0)) return no('Invoice number Field is Required', 'CmbInvoiceNo');
        if ((ref === 74 || ref === 75) && (!p.packingTypeSelected || p.packingTypeId === 0)) return no('PackingType Field is Required', 'cmbPackingType');
        if (ref === 51 && (p.loadWeight === '' || num(p.loadWeight) === 0)) return no('Load Weight Field is Required', 'txtLoadWeight');
        if (str(p.firstWeight).trim() === '' || num(p.firstWeight) === 0) return no('First Weight Field is Required', 'txtFirstWeight');
        if (update) {
            if (str(p.secondWeight).trim() === '' || num(p.secondWeight) === 0) return no('Second Weight Field is Required', 'txtsecondWeight');
            if (ref === 51) {
                if (str(p.tearWeight).trim() === '' || num(p.tearWeight) === 0) return no('Tare Weight Field is Required', 'txtTearWeight');
                if (str(p.supplierNetWeight).trim() === '' || num(p.supplierNetWeight) === 0) return no('Supplier Net Weight Field is Required', 'txtSupplierWbNetWeight');
            }
            var w = str(p.workingReportNo).trim();
            if ((ref === 52 || ref === 74 || ref === 92 || ref === 91) && (w === '' || w === '0')) {
                enable('txtWorkingReportNo', true);
                /* 91 needs the delivery order type — the server decides that one. */
                if (ref !== 91) return no('WorkingReportNo Field is Required', 'txtWorkingReportNo');
            }
        }
        if (p.ticketNo === 0) return no('Ticket No Field is Required', 'txtTickectNo');
        if (str(p.vehicleNo).trim() === '') return no('Vehicle Number Field is Required', 'txtVehicleNo');
        var rule = cfg.weighBridgeForOutwardPartyAndItemWise && p.refTypeText === 'GatePass OutWard' && gpType !== 'Export' && gpType !== 'General';
        if (rule && (!p.partySelected || p.supplierCustomerId === 0)) return no('PartyName Field is Required', 'CmbPartyName');
        if (!p.itemSelected || p.itemId === 0) return no('ItemName Field is Required', 'CmbItem');
        if (rule && p.supplierCustomerId !== p.itemPartyId) { setVal('CmbPartyName', ''); return no('PartyName Field is Required', 'CmbPartyName'); }
        return true;
    }

    // ============================================================================= save/update

    /** btnsave_Click:1768. */
    function save() {
        if (!saveActive()) return;
        var p = payload();
        if (!FormValidation(p, false) || !ask('Are you sure to Save?')) return;
        var gpId = p.refDocId;
        $id('btnsave').classList.add('is-busy');
        post('/save', p).then(function (res) {
            $id('btnsave').classList.remove('is-busy');
            if (!res || !res.success) return;
            box(res.message);
            return formRefresh().then(function () {
                if ($id('ChkBox').checked) printSlip(res.id);
                if ($id('chkPrintWithLab').checked) slip257For(gpId);
            });
        }).catch(function (e) { $id('btnsave').classList.remove('is-busy'); box(e.message); });
    }

    /** btnUpdate_Click:1847. */
    function update() {
        if (!updateActive()) return;
        if (Approved) { box('Record Not Update because record has approve'); return; }
        var p = payload();
        if (!FormValidation(p, true)) return;
        if (p.refTypeText === 'GatePass Inward') {
            var net = num(p.netWeight), sup = num(p.supplierNetWeight);
            if (Math.abs(net - sup) > num(cfg.weighbridgeTolerance) && str(p.weightDiffComments).trim() === '') {
                box('Supplier Net Weight in Weigh Bridge does not match the Weighbridge Net Weight.\nSupplier Net Weight: '
                    + dstr(sup) + '\nWeighbridge Net Weight: ' + dstr(net) + '\n\nDifference Weight Remarks are required');
                return;
            }
        }
        if (!ask('Are you sure to Update?')) return;
        var gpId = p.refDocId;
        $id('btnUpdate').classList.add('is-busy');
        post('/update', p).then(function (res) {
            $id('btnUpdate').classList.remove('is-busy');
            if (res && res.success) box(res.message);
            return formRefresh().then(function () {
                if ($id('ChkBox').checked) printSlip(res ? res.id : 0);
                if ($id('chkPrintWithLab').checked) slip257For(gpId);
            });
        }).catch(function (e) {
            $id('btnUpdate').classList.remove('is-busy');
            focusEl('txtWeightDiffComments');
            box(e.message);
        });
    }

    // ================================================================================ read by id

    /** ReadById(ID):1947. */
    function readById(id) {
        return get('/' + id).then(function (d) {
            var r = d.row;
            show('lblMultiItem', true);
            RECID = id;
            enable('txtTickectNo', true);
            tab('tabForm');
            setVal('txtTickectNo', ci(r, 'TicketNo'));
            RefDocumentTypeId = intOf(ci(r, 'RefDocumentTypeId'));
            var reftypeid = intOf(ci(r, 'ReferenceDocTypeId'));
            enable('CmbGatePassNo', false);

            /* Setting the reference type fires its TextChanged (bending); binding the one-row
               gate pass and activating it fires the gate pass's (the fill). Then the ticket's own
               values are written over whatever the fill put in. */
            var oneRow = null, withCols = false;
            if (reftypeid === 91) {
                oneRow = { Id: intOf(ci(r, 'ReferenceDocNoId')), GpSrNo: intOf(ci(r, 'GpSrNo')), VehicleNo: str(ci(r, 'VehicleNo')), GatepassType: str(ci(r, 'GatepassType')) };
                withCols = true;
            } else if ([51, 52, 92, 54, 55, 1507, 1601].indexOf(reftypeid) >= 0) {
                oneRow = { Id: intOf(ci(r, 'ReferenceDocNoId')), GpSrNo: intOf(ci(r, 'GpSrNo')), VehicleNo: intOf(ci(r, 'VehicleNo')) };
            } else if (reftypeid === 74 || reftypeid === 75) {
                oneRow = { Id: val('txtTickectNo'), GpSrNo: val('txtTickectNo') };
            }
            selectValue('cmbReferenceType', reftypeid);
            var chain = cmbReferenceType_Leave().then(function () {
                if (!oneRow) return;
                refDocRows = [oneRow];
                $id('CmbGatePassNo').setAttribute('data-dtcombo', withCols ? 'wbmGatePass' : 'single');
                bindSelect('CmbGatePassNo', refDocRows, 'Id', 'GpSrNo', { attrs: [['vehicle', 'VehicleNo'], ['gptype', 'GatepassType']] });
                activateFirst('CmbGatePassNo');
                enable('CmbGatePassNo', false);
                return BindingAgainstGatePass();
            });
            if (reftypeid === 91) {
                chain = chain.then(function () {
                    setVal('txtWeightDiffComments', str(ci(r, 'WeightDiffComments')));
                    var gp = intOf(val('CmbGatePassNo')), gpType = str(ci(r, 'GatepassType'));
                    var p = (gp > 0 && gpType === 'Export') ? getInvoiceNo(gp) : Promise.resolve();
                    if (cfg.weighBridgeForOutwardPartyAndItemWise && gp > 0 && gpType !== 'Export') {
                        p = p.then(function () {
                            return get('/delivery-order-parties-items', { gpId: gp, recId: RECID }, shared).then(function (rows) {
                                dtPartiesAndItemsFromDeliveryOrder = rows || [];
                                PartyNameBindforOutward();
                                show('lblMultiItem', false);
                                CmbPartyName_Leave();
                            });
                        });
                    }
                    return p;
                });
            }
            return chain.then(function () {
                setVal('txtFirstWeight', str(ci(r, 'FirstWeight')));
                var fd = parseDate(ci(r, 'FirstDateTime'));
                if (fd) { firstDate = fd; setVal('txtFirstDate', timeOf(fd)); }
                setVal('txtsecondWeight', str(ci(r, 'SecondWeight')));
                setVal('txtNetWeight', str(ci(r, 'NetWbWeight')));
                setVal('txtWorkingReportNo', str(ci(r, 'WorkingReportNo')));
                if (val('txtNetWeight') === '') CalculateNetWeight();
                if (val('txtFirstWeight') !== '' && intOf(val('txtsecondWeight')) > 0) {
                    enable('txtBilityNo', false); enable('txtVehicleNo', false); enable('CmbGatePassNo', false);
                } else {
                    enable('txtsecondWeight', true);
                }
                setVal('txtBilityNo', ci(r, 'BiltyNo'));
                setVal('txtVehicleNo', ci(r, 'VehicleNo'));
                setVal('CmbPartyName', ci(r, 'PartyName'));
                selectValue('cmbVehicleType', intOf(ci(r, 'VehicleType')));      // Value = ToInt(VehicleType)
                setVal('CmbWeighBridgeType', str(ci(r, 'WeighBridgeType')));
                setSelectTextKeep('cmbPackingType', str(ci(r, 'PackingType')));
                setVal('txtWbCharges', ci(r, 'WbCharges'));
                setVal('txtLoadWeight', dstr(num(ci(r, 'LoadWeight'))));
                setVal('txtTearWeight', dstr(num(ci(r, 'TearWeight'))));
                setVal('txtSupplierWbNetWeight', dstr(num(ci(r, 'SupplierWbNetWeight'))));
                setVal('txtItemqty', ci(r, 'ItemQty'));
                setVal('txtRemarks', ci(r, 'WbRemarks'));
                Approved = !!d.approved;                                           // IsApproved only
                var setItem = function () { if (!setTextById('item', 'CmbItem', ci(r, 'ItemId'))) setVal('CmbItem', ''); };
                if (reftypeid === 51 && [105, 106, 110, 41].indexOf(RefDocumentTypeId) >= 0) {
                    setVal('CmbItem', '');
                    return labItems(0, setItem);
                }
                setItem();
            }).then(function () {
                focusEl('txtsecondWeight');
                show('btnsave', false); show('btnUpdate', true);
            });
        }).catch(function (e) { box(e.message); });
    }

    // ============================================================================ pending grids

    function loadPending() {
        return Promise.all([
            get('/pending-second').then(function (rows) {
                pendingSecondRows = rows || [];
                renderGrid('grdfill', pendingSecondRows, PENDING2_COLS, {
                    buttons: [{ key: 'Edit', text: 'Edit' }],
                    onButton: function (k, row) { readById(intOf(ci(row, 'Id'))); },
                    onDblClick: function (row) { readById(intOf(ci(row, 'Id'))); },
                    rowStyle: labStyle
                });
            }),
            get('/pending-first').then(function (rows) {
                pendingFirstRows = rows || [];
                renderGrid('grdPendingGpForWeighBridge', pendingFirstRows, PENDING1_COLS, { rowStyle: pending1Style });
            })
        ]).catch(function (e) { box(e.message); });
    }

    function labStyle(col, v) {
        if (col === 'LabStatus') return v === 'Pending' ? 'c-red' : (v === 'Accepted' ? 'c-green' : '');
        if (col === 'LabApprovalStatus') return v === 'Not Approved' ? 'c-red' : (v === 'Approved' ? 'c-green' : '');
        return '';
    }
    function pending1Style(col, v) {
        if (col === 'LabStatus' || col === 'LabApprovalStatus') return labStyle(col, v);
        if (col === 'AccessWeight') return num(v) === 0 ? 'c-green' : (num(v) > 0 ? 'c-red' : '');
        if (col === 'GpApprovalStatus') return v === 'Not Approved' ? 'c-red' : (v === 'Approved' ? 'c-green' : '');
        return '';
    }

    var PENDING2_COLS = [
        { k: 'Id', hidden: true }, { k: 'DocTypeId', hidden: true }, { k: 'ReferenceDocTypeId', hidden: true },
        { k: 'LabStatus' }, { k: 'LabApprovalStatus' }, { k: 'DocDate', t: 'date' },
        { k: 'DocumentType', s: 'DocumentTypeDescription' }, { k: 'TicketNo', t: 'int' }, { k: 'GpSrNo', t: 'int' },
        { k: 'VehicleNo' }, { k: 'BiltyNo' }, { k: 'PartyName' }, { k: 'ItemName', s: 'ItemDescription' },
        { k: 'ItemQty', t: 'num', sum: true }, { k: 'WbCharges', t: 'num' }, { k: 'WbRemarks' },
        { k: 'FirstWeight', t: 'num' }, { k: 'FirstWtWbStatus' }, { k: 'SecondWeight', t: 'num' },
        { k: 'EntryUser', s: 'UserName' }
    ];
    var PENDING1_COLS = [
        { k: 'Id', hidden: true }, { k: 'DocumentTypeId', hidden: true }, { k: 'LabStatus' }, { k: 'LabApprovalStatus' },
        { k: 'DocumentType' }, { k: 'GpDate', t: 'date' }, { k: 'GpSrNo', t: 'int', cap: 'Gp No' }, { k: 'PartyName' },
        { k: 'ItemName' }, { k: 'ItemQty', t: 'num', sum: true }, { k: 'VehicleType' }, { k: 'VehicleNo' },
        { k: 'BiltyNo' }, { k: 'Remarks' }, { k: 'GpStatus' }, { k: 'EntryUser', s: 'UserName', hidden: true },
        { k: 'GpApprovalStatus' }, { k: 'AccessWeight', t: 'num', sum: true }
    ];
    /* bindGrid():2595 — no packing columns on this form; CommonServices.GridWrappingAndColumnSettings
       sums the double columns. */
    var HISTORY_COLS = [
        { k: 'RecordNo', t: 'int' }, { k: 'Id', hidden: true }, { k: 'DocDate', t: 'date' },
        { k: 'DocumentType', s: 'DocumentTypeDescription' }, { k: 'WeighBridgeType' },
        { k: 'ReferenceDocNoId', hidden: true }, { k: 'GpSrNo', t: 'int' }, { k: 'TicketNo', t: 'int' },
        { k: 'PartyName' }, { k: 'VehicleNo' }, { k: 'BiltyNo' }, { k: 'ItemDescription' },
        { k: 'ReferenceDocTypeId', hidden: true }, { k: 'ItemQty', t: 'num', sum: true, d: 2 },
        { k: 'WbCharges', t: 'num', sum: true, d: 2 }, { k: 'WorkingReportNo' },
        { k: 'FirstWeight', t: 'num', sum: true, d: 2 }, { k: 'FirstWtWbStatus' },
        { k: 'SecondWeight', t: 'num', sum: true, d: 2 }, { k: 'SecondWtWbStatus' },
        { k: 'NetWbWeight', t: 'num', sum: true, d: 2 }, { k: 'WbRemarks' },
        { k: 'EntryDate', t: 'date' }, { k: 'EntryUser', s: 'UserName' }
    ];

    function renderGrid(hostId, rows, cols, opts) {
        opts = opts || {};
        var host = $id(hostId);
        if (!rows.length) { host.innerHTML = ''; return; }
        var visible = cols.filter(function (c) { return !c.hidden; }), buttons = opts.buttons || [], sums = {};
        var h = '<table class="win-grid"><thead><tr>';
        buttons.forEach(function (b) { h += '<th>' + esc(b.text) + '</th>'; });
        visible.forEach(function (c) { h += '<th>' + esc(c.cap || c.k) + '</th>'; });
        h += '</tr></thead><tbody>';
        rows.forEach(function (r, i) {
            h += '<tr data-i="' + i + '">';
            buttons.forEach(function (b) { h += '<td><button type="button" class="grid-btn" data-btn="' + esc(b.key) + '">' + esc(b.text) + '</button></td>'; });
            visible.forEach(function (c) {
                var v = ci(r, c.s || c.k);
                if (c.sum) sums[c.k] = (sums[c.k] || 0) + num(v);
                var shown = c.t === 'num' ? fmt(v, c.d) : (c.t === 'date' ? shortDate(v) : str(v));
                h += '<td class="' + (c.t === 'num' ? 'num' : (c.t === 'int' ? 'int' : '')) + ' ' + (opts.rowStyle ? opts.rowStyle(c.k, str(v)) : '') + '">' + esc(shown) + '</td>';
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
        host.innerHTML = h + '</table>';
        var tbody = host.querySelector('tbody');
        tbody.addEventListener('click', function (e) {
            var tr = e.target.closest('tr'); if (!tr) return;
            host.querySelectorAll('tr.is-current').forEach(function (x) { x.classList.remove('is-current'); });
            tr.classList.add('is-current');
            var b = e.target.closest('[data-btn]');
            if (b && opts.onButton) opts.onButton(b.getAttribute('data-btn'), rows[+tr.getAttribute('data-i')]);
        });
        if (opts.onDblClick) tbody.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tr'); if (tr) opts.onDblClick(rows[+tr.getAttribute('data-i')]);
        });
    }
    function currentRow(hostId, rows) { var tr = $id(hostId).querySelector('tr.is-current'); return tr ? rows[+tr.getAttribute('data-i')] : null; }

    // ================================================================================== history

    function pickDate(chk, id) { return $id(chk).checked ? val(id) : ''; }

    /** bindGrid():2595. */
    function showHistory() {
        var mode = (document.querySelector('input[name=histDate]:checked') || {}).value || 'doc';
        return get('/history', {
            dateMode: mode, fromDate: pickDate('chkFromDate', 'FromDate'), toDate: pickDate('chkToDate', 'ToDate'),
            ticketNoFrom: intOf(val('txtTicketNoFrom')), ticketNoTo: intOf(val('txtTicketNoTo')),
            gpNoFrom: intOf(val('GPNoFrom')), gpNoTo: intOf(val('GpNoTo')), vehicleNo: val('TxtVehicleNumber').trim(),
            refDocumentTypeId: intOf(val('CmbGatePassTypeHistory')), wbTypeId: intOf(val('CmbWbTypeHistory')),
            wbTypeText: selectedText('CmbWbTypeHistory')
        }).then(function (rows) {
            historyRows = rows || [];
            $id('lblHistoryCount').textContent = historyRows.length ? '(' + historyRows.length + ')' : '';
            renderGrid('DataGridHistory', historyRows, HISTORY_COLS, {
                buttons: [{ key: 'Edit', text: 'Edit' }, { key: 'Print', text: 'Print' }, { key: 'PrintWithLab', text: 'PrintWithLab' }],
                onButton: historyButton
            });
        }).catch(function (e) { box(e.message); });
    }

    /** DataGridHistory_ColumnButtonClick:2764 — Edit refreshes the form first. */
    function historyButton(key, row) {
        if (key === 'Edit') { formRefresh().then(function () { readById(intOf(ci(row, 'Id'))); }); }
        if (key === 'Print') printSlip(intOf(ci(row, 'Id')));
        if (key === 'PrintWithLab') {
            if (intOf(ci(row, 'ReferenceDocTypeId')) !== 51 || intOf(ci(row, 'ReferenceDocNoId')) <= 0) { box("This Print Is Only For 'Inward GatePass'"); return; }
            slip257For(intOf(ci(row, 'ReferenceDocNoId')));
        }
    }

    function newHistory() {
        setVal('txtTicketNoFrom', '0'); setVal('txtTicketNoTo', ''); setVal('TxtVehicleNumber', '');
        setVal('GPNoFrom', ''); setVal('GpNoTo', '');
        setVal('FromDate', isoDay(daysAgo(3))); setVal('ToDate', isoDay(new Date()));
    }

    function refreshHistoryLists() {
        get('/refresh-history-lists').then(function (d) {
            bindText('wbType', 'dlWbType', d.wbTypes, {});
            bindSelect('CmbWbTypeHistory', d.wbTypes, 'Id', 'GpTypeDescription');
            var prev = val('cmbReferenceType');
            bindSelect('cmbReferenceType', d.referenceTypes, 'Id', 'Name');
            bindSelect('CmbGatePassTypeHistory', d.referenceTypes, 'Id', 'Name');
            activateFirst('cmbReferenceType');                         // GatePassTypeFill activates row 0
            if (val('cmbReferenceType') !== prev) cmbReferenceType_Leave();
        }).catch(function (e) { box(e.message); });
    }

    // =================================================================================== prints

    /** CommonServices.WbTransactionSlip280(PrintId):8861. */
    function printSlip(id) {
        if (!id) { box('No Record Found For Display'); return; }
        get('/slip/' + id).then(function (rows) {
            if (!rows || !rows.length) { box('No Record Found For Display'); return; }
            openPrint('280 - Weigh Bridge Slip', [{ title: '', rows: rows }]);
        }).catch(function (e) { box(e.message); });
    }
    function print280() { if (rights.canPrint) printSlip(RECID); }

    /** btnSlip257_Click:2983. */
    function slip257() {
        if (intOf(val('cmbReferenceType')) === 51 && selectedText('cmbReferenceType').trim() === 'GatePass Inward') {
            if (intOf(val('CmbGatePassNo')) > 0) { slip257For(intOf(val('CmbGatePassNo'))); return; }
            box('Please Select the GatePass No First...'); return;
        }
        box('Please Select the GatePass Type To Inward...');
    }
    function slip257For(gpId) {
        if (!(gpId > 0)) { box('No Record Found For Display'); return; }
        get('/gate-pass-slip/' + gpId, null, shared).then(function (d) {
            openPrint('257 - Inward Gate Pass With Weigh Bridge And Lab Slip', [
                { title: 'Gate Pass', rows: d.main || [] },
                { title: 'Lab Analysis', rows: d.lab || [], table: true },
                { title: 'Weigh Bridge', rows: d.weighBridge || [], table: true }
            ]);
        }).catch(function (e) { box(e.message); });
    }
    function openPrint(title, sections) {
        var w = window.open('', '_blank');
        if (!w) { box('The browser blocked the print window. Allow pop-ups for this site.'); return; }
        var h = '<!DOCTYPE html><html><head><meta charset="utf-8"><title>' + esc(title) + '</title><style>'
              + 'body{font-family:Verdana,sans-serif;font-size:11px;margin:16px}h1{font-size:15px;margin:0 0 2px}h2{font-size:12px;margin:14px 0 4px;border-bottom:1px solid #000}'
              + 'table{border-collapse:collapse;width:100%}td,th{border:1px solid #444;padding:3px 5px;text-align:left}th{background:#eee}'
              + '.kv td:first-child{width:32%;font-weight:bold;background:#f6f6f6}@media print{button{display:none}}</style></head><body>'
              + '<div style="font-weight:bold;font-size:13px">' + esc(L ? L.companyName : '') + '</div><h1>' + esc(title) + '</h1><button onclick="window.print()">Print</button>';
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
        w.document.open(); w.document.write(h + '</body></html>'); w.document.close();
    }

    // ===================================================================================== misc

    function notPorted(what) { box(what + ' has not been built on the web yet.'); }
    /** MakeShortCutKeys():3120 — this form's own list. */
    function shortcutKeys() {
        var rows = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'],
            ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Ctrl+F1', 'For Lookup'], ['Ctrl+F5', 'For Getting Weight From Indicator'],
            ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'],
            ['Ctrl+ArrowUp', 'For Focus on GatePass Type'], ['Ctrl+Enter', 'For Update Record When Focus On Any Grid '],
            ['Ctrl+Space', "To Call Function's On Button Or Link When Focus On Any Grid "]];
        $id('shortcutTable').innerHTML = '<thead><tr><th>KeyCombination</th><th>Description</th></tr></thead><tbody>'
            + rows.map(function (r) { return '<tr><td>' + esc(r[0]) + '</td><td>' + esc(r[1]) + '</td></tr>'; }).join('') + '</tbody>';
        $id('shortcutModal').classList.add('is-open');
    }
    function closeShortcutKeys() { $id('shortcutModal').classList.remove('is-open'); }

    function tab(id) {
        document.querySelectorAll('#tabs1 .win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
        ['tabForm', 'tabHistory'].forEach(function (p) { $id(p).classList.toggle('is-active', p === id); });
        if (id === 'tabHistory') focusEl('FromDate');
    }
    function tab2(id) {
        document.querySelectorAll('#tabs2 .win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab2') === id); });
        ['tabPending2nd', 'tabPending1st'].forEach(function (p) { $id(p).classList.toggle('is-active', p === id); });
    }
    function historyActive() { return $id('tabHistory').classList.contains('is-active'); }

    function digitsOnly(id, allowDot) {
        var e = $id(id); if (!e) return;
        e.addEventListener('keypress', function (ev) {
            if (ev.ctrlKey || ev.metaKey || ev.key.length !== 1 || /\d/.test(ev.key)) return;
            if (allowDot && ev.key === '.' && e.value.indexOf('.') < 0) return;
            ev.preventDefault();
        });
    }

    function wire() {
        $id('cmbReferenceType').addEventListener('change', function () { cmbReferenceType_Leave(); });
        $id('CmbGatePassNo').addEventListener('change', function () { BindingAgainstGatePass(); });
        $id('cmbVehicleNo').addEventListener('change', cmbVehicleNo_TextChanged);
        $id('CmbPartyName').addEventListener('blur', CmbPartyName_Leave);
        $id('CmbItem').addEventListener('blur', function () { enforceLimit('item', 'CmbItem'); });
        $id('txtFirstWeight').addEventListener('input', CalculateNetWeight);
        $id('txtsecondWeight').addEventListener('input', CalculateNetWeight);
        $id('txtLoadWeight').addEventListener('input', CalculateSupplierNetWeight);
        $id('txtTearWeight').addEventListener('input', CalculateSupplierNetWeight);
        $id('txtVehicleNo').addEventListener('input', function () {
            var e = $id('txtVehicleNo'), p = e.selectionStart; e.value = e.value.toUpperCase(); e.setSelectionRange(p, p);
        });
        ['txtFirstWeight', 'txtsecondWeight', 'txtWbCharges', 'txtWorkingReportNo'].forEach(function (id) { digitsOnly(id, false); });
        ['txtLoadWeight', 'txtTearWeight', 'txtTicketNoFrom', 'txtTicketNoTo', 'GPNoFrom', 'GpNoTo'].forEach(function (id) { digitsOnly(id, true); });
        document.addEventListener('keydown', onKeyDown);
    }

    /** frmWeightbridge_KeyDown:3151 (the manual form's handler keeps the 411 name). Escape also
     *  closes the desktop form; here it is left to the dropdowns, which use it to close. */
    function onKeyDown(e) {
        var k = e.key;
        if (k === 'Enter' && !e.ctrlKey && !e.altKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName)
                && e.target.type !== 'checkbox' && e.target.type !== 'radio') {
            var f = Array.prototype.filter.call(document.querySelectorAll('input,select,button'), function (x) {
                return !x.disabled && x.offsetParent !== null && x.tabIndex >= 0;
            });
            var i = f.indexOf(e.target);
            if (i >= 0 && i < f.length - 1) { e.preventDefault(); f[i + 1].focus(); }
        }
        if (e.ctrlKey && (k === 'e' || k === 'E')) { e.preventDefault(); window.location.href = '/weighbridge'; return; }
        if (e.ctrlKey && (k === 't' || k === 'T')) { e.preventDefault(); tab(historyActive() ? 'tabForm' : 'tabHistory'); }
        if (e.ctrlKey && e.altKey && (k === 'Control' || k === 'Alt')) shortcutKeys();
        if (historyActive()) {
            if (e.ctrlKey && (k === 'p' || k === 'P')) { e.preventDefault(); var hr = currentRow('DataGridHistory', historyRows); if (hr) printSlip(intOf(ci(hr, 'Id'))); }
            if (e.ctrlKey && k === 'Enter' && document.activeElement === $id('DataGridHistory')) {
                var r2 = currentRow('DataGridHistory', historyRows);
                if (r2) formRefresh().then(function () { readById(intOf(ci(r2, 'Id'))); });
            }
            return;
        }
        if (e.ctrlKey && (k === 's' || k === 'S') && saveActive()) { e.preventDefault(); save(); }
        if (e.ctrlKey && (k === 'u' || k === 'U') && updateActive()) { e.preventDefault(); update(); }
        if (e.ctrlKey && (k === 'n' || k === 'N')) { e.preventDefault(); formRefresh(); }
        if (e.ctrlKey && k === 'F5') { e.preventDefault(); focusEl('cmbReferenceType'); }
        if (e.ctrlKey && (k === 'p' || k === 'P') && updateActive()) { e.preventDefault(); print280(); }
        if (e.ctrlKey && k === 'ArrowUp') { e.preventDefault(); focusEl('cmbReferenceType'); }
        if (e.ctrlKey && k === 'ArrowDown') { e.preventDefault(); $id('grdfill').focus(); }
        if (e.ctrlKey && k === 'F1') { e.preventDefault(); window.open('/weighbridge/vehicle-weight-lookup', '_blank'); }
        if (e.ctrlKey && k === 'ArrowRight') {
            e.preventDefault();
            if ($id('tabPending1st').classList.contains('is-active')) { tab2('tabPending2nd'); $id('grdfill').focus(); }
            else { tab2('tabPending1st'); $id('grdPendingGpForWeighBridge').focus(); }
        }
        if (e.ctrlKey && k === 'Enter' && document.activeElement === $id('grdfill')) {
            var pr = currentRow('grdfill', pendingSecondRows); if (pr) readById(intOf(ci(pr, 'Id')));
        }
    }

    window.WeighBridgeManual = {
        newForm: function () { formRefresh(); }, save: save, update: update, refresh: refresh,
        print280: print280, slip257: slip257, notPorted: notPorted, shortcutKeys: shortcutKeys,
        closeShortcutKeys: closeShortcutKeys, tab: tab, tab2: tab2, showHistory: showHistory,
        newHistory: newHistory, refreshHistoryLists: refreshHistoryLists
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init);
    else init();
})();
