/* 476 Purchase Report - Architecture.WinApp.Inventory_Reports.frmPurchaseInvoiceHistory. ":NNN" = line in frmPurchaseInvoiceHistory.cs.
   API /purchase/api/reports/invoice-history. Built on pr_common.js (window.PR). */
(function () {
    'use strict';
    var $ = PR.$, API = '/purchase/api/reports/invoice-history', K = PR.K;
    var st = { init: null, summaryRaw: [], lastType: '' };
    var from = new PR.Picker('GRNfromdate'), to = new PR.Picker('GRNToDate');
    var dueFrom = new PR.Picker('datDueFromDate', 'chkDueFrom'), dueTo = new PR.Picker('datDueToDate', 'chkDueTo');
    var branches = new PR.CheckList('cmbBranchName'), customGroups = new PR.CheckList('CmbCustomGroup');
    var grid = new PR.Grid('DataGridHistory', 'lblCount');
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    function p2(n) { return n < 10 ? '0' + n : String(n); }
    /** "dd-MMM-yyyy" */
    function dmy(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || ''));
        return m ? m[3] + '-' + MON[parseInt(m[2], 10) - 1] + '-' + m[1] : (v == null ? '' : String(v));
    }
    /** "dd-MMM-yyyy hh:mm tt" */
    function dmyt(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})[ T](\d{2}):(\d{2})/.exec(String(v || ''));
        if (!m) return dmy(v);
        var h = parseInt(m[4], 10), ap = h >= 12 ? 'PM' : 'AM'; h = h % 12 || 12;
        return m[3] + '-' + MON[parseInt(m[2], 10) - 1] + '-' + m[1] + ' ' + p2(h) + ':' + m[5] + ' ' + ap;
    }
    function nz(v) { return PR.int(v); }
    function reportType() { return PR.text('CmbReprotType'); }
    function groupBy() { return PR.text('CmbCustomerItemGroup'); }

    // ------------------------------------------------------------------ DDL

    var LISTS = ['CmbParentCategory', 'CmbItemCategory', 'CmbItemType', 'cmbItem', 'CmbWareHouse', 'CmbJobLot', 'CmbCropYear', 'CmbSupplier', 'CmbCommissionAgent'];
    /** GetDataForPurchaseInvoiceRegisterDropdownBind :265 - nine lists out of one call; Rows[0] (the first Crop Year) is activated. */
    function bindLookups(l) {
        var o = { value: 'Id', text: 'Name' }, z = { value: 'Id', text: 'Name', zeroIndex: true };
        PR.fill('CmbParentCategory', l.parentCategories, z);
        PR.fill('CmbItemCategory', l.itemCategories, z);
        PR.fill('CmbItemType', l.itemTypes, z);
        PR.fill('cmbItem', l.items, z);
        PR.fill('CmbWareHouse', l.warehouses, z);
        PR.fill('CmbJobLot', l.jobLots, z);
        PR.fill('CmbCropYear', l.cropYears, o);
        PR.fill('CmbSupplier', l.suppliers, z);
        PR.fill('CmbCommissionAgent', l.commissionAgents, z);
        LISTS.forEach(function (id) { $(id).value = ''; });
        var crop = $('CmbCropYear'); crop.value = crop.options.length > 1 ? crop.options[1].value : '';
        LISTS.forEach(function (id) { $(id).dispatchEvent(new Event('change')); });
    }
    function rebind() {
        return PR.request(API + '/lookups?' + branches.ids().map(function (i) { return 'branchIds=' + encodeURIComponent(i); }).join('&')).then(bindLookups);
    }
    /** cmbBranchName_Leave :2158 - rebind, or empty the nine combos when no branch is ticked. */
    branches.onLeave = function () {
        if (branches.ids().length) { PR.run(rebind); return; }
        LISTS.forEach(function (id) { PR.fill(id, [], {}); });
    };

    // ------------------------------------------------------------------ columns

    function num3(key, w) { return { key: key, width: w, type: 'num', fmt: 'n3', agg: 'sum' }; }
    function amt(key, w) { return { key: key, width: w, type: 'num', fmt: 'amt', agg: 'sum' }; }

    /** GridSettings :960 - desktop column order (dt :576), widths, captions, hidden ids, aggregates, Slip at position 0 and Voucher at 1. */
    function detailColumns(group) {
        return [
            { key: 'Print', caption: 'Slip', width: 40, button: 'Slip' },
            { key: 'Voucher', caption: 'Voucher', width: 60, button: 'Voucher' },
            { key: 'InvoiceId', hidden: true }, { key: 'InvoiceDetailId', hidden: true },
            { key: 'BranchName', width: K.BranchName }, { key: 'InvoiceType', width: 90 },
            { key: 'InvoiceDate', width: K.Date + 20 },
            { key: 'InvoiceNo', width: K.DocNo, open: function (r) { return nz(r.InvoiceId) > 0 ? [nz(r.DocumentTypeId), nz(r.InvoiceId)] : null; } },
            { key: 'ManualBillNo', width: 80 }, { key: 'GrnId', hidden: true },
            { key: 'GrnNo', width: K.DocNo, link: true, linkIf: function (r) { return nz(r.GrnId) > 0; } },
            { key: 'GrnDate', width: K.Date + 20 }, { key: 'OrderId', hidden: true },
            { key: 'OrderNo', width: K.DocNo, open: function (r) { return nz(r.OrderId) > 0 ? [41, nz(r.OrderId)] : null; } },
            { key: 'OrderDate', width: K.Date + 20 }, { key: 'DetailIdsCount', hidden: true }, { key: 'GpId', hidden: true },
            { key: 'GpNo', width: K.GpNo, link: true, linkIf: function (r) { return nz(r.GpId) > 0; } },
            { key: 'GpDate', width: K.Date + 20 }, { key: 'DocumentTypeId', hidden: true }, { key: 'SupplierCustomerId', hidden: true },
            { key: 'SupplierName', width: K.SupplierName + 20, link: true, linkIf: function (r) { return nz(r.SupplierCustomerId) > 0; }, hidden: group === 'SupplierName' },
            { key: 'CommissionAgent', width: K.SupplierName },
            amt('CommAmount', K.Amount), amt('CreditAdd', K.Amount), amt('DebitLess', K.Amount),
            { key: 'VehicleNo', width: K.VehicleNo }, { key: 'BiltyNo', width: K.BiltyNo },
            { key: 'ItemName', width: K.ItemName, hidden: group === 'ItemName' }, { key: 'WareHouseName', width: K.WareHouseName },
            { key: 'JobLot', width: K.JobLot }, { key: 'CropYear', width: K.CropYear }, { key: 'PackingType', width: K.PackingType },
            { key: 'PackUom', width: K.Uom },
            num3('ItemQty', K.Qty), num3('GrossWeight', K.Weight), num3('EbPerUint', K.Weight + 20), num3('EbTotal', K.Weight),
            num3('WtCut', K.Weight), num3('WcutTotal', K.Weight), num3('AddLess', K.AddLess), num3('NetBillWeight', K.Weight),
            num3('NetStockWeight', K.Weight),
            { key: 'ItemRate', width: K.Rate, type: 'num', fmt: 'rate' }, { key: 'RateUom', width: K.Uom },
            { key: 'RateCut', width: K.Rate, type: 'num', fmt: 'rate', agg: 'avg' },
            amt('RateCutAmount', K.Amount), amt('ItemAmount', K.Amount + 10), amt('FreightAmt', K.Amount),
            amt('AdvanceFreight', K.Amount), amt('ExpenseAmt', K.Amount), amt('CommissionAmt', K.Amount),
            amt('ItemNetAmount', K.Amount + 10), amt('BillAmount', K.Amount + 10),
            { key: 'EntryDate', width: K.DateTime + 10 }, { key: 'EntryUser', width: K.EntryUser + 20 },
            { key: 'VoucherHeadId', hidden: true }, { key: 'IsApproved', width: K.Status },
            { key: 'NoOfAttachments', width: K.NoOfAttachments - 10, link: true }, { key: 'TicketNos', width: 70 }
        ].map(function (c) {
            var cp = { EbPerUint: 'Empty Bag Per Unit', EbTotal: 'Empty Bag Total', WtCut: 'Weight Cut', WcutTotal: 'Weight Cut Total',
                FreightAmt: 'Freight Amount', CreditAdd: 'Freight_ChargeToParty', DebitLess: 'JournalLessFromParty' }[c.key];
            if (cp) c.caption = cp;
            return c;
        });
    }

    /** GridSettingsPaddyandRice :1164. */
    function summaryColumns(group) {
        var q = function (k) { return { key: k, width: K.Qty, type: 'num', fmt: 'n2', agg: 'sum' }; };
        var w = function (k) { return { key: k, width: K.Weight, type: 'num', fmt: 'n2', agg: 'sum' }; };
        var a = function (k) { return { key: k, width: K.Amount, type: 'num', fmt: 'amt', agg: 'sum' }; };
        var r = function (k) { return { key: k, width: K.Rate, type: 'num', fmt: 'rate' }; };
        return [
            { key: 'BranchName', width: K.BranchName }, { key: 'ItemCode', hidden: true }, { key: 'ItemName', width: K.ItemName },
            { key: 'CropYear', width: K.CropYear, hidden: group === 'CropYear' },
            { key: 'OpQty', hidden: true }, q('OpActualQty'), { key: 'OpWeight', hidden: true }, { key: 'OpAmount', hidden: true },
            a('OpItemExpAmt'), { key: 'OpExp', hidden: true }, { key: 'OpRate', hidden: true },
            q('CurrQty'), q('CurrAcutalQty'), w('CurrWeight'), a('CurrAmount'), a('CurrItemExpAmt'), a('CurrExp'), r('CurrRate'),
            q('TotalQty'), q('ClActualQty'), w('TotalWeight'), a('Amount'), a('ClItemExpAmt'), a('Expense'), r('Rate')
        ];
    }

    var AVG_CAP = { AvgRateWithExp: 'Avg Rate With Expense', AmountWithExp: 'Amount With Expense', MiscExp: 'Miscellaneous Expense',
        MiscExp40Kg: 'Miscellaneous Expense 40Kg', MisExp: 'Miscellaneous Expense', MisExp40Kg: 'Miscellaneous Expense 40Kg',
        FreightExp: 'Freight Expense', FreightExp40Kg: 'Freight Expense 40Kg', CommExp: 'Commission Expense',
        CommExp40Kg: 'Commission Expense 40Kg', Expense40Kg: 'Expense 40Kg' };
    function cap(c) { if (AVG_CAP[c.key]) c.caption = AVG_CAP[c.key]; return c; }

    /** GridAvgRateByItemSettings :1272. */
    function avgItemColumns() {
        var a = function (k, w) { return { key: k, width: w || K.Amount, type: 'num', fmt: 'amt', agg: 'sum' }; };
        return [
            { key: 'BranchName', width: K.BranchName }, { key: 'InvoiceDate', hidden: true }, { key: 'ItemId', hidden: true },
            { key: 'ItemName', width: K.ItemName + 20 }, { key: 'ItemType', hidden: true }, { key: 'CropYear', width: K.CropYear },
            { key: 'ItemQty', width: K.Qty, type: 'num', fmt: 'n2', agg: 'sum' },
            num3('NetBillWeight', K.Weight), { key: 'WeightByItem', hidden: true }, { key: 'AvgRate', width: K.Rate, type: 'num', fmt: 'rate' },
            { key: 'TotPurPrcnt', hidden: true }, a('Amount', K.Amount + 10), a('TotalExpense', K.Amount + 10),
            { key: 'AvgRateWithExp', width: K.Rate, type: 'num', fmt: 'rate' }, a('AmountWithExp', K.Amount + 30),
            a('MiscExp'), a('MiscExp40Kg'), a('FreightExp'), a('FreightExp40Kg'), a('CommExp', K.Amount + 10), a('CommExp40Kg'), a('Expense40Kg')
        ].map(cap);
    }

    /** GridAvgRateBySupplierSettings :1375. */
    function avgSupplierColumns() {
        var a = function (k, w) { return { key: k, width: w || K.Amount, type: 'num', fmt: 'amt', agg: 'sum' }; };
        return [
            { key: 'BranchName', width: K.BranchName }, { key: 'SupplierCustomerId', hidden: true },
            { key: 'SupplierName', width: K.SupplierName }, { key: 'ItemId', hidden: true }, { key: 'ItemName', width: K.ItemName },
            { key: 'CropYear', width: K.CropYear }, { key: 'PackUom', width: K.Uom },
            { key: 'ItemQty', width: K.Qty, type: 'num', fmt: 'n2', agg: 'sum' }, { key: 'PackSizeQty', width: K.Qty + 20, type: 'num', fmt: 'n2', agg: 'sum' },
            { key: 'NetBillWeight', width: K.Weight, type: 'num', fmt: 'n2', agg: 'sum' },
            { key: 'WeightByItem', width: K.Weight, type: 'num', fmt: 'n2', agg: 'sum' },
            { key: 'AvgRate', width: K.Rate, type: 'num', fmt: 'rate' }, { key: 'TotPurPercent', width: K.Qty, type: 'num', fmt: 'n2' },
            a('Amount', K.Amount + 10), a('TotalExpense', K.Amount + 10), { key: 'AvgRateWithExp', width: K.Amount, type: 'num', fmt: 'rate' },
            a('AmountWithExp', K.Amount + 25), a('MisExp'), a('MisExp40Kg'), a('FreightExp'), a('FreightExp40Kg'),
            a('CommExp', K.Amount + 20), a('CommExp40Kg'), a('Expense40Kg')
        ].map(cap);
    }

    // ------------------------------------------------------------------ filters / show

    function filter(extra) {
        var f = {
            branchIds: branches.ids(), fromDate: from.value(), toDate: to.value(),
            fromNo: PR.val('txtDocNoFrm'), toNo: PR.val('txtDocNoTo'),
            supplierId: PR.val('CmbSupplier'), itemId: PR.val('cmbItem'), commissionAgentId: PR.val('CmbCommissionAgent'),
            jobLotId: PR.val('CmbJobLot'), itemCategoryId: PR.val('CmbItemCategory'), itemTypeId: PR.val('CmbItemType'),
            warehouseId: PR.val('CmbWareHouse'), parentCategoryId: PR.val('CmbParentCategory'), cropYear: PR.text('CmbCropYear'),
            rateFrom: $('txtratefrom').value.trim(), rateTo: $('txtrateto').value.trim(),
            dueFrom: dueFrom.value(), dueTo: dueTo.value(), customGroupIds: customGroups.ids(),
            reportType: reportType(), groupBy: groupBy()
        };
        for (var k in (extra || {})) f[k] = extra[k];
        return f;
    }

    function prep(rows) {
        return rows.map(function (r) {
            var c = {}; for (var k in r) c[k] = r[k];
            ['InvoiceDate', 'GrnDate', 'OrderDate', 'GpDate'].forEach(function (k) { if (k in c) c[k] = dmy(c[k]); });
            if ('EntryDate' in c) c.EntryDate = dmyt(c.EntryDate);
            return c;
        });
    }

    /** btnshow_Click :1576. */
    function show() {
        var type = reportType();
        if (!type) { PR.box('Please Select Report Type'); return Promise.resolve(); }
        if (!branches.ids().length) { branches.el.querySelector('summary').focus(); return Promise.reject(new Error('Select Branch First')); }
        return PR.request(API + '/rows', filter()).then(function (res) {
            var rows = res.rows || [];
            st.lastType = type;
            if (type === 'Current & Total Purchases') st.summaryRaw = res.raw || [];
            if (!rows.length) { grid.clear(); return; }
            var g = groupBy();
            if (type === 'PurchsaeDetail') {
                var key = g === 'GroupByItem' ? 'ItemName' : g === 'GroupBySupplier' ? 'SupplierName' : null;
                grid.show(detailColumns(key), prep(rows), { groupBy: key, onLink: link, onButton: button });
            } else if (type === 'Current & Total Purchases') {
                var gk = g === 'GroupItem&CropYear' ? 'CropYear' : null;
                grid.show(summaryColumns(gk), rows, { groupBy: gk });
            } else if (type === 'AvgRateByItem') grid.show(avgItemColumns(), prep(rows), {});
            else if (type === 'AvgRateBySupplier') grid.show(avgSupplierColumns(), rows, {});
        });
    }

    // ------------------------------------------------------------------ grid buttons and links

    /** DataGridHistory_ColumnButtonClick :1870. */
    function button(col, r) {
        if (col === 'Voucher') PR.run(function () {
            return PR.printTemplate('103-AcRptPurchaseSalesVoucherSlip.rpt', { id: nz(r.VoucherHeadId), documentTypeId: nz(r.DocumentTypeId) });
        });
        if (col === 'Print') PR.run(function () { return PR.printTemplate('220A-InvRptPurchaseBillSupplierRiceSlip.rpt', { id: nz(r.InvoiceId) }); });
    }
    /** DataGridHistory_LinkClicked :1914. */
    function link(col, r) {
        if (col === 'SupplierName') openLedger(nz(r.SupplierCustomerId));
        if (col === 'GrnNo' && nz(r.GrnId) > 0) PR.run(function () {
            return PR.printReport('/reports/print/211-goods-receipts-notes-rice-slip', { id: nz(r.GrnId), documentTypeId: 46 });
        });
        if (col === 'GpNo' && nz(r.GpId) > 0) PR.run(function () { return PR.printTemplate('251-InvRptInwardGatePassSlip.rpt', { id: nz(r.GpId) }); });
        if (col === 'NoOfAttachments') attachments(nz(r.InvoiceId), nz(r.DocumentTypeId));
    }
    /** CommonServices.GoToCustomerLedgerFromLinkedEvent(SupplierCustomerId, GRNfromdate, GRNToDate). */
    function openLedger(id) {
        PR.run(function () {
            return PR.request(API + '/gl-account?supplierCustomerId=' + encodeURIComponent(id)).then(function (r) {
                var q = new URLSearchParams(); q.set('accountId', r.accountId);
                if (from.date()) q.set('fromDate', from.date()); if (to.date()) q.set('toDate', to.date());
                window.open('/accounts/reports/general-ledger?' + q.toString(), '_blank');
            });
        });
    }
    /** CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId). */
    function attachments(id, docType) {
        var q = '?id=' + encodeURIComponent(id) + '&documentTypeId=' + encodeURIComponent(docType);
        PR.run(function () {
            return PR.request(API + '/attachments' + q).then(function (rows) {
                if (!rows || !rows.length) return;
                return PR.dialog('Attachments', '<table class="pr-grid"><thead><tr><th style="width:220px">AttachmentName</th><th style="width:220px">CustomName</th><th style="width:130px">EntryDate</th></tr></thead><tbody>'
                    + rows.map(function (a) { return '<tr><td><a href="' + API + '/attachments/' + PR.esc(a.Id) + q + '">' + PR.esc(a.AttachmentName) + '</a></td><td>' + PR.esc(a.CustomName) + '</td><td>' + PR.esc(dmyt(a.EntryDate)) + '</td></tr>'; }).join('')
                    + '</tbody></table>');
            });
        });
    }

    // ------------------------------------------------------------------ prints (toolstrip)

    /** The data the desktop re-queries for a print, handed to the .rpt through the grid print. */
    function printFrom(kind, rpt, failText) {
        if (!branches.ids().length && kind !== 'register') { PR.box(failText || 'Select Branch First'); return; }
        PR.run(function () {
            return PR.request(API + '/print-data', filter({ kind: kind })).then(function (d) {
                return PR.printGrid(rpt, rpt.replace(/\.rpt$/, ''), d.raw);
            }).catch(function (e) { if (failText) { throw new Error(failText); } throw e; });
        });
    }
    function printSummary(rpt) {
        if (!st.summaryRaw.length) { PR.box('Not Record Found For Display'); return; }
        PR.run(function () { return PR.printGrid(rpt, rpt.replace(/\.rpt$/, ''), st.summaryRaw); });
    }

    /** The Print drop-down lists the .rpt files of the desktop's "Purchase" report folder (ReportsLoad :246); only the one named in the source can be offered. */
    var DROPDOWN = ['224-InvRptPurchaseBillRegister'];
    function loadDropdown() {
        $('tsDropDownMenu').innerHTML = DROPDOWN.map(function (n) { return '<button type="button" data-rpt="' + PR.esc(n) + '">' + PR.esc(n) + '</button>'; }).join('');
    }
    $('tsDropDownMenu').addEventListener('click', function (e) {
        var b = e.target.closest('button[data-rpt]'); if (!b) return;
        $('tsDropDown').open = false;
        printFrom('register', b.dataset.rpt + '.rpt');
    });

    /** CmbReprotType_ValueChanged :1966. */
    function typeChanged() {
        var id = PR.val('CmbReprotType'), v = function (el, on) { $(el).hidden = !on; };
        if (id === 1 || id === 2) {
            v('tsDropDown', id === 2); v('btnSummeryFormat1', id === 1); v('btn232SummeryFormat2', id === 1);
            v('label7', true); v('boxGroup', true); v('print', false); v('btnPrintSummery', false);
        } else {
            v('print', id === 3); v('btnPrintSummery', id !== 3);
            v('label7', false); v('boxGroup', false); v('tsDropDown', false); v('btnSummeryFormat1', false); v('btn232SummeryFormat2', false);
        }
        $('label7').style.display = $('label7').hidden ? 'none' : ''; $('boxGroup').style.display = $('boxGroup').hidden ? 'none' : '';
    }

    /** Reset :488. */
    function Reset() {
        $('txtDocNoFrm').value = ''; $('txtDocNoTo').value = '';
        ['cmbItem', 'CmbSupplier', 'CmbCustomerItemGroup'].forEach(function (id) { $(id).value = ''; $(id).dispatchEvent(new Event('change')); });
        $('cmbDateType').value = '2'; $('cmbDateType').dispatchEvent(new Event('change'));
        $('cmbDateType').focus();
    }
    function refresh() { return rebind(); }

    function shortcuts() {
        PR.shortcuts([['Ctrl+S', 'For Show Record'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Combo Date'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
            ['Ctrl+ArrowDown', 'For Focus On Grid History'], ['Ctrl+ArrowUp', 'For Focus On Date combo in Filters Box']]);
    }

    /** frmPurchaseInvoiceHistory_Load :184. */
    function load() {
        return PR.request(API + '/init').then(function (d) {
            st.init = d;
            PR.cfg.amountDecimals = d.amountDecimals; PR.cfg.rateDecimals = d.rateDecimals;
            from.set(d.now); to.set(d.now);
            loadDropdown();
            bindLookups(d.lookups);
            PR.fill('CmbReprotType', d.reportTypes, { value: 'Id', text: 'ReportName' });
            $('CmbReprotType').value = '2';                                                  // Rows[1] -> PurchsaeDetail
            customGroups.set((d.customGroups || []).map(function (g) { return { id: g.Id, name: g.Name }; }), []);
            PR.fill('CmbCustomerItemGroup', d.groups, { value: 'Id', text: 'GroupName' });
            $('CmbCustomerItemGroup').value = '3';                                           // Rows[2] -> GroupItem&CropYear
            PR.fill('cmbDateType', d.dateTypes, { value: 'Id', text: 'Parameters', zeroIndex: true });
            $('cmbDateType').value = '2';                                                    // Rows[2] -> This Week
            PR.applyDateType(2, from, to, d.yearStart);
            branches.set((d.branches || []).map(function (b) { return { id: b.BranchId, name: b.BranchName }; }), d.branchId ? [d.branchId] : []);
            ['CmbReprotType', 'CmbCustomerItemGroup', 'cmbDateType'].forEach(function (id) { $(id).dispatchEvent(new Event('change')); });
            typeChanged();
            $('cmbDateType').focus();
        });
    }

    $('cmbDateType').addEventListener('change', function () { PR.applyDateType($('cmbDateType').value, from, to, st.init ? st.init.yearStart : ''); });
    $('CmbReprotType').addEventListener('change', typeChanged);
    $('btnshow').addEventListener('click', function () { PR.run(show); });
    $('btnNew').addEventListener('click', Reset);
    $('btnRefrsh').addEventListener('click', function () { PR.run(refresh); });
    $('print').addEventListener('click', function () { printFrom('avgItem', '221-InvRptPurchaseInvoicePurchaseAvgRateByItem.rpt', null); });
    $('btnPrintSummery').addEventListener('click', function () { printFrom('avgSupplier', '222-InvRptPurchaseInvoicePurchaseAvgRateByItemSupplier.rpt', 'No Record Found'); });
    $('btnSummeryFormat1').addEventListener('click', function () { printSummary('232-SpInventoryTransactions_PurchasesingRicePaddy_NewRpt_Format-I.rpt'); });
    $('btn232SummeryFormat2').addEventListener('click', function () { printSummary('232-SpInventoryTransactions_PurchasesingRicePaddy_NewRpt_Format-II.rpt'); });
    $('btn297PurchaseBill').addEventListener('click', function () { printFrom('register', '297-InvRptPurchase BillActivity.rpt', null); });
    PR.digitsOnly(['txtDocNoFrm', 'txtDocNoTo']);
    ['txtratefrom', 'txtrateto'].forEach(function (id) {   // CommonServices.OnlytextdecimelFunction
        var el = $(id);
        el.addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[0-9.]/.test(e.key)) e.preventDefault(); if (e.key === '.' && el.value.indexOf('.') >= 0) e.preventDefault(); });
        el.addEventListener('input', function () { var v = el.value.replace(/[^0-9.]/g, ''); if (v !== el.value) el.value = v; });
    });
    PR.gridTools(grid, 'btnGridPrint', 'btnGridExport', function () { return 'Purchase Report'; });
    PR.fullscreen('btnFullscreen', 'gridSection');
    PR.enterAsTab();
    /** frmPurchaseInvoiceHistory_KeyDown :1479. */
    document.addEventListener('keydown', function (e) {
        var k = e.key.toLowerCase();
        if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { if (!document.querySelector('dialog[open]')) { e.preventDefault(); location.href = '/purchase/reports'; } return; }
        if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
        if (!e.ctrlKey) return;
        var a = { s: function () { PR.run(show); }, r: function () { PR.run(refresh); }, n: Reset,
            p: function () { if (!$('print').hidden) $('print').click(); else if (!$('btnPrintSummery').hidden) $('btnPrintSummery').click(); },
            f5: function () { $('cmbDateType').focus(); }, arrowup: function () { $('cmbDateType').focus(); },
            arrowdown: function () { $('gridWrap').focus(); } }[k];
        if (a) { e.preventDefault(); a(); }
    });
    PR.run(load);
})();
