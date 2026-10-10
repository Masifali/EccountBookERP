/* ============================================================================================
 * Screen 451 StockTranfserRegister - Architecture.WinApp.pcc.Reports.StockTranfserRegister (.cs, 1559 lines)
 * Page: templates/sale/reports/pcc_stock_transfer_register.html (route /sale/reports/pcc/stock-transfer-register).
 *   frmGatePassReport_Load :214   ComboBind, Datetypefill, cmbperemeter.Focus()
 *   Datetypefill :199             CommonServices.DateType, BindDDL(ZeroIndex true), Rows[2].Activate() ("This Week", From = today - 7)
 *   ComboBind :112 / btnRefreshh_Click :392   InvStockTransferHeader.AllComboAgainstStockTransfer1 = [pcc].[USP_GetDataForDropDownFromStockTransfer], split by Activity:
 *                                 Item, ItemType, ItemCategory, ParentCategory, VehicleNo (Id / ReferenceName); a list is bound only when it has rows (ZeroIndex false)
 *   GridFill :231 / btnSearch_Click :320   InvStockTransferHeader.StockTransferRegister = [pcc].[USP_InvStockTransferSlipRegister] DocumentTypeId 1860, ApprovedFilter "All";
 *                                 rows copied to Id, DocumentTypeId, DocNo, DocDate (short date), ItemName, "VehicleNo.", ItemQty, ItemWeight, NetWeight, ItemRate, ItemAmount;
 *                                 no rows -> ClearStructure
 *   grdfrmSetting :285            Id and DocumentTypeId hidden; ItemWeight / NetWeight "#,##0.##", ItemQty "#,#", ItemAmount stringFormatsingle (all summed), ItemRate
 *                                 DecimalRateFormate (average); GridAutoAdjustmentNew
 *   toolStripButton1_Click :350   "Not Record Found For Display" or 1860-StockTransferManual_Register.rpt over the last Show's table
 *   btnRefresh_Click (New) :373   clears Item, Item Type, Item Category, Parent Category, Vehicle No, Doc From / To, focus Parent Category
 *   KeyDown :416, MakeShortCutKeys :467, cmbperemeter_ValueChanged :505, btnshow_Leave :566
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, el = A.el;
    var API = '/api/sale/salepcc/stock-transfer-register';
    var lookup = { amountDecimals: 0, rateDecimals: 0, yearStart: null };
    var lastArgs = null, hasRows = false, seq = 0;
    var SHORTCUTS = [['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
        ['Ctrl+F5', 'For Focus On DateType '], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
        ['Ctrl+ArrowUp', 'For Focus On Supplier Customer in Filter']];

    var grid = new S.Grid({ tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', headerLines: 1, frozen: 0, autosize: true });
    (function averageFooter() {                                              // AggregateFunction 3 (average) of a column total
        var rr = grid.render;
        grid.render = function () {
            rr.call(this);
            if (!this.columns) return;
            var g = this, cols = this.visibleCols(), rows = this.visibleRows();
            var tr = A.el(this.cfg.tableId).tFoot && A.el(this.cfg.tableId).tFoot.querySelector('tr.tot');
            if (!tr) return;
            cols.forEach(function (c, i) {
                if (!c.avg || !tr.cells[i]) return;
                var s = 0; rows.forEach(function (r) { s += A.toDouble(A.ci(r, c.key)); });
                tr.cells[i].className = 'num tt'; tr.cells[i].textContent = rows.length ? S.fmtNum(s / rows.length, c.totalFmt || c.fmt) : '';
            });
        };
    }());

    function columns() {
        var single = S.fmtSingle(lookup.amountDecimals), rate = S.fmtRate(lookup.rateDecimals);
        return [
            { key: 'Id', caption: 'Id', hidden: true }, { key: 'DocumentTypeId', caption: 'DocumentTypeId', hidden: true },
            { key: 'DocNo', caption: 'DocNo' }, { key: 'DocDate', caption: 'DocDate', date: 'dd/MM/yyyy' }, { key: 'ItemName', caption: 'ItemName' },
            { key: 'VehicleNo.', caption: 'VehicleNo.' },
            { key: 'ItemQty', caption: 'ItemQty', fmt: '#,#', totalFmt: '#,#', sum: true, num: true },
            { key: 'ItemWeight', caption: 'ItemWeight', fmt: '#,##0.##', totalFmt: '#,##0.##', sum: true, num: true },
            { key: 'NetWeight', caption: 'NetWeight', fmt: '#,##0.##', totalFmt: '#,##0.##', sum: true, num: true },
            { key: 'ItemRate', caption: 'ItemRate', fmt: rate, totalFmt: rate, avg: true, num: true },
            { key: 'ItemAmount', caption: 'ItemAmount', fmt: single, totalFmt: single, sum: true, num: true }
        ];
    }
    function mapRows(rows) {                                                 // the dtlst copy of GridFill
        return rows.map(function (r) {
            return { Id: A.toInt(A.ci(r, 'Id')), DocumentTypeId: A.toInt(A.ci(r, 'DocumentTypeId')), DocNo: A.toInt(A.ci(r, 'DocNo')), DocDate: A.ci(r, 'DocDate'),
                     ItemName: A.ci(r, 'ItemName'), 'VehicleNo.': A.ci(r, 'VehicleNo'), ItemQty: A.toDouble(A.ci(r, 'ItemQty')), ItemWeight: A.toDouble(A.ci(r, 'ItemWeight')),
                     NetWeight: A.toDouble(A.ci(r, 'NetWeight')), ItemRate: A.toDouble(A.ci(r, 'ItemRate')), ItemAmount: A.toDouble(A.ci(r, 'ItemAmount')) };
        });
    }

    // ------------------------------------------------------------------ lists
    function bindCombos(data) {                                              // ComboBind: a list is bound only when it has rows
        var c = (data && data.combos) || {};
        if (!Object.keys(c).length) return;
        function bind(id, key) { var rows = c[key] || []; if (rows.length) S.fill(el(id), rows, false); }
        bind('cmbItem', 'Item'); bind('cmbitemType', 'ItemType'); bind('cmbitemcatgory', 'ItemCategory');
        bind('cmbparentcatgory', 'ParentCategory'); bind('cmbvehicleno', 'VehicleNo');
    }
    function datetypefill() { S.fill(el('cmbperemeter'), S.dateTypeRows(false), true); S.activate('cmbperemeter', 2, true); }
    el('cmbperemeter').addEventListener('change', function () {              // cmbperemeter_ValueChanged
        S.dateRule(A.toInt(this.value), el('gpFromDate'), el('gpToDate'), lookup.yearStart);
    });

    // ------------------------------------------------------------------ Show / print
    function args() {
        var vehicle = S.selInt('cmbvehicleno');
        return { fromDate: el('gpFromDate').value, toDate: el('gpToDate').value, fromDocNo: A.toIntText(el('txtfromdoc').value), toDocNo: A.toIntText(el('txttodoc').value),
                 itemId: S.selInt('cmbItem'), itemTypeId: S.selInt('cmbitemType'), itemCategoryId: S.selInt('cmbitemcatgory'),
                 vehicleNo: vehicle ? String(vehicle) : '' };             // Conversion.ToString(cmbvehicleno.Value): the combo value
    }
    function show() {                                                        // btnSearch_Click -> GridFill
        var b = el('show'); if (b.disabled) return Promise.resolve();
        var a = args(), token = ++seq;
        A.busy(b, true);
        return A.getJson(API + '/rows?' + S.qs(a)).then(function (rows) {
            if (token !== seq) return;
            lastArgs = a; hasRows = !!(rows && rows.length);
            if (!hasRows) { grid.clear(); return; }                          // ClearStructure
            grid.setData(columns(), mapRows(rows));
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }
    function print() {                                                       // toolStripButton1_Click
        var b = el('print'); if (b.disabled) return;
        if (!hasRows || !lastArgs) { alert('Not Record Found For Display'); return; }
        A.busy(b, true);
        A.openPdf('/sale/reports/pcc/print/stock-transfer-register?' + S.qs(lastArgs)).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function reset() {                                                       // btnRefresh_Click ("New")
        ['cmbItem', 'cmbitemType', 'cmbitemcatgory', 'cmbparentcatgory', 'cmbvehicleno'].forEach(function (id) {
            var s = el(id); s.value = ''; s.dispatchEvent(new Event('change', { bubbles: true }));
        });
        el('txtfromdoc').value = ''; el('txttodoc').value = '';
        S.focus('cmbparentcatgory');
    }
    function refresh() {                                                     // btnRefreshh_Click -> ComboBind
        var b = el('refresh'); if (b.disabled) return;
        A.busy(b, true);
        A.getJson(API + '/lookups').then(function (data) { lookup = data || lookup; bindCombos(data); }).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function closeForm() { w.location.href = '/sale/reports'; }

    S.digitsOnly('txtfromdoc'); S.digitsOnly('txttodoc');
    el('show').addEventListener('click', show);
    el('reset').addEventListener('click', reset);
    el('refresh').addEventListener('click', refresh);
    el('print').addEventListener('click', print);
    el('shortcuts').addEventListener('click', function () { A.shortcuts(SHORTCUTS); });
    S.closeShortcuts();
    el('show').addEventListener('blur', function (e) { if (e.relatedTarget && el('grid').contains(e.relatedTarget)) S.focus('cmbperemeter'); });   // btnshow_Leave
    S.keys({ s: show, e: closeForm, r: refresh, n: reset, p: print, F5: function () { S.focus('cmbperemeter'); }, ArrowUp: function () { S.focus('cmbperemeter'); },
             ArrowDown: function () { grid.focus(); }, alt: function () { A.shortcuts(SHORTCUTS); } }, closeForm);

    // ------------------------------------------------------------------ start (Load)
    el('gpFromDate').value = A.today(); el('gpToDate').value = A.today();
    grid.render();
    datetypefill();
    A.getJson(API + '/lookups').then(function (data) {
        lookup = data || lookup;
        bindCombos(data);                                                    // ComboBind
        S.activate('cmbperemeter', 2, true);                                 // the year start is known now: re-apply the date type rule
        S.focus('cmbperemeter');
    }).catch(function (e) { alert(e.message); });
}(window, document));
