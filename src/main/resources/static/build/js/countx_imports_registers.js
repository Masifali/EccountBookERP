/* ============================================================================================
 * countx_imports_registers.js - the four Import registers (Architecture.WinApp.ImportReports):
 *   392 invoice  ImportInvoiceRegister   807-ImInvoiceRegister_WithAvgRates.rpt
 *   393 contract ImportContractRegister  808-ImportContractRegister.rpt
 *   394 grn      ImportGrnRegister       811-ImportGrnRegister.rpt
 *   395 purchase ImportPurchaseRegister  810-ImportPurchaseOrder.rpt
 * <body data-kind / data-key / data-rpt> choose the register. Show fills the grid with the procedure's own columns
 * (RetrieveStructure) and the form's GridSettings; Print prints the same data. Exports window.ImpARg.
 * ============================================================================================ */
(function () {
    'use strict';

    var P = {};
    window.ImpARg = P;
    var KIND = document.body.getAttribute('data-kind');
    var KEY = document.body.getAttribute('data-key');
    var API = '/api/import/reports/' + KIND;
    var dtgrid = [], lastArgs = null, grid = null;

    /** GridSettings per form: hidden columns, formats and the aggregate of each formatted column ('sum' / 'avg'). */
    var F = '#,#.##';
    var SETTINGS = {
        invoice: { group: 'SupplierName', cols: { NoofBags: [F, 'sum'], Mton: ['#,#.###', 'sum', '#,#.##'], NetWeight: [F, 'sum'], RatePrice: [F, 'avg'],
            FcAmount: [F, 'sum'], AddLessAmount: [F, 'sum'], Expenses: [F, 'sum'], NoOfContainers: [F, 'sum'], ConversionRate: [F, 'avg'],
            GrandAmount: [F, 'sum'], AvgRate: [F, null] } },
        contract: { hidden: ['id', 'Address1', 'Address2', 'ApprovedUserName', 'CommodityDetial', 'CompanyEmailExport', 'CompanyPhoneNo', 'ConversionRate',
            'CustomerCode', 'DeliveryTermId', 'DestinationPortName', 'EquivalentAmount', 'ExImProformaInvoice', 'ExpiryDate', 'ExpiryPlace', 'ExporterBankName',
            'InquiryReference', 'inrEquivalent', 'inrUomCode', 'InsepctionDescription', 'InsepctionRequired', 'IsApproved', 'ItemOtherDescription',
            'LegalizationRequired', 'LoadingPortName', 'LotReference', 'ModifyDate', 'ModifyUserName', 'NoOfShipments', 'NotifyPartyName', 'OtrEquivalent',
            'PartialShipment', 'PostDate', 'InnerQty', 'ProductSpecification', 'ProductionSchDaysBefore', 'QuotReference', 'rtuomEquivalent', 'SalesContractRef',
            'SalesContratDate', 'SalesPersonName', 'SampleSchDaysBefore', 'ShipmentIntervalDays', 'ShipmentStartDate', 'ShippingMarks', 'TransShipment'],
            captions: { CustomerName: 'Supplier Name' },
            cols: { FCurrencyAmount: [F, 'sum'], GrossWeightKgs: [F, 'sum'], NetWeightKgs: [F, 'sum'], RatePrice: [F, 'avg'], OuterQty: [F, 'sum'],
                NoOfContainers: [F, 'sum'], ItemAmount: [F, 'sum'] } },
        grn: { hidden: ['id', 'Status'], cols: { FreightAmt: [F, 'sum'], OtherCharges: [F, 'sum'], Qty: [F, 'sum'], GrossWeight: [F, 'sum'], NetWeight: [F, 'sum'],
            AdLsWeight: [F, 'sum'], StockWeight: [F, 'sum'], POGrossweight: [F, 'sum'], NetWeight1: [F, 'sum'], NoOfContainer: [F, 'sum'] } },
        purchase: { cols: {} }
    };
    var SET = SETTINGS[KIND] || { cols: {} };

    function isDateText(v) { return typeof v === 'string' && /^\d{4}-\d{2}-\d{2}([T ]\d{2}:\d{2})/.test(v); }

    /** RetrieveStructure + GridSettings. */
    function bind(rows) {
        var keys = rows.length ? Object.keys(rows[0]) : [];
        if (SET.group && keys.indexOf(SET.group) > 0) { keys.splice(keys.indexOf(SET.group), 1); keys.unshift(SET.group); }   // RootTable.Groups.Add
        var hidden = {};
        (SET.hidden || []).forEach(function (h) { hidden[h.toLowerCase()] = true; });
        var foot = {};
        var cols = keys.map(function (k) {
            var cfg = null;
            Object.keys(SET.cols).forEach(function (c) { if (c.toLowerCase() === k.toLowerCase()) cfg = SET.cols[c]; });
            var caption = (SET.captions && SET.captions[k]) || k;
            if (hidden[k.toLowerCase()]) return { key: k, hidden: true };
            if (cfg) {
                if (cfg[1]) foot[k] = (function (pat, agg) { return function (rs) { return ImpA.fmt(agg === 'avg' ? ImpA.avgOf(rs, k) : ImpA.sumOf(rs, k), pat); }; })(cfg[2] || cfg[0], cfg[1]);
                return { key: k, caption: caption, type: 'num', align: 'right', sum: !!cfg[1], render: (function (pat) { return function (v) { return HRM.esc(ImpA.fmt(v, pat)); }; })(cfg[0]) };
            }
            var sample = null;
            rows.some(function (r) { if (r[k] !== null && r[k] !== undefined && r[k] !== '') { sample = r[k]; return true; } return false; });
            if (isDateText(sample)) return { key: k, caption: caption, type: 'datetime' };
            if (typeof sample === 'number') return { key: k, caption: caption, align: 'right' };
            return { key: k, caption: caption };
        });
        var t = document.getElementById('DataGridHistory');
        var nt = t.cloneNode(false);                                  // a fresh table: the old grid's listeners go with the old node
        t.parentNode.replaceChild(nt, t);
        grid = new HRM.Grid('DataGridHistory', { columns: cols, filterRow: true, totals: true, emptyText: '',
            onDraw: function (g) { ImpA.footer(g, foot); } });
        grid.set(rows);
    }

    function args() {
        var d = new Date(), pad = function (n) { return String(n).padStart(2, '0'); };
        var time = ' ' + pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds());
        return { fromDate: HRM.val('GRNfromdate'), toDate: HRM.val('GRNToDate') + time,
            supplierId: HRM.comboVal('CmbSupplier'), itemId: HRM.comboVal('cmbitem') };
    }

    /** btnshow_Click -> gridHisory(). */
    P.btnshow = function (b) {
        var a = args();
        return HRM.busy(b, function () {
            return HRM.get(API + '/show', { from: HRM.val('GRNfromdate'), to: HRM.val('GRNToDate'), supplierId: a.supplierId, itemId: a.itemId }).then(function (rows) {
                dtgrid = rows || []; lastArgs = a;
                if (dtgrid.length) bind(dtgrid);
                else { document.getElementById('DataGridHistory').innerHTML = ''; grid = null; }
                HRM.text('lblStatus', dtgrid.length + ' record(s)');
            }).catch(HRM.fail);
        });
    };

    /** print_Click: the rows of the last Show, else "Record Not Found For Dispaly". */
    P.print = function (b) {
        if (!dtgrid.length || !lastArgs) { HRM.box('Record Not Found For Dispaly'); return; }
        return ImpA.checkedPrint(b, API + '/print-check', null, KEY, lastArgs);
    };

    /** frmSaleInvoiceRegister_Load. */
    function load() {
        HRM.footer(null);
        HRM.setVal('GRNToDate', HRM.today());
        return HRM.loading(HRM.get(API + '/setup')).then(function (d) {
            HRM.fill('CmbSupplier', d.suppliers, 'Id', 'CompanyName', { zero: '' });
            HRM.fill('cmbitem', d.items, 'Id', 'ItemName', { zero: '' });
            HRM.setVal('GRNfromdate', d.fromDate || HRM.today());
        }).catch(HRM.fail);
    }

    HRM.keys({ 'ctrl+e': HRM.close, 'esc': HRM.close });
    document.addEventListener('DOMContentLoaded', load);
})();
