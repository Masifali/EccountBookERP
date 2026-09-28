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
    var pending = false, initialized = false, notice;
    var comboIds = ['cmbItem', 'CmbFromJobLot', 'CmbJobLotTo', 'CmbFromWareHouse', 'CmbToWareHouse'];

    function message(value, error) {
        if (!notice) {
            notice = document.createElement('div'); notice.id = 'stockTransferLookupStatus';
            notice.setAttribute('role', 'status'); notice.setAttribute('aria-live', 'polite');
            notice.style.cssText = 'padding:8px 10px;font:13px Verdana,sans-serif;background:#edf7f7;border:1px solid #b6d8d8;white-space:normal;';
            $id('historySection').before(notice);
        }
        notice.textContent = value || ''; notice.hidden = !value;
        notice.style.color = error ? '#a00000' : '#005959';
    }

    /* Keep the whole lookup/Show chain busy, including initial load and branch changes. */
    function run(work) {
        if (pending) return Promise.resolve();
        pending = true; message('Loading...');
        var controls = Array.from(document.querySelectorAll('.rb-root button,.rb-root input,.rb-root select'))
            .map(function (el) { return { el: el, disabled: el.disabled }; });
        controls.forEach(function (s) { s.el.disabled = true; s.el.setAttribute('aria-busy', 'true'); });
        return Promise.resolve().then(work).catch(function (e) {
            message(e.message || 'Could not load the report. Click Refresh to try again.', true);
        }).finally(function () {
            controls.forEach(function (s) { s.el.disabled = s.disabled; s.el.removeAttribute('aria-busy'); });
            pending = false;
            if (notice && notice.textContent === 'Loading...') message('');
        });
    }
    function get(url) {
        return C.getJson(url).then(function (data) {
            // The shared helper can return null after a redirect to an HTML login page.
            if (data == null) throw new Error('The session has expired or filters could not be loaded. Sign in again, then click Refresh.');
            return data;
        });
    }

    function val(id) { return $id(id).value; }
    function num(id) { return C.intOf(val(id)); }

    // ------------------------------------------------------------------ branch tick-list (BranchesFill:216)

    function renderBranches(list, checked) {
        branches = list || [];
        var on = {};
        (checked || []).forEach(function (id) { on[id] = 1; });
        $id('CmbBranchNamePanel').innerHTML = '<label class="rb-multi-head">Branch Name</label>' + branches.map(function (b) {
            return '<label><input type="checkbox" value="' + C.esc(b.Id) + '"' + (on[b.Id] ? ' checked' : '') + '> ' + C.esc(b.Name) + '</label>';
        }).join('') + (!branches.length ? '<label>No branches with stock transfers.</label>' : '');
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
            .map(function (b) { return b.Name; }).join(',') || (branches.length ? 'Select Branch' : 'No stock-transfer branches');
    }
    function toggleBranches() {
        if (pending) return;
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
        if (checkedBranchIds().length) return run(comboBind);
        /* :548-561 Text = string.Empty; ActiveRow = null - the combos show nothing (Value -> 0) */
        comboIds.forEach(K.clearCombo);
        return Promise.resolve();
    }

    // ------------------------------------------------------------------ combos

    /* ComboBind:114. Unlike the desktop's early return on empty (:139), clear stale options and
       explain why the report has no choices. Every populated combo is
       re-bound with BindDDL(..., ZeroIndex: true) (:182-186): "...Select Any Value..." (0) first and
       Value = 0, so a previous selection is dropped - even for a list that came back empty. */
    function comboBind() {
        return get(API + '/combos' + C.qs({ branchIds: checkedBranchIds().join(',') })).then(function (d) {
            K.bindZeroIndex('cmbItem', d.items, 'Id', 'Name');                   /* :182 */
            K.bindZeroIndex('CmbFromJobLot', d.jobLotsFrom, 'Id', 'Name');       /* :183 */
            K.bindZeroIndex('CmbJobLotTo', d.jobLotsTo, 'Id', 'Name');           /* :184 */
            K.bindZeroIndex('CmbFromWareHouse', d.warehousesFrom, 'Id', 'Name'); /* :185 */
            K.bindZeroIndex('CmbToWareHouse', d.warehousesTo, 'Id', 'Name');     /* :186 */
            if (d.empty) message(checkedBranchIds().length
                ? 'No stock transfers found for the selected branches. These filters list items, warehouses and job lots from saved stock transfers.'
                : 'No stock transfers found for the current company. These filters list items, warehouses and job lots from saved stock transfers. Check the selected company.');
            else if (!branches.length) message('No stock-transfer branches are available for this report. Check the selected company and branch allocations.');
            else message('');
        });
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
        return run(function () { return loadFilters().then(function () {
            $id('gpFromDate').focus();
            if (checkedBranchIds().length) return show();
        }); });
    }
    function loadFilters() {
        var previous = checkedBranchIds();
        return get(API + '/lookups').then(function (d) {
            yearStart = d.fromDate;
            if (!initialized) {
                if (yearStart) $id('gpFromDate').value = yearStart;
                $id('gpToDate').value = d.toDate || C.today();
            }
            K.guardDate('gpFromDate'); K.guardDate('gpToDate');               /* a DateTimePicker is never empty */
            var stillAvailable = previous.filter(function (id) { return (d.branches || []).some(function (b) { return +b.Id === id; }); });
            renderBranches(d.branches, stillAvailable.length ? stillAvailable : d.defaultBranchIds);
            documentTypeFill(d.documentTypes);
            initialized = true;
            return comboBind();
        });
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
        // Also recover branches after transfers are added or the active company changes.
        return loadFilters();
    }

    // ------------------------------------------------------------------ grid

    /* GridFill:269 */
    function show() {
        var ids = checkedBranchIds();
        if (!ids.length) { $id('CmbBranchNameText').focus(); message(branches.length ? 'Select Branch First' : 'No stock-transfer branches are available for the current company. Click Refresh after stock transfers are entered.'); return Promise.resolve(); }  /* :346-347 */
        var q = C.qs({
            branchIds: ids.join(','), fromDate: val('gpFromDate'), toDate: val('gpToDate'),
            itemId: num('cmbItem'), fromWarehouseId: num('CmbFromWareHouse'), toWarehouseId: num('CmbToWareHouse'),
            jobLotId: num('CmbFromJobLot'), toJobLotId: num('CmbJobLotTo'), documentType: val('cmbDocumenttype')
        });
        return get(API + q).then(function (d) {
            dtGrid = d.raw || [];
            var rows = (d.rows || []).map(function (r, i) { return { r: r, i: i }; });
            if (!rows.length) { K.clear('grdfrm'); return; }                /* :342 */
            /* layout SortKey ColIndex 22 = Qty, ascending */
            rows.sort(function (a, b) { return (K.toNum(a.r.Qty) - K.toNum(b.r.Qty)) || (a.i - b.i); });
            K.render('grdfrm', columns(), rows.map(function (x) { return x.r; }));
        });
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

    window.RptST = { show: function () { return run(show); }, newClick: function () { return run(newClick); }, refresh: function () { return run(refresh); }, printRegister: printRegister,
        toggleBranches: toggleBranches, gotoHistory: gotoHistory };
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', load, { once: true });
    else load();
})();
