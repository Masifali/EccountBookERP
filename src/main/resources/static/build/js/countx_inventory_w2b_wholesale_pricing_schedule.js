/* WholeSale/frmItemPricingSchedule.cs — ScreenDefinition 107 "Whole Sale Pricing" (frmPosWhSalePricing). Line refs ":NNN" are that file.
   API: /inventory/wholesale-pricing-schedule/api.
     frmItemPricingSchedule_Load :122   rights (:127-129), priceTypeId 6 (:130-137), ItemFill, ItemTypeFill, ItemCategoryFill
     btnAdd_Click :667                  GetPreviousPricesByItemId(cmbItem) → grid replaced by that item's latest price row
     btnSearch_Click :640               GetFilteredItems :323 — only when a category or a type is chosen
     Insert :355                        "Grid record not found" → "Are you sure to Save?/Update?" → Save → "Save/Update Successfully" → Reset
     tab history :687                   FormHistory; "Edit" button :702 → GetByItemId :498 (one item at a time)
     KeyDown :166                       Ctrl+S save (Form tab), Ctrl+N new, Ctrl+T tab toggle, Ctrl+E / Esc close, Ctrl+U update */
(function () {
    'use strict';
    var SC = window.StoreCommon, X = window.Inv4a, $id = SC.$id, API = '/inventory/wholesale-pricing-schedule/api';
    var st = { rights: {}, rows: [], history: [], recId: 0 };

    /* datagrid (dtGrid :138-145; dataGridSetting :159) — ItemId, ScheduleDesc, EffectedDate, BranchId hidden; ItemName 400; ItemPrice read-only "#,#.##"; NewPrice editable; Delete */
    var COLS = [
        { key: 'ItemId', hidden: true }, { key: 'ItemName', cap: 'ItemName', width: 400 },
        { key: 'ItemPrice', cap: 'ItemPrice', fmt: '#,#.##' },
        { key: 'NewPrice', cap: 'NewPrice', edit: 'num' },
        { key: 'Delete', cap: 'Delete', btn: 'X' }
    ];
    function renderGrid() {
        X.renderGrid('datagrid', COLS, { rows: st.rows, filter: true, navId: 'lblNavigator',
            onEdit: function (i, key, v) { if (key === 'NewPrice') st.rows[i].NewPrice = X.num(v); },
            onButton: function (i) { gridDelete(i); } });
    }
    /* datagrid_ColumnButtonClick :735 — only while Save is visible and enabled */
    function gridDelete(i) {
        var save = $id('btnsave');
        if (!save.classList.contains('is-hidden') && !save.disabled) { st.rows.splice(i, 1); renderGrid(); }
        else alert('You Can not Delete Detail Record In Update Mode... ');
    }
    function setRows(list) {
        st.rows = list || [];
        if (st.rows.length) renderGrid(); else { X.clearGrid('datagrid'); $id('lblNavigator').textContent = ''; }   // :306-309 DataSource = null
    }

    /* GetPreviousPricesByItemId :271 */
    function previous(itemIds, cat, typ) {
        SC.postJson(API + '/previous-prices', { itemIds: itemIds, itemCategoryId: cat, itemTypeId: typ }).then(setRows).catch(X.fail);
    }
    /* btnAdd_Click :667 */
    function btnAdd() { var ids = $id('cmbItem').value; if (ids !== '') previous(ids, 0, 0); }
    /* btnSearch_Click :640 → GetFilteredItems :323 */
    function btnSearch() {
        var c = X.selVal('cmbItemCategory'), t = X.selVal('cmbItemType');
        if (c > 0 || t > 0) previous('', c, t);
    }

    /* Reset :586 */
    function reset() {
        X.bindDdl('cmbItem', st.items || [], 'ItemId', 'ItemName');
        $id('cmbItemCategory').value = ''; $id('cmbItemType').value = ''; $id('txtDescription').value = '';
        $id('txtEffectedDate').value = SC.today();
        st.rows = []; X.clearGrid('datagrid'); $id('lblNavigator').textContent = '';
        $id('btnsave').classList.remove('is-hidden'); $id('btnUpdate').classList.add('is-hidden');
        st.recId = 0; $id('btnAdd').disabled = false; $id('btnSearch').disabled = false; $id('cmbItem').focus();
    }

    /* Insert :355 */
    function insert() {
        if (!st.rows.length) { alert('Grid record not found'); return; }
        if (!confirm(st.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        SC.postJson(API + '/save', { recId: st.recId, scheduleDesc: $id('txtDescription').value, effectedDate: $id('txtEffectedDate').value,
            effectedTime: $id('datEffectedTime').value, rows: st.rows }).then(function (res) {
            alert(res.message || (st.recId > 0 ? 'Update Successfully' : 'Save Successfully')); reset();
        }).catch(X.fail);
    }

    /* FormHistory :439 + DataGridHistorySetting :476 (Edit column; Id, ItemId hidden) */
    var HCOLS = [
        { key: 'Edit', cap: 'Edit', btn: 'Edit' }, { key: 'Id', hidden: true }, { key: 'ItemId', hidden: true },
        { key: 'ItemName', cap: 'ItemName' }, { key: 'EffectedDate', cap: 'EffectedDate', date: X.ddMMMyyTime },
        { key: 'ItemPrice', cap: 'ItemPrice', fmt: '#,##0.##' }, { key: 'EntryUserName', cap: 'EntryUserName' }
    ];
    function formHistory() {
        SC.getJson(API + '/history').then(function (list) {
            st.history = list || [];
            if (st.history.length) X.renderGrid('DataGridHistory', HCOLS, { rows: st.history, filter: true,
                onButton: function (i) { getByItemId(st.history[i].ItemId); } });
            else X.clearGrid('DataGridHistory');
        }).catch(X.fail);
    }
    /* GetByItemId :498 */
    function getByItemId(itemId) {
        if (st.rows.length > 0) { alert('You cannot update price of more than 1 items at time!'); return; }
        SC.getJson(API + '/edit' + SC.qs({ itemId: itemId })).then(function (d) {
            if (!d) return;
            st.recId = d.Id; $id('txtDescription').value = d.ScheduleDesc || '';
            var m = /^(\d{4}-\d{2}-\d{2})[ T](\d{2}:\d{2}(?::\d{2})?)/.exec(d.EffectedDate || '');
            $id('txtEffectedDate').value = m ? m[1] : SC.today(); if (m) $id('datEffectedTime').value = m[2].length === 5 ? m[2] + ':00' : m[2];
            $id('cmbItem').value = String(d.ItemId);
            st.rows = [{ ItemId: d.ItemId, ItemName: d.ItemName, BranchId: 0, ItemPrice: d.ItemPrice, NewPrice: 0 }];
            renderGrid();
            $id('btnUpdate').classList.remove('is-hidden'); $id('btnsave').classList.add('is-hidden');
            $id('btnAdd').disabled = true; $id('btnSearch').disabled = true;
            tabs.select('tabForm', true);
        }).catch(X.fail);
    }

    var tabs;
    document.addEventListener('DOMContentLoaded', function () {
        tabs = X.tabs('tabRoot', function (name) { if (name === 'tabHistory') formHistory(); });        // :687
        $id('btnnew').addEventListener('click', reset);
        $id('btnsave').addEventListener('click', insert);
        $id('btnUpdate').addEventListener('click', insert);
        $id('btnAdd').addEventListener('click', btnAdd);
        $id('btnSearch').addEventListener('click', btnSearch);
        document.addEventListener('keydown', function (e) {                                                // :166
            var k = (e.key || '').toLowerCase(), save = $id('btnsave'), upd = $id('btnUpdate');
            if (e.key === 'Enter' && e.target && e.target.tagName !== 'TEXTAREA' && !e.target.closest('.fx-grid-host')) { e.preventDefault(); var f = Array.prototype.filter.call(document.querySelectorAll('input,select,button'), function (x) { return !x.disabled && x.offsetParent !== null; }); var i = f.indexOf(e.target); if (i >= 0 && f[i + 1]) f[i + 1].focus(); return; }
            if (e.ctrlKey && k === 's' && !save.classList.contains('is-hidden') && !save.disabled && tabs.current() === 'tabForm') { e.preventDefault(); insert(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            else if (e.ctrlKey && k === 't') { e.preventDefault(); tabs.select(tabs.current() === 'tabHistory' ? 'tabForm' : 'tabHistory'); }
            else if ((e.ctrlKey && k === 'e') || k === 'escape') { e.preventDefault(); X.closeForm(); }
            else if (e.ctrlKey && k === 'u' && !upd.classList.contains('is-hidden') && !upd.disabled) { e.preventDefault(); insert(); }
        });
        /* frmItemPricingSchedule_Load :122 */
        $id('txtEffectedDate').value = SC.today(); $id('datEffectedTime').value = X.nowTime();
        SC.getJson(API + '/init').then(function (d) {
            st.rights = d.rights || {};
            $id('btnsave').disabled = !st.rights.save; $id('btnUpdate').disabled = !st.rights.update;       // :128-129
            st.items = d.items || [];
            X.bindDdl('cmbItem', st.items, 'ItemId', 'ItemName');
            X.bindDdl('cmbItemType', d.itemTypes || [], 'Id', 'Name');
            X.bindDdl('cmbItemCategory', d.categories || [], 'Id', 'Name');
        }).catch(X.fail);
    });
})();
