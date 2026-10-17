/* frmItemPricingSchedule_New (BookingOffice/frmItemPricingSchedule_New.cs) — ScreenDefinition 796 "Pricing Schedule For Booking".
   Line refs ":NNN" are that file. API: /inventory/pricing-schedule-for-booking/api.
     frmItemPricingSchedule_Load :220   ParentCategoryBind, CropYearBind, ItemCategoryAndTypeFill, ItemFill, CmbParentCatagory_Leave, HistoryComboFill
     btnAdd_Click :745                  FormValidation (Item, Crop Year, Price, RateUom) → same item + same crop year check → dt.Rows.Add
     btnSearch_Click :956               GetItemWithRatesForItemPricingSchedule → dt (CropYearId from the procedure, 0 when unrated)
     GrdCropYearBind :839               CropYearId column is a drop-down ("Crop Year") inside the grid
     CmbCropYearForUpdateOfGridRows_Leave :1225   fills CropYearId of every row that has 0
     Insert :558                        "Grid record not found" → confirm → " Crop Year Field Required In Grid " / RateUom check → "Save Successfully" → Reset
     History tab: btnShow → FormHistory :624; btnNewHistory :1206; btnRefreshHistory :1194; btnPrint325 :1152
     tabControl1_SelectedIndexChanged :886 only moves focus (no auto history load on this form) */
