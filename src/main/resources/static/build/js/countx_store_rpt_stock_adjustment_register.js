/* ============================================================================================
 * 454 "Stock Adjustment Report" - StockAdjustmentRegister.cs (ScreenName StockAdjustmentRegister)
 * API /api/store/reports/stock-adjustment-register. Rules and notes: StoreReportsBService.
 * ============================================================================================ */
(function () {
    'use strict';

    var C = window.StoreCommon, K = window.StoreRptB, $id = C.$id;
    var API = '/api/store/reports/stock-adjustment-register';
    var grnData = [];              /* GrnData - what the last Show fetched; 410-Register prints it */
    var amountDec = 0, rateDec = 2;
    var initialized = false, pending = false;

    function zeros(n) { return new Array(n + 1).join('0'); }
    function val(id) { return $id(id).value; }
    function num(id) { return C.intOf(val(id)); }

    /* StockAdjustmentRegister_Load:107 */
    function load() {
        return C.getJson(API + '/lookups').then(function (d) {
            bindIfRows('CmbCropYear', d.cropYears);                          /* :133 */
            fillRefreshLists(d);
            amountDec = d.amountDecimals || 0;
            rateDec = d.rateDecimals === undefined ? 2 : d.rateDecimals;
            if (d.fromDate) $id('datFromDate').value = d.fromDate;          /* :118 ActiveYr.Start_Period */
            $id('datToDate').value = d.toDate || C.today();                 /* designer default: Now */
            K.guardDate('datFromDate'); K.guardDate('datToDate');           /* a DateTimePicker is never empty */
            $id('datFromDate').focus();
            initialized = true;
        });
    }

    /* BindDDLNew / BindDDL ZeroIndex false. Clear a failed/empty source on refresh so old
       options cannot look like a successful reload. */
    /* ItemBind:197 - DDL.BindDDL keeps every column of Sp_Item_GetAllMethod 'ReadAllItems' visible except Id,
       with column 1 captioned "Item Name". */
    var ITEM_COLS = ['ItemCode', 'ItemCodeNew', 'InventoryParentCategoriesId', 'ItemCategoryId', 'ItemTypeId', 'ItemQcGradeId',
        'ItemQcGrade', 'MaxMeaurementUnitId', 'WeightKgs', 'MaxMeaurement', 'PurchaseGLAC', 'SaleGLAC', 'COGSGLAC',
        'CategoryDescription', 'TypeDescription'];
    function itemColumns(rows) {
        if (!rows || !rows.length) return;
        var el = $id('CmbItem'), byId = {};
        rows.forEach(function (r) { byId[String(r.Id)] = r; });
        Array.prototype.forEach.call(el.options, function (o) {
            var r = byId[o.value];
            if (r) o.setAttribute('data-extra', ITEM_COLS.map(function (k) { return String(r[k] == null ? '' : r[k]).replace(/\|/g, '/'); }).join('|'));
        });
        el.setAttribute('data-columns', ['Item Name'].concat(ITEM_COLS).join('|'));
    }
    function bindIfRows(id, rows) { C.fillSelect(id, rows || [], 'Id', 'Name'); }
    function fillRefreshLists(d) {
        bindIfRows('CmbWarehouseName', d.warehouses);                      /* :149 */
        bindIfRows('CmbJobLotName', d.jobLots);                            /* :165 */
        bindIfRows('CmbEntryType', d.entryTypes);                          /* :181 */
        bindIfRows('CmbItem', d.items);                                    /* :197 */
        itemColumns(d.items);
        var errors = d.lookupErrors || {};
        if (errors.entryTypes) {
            $id('CmbEntryType').innerHTML = '<option value="0">Entry Type unavailable</option>';
            $id('CmbEntryType').value = '0';
        }
        C.lookupNotice(Object.keys(errors).map(function (key) { return errors[key]; }).join(' '), true);
    }

    /* toolStripButton1_Click:491 - Item, Entry Type, Job Lot, Warehouse (Crop Year is not re-read) */
    function refresh() {
        if (pending) return Promise.resolve();
        pending = true;
        C.lookupNotice('Loading filters...');
        var states = Array.from(document.querySelectorAll('.rb-root button,.rb-root input,.rb-root select'))
            .map(function (el) { return { el: el, disabled: el.disabled }; });
        states.forEach(function (s) { s.el.disabled = true; });
        return Promise.resolve().then(function () {
            return initialized ? C.getJson(API + '/refresh').then(fillRefreshLists) : load();
        }).catch(function (e) { C.lookupNotice(e.message || 'Could not load the filters. Click Refresh to try again.', true); })
            .finally(function () { states.forEach(function (s) { s.el.disabled = s.disabled; }); pending = false; });
    }

    /* Reset:207 - the five combos' text only; dates, grid and GrnData stay */
    function reset() {
        ['CmbCropYear', 'CmbWarehouseName', 'CmbJobLotName', 'CmbEntryType', 'CmbItem'].forEach(function (id) { $id(id).value = '0'; });
        $id('datFromDate').focus();
    }

    /* gridHisory:224 */
    function show() {
        var q = C.qs({
            fromDate: val('datFromDate'), toDate: val('datToDate'),
            itemId: num('CmbItem'), cropYearId: num('CmbCropYear'), jobLotId: num('CmbJobLotName'),
            warehouseId: num('CmbWarehouseName'), entryTypeId: num('CmbEntryType')
        });
        return C.getJson(API + q).then(function (d) {
            grnData = d.rows || [];
            if (!grnData.length) { K.clear('DataGridHistory'); return; }    /* :278 ClearStructure */
            K.render('DataGridHistory', columns(d.columns), grnData, { frozen: 1 });
        }).catch(function (e) { alert(e.message); });
    }

    /* RetrieveStructure + GridSetting:287 */
    function columns(keys) {
        var hidden = { Id: 1, DocumentTypeId: 1, CompLogoImage: 1 };            /* :291-293 */
        var q = function (v) { return K.fmt(v, '#,##0.###'); }, qt = function (v) { return K.fmt(v, '#,##0.##'); };
        var amt = '#,##0.' + zeros(amountDec), rate = '#,#0.' + zeros(rateDec);
        var special = {
            Qty:       { align: 'right', agg: 'sum', format: q, total: qt },          /* :295-298 */
            NetWeight: { align: 'right', agg: 'sum', format: q, total: qt },          /* :299-302 */
            Amount:    { align: 'right', agg: 'sum', format: function (v) { return K.fmt(v, amt); }, total: function (v) { return K.fmt(v, amt); } },
            ItemRate:  { align: 'right', agg: 'avg', format: function (v) { return K.fmt(v, rate); }, total: function (v) { return K.fmt(v, rate); } },
            NoOfAttachments: { link: function () { alert('The attachment viewer is not available on the web.'); } }  /* :294 link column */
        };
        var dates = { DocDate: 1, EnteryDate: 1, ModifyDate: 1 };
        var cols = [{ caption: 'Print', button: 'Print', onButton: printSlip }];   /* :311-318, position 0 */
        keys.forEach(function (k) {
            if (hidden[k]) return;
            var c = { key: k, caption: K.caption(k) };
            if (dates[k]) c.format = function (v) { return K.general(v); };
            var s = special[k];
            if (s) Object.keys(s).forEach(function (p) { c[p] = s[p]; });
            cols.push(c);
        });
        return cols;
    }

    /* DataGridHistory_ColumnButtonClick:445 -> GenerateReport:384 (409-InvStockAdjustmentSlip.rpt) */
    function printSlip(row) {
        return C.getJson(API + '/' + C.intOf(C.ci(row, 'Id')) + '/slip').then(function (rows) {
            if (!K.printRows(rows, '409-InvStockAdjustmentSlip')) alert('Not Record Found For Display');
        }).catch(function (e) { alert(e.message); });
    }

    /* btn334Register_Click:420 (410-InvStockAdjustmentRegister.rpt) */
    function printRegister() {
        if (!grnData.length) { alert('Not Record Found For Display'); return; }
        K.printRows(grnData, '410-InvStockAdjustmentRegister');
    }

    /* Footer History button (web): the history grid is below the filters, as on the desktop. */
    function gotoHistory() { $id('historySection').scrollIntoView({ behavior: 'smooth', block: 'start' }); }

    window.RptSA = { show: show, reset: reset, refresh: refresh, printRegister: printRegister, gotoHistory: gotoHistory };
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', refresh);
    else refresh();
})();
