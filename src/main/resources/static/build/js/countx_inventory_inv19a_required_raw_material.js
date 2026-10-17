/* RequiredRawMaterialAgainstSaleOrders (Inventory_Reports/RequiredRawMaterialAgainstSaleOrders.cs) - ScreenDefinition 294.
   Line refs ":NNN" are that file. API: /inventory/reports/required-raw-material-against-sale-orders/api. */
(function () {
    'use strict';
    var SC = window.StoreCommon, X = window.Inv19a, $id = SC.$id, num = SC.num, API = '/inventory/reports/required-raw-material-against-sale-orders/api';
    var F2 = { dec: 2 }, F2P = { dec: 2, paren: true };          // "#,##0.##"  /  "#,##0.##;(#,##0.##)"

    var st = { init: null, history: [], historyRaw: [], raw: [] };

    /* GridSetting :222 - Id, ItemId, RawPackUom, Equivalent hidden; OrderItem link + group (GroupTotals Always); RequiredWt summed. */
    var gridHistory = new X.Grid('GridHistory', [
        { key: 'Id', hidden: true }, { key: 'ItemId', hidden: true },
        { key: 'OrderItem', caption: 'Order Item', width: 170, link: true, title: 'frmSaleOrderHistory (Open orders of this item)' },
        { key: 'OrderUom', caption: 'Order Uom', width: 70 },
        { key: 'OrderQty', caption: 'Order Qty', width: 80, num: F2 },
        { key: 'OrderWeight', caption: 'Order Weight', width: 90, num: F2 },
        { key: 'RawItem', caption: 'Raw Item', width: 170 },
        { key: 'RawPackUom', hidden: true },
        { key: 'RequiredWt', caption: 'Required Wt', width: 90, num: F2, sum: true },
        { key: 'Equivalent', hidden: true },
        { key: 'RawPrct', caption: 'Raw Prct', width: 70, num: F2 }
    ], { groups: ['OrderItem'], navigator: '#navHistory', onLink: GridHistory_LinkClicked });

    /* GridRawSettings :185 - positions ItemName 0, RequiredGrossQty 1, RequiredGrossWt 2, AvailableStockWeight 3, NetRequiredWeight 4; all four numerics summed. */
    var gridRaw = new X.Grid('GridRawItems', [
        { key: 'ItemId', hidden: true },
        { key: 'ItemName', caption: 'Item Name', width: 170, link: true, title: 'frmStockReport (this item)' },
        { key: 'RequiredGrossQty', caption: 'Required Gross Qty', width: 75, num: F2P, sum: true },
        { key: 'RequiredGrossWt', caption: 'Required Gross Wt', width: 75, num: F2P, sum: true },
        { key: 'AvailableStockWeight', caption: 'Available Stock Weight', width: 75, num: F2P, sum: true },
        { key: 'NetRequiredWeight', caption: 'Net Required Weight', width: 75, num: F2P, sum: true }
    ], { navigator: '#navRaw', onLink: GridRawItems_LinkClicked });

    function packUom() { return num($id('txtqtyUom').value); }

    /* GridFill :121 (server reshapes both DataSet tables and runs UpdateBal :394) */
    function GridFill() {
        return SC.postJson(API + '/show', { toDate: $id('datToDate').value, packUom: packUom() }).then(function (d) {
            st.history = d.history || []; st.historyRaw = d.historyRaw || []; st.raw = d.rawItems || [];
            gridHistory.setRows(st.history);                 // empty -> GridHistory.ClearStructure() :162
            gridRaw.setRows(st.raw);
        });
    }

    /* txtqtyUom_TextChanged :371 - RequiredGrossQty = RequiredGrossWt / packUom when 0 < packUom <= 100 */
    function txtqtyUom_TextChanged() {
        var p = packUom();
        if (p > 0 && p <= 100) { st.raw.forEach(function (r) { r.RequiredGrossQty = num(r.RequiredGrossWt) / p; }); gridRaw.renderBody(); }
    }
    /* txtqtyUom_Leave :433 */
    function txtqtyUom_Leave() {
        var p = packUom();
        if (p > 0 && p <= 100) { txtqtyUom_TextChanged(); return; }
        alert('You Can not Enter PackUom greater than 100 and Less Than Zero...');
        $id('txtqtyUom').value = '100';
        txtqtyUom_TextChanged();
    }

    /* GridHistory_LinkClicked :309 - frmSaleOrderHistory(FromDate = FY Start_Period, To_Date = now, CmbItem = ItemId, OrderStatus "Open", IsApproved "All") */
    function GridHistory_LinkClicked(key, r) {
        if (key !== 'OrderItem' || !r) return;
        var itemId = num(r.ItemId);
        if (itemId > 0) window.open('/sale/reports/engr/sale-order-history' + SC.qs({ itemId: itemId, fromDate: st.init ? st.init.fyStart : '', toDate: SC.today(), orderStatus: 'Open', isApproved: 'All' }), '_blank');
    }
    /* GridRawItems_LinkClicked :337 - frmStockReport(cmbItem = ItemId) */
    function GridRawItems_LinkClicked(key, r) {
        if (key !== 'ItemName' || !r) return;
        var itemId = num(r.ItemId);
        if (itemId > 0) window.open('/inventory/stock-report' + SC.qs({ itemId: itemId }), '_blank');
    }

    /* btnRegister_Click :268 - dtHistory (the untouched Tables[0]) -> 464-RequiredRawMaterialRegisterTwo.rpt */
    function btnRegister_Click() { X.printRows('464-RequiredRawMaterialRegisterTwo.rpt', st.historyRaw, 'No Record Found For Display'); }
    /* txtSummPrint_Click :453 - RawItemdt -> 463-RequiredRawMaterialRegister.rpt */
    function txtSummPrint_Click() {
        X.printRows('463-RequiredRawMaterialRegister.rpt', st.raw.map(function (r) {
            return { ItemId: r.ItemId, ItemName: r.ItemName, RequiredGrossWt: r.RequiredGrossWt, RequiredGrossQty: r.RequiredGrossQty, AvailableStockWeight: r.AvailableStockWeight, NetRequiredWeight: r.NetRequiredWeight };
        }), 'No Record Found For Display');
    }

    /* RequiredRawMaterialAgainstSaleOrders_Load :88 - GridFill() at once with datToDate = today */
    function load() {
        SC.getJson(API + '/init').then(function (d) {
            st.init = d;
            $id('datToDate').value = d.toDate || SC.today();
            $id('txtqtyUom').value = String(d.packUom || 100);
            return GridFill();
        }).catch(function (e) { alert(e.message); });
    }

    document.addEventListener('DOMContentLoaded', function () {
        $id('btnshow').addEventListener('click', function () { SC.withBusy($id('btnshow'), function () { return GridFill().catch(function (e) { alert(e.message); }); }); });   // btnshow_Click :109
        $id('btnRegister').addEventListener('click', btnRegister_Click);
        $id('txtSummPrint').addEventListener('click', txtSummPrint_Click);
        $id('txtqtyUom').addEventListener('keypress', function (e) {   // txtqtyUom_KeyPress :359 - CommonServices.OnlytextNumberFunction
            if (e.key.length === 1 && !/[0-9.]/.test(e.key)) e.preventDefault();
            if (e.key === '.' && e.target.value.indexOf('.') >= 0) e.preventDefault();
        });
        $id('txtqtyUom').addEventListener('input', txtqtyUom_TextChanged);
        $id('txtqtyUom').addEventListener('blur', txtqtyUom_Leave);
        document.addEventListener('keydown', function (e) { if (e.key === 'Escape') X.closeForm(); });
        load();
    });
})();
