/* ============================================================================================
 * Screen 281 "Production Job Order" - Architecture.WinApp.Production.frmProductionJobOrderMain.
 * Line numbers below are frmProductionJobOrderMain.cs in recovery/resolved-source.
 *
 * BUTTON VISIBILITY IS STATE, EXACTLY AS ON THE DESKTOP
 *   Load :433     Save visible, Update hidden, SaveAs hidden
 *   History Edit  (:2119) Save hidden, SaveAs hidden, Update visible
 *   History SaveAs(:2126) Save hidden, Update hidden, SaveAs visible
 *   History double-click (:1928) Save hidden, Update visible - SaveAs is NOT touched
 *   Reset :1446   Update hidden, Save visible - SaveAs is NOT touched either
 * and the row-delete rules read that state: "btnsave.Visible && btnsave.Enabled" means the grid
 * row is simply dropped; otherwise a plant row is checked against Production (:971) and a rate row
 * is deleted in the database at once (:1157).
 *
 * RECID
 *   ReadById sets it to the opened record, for Edit AND SaveAs (:1826). Save and SaveAs zero it
 *   before Insert (:1785, :1814), even when validation then refuses. Reset does not touch it. The
 *   toolbar 620-Print sets it to the current history row (:2309). All reproduced.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/production/job-order-main';

    /* ------------------------------------------------------------------ state */

    var recId = 0;                                  // RECID
    var updateMode = false;                         // UpdateMode (KeyDown Ctrl+S / Ctrl+U)
    var vis = { save: true, update: false, saveAs: false };
    var flags = { byProductRateEditableIsAllow: false, generateJobOrderNo: false };
    var rights = { canSave: false, canUpdate: false, canPrint: false };
    var financialYearStart = '';
    var rateDecimals = 2;

    var plants = [];        // dtDetail        {Id, PlantId, PlantName, RemarksDetail}
    var byProduct = [];     // dtByProduct     {Id, EffectiveDate, ItemId, ItemName, Rate40Kg}
    var finish = [];        // dtFinish        same shape
    var schedule = [];      // dtExportSchedule{Id, ContractId, SaleContract, ScheduleId, ScheduleNo, ItemId, Item, MTon, Remarks}
    var scheduleHasX = true;// ScheduleGridSetting adds "Delete" only when btnsave is Enabled AND Visible
    var loaderRows = [];

    var editPlantIndex = -1;                  // UpdateDetailIndex
    var editRateIndex = { 1: -1, 2: -1 };     // UpdateDetailIndexForByProduct / ...ForFinishGoods
    var current = { plant: -1, 1: -1, 2: -1 };// CurrentRow of each grid
    var historyRows = [];
    var historyCurrent = -1;                  // DataGridHistory.CurrentRow

    /* ------------------------------------------------------------------ dom helpers */

    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function box(m) { window.alert(m); }
    function ask(m) { return window.confirm(m); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function focusEl(id) { var e = $id(id); if (!e) return; try { e.focus(); } catch (x) { /* ignore */ } }

    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function ci(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        for (var k in row) {
            if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === name.toLowerCase()) return row[k];
        }
        return '';
    }
    function num(v) {
        var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, ''));
        return isNaN(n) ? 0 : n;
    }
    function intOf(v) { return Math.trunc(num(v)); }
    function selText(id) {
        var el = $id(id);
        if (!el || el.selectedIndex < 0) return '';
        var o = el.options[el.selectedIndex];
        return (o && o.value !== '') ? o.text : '';
    }

    var MONTHS = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    function parts(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2}))?/.exec(String(v || ''));
        return m ? { y: m[1], M: m[2], d: m[3], h: m[4], mi: m[5] } : null;
    }
    /** "dd-MMM-yy" - GridByProductSetting:748. */
    function fmtDMY(v) { var p = parts(v); return p ? p.d + '-' + MONTHS[+p.M - 1] + '-' + p.y.slice(2) : String(v || ''); }
    /** A DateTime column with no FormatString (grdFinishGoods, history Date/StartDate/EndDate). */
    function fmtShort(v) { var p = parts(v); return p ? p.d + '/' + p.M + '/' + p.y : String(v || ''); }
    /** "dd-MM-yyyy hh:mm tt" - HgridSetting:2045-2047. */
    function fmtStamp(v) {
        var p = parts(v);
        if (!p) return String(v || '');
        var h = +(p.h || 0), mi = p.mi || '00', tt = h < 12 ? 'AM' : 'PM';
        var h12 = h % 12 === 0 ? 12 : h % 12;
        return p.d + '-' + p.M + '-' + p.y + ' ' + String(h12).padStart(2, '0') + ':' + mi + ' ' + tt;
    }
    /** "#,##0.###" - ScheduleGridSetting:825. */
    function fmtMTon(v) {
        var s = num(v).toFixed(3).replace(/0+$/, '').replace(/\.$/, '');
        var p = s.split('.');
        p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return p.join('.');
    }
    /** clsGlobalVariables.DecimalRateFormate - "#,#0." + n zeros. */
    function fmtRate(v) { return num(v).toFixed(rateDecimals).replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
    function iso(d) {
        return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0');
    }
    function today() { return iso(new Date()); }
    function isoOf(v) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || '')); return m ? m[0] : ''; }

    /* Disable -> spinner -> ignore repeats -> re-enable on success AND failure. */
    function busy(btnId, fn) {
        var b = $id(btnId);
        if (b) {
            if (b.classList.contains('is-busy')) return;
            b.dataset.wasDisabled = b.disabled ? '1' : '';
            b.disabled = true;
            b.classList.add('is-busy');
        }
        var done = function () {
            if (!b) return;
            b.classList.remove('is-busy');
            b.disabled = b.dataset.wasDisabled === '1';
            applyButtons();
        };
        var p;
        try { p = fn(); } catch (e) { done(); throw e; }
        if (p && typeof p.then === 'function') p.then(done, done); else done();
        return p;
    }

    function csrf(opt) {
        var token = document.querySelector('meta[name="_csrf"]');
        var header = document.querySelector('meta[name="_csrf_header"]');
        if (token && header && opt.method && opt.method.toUpperCase() !== 'GET') {
            opt.headers[header.getAttribute('content')] = token.getAttribute('content');
        }
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

    /* ------------------------------------------------------------------ pickers */

    /** Rebind, keeping the current selection when it is still in the new list
     *  (InfragisticsHelper.BindAndRetainSelection / a re-bound UltraCombo keeps its Value). */
    function fill(selectId, rows, valueKey, textKey, byText) {
        var el = $id(selectId);
        if (!el) return;
        var keep = el.value;
        var html = '<option value=""></option>';
        (rows || []).forEach(function (r) {
            var t = ci(r, textKey);
            var v = byText ? t : ci(r, valueKey);
            html += '<option value="' + esc(v) + '">' + esc(t) + '</option>';
        });
        el.innerHTML = html;
        if (keep !== '') el.value = keep;
        if (el.value !== keep) el.value = '';
    }

    /** Each list is replaced only when it has rows - every Bind* on this form is guarded by
     *  "if (dataTable.Rows.Count > 0)", so an empty result leaves the old list in place. */
    function bindIf(selectId, rows, valueKey, textKey, byText) {
        if (rows && rows.length) fill(selectId, rows, valueKey, textKey, byText);
    }

    function applyFlagsAndRights(d) {
        rights.canSave = !!d.canSave;
        rights.canUpdate = !!d.canUpdate;
        rights.canPrint = !!d.canPrint;
        flags.byProductRateEditableIsAllow = !!d.byProductRateEditableIsAllow;
        flags.generateJobOrderNo = !!d.generateJobOrderNo;
        if (d.rateDecimals !== undefined && d.rateDecimals !== null) rateDecimals = intOf(d.rateDecimals);
        /* :365-367 / :1512-1514 - one of the two Production # controls, never both. */
        show('btnGenerateProductionNo', flags.generateJobOrderNo);
        show('fldProductionNoCombo', flags.generateJobOrderNo);
        show('fldProductionNoText', !flags.generateJobOrderNo);
    }

    /** frmProductionJobOrder_Load:352 - every list, then the last saved WIP account/item/warehouse. */
    function loadAll() {
        return getJson(api + '/lookups').then(function (d) {
            d = d || {};
            applyFlagsAndRights(d);
            financialYearStart = isoOf(d.financialYearStart);

            if (flags.generateJobOrderNo) bindIf('cmbProductionNo', d.generatedJobOrderNos, 'Id', 'FinalInvoiceNo', true);
            /* ParameterFill:522 - bound, then row 0 activated, which fires ValueChanged. */
            bindIf('cmbPeremeter', d.dateTypes, 'Id', 'Parameters');
            if (d.dateTypes && d.dateTypes.length) { setVal('cmbPeremeter', ci(d.dateTypes[0], 'Id')); datePreset(); }
            bindIf('cmbHistoryJobOrder', d.jobOrderNos, 'Id', 'Name');
            bindAccounts(d);
            bindIf('cmbWipItem', d.wipItems, 'Id', 'ItemName');
            bindIf('cmbPlanType', d.planTypes, 'Id', 'PlanTypeDescription', true);
            if (intOf(d.planCode) > 0) setVal('txtPlanCode', d.planCode);
            bindIf('cmbPlantFeader', d.plants, 'PlantId', 'PlantName');
            bindIf('cmbProductionType', d.productionTypes, 'Id', 'ProductionTypeDescription', true);
            /* StatusBind:698 - row 0 ("In Process") activated on load. Reset clears it (:1444). */
            bindIf('cmbStatus', d.statuses, 'Id', 'Status', true);
            if (d.statuses && d.statuses.length) setVal('cmbStatus', ci(d.statuses[0], 'Status'));
            bindIf('cmbItemByProduct', d.items, 'Id', 'ItemName');
            bindIf('cmbItemFinish', d.items, 'Id', 'ItemName');
            bindIf('cmbWipWarehouse', d.warehouses, 'Id', 'WarehouseName');
            restoreLastWip(d);
            applyButtons();
            renderAllGrids();
        });
    }

    function bindAccounts(d) {
        /* AccountFills:626 - one call, three combos. */
        bindIf('cmbWorkInProcess', d.accounts, 'Id', 'AccountTitle');
        bindIf('cmbFinishGoodsAc', d.accounts, 'Id', 'AccountTitle');
        bindIf('cmbByProductionAc', d.accounts, 'Id', 'AccountTitle');
    }

    /** GetLastSaveAccount:543 + :384-395 - each restored only when > 0. */
    function restoreLastWip(d) {
        if (intOf(d.lastWipAccountId) > 0)   setVal('cmbWorkInProcess', d.lastWipAccountId);
        if (intOf(d.lastWipItemId) > 0)      setVal('cmbWipItem', d.lastWipItemId);
        if (intOf(d.lastWipWareHouseId) > 0) setVal('cmbWipWarehouse', d.lastWipWareHouseId);
    }

    /** :360-362 - Enabled from the rights; Visible from the form state. SaveAs has no rights line
     *  on the desktop, so it is never disabled here either (the server still checks Save). */
    function applyButtons() {
        show('btnSave', vis.save);
        show('btnUpdate', vis.update);
        show('btnSaveAs', vis.saveAs);
        var s = $id('btnSave'), u = $id('btnUpdate'), p = $id('btnPrint');
        if (!s.classList.contains('is-busy')) s.disabled = !rights.canSave;
        if (!u.classList.contains('is-busy')) u.disabled = !rights.canUpdate;
        if (!p.classList.contains('is-busy')) p.disabled = !rights.canPrint;

        var missing = [];
        if (!rights.canSave) missing.push('Save');
        if (!rights.canUpdate) missing.push('Update');
        if (!rights.canPrint) missing.push('Print');
        var note = $id('rightsNote');
        note.textContent = missing.length
            ? 'Your account does not have the ' + missing.join(', ') + ' right on this screen, so those buttons are off.'
            : '';
        note.classList.toggle('is-hidden', !missing.length);
    }
    function saveModeActive() { return vis.save && rights.canSave; }   // btnsave.Visible && btnsave.Enabled

    /* ------------------------------------------------------------------ tabs */

    function currentTab() { return $id('tabHistory').classList.contains('is-active') ? 1 : 0; }
    function tab(id) {
        ['tabForm', 'tabHistory'].forEach(function (t) { $id(t).classList.toggle('is-active', t === id); });
        $id('tabs1').querySelectorAll('.jo-tab').forEach(function (b) {
            b.classList.toggle('is-active', b.getAttribute('data-tab') === id);
        });
    }
    function tab2(id) {
        ['tabDetail', 'tabSchedule'].forEach(function (t) { $id(t).classList.toggle('is-active', t === id); });
        $id('tabs2').querySelectorAll('.jo-tab').forEach(function (b) {
            b.classList.toggle('is-active', b.getAttribute('data-tab') === id);
        });
    }

    /** ctrlGrdBar - the grid tool on each bar. On the web: the container goes full screen. */
    function full(hostId) {
        var h = $id(hostId);
        if (!h) return;
        var on = !h.classList.contains('jo-full');
        document.querySelectorAll('.jo-full').forEach(function (x) { x.classList.remove('jo-full'); });
        if (on) h.classList.add('jo-full');
    }

    /* ------------------------------------------------------- plant grid (GridDetail) */

    function rowAttrs(kind, i, cur, editing) {
        return ' tabindex="0" data-kind="' + kind + '" data-i="' + i + '" class="'
             + (i === editing ? 'is-editing' : (i === cur ? 'is-current' : '')) + '"';
    }

    function renderPlants() {
        $id('gridPlant').innerHTML = plants.length ? plants.map(function (r, i) {
            return '<tr' + rowAttrs('plant', i, current.plant, editPlantIndex) + '>'
                 + '<td class="col-x"><button type="button" class="jo-x" tabindex="-1" data-act="del">X</button></td>'
                 + '<td>' + esc(r.PlantName) + '</td><td class="wrap">' + esc(r.RemarksDetail) + '</td></tr>';
        }).join('') : '';
        $id('lblPlantCount').textContent = plants.length;
    }

    /** FormValidationDetail:882. */
    function plantFieldOk() {
        if (!selText('cmbPlantFeader').trim()) {
            box('Plant Feeder Field is Required');
            focusEl('cmbPlantFeader');
            return false;
        }
        return true;
    }

    /** btnplus_Click:851. */
    function addPlant() {
        if (!plantFieldOk()) return;
        var id = intOf(val('cmbPlantFeader'));
        for (var i = 0; i < plants.length; i++) {
            if (intOf(plants[i].PlantId) === id) { box('Same Plant Already Exist In Detail'); return; }
        }
        plants.push({ Id: 0, PlantId: id, PlantName: selText('cmbPlantFeader').trim(), RemarksDetail: val('txtRemarksDetail').trim() });
        renderPlants();
        setVal('cmbPlantFeader', '');       // :871 - the Comments box keeps its text
    }

    /** GridDetail_DoubleClick:893. */
    function editPlant(i) {
        show('btnPlus', false);
        show('btnUpdateDetail', true);
        show('btnCancelUpdateDetail', true);
        var r = plants[i];
        if (!r) return;
        editPlantIndex = i;
        setVal('cmbPlantFeader', r.PlantId);
        setVal('txtRemarksDetail', r.RemarksDetail);
        renderPlants();
    }

    /** btnUpdateDetail_Click:914. */
    function updatePlant() {
        if (!plantFieldOk()) return;
        var id = intOf(val('cmbPlantFeader'));
        for (var i = 0; i < plants.length; i++) {
            if (i !== editPlantIndex && intOf(plants[i].PlantId) === id) { box('Same Plant Already Exist In Detail'); return; }
        }
        if (plants[editPlantIndex]) {
            plants[editPlantIndex].PlantId = id;
            plants[editPlantIndex].PlantName = selText('cmbPlantFeader').trim();
            plants[editPlantIndex].RemarksDetail = val('txtRemarksDetail').trim();
        }
        cancelPlant();
    }

    /** btnCancelUpdateDetial_Click:947. */
    function cancelPlant() {
        editPlantIndex = -1;
        show('btnPlus', true);
        show('btnUpdateDetail', false);
        show('btnCancelUpdateDetail', false);
        setVal('cmbPlantFeader', '');
        renderPlants();
    }

    /** GridDetail_ColumnButtonClick:962. */
    function deletePlant(i) {
        var r = plants[i];
        if (!r) return;
        if (!ask('Are you sure to Delete?')) return;
        var drop = function () {
            plants.splice(i, 1);
            current.plant = -1;
            if (editPlantIndex === i) editPlantIndex = -1;
            else if (editPlantIndex > i) editPlantIndex--;
            renderPlants();
        };
        if (saveModeActive()) { drop(); return; }
        /* CheckPlantExistinProductionagaintJobOrder(JobOrderId = RECID, PlantId). */
        getJson(api + '/plant-deletable?jobOrderId=' + recId + '&plantId=' + encodeURIComponent(r.PlantId)
                + '&plantName=' + encodeURIComponent(r.PlantName || ''))
            .then(function (d) {
                if (d && d.deletable) drop();
                else box((d && d.message) || 'That plant has been referred to production.');
            })
            .catch(function (e) { box(e.message); });
    }

    /* ------------------------------------------------- rate grids (By Product / Finish) */

    function rateState(type) {
        return type === 1
            ? { rows: byProduct, grid: 'gridByProduct', count: 'lblByProductCount', date: 'datEffectiveDateByProduct',
                item: 'cmbItemByProduct', rate: 'txtRateByProduct', btn: 'btnAddUpdateByProduct', cancel: 'btnCancelByProduct',
                fmt: fmtDMY }
            : { rows: finish, grid: 'gridFinish', count: 'lblFinishCount', date: 'datEffectiveDateFinish',
                item: 'cmbItemFinish', rate: 'txtRateFinish', btn: 'btnAddUpdateFinish', cancel: 'btnCancelFinish',
                fmt: fmtShort };   /* GridFinishGoodsSetting:771 sets no FormatString on EffectiveDate */
    }

    function renderRates(type) {
        var s = rateState(type);
        $id(s.grid).innerHTML = s.rows.map(function (r, i) {
            return '<tr' + rowAttrs('rate' + type, i, current[type], editRateIndex[type]) + '>'
                 + '<td class="col-x"><button type="button" class="jo-x" tabindex="-1" data-act="del">X</button></td>'
                 + '<td>' + esc(s.fmt(r.EffectiveDate)) + '</td>'
                 + '<td class="wrap">' + esc(r.ItemName) + '</td>'
                 + '<td class="num">' + esc(fmtRate(r.Rate40Kg)) + '</td></tr>';
        }).join('');
        $id(s.count).textContent = s.rows.length;
    }

    /** FormValidationByProduct:1028 / FormValidationFinishGoods:1225. */
    function rateFieldsOk(type) {
        var s = rateState(type);
        if (intOf(val(s.item)) === 0) { box('Item Name Field is Required'); focusEl(s.item); return false; }
        if (num(val(s.rate)) === 0) { box('Rate40Kg Field is Required'); focusEl(s.rate); return false; }
        return true;
    }

    /** btnAddUpdateByProduct_Click:1045 / btnAddUpdateFinish_Click:1242 - the button's TEXT decides. */
    function addRate(type) {
        var s = rateState(type);
        if (!rateFieldsOk(type)) return;
        var d = val(s.date), itemId = intOf(val(s.item));
        var updating = $id(s.btn).textContent.trim() === 'Update';
        for (var i = 0; i < s.rows.length; i++) {
            if (updating && i === editRateIndex[type]) continue;
            if (isoOf(s.rows[i].EffectiveDate) === isoOf(d) && intOf(s.rows[i].ItemId) === itemId) {
                box('In Same Effective Date Same Item Already Exist in record. Please Check!');
                return;
            }
        }
        if (!updating) {
            s.rows.push({ Id: 0, EffectiveDate: d, ItemId: itemId, ItemName: selText(s.item), Rate40Kg: num(val(s.rate)) });
        } else if (s.rows[editRateIndex[type]]) {
            var r = s.rows[editRateIndex[type]];
            r.EffectiveDate = d; r.ItemId = itemId; r.ItemName = selText(s.item); r.Rate40Kg = num(val(s.rate));
            $id(s.btn).textContent = 'Add';
            show(s.cancel, false);
            editRateIndex[type] = -1;
        }
        setVal(s.item, '');                 // :1100 - the effective date stays
        setVal(s.rate, '');
        renderRates(type);
        focusEl(s.date);
    }

    /** grdByProduct_DoubleClick:1126 / grdFinishGoods_DoubleClick:1323. */
    function editRate(type, i) {
        var s = rateState(type);
        var r = s.rows[i];
        if (r) {
            editRateIndex[type] = i;
            setVal(s.date, isoOf(r.EffectiveDate));
            setVal(s.item, r.ItemId);
            setVal(s.rate, fmtRate(r.Rate40Kg));
        }
        $id(s.btn).textContent = 'Update';
        show(s.cancel, true);
        renderRates(type);
        focusEl(s.date);
    }

    /** btnCancelByProduct_Click:1110 / btnCancelFinish_Click:1307. */
    function cancelRate(type) {
        var s = rateState(type);
        editRateIndex[type] = -1;
        $id(s.btn).textContent = 'Add';
        show(s.cancel, false);
        setVal(s.item, '');
        setVal(s.rate, '');
        renderRates(type);
    }

    /** grdByProduct_ColumnButtonClick:1148 / grdFinishGoods_ColumnButtonClick:1345.
     *  Not in save mode: JoBOrderDetailScheduleDeleteById(row Id) runs at once. A row with Id 0
     *  (added in this session, or every row after SaveAs) deletes nothing in the database. */
    function deleteRate(type, i) {
        var s = rateState(type);
        var r = s.rows[i];
        if (!r) return;
        if (!ask('Are you sure to Delete?')) return;
        var drop = function () {
            s.rows.splice(i, 1);
            current[type] = -1;
            if (editRateIndex[type] === i) editRateIndex[type] = -1;
            else if (editRateIndex[type] > i) editRateIndex[type]--;
            renderRates(type);
        };
        if (saveModeActive() || intOf(r.Id) === 0) { drop(); return; }
        request(api + '/delete-rate-schedule-row?jobOrderId=' + recId + '&detailId=' + intOf(r.Id), { method: 'POST' })
            .then(drop)
            .catch(function (e) { box(e.message); });
    }

    /* ------------------------------------------------- export schedule grid */

    /** ScheduleGridSetting:799 - runs on Load, Reset, ReadById and LoadInGrid, and adds the X
     *  column only if Save is enabled AND visible at that moment. */
    function scheduleSetting() { scheduleHasX = saveModeActive(); renderSchedule(); }

    function renderSchedule() {
        $id('scheduleHead').innerHTML = (scheduleHasX ? '<th class="col-x">X</th>' : '')
            + '<th style="width:130px;">SaleContract</th><th style="width:130px;">ScheduleNo</th>'
            + '<th style="width:250px;">Item</th><th class="num" style="width:90px;">MTon</th><th>Remarks</th>';
        var total = 0;
        $id('gridSchedule').innerHTML = schedule.map(function (r, i) {
            total += num(r.MTon);
            return '<tr>'
                 /* The desktop wires no handler to grdScheduleInfo.ColumnButtonClick (see the event
                    list at :3013-4930), so its X does nothing. Drawn, and inert, for the same reason:
                    the save procedure is Insert_Update, so a row dropped here would come back from
                    the database on the next open anyway. */
                 + (scheduleHasX ? '<td class="col-x"><button type="button" class="jo-x" disabled '
                    + 'title="No handler on the desktop grid - the row is not removed.">X</button></td>' : '')
                 + '<td>' + esc(r.SaleContract) + '</td><td>' + esc(r.ScheduleNo) + '</td>'
                 + '<td class="wrap">' + esc(r.Item) + '</td><td class="num">' + esc(fmtMTon(r.MTon)) + '</td>'
                 + '<td><input type="text" class="jo-cellin" value="' + esc(r.Remarks) + '" data-i="' + i + '" data-act="remarks"></td></tr>';
        }).join('');
        /* MTon AggregateFunction.Sum, TotalFormatString "#,##0.###". */
        $id('gridScheduleFoot').innerHTML = schedule.length
            ? '<tr>' + (scheduleHasX ? '<td></td>' : '') + '<td>Total</td><td></td><td></td><td class="num">'
              + esc(fmtMTon(total)) + '</td><td></td></tr>' : '';
        $id('lblScheduleCount').textContent = schedule.length;
    }

    function renderAllGrids() { renderPlants(); renderRates(1); renderRates(2); scheduleSetting(); }

    /* ------------------------------------------------- the loader dialog */

    function openLoader() {
        $id('loaderModal').classList.add('is-open');
        return busy('btnLoadScheduleInfo', function () {
            return getJson(api + '/schedule-lookups').then(function (d) {
                d = d || {};
                fill('loaderContract', d.contracts, 'Id', 'Name');
                fill('loaderCustomer', d.customers, 'Id', 'Name');
                fill('loaderItem', d.items, 'Id', 'Name');
                fill('loaderThirdParty', d.thirdParties, 'Id', 'Name');
                if (!val('loaderFrom')) setVal('loaderFrom', isoOf(d.defaultFromDate));
                if (!val('loaderTo')) setVal('loaderTo', today());
                return loaderShow();
            }).catch(function (e) { box(e.message); });
        });
    }
    function closeLoader() { $id('loaderModal').classList.remove('is-open'); }

    var LOADER_HIDE = ['Id', 'ContractId', 'ItemId', 'SupplierCustomerId'];
    var loaderCols = [];

    function loaderShow() {
        return busy('btnLoaderShow', function () {
            var q = [];
            if (val('loaderFrom')) q.push('fromDate=' + encodeURIComponent(val('loaderFrom')));
            if (val('loaderTo')) q.push('toDate=' + encodeURIComponent(val('loaderTo')));
            q.push('contractId=' + intOf(val('loaderContract')));
            q.push('itemId=' + intOf(val('loaderItem')));
            q.push('supplierCustomerId=' + intOf(val('loaderCustomer')));
            q.push('thirdPartyId=' + intOf(val('loaderThirdParty')));
            return getJson(api + '/schedule?' + q.join('&')).then(function (rows) {
                loaderRows = rows || [];
                loaderCols = loaderRows.length
                    ? Object.keys(loaderRows[0]).filter(function (c) { return LOADER_HIDE.indexOf(c) < 0; }) : [];
                $id('loaderHead').innerHTML = '<th class="col-x"><input type="checkbox" id="loaderAll" title="Select all"></th>'
                    + loaderCols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('');
                $id('loaderBody').innerHTML = loaderRows.length
                    ? loaderRows.map(function (r, i) {
                          return '<tr><td class="col-x"><input type="checkbox" data-i="' + i + '"></td>'
                               + loaderCols.map(function (c) { return '<td>' + esc(r[c]) + '</td>'; }).join('') + '</tr>';
                      }).join('')
                    : '<tr class="jo-empty"><td colspan="' + (loaderCols.length + 1) + '">No records</td></tr>';
                $id('lblLoaderCount').textContent = loaderRows.length + ' record(s)';
                var all = $id('loaderAll');
                if (all) all.addEventListener('change', function () {
                    $id('loaderBody').querySelectorAll('input[type="checkbox"]').forEach(function (c) { c.checked = all.checked; });
                });
            }).catch(function (e) {
                loaderRows = []; loaderCols = [];
                $id('loaderHead').innerHTML = ''; $id('loaderBody').innerHTML = ''; $id('lblLoaderCount').textContent = '';
                box(e.message);
            });
        });
    }

    /** LoadInGrid:2496 - skipped when ScheduleId AND ItemId are already in the grid. */
    function loaderApply() {
        var boxes = $id('loaderBody').querySelectorAll('input[type="checkbox"]:checked');
        for (var b = 0; b < boxes.length; b++) {
            var r = loaderRows[parseInt(boxes[b].getAttribute('data-i'), 10)];
            if (!r) continue;
            var scheduleId = intOf(ci(r, 'Id')), itemId = intOf(ci(r, 'ItemId')), dup = false;
            for (var i = 0; i < schedule.length; i++) {
                if (intOf(schedule[i].ScheduleId) === scheduleId && intOf(schedule[i].ItemId) === itemId) { dup = true; break; }
            }
            if (dup) continue;
            schedule.push({ Id: 0, ContractId: intOf(ci(r, 'ContractId')), SaleContract: ci(r, 'ContractNo'),
                            ScheduleId: scheduleId, ScheduleNo: ci(r, 'ScheduleCode'), ItemId: itemId,
                            Item: ci(r, 'ItemName'), MTon: num(ci(r, 'MTon')), Remarks: '' });
        }
        scheduleSetting();
        closeLoader();
    }

    /* ------------------------------------------------------------------ save */

    /** FormValidation:1544 then the four date checks of Insert():1613-1637, before the confirm. */
    function headerOk() {
        var code = val('txtPlanCode').trim();
        if (code === '' || code === '0') { box('Plan Code Field Required'); focusEl('txtPlanCode'); return false; }
        if (flags.generateJobOrderNo) {
            var t = selText('cmbProductionNo');
            if (t === '' || t === '0') { box('Mannual ReportNo Field Required'); focusEl('cmbProductionNo'); return false; }
        } else {
            var p = val('txtProductionNo');
            if (p === '' || p === '0') { box('Mannual ReportNo Field Required'); focusEl('txtProductionNo'); return false; }
        }
        if (!val('cmbProductionType')) { box('Production Type Field Required'); focusEl('cmbProductionType'); return false; }
        if (!val('cmbPlanType')) { box('Plan Type Field Required'); focusEl('cmbPlanType'); return false; }
        if (!val('cmbStatus')) { box('Status Field Required'); focusEl('cmbStatus'); return false; }
        if (!val('cmbWorkInProcess')) { box('workinprocess field required'); focusEl('cmbWorkInProcess'); return false; }
        if (!val('cmbWipItem')) { box('WIPItem Field Required'); focusEl('cmbWipItem'); return false; }
        if (intOf(val('cmbWipWarehouse')) === 0) { box('WIP Warehouse Field Required'); focusEl('cmbWipWarehouse'); return false; }

        var plan = isoOf(val('txtPlanDate')), start = isoOf(val('txtStartDate')), end = isoOf(val('txtEndDate'));
        if (plan && start && plan > start) { box('Plan Date Cannot Be Greater Than StartDate'); focusEl('txtPlanDate'); return false; }
        if (start && end && start > end) { box('Start Date Cannot Be Greater Than End Date'); focusEl('txtStartDate'); return false; }
        if (byProduct.some(function (r) { return isoOf(r.EffectiveDate) < plan; })) {
            box('By Product Effective Date Cannot Be Less Than Plan Date'); focusEl('datEffectiveDateByProduct'); return false;
        }
        if (finish.some(function (r) { return isoOf(r.EffectiveDate) < plan; })) {
            box('Finish Goods Effective Date Cannot Be Less Than Plan Date'); focusEl('datEffectiveDateFinish'); return false;
        }
        return true;
    }

    /** Insert():1655-1745 - every field, in the model's names. */
    function collect() {
        var rates = [];
        byProduct.forEach(function (r) {
            rates.push({ EffectiveDate: isoOf(r.EffectiveDate), ItemRate: num(r.Rate40Kg), Id: intOf(r.Id),
                         ItemId: intOf(r.ItemId), JobOrderId: 0, TransTypeId: 1 });
        });
        finish.forEach(function (r) {
            rates.push({ EffectiveDate: isoOf(r.EffectiveDate), ItemRate: num(r.Rate40Kg), Id: intOf(r.Id),
                         ItemId: intOf(r.ItemId), JobOrderId: 0, TransTypeId: 2 });
        });
        return {
            Id: recId,
            PlanDate: val('txtPlanDate'),
            PlanCode: intOf(val('txtPlanCode')),
            /* :1658 - the header takes whatever the Plant/Feader picker holds at that moment. */
            InvProductionPlantId: intOf(val('cmbPlantFeader')),
            PlanType: selText('cmbPlanType'),
            PlanTypeSrNo: intOf(val('txtPlanTypeSrNo')),
            RefInvoiceNo: flags.generateJobOrderNo ? selText('cmbProductionNo') : val('txtProductionNo'),
            LotReference: val('txtOtherReference'),
            ProductionType: selText('cmbProductionType'),
            WorkInProccessAcId: intOf(val('cmbWorkInProcess')),
            FinishGoodsAcId: intOf(val('cmbFinishGoodsAc')),
            ByProductacId: intOf(val('cmbByProductionAc')),
            WipItemId: intOf(val('cmbWipItem')),
            WipWareHouseId: intOf(val('cmbWipWarehouse')),
            StartDate: val('txtStartDate'),
            EndDate: val('txtEndDate'),
            PlanStatus: selText('cmbStatus'),
            FinishGoodRate: num(val('txtAvgFinishGoodRate').trim()),
            OtherInstructions: val('txtOtherInstruction'),
            InvProductionJobOrderPlantslist: plants.map(function (r) {
                return { Id: intOf(r.Id), InvProductionJobOrderId: 0, PlantId: intOf(r.PlantId), RemarksDetail: r.RemarksDetail };
            }),
            JobOrderRateSchedulelist: rates,
            InvProductionJobOrderAndOrderAllocationList: schedule.map(function (r) {
                return { MTon: num(r.MTon), Id: intOf(r.Id), InvProductionJobOrderId: 0, ItemId: intOf(r.ItemId),
                         OrderId: intOf(r.ContractId), OrderTypeId: 1, ScheduleId: intOf(r.ScheduleId), Remarks: r.Remarks };
            })
        };
    }

    /** Insert():1606. The plant / by-product / finish checks come AFTER the confirm on the
     *  desktop, so they are left to the server, which answers with the same words. */
    function insert(btnId) {
        return busy(btnId, function () {
            if (!headerOk()) return;
            var wasUpdate = recId > 0;
            if (!ask(wasUpdate ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            return request(api + '/save', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(collect())
            }).then(function (d) {
                box((d && d.message) || (wasUpdate ? 'Update Successfully' : 'Save Successfully'));
                if (wasUpdate) updateMode = false;
                var printId = d ? intOf(d.id) : 0;
                return reset().then(function () {
                    if ($id('chkPrint').checked && printId > 0) return print(printId, null);
                });
            }).catch(function (e) { box(e.message); });
        });
    }

    function save()   { recId = 0; return insert('btnSave'); }     // btnsave_Click:1781
    function saveAs() { recId = 0; return insert('btnSaveAs'); }   // btnSaveAs_Click:1810
    function update() {                                             // btnUpdate_Click:1794
        if (recId === 0) { box('Record Not Update Because Record Not Found'); return; }
        return insert('btnUpdate');
    }

    /* ------------------------------------------------------------------ print (620) */

    /** CommonServices.PrintJobOrder620 - pre-check, then the Crystal slip. */
    function print(id, btnId) {
        var run = function () {
            var w = window.open('', '_blank');
            return getJson(api + '/print-check?id=' + id).then(function (d) {
                if (d && d.message) { if (w) w.close(); box(d.message); return; }
                var opt = { method: 'POST', credentials: 'same-origin',
                            headers: { 'Content-Type': 'application/json', Accept: 'application/pdf, text/plain' },
                            body: JSON.stringify({ id: id }) };
                csrf(opt);
                return fetch('/api/reports/jo-620/print.pdf', opt).then(function (r) {
                    var type = r.headers.get('Content-Type') || '';
                    if (r.ok && type.indexOf('application/pdf') >= 0) {
                        return r.blob().then(function (b) {
                            var url = URL.createObjectURL(b);
                            if (w) w.location.href = url; else window.open(url, '_blank');
                        });
                    }
                    return r.text().then(function (t) { if (w) w.close(); box(t || ('Print failed (' + r.status + ')')); });
                });
            }).catch(function (e) { if (w) w.close(); box(e.message); });
        };
        return btnId ? busy(btnId, run) : run();
    }

    /** Print_Click:2307 - prints the CURRENT HISTORY ROW, and sets RECID to it. */
    function printCurrent() {
        var r = historyRows[historyCurrent];
        if (!r) {
            /* The desktop dereferences DataGridHistory.CurrentRow without a check here, so with no
               history row it fails with a null-reference error. */
            box('Object reference not set to an instance of an object.\n\n(620-Print prints the row selected in History - show History and select a row first.)');
            return;
        }
        recId = intOf(r.Id);
        return print(recId, 'btnPrint');
    }

    /* ------------------------------------------------------------------ open a record */

    /** ReadById:1823. */
    function readById(id, saveAsMode) {
        return getJson(api + '/' + id).then(function (d) {
            if (!d) return;
            recId = id;
            var h = d.header || {};
            tab('tabForm');
            setVal('txtPlanDate', isoOf(h.PlanDate));
            setVal('txtPlanCode', h.PlanCode);
            setVal('cmbWorkInProcess', intOf(h.WorkInProccessAcId) || '');
            setVal('cmbFinishGoodsAc', intOf(h.FinishGoodsAcId) || '');
            setVal('cmbByProductionAc', intOf(h.ByProductacId) || '');
            setVal('cmbWipItem', intOf(h.WipItemId) || '');
            setVal('cmbWipWarehouse', intOf(h.WipWareHouseId) || '');
            setVal('cmbPlanType', h.PlanType);
            setVal('txtPlanTypeSrNo', h.PlanTypeSrNo);
            if (flags.generateJobOrderNo) {
                bindIf('cmbProductionNo', d.generatedJobOrderNos, 'Id', 'FinalInvoiceNo', true);
                setVal('cmbProductionNo', h.RefInvoiceNo);
            } else {
                setVal('txtProductionNo', h.RefInvoiceNo);
            }
            setVal('txtOtherReference', h.LotReference);
            setVal('cmbProductionType', h.ProductionType);
            setVal('txtStartDate', isoOf(h.StartDate));
            setVal('txtEndDate', isoOf(h.EndDate));
            setVal('cmbStatus', h.PlanStatus);
            setVal('txtAvgFinishGoodRate', h.FinishGoodRate === null || h.FinishGoodRate === undefined ? '' : String(num(h.FinishGoodRate)));
            setVal('txtOtherInstruction', h.OtherInstructions);

            /* saveAs: every detail row comes in with Id 0 (:1864, :1874, :1896). */
            var z = function (r) { if (saveAsMode) r.Id = 0; return r; };
            plants = (d.plants || []).map(z);
            byProduct = (d.byProduct || []).map(z);
            finish = (d.finishGoods || []).map(z);
            schedule = (d.schedule || []).map(z);
            current = { plant: -1, 1: -1, 2: -1 };
            editPlantIndex = -1; editRateIndex = { 1: -1, 2: -1 };
            renderAllGrids();
            updateMode = true;
            $id('lblRecId').textContent = (saveAsMode ? 'Copy of #' : 'Record #') + id;
        }).catch(function (e) { box(e.message); });
    }

    /** DataGridHistory_ColumnButtonClick:2112 and DataGridHistory_DoubleClick:1924. */
    function historyAction(i, action) {
        var r = historyRows[i];
        if (!r) return;
        setHistoryCurrent(i);
        var id = intOf(r.Id);
        if (action === 'edit') {
            vis.save = false; vis.saveAs = false; vis.update = true; applyButtons();
            return readById(id, false);
        }
        if (action === 'saveas') {
            vis.save = false; vis.update = false; vis.saveAs = true; applyButtons();
            return readById(id, true);
        }
        if (action === 'dbl') {
            vis.save = false; vis.update = true; applyButtons();
            return readById(id, false);
        }
        if (action === 'print') return print(id, null);   // no rights check on the desktop column
        if (action === 'attach') return openAttachments(id);
    }

    /* ------------------------------------------------------------------ new / refresh */

    /** Reset():1427. NOT cleared, as on the desktop: Start/End Date, Average Finish Rate,
     *  PlanTypeSrNo, the Comments box, the effective dates, RECID and the SaveAs button. */
    function reset() {
        setVal('txtPlanDate', today());
        setVal('txtProductionNo', '');
        setVal('cmbProductionNo', '');
        setVal('txtOtherInstruction', '');
        setVal('txtOtherReference', '');
        setVal('cmbWorkInProcess', '');
        setVal('cmbWipItem', '');
        setVal('cmbWipWarehouse', '');
        setVal('cmbFinishGoodsAc', '');
        setVal('cmbByProductionAc', '');
        setVal('cmbProductionType', '');
        setVal('cmbStatus', '');
        setVal('cmbPlanType', '');
        vis.update = false; vis.save = true;
        updateMode = false;
        $id('lblRecId').textContent = '';

        plants = []; byProduct = []; finish = []; schedule = [];
        current = { plant: -1, 1: -1, 2: -1 };
        setVal('cmbPlantFeader', '');
        show('btnPlus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetail', false);
        editPlantIndex = -1;
        editRateIndex = { 1: -1, 2: -1 };
        $id('btnAddUpdateByProduct').textContent = 'Add'; show('btnCancelByProduct', false);
        $id('btnAddUpdateFinish').textContent = 'Add'; show('btnCancelFinish', false);
        applyButtons();
        renderAllGrids();
        focusEl('txtPlanDate');

        /* GeneratePlanCode, GetLastSaveAccount and (with the flag) BindJobOrderNo run again. */
        return getJson(api + '/lookups').then(function (d) {
            d = d || {};
            if (intOf(d.planCode) > 0) setVal('txtPlanCode', d.planCode);
            restoreLastWip(d);
            if (flags.generateJobOrderNo) bindIf('cmbProductionNo', d.generatedJobOrderNos, 'Id', 'FinalInvoiceNo', true);
        }).catch(function (e) { box(e.message); });
    }

    /** btnRefresh_Click:1506 - flags, Production # list, WIP items, accounts, plan types,
     *  production types, plan code and plants. Nothing on the form is cleared. */
    function refresh() {
        return busy('btnRefresh', function () {
            return getJson(api + '/lookups').then(function (d) {
                d = d || {};
                applyFlagsAndRights(d);
                if (flags.generateJobOrderNo) bindIf('cmbProductionNo', d.generatedJobOrderNos, 'Id', 'FinalInvoiceNo', true);
                bindIf('cmbWipItem', d.wipItems, 'Id', 'ItemName');
                bindAccounts(d);
                bindIf('cmbPlanType', d.planTypes, 'Id', 'PlanTypeDescription', true);
                bindIf('cmbProductionType', d.productionTypes, 'Id', 'ProductionTypeDescription', true);
                if (intOf(d.planCode) > 0) setVal('txtPlanCode', d.planCode);
                bindIf('cmbPlantFeader', d.plants, 'PlantId', 'PlantName');
                applyButtons();
            }).catch(function (e) { box(e.message); });
        });
    }

    /* ------------------------------------------------------------------ history */

    function dateMode() {
        var r = document.querySelector('input[name="dateMode"]:checked');
        return r ? r.value : 'doc';
    }

    /** cmbperemeter_ValueChanged:2142. */
    function datePreset() {
        var v = intOf(val('cmbPeremeter')), d = new Date();
        if (v === 1) setVal('datHistoryFrom', iso(d));
        else if (v === 2) { var w = new Date(); w.setDate(w.getDate() - 7); setVal('datHistoryFrom', iso(w)); }
        else if (v === 3) { setVal('datHistoryFrom', iso(new Date(d.getUTCFullYear(), d.getUTCMonth(), 1))); setVal('datHistoryTo', iso(d)); }
        else if (v === 4) { setVal('datHistoryFrom', iso(new Date(d.getFullYear(), 0, 1))); setVal('datHistoryTo', iso(d)); }
        else if (v === 5) { if (financialYearStart) setVal('datHistoryFrom', financialYearStart); setVal('datHistoryTo', iso(d)); }
    }

    /** btnNewHistory_Click:1908. */
    function newHistory() {
        var per = $id('cmbPeremeter');
        if (per.options.length > 1) { per.value = per.options[1].value; datePreset(); }
        setVal('txtOrderNoFrom', '');
        setVal('txtOrderNoTo', '');
        setVal('cmbHistoryJobOrder', '');
        focusEl('cmbPeremeter');
    }

    var HISTORY_COLS = [
        ['Date', fmtShort, 100], ['Code', null, 50], ['Type', null, 70], ['Production#', null, 60], ['PType', null, 80],
        ['StartDate', fmtShort, 100], ['EndDate', fmtShort, 100], ['PlanStatus', null, 80], ['OtherInst', null, 150],
        ['EntryUser', null, 0], ['EntryDate', fmtStamp, 0], ['ModifyUser', null, 0], ['ModifyDate', fmtStamp, 0],
        ['ApprovedStatus', null, 0], ['ApprovedUser', null, 0], ['ApprovedDate', fmtStamp, 0], ['Attachments', null, 0]
    ];

    /** BindHistoryGrid:1938 / HgridSetting:2030 - Edit, SaveAs, Print frozen in front. */
    function history() {
        return busy('btnShowHistory', function () {
            /* The two pickers have no ShowCheckBox, so .Checked is always true - both dates go. */
            var q = ['dateMode=' + encodeURIComponent(dateMode()),
                     'fromDate=' + encodeURIComponent(val('datHistoryFrom')),
                     'toDate=' + encodeURIComponent(val('datHistoryTo')),
                     'docNoFrom=' + intOf(val('txtOrderNoFrom')),
                     'docNoTo=' + intOf(val('txtOrderNoTo')),
                     'jobOrderId=' + intOf(val('cmbHistoryJobOrder'))];
            return getJson(api + '/history?' + q.join('&')).then(function (d) {
                historyRows = (d && d.rows) || [];
                historyCurrent = historyRows.length ? 0 : -1;
                $id('lblHistoryScope').textContent = (d && d.canViewAllRecords) ? 'All users' : 'Your records only';
                renderHistory();
            }).catch(function (e) {
                historyRows = []; historyCurrent = -1; renderHistory();
                box(e.message);
            });
        });
    }

    function renderHistory() {
        $id('historyHead').innerHTML = '<th style="width:40px;">Edit</th><th style="width:60px;">SaveAs</th><th style="width:50px;">Print</th>'
            + HISTORY_COLS.map(function (c) {
                  return '<th' + (c[2] ? ' style="min-width:' + c[2] + 'px;"' : '') + (c[0] === 'Code' ? ' class="num"' : '') + '>' + esc(c[0]) + '</th>';
              }).join('');
        $id('historyBody').innerHTML = historyRows.length ? historyRows.map(function (r, i) {
            return '<tr tabindex="0" data-kind="history" data-i="' + i + '" class="' + (i === historyCurrent ? 'is-current' : '') + '">'
                 + '<td><button type="button" class="jo-cellbtn" tabindex="-1" data-act="edit">Edit</button></td>'
                 + '<td><button type="button" class="jo-cellbtn" tabindex="-1" data-act="saveas">SaveAs</button></td>'
                 + '<td><button type="button" class="jo-cellbtn" tabindex="-1" data-act="print">Print</button></td>'
                 + HISTORY_COLS.map(function (c) {
                       var v = r[c[0]];
                       if (c[0] === 'Code') {
                           return '<td class="num"><button type="button" class="jo-link" tabindex="-1" data-act="dbl" '
                                + 'title="Open this job order (same as double-click)">' + esc(v) + '</button></td>';
                       }
                       if (c[0] === 'Attachments') {
                           /* ColumnType.Link - DataGridHistory_LinkClicked:2082. */
                           return '<td class="num"><button type="button" class="jo-link" tabindex="-1" data-act="attach">'
                                + esc(v === null || v === undefined || v === '' ? '0' : v) + '</button></td>';
                       }
                       return '<td' + (c[0] === 'OtherInst' ? ' class="wrap"' : '') + '>' + esc(c[1] ? c[1](v) : v) + '</td>';
                   }).join('') + '</tr>';
        }).join('') : '<tr class="jo-empty"><td colspan="' + (HISTORY_COLS.length + 3) + '">No records</td></tr>';
        $id('lblHistoryCount').textContent = historyRows.length + ' record(s)';
    }

    function setHistoryCurrent(i) {
        historyCurrent = i;
        $id('historyBody').querySelectorAll('tr[data-kind="history"]').forEach(function (tr) {
            tr.classList.toggle('is-current', +tr.getAttribute('data-i') === i);
        });
    }

    function openAttachments(id) {
        $id('attachBody').innerHTML = '<tr class="jo-empty"><td>Loading...</td></tr>';
        $id('attachModal').classList.add('is-open');
        return getJson(api + '/attachments?id=' + id).then(function (rows) {
            rows = rows || [];
            $id('attachBody').innerHTML = rows.length
                ? rows.map(function (r) { return '<tr><td class="wrap">' + esc(r.Attachment) + '</td></tr>'; }).join('')
                : '<tr class="jo-empty"><td>No attachments</td></tr>';
        }).catch(function (e) { $id('attachBody').innerHTML = ''; box(e.message); });
    }
    function closeAttachments() { $id('attachModal').classList.remove('is-open'); }

    /* ------------------------------------------------------------------ grid events */

    function rowOf(e) {
        var tr = e.target.closest('tr[data-kind]');
        return tr ? { kind: tr.getAttribute('data-kind'), i: +tr.getAttribute('data-i'), tr: tr } : null;
    }
    function markCurrent(kind, i) {
        if (kind === 'plant') current.plant = i;
        else if (kind === 'rate1') current[1] = i;
        else if (kind === 'rate2') current[2] = i;
        var body = kind === 'plant' ? 'gridPlant' : kind === 'rate1' ? 'gridByProduct' : 'gridFinish';
        $id(body).querySelectorAll('tr[data-kind]').forEach(function (tr) {
            if (!tr.classList.contains('is-editing')) tr.classList.toggle('is-current', +tr.getAttribute('data-i') === i);
        });
    }

    function wireGrids() {
        [['gridPlant', 'plant'], ['gridByProduct', 'rate1'], ['gridFinish', 'rate2']].forEach(function (g) {
            var body = $id(g[0]);
            body.addEventListener('click', function (e) {
                var r = rowOf(e); if (!r) return;
                markCurrent(r.kind, r.i);
                var btn = e.target.closest('button[data-act="del"]');
                if (!btn) return;
                if (r.kind === 'plant') deletePlant(r.i); else deleteRate(r.kind === 'rate1' ? 1 : 2, r.i);
            });
            body.addEventListener('dblclick', function (e) {
                var r = rowOf(e); if (!r || e.target.closest('button')) return;
                if (r.kind === 'plant') editPlant(r.i); else editRate(r.kind === 'rate1' ? 1 : 2, r.i);
            });
            /* grdByProduct_KeyDown:1179 / grdFinishGoods_KeyDown:1376 - Ctrl+Enter edits the current
               row, Ctrl+Space on it deletes. GridDetail has no KeyDown handler on the desktop. */
            body.addEventListener('keydown', function (e) {
                var r = rowOf(e); if (!r || r.kind === 'plant' || !e.ctrlKey) return;
                var type = r.kind === 'rate1' ? 1 : 2;
                if (e.key === 'Enter') { e.preventDefault(); e.stopPropagation(); editRate(type, r.i); }
                else if (e.key === ' ') { e.preventDefault(); deleteRate(type, r.i); }
            });
            body.addEventListener('focusin', function (e) { var r = rowOf(e); if (r) markCurrent(r.kind, r.i); });
        });

        $id('gridSchedule').addEventListener('input', function (e) {
            var t = e.target;
            if (t && t.getAttribute('data-act') === 'remarks') {
                var i = +t.getAttribute('data-i');
                if (schedule[i]) schedule[i].Remarks = t.value;
            }
        });

        var hb = $id('historyBody');
        hb.addEventListener('click', function (e) {
            var r = rowOf(e); if (!r) return;
            setHistoryCurrent(r.i);
            var btn = e.target.closest('button[data-act]');
            if (btn) historyAction(r.i, btn.getAttribute('data-act'));
        });
        hb.addEventListener('dblclick', function (e) {
            var r = rowOf(e); if (!r || e.target.closest('button')) return;
            historyAction(r.i, 'dbl');
        });
        hb.addEventListener('focusin', function (e) { var r = rowOf(e); if (r) setHistoryCurrent(r.i); });
    }

    /* ------------------------------------------------------------------ keyboard */

    /** SendKeys.Send("{TAB}") on Enter - the next focusable control. */
    function focusNext(from) {
        var all = Array.prototype.filter.call(
            document.querySelectorAll('input, select, textarea, button, [tabindex="0"]'),
            function (el) {
                return !el.disabled && el.tabIndex >= 0 && el.offsetParent !== null
                    && !(el.tagName === 'SELECT' && el.classList.contains('dtcombo') && getComputedStyle(el).display === 'none');
            });
        var i = all.indexOf(from);
        if (i >= 0 && i + 1 < all.length) all[i + 1].focus();
    }

    function anyModalOpen() { return !!document.querySelector('.cx-modal.is-open'); }

    /** frmProductionJobOrder_KeyDown:2192. */
    function onKey(e) {
        var t = e.target, k = e.key, ctrl = e.ctrlKey || e.metaKey;
        if (k === 'Escape') {
            if (e.defaultPrevented) return;                 // a combo closed its list
            if (anyModalOpen()) { closeLoader(); closeAttachments(); return; }
            var f = document.querySelector('.jo-full');
            if (f) { f.classList.remove('jo-full'); return; }
            window.location.href = '/production';           // Close()
            return;
        }
        if (k === 'Enter' && !ctrl) {
            if (e.defaultPrevented || !t || t.tagName === 'BUTTON' || t.tagName === 'TEXTAREA') return;
            if (t.closest && t.closest('.cx-modal')) return;
            if (t.tagName === 'TR') return;
            e.preventDefault();
            focusNext(t);
            return;
        }
        if (!ctrl) return;
        var key = k.length === 1 ? k.toLowerCase() : k;
        var tabIdx = currentTab();

        if (key === 's') {
            e.preventDefault();
            if (tabIdx === 0 && !updateMode) save();
            else if (tabIdx === 1) history();
            return;
        }
        if (key === 'n') {
            e.preventDefault();
            reset();
            if (tabIdx === 1) newHistory();
            return;
        }
        if (key === 't') { e.preventDefault(); tab(tabIdx === 1 ? 'tabForm' : 'tabHistory'); return; }
        if (key === 'e') { e.preventDefault(); window.location.href = '/production'; return; }
        if (key === 'u') { e.preventDefault(); if (updateMode) update(); return; }
        if (key === 'ArrowDown') {
            e.preventDefault();
            if (tabIdx === 1) { focusFirstRow('historyBody'); return; }
            var inPlant = t.closest && t.closest('#gridPlant'), inBy = t.closest && t.closest('#gridByProduct'), inFin = t.closest && t.closest('#gridFinish');
            focusFirstRow(inPlant ? 'gridByProduct' : inBy ? 'gridFinish' : 'gridPlant');
            return;
        }
        if (key === 'ArrowUp') {
            e.preventDefault();
            if (tabIdx === 1) { focusEl('cmbPeremeter'); return; }
            if (t.closest && t.closest('#gridByProduct')) focusEl('datEffectiveDateByProduct');
            else if (t.closest && t.closest('#gridFinish')) focusEl('datEffectiveDateFinish');
            else focusEl('cmbPlantFeader');
        }
    }
    function focusFirstRow(bodyId) {
        var tr = $id(bodyId).querySelector('tr[tabindex="0"].is-current') || $id(bodyId).querySelector('tr[tabindex="0"]');
        if (tr) tr.focus();
    }

    /* ------------------------------------------------------------------ boot */

    function boot() {
        /* Designer defaults: every DateTimePicker starts at DateTime.Now. */
        ['txtPlanDate', 'txtStartDate', 'txtEndDate', 'datEffectiveDateByProduct', 'datEffectiveDateFinish',
         'datHistoryFrom', 'datHistoryTo'].forEach(function (id) { setVal(id, today()); });

        document.addEventListener('keydown', onKey);

        /* OnlytextdecimelFunction - txtAvgFinishGoodRate :2349 and both rate boxes; the two Doc
           No boxes take digits only. */
        ['txtAvgFinishGoodRate', 'txtRateByProduct', 'txtRateFinish', 'txtOrderNoFrom', 'txtOrderNoTo'].forEach(function (id) {
            var el = $id(id);
            if (!el) return;
            var decimals = !(id === 'txtOrderNoFrom' || id === 'txtOrderNoTo');
            el.addEventListener('keypress', function (e) {
                if (e.key.length !== 1 || e.ctrlKey) return;
                if (/[0-9]/.test(e.key)) return;
                if (decimals && e.key === '.' && el.value.indexOf('.') < 0) return;
                e.preventDefault();
            });
        });

        wireGrids();
        applyButtons();
        renderAllGrids();
        renderHistory();
        loadAll().catch(function (e) { box(e.message); });
        focusEl('txtPlanDate');
    }

    window.JobOrderMain = {
        tab: tab, tab2: tab2, full: full,
        addPlant: addPlant, updatePlant: updatePlant, cancelPlant: cancelPlant,
        addRate: addRate, cancelRate: cancelRate,
        openLoader: openLoader, closeLoader: closeLoader, loaderShow: loaderShow, loaderApply: loaderApply,
        save: save, saveAs: saveAs, update: update, printCurrent: printCurrent,
        reset: reset, refresh: refresh,
        history: history, datePreset: datePreset, newHistory: newHistory,
        closeAttachments: closeAttachments
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
    else boot();
}());
