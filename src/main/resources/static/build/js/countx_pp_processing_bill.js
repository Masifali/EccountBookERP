/* ============================================================================================
 * countx_pp_processing_bill.js - 675 Processing Bill (Party Processing)
 * Desktop: Architecture.WinApp.PartyProcessing.ProductionPartyProcessingBill.cs. Exports window.PpPb.
 * Load, CmbJobOrderNo_Leave (stock party, input grid, by-product grid), grid edits (by-products, PM, OH),
 * BillAmountCalculations, Insert (server: FormValidation, row checks, MakeVoucher, DAL), ReadById, Delete, history
 * (Edit / Print / Voucher, detail grids), keys.
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/party-processing/processing-bill/';
    var P = {}; window.PpPb = P;
    var S = { rights: {}, recId: 0, removed: [], debitAccounts: [], pmItems: [], ohAccounts: [], defaultDays: 0, rateUoms: [] };
    var tabs = PPC.tabs(function (t) { HRM.focus(t === 'history' ? 'FromDateHistory' : 'datBillDate'); });
    var f3 = function (k, c, p) { return PPC.numCol(k, c, p || '#,##.###'); };

    var grdInput = new HRM.Grid('grdInput', { emptyText: '', totals: true,
        columns: [{ key: 'ItemId', hidden: true }, { key: 'ItemName', caption: 'ItemName', width: 250 }, { key: 'ItemCode', caption: 'ItemCode' },
            f3('Uom', 'Uom'), PPC.numCol('Qty', 'Qty', '#,##.###', { sum: true }), PPC.numCol('StockWeight', 'StockWeight', '#,##.###', { sum: true })] });
    var n2 = function (k, c, edit) {
        var col = PPC.numCol(k, c, '#,##0.##');
        if (edit) { col.type = 'edit-num'; delete col.render; }
        return col;
    };
    var grdByProducts = new HRM.Grid('grdByProducts', { emptyText: '', totals: true,
        columns: [PPC.btnCol('Delete', 'X', 30), { key: 'Id', hidden: true }, { key: 'ItemId', hidden: true }, { key: 'ItemCode', caption: 'ItemCode' },
            { key: 'ItemName', caption: 'ItemName', width: 170 }, { key: 'PackSize', caption: 'PackSize', type: 'num', decimals: 0 },
            n2('Qty', 'Qty'), n2('StockWeight', 'StockWeight'), n2('PurchaseQty', 'PurchaseQty', true), n2('PurchaseWeight', 'PurchaseWeight', true),
            n2('PurchaseRate', 'PurchaseRate', true), { key: 'RateUom', caption: 'RateUom', type: 'num', decimals: 0 },
            { key: 'Amount', caption: 'Amount', type: 'num', decimals: 2, sum: true },
            { key: 'DebitAccountId', caption: 'Debit Account', width: 200, type: 'select',
                options: function () { return [[0, '']].concat(S.debitAccounts.map(function (a) { return [a.Id, a.AccountTitle]; })); } },
            { key: 'Remarks', caption: 'Remarks', type: 'edit', width: 160 }],
        onChange: byProductUpdated });
    PPC.onButton(grdByProducts, 'grdByProducts', function (r, act, i) {     // grdByProducts_ColumnButtonClick
        if (act !== 'Delete') return;
        if (HRM.int(r.Id) > 0) {
            if (!HRM.ask('Are you sure to Delete?')) return;
            S.removed.push(Object.assign({}, r));
        }
        grdByProducts.remove(i);
        calc();
    });
    function byProductUpdated(r, key) {                                     // grdByProducts_CellUpdated
        if (key === 'PurchaseQty' && HRM.str(r.PurchaseQty) !== '' && HRM.str(r.PackSize) !== '') {
            if (HRM.num(r.PurchaseQty) <= HRM.num(r.Qty)) r.PurchaseWeight = HRM.num(r.PurchaseQty) * HRM.num(r.PackSize);
            else { HRM.box('Purchase Qty Can not be Greater Than Stock Qty'); r.PurchaseQty = r.Qty; r.PurchaseWeight = r.StockWeight; }
        }
        if (key === 'PurchaseRate' || key === 'RateUom' || key === 'PurchaseWeight' || key === 'PurchaseQty') {
            if (HRM.num(r.PurchaseWeight) <= HRM.num(r.StockWeight)) r.Amount = HRM.num(r.PurchaseWeight) / HRM.num(r.RateUom) * HRM.num(r.PurchaseRate);
            else { HRM.box('Purchase Weight Can not be Greater Than Stock Weight'); r.PurchaseWeight = r.StockWeight; }
        }
        grdByProducts.draw();
        calc();
    }
    function lineGrid(id, keyCol, opts, onAdd) {                            // grdPM / grdOH
        var g = new HRM.Grid(id, { emptyText: '', totals: true,
            columns: [PPC.btnCol('Delete', 'X', 30), PPC.btnCol('Add', '+', 30), keyCol,
                { key: 'Qty', caption: 'Qty', type: 'edit-num', decimals: 0 }, { key: 'Rate', caption: 'Rate', type: 'edit-num', decimals: 2 },
                { key: 'Amount', caption: 'Amount', type: 'edit-num', decimals: 0, sum: true }, { key: 'Remarks', caption: 'Remarks', type: 'edit' }],
            onChange: function (r, key) {
                if (key === 'Qty' || key === 'Rate') r.Amount = HRM.num(r.Qty) * HRM.num(r.Rate);
                g.draw(); calc();
            } });
        PPC.onButton(g, id, function (r, act, i) {
            if (act === 'Delete') { g.remove(i); if (!g.rows().length) onAdd(); }
            if (act === 'Add') onAdd();
            calc();
        });
        return g;
    }
    var grdPM = lineGrid('grdPM', { key: 'ItemId', caption: 'Item Name', width: 200, type: 'select',
        options: function () { return [[0, '']].concat(S.pmItems.map(function (x) { return [x.Id, x.ItemName]; })); } }, function () { pmRow(); });
    var grdOH = lineGrid('grdOH', { key: 'AccountId', caption: 'AccountTitle', width: 250, type: 'select',
        options: function () { return [[0, '']].concat(S.ohAccounts.map(function (x) { return [x.Id, x.AccountTitle]; })); } }, function () { ohRow(); });
    function pmRow() { grdPM.add({ ItemId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' }); }
    function ohRow() { grdOH.add({ AccountId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' }); }

    var hMain = new HRM.Grid('GrdHistoryMain', { filterRow: true, emptyText: 'No record found.',
        columns: [PPC.btnCol('Edit', 'Edit'), PPC.btnCol('Print', 'Print'), PPC.btnCol('Voucher', 'Voucher', 80), { key: 'Id', hidden: true },
            { key: 'BillDate', caption: 'BillDate', type: 'date' }, { key: 'BillNo', caption: 'BillNo', type: 'int' }, { key: 'DocumentTypeId', hidden: true },
            { key: 'InvProductionJobOrderId', hidden: true }, { key: 'JobOrderNo', caption: 'JobOrderNo' }, { key: 'STockPartyId', hidden: true },
            { key: 'StockParty', caption: 'StockParty' }, { key: 'CreditAccountId', hidden: true }, { key: 'CreditAccount', caption: 'CreditAccount' },
            PPC.numCol('Qty', 'Qty', '#,##0.###'), PPC.numCol('Weight', 'Weight', '#,##0.###'), PPC.numCol('Rate', 'Rate', '#,##0.##'),
            { key: 'RateUom', caption: 'RateUom' }, PPC.numCol('Amount', 'Amount', '#,##0'), PPC.numCol('TotalBPPurchaseAmt', 'TotalBPPurchaseAmt', '#,##0'),
            { key: 'RemarksHeader', caption: 'RemarksHeader' }, { key: 'ApprovalStatus', caption: 'ApprovalStatus' },
            { key: 'EntryDate', caption: 'EntryDate', type: 'datetime' }, { key: 'EntryUserName', caption: 'EntryUserName' },
            { key: 'ModifyDate', caption: 'ModifyDate', type: 'datetime' }, { key: 'ModifyUserName', caption: 'ModifyUserName' },
            { key: 'ApprovedDate', caption: 'ApprovedDate', type: 'datetime' }, { key: 'ApprovedUserName', caption: 'ApprovedUserName' },
            { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'int' }],
        onDouble: function (r) { readById(HRM.int(r.Id)); },
        onSelect: function (r) {                                             // BindHistoryDetail
            HRM.get(API + 'history-detail', { id: HRM.int(r.Id), jobOrderId: HRM.int(r.InvProductionJobOrderId) }).then(function (o) {
                hInput.set(o.inputs || []); hBp.set(o.outputs || []); hPm.set(o.packing || []); hOh.set(o.overheads || []);
            }).catch(HRM.fail);
        } });
    PPC.onButton(hMain, 'GrdHistoryMain', function (r, act, i, b) {        // DataGridHistory_ColumnButtonClick
        var id = HRM.int(r.Id);
        if (act === 'Edit') readById(id);
        else if (act === 'Print') report(id, b, true);
        else if (act === 'Voucher') PPC.voucher(API + 'voucher', id, b);
    });
    var hInput = new HRM.Grid('GrdHistoryInput', { emptyText: '', columns: [{ key: 'ItemId', hidden: true }, { key: 'ItemName', caption: 'ItemName' },
        { key: 'ItemCode', caption: 'ItemCode' }, f3('Uom', 'Uom'), f3('Qty', 'Qty'), f3('StockWeight', 'StockWeight')] });
    var hBp = new HRM.Grid('grdhistoryBpPurchased', { emptyText: '', columns: [{ key: 'ItemCode', caption: 'ItemCode' }, { key: 'ItemName', caption: 'ItemName' },
        PPC.numCol('Qty', 'Qty', '#,##0.##'), { key: 'UOM', caption: 'UOM' }, PPC.numCol('Weight', 'Weight', '#,##0.##'), PPC.numCol('Rate', 'Rate', '#,##0.##'),
        PPC.numCol('Amount', 'Amount', '#,##0.##'), { key: 'Remarks', caption: 'Remarks' }, { key: 'DebitAccount', caption: 'DebitAccount' }] });
    var hPm = new HRM.Grid('grdHistoryPM', { emptyText: '', columns: [{ key: 'ItemCode', caption: 'ItemCode' }, { key: 'ItemName', caption: 'ItemName' },
        PPC.numCol('Qty', 'Qty', '#,0##.##'), { key: 'Rate', caption: 'Rate' }, PPC.numCol('Amount', 'Amount', '#,0##.##'), { key: 'Remarks', caption: 'Remarks' }] });
    var hOh = new HRM.Grid('grdhistoryOH', { emptyText: '', columns: [{ key: 'Account', caption: 'Account' }, PPC.numCol('Qty', 'Qty', '#,##0.##'),
        { key: 'Rate', caption: 'Rate' }, PPC.numCol('Amount', 'Amount', '#,##0.##'), { key: 'Remarks', caption: 'Remarks' }] });

    // ------------------------------------------------------------------ load / calculations
    function histDates(days) { HRM.setVal('FromDateHistory', HRM.addDays(HRM.today(), -(days > 0 ? days : 3))); HRM.setVal('ToDateHistory', HRM.today()); }
    function rateUomFirst() { var s = PPC.$('CmbRateUom'); if (s && s.options.length) { s.selectedIndex = 0; HRM.refreshCombos(); } }
    function load() {                                                       // frmProductionJobOrder_Load
        HRM.setVal('datBillDate', HRM.today());
        return HRM.loading(HRM.get(API + 'setup').then(function (o) {
            S.rights = o.rights || {}; S.debitAccounts = o.debitAccounts || []; S.pmItems = o.pmItems || []; S.ohAccounts = o.ohAccounts || [];
            S.defaultDays = HRM.int(o.defaultDays);
            HRM.applyRights(S.rights, { save: 'btnsave', update: 'btnUpdate', print: ['BtnPrint', 'ChkBox'], delete: 'btnDelete' });
            HRM.fill('CmbCreditAc', o.creditAccounts, 'Id', 'AccountTitle', { zero: '' });
            HRM.setVal('txtBillNo', o.billNo);
            HRM.fill('CmbRateUom', o.rateUoms, 'Id', 'RateUom', { zero: false });
            rateUomFirst();
            HRM.fill('CmbJobOrderNo', o.jobOrders, 'Id', 'PlanCode', { zero: '' });
            HRM.fill('CmbStockParty', [], 'StockPartyId', 'StockParty', { zero: false });
            HRM.fill('CmbJobOrderHistory', o.historyJobOrders, 'Id', 'name', { zero: '' });
            histDates(S.defaultDays);
            grdInput.set([]); grdByProducts.set([]); grdPM.set([]); pmRow(); grdOH.set([]); ohRow();
            calc();
            HRM.focus('datBillDate');
        }).catch(HRM.fail));
    }
    function jobOrderLeave(keepParty) {                                     // CmbJobOrderNo_Leave
        var id = HRM.comboVal('CmbJobOrderNo');
        return HRM.get(API + 'job-order', { id: id }).then(function (o) {
            HRM.fill('CmbStockParty', o.stockParties, 'StockPartyId', 'StockParty', { zero: false });
            if (keepParty) HRM.setCombo('CmbStockParty', keepParty);
            grdInput.set(o.inputs || []);
            S.removed = [];
            grdByProducts.set(o.outputs || []);
            calc();
            return o;
        }).catch(HRM.fail);
    }
    function net(v) { return isFinite(v) ? v : 0; }
    function calc() {                                                       // BillAmountCalculations
        var rate = 0, rateUom = 0;
        if (HRM.comboVal('CmbRateUom') > 0) { rate = PPC.dbl('txtRate'); rateUom = HRM.num(HRM.comboText('CmbRateUom')); }
        var inW = grdInput.rows().length ? grdInput.sum('StockWeight') : 0;
        var pm = grdPM.rows().length ? grdPM.sum('Amount') : 0;
        var oh = grdOH.rows().length ? grdOH.sum('Amount') : 0;
        var bp = grdByProducts.rows().length ? grdByProducts.sum('Amount') : 0;
        var bags = inW / rateUom;
        HRM.setVal('txtNoOfBags', isFinite(bags) ? PPC.fmt(PPC.round(bags, 0), '#,##') : '');
        var proc = bags * rate;
        HRM.setVal('txtProcesssingAmount', isFinite(proc) ? PPC.fmt(PPC.round(proc, 0), '#,##') : '');
        HRM.setVal('txtPMAmount', PPC.fmt(PPC.round(pm, 0), '#,##'));
        HRM.setVal('txtOHAmount', PPC.fmt(PPC.round(oh, 0), '#,##'));
        HRM.setVal('txtBPPurchaseTotalAmt', PPC.fmt(PPC.round(bp, 0), '#,##'));
        var bill = net(proc) + pm + oh - bp;
        HRM.setVal('txtBillAmount', PPC.fmt(PPC.round(bill, 0), '#,##'));
    }

    // ------------------------------------------------------------------ save / read / delete
    function reset() {                                                      // Reset()
        S.recId = 0; S.removed = [];
        HRM.setVal('datBillDate', HRM.today()); HRM.setVal('txtRate', ''); HRM.setVal('txtRemarks', '');
        HRM.setCombo('CmbJobOrderNo', 0); rateUomFirst();
        HRM.fill('CmbStockParty', [], 'StockPartyId', 'StockParty', { zero: false });
        HRM.setVal('txtNoOfBags', ''); HRM.setVal('txtProcesssingAmount', ''); HRM.setCombo('CmbCreditAc', 0);
        HRM.show('btnUpdate', false); HRM.show('btnDelete', false); HRM.show('btnsave', true);
        grdInput.set([]); grdByProducts.set([]); grdPM.set([]); pmRow(); grdOH.set([]); ohRow();
        calc();
        HRM.focus('datBillDate');
        return HRM.get(API + 'code').then(function (c) { HRM.setVal('txtBillNo', c.billNo); }).catch(HRM.fail);
    }
    P.btnnew = function () { reset(); };
    P.btnRefresh = function (b) {                                           // btnRefresh_Click: JobOrderNoFill
        HRM.busy(b, function () { return HRM.get(API + 'refresh').then(function (o) { HRM.fill('CmbJobOrderNo', o.jobOrders, 'Id', 'PlanCode', { zero: '', keep: true }); }).catch(HRM.fail); });
    };
    function strip(id) { return String(HRM.val(id)).replace(/,/g, ''); }
    function insert(btn) {                                                  // Insert()
        var billNo = HRM.val('txtBillNo').trim(), rate = HRM.val('txtRate');
        if (billNo === '' || billNo === '0') { HRM.box('BillNo Field Required'); return; }
        if (rate === '' || rate === '0') { HRM.box('Rate Field Required'); HRM.focus('txtRate'); return; }
        if (!HRM.comboVal('CmbStockParty')) { HRM.box('StockParty Field Required'); HRM.focus('CmbStockParty'); return; }
        if (!HRM.comboVal('CmbCreditAc')) { HRM.box('CreditAccount Field Required'); HRM.focus('CmbCreditAc'); return; }
        if (!HRM.comboVal('CmbRateUom')) { HRM.box('RateUom Field Required'); HRM.focus('CmbRateUom'); return; }
        if (!HRM.comboVal('CmbJobOrderNo')) { HRM.box('JobOrder Field Required'); HRM.focus('CmbJobOrderNo'); return; }
        if (!HRM.ask(S.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var body = { id: S.recId, billNo: billNo, billDate: HRM.val('datBillDate'), jobOrderId: HRM.comboVal('CmbJobOrderNo'),
            stockPartyId: HRM.comboVal('CmbStockParty'), creditAccountId: HRM.comboVal('CmbCreditAc'), rateUomId: HRM.comboVal('CmbRateUom'),
            rateUomText: HRM.comboText('CmbRateUom'), rate: rate, noOfBags: strip('txtNoOfBags'), processingAmount: strip('txtProcesssingAmount'),
            bpTotal: strip('txtBPPurchaseTotalAmt'), remarks: HRM.val('txtRemarks'), outputs: grdByProducts.rows(), removedOutputs: S.removed,
            overheads: grdOH.rows(), packing: grdPM.rows() };
        HRM.busy(btn, function () {
            return HRM.post(API + 'save', body).then(function (r) {
                HRM.box(r.message);
                var id = HRM.int(r.id);
                return reset().then(function () { if (HRM.checked('ChkBox')) report(id, null, false); });
            }).catch(HRM.fail);
        });
    }
    P.btnsave = function (b) { S.recId = 0; insert(b); };
    P.btnUpdate = function (b) { if (!S.recId) { HRM.box('Record Not Update Because Record Not Found'); return; } insert(b); };
    P.btnDelete = function (b) {                                            // btnDelete_Click
        if (!S.recId) { HRM.box('Record Id Not Found'); return; }
        if (!HRM.ask('Are you sure to Delete?')) return;
        HRM.busy(b, function () { return HRM.post(API + 'delete?id=' + S.recId, {}).then(function (r) { HRM.box(r.message); return reset(); }).catch(HRM.fail); });
    };
    function readById(id) {                                                 // ReadById
        return HRM.loading(HRM.get(API + 'read', { id: id }).then(function (h) {
            S.recId = HRM.int(h.Id); S.removed = [];
            HRM.show('btnsave', false); HRM.show('btnUpdate', true); HRM.show('btnDelete', true);
            tabs.show('form');
            HRM.setVal('txtBillNo', h.BillNo);
            HRM.setVal('datBillDate', HRM.day(h.BillDate));
            HRM.setCombo('CmbJobOrderNo', h.InvProductionJobOrderId);
            HRM.fill('CmbStockParty', h.stockParties, 'StockPartyId', 'StockParty', { zero: false });
            HRM.setCombo('CmbStockParty', h.StockPartyId);
            grdInput.set(h.inputs || []);
            grdByProducts.set(h.outputs || []);
            HRM.setCombo('CmbCreditAc', h.CreditAccountId);
            HRM.setVal('txtRate', PPC.fmt(h.Rate, '#,##.###'));
            HRM.setComboText('CmbRateUom', String(h.RateUomId));             // the desktop sets the combo TEXT to the saved RateUomId
            HRM.setVal('txtRemarks', h.RemarksHeader);
            grdPM.set(h.packing || []); if (!grdPM.rows().length) pmRow();
            grdOH.set(h.overheads || []); if (!grdOH.rows().length) ohRow();
            calc();
        }).catch(HRM.fail));
    }
    function report(id, btn, fromHistory) {                                 // ProductionPartyProcessingBillReport_611(JobOrderId, Id)
        return HRM.busy(btn, function () {
            return HRM.get(API + 'print', { id: id, history: !!fromHistory }).then(function (g) {
                if (window.CrystalPrint) return window.CrystalPrint.open('ppc-611', { id: g.id, jobOrderId: g.jobOrderId });
            }).catch(HRM.fail);
        });
    }
    P.print = function (b) { report(S.recId, b, false); };

    // ------------------------------------------------------------------ history
    P.btnNewHistory = function () {                                        // btnNewHistory_Click
        histDates(S.defaultDays); HRM.setVal('txtFromDocNoHistory', ''); HRM.setVal('txtToDocNoHistory', ''); HRM.setCombo('CmbJobOrderHistory', 0);
        hMain.clear(); hInput.clear(); hBp.clear(); hPm.clear(); hOh.clear();
    };
    P.btnRefreshHistory = function (b) {
        HRM.busy(b, function () { return HRM.get(API + 'history-combos').then(function (o) { HRM.fill('CmbJobOrderHistory', o.historyJobOrders, 'Id', 'name', { zero: '' }); }).catch(HRM.fail); });
    };
    P.BtnShowHistory = function (b) {                                       // BindHistoryGrid
        HRM.busy(b, function () {
            return HRM.post(API + 'history', PPC.hist(null, { jobOrderId: HRM.comboVal('CmbJobOrderHistory') })).then(function (rows) {
                hMain.set(rows); hInput.clear(); hBp.clear(); hPm.clear(); hOh.clear();
            }).catch(HRM.fail);
        });
    };
    P.shortcuts = function () {
        PPC.shortcuts([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+E', 'For Close'],
            ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Alt+1', 'For Print'], ['Ctrl+T', 'For Tab Transfer']]);
    };

    // ------------------------------------------------------------------ events / keys
    PPC.$('CmbJobOrderNo').addEventListener('change', function () { jobOrderLeave(0); });
    PPC.$('txtRate').addEventListener('input', calc);
    PPC.$('CmbRateUom').addEventListener('change', calc);
    PPC.guard('txtRate', 'dec'); PPC.guard('txtFromDocNoHistory', 'int'); PPC.guard('txtToDocNoHistory', 'int');
    function onForm() { return tabs.current() === 'form'; }
    function can(id) { return HRM.visible(id) && !PPC.$(id).disabled; }
    HRM.keys({
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+t': function () { tabs.show(onForm() ? 'history' : 'form'); },
        'ctrl+s': function () { if (onForm() && can('btnsave')) P.btnsave(PPC.$('btnsave')); },
        'ctrl+shift+delete': function () { if (onForm() && can('btnDelete')) P.btnDelete(PPC.$('btnDelete')); },
        'ctrl+u': function () { if (onForm() && can('btnUpdate')) P.btnUpdate(PPC.$('btnUpdate')); },
        'ctrl+n': function () { if (onForm()) P.btnnew(); },
        'ctrl+r': function () { if (onForm()) P.btnRefresh(PPC.$('btnRefresh')); },
        'ctrl+p': function () { if (onForm()) P.print(PPC.$('BtnPrint')); },
        'alt+1': function () { if (onForm()) P.print(PPC.$('BtnPrint')); }
    });
    HRM.footer(function () { tabs.show('history'); });
    load();
})();
