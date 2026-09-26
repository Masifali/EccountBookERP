/* ============================================================================================
 * Screen 320 "Stock Adjustment" — Architecture.WinApp.StoreManagement.frmStockAdjustment,
 * DocumentTypeId 70. Line refs are frmStockAdjustment.cs unless stated.
 *
 * Two ways in, never mixed:
 *   - the Detail box (+ / double-click or Edit to change a manual row), btnplus_Click:903;
 *   - Loader (Entry Type "Loss" only) → LoadavailableTransactionsForIssuance, btnLoadAvailableStock_Click:2335.
 * Entry bar events: txtqty_TextChanged:2242, txtweight_TextChanged:2266, combitempck_Leave:2224,
 * CalculateWeight:2201, GetAvgRate:2146, AvailableStock:2282, CmbEntryType_Leave:670.
 * Grid edits: grd_CellUpdated:1158 (ItemQty, Weight, Comments, Account Title).
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc, num = C.num, intOf = C.intOf;
    var api = '/api/store/stock-adjustment';

    var look = { rights: {}, items: [], warehouses: [], jobLots: [], cropYears: [], packingTypes: [], entryTypes: [] };
    var accounts = [];          // dtAccounts
    var uoms = [];              // dtuomsch of the current item
    var table = [];             // the grid DataTable
    var recId = 0, voucherHeadId = 0, updateDetailIndex = -1;
    var rateToken = 0;
    var history = [], historySel = -1;
    var loaderRows = [], loaderChecked = {}, loaderFailed = null;

    function sel(id) { var e = $id(id); return e.selectedOptions && e.selectedOptions[0] ? e.selectedOptions[0].textContent.trim() : ''; }
    function val(id) { return intOf($id(id).value); }
    function nowIso() { var d = new Date(); return C.today() + 'T' + String(d.getHours()).padStart(2, '0') + ':' + String(d.getMinutes()).padStart(2, '0') + ':00'; }
    function hasRef(r) { return intOf(r.RefDocumentTypeId) !== 0 || intOf(r.RefDocNoId) !== 0 || intOf(r.RefDocSubIdNo) !== 0; }
    function fmt(v, d) { return num(v).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: d }); }
    function amt(v) { var d = intOf(look.amountRoundDigits); d = d >= 1 && d <= 4 ? d : 0; return num(v).toLocaleString('en-US', { minimumFractionDigits: d, maximumFractionDigits: d }); }
    function uomEq(id) { var u = uoms.find(function (x) { return x.Id === intOf(id); }); return u ? num(u.Equivalent) : 0; }
    function fillUom(elId, keepText) {
        C.fillSelect(elId, uoms, 'Id', 'UOMCode');
        var el = $id(elId); el.value = '0';
        for (var i = 0; i < el.options.length; i++) if (keepText && el.options[i].textContent.trim() === keepText) { el.selectedIndex = i; break; }
    }
    /* MidpointRounding.AwayFromZero */
    function roundAway(v, d) { var f = Math.pow(10, Math.max(0, d)); return Math.sign(v) * Math.round(Math.abs(v) * f) / f; }

    /* -------------------------------------------------------------------------------- load */

    function init() {
        C.getJson(api + '/lookups').then(function (l) {
            look = l;
            var r = l.rights || {};
            $id('btnsave').disabled = !r.save;
            $id('btnupdate').disabled = !r.update;
            $id('btnprint').disabled = !r.print;
            $id('ChkPrint').checked = !!r.print;                                   // :285 Load only
            $id('txtdocno').value = l.docNo > 0 ? l.docNo : '';
            $id('DocDate').value = C.today();
            $id('FromDateHistory').value = C.today();                              // designer: DateTime.Now, checked
            $id('ToDateHistory').value = C.today();
            C.fillSelect('CmbEntryType', l.entryTypes, 'Id', 'Name');
            C.fillSelect('CmbWareHouse', l.warehouses, 'Id', 'Name');
            C.fillSelect('CmbCropYear', l.cropYears, 'Id', 'Name');
            C.fillSelect('CmbJobLot', l.jobLots, 'Id', 'Name');
            C.fillSelect('CmbPackingType', l.packingTypes, 'Id', 'Name');
            itemBind();
            C.fillSelect('CmbAdjustmentTypeHistory', l.historyTypes, 'Id', 'Name');
            render();
        }).catch(function (e) { alert(e.message); });
    }

    function itemBind() { C.fillSelect('CmbItemName', look.items, 'Id', $id('rdbtnItemName').checked ? 'ItemName' : 'ItemCode'); }
    function selectedItem() { var id = val('CmbItemName'); return look.items.find(function (x) { return x.Id === id; }) || null; }
    function itemModeChanged() { var keep = $id('CmbItemName').value; itemBind(); $id('CmbItemName').value = keep; }   // :552

    /* CmbEntryType_Leave:670 — keeps the account when it is in the new list. */
    function entryTypeLeave() {
        var keep = val('CmbAccountTitle');
        return C.getJson(api + '/accounts' + C.qs({ entryTypeId: val('CmbEntryType') })).then(function (list) {
            accounts = list || [];
            C.fillSelect('CmbAccountTitle', accounts, 'Id', 'Name');
            $id('CmbAccountTitle').value = accounts.some(function (a) { return a.Id === keep; }) ? String(keep) : '0';
        }).catch(function (e) { alert(e.message); });
    }

    /* bindRateUomAndItemPackUom:581 — keeps the texts; an empty schedule leaves the old lists. */
    function bindUoms(itemId) {
        var packText = sel('CmbPackUom'), rateText = sel('CmbRateUom');
        return C.getJson(api + '/uoms' + C.qs({ itemId: itemId })).then(function (list) {
            if (!list || !list.length) return;
            uoms = list;
            fillUom('CmbPackUom', packText);
            fillUom('CmbRateUom', rateText);
        }).catch(function (e) { alert(e.message); });
    }

    function availableStock() {                                                   // :2282
        return C.getJson(api + '/stock-balance' + C.qs({ recId: recId, itemId: val('CmbItemName'), docDate: $id('DocDate').value,
            jobLotId: val('CmbJobLot'), warehouseId: val('CmbWareHouse'), cropYear: val('CmbCropYear') ? sel('CmbCropYear') : '' }))
            .then(function (r) {
                var l = $id('lblStockBalance');
                if (r.visible) { l.classList.remove('is-hidden'); l.textContent = fmt(r.balance, 2); } else l.classList.add('is-hidden');
            }).catch(function (e) { alert(e.message); });
    }

    function itemLeave() { bindUoms(val('CmbItemName')).then(function () { availableStock(); getAvgRate(); }); }   // :2321
    function stockLeave() { availableStock(); getAvgRate(); }                                                     // :2303-2319

    /* CalculateWeight:2201 */
    function calc() {
        var w = $id('txtweight').value, r = $id('txtItemRate').value;
        if (w !== '' && r !== '' && val('CmbRateUom') !== 0) {
            $id('txtItemAmount').value = String(num(w.trim()) / uomEq($id('CmbRateUom').value) * num(r.trim()));
        } else $id('txtItemAmount').value = '0';
    }
    function weightFromQty() {
        if (val('CmbPackUom') !== 0) $id('txtweight').value = String(uomEq($id('CmbPackUom').value) * num($id('txtqty').value));
    }
    function qtyChanged() { weightFromQty(); calc(); getAvgRate(); }             // :2242 (weight setter fires :2266 too)
    function weightChanged() { calc(); getAvgRate(); }                           // :2266
    function packUomLeave() {                                                   // :2224
        /* setting txtweight fires txtweight_TextChanged (:2266): CalculateWeight + GetAvgRate */
        var before = $id('txtweight').value;
        weightFromQty(); calc();
        if ($id('txtweight').value !== before) getAvgRate();
    }

    /* GetAvgRate:2146 — Loss only; the features blank the box and let the server price it. */
    function getAvgRate(jobLotOverride) {
        var box = $id('txtItemRate');
        if (val('CmbEntryType') !== 2) { box.disabled = false; return Promise.resolve(); }
        var rate = num(box.value);
        if (look.fifoMethod || look.avgFeatureMethod) {
            box.value = '0'; box.disabled = false; calc();
            var t = ++rateToken;
            return C.getJson(api + '/rate' + C.qs({ mode: 'entry', recId: recId, itemId: val('CmbItemName'), docDate: $id('DocDate').value,
                packUomId: val('CmbPackUom'), warehouseId: val('CmbWareHouse'), packingTypeId: val('CmbPackingType'),
                jobLotId: jobLotOverride !== undefined ? jobLotOverride : val('CmbJobLot'), cropYearId: val('CmbCropYear'),
                qty: num($id('txtqty').value), weight: num($id('txtweight').value) })).then(function (r) {
                if (t !== rateToken) return;
                if (num(r.rate) > 0) { box.value = String(r.rate); box.disabled = true; calc(); }
            }).catch(function (e) { alert(e.message); });
        }
        if (rate > 0) { box.value = String(rate); box.disabled = true; }
        return Promise.resolve();
    }
    /* What clearing the qty box does to the rate box (Reset/ResetDetail fire txtqty_TextChanged with
       no item): with a feature the server rate for item 0 is 0 → enabled; without, a rate > 0 disables it. */
    function rateStateOnClear(type) {
        var box = $id('txtItemRate');
        if (type !== 2) { box.disabled = false; return; }
        if (look.fifoMethod || look.avgFeatureMethod) { box.disabled = false; return; }
        if (num(box.value) > 0) box.disabled = true;
    }

    /* ---------------------------------------------------------------------------- entry bar */

    function validateDetail() {                                                   // FormValidationDetail:728
        var c = [['CmbWareHouse', 'WareHouseName field is required'], ['CmbItemName', 'ItemName field is required'],
            ['CmbCropYear', 'CropYear field is required'], ['CmbJobLot', 'JobLot field is required'],
            ['CmbPackingType', 'PackingType field is required'], ['CmbPackUom', 'PackUOM field is required']];
        for (var i = 0; i < c.length; i++) if (val(c[i][0]) === 0) { alert(c[i][1]); $id(c[i][0]).focus(); return false; }
        if (val('CmbEntryType') !== 2 && num($id('txtqty').value) === 0) { alert('ItemQty field is required'); $id('txtqty').focus(); return false; }
        if (num($id('txtweight').value) === 0) { alert('Weight field is required'); $id('txtweight').focus(); return false; }
        if (num($id('txtItemRate').value) === 0) { alert('ItemRate field is required'); $id('txtItemRate').focus(); return false; }
        if (val('CmbRateUom') === 0) { alert('RateUom field is required'); $id('CmbRateUom').focus(); return false; }
        if (num($id('txtItemAmount').value) === 0) { alert('ItemAmount field is required'); return false; }
        if (val('CmbAccountTitle') === 0) { alert('AccountTitle field is required'); $id('CmbAccountTitle').focus(); return false; }
        return true;
    }

    function add() {                                                              // btnplus_Click:903
        if (table.some(hasRef)) { alert("A record From loader exists in the grid, so you can't add a manual record."); return; }
        if (!validateDetail()) return;
        var it = selectedItem();
        table.push({
            Id: 0, RefDocumentTypeId: 0, RefDocNoId: 0, RefDocSubIdNo: 0, RefDocumentType: '', RefDocDate: nowIso(), RefDocNo: 0,
            WareHouseId: val('CmbWareHouse'), WareHouse: sel('CmbWareHouse'), ItemId: val('CmbItemName'),
            Item: it ? it.ItemName : '', ItemCode: it ? it.ItemCode : '', CropYearId: val('CmbCropYear'), CropYear: sel('CmbCropYear'),
            JobLotId: val('CmbJobLot'), JobLot: sel('CmbJobLot'), PackingTypeId: val('CmbPackingType'), PackingType: sel('CmbPackingType'),
            ItemUOMId: val('CmbPackUom'), ItemUOM: sel('CmbPackUom'), ItemUOMEquivalent: uomEq($id('CmbPackUom').value),
            ItemQty: num($id('txtqty').value), Weight: num($id('txtweight').value), ItemRate: num($id('txtItemRate').value),
            RateUomId: val('CmbRateUom'), RateUom: sel('CmbRateUom'), RateUomEquivalent: uomEq($id('CmbRateUom').value),
            ItemAmount: num($id('txtItemAmount').value), AccountId: val('CmbAccountTitle'), Comments: $id('txtremarksdetail').value.trim()
        });
        render();
        resetDetail();
        $id('CmbEntryType').disabled = true;
    }

    /* grd_DoubleClick:1039 */
    function editRow(i) {
        var r = table[i];
        if (!r) return;
        if (hasRef(r)) { alert("The record is from the loader, so you can't update it from the fields. You can change the Qty,Weight, and Account_Title in the grid."); return; }
        updateDetailIndex = i;
        var prevJobLot = val('CmbJobLot');
        $id('CmbWareHouse').value = String(r.WareHouseId);
        $id('CmbItemName').value = String(r.ItemId);
        bindUoms(r.ItemId).then(function () {
            $id('CmbPackUom').value = String(r.ItemUOMId);
            $id('CmbPackingType').value = String(r.PackingTypeId);
            $id('CmbCropYear').value = String(r.CropYearId);
            $id('txtqty').value = String(r.ItemQty);
            $id('txtweight').value = String(r.Weight);
            $id('txtItemRate').value = String(r.ItemRate);
            $id('txtItemAmount').value = String(r.ItemAmount);
            $id('CmbJobLot').value = String(r.JobLotId);
            $id('CmbRateUom').value = String(r.RateUomId);
            $id('CmbAccountTitle').value = String(r.AccountId);
            $id('txtremarksdetail').value = r.Comments || '';
            /* The qty / weight setters ran GetAvgRate before JobLot was set; only the box's state is
               taken from it — the row's rate and amount are restored after, as on the desktop. */
            var keepRate = $id('txtItemRate').value, keepAmt = $id('txtItemAmount').value;
            getAvgRate(prevJobLot).then(function () { $id('txtItemRate').value = keepRate; $id('txtItemAmount').value = keepAmt; });
            $id('btnplus').classList.add('is-hidden');
            $id('btnUpdateDetail').classList.remove('is-hidden');
            $id('btnCancelUpdateDetial').classList.remove('is-hidden');
        });
    }

    /* btnUpdateDetail_Click:1078 — the two equivalents are NOT refreshed (desktop). */
    function updateDetail() {
        if (!validateDetail()) return;
        var r = table[updateDetailIndex], it = selectedItem();
        if (!r) return;
        r.WareHouseId = val('CmbWareHouse'); r.WareHouse = sel('CmbWareHouse');
        r.ItemId = val('CmbItemName'); r.Item = it ? it.ItemName : ''; r.ItemCode = it ? it.ItemCode : '';
        r.ItemUOMId = val('CmbPackUom'); r.ItemUOM = sel('CmbPackUom');
        r.PackingTypeId = val('CmbPackingType'); r.PackingType = sel('CmbPackingType');
        r.CropYearId = val('CmbCropYear'); r.CropYear = sel('CmbCropYear');
        r.ItemQty = num($id('txtqty').value); r.ItemRate = num($id('txtItemRate').value);
        r.ItemAmount = num($id('txtItemAmount').value); r.Weight = num($id('txtweight').value);
        r.JobLotId = val('CmbJobLot'); r.JobLot = sel('CmbJobLot');
        r.RateUomId = val('CmbRateUom'); r.RateUom = sel('CmbRateUom');
        r.AccountId = val('CmbAccountTitle'); r.Comments = $id('txtremarksdetail').value;
        render();
        resetDetail();
    }

    /* ResetDetail:850 — crop year and rate uom are kept. */
    function resetDetail() {
        $id('CmbItemName').value = '0';
        $id('CmbPackUom').value = '0';
        $id('txtqty').value = '';
        rateStateOnClear(val('CmbEntryType'));
        $id('txtweight').value = '';
        $id('txtItemRate').value = '0';
        $id('txtItemAmount').value = '0';
        $id('CmbPackingType').value = '0';
        $id('txtremarksdetail').value = '';
        $id('CmbWareHouse').value = '0';
        $id('CmbJobLot').value = '0';
        $id('CmbAccountTitle').value = '0';
        $id('btnplus').classList.remove('is-hidden');
        $id('btnUpdateDetail').classList.add('is-hidden');
        $id('btnCancelUpdateDetial').classList.add('is-hidden');
    }

    /* Reset():815 — Doc Date, warehouse, job lot, crop year, rate uom and account are kept. */
    function reset() {
        var oldType = val('CmbEntryType');
        recId = 0; voucherHeadId = 0;
        $id('CmbEntryType').disabled = false;
        $id('txtItemRate').disabled = false;
        $id('CmbItemName').value = '0';
        $id('CmbPackUom').value = '0';
        $id('txtqty').value = '';
        rateStateOnClear(oldType);
        $id('txtweight').value = '';
        $id('CmbPackingType').value = '0';
        $id('txtremarks').value = '';
        $id('CmbEntryType').value = '0';
        $id('txtItemRate').value = '0';
        $id('txtItemAmount').value = '0';
        $id('btnsave').classList.remove('is-hidden');
        $id('btnupdate').classList.add('is-hidden');
        $id('btnplus').classList.remove('is-hidden');
        $id('btnUpdateDetail').classList.add('is-hidden');
        $id('btnCancelUpdateDetial').classList.add('is-hidden');
        $id('lblRecId').textContent = '';
        C.getJson(api + '/doc-no').then(function (r) { if (r.docNo > 0) $id('txtdocno').value = r.docNo; }).catch(function (e) { alert(e.message); });
        table = [];
        render();
    }

    /* btnRefresh_Click:875 — ItemBind appends to dtitem without clearing it, so items repeat (desktop). */
    function refresh() {
        bindUoms(val('CmbItemName'));
        C.getJson(api + '/refresh').then(function (l) {
            look.packingTypes = l.packingTypes; look.warehouses = l.warehouses; look.jobLots = l.jobLots;
            look.items = look.items.concat(l.items);
            C.fillSelect('CmbPackingType', l.packingTypes, 'Id', 'Name');
            C.fillSelect('CmbWareHouse', l.warehouses, 'Id', 'Name');
            C.fillSelect('CmbJobLot', l.jobLots, 'Id', 'Name');
            var keep = $id('CmbItemName').value; itemBind(); $id('CmbItemName').value = keep;
        }).catch(function (e) { alert(e.message); });
    }

    /* --------------------------------------------------------------------------------- grid */

    var COLS = [['RefDocumentType', 'RefDocumentType', 'ref'], ['RefDocDate', 'RefDocDate', 'ref'], ['RefDocNo', 'RefDocNo', 'ref'],
        ['WareHouse', 'WareHouse'], ['Item', 'Item'], ['ItemCode', 'ItemCode'], ['CropYear', 'CropYear'], ['JobLot', 'JobLot'],
        ['PackingType', 'PackingType'], ['ItemUOM', 'ItemUOM'], ['ItemQty', 'ItemQty', 'qty'], ['Weight', 'Weight', 'wt'],
        ['ItemRate', 'ItemRate', 'rate'], ['RateUom', 'RateUom'], ['ItemAmount', 'ItemAmount', 'amt'], ['AccountId', 'Account Title', 'acc'],
        ['Comments', 'Comments', 'cmt']];

    function render() {
        var showRef = table.some(function (r) { return intOf(r.RefDocumentTypeId) > 0; });   // grdSettings:958
        var cols = COLS.filter(function (c) { return c[2] !== 'ref' || showRef; });
        var g = $id('grd');
        g.querySelector('thead').innerHTML = '<tr><th>Delete</th><th>Edit</th>' + cols.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
        var accOpts = function (v) {
            return '<option value="0"></option>' + accounts.map(function (a) {
                return '<option value="' + a.Id + '"' + (a.Id === intOf(v) ? ' selected' : '') + '>' + esc(a.Name) + '</option>';
            }).join('');
        };
        g.querySelector('tbody').innerHTML = table.map(function (r, i) {
            return '<tr data-i="' + i + '" ondblclick="StockAdj.editRow(' + i + ')"><td><button type="button" class="cx-x" onclick="StockAdj.deleteRow(' + i + ')">Delete</button></td>' +
                '<td><button type="button" class="cx-link" onclick="StockAdj.editRow(' + i + ')">Edit</button></td>' +
                cols.map(function (c) {
                    var k = c[0], v = r[k];
                    if (c[2] === 'qty' || c[2] === 'wt') return '<td class="num"><input type="text" class="num" value="' + esc(v) + '" onchange="StockAdj.cell(' + i + ',\'' + k + '\',this.value)"></td>';
                    if (c[2] === 'cmt') return '<td><input type="text" value="' + esc(v || '') + '" onchange="StockAdj.cell(' + i + ',\'Comments\',this.value)"></td>';
                    if (c[2] === 'acc') return '<td><select class="win-combo" onchange="StockAdj.cell(' + i + ',\'AccountId\',this.value)">' + accOpts(v) + '</select></td>';
                    if (c[2] === 'rate') return '<td class="num">' + fmt(v, 3) + '</td>';
                    if (c[2] === 'amt') return '<td class="num">' + amt(v) + '</td>';
                    if (k === 'RefDocDate') return '<td>' + esc(C.gridDate(v)) + '</td>';
                    return '<td>' + esc(v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        var sum = function (k) { return table.reduce(function (s, r) { return s + num(r[k]); }, 0); };
        g.querySelector('tfoot').innerHTML = table.length ? '<tr><td colspan="2"></td>' + cols.map(function (c) {
            if (c[2] === 'qty' || c[2] === 'wt') return '<td class="num"><b>' + fmt(sum(c[0]), 2) + '</b></td>';
            if (c[2] === 'amt') return '<td class="num"><b>' + amt(sum('ItemAmount')) + '</b></td>';
            return '<td></td>';
        }).join('') + '</tr>' : '';
    }

    /* grd_CellUpdated:1158 */
    function cell(i, key, value) {
        var r = table[i];
        if (!r) return;
        if (key === 'ItemQty') {
            r.ItemQty = num(value);
            r.Weight = r.ItemQty * num(r.ItemUOMEquivalent);
            r.ItemAmount = r.Weight / num(r.RateUomEquivalent) * num(r.ItemRate);
        } else if (key === 'Weight') {
            r.Weight = num(value);
            r.ItemAmount = roundAway(r.Weight / num(r.RateUomEquivalent) * num(r.ItemRate), intOf(look.amountRoundDigits));
        } else if (key === 'AccountId') r.AccountId = intOf(value);
        else r[key] = value;
        render();
    }

    function deleteRow(i) {                                                        // grd_ColumnButtonClick:1123
        if (!confirm('Are you sure to Delete?')) return;
        table.splice(i, 1);
        if (!table.length) $id('CmbEntryType').disabled = false;
        render();
    }

    /* AvgRateUpdateOnDocDateChange:2414 — stops at the first loader row. */
    function generateRate() {
        if (!table.length || val('CmbEntryType') !== 2) return;
        var i = 0;
        function next() {
            if (i >= table.length) { render(); return; }
            var r = table[i++];
            if (hasRef(r)) { render(); return; }
            C.getJson(api + '/rate' + C.qs({ mode: 'grid', recId: recId, itemId: r.ItemId, docDate: $id('DocDate').value,
                packUomId: r.ItemUOMId, warehouseId: r.WareHouseId, packingTypeId: r.PackingTypeId, jobLotId: r.JobLotId,
                cropYearId: r.CropYearId, qty: num(r.ItemQty), weight: num(r.Weight) })).then(function (res) {
                var rate = num(res.rate);
                if (rate > 0) { r.ItemRate = rate; r.ItemAmount = num(r.Weight) / num(r.RateUomEquivalent) * rate; }
                next();
            }).catch(function (e) { render(); alert(e.message); });
        }
        next();
    }

    /* --------------------------------------------------------------------------------- save */

    function doSave(isUpdate) {                                                    // Insert():1195
        if (!table.length) { alert('Grid Record Not Found'); return; }
        var dn = $id('txtdocno').value.trim();
        if (dn === '' || dn === '0') { alert('DocNo Field is Required'); return; }
        if (val('CmbEntryType') === 0) { alert('Entry Type Field is Required'); $id('CmbEntryType').focus(); return; }
        if (!confirm(isUpdate ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var body = { Id: isUpdate ? recId : 0, DocDate: $id('DocDate').value, Remarks: $id('txtremarks').value,
            AdjustmentTypeId: val('CmbEntryType'), rows: table };
        C.postJson(api + '/save', body).then(function (r) {
            alert(r.message);
            var id = r.id;
            reset();
            if ($id('ChkPrint').checked) printId(id);
        }).catch(function (e) { alert(e.message); });
    }
    function save() { recId = 0; doSave(false); }                                  // btnsave_Click:1341
    function update() {                                                            // btnupdate_Click:1354
        if (recId === 0) { alert('Record Not Update because RecId Not Found'); return; }
        doSave(true);
    }

    function printId(id) {
        if (!id) { alert('No Record Found For Display'); return; }
        C.printSlip(api + '/' + id + '/slip', '409 - Stock Adjustment Slip');
    }
    function print() { printId(recId); }
    function voucherId(id) {
        if (!id) { alert('VoucherId Not Found'); return; }
        C.printSlip(api + '/voucher/' + id + '/slip', '118 - Voucher Slip');
    }
    function voucher() { voucherId(voucherHeadId); }

    /* ReadById:1370 */
    function open(id, vhId) {
        recId = id; voucherHeadId = intOf(vhId);
        C.getJson(api + '/' + id).then(function (h) {
            $id('CmbEntryType').disabled = true;
            tab('tabForm');
            $id('DocDate').value = C.isoDay(h.DocDate);
            $id('txtdocno').value = h.DocNo;
            $id('txtremarks').value = h.RemarksHeader || '';
            $id('CmbEntryType').value = String(h.AdjustmentTypeId);
            return entryTypeLeave().then(function () {
                table = h.rows || [];
                render();
                $id('btnsave').classList.add('is-hidden');
                $id('btnupdate').classList.remove('is-hidden');
                $id('lblRecId').textContent = 'Record Id: ' + id;
            });
        }).catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------------------ history */

    function tab(id) {
        document.querySelectorAll('.win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
        document.querySelectorAll('.win-tab-panel').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
    }

    function showHistory() {                                                      // gridhistoryfill:1492
        C.getJson(api + '/history' + C.qs({
            fromDate: $id('chkFromDate').checked ? $id('FromDateHistory').value : '',
            toDate: $id('chkToDate').checked ? $id('ToDateHistory').value : '',
            docNoFrom: intOf($id('FromDocNoHistory').value), docNoTo: intOf($id('ToDocNoHistory').value),
            adjustmentTypeId: val('CmbAdjustmentTypeHistory') })).then(function (rows) {
            history = rows || []; historySel = -1;
            renderHistory();
        }).catch(function (e) { alert(e.message); });
    }
    function renderHistory() {
        var g = $id('grdhistory');
        if (!history.length) { g.querySelector('thead').innerHTML = ''; g.querySelector('tbody').innerHTML = ''; return; }
        g.querySelector('thead').innerHTML = '<tr><th>Edit</th><th>Print</th><th>Voucher</th><th>DocDate</th><th>DocNo</th><th>RemarksHeader</th><th>EntryType</th><th>EnteryDate</th><th>EntryUser</th><th>ModifyDate</th><th>ModifyUser</th><th>NoOfAttachments</th></tr>';
        g.querySelector('tbody').innerHTML = history.map(function (r, i) {
            return '<tr data-i="' + i + '" class="' + (i === historySel ? 'is-selected' : '') + '" onclick="StockAdj.historySelect(' + i + ')" ondblclick="StockAdj.historyEdit(' + i + ')">' +
                '<td><button type="button" class="cx-link" onclick="event.stopPropagation();StockAdj.historyEdit(' + i + ')">Edit</button></td>' +
                '<td><button type="button" class="cx-link" onclick="event.stopPropagation();StockAdj.historyPrint(' + i + ')">Print</button></td>' +
                '<td><button type="button" class="cx-link" onclick="event.stopPropagation();StockAdj.historyVoucher(' + i + ')">Voucher</button></td>' +
                '<td>' + esc(C.gridDate(r.DocDate)) + '</td><td class="num">' + esc(r.DocNo) + '</td><td>' + esc(r.RemarksHeader) + '</td>' +
                '<td>' + esc(r.EntryType) + '</td><td>' + esc(C.gridDateTime(r.EnteryDate, true)) + '</td><td>' + esc(r.EntryUser) + '</td>' +
                '<td>' + esc(C.gridDateTime(r.ModifyDate, true)) + '</td><td>' + esc(r.ModifyUser) + '</td><td class="num">' + esc(r.NoOfAttachments) + '</td></tr>';
        }).join('');
    }
    /* grdhistory_SelectionChanged:1663 → GridDetailBind:1680, which also clears the FORM grid (desktop). */
    function historySelect(i) {
        historySel = i; renderHistory();
        var r = history[i];
        C.getJson(api + '/' + r.Id).then(function (h) {
            table = []; render();                                                  // GridDetailBind:1711 table.Rows.Clear()
            var rows = h.rows || [], g = $id('grdHistoryDetail');
            var ref = rows.some(function (x) { return intOf(x.RefDocNo) > 0; });
            var cols = (ref ? ['RefDocumentType', 'RefDocDate', 'RefDocNo'] : []).concat(['WareHouse', 'Item', 'ItemCode', 'CropYear', 'JobLot',
                'PackingType', 'ItemUOM', 'ItemQty', 'Weight', 'ItemRate', 'RateUom', 'ItemAmount', 'AccountTitle', 'Comments']);
            g.querySelector('thead').innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + c + '</th>'; }).join('') + '</tr>';
            function cellOf(x, c) {
                if (c === 'ItemQty' || c === 'Weight') return '<td class="num">' + fmt(x[c], 2) + '</td>';
                if (c === 'ItemRate') return '<td class="num">' + fmt(x[c], 3) + '</td>';
                if (c === 'ItemAmount') return '<td class="num">' + amt(x[c]) + '</td>';
                if (c === 'RefDocDate') return '<td>' + esc(C.gridDate(x[c])) + '</td>';
                return '<td>' + esc(x[c]) + '</td>';
            }
            /* designer: grdHistoryDetail.DoubleClick += grdhistory_DoubleClick — opens the selected history row */
            g.querySelector('tbody').innerHTML = rows.map(function (x) { return '<tr ondblclick="StockAdj.historyEdit(' + historySel + ')">' + cols.map(function (c) { return cellOf(x, c); }).join('') + '</tr>'; }).join('');
            var s = function (k) { return rows.reduce(function (a, x) { return a + num(x[k]); }, 0); };
            g.querySelector('tfoot').innerHTML = rows.length ? '<tr>' + cols.map(function (c) {
                if (c === 'ItemQty' || c === 'Weight') return '<td class="num"><b>' + fmt(s(c), 2) + '</b></td>';
                if (c === 'ItemAmount') return '<td class="num"><b>' + amt(s(c)) + '</b></td>';
                return '<td></td>';
            }).join('') + '</tr>' : '';
        }).catch(function (e) { alert(e.message); });
    }
    function historyEdit(i) { var r = history[i]; if (r) open(intOf(r.Id), r.VoucherHeadId); }
    function historyPrint(i) { var r = history[i]; if (r) printId(intOf(r.Id)); }
    function historyVoucher(i) { var r = history[i]; if (r) voucherId(intOf(r.VoucherHeadId)); }
    function newHistory() {                                                        // ResetHistory:1421
        var d = new Date(); d.setDate(d.getDate() - 3);
        $id('FromDateHistory').value = d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0');
        $id('ToDateHistory').value = C.today();
        $id('FromDocNoHistory').value = ''; $id('ToDocNoHistory').value = '';
        $id('CmbAdjustmentTypeHistory').value = '0';
        history = []; renderHistory();
        ['thead', 'tbody', 'tfoot'].forEach(function (s) { $id('grdHistoryDetail').querySelector(s).innerHTML = ''; });
    }
    function refreshHistory() {                                                    // btnRefreshHistory_Click:1456
        C.getJson(api + '/history-types').then(function (l) { C.fillSelect('CmbAdjustmentTypeHistory', l, 'Id', 'Name'); })
            .catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------------------- loader */

    function openLoader() {                                                        // btnLoadAvailableStock_Click:2335
        if (sel('CmbEntryType') !== 'Loss') { $id('CmbEntryType').focus(); alert("You can Only Load Data Against EntryType 'Loss'"); return; }
        if (table.some(function (r) { return intOf(r.RefDocumentTypeId) === 0 || intOf(r.RefDocNoId) === 0 || intOf(r.RefDocSubIdNo) === 0; })) {
            alert("A record without the loader exists in the grid, so you can't add a record from the loader."); return;
        }
        /* A new dialog per click: fresh filters each time. */
        loaderRows = []; loaderChecked = {}; loaderFailed = null;
        ['ldParent', 'ldCategory', 'ldType', 'ldJobLot', 'ldCrop', 'ldWarehouse', 'ldDocType', 'ldParty', 'ldItem'].forEach(function (id) { C.fillSelect(id, [], 'Id', 'Name'); });
        $id('ldFrom').value = C.today(); $id('ldTo').value = C.today();
        renderLoader();
        C.openModal('dlgLoader');
        C.getJson(api + '/loader/lookups').then(function (l) {
            if (l.failed) { loaderFailed = l.failed; alert(l.failed); return; }        // desktop note 14
            C.fillSelect('ldParent', l.ParentCategories, 'Id', 'Name');
            C.fillSelect('ldCategory', l.ItemCategories, 'Id', 'Name');
            C.fillSelect('ldType', l.ItemTypes, 'Id', 'Name');
            C.fillSelect('ldJobLot', l.JobLot, 'Id', 'Name');
            C.fillSelect('ldCrop', l.CropYear, 'Id', 'Name');
            C.fillSelect('ldWarehouse', l.Warehouse, 'Id', 'Name');
            C.fillSelect('ldDocType', l.DocumentType, 'Id', 'Name');
            C.fillSelect('ldParty', l.Supplier_Customer, 'Id', 'Name');
            C.fillSelect('ldItem', l.Items, 'Id', 'Name');
            if (l.fromDate) $id('ldFrom').value = l.fromDate;
            loaderSearch();
        }).catch(function (e) { alert(e.message); });
    }
    function loaderSearch() {                                                      // PendingInventoryTransactionsForIssuanceLoad:319
        C.getJson(api + '/loader/search' + C.qs({ fromDate: $id('ldFrom').value, toDate: $id('ldTo').value,
            parentCategoryId: val('ldParent'), itemCategoryId: val('ldCategory'), itemTypeId: val('ldType'), jobLotId: val('ldJobLot'),
            cropYear: val('ldCrop') ? sel('ldCrop') : '', warehouseId: val('ldWarehouse'), refDocumentTypeId: val('ldDocType'),
            supplierCustomerId: val('ldParty'), itemId: val('ldItem') })).then(function (rows) {
            loaderRows = rows || []; loaderChecked = {};
            renderLoader();
        }).catch(function (e) { alert(e.message); });
    }
    function loaderReset() { $id('ldParty').value = '0'; $id('ldItem').value = '0'; loaderRows = []; loaderChecked = {}; renderLoader(); }
    /* grdSettings:415 — AVgRate / ItemAmount hidden (FoodProductionWithValues.ValuesShowRights is false by default). */
    var LCOLS = ['RefDocumentType', 'DocDate', 'DocCodeNo', 'ManualNo', 'GrnNo', 'SupplierCustomerName', 'VehicleNo', 'BiltyNo', 'GpNo',
        'WareHouseCode', 'RefWarehouse', 'ItemName', 'ItemCode', 'CropBatch', 'JobLotCode', 'PackingType', 'PackUom',
        'QtyIn', 'QtyOut', 'QtyBalance', 'WeightIn', 'WeightOut', 'WeightBalance', 'RateUom', 'Remarks'];
    var LNUM = { QtyIn: 1, QtyOut: 1, QtyBalance: 1, WeightIn: 1, WeightOut: 1, WeightBalance: 1 };
    function renderLoader() {
        var g = $id('grdLoader');
        if (!loaderRows.length) { g.querySelector('thead').innerHTML = ''; g.querySelector('tbody').innerHTML = ''; selectedTotals(); return; }
        g.querySelector('thead').innerHTML = '<tr><th><input type="checkbox" onchange="StockAdj.loaderAll(this.checked)"></th>' +
            LCOLS.map(function (c) { return '<th>' + c + '</th>'; }).join('') + '</tr>';
        g.querySelector('tbody').innerHTML = loaderRows.map(function (r, i) {
            return '<tr><td><input type="checkbox"' + (loaderChecked[i] ? ' checked' : '') + ' onchange="StockAdj.loaderCheck(' + i + ',this.checked)"></td>' +
                LCOLS.map(function (c) {
                    if (LNUM[c]) return '<td class="num">' + fmt(r[c], 2) + '</td>';
                    if (c === 'DocDate') return '<td>' + esc(C.gridDate(r[c])) + '</td>';
                    return '<td>' + esc(r[c] === null || r[c] === undefined ? '' : r[c]) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        selectedTotals();
    }
    /* SelectedWeightCalculation:636 — Math.Round(...).ToString("0,0"). */
    function selectedTotals() {
        var w = 0, q = 0;
        loaderRows.forEach(function (r, i) { if (loaderChecked[i]) { w += num(r.WeightBalance); q += num(r.QtyBalance); } });
        function f(v) { var n = Math.round(v); if (Math.abs(v % 1) === 0.5) n = 2 * Math.round(v / 2); var s = Math.abs(n).toLocaleString('en-US'); if (s.length < 2) s = '0' + s; return (n < 0 ? '-' : '') + s; }
        $id('txtSelectedStock').value = f(w);
        $id('txtSelectedQty').value = f(q);
    }
    function loaderCheck(i, on) { loaderChecked[i] = on; selectedTotals(); }
    function loaderAll(on) { loaderRows.forEach(function (r, i) { loaderChecked[i] = on; }); renderLoader(); }

    /* btnLoadOnInvoice_Click_1:566 → LoadDetailDataFromavailableTransactions:2367 */
    function loaderLoad() {
        var picked = loaderRows.filter(function (r, i) { return loaderChecked[i]; });
        if (!picked.length) { alert('Please Select Row first'); return; }
        if (loaderFailed) { alert('Input array is longer than the number of columns in this table.'); return; }
        C.closeModal('dlgLoader');
        picked.forEach(function (r) {
            var dup = table.some(function (x) {
                return intOf(x.RefDocumentTypeId) === intOf(r.RefDocumentTypeId) && intOf(x.RefDocNoId) === intOf(r.RefDocIdNo)
                    && intOf(x.RefDocSubIdNo) === intOf(r.RefDocSubIdNo);
            });
            if (dup) return;
            table.push({
                Id: 0, RefDocumentTypeId: intOf(r.RefDocumentTypeId), RefDocNoId: intOf(r.RefDocIdNo), RefDocSubIdNo: intOf(r.RefDocSubIdNo),
                RefDocumentType: r.RefDocumentType || '', RefDocDate: r.DocDate, RefDocNo: intOf(r.DocCodeNo),
                WareHouseId: intOf(r.WarehouseId), WareHouse: r.WareHouseCode || '', ItemId: intOf(r.ItemId), Item: r.ItemName || '',
                ItemCode: r.ItemCode || '', CropYearId: intOf(r.CropYearId), CropYear: r.CropBatch || '', JobLotId: intOf(r.JobLotId),
                JobLot: r.JobLotCode || '', PackingTypeId: intOf(r.InvPackingTypeId), PackingType: r.PackingType || '',
                ItemUOMId: intOf(r.ItemUom), ItemUOM: r.PackUom || '', ItemUOMEquivalent: num(r.PackSize),
                ItemQty: num(r.QtyBalance), Weight: num(r.WeightBalance), ItemRate: num(r.AVgRate), RateUomId: intOf(r.RateUomId),
                RateUom: r.RateUom || '', RateUomEquivalent: num(r.Equivalent), ItemAmount: num(r.ItemAmount), AccountId: 0, Comments: ''
            });
        });
        render();
        $id('CmbEntryType').disabled = true;
    }

    window.StockAdj = {
        reset: reset, refresh: refresh, save: save, update: update, print: print, voucher: voucher,
        openLoader: openLoader, generateRate: generateRate, tab: tab, entryTypeLeave: entryTypeLeave,
        stockLeave: stockLeave, itemModeChanged: itemModeChanged, itemLeave: itemLeave, packUomLeave: packUomLeave,
        qtyChanged: qtyChanged, weightChanged: weightChanged, calc: calc, add: add, updateDetail: updateDetail,
        resetDetail: resetDetail, editRow: editRow, deleteRow: deleteRow, cell: cell,
        showHistory: showHistory, newHistory: newHistory, refreshHistory: refreshHistory, historySelect: historySelect,
        historyEdit: historyEdit, historyPrint: historyPrint, historyVoucher: historyVoucher,
        loaderSearch: loaderSearch, loaderReset: loaderReset, loaderLoad: loaderLoad, loaderCheck: loaderCheck, loaderAll: loaderAll
    };
    document.addEventListener('DOMContentLoaded', init);
})();
