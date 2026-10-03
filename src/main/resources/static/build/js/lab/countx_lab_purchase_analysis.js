/* ============================================================================================
 * Screen 160 "Purchase Analysis" — Architecture.WinApp.Lab/InvLabPurchaseAnalysis.cs (DocumentTypeId 303)
 * and its loader dialog PendingGPForPurchaseLab.cs. Line references (:n) are InvLabPurchaseAnalysis.cs
 * unless prefixed "PGP:" (PendingGPForPurchaseLab.cs).
 *
 * The page reproduces the form's events one for one (gate pass / item / rate uom / sample / analysis
 * group Leave, ReadById, refresh, the history grid's cell rules). The server re-validates everything
 * it saves and reads supplier, order no, purchase order and the parameter rows' standards itself; the
 * Doc No shown here is display only.
 *
 * Custom numeric format strings round away from zero (roundAway); "#,#" and "#,#.##" print nothing
 * for zero.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/lab/purchase-analysis';
    var RPT_653 = '653-RptInvLabPurchaseAnalysisSlip.rpt';
    var RPT_257 = '257-InwardGatePassWithWbAndLabSlip.rpt';
    var MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var DEFAULT_TEXT = '-- Select --';                       // InfragisticsHelper.InsertDefaultRow
    var MAX_PIC_BYTES = 5 * 1024 * 1024;                     // DesktopAttachmentStore.MAX_BYTES

    /* Form-specific combo columns (countx_desktop_combo.js). The first column is always the display member. */
    if (window.DesktopCombo) {
        /* GpNoFill (:770-785): Id hidden; GpSrNo, VehicleNo, OrderType, SupplierName, LabType. */
        window.DesktopCombo.define('paGatePass', [
            { caption: 'GpSrNo', flex: 2 },
            { caption: 'VehicleNo', flex: 2, key: 'vehicle' },
            { caption: 'OrderType', flex: 3, key: 'ordertype' },
            { caption: 'SupplierName', flex: 4, key: 'supplier' },
            { caption: 'LabType', flex: 2, key: 'labtype' }
        ]);
        /* cmbitem (:877-881 / :997-999 / :1072-1074): ItemName and Parentcategory stay visible. */
        window.DesktopCombo.define('paItem', [
            { caption: 'ItemName', flex: 4 },
            { caption: 'Parentcategory', flex: 2, key: 'parent' }
        ]);
        /* cmbPartyName over the PO rows (:883-888): SupplierCustomerId and PartyName stay visible. */
        window.DesktopCombo.define('paParty41', [
            { caption: 'PartyName', flex: 4 },
            { caption: 'SupplierCustomerId', flex: 2, key: 'sid' }
        ]);
        /* AnalysisGroup (:964-965): Analysis Group, GroupType (GroupTypeId hidden). */
        window.DesktopCombo.define('paGroup', [
            { caption: 'Analysis Group', flex: 4 },
            { caption: 'GroupType', flex: 2, key: 'grouptype' }
        ]);
        /* WarehouseFill (:744): the whole USP_GetWarehousesAllocatedToBranch row, Id hidden. */
        window.DesktopCombo.define('paWarehouse', [
            { caption: 'Warehouse Name', flex: 4 },
            { caption: 'BranchId', flex: 1, key: 'branchid' },
            { caption: 'BranchName', flex: 3, key: 'branch' },
            { caption: 'WareHouseTypeId', flex: 1, key: 'typeid' },
            { caption: 'WareHouseType', flex: 2, key: 'type' },
            { caption: 'IsActive', flex: 1, key: 'active', type: 'check' },
            { caption: 'PlantId', flex: 1, key: 'plantid' },
            { caption: 'PlantName', flex: 2, key: 'plant' }
        ]);
        /* CmbRateUom from the pricing schedule (:1279): RecordNo hidden; RateUomId (captioned "RateUom"), RateUom. */
        window.DesktopCombo.define('paRateUomSch', [
            { caption: 'RateUom', flex: 3 },
            { caption: 'RateUom', flex: 1, key: 'uid' }
        ]);
        /* CmbRateUom from the global UOM schedule (:1298): UOMCode (captioned "RateUom"), Equivalent, BaseRateUom, BasePackUom. */
        window.DesktopCombo.define('paRateUomGlob', [
            { caption: 'RateUom', flex: 3 },
            { caption: 'Equivalent', flex: 2, key: 'eq', type: 'num' },
            { caption: 'BaseRateUom', flex: 2, key: 'baserate', type: 'check' },
            { caption: 'BasePackUom', flex: 2, key: 'basepack', type: 'check' }
        ]);
    }

    var L = null;                                            // lookups
    var st = {
        recId: 0,                                            // RecId
        analysisGroupId: 0,                                  // AnalysisGroupId (:102)
        ref: 0,                                              // RefDocumentTypeId (:60)
        dtgpsr: [],                                          // dtgpsr — the gate pass rows
        detaillst: [], grdSel: -1, subOf: -1,                // detaillst, the grid's current row, the row grdSubItems shows
        sample: { details: [], subParams: [] },              // sampledetailist / lstDetailSub
        pic: { analysis: newPic(), cooking: newPic() },
        hist: [], histSel: -1, histDetailId: 0, histDetail: [], histDetailOrig: [], histSample: [],
        loader: [],
        grdFocus: false
    };
    var flt = { grdhistory: {}, grdSubItems: {}, grdHistroryPurAnalysis: {}, grdHistrorySamAnalysis: {} };

    function newPic() { return { action: 'keep', fileName: '', dataBase64: '' }; }

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
    /** Conversion.ToInt — an integer literal or 0. */
    function intOf(v) {
        var s = String(v === undefined || v === null ? '' : v).trim();
        return /^[+-]?\d+$/.test(s) ? parseInt(s, 10) : 0;
    }
    function roundAway(x, n) {
        var f = Math.pow(10, n || 0);
        return Math.sign(x) * Math.round(Math.abs(x) * f + 1e-9) / f;
    }
    /** double.ToString() */
    function netStr(v) {
        if (!isFinite(v)) return '';
        return String(parseFloat(Number(v).toPrecision(15)));
    }
    function group(x, minD, maxD) { return x.toLocaleString('en-US', { minimumFractionDigits: minD, maximumFractionDigits: maxD }); }
    /** "#,##0.##" / "#,##0.###" */
    function fmtHash(v, n) { return group(roundAway(num(v), n), 0, n); }
    /** "#,#" / "#,#.##" — zero prints nothing. */
    function fmtOpt(v, n) { var x = roundAway(num(v), n); return x === 0 ? '' : group(x, 0, n); }
    function pad(n) { return String(n).padStart(2, '0'); }
    function isoOf(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function today() { return isoOf(new Date()); }
    function isoDay(v) { var m = /^(\d{4}-\d{2}-\d{2})/.exec(String(v || '')); return m ? m[1] : ''; }
    /** "dd-MMM-yyyy" */
    function dMMMyyyy(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || ''));
        return m ? m[3] + '-' + MONTHS[+m[2] - 1] + '-' + m[1] : '';
    }
    function hhmmtt(v) {
        var m = /[T ](\d{2}):(\d{2})/.exec(String(v || ''));
        if (!m) return '';
        var h = +m[1], ap = h >= 12 ? 'PM' : 'AM';
        h = h % 12; if (h === 0) h = 12;
        return pad(h) + ':' + m[2] + ' ' + ap;
    }
    /** "dd-MMM-yyyy hh:mm tt" */
    function dMMMyyyyTime(v) { var d = dMMMyyyy(v); return d ? (d + ' ' + hhmmtt(v)).trim() : ''; }
    function msg(e) { return e && e.message ? e.message : String(e); }
    function rights() { return (L && L.rights) || {}; }
    function show(el, on) { if (typeof el === 'string') el = $(el); if (el) el.hidden = !on; }
    function visible(id) { return !$(id).hidden; }

    async function apiRaw(method, url, body) {
        var opt = { method: method, credentials: 'same-origin', headers: { Accept: 'application/json' } };
        if (body !== undefined) { opt.headers['Content-Type'] = 'application/json'; opt.body = JSON.stringify(body); }
        var res = await fetch(url, opt);
        var text = await res.text();
        var data = null;
        try { data = text ? JSON.parse(text) : null; } catch (e) { data = null; }
        return { ok: res.ok, status: res.status, data: data };
    }
    async function api(method, url, body) {
        var r = await apiRaw(method, url, body);
        if (!r.ok) throw new Error(r.data && (r.data.message || r.data.error) || ('The request failed (' + r.status + ').'));
        return r.data;
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
    /** DDL.BindDDL / BindAndRetainSelection — replaces the list; the caller decides the selection. */
    function fill(id, rows, valueKey, textKey, withDefault, attrFn) {
        var el = $(id), html = withDefault ? opt('0', DEFAULT_TEXT) : '';
        (rows || []).forEach(function (r) { html += opt(r[valueKey], r[textKey], attrFn ? attrFn(r) : null); });
        el.innerHTML = html;
        el.selectedIndex = -1;
    }
    function has(id, v) { return Array.prototype.some.call($(id).options, function (o) { return o.value === String(v); }); }
    /** combo.Value = v — a value not in the list leaves the combo without a selection. */
    function setVal(id, v) {
        var el = $(id);
        if (v !== null && v !== undefined && has(id, v)) el.value = String(v); else el.selectedIndex = -1;
    }
    function comboVal(id) { var el = $(id); return el.selectedIndex < 0 ? 0 : intOf(el.value); }
    /** combo.Text — exactly what the box shows (the default row included, as :1878 saves it). */
    function comboText(id) { var el = $(id); return el.selectedIndex < 0 ? '' : el.options[el.selectedIndex].textContent; }
    function active(id) { return $(id).selectedIndex >= 0; }                          // ActiveRow != null
    function dropAdhoc(id) { Array.prototype.slice.call($(id).querySelectorAll('option[data-adhoc]')).forEach(function (o) { o.remove(); }); }
    function clearCombo(id) { dropAdhoc(id); $(id).selectedIndex = -1; }              // combo.Text = string.Empty
    function unbind(id) { $(id).innerHTML = ''; $(id).selectedIndex = -1; }           // Text = "" ; DataSource = null
    /**
     * combo.Text = text — selects the row whose display text matches; a text that matches no row stays in the
     * box on the desktop, so it is kept here as an ad-hoc entry (the PO line's Crop, :1248).
     */
    function setText(id, text) {
        var el = $(id), t = String(text === undefined || text === null ? '' : text);
        dropAdhoc(id);
        if (t === '') { el.selectedIndex = -1; return; }
        for (var i = 0; i < el.options.length; i++) {
            if (el.options[i].textContent.trim().toLowerCase() === t.trim().toLowerCase()) { el.selectedIndex = i; return; }
        }
        el.insertAdjacentHTML('beforeend', '<option value="' + esc(t) + '" data-adhoc="1">' + esc(t) + '</option>');
        el.selectedIndex = el.options.length - 1;
    }
    function selectedAttr(id, attr) {
        var el = $(id);
        if (el.selectedIndex < 0) return '';
        return el.options[el.selectedIndex].getAttribute('data-' + attr) || '';
    }
    function family(id, name, caption) {
        var el = $(id);
        el.setAttribute('data-dtcombo', name);
        if (caption) el.setAttribute('data-dtcombo-caption', caption);
    }
    function focusCtl(id) {
        var el = $(id);
        if (!el) return;
        if (el.__dtcombo && el.__dtcombo.input) el.__dtcombo.input.focus(); else if (!el.disabled) el.focus();
    }

    /** GpNoFill (:753) — no ZeroIndex row, nothing activated. */
    function bindGatePasses() {
        family('cmbGatePassNo', 'paGatePass', 'GpSrNo');
        fill('cmbGatePassNo', L.gatePasses, 'Id', 'GpSrNo', false, function (r) {
            return { vehicle: r.VehicleNo, ordertype: r.OrderType, supplier: r.SupplierName, labtype: r.LabType };
        });
    }
    /**
     * InfragisticsHelper.BindAndRetainSelection(combo, dt, "Id", "Description", caption, AllColumns, previous,
     * insertDefaultRow: true) — no rows: Text "" and no list; otherwise the "-- Select --" row (value 0) is
     * inserted and the previous value is kept when the list has it, else row 0 (the default row) is activated.
     */
    function bindRetain(id, rows, previous) {
        if (!rows || !rows.length) { unbind(id); return; }
        fill(id, rows, 'Id', 'Name', true);
        var inSource = rows.some(function (r) { return String(r.Id) === String(previous); });
        if (inSource) setVal(id, previous); else $(id).selectedIndex = 0;
    }
    function bindCrop() { bindRetain('cmbCropYear', L.cropYears, comboVal('cmbCropYear')); }                 // :1008
    /* :1026 passes Conversion.ToInt(cmbCropYear.Value) as the packing type's previous value — the desktop's own slip. */
    function bindPacking() { bindRetain('CmbPackingType', L.packingTypes, comboVal('cmbCropYear')); }        // :1021
    function bindJobLot() { bindRetain('CmbJobLot', L.jobLots, comboVal('CmbJobLot')); }                     // :1034
    /** LabstatusFill (:1083) */
    function bindLabStatus() {
        fill('CmbLabStatus', [{ Id: 1, Name: 'Accepted' }, { Id: 2, Name: 'Rejected' }], 'Id', 'Name', false);
        $('CmbLabStatus').selectedIndex = 0;                                           // Rows[0].Activate()
    }
    /** WarehouseFill (:727) — bound only when the procedure returns rows. */
    function bindWarehouse() {
        if (!L.warehouses || !L.warehouses.length) return;
        var prev = comboVal('CmbWarehouse');
        fill('CmbWarehouse', L.warehouses, 'Id', 'Name', false, function (r) {
            return { branchid: r.BranchId, branch: r.BranchName, typeid: r.WareHouseTypeId, type: r.WareHouseType, active: r.IsActive ? 1 : 0, plantid: r.PlantId, plant: r.PlantName };
        });
        setVal('CmbWarehouse', prev || null);
    }
    /** WeightCutOnFill (:1127) */
    function bindWeightCutOn() {
        fill('CmbWtCuton', L.weightCutOn, 'Id', 'Name', false);
        $('CmbWtCuton').selectedIndex = $('CmbWtCuton').options.length ? 0 : -1;       // Rows[0].Activate()
        wtCutOnLeave();
    }
    /** PestResult (:1101) / PestStatus (:1114) — two columns, no default row, nothing activated. */
    function bindPest() {
        var pr = comboVal('CmbPestResult'), ps = comboVal('CmbPestStatus');
        if (L.pestResults && L.pestResults.length) { fill('CmbPestResult', L.pestResults, 'Id', 'Name', false); setVal('CmbPestResult', pr || null); } else unbind('CmbPestResult');
        if (L.pestStatuses && L.pestStatuses.length) { fill('CmbPestStatus', L.pestStatuses, 'Id', 'Name', false); setVal('CmbPestStatus', ps || null); } else unbind('CmbPestStatus');
    }
    /** :582 / :2251 — CmbPestResult.Rows[0].Activate() */
    function activateFirstPestResult() {
        if ($('CmbPestResult').options.length > 0) { $('CmbPestResult').selectedIndex = 0; pestResultChanged(); }
    }
    /** CmbPestResult_ValueChanged (:3463) */
    function pestResultChanged() {
        var v = comboVal('CmbPestResult');
        if (v > 0) setVal('CmbPestStatus', v === 1 ? 2 : 1); else clearCombo('CmbPestStatus');
    }
    /** GetConfiguration (:681) */
    function getConfiguration() {
        $('txtItemRate').disabled = !!L.rateFromSchedule;
        $('txtItemRate').readOnly = !!L.rateFromSchedule;
    }
    /** GetConfigurationsFromGlobalAndBindValuesInColumns (:696) */
    function configDefaults() {
        if (intOf(L.defaultJobLot) !== 0) setVal('CmbJobLot', L.defaultJobLot);
        if (intOf(L.defaultCropYear) !== 0 && $('cmbCropYear').options.length) setVal('cmbCropYear', L.defaultCropYear);
        if (intOf(L.defaultPackingType) !== 0 && $('CmbPackingType').options.length) setVal('CmbPackingType', L.defaultPackingType);
        if (intOf(L.defaultWarehouse) !== 0 && $('CmbWarehouse').options.length) setVal('CmbWarehouse', L.defaultWarehouse);
    }
    /** HistoryComboFill (:597) — "Id" / "name", no default row, nothing selected. */
    function bindHistoryCombos(h) {
        h = h || {};
        [['CmbCustomerHistory', h.suppliers], ['CmbItemHistory', h.items], ['CmbAnalystStatusHistory', h.analystStatuses],
         ['cmbApprovedStatusHistory', h.approvedStatuses], ['CmbGpNoHistory', h.gpNos], ['CmbVehicleNoHistory', h.vehicles],
         ['CmbPestResultHistory', h.pestResults], ['CmbPestStatusHistory', h.pestStatuses]].forEach(function (p) {
            var prevText = comboText(p[0]);
            fill(p[0], p[1] || [], 'Id', 'Name', false);
            if (prevText) { var el = $(p[0]); for (var i = 0; i < el.options.length; i++) if (el.options[i].textContent === prevText) { el.selectedIndex = i; break; } }
        });
    }

    // ===================================================================== form arithmetic

    /** NetRateCalculation (:3227) */
    function netRateCalc() {
        $('txtNetRate').value = fmtOpt(num($('txtItemRate').value.trim()) + num($('txtAddRate').value.trim()) - num($('txtLessRate').value.trim()), 0);
    }
    /** A programmatic .Text assignment on a box wired to txtLessRate_TextChanged / txtAddRate_TextChanged (:7... designer). */
    function setRate(id, v) { $(id).value = v === undefined || v === null ? '' : String(v); netRateCalc(); }
    /** CmbWtCuton_Leave (:3247) — also the combo's ValueChanged. */
    function wtCutOnLeave() {
        var two = comboVal('CmbWtCuton') === 2;
        $('txtWeightCutUom').disabled = !two;
        if (!two) $('txtWeightCutUom').value = '0';
    }

    // ===================================================================== form events

    function itemAttrs(r) { return { podetail: r.PoDetailId, parent: r.Parentcategory, parentid: r.ParentcategoryId }; }

    /** txtgatepassno_Leave (:823) */
    async function gatePassLeave() {
        st.dtgpsr = [];
        st.ref = 0;
        unbind('cmbitem');
        var id = active('cmbGatePassNo') ? comboVal('cmbGatePassNo') : 0;
        var data = id > 0 ? await api('GET', API + '/gate-pass/' + id) : { rows: [], items: [] };
        var rows = data.rows || [];
        st.dtgpsr = rows;
        if (rows.length > 0) {
            var r0 = rows[0];
            $('txtOrderNo').value = r0.OrderNo;
            $('txtbiltyno').value = r0.BiltyNo;
            $('datBiltyDate').value = isoDay(r0.BiltyDate) || '1900-01-01';           // Conversion.ToDateTime(DBNull)
            $('txtvehicleno').value = r0.VehicleNo;
            $('txtCityArea').value = r0.CityName;
            st.ref = intOf(r0.RefDocumentTypeId);
            $('txtPurchaseType').value = r0.OrderType;
            $('txtGpQty').value = r0.GpQty;
            $('label21').textContent = 'Lab Purchase Analysis (' + r0.OrderType + ')';
            if (st.ref !== 41) {
                family('cmbPartyName', 'single', 'Party Name');
                fill('cmbPartyName', [{ Id: r0.SupplierCustomerId, Name: r0.PartyName }], 'Id', 'Name', false);
                $('cmbPartyName').selectedIndex = 0;
            }
            var items = data.items || [];
            if (st.ref === 41) {
                $('txtPurchaseOrderId').value = r0.PurchaseOrderId;
                if (items.length > 0) {
                    fill('cmbitem', items, 'Id', 'ItemName', false, itemAttrs);
                    $('cmbitem').selectedIndex = 0;
                    family('cmbPartyName', 'paParty41', 'PartyName');
                    fill('cmbPartyName', items, 'SupplierCustomerId', 'PartyName', false, function (r) { return { sid: r.SupplierCustomerId }; });
                    $('cmbPartyName').selectedIndex = 0;
                }
                $('txtMoisture').value = netStr(num(data.moisture));
            } else if (items.length > 0) {                                             // ItemsBind / ItemsBindFromPricingSchedule
                fill('cmbitem', items, 'Id', 'ItemName', false, itemAttrs);
            }
            if (active('cmbitem') && comboVal('cmbitem') > 0) await analysisGroup(intOf(selectedAttr('cmbitem', 'parentid')));
            $('txtItemRate').disabled = !(st.ref === 106 && !L.rateFromSchedule);
            $('txtAddRate').disabled = st.ref !== 106;
            $('txtLessRate').disabled = st.ref !== 106;
            $('CmbRateUom').disabled = st.ref !== 106;
            $('txtRateCut').disabled = st.ref === 106;
        } else {
            $('txtOrderNo').value = '';
            $('txtbiltyno').value = '';
            $('txtvehicleno').value = '';
        }
    }

    /** AnalysisGroup(ParentcategoryId) (:936) */
    async function analysisGroup(parentCategoryId) {
        var rows = await api('GET', API + '/analysis-groups?parentCategoryId=' + intOf(parentCategoryId));
        if (rows && rows.length > 0) {
            var prev = comboVal('cmbanalysisgroup');
            fill('cmbanalysisgroup', rows, 'Id', 'Name', false, function (r) { return { grouptype: r.GroupType }; });
            setVal('cmbanalysisgroup', prev || null);
        } else {
            unbind('cmbanalysisgroup');
        }
    }

    /** cmbitem_Leave (:1226) */
    async function itemLeave() {
        if (active('cmbitem') && comboVal('cmbitem') > 0) {
            if (st.dtgpsr.length > 0) {
                var ref0 = intOf(st.dtgpsr[0].RefDocumentTypeId);
                if (ref0 === 41) {
                    var poDetail = intOf(selectedAttr('cmbitem', 'podetail')), dt = [];
                    for (var i = 0; i < st.dtgpsr.length; i++) {
                        if (intOf(st.dtgpsr[i].PoDetailId) === poDetail) {
                            setText('cmbCropYear', st.dtgpsr[i].Crop);
                            dt.push({ Id: st.dtgpsr[i].InvLabSampleAnalysisHeaderId, Name: st.dtgpsr[i].SampleNo });
                            break;
                        }
                    }
                    if (dt.length > 0) fill('CmbSampleAnaylsis', dt, 'Id', 'Name', false); else unbind('CmbSampleAnaylsis');
                    var m = await api('GET', API + '/moisture?itemId=' + comboVal('cmbitem') + '&purchaseOrderId=' + intOf($('txtPurchaseOrderId').value));
                    $('txtMoisture').value = netStr(num(m.moisture));
                } else if (ref0 === 106 || ref0 === 110) {
                    var ru = await api('GET', API + '/rate-uoms?itemId=' + comboVal('cmbitem'));
                    if (ru.rows && ru.rows.length > 0) {
                        if (ru.fromSchedule) {
                            family('CmbRateUom', 'paRateUomSch', 'RateUom');
                            fill('CmbRateUom', ru.rows, 'Id', 'Name', false, function (r) { return { uid: r.Id }; });
                        } else {
                            family('CmbRateUom', 'paRateUomGlob', 'RateUom');
                            fill('CmbRateUom', ru.rows, 'Id', 'Name', false, function (r) {
                                return { eq: netStr(num(r.Equivalent)), baserate: r.BaseRateUom ? 1 : 0, basepack: r.BasePackUom ? 1 : 0 };
                            });
                        }
                        $('CmbRateUom').selectedIndex = 0;
                        await rateUomLeave();
                    } else {
                        $('txtItemRate').value = '0';
                        $('txtAddRate').value = '0';
                        $('txtLessRate').value = '0';
                        $('txtNetRate').value = '0';
                        unbind('CmbRateUom');
                    }
                }
            }
            await analysisGroup(intOf(selectedAttr('cmbitem', 'parentid')));
        } else {
            $('txtMoisture').value = '0';
            $('txtItemRate').value = '0';
            $('txtAddRate').value = '0';
            $('txtLessRate').value = '0';
            $('txtNetRate').value = '0';
            clearCombo('cmbanalysisgroup');
            unbind('CmbRateUom');
            $('txtScheduleId').value = '0';
        }
    }

    /** CmbRateUom_Leave (:1376) */
    async function rateUomLeave() {
        if (active('CmbRateUom') && active('cmbitem') && L.rateFromSchedule) {
            if (st.dtgpsr.length > 0 && (intOf(st.dtgpsr[0].RefDocumentTypeId) === 106 || intOf(st.dtgpsr[0].RefDocumentTypeId) === 110)) {
                var r = await api('GET', API + '/item-rate?itemId=' + comboVal('cmbitem') + '&rateUomId=' + comboVal('CmbRateUom'));
                if (r.found) {
                    $('txtItemRate').value = r.ItemRate;
                    $('txtNetRate').value = r.ItemRate;
                    $('txtScheduleId').value = String(r.Id);
                    netRateCalc();
                } else {
                    setRate('txtItemRate', '0');
                    $('txtScheduleId').value = '0';
                }
            }
        } else {
            $('txtScheduleId').value = '0';
        }
    }

    /** CmbSampleAnaylsis_Leave (:1335) */
    async function sampleLeave() {
        var r = await api('GET', API + '/sample/' + comboVal('CmbSampleAnaylsis'));
        if (!r || !r.found) return;
        st.sample = { details: r.details || [], subParams: r.subParams || [] };
        renderSample();
        renderSampleSub(st.sample.subParams);
        renderGrd();
    }

    /** cmbanalysisgroup_Leave (:1141) */
    async function groupLeave() {
        var gid = comboVal('cmbanalysisgroup');
        if (st.recId > 0 && gid === st.analysisGroupId) return;
        st.detaillst = [];
        st.analysisGroupId = 0;
        st.grdSel = -1; st.subOf = -1;
        var rows = await api('GET', API + '/parameters?groupId=' + gid);
        if (rows && rows.length > 0) st.detaillst = rows;
        renderGrd();
        renderSub();
    }

    // ============================================================================ grids

    function thW(caption, w, lines) {
        return '<th style="min-width:' + w + 'px;width:' + w + 'px;' + (lines ? 'height:' + (lines * 14) + 'px;' : '') + '">' + esc(caption) + '</th>';
    }
    function clearTable(id) { var t = $(id); t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; if (t.tFoot) t.tFoot.innerHTML = ''; }
    /** Janus filter row (FilterMode Automatic, "contains"). */
    function filterRow(gridId, cols, lead) {
        var f = flt[gridId];
        return '<tr class="flt">' + (lead || '') + cols.map(function (c) {
            return '<td><input type="text" data-f="' + c[0] + '" value="' + esc(f[c[0]] || '') + '"></td>';
        }).join('') + '</tr>';
    }
    function passes(gridId, cols, row, cellFn) {
        var f = flt[gridId];
        return cols.every(function (c) {
            var q = f[c[0]];
            return !q || String(cellFn(c, row)).toLowerCase().indexOf(String(q).toLowerCase()) >= 0;
        });
    }
    function wireFilter(gridId, renderFn) {
        $(gridId).addEventListener('input', function (e) {
            var k = e.target.getAttribute && e.target.getAttribute('data-f');
            if (!k) return;
            flt[gridId][k] = e.target.value;
            var pos = e.target.selectionStart;
            renderFn();
            var again = $(gridId).querySelector('input[data-f="' + k + '"]');
            if (again) { again.focus(); try { again.setSelectionRange(pos, pos); } catch (x) { } }
        });
    }

    /** The first sample row's result when it is the same parameter (grd_FormattingRow :1735-1748 reads rows[0] only). */
    function sampleValueFor(itemsId) {
        var d = st.sample.details;
        if (!d || !d.length) return 0;
        var v = num(d[0].ResultValue);
        return intOf(d[0].InvLabAnalysisItemsId) === intOf(itemsId) && v > 0 ? v : 0;
    }
    /** grd_FormattingRow (:1708): [the result cell is red+bold, the whole row is red]. */
    function grdStyle(r) {
        var res = num(r.InAnalysisResult), sample = sampleValueFor(r.InvLabAnalysisItemsId);
        var cell = res > 0 && res > num(r.MaxValue);
        var row = sample > 0 && res > 0 && res > sample;
        return [cell, row];
    }
    /**
     * grdsetting (:1583): hidden Id, InvLabGroupAnalysisStandardsId, InvLabAnalysisPurchaseHeaderId,
     * InvLabAnalysisItemsId, ParamsMinValue, ParamsMaxValue; grouped by "Analysis Group" (hidden when
     * grouped); editable: Editable After Approval (check), Analysis Result, RemarksDetail; header 3 lines;
     * "#,##0.##"; totals on MinValue, MaxValue, Analysis Result. Columns in model order (model 0619).
     */
    function renderGrd() {
        var t = $('grd'), rows = st.detaillst;
        if (!rows.length) { clearTable('grd'); return; }
        t.tHead.innerHTML = '<tr>' + thW('AnalysisStutusId', 60, 3) + thW('Analysis Parameter', 160, 3) + thW('MinValue', 65, 3) + thW('MaxValue', 65, 3)
            + thW('Editable After Approval', 70, 3) + thW('Analysis Result', 65, 3) + thW('RemarksDetail', 150, 3) + thW('PmLabStatus', 100, 3) + thW('', 26, 3) + '</tr>';
        var html = '', counts = {}, order = [];
        rows.forEach(function (r) {
            counts[r.AnalysisGroupDescription] = (counts[r.AnalysisGroupDescription] || 0) + 1;
            if (order.indexOf(r.AnalysisGroupDescription) < 0) order.push(r.AnalysisGroupDescription);
        });
        order.forEach(function (g) {
            html += '<tr class="grp"><td colspan="9">Analysis Group: ' + esc(g) + ' (' + counts[g] + (counts[g] === 1 ? ' item)' : ' items)') + '</td></tr>';
            rows.forEach(function (r, i) {
                if (r.AnalysisGroupDescription !== g) return;
                var s = grdStyle(r);
                html += '<tr data-i="' + i + '" class="' + (i === st.grdSel ? 'is-sel ' : '') + (s[1] ? 'pa-rowred' : '') + '">'
                    + '<td class="c">' + esc(intOf(r.AnalysisStutusId)) + '</td>'
                    + '<td><a class="lnk" data-link="1">' + esc(r.AnalysisParameterDescription) + '</a></td>'
                    + '<td class="n">' + fmtHash(r.MinValue, 2) + '</td>'
                    + '<td class="n">' + fmtHash(r.MaxValue, 2) + '</td>'
                    + '<td class="c"><input type="checkbox" data-k="IsEditableAfterApproval"' + (r.IsEditableAfterApproval ? ' checked' : '') + '></td>'
                    + '<td class="n' + (s[0] ? ' pa-cellred' : '') + '"><input type="text" class="r" data-k="InAnalysisResult" value="' + esc(fmtHash(r.InAnalysisResult, 2)) + '"' + (s[0] ? ' style="color:red;font-weight:bold;"' : '') + '></td>'
                    + '<td><input type="text" data-k="RemarksDetail" maxlength="250" value="' + esc(r.RemarksDetail === null || r.RemarksDetail === undefined ? '' : r.RemarksDetail) + '"></td>'
                    + '<td>' + esc(r.PmLabStatus === null || r.PmLabStatus === undefined ? '' : r.PmLabStatus) + '</td>'
                    /* grd.AllowDelete = True (designer :4738): the Delete key removes the row; the save sends the rows left. */
                    + '<td class="c"><button type="button" class="gbtn" data-del="1" title="Delete this row (Delete key on the desktop grid)">&times;</button></td></tr>';
            });
        });
        t.tBodies[0].innerHTML = html;
        grdTotals();
    }
    function grdTotals() {
        var a = 0, b = 0, c = 0;
        st.detaillst.forEach(function (r) { a += num(r.MinValue); b += num(r.MaxValue); c += num(r.InAnalysisResult); });
        $('grd').tFoot.innerHTML = '<tr><td></td><td></td><td class="n">' + fmtHash(a, 2) + '</td><td class="n">' + fmtHash(b, 2) + '</td><td></td><td class="n" id="grdTotResult">'
            + fmtHash(c, 2) + '</td><td></td><td></td><td></td></tr>';
    }
    /** After a cell edit: the row's colours and the totals, without rebuilding the editors. */
    function grdRestyle(tr, r) {
        var s = grdStyle(r);
        tr.classList.toggle('pa-rowred', s[1]);
        var inp = tr.querySelector('input[data-k="InAnalysisResult"]');
        if (inp) { inp.style.color = s[0] ? 'red' : ''; inp.style.fontWeight = s[0] ? 'bold' : ''; inp.parentNode.classList.toggle('pa-cellred', s[0]); }
        grdTotals();
    }
    function onGrdChange(e) {
        var tr = e.target.closest('tr[data-i]'), k = e.target.getAttribute('data-k');
        if (!tr || !k) return;
        var r = st.detaillst[+tr.getAttribute('data-i')];
        if (!r) return;
        if (k === 'IsEditableAfterApproval') { r.IsEditableAfterApproval = e.target.checked; return; }
        if (k === 'RemarksDetail') { r.RemarksDetail = e.target.value; return; }
        var txt = e.target.value.replace(/,/g, '').trim();
        if (txt !== '' && !isFinite(Number(txt))) { e.target.value = fmtHash(r.InAnalysisResult, 2); return; }   // not a number: the cell keeps its value
        r.InAnalysisResult = txt === '' ? 0 : Number(txt);
        e.target.value = fmtHash(r.InAnalysisResult, 2);
        grdRestyle(tr, r);
    }
    function onGrdClick(e) {
        var tr = e.target.closest('tr[data-i]');
        if (!tr) return;
        var i = +tr.getAttribute('data-i');
        st.grdSel = i;
        $('grd').querySelectorAll('tbody tr[data-i]').forEach(function (x) { x.classList.toggle('is-sel', +x.getAttribute('data-i') === i); });
        if (e.target.closest('button[data-del]')) { grdDeleteCurrent(); return; }                       // row delete (AllowDelete :4738)
        if (e.target.closest('a[data-link]')) { st.subOf = i; flt.grdSubItems = {}; renderSub(); }     // grd_LinkClicked (:1693)
    }
    /** The grid allows a row to be deleted (designer: grd.AllowDelete = True) — the Delete key on the current row. */
    function grdDeleteCurrent() {
        if (st.grdSel < 0 || st.grdSel >= st.detaillst.length) return;
        st.detaillst.splice(st.grdSel, 1);
        if (st.subOf === st.grdSel) st.subOf = -1; else if (st.subOf > st.grdSel) st.subOf--;
        st.grdSel = -1;
        renderGrd();
        renderSub();
    }

    var SUB_COLS = [['SubParameterName', 'SubParameterName'], ['ResultValue', 'ResultValue']];
    /** GrdSubsetting (:1625): SubParameterName, ResultValue ("#,#.##", total); the ids hidden. */
    function renderSub() {
        var t = $('grdSubItems');
        var row = st.subOf >= 0 ? st.detaillst[st.subOf] : null;
        if (!row) { clearTable('grdSubItems'); return; }
        var subs = row.subs || [];
        t.tHead.innerHTML = '<tr><th style="width:60%;">SubParameterName</th><th>ResultValue</th></tr>' + filterRow('grdSubItems', SUB_COLS);
        var total = 0;
        t.tBodies[0].innerHTML = subs.map(function (s, i) {
            if (!passes('grdSubItems', SUB_COLS, s, function (c, x) { return c[0] === 'ResultValue' ? fmtOpt(x.ResultValue, 2) : x.SubParameterName; })) return '';
            total += num(s.ResultValue);
            return '<tr data-i="' + i + '"><td>' + esc(s.SubParameterName) + '</td><td class="n"><input type="text" class="r" data-k="ResultValue" value="' + esc(fmtOpt(s.ResultValue, 2)) + '"></td></tr>';
        }).join('');
        t.tFoot.innerHTML = '<tr><td></td><td class="n">' + fmtOpt(total, 2) + '</td></tr>';
    }
    function onSubChange(e) {
        var tr = e.target.closest('tr[data-i]');
        if (!tr || e.target.getAttribute('data-k') !== 'ResultValue') return;
        var row = st.detaillst[st.subOf];
        if (!row) return;
        var s = row.subs[+tr.getAttribute('data-i')];
        var txt = e.target.value.replace(/,/g, '').trim();
        if (txt !== '' && !isFinite(Number(txt))) { e.target.value = fmtOpt(s.ResultValue, 2); return; }
        s.ResultValue = txt === '' ? 0 : Number(txt);
        renderSub();
    }

    /** GrdSamplesetting (:1548): widths 110/150/55/55/55/200, "#,##0.###", totals on Min / Max / Result. */
    function renderSample() {
        var t = $('grdSample'), rows = st.sample.details || [];
        if (!rows.length) { clearTable('grdSample'); return; }
        t.tHead.innerHTML = '<tr>' + thW('AnalysisGroupDescription', 110, 3) + thW('AnalysisParameterDescription', 150, 3) + thW('MinValue', 55, 3) + thW('MaxValue', 55, 3)
            + thW('ResultValue', 55, 3) + thW('RemarksDetail', 200, 3) + '</tr>';
        var a = 0, b = 0, c = 0;
        t.tBodies[0].innerHTML = rows.map(function (r) {
            a += num(r.MinValue); b += num(r.MaxValue); c += num(r.ResultValue);
            return '<tr><td>' + esc(r.AnalysisGroupDescription) + '</td><td>' + esc(r.AnalysisParameterDescription) + '</td><td class="n">' + fmtHash(r.MinValue, 3)
                + '</td><td class="n">' + fmtHash(r.MaxValue, 3) + '</td><td class="n">' + fmtHash(r.ResultValue, 3) + '</td><td>' + esc(r.RemarksDetail) + '</td></tr>';
        }).join('');
        t.tFoot.innerHTML = '<tr><td></td><td></td><td class="n">' + fmtHash(a, 3) + '</td><td class="n">' + fmtHash(b, 3) + '</td><td class="n">' + fmtHash(c, 3) + '</td><td></td></tr>';
    }
    /** GrdSampleSubsetting (:1647): grouped by ParentParameterName with group totals; SubParameterName, ResultValue ("#,#.##"). */
    function renderSampleSub(list) {
        var t = $('grdSampleSubItem');
        if (!list || !list.length) { clearTable('grdSampleSubItem'); return; }
        t.tHead.innerHTML = '<tr><th style="width:60%;">SubParameterName</th><th>ResultValue</th></tr>';
        var order = [], html = '', total = 0;
        list.forEach(function (s) { if (order.indexOf(s.ParentParameterName) < 0) order.push(s.ParentParameterName); });
        order.forEach(function (g) {
            var rows = list.filter(function (s) { return s.ParentParameterName === g; }), sub = 0;
            html += '<tr class="grp"><td colspan="2">ParentParameterName: ' + esc(g) + ' (' + rows.length + (rows.length === 1 ? ' item)' : ' items)') + '</td></tr>';
            rows.forEach(function (s) {
                sub += num(s.ResultValue);
                html += '<tr><td>' + esc(s.SubParameterName) + '</td><td class="n">' + fmtOpt(s.ResultValue, 2) + '</td></tr>';
            });
            html += '<tr class="grp"><td></td><td class="n">' + fmtOpt(sub, 2) + '</td></tr>';
            total += sub;
        });
        t.tBodies[0].innerHTML = html;
        t.tFoot.innerHTML = '<tr><td></td><td class="n">' + fmtOpt(total, 2) + '</td></tr>';
    }

    // ========================================================================= pictures

    function picBox(which) { return $(which === 'cooking' ? 'cookingpic' : 'LiveViewPicBox'); }
    function showPic(which, src) { picBox(which).innerHTML = src ? '<img alt="" src="' + esc(src) + '">' : ''; }
    /** TakePhotoButton_Click (:2861) / btnCapturecooking_Click (:2887) — *.jpg, *.jpeg, *.png. */
    function browsePic(which) {
        if (!L.attachmentPathSet) { alert('Attachment FilePath Not Configure Please Check!'); return; }
        $(which === 'cooking' ? 'fileCookingPic' : 'fileAnalysisPic').click();
    }
    function onPicChosen(which, input) {
        var f = input.files && input.files[0];
        input.value = '';
        if (!f) return;
        if (!/\.(jpg|jpeg|png)$/i.test(f.name)) { alert('Only .jpg, .jpeg and .png pictures can be attached'); return; }
        if (f.size <= 0 || f.size > MAX_PIC_BYTES) { alert('Attachment must be between 1 byte and 5 MB'); return; }
        var reader = new FileReader();
        reader.onload = function () {
            var url = String(reader.result), comma = url.indexOf(',');
            st.pic[which] = { action: 'new', fileName: f.name, dataBase64: comma >= 0 ? url.substring(comma + 1) : '' };
            showPic(which, url);
        };
        reader.readAsDataURL(f);
    }
    /** btnResetDriv_Click (:2941) / btnResetCooking_Click (:2984) */
    function resetPic(which) { st.pic[which] = { action: 'clear', fileName: '', dataBase64: '' }; showPic(which, ''); }

    // ============================================================================= save

    var FOCUS = {
        'Please GpNo GatePass No': 'cmbGatePassNo', 'Please Select Analysis Group No': 'cmbanalysisgroup', 'Item Field Required': 'cmbitem',
        'NoofBags Field Required': 'txtNoofBagsInspection', 'Packing Type Field is Required': 'CmbPackingType', 'RateUom Field Required': 'CmbRateUom',
        'ItemRate Field Required': 'txtItemRate', 'NetRate Field Required': 'txtNetRate', 'AnaylstName field required': 'txtAnaylstName',
        'WeightCut / Touch Cannot Be Greater Than 10': 'txtWeightCut', 'WeightCutUom Must Be Between 40 and 105': 'txtWeightCutUom',
        'Qty For Weight Cut / Touch Cannot Be Greater Than GatePass Qty': 'txtQtyForWtCut', 'Warehouse field required': 'CmbWarehouse',
        'Weight Cut On field required': 'CmbWtCuton', 'Status field required': 'CmbLabStatus', 'Pest Result field required': 'CmbPestResult',
        'Pest Status field required': 'CmbPestStatus'
    };
    function valOrNull(id) { return active(id) ? comboVal(id) : null; }

    function collect(recId) {
        return {
            Id: recId, confirm: false,
            DocDate: $('txtdocdate').value,
            GatePassInwardId: valOrNull('cmbGatePassNo'),
            AnalysisGroupId: valOrNull('cmbanalysisgroup'),
            ItemId: valOrNull('cmbitem'),
            PoDetailId: intOf(selectedAttr('cmbitem', 'podetail')),
            NoofBagsInspection: $('txtNoofBagsInspection').value,
            PackingTypeId: valOrNull('CmbPackingType'),
            RateUomId: valOrNull('CmbRateUom'),
            ItemRate: $('txtItemRate').value, AddRate: $('txtAddRate').value, LessRate: $('txtLessRate').value,
            RateCut: $('txtRateCut').value, NetRate: $('txtNetRate').value, ScheduleId: $('txtScheduleId').value,
            AnaylstName: $('txtAnaylstName').value,
            WeightCut: $('txtWeightCut').value, WeightCutUom: $('txtWeightCutUom').value, QtyForWtCut: $('txtQtyForWtCut').value,
            WarehouseId: valOrNull('CmbWarehouse'), WeightCutOnId: valOrNull('CmbWtCuton'),
            LabStatusId: valOrNull('CmbLabStatus'), LabStatusText: comboText('CmbLabStatus'),
            PestResultId: valOrNull('CmbPestResult'), PestStatusId: valOrNull('CmbPestStatus'),
            JobLotId: comboVal('CmbJobLot'), SampleAnalysisId: comboVal('CmbSampleAnaylsis'),
            Crop: comboText('cmbCropYear'),
            PartyLotRefNo: $('PartyLotRefNo').value, Remarks: $('txtremarks').value,
            WtCutWillApply: $('ChkWtCutWillApply').checked, RateCutWillApply: $('ChkRateCutWillApply').checked,
            AnalysisPic: st.pic.analysis, CookingPic: st.pic.cooking,
            rows: st.detaillst.map(function (r) {
                return {
                    InvLabAnalysisItemsId: r.InvLabAnalysisItemsId, InAnalysisResult: num(r.InAnalysisResult),
                    RemarksDetail: r.RemarksDetail === undefined ? null : r.RemarksDetail,
                    subs: (r.subs || []).map(function (s) { return { SubParameterId: s.SubParameterId, ResultValue: num(s.ResultValue) }; })
                };
            })
        };
    }

    /** Insert() (:1799) — btnSave_Click forces RecId 0 (:1968); btnUpdate_Click keeps it (:1977). */
    function insert(button, isUpdate) {
        return act(button, async function () {
            if (!isUpdate) st.recId = 0;
            var body = collect(st.recId);
            var r = await apiRaw('POST', API + '/save', body);
            if (r.status === 409 && r.data && r.data.confirm) {                         // "Are you sure to Save?" / "Are you sure to Update?"
                if (!window.confirm(r.data.message)) return;
                body.confirm = true;
                r = await apiRaw('POST', API + '/save', body);
            }
            if (!r.ok) {
                var m = r.data && (r.data.message || r.data.error) || ('The request failed (' + r.status + ').');
                alert(m);
                if (FOCUS[m]) focusCtl(FOCUS[m]);
                return;
            }
            alert(r.data.message);                                                       // "Save Successfully" / "Update Successfully"
            var savedId = r.data.id, gpId = r.data.gatePassId;
            await refresh();
            if ($('IsPrint').checked) await print653(savedId, false);                    // :1949
            if ($('chkPrintII').checked) print257(gpId);                                 // :1953
        });
    }

    // ============================================================================= read

    /** ReadById (:1989) */
    async function readById(id) {
        await refresh();
        show('btnSave', false);
        show('btnUpdate', true);
        $('cmbGatePassNo').disabled = true;
        st.recId = id;
        var o = await api('GET', API + '/' + id);
        if (!o) return;
        st.detaillst = [];
        selectTab('tabForm');
        $('txtdocno').value = o.DocNo;
        $('txtdocdate').value = isoDay(o.DocDate);
        $('txtbiltyno').value = o.BiltyNo;
        $('datBiltyDate').value = isoDay(o.BiltyDate) || '1900-01-01';
        $('txtCityArea').value = o.CityName;
        $('txtPurchaseType').value = o.PurchaseType;
        $('txtApprovedBy').value = o.UserNameAusr;
        $('txtApprovalStatus').value = o.ApprovalStatus;
        $('txtvehicleno').value = o.VehicleNo;
        $('txtPurchaseOrderId').value = o.PurchaseOrderId;
        $('txtOrderNo').value = o.SupCustCode;
        $('txtNoofBagsInspection').value = o.NoofBagsInspection;
        st.ref = intOf(o.RefDocumentTypeId);
        setVal('CmbWtCuton', o.WeightCutOnId); wtCutOnLeave();
        $('txtWeightCutUom').value = o.WeightCutUom;
        $('ChkRateCutWillApply').checked = !!o.IsRateCutCompulsory;
        $('ChkWtCutWillApply').checked = !!o.IsWeightCutCompulsory;
        $('ChkRateCutWillApply').disabled = true;
        $('ChkWtCutWillApply').disabled = true;
        family('cmbGatePassNo', 'single', 'GpSrNo');
        fill('cmbGatePassNo', [{ Id: o.GatePassInwardId, Name: o.GpSrNo }], 'Id', 'Name', false);
        $('cmbGatePassNo').selectedIndex = 0;
        await gatePassLeave();
        $('PartyLotRefNo').value = o.PartyLotRefNo;
        clearCombo('cmbitem');
        clearCombo('cmbPartyName');
        setVal('cmbitem', o.ItemId);
        setVal('cmbPartyName', o.SupplierCustomerId);
        if (intOf(o.RateUomId) > 0) {
            family('CmbRateUom', 'single', 'RateUom');
            fill('CmbRateUom', [{ Id: o.RateUomId, Name: o.RateUom }], 'Id', 'Name', false);
            $('CmbRateUom').selectedIndex = 0;
        }
        if (st.ref === 106) {
            await itemLeave();
            if (intOf(o.RateUomId) > 0) setVal('CmbRateUom', o.RateUomId);
        } else {
            unbind('CmbRateUom');
        }
        fill('CmbSampleAnaylsis', [{ Id: o.InvLabSampleLogRegisterId, Name: o.LabSampleNo }], 'Id', 'Name', false);
        $('CmbSampleAnaylsis').selectedIndex = 0;
        await sampleLeave();
        setText('CmbLabStatus', o.IsAccepted ? 'Accepted' : 'Rejected');
        if (active('cmbitem') && comboVal('cmbitem') > 0) await analysisGroup(intOf(selectedAttr('cmbitem', 'parentid')));
        setText('cmbCropYear', o.Crop);
        setVal('CmbWarehouse', o.WarehouseId);
        setVal('CmbPackingType', o.PackingTypeId);
        if (intOf(o.JobLotId) > 0) setVal('CmbJobLot', o.JobLotId);
        if (intOf(o.PestResultId) > 0) { setVal('CmbPestResult', o.PestResultId); pestResultChanged(); }
        if (intOf(o.PestStatusId) > 0) setVal('CmbPestStatus', o.PestStatusId);
        setVal('cmbanalysisgroup', o.InvLabAnalysisGroup);
        st.analysisGroupId = intOf(o.InvLabAnalysisGroup);
        $('txtremarks').value = o.RemarksHeader;
        if (st.ref === 106) {
            $('txtAddRate').value = o.Premium;
            $('txtLessRate').value = o.DeductionRate;
            $('txtItemRate').value = o.ItemRate;
        }
        $('txtRateCut').value = o.DeductionRate;
        $('txtWeightCut').value = o.DeductionWeight;
        $('txtQtyForWtCut').value = o.QtyForWtCut;
        $('txtAnaylstName').value = o.AnalystName;
        $('txtScheduleId').value = o.PricingScheduleId;
        netRateCalc();
        st.detaillst = o.rows || [];
        st.grdSel = -1; st.subOf = -1;
        renderGrd();
        renderSub();
        st.pic.analysis = newPic();
        st.pic.cooking = newPic();
        showPic('analysis', o.HasAnalysisPic ? API + '/' + id + '/picture/analysis' : '');
        showPic('cooking', o.HasCookingPic ? API + '/' + id + '/picture/cooking' : '');
    }

    /** refresh() (:2185) — also btnNew_Click (:2180). */
    async function refresh() {
        st.pic.analysis = newPic();
        st.pic.cooking = newPic();
        st.analysisGroupId = 0;
        st.recId = 0;
        $('label21').textContent = 'Lab Purchase Analysis';
        var d = await api('GET', API + '/new');
        Object.keys(d).forEach(function (k) { L[k] = d[k]; });
        if (intOf(L.docNo) > 0) $('txtdocno').value = L.docNo;                          // GenerateCode
        bindGatePasses();                                                                // GpNoFill
        show('btnSave', true);
        show('btnUpdate', false);
        clearCombo('cmbGatePassNo');
        $('PartyLotRefNo').value = '';
        $('txtbiltyno').value = '';
        $('datBiltyDate').value = today();
        $('txtCityArea').value = '';
        $('txtPurchaseType').value = '';
        $('txtApprovedBy').value = '';
        $('txtApprovalStatus').value = '';
        $('txtGpQty').value = '';
        $('txtvehicleno').value = '';
        $('txtOrderNo').value = '';
        clearCombo('CmbSampleAnaylsis');
        clearCombo('cmbitem');
        clearCombo('cmbanalysisgroup');
        $('txtWeightCut').value = '';
        $('txtAddRate').value = '';
        $('txtLessRate').value = '';
        $('txtRateCut').value = '';
        $('txtItemRate').value = '';
        $('txtNetRate').value = '';
        $('txtQtyForWtCut').value = '';
        $('txtPurchaseOrderId').value = '';
        $('txtAnaylstName').value = '';
        clearCombo('cmbCropYear');
        clearCombo('CmbPackingType');
        clearCombo('CmbJobLot');
        clearCombo('CmbWarehouse');
        $('txtremarks').value = '';
        $('txtNoofBagsInspection').value = '';
        showPic('cooking', '');
        showPic('analysis', '');
        $('cmbGatePassNo').disabled = false;
        unbind('CmbRateUom');
        $('txtScheduleId').value = '';
        st.detaillst = [];
        st.dtgpsr = [];
        st.grdSel = -1; st.subOf = -1;
        st.sample = { details: [], subParams: [] };
        clearTable('grd'); clearTable('grdSample'); clearTable('grdSubItems'); clearTable('grdSampleSubItem');
        $('ChkRateCutWillApply').disabled = false;
        $('ChkWtCutWillApply').disabled = false;
        getConfiguration();
        configDefaults();
        activateFirstPestResult();
        focusCtl('cmbGatePassNo');
    }

    /** btnRefresh_Click (:3441) */
    function formRefresh() {
        return act($('btnRefresh'), async function () {
            var d = await api('GET', API + '/refresh');
            Object.keys(d).forEach(function (k) { L[k] = d[k]; });
            bindGatePasses();
            bindCrop();
            bindPacking();
            bindJobLot();
            if (intOf(L.docNo) > 0) $('txtdocno').value = L.docNo;
            bindLabStatus();
            bindWarehouse();
            bindWeightCutOn();
            configDefaults();
        });
    }

    // =========================================================================== prints

    /** LabAnalysisReport653 (:3157) — 653-RptInvLabPurchaseAnalysisSlip.rpt; @Id is the lab record (print contract arg "history"). */
    async function print653(id, toolbar) {
        try {
            if (intOf(id) <= 0) throw new Error('Record Not Found For Display');
            await api('GET', API + '/' + intOf(id) + '/print-check?toolbar=' + (toolbar ? 'true' : 'false'));
            if (!window.printRpt) throw new Error('The print runtime is not loaded on this page.');
            window.printRpt(RPT_653, { history: intOf(id) });
        } catch (e) { alert(msg(e)); }
    }
    /** CommonServices.InwardGatePassWithWbAndLabSlip(GatePassId) (CommonServices.cs:14268) — 257 slip. */
    function print257(gpId) {
        if (intOf(gpId) <= 0) { alert('No Record Found For Display'); return; }
        if (!window.printRpt) { alert('The print runtime is not loaded on this page.'); return; }
        return window.printRpt(RPT_257, { id: intOf(gpId) });
    }

    // ========================================================================== history

    /* [key, caption, kind, width] — historygridfill (:2422-2456) + gridsetting (:2329). Hidden: Id,
       WeightCutBefore, RateCutBefore, AnalysisPicValue, CookingPicValue. */
    var HIST_COLS = [
        ['LabType', 'LabType', '', 80], ['DocNo', 'DocNo', 'i', 60], ['DocDate', 'DocDate', 'd', 80], ['GatePassNo', 'GatePass#', 'i', 80],
        ['SupplierCode', 'SupplierCode', '', 90], ['PartyName', 'PartyName', '', 220], ['BiltyNo', 'BiltyNo', '', 70], ['VehicleNo', 'VehicleNo', '', 100],
        ['CropYear', 'CropYear', '', 80], ['WarehouseName', 'WarehouseName', '', 150], ['AnalysisItem', 'AnalysisItem', '', 250],
        ['WeightCutOn', 'WeightCutOn', '', 100], ['WeightCutCompulsory', 'WeightCutCompulsory', 'b', 100], ['WeightCut', 'WeightCut', 'e', 100],
        ['RateCutCompulsory', 'RateCutCompulsory', 'b', 100], ['RateCut', 'RateCut', 'e', 100], ['AnalystName', 'AnalystName', '', 200],
        ['AnalystStatus', 'AnalystStatus', '', 100], ['ApprovedStatus', 'ApprovedStatus', '', 100], ['PestResult', 'PestResult', '', 130],
        ['PestStatus', 'PestStatus', '', 130], ['EntryUser', 'EntryUser', '', 100], ['EntryDate', 'EntryDate', 't', 150],
        ['ApprovedUser', 'ApprovedUser', '', 100], ['ApprovedDate', 'ApprovedDate', 't', 150], ['AnalysisPic', 'AnalysisPic', 'p', 60],
        ['CookingPic', 'CookingPic', 'p', 60], ['Attachments', 'Attachments', 'l', 60], ['Remarks', 'Remarks', '', 300]
    ];
    function histCell(c, r) {
        var v = r[c[0]];
        if (c[2] === 'd') return dMMMyyyy(v);
        if (c[2] === 't') return dMMMyyyyTime(v);
        if (c[2] === 'b') return v ? 'True' : 'False';
        return v === null || v === undefined ? '' : v;
    }
    /** gridsetting (:2329): Edit / Print / Update buttons first (frozen), header 3 lines, RateCut and WeightCut editable. */
    function renderHistory() {
        var t = $('grdhistory');
        if (!st.hist.length) { clearTable('grdhistory'); return; }
        t.tHead.innerHTML = '<tr>' + thW('Edit', 50, 3) + thW('Print', 50, 3) + thW('Update', 60, 3)
            + HIST_COLS.map(function (c) { return thW(c[1], c[3], 3); }).join('') + '</tr>'
            + filterRow('grdhistory', HIST_COLS, '<td></td><td></td><td></td>');
        t.tBodies[0].innerHTML = st.hist.map(function (r, i) {
            if (!passes('grdhistory', HIST_COLS, r, histCell)) return '';
            return '<tr data-i="' + i + '"' + (i === st.histSel ? ' class="is-sel"' : '') + '>'
                + '<td><button type="button" class="gbtn" data-act="Edit">Edit</button></td>'
                + '<td><button type="button" class="gbtn" data-act="Print">Print</button></td>'
                + '<td><button type="button" class="gbtn" data-act="Update">Update</button></td>'
                + HIST_COLS.map(function (c) {
                    var v = r[c[0]];
                    if (c[2] === 'b') return '<td class="c"><input type="checkbox" disabled' + (v ? ' checked' : '') + '></td>';
                    if (c[2] === 'e') return '<td><input type="text" data-k="' + c[0] + '" value="' + esc(v) + '"></td>';
                    if (c[2] === 'p') return '<td class="c"><a class="lnk" data-pic="' + (c[0] === 'CookingPic' ? 'cooking' : 'analysis') + '">' + esc(v) + '</a></td>';
                    if (c[2] === 'l') return '<td class="c"><a class="lnk" title="The desktop link handler looks for a column named NoOfAttachments and does nothing here (:2556); attachments are not ported">' + esc(v) + '</a></td>';
                    if (c[0] === 'DocNo') return '<td class="c"><a class="lnk doc" data-open="1" title="Open this record in the form">' + esc(histCell(c, r)) + '</a></td>';
                    return '<td' + (c[2] === 'i' ? ' class="c"' : '') + '>' + esc(histCell(c, r)) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        t.tFoot.innerHTML = '<tr><td></td><td></td><td></td>' + HIST_COLS.map(function () { return '<td></td>'; }).join('') + '</tr>';
    }

    /** historygridfill (:2380) */
    async function historyGridFill() {
        var q = '?fromChecked=' + $('chkFromDateHistory').checked + '&fromDate=' + encodeURIComponent($('FromDateHistory').value)
            + '&toChecked=' + $('chkToDateHistory').checked + '&toDate=' + encodeURIComponent($('ToDateHistory').value)
            + '&fromDocNo=' + encodeURIComponent($('FromDocNoHistory').value) + '&toDocNo=' + encodeURIComponent($('ToDocNoHistory').value)
            + '&supplierId=' + comboVal('CmbCustomerHistory') + '&itemId=' + comboVal('CmbItemHistory') + '&gpId=' + comboVal('CmbGpNoHistory')
            + '&approvedStatus=' + encodeURIComponent(comboText('cmbApprovedStatusHistory'))
            + '&analystStatus=' + encodeURIComponent(comboText('CmbAnalystStatusHistory'))
            + '&vehicleNo=' + encodeURIComponent(comboText('CmbVehicleNoHistory'));
        var rows = await api('GET', API + '/history' + q) || [];
        if (rows.length === 0) {
            st.hist = []; st.histSel = -1; st.histDetail = []; st.histDetailOrig = []; st.histDetailId = 0; st.histSample = [];
            alert('Record Not Found');
            clearTable('grdhistory'); clearTable('grdHistroryPurAnalysis'); clearTable('grdHistrorySamAnalysis');
            return;
        }
        st.hist = rows;
        st.histSel = -1;
        flt.grdhistory = {};
        renderHistory();
        await historySelectionChanged(0);                                              // a newly bound grid positions on its first row
    }
    function showHistory() { return act($('btnShowHistory'), historyGridFill); }

    /** btnNewHistory_Click (:2281) — both pickers reset to today; the pest combos are not cleared. */
    function newHistory() {
        $('FromDateHistory').value = today(); $('chkFromDateHistory').checked = true; $('FromDateHistory').disabled = false;
        $('ToDateHistory').value = today(); $('chkToDateHistory').checked = true; $('ToDateHistory').disabled = false;
        $('FromDocNoHistory').value = '';
        $('ToDocNoHistory').value = '';
        ['CmbCustomerHistory', 'CmbItemHistory', 'CmbAnalystStatusHistory', 'cmbApprovedStatusHistory', 'CmbGpNoHistory', 'CmbVehicleNoHistory'].forEach(clearCombo);
        st.hist = []; st.histSel = -1; st.histDetail = []; st.histDetailOrig = []; st.histDetailId = 0; st.histSample = [];
        clearTable('grdhistory'); clearTable('grdHistrorySamAnalysis'); clearTable('grdHistroryPurAnalysis');
    }

    /** grdhistory_SelectionChanged (:2632) */
    async function historySelectionChanged(i) {
        var r = st.hist[i];
        if (!r) return;
        st.histSel = i;
        $('grdhistory').querySelectorAll('tbody tr[data-i]').forEach(function (tr) { tr.classList.toggle('is-sel', +tr.getAttribute('data-i') === i); });
        try {
            $('label14').textContent = 'Purchase Analysis (' + intOf(r.DocNo) + ')';
            /* :2653-2693 — the edited "Editable After Approval" results of the record shown before. */
            if (st.histDetail.length > 0 && st.histDetail.some(function (x) { return x.IsEditableAfterApproval; })) {
                var save = st.histDetail.filter(function (x) {
                    if (!x.IsEditableAfterApproval) return false;
                    var o = st.histDetailOrig.filter(function (y) { return y.Id === x.Id; })[0];
                    return !!o && x.ResultValue !== o.ResultValue;
                }).map(function (x) { return { Id: x.Id, ResultValue: num(x.ResultValue), Remarks: x.Remarks === null || x.Remarks === undefined ? '' : x.Remarks }; });
                if (save.length > 0) {
                    await api('POST', API + '/' + st.histDetailId + '/parameter-results', save);
                    alert('Data Updated Successfully');
                }
            }
            var o = await api('GET', API + '/' + r.Id);
            st.histDetailId = r.Id;
            st.histDetail = (o.rows || []).map(function (d) {
                return {
                    Id: d.Id, AnalysisGroup: d.AnalysisGroupDescription, AnalysisParameter: d.AnalysisParameterDescription,
                    MinValue: d.MinValue, MaxValue: d.MaxValue, ResultValue: num(d.InAnalysisResult), Remarks: d.RemarksDetail,
                    ParamsMinValue: d.ParamsMinValue, ParamsMaxValue: d.ParamsMaxValue, IsEditableAfterApproval: !!d.IsEditableAfterApproval
                };
            });
            st.histDetailOrig = st.histDetail.map(function (x) { return { Id: x.Id, ResultValue: x.ResultValue }; });
            flt.grdHistroryPurAnalysis = {};
            renderHistDetail();
            st.histSample = [];
            clearTable('grdHistrorySamAnalysis');
            var s = await api('GET', API + '/sample/' + intOf(o.InvLabSampleLogRegisterId));
            if (s && s.found) {
                st.histSample = s.details || [];
                flt.grdHistrorySamAnalysis = {};
                renderHistSample();
            }
        } catch (e) { alert(msg(e)); }
    }

    var HD_COLS = [['AnalysisParameter', 'AnalysisParameter', 140], ['MinValue', 'MinValue', 65], ['MaxValue', 'MaxValue', 65], ['ResultValue', 'ResultValue', 65],
        ['Remarks', 'Remarks', 185], ['ParamsMinValue', 'ParamsMinValue', 65], ['ParamsMaxValue', 'ParamsMaxValue', 65]];
    function hdCell(c, r) {
        if (c[0] === 'ResultValue') return fmtOpt(r.ResultValue, 2);
        if (c[0] === 'MinValue' || c[0] === 'MaxValue' || c[0] === 'ParamsMinValue' || c[0] === 'ParamsMaxValue') return netStr(num(r[c[0]]));
        return r[c[0]] === null || r[c[0]] === undefined ? '' : r[c[0]];
    }
    /**
     * GrdHistoryDetailPurSetting (:2747): Id and IsEditableAfterApproval hidden, grouped by AnalysisGroup,
     * header 2 lines, ResultValue "#,#.##" with a total. An "Editable After Approval" row shows its parameter
     * dark green and bold (:2779) and lets ResultValue and Remarks be edited (:2809 / NotEditTableFields :1775).
     */
    function renderHistDetail() {
        var t = $('grdHistroryPurAnalysis'), rows = st.histDetail;
        t.tHead.innerHTML = '<tr>' + HD_COLS.map(function (c) { return thW(c[1], c[2], 2); }).join('') + '</tr>' + filterRow('grdHistroryPurAnalysis', HD_COLS);
        var order = [], html = '', total = 0;
        rows.forEach(function (r) { if (order.indexOf(r.AnalysisGroup) < 0) order.push(r.AnalysisGroup); });
        order.forEach(function (g) {
            var mine = [];
            rows.forEach(function (r, i) { if (r.AnalysisGroup === g && passes('grdHistroryPurAnalysis', HD_COLS, r, hdCell)) mine.push(i); });
            if (!mine.length) return;
            html += '<tr class="grp"><td colspan="7">AnalysisGroup: ' + esc(g) + ' (' + mine.length + (mine.length === 1 ? ' item)' : ' items)') + '</td></tr>';
            mine.forEach(function (i) {
                var r = rows[i], ed = r.IsEditableAfterApproval;
                total += num(r.ResultValue);
                html += '<tr data-i="' + i + '"><td' + (ed ? ' class="pa-green"' : '') + '>' + esc(r.AnalysisParameter) + '</td>'
                    + '<td>' + esc(hdCell(HD_COLS[1], r)) + '</td><td>' + esc(hdCell(HD_COLS[2], r)) + '</td>'
                    + (ed ? '<td class="n"><input type="text" class="r" data-k="ResultValue" value="' + esc(fmtOpt(r.ResultValue, 2)) + '"></td>'
                          : '<td class="n">' + esc(fmtOpt(r.ResultValue, 2)) + '</td>')
                    + (ed ? '<td><input type="text" data-k="Remarks" value="' + esc(hdCell(HD_COLS[4], r)) + '"></td>' : '<td>' + esc(hdCell(HD_COLS[4], r)) + '</td>')
                    + '<td class="n">' + esc(hdCell(HD_COLS[5], r)) + '</td><td class="n">' + esc(hdCell(HD_COLS[6], r)) + '</td></tr>';
            });
        });
        t.tBodies[0].innerHTML = html;
        t.tFoot.innerHTML = '<tr><td></td><td></td><td></td><td class="n">' + fmtOpt(total, 2) + '</td><td></td><td></td><td></td></tr>';
    }
    function onHistDetailChange(e) {
        var tr = e.target.closest('tr[data-i]'), k = e.target.getAttribute('data-k');
        if (!tr || !k) return;
        var r = st.histDetail[+tr.getAttribute('data-i')];
        if (!r || !r.IsEditableAfterApproval) return;
        if (k === 'Remarks') { r.Remarks = e.target.value; return; }
        var txt = e.target.value.replace(/,/g, '').trim();
        if (txt !== '' && !isFinite(Number(txt))) { e.target.value = fmtOpt(r.ResultValue, 2); return; }
        r.ResultValue = txt === '' ? 0 : Number(txt);
        renderHistDetail();
    }

    var HS_COLS = [['AnalysisParameter', 'AnalysisParameter', 140], ['MinValue', 'MinValue', 65], ['MaxValue', 'MaxValue', 65], ['ResultValue', 'ResultValue', 65]];
    function hsCell(c, r) {
        if (c[0] === 'AnalysisParameter') return r.AnalysisParameterDescription;
        return netStr(num(r[c[0]]));
    }
    /** :2719-2738 + GrdHistorySampSetting (:2832): Id hidden, grouped by AnalysisGroup, header 2 lines. */
    function renderHistSample() {
        var t = $('grdHistrorySamAnalysis'), rows = st.histSample;
        if (!rows.length) { clearTable('grdHistrorySamAnalysis'); return; }
        t.tHead.innerHTML = '<tr>' + HS_COLS.map(function (c) { return thW(c[1], c[2], 2); }).join('') + '</tr>' + filterRow('grdHistrorySamAnalysis', HS_COLS);
        var order = [], html = '';
        rows.forEach(function (r) { if (order.indexOf(r.AnalysisGroupDescription) < 0) order.push(r.AnalysisGroupDescription); });
        order.forEach(function (g) {
            var mine = rows.filter(function (r) { return r.AnalysisGroupDescription === g && passes('grdHistrorySamAnalysis', HS_COLS, r, hsCell); });
            if (!mine.length) return;
            html += '<tr class="grp"><td colspan="4">AnalysisGroup: ' + esc(g) + ' (' + mine.length + (mine.length === 1 ? ' item)' : ' items)') + '</td></tr>';
            mine.forEach(function (r) {
                html += '<tr><td>' + esc(r.AnalysisParameterDescription) + '</td><td>' + esc(netStr(num(r.MinValue))) + '</td><td>' + esc(netStr(num(r.MaxValue)))
                    + '</td><td class="n">' + esc(netStr(num(r.ResultValue))) + '</td></tr>';
            });
        });
        t.tBodies[0].innerHTML = html;
        t.tFoot.innerHTML = '<tr><td></td><td></td><td></td><td></td></tr>';
    }

    /** grdhistory_CellUpdated (:2472) */
    function onHistoryCellChange(e) {
        var tr = e.target.closest('tr[data-i]'), k = e.target.getAttribute('data-k');
        if (!tr || (k !== 'WeightCut' && k !== 'RateCut')) return;
        var r = st.hist[+tr.getAttribute('data-i')];
        if (!r) return;
        var v = e.target.value;
        if (k === 'WeightCut') {
            if (!r.WeightCutCompulsory) {
                alert("WeightCut can't be changed when WeightCut is not Compulsory .");
                e.target.value = r.WeightCut = netStr(num(r.WeightCutBefore));
                return;
            }
            if (num(v) < 0) { alert('WeightCut cannot be less than zero.'); e.target.value = r.WeightCut = '0'; return; }
            r.WeightCut = v;
            r.WeightCutBefore = netStr(num(v));
        } else {
            if (!r.RateCutCompulsory) {
                alert("Rate Cut can't be changed when RateCut is not Compulsory .");
                e.target.value = r.RateCut = netStr(num(r.RateCutBefore));
                return;
            }
            if (num(v) < 0) { alert('Rate Cut cannot be less than zero.'); e.target.value = r.RateCut = '0'; return; }
            r.RateCut = v;
            r.RateCutBefore = netStr(num(v));
        }
    }
    /** grdhistory_ColumnButtonClick (:2578) */
    function historyButton(i, key, button) {
        var r = st.hist[i];
        if (!r) return;
        if (key === 'Edit') return act(button, async function () { await readById(r.Id); });
        if (key === 'Print') return act(button, function () { return print653(r.Id, false); });
        if (key !== 'Update') return;
        var tr = $('grdhistory').querySelector('tbody tr[data-i="' + i + '"]');
        if (!r.WeightCutCompulsory) {
            alert("Weight Cut can't be changed when it is not compulsory.");
            r.WeightCut = netStr(num(r.WeightCutBefore));
            if (tr) tr.querySelector('input[data-k="WeightCut"]').value = r.WeightCut;
            return;
        }
        if (!r.RateCutCompulsory) {
            alert("Rate Cut can't be changed when it is not compulsory.");
            r.RateCut = netStr(num(r.RateCutBefore));
            if (tr) tr.querySelector('input[data-k="RateCut"]').value = r.RateCut;
            return;
        }
        return act(button, async function () {
            var res = await api('POST', API + '/' + r.Id + '/cuts', { WeightCut: String(r.WeightCut), RateCut: String(r.RateCut) });
            alert(res.message);                                                          // "Data updated successfully."
            await historyGridFill();
        });
    }
    function onHistoryClick(e) {
        var tr = e.target.closest('tbody tr[data-i]');
        if (!tr) return;
        var i = +tr.getAttribute('data-i');
        var b = e.target.closest('button[data-act]');
        var pic = e.target.closest('a[data-pic]');
        var open = e.target.closest('a[data-open]');
        if (open) {                                                                      // web rule 4: the DocNo opens the record (= Edit, :2590)
            var ro = st.hist[i];
            if (ro) act(open, async function () { if (i !== st.histSel) await historySelectionChanged(i); await readById(ro.Id); });
            return;
        }
        if (i !== st.histSel) {
            var p = historySelectionChanged(i);
            if (b) { p.then(function () { historyButton(i, b.getAttribute('data-act'), b); }); return; }
        } else if (b) { historyButton(i, b.getAttribute('data-act'), b); return; }
        if (pic) {                                                                       // grdhistory_LinkClicked (:2547)
            var r = st.hist[i], which = pic.getAttribute('data-pic');
            if (r && intOf(r[which === 'cooking' ? 'CookingPic' : 'AnalysisPic']) === 1) {
                $('dlgPicImg').src = API + '/' + r.Id + '/picture/' + which;
                show('dlgPic', true);
            }
        }
    }

    // =========================================================================== loader

    /* PGP:204-216 + grdSettings (PGP:236): Id and RefDocumentTypeId hidden; "Select" first (30). */
    var LDR_COLS = [['GpNo', 'GpNo', 80], ['GpDate', 'GpDate', 80], ['VehicleType', 'VehicleType', 100], ['VehicleNo', 'VehicleNo', 100],
        ['PartyName', 'PartyName', 220], ['BiltyNo', 'BiltyNo', 80], ['City', 'City', 150], ['InTime', 'InTime', 90], ['EntryUser', 'EntryUser', 120],
        ['EntryDate', 'EntryDate', 120], ['OrderType', 'OrderType', 130]];
    function ldrCell(c, r) {
        if (c[0] === 'GpDate') return dMMMyyyy(r.GpDate);
        if (c[0] === 'InTime') return hhmmtt(r.InTime);
        if (c[0] === 'EntryDate') return dMMMyyyyTime(r.EntryDate);
        return r[c[0]] === null || r[c[0]] === undefined ? '' : r[c[0]];
    }
    function renderLoader() {
        var t = $('ldrGrd');
        if (!st.loader.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; return; }
        t.tHead.innerHTML = '<tr><th style="width:30px;"><input type="checkbox" id="ldrAll"></th>' + LDR_COLS.map(function (c) { return thW(c[1], c[2]); }).join('') + '</tr>';
        t.tBodies[0].innerHTML = st.loader.map(function (r, i) {
            return '<tr data-i="' + i + '"><td class="c"><input type="checkbox" data-pick></td>' + LDR_COLS.map(function (c) {
                if (c[0] === 'GpNo') return '<td class="c"><a class="lnk doc" data-pickgp="1" title="Load this gate pass">' + esc(ldrCell(c, r)) + '</a></td>';
                return '<td>' + esc(ldrCell(c, r)) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        $('ldrAll').addEventListener('change', function () { var on = this.checked; t.querySelectorAll('input[data-pick]').forEach(function (c) { c.checked = on; }); });
    }
    /** CombosFill (PGP:93) */
    async function loaderCombos() {
        var d = await api('GET', API + '/loader/combos');
        if (d.suppliers && d.suppliers.length) { var a = comboVal('ldrCmbSupplier'); fill('ldrCmbSupplier', d.suppliers, 'Id', 'Name', false); setVal('ldrCmbSupplier', a || null); } else unbind('ldrCmbSupplier');
        if (d.orderTypes && d.orderTypes.length) { var b = comboVal('ldrCmbOrderType'); fill('ldrCmbOrderType', d.orderTypes, 'Id', 'Name', false); setVal('ldrCmbOrderType', b || null); } else unbind('ldrCmbOrderType');
    }
    /** FillGrid (PGP:186) */
    async function loaderFill() {
        st.loader = await api('GET', API + '/loader?supplierId=' + comboVal('ldrCmbSupplier') + '&orderTypeId=' + comboVal('ldrCmbOrderType')) || [];
        renderLoader();
    }
    /** BtnLoader_Click (:3203) -> LoadPendingGPForPurchaseLab_Load (PGP:77) */
    function openLoader() {
        return act($('BtnLoader'), async function () {
            unbind('ldrCmbSupplier'); unbind('ldrCmbOrderType');
            st.loader = []; renderLoader();
            show('dlgLoader', true);
            await loaderCombos();
            await loaderFill();
        });
    }
    /** BtnLoad_Click (PGP:286) */
    function loaderLoad() {
        var ids = [];
        $('ldrGrd').querySelectorAll('tbody tr[data-i]').forEach(function (tr) {
            var c = tr.querySelector('input[data-pick]');
            if (c && c.checked) ids.push(intOf(st.loader[+tr.getAttribute('data-i')].Id));
        });
        if (ids.length === 0) { alert('Chek the row first'); return; }
        var first = 0;
        for (var i = 0; i < ids.length; i++) {
            if (ids[i] === 0) continue;
            if (first === 0) first = ids[i];
            if (first !== ids[i]) { alert('Sorry, you can select only one record'); return; }
        }
        loaderPick(first, $('ldrBtnLoad'));
    }
    /** BtnLoader_Click (:3210-3213) after the dialog hides: cmbGatePassNo.Value = GpId; txtgatepassno_Leave. */
    function loaderPick(gpId, button) {
        if (!(gpId > 0)) { show('dlgLoader', false); return; }
        return act(button || null, async function () {
            show('dlgLoader', false);
            setVal('cmbGatePassNo', gpId);
            await gatePassLeave();
        });
    }
    /** MakeShortCutKeys (PGP:327) */
    function showKeys() {
        var keys = [['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+L', 'To Press Load Button'], ['Ctrl+S', 'For Search'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
        $('grdKeys').tBodies[0].innerHTML = keys.map(function (k) { return '<tr><td>' + esc(k[0]) + '</td><td>' + esc(k[1]) + '</td></tr>'; }).join('');
        show('dlgKeys', true);
    }
    /** LoadPurchaseOrder_KeyDown (PGP:363) */
    function loaderKey(e) {
        var c = e.ctrlKey, k = e.key;
        if (k === 'Escape' || (c && (k === 'e' || k === 'E'))) { e.preventDefault(); show('dlgLoader', false); }
        else if (c && e.altKey) { e.preventDefault(); showKeys(); }
        else if (c && (k === 's' || k === 'S')) { e.preventDefault(); act($('ldrBtnShow'), loaderFill); }
        else if (c && (k === 'l' || k === 'L')) { e.preventDefault(); loaderLoad(); }
        else if (c && (k === 'n' || k === 'N')) { e.preventDefault(); act($('ldrBtnNew'), async function () { clearCombo('ldrCmbSupplier'); clearCombo('ldrCmbOrderType'); await loaderFill(); }); }
        else if (c && (k === 'r' || k === 'R')) { e.preventDefault(); act($('ldrBtnRefresh'), loaderCombos); }
    }

    // ============================================================================= tabs / keys

    function selectTab(id) {
        ['tabForm', 'tabHistory'].forEach(function (t) { $(t).classList.toggle('is-active', t === id); });
        $('tabBtnForm').classList.toggle('is-active', id === 'tabForm');
        $('tabBtnHistory').classList.toggle('is-active', id === 'tabHistory');
        $('btnHistory').innerHTML = id === 'tabHistory' ? '<i class="fa fa-pencil-square-o"></i> Form' : '<i class="fa fa-history"></i> History';
        if (id === 'tabHistory') $('FromDateHistory').focus();                          // tabControl1_SelectedIndexChanged (:2273)
    }
    function closeFullscreen() {
        var full = document.querySelector('.wf-block.is-full');
        if (full) full.classList.remove('is-full');
        return !!full;
    }
    function onTab1() { return $('tabHistory').classList.contains('is-active'); }
    function focusNext(from) {
        var list = Array.prototype.filter.call(document.querySelectorAll('.pa-page.is-active input, .pa-page.is-active select, .pa-page.is-active textarea, .pa-page.is-active button'), function (el) {
            return !el.disabled && el.type !== 'hidden' && el.type !== 'file' && el.offsetParent !== null && el.getAttribute('tabindex') !== '-1';
        });
        var i = list.indexOf(from);
        if (i >= 0 && i + 1 < list.length) list[i + 1].focus();
    }
    /** InvLabPurchaseAnalysis_KeyDown (:3371) */
    function onKey(e) {
        if (!L) return;
        if (!$('dlgPic').hidden) { if (e.key === 'Escape') show('dlgPic', false); return; }
        if (!$('dlgKeys').hidden) { if (e.key === 'Escape') show('dlgKeys', false); return; }
        if (!$('dlgLoader').hidden) { loaderKey(e); return; }
        if (e.key === 'Escape' && closeFullscreen()) { e.preventDefault(); return; }
        var c = e.ctrlKey, k = e.key, tag = e.target && e.target.tagName;
        /* An open combo drop-down owns Enter and Escape (pick / close the list). */
        var comboOpen = !!(e.target && e.target.classList && e.target.classList.contains('dtcombo-input') && e.target.getAttribute('aria-expanded') === 'true');
        if (comboOpen && (k === 'Enter' || k === 'Escape')) return;
        if (k === 'Enter' && !c && !e.altKey) {                                         // Keys.Return -> {TAB}
            if (tag === 'BUTTON' || tag === 'A') return;
            if (tag === 'INPUT' || tag === 'SELECT' || tag === 'TEXTAREA') { e.preventDefault(); focusNext(e.target); }
            return;
        }
        if (k === 'Delete' && !onTab1() && st.grdFocus && tag !== 'INPUT' && tag !== 'TEXTAREA' && tag !== 'SELECT') { grdDeleteCurrent(); return; }
        if (c && (k === 's' || k === 'S')) {
            e.preventDefault();
            if (!onTab1()) { if (visible('btnSave') && !$('btnSave').disabled) insert($('btnSave'), false); }
            else showHistory();
        } else if (c && (k === 'n' || k === 'N')) {
            e.preventDefault();
            if (!onTab1()) act($('btnNew'), refresh); else newHistory();
        } else if (c && (k === 't' || k === 'T')) {
            e.preventDefault();
            if (onTab1()) { selectTab('tabForm'); $('txtdocdate').focus(); } else selectTab('tabHistory');
        } else if ((c && (k === 'e' || k === 'E')) || k === 'Escape') {
            e.preventDefault();
            window.location.href = '/quality';                                           // Close()
        } else if (c && (k === 'u' || k === 'U')) {
            e.preventDefault();
            if (visible('btnUpdate') && !$('btnUpdate').disabled) insert($('btnUpdate'), true);
        } else if (c && (k === 'p' || k === 'P')) {
            e.preventDefault();
            act($('btnprint'), function () { return print653(st.recId, true); });
        }
    }
    /** CommonServices.OnlytextdecimelFunction (digits and one '.') / OnlytextNumberFunction (digits). */
    function numericGuards() {
        document.addEventListener('keypress', function (e) {
            var el = e.target;
            if (!el || !el.hasAttribute || e.ctrlKey || e.metaKey || e.key.length !== 1) return;
            if (el.hasAttribute('data-integer')) { if (!/\d/.test(e.key)) e.preventDefault(); }
            else if (el.hasAttribute('data-decimal')) {
                if (!/[\d.]/.test(e.key)) e.preventDefault();
                else if (e.key === '.' && el.value.indexOf('.') > -1) e.preventDefault();
            }
        });
    }
    /** A combo whose text was emptied by the operator has no ActiveRow (the desktop's cleared UltraCombo). */
    function wireComboClear() {
        document.addEventListener('change', function (e) {
            var inp = e.target;
            if (!inp || !inp.classList || !inp.classList.contains('dtcombo-input')) return;
            var wrap = inp.closest('.dtcombo-wrap'), sel = wrap ? wrap.querySelector('select') : null;
            if (sel && inp.value.trim() === '' && sel.selectedIndex >= 0) {
                sel.selectedIndex = -1;
                sel.dispatchEvent(new Event('change', { bubbles: true }));
            }
        }, true);
    }

    // =============================================================================== wiring

    function wire() {
        $('btnNew').addEventListener('click', function () { act($('btnNew'), refresh); });
        $('btnRefresh').addEventListener('click', formRefresh);
        $('btnSave').addEventListener('click', function () { insert($('btnSave'), false); });
        $('btnUpdate').addEventListener('click', function () { insert($('btnUpdate'), true); });
        $('btnSlip257').addEventListener('click', function () { act(this, async function () { await print257(active('cmbGatePassNo') ? comboVal('cmbGatePassNo') : 0); }); });   // :3191
        $('btnprint').addEventListener('click', function () { act(this, function () { return print653(st.recId, true); }); });           // :3145
        $('btnHistory').addEventListener('click', function () { act(this, async function () { selectTab(onTab1() ? 'tabForm' : 'tabHistory'); }); });
        document.querySelectorAll('[data-full]').forEach(function (b) {                                                                  // rule 6: grid fullscreen toggle
            b.addEventListener('click', function () { $(b.getAttribute('data-full')).classList.toggle('is-full'); });
        });
        $('BtnLoader').addEventListener('click', openLoader);

        $('cmbGatePassNo').addEventListener('change', function () { act(null, gatePassLeave); });
        $('cmbitem').addEventListener('change', function () { act(null, itemLeave); });
        $('CmbRateUom').addEventListener('change', function () { act(null, rateUomLeave); });
        $('CmbSampleAnaylsis').addEventListener('change', function () { act(null, sampleLeave); });
        $('cmbanalysisgroup').addEventListener('change', function () { act(null, groupLeave); });
        $('CmbWtCuton').addEventListener('change', wtCutOnLeave);
        $('CmbPestResult').addEventListener('change', pestResultChanged);
        ['txtItemRate', 'txtAddRate', 'txtLessRate', 'txtRateCut'].forEach(function (id) { $(id).addEventListener('input', netRateCalc); });

        /* The Delete key removes the current row only while the parameter grid is the control last clicked. */
        document.addEventListener('mousedown', function (e) { st.grdFocus = !!(e.target && e.target.closest && e.target.closest('#grd')); });
        $('grd').addEventListener('change', onGrdChange);
        $('grd').addEventListener('click', onGrdClick);
        $('grdSubItems').addEventListener('change', onSubChange);
        wireFilter('grdSubItems', renderSub);

        $('TakePhotoButton').addEventListener('click', function () { browsePic('analysis'); });
        $('btnCapturecooking').addEventListener('click', function () { browsePic('cooking'); });
        $('btnResetDriv').addEventListener('click', function () { resetPic('analysis'); });
        $('btnResetCooking').addEventListener('click', function () { resetPic('cooking'); });
        $('fileAnalysisPic').addEventListener('change', function () { onPicChosen('analysis', this); });
        $('fileCookingPic').addEventListener('change', function () { onPicChosen('cooking', this); });

        $('tabBtnForm').addEventListener('click', function () { selectTab('tabForm'); });
        $('tabBtnHistory').addEventListener('click', function () { selectTab('tabHistory'); });

        $('btnNewHistory').addEventListener('click', function () { act(this, async function () { newHistory(); }); });
        $('btnRefreshHistory').addEventListener('click', function () {
            act($('btnRefreshHistory'), async function () { L.history = await api('GET', API + '/history-refresh'); bindHistoryCombos(L.history); });
        });
        $('btnShowHistory').addEventListener('click', showHistory);
        $('chkFromDateHistory').addEventListener('change', function () { $('FromDateHistory').disabled = !this.checked; });
        $('chkToDateHistory').addEventListener('change', function () { $('ToDateHistory').disabled = !this.checked; });
        $('grdhistory').addEventListener('click', onHistoryClick);
        $('grdhistory').addEventListener('change', onHistoryCellChange);
        $('grdhistory').addEventListener('dblclick', function (e) {                     // grdhistory_DoubleClick (:2534)
            var tr = e.target.closest('tbody tr[data-i]');
            if (!tr || e.target.closest('input,button,a')) return;
            var r = st.hist[+tr.getAttribute('data-i')];
            if (r) act(null, async function () { await readById(r.Id); });
        });
        wireFilter('grdhistory', renderHistory);
        $('grdHistroryPurAnalysis').addEventListener('change', onHistDetailChange);
        wireFilter('grdHistroryPurAnalysis', renderHistDetail);
        wireFilter('grdHistrorySamAnalysis', renderHistSample);

        $('dlgLoaderClose').addEventListener('click', function () { show('dlgLoader', false); });                 // btnclose_Click: GpId = 0
        $('ldrBtnNew').addEventListener('click', function () {                                                     // btnReset_Click (PGP:148)
            act($('ldrBtnNew'), async function () { clearCombo('ldrCmbSupplier'); clearCombo('ldrCmbOrderType'); await loaderFill(); });
        });
        $('ldrBtnRefresh').addEventListener('click', function () { act($('ldrBtnRefresh'), loaderCombos); });     // BtnRefresh_Click (PGP:162)
        $('ldrBtnShortCutKey').addEventListener('click', function () { act(this, async function () { showKeys(); }); });
        $('ldrBtnShow').addEventListener('click', function () { act($('ldrBtnShow'), loaderFill); });
        $('ldrBtnLoad').addEventListener('click', loaderLoad);
        $('ldrGrd').addEventListener('click', function (e) {                                                       // web rule 4: the GpNo link loads that gate pass
            var a = e.target.closest('a[data-pickgp]'), tr = e.target.closest('tbody tr[data-i]');
            if (!a || !tr) return;
            var r = st.loader[+tr.getAttribute('data-i')];
            if (r) loaderPick(intOf(r.Id), a);
        });
        $('dlgKeysClose').addEventListener('click', function () { show('dlgKeys', false); });
        $('dlgPicClose').addEventListener('click', function () { show('dlgPic', false); });

        document.addEventListener('keydown', onKey);
        numericGuards();
        wireComboClear();
    }

    // =============================================================================== load

    /** InvLabPurchaseAnalysis_Load (:544) */
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
        var r = rights();
        getConfiguration();
        $('btnSave').disabled = !r.save;                                                 // :558-560
        $('btnprint').disabled = !r.print;
        $('btnUpdate').disabled = !r.update;
        show('btnSave', true);
        show('btnUpdate', false);
        $('txtGpQty').disabled = true;
        $('txtdocdate').value = today();
        $('datBiltyDate').value = today();
        bindGatePasses();
        bindCrop();
        bindPacking();
        bindJobLot();
        if (intOf(L.docNo) > 0) $('txtdocno').value = L.docNo;
        bindLabStatus();
        bindWarehouse();
        bindWeightCutOn();
        configDefaults();
        bindPest();
        activateFirstPestResult();
        $('FromDateHistory').value = today();
        $('ToDateHistory').value = today();
        bindHistoryCombos(L.history);
        $('txtWeightCutUom').disabled = comboVal('CmbWtCuton') !== 2;
        focusCtl('cmbGatePassNo');
        /* ?id=<record id> — the report pages link here; the record is opened as the History "Edit" button does. */
        var open = intOf(new URLSearchParams(window.location.search).get('id'));
        if (open > 0) await act(null, function () { return readById(open); });
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
}());
