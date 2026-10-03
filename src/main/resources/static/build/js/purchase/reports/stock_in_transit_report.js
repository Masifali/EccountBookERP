/* 869 Stock In Transit Report - Architecture.WinApp.SupplierPortal.Reports.frmSupplierDispatchPreBillReport.
   ":NNN" = line in frmSupplierDispatchPreBillReport.cs. API /purchase/api/reports/stock-in-transit. Built on pr_common.js. */
(function () {
    'use strict';
    var $ = PR.$, API = '/purchase/api/reports/stock-in-transit', SCREEN = 869, K = PR.K;
    var st = { init: null, dtGrid: [], activity: '' };
    var from = new PR.Picker('txtDateFrom', 'chkDateFrom'), to = new PR.Picker('txtToDate', 'chkToDate');
    var branches = new PR.CheckList('cmbBranchName'), parents = new PR.CheckList('CmbParentCategory');
    var grid = new PR.Grid('DataGridHistory', 'lblCount');

    /** AllComboBind :263 - USP_GetDataForDropDownFromSupplierDispatch (251) by Activity; parent categories all ticked. */
    function bindLookups(rows) {
        var by = function (a) { return PR.byActivity(rows, a); };
        var pc = by('ParentCategories').map(function (r) { return { id: r.Id, name: r.ReferenceName }; });
        parents.set(pc, pc.map(function (p) { return p.id; }));
        [['cmbItem', 'Item'], ['cmbSupplierCustomer', 'Supplier'], ['CmbCommissionAgent', 'CommissionAgent'], ['CmbRefernceParty', 'ReferencePartyName'],
            ['CmbVehicleNo', 'VehicleNo'], ['CmbCity', 'City']].forEach(function (p) { PR.fill(p[0], by(p[1]), {}); });
    }
    function AllComboBind() { return PR.request(API + '/lookups?branchIds=' + encodeURIComponent(branches.ids().join(','))).then(bindLookups); }
    /** cmbBranchName_Leave :413. */
    branches.onLeave = function () {
        if (branches.ids().length) { PR.run(AllComboBind); return; }
        parents.set([], []);
        ['cmbItem', 'cmbSupplierCustomer', 'CmbCommissionAgent', 'CmbRefernceParty', 'CmbVehicleNo', 'CmbCity'].forEach(function (id) { PR.fill(id, [], {}); });
    };

    /** DataGridHistorySetting :655 - dtcol order (:546), hidden columns, ConfigureNumericalColumn (:632), per-report visibility. */
    function columns(type) {
        var DEC = ['TotalQty', 'SupplierWbWeight', 'TotalAmount', 'FcyAmount', 'ExchangeRate', 'Freight', 'OtherExpense', 'AdvanceFreight', 'CommAmount',
            'Qty', 'NetWeight', 'Rate', 'Amount', 'LedgerAmount', 'LedgerIncludingBillAmount'];
        var W = { BranchName: K.BranchName, DocNo: K.DocNo, BranchSrNo: K.DocNo, DocDate: K.Date, VehicleNo: K.VehicleNo, BiltyNo: K.BiltyNo, DepartureDate: K.Date,
            TransitDays: K.DueDays, ETAdestination: K.Date + 10, TotalQty: K.Qty, SupplierWbWeight: K.Weight, TotalAmount: K.Amount, FcyAmount: K.Amount,
            Currency: K.Currency, ExchangeRate: K.Rate, Freight: K.Freight, OtherExpense: K.Freight, AdvanceFreight: K.Freight, CityName: K.CityName,
            Remarks: K.Remarks, EntryUser: K.EntryUser, EntryDate: K.Date, ModifyUser: K.EntryUser, ModifyDate: K.Date, ApprovedUser: K.EntryUser,
            ApprovedDate: K.Date, ApprovalStatus: K.Status, SupplierName: K.SupplierName, OrderNo: K.DocNo, OrderDate: K.Date, CommissionAgentName: K.String,
            CommAmount: K.Amount, ReferencePartyName: K.String, ItemCode: K.ItemCode, ItemName: K.ItemName, CropYear: K.CropYear, PackingType: K.PackingType,
            PackUom: K.Uom, Qty: K.Qty, NetWeight: K.Weight, Rate: K.Rate, RateUom: K.Uom, Amount: K.Amount, NoOfAttachments: K.NoOfAttachments,
            TotalVehicles: K.VehicleNo, EnRouteVehicles: K.VehicleNo, PreviouslyReachedVehicles: K.VehicleNo, TodayReachedVehicle: K.VehicleNo,
            ExpectedTodayReachedVehicle: K.VehicleNo + 40, LedgerAmount: 100, LedgerIncludingBillAmount: 100 };
        var KEYS = ['Id', 'DocumentTypeId', 'DocumentType', 'BranchName', 'DocNo', 'BranchSrNo', 'DocDate', 'VehicleNo', 'BiltyNo', 'DepartureDate', 'TransitDays',
            'ETAdestination', 'TotalQty', 'SupplierWbWeight', 'TotalAmount', 'FcyAmount', 'Currency', 'ExchangeRate', 'Freight', 'OtherExpense', 'AdvanceFreight',
            'CityName', 'Remarks', 'EntryUser', 'EntryDate', 'ModifyUser', 'ModifyDate', 'ApprovedUser', 'ApprovedDate', 'ApprovalStatus', 'DetailId', 'SupplierId',
            'SupplierGLId', 'SupplierName', 'OrderId', 'OrderNo', 'OrderDate', 'CommissionAgentId', 'CommissionAgentName', 'CommAmount', 'ReferencePartyId',
            'ReferencePartyName', 'ItemId', 'ItemCode', 'ItemName', 'CropYear', 'PackingType', 'PackUom', 'Qty', 'NetWeight', 'Rate', 'RateUom', 'Amount',
            'NoOfAttachments', 'TotalVehicles', 'EnRouteVehicles', 'PreviouslyReachedVehicles', 'TodayReachedVehicle', 'ExpectedTodayReachedVehicle',
            'LedgerAmount', 'LedgerIncludingBillAmount'];
        var hidden = ['Id', 'DocumentTypeId', 'OrderId', 'DetailId', 'SupplierId', 'SupplierGLId', 'ReferencePartyId', 'CommissionAgentId', 'ItemId', 'FcyAmount',
            'Currency', 'ExchangeRate', 'TotalVehicles', 'EnRouteVehicles', 'PreviouslyReachedVehicles', 'TodayReachedVehicle', 'ExpectedTodayReachedVehicle',
            'LedgerAmount', 'LedgerIncludingBillAmount'];
        if (type === 'Customized Detail Report') hidden = hidden.concat(['DocumentType', 'DepartureDate', 'TransitDays', 'ETAdestination', 'TotalQty', 'SupplierWbWeight',
            'TotalAmount', 'Freight', 'OtherExpense', 'CityName', 'Remarks', 'EntryUser', 'EntryDate', 'ModifyUser', 'ModifyDate', 'ApprovedUser', 'ApprovedDate',
            'ApprovalStatus', 'OrderNo', 'OrderDate', 'CommissionAgentName', 'ReferencePartyName', 'CropYear', 'PackingType', 'PackUom', 'RateUom', 'NoOfAttachments']);
        else if (type === 'DocWise Report') hidden = hidden.concat(['DocumentType', 'OrderNo', 'OrderDate', 'ReferencePartyName', 'CropYear', 'PackingType', 'PackUom',
            'RateUom', 'NoOfAttachments']);
        var cols = KEYS.map(function (k) {
            var c = { key: k, width: W[k] || 90, hidden: hidden.indexOf(k) >= 0 };
            if (k === 'ETAdestination') c.caption = 'ETA Destination';
            if (k === 'Amount') c.caption = 'Bill Amount';
            // the order number opens its purchase order (the stock-in-transit page itself has no ?id= entry yet)
            if (k === 'OrderNo') c.open = function (r) { return PR.int(r.OrderId) > 0 ? [41, PR.int(r.OrderId)] : null; };
            if (k === 'EntryDate' || k === 'ModifyDate' || k === 'ApprovedDate') { c.type = 'datetime'; c.width = K.DateTime; }
            if (k === 'DocDate' || k === 'DepartureDate' || k === 'OrderDate') c.type = 'short';
            if (DEC.indexOf(k) >= 0) {
                c.type = 'num';
                if (/Amount/.test(k)) { c.fmt = 'amt'; c.agg = 'sum'; } else if (/Rate/.test(k)) c.fmt = 'rate'; else { c.fmt = 'n2'; c.agg = 'sum'; }
            }
            return c;
        });
        if (type === 'Summary By Party') {
            var show = ['SupplierName', 'TotalVehicles', 'EnRouteVehicles', 'PreviouslyReachedVehicles', 'TodayReachedVehicle', 'ExpectedTodayReachedVehicle',
                'Qty', 'NetWeight', 'Amount', 'LedgerAmount', 'LedgerIncludingBillAmount'];
            cols.forEach(function (c) { c.hidden = show.indexOf(c.key) < 0; if (c.key === 'SupplierName') c.link = true; });
            cols.sort(function (a, b) { var x = show.indexOf(a.key), y = show.indexOf(b.key); return (x < 0 ? 99 : x) - (y < 0 ? 99 : y); });
        }
        if (type === 'Detail Report' || type === 'Customized Detail Report' || type === 'DocWise Report')
            cols.unshift({ key: 'Print', caption: 'Print', width: 40, button: 'Print' });
        return cols;
    }

    /** GridFill :471. */
    function GridFill() {
        if (!branches.ids().length) return Promise.reject(new Error('Select Branch First'));
        var type = PR.text('cmbActivity');
        if (!type) return Promise.reject(new Error('Select Report Type First'));
        var receiving = document.querySelector('input[name=receiving]:checked').value;
        if (receiving !== 'All' && !to.checked()) return Promise.reject(new Error('Please Select ToDate For Filtering Pending Or Received Records'));
        return PR.request(API + '/rows', {
            branchIds: branches.ids(), reportType: type, fromDate: from.value(), toDate: to.value(),
            fromNo: PR.val('txtFromDocNo'), toNo: PR.val('txtToDocNo'), orderNoFrom: PR.val('txtOrderNoFrom'), orderNoTo: PR.val('txtOrderNoTo'),
            parentCategoryIds: parents.ids(), itemId: PR.val('cmbItem'), supplierId: PR.val('cmbSupplierCustomer'),
            commissionAgentId: PR.val('CmbCommissionAgent'), referencePartyId: PR.val('CmbRefernceParty'), driverName: '',
            vehicleNo: PR.text('CmbVehicleNo'), cityId: PR.val('CmbCity'), receiving: receiving
        }).then(function (res) {
            st.dtGrid = res.raw || []; st.activity = type;
            if (!(res.rows || []).length) { grid.clear(); return; }
            grid.show(columns(type), res.rows, {
                onButton: function (col, r) {           // DataGridHistory_ColumnButtonClick :878 -> SupplierDispatchPreBillSlip251
                    if (col === 'Print') PR.run(function () { return PR.printTemplate('251_SupplierDispatchPreBillSlip.rpt', { id: PR.int(r.Id) }); });
                },
                onLink: function (col, r) {             // DataGridHistory_LinkClicked :899
                    if (col === 'SupplierName') PR.openLedger(SCREEN, PR.int(r.SupplierId), from.date(), to.date());
                } });
        });
    }
    /** BtnPrintDropDown_DropDownItemClicked :943 - dtGrid into the chosen .rpt of the SupplierDispatchedPreBill_* folder. */
    function print() {
        if (!PR.text('cmbActivity')) { PR.box('Select Report Type First'); return; }
        if (!st.dtGrid.length) { PR.box('Record Not Found... '); return; }
        PR.run(function () { return PR.printGrid('', 'Supplier Dispatched Report - ' + st.activity, st.dtGrid); });
    }
    /** btnNew_Click :1031. */
    function reset() {
        $('txtFromDocNo').value = ''; $('txtToDocNo').value = '';
        parents.clear();
        ['cmbItem', 'cmbSupplierCustomer', 'CmbCommissionAgent', 'CmbRefernceParty', 'CmbVehicleNo', 'CmbCity'].forEach(function (id) { $(id).value = ''; });
        $('cmbActivity').value = '1';
    }
    /** btnRefresh_Click :1072 - BranchesFill (keeps the ticked branches when the user's is not allocated) + AllComboBind. */
    function refresh() {
        return PR.request(API + '/init').then(function (d) {
            var keep = branches.ids();
            branches.set((d.branches || []).map(function (b) { return { id: b.BranchId, name: b.BranchName }; }), d.branchId ? [d.branchId] : keep);
            return AllComboBind();
        });
    }
    function shortcuts() {
        PR.shortcuts([['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Alt + 1', 'For print 312-SalesRegisterSummary'], ['Alt + 2', 'For print 314-SalesRegisterSummaryByItem'],
            ['Alt + 3', 'For print 316-SalesRegisterSummaryByItemWithoutPacking'], ['Alt + 4', 'For print 317-SalesRegisterSummaryByWarehouse'],
            ['Alt + 5', 'For print 313-SalesRegisterSummaryByCustomer'], ['Alt + 6', 'For print 315-SalesRegisterSummaryByCustomer&Item'],
            ['Ctrl+F5', 'For Focus On DateType '], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
            ['Ctrl+ArrowUp', 'For Focus On DateType in Filter'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    /** frmSupplierDispatchPreBillReport_Load :183. */
    function load() {
        return PR.request(API + '/init').then(function (d) {
            st.init = d; PR.cfg.amountDecimals = d.amountDecimals; PR.cfg.rateDecimals = d.rateDecimals;
            from.set(d.now);
            branches.set((d.branches || []).map(function (b) { return { id: b.BranchId, name: b.BranchName }; }), d.branchId ? [d.branchId] : []);
            bindLookups(d.lookups);
            PR.fill('cmbActivity', d.activities.map(function (a, i) { return { Id: i + 1, Name: a }; }), { text: 'Name' });
            $('cmbActivity').value = '1';                   // Rows[0] "Detail Report"
            to.set(d.maxEta); to.setChecked(true);          // GetMaxETA_DateForPreBill -> txtToDate
            from.setChecked(false);
            var q = new URLSearchParams(location.search);   // a caller's party / item and dates
            if (q.get('supplierId') || q.get('itemId')) {
                if (q.get('supplierId')) $('cmbSupplierCustomer').value = q.get('supplierId');
                if (q.get('itemId')) $('cmbItem').value = q.get('itemId');
                if (q.get('fromDate')) from.set(q.get('fromDate'));
                if (q.get('toDate')) to.set(q.get('toDate'));
                return GridFill();
            }
        });
    }

    $('btnShow').addEventListener('click', function () { PR.run(GridFill); });
    $('btnNew').addEventListener('click', reset);
    $('btnRefresh').addEventListener('click', function () { PR.run(refresh); });
    $('BtnPrintDropDown').addEventListener('click', print);
    $('btnShortcut').addEventListener('click', shortcuts);
    PR.digitsOnly(['txtFromDocNo', 'txtToDocNo', 'txtOrderNoFrom', 'txtOrderNoTo']);
    PR.gridTools(grid, 'btnGridPrint', 'btnGridExport', function () { return 'Supplier Dispatched Report - ' + (st.activity || ''); });
    PR.fullscreen('btnFullscreen', 'gridSection');
    PR.enterAsTab();
    /** frmSupplierDispatchPreBillReport_KeyDown :1148 (Ctrl+F5 / Ctrl+Up focus the hidden Date Type combo: nothing visible happens). */
    document.addEventListener('keydown', function (e) {
        var k = e.key.toLowerCase();
        if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { if (!document.querySelector('dialog[open]')) { e.preventDefault(); location.href = '/purchase/reports'; } return; }
        if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
        if (!e.ctrlKey) return;
        var a = { s: function () { PR.run(GridFill); }, n: reset, r: function () { PR.run(refresh); }, arrowdown: function () { $('gridWrap').focus(); } }[k];
        if (a) { e.preventDefault(); a(); }
    });
    PR.run(load);
})();
