/* frmItemPricingSchedule (Lookups/frmItemPricingSchedule.cs) — ScreenDefinition 91 "Item Pricing Schedule".
   Line refs ":NNN" are that file. API: /inventory/item-pricing-schedule/api.
     frmItemPricingSchedule_Load :189   ParentCategoryBind (Rows[1] active) → ItemCategoryByParentIdFill → ItemFill(parent)
     CmbParentCatagory_Leave :723       ItemCategoryByParentIdFill + ItemFill
     cmbItemCategory_Leave :828         ItemTypeByCategoryIdFill (the type table carries ItemCategoryId 0 → falls back to all)
     cmbItem Leave :546                 GetUomScheduleByItemId → CmbRateUom
     CmbRateUom_Leave :736              GetLastItemRateByItemIdAndUomId → txtLastRate / datPreviousDate
     btnAdd_Click :588                  FormValidation → duplicate-item check → dt.Rows.Add → GridSetting
     btnSearch_Click :770               GetItemWithRatesForItemPricingSchedule → dt (applyRateUom, Show Only Rated Items)
     Insert :432                        "Grid record not found" → "Are you sure to Save?" → save → "Save Successfully" → Reset
     tab History :704                   FormHistory;  btnPrintHistory :916 → 325-ItemPricingList.rpt
     KeyDown :383                       Ctrl+S save (Form tab), Ctrl+N new, Ctrl+T tab toggle, Ctrl+E / Esc close, Ctrl+U update */
