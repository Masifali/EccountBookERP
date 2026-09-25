/* ============================================================================================
 * Stock Conversion - invfrmStockConversionProduction.cs, DocTypeId 66.
 *
 * VIEWER: documents open, recalculate (Generate), print (605) and show their voucher (118), exactly
 * as the desktop does. Save / Update answer with a message: the desktop's save also posts stock
 * movements, a voucher (whose type-3 gain case cannot balance) and contractor wages bills, and
 * none of that is ported. Nothing on this page writes.
 *
 * Layout, captions, grid columns and formats follow InitializeComponent and the grid-setting
 * methods; the line numbers below are resolved-source's.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/production/stock-conversion';
    var RECID = 0;
    var K = window.ReportKit;

    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function say(m) { var e = $id('lblFormStatus'); if (e) e.textContent = m; }
    function box(m) { window.alert(m); }

    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function num(v) {
        var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, ''));
        return isNaN(n) ? 0 : n;
    }
    function int(v) {
        var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10);
        return isNaN(n) ? 0 : n;
    }
    function fmt(n) {
        return (Math.round(n * 100) / 100).toLocaleString('en-US', { maximumFractionDigits: 2 });
    }

    /* Case-insensitive column read: a procedure's casing is not guaranteed, and a grid that
       silently shows blanks because the column was "ItemName" not "itemname" is the hardest kind
       of bug to see. */
    function col(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        for (var k in row) {
            if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === name.toLowerCase()) {
                return row[k];
            }
        }
        return '';
    }

    /** yyyy-MM-dd without toISOString, which shifts the day in any timezone behind UTC. */
    function dateOnly(v) {
        if (!v) return '';
        var s = String(v);
        var m = s.match(/^(\d{4})-(\d{2})-(\d{2})/);
        if (m) return m[1] + '-' + m[2] + '-' + m[3];
        var d = new Date(s);
        if (isNaN(d.getTime())) return '';
        return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0')
             + '-' + String(d.getDate()).padStart(2, '0');
    }
    function today() {
        var d = new Date();
        return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0')
             + '-' + String(d.getDate()).padStart(2, '0');
    }

    /* The button contract, one place: disabled at once, spinner, duplicate clicks ignored,
       re-enabled on success AND on failure. */
    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
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

    /* ------------------------------------------------ Detail field calculations (:5638-5757) */

    var UOM_ROWS = [];          // the item's schedule: Id, UOMCode, Equivalent, QtyEquivalent, BaseRateUom
    function uomEquivalent(selectId) {
        var id = String(val(selectId));
        for (var i = 0; i < UOM_ROWS.length; i++) if (String(col(UOM_ROWS[i], 'Id')) === id) return num(col(UOM_ROWS[i], 'Equivalent'));
        return 0;
    }
    /** CalculateWeight:5680 - Weight = Qty x the pack UOM's Equivalent, "#,##0.###"; else 0. */
    function calculateWeight() {
        var eq = uomEquivalent('cmbUOM'), q = num(val('txtQty'));
        setVal('txtUnitWeight', (eq > 0 && q > 0) ? K.num(eq * q, 3) : '0');
    }
    /** AmountCalculation:5638 - Amount = Weight / Rate UOM Equivalent x Rate, stringFormatsingle; else 0. */
    function amountCalculation() {
        var eq = uomEquivalent('cmbRateUom'), r = num(val('txtRate')), w = num(val('txtUnitWeight'));
        setVal('txtAmount', (r > 0 && eq > 0 && w > 0) ? K.fixed(w / eq * r, amountDec()) : '0');
    }
    function amountDec() { return LK && LK.amountDecimals !== undefined ? int(LK.amountDecimals) : 2; }
    function rateDec() { return LK && LK.rateDecimals !== undefined ? int(LK.rateDecimals) : 2; }
    function fq(v) { return K.num(v, 3); }                  // "#,##0.###"
    function fr(v) { return K.fixed(v, rateDec()); }        // DecimalRateFormate
    function fa(v) { return K.fixed(v, amountDec()); }      // stringFormatsingle

    // ------------------------------------------------------------------------------ lookups

    /* The thirteen pickers are bound to the same sources the desktop's Load binds (:749-760) -
       one read, /lookups - and the two UOM pickers per item, /uoms. Which column is the value and
       which the text is the desktop's BindDDL call, noted beside each fill below. */
    var LK = null;              // the /lookups payload
    var DETAIL_ROWS = [];       // the rows the Conversion Detail grid is showing

    /* DDL.BindDDL(..., ZeroIndex: false) inserts no "0" row of its own; the leading blank option
       here is the web's way of showing "nothing selected", which an UltraCombo shows as empty
       text. keep = the value to re-select if it is still in the new list (BindAndRetainSelection
       and BindProductionDepartment both do this; the others start empty). */
    function fill(id, rows, valueKey, textKey, keep) {
        var sel = $id(id);
        if (!sel) return;
        var html = '<option value="0"></option>';
        var found = false;
        (rows || []).forEach(function (r) {
            var v = String(col(r, valueKey));
            if (keep !== undefined && keep !== null && String(keep) === v) found = true;
            html += '<option value="' + esc(v) + '">' + esc(col(r, textKey)) + '</option>';
        });
        sel.innerHTML = html;
        sel.value = found ? String(keep) : '0';
    }
    function selectFirst(id) {
        var sel = $id(id);
        if (sel && sel.options.length > 1) sel.selectedIndex = 1;
    }
    /** UltraCombo.Text = x - selects the row whose display text is x, else leaves it empty. */
    function selectByText(id, text) {
        var sel = $id(id);
        if (!sel) return false;
        var t = String(text === null || text === undefined ? '' : text);
        for (var i = 0; i < sel.options.length; i++) {
            if (sel.options[i].textContent === t && sel.options[i].value !== '0') { sel.selectedIndex = i; return true; }
        }
        sel.value = '0';
        return false;
    }
    function selectedText(id) {
        var sel = $id(id);
        if (!sel || sel.selectedIndex < 0 || sel.value === '0') return '';
        return sel.options[sel.selectedIndex].textContent;
    }
    function refreshCombos() { if (window.DesktopCombo) window.DesktopCombo.refresh(); }

    function loadEntryTypes() {
        return getJson(api + '/entry-types').then(function (rows) {
            /* CmbEntryTypeFill:792 - DDL.BindDDL(dt, cmbEntryType, "Id", "EntryType", ...). */
            fill('cmbEntryType', rows, 'Id', 'EntryType', int(val('cmbEntryType')) || null);
            refreshCombos();
        });
    }

    /**
     * Load:749-760 and btnRefresh_Click:1745. On Refresh the desktop re-binds the lists without
     * resetting the form: Production Department and Conversion Type keep their selection when it
     * is still offered, the detail pickers are re-bound empty.
     */
    function loadLookups(firstTime) {
        return getJson(api + '/lookups').then(function (d) {
            LK = d || {};
            if (firstTime) {
                /* ParentCategoryFill:1142 - "Id"/"InvParentCateDescription", then Rows[0].Activate().
                   Only at Load: btnRefresh_Click does not call it again. */
                fill('cmbParentCategory', LK.parentCategories, 'Id', 'InvParentCateDescription');
                selectFirst('cmbParentCategory');
            }
            /* BindProductionDepartment:831 - "Id"/"WareHouseName", previous value kept if present. */
            fill('CmbProductionDepartment', LK.productionDepartments, 'Id', 'WareHouseName',
                 int(val('CmbProductionDepartment')) || null);
            /* BindProductionType:871 - "Id"/"type" via BindAndRetainSelection, then Rows[0].Activate().
               Row 5 is already gone unless ERP feature 24 is on. */
            fill('CmbConversionType', LK.conversionTypes, 'Id', 'type');
            selectFirst('CmbConversionType');
            /* Warehouse():894 - "Id"/"WareHouseName". */
            fill('cmbGodown', LK.warehouses, 'Id', 'WareHouseName');
            /* ItemFill + BindItemCombo(parent category). */
            bindItems();
            /* combojoblotfill():966 - one list bound to BOTH cmbLot and CmbJobLotForGrid. */
            fill('cmbLot', LK.jobLots, 'Id', 'JobLotDescription');
            fill('CmbJobLotForGrid', LK.jobLots, 'Id', 'JobLotDescription');
            /* CropYear():990 - "Id"/"CropYear". */
            fill('CmbCropyr', LK.cropYears, 'Id', 'CropYear');
            /* bagType():1008 - "Id"/"PackTypeDesc". */
            fill('cmbBagType', LK.packingTypes, 'Id', 'PackTypeDesc');
            if (firstTime) {
                /* MoistureSlabFill():1387 - Load only; Refresh does not re-bind it. */
                fill('cmbMoistureSlab', LK.moistureSlabs, 'Id', 'MoistureSlabDescription');
            }
            /* AccountFills():1025 - "Id"/"AccountTitle", account types 4,12,10,9. */
            fill('CmbDifferenceAccount', LK.differenceAccounts, 'Id', 'AccountTitle',
                 int(val('CmbDifferenceAccount')) || null);
            conversionTypeChanged();
            refreshCombos();
        });
    }

    /**
     * BindItemCombo:934 - the cached ReadAllItems list, filtered IN MEMORY on
     * InventoryParentCategoriesId when a parent category is chosen, else the whole list.
     * "Id"/"ItemName". Re-binding clears the item, and with it the two UOM pickers.
     */
    function bindItems() {
        var parent = int(val('cmbParentCategory'));
        var all = (LK && LK.items) || [];
        var rows = parent > 0
            ? all.filter(function (r) { return int(col(r, 'InventoryParentCategoriesId')) === parent; })
            : all;
        fill('cmbItem', rows, 'Id', 'ItemName');
        fill('cmbUOM', [], 'Id', 'UOMCode');
        fill('cmbRateUom', [], 'Id', 'UOMCode');
    }

    /**
     * bindRateUomAndItemPackUom:1041 (from cmbItem_Leave). Both pickers get the item's schedule
     * ("Id"/"UOMCode"); each keeps its previous TEXT only when a row with the same UOMCode exists.
     * Returns a promise so a row being loaded can set the ids afterwards (grdByProduct_DoubleClick
     * does exactly that order: bind, then .Value = id).
     */
    function bindUoms() {
        var itemId = int(val('cmbItem'));
        var packText = selectedText('cmbUOM'), rateText = selectedText('cmbRateUom');
        if (itemId <= 0) {
            UOM_ROWS = [];
            fill('cmbUOM', [], 'Id', 'UOMCode');
            fill('cmbRateUom', [], 'Id', 'UOMCode');
            refreshCombos();
            return Promise.resolve();
        }
        return getJson(api + '/uoms?itemId=' + itemId).then(function (rows) {
            UOM_ROWS = rows || [];
            fill('cmbUOM', rows, 'Id', 'UOMCode');
            fill('cmbRateUom', rows, 'Id', 'UOMCode');
            if (packText) selectByText('cmbUOM', packText);
            if (rateText) selectByText('cmbRateUom', rateText);
            refreshCombos();
        });
    }

    function itemChanged() {
        bindUoms().catch(function (e) { box(e.message); });
    }

    /** cmbParentCategory_Leave:1503 - BindItemCombo on the new category. */
    function parentCategoryChanged() {
        bindItems();
        refreshCombos();
    }

    /**
     * CmbConversionType_Leave:1448 - Diff A/c with its label for type 3, Remarks 26px high for
     * type 3 and 54px otherwise, and the toolbar/checkbox visibility rules. Note the desktop's
     * own override: Issuance Load becomes visible for every type but 5 here, whatever
     * IssuanceByLoader said at Load (kept).
     */
    function conversionTypeChanged() {
        var t = int(val('CmbConversionType'));
        var t3 = t === 3, t5 = t === 5;
        var w = $id('wrapDifferenceAccount');
        if (w) w.style.display = t3 ? '' : 'none';
        $id('txtRemarks').style.height = t3 ? '26px' : '54px';
        show('btnLoadOutPut', !t3 && LK && LK.saleMinusAllowedAgainstFifo);
        show('btnIssuanceLoad', !t5);
        show('btnStockReleaseFromFumigation', t5);
        var wages = !t5 && LK && LK.contractWagesChargeToProduct;
        show('btnWagesSchedule', wages); show('btnWagesExempt', wages); show('tabWages', wages);
        show('wrapFumigation', !t5 && LK && LK.issuanceByLoader);
        renderSummary();
    }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }

    /**
     * cmbEntryType_Leave:1407. Parent category and entry type are required, in that order.
     * "Recovery Head Rice" makes Rate read-only. Any type other than "Issue" re-binds the item
     * list (BindItemCombo) and the job lots (combojoblotfill), which clears both selections.
     */
    function entryTypeChanged() {
        if (!selectedText('cmbParentCategory')) {
            box('Please Select Parent Category First!');
            return;
        }
        var t = selectedText('cmbEntryType');
        if (!t) { box('Please Select Entry Type First!'); return; }
        var rate = $id('txtRate');
        if (rate) rate.readOnly = (t === 'Recovery Head Rice');
        if (t !== 'Issue') {
            bindItems();
            fill('cmbLot', (LK && LK.jobLots) || [], 'Id', 'JobLotDescription');
            fill('CmbJobLotForGrid', (LK && LK.jobLots) || [], 'Id', 'JobLotDescription');
            /* txtAverageRate is also zeroed here; the Avg Rate box is not on this page. */
        }
        refreshCombos();
        /* :1439-1440 - the handler ends with CalculateWeight + AmountCalculation. */
        calculateWeight(); amountCalculation();
    }

    /** resetDetail:1677. */
    function resetDetail() {
        ['cmbItem', 'cmbUOM', 'cmbGodown', 'CmbCropyr', 'cmbLot', 'cmbBagType', 'cmbRateUom',
         'cmbMoistureSlab'].forEach(function (id) { var e = $id(id); if (e) e.value = '0'; });
        ['txtUnitWeight', 'txtMoisture', 'txtQty', 'txtRate', 'txtAmount', 'txtdeailRemarks', 'txtStock', 'txtAverageRate']
            .forEach(function (id) { setVal(id, ''); });
        UOM_ROWS = [];
    }

    var INPUT_ROWS = [], OUTPUT_ROWS = [], PM_ROWS = [], OH_ROWS = [], WAGES_ROWS = [], WAGES_SUB = 'regular';

    /* CmbConversionType.Enabled = grdInput.RowCount == 0 && grdByProduct.RowCount == 0. */
    function lockConversionType() {
        var e = $id('CmbConversionType');
        if (e) e.disabled = (INPUT_ROWS.length + OUTPUT_ROWS.length) > 0;
        refreshCombos();
    }

    /**
     * grdByProduct_DoubleClick:2314 - output rows only (grdInput's double-click is empty).
     * Refused for type 5 and for a row whose stock was issued elsewhere. The row's Remarks go
     * into the HEADER Remarks box, as on the desktop.
     */
    function editRow(i) {
        var r = OUTPUT_ROWS[i];
        if (!r) return;
        if (int(val('CmbConversionType')) === 5) return;
        if (num(col(r, 'IssueWeight')) > 0) { box('Record Not Update because record has exist another Form'); return; }
        selectByText('cmbEntryType', col(r, 'EntryType'));
        setSel('cmbGodown', col(r, 'WarehouseId'));
        setSel('cmbLot', col(r, 'JobLotId'));
        setSel('cmbBagType', col(r, 'PackingtypeId'));
        setSel('cmbItem', col(r, 'ItemId'));
        return bindUoms().then(function () {
            setSel('cmbUOM', col(r, 'ItemUomId'));
            selectByText('CmbCropyr', col(r, 'CropBatch'));
            setSel('cmbRateUom', col(r, 'RateUOMId'));
            setVal('txtQty', col(r, 'Qty'));
            setVal('txtUnitWeight', col(r, 'Weight'));
            setVal('txtRate', fr(col(r, 'Rate')));
            setVal('txtAmount', fa(col(r, 'Amount')));
            setVal('txtMoisture', col(r, 'Moisture'));
            setSel('cmbMoistureSlab', col(r, 'MoistureSlabId'));
            setVal('txtRemarks', col(r, 'Remarks'));
            refreshCombos();
            say('Output row ' + (i + 1) + ' shown in Detail (Save is not enabled).');
        }).catch(function (e) { box(e.message); });
    }

    /** BtnUpdateComboValueIngrid_Click:7102 -> UpdatejobLotInGrid:7117 on the output rows. */
    function applyJobLotToGrid() {
        if (int(val('CmbConversionType')) === 5) return;
        if (!OUTPUT_ROWS.length) { box('Output Grid Record not Found'); return; }
        var id = int(val('CmbJobLotForGrid'));
        var text = id > 0 ? selectedText('CmbJobLotForGrid') : '';
        if (id <= 0) {
            var first = OUTPUT_ROWS.filter(function (r) { return int(col(r, 'JobLotId')) > 0; })[0];
            if (first) { id = int(col(first, 'JobLotId')); text = col(first, 'JobLotDescription'); }
        }
        if (id === 0) { box('Please Select JobLot'); return; }
        OUTPUT_ROWS.forEach(function (r) { r.JobLotId = id; r.JobLotDescription = text; });
        renderOutput();
        say('Job lot applied to ' + OUTPUT_ROWS.length + ' output row(s) on screen. Not saved.');
    }

    // --------------------------------------------------------------------------------- load

    function loadByDocNo() {
        var docNo = int(val('txtLoadDocNo'));
        if (docNo <= 0) { box('Enter a document number'); return; }
        return busy('btnLoad', function () {
            say('Loading...');
            return getJson(api + '/id-by-doc-no?docSrNo=' + docNo).then(function (d) {
                var id = d && d.id ? d.id : 0;
                if (id <= 0) { say('Not found.'); box('No Stock Conversion with document number ' + docNo); return; }
                return load(id);
            }).catch(function (e) { say('Not loaded.'); box(e.message); });
        });
    }

    /** ReadById:4663. RefreshForm + resetDetail + summeryreset first, so nothing of the previous
     *  document survives (Pro_Department is set only when the loaded value is > 0). */
    function load(id) {
        return getJson(api + '/' + id).then(function (d) {
            if (!d || !d.header) { box('Stock Conversion ' + id + ' not found.'); return; }
            var h = d.header;
            resetDetail();
            setSel('CmbProductionDepartment', 0);
            RECID = int(col(h, 'Id'));

            setVal('txtdocnumber', col(h, 'DocSrNo'));
            $id('lblDocNo').textContent = col(h, 'DocSrNo') ? '#' + col(h, 'DocSrNo') : '';
            setVal('txtDocdate', dateOnly(col(h, 'DocDate')));
            setVal('txtProductionNo', col(h, 'ProductionNo'));
            setVal('txtRemarks', col(h, 'Remarks'));
            setSel('cmbParentCategory', col(h, 'parentCategoryId'));
            setSel('CmbConversionType', int(col(h, 'ConversionTypeId')));
            if (int(val('CmbConversionType')) === 0) setSel('CmbConversionType', 1);
            if (int(val('CmbConversionType')) === 3) setSel('CmbDifferenceAccount', col(h, 'DifferenceAccountId'));
            if (int(col(h, 'EBDepartmentId')) > 0) setSel('CmbProductionDepartment', col(h, 'EBDepartmentId'));
            bindItems();

            INPUT_ROWS = []; OUTPUT_ROWS = [];
            var onHold = false;
            (d.details || []).forEach(function (r) {
                var o = {}; for (var k in r) o[k] = r[k];
                if (String(col(o, 'EntryType')) === 'Issue') { o.BalQty = 0; o.BalWeight = 0; INPUT_ROWS.push(o); }
                else {
                    /* :4744 - AmountWithoutExpenses = Amount - (PackingMaterialAmount + ExpenseAmount) */
                    o.AmountWithoutExpenses = num(col(o, 'Amount')) - (num(col(o, 'PackingMaterialAmount')) + num(col(o, 'ExpenseAmount')));
                    onHold = !!col(o, 'IsOnHold') && col(o, 'IsOnHold') !== 0 && col(o, 'IsOnHold') !== '0';
                    OUTPUT_ROWS.push(o);
                }
            });
            $id('chkFumigationOnHold').checked = onHold;      // the LAST output row's IsOnHold wins
            PM_ROWS = d.packings || [];
            OH_ROWS = d.expenses || [];
            WAGES_ROWS = d.wages || [];
            /* ReadById:4757 - Generate runs unless the type is 3. */
            if (int(col(h, 'ConversionTypeId')) !== 3) handleAverageRateCalculation(INPUT_ROWS.concat(OUTPUT_ROWS));
            conversionTypeChanged();
            renderAll();
            /* :4709-4710 - Update visible, Save hidden. */
            show('btnUpdate', true); show('btnSave', false);
            showView('form');
            say('Document ' + col(h, 'DocSrNo') + ' loaded.');
        });
    }

    /** UltraCombo.Value = id: selects it when the list has it, otherwise nothing. */
    function setSel(id, v) {
        var sel = $id(id);
        if (!sel) return;
        var s = String(v === null || v === undefined ? '' : v);
        var ok = false;
        for (var i = 0; i < sel.options.length; i++) if (sel.options[i].value === s) { ok = true; break; }
        sel.value = ok ? s : '0';
    }

    /**
     * handleAverageRateCalculation:6053, on the loaded rows.
     *   input      = sum of every Issue row's Amount
     *   byProduct  = sum of the stored Amount of the "Recovery By Product" rows
     *   headWeight = sum of the "Recovery Head Rice" rows' Weight
     *   perKg      = (input - byProduct) / headWeight
     * By-product row:  Amount = AmountWithoutExpenses + PMAmount + ExpAmount + ItemPMAmount
     *                           + ItemExpAmount + WagesAmount
     * Head Rice row:   AmountWithoutExpenses = Weight / RateEquivalent * (perKg * RateEquivalent)
     *                  Amount = that + the same five additions
     *                  Rate   = Amount / Weight * RateEquivalent
     * RateEquivalent is the Rate UOM's Equivalent (the detail procedure's "Equivalent").
     * Division by zero is left to behave as the C# double does (Infinity/NaN), not guarded.
     */
    function handleAverageRateCalculation(rows) {
        var input = 0, byProduct = 0, headWeight = 0;
        rows.forEach(function (r) {
            var t = String(col(r, 'EntryType'));
            if (t === 'Issue') input += num(col(r, 'Amount'));
            else if (t === 'Recovery By Product') byProduct += num(col(r, 'Amount'));
            else if (t === 'Recovery Head Rice') headWeight += num(col(r, 'Weight'));
        });
        var perKg = (input - byProduct) / headWeight;
        rows.forEach(function (r) {
            var t = String(col(r, 'EntryType'));
            var extras = num(col(r, 'PackingMaterialAmount')) + num(col(r, 'ExpenseAmount'))
                       + num(col(r, 'ItemPmCost')) + num(col(r, 'ItemOhCost')) + num(col(r, 'WagesAmount'));
            if (t === 'Recovery By Product') {
                r.Amount = num(r.AmountWithoutExpenses) + extras;
            } else if (t === 'Recovery Head Rice') {
                var eq = num(col(r, 'Equivalent'));
                var w = num(col(r, 'Weight'));
                var awe = w / eq * (perKg * eq);
                r.AmountWithoutExpenses = awe;
                r.Amount = awe + extras;
                r.Rate = r.Amount / w * eq;
            }
        });
    }

    // -------------------------------------------------------------------------------- grids

    function renderAll() { renderInput(); renderOutput(); renderPackings(); renderExpenses(); renderWages(); renderSummary(); lockConversionType(); }

    /* ------------------------------------------------------------ contractor wages (read view)
     * WagesDetailReadbyId:5285 fills dtdetail (Regular Wages, grdwagesDetail) and dtStiching
     * (Other Wages, grdStiching - rows with WagesTypeId 2) from
     * USP_InvContractorWagesBillHeader_DetailByRefDocument. DetailGridSettings / GridStichingSettings:
     * grouped by TransactionType, visible columns below (SupplierId shows the contractor, captioned
     * "Contractor Name"; WagesId the activity), RateWithoutAddLess / RateAddLess only with
     * EnableAddLessOnWagesRegular, Weight/Quantity/BillWeight/Amount summed, Free Of Cost in red. */
    function wagesCols() {
        var c = [['CompanyName','Contractor Name','t'],['WagesAccountName','Labour / Wages Activity','t'],['__WagesType','Wages Type','t'],
            ['PackTypeDesc','packing Type','t'],['Weight','Weight','q',1],['PackSize','Pack Size','t'],['Qty','Quantity','q',1],
            ['WeightCut','Weight Cut','q'],['BillWeight','Bill Weight','q',1]];
        if (LK && LK.enableAddLessOnWagesRegular) c.push(['WageRate','Rate Without Add Less','q'], ['RateAddLess','Rate Add Less','q']);
        c.push(['WageRate','Rate','q'],['WagesAmount','Amount','q',1],['ItemName','Item','t'],['JobLotDescription','job Lot','t'],
            ['Crop','Crop','t'],['WareHouseFrom','Move From','t'],['RefLineId','RowNo','t']);
        return c;
    }
    function wagesTab(which) {
        WAGES_SUB = which;
        $id('wagesSubRegular').classList.toggle('is-active', which === 'regular');
        $id('wagesSubOther').classList.toggle('is-active', which === 'other');
        $id('wagesTitle').textContent = which === 'regular' ? 'In this Grid User Will add Regular Wages' : 'In this Grid User Will add Other Wages';
        renderWages();
    }
    function renderWages() {
        if (!$id('wagesHead')) return;
        var cols = wagesCols();
        var rows = WAGES_ROWS.filter(function (r) { return (int(col(r, 'WagesTypeId')) === 2) === (WAGES_SUB === 'other'); });
        var cell = function (r, c) {
            if (c[0] === '__WagesType') return (col(r, 'FreeOfCost') === true || int(col(r, 'FreeOfCost')) === 1) ? 'Free Of Cost' : 'Regular';
            var v = col(r, c[0]);
            return c[2] === 'q' ? (v === null || v === '' || v === undefined ? '' : fq(v)) : esc(v);
        };
        $id('wagesHead').innerHTML = cols.map(function (c) { return '<th' + (c[2] === 'q' ? ' style="text-align:right"' : '') + '>' + esc(c[1]) + '</th>'; }).join('');
        var groups = [];
        rows.forEach(function (r) { var t = String(col(r, 'TransactionType') || ''); if (groups.indexOf(t) < 0) groups.push(t); });
        var html = '';
        groups.forEach(function (g) {
            html += '<tr class="sc-group"><td colspan="' + cols.length + '" style="background:#dfe9f5;font-weight:bold;">&#8863; Transaction Type: ' + esc(g) + '</td></tr>';
            rows.filter(function (r) { return String(col(r, 'TransactionType') || '') === g; }).forEach(function (r) {
                var foc = cell(r, ['__WagesType']) === 'Free Of Cost';
                html += '<tr' + (foc ? ' style="color:red"' : '') + '>' + cols.map(function (c) {
                    return '<td' + (c[2] === 'q' ? ' style="text-align:right"' : '') + '>' + cell(r, c) + '</td>'; }).join('') + '</tr>';
            });
        });
        $id('gridWages').innerHTML = html || '<tr><td colspan="' + cols.length + '" style="color:#777">No wages rows.</td></tr>';
        $id('wagesFoot').innerHTML = '<tr>' + cols.map(function (c) {
            return '<td style="text-align:right;font-weight:bold">' + (c[3] ? fq(rows.reduce(function (a, r) { return a + num(col(r, c[0])); }, 0)) : '') + '</td>'; }).join('') + '</tr>';
    }

    /** [name, caption, kind] - kind: q "#,##0.###", r rate, a amount, t text. Totals where the desktop sums. */
    var INPUT_COLS = [['EntryType','EntryType','t'],['WareHouseName','WareHouse','t'],['CropBatch','CropYear','t'],['ItemName','Item','t'],
        ['JobLotDescription','JobLot','t'],['UomCode','UOM','t'],['PackTypeDesc','PackingType','t'],['BalQty','BalQty','q',1],
        ['BalWeight','BalWeight','q',1],['Qty','Quantity','q',1],['Weight','Weight','q',1],['Rate','Rate','r'],['RateUom','RateUOM','t'],
        ['Amount','Amount','a',1],['Moisture','Moisture','t'],['MoistureSlabDescription','Moisture Slab Description','t'],['Remarks','Remarks','t']];
    function outputCols() {
        var c = [['EntryType','EntryType','t'],['WareHouseName','WareHouse','t'],['CropBatch','CropYear','t'],['ItemName','Item','t'],
            ['JobLotDescription','JobLot','t'],['UomCode','UOM','t'],['PackTypeDesc','PackingType','t'],['Qty','Quantity','q',1],
            ['Weight','Weight','q',1],['Rate','Rate','r'],['RateUom','RateUOM','t'],['AmountWithoutExpenses','AmountWithoutExpenses','a',1],
            ['Moisture','Moisture','t'],['MoistureSlabDescription','MoistureSlabDescription','t'],
            ['PackingMaterialAmount','General Packing Material Amount','a',1],['ItemPmCost','Item Packing Material Amount','a',1],
            ['ExpenseAmount','General Overhead Amount','a',1],['ItemOhCost','Item Overhead Amount','a',1],['Amount','Amount','a',1],
            ['Remarks','Remarks','t'],['IssueWeight','IssueWeight','q',1]];
        if (LK && LK.contractWagesChargeToProduct) c.push(['WagesAmount','WagesAmount','a',1]);
        return c;
    }
    function fmtKind(kind, v) { return kind === 'q' ? fq(v) : kind === 'r' ? fr(v) : kind === 'a' ? fa(v) : v; }

    function drawGrid(headId, bodyId, footId, cols, rows, pre, preHead, rowAttrs) {
        $id(headId).innerHTML = preHead + cols.map(function (c) {
            return '<th' + (c[2] !== 't' ? ' class="num"' : '') + '>' + esc(c[1]) + '</th>';
        }).join('');
        $id(bodyId).innerHTML = rows.map(function (r, i) {
            return '<tr' + (rowAttrs ? rowAttrs(r, i) : '') + '>' + pre(r, i) + cols.map(function (c) {
                return '<td' + (c[2] !== 't' ? ' class="num"' : '') + '>' + esc(fmtKind(c[2], col(r, c[0]))) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        var preCount = (preHead.match(/<th/g) || []).length;
        $id(footId).innerHTML = rows.length ? '<tr>' + (preCount ? '<td colspan="' + preCount + '"></td>' : '') + cols.map(function (c, k) {
            if (!c[3]) return '<td>' + (k === 0 && !preCount ? 'Total' : '') + '</td>';
            var t = rows.reduce(function (a, r) { return a + num(col(r, c[0])); }, 0);
            return '<td class="num">' + esc(fmtKind(c[2], t)) + '</td>';
        }).join('') + '</tr>' : '';
    }

    /* gridsettings (Input): [Select] checkbox + [X], two frozen columns. The X and the
       forward transfer belong to the edit side and are inert here. */
    function renderInput() {
        drawGrid('inputHead', 'gridInput', 'inputFoot', INPUT_COLS, INPUT_ROWS,
            function () { return '<td style="text-align:center;"><input type="checkbox" disabled></td><td><button type="button" class="sc-x" disabled>X</button></td>'; },
            '<th>Select</th><th>X</th>');
    }
    function renderOutput() {
        drawGrid('outputHead', 'gridOutput', 'outputFoot', outputCols(), OUTPUT_ROWS,
            function () { return '<td><button type="button" class="sc-x" disabled>X</button></td>'; }, '<th>X</th>',
            function (r, i) { return ' data-i="' + i + '" title="Double-click to show this row in Detail"'; });
    }
    /* ChargeTo is stored as "1"/"2"; the grid's value list shows the text (:816-824). */
    function chargeTo(v) { var s = String(v === null || v === undefined ? '' : v); return s === '1' ? 'Recovery By Product' : s === '2' ? 'Recovery Head Rice' : s; }
    function renderPackings() {
        var rows = PM_ROWS.map(function (r) { var o = {}; for (var k in r) o[k] = r[k]; o.ChargeToText = chargeTo(col(r, 'ChargeTo')); return o; });
        drawGrid('pmHead', 'gridPacking', 'pmFoot',
            [['ItemName','Item','t'],['WarehouseName','WareHouse','t'],['rackName','RackName','t'],['ItemCondition','Item Condition','t'],
             ['BrandName','BrandItem','t'],['BrandUom','BrandUom','t'],['ItemQty','ItemQTY','q',1],['ItemRate','ItemRate','r'],
             ['ItemAmount','Amount','a',1],['ChargeToText','Charge To','t'],['ContractScheduleNo','Schedule / Invoice No','t']],
            rows, function () { return '<td><button type="button" class="sc-x" disabled>X</button></td><td><button type="button" class="sc-x" disabled>+</button></td>'; },
            '<th>X</th><th>+</th>');
    }
    function renderExpenses() {
        var rows = OH_ROWS.map(function (r) { var o = {}; for (var k in r) o[k] = r[k]; o.ChargeToText = chargeTo(col(r, 'ChargeTo')); return o; });
        /* The account column is captioned "Item" on the desktop (:2565) - kept. */
        drawGrid('ohHead', 'gridExpense', 'ohFoot',
            [['AccountTitle','Item','t'],['BrandName','BrandItem','t'],['BrandUom','BrandUom','t'],['LedgerRemarks','LedgerRemarks','t'],
             ['ExpAmount','ExpAmount','a',1],['ChargeToText','ChargeTo','t']],
            rows, function () { return '<td><button type="button" class="sc-x" disabled>X</button></td><td><button type="button" class="sc-x" disabled>+</button></td>'; },
            '<th>X</th><th>+</th>');
    }

    /**
     * GenerateSummaryForUser:6234. Rates start as the SUM of the rows' rates and are replaced by
     * amount / weight x 40 when both are > 0. Gain/Loss (Finish Goods - Input) is visible only
     * for type 3; Wages Amount only with ContractWagesChargetoProductForStockConversion.
     */
    function renderSummary() {
        var s = { iq: 0, iw: 0, ir: 0, ia: 0, bq: 0, bw: 0, br: 0, ba: 0, fq: 0, fw: 0, fr: 0, fa: 0 };
        INPUT_ROWS.forEach(function (r) { s.iq += num(col(r, 'Qty')); s.iw += num(col(r, 'Weight')); s.ir += num(col(r, 'Rate')); s.ia += num(col(r, 'Amount')); });
        OUTPUT_ROWS.forEach(function (r) {
            var t = String(col(r, 'EntryType'));
            if (t === 'Recovery By Product') { s.bq += num(col(r, 'Qty')); s.bw += num(col(r, 'Weight')); s.br += num(col(r, 'Rate')); s.ba += num(col(r, 'Amount')); }
            else if (t === 'Recovery Head Rice') { s.fq += num(col(r, 'Qty')); s.fw += num(col(r, 'Weight')); s.fr += num(col(r, 'Rate')); s.fa += num(col(r, 'Amount')); }
        });
        if (s.ia > 0 && s.iw > 0) s.ir = s.ia / s.iw * 40;
        if (s.ba > 0 && s.bw > 0) s.br = s.ba / s.bw * 40;
        if (s.fa > 0 && s.fw > 0) s.fr = s.fa / s.fw * 40;
        var pmQ = 0, pmR = 0, pmA = 0, oh = 0;
        PM_ROWS.forEach(function (p) { pmQ += num(col(p, 'ItemQty')); pmR += num(col(p, 'ItemRate')); pmA += num(col(p, 'ItemAmount')); });
        OH_ROWS.forEach(function (e) { oh += num(col(e, 'ExpAmount')); });
        var wages = OUTPUT_ROWS.reduce(function (a, r) { return a + num(col(r, 'WagesAmount')); }, 0);
        var diff = s.fa - s.ia, gain = diff > 0, colr = gain ? '#1b7a1b' : '#b3261e';
        var t3 = int(val('CmbConversionType')) === 3;
        function head() { return '<thead><tr><th>Description</th><th class="num">ItemQty</th><th class="num">NetWeight</th><th class="num">Rate</th><th class="num">Amount</th></tr></thead>'; }
        function row(label, q, w, r, a, style) {
            return '<tr><td' + (style || '') + '>' + esc(label) + '</td><td class="num">' + (q === null ? '' : esc(fq(q))) + '</td><td class="num">'
                 + (w === null ? '' : esc(fq(w))) + '</td><td class="num">' + (r === null ? '' : esc(fr(r))) + '</td><td class="num"' + (style || '') + '>'
                 + (a === null ? '' : esc(fa(a))) + '</td></tr>';
        }
        var left = row('Raw Material', s.iq, s.iw, s.ir, s.ia) + row('By Product', s.bq, s.bw, s.br, s.ba) + row('Finish Goods', s.fq, s.fw, s.fr, s.fa)
                 + (LK && LK.contractWagesChargeToProduct ? row('Wages Amount', null, null, null, wages) : '');
        var right = row('Over Heads', null, null, null, oh) + row('Packing Material', pmQ, null, pmR, pmA)
                 + (t3 ? row(gain ? 'Gain' : 'Loss', null, null, null, diff, ' style="font-weight:bold;color:' + colr + ';"') : '');
        $id('scSummary').innerHTML =
            '<div><div class="sc-bar">Summery Calculations</div><table class="win-grid">' + head() + '<tbody>' + left + '</tbody></table></div>'
          + '<div><div class="sc-bar">&nbsp;</div><table class="win-grid">' + head() + '<tbody>' + right + '</tbody></table></div>';
    }

    // ------------------------------------------------------------------------------ history

    var HISTORY_ROWS = [];
    var historySeq = 0;

    /** btnshowHistory_Click:5215 -> BindHistoryGrid. Print (position 0) and Edit (1) frozen. */
    function loadHistory() {
        return busy('btnHistoryRefresh', function () {
            var q = [];
            function add(name, v) { if (v !== '' && v !== null && v !== undefined) q.push(name + '=' + encodeURIComponent(v)); }
            var mode = (document.querySelector('input[name="scDateMode"]:checked') || {}).value || 'doc';
            var names = { doc: ['fromDate', 'toDate'], entry: ['entryFromDate', 'entryToDate'],
                          modify: ['modifyFromDate', 'modifyToDate'], approved: ['approvedDateFrom', 'approvedDateTo'] }[mode];
            if ($id('fFromOn').checked) add(names[0], val('fFromDate'));
            if ($id('fToOn').checked) add(names[1], val('fToDate'));
            add('docNoFrom', val('fDocNoFrom'));
            add('docNoTo', val('fDocNoTo'));
            return getJson(api + '/history' + (q.length ? '?' + q.join('&') : '')).then(function (d) {
                HISTORY_ROWS = (d && d.rows) || [];
                renderHistory();
                historySubGrids(null);
                $id('lblHistoryCount').textContent = HISTORY_ROWS.length + ' record(s)' + (d && d.canViewAllRecords ? '' : ' - your own records only');
                /* the grid activates its first row, which fills the three detail grids */
                if (HISTORY_ROWS.length) historyRowSelected(0);
            }).catch(function (e) { box(e.message); });
        });
    }

    var HCOLS = [['DocNo','DocNo'],['DocDate','DocDate'],['ProductionNo','ProductionNo'],['Remarks','Remarks'],['EntryDate','EntryDate'],
        ['EntryUser','EntryUser'],['ModifyDate','ModifyDate'],['ModifyUser','ModifyUser'],['ApprovedDate','PostDate'],['ApprovedUser','ApprovedUser']];
    function renderHistory() {
        $id('historyHead').innerHTML = '<th style="position:sticky;left:0;z-index:6;">Print</th><th style="position:sticky;left:52px;z-index:6;">Edit</th>'
            + HCOLS.map(function (c) { return '<th>' + esc(c[0]) + '</th>'; }).join('');
        $id('gridHistory').innerHTML = HISTORY_ROWS.length ? HISTORY_ROWS.map(function (r, i) {
            return '<tr tabindex="0" data-i="' + i + '">'
                 + '<td style="position:sticky;left:0;background:#fff;"><button type="button" class="sc-cellbtn" data-act="print">Print</button></td>'
                 + '<td style="position:sticky;left:52px;background:#fff;"><button type="button" class="sc-cellbtn" data-act="edit">Edit</button></td>'
                 + HCOLS.map(function (c) {
                       var v = col(r, c[1]);
                       if (c[0] === 'DocNo') return '<td><a class="win-doc-link" data-act="edit">' + esc(v) + '</a></td>';
                       if (c[0] === 'DocDate') v = fmtDate(v, false); else if (/Date$/.test(c[0])) v = fmtDate(v, true);
                       return '<td>' + esc(v) + '</td>';
                   }).join('') + '</tr>';
        }).join('') : '<tr><td colspan="' + (HCOLS.length + 2) + '">No records</td></tr>';
        K.filterRow($id('tblHistory'));
    }

    /** grdHistoryMain_SelectionChanged:5435 -> BindDetailsByHeaderId. A reply for a row that is no
     *  longer selected is dropped, so a late answer cannot overwrite a newer one. */
    function historyRowSelected(i) {
        var r = HISTORY_ROWS[i];
        if (!r) return;
        $id('gridHistory').querySelectorAll('tr[data-i]').forEach(function (tr) { tr.classList.toggle('is-current', +tr.getAttribute('data-i') === i); });
        var seq = ++historySeq;
        return getJson(api + '/' + int(col(r, 'Id'))).then(function (d) {
            if (seq !== historySeq) return;
            /* Only a document WITH detail rows refills the grids (:5456). */
            if (d && d.details && d.details.length) historySubGrids(d);
        }).catch(function (e) { box(e.message); });
    }

    function historySubGrids(d) {
        var io = $id('gridHistoryIO'), pm = $id('gridHistoryPM'), oh = $id('gridHistoryOH');
        if (!d) { io.innerHTML = ''; pm.innerHTML = ''; oh.innerHTML = ''; return; }
        function table(spec, rows) {
            var foot = '<tr>' + spec.map(function (c, k) {
                if (!c[3]) return '<td>' + (k === 0 ? 'Total' : '') + '</td>';
                var t = rows.reduce(function (a, r) { return a + num(col(r, c[1])); }, 0);
                return '<td class="num">' + esc(fmtKind(c[2], t)) + '</td>';
            }).join('') + '</tr>';
            return '<table class="win-grid"><thead><tr>' + spec.map(function (c) { return '<th' + (c[2] !== 't' ? ' class="num"' : '') + '>' + esc(c[0]) + '</th>'; }).join('')
                 + '</tr></thead><tbody>' + rows.map(function (r) {
                       return '<tr>' + spec.map(function (c) {
                           var v = c[1] === 'ChargeTo' ? chargeTo(col(r, 'ChargeTo')) : fmtKind(c[2], col(r, c[1]));
                           return '<td' + (c[2] !== 't' ? ' class="num"' : '') + '>' + esc(v) + '</td>';
                       }).join('') + '</tr>';
                   }).join('') + '</tbody>' + (rows.length ? '<tfoot>' + foot + '</tfoot>' : '') + '</table>';
        }
        io.innerHTML = table([['EntryType','EntryType','t'],['WareHouse','WareHouseName','t'],['CropBatch','CropBatch','t'],['ItemName','ItemName','t'],
            ['JobLot','JobLotDescription','t'],['PackUom','UomCode','t'],['PackType','PackTypeDesc','t'],['Qty','Qty','q',1],['Weight','Weight','q',1],
            ['Rate','Rate','r'],['RateUom','RateUom','t'],['PMAmount','PackingMaterialAmount','a',1],['ItemPMAmount','ItemPmCost','a',1],
            ['ExpAmount','ExpenseAmount','a',1],['ItemExpAmount','ItemOhCost','a',1],['Amount','Amount','a',1],['Remarks','Remarks','t']], d.details || []);
        pm.innerHTML = table([['WarehouseName','WarehouseName','t'],['Item','ItemName','t'],['BrandItem','BrandName','t'],['BrandUom','BrandUom','t'],
            ['ItemQTY','ItemQty','q',1],['Rate','ItemRate','r'],['Amount','ItemAmount','a',1],['ChargeTo','ChargeTo','t'],['Schedule / Invoice No','ContractScheduleNo','t']], d.packings || []);
        oh.innerHTML = table([['ChartofAccount','AccountTitle','t'],['BrandItem','BrandName','t'],['BrandUom','BrandUom','t'],
            ['ExpAmount','ExpAmount','a',1],['LedgerRemarks','LedgerRemarks','t']], d.expenses || []);
    }

    /** btnNewHistory_Click:5191. */
    function newHistory() {
        setVal('fDocNoFrom', ''); setVal('fDocNoTo', '');
        var f = new Date(); f.setDate(f.getDate() - 3);
        setVal('fFromDate', dateOnly(f.getFullYear() + '-' + String(f.getMonth() + 1).padStart(2, '0') + '-' + String(f.getDate()).padStart(2, '0')));
        setVal('fToDate', today());
        /* DateTimePicker.Value's setter also sets Checked = true (ShowCheckBox pickers start checked), so both
           date filters are ON after New - an unchecked box meant no date filter and returned every record. */
        $id('fFromOn').checked = true; $id('fToOn').checked = true;
        var r = document.querySelector('input[name="scDateMode"][value="doc"]'); if (r) r.checked = true;
        HISTORY_ROWS = []; $id('historyHead').innerHTML = ''; $id('gridHistory').innerHTML = ''; historySubGrids(null);
        $id('lblHistoryCount').textContent = '';
    }

    /** dd-MMM-yyyy, and dd-MMM-yyyy hh:mm tt for the time-stamped columns. */
    function fmtDate(v, withTime) {
        if (!v) return '';
        var d = new Date(String(v).replace(' ', 'T'));
        if (isNaN(d.getTime())) return String(v);
        var m = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'][d.getMonth()];
        var s = String(d.getDate()).padStart(2, '0') + '-' + m + '-' + d.getFullYear();
        if (!withTime) return s;
        var h = d.getHours(), ap = h >= 12 ? 'PM' : 'AM';
        h = h % 12; if (h === 0) h = 12;
        return s + ' ' + String(h).padStart(2, '0') + ':' + String(d.getMinutes()).padStart(2, '0') + ' ' + ap;
    }

    // -------------------------------------------------------------------------------- views

    /** tabControl1 - switching only moves focus (:5172); the history query runs on Show. */
    function showView(which) {
        var form = which !== 'history';
        $id('mainViewForm').style.display = form ? '' : 'none';
        $id('mainViewHistory').style.display = form ? 'none' : '';
        $id('tabForm').classList.toggle('is-active', form);
        $id('tabHistory').classList.toggle('is-active', !form);
        (form ? $id('txtDocdate') : $id('fFromDate')).focus();
    }
    function tab2(paneId) {
        ['paneInput', 'paneOutput', 'panePm', 'paneOh', 'paneWages'].forEach(function (p) { $id(p).classList.toggle('is-active', p === paneId); });
        $id('scTabs2').querySelectorAll('.sc-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-pane') === paneId); });
    }
    function toggleFullscreen(boxId) { var el = $id(boxId); if (el) el.classList.toggle('is-fullscreen'); }

    /** btnnew_Click -> RefreshForm:1630: the doc date is NOT touched. */
    function btnNew() {
        return busy('btnNew', function () {
            RECID = 0;
            ['txtdocnumber', 'txtProductionNo', 'txtRemarks', 'txtLoadDocNo'].forEach(function (id) { setVal(id, ''); });
            setSel('CmbProductionDepartment', 0);
            resetDetail();
            INPUT_ROWS = []; OUTPUT_ROWS = []; PM_ROWS = []; OH_ROWS = []; WAGES_ROWS = [];
            $id('chkFumigationOnHold').checked = false;
            conversionTypeChanged();
            renderAll();
            show('btnSave', true); show('btnUpdate', false);
            $id('lblDocNo').textContent = '';
            say('Ready');
            refreshCombos();
            return getJson(api + '/next-code').then(function (d) {
                var code = d && d.docSrNo ? d.docSrNo : 0;
                if (code > 0) setVal('txtdocnumber', code);
            }).catch(function (e) { box(e.message); });
        });
    }

    /* btnRefresh_Click:1745 re-reads the lists; it does not clear the document. */
    function btnRefresh() {
        return busy('btnRefresh', function () {
            return loadLookups(false).then(applyFlags).catch(function (e) { box(e.message); });
        });
    }

    /* Save / Update do not POST. */
    function btnSave() {
        box('Save is not enabled for Stock Conversion yet.\n\n'
          + 'The desktop Save also writes inventory transactions, an accounting voucher and '
          + 'contractor wages bills in the same transaction; until those are ported, saving here '
          + 'would create a conversion with no stock movement and no voucher.');
    }

    /** btnGenerate_Click - handleAverageRateCalculation over the rows on screen. */
    function generate() {
        handleAverageRateCalculation(INPUT_ROWS.concat(OUTPUT_ROWS));
        renderAll();
        say('Rates regenerated on screen.');
    }

    /** Print_Click:6195 -> StockConversionSummary_605(RecId); 0 -> "No Record Found For Display". */
    function print() {
        if (RECID === 0) { box('No Record Found For Display'); return; }
        return window.CrystalPrint.open('sc-605', { id: RECID }, 'btnPrint');
    }
    function printId(id) { return window.CrystalPrint.open('sc-605', { id: id }); }

    /** btnVoucher_Click:6207 -> VoucherReport_118(VoucherHeadIdGet(RecId, 66)). */
    function voucher() {
        var win = window.CrystalPrint.reserve();
        return busy('btnVoucher', function () {
            return getJson(api + '/voucher-head?id=' + RECID).then(function (d) {
                var vh = d ? int(d.voucherHeadId) : 0;
                if (vh === 0) { window.CrystalPrint.release(win); box('VoucherId Not Found'); return; }
                return window.CrystalPrint.open('acc-118', { id: vh, documentTypeId: 0 }, null, win);
            }).catch(function (e) { window.CrystalPrint.release(win); box(e.message); });
        });
    }

    function shortcuts() {
        K.shortcuts([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'],
                     ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print Slip'], ['Alt+1', 'For Print Slip'],
                     ['Ctrl+F5', 'For Focus on Production No'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'],
                     ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowRight', 'For Change Focus from one Grid To another Grid'],
                     ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    function applyFlags() {
        var r = (LK && LK.rights) || {};
        $id('btnSave').disabled = !r.save;
        $id('btnUpdate').disabled = !r.update;
        $id('btnPrint').disabled = !r.print;
        conversionTypeChanged();
    }

    function onForm() { return $id('mainViewForm').style.display !== 'none'; }
    function focusFirstRow(bodyId) { var t = $id(bodyId).querySelector('tr'); if (t) { t.tabIndex = 0; t.focus(); } }

    function boot() {
        setVal('txtDocdate', today());
        newHistory();

        /* KeyDown:6493. */
        K.enterToTab();
        K.keys({
            'ctrl+t': function () { showView(onForm() ? 'history' : 'form'); },
            'ctrl+e': K.close, 'esc': K.close, 'ctrl+alt': shortcuts,
            'ctrl+s': function () { if (onForm()) { var b = $id('btnSave'); if (!b.classList.contains('is-hidden') && !b.disabled) btnSave(); } else loadHistory(); },
            'ctrl+u': function () { var b = $id('btnUpdate'); if (onForm() && !b.classList.contains('is-hidden') && !b.disabled) btnSave(); },
            'ctrl+n': function () { if (onForm()) btnNew(); },
            'ctrl+r': function () { if (onForm()) btnRefresh(); },
            'ctrl+p': function () { if (onForm()) print(); },
            'alt+1': function () { if (onForm()) print(); },
            'ctrl+f5': function () { (onForm() ? $id('txtDocdate') : $id('fFromDate')).focus(); },
            'ctrl+arrowup': function () { (onForm() ? $id('txtProductionNo') : $id('fFromDate')).focus(); },
            'ctrl+arrowdown': function () { if (onForm()) { tab2('paneInput'); focusFirstRow('gridInput'); } else focusFirstRow('gridHistory'); },
            'ctrl+arrowright': function () {
                var a = document.activeElement;
                if (!onForm()) return;
                if (a && a.closest && a.closest('#gridInput')) { tab2('paneOutput'); focusFirstRow('gridOutput'); }
                else if (a && a.closest && a.closest('#gridOutput')) { tab2('panePm'); focusFirstRow('gridPacking'); }
                else { tab2('paneInput'); focusFirstRow('gridInput'); }
            }
        });

        /* Detail calculations - txtQty / cmbUOM / txtRate / cmbRateUom / txtUnitWeight TextChanged. */
        $id('txtQty').addEventListener('input', function () { calculateWeight(); amountCalculation(); });
        $id('cmbUOM').addEventListener('change', function () { calculateWeight(); amountCalculation(); });
        $id('txtRate').addEventListener('input', amountCalculation);
        $id('cmbRateUom').addEventListener('change', amountCalculation);
        $id('txtUnitWeight').addEventListener('input', amountCalculation);
        /* txtRate_KeyPress:6336 - digits and one decimal point. */
        ['txtRate', 'txtQty', 'txtUnitWeight', 'txtMoisture'].forEach(function (id) {
            $id(id).addEventListener('keypress', function (e) {
                if (e.key.length !== 1 || e.ctrlKey) return;
                if (/[0-9]/.test(e.key) || (e.key === '.' && this.value.indexOf('.') < 0)) return;
                e.preventDefault();
            });
        });

        $id('gridOutput').addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) editRow(+tr.getAttribute('data-i')); });
        var hb = $id('gridHistory');
        hb.addEventListener('click', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            var i = +tr.getAttribute('data-i'), act = e.target.closest('[data-act]');
            var r = HISTORY_ROWS[i];
            if (act && act.getAttribute('data-act') === 'print') { printId(int(col(r, 'Id'))); return; }
            if (act && act.getAttribute('data-act') === 'edit') { load(int(col(r, 'Id'))).catch(function (x) { box(x.message); }); return; }
            historyRowSelected(i);
        });
        hb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('button')) load(int(col(HISTORY_ROWS[+tr.getAttribute('data-i')], 'Id'))); });
        /* grdHistoryMain_KeyDown:6666 - Ctrl+Enter opens, Ctrl+P prints, Ctrl+Space the button. */
        hb.addEventListener('keydown', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr || !e.ctrlKey) return;
            var r = HISTORY_ROWS[+tr.getAttribute('data-i')];
            if (e.key === 'Enter' || e.key === ' ') { e.preventDefault(); e.stopPropagation(); load(int(col(r, 'Id'))); }
            else if (e.key === 'p' || e.key === 'P') { e.preventDefault(); e.stopPropagation(); printId(int(col(r, 'Id'))); }
        });

        Promise.all([
            loadEntryTypes().catch(function (e) { box('Entry Type: ' + e.message); }),
            loadLookups(true).catch(function (e) { box(e.message); })
        ]).then(function () { applyFlags(); return btnNew(); })
          .catch(function (e) { box(e.message); });
    }

    window.StockConversion = {
        load: load, loadByDocNo: loadByDocNo, wagesTab: wagesTab, loadHistory: loadHistory, newHistory: newHistory,
        showView: showView, tab2: tab2, toggleFullscreen: toggleFullscreen,
        btnNew: btnNew, btnRefresh: btnRefresh, btnSave: btnSave, generate: generate,
        print: print, voucher: voucher, shortcuts: shortcuts,
        conversionTypeChanged: conversionTypeChanged, parentCategoryChanged: parentCategoryChanged,
        entryTypeChanged: entryTypeChanged, itemChanged: itemChanged,
        applyJobLotToGrid: applyJobLotToGrid
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
    else boot();
}());
