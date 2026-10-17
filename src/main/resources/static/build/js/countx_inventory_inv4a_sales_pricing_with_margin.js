/* frmItemSalesPricingSchedule (WholeSale/frmItemSalesPricingSchedule.cs) — ScreenDefinition 100 "Sales Pricing (With Margin)".
   Line refs ":NNN" are that file. API: /inventory/sales-pricing-with-margin/api. priceTypeId 6.
     frmItemPricingSchedule_Load :215   rights, RoundingForItemPricing, FillDiscountTypes, ItemFill, BindCropYear, grid columns, history combos
     cmbDiscType_Leave :1133            ItemFill (GetItemsFromRecipe by discount type) — checked-list combo
     btnAdd_Click :685 "Show"           "Packing Uom Field Required" → GetRecipsForFinalList → dtGrid → GridSetting → FinalAmountCalculate
     txtMarginPercent_TextChanged :942  FinalAmountCalculate → FinalGridFill (rounding by RoundingFactor)
     Insert :455                        confirm → "Item Required" → Save → "Save Successfully" / "Update Successfully" → Reset
     History: BtnShowHistoryReport :1001 (FinalItemSalePriceListSlip → DataGridHistory + GrdForFinalPrice), btnPrint :964 (324),
              btnPrint324A :1157 (dtForFinalPrice), BtnUpdateHis :1218 (UpdateIsActiveFromHistory then Show again)
     KeyDown :545                       Ctrl+S / Ctrl+N / Ctrl+T / Ctrl+E,Esc / Ctrl+U */
