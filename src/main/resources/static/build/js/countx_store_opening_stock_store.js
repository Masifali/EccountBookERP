/* ============================================================================================
 * Screen 341 "Opening Stock Store" — Architecture.WinApp.Inventory_Definition.frmStoreOpeningStockBalancing
 * (store path: ScreenName "frmStoreOpeningStockBalancing", BaseDocumentTypeId 1), DocumentTypeId 39.
 *
 * One record per document — no grid. The entry boxes ARE the document.
 *
 *   cmbItem ValueChanged + Leave → cmbItem_Leave:440   UOMs (ItemUomFromGlobalBind), warehouses, racks
 *   cmbWarehouse_Leave:484                               racks of the item in that warehouse
 *   CmbRackName_Leave:519                                warehouse = the rack's warehouse
 *   TextChanged handlers :1133-1189                      qty / secondary qty / per-item weight /
 *                                                        secondary rate / rate couple, then CalculateAmount
 *   Insert():583                                         ValidateControls list → confirm → save → FormReset
 *   ReadById:708                                         FormReset, then every box in the desktop's order
 *
 * Programmatic text changes fire the same handlers the desktop's TextChanged would (setText), and
 * the "active control" is the focused box's AccessibleName (data-acc), as in the desktop.
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc, intOf = C.intOf;
    var api = '/api/store/opening-stock-store';

    var look = { rights: {}, items: [], uoms: [], racks: [], jobLots: [], itemConditions: [], stockCreditAccounts: [] };
    var recId = 0;
    /* :58-64 — STATIC on the desktop: set once per page, never reset by FormReset or ReadById. */
    var flags = { byPerItem: true, bySecQty: false, bySecRate: true, byItemRate: false };
    var updRates = false, updSec = false;                // _isUpdatingRates / _isUpdatingSecondary
    var historyRows = [];
    var attach = { existing: [], removed: [], files: [] };     // the Attachment form's AT (lst / removed)
    var upload = { columns: [], rows: [] };                     // frmItemDefineExcelSheetUpload grid

    // ------------------------------------------------------------------------------ numbers

    var FLOAT = /^[+-]?(\d[\d,]*)?(\.\d*)?([eE][+-]?\d+)?$/;
    /** double.TryParse (NumberStyles.Float | AllowThousands) — NaN when it would fail. */
    function tryParse(t) {
        var s = String(t === null || t === undefined ? '' : t).trim();
        if (!s || !FLOAT.test(s) || !/\d/.test(s) || /^[+-]?,/.test(s)) return NaN;
        return parseFloat(s.replace(/,/g, ''));
    }
    /** Conversion.ToDouble — unparseable is 0. */
    function d(t) { var v = tryParse(t); return isNaN(v) ? 0 : v; }
    /** Math.Round(x, 3) — MidpointRounding.ToEven. */
    function round3(x) {
        var v = x * 1000, f = Math.floor(v), diff = v - f;
        var r = diff > 0.5 ? f + 1 : diff < 0.5 ? f : (f % 2 === 0 ? f : f + 1);
        return r / 1000;
    }
    /** .NET Framework ToString("#,##0.###") — 15 significant digits, half away from zero, grouped. */
    function fmt(x) {
        if (!isFinite(x)) return isNaN(x) ? 'NaN' : (x > 0 ? 'Infinity' : '-Infinity');
        var neg = x < 0, s = Math.abs(x).toPrecision(15);
        var out;
        if (/e/i.test(s)) {
            out = Math.abs(x).toFixed(3);
        } else {
            var parts = s.split('.'), ip = parts[0], fp = parts[1] || '';
            var keep = (fp + '000').slice(0, 3), next = fp.length > 3 ? fp.charAt(3) : '0';
            var n = BigInt(ip + keep);
            if (next >= '5') n += 1n;
            var digits = n.toString().padStart(4, '0');
            out = digits.slice(0, -3) + '.' + digits.slice(-3);
        }
        var ip2 = out.split('.')[0], fp2 = (out.split('.')[1] || '').replace(/0+$/, '');
        ip2 = ip2.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        var res = fp2 ? ip2 + '.' + fp2 : ip2;
        return neg && res !== '0' ? '-' + res : res;
    }

    // ------------------------------------------------------------------------------ text / combos

    var handlers = {};
    /** txt.Text = v — TextChanged fires only when the text actually changes. */
    function setText(id, v) {
        var el = $id(id);
        v = v === null || v === undefined ? '' : String(v);
        if (el.value === v) return;
        el.value = v;
        if (handlers[id]) handlers[id]();
    }
    function active() {
        var a = document.activeElement;
        return a && a.getAttribute ? a.getAttribute('data-acc') : null;
    }
    /** The combo's Text: the selected row's display text, "" when no row is active. */
    function comboText(id) {
        var el = $id(id), o = el.selectedOptions && el.selectedOptions[0];
        return o && o.value !== '' ? o.textContent : '';
    }
    function hasOption(el, v) {
        return Array.prototype.some.call(el.options, function (o) { return o.value === String(v) && o.value !== ''; });
    }
    /** combo.Text = "" — no active row (a hidden blank entry is selected; intOf('') is 0). */
    function clearText(id) {
        var el = $id(id), before = comboText(id);
        if (el.options.length) {
            if (!hasBlank(el)) el.insertAdjacentHTML('afterbegin', '<option value="" hidden></option>');
            el.value = '';
        }
        if (id === 'cmbRateUOM' && comboText(id) !== before) calcAmount();
    }
    function hasBlank(el) { return Array.prototype.some.call(el.options, function (o) { return o.value === ''; }); }
    /** combo.Value = v — a value not in the list leaves no active row. */
    function setValue(id, v) {
        var el = $id(id), before = comboText(id);
        if (hasOption(el, v)) el.value = String(v); else { clearText(id); return; }
        if (id === 'cmbRateUOM' && comboText(id) !== before) calcAmount();      // cmbRateUOM_TextChanged:1133
    }
    /**
     * InfragisticsHelper.BindAndRetainSelection: an empty source clears the combo (no rows at all);
     * otherwise a "-- Select --" row (value 0) is inserted first, the previous value is kept when
     * present, else row 0 is activated (ActivateRow true) or the text is left empty (ActivateRow false).
     */
    function bind(id, rows, valueKey, textKey, keepValue, activate) {
        var el = $id(id);
        if (!rows || !rows.length) { el.innerHTML = ''; return; }
        var h = '<option value="0">-- Select --</option>';
        rows.forEach(function (r) { h += '<option value="' + esc(r[valueKey]) + '">' + esc(r[textKey]) + '</option>'; });
        el.innerHTML = h;
        if (keepValue !== undefined && keepValue !== null && rows.some(function (r) { return String(r[valueKey]) === String(keepValue); })) el.value = String(keepValue);
        else if (activate === false) { el.insertAdjacentHTML('afterbegin', '<option value="" hidden></option>'); el.value = ''; }
        else el.value = '0';
    }
    /** The same, retaining by TEXT (CommonBindings.ItemUomFromGlobalBind → previousValueColumnName "UOMCode"). */
    function bindByText(id, rows, keepText) {
        var el = $id(id);
        if (!rows || !rows.length) { el.innerHTML = ''; return; }
        var h = '<option value="0">-- Select --</option>';
        rows.forEach(function (r) { h += '<option value="' + esc(r.Id) + '">' + esc(r.UOMCode) + '</option>'; });
        el.innerHTML = h;
        var hit = keepText ? rows.find(function (r) { return r.UOMCode === keepText; }) : null;
        el.value = hit ? String(hit.Id) : '0';
    }

    // ------------------------------------------------------------------------------ entry bar

    function itemNameBind() {                                                  // ItemNameBind:415
        bind('cmbItem', look.items, 'Id', $id('rdSearchByName').checked ? 'ItemName' : 'ItemCode', intOf($id('cmbItem').value), false);
    }
    function searchModeChanged() { itemNameBind(); }                          // rdSearchByName_CheckedChanged:435

    function itemLeave() {                                                     // cmbItem_Leave:440
        var itemId = intOf($id('cmbItem').value);
        var whId = intOf($id('cmbWarehouse').value);
        uomBind(itemId);
        warehouseBind(itemId);
        rackBind(itemId, whId);
    }
    /** CommonBindings.ItemUomFromGlobalBind(itemId, cmbUOM, cmbRateUOM, CmbSecondaryUom), setBaseValues = true. */
    function uomBind(itemId) {
        var rateBefore = comboText('cmbRateUOM');
        var list = look.uoms.filter(function (u) { return u.ItemId === itemId; });
        bindByText('cmbUOM', list, comboText('cmbUOM'));
        bindByText('cmbRateUOM', list, rateBefore);
        bindByText('CmbSecondaryUom', list, comboText('CmbSecondaryUom'));
        if (list.length) {
            var bp = list.find(function (u) { return u.BasePackUom; });
            var br = list.find(function (u) { return u.BaseRateUom; });
            var bs = list.find(function (u) { return u.BaseSecondaryUom; });
            if (bp) $id('cmbUOM').value = String(bp.Id);
            if (br) $id('cmbRateUOM').value = String(br.Id);
            if (bs) $id('CmbSecondaryUom').value = String(bs.Id);
        }
        if (comboText('cmbRateUOM') !== rateBefore) calcAmount();
    }
    function warehouseBind(itemId) {                                           // WareHouseBindFromGlobalRacksByItemId:456
        var whs = C.distinct(look.racks.filter(function (r) { return r.ItemId === itemId; }), 'WarehouseId')
            .map(function (r) { return { Id: r.WarehouseId, WareHouseName: r.WareHouseName }; });
        bind('cmbWarehouse', whs, 'Id', 'WareHouseName', intOf($id('cmbWarehouse').value));
    }
    function rackBind(itemId, whId) {                                          // RackBindFromGlobalRacksByItemId:491
        var racks = C.distinct(look.racks.filter(function (r) {
            return r.ItemId === itemId && (whId === 0 || r.WarehouseId === whId);
        }), 'Id');
        bind('CmbRackName', racks, 'Id', 'RackName', intOf($id('CmbRackName').value));
    }
    function warehouseLeave() {                                                // cmbWarehouse_Leave:484
        rackBind(intOf($id('cmbItem').value), intOf($id('cmbWarehouse').value));
    }
    function rackLeave() {                                                     // CmbRackName_Leave:519
        var rackId = intOf($id('CmbRackName').value);
        var r = look.racks.find(function (x) { return x.Id === rackId; });
        var wh = r ? r.WarehouseId : 0;
        if (wh > 0) setValue('cmbWarehouse', wh);
    }

    function jobLotBind() { bind('cmbJobLot', look.jobLots, 'Id', 'Description', intOf($id('cmbJobLot').value)); }
    function conditionBind() { bind('CmbItemCondition', look.itemConditions, 'Id', 'Description', intOf($id('CmbItemCondition').value)); }
    function stockCreditBind() { bind('cmbStockCreditAcc', look.stockCreditAccounts, 'Id', 'AccountTitle', intOf($id('cmbStockCreditAcc').value)); }

    // ------------------------------------------------------------------------------ calculations

    function calcAmount() {                                                    // CalculateAmount:1113
        /* ActiveRow != null — the "-- Select --" row counts (its Equivalent cell is DBNull → 0). */
        var el = $id('cmbRateUOM'), activeRow = el.options.length > 0 && el.value !== '';
        var rateUomId = intOf(el.value);
        var row = rateUomId ? look.uoms.find(function (u) { return u.Id === rateUomId; }) : null;
        var eq = row ? C.num(row.Equivalent) : 0;
        var qty = d($id('txtQty').value), rate = d($id('txtRate').value);
        if (activeRow && qty !== 0 && rate !== 0) $id('txtAmount').value = fmt(rate * (qty / eq));
        else $id('txtAmount').value = '0';
    }
    function secondaryCalc() {                                                 // SecondaryUomQtyAndPerItemSecondaryQtyCalculations:1191
        if (updSec) return;
        try {
            updSec = true;
            var perItem = d($id('txtPerItemWeight').value), total = d($id('txtSecondaryUomQty').value), qty = d($id('txtQty').value);
            if (qty <= 0) { setText('txtPerItemWeight', '0'); setText('txtSecondaryUomQty', '0'); return; }
            var act = active();
            if (act === 'PerItemWeight') {
                setText('txtSecondaryUomQty', fmt(round3(perItem * qty)));
                flags.byPerItem = true; flags.bySecQty = false;
            } else if (act === 'SecondaryUomQty') {
                setText('txtPerItemWeight', fmt(round3(total / qty)));
                flags.byPerItem = false; flags.bySecQty = true;
            } else if (flags.bySecQty) {
                setText('txtPerItemWeight', fmt(round3(total / qty)));
            } else if (flags.byPerItem) {
                setText('txtSecondaryUomQty', fmt(round3(perItem * qty)));
            }
        } finally { updSec = false; }
    }
    function rateCalc() {                                                      // ItemRateAndSecondaryRateCalculations:1241
        if (updRates) return;
        try {
            updRates = true;
            var itemRate = d($id('txtRate').value), secRate = d($id('txtSecondaryUomRate').value);
            var secQty = d($id('txtSecondaryUomQty').value), qty = d($id('txtQty').value);
            if (qty <= 0 && secQty <= 0) { setText('txtRate', '0'); setText('txtSecondaryUomRate', '0'); return; }
            var act = active();
            if (act === 'ItemRate') {
                setText('txtSecondaryUomRate', fmt(secQty > 0 ? round3(itemRate * (qty / secQty)) : 0));
                flags.byItemRate = true; flags.bySecRate = false;
            } else if (act === 'SecondaryUomRate') {
                setText('txtRate', fmt(qty > 0 ? round3(secRate * secQty / qty) : 0));
                flags.byItemRate = false; flags.bySecRate = true;
            } else if (flags.bySecRate) {
                setText('txtRate', fmt(qty > 0 ? round3(secRate * secQty / qty) : 0));
            } else if (flags.byItemRate) {
                setText('txtSecondaryUomRate', fmt(secQty > 0 ? round3(itemRate * (qty / secQty)) : 0));
            }
        } finally { updRates = false; }
    }
    handlers.txtQty = function () { rateCalc(); calcAmount(); };                                   // :1138
    handlers.txtSecondaryUomQty = function () { secondaryCalc(); rateCalc(); calcAmount(); };       // :1144
    handlers.txtSecondaryUomRate = function () { rateCalc(); calcAmount(); };                       // :1158
    handlers.txtRate = function () { rateCalc(); calcAmount(); };                                   // :1171
    handlers.txtPerItemWeight = function () { secondaryCalc(); rateCalc(); calcAmount(); };         // :1184

    // ------------------------------------------------------------------------------ reset / open

    function setMode(update) {
        $id('btnSave').classList.toggle('is-hidden', update);
        $id('btnUpdate').classList.toggle('is-hidden', !update);
        $id('lblRecId').textContent = update ? 'Record Id: ' + recId : ' ';
    }
    /** FormReset:768 — what it clears, nothing more (Doc Date, Remarks, Job Lot, … stay). */
    function formReset() {
        setMode(false);
        recId = 0;
        updRates = false; updSec = false;
        attach = { existing: [], removed: [], files: [] };                     // AT = new Attachment()
        setText('txtQty', '');
        clearText('cmbUOM');
        if ($id('cmbItem').value !== '') { clearText('cmbItem'); itemLeave(); } // ValueChanged → cmbItem_Leave
        setText('txtRate', '');
        $id('txtAmount').value = '';
        clearText('cmbRateUOM');
        var p = C.getJson(api + '/next-doc-no').then(function (r) { $id('txtDocNo').value = r.docNo; });
        loadHistory();
        $id('cmbWarehouse').focus();
        return p;
    }
    function newClick() { formReset().catch(function (e) { alert(e.message); }); }

    function open(id) {                                                        // ReadById:708
        return formReset().then(function () {
            return C.getJson(api + '/' + id);
        }).then(function (h) {
            recId = h.Id;
            setMode(true);
            $id('txtDocNo').value = h.DocNo;
            $id('txtDocdate').value = C.isoDay(h.DocDate);
            $id('txtRemarks').value = h.Remarks || '';
            var before = intOf($id('cmbItem').value);
            setValue('cmbItem', h.ItemId);
            if (intOf($id('cmbItem').value) !== before || before === 0) itemLeave(); // ValueChanged
            setValue('cmbWarehouse', h.WarehouseId);
            itemLeave();                                                       // :741
            setValue('CmbRackName', h.RackId);
            setValue('cmbJobLot', h.JobLotId);
            setValue('CmbItemCondition', h.ItemConditionId);
            setValue('cmbUOM', h.ItemUomSch);
            setText('txtQty', h.Qty);
            setValue('CmbSecondaryUom', h.SecondaryUomId);
            setText('txtSecondaryUomQty', h.SecondaryUomQty);
            setText('txtPerItemWeight', h.PerItemSecondaryUomQty);
            setText('txtSecondaryUomRate', h.SecondaryUomItemRate);
            setText('txtRate', h.ItemRate);
            setValue('cmbRateUOM', h.RateUomSch);
            $id('txtAmount').value = h.ItemAmount;
            setValue('cmbStockCreditAcc', h.StockCrGLAcId);
            window.scrollTo(0, 0);
            $id('cmbWarehouse').focus();
            return C.getJson(api + '/' + id + '/attachments').then(function (rows) {   // LoadAttachmentsForObject
                attach.existing = rows || [];
            });
        }).catch(function (e) { alert(e.message); });
    }

    // ------------------------------------------------------------------------------ save

    /** FormHelper.ValidateControls — the Insert():589 list, in order. */
    function validate() {
        var list = [
            ['txtDocNo', 'Document No', 'int'],
            ['cmbItem', 'Item', 'combo'],
            ['cmbWarehouse', 'Warehouse', 'combo'],
            ['CmbRackName', 'RackName', 'combo'],
            ['cmbJobLot', 'Job Lot', 'combo'],
            ['CmbItemCondition', 'Item Condition', 'combo'],
            ['cmbUOM', 'UOM', 'combo'],
            ['txtQty', 'Quantity', 'double'],
            ['txtRate', 'Rate', 'double'],
            ['cmbRateUOM', 'Rate UOM', 'combo'],
            ['txtAmount', 'Amount', 'double'],
            ['cmbStockCreditAcc', 'Stock Credit Account', 'combo']
        ];
        if (intOf($id('CmbItemCondition').value) === 4) {                     // :604
            list.splice(8, 0, ['CmbSecondaryUom', 'Secondary UOM', 'combo']);
            list.splice(9, 0, ['txtSecondaryUomQty', 'Secondary UOM Qty', 'double']);
            list.splice(10, 0, ['txtSecondaryUomRate', 'Secondary UOM Rate', 'double']);
        }
        for (var i = 0; i < list.length; i++) {
            var el = $id(list[i][0]), name = list[i][1], type = list[i][2], bad = false, msg;
            if (type === 'combo') { bad = intOf(el.value) === 0; msg = name + ' field is required'; }
            else if (type === 'int') { bad = !/^\s*[+-]?\d+\s*$/.test(el.value) || parseInt(el.value, 10) === 0; msg = name + ' must be a non-zero number'; }
            else { var v = tryParse(el.value); bad = isNaN(v) || v === 0; msg = name + ' must be a non-zero number'; }
            if (bad) { alert(msg); if (!el.disabled) el.focus(); return false; }
        }
        return true;
    }
    function payload() {
        return {
            Id: recId,
            DocDate: $id('txtDocdate').value,
            Remarks: $id('txtRemarks').value,
            ItemId: intOf($id('cmbItem').value),
            WarehouseId: intOf($id('cmbWarehouse').value),
            RackId: intOf($id('CmbRackName').value),
            JobLotId: intOf($id('cmbJobLot').value),
            ItemConditionId: intOf($id('CmbItemCondition').value),
            ItemUomSch: intOf($id('cmbUOM').value),
            Qty: $id('txtQty').value,
            SecondaryUomId: intOf($id('CmbSecondaryUom').value),
            SecondaryUomQty: $id('txtSecondaryUomQty').value,
            SecondaryUomRate: $id('txtSecondaryUomRate').value,
            Rate: $id('txtRate').value,
            RateUomSch: intOf($id('cmbRateUOM').value),
            Amount: $id('txtAmount').value,
            StockCrGLAcId: intOf($id('cmbStockCreditAcc').value),
            files: attach.files,
            removeAttachmentIds: attach.removed
        };
    }
    function insert() {                                                        // Insert():583
        if (!validate()) return;
        if (!confirm((recId === 0 ? 'Are you sure to Save' : 'Are you sure to Update') + '?')) return;
        C.postJson(api + '/save', payload()).then(function (r) {
            alert(r.message);
            return formReset();
        }).catch(function (e) { alert(e.message); });
    }
    function save() { recId = 0; insert(); }                                  // btnSave_Click:679
    function update() {                                                        // btnUpdate_Click:692
        if (recId === 0) { alert('No Record Found To update'); return; }
        insert();
    }
    function print() {                                                         // btnPrint_Click:1292
        if (recId === 0) { alert('PrintId not found...'); return; }
        C.printSlip(api + '/' + recId + '/slip', '416-InvStockOpeningBalanceHeader_Register');
    }
    function refresh() {                                                       // btnRefresh_Click:811
        C.getJson(api + '/globals').then(function (g) {
            Object.keys(g).forEach(function (k) { look[k] = g[k]; });
            itemNameBind(); jobLotBind(); conditionBind(); stockCreditBind();
        }).catch(function (e) { alert(e.message); });
    }

    // ------------------------------------------------------------------------------ history

    var COLS = [
        ['DocNo', 'n0'], ['DocDate', 'date'], ['ItemName'], ['Warehouse'], ['RackName'], ['JobLot'], ['ItemCondition'],
        ['ItemUOM'], ['Qty', 'n'], ['SecondaryUom'], ['SecondaryUomQty', 'n'], ['PerItemWeight', 'n'],
        ['SecondaryUomItemRate', 'n'], ['ItemRate', 'n'], ['UOM'], ['totalAmount', 'n'], ['StockCreditAccount'],
        ['Remarks'], ['Issued', 'n'], ['EntryDate', 'dt'], ['EntryUser'], ['ModifyDate', 'dt'], ['ModifyUser'],
        ['NoOfAttachments', 'n0']
    ];                                                                         // dtHistoryGrid; Id and IsApproved hidden (GridSetting:942/968)
    function cell(v, kind) {
        if (v === null || v === undefined || v === '') return '';
        if (kind === 'n') return fmt(C.num(v));
        if (kind === 'date') return C.gridDate(v);
        if (kind === 'dt') return C.gridDateTime(v, true);
        return String(v);
    }
    function renderHistory() {
        var t = $id('DataGridHistory');
        $id('historyNavigator').textContent = historyRows.length ? 'Records: ' + historyRows.length : '\u00a0';
        if (!historyRows.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; return; }   // ClearStructure
        t.tHead.innerHTML = '<tr>' + COLS.map(function (c) { return '<th>' + esc(c[0]) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = historyRows.map(function (r, i) {
            return '<tr data-i="' + i + '">' + COLS.map(function (c) {
                var v = esc(cell(r[c[0]], c[1]));
                if (c[0] === 'DocNo') return '<td class="num pick" data-open="' + r.Id + '">' + v + '</td>';
                if (c[0] === 'NoOfAttachments') return '<td class="num pick" data-att="' + r.Id + '">' + v + '</td>';   // LinkClicked:1081
                return '<td' + (c[1] === 'n' || c[1] === 'n0' ? ' class="num"' : '') + '>' + v + '</td>';
            }).join('') + '</tr>';
        }).join('');
    }
    function loadHistory() {                                                   // GridHistoryBind:861
        var q = {
            fromDate: $id('chkFromDateHistory').checked ? $id('FromDateHistory').value : '',
            toDate: $id('chkToDateHistory').checked ? $id('ToDateHistory').value : '',
            fromDocNo: $id('FromDocNoHistory').value,
            toDocNo: $id('ToDocNoHistory').value,
            accountId: intOf($id('CmbStockAccountHistory').value),
            itemStockAccountId: intOf($id('CmbItemStockAccountHistory').value),
            itemId: intOf($id('CmbItemNameHistory').value)
        };
        return C.getJson(api + '/history' + C.qs(q)).then(function (rows) {
            historyRows = rows || [];
            renderHistory();
        }).catch(function (e) { alert(e.message); });
    }
    function showHistory() { loadHistory(); }                                  // btnshow_Click:972
    function resetHistory() {                                                  // BtnResetHistory_Click:984
        clearText('CmbStockAccountHistory');
        clearText('CmbItemNameHistory');
        historyRows = [];
        renderHistory();
        $id('FromDateHistory').focus();
    }
    function historyComboBind(c) {                                             // HistoryComboBind:1026
        if (!c || c.empty) return;
        bind('CmbStockAccountHistory', c.stockAccounts, 'Id', 'Name', intOf($id('CmbStockAccountHistory').value), false);
        bind('CmbItemNameHistory', c.items, 'Id', 'Name', intOf($id('CmbItemNameHistory').value), false);
        bind('CmbItemStockAccountHistory', c.itemStockAccounts, 'Id', 'Name', intOf($id('CmbItemStockAccountHistory').value), false);
    }
    function refreshHistory() {                                                // BtnRefreshHistory_Click:999
        C.getJson(api + '/history-combos').then(historyComboBind).catch(function (e) { alert(e.message); });
    }

    /** The page footer's History button — the history is always on screen under the form. */
    function gotoHistory() {
        $id('historySection').scrollIntoView({ behavior: 'smooth', block: 'start' });
        $id('FromDateHistory').focus({ preventScroll: true });
    }

    // ------------------------------------------------------------------------------ attachments

    /** btnAttachment_Click — AT.Show(): the current document's list; add / remove until Save. */
    function attachment() {
        $id('attTitle').textContent = 'Attachments';
        $id('attAdd').style.display = '';
        $id('attNote').style.display = '';
        renderAttachments(false, recId);
        C.openModal('dlgAttachment');
    }
    /** NoOfAttachments link — GetNoofAttachmentsByScreenName(Id, base.Name): that record's files. */
    function viewAttachments(id) {
        C.getJson(api + '/' + id + '/attachments').then(function (rows) {
            $id('attTitle').textContent = 'Attachments';
            $id('attAdd').style.display = 'none';
            $id('attNote').style.display = 'none';
            renderAttachments(true, id, rows || []);
            C.openModal('dlgAttachment');
        }).catch(function (e) { alert(e.message); });
    }
    function renderAttachments(readOnly, docId, rows) {
        var t = $id('grdAttachments');
        t.tHead.innerHTML = '<tr><th>File</th><th>Entry Date</th><th></th></tr>';
        var list = readOnly ? rows.map(function (r) { return { kind: 'db', row: r }; })
            : attach.existing.filter(function (r) { return attach.removed.indexOf(r.Id) < 0; }).map(function (r) { return { kind: 'db', row: r }; })
                .concat(attach.files.map(function (f, i) { return { kind: 'new', i: i, row: { Attachment: f.name } }; }));
        t.tBodies[0].innerHTML = list.length ? list.map(function (x) {
            var name = esc(x.row.Attachment);
            var link = x.kind === 'db' ? '<a href="' + api + '/' + docId + '/attachments/' + x.row.Id + '">' + name + '</a>' : name + ' <i>(new)</i>';
            var del = readOnly ? '' : '<button type="button" class="cx-x" data-k="' + x.kind + '" data-v="' + (x.kind === 'db' ? x.row.Id : x.i) + '">x</button>';
            return '<tr><td>' + link + '</td><td>' + esc(C.gridDateTime(x.row.EntryDate || '', true)) + '</td><td>' + del + '</td></tr>';
        }).join('') : '<tr><td colspan="3">No attachments</td></tr>';
        t.tBodies[0].querySelectorAll('button[data-k]').forEach(function (b) {
            b.onclick = function () {
                var v = intOf(b.getAttribute('data-v'));
                if (b.getAttribute('data-k') === 'db') attach.removed.push(v); else attach.files.splice(v, 1);
                renderAttachments(false, docId);
            };
        });
    }
    function attachmentAdd() {
        var input = $id('attFiles'), files = Array.prototype.slice.call(input.files || []);
        if (!files.length) return;
        Promise.all(files.map(function (f) {
            return new Promise(function (resolve, reject) {
                if (f.size > 5 * 1024 * 1024) { reject(new Error('Attachment exceeds 5 MB')); return; }
                var rd = new FileReader();
                rd.onload = function () { resolve({ name: f.name, base64: String(rd.result).split(',')[1] || '' }); };
                rd.onerror = function () { reject(new Error('Could not read ' + f.name)); };
                rd.readAsDataURL(f);
            });
        })).then(function (list) {
            attach.files = attach.files.concat(list);
            input.value = '';
            renderAttachments(false, recId);
        }).catch(function (e) { alert(e.message); });
    }

    // ------------------------------------------------------------------------------ Upload Opening

    /** btnUploadOpening_Click — frmItemDefineExcelSheetUpload, ItemDefinitionType 2 (store). */
    function uploadOpening() { renderUpload(); C.openModal('dlgUpload'); }
    function uploadRead() {                                                    // btnUpload_Click_1
        var f = $id('upFile').files && $id('upFile').files[0];
        if (!f || !/\.xlsx$/i.test(f.name)) { alert('please choose  .xlsx file only.'); return; }
        var fd = new FormData();
        fd.append('file', f);
        var headers = { Accept: 'application/json' };
        var token = document.querySelector('meta[name="_csrf"]'), header = document.querySelector('meta[name="_csrf_header"]');
        if (token && header) headers[header.getAttribute('content')] = token.getAttribute('content');
        var btn = $id('btnUpload');
        btn.disabled = true;
        fetch(api + '/upload-opening/preview', { method: 'POST', body: fd, credentials: 'same-origin', headers: headers })
            .then(function (r) { return r.text().then(function (t) { var b = null; try { b = t ? JSON.parse(t) : null; } catch (e) { } if (!r.ok) throw new Error((b && b.message) || ('Request failed (' + r.status + ')')); return b; }); })
            .then(function (b) { upload = { columns: b.columns || [], rows: b.rows || [] }; renderUpload(); })
            .catch(function (e) { alert(e.message); })
            .then(function () { btn.disabled = false; });
    }
    function renderUpload() {                                                  // GenerateOneDt
        var t = $id('grdExcelDataView');
        if (!upload.rows.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; return; }
        t.tHead.innerHTML = '<tr>' + upload.columns.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = upload.rows.map(function (r) {
            return '<tr>' + upload.columns.map(function (c) { return '<td>' + esc(r[c]) + '</td>'; }).join('') + '</tr>';
        }).join('');
    }
    function uploadClear() { upload = { columns: [], rows: [] }; renderUpload(); }   // btnClear_Click
    function uploadSave() {                                                    // btnSave_Click:143
        if (!upload.rows.length) return;
        C.postJson(api + '/upload-opening/save', { rows: upload.rows }).then(function (r) {
            if (r && r.saved) { alert(r.message); uploadClear(); }
        }).catch(function (e) { alert(e.message); });
    }

    // ------------------------------------------------------------------------------ wiring / load

    function wire() {
        ['txtQty', 'txtSecondaryUomQty', 'txtSecondaryUomRate', 'txtRate', 'txtPerItemWeight'].forEach(function (id) {
            $id(id).addEventListener('input', function () { handlers[id](); });
        });
        $id('cmbItem').addEventListener('change', itemLeave);                 // ValueChanged
        $id('cmbItem').addEventListener('blur', itemLeave);                   // Leave
        $id('cmbWarehouse').addEventListener('change', warehouseLeave);
        $id('cmbWarehouse').addEventListener('blur', warehouseLeave);
        $id('CmbRackName').addEventListener('change', rackLeave);
        $id('CmbRackName').addEventListener('blur', rackLeave);
        $id('cmbRateUOM').addEventListener('change', calcAmount);             // TextChanged
        $id('DataGridHistory').addEventListener('click', function (e) {       // Doc No link
            var td = e.target.closest('td[data-open]');
            if (td) open(intOf(td.getAttribute('data-open')));
        });
        $id('DataGridHistory').addEventListener('click', function (e) {       // NoOfAttachments link
            var td = e.target.closest('td[data-att]');
            if (td) viewAttachments(intOf(td.getAttribute('data-att')));
        });
        $id('DataGridHistory').addEventListener('dblclick', function (e) {    // DataGridHistory_DoubleClick:1064
            var tr = e.target.closest('tbody tr[data-i]');
            if (tr && !e.target.closest('td[data-open],td[data-att]')) open(historyRows[intOf(tr.getAttribute('data-i'))].Id);
        });
    }

    function init() {
        wire();
        var today = C.today();
        $id('txtDocdate').value = today;                                       // DateTimePicker default: now
        $id('FromDateHistory').value = today;                                  // ShowCheckBox, Checked by default
        $id('ToDateHistory').value = today;
        C.getJson(api + '/lookups').then(function (l) {
            Object.keys(l).forEach(function (k) { look[k] = l[k]; });
            var r = look.rights || {};
            $id('btnSave').disabled = !r.save;                                 // :267 / :328
            $id('btnUpdate').disabled = !r.update;
            $id('btnPrint').disabled = !r.print;                               // :330
            historyComboBind(l.historyCombos);                                 // Form_Load:276
            if (recId === 0) $id('txtDocNo').value = l.docNo;                  // :332
            itemNameBind(); jobLotBind(); conditionBind(); stockCreditBind();
            var m = /[?&]id=(\d+)/.exec(location.search);
            var p = loadHistory();
            $id('txtDocdate').focus();
            if (m) return p.then(function () { return open(intOf(m[1])); });
        }).catch(function (e) { alert(e.message); });
    }
    document.addEventListener('DOMContentLoaded', init);

    window.OpenStore = {
        newClick: newClick, refresh: refresh, save: save, update: update, print: print,
        searchModeChanged: searchModeChanged, open: open, gotoHistory: gotoHistory,
        attachment: attachment, attachmentAdd: attachmentAdd, uploadOpening: uploadOpening, uploadRead: uploadRead,
        uploadClear: uploadClear, uploadSave: uploadSave,
        showHistory: showHistory, resetHistory: resetHistory, refreshHistory: refreshHistory
    };
})();
