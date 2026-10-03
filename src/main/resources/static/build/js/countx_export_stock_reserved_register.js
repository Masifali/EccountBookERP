/* ============================================================================================
 * countx_export_stock_reserved_register.js - frmStockReservedRegister.cs "Stock Reserved Against third Party Analysis"
 * (no ScreenDefinition row). Data: /api/export/stock-reserved-register (USP_InventoryStockReserved_SlipAndRegister,
 * Usp_AllComboAgainstInventoryStockReserved). Print: exp-o-225 (225_StockReservedRegister.rpt) with the filters of the last
 * search (the desktop prints dtFromDb). RefDocNo opens the third party analysis (/export/third-party-inspection?id=RefDocIdNo).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptO, $id = H.$id, box = H.box;
    var API = '/api/export/stock-reserved-register';
    var S = { rows: [], last: null, parents: [], picked: [] };
    /* grdSettings(): GridEX_Helper.GridWrappingAndColumnSettings(grd, 3 header lines, 2 decimals, hidden ids): decimal columns
       "#,##0.##" with Σ, *Amount* columns the amount format with Σ, *Rate* the rate format; Aflatoxins "#,##0.##". */
    var D = function (k) { return { key: k, caption: k, num: true, sum: true, fmt: 'o2' }; };
    var COLS = [
        { key: 'StockStatus', caption: 'StockStatus' }, { key: 'RefDocumentTypeId', hidden: true }, { key: 'RefDocIdNo', hidden: true },
        { key: 'RefDocSubIdNo', hidden: true }, { key: 'ThirdPartyAnalysisDate', caption: 'ThirdPartyAnalysisDate', fmt: 'd' },
        { key: 'LotRefNo', caption: 'LotRefNo' }, { key: 'RefDocNo', caption: 'RefDocNo', link: true }, { key: 'RefDocDate', caption: 'RefDocDate', fmt: 'd' },
        { key: 'ReportStatus', caption: 'ReportStatus' }, { key: 'SupplierCustomerId', hidden: true }, { key: 'PartyName', caption: 'PartyName' },
        { key: 'VehicleNo', caption: 'VehicleNo' }, { key: 'BiltyNo', caption: 'BiltyNo' }, { key: 'GpNo', caption: 'GpNo' },
        { key: 'WareHouseName', caption: 'WareHouseName' }, { key: 'ItemId', hidden: true }, { key: 'ItemName', caption: 'ItemName' },
        { key: 'ItemCode', caption: 'ItemCode' }, { key: 'ItemUom', caption: 'ItemUom' }, { key: 'CropYear', caption: 'CropYear' },
        { key: 'JobLot', caption: 'JobLot' }, { key: 'PackingType', caption: 'PackingType' }, D('Aflatoxins'), D('InQty'), D('OutQty'), D('BalQty'),
        D('InWeight'), D('OutWeight'), D('BalWeight'), D('ReservedQty'), D('ReservedWeight'),
        { key: 'ItemRate', caption: 'ItemRate', num: true, fmt: 'o4' }, { key: 'RateUom', caption: 'RateUom' },
        { key: 'Amount', caption: 'Amount', num: true, sum: true, fmt: 'n2' }, { key: 'TranRemarks', caption: 'TranRemarks' }
    ];

    /* ---------------------------------------------------------------- Parent Category (UltraCombo CheckedListSettings, "," separator) */
    function parentText() { return S.parents.filter(function (p) { return S.picked.indexOf(String(p.Id)) >= 0; }).map(function (p) { return p.name; }).join(','); }
    function drawParents(filter) {
        var pop = $id('CmbItemParentCategoryPop'), f = (filter || '').toLowerCase();
        var all = S.parents.length && S.parents.every(function (p) { return S.picked.indexOf(String(p.Id)) >= 0; });
        var h = '<input type="text" id="CmbItemParentCategorySearch" placeholder="Search..." value="' + H.esc(filter || '') + '"/>'
            + '<label class="srr-all"><input type="checkbox" data-all="1"' + (all ? ' checked' : '') + '/> Parent Categories</label>';
        S.parents.forEach(function (p) {
            if (f && String(p.name).toLowerCase().indexOf(f) < 0) return;
            h += '<label><input type="checkbox" data-id="' + H.esc(p.Id) + '"' + (S.picked.indexOf(String(p.Id)) >= 0 ? ' checked' : '') + '/> ' + H.esc(p.name) + '</label>';
        });
        pop.innerHTML = h;
        $id('CmbItemParentCategoryBtn').textContent = parentText();
    }
    function wireParents() {
        var btn = $id('CmbItemParentCategoryBtn'), pop = $id('CmbItemParentCategoryPop');
        btn.addEventListener('click', function () { pop.classList.toggle('is-hidden'); if (!pop.classList.contains('is-hidden')) { drawParents(''); var s = $id('CmbItemParentCategorySearch'); if (s) s.focus(); } });
        pop.addEventListener('input', function (e) { if (e.target.id === 'CmbItemParentCategorySearch') { var v = e.target.value; drawParents(v); var s = $id('CmbItemParentCategorySearch'); s.focus(); s.setSelectionRange(v.length, v.length); } });
        pop.addEventListener('change', function (e) {
            var t = e.target; if (t.type !== 'checkbox') return;
            if (t.getAttribute('data-all')) S.picked = t.checked ? S.parents.map(function (p) { return String(p.Id); }) : [];
            else {
                var id = t.getAttribute('data-id'), k = S.picked.indexOf(id);
                if (t.checked && k < 0) S.picked.push(id); else if (!t.checked && k >= 0) S.picked.splice(k, 1);
            }
            drawParents(($id('CmbItemParentCategorySearch') || {}).value || '');
        });
        document.addEventListener('mousedown', function (e) { if (!$id('CmbItemParentCategory').contains(e.target)) pop.classList.add('is-hidden'); });
    }
    /** PendingTransactions(): the checked names are matched back to ids in text order, each followed by "," (trailing comma kept). */
    function parentIds() {
        var text = parentText();
        if (!text) return '';
        var ids = '';
        (text.trim() + ',').split(',').forEach(function (name) {
            for (var i = 0; i < S.parents.length; i++) if (String(S.parents[i].name) === name) { ids += String(S.parents[i].Id) + ','; break; }
        });
        return ids;
    }

    /* ---------------------------------------------------------------- combos */
    /** StockComboFill: InfragisticsHelper.BindAndRetainSelection (selection kept) per ActivityType; Parent Categories re-bound. */
    function comboFill(g) {
        g = g || {};
        H.bind('CmbWarehouse', g.WareHouse, 'Id', 'name', true);
        H.bind('CmbCropYear', g.CropYear, 'Id', 'name', true);
        H.bind('CmbJobLot', g.JobLot, 'Id', 'name', true);
        H.bind('CmbItemName', g.ItemName, 'Id', 'name', true);
        H.bind('CmbSupplier', g.Supplier, 'Id', 'name', true);
        H.bind('CmbReferenceDocument', g.RefRefDocumentType, 'Id', 'name', true);
        H.bind('CmbLotTrackingNo', g.RefDocNo, 'Id', 'name', true);
        H.bind('CmbClassGroup', g.ItemClassGroup, 'Id', 'name', true);
        S.parents = g.ItemParentCategory || [];
        S.picked = [];                                   /* DDL.BindDDL re-creates the checked list */
        drawParents('');
    }

    function filters() {
        return {
            fromDate: H.checked('RefFromDateChk') ? H.val('RefFromDate') : '',
            toDate: H.checked('RefTodateChk') ? H.val('RefTodate') : '',
            docNoFrom: H.netI(H.val('RefDocNoFrom')), docNoTo: H.netI(H.val('RefDocNoTo')),
            warehouseId: H.netI(H.val('CmbWarehouse')), jobLotId: H.netI(H.val('CmbJobLot')), itemId: H.netI(H.val('CmbItemName')),
            customerId: H.netI(H.val('CmbSupplier')), refRefDocumentTypeId: H.netI(H.val('CmbReferenceDocument')),
            refDocId: H.netI(H.val('CmbLotTrackingNo')),
            actionId: H.checked('rdReserved') ? 1 : H.checked('rdNonReserved') ? 2 : 0,
            statusId: H.checked('rdSamplingPending') ? 1 : H.checked('rdSamplingComplete') ? 2 : 0,
            skipZero: H.checked('chkSkipZero') ? 1 : 0,
            parentCategoryIds: parentIds(), classGroupId: H.netI(H.val('CmbClassGroup'))
            /* obj.CropYear (combo text) is set by the form but the BLL only reads CropYearId - never sent */
        };
    }
    /** rows -> RetrieveStructure + grdSettings; none -> ClearStructure. */
    function pending() {
        var f = filters();
        return H.postJson(API + '/show', f).then(function (rows) {
            S.rows = rows || []; S.last = f;
            if (S.rows.length) { H.drawGrid('grd', COLS, S.rows, {}); H.show('grdEmpty', false); }
            else { H.clearGrid('grd'); $id('grd').querySelector('thead').innerHTML = ''; H.show('grdEmpty', true); }
        }).catch(function (e) { box(e.message); });
    }
    function load() {
        H.setText('RefTodate', H.today());
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            H.setText('RefFromDate', d.yearStart || H.today());   /* RefFromDate.Value = ActiveYr.Start_Period */
            if (d.comboError) { box(d.comboError); return; }
            comboFill(d.combos);
            $id('RefFromDateChk').focus();
        }).catch(function () { box('Error occurred during database call.'); });
    }
    function show(btn) { return H.busy(btn, pending); }
    /** btnNew_Click ("&Reset"): six combo texts blank, grid cleared, PendingTransactions, focus Ref Date From. */
    function btnNew(btn) {
        ['CmbWarehouse', 'CmbCropYear', 'CmbJobLot', 'CmbItemName', 'CmbSupplier', 'CmbLotTrackingNo'].forEach(function (id) { H.setVal(id, '0'); });
        H.clearGrid('grd');
        return H.busy(btn, pending).then(function () { $id('RefFromDateChk').focus(); });
    }
    /** btnRefresh_Click: StockComboFill(ComboDbCall()). */
    function btnRefresh(btn) {
        return H.busy(btn, function () { return H.getJson(API + '/combos').then(function (d) { comboFill((d || {}).combos); }); });
    }
    /** btnRegister_Click: "Not Record Found For Display" without rows, else 225 (@PrintedBy = user name). */
    function btnRegister(btn) {
        if (!S.rows.length || !S.last) { box('Not Record Found For Display'); return; }
        var f = S.last;
        return H.print('exp-o-225', { refDocId: f.refDocId, refRefDocumentTypeId: f.refRefDocumentTypeId, fromDate: f.fromDate, toDate: f.toDate,
            docNoFrom: f.docNoFrom, docNoTo: f.docNoTo, supplierCustomerId: f.customerId, warehouseId: f.warehouseId, itemId: f.itemId,
            jobLotId: f.jobLotId, actionId: f.actionId, statusId: f.statusId, skipZero: f.skipZero, classGroupId: f.classGroupId,
            parentCategoryIds: f.parentCategoryIds }, btn);
    }
    function shortcuts() {
        H.showShortcuts([['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
            ['Ctrl+F5', 'For Focus On DateType '], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
            ['Ctrl+ArrowUp', 'For Focus On DateType in Filter'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }
    function digitsOnly(id) {   /* CommonServices.OnlytextNumberFunction + ShortcutsEnabled = false */
        var e = $id(id);
        e.addEventListener('keypress', function (ev) { if (ev.key && ev.key.length === 1 && !/[0-9]/.test(ev.key)) ev.preventDefault(); });
        e.addEventListener('paste', function (ev) { ev.preventDefault(); });
    }
    document.addEventListener('DOMContentLoaded', function () {
        H.fullscreenButtons();
        wireParents();
        digitsOnly('RefDocNoFrom'); digitsOnly('RefDocNoTo');
        ['RefFromDate', 'RefTodate'].forEach(function (id) {
            $id(id + 'Chk').addEventListener('change', function () { $id(id).disabled = !this.checked; });
        });
        H.gridEvents('grd', {
            link: function (key, i) {
                var r = S.rows[i]; if (!r) return;
                if (key === 'RefDocNo' && H.netI(r.RefDocIdNo) > 0) global.open('/export/third-party-inspection?id=' + H.netI(r.RefDocIdNo), '_blank');
            }
        });
        document.addEventListener('keydown', function (e) {
            if (H.baseKeys(e, shortcuts)) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 's') { e.preventDefault(); show($id('btngrnlod')); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew($id('btnNew')); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); btnRefresh($id('btnRefresh')); }
            else if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); $id('RefFromDateChk').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grd'); }
        });
        load();
    });
    global.ExportSrr = { show: show, btnNew: btnNew, btnRefresh: btnRefresh, btnRegister: btnRegister, shortcuts: shortcuts };
}(window));
