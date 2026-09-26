/* ============================================================================================
 * 456 "Stock Transfer Report" - frmStockTranfserRegister.cs (ScreenName StockTransferRegister)
 * API /api/store/reports/stock-transfer-register. Rules and notes: StoreReportsBService.
 * ============================================================================================ */
(function () {
    'use strict';

    var C = window.StoreCommon, K = window.StoreRptB, $id = C.$id;
    var API = '/api/store/reports/stock-transfer-register';
    var dtGrid = [];               /* dtGrid - the procedure rows of the last Show; 408-Register prints them */
    var branches = [];
    var yearStart = null;

    function val(id) { return $id(id).value; }
    function num(id) { return C.intOf(val(id)); }

    // ------------------------------------------------------------------ branch tick-list (BranchesFill:216)

    function renderBranches(list, checked) {
        branches = list || [];
        var on = {};
        (checked || []).forEach(function (id) { on[id] = 1; });
        $id('CmbBranchNamePanel').innerHTML = '<label class="rb-multi-head">Branch Name</label>' + branches.map(function (b) {
            return '<label><input type="checkbox" value="' + C.esc(b.Id) + '"' + (on[b.Id] ? ' checked' : '') + '> ' + C.esc(b.Name) + '</label>';
        }).join('');
        $id('CmbBranchNamePanel').querySelectorAll('input').forEach(function (i) { i.onchange = syncBranchText; });
        syncBranchText();
    }
    function checkedBranchIds() {
        return Array.prototype.map.call($id('CmbBranchNamePanel').querySelectorAll('input:checked'), function (i) { return +i.value; });
    }
    /* CheckedListSettings ListSeparator "," - the combo's Text is the ticked names */
    function syncBranchText() {
        var on = {};
        checkedBranchIds().forEach(function (id) { on[id] = 1; });
        $id('CmbBranchNameText').textContent = branches.filter(function (b) { return on[b.Id]; })
            .map(function (b) { return b.Name; }).join(',');
    }
    function toggleBranches() {
        var box = $id('CmbBranchName');
        if (box.classList.contains('is-open')) { box.classList.remove('is-open'); branchLeave(); }
        else box.classList.add('is-open');
    }
    document.addEventListener('mousedown', function (e) {
        var box = $id('CmbBranchName');
        if (box && box.classList.contains('is-open') && !box.contains(e.target)) { box.classList.remove('is-open'); branchLeave(); }
    });

    /* CmbBranchName_Leave:543 - ticked branches rebuild the combos; none ticked clears their selection */
    function branchLeave() {
        if (checkedBranchIds().length) return comboBind();
        /* :548-561 Text = string.Empty; ActiveRow = null - the combos show nothing (Value -> 0) */
        ['CmbFromWareHouse', 'CmbToWareHouse', 'cmbItem', 'CmbFromJobLot', 'CmbJobLotTo'].forEach(K.clearCombo);
        return Promise.resolve();
    }

    // ------------------------------------------------------------------ combos

    /* ComboBind:114 - an empty answer leaves the lists as they were (:139). Otherwise every combo is
       re-bound with BindDDL(..., ZeroIndex: true) (:182-186): "...Select Any Value..." (0) first and
       Value = 0, so a previous selection is dropped - even for a list that came back empty. */
    function comboBind() {
        return C.getJson(API + '/combos' + C.qs({ branchIds: checkedBranchIds().join(',') })).then(function (d) {
            if (d.empty) return;
            K.bindZeroIndex('cmbItem', d.items, 'Id', 'Name');                   /* :182 */
            K.bindZeroIndex('CmbFromJobLot', d.jobLotsFrom, 'Id', 'Name');       /* :183 */
            K.bindZeroIndex('CmbJobLotTo', d.jobLotsTo, 'Id', 'Name');           /* :184 */
            K.bindZeroIndex('CmbFromWareHouse', d.warehousesFrom, 'Id', 'Name'); /* :185 */
            K.bindZeroIndex('CmbToWareHouse', d.warehousesTo, 'Id', 'Name');     /* :186 */
        }).catch(function (e) { alert(e.message); });
    }

    /* DocumentTypefill:198 - BindDDLNew, no row active: the blank entry is "no active row" */
    function documentTypeFill(list) {
        var el = $id('cmbDocumenttype');
        el.innerHTML = '<option value=""></option>' + (list || []).map(function (r) {
            return '<option value="' + C.esc(r.Name) + '">' + C.esc(r.Name) + '</option>';
        }).join('');
        el.value = '';
    }

    // ------------------------------------------------------------------ load / toolbar

    /* frmGatePassReport_Load:244 - dates, BranchesFill, ComboBind, DocumentTypefill, GridFill */
    function load() {
        C.getJson(API + '/lookups').then(function (d) {
            yearStart = d.fromDate;
            if (yearStart) $id('gpFromDate').value = yearStart;
            $id('gpToDate').value = d.toDate || C.today();
            K.guardDate('gpFromDate'); K.guardDate('gpToDate');               /* a DateTimePicker is never empty */
            renderBranches(d.branches, d.defaultBranchIds);
            documentTypeFill(d.documentTypes);
            return comboBind();
        }).then(function () {
            $id('gpFromDate').focus();
            return show();
        }).catch(function (e) { alert(e.message); });
    }

    /* btnRefresh_Click:480 ("New") - dates, Item Name text cleared (no active row), then GridFill */
    function newClick() {
        if (yearStart) $id('gpFromDate').value = yearStart;
        $id('gpToDate').value = C.today();
        K.clearCombo('cmbItem');
        return show();
    }

    /* toolStripButton1_Click_1:495 ("Refresh") - ComboBind + DocumentTypefill */
    function refresh() {
        return comboBind().then(function () {
            return C.getJson(API + '/document-types').then(documentTypeFill);
        }).catch(function (e) { alert(e.message); });
    }

    // ------------------------------------------------------------------ grid

    /* GridFill:269 */
    function show() {
        var ids = checkedBranchIds();
        if (!ids.length) { $id('CmbBranchNameText').focus(); alert('Select Branch First'); return Promise.resolve(); }  /* :346-347 */
        var q = C.qs({
            branchIds: ids.join(','), fromDate: val('gpFromDate'), toDate: val('gpToDate'),
            itemId: num('cmbItem'), fromWarehouseId: num('CmbFromWareHouse'), toWarehouseId: num('CmbToWareHouse'),
            jobLotId: num('CmbFromJobLot'), toJobLotId: num('CmbJobLotTo'), documentType: val('cmbDocumenttype')
        });
        return C.getJson(API + q).then(function (d) {
            dtGrid = d.raw || [];
            var rows = (d.rows || []).map(function (r, i) { return { r: r, i: i }; });
            if (!rows.length) { K.clear('grdfrm'); return; }                /* :342 */
            /* layout SortKey ColIndex 22 = Qty, ascending */
            rows.sort(function (a, b) { return (K.toNum(a.r.Qty) - K.toNum(b.r.Qty)) || (a.i - b.i); });
            K.render('grdfrm', columns(), rows.map(function (x) { return x.r; }));
        }).catch(function (e) { alert(e.message); });
    }

    /* Column sets of grdfrm_DesignTimeLayout (order), their columns' captions, grdfrmSetting:355 */
    function columns() {
        var w = function (v) { return K.fmt(v, '#,##0.##'); };
        var qty = function (v) { return K.fmt(v, '#,#'); };
        var t = function (k, cap) { return { key: k, caption: cap }; };
        var weight = function (k, cap) { return { key: k, caption: cap, align: 'right', agg: 'sum', format: w, total: w }; };
        return [
            t('DocNo', 'Doc No'),
            { key: 'DocDate', caption: 'Doc Date', format: K.dMMMyy },                     /* :359 dd-MMM-yy */
            t('GpSrNo', 'Gp No'),
            t('DoNo', 'Do No'),
            t('TicketNo', 'Ticket No'),
            t('CropYear', 'Crop'),
            t('TransferType', 'Transfer Type'),
            t('ItemName', 'Item Name'),
            { key: 'Equivalent', caption: 'Pack Size', align: 'center' },                 /* :360, :389 */
            { key: 'Qty', caption: 'Qty', align: 'right', agg: 'sum', format: qty, total: qty },  /* :395-398 */
            weight('GrossWeight', 'Gross Weight'),
            weight('NetWeight', 'Net Weight'),
            weight('WbNetWeight', 'WeighBridge Weight'),
            t('OtherWeight', 'Other Weight'),                                              /* bound to "OtherWeight." - blank */
            weight('DiffWeight', 'Difference Weight'),
            weight('AdLsWeight', 'Add Less Weight'),                                       /* :403-405, no TotalFormatString */
            { key: 'EbUnit', caption: 'Eb Unit', align: 'center' },                        /* :390 */
            t('EbTotal', 'Eb Total'),                                                      /* bound to "EbTotal " - blank */
            t('ToWareHouseName', 'To WareHouse Name'),
            t('VehicleNo', 'Vehicle No'),
            t('BiltyNo', 'Bilty No'),
            t('JobLotTo', 'Job Lot To'),
            t('WorkingReportNo', 'Working Report No'),
            t('EntryUserName', 'Entry User')
            /* EntryDate, ApprovedDate, ApprovedUserName, IsApproved: column sets hidden (:415-418); Id hidden (:414) */
        ];
    }

    /* toolStripButton1_Click:457 (408-StockTransferRegister.rpt) */
    function printRegister() {
        if (!dtGrid.length) { alert('Not Record Found For Display'); return; }
        K.printRows(dtGrid, '408-StockTransferRegister');
    }

    /* Footer History button (web): the history grid is below the filters, as on the desktop. */
    function gotoHistory() { $id('historySection').scrollIntoView({ behavior: 'smooth', block: 'start' }); }

    window.RptST = { show: show, newClick: newClick, refresh: refresh, printRegister: printRegister,
        toggleBranches: toggleBranches, gotoHistory: gotoHistory };
    document.addEventListener('DOMContentLoaded', load);
})();
