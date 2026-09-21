/* ============================================================================================
 * Screen 281 "Production Job Order" — Architecture.WinApp.Production.frmProductionJobOrderMain.
 *
 * Every rule below is the desktop form's, with its own message text:
 *
 *   btnplus_Click:905          Plant required, no duplicate plant, row keeps its Comments box
 *   btnAddUpdateByProduct:1107 Item + Rate required, no (EffectiveDate, Item) duplicate
 *   btnAddUpdateFinish:1309    the same, on the Finish Goods grid
 *   LoadInGrid:2582            a schedule already in the grid (ScheduleId + ItemId) is skipped
 *   btnsave_Click:1863         RECID = 0 then Insert()  -> Save ALWAYS inserts, even on a record
 *                              that was opened from History. SaveAs does the same.
 *   btnUpdate_Click:1876       refuses with "Record Not Update Because Record Not Found" at 0
 *   Insert():1678              the nine validations, in order
 *
 * Delete behaves differently before and after the record exists, exactly as the desktop does:
 * on an unsaved job order a row is simply dropped from the grid; on a saved one a plant row is
 * first checked against Production, and a rate row is deleted in the database there and then.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/production/job-order-main';

    /* ------------------------------------------------------------------ state */

    var recId = 0;                 // RECID
    var flags = {                  // the two configuration switches, from the server
        byProductRateEditableIsAllow: false,
        generateJobOrderNo: false
    };
    var rights = { canSave: false, canUpdate: false, canPrint: false };
    var financialYearStart = '';

    var plants = [];               // dtDetail        {Id, PlantId, PlantName, RemarksDetail}
    var byProduct = [];            // dtByProduct     {Id, EffectiveDate, ItemId, ItemName, Rate40Kg}
    var finish = [];               // dtFinish        same shape
    var schedule = [];             // dtExportSchedule{Id, ContractId, SaleContract, ScheduleId,
                                   //                  ScheduleNo, ItemId, Item, MTon, Remarks}
    var loaderRows = [];           // the dialog's current result set

    var editPlantIndex = -1;       // UpdateDetailIndex
    var editRateIndex = { 1: -1, 2: -1 };   // UpdateDetailIndexForByProduct / ...ForFinishGoods

    /* ------------------------------------------------------------------ dom helpers */

    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function box(m) { window.alert(m); }
    function ask(m) { return window.confirm(m); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }

    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function ci(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        for (var k in row) {
            if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === name.toLowerCase()) {
                return row[k];
            }
        }
        return '';
    }
    function num(v) {
        var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, ''));
        return isNaN(n) ? 0 : n;
    }
    function intOf(v) { return Math.trunc(num(v)); }

    /** "dd-MMM-yy" — GridByProductSetting:805 gives EffectiveDate this format. */
    var MONTHS = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    function gridDate(iso) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(iso || ''));
        if (!m) return String(iso || '');
        return m[3] + '-' + MONTHS[parseInt(m[2], 10) - 1] + '-' + m[1].slice(2);
    }
    /** "#,##0.###" — ScheduleGridSetting:884 gives MTon this format. */
    function fmtMTon(v) {
        var n = num(v);
        var s = n.toFixed(3).replace(/0+$/, '').replace(/\.$/, '');
        var parts = s.split('.');
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return parts.join('.');
    }
    function fmtRate(v) {
        var n = num(v);
        return n.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    }
    function today() {
        var d = new Date();
        return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0')
             + '-' + String(d.getDate()).padStart(2, '0');
    }
    function isoOf(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || ''));
        return m ? m[0] : '';
    }

    /* Disable → spinner → ignore repeat clicks → re-enable on success AND failure. A double
       click on Save must not be able to write two job orders. */
    function busy(btnId, fn) {
        var b = $id(btnId);
        if (b) {
            if (b.disabled || b.classList.contains('is-busy')) return;
            b.disabled = true;
            b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } };
        var p;
        try { p = fn(); } catch (e) { done(); throw e; }
        if (p && typeof p.then === 'function') p.then(done, done); else done();
        return p;
    }

    function request(url, options) {
        var opt = options || {};
        opt.credentials = 'same-origin';
        opt.headers = opt.headers || {};
        opt.headers.Accept = 'application/json';
        var token = document.querySelector('meta[name="_csrf"]');
        var header = document.querySelector('meta[name="_csrf_header"]');
        if (token && header && opt.method && opt.method.toUpperCase() !== 'GET') {
            opt.headers[header.getAttribute('content')] = token.getAttribute('content');
        }
        return fetch(url, opt).then(function (r) {
            return r.text().then(function (t) {
                var body = null;
                try { body = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
                if (!r.ok) {
                    throw new Error((body && body.message) || ('Request failed (' + r.status + ')'));
                }
                return body;
            });
        });
    }
    function getJson(url) { return request(url, {}); }

    /* ------------------------------------------------------------------ pickers */

    function fill(selectId, rows, valueKey, textKey) {
        var el = $id(selectId);
        if (!el) return;
        /* ZeroIndex is false on every picker of this form, so there is no blank "…Select Any
           Value…" row — but an empty first option is still needed so a field can read as unset. */
        var html = '<option value=""></option>';
        (rows || []).forEach(function (r) {
            html += '<option value="' + esc(ci(r, valueKey)) + '">' + esc(ci(r, textKey)) + '</option>';
        });
        el.innerHTML = html;
        if (window.DesktopCombo && window.DesktopCombo.refresh) window.DesktopCombo.refresh();
    }

    /** A combo whose stored value is its TEXT, not an id — cmbPlanType, cmbProductionType,
     *  cmbStatus and CmbProductionNo all save their caption (Insert():1740-1753). */
    function fillByText(selectId, rows, textKey) {
        var el = $id(selectId);
        if (!el) return;
        var html = '<option value=""></option>';
        (rows || []).forEach(function (r) {
            var t = ci(r, textKey);
            html += '<option value="' + esc(t) + '">' + esc(t) + '</option>';
        });
        el.innerHTML = html;
        if (window.DesktopCombo && window.DesktopCombo.refresh) window.DesktopCombo.refresh();
    }

    function loadLookups() {
        return getJson(api + '/lookups').then(function (d) {
            d = d || {};
            rights.canSave   = !!d.canSave;
            rights.canUpdate = !!d.canUpdate;
            rights.canPrint  = !!d.canPrint;
            flags.byProductRateEditableIsAllow = !!d.byProductRateEditableIsAllow;
            flags.generateJobOrderNo = !!d.generateJobOrderNo;
            financialYearStart = isoOf(d.financialYearStart);

            fill('cmbPlantFeader',    d.plants,       'PlantId', 'PlantName');
            fill('cmbWipItem',        d.wipItems,     'Id',      'ItemName');
            fill('cmbWorkInProcess',  d.accounts,     'Id',      'AccountTitle');
            fill('cmbFinishGoodsAc',  d.accounts,     'Id',      'AccountTitle');
            fill('cmbByProductionAc', d.accounts,     'Id',      'AccountTitle');
            fill('cmbWipWarehouse',   d.warehouses,   'Id',      'WarehouseName');
            fill('cmbItemByProduct',  d.items,        'Id',      'ItemName');
            fill('cmbItemFinish',     d.items,        'Id',      'ItemName');
            fill('cmbHistoryJobOrder',d.jobOrderNos,  'Id',      'Name');

            fillByText('cmbPlanType',       d.planTypes,       'PlanTypeDescription');
            fillByText('cmbProductionType', d.productionTypes, 'ProductionTypeDescription');
            fillByText('cmbStatus',         d.statuses,        'Status');
            fillByText('cmbProductionNo',   d.generatedJobOrderNos, 'FinalInvoiceNo');

            /* cmbperemeter — the five date presets. */
            var per = $id('cmbPeremeter');
            per.innerHTML = '<option value=""></option>' + (d.dateTypes || []).map(function (r) {
                return '<option value="' + esc(ci(r, 'Id')) + '">' + esc(ci(r, 'Parameters')) + '</option>';
            }).join('');

            /* :379-383 — one of the two Production # controls, never both. */
            show('fldProductionNoCombo', flags.generateJobOrderNo);
            show('fldProductionNoText', !flags.generateJobOrderNo);

            setVal('txtPlanCode', d.planCode || '');

            /* GetLastSaveAccount:568 — restored on a fresh form. */
            if (intOf(d.lastWipAccountId)   > 0) setVal('cmbWorkInProcess', d.lastWipAccountId);
            if (intOf(d.lastWipItemId)      > 0) setVal('cmbWipItem',       d.lastWipItemId);
            if (intOf(d.lastWipWareHouseId) > 0) setVal('cmbWipWarehouse',  d.lastWipWareHouseId);

            applyRights();
        });
    }

    /** :372-374 — the desktop simply disables the buttons; so does this, and the server refuses
     *  the same operations regardless, because a disabled button is not a permission check. */
    function applyRights() {
        $id('btnSave').disabled   = !rights.canSave;
        $id('btnSaveAs').disabled = !rights.canSave;
        $id('btnUpdate').disabled = !rights.canUpdate;
        $id('btnPrint').disabled  = !rights.canPrint;

        var missing = [];
        if (!rights.canSave)   missing.push('Save');
        if (!rights.canUpdate) missing.push('Update');
        if (!rights.canPrint)  missing.push('Print');
        var note = $id('rightsNote');
        if (missing.length) {
            note.textContent = 'Your account does not have the ' + missing.join(', ')
                             + ' right on this screen, so those buttons are off.';
            note.classList.remove('is-hidden');
        } else {
            note.classList.add('is-hidden');
        }
    }

    /* ------------------------------------------------------------------ tabs */

    function tab(id) {
        ['tabForm', 'tabHistory'].forEach(function (t) {
            $id(t).classList.toggle('is-active', t === id);
        });
        document.querySelectorAll('.win-tabs')[0].querySelectorAll('.win-tab').forEach(function (b) {
            b.classList.toggle('is-active', b.getAttribute('data-tab') === id);
        });
    }
    function tab2(id) {
        ['tabDetail', 'tabSchedule'].forEach(function (t) {
            $id(t).classList.toggle('is-active', t === id);
        });
        document.querySelectorAll('.win-tabs')[1].querySelectorAll('.win-tab').forEach(function (b) {
            b.classList.toggle('is-active', b.getAttribute('data-tab') === id);
        });
    }

    /* ------------------------------------------------------- plant grid (GridDetail) */

    function renderPlants() {
        $id('gridPlant').innerHTML = plants.map(function (r, i) {
            return '<tr class="' + (i === editPlantIndex ? 'is-editing' : '') + '"'
                 + ' ondblclick="JobOrderMain.editPlant(' + i + ')">'
                 + '<td class="cx-col-x"><button type="button" class="cx-x"'
                 + ' onclick="event.stopPropagation();JobOrderMain.deletePlant(' + i + ')">X</button></td>'
                 + '<td>' + esc(r.PlantName) + '</td>'
                 + '<td>' + esc(r.RemarksDetail) + '</td></tr>';
        }).join('');
        $id('lblPlantCount').textContent = plants.length + ' record(s)';
    }

    /** FormValidationDetail:936. */
    function plantFieldOk() {
        if (intOf(val('cmbPlantFeader')) === 0) {
            box('Plant Feeder Field is Required');
            $id('cmbPlantFeader').focus();
            return false;
        }
        return true;
    }
    function plantText() {
        var el = $id('cmbPlantFeader');
        var o = el.options[el.selectedIndex];
        return o ? o.text.trim() : '';
    }

    function addPlant() {
        if (!plantFieldOk()) return;
        var id = intOf(val('cmbPlantFeader'));
        for (var i = 0; i < plants.length; i++) {
            if (intOf(plants[i].PlantId) === id) { box('Same Plant Already Exist In Detail'); return; }
        }
        plants.push({
            Id: 0, PlantId: id, PlantName: plantText(),
            RemarksDetail: val('txtRemarksDetail').trim()
        });
        renderPlants();
        /* :933 clears the plant picker only — the Comments box keeps its text. */
        setVal('cmbPlantFeader', '');
    }

    function editPlant(i) {
        var r = plants[i];
        if (!r) return;
        editPlantIndex = i;
        setVal('cmbPlantFeader', r.PlantId);
        setVal('txtRemarksDetail', r.RemarksDetail);
        show('btnPlus', false);
        show('btnUpdateDetail', true);
        show('btnCancelUpdateDetail', true);
        renderPlants();
    }

    function updatePlant() {
        if (editPlantIndex < 0) return;
        if (!plantFieldOk()) return;
        var id = intOf(val('cmbPlantFeader'));
        for (var i = 0; i < plants.length; i++) {
            if (i !== editPlantIndex && intOf(plants[i].PlantId) === id) {
                box('Same Plant Already Exist In Detail');
                return;
            }
        }
        plants[editPlantIndex].PlantId = id;
        plants[editPlantIndex].PlantName = plantText();
        plants[editPlantIndex].RemarksDetail = val('txtRemarksDetail').trim();
        cancelPlant();
    }

    function cancelPlant() {
        editPlantIndex = -1;
        show('btnPlus', true);
        show('btnUpdateDetail', false);
        show('btnCancelUpdateDetail', false);
        setVal('cmbPlantFeader', '');
        renderPlants();
    }

    /**
     * GridDetail_ColumnButtonClick:1016. Before the record exists the row is simply dropped.
     * On a saved record the desktop asks whether a Production document already used that plant,
     * and refuses by name if so.
     */
    function deletePlant(i) {
        var r = plants[i];
        if (!r) return;
        if (!ask('Are you sure to Delete?')) return;
        if (recId === 0) {
            plants.splice(i, 1);
            if (editPlantIndex === i) cancelPlant(); else renderPlants();
            return;
        }
        getJson(api + '/plant-deletable?jobOrderId=' + recId
                    + '&plantId=' + encodeURIComponent(r.PlantId)
                    + '&plantName=' + encodeURIComponent(r.PlantName || ''))
            .then(function (d) {
                if (d && d.deletable) {
                    plants.splice(i, 1);
                    if (editPlantIndex === i) cancelPlant(); else renderPlants();
                } else {
                    box((d && d.message) || 'That plant has been referred to production.');
                }
            })
            .catch(function (e) { box(e.message); });
    }

    /* ------------------------------------------------- rate grids (By Product / Finish) */

    function rateState(type) {
        return type === 1
            ? { rows: byProduct, grid: 'gridByProduct', count: 'lblByProductCount',
                date: 'datEffectiveDateByProduct', item: 'cmbItemByProduct',
                rate: 'txtRateByProduct', btn: 'btnAddUpdateByProduct', cancel: 'btnCancelByProduct' }
            : { rows: finish,    grid: 'gridFinish',    count: 'lblFinishCount',
                date: 'datEffectiveDateFinish', item: 'cmbItemFinish',
                rate: 'txtRateFinish', btn: 'btnAddUpdateFinish', cancel: 'btnCancelFinish' };
    }

    function renderRates(type) {
        var s = rateState(type);
        $id(s.grid).innerHTML = s.rows.map(function (r, i) {
            return '<tr class="' + (i === editRateIndex[type] ? 'is-editing' : '') + '"'
                 + ' ondblclick="JobOrderMain.editRate(' + type + ',' + i + ')">'
                 + '<td class="cx-col-x"><button type="button" class="cx-x"'
                 + ' onclick="event.stopPropagation();JobOrderMain.deleteRate(' + type + ',' + i + ')">X</button></td>'
                 + '<td>' + esc(gridDate(r.EffectiveDate)) + '</td>'
                 + '<td>' + esc(r.ItemName) + '</td>'
                 + '<td class="num">' + esc(fmtRate(r.Rate40Kg)) + '</td></tr>';
        }).join('');
        $id(s.count).textContent = s.rows.length + ' record(s)';
    }

    /** FormValidationByProduct:1090 / FormValidationFinishGoods:1292 — identical wording. */
    function rateFieldsOk(type) {
        var s = rateState(type);
        if (intOf(val(s.item)) === 0) {
            box('Item Name Field is Required');
            $id(s.item).focus();
            return false;
        }
        if (num(val(s.rate)) === 0) {
            box('Rate40Kg Field is Required');
            $id(s.rate).focus();
            return false;
        }
        return true;
    }
    function itemText(selectId) {
        var el = $id(selectId);
        var o = el.options[el.selectedIndex];
        return o ? o.text : '';
    }

    /** One button: "Add" inserts, "Update" writes back the row being edited (:1107 / :1309). */
    function addRate(type) {
        var s = rateState(type);
        if (!rateFieldsOk(type)) return;
        var d = val(s.date), itemId = intOf(val(s.item));
        var editing = editRateIndex[type];

        for (var i = 0; i < s.rows.length; i++) {
            if (i === editing) continue;
            if (isoOf(s.rows[i].EffectiveDate) === isoOf(d) && intOf(s.rows[i].ItemId) === itemId) {
                box('In Same Effective Date Same Item Already Exist in record. Please Check!');
                return;
            }
        }
        if (editing < 0) {
            s.rows.push({ Id: 0, EffectiveDate: d, ItemId: itemId,
                          ItemName: itemText(s.item), Rate40Kg: num(val(s.rate)) });
        } else {
            s.rows[editing].EffectiveDate = d;
            s.rows[editing].ItemId = itemId;
            s.rows[editing].ItemName = itemText(s.item);
            s.rows[editing].Rate40Kg = num(val(s.rate));
            editRateIndex[type] = -1;
            $id(s.btn).textContent = 'Add';
            show(s.cancel, false);
        }
        /* :1160 — the item and rate clear, the effective date stays for the next row. */
        setVal(s.item, '');
        setVal(s.rate, '');
        renderRates(type);
    }

    function editRate(type, i) {
        var s = rateState(type);
        var r = s.rows[i];
        if (!r) return;
        editRateIndex[type] = i;
        setVal(s.date, isoOf(r.EffectiveDate));
        setVal(s.item, r.ItemId);
        setVal(s.rate, fmtRate(r.Rate40Kg));
        $id(s.btn).textContent = 'Update';
        show(s.cancel, true);
        renderRates(type);
    }

    function cancelRate(type) {
        var s = rateState(type);
        editRateIndex[type] = -1;
        $id(s.btn).textContent = 'Add';
        show(s.cancel, false);
        setVal(s.item, '');
        setVal(s.rate, '');
        renderRates(type);
    }

    /**
     * grdByProduct_ColumnButtonClick:1210 / grdFinishGoods_ColumnButtonClick:1412.
     * On an unsaved job order the row is dropped. On a saved one the desktop deletes it in the
     * database at once — USP_JoBOrderScheduleDeleteById — because the insert procedure only
     * inserts and a removed row would otherwise come straight back on reload.
     */
    function deleteRate(type, i) {
        var s = rateState(type);
        var r = s.rows[i];
        if (!r) return;
        if (!ask('Are you sure to Delete?')) return;

        var drop = function () {
            s.rows.splice(i, 1);
            if (editRateIndex[type] === i) cancelRate(type); else renderRates(type);
        };
        if (recId === 0 || intOf(r.Id) === 0) { drop(); return; }

        request(api + '/delete-rate-schedule-row?jobOrderId=' + recId + '&detailId=' + intOf(r.Id),
                { method: 'POST' })
            .then(drop)
            .catch(function (e) { box(e.message); });
    }

    /* ------------------------------------------------- export schedule grid */

    function renderSchedule() {
        var total = 0;
        $id('gridSchedule').innerHTML = schedule.map(function (r, i) {
            total += num(r.MTon);
            /* ScheduleGridSetting:853 makes every column NoEdit except Remarks. */
            return '<tr>'
                 + '<td class="cx-col-x"><button type="button" class="cx-x"'
                 + ' onclick="JobOrderMain.deleteSchedule(' + i + ')">X</button></td>'
                 + '<td>' + esc(r.SaleContract) + '</td>'
                 + '<td>' + esc(r.ScheduleNo) + '</td>'
                 + '<td>' + esc(r.Item) + '</td>'
                 + '<td class="num">' + esc(fmtMTon(r.MTon)) + '</td>'
                 + '<td><input type="text" class="win-textbox" style="height:20px;"'
                 + ' value="' + esc(r.Remarks) + '"'
                 + ' oninput="JobOrderMain.scheduleRemarks(' + i + ', this.value)"></td>'
                 + '</tr>';
        }).join('');
        /* MTon carries a Sum aggregate on the desktop (:884). */
        $id('gridScheduleFoot').innerHTML = schedule.length
            ? '<tr class="cx-grand"><td></td><td>Total</td><td></td><td></td>'
              + '<td class="num">' + esc(fmtMTon(total)) + '</td><td></td></tr>'
            : '';
        $id('lblScheduleCount').textContent = schedule.length + ' record(s)';
    }

    function scheduleRemarks(i, v) { if (schedule[i]) schedule[i].Remarks = v; }

    function deleteSchedule(i) {
        if (!ask('Are you sure to Delete?')) return;
        schedule.splice(i, 1);
        renderSchedule();
    }

    /* ------------------------------------------------- the loader dialog */

    function openLoader() {
        $id('loaderModal').classList.add('is-open');
        getJson(api + '/schedule-lookups').then(function (d) {
            d = d || {};
            fill('loaderContract',   d.contracts,    'Id', 'Name');
            fill('loaderCustomer',   d.customers,    'Id', 'Name');
            fill('loaderItem',       d.items,        'Id', 'Name');
            fill('loaderThirdParty', d.thirdParties, 'Id', 'Name');
            if (!val('loaderFrom')) setVal('loaderFrom', isoOf(d.defaultFromDate));
            loaderShow();
        }).catch(function (e) { box(e.message); });
    }
    function closeLoader() { $id('loaderModal').classList.remove('is-open'); }

    /* The grid is drawn from whatever columns the procedure returns — the desktop copies the
       result set into dtLoader unchanged (:430), so no column list is invented here. */
    var LOADER_HIDE = ['Id', 'ContractId', 'ItemId', 'SupplierCustomerId'];
    var loaderCols = [];

    function loaderShow() {
        return busy('btnLoaderShow', function () {
            var q = [];
            if (val('loaderFrom')) q.push('fromDate=' + encodeURIComponent(val('loaderFrom')));
            if (val('loaderTo'))   q.push('toDate='   + encodeURIComponent(val('loaderTo')));
            /* GridDataDbCall():290 sends all four unconditionally, zero meaning "no filter". */
            q.push('contractId='         + encodeURIComponent(intOf(val('loaderContract'))));
            q.push('itemId='             + encodeURIComponent(intOf(val('loaderItem'))));
            q.push('supplierCustomerId=' + encodeURIComponent(intOf(val('loaderCustomer'))));
            q.push('thirdPartyId='       + encodeURIComponent(intOf(val('loaderThirdParty'))));
            return getJson(api + '/schedule?' + q.join('&')).then(function (rows) {
                loaderRows = rows || [];
                loaderCols = loaderRows.length
                    ? Object.keys(loaderRows[0]).filter(function (c) {
                          return LOADER_HIDE.indexOf(c) < 0;
                      })
                    : [];
                $id('loaderHead').innerHTML = '<th class="cx-col-x"></th>'
                    + loaderCols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('');
                $id('loaderBody').innerHTML = loaderRows.length
                    ? loaderRows.map(function (r, i) {
                          return '<tr><td class="cx-col-x">'
                               + '<input type="checkbox" data-i="' + i + '"></td>'
                               + loaderCols.map(function (c) {
                                     return '<td>' + esc(r[c]) + '</td>';
                                 }).join('') + '</tr>';
                      }).join('')
                    : '<tr><td colspan="' + (loaderCols.length + 1) + '">No records</td></tr>';
                $id('lblLoaderCount').textContent = loaderRows.length + ' record(s)';
            }).catch(function (e) {
                loaderRows = []; loaderCols = [];
                $id('loaderHead').innerHTML = '';
                $id('loaderBody').innerHTML = '';
                $id('lblLoaderCount').textContent = '';
                box(e.message);
            });
        });
    }

    /**
     * LoadInGrid:2582 — a schedule already in the grid, matched on ScheduleId AND ItemId, is
     * skipped rather than added twice. The desktop reads ContractId, ContractNo, Id, ScheduleCode,
     * ItemId, ItemName and MTon out of the loader's own result set, and leaves Remarks empty.
     */
    function loaderApply() {
        var boxes = $id('loaderBody').querySelectorAll('input[type="checkbox"]:checked');
        for (var b = 0; b < boxes.length; b++) {
            var r = loaderRows[parseInt(boxes[b].getAttribute('data-i'), 10)];
            if (!r) continue;
            var scheduleId = intOf(ci(r, 'Id'));
            var itemId = intOf(ci(r, 'ItemId'));
            var dup = false;
            for (var i = 0; i < schedule.length; i++) {
                if (intOf(schedule[i].ScheduleId) === scheduleId
                 && intOf(schedule[i].ItemId) === itemId) { dup = true; break; }
            }
            if (dup) continue;
            schedule.push({
                Id: 0,
                ContractId:   intOf(ci(r, 'ContractId')),
                SaleContract: ci(r, 'ContractNo'),
                ScheduleId:   scheduleId,
                ScheduleNo:   ci(r, 'ScheduleCode'),
                ItemId:       itemId,
                Item:         ci(r, 'ItemName'),
                MTon:         num(ci(r, 'MTon')),
                Remarks:      ''
            });
        }
        renderSchedule();
        closeLoader();
    }

    /* ------------------------------------------------------------------ save */

    function collect() {
        var rates = [];
        byProduct.forEach(function (r) {
            rates.push({ EffectiveDate: isoOf(r.EffectiveDate), ItemRate: num(r.Rate40Kg),
                         Id: intOf(r.Id), ItemId: intOf(r.ItemId), JobOrderId: 0, TransTypeId: 1 });
        });
        finish.forEach(function (r) {
            rates.push({ EffectiveDate: isoOf(r.EffectiveDate), ItemRate: num(r.Rate40Kg),
                         Id: intOf(r.Id), ItemId: intOf(r.ItemId), JobOrderId: 0, TransTypeId: 2 });
        });
        return {
            Id: recId,
            PlanDate: val('txtPlanDate'),
            PlanCode: intOf(val('txtPlanCode')),
            InvProductionPlantId: 0,
            PlanType: val('cmbPlanType'),
            PlanTypeSrNo: intOf(val('txtPlanTypeSrNo')),
            RefInvoiceNo: flags.generateJobOrderNo ? val('cmbProductionNo') : val('txtProductionNo'),
            LotReference: val('txtOtherReference'),
            ProductionType: val('cmbProductionType'),
            WorkInProccessAcId: intOf(val('cmbWorkInProcess')),
            FinishGoodsAcId: intOf(val('cmbFinishGoodsAc')),
            ByProductacId: intOf(val('cmbByProductionAc')),
            WipItemId: intOf(val('cmbWipItem')),
            WipWareHouseId: intOf(val('cmbWipWarehouse')),
            StartDate: val('txtStartDate'),
            EndDate: val('txtEndDate'),
            PlanStatus: val('cmbStatus'),
            FinishGoodRate: num(val('txtAvgFinishGoodRate')),
            OtherInstructions: val('txtOtherInstruction'),
            InvProductionJobOrderPlantslist: plants.map(function (r) {
                return { Id: intOf(r.Id), InvProductionJobOrderId: 0,
                         PlantId: intOf(r.PlantId), RemarksDetail: r.RemarksDetail };
            }),
            JobOrderRateSchedulelist: rates,
            InvProductionJobOrderAndOrderAllocationList: schedule.map(function (r) {
                return { MTon: num(r.MTon), Id: intOf(r.Id), InvProductionJobOrderId: 0,
                         ItemId: intOf(r.ItemId), OrderId: intOf(r.ContractId),
                         OrderTypeId: 1, ScheduleId: intOf(r.ScheduleId), Remarks: r.Remarks };
            })
        };
    }

    function post(btnId, asUpdate) {
        return busy(btnId, function () {
            if (!ask(asUpdate ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            var body = collect();
            /* :1865 and :1894 — Save and SaveAs both set RECID = 0 first, so both INSERT. */
            if (!asUpdate) body.Id = 0;
            return request(api + '/save', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(body)
            }).then(function (d) {
                box((d && d.message) || 'Saved.');
                reset();
            }).catch(function (e) { box(e.message); });
        });
    }

    function save()   { post('btnSave',   false); }
    function saveAs() { post('btnSaveAs', false); }
    function update() {
        /* btnUpdate_Click:1876 — the desktop's own refusal. */
        if (recId === 0) { box('Record Not Update Because Record Not Found'); return; }
        post('btnUpdate', true);
    }

    /* ------------------------------------------------------------------ open a record */

    function open_(id) {
        return getJson(api + '/' + id).then(function (d) {
            if (!d) { box('Record Not Found'); return; }
            var h = d.header || {};
            recId = intOf(h.Id);
            $id('lblRecId').textContent = 'Record #' + recId;

            setVal('txtPlanDate',   isoOf(h.PlanDate));
            setVal('txtPlanCode',   h.PlanCode);
            setVal('cmbWorkInProcess',  h.WorkInProccessAcId);
            setVal('cmbFinishGoodsAc',  h.FinishGoodsAcId);
            setVal('cmbByProductionAc', h.ByProductacId);
            setVal('cmbWipItem',        h.WipItemId);
            setVal('cmbWipWarehouse',   h.WipWareHouseId);
            setVal('cmbPlanType',       h.PlanType);
            setVal('txtPlanTypeSrNo',   h.PlanTypeSrNo);
            if (flags.generateJobOrderNo) setVal('cmbProductionNo', h.RefInvoiceNo);
            else setVal('txtProductionNo', h.RefInvoiceNo);
            setVal('txtOtherReference', h.LotReference);
            setVal('cmbProductionType', h.ProductionType);
            setVal('txtStartDate',      isoOf(h.StartDate));
            setVal('txtEndDate',        isoOf(h.EndDate));
            setVal('cmbStatus',         h.PlanStatus);
            setVal('txtAvgFinishGoodRate', h.FinishGoodRate);
            setVal('txtOtherInstruction',  h.OtherInstructions);

            plants    = d.plants      || [];
            byProduct = d.byProduct   || [];
            finish    = d.finishGoods || [];
            schedule  = d.schedule    || [];
            cancelPlant();
            cancelRate(1);
            cancelRate(2);
            renderSchedule();
            tab('tabForm');
            tab2('tabDetail');
        }).catch(function (e) { box(e.message); });
    }

    /**
     * Reset():1499 — note what it does NOT clear: Start Date, End Date, the Average Finish Rate
     * and the Comments box all keep their values on the desktop, and so do they here.
     */
    function reset() {
        recId = 0;
        $id('lblRecId').textContent = '';
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
        setVal('txtPlanTypeSrNo', '');
        plants = []; byProduct = []; finish = []; schedule = [];
        cancelPlant();
        cancelRate(1);
        cancelRate(2);
        renderSchedule();
        /* GeneratePlanCode() and GetLastSaveAccount() both run again on Reset (:1523, :1525). */
        return loadLookups().catch(function (e) { box(e.message); });
    }

    function refresh() { return reset(); }

    /* ------------------------------------------------------------------ history */

    function dateMode() {
        var r = document.querySelector('input[name="dateMode"]:checked');
        return r ? r.value : 'doc';
    }

    /** cmbperemeter_ValueChanged:2228 — the five presets, with the desktop's own arithmetic. */
    function datePreset() {
        var v = intOf(val('cmbPeremeter'));
        var d = new Date();
        function iso(x) {
            return x.getFullYear() + '-' + String(x.getMonth() + 1).padStart(2, '0')
                 + '-' + String(x.getDate()).padStart(2, '0');
        }
        if (v === 1) { setVal('datHistoryFrom', iso(d)); }
        else if (v === 2) { var w = new Date(); w.setDate(w.getDate() - 7); setVal('datHistoryFrom', iso(w)); }
        else if (v === 3) { setVal('datHistoryFrom', iso(new Date(d.getFullYear(), d.getMonth(), 1)));
                            setVal('datHistoryTo', iso(d)); }
        else if (v === 4) { setVal('datHistoryFrom', iso(new Date(d.getFullYear(), 0, 1)));
                            setVal('datHistoryTo', iso(d)); }
        else if (v === 5) { if (financialYearStart) setVal('datHistoryFrom', financialYearStart);
                            setVal('datHistoryTo', iso(d)); }
    }

    var HISTORY_COLS = ['Date', 'Code', 'Type', 'Production#', 'PType', 'StartDate', 'EndDate',
                        'PlanStatus', 'OtherInst', 'EntryUser', 'EntryDate', 'ModifyUser',
                        'ModifyDate', 'ApprovedStatus', 'ApprovedUser', 'ApprovedDate',
                        'Attachments'];

    function history() {
        return busy('btnShowHistory', function () {
            var q = ['dateMode=' + encodeURIComponent(dateMode())];
            if (val('datHistoryFrom')) q.push('fromDate=' + encodeURIComponent(val('datHistoryFrom')));
            if (val('datHistoryTo'))   q.push('toDate='   + encodeURIComponent(val('datHistoryTo')));
            q.push('docNoFrom=' + encodeURIComponent(intOf(val('txtOrderNoFrom'))));
            q.push('docNoTo='   + encodeURIComponent(intOf(val('txtOrderNoTo'))));
            q.push('jobOrderId=' + encodeURIComponent(intOf(val('cmbHistoryJobOrder'))));

            return getJson(api + '/history?' + q.join('&')).then(function (d) {
                var rows = (d && d.rows) || [];
                $id('lblHistoryScope').textContent = (d && d.canViewAllRecords)
                    ? 'All users' : 'Your records only';
                /* Edit / SaveAs / Print are the desktop's three button columns (HgridSetting). */
                $id('historyHead').innerHTML = '<th>Edit</th><th>SaveAs</th>'
                    + HISTORY_COLS.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('');
                $id('historyBody').innerHTML = rows.length
                    ? rows.map(function (r) {
                          return '<tr>'
                               + '<td><button type="button" class="win-btn-small"'
                               + ' onclick="JobOrderMain.open(' + intOf(r.Id) + ')">Edit</button></td>'
                               + '<td><button type="button" class="win-btn-small"'
                               + ' onclick="JobOrderMain.openAsCopy(' + intOf(r.Id) + ')">SaveAs</button></td>'
                               + HISTORY_COLS.map(function (c) {
                                     return '<td' + (c === 'Code' ? ' class="num"' : '') + '>'
                                          + esc(r[c]) + '</td>';
                                 }).join('') + '</tr>';
                      }).join('')
                    : '<tr><td colspan="' + (HISTORY_COLS.length + 2) + '">No records</td></tr>';
                $id('lblHistoryCount').textContent = rows.length + ' record(s)';
            }).catch(function (e) {
                $id('historyHead').innerHTML = '';
                $id('historyBody').innerHTML = '';
                $id('lblHistoryCount').textContent = '';
                box(e.message);
            });
        });
    }

    /**
     * DataGridHistory_ColumnButtonClick:2199 — "SaveAs" opens the record but leaves only the
     * SaveAs button live, so the next save writes a NEW job order rather than updating this one.
     * Reproduced by clearing the record id after the read.
     */
    function openAsCopy(id) {
        return open_(id).then(function () {
            recId = 0;
            $id('lblRecId').textContent = 'Copy of #' + id + ' — saving will create a new job order';
            plants.forEach(function (r) { r.Id = 0; });
            byProduct.forEach(function (r) { r.Id = 0; });
            finish.forEach(function (r) { r.Id = 0; });
            schedule.forEach(function (r) { r.Id = 0; });
        });
    }

    /* ------------------------------------------------------------------ boot */

    function boot() {
        setVal('txtPlanDate', today());
        setVal('txtStartDate', today());
        setVal('txtEndDate', today());
        setVal('datEffectiveDateByProduct', today());
        setVal('datEffectiveDateFinish', today());

        /* frmProductionJobOrder_KeyDown:2278 — Enter moves on rather than submitting. */
        document.addEventListener('keydown', function (e) {
            if (e.key === 'Enter' && e.target && e.target.tagName !== 'BUTTON'
                && e.target.tagName !== 'TEXTAREA') {
                e.preventDefault();
            }
        });
        /* txtAvgFinishGoodRate_KeyPress:2435 and the two rate boxes — digits and one dot only. */
        ['txtAvgFinishGoodRate', 'txtRateByProduct', 'txtRateFinish',
         'txtOrderNoFrom', 'txtOrderNoTo'].forEach(function (id) {
            var el = $id(id);
            if (!el) return;
            el.addEventListener('keypress', function (e) {
                if (e.key.length !== 1) return;
                var decimals = (id === 'txtOrderNoFrom' || id === 'txtOrderNoTo') ? false : true;
                if (/[0-9]/.test(e.key)) return;
                if (decimals && e.key === '.' && el.value.indexOf('.') < 0) return;
                e.preventDefault();
            });
        });

        /* cmbPlanType_Leave:768 — leaving Plan Type asks for that type's next serial. */
        $id('cmbPlanType').addEventListener('change', function () {
            var t = val('cmbPlanType');
            if (!t) { setVal('txtPlanTypeSrNo', ''); return; }
            getJson(api + '/next-plan-type-code?planType=' + encodeURIComponent(t))
                .then(function (d) { setVal('txtPlanTypeSrNo', (d && d.planTypeSrNo) || ''); })
                .catch(function (e) { box(e.message); });
        });

        renderPlants();
        renderRates(1);
        renderRates(2);
        renderSchedule();
        loadLookups().catch(function (e) { box(e.message); });
    }

    window.JobOrderMain = {
        tab: tab, tab2: tab2,
        addPlant: addPlant, editPlant: editPlant, updatePlant: updatePlant,
        cancelPlant: cancelPlant, deletePlant: deletePlant,
        addRate: addRate, editRate: editRate, cancelRate: cancelRate, deleteRate: deleteRate,
        scheduleRemarks: scheduleRemarks, deleteSchedule: deleteSchedule,
        openLoader: openLoader, closeLoader: closeLoader, loaderShow: loaderShow,
        loaderApply: loaderApply,
        save: save, saveAs: saveAs, update: update,
        open: open_, openAsCopy: openAsCopy,
        reset: reset, refresh: refresh,
        history: history, datePreset: datePreset
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
    else boot();
}());
