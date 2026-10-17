/* frmItemDiscountingScheduleForBooking (WholeSale/frmItemDiscountingScheduleForBooking.cs) — ScreenDefinition 99 "Item Discounting Schedule".
   Line refs ":NNN" are that file. API: /inventory/item-discounting-schedule/api. priceTypeId 6 (" ( Whole Sale )").
     frmItemPricingSchedule_Load :167   policy cards ("Regular Discount" only), TargetTypeFill, DiscountTypeFill, DiscountByFill, ItemFill,
                                        ItemCategoryFill, ItemTypeFill; both item group boxes hidden; ScheduleTab removed until a card is clicked
     UserControl_Click :235             lblPolicyType = "<policy> ( Whole Sale )", ScheduleTab added and selected
     CmbDiscountBy_TextChanged :865     1 → One By One box, 2 → Filter box
     btnAdd_Click :783                  "Item Field Is Required" / "Discount Percent Field Is Required" / "Same Item has Already Present In Grid!"
     btnSearch_Click :727               "Item Category Or Item Type Field Is Required" / "Discount Percent Field Is Required" → GetFilteredItems :525
     Insert :573                        FormValiadation → confirm → "At Least One Item Required" → "Item Required" → "Save Successfully" → Reset
     tab history :821                   FormHistory; DataGridHistory double-click → ReadById (empty on the desktop)
     KeyDown :476                       Ctrl+S (tab index 0 only — the Policy tab, so Save by key never fires), Ctrl+N, Ctrl+T, Ctrl+E / Esc, Ctrl+U */
