/* frmGetItemPricingList (Inventory_Reports/frmGetItemPricingList.cs) — ScreenDefinition 803 "Item Pricing List".
   Line refs ":NNN" are that file. API: /inventory/reports/item-pricing-list/api.
   Optional query (RequestedByOtherDocument :105-113): categoryId → cmbCategory preset and the grid filled at once.
     Load :98 / Refresh :363   CategoryFill, TypeFill, EffectedDateFill (each keeps the current selection when still offered)
     btnshow :261              gridHisory → USP_GetItemPricingList; empty → grid cleared
     btnNew :395               Reset; btnRegister :542 → 325_01-ItemPricingList.rpt with dtGrid (raw rows)
     KeyDown :434              Ctrl+E/Esc close, Ctrl+N new, Ctrl+R refresh, Ctrl+S show, Ctrl+Down grid, Ctrl+Up / Ctrl+F5 category, Ctrl+P print, Ctrl+Alt shortcuts */
(function () {
    'use strict';
    var SC = window.StoreCommon, X = window.Inv4a, $id = SC.$id, API = '/inventory/reports/item-pricing-list/api';

    var st = { raw: [], rows: [] };

    /* dtItemPricing :290-302 / grdSetting :324 (RecordNo, ItemId hidden; EffectedDate "dd-MMM-yy hh:mm tt") */
    var COLS = [{ key: 'RecordNo', hidden: true }, { key: 'ItemCategory', cap: 'Item Category' }, { key: 'ItemType', cap: 'Item Type' }, { key: 'EffectedDate', cap: 'Effected Date', date: X.ddMMMyyTime },
        { key: 'ItemId', hidden: true }, { key: 'ItemName', cap: 'Item Name' }, { key: 'RateUom', cap: 'Rate Uom' }, { key: 'ItemPrice', cap: 'Item Price', fmt: '#,##0.##' }, { key: 'ItemRate', cap: 'Item Rate' }];

    function render() { X.renderGrid('DataGridHistory', COLS, { rows: st.rows, filter: true, navId: 'lblNavigator' }); if (!st.rows.length) $id('lblNavigator').textContent = ''; }

    /* gridHisory :273 */
    function gridHistory() {
        return SC.postJson(API + '/show', { itemCategoryId: X.selVal('cmbCategory'), itemTypeId: X.selVal('cmbItemtype'), effectedDate: $id('CmbEffectedDate').value }).then(function (dt) {
            st.raw = dt || [];
            st.rows = st.raw.map(function (r) { return { RecordNo: r.RecordNo, ItemCategory: r.CategoryDescription, ItemType: r.TypeDescription, EffectedDate: r.EffectedDate, ItemId: r.ItemId,
                ItemName: r.ItemName, RateUom: r.RateUom, ItemPrice: r.ItemPrice, ItemRate: r.ItemRate }; });
            render();
        }).catch(X.fail);
    }
    /* CategoryFill :175 / TypeFill :218 / EffectedDateFill :130 — keep the selection when still offered, else blank */
    function fills(d) {
        X.bindDdl('cmbCategory', d.categories || [], 'Id', 'Name', true);
        X.bindDdl('cmbItemtype', d.itemTypes || [], 'Id', 'Name', true);
        var el = $id('CmbEffectedDate'), prevText = X.selText('CmbEffectedDate');
        el.innerHTML = '<option value=""></option>';
        (d.effectedDates || []).forEach(function (r) { el.appendChild(new Option(X.ddMMMyyTime(r.EffectedDate), String(r.EffectedDate).slice(0, 10))); });
        if (prevText) { var hit = Array.prototype.filter.call(el.options, function (o) { return o.textContent === prevText; })[0]; el.value = hit ? hit.value : ''; }
    }
    function refresh() { return SC.getJson(API + '/init').then(fills).catch(X.fail); }                    // toolStripButton1_Click :363
    function reset() { $id('cmbCategory').value = ''; $id('cmbItemtype').value = ''; $id('CmbEffectedDate').value = ''; st.raw = []; st.rows = []; X.clearGrid('DataGridHistory'); $id('lblNavigator').textContent = ''; $id('cmbCategory').focus(); }   // :347
    function print() { if (!st.raw.length) { alert('Record Not Found For Display'); return; } X.printTable('325_01-ItemPricingList.rpt', st.raw); }   // :542
    function shortcutKeys() {                                                                              // MakeShortCutKeys :486
        alert(['Ctrl+S  For show data', 'Ctrl+E  For Close', 'Ctrl+R  For Refresh', 'Ctrl+N  For New', 'Ctrl+P  For Print', 'Ctrl+F5  For Focus on Category', 'Ctrl+alt  To Show ShortCut Category', 'Ctrl+ArrowDown  For Focus On Grid'].join('\n'));
    }
    /* frmGetItemPricingList_Load :98 */
    function load() {
        var q = new URLSearchParams(location.search), cat = X.toInt(q.get('categoryId'));
        SC.getJson(API + '/init').then(function (d) {
            fills(d);
            if (cat) { $id('cmbCategory').value = String(cat); gridHistory(); }                          // RequestedByOtherDocument :109-113
            $id('cmbCategory').focus();
        }).catch(X.fail);
    }

    document.addEventListener('DOMContentLoaded', function () {
        $id('btnshow').addEventListener('click', function () { SC.withBusy($id('btnshow'), gridHistory); });
        $id('btnNew').addEventListener('click', reset);
        $id('toolStripButton1').addEventListener('click', function () { SC.withBusy($id('toolStripButton1'), refresh); });
        $id('btnRegister').addEventListener('click', print);
        $id('toolStripButton2').addEventListener('click', shortcutKeys);
        document.addEventListener('keydown', function (e) {                                                // :434
            var k = (e.key || '').toLowerCase();
            if (e.key === 'Enter' && e.target && !e.target.closest('.fx-grid-host')) { e.preventDefault(); var f = Array.prototype.filter.call(document.querySelectorAll('input,select,button'), function (x) { return !x.disabled && x.offsetParent !== null; }); var i = f.indexOf(e.target); if (i >= 0 && f[i + 1]) f[i + 1].focus(); return; }
            if ((e.ctrlKey && k === 'e') || k === 'escape') { e.preventDefault(); X.closeForm(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); gridHistory(); }
            else if (e.ctrlKey && k === 'arrowdown') { e.preventDefault(); var inp = $id('DataGridHistory').querySelector('input'); if (inp) inp.focus(); }
            else if (e.ctrlKey && (k === 'arrowup' || k === 'f5')) { e.preventDefault(); $id('cmbCategory').focus(); }
            else if (e.ctrlKey && k === 'p') { e.preventDefault(); print(); }
            else if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcutKeys(); }
        });
        load();
    });
})();