(function () {
    'use strict';
    var SC = window.StoreCommon, X = window.Inv4a, $id = SC.$id, API = '/inventory/sales-pricing-with-margin/api';

    var st = { rights: {}, roundingFactor: 0, items: [], dtGrid: [], dtFinal: [], history: [], dtForFinalPrice: [], original: [], recId: 0 };

    function rateCap() { return 'Rate ' + $id('txtPackingUom').value.trim() + ' KG'; }           // :764 / :654
    /* grd — dtGrid :238-255 with GridSetting :739 (hidden: Id, BrandCropYearId, BrandCropYear, BrandId, ItemId, PreviousRate, RateChangeable, SemiEffectedDate, CropYearId, Prcnt; grouped by BrandName; red when RateChangeable = 1) */
    function gridCols() {
        return [
            { key: 'Id', hidden: true }, { key: 'BrandId', hidden: true }, { key: 'BrandName', hidden: true }, { key: 'BrandCropYearId', hidden: true }, { key: 'BrandCropYear', hidden: true },
            { key: 'ItemId', hidden: true }, { key: 'ItemName', cap: 'ItemName', width: 250 }, { key: 'CropYearId', hidden: true }, { key: 'CropYear', cap: 'CropYear' },
            { key: 'PreviousRate', hidden: true }, { key: 'ItemRate', cap: 'ItemRate', fmt: '#,#', width: 120 }, { key: 'RateChangeable', hidden: true },
            { key: 'Percent', cap: 'Percent', width: 60 }, { key: 'Prcnt', hidden: true }, { key: 'PercentRate', cap: 'PercentRate', fmt: '#,#', width: 120 },
            { key: 'Expenses', cap: 'Expenses', fmt: '#,#', width: 100 }, { key: 'PackingRate', cap: rateCap(), fmt: '#,#', tfmt: '#,#.#', width: 120 }, { key: 'SemiEffectedDate', hidden: true }
        ];
    }
    function renderGrid() {
        X.renderGrid('grd', gridCols(), { rows: st.dtGrid, group: 'BrandName', groupCaption: 'BrandName', groupTotals: true, totals: ['Expenses'], filter: true,
            redIf: function (r) { return X.toInt(r.RateChangeable) === 1; } });
    }
    /* grdFinal — FinalGridFill :852-866 */
    var FCOLS = [{ key: 'BrandId', hidden: true }, { key: 'BrandName', cap: 'BrandName', width: 200 }, { key: 'CropYearId', hidden: true }, { key: 'CropYear', cap: 'CropYear', width: 70 },
        { key: 'PackingRate', hidden: true }, { key: 'FinalRate', cap: 'Base Rate', fmt: '#,#', width: 120 }];
    function renderFinal() { X.renderGrid('grdFinal', FCOLS, { rows: st.dtFinal }); }

    /* ItemFill :268 — GetItemsFromRecipe(DiscountTypeId); multi-check "Item Name" */
    function itemFill() {
        X.multi('cmbItem', []);
        return SC.getJson(API + '/items' + SC.qs({ discountTypeId: X.selVal('cmbDiscType') })).then(function (list) { st.items = list || []; X.multi('cmbItem', st.items); }).catch(X.fail);
    }
    /* Reset :390 */
    function reset() {
        $id('datEffectedDate').disabled = false; $id('datEffectedTime').disabled = false;
        $id('cmbDiscType').value = ''; X.multiClear('cmbItem'); $id('txtDescription').value = ''; $id('txtMarginPercent').value = '';
        $id('datEffectedDate').value = SC.today();
        st.dtGrid = []; st.dtFinal = []; X.clearGrid('grdFinal'); X.clearGrid('grd');
        $id('btnsave').classList.remove('is-hidden'); $id('btnUpdate').classList.add('is-hidden');
        st.recId = 0; $id('btnAdd').disabled = false;
        itemFill();
    }

    /* btnAdd_Click :685 */
    function btnAdd() {
        $id('datEffectedDate').disabled = true; $id('datEffectedTime').disabled = true;
        if (X.num($id('txtPackingUom').value) === 0) { alert('Packing Uom Field Required'); return; }
        var ids = '';
        X.multiChecked('cmbItem').forEach(function (c) { var r = st.items.filter(function (x) { return x.Name === c.name; })[0]; if (r) ids += ',' + r.Id; });   // :706-715
        st.dtGrid = []; st.dtFinal = []; X.clearGrid('grdFinal');
        SC.postJson(API + '/recipes', { packingUom: $id('txtPackingUom').value, effectedDate: $id('datEffectedDate').value, discountTypeId: X.selVal('cmbDiscType'),
            cropYearId: X.selVal('CmbCropYear'), itemIds: ids }).then(function (dt) {
            if (dt && dt.length) {
                var uom = X.num($id('txtPackingUom').value);
                st.dtGrid = dt.map(function (r) {                                                   // :722-726
                    var packingRate = X.num(r.PercentRate) / 40.0 * uom;
                    return { Id: r.Id, BrandId: r.BrandId, BrandName: r.BrandName, BrandCropYearId: X.toInt(r.BrandCropYearId), BrandCropYear: r.BrandCropYear, ItemId: r.sItemId, ItemName: r.ItemName,
                        CropYearId: X.num(r.CropYearId), CropYear: r.CropYear, PreviousRate: X.num(r.PreviousRate), ItemRate: X.num(r.ItemRate), RateChangeable: r.RateChangeable,
                        Percent: r.Percentage, Prcnt: X.num(r.Prcnt), PercentRate: X.num(r.PercentRate), Expenses: X.num(r.Expenses), PackingRate: packingRate + X.num(r.Expenses), SemiEffectedDate: r.SemiEffectedDate };
                });
                renderGrid();
                finalAmountCalculate();
            } else { X.clearGrid('grd'); }
        }).catch(X.fail);
    }

    /* FinalAmountCalculate :899 — PackingRate = (PercentRate / 40 * PackingUom + Expenses) / ("0." + (100 - Margin)) when Margin > 0 */
    function finalAmountCalculate() {
        try {
            if (st.dtGrid.length) {
                var uom = X.num($id('txtPackingUom').value.trim()), margin = X.num($id('txtMarginPercent').value.trim());
                st.dtGrid.forEach(function (r) {
                    var semiNet = X.num(r.PercentRate) / 40.0 * uom + X.num(r.Expenses);
                    if (margin > 100) { $id('txtMarginPercent').value = '0'; throw new Error('Margin cannot be greater than 100% please check'); }
                    var v = semiNet;
                    if (margin > 0) { var formula = X.num('0.' + String(100 - margin)); v = semiNet / formula; }       // Conversion.ToDouble("0.97.5") = 0 → ∞, as on the desktop
                    r.PackingRate = v;
                });
                renderGrid();
                finalGridFill();
            }
        } catch (e) { alert(e.message); }
    }
    function floorTo(a, n) { return Math.floor(a / n) * n; }
    function ceilTo(a, n) { return Math.ceil(a / n) * n; }
    /** Math.Round(x, 0) — MidpointRounding.ToEven */
    function roundEven(x) { var f = Math.floor(x), d = x - f; if (d > 0.5) return f + 1; if (d < 0.5) return f; return f % 2 === 0 ? f : f + 1; }
    /* FinalGridFill :795 — one row per (BrandId, BrandCropYearId) run; FinalPrice = Σ PackingRate; LetestRate by RoundingFactor */
    function finalGridFill() {
        if (!st.dtGrid.length) return;
        var RF = st.roundingFactor, half = Math.round(RF / 2 * 100) / 100, out = [], brnd = 0, crop = 0;
        st.dtGrid.forEach(function (r) {
            var bId = X.toInt(r.BrandId), cId = X.toInt(r.BrandCropYearId);
            if ((brnd === 0 && crop === 0) || brnd !== bId || crop !== cId) {
                var sum = st.dtGrid.filter(function (x) { return String(x.BrandId) === String(r.BrandId) && X.toInt(x.BrandCropYearId) === cId; }).reduce(function (a, x) { return a + (Number(x.PackingRate) || 0); }, 0);
                var num = sum % 100.0, cmp = roundEven(num / RF) * RF, diff = num - cmp, latest;
                if (!(diff > 0)) latest = ceilTo(sum, RF); else if (!(diff >= half)) latest = floorTo(sum, RF); else latest = ceilTo(sum, RF);
                out.push({ BrandId: r.BrandId, BrandName: r.BrandName, CropYearId: r.BrandCropYearId, CropYear: r.BrandCropYear, PackingRate: sum, FinalRate: latest });
                brnd = bId; crop = cId;
            }
        });
        st.dtFinal = out;
        renderFinal();
    }

    /* Insert :455 */
    function insert() {
        if (!confirm('Are you sure to Save?')) return;
        for (var i = 0; i < st.dtFinal.length; i++) if (X.toInt(st.dtFinal[i].BrandId) === 0) { alert('Item Required'); return; }
        SC.postJson(API + '/save', { recId: st.recId, effectedDate: $id('datEffectedDate').value, effectedTime: $id('datEffectedTime').value, marginPercent: $id('txtMarginPercent').value,
            packingUom: $id('txtPackingUom').value, description: $id('txtDescription').value, discountTypeText: X.selText('cmbDiscType'), finalRows: st.dtFinal, rows: st.dtGrid })
            .then(function (res) { alert(res.message); reset(); }).catch(X.fail);
    }

    /* ---------------------------------------------------------------- History tab */
    function hCols() {
        return [{ key: 'Id', hidden: true }, { key: 'BrandId', hidden: true }, { key: 'BrandName', hidden: true }, { key: 'BrandCropYearId', hidden: true }, { key: 'ItemId', hidden: true },
            { key: 'ItemName', cap: 'ItemName', width: 250 }, { key: 'CropYear', cap: 'CropYear' }, { key: 'PreviousRate', hidden: true }, { key: 'ItemRate', cap: 'ItemRate', fmt: '#,#', width: 120 },
            { key: 'RateChangeable', hidden: true }, { key: 'Percent', cap: 'Percent', width: 80 }, { key: 'PercentRate', cap: 'PercentRate', fmt: '#,#', width: 100 },
            { key: 'Expenses', cap: 'Expenses', fmt: '#,#', width: 100 }, { key: 'PackingRate', cap: rateCap(), fmt: '#,#', tfmt: '#,#.#', width: 120 }];
    }
    var PCOLS = [{ key: 'BrandId', hidden: true }, { key: 'BrandName', cap: 'BrandName', width: 250 }, { key: 'BrandCropYearId', hidden: true }, { key: 'BrandCropYear', cap: 'Crop Year', width: 60 },
        { key: 'Margin', cap: 'Margin', cls: 'c', width: 50 }, { key: 'PreviousRate', cap: 'PreviousRate', fmt: '#,#', width: 70 }, { key: 'PackingRate', cap: 'PackingRate', fmt: '#,#.#', width: 70 },
        { key: 'BaseRate', cap: 'BaseRate', fmt: '#,#', width: 70 }, { key: 'IsActive', cap: 'IsActive', chk: true, width: 60 }];
    function historyBody() { return { effectedDate: $id('txtEffectedDateForHistory').value, cropYearId: X.selVal('CmbCropYearHistory'), itemId: X.selVal('CmbItemNameHistory') }; }
    /* BtnShowHistoryReport_Click :1001 */
    function showHistory() {
        st.dtForFinalPrice = [];
        return SC.postJson(API + '/final-price-list', historyBody()).then(function (dt) {
            st.history = dt.map(function (r) { return { Id: r.Id, BrandId: r.BrandId, BrandName: r.BrandName, BrandCropYearId: r.BrandCropYearId, ItemId: r.sItemId, ItemName: r.ItemName, CropYear: r.CropYear,
                PreviousRate: X.num(r.PreviousRate), ItemRate: X.num(r.ItemRate), RateChangeable: r.RateChangeable, Percent: r.Percentage, PercentRate: X.num(r.PercentRate), Expenses: X.num(r.Expenses), PackingRate: X.num(r.PackingRateDetail) }; });
            X.renderGrid('DataGridHistory', hCols(), { rows: st.history, group: 'BrandName', groupCaption: 'BrandName', groupTotals: true, totals: ['Expenses'], filter: true });
            var seen = {};
            dt.forEach(function (r) {                                                              // :1066-1072 distinct BrandId + BrandCropYearId
                var k = String(r.BrandId) + '|' + String(r.BrandCropYearId);
                if (seen[k]) return; seen[k] = 1;
                st.dtForFinalPrice.push({ BrandId: r.BrandId, BrandName: r.BrandName, BrandCropYearId: r.BrandCropYearId, BrandCropYear: r.BrandCropYear, Margin: r.Margin, PreviousRate: X.num(r.PreviousRate),
                    PackingRate: X.num(r.PackingRate), BaseRate: X.num(r.MessageRate), IsActive: !!(r.IsActive === true || r.IsActive === 1 || r.IsActive === 'true' || r.IsActive === '1'),
                    CompName: r.CompName, CompAddress: r.CompAddress, CompEmailA: r.CompEmailA, CompEmailB: r.CompEmailB, CompLogoImage: r.CompLogoImage, ReportingRemarks: r.ReportingRemarks });
            });
            st.original = st.dtForFinalPrice.map(function (r) { return { BrandId: r.BrandId, BrandCropYearId: r.BrandCropYearId, IsActive: r.IsActive }; });
            X.renderGrid('GrdForFinalPrice', PCOLS, { rows: st.dtForFinalPrice, filter: true, onCheck: function (i, k, v) { st.dtForFinalPrice[i].IsActive = v; } });
        }).catch(function (e) { st.history = []; X.clearGrid('DataGridHistory'); X.clearGrid('GrdForFinalPrice'); X.fail(e); });   // :1020-1022
    }
    /* btnPrint_Click :964 — 324 with the slip rows for the three filters */
    function btnPrint() { SC.postJson(API + '/final-price-list', historyBody()).then(function (dt) { X.printTable('324-SalesItemPricingList.rpt', dt); }).catch(X.fail); }
    /* btnPrint324A_Click :1157 */
    function btnPrint324A() { if (!st.dtForFinalPrice.length) { alert('Record Not Found For Display'); return; } X.printTable('324A_SalesPricingList.rpt', st.dtForFinalPrice); }
    /* BtnUpdateHis_Click :1218 → UpdateIsActiveFromHistory :1179 → BtnShowHistoryReport_Click */
    function btnUpdateHis() {
        if (!confirm('Are you sure to Update?')) { showHistory(); return; }
        var changed = st.dtForFinalPrice.filter(function (r) {
            var o = st.original.filter(function (x) { return String(x.BrandId) === String(r.BrandId) && String(x.BrandCropYearId) === String(r.BrandCropYearId); })[0];
            return o && o.IsActive !== r.IsActive;
        }).map(function (r) { return { BrandId: r.BrandId, BrandCropYearId: r.BrandCropYearId, IsActive: r.IsActive }; });
        SC.postJson(API + '/active-inactive', { rows: changed }).then(function (res) { alert(res.message || 'Updated Successfully'); }).catch(X.fail).then(showHistory);
    }

    /* frmItemPricingSchedule_Load :215 */
    function load() {
        $id('datEffectedDate').value = SC.today(); $id('datEffectedTime').value = X.nowTime(); $id('txtEffectedDateForHistory').value = SC.today();
        SC.getJson(API + '/init').then(function (d) {
            st.rights = d.rights || {}; st.roundingFactor = X.num(d.roundingFactor);
            $id('btnsave').disabled = !st.rights.save; $id('btnUpdate').disabled = !st.rights.update;
            X.bindDdl('cmbDiscType', d.discountTypes || []);                                        // BindDDLNew "Discount Type"
            st.items = d.items || []; X.multi('cmbItem', st.items);
            X.bindDdl('CmbCropYear', d.cropYears || []);                                            // BindDDL "Crop Year"
            X.bindDdl('CmbCropYearHistory', d.historyCropYears || []);
            X.bindDdl('CmbItemNameHistory', d.historyItems || []);
            X.renderGrid('grd', gridCols(), { rows: [] });
        }).catch(X.fail);
    }

    var tabs;
    document.addEventListener('DOMContentLoaded', function () {
        tabs = X.tabs('tabRoot', null);                                                             // tabControl1_SelectedIndexChanged :594 is empty
        $id('btnnew').addEventListener('click', reset);
        $id('btnsave').addEventListener('click', insert);
        $id('btnUpdate').addEventListener('click', insert);
        $id('btnAllocateBrandtoDiscount').addEventListener('click', function () { alert('InvAllocateBrandItemToDiscType is a separate desktop form; it is not part of this page.'); });
        $id('btnAdd').addEventListener('click', btnAdd);
        $id('cmbDiscType').addEventListener('change', itemFill);                                    // Leave :1133
        $id('txtMarginPercent').addEventListener('input', function () { this.value = this.value.replace(/[^0-9.]/g, ''); finalAmountCalculate(); });   // OnlytextdecimelFunction + TextChanged :942
        $id('txtPackingUom').addEventListener('input', function () { this.value = this.value.replace(/[^0-9]/g, ''); });                               // OnlytextNumberFunction :875
        $id('BtnShowHistoryReport').addEventListener('click', showHistory);
        $id('btnPrint').addEventListener('click', btnPrint);
        $id('btnPrint324A').addEventListener('click', btnPrint324A);
        $id('BtnUpdateHis').addEventListener('click', btnUpdateHis);
        document.addEventListener('keydown', function (e) {                                         // :545
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