(function () {
    'use strict';
    var SC = window.StoreCommon, X = window.Inv4a, $id = SC.$id, API = '/inventory/pricing-schedule-for-booking/api';

    var st = { rights: {}, categories: [], itemTypes: [], items: [], cropYears: [], uoms: [], rows: [], history: [], recId: 0 };

    function cols() {
        return [
            { key: 'Delete', cap: 'X', btn: 'X' },
            { key: 'ItemId', hidden: true }, { key: 'RateUomId', hidden: true }, { key: 'BaseUomId', hidden: true },
            { key: 'ItemCategory', cap: 'ItemCategory', width: 150 },
            { key: 'ItemName', cap: 'ItemName', width: 350 },
            { key: 'CropYearId', cap: 'Crop Year', edit: { options: st.cropYears } },
            { key: 'BaseUom', hidden: true },
            { key: 'PreviousDate', cap: 'PreviousDate', date: X.ddMMMyyyy },
            { key: 'PreviousRate', cap: 'PreviousRate', fmt: '#,#.##' },
            { key: 'RateUom', cap: 'RateUom', cls: 'n' },
            { key: 'NewPrice', cap: 'NewPrice', edit: 'num' }
        ];
    }
    function renderGrid() {
        X.renderGrid('grd', cols(), { rows: st.rows, group: 'ItemCategory', groupCaption: 'ItemCategory', filter: true, navId: 'lblNavigator',
            onEdit: function (i, key, v) { if (key === 'NewPrice') st.rows[i].NewPrice = X.num(v); if (key === 'CropYearId') st.rows[i].CropYearId = X.toInt(v); },
            onButton: function (i) { grdDelete(i); } });
    }
    function grdDelete(i) {                                                                     // :1045
        var save = $id('btnsave');
        if (!save.classList.contains('is-hidden') && !save.disabled) { st.rows.splice(i, 1); renderGrid(); }
        else alert('You Can not Delete Detail Record In Update Mode... ');
    }

    function parentCategoryBind(list) {
        X.bindDdl('CmbParentCatagory', list);
        var el = $id('CmbParentCatagory');
        if (list && list.length) el.value = String(list[Math.min(1, list.length - 1)].Id);      // Rows[1].Activate() :329
    }
    function itemCategoryByParentIdFill() {                                                     // :350 (no history combo on this form)
        var pid = X.selVal('CmbParentCatagory');
        var dt = st.categories.filter(function (r) { return r.InventoryParentCategoriesId === pid; });
        X.bindDdl('cmbItemCategory', dt.length ? dt : st.categories, 'Id', 'CategoryDescription');
    }
    function itemTypeByCategoryIdFill() {                                                       // :375
        var cid = X.selVal('cmbItemCategory');
        var dt = st.itemTypes.filter(function (r) { return r.ItemCategoryId === cid; });
        X.bindDdl('cmbItemType', dt.length ? dt : st.itemTypes, 'Id', 'TypeDescription');
    }
    function itemFill() {                                                                       // :251
        var pid = X.selVal('CmbParentCatagory');
        if (!pid) { st.items = []; X.bindDdl('cmbItem', []); return Promise.resolve(); }
        return SC.getJson(API + '/items' + SC.qs({ parentId: pid })).then(function (list) { st.items = list || []; X.bindDdl('cmbItem', st.items, 'Id', 'ItemName'); }).catch(X.fail);
    }
    function itemRow() { var id = X.selVal('cmbItem'); return st.items.filter(function (r) { return r.Id === id; })[0] || null; }
    function cmbItemChanged() {                                                                 // :703
        var id = X.selVal('cmbItem');
        if (!id) { st.uoms = []; X.bindDdl('CmbRateUom', []); return; }
        SC.getJson(API + '/uoms' + SC.qs({ itemId: id })).then(function (list) { st.uoms = list || []; X.bindDdl('CmbRateUom', st.uoms, 'Id', 'UOMCode'); }).catch(X.fail);
    }
    function uomRow() { var id = X.selVal('CmbRateUom'); return st.uoms.filter(function (r) { return r.Id === id; })[0] || null; }
    function rateUomLeave() {                                                                   // :922
        SC.getJson(API + '/last-rate' + SC.qs({ itemId: X.selVal('cmbItem'), rateUomId: X.selVal('CmbRateUom') })).then(function (d) {
            $id('txtLastRate').value = d.ItemRate; $id('datPreviousDate').value = d.PreviousDate || SC.today();
        }).catch(X.fail);
    }

    function formValidation() {                                                                 // :857
        if (!X.hasActiveRow('cmbItem')) { $id('cmbItem').focus(); alert('Item Field Required'); return false; }
        if (!X.hasActiveRow('cmbCropYear')) { $id('cmbCropYear').focus(); alert('Crop Year Field Required'); return false; }
        if (X.num($id('txtNewPrice').value) === 0) { $id('txtNewPrice').focus(); alert('Price Field Required'); return false; }
        if (!X.hasActiveRow('CmbRateUom')) { $id('CmbRateUom').focus(); alert('RateUom Field Required'); return false; }
        return true;
    }
    function btnAdd() {                                                                         // :745
        try {
            if (!formValidation()) return;
            var itemId = X.selVal('cmbItem'), cropId = X.selVal('cmbCropYear');
            if (st.rows.some(function (r) { return r.ItemId === itemId && X.toInt(r.CropYearId) === cropId; })) { $id('cmbItem').focus(); throw new Error('You cannot add same item With Same CropYear in grid'); }
            var ir = itemRow(), ur = uomRow();
            st.rows.push({ ItemId: itemId, RateUomId: X.selVal('CmbRateUom'), BaseUomId: 0, ItemCategory: ir ? ir.ItemCategory : '', ItemName: X.selText('cmbItem'), CropYearId: cropId, BaseUom: 0,
                PreviousDate: $id('datPreviousDate').value, PreviousRate: X.num($id('txtLastRate').value), RateUom: ur ? X.toInt(ur.Equivalent) : 0, NewPrice: X.num($id('txtNewPrice').value) });
            renderGrid();
            resetDetail();
        } catch (e) { X.fail(e); }
    }
    function resetDetail() { $id('cmbItem').value = ''; $id('CmbRateUom').value = ''; $id('txtNewPrice').value = ''; $id('cmbItem').focus(); }   // :774
    function reset() {                                                                          // :724
        st.rows = []; X.clearGrid('grd'); $id('lblNavigator').textContent = '';
        $id('cmbItem').value = ''; $id('txtNewPrice').value = ''; $id('txtLastRate').value = '';
        $id('btnsave').classList.remove('is-hidden'); $id('btnUpdate').classList.add('is-hidden');
        st.recId = 0; $id('btnAdd').disabled = false; $id('cmbItem').focus();
    }

    function btnSearch() {                                                                      // :956
        SC.postJson(API + '/search', { parentCategoryId: X.selVal('CmbParentCatagory'), itemCategoryId: X.selVal('cmbItemCategory'), itemTypeId: X.selVal('cmbItemType'),
            applyDate: $id('chkApplyDate').checked, effectedDate: $id('datEffectedForFilter').value }).then(function (data) {
            if (data && data.length) {
                var apply = X.num($id('txtApplyRateUom').value.trim());
                if (apply === 0) apply = 40;
                var dt = data.map(function (r) {
                    if (X.num(r.RateUom) !== 0 && apply === 0) apply = X.num(r.RateUom);
                    return { ItemId: r.ItemId, RateUomId: r.RateUomId, BaseUomId: r.BaseUomId, ItemCategory: r.ItemCategory, ItemName: r.ItemName, CropYearId: r.CropYearId, BaseUom: r.BaseUom,
                        PreviousDate: r.PreviousDate, PreviousRate: X.num(r.PreviousRate), RateUom: apply, NewPrice: 0 };
                });
                if ($id('CbOnlyShowRatedItems').checked) dt = dt.filter(function (r) { return r.PreviousRate !== 0; });
                if (dt.length) { st.rows = dt; renderGrid(); }
            } else { st.rows = []; X.clearGrid('grd'); $id('lblNavigator').textContent = ''; }
        }).catch(X.fail);
    }
    function cropYearForAllRows() {                                                             // :1225
        var v = X.selVal('CmbCropYearForUpdateOfGridRows');
        if (v <= 0) return;
        st.rows.forEach(function (r) { if (X.toInt(r.CropYearId) === 0) r.CropYearId = v; });
        renderGrid();
    }

    function insert() {                                                                         // :558
        if (!st.rows.length) { alert('Grid record not found'); return; }
        if (!confirm('Are you sure to Save?')) return;
        SC.postJson(API + '/save', { effectedDate: $id('txtEffectedDate').value, effectedTime: $id('datEffectedTime').value, rows: st.rows }).then(function (res) {
            alert(res.message || 'Save Successfully'); reset();
        }).catch(X.fail);
    }

    /* ---------------------------------------------------------------- History tab */
    var HCOLS = [
        { key: 'Select', cap: '', chk: 'all' }, { key: 'Id', hidden: true },
        { key: 'EffectedDate', cap: 'EffectedDate', date: X.ddMMMyyTime }, { key: 'ItemId', hidden: true }, { key: 'ItemName', cap: 'ItemName' },
        { key: 'CropYearId', hidden: true }, { key: 'CropYear', cap: 'CropYear' }, { key: 'ItemRate', cap: 'ItemRate', fmt: '#,##0.##' },
        { key: 'RateUom', cap: 'RateUom' }, { key: 'EntryDate', cap: 'EntryDate', date: X.ddMMMyyTime }, { key: 'UserName', cap: 'UserName' }
    ];
    function historyBody() {
        return { parentCategoryId: X.selVal('CmbParentCategoryHistory'), itemCategoryId: X.selVal('CmbItemCategoryHistory'), itemTypeId: X.selVal('CmbItemTypeHistory'),
            itemId: X.selVal('CmbItemHistory'), cropYearId: X.selVal('CmbCropYearHistory'), effectedDate: $id('cmbEffectedDate').value };
    }
    function formHistory() {                                                                    // :624
        SC.postJson(API + '/history', historyBody()).then(function (list) {
            st.history = (list || []).map(function (r) { r.Select = false; return r; });
            if (st.history.length) X.renderGrid('DataGridHistory', HCOLS, { rows: st.history, filter: true, onCheck: function (i, k, v) { st.history[i].Select = v; } });
            else X.clearGrid('DataGridHistory');
        }).catch(X.fail);
    }
    function historyComboFill(h) {                                                              // :399 — BindDDL(ZeroIndex:false) six times
        X.bindDdl('CmbParentCategoryHistory', h.ParentCategories, 'Id', 'Name', true);
        X.bindDdl('CmbItemCategoryHistory', h.ItemCategories, 'Id', 'Name', true);
        X.bindDdl('CmbItemTypeHistory', h.ItemTypes, 'Id', 'Name', true);
        X.bindDdl('CmbItemHistory', h.Item, 'Id', 'Name', true);
        X.bindDdl('CmbCropYearHistory', h.CropYear, 'Id', 'Name', true);
        var el = $id('cmbEffectedDate'), prev = el.value;
        el.innerHTML = '<option value=""></option>';
        (h.EffectedDate || []).forEach(function (r) { el.appendChild(new Option(X.ddMMMyy(r.Name), r.Name)); });   // value = ISO day, text dd-MMM-yy (:456)
        el.value = prev; if (el.value !== prev) el.value = '';
    }
    function btnNewHistory() {                                                                  // :1206
        ['CmbParentCategoryHistory', 'CmbItemCategoryHistory', 'CmbItemTypeHistory', 'CmbItemHistory', 'CmbCropYearHistory', 'cmbEffectedDate'].forEach(function (id) { $id(id).value = ''; });
        st.history = []; X.clearGrid('DataGridHistory'); $id('CmbParentCategoryHistory').focus();
    }
    function btnRefreshHistory() { SC.getJson(API + '/history-lookups').then(historyComboFill).catch(X.fail); }    // :1194
    function btnPrint325() {                                                                    // :1152
        var ids = st.history.filter(function (r) { return r.Select; }).map(function (r) { return r.Id; });
        SC.postJson(API + '/print-rows', { itemCategoryId: X.selVal('CmbItemCategoryHistory'), itemTypeId: X.selVal('CmbItemTypeHistory'),
            ids: ids.length ? ids.join(',') : '', effectedDate: $id('cmbEffectedDate').value })
            .then(function (rows) { X.printTable('325-ItemPricingList.rpt', rows); }).catch(X.fail);
    }

    function applyLookups(d) {
        st.categories = d.categories || []; st.itemTypes = d.itemTypes || []; st.items = d.items || []; st.cropYears = d.cropYears || [];
        parentCategoryBind(d.parentCategories || []);
        X.bindDdl('cmbCropYear', st.cropYears, 'Id', 'Name', true);                             // CropYearBind :279 (BindDDLNew, caption "Crop Year")
        X.bindDdl('CmbCropYearForUpdateOfGridRows', st.cropYears, 'Id', 'Name', true);
        X.bindDdl('cmbItem', st.items, 'Id', 'ItemName');
        itemCategoryByParentIdFill();
        itemTypeByCategoryIdFill();
        if (st.rows.length) renderGrid();                                                       // GrdCropYearBind :1078
    }
    function load() {                                                                           // :220
        $id('txtEffectedDate').value = SC.today(); $id('datEffectedTime').value = X.nowTime();
        $id('datEffectedForFilter').value = SC.today(); $id('datPreviousDate').value = SC.today();
        SC.getJson(API + '/init').then(function (d) {
            st.rights = d.rights || {};
            $id('btnsave').disabled = !st.rights.save; $id('btnUpdate').disabled = !st.rights.update;       // :225-226
            applyLookups(d);
            historyComboFill(d.history || {});
        }).catch(X.fail);
    }

    var tabs;
    document.addEventListener('DOMContentLoaded', function () {
        tabs = X.tabs('tabRoot', function (name) { if (name === 'tabHistory') $id('CmbParentCategoryHistory').focus(); else $id('txtEffectedDate').focus(); });   // :886
        $id('btnnew').addEventListener('click', reset);
        $id('btnsave').addEventListener('click', insert);
        $id('btnUpdate').addEventListener('click', insert);
        $id('btnrefresh').addEventListener('click', function () { SC.getJson(API + '/refresh').then(applyLookups).catch(X.fail); });   // :1072
        $id('btnAdd').addEventListener('click', btnAdd);
        $id('btnSearch').addEventListener('click', btnSearch);
        $id('btnShow').addEventListener('click', formHistory);
        $id('btnNewHistory').addEventListener('click', btnNewHistory);
        $id('btnRefreshHistory').addEventListener('click', btnRefreshHistory);
        $id('btnPrint325').addEventListener('click', btnPrint325);
        $id('CmbParentCatagory').addEventListener('change', function () { itemCategoryByParentIdFill(); itemFill(); });   // :909
        $id('cmbItemCategory').addEventListener('change', itemTypeByCategoryIdFill);                                       // :1014
        $id('cmbItem').addEventListener('change', cmbItemChanged);
        $id('CmbRateUom').addEventListener('change', rateUomLeave);
        $id('CmbCropYearForUpdateOfGridRows').addEventListener('change', cropYearForAllRows);
        $id('chkApplyDate').addEventListener('change', function () { $id('datEffectedForFilter').disabled = !$id('chkApplyDate').checked; });   // :1026
        document.addEventListener('keydown', function (e) {                                                     // :509
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
