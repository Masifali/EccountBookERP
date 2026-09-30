/* ============================================================================================
 * countx_logistics_services_bill.js - 955 Logistic Purchase Services Bill
 * Architecture.WinApp.Service.lgstcm.frmLogisticPurchaseServicesBill (DocumentTypeId 1304) with its loaders
 * frmLoadPurchaseOrderForServicesBill (Load PO) and frmLoadPendingFreightVoucherExportForServicesBill (Load Freight Voucher).
 * Handlers follow the desktop form: validation texts and order, the PO / freight-voucher modes of the detail grid,
 * New / Save / Update / Delete, the History tab and its Print / Voucher buttons. Exposes window.LgsSB.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/logistics/services-bill';
    var DOC_TYPE = 1304;
    var P = {};
    window.LgsSB = P;

    var RecId = 0, UpdateMode = false, rights = {}, cfg = {}, lk = {}, removed = [];
    var FreightVoucherIds = [], dtFreightDetail = [], invoice = null, debitAccounts = [];
    var D = LgsB.serviceDetail({
        api: API, cfg: function () { return cfg; }, docDate: function () { return HRM.val('txtdocdate'); },
        currentTaxNameId: function () { var r = grd && grd.current(); return r ? HRM.int(r.TaxNameId) : 0; }
    });
    function amt() { return HRM.int(cfg.amountDecimals); }
    function rateDec() { return HRM.int(cfg.fcyRateDecimals) || 4; }
    function freight() { return FreightVoucherIds.length > 0; }
    function addFv(id) { id = HRM.int(id); if (FreightVoucherIds.indexOf(id) < 0) FreightVoucherIds.push(id); }

    // ------------------------------------------------------------------------ detail grid (grdCustomerDetailSetting)
    /**
     * LogisticsServicesBill_Helper.DetailGridCommonSetting is not in the decompiled source: the columns follow the
     * dtCustomerDetail fields the form reads and writes. Editable: Qty / NetWeight / TransactionExchangeRate / Remarks /
     * Tax% (config) - only Remarks / Tax% when rows come from a freight voucher, which also hides the PO / agreement /
     * ref-doc / rate columns. DebitAccountId is a combo of the accounts of types 6, 8, 10 (DetailGridComboBind).
     */
    function detailColumns(editable, fr, history) {
        var ed = function (t) { return editable ? t : (t === 'edit-num' ? 'num' : 'text'); };
        var tax = editable && cfg.taxPercentEditable ? 'edit-num' : 'num';
        var cols = [
            { key: 'Id', hidden: true }, { key: 'ChargesTypeId', hidden: true },
            { key: 'AgreementHeaderId', hidden: true }, { key: 'AgreementDetailId', hidden: true },
            { key: 'AgreementNo', caption: 'AgreementNo', width: 70, hidden: fr },
            { key: 'PurchaseOrderHeaderId', hidden: true }, { key: 'PurchaseOrderDetailId', hidden: true },
            { key: 'PurchaseOrderNo', caption: 'PurchaseOrderNo', width: 70, hidden: fr },
            { key: 'PartyRefDocNo', caption: 'PartyRefDocNo', width: 100, hidden: fr },
            { key: 'PartyRefDocDate', caption: 'PartyRefDocDate', type: 'date', width: 80, hidden: fr },
            { key: 'ItemId', hidden: true },
            { key: 'ServiceItemCode', caption: 'ServiceItemCode', width: 70 },
            { key: 'ServiceItemName', caption: 'ServiceItemName', width: 170 },
            { key: 'TransactionCurrencyId', hidden: true },
            { key: 'TransactionCurrency', caption: 'TransactionCurrency', width: 70 },
            { key: 'TransactionExchangeRate', caption: 'TransactionExchangeRate', type: fr ? 'num' : ed('edit-num'), decimals: rateDec(), width: 80 },
            LgsB.numCol('ServiceRate', 'ServiceRate', rateDec(), { hidden: fr }),
            { key: 'RateBaseId', hidden: true }, { key: 'RateBase', caption: 'RateBase', width: 70, hidden: fr }, { key: 'RateBaseEquivalent', hidden: true },
            { key: 'Qty', caption: 'Qty', type: fr ? 'num' : ed('edit-num'), decimals: 2, width: 70 },
            { key: 'NetWeight', caption: 'NetWeight', type: fr ? 'num' : ed('edit-num'), decimals: 2, width: 80 },
            LgsB.numCol('TcyAmount', 'TcyAmount', amt(), { sum: true }),
            LgsB.numCol('LcyAmount', 'LcyAmount', amt(), { sum: true }),
            { key: 'TaxNameId', hidden: true }, { key: 'TaxName', caption: 'TaxName', width: 80 },
            { key: 'Tax%', caption: 'Tax%', type: tax, decimals: 3, width: 55 },
            LgsB.numCol('TaxAmount', 'TaxAmount', amt(), { sum: true }),
            LgsB.numCol('TotalTcyAmount', 'TotalTcyAmount', amt(), { sum: true })
        ];
        if (history) cols.push({ key: 'DebitAccountId', hidden: true }, { key: 'DebitAccountTitle', caption: 'Debit Account Title', width: 150 });
        else cols.push({ key: 'DebitAccountId', caption: 'Debit Account Title', width: 150, type: editable ? 'select' : 'text',
            options: function () { return [[0, '']].concat(debitAccounts.map(function (a) { return [a.Id, a.AccountTitle]; })); } });
        return cols.concat([
            { key: 'LocationTypeId', hidden: true }, { key: 'LocationType', caption: 'LocationType', width: 80 },
            { key: 'LocationFromId', hidden: true }, { key: 'LocationFrom', caption: 'LocationFrom', width: 100 },
            { key: 'LocationToId', hidden: true }, { key: 'LocationTo', caption: 'LocationTo', width: 100 },
            { key: 'Remarks', caption: 'Remarks', type: editable ? 'edit' : 'text', width: 140 }
        ]);
    }
    var grd, grdHistory, grdHistoryDetail;
    /** BindGrids -> grdCustomerDetailSetting(grdCustomerDetail, FreightVoucherIds.Any()). */
    function BindGrids() {
        var keep = grd ? grd.rows() : [], fr = freight();
        var btns = fr ? [LgsB.btnCol('Delete', '', 'X', 20)] : [LgsB.btnCol('Delete', '', 'X', 20), LgsB.btnCol('Edit', '', 'Edit', 40)];
        grd = LgsB.freshGrid('grdCustomerDetail', {
            columns: btns.concat(detailColumns(true, fr, false)),
            totals: true, emptyText: '',
            onDouble: function (r, i) { grdCustomerDetail_DoubleClick(r, i); },
            onChange: function (r, k) { grdCustomerDetail_CellUpdated(r, k); }
        });
        LgsB.trackColumn(grd);
        LgsB.onButton(grd, function (r, act, i) {                              // grdCustomerDetail_ColumnButtonClick
            if (act === 'Delete') DeleteCustomerDetailRow(r, i);
            if (act === 'Edit') grdCustomerDetail_DoubleClick(r, i);
        });
        LgsB.gridKeys(grd, {                                                   // grdCustomerDetail_KeyDown
            'ctrl+space': function (r, i) { if (LgsB.lastCol(grd) === 'Delete') DeleteCustomerDetailRow(r, i); },
            'ctrl+delete': function (r, i) { DeleteCustomerDetailRow(r, i); },
            'f1': function (r) { gridLookup(r); }
        });
        grd.set(keep);
    }
    function redraw() { grd.draw(); }

    /** grdCustomerDetail_CellUpdated. */
    function grdCustomerDetail_CellUpdated(r, key) {
        if (key === 'TransactionExchangeRate' || key === 'Qty' || key === 'NetWeight' || key === 'RateBaseEquivalent') {
            CustomerDetail_AmountUpdateInRow(r);
            var rb = HRM.int(r.RateBaseId);
            if (key === 'Qty' && rb === 5) HRM.box('Qty Will be 01 In Case Of ' + HRM.str(r.RateBase));
            if (key === 'NetWeight' && (rb === 3 || rb === 4)) HRM.box('NetWeight Is Auto-Calculated In Case Of ' + HRM.str(r.RateBase));
        }
        if (key === 'TcyAmount' || key === 'TaxNameId' || key === 'Tax%') LgsB.rowTax(r, HRM.int(cfg.amountRound));
        redraw();
    }
    /** CustomerDetail_AmountUpdateInRow: freight rows only re-price Lcy = Tcy * rate. */
    function CustomerDetail_AmountUpdateInRow(r) {
        if (freight()) {
            r.LcyAmount = HRM.num(r.TcyAmount) * HRM.num(r.TransactionExchangeRate);
            LgsB.rowTax(r, HRM.int(cfg.amountRound));
        } else LgsB.rowAmount(r, HRM.int(cfg.amountRound));
    }

    /** F1 on the current column (GrdPopUp). */
    function gridLookup(r) {
        var c = LgsB.lastCol(grd);
        if (!freight() && (c === 'TransactionCurrencyId' || c === 'TransactionCurrency')) {
            LgsB.pick('Transaction Currency', D.currencies, [{ key: 'Id', hidden: true }, { key: 'CurrencyCode', caption: 'CurrencyCode', type: 'code' }], function (p) {
                r.TransactionCurrencyId = HRM.int(p.Id); r.TransactionCurrency = HRM.str(p.CurrencyCode); redraw();
            });
        } else if (c === 'TaxNameId' || c === 'TaxName') {
            if (!HRM.int(r.ItemId)) { HRM.box('Service Item Not Found In Current Row'); return; }
            HRM.get(API + '/tax', { itemId: r.ItemId, docDate: HRM.val('txtdocdate') }).then(function (rows) {
                LgsB.pick('Tax Name', rows || [], [{ key: 'Id', hidden: true }, { key: 'TaxType', caption: 'TaxType', type: 'code' }, { key: 'TaxPrcnt', caption: 'TaxPrcnt' }], function (p) {
                    r.TaxNameId = HRM.int(p.Id); r.TaxName = HRM.str(p.TaxType); r['Tax%'] = HRM.num(p.TaxPrcnt);
                    LgsB.rowTax(r, HRM.int(cfg.amountRound)); redraw();
                });
            }).catch(HRM.fail);
        } else if (c === 'DebitAccountId') {
            LgsB.pick('Debit Account', debitAccounts, [{ key: 'Id', hidden: true }, { key: 'AccountTitle', caption: 'AccountTitle', type: 'code', width: 220 }, { key: 'AccountCode', caption: 'AccountCode' }], function (p) {
                r.DebitAccountId = HRM.int(p.Id); redraw();
            });
        } else if (c === 'LocationTypeId' || c === 'LocationType') {
            var was = HRM.int(r.LocationTypeId);
            LgsB.pick('Location Type', D.locTypes, [{ key: 'Id', hidden: true }, { key: 'Name', caption: 'Name', type: 'code' }], function (p) {
                r.LocationTypeId = HRM.int(p.Id); r.LocationType = HRM.str(p.Name);
                if (was !== r.LocationTypeId) { r.LocationFromId = 0; r.LocationFrom = ''; r.LocationToId = 0; r.LocationTo = ''; }
                redraw();
            });
        } else if (c === 'LocationFromId' || c === 'LocationFrom' || c === 'LocationToId' || c === 'LocationTo') {
            var t = HRM.int(r.LocationTypeId);
            if (!t) { HRM.box('Location Type Not Found In Current Row'); return; }
            var from = c.indexOf('From') >= 0;
            D.locations(t).then(function (rows) {
                LgsB.pick(from ? 'Location From' : 'Location To', rows, [{ key: 'Id', hidden: true }, { key: 'LocationName', caption: 'LocationName', type: 'code' }], function (p) {
                    if (from) { r.LocationFromId = HRM.int(p.Id); r.LocationFrom = HRM.str(p.LocationName); }
                    else { r.LocationToId = HRM.int(p.Id); r.LocationTo = HRM.str(p.LocationName); }
                    redraw();
                });
            }).catch(HRM.fail);
        }
    }

    // ------------------------------------------------------------------------ detail entry
    P.BtnAddCustomer = function () {                                           // BtnAddCustomer_Click
        if (freight()) { HRM.box('You cannot add new row as there are rows linked with Freight Voucher in the grid. Please remove those rows to add new row.'); return; }
        if (!grd.rows().length) { HRM.box('No rows available in the customer detail grid.Please load atleast one row in detail grid from Purchase Order'); return; }
        if (!D.validate('CmbServiceProviderOrBrokerAgent')) return;
        var dr = { Id: 0, ChargesTypeId: 19 };
        D.fillRow(dr);
        grd.add(dr);
        D.reset();
        txtGlcyRate_TextChanged();
    };
    P.BtnUpdateCustmer = function () {                                         // BtnUpdateCustmer_Click
        if (!D.validate('CmbServiceProviderOrBrokerAgent')) return;
        var rows = grd.rows();
        if (D.updateIndex < 0 || D.updateIndex >= rows.length) return;
        D.fillRow(rows[D.updateIndex]);
        redraw();
        D.reset();
        txtGlcyRate_TextChanged();
    };
    P.BtnCancelCustomer = function () { D.reset(); };
    function grdCustomerDetail_DoubleClick(r, i) {
        if (!r) return;
        if (freight()) { HRM.box('You cannot Update row linked with Freight Voucher.'); return; }
        if (HRM.int(r.PurchaseOrderHeaderId) > 0 || HRM.int(r.ChargesTypeId) === 18) { HRM.box('You cannot edit this Row as it is linked with Purchase Order.'); return; }
        D.loadRow(r, i).catch(HRM.fail);
    }
    /** DeleteCustomerDetailRow (FreightVoucherIds stay as they are - as in the desktop). */
    function DeleteCustomerDetailRow(r, i) {
        if (D.updateIndex !== -1) { HRM.box('Please Reset the Detail first..'); return; }
        if (HRM.int(r.Id) !== 0) {
            if (!HRM.ask('Are you sure to Delete?')) return;
            removed.push(r);
        }
        grd.remove(i);
    }
    /** txtGlcyRate_TextChanged. */
    function txtGlcyRate_TextChanged() {
        var lcy = grd.sum('LcyAmount'), rate = HRM.num(HRM.val('txtGlcyRate'));
        HRM.setVal('txtGlcyAmount', LgsB.net(rate > 0 ? lcy / rate : 0, D.fcyFmt()));
    }
    /** CmbBrokerAgentName_Leave: the Bill To Party's GL currency into Party Glcy. */
    function CmbBrokerAgentName_Leave() {
        var r = LgsB.row('CmbBillToPartyName'), cur = HRM.comboVal('CmbBillToPartyName') > 0 && r ? HRM.int(r.CurrencyId) : 0;
        HRM.setCombo('CmbGlcyCurrency', cur > 0 ? cur : 0);
    }
    /** txtdocdate_ValueChanged. */
    function txtdocdate_ValueChanged() {
        var rows = grd.rows(); if (!rows.length) return;
        var ids = rows.map(function (r) { return r.ItemId !== null && r.ItemId !== undefined ? ',' + r.ItemId : ''; }).join('');
        HRM.get(API + '/tax-by-items', { itemIds: ids, docDate: HRM.val('txtdocdate') }).then(function (tax) {
            var by = {};
            (tax || []).forEach(function (t) { var k = HRM.int(HRM.col(t, 'ItemId')); if (!(k in by)) by[k] = t; });
            rows.forEach(function (r) {
                var t = by[HRM.int(r.ItemId)];
                if (t) {
                    r.TaxNameId = HRM.int(HRM.col(t, 'TaxNameId')); r.TaxName = HRM.str(HRM.col(t, 'TaxName')); r['Tax%'] = HRM.num(HRM.col(t, 'TaxPercent'));
                    LgsB.rowTax(r, HRM.int(cfg.amountRound));
                } else { r.TaxNameId = 0; r.TaxName = 0; r['Tax%'] = 0; r.TaxAmount = 0; r.TotalTcyAmount = 0; }
            });
            redraw();
        }).catch(HRM.fail);
    }
    /** InvoiceNoBind(InvoiceId, InvoiceNo, InvoiceAccountId): the read-only invoice combo holds just this row. */
    function InvoiceNoBind(id, no, accountId) {
        invoice = { Id: HRM.int(id), InvoiceNo: HRM.str(no), InvoiceAccountId: HRM.int(accountId) };
        LgsB.fillCols('CmbInvoiceNo', [invoice], 'Id', 'InvoiceNo', [], null, { zero: '', keep: false });
        HRM.setCombo('CmbInvoiceNo', invoice.Id);
    }
    function paymentTermEnabled(on) { HRM.enable('CmbPaymentTerm', on); HRM.enable('txtDueDays', on); }

    // ------------------------------------------------------------------------ binding
    function bindLists(d) {
        cfg = d.config || cfg;
        lk = D.lookups(d.lookups);
        HRM.fill('CmbServiceType', lk.ServiceType, 'Id', 'Name', { zero: '', keep: true });
        HRM.fill('CmbPaymentTerm', lk.PaymentTerm, 'Id', 'Name', { zero: '', keep: true });
        if (!HRM.comboVal('CmbPaymentTerm') && lk.PaymentTerm.length) HRM.setCombo('CmbPaymentTerm', lk.PaymentTerm[0].Id);
        D.bindLookups(lk);
        debitAccounts = d.debitAccounts || [];
        var sup = d.suppliers || [];
        ['CmbBillToPartyName', 'CmbServiceProviderOrBrokerAgent'].forEach(function (id) {
            LgsB.fillCols(id, sup, 'Id', 'CompanyName', ['PartyCode', 'CityName', 'CurrencyCode'], ['CompanyName', 'PartyCode', 'CityName', 'CurrencyCode'], { zero: '' });
        });
        D.bindCurrencies(d.currencies);
        D.cities = d.cities || [];
        D.bindItems(d.items);
        if (HRM.int(cfg.foreignBaseCurrency) > 0) {                            // GetConfigurationsFromGlobalandBind
            HRM.setCombo('CmbFcyCurrency', cfg.foreignBaseCurrency);
            if (!(HRM.num(HRM.val('txtFcyExchangeRate')) > 0)) HRM.setVal('txtFcyExchangeRate', LgsB.net(cfg.fcyBaseCurrencyRate, D.fcyFmt()));
        }
        HRM.$('txtTaxPercnt').disabled = !cfg.taxPercentEditable;
        document.querySelectorAll('.lgsb-glcy').forEach(function (e) { e.classList.toggle('is-hidden', !cfg.multiCurrency); });
    }
    function bindHistoryCombos(rows) {                                         // HistoryComboBind
        if (!(rows || []).length) return;
        var st = [], sp = [];
        rows.forEach(function (r) {
            var a = HRM.str(HRM.col(r, 'Activity')), o = { Id: HRM.int(HRM.col(r, 'Id')), Name: HRM.str(HRM.col(r, 'ReferenceName')) };
            if (a === 'ServiceType') st.push(o); else if (a === 'BrokerOrServiceProvider') sp.push(o);
        });
        HRM.fill('CmbServiceTypeHistory', st, 'Id', 'Name', { zero: '', keep: true });
        HRM.fill('CmbServiceProviderHistory', sp, 'Id', 'Name', { zero: '', keep: true });
    }

    // ------------------------------------------------------------------------ New / Refresh / Save / Update / Delete
    function buttons(update) {
        HRM.show('btnsave', !update); HRM.show('btnUpdate', update); HRM.show('btnDelete', update);
    }
    function FromReset(docNo) {
        RecId = 0; removed = []; FreightVoucherIds = []; dtFreightDetail = []; invoice = null;
        buttons(false); UpdateMode = false;
        paymentTermEnabled(false);
        HRM.setVal('txtrefbillno', '');
        LgsB.fillCols('CmbInvoiceNo', [], 'Id', 'InvoiceNo', [], null, { zero: '', keep: false });
        HRM.setCombo('CmbBillToPartyName', 0); HRM.setCombo('CmbServiceProviderOrBrokerAgent', 0);
        ['txtDueDays', 'txtGlcyRate', 'txtGlcyAmount', 'txtRemarksHeader'].forEach(function (i) { HRM.setVal(i, ''); });
        D.reset();
        grd.clear();
        BindGrids();
        HRM.focus('txtdocdate');
        if (docNo !== undefined) { HRM.setVal('txtdocno', HRM.str(docNo)); return Promise.resolve(); }
        return HRM.get(API + '/setup').then(function (d) { HRM.setVal('txtdocno', HRM.str(d.docNo)); }).catch(HRM.fail);
    }
    P.btnnew = function () { return FromReset(); };
    P.btnRefresh = function (btn) {                                            // btnRefresh_Click
        return HRM.busy(btn || 'btnRefresh', function () {
            return HRM.get(API + '/refresh').then(function (d) { bindLists(d); redraw(); CmbBrokerAgentName_Leave(); }).catch(HRM.fail);
        });
    };
    function payload() {
        return {
            recId: RecId, docNo: HRM.val('txtdocno'), docDate: HRM.val('txtdocdate') + 'T' + HRM.nowTime() + ':00',
            serviceTypeId: HRM.comboVal('CmbServiceType'), exportInvoiceId: HRM.comboVal('CmbInvoiceNo'), exportInvoiceNo: LgsB.text('CmbInvoiceNo'),
            billToPartyId: HRM.comboVal('CmbBillToPartyName'), serviceProviderId: HRM.comboVal('CmbServiceProviderOrBrokerAgent'),
            referenceNo: HRM.val('txtrefbillno'), paymentTermId: HRM.comboVal('CmbPaymentTerm'), dueDays: HRM.val('txtDueDays'),
            fcyCurrencyId: HRM.comboVal('CmbFcyCurrency'), fcyExchangeRate: HRM.val('txtFcyExchangeRate'), remarks: HRM.val('txtRemarksHeader'),
            freightVoucherIds: FreightVoucherIds.slice(),
            glcyCurrencyId: HRM.comboVal('CmbGlcyCurrency'), glcyRate: HRM.val('txtGlcyRate'), glcyAmount: HRM.val('txtGlcyAmount'),
            rows: grd.rows(), removed: RecId > 0 ? removed : [], freight: dtFreightDetail
        };
    }
    /** Insert(): ValidateControls, ConfirmAction, ServicesBillHeader.Save, then the previews. */
    function Insert(btn) {
        if (!grd.rows().length) { HRM.box('Detail Record Not Found'); return; }
        function c(id, name) { if (!HRM.comboVal(id)) { HRM.box(name + ' field is required'); HRM.focus(id); return false; } return true; }
        function n(id, name) { if (HRM.num(HRM.val(id)) === 0) { HRM.box(name + ' must be a non-zero number'); HRM.focus(id); return false; } return true; }
        if (!n('txtdocno', 'Doc No') || !c('CmbServiceType', 'Service Type') || !c('CmbPaymentTerm', 'Payment Term') || !n('txtDueDays', 'Due Days') ||
            !c('CmbFcyCurrency', 'Foreign Currency') || !n('txtFcyExchangeRate', 'Fcy Rate')) return;
        if (cfg.multiCurrency && (!c('CmbGlcyCurrency', 'Glcy Currency') || !n('txtGlcyRate', 'Glcy Rate') || !n('txtGlcyAmount', 'Glcy Amount'))) return;
        if (!HRM.ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
        var preview = HRM.checked('ChkPreview') && rights.print, voucher = HRM.checked('chkVoucher') && rights.print;
        var win = preview && window.CrystalPrint ? window.CrystalPrint.reserve() : null;
        var win2 = voucher && window.CrystalPrint ? window.CrystalPrint.reserve() : null;
        return HRM.busy(btn, function () {
            return HRM.post(API + '/save', payload()).then(function (d) {
                HRM.box(d.message);
                FromReset(d.docNo);
                if (preview && window.CrystalPrint) window.CrystalPrint.open('lgsb-1304', { id: d.id }, null, win);
                if (voucher && window.CrystalPrint) {
                    HRM.get(API + '/voucher-id', { id: d.id }).then(function (v) {
                        window.CrystalPrint.open('acc-118', { id: HRM.int(v.voucherHeadId), documentTypeId: DOC_TYPE }, null, win2);
                    }).catch(function (e) { if (win2) window.CrystalPrint.release(win2); HRM.fail(e); });
                }
            }).catch(function (e) {
                if (win) window.CrystalPrint.release(win);
                if (win2) window.CrystalPrint.release(win2);
                HRM.fail(e);
            });
        }, 'sb-save');
    }
    P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };
    P.btnUpdate = function (btn) { return Insert(btn || 'btnUpdate'); };
    P.btnDelete = function (btn) {                                             // btnDelete_Click
        if (!(RecId > 0)) { HRM.box('No record found to Delete'); return; }
        if (!HRM.ask('Are you sure to Delete?')) return;
        return HRM.busy(btn || 'btnDelete', function () {
            return HRM.post(API + '/delete?id=' + RecId, {}).then(function (d) { HRM.box(d.message); FromReset(d.docNo); }).catch(HRM.fail);
        });
    };
    /** ReadById(Id). */
    function ReadById(id) {
        return HRM.loading(FromReset().then(function () {
            RecId = id;
            return HRM.get(API + '/by-id', { id: id });
        }).then(function (o) {
            var h = o.header, c = function (k) { return HRM.col(h, k); };
            show('tabPage1');
            UpdateMode = true; buttons(true);
            HRM.setVal('txtdocdate', HRM.day(c('DocumentDate')));
            HRM.setVal('txtdocno', HRM.str(c('DocumentNo')));
            HRM.setCombo('CmbServiceType', HRM.int(c('serviceTypeId')));
            InvoiceNoBind(c('ExportInvoiceId'), c('ExportInvoiceNo'), c('InvoiceAccountId'));
            HRM.setCombo('CmbBillToPartyName', HRM.int(c('BilltoPartyId')));
            HRM.setCombo('CmbServiceProviderOrBrokerAgent', HRM.int(c('BrokerOrServiceProviderId')));
            HRM.setVal('txtrefbillno', HRM.str(c('ReferenceNo')));
            HRM.setCombo('CmbPaymentTerm', HRM.int(c('paymentTermId')));
            HRM.setVal('txtDueDays', HRM.str(c('DueDays')));
            HRM.setCombo('CmbFcyCurrency', HRM.int(c('fcyCurrencyId')));
            HRM.setVal('txtFcyExchangeRate', LgsB.net(c('fcyExchangeRate'), D.fcyFmt()));
            HRM.setVal('txtRemarksHeader', HRM.str(c('RemarksHeader')));
            HRM.setCombo('CmbGlcyCurrency', HRM.int(c('GlcyCurrencyId')));
            HRM.setVal('txtGlcyRate', LgsB.net(c('GlcyExchangeRate'), D.fcyFmt()));
            HRM.setVal('txtGlcyAmount', LgsB.net(c('GlcyAmount'), D.fcyFmt()));
            FreightVoucherIds = [];
            if (c('FreightVoucherOutwardIds') !== null && c('FreightVoucherOutwardIds') !== undefined) HRM.str(c('FreightVoucherOutwardIds')).split(',').forEach(addFv);
            dtFreightDetail = o.freight || [];
            grd.set(o.detail || []);
            BindGrids();
            HRM.focus('txtdocdate');
            txtGlcyRate_TextChanged();
            paymentTermEnabled(freight());
        }).catch(HRM.fail));
    }
    P.btnPrint = function (btn) {                                              // btnPrint_Click -> ServicesBillHeader_Slip(RecId)
        return LgsB.print('lgsb-1304', { id: RecId }, function () { return HRM.get(API + '/print-check', { id: RecId }); }, btn || 'btnPrint');
    };
    /** btnPrintVoucher_Click: VoucherReport_118(VoucherHeadIdGet(RecId, 1304), 1304). */
    function voucher118(id, docType, btn) {
        var win = window.CrystalPrint ? window.CrystalPrint.reserve() : null;
        return HRM.busy(btn, function () {
            return HRM.get(API + '/voucher-id', { id: id }).then(function (v) {
                if (!window.CrystalPrint) { HRM.box('The print is not available.'); return; }
                var args = { id: HRM.int(v.voucherHeadId) };
                if (docType) args.documentTypeId = docType;
                return window.CrystalPrint.open('acc-118', args, null, win);
            }).catch(function (e) { if (win) window.CrystalPrint.release(win); HRM.fail(e); });
        });
    }
    P.btnPrintVoucher = function (btn) {
        if (!(RecId > 0)) { HRM.box('Record Not Found For Display'); return; }
        return voucher118(RecId, DOC_TYPE, btn || 'btnPrintVoucher');
    };
    var KEYS = [['Ctrl+S', 'For Save When on Entry form and For Show Data when on History Form'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'],
        ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Doc Date'],
        ['Ctrl+F10', 'For Open Attachments'], ['F1', 'For Combo Lookup on Current Column of any Focused Grid'], ['Ctrl+T', 'For Tab Transfer'],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+D', 'For Adding an row in Focused Grid'], ['Ctrl+Delete', 'For Deleting an row of Focused Grid'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On on Doc Date'], ['Ctrl+ArrowRight', 'to change focus from one grid to another'],
        ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    P.btnshortcutkeys = function () { LgsB.shortcuts(KEYS); };

    // ------------------------------------------------------------------------ Load PO (frmLoadPurchaseOrderForServicesBill)
    P.BtnAgreement = function () {
        if (freight()) { HRM.box("Data Already loaded from 'Freight Voucher Export'. You cannot load from Order until you remove those rows."); return; }
        var f = LgsB.f;
        LgsB.loader({
            title: 'Load PO', caption: 'Pending Purchase Order For Services Bill',
            filters: f('From Date', '<input type="date" class="win-textbox" data-f="fromDate">') + f('To Date', '<input type="date" class="win-textbox" data-f="toDate">') +
                f('Doc No From', '<input type="text" class="win-textbox" data-f="fromDocNo" data-guard="integer">') + f('Doc No To', '<input type="text" class="win-textbox" data-f="toDocNo" data-guard="integer">') +
                f('Service Type', '<select class="win-combo dtcombo" id="ldServiceType"></select>') + f('Bill To Party', '<select class="win-combo dtcombo" id="ldBrokerAgent"></select>') +
                f('Broker Agent / Service Provider', '<select class="win-combo dtcombo" id="ldServiceProvider"></select>') + f('Item Name', '<select class="win-combo dtcombo" id="ldItemName"></select>'),
            onReady: function (body) {
                body.querySelector('[data-f="fromDate"]').value = HRM.addDays(HRM.today(), -7);
                body.querySelector('[data-f="toDate"]').value = HRM.today();
                return combos();
            },
            refresh: function () { return combos(); },
            reset: function (body) {                                           // btnReset_Click
                if (LgsB.yearStart) body.querySelector('[data-f="fromDate"]').value = LgsB.yearStart;
                body.querySelector('[data-f="fromDocNo"]').value = ''; body.querySelector('[data-f="toDocNo"]').value = '';
                HRM.setCombo('ldBrokerAgent', 0);
            },
            search: function (body) {                                          // PendingDataDbCall
                var q = function (k) { return body.querySelector('[data-f="' + k + '"]').value; };
                return HRM.post(API + '/pending-po', {
                    fromDate: q('fromDate'), toDate: q('toDate'), fromDocNo: q('fromDocNo'), toDocNo: q('toDocNo'),
                    serviceTypeId: HRM.comboVal('ldServiceType'), billToPartyId: HRM.comboVal('ldBrokerAgent'),
                    serviceProviderId: HRM.comboVal('ldServiceProvider'), itemId: HRM.comboVal('ldItemName')
                });
            },
            rows: function (all) {                                             // GrdDataBind: one row per PurchaseOrderHeaderId
                var seen = {}, out = [];
                all.forEach(function (r) {
                    var id = HRM.int(HRM.col(r, 'PurchaseOrderHeaderId')); if (seen[id]) return; seen[id] = 1;
                    var c = function (k) { return HRM.col(r, k); };
                    out.push({ RecordNo: c('RecordNo'), PurchaseOrderHeaderId: id, DocumentTypeId: c('DocumentTypeId'), DocumentNo: c('DocumentNo'), DocumentDate: c('DocumentDate'),
                        ExportInvoiceId: c('ExportInvoiceId'), ExportInvoiceNo: c('ExportInvoiceNo'), ServiceTypeId: c('ServiceTypeId'), ServiceType: c('ServiceType'),
                        BrokerAgentId: c('BrokerAgentId'), BillToPartyName: c('BillToPartyName'), ServiceProviderId: c('ServiceProviderId'),
                        BrokerOrServiceProviderName: c('BrokerOrServiceProviderName'), OrderValidityDate: c('OrderValidityDate'), PaymentTerm: c('PaymentTerm'),
                        DueDays: c('DueDays'), GlCurrencyCode: c('GlCurrencyCode'), GlcyExchangeRate: c('GlcyExchangeRate'), GlcyAmount: c('GlcyAmount'),
                        EntryDate: c('EntryDate'), EntryUserName: c('EntryUserName'), ModifyDate: c('ModifyDate'), ModifyUserName: c('ModifyUserName'),
                        IsApproved: c('IsApproved'), ApprovedDate: c('ApprovedDate'), ApprovalUserName: c('ApprovalUserName'), Remarks: c('RemarksHeader'),
                        NoOfAttachments: c('NoOfAttachments') });
                });
                return out;
            },
            columns: [{ key: 'RecordNo', hidden: true }, { key: 'PurchaseOrderHeaderId', hidden: true }, { key: 'DocumentTypeId', hidden: true },
                { key: 'DocumentNo', caption: 'DocumentNo', width: 70 }, { key: 'DocumentDate', caption: 'DocumentDate', type: 'date' },
                { key: 'ExportInvoiceId', hidden: true }, { key: 'ExportInvoiceNo', caption: 'ExportInvoiceNo', width: 90 },
                { key: 'ServiceTypeId', hidden: true }, { key: 'ServiceType', caption: 'ServiceType', width: 120 },
                { key: 'BrokerAgentId', hidden: true }, { key: 'BillToPartyName', caption: 'BillToPartyName', width: 200 },
                { key: 'ServiceProviderId', hidden: true }, { key: 'BrokerOrServiceProviderName', caption: 'BrokerOrServiceProviderName', width: 200 },
                { key: 'OrderValidityDate', caption: 'OrderValidityDate', type: 'date' }, { key: 'PaymentTerm', caption: 'PaymentTerm', width: 120 },
                { key: 'DueDays', caption: 'DueDays', width: 80 },
                { key: 'GlCurrencyCode', caption: 'GlCurrencyCode', hidden: !cfg.multiCurrency }, LgsB.numCol('GlcyExchangeRate', 'GlcyExchangeRate', 4, { hidden: !cfg.multiCurrency }),
                LgsB.numCol('GlcyAmount', 'GlcyAmount', 2, { hidden: !cfg.multiCurrency }),
                { key: 'EntryDate', caption: 'EntryDate', type: 'date' }, { key: 'EntryUserName', caption: 'EntryUserName' },
                { key: 'ModifyDate', caption: 'ModifyDate', type: 'date' }, { key: 'ModifyUserName', caption: 'ModifyUserName' },
                { key: 'IsApproved', caption: 'IsApproved', type: 'check' }, { key: 'ApprovedDate', caption: 'ApprovedDate', type: 'date' },
                { key: 'ApprovalUserName', caption: 'ApprovalUserName' }, { key: 'Remarks', caption: 'Remarks', width: 150 },
                { key: 'NoOfAttachments', caption: 'NoOfAttachments' }],
            detailColumns: [{ key: 'AgreementDocNo', caption: 'AgreementDocNo', width: 70 }, { key: 'PartyRefDocNo', caption: 'PartyRefDocNo' },
                { key: 'PartyRefDocDate', caption: 'PartyRefDocDate', type: 'date' }, { key: 'ItemName', caption: 'ItemName', width: 170 },
                { key: 'ItemCode', caption: 'ItemCode', width: 60 }, { key: 'TransactionCurrencyCode', caption: 'TransactionCurrencyCode', width: 80 },
                LgsB.numCol('TransactionExchangeRate', 'TransactionExchangeRate', 4), LgsB.numCol('OrderRate', 'OrderRate', 4),
                { key: 'RateBaseUom', caption: 'RateBaseUom' }, LgsB.numCol('Qty', 'Qty', 2), LgsB.numCol('UsedInInvoiceQty', 'UsedInInvoiceQty', 2),
                LgsB.numCol('BalQty', 'BalQty', 2), LgsB.numCol('TcyAmount', 'TcyAmount', 2), LgsB.numCol('LcyAmount', 'LcyAmount', 2),
                { key: 'TaxName', caption: 'TaxName', width: 100 }, LgsB.numCol('TaxPercent', 'TaxPercent', 3), LgsB.numCol('TaxAmount', 'TaxAmount', 2),
                LgsB.numCol('TotalAmount', 'TotalAmount', 2), { key: 'FromLocation', caption: 'FromLocation', width: 100 },
                { key: 'ToLocation', caption: 'ToLocation', width: 100 }, { key: 'Remarks', caption: 'Remarks', width: 150 }],
            detail: function (row, all) {                                      // DetailGridBind
                return all.filter(function (r) { return HRM.int(HRM.col(r, 'PurchaseOrderHeaderId')) === HRM.int(row.PurchaseOrderHeaderId); }).map(function (r) {
                    var c = function (k) { return HRM.col(r, k); }, port = HRM.int(c('LocationTypeId')) === 16;
                    return { AgreementDocNo: c('AgreementDocNo'), PartyRefDocNo: c('partyRefDocNo'), PartyRefDocDate: c('partyRefDocDate'), ItemName: c('ItemName'),
                        ItemCode: c('ItemCode'), TransactionCurrencyCode: c('TransactionCurrencyCode'), TransactionExchangeRate: c('transactionExchangeRate'),
                        OrderRate: c('OrderRate'), RateBaseUom: c('RateBaseUom'), Qty: c('Qty'), UsedInInvoiceQty: c('UsedInInvoiceQty'), BalQty: c('BalQty'),
                        TcyAmount: c('tcyAmount'), LcyAmount: c('lcyAmount'), TaxName: c('TaxName'), TaxPercent: c('TaxPercent'), TaxAmount: c('TaxAmount'),
                        TotalAmount: c('TotalAmount'), FromLocation: port ? c('LoadingPort') : c('FromCity'), ToLocation: port ? c('DestinationPort') : c('ToCity'),
                        Remarks: c('Remarks') };
                });
            },
            load: function (checked, all) {                                    // btnLoadOnInvoice_Click_1
                var base = HRM.int(checked[0].PurchaseOrderHeaderId);
                for (var i = 0; i < checked.length; i++) if (HRM.int(checked[i].PurchaseOrderHeaderId) !== base) { HRM.box('Sorry! You can Only Check rows which have the same DocNo'); return false; }
                return all.filter(function (r) { return HRM.int(HRM.col(r, 'PurchaseOrderHeaderId')) === base; });
            },
            done: function (rows) { LoadInGridDetail(rows); }
        });
        function combos() {
            return HRM.get(API + '/po-combos').then(function (rows) {
                if (!(rows || []).length) return;
                var g = { ServiceType: [], ServiceProvider: [], BrokerAgentName: [], ServiceItem: [] };
                rows.forEach(function (r) { var a = HRM.str(HRM.col(r, 'Activity')); if (g[a]) g[a].push({ Id: HRM.int(HRM.col(r, 'Id')), Name: HRM.str(HRM.col(r, 'ReferenceName')) }); });
                HRM.fill('ldServiceType', g.ServiceType, 'Id', 'Name', { zero: '', keep: true });
                HRM.fill('ldServiceProvider', g.ServiceProvider, 'Id', 'Name', { zero: '', keep: true });
                HRM.fill('ldItemName', g.ServiceItem, 'Id', 'Name', { zero: '', keep: true });
                HRM.fill('ldBrokerAgent', g.BrokerAgentName, 'Id', 'Name', { zero: '', keep: true });
            });
        }
    };
    /** LoadInGridDetail(dt): the purchase-order rows into the detail (one purchase order per bill). */
    function LoadInGridDetail(dt) {
        if (dt && dt.length) {
            var existing = null;
            grd.rows().forEach(function (r) { if (!existing && HRM.int(r.PurchaseOrderHeaderId) > 0) existing = r; });
            if (existing && HRM.int(existing.PurchaseOrderHeaderId) !== HRM.int(HRM.col(dt[0], 'PurchaseOrderHeaderId'))) {
                HRM.box('Already loaded rows belong to a different PurchaseOrder. You cannot load rows from another PurchaseOrder.'); return;
            }
            var h = dt[0], c = function (k) { return HRM.col(h, k); };
            HRM.setCombo('CmbServiceType', HRM.int(c('ServiceTypeId')));
            InvoiceNoBind(c('ExportInvoiceId'), c('ExportInvoiceNo'), c('InvoiceAccountId'));
            HRM.setCombo('CmbBillToPartyName', HRM.int(c('brokerAgentId')));
            HRM.setCombo('CmbServiceProviderOrBrokerAgent', HRM.int(c('serviceProviderId')));
            if (!HRM.comboVal('CmbFcyCurrency')) HRM.setCombo('CmbFcyCurrency', HRM.int(c('fcyCurrencyId')));
            if (HRM.comboVal('CmbFcyCurrency') && HRM.num(HRM.val('txtFcyExchangeRate')) === 0) HRM.setVal('txtFcyExchangeRate', LgsB.net(c('FcyExchangeRate'), D.fcyFmt()));
            HRM.setCombo('CmbPaymentTerm', HRM.int(c('paymentTermId')));
            HRM.setVal('txtDueDays', LgsB.net(c('DueDays'), '#,#0'));
            HRM.setVal('txtGlcyRate', LgsB.net(c('GlcyExchangeRate'), '#,#0'));
            var seen = {};
            grd.rows().forEach(function (r) { seen[HRM.int(r.PurchaseOrderDetailId)] = 1; });
            dt.forEach(function (dr) {
                var g = function (k) { return HRM.col(dr, k); }, did = HRM.int(g('DetailId'));
                if (seen[did]) return; seen[did] = 1;
                var tcy = HRM.num(g('TcyAmount')), pct = g('TaxPercent') !== undefined ? HRM.num(g('TaxPercent')) : 0, tax = tcy * pct / 100.0;
                var lt = HRM.int(g('LocationTypeId')), port = lt === 16;
                grd.data.push({
                    Id: 0, ChargesTypeId: 18, AgreementHeaderId: g('AgreementHeaderId'), AgreementDetailId: g('AgreementDetailId'),
                    PurchaseOrderHeaderId: g('PurchaseOrderHeaderId'), PurchaseOrderDetailId: did, PurchaseOrderNo: g('DocumentNo'),
                    PartyRefDocNo: g('PartyRefDocNo'), PartyRefDocDate: g('PartyRefDocDate'),
                    ItemId: g('ServiceItemId'), ServiceItemCode: g('ItemCode'), ServiceItemName: g('ItemName'),
                    TransactionCurrencyId: g('transactionCurrencyId'), TransactionCurrency: g('TransactionCurrencyCode'),
                    TransactionExchangeRate: g('TransactionExchangeRate'), ServiceRate: g('OrderRate'),
                    RateBaseId: g('RateBaseUomId'), RateBase: g('RateBaseUom'), RateBaseEquivalent: g('RateBaseUomEquivalent'),
                    Qty: g('BalQty'), NetWeight: g('BalWeight'), TcyAmount: tcy, LcyAmount: g('LcyAmount'),
                    TaxNameId: g('TaxNameId') !== undefined ? g('TaxNameId') : 0, TaxName: g('TaxName') !== undefined ? g('TaxName') : '',
                    'Tax%': pct, TaxAmount: tax, TotalTcyAmount: tcy + tax, DebitAccountId: null,
                    LocationTypeId: lt, LocationType: HRM.str(g('LocationType')),
                    LocationFromId: port ? g('LoadingPortId') : g('FromCityId'), LocationFrom: port ? g('LoadingPort') : g('FromCity'),
                    LocationToId: port ? g('DestinationPortId') : g('ToCityId'), LocationTo: port ? g('DestinationPort') : g('ToCity'),
                    Remarks: g('Remarks')
                });
            });
        }
        BindGrids();
        CmbBrokerAgentName_Leave();
        txtGlcyRate_TextChanged();
    }

    // ------------------------------------------------------------------------ Load Freight Voucher (frmLoadPendingFreightVoucherExportForServicesBill)
    P.BtnLoadFreightVoucher = function () {
        if (grd.rows().some(function (r) { return HRM.int(r.PurchaseOrderHeaderId) > 0; })) {
            HRM.box("Data already loaded from 'Order'. You cannot load from Freight Voucher until you remove those rows."); return;
        }
        var f = LgsB.f;
        LgsB.loader({
            title: 'Load Freight Voucher', caption: 'Pending Freight Voucher (Export) For Allocation Of Charges To Shipment,Lot,or Invoice',
            autoShow: false,
            filters: f('Export Invoice No *', '<select class="win-combo dtcombo" id="ldInvoiceNo"></select>') + f('Customer', '<select class="win-combo dtcombo" id="ldCustomer"></select>') +
                f('Transporter', '<select class="win-combo dtcombo" id="ldTransporter"></select>') +
                '<div class="hrm-field hrm-stack lgsb-w2"><label>&nbsp;</label><div class="lgsb-radios">' +
                '<label><input type="radio" name="ldFvAction" value="0"> All (Pending &amp; Complete)</label>' +
                '<label><input type="radio" name="ldFvAction" value="1" checked> Pending For Voucher Entry</label>' +
                '<label><input type="radio" name="ldFvAction" value="2"> Freight Entry Completed</label></div></div>',
            onReady: function () { return combos(); },
            refresh: function () { return combos(); },
            reset: function () { HRM.setCombo('ldCustomer', 0); HRM.setCombo('ldTransporter', 0); },
            search: function (body) {                                          // PendingDataDbCall
                if (!HRM.comboVal('ldInvoiceNo')) { HRM.box('Please select Invoice No'); return []; }
                var a = body.querySelector('input[name="ldFvAction"]:checked');
                return HRM.post(API + '/pending-freight', { invoiceId: HRM.comboVal('ldInvoiceNo'), customerId: HRM.comboVal('ldCustomer'),
                    transporterId: HRM.comboVal('ldTransporter'), actionId: a ? HRM.int(a.value) : 1 });
            },
            rows: function (all) {                                             // GrdDataBind
                return all.map(function (r) {
                    var c = function (k) { return HRM.col(r, k); };
                    return { Id: c('Id'), DocumentTypeId: c('DocumentTypeId'), DocNo: c('DocNo'), DocDate: c('DocDate'), SupplierCustomerId: c('SupplierCustomerId'),
                        CustomerName: c('CustomerName'), ContractNos: c('ContractNos'), ExImInvoiceId: c('ExImInvoiceId'), ExportInvoiceNo: c('ExportInvoiceNo'),
                        ExportInvoiceDate: c('ExportInvoiceDate'), InvDeliveryOrderId: c('InvDeliveryOrderId'), DeliveryOrderNo: c('DeliveryOrderNo'),
                        DeliveryOrderDate: c('DeliveryOrderDate'), DoQty: c('DoQty'), DoWeight: c('DoWeight'), TotalQty: c('TotalQty'), TotalWeight: c('TotalWeight'),
                        TransporterId: c('TransporterId'), TransporterName: c('TransporterName'), BillToPartyId: c('BillToPartyId'), BillToPartyName: c('BillToPartyName'),
                        GatePassOutwardId: c('GatePassOutwardId'), GpSrNo: c('GpSrNo'), GpDate: c('GpDate'), VehicleNo: c('VehicleNo'), BiltyFreight: c('BiltyFreight'),
                        OtherCharges: c('OtherCharges'), TotalBiltyFreight: c('TotalBiltyFreight'), ChargesToThisInvoice: c('ProportionatedBiltyFreight'),
                        LoadingFromCityName: c('LoadingFromCityName'), UnloadingToCityName: c('UnloadingToCityName'), ServicesBillHeaderId: c('ServicesBillHeaderId'),
                        ServiceBillNo: c('ServiceBillNo'), ServiceBillDate: c('ServiceBillDate'), InvoiceAccountId: c('InvoiceAccountId') };
                });
            },
            columns: [{ key: 'Id', hidden: true }, { key: 'DocumentTypeId', hidden: true }, { key: 'DocNo', caption: 'DocNo', width: 70 },
                { key: 'DocDate', caption: 'DocDate', type: 'date' }, { key: 'SupplierCustomerId', hidden: true }, { key: 'CustomerName', caption: 'CustomerName', width: 160 },
                { key: 'ContractNos', caption: 'ContractNos', width: 100 }, { key: 'ExImInvoiceId', hidden: true }, { key: 'ExportInvoiceNo', caption: 'ExportInvoiceNo', width: 80 },
                { key: 'ExportInvoiceDate', caption: 'ExportInvoiceDate', type: 'date' }, { key: 'InvDeliveryOrderId', hidden: true },
                { key: 'DeliveryOrderNo', caption: 'DeliveryOrderNo', width: 80 }, { key: 'DeliveryOrderDate', caption: 'DeliveryOrderDate', type: 'date' },
                LgsB.numCol('DoQty', 'DoQty', 2), LgsB.numCol('DoWeight', 'DoWeight', 2), LgsB.numCol('TotalQty', 'TotalQty', 2), LgsB.numCol('TotalWeight', 'TotalWeight', 2),
                { key: 'TransporterId', hidden: true }, { key: 'TransporterName', caption: 'TransporterName', width: 180 }, { key: 'BillToPartyId', hidden: true },
                { key: 'BillToPartyName', caption: 'BillToPartyName', width: 180 }, { key: 'GatePassOutwardId', hidden: true }, { key: 'GpSrNo', caption: 'GpSrNo', width: 60 },
                { key: 'GpDate', caption: 'GpDate', type: 'date' }, { key: 'VehicleNo', caption: 'VehicleNo', width: 100 }, LgsB.numCol('BiltyFreight', 'BiltyFreight', 2),
                LgsB.numCol('OtherCharges', 'OtherCharges', 2), LgsB.numCol('TotalBiltyFreight', 'TotalBiltyFreight', 2), LgsB.numCol('ChargesToThisInvoice', 'ChargesToThisInvoice', 2),
                { key: 'LoadingFromCityName', caption: 'LoadingFromCityName', width: 120 }, { key: 'UnloadingToCityName', caption: 'UnloadingToCityName', width: 120 },
                { key: 'ServicesBillHeaderId', hidden: true }, { key: 'ServiceBillNo', caption: 'ServiceBillNo', width: 90 },
                { key: 'ServiceBillDate', caption: 'ServiceBillDate', type: 'date' }, { key: 'InvoiceAccountId', hidden: true }],
            load: function (checked, all) {                                    // btnLoadOnInvoice_Click_1
                var base = HRM.int(checked[0].ExImInvoiceId), ids = [];
                for (var i = 0; i < checked.length; i++) {
                    var r = checked[i];
                    if (HRM.int(r.ExImInvoiceId) !== base) { HRM.box('Sorry! You can Only Check rows which have the same Gp'); return false; }
                    var id = HRM.int(r.Id);
                    if (HRM.int(r.ServicesBillHeaderId) > 0 && !FreightVoucherIds.length && FreightVoucherIds.indexOf(id) < 0) {
                        HRM.box('Sorry! You cannot load row with DocNo ' + HRM.int(r.DocNo) + '.\nIt already exists against another ServicesBill ' + HRM.int(r.ServiceBillNo) + '.');
                        return false;
                    }
                    ids.push(id);
                }
                return all.filter(function (r) { return ids.indexOf(HRM.int(HRM.col(r, 'Id'))) >= 0; });
            },
            done: function (rows) { FreightVoucherDataFromLoader(rows); }
        });
        function combos() {
            return HRM.get(API + '/fv-combos').then(function (rows) {
                if (!(rows || []).length) return;
                var g = { CustomerName: [], Transporter: [], ExportInvoiceNo: [] };
                rows.forEach(function (r) { var a = HRM.str(HRM.col(r, 'Activity')); if (g[a]) g[a].push({ Id: HRM.int(HRM.col(r, 'Id')), Name: HRM.str(HRM.col(r, 'ReferenceName')) }); });
                HRM.fill('ldInvoiceNo', g.ExportInvoiceNo, 'Id', 'Name', { zero: '', keep: true });
                HRM.fill('ldCustomer', g.CustomerName, 'Id', 'Name', { zero: '', keep: true });
                HRM.fill('ldTransporter', g.Transporter, 'Id', 'Name', { zero: '', keep: true });
            });
        }
    };
    /** FreightVoucherDataFromLoader -> LoadInGridDetailFromVoucherByIds -> VoucherSummaryInfoFillInGrid. */
    function FreightVoucherDataFromLoader(dtLoader) {
        if (!dtLoader || !dtLoader.length) return;
        var h = dtLoader[0], inv = HRM.int(HRM.col(h, 'ExImInvoiceId'));
        if (grd.rows().length && HRM.comboVal('CmbInvoiceNo') !== inv) { HRM.box('Already loaded rows belong to a different Invoice. You cannot load rows from another Invoice.'); return; }
        dtLoader.forEach(function (r) { addFv(HRM.col(r, 'Id')); });
        HRM.setCombo('CmbServiceType', 3);
        InvoiceNoBind(inv, HRM.col(h, 'ExportInvoiceNo'), HRM.col(h, 'InvoiceAccountId'));
        HRM.setCombo('CmbBillToPartyName', HRM.int(HRM.col(h, 'BillToPartyId')));
        HRM.get(API + '/freight-by-ids', { invoiceId: inv, ids: FreightVoucherIds.join(',') }).then(function (dt) {
            var seen = {};
            dtFreightDetail.forEach(function (r) { seen[HRM.int(r.FreightVoucherOutwardId)] = 1; });
            (dt || []).forEach(function (dr) {
                var g = function (k) { return HRM.col(dr, k); }, id = HRM.int(g('Id'));
                if (seen[id]) return; seen[id] = 1;
                dtFreightDetail.push({ Id: 0, FreightVoucherOutwardId: id, GatePassOutwardId: g('GatePassOutwardId'), DeliveryOrderId: g('InvDeliveryOrderId'),
                    Qty: HRM.num(g('DoQty')), Weight: HRM.num(g('DoWeight')), FreightRate: HRM.num(g('freightRate')), RateBaseUomId: g('rateUomId'),
                    QtyForRate: HRM.num(g('QtyforRate')), BiltyFreight: HRM.num(g('biltyFreight')), OtherCharges: HRM.num(g('otherCharges')),
                    TotalBiltyFreight: HRM.num(g('totalbiltyfreight')), ThisRowFreightAmount: HRM.num(g('ProportionatedBiltyFreight')), Remarks: '' });
            });
        }).catch(HRM.fail).then(VoucherSummaryInfoFillInGrid);
    }
    /** VoucherSummaryInfoFillInGrid: one transport-service row carrying the sums of the loaded freight rows. */
    function VoucherSummaryInfoFillInGrid() {
        if (!dtFreightDetail.length) { HRM.box('No pending freight voucher data found.'); return; }
        var qty = 0, w = 0, amount = 0;
        dtFreightDetail.forEach(function (r) { qty += HRM.num(r.Qty); w += HRM.num(r.Weight); amount += HRM.num(r.ThisRowFreightAmount); });
        var item = D.item(cfg.defaultTransportServiceItemId), cur = null;
        D.currencies.forEach(function (c) { if (HRM.int(c.Id) === HRM.int(cfg.localBaseCurrency)) cur = c; });
        var acId = invoice ? HRM.int(invoice.InvoiceAccountId) : 0;
        var rows = grd.rows(), dr;
        if (rows.length) dr = rows[0]; else { dr = {}; grd.data.push(dr); }
        dr.ChargesTypeId = 19;
        dr.ItemId = HRM.int(cfg.defaultTransportServiceItemId);
        dr.ServiceItemCode = item ? item.ItemCode : null;
        dr.ServiceItemName = item ? item.ItemName : null;
        dr.TransactionCurrencyId = HRM.int(cfg.localBaseCurrency);
        dr.TransactionCurrency = cur ? cur.CurrencyCode : null;
        dr.TransactionExchangeRate = HRM.num(cfg.localBaseCurrencyRate);
        dr.Qty = qty; dr.NetWeight = w; dr.TcyAmount = amount;
        dr.LcyAmount = amount * HRM.num(cfg.localBaseCurrencyRate);
        dr.TotalTcyAmount = amount;
        if (acId > 0) dr.DebitAccountId = acId;
        BindGrids();
        CmbBrokerAgentName_Leave();
        txtGlcyRate_TextChanged();
        paymentTermEnabled(true);
    }

    // ------------------------------------------------------------------------ History tab
    function historyGrids() {
        grdHistory = new HRM.Grid('grdHistory', {
            columns: [LgsB.btnCol('Edit', 'Edit', 'Edit', 50), LgsB.btnCol('Print', 'Print', 'Print', 50), LgsB.btnCol('Voucher', 'Voucher', 'Voucher', 70),
                { key: 'Id', hidden: true }, { key: 'DocumentTypeId', hidden: true },
                { key: 'DocumentNo', caption: 'DocumentNo', width: 70 }, { key: 'DocumentDate', caption: 'DocumentDate', type: 'date' },
                { key: 'ServiceTypeId', hidden: true }, { key: 'ServiceType', caption: 'ServiceType', width: 150 },
                { key: 'ExportInvoiceId', hidden: true }, { key: 'ExportInvoiceNo', caption: 'ExportInvoiceNo', width: 120 },
                { key: 'BillToPartyId', hidden: true }, { key: 'BillToPartyName', caption: 'BillToPartyName', width: 180 },
                { key: 'BrokerOrServiceProviderId', hidden: true }, { key: 'BrokerAgentOrServiceProvider', caption: 'BrokerAgentOrServiceProvider', width: 180 },
                { key: 'ReferenceNo', caption: 'ReferenceNo', width: 120 }, { key: 'PaymentTermId', hidden: true }, { key: 'PaymentTerm', caption: 'PaymentTerm' },
                { key: 'DueDays', caption: 'DueDays' }, { key: 'FcyCurrencyId', hidden: true }, { key: 'FcyCode', caption: 'FcyCode', width: 60 },
                LgsB.numCol('FcyExchangeRate', 'FcyExchangeRate', rateDec(), { width: 90 }),
                /* HistoryGridSetting hides "GlCurrencyCode" - this grid's column is GlcyCurrencyCode, so it always shows (as in the desktop). */
                { key: 'GlcyCurrencyCode', caption: 'GlcyCurrencyCode', width: 60 },
                LgsB.numCol('GlcyExchangeRate', 'GlcyExchangeRate', rateDec(), { width: 90, hidden: !cfg.multiCurrency }),
                LgsB.numCol('GlcyAmount', 'GlcyAmount', amt(), { width: 120, hidden: !cfg.multiCurrency }),
                { key: 'RemarksHeader', caption: 'RemarksHeader', width: 150 }, { key: 'EntryDate', caption: 'EntryDate', type: 'datetime' },
                { key: 'EntryUserName', caption: 'EntryUserName' }, { key: 'ModifyDate', caption: 'ModifyDate', type: 'datetime' },
                { key: 'ModifyUserName', caption: 'ModifyUserName' }, { key: 'IsApproved', caption: 'IsApproved', type: 'check' },
                { key: 'ApprovedDate', caption: 'ApprovedDate', type: 'datetime' }, { key: 'ApprovalUserName', caption: 'ApprovalUserName' },
                { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'code' }, { key: 'FreightVoucherOutwardIds', hidden: true }],
            filterRow: true, emptyText: '',
            onSelect: function (r) { GetDetailGrdByHeadId(HRM.int(r.Id), HRM.str(r.FreightVoucherOutwardIds) !== ''); },
            onDouble: function (r) { if (!rights.update) { HRM.box("ypu don't have updae rights..."); return; } ReadById(HRM.int(r.Id)); },
            onCode: function () { LgsB.noAttachments(); }
        });
        LgsB.trackColumn(grdHistory);
        function act(r, a) {                                                   // DatagridHistory_ColumnButtonClick / KeyDown
            if (a === 'Print') {
                if (!rights.print) { HRM.box("you don't have print rights..."); return; }
                LgsB.print('lgsb-1304', { id: HRM.int(r.Id) }, function () { return HRM.get(API + '/print-check', { id: r.Id }); });
            }
            if (a === 'Voucher') {
                if (!rights.print) { HRM.box("you don't have print rights..."); return; }
                voucher118(HRM.int(r.Id), 0);                                  // VoucherReport_118(VoucherHeadIdGet(Id, DocumentTypeId)) - no document type passed
            }
            if (a === 'Edit') { if (!rights.update) { HRM.box("you don't have update rights..."); return; } ReadById(HRM.int(r.Id)); }
            if (a === 'NoOfAttachments') LgsB.noAttachments();
        }
        LgsB.onButton(grdHistory, function (r, a) { act(r, a); });
        LgsB.gridKeys(grdHistory, {
            'ctrl+enter': function (r) { if (!rights.update) { HRM.box("you don't have update rights..."); return; } ReadById(HRM.int(r.Id)); },
            'ctrl+space': function (r) { act(r, LgsB.lastCol(grdHistory)); }
        });
        grdHistoryDetail = LgsB.freshGrid('grdHistoryDetail', { columns: detailColumns(false, false, true), totals: true, emptyText: '' });
    }
    /** GetDetailGrdByHeadId(Id, FreightExists) -> grddetailhistorySettings. */
    function GetDetailGrdByHeadId(id, fr) {
        HRM.get(API + '/history-detail', { id: id }).then(function (rows) {
            grdHistoryDetail = LgsB.freshGrid('grdHistoryDetail', { columns: detailColumns(false, fr, true), totals: true, emptyText: '' });
            grdHistoryDetail.set(rows || []);
        }).catch(HRM.fail);
    }
    P.btnshow = function (btn) {                                               // HistoryGridFill
        return HRM.busy(btn || 'btnshow', function () {
            var f = LgsB.historyFilter({ serviceTypeId: HRM.comboVal('CmbServiceTypeHistory'), serviceProviderId: HRM.comboVal('CmbServiceProviderHistory') });
            return HRM.post(API + '/history', f).then(function (rows) {
                grdHistory.set((rows || []).map(function (r) {
                    var c = function (k) { return HRM.col(r, k); };
                    return { Id: c('ServicesBillHeaderId'), DocumentTypeId: c('DocumentTypeId'), DocumentNo: c('DocumentNo'), DocumentDate: c('DocumentDate'),
                        ServiceTypeId: c('ServiceTypeId'), ServiceType: c('ServiceType'), ExportInvoiceId: c('ExportInvoiceId'), ExportInvoiceNo: c('ExportInvoiceNo'),
                        BillToPartyId: c('BillToPartyId'), BillToPartyName: c('BillToPartyName'), BrokerOrServiceProviderId: c('BrokerOrServiceProviderId'),
                        BrokerAgentOrServiceProvider: c('BrokerAgentOrServiceProvider'), ReferenceNo: c('ReferenceNo'), PaymentTermId: c('PaymentTermId'),
                        PaymentTerm: c('PaymentTerm'), DueDays: c('DueDays'), FcyCurrencyId: c('FcyCurrencyId'), FcyCode: c('FcyCode'),
                        FcyExchangeRate: c('FcyExchangeRate'), GlcyCurrencyCode: c('GlcyCurrencyCode'), GlcyExchangeRate: c('GlcyExchangeRate'),
                        GlcyAmount: c('GlcyAmount'), RemarksHeader: c('RemarksHeader'), EntryDate: c('EntryDate'), EntryUserName: c('EntryUserName'),
                        ModifyDate: c('ModifyDate'), ModifyUserName: c('ModifyUserName'), IsApproved: c('IsApproved'), ApprovedDate: c('ApprovedDate'),
                        ApprovalUserName: c('ApprovalUserName'), NoOfAttachments: c('NoOfAttachments'), FreightVoucherOutwardIds: c('FreightVoucherOutwardIds') };
                }));
                grdHistoryDetail.clear();
            }).catch(HRM.fail);
        });
    };
    P.btnHistoryRefresh = function () {                                       // btnHistoryRefresh_Click
        HRM.setCombo('cmbDateTypeHistory', 0); HRM.setCombo('CmbServiceTypeHistory', 0); HRM.setCombo('CmbServiceProviderHistory', 0);
        grdHistory.clear(); grdHistoryDetail.clear(); HRM.focus('cmbDateTypeHistory');
    };
    P.BtnRefreshHistory = function (btn) {                                     // BtnRefreshHistory_Click
        LgsB.dateType('cmbDateTypeHistory', 'FromDateHistory', 'ToDateHistory', LgsB.yearStart);
        return HRM.busy(btn || 'BtnRefreshHistory', function () { return HRM.get(API + '/history-combos').then(bindHistoryCombos).catch(HRM.fail); });
    };

    // ------------------------------------------------------------------------ form load
    var show = LgsB.tabs(function (id) { HRM.focus(id === 'tabPage3' ? 'cmbDateTypeHistory' : 'txtdocdate'); });
    function init() {
        BindGrids();
        D.wire();
        wire();
        HRM.setVal('txtdocdate', HRM.today());
        HRM.setVal('txtPartyRefDocDate', HRM.today());
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {             // InitializeComponentMethod
            rights = d.rights || {};
            LgsB.rights(rights, { save: 'btnsave', update: 'btnUpdate', delete: 'btnDelete', print: 'btnPrint' });
            cfg = d.config || {};
            if (RecId === 0) HRM.setVal('txtdocno', HRM.str(d.docNo));
            bindLists(d);
            BindGrids();
            historyGrids();
            bindHistoryCombos(d.historyCombos);
            LgsB.dateType('cmbDateTypeHistory', 'FromDateHistory', 'ToDateHistory', d.yearStart);
            HRM.setVal('FromDateHistory', HRM.addDays(HRM.today(), -(HRM.int(cfg.defaultDays) > 0 ? HRM.int(cfg.defaultDays) : 3)));
            HRM.setVal('ToDateHistory', HRM.today());
            HRM.focus('txtdocdate');
            var id = HRM.int(HRM.param('id'));
            if (id > 0) ReadById(id);
        }).catch(HRM.fail);
    }
    function wire() {
        LgsB.onLeave('CmbBillToPartyName', CmbBrokerAgentName_Leave);
        HRM.$('txtGlcyRate').addEventListener('input', txtGlcyRate_TextChanged);
        HRM.$('txtdocdate').addEventListener('change', txtdocdate_ValueChanged);
        var inv = HRM.$('CmbInvoiceNo').parentElement;                          // CmbInvoiceNo_MouseHover / MouseLeave
        inv.addEventListener('mouseenter', function () {
            var id = HRM.comboVal('CmbInvoiceNo'); if (!id) return;
            HRM.get(API + '/invoice-info', { id: id }).then(function (d) { if (d && d.found) LgsB.invoiceCard(inv, 'Invoice Preview — ' + LgsB.text('CmbInvoiceNo'), d.row); else LgsB.hideCard(); }).catch(function () { });
        });
        inv.addEventListener('mouseleave', LgsB.hideCardSoon);
        HRM.footer(function () { show('tabPage3'); HRM.$('boxHistory').scrollIntoView({ block: 'nearest' }); });
        HRM.keys({                                                             // frmBillOfMaterial_Manufacturing_KeyDown
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
            'ctrl+arrowdown': function () { var t = LgsB.activeTab() === 'tabPage3' ? 'grdHistoryDetail' : 'grdCustomerDetail'; var tr = HRM.$(t).querySelector('tbody tr'); if (tr) tr.focus(); },
            'ctrl+arrowup': function () { if (LgsB.activeTab() === 'tabPage3') { var tr = HRM.$('grdHistory').querySelector('tbody tr'); if (tr) tr.focus(); } else HRM.focus('txtdocdate'); }
        });
    }
    init();
})();
