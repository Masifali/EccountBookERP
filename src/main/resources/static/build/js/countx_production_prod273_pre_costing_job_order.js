/* ============================================================================================
 * Screen 273 "Production Job Order (Pre Costing)" — Architecture.WinApp.Production.
 * ProductionPreCostingAndJobOrder (base.Name = "ProductionPreCostingAndJobOrder"), DocumentTypeId 401.
 * Line numbers below are ProductionPreCostingAndJobOrder.cs.
 *
 *   frmProductionJobOrder_Load:769   rights, flags, every picker, GetLastSaveAccount, GeneratePlanCode   (init)
 *   Reset:1436                       (reset)          btnRefresh_Click:1520 (refresh)
 *   Insert():1631                    client pre-checks → confirm → btnGenerate_Click → totals confirms → POST save
 *   ReadById:2088                    (readById)       DataGridHistory_SaveAs:4898 (historyAction 'saveas')
 *   Output grid  :3007-3594          Input grid :2265-2969      OverHeads :3674-4155
 *   Packing Mat. :4157-4562          Plants :6509-6767          Lab Standards :6845-7170
 *   btnGenerate_Click:6185           CostingCalculations:6200, InsertPMInOutPutGrid:6328, InsertOhInOutPutGrid:6399,
 *                                    CalculateNEtRateAndamount:6470
 *   History :4564-5169               KeyDown :5674            Load Invoices :6096-6150
 *
 * BUTTON VISIBILITY IS STATE, AS ON THE DESKTOP
 *   Reset        Save visible, Update hidden, SaveAs hidden            (:1473)
 *   ReadById     Save hidden, SaveAs hidden, Update visible            (:2100)
 *   History SaveAs  Save hidden, Update hidden, SaveAs visible         (:4904)
 * and the plant-row delete reads that state ("btnsave.Visible && btnsave.Enabled" = just drop the row, :6644).
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/production/pre-costing-job-order/api';
    var RPT = '630-PreCostProductionJobOrderSlip.rpt';

    /* ------------------------------------------------------------------ state */
    var look = {};
    var rights = {};
    var withoutRates = false;          // JobOrderCreatewithoutRates (:825)
    var checkStock = false;            // CheckStockAndGiveWarningMessageOnJobOrder (:826)
    var rateDecimals = 2;
    var recId = 0;                     // RECID
    var updateMode = false;            // UpdateMode
    var vis = { save: true, update: false, saveAs: false };

    var outputs = [], inputs = [], overheads = [], packing = [], plants = [], labRows = [];
    var removedOutputs = [], removedInputs = [], removedOverheads = [], removedPacking = [];   // lstRemove*
    var edit = { output: -1, input: -1, oh: -1, pm: -1, plant: -1 };                           // updateDetailIndex*
    var current = { output: -1, input: -1, oh: -1, pm: -1, plant: -1, lab: -1 };
    var historyRows = [], historyCurrent = -1;
    var loaderRows = [], loaderOpen = false;
    var suppressEntryType = false;

    /* ------------------------------------------------------------------ dom helpers */
    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function box(m) { window.alert(m); }
    function ask(m) { return window.confirm(m); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function focusEl(id) { var e = $id(id); if (!e) return; try { e.focus(); } catch (x) { /* ignore */ } }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function num(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function intOf(v) { return Math.trunc(num(v)); }
    /* Convert.ToInt32(double) — banker's rounding (GetBrandQtyByBrandUomAndBrnadUom:6315). */
    function cInt(v) { var n = num(v), f = Math.floor(n), d = n - f; if (d > 0.5) return f + 1; if (d < 0.5) return f; return f % 2 === 0 ? f : f + 1; }
    function selText(id) {
        var el = $id(id);
        if (!el || el.selectedIndex < 0) return '';
        var o = el.options[el.selectedIndex];
        return (o && o.value !== '' && o.value !== '0') ? o.text : '';
    }
    function cv(id) { return intOf(val(id)); }
    function fmt(v, places) {
        var n = num(v);
        return n.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: places === undefined ? 3 : places });
    }
    /* "#,#" — no decimals, blank for zero (CostingCalculations:6281). */
    function fmt0(v) { var n = Math.round(num(v)); return n === 0 ? '0' : n.toLocaleString('en-US'); }
    function fmtRate(v) { return num(v).toLocaleString('en-US', { minimumFractionDigits: rateDecimals, maximumFractionDigits: rateDecimals }); }
    var MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function parts(v) { var m = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2}))?/.exec(String(v || '')); return m ? { y: m[1], M: m[2], d: m[3], h: m[4], mi: m[5] } : null; }
    function fmtDMY(v) { var p = parts(v); return p ? p.d + '-' + MONTHS[+p.M - 1] + '-' + p.y : String(v || ''); }
    function fmtDate(v) { var p = parts(v); return p ? p.d + '-' + p.M + '-' + p.y : String(v || ''); }
    function fmtStamp(v) {
        var p = parts(v); if (!p) return String(v || '');
        var h = +(p.h || 0), tt = h < 12 ? 'AM' : 'PM', h12 = h % 12 === 0 ? 12 : h % 12;
        return p.d + '-' + p.M + '-' + p.y + ' ' + String(h12).padStart(2, '0') + ':' + (p.mi || '00') + ' ' + tt;
    }
    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function today() { return iso(new Date()); }
    function isoOf(v) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || '')); return m ? m[0] : ''; }

    function csrf(opt) {
        var token = document.querySelector('meta[name="_csrf"]'), header = document.querySelector('meta[name="_csrf_header"]');
        if (token && header && opt.method && opt.method.toUpperCase() !== 'GET') opt.headers[header.getAttribute('content')] = token.getAttribute('content');
    }
    function request(url, options) {
        var opt = options || {};
        opt.credentials = 'same-origin';
        opt.headers = opt.headers || {};
        opt.headers.Accept = 'application/json';
        csrf(opt);
        return fetch(url, opt).then(function (r) {
            return r.text().then(function (t) {
                var body = null;
                try { body = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
                if (!r.ok) throw new Error((body && body.message) || t || ('Request failed (' + r.status + ')'));
                return body;
            });
        });
    }
    function getJson(url) { return request(url, {}); }
    function postJson(url, body) { return request(url, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body) }); }
    function qs(o) { var a = []; Object.keys(o).forEach(function (k) { if (o[k] !== undefined && o[k] !== null && o[k] !== '') a.push(k + '=' + encodeURIComponent(o[k])); }); return a.length ? '?' + a.join('&') : ''; }
    function fail(e) { box(e && e.message ? e.message : String(e)); }

    /* ------------------------------------------------------------------ pickers */

    /** DropDownBind.BindDDL / BindDDLNew: default row when zeroIndex, selection kept when still present. */
    function fill(selectId, rows, zeroIndex, keepValue) {
        var el = $id(selectId); if (!el) return;
        var keep = keepValue === undefined ? el.value : String(keepValue);
        var h = zeroIndex ? '<option value="0">...Select Any Value...</option>' : '<option value=""></option>';
        (rows || []).forEach(function (r) { h += '<option value="' + esc(r.Id) + '">' + esc(r.Name) + '</option>'; });
        el.innerHTML = h;
        el.value = keep;
        if (el.value !== keep) el.selectedIndex = 0;
    }
    /** "if (dt.Rows.Count > 0) Bind" — an empty list leaves the old list in place. */
    function fillIf(selectId, rows, zeroIndex) { if (rows && rows.length) fill(selectId, rows, zeroIndex); }
    function setCombo(id, v) { var el = $id(id); if (!el) return; el.value = String(v === null || v === undefined ? '' : v); if (el.value !== String(v) && el.options.length) el.selectedIndex = 0; }
    function clearCombo(id) { var el = $id(id); if (el && el.options.length) el.selectedIndex = 0; }
    function firstRow(id) { var el = $id(id); if (el && el.options.length) { el.selectedIndex = el.options[0].value === '' ? 1 : 0; } }     // Rows[0].Activate()
    function setOptions(id, rows) { fill(id, rows, false); }

    function applyRights() {
        rights = look.rights || {};
        $id('btnsave').disabled = !rights.save;                 // :778
        $id('btnSaveAs').disabled = !rights.save;               // :779
        $id('btnUpdate').disabled = !rights.update;             // :780
        $id('Print').disabled = !rights.print;                  // :781
        var missing = [];
        if (!rights.save) missing.push('Save');
        if (!rights.update) missing.push('Update');
        if (!rights.print) missing.push('Print');
        var note = $id('rightsNote');
        note.textContent = missing.length ? 'Your account does not have the ' + missing.join(', ') + ' right on this screen, so those buttons are off.' : '';
        note.classList.toggle('is-hidden', !missing.length);
    }
    function applyVisibility() {
        show('btnsave', vis.save); show('btnUpdate', vis.update); show('btnSaveAs', vis.saveAs);
    }
    function saveModeActive() { return vis.save && !!rights.save; }     // btnsave.Visible && btnsave.Enabled

    /** :827-838, :990-992 — the rate controls follow JobOrderCreatewithoutRates. */
    function applyRateFlag() {
        ['txtRateInPut', 'CmbRateUomInPut', 'txtItemRateOutPut', 'CmbRateUomOutPut', 'txtItemRateOverHeads', 'txtExpAmountOverHead',
         'txtPackingMaterialRate', 'txtPackingMaterialAmount'].forEach(function (id) { var e = $id(id); if (e) e.disabled = withoutRates || (id === 'txtPackingMaterialAmount'); });
        $id('BtnGenerateInPutRates').disabled = withoutRates;
        $id('txtRateInPut').readOnly = false;                    // :991
    }

    function bindAll(d) {
        fillIf('CmbWipItem', d.wipItems, false);                                          // ItemFill:1426 (BindDDLNew, no default row)
        fillIf('cmbjoblotinput', d.jobLots, true); fillIf('cmbJobLotOutput', d.jobLots, true);   // JobLotBind:5277 (default row)
        fillIf('CmbWorkInProcess', d.accounts, false);                                     // AccountFills:1382
        fillIf('cmbPlanType', d.planTypes, false);                                         // PlanTypeBind:5509
        fillIf('cmbPlantFeader', d.plants, false);                                         // PlantFeader:1081
        fillIf('cmbProductionType', d.productionTypes, false);                             // ProductionTypeBind:5527
        fillIf('cmbcropyearinput', d.cropYears, true); fillIf('cmbCropYearOutput', d.cropYears, true);   // CropYear:5198
        /* WareHouseFill:5175 — BindAndRetainSelection(..., insertDefaultRow: true). */
        ['cmbwarehouseinput', 'cmbWareHouseOutPut', 'CmbWarehousePackingMaterial', 'CmbWipWarehouse'].forEach(function (id) { fill(id, d.warehouses, true); });
        itemDetailFill();                                                                   // ItemDetailFill:5229
        fillIf('cmbPackingType', d.packingTypes, false); fillIf('cmbPackTypeInput', d.packingTypes, false);   // CmbPackingTypeFill:5464
        fillIf('cmbItemPackingMaterial', d.packingMaterialItems, false);                   // PackingMaterialItemBind:5626
        fillIf('cmbChartofAccountOverHead', d.overheadAccounts, false);                    // ChartOfAccountOverHead:3695
    }

    /** ItemDetailFill:5229 — both item pickers from Item.ReadAllItems. */
    function itemDetailFill() {
        var items = look.itemsReadAll || [];
        if (!items.length) return;
        fill('cmbItem', items, false);
        fill('cmbItemInput', items, false);
    }

    /* ------------------------------------------------------------------ load */

    function init() {
        ['txtPlanDate', 'txtstartDate', 'txtEndDate', 'FromDateHistory', 'ToDateHistory'].forEach(function (id) { setVal(id, today()); });
        wireInputs();
        wireGrids();
        renderAll();
        getJson(api + '/lookups').then(function (d) {
            look = d || {};
            applyRights();
            withoutRates = !!look.jobOrderCreatewithoutRates;
            checkStock = !!look.checkStockAndGiveWarningMessageOnJobOrder;
            rateDecimals = look.rateDecimals === undefined ? 2 : intOf(look.rateDecimals);
            applyRateFlag();
            bindAll(look);
            fill('cmbStatus', look.statuses, false);                        // StatusBind:5546
            fill('cmbentrytype', look.entryTypes, false);                  // EntryTypeFill:2235
            fill('cmbFiltersType', look.filterTypes, false);               // FilterTypeFill:1101
            fill('CmbEntryTypeOP', look.outputEntryTypes, false);          // EntryTypeOutPut:2998
            fill('cmbperemeter', look.dateTypes, false);                   // ParameterFill:1035
            fill('xmbjoborderno', look.jobOrderNos, false);                // HistoryJobOrderNofill:4593
            partyModeChanged();                                             // SupplierDtFillFromStandard + BindSupplierName
            restoreLastWip(look);                                           // :862-873
            firstRow('cmbperemeter'); datePreset();                         // :1036 Rows[0].Activate
            firstRow('CmbEntryTypeOP');                                     // :2999
            firstRow('cmbPackingType'); firstRow('cmbPackTypeInput');       // :5466-5467
            firstRow('cmbStatus');                                          // :988
            suppressEntryType = true; firstRow('cmbentrytype'); suppressEntryType = false;
            entryTypeChanged();                                             // :2236 Rows[0].Activate → Input Detail tab
            applyVisibility();
            renderAll();
            focusEl('txtPlanDate');
        }).catch(fail);
    }

    function restoreLastWip(d) {
        if (intOf(d.lastWipAccountId) > 0) setCombo('CmbWorkInProcess', d.lastWipAccountId);
        if (intOf(d.lastWipItemId) > 0) setCombo('CmbWipItem', d.lastWipItemId);
        if (intOf(d.lastWipWareHouseId) > 0) setCombo('CmbWipWarehouse', d.lastWipWareHouseId);
    }

    /** btnRefresh_Click:1520 — rebinds without a Reset. */
    function refresh() {
        getJson(api + '/refresh').then(function (d) {
            Object.keys(d || {}).forEach(function (k) { look[k] = d[k]; });
            checkStock = !!look.checkStockAndGiveWarningMessageOnJobOrder;
            bindAll(look);
            fill('cmbFiltersType', look.filterTypes, false);
            partyModeChanged();
        }).catch(fail);
    }

    /** Reset:1436. NOT touched: Plan Date, Start/End Date, Job Order No pickers' lists, Preview. */
    function reset() {
        recId = 0;
        removedInputs = []; removedOutputs = []; removedPacking = []; removedOverheads = [];
        ['txtInvoiceReference', 'txtOtherInstruction', 'txtPlantInstruction', 'txtProductionInstruction', 'txtDeliveryInstruction'].forEach(function (id) { setVal(id, ''); });
        clearCombo('CmbWorkInProcess'); clearCombo('CmbWipItem'); clearCombo('CmbLotAc'); clearCombo('cmbProductionType');
        firstRow('cmbStatus'); clearCombo('cmbPlanType');
        outputs = []; inputs = []; overheads = []; packing = []; plants = []; labRows = [];
        edit = { output: -1, input: -1, oh: -1, pm: -1, plant: -1 };
        current = { output: -1, input: -1, oh: -1, pm: -1, plant: -1, lab: -1 };
        vis = { save: true, update: false, saveAs: false };
        updateMode = false;
        applyVisibility();
        $id('lblRecId').textContent = '';
        ['txtInPutTotalQty', 'txtInPutTotalWeight', 'txtTotalAmountInPut', 'txtTotalQtyBP', 'txtTotalWeightBP', 'txtTotalAmountBP',
         'txtTotalQtyFG', 'txtTotalWeightFG', 'txtTotalAmountFG', 'txtFGAvgRate', 'txtFGRateWithoutExp', 'txtPackingMaterialAmount', 'txtOverHeadsAmount'].forEach(function (id) { setVal(id, '0'); });
        renderAll();
        focusEl('txtPlanDate');
        return getJson(api + '/reset').then(function (d) {
            if (intOf(d.planCode) > 0) setVal('txtPlanCode', d.planCode);              // GeneratePlanCode:1408
            if (intOf(d.lastWipAccountId) > 0 || intOf(d.lastWipItemId) > 0) setCombo('CmbWipItem', d.lastWipItemId);   // :1497
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ tabs */

    function currentTab() { return $id('tabHistory').classList.contains('is-active') ? 1 : 0; }
    function tab(id) {
        ['tabForm', 'tabHistory'].forEach(function (t) { $id(t).classList.toggle('is-active', t === id); });
        $id('tabs1').querySelectorAll('.pc-tab1').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === id); });
        if (id === 'tabHistory') focusEl('FromDateHistory'); else focusEl('txtPlanDate');     // :4667
    }
    var TAB2 = ['tabPage3', 'tabPage4', 'tabPage5', 'tabPage6', 'tabPage7', 'tabLabStandard'];
    function tab2Index() { for (var i = 0; i < TAB2.length; i++) if ($id(TAB2[i]).classList.contains('is-active')) return i; return 0; }
    /** tabControl2_SelectedIndexChanged:6152. */
    function tab2(id, silent) {
        var changed = !$id(id).classList.contains('is-active');
        TAB2.forEach(function (t) { $id(t).classList.toggle('is-active', t === id); });
        $id('tabs2').querySelectorAll('.pc-tab2').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === id); });
        if (!changed && silent) return;
        var idx = TAB2.indexOf(id);
        if (idx === 0) { suppressEntryType = true; setCombo('cmbentrytype', 2); suppressEntryType = false; }          // Rows[1].Activate
        else if (idx === 2) { brandItemBindOnOhAndPm(); }
        else if (idx === 3) { brandItemBindOnOhAndPm(); }
        else if (idx === 4) { focusEl('cmbPlantFeader'); }
        brandItemBindOnLabStandard();                                                                                 // :6177
    }
    function tab3(id) {
        ['tabPage9', 'tabPage8', 'tabPage11', 'tabPage10'].forEach(function (t) { $id(t).classList.toggle('is-active', t === id); });
        $id('tabs3').querySelectorAll('.pc-tab2').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === id); });
    }

    /* ------------------------------------------------------------------ Filters For Item */

    /** cmbentrytype_ValueChanged:2244. */
    function entryTypeChanged() {
        if (suppressEntryType) return;
        var t = selText('cmbentrytype');
        if (t === 'Input') tab2('tabPage4', true);
        if (t === 'Output') tab2('tabPage3', true);
        docNoChanged();
    }
    /** cmbFiltersType_Leave:1109. */
    function filterTypeChanged() {
        var ft = cv('cmbFiltersType');
        $id('cmbDocNo').innerHTML = '<option value=""></option>';
        if (!ft) return;
        getJson(api + '/doc-no-list' + qs({ filterType: ft })).then(function (rows) {
            if (rows && rows.length) fill('cmbDocNo', rows, false);
        }).catch(fail);
    }
    function filterItems(list, key, id) {
        return (list || []).filter(function (r) { return intOf(r[key]) === id; }).map(function (r) { return { Id: r.Id, Name: r.Name }; });
    }
    /** cmbDocNo_Leave:1138 — Output: Item.GetAll filtered; Input: Item.ReadAllItems filtered; Sale Order / Export Contract from the server. */
    function docNoChanged() {
        var ft = selText('cmbFiltersType'), docId = cv('cmbDocNo'), entry = selText('cmbentrytype');
        if (ft && entry === 'Output') {
            if (ft === 'Item Category' && docId > 0) setOptions('cmbItem', filterItems(look.itemsGetAll, 'ItemCategoryId', docId));
            if (ft === 'Item Type' && docId > 0) setOptions('cmbItem', filterItems(look.itemsGetAll, 'ItemTypeId', docId));
            if ((ft === 'SaleOrder' || ft === 'Export Contract') && docId > 0) {
                getJson(api + '/items-by-doc' + qs({ filterType: cv('cmbFiltersType'), docId: docId })).then(function (rows) {
                    if (rows && rows.length) setOptions('cmbItem', rows); else $id('cmbItem').innerHTML = '<option value=""></option>';
                }).catch(fail);
            }
        } else if (!ft || !docId) { itemDetailFill(); }
        if (ft && entry === 'Input') {
            if (ft === 'Item Category' && docId > 0) setOptions('cmbItemInput', filterItems(look.itemsReadAll, 'ItemCategoryId', docId));
            if (ft === 'Item Type' && docId > 0) setOptions('cmbItemInput', filterItems(look.itemsReadAll, 'ItemTypeId', docId));
            if ((ft === 'SaleOrder' || ft === 'Export Contract') && docId > 0) {
                getJson(api + '/items-by-doc' + qs({ filterType: cv('cmbFiltersType'), docId: docId })).then(function (rows) {
                    if (rows && rows.length) setOptions('cmbItemInput', rows); else $id('cmbItemInput').innerHTML = '<option value=""></option>';
                }).catch(fail);
            }
        } else if (!ft || !docId) { itemDetailFill(); }
    }

    /* ------------------------------------------------------------------ UOM helpers (dtAllUomWithItemId) */

    function uomsOf(itemId) { return (look.allUoms || []).filter(function (u) { return intOf(u.ItemId) === itemId; }); }
    function uomEq(uomId) { var u = (look.allUoms || []).filter(function (x) { return intOf(x.Id) === uomId; })[0]; return u ? num(u.Equivalent) : 0; }
    /** bindPackSizeInput:2284 / bindRateUomAndItemPackUom:3022 — rebind both UOM pickers, keep the TEXT when still present. */
    function bindItemUoms(itemId, packId, rateId) {
        var packText = selText(packId), rateText = selText(rateId);
        var list = uomsOf(itemId).map(function (u) { return { Id: u.Id, Name: u.UOMCode }; });
        if (!list.length) { clearCombo(packId); clearCombo(rateId); return; }
        fill(packId, list, false, ''); fill(rateId, list, false, '');
        var byText = function (id, t) { var el = $id(id); if (!t) return; for (var i = 0; i < el.options.length; i++) if (el.options[i].text === t) { el.selectedIndex = i; return; } el.selectedIndex = 0; };
        byText(packId, packText); byText(rateId, rateText);
    }

    /* ------------------------------------------------------------------ generic grid renderer */

    function renderGrid(tableId, cols, rows, opts) {
        opts = opts || {};
        var t = $id(tableId);
        var visible = cols.filter(function (c) { return !c.hidden; });
        var head = (opts.x ? '<th class="col-x">X</th>' : '') + visible.map(function (c) { return '<th' + (c.w ? ' style="min-width:' + c.w + 'px"' : '') + '>' + esc(c.cap || c.key) + '</th>'; }).join('');
        t.tHead.innerHTML = '<tr>' + head + '</tr>';
        t.tBodies[0].innerHTML = rows.map(function (r, i) {
            var cls = i === opts.editing ? 'is-editing' : (i === opts.current ? 'is-current' : '');
            return '<tr tabindex="0" data-i="' + i + '" class="' + cls + '">' + (opts.x ? '<td class="col-x"><button type="button" class="pc-x" tabindex="-1" data-act="del">X</button></td>' : '')
                + visible.map(function (c) {
                    var v = r[c.key];
                    if (c.edit === 'num') return '<td class="num"><input type="text" class="cell" data-i="' + i + '" data-k="' + c.key + '" value="' + esc(v === null || v === undefined ? '' : v) + '"></td>';
                    if (c.edit === 'check') return '<td class="ctr"><input type="checkbox" data-i="' + i + '" data-k="' + c.key + '"' + (v ? ' checked' : '') + '></td>';
                    if (c.date) return '<td>' + esc(c.date(v)) + '</td>';
                    if (c.num) return '<td class="num">' + esc(c.rate ? fmtRate(v) : fmt(v, c.places)) + '</td>';
                    return '<td' + (c.cls ? ' class="' + c.cls + '"' : '') + '>' + esc(v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        if (t.tFoot) {
            var anySum = visible.some(function (c) { return c.sum; });
            t.tFoot.innerHTML = rows.length && anySum ? '<tr>' + (opts.x ? '<td></td>' : '') + visible.map(function (c) {
                if (!c.sum) return '<td></td>';
                var s = rows.reduce(function (a, r) { return a + num(r[c.key]); }, 0);
                return '<td class="num">' + esc(fmt(s, c.places)) + '</td>';
            }).join('') + '</tr>' : '';
        }
    }

    /* ---------------------------------------------- column sets (dt order, hidden ids removed, Delete first) */
    function outputCols() {
        var h = withoutRates;                                                                       // PlannedOutputGridSetting:3455
        return [
            { key: 'EntryType', w: 120 }, { key: 'WareHouseName', w: 120 }, { key: 'JobLotDescription', w: 120 }, { key: 'ItemName', w: 120 },
            { key: 'CropYear', w: 100 }, { key: 'PackingTypeName', w: 100 }, { key: 'QtyOuter', w: 70, num: true, sum: true }, { key: 'OuterPackSizeName', w: 80 },
            { key: 'Weight', w: 80, num: true, sum: true }, { key: 'ItemRate', w: 80, num: true, hidden: h }, { key: 'RateUom', w: 70, hidden: h },
            { key: 'ItemAmount', w: 100, num: true, sum: true, hidden: h }, { key: 'OHExpense', w: 100, num: true, sum: true, hidden: h }, { key: 'PMExpense', w: 100, num: true, sum: true, hidden: h },
            { key: 'OHGeneralExpense', w: 100, num: true, sum: true, hidden: h }, { key: 'PMGeneralExpense', w: 100, num: true, sum: true, hidden: h },
            { key: 'NetRate', w: 80, num: true, hidden: h }, { key: 'NetAmount', w: 100, num: true, sum: true, hidden: h }, { key: 'Remarks', w: 150 }
        ];
    }
    function inputCols() {
        var h = withoutRates;                                                                       // InputDetailGridSetting:2463
        return [
            { key: 'DocDate', w: 80, date: fmtDMY }, { key: 'DocNo', w: 70, cls: 'ctr' }, { key: 'WareHouse', w: 120 }, { key: 'JobLot', w: 120 }, { key: 'ItemName', w: 120 },
            { key: 'CropYear', w: 90 }, { key: 'PackType', w: 100 }, { key: 'BalanceQty', w: 80, num: true }, { key: 'BalanceWeight', w: 80, num: true },
            { key: 'Qty', w: 70, edit: 'num' }, { key: 'PackSize', w: 70 }, { key: 'Weight', w: 80, edit: 'num' },
            { key: 'ItemRate', w: 70, num: true, hidden: h }, { key: 'RateUom', w: 70, hidden: h }, { key: 'ItemAmount', w: 90, num: true, sum: true, hidden: h }, { key: 'Remarks', w: 120 }
        ];
    }
    var OH_COLS = [{ key: 'ChartOfAccount', w: 150 }, { key: 'Brand', w: 180 }, { key: 'BrandUom', w: 70 }, { key: 'ItemQty', w: 70, num: true, sum: true },
                   { key: 'ItemRate', w: 70, num: true }, { key: 'Amount', w: 90, num: true, sum: true }, { key: 'Remarks', w: 150 }];
    function pmCols() {
        var h = !rights.rateAndAmount;                                                              // grdDetailPackingMaterialSettings:4346
        return [{ key: 'Warehouse', w: 150 }, { key: 'PackingItem', w: 150 }, { key: 'Brand', w: 180 }, { key: 'BrandUom', w: 70 }, { key: 'ItemQty', w: 70, num: true, sum: true },
                { key: 'ItemRate', w: 70, num: true, hidden: h }, { key: 'Amount', w: 90, num: true, sum: true, hidden: h }, { key: 'Remarks', w: 150 }];
    }
    var PLANT_COLS = [{ key: 'PlantName', w: 200 }, { key: 'AssumedPlantCapacity', w: 100, num: true }, { key: 'RemarksDetail', w: 300 }];
    var LAB_COLS = [{ key: 'AnalysisGroup', w: 180 }, { key: 'AnalysisItem', w: 180 }, { key: 'MinValue', w: 90, edit: 'num' }, { key: 'MaxValue', w: 90, edit: 'num' }, { key: 'Active', w: 70, edit: 'check' }];

    function renderOutputs() { renderGrid('grdPlannedOutput', outputCols(), outputs, { x: true, current: current.output, editing: edit.output }); $id('lblOutputCount').textContent = outputs.length; }
    function renderInputs() { renderGrid('grdInputDetail', inputCols(), inputs, { x: true, current: current.input, editing: edit.input }); $id('lblInputCount').textContent = inputs.length; }
    function renderOverheads() { renderGrid('grdOverHead', OH_COLS, overheads, { x: true, current: current.oh, editing: edit.oh }); $id('lblOverheadCount').textContent = overheads.length; }
    function renderPacking() { renderGrid('grdPackingMaterial', pmCols(), packing, { x: true, current: current.pm, editing: edit.pm }); $id('lblPackingCount').textContent = packing.length; }
    function renderPlants() { renderGrid('grdPlantdetail', PLANT_COLS, plants, { x: true, current: current.plant, editing: edit.plant }); $id('lblPlantCount').textContent = plants.length; }
    function renderLab() { renderGrid('grdLabStandards', LAB_COLS, labRows, { x: true, current: current.lab }); $id('lblLabCount').textContent = labRows.length; }
    function renderAll() { renderOutputs(); renderInputs(); renderOverheads(); renderPacking(); renderPlants(); renderLab(); }

    /* ================================================================== OUTPUT (Planned Output Requirnment) */

    /** cmbItem_Leave:3007. */
    function outputItemLeave() { var id = cv('cmbItem'); if (id > 0) bindItemUoms(id, 'cmbouteruom', 'CmbRateUomOutPut'); }
    /** Getweight:3217. */
    function outputWeight() {
        var eq = uomEq(cv('cmbouteruom')), q = num(val('txtOuterqty'));
        if (eq > 0 && q > 0) setVal('txtweight', eq * q);
    }
    /** OutPutAmountCalculations:3237. */
    function outputAmountCalc() {
        setVal('txtItemAmountOutPut', '0');
        var w = num(val('txtweight')), eq = uomEq(cv('CmbRateUomOutPut')), rate = num(val('txtItemRateOutPut'));
        if (w > 0 && cv('CmbRateUomOutPut') > 0 && rate > 0 && eq > 0) setVal('txtItemAmountOutPut', w / eq * rate);
    }
    function outerUomChanged() { outputWeight(); outputAmountCalc(); }
    /** txtPrctOfInputforOutputQty_TextChanged:3183. */
    function pctChanged() {
        var p = parseFloat(val('txtPrctOfInputforOutputQty'));
        if (isNaN(p) || p <= 0) return;
        if (p > 100) { p = 100; setVal('txtPrctOfInputforOutputQty', '100'); }
        if (!inputs.length) { box('Input Data Not found...'); return; }
        var outW = inputs.reduce(function (a, r) { return a + num(r.Weight); }, 0) * (p / 100);
        var eq = uomEq(cv('cmbouteruom'));
        setVal('txtOuterqty', eq > 0 ? Math.round(outW / eq) : 0);
    }
    /** FormValidationDetailOutPut:3106. */
    function outputFieldsOk() {
        if (cv('CmbEntryTypeOP') === 0) { box('EntryType Field is Required'); focusEl('CmbEntryTypeOP'); return false; }
        if (cv('cmbWareHouseOutPut') === 0) { box('WareHouse is Required'); focusEl('cmbWareHouseOutPut'); return false; }
        if (cv('cmbJobLotOutput') === 0) { box('JobLot is Required'); focusEl('cmbJobLotOutput'); return false; }
        if (cv('cmbItem') === 0) { box('Item Name Field is Required'); focusEl('cmbItem'); return false; }
        if (cv('cmbCropYearOutput') === 0) { box('CropYear is Required'); focusEl('cmbCropYearOutput'); return false; }
        if (cv('cmbPackingType') === 0) { box('Packing Type Field is Required'); focusEl('cmbPackingType'); return false; }
        if (num(val('txtOuterqty')) <= 0) { box('Outer Qty Field is Required'); focusEl('txtOuterqty'); return false; }
        if (cv('cmbouteruom') === 0) { box('Outer UOM Field is Required'); focusEl('cmbouteruom'); return false; }
        if (num(val('txtweight')) <= 0) { box('Weight is Required'); focusEl('txtweight'); return false; }
        var et = selText('CmbEntryTypeOP');
        if (!withoutRates && num(val('txtItemRateOutPut')) === 0 && et !== 'AutoRate') { box('ItemRate Field is Required'); focusEl('txtItemRateOutPut'); return false; }
        if (!withoutRates && cv('CmbRateUomOutPut') === 0) { box('Item Rate Uom Field is Required'); focusEl('CmbRateUomOutPut'); return false; }
        if (!withoutRates && num(val('txtItemAmountOutPut')) === 0 && et !== 'AutoRate') { box('ItemAmount Field is Required'); focusEl('txtItemAmountOutPut'); return false; }
        return true;
    }
    function outputFromForm(id) {
        return {
            Id: id, EntryTypeId: cv('CmbEntryTypeOP'), EntryType: selText('CmbEntryTypeOP'), WareHouseId: cv('cmbWareHouseOutPut'), WareHouseName: selText('cmbWareHouseOutPut').trim(),
            JobLotId: cv('cmbJobLotOutput'), JobLotDescription: selText('cmbJobLotOutput').trim(), ItemId: cv('cmbItem'), ItemName: selText('cmbItem').trim(),
            CropYearId: cv('cmbCropYearOutput'), CropYear: selText('cmbCropYearOutput').trim(), PackingTypeId: cv('cmbPackingType'), PackingTypeName: selText('cmbPackingType').trim(),
            QtyOuter: num(val('txtOuterqty')), OuterPackSizeId: cv('cmbouteruom'), OuterPackSizeName: selText('cmbouteruom').trim(), EquivalentPack: uomEq(cv('cmbouteruom')),
            Weight: num(val('txtweight')), ItemRate: num(val('txtItemRateOutPut')), RateUomId: cv('CmbRateUomOutPut'), RateUom: selText('CmbRateUomOutPut'),
            EquivalentRate: withoutRates ? 0 : uomEq(cv('CmbRateUomOutPut')), ItemAmount: num(val('txtItemAmountOutPut')),
            OHExpense: 0, PMExpense: 0, OHGeneralExpense: 0, PMGeneralExpense: 0, NetRate: num(val('txtItemRateOutPut')), NetAmount: num(val('txtItemAmountOutPut')),
            Remarks: val('txtremarks').trim()
        };
    }
    /** btnplus_Click:3305. */
    function addOutput() {
        if (!outputFieldsOk()) return;
        outputs.push(outputFromForm(0));
        resetOutputEntry();
        insertPmInOutput(); insertOhInOutput();
        renderOutputs();
    }
    /** ResetDetailOutPut:3082. */
    function resetOutputEntry() {
        edit.output = -1;
        clearCombo('cmbItem'); setVal('txtOuterqty', ''); clearCombo('cmbouteruom'); setVal('txtweight', ''); setVal('txtremarks', '');
        setVal('txtItemRateOutPut', ''); clearCombo('CmbRateUomOutPut'); setVal('txtItemAmountOutPut', '');
        focusEl('CmbEntryTypeOP');
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
    }
    /** grdPlannedOutput_DoubleClick:3326. */
    function editOutput(i) {
        var r = outputs[i]; if (!r) return;
        var go = function () {
            edit.output = i;
            setCombo('CmbEntryTypeOP', r.EntryTypeId); setCombo('cmbWareHouseOutPut', r.WareHouseId); setCombo('cmbJobLotOutput', r.JobLotId);
            setCombo('cmbItem', r.ItemId); bindItemUoms(r.ItemId, 'cmbouteruom', 'CmbRateUomOutPut');
            setCombo('cmbCropYearOutput', r.CropYearId); setCombo('cmbPackingType', r.PackingTypeId); setVal('txtOuterqty', r.QtyOuter);
            setCombo('cmbouteruom', r.OuterPackSizeId); setVal('txtweight', r.Weight); setVal('txtItemRateOutPut', r.ItemRate);
            setCombo('CmbRateUomOutPut', r.RateUomId); setVal('txtItemAmountOutPut', r.ItemAmount); setVal('txtremarks', r.Remarks);
            show('btnplus', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
            suppressEntryType = true; setCombo('cmbentrytype', 2); suppressEntryType = false;          // Rows[1].Activate (Output)
            renderOutputs();
        };
        if (intOf(r.Id) > 0) {
            getJson(api + '/referenced' + qs({ jobOrderId: recId, productionDocType: 112 })).then(function (d) {
                if (intOf(d.count) > 0) { box('Record Not Update Because Record has exist in Production OutPut'); return; }
                go();
            }).catch(fail);
        } else go();
    }
    /** btnUpdateDetail_Click:3391. */
    function updateOutput() {
        if (!outputFieldsOk()) return;
        var r = outputs[edit.output]; if (!r) return;
        var f = outputFromForm(r.Id);
        ['EntryTypeId', 'EntryType', 'WareHouseId', 'WareHouseName', 'JobLotId', 'JobLotDescription', 'ItemId', 'ItemName', 'CropYearId', 'CropYear', 'PackingTypeId', 'PackingTypeName',
         'QtyOuter', 'OuterPackSizeId', 'OuterPackSizeName', 'EquivalentPack', 'Weight', 'ItemRate', 'RateUomId', 'RateUom', 'EquivalentRate', 'ItemAmount', 'Remarks'].forEach(function (k) { r[k] = f[k]; });
        resetOutputEntry();
        insertOhInOutput(); insertPmInOutput();
        renderOutputs();
    }
    function cancelOutput() { resetOutputEntry(); renderOutputs(); }
    /** OutputDetailRowDelete:3531. */
    function deleteOutput(i) {
        var r = outputs[i]; if (!r) return;
        if (edit.output !== -1) { box('Reset Detail First'); return; }
        if (intOf(r.Id) > 0) {
            if (!ask('Are you sure to Delete?')) return;
            var d = Object.assign({}, r); d.ActionTypeId = 3; removedOutputs.push(d);
        }
        outputs.splice(i, 1); current.output = -1;
        insertPmInOutput(); insertOhInOutput();
        runCalculations();
    }

    /* ================================================================== INPUT (Input Detail) */

    /** cmbItemInput_Leave:2265 — pack / rate UOMs, InPutRateBind, lblStockWeight. */
    function inputItemLeave() {
        var id = cv('cmbItemInput');
        if (id > 0) { bindItemUoms(id, 'cmbpacksizeinput', 'CmbRateUomInPut'); inputRateBind(); }
        getJson(api + '/stock-weight' + qs({ itemId: id, planDate: val('txtPlanDate') })).then(function (d) {
            var s = num(d && d.balWeight);
            $id('lblStockWeight').textContent = s > 0 ? fmt(s, 3) : '0';
        }).catch(function () { $id('lblStockWeight').textContent = '0'; });
    }
    /** InPutRateBind:2862 — AvgRateOnlyForCGS(item, PlanDate, 401, RECID, JobLot, CropYearId, null, 0, Branch); Rate UOM = Equivalent 40, else 1. */
    function inputRateBind() {
        if (withoutRates) return;
        var itemId = cv('cmbItemInput');
        if (itemId <= 0) { setVal('txtRateInPut', '0'); clearCombo('CmbRateUomInPut'); return; }
        getJson(api + '/avg-rate' + qs({ itemId: itemId, planDate: val('txtPlanDate'), recId: recId, jobLotId: cv('cmbjoblotinput'), cropYearId: cv('cmbcropyearinput') })).then(function (d) {
            var rate = num(d && d.rate);
            if (rate > 0) {
                var list = uomsOf(itemId), rateUom = 1;
                var row = list.filter(function (u) { return num(u.Equivalent) === 40; })[0] || list.filter(function (u) { return num(u.Equivalent) === 1; })[0];
                if (row) { rateUom = num(row.Equivalent); setCombo('CmbRateUomInPut', row.Id); }
                setVal('txtRateInPut', (rate * rateUom).toFixed(2));
            } else { setVal('txtRateInPut', '0'); clearCombo('CmbRateUomInPut'); }
            inputAmountCalc();
        }).catch(fail);
    }
    /** GetweightInput:2773. */
    function inputWeight() { var eq = uomEq(cv('cmbpacksizeinput')), q = num(val('txtqtyinput')); if (eq > 0 && q > 0) setVal('txtweightinput', eq * q); }
    /** InPutAmountCalculations:2793. */
    function inputAmountCalc() {
        if (withoutRates) return;
        setVal('txtAmountInPut', '0');
        var w = num(val('txtweightinput')), eq = uomEq(cv('CmbRateUomInPut')), rate = num(val('txtRateInPut'));
        if (w > 0 && cv('CmbRateUomInPut') > 0 && rate > 0 && eq > 0) setVal('txtAmountInPut', w / eq * rate);
    }
    function packSizeChanged() { inputWeight(); inputAmountCalc(); }
    /** FormValidationDetailInPut:2340. */
    function inputFieldsOk() {
        if (cv('cmbwarehouseinput') === 0) { box('Warehouse is Required'); focusEl('cmbwarehouseinput'); return false; }
        if (cv('cmbjoblotinput') === 0) { box('Job Lot Field is Required'); focusEl('cmbjoblotinput'); return false; }
        if (cv('cmbItemInput') === 0) { box('Item Name Field is Required'); focusEl('cmbItemInput'); return false; }
        if (cv('cmbcropyearinput') === 0) { box('Crop Year Field is Required'); focusEl('cmbcropyearinput'); return false; }
        if (cv('cmbPackTypeInput') === 0) { box('PackType Input is Required'); focusEl('cmbPackTypeInput'); return false; }
        var q = val('txtqtyinput').trim();
        if (q === '' || q === '0') { box('Qty Field is Required'); focusEl('txtqtyinput'); return false; }
        if (cv('cmbpacksizeinput') === 0) { box('Pack Size Field is Required'); focusEl('cmbpacksizeinput'); return false; }
        var w = val('txtweightinput').trim();
        if (w === '' || w === '0') { box('Weight Field is Required'); focusEl('txtweightinput'); return false; }
        if (!withoutRates && num(val('txtRateInPut')) <= 0) { box('ItemRate Field is Required'); focusEl('txtRateInPut'); return false; }
        if (!withoutRates && cv('CmbRateUomInPut') === 0) { box('RateUom Field is Required'); focusEl('cmbpacksizeinput'); return false; }
        if (!withoutRates && num(val('txtAmountInPut')) <= 0) { box('ItemAmount Field is Required'); focusEl('txtAmountInPut'); return false; }
        var stock = num($id('lblStockWeight').textContent);
        if (checkStock && stock <= 0 && val('txtremarksinput') === '') { box('Remarks Field is Required'); focusEl('txtremarksinput'); return false; }
        return true;
    }
    /** btnadddetailinputgrd_Click:2519 — the stock warning first, then the field checks. */
    function stockWarningOk() {
        var stock = num($id('lblStockWeight').textContent);
        if (checkStock && stock <= 0 && val('txtremarksinput') === '') {
            return ask('No stock is available for this item. Do you want to continue adding it?');
        }
        return true;
    }
    function inputFromForm() {
        return {
            WareHouseId: cv('cmbwarehouseinput'), WareHouse: selText('cmbwarehouseinput').trim(), JobLotId: cv('cmbjoblotinput'), JobLot: selText('cmbjoblotinput').trim(),
            ItemId: cv('cmbItemInput'), ItemName: selText('cmbItemInput').trim(), CropYearId: cv('cmbcropyearinput'), CropYear: selText('cmbcropyearinput').trim(),
            PackTypeId: cv('cmbPackTypeInput'), PackType: selText('cmbPackTypeInput').trim(), Qty: num(val('txtqtyinput')), PackSizeId: cv('cmbpacksizeinput'),
            PackSize: selText('cmbpacksizeinput').trim(), PackSizeEquivalent: uomEq(cv('cmbpacksizeinput')), Weight: num(val('txtweightinput')),
            ItemRate: num(val('txtRateInPut')), RateUomId: cv('CmbRateUomInPut'), RateUom: selText('CmbRateUomInPut'), RateUomEquivalent: cv('CmbRateUomInPut') ? uomEq(cv('CmbRateUomInPut')) : 0,
            ItemAmount: num(val('txtAmountInPut')), Remarks: val('txtremarksinput').trim()
        };
    }
    function addInput() {
        if (!stockWarningOk() || !inputFieldsOk()) return;
        var f = inputFromForm();
        inputs.push(Object.assign({ Id: 0, RefDocumentTypeId: 0, RefDocIdNo: 0, RefDocSubIdNo: 0, DocDate: today(), DocNo: 0, BalanceQty: f.Qty, BalanceWeight: f.Weight }, f));
        resetInputEntry();
        renderInputs();
    }
    /** ResetDetailInPut:2418. */
    function resetInputEntry() {
        edit.input = -1;
        focusEl('cmbwarehouseinput');
        clearCombo('cmbItemInput'); clearCombo('cmbpacksizeinput'); setVal('txtqtyinput', ''); setVal('txtweightinput', ''); setVal('txtremarksinput', '');
        setVal('txtRateInPut', ''); clearCombo('CmbRateUomInPut'); setVal('txtAmountInPut', '');
        show('btnadddetailinputgrd', true); show('btnupdategrdinput', false); show('btncancelgrdinput', false);
    }
    /** grdInputDetail_DoubleClick:2539. */
    function editInput(i) {
        var r = inputs[i]; if (!r) return;
        getJson(api + '/referenced' + qs({ jobOrderId: recId, productionDocType: 80 })).then(function (d) {
            if (intOf(d.count) > 0) { box('Record Not Update Because Record has exist in Production InPut'); return; }
            if (intOf(r.RefDocumentTypeId) > 0) { box('Record cannot be edit because this record against invoices loader'); return; }
            itemDetailFill();
            edit.input = i;
            setCombo('cmbwarehouseinput', r.WareHouseId); setCombo('cmbjoblotinput', r.JobLotId); setCombo('cmbItemInput', r.ItemId);
            inputItemLeave();
            setCombo('cmbcropyearinput', r.CropYearId);
            bindItemUoms(r.ItemId, 'cmbpacksizeinput', 'CmbRateUomInPut');
            setVal('txtqtyinput', r.Qty); setCombo('cmbpacksizeinput', r.PackSizeId); setVal('txtweightinput', r.Weight); setCombo('cmbPackTypeInput', r.PackTypeId);
            setVal('txtRateInPut', r.ItemRate); setCombo('CmbRateUomInPut', r.RateUomId); setVal('txtAmountInPut', r.ItemAmount); setVal('txtremarksinput', r.Remarks);
            show('btnadddetailinputgrd', false); show('btnupdategrdinput', true); show('btncancelgrdinput', true);
            suppressEntryType = true; setCombo('cmbentrytype', 1); suppressEntryType = false;          // Rows[0].Activate (Input)
            renderInputs();
        }).catch(fail);
    }
    /** btnupdategrdinput_Click:2594. */
    function updateInput() {
        if (!stockWarningOk() || !inputFieldsOk()) return;
        var r = inputs[edit.input]; if (!r) return;
        Object.assign(r, inputFromForm());
        resetInputEntry();
        renderInputs();
    }
    function cancelInput() { resetInputEntry(); renderInputs(); }
    /** DeleteInputdetailRow:2713. */
    function deleteInput(i) {
        var r = inputs[i]; if (!r) return;
        if (edit.input !== -1) { box('Reset Detail First'); return; }
        if (intOf(r.Id) > 0) {
            if (!ask('Are you sure to Delete?')) return;
            var d = Object.assign({}, r); d.ActionTypeId = 3; removedInputs.push(d);
        }
        inputs.splice(i, 1); current.input = -1;
        runCalculations();
    }
    /** grdInputDetail_CellUpdated:2647 — Qty → Weight, Amount; Weight → Amount (zero refused). */
    function inputCellChanged(i, key, value) {
        var r = inputs[i]; if (!r) return;
        var prev = num(r.ItemAmount);
        if (key === 'Qty') {
            r.Qty = num(value);
            r.Weight = r.Qty * num(r.PackSizeEquivalent);
            r.ItemAmount = num(r.RateUomEquivalent) ? r.Weight / num(r.RateUomEquivalent) * num(r.ItemRate) : 0;
        } else if (key === 'Weight') {
            var w = num(value);
            var amt = num(r.RateUomEquivalent) ? w / num(r.RateUomEquivalent) * num(r.ItemRate) : 0;
            if (amt === 0) { r.ItemAmount = prev; renderInputs(); box('Amount cannot be equal to zero please check'); return; }
            r.Weight = w; r.ItemAmount = amt;
        }
        renderInputs();
    }
    /** BtnGenerateInPutRates_Click:2918. */
    function generateInputRates() {
        if (!inputs.length) { box('Input Grid is Empty'); return Promise.resolve(); }
        var planDate = val('txtPlanDate');
        return inputs.reduce(function (p, r) {
            return p.then(function () {
                return getJson(api + '/avg-rate' + qs({ itemId: r.ItemId, planDate: planDate, recId: recId, jobLotId: r.JobLotId, cropYearId: 0, cropYear: r.CropYear })).then(function (d) {
                    var rate = num(d && d.rate);
                    if (rate > 0) {
                        var list = uomsOf(intOf(r.ItemId)), rateUom = 1;
                        var row = list.filter(function (u) { return num(u.Equivalent) === 40; })[0] || list.filter(function (u) { return num(u.Equivalent) === 1; })[0];
                        if (row) { rateUom = num(row.Equivalent); r.RateUomId = row.Id; r.RateUom = row.UOMCode; r.RateUomEquivalent = rateUom; }
                        r.ItemRate = rate * rateUom;
                        r.ItemAmount = rateUom > 0 ? num(r.Weight) / rateUom * r.ItemRate : 0;
                    }
                });
            });
        }, Promise.resolve()).then(function () { renderInputs(); runCalculations(); }).catch(fail);
    }

    /* ================================================================== OVERHEADS */

    /** GetBrandItems:3724 — the Output grid's items (finished only for the Lab Standard picker). */
    function brandItems(onlyFinished) {
        var seen = {}, out = [];
        outputs.forEach(function (r) {
            if (onlyFinished && intOf(r.EntryTypeId) !== 2) return;
            if (seen[r.ItemId]) return; seen[r.ItemId] = 1;              // distinct for the picker
            out.push({ Id: r.ItemId, Name: r.ItemName });
        });
        return out;
    }
    /** BrandItemBindOnOhandPM:3741 — BindAndRetainSelection, no default row, no row activated. */
    function brandItemBindOnOhAndPm() {
        var list = brandItems(false);
        fill('CmbBrandNameOverHead', list, false); fill('CmbBrandNamePackingMaterial', list, false);
    }
    /** BrandItemBindonLabStandard:3754. */
    function brandItemBindOnLabStandard() {
        var list = brandItems(true);
        fill('CmbBrandNameLabStandard', list, false);
        if (list.length) labBrandLeave();
    }
    /** BindBrandItemsUOMFromOutPutGrid:3770 / ...PackingMaterialGrid:4157 — the brand's outer UOMs from the Output grid. */
    function brandUoms(brandId) {
        var seen = {}, out = [];
        outputs.forEach(function (r) {
            if (intOf(r.ItemId) !== brandId || seen[r.OuterPackSizeId]) return;
            seen[r.OuterPackSizeId] = 1;
            out.push({ Id: r.OuterPackSizeId, Name: r.OuterPackSizeName });
        });
        return out;
    }
    function bindBrandUoms(brandCombo, uomCombo) {
        var list = outputs.length ? brandUoms(cv(brandCombo)) : [];
        if (list.length) fill(uomCombo, list, false); else { $id(uomCombo).innerHTML = '<option value=""></option>'; }
    }
    /** GetBrandQtyByBrandUomAndBrnadUom:6302 — Σ Convert.ToInt32(QtyOuter) of the matching Output rows. */
    function brandQty(brandId, brandUomId) {
        if (!outputs.length) return 0;
        return outputs.reduce(function (a, r) {
            return a + ((brandId === intOf(r.ItemId) && (brandUomId === 0 || brandUomId === intOf(r.OuterPackSizeId))) ? cInt(r.QtyOuter) : 0);
        }, 0);
    }
    function ohCalcQty() { var q = brandQty(cv('CmbBrandNameOverHead'), cv('CmbBrandUomOverHead')); setVal('txtItemQtyOverHead', q !== 0 ? q : 0); }
    /** CaluculatonOfOverHeads:4063 — Math.Round(qty * rate). */
    function ohCalcAmount() {
        if (val('txtItemQtyOverHead') !== '' && val('txtItemRateOverHeads') !== '') setVal('txtExpAmountOverHead', Math.round(num(val('txtItemQtyOverHead')) * num(val('txtItemRateOverHeads'))));
    }
    function ohBrandLeave() { bindBrandUoms('CmbBrandNameOverHead', 'CmbBrandUomOverHead'); ohCalcAmount(); ohCalcQty(); }
    function ohBrandUomLeave() { ohCalcAmount(); ohCalcQty(); }
    /** FormValidationOfDetailPortionOverheads:3808. */
    function ohFieldsOk() {
        if (cv('cmbChartofAccountOverHead') === 0) { box('OverHead Account Field Required'); focusEl('cmbChartofAccountOverHead'); return false; }
        var brandSel = $id('CmbBrandNameOverHead').selectedIndex > 0 || $id('CmbBrandNameOverHead').value !== '';
        if (brandSel && cv('CmbBrandNameOverHead') === 0 && cv('CmbBrandUomOverHead') === 0) { box('outPut ItemUOM Field Required'); focusEl('CmbBrandUomOverHead'); return false; }
        if (!withoutRates && (val('txtExpAmountOverHead').trim() === '' || num(val('txtExpAmountOverHead')) === 0)) { box('Amount Field Required'); focusEl('txtExpAmountOverHead'); return false; }
        return true;
    }
    function ohFromForm() {
        return { ChartOfAccountId: cv('cmbChartofAccountOverHead'), ChartOfAccount: selText('cmbChartofAccountOverHead').trim(), BrandId: cv('CmbBrandNameOverHead'), Brand: selText('CmbBrandNameOverHead').trim(),
                 BrandUomId: cv('CmbBrandUomOverHead'), BrandUom: selText('CmbBrandUomOverHead').trim(), ItemQty: num(val('txtItemQtyOverHead')), ItemRate: num(val('txtItemRateOverHeads')),
                 Amount: num(val('txtExpAmountOverHead')), Remarks: val('txtLedgerRemarksOverHead').trim() };
    }
    function addOverhead() {
        if (!ohFieldsOk()) return;
        overheads.push(Object.assign({ Id: 0 }, ohFromForm()));
        resetOhEntry(); renderOverheads(); focusEl('cmbChartofAccountOverHead');
        insertOhInOutput();
    }
    /** ResetdetailOverHead:3831. */
    function resetOhEntry() {
        edit.oh = -1;
        ['txtLedgerRemarksOverHead', 'txtItemQtyOverHead', 'txtItemRateOverHeads', 'txtExpAmountOverHead'].forEach(function (id) { setVal(id, ''); });
        clearCombo('CmbBrandNameOverHead'); clearCombo('CmbBrandUomOverHead'); clearCombo('cmbChartofAccountOverHead');
        show('btnAddOH', true); show('btnUpdateOH', false); show('BtnCancelOH', false);
    }
    /** grdOverHead_DoubleClick:3945. */
    function editOverhead(i) {
        var r = overheads[i]; if (!r) return;
        edit.oh = i;
        setCombo('cmbChartofAccountOverHead', r.ChartOfAccountId); setCombo('CmbBrandNameOverHead', r.BrandId);
        bindBrandUoms('CmbBrandNameOverHead', 'CmbBrandUomOverHead'); setCombo('CmbBrandUomOverHead', r.BrandUomId);
        setVal('txtItemQtyOverHead', r.ItemQty); setVal('txtItemRateOverHeads', r.ItemRate); setVal('txtExpAmountOverHead', r.Amount); setVal('txtLedgerRemarksOverHead', r.Remarks);
        show('btnAddOH', false); show('btnUpdateOH', true); show('BtnCancelOH', true);
        focusEl('cmbChartofAccountOverHead');
        renderOverheads();
    }
    function updateOverhead() {
        if (!ohFieldsOk()) return;
        var r = overheads[edit.oh]; if (!r) return;
        Object.assign(r, ohFromForm());
        resetOhEntry(); renderOverheads(); insertOhInOutput();
    }
    function cancelOverhead() { resetOhEntry(); renderOverheads(); }
    /** OverHeadDetailRowDelete:3992. */
    function deleteOverhead(i) {
        var r = overheads[i]; if (!r) return;
        if (edit.oh !== -1) { box('Reset Detail First'); return; }
        if (intOf(r.Id) > 0) {
            if (!ask('Are you sure to Delete?')) return;
            var d = Object.assign({}, r); d.ActionTypeId = 3; removedOverheads.push(d);
        }
        overheads.splice(i, 1); current.oh = -1;
        insertOhInOutput();
        runCalculations();
    }

    /* ================================================================== PACKING MATERIAL */

    function pmCalcQty() { var q = brandQty(cv('CmbBrandNamePackingMaterial'), cv('CmbBrandUomPackingMaterial')); setVal('txtPackingMaterialQTY', q !== 0 ? q : 0); }
    /** CaluculatonofPackingMaterial:4482. */
    function pmCalcAmount() {
        if (val('txtPackingMaterialQTY') !== '' && val('txtPackingMaterialRate') !== '') setVal('txtAmountPackingMaterial', Math.round(num(val('txtPackingMaterialRate')) * num(val('txtPackingMaterialQTY'))));
    }
    function pmBrandLeave() { bindBrandUoms('CmbBrandNamePackingMaterial', 'CmbBrandUomPackingMaterial'); pmCalcQty(); }
    function pmBrandUomLeave() { pmCalcQty(); }
    /** FormValidationOfDetailPortionPackingMaterial:4198. */
    function pmFieldsOk() {
        if (cv('CmbWarehousePackingMaterial') === 0) { box('Warehouse Field Required'); focusEl('CmbWarehousePackingMaterial'); return false; }
        if (cv('cmbItemPackingMaterial') === 0) { box('Packing Material Item Field Required'); focusEl('cmbItemPackingMaterial'); return false; }
        var brandSel = $id('CmbBrandNamePackingMaterial').value !== '';
        if (brandSel && cv('CmbBrandNamePackingMaterial') === 0 && cv('CmbBrandUomPackingMaterial') === 0) { box('outPut ItemUOM Field Required'); focusEl('CmbBrandUomPackingMaterial'); return false; }
        if (!withoutRates && (val('txtPackingMaterialRate').trim() === '' || num(val('txtPackingMaterialRate')) === 0)) { box('Rate Field Required'); focusEl('txtPackingMaterialRate'); return false; }
        if (!withoutRates && (val('txtAmountPackingMaterial').trim() === '' || num(val('txtAmountPackingMaterial')) === 0)) { box('Amount Field Required'); focusEl('txtAmountPackingMaterial'); return false; }
        return true;
    }
    function pmFromForm() {
        return { WarehouseId: cv('CmbWarehousePackingMaterial'), Warehouse: selText('CmbWarehousePackingMaterial').trim(), PackingItemId: cv('cmbItemPackingMaterial'), PackingItem: selText('cmbItemPackingMaterial').trim(),
                 BrandId: cv('CmbBrandNamePackingMaterial'), Brand: selText('CmbBrandNamePackingMaterial').trim(), BrandUomId: cv('CmbBrandUomPackingMaterial'), BrandUom: selText('CmbBrandUomPackingMaterial').trim(),
                 ItemQty: num(val('txtPackingMaterialQTY')), ItemRate: num(val('txtPackingMaterialRate')), Amount: num(val('txtAmountPackingMaterial')), Remarks: val('txtremarksPackingMaterial').trim() };
    }
    function addPacking() {
        if (!pmFieldsOk()) return;
        packing.push(Object.assign({ Id: 0 }, pmFromForm()));
        resetPmEntry(); renderPacking(); focusEl('CmbWarehousePackingMaterial');
        insertPmInOutput();
    }
    /** ResetdetailPackingMaterial:4233. */
    function resetPmEntry() {
        edit.pm = -1;
        ['txtremarksPackingMaterial', 'txtPackingMaterialQTY', 'txtPackingMaterialRate', 'txtAmountPackingMaterial'].forEach(function (id) { setVal(id, ''); });
        clearCombo('CmbBrandNamePackingMaterial'); clearCombo('CmbBrandUomPackingMaterial'); clearCombo('cmbItemPackingMaterial');
        show('btnAddDetailPm', true); show('btnUpdateDetailPm', false); show('btnCancelDetailPm', false);
    }
    /** grdPackingMaterial_DoubleClick:4310. */
    function editPacking(i) {
        var r = packing[i]; if (!r) return;
        edit.pm = i;
        setCombo('CmbWarehousePackingMaterial', r.WarehouseId); setCombo('cmbItemPackingMaterial', r.PackingItemId); setCombo('CmbBrandNamePackingMaterial', r.BrandId);
        bindBrandUoms('CmbBrandNamePackingMaterial', 'CmbBrandUomPackingMaterial'); setCombo('CmbBrandUomPackingMaterial', r.BrandUomId);
        setVal('txtPackingMaterialQTY', r.ItemQty); setVal('txtPackingMaterialRate', r.ItemRate); setVal('txtAmountPackingMaterial', r.Amount); setVal('txtremarksPackingMaterial', r.Remarks);
        show('btnAddDetailPm', false); show('btnUpdateDetailPm', true); show('btnCancelDetailPm', true);
        focusEl('CmbWarehousePackingMaterial');
        renderPacking();
    }
    function updatePacking() {
        if (!pmFieldsOk()) return;
        var r = packing[edit.pm]; if (!r) return;
        Object.assign(r, pmFromForm());
        resetPmEntry(); renderPacking(); insertPmInOutput();
    }
    function cancelPacking() { resetPmEntry(); renderPacking(); }
    /** PackingMaterialDetailrowDelete:4398 — the desktop tests the OUTPUT grid's edit index here (:4409); reproduced. */
    function deletePacking(i) {
        var r = packing[i]; if (!r) return;
        if (edit.output !== -1) { box('Reset Detail First'); return; }
        if (intOf(r.Id) > 0) {
            if (!ask('Are you sure to Delete?')) return;
            var d = Object.assign({}, r); d.ActionTypeId = 3; removedPacking.push(d);
        }
        packing.splice(i, 1); current.pm = -1;
        insertPmInOutput();
        runCalculations();
    }

    /* ================================================================== PLANTS */

    /** FormValidationPlantDetail:6542. */
    function plantFieldsOk() {
        if (!selText('cmbPlantFeader').trim()) { box('Plant Feeder Field is Required'); focusEl('cmbPlantFeader'); return false; }
        return true;
    }
    /** btnAddPlantDetail_Click:6509. */
    function addPlant() {
        if (!plantFieldsOk()) return;
        var id = cv('cmbPlantFeader');
        for (var i = 0; i < plants.length; i++) if (intOf(plants[i].PlantId) === id) { box('Same Plant Already Exist In Detail'); return; }
        plants.push({ Id: 0, PlantId: id, PlantName: selText('cmbPlantFeader').trim(), AssumedPlantCapacity: num(val('txtAssumedPlantCapacity')), RemarksDetail: val('txtRemarksPlantDetail').trim() });
        renderPlants();
        clearCombo('cmbPlantFeader'); setVal('txtAssumedPlantCapacity', ''); focusEl('cmbPlantFeader');
    }
    /** grdPlantdetail_DoubleClick:6553 — the desktop hides btnplus (the OUTPUT add button) here, not btnAddPlantDetail; reproduced. */
    function editPlant(i) {
        show('btnplus', false); show('btnUpdatePlantDetail', true); show('btnCanclePlantdetail', true);
        var r = plants[i]; if (!r) return;
        edit.plant = i;
        setCombo('cmbPlantFeader', r.PlantId); setVal('txtAssumedPlantCapacity', r.AssumedPlantCapacity); setVal('txtRemarksPlantDetail', r.RemarksDetail);
        renderPlants();
    }
    /** btnUpdatePlantDetail_Click:6575. */
    function updatePlant() {
        if (!plantFieldsOk()) return;
        var id = cv('cmbPlantFeader');
        for (var i = 0; i < plants.length; i++) if (i !== edit.plant && intOf(plants[i].PlantId) === id) { box('Same Plant Already Exist In Detail'); return; }
        var r = plants[edit.plant];
        if (r) { r.PlantId = id; r.PlantName = selText('cmbPlantFeader').trim(); r.AssumedPlantCapacity = num(val('txtAssumedPlantCapacity')); r.RemarksDetail = val('txtRemarksPlantDetail').trim(); }
        edit.plant = -1;
        show('btnAddPlantDetail', true); show('btnUpdatePlantDetail', false); show('btnCanclePlantdetail', false);
        clearCombo('cmbPlantFeader'); setVal('txtAssumedPlantCapacity', ''); focusEl('cmbPlantFeader');
        renderPlants();
    }
    /** btnCanclePlantdetail_Click:6611. */
    function cancelPlant() {
        edit.plant = -1;
        show('btnAddPlantDetail', true); show('btnUpdatePlantDetail', false); show('btnCanclePlantdetail', false);
        clearCombo('cmbPlantFeader'); focusEl('cmbPlantFeader');
        renderPlants();
    }
    /** grdPlantdetail_ColumnButtonClick:6627. */
    function deletePlant(i) {
        var r = plants[i]; if (!r) return;
        if (!ask('Are you sure to Delete?')) return;
        var drop = function () { plants.splice(i, 1); current.plant = -1; if (edit.plant === i) edit.plant = -1; renderPlants(); };
        if (saveModeActive()) { drop(); return; }
        getJson(api + '/plant-deletable' + qs({ jobOrderId: recId, plantId: r.PlantId, plantName: r.PlantName })).then(function (d) {
            if (d && d.deletable) drop(); else box((d && d.message) || 'That plant has been referred to production.');
        }).catch(fail);
    }

    /* ================================================================== LAB STANDARDS */

    /** SupplierDtFillFromStandard:6858 + BindSupplierName:6892 — Business / Nick name, "Parent / Child" for sub parties. */
    function partyModeChanged() {
        var useNick = $id('RadNickName').checked;
        var all = look.parties || [], byId = {};
        all.forEach(function (p) { byId[p.Id] = p; });
        var list = all.map(function (p) {
            var name = useNick ? p.NickName : p.CompanyName;
            if (p.IsSubSupCust && intOf(p.ParentsSupCustId) > 0 && byId[p.ParentsSupCustId]) {
                var par = byId[p.ParentsSupCustId];
                name = (useNick ? par.NickName : par.CompanyName) + ' / ' + name;
            }
            return { Id: p.Id, Name: name };
        });
        fill('CmbBuyerName', list, false);
    }
    /** CmbBrandNameLabStandard_Leave / CmbBuyerName_Leave:6934 → AnalysisGroupBind:6968. */
    function labBrandLeave() {
        var brand = cv('CmbBrandNameLabStandard'), party = cv('CmbBuyerName'), prev = cv('CmbAnalysisGroup');
        getJson(api + '/analysis-groups' + qs({ brandItemId: brand, supplierCustomerId: party })).then(function (rows) {
            fill('CmbAnalysisGroup', rows || [], false, prev);
            var nv = cv('CmbAnalysisGroup');
            if (nv > 0 && prev > 0 && nv !== prev) labGroupLeave();
        }).catch(fail);
    }
    function labBuyerLeave() { labBrandLeave(); }
    /** GridFillByAnalysisGroup:7009 — rows not in the schedule are removed, missing ones added (Id 0). */
    function labGroupLeave() {
        var groupId = cv('CmbAnalysisGroup');
        if (groupId <= 0) { labRows = []; renderLab(); return; }
        getJson(api + '/analysis-group-rows' + qs({ mainId: groupId })).then(function (list) {
            list = list || [];
            if (!list.length) { box('No data found.'); return; }
            var keys = {};
            list.forEach(function (x) { keys[x.AnalysisGroupId + '_' + x.AnalysisItemId] = 1; });
            labRows = labRows.filter(function (r) { return keys[r.AnalysisGroupId + '_' + r.AnalysisItemId]; });
            var existing = {};
            labRows.forEach(function (r) { existing[r.AnalysisGroupId + '_' + r.AnalysisItemId] = 1; });
            list.forEach(function (x) { if (!existing[x.AnalysisGroupId + '_' + x.AnalysisItemId]) labRows.push(Object.assign({}, x)); });
            renderLab();
        }).catch(function (e) { box('Error:\n' + e.message); });
    }
    /** DeleteCustomerDetailRow:7143. */
    function deleteLab(i) {
        var r = labRows[i]; if (!r) return;
        if (intOf(r.Id) !== 0 && !ask('Are you sure to Delete?')) return;
        labRows.splice(i, 1); current.lab = -1;
        renderLab();
    }

    /* ================================================================== COSTING (btnGenerate_Click:6185) */

    function runCalculations() { costingCalculations(); insertPmInOutput(); insertOhInOutput(); calculateNetRateAndAmount(); renderAll(); }

    /** CostingCalculations:6200. */
    function costingCalculations() {
        var rawQty = 0, rawWeight = 0, rawAmount = 0, bpQty = 0, bpWeight = 0, bpAmount = 0, fgQty = 0, fgWeight = 0, fgAmount = 0, ohAmount = 0, pmAmount = 0, avg = 0, avgWo = 0;
        inputs.forEach(function (r) { rawQty += num(r.Qty); rawWeight += num(r.Weight); rawAmount += num(r.ItemAmount); });
        overheads.forEach(function (r) { ohAmount += num(r.Amount); });
        packing.forEach(function (r) { pmAmount += num(r.Amount); });
        outputs.forEach(function (r) {
            if (intOf(r.EntryTypeId) === 1) { bpQty += num(r.QtyOuter); bpWeight += num(r.Weight); bpAmount += num(r.ItemAmount); }
            else { fgQty += num(r.QtyOuter); fgWeight += num(r.Weight); }
        });
        if (fgWeight > 0) {
            fgAmount = rawAmount + ohAmount + pmAmount - bpAmount;
            avgWo = (rawAmount - bpAmount) / fgWeight;
            avg = (rawAmount + ohAmount + pmAmount - bpAmount) / fgWeight;
        }
        outputs.forEach(function (r) {
            if (intOf(r.EntryTypeId) !== 2) return;
            var rateUom = num(r.EquivalentRate), w = num(r.Weight);
            r.ItemRate = rateUom * avg;
            r.ItemAmount = (w > 0 && rateUom > 0 && num(r.ItemRate) > 0) ? w / rateUom * num(r.ItemRate) : 0;
        });
        setVal('txtInPutTotalQty', fmt0(rawQty)); setVal('txtInPutTotalWeight', fmt0(rawWeight)); setVal('txtTotalAmountInPut', fmt0(rawAmount));
        setVal('txtTotalQtyBP', fmt0(bpQty)); setVal('txtTotalWeightBP', fmt0(bpWeight)); setVal('txtTotalAmountBP', fmt0(bpAmount));
        setVal('txtTotalQtyFG', fmt0(fgQty)); setVal('txtTotalWeightFG', fmt0(fgWeight)); setVal('txtTotalAmountFG', fmt0(fgAmount));
        setVal('txtFGAvgRate', fmt(avg, 3)); setVal('txtFGRateWithoutExp', fmt(avgWo, 3));
        setVal('txtPackingMaterialAmount', fmt0(pmAmount)); setVal('txtOverHeadsAmount', fmt0(ohAmount));
    }
    /** InsertPMInOutPutGrid:6328 / InsertOhInOutPutGrid:6399 — the same distribution for both grids. */
    function distribute(source, expenseKey, generalKey) {
        var byBrand = {}, zero = 0;
        source.forEach(function (r) {
            var k = intOf(r.BrandId) + '_' + intOf(r.BrandUomId);
            byBrand[k] = (byBrand[k] || 0) + num(r.Amount);
            if (intOf(r.BrandId) === 0) zero += num(r.Amount);
        });
        outputs.forEach(function (o) { o[expenseKey] = 0; });
        Object.keys(byBrand).forEach(function (k) {
            var p = k.split('_'), brandId = +p[0], uomId = +p[1], amount = byBrand[k];
            outputs.forEach(function (o) {
                if (intOf(o.ItemId) === brandId && intOf(o.OuterPackSizeId) === uomId) { amount += num(o[expenseKey]); o[expenseKey] = amount; }
            });
        });
        /* "AutoRate" rows share the brand-less amount by weight; this screen's entry types are ByProduct / FinishGoods only. */
        var totalAuto = outputs.reduce(function (a, o) { return a + (o.EntryType === 'AutoRate' ? num(o.Weight) : 0); }, 0);
        outputs.forEach(function (o) { o[generalKey] = o.EntryType === 'AutoRate' ? num(o.Weight) / totalAuto * 100 / 100 * zero : 0; });
    }
    function insertPmInOutput() { distribute(packing, 'PMExpense', 'PMGeneralExpense'); }
    function insertOhInOutput() { distribute(overheads, 'OHExpense', 'OHGeneralExpense'); }
    /** CalculateNEtRateAndamount:6470. */
    function calculateNetRateAndAmount() {
        outputs.forEach(function (o) {
            if (withoutRates) { o.NetAmount = 0; o.NetRate = 0; return; }
            var net = num(o.ItemAmount) + num(o.OHExpense) + num(o.OHGeneralExpense) + num(o.PMExpense) + num(o.PMGeneralExpense);
            o.NetAmount = net;
            o.NetRate = num(o.Weight) ? net / num(o.Weight) * num(o.EquivalentRate) : 0;
        });
    }

    /* ================================================================== SAVE */

    /** FormValidation:1578. */
    function headerOk() {
        var code = val('txtPlanCode').trim();
        if (code === '' || intOf(code) === 0) { box('Plan Code Field Required'); focusEl('txtPlanCode'); return false; }
        var ref = val('txtInvoiceReference');
        if (ref === '' || ref === '0') { box('Manual ReportNo Field Required'); focusEl('txtInvoiceReference'); return false; }
        if (cv('cmbProductionType') === 0) { box('Production Type Field Required'); focusEl('cmbProductionType'); return false; }
        if (cv('CmbWorkInProcess') === 0) { box('WorkInProcess Field Required'); focusEl('CmbWorkInProcess'); return false; }
        if (cv('CmbWipItem') === 0) { box('WIPItem Field Required'); focusEl('CmbWipItem'); return false; }
        if (cv('CmbWipWarehouse') === 0) { box('WIP Warehouse Field Required'); focusEl('CmbWipWarehouse'); return false; }
        if (cv('cmbPlanType') === 0) { box('Plan Type Field Required'); focusEl('cmbPlanType'); return false; }
        if (cv('cmbStatus') === 0) { box('Status Field Required'); focusEl('cmbStatus'); return false; }
        return true;
    }
    function body() {
        return {
            Id: recId, PlanDate: val('txtPlanDate'), PlanCode: intOf(val('txtPlanCode')), InvProductionPlantId: cv('cmbPlantFeader'),
            PlanTypeId: cv('cmbPlanType'), PlanType: selText('cmbPlanType'), RefInvoiceNo: val('txtInvoiceReference'),
            ProductionTypeId: cv('cmbProductionType'), ProductionType: selText('cmbProductionType'),
            WorkInProccessAcId: cv('CmbWorkInProcess'), WipWareHouseId: cv('CmbWipWarehouse'), JobLotId: cv('CmbLotAc'), WipItemId: cv('CmbWipItem'),
            StartDate: val('txtstartDate'), EndDate: val('txtEndDate'), PlanStatusId: cv('cmbStatus'), PlanStatus: selText('cmbStatus'),
            PackingInstructions: val('txtPlantInstruction'), ProductionInsturction: val('txtProductionInstruction'),
            DeliveryInstructions: val('txtDeliveryInstruction'), OtherInstructions: val('txtOtherInstruction'),
            outputs: outputs, inputs: inputs, packing: packing, overheads: overheads, plants: plants,
            lab: { BrandItemId: cv('CmbBrandNameLabStandard'), SupplierCustomerId: cv('CmbBuyerName'), LabStandardMainId: cv('CmbAnalysisGroup'), rows: labRows },
            removedOutputs: removedOutputs, removedInputs: removedInputs, removedPacking: removedPacking, removedOverheads: removedOverheads
        };
    }
    /** Insert():1631. */
    function insert(btnId) {
        if (!outputs.length) { box('OutPut Grid Record Not Found'); return; }
        if (!inputs.length) { box('Input Grid Record Not Found'); return; }
        if (!headerOk()) return;
        if (labRows.length && (cv('CmbAnalysisGroup') === 0 || cv('CmbBrandNameLabStandard') === 0)) {
            box(cv('CmbAnalysisGroup') === 0 ? 'Schedule Name Field Required' : 'Brand Item Field Required');
            tab2('tabLabStandard', true);
            return;
        }
        var wasUpdate = recId > 0;
        if (!ask(wasUpdate ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        runCalculations();                                                                             // :1683
        var rawTotal = num(val('txtTotalAmountInPut')), inputTotal = inputs.reduce(function (a, r) { return a + num(r.ItemAmount); }, 0);
        if (Math.round(rawTotal) !== Math.round(inputTotal) && !ask('Raw Value ' + rawTotal + ' In Header and Total InPut Amount ' + inputTotal + ' are not equal, Are you sure to Proceed?')) return;
        var finishTotal = num(val('txtTotalAmountBP')) + num(val('txtTotalAmountFG')), outTotal = outputs.reduce(function (a, r) { return a + num(r.NetAmount); }, 0);
        if (Math.round(finishTotal) !== Math.round(outTotal) && !ask('Finish Value ' + finishTotal + ' In Header and Total OutPut Amount ' + outTotal + ' are not equal, Are you sure to Proceed?')) return;
        var b = $id(btnId); if (b) b.classList.add('is-busy');
        postJson(api + '/save', body()).then(function (res) {
            box(res.message);
            if (wasUpdate) updateMode = false;
            var printId = intOf(res.id);
            return reset().then(function () { if ($id('chkPrint').checked) printId > 0 ? printJobOrder(printId) : box('Record Id Not Found'); });
        }).catch(fail).then(function () { if (b) b.classList.remove('is-busy'); });
    }
    function save() { recId = 0; insert('btnsave'); }                                                  // btnsave_Click:2059
    function saveAs() { recId = 0; insert('btnSaveAs'); }                                              // btnSaveAs_Click:2213
    function update() {                                                                                // btnUpdate_Click:2072
        if (recId === 0) { box('Record Not Update Because Record Not Found'); return; }
        insert('btnUpdate');
    }

    /* ------------------------------------------------------------------ print (630) */
    /** CommonServices.PrintJobOrder630(id) → 630-PreCostProductionJobOrderSlip.rpt (@DocumentTypeId 401, @BranchesId, @Id). */
    function printJobOrder(id) {
        if (!id) { box('Record Id Not Found'); return; }
        window.printRpt(RPT, { id: id, documentTypeId: 401, branchesId: look.branchId || 0 });
    }
    function print() { printJobOrder(recId); }                                                          // Print_Click:6072

    /* ------------------------------------------------------------------ read */

    /** ReadById:2088. */
    function readById(id) {
        return reset().then(function () { return getJson(api + '/' + id); }).then(function (d) {
            var h = d.header || {};
            recId = id;
            vis = { save: false, update: true, saveAs: false }; applyVisibility();
            updateMode = true;
            tab('tabForm');
            setVal('txtPlanDate', isoOf(h.PlanDate)); setVal('txtPlanCode', h.PlanCode);
            if (intOf(h.InvProductionPlantId) > 0) setCombo('cmbPlantFeader', h.InvProductionPlantId);
            setCombo('CmbWorkInProcess', h.WorkInProccessAcId); setCombo('CmbWipWarehouse', h.WipWareHouseId); setCombo('CmbLotAc', h.JobLotId); setCombo('CmbWipItem', h.WipItemId);
            setByText('cmbPlanType', h.PlanType); setVal('txtInvoiceReference', h.RefInvoiceNo); setByText('cmbProductionType', h.ProductionType);
            setVal('txtstartDate', isoOf(h.StartDate)); setVal('txtEndDate', isoOf(h.EndDate)); setByText('cmbStatus', h.PlanStatus);
            setVal('txtProductionInstruction', h.ProductionInsturction); setVal('txtDeliveryInstruction', h.DeliveryInstructions);
            setVal('txtPlantInstruction', h.PackingInstructions); setVal('txtOtherInstruction', h.OtherInstructions);
            inputs = d.inputs || []; outputs = d.outputs || []; overheads = d.overheads || []; packing = d.packing || []; plants = d.plants || [];
            var lab = d.lab || {};
            labRows = lab.rows || [];
            if (labRows.length) {
                brandItemBindOnLabStandard();
                setCombo('CmbBrandNameLabStandard', lab.BrandItemId); setCombo('CmbBuyerName', lab.SupplierCustomerId);
                getJson(api + '/analysis-groups' + qs({ brandItemId: lab.BrandItemId, supplierCustomerId: lab.SupplierCustomerId })).then(function (rows) {
                    fill('CmbAnalysisGroup', rows || [], false, lab.LabStandardMainId);
                }).catch(fail);
            }
            $id('lblRecId').textContent = 'Record #' + id;
            costingCalculations();                                                                      // :2172
            renderAll();
            tab2(TAB2[tab2Index()]);                                                                    // :2173 tabControl2_SelectedIndexChanged
        }).catch(fail);
    }
    function setByText(id, text) {
        var el = $id(id); if (!el) return;
        for (var i = 0; i < el.options.length; i++) if (el.options[i].text === String(text || '')) { el.selectedIndex = i; return; }
        el.selectedIndex = 0;
    }

    /* ------------------------------------------------------------------ history */

    /** cmbperemeter_ValueChanged:4615. */
    function datePreset() {
        var v = cv('cmbperemeter'), d = new Date();
        if (v === 1) { setVal('FromDateHistory', iso(d)); setVal('ToDateHistory', iso(d)); }
        else if (v === 2) { var w = new Date(); w.setDate(w.getDate() - 7); setVal('FromDateHistory', iso(w)); setVal('ToDateHistory', iso(d)); }
        else if (v === 3) { setVal('FromDateHistory', iso(new Date(d.getUTCFullYear(), d.getUTCMonth(), 1))); setVal('ToDateHistory', iso(d)); }
        else if (v === 4) { setVal('FromDateHistory', iso(new Date(d.getFullYear(), 0, 1))); setVal('ToDateHistory', iso(d)); }
        else if (v === 5) { if (look.financialYearStart) setVal('FromDateHistory', isoOf(look.financialYearStart)); setVal('ToDateHistory', iso(d)); }
    }
    /** btnNewHistory_Click:1550 — ParameterFill (row 0), clear the numbers and the Job Order No. */
    function newHistory() { firstRow('cmbperemeter'); datePreset(); setVal('txtOrderNoFrom', ''); setVal('txtOrderNoTo', ''); clearCombo('xmbjoborderno'); focusEl('cmbperemeter'); }
    /** BtnRefereshHistory_Click:4603 — HistoryJobOrderNofill. */
    function refreshHistory() { getJson(api + '/lookups').then(function (d) { fillIf('xmbjoborderno', d.jobOrderNos, false); }).catch(fail); }
    function dateMode() { var r = document.querySelector('input[name="dateMode"]:checked'); return r ? r.value : 'doc'; }
    var HIST_COLS = [
        { key: 'DocDate', date: fmtDMY, w: 80 }, { key: 'DocNo', w: 70, cls: 'ctr', open: true }, { key: 'PlanType', w: 120 }, { key: 'JobOrderNo', w: 120 }, { key: 'ProductionType', w: 100 },
        { key: 'StartDate', date: fmtDate, w: 80 }, { key: 'EndDate', date: fmtDate, w: 80 }, { key: 'PlanStatus', w: 100 }, { key: 'PackingInstruction', w: 150 }, { key: 'DeliveryInstruction', w: 150 },
        { key: 'ProductionInstruction', w: 150 }, { key: 'OtherInstruction', w: 150 }, { key: 'EntryUser', w: 110 }, { key: 'EntryDate', date: fmtStamp, w: 80 }, { key: 'ModifyUser', w: 110 },
        { key: 'ModifyDate', date: fmtStamp, w: 80 }, { key: 'ApprovedStatus', w: 80 }, { key: 'ApprovedUser', w: 110 }, { key: 'ApprovedDate', date: fmtStamp, w: 80 }, { key: 'NoOfAttachments', w: 80, link: true }
    ];
    /** BindHistoryGrid:4679. */
    function history() {
        getJson(api + '/history' + qs({ dateMode: dateMode(), fromDate: val('FromDateHistory'), toDate: val('ToDateHistory'),
            docNoFrom: intOf(val('txtOrderNoFrom')) || '', docNoTo: intOf(val('txtOrderNoTo')) || '', jobOrderId: cv('xmbjoborderno') || '' })).then(function (rows) {
            historyRows = rows || []; historyCurrent = -1;
            $id('lblHistoryScope').textContent = rights.viewAll ? 'All users' : 'Your records only';
            renderHistory();
            if (!historyRows.length) { ['grdInputHistory', 'grdOutPutHistory', 'grdOverheadHistory', 'grdPackingMaterialHistory'].forEach(function (g) { renderGrid(g, [], [], {}); }); }   // :4774
            else selectHistory(0);
        }).catch(fail);
    }
    function renderHistory() {
        var t = $id('DataGridHistory');
        t.tHead.innerHTML = '<tr><th style="width:50px">Edit</th><th style="width:60px">SaveAs</th><th style="width:50px">Print</th>'
            + HIST_COLS.map(function (c) { return '<th style="min-width:' + c.w + 'px">' + esc(c.key) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = historyRows.length ? historyRows.map(function (r, i) {
            return '<tr tabindex="0" data-i="' + i + '" class="' + (i === historyCurrent ? 'is-current' : '') + '">'
                + '<td class="ctr"><button type="button" class="pc-cellbtn" data-act="edit">Edit</button></td>'
                + '<td class="ctr"><button type="button" class="pc-cellbtn" data-act="saveas">SaveAs</button></td>'
                + '<td class="ctr"><button type="button" class="pc-cellbtn" data-act="print">Print</button></td>'
                + HIST_COLS.map(function (c) {
                    var v = r[c.key];
                    if (c.link) return '<td class="num"><button type="button" class="pc-link" data-act="attach">' + esc(v === null || v === undefined ? '0' : v) + '</button></td>';
                    if (c.date) return '<td>' + esc(c.date(v)) + '</td>';
                    return '<td' + (c.cls ? ' class="' + c.cls + '"' : '') + '>' + esc(v) + '</td>';
                }).join('') + '</tr>';
        }).join('') : '<tr class="pc-empty"><td colspan="' + (HIST_COLS.length + 3) + '">No records</td></tr>';
        $id('lblHistoryCount').textContent = historyRows.length ? historyRows.length + ' record(s)' : '';
    }
    /** DataGridHistory_SelectionChanged:4931 — GetByID → the four detail grids. */
    function selectHistory(i) {
        historyCurrent = i;
        $id('DataGridHistory').tBodies[0].querySelectorAll('tr[data-i]').forEach(function (tr) { tr.classList.toggle('is-current', +tr.getAttribute('data-i') === i); });
        var r = historyRows[i]; if (!r) return;
        getJson(api + '/' + r.Id).then(function (d) {
            renderGrid('grdInputHistory', [{ key: 'DocDate', date: fmtDMY, w: 80 }, { key: 'DocNo', w: 70 }, { key: 'WareHouse', w: 120 }, { key: 'JobLot', w: 120 }, { key: 'ItemName', w: 120 }, { key: 'CropYear', w: 90 },
                { key: 'PackType', w: 100 }, { key: 'Qty', w: 70, num: true, sum: true }, { key: 'PackSize', w: 70 }, { key: 'Weight', w: 80, num: true, sum: true }, { key: 'ItemRate', w: 70, num: true },
                { key: 'RateUom', w: 70 }, { key: 'ItemAmount', w: 90, num: true, sum: true }, { key: 'Remarks', w: 120 }], d.inputs || [], {});
            renderGrid('grdOutPutHistory', outputCols().map(function (c) { var x = Object.assign({}, c); x.hidden = false; return x; }), d.outputs || [], {});
            renderGrid('grdOverheadHistory', OH_COLS, d.overheads || [], {});
            renderGrid('grdPackingMaterialHistory', [{ key: 'Warehouse', w: 150 }, { key: 'PackingItem', w: 150 }, { key: 'Brand', w: 180 }, { key: 'BrandUom', w: 70 }, { key: 'ItemQty', w: 70, num: true, sum: true },
                { key: 'ItemRate', w: 70, num: true }, { key: 'Amount', w: 90, num: true, sum: true }, { key: 'Remarks', w: 150 }], d.packing || [], {});
        }).catch(fail);
    }
    /** DataGridHistory_ColumnButtonClick:4869 / _DoubleClick:4851 / _LinkClicked:4914 / _SaveAs:4898. */
    function historyAction(i, action) {
        var r = historyRows[i]; if (!r) return;
        selectHistory(i);
        var id = intOf(r.Id);
        if (action === 'edit' || action === 'dbl') return readById(id);
        if (action === 'print') return printJobOrder(id);
        if (action === 'attach') return openAttachments(id);
        if (action === 'saveas') {
            return readById(id).then(function () {
                return generateInputRates().then(function () { vis = { save: false, update: false, saveAs: true }; applyVisibility(); $id('lblRecId').textContent = 'Copy of #' + id; });
            });
        }
    }
    function openAttachments(id) {
        $id('attachBody').innerHTML = '<tr class="pc-empty"><td>Loading...</td></tr>';
        $id('attachModal').classList.add('is-open');
        getJson(api + '/attachments?id=' + id).then(function (rows) {
            rows = rows || [];
            $id('attachBody').innerHTML = rows.length ? rows.map(function (r) { return '<tr><td>' + esc(r.Attachment) + '</td></tr>'; }).join('') : '<tr class="pc-empty"><td>No attachments</td></tr>';
        }).catch(function (e) { $id('attachBody').innerHTML = ''; fail(e); });
    }
    function closeAttachments() { $id('attachModal').classList.remove('is-open'); }

    /* ------------------------------------------------------------------ Load Invoices (LoadavailableTransactionsForIssuance) */

    function openLoader() {
        loaderOpen = true; loaderRows = [];
        $id('loaderModal').classList.add('is-open');
        renderLoader();
        getJson(api + '/loader/lookups').then(function (d) {
            var L = (d && d.lists) || {};
            fill('ldWarehouse', L.Warehouse, false); fill('ldItem', L.Items, false); fill('ldJobLot', L.JobLot, false); fill('ldCropYear', L.CropYear, false);
            fill('ldDocType', L.DocumentType, false); fill('ldParty', L.Supplier_Customer, false); fill('ldParentCategory', L.ParentCategories, false);
            fill('ldItemCategory', L.ItemCategories, false); fill('ldItemType', L.ItemTypes, false);
            setVal('ldFromDate', isoOf(d.financialYearStart) || today()); setVal('ldToDate', today());
            return loaderSearch();
        }).catch(fail);
    }
    function closeLoader() { loaderOpen = false; $id('loaderModal').classList.remove('is-open'); }
    var LD_COLS = [{ key: 'RefDocumentType', w: 90 }, { key: 'DocDate', date: fmtDMY, w: 80 }, { key: 'DocNo', w: 60 }, { key: 'ManualNo', w: 70 }, { key: 'SupplierCustomerName', w: 140 }, { key: 'WareHouse', w: 90 },
                   { key: 'JobLotCode', w: 90 }, { key: 'ItemName', w: 140 }, { key: 'CropYear', w: 70 }, { key: 'PackTypeCode', w: 70 }, { key: 'PackUom', w: 60 },
                   { key: 'QtyBalance', num: true, w: 70 }, { key: 'WeightBalance', num: true, w: 80 }, { key: 'ItemRate', num: true, w: 70 }, { key: 'RateUOM', w: 60 }, { key: 'ItemAmount', num: true, w: 90 }];
    function renderLoader() {
        var t = $id('grdLoader');
        t.tHead.innerHTML = '<tr><th class="col-x"><input type="checkbox" id="ldAll"></th>' + LD_COLS.map(function (c) { return '<th style="min-width:' + c.w + 'px">' + c.key + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = loaderRows.length ? loaderRows.map(function (r, i) {
            return '<tr><td class="col-x"><input type="checkbox" data-c="' + i + '"></td>' + LD_COLS.map(function (c) {
                var v = r[c.key];
                if (c.date) return '<td>' + esc(c.date(v)) + '</td>';
                if (c.num) return '<td class="num">' + esc(fmt(v, 3)) + '</td>';
                return '<td>' + esc(v) + '</td>';
            }).join('') + '</tr>';
        }).join('') : '<tr class="pc-empty"><td colspan="' + (LD_COLS.length + 1) + '">No records</td></tr>';
        $id('lblLoaderCount').textContent = loaderRows.length ? loaderRows.length + ' record(s)' : '';
        var all = $id('ldAll');
        if (all) all.onchange = function () { t.tBodies[0].querySelectorAll('[data-c]').forEach(function (c) { c.checked = all.checked; }); };
    }
    function loaderSearch() {
        return getJson(api + '/loader/search' + qs({ fromDate: val('ldFromDate'), toDate: val('ldToDate'), warehouseId: cv('ldWarehouse') || '', itemId: cv('ldItem') || '',
            jobLotId: cv('ldJobLot') || '', cropYear: selText('ldCropYear'), refDocumentTypeId: cv('ldDocType') || '', supplierCustomerId: cv('ldParty') || '',
            parentCategory: cv('ldParentCategory') || '', itemCategoryId: cv('ldItemCategory') || '', itemTypeId: cv('ldItemType') || '' })).then(function (rows) {
            loaderRows = rows || []; renderLoader();
        }).catch(fail);
    }
    /** LoadDataDetailfromPurchaseInvoivce:6111 — a row already in the grid (same Ref keys) gets its balances added; others are appended. */
    function loaderLoad() {
        var picked = [];
        $id('grdLoader').tBodies[0].querySelectorAll('[data-c]:checked').forEach(function (c) { picked.push(loaderRows[+c.getAttribute('data-c')]); });
        if (!picked.length) { box('Check the row first'); return; }
        picked.forEach(function (p) {
            var found = false;
            inputs.forEach(function (r) {
                if (intOf(r.RefDocumentTypeId) === intOf(p.RefDocumentTypeId) && intOf(r.RefDocIdNo) === intOf(p.RefDocIdNo) && intOf(r.RefDocSubIdNo) === intOf(p.RefDocSubIdNo)) {
                    r.BalanceWeight = num(r.BalanceWeight) + num(p.WeightBalance);
                    r.BalanceQty = num(r.BalanceQty) + num(p.QtyBalance);
                    found = true;
                }
            });
            if (!found) {
                inputs.push({ Id: 0, RefDocumentTypeId: p.RefDocumentTypeId, RefDocIdNo: p.RefDocIdNo, RefDocSubIdNo: p.RefDocSubIdNo, DocDate: p.DocDate, DocNo: p.DocNo,
                    WareHouseId: p.WareHouseId, WareHouse: p.WareHouse, JobLotId: p.JobLotId, JobLot: p.JobLotCode, ItemId: p.ItemId, ItemName: p.ItemName, CropYearId: p.CropYearId, CropYear: p.CropYear,
                    PackTypeId: p.PackingTypeId, PackType: p.PackTypeCode, BalanceQty: num(p.QtyBalance), BalanceWeight: num(p.WeightBalance), Qty: num(p.QtyBalance), PackSizeId: p.ItemUOMId,
                    PackSize: p.PackUom, PackSizeEquivalent: num(p.ItemUOM), Weight: num(p.WeightBalance), ItemRate: num(p.ItemRate), RateUomId: p.RateUOMId, RateUom: p.RateUOM,
                    RateUomEquivalent: num(p.Equivalent), ItemAmount: num(p.ItemAmount), Remarks: '' });
            }
        });
        renderInputs();
        closeLoader();
    }

    /* ------------------------------------------------------------------ shortcut keys */

    function shortcutKeys() {
        box(['Ctrl+S  For Save When on Entry form and For Show Data when on History Form', 'Ctrl+U  For Update', 'Ctrl+E  For Close', 'Ctrl+R  For Refresh', 'Ctrl+N  For New',
            'Ctrl+P  For Print', 'Ctrl+L  For Load Input', 'Ctrl+F5  For Focus on Doc Date', 'Ctrl+F10  For Open Attachments', 'F1  For Combo Lookup on Current Column of any Focused Grid',
            'Ctrl+T  For Tab Transfer', 'Ctrl+alt  To Show ShortCut Keys Form', 'Ctrl+D  For Adding an row in Focused Grid', 'Ctrl+Delete  For Deleting an row of Focused Grid',
            'Ctrl+ArrowDown  For Focus On Detail Grid', 'Ctrl+ArrowUp  For Focus On on Doc Date', 'Ctrl+ArrowRight  to change focus from one grid to another',
            'Ctrl+Enter  When Focus On Any Grid For Update Record', 'Ctrl+Space  When Focus On Any Grid To Call Function\'s On Button Or Link'].join('\n'));
    }
    function focusNext(from) {
        var all = Array.prototype.filter.call(document.querySelectorAll('input, select, textarea, button, [tabindex="0"]'), function (el) {
            return !el.disabled && el.tabIndex >= 0 && el.offsetParent !== null && !(el.tagName === 'SELECT' && el.classList.contains('dtcombo') && getComputedStyle(el).display === 'none');
        });
        var i = all.indexOf(from);
        if (i >= 0 && i + 1 < all.length) all[i + 1].focus();
    }
    /** frmProductionJobOrder_KeyDown:5674. */
    function onKey(e) {
        var t = e.target, k = e.key, ctrl = e.ctrlKey || e.metaKey;
        if (k === 'Escape') {
            if (e.defaultPrevented) return;
            if (loaderOpen) { closeLoader(); return; }
            if ($id('attachModal').classList.contains('is-open')) { closeAttachments(); return; }
            window.location.href = '/production';
            return;
        }
        if (k === 'Enter' && !ctrl) {
            if (e.defaultPrevented || !t || t.tagName === 'BUTTON' || t.tagName === 'TEXTAREA' || t.tagName === 'TR') return;
            if (t.closest && t.closest('.cx-modal')) return;
            e.preventDefault(); focusNext(t); return;
        }
        if (ctrl && e.altKey) { shortcutKeys(); return; }
        if (!ctrl) return;
        var key = k.length === 1 ? k.toLowerCase() : k, onHistory = currentTab() === 1;
        if (key === 't') { e.preventDefault(); tab(onHistory ? 'tabForm' : 'tabHistory'); return; }
        if (key === 'e') { e.preventDefault(); window.location.href = '/production'; return; }
        if (!onHistory) {
            if (key === 's') { e.preventDefault(); if (vis.save && rights.save) save(); return; }
            if (key === 'u') { e.preventDefault(); if (vis.update && rights.update && updateMode) update(); return; }
            if (key === 'p') { e.preventDefault(); if (rights.print) print(); return; }
            if (key === 'n') { e.preventDefault(); reset(); return; }
            if (key === 'l') { e.preventDefault(); openLoader(); return; }
            if (key === 'r') { e.preventDefault(); refresh(); return; }
            if (key === 'F5') { e.preventDefault(); focusEl('txtPlanDate'); return; }
            if (key === 'F12') { e.preventDefault(); if (vis.saveAs && rights.save) saveAs(); return; }
            if (key === 'ArrowUp') { e.preventDefault(); focusEl('txtPlanDate'); return; }
            if (key === 'ArrowRight') {
                e.preventDefault();
                var i = tab2Index(); tab2(TAB2[(i + 1) % 5]); return;
            }
        } else {
            if (key === 's') { e.preventDefault(); history(); return; }
            if (key === 'ArrowDown') { e.preventDefault(); var tr = $id('DataGridHistory').querySelector('tr[data-i]'); if (tr) tr.focus(); return; }
        }
    }

    /* ------------------------------------------------------------------ wiring */

    function wireInputs() {
        $id('txtOuterqty').addEventListener('input', function () { outputWeight(); outputAmountCalc(); });          // :3264
        $id('txtweight').addEventListener('input', outputAmountCalc);                                               // :6781
        $id('txtItemRateOutPut').addEventListener('input', outputAmountCalc);                                       // :3300
        $id('txtPrctOfInputforOutputQty').addEventListener('input', pctChanged);                                    // :3183
        $id('txtqtyinput').addEventListener('input', function () { inputWeight(); inputAmountCalc(); });            // :2824
        $id('txtweightinput').addEventListener('input', inputAmountCalc);                                            // :6769
        $id('txtRateInPut').addEventListener('input', inputAmountCalc);                                              // :2906
        $id('txtItemQtyOverHead').addEventListener('blur', ohCalcAmount);                                            // :4079
        $id('txtItemRateOverHeads').addEventListener('input', ohCalcAmount);                                         // :4091
        $id('txtPackingMaterialQTY').addEventListener('input', pmCalcAmount);                                        // :4470
        $id('txtPackingMaterialRate').addEventListener('input', pmCalcAmount);                                       // :4498
        /* OnlytextdecimelFunction — digits and one dot. */
        ['txtOuterqty', 'txtPrctOfInputforOutputQty', 'txtItemRateOutPut', 'txtqtyinput', 'txtweightinput', 'txtRateInPut', 'txtItemQtyOverHead', 'txtItemRateOverHeads',
         'txtExpAmountOverHead', 'txtPackingMaterialQTY', 'txtPackingMaterialRate', 'txtAssumedPlantCapacity', 'txtOrderNoFrom', 'txtOrderNoTo'].forEach(function (id) {
            var el = $id(id); if (!el) return;
            var decimals = !(id === 'txtOrderNoFrom' || id === 'txtOrderNoTo');
            el.addEventListener('keypress', function (e) {
                if (e.key.length !== 1 || e.ctrlKey) return;
                if (/[0-9]/.test(e.key)) return;
                if (decimals && e.key === '.' && el.value.indexOf('.') < 0) return;
                e.preventDefault();
            });
        });
        document.addEventListener('keydown', onKey);
    }

    function wireGrids() {
        var grids = [
            ['grdPlannedOutput', 'output', editOutput, deleteOutput], ['grdInputDetail', 'input', editInput, deleteInput], ['grdOverHead', 'oh', editOverhead, deleteOverhead],
            ['grdPackingMaterial', 'pm', editPacking, deletePacking], ['grdPlantdetail', 'plant', editPlant, deletePlant], ['grdLabStandards', 'lab', null, deleteLab]
        ];
        grids.forEach(function (g) {
            var body = $id(g[0]).tBodies[0];
            var rowOf = function (e) { var tr = e.target.closest('tr[data-i]'); return tr ? +tr.getAttribute('data-i') : -1; };
            body.addEventListener('click', function (e) {
                var i = rowOf(e); if (i < 0) return;
                current[g[1]] = i;
                body.querySelectorAll('tr[data-i]').forEach(function (tr) { if (!tr.classList.contains('is-editing')) tr.classList.toggle('is-current', +tr.getAttribute('data-i') === i); });
                if (e.target.closest('button[data-act="del"]')) g[3](i);
            });
            body.addEventListener('dblclick', function (e) { var i = rowOf(e); if (i >= 0 && g[2] && !e.target.closest('button, input')) g[2](i); });
            body.addEventListener('keydown', function (e) {
                var i = rowOf(e); if (i < 0 || !e.ctrlKey) return;
                if (e.key === 'Enter' && g[2]) { e.preventDefault(); g[2](i); }
                else if (e.key === 'Delete' || e.key === ' ') { e.preventDefault(); g[3](i); }
            });
        });
        $id('grdInputDetail').tBodies[0].addEventListener('change', function (e) {
            var t = e.target; if (!t.classList.contains('cell')) return;
            inputCellChanged(+t.getAttribute('data-i'), t.getAttribute('data-k'), t.value);
        });
        $id('grdLabStandards').tBodies[0].addEventListener('change', function (e) {
            var t = e.target, r = labRows[+t.getAttribute('data-i')]; if (!r) return;
            var k = t.getAttribute('data-k');
            if (k === 'Active') r.Active = t.checked; else r[k] = num(t.value);
        });
        var hb = $id('DataGridHistory').tBodies[0];
        hb.addEventListener('click', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            var i = +tr.getAttribute('data-i'), b = e.target.closest('button[data-act]');
            if (b) historyAction(i, b.getAttribute('data-act')); else selectHistory(i);
        });
        hb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('button')) historyAction(+tr.getAttribute('data-i'), 'dbl'); });
    }

    document.addEventListener('DOMContentLoaded', init);

    window.PreCost = {
        newClick: reset, refresh: refresh, save: save, update: update, saveAs: saveAs, print: print, shortcutKeys: shortcutKeys,
        tab: tab, tab2: tab2, tab3: tab3, entryTypeChanged: entryTypeChanged, filterTypeChanged: filterTypeChanged, docNoChanged: docNoChanged,
        outputItemLeave: outputItemLeave, outerUomChanged: outerUomChanged, outputAmountCalc: outputAmountCalc, addOutput: addOutput, updateOutput: updateOutput, cancelOutput: cancelOutput,
        inputItemLeave: inputItemLeave, inputRateBind: inputRateBind, packSizeChanged: packSizeChanged, inputAmountCalc: inputAmountCalc, addInput: addInput, updateInput: updateInput, cancelInput: cancelInput,
        generateInputRates: generateInputRates, runCalculations: runCalculations,
        ohBrandLeave: ohBrandLeave, ohBrandUomLeave: ohBrandUomLeave, addOverhead: addOverhead, updateOverhead: updateOverhead, cancelOverhead: cancelOverhead,
        pmBrandLeave: pmBrandLeave, pmBrandUomLeave: pmBrandUomLeave, addPacking: addPacking, updatePacking: updatePacking, cancelPacking: cancelPacking,
        addPlant: addPlant, updatePlant: updatePlant, cancelPlant: cancelPlant,
        partyModeChanged: partyModeChanged, labBrandLeave: labBrandLeave, labBuyerLeave: labBuyerLeave, labGroupLeave: labGroupLeave,
        datePreset: datePreset, newHistory: newHistory, refreshHistory: refreshHistory, history: history, closeAttachments: closeAttachments,
        openLoader: openLoader, closeLoader: closeLoader, loaderSearch: loaderSearch, loaderLoad: loaderLoad
    };
})();
