/* ============================================================================================
 * countx_logistics_freight_voucher_export.js - 965 Freight Voucher (Export)
 * Architecture.WinApp.Service.lgstcm.frmFreightVoucherExport (DocumentTypeId 1305) + frmLoadPendingGpForFreightVoucher.
 * Gate pass / delivery order pick, transporter -> pending logistic PO, the freight calculations (CalculateQtyForRate,
 * CalculateBiltyFright, CalculateNetPaid), the credit-entry group with account balance and cheques, the add / less
 * expense grid, New / Save / Update / Delete, prints 1305 and the 102 voucher slip, History. Exposes window.LgsFV.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/logistics/freight-voucher-export';
    var P = {};
    window.LgsFV = P;

    var RecId = 0, UpdateMode = false, rights = {}, cfg = {}, lists = {}, removed = [], drRecord = [];
    var otherItems = [], updateDetailIndex = -1, grdPay, grdExp, grdHistory, grdHistoryDetail;

    function zeros(n) { return new Array(n + 1).join('0'); }
    function amtFmt() { var n = HRM.int(cfg.amountDecimals); return n > 0 ? '#,##0.' + zeros(n) : '#,##0'; }
    function rateFmt() { var n = HRM.int(cfg.rateDecimals); return n > 0 ? '#,##0.' + zeros(n) : '#,##0'; }
    function amt() { return HRM.int(cfg.amountDecimals); }
    function num(id) { return HRM.num(HRM.val(id)); }
    function d(v) { return v ? HRM.fmtDate(v) : ''; }

    // ------------------------------------------------------------------------ combos
    /** GpAndDoBind: one pending row gives a gate-pass row and a delivery-order row. */
    function GpAndDoBind(rows) {
        var gp = [], dor = [];
        (rows || []).forEach(function (r) {
            var c = function (k) { return HRM.col(r, k); };
            gp.push({ Id: c('GpId'), GpNo: c('GpSrNo'), GpDate: c('GpDate'), GpDateText: d(c('GpDate')), VehicleType: c('VehicleType'), VehicleNo: c('VehicleNo'),
                BiltyNo: c('BiltyNo'), BiltyDate: c('BiltyDate'), BiltyDateText: d(c('BiltyDate')), WbNetWeight: c('WbNetWeight'),
                InDateTime: c('InDateTime'), OutDateTime: c('OutDateTime'), InText: HRM.fmtDateTime(c('InDateTime')), OutText: HRM.fmtDateTime(c('OutDateTime')), DoId: c('DeliveryOrderId') });
            dor.push({ Id: c('DeliveryOrderId'), DoNo: c('DeliveryOrderNo'), DoDate: c('DeliveryOrderDate'), DoDateText: d(c('DeliveryOrderDate')), GpId: c('GpId'),
                DoQty: c('DoQty'), DoWeight: c('DoWeight') });
        });
        LgsB.fillCols('CmbGpNo', gp, 'Id', 'GpNo', ['GpDateText', 'VehicleType', 'VehicleNo', 'BiltyNo', 'BiltyDateText', 'WbNetWeight', 'InText', 'OutText'],
            ['GpNo', 'GpDate', 'VehicleType', 'VehicleNo', 'BiltyNo', 'BiltyDate', 'WbNetWeight', 'InDateTime', 'OutDateTime'], { zero: '' });
        LgsB.fillCols('CmbDeliveryOrderNo', dor, 'Id', 'DoNo', ['DoDateText', 'DoQty', 'DoWeight'], ['DoNo', 'DoDate', 'DoQty', 'DoWeight'], { zero: '' });
    }
    /** PurchaseOrderNoBind (CmbTransporter_Leave). */
    function CmbTransporter_Leave() {
        return HRM.get(API + '/pending-po', { partyId: HRM.comboVal('CmbTransporter'), recId: RecId }).then(function (rows) {
            var po = (rows || []).map(function (r) {
                var c = function (k) { return HRM.col(r, k); };
                return { Id: c('Id'), DocNo: c('DocNo'), DocDate: c('DocDate'), DocDateText: d(c('DocDate')), RateBaseUomId: c('RateBaseUomId'), Rate: c('Rate'), Qty: c('Qty') };
            });
            LgsB.fillCols('CmbLogisticPoNo', po, 'Id', 'DocNo', ['DocDateText', 'Rate', 'Qty'], ['DocNo', 'DocDate', 'Rate', 'Qty'], { zero: '' });
            if (po.length) CmbLogisticPoNo_Leave();
        }).catch(HRM.fail);
    }
    function bindLists(d0) {
        lists = d0; cfg = d0.config || cfg;
        if (d0.pendingGp) GpAndDoBind(d0.pendingGp);
        LgsB.fillCols('CmbTransporter', d0.suppliers || [], 'Id', 'CompanyName', ['PartyCode', 'CityName', 'CurrencyCode'], ['CompanyName', 'PartyCode', 'CityName', 'CurrencyCode'], { zero: '' });
        var cities = d0.cities || [];
        HRM.fill('CmbLoadingCity', cities, 'Id', 'Description', { zero: '', keep: true });
        HRM.fill('CmbUnLoadingCity', cities, 'Id', 'Description', { zero: '', keep: true });
        var uom = [], bw = [];
        (d0.lookups || []).forEach(function (r) {
            var a = HRM.str(HRM.col(r, 'Activity')), o = { Id: HRM.int(HRM.col(r, 'Id')), Name: HRM.str(HRM.col(r, 'ReferenceName')) };
            if (a === 'BillWeightBase') bw.push(o);
            else if (a === 'RateBaseUom') { o.Equivalent = HRM.num(HRM.col(r, 'OtherReference')); uom.push(o); }
        });
        LgsB.fillCols('CmbRateUom', uom, 'Id', 'Name', ['Equivalent'], ['Name', 'Equivalent'], { zero: '' });
        HRM.fill('CmbBillWeightBase', bw, 'Id', 'Name', { zero: '', keep: true });
        LgsB.fillCols('CmbChargesToDrAccount', d0.chargesAccounts || [], 'Id', 'AccountTitle', ['AccountCode'], ['AccountTitle', 'AccountCode'], { zero: '' });
        HRM.fill('CmbTransactionType', [{ Id: 1, Name: 'Cash' }, { Id: 2, Name: 'Bank' }, { Id: 3, Name: 'Other' }], 'Id', 'Name', { zero: '', keep: true });
        HRM.fill('CmbInstrumentType', d0.instrumentTypes || [], 'Id', 'InstrumentType', { zero: '', keep: true });
        otherItems = d0.otherCharges || [];
        BillToCrAccountBind();
        if (HRM.int(cfg.chargesToAccount) > 0) HRM.setCombo('CmbChargesToDrAccount', cfg.chargesToAccount);   // GetConfigurationsFromGlobalandBind
        var book = !!cfg.chequeBookEnabled;
        HRM.show('wrapCmbChequeNo', book); HRM.show('wrapTxtChequeNo', !book);
    }
    /** BillToCrAccountBindFromGlobal: 1 Cash (type 2), 2 Bank (15), 3 Other (3/6/8). */
    function BillToCrAccountBind() {
        var t = HRM.comboVal('CmbTransactionType');
        if (!t) {
            LgsB.fillCols('CmbAccountTitle', [], 'Id', 'AccountTitle', [], null, { zero: '', keep: false });
            LgsB.fillCols('CmbChequeNo', [], 'Id', 'CheqNo', [], null, { zero: '', keep: false });
            return;
        }
        var rows = t === 1 ? lists.cashAccounts : t === 2 ? lists.bankAccounts : lists.otherAccounts;
        LgsB.fillCols('CmbAccountTitle', rows || [], 'Id', 'AccountTitle', ['AccountCode', 'AccountClass', 'AccountType', 'ParentAccountTitle'],
            ['AccountTitle', 'AccountCode', 'AccountClass', 'AccountType', 'ParentAccountTitle'], { zero: '' });
    }
    function CmbTransactionType_Leave() {
        HRM.enable('CmbAccountTitle', true);
        if ((lists.instrumentTypes || []).length) HRM.setCombo('CmbInstrumentType', HRM.comboVal('CmbTransactionType') === 2 ? 1 : 3);
        BillToCrAccountBind();
        CalculateNetPaid();
    }
    function CmbAccountTitle_Leave() {
        AccountCurrentBalance();
        CheqNoFill();
        CalculateNetPaid();
    }
    /** AccountCurrentBalance: "#,#;(#,#);0" + Dr / Cr / Nill. */
    function AccountCurrentBalance() {
        var id = HRM.comboVal('CmbAccountTitle');
        return HRM.get(API + '/balance', { accountId: id }).then(function (b) {
            if (b && b.found) {
                var v = HRM.num(b.Balance);
                HRM.text('BalanceValue', LgsB.net(v, '#,#;(#,#);0') + (v > 0 ? ' Dr' : v < 0 ? ' Cr' : ' Nill'));
            } else HRM.text('BalanceValue', '0');
        }).catch(HRM.fail);
    }
    /** CheqNoFill: outstanding cheques of the bank when the cheque book is enabled. */
    function CheqNoFill() {
        if (HRM.comboVal('CmbTransactionType') === 2 && cfg.chequeBookEnabled) {
            return HRM.get(API + '/cheques', { bankId: HRM.comboVal('CmbAccountTitle'), recId: RecId }).then(function (rows) {
                LgsB.fillCols('CmbChequeNo', (rows || []).map(function (r) { return { Id: HRM.col(r, 'Id'), CheqNo: HRM.col(r, 'CheqNo') }; }), 'Id', 'CheqNo', [], null, { zero: '', keep: false });
            }).catch(HRM.fail);
        }
        LgsB.fillCols('CmbChequeNo', [], 'Id', 'CheqNo', [], null, { zero: '', keep: false });
        return Promise.resolve();
    }
    /** CmbGpNo_Leave: the gate pass fills its DO, dates, vehicle, bilty and WB weight (and the doc date while saving new). */
    function CmbGpNo_Leave() {
        var r = LgsB.row('CmbGpNo'); if (!HRM.comboVal('CmbGpNo') || !r) return;
        var doId = HRM.int(r.DoId);
        if (doId > 0) {
            if (HRM.comboVal('CmbDeliveryOrderNo') !== doId) { HRM.setCombo('CmbDeliveryOrderNo', doId); CmbDeliveryOrderNo_Leave(); }
            HRM.setVal('txtGpDate', HRM.day(r.GpDate));
            HRM.setVal('txtVehicleNo', HRM.str(r.VehicleNo));
            HRM.setVal('txtBiltyNo', HRM.str(r.BiltyNo));
            HRM.setVal('txtBiltyDate', r.BiltyDate ? HRM.day(r.BiltyDate) : HRM.day(r.GpDate));
            HRM.setVal('txtNetWeightWb', HRM.str(r.WbNetWeight));
            if (r.OutDateTime && HRM.visible('btnsave') && !HRM.$('btnsave').disabled) HRM.setVal('txtdocdate', HRM.day(r.OutDateTime));
            txtNetBillWeight_TextChanged();
        }
    }
    function CmbDeliveryOrderNo_Leave() {
        var r = LgsB.row('CmbDeliveryOrderNo'); if (!HRM.comboVal('CmbDeliveryOrderNo') || !r) return;
        var gpId = HRM.int(r.GpId);
        if (gpId > 0) {
            if (HRM.comboVal('CmbGpNo') !== gpId) { HRM.setCombo('CmbGpNo', gpId); CmbGpNo_Leave(); }
            HRM.setVal('txtDeliveryOrderDate', HRM.day(r.DoDate));
            HRM.setVal('txtNoOfBags', HRM.str(r.DoQty));
            HRM.setVal('txtNetWeightDo', HRM.str(r.DoWeight));
            txtNoOfBags_TextChanged();
            txtNetBillWeight_TextChanged();
        }
    }
    /** CmbLogisticPoNo_Leave: an order fixes the rate and its uom (and locks the rate fields). */
    function CmbLogisticPoNo_Leave() {
        ['CmbRateUom', 'txtFreightRate', 'txtQtyForRate', 'txtBiltyFreight', 'txtNetWeightDo'].forEach(function (i) { HRM.enable(i, true); });
        var r = LgsB.row('CmbLogisticPoNo');
        if (HRM.comboVal('CmbLogisticPoNo') && r) {
            HRM.setVal('txtFreightRate', LgsB.net(r.Rate, '#,##.##'));
            HRM.setCombo('CmbRateUom', HRM.int(r.RateBaseUomId));
            ['CmbRateUom', 'txtFreightRate', 'txtQtyForRate', 'txtBiltyFreight', 'txtNetWeightDo'].forEach(function (i) { HRM.enable(i, false); });
            CalculateQtyForRate();
            CalculateBiltyFright();
            CalculateNetPaid();
        }
    }

    // ------------------------------------------------------------------------ calculations
    function CalculateQtyForRate() {
        var w = num('txtNetBillWeight'), u = HRM.comboVal('CmbRateUom');
        HRM.setVal('txtQtyForRate', LgsB.net(u === 3 ? w / 1000.0 : u === 4 ? w : 1.0, '#,##.###'));
    }
    function CalculateBiltyFright() {
        var rate = num('txtFreightRate'), b = rate > 0 ? rate * num('txtQtyForRate') : 0;
        HRM.setVal('txtBiltyFreight', LgsB.net(b, amtFmt()));
    }
    function CalculateNetPaid() {
        var bilty = num('txtBiltyFreight');
        var other = grdExp ? grdExp.sum('AddAmount') - grdExp.sum('LessAmount') : 0;
        HRM.setVal('txtOtherCharges', LgsB.net(other, amtFmt()));
        HRM.setVal('txtTotalBiltyFreight', LgsB.net(bilty + other, amtFmt()));
        var adv = grdPay ? grdPay.sum('Amount') : 0;
        HRM.setVal('txtAdvanceCashPaid', LgsB.net(adv, amtFmt()));
        HRM.setVal('txtNetFreightPayable', LgsB.net(bilty + other - adv, amtFmt()));
    }
    function txtNoOfBags_TextChanged() { CalculateQtyForRate(); CalculateBiltyFright(); CalculateNetPaid(); }
    /** txtNetBillWeight_TextChanged / CmbBillWeightBase_ValueChanged: 23 = DO weight, 24 = WB weight. */
    function txtNetBillWeight_TextChanged() {
        var b = HRM.comboVal('CmbBillWeightBase');
        if (b === 23) HRM.setVal('txtNetBillWeight', HRM.val('txtNetWeightDo'));
        if (b === 24) HRM.setVal('txtNetBillWeight', HRM.val('txtNetWeightWb'));
        CalculateQtyForRate(); CalculateBiltyFright(); CalculateNetPaid();
    }

    // ------------------------------------------------------------------------ payment (credit entries) grid
    /** FreightVoucherOutward_Helper.PaymentDetailGridCommonSetting is not decompiled: columns follow dtPaymentDetail. */
    function payColumns(editable) {
        return [{ key: 'Id', hidden: true }, { key: 'TransactionTypeId', hidden: true }, { key: 'TransactionType', caption: 'TransactionType', width: 90 },
            { key: 'InstrumentTypeId', hidden: true }, { key: 'InstrumentType', caption: 'InstrumentType', width: 90 },
            { key: 'AccountTitleId', hidden: true }, { key: 'AccountTitle', caption: 'AccountTitle', width: 200 },
            { key: 'ChequeId', hidden: true }, { key: 'ChequeNo', caption: 'ChequeNo', width: 90 }, { key: 'ChequeDate', caption: 'ChequeDate', type: 'date' },
            { key: 'Amount', caption: 'Amount', type: editable ? 'edit-num' : 'num', decimals: amt(), sum: true, width: 100 },
            { key: 'PayeeTitle', caption: 'PayeeTitle', type: editable ? 'edit' : 'text', width: 140 },
            { key: 'Remarks', caption: 'Remarks', type: editable ? 'edit' : 'text', width: 160 }];
    }
    function buildPayGrid() {
        var keep = grdPay ? grdPay.rows() : [];
        grdPay = LgsB.freshGrid('grdPaymentDetail', {
            columns: [LgsB.btnCol('Delete', '', 'X', 20), LgsB.btnCol('Edit', '', 'Edit', 40)].concat(payColumns(true)),
            totals: true, emptyText: '',
            onDouble: function (r, i) { grdCustomerDetail_DoubleClick(r, i); },
            onChange: function () { CalculateNetPaid(); }
        });
        LgsB.trackColumn(grdPay);
        LgsB.onButton(grdPay, function (r, act, i) {
            if (act === 'Delete') DeleteCustomerDetailRow(r, i);
            if (act === 'Edit') grdCustomerDetail_DoubleClick(r, i);
            CalculateNetPaid();
            transporterLock();
        });
        LgsB.gridKeys(grdPay, {
            'ctrl+space': function (r, i) { if (LgsB.lastCol(grdPay) === 'Delete') DeleteCustomerDetailRow(r, i); },
            'ctrl+delete': function (r, i) { DeleteCustomerDetailRow(r, i); }
        });
        grdPay.set(keep);
    }
    function transporterLock() { HRM.enable('CmbTransporter', grdPay.rows().length === 0); }
    /** FormValidation. */
    function FormValidation() {
        if (!HRM.comboVal('CmbTransporter')) { HRM.box('Transporter Account field is required'); HRM.focus('CmbTransporter'); return false; }
        if (HRM.comboVal('CmbTransactionType') === 2 && !HRM.comboVal('CmbInstrumentType')) { HRM.box('Instrument Type field is required'); HRM.focus('CmbInstrumentType'); return false; }
        if (HRM.comboVal('CmbTransactionType') === 2 && HRM.comboVal('CmbInstrumentType') === 1) {
            if (cfg.chequeBookEnabled) { if (!HRM.comboVal('CmbChequeNo')) { HRM.box('Cheque No Field is Required'); HRM.focus('CmbChequeNo'); return false; } }
            else if (!HRM.val('txtChequeNo')) { HRM.box('Cheque No field is required'); HRM.focus('txtChequeNo'); return false; }
        }
        if (!HRM.comboVal('CmbAccountTitle')) { HRM.box('Bill To Account(Cr) field is required'); HRM.focus('CmbAccountTitle'); return false; }
        return true;
    }
    function entryChecks(others) {
        if (!FormValidation()) return false;
        if (!HRM.comboVal('CmbTransactionType')) { HRM.box('Transaction Type field is required'); HRM.focus('CmbTransactionType'); return false; }
        if (!(num('txtAmount') > 0)) { HRM.box('Net Amount field is Required'); HRM.focus('txtAmount'); return false; }
        if (others + num('txtAmount') > num('txtTotalBiltyFreight')) { HRM.box('Amount is greater than total paid amount'); return false; }
        var t = LgsB.row('CmbTransporter');
        if (t && HRM.int(t.GlAccountId) === HRM.comboVal('CmbAccountTitle')) { HRM.box('Transporter account cannot be add in grid please check....'); return false; }
        return true;
    }
    function FillCustomerDetailRow(dr) {
        dr.TransactionTypeId = HRM.comboVal('CmbTransactionType'); dr.TransactionType = LgsB.text('CmbTransactionType');
        dr.InstrumentTypeId = HRM.comboVal('CmbInstrumentType'); dr.InstrumentType = LgsB.text('CmbInstrumentType');
        dr.AccountTitleId = HRM.comboVal('CmbAccountTitle'); dr.AccountTitle = LgsB.text('CmbAccountTitle');
        dr.ChequeId = cfg.chequeBookEnabled ? HRM.comboVal('CmbChequeNo') : 0;
        dr.ChequeNo = cfg.chequeBookEnabled ? LgsB.text('CmbChequeNo') : HRM.val('txtChequeNo');
        dr.ChequeDate = HRM.val('datChequeDate');
        dr.Amount = num('txtAmount');
        dr.PayeeTitle = HRM.val('txtPayeeTitle'); dr.Remarks = HRM.val('txtRemarks');
        return dr;
    }
    P.BtnAddCustomer = function () {                                           // BtnAddCustomer_Click
        if (!entryChecks(grdPay.sum('Amount'))) return;
        grdPay.add(FillCustomerDetailRow({ Id: 0 }));
        ResetCustomerDetail(); CalculateNetPaid(); transporterLock();
    };
    P.BtnUpdateCustmer = function () {                                         // BtnUpdateCustmer_Click
        var rows = grdPay.rows(), cur = rows[updateDetailIndex];
        if (!entryChecks(grdPay.sum('Amount') - (cur ? HRM.num(cur.Amount) : 0))) return;
        if (updateDetailIndex < 0 || updateDetailIndex >= rows.length) return;
        FillCustomerDetailRow(cur); grdPay.draw();
        ResetCustomerDetail(); CalculateNetPaid(); transporterLock();
    };
    P.BtnCancelCustomer = function () { ResetCustomerDetail(); };
    function grdCustomerDetail_DoubleClick(r, i) {
        if (!r) return;
        updateDetailIndex = i;
        HRM.setCombo('CmbTransactionType', HRM.int(r.TransactionTypeId));
        CmbTransactionType_Leave();
        if (HRM.int(r.InstrumentTypeId) > 0) HRM.setCombo('CmbInstrumentType', HRM.int(r.InstrumentTypeId));
        HRM.setCombo('CmbAccountTitle', HRM.int(r.AccountTitleId));
        AccountCurrentBalance();
        Promise.resolve(CheqNoFill()).then(function () {
            if (cfg.chequeBookEnabled) HRM.setCombo('CmbChequeNo', HRM.int(r.ChequeId)); else HRM.setVal('txtChequeNo', HRM.str(r.ChequeNo));
        });
        HRM.setVal('datChequeDate', HRM.day(r.ChequeDate));
        HRM.setVal('txtAmount', LgsB.net(r.Amount, '#,##0.###'));
        HRM.setVal('txtPayeeTitle', HRM.str(r.PayeeTitle)); HRM.setVal('txtRemarks', HRM.str(r.Remarks));
        HRM.show('BtnAddCustomer', false); HRM.show('BtnUpdateCustmer', true); HRM.show('BtnCancelCustomer', true);
        HRM.focus('CmbTransactionType');
    }
    function DeleteCustomerDetailRow(r, i) {
        if (updateDetailIndex !== -1) { HRM.box('Please Reset the Detail first..'); return; }
        if (HRM.int(r.Id) !== 0) { if (!HRM.ask('Are you sure to Delete?')) return; removed.push(r); }
        grdPay.remove(i);
    }
    function ResetCustomerDetail() {
        HRM.setCombo('CmbInstrumentType', 0);
        LgsB.fillCols('CmbAccountTitle', [], 'Id', 'AccountTitle', [], null, { zero: '', keep: false });
        HRM.text('BalanceValue', '0');
        HRM.setCombo('CmbChequeNo', 0); HRM.setVal('txtChequeNo', '');
        HRM.setVal('datChequeDate', HRM.today());
        ['txtAmount', 'txtPayeeTitle', 'txtRemarks'].forEach(function (i) { HRM.setVal(i, ''); });
        updateDetailIndex = -1;
        HRM.show('BtnAddCustomer', true); HRM.show('BtnCancelCustomer', false); HRM.show('BtnUpdateCustmer', false);
        HRM.focus('CmbTransactionType');
    }

    // ------------------------------------------------------------------------ expense grid (Add / Less From Bilty Freight)
    function itemName(id) { var n = ''; otherItems.forEach(function (o) { if (HRM.int(HRM.col(o, 'LogiticOtherChargesItemsId')) === HRM.int(id)) n = HRM.str(HRM.col(o, 'Description')); }); return n; }
    function buildExpGrid() {
        var keep = grdExp ? grdExp.rows() : [];
        grdExp = LgsB.freshGrid('grdInvExp', {
            columns: [LgsB.btnCol('Delete', '', 'Clear', 50), { key: 'Id', hidden: true },
                { key: 'ItemId', caption: 'Add/Less Account', width: 140, render: function (v) { return HRM.esc(itemName(v)); } },
                { key: 'Qty', caption: 'Qty', type: 'edit-num', decimals: 2, width: 55 }, { key: 'Rate', caption: 'Rate', type: 'edit-num', decimals: 2, width: 55 },
                { key: 'AddAmount', caption: 'AddAmount', type: 'edit-num', decimals: amt(), sum: true, width: 70 },
                { key: 'LessAmount', caption: 'LessAmount', type: 'edit-num', decimals: amt(), sum: true, width: 70 },
                { key: 'Remarks', caption: 'Remarks', type: 'edit', width: 90 }],
            totals: true, emptyText: '',
            onChange: function (r, k) { grdInvExp_CellUpdated(r, k); }
        });
        LgsB.trackColumn(grdExp);
        LgsB.onButton(grdExp, function (r, act) { if (act === 'Delete') DeleteRowInExpenseGrid(r); CalculateNetPaid(); });
        LgsB.gridKeys(grdExp, {
            'ctrl+space': function (r) { if (LgsB.lastCol(grdExp) === 'Delete') DeleteRowInExpenseGrid(r); CalculateNetPaid(); },
            'ctrl+delete': function (r) { DeleteRowInExpenseGrid(r); CalculateNetPaid(); }
        });
        grdExp.set(keep);
    }
    /** GenerateRowsInExpenseGrid: one row per charges item (only the missing ones when the grid has rows). */
    function GenerateRowsInExpenseGrid() {
        if (!otherItems.length) return;
        var rows = grdExp.rows(), has = rows.length > 0, seen = {};
        if (!has) rows = [];
        rows.forEach(function (r) { seen[HRM.int(r.ItemId)] = 1; });
        otherItems.forEach(function (o) {
            var id = HRM.int(HRM.col(o, 'LogiticOtherChargesItemsId'));
            if (has && seen[id]) return;
            seen[id] = 1;
            rows.push({ Id: 0, ItemId: id, Qty: 0, Rate: 0, AddAmount: 0, LessAmount: 0, Remarks: '' });
        });
        grdExp.set(rows);
    }
    function DeleteRowInExpenseGrid(r) {
        if (HRM.int(r.Id) > 0 && !HRM.ask('Are you sure to Clear this Row?')) return;
        r.Id = 0; r.Qty = 0; r.Rate = 0; r.AddAmount = 0; r.LessAmount = 0;
        GenerateRowsInExpenseGrid();
    }
    function grdInvExp_CellUpdated(r, k) {
        var q = HRM.num(r.Qty);
        if (k === 'Qty' || k === 'Rate') { r.LessAmount = 0; r.AddAmount = q * HRM.num(r.Rate); }
        else if (k === 'AddAmount') { r.Rate = q === 0 ? 0 : HRM.num(r.AddAmount) / q; r.LessAmount = 0; }
        else if (k === 'LessAmount') { r.Rate = q === 0 ? 0 : HRM.num(r.LessAmount) / q; r.AddAmount = 0; }
        else return;
        grdExp.draw();
        CalculateNetPaid();
    }
    P.BtnOtherChargesItem = function (btn) {                                   // BtnOtherChargesItem_Click
        HRM.box('The Define Charges Item form (frmLogiticOtherChargesItems) is not part of the web port - the list is reloaded.');
        return HRM.busy(btn || 'BtnOtherChargesItem', function () {
            return HRM.get(API + '/other-charges').then(function (rows) { otherItems = rows || []; grdExp.draw(); GenerateRowsInExpenseGrid(); }).catch(HRM.fail);
        });
    };

    // ------------------------------------------------------------------------ New / Refresh / Save / Update / Delete
    function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnUpdate', update); HRM.show('btnDelete', update); }
    function FromReset(docNo) {
        RecId = 0; removed = []; drRecord = [];
        buttons(false); UpdateMode = false;
        ['CmbGpNo', 'CmbDeliveryOrderNo', 'CmbTransporter', 'CmbLoadingCity', 'CmbUnLoadingCity', 'CmbLogisticPoNo'].forEach(function (i) { HRM.setCombo(i, 0); });
        ['txtRemarksHeader', 'txtVehicleNo', 'txtBiltyNo', 'txtNoOfBags', 'txtNetWeightDo', 'txtNetWeightWb', 'txtNetBillWeight',
            'txtFreightRate', 'txtQtyForRate', 'txtBiltyFreight', 'txtOtherCharges', 'txtAdvanceCashPaid', 'txtNetFreightPayable', 'txtTotalBiltyFreight'].forEach(function (i) { HRM.setVal(i, ''); });
        ResetCustomerDetail();
        grdPay.clear(); grdExp.clear(); GenerateRowsInExpenseGrid();
        ['CmbRateUom', 'txtFreightRate', 'txtQtyForRate', 'txtBiltyFreight', 'txtNetWeightDo'].forEach(function (i) { HRM.enable(i, true); });
        transporterLock();
        HRM.focus('txtdocdate');
        return Promise.all([
            docNo !== undefined ? Promise.resolve({ docNo: docNo }) : HRM.get(API + '/setup', { recId: 0 }),
            HRM.get(API + '/pending-gp', { recId: 0 })
        ]).then(function (res) {
            HRM.setVal('txtdocno', HRM.str(res[0].docNo));
            GpAndDoBind(res[1]);
            return CmbTransporter_Leave();
        }).catch(HRM.fail);
    }
    P.btnnew = function () { return FromReset(); };
    P.btnRefresh = function (btn) {                                            // btnRefresh_Click
        return HRM.busy(btn || 'btnRefresh', function () {
            return HRM.get(API + '/refresh', { recId: RecId }).then(function (d0) {
                bindLists(d0); transporterLock(); grdExp.draw(); GenerateRowsInExpenseGrid(); return CmbTransporter_Leave();
            }).catch(HRM.fail);
        });
    };
    function payload() {
        return {
            recId: RecId, docNo: HRM.val('txtdocno'), docDate: HRM.val('txtdocdate') + 'T' + HRM.nowTime() + ':00',
            gpId: HRM.comboVal('CmbGpNo'), doId: HRM.comboVal('CmbDeliveryOrderNo'), doNo: LgsB.text('CmbDeliveryOrderNo'),
            transporterId: HRM.comboVal('CmbTransporter'), loadingCityId: HRM.comboVal('CmbLoadingCity'), unloadingCityId: HRM.comboVal('CmbUnLoadingCity'),
            vehicleNo: HRM.val('txtVehicleNo'), biltyNo: HRM.val('txtBiltyNo'), biltyDate: HRM.val('txtBiltyDate'), poId: HRM.comboVal('CmbLogisticPoNo'),
            noOfBags: HRM.num(HRM.val('txtNoOfBags')), netWeightDo: HRM.num(HRM.val('txtNetWeightDo')), netWeightWb: HRM.num(HRM.val('txtNetWeightWb')),
            billWeightBaseId: HRM.comboVal('CmbBillWeightBase'), billWeight: HRM.num(HRM.val('txtNetBillWeight')), freightRate: HRM.num(HRM.val('txtFreightRate')),
            rateUomId: HRM.comboVal('CmbRateUom'), rateUomCode: LgsB.text('CmbRateUom'), qtyForRate: HRM.num(HRM.val('txtQtyForRate')),
            biltyFreight: HRM.num(HRM.val('txtBiltyFreight')), netPayable: HRM.num(HRM.val('txtNetFreightPayable')),
            chargesToAccountId: HRM.comboVal('CmbChargesToDrAccount'), remarks: HRM.val('txtRemarksHeader'),
            payments: grdPay.rows(), removed: RecId > 0 ? removed : [], expenses: grdExp.rows(), drRecord: drRecord
        };
    }
    /** Insert(). */
    function Insert(btn) {
        function c(id, name) { if (!HRM.comboVal(id)) { HRM.box(name + ' field is required'); HRM.focus(id); return false; } return true; }
        function t(id, name) { if (!HRM.val(id).trim()) { HRM.box(name + ' field is required'); HRM.focus(id); return false; } return true; }
        function n(id, name) { if (HRM.num(HRM.val(id)) === 0) { HRM.box(name + ' must be a non-zero number'); HRM.focus(id); return false; } return true; }
        if (!n('txtdocno', 'Doc No') || !c('CmbGpNo', 'Gate Pass No') || !c('CmbDeliveryOrderNo', 'Delivery Order No') || !c('CmbTransporter', 'Transporter') ||
            !c('CmbLoadingCity', 'Loading City') || !c('CmbUnLoadingCity', 'Un-Loading City') || !t('txtVehicleNo', 'Vehicle No') || !t('txtBiltyNo', 'Bilty No') ||
            !n('txtNoOfBags', 'No Of Bags') || !c('CmbBillWeightBase', 'Bill Weight Base') || !n('txtNetBillWeight', 'Bill Weight') ||
            !n('txtFreightRate', 'Freight Rate') || !c('CmbRateUom', 'Rate Uom') || !n('txtQtyForRate', 'Qty For Rate') || !n('txtBiltyFreight', 'Bilty Freight')) return;
        if (num('txtNetFreightPayable') < 0) { HRM.box('Net Freight Payable cannot be lesser than zero.'); HRM.focus('CmbTransactionType'); return; }
        if (HRM.comboVal('CmbLoadingCity') === HRM.comboVal('CmbUnLoadingCity')) { HRM.box('Loading City and Unloading City cannot be the same.'); HRM.focus('CmbUnLoadingCity'); return; }
        if (!HRM.comboVal('CmbChargesToDrAccount')) {
            HRM.box("Please select a Charges To Account (Dr).\n\nIf the list is empty, go to Configuration, set the default Charges Account's Custom Group, and then click Refresh and then Select an Account.");
            HRM.focus('CmbChargesToDrAccount'); return;
        }
        CalculateNetPaid();
        if (!HRM.ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
        var preview = HRM.checked('ChkPreview') && rights.print, voucher = HRM.checked('chkVoucher') && rights.print;
        var win = preview && window.CrystalPrint ? window.CrystalPrint.reserve() : null;
        var win2 = voucher && window.CrystalPrint ? window.CrystalPrint.reserve() : null;
        return HRM.busy(btn, function () {
            return HRM.post(API + '/save', payload()).then(function (res) {
                HRM.box(res.message);
                FromReset(res.docNo);
                if (preview && window.CrystalPrint) window.CrystalPrint.open('lgsb-1305', { id: res.id }, null, win);
                if (voucher && window.CrystalPrint) {
                    HRM.get(API + '/voucher-id', { id: res.id }).then(function (v) { window.CrystalPrint.open('hrm-102', { id: HRM.int(v.voucherHeadId) }, null, win2); })
                        .catch(function (e) { if (win2) window.CrystalPrint.release(win2); HRM.fail(e); });
                }
            }).catch(function (e) {
                if (win) window.CrystalPrint.release(win);
                if (win2) window.CrystalPrint.release(win2);
                HRM.fail(e);
            });
        }, 'fv-save');
    }
    P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };
    P.btnUpdate = function (btn) { return Insert(btn || 'btnUpdate'); };
    P.btnDelete = function (btn) {
        if (!(RecId > 0)) { HRM.box('No record found to Delete'); return; }
        if (!HRM.ask('Are you sure to Delete?')) return;
        return HRM.busy(btn || 'btnDelete', function () {
            return HRM.post(API + '/delete?id=' + RecId, {}).then(function (r) { HRM.box(r.message); FromReset(r.docNo); }).catch(HRM.fail);
        });
    };
    /** ReadById(Id). */
    function ReadById(id) {
        return HRM.loading(FromReset().then(function () {
            RecId = id;
            return Promise.all([HRM.get(API + '/by-id', { id: id }), HRM.get(API + '/pending-gp', { recId: id })]);
        }).then(function (res) {
            var o = res[0], h = o.header, c = function (k) { return HRM.col(h, k); };
            show('tabPage1');
            UpdateMode = true; buttons(true);
            HRM.setVal('txtdocdate', HRM.day(c('DocDate')));
            HRM.setVal('txtdocno', HRM.str(c('DocNo')));
            GpAndDoBind(res[1]);
            HRM.setCombo('CmbDeliveryOrderNo', HRM.int(c('InvDeliveryOrderId'))); CmbDeliveryOrderNo_Leave();
            HRM.setCombo('CmbGpNo', HRM.int(c('GatePassOutwardId'))); CmbGpNo_Leave();
            HRM.setCombo('CmbTransporter', HRM.int(c('transporterId')));
            return CmbTransporter_Leave().then(function () {
                HRM.setCombo('CmbLoadingCity', HRM.int(c('loadingFromCityId')));
                HRM.setCombo('CmbUnLoadingCity', HRM.int(c('unloadingToCityId')));
                HRM.setVal('txtRemarksHeader', HRM.str(c('RemarksHeader')));
                HRM.setVal('txtVehicleNo', HRM.str(c('vehicleNo')));
                HRM.setVal('txtBiltyNo', HRM.str(c('biltyNo')));
                HRM.setVal('txtBiltyDate', HRM.day(c('biltyDate')));
                HRM.setCombo('CmbLogisticPoNo', HRM.int(c('PurchaseOrderHeaderId'))); CmbLogisticPoNo_Leave();
                HRM.setVal('txtNoOfBags', LgsB.net(c('NoOfBags'), '#,##.##'));
                HRM.setVal('txtNetWeightDo', LgsB.net(c('NetWeightDo'), '#,##.##'));
                HRM.setVal('txtNetWeightWb', LgsB.net(c('NetWeightWB'), '#,##.##'));
                HRM.setCombo('CmbBillWeightBase', HRM.int(c('BillWeightBaseId')));
                HRM.setVal('txtNetBillWeight', LgsB.net(c('netWeight'), '#,##.##'));
                HRM.setVal('txtFreightRate', LgsB.net(c('freightRate'), rateFmt()));
                HRM.setCombo('CmbRateUom', HRM.int(c('rateUomId')));
                HRM.setVal('txtQtyForRate', LgsB.net(c('QtyforRate'), '#,##.##'));
                HRM.setVal('txtBiltyFreight', LgsB.net(c('biltyFreight'), amtFmt()));
                HRM.setVal('txtOtherCharges', LgsB.net(c('otherCharges'), amtFmt()));
                HRM.setVal('txtTotalBiltyFreight', LgsB.net(c('totalbiltyfreight'), amtFmt()));
                HRM.setVal('txtAdvanceCashPaid', LgsB.net(c('AdvanceOrCashFreight'), amtFmt()));
                HRM.setVal('txtNetFreightPayable', LgsB.net(c('totalFreight'), amtFmt()));
                HRM.setCombo('CmbChargesToDrAccount', HRM.int(c('chargeToDrAccountId')));
                drRecord = o.drRecord || [];
                grdPay.set(o.payments || []);
                grdExp.set(o.expenses || []);
                GenerateRowsInExpenseGrid();
                CalculateNetPaid();
                transporterLock();
                HRM.focus('txtdocdate');
            });
        }).catch(HRM.fail));
    }
    P.btnPrint = function (btn) {                                              // btnPrint_Click -> FreightVoucherOutward_Slip(RecId)
        return LgsB.print('lgsb-1305', { id: RecId }, function () { return HRM.get(API + '/print-check', { id: RecId }); }, btn || 'btnPrint');
    };
    function voucher102(id, btn) {
        var win = window.CrystalPrint ? window.CrystalPrint.reserve() : null;
        return HRM.busy(btn, function () {
            return HRM.get(API + '/voucher-id', { id: id }).then(function (v) {
                if (!window.CrystalPrint) { HRM.box('The print is not available.'); return; }
                return window.CrystalPrint.open('hrm-102', { id: HRM.int(v.voucherHeadId) }, null, win);
            }).catch(function (e) { if (win) window.CrystalPrint.release(win); HRM.fail(e); });
        });
    }
    P.btnPrintVoucher = function (btn) {                                       // btnPrintVoucher_Click
        if (!(RecId > 0)) { HRM.box('Record Not Found For Display'); return; }
        return voucher102(RecId, btn || 'btnPrintVoucher');
    };
    var KEYS = [['Ctrl+S', 'For Save When on Entry form and For Show Data when on History Form'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'],
        ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Doc Date'],
        ['Ctrl+F10', 'For Open Attachments'], ['F1', 'For Combo Lookup on Current Column of any Focused Grid'], ['Ctrl+T', 'For Tab Transfer'],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+D', 'For Adding an row in Focused Grid'], ['Ctrl+Delete', 'For Deleting an row of Focused Grid'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On on Doc Date'], ['Ctrl+ArrowRight', 'to change focus from one grid to another'],
        ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    P.btnshortcutkeys = function () { LgsB.shortcuts(KEYS); };

    // ------------------------------------------------------------------------ Gate Pass Loader (frmLoadPendingGpForFreightVoucher)
    P.BtnLoadGp = function () {
        var already = HRM.comboVal('CmbGpNo'), f = LgsB.f;
        LgsB.loader({
            title: 'Gate Pass Loader', caption: 'Pending Gate Pass & Delivery Order for Freight Entry Voucher (Export)',
            filters: f('Gp Date From', '<input type="date" class="win-textbox" data-f="fromDate">') + f('Gp Date To', '<input type="date" class="win-textbox" data-f="toDate">') +
                f('Gp No From', '<input type="text" class="win-textbox" data-f="fromDocNo" data-guard="integer">') + f('Gp No To', '<input type="text" class="win-textbox" data-f="toDocNo" data-guard="integer">') +
                f('Customer', '<select class="win-combo dtcombo" id="ldGpCustomer"></select>') + f('Transporter', '<select class="win-combo dtcombo" id="ldGpTransporter"></select>') +
                '<div class="hrm-field hrm-stack lgsb-w2"><label>&nbsp;</label><div class="lgsb-radios">' +
                '<label><input type="radio" name="ldGpAction" value="0"> All (Pending &amp; Complete)</label>' +
                '<label><input type="radio" name="ldGpAction" value="1" checked> Pending For Voucher Entry</label>' +
                '<label><input type="radio" name="ldGpAction" value="2"> Freight Entry Completed</label></div></div>',
            onReady: function (body) {
                body.querySelector('[data-f="fromDate"]').value = HRM.addDays(HRM.today(), -7);
                body.querySelector('[data-f="toDate"]').value = HRM.today();
                return combos();
            },
            refresh: function () { return combos(); },
            reset: function (body) {
                if (LgsB.yearStart) body.querySelector('[data-f="fromDate"]').value = LgsB.yearStart;
                body.querySelector('[data-f="fromDocNo"]').value = ''; body.querySelector('[data-f="toDocNo"]').value = '';
                HRM.setCombo('ldGpCustomer', 0);
            },
            search: function (body) {
                var q = function (k) { return body.querySelector('[data-f="' + k + '"]').value; }, a = body.querySelector('input[name="ldGpAction"]:checked');
                return HRM.post(API + '/gp-loader', { fromDate: q('fromDate'), toDate: q('toDate'), fromDocNo: q('fromDocNo'), toDocNo: q('toDocNo'),
                    customerId: HRM.comboVal('ldGpCustomer'), transporterId: HRM.comboVal('ldGpTransporter'), actionId: a ? HRM.int(a.value) : 1 });
            },
            rows: function (all) {
                return all.map(function (r) {
                    var c = function (k) { return HRM.col(r, k); };
                    return { Id: c('GpId'), GpSrNo: c('GpSrNo'), GpDate: c('GpDate'), VehicleType: c('VehicleType'), VehicleNo: c('VehicleNo'), BiltyNo: c('BiltyNo'),
                        BiltyDate: c('BiltyDate'), GpBiltyFreight: c('GpBiltyFreight'), GpNetFreight: c('GpNetFreight'), CustomerNames: c('CustomerNames'),
                        ItemNames: c('ItemNames'), DeliveryOrderId: c('DeliveryOrderId'), DeliveryOrderNo: c('DeliveryOrderNo'), DeliveryOrderDate: c('DeliveryOrderDate'),
                        DoQty: c('DoQty'), DoWeight: c('DoWeight'), WbFirstWeight: c('WbFirstWeight'), WbSecondWeight: c('WbSecondWeight'), WbNetWeight: c('WbNetWeight'),
                        FreightVoucherOutwardId: c('FreightVoucherOutwardId'), FreightVoucherNo: c('FreightVoucherNo'), FreightVoucherDate: c('FreightVoucherDate'),
                        TransporterName: c('TransporterName'), LoadingFromCityName: c('LoadingFromCityName'), UnloadingToCityName: c('UnloadingToCityName'),
                        TotalBiltyFreight: c('TotalBiltyFreight') };
                });
            },
            checked: function (r) { return already > 0 && HRM.int(r.Id) === already; },
            columns: [{ key: 'Id', hidden: true }, { key: 'GpSrNo', caption: 'GpSrNo', width: 60 }, { key: 'GpDate', caption: 'GpDate', type: 'date' },
                { key: 'VehicleType', caption: 'VehicleType', width: 120 }, { key: 'VehicleNo', caption: 'VehicleNo', width: 100 }, { key: 'BiltyNo', caption: 'BiltyNo', width: 70 },
                { key: 'BiltyDate', caption: 'BiltyDate', type: 'date' }, LgsB.numCol('GpBiltyFreight', 'GpBiltyFreight', 2), LgsB.numCol('GpNetFreight', 'GpNetFreight', 2),
                { key: 'CustomerNames', caption: 'CustomerNames', width: 200 }, { key: 'ItemNames', caption: 'ItemNames', width: 200 }, { key: 'DeliveryOrderId', hidden: true },
                { key: 'DeliveryOrderNo', caption: 'DeliveryOrderNo', width: 65 }, { key: 'DeliveryOrderDate', caption: 'DeliveryOrderDate', type: 'date' },
                LgsB.numCol('DoQty', 'DoQty', 2), LgsB.numCol('DoWeight', 'DoWeight', 2), { key: 'WbFirstWeight', hidden: true }, { key: 'WbSecondWeight', hidden: true },
                LgsB.numCol('WbNetWeight', 'WbNetWeight', 2), { key: 'FreightVoucherOutwardId', hidden: true }, { key: 'FreightVoucherNo', caption: 'FreightVoucherNo', width: 65 },
                { key: 'FreightVoucherDate', caption: 'FreightVoucherDate', type: 'date' }, { key: 'TransporterName', caption: 'TransporterName', width: 180 },
                { key: 'LoadingFromCityName', caption: 'LoadingFromCityName', width: 100 }, { key: 'UnloadingToCityName', caption: 'UnloadingToCityName', width: 100 },
                LgsB.numCol('TotalBiltyFreight', 'TotalBiltyFreight', 2)],
            load: function (checked, all) {
                var base = HRM.int(checked[0].Id);
                for (var i = 0; i < checked.length; i++) {
                    var r = checked[i];
                    if (HRM.int(r.Id) !== base) { HRM.box('Sorry! You can Only Check rows which have the same Gp'); return false; }
                    var fv = HRM.int(r.FreightVoucherOutwardId);
                    if (fv > 0 && fv !== RecId) {
                        HRM.box('Sorry! You cannot load GpNo ' + HRM.int(r.GpSrNo) + ".\nIt already exists against In another Freight-Voucher  '" + HRM.int(r.FreightVoucherNo) + "'.");
                        return false;
                    }
                }
                return all.filter(function (r) { return HRM.int(HRM.col(r, 'GpId')) === base; });
            },
            done: function (rows) {                                            // BtnLoadGp_Click
                var gp = HRM.int(HRM.col(rows[0], 'GpId'));
                if (already !== gp) { HRM.setCombo('CmbGpNo', gp); CmbGpNo_Leave(); }
                HRM.focus('CmbTransporter');
            }
        });
        function combos() {
            return HRM.get(API + '/gp-loader-combos').then(function (d0) {
                HRM.fill('ldGpCustomer', d0.customers || [], 'Id', 'ReferenceName', { zero: '', keep: true });
                HRM.fill('ldGpTransporter', d0.transporters || [], 'Id', 'ReferenceName', { zero: '', keep: true });
            });
        }
    };

    // ------------------------------------------------------------------------ History tab
    function historyGrids() {
        grdHistory = new HRM.Grid('grdHistory', {
            columns: [LgsB.btnCol('Edit', 'Edit', 'Edit', 50), LgsB.btnCol('Print', 'Print', 'Print', 50), LgsB.btnCol('Voucher', 'Voucher', 'Voucher', 70),
                { key: 'Id', hidden: true }, { key: 'DocNo', caption: 'DocNo', width: 70 }, { key: 'DocDate', caption: 'DocDate', type: 'date' },
                { key: 'DocumentTypeId', hidden: true }, { key: 'GatePassOutwardId', hidden: true }, { key: 'GpSrNo', caption: 'GpSrNo', width: 60 },
                { key: 'GpDate', caption: 'GpDate', type: 'date' }, { key: 'InvDeliveryOrderId', hidden: true }, { key: 'DeliveryOrderNo', caption: 'DeliveryOrderNo', width: 70 },
                { key: 'DeliveryOrderDate', caption: 'DeliveryOrderDate', type: 'date' }, { key: 'TransporterId', hidden: true },
                { key: 'TransporterAccountTitle', caption: 'TransporterAccountTitle', width: 150 }, { key: 'VehicleNo', caption: 'VehicleNo', width: 90 },
                { key: 'BiltyNo', caption: 'BiltyNo', width: 90 }, { key: 'BiltyDate', caption: 'BiltyDate', type: 'date' }, { key: 'ChargeToDrAccountId', hidden: true },
                { key: 'ChargedToDrAccountTitle', caption: 'ChargedToDrAccountTitle', width: 150 }, { key: 'LoadingFromCityName', caption: 'LoadingFromCityName', width: 100 },
                { key: 'UnloadingToCityName', caption: 'UnloadingToCityName', width: 100 }, { key: 'PurchaseOrderHeaderId', hidden: true },
                { key: 'PurchaseOrderNo', caption: 'PurchaseOrderNo', width: 70 }, { key: 'NoOfBags', caption: 'NoOfBags', width: 80 },
                LgsB.numCol('NetWeightDo', 'NetWeightDo', 2), LgsB.numCol('NetWeightWb', 'NetWeightWb', 2), LgsB.numCol('BillWeight', 'BillWeight', 2),
                LgsB.numCol('FreightRate', 'FreightRate', 2), { key: 'RateUomCode', caption: 'RateUomCode', width: 60 }, LgsB.numCol('QtyforRate', 'QtyforRate', 3),
                LgsB.numCol('BiltyFreight', 'BiltyFreight', amt()), LgsB.numCol('OtherCharges', 'OtherCharges', amt()), LgsB.numCol('TotalFreight', 'TotalFreight', amt()),
                { key: 'EntryDate', caption: 'EntryDate', type: 'datetime' }, { key: 'EntryUserName', caption: 'EntryUserName' },
                { key: 'ModifyDate', caption: 'ModifyDate', type: 'datetime' }, { key: 'ModifyUserName', caption: 'ModifyUserName' },
                { key: 'IsApproved', caption: 'IsApproved', type: 'check' }, { key: 'ApprovedDate', caption: 'ApprovedDate', type: 'datetime' },
                { key: 'ApprovalUserName', caption: 'ApprovalUserName' }, { key: 'RemarksHeader', caption: 'RemarksHeader', width: 150 },
                { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'code' }],
            filterRow: true, emptyText: '',
            onSelect: function (r) { GetDetailGrdByHeadId(HRM.int(r.Id)); },
            onDouble: function (r) { if (!rights.update) { HRM.box("ypu don't have updae rights..."); return; } ReadById(HRM.int(r.Id)); },
            onCode: function () { LgsB.noAttachments(); }
        });
        LgsB.trackColumn(grdHistory);
        function act(r, a) {
            if (a === 'Print') { if (!rights.print) { HRM.box("you don't have print rights..."); return; } LgsB.print('lgsb-1305', { id: HRM.int(r.Id) }, function () { return HRM.get(API + '/print-check', { id: r.Id }); }); }
            if (a === 'Voucher') { if (!rights.print) { HRM.box("you don't have print rights..."); return; } voucher102(HRM.int(r.Id)); }
            if (a === 'Edit') { if (!rights.update) { HRM.box("you don't have update rights..."); return; } ReadById(HRM.int(r.Id)); }
            if (a === 'NoOfAttachments') LgsB.noAttachments();
        }
        LgsB.onButton(grdHistory, function (r, a) { act(r, a); });
        LgsB.gridKeys(grdHistory, {
            'ctrl+enter': function (r) { if (!rights.update) { HRM.box("you don't have update rights..."); return; } ReadById(HRM.int(r.Id)); },
            'ctrl+space': function (r) { act(r, LgsB.lastCol(grdHistory)); }
        });
        grdHistoryDetail = new HRM.Grid('grdHistoryDetail', { columns: payColumns(false), totals: true, emptyText: '' });
    }
    function GetDetailGrdByHeadId(id) {
        HRM.get(API + '/history-detail', { id: id }).then(function (rows) { grdHistoryDetail.set(rows || []); }).catch(HRM.fail);
    }
    P.btnshow = function (btn) {                                               // HistoryGridFill
        return HRM.busy(btn || 'btnshow', function () {
            return HRM.post(API + '/history', LgsB.historyFilter()).then(function (rows) {
                grdHistory.set((rows || []).map(function (r) {
                    var c = function (k) { return HRM.col(r, k); };
                    return { Id: c('FreightVoucherOutwardId'), DocNo: c('DocNo'), DocDate: c('DocDate'), DocumentTypeId: c('DocumentTypeId'),
                        GatePassOutwardId: c('GatePassOutwardId'), GpSrNo: c('GpSrNo'), GpDate: c('GpDate'), InvDeliveryOrderId: c('InvDeliveryOrderId'),
                        DeliveryOrderNo: c('DeliveryOrderNo'), DeliveryOrderDate: c('DeliveryOrderDate'), TransporterId: c('TransporterId'),
                        TransporterAccountTitle: c('TransporterAccountTitle'), VehicleNo: c('VehicleNo'), BiltyNo: c('BiltyNo'), BiltyDate: c('BiltyDate'),
                        ChargeToDrAccountId: c('ChargeToDrAccountId'), ChargedToDrAccountTitle: c('ChargedToDrAccountTitle'), LoadingFromCityName: c('LoadingFromCityName'),
                        UnloadingToCityName: c('UnloadingToCityName'), PurchaseOrderHeaderId: c('PurchaseOrderHeaderId'), PurchaseOrderNo: c('PurchaseOrderNo'),
                        NoOfBags: c('NoOfBags'), NetWeightDo: c('NetWeightDo'), NetWeightWb: c('NetWeightWB'), BillWeight: c('NetWeight'), FreightRate: c('FreightRate'),
                        RateUomCode: c('RateUomCode'), QtyforRate: c('QtyforRate'), BiltyFreight: c('BiltyFreight'), OtherCharges: c('OtherCharges'),
                        TotalFreight: c('TotalFreight'), EntryDate: c('EntryDate'), EntryUserName: c('EntryUserName'), ModifyDate: c('ModifyDate'),
                        ModifyUserName: c('ModifyUserName'), IsApproved: c('IsApproved'), ApprovedDate: c('ApprovedDate'), ApprovalUserName: c('ApprovalUserName'),
                        RemarksHeader: c('RemarksHeader'), NoOfAttachments: c('NoOfAttachments') };
                }));
                grdHistoryDetail.clear();
            }).catch(HRM.fail);
        });
    };
    P.btnHistoryRefresh = function () { HRM.setCombo('cmbDateTypeHistory', 0); grdHistory.clear(); grdHistoryDetail.clear(); HRM.focus('cmbDateTypeHistory'); };
    P.BtnRefreshHistory = function () { LgsB.dateType('cmbDateTypeHistory', 'FromDateHistory', 'ToDateHistory', LgsB.yearStart); };

    // ------------------------------------------------------------------------ form load
    var show = LgsB.tabs(function (id) { HRM.focus(id === 'tabPage3' ? 'cmbDateTypeHistory' : 'txtdocdate'); });
    function init() {
        buildPayGrid(); buildExpGrid();
        wire();
        HRM.setVal('txtdocdate', HRM.today()); HRM.setVal('txtBiltyDate', HRM.today()); HRM.setVal('datChequeDate', HRM.today());
        HRM.loading(HRM.get(API + '/setup', { recId: 0 })).then(function (d0) {
            rights = d0.rights || {};
            LgsB.rights(rights, { save: 'btnsave', update: 'btnUpdate', delete: 'btnDelete', print: 'btnPrint' });
            cfg = d0.config || {};
            if (RecId === 0) HRM.setVal('txtdocno', HRM.str(d0.docNo));
            bindLists(d0);
            buildPayGrid(); buildExpGrid(); GenerateRowsInExpenseGrid();
            historyGrids();
            LgsB.dateType('cmbDateTypeHistory', 'FromDateHistory', 'ToDateHistory', d0.yearStart);
            HRM.setVal('FromDateHistory', HRM.addDays(HRM.today(), -(HRM.int(cfg.defaultDays) > 0 ? HRM.int(cfg.defaultDays) : 3)));
            HRM.setVal('ToDateHistory', HRM.today());
            HRM.focus('txtdocdate');
            var id = HRM.int(HRM.param('id'));
            if (id > 0) return ReadById(id);
            return CmbTransporter_Leave();
        }).catch(HRM.fail);
    }
    function wire() {
        LgsB.onLeave('CmbGpNo', CmbGpNo_Leave);
        LgsB.onLeave('CmbDeliveryOrderNo', CmbDeliveryOrderNo_Leave);
        LgsB.onLeave('CmbTransporter', CmbTransporter_Leave);
        LgsB.onLeave('CmbLogisticPoNo', CmbLogisticPoNo_Leave);
        LgsB.onLeave('CmbBillWeightBase', txtNetBillWeight_TextChanged);
        LgsB.onLeave('CmbRateUom', function () { CalculateQtyForRate(); CalculateBiltyFright(); CalculateNetPaid(); });
        LgsB.onLeave('CmbTransactionType', CmbTransactionType_Leave);
        LgsB.onLeave('CmbAccountTitle', CmbAccountTitle_Leave);
        HRM.$('txtNoOfBags').addEventListener('input', txtNoOfBags_TextChanged);
        HRM.$('txtNetWeightDo').addEventListener('input', txtNetBillWeight_TextChanged);
        HRM.$('txtFreightRate').addEventListener('input', function () { CalculateBiltyFright(); CalculateNetPaid(); });
        HRM.$('txtQtyForRate').addEventListener('input', function () { CalculateBiltyFright(); CalculateNetPaid(); });
        HRM.$('txtBiltyFreight').addEventListener('input', CalculateNetPaid);
        HRM.$('txtAmount').addEventListener('input', CalculateNetPaid);
        HRM.footer(function () { show('tabPage3'); HRM.$('boxHistory').scrollIntoView({ block: 'nearest' }); });
        HRM.keys({
            'ctrl+t': function () { if (LgsB.activeTab() === 'tabPage3') show('tabPage1'); else show('tabPage3'); },
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+alt+control': P.btnshortcutkeys, 'ctrl+alt+alt': P.btnshortcutkeys,
            'ctrl+shift+delete': function () { if (LgsB.activeTab() === 'tabPage1' && rights.delete && !HRM.$('btnDelete').disabled) P.btnDelete(); },
            'ctrl+s': function () {
                if (LgsB.activeTab() === 'tabPage3') { P.btnshow(); return; }
                if (HRM.visible('btnsave') && !HRM.$('btnsave').disabled) P.btnsave();
            },
            'ctrl+u': function () { if (LgsB.activeTab() === 'tabPage1' && HRM.visible('btnUpdate') && !HRM.$('btnUpdate').disabled && UpdateMode) P.btnUpdate(); },
            'ctrl+p': function () { if (LgsB.activeTab() === 'tabPage1' && HRM.visible('btnPrint') && !HRM.$('btnPrint').disabled) P.btnPrint(); },
            'ctrl+n': function () { if (LgsB.activeTab() === 'tabPage1') P.btnnew(); },
            'ctrl+r': function () { if (LgsB.activeTab() === 'tabPage1') P.btnRefresh(); },
            'ctrl+f5': function () { if (LgsB.activeTab() === 'tabPage1') HRM.focus('txtdocdate'); },
            'ctrl+f10': function () { if (LgsB.activeTab() === 'tabPage1') LgsB.noAttachments(); },
            'ctrl+arrowdown': function () { var t = LgsB.activeTab() === 'tabPage3' ? 'grdHistoryDetail' : 'grdPaymentDetail'; var tr = HRM.$(t).querySelector('tbody tr'); if (tr) tr.focus(); },
            'ctrl+arrowup': function () { if (LgsB.activeTab() === 'tabPage3') { var tr = HRM.$('grdHistory').querySelector('tbody tr'); if (tr) tr.focus(); } else HRM.focus('txtdocdate'); }
        });
    }
    init();
})();
