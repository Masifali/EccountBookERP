/* ============================================================================================
 * Stock Conversion - invfrmStockConversionProduction.cs, DocTypeId 66.
 *
 * The whole form: Detail entry (Add / Update / Cancel), the Input, Output, Packing Material,
 * Overhead and Contractor Wages grids with their cell events, the Issuance loader
 * (LoadavailableTransactionsForIssuance, a modal), the Release Fumigation Stock loader
 * (LoadavailableTransactionsForStockReleaseFromFumigation, a modal), the Load OutPut loader
 * (frmLoadStockShortFallForSales, a modal), Wages Schedule / Wages Exempt (new windows, as the
 * desktop .Show()s them), Save / Update (Insert():4365) / Delete, the
 * history, the 605 print and the 118 voucher. Line numbers are src280/invfrmStockConversionProduction.cs.
 *
 * Every grid row keeps the desktop DataTable's own column names (table, tableByProduct,
 * dtPackingMaterial, dtExpense, dtdetail, dtStiching), so each handler reads like the C# it ports.
 * Where the desktop computes from a cell's formatted .Text rather than its .Value, the same
 * formatted text is used here (txt()), so the rounding matches.
 * ============================================================================================ */
(function () {
    'use strict';

    /* frmwagesBillHeader as a modal (ShowDialog): /production/wages-bill in an overlay iframe; the
       caller continues (reset) only after the dialog closes, as the desktop blocks until then. */
    function p280OpenWages(refDocTypeId, refDocId, grossWeightTotal, onClosed) {
        var ov = document.createElement('div');
        ov.style.cssText = 'position:fixed;inset:0;z-index:9800;background:rgba(0,0,0,.35);';
        var fr = document.createElement('iframe');
        fr.src = '/production/wages-bill?' + new URLSearchParams({ refDocTypeId: refDocTypeId, refDocId: refDocId,
                                                                  grossWeightTotal: grossWeightTotal || 0 });
        fr.style.cssText = 'position:absolute;inset:12px;width:calc(100% - 24px);height:calc(100% - 24px);border:1px solid #555;background:#fff;';
        ov.appendChild(fr); document.body.appendChild(ov);
        window.P280WagesClosed = function (r) { ov.remove(); window.P280WagesClosed = null; if (onClosed) onClosed(r); };
    }


    var api = '/api/production/stock-conversion';
    var K = window.ReportKit;

    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function say(m) { var e = $id('lblFormStatus'); if (e) e.textContent = m; }
    function box(m) { window.alert(m); }
    function ask(m) { return window.confirm(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function col(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return '';
    }
    function dateOnly(v) {
        if (!v) return '';
        var m = String(v).match(/^(\d{4})-(\d{2})-(\d{2})/);
        if (m) return m[1] + '-' + m[2] + '-' + m[3];
        var d = new Date(String(v));
        if (isNaN(d.getTime())) return '';
        return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0');
    }
    function today() { return dateOnly(new Date().getFullYear() + '-' + String(new Date().getMonth() + 1).padStart(2, '0') + '-' + String(new Date().getDate()).padStart(2, '0')); }
    function nowIso() {
        var d = new Date();
        return today() + 'T' + String(d.getHours()).padStart(2, '0') + ':' + String(d.getMinutes()).padStart(2, '0') + ':' + String(d.getSeconds()).padStart(2, '0');
    }

    // ------------------------------------------------------------------ .NET conversions

    /** Conversion.ToDouble: Convert.ToDouble, 0 for null/""/unparsable/Infinity; NaN stays NaN. */
    function netD(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isNaN(v) ? NaN : (isFinite(v) ? v : 0);
        if (typeof v === 'boolean') return v ? 1 : 0;
        var s = String(v).trim();
        if (s === '') return 0;
        if (s === 'NaN') return NaN;
        var t = s.replace(/,/g, '');
        if (!/^[+-]?(\d+\.?\d*|\.\d+)([eE][+-]?\d+)?$/.test(t)) return 0;
        var n = parseFloat(t);
        return isFinite(n) ? n : 0;
    }
    /** Math.Round(x, d) - MidpointRounding.ToEven. */
    function rnd(x, d) {
        if (!isFinite(x)) return x;
        var m = Math.pow(10, d || 0), y = x * m, f = Math.floor(y), r = y - f;
        var out = Math.abs(r - 0.5) < 1e-9 ? (f % 2 === 0 ? f : f + 1) : Math.round(y);
        return out / m;
    }
    /** Conversion.ToInt: Convert.ToInt32 - a double rounds to even, a string must be a plain integer, else 0. */
    function netI(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isFinite(v) ? rnd(v, 0) : 0;
        if (typeof v === 'boolean') return v ? 1 : 0;
        var s = String(v).trim();
        return /^[+-]?\d+$/.test(s) ? parseInt(s, 10) : 0;
    }
    /** A double through double.ToString() and back (.NET Framework "G", 15 digits). */
    function g15(x) { return (typeof x === 'number' && isFinite(x)) ? Number(x.toPrecision(15)) : x; }
    function csStr(x) {
        if (typeof x !== 'number') return String(x === null || x === undefined ? '' : x);
        if (isNaN(x)) return 'NaN';
        if (!isFinite(x)) return x > 0 ? 'Infinity' : '-Infinity';
        return String(g15(x));
    }
    function truthy(v) { return v === true || v === 1 || v === '1' || String(v).toLowerCase() === 'true'; }

    // --------------------------------------------------------------------------- formats

    function amountDec() { return LK && LK.amountDecimals !== undefined ? netI(LK.amountDecimals) : 2; }
    function rateDec() { return LK && LK.rateDecimals !== undefined ? netI(LK.rateDecimals) : 2; }
    function F(kind, v) {
        if (v === null || v === undefined || v === '') return '';
        if (typeof v === 'number' && isNaN(v)) return 'NaN';
        switch (kind) {
            case 'q': return K.num(v, 3);        // "#,##0.###"
            case 'q4': return K.num(v, 4);       // "#,##0.####"
            case 'q2': return K.num(v, 2);       // "#,##0.##"
            case 'r': return K.fixed(v, rateDec());   // DecimalRateFormate
            case 'a': return K.fixed(v, amountDec()); // stringFormatsingle
            case 'raw': return typeof v === 'number' ? csStr(v) : String(v);
            default: return String(v);
        }
    }

    // ------------------------------------------------------------------------ requests

    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) {
            if (b.disabled || b.classList.contains('is-busy')) return;
            b.disabled = true;
            b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); applyRightsToButtons(); } };
        var p;
        try { p = fn(); } catch (e) { done(); throw e; }
        if (p && typeof p.then === 'function') p.then(done, done); else done();
        return p;
    }
    function parse(r) {
        return r.text().then(function (t) {
            var body = null;
            try { body = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
            if (!r.ok) throw new Error((body && body.message) || ('Request failed (' + r.status + ')'));
            return body;
        });
    }
    function getJson(url) {
        return fetch(url, { headers: { 'Accept': 'application/json' }, credentials: 'same-origin' }).then(parse);
    }
    function postJson(url, body) {
        var h = { 'Accept': 'application/json', 'Content-Type': 'application/json' };
        var t = document.querySelector('meta[name="_csrf"]'), n = document.querySelector('meta[name="_csrf_header"]');
        if (t && n && t.getAttribute('content') && n.getAttribute('content')) h[n.getAttribute('content')] = t.getAttribute('content');
        return fetch(url, { method: 'POST', headers: h, credentials: 'same-origin', body: JSON.stringify(body) }).then(parse);
    }
    function qs(o) {
        var a = [];
        for (var k in o) if (o[k] !== undefined && o[k] !== null && o[k] !== '') a.push(k + '=' + encodeURIComponent(o[k]));
        return a.length ? '?' + a.join('&') : '';
    }

    // ------------------------------------------------------------------------------ state

    var LK = null;          // /lookups (the thirteen combos, rights, formats)
    var ES = {};            // /edit-setup (Load's switches and the grid value lists)
    var PM_ITEMS = [];      // GridPmDropdownBind
    var SCHEDULES = [];     // dtScheduleData
    var BRAND_ITEMS = [];   // dtBrandItem   (btnGenerateItemsAndUom)
    var BRAND_UOMS = [];    // dtBrandUom
    var UOM_ROWS = [];      // the Detail item's UOM schedule
    var ENTRY_TYPES = [];

    var RECID = 0;
    var updateDetailIndex = -1;
    var InputDetailRowsRemoveIds = '';
    var DetailEditMood = false;
    var CheckExpenseAmountTotal = false;
    var checkPackingMaterialAmountTotal = false;
    var docDateTag = null;          // txtDocdate.Tag
    var saveVisible = true;         // btnsave.Visible (Update visible = !saveVisible)
    /* gridsettings(FromFumigation: true):2169 - after a fumigation load grdInput is rebuilt WITHOUT its
       "Delete" (X) button column; the next plain gridsettings() (Load, AddInGrid, the Issuance loader,
       ReadById) puts it back. New/Refresh do not re-run gridsettings, so it stays off until then. */
    var inputNoDelete = false;

    var INPUT = [];     // table
    var OUTPUT = [];    // tableByProduct
    var PMR = [];       // dtPackingMaterial
    var OHR = [];       // dtExpense
    var WG = [];        // dtdetail   (Regular Wages)
    var ST = [];        // dtStiching (Other Wages)
    var CUR = { in: -1, out: -1, pm: -1, oh: -1, wg: -1, st: -1 };   // GridEX.CurrentRow
    var SUM = {};       // the summary text boxes (their .Text)

    function convType() { return netI(val('CmbConversionType')); }
    function selText(id) { var s = $id(id); return (!s || s.selectedIndex < 0 || s.value === '0' || s.value === '') ? '' : s.options[s.selectedIndex].textContent; }
    function hasSel(id) { var s = $id(id); return !!s && s.value !== '0' && s.value !== ''; }
    function refreshCombos() { if (window.DesktopCombo) window.DesktopCombo.refresh(); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function isShown(id) { var e = $id(id); return !!e && !e.classList.contains('is-hidden'); }

    function fill(id, rows, valueKey, textKey, keep) {
        var sel = $id(id);
        if (!sel) return;
        var html = '<option value="0"></option>', found = false;
        (rows || []).forEach(function (r) {
            var v = String(col(r, valueKey));
            if (keep !== undefined && keep !== null && String(keep) === v) found = true;
            html += '<option value="' + esc(v) + '">' + esc(col(r, textKey)) + '</option>';
        });
        sel.innerHTML = html;
        sel.value = found ? String(keep) : '0';
    }
    function selectFirst(id) { var s = $id(id); if (s && s.options.length > 1) s.selectedIndex = 1; }
    function selectByText(id, text) {
        var s = $id(id);
        if (!s) return false;
        var t = String(text === null || text === undefined ? '' : text);
        for (var i = 0; i < s.options.length; i++) if (s.options[i].textContent === t && s.options[i].value !== '0') { s.selectedIndex = i; return true; }
        s.value = '0';
        return false;
    }
    function setSel(id, v) {
        var s = $id(id);
        if (!s) return;
        var x = String(v === null || v === undefined ? '' : v), ok = false;
        for (var i = 0; i < s.options.length; i++) if (s.options[i].value === x) { ok = true; break; }
        s.value = ok ? x : '0';
    }

    // ============================================================================ lookups

    function loadEntryTypes() {
        return getJson(api + '/entry-types').then(function (rows) {
            ENTRY_TYPES = rows || [];
            /* CmbEntryTypeFill:833 - DDL.BindDDL(dt, cmbEntryType, "Id", "EntryType"). */
            fill('cmbEntryType', rows, 'Id', 'EntryType', netI(val('cmbEntryType')) || null);
            refreshCombos();
        });
    }

    /** Load:780-792 and btnRefresh_Click:1853. */
    function loadLookups(firstTime) {
        return getJson(api + '/lookups').then(function (d) {
            LK = d || {};
            if (firstTime) {
                fill('cmbParentCategory', LK.parentCategories, 'Id', 'InvParentCateDescription');
                selectFirst('cmbParentCategory');
            }
            fill('CmbProductionDepartment', LK.productionDepartments, 'Id', 'WareHouseName', netI(val('CmbProductionDepartment')) || null);
            /* BindAndRetainSelection keeps the current value, then Rows[0].Activate(). */
            var keepType = netI(val('CmbConversionType'));
            fill('CmbConversionType', LK.conversionTypes, 'Id', 'type', keepType || null);
            if (!keepType) selectFirst('CmbConversionType');
            fill('cmbGodown', LK.warehouses, 'Id', 'WareHouseName');
            bindItems();
            fill('cmbLot', LK.jobLots, 'Id', 'JobLotDescription');
            fill('CmbJobLotForGrid', LK.jobLots, 'Id', 'JobLotDescription');
            fill('CmbCropyr', LK.cropYears, 'Id', 'CropYear');
            fill('cmbBagType', LK.packingTypes, 'Id', 'PackTypeDesc');
            if (firstTime) fill('cmbMoistureSlab', LK.moistureSlabs, 'Id', 'MoistureSlabDescription');
            fill('CmbDifferenceAccount', LK.differenceAccounts, 'Id', 'AccountTitle', netI(val('CmbDifferenceAccount')) || null);
            refreshCombos();
        });
    }
    function loadEditSetup() {
        return getJson(api + '/edit-setup').then(function (d) { ES = d || {}; });
    }
    /** GridPmDropdownBind:2915 - "14,17" for Conversion Type 5, else "14". */
    function loadPmItems() {
        return getJson(api + '/pm-items?conversionTypeId=' + convType()).then(function (r) { PM_ITEMS = r || []; renderPm(); })
            .catch(function (e) { box('Exception from PM dropdown bind: ' + e.message); });
    }
    /** ScheduleNoDbCall:2896 - with the RecId of the moment. */
    function loadSchedules() {
        return getJson(api + '/schedules?recId=' + RECID).then(function (r) { SCHEDULES = r || []; renderPm(); });
    }

    function bindItems() {
        var parent = netI(val('cmbParentCategory'));
        var all = (LK && LK.items) || [];
        var rows = parent > 0 ? all.filter(function (r) { return netI(col(r, 'InventoryParentCategoriesId')) === parent; }) : all;
        fill('cmbItem', rows, 'Id', 'ItemName');
    }

    /** bindRateUomAndItemPackUom:1098 - both pickers keep their TEXT when the new schedule has it. */
    function bindUoms() {
        var itemId = netI(val('cmbItem'));
        var packText = selText('cmbUOM'), rateText = selText('cmbRateUom');
        if (itemId <= 0) {
            UOM_ROWS = [];
            fill('cmbUOM', [], 'Id', 'UOMCode'); fill('cmbRateUom', [], 'Id', 'UOMCode');
            refreshCombos();
            return Promise.resolve();
        }
        return getJson(api + '/uoms?itemId=' + itemId).then(function (rows) {
            UOM_ROWS = rows || [];
            fill('cmbUOM', rows, 'Id', 'UOMCode'); fill('cmbRateUom', rows, 'Id', 'UOMCode');
            if (UOM_ROWS.length) {
                if (packText) selectByText('cmbUOM', packText);
                if (rateText) selectByText('cmbRateUom', rateText);
            }
            refreshCombos();
            calculateWeight(); amountCalculation();
        });
    }
    /** SelectedRow.Cells[2] - the schedule's Equivalent. */
    function uomEquivalent(selectId) {
        var id = String(val(selectId));
        for (var i = 0; i < UOM_ROWS.length; i++) if (String(col(UOM_ROWS[i], 'Id')) === id) return netD(col(UOM_ROWS[i], 'Equivalent'));
        return 0;
    }

    // ===================================================================== Detail portion

    /** CalculateWeight:5863. */
    function calculateWeight() {
        if (!hasSel('cmbUOM')) { setVal('txtUnitWeight', '0'); return; }
        var eq = uomEquivalent('cmbUOM'), q = netD(val('txtQty'));
        setVal('txtUnitWeight', (eq > 0 && q > 0) ? K.num(eq * q, 3) : '0');
    }
    /** AmountCalculation:5821. */
    function amountCalculation() {
        if (!hasSel('cmbRateUom')) { setVal('txtAmount', '0'); return; }
        var eq = uomEquivalent('cmbRateUom'), r = netD(val('txtRate')), w = netD(val('txtUnitWeight'));
        setVal('txtAmount', (r > 0 && eq > 0 && w > 0) ? F('a', w / eq * r) : '0');
    }

    /** GetAvgRate:1162 - AvgRateOnlyForCGS(item, docDate, 66, RecId, lot, CmbCropyr.Value, null, godown). */
    function getAvgRate() {
        return getJson(api + '/avg-rate' + qs({ itemId: netI(val('cmbItem')), docDate: val('txtDocdate'), recId: RECID,
            jobLotId: netI(val('cmbLot')), cropYearId: netI(val('CmbCropyr')), warehouseId: netI(val('cmbGodown')) }))
            .then(function (d) {
                var rate = netD(d && d.rate);
                if (rate > 0) {
                    setVal('txtAverageRate', csStr(rate));
                    if (!DetailEditMood) setVal('txtRate', csStr(rate));
                } else {
                    setVal('txtAverageRate', '0');
                    setVal('txtRate', '0');
                }
                amountCalculation();
            }).catch(function (e) { box(e.message); });
    }
    /** GetCurrentItemStockByWarehouseByJobLotAndCropYear:1453 - ToDate = DateTime.Now, CropYear = the combo text. */
    function getCurrentItemStock() {
        return getJson(api + '/stock-filter' + qs({ activity: 'GetCurrentItemStockByWarehouseByJobLotAndCropYear',
            itemCategoryId: netI(val('cmbParentCategory')), warehouseId: netI(val('cmbGodown')), itemId: netI(val('cmbItem')),
            jobLotId: netI(val('cmbLot')), docDateTo: nowIso(), cropYear: selText('CmbCropyr') }))
            .then(function (rows) {
                if (rows && rows.length) setVal('txtStock', csStr(rnd(netD(col(rows[0], 'AvailableItemStock')), 2)));
                else setVal('txtStock', '');
            }).catch(function (e) { box(e.message); });
    }

    /** cmbItem_Leave:1146. */
    function itemChanged() {
        var p = selText('cmbEntryType') === 'Issue' ? getAvgRate() : Promise.resolve();
        p.then(bindUoms).catch(function (e) { box(e.message); });
    }
    function cropChanged() { if (selText('cmbEntryType') === 'Issue') getAvgRate(); }        // CmbCropyr_Leave:6301
    function lotChanged() { if (selText('cmbEntryType') === 'Issue') { getCurrentItemStock(); getAvgRate(); } }   // cmbLot_Leave:6309
    function godownChanged() { if (selText('cmbEntryType') === 'Issue') getAvgRate(); }      // cmbGodown_Leave:6318
    function parentCategoryChanged() { bindItems(); refreshCombos(); }                         // cmbParentCategory_Leave:1614

    /** cmbEntryType_Leave:1518. */
    function entryTypeChanged() {
        if (!selText('cmbParentCategory')) { box('Please Select Parent Category First!'); $id('cmbParentCategory').focus(); return; }
        var t = selText('cmbEntryType');
        if (!t) { box('Please Select Entry Type First!'); $id('cmbEntryType').focus(); return; }
        var rate = $id('txtRate');
        rate.readOnly = (t === 'Recovery Head Rice');
        rate.tabIndex = rate.readOnly ? -1 : 0;
        if (t !== 'Issue') {
            bindItems();
            fill('cmbLot', (LK && LK.jobLots) || [], 'Id', 'JobLotDescription');
            fill('CmbJobLotForGrid', (LK && LK.jobLots) || [], 'Id', 'JobLotDescription');
            setVal('txtAverageRate', '0');
        }
        refreshCombos();
        calculateWeight(); amountCalculation();
    }

    /** CmbConversionType_Leave:1559. */
    function conversionTypeChanged() {
        var t = convType(), t5 = t === 5, t3 = t === 3;
        $id('wrapDifferenceAccount').style.display = t3 ? '' : 'none';
        $id('txtRemarks').style.height = t3 ? '26px' : '54px';
        show('btnLoadOutPut', !t3 && !!ES.saleMinusAllowedAgainstFifo);
        show('btnIssuanceLoad', !t5);
        show('btnStockReleaseFromFumigation', t5);
        if (t5) {
            loadPmItems();
            show('tabWages', false); if ($id('paneWages').classList.contains('is-active')) tab2('paneInput');
            show('btnWagesSchedule', false); show('btnWagesExempt', false);
            show('wrapFumigation', false);
            renderSummary();
            return;
        }
        var w = !!ES.contractWagesChargeToProduct;
        show('btnWagesSchedule', w); show('btnWagesExempt', w); show('tabWages', w);
        if (!w && $id('paneWages').classList.contains('is-active')) tab2('paneInput');
        show('wrapFumigation', !!ES.issuanceByLoader);
        renderSummary();
    }

    /** FormValidationOfDetailPortion:1649. */
    function formValidationOfDetailPortion() {
        function need(ok, msg, id) { if (!ok) { box(msg); if (id) $id(id).focus(); } return ok; }
        if (!need(convType() !== 0, 'Conversion Type Field Required', 'CmbConversionType')) return false;
        if (!need(hasSel('cmbEntryType'), 'Entry Type Field Required', 'cmbEntryType')) return false;
        if (!need(hasSel('cmbGodown'), 'Ware house Field Required', 'cmbGodown')) return false;
        if (!need(hasSel('CmbCropyr'), 'Crop Year Field Required', 'CmbCropyr')) return false;
        if (!need(hasSel('cmbItem'), 'Item Field Field Required', 'cmbItem')) return false;
        if (!need(hasSel('cmbLot'), 'Lot Field Required', 'cmbLot')) return false;
        if (!need(hasSel('cmbUOM'), 'UOM Field Required', 'cmbUOM')) return false;
        if (!need(hasSel('cmbBagType'), 'Bag Type Field Required', 'cmbBagType')) return false;
        var q = val('txtQty').trim();
        if (!need(!(selText('cmbEntryType') !== 'Issue' && (q === '' || q === '0')), 'Bag Quantity Field Required', 'txtQty')) return false;
        var w = val('txtUnitWeight').trim();
        if (!need(!(w === '' || w === '0'), 'Unit Weight Field Required', 'txtUnitWeight')) return false;
        if (selText('cmbEntryType') !== 'Recovery Head Rice') {
            if (!need(val('txtRate') !== '', 'Rate  Field Required', 'txtRate')) return false;
            if (!need(netD(val('txtAmount')) !== 0, 'Amount  Field Required', 'txtAmount')) return false;
        }
        if (!need(hasSel('cmbRateUom'), 'Rate UOM Field Required', 'cmbRateUom')) return false;
        if (!need(val('txtMoisture') !== '', 'Moisture Field Required', 'txtMoisture')) return false;
        return true;
    }
    function nullIfZero(id) { var v = netI(val(id)); return v === 0 ? null : v; }

    /** AddInGrid_Click:1890. */
    function addInGrid() {
        try {
            if (convType() === 5) throw new Error("Can't add manual entry in case of Release stock from fumigation");
            if (!formValidationOfDetailPortion()) return;
            if (selText('cmbEntryType') === 'Issue') {
                if (convType() === 2 && INPUT.length > 0) {
                    var itemId = netI(val('cmbItem'));
                    if (!INPUT.every(function (r) { return netI(r.ItemId) === itemId; })) { box('Please add same Item,another Item Already Exists in Input grid.'); return; }
                }
                if (ES.fifoCgs) { box('You cannot add a row manually when FIFO is on.'); return; }
                var maxLine = 0;
                if (INPUT.length > 0) maxLine = Math.max.apply(null, INPUT.map(function (r) { return netI(r.LineId); }));
                INPUT.push({ Id: 0, RefDocumentTypeId: 0, RefDocNoId: 0, RefDocSubId: 0, EntryType: selText('cmbEntryType'),
                    WareHouseId: netI(val('cmbGodown')), WareHouse: selText('cmbGodown'), CropYear: selText('CmbCropyr'),
                    ItemId: netI(val('cmbItem')), Item: selText('cmbItem'), JobLotId: netI(val('cmbLot')), JobLot: selText('cmbLot'),
                    ItemUOMId: netI(val('cmbUOM')), UOM: selText('cmbUOM'), PackEquivalent: uomEquivalent('cmbUOM'),
                    PackingTypeId: netI(val('cmbBagType')), PackingType: selText('cmbBagType'),
                    BalQty: netD(val('txtQty')), BalWeight: netD(val('txtUnitWeight')), Quantity: netD(val('txtQty')), Weight: netD(val('txtUnitWeight')),
                    Rate: netD(val('txtRate')), RateUOMId: netI(val('cmbRateUom')), RateUOM: selText('cmbRateUom'), RateEquivalent: uomEquivalent('cmbRateUom'),
                    Amount: netD(val('txtAmount')), Moisture: val('txtMoisture').trim(), MoistureSlabId: nullIfZero('cmbMoistureSlab'),
                    Remarks: val('txtdeailRemarks'), LineId: maxLine + 1, labIPmActivityLogId: null, _chk: false });
                inputNoDelete = false;          // gridsettings():1926
                renderInput();
                afterDetail(ES.contractWagesChargeToProduct ? wagesFill('in') : null);
                resetDetail();
                DetailEditMood = false;
            } else {
                OUTPUT.push({ Id: 0, EntryType: selText('cmbEntryType'), WareHouseId: netI(val('cmbGodown')), WareHouse: selText('cmbGodown'),
                    CropYear: selText('CmbCropyr'), ItemId: netI(val('cmbItem')), Item: selText('cmbItem'), JobLotId: netI(val('cmbLot')), JobLot: selText('cmbLot'),
                    ItemUOMId: netI(val('cmbUOM')), UOM: selText('cmbUOM'), PackEquivalent: uomEquivalent('cmbUOM'),
                    PackingTypeId: netI(val('cmbBagType')), PackingType: selText('cmbBagType'), Quantity: netD(val('txtQty')), Weight: netD(val('txtUnitWeight')),
                    Rate: netD(val('txtRate')), RateUOMId: netI(val('cmbRateUom')), RateUOM: selText('cmbRateUom'), RateEquivalent: uomEquivalent('cmbRateUom'),
                    AmountWithoutExpenses: netD(val('txtAmount')), Moisture: val('txtMoisture').trim(), MoistureSlabId: nullIfZero('cmbMoistureSlab'),
                    MoistureSlabDescription: selText('cmbMoistureSlab'), PMAmount: 0, ItemPMAmount: 0, ExpAmount: 0, ItemExpAmount: 0,
                    Amount: netD(val('txtAmount')), Remarks: val('txtdeailRemarks'), IssueWeight: 0, LineId: null, WagesAmount: null, labIPmActivityLogId: null });
                renderOutput();
                afterDetail(ES.contractWagesChargeToProduct ? wagesFill('out') : null);
                resetDetail();
            }
            proportionateOverheadGrid();
            proportionatedPackingMaterialAmountInOutputGrid();
            generateSummaryForUser();
            $id('cmbEntryType').focus();
            lockConversionType();
            renderAll();
        } catch (e) { box(e.message); }
    }
    function afterDetail(p) { if (p && p.then) p.then(renderAll).catch(function (e) { box(e.message); }); }

    /** btnUpdateDetail_Click:1958. */
    function updateDetail() {
        try {
            if (!formValidationOfDetailPortion()) return;
            var i = updateDetailIndex, r;
            if (selText('cmbEntryType') === 'Issue') {
                if (convType() === 2 && INPUT.length > 0) {
                    var itemId = netI(val('cmbItem'));
                    /* All(ItemId == X && RowIndex != updateDetailIndex) - reproduced as written. */
                    if (!INPUT.every(function (x, k) { return netI(x.ItemId) === itemId && k !== i; })) { box('Please add same Item,another Item Already Exists in Input grid.'); return; }
                }
                if (ES.fifoCgs) { box('You cannot add a row manually when FIFO is on.'); return; }
                r = INPUT[i];
                if (!r) throw new Error('There is no row at position ' + i + '.');
                r.EntryType = selText('cmbEntryType'); r.WareHouseId = netI(val('cmbGodown')); r.WareHouse = selText('cmbGodown');
                r.JobLotId = netI(val('cmbLot')); r.JobLot = selText('cmbLot'); r.PackingTypeId = netI(val('cmbBagType')); r.PackingType = selText('cmbBagType');
                r.ItemId = netI(val('cmbItem')); r.Item = selText('cmbItem'); r.ItemUOMId = netI(val('cmbUOM')); r.UOM = selText('cmbUOM');
                r.PackEquivalent = uomEquivalent('cmbUOM'); r.CropYear = selText('CmbCropyr');
                r.Quantity = netD(val('txtQty')); r.Weight = netD(val('txtUnitWeight')); r.Rate = netD(val('txtRate'));
                r.RateUOMId = netI(val('cmbRateUom')); r.RateUOM = selText('cmbRateUom'); r.RateEquivalent = uomEquivalent('cmbRateUom');
                r.Amount = netD(val('txtAmount')); r.Moisture = val('txtMoisture'); r.MoistureSlabId = nullIfZero('cmbMoistureSlab'); r.Remarks = val('txtdeailRemarks');
                detailButtons(false);
                if (ES.contractWagesChargeToProduct) afterDetail(wagesFill('in'));
            } else {
                r = OUTPUT[i];
                if (!r) throw new Error('There is no row at position ' + i + '.');
                r.EntryType = selText('cmbEntryType'); r.WareHouseId = netI(val('cmbGodown')); r.WareHouse = selText('cmbGodown');
                r.JobLotId = netI(val('cmbLot')); r.JobLot = selText('cmbLot'); r.PackingTypeId = netI(val('cmbBagType')); r.PackingType = selText('cmbBagType');
                r.ItemId = netI(val('cmbItem')); r.Item = selText('cmbItem'); r.ItemUOMId = netI(val('cmbUOM')); r.UOM = selText('cmbUOM');
                r.PackEquivalent = uomEquivalent('cmbUOM'); r.CropYear = selText('CmbCropyr');
                r.Quantity = netD(val('txtQty')); r.Weight = netD(val('txtUnitWeight')); r.Rate = netD(val('txtRate'));
                r.RateUOMId = netI(val('cmbRateUom')); r.RateUOM = selText('cmbRateUom'); r.RateEquivalent = uomEquivalent('cmbRateUom');
                r.Amount = netD(val('txtAmount')); r.AmountWithoutExpenses = netD(val('txtAmount')); r.Remarks = val('txtdeailRemarks');
                /* Moisture and Moisture Slab are NOT written back for an output row (:2015-2036). */
                detailButtons(false);
                if (ES.contractWagesChargeToProduct) afterDetail(wagesFill('out'));
            }
            resetDetail();
            proportionateOverheadGrid();
            proportionatedPackingMaterialAmountInOutputGrid();
            generateSummaryForUser();
            $id('cmbEntryType').focus();
            lockConversionType();
            renderAll();
        } catch (e) { box(e.message); }
    }
    function detailButtons(editing) { show('Add', !editing); show('btnUpdateDetail', editing); show('btnCancelDetail', editing); }
    /** btnCancelDetail_Click:2058. */
    function cancelDetail() { detailButtons(false); resetDetail(); }

    /** resetDetail:1788. */
    function resetDetail() {
        updateDetailIndex = -1;
        ['cmbItem', 'cmbUOM', 'cmbGodown', 'CmbCropyr', 'cmbLot', 'cmbBagType', 'cmbRateUom', 'cmbMoistureSlab']
            .forEach(function (id) { var e = $id(id); if (e) e.value = '0'; });
        ['txtUnitWeight', 'txtAverageRate', 'txtMoisture', 'txtQty', 'txtRate', 'txtAmount', 'txtdeailRemarks'].forEach(function (id) { setVal(id, ''); });
        setVal('txtStock', '0');
        CheckExpenseAmountTotal = false;
        checkPackingMaterialAmountTotal = false;
        lockConversionType();
        refreshCombos();
    }
    /** CmbConversionType.Enabled = grdInput.RowCount == 0 && grdByProduct.RowCount == 0. */
    function lockConversionType() { var e = $id('CmbConversionType'); if (e) e.disabled = (INPUT.length + OUTPUT.length) > 0; refreshCombos(); }

    /** grdByProduct_DoubleClick:2428. */
    function editOutputRow(i) {
        try {
            if (convType() === 5) return;
            var r = OUTPUT[i];
            if (!r) return;
            updateDetailIndex = i;
            if (netD(r.IssueWeight) > 0) throw new Error('Record Not Update because record has exist another Form');
            selectByText('cmbEntryType', r.EntryType);
            setSel('cmbGodown', r.WareHouseId); setSel('cmbLot', r.JobLotId); setSel('cmbBagType', r.PackingTypeId); setSel('cmbItem', r.ItemId);
            bindUoms().then(function () {
                setSel('cmbUOM', r.ItemUOMId);
                selectByText('CmbCropyr', r.CropYear);
                setSel('cmbRateUom', r.RateUOMId);
                setVal('txtQty', F('q', r.Quantity));
                setVal('txtUnitWeight', F('q', r.Weight));
                setVal('txtRate', F('r', r.Rate));
                setVal('txtAmount', F('a', r.Amount));
                setVal('txtMoisture', r.Moisture);
                setSel('cmbMoistureSlab', r.MoistureSlabId);
                setVal('txtRemarks', r.Remarks);        // the HEADER remarks box, as on the desktop
                calculateWeight(); amountCalculation();
                /* the TextChanged events recompute Weight/Amount as the boxes are filled; the grid
                   texts are then shown as they were written. */
                setVal('txtUnitWeight', F('q', r.Weight)); setVal('txtAmount', F('a', r.Amount));
                detailButtons(true);
                refreshCombos();
            }).catch(function (e) { box(e.message); });
        } catch (e) { box(e.message); }
    }

    /** BtnUpdateComboValueIngrid_Click:7289 -> UpdatejobLotInGrid:7304. */
    function applyJobLotToGrid() {
        if (convType() === 5) return;
        try {
            if (!OUTPUT.length) throw new Error('Output Grid Record not Found');
            var id = netI(val('CmbJobLotForGrid')), text = '';
            if (id > 0) text = selText('CmbJobLotForGrid');
            else {
                var first = OUTPUT.filter(function (r) { return netI(r.JobLotId) > 0; })[0];
                if (first) { id = netI(first.JobLotId); text = first.JobLot; }
            }
            if (id === 0) { $id('CmbJobLotForGrid').focus(); throw new Error('Please Select JobLot'); }
            OUTPUT.forEach(function (r) { r.JobLotId = id; r.JobLot = text; });
            renderOutput();
        } catch (e) { box(e.message); }
    }

    // ================================================================ Input / Output grids

    /** grdInput_CellUpdated:2195. */
    function inputEdited(i, k, v) {
        var r = INPUT[i];
        if (!r) return;
        try {
            if (k === 'MoistureSlabId') r[k] = netI(v) || null;
            else if (k === 'Moisture' || k === 'Remarks') r[k] = v;
            else r[k] = netD(v);
            if (k === 'Quantity') {
                var w = netD(r.Quantity) * netD(r.PackEquivalent);
                r.Weight = w;
                r.Amount = w / netD(r.RateEquivalent) * netD(r.Rate);
            }
            if (k === 'Weight') {
                if (netD(r.BalWeight) < netD(r.Weight) && saveVisible && !$id('btnSave').disabled) {
                    r.Weight = 0;
                    renderInput();
                    throw new Error('Weight cannot greater than Balance Weight Please check!');
                }
                r.Amount = netD(r.Weight) / netD(r.RateEquivalent) * netD(r.Rate);
            }
            generateSummaryForUser();
            renderAll();
            afterDetail(wagesFill('in'));
        } catch (e) { box(e.message); }
    }
    /** grdInput_ColumnButtonClick:2234 (and Ctrl+Delete / Ctrl+Space on X, grdInput_KeyDown:6890). */
    function deleteInputRow(i, fromKeys) {
        var r = INPUT[i];
        if (!r) return;
        try {
            if (!ask('Are you sure to Delete?')) return;
            if (netI(r.Id) !== 0) InputDetailRowsRemoveIds = InputDetailRowsRemoveIds + ',' + String(r.Id);
            INPUT.splice(i, 1);
            if (!fromKeys) {
                proportionateOverheadGrid();
                proportionatedPackingMaterialAmountInOutputGrid();
                generateSummaryForUser();
                afterDetail(wagesFill('in'));
            } else {
                generateSummaryForUser();
            }
            lockConversionType();
            renderAll();
        } catch (e) { box(e.message); }
    }

    /** InputRowsForwardToOutput:2599 (BtnForwardRowsFromInputToOutPut_Click:2643). */
    function forwardRows() {
        if (convType() === 5) return;
        try { inputRowsForwardToOutput(); } catch (e) { box(e.message); }
    }
    function inputRowsForwardToOutput() {
        if (INPUT.length === 0) throw new Error('Input Grid Have no Rows..');
        var checked = INPUT.filter(function (r) { return r._chk; });
        if (checked.length === 0) throw new Error('Please Check any Row First...');
        checked.forEach(function (r) {
            var exists = OUTPUT.some(function (o) { return o.LineId !== null && o.LineId !== undefined && o.LineId !== '' && netI(o.LineId) === netI(r.LineId); });
            if (!exists) {
                OUTPUT.push({ Id: 0, EntryType: 'Recovery Head Rice', WareHouseId: netI(r.WareHouseId), WareHouse: String(r.WareHouse || ''),
                    CropYear: String(r.CropYear || ''), ItemId: netI(r.ItemId), Item: String(r.Item || ''), JobLotId: netI(r.JobLotId), JobLot: String(r.JobLot || ''),
                    ItemUOMId: netI(r.ItemUOMId), UOM: String(r.UOM || ''), PackEquivalent: netD(r.PackEquivalent), PackingTypeId: netI(r.PackingTypeId),
                    PackingType: String(r.PackingType || ''), Quantity: netD(r.Quantity), Weight: netD(r.Weight), Rate: netD(r.Rate),
                    RateUOMId: netI(r.RateUOMId), RateUOM: String(r.RateUOM || ''), RateEquivalent: netD(r.RateEquivalent),
                    AmountWithoutExpenses: netD(r.Amount), Moisture: netD(r.Moisture), MoistureSlabId: netI(r.MoistureSlabId),
                    MoistureSlabDescription: slabText(r.MoistureSlabId), PMAmount: 0, ItemPMAmount: 0, ExpAmount: 0, ItemExpAmount: 0,
                    Amount: netD(r.Amount), Remarks: String(r.Remarks || ''), IssueWeight: 0, LineId: netI(r.LineId), WagesAmount: 0,
                    labIPmActivityLogId: netI(r.labIPmActivityLogId) });
            }
        });
        if (OUTPUT.length > 0) tab2('paneOutput');
        renderAll();
        if (ES.contractWagesChargeToProduct && convType() !== 5) {
            afterDetail(wagesFill('out').then(function () { return wagesFill('in'); }));
        }
    }
    function slabText(id) {
        var s = String(id === null || id === undefined ? '' : id);
        var r = ((LK && LK.moistureSlabs) || []).filter(function (x) { return String(col(x, 'Id')) === s; })[0];
        return r ? String(col(r, 'MoistureSlabDescription')) : '';
    }

    /** grdByProduct_CellUpdated:2508. */
    function outputEdited(i, k, v) {
        var r = OUTPUT[i];
        if (!r) return;
        try {
            r[k] = netD(v);
            var a;
            if (k === 'Quantity') {
                var w = netD(r.Quantity) * netD(r.PackEquivalent);
                r.Weight = w;
                a = w / netD(r.RateEquivalent) * netD(r.Rate);
                r.Amount = a; r.AmountWithoutExpenses = a;
            }
            if (k === 'Weight') { a = netD(r.Weight) / netD(r.RateEquivalent) * netD(r.Rate); r.Amount = a; r.AmountWithoutExpenses = a; }
            if (k === 'Rate') { a = netD(r.Weight) / netD(r.RateEquivalent) * netD(r.Rate); r.Amount = a; r.AmountWithoutExpenses = a; }
            proportionatedPackingMaterialAmountInOutputGrid();
            proportionateOverheadGrid();
            generateSummaryForUser();
            renderAll();
            afterDetail(wagesFill('out'));
        } catch (e) { box(e.message); }
    }
    /** DeletebyProductRow:2471. */
    function deleteOutputRow(i) {
        try {
            if (convType() === 5) return;
            if (updateDetailIndex !== -1) throw new Error('Reset the Detail First...');
            if (saveVisible && !$id('btnSave').disabled) OUTPUT.splice(i, 1);
            proportionateOverheadGrid();
            proportionatedPackingMaterialAmountInOutputGrid();
            generateSummaryForUser();
            lockConversionType();
            renderAll();
            afterDetail(wagesFill('out'));
        } catch (e) { box(e.message); }
    }

    // ============================================================= PM / Overhead grids

    function newPmRow() { return { ItemId: null, WareHouseId: null, WareHouse: '', RackId: null, RackName: '', ItemConditionId: null,
        BrandItemId: null, BrandItem: '', BrandUomId: null, BrandUom: '', ItemQTY: null, ItemRate: null, Amount: null, ChargeTo: '', ContractScheduleId: null }; }
    /** AddRowInExpenseGrid:2658 - dtExpense.Rows.Add(0, 0, "", 0, "", 0, 0) - ChargeTo stays null. */
    function newOhRow() { return { ChartofAccount: '0', BrandItemId: 0, BrandItem: '', BrandUomId: 0, BrandUom: '', LedgerRemarks: '0', ExpAmount: 0, ChargeTo: null }; }

    /** ChargeTo value list: the stored value may be the Id ("1"/"2") or, for a saved row, the text it was saved as. */
    function chargeText(v) {
        var s = String(v === null || v === undefined ? '' : v);
        var r = (ES.chargeTo || []).filter(function (x) { return String(x.Id) === s; })[0];
        return r ? r.ChargeTo : s;
    }
    function listText(list, idKey, textKey, v) {
        var s = String(v === null || v === undefined ? '' : v);
        var r = (list || []).filter(function (x) { return String(col(x, idKey)) === s; })[0];
        return r ? String(col(r, textKey)) : '';
    }

    /** grdPackingMaterial_CellUpdated:2851. */
    function pmEdited(i, k, v) {
        var r = PMR[i];
        if (!r) return;
        if (k === 'ItemQTY' || k === 'ItemRate' || k === 'Amount') r[k] = v === '' ? null : netD(v);
        else if (k === 'ChargeTo') r[k] = v === '0' ? '' : v;
        else r[k] = (v === '0' || v === '') ? null : netI(v);
        var needRate = (k === 'ItemQTY' || k === 'ItemId' || k === 'WareHouseId' || k === 'ItemConditionId');
        var p = needRate
            ? getJson(api + '/pm-rate' + qs({ itemId: netI(r.ItemId), docDate: val('txtDocdate'), itemConditionId: netI(r.ItemConditionId), recId: RECID }))
                .then(function (d) { r.ItemRate = netD(d && d.rate); })
            : Promise.resolve();
        p.then(function () {
            if (needRate || k === 'ItemRate') {
                if (r.ItemQTY !== null && r.ItemQTY !== undefined && r.ItemQTY !== '' && r.ItemRate !== null && r.ItemRate !== undefined && r.ItemRate !== '')
                    r.Amount = netD(r.ItemQTY) * netD(r.ItemRate);
                else r.Amount = 0;
            }
            proportionatedPackingMaterialAmountInOutputGrid();
            generateSummaryForUser();
            renderAll();
        }).catch(function (e) { box(e.message); renderAll(); });
    }
    /** grdPackingMaterial_ColumnButtonClick:2819. */
    function pmButton(i, act) {
        try {
            if (act === 'del') { PMR.splice(i, 1); if (PMR.length === 0) PMR.push(newPmRow()); proportionatedPackingMaterialAmountInOutputGrid(); }
            if (act === 'add') { PMR.push(newPmRow()); proportionatedPackingMaterialAmountInOutputGrid(); }
            renderAll();
        } catch (e) { box(e.message); }
    }
    /** grdOverHead_CellUpdated:2789. */
    function ohEdited(i, k, v) {
        var r = OHR[i];
        if (!r) return;
        if (k === 'ExpAmount') r[k] = netD(v);
        else if (k === 'LedgerRemarks') r[k] = v;
        else if (k === 'ChargeTo') r[k] = v === '0' ? null : v;
        else r[k] = v;
        try {
            proportionateOverheadGrid();
            generateSummaryForUser();
        } catch (e) { box(e.message); }
        renderAll();
    }
    /** grdOverHead_ColumnButtonClick:2756. */
    function ohButton(i, act) {
        try {
            if (act === 'del') { OHR.splice(i, 1); if (OHR.length === 0) OHR.push(newOhRow()); proportionateOverheadGrid(); }
            if (act === 'add') { OHR.push(newOhRow()); proportionateOverheadGrid(); }
            generateSummaryForUser();
            renderAll();
        } catch (e) { box(e.message); }
    }

    /** btnGenerateItemsAndUom_Click:7557 - distinct (ItemId, Item) and (ItemId, ItemUOMId, UOM) of the output rows. */
    function generateItemsAndUom() {
        BRAND_ITEMS = []; BRAND_UOMS = [];
        if (OUTPUT.length <= 0) return;
        var seenI = {}, seenU = {};
        OUTPUT.forEach(function (r) {
            var ki = r.ItemId + '|' + r.Item;
            if (!seenI[ki]) { seenI[ki] = 1; BRAND_ITEMS.push({ ItemId: r.ItemId, Item: r.Item }); }
            var ku = r.ItemId + '|' + r.ItemUOMId + '|' + r.UOM;
            if (!seenU[ku]) { seenU[ku] = 1; BRAND_UOMS.push({ ItemId: r.ItemId, ItemUOMId: r.ItemUOMId, UOM: r.UOM }); }
        });
        say('Brand Item / Brand Uom lists generated from ' + OUTPUT.length + ' output row(s).');
    }

    // ------------------------------------------------------------- GrdPopUp (F1 pick lists)

    var PICK = null;
    /** GrdPopUp(dt, idCol, nameCol[, extra]) - closing without a choice returns Id 0 / Name "" (the
     *  desktop then writes those into the cells). */
    function pick(title, rows, idKey, nameKey, extraKey) {
        return new Promise(function (resolve) {
            PICK = { rows: rows || [], idKey: idKey, nameKey: nameKey, extraKey: extraKey, resolve: resolve };
            $id('pickTitle').textContent = title;
            $id('pickHead').innerHTML = '<th>' + esc(nameKey) + '</th>' + (extraKey ? '<th>' + esc(extraKey) + '</th>' : '');
            $id('pickFilter').value = '';
            drawPick();
            $id('pickModal').classList.add('is-open');
            $id('pickFilter').focus();
        });
    }
    function drawPick() {
        var f = val('pickFilter').toLowerCase();
        $id('pickBody').innerHTML = PICK.rows.map(function (r, i) {
            var t = String(col(r, PICK.nameKey)), x = PICK.extraKey ? String(col(r, PICK.extraKey)) : '';
            if (f && (t + ' ' + x).toLowerCase().indexOf(f) < 0) return '';
            return '<tr tabindex="0" data-i="' + i + '" style="cursor:pointer;"><td>' + esc(t) + '</td>' + (PICK.extraKey ? '<td>' + esc(x) + '</td>' : '') + '</tr>';
        }).join('') || '<tr><td colspan="2">No records</td></tr>';
    }
    function pickDone(i) {
        if (!PICK) return;
        var p = PICK, r = i === null ? null : p.rows[i];
        PICK = null;
        $id('pickModal').classList.remove('is-open');
        p.resolve(r ? { id: netI(col(r, p.idKey)), name: String(col(r, p.nameKey)), row: r } : { id: 0, name: '', row: null });
    }
    function pickClose() { pickDone(null); }

    function racks() { return ES.racks || []; }
    /** grdPM_KeyDown:6969, F1. */
    function pmF1(i, key) {
        var r = PMR[i];
        if (!r) return;
        var itemId = netI(r.ItemId), whId = netI(r.WareHouseId), rackId = netI(r.RackId);
        var brandItemId = netI(r.BrandItemId), brandUomId = netI(r.BrandUomId);
        var p;
        if (key === 'WareHouse') {
            var seen = {}, list = [];
            racks().forEach(function (x) { if (netI(x.ItemId) === itemId && !seen[x.WarehouseId]) { seen[x.WarehouseId] = 1; list.push({ Id: x.WarehouseId, WareHouseName: x.WareHouseName }); } });
            p = pick('Warehouse', list, 'Id', 'WareHouseName').then(function (s) {
                r.WareHouseId = s.id; r.WareHouse = s.name;
                var rk = racks().filter(function (x) { return netI(x.ItemId) === itemId && netI(x.WarehouseId) === s.id; });
                if (rackId > 0) {
                    if (!rk.some(function (x) { return netI(x.Id) === rackId; })) {
                        if (rk.length === 1) { r.RackId = rk[0].Id; r.RackName = rk[0].RackName; }
                        else { r.RackId = 0; r.RackName = ''; }
                    }
                } else if (rk.length === 1) { r.RackId = rk[0].Id; r.RackName = rk[0].RackName; }
            });
        } else if (key === 'RackName') {
            var seenR = {}, fr = [];
            racks().forEach(function (x) { if (netI(x.ItemId) === itemId && (whId === 0 || netI(x.WarehouseId) === whId) && !seenR[x.Id]) { seenR[x.Id] = 1; fr.push(x); } });
            if (fr.length === 0) return;
            p = pick('Rack', fr.map(function (x) { return { Id: x.Id, RackName: x.RackName, WarehouseId: x.WarehouseId, WarehouseName: x.WareHouseName }; }), 'Id', 'RackName', 'WarehouseName')
                .then(function (s) {
                    r.RackId = s.id; r.RackName = s.name;
                    if (whId === 0) {
                        var sel = fr.filter(function (x) { return netI(x.Id) === s.id; })[0];
                        if (sel) {
                            r.WareHouseId = sel.WarehouseId;
                            /* r.Cells["WareHouseName"] - a column this grid does not have. */
                            throw new Error('Object reference not set to an instance of an object.');
                        }
                    }
                });
        } else if (key === 'BrandItem') {
            p = pick('Brand Item', BRAND_ITEMS, 'ItemId', 'Item').then(function (s) {
                r.BrandItemId = s.id; r.BrandItem = s.name; brandItemId = s.id;
                if (s.id > 0) {
                    var u = BRAND_UOMS.filter(function (x) { return netI(x.ItemId) === brandItemId; });
                    if (u.length === 1) { brandUomId = netI(u[0].ItemUOMId); r.BrandUomId = brandUomId; r.BrandUom = u[0].UOM; }
                }
            });
        } else if (key === 'BrandUom') {
            if (brandItemId <= 0) { box('Please Select Brand Item first!'); return; }
            p = pick('Brand Uom', BRAND_UOMS.filter(function (x) { return netI(x.ItemId) === brandItemId; }), 'ItemUOMId', 'UOM').then(function (s) {
                brandUomId = s.id; r.BrandUomId = s.id; r.BrandUom = s.name;
            });
        } else return;
        p.then(function () {
            if (brandItemId > 0 && brandUomId > 0) proportionatedPackingMaterialAmountInOutputGrid();
            generateSummaryForUser();
            renderAll();
        }).catch(function (e) { renderAll(); box(e.message); });
    }
    /** grdOH_KeyDown:7180, F1. */
    function ohF1(i, key) {
        var r = OHR[i];
        if (!r) return;
        var brandItemId = netI(r.BrandItemId), brandUomId = netI(r.BrandUomId), p;
        if (key === 'BrandItem') {
            p = pick('Brand Item', BRAND_ITEMS, 'ItemId', 'Item').then(function (s) {
                brandItemId = s.id; r.BrandItemId = s.id; r.BrandItem = s.name;
                var u = BRAND_UOMS.filter(function (x) { return netI(x.ItemId) === brandItemId; });
                if (u.length === 1) { brandUomId = netI(u[0].ItemUOMId); r.BrandUomId = brandUomId; r.BrandUom = u[0].UOM; }
                if (brandItemId === 0) { brandUomId = 0; r.BrandUomId = 0; r.BrandUom = ''; }
            });
        } else if (key === 'BrandUom') {
            if (brandItemId <= 0) { box('Please Select Brand Item first!'); return; }
            p = pick('Brand Uom', BRAND_UOMS.filter(function (x) { return netI(x.ItemId) === brandItemId; }), 'ItemUOMId', 'UOM').then(function (s) {
                brandUomId = s.id; r.BrandUomId = s.id; r.BrandUom = s.name;
            });
        } else return;
        p.then(function () {
            if (brandItemId > 0 && brandUomId > 0) proportionateOverheadGrid();
            generateSummaryForUser();
            renderAll();
        }).catch(function (e) { renderAll(); box(e.message); });
    }

    // ========================================================== proportioning and costing

    /** Shared by proportionatedPackingMaterialAmountInOutputGrid:5941 and proportionateOverheadGrid:6115. */
    function proportion(src, amountKey, outGeneral, outItem, gridLabel) {
        var weightOld = {}, amountOld = {}, weightBrand = {}, amountBrand = {};
        if (OUTPUT.length > 0 && src.length > 0) {
            OUTPUT.forEach(function (o) {
                var e = String(o.EntryType || ''), bk = e + '_' + netI(o.ItemId) + '_' + netI(o.ItemUOMId), w = netD(o.Weight);
                weightOld[e] = (weightOld[e] || 0) + w;
                weightBrand[bk] = (weightBrand[bk] || 0) + w;
            });
            src.forEach(function (r, idx) {
                var e = chargeText(r.ChargeTo), it = netI(r.BrandItemId), u = netI(r.BrandUomId), a = netD(r[amountKey]);
                if (it === 0) { amountOld[e] = (amountOld[e] || 0) + a; return; }
                if (u === 0) throw new Error(gridLabel + ' grid row#:' + (idx + 1) + ' must have Brand Uom');
                var bk = e + '_' + it + '_' + u;
                amountBrand[bk] = (amountBrand[bk] || 0) + a;
            });
            OUTPUT.forEach(function (o) {
                var e = String(o.EntryType || ''), bk = e + '_' + netI(o.ItemId) + '_' + netI(o.ItemUOMId), w = netD(o.Weight);
                if (Object.prototype.hasOwnProperty.call(amountBrand, bk)) {
                    var tw = weightBrand[bk] || 0;
                    o[outItem] = tw > 0 ? amountBrand[bk] / tw * w : 0;
                } else o[outItem] = 0;
                if (Object.prototype.hasOwnProperty.call(amountOld, e)) {
                    var tw2 = weightOld[e] || 0;
                    o[outGeneral] = tw2 > 0 ? amountOld[e] / tw2 * w : 0;
                } else o[outGeneral] = 0;
            });
        } else if (OUTPUT.length > 0 && src.length === 0) {
            OUTPUT.forEach(function (o) { o[outGeneral] = 0; o[outItem] = 0; });
        }
    }
    function proportionatedPackingMaterialAmountInOutputGrid() {
        try { proportion(PMR, 'Amount', 'PMAmount', 'ItemPMAmount', 'Packing Material'); } catch (e) { box(e.message); }
    }
    function proportionateOverheadGrid() {
        try { proportion(OHR, 'ExpAmount', 'ExpAmount', 'ItemExpAmount', 'Over Head'); } catch (e) { box(e.message); }
    }

    /** The cell's .Text as the grid formats it (gridsettingsByProduct / gridsettings formats). */
    var OUT_FMT = { AmountWithoutExpenses: 'a', PMAmount: 'a', ExpAmount: 'a', ItemPMAmount: 'a', ItemExpAmount: 'a', WagesAmount: 'a',
        Amount: 'a', Quantity: 'q', Weight: 'q', Rate: 'r', RateEquivalent: 'raw' };
    function txt(row, key, fmts) { var v = row[key]; if (v === null || v === undefined || v === '') return ''; return F((fmts || OUT_FMT)[key] || 'raw', v); }

    /** handleAverageRateCalculation:6236. */
    function handleAverageRateCalculation() {
        try {
            var input = 0, byProduct = 0, head = 0;
            INPUT.forEach(function (r) { input += netD(r.Amount); });
            OUTPUT.forEach(function (r) {
                if (r.EntryType === 'Recovery By Product') byProduct += netD(r.Amount);
                else if (r.EntryType === 'Recovery Head Rice') head += netD(r.Weight);
            });
            var perKg = netD(netD(input) - netD(byProduct)) / netD(head);
            OUTPUT.forEach(function (r) {
                var extras = function () {
                    return netD(txt(r, 'PMAmount')) + netD(txt(r, 'ExpAmount')) + netD(txt(r, 'ItemPMAmount')) + netD(txt(r, 'ItemExpAmount')) + netD(txt(r, 'WagesAmount'));
                };
                if (r.EntryType === 'Recovery By Product') {
                    r.Amount = g15(netD(txt(r, 'AmountWithoutExpenses')) + extras());
                } else if (r.EntryType === 'Recovery Head Rice') {
                    var eq = netD(txt(r, 'RateEquivalent'));
                    var rateUom = netD(perKg) * eq;
                    var awe = netD(txt(r, 'Weight')) / eq * netD(rateUom);
                    r.AmountWithoutExpenses = g15(awe);
                    var awx = netD(awe) + extras();
                    r.Amount = g15(awx);
                    r.Rate = g15(netD(awx) / netD(txt(r, 'Weight')) * eq);
                }
            });
            generateSummaryForUser();
        } catch (e) { box(e.message); }
    }

    /** btnGenerate_Click:6402 - asks first only for Conversion Type 3. */
    function generate() {
        if (convType() !== 3 || ask('Are you sure to Generate When ConversionType [' + selText('CmbConversionType') + ']?')) {
            handleAverageRateCalculation();
            renderAll();
        }
    }

    /** GridEX GetTotal(col, Sum) over the rows. */
    function total(rows, key) { return rows.reduce(function (a, r) { return a + netD(r[key]); }, 0); }
    function wagesRegularTotal() {
        var t = 0;
        WG.concat(ST).forEach(function (r) { if (String(r.WagesType) === 'Regular') t += netD(r.Amount); });
        return t;
    }

    /** summeryreset:1813. */
    function summeryreset() {
        SUM = { iq: '', iw: '', ir: '', ia: '', bq: '', bw: '', br: '', ba: '', fq: '', fw: '', fr: '', fa: '',
                ohA: '', ohQ: '', ohR: '', ohW: '', pmA: '', pmQ: '', pmR: '', pmW: '', diffLabel: 'None', diffColor: '#000',
                diff: F('a', 0), wages: SUM.wages === undefined ? '0' : SUM.wages };
    }
    /** GenerateSummaryForUser:6417. */
    function generateSummaryForUser() {
        summeryreset();
        if (INPUT.length > 0) {
            SUM.iq = F('q', total(INPUT, 'Quantity')); SUM.iw = F('q', total(INPUT, 'Weight'));
            SUM.ir = F('r', total(INPUT, 'Rate')); SUM.ia = F('a', total(INPUT, 'Amount'));
        }
        OUTPUT.forEach(function (r) {
            if (OUTPUT.length === 1) {
                if (r.EntryType === 'Recovery By Product') { SUM.bq = F('q', netD(txt(r, 'Quantity'))); SUM.bw = F('q', netD(txt(r, 'Weight'))); SUM.br = F('r', netD(txt(r, 'Rate'))); SUM.ba = F('a', netD(txt(r, 'Amount'))); }
                else if (r.EntryType === 'Recovery Head Rice') { SUM.fq = F('q', netD(txt(r, 'Quantity'))); SUM.fw = F('q', netD(txt(r, 'Weight'))); SUM.fr = F('r', netD(txt(r, 'Rate'))); SUM.fa = F('a', netD(txt(r, 'Amount'))); }
            } else if (OUTPUT.length > 1) {
                if (r.EntryType === 'Recovery By Product') {
                    SUM.bq = F('q', netD(SUM.bq) + netD(r.Quantity)); SUM.bw = F('q', netD(SUM.bw) + netD(r.Weight));
                    SUM.br = F('r', netD(SUM.br) + netD(r.Rate)); SUM.ba = F('a', netD(SUM.ba) + netD(r.Amount));
                } else if (r.EntryType === 'Recovery Head Rice') {
                    SUM.fq = F('q', netD(SUM.fq) + netD(r.Quantity)); SUM.fw = F('q', netD(SUM.fw) + netD(r.Weight));
                    SUM.fr = F('r', netD(SUM.fr) + netD(r.Rate)); SUM.fa = F('a', netD(SUM.fa) + netD(r.Amount));
                }
            }
        });
        var gl = netD(SUM.fa) - netD(SUM.ia), gain = gl > 0;
        SUM.diffLabel = gain ? 'Gain' : 'Loss'; SUM.diffColor = gain ? 'green' : 'red'; SUM.diff = F('a', gl);
        SUM.pmQ = F('q', total(PMR, 'ItemQTY')); SUM.pmR = F('r', total(PMR, 'ItemRate')); SUM.pmA = F('a', total(PMR, 'Amount'));
        SUM.ohA = F('a', total(OHR, 'ExpAmount'));
        SUM.wages = F('a', wagesRegularTotal());
        if (netD(SUM.ia) > 0 && netD(SUM.iw) > 0) SUM.ir = F('r', netD(SUM.ia) / netD(SUM.iw) * 40);
        if (netD(SUM.ba) > 0 && netD(SUM.bw) > 0) SUM.br = F('r', netD(SUM.ba) / netD(SUM.bw) * 40);
        if (netD(SUM.fa) > 0 && netD(SUM.fw) > 0) SUM.fr = F('r', netD(SUM.fa) / netD(SUM.fw) * 40);
        renderSummary();
    }
    /** groupBox2 "Summery Calculations" - two panels of text boxes. */
    function renderSummary() {
        var t3 = convType() === 3, w = !!ES.contractWagesChargeToProduct;
        function head() { return '<thead><tr><th>Description</th><th class="num">ItemQty</th><th class="num">NetWeight</th><th class="num">Rate</th><th class="num">Amount</th></tr></thead>'; }
        function row(label, q, wt, r, a, style) {
            return '<tr><td' + (style || '') + '>' + esc(label) + '</td><td class="num">' + esc(q) + '</td><td class="num">' + esc(wt)
                 + '</td><td class="num">' + esc(r) + '</td><td class="num"' + (style || '') + '>' + esc(a) + '</td></tr>';
        }
        var left = row('Raw Material', SUM.iq, SUM.iw, SUM.ir, SUM.ia) + row('By Product', SUM.bq, SUM.bw, SUM.br, SUM.ba)
                 + row('Finish Goods', SUM.fq, SUM.fw, SUM.fr, SUM.fa)
                 + (w ? row('Wages Amount', '', '', '', SUM.wages) : '');
        var right = row('Over Heads', SUM.ohQ, SUM.ohW, SUM.ohR, SUM.ohA) + row('Packing Material', SUM.pmQ, SUM.pmW, SUM.pmR, SUM.pmA)
                  + (t3 ? row(SUM.diffLabel === 'None' ? 'Gain/Loss' : SUM.diffLabel, '', '', '', SUM.diff, ' style="font-weight:bold;color:' + SUM.diffColor + ';"') : '');
        $id('scSummary').innerHTML =
            '<div><div class="sc-bar">Summery Calculations</div><table class="win-grid">' + head() + '<tbody>' + left + '</tbody></table></div>'
          + '<div><div class="sc-bar">&nbsp;</div><table class="win-grid">' + head() + '<tbody>' + right + '</tbody></table></div>';
    }

    /** CheckExpenseandPackingMaterialAmountTotal:6062 - CheckExpenseAmountTotal is never cleared here. */
    function checkExpenseandPackingMaterialAmountTotal() {
        checkPackingMaterialAmountTotal = false;
        var pm = total(PMR, 'Amount'), ex = total(OHR, 'ExpAmount');
        var exBP = total(OUTPUT, 'ExpAmount') + total(OUTPUT, 'ItemExpAmount');
        var pmBP = total(OUTPUT, 'PMAmount') + total(OUTPUT, 'ItemPMAmount');
        if (pm > 0 && pmBP > 0 && pm !== pmBP) {
            if (pm > pmBP) { if (pm - pmBP > 5) checkPackingMaterialAmountTotal = true; }
            else if (pmBP > pm && pmBP - pm > 5) checkPackingMaterialAmountTotal = true;
        }
        if (!(ex > 0) || !(exBP > 0) || ex === exBP) return;
        if (ex > exBP) { if (ex - exBP > 5) CheckExpenseAmountTotal = true; }
        else if (exBP > ex && exBP - ex > 5) CheckExpenseAmountTotal = true;
    }

    // ======================================================================= wages grids

    function wagesTable(which) { return which === 'st' ? ST : WG; }

    /** WagesGridFillFromInputOutput:3220 - TypeId 0 both tables (dtStiching only for outputs), 1 regular, 2 other. */
    function wagesFill(grid, typeId) {
        typeId = typeId || 0;
        var isInput = grid === 'in', rows = isInput ? INPUT : OUTPUT, tt = isInput ? 'Issue' : 'Recovery';
        var jobs = [];
        if (rows.length > 0) {
            var groups = [], idx = {};
            rows.forEach(function (r) {
                var key = [netI(r.ItemId), String(r.Item || ''), netI(r.WareHouseId), String(r.WareHouse || ''), String(r.CropYear || ''),
                           netI(r.JobLotId), String(r.JobLot || ''), netI(r.PackingTypeId), String(r.PackingType || ''), netD(r.PackEquivalent)].join('\u0001');
                var g = idx[key];
                if (!g) {
                    g = idx[key] = { ItemId: netI(r.ItemId), Item: String(r.Item || ''), WarehouseId: netI(r.WareHouseId), Warehouse: String(r.WareHouse || ''),
                        Crop: String(r.CropYear || ''), JobLotId: netI(r.JobLotId), JobLot: String(r.JobLot || ''), PackingTypeId: netI(r.PackingTypeId),
                        PackingType: String(r.PackingType || ''), PackEquivalent: netD(r.PackEquivalent), Quantity: 0, Weight: 0, TransactionType: tt };
                    groups.push(g);
                }
                g.Quantity += netD(r.Quantity);
                g.Weight += netD(r.Weight);
            });
            if (typeId === 0) { jobs.push(updateSummaryTable(WG, groups, tt)); if (tt !== 'Issue') jobs.push(updateSummaryTable(ST, groups, tt)); }
            else if (typeId === 1) jobs.push(updateSummaryTable(WG, groups, tt));
            else if (typeId === 2 && tt !== 'Issue') jobs.push(updateSummaryTable(ST, groups, tt));
        } else {
            if (typeId === 0) { removeByType(WG, tt); removeByType(ST, tt); }
            else if (typeId === 1) removeByType(WG, tt);
            else if (typeId === 2) removeByType(ST, tt);
        }
        return Promise.all(jobs).then(function () {
            proportionateWagesAmountInOutputGrid();
        }).catch(function (e) { throw new Error('Error in WagesGridFillFromInputOutput: ' + e.message); });
    }
    function sameKey(r, g) {
        return netI(r.ItemId) === g.ItemId && netI(r.MoveFromId) === g.WarehouseId && String(r.Crop === null || r.Crop === undefined ? '' : r.Crop) === g.Crop
            && netI(r.jobLotId) === g.JobLotId && netI(r.packingTypeId) === g.PackingTypeId && netD(r.PackSize) === g.PackEquivalent
            && String(r.TransactionType) === g.TransactionType;
    }
    /** UpdateSummaryTable:3322. New rows need CheckItemsFreeofcostforWages(docDate, 66, item, 0). */
    function updateSummaryTable(table, groups, tt) {
        var news = [];
        groups.forEach(function (g) {
            var existing = null;
            for (var i = 0; i < table.length; i++) if (sameKey(table[i], g)) { existing = table[i]; break; }
            if (!existing) news.push(g);
            else {
                existing.Quantity = g.Quantity; existing.BillQty = g.Quantity; existing.BillWeight = g.Weight; existing.Weight = g.Weight;
                existing.RefDocQty = g.Quantity; existing.RefDocWeight = g.Weight;
                var rate = netD(existing.Rate);
                var amount = ES.wagesAmountCalculateOnQty ? g.Quantity * rate : (g.PackEquivalent > 0 ? g.Weight / g.PackEquivalent * rate : 0);
                existing.Amount = rnd(amount, 2);
            }
        });
        /* the new rows take max(RefLineId)+1, +2 ... counted BEFORE the stale rows are removed. */
        var baseMax = table.reduce(function (a, r) { return Math.max(a, netI(r.RefLineId)); }, 0);
        var keys = groups;
        for (var k = table.length - 1; k >= 0; k--) {
            var r = table[k];
            if (String(r.TransactionType) !== tt) continue;
            if (!keys.some(function (g) { return sameKey(r, g); })) table.splice(k, 1);
        }
        if (!news.length) return Promise.resolve();
        return postJson(api + '/wages-free', { docDate: val('txtDocdate'), rows: news.map(function (g) { return { itemId: g.ItemId, wagesId: 0 }; }) })
            .then(function (flags) {
                news.forEach(function (g, i) {
                    var maxRow = baseMax + i;
                    table.push({ SupplierId: null, ContractorName: null, WagesId: null, WagesAccount: null,
                        WagesType: flags && flags[i] ? 'Free Of Cost' : 'Regular', Date: null, packingTypeId: g.PackingTypeId, packingType: g.PackingType,
                        Weight: g.Weight, PackSize: g.PackEquivalent, Quantity: g.Quantity, WeightCut: null, BillQty: g.Quantity, BillWeight: g.Weight,
                        RateWithoutAddLess: null, RateAddLess: null, Rate: null, Amount: null, ItemId: g.ItemId, Item: g.Item, jobLotId: g.JobLotId,
                        jobLot: g.JobLot, Crop: g.Crop, MoveFromId: g.WarehouseId, MoveFrom: g.Warehouse, MoveToId: g.WarehouseId, MoveTo: g.Warehouse,
                        PurchaseGLAC: null, WarehouseType: null, RefDocQty: g.Quantity, RefDocWeight: g.Weight, RefLineId: maxRow + 1,
                        WagesScheduleId: null, TransactionType: g.TransactionType });
                });
            });
    }
    function removeByType(table, tt) { for (var k = table.length - 1; k >= 0; k--) if (String(table[k].TransactionType) === tt) table.splice(k, 1); }

    /** ProportionateWagesAmountInOutputGrid:6333. */
    function proportionateWagesAmountInOutputGrid() {
        var totalWages = wagesRegularTotal();
        SUM.wages = F('a', totalWages);
        var totalWeight = total(OUTPUT, 'Weight');
        if (OUTPUT.length === 0) return;
        if ((WG.length === 0 && ST.length === 0) || totalWeight <= 0) { OUTPUT.forEach(function (o) { o.WagesAmount = 0; }); return; }
        OUTPUT.forEach(function (o) { o.WagesAmount = g15(totalWages / totalWeight * netD(o.Weight)); });
    }

    function clampAddLess(rwal, ral, showMsg) {
        if (ES.percentageForRateAddLess > 0) {
            var byCfg = ES.percentageForRateAddLess * rwal / 100;
            var minus = !(ral > 0);
            if (Math.abs(ral) > byCfg) {
                ral = minus ? netD('-' + csStr(byCfg)) : byCfg;
                if (showMsg) box('RateAddLess can not be grater than RateAdLess In config ' + csStr(byCfg));
            }
        } else {
            ral = 0;
            if (showMsg) box('Please set RateAddLess Percentage in config first...');
        }
        return ral;
    }
    function fetchWagesRate(r) {
        return getJson(api + '/wages-rate' + qs({ docDate: val('txtDocdate'), packSize: netD(r.PackSize), wagesId: netI(r.WagesId), contractorId: netI(r.SupplierId) }))
            .then(function (d) { return d && d.WagesRate !== undefined ? { rate: netD(d.WagesRate), id: netI(d.ScheduleId) } : null; });
    }
    function fetchFree(r) {
        return postJson(api + '/wages-free', { docDate: val('txtDocdate'), rows: [{ itemId: netI(r.ItemId), wagesId: netI(r.WagesId) }] })
            .then(function (f) { return !!(f && f[0]); });
    }

    /** grdwagesDetail_CellUpdated:3578. */
    function regularEdited(i, k, v) {
        var r = WG[i];
        if (!r) return;
        if (k === 'SupplierId' || k === 'WagesId') r[k] = v === '0' ? null : netI(v);
        else r[k] = netD(v);
        var p = Promise.resolve();
        if (k === 'WagesId') p = fetchFree(r).then(function (free) { r.WagesType = free ? 'Free Of Cost' : 'Regular'; });
        p.then(function () {
            if (k === 'Quantity' && netD(r.Quantity) > 0 && netD(r.PackSize) > 0) {
                var q = netD(r.Quantity);
                r.Quantity = rnd(q, 2);
                var w = q * netD(r.PackSize);
                r.Weight = netD(csStr(w));
                r.BillWeight = rnd(w - Math.abs(q * netD(r.WeightCut)), 2);
            }
            if ((k === 'Weight' || k === 'WeightCut') && netD(r.Weight) > 0 && netD(r.PackSize) > 0) {
                var q2 = netD(r.Weight) / netD(r.PackSize);
                r.Quantity = rnd(q2, 2);
                r.BillWeight = rnd(netD(r.Weight) - Math.abs(netD(r.WeightCut) * q2), 2);
            }
            var rateJob = Promise.resolve(null);
            if (['WagesId', 'SupplierId', 'Quantity', 'Weight', 'WeightCut', 'RateAddLess'].indexOf(k) >= 0) {
                if (String(r.WagesType) === 'Free Of Cost' || String(r.WarehouseType || '') === 'Dryer') {
                    r.WagesScheduleId = 0; r.Rate = 0; r.Amount = 0;
                } else rateJob = fetchWagesRate(r);
            }
            return rateJob.then(function (d) {
                var wr = d ? d.rate : 0, sid = d ? d.id : 0;
                if (wr > 0) {
                    r.Rate = wr; r.WagesScheduleId = sid;
                    if (netD(r.Rate) > 0 && netD(r.BillWeight) > 0) {
                        var rwal = netD(r.Rate);
                        r.RateWithoutAddLess = rwal;
                        var ral = netD(r.RateAddLess);
                        if (ES.enableAddLessOnWagesRegular && k === 'RateAddLess') { ral = clampAddLess(rwal, ral, true); r.RateAddLess = ral; }
                        var rate = rwal + ral;
                        r.Rate = rate;
                        var amt = ES.wagesAmountCalculateOnQty ? netD(r.Quantity) * rate : netD(r.BillWeight) / netD(r.PackSize) * rate;
                        r.Amount = rnd(amt, 2);
                    } else r.Amount = 0;
                } else {
                    r.WagesScheduleId = 0; r.Rate = 0; r.RateWithoutAddLess = 0; r.Amount = 0;
                }
            });
        }).then(function () {
            proportionateWagesAmountInOutputGrid();
            generateSummaryForUser();
            renderAll();
        }).catch(function (e) { box(e.message); renderAll(); });
    }

    /** grdStiching_CellUpdated:3955. */
    function otherEdited(i, k, v) {
        var r = ST[i];
        if (!r) return;
        if (k === 'SupplierId' || k === 'WagesId') r[k] = v === '0' ? null : netI(v);
        else r[k] = netD(v);
        var p = Promise.resolve();
        if (k === 'WagesId') p = fetchFree(r).then(function (free) { r.WagesType = free ? 'Free Of Cost' : 'Regular'; });
        p.then(function () {
            if (['WagesId', 'SupplierId', 'Quantity', 'Weight', 'WeightCut', 'RateAddLess'].indexOf(k) < 0) return null;
            if (String(r.WagesType) === 'Free Of Cost' || String(r.WarehouseType || '') === 'Dryer') { r.Rate = 0; r.Amount = 0; return null; }
            return fetchWagesRate(r).then(function (d) {
                var wr = d ? d.rate : 0, sid = d ? d.id : 0;
                if (wr > 0) {
                    r.WagesScheduleId = sid; r.Rate = wr;
                    if (netD(r.Weight) > 0 && netD(r.PackSize) > 0 && !ES.wagesAmountCalculateOnQty) r.BillWeight = rnd(netD(r.Weight), 2);
                    if (netD(r.Quantity) > 0 && netD(r.WeightCut) > 0 && netD(r.PackSize) > 0 && netD(r.Weight) > 0) {
                        var q = netD(r.Quantity), bw = netD(r.Weight) - Math.abs(netD(r.WeightCut) * q);
                        if (!ES.wagesAmountCalculateOnQty) r.BillWeight = rnd(bw, 2); else r.Quantity = rnd(q, 2);
                    }
                    if (netD(r.Rate) > 0 && netD(r.BillWeight) > 0) {
                        var rwal = netD(r.Rate);
                        r.RateWithoutAddLess = rwal;
                        var ral = netD(r.RateAddLess);
                        if (ES.enableAddLessOnWagesRegular && k === 'RateAddLess') { ral = clampAddLess(rwal, ral, true); r.RateAddLess = ral; }
                        var rate = rwal + ral;
                        r.Rate = rate;
                        r.Amount = rnd(netD(r.BillWeight) / netD(r.PackSize) * rate, 2);
                    } else r.Amount = 0;
                } else { r.Rate = 0; r.WagesScheduleId = 0; }
            });
        }).then(function () {
            if (k === 'Weight' || k === 'WeightCut') {
                if (netD(r.Weight) > 0 && netD(r.PackSize) > 0) {
                    var q2 = netD(r.Weight) / netD(r.PackSize);
                    r.BillWeight = rnd(netD(r.Weight), 2);
                    r.Quantity = rnd(q2, 2);
                }
                if (netD(r.Quantity) > 0 && netD(r.WeightCut) > 0 && netD(r.PackSize) > 0 && netD(r.Weight) > 0) {
                    var q3 = netD(r.Quantity);
                    r.BillWeight = rnd(netD(r.Weight) - Math.abs(netD(r.WeightCut) * q3), 2);
                    r.Quantity = rnd(q3, 2);
                }
                stitchAmount(r, k);
            }
            if (k === 'Quantity' || k === 'WeightCut') {
                if (netD(r.Quantity) > 0 && netD(r.PackSize) > 0 && netD(r.Weight) > 0) {
                    var wc = netD(r.WeightCut), ps = netD(r.PackSize), q4 = netD(r.Quantity);
                    var w3 = q4 * ps, bw3 = w3 - Math.abs(q4 * Math.abs(wc));
                    if (!ES.wagesAmountCalculateOnQty) { r.Weight = netD(csStr(w3)); r.BillWeight = rnd(bw3, 2); }
                    else r.Quantity = rnd(q4, 2);
                }
                if (netD(r.Quantity) > 0 && netD(r.WeightCut) > 0 && netD(r.PackSize) > 0 && netD(r.Weight) > 0) {
                    var bw4 = netD(r.Weight) - Math.abs(netD(r.WeightCut) * netD(r.Quantity));
                    if (!ES.wagesAmountCalculateOnQty) r.BillWeight = rnd(bw4, 2);
                }
                if (netD(r.Rate) > 0 && netD(r.BillWeight) > 0) {
                    var rwal3 = netD(r.Rate), ral3 = netD(r.RateAddLess);
                    if (ES.enableAddLessOnWagesRegular && k === 'RateAddLess') { ral3 = clampAddLess(rwal3, ral3, true); r.RateAddLess = ral3; }
                    var rate3 = rwal3 + ral3;
                    r.Rate = rate3;
                    r.Amount = rnd(netD(r.BillWeight) / netD(r.PackSize) * rate3, 2);
                } else r.Amount = 0;
            }
            proportionateWagesAmountInOutputGrid();
            generateSummaryForUser();
            renderAll();
        }).catch(function (e) { box(e.message); renderAll(); });
    }
    function stitchAmount(r, k) {
        if (netD(r.Rate) > 0 && netD(r.BillWeight) > 0) {
            var rwal = netD(r.Rate);
            r.RateWithoutAddLess = rwal;
            var ral = netD(r.RateAddLess);
            if (ES.enableAddLessOnWagesRegular && k === 'RateAddLess') { ral = clampAddLess(rwal, ral, true); r.RateAddLess = ral; }
            var rate = rwal + ral;
            r.Rate = rate;
            r.Amount = rnd(netD(r.BillWeight) / netD(r.PackSize) * rate, 2);
        } else r.Amount = 0;
    }

    function copyWages(rrr, over) {
        var o = {};
        for (var k in rrr) if (Object.prototype.hasOwnProperty.call(rrr, k)) o[k] = rrr[k];
        for (var j in over) o[j] = over[j];
        return o;
    }
    /** AddRowInGLGrid:3746 (regular) and AddRowInStichingGrid:3853 (other). */
    function addWagesRow(which, i) {
        try {
            var table = wagesTable(which), rrr = table[i];
            if (!rrr) return;
            var tt = String(rrr.TransactionType), src = tt === 'Issue' ? INPUT : (tt === 'Recovery' ? OUTPUT : []);
            var totalWeight = total(src, 'Weight'), totalQty = total(src, 'Quantity');
            var weight = 0, weightCut = 0, qty = 0;
            table.forEach(function (x) { if (String(x.TransactionType) === tt) { weight += netD(x.Weight); weightCut = netD(x.WeightCut); qty += netD(x.Quantity); } });
            var grossW = totalWeight - weight, grossQ = totalQty - qty, ps = netD(rrr.PackSize), row = null;
            var over = { TransactionType: tt };
            if (which === 'st') {
                /* the wages TYPE comes from the Regular grid's current row (grdwagesDetail.CurrentRow). */
                var cur = WG[CUR.wg];
                if (!cur) throw new Error('Object reference not set to an instance of an object.');
                over.WagesType = cur.WagesType;
            }
            if (ES.wagesAmountCalculateOnQty) {
                if (grossQ > 0) {
                    var iw = grossQ * ps;
                    row = copyWages(rrr, over);
                    row.Weight = which === 'st' ? iw : rnd(iw, 2); row.Quantity = rnd(grossQ, 2); row.BillWeight = iw;
                    row.Amount = rnd(grossQ * netD(rrr.Rate), 2);
                } else row = copyWages(rrr, over);   // DocType 66: OtherCondition is always true (and AddRowInStichingGrid copies too)
            } else if (grossW > 0) {
                var iq = grossW / ps, bwp = grossW - iq * weightCut;
                row = copyWages(rrr, over);
                row.Weight = grossW; row.Quantity = rnd(iq, 2); row.BillWeight = bwp;
                row.Amount = rnd(bwp / ps * netD(rrr.Rate), 2);
            } else row = copyWages(rrr, over);
            if (row) table.push(row);
            proportionateWagesAmountInOutputGrid();
            generateSummaryForUser();
            renderAll();
        } catch (e) { box(e.message); }
    }
    /** grdwagesDetail_ColumnButtonClick:3716 / grdStiching_ColumnButtonClick:3925. */
    function wagesButton(which, i, act) {
        var table = wagesTable(which);
        if (act === 'del') {
            if (table.length <= 1) { box(which === 'st' ? 'You Can Not Delete All rows....' : 'You Can Not Delete All rows'); return; }
            table.splice(i, 1);
        }
        if (act === 'add') { addWagesRow(which, i); return; }
        proportionateWagesAmountInOutputGrid();
        generateSummaryForUser();
        renderAll();
    }
    /** AddContractorValuesForAllInGrid:7417 - the FIRST row's contractor and wages account onto every row. */
    function applyAll(which) {
        var table = which === 'oth' ? ST : WG, cur = which === 'oth' ? CUR.st : CUR.wg;
        if (!table[cur] || !table.length) { proportionateWagesAmountInOutputGrid(); generateSummaryForUser(); renderAll(); return; }
        var first = table[0], c = netI(first.SupplierId), w = netI(first.WagesId);
        if (c === 0 && w === 0) { box('First row contains No values Of Contractor And Wages Account.'); return; }
        var btn = which === 'oth' ? 'btnAddContractorValuesForAllSticingWages' : 'btnAddContractorValuesForAllDetailWages';
        return busy(btn, function () {
            return table.reduce(function (p, r) {
                return p.then(function () {
                    r.SupplierId = c; r.WagesId = w;
                    return fetchWagesRate(r).then(function (d) {
                        var wr = d ? d.rate : 0, sid = d ? d.id : 0;
                        if (wr > 0) {
                            r.WagesScheduleId = sid; r.Rate = wr;
                            if (netD(r.Weight) > 0 && netD(r.PackSize) > 0) r.BillWeight = rnd(netD(r.Weight), 2);
                            if (netD(r.Quantity) > 0 && netD(r.WeightCut) > 0 && netD(r.PackSize) > 0 && netD(r.Weight) > 0)
                                r.BillWeight = rnd(netD(r.Weight) - Math.abs(netD(r.WeightCut) * netD(r.Quantity)), 2);
                            if (netD(r.Rate) > 0 && netD(r.BillWeight) > 0) {
                                var rwal = netD(r.Rate);
                                r.RateWithoutAddLess = rwal;
                                var ral = netD(r.RateAddLess);
                                if (ES.enableAddLessOnWagesRegular) { ral = clampAddLess(rwal, ral, false); r.RateAddLess = ral; }
                                var rate = rwal + ral;          /* the Rate cell keeps the schedule rate */
                                var amt = ES.wagesAmountCalculateOnQty ? netD(r.Quantity) * rate : netD(r.BillWeight) / netD(r.PackSize) * rate;
                                r.Amount = rnd(amt, 2);
                            } else r.Amount = 0;
                        } else { r.Rate = 0; r.WagesScheduleId = 0; }
                    });
                });
            }, Promise.resolve()).then(function () {
                proportionateWagesAmountInOutputGrid();
                generateSummaryForUser();
                renderAll();
            }).catch(function (e) { box(e.message); renderAll(); });
        });
    }
    /** btnResetWagesFromInput_Click:3114 / btnResetWagesForOutPut_Click:3132 / btnResetOtherWagesForOutPut_Click:3154. */
    function resetWages(kind) {
        var p;
        if (kind === 'regIn') { removeByType(WG, 'Issue'); p = wagesFill('in', 1); }
        else if (kind === 'regOut') { removeByType(WG, 'Recovery'); p = wagesFill('out', 1); }
        else { removeByType(ST, 'Recovery'); p = wagesFill('out', 2); }
        p.then(function () { generateSummaryForUser(); renderAll(); }).catch(function (e) { box(e.message); });
    }

    /** WagesDetailReadbyId:5285. */
    function wagesDetailReadById(id) {
        return getJson(api + '/wages-detail?id=' + id).then(function (rows) {
            (rows || []).forEach(function (row) {
                var t = {
                    SupplierId: col(row, 'ContractorId'), ContractorName: col(row, 'CompanyName'), WagesId: netI(col(row, 'InvConractorWagesAccountsId')),
                    WagesAccount: col(row, 'WagesAccountName'), WagesType: truthy(col(row, 'FreeOfCost')) ? 'Free Of Cost' : 'Regular',
                    Date: col(row, 'RefDocDate'), packingTypeId: col(row, 'InvPackingTypeId'), packingType: col(row, 'PackTypeDesc'),
                    Weight: netD(col(row, 'Weight')), PackSize: col(row, 'PackSize'), Quantity: netD(col(row, 'Qty')), WeightCut: col(row, 'WeightCut'),
                    BillQty: netD(col(row, 'BillQty')), BillWeight: netD(col(row, 'BillWeight')), RateWithoutAddLess: col(row, 'WageRate'),
                    RateAddLess: netD(col(row, 'RateAddLess')), Rate: col(row, 'WageRate'), Amount: netD(col(row, 'WagesAmount')),
                    ItemId: col(row, 'ItemId'), Item: col(row, 'ItemName'), jobLotId: col(row, 'JobLotId'), jobLot: col(row, 'JobLotDescription'),
                    Crop: col(row, 'Crop'), MoveFromId: col(row, 'WareHouseFromId'), MoveFrom: col(row, 'WareHouseFrom'), MoveToId: col(row, 'WareHouseToId'),
                    MoveTo: col(row, 'WareHouseTo'), PurchaseGLAC: col(row, 'PurchaseGLAC'), WarehouseType: col(row, 'WarehouseType'),
                    RefDocQty: netD(col(row, 'RefDocQty')), RefDocWeight: netD(col(row, 'RefDocWeight')), RefLineId: netI(col(row, 'RefLineId')),
                    WagesScheduleId: col(row, 'InvContractorWagesScheduleId'), TransactionType: col(row, 'TransactionType')
                };
                (netI(col(row, 'WagesTypeId')) === 2 ? ST : WG).push(t);
            });
        }).catch(function (e) { throw new Error('Error loading Wages details for ID ' + id + (e && e.message ? ': ' + e.message : '')); });
    }

    // ============================================================================ rendering

    /* Column spec: [key, caption, fmt, total?, editor?] - editor: 'num' | 'text' | fn(row)->options list [[v,t],...] */
    function cellHtml(r, i, c, editable) {
        var k = c[0], fmt = c[2], ed = editable ? c[4] : null, v = r[k];
        var cls = (fmt !== 't') ? ' class="num' + (ed ? ' ed' : '') + '"' : (ed ? ' class="ed"' : '');
        if (typeof ed === 'function') {
            var opts = ed(r) || [], cur = String(v === null || v === undefined ? '' : v), found = false;
            var html = opts.map(function (o) { var s = String(o[0]) === cur; if (s) found = true; return '<option value="' + esc(o[0]) + '"' + (s ? ' selected' : '') + '>' + esc(o[1]) + '</option>'; }).join('');
            if (!found && cur !== '' && cur !== '0') html = '<option value="' + esc(cur) + '" selected>' + esc((c[5] ? c[5](r) : '') || cur) + '</option>' + html;
            return '<td class="ed"><select data-i="' + i + '" data-k="' + esc(k) + '"><option value="0"></option>' + html + '</select></td>';
        }
        var disp = c[5] ? c[5](r) : (fmt === 't' ? (v === null || v === undefined ? '' : v) : F(fmt, v));
        if (ed === 'num' || ed === 'text') {
            return '<td' + cls + '><input type="text" data-i="' + i + '" data-k="' + esc(k) + '"' + (ed === 'num' ? ' class="num"' : '') + ' value="' + esc(disp) + '"/></td>';
        }
        return '<td' + cls + '>' + esc(disp) + (c[6] ? '<button type="button" class="sc-f1" data-act="f1" data-k="' + esc(k) + '" data-i="' + i + '" title="F1">&hellip;</button>' : '') + '</td>';
    }
    function drawGrid(o) {
        var cols = o.cols.filter(function (c) { return !c.hidden; });
        $id(o.head).innerHTML = (o.preHead || '') + cols.map(function (c) { return '<th' + (c[2] !== 't' ? ' class="num"' : '') + '>' + esc(c[1]) + '</th>'; }).join('') + (o.postHead || '');
        var order = o.rows.map(function (r, i) { return i; });
        var html = '', lastGroup = null, preCount = ((o.preHead || '').match(/<th/g) || []).length, postCount = ((o.postHead || '').match(/<th/g) || []).length;
        if (o.groupBy) {
            order.sort(function (a, b) { var x = String(o.rows[a][o.groupBy]), y = String(o.rows[b][o.groupBy]); return x < y ? -1 : x > y ? 1 : a - b; });
        }
        order.forEach(function (i) {
            var r = o.rows[i];
            if (o.groupBy && String(r[o.groupBy]) !== lastGroup) {
                lastGroup = String(r[o.groupBy]);
                html += '<tr class="sc-group"><td colspan="' + (cols.length + preCount + postCount) + '">' + esc(o.groupCaption || o.groupBy) + ': ' + esc(lastGroup) + '</td></tr>';
            }
            var edit = o.editable ? o.editable(r, i) : false;
            html += '<tr tabindex="-1" data-i="' + i + '"' + (o.rowClass ? ' class="' + o.rowClass(r, i) + (CUR[o.key] === i ? ' is-current' : '') + '"' : (CUR[o.key] === i ? ' class="is-current"' : ''))
                  + '>' + (o.pre ? o.pre(r, i) : '') + cols.map(function (c) { return cellHtml(r, i, c, edit && c[4]); }).join('') + (o.post ? o.post(r, i) : '') + '</tr>';
        });
        $id(o.body).innerHTML = html;
        var any = cols.some(function (c) { return c[3]; });
        $id(o.foot).innerHTML = (o.rows.length && any) ? '<tr>' + (preCount ? '<td colspan="' + preCount + '"></td>' : '') + cols.map(function (c) {
            if (!c[3]) return '<td></td>';
            return '<td class="num">' + esc(F(c[2] === 't' ? 'raw' : c[2], o.rows.reduce(function (a, r) { return a + netD(r[c[0]]); }, 0))) + '</td>';
        }).join('') + (postCount ? '<td colspan="' + postCount + '"></td>' : '') + '</tr>' : '';
    }
    function opts(list, idKey, textKey) { return function () { return (list() || []).map(function (x) { return [col(x, idKey), col(x, textKey)]; }); }; }
    function slabOptions() { return ((LK && LK.moistureSlabs) || []).map(function (x) { return [col(x, 'Id'), col(x, 'MoistureSlabDescription')]; }); }

    function inputCols() {
        var qEd = ES.issuanceByLoader ? 'num' : null;
        return [['EntryType', 'EntryType', 't'], ['WareHouse', 'WareHouse', 't'], ['CropYear', 'CropYear', 't'], ['Item', 'Item', 't'],
            ['JobLot', 'JobLot', 't'], ['UOM', 'UOM', 't'], ['PackingType', 'PackingType', 't'], ['BalQty', 'BalQty', 'q', 1],
            ['BalWeight', 'BalWeight', 'q', 1], ['Quantity', 'Quantity', 'q', 1, qEd], ['Weight', 'Weight', 'q', 1, qEd], ['Rate', 'Rate', 'r'],
            ['RateUOM', 'RateUOM', 't'], ['Amount', 'Amount', 'a', 1], ['Moisture', 'Moisture', 't', 0, 'text'],
            ['MoistureSlabId', 'Moisture Slab Description', 't', 0, slabOptions, function (r) { return slabText(r.MoistureSlabId); }],
            ['Remarks', 'Remarks', 't', 0, 'text']];
    }
    function renderInput() {
        var allChecked = INPUT.length > 0 && INPUT.every(function (r) { return r._chk; });
        drawGrid({ key: 'in', head: 'inputHead', body: 'gridInput', foot: 'inputFoot', cols: inputCols(), rows: INPUT,
            editable: function () { return true; },
            preHead: '<th><input type="checkbox" id="chkInputAll"' + (allChecked ? ' checked' : '') + ' title="Select all"></th>' + (inputNoDelete ? '' : '<th>X</th>'),
            pre: function (r, i) { return '<td style="text-align:center;"><input type="checkbox" data-act="chk" data-i="' + i + '"' + (r._chk ? ' checked' : '') + '></td>' + (inputNoDelete ? '' : '<td><button type="button" class="sc-x" data-act="del" data-i="' + i + '">X</button></td>'); } });
    }
    function outputCols() {
        var t = convType();
        var qEd = t !== 5 ? 'num' : null, rEd = (t === 3) ? 'num' : null;
        var c = [['EntryType', 'EntryType', 't'], ['WareHouse', 'WareHouse', 't'], ['CropYear', 'CropYear', 't'], ['Item', 'Item', 't'],
            ['JobLot', 'JobLot', 't'], ['UOM', 'UOM', 't'], ['PackingType', 'PackingType', 't'], ['Quantity', 'Quantity', 'q', 1, qEd],
            ['Weight', 'Weight', 'q', 1, qEd], ['Rate', 'Rate', 'r', 0, rEd], ['RateUOM', 'RateUOM', 't'], ['AmountWithoutExpenses', 'AmountWithoutExpenses', 'a', 1],
            ['Moisture', 'Moisture', 't'], ['MoistureSlabDescription', 'MoistureSlabDescription', 't'],
            ['PMAmount', 'General Packing Material Amount', 'a', 1], ['ItemPMAmount', 'Item Packing Material Amount', 'a', 1],
            ['ExpAmount', 'General Overhead Amount', 'a', 1], ['ItemExpAmount', 'Item Overhead Amount', 'a', 1], ['Amount', 'Amount', 'a', 1],
            ['Remarks', 'Remarks', 't'], ['IssueWeight', 'IssueWeight', 'raw']];
        var wg = ['WagesAmount', 'WagesAmount', 'a', 1];
        if (!ES.contractWagesChargeToProduct) wg.hidden = true;
        c.push(wg);
        return c;
    }
    function renderOutput() {
        drawGrid({ key: 'out', head: 'outputHead', body: 'gridOutput', foot: 'outputFoot', cols: outputCols(), rows: OUTPUT,
            editable: function () { return true; },
            preHead: '<th>X</th>', pre: function (r, i) { return '<td><button type="button" class="sc-x" data-act="del" data-i="' + i + '">X</button></td>'; } });
    }
    function pmCols() {
        var fifo = !!ES.fifoCgs;
        return [['ItemId', 'Item', 't', 0, opts(function () { return PM_ITEMS; }, 'Id', 'ItemName'), function (r) { return listText(PM_ITEMS, 'Id', 'ItemName', r.ItemId); }],
            ['WareHouse', 'WareHouse', 't', 0, null, null, true], ['RackName', 'RackName', 't', 0, null, null, true],
            ['ItemConditionId', 'Item Condition', 't', 0, opts(function () { return ES.itemConditions; }, 'Id', 'Description'), function (r) { return listText(ES.itemConditions, 'Id', 'Description', r.ItemConditionId); }],
            ['BrandItem', 'BrandItem', 't', 0, null, null, true], ['BrandUom', 'BrandUom', 't', 0, null, null, true],
            ['ItemQTY', 'ItemQTY', 'q', 1, 'num'], ['ItemRate', 'ItemRate', 'r', 0, fifo ? null : 'num'], ['Amount', 'Amount', 'a', 1, fifo ? null : 'num'],
            ['ChargeTo', 'Charge To', 't', 0, opts(function () { return ES.chargeTo; }, 'Id', 'ChargeTo'), function (r) { return chargeText(r.ChargeTo); }],
            ['ContractScheduleId', 'Schedule / Invoice No', 't', 0, opts(function () { return SCHEDULES; }, 'Id', 'ScheduleCode'), function (r) { return listText(SCHEDULES, 'Id', 'ScheduleCode', r.ContractScheduleId); }]];
    }
    function renderPm() {
        drawGrid({ key: 'pm', head: 'pmHead', body: 'gridPacking', foot: 'pmFoot', cols: pmCols(), rows: PMR, editable: function () { return true; },
            preHead: '<th>X</th><th>+</th>',
            pre: function (r, i) { return '<td><button type="button" class="sc-x" data-act="del" data-i="' + i + '">X</button></td><td><button type="button" class="sc-x" data-act="add" data-i="' + i + '">+</button></td>'; } });
    }
    function ohCols() {
        return [['ChartofAccount', 'Item', 't', 0, opts(function () { return ES.overheadAccounts; }, 'Id', 'AccountTitle'), function (r) { return listText(ES.overheadAccounts, 'Id', 'AccountTitle', r.ChartofAccount); }],
            ['BrandItem', 'BrandItem', 't', 0, null, null, true], ['BrandUom', 'BrandUom', 't', 0, null, null, true],
            ['LedgerRemarks', 'LedgerRemarks', 't', 0, 'text'], ['ExpAmount', 'ExpAmount', 'a', 1, 'num'],
            ['ChargeTo', 'ChargeTo', 't', 0, opts(function () { return ES.chargeTo; }, 'Id', 'ChargeTo'), function (r) { return chargeText(r.ChargeTo); }]];
    }
    function renderOh() {
        drawGrid({ key: 'oh', head: 'ohHead', body: 'gridExpense', foot: 'ohFoot', cols: ohCols(), rows: OHR, editable: function () { return true; },
            preHead: '<th>X</th><th>+</th>',
            pre: function (r, i) { return '<td><button type="button" class="sc-x" data-act="del" data-i="' + i + '">X</button></td><td><button type="button" class="sc-x" data-act="add" data-i="' + i + '">+</button></td>'; } });
    }
    function wagesCols(which) {
        var al = !!ES.enableAddLessOnWagesRegular, st = which === 'st';
        var fmtQ = st ? 'raw' : 'q4';
        var accounts = st ? function () { return ES.stitchingAccounts; } : function () { return ES.wagesAccounts; };
        var c = [['SupplierId', 'Contractor Name', 't', 0, opts(function () { return ES.contractors; }, 'Id', 'CompanyName'), function (r) { return listText(ES.contractors, 'Id', 'CompanyName', r.SupplierId); }],
            ['WagesId', 'Labour / Wages Activity', 't', 0, opts(accounts, 'Id', 'WagesAccountName'), function (r) { return listText(accounts(), 'Id', 'WagesAccountName', r.WagesId); }],
            ['WagesType', 'WagesType', 't'], ['packingType', 'packingType', 't'], ['Weight', 'Weight', fmtQ, 1, 'num'], ['PackSize', 'PackSize', 'raw'],
            ['Quantity', 'Quantity', fmtQ, 1, 'num'], ['WeightCut', 'WeightCut', st ? 'raw' : 'q', 0, 'num'], ['BillWeight', 'BillWeight', fmtQ, 1],
            ['RateWithoutAddLess', 'RateWithoutAddLess', st ? 'raw' : 'q4'], ['RateAddLess', 'RateAddLess', st ? 'raw' : 'q4', st ? 1 : 0, al ? 'num' : null],
            ['Rate', 'Rate', st ? 'raw' : 'q4'], ['Amount', 'Amount', st ? 'raw' : 'q4', 1], ['Item', 'Item', 't'], ['jobLot', 'jobLot', 't'],
            ['Crop', 'Crop', 't'], ['MoveFrom', 'MoveFrom', 't'], ['RefLineId', 'RowNo', 'raw']];
        if (!al) { c[9].hidden = true; c[10].hidden = true; }
        return c;
    }
    function renderWages() {
        ['wg', 'st'].forEach(function (w) {
            var st = w === 'st';
            drawGrid({ key: w, head: st ? 'stitchHead' : 'wagesHead', body: st ? 'gridStitch' : 'gridWages', foot: st ? 'stitchFoot' : 'wagesFoot',
                cols: wagesCols(w), rows: wagesTable(w), editable: function () { return true; }, groupBy: 'TransactionType', groupCaption: 'TransactionType',
                rowClass: function (r) { return String(r.WagesType) === 'Free Of Cost' ? 'sc-free' : ''; },
                postHead: '<th>Add</th><th>X</th>',
                post: function (r, i) { return '<td><button type="button" class="sc-x" data-act="add" data-i="' + i + '">Add</button></td><td><button type="button" class="sc-x" data-act="del" data-i="' + i + '">X</button></td>'; } });
        });
    }
    function renderAll() { renderInput(); renderOutput(); renderPm(); renderOh(); renderWages(); renderSummary(); }

    /* Event delegation per grid body. */
    function wireGrid(bodyId, key, h) {
        var b = $id(bodyId);
        b.addEventListener('change', function (e) {
            var t = e.target;
            if (t.getAttribute('data-act') === 'chk') { INPUT[+t.getAttribute('data-i')]._chk = t.checked; renderInput(); return; }
            var k = t.getAttribute('data-k');
            if (!k || !h.edit) return;
            h.edit(+t.getAttribute('data-i'), k, t.value);
        });
        b.addEventListener('click', function (e) {
            var tr = e.target.closest('tr[data-i]');
            if (tr) { CUR[key] = +tr.getAttribute('data-i'); b.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); }); }
            var a = e.target.closest('[data-act]');
            if (!a || a.getAttribute('data-act') === 'chk') return;
            var i = +a.getAttribute('data-i');
            if (a.getAttribute('data-act') === 'f1') { if (h.f1) h.f1(i, a.getAttribute('data-k')); return; }
            if (h.act) h.act(i, a.getAttribute('data-act'));
        });
        b.addEventListener('focusin', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) CUR[key] = +tr.getAttribute('data-i'); });
        b.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && h.dbl && !e.target.closest('input,select,button')) h.dbl(+tr.getAttribute('data-i')); });
        b.addEventListener('keydown', function (e) {
            var tr = e.target.closest('tr[data-i]');
            if (!tr) return;
            var i = +tr.getAttribute('data-i'), cell = e.target.closest('td');
            var inner = cell ? cell.querySelector('[data-k]') : null;
            var colKey = (e.target.getAttribute && e.target.getAttribute('data-k')) || (inner ? inner.getAttribute('data-k') : '');
            if (e.ctrlKey && e.key === 'Delete' && h.keyDel) { e.preventDefault(); h.keyDel(i); }
            else if (e.ctrlKey && (e.key === 'd' || e.key === 'D') && h.keyAdd) { e.preventDefault(); h.keyAdd(i); }
            else if (e.key === 'F1' && h.f1 && colKey) { e.preventDefault(); h.f1(i, colKey); }
        });
    }

    // ============================================================= Issuance loader (modal)

    var LD = { rows: [], setup: null, open: false };
    /** btnIssuanceLoad_Click:6600. */
    function btnIssuanceLoad() {
        if (convType() === 0) { $id('CmbConversionType').focus(); box('Please Select Conversion Type'); return; }
        LD = { rows: [], setup: null, open: true, singleItem: convType() === 2 };
        $id('loaderModal').classList.add('is-open');
        $id('loaderHead').innerHTML = ''; $id('gridLoader').innerHTML = ''; $id('loaderFoot').innerHTML = '';
        setVal('ldTo', today()); setVal('ldFrom', today()); setVal('ldSelWeight', ''); setVal('ldSelQty', '');
        return busy('btnIssuanceLoad', function () {
            return getJson(api + '/loader/setup').then(function (d) {
                LD.setup = d || {};
                if (LD.setup.loadError) { box(LD.setup.loadError); return; }
                if (LD.setup.fromDate) setVal('ldFrom', dateOnly(LD.setup.fromDate));
                var l = LD.setup.lists || {};
                fill('ldParent', l.ParentCategories, 'Id', 'name'); fill('ldItemCategory', l.ItemCategories, 'Id', 'name');
                fill('ldItemType', l.ItemTypes, 'Id', 'name'); fill('ldJobLot', l.JobLot, 'Id', 'name'); fill('ldCrop', l.CropYear, 'Id', 'name');
                fill('ldWarehouse', l.Warehouse, 'Id', 'name'); fill('ldRefDoc', l.DocumentType, 'Id', 'name'); fill('ldParty', l.Supplier_Customer, 'Id', 'name');
                fill('ldItem', l.Items, 'Id', 'name');
                refreshCombos();
                $id('ldFrom').focus();
                return loaderSearch();
            }).catch(function (e) { box(e.message); });
        });
    }
    /** PendingInventoryTransactionsForIssuanceLoad:319. */
    function loaderSearch() {
        if (!LD.setup || LD.setup.loadError) return;
        return busy('btngrnlod', function () {
            return getJson(api + '/loader/search' + qs({ fromDate: val('ldFrom'), toDate: val('ldTo'), parentCategoryId: netI(val('ldParent')),
                itemCategoryId: netI(val('ldItemCategory')), itemTypeId: netI(val('ldItemType')), jobLotId: netI(val('ldJobLot')),
                cropYear: selText('ldCrop'), warehouseId: netI(val('ldWarehouse')), refDocumentTypeId: netI(val('ldRefDoc')),
                supplierCustomerId: netI(val('ldParty')), itemId: netI(val('ldItem')) }))
                .then(function (rows) { LD.rows = (rows || []).map(function (r) { r._chk = false; return r; }); drawLoader(); })
                .catch(function (e) { box(e.message); });
        });
    }
    var LD_COLS = [['RefDocumentType', 'RefDocumentType', 't'], ['DocDate', 'DocDate', 'd'], ['DocCodeNo', 'DocCodeNo', 't'], ['ManualNo', 'ManualNo', 't'],
        ['GrnNo', 'GrnNo', 't'], ['SupplierCustomerName', 'SupplierCustomerName', 't'], ['VehicleNo', 'VehicleNo', 't'], ['BiltyNo', 'BiltyNo', 't'],
        ['GpNo', 'GpNo', 't'], ['WareHouseCode', 'WareHouseCode', 't'], ['RefWarehouse', 'RefWarehouse', 't'], ['ItemName', 'ItemName', 't'],
        ['ItemCode', 'ItemCode', 't'], ['CropBatch', 'CropBatch', 't'], ['JobLotCode', 'JobLotCode', 't'], ['PackingType', 'PackingType', 't'],
        ['PackUom', 'PackUom', 't'], ['QtyIn', 'QtyIn', 'q2', 1], ['QtyOut', 'QtyOut', 'q2', 1], ['QtyBalance', 'QtyBalance', 'q2', 1],
        ['WeightIn', 'WeightIn', 'q2', 1], ['WeightOut', 'WeightOut', 'q2', 1], ['WeightBalance', 'WeightBalance', 'q2', 1],
        ['AVgRate', 'AVgRate', 'r', 0, 'rate'], ['RateUom', 'RateUom', 't'], ['ItemAmount', 'ItemAmount', 'a', 1, 'rate'], ['Remarks', 'Remarks', 't']];
    function drawLoader() {
        var showValues = !!(LD.setup && LD.setup.valuesShowRights);
        var cols = LD_COLS.filter(function (c) { return showValues || c[4] !== 'rate'; });
        if (!LD.rows.length) { $id('loaderHead').innerHTML = ''; $id('gridLoader').innerHTML = ''; $id('loaderFoot').innerHTML = ''; return; }
        var all = LD.rows.every(function (r) { return r._chk; });
        $id('loaderHead').innerHTML = '<th><input type="checkbox" id="ldAll"' + (all ? ' checked' : '') + '> Select</th>' + cols.map(function (c) { return '<th' + (c[2] !== 't' && c[2] !== 'd' ? ' class="num"' : '') + '>' + esc(c[1]) + '</th>'; }).join('');
        $id('gridLoader').innerHTML = LD.rows.map(function (r, i) {
            return '<tr data-i="' + i + '"><td style="text-align:center;"><input type="checkbox" data-i="' + i + '"' + (r._chk ? ' checked' : '') + '></td>' + cols.map(function (c) {
                var v = col(r, c[0]);
                if (c[2] === 'd') { var p = dateOnly(v); v = p ? p.slice(8, 10) + '/' + p.slice(5, 7) + '/' + p.slice(0, 4) : v; }
                else if (c[2] !== 't') v = F(c[2], v);
                return '<td' + (c[2] !== 't' && c[2] !== 'd' ? ' class="num"' : '') + '>' + esc(v) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        $id('loaderFoot').innerHTML = '<tr><td></td>' + cols.map(function (c) {
            return c[3] ? '<td class="num">' + esc(F(c[2], LD.rows.reduce(function (a, r) { return a + netD(col(r, c[0])); }, 0))) + '</td>' : '<td></td>';
        }).join('') + '</tr>';
        K.filterRow($id('tblLoader'));
        selectedWeightCalculation();
    }
    /** SelectedWeightCalculation:636 - Math.Round(x).ToString("0,0"). */
    function selectedWeightCalculation() {
        var w = 0, q = 0;
        LD.rows.forEach(function (r) { if (r._chk) { w += netD(col(r, 'WeightBalance')); q += netD(col(r, 'QtyBalance')); } });
        setVal('ldSelWeight', K.pad2(rnd(w, 0))); setVal('ldSelQty', K.pad2(rnd(q, 0)));
    }
    /** btnReset_Click:552. */
    function loaderReset() { setSel('ldParty', 0); setSel('ldItem', 0); LD.rows = []; drawLoader(); refreshCombos(); $id('ldParent').focus(); }
    /** btnclose_Click:560 / Esc - nothing is loaded. */
    function loaderClose() { LD.open = false; $id('loaderModal').classList.remove('is-open'); }
    /** btnLoadOnInvoice_Click_1:566 then LoadDataDetailfromPurchaseInvoivce:6626. */
    function loaderLoad() {
        if (!LD.setup || LD.setup.loadError) { box('Input array is longer than the number of columns in this table.'); return; }
        var sel = LD.rows.filter(function (r) { return r._chk; });
        if (sel.length === 0) { box('Please Select Row first'); return; }
        if (LD.singleItem) {
            var first = netI(col(sel[0], 'ItemId'));
            if (!sel.every(function (r) { return netI(col(r, 'ItemId')) === first; })) { box('Please select rows with the same ItemId.'); return; }
        }
        loaderClose();
        try {
            var existing = {};
            INPUT.forEach(function (r) { existing[netI(r.RefDocSubId)] = 1; });
            var maxLine = INPUT.length ? Math.max.apply(null, INPUT.map(function (r) { return netI(r.LineId); })) : 0;
            sel.forEach(function (d) {
                var sub = netI(col(d, 'RefDocSubIdNo'));
                if (existing[sub]) return;
                maxLine++;
                INPUT.push({ Id: 0, RefDocumentTypeId: netI(col(d, 'RefDocumentTypeId')), RefDocNoId: netI(col(d, 'RefDocIdNo')), RefDocSubId: sub,
                    EntryType: 'Issue', WareHouseId: netI(col(d, 'WarehouseId')), WareHouse: String(col(d, 'WareHouseCode')), CropYear: String(col(d, 'CropBatch')),
                    ItemId: netI(col(d, 'ItemId')), Item: String(col(d, 'ItemName')), JobLotId: netI(col(d, 'JobLotId')), JobLot: String(col(d, 'JobLotCode')),
                    ItemUOMId: netI(col(d, 'ItemUom')), UOM: col(d, 'PackUom'), PackEquivalent: netD(col(d, 'PackSize')),
                    PackingTypeId: netI(col(d, 'InvPackingTypeId')), PackingType: String(col(d, 'PackingType')),
                    BalQty: netD(col(d, 'QtyBalance')), BalWeight: netD(col(d, 'WeightBalance')), Quantity: netD(col(d, 'QtyBalance')), Weight: netD(col(d, 'WeightBalance')),
                    Rate: netD(col(d, 'AVgRate')), RateUOMId: netI(col(d, 'RateUomId')), RateUOM: col(d, 'RateUom'), RateEquivalent: netD(col(d, 'Equivalent')),
                    Amount: netD(col(d, 'ItemAmount')), Moisture: '', MoistureSlabId: 0, Remarks: '', LineId: maxLine, labIPmActivityLogId: 0, _chk: false });
                existing[sub] = 1;
            });
            inputNoDelete = false;              // gridsettings():6665
            proportionateOverheadGrid();
            proportionatedPackingMaterialAmountInOutputGrid();
            generateSummaryForUser();
        } catch (e) { box(e.message); }
        lockConversionType();
        renderAll();
        if (ES.contractWagesChargeToProduct) afterDetail(wagesFill('in'));
    }

    // ======================================================== .NET DataTable typed Rows.Add

    /* DataRowCollection.Add(params object[]) into a TYPED DataTable: each value is stored through the
       column's storage (IConvertible.ToXxx); a value that does not convert throws ArgumentException
       "<inner message>Couldn't store <value> in <Column> Column.  Expected type is <Type>." and the row
       is not added. null -> DBNull. Missing trailing values stay DBNull. schema: [[name, 's'|'i'|'d'|'dt'], ...] */
    var NET_TYPE = { s: 'String', i: 'Int32', d: 'Double', dt: 'DateTime' };
    function dtRow(schema, values) {
        if (values.length > schema.length) throw new Error('Input array is longer than the number of columns in this table.');
        var row = {};
        schema.forEach(function (c, k) {
            var v = k < values.length ? values[k] : null, name = c[0], t = c[1];
            if (v === null || v === undefined) { row[name] = null; return; }
            var shown = typeof v === 'number' ? csStr(v) : (typeof v === 'boolean' ? (v ? 'True' : 'False') : String(v));
            function fail(inner) { throw new Error(inner + "Couldn't store <" + shown + '> in ' + name + ' Column.  Expected type is ' + NET_TYPE[t] + '.'); }
            if (t === 's') { row[name] = shown; return; }
            if (t === 'i') {
                if (typeof v === 'number') { row[name] = rnd(v, 0); return; }
                if (typeof v === 'boolean') { row[name] = v ? 1 : 0; return; }
                if (!/^\s*[+-]?\d+\s*$/.test(shown)) fail('Input string was not in a correct format.');
                row[name] = parseInt(shown, 10); return;
            }
            if (t === 'd') {
                if (typeof v === 'number') { row[name] = v; return; }
                if (typeof v === 'boolean') { row[name] = v ? 1 : 0; return; }
                if (!/^\s*[+-]?(\d[\d,]*\.?\d*|\.\d+)([eE][+-]?\d+)?\s*$/.test(shown)) fail('Input string was not in a correct format.');
                row[name] = parseFloat(shown.replace(/,/g, '')); return;
            }
            /* DateTime */
            if (!/^\s*\d{4}-\d{2}-\d{2}/.test(shown) || isNaN(new Date(shown.trim().slice(0, 19)).getTime())) fail('String was not recognized as a valid DateTime.');
            row[name] = shown.trim();
        });
        return row;
    }
    /** cell.Value.ToString() - DBNull.Value.ToString() is "". */
    function vs(v) { return (v === null || v === undefined) ? '' : (typeof v === 'number' ? csStr(v) : String(v)); }
    /** Conversion.ToString - "" for null. */
    function cs(v) { return (v === null || v === undefined) ? '' : (typeof v === 'number' ? csStr(v) : String(v)); }

    /* table (grdInput) - Load:644-674. */
    var INPUT_SCHEMA = [['Id', 'i'], ['RefDocumentTypeId', 'i'], ['RefDocNoId', 'i'], ['RefDocSubId', 'i'], ['EntryType', 's'],
        ['WareHouseId', 'i'], ['WareHouse', 's'], ['CropYear', 's'], ['ItemId', 'i'], ['Item', 's'], ['JobLotId', 'i'], ['JobLot', 's'],
        ['ItemUOMId', 'i'], ['UOM', 's'], ['PackEquivalent', 'd'], ['PackingTypeId', 'i'], ['PackingType', 's'], ['BalQty', 'd'],
        ['BalWeight', 'd'], ['Quantity', 'd'], ['Weight', 'd'], ['Rate', 'd'], ['RateUOMId', 'i'], ['RateUOM', 's'], ['RateEquivalent', 'd'],
        ['Amount', 'd'], ['Moisture', 's'], ['MoistureSlabId', 'i'], ['Remarks', 's'], ['LineId', 'i'], ['labIPmActivityLogId', 'i']];
    /* tableByProduct (grdByProduct) - Load:678-711. */
    var OUTPUT_SCHEMA = [['Id', 'i'], ['EntryType', 's'], ['WareHouseId', 'i'], ['WareHouse', 's'], ['CropYear', 's'], ['ItemId', 'i'],
        ['Item', 's'], ['JobLotId', 'i'], ['JobLot', 's'], ['ItemUOMId', 'i'], ['UOM', 's'], ['PackEquivalent', 'd'], ['PackingTypeId', 'i'],
        ['PackingType', 's'], ['Quantity', 'd'], ['Weight', 'd'], ['Rate', 'd'], ['RateUOMId', 'i'], ['RateUOM', 's'], ['RateEquivalent', 'd'],
        ['AmountWithoutExpenses', 'd'], ['Moisture', 's'], ['MoistureSlabId', 'i'], ['MoistureSlabDescription', 's'], ['PMAmount', 'd'],
        ['ItemPMAmount', 'd'], ['ExpAmount', 'd'], ['ItemExpAmount', 'd'], ['Amount', 'd'], ['Remarks', 's'], ['IssueWeight', 'd'],
        ['LineId', 'i'], ['WagesAmount', 'd'], ['labIPmActivityLogId', 'i']];

    // ============================== LoadavailableTransactionsForStockReleaseFromFumigation (modal)

    /* dtIssuance - LoadInvoices_Load:163-196 (33 columns; untyped ones are String). */
    var FUM_DT_SCHEMA = [['RefDocumentTypeId', 's'], ['RefDocumentType', 's'], ['RefDocIdNo', 's'], ['RefDocSubIdNo', 's'],
        ['SupplierCustomerId', 'i'], ['PartyName', 's'], ['DocDate', 'dt'], ['DocNo', 'i'], ['WareHouseId', 's'], ['WareHouse', 's'],
        ['ItemId', 's'], ['ItemName', 's'], ['ItemCode', 's'], ['ItemUOMId', 's'], ['PackUom', 's'], ['ItemUOM', 's'], ['CropYearId', 's'],
        ['CropYear', 's'], ['JobLotId', 's'], ['JobLotCode', 's'], ['PackingTypeId', 's'], ['PackTypeCode', 's'], ['BalQty', 'd'],
        ['BalWeight', 'd'], ['ItemQty', 'd'], ['Weight', 'd'], ['ItemRate', 's'], ['RateUOMId', 's'], ['RateUOM', 's'], ['Equivalent', 's'],
        ['ItemAmount', 'd'], ['Remarks', 's'], ['labIPmActivityLogId', 'i']];
    /* grdSettings:438 - the visible columns in their order (BiltyNo moved after VehicleNo; the hidden ones
       dropped). [key, fmt, total?, valuesRight?]. ConfigureNumericalColumn: "Amount" -> stringFormatsingle
       + Sum; "Rate" -> DecimalRateFormate; other decimals "#,##0.##" + Sum. */
    var FUM_COLS = [['RefDocumentType', 't'], ['DocDate', 'd'], ['DocCodeNo', 't'], ['GrnNo', 't'], ['SupplierCustomerName', 't'],
        ['VehicleNo', 't'], ['BiltyNo', 't'], ['GpNo', 't'], ['GPDate', 'd'], ['WareHouseCode', 't'], ['ItemName', 't'], ['ItemCode', 't'],
        ['CropBatch', 't'], ['JobLotCode', 't'], ['PackingType', 't'], ['PackUom', 't'], ['QtyIn', 'q2', 1], ['QtyOut', 'q2', 1],
        ['QtyBalance', 'q2', 1], ['WeightIn', 'q2', 1], ['WeightOut', 'q2', 1], ['WeightBalance', 'q2', 1], ['AVgRate', 'r', 0, 1],
        ['RateUom', 't'], ['ItemAmount', 'a', 1, 1], ['Remarks', 't'], ['IPMJobLot', 't'], ['StepDescription', 't'], ['qcActivityDate', 'D'],
        ['StatusDescription', 't'], ['nextActivityPlanDate', 'D'], ['fumigatedBy', 't'], ['checkBy', 't'], ['verifiedBy', 't'],
        ['ReleaseHoldStatus', 't'], ['qcActivityDescription', 't']];
    var FM = { open: false, rows: [], setup: null, dt: [], hasColumns: false };
    function fumOpen() { return FM.open; }

    /** btnStockReleaseFromFumigation_Click:7710. */
    function btnStockReleaseFromFumigation() {
        try {
            if (convType() === 0) { $id('CmbConversionType').focus(); throw new Error('Please Select Conversion Type'); }
        } catch (e) { box(e.message); return; }
        /* new LoadavailableTransactionsForStockReleaseFromFumigation(UserAccount): AvailableForFumigation,
           JobOrderItems and CanLoadOnlyPurchaseAndSingleItemRows are left at 0 / "" / false by the caller. */
        FM = { open: true, rows: [], setup: null, dt: [], hasColumns: false };
        $id('fumModal').classList.add('is-open');
        $id('fumHead').innerHTML = ''; $id('gridFum').innerHTML = ''; $id('fumFoot').innerHTML = '';
        setVal('fmTo', today()); setVal('fmFrom', today()); setVal('fmSelWeight', ''); setVal('fmSelQty', '');
        ['fmParent', 'fmItemCategory', 'fmItemType', 'fmJobLot', 'fmCrop', 'fmWarehouse', 'fmRefDoc', 'fmParty', 'fmItem'].forEach(function (id) { fill(id, [], 'Id', 'name'); });
        refreshCombos();
        /* LoadInvoices_Load:133 - the same rights read, FromDate and StockComboFill as the Issuance loader. */
        return busy('btnStockReleaseFromFumigation', function () {
            return getJson(api + '/loader/setup').then(function (d) {
                FM.setup = d || {};
                if (FM.setup.loadError) { box(FM.setup.loadError); return; }       // Load stops: no columns, combos or search
                if (FM.setup.fromDate) setVal('fmFrom', dateOnly(FM.setup.fromDate));
                $id('fmParent').focus();
                FM.hasColumns = true;
                var l = FM.setup.lists || {};
                fill('fmParent', l.ParentCategories, 'Id', 'name'); fill('fmItemCategory', l.ItemCategories, 'Id', 'name');
                fill('fmItemType', l.ItemTypes, 'Id', 'name'); fill('fmJobLot', l.JobLot, 'Id', 'name'); fill('fmCrop', l.CropYear, 'Id', 'name');
                fill('fmWarehouse', l.Warehouse, 'Id', 'name'); fill('fmRefDoc', l.DocumentType, 'Id', 'name'); fill('fmParty', l.Supplier_Customer, 'Id', 'name');
                fill('fmItem', l.Items, 'Id', 'name');
                refreshCombos();
                return fumSearchNow().then(function () { $id('fmFrom').focus(); });
            }).catch(function (e) { box(e.message); });
        });
    }
    /** btngrnlod_Click / PendingInventoryTransactionsForIssuanceLoad:325 - labIPmActivityLog.HoldStockForFumigation. */
    function fumSearch() { if (!FM.open) return; return busy('fmSearch', fumSearchNow); }
    function fumSearchNow() {
        return getJson(api + '/fumigation/search' + qs({ fromDate: val('fmFrom'), toDate: val('fmTo'), parentCategoryId: netI(val('fmParent')),
            itemCategoryId: netI(val('fmItemCategory')), itemTypeId: netI(val('fmItemType')), jobLotId: netI(val('fmJobLot')),
            cropYear: selText('fmCrop'), warehouseId: netI(val('fmWarehouse')), refDocumentTypeId: netI(val('fmRefDoc')),
            supplierCustomerId: netI(val('fmParty')), itemId: netI(val('fmItem')) }))
            .then(function (rows) { FM.rows = (rows || []).map(function (r) { r._chk = false; return r; }); drawFum(); })
            .catch(function (e) { box(e.message); });
    }
    function fumCell(c, v) {
        if (c[1] === 'd') { var p = dateOnly(v); return p ? p.slice(8, 10) + '/' + p.slice(5, 7) + '/' + p.slice(0, 4) : (v === null || v === undefined ? '' : String(v)); }
        if (c[1] === 'D') return (v === null || v === undefined || v === '') ? '' : K.dMMMyyyy(v);       // "dd-MMM-yyyy"
        if (c[1] === 't') return v === null || v === undefined ? '' : String(v);
        return (v === null || v === undefined || v === '') ? '' : F(c[1], v);
    }
    /** grd.DataSource = dtcol; grdSettings:438 (Select column first, frozen; header selector; filter row). */
    function drawFum() {
        var showValues = !!(FM.setup && FM.setup.valuesShowRights);
        var cols = FUM_COLS.filter(function (c) { return showValues || !c[3]; });
        if (!FM.rows.length) { $id('fumHead').innerHTML = ''; $id('gridFum').innerHTML = ''; $id('fumFoot').innerHTML = ''; return; }
        var all = FM.rows.every(function (r) { return r._chk; });
        $id('fumHead').innerHTML = '<th><input type="checkbox" id="fmAll"' + (all ? ' checked' : '') + '> Select</th>'
            + cols.map(function (c) { return '<th' + (/^(q2|r|a)$/.test(c[1]) ? ' class="num"' : '') + '>' + esc(c[0]) + '</th>'; }).join('');
        $id('gridFum').innerHTML = FM.rows.map(function (r, i) {
            return '<tr data-i="' + i + '" tabindex="-1"><td style="text-align:center;"><input type="checkbox" data-i="' + i + '"' + (r._chk ? ' checked' : '') + '></td>'
                + cols.map(function (c) { return '<td' + (/^(q2|r|a)$/.test(c[1]) ? ' class="num"' : '') + '>' + esc(fumCell(c, col(r, c[0]))) + '</td>'; }).join('') + '</tr>';
        }).join('');
        $id('fumFoot').innerHTML = '<tr><td></td>' + cols.map(function (c) {
            return c[2] ? '<td class="num">' + esc(F(c[1], FM.rows.reduce(function (a, r) { return a + netD(col(r, c[0])); }, 0))) + '</td>' : '<td></td>';
        }).join('') + '</tr>';
        K.filterRow($id('tblFum'));
    }
    /** SelectedWeightCalculation:701 - Math.Round(x).ToString("0,0"). */
    function fumSelectedWeight() {
        var w = 0, q = 0;
        FM.rows.forEach(function (r) { if (r._chk) { w += netD(col(r, 'WeightBalance')); q += netD(col(r, 'QtyBalance')); } });
        setVal('fmSelWeight', K.pad2(rnd(w, 0))); setVal('fmSelQty', K.pad2(rnd(q, 0)));
    }
    /** btnReset_Click:590 - Party and Item cleared, the grid structure cleared; the rest stays. */
    function fumReset() {
        $id('fmParent').focus();
        setSel('fmParty', 0); setSel('fmItem', 0);
        FM.rows = []; drawFum(); refreshCombos();
    }
    /** btnLoadOnInvoice_Click_1:604, AvailableForFumigation == 0 branch (CanLoadOnlyPurchaseAndSingleItemRows false). */
    function fumLoad() {
        if (!FM.open) return;
        try {
            var sel = FM.rows.filter(function (r) { return r._chk; });
            if (sel.length === 0) { box('Please Select Row first'); return; }
            sel.forEach(function (r) {
                /* Load stopped before the 33 columns were added (loadError): Rows.Add on a table with no columns. */
                if (!FM.hasColumns) throw new Error('Input array is longer than the number of columns in this table.');
                FM.dt.push(dtRow(FUM_DT_SCHEMA, [vs(col(r, 'RefDocumentTypeId')), vs(col(r, 'RefDocumentType')), vs(col(r, 'RefDocIdNo')),
                    vs(col(r, 'RefDocSubIdNo')), vs(col(r, 'SupplierCustomerId')), vs(col(r, 'SupplierCustomerName')), vs(col(r, 'DocDate')),
                    vs(col(r, 'DocCodeNo')), netI(col(r, 'WarehouseId')), vs(col(r, 'WareHouseCode')), netI(col(r, 'ItemId')),
                    vs(col(r, 'ItemName')), vs(col(r, 'ItemCode')), netI(col(r, 'ItemUom')), col(r, 'PackUom'), netD(col(r, 'PackSize')),
                    netI(col(r, 'CropYearId')), vs(col(r, 'CropBatch')), netI(col(r, 'JobLotId')), vs(col(r, 'JobLotCode')),
                    netI(col(r, 'InvPackingTypeId')), vs(col(r, 'PackingType')), netD(col(r, 'QtyBalance')), netD(col(r, 'WeightBalance')),
                    netD(col(r, 'QtyBalance')), netD(col(r, 'WeightBalance')), netD(col(r, 'AVgRate')), netI(col(r, 'RateUomId')),
                    col(r, 'RateUom'), netD(col(r, 'Equivalent')), netD(col(r, 'ItemAmount')), '', netI(col(r, 'labIPmActivityLogId'))]));
            });
        } catch (e) { box(e.message); return; }        // the dialog stays open; rows added before the failure stay in dtIssuance
        fumClose();                                     // Hide()
    }
    /** The dialog is gone (Load's Hide(), the title-bar X, Esc / Ctrl+E Close()): ShowDialog returns and the
     *  caller continues - LoadDataDetailfromFumigation(loadFumigation.dtIssuance), then the Conversion Type lock. */
    function fumClose() {
        if (!FM.open) return;
        FM.open = false;
        $id('fumModal').classList.remove('is-open');
        loadDataDetailfromFumigation(FM.dt);
        lockConversionType();
        renderAll();
    }
    /** LoadDataDetailfromFumigation:7733. */
    function loadDataDetailfromFumigation(dt) {
        try {
            if (dt.length === 0) return;
            var existing = {};
            INPUT.forEach(function (r) { existing[netI(r.RefDocSubId)] = 1; });
            var maxLineId = INPUT.length ? Math.max.apply(null, INPUT.map(function (r) { return netI(r.LineId); })) : 0;
            for (var i = 0; i < dt.length; i++) {
                var d = dt[i], sub = netI(d.RefDocSubIdNo);
                if (existing[sub]) continue;
                maxLineId++;
                var row = dtRow(INPUT_SCHEMA, [0, d.RefDocumentTypeId, d.RefDocIdNo, d.RefDocSubIdNo, 'Issue', d.WareHouseId, d.WareHouse,
                    d.CropYear, d.ItemId, d.ItemName, d.JobLotId, d.JobLotCode, d.ItemUOMId, d.PackUom, d.ItemUOM, d.PackingTypeId,
                    d.PackTypeCode, d.ItemQty, d.Weight, d.ItemQty, d.Weight, d.ItemRate, d.RateUOMId, d.RateUOM, d.Equivalent,
                    d.ItemAmount, '', 0, '', maxLineId, d.labIPmActivityLogId]);
                row._chk = false;
                INPUT.push(row);
                existing[sub] = 1;
            }
            inputNoDelete = true;                                   // gridsettings(FromFumigation: true)
            INPUT.forEach(function (r) { r._chk = true; });          // every record row IsChecked = true
            inputRowsForwardToOutput();
            proportionateOverheadGrid();
            proportionatedPackingMaterialAmountInOutputGrid();
            generateSummaryForUser();
        } catch (e) { box(e.message); }
    }

    // ================================================== frmLoadStockShortFallForSales (modal)

    /* dtGrid - PendingOrderLoad:239-262, grdSettings:285 (seven Ids hidden, GridWrappingAndColumnSettings(grd,3,3):
       decimals "#,##0.###" + Sum, "Amount" columns stringFormatsingle + Sum). */
    var SF_COLS = [['ItemName', 't'], ['ItemCode', 't'], ['WareHouseName', 't'], ['ItemUom', 't'], ['JobLot', 't'],
        ['PackingType', 't'], ['CropYear', 't'], ['AvailableQty', 'q', 1], ['AvailableWeight', 'q', 1], ['AvailableAmount', 'a', 1],
        ['SaleQty', 'q', 1], ['SaleWeight', 'q', 1], ['SaleAmount', 'a', 1], ['RequiredQty', 'q', 1], ['RequiredWeight', 'q', 1],
        ['RequiredAmount', 'a', 1]];
    var SF = { open: false, history: [], rows: [], loader: [], docDate: '' };
    function sfOpen() { return SF.open; }

    /** BtnLoadOutPut_Click:7355. */
    function btnLoadOutPut() {
        try {
            var t = convType();
            if (t === 0 || t === 3) { $id('CmbConversionType').focus(); throw new Error("You Can't Select Conversion Type [Value Gain/Loss]"); }
        } catch (e) { box(e.message); return; }
        /* new frmLoadStockShortFallForSales(UserAccount): DocDate = DateTime.Now, dtLoader an empty table. */
        SF = { open: true, history: [], rows: [], loader: [], docDate: nowIso() };
        $id('sfModal').classList.add('is-open');
        $id('sfHead').innerHTML = ''; $id('gridSf').innerHTML = ''; $id('sfFoot').innerHTML = '';
        ['sfParent', 'sfJobLot', 'sfCrop', 'sfWarehouse', 'sfItem'].forEach(function (id) { fill(id, [], 'Id', 'name'); });
        refreshCombos();
        /* LoadInvoices_Load:95 - StockComboFill, PendingOrderLoad, focus Parent Category. */
        return busy('btnLoadOutPut', function () {
            return sfComboFill().then(sfPendingOrderLoad).then(function () { $id('sfParent').focus(); });
        });
    }
    /** StockComboFill:125 (also btnRefresh_Click) - a fresh bind drops the selections. */
    function sfComboFill() {
        return getJson(api + '/shortfall/setup').then(function (d) {
            var l = (d && d.lists) || {};
            fill('sfParent', l.ParentCategories, 'Id', 'name'); fill('sfJobLot', l.JobLot, 'Id', 'name');
            fill('sfCrop', l.CropYear, 'Id', 'name'); fill('sfWarehouse', l.Warehouse, 'Id', 'name'); fill('sfItem', l.Items, 'Id', 'name');
            refreshCombos();
        }).catch(function (e) { box(e.message); });
    }
    /** PendingOrderLoad:224 - InventoryStockEvalautionDetail.StockConversion_BalanceSalesForStock. */
    function sfPendingOrderLoad() {
        SF.history = [];                                            // dtHistory.Rows.Clear()
        return getJson(api + '/shortfall/search' + qs({ docDate: SF.docDate, parentCategoryId: netI(val('sfParent')),
            itemId: netI(val('sfItem')), warehouseId: netI(val('sfWarehouse')), jobLotId: netI(val('sfJobLot')), cropYear: selText('sfCrop') }))
            .then(function (rows) {
                SF.history = rows || [];
                if (SF.history.length > 0) {
                    SF.rows = SF.history.map(function (r) {
                        var g = {};
                        ['ItemId', 'ItemName', 'ItemCode', 'WarehouseId', 'WareHouseName', 'ItemUomId', 'ItemUom', 'ItemEquivalent', 'JobLotId',
                         'JobLot', 'PackingTypeId', 'PackingType', 'CropYear', 'CropYearId', 'AvailableQty', 'AvailableWeight', 'AvailableAmount',
                         'SaleQty', 'SaleWeight', 'SaleAmount', 'RequiredQty', 'RequiredWeight', 'RequiredAmount'].forEach(function (k) { g[k] = col(r, k); });
                        g._chk = false;
                        return g;
                    });
                } else {
                    SF.rows = [];                                       // grd.ClearStructure()
                }
                drawSf();
            }).catch(function (e) { box(e.message); });           // the grid keeps what it showed
    }
    function drawSf() {
        if (!SF.rows.length) { $id('sfHead').innerHTML = ''; $id('gridSf').innerHTML = ''; $id('sfFoot').innerHTML = ''; return; }
        var all = SF.rows.every(function (r) { return r._chk; });
        $id('sfHead').innerHTML = '<th><input type="checkbox" id="sfAll"' + (all ? ' checked' : '') + '></th>'
            + SF_COLS.map(function (c) { return '<th' + (c[1] !== 't' ? ' class="num"' : '') + '>' + esc(c[0]) + '</th>'; }).join('');
        $id('gridSf').innerHTML = SF.rows.map(function (r, i) {
            return '<tr data-i="' + i + '" tabindex="-1"><td style="text-align:center;"><input type="checkbox" data-i="' + i + '"' + (r._chk ? ' checked' : '') + '></td>'
                + SF_COLS.map(function (c) {
                    var v = r[c[0]];
                    return '<td' + (c[1] !== 't' ? ' class="num"' : '') + '>' + esc(c[1] === 't' ? (v === null || v === undefined ? '' : v) : ((v === null || v === undefined || v === '') ? '' : F(c[1], v))) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        $id('sfFoot').innerHTML = '<tr><td></td>' + SF_COLS.map(function (c) {
            return c[2] ? '<td class="num">' + esc(F(c[1], SF.rows.reduce(function (a, r) { return a + netD(r[c[0]]); }, 0))) + '</td>' : '<td></td>';
        }).join('') + '</tr>';
    }
    function sfSearch() { if (!SF.open) return; return busy('sfSearch', sfPendingOrderLoad); }
    /** btnNew -> btnReset_Click:304 - focus Parent Category and search again. */
    function sfNew() { if (!SF.open) return; $id('sfParent').focus(); return busy('sfNew', sfPendingOrderLoad); }
    /** btnRefresh_Click:317 - StockComboFill. */
    function sfRefresh() { if (!SF.open) return; return busy('sfRefresh', sfComboFill); }
    /** MakeShortCutKeys:356 (the list is shown; the form itself never wires its KeyDown). */
    function sfShortcuts() {
        K.shortcuts([['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+L', 'To Press Load Button'],
                     ['Ctrl+S', 'For Search'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
                     ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }
    /** btnLoadOnInvoice_Click_1:329 - one dtHistory row per distinct Warehouse|Item|Uom|JobLot|PackingType|CropYear. */
    function sfLoad() {
        if (!SF.open) return;
        var checked = SF.rows.filter(function (r) { return r._chk; });
        if (checked.length > 0) {
            SF.loader = [];
            var seen = {};
            function key(o) { return [o.WarehouseId, o.ItemId, o.ItemUomId, o.JobLotId, o.PackingTypeId, o.CropYear].map(vs).join('|'); }
            checked.forEach(function (r) {
                var k = key(r);
                if (seen[k]) return;
                seen[k] = 1;
                var m = SF.history.filter(function (h) {
                    return key({ WarehouseId: col(h, 'WarehouseId'), ItemId: col(h, 'ItemId'), ItemUomId: col(h, 'ItemUomId'), JobLotId: col(h, 'JobLotId'),
                                 PackingTypeId: col(h, 'PackingTypeId'), CropYear: col(h, 'CropYear') }) === k;
                })[0];
                if (m) SF.loader.push(m);
            });
            sfClose();                                                  // Hide()
        } else {
            SF.loader = [];                                             // dtLoader.Clear()
            box('Check the row first');
        }
    }
    /** The dialog is gone (Load's Hide() or the title-bar X): the rest of BtnLoadOutPut_Click:7367 runs. */
    function sfClose() {
        if (!SF.open) return;
        SF.open = false;
        $id('sfModal').classList.remove('is-open');
        loadOutPutDataFromStockShortFallForSales();
        lockConversionType();
        var p = ES.contractWagesChargeToProduct ? wagesFill('out') : null;
        renderAll();
        afterDetail(p);
    }
    /** LoadOutPutDataFromStockShortFallForSales:7379 - tableByProduct.Rows.Add with THIRTY values, as written. */
    function loadOutPutDataFromStockShortFallForSales() {
        try {
            if (SF.loader.length <= 0) return;
            SF.loader.forEach(function (dr) {
                /* Value 28 is "" and lands in ItemExpAmount (Double): the desktop's Rows.Add throws there
                   ("Input string was not in a correct format.Couldn't store <> in ItemExpAmount Column.
                   Expected type is Double.") for the first row, so nothing is added. Reproduced, not fixed. */
                var row = dtRow(OUTPUT_SCHEMA, [0, 'Recovery Head Rice', netI(col(dr, 'WarehouseId')), cs(col(dr, 'WareHouseName')),
                    cs(col(dr, 'CropYear')), netI(col(dr, 'ItemId')), cs(col(dr, 'ItemName')), netI(col(dr, 'JobLotId')), cs(col(dr, 'JobLot')),
                    netI(col(dr, 'ItemUomId')), cs(col(dr, 'ItemUom')), netD(col(dr, 'ItemEquivalent')), netI(col(dr, 'PackingTypeId')),
                    cs(col(dr, 'PackingType')), netD(col(dr, 'RequiredQty')), netD(col(dr, 'RequiredWeight')), 0, netI(col(dr, 'ItemUomId')),
                    cs(col(dr, 'ItemUom')), netD(col(dr, 'ItemEquivalent')), netD(col(dr, 'RequiredAmount')), 0, 0, '', 0, 0,
                    netD(col(dr, 'RequiredAmount')), '', 0, 0]);
                OUTPUT.push(row);
            });
            proportionateOverheadGrid();
            proportionatedPackingMaterialAmountInOutputGrid();
            generateSummaryForUser();
        } catch (e) { box(e.message); }
    }

    // ============================================================ Wages Schedule / Wages Exempt

    /** btnWagesSchedule_Click:7531 - new frmContractWagesSchedule(UserAccount).Show(): the ported page
     *  (AccountsModuleViewController /accounts/vouchers/wages-rate-schedule) in a new window. */
    function btnWagesSchedule() {
        try { var w = window.open('/accounts/vouchers/wages-rate-schedule', '_blank'); if (w) w.focus(); }
        catch (e) { box(e.message); }
    }
    /** btnWagesExempt_Click:7545 - new WagesExemptItemSchedule(UserAccount).Show(), in a new window. */
    function btnWagesExempt() {
        try { var w = window.open('/production/wages-exempt-item-schedule', '_blank'); if (w) w.focus(); }
        catch (e) { box(e.message); }
    }

    // ============================================================================ Save

    function fround(x) { return Math.fround(x); }

    /** ValidationforRefRowQty:3080 / ValidationforRefRowWeight:3046. */
    function validateRefRows(table, gridName) {
        var groups = {}, order = [];
        table.forEach(function (r) {
            var byQty = !!ES.wagesAmountCalculateOnQty;
            var key = netI(r.WagesId) + '|' + netI(r.RefLineId) + '|' + netD(byQty ? r.RefDocQty : r.RefDocWeight);
            if (!groups[key]) { groups[key] = { WagesId: netI(r.WagesId), RefLineId: netI(r.RefLineId), Ref: netD(byQty ? r.RefDocQty : r.RefDocWeight), Total: 0 }; order.push(key); }
            groups[key].Total += netD(byQty ? r.Quantity : r.BillWeight);
        });
        order.forEach(function (k) {
            var g = groups[k];
            if (g.Total > g.Ref) {
                var name = listText(ES.wagesAccounts, 'Id', 'WagesAccountName', g.WagesId);
                if (ES.wagesAmountCalculateOnQty)
                    throw new Error('TotalQty against Reference RowNo and Wages Account Should be Equal to or less than Reference Row Qty\n'
                        + 'Here TotalQty (' + csStr(g.Total) + ') exceeds Reference Row Qty (' + csStr(g.Ref) + ') for WagesAccount (' + name + ') and RowNo ' + g.RefLineId + ' in ' + gridName + ' Grid');
                throw new Error('TotalBillWeight against Reference RowNo and Account Should be Equal to or less than Reference Row Weight\n'
                    + 'Here TotalBillWeight (' + csStr(g.Total) + ') exceeds Reference Row Weight (' + csStr(g.Ref) + ') for WagesAccount (' + name + ') and RowNo ' + g.RefLineId + '  in ' + gridName + ' Grid');
            }
        });
    }
    var WG_FMT = { Weight: 'q4', Quantity: 'q4', WeightCut: 'q', BillQty: 'q4', BillWeight: 'q4', RateWithoutAddLess: 'q4', RateAddLess: 'q4', Rate: 'q4', Amount: 'q4' };
    function wtxt(r, k, st) { var v = r[k]; if (v === null || v === undefined || v === '') return ''; return st ? F('raw', v) : F(WG_FMT[k] || 'raw', v); }

    /** AddWagesListInInsert:5008. */
    function addWagesListInInsert(isInput) {
        validateRefRows(WG, 'Regular_Wages');
        validateRefRows(ST, 'Other_Wages');
        var refDoc = isInput ? 'Issue' : 'Recovery', lines = [], docDate = val('txtDocdate');
        WG.filter(function (r) { return String(r.TransactionType) === refDoc; }).forEach(function (r) {
            if (netI(r.WagesId) === 0 || listText(ES.wagesAccounts, 'Id', 'WagesAccountName', r.WagesId) === '') throw new Error('WagesAccount Field required In Regular Wages Grid...');
            if (netI(netD(r.Quantity)) === 0 || wtxt(r, 'Quantity') === '') throw new Error('Quantity Field required In Regular Wages Grid...');
            if (netD(r.Weight) === 0 || wtxt(r, 'Weight') === '') throw new Error('Weight Field required In Regular Wages Grid...');
            if (netD(r.PackSize) === 0 || wtxt(r, 'PackSize') === '') throw new Error('PackSize Field required In Regular Wages Grid...');
            var wd = { InvConractorWagesAccountsId: netI(r.WagesId), Qty: netD(r.Quantity), Weight: netD(wtxt(r, 'Weight')), PackSize: netD(wtxt(r, 'PackSize')) };
            var free = String(r.WagesType) === 'Free Of Cost';
            if (free || String(r.WarehouseType || '') === 'Dryer') { wd.WageRate = 0; wd.RateAddLess = 0; wd.WagesAmount = 0; wd.InvContractorWagesScheduleId = 0; }
            else {
                wd.WageRate = netD(wtxt(r, 'Rate')); wd.InvContractorWagesScheduleId = netI(r.WagesScheduleId); wd.RateAddLess = netD(wtxt(r, 'RateAddLess'));
                if (wd.WageRate === 0) throw new Error('Rate Field required In Regular Wages Grid...');
            }
            wd.FreeOfCost = free;
            wd.WagesAmount = free ? 0 : (ES.wagesAmountCalculateOnQty ? rnd(wd.Qty * wd.WageRate, 2) : rnd(wd.Weight / wd.PackSize * wd.WageRate, 2));
            if (netI(r.SupplierId) === 0 || listText(ES.contractors, 'Id', 'CompanyName', r.SupplierId) === '') throw new Error('ContractorAccount Field required In Regular Wages Grid...');
            wd.ContractorId = netI(r.SupplierId);
            if (netI(r.ItemId) === 0 || String(r.ItemId === null || r.ItemId === undefined ? '' : r.ItemId) === '') throw new Error('Item Name Field required In Regular Wages Grid...');
            wd.ItemId = netI(String(r.ItemId)); wd.ItemName = String(r.Item || '');
            wd.CompanyName = listText(ES.contractors, 'Id', 'CompanyName', r.SupplierId);
            wd.WagesAccountName = listText(ES.wagesAccounts, 'Id', 'WagesAccountName', r.WagesId);
            wd.WagesTypeId = netI(r.WagesType);            /* Convert.ToInt32("Regular") fails -> 0 */
            wd.JobLotId = netI(String(r.jobLotId)); wd.Crop = String(r.Crop || ''); wd.InvPackingTypeId = netI(String(r.packingTypeId));
            wd.WareHouseFromId = netI(String(r.MoveFromId)); wd.WareHouseToId = netI(String(r.MoveToId));
            wd.BillQty = netD(wtxt(r, 'BillQty')); wd.WeightCut = netD(wtxt(r, 'WeightCut'));
            wd.RefDocQty = netD(F('raw', r.RefDocQty)); wd.RefDocWeight = netD(F('raw', r.RefDocWeight)); wd.RefLineId = netI(F('raw', r.RefLineId));
            wd.RefDocDate = docDate;
            wd.BillWeight = netD(r.Weight);                 /* BillWeight = the Weight cell's value */
            lines.push(wd);
        });
        var other = ST.filter(function (r) { return String(r.TransactionType) === refDoc; });
        if (!other.length && ES.stichingWagesCompulsory && refDoc !== 'Issue') {
            /* grdwagesDetail.GetRows() is grouped by TransactionType: the first row it returns is a group
               row whose PackSize text reads 0, which is < 100 whenever the Regular grid has rows. */
            if (WG.length > 0) throw new Error('Stitching Wages Grid... Record Not Found\n Stitching Wages Compulsory Configuration is On');
        }
        other.forEach(function (r) {
            var noWages = netI(r.WagesId) === 0 || listText(ES.stitchingAccounts, 'Id', 'WagesAccountName', r.WagesId).trim() === '';
            var noSup = netI(r.SupplierId) === 0 || listText(ES.contractors, 'Id', 'CompanyName', r.SupplierId).trim() === '';
            if (refDoc === 'Issue' || (!ES.stichingWagesCompulsory && noWages && noSup)) return;
            var g = 'Other Wages Grid ...';
            if (netI(r.WagesId) === 0 || listText(ES.stitchingAccounts, 'Id', 'WagesAccountName', r.WagesId) === '') throw new Error('WagesAccount Field required In ' + g);
            if (netI(netD(r.Quantity)) === 0 || wtxt(r, 'Quantity', true) === '') throw new Error('Quantity Field required In ' + g);
            if (netD(r.Weight) === 0 || wtxt(r, 'Weight', true) === '') throw new Error('Weight Field required In ' + g);
            if (netD(r.PackSize) === 0 || wtxt(r, 'PackSize', true) === '') throw new Error('PackSize Field Required In ' + g);
            var wds = { InvConractorWagesAccountsId: netI(r.WagesId), Qty: netD(r.Quantity), Weight: netD(wtxt(r, 'Weight', true)), PackSize: netD(wtxt(r, 'PackSize', true)) };
            if (String(r.WagesType) === 'Free Of Cost' || String(r.WarehouseType || '') === 'Dryer') { wds.WageRate = 0; wds.RateAddLess = 0; wds.WagesAmount = 0; wds.InvContractorWagesScheduleId = 0; }
            else {
                wds.WageRate = netD(wtxt(r, 'Rate', true)); wds.InvContractorWagesScheduleId = netI(r.WagesScheduleId); wds.RateAddLess = netD(wtxt(r, 'RateAddLess', true));
                if (wds.WageRate === 0) throw new Error('Rate Field required In ' + g);
                wds.WagesAmount = ES.wagesAmountCalculateOnQty ? rnd(wds.Qty * wds.WageRate, 2) : rnd(wds.Weight / wds.PackSize * wds.WageRate, 2);
            }
            if (netI(r.SupplierId) === 0 || listText(ES.contractors, 'Id', 'CompanyName', r.SupplierId) === '') throw new Error('ContractorAccount Field required In ' + g);
            wds.ContractorId = netI(r.SupplierId);
            if (netI(r.ItemId) === 0 || String(r.ItemId === null || r.ItemId === undefined ? '' : r.ItemId) === '') throw new Error('Item Name Field required In ' + g);
            wds.ItemId = netI(String(r.ItemId)); wds.ItemName = String(r.Item || '');
            wds.CompanyName = listText(ES.contractors, 'Id', 'CompanyName', r.SupplierId);
            wds.WagesAccountName = listText(ES.stitchingAccounts, 'Id', 'WagesAccountName', r.WagesId);
            wds.RefDocDate = r.Date ? dateOnly(r.Date) : '';     /* Conversion.ToDateTime(null) = 1900-01-01 (server side) */
            wds.JobLotId = netI(String(r.jobLotId)); wds.Crop = String(r.Crop || ''); wds.InvPackingTypeId = netI(String(r.packingTypeId));
            wds.WareHouseFromId = netI(String(r.MoveFromId)); wds.WareHouseToId = netI(String(r.MoveToId));
            wds.BillQty = netI(wtxt(r, 'BillQty', true));    /* Conversion.ToInt(Text) */
            wds.WeightCut = netD(wtxt(r, 'WeightCut', true)); wds.RefDocQty = netD(F('raw', r.RefDocQty)); wds.RefDocWeight = netD(F('raw', r.RefDocWeight));
            wds.RefLineId = netI(F('raw', r.RefLineId));
            wds.FreeOfCost = String(r.WagesType) === 'Free Of Cost';
            wds.WagesTypeId = 2;
            wds.BillWeight = netD(r.Weight);
            lines.push(wds);
        });
        var src = isInput ? INPUT : OUTPUT;
        return { RefDocument: refDoc, QtyTotal: total(src, 'Quantity'), WeightTotal: total(src, 'Weight'), lines: lines };
    }

    /** FormValidation:1626. */
    function formValidation() {
        if (convType() === 0) { box('Conversion Type Field Required'); $id('CmbConversionType').focus(); return false; }
        var dn = val('txtdocnumber');
        if (dn.trim() === '' || dn === '0') { box('document Number Field Required'); $id('txtdocnumber').focus(); return false; }
        if (val('txtProductionNo').trim() === '') { box('Production NO. Field Required'); $id('txtProductionNo').focus(); return false; }
        return true;
    }

    /** Insert():4365 - every check the form makes, then the POST. */
    function insert(buttonId) {
        var body;
        try {
            if (INPUT.length === 0) throw new Error('InPut Grid Not Found');
            if (OUTPUT.length === 0) throw new Error('OutPut Grid Not Found');
            if (!formValidation()) return;
            if (RECID > 0) { if (!ask('Are you sure to Update?')) return; }
            else if (!ask('Are you sure to Save?')) return;
            if (convType() !== 3) handleAverageRateCalculation();
            if (SUM.fa.trim() === '' || netD(SUM.fa) === 0) throw new Error('Finish Goods Receod Not Found in Grid Please Check!');
            var k;
            for (k = 0; k < PMR.length; k++) if (netD(PMR[k].Amount) > 0 && netI(PMR[k].ItemId) === 0) { box('Please Select an Item Against Expense First'); return; }
            for (k = 0; k < OHR.length; k++) if (netD(OHR[k].ExpAmount) > 0 && netI(OHR[k].ChartofAccount) === 0) { box('Please Select Account Title First'); return; }
            var hasIssue = false, firstItem = -1, same = true;
            INPUT.forEach(function (r) {
                if (r.EntryType === 'Issue') hasIssue = true;
                if (convType() === 2) { var it = netI(r.ItemId); if (firstItem === -1) firstItem = it; if (it !== firstItem) same = false; }
            });
            if (!hasIssue) { box('Issue is Required in Detail Grid'); return; }
            if (convType() === 2 && !same) { box('Rows in Input Grid should have the same Item when Conversion Type is ' + selText('CmbConversionType')); return; }

            var inW = netD(SUM.iw), bpW = netD(SUM.bw), fgW = netD(SUM.fw), diffW = inW - (bpW + fgW);
            var gainLossId = 0, diffAcc = 0;
            if (convType() === 3 && Math.abs(netD(SUM.ia) - netI(SUM.fa)) > 0) {
                if (!hasSel('CmbDifferenceAccount')) { $id('CmbDifferenceAccount').focus(); box('Difference A/c required when Conversion Type is ' + selText('CmbConversionType')); return; }
                diffAcc = netI(val('CmbDifferenceAccount'));
            }
            var tol = netD(ES.gainLossTolerance);
            if (diffW > 0 || diffW < 0) {
                var pct = Math.abs(diffW) * 100 / inW, tolW = inW * tol / 100, word = diffW > 0 ? 'Loss' : 'Gain';
                gainLossId = diffW > 0 ? 1 : 2;
                if (pct > tol) {
                    throw new Error('The difference between Input Weight and Output Weight is greater than the configured tolerance.' + (diffW > 0 ? '' : ' ')
                        + '\nDifference Weight: ' + csStr(Math.abs(rnd(diffW, 4))) + ' | Tolerance Weight: ' + csStr(rnd(tolW, 4)) + '.\nThe entry is in ' + word + '.');
                }
                if (pct < tol && !ask('There is difference between Input Weight and Output Weight.\n Input Weight is: ' + csStr(inW) + ' | OutPut Weight is: '
                        + csStr(bpW + fgW) + ' and.\nDifference Weight is ' + csStr(diffW) + '.The entry is in ' + word + '. Are you sure you want to save?')) return;
            }

            var details = [];
            for (k = 0; k < INPUT.length; k++) {
                var r3 = INPUT[k];
                if (netI(String(r3.ItemId === null || r3.ItemId === undefined ? '' : r3.ItemId)) === 0) continue;
                var d3 = { Id: netI(r3.Id), RefDocumentTypeId: netI(r3.RefDocumentTypeId), RefDocNoId: netI(r3.RefDocNoId), RefDocSubId: netI(r3.RefDocSubId),
                    EntryType: String(r3.EntryType || ''), WarehouseId: netI(r3.WareHouseId), ItemId: netI(r3.ItemId), ItemUomId: netI(r3.ItemUOMId),
                    CropBatch: String(r3.CropYear || ''), JobLotId: netI(r3.JobLotId), PackingtypeId: netI(r3.PackingTypeId), Qty: netD(r3.Quantity), Weight: netD(r3.Weight) };
                if (d3.Weight === 0) { box('Weight field Required'); return; }
                d3.Rate = netD(r3.Rate);
                if (d3.Rate === 0) { box('ItemRate field Required In Row#' + (k + 1) + 'In InPut Grid'); return; }
                d3.RateUOMId = netI(r3.RateUOMId);
                d3.Amount = netD(r3.Amount);
                if (d3.Amount === 0) { box('Amount field Required In Row#' + (k + 1) + 'In InPut Grid'); return; }
                d3.Moisture = netD(r3.Moisture); d3.MoistureSlabId = netI(r3.MoistureSlabId);
                d3.Remarks = String(r3.Remarks === null || r3.Remarks === undefined ? '' : r3.Remarks);
                d3.LineId = netI(F('raw', r3.LineId)); d3.labIPmActivityLogId = netI(F('raw', r3.labIPmActivityLogId));
                details.push(d3);
            }
            var onHold = !!ES.stockReleaseFromFumigation && $id('chkFumigationOnHold').checked;
            for (k = 0; k < OUTPUT.length; k++) {
                var r4 = OUTPUT[k];
                if (netI(String(r4.ItemId === null || r4.ItemId === undefined ? '' : r4.ItemId)) === 0) continue;
                var d4 = { Id: netI(r4.Id), EntryType: String(r4.EntryType || ''), WarehouseId: netI(r4.WareHouseId), ItemId: netI(r4.ItemId), ItemUomId: netI(r4.ItemUOMId),
                    CropBatch: String(r4.CropYear || ''), JobLotId: netI(r4.JobLotId), PackingtypeId: netI(r4.PackingTypeId), Qty: netD(r4.Quantity), Weight: netD(r4.Weight),
                    Rate: netD(r4.Rate) };
                if (d4.Rate === 0) { box('ItemRate field Required In Row#' + (k + 1) + 'In Output Grid'); return; }
                d4.RateUOMId = netI(r4.RateUOMId);
                d4.Moisture = netI(F('raw', r4.Moisture));          /* Conversion.ToInt(Moisture.Text) */
                d4.MoistureSlabId = netI(F('raw', r4.MoistureSlabId));
                d4.Amount = netD(r4.Amount);
                if (d4.Amount === 0) { box('Amount field Required In Row#' + (k + 1) + 'In Output Grid'); return; }
                d4.ExpenseAmount = fround(netD(r4.ExpAmount)); d4.ItemOhCost = fround(netD(r4.ItemExpAmount));   /* Conversion.ToSingle */
                d4.PackingMaterialAmount = netD(r4.PMAmount); d4.ItemPmCost = netD(r4.ItemPMAmount);
                d4.Remarks = String(r4.Remarks === null || r4.Remarks === undefined ? '' : r4.Remarks);
                d4.LineId = netI(F('raw', r4.LineId)); d4.WagesAmount = netD(r4.WagesAmount); d4.labIPmActivityLogId = netI(r4.labIPmActivityLogId);
                d4.IsOnHold = onHold;
                details.push(d4);
            }
            var packings = [];
            for (k = 0; k < PMR.length; k++) {
                var r5 = PMR[k];
                if (netI(String(r5.ItemId === null || r5.ItemId === undefined ? '' : r5.ItemId)) === 0) continue;
                var p5 = { ItemId: netI(r5.ItemId), WarehouseId: netI(r5.WareHouseId), RackId: netI(r5.RackId), ItemConditionId: netI(r5.ItemConditionId) };
                if (p5.ItemConditionId === 0) throw new Error('Item Condition field required in Packing grid at row#' + (k + 1));
                p5.BrandItemId = netI(r5.BrandItemId); p5.BrandItemUomId = netI(r5.BrandUomId); p5.ItemQty = netD(r5.ItemQTY);
                validateField(p5.ItemId, 'Item', k); validateField(p5.WarehouseId, 'Warehouse', k); validateField(p5.RackId, 'RackName', k); validateField(p5.ItemQty, 'Qty', k);
                if (p5.BrandItemId > 0 && p5.BrandItemUomId === 0) { box('Brand Uom Field Require for Brand Item:' + String(r5.BrandItem || '')); return; }
                p5.ItemRate = netD(F('r', r5.ItemRate)); p5.ItemAmount = netD(F('a', r5.Amount));
                validateField(p5.ItemAmount, 'Amount', k);
                p5.ChargeTo = chargeText(r5.ChargeTo);
                validateField(p5.ChargeTo, 'Charge To', k);
                p5.ContractScheduleId = netI(r5.ContractScheduleId);
                var sch = SCHEDULES.filter(function (s) { return netI(s.Id) === p5.ContractScheduleId; })[0];
                p5.ExImInvoiceId = sch ? netI(sch.InvoiceId) : 0;
                packings.push(p5);
            }
            var expenses = [];
            for (k = 0; k < OHR.length; k++) {
                var r6 = OHR[k];
                if (netI(String(r6.ChartofAccount === null || r6.ChartofAccount === undefined ? '' : r6.ChartofAccount)) === 0) continue;
                if (listText(ES.overheadAccounts, 'Id', 'AccountTitle', r6.ChartofAccount) === '') { box('ChartofAccount Field Require '); return; }
                var e6 = { ChartOfAccountId: netI(r6.ChartofAccount), BrandItemId: netI(r6.BrandItemId), BrandItemUomId: netI(r6.BrandUomId) };
                if (e6.BrandItemId > 0 && e6.BrandItemUomId === 0) { box('Brand Uom Field Require for Brand Item:' + String(r6.BrandItem || '')); return; }
                e6.LedgerRemarks = String(r6.LedgerRemarks === null || r6.LedgerRemarks === undefined ? '' : r6.LedgerRemarks);
                if (F('a', r6.ExpAmount) === '') { box('ExpAmount Field Require '); return; }
                e6.ExpAmount = netD(F('a', r6.ExpAmount));
                if (chargeText(r6.ChargeTo) === '') { box('Charge To is Required'); return; }
                e6.ChargeTo = chargeText(r6.ChargeTo);
                expenses.push(e6);
            }
            if (packings.length === 0) {
                if (ES.pmCompulsoryForStop) { box('Packing Material Detail is Compulsory'); return; }
                if (ES.pmCompulsoryForWarning) {
                    if (RECID > 0) { if (!ask('Are you sure to Update?No Packing Material Detail Added')) return; }
                    else if (!ask('Are you sure to Save?No Packing Material Detail Added')) return;
                }
            }
            checkExpenseandPackingMaterialAmountTotal();
            if (checkPackingMaterialAmountTotal) { box('Packing Material Amount not Equal By Product PMAmount'); return; }
            if (CheckExpenseAmountTotal) { box('Expenses Amount not Equal ByProduct Expense Amount'); return; }
            var bills = [];
            if (ES.contractWagesChargeToProduct && convType() !== 5) { bills.push(addWagesListInInsert(false)); bills.push(addWagesListInInsert(true)); }
            body = { id: RECID, docSrNo: netI(val('txtdocnumber')), docDate: val('txtDocdate'), productionNo: val('txtProductionNo'),
                remarks: val('txtRemarks'), conversionTypeId: convType(), parentCategoryId: netI(val('cmbParentCategory')),
                ebDepartmentId: netI(val('CmbProductionDepartment')), differenceAccountId: diffAcc, gainLossId: gainLossId,
                inputDetailRowsRemoveIds: InputDetailRowsRemoveIds || '', details: details, packings: packings, expenses: expenses, wagesBills: bills };
        } catch (e) { renderAll(); box(e.message); return; }
        renderAll();
        var wasUpdate = RECID > 0, type = convType();
        return busy(buttonId, function () {
            say('Saving...');
            return postJson(api + '/save', body).then(function (d) {
                box(d && d.message ? d.message : ((wasUpdate ? 'Record Update Successfully ' : 'Record Save Successfully ') + body.docSrNo));
                var code = d ? netI(d.id) : 0;
                if (!ES.contractWagesChargeToProduct && type !== 5 && ES.wagesStatus && ES.wagesActive) {
                    /* :4785 - frmwagesBillHeader(RefDocTypeId 66, RefDocId = code); no delete, no gross weight. */
                    p280OpenWages(66, code, 0, null);
                }
                if ($id('chkPrint').checked) window.CrystalPrint.open('sc-605', { id: code });
                refreshForm();
                summeryreset(); renderSummary();
                say('Saved.');
            }).catch(function (e) { say('Not saved.'); box(e.message); });
        });
    }
    function validateField(v, name, k) {
        var bad = v === null || v === undefined || (typeof v === 'number' && v <= 0) || (typeof v === 'string' && v.trim() === '');
        if (bad) throw new Error(name + ' is required in Packing Grid at row No: ' + (k + 1));
    }

    /** btnsave_Click:4807 - RecId = 0 first, so Save always inserts. */
    function btnSave() { if (!isShown('btnSave') || $id('btnSave').disabled) return; RECID = 0; insert('btnSave'); }
    /** btnUpdate_Click:4951. */
    function btnUpdate() {
        if (!isShown('btnUpdate') || $id('btnUpdate').disabled) return;
        if (RECID === 0) { box('RecId not found'); return; }
        insert('btnUpdate');
    }
    /** btnDelete_Click:4967. */
    function btnDelete() {
        var r = (LK && LK.rights) || {};
        if (!r.delete) { box("Record cannot be delete because you don't have right...."); return; }
        if (!ask('Are you sure to Delete?')) return;
        if (RECID <= 0) { box('RecordId Not Found.....'); return; }
        return busy('btnDelete', function () {
            return postJson(api + '/delete', { id: RECID }).then(function (d) {
                box(d && d.message ? d.message : 'Delete Record Successfully');
                refreshForm();
            }).catch(function (e) { box(e.message); });
        });
    }

    // ======================================================================== load / new

    /** RefreshForm:1741. The doc date is not touched. */
    function refreshForm() {
        InputDetailRowsRemoveIds = '';
        RECID = 0;
        SUM.wages = '0';
        saveVisible = true; show('btnSave', true); show('btnUpdate', false); show('btnDelete', false);
        setVal('txtProductionNo', ''); setSel('CmbProductionDepartment', 0); setVal('txtRemarks', ''); setVal('txtQty', ''); setVal('txtUnitWeight', '');
        $id('lblDocNo').textContent = '';
        INPUT = []; OUTPUT = []; OHR = [newOhRow()]; PMR = [newPmRow()]; WG = []; ST = [];
        CUR = { in: -1, out: -1, pm: -1, oh: -1, wg: -1, st: -1 };
        lockConversionType();
        conversionTypeChanged();
        $id('chkFumigationOnHold').checked = false;
        renderAll();
        /* GenerateDocNumber:803 runs inside RefreshForm, before anything that follows it. */
        var code = getJson(api + '/next-code').then(function (d) { var c = d && d.docSrNo ? d.docSrNo : 0; if (c > 0) setVal('txtdocnumber', c); }).catch(function (e) { box(e.message); });
        return Promise.all([code, loadPmItems()]);
    }
    /** btnnew_Click:1846. */
    function btnNew() {
        return busy('btnNew', function () {
            var p = refreshForm();
            resetDetail(); detailButtons(false);
            summeryreset(); renderSummary();
            say('Ready');
            return p;
        });
    }
    /** btnRefresh_Click:1853. */
    function btnRefresh() {
        return busy('btnRefresh', function () {
            return Promise.all([loadLookups(false), loadEditSetup()]).then(function () {
                return Promise.all([loadSchedules(), loadPmItems()]);
            }).then(function () { applyRightsToButtons(); renderAll(); }).catch(function (e) { box(e.message); });
        });
    }

    function loadByDocNo() {
        var docNo = netI(val('txtLoadDocNo'));
        if (docNo <= 0) { box('Enter a document number'); return; }
        return busy('btnLoad', function () {
            return getJson(api + '/id-by-doc-no?docSrNo=' + docNo).then(function (d) {
                var id = d && d.id ? d.id : 0;
                if (id <= 0) { box('No Stock Conversion with document number ' + docNo); return; }
                return readById(id);
            }).catch(function (e) { box(e.message); });
        });
    }

    /** ReadById:4820. */
    function readById(id) {
        return refreshForm().then(function () {
            resetDetail(); detailButtons(false);
            summeryreset();
            RECID = id;
            InputDetailRowsRemoveIds = '';
            showView('form');
            return getJson(api + '/' + id);
        }).then(function (d) {
            if (!d || !d.header) { INPUT = []; OUTPUT = []; PMR = []; OHR = []; summeryreset(); renderAll(); return; }
            var h = d.header;
            show('btnDelete', true);
            setVal('txtdocnumber', col(h, 'DocSrNo'));
            $id('lblDocNo').textContent = col(h, 'DocSrNo') ? '#' + col(h, 'DocSrNo') : '';
            setVal('txtProductionNo', col(h, 'ProductionNo'));
            setVal('txtDocdate', dateOnly(col(h, 'DocDate'))); docDateTag = val('txtDocdate');
            setVal('txtRemarks', col(h, 'Remarks'));
            setSel('cmbParentCategory', col(h, 'parentCategoryId'));
            var ct = netI(col(h, 'ConversionTypeId'));
            $id('CmbConversionType').disabled = false;
            setSel('CmbConversionType', ct);
            if (convType() === 0) setSel('CmbConversionType', 1);
            if (convType() === 3) setSel('CmbDifferenceAccount', col(h, 'DifferenceAccountId'));
            if (netI(col(h, 'EBDepartmentId')) > 0) setSel('CmbProductionDepartment', col(h, 'EBDepartmentId'));
            conversionTypeChanged();
            PMR = (d.packings || []).map(function (p) {
                return { ItemId: col(p, 'ItemId'), WareHouseId: col(p, 'WarehouseId'), WareHouse: col(p, 'WarehouseName'), RackId: col(p, 'RackId'),
                    RackName: col(p, 'rackName'), ItemConditionId: col(p, 'ItemConditionId'), BrandItemId: col(p, 'BrandItemId'), BrandItem: col(p, 'BrandName'),
                    BrandUomId: col(p, 'BrandItemUomId'), BrandUom: col(p, 'BrandUom'), ItemQTY: netD(col(p, 'ItemQty')), ItemRate: netD(col(p, 'ItemRate')),
                    Amount: netD(col(p, 'ItemAmount')), ChargeTo: col(p, 'ChargeTo'), ContractScheduleId: col(p, 'ContractScheduleId') };
            });
            if (!PMR.length) PMR.push(newPmRow());
            saveVisible = false; show('btnUpdate', true); show('btnSave', false);
            OHR = (d.expenses || []).map(function (e) {
                return { ChartofAccount: String(col(e, 'ChartOfAccountId')), BrandItemId: col(e, 'BrandItemId'), BrandItem: col(e, 'BrandName'),
                    BrandUomId: col(e, 'BrandItemUomId'), BrandUom: col(e, 'BrandUom'), LedgerRemarks: col(e, 'LedgerRemarks'),
                    ExpAmount: netD(col(e, 'ExpAmount')), ChargeTo: col(e, 'ChargeTo') };
            });
            if (!OHR.length) OHR.push(newOhRow());
            INPUT = []; OUTPUT = [];
            inputNoDelete = false;              // gridsettings():4914
            var issueLine = 1, outLine = 1;
            (d.details || []).forEach(function (x) {
                var lineId = netI(col(x, 'LineId'));
                if (ct === 5) lineId = String(col(x, 'EntryType')) === 'Issue' ? issueLine++ : outLine++;
                if (String(col(x, 'EntryType')) === 'Issue') {
                    INPUT.push({ Id: netI(col(x, 'Id')), RefDocumentTypeId: netI(col(x, 'RefDocumentTypeId')), RefDocNoId: netI(col(x, 'RefDocNoId')),
                        RefDocSubId: netI(col(x, 'RefDocSubId')), EntryType: col(x, 'EntryType'), WareHouseId: netI(col(x, 'WarehouseId')),
                        WareHouse: col(x, 'WareHouseName'), CropYear: col(x, 'CropBatch'), ItemId: netI(col(x, 'ItemId')), Item: col(x, 'ItemName'),
                        JobLotId: netI(col(x, 'JobLotId')), JobLot: col(x, 'JobLotDescription'), ItemUOMId: netI(col(x, 'ItemUomId')), UOM: col(x, 'UomCode'),
                        PackEquivalent: netD(col(x, 'UOMDescription')), PackingTypeId: netI(col(x, 'PackingtypeId')), PackingType: col(x, 'PackTypeDesc'),
                        BalQty: 0, BalWeight: 0, Quantity: netD(col(x, 'Qty')), Weight: netD(col(x, 'Weight')), Rate: netD(col(x, 'Rate')),
                        RateUOMId: netI(col(x, 'RateUOMId')), RateUOM: col(x, 'RateUom'), RateEquivalent: netD(col(x, 'Equivalent')), Amount: netD(col(x, 'Amount')),
                        Moisture: F('raw', col(x, 'Moisture')), MoistureSlabId: col(x, 'MoistureSlabId'), Remarks: col(x, 'Remarks'), LineId: lineId,
                        labIPmActivityLogId: col(x, 'labIPmActivityLogId'), _chk: false });
                } else {
                    var awe = netD(col(x, 'Amount')) - (netD(col(x, 'PackingMaterialAmount')) + netD(col(x, 'ExpenseAmount')));
                    $id('chkFumigationOnHold').checked = truthy(col(x, 'IsOnHold'));
                    OUTPUT.push({ Id: netI(col(x, 'Id')), EntryType: col(x, 'EntryType'), WareHouseId: netI(col(x, 'WarehouseId')), WareHouse: col(x, 'WareHouseName'),
                        CropYear: col(x, 'CropBatch'), ItemId: netI(col(x, 'ItemId')), Item: col(x, 'ItemName'), JobLotId: netI(col(x, 'JobLotId')),
                        JobLot: col(x, 'JobLotDescription'), ItemUOMId: netI(col(x, 'ItemUomId')), UOM: col(x, 'UomCode'), PackEquivalent: netD(col(x, 'UOMDescription')),
                        PackingTypeId: netI(col(x, 'PackingtypeId')), PackingType: col(x, 'PackTypeDesc'), Quantity: netD(col(x, 'Qty')), Weight: netD(col(x, 'Weight')),
                        Rate: netD(col(x, 'Rate')), RateUOMId: netI(col(x, 'RateUOMId')), RateUOM: col(x, 'RateUom'), RateEquivalent: netD(col(x, 'Equivalent')),
                        AmountWithoutExpenses: awe, Moisture: F('raw', col(x, 'Moisture')), MoistureSlabId: col(x, 'MoistureSlabId'),
                        MoistureSlabDescription: col(x, 'MoistureSlabDescription'), PMAmount: netD(col(x, 'PackingMaterialAmount')), ItemPMAmount: netD(col(x, 'ItemPmCost')),
                        ExpAmount: netD(col(x, 'ExpenseAmount')), ItemExpAmount: netD(col(x, 'ItemOhCost')), Amount: netD(col(x, 'Amount')), Remarks: col(x, 'Remarks'),
                        IssueWeight: netD(col(x, 'IssueWeight')), LineId: lineId, WagesAmount: netD(col(x, 'WagesAmount')), labIPmActivityLogId: col(x, 'labIPmActivityLogId') });
                }
            });
            proportionatedPackingMaterialAmountInOutputGrid();
            proportionateOverheadGrid();
            if (convType() !== 3) handleAverageRateCalculation();
            generateSummaryForUser();
            lockConversionType();
            renderAll();
            say('Document ' + col(h, 'DocSrNo') + ' loaded.');
            var p = loadPmItems();
            if (ES.contractWagesChargeToProduct) {
                p = p.then(function () { return wagesDetailReadById(id); })
                    .then(function () { return wagesFill('out'); })
                    .then(function () { return wagesFill('in'); })
                    .then(function () { generateSummaryForUser(); renderAll(); });
            }
            return p;
        }).catch(function (e) { box(e.message); });
    }

    /** txtDocdate Leave -> txtDocdate_ValueChanged:7680. */
    function docDateLeave() {
        var prev = docDateTag || val('txtDocdate'), cur = val('txtDocdate');
        if (prev === cur) return;
        docDateTag = cur;
        if (total(PMR, 'Amount') > 0) {
            if (!ask('By changing doc date the rates in Packing Material grid will be change, so that amount change.Are you sure to change doc date ?')) {
                docDateTag = prev; setVal('txtDocdate', prev);
            } else {
                /* AvgRateUpdateOnDocDateChangeForPm:7646, then handleAverageRateCalculation. */
                PMR.reduce(function (p, r) {
                    return p.then(function () {
                        return getJson(api + '/pm-rate' + qs({ itemId: netI(r.ItemId), docDate: cur, itemConditionId: netI(r.ItemConditionId), recId: RECID }))
                            .then(function (d) { var rate = netD(d && d.rate); r.ItemRate = rate; r.Amount = netD(r.ItemQTY) * rate; });
                    });
                }, Promise.resolve()).then(function () { handleAverageRateCalculation(); renderAll(); }).catch(function (e) { box(e.message); });
            }
        }
    }

    // ============================================================================ history

    var HISTORY_ROWS = [], historySeq = 0;
    function loadHistory() {
        return busy('btnHistoryRefresh', function () {
            var q = [];
            function add(name, v) { if (v !== '' && v !== null && v !== undefined) q.push(name + '=' + encodeURIComponent(v)); }
            var mode = (document.querySelector('input[name="scDateMode"]:checked') || {}).value || 'doc';
            var names = { doc: ['fromDate', 'toDate'], entry: ['entryFromDate', 'entryToDate'], modify: ['modifyFromDate', 'modifyToDate'], approved: ['approvedDateFrom', 'approvedDateTo'] }[mode];
            if ($id('fFromOn').checked) add(names[0], val('fFromDate'));
            if ($id('fToOn').checked) add(names[1], val('fToDate'));
            add('docNoFrom', val('fDocNoFrom')); add('docNoTo', val('fDocNoTo'));
            return getJson(api + '/history' + (q.length ? '?' + q.join('&') : '')).then(function (d) {
                HISTORY_ROWS = (d && d.rows) || [];
                renderHistory();
                historySubGrids(null);
                $id('lblHistoryCount').textContent = HISTORY_ROWS.length + ' record(s)' + (d && d.canViewAllRecords ? '' : ' - your own records only');
                if (HISTORY_ROWS.length) historyRowSelected(0);
            }).catch(function (e) { box(e.message); });
        });
    }
    var HCOLS = [['DocNo', 'DocNo'], ['DocDate', 'DocDate'], ['ProductionNo', 'ProductionNo'], ['Remarks', 'Remarks'], ['EntryDate', 'EntryDate'],
        ['EntryUser', 'EntryUser'], ['ModifyDate', 'ModifyDate'], ['ModifyUser', 'ModifyUser'], ['ApprovedDate', 'PostDate'], ['ApprovedUser', 'ApprovedUser']];
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
    function historyRowSelected(i) {
        var r = HISTORY_ROWS[i];
        if (!r) return;
        $id('gridHistory').querySelectorAll('tr[data-i]').forEach(function (tr) { tr.classList.toggle('is-current', +tr.getAttribute('data-i') === i); });
        var seq = ++historySeq;
        return getJson(api + '/' + netI(col(r, 'Id'))).then(function (d) {
            if (seq !== historySeq) return;
            if (d && d.details && d.details.length) historySubGrids(d);
        }).catch(function (e) { box(e.message); });
    }
    function historySubGrids(d) {
        var io = $id('gridHistoryIO'), pm = $id('gridHistoryPM'), oh = $id('gridHistoryOH');
        if (!d) { io.innerHTML = ''; pm.innerHTML = ''; oh.innerHTML = ''; return; }
        function kind(c) { return c === 'q' ? 'q' : c; }
        function table(spec, rows) {
            var foot = '<tr>' + spec.map(function (c, k) {
                if (!c[3]) return '<td>' + (k === 0 ? 'Total' : '') + '</td>';
                return '<td class="num">' + esc(F(kind(c[2]), rows.reduce(function (a, r) { return a + netD(col(r, c[1])); }, 0))) + '</td>';
            }).join('') + '</tr>';
            return '<table class="win-grid"><thead><tr>' + spec.map(function (c) { return '<th' + (c[2] !== 't' ? ' class="num"' : '') + '>' + esc(c[0]) + '</th>'; }).join('')
                 + '</tr></thead><tbody>' + rows.map(function (r) {
                       return '<tr>' + spec.map(function (c) {
                           var v = c[1] === 'ChargeTo' ? chargeText(col(r, 'ChargeTo')) : (c[2] === 't' ? col(r, c[1]) : F(kind(c[2]), col(r, c[1])));
                           return '<td' + (c[2] !== 't' ? ' class="num"' : '') + '>' + esc(v) + '</td>';
                       }).join('') + '</tr>';
                   }).join('') + '</tbody>' + (rows.length ? '<tfoot>' + foot + '</tfoot>' : '') + '</table>';
        }
        io.innerHTML = table([['EntryType', 'EntryType', 't'], ['WareHouse', 'WareHouseName', 't'], ['CropBatch', 'CropBatch', 't'], ['ItemName', 'ItemName', 't'],
            ['JobLot', 'JobLotDescription', 't'], ['PackUom', 'UomCode', 't'], ['PackType', 'PackTypeDesc', 't'], ['Qty', 'Qty', 'q', 1], ['Weight', 'Weight', 'q', 1],
            ['Rate', 'Rate', 'r'], ['RateUom', 'RateUom', 't'], ['PMAmount', 'PackingMaterialAmount', 'a', 1], ['ItemPMAmount', 'ItemPmCost', 'a', 1],
            ['ExpAmount', 'ExpenseAmount', 'a', 1], ['ItemExpAmount', 'ItemOhCost', 'a', 1], ['Amount', 'Amount', 'a', 1], ['Remarks', 'Remarks', 't']], d.details || []);
        pm.innerHTML = table([['WarehouseName', 'WarehouseName', 't'], ['Item', 'ItemName', 't'], ['BrandItem', 'BrandName', 't'], ['BrandUom', 'BrandUom', 't'],
            ['ItemQTY', 'ItemQty', 'q', 1], ['Rate', 'ItemRate', 'r'], ['Amount', 'ItemAmount', 'a', 1], ['ChargeTo', 'ChargeTo', 't'], ['Schedule / Invoice No', 'ContractScheduleNo', 't']], d.packings || []);
        oh.innerHTML = table([['ChartofAccount', 'AccountTitle', 't'], ['BrandItem', 'BrandName', 't'], ['BrandUom', 'BrandUom', 't'],
            ['ExpAmount', 'ExpAmount', 'a', 1], ['LedgerRemarks', 'LedgerRemarks', 't']], d.expenses || []);
    }
    function newHistory() {
        setVal('fDocNoFrom', ''); setVal('fDocNoTo', '');
        var f = new Date(); f.setDate(f.getDate() - 3);
        setVal('fFromDate', f.getFullYear() + '-' + String(f.getMonth() + 1).padStart(2, '0') + '-' + String(f.getDate()).padStart(2, '0'));
        setVal('fToDate', today());
        var r = document.querySelector('input[name="scDateMode"][value="doc"]'); if (r) r.checked = true;
        HISTORY_ROWS = []; $id('historyHead').innerHTML = ''; $id('gridHistory').innerHTML = ''; historySubGrids(null);
        $id('lblHistoryCount').textContent = '';
    }
    function fmtDate(v, withTime) {
        if (!v) return '';
        var d = new Date(String(v).replace(' ', 'T'));
        if (isNaN(d.getTime())) return String(v);
        var m = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'][d.getMonth()];
        var s = String(d.getDate()).padStart(2, '0') + '-' + m + '-' + d.getFullYear();
        if (!withTime) return s;
        var hh = d.getHours(), ap = hh >= 12 ? 'PM' : 'AM';
        hh = hh % 12; if (hh === 0) hh = 12;
        return s + ' ' + String(hh).padStart(2, '0') + ':' + String(d.getMinutes()).padStart(2, '0') + ' ' + ap;
    }

    // ============================================================================== views

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
    function tab3(paneId) {
        ['paneRegWages', 'paneOtherWages'].forEach(function (p) { $id(p).classList.toggle('is-active', p === paneId); });
        $id('scTabs3').querySelectorAll('.sc-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-pane') === paneId); });
    }
    function toggleFullscreen(boxId) { var el = $id(boxId); if (el) el.classList.toggle('is-fullscreen'); }

    /** Print_Click:6378 -> StockConversionSummary_605(RecId). */
    function print() {
        if (RECID === 0) { box('No Record Found For Display'); return; }
        return window.CrystalPrint.open('sc-605', { id: RECID }, 'btnPrint');
    }
    function printId(id) { return window.CrystalPrint.open('sc-605', { id: id }); }
    /** btnVoucher_Click:6390 -> VoucherReport_118(VoucherHeadIdGet(RecId, 66)). */
    function voucher() {
        var win = window.CrystalPrint.reserve();
        return busy('btnVoucher', function () {
            return getJson(api + '/voucher-head?id=' + RECID).then(function (d) {
                var vh = d ? netI(d.voucherHeadId) : 0;
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

    /** Load:625-629 - Save / Print / Update / Delete enabled from the grants. */
    function applyRightsToButtons() {
        var r = (LK && LK.rights) || {};
        $id('btnSave').disabled = !r.save;
        $id('btnUpdate').disabled = !r.update;
        $id('btnPrint').disabled = !r.print;
        $id('btnDelete').disabled = !r.delete;
    }

    function onForm() { return $id('mainViewForm').style.display !== 'none'; }
    function focusFirstRow(bodyId) { var t = $id(bodyId).querySelector('tr[data-i]'); if (t) { t.tabIndex = 0; t.focus(); } }

    /* The two ShowDialog loaders block the form under them. LoadavailableTransactionsForStockReleaseFromFumigation
       has KeyPreview + KeyDown (:662): Esc / Ctrl+E Close, Ctrl+Up From date, Ctrl+Down grid, Ctrl+F5 Parent
       Category, Ctrl+L Load, Ctrl+S Search. frmLoadStockShortFallForSales wires no KeyDown at all, so no
       shortcut does anything while it is open. */
    function guardKeys(map) {
        var FUM_KEYS = {
            'esc': fumClose, 'ctrl+e': fumClose, 'ctrl+s': fumSearch, 'ctrl+l': fumLoad,
            'ctrl+arrowup': function () { $id('fmFrom').focus(); },
            'ctrl+arrowdown': function () { var t = $id('gridFum').querySelector('tr[data-i]'); if (t) t.focus(); },
            'ctrl+f5': function () { $id('fmParent').focus(); }
        };
        var out = {};
        Object.keys(map).forEach(function (k) {
            out[k] = function (e) {
                if (sfOpen()) return;
                if (fumOpen()) { if (FUM_KEYS[k]) FUM_KEYS[k](e); return; }
                map[k](e);
            };
        });
        return out;
    }

    function boot() {
        setVal('txtDocdate', today());
        docDateTag = val('txtDocdate');
        newHistory();
        summeryreset();

        K.enterToTab();
        K.keys(guardKeys({
            'ctrl+t': function () { showView(onForm() ? 'history' : 'form'); },
            'ctrl+e': function () { if ($id('loaderModal').classList.contains('is-open')) loaderClose(); else K.close(); },
            'esc': function () {
                if ($id('pickModal').classList.contains('is-open')) pickClose();
                else if ($id('loaderModal').classList.contains('is-open')) loaderClose();
                else K.close();
            },
            'ctrl+alt': shortcuts,
            'ctrl+s': function () {
                if ($id('loaderModal').classList.contains('is-open')) { loaderSearch(); return; }
                if (onForm()) btnSave(); else loadHistory();
            },
            'ctrl+l': function () { if ($id('loaderModal').classList.contains('is-open')) loaderLoad(); },
            'ctrl+u': function () { if (onForm()) btnUpdate(); },
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
        }));

        /* Detail TextChanged / Leave events. */
        $id('txtQty').addEventListener('input', function () { calculateWeight(); amountCalculation(); });
        $id('cmbUOM').addEventListener('change', function () { calculateWeight(); amountCalculation(); });
        $id('txtRate').addEventListener('input', amountCalculation);
        $id('cmbRateUom').addEventListener('change', amountCalculation);
        $id('txtUnitWeight').addEventListener('input', amountCalculation);
        $id('CmbCropyr').addEventListener('change', cropChanged);
        $id('cmbLot').addEventListener('change', lotChanged);
        $id('cmbGodown').addEventListener('change', godownChanged);
        $id('txtDocdate').addEventListener('blur', docDateLeave);
        /* txtRate_KeyPress:6519 - digits and one decimal point. */
        ['txtRate', 'txtQty', 'txtUnitWeight', 'txtMoisture'].forEach(function (id) {
            $id(id).addEventListener('keypress', function (e) {
                if (e.key.length !== 1 || e.ctrlKey) return;
                if (/[0-9]/.test(e.key) || (e.key === '.' && this.value.indexOf('.') < 0)) return;
                e.preventDefault();
            });
        });
        ['fDocNoFrom', 'fDocNoTo'].forEach(function (id) {
            $id(id).addEventListener('keypress', function (e) { if (e.key.length === 1 && !e.ctrlKey && !/[0-9]/.test(e.key)) e.preventDefault(); });
        });

        wireGrid('gridInput', 'in', { edit: inputEdited, act: function (i, a) { if (a === 'del') deleteInputRow(i, false); },
            keyDel: function (i) { deleteInputRow(i, true); } });
        $id('inputHead').addEventListener('change', function (e) {
            if (e.target.id === 'chkInputAll') { var on = e.target.checked; INPUT.forEach(function (r) { r._chk = on; }); renderInput(); }
        });
        wireGrid('gridOutput', 'out', { edit: outputEdited, act: function (i, a) { if (a === 'del') deleteOutputRow(i); },
            dbl: editOutputRow, keyDel: deleteOutputRow });
        wireGrid('gridPacking', 'pm', { edit: pmEdited, act: pmButton, f1: pmF1,
            keyDel: function (i) { PMR.splice(i, 1); if (PMR.length === 0) PMR.push(newPmRow()); proportionatedPackingMaterialAmountInOutputGrid(); generateSummaryForUser(); renderAll(); },
            keyAdd: function () { PMR.push(newPmRow()); proportionatedPackingMaterialAmountInOutputGrid(); generateSummaryForUser(); renderAll(); } });
        wireGrid('gridExpense', 'oh', { edit: ohEdited, act: ohButton, f1: ohF1,
            /* grdOH_KeyDown:7208 re-adds a blank row when the PACKING table is empty (sic). */
            keyDel: function (i) { OHR.splice(i, 1); if (PMR.length === 0) OHR.push(newOhRow()); proportionateOverheadGrid(); generateSummaryForUser(); renderAll(); },
            keyAdd: function () { OHR.push(newOhRow()); proportionateOverheadGrid(); generateSummaryForUser(); renderAll(); } });
        wireGrid('gridWages', 'wg', { edit: regularEdited, act: function (i, a) { wagesButton('wg', i, a); } });
        wireGrid('gridStitch', 'st', { edit: otherEdited, act: function (i, a) { wagesButton('st', i, a); } });

        /* the loader grid */
        $id('gridLoader').addEventListener('change', function (e) {
            var i = e.target.getAttribute('data-i');
            if (i === null) return;
            LD.rows[+i]._chk = e.target.checked;
            selectedWeightCalculation();
            var all = $id('ldAll'); if (all) all.checked = LD.rows.every(function (r) { return r._chk; });
        });
        $id('loaderHead').addEventListener('change', function (e) {
            if (e.target.id !== 'ldAll') return;
            var on = e.target.checked;
            $id('gridLoader').querySelectorAll('tr[data-i]').forEach(function (tr) {
                if (tr.style.display === 'none') return;           // the header selector checks the filtered rows
                LD.rows[+tr.getAttribute('data-i')]._chk = on;
                var cb = tr.querySelector('input[type="checkbox"]'); if (cb) cb.checked = on;
            });
            selectedWeightCalculation();
        });
        /* the fumigation loader grid - CellValueChanged / ColumnHeaderClick -> SelectedWeightCalculation */
        $id('gridFum').addEventListener('change', function (e) {
            var i = e.target.getAttribute('data-i');
            if (i === null) return;
            FM.rows[+i]._chk = e.target.checked;
            fumSelectedWeight();
            var all = $id('fmAll'); if (all) all.checked = FM.rows.every(function (r) { return r._chk; });
        });
        $id('fumHead').addEventListener('change', function (e) {
            if (e.target.id !== 'fmAll') return;
            var on = e.target.checked;
            $id('gridFum').querySelectorAll('tr[data-i]').forEach(function (tr) {
                if (tr.style.display === 'none') return;
                FM.rows[+tr.getAttribute('data-i')]._chk = on;
                var cb = tr.querySelector('input[type="checkbox"]'); if (cb) cb.checked = on;
            });
            fumSelectedWeight();
        });
        /* the short-fall loader grid */
        $id('gridSf').addEventListener('change', function (e) {
            var i = e.target.getAttribute('data-i');
            if (i === null) return;
            SF.rows[+i]._chk = e.target.checked;
            var all = $id('sfAll'); if (all) all.checked = SF.rows.every(function (r) { return r._chk; });
        });
        $id('sfHead').addEventListener('change', function (e) {
            if (e.target.id !== 'sfAll') return;
            var on = e.target.checked;
            SF.rows.forEach(function (r) { r._chk = on; });
            $id('gridSf').querySelectorAll('input[type="checkbox"]').forEach(function (cb) { cb.checked = on; });
        });
        /* the pick list */
        $id('pickFilter').addEventListener('input', drawPick);
        $id('pickBody').addEventListener('click', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) pickDone(+tr.getAttribute('data-i')); });
        $id('pickBody').addEventListener('keydown', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && e.key === 'Enter') { e.preventDefault(); e.stopPropagation(); pickDone(+tr.getAttribute('data-i')); } });

        var hb = $id('gridHistory');
        hb.addEventListener('click', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            var i = +tr.getAttribute('data-i'), act = e.target.closest('[data-act]'), r = HISTORY_ROWS[i];
            if (act && act.getAttribute('data-act') === 'print') { printId(netI(col(r, 'Id'))); return; }
            if (act && act.getAttribute('data-act') === 'edit') { readById(netI(col(r, 'Id'))); return; }
            historyRowSelected(i);
        });
        hb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('button')) readById(netI(col(HISTORY_ROWS[+tr.getAttribute('data-i')], 'Id'))); });
        hb.addEventListener('keydown', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr || !e.ctrlKey) return;
            var r = HISTORY_ROWS[+tr.getAttribute('data-i')];
            if (e.key === 'Enter' || e.key === ' ') { e.preventDefault(); e.stopPropagation(); readById(netI(col(r, 'Id'))); }
            else if (e.key === 'p' || e.key === 'P') { e.preventDefault(); e.stopPropagation(); printId(netI(col(r, 'Id'))); }
        });

        OHR = [newOhRow()]; PMR = [newPmRow()];
        Promise.all([
            loadEntryTypes().catch(function (e) { box('Entry Type: ' + e.message); }),
            loadLookups(true).catch(function (e) { box(e.message); }),
            loadEditSetup().catch(function (e) { box(e.message); })
        ]).then(function () {
            /* Load:618-619 - then CmbConversionType_Leave (:793) decides the final visibility. */
            show('btnIssuanceLoad', !!ES.issuanceByLoader);
            show('wrapFumigation', !!ES.issuanceByLoader && !!ES.stockReleaseFromFumigation);
            applyRightsToButtons();
            show('btnDelete', false);
            conversionTypeChanged();
            loadSchedules().catch(function (e) { box(e.message); });
            return btnNew();
        }).catch(function (e) { box(e.message); });
    }

    window.StockConversion = {
        load: readById, loadByDocNo: loadByDocNo, loadHistory: loadHistory, newHistory: newHistory,
        showView: showView, tab2: tab2, tab3: tab3, toggleFullscreen: toggleFullscreen,
        btnNew: btnNew, btnRefresh: btnRefresh, btnSave: btnSave, btnUpdate: btnUpdate, btnDelete: btnDelete,
        generate: generate, print: print, voucher: voucher, shortcuts: shortcuts,
        conversionTypeChanged: conversionTypeChanged, parentCategoryChanged: parentCategoryChanged,
        entryTypeChanged: entryTypeChanged, itemChanged: itemChanged, applyJobLotToGrid: applyJobLotToGrid,
        addInGrid: addInGrid, updateDetail: updateDetail, cancelDetail: cancelDetail, forwardRows: forwardRows,
        generateItemsAndUom: generateItemsAndUom, applyAll: applyAll, resetWages: resetWages,
        btnIssuanceLoad: btnIssuanceLoad, loaderSearch: loaderSearch, loaderReset: loaderReset, loaderClose: loaderClose, loaderLoad: loaderLoad,
        btnLoadOutPut: btnLoadOutPut, pickClose: pickClose,
        btnStockReleaseFromFumigation: btnStockReleaseFromFumigation, fumSearch: fumSearch, fumLoad: fumLoad, fumReset: fumReset, fumClose: fumClose,
        sfClose: sfClose, sfLoad: sfLoad, sfSearch: sfSearch, sfNew: sfNew, sfRefresh: sfRefresh, sfShortcuts: sfShortcuts,
        btnWagesSchedule: btnWagesSchedule, btnWagesExempt: btnWagesExempt
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
    else boot();
}());
