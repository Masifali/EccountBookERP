/* ============================================================================================
 * Screen 491 SalePriceListWithDiscount - Architecture.WinApp.WholeSale.SalePriceListWithDiscount (SalePriceListWithDiscount.cs)
 * Page: templates/sale/reports/sarpt1_sale_price_list_with_discount.html (route /sale/reports/sale-price-list-with-discount).
 *   SalePriceListWithDiscount_Load :192  RoundingFactor from the "RoundingForItemPricing" config, ItemCategoryFill, GetItemPriceList (shows at once)
 *   ItemCategoryFill        :224  CommonServices.GetItemCategoryFromPricingShedule(6) -> Id = ItemCategoryId, Name = CategoryDescription,
 *                                 BindDDL(ZeroIndex false, caption "Item Category"), second column width 350
 *   GetItemPriceList        :89   USP_GetItemSalePriceListWithDiscounts (EffectedDate, ItemCategoryId); six "n%Dicount" columns from
 *                                 GeneratePriceForPercent (:210, done by the server); ItemPrice + the six are right aligned "#,#";
 *                                 captions BRONZE / PEARL / SILVER / GOLD / PLATINUM / DIAMOND; GridAutoAdjustment; no rows -> ClearStructure
 *   btnShow_Click :77, btnRefresh_Click :248 (ItemCategoryFill), btnRegister_Click :260 (616-GetItemSalePriceListRegister.rpt over the grid table)
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, el = A.el;
    var API = '/api/sale/sarpt1/sale-price-list-with-discount';
    var args = null, seq = 0;

    function bindCategories(rows) {
        S.fill(el('cmbItemCategory'), (rows || []).map(function (r) {
            return { Id: A.ci(r, 'ItemCategoryId'), name: A.ci(r, 'CategoryDescription') };
        }), false);
    }
    var grid = new S.Grid({ tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', headerLines: 1, autosize: true });
    function colDefs() {
        var pct = ['BRONZE', 'PEARL', 'SILVER', 'GOLD', 'PLATINUM', 'DIAMOND'];
        var cols = [
            { key: 'CategoryDescription', caption: 'CategoryDescription' },
            { key: 'EffectedDate', caption: 'EffectedDate', date: 'dd-MMM-yy' },
            { key: 'ItemName', caption: 'ItemName' },
            { key: 'RateUom', caption: 'RateUom' },
            { key: 'ItemPrice', caption: 'ItemPrice', fmt: '#,#', num: true }
        ];
        pct.forEach(function (c, i) { cols.push({ key: (i + 1) + '%Dicount', caption: c, fmt: '#,#', num: true }); });
        return cols;
    }
    function currentArgs() { return { effectedDate: el('txtEffectedDate').value, itemCategoryId: S.selInt('cmbItemCategory') }; }

    function show() {
        var b = el('show'); if (b.disabled) return;
        var a = currentArgs(), token = ++seq;
        A.busy(b, true);
        A.getJson(API + '/rows?' + S.qs(a)).then(function (rows) {
            if (token !== seq) return;
            args = a;
            if (!rows || !rows.length) { grid.clear(); return; }               // ClearStructure
            grid.setData(colDefs(), rows);
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }
    function loadCategories() {
        return A.getJson(API + '/lookups').then(function (data) { bindCategories(data.categories); });
    }
    function refresh() {
        var b = el('refresh'); if (b.disabled) return;
        A.busy(b, true);
        loadCategories().catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function print() {
        var b = el('print'); if (b.disabled) return;
        A.busy(b, true);
        A.openPdf('/sale/reports/sarpt1/print/sale-price-list-with-discount?' + S.qs(args || currentArgs()))
            .catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }

    el('show').addEventListener('click', show);
    el('refresh').addEventListener('click', refresh);
    el('print').addEventListener('click', print);
    el('txtEffectedDate').value = A.today();
    grid.render();
    loadCategories().catch(function (e) { alert(e.message); }).then(show);     // ItemCategoryFill, then GetItemPriceList
}(window, document));
