/* ============================================================================================
 * Production Job Order - frmProductionJobOrder.cs, DocumentTypeId 401.
 *
 * Every handler below is a port of a named desktop handler. The desktop name is in the comment
 * above it, so the two can be read side by side. Nothing here invents a behaviour the form does
 * not have, and nothing here quietly improves one it does.
 *
 * ---------------------------------------------------------------------------------------------
 * THE BUTTON CONTRACT
 * ---------------------------------------------------------------------------------------------
 * Every button that issues a request goes through busy(): disabled at once, spinner shown,
 * further clicks ignored while in flight, and re-enabled on success AND on failure. There is no
 * path out of busy() that leaves a button stuck.
 *
 * ---------------------------------------------------------------------------------------------
 * TWO DESKTOP DEFECTS ARE REPRODUCED, NOT FIXED - both are marked DESKTOP-DEFECT below
 * ---------------------------------------------------------------------------------------------
 *   1. btnplus_Click (:2400) writes txtOuterqty into BOTH the QtyInner and QtyOuter columns, so
 *      a newly ADDED output row loses the Inner Qty that was typed. btnUpdateDetail_Click (:2505)
 *      writes txtinnerqty into QtyInner correctly, so editing the row afterwards repairs it.
 *   2. Neither detail grid has a row-delete anywhere on the form: rows can only be added and
 *      double-clicked to edit. No delete is added here.
 *
 * Both are left exactly as the desktop behaves because the two apps must save the same data.
 * They are reported separately for a decision rather than changed here.
 * ============================================================================================ */
