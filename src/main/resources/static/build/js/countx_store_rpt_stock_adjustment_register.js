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

    function zeros(n) { return new Array(n + 1).join('0'); }
    function val(id) { return $id(id).value; }
    function num(id) { return C.intOf(val(id)); }

    /* StockAdjustmentRegister_Load:107 */
    function load() {
        C.getJson(API + '/lookups').then(function (d) {
            bindIfRows('CmbCropYear', d.cropYears);                          /* :133 */
            fillRefreshLists(d);
            amountDec = d.amountDecimals || 0;
            rateDec = d.rateDecimals === undefined ? 2 : d.rateDecimals;
            if (d.fromDate) $id('datFromDate').value = d.fromDate;          /* :118 ActiveYr.Start_Period */
            $id('datToDate').value = d.toDate || C.today();                 /* designer default: Now */
            K.guardDate('datFromDate'); K.guardDate('datToDate');           /* a DateTimePicker is never empty */
            $id('datFromDate').focus();
        }).catch(function (e) { alert(e.message); });
    }

    /* Each bind runs only when the list has rows (if (dt.Rows.Count > 0), :131-199): an empty
       answer leaves the combo as it was. BindDDLNew / BindDDL ZeroIndex false - no default row. */
    function bindIfRows(id, rows) { if (rows && rows.length) C.fillSelect(id, rows, 'Id', 'Name'); }
    function fillRefreshLists(d) {
        bindIfRows('CmbWarehouseName', d.warehouses);                      /* :149 */
        bindIfRows('CmbJobLotName', d.jobLots);                            /* :165 */
        bindIfRows('CmbEntryType', d.entryTypes);                          /* :181 */
        bindIfRows('CmbItem', d.items);                                    /* :197 */
    }

    /* toolStripButton1_Click:491 - Item, Entry Type, Job Lot, Warehouse (Crop Year is not re-read) */
    function refresh() {
        return C.getJson(API + '/refresh').then(fillRefreshLists).catch(function (e) { alert(e.message); });
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
    document.addEventListener('DOMContentLoaded', load);
})();
