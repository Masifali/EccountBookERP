/* ============================================================================================
 * countx_logistics_purchase_order.js - 949 Logistic Purchase Order
 * Architecture.WinApp.Service.lgstcm.frmLogisticPurchaseOrder (DocumentTypeId 1303) + frmLoadLogisticAgreementForPO.
 * Every handler follows the desktop form: validation texts and order, what New / Save / Update / Delete / double-click
 * do to the buttons, the detail entry and grid calculations, the History tab. Exposes window.LgsPO.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/logistics/purchase-order';
    var P = {};
    window.LgsPO = P;

    var RecId = 0, UpdateMode = false, rights = {}, cfg = {}, lists = {}, removed = [], lk = {};
    var D = LgsB.serviceDetail({
        api: API, cfg: function () { return cfg; }, docDate: function () { return HRM.val('txtdocdate'); },
        currentTaxNameId: function () { var r = grd.current(); return r ? HRM.int(r.TaxNameId) : 0; }
    });
    function amt() { return HRM.int(cfg.amountDecimals); }
    function rateDec() { return HRM.int(cfg.fcyRateDecimals) || 4; }

    // ------------------------------------------------------------------------ detail grid (LogisticsPurchaseOrder_Helper.DetailGridCommonSetting)
    function detailColumns(editable) {
        var ed = function (k, t) { return editable ? t : (t === 'edit-num' ? 'num' : 'text'); };
        return [
            { key: 'Id', hidden: true }, { key: 'AgreementHeaderId', hidden: true }, { key: 'AgreementDetailId', hidden: true },
            { key: 'AgreementNo', caption: 'AgreementNo', width: 70 },
            { key: 'PartyRefDocNo', caption: 'PartyRefDocNo', width: 100 },
            { key: 'PartyRefDocDate', caption: 'PartyRefDocDate', type: 'date', width: 80 },
            { key: 'ItemId', hidden: true },
            { key: 'ServiceItemCode', caption: 'ServiceItemCode', width: 70 },
            { key: 'ServiceItemName', caption: 'ServiceItemName', width: 170 },
            { key: 'TransactionCurrencyId', hidden: true },
            { key: 'TransactionCurrency', caption: 'TransactionCurrency', width: 70 },
            { key: 'TransactionExchangeRate', caption: 'TransactionExchangeRate', type: ed('', 'edit-num'), decimals: rateDec(), width: 80 },
            LgsB.numCol('ServiceRate', 'ServiceRate', rateDec()),
            { key: 'RateBaseId', hidden: true }, { key: 'RateBase', caption: 'RateBase', width: 70 }, { key: 'RateBaseEquivalent', hidden: true },
            { key: 'Qty', caption: 'Qty', type: ed('', 'edit-num'), decimals: 2, width: 70 },
            { key: 'NetWeight', caption: 'NetWeight', type: ed('', 'edit-num'), decimals: 2, width: 80 },
            LgsB.numCol('TcyAmount', 'TcyAmount', amt(), { sum: true }),
            LgsB.numCol('LcyAmount', 'LcyAmount', amt(), { sum: true }),
            { key: 'TaxNameId', hidden: true }, { key: 'TaxName', caption: 'TaxName', width: 80 },
            { key: 'Tax%', caption: 'Tax%', type: editable && cfg.taxPercentEditable ? 'edit-num' : 'num', decimals: 3, width: 55 },
            LgsB.numCol('TaxAmount', 'TaxAmount', amt(), { sum: true }),
            LgsB.numCol('TotalTcyAmount', 'TotalTcyAmount', amt(), { sum: true }),
            { key: 'LocationTypeId', hidden: true }, { key: 'LocationType', caption: 'LocationType', width: 80 },
            { key: 'LocationFromId', hidden: true }, { key: 'LocationFrom', caption: 'LocationFrom', width: 100 },
            { key: 'LocationToId', hidden: true }, { key: 'LocationTo', caption: 'LocationTo', width: 100 },
            { key: 'Remarks', caption: 'Remarks', type: editable ? 'edit' : 'text', width: 140 }
        ];
    }
    var grd, grdHistory, grdHistoryDetail;
    function buildDetailGrid() {
        var keep = grd ? grd.rows() : [];
        grd = LgsB.freshGrid('grdCustomerDetail', {
            columns: [LgsB.btnCol('Delete', '', 'X', 20), LgsB.btnCol('Edit', '', 'Edit', 40)].concat(detailColumns(true)),
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
            'ctrl+space': function (r, i) { var c = LgsB.lastCol(grd); if (c === 'Delete') DeleteCustomerDetailRow(r, i); if (c === 'Edit') grdCustomerDetail_DoubleClick(r, i); },
            'ctrl+delete': function (r, i) { DeleteCustomerDetailRow(r, i); },
            'ctrl+enter': function (r, i) { grdCustomerDetail_DoubleClick(r, i); },
            'f1': function (r) { gridLookup(r); }
        });
        if (keep.length) grd.set(keep);
    }
    function redraw() { grd.draw(); }

    /** grdCustomerDetail_CellUpdated. */
    function grdCustomerDetail_CellUpdated(r, key) {
        if (key === 'TransactionExchangeRate' || key === 'Qty' || key === 'NetWeight' || key === 'RateBaseEquivalent') {
            LgsB.rowAmount(r, HRM.int(cfg.amountRound));
            var rb = HRM.int(r.RateBaseId);
            if (key === 'Qty' && rb === 5) HRM.box('Qty Will be 01 In Case Of ' + HRM.str(r.RateBase));
            if (key === 'NetWeight' && (rb === 3 || rb === 4)) HRM.box('NetWeight Is Auto-Calculated In Case Of ' + HRM.str(r.RateBase));
        }
        if (key === 'TcyAmount' || key === 'TaxNameId' || key === 'Tax%') LgsB.rowTax(r, HRM.int(cfg.amountRound));
        redraw();
        txtGlcyRate_TextChanged();
    }

    /** F1 on the current column of the grid (GrdPopUp). */
    function gridLookup(r) {
        var c = LgsB.lastCol(grd);
        if (c === 'TransactionCurrencyId' || c === 'TransactionCurrency') {
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
        if (!D.validate('CmbServiceProvider')) return;
        var dr = { Id: 0, AgreementHeaderId: 0, AgreementDetailId: 0, AgreementNo: 0 };
        D.fillRow(dr);
        grd.add(dr);
        D.reset();
        txtGlcyRate_TextChanged();
    };
    P.BtnUpdateCustmer = function () {                                         // BtnUpdateCustmer_Click
        if (!D.validate('CmbServiceProvider')) return;
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
        if (HRM.int(r.AgreementHeaderId) > 0) { HRM.box('You cannot edit this Row as it is linked with Agreement.'); return; }
        D.loadRow(r, i).catch(HRM.fail);
    }
    function DeleteCustomerDetailRow(r, i) {
        if (D.updateIndex !== -1) { HRM.box('Please Reset the Detail first..'); return; }
        if (HRM.int(r.Id) !== 0) {
            if (!HRM.ask('Are you sure to Delete?')) return;
            removed.push(r);
        }
        grd.remove(i);
        EnableDisableHeaderColumns();
        txtGlcyRate_TextChanged();
    }
    function EnableDisableHeaderColumns() {
        var has = grd.rows().some(function (r) { return HRM.int(r.AgreementHeaderId) > 0; });
        ['CmbServiceType', 'CmbBrokerAgentName', 'CmbServiceProvider'].forEach(function (id) { HRM.enable(id, !has); });
    }
    /** txtGlcyRate_TextChanged: SUM(LcyAmount) / Glcy rate. */
    function txtGlcyRate_TextChanged() {
        var lcy = grd.sum('LcyAmount'), rate = HRM.num(HRM.val('txtGlcyRate'));
        HRM.setVal('txtGlcyAmount', LgsB.net(rate > 0 ? lcy / rate : 0, D.fcyFmt()));
    }
    /** CmbBrokerAgentName_Leave: the party's GL currency into Party Glcy. */
    function CmbBrokerAgentName_Leave() {
        var r = LgsB.row('CmbBrokerAgentName'), cur = HRM.comboVal('CmbBrokerAgentName') > 0 && r ? HRM.int(r.CurrencyId) : 0;
        HRM.setCombo('CmbGlcyCurrency', cur > 0 ? cur : 0);
    }
    /** txtdocdate_ValueChanged: the tax schedule of every row's item on the new date. */
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

    // ------------------------------------------------------------------------ binding
    function bindLists(d) {
        lists = d; cfg = d.config || cfg;
        lk = D.lookups(d.lookups);
        HRM.fill('CmbServiceType', lk.ServiceType, 'Id', 'Name', { zero: '', keep: true });
        HRM.fill('CmbPaymentTerm', lk.PaymentTerm, 'Id', 'Name', { zero: '', keep: true });           // insertDefaultRow, ActivateRow 1
        if (!HRM.comboVal('CmbPaymentTerm') && lk.PaymentTerm.length) HRM.setCombo('CmbPaymentTerm', lk.PaymentTerm[0].Id);
        D.bindLookups(lk);
        LgsB.fillCols('CmbInvoiceNo', d.invoices || [], 'Id', 'InvoiceNo', [], null, { zero: '' });
        var sup = d.suppliers || [];
        ['CmbBrokerAgentName', 'CmbServiceProvider'].forEach(function (id) {
            LgsB.fillCols(id, sup, 'Id', 'CompanyName', ['PartyCode', 'CityName', 'CurrencyCode'], ['CompanyName', 'PartyCode', 'CityName', 'CurrencyCode'], { zero: '' });
        });
        D.bindCurrencies(d.currencies);
        D.cities = d.cities || [];
        D.bindItems(d.items);
        HRM.$('txtTaxPercnt').disabled = !cfg.taxPercentEditable;              // GetConfigurationsFromGlobalandBind
        if (HRM.int(cfg.foreignBaseCurrency) > 0) {
            HRM.setCombo('CmbFcyCurrency', cfg.foreignBaseCurrency);
            if (!(HRM.num(HRM.val('txtFcyExchangeRate')) > 0)) HRM.setVal('txtFcyExchangeRate', LgsB.net(cfg.fcyBaseCurrencyRate, D.fcyFmt()));
        }
        document.querySelectorAll('.lgsb-glcy').forEach(function (e) { e.classList.toggle('is-hidden', !cfg.multiCurrency); });
    }
    function bindHistoryCombos(rows) {                                         // HistoryComboBind
        var st = [], sp = [];
        (rows || []).forEach(function (r) {
            var a = HRM.str(HRM.col(r, 'Activity')), o = { Id: HRM.int(HRM.col(r, 'Id')), Name: HRM.str(HRM.col(r, 'ReferenceName')) };
            if (a === 'ServiceType') st.push(o); else if (a === 'ServiceProvider') sp.push(o);
        });
        if (!(rows || []).length) return;
        HRM.fill('CmbServiceTypeHistory', st, 'Id', 'Name', { zero: '', keep: true });
        HRM.fill('CmbServiceProviderHistory', sp, 'Id', 'Name', { zero: '', keep: true });
    }

    // ------------------------------------------------------------------------ New / Refresh / Save / Update / Delete
    function buttons(mode) {                                                   // save | update | saveas
        HRM.show('btnsave', mode === 'save'); HRM.show('btnUpdate', mode === 'update');
        HRM.show('btnSaveAs', mode === 'saveas'); HRM.show('btnDelete', mode === 'update');
    }
    function FromReset(docNo) {
        RecId = 0; removed = [];
        buttons('save'); UpdateMode = false;
        ['CmbInvoiceNo', 'CmbBrokerAgentName', 'CmbServiceProvider'].forEach(function (id) { HRM.setCombo(id, 0); });
        HRM.setVal('txtDueDays', ''); HRM.setVal('txtRemarksHeader', '');
        D.reset();
        grd.clear();
        EnableDisableHeaderColumns();
        HRM.focus('txtdocdate');
        if (docNo !== undefined) { HRM.setVal('txtdocno', HRM.str(docNo)); return Promise.resolve(); }
        return HRM.get(API + '/setup', { recId: 0 }).then(function (d) { HRM.setVal('txtdocno', HRM.str(d.docNo)); }).catch(HRM.fail);
    }
    P.btnnew = function () { return FromReset(); };
    P.btnRefresh = function (btn) {                                            // btnRefresh_Click
        return HRM.busy(btn || 'btnRefresh', function () {
            return HRM.get(API + '/refresh', { recId: RecId }).then(function (d) { bindLists(d); CmbBrokerAgentName_Leave(); }).catch(HRM.fail);
        });
    };
    function payload() {
        return {
            recId: RecId, docNo: HRM.val('txtdocno'), docDate: HRM.val('txtdocdate') + 'T' + HRM.nowTime() + ':00',
            exportInvoiceId: HRM.comboVal('CmbInvoiceNo'), serviceTypeId: HRM.comboVal('CmbServiceType'),
            brokerAgentId: HRM.comboVal('CmbBrokerAgentName'), serviceProviderId: HRM.comboVal('CmbServiceProvider'),
            paymentTermId: HRM.comboVal('CmbPaymentTerm'), dueDays: HRM.val('txtDueDays'), validityDate: HRM.val('txtEffectiveDate'),
            fcyCurrencyId: HRM.comboVal('CmbFcyCurrency'), fcyExchangeRate: HRM.val('txtFcyExchangeRate'), remarks: HRM.val('txtRemarksHeader'),
            glcyCurrencyId: HRM.comboVal('CmbGlcyCurrency'), glcyRate: HRM.val('txtGlcyRate'), glcyAmount: HRM.val('txtGlcyAmount'),
            rows: grd.rows(), removed: RecId > 0 ? removed : []
        };
    }
    /** Insert(): FormHelper.ValidateControls in the form's order, the validity check, the confirmation, save. */
    function Insert(btn) {
        if (!grd.rows().length) { HRM.box('Detail Record Not Found'); return; }
        function c(id, name) { if (!HRM.comboVal(id)) { HRM.box(name + ' field is required'); HRM.focus(id); return false; } return true; }
        function n(id, name) { if (HRM.num(HRM.val(id)) === 0) { HRM.box(name + ' must be a non-zero number'); HRM.focus(id); return false; } return true; }
        if (!n('txtdocno', 'Doc No') || !c('CmbServiceType', 'Service Type') || !c('CmbBrokerAgentName', 'Bill To Party') ||
            !c('CmbServiceProvider', 'Broker Agent Or Service Provider') || !c('CmbPaymentTerm', 'Payment Term') || !n('txtDueDays', 'Due Days') ||
            !c('CmbFcyCurrency', 'Foreign Currency') || !n('txtFcyExchangeRate', 'Fcy Rate')) return;
        if (cfg.multiCurrency && (!c('CmbGlcyCurrency', 'Glcy Currency') || !n('txtGlcyRate', 'Glcy Rate') || !n('txtGlcyAmount', 'Glcy Amount'))) return;
        if (HRM.val('txtEffectiveDate') < HRM.val('txtdocdate')) { HRM.box('Order Validity Date cannot be less than Document Date'); return; }
        if (!HRM.ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
        var preview = HRM.checked('ChkPreview') && rights.print;
        var win = preview && window.CrystalPrint ? window.CrystalPrint.reserve() : null;
        return HRM.busy(btn, function () {
            return HRM.post(API + '/save', payload()).then(function (d) {
                HRM.box(d.message);
                FromReset(d.docNo);
                if (preview && window.CrystalPrint) window.CrystalPrint.open('lgsb-1303', { id: d.id }, null, win);
            }).catch(function (e) { if (win) window.CrystalPrint.release(win); HRM.fail(e); });
        }, 'po-save');
    }
    P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };
    P.btnSaveAs = function (btn) { RecId = 0; return Insert(btn || 'btnSaveAs'); };
    P.btnUpdate = function (btn) { return Insert(btn || 'btnUpdate'); };
    P.btnDelete = function (btn) {                                             // btnDelete_Click
        if (!(RecId > 0)) { HRM.box('No record found to Delete'); return; }
        if (!HRM.ask('Are you sure to Delete?')) return;
        return HRM.busy(btn || 'btnDelete', function () {
            return HRM.post(API + '/delete?id=' + RecId, {}).then(function (d) { HRM.box(d.message); FromReset(d.docNo); }).catch(HRM.fail);
        });
    };
    /** ReadById(Id). */
    function ReadById(id, saveAs) {
        return HRM.loading(FromReset().then(function () {
            RecId = id;
            return Promise.all([HRM.get(API + '/by-id', { id: id }), HRM.get(API + '/refresh', { recId: id })]);
        }).then(function (res) {
            var o = res[0], h = o.header;
            LgsB.fillCols('CmbInvoiceNo', res[1].invoices || [], 'Id', 'InvoiceNo', [], null, { zero: '' });
            show('tabPage1');
            UpdateMode = true; buttons('update');
            HRM.setVal('txtdocdate', HRM.day(HRM.col(h, 'DocumentDate')));
            HRM.setVal('txtdocno', HRM.str(HRM.col(h, 'DocumentNo')));
            HRM.setCombo('CmbServiceType', HRM.int(HRM.col(h, 'serviceTypeId')));
            HRM.setCombo('CmbInvoiceNo', HRM.int(HRM.col(h, 'ExportInvoiceId')));
            HRM.setCombo('CmbBrokerAgentName', HRM.int(HRM.col(h, 'brokerAgentId')));
            HRM.setCombo('CmbServiceProvider', HRM.int(HRM.col(h, 'serviceProviderId')));
            HRM.setCombo('CmbPaymentTerm', HRM.int(HRM.col(h, 'paymentTermId')));
            HRM.setVal('txtDueDays', HRM.str(HRM.col(h, 'DueDays')));
            HRM.setVal('txtEffectiveDate', HRM.day(HRM.col(h, 'OrderValidityDate')));
            HRM.setCombo('CmbFcyCurrency', HRM.int(HRM.col(h, 'fcyCurrencyId')));
            HRM.setVal('txtFcyExchangeRate', LgsB.net(HRM.col(h, 'fcyExchangeRate'), D.fcyFmt()));
            HRM.setVal('txtRemarksHeader', HRM.str(HRM.col(h, 'RemarksHeader')));
            HRM.setCombo('CmbGlcyCurrency', HRM.int(HRM.col(h, 'GlcyCurrencyId')));
            HRM.setVal('txtGlcyRate', LgsB.net(HRM.col(h, 'GlcyExchangeRate'), D.fcyFmt()));
            HRM.setVal('txtGlcyAmount', LgsB.net(HRM.col(h, 'GlcyAmount'), D.fcyFmt()));
            grd.set(o.detail || []);
            HRM.focus('txtdocdate');
            EnableDisableHeaderColumns();
            txtGlcyRate_TextChanged();
            if (saveAs) { buttons('saveas'); HRM.setVal('txtdocdate', HRM.today()); }         // DataGridHistory_SaveAs
        }).catch(function (e) { HRM.fail(e); }));
    }
    P.btnPrint = function (btn) {                                              // btnPrint_Click -> PurchaseOrderHeader_Slip(RecId)
        return LgsB.print('lgsb-1303', { id: RecId }, function () { return HRM.get(API + '/print-check', { id: RecId }); }, btn || 'btnPrint');
    };
    P.btnDefineItem = function () { HRM.open('/logistics/define-services-item'); };
    var KEYS = [['Ctrl+S', 'For Save When on Entry form and For Show Data when on History Form'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'],
        ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Doc Date'],
        ['Ctrl+F10', 'For Open Attachments'], ['F1', 'For Combo Lookup on Current Column of any Focused Grid'], ['Ctrl+T', 'For Tab Transfer'],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+D', 'For Adding an row in Focused Grid'], ['Ctrl+Delete', 'For Deleting an row of Focused Grid'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On on Doc Date'], ['Ctrl+ArrowRight', 'to change focus from one grid to another'],
        ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    P.btnshortcutkeys = function () { LgsB.shortcuts(KEYS); };

    // ------------------------------------------------------------------------ Load Agreement (frmLoadLogisticAgreementForPO)
    P.BtnAgreement = function () {
        var f = LgsB.f, dt = [];
        LgsB.loader({
            title: 'Load Agreement', caption: 'Pending Agreement For Purchase Order',
            filters: f('From Date', '<input type="date" class="win-textbox" data-f="fromDate">') + f('To Date', '<input type="date" class="win-textbox" data-f="toDate">') +
                f('Doc No From', '<input type="text" class="win-textbox" data-f="fromDocNo" data-guard="integer">') + f('Doc No To', '<input type="text" class="win-textbox" data-f="toDocNo" data-guard="integer">') +
                f('Service Type', '<select class="win-combo dtcombo" id="ldServiceType"></select>') + f('Broker Agent', '<select class="win-combo dtcombo" id="ldBrokerAgent"></select>') +
                f('Service Provider', '<select class="win-combo dtcombo" id="ldServiceProvider"></select>') + f('Item Name', '<select class="win-combo dtcombo" id="ldItemName"></select>'),
            onReady: function (body) {
                body.querySelector('[data-f="fromDate"]').value = HRM.addDays(HRM.today(), -7);
                body.querySelector('[data-f="toDate"]').value = HRM.today();
                return combos();
            },
            refresh: function () { return combos(); },
            reset: function (body) {                                           // btnReset_Click
                body.querySelector('[data-f="fromDate"]').value = LgsB.yearStart || body.querySelector('[data-f="fromDate"]').value;
                body.querySelector('[data-f="fromDocNo"]').value = ''; body.querySelector('[data-f="toDocNo"]').value = '';
                HRM.setCombo('ldBrokerAgent', 0);
            },
            search: function (body) {                                          // PendingDataDbCall
                var q = function (k) { return body.querySelector('[data-f="' + k + '"]').value; };
                return HRM.post(API + '/agreements', {
                    fromDate: q('fromDate'), toDate: q('toDate'), fromDocNo: q('fromDocNo'), toDocNo: q('toDocNo'),
                    serviceTypeId: HRM.comboVal('ldServiceType'), brokerAgentId: HRM.comboVal('ldBrokerAgent'),
                    serviceProviderId: HRM.comboVal('ldServiceProvider'), itemId: HRM.comboVal('ldItemName')
                }).then(function (rows) { dt = rows || []; return dt; });
            },
            rows: function (all) {                                             // GrdDataBind: one row per AgreementHeaderId
                var seen = {}, out = [];
                all.forEach(function (r) { var id = HRM.int(HRM.col(r, 'AgreementHeaderId')); if (seen[id]) return; seen[id] = 1; out.push(Object.assign({}, r)); });
                return out;
            },
            columns: [{ key: 'RecordNo', hidden: true }, { key: 'AgreementHeaderId', hidden: true }, { key: 'DocumentTypeId', hidden: true },
                { key: 'DocumentNo', caption: 'DocumentNo', width: 70 }, { key: 'DocumentDate', caption: 'DocumentDate', type: 'date' },
                { key: 'ServiceTypeId', hidden: true }, { key: 'ServiceType', caption: 'ServiceType', width: 120 }, { key: 'ServiceProviderId', hidden: true },
                { key: 'ServiceProviderName', caption: 'ServiceProviderName', width: 180 }, { key: 'EffectiveDateFrom', caption: 'EffectiveDateFrom', type: 'date' },
                { key: 'EffectiveDateTo', caption: 'EffectiveDateTo', type: 'date' }, { key: 'IsActive', caption: 'IsActive', type: 'check' },
                { key: 'EntryUserName', caption: 'EntryUserName' }, { key: 'EntryDate', caption: 'EntryDate', type: 'date' },
                { key: 'ModifyDate', caption: 'ModifyDate', type: 'date' }, { key: 'ModifyUserName', caption: 'ModifyUserName' },
                { key: 'IsApproved', caption: 'IsApproved', type: 'check' }, { key: 'ApprovedDate', caption: 'ApprovedDate', type: 'date' },
                { key: 'ApprovalUserName', caption: 'ApprovalUserName' }, { key: 'RemarksHeader', caption: 'RemarksHeader', width: 150 },
                { key: 'NoOfAttachments', caption: 'NoOfAttachments' }],
            detailColumns: [{ key: 'PartyRefDocNo', caption: 'PartyRefDocNo' }, { key: 'PartyRefDocDate', caption: 'PartyRefDocDate', type: 'date' },
                { key: 'ItemName', caption: 'ItemName', width: 170 }, { key: 'ItemCode', caption: 'ItemCode', width: 60 },
                { key: 'TransactionCurrencyCode', caption: 'TcyCode', width: 80 }, LgsB.numCol('TransactionExchangeRate', 'TcyExchangeRate', 4),
                LgsB.numCol('ServiceRate', 'ServiceRate', 4), { key: 'RateBaseUom', caption: 'RateBaseUom' },
                LgsB.numCol('TcyAmount', 'TcyAmount', 2), LgsB.numCol('LcyAmount', 'LcyAmount', 2), { key: 'Remarks', caption: 'Remarks', width: 150 }],
            detail: function (row, all) { return all.filter(function (r) { return HRM.int(HRM.col(r, 'AgreementHeaderId')) === HRM.int(HRM.col(row, 'AgreementHeaderId')); }); },
            load: function (checked, all) {                                    // btnLoadOnInvoice_Click_1
                var base = HRM.int(HRM.col(checked[0], 'AgreementHeaderId'));
                for (var i = 0; i < checked.length; i++) if (HRM.int(HRM.col(checked[i], 'AgreementHeaderId')) !== base) { HRM.box('Sorry! You can Only Check rows which have the same DocNo'); return false; }
                return all.filter(function (r) { return HRM.int(HRM.col(r, 'AgreementHeaderId')) === base; });
            },
            done: function (rows) { LoadInGridDetail(rows); txtGlcyRate_TextChanged(); }
        });
        function combos() {
            return HRM.get(API + '/agreement-combos').then(function (rows) {
                var g = { ServiceType: [], ServiceProvider: [], BrokerAgent: [], ServiceItem: [] };
                (rows || []).forEach(function (r) { var a = HRM.str(HRM.col(r, 'Activity')); if (g[a]) g[a].push({ Id: HRM.int(HRM.col(r, 'Id')), Name: HRM.str(HRM.col(r, 'ReferenceName')) }); });
                if (!(rows || []).length) return;
                HRM.fill('ldServiceType', g.ServiceType, 'Id', 'Name', { zero: '', keep: true });
                HRM.fill('ldServiceProvider', g.ServiceProvider, 'Id', 'Name', { zero: '', keep: true });
                HRM.fill('ldItemName', g.ServiceItem, 'Id', 'Name', { zero: '', keep: true });
                HRM.fill('ldBrokerAgent', g.BrokerAgent, 'Id', 'Name', { zero: '', keep: true });
            });
        }
    };
    /** LoadInGridDetail(dt): the agreement rows into the detail (one agreement per order). */
    function LoadInGridDetail(dt) {
        if (!dt || !dt.length) return;
        var existing = null;
        grd.rows().forEach(function (r) { if (!existing && HRM.int(r.AgreementHeaderId) > 0) existing = r; });
        if (existing && HRM.int(existing.AgreementHeaderId) !== HRM.int(HRM.col(dt[0], 'AgreementHeaderId'))) {
            HRM.box('Already loaded rows belong to a different Agreement. You cannot load rows from another Agreement.'); return;
        }
        var h = dt[0];
        HRM.setCombo('CmbServiceType', HRM.int(HRM.col(h, 'ServiceTypeId')));
        HRM.setCombo('CmbBrokerAgentName', HRM.int(HRM.col(h, 'BrokerAgentId'))); CmbBrokerAgentName_Leave();
        HRM.setCombo('CmbServiceProvider', HRM.int(HRM.col(h, 'ServiceProviderId')));
        if (!HRM.comboVal('CmbFcyCurrency')) HRM.setCombo('CmbFcyCurrency', HRM.int(HRM.col(h, 'fcyCurrencyId')));
        if (HRM.comboVal('CmbFcyCurrency') && HRM.num(HRM.val('txtFcyExchangeRate')) === 0) HRM.setVal('txtFcyExchangeRate', LgsB.net(HRM.col(h, 'FcyExchangeRate'), D.fcyFmt()));
        var seen = {};
        grd.rows().forEach(function (r) { seen[HRM.int(r.AgreementDetailId)] = 1; });
        dt.forEach(function (dr) {
            var did = HRM.int(HRM.col(dr, 'DetailId'));
            if (seen[did]) return; seen[did] = 1;
            var tcy = HRM.num(HRM.col(dr, 'TcyAmount')), pct = HRM.col(dr, 'Tax%') !== null ? HRM.num(HRM.col(dr, 'Tax%')) : 0, tax = tcy * pct / 100;
            grd.data.push({
                Id: 0, AgreementHeaderId: HRM.int(HRM.col(dr, 'AgreementHeaderId')), AgreementDetailId: did, AgreementNo: HRM.int(HRM.col(dr, 'DocumentNo')),
                PartyRefDocNo: HRM.str(HRM.col(dr, 'PartyRefDocNo')), PartyRefDocDate: HRM.col(dr, 'PartyRefDocDate'),
                ItemId: HRM.int(HRM.col(dr, 'ServiceItemId')), ServiceItemCode: HRM.str(HRM.col(dr, 'ItemCode')), ServiceItemName: HRM.str(HRM.col(dr, 'ItemName')),
                TransactionCurrencyId: HRM.int(HRM.col(dr, 'transactionCurrencyId')), TransactionCurrency: HRM.str(HRM.col(dr, 'TransactionCurrencyCode')),
                TransactionExchangeRate: HRM.num(HRM.col(dr, 'TransactionExchangeRate')), ServiceRate: HRM.num(HRM.col(dr, 'ServiceRate')),
                RateBaseId: HRM.int(HRM.col(dr, 'serviceRateBaseUomId')), RateBase: HRM.str(HRM.col(dr, 'RateBaseUom')),
                RateBaseEquivalent: HRM.num(HRM.col(dr, 'RateBaseUomEquivalent')), Qty: null, NetWeight: null,
                TcyAmount: tcy, LcyAmount: HRM.num(HRM.col(dr, 'LcyAmount')),
                TaxNameId: HRM.col(dr, 'TaxNameId') !== null ? HRM.int(HRM.col(dr, 'TaxNameId')) : 0, TaxName: HRM.col(dr, 'TaxName') !== null ? HRM.str(HRM.col(dr, 'TaxName')) : '',
                'Tax%': pct, TaxAmount: tax, TotalTcyAmount: tcy + tax,
                LocationTypeId: null, LocationType: null, LocationFromId: null, LocationFrom: null, LocationToId: null, LocationTo: null,
                Remarks: HRM.str(HRM.col(dr, 'Remarks'))
            });
        });
        redraw();
        EnableDisableHeaderColumns();
    }

    // ------------------------------------------------------------------------ History tab
    function historyGrids() {
        grdHistory = new HRM.Grid('grdHistory', {
            columns: [LgsB.btnCol('Edit', 'Edit', 'Edit', 50), LgsB.btnCol('SaveAs', 'Save As', 'Save As', 60), LgsB.btnCol('Print', 'Print', 'Print', 50),
                { key: 'Id', hidden: true }, { key: 'DocumentTypeId', hidden: true },
                { key: 'DocumentNo', caption: 'DocumentNo', width: 70 }, { key: 'DocumentDate', caption: 'DocumentDate', type: 'date' },
                { key: 'ExportInvoiceId', hidden: true }, { key: 'ExportInvoiceNo', caption: 'ExportInvoiceNo', width: 100 },
                { key: 'ServiceTypeId', hidden: true }, { key: 'ServiceType', caption: 'ServiceType', width: 150 },
                { key: 'ServiceProviderId', hidden: true }, { key: 'ServiceProviderName', caption: 'ServiceProviderName', width: 180 },
                { key: 'BrokerAgentId', hidden: true }, { key: 'BrokerAgentName', caption: 'BrokerAgentName', width: 180 },
                { key: 'PaymentTermId', hidden: true }, { key: 'PaymentTerm', caption: 'PaymentTerm' }, { key: 'DueDays', caption: 'DueDays' },
                { key: 'OrderValidityDate', caption: 'OrderValidityDate', type: 'date' }, { key: 'FcyCurrencyId', hidden: true },
                { key: 'FcyCode', caption: 'FcyCode', width: 60 }, LgsB.numCol('FcyExchangeRate', 'FcyExchangeRate', rateDec()),
                { key: 'GlCurrencyCode', caption: 'GlCurrencyCode', width: 60, hidden: !cfg.multiCurrency },
                LgsB.numCol('GlcyExchangeRate', 'GlcyExchangeRate', rateDec(), { hidden: !cfg.multiCurrency }),
                LgsB.numCol('GlcyAmount', 'GlcyAmount', amt(), { hidden: !cfg.multiCurrency }),
                { key: 'RemarksHeader', caption: 'RemarksHeader', width: 150 }, { key: 'EntryDate', caption: 'EntryDate', type: 'datetime' },
                { key: 'EntryUserName', caption: 'EntryUserName' }, { key: 'ModifyDate', caption: 'ModifyDate', type: 'datetime' },
                { key: 'ModifyUserName', caption: 'ModifyUserName' }, { key: 'IsApproved', caption: 'IsApproved', type: 'check' },
                { key: 'ApprovedDate', caption: 'ApprovedDate', type: 'datetime' }, { key: 'ApprovalUserName', caption: 'ApprovalUserName' },
                { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'code' }],
            filterRow: true, emptyText: '',
            onSelect: function (r) { GetDetailGrdByHeadId(HRM.int(r.Id)); },
            onDouble: function (r) { if (!rights.update) { HRM.box("ypu don't have updae rights..."); return; } ReadById(HRM.int(r.Id)); },
            onCode: function () { LgsB.noAttachments(); }
        });
        LgsB.trackColumn(grdHistory);
        LgsB.onButton(grdHistory, function (r, act) {                         // DatagridHistory_ColumnButtonClick
            if (act === 'Print') LgsB.print('lgsb-1303', { id: HRM.int(r.Id) }, function () { return HRM.get(API + '/print-check', { id: r.Id }); });
            if (act === 'Edit') { if (!rights.update) { HRM.box("you don't have update rights..."); return; } ReadById(HRM.int(r.Id)); }
            if (act === 'SaveAs') ReadById(HRM.int(r.Id), true);
        });
        LgsB.gridKeys(grdHistory, {
            'ctrl+enter': function (r) { if (!rights.update) { HRM.box("you don't have update rights..."); return; } ReadById(HRM.int(r.Id)); },
            'ctrl+space': function (r) {
                var c = LgsB.lastCol(grdHistory);
                if (c === 'Edit') { if (!rights.update) { HRM.box("you don't have update rights..."); return; } ReadById(HRM.int(r.Id)); }
                if (c === 'Print') LgsB.print('lgsb-1303', { id: HRM.int(r.Id) }, function () { return HRM.get(API + '/print-check', { id: r.Id }); });
                if (c === 'NoOfAttachments') LgsB.noAttachments();
                if (c === 'SaveAs') ReadById(HRM.int(r.Id), true);
            }
        });
        grdHistoryDetail = new HRM.Grid('grdHistoryDetail', { columns: detailColumns(false), totals: true, emptyText: '' });
    }
    function GetDetailGrdByHeadId(id) {
        HRM.get(API + '/history-detail', { id: id }).then(function (rows) { grdHistoryDetail.set(rows || []); }).catch(HRM.fail);
    }
    /** HistoryGridFill: the filters to FormHistory, the rows into dtHistoryGrid. */
    P.btnshow = function (btn) {
        return HRM.busy(btn || 'btnshow', function () {
            var f = LgsB.historyFilter({ serviceTypeId: HRM.comboVal('CmbServiceTypeHistory'), serviceProviderId: HRM.comboVal('CmbServiceProviderHistory') });
            return HRM.post(API + '/history', f).then(function (rows) {
                grdHistory.set((rows || []).map(function (r) {
                    var c = function (k) { return HRM.col(r, k); };
                    return { Id: c('PurchaseOrderHeaderId'), DocumentTypeId: c('DocumentTypeId'), DocumentNo: c('DocumentNo'), DocumentDate: c('DocumentDate'),
                        ExportInvoiceId: c('ExportInvoiceId'), ExportInvoiceNo: c('ExportInvoiceNo'), ServiceTypeId: c('ServiceTypeId'), ServiceType: c('ServiceType'),
                        ServiceProviderId: c('ServiceProviderId'), ServiceProviderName: c('ServiceProviderName'), BrokerAgentId: c('BrokerAgentId'),
                        BrokerAgentName: c('BrokerAgentName'), PaymentTermId: c('PaymentTermId'), PaymentTerm: c('PaymentTerm'), DueDays: c('DueDays'),
                        OrderValidityDate: c('OrderValidityDate'), FcyCurrencyId: c('FcyCurrencyId'), FcyCode: c('FcyCode'), FcyExchangeRate: c('FcyExchangeRate'),
                        GlCurrencyCode: c('GlCurrencyCode'), GlcyExchangeRate: c('GlcyExchangeRate'), GlcyAmount: c('GlcyAmount'), RemarksHeader: c('RemarksHeader'),
                        EntryDate: c('EntryDate'), EntryUserName: c('EntryUserName'), ModifyDate: c('ModifyDate'), ModifyUserName: c('ModifyUserName'),
                        IsApproved: c('IsApproved'), ApprovedDate: c('ApprovedDate'), ApprovalUserName: c('ApprovalUserName'), NoOfAttachments: c('NoOfAttachments') };
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
        buildDetailGrid();
        D.wire();
        L_wire();
        HRM.setVal('txtdocdate', HRM.today());
        HRM.setVal('txtEffectiveDate', HRM.today());
        HRM.setVal('txtPartyRefDocDate', HRM.today());
        HRM.loading(HRM.get(API + '/setup', { recId: 0 })).then(function (d) {       // InitializeComponentMethod
            rights = d.rights || {};
            LgsB.rights(rights, { save: ['btnsave', 'btnSaveAs'], update: 'btnUpdate', delete: 'btnDelete', print: 'btnPrint' });
            cfg = d.config || {};
            if (RecId === 0) HRM.setVal('txtdocno', HRM.str(d.docNo));
            bindLists(d);
            buildDetailGrid();                                                 // DetailGridCommonSetting with the configuration read
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
    function L_wire() {
        LgsB.onLeave('CmbBrokerAgentName', CmbBrokerAgentName_Leave);
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
            'ctrl+shift+delete': function () { if (LgsB.activeTab() === 'tabPage1' && rights.delete && HRM.visible('btnDelete') && !HRM.$('btnDelete').disabled) P.btnDelete(); },
            'ctrl+s': function () {
                if (LgsB.activeTab() === 'tabPage3') { P.btnshow(); return; }
                if (HRM.visible('btnsave') && !HRM.$('btnsave').disabled) P.btnsave();
            },
            'ctrl+u': function () { if (LgsB.activeTab() === 'tabPage1' && HRM.visible('btnUpdate') && !HRM.$('btnUpdate').disabled && UpdateMode) P.btnUpdate(); },
            'ctrl+p': function () { if (LgsB.activeTab() === 'tabPage1' && !HRM.$('btnPrint').disabled) P.btnPrint(); },
            'ctrl+n': function () { if (LgsB.activeTab() === 'tabPage1') P.btnnew(); },
            'ctrl+r': function () { if (LgsB.activeTab() === 'tabPage1') P.btnRefresh(); },
            'ctrl+f5': function () { HRM.focus('txtdocdate'); },
            'ctrl+f10': LgsB.noAttachments,
            'ctrl+f12': function () { if (HRM.visible('btnSaveAs') && !HRM.$('btnSaveAs').disabled) P.btnSaveAs(); },
            'ctrl+arrowdown': function () { var t = LgsB.activeTab() === 'tabPage3' ? 'grdHistoryDetail' : 'grdCustomerDetail'; var tr = HRM.$(t).querySelector('tbody tr'); if (tr) tr.focus(); },
            'ctrl+arrowup': function () { if (LgsB.activeTab() === 'tabPage3') { var tr = HRM.$('grdHistory').querySelector('tbody tr'); if (tr) tr.focus(); } else HRM.focus('txtdocdate'); }
        });
    }
    init();
})();
