/* ============================================================================================
 * countx_export_gd_mapping.js - frmGdBreakUpByInvoiceNew.cs (Architecture.WinApp.Export), screen 952
 * "Goods Declaration (GD) / Bank Invoice & GD Mapping". Main tabs "GD BreakUp" | "Bank Invoice Payment
 * Schedule & GD Mapping", each with Form | History. Every desktop event has its counterpart here;
 * data through /api/export/gd-bank-invoice-mapping (ExportBankGdController -> ExportGdMappingService).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var X = global.ExBG, $id = X.$id, box = X.box, ask = X.ask, val = X.val, setText = X.setText, netD = X.netD, netI = X.netI, fmt = X.fmt, str = X.str, col = X.col;
    var API = '/api/export/gd-bank-invoice-mapping';

    var PERM = { Save: true, Update: true };
    var CFG = { defaultDaysToLessFromHistoryFromDate: 0, allowOneRowPerInvoiceOnGD: false };
    var INVOICES = [], BANKS = [], HIST_BANKS = [];
    var GD = { rows: [], removed: [], updateIndex: -1, recId: 0 };
    var HIST = [];
    var DT_INVOICES = [], GDS = [], PT = [], FIS = [], ADV_HIST_INV = [];
    var ADV = { rows: [], removedIds: '', updateIndex: -1 };
    var ADV_HIST = [];
    var CUR = { gd: -1, hist: -1, adv: -1, advHist: -1 };

    // ------------------------------------------------------------------------------ tabs
    function mainTab(name) {
        document.querySelectorAll('.ex-main-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-main') === name); });
        document.querySelectorAll('.ex-main-panel').forEach(function (p) { p.classList.toggle('is-active', p.id === name); });
        if (name === 'mainGd') X.focus('CmbInvoiceGdBreakUp');
    }
    function tab(group, panelId) {
        X.innerTab(group, panelId, group === 'gd' ? 'btnGdFooterHistory' : 'btnAdvFooterHistory', function (p, onHist) {
            if (group === 'gd') { if (onHist) X.focus('FromDateGdBreakupHistory'); else X.focus('CmbInvoiceGdBreakUp'); }
            /* tabControl3_SelectedIndexChanged is empty on the desktop: the advance history loads only by its Show button */
        });
    }
    function toggleHistory(group) {
        var cur = X.activeInner(group);
        tab(group, group === 'gd' ? (cur === 'gdHistory' ? 'gdForm' : 'gdHistory') : (cur === 'advHistory' ? 'advForm' : 'advHistory'));
    }

    // ------------------------------------------------------------------------------ load
    function load() {
        return X.getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false };
            if (d.config) CFG = d.config;
            $id('BtnSaveGdBreakUp').disabled = !PERM.Save;
            $id('BtnSaveAdvanceAgaintGD').disabled = !PERM.Save;
            ['invoices', 'banks', 'customInvoices', 'historyBanks', 'advHistoryInvoices'].forEach(function (k) { if (d[k + 'Error']) box(d[k + 'Error']); });
            INVOICES = d.invoices || []; bindInvoices();
            BANKS = d.banks || []; bindBanks();
            DT_INVOICES = d.customInvoices || []; bindAdvInvoices();
            HIST_BANKS = d.historyBanks || []; X.bind('CmbBankGdBankHistory', HIST_BANKS, 'Id', 'name', []);
            ADV_HIST_INV = d.advHistoryInvoices || []; X.bind('CmbInvoiceInvoiceAdvanceHistory', ADV_HIST_INV, 'Id', 'name', []);
            var days = netI(CFG.defaultDaysToLessFromHistoryFromDate);
            setText('FromDateGdBreakupHistory', X.daysAgo(days > 0 ? days : 3)); setText('ToDateGdBreakupHistory', X.today());
            setText('datFromDateAdvanceHistory', X.daysAgo(days > 0 ? days : 3)); setText('datToDateAdvanceHistory', X.today());
            setText('datGDDateGdBreakUp', X.today()); setText('datInvoiceDateGdBreakUp', X.today());
            gdRender(); advRender(); ptRender();
            $id('gdFooterInfo').textContent = 'frmGdBreakUpByInvoice (New)  -  Document Type 214';
            X.focus('CmbInvoiceGdBreakUp');
        }).catch(function () { box('Error occurred during database call.'); });
    }

    // ============================================================================== GD BreakUp - combos
    function bindInvoices() { X.bind('CmbInvoiceGdBreakUp', INVOICES, 'Id', 'InvoiceNo', ['BankInvoiceAmount']); invoiceChanged(); }
    function bindBanks() { X.bind('CmbBankNameGdBreakUp', BANKS, 'Id', 'BranchName', ['BankIBANNo']); }
    /* CmbInvoiceGdBreakUp_TextChanged: Invoice Balance = cell [2] (BankInvoiceAmount) of the active row, else "0". */
    function invoiceChanged() { var r = X.findRow(INVOICES, val('CmbInvoiceGdBreakUp')); setText('txtInvoiceBalanceGdBreakUp', r ? str(col(r, 'BankInvoiceAmount')) : '0'); }
    function setInvoice(id, text) {
        if (netI(id) > 0 && !X.findRow(INVOICES, id)) { INVOICES.push({ Id: netI(id), InvoiceNo: text, BankInvoiceAmount: 0 }); X.bind('CmbInvoiceGdBreakUp', INVOICES, 'Id', 'InvoiceNo', ['BankInvoiceAmount']); }
        X.setVal('CmbInvoiceGdBreakUp', netI(id)); invoiceChanged();
    }

    // ============================================================================== GD BreakUp - calculations
    function calcFob() { setText('txtFobValueGdBreakUp', fmt(netD(val('txtGDValueGdBreakUp')) - netD(val('txtFreightGdBreakUp')))); }
    function calcFtt() {
        if (netD(val('txtFTTPrcntGdBreakUp')) > 100) { setText('txtFTTPrcntGdBreakUp', '10'); box('Percent Cant be Greater Than 100'); }
        setText('txtFttAmountGdBreakUp', fmt(netD(val('txtFobValueGdBreakUp')) * netD(val('txtFTTPrcntGdBreakUp')) / 100));
    }
    function calcNet() { var gd = netD(val('txtGDValueGdBreakUp')); setText('txtNetAmountGdBreakUp', gd > 0 ? fmt(gd - netD(val('txtFttAmountGdBreakUp'))) : fmt(0)); }
    /* txtGDValue_TextChanged: FOB (-> FTT -> Net) then Net; txtFreight: FOB (-> FTT -> Net); txtFTTPrcnt: FTT (-> Net) then Net. */
    function onGdValue() { calcFob(); calcFtt(); calcNet(); }
    function onFreight() { calcFob(); calcFtt(); calcNet(); }
    function onFttPercent() { calcFtt(); calcNet(); }

    // ============================================================================== GD BreakUp - grid
    var GD_COLS = [
        { key: '_x', html: function (v, r, i) { return '<button type="button" class="win-x" data-del="' + i + '" title="Delete">X</button>'; }, cls: 'win-cell-btn' },
        'PartyInvoiceNo', { key: 'GDNo', link: true }, { key: 'GDDate', fmt: X.shortDate }, 'InvoiceNo', { key: 'InvoiceDate', fmt: X.shortDate }, 'BankName',
        { key: 'DueDays', num: true, fmt: function (v) { return str(netI(v)); } }, { key: 'ExchangeRate', num: true, fmt: X.fmt4 },
        { key: 'GDValue', num: true, sum: true, fmt: fmt }, { key: 'Freight', num: true, sum: true, fmt: X.fmt2 }, { key: 'FOBValue', num: true, sum: true, fmt: fmt },
        { key: 'FTTPercent', num: true, fmt: fmt }, { key: 'FttAmount', num: true, sum: true, fmt: X.fmt2 }, { key: 'NetAmount', num: true, sum: true, fmt: X.fmt2 }, 'Remarks'
    ];
    function gdRender() { X.drawGrid('gdBody', 'gdFoot', GD.rows, GD_COLS, { cur: CUR.gd }); }
    /* GdBreakUpFormValidation - FormHelper.ValidateControls order. */
    function gdFormValidation() {
        if (!X.hasSel('CmbInvoiceGdBreakUp')) { box('Invoice No field is required'); X.focus('CmbInvoiceGdBreakUp'); return false; }
        if (!val('txtGDNoGdBreakUp').trim()) { box('GD No field is required'); X.focus('txtGDNoGdBreakUp'); return false; }
        if (!val('txtBankInvoiceNoGDBreakUp').trim()) { box('Bank Invoice No field is required'); X.focus('txtBankInvoiceNoGDBreakUp'); return false; }
        if (!/^\s*[+-]?\d+\s*$/.test(val('txtDueDaysGdBreakUp')) || netI(val('txtDueDaysGdBreakUp')) === 0) { box('Due Days must be a non-zero number'); X.focus('txtDueDaysGdBreakUp'); return false; }
        if (netD(val('txtGDValueGdBreakUp')) === 0) { box('GD Value must be a non-zero number'); X.focus('txtGDValueGdBreakUp'); return false; }
        if (netD(val('txtFobValueGdBreakUp')) === 0) { box('FOB Value must be a non-zero number'); X.focus('txtFobValueGdBreakUp'); return false; }
        if (netD(val('txtNetAmountGdBreakUp')) === 0) { box('Net Amount must be a non-zero number'); X.focus('txtNetAmountGdBreakUp'); return false; }
        return true;
    }
    function gdRowFromFields(base) {
        var r = base || {};
        r.Id = r.Id || 0;
        r.InvoiceId = netI(val('CmbInvoiceGdBreakUp')); r.PartyInvoiceNo = X.selText('CmbInvoiceGdBreakUp');
        r.GDNo = val('txtGDNoGdBreakUp').trim(); r.GDDate = val('datGDDateGdBreakUp') || X.today();
        r.InvoiceNo = val('txtBankInvoiceNoGDBreakUp').trim(); r.InvoiceDate = val('datInvoiceDateGdBreakUp') || X.today();
        r.OtherAmount = netD(val('txtInvoiceBalanceGdBreakUp'));
        r.BankId = netI(val('CmbBankNameGdBreakUp')); r.BankName = X.selText('CmbBankNameGdBreakUp');
        r.DueDays = netI(val('txtDueDaysGdBreakUp')); r.ExchangeRate = netD(val('txtExchangeRateGdBreakUp'));
        r.GDValue = netD(val('txtGDValueGdBreakUp')); r.Freight = netD(val('txtFreightGdBreakUp')); r.FOBValue = netD(val('txtFobValueGdBreakUp'));
        r.FTTPercent = netD(val('txtFTTPrcntGdBreakUp')); r.FttAmount = netD(val('txtFttAmountGdBreakUp')); r.NetAmount = netD(val('txtNetAmountGdBreakUp'));
        r.Remarks = val('txtRemarksGdBreakUp').trim();
        return r;
    }
    /* btnAddGDBreakUpDetail_Click */
    function gdAddRow() {
        try {
            if (!gdFormValidation()) return;
            var inv = netI(val('CmbInvoiceGdBreakUp'));
            if (GD.rows.length > 0) {
                for (var i = 0; i < GD.rows.length; i++) if (netI(GD.rows[i].InvoiceId) !== inv) throw new Error('You can only add data against one invoice in the grid');
                if (CFG.allowOneRowPerInvoiceOnGD) throw new Error('You can only one Row Per invoice in the grid');
            }
            if (CFG.allowOneRowPerInvoiceOnGD && netD(val('txtGDValueGdBreakUp')) !== netD(val('txtInvoiceBalanceGdBreakUp'))) throw new Error('GD Value Should be Equal To Invoice Balance');
            GD.rows.push(gdRowFromFields({ Id: 0 })); CUR.gd = -1; gdRender();
            gdResetDetails();
            X.setEnabled('CmbInvoiceGdBreakUp', false);   /* CmbInvoiceGdBreakUp.ReadOnly = true */
        } catch (e) { box(e.message); }
    }
    /* grdGdBreakUp_DoubleClick */
    function gdEditRow(i) {
        var item = GD.rows[i]; if (!item) return;
        GD.updateIndex = i;
        X.getJson(API + '/invoices?recId=' + GD.recId).then(function (rows) { INVOICES = rows || []; bindInvoices(); }).catch(function (e) { box(e.message); }).then(function () {
            setInvoice(item.InvoiceId, item.PartyInvoiceNo);
            setText('txtGDNoGdBreakUp', item.GDNo); setText('datGDDateGdBreakUp', X.isoDate(item.GDDate));
            setText('txtBankInvoiceNoGDBreakUp', item.InvoiceNo); setText('datInvoiceDateGdBreakUp', X.isoDate(item.InvoiceDate));
            X.setVal('CmbBankNameGdBreakUp', item.BankId);
            setText('txtRemarksGdBreakUp', item.Remarks); setText('txtDueDaysGdBreakUp', str(item.DueDays)); setText('txtExchangeRateGdBreakUp', str(item.ExchangeRate));
            setText('txtGDValueGdBreakUp', fmt(item.GDValue)); setText('txtFreightGdBreakUp', fmt(item.Freight)); setText('txtFobValueGdBreakUp', fmt(item.FOBValue));
            setText('txtFTTPrcntGdBreakUp', netD(item.FTTPercent) > 0 ? fmt(item.FTTPercent) : '10');
            setText('txtFttAmountGdBreakUp', fmt(item.FttAmount)); setText('txtNetAmountGdBreakUp', fmt(item.NetAmount));
            X.show('btnAddGDBreakUpDetail', false); X.show('btnUpdateGDBreakUpDetail', true); X.show('btnCancelGDBreakUpDetail', true);
            X.focus('CmbInvoiceGdBreakUp');
        });
    }
    /* btnUpdateGDBreakUpDetail_Click */
    function gdUpdateRow() {
        try {
            if (!gdFormValidation()) return;
            if (GD.updateIndex < 0 || !GD.rows[GD.updateIndex]) return;
            var inv = netI(val('CmbInvoiceGdBreakUp'));
            for (var i = 0; i < GD.rows.length; i++) if (i !== GD.updateIndex && netI(GD.rows[i].InvoiceId) !== inv) throw new Error('You can only add data against one invoice in the grid');
            if (CFG.allowOneRowPerInvoiceOnGD && netD(val('txtGDValueGdBreakUp')) !== netD(val('txtInvoiceBalanceGdBreakUp'))) throw new Error('GD Value Should be Equal To Invoice Balance');
            var cur = GD.rows[GD.updateIndex];
            GD.rows[GD.updateIndex] = gdRowFromFields({ Id: cur.Id });
            gdRender(); gdResetDetails(); X.focus('CmbInvoiceGdBreakUp');
        } catch (e) { box(e.message); }
    }
    function gdCancelRow() { gdResetDetails(); }
    /* DeleteDetailRowGdBreakUp */
    function gdDeleteRow(i) {
        var r = GD.rows[i]; if (!r) return;
        if (netI(r.Id) > 0) { if (!ask('Are you sure to Delete?')) return; GD.removed.push(X.copy(r)); }
        GD.rows.splice(i, 1);
        if (GD.updateIndex === i) gdResetDetails();
        CUR.gd = -1; gdRender();
    }
    /* ResetGdBreakUpDetails */
    function gdResetDetails() {
        GD.updateIndex = -1;
        setText('txtGDNoGdBreakUp', ''); setText('txtBankInvoiceNoGDBreakUp', ''); X.setVal('CmbBankNameGdBreakUp', '0');
        ['txtRemarksGdBreakUp', 'txtDueDaysGdBreakUp', 'txtFreightGdBreakUp', 'txtGDValueGdBreakUp', 'txtFobValueGdBreakUp', 'txtFttAmountGdBreakUp', 'txtNetAmountGdBreakUp'].forEach(function (id) { setText(id, ''); });
        setText('txtFTTPrcntGdBreakUp', '0');
        X.show('btnAddGDBreakUpDetail', true); X.show('btnUpdateGDBreakUpDetail', false); X.show('btnCancelGDBreakUpDetail', false);
        X.focus('txtGDNoGdBreakUp');
    }
    /* FormReset */
    function gdFormReset() {
        gdResetDetails();
        GD.rows = []; GD.removed = []; CUR.gd = -1; gdRender();
        X.setEnabled('CmbInvoiceGdBreakUp', true);
        GD.recId = 0;
        return X.getJson(API + '/invoices?recId=0').then(function (rows) { INVOICES = rows || []; bindInvoices(); X.setVal('CmbInvoiceGdBreakUp', '0'); invoiceChanged(); }).catch(function (e) { box(e.message); });
    }
    function gdNew() { return gdFormReset(); }
    /* BtnRefreshGdBreakUp_Click */
    function gdRefresh(btn) {
        return X.busy(btn, function () {
            return X.getJson(API + '/refresh?recId=' + GD.recId).then(function (d) { d = d || {}; if (d.config) CFG = d.config; INVOICES = d.invoices || []; bindInvoices(); BANKS = d.banks || []; bindBanks(); });
        });
    }
    /* BtnSaveGdBreakUp_Click -> Insert() */
    function gdSave(btn) {
        return X.busy(btn, function () {
            if (GD.rows.length === 0) { tab('gd', 'gdForm'); box('GDBreakUp Detail Record Not Found'); return Promise.resolve(); }
            if (!ask('Are you sure to Save')) return Promise.resolve();
            return X.postJson(API + '/save', { rows: GD.rows, removed: GD.removed }).then(function (d) { box((d && d.message) || 'Record Save Successfully'); return gdFormReset(); }).catch(function (e) { box(e.message); });
        });
    }
    /* ReadyByIdGdBreak(InvoiceId) */
    function gdReadById(invoiceId) {
        GD.recId = netI(invoiceId);
        return X.getJson(API + '/by-invoice?invoiceId=' + GD.recId).then(function (d) {
            d = d || {};
            tab('gd', 'gdForm');
            INVOICES = d.invoices || []; bindInvoices();
            GD.rows = d.rows || []; GD.removed = []; CUR.gd = -1; gdRender();
        }).catch(function (e) { box(e.message); });
    }

    // ============================================================================== GD BreakUp - history
    var HIST_COLS = [
        { key: '_e', html: function (v, r, i) { return PERM.Update ? '<button type="button" class="win-edit" data-edit="' + i + '">Edit</button>' : ''; }, cls: 'win-cell-btn' },
        { key: 'InvoiceNo', link: true }, 'GDNO', { key: 'GDDate', fmt: X.shortDate }, 'BankInvoiceNo', { key: 'BankInvoiceDate', fmt: X.shortDate }, 'BankName',
        { key: 'DueDays', num: true, fmt: function (v) { return str(netI(v)); } }, { key: 'ExchangeRate', num: true, fmt: X.fmt4 }, { key: 'GDValue', num: true, sum: true, fmt: fmt },
        { key: 'Freight', num: true, sum: true, fmt: X.fmt2 }, { key: 'FobValue', num: true, sum: true, fmt: fmt }, { key: 'FTTPercent', num: true, fmt: fmt },
        { key: 'FTTAmount', num: true, sum: true, fmt: X.fmt2 }, { key: 'NetAmount', num: true, sum: true, fmt: X.fmt2 }, 'Remarks',
        { key: '_b', html: function (v, r, i) { return '<button type="button" class="win-edit" data-act="breakup" data-i="' + i + '">BreakUp</button>'; }, cls: 'win-cell-btn' }
    ];
    function histRender() { X.show('gdHistEditTh', PERM.Update); X.drawGrid('gdHistBody', 'gdHistFoot', HIST, HIST_COLS, { cur: CUR.hist, empty: 'gdHistEmpty' }); }
    function gdHistoryShow(btn) {
        return X.busy(btn, function () {
            var q = [];
            if ($id('FromDateGdBreakupHistoryChk').checked && val('FromDateGdBreakupHistory')) q.push('fromDate=' + val('FromDateGdBreakupHistory'));
            if ($id('ToDateGdBreakupHistoryChk').checked && val('ToDateGdBreakupHistory')) q.push('toDate=' + val('ToDateGdBreakupHistory'));
            q.push('bankId=' + netI(val('CmbBankGdBankHistory')));
            return X.getJson(API + '/history?' + q.join('&')).then(function (rows) { HIST = rows || []; CUR.hist = -1; histRender(); }).catch(function (e) { box(e.message); });
        });
    }
    function gdHistoryReset() { setText('FromDateGdBreakupHistory', X.today()); setText('ToDateGdBreakupHistory', X.today()); X.setVal('CmbBankGdBankHistory', '0'); HIST = []; CUR.hist = -1; histRender(); X.show('gdHistEmpty', false); }
    function gdHistoryRefresh(btn) { return X.busy(btn, function () { return X.getJson(API + '/history-banks').then(function (rows) { HIST_BANKS = rows || []; X.bind('CmbBankGdBankHistory', HIST_BANKS, 'Id', 'name', []); }).catch(function (e) { box(e.message); }); }); }
    /* grdGDhistory "ContainerBreakUp" -> GdContainerBreakUp(GdIdFromBreakUp = Id): that form is not part of this port. */
    function containerBreakUp(i) { var r = HIST[i]; if (!r) return; window.open('/export/gd-container-break-up?gdId=' + netI(col(r, 'Id')), '_blank'); }
    function fiOpening() { window.open('/export/fi-opening', '_blank'); }

    // ============================================================================== Bank Invoice Payment Schedule & GD Mapping
    /* InvoicesNoBindAdvanceUtilize: distinct Id / InvoiceNo of dtInvoices. */
    function bindAdvInvoices() {
        var seen = {}, rows = [];
        DT_INVOICES.forEach(function (r) { var id = netI(col(r, 'Id')); if (!seen[id]) { seen[id] = 1; rows.push({ Id: id, InvoiceNo: str(col(r, 'InvoiceNo')) }); } });
        X.bind('CmbInvoiceAdvanceUtilize', rows, 'Id', 'InvoiceNo', []);
    }
    /* GDsNoBindAdvanceUtilize: the GDs of dtInvoices for the chosen invoice (GdId, GDNO, GDValue, DocumentTypeId, GdDocTypeId). */
    function bindGds() {
        var inv = netI(val('CmbInvoiceAdvanceUtilize'));
        if (inv > 0) {
            var rows = DT_INVOICES.filter(function (r) { return netI(col(r, 'Id')) === inv; });
            if (!rows.length) return;
            GDS = rows.map(function (r) { return { GdId: netI(col(r, 'GdId')), GDNO: str(col(r, 'GDNO')), GDValue: netD(col(r, 'GDValue')), DocumentTypeId: netI(col(r, 'DocumentTypeId')), GdDocTypeId: netI(col(r, 'GdDocTypeId')) }; });
            X.bind('CmbGdNoAdvanceUtilize', GDS, 'GdId', 'GDNO', ['GDValue']);
        } else { GDS = []; X.bind('CmbGdNoAdvanceUtilize', GDS, 'GdId', 'GDNO', []); }
    }
    /* FinancialInstrumentBindAdvanceUtilize(PaymentTermId): the payment-term rows -> (Id, FINo, FcyAmount, DocumentTypeId). */
    function bindFis(paymentTermId) {
        FIS = PT.filter(function (r) { return netI(r.PaymentTermId) === paymentTermId; }).map(function (r) { return { Id: netI(r.ExImEFormRegistrationId), FINo: str(r.FinancialInstrumentNo), FcyAmount: netD(r.FcyAmount), DocumentTypeId: netI(r.DocumentTypeId) }; });
        X.bind('CmbFIAdvanceUtlize', FIS, 'Id', 'FINo', ['FcyAmount']);
        fiChanged();
    }
    /* PaymentTermGdUtilizeFill: distinct PaymentTermId / PaymentTerm of dtPaymentTerm. */
    function bindPaymentTerms() {
        var seen = {}, rows = [];
        PT.forEach(function (r) { var id = netI(r.PaymentTermId); if (!seen[id]) { seen[id] = 1; rows.push({ PaymentTermId: id, PaymentTerm: str(r.PaymentTerm) }); } });
        X.bind('CmbPaymentTermGdUtilize', rows, 'PaymentTermId', 'PaymentTerm', []);
    }
    var PT_COLS = ['PaymentTerm', 'FinancialInstrumentNo', { key: 'PrcntOfTotal', num: true, sum: true, fmt: function (v) { return X.fmtHash(v, 2); } }, { key: 'FcyAmount', num: true, sum: true, fmt: function (v) { return X.fmtHash(v, 3); } }, { key: 'DueDays', num: true, fmt: function (v) { return str(netI(v)); } }];
    function ptRender() { X.drawGrid('ptBody', 'ptFoot', PT, PT_COLS, {}); }
    /* CmbInvoiceAdvanceUtilize_Leave */
    function advInvoiceLeave() {
        var inv = netI(val('CmbInvoiceAdvanceUtilize'));
        bindGds();
        if (inv > 0) {
            return X.getJson(API + '/payment-terms?invoiceId=' + inv).then(function (rows) { PT = rows || []; ptRender(); bindPaymentTerms(); }).catch(function (e) { box(e.message); });
        }
        PT = []; ptRender(); X.bind('CmbPaymentTermGdUtilize', [], 'PaymentTermId', 'PaymentTerm', []);
        return Promise.resolve();
    }
    /* CmbGdNoAdvanceUtilize_Leave: GD Value = GDValue - utilised in the grid for that GD. */
    function gdNoChanged() {
        var g = X.findRow(GDS, val('CmbGdNoAdvanceUtilize'), 'GdId');
        if (!g) { setText('txtGdValueAdvanceUtlize', ''); return; }
        var gdId = netI(val('CmbGdNoAdvanceUtilize'));
        setText('txtGdValueAdvanceUtlize', str(netD(g.GDValue) - X.sum(ADV.rows, 'UtilizeAmount', function (r) { return netI(r.GDId) === gdId; })));
    }
    /* CmbPaymentTermGdUtilize_Leave: FI combo (term 1 only); Utilize Amount = term FcyAmount total - already utilised for the term. */
    function paymentTermLeave() {
        FIS = []; X.bind('CmbFIAdvanceUtlize', FIS, 'Id', 'FINo', []); setText('txtFIBalanceAdvanceUtlize', '');
        if (!X.hasSel('CmbPaymentTermGdUtilize')) return;
        var ptId = netI(val('CmbPaymentTermGdUtilize'));
        if (ptId === 1) bindFis(ptId);
        var total = X.sum(PT, 'FcyAmount', function (r) { return netI(r.PaymentTermId) === ptId; });
        var used = X.sum(ADV.rows, 'UtilizeAmount', function (r) { return netI(r.PaymentTermId) === ptId; });
        setText('txtAdvanceUtlize', str(total - used));
    }
    /* CmbFIAdvanceUtlize_TextChanged / Leave: Balance = FcyAmount - utilised in the grid for that FI. */
    function fiChanged() {
        var f = X.findRow(FIS, val('CmbFIAdvanceUtlize'));
        if (!f) { setText('txtFIBalanceAdvanceUtlize', ''); return; }
        var fiId = netI(val('CmbFIAdvanceUtlize'));
        setText('txtFIBalanceAdvanceUtlize', str(netD(f.FcyAmount) - X.sum(ADV.rows, 'UtilizeAmount', function (r) { return netI(r.FIId) === fiId; })));
    }
    var ADV_COLS = [
        { key: '_x', html: function (v, r, i) { return '<button type="button" class="win-x" data-del="' + i + '" title="Delete">X</button>'; }, cls: 'win-cell-btn' },
        'InvoiceNo', { key: 'GDNo', link: true }, 'PaymentTerm', 'FINo', { key: 'UtilizeAmount', num: true, sum: true, fmt: X.fmt2 }
    ];
    function advRender() { X.drawGrid('advBody', 'advFoot', ADV.rows, ADV_COLS, { cur: CUR.adv }); }
    /* AdvanceUtilizeFormValidation */
    function advFormValidation() {
        if (!X.hasSel('CmbInvoiceAdvanceUtilize')) { box('Invoice No field is required'); X.focus('CmbInvoiceAdvanceUtilize'); return false; }
        if (!X.hasSel('CmbGdNoAdvanceUtilize')) { box('GD No field is required'); X.focus('CmbGdNoAdvanceUtilize'); return false; }
        if (!X.hasSel('CmbPaymentTermGdUtilize')) { box('Payment Term field is required'); X.focus('CmbPaymentTermGdUtilize'); return false; }
        if (netD(val('txtAdvanceUtlize')) === 0) { box('Utilize Amount must be a non-zero number'); X.focus('txtAdvanceUtlize'); return false; }
        return true;
    }
    function n2(v) { return netD(v).toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }); }
    /* btnAddAdvanceUtlize_Click */
    function advAddRow() {
        try {
            if (!advFormValidation()) return;
            var ptId = netI(val('CmbPaymentTermGdUtilize'));
            if (ptId === 1 && !X.hasSel('CmbFIAdvanceUtlize')) { box('Financial Instrument field is required...'); X.focus('CmbFIAdvanceUtlize'); return; }
            for (var i = 0; i < ADV.rows.length; i++) if (netI(ADV.rows[i].InvoiceId) !== netI(val('CmbInvoiceAdvanceUtilize'))) { box('Record cannot be add against multi invoice'); return; }
            var utilizeAmount = netD(val('txtAdvanceUtlize')), gdBalance = netD(val('txtGdValueAdvanceUtlize'));
            if (utilizeAmount > gdBalance) { box('Utilize Amount (' + n2(utilizeAmount) + ') cannot exceed GD Balance (' + n2(gdBalance) + ') for the selected GD.'); return; }
            if (ptId === 1) { var fiBalance = netD(val('txtFIBalanceAdvanceUtlize')); if (utilizeAmount > fiBalance) { box('Utilize Amount (' + n2(utilizeAmount) + ') cannot exceed FI Balance (' + n2(fiBalance) + ') for the selected FI.'); return; } }
            var g = X.findRow(GDS, val('CmbGdNoAdvanceUtilize'), 'GdId') || {}, f = X.findRow(FIS, val('CmbFIAdvanceUtlize')) || {};
            ADV.rows.push({
                Id: 0, InvoiceId: netI(val('CmbInvoiceAdvanceUtilize')), InvoiceNo: X.selText('CmbInvoiceAdvanceUtilize'),
                GDId: netI(val('CmbGdNoAdvanceUtilize')), GDNo: X.selText('CmbGdNoAdvanceUtilize'), PaymentTermId: ptId, PaymentTerm: X.selText('CmbPaymentTermGdUtilize'),
                DocumentTypeId: netI(g.GdDocTypeId), FIId: netI(val('CmbFIAdvanceUtlize')), FINo: X.selText('CmbFIAdvanceUtlize'), FIDocumentTypeId: netI(f.DocumentTypeId),
                UtilizeAmount: utilizeAmount
            });
            CUR.adv = -1; advRender();
            advResetDetails();
        } catch (e) { box(e.message); }
    }
    /* grdAdvanceUtlize_DoubleClick */
    function advEditRow(i) {
        var item = ADV.rows[i]; if (!item) return;
        ADV.updateIndex = i;
        X.setVal('CmbInvoiceAdvanceUtilize', item.InvoiceId); bindGds();
        X.setVal('CmbGdNoAdvanceUtilize', item.GDId);
        X.setVal('CmbPaymentTermGdUtilize', item.PaymentTermId);
        if (netI(item.PaymentTermId) === 1) bindFis(1);
        X.setVal('CmbFIAdvanceUtlize', item.FIId);
        setText('txtAdvanceUtlize', str(item.UtilizeAmount));
        X.show('btnAddAdvanceAgaintGdDetail', false); X.show('btnUpdateAdvanceAgaintGdDetail', true); X.show('btnCancelAdvanceAgaintGdDetail', true);
        X.focus('CmbInvoiceAdvanceUtilize');
    }
    /* btnUpdateAdvanceUtilize_Click (the desktop leaves DocumentTypeId / FIDocumentTypeId of the row as they were) */
    function advUpdateRow() {
        try {
            if (!advFormValidation()) return;
            var ptId = netI(val('CmbPaymentTermGdUtilize'));
            if (ptId === 1 && !X.hasSel('CmbFIAdvanceUtlize')) { box('Financial Instrument field is required...'); X.focus('CmbFIAdvanceUtlize'); return; }
            for (var i = 0; i < ADV.rows.length; i++) if (netI(ADV.rows[i].InvoiceId) !== netI(val('CmbInvoiceAdvanceUtilize'))) { box('Record cannot be add against multi invoice'); return; }
            var r = ADV.rows[ADV.updateIndex]; if (!r) return;
            r.InvoiceId = netI(val('CmbInvoiceAdvanceUtilize')); r.InvoiceNo = X.selText('CmbInvoiceAdvanceUtilize');
            r.GDId = netI(val('CmbGdNoAdvanceUtilize')); r.GDNo = X.selText('CmbGdNoAdvanceUtilize');
            r.PaymentTermId = ptId; r.PaymentTerm = X.selText('CmbPaymentTermGdUtilize');
            r.FIId = netI(val('CmbFIAdvanceUtlize')); r.FINo = X.selText('CmbFIAdvanceUtlize');
            r.UtilizeAmount = netD(val('txtAdvanceUtlize'));
            advRender();
            X.show('btnAddAdvanceAgaintGdDetail', true); X.show('btnUpdateAdvanceAgaintGdDetail', false); X.show('btnCancelAdvanceAgaintGdDetail', false);
            advResetDetails(); X.focus('CmbInvoiceAdvanceUtilize');
        } catch (e) { box(e.message); }
    }
    function advCancelRow() { ADV.updateIndex = -1; X.show('btnAddAdvanceAgaintGdDetail', true); X.show('btnUpdateAdvanceAgaintGdDetail', false); X.show('btnCancelAdvanceAgaintGdDetail', false); }
    /* grdAdvanceUtlize_ColumnButtonClick */
    function advDeleteRow(i) {
        var r = ADV.rows[i]; if (!r) return;
        if (netI(r.Id) > 0) { if (!ask('Are you sure to Delete?')) return; ADV.removedIds = ADV.removedIds + ',' + str(r.Id); }
        ADV.rows.splice(i, 1);
        if (ADV.updateIndex === i) advCancelRow();
        CUR.adv = -1; advRender();
    }
    /* ResetGdAdvanceUtilizeDetails */
    function advResetDetails() { ADV.updateIndex = -1; setText('txtAdvanceUtlize', ''); X.focus('CmbGdNoAdvanceUtilize'); }
    /* BtnNewAdvanceAgaintGD_Click */
    function advNew() {
        ADV.removedIds = '';
        X.setVal('CmbInvoiceAdvanceUtilize', '0'); X.setVal('CmbFIAdvanceUtlize', '0');
        setText('txtGdValueAdvanceUtlize', ''); setText('txtFIBalanceAdvanceUtlize', '');
        GDS = []; X.bind('CmbGdNoAdvanceUtilize', GDS, 'GdId', 'GDNO', []);
        X.show('btnAddAdvanceAgaintGdDetail', true); X.show('btnUpdateAdvanceAgaintGdDetail', false); X.show('btnCancelAdvanceAgaintGdDetail', false);
        advResetDetails();
        ADV.rows = []; CUR.adv = -1; advRender();
        PT = []; ptRender(); X.bind('CmbPaymentTermGdUtilize', [], 'PaymentTermId', 'PaymentTerm', []);
        return X.getJson(API + '/custom-invoices?invoiceId=0').then(function (rows) { DT_INVOICES = rows || []; bindAdvInvoices(); }).catch(function (e) { box(e.message); });
    }
    function advRefresh() { /* BtnRefreshAdvanceAgaintGD_Click is empty on the desktop */ }
    /* BtnSaveAdvanceAgaintGD_Click */
    function advSave(btn) {
        return X.busy(btn, function () {
            if (ADV.rows.length <= 0) { box('Advance Utilize Grid Record not found'); return Promise.resolve(); }
            if (!ask('Are you sure to Save?')) return Promise.resolve();
            return X.postJson(API + '/advance/save', { rows: ADV.rows, removedIds: ADV.removedIds, gds: GDS, paymentTerms: PT }).then(function (d) {
                box((d && d.message) || 'Record Save Successfully');
                ADV.rows = []; CUR.adv = -1; advRender(); ADV.removedIds = '';
                return advNew();
            }).catch(function (e) { box(e.message); });
        });
    }
    /* button1_Click (Show) */
    var ADV_HIST_COLS = ['InvoiceNo', { key: 'GDNo', link: true }, 'PaymentTerm', 'FINo', { key: 'UtilizeAmount', num: true, sum: true, fmt: X.fmt2 }, { key: 'EntryDate', fmt: X.ddMMyyyyHm }, 'EntryUser', { key: 'ModifyDate', fmt: X.ddMMyyyyHm }, 'ModifyUser'];
    function advHistoryShow(btn) {
        return X.busy(btn, function () {
            var q = ['invoiceId=' + netI(val('CmbInvoiceInvoiceAdvanceHistory'))];
            if ($id('datFromDateAdvanceHistoryChk').checked && val('datFromDateAdvanceHistory')) q.push('fromDate=' + val('datFromDateAdvanceHistory'));
            if ($id('datToDateAdvanceHistoryChk').checked && val('datToDateAdvanceHistory')) q.push('toDate=' + val('datToDateAdvanceHistory'));
            return X.getJson(API + '/advance/history?' + q.join('&')).then(function (rows) { ADV_HIST = rows || []; CUR.advHist = -1; X.drawGrid('advHistBody', 'advHistFoot', ADV_HIST, ADV_HIST_COLS, { cur: CUR.advHist, empty: 'advHistEmpty' }); }).catch(function (e) { box(e.message); });
        });
    }
    /* grdAdvanceHistory_DoubleClick: Form tab, dtInvoices re-read for that invoice, its rows into the grid. */
    function advHistoryPick(i) {
        var r = ADV_HIST[i]; if (!r) return;
        tab('adv', 'advForm');
        ADV.rows = []; CUR.adv = -1;
        var invoiceid = netI(col(r, 'InvoiceId'));
        X.getJson(API + '/custom-invoices?invoiceId=' + invoiceid).then(function (rows) {
            DT_INVOICES = rows || []; bindAdvInvoices();
            var p = Promise.resolve();
            if (invoiceid > 0) { X.setVal('CmbInvoiceAdvanceUtilize', invoiceid); p = advInvoiceLeave(); }
            return p;
        }).then(function () {
            ADV.rows = ADV_HIST.filter(function (h) { return netI(col(h, 'InvoiceId')) === invoiceid; }).map(X.copy);
            advRender();
        }).catch(function (e) { box(e.message); });
    }

    // ============================================================================== wiring
    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.ex-main-tab').forEach(function (b) { b.addEventListener('click', function () { mainTab(b.getAttribute('data-main')); }); });
        X.wireTabs(tab);
        X.on('CmbInvoiceGdBreakUp', 'change', invoiceChanged);
        X.on('txtGDValueGdBreakUp', 'input', onGdValue);
        X.on('txtFreightGdBreakUp', 'input', onFreight);
        X.on('txtFTTPrcntGdBreakUp', 'input', onFttPercent);
        X.on('txtFTTPrcntGdBreakUp', 'change', onFttPercent);
        X.on('CmbInvoiceAdvanceUtilize', 'change', advInvoiceLeave);
        X.on('CmbGdNoAdvanceUtilize', 'change', gdNoChanged);
        X.on('CmbPaymentTermGdUtilize', 'change', paymentTermLeave);
        X.on('CmbFIAdvanceUtlize', 'change', fiChanged);
        X.wireGrid('gdBody', { del: gdDeleteRow, open: gdEditRow, select: function (i) { CUR.gd = i; } });
        X.wireGrid('gdHistBody', { open: function (i) { var r = HIST[i]; if (r) gdReadById(col(r, 'ExImInvoiceId')); }, select: function (i) { CUR.hist = i; }, act: function (a, i) { if (a === 'breakup') containerBreakUp(i); } });
        X.wireGrid('advBody', { del: advDeleteRow, open: advEditRow, select: function (i) { CUR.adv = i; } });
        X.wireGrid('advHistBody', { open: advHistoryPick, select: function (i) { CUR.advHist = i; } });
        /* ImProformaInvoice_KeyDown */
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (X.enterMovesOn(e)) return;
            var onGd = $id('mainGd').classList.contains('is-active');
            var onHist = onGd ? X.activeInner('gd') === 'gdHistory' : X.activeInner('adv') === 'advHistory';
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); X.cancel(); return; }
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(onGd ? 'gd' : 'adv'); return; }
            if (e.ctrlKey && k === 'r') { e.preventDefault(); gdHistoryRefresh($id('btnRefreshGdBreakupHistory')); return; }
            if (onGd && onHist) {
                if (e.ctrlKey && k === 'n') { e.preventDefault(); gdHistoryReset(); }
                if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); var tr = $id('gdHistBody').querySelector('tr'); if (tr) tr.scrollIntoView(); }
                if (e.ctrlKey && e.key === 'ArrowUp') { e.preventDefault(); X.focus('FromDateGdBreakupHistory'); }
                if (e.ctrlKey && (e.key === 'Enter' || e.key === ' ') && CUR.hist >= 0) { e.preventDefault(); var r = HIST[CUR.hist]; if (r) gdReadById(col(r, 'ExImInvoiceId')); }
            }
        });
        load();
    });

    global.ExportGdMapping = {
        gdNew: gdNew, gdSave: gdSave, gdRefresh: gdRefresh, gdAddRow: gdAddRow, gdUpdateRow: gdUpdateRow, gdCancelRow: gdCancelRow,
        gdHistoryShow: gdHistoryShow, gdHistoryReset: gdHistoryReset, gdHistoryRefresh: gdHistoryRefresh, fiOpening: fiOpening,
        advNew: advNew, advRefresh: advRefresh, advSave: advSave, advAddRow: advAddRow, advUpdateRow: advUpdateRow, advCancelRow: advCancelRow,
        advHistoryShow: advHistoryShow, toggleHistory: toggleHistory
    };
}(window));