(function () {
    'use strict';
    var SC = window.StoreCommon, X = window.Inv4a, $id = SC.$id, API = '/inventory/item-pricing-schedule/api';

    var st = { rights: {}, categories: [], itemTypes: [], items: [], rows: [], history: [], recId: 0 };

    /* grd (dt :196-205) — Delete button at position 0; hidden ItemId, RateUomId, BaseUomId, BaseUom; grouped by ItemCategory */
    var COLS = [
        { key: 'Delete', cap: 'Delete', btn: 'X' },
        { key: 'ItemId', hidden: true }, { key: 'RateUomId', hidden: true }, { key: 'BaseUomId', hidden: true },
        { key: 'ItemCategory', cap: 'ItemCategory', width: 150 },
        { key: 'ItemName', cap: 'ItemName', width: 350 },
        { key: 'BaseUom', hidden: true },
        { key: 'PreviousDate', cap: 'PreviousDate', date: X.ddMMMyyyy },
        { key: 'PreviousRate', cap: 'PreviousRate', fmt: '#,#.##' },
        { key: 'RateUom', cap: 'RateUom', cls: 'n' },
        { key: 'NewPrice', cap: 'NewPrice', edit: 'num' }
    ];
    function renderGrid() {
        X.renderGrid('grd', COLS, { rows: st.rows, group: 'ItemCategory', groupCaption: 'ItemCategory', filter: true, navId: 'lblNavigator',
            onEdit: function (i, key, v) { if (key === 'NewPrice') st.rows[i].NewPrice = X.num(v); },
            onButton: function (i) { grdDelete(i); } });
    }
    /* grd_ColumnButtonClick :859 — only while Save is visible and enabled */
    function grdDelete(i) {
        var save = $id('btnsave');
        if (!save.classList.contains('is-hidden') && !save.disabled) { st.rows.splice(i, 1); renderGrid(); }
        else alert('You Can not Delete Detail Record In Update Mode... ');
    }

    /* ParentCategoryBind :245 — ids 1/2/4 (server), then Rows[1].Activate() */
    function parentCategoryBind(list) {
        X.bindDdl('CmbParentCatagory', list);
        var el = $id('CmbParentCatagory');
        if (list && list.length) el.value = String(list[Math.min(1, list.length - 1)].Id);
    }
    /* ItemCategoryByParentIdFill :294 — rows whose InventoryParentCategoriesId = parent, else the whole table */
    function itemCategoryByParentIdFill() {
        var pid = X.selVal('CmbParentCatagory');
        var dt = st.categories.filter(function (r) { return r.InventoryParentCategoriesId === pid; });
        if (!dt.length) dt = st.categories;
        X.bindDdl('cmbItemCategory', dt, 'Id', 'CategoryDescription');
        X.bindDdl('CmbItemCategoryHistory', dt, 'Id', 'CategoryDescription');
    }
    /* ItemTypeByCategoryIdFill :321 — filtered on the table's ItemCategoryId (always 0 from the procedure) */
    function itemTypeByCategoryIdFill() {
        var cid = X.selVal('cmbItemCategory');
        var dt = st.itemTypes.filter(function (r) { return r.ItemCategoryId === cid; });
        if (!dt.length) dt = st.itemTypes;
        X.bindDdl('cmbItemType', dt, 'Id', 'TypeDescription');
        X.bindDdl('CmbItemTypeHistory', dt, 'Id', 'TypeDescription');
    }
    /* ItemFill :217 */
    function itemFill() {
        var pid = X.selVal('CmbParentCatagory');
        if (!pid) { st.items = []; X.bindDdl('cmbItem', []); return Promise.resolve(); }
        return SC.getJson(API + '/items' + SC.qs({ parentId: pid })).then(function (list) { st.items = list || []; X.bindDdl('cmbItem', st.items, 'Id', 'ItemName'); }).catch(X.fail);
    }
    function itemRow() { var id = X.selVal('cmbItem'); return st.items.filter(function (r) { return r.Id === id; })[0] || null; }
    /* cmbItem_ValueChanged :546 */
    function cmbItemChanged() {
        var id = X.selVal('cmbItem');
        if (!id) { st.uoms = []; X.bindDdl('CmbRateUom', []); return; }
        SC.getJson(API + '/uoms' + SC.qs({ itemId: id })).then(function (list) { st.uoms = list || []; X.bindDdl('CmbRateUom', st.uoms, 'Id', 'UOMCode'); }).catch(X.fail);
    }
    function uomRow() { var id = X.selVal('CmbRateUom'); return (st.uoms || []).filter(function (r) { return r.Id === id; })[0] || null; }
    /* CmbRateUom_Leave :736 */
    function rateUomLeave() {
        SC.getJson(API + '/last-rate' + SC.qs({ itemId: X.selVal('cmbItem'), rateUomId: X.selVal('CmbRateUom') })).then(function (d) {
            $id('txtLastRate').value = d.ItemRate; $id('datPreviousDate').value = d.PreviousDate || SC.today();
        }).catch(X.fail);
    }

    /* FormValidation :681 */
    function formValidation() {
        if (!X.hasActiveRow('cmbItem')) { $id('cmbItem').focus(); alert('Item Field Required'); return false; }
        if (X.num($id('txtNewPrice').value) === 0) { $id('txtNewPrice').focus(); alert('Price Field Required'); return false; }
        if (!X.hasActiveRow('CmbRateUom')) { $id('CmbRateUom').focus(); alert('RateUom Field Required'); return false; }
        return true;
    }
    /* btnAdd_Click :588 */
    function btnAdd() {
        try {
            if (!formValidation()) return;
            var itemId = X.selVal('cmbItem');
            if (st.rows.some(function (r) { return r.ItemId === itemId; })) { $id('cmbItem').focus(); throw new Error('You cannot add same item With Same CropYear in grid'); }
            var ir = itemRow(), ur = uomRow();
            st.rows.push({ ItemId: itemId, RateUomId: X.selVal('CmbRateUom'), BaseUomId: 0, ItemCategory: ir ? ir.ItemCategory : '', ItemName: X.selText('cmbItem'), BaseUom: 0,
                PreviousDate: $id('datPreviousDate').value, PreviousRate: X.num($id('txtLastRate').value), RateUom: ur ? X.toInt(ur.Equivalent) : 0, NewPrice: X.num($id('txtNewPrice').value) });
            renderGrid();
            resetDetail();
        } catch (e) { X.fail(e); }
    }
    /* ResetDetail :617 */
    function resetDetail() { $id('cmbItem').value = ''; $id('CmbRateUom').value = ''; $id('txtNewPrice').value = ''; $id('cmbItem').focus(); }
    /* Reset :567 */
    function reset() {
        st.rows = []; X.clearGrid('grd'); $id('lblNavigator').textContent = '';
        $id('cmbItem').value = ''; $id('txtNewPrice').value = ''; $id('txtLastRate').value = '';
        $id('btnsave').classList.remove('is-hidden'); $id('btnUpdate').classList.add('is-hidden');
        st.recId = 0; $id('btnAdd').disabled = false; $id('cmbItem').focus();
    }

    /* btnSearch_Click :770 */
    function btnSearch() {
        SC.postJson(API + '/search', { parentCategoryId: X.selVal('CmbParentCatagory'), itemCategoryId: X.selVal('cmbItemCategory'), itemTypeId: X.selVal('cmbItemType'),
            applyDate: $id('chkApplyDate').checked, effectedDate: $id('datEffectedForFilter').value }).then(function (data) {
            if (data && data.length) {
                var apply = X.num($id('txtApplyRateUom').value.trim());
                if (apply === 0) apply = 40;                                               // :791-794
                var dt = data.map(function (r) {
                    if (X.num(r.RateUom) !== 0 && apply === 0) apply = X.num(r.RateUom);  // :797 (never true after the 40 default)
                    return { ItemId: r.ItemId, RateUomId: r.RateUomId, BaseUomId: r.BaseUomId, ItemCategory: r.ItemCategory, ItemName: r.ItemName, BaseUom: r.BaseUom,
                        PreviousDate: r.PreviousDate, PreviousRate: X.num(r.PreviousRate), RateUom: apply, NewPrice: 0 };
                });
                if ($id('CbOnlyShowRatedItems').checked) dt = dt.filter(function (r) { return r.PreviousRate !== 0; });   // :803-809
                if (dt.length) { st.rows = dt; renderGrid(); }                               // :810 — otherwise the grid is left as it was
            } else { st.rows = []; X.clearGrid('grd'); $id('lblNavigator').textContent = ''; }   // :819 grd.ClearStructure()
        }).catch(X.fail);
    }

    /* Insert :432 */
    function insert() {
        if (!st.rows.length) { alert('Grid record not found'); return; }
        if (!confirm('Are you sure to Save?')) return;
        SC.postJson(API + '/save', { effectedDate: $id('txtEffectedDate').value, effectedTime: $id('datEffectedTime').value, rows: st.rows }).then(function (res) {
            alert(res.message || 'Save Successfully'); reset();
        }).catch(X.fail);
    }

    /* FormHistory :492 + DataGridHistorySetting :524 (Select column at 0; Id, ItemId, CropYearId hidden) */
    var HCOLS = [
        { key: 'Select', cap: '', chk: 'all' }, { key: 'Id', hidden: true },
        { key: 'EffectiveDate', cap: 'EffectiveDate', date: X.ddMMMyyTime }, { key: 'ItemId', hidden: true }, { key: 'ItemName', cap: 'ItemName' },
        { key: 'CropYearId', hidden: true }, { key: 'CropYear', cap: 'CropYear' }, { key: 'ItemRate', cap: 'ItemRate', fmt: '#,##0.##' },
        { key: 'RateUom', cap: 'RateUom' }, { key: 'EntryDate', cap: 'EntryDate', date: X.ddMMMyyTime }, { key: 'UserName', cap: 'UserName' }, { key: 'IsActive', cap: 'IsActive' }
    ];
    function formHistory() {
        SC.getJson(API + '/history').then(function (list) {
            st.history = (list || []).map(function (r) { r.Select = false; return r; });
            if (st.history.length) X.renderGrid('DataGridHistory', HCOLS, { rows: st.history, filter: true, onCheck: function (i, k, v) { st.history[i].Select = v; } });
            else X.clearGrid('DataGridHistory');
        }).catch(X.fail);
    }
    /* btnPrintHistory_Click :916 */
    function btnPrintHistory() {
        var ids = st.history.filter(function (r) { return r.Select; }).map(function (r) { return r.Id; });
        SC.postJson(API + '/print-rows', { itemCategoryId: X.selVal('CmbItemCategoryHistory'), itemTypeId: X.selVal('CmbItemTypeHistory'),
            ids: ids.length ? ids.join(',') + ',' : '', effectedDate: $id('datEffectedDateHistory').value })
            .then(function (rows) { X.printTable('325-ItemPricingList.rpt', rows); }).catch(X.fail);
    }

    function applyLookups(d) {
        st.categories = d.categories || []; st.itemTypes = d.itemTypes || []; st.items = d.items || [];
        parentCategoryBind(d.parentCategories || []);
        X.bindDdl('cmbItem', st.items, 'Id', 'ItemName');
        itemCategoryByParentIdFill();                                                       // CmbParentCatagory_Leave(null, null) :209
        itemTypeByCategoryIdFill();
    }
    /* frmItemPricingSchedule_Load :189 */
    function load() {
        $id('txtEffectedDate').value = SC.today(); $id('datEffectedTime').value = X.nowTime();
        $id('datEffectedForFilter').value = SC.today(); $id('datPreviousDate').value = SC.today(); $id('datEffectedDateHistory').value = SC.today();
        SC.getJson(API + '/init').then(function (d) {
            st.rights = d.rights || {};
            $id('btnsave').disabled = !st.rights.save; $id('btnUpdate').disabled = !st.rights.update;       // :194-195
            applyLookups(d);
        }).catch(X.fail);
    }
    /* btnrefresh_Click :886 */
    function btnRefresh() { SC.getJson(API + '/refresh').then(applyLookups).catch(X.fail); }

    var tabs;
    document.addEventListener('DOMContentLoaded', function () {
        tabs = X.tabs('tabRoot', function (name) { if (name === 'tabHistory') formHistory(); });     // :704
        $id('btnnew').addEventListener('click', reset);
        $id('btnsave').addEventListener('click', insert);
        $id('btnUpdate').addEventListener('click', insert);
        $id('btnrefresh').addEventListener('click', btnRefresh);
        $id('btnAdd').addEventListener('click', btnAdd);
        $id('btnSearch').addEventListener('click', btnSearch);
        $id('btnPrintHistory').addEventListener('click', btnPrintHistory);
        $id('CmbParentCatagory').addEventListener('change', function () { itemCategoryByParentIdFill(); itemFill(); });   // Leave :723
        $id('cmbItemCategory').addEventListener('change', itemTypeByCategoryIdFill);                                       // Leave :828
        $id('cmbItem').addEventListener('change', cmbItemChanged);
        $id('CmbRateUom').addEventListener('change', rateUomLeave);
        $id('chkApplyDate').addEventListener('change', function () { $id('datEffectedForFilter').disabled = !$id('chkApplyDate').checked; });   // :840
        document.addEventListener('keydown', function (e) {                                                     // :383
            var k = (e.key || '').toLowerCase(), save = $id('btnsave'), upd = $id('btnUpdate');
            if (e.key === 'Enter' && e.target && e.target.tagName !== 'TEXTAREA' && !e.target.closest('.fx-grid-host')) { e.preventDefault(); var f = Array.prototype.filter.call(document.querySelectorAll('input,select,button'), function (x) { return !x.disabled && x.offsetParent !== null; }); var i = f.indexOf(e.target); if (i >= 0 && f[i + 1]) f[i + 1].focus(); return; }
            if (e.ctrlKey && k === 's' && !save.classList.contains('is-hidden') && !save.disabled && tabs.current() === 'tabForm') { e.preventDefault(); insert(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            else if (e.ctrlKey && k === 't') { e.preventDefault(); tabs.select(tabs.current() === 'tabHistory' ? 'tabForm' : 'tabHistory'); }
            else if ((e.ctrlKey && k === 'e') || k === 'escape') { e.preventDefault(); X.closeForm(); }
            else if (e.ctrlKey && k === 'u' && !upd.classList.contains('is-hidden') && !upd.disabled) { e.preventDefault(); insert(); }
        });
        load();
    });
})();
