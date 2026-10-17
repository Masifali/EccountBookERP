/* ============================================================================================
 * Screen 600 "Production Job Order" (Production Steel) -
 * Architecture.WinApp.Steel.Production.ProductionPreCostingAndJobOrder_St. Line numbers below are
 * that file's. DocumentTypeId 1511.
 *
 * Grids are DataTables on the desktop (dtPlannedOutput :699, dtInputDetail :730, dtOH :758,
 * dtPlantdetail :786); here they are arrays with the same column names. Rows removed in update
 * mode go to Removed*List with ActionTypeId 3 (:4896, :4958, :3006, :5143) and travel with the save.
 * tabPage6 "Packing Material" is removed on load (:654) so PM amounts are always 0 here, exactly as
 * InsertPMInOutPutGrid computes them over an empty grid.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/production/steel/pre-costing-job-order/api';
    var DOC_TYPE = 1511;

    var recId = 0, updateMode = false;
    var vis = { save: true, update: false };
    var rights = { save: false, update: false, print: false, rate: true };
    var rateDecimals = 2, amountDecimals = 0, branchesId = 0;
    var lists = {};
    var outputs = [], inputs = [], ohs = [], plants = [];
    var removed = { output: [], input: [], oh: [], plant: [] };
    var edit = { output: -1, input: -1, oh: -1, plant: -1 };
    var current = { output: -1, input: -1, oh: -1, plant: -1 };
    var historyRows = [], historyCurrent = -1;
    var loaderRows = [];

    /* ------------------------------------------------------------------ dom helpers */
    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function box(m) { window.alert(m); }
    function ask(m) { return window.confirm(m); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function focusEl(id) { var e = $id(id); if (e) { try { e.focus(); } catch (x) { /* ignore */ } } }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function ci(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === name.toLowerCase()) return row[k];
        return '';
    }
    function pick(row, names) {
        for (var i = 0; i < names.length; i++) { var v = ci(row, names[i]); if (v !== '' && v !== null && v !== undefined) return v; }
        return '';
    }
    function num(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function intOf(v) { return Math.trunc(num(v)); }
    function selText(id) { var el = $id(id); if (!el || el.selectedIndex < 0) return ''; var o = el.options[el.selectedIndex]; return (o && o.value !== '') ? o.text : ''; }
    function selEq(id) { var el = $id(id); if (!el || el.selectedIndex < 0) return 0; var o = el.options[el.selectedIndex]; return o ? num(o.getAttribute('data-eq')) : 0; }
    function hasRow(id) { return val(id) !== ''; }

    var MONTHS = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    function parts(v) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || '')); return m ? { y: m[1], M: m[2], d: m[3] } : null; }
    /** "dd-MMM-yyy" (BindHistoryGrid :3589). */
    function fmtDMY(v) { var p = parts(v); return p ? p.d + '-' + MONTHS[+p.M - 1] + '-' + p.y : String(v || ''); }
    function group(s) { return s.replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
    /** "#,#" - integers, blank for 0. */
    function fmtInt(v) { var n = Math.round(num(v)); return n === 0 ? '' : group(String(n)); }
    /** "#,##0.###" / "#,#.##" - up to n decimals. */
    function fmtDec(v, n) {
        var s = num(v).toFixed(n).replace(/0+$/, '').replace(/\.$/, '');
        var p = s.split('.'); p[0] = group(p[0]); return p.join('.');
    }
    /** clsGlobalVariables.DecimalRateFormate "#,#0.<n>" */
    function fmtRate(v) { var p = num(v).toFixed(rateDecimals).split('.'); p[0] = group(p[0]); return p.join('.'); }
    /** clsGlobalVariables.stringFormatsingle "#,##0.<n>" */
    function fmtAmt(v) { var p = num(v).toFixed(amountDecimals).split('.'); p[0] = group(p[0]); return p.join('.'); }
    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function today() { return iso(new Date()); }
    function isoOf(v) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || '')); return m ? m[0] : ''; }

    function busy(btnId, fn) {
        var b = $id(btnId);
        if (b) { if (b.classList.contains('is-busy')) return; b.dataset.wasDisabled = b.disabled ? '1' : ''; b.disabled = true; b.classList.add('is-busy'); }
        var done = function () { if (!b) return; b.classList.remove('is-busy'); b.disabled = b.dataset.wasDisabled === '1'; applyButtons(); };
        var p;
        try { p = fn(); } catch (e) { done(); throw e; }
        if (p && typeof p.then === 'function') p.then(done, done); else done();
        return p;
    }
    function csrf(opt) {
        var t = document.querySelector('meta[name="_csrf"]'), h = document.querySelector('meta[name="_csrf_header"]');
        if (t && h && opt.method && opt.method.toUpperCase() !== 'GET') opt.headers[h.getAttribute('content')] = t.getAttribute('content');
    }
    function request(url, options) {
        var opt = options || {};
        opt.credentials = 'same-origin'; opt.headers = opt.headers || {}; opt.headers.Accept = 'application/json';
        csrf(opt);
        return fetch(url, opt).then(function (r) {
            return r.text().then(function (t) {
                var body = null; try { body = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
                if (!r.ok) throw new Error((body && body.message) || t || ('Request failed (' + r.status + ')'));
                return body;
            });
        });
    }
    function getJson(url) { return request(url, {}); }
    function postJson(url, body) { return request(url, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body) }); }

    /* ------------------------------------------------------------------ pickers */
    /** DDL.BindDDL - the picker keeps its value when it is still in the new list. */
    function fill(id, rows, valueKey, textKey, eqKey) {
        var el = $id(id); if (!el) return;
        var keep = el.value, html = '<option value=""></option>';
        (rows || []).forEach(function (r) {
            var v = ci(r, valueKey), t = ci(r, textKey);
            html += '<option value="' + esc(v) + '"' + (eqKey ? ' data-eq="' + esc(ci(r, eqKey)) + '"' : '') + '>' + esc(t) + '</option>';
        });
        el.innerHTML = html;
        if (keep !== '') el.value = keep;
        if (el.value !== keep) el.value = '';
    }
    function bindIf(id, rows, valueKey, textKey, eqKey) { if (rows && rows.length) fill(id, rows, valueKey, textKey, eqKey); }
    function clearList(id) { fill(id, [], 'Id', 'Name'); }

    function applyButtons() {
        show('btnsave', vis.save); show('btnUpdate', vis.update);
        var s = $id('btnsave'), u = $id('btnUpdate'), p = $id('Print');
        if (!s.classList.contains('is-busy')) s.disabled = !rights.save;
        if (!u.classList.contains('is-busy')) u.disabled = !rights.update;
        if (!p.classList.contains('is-busy')) p.disabled = !rights.print;
        var missing = [];
        if (!rights.save) missing.push('Save'); if (!rights.update) missing.push('Update'); if (!rights.print) missing.push('Print');
        var note = $id('rightsNote');
        note.textContent = missing.length ? 'Your account does not have the ' + missing.join(', ') + ' right on this screen, so those buttons are off.' : '';
        note.classList.toggle('is-hidden', !missing.length);
    }
    function saveModeActive() { return vis.save && rights.save; }   // btnsave.Visible && btnsave.Enabled

    /** frmProductionJobOrder_Load:647 */
    function loadAll() {
        return getJson(api + '/lookups').then(function (d) {
            d = d || {}; lists = d;
            rights.save = !!d.canSave; rights.update = !!d.canUpdate; rights.print = !!d.canPrint; rights.rate = !!d.canRateAndAmount;
            rateDecimals = intOf(d.rateDecimals); amountDecimals = intOf(d.amountDecimals); branchesId = intOf(d.branchesId);
            bindIf('CmbWipItem', d.wipItems, 'Id', 'ItemName');                       // ItemFill:1167
            bindIf('cmbjoblotinput', d.jobLots, 'Id', 'JobLotDescription');           // JobLotBind:4062
            bindIf('cmbJobLotOutput', d.jobLots, 'Id', 'JobLotDescription');
            bindIf('CmbWorkInProcess', d.accounts, 'Id', 'AccountTitle');             // AccountFills:1121
            fill('cmbPlanType', d.planTypes, 'Id', 'Name');                           // PlanTypeBind:4253
            if (intOf(d.planCode) > 0) setVal('txtPlanCode', d.planCode);             // GeneratePlanCode:1137
            bindIf('cmbPlantFeader', d.plants, 'Id', 'Description');                  // PlantFeader:811
            fill('cmbProductionType', d.productionTypes, 'Id', 'Name');               // ProductionTypeBind:4275
            fill('cmbStatus', d.statuses, 'Id', 'Name');                              // StatusBind:4297
            fill('cmbentrytype', d.entryTypes, 'Id', 'Name');                         // EntryTypeFill:1661
            bindIf('cmbwarehouseinput', d.warehouses, 'Id', 'WareHouseName');          // WareHouseFill:4001
            bindIf('cmbWareHouseOutPut', d.warehouses, 'Id', 'WareHouseName');
            bindIf('cmbItem', d.items, 'Id', 'ItemName');                             // ItemDetailFill:4023
            bindIf('cmbItemInput', d.items, 'Id', 'ItemName');
            bindIf('cmbPackingType', d.packingTypes, 'Id', 'PackTypeDesc');           // CmbPackingTypeFill:4234
            bindIf('cmbPackTypeInput', d.packingTypes, 'Id', 'PackTypeDesc');
            if (d.packingTypes && d.packingTypes.length) { setVal('cmbPackingType', ci(d.packingTypes[0], 'Id')); setVal('cmbPackTypeInput', ci(d.packingTypes[0], 'Id')); } // Rows[0].Activate :4243
            fill('cmbFiltersType', d.filterTypes, 'Id', 'Name');                      // FilterTypeFill:836
            fill('CmbEntryTypeOP', d.outputEntryTypes, 'Id', 'Name');                 // EntryTypeOutPut:2250
            if (d.outputEntryTypes && d.outputEntryTypes.length) setVal('CmbEntryTypeOP', ci(d.outputEntryTypes[0], 'Id'));
            bindIf('cmbChartofAccountOverHead', d.overheadAccounts, 'Id', 'AccountTitle'); // ChartOfAccountOverHead:2679
            if (d.statuses && d.statuses.length) setVal('cmbStatus', ci(d.statuses[0], 'Id'));   // :802 Rows[0].Activate
            if (d.entryTypes && d.entryTypes.length) { setVal('cmbentrytype', ci(d.entryTypes[0], 'Id')); entryTypeChanged(); } // :1671
            applyButtons();
            renderAll();
        });
    }

    /** btnRefresh_Click:1250 */
    function refresh() {
        return busy('btnRefresh', function () {
            return getJson(api + '/lookups').then(function (d) {
                d = d || {}; lists = d;
                bindIf('CmbWipItem', d.wipItems, 'Id', 'ItemName');
                bindIf('cmbjoblotinput', d.jobLots, 'Id', 'JobLotDescription'); bindIf('cmbJobLotOutput', d.jobLots, 'Id', 'JobLotDescription');
                bindIf('CmbWorkInProcess', d.accounts, 'Id', 'AccountTitle');
                fill('cmbPlanType', d.planTypes, 'Id', 'Name');
                bindIf('cmbPlantFeader', d.plants, 'Id', 'Description');
                bindIf('cmbwarehouseinput', d.warehouses, 'Id', 'WareHouseName'); bindIf('cmbWareHouseOutPut', d.warehouses, 'Id', 'WareHouseName');
                bindIf('cmbItem', d.items, 'Id', 'ItemName'); bindIf('cmbItemInput', d.items, 'Id', 'ItemName');
                bindIf('cmbPackingType', d.packingTypes, 'Id', 'PackTypeDesc'); bindIf('cmbPackTypeInput', d.packingTypes, 'Id', 'PackTypeDesc');
                fill('cmbFiltersType', d.filterTypes, 'Id', 'Name');
            }).catch(function (e) { box(e.message); });
        });
    }

    /* ------------------------------------------------------------------ tabs */
    function tab(id) {
        ['tabForm', 'tabHistory'].forEach(function (t) { $id(t).classList.toggle('is-active', t === id); });
        $id('tabs1').querySelectorAll('.sj-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === id); });
        if (id === 'tabHistory') loadHistory(50);                      // tabControl1_SelectedIndexChanged:3525
    }
    function currentTab() { return $id('tabHistory').classList.contains('is-active') ? 1 : 0; }
    /** tabControl2_SelectedIndexChanged:4518 */
    function tab2(id) {
        ['tabPage3', 'tabPage4', 'tabPage5', 'tabPage7'].forEach(function (t) { $id(t).classList.toggle('is-active', t === id); });
        $id('tabs2').querySelectorAll('.sj-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === id); });
        if (id === 'tabPage3') { setVal('cmbentrytype', '2'); }
        else if (id === 'tabPage5') { bindBrandItems(); }
        else if (id === 'tabPage7') { focusEl('cmbPlantFeader'); }
    }
    function full(hostId) {
        var h = $id(hostId); if (!h) return;
        var on = !h.classList.contains('sj-full');
        document.querySelectorAll('.sj-full').forEach(function (x) { x.classList.remove('sj-full'); });
        if (on) h.classList.add('sj-full');
    }

    /* ------------------------------------------------------------------ Filters For Item */
    /** cmbentrytype_ValueChanged:1679 */
    function entryTypeChanged() {
        var t = selText('cmbentrytype');
        var panes = ['tabPage3', 'tabPage4', 'tabPage5', 'tabPage7'];
        var target = t === 'Input' ? 'tabPage4' : (t === 'Output' ? 'tabPage3' : null);
        if (target) {
            panes.forEach(function (p) { $id(p).classList.toggle('is-active', p === target); });
            $id('tabs2').querySelectorAll('.sj-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === target); });
        }
        docNoChanged();
    }
    /** cmbFiltersType_Leave:855 */
    function filterTypeChanged() {
        clearList('cmbDocNo');
        var f = selText('cmbFiltersType');
        if (!f) return;
        getJson(api + '/doc-nos?filterType=' + encodeURIComponent(f)).then(function (rows) {
            if (rows && rows.length) fill('cmbDocNo', rows, 'Id', 'Name'); else clearList('cmbDocNo');
        }).catch(function (e) { box(e.message); });
    }
    /** cmbDocNo_Leave:884 - the OUTPUT item list (Output) or the INPUT item list (Input); no filter -> ItemDetailFill. */
    function docNoChanged() {
        var f = selText('cmbFiltersType'), docNo = intOf(val('cmbDocNo')), et = selText('cmbentrytype');
        var target = et === 'Input' ? 'cmbItemInput' : 'cmbItem';
        if (!f || docNo === 0) { bindIf('cmbItem', lists.items, 'Id', 'ItemName'); bindIf('cmbItemInput', lists.items, 'Id', 'ItemName'); return; }
        getJson(api + '/filtered-items?filterType=' + encodeURIComponent(f) + '&docNo=' + docNo + '&entryType=' + encodeURIComponent(et || 'Output'))
            .then(function (rows) { fill(target, rows || [], 'Id', 'Name'); })
            .catch(function (e) { box(e.message); });
    }

    /* ------------------------------------------------------------------ Planned Output (grdPlannedOutput) */
    var OUT_COLS = [
        ['EntryType', null], ['WareHouseName', null], ['JobLotDescription', null], ['ItemName', null], ['PackingTypeName', null],
        ['QtyOuter', function (v) { return fmtDec(v, 2); }], ['OuterPackSizeName', null], ['Weight', function (v) { return fmtDec(v, 2); }],
        ['ItemRate', fmtRate], ['RateUom', null], ['ItemAmount', fmtAmt], ['OHExpense', fmtAmt], ['PMExpense', fmtAmt],
        ['OHGeneralExpense', fmtAmt], ['PMGeneralExpense', fmtAmt], ['NetRate', fmtRate], ['NetAmount', fmtAmt], ['Remarks', null]
    ];
    var OUT_SUM = { QtyOuter: 'sum', Weight: 'sum', ItemAmount: 'sum', OHExpense: 'sum', PMExpense: 'sum', OHGeneralExpense: 'sum', PMGeneralExpense: 'sum', NetAmount: 'sum', ItemRate: 'avg', NetRate: 'avg' };

    function renderTable(headId, bodyId, footId, cols, rows, kind, cur, editing, sums) {
        $id(headId).innerHTML = '<th class="col-x">X</th>' + cols.map(function (c) { return '<th' + (c[1] ? ' class="num"' : '') + '>' + esc(c[0]) + '</th>'; }).join('');
        $id(bodyId).innerHTML = rows.length ? rows.map(function (r, i) {
            return '<tr tabindex="0" data-kind="' + kind + '" data-i="' + i + '" class="' + (i === editing ? 'is-editing' : (i === cur ? 'is-current' : '')) + '">'
                + '<td class="col-x"><button type="button" class="sj-x" tabindex="-1" data-act="del">X</button></td>'
                + cols.map(function (c) { var v = r[c[0]]; return '<td' + (c[1] ? ' class="num"' : '') + '>' + esc(c[1] ? c[1](v) : v) + '</td>'; }).join('') + '</tr>';
        }).join('') : '';
        if (footId) {
            var ft = $id(footId);
            if (!rows.length) { ft.innerHTML = ''; }
            else ft.innerHTML = '<tr><td></td>' + cols.map(function (c) {
                var k = sums[c[0]]; if (!k) return '<td></td>';
                var t = 0; rows.forEach(function (r) { t += num(r[c[0]]); });
                if (k === 'avg') t = t / rows.length;
                return '<td class="num">' + esc(c[1] ? c[1](t) : t) + '</td>';
            }).join('') + '</tr>';
        }
    }
    function renderOutputs() {
        renderTable('outputHead', 'outputBody', 'outputFoot', OUT_COLS, outputs, 'output', current.output, edit.output, OUT_SUM);
        $id('lblOutputCount').textContent = outputs.length;
    }

    /** cmbItem_Leave:4050 -> bindRateUomAndItemPackUom:4079 */
    function outputItemChanged() {
        var itemId = intOf(val('cmbItem'));
        if (!itemId) return;
        getJson(api + '/uom-schedule?itemId=' + itemId).then(function (rows) {
            if (rows && rows.length) { fill('cmbouteruom', rows, 'Id', 'UOMCode', 'Equivalent'); fill('CmbRateUomOutPut', rows, 'Id', 'UOMCode', 'Equivalent'); }
        }).catch(function (e) { box(e.message); });
    }
    /** Getweight:2363 + OutPutAmountCalculations:2387 */
    function outputQtyChanged() {
        if (hasRow('cmbouteruom')) {
            var uom = selEq('cmbouteruom'), qty = num(val('txtOuterqty'));
            setVal('txtweight', (uom > 0 && qty > 0) ? String(uom * qty) : '0');
        }
        outputAmount();
    }
    function outputAmount() {
        setVal('txtItemAmountOutPut', '0');
        if (num(val('txtweight')) > 0 && hasRow('CmbRateUomOutPut')) {
            var rateUom = selEq('CmbRateUomOutPut'), rate = num(val('txtItemRateOutPut')), w = num(val('txtweight'));
            if (w > 0 && rate > 0 && rateUom > 0) setVal('txtItemAmountOutPut', String(w / rateUom * rate));
        }
    }
    /** FormValidationDetailOutPut:2292 */
    function outputOk() {
        if (!hasRow('CmbEntryTypeOP')) { box('EntryType Field is Required'); focusEl('CmbEntryTypeOP'); return false; }
        if (!hasRow('cmbWareHouseOutPut')) { box('WareHouse is Required'); focusEl('cmbWareHouseOutPut'); return false; }
        if (!hasRow('cmbJobLotOutput')) { box('JobLot is Required'); focusEl('cmbJobLotOutput'); return false; }
        if (!hasRow('cmbItem')) { box('Item Name Field is Required'); focusEl('cmbItem'); return false; }
        if (!hasRow('cmbPackingType')) { box('Item Pack Unit Field is Required'); focusEl('cmbPackingType'); return false; }
        var q = val('txtOuterqty').trim(); if (q === '' || q === '0') { box('Outer Qty Field is Required'); focusEl('txtOuterqty'); return false; }
        if (!hasRow('cmbouteruom')) { box('Outer UOM Field is Required'); focusEl('cmbouteruom'); return false; }
        var w = val('txtweight').trim(); if (w === '' || w === '0') { box('Weight is Required'); focusEl('txtweight'); return false; }
        var auto = selText('CmbEntryTypeOP') === 'AutoRate';
        if (num(val('txtItemRateOutPut')) === 0 && !auto) { box('ItemRate Field is Required'); focusEl('txtItemRateOutPut'); return false; }
        if (!hasRow('CmbRateUomOutPut')) { box('Item Rate Uom Field is Required'); focusEl('CmbRateUomOutPut'); return false; }
        if (num(val('txtItemAmountOutPut')) === 0 && !auto) { box('ItemAmount Field is Required'); focusEl('txtItemAmountOutPut'); return false; }
        return true;
    }
    function outputFromForm(r) {
        r.EntryTypeId = intOf(val('CmbEntryTypeOP')); r.EntryType = selText('CmbEntryTypeOP');
        r.WareHouseId = intOf(val('cmbWareHouseOutPut')); r.WareHouseName = selText('cmbWareHouseOutPut').trim();
        r.JobLotId = intOf(val('cmbJobLotOutput')); r.JobLotDescription = selText('cmbJobLotOutput').trim();
        r.ItemId = intOf(val('cmbItem')); r.ItemName = selText('cmbItem').trim();
        r.PackingTypeId = intOf(val('cmbPackingType')); r.PackingTypeName = selText('cmbPackingType').trim();
        r.QtyOuter = num(val('txtOuterqty')); r.OuterPackSizeId = intOf(val('cmbouteruom')); r.OuterPackSizeName = selText('cmbouteruom').trim();
        r.EquivalentPack = selEq('cmbouteruom'); r.Weight = num(val('txtweight'));
        r.ItemRate = num(val('txtItemRateOutPut')); r.RateUomId = intOf(val('CmbRateUomOutPut')); r.RateUom = selText('CmbRateUomOutPut');
        r.EquivalentRate = selEq('CmbRateUomOutPut'); r.ItemAmount = num(val('txtItemAmountOutPut')); r.Remarks = val('txtremarks').trim();
        return r;
    }
    /** btnplus_Click:2455 */
    function addOutput() {
        if (!outputOk()) return;
        var r = outputFromForm({ Id: 0, OHExpense: 0, PMExpense: 0, OHGeneralExpense: 0, PMGeneralExpense: 0 });
        r.NetRate = r.ItemRate; r.NetAmount = r.ItemAmount;
        outputs.push(r);
        resetOutputEntry();
        recalcAll();
    }
    /** grdPlannedOutput_DoubleClick:2478 */
    function editOutput(i) {
        getJson(api + '/ref-check?documentTypeId=112&jobOrderId=' + recId).then(function (d) {
            if (d && d.count > 0) { box(d.message); return; }
            var r = outputs[i]; if (!r) return;
            edit.output = i;
            setVal('CmbEntryTypeOP', r.EntryTypeId); setVal('cmbWareHouseOutPut', r.WareHouseId); setVal('cmbJobLotOutput', r.JobLotId); setVal('cmbItem', r.ItemId);
            return getJson(api + '/uom-schedule?itemId=' + r.ItemId).then(function (rows) {
                if (rows && rows.length) { fill('cmbouteruom', rows, 'Id', 'UOMCode', 'Equivalent'); fill('CmbRateUomOutPut', rows, 'Id', 'UOMCode', 'Equivalent'); }
                setVal('cmbPackingType', r.PackingTypeId); setVal('txtOuterqty', r.QtyOuter); setVal('cmbouteruom', r.OuterPackSizeId);
                setVal('txtweight', r.Weight); setVal('txtItemRateOutPut', r.ItemRate); setVal('CmbRateUomOutPut', r.RateUomId);
                setVal('txtItemAmountOutPut', r.ItemAmount); setVal('txtremarks', r.Remarks);
                show('btnplus', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
                setVal('cmbentrytype', '2');                               // :2518 cmbentrytype.Rows[1].Activate()
                renderOutputs();
            });
        }).catch(function (e) { box(e.message); });
    }
    /** btnUpdateDetail_Click:2542 */
    function updateOutput() {
        if (!outputOk()) return;
        var r = outputs[edit.output]; if (!r) { cancelOutput(); return; }
        outputFromForm(r);
        cancelOutput();
        insertOhInOutputGrid(); insertPmInOutputGrid(); renderOutputs();
    }
    /** btnCancelUpdateDetial_Click:2527 / ResetDetailOutPut:2268 */
    function cancelOutput() { edit.output = -1; resetOutputEntry(); renderOutputs(); }
    function resetOutputEntry() {
        ['cmbItem', 'cmbPackingType', 'txtOuterqty', 'cmbouteruom', 'txtweight', 'txtremarks', 'txtItemRateOutPut', 'CmbRateUomOutPut', 'txtItemAmountOutPut'].forEach(function (id) { setVal(id, ''); });
        focusEl('cmbItem'); show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
    }
    /** grdPlannedOutput_ColumnButtonClick:4858 */
    function deleteOutput(i) {
        var r = outputs[i]; if (!r) return;
        if (!ask('Are you sure to Delete?')) return;
        if (intOf(r.Id) > 0) { var c = JSON.parse(JSON.stringify(r)); c.ActionTypeId = 3; removed.output.push(c); }
        outputs.splice(i, 1); current.output = -1; if (edit.output === i) edit.output = -1; else if (edit.output > i) edit.output--;
        recalcAll();
    }

    /* ------------------------------------------------------------------ Input Detail (grdInputDetail) */
    var IN_COLS = [
        ['WareHouse', null], ['JobLot', null], ['ItemName', null], ['PackType', null],
        ['BalanceQty', function (v) { return fmtDec(v, 3); }], ['BalanceWeight', function (v) { return fmtDec(v, 3); }],
        ['Qty', 'edit3'], ['PackSize', null], ['Weight', 'edit3'], ['ItemRate', fmtRate], ['RateUom', null], ['ItemAmount', fmtAmt], ['Remarks', null]
    ];
    var IN_SUM = { BalanceQty: 'sum', BalanceWeight: 'sum', Qty: 'sum', Weight: 'sum', ItemAmount: 'sum' };
    function renderInputs() {
        $id('inputHead').innerHTML = '<th class="col-x">X</th>' + IN_COLS.map(function (c) { return '<th' + (c[1] ? ' class="num"' : '') + '>' + esc(c[0]) + '</th>'; }).join('');
        $id('inputBody').innerHTML = inputs.length ? inputs.map(function (r, i) {
            return '<tr tabindex="0" data-kind="input" data-i="' + i + '" class="' + (i === edit.input ? 'is-editing' : (i === current.input ? 'is-current' : '')) + '">'
                + '<td class="col-x"><button type="button" class="sj-x" tabindex="-1" data-act="del">X</button></td>'
                + IN_COLS.map(function (c) {
                    var v = r[c[0]];
                    if (c[1] === 'edit3') return '<td class="num"><input type="text" class="sj-cellin" data-col="' + c[0] + '" data-i="' + i + '" value="' + esc(fmtDec(v, 3)) + '"></td>';   // AllowEdit: Qty / Weight (:1848)
                    return '<td' + (c[1] ? ' class="num"' : '') + '>' + esc(c[1] ? c[1](v) : v) + '</td>';
                }).join('') + '</tr>';
        }).join('') : '';
        var ft = $id('inputFoot');
        ft.innerHTML = inputs.length ? '<tr><td></td>' + IN_COLS.map(function (c) {
            if (!IN_SUM[c[0]]) return '<td></td>';
            var t = 0; inputs.forEach(function (r) { t += num(r[c[0]]); });
            return '<td class="num">' + esc(c[0] === 'ItemAmount' ? fmtAmt(t) : fmtDec(t, 3)) + '</td>';
        }).join('') + '</tr>' : '';
        $id('lblInputCount').textContent = inputs.length;
    }
    /** grdInputDetail_CellUpdated:2044 */
    function inputCellUpdated(i, col, value) {
        var r = inputs[i]; if (!r) return;
        if (col === 'Qty') {
            var qty = num(value), uom = num(r.PackSizeEquivalent);
            r.Qty = qty; r.Weight = qty * uom;
            if (num(r.BalanceQty) < qty) { r.Qty = 0; r.Weight = 0; box('Qty cannot greater than BalanceQty Please check!'); }
            r.ItemAmount = (qty * uom) / num(r.RateUomEquivalent) * num(r.ItemRate);
            if (!isFinite(r.ItemAmount)) r.ItemAmount = 0;
        } else if (col === 'Weight') {
            var w = num(value); r.Weight = w;
            if (num(r.BalanceWeight) < w) { r.Weight = 0; box('Weight cannot greater than BalanceWeight Please check!'); }
            r.ItemAmount = w / num(r.RateUomEquivalent) * num(r.ItemRate);
            if (!isFinite(r.ItemAmount)) r.ItemAmount = 0;
        }
        renderInputs();
    }
    /** cmbItemInput_Leave:1700 -> bindPackSizeInput:1713 + GetAvgRate:2183 */
    function inputItemChanged() {
        var itemId = intOf(val('cmbItemInput'));
        var packText = selText('cmbpacksizeinput'), rateText = selText('CmbRateUomInPut');
        if (!itemId) { clearList('cmbpacksizeinput'); clearList('CmbRateUomInPut'); return; }
        getJson(api + '/uom-schedule?itemId=' + itemId).then(function (rows) {
            if (rows && rows.length) {
                fill('cmbpacksizeinput', rows, 'Id', 'UOMCode', 'Equivalent'); fill('CmbRateUomInPut', rows, 'Id', 'UOMCode', 'Equivalent');
                var byText = function (id, t) { var el = $id(id); el.value = ''; for (var k = 0; k < el.options.length; k++) if (el.options[k].text === t && t) { el.value = el.options[k].value; break; } };
                byText('cmbpacksizeinput', packText); byText('CmbRateUomInPut', rateText);
            } else { clearList('cmbpacksizeinput'); clearList('CmbRateUomInPut'); }
            return inputAvgRate();
        }).catch(function (e) { box(e.message); });
    }
    /** GetAvgRate:2183 - AvgRateOnlyForCGS(item, PlanDate, 1511, RECID, joblot, 0, null, warehouse, branch). */
    function inputAvgRate() {
        var itemId = intOf(val('cmbItemInput'));
        if (!itemId) return Promise.resolve();
        return getJson(api + '/avg-rate?itemId=' + itemId + '&docDate=' + encodeURIComponent(val('txtPlanDate')) + '&documentTypeId=1511&recId=' + recId
                + '&jobLotId=' + intOf(val('cmbjoblotinput')) + '&warehouseId=' + intOf(val('cmbwarehouseinput')))
            .then(function (d) {
                if (d && num(d.rate) > 0) {
                    setVal('txtRateInPut', String(num(d.rate) * 1.0));
                    setVal('CmbRateUomInPut', intOf(d.rateUomId) || '');
                    $id('CmbRateUomInPut').disabled = true;
                } else { setVal('txtRateInPut', '0'); $id('CmbRateUomInPut').disabled = false; }
                inputAmount();
            }).catch(function (e) { box(e.message); });
    }
    /** GetweightInput:2094 + InPutAmountCalculations:2118 */
    function inputQtyChanged() {
        if (hasRow('cmbpacksizeinput')) {
            var uom = selEq('cmbpacksizeinput'), qty = num(val('txtqtyinput'));
            setVal('txtweightinput', (uom > 0 && qty > 0) ? String(uom * qty) : '0');
        }
        inputAmount();
    }
    function inputAmount() {
        setVal('txtAmountInPut', '0');
        if (num(val('txtweightinput')) > 0 && hasRow('CmbRateUomInPut')) {
            var rateUom = selEq('CmbRateUomInPut'), rate = num(val('txtRateInPut')), w = num(val('txtweightinput'));
            if (w > 0 && rate > 0 && rateUom > 0) setVal('txtAmountInPut', String(w / rateUom * rate));
        }
    }
    /** FormValidationDetailInPut:1759 */
    function inputOk() {
        if (!hasRow('cmbwarehouseinput')) { box('Warehouse is Required'); focusEl('cmbwarehouseinput'); return false; }
        if (!hasRow('cmbjoblotinput')) { box('Job Lot Field is Required'); focusEl('cmbjoblotinput'); return false; }
        if (!hasRow('cmbItemInput')) { box('Item Name Field is Required'); focusEl('cmbItemInput'); return false; }
        if (!hasRow('cmbPackTypeInput')) { box('PackType Input is Required'); focusEl('cmbPackTypeInput'); return false; }
        var q = val('txtqtyinput').trim(); if (q === '' || q === '0') { box('Qty Field is Required'); focusEl('txtqtyinput'); return false; }
        if (!hasRow('cmbpacksizeinput')) { box('Pack Size Field is Required'); focusEl('cmbpacksizeinput'); return false; }
        var w = val('txtweightinput').trim(); if (w === '' || w === '0') { box('Weight Field is Required'); focusEl('txtweightinput'); return false; }
        if (num(val('txtRateInPut')) === 0) { box('ItemRate Field is Required'); focusEl('txtRateInPut'); return false; }
        if (num(val('txtAmountInPut')) === 0) { box('ItemAmount Field is Required'); focusEl('txtAmountInPut'); return false; }
        return true;
    }
    function inputFromForm(r) {
        r.WareHouseId = intOf(val('cmbwarehouseinput')); r.WareHouse = selText('cmbwarehouseinput').trim();
        r.JobLotId = intOf(val('cmbjoblotinput')); r.JobLot = selText('cmbjoblotinput').trim();
        r.ItemId = intOf(val('cmbItemInput')); r.ItemName = selText('cmbItemInput').trim();
        r.PackTypeId = intOf(val('cmbPackTypeInput')); r.PackType = selText('cmbPackTypeInput').trim();
        r.Qty = num(val('txtqtyinput')); r.PackSizeId = intOf(val('cmbpacksizeinput')); r.PackSize = selText('cmbpacksizeinput').trim();
        r.PackSizeEquivalent = selEq('cmbpacksizeinput'); r.Weight = num(val('txtweightinput'));
        r.ItemRate = num(val('txtRateInPut')); r.RateUomId = intOf(val('CmbRateUomInPut')); r.RateUom = selText('CmbRateUomInPut').trim();
        r.RateUomEquivalent = selEq('CmbRateUomInPut'); r.ItemAmount = num(val('txtAmountInPut')); r.Remarks = val('txtremarksinput').trim();
        return r;
    }
    /** btnadddetailinputgrd_Click:1921 - BalanceQty / BalanceWeight take the typed Qty / Weight. */
    function addInput() {
        if (!inputOk()) return;
        var r = inputFromForm({ Id: 0, RefDocumentTypeId: 0, RefDocIdNo: 0, RefDocSubIdNo: 0 });
        r.BalanceQty = r.Qty; r.BalanceWeight = r.Weight;
        inputs.push(r);
        resetInputEntry(); renderInputs();
    }
    /** grdInputDetail_DoubleClick:1940 */
    function editInput(i) {
        getJson(api + '/ref-check?documentTypeId=80&jobOrderId=' + recId).then(function (d) {
            if (d && d.count > 0) { box(d.message); return; }
            var r = inputs[i]; if (!r) return;
            if (intOf(r.RefDocumentTypeId) > 0) { box('Record cannot be edit because this record against invoices loader'); return; }
            bindIf('cmbItemInput', lists.items, 'Id', 'ItemName');           // ItemDetailFill()
            edit.input = i;
            setVal('cmbwarehouseinput', r.WareHouseId); setVal('cmbjoblotinput', r.JobLotId); setVal('cmbItemInput', r.ItemId);
            return getJson(api + '/uom-schedule?itemId=' + r.ItemId).then(function (rows) {
                if (rows && rows.length) { fill('cmbpacksizeinput', rows, 'Id', 'UOMCode', 'Equivalent'); fill('CmbRateUomInPut', rows, 'Id', 'UOMCode', 'Equivalent'); }
                setVal('txtqtyinput', r.Qty); setVal('cmbpacksizeinput', r.PackSizeId); setVal('txtweightinput', r.Weight); setVal('cmbPackTypeInput', r.PackTypeId);
                setVal('txtRateInPut', r.ItemRate); setVal('CmbRateUomInPut', r.RateUomId); setVal('txtAmountInPut', r.ItemAmount); setVal('txtremarksinput', r.Remarks);
                show('btnadddetailinputgrd', false); show('btnupdategrdinput', true); show('btncancelgrdinput', true);
                setVal('cmbentrytype', '1');                                   // :1984 cmbentrytype.Rows[0].Activate()
                focusEl('cmbwarehouseinput'); renderInputs();
            });
        }).catch(function (e) { box(e.message); });
    }
    /** btnupdategrdinput_Click:1994 */
    function updateInput() {
        if (!inputOk()) return;
        var r = inputs[edit.input]; if (r) inputFromForm(r);
        cancelInput();
    }
    function cancelInput() { edit.input = -1; resetInputEntry(); renderInputs(); }
    /** ResetDetailInPut:1818 */
    function resetInputEntry() {
        ['cmbItemInput', 'cmbjoblotinput', 'cmbpacksizeinput', 'txtqtyinput', 'txtweightinput', 'cmbwarehouseinput', 'txtremarksinput', 'txtRateInPut', 'CmbRateUomInPut', 'txtAmountInPut'].forEach(function (id) { setVal(id, ''); });
        $id('CmbRateUomInPut').disabled = false;
        focusEl('cmbItemInput'); show('btnadddetailinputgrd', true); show('btnupdategrdinput', false); show('btncancelgrdinput', false);
    }
    /** grdInputDetail_ColumnButtonClick:4924 */
    function deleteInput(i) {
        var r = inputs[i]; if (!r) return;
        if (!ask('Are you sure to Delete?')) return;
        if (intOf(r.Id) > 0) { var c = JSON.parse(JSON.stringify(r)); c.ActionTypeId = 3; removed.input.push(c); }
        inputs.splice(i, 1); current.input = -1; if (edit.input === i) edit.input = -1; else if (edit.input > i) edit.input--;
        recalcAll();
    }

    /* ------------------------------------------------------------------ OverHeads (grdOverHead) */
    var OH_COLS = [['ChartOfAccount', null], ['Brand', null], ['BrandUom', 'r'], ['ItemQty', fmtInt], ['ItemRate', function (v) { return fmtDec(v, 2); }], ['Amount', fmtInt], ['Remarks', null]];
    function renderOhs() {
        $id('ohBody').innerHTML = ohs.length ? ohs.map(function (r, i) {
            return '<tr tabindex="0" data-kind="oh" data-i="' + i + '" class="' + (i === edit.oh ? 'is-editing' : (i === current.oh ? 'is-current' : '')) + '">'
                + '<td class="col-x"><button type="button" class="sj-x" tabindex="-1" data-act="del">X</button></td>'
                + OH_COLS.map(function (c) { var v = r[c[0]]; var f = typeof c[1] === 'function' ? c[1] : null; return '<td' + (c[1] ? ' class="num"' : '') + '>' + esc(f ? f(v) : v) + '</td>'; }).join('') + '</tr>';
        }).join('') : '';
        var q = 0, a = 0, rt = 0; ohs.forEach(function (r) { q += num(r.ItemQty); a += num(r.Amount); rt += num(r.ItemRate); });
        $id('ohFoot').innerHTML = ohs.length ? '<tr><td></td><td></td><td></td><td></td><td class="num">' + esc(fmtInt(q)) + '</td><td class="num">' + esc(fmtDec(rt / ohs.length, 2)) + '</td><td class="num">' + esc(fmtInt(a)) + '</td><td></td></tr>' : '';
        $id('lblOHCount').textContent = ohs.length;
    }
    /** BindBrandItemsFromOutPutGrid:2729 - every output row (ItemId, ItemName), as the desktop lists them. */
    function bindBrandItems() {
        var rows = outputs.map(function (r) { return { Id: r.ItemId, Name: r.ItemName }; });
        if (rows.length) fill('CmbBrandNameOverHead', rows, 'Id', 'Name'); else clearList('CmbBrandNameOverHead');
    }
    /** BindBrandItemsUOMFromOutPutGrid:2769 */
    function bindBrandUoms() {
        var brand = intOf(val('CmbBrandNameOverHead'));
        var rows = outputs.filter(function (r) { return intOf(r.ItemId) === brand; }).map(function (r) { return { Id: r.OuterPackSizeId, Name: r.OuterPackSizeName }; });
        if (rows.length) fill('CmbBrandUomOverHead', rows, 'Id', 'Name'); else clearList('CmbBrandUomOverHead');
    }
    /** GetBrandQtyByBrandUomAndBrnadUom:4659 - the sum of the INTEGER part of QtyOuter. */
    function brandQty(brandId, brandUomId) {
        var q = 0;
        outputs.forEach(function (r) { if (brandId === intOf(r.ItemId) && (brandUomId === 0 || brandUomId === intOf(r.OuterPackSizeId))) q += Math.trunc(num(r.QtyOuter)); });
        return q;
    }
    /** CmbBrandNameOverHead_Leave:2709 */
    function ohBrandChanged() { bindBrandUoms(); ohCalc(); ohQty(); }
    /** CmbBrandUomOverHead_Leave:2723 */
    function ohBrandUomChanged() { ohCalc(); ohQty(); }
    /** CalculateQtyOverHeads:3033 */
    function ohQty() {
        var q = brandQty(intOf(val('CmbBrandNameOverHead')), intOf(val('CmbBrandUomOverHead')));
        setVal('txtItemQtyOverHead', q !== 0 ? String(q) : '0');
    }
    /** CaluculatonOfOverHeads:3054 - Math.Round(qty*rate, DefaultNoofDecimalPointsForAmount, AwayFromZero) */
    function ohCalc() {
        var q = num(val('txtItemQtyOverHead')), r = num(val('txtItemRateOverHeads'));
        if (q > 0 && r > 0) { var f = Math.pow(10, amountDecimals); var a = Math.sign(q * r) * Math.round(Math.abs(q * r) * f) / f; setVal('txtExpAmountOverHead', fmtAmt(a)); }
        else setVal('txtExpAmountOverHead', '');
    }
    /** FormValidationOfDetailPortionOverheads:2807 */
    function ohOk() {
        if (!hasRow('cmbChartofAccountOverHead')) { box('OverHead Account Field Required'); focusEl('cmbChartofAccountOverHead'); return false; }
        if (hasRow('CmbBrandNameOverHead') && !hasRow('CmbBrandUomOverHead')) { box('outPut ItemUOM Field Required'); focusEl('CmbBrandUomOverHead'); return false; }
        if (val('txtExpAmountOverHead').trim() === '' || num(val('txtExpAmountOverHead')) === 0) { box('Amount Field Required'); focusEl('txtExpAmountOverHead'); return false; }
        return true;
    }
    function ohFromForm(r) {
        r.ChartOfAccountId = intOf(val('cmbChartofAccountOverHead')); r.ChartOfAccount = selText('cmbChartofAccountOverHead').trim();
        r.BrandId = intOf(val('CmbBrandNameOverHead')); r.Brand = selText('CmbBrandNameOverHead').trim();
        r.BrandUomId = intOf(val('CmbBrandUomOverHead')); r.BrandUom = selText('CmbBrandUomOverHead').trim();
        r.ItemQty = num(val('txtItemQtyOverHead')); r.ItemRate = num(val('txtItemRateOverHeads')); r.Amount = num(val('txtExpAmountOverHead'));
        r.Remarks = val('txtLedgerRemarksOverHead').trim();
        return r;
    }
    /** btnAddOH_Click:2852 */
    function addOH() {
        if (!ohOk()) return;
        ohs.push(ohFromForm({ Id: 0 }));
        resetOhEntry(); focusEl('cmbChartofAccountOverHead');
        costing(); insertOhInOutputGrid(); netRateAndAmount(); renderAll();
    }
    /** grdOverHead_DoubleClick:2949 */
    function editOH(i) {
        var r = ohs[i]; if (!r) return;
        edit.oh = i;
        setVal('cmbChartofAccountOverHead', r.ChartOfAccountId); bindBrandItems(); setVal('CmbBrandNameOverHead', r.BrandId); bindBrandUoms(); setVal('CmbBrandUomOverHead', r.BrandUomId);
        setVal('txtItemQtyOverHead', r.ItemQty); setVal('txtItemRateOverHeads', r.ItemRate); setVal('txtExpAmountOverHead', r.Amount); setVal('txtLedgerRemarksOverHead', r.Remarks);
        show('btnAddOH', false); show('btnUpdateOH', true); show('BtnCancelOH', true); focusEl('cmbChartofAccountOverHead'); renderOhs();
    }
    /** btnUpdateOH_Click:2875 */
    function updateOH() {
        if (!ohOk()) return;
        var r = ohs[edit.oh]; if (r) ohFromForm(r);
        edit.oh = -1; resetOhEntry(); insertOhInOutputGrid(); renderAll();
    }
    function cancelOH() { edit.oh = -1; resetOhEntry(); renderOhs(); }
    /** ResetdetailOverHead:2830 */
    function resetOhEntry() {
        ['txtLedgerRemarksOverHead', 'txtItemQtyOverHead', 'txtItemRateOverHeads', 'txtExpAmountOverHead', 'CmbBrandNameOverHead', 'CmbBrandUomOverHead', 'cmbChartofAccountOverHead'].forEach(function (id) { setVal(id, ''); });
        show('btnAddOH', true); show('btnUpdateOH', false); show('BtnCancelOH', false);
    }
    /** grdOverHead_ColumnButtonClick:2979 */
    function deleteOH(i) {
        var r = ohs[i]; if (!r) return;
        if (!ask('Are you sure to Delete?')) return;
        if (intOf(r.Id) > 0) { var c = JSON.parse(JSON.stringify(r)); c.ActionTypeId = 3; removed.oh.push(c); }
        ohs.splice(i, 1); current.oh = -1; if (edit.oh === i) edit.oh = -1; else if (edit.oh > i) edit.oh--;
        costing(); insertOhInOutputGrid(); netRateAndAmount(); renderAll();
    }

    /* ------------------------------------------------------------------ Plants Detail (grdPlantdetail) */
    function renderPlants() {
        $id('plantBody').innerHTML = plants.length ? plants.map(function (r, i) {
            return '<tr tabindex="0" data-kind="plant" data-i="' + i + '" class="' + (i === edit.plant ? 'is-editing' : (i === current.plant ? 'is-current' : '')) + '">'
                + '<td class="col-x"><button type="button" class="sj-x" tabindex="-1" data-act="del">X</button></td><td>' + esc(r.PlantName) + '</td><td>' + esc(r.RemarksDetail) + '</td></tr>';
        }).join('') : '';
        $id('lblPlantCount').textContent = plants.length;
    }
    /** FormValidationPlantDetail:5017 */
    function plantOk() {
        if (!hasRow('cmbPlantFeader') || !selText('cmbPlantFeader').trim()) { box('Plant Feeder Field is Required'); focusEl('cmbPlantFeader'); return false; }
        return true;
    }
    /** btnAddPlantDetail_Click:4986 */
    function addPlant() {
        if (!plantOk()) return;
        var id = intOf(val('cmbPlantFeader'));
        for (var i = 0; i < plants.length; i++) if (intOf(plants[i].PlantId) === id) { box('Same Plant Already Exist In Detail'); return; }
        plants.push({ Id: 0, PlantId: id, PlantName: selText('cmbPlantFeader').trim(), RemarksDetail: val('txtRemarksPlantDetail').trim() });
        setVal('cmbPlantFeader', ''); renderPlants();
    }
    /** grdPlantdetail_DoubleClick:5028 */
    function editPlant(i) {
        var r = plants[i]; if (!r) return;
        edit.plant = i; show('btnAddPlantDetail', false); show('btnUpdatePlantDetail', true); show('btnCanclePlantdetail', true);
        setVal('cmbPlantFeader', r.PlantId); setVal('txtRemarksPlantDetail', r.RemarksDetail); renderPlants();
    }
    /** btnUpdatePlantDetail_Click:5049 */
    function updatePlant() {
        if (!plantOk()) return;
        var id = intOf(val('cmbPlantFeader'));
        for (var i = 0; i < plants.length; i++) if (i !== edit.plant && intOf(plants[i].PlantId) === id) { box('Same Plant Already Exist In Detail'); return; }
        var r = plants[edit.plant]; if (r) { r.PlantId = id; r.PlantName = selText('cmbPlantFeader').trim(); r.RemarksDetail = val('txtRemarksPlantDetail').trim(); }
        cancelPlant();
    }
    /** btnCanclePlantdetail_Click:5082 */
    function cancelPlant() { edit.plant = -1; show('btnAddPlantDetail', true); show('btnUpdatePlantDetail', false); show('btnCanclePlantdetail', false); setVal('cmbPlantFeader', ''); focusEl('cmbPlantFeader'); renderPlants(); }
    /** grdPlantdetail_ColumnButtonClick:5098 */
    function deletePlant(i) {
        var r = plants[i]; if (!r) return;
        if (!ask('Are you sure to Delete?')) return;
        var drop = function () { plants.splice(i, 1); current.plant = -1; if (edit.plant === i) edit.plant = -1; else if (edit.plant > i) edit.plant--; renderPlants(); };
        if (intOf(r.Id) === 0) { drop(); return; }
        getJson(api + '/plant-deletable?jobOrderId=' + recId + '&plantId=' + r.PlantId + '&plantName=' + encodeURIComponent(r.PlantName || '')).then(function (d) {
            if (d && d.deletable) { var c = JSON.parse(JSON.stringify(r)); c.ActionTypeId = 3; removed.plant.push(c); drop(); }
            else box((d && d.message) || 'That plant has been referred to production.');
        }).catch(function (e) { box(e.message); });
    }

    /* ------------------------------------------------------------------ costing (btnGenerate_Click:4550) */
    function recalcAll() { costing(); insertPmInOutputGrid(); insertOhInOutputGrid(); netRateAndAmount(); renderAll(); }
    function generate() { recalcAll(); }
    /** CostingCalculations:4565 */
    function costing() {
        var rawQty = 0, rawWeight = 0, rawAmount = 0, bpQty = 0, bpWeight = 0, bpAmount = 0, fgQty = 0, fgWeight = 0, fgAmount = 0, ohAmount = 0, pmAmount = 0, avg = 0, avgWo = 0;
        inputs.forEach(function (r) { rawQty += num(r.Qty); rawWeight += num(r.Weight); rawAmount += num(r.ItemAmount); });
        ohs.forEach(function (r) { ohAmount += num(r.Amount); });
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
            if (intOf(r.EntryTypeId) === 2) {
                var rateUom = num(r.EquivalentRate), w = num(r.Weight);
                r.ItemRate = rateUom * avgWo;
                r.ItemAmount = rateUom > 0 ? w / rateUom * num(r.ItemRate) : 0;
            }
        });
        setVal('txtInPutTotalQty', fmtInt(rawQty)); setVal('txtInPutTotalWeight', fmtInt(rawWeight)); setVal('txtTotalAmountInPut', fmtInt(rawAmount));
        setVal('txtTotalQtyBP', fmtInt(bpQty)); setVal('txtTotalWeightBP', fmtInt(bpWeight)); setVal('txtTotalAmountBP', fmtInt(bpAmount));
        setVal('txtTotalQtyFG', fmtInt(fgQty)); setVal('txtTotalWeightFG', fmtInt(fgWeight)); setVal('txtTotalAmountFG', fmtInt(fgAmount));
        setVal('txtFGAvgRate', fmtDec(avg, 3)); setVal('txtFGRateWithoutExp', fmtDec(avgWo, 3));
        setVal('txtPackingMaterialAmount', fmtInt(pmAmount)); setVal('txtOverHeadsAmount', fmtInt(ohAmount));
    }
    /** InsertPMInOutPutGrid:4685 - the Packing Material grid does not exist on this form, so every PM expense is 0. */
    function insertPmInOutputGrid() { outputs.forEach(function (r) { r.PMExpense = 0; r.PMGeneralExpense = 0; }); }
    /** InsertOhInOutPutGrid:4756 */
    function insertOhInOutputGrid() {
        var byBrand = {}, zero = 0;
        ohs.forEach(function (r) {
            var k = intOf(r.BrandId) + '|' + intOf(r.BrandUomId);
            byBrand[k] = (byBrand[k] || 0) + num(r.Amount);
            if (intOf(r.BrandId) === 0) zero += num(r.Amount);
        });
        outputs.forEach(function (r) { r.OHExpense = 0; });
        Object.keys(byBrand).forEach(function (k) {
            var brandId = intOf(k.split('|')[0]), uomId = intOf(k.split('|')[1]), amount = byBrand[k];
            outputs.forEach(function (r) {
                if (intOf(r.ItemId) === brandId && intOf(r.OuterPackSizeId) === uomId) { amount += num(r.OHExpense); r.OHExpense = amount; }
            });
        });
        var total = 0; outputs.forEach(function (r) { if (r.EntryType === 'AutoRate') total += num(r.Weight); });
        outputs.forEach(function (r) {
            if (r.EntryType === 'AutoRate') { var g = num(r.Weight) / total * 100 / 100 * zero; r.OHGeneralExpense = isFinite(g) ? g : 0; }
            else r.OHGeneralExpense = 0;
        });
    }
    /** CalculateNEtRateAndamount:4827 */
    function netRateAndAmount() {
        outputs.forEach(function (r) {
            var net = num(r.ItemAmount) + num(r.OHExpense) + num(r.OHGeneralExpense) + num(r.PMExpense) + num(r.PMGeneralExpense);
            r.NetAmount = net;
            var nr = net / num(r.Weight) * num(r.EquivalentRate);
            r.NetRate = isFinite(nr) ? nr : 0;
        });
    }
    /** BtnGenerateInPutRates_Click:5226 - AvgRateOnlyForCGS(item, PlanDate, 401, RECID, joblot, 0, null, warehouse, branch) per row. */
    function generateInputRates() {
        if (!inputs.length) { box('Input Grid is Empty'); return; }
        return busy('BtnGenerateInPutRates', function () {
            var p = Promise.resolve();
            inputs.forEach(function (r) {
                p = p.then(function () {
                    return getJson(api + '/avg-rate?itemId=' + intOf(r.ItemId) + '&docDate=' + encodeURIComponent(val('txtPlanDate')) + '&documentTypeId=401&recId=' + recId
                            + '&jobLotId=' + intOf(r.JobLotId) + '&warehouseId=' + intOf(r.WareHouseId)).then(function (d) {
                        var rate = num(d && d.rate);
                        if (rate > 0) {
                            var eq = 1;
                            if (intOf(d.rateUomId) > 0) { eq = num(d.rateUomEquivalent); r.RateUomId = intOf(d.rateUomId); r.RateUom = d.rateUom; r.RateUomEquivalent = eq; }
                            else { eq = 0; r.RateUomId = 0; r.RateUom = ''; r.RateUomEquivalent = 0; }
                            var R = rate * eq; r.ItemRate = R;
                            r.ItemAmount = eq > 0 ? num(r.Weight) / eq * R : 0;
                        }
                    });
                });
            });
            return p.then(function () { recalcAll(); }).catch(function (e) { box(e.message); });
        });
    }

    function renderAll() { renderOutputs(); renderInputs(); renderOhs(); renderPlants(); }

    /* ------------------------------------------------------------------ save */
    /** FormValidation:1302 */
    function headerOk() {
        if (val('txtPlanCode').trim() === '' || intOf(val('txtPlanCode')) === 0) { box('Plan Code Field Required'); focusEl('txtPlanCode'); return false; }
        if (val('txtInvoiceReference') === '' || val('txtInvoiceReference') === '0') { box('Mannual ReportNo Field Required'); focusEl('txtInvoiceReference'); return false; }
        if (!hasRow('cmbProductionType')) { box('Production Type Field Required'); focusEl('cmbProductionType'); return false; }
        if (!hasRow('CmbWorkInProcess')) { box('WorkInProcess Field Required'); focusEl('CmbWorkInProcess'); return false; }
        if (!hasRow('CmbWipItem')) { box('WIPItem Field Required'); focusEl('CmbWipItem'); return false; }
        if (!hasRow('cmbPlanType')) { box('Plan Type Field Required'); focusEl('cmbPlanType'); return false; }
        if (!hasRow('cmbStatus')) { box('Status Field Required'); focusEl('cmbStatus'); return false; }
        return true;
    }
    function collect() {
        return {
            Id: recId, DocDate: val('txtPlanDate'), DocNo: intOf(val('txtPlanCode')), PlanType: selText('cmbPlanType'),
            ProductionJobOrderNo: val('txtInvoiceReference'), ProductionType: selText('cmbProductionType'),
            WorkInProccessAcId: intOf(val('CmbWorkInProcess')), WipItemId: intOf(val('CmbWipItem')),
            StartDate: val('txtstartDate'), EndDate: val('txtEndDate'), PlanStatus: selText('cmbStatus'),
            PackingInstructions: val('txtPlantInstruction'), ProductionInsturction: val('txtProductionInstruction'),
            DeliveryInstructions: val('txtDeliveryInstruction'), OtherInstructions: val('txtOtherInstruction'),
            outputs: removed.output.concat(outputs), inputs: removed.input.concat(inputs),
            overheads: removed.oh.concat(ohs), plants: removed.plant.concat(plants)
        };
    }
    /** Insert:1349 */
    function insert(btnId) {
        return busy(btnId, function () {
            if (!headerOk()) return;
            var wasUpdate = recId > 0;
            if (!ask(wasUpdate ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            generate();                                                  // :1380 btnGenerate_Click(null, null)
            if (!outputs.length) { box('OutPut Grid Record Not Found'); return; }
            if (!inputs.length) { box('Input Grid Record Not Found'); return; }
            return postJson(api + '/save', collect()).then(function (d) {
                box((d && d.message) || (wasUpdate ? 'Update Successfully' : 'Save Successfully'));
                if (wasUpdate) updateMode = false;
                var printId = d ? intOf(d.id) : 0;
                reset();
                if ($id('chkPrint').checked && printId > 0) return print(printId, null);
            }).catch(function (e) { box(e.message); });
        });
    }
    function save() { recId = 0; return insert('btnsave'); }                      // btnsave_Click:1632
    function update() { if (recId === 0) { box('Record Not Update Because Record Not Found'); return; } return insert('btnUpdate'); }   // btnUpdate_Click:1645

    /** Reset:1184 */
    function reset() {
        outputs = []; inputs = []; plants = []; ohs = [];
        removed = { output: [], input: [], oh: [], plant: [] }; edit = { output: -1, input: -1, oh: -1, plant: -1 }; current = { output: -1, input: -1, oh: -1, plant: -1 };
        setVal('txtPlanDate', today());
        ['txtInvoiceReference', 'txtOtherInstruction', 'txtPlantInstruction', 'txtProductionInstruction', 'txtDeliveryInstruction', 'CmbWorkInProcess', 'CmbWipItem'].forEach(function (id) { setVal(id, ''); });
        setVal('cmbProductionType', '1'); setVal('cmbStatus', '1'); setVal('cmbPlanType', '1');
        vis.update = false; vis.save = true; updateMode = false; recId = 0; $id('lblRecId').textContent = '';
        ['txtInPutTotalQty', 'txtInPutTotalWeight', 'txtTotalAmountInPut', 'txtTotalQtyBP', 'txtTotalWeightBP', 'txtTotalAmountBP', 'txtTotalQtyFG', 'txtTotalWeightFG', 'txtTotalAmountFG', 'txtFGAvgRate', 'txtFGRateWithoutExp', 'txtPackingMaterialAmount', 'txtOverHeadsAmount'].forEach(function (id) { setVal(id, '0'); });
        resetOutputEntry(); resetInputEntry(); resetOhEntry(); cancelPlant();
        applyButtons(); renderAll(); focusEl('txtPlanDate');
        getJson(api + '/next-code').then(function (d) { if (d && intOf(d.planCode) > 0) setVal('txtPlanCode', d.planCode); }).catch(function (e) { box(e.message); });
    }

    /* ------------------------------------------------------------------ history */
    var HIST_COLS = [['DocDate', fmtDMY], ['DocNo', null], ['PlanType', null], ['JobOrderNo', null], ['ProductionType', null], ['StartDate', fmtDMY], ['EndDate', fmtDMY],
        ['PlanStatus', null], ['PackingInstruction', null], ['DeliveryInstruction', null], ['ProductionInsturction', null], ['OtherInstruction', null], ['NoOfAttachments', 'link']];
    /** BindHistoryGrid:3545 */
    function loadHistory(noOfRecords) {
        return getJson(api + '/history?noOfRecords=' + noOfRecords).then(function (d) {
            historyRows = (d && d.rows) || []; historyCurrent = historyRows.length ? 0 : -1;
            renderHistory();
            if (historyCurrent >= 0) historyDetail(intOf(historyRows[0].Id));
        }).catch(function (e) { historyRows = []; renderHistory(); box(e.message); });
    }
    /** HgridSetting:3609 - Print, Edit frozen in front. */
    function renderHistory() {
        $id('historyHead').innerHTML = '<th style="width:50px;">Print</th><th style="width:50px;">Edit</th>' + HIST_COLS.map(function (c) { return '<th>' + esc(c[0]) + '</th>'; }).join('');
        $id('historyBody').innerHTML = historyRows.length ? historyRows.map(function (r, i) {
            return '<tr tabindex="0" data-kind="history" data-i="' + i + '" class="' + (i === historyCurrent ? 'is-current' : '') + '">'
                + '<td><button type="button" class="sj-cellbtn" tabindex="-1" data-act="print">Print</button></td>'
                + '<td><button type="button" class="sj-cellbtn" tabindex="-1" data-act="edit">Edit</button></td>'
                + HIST_COLS.map(function (c) {
                    var v = r[c[0]];
                    if (c[1] === 'link') return '<td class="num"><button type="button" class="sj-link" tabindex="-1" data-act="attach">' + esc(v === null || v === undefined || v === '' ? '0' : v) + '</button></td>';
                    return '<td>' + esc(c[1] ? c[1](v) : v) + '</td>';
                }).join('') + '</tr>';
        }).join('') : '<tr class="sj-empty"><td colspan="' + (HIST_COLS.length + 2) + '">No records</td></tr>';
        $id('lblHistoryCount').textContent = historyRows.length + ' record(s)';
    }
    /** DataGridHistory_SelectionChanged:3764 - the three detail grids of the selected row. */
    function historyDetail(id) {
        if (!id) return;
        getJson(api + '/' + id).then(function (d) {
            var inCols = [['WareHouse', null], ['JobLot', null], ['ItemName', null], ['PackType', null], ['Qty', fmtInt], ['PackSize', null], ['Weight', fmtInt], ['ItemRate', fmtRate], ['RateUom', null], ['ItemAmount', fmtAmt], ['Remarks', null]];
            simpleTable('hInHead', 'hInBody', 'hInFoot', inCols, d.inputs || [], { Qty: 1, Weight: 1, ItemAmount: 1 });
            var outCols = [['EntryType', null], ['WareHouseName', null], ['JobLotDescription', null], ['ItemName', null], ['PackingTypeName', null], ['QtyOuter', function (v) { return fmtDec(v, 2); }], ['OuterPackSizeName', null],
                ['Weight', function (v) { return fmtDec(v, 2); }], ['ItemRate', fmtRate], ['RateUom', null], ['ItemAmount', fmtAmt], ['OHExpense', fmtAmt], ['PMExpense', fmtAmt], ['OHGeneralExpense', fmtAmt], ['PMGeneralExpense', fmtAmt], ['NetRate', fmtRate], ['NetAmount', fmtAmt], ['Remarks', null]];
            simpleTable('hOutHead', 'hOutBody', 'hOutFoot', outCols, d.outputs || [], { QtyOuter: 1, Weight: 1, ItemAmount: 1, OHExpense: 1, PMExpense: 1, OHGeneralExpense: 1, PMGeneralExpense: 1, NetRate: 1, NetAmount: 1 });
            var ohCols = [['ChartOfAccount', null], ['Brand', null], ['BrandUom', 'r'], ['ItemQty', fmtInt], ['ItemRate', function (v) { return fmtDec(v, 2); }], ['Amount', fmtInt], ['Remarks', null]];
            simpleTable('hOhHead', 'hOhBody', 'hOhFoot', ohCols, d.overheads || [], { ItemQty: 1, Amount: 1 });
        }).catch(function (e) { box(e.message); });
    }
    function simpleTable(headId, bodyId, footId, cols, rows, sums) {
        $id(headId).innerHTML = cols.map(function (c) { return '<th' + (c[1] ? ' class="num"' : '') + '>' + esc(c[0]) + '</th>'; }).join('');
        $id(bodyId).innerHTML = rows.length ? rows.map(function (r) {
            return '<tr>' + cols.map(function (c) { var f = typeof c[1] === 'function' ? c[1] : null; return '<td' + (c[1] ? ' class="num"' : '') + '>' + esc(f ? f(r[c[0]]) : r[c[0]]) + '</td>'; }).join('') + '</tr>';
        }).join('') : '<tr class="sj-empty"><td colspan="' + cols.length + '">No records</td></tr>';
        $id(footId).innerHTML = rows.length ? '<tr>' + cols.map(function (c) {
            if (!sums[c[0]]) return '<td></td>';
            var t = 0; rows.forEach(function (r) { t += num(r[c[0]]); });
            var f = typeof c[1] === 'function' ? c[1] : null;
            return '<td class="num">' + esc(f ? f(t) : t) + '</td>';
        }).join('') + '</tr>' : '';
    }
    /** btnNewHistory_Click:1271 - clears the detail grids. */
    function newHistory() { ['hInBody', 'hOutBody', 'hOhBody', 'hInFoot', 'hOutFoot', 'hOhFoot'].forEach(function (id) { $id(id).innerHTML = ''; }); }

    /** DataGridHistory_DoubleClick:3641 */
    function readById(id) {
        return getJson(api + '/' + id).then(function (d) {
            reset();
            vis.save = false; vis.update = true;
            recId = id;
            var h = d.header || {};
            tab('tabForm');
            setVal('txtPlanDate', isoOf(h.DocDate)); setVal('txtPlanCode', h.DocNo);
            setVal('CmbWorkInProcess', intOf(h.WorkInProccessAcId) || ''); setVal('CmbWipItem', intOf(h.WipItemId) || '');
            setByText('cmbPlanType', h.PlanType); setVal('txtInvoiceReference', h.ProductionJobOrderNo); setByText('cmbProductionType', h.ProductionType);
            setVal('txtstartDate', isoOf(h.StartDate)); setVal('txtEndDate', isoOf(h.EndDate)); setByText('cmbStatus', h.PlanStatus);
            setVal('txtProductionInstruction', h.ProductionInsturction); setVal('txtDeliveryInstruction', h.DeliveryInstructions);
            setVal('txtPlantInstruction', h.PackingInstructions); setVal('txtOtherInstruction', h.OtherInstructions);
            inputs = d.inputs || []; outputs = d.outputs || []; ohs = d.overheads || []; plants = d.plants || [];
            updateMode = true;
            $id('lblRecId').textContent = 'Record #' + id;
            applyButtons(); costing(); renderAll();
        }).catch(function (e) { box(e.message); });
    }
    function setByText(id, text) {
        var el = $id(id); if (!el) return; el.value = '';
        for (var k = 0; k < el.options.length; k++) if (el.options[k].text === String(text || '')) { el.value = el.options[k].value; break; }
    }
    function historyAction(i, action) {
        var r = historyRows[i]; if (!r) return;
        historyCurrent = i; renderHistory();
        var id = intOf(r.Id);
        if (action === 'edit' || action === 'dbl') return readById(id);      // DataGridHistory_ColumnButtonClick:3714
        if (action === 'print') return print(id, null);
        if (action === 'attach') return openAttachments(id);                 // DataGridHistory_LinkClicked:3733
        if (action === 'select') return historyDetail(id);
    }
    function openAttachments(id) {
        $id('attachBody').innerHTML = '<tr class="sj-empty"><td>Loading...</td></tr>';
        $id('attachModal').classList.add('is-open');
        return getJson(api + '/attachments?id=' + id).then(function (rows) {
            rows = rows || [];
            $id('attachBody').innerHTML = rows.length ? rows.map(function (r) { return '<tr><td>' + esc(r.Attachment) + '</td></tr>'; }).join('') : '<tr class="sj-empty"><td>No attachments</td></tr>';
        }).catch(function (e) { $id('attachBody').innerHTML = ''; box(e.message); });
    }
    function closeAttachments() { $id('attachModal').classList.remove('is-open'); }

    /* ------------------------------------------------------------------ print (1563) */
    /** CommonServices.ProductionJobOrderSlipSteel1563:16748 - JobOrderPrintNew first, then 1563-ProductionJobOrderSlip.rpt. */
    function print(id, btnId) {
        var run = function () {
            return getJson(api + '/print-check?id=' + id).then(function (d) {
                if (d && d.message) { box(d.message); return; }
                if (typeof window.printRpt !== 'function') { box('print-rpt.js is not loaded'); return; }
                return window.printRpt('1563-ProductionJobOrderSlip.rpt', { id: id, documentTypeId: DOC_TYPE, branchesId: (d && d.branchesId) || branchesId });
            }).catch(function (e) { box(e.message); });
        };
        return btnId ? busy(btnId, run) : run();
    }
    /** Print_Click:4421 */
    function printCurrent() { if (recId > 0) return print(recId, 'Print'); box('No Record Selected'); }

    /* ------------------------------------------------------------------ Load Invoices (LoadavailableTransactionsForIssuance) */
    var LOADER_COLS = ['RefDocumentType', 'DocDate', 'DocCodeNo', 'ManualNo', 'GrnNo', 'SupplierCustomerName', 'VehicleNo', 'GpNo', 'WareHouseCode', 'RefWarehouse', 'ItemName', 'ItemCode',
        'CropBatch', 'JobLotCode', 'PackingType', 'PackUom', 'PackSize', 'QtyIn', 'QtyOut', 'QtyBalance', 'WeightIn', 'WeightOut', 'WeightBalance', 'ReserveWeight', 'StockWeightOut', 'AVgRate', 'RateUom', 'ItemAmount', 'BiltyNo', 'TranRemarks'];
    function openLoader() {
        $id('loaderModal').classList.add('is-open');
        if (!val('FromDate')) { var d = new Date(); d.setDate(d.getDate() - 7); setVal('FromDate', iso(d)); }
        if (!val('Todate')) setVal('Todate', today());
        getJson(api + '/loader/combos').then(function (c) {
            c = c || {};
            fill('CmbItemParentCategory', c.parentCategories, 'Id', 'Name'); fill('CmbItemCategory', c.itemCategories, 'Id', 'Name'); fill('CmbItemType', c.itemTypes, 'Id', 'Name');
            fill('CmbJobLot', c.jobLots, 'Id', 'Name'); fill('CmbCropYear', c.cropYears, 'Id', 'Name'); fill('CmbWarehouse', c.warehouses, 'Id', 'Name');
            fill('CmbReferenceDocument', c.docTypes, 'Id', 'Name'); fill('CmbSupplier', c.parties, 'Id', 'Name'); fill('CmbItemName', c.items, 'Id', 'Name');
        }).catch(function (e) { box(e.message); });
    }
    function closeLoader() { $id('loaderModal').classList.remove('is-open'); }
    function loaderReset() { ['CmbItemParentCategory', 'CmbItemCategory', 'CmbItemType', 'CmbJobLot', 'CmbCropYear', 'CmbWarehouse', 'CmbReferenceDocument', 'CmbSupplier', 'CmbItemName'].forEach(function (id) { setVal(id, ''); }); loaderRows = []; renderLoader(); }
    function loaderShow() {
        return busy('btngrnlod', function () {
            var q = ['from=' + encodeURIComponent(val('FromDate')), 'to=' + encodeURIComponent(val('Todate')), 'parentCategoryId=' + intOf(val('CmbItemParentCategory')),
                'itemCategoryId=' + intOf(val('CmbItemCategory')), 'itemTypeId=' + intOf(val('CmbItemType')), 'jobLotId=' + intOf(val('CmbJobLot')),
                'cropYear=' + encodeURIComponent(selText('CmbCropYear')), 'warehouseId=' + intOf(val('CmbWarehouse')), 'refDocumentTypeId=' + intOf(val('CmbReferenceDocument')),
                'supplierCustomerId=' + intOf(val('CmbSupplier')), 'itemId=' + intOf(val('CmbItemName'))];
            return getJson(api + '/loader/rows?' + q.join('&')).then(function (rows) { loaderRows = rows || []; renderLoader(); }).catch(function (e) { box(e.message); });
        });
    }
    function renderLoader() {
        $id('loaderHead').innerHTML = '<th><input type="checkbox" id="loaderAll" title="Select all"></th>' + LOADER_COLS.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('');
        $id('loaderBody').innerHTML = loaderRows.length ? loaderRows.map(function (r, i) {
            return '<tr><td><input type="checkbox" class="loader-sel" data-i="' + i + '"></td>' + LOADER_COLS.map(function (c) {
                var v = ci(r, c); if (c === 'DocDate') v = fmtDMY(v);
                return '<td>' + esc(v) + '</td>';
            }).join('') + '</tr>';
        }).join('') : '<tr class="sj-empty"><td colspan="' + (LOADER_COLS.length + 1) + '">No records</td></tr>';
        $id('lblLoaderCount').textContent = loaderRows.length + ' record(s)';
        var all = $id('loaderAll');
        if (all) all.addEventListener('change', function () { document.querySelectorAll('.loader-sel').forEach(function (c) { c.checked = all.checked; }); selectedTotals(); });
        document.querySelectorAll('.loader-sel').forEach(function (c) { c.addEventListener('change', selectedTotals); });
        selectedTotals();
    }
    /** SelectedWeightCalculation:636 */
    function selectedTotals() {
        var q = 0, w = 0;
        document.querySelectorAll('.loader-sel:checked').forEach(function (c) { var r = loaderRows[+c.getAttribute('data-i')]; if (r) { q += num(ci(r, 'QtyBalance')); w += num(ci(r, 'WeightBalance')); } });
        $id('lblSelectedQty').textContent = fmtDec(q, 3); $id('lblSelectedWeight').textContent = fmtDec(w, 3);
    }
    /** btnLoadOnInvoice_Click_1:566 + LoadDataDetailfromPurchaseInvoivce:4477 */
    function loaderApply() {
        var sel = Array.prototype.map.call(document.querySelectorAll('.loader-sel:checked'), function (c) { return loaderRows[+c.getAttribute('data-i')]; }).filter(Boolean);
        if (!sel.length) { box('Please Select Row first'); return; }
        sel.forEach(function (r) {
            var found = false;
            inputs.forEach(function (x) {
                if (intOf(ci(r, 'RefDocumentTypeId')) === intOf(x.RefDocumentTypeId) && intOf(ci(r, 'RefDocIdNo')) === intOf(x.RefDocIdNo) && intOf(ci(r, 'RefDocSubIdNo')) === intOf(x.RefDocSubIdNo)) {
                    x.BalanceWeight = num(x.BalanceWeight) + num(ci(r, 'WeightBalance'));
                    x.BalanceQty = num(x.BalanceQty) + num(ci(r, 'QtyBalance'));
                    found = true;
                }
            });
            if (!found) {
                inputs.push({
                    Id: 0, RefDocumentTypeId: intOf(ci(r, 'RefDocumentTypeId')), RefDocIdNo: intOf(ci(r, 'RefDocIdNo')), RefDocSubIdNo: intOf(ci(r, 'RefDocSubIdNo')),
                    WareHouseId: intOf(ci(r, 'WarehouseId')), WareHouse: ci(r, 'WareHouseCode'), JobLotId: intOf(ci(r, 'JobLotId')), JobLot: ci(r, 'JobLotCode'),
                    ItemId: intOf(ci(r, 'ItemId')), ItemName: ci(r, 'ItemName'), PackTypeId: intOf(ci(r, 'InvPackingTypeId')), PackType: ci(r, 'PackingType'),
                    BalanceQty: num(ci(r, 'QtyBalance')), BalanceWeight: num(ci(r, 'WeightBalance')), Qty: num(ci(r, 'QtyBalance')),
                    PackSizeId: intOf(ci(r, 'ItemUom')), PackSize: ci(r, 'PackUom'), PackSizeEquivalent: num(ci(r, 'PackSize')), Weight: num(ci(r, 'WeightBalance')),
                    ItemRate: num(ci(r, 'AVgRate')), RateUomId: intOf(ci(r, 'RateUomId')), RateUom: ci(r, 'RateUom'), RateUomEquivalent: num(ci(r, 'Equivalent')),
                    ItemAmount: num(ci(r, 'ItemAmount')), Remarks: ''
                });
            }
        });
        closeLoader(); renderInputs();
    }

    /* ------------------------------------------------------------------ grid events */
    function rowOf(e) { var tr = e.target.closest('tr[data-kind]'); return tr ? { kind: tr.getAttribute('data-kind'), i: +tr.getAttribute('data-i') } : null; }
    function markCurrent(kind, i) {
        current[kind] = i;
        var body = { output: 'outputBody', input: 'inputBody', oh: 'ohBody', plant: 'plantBody' }[kind];
        if (body) $id(body).querySelectorAll('tr[data-kind]').forEach(function (tr) { if (!tr.classList.contains('is-editing')) tr.classList.toggle('is-current', +tr.getAttribute('data-i') === i); });
    }
    function wireGrids() {
        [['outputBody', 'output'], ['inputBody', 'input'], ['ohBody', 'oh'], ['plantBody', 'plant']].forEach(function (g) {
            var body = $id(g[0]);
            body.addEventListener('click', function (e) {
                var r = rowOf(e); if (!r) return;
                markCurrent(r.kind, r.i);
                if (!e.target.closest('button[data-act="del"]')) return;
                ({ output: deleteOutput, input: deleteInput, oh: deleteOH, plant: deletePlant })[r.kind](r.i);
            });
            body.addEventListener('dblclick', function (e) {
                var r = rowOf(e); if (!r || e.target.closest('button') || e.target.closest('input')) return;
                ({ output: editOutput, input: editInput, oh: editOH, plant: editPlant })[r.kind](r.i);
            });
            body.addEventListener('focusin', function (e) { var r = rowOf(e); if (r) markCurrent(r.kind, r.i); });
        });
        $id('inputBody').addEventListener('change', function (e) {
            var t = e.target; if (!t || !t.classList.contains('sj-cellin')) return;
            inputCellUpdated(+t.getAttribute('data-i'), t.getAttribute('data-col'), t.value);
        });
        var hb = $id('historyBody');
        hb.addEventListener('click', function (e) {
            var r = rowOf(e); if (!r) return;
            var btn = e.target.closest('button[data-act]');
            historyAction(r.i, btn ? btn.getAttribute('data-act') : 'select');
        });
        hb.addEventListener('dblclick', function (e) { var r = rowOf(e); if (!r || e.target.closest('button')) return; historyAction(r.i, 'dbl'); });
    }

    /* ------------------------------------------------------------------ keyboard (frmProductionJobOrder_KeyDown:4367) */
    function focusNext(from) {
        var all = Array.prototype.filter.call(document.querySelectorAll('input, select, textarea, button, [tabindex="0"]'), function (el) {
            return !el.disabled && el.tabIndex >= 0 && el.offsetParent !== null && !(el.tagName === 'SELECT' && el.classList.contains('dtcombo') && getComputedStyle(el).display === 'none');
        });
        var i = all.indexOf(from); if (i >= 0 && i + 1 < all.length) all[i + 1].focus();
    }
    function onKey(e) {
        var t = e.target, k = e.key, ctrl = e.ctrlKey || e.metaKey;
        if (k === 'Escape') {
            if (e.defaultPrevented) return;
            if (document.querySelector('.cx-modal.is-open')) { closeLoader(); closeAttachments(); return; }
            var f = document.querySelector('.sj-full'); if (f) { f.classList.remove('sj-full'); return; }
            window.location.href = '/production'; return;
        }
        if (k === 'Enter' && !ctrl) {
            if (e.defaultPrevented || !t || t.tagName === 'BUTTON' || t.tagName === 'TEXTAREA' || t.tagName === 'TR') return;
            if (t.closest && t.closest('.cx-modal')) return;
            e.preventDefault(); focusNext(t); return;
        }
        if (!ctrl) return;
        var key = k.length === 1 ? k.toLowerCase() : k, tabIdx = currentTab();
        if (key === 's') { e.preventDefault(); if (tabIdx === 0 && !updateMode) save(); return; }
        if (key === 'n') { e.preventDefault(); reset(); return; }
        if (key === 't') { e.preventDefault(); tab(tabIdx === 1 ? 'tabForm' : 'tabHistory'); return; }
        if (key === 'e') { e.preventDefault(); window.location.href = '/production'; return; }
        if (key === 'u') { e.preventDefault(); if (updateMode) update(); }
    }

    /* ------------------------------------------------------------------ boot */
    function boot() {
        ['txtPlanDate', 'txtstartDate', 'txtEndDate'].forEach(function (id) { setVal(id, today()); });
        document.addEventListener('keydown', onKey);
        /* CommonServices.OnlytextdecimelFunction on the numeric boxes */
        ['txtOuterqty', 'txtItemRateOutPut', 'txtqtyinput', 'txtweightinput', 'txtItemQtyOverHead', 'txtItemRateOverHeads', 'txtExpAmountOverHead'].forEach(function (id) {
            var el = $id(id); if (!el) return;
            el.addEventListener('keypress', function (e) {
                if (e.key.length !== 1 || e.ctrlKey) return;
                if (/[0-9]/.test(e.key)) return;
                if (e.key === '.' && el.value.indexOf('.') < 0) return;
                e.preventDefault();
            });
        });
        wireGrids(); applyButtons(); renderAll(); renderHistory();
        loadAll().catch(function (e) { box(e.message); });
        focusEl('txtPlanDate');
    }

    window.SteelJO = {
        tab: tab, tab2: tab2, full: full, reset: reset, refresh: refresh, save: save, update: update, generate: generate, generateInputRates: generateInputRates,
        printCurrent: printCurrent, entryTypeChanged: entryTypeChanged, filterTypeChanged: filterTypeChanged, docNoChanged: docNoChanged,
        outputItemChanged: outputItemChanged, outputQtyChanged: outputQtyChanged, outputAmount: outputAmount, addOutput: addOutput, updateOutput: updateOutput, cancelOutput: cancelOutput,
        inputItemChanged: inputItemChanged, inputAvgRate: inputAvgRate, inputQtyChanged: inputQtyChanged, inputAmount: inputAmount, addInput: addInput, updateInput: updateInput, cancelInput: cancelInput,
        ohBrandChanged: ohBrandChanged, ohBrandUomChanged: ohBrandUomChanged, ohCalc: ohCalc, addOH: addOH, updateOH: updateOH, cancelOH: cancelOH,
        addPlant: addPlant, updatePlant: updatePlant, cancelPlant: cancelPlant,
        loadHistory: loadHistory, newHistory: newHistory, closeAttachments: closeAttachments,
        openLoader: openLoader, closeLoader: closeLoader, loaderShow: loaderShow, loaderReset: loaderReset, loaderApply: loaderApply
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot); else boot();
}());