(function () {
    'use strict';

    /* ---------------------------------------------------------------------------- state */

    var LK = {};                       // the lookups payload
    var RECID = 0;                     // the loaded job order, 0 for a new one
    var outputRows = [];               // dtPlannedOutput
    var inputRows = [];                // dtInputDetail
    var updateDetailIndexoutput = -1;  // the desktop's own two edit cursors
    var updateDetailIndexInput = -1;

    var api = '/api/production/job-order';

    /* ---------------------------------------------------------------------------- utility */

    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function text(id) {
        var e = $id(id);
        if (!e) return '';
        if (e.tagName === 'SELECT') {
            var o = e.options[e.selectedIndex];
            return o ? o.text : '';
        }
        return e.value;
    }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function num(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function int(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isNaN(n) ? 0 : n; }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function say(m) { var e = $id('lblFormStatus'); if (e) e.textContent = m; }

    /* The desktop's MessageBox.Show. One place, so every message reaches the operator the same
       way and none of them is swallowed. */
    function box(message) { window.alert(message); }

    /* The button contract, in one function. */
    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) {
            if (b.disabled || b.classList.contains('is-busy')) return;   // duplicate click
            b.disabled = true;
            b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } };
        var p;
        try { p = fn(); } catch (e) { done(); throw e; }
        if (p && typeof p.then === 'function') p.then(done, done); else done();
        return p;
    }

    function getJson(url) {
        return fetch(url, { headers: { 'Accept': 'application/json' }, credentials: 'same-origin' })
            .then(function (r) {
                return r.text().then(function (t) {
                    var body = null;
                    try { body = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
                    if (!r.ok) throw new Error((body && body.message) || ('Request failed (' + r.status + ')'));
                    return body;
                });
            });
    }

    function postJson(url, payload) {
        return fetch(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' },
            credentials: 'same-origin',
            body: JSON.stringify(payload)
        }).then(function (r) {
            return r.text().then(function (t) {
                var body = null;
                try { body = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
                if (!r.ok) throw new Error((body && body.message) || ('Request failed (' + r.status + ')'));
                return body;
            });
        });
    }

    /* ------------------------------------------------------------------ combo population */

    /**
     * DDL.BindDDL / DDL.BindDDLNew, ported.
     *
     * codeField is what makes a combo two-column: BindDDL keeps the source table's other columns
     * and BindDDLNew builds a fresh two-column table and keeps only one visible. So a combo the
     * desktop binds with BindDDLNew is filled here with no codeField, and looks single-column
     * because that is what it is - not because anything was left out.
     */
    function fill(id, rows, valueField, textField, codeField) {
        var sel = $id(id);
        if (!sel) return;
        var keep = sel.value;
        var html = '<option value="0"></option>';
        (rows || []).forEach(function (r) {
            var v = r[valueField];
            if (v === null || v === undefined) return;
            var label = r[textField];
            var code = codeField ? r[codeField] : null;
            html += '<option value="' + esc(v) + '"'
                  + (code === null || code === undefined ? '' : ' data-code="' + esc(code) + '"')
                  + '>' + esc(label) + '</option>';
        });
        sel.innerHTML = html;
        /* Keep the selection when the list is rebuilt under it, ditto
           InfragisticsHelper.BindAndRetainSelection. */
        if (keep) {
            var opts = sel.options;
            for (var i = 0; i < opts.length; i++) {
                if (opts[i].value === String(keep)) { sel.selectedIndex = i; break; }
            }
        }
        sel.dispatchEvent(new Event('change', { bubbles: false }));
    }

    /** The first column present out of several candidates, so a two-column combo shows a code
     *  when the procedure returns one and stays single-column when it does not. */
    function firstColumn(rows, names) {
        if (!rows || !rows.length) return null;
        for (var i = 0; i < names.length; i++) {
            if (Object.prototype.hasOwnProperty.call(rows[0], names[i])) return names[i];
        }
        return null;
    }

    /* =========================================================================== bootstrap */

    function loadLookups() {
        return getJson(api + '/lookups').then(function (d) {
            LK = d || {};

            /* Single-column (BindDDLNew on the desktop). */
            fill('cmbPlantFeader', LK.plants, 'Id', 'Description');
            fill('cmbCropYearOutput', LK.cropYears, 'Id', 'CropYear');
            fill('cmbcropyearinput', LK.cropYears, 'Id', 'CropYear');
            fill('cmbJobLotOutput', LK.jobLots, 'Id', 'JobLotDescription');
            fill('cmbjoblotinput', LK.jobLots, 'Id', 'JobLotDescription');
            fill('cmbPackingType', LK.packingTypes, 'Id', 'PackTypeDesc');
            fill('cmbPackTypeInput', LK.packingTypes, 'Id', 'PackTypeDesc');
            fill('cmbentrytype', LK.entryTypes, 'Id', 'name');
            fill('cmbFiltersType', LK.filterTypes, 'Id', 'name');
            fill('cmbItem', LK.items, 'Id', 'ItemName');
            fill('cmbItemInput', LK.items, 'Id', 'ItemName');

            /* Multi-column (BindDDL on the desktop) - a code column only when one is there. */
            var whCode = firstColumn(LK.warehouses, ['WareHouseCode', 'WarehouseCode', 'Code']);
            fill('cmbWareHouseOutPut', LK.warehouses, 'Id', 'WareHouseName', whCode);
            fill('cmbwarehouseinput', LK.warehouses, 'Id', 'WareHouseName', whCode);

            var acCode = firstColumn(LK.coaAccounts, ['AccountCode', 'Code']);
            fill('CmbWorkInProcess', LK.coaAccounts, 'Id', 'AccountTitle', acCode);
            fill('CmbFinishGoodsAc', LK.coaAccounts, 'Id', 'AccountTitle', acCode);
            fill('CmbByProductionAc', LK.coaAccounts, 'Id', 'AccountTitle', acCode);

            var itCode = firstColumn(LK.wipItems, ['ItemCode', 'Code']);
            fill('CmbWipItem', LK.wipItems, 'Id', 'ItemName', itCode);

            /* PlanTypeBind / ProductionTypeBind / StatusBind - literal on the desktop too. */
            fill('cmbPlanType', [{ Id: 1, Type: 'Local' }, { Id: 2, Type: 'Export' }, { Id: 3, Type: 'Party' }], 'Id', 'Type');
            fill('cmbProductionType', [{ Id: 1, T: 'Husking' }, { Id: 2, T: 'Processing' }, { Id: 3, T: 'Other' }], 'Id', 'T');
            fill('cmbStatus', [{ Id: 1, S: 'In Process' }, { Id: 2, S: 'Complete' }, { Id: 3, S: 'Cancel' }], 'Id', 'S');

            if (window.DesktopCombo) window.DesktopCombo.refresh();
        });
    }

    /* GeneratePlanCode (:1127) */
    function GeneratePlanCode() {
        return getJson(api + '/next-code').then(function (d) {
            var code = d && d.planCode ? d.planCode : 0;
            if (code > 0) {
                setVal('txtPlanCode', code);
                $id('lblPlanCode').textContent = code;
            }
        });
    }

    /* cmbPlanType change -> GeneratePlanTypeCode */
    function onPlanTypeChange() {
        var t = text('cmbPlanType');
        if (!t) { setVal('txtPlanTypeSrNo', ''); return; }
        getJson(api + '/next-plan-type-code?planType=' + encodeURIComponent(t))
            .then(function (d) { setVal('txtPlanTypeSrNo', (d && d.planTypeSrNo) || ''); })
            .catch(function (e) { box(e.message); });
    }

    /* ====================================================== entry type / filter / document */

    /* cmbentrytype_ValueChanged (:552) - Input selects tab 1, Output selects tab 0, and the
       document filter is re-applied. */
    function onEntryTypeChange() {
        var t = text('cmbentrytype');
        if (t === 'Input') selectDetailTab(1, true);
        if (t === 'Output') selectDetailTab(0, true);
        onDocNoChange();
    }

    /* tabControl2_SelectedIndexChanged (:2712) - the tab writes back to the Entry Type combo.
       fromCombo stops the two from bouncing off each other. */
    function selectDetailTab(index, fromCombo) {
        $id('paneOutput').style.display = index === 0 ? '' : 'none';
        $id('paneInput').style.display = index === 1 ? '' : 'none';
        $id('tabOutput').className = index === 0 ? 'active' : '';
        $id('tabInput').className = index === 1 ? 'active' : '';
        if (!fromCombo) {
            var sel = $id('cmbentrytype');
            var want = index === 0 ? 'Output' : 'Input';
            for (var i = 0; i < sel.options.length; i++) {
                if (sel.options[i].text === want) { sel.selectedIndex = i; break; }
            }
            if (window.jQuery) window.jQuery(sel).trigger('change');
            else sel.dispatchEvent(new Event('change'));
            onDocNoChange();
        }
    }

    /* cmbFiltersType_Leave (:591) - clear the document combo, then rebind it from the filter. */
    function onFilterTypeChange() {
        var f = text('cmbFiltersType');
        var doc = $id('cmbDocNo');
        doc.innerHTML = '<option value="0"></option>';
        $id('lblDocNo').textContent = 'Document';
        if (!f) { if (window.DesktopCombo) window.DesktopCombo.refresh(); onDocNoChange(); return; }

        getJson(api + '/doc-no?filterType=' + encodeURIComponent(f)).then(function (d) {
            fill('cmbDocNo', d.rows, d.valueField, d.displayField);
            $id('lblDocNo').textContent = d.caption || 'Document';
            var c = $id('cmbDocNo');
            if (c) c.setAttribute('data-dtcombo-caption', d.caption || 'Document');
            if (window.DesktopCombo) window.DesktopCombo.refresh();
            onDocNoChange();
        }).catch(function (e) { box(e.message); });
    }

    /* cmbDocNo_Leave (:620) - which items the active grid's item picker may offer. With no filter
       or no document the desktop calls ItemDetailFill(), which refills BOTH pickers. */
    function onDocNoChange() {
        var entry = text('cmbentrytype') || 'Input';
        var f = text('cmbFiltersType');
        var id = int(val('cmbDocNo'));
        var url = api + '/filtered-items?entryType=' + encodeURIComponent(entry)
                + '&filterType=' + encodeURIComponent(f || '')
                + '&docNoId=' + id;

        getJson(url).then(function (d) {
            var unfiltered = !f || id <= 0;
            if (unfiltered) {
                fill('cmbItem', d.rows, d.valueField, 'ItemName');
                fill('cmbItemInput', d.rows, d.valueField, 'ItemName');
            } else if (entry === 'Output') {
                fill('cmbItem', d.rows, d.valueField, 'ItemName');
            } else {
                fill('cmbItemInput', d.rows, d.valueField, 'ItemName');
            }
            if (window.DesktopCombo) window.DesktopCombo.refresh();
        }).catch(function (e) { box(e.message); });
    }

    /* ============================================================ per-item UOM and weights */

    /* bindRateUomAndItemPackUom (:1009) - Inner and Outer UOM, both showing Equivalent. */
    function onOutputItemChange() {
        var itemId = int(val('cmbItem'));
        if (itemId <= 0) return;
        getJson(api + '/uom-schedule?itemId=' + itemId).then(function (rows) {
            fill('cmbinneruom', rows, 'Id', 'Equivalent');
            fill('cmbouteruom', rows, 'Id', 'Equivalent');
            if (window.DesktopCombo) window.DesktopCombo.refresh();
        }).catch(function (e) { box(e.message); });
    }

    /* bindPackSizeInput (:1032) - the input grid's Pack Size, also Equivalent. */
    function onInputItemChange() {
        var itemId = int(val('cmbItemInput'));
        if (itemId <= 0) return;
        getJson(api + '/uom-schedule?itemId=' + itemId).then(function (rows) {
            fill('cmbpacksizeinput', rows, 'Id', 'Equivalent');
            if (window.DesktopCombo) window.DesktopCombo.refresh();
        }).catch(function (e) { box(e.message); });
    }

    /* Getweight (:2300) - weight = outer UOM TEXT x outer qty, and only when both are above zero.
       The desktop multiplies; it does not divide, and it writes nothing at all when either is 0,
       so no zero is ever written over a weight the operator typed. */
    function Getweight() {
        var outeruom = num(text('cmbouteruom'));
        var outerqty = num(val('txtOuterqty'));
        if (outeruom > 0 && outerqty > 0) setVal('txtweight', outeruom * outerqty);
    }

    /* GetweightInput (:2318) - the same rule on the input side. */
    function GetweightInput() {
        var uom = num(text('cmbpacksizeinput'));
        var qty = num(val('txtqtyinput'));
        if (uom > 0 && qty > 0) setVal('txtweightinput', uom * qty);
    }

    /* ========================================================== planned output grid (rows) */

    /* FormValidationDetailOutPut (:1480) - message for message. */
    function FormValidationDetailOutPut() {
        if (int(val('cmbItem')) <= 0)             { box('Item Name Field is Required'); return false; }
        if (int(val('cmbPackingType')) <= 0)      { box('Item Pack Unit Field is Required'); return false; }
        if (int(val('cmbinneruom')) <= 0)         { box('Inner UOM Field is Required'); return false; }
        var oq = val('txtOuterqty').trim();
        if (oq === '' || oq === '0')              { box('Outer Qty Field is Required'); return false; }
        if (int(val('cmbouteruom')) <= 0)         { box('Outer UOM Field is Required'); return false; }
        var w = val('txtweight').trim();
        if (w === '' || w === '0')                { box('Weight is Required'); return false; }
        if (int(val('cmbCropYearOutput')) <= 0)   { box('CropYear is Required'); return false; }
        if (int(val('cmbWareHouseOutPut')) <= 0)  { box('WareHouse is Required'); return false; }
        if (int(val('cmbJobLotOutput')) <= 0)     { box('JobLot is Required'); return false; }
        return true;
    }

    /* btnplus_Click (:2394) */
    function btnplus_Click() {
        if (!FormValidationDetailOutPut()) return;
        outputRows.push({
            ItemId: val('cmbItem'), ItemName: text('cmbItem').trim(),
            PackingTypeId: val('cmbPackingType'), PackingTypeName: text('cmbPackingType').trim(),
            /* DESKTOP-DEFECT 1 - the add path writes the OUTER qty into QtyInner. Reproduced so
               the two apps store the same number; see the header note. */
            QtyInner: val('txtOuterqty').trim(),
            InnerPackSizeId: val('cmbinneruom'), InnerPackSizeName: text('cmbinneruom').trim(),
            QtyOuter: val('txtOuterqty').trim(),
            OuterPackSizeId: val('cmbouteruom'), OuterPackSizeName: text('cmbouteruom').trim(),
            Weight: val('txtweight').trim(), PackRemarks: val('txtremarks').trim(),
            CropYearId: val('cmbCropYearOutput'), CropYear: text('cmbCropYearOutput').trim(),
            WareHouseId: val('cmbWareHouseOutPut'), WareHouseName: text('cmbWareHouseOutPut').trim(),
            JobLotId: val('cmbJobLotOutput'), JobLotDescription: text('cmbJobLotOutput').trim()
        });
        renderOutput();
        clearOutputEntry();
        $id('cmbItem').focus();
    }

    /* grdPlannedOutput_DoubleClick (:2425) - guarded by the production reference check first. */
    function editOutputRow(i) {
        var apply = function () {
            var r = outputRows[i];
            updateDetailIndexoutput = i;
            setVal('cmbItem', r.ItemId);
            onOutputItemChange();
            setVal('cmbPackingType', r.PackingTypeId);
            setVal('txtinnerqty', r.QtyInner);
            setVal('cmbinneruom', r.InnerPackSizeId);
            setVal('txtOuterqty', r.QtyOuter);
            setVal('cmbouteruom', r.OuterPackSizeId);
            setVal('txtweight', r.Weight);
            setVal('txtremarks', r.PackRemarks);
            setVal('cmbCropYearOutput', r.CropYearId);
            setVal('cmbWareHouseOutPut', r.WareHouseId);
            setVal('cmbJobLotOutput', r.JobLotId);
            $id('btnplus').style.display = 'none';
            $id('btnUpdateDetail').style.display = '';
            $id('btnCancelUpdateDetial').style.display = '';
            selectDetailTab(0);
            if (window.DesktopCombo) window.DesktopCombo.refresh();
        };
        if (RECID <= 0) { apply(); return; }
        getJson(api + '/row-editable?grid=output&jobOrderId=' + RECID).then(function (d) {
            if (!d.editable) { box(d.message); return; }
            apply();
        }).catch(function (e) { box(e.message); });
    }

    /* btnUpdateDetail_Click (:2491) - note QtyInner comes from txtinnerqty here, unlike the add. */
    function btnUpdateDetail_Click() {
        if (!FormValidationDetailOutPut()) return;
        if (updateDetailIndexoutput < 0) return;
        outputRows[updateDetailIndexoutput] = {
            ItemId: val('cmbItem'), ItemName: text('cmbItem'),
            PackingTypeId: val('cmbPackingType'), PackingTypeName: text('cmbPackingType'),
            QtyInner: val('txtinnerqty'),
            InnerPackSizeId: val('cmbinneruom'), InnerPackSizeName: text('cmbinneruom'),
            QtyOuter: val('txtOuterqty'),
            OuterPackSizeId: val('cmbouteruom'), OuterPackSizeName: text('cmbouteruom'),
            Weight: val('txtweight'), PackRemarks: val('txtremarks'),
            CropYearId: val('cmbCropYearOutput'), CropYear: text('cmbCropYearOutput'),
            WareHouseId: val('cmbWareHouseOutPut'), WareHouseName: text('cmbWareHouseOutPut'),
            JobLotId: val('cmbJobLotOutput'), JobLotDescription: text('cmbJobLotOutput')
        };
        renderOutput();
        btnCancelUpdateDetial_Click();
    }

    /* btnCancelUpdateDetial_Click (:2465) */
    function btnCancelUpdateDetial_Click() {
        $id('btnplus').style.display = '';
        $id('btnUpdateDetail').style.display = 'none';
        $id('btnCancelUpdateDetial').style.display = 'none';
        updateDetailIndexoutput = -1;
        clearOutputEntry();
        ['cmbWareHouseOutPut', 'cmbCropYearOutput', 'cmbJobLotOutput'].forEach(function (id) { setVal(id, '0'); });
        if (window.DesktopCombo) window.DesktopCombo.refresh();
        $id('cmbItem').focus();
    }

    function clearOutputEntry() {
        setVal('cmbItem', '0'); setVal('cmbPackingType', '0');
        setVal('txtinnerqty', ''); setVal('cmbinneruom', '0');
        setVal('txtOuterqty', ''); setVal('cmbouteruom', '0');
        setVal('txtweight', ''); setVal('txtremarks', '');
        if (window.DesktopCombo) window.DesktopCombo.refresh();
    }

    function renderOutput() {
        var html = '';
        var tQtyInner = 0, tQtyOuter = 0, tWeight = 0;
        outputRows.forEach(function (r, i) {
            tQtyInner += num(r.QtyInner); tQtyOuter += num(r.QtyOuter); tWeight += num(r.Weight);
            html += '<tr ondblclick="ProductionJobOrder.editOutputRow(' + i + ')">'
                 + '<td>' + esc(r.ItemName) + '</td>'
                 + '<td>' + esc(r.PackingTypeName) + '</td>'
                 + '<td style="text-align:right;">' + esc(r.QtyInner) + '</td>'
                 + '<td>' + esc(r.InnerPackSizeName) + '</td>'
                 + '<td style="text-align:right;">' + esc(r.QtyOuter) + '</td>'
                 + '<td>' + esc(r.OuterPackSizeName) + '</td>'
                 + '<td style="text-align:right;">' + esc(r.Weight) + '</td>'
                 + '<td>' + esc(r.PackRemarks) + '</td>'
                 + '<td>' + esc(r.CropYear) + '</td>'
                 + '<td>' + esc(r.WareHouseName) + '</td>'
                 + '<td>' + esc(r.JobLotDescription) + '</td>'
                 + '</tr>';
        });
        $id('gridPlannedOutput').innerHTML = html;
        /* AggregateFunction = Sum on QtyInner, QtyOuter and Weight (PlannedOutputGridSetting). */
        $id('lblOutputTotals').textContent =
            'Inner ' + fmt(tQtyInner) + '   Outer ' + fmt(tQtyOuter) + '   Weight ' + fmt(tWeight) + '   ';
    }

    /* ================================================================ input detail grid */

    /* FormValidationDetailInPut (:1525) - "Cropt Year" is the desktop's own spelling. */
    function FormValidationDetailInPut() {
        if (int(val('cmbItemInput')) <= 0)      { box('Item Name Field is Required'); return false; }
        if (int(val('cmbcropyearinput')) <= 0)  { box('Cropt Year Field is Required'); return false; }
        if (int(val('cmbjoblotinput')) <= 0)    { box('Job Lot Field is Required'); return false; }
        var q = val('txtqtyinput').trim();
        if (q === '' || q === '0')              { box('Qty Field is Required'); return false; }
        if (int(val('cmbpacksizeinput')) <= 0)  { box('Pack Size Field is Required'); return false; }
        var w = val('txtweightinput').trim();
        if (w === '' || w === '0')              { box('Weight Field is Required'); return false; }
        if (int(val('cmbwarehouseinput')) <= 0) { box('Warehouse is Required'); return false; }
        if (int(val('cmbPackTypeInput')) <= 0)  { box('PackType Input is Required'); return false; }
        return true;
    }

    /* btnadddetailinputgrd_Click (:2540) */
    function btnadddetailinputgrd_Click() {
        if (!FormValidationDetailInPut()) return;
        inputRows.push({
            ItemId: val('cmbItemInput'), ItemName: text('cmbItemInput').trim(),
            CropYearId: val('cmbcropyearinput'), CropYear: text('cmbcropyearinput').trim(),
            JobLotId: val('cmbjoblotinput'), JobLot: text('cmbjoblotinput').trim(),
            Qty: val('txtqtyinput').trim(),
            PackSizeId: val('cmbpacksizeinput'), PackSize: text('cmbpacksizeinput').trim(),
            Weight: val('txtweightinput').trim(),
            WareHouseId: val('cmbwarehouseinput'), WareHouse: text('cmbwarehouseinput').trim(),
            Remarks: val('txtremarksinput').trim(),
            PackTypeId: val('cmbPackTypeInput'), PackType: text('cmbPackTypeInput').trim()
        });
        renderInput();
        clearInputEntry();
        $id('cmbItemInput').focus();
    }

    /* grdInputDetail_DoubleClick (:2571) */
    function editInputRow(i) {
        var apply = function () {
            var r = inputRows[i];
            updateDetailIndexInput = i;
            setVal('cmbItemInput', r.ItemId);
            onInputItemChange();
            setVal('cmbcropyearinput', r.CropYearId);
            setVal('cmbjoblotinput', r.JobLotId);
            setVal('txtqtyinput', r.Qty);
            setVal('cmbpacksizeinput', r.PackSizeId);
            setVal('txtweightinput', r.Weight);
            setVal('cmbwarehouseinput', r.WareHouseId);
            setVal('txtremarksinput', r.Remarks);
            setVal('cmbPackTypeInput', r.PackTypeId);
            $id('btnadddetailinputgrd').style.display = 'none';
            $id('btnupdategrdinput').style.display = '';
            $id('btncancelgrdinput').style.display = '';
            selectDetailTab(1);
            if (window.DesktopCombo) window.DesktopCombo.refresh();
        };
        if (RECID <= 0) { apply(); return; }
        getJson(api + '/row-editable?grid=input&jobOrderId=' + RECID).then(function (d) {
            if (!d.editable) { box(d.message); return; }
            apply();
        }).catch(function (e) { box(e.message); });
    }

    function btnupdategrdinput_Click() {
        if (!FormValidationDetailInPut()) return;
        if (updateDetailIndexInput < 0) return;
        inputRows[updateDetailIndexInput] = {
            ItemId: val('cmbItemInput'), ItemName: text('cmbItemInput'),
            CropYearId: val('cmbcropyearinput'), CropYear: text('cmbcropyearinput'),
            JobLotId: val('cmbjoblotinput'), JobLot: text('cmbjoblotinput'),
            Qty: val('txtqtyinput'),
            PackSizeId: val('cmbpacksizeinput'), PackSize: text('cmbpacksizeinput'),
            Weight: val('txtweightinput'),
            WareHouseId: val('cmbwarehouseinput'), WareHouse: text('cmbwarehouseinput'),
            Remarks: val('txtremarksinput'),
            PackTypeId: val('cmbPackTypeInput'), PackType: text('cmbPackTypeInput')
        };
        renderInput();
        btncancelgrdinput_Click();
    }

    function btncancelgrdinput_Click() {
        $id('btnadddetailinputgrd').style.display = '';
        $id('btnupdategrdinput').style.display = 'none';
        $id('btncancelgrdinput').style.display = 'none';
        updateDetailIndexInput = -1;
        clearInputEntry();
        $id('cmbItemInput').focus();
    }

    function clearInputEntry() {
        setVal('cmbItemInput', '0'); setVal('cmbcropyearinput', '0'); setVal('cmbjoblotinput', '0');
        setVal('cmbpacksizeinput', '0'); setVal('txtqtyinput', ''); setVal('txtweightinput', '');
        setVal('cmbwarehouseinput', '0'); setVal('txtremarksinput', '');
        if (window.DesktopCombo) window.DesktopCombo.refresh();
    }

    function renderInput() {
        var html = '';
        var tQty = 0, tWeight = 0;
        inputRows.forEach(function (r, i) {
            tQty += num(r.Qty); tWeight += num(r.Weight);
            html += '<tr ondblclick="ProductionJobOrder.editInputRow(' + i + ')">'
                 + '<td>' + esc(r.ItemName) + '</td>'
                 + '<td>' + esc(r.CropYear) + '</td>'
                 + '<td>' + esc(r.JobLot) + '</td>'
                 + '<td style="text-align:right;">' + esc(r.Qty) + '</td>'
                 + '<td>' + esc(r.PackSize) + '</td>'
                 + '<td style="text-align:right;">' + esc(r.Weight) + '</td>'
                 + '<td>' + esc(r.WareHouse) + '</td>'
                 + '<td>' + esc(r.Remarks) + '</td>'
                 + '<td>' + esc(r.PackType) + '</td>'
                 + '</tr>';
        });
        $id('gridInputDetail').innerHTML = html;
        $id('lblInputTotals').textContent = 'Qty ' + fmt(tQty) + '   Weight ' + fmt(tWeight) + '   ';
    }

    /* FormatString "#,##0.##" on the two totals. */
    function fmt(n) {
        return (Math.round(n * 100) / 100).toLocaleString('en-US', { maximumFractionDigits: 2 });
    }

    /* ======================================================================== save / load */

    /* Insert (:1596) - Confirm first, exactly as the desktop asks. */
    function btnSave_Click() {
        var btn = RECID > 0 ? $id('btnUpdate') : $id('btnSave');
        if (!window.confirm(RECID > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;

        busy(btn, function () {
            var dto = {
                Id: RECID,
                PlanDate: val('txtPlanDate'),
                PlanCode: int(val('txtPlanCode')),
                InvProductionPlantId: int(val('cmbPlantFeader')),
                PlanType: text('cmbPlanType'),
                PlanTypeSrNo: int(val('txtPlanTypeSrNo')),
                RefInvoiceNo: val('txtInvoiceReference'),
                LotReference: val('txtOtherReference'),
                ProductionType: text('cmbProductionType'),
                RefSalesOrderId: int(val('cmbSaleOrder')),
                WorkInProccessAcId: int(val('CmbWorkInProcess')),
                FinishGoodsAcId: int(val('CmbFinishGoodsAc')),
                ByProductacId: int(val('CmbByProductionAc')),
                JobLotId: int(val('CmbLotAc')),
                WipItemId: int(val('CmbWipItem')),
                StartDate: val('txtstartDate'),
                EndDate: val('txtEndDate'),
                PlanStatus: text('cmbStatus'),
                PackingInstructions: val('txtPlantInstruction'),
                ProductionInsturction: val('txtProductionInstruction'),
                DeliveryInstructions: val('txtDeliveryInstruction'),
                OtherInstructions: val('txtOtherInstruction'),
                invProductionJobOrderOutput: outputRows.map(function (r) {
                    return {
                        ItemId: int(r.ItemId),
                        InvPackingTypeId: int(r.PackingTypeId),
                        QtyInner: num(r.QtyInner),
                        UomSchIdInner: int(r.InnerPackSizeId),
                        QtyOuter: num(r.QtyOuter),
                        UomSchIdOuter: int(r.OuterPackSizeId),
                        NetWeight: num(r.Weight),
                        PackRemarks: r.PackRemarks || '',
                        CropYearId: int(r.CropYearId),
                        WarehouseId: int(r.WareHouseId),
                        JobLotId: int(r.JobLotId)
                    };
                }),
                invProductionJobOrderInput: inputRows.map(function (r) {
                    return {
                        ItemId: int(r.ItemId),
                        CropYearId: int(r.CropYearId),
                        BatchCrop: r.CropYear || '',
                        JobLotId: int(r.JobLotId),
                        Qty: num(r.Qty),
                        UomSchIdQty: int(r.PackSizeId),
                        NetWeight: num(r.Weight),
                        WarehouseId: int(r.WareHouseId),
                        PackingTypeId: int(r.PackTypeId),
                        RemarksInputDt: r.Remarks || ''
                    };
                })
            };

            say('Saving...');
            return postJson(api + '/save', dto).then(function (res) {
                box((RECID > 0 ? 'Update Successfully' : 'Save Successfully') + (res.planCode || ''));
                say('Saved.');
                btnNew_Click();
            }).catch(function (e) {
                say('Not saved.');
                box(e.message);
            });
        });
    }

    /* Reset (:1390) */
    function btnNew_Click() {
        RECID = 0;
        outputRows = []; inputRows = [];
        renderOutput(); renderInput();
        setVal('txtPlanDate', today());
        ['txtInvoiceReference', 'txtOtherInstruction', 'txtOtherReference', 'txtPlantInstruction',
         'txtProductionInstruction', 'txtDeliveryInstruction', 'txtPlanTypeSrNo'].forEach(function (id) { setVal(id, ''); });
        ['cmbSaleOrder', 'CmbWorkInProcess', 'CmbWipItem', 'CmbFinishGoodsAc', 'CmbByProductionAc',
         'CmbLotAc', 'cmbProductionType', 'cmbStatus', 'cmbPlanType', 'cmbPlantFeader'].forEach(function (id) { setVal(id, '0'); });
        clearOutputEntry(); clearInputEntry();
        btnCancelUpdateDetial_Click(); btncancelgrdinput_Click();
        $id('btnUpdate').style.display = 'none';
        $id('btnSave').style.display = '';
        /* cmbJobLotOutput / cmbjoblotinput are re-enabled by Reset (:1420-1421). */
        $id('cmbJobLotOutput').disabled = false;
        $id('cmbjoblotinput').disabled = false;
        say('Ready');
        if (window.DesktopCombo) window.DesktopCombo.refresh();
        return GeneratePlanCode().catch(function (e) { box(e.message); });
    }

    function btnRefresh_Click() { busy('btnRefresh', function () { return btnNew_Click(); }); }

    function today() {
        var d = new Date();
        var m = String(d.getMonth() + 1).padStart(2, '0');
        var day = String(d.getDate()).padStart(2, '0');
        return d.getFullYear() + '-' + m + '-' + day;
    }

    /** yyyy-MM-dd from whatever shape the procedure returned - never via toISOString, which
     *  shifts the day in any timezone behind UTC. */
    function dateOnly(v) {
        if (!v) return '';
        var s = String(v);
        var m = s.match(/^(\d{4})-(\d{2})-(\d{2})/);
        if (m) return m[1] + '-' + m[2] + '-' + m[3];
        var d = new Date(s);
        if (isNaN(d.getTime())) return '';
        return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0');
    }

    /* DataGridHistory_DoubleClick (:1850) -> load that job order into the form. */
    function loadJobOrder(id) {
        return getJson(api + '/' + id).then(function (d) {
            if (!d || !d.header) { box('Job Order ' + id + ' not found.'); return; }
            var h = d.header;
            RECID = int(h.Id);
            setVal('txtPlanDate', dateOnly(h.PlanDate));
            setVal('txtPlanCode', h.PlanCode);
            $id('lblPlanCode').textContent = h.PlanCode === null || h.PlanCode === undefined ? '-' : h.PlanCode;
            setVal('cmbPlantFeader', h.InvProductionPlantId);
            selectByText('cmbPlanType', h.PlanType);
            setVal('txtPlanTypeSrNo', h.PlanTypeSrNo);
            setVal('txtInvoiceReference', h.RefInvoiceNo);
            setVal('txtOtherReference', h.LotReference);
            selectByText('cmbProductionType', h.ProductionType);
            setVal('cmbSaleOrder', h.RefSalesOrderId);
            setVal('CmbWorkInProcess', h.WorkInProccessAcId);
            setVal('CmbFinishGoodsAc', h.FinishGoodsAcId);
            setVal('CmbByProductionAc', h.ByProductacId);
            setVal('CmbLotAc', h.JobLotId);
            setVal('CmbWipItem', h.WipItemId);
            setVal('txtstartDate', dateOnly(h.StartDate));
            setVal('txtEndDate', dateOnly(h.EndDate));
            selectByText('cmbStatus', h.PlanStatus);
            setVal('txtPlantInstruction', h.PackingInstructions);
            setVal('txtProductionInstruction', h.ProductionInsturction);
            setVal('txtDeliveryInstruction', h.DeliveryInstructions);
            setVal('txtOtherInstruction', h.OtherInstructions);

            /* LoadData (:1885-1900) - the same column order the desktop puts in its two grids. */
            inputRows = (d.input || []).map(function (r) {
                return {
                    ItemId: r.ItemId, ItemName: r.ItemName,
                    CropYearId: r.CropYearId, CropYear: r.BatchCrop,
                    JobLotId: r.JobLotId, JobLot: r.JobLotCode,
                    Qty: r.Qty, PackSizeId: r.UomSchIdQty, PackSize: r.UOMCode,
                    Weight: r.NetWeight,
                    WareHouseId: r.WarehouseId, WareHouse: r.WareHouseName,
                    Remarks: r.RemarksInputDt,
                    PackTypeId: r.PackingTypeId, PackType: r.PackingType
                };
            });
            outputRows = (d.output || []).map(function (r) {
                return {
                    ItemId: r.ItemId, ItemName: r.ItemName,
                    PackingTypeId: r.InvPackingTypeId, PackingTypeName: r.PackTypeDesc,
                    QtyInner: r.QtyInner, InnerPackSizeId: r.UomSchIdInner, InnerPackSizeName: r.InnerUomCode,
                    QtyOuter: r.QtyOuter, OuterPackSizeId: r.UomSchIdOuter, OuterPackSizeName: r.outerUOMCode,
                    Weight: r.NetWeight, PackRemarks: r.PackRemarks,
                    CropYearId: r.CropYearId, CropYear: r.CropYear,
                    WareHouseId: r.WarehouseId, WareHouseName: r.WareHouseName,
                    JobLotId: r.JobLotId, JobLotDescription: r.JobLotDescription
                };
            });
            renderInput(); renderOutput();

            $id('btnSave').style.display = 'none';
            $id('btnUpdate').style.display = '';
            showView('form');
            say('Job Order ' + h.PlanCode + ' loaded.');
            if (window.DesktopCombo) window.DesktopCombo.refresh();
        }).catch(function (e) { box(e.message); });
    }

    /** The header stores PlanType / ProductionType / PlanStatus as TEXT, so the combo is matched
     *  on its label rather than on an id that is not in the row. */
    function selectByText(id, label) {
        var sel = $id(id);
        if (!sel) return;
        var want = String(label === null || label === undefined ? '' : label).trim();
        for (var i = 0; i < sel.options.length; i++) {
            if (sel.options[i].text.trim() === want) { sel.selectedIndex = i; return; }
        }
        sel.selectedIndex = 0;
    }

    /* ============================================================================ history */

    /* BindHistoryGrid (:1919) - 50 on open, 0 for Load All. */
    function loadHistory(noOfRecords) {
        var btn = noOfRecords === 0 ? $id('btnloadall') : $id('btnTop50');
        return busy(btn, function () {
            return getJson(api + '/history?noOfRecords=' + (noOfRecords || 0)).then(function (d) {
                var rows = d.rows || [];
                var html = '';
                rows.forEach(function (r) {
                    html += '<tr>'
                         + '<td>' + esc(r.Date) + '</td>'
                         + '<td><a class="win-doc-link" onclick="ProductionJobOrder.loadJobOrder(' + int(r.Id) + ')">' + esc(r.Code) + '</a></td>'
                         + '<td>' + esc(r.Type) + '</td>'
                         + '<td>' + esc(r.InvRef) + '</td>'
                         + '<td>' + esc(r.PType) + '</td>'
                         + '<td>' + esc(r.SO) + '</td>'
                         + '<td>' + esc(r.StartDate) + '</td>'
                         + '<td>' + esc(r.EndDate) + '</td>'
                         + '<td>' + esc(r.PlanStatus) + '</td>'
                         + '<td>' + esc(r.PlantInst) + '</td>'
                         + '<td>' + esc(r.DelInst) + '</td>'
                         + '<td>' + esc(r.ProdInst) + '</td>'
                         + '<td>' + esc(r.OtherInst) + '</td>'
                         + '<td>' + esc(r.Attachments) + '</td>'
                         + '<td><button type="button" class="win-btn-tool" style="background:#e0f2f1;"'
                         + ' onclick="ProductionJobOrder.loadJobOrder(' + int(r.Id) + ')">Detail</button></td>'
                         + '</tr>';
                });
                $id('gridHistory').innerHTML = html;
                $id('lblHistoryCount').textContent = rows.length + ' record(s)'
                    + (d.canViewAllRecords ? '' : ' - your own records only');
            }).catch(function (e) { box(e.message); });
        });
    }

    /* ============================================================================== views */

    function showView(which) {
        var form = which !== 'history';
        $id('mainViewForm').style.display = form ? '' : 'none';
        $id('mainViewHistory').style.display = form ? 'none' : '';
        $id('tabForm').className = form ? 'active' : '';
        $id('tabHistory').className = form ? '' : 'active';
        if (!form) loadHistory(50);          // tabControl1_SelectedIndexChanged (:2288)
    }

    function toggleFullscreen(boxId) {
        var el = $id(boxId);
        if (el) el.classList.toggle('is-fullscreen');
    }

    /* ================================================================================ boot */

    function boot() {
        setVal('txtPlanDate', today());
        setVal('txtstartDate', today());
        setVal('txtEndDate', today());
        loadLookups().then(function () {
            /* EntryTypeFill activates Rows[0] = "Input", which selects tab 1. */
            selectByText('cmbentrytype', 'Input');
            selectDetailTab(1, true);
            if (window.DesktopCombo) window.DesktopCombo.refresh();
            return GeneratePlanCode();
        }).catch(function (e) { box(e.message); });
    }

    window.ProductionJobOrder = {
        editOutputRow: editOutputRow,
        editInputRow: editInputRow,
        loadJobOrder: loadJobOrder
    };

    /* The template calls these by name from inline handlers. */
    window.onPlanTypeChange = onPlanTypeChange;
    window.onEntryTypeChange = onEntryTypeChange;
    window.onFilterTypeChange = onFilterTypeChange;
    window.onDocNoChange = onDocNoChange;
    window.onOutputItemChange = onOutputItemChange;
    window.onInputItemChange = onInputItemChange;
    window.Getweight = Getweight;
    window.GetweightInput = GetweightInput;
    window.btnplus_Click = btnplus_Click;
    window.btnUpdateDetail_Click = btnUpdateDetail_Click;
    window.btnCancelUpdateDetial_Click = btnCancelUpdateDetial_Click;
    window.btnadddetailinputgrd_Click = btnadddetailinputgrd_Click;
    window.btnupdategrdinput_Click = btnupdategrdinput_Click;
    window.btncancelgrdinput_Click = btncancelgrdinput_Click;
    window.btnSave_Click = btnSave_Click;
    window.btnNew_Click = btnNew_Click;
    window.btnRefresh_Click = btnRefresh_Click;
    window.selectDetailTab = selectDetailTab;
    window.showView = showView;
    window.toggleFullscreen = toggleFullscreen;
    window.loadHistory = loadHistory;

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
    else boot();
}());