(function () {
    'use strict';
    var SC = window.StoreCommon, X = window.Inv4a, $id = SC.$id, API = '/inventory/item-discounting-schedule/api';

    var st = { policyId: 0, priceTypeId: 6, items: [], rows: [], recId: 0 };

    /* datagrid — dtGrid :217-219 (ItemId hidden, ItemName 300 read-only, ItemPercent editable) + "X" delete column while Save is enabled and visible */
    function cols() {
        var c = [{ key: 'ItemId', hidden: true }, { key: 'ItemName', cap: 'ItemName', width: 300 }, { key: 'ItemPercent', cap: 'ItemPercent', edit: 'num' }];
        var save = $id('btnsave');
        if (!save.disabled && !save.classList.contains('is-hidden')) c.push({ key: 'Delete', cap: 'X', btn: 'X' });
        return c;
    }
    function renderGrid() {
        X.renderGrid('datagrid', cols(), { rows: st.rows, filter: true,
            onEdit: function (i, k, v) { if (k === 'ItemPercent') st.rows[i].ItemPercent = v; },
            onButton: function (i) { st.rows.splice(i, 1); renderGrid(); } });            // datagrid_ColumnButtonClick :886
    }

    /* UserControl_Click :235 */
    function cardClick(card) {
        st.policyId = card.Id;
        $id('lblPolicyType').textContent = card.Name + (st.priceTypeId === 6 ? ' ( Whole Sale )' : ' ( Retail )');
        $id('tabFormHeader').classList.remove('is-hidden');
        tabs.select('tabForm');
    }
    /* CmbDiscountBy_TextChanged :865 */
    function discountByChanged() {
        var v = X.selVal('CmbDiscountBy');
        if (v === 1) { $id('OneByOneItemGroupBox').classList.remove('is-hidden'); $id('FilterItemGroupBox').classList.add('is-hidden'); }
        else if (v === 2) { $id('OneByOneItemGroupBox').classList.add('is-hidden'); $id('FilterItemGroupBox').classList.remove('is-hidden'); }
    }
    /* GetFilteredItems :525 */
    function getFilteredItems() {
        st.rows = []; X.clearGrid('datagrid');
        var cat = X.selVal('cmbItemCategory'), typ = X.selVal('cmbItemType');
        if (cat <= 0 && typ <= 0) return;
        SC.getJson(API + '/items' + SC.qs({ itemCategoryId: cat, itemTypeId: typ })).then(function (dt) {
            if (dt && dt.length) { st.rows = dt.map(function (r) { return { ItemId: r.Id, ItemName: r.Name, ItemPercent: $id('txtDiscountValue').value }; }); renderGrid(); }
            else { st.rows = []; X.clearGrid('datagrid'); }
        }).catch(X.fail);
    }
    /* btnSearch_Click :727 */
    function btnSearch() {
        if (X.selText('cmbItemCategory') === '' && X.selText('cmbItemType') === '') { alert('Item Category Or Item Type Field Is Required'); $id('cmbItemCategory').focus(); return; }
        var dv = $id('txtDiscountValue').value;
        if (dv === '' || dv === '0') { alert('Discount Percent Field Is Required'); $id('txtDiscountValue').focus(); return; }
        getFilteredItems();
    }
    /* btnAdd_Click :783 */
    function btnAdd() {
        try {
            if (X.selText('cmbItem') === '') { alert('Item Field Is Required'); $id('cmbItem').focus(); return; }
            var dv = $id('txtDiscountValue').value;
            if (dv === '' || dv === '0') { alert('Discount Percent Field Is Required'); $id('txtDiscountValue').focus(); return; }
            var id = X.selVal('cmbItem');
            if (st.rows.some(function (r) { return X.toInt(r.ItemId) === id; })) throw new Error('Same Item has Already Present In Grid!');
            st.rows.push({ ItemId: id, ItemName: X.selText('cmbItem'), ItemPercent: dv });
            renderGrid();
        } catch (e) { X.fail(e); }
    }
    /* Reset :752 */
    function reset() {
        $id('txtDiscountName').value = ''; $id('txtDiscountDiscription').value = '';
        $id('txtFromDate').value = SC.today(); $id('txtToDate').value = SC.today();
        $id('cmbDiscountType').value = ''; $id('CmbDiscountBy').value = ''; $id('txtDiscountValue').value = ''; $id('cmbTargetTaype').value = '';
        $id('cmbItem').value = ''; $id('cmbItemCategory').value = ''; $id('cmbItemType').value = '';
        getFilteredItems();
        $id('btnsave').classList.remove('is-hidden'); $id('btnUpdate').classList.add('is-hidden');
        st.recId = 0; $id('txtDiscountName').focus();
    }
    function body() {
        return { recId: st.recId, policyId: st.policyId, discountName: $id('txtDiscountName').value, discountDescription: $id('txtDiscountDiscription').value,
            fromDate: $id('txtFromDate').value, toDate: $id('txtToDate').value, discountTypeId: X.selVal('cmbDiscountType'), targetTypeId: X.selVal('cmbTargetTaype'),
            discountById: X.selVal('CmbDiscountBy'), rows: st.rows };
    }
    /* Insert :573 — FormValiadation (server, desktop order) → confirm → "At Least One Item Required" → save */
    function insert() {
        var b = body();
        SC.postJson(API + '/validate', b).then(function () {
            if (!confirm(st.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return null;
            if (!st.rows.length) { alert('At Least One Item Required'); return null; }
            return SC.postJson(API + '/save', b).then(function (res) { if (res.message) alert(res.message); reset(); });
        }).catch(X.fail);
    }

    /* FormHistory :658 / DataGridHistorySetting :701 */
    var HCOLS = [{ key: 'Id', hidden: true }, { key: 'DocNo', cap: 'DocNo', width: 60 }, { key: 'FromDate', cap: 'FromDate', date: X.ddMMMyyyy, width: 90 }, { key: 'ToDate', cap: 'ToDate', date: X.ddMMMyyyy, width: 90 },
        { key: 'DiscountName', cap: 'DiscountName', width: 150 }, { key: 'DiscountType', cap: 'DiscountType', width: 150 }, { key: 'ItemName', cap: 'ItemName', width: 150 },
        { key: 'Percent', cap: 'Percent', cls: 'n', width: 60 }, { key: 'DiscountDescription', cap: 'DiscountDescription', width: 200 }, { key: 'Status', cap: 'Status' }];
    function formHistory() {
        SC.getJson(API + '/history').then(function (list) {
            if (list && list.length) X.renderGrid('DataGridHistory', HCOLS, { rows: list, filter: true, onDouble: function () { /* ReadById :934 is empty on the desktop */ } });
        }).catch(X.fail);
    }

    /* frmItemPricingSchedule_Load :167 */
    function load() {
        $id('txtFromDate').value = SC.today(); $id('txtToDate').value = SC.today();
        SC.getJson(API + '/init').then(function (d) {
            st.priceTypeId = d.priceTypeId || 6;
            if (!d.policyTypesFound) { alert('No Policy Types Define'); return; }
            var flow = $id('FlowLayout');
            flow.innerHTML = (d.policyTypes || []).map(function (p, i) {
                return '<div class="fx-card" data-i="' + i + '"><div class="cnt">' + (i + 1) + '</div><div class="ttl">' + SC.esc(p.Name) + '</div></div>';
            }).join('');
            flow.querySelectorAll('.fx-card').forEach(function (c) { c.addEventListener('click', function () { cardClick(d.policyTypes[+c.getAttribute('data-i')]); }); });
            X.bindDdl('cmbTargetTaype', d.targetTypes || []); if ((d.targetTypes || []).length) $id('cmbTargetTaype').selectedIndex = 1;     // Rows[0].Activate :344
            X.bindDdl('cmbDiscountType', d.discountTypes || []); if ((d.discountTypes || []).length) $id('cmbDiscountType').selectedIndex = 1; // :315
            X.bindDdl('CmbDiscountBy', [{ Id: 1, Name: 'One By One Item' }, { Id: 2, Name: 'Filter Items' }]);                            // DiscountByFill :276
            st.items = d.items || []; X.bindDdl('cmbItem', st.items);
            X.bindDdl('cmbItemCategory', d.categories || []);
            X.bindDdl('cmbItemType', d.itemTypes || []);
            $id('OneByOneItemGroupBox').classList.add('is-hidden'); $id('FilterItemGroupBox').classList.add('is-hidden');
            X.renderGrid('datagrid', cols(), { rows: [] });
        }).catch(X.fail);
    }

    var tabs;
    document.addEventListener('DOMContentLoaded', function () {
        tabs = X.tabs('tabRoot', function (name) { if (name === 'tabHistory') formHistory(); });    // :821 (SelectedIndex 1 = history while the Form tab is removed)
        $id('btnnew').addEventListener('click', reset);
        $id('btnsave').addEventListener('click', insert);
        $id('btnUpdate').addEventListener('click', insert);
        $id('btnAdd').addEventListener('click', btnAdd);
        $id('btnSearch').addEventListener('click', btnSearch);
        $id('CmbDiscountBy').addEventListener('change', discountByChanged);
        document.addEventListener('keydown', function (e) {                                       // :476
            var k = (e.key || '').toLowerCase(), save = $id('btnsave'), upd = $id('btnUpdate');
            if (e.key === 'Enter' && e.target && e.target.tagName !== 'TEXTAREA' && !e.target.closest('.fx-grid-host')) { e.preventDefault(); var f = Array.prototype.filter.call(document.querySelectorAll('input,select,button'), function (x) { return !x.disabled && x.offsetParent !== null; }); var i = f.indexOf(e.target); if (i >= 0 && f[i + 1]) f[i + 1].focus(); return; }
            if (e.ctrlKey && k === 's' && !save.classList.contains('is-hidden') && !save.disabled && tabs.current() === 'tabPolicy') { e.preventDefault(); insert(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            else if (e.ctrlKey && k === 't') { e.preventDefault(); tabs.select(tabs.current() === 'tabHistory' ? 'tabPolicy' : 'tabHistory'); }
            else if ((e.ctrlKey && k === 'e') || k === 'escape') { e.preventDefault(); X.closeForm(); }
            else if (e.ctrlKey && k === 'u' && !upd.classList.contains('is-hidden') && !upd.disabled) { e.preventDefault(); insert(); }
        });
        load();
    });
})();
