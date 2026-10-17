/* ============================================================================================
 * Screens 777 "Define Item" / 776 "Item Definition_Engr" — Architecture.WinApp.Inventory_Definition.frmAddItem_Engr
 * (base.Name "frmAddItem_Engr"). Line refs ":NNN" are frmAddItem_Engr.cs.
 *
 *   InvDefrmAddItem_Load :337        rights, combo fills, CompaniesBindInGrid :1029, rdWithVariant checked   (init)
 *   toolStripButton1_Click :1227     Refresh — every combo again, keeping the selected ids
 *   cmbItemCategory_Leave :974       codes + GL accounts of the category; cmbItemClass.Rows[1].Activate()
 *   Insert :1260                     precheck (FormValidationForAddItem) → "Are you sure to Save/Update?" → save → refresh :1150
 *   btnnew_Click :1198               refresh + clear Base Unit / Item Type / Item Class / Mother Item / Modal
 *   ReadById :1456                   (readById) — from the History grid double-click :1545
 *   tabCAddItem_SelectedIndexChanged :1537   History tab → grdfrmfill(50); btnLoadAll / btnsearch → grdfrmfill()
 *   InvDefrmAddItem_KeyDown :1784    Ctrl+S, Ctrl+N, Ctrl+T, Ctrl+E/Esc, Ctrl+Shift+F10 (Update ItemName group), Ctrl+U
 *   btnUomUpdate_Click :1752         Item.UpdateItemName per checked row
 * The Concrete page (screen 93, frmAddItemConcrete) is the same code with a few other fields; data-variant on #frmRoot
 * selects them (ENGR | CONCRETE).
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc;
    var root = $id('frmRoot');
    var api = root.getAttribute('data-api');
    var variant = root.getAttribute('data-variant');
    var ENGR = variant === 'ENGR';

    var rights = {};
    var look = {};
    var recId = 0;
    var companies = [];            // grdAllocation rows {Id, Location, Value}
    var historyRows = [], historyChecked = {}, nameEdits = {}, filters = {};
    var updateNameMode = false;    // grpUpdateItemName.Visible && Enabled

    // ------------------------------------------------------------------------------ helpers
    function fail(e) { alert(e && e.message ? e.message : String(e)); }
    function el(id) { return $id(id); }
    function val(id) { var e = el(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = el(id); if (e) e.value = v === null || v === undefined ? '' : String(v); }
    function chk(id) { var e = el(id); return !!(e && e.checked); }
    function setChk(id, v) { var e = el(id); if (e) e.checked = !!v; }
    function intOf(v) { var n = parseInt(v, 10); return isNaN(n) ? 0 : n; }
    function fmt(v, places) { return (Number(v) || 0).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: places }); }

    /* DDL.BindDDL(dt, cmb, id, name, caption, ZeroIndex) — a blank row first; the selected id is kept when it still exists (:411-432). */
    function bind(id, list) {
        var e = el(id);
        if (!e) return;
        var keep = e.value;
        e.innerHTML = '<option value="0"></option>';
        (list || []).forEach(function (r) { e.appendChild(new Option(String(r.Name), String(r.Id))); });
        if (keep && keep !== '0' && (list || []).some(function (r) { return String(r.Id) === keep; })) e.value = keep; else e.value = '0';
    }

    // ------------------------------------------------------------------------------ load
    function init() {
        bindTabs();
        C.getJson(api + '/lookups').then(function (d) {
            look = d; rights = d.rights || {};
            el('btnsave').disabled = !rights.save;          // :345
            el('btnprint').disabled = !rights.print;        // :346
            el('btnupdate').disabled = !rights.update;      // :347
            fillAll(d);
            companiesBindInGrid();                           // :367
            el('btnsave').classList.remove('is-hidden');     // :370
            el('btnupdate').classList.add('is-hidden');      // :371
            el('cmbItemCategory').focus();
            if (ENGR) setChk('rdWithVariant', true);         // :374 (Engr only)
            if (!ENGR) { setChk('chkCommissionOnPurchase', true); setChk('chkCommissionOnSale', true); }
        }).catch(fail);

        el('toolStripButton1').addEventListener('click', refreshCombos);
        el('btnnew').addEventListener('click', newClick);
        el('btnsave').addEventListener('click', function () { recId = 0; insert(); });   // BtnSave_Click :1431
        el('btnupdate').addEventListener('click', insert);                                // btnupdate_Click :1444
        el('cmbItemCategory').addEventListener('change', categoryLeave);
        el('btnLoadAll').addEventListener('click', function () { grdfrmfill(0); });
        el('btnsearch').addEventListener('click', function () { grdfrmfill(0); });
        el('btnUomUpdate').addEventListener('click', uomUpdate);
        document.querySelectorAll('.fx-btn[data-href]').forEach(function (b) {
            b.addEventListener('click', function () { window.open(b.getAttribute('data-href'), '_blank'); });
        });
        document.addEventListener('keydown', keyDown);
    }

    /* InvDefrmAddItem_Load :357-366 / toolStripButton1_Click :1231-1240 */
    function fillAll(d) {
        bind('cmbItemType', d.itemTypes);
        bind('cmbItemClass', d.itemClasses);
        bind('cmbPurchaseGL', d.purchaseAccounts);
        bind('cmbSaleGL', d.saleAccounts);
        bind('cmbCGSGL', d.cgsAccounts);
        bind('cmbBuyerOrSupplier', d.suppliers);
        bind('cmbManufacturerID', d.suppliers);
        bind('cmbcountry', d.countries);
        bind('cmbitemcathistory', d.categories);
        bind('cmbItemCategory', d.categories);
        if (ENGR) { bind('CmbModal', d.itemModels); bind('CmbMotherItem', d.motherItems); }
        else { bind('CmbQualityGrade', d.qcGrades); bind('CmbMaxMrmtUnit', d.uoms); }
        bind('cmbTaxType', d.taxTypes);
        bind('cmbBaseUnit', d.uoms);
    }
    function refreshCombos() {
        C.getJson(api + '/lookups').then(function (d) { look = d; fillAll(d); }).catch(fail);
    }

    /* CompaniesBindInGrid :1029 — Id (hidden), Location, Value (checkbox, header selector, position 0), every row ticked. */
    function companiesBindInGrid() {
        companies = (look.companies || []).map(function (c) { return { Id: c.Id, Location: c.Name, Value: true }; });
        renderAllocation();
    }
    function renderAllocation() {
        var t = el('grdAllocation');
        var all = companies.length > 0 && companies.every(function (c) { return c.Value; });
        t.tHead.innerHTML = '<tr><th style="width:40px"><input type="checkbox" id="allocAll"' + (all ? ' checked' : '') + '></th><th style="min-width:250px">Location</th></tr>';
        t.tBodies[0].innerHTML = companies.map(function (c, i) {
            return '<tr><td class="c"><input type="checkbox" data-a="' + i + '"' + (c.Value ? ' checked' : '') + '></td><td>' + esc(c.Location) + '</td></tr>';
        }).join('');
        t.tBodies[0].querySelectorAll('[data-a]').forEach(function (cb) {
            cb.onchange = function () { companies[parseInt(cb.getAttribute('data-a'), 10)].Value = cb.checked; renderAllocation(); };
        });
        var allCb = el('allocAll');
        if (allCb) allCb.onchange = function () { companies.forEach(function (c) { c.Value = allCb.checked; }); renderAllocation(); };
    }

    /* cmbItemCategory_Leave :974 — only while btnsave is visible and enabled; then cmbItemClass.Rows[1].Activate() */
    function categoryLeave() {
        var save = el('btnsave');
        var done = function () {
            var cls = el('cmbItemClass');
            if (cls.options.length > 1) cls.selectedIndex = 1;                       // :1007 Rows[1] = first data row
        };
        if (!save.classList.contains('is-hidden') && !save.disabled) {
            C.getJson(api + '/category-leave' + C.qs({ itemCategoryId: intOf(val('cmbItemCategory')) })).then(function (r) {
                if (r.ItemCode !== undefined) { setVal('txtItemCode', r.ItemCode); setVal('txtItemCodeNew', r.ItemCodeNew); }
                if (r.InventoryAccountId !== undefined) {
                    setVal('cmbPurchaseGL', r.InventoryAccountId); setVal('cmbSaleGL', r.RevenueAccountId); setVal('cmbCGSGL', r.CGSAccountId);
                }
                done();
            }).catch(function () { alert('Record Not Found'); });                     // :1011
        } else {
            done();
        }
    }

    // ------------------------------------------------------------------------------ save
    function variantValue() {
        if (chk('rdWithVariant')) return 'with';
        if (chk('rdWithoutVariant')) return 'without';
        return '';
    }
    /* Insert :1291-1353 — the Item fields the form sets (the server applies Conversion.ToInt / ToDouble / ToDecimal). */
    function body() {
        var b = {
            Id: recId,
            ItemCategoryId: intOf(val('cmbItemCategory')), ItemName: val('txtItemName'), ItemCode: val('txtItemCode'), ItemCodeNew: val('txtItemCodeNew'),
            ItemTypeId: intOf(val('cmbItemType')), ItemClassId: intOf(val('cmbItemClass')), BaseUnitId: intOf(val('cmbBaseUnit')),
            PurchaseGLAC: intOf(val('cmbPurchaseGL')), SaleGLAC: intOf(val('cmbSaleGL')), COGSGLAC: intOf(val('cmbCGSGL')),
            ApplyGST: chk('chbGST'), TaxTypeId: intOf(val('cmbTaxType')), Variant: variantValue(), ItemStatus: chk('chkstatus'),
            ManufactureId: intOf(val('cmbManufacturerID')), ManufacturePartNo: val('txtManufacturerPartNo'),
            BuyerSupplierId: intOf(val('cmbBuyerOrSupplier')), BuyerPartNo: val('txtBuyerPartNo'), ProductNo: val('txtProductNumber'),
            PurchaseRate: val('txtPurchaseRate'), MaxStockLevel: val('txtMaxStockLevel'), ReorderLevel: val('txtReorderLevel'),
            ItemOriginId: intOf(val('cmbcountry')), CostPrice: val('txtCostPrice'), CostUOMText: val('cmbCostUOM'),
            PurchaseUOMText: val('cmbPurchaseUOM'), SaleUOMText: val('cmbSaleUOM'), AmountCalcType: val('cmbAmountCalcType'),
            ApplyVAT: chk('chbVST'), ApplyExciese: chk('chbExcise'), OtherTax: chk('chbOther'),
            companies: companies.map(function (c) { return { Id: c.Id, Value: c.Value }; })
        };
        if (ENGR) {
            b.MotherItemId = intOf(val('CmbMotherItem')); b.itemModalId = intOf(val('CmbModal'));
            b.WeightSemiFinish = val('txtWeightSemiFinishDye'); b.WeightSemiFinishSand = val('txtWeightSemiFinishSand'); b.WeightFinishGoods = val('txtWeightFinishGoods');
        } else {
            b.ItemQcGradeId = intOf(val('CmbQualityGrade')); b.WeightKgs = val('txtWeightKgs'); b.MaxMeaurement = val('txtMaxMeaurement');
            b.MaxMeaurementUnitId = intOf(val('CmbMaxMrmtUnit')); b.CommOnSale = chk('chkCommissionOnSale'); b.CommOnPurchase = chk('chkCommissionOnPurchase');
        }
        return b;
    }

    /* Insert :1260 */
    function insert() {
        var b = body();
        C.postJson(api + '/precheck', b).then(function (pre) {
            if (!confirm(pre.update ? 'Are you sure to Update?' : 'Are you sure to Save?')) return null;   // :1281/:1287
            return C.postJson(api + '/save', b).then(function (res) {
                alert(res.message);                                                                          // :1413/:1421
                refresh();                                                                                   // :1423
            });
        }).catch(fail);
    }

    /* refresh :1150 — Item Category / Type / Class / Mother Item / Modal / Code (new) / Base Unit stay as they are. */
    function refresh() {
        el('cmbItemCategory').disabled = false;                 // cmbItemCategory.ReadOnly = false
        el('btnsave').classList.remove('is-hidden');
        el('btnupdate').classList.add('is-hidden');
        ['cmbAmountCalcType', 'cmbBuyerOrSupplier', 'cmbCGSGL', 'cmbCostUOM', 'cmbcountry', 'cmbManufacturerID', 'cmbPurchaseGL', 'cmbPurchaseUOM', 'cmbSaleGL', 'cmbSaleUOM']
            .forEach(function (id) { var e = el(id); if (e) e.value = e.tagName === 'SELECT' ? (e.options.length ? e.options[0].value : '') : ''; });
        ['txtItemCode', 'txtBuyerPartNo', 'txtCostPrice', 'txtItemName', 'txtManufacturerPartNo', 'txtMaxStockLevel', 'txtProductNumber', 'txtPurchaseRate', 'txtReorderLevel', 'txtSaleRate']
            .forEach(function (id) { setVal(id, ''); });
        setChk('chbExcise', false); setChk('chbGST', false); setChk('chbVST', false); setChk('chbOther', false);
        el('cmbItemCategory').focus();
        companiesBindInGrid();                                   // :1185
        categoryLeave();                                         // :1186
        el('panelDetails').classList.add('is-hidden');
        setChk('rdWithoutVariant', false); setChk('rdWithVariant', false);     // :1189-1190
    }

    /* btnnew_Click :1198 (C: also Max Meaurement, Weight, Max Unit, both commission checks ticked and shown) */
    function newClick() {
        refresh();
        ['cmbBaseUnit', 'cmbItemType', 'cmbItemClass', 'CmbMotherItem', 'CmbModal', 'CmbQualityGrade', 'CmbMaxMrmtUnit'].forEach(function (id) { var e = el(id); if (e) e.value = '0'; });
        if (!ENGR) {
            setVal('txtMaxMeaurement', ''); setVal('txtWeightKgs', '');
            setChk('chkCommissionOnPurchase', true); setChk('chkCommissionOnSale', true);
        }
    }

    // ------------------------------------------------------------------------------ read
    /* ReadById :1456 */
    function readById(id) {
        C.getJson(api + '/' + id).then(function (it) {
            recId = it.Id;
            el('btnsave').classList.add('is-hidden');
            el('btnupdate').classList.remove('is-hidden');
            tab('tabForm');                                                   // SelectedIndex = 0
            setVal('cmbItemCategory', it.ItemCategoryId); el('cmbItemCategory').disabled = true;   // ReadOnly = true
            setVal('txtItemCode', it.ItemCode); setVal('txtItemCodeNew', it.ItemCodeNew); setVal('txtItemName', it.ItemName);
            if (ENGR) {
                setVal('CmbMotherItem', it.MotherItemId);
                setVal('txtWeightSemiFinishDye', it.WeightSemiFinish); setVal('txtWeightSemiFinishSand', it.WeightSemiFinishSand); setVal('txtWeightFinishGoods', it.WeightFinishGoods);
                setVal('CmbModal', it.itemModalId);
            } else {
                setVal('CmbQualityGrade', it.ItemQcGradeId);
                setVal('txtWeightKgs', it.WeightKgs); setVal('txtMaxMeaurement', it.MaxMeaurement); setVal('CmbMaxMrmtUnit', it.MaxMeaurementUnitId);
                setChk('chkCommissionOnPurchase', it.CommOnPurchase); setChk('chkCommissionOnSale', it.CommOnSale);
            }
            setVal('cmbItemType', it.ItemTypeId); setVal('cmbItemClass', it.ItemClassId); setVal('cmbBaseUnit', it.BaseUnitId);
            setVal('cmbPurchaseGL', it.PurchaseGLAC); setVal('cmbSaleGL', it.SaleGLAC); setVal('cmbCGSGL', it.COGSGLAC);
            setChk('chbGST', it.ApplyGST); setVal('cmbTaxType', it.TaxTypeId); setChk('chkstatus', it.ItemStatus);
            setChk('rdWithVariant', !!it.ItemWithVarient); setChk('rdWithoutVariant', !it.ItemWithVarient);
            setVal('cmbManufacturerID', it.ManufactureId); setVal('txtManufacturerPartNo', it.ManufacturePartNo);
            setVal('cmbBuyerOrSupplier', it.BuyerSupplierId); setVal('txtBuyerPartNo', it.BuyerPartNo); setVal('txtProductNumber', it.ProductNo);
            setVal('txtMaxStockLevel', it.MaxStockLevel); setVal('txtReorderLevel', it.ReorderLevel); setVal('cmbcountry', it.ItemOriginId);
            setVal('txtCostPrice', it.CostPrice); setVal('cmbCostUOM', it.UOMScheduleIdCostRate);
            setVal('txtPurchaseRate', it.PurchasePrice); setVal('cmbPurchaseUOM', it.UOMScheduleIdPurRate);
            setVal('txtSaleRate', it.WholeSalePrice); setVal('cmbSaleUOM', it.UOMScheduleIdWhsRate);
            setVal('cmbAmountCalcType', it.AmountCalcType || '');
            setChk('chbVST', it.ApplyVAT); setChk('chbExcise', it.ApplyExciese); setChk('chbOther', it.OtherTax);
        }).catch(fail);
    }

    // ------------------------------------------------------------------------------ tabs / history
    function bindTabs() {
        document.querySelectorAll('.fx-tabs .win-tab').forEach(function (t) { t.addEventListener('click', function () { tab(t.getAttribute('data-tab')); }); });
    }
    function activeTab() { var a = document.querySelector('.fx-tabs .win-tab.is-active'); return a ? a.getAttribute('data-tab') : 'tabForm'; }
    function tab(name) {
        document.querySelectorAll('.fx-panel').forEach(function (p) { p.classList.toggle('is-active', p.id === name); });
        document.querySelectorAll('.fx-tabs .win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === name); });
        if (name === 'tabHistory') grdfrmfill(50);                     // tabCAddItem_SelectedIndexChanged :1537
    }

    /* grdfrmfill :1586 — dt columns per form; "Select" (ActAsSelector) only in the Update ItemName mode. */
    var HCOLS = ENGR ? [
        { k: 'ItemParentCategory', c: 'ItemParentCategory' }, { k: 'ItemCategory', c: 'ItemCategory' }, { k: 'ItemType', c: 'ItemType' },
        { k: 'MotherItem', c: 'MotherItem' }, { k: 'ItemClass', c: 'ItemClass' }, { k: 'ItemCode', c: 'ItemCode' }, { k: 'ItemCodeNew', c: 'Item Code' },
        { k: 'ItemName', c: 'ItemName', name: true }, { k: 'BaseUnit', c: 'BaseUnit' },
        { k: 'WeightSemiFinishDye', c: 'WeightSemiFinishDye', n: 4 }, { k: 'WeightSemiFinishSand', c: 'WeightSemiFinishSand', n: 4 }, { k: 'WeightFinishGoods', c: 'WeightFinishGoods', n: 4 },
        { k: 'ItemModal', c: 'ItemModal' }, { k: 'Active', c: 'Active' }, { k: 'StockAc', c: 'StockAc' }, { k: 'SaleAc', c: 'SaleAc' }, { k: 'CgsAc', c: 'CgsAc' },
        { k: 'HasVariant', c: 'HasVariant' }, { k: 'EntryDate', c: 'EntryDate' }, { k: 'EntryUser', c: 'EntryUser' }, { k: 'ModifyDate', c: 'ModifyDate' },
        { k: 'ModifyUser', c: 'ModifyUser' }, { k: 'NoOfAttachments', c: 'NoOfAttachments', link: true }
    ] : [
        { k: 'ItemCode', c: 'ItemCode' }, { k: 'ItemCodeNew', c: 'Item Code ' }, { k: 'ItemName', c: 'ItemName', name: true }, { k: 'ItemQcGrade', c: 'ItemQcGrade' },
        { k: 'ItemType', c: 'ItemType' }, { k: 'ItemCategory', c: 'ItemCategory' }, { k: 'ItemParentCategory', c: 'ItemParentCategory' }, { k: 'ItemClass', c: 'ItemClass' },
        { k: 'BaseUnit', c: 'BaseUnit' }, { k: 'WeightKgs', c: 'WeightKgs', n: 2 }, { k: 'MaxLength', c: 'MaxLength', n: 2 }, { k: 'MaxLengthUnit', c: 'MaxLengthUnit' },
        { k: 'ItemStatus', c: 'ItemStatus' }, { k: 'StockAc', c: 'StockAc' }, { k: 'SaleAc', c: 'SaleAc' }, { k: 'CgsAc', c: 'CgsAc' }, { k: 'ItemWithVarient', c: 'ItemWithVarient' },
        { k: 'EntryDate', c: 'EntryDate' }, { k: 'EntryUser', c: 'EntryUser' }, { k: 'NoOfAttachments', c: 'NoOfAttachments', link: true }
    ];

    function grdfrmfill(noOfRecords) {
        C.getJson(api + '/history' + C.qs({ noOfRecords: noOfRecords, itemCategoryId: intOf(val('cmbitemcathistory')) })).then(function (rows) {
            if (!rows.length) return;                                        // :1607 — an empty result leaves the grid as it was
            historyRows = rows; historyChecked = {}; nameEdits = {}; filters = {};
            renderHistory();
        }).catch(fail);
    }
    function visible() {
        var keys = Object.keys(filters).filter(function (k) { return filters[k]; });
        return historyRows.map(function (r, i) { return { r: r, i: i }; }).filter(function (x) {
            return keys.every(function (k) { return String(x.r[k] === null || x.r[k] === undefined ? '' : x.r[k]).toLowerCase().indexOf(filters[k].toLowerCase()) >= 0; });
        });
    }
    function renderHistory() {
        var t = el('grdhistory');
        var head = (updateNameMode ? '<th style="width:30px">Select</th>' : '') + HCOLS.map(function (c) { return '<th>' + esc(c.c) + '</th>'; }).join('');
        var flt = (updateNameMode ? '<td></td>' : '') + HCOLS.map(function (c) { return '<td><input type="text" data-f="' + c.k + '" value="' + esc(filters[c.k] || '') + '"></td>'; }).join('');
        t.tHead.innerHTML = '<tr>' + head + '</tr><tr class="flt">' + flt + '</tr>';
        var vis = visible();
        t.tBodies[0].innerHTML = vis.map(function (x) {
            var r = x.r, i = x.i;
            return '<tr data-i="' + i + '">' + (updateNameMode ? '<td class="c"><input type="checkbox" data-s="' + i + '"' + (historyChecked[i] ? ' checked' : '') + '></td>' : '') +
                HCOLS.map(function (c) {
                    var v = r[c.k];
                    if (c.name && updateNameMode) return '<td><input type="text" class="nm" data-n="' + i + '" value="' + esc(nameEdits[i] !== undefined ? nameEdits[i] : v) + '"></td>';
                    if (c.link) return '<td class="c"><a data-att="' + i + '">' + esc(v) + '</a></td>';
                    if (c.n !== undefined) return '<td class="n">' + esc(fmt(v, c.n)) + '</td>';
                    return '<td>' + esc(v === null || v === undefined ? '' : v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        el('lblNavigator').textContent = historyRows.length ? vis.length + ' of ' + historyRows.length + ' rows' : '';
        t.tHead.querySelectorAll('[data-f]').forEach(function (inp) {
            inp.oninput = function () { filters[inp.getAttribute('data-f')] = inp.value; var k = inp.getAttribute('data-f'); renderHistory(); var a = t.tHead.querySelector('[data-f="' + k + '"]'); if (a) { a.focus(); a.setSelectionRange(a.value.length, a.value.length); } };
        });
        t.tBodies[0].querySelectorAll('tr[data-i]').forEach(function (tr) {
            tr.ondblclick = function (e) {                                   // grdhistory_DoubleClick :1545
                if (e.target && (e.target.tagName === 'INPUT' || e.target.tagName === 'A')) return;
                readById(historyRows[parseInt(tr.getAttribute('data-i'), 10)].Id);
            };
            tr.onclick = function () { t.tBodies[0].querySelectorAll('tr.sel').forEach(function (x) { x.classList.remove('sel'); }); tr.classList.add('sel'); };
        });
        t.tBodies[0].querySelectorAll('[data-s]').forEach(function (cb) { cb.onchange = function () { historyChecked[cb.getAttribute('data-s')] = cb.checked; }; });
        t.tBodies[0].querySelectorAll('[data-n]').forEach(function (inp) { inp.oninput = function () { nameEdits[inp.getAttribute('data-n')] = inp.value; }; });
        t.tBodies[0].querySelectorAll('[data-att]').forEach(function (a) {
            a.onclick = function () { alert('Attachments are not ported to the web page'); };   // grdhistory_LinkClicked :1707 → GetNoofAttachmentsByScreenName
        });
    }

    /* btnUomUpdate_Click :1752 — "Please check row first", UpdateItemName(ItemName, ItemCode, Id) per checked row, then grdfrmfill() */
    function uomUpdate() {
        var rows = [];
        Object.keys(historyChecked).forEach(function (i) {
            if (!historyChecked[i]) return;
            var r = historyRows[parseInt(i, 10)];
            rows.push({ Id: r.Id, ItemName: nameEdits[i] !== undefined ? nameEdits[i] : r.ItemName, ItemCode: r.ItemCode });
        });
        if (!rows.length) { alert('Please check row first'); return; }
        C.postJson(api + '/update-names', rows).then(function (res) { alert(res.message); grdfrmfill(0); }).catch(fail);
    }

    /* Ctrl+Shift+F10 :1820 — toggles grpUpdateItemName (needs the Update right) and OnItemUpdateInHistory :1649 */
    function toggleUpdateNames() {
        if (!rights.update) return;
        updateNameMode = !updateNameMode;
        el('grpUpdateItemName').classList.toggle('is-hidden', !updateNameMode);
        renderHistory();
    }

    // ------------------------------------------------------------------------------ keys
    /* InvDefrmAddItem_KeyDown :1784 */
    function keyDown(e) {
        var k = (e.key || '').toLowerCase();
        var onForm = activeTab() === 'tabForm';
        if (e.ctrlKey && k === 's' && onForm && !el('btnsave').classList.contains('is-hidden')) { e.preventDefault(); recId = 0; insert(); return; }
        if (e.ctrlKey && k === 'n') { e.preventDefault(); newClick(); return; }
        if (e.ctrlKey && k === 't') { e.preventDefault(); tab(activeTab() === 'tabHistory' ? 'tabForm' : 'tabHistory'); return; }
        if ((e.ctrlKey && k === 'e') || k === 'escape') { e.preventDefault(); window.location.href = '/modules'; return; }
        if (e.ctrlKey && e.shiftKey && k === 'f10') { e.preventDefault(); toggleUpdateNames(); return; }
        if (e.ctrlKey && k === 'u' && !el('btnupdate').classList.contains('is-hidden')) { e.preventDefault(); insert(); }
    }

    init();
    var qid = parseInt(new URLSearchParams(window.location.search).get('id') || '0', 10);
    if (qid > 0) readById(qid);
})();
