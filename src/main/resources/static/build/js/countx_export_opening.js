/* ============================================================================================
 * countx_export_opening.js - ExportOpening.cs (Architecture.WinApp.Export), screen 189 "Export Opening Balance"
 * (desktop caption "Commercial Invoice", panel "Commercial Invoice Opening"), DocumentTypeId 212, rights of
 * "EximInvoice" (screen 211), with the popup LoadSalesContractForExportOpening.
 * Every toolbar button, TextChanged / Leave, grid button / double-click / cell edit, entry panel, history action and
 * keyboard shortcut of the desktop form has its counterpart here, with the desktop's messages and order.
 * Data: /api/export/export-opening (ExportOpeningController -> ExportOpeningService -> the desktop's procedures).
 * Print: CrystalPrint '521-exportinvoiceslip' { eximInvoiceId } (CommonServices.CommercialInvoiceSlip521).
 * ============================================================================================ */
(function (global) {
    'use strict';

    var X = global.ExM;
    var $id = X.$id, box = X.box, ask = X.ask, netD = X.netD, netI = X.netI, str = X.str, col = X.col;
    var API = '/api/export/export-opening';

    var PERM = { Save: true, Update: true, Print: true };
    var CFG = { defaultDaysToLessFromHistoryFromDate: 0, flagFin: false, autoGenInvoiceNo: false, rateAddLessOnCommercialInvoice: false, allowExportMultiCompanies: false };
    var L = { paymentTerms: [], deliveryTerms: [], farmingNTrade: [], fis: [], uoms: [], hsCodes: [] };
    var S = { recId: 0, updateMode: false, detail: [], other: [], pay: [], removed: [], udi: -1, udiO: -1, udiPT: -1, invoiceUpdateFlag: 0, items: [], oitems: [] };
    var H = { rows: [], cur: -1, detail: [] };
    var LD = { rows: [], main: [], checked: {}, cur: -1 };

    /* .NET formats used by the form. */
    function f3(v) { return X.opt(v, 3); }                  /* "#,##0.###" */
    function f3z(v) { return X.opt(v, 3, true); }           /* "#,#.###" (0 -> "") */
    function f4(v) { return X.opt(v, 4); }                  /* "#,##0.####" */
    function f00(v) { var s = X.fixed(v, 0); var neg = s.charAt(0) === '-'; var d = neg ? s.substring(1) : s; if (d.length < 2) d = '0' + d; return (neg ? '-' : '') + d; }   /* "0,0" */
    function roundEven(v) { var r = Math.round(v); if (Math.abs(v % 1) === 0.5 && r % 2 !== 0) r -= 1; return r; }   /* Math.Round(double) */
    function sum(rows, k) { var t = 0; rows.forEach(function (r) { t += netD(r[k]); }); return t; }

    // ------------------------------------------------------------------------------ TextChanged emulation

    var HANDLERS = {};
    /* Set a textbox / combo from code; the desktop's TextChanged / ValueChanged fires only when the value changes. */
    function setT(id, v) {
        var e = $id(id); if (!e) return;
        var before = e.value;
        if (e.tagName === 'SELECT') X.setVal(id, v); else e.value = str(v);
        if (e.value !== before && HANDLERS[id]) HANDLERS[id]();
    }
    function setComboText(id, t) {
        var s = $id(id); if (!s) return;
        var before = s.value, hit = '0';
        Array.prototype.forEach.call(s.options, function (o) { if (o.value !== '0' && o.text === str(t) && hit === '0') hit = o.value; });
        s.value = hit; X.refreshCombos();
        if (s.value !== before && HANDLERS[id]) HANDLERS[id]();
    }
    function fire(id) { if (HANDLERS[id]) HANDLERS[id](); }

    // ------------------------------------------------------------------------------ columns

    var DETAIL_COLS = [
        { k: 'Id', hide: true }, { k: 'ContractId', hide: true }, { k: 'ContractDetailId', hide: true }, { k: 'ContractNo', hide: true },
        { k: 'ContractDate', hide: true }, { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 220 }, { k: 'PackTypeId', hide: true },
        { k: 'PackType', t: 'PackType', w: 150 }, { k: 'CropYearId', hide: true }, { k: 'CropYear', t: 'CropYear' }, { k: 'JobLotId', hide: true },
        { k: 'JobLot', t: 'JobLot' }, { k: 'QtyMTon', t: 'Qty/M.Ton', num: 3, zeroEmpty: true }, { k: 'PackSizeId', hide: true }, { k: 'PackSize', t: 'PackSize' },
        { k: 'NoOfBags', t: 'NoOfBags', num: 0, zeroEmpty: true }, { k: 'RateWithoutAddLess', t: 'RateWithoutAddLess', num: 3, zeroEmpty: true },
        { k: 'RateAddLess', t: 'RateAddLess', num: 3, zeroEmpty: true }, { k: 'CostMTon', t: 'Cost/M.Ton', num: 3, zeroEmpty: true, noTotal: true },
        { k: 'ContractRate', t: 'ContractRate' }, { k: 'RateUOMId', hide: true }, { k: 'RateUOM', t: 'RateUOM' }, { k: 'RateEquivalent', hide: true },
        { k: 'Amount', t: 'Amount', num: 3, zeroEmpty: true }, { k: 'HSCode', t: 'HSCode' }, { k: 'ItemDetail', t: 'ItemDetail', w: 160 },
        { k: 'PackingDetail', t: 'PackingDetail', w: 140 }, { k: 'OtherItemAmount', t: 'OtherItemAmount', num: 3 },
        { k: 'ExImFarmingNTradeId', hide: true }, { k: 'FarmingNTrade', t: 'FarmingNTrade' }, { k: 'ExImFarmingTypeId', hide: true }, { k: 'ExImTradeTypeId', hide: true }
    ];
    function detailCols() {
        return DETAIL_COLS.map(function (c) {
            var o = {}; for (var k in c) o[k] = c[k];
            if (o.k === 'ContractRate') o.hide = !CFG.flagFin;
            if (o.k === 'RateWithoutAddLess') o.hide = !CFG.rateAddLessOnCommercialInvoice;
            if (o.k === 'RateAddLess') { o.hide = !CFG.rateAddLessOnCommercialInvoice; if (CFG.rateAddLessOnCommercialInvoice) o.edit = 'num'; }
            return o;
        });
    }
    var OTHER_COLS = [
        { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 350 }, { k: 'Qty', t: 'Qty', num: 2 }, { k: 'Rate', t: 'Rate', num: 0 },
        { k: 'Amount', t: 'Amount', num: 2 }, { k: 'Remarks', t: 'Remarks', w: 350 }
    ];
    var PAY_COLS = [
        { k: 'Id', hide: true }, { k: 'PaymentTermId', hide: true }, { k: 'PaymentTerm', t: 'PaymentTerm', w: 350 }, { k: 'DocumentTypeId', hide: true },
        { k: 'ExImEFormRegistrationId', hide: true }, { k: 'FinancialInstrumentNo', t: 'FinancialInstrumentNo', w: 350 },
        { k: 'PrcntOfTotal', t: '%OfTotal', num: 2, zeroEmpty: true }, { k: 'FcyAmount', t: 'FcyAmount', num: 2, zeroEmpty: true },
        { k: 'DueDays', t: 'DueDays' }, { k: 'Remarks', t: 'Remarks', w: 160 }
    ];
    var HIST_COLS = [
        { k: 'Id', hide: true }, { k: 'InvoiceNo', t: 'InvoiceNo', w: 80, code: true }, { k: 'DocCode', t: 'DocCode', w: 70 }, { k: 'DocDate', t: 'DocDate', date: 'short' },
        { k: 'CustomerName', t: 'CustomerName', w: 140 }, { k: 'NoOfContainers', t: 'NoOfContainers', num: 0 }, { k: 'GrossWeight', t: 'GrossWeight', num: 3 },
        { k: 'NetWeight', t: 'NetWeight', num: 3 }, { k: 'FcyAmount', t: 'FcyAmount', num: 3 }, { k: 'AddLess', t: 'Add/Less', num: 3 },
        { k: 'TotalAmount', t: 'TotalAmount', num: 3 }, { k: 'LoadingPort', t: 'LoadingPort' }, { k: 'DestinationPort', t: 'DestinationPort' },
        { k: 'EntryUser', t: 'EntryUser' }, { k: 'EntryDate', t: 'EntryDate', date: 'dtm' }, { k: 'ModifyUser', t: 'ModifyUser' },
        { k: 'ModifyDate', t: 'ModifyDate', date: 'dtm' }, { k: 'NoOfAttachments', t: 'NoOfAttachments', code: true }
    ];
    function histDetailCols() {
        return [
            { k: 'ContractNo', t: 'ContractNo' }, { k: 'ItemName', t: 'ItemName', w: 160 }, { k: 'PackType', t: 'PackType' }, { k: 'CropYear', t: 'CropYear' },
            { k: 'JobLot', t: 'JobLot' }, { k: 'QtyMTon', t: 'Qty/M.Ton', num: 3 }, { k: 'PackSize', t: 'PackSize' }, { k: 'NoOfBags', t: 'NoOfBags', num: 3 },
            { k: 'RateWithoutAddLess', t: 'RateWithoutAddLess', num: 3, hide: !CFG.rateAddLessOnCommercialInvoice },
            { k: 'RateAddLess', t: 'RateAddLess', num: 3, hide: !CFG.rateAddLessOnCommercialInvoice }, { k: 'CostMTon', t: 'Cost/M.Ton', num: 3 },
            { k: 'CostUom', t: 'CostUom' }, { k: 'Amount', t: 'Amount', num: 3 }, { k: 'HSCode', t: 'HSCode' }, { k: 'ItemDetail', t: 'ItemDetail' },
            { k: 'FarmingNTrade', t: 'FarmingNTrade' }, { k: 'PackingDetail', t: 'PackingDetail' }
        ];
    }
    var LD_COLS = [
        { k: 'Id', hide: true }, { k: 'DocumentTypeId', hide: true }, { k: 'DocNo', t: 'DocNo', w: 60 }, { k: 'ContractNo', t: 'ContractNo', w: 90 },
        { k: 'ContractDate', t: 'ContractDate', date: 'short' }, { k: 'OrderDate', t: 'OrderDate', date: 'short' }, { k: 'ScheduleStatus', hide: true },
        { k: 'SupplierCustomerId', hide: true }, { k: 'PartyName', t: 'PartyName', w: 130 }, { k: 'SalesMan', t: 'SalesMan', w: 120 },
        { k: 'ExporterBank', t: 'ExporterBank', w: 130 }, { k: 'ShipmentStartDate', t: 'ShipmentStartDate', date: 'short' }, { k: 'DeliveryDays', t: 'DeliveryDays' },
        { k: 'LastShipmentDate', t: 'LastShipmentDate', date: 'short' }, { k: 'CustomerApprovedDate', t: 'CustomerApprovedDate', date: 'short' },
        { k: 'CustomerApprovalRemarks', t: 'CustomerApprovalRemarks', w: 150 }, { k: 'FCurrencyId', hide: true }, { k: 'CurrencyCode', t: 'CurrencyCode' },
        { k: 'ExchangeRate', t: 'ExchangeRate' }, { k: 'NoOfContainers', t: 'NoOfContainers' }, { k: 'DeliveryTerm', t: 'DeliveryTerm' },
        { k: 'PaymentTerm', t: 'PaymentTerm' }, { k: 'LoadingPort', t: 'LoadingPort' }, { k: 'DestinationPort', t: 'DestinationPort' },
        { k: 'InsuranceRemarks', t: 'InsuranceRemarks' }, { k: 'RemarksHeader', t: 'RemarksHeader' }, { k: 'EntryUser', t: 'EntryUser' },
        { k: 'EntryDate', t: 'EntryDate', date: 'dtm' }, { k: 'ModifyUser', t: 'ModifyUser' }, { k: 'ModifyDate', t: 'ModifyDate', date: 'dtm' },
        { k: 'NoOfAttachments', t: 'NoOfAttachments', code: true }
    ];
    var LD_DETAIL_COLS = [
        { k: 'DetailId', hide: true }, { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 150 }, { k: 'PackingType', t: 'PackingType' },
        { k: 'CropYear', t: 'CropYear' }, { k: 'CommodityDetail', t: 'CommodityDetail' }, { k: 'PackUom', t: 'PackUom' },
        { k: 'MTon', t: 'MTon', fixed: 0 }, { k: 'ShipMTon', t: 'ShipMTon', fixed: 0 }, { k: 'BalMTon', t: 'BalMTon', fixed: 0 },
        { k: 'NoOfBags', t: 'NoOfBags', fixed: 0 }, { k: 'ShipBags', t: 'ShipBags', fixed: 0 }, { k: 'BalBags', t: 'BalBags', fixed: 0 },
        { k: 'NetWeight', t: 'NetWeight', fixed: 0 }, { k: 'ShipWeight', t: 'ShipWeight', fixed: 0 }, { k: 'BalWeight', t: 'BalWeight', fixed: 0 },
        { k: 'ItemRate', t: 'ItemRate' }, { k: 'Amount', t: 'Amount', fixed: 0 }, { k: 'ShipAmount', t: 'ShipAmount', fixed: 0 }, { k: 'BalAmount', t: 'BalAmount', fixed: 0 }
    ];

    // ------------------------------------------------------------------------------ tabs / load

    function onHistory() { return $id('eoHistory').classList.contains('is-active'); }
    function tabChanged(p) {
        var hist = p === 'eoHistory';
        var fb = $id('btnEoFooterHistory');
        fb.querySelector('span').textContent = hist ? 'Form' : 'History';
        fb.querySelector('i').className = hist ? 'fa fa-file-text-o' : 'fa fa-history';
        X.focus(hist ? 'FromDateHistory' : 'txtDocDate');           /* tabControl1_SelectedIndexChanged */
    }
    function toggleHistory() { var p = onHistory() ? 'eoForm' : 'eoHistory'; X.selectTab('eo', p); tabChanged(p); }
    function detailTab() { var a = document.querySelector('.win-tabs[data-tabs="eoDet"] .win-tab.is-active'); return a ? ['tabPage3', 'tabPage4', 'tabPage5'].indexOf(a.getAttribute('data-tab')) : 0; }
    function selectDetailTab(i) { X.selectTab('eoDet', ['tabPage3', 'tabPage4', 'tabPage5'][i]); }

    /* BindData / btnRefresh_Click combos (values kept when they still exist, as the desktop's Fill methods do). */
    function bindCombos(d) {
        if (d.creditAccounts) X.bind('CmbCreditAccount', d.creditAccounts, 'Id', 'AccountTitle');
        if (d.customers) X.bind('cmbSupCust', d.customers, 'Id', 'name');
        if (d.notifyParties) { X.bind('cmbNotifyParty1', d.notifyParties, 'Id', 'CompanyName'); X.bind('cmbNotifyParty2', d.notifyParties, 'Id', 'CompanyName'); }
        if (d.deliveryTerms) { L.deliveryTerms = d.deliveryTerms; X.bind('cmbdeliverytermnew', d.deliveryTerms, 'Id', 'Code', ['Description']); }
        if (d.paymentTerms && d.paymentTerms.length) {
            L.paymentTerms = d.paymentTerms;
            X.bind('cmbPaymentTermsNew', d.paymentTerms, 'Id', 'lcOrderTerm', ['Description']);
            X.bind('dcmbpaymentterm', d.paymentTerms, 'Id', 'lcOrderTerm', ['Description']);
        }
        if (d.loadingPorts) X.bind('cmbLoadingPort', d.loadingPorts, 'Id', 'PortName');
        if (d.destinationPorts) X.bind('cmbDestinationPort', d.destinationPorts, 'Id', 'PortName');
        if (d.currencies) X.bind('cmbfcycode', d.currencies, 'Id', 'CurrencyName');
        if (d.exporterBanks) {
            X.bind('cmbexporterBankNew', d.exporterBanks, 'Id', 'BranchName');
            X.bind('cmbimporterBankNew', d.importerBanks || [], 'Id', 'BranchName');
        }
        /* CarierType(): fixed rows, Rows[1] (= "By Sea", row 0 is the ZeroIndex row) activated on every bind. */
        X.setVal('cmbcareiertype', '1');
        if (d.cropYears) X.bind('CmbCropYear', d.cropYears, 'Id', 'CropYear');
        if (d.jobLots) X.bind('CmbJobLot', d.jobLots, 'Id', 'JobLotDescription');
        if (d.packTypes) X.bind('combpcktype', d.packTypes, 'Id', 'Description');
        if (d.farmingNTrade) { L.farmingNTrade = d.farmingNTrade; X.bind('CmbFarmingNTrade', d.farmingNTrade, 'Id', 'FarmingNTrade'); }
    }
    function load() {
        return X.getJson(API + '/setup').then(function (d) {
            d = d || {};
            PERM = d.permissions || PERM;
            CFG = d.config || CFG;
            /* ConfigureRights */
            $id('btnsave').disabled = !PERM.Save;
            $id('btnupdate').disabled = !PERM.Update;
            $id('btnPrint').disabled = !PERM.Print;
            setInvoiceNoState(!!CFG.autoGenInvoiceNo, d.invoiceNo);
            X.show('rowExportCompany', !!CFG.allowExportMultiCompanies);
            if (CFG.allowExportMultiCompanies) X.bind('CmbExportCompany', d.exportCompanies || [], 'Id', 'CompName');
            bindHistoryCustomers(d.historyCustomers || []);
            X.setText('FromDateHistory', CFG.defaultDaysToLessFromHistoryFromDate > 0 ? X.daysAgo(CFG.defaultDaysToLessFromHistoryFromDate) : X.daysAgo(3));
            X.setText('ToDateHistory', X.today());
            /* BindData */
            X.bind('cmbbranches', d.branches || [], 'Id', 'BranchName');
            X.bind('cmbproject', d.projects || [], 'Id', 'ProjectName');
            X.setText('txtdocno', netI(d.docNo) > 0 ? d.docNo : '');
            bindCombos(d);
            /* ImporterandExportBankFill: Rows[1] (first real exporter bank) activated. */
            var eb = $id('cmbexporterBankNew'); if (eb && eb.options.length > 1) X.setVal('cmbexporterBankNew', eb.options[1].value);
            X.setText('txtDocDate', X.today());
            X.setText('txtIformDate', X.today());
            drawDetail(); drawOther(); drawPay();
            showMode(false);
            $id('eoFooterInfo').textContent = 'ExportOpening  -  Document Type 212';
            X.focus('txtDocDate');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    function setInvoiceNoState(auto, invoiceNo) {
        CFG.autoGenInvoiceNo = auto;
        $id('cmbInvoiceNo').disabled = !!auto;
        if (auto && str(invoiceNo) !== '') X.setText('cmbInvoiceNo', invoiceNo);   /* GenerateInvoiceNo: only when Code != "" */
    }
    function showMode(update) {
        S.updateMode = update;
        X.show('btnsave', !update);
        X.show('btnupdate', update);
    }

    // ------------------------------------------------------------------------------ calculations

    /* getTotalnetWeightFromGrid */
    function getTotalNet() {
        setT('txtNetWeight', f3(sum(S.detail, 'QtyMTon')));
        setT('txtfcyAmount', f3(sum(S.detail, 'Amount')));
    }
    /* TotalAmountCalculate */
    function totalAmountCalculate() {
        var other = sum(S.other, 'Amount'), addless = netD(X.val('txtaddlessamount')), fcy = netD(X.val('txtfcyAmount'));
        if (fcy > 0) X.setText('txtTotalNetAmount', f3z(fcy + addless + other));
        var total = netD(X.val('txtTotalNetAmount'));
        if (total > 0 && S.pay.length > 0) {
            S.pay.forEach(function (r) { r.FcyAmount = netD(r.PrcntOfTotal) * total / 100.0; });
            drawPay();
        }
        calculateLocalAmount();
    }
    /* CalculateLocalAmount */
    function calculateLocalAmount() {
        var fcy = netD(X.val('txtTotalNetAmount')), rate = netD(X.val('txtExRate'));
        X.setText('txtLocalAmt', fcy > 0 && rate > 0 ? f00(fcy * rate) : '0');
    }
    /* ProportionateOtherItemAmount */
    function proportionate() {
        var contract = sum(S.detail, 'Amount'), other = sum(S.other, 'Amount');
        S.detail.forEach(function (r) { r.OtherItemAmount = other > 0 ? other / contract * netD(r.Amount) : 0; });
        drawDetail();
    }
    function uomEq(id) {
        var v = netI(X.val(id)); if (v === 0) return 0;
        for (var i = 0; i < L.uoms.length; i++) if (netI(L.uoms[i].Id) === v) return netD(L.uoms[i].Equivalent);
        return 0;
    }
    /* CalculateWeight + CalculatAmount (txtQtyMTon / pack uom / pack type / txtCostMTon / rate uom TextChanged). */
    function calcWeightAmount() {
        X.setText('txtNoOfBags', '0');
        var mton = netD(X.val('txtQtyMTon')), outer = uomEq('CmbPackUom');
        if (mton > 0 && outer > 0) X.setText('txtNoOfBags', String(roundEven(mton * 1000.0 / outer)));
        X.setText('txtAmount', '0');
        var rate = netD(X.val('txtCostMTon')), eq = uomEq('CmbRateUom'), qty = netD(X.val('txtQtyMTon'));
        if (rate > 0 && eq > 0 && qty > 0) X.setText('txtAmount', f00(rate / eq * (qty * 1000.0)));
    }
    /* OtherItemsCalculations */
    function otherCalc() {
        var q = netD(X.val('txtoQty')), r = netD(X.val('txtoRate'));
        X.setText('txtoAmount', q > 0 && r > 0 ? X.raw(q * r) : '0');
    }
    /* PaymentDetailPercentCalculate */
    function percentCalc() {
        var total = netD(X.val('txtTotalNetAmount')), pct = netD(X.val('dtxtpercentoftotal'));
        if (total > 0 && pct > 0) {
            if (pct > 100.0) { X.setText('dtxtpercentoftotal', '0'); box('Percent cannot be greater than 100 Please check'); return; }
            X.setText('dtxtfcyamount', f3z(total * pct / 100.0));
        }
    }
    /* dtxtfcyamount_TextChanged */
    function fcyPercent() {
        var total = netD(X.val('txtTotalNetAmount')), fcy = netD(X.val('dtxtfcyamount'));
        if (total > 0 && fcy > 0) {
            var pct = fcy / total * 100.0;
            X.setText('dtxtpercentoftotal', X.raw(pct));
            if (pct > 100.0) { X.setText('dtxtpercentoftotal', '0'); box('Percent cannot be greater than 100 Please check'); }
        }
    }

    // ------------------------------------------------------------------------------ grids

    function drawDetail() {
        X.grid('grdDetail', {
            cols: detailCols(), rows: S.detail, current: S.udi >= 0 ? S.udi : undefined,
            lead: [{ t: 'X', html: function () { return X.btnHtml('Delete', 'X'); } }, { t: '+', html: function () { return X.btnHtml('Add', '+'); } }],
            onDbl: function (i) { detailDblClick(i); },
            onButton: function (i, act) { detailButton(i, act); },
            onEdit: function (i, k, v) { detailCellUpdated(i, k, v); },
            onKey: function (i, e) { if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); detailDblClick(i); } }
        });
    }
    function drawOther() {
        X.grid('grdotheritems', {
            cols: OTHER_COLS, rows: S.other, current: S.udiO >= 0 ? S.udiO : undefined,
            onDbl: function (i) { otherDblClick(i); },
            onKey: function (i, e) { if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); otherDblClick(i); } }
        });
    }
    function drawPay() {
        X.grid('grdpaymentdetail', {
            cols: PAY_COLS, rows: S.pay, current: S.udiPT >= 0 ? S.udiPT : undefined,
            lead: [{ t: 'X', html: function () { return X.btnHtml('Delete', 'X'); } }],
            onDbl: function (i) { payDblClick(i); },
            onButton: function (i, act) { if (act === 'Delete') payDelete(i); },
            onKey: function (i, e) { if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); payDblClick(i); } }
        });
    }

    /* grdDetail_ColumnButtonClick */
    function detailButton(i, act) {
        try {
            var r = S.detail[i]; if (!r) return;
            if (act === 'Delete') {
                if (S.udi !== -1) throw new Error('Reset the Detail First');
                if (netI(r.Id) > 0) {
                    if (!ask('Are you sure to Delete?')) return;
                    S.removed.push(r);
                }
                S.detail.splice(i, 1);
            } else if (act === 'Add') {
                var c = {}; for (var k in r) c[k] = r[k];
                c.Id = 0;
                S.detail.push(c);
            }
            drawDetail();
            getTotalNet();
            proportionate();
        } catch (e) { box(e.message); }
    }
    /* grdDetail_CellUpdated (RateAddLess editable when RateAddLessOnCommercialInvoice). */
    function detailCellUpdated(i, k, v) {
        var r = S.detail[i]; if (!r) return;
        r[k] = netD(v);
        var qty = netD(r.QtyMTon), rate = netD(r.RateWithoutAddLess) + netD(r.RateAddLess), eq = netD(r.RateEquivalent);
        r.CostMTon = rate;
        if (rate > 0 && eq > 0 && qty > 0) r.Amount = rate / eq * (qty * 1000.0);
        drawDetail();
        getTotalNet();
        proportionate();
        calculateLocalAmount();
    }
    /* grdDetail_DoubleClick */
    function detailDblClick(i) {
        var r = S.detail[i]; if (!r) return;
        S.udi = i;
        X.setVal('combitem', r.ItemId);
        return bindRateUomAndItemPackUom().then(function () {
            setT('combpcktype', r.PackTypeId);
            setT('CmbPackUom', r.PackSizeId);
            setT('CmbCropYear', r.CropYearId);
            setT('CmbJobLot', r.JobLotId);
            setT('txtQtyMTon', X.raw(r.QtyMTon));
            setT('CmbRateUom', r.RateUOMId);
            setT('txtNoOfBags', X.raw(r.NoOfBags));
            setT('txtCostMTon', X.raw(r.CostMTon));
            setT('txtAmount', X.raw(r.Amount));
            X.setText('txtitemdetail', r.ItemDetail);
            setHsText(r.HSCode);
            setT('CmbFarmingNTrade', r.ExImFarmingNTradeId);
            X.setText('txtpackingdetail', r.PackingDetail);
            X.show('btnAddinGrid', false); X.show('btnGridUpdate', true); X.show('btnCancel', true);
            $id('txtCostMTon').readOnly = (S.invoiceUpdateFlag === 0 || !CFG.flagFin);
            drawDetail();
            X.focus('combpcktype');
        }).catch(function (e) { box(e.message); });
    }
    /* bindRateUomAndItemPackUom + HSCodeBind (combitem_Leave). */
    function bindRateUomAndItemPackUom() {
        var itemId = netI(X.val('combitem'));
        var packText = X.text('CmbPackUom'), rateText = X.text('CmbRateUom');
        return X.getJson(API + '/item?itemId=' + itemId + '&customerId=' + netI(X.val('cmbSupCust'))).then(function (d) {
            d = d || {};
            L.uoms = d.uoms || [];
            X.bind('CmbPackUom', L.uoms, 'Id', 'UOMCode', ['Equivalent'], '0');
            X.bind('CmbRateUom', L.uoms, 'Id', 'UOMCode', ['Equivalent'], '0');
            if (packText) setComboText('CmbPackUom', packText);
            if (rateText) setComboText('CmbRateUom', rateText);
            bindHs(d.hsCodes || []);
        });
    }
    function bindHs(rows) {
        if (!rows.length) return;                                 /* HSCodeBind returns when there are no rows */
        var keep = X.text('CmbHSCode');
        L.hsCodes = rows;
        X.bind('CmbHSCode', rows, 'HSCode', 'HSCode', null, '0');
        if (keep) setHsText(keep);
    }
    /* CmbHSCode.Text = value - an UltraCombo accepts free text. */
    function setHsText(t) {
        var s = $id('CmbHSCode'); t = str(t);
        if (t === '') { s.value = '0'; X.refreshCombos(); return; }
        var found = Array.prototype.some.call(s.options, function (o) { return o.value === t; });
        if (!found) { var o = document.createElement('option'); o.value = t; o.text = t; s.appendChild(o); }
        s.value = t; X.refreshCombos();
    }
    function hsText() { return X.val('CmbHSCode') === '0' ? '' : X.val('CmbHSCode'); }

    /* DetailFormValidation */
    function detailValid() {
        var checks = [
            [netI(X.val('combitem')) === 0, 'Item field required', 'combitem'],
            [netI(X.val('combpcktype')) === 0, 'Pack Type field required', 'combpcktype'],
            [netI(X.val('CmbCropYear')) === 0, 'Crop Year field required', 'CmbCropYear'],
            [netI(X.val('CmbJobLot')) === 0, 'JobLot field required', 'CmbJobLot'],
            [netI(X.val('CmbPackUom')) === 0, 'Pack uom field required', 'CmbPackUom'],
            [X.val('txtQtyMTon').trim() === '' || !(netD(X.val('txtQtyMTon')) > 0), 'Qty/M.Ton field required', 'txtQtyMTon'],
            [X.val('txtNoOfBags').trim() === '' || !(netD(X.val('txtNoOfBags')) > 0), 'No of Bags field required', 'txtNoOfBags'],
            [X.val('txtCostMTon').trim() === '' || !(netD(X.val('txtCostMTon')) > 0), 'Cost/M.Ton field required', 'txtCostMTon'],
            [netI(X.val('CmbRateUom')) === 0, 'Rate UOM field required', 'CmbRateUom'],
            [X.val('txtAmount').trim() === '' || !(netD(X.val('txtAmount')) > 0), 'Amount field required', 'txtAmount'],
            [X.val('txtTotalNetAmount').trim() === '' || !(netD(X.val('txtTotalNetAmount')) > 0), 'Total Net Amount  field required', 'txtTotalNetAmount']
        ];
        for (var i = 0; i < checks.length; i++) if (checks[i][0]) { box(checks[i][1]); X.focus(checks[i][2]); return false; }
        return true;
    }
    /* btnAddinGrid_Click */
    function btnAddinGrid() { box('Sorry Cannot Add New Record Only Update Detail Record : Thank You'); }
    /* btnGridUpdate_Click */
    function btnGridUpdate() {
        try {
            if (!detailValid()) return;
            var r = S.detail[S.udi]; if (!r) return;
            var fnt = null, fntId = netI(X.val('CmbFarmingNTrade'));
            if (fntId > 0) L.farmingNTrade.forEach(function (x) { if (netI(x.Id) === fntId) fnt = x; });
            r.ItemId = netI(X.val('combitem')); r.ItemName = X.text('combitem');
            r.PackTypeId = netI(X.val('combpcktype')); r.PackType = X.text('combpcktype');
            r.QtyMTon = netD(X.val('txtQtyMTon'));
            r.PackSizeId = netI(X.val('CmbPackUom')); r.PackSize = X.text('CmbPackUom');
            r.CropYearId = netI(X.val('CmbCropYear')); r.CropYear = X.text('CmbCropYear');
            r.JobLotId = netI(X.val('CmbJobLot')); r.JobLot = X.text('CmbJobLot');
            r.NoOfBags = netD(X.val('txtNoOfBags'));
            r.CostMTon = netD(X.val('txtCostMTon'));
            r.RateUOMId = netI(X.val('CmbRateUom')); r.RateUOM = X.text('CmbRateUom');
            r.RateEquivalent = uomEq('CmbRateUom');
            r.Amount = netD(X.val('txtAmount'));
            r.HSCode = hsText();
            r.ItemDetail = X.val('txtitemdetail');
            r.PackingDetail = X.val('txtpackingdetail');
            r.ExImFarmingNTradeId = fnt ? netI(fnt.Id) : 0;
            r.FarmingNTrade = fnt ? str(fnt.FarmingNTrade) : '';
            r.ExImFarmingTypeId = fnt ? netI(fnt.ExImFarmingTypeId) : 0;
            r.ExImTradeTypeId = fnt ? netI(fnt.ExImTradeTypeId) : 0;
            drawDetail();
            getTotalNet();
            proportionate();
            detailFormReset();
            calculateLocalAmount();
            X.focus('combpcktype');
        } catch (e) { box(e.message); }
    }
    function btnCancel() { detailFormReset(); }
    /* DetailFormReset */
    function detailFormReset() {
        S.udi = -1;
        setT('combitem', '0');
        setT('CmbFarmingNTrade', '0');
        setT('combpcktype', '0');
        setT('txtQtyMTon', '');
        setT('CmbPackUom', '0');
        setT('txtNoOfBags', '');
        setT('txtCostMTon', '');
        setT('CmbJobLot', '0');
        setT('CmbRateUom', '0');
        setT('txtAmount', '');
        X.setText('txtitemdetail', '');
        X.setText('txtpackingdetail', '');
        setHsText('');
        X.show('btnAddinGrid', true); X.show('btnGridUpdate', false); X.show('btnCancel', false);
        selectDetailTab(0);
        drawDetail();
    }

    // ------------------------------------------------------------------------------ other items

    function otherValid() {
        if (netI(X.val('CmbOtherItem')) === 0) { box('Item field required'); X.focus('CmbOtherItem'); return false; }
        if (netD(X.val('txtoRate')) === 0) { box('Rate field required'); X.focus('txtoRate'); return false; }
        return true;
    }
    function btnoadd() {
        try {
            if (!otherValid()) return;
            S.other.push({ ItemId: netI(X.val('CmbOtherItem')), ItemName: X.text('CmbOtherItem'), Qty: netD(X.val('txtoQty')), Rate: netD(X.val('txtoRate')),
                Amount: netD(X.val('txtoAmount')), Remarks: X.val('txtoRemarks').trim() });
            drawOther();
            resetOtherItems();
            totalAmountCalculate();
            proportionate();
        } catch (e) { box(e.message); }
    }
    function otherDblClick(i) {
        var r = S.other[i]; if (!r) return;
        S.udiO = i;
        X.setVal('CmbOtherItem', r.ItemId);
        setT('txtoQty', X.raw(r.Qty));
        X.setText('txtoRate', X.raw(r.Rate));
        X.setText('txtoAmount', X.raw(r.Amount));
        X.setText('txtoRemarks', r.Remarks);
        X.show('btnoadd', false); X.show('btnoUpdate', true); X.show('btnoCancel', true);
        drawOther();
        X.focus('CmbOtherItem');
    }
    function btnoUpdate() {
        try {
            if (!otherValid()) return;
            var r = S.other[S.udiO]; if (!r) return;
            r.ItemId = netI(X.val('CmbOtherItem')); r.ItemName = X.text('CmbOtherItem');
            r.Qty = netD(X.val('txtoQty')); r.Rate = netD(X.val('txtoRate')); r.Amount = netD(X.val('txtoAmount')); r.Remarks = X.val('txtoRemarks');
            X.show('btnoadd', true); X.show('btnoUpdate', false); X.show('btnoCancel', false);
            resetOtherItems();
            drawOther();
            X.focus('txtoQty');
            totalAmountCalculate();
            proportionate();
        } catch (e) { box(e.message); }
    }
    function btnoCancel() {
        X.show('btnoadd', true); X.show('btnoUpdate', false); X.show('btnoCancel', false);
        resetOtherItems();
        S.udiO = -1;
        drawOther();
    }
    function resetOtherItems() {
        S.udiO = -1;
        X.setVal('CmbOtherItem', '0');
        setT('txtoQty', '');
        X.setText('txtoRate', '');
        X.setText('txtoAmount', '');
        X.setText('txtoRemarks', '');
    }

    // ------------------------------------------------------------------------------ payment detail

    function fiRow() {
        var v = netI(X.val('CmbFinInstruments')); if (v <= 0) return null;
        for (var i = 0; i < L.fis.length; i++) if (netI(L.fis[i].Id) === v) return L.fis[i];
        return null;
    }
    function payValid() {
        if (netI(X.val('dcmbpaymentterm')) === 0) { box('Payment Term field required'); X.focus('dcmbpaymentterm'); return false; }
        if (netD(X.val('dtxtpercentoftotal')) === 0 || X.val('dtxtpercentoftotal') === '') { box('Percent Field Required'); X.focus('dtxtpercentoftotal'); return false; }
        if (netD(X.val('dtxtfcyamount')) === 0 || X.val('dtxtfcyamount') === '') { box('Fcy Amount  Field Required'); X.focus('dtxtfcyamount'); return false; }
        return true;
    }
    function btndadd() {
        try {
            if (!payValid()) return;
            var fi = fiRow();
            S.pay.forEach(function (r) {
                if (fi && netI(r.ExImEFormRegistrationId) === netI(fi.Id) && netI(r.DocumentTypeId) === netI(fi.DocumnetTypeId))
                    throw new Error('Financial Instrument No already add in Grid Please Check');
            });
            S.pay.push({ Id: 0, PaymentTermId: netI(X.val('dcmbpaymentterm')), PaymentTerm: X.text('dcmbpaymentterm'), DocumentTypeId: fi ? netI(fi.DocumnetTypeId) : 0,
                ExImEFormRegistrationId: netI(X.val('CmbFinInstruments')), FinancialInstrumentNo: fi ? X.text('CmbFinInstruments') : '',
                PrcntOfTotal: netD(X.val('dtxtpercentoftotal')), FcyAmount: netD(X.val('dtxtfcyamount')), DueDays: netD(X.val('dtxtdueday')),
                Remarks: X.val('txtPaymentRemarksDetail').trim() });
            drawPay();
            resetPaymentDetails();
            X.focus('dcmbpaymentterm');
        } catch (e) { box(e.message); }
    }
    function btndupdate() {
        try {
            if (!payValid()) return;
            var fi = fiRow();
            S.pay.forEach(function (r, i) {
                if (S.udiPT !== i && netI(r.ExImEFormRegistrationId) > 0 && netI(r.ExImEFormRegistrationId) === netI(X.val('CmbFinInstruments'))
                    && fi && netI(r.DocumentTypeId) === netI(fi.DocumnetTypeId))
                    throw new Error('Financial Instrument No already add in Grid Please Check');
            });
            var r = S.pay[S.udiPT]; if (!r) return;
            r.PaymentTermId = netI(X.val('dcmbpaymentterm')); r.PaymentTerm = X.text('dcmbpaymentterm');
            r.DocumentTypeId = fi ? netI(fi.DocumnetTypeId) : 0;
            r.ExImEFormRegistrationId = netI(X.val('CmbFinInstruments'));
            r.FinancialInstrumentNo = fi ? X.text('CmbFinInstruments') : '';
            r.PrcntOfTotal = netD(X.val('dtxtpercentoftotal'));
            r.FcyAmount = netD(X.val('dtxtfcyamount'));
            r.DueDays = X.val('dtxtdueday');
            r.Remarks = X.val('txtPaymentRemarksDetail');
            X.show('btndadd', true); X.show('btndupdate', false); X.show('btndcancel', false);
            resetPaymentDetails();
            drawPay();
            X.focus('dcmbpaymentterm');
        } catch (e) { box(e.message); }
    }
    function btndcancel() {
        X.show('btndadd', true); X.show('btndupdate', false); X.show('btndcancel', false);
        resetPaymentDetails();
        drawPay();
    }
    function payDblClick(i) {
        var r = S.pay[i]; if (!r) return;
        S.udiPT = i;
        setT('dcmbpaymentterm', r.PaymentTermId);
        setT('CmbFinInstruments', r.ExImEFormRegistrationId);
        X.setText('dtxtpercentoftotal', X.raw(r.PrcntOfTotal));
        X.setText('dtxtfcyamount', X.raw(r.FcyAmount));
        X.setText('dtxtdueday', str(r.DueDays));
        X.setText('txtPaymentRemarksDetail', r.Remarks);
        X.show('btndadd', false); X.show('btndupdate', true); X.show('btndcancel', true);
        drawPay();
        X.focus('dcmbpaymentterm');
    }
    function payDelete(i) {
        try {
            var r = S.pay[i]; if (!r) return;
            if (S.udiPT !== -1) throw new Error('Reset Detail First...');
            if (netI(r.Id) > 0) throw new Error('You can not Delete this Row...');
            S.pay.splice(i, 1);
            drawPay();
        } catch (e) { box(e.message); }
    }
    function resetPaymentDetails() {
        S.udiPT = -1;
        setT('dcmbpaymentterm', '0');
        setT('CmbFinInstruments', '0');
        X.setText('dtxtpercentoftotal', '0');
        X.setText('dtxtfcyamount', '0');
        X.setText('dtxtdueday', '0');
    }
    /* dcmbfino_ValueChanged */
    function fiChanged() {
        var fi = fiRow(); if (!fi) return;
        setT('dcmbpaymentterm', fi.PaymenttermId);
        X.getJson(API + '/fi-balance?documentTypeId=' + netI(fi.DocumnetTypeId) + '&id=' + netI(fi.Id))
            .then(function (d) { X.setText('txtBalanceFI', f00(d && d.balance)); })
            .catch(function (e) { box(e.message); });
    }
    function termDescription(list, id) {
        var v = netI(id); for (var i = 0; i < list.length; i++) if (netI(list[i].Id) === v) return str(list[i].Description);
        return '';
    }

    // ------------------------------------------------------------------------------ customer (cmbSupCust_Leave)

    function customerLeave() {
        return X.getJson(API + '/customer?customerId=' + netI(X.val('cmbSupCust')) + '&itemId=' + netI(X.val('combitem'))).then(function (d) {
            d = d || {};
            L.fis = d.fis || [];
            X.bind('CmbFinInstruments', L.fis, 'Id', 'EFormNo');                     /* BindFinancialInstrument */
            X.bind('cmbConsignee', d.consignees || [], 'Id', 'CompanyName');          /* bindConsigneeAgainstCustomer */
            bindHs(d.hsCodes || []);                                                   /* HSCodeBind */
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ toolbar

    /* FormReset (+ DetailFormReset - btnNew_Click). */
    function formReset() {
        S.removed = [];
        showMode(false);
        S.recId = 0;
        return X.getJson(API + '/reset').then(function (d) {
            d = d || {};
            if (netI(d.docNo) > 0) X.setText('txtdocno', d.docNo);
            setInvoiceNoState(!!d.autoGenInvoiceNo, d.invoiceNo);
            X.setVal('cmbbranches', '0');
            X.setVal('cmbproject', '0');
            X.setVal('cmbcareiertype', '1');
            X.setText('txtDocDate', X.today());
            X.setVal('cmbSupCust', '0');
            return customerLeave();
        }).then(function () {
            ['cmbConsignee', 'cmbNotifyParty1', 'cmbNotifyParty2', 'cmbimporterBankNew', 'cmbdeliverytermnew', 'cmbLoadingPort', 'cmbDestinationPort', 'cmbexporterBankNew', 'cmbfcycode']
                .forEach(function (id) { setT(id, '0'); });
            setT('cmbPaymentTermsNew', '0');
            ['txtcertificate', 'txtcertificateOfOrigion', 'txtLotRef', 'txtLocalAmt', 'txtGrossWeight', 'txtNoOfContainer', 'txtIformNo', 'txtaddlesscommnets', 'txtTotalNetAmount']
                .forEach(function (id) { X.setText(id, ''); });
            S.detail = []; S.other = []; S.pay = [];
            setT('txtfcyAmount', '');
            setT('txtExRate', '');
            setT('txtNetWeight', '');
            setT('txtaddlessamount', '');
            X.setText('txtTotalNetAmount', '');
            drawDetail(); drawOther(); drawPay();
            S.udiPT = -1;
            X.setVal('combitem', '0'); S.items = []; X.bind('combitem', [], 'Id', 'Name', null, '0');
            X.setText('txtDocDate', X.today());
            X.setText('txtIformDate', X.today());
            X.focus('txtDocDate');
            detailFormReset();
            resetPaymentDetails();
        });
    }
    function btnNew(btn) { return X.busy(btn, function () { return formReset().then(function () { detailFormReset(); }); }); }
    function btnRefresh(btn) {
        return X.busy(btn, function () {
            return X.getJson(API + '/refresh').then(function (d) {
                d = d || {};
                bindCombos(d);
                setInvoiceNoState(!!d.autoGenInvoiceNo, d.invoiceNo);
                if (netI(X.val('combitem')) > 0) return bindRateUomAndItemPackUom();   /* HSCodeBind */
            });
        });
    }

    /* formvalidation */
    function formValid() {
        var c = [
            [CFG.allowExportMultiCompanies && netI(X.val('CmbExportCompany')) === 0, 'Export Company Is required', 'CmbExportCompany'],
            [X.val('txtdocno') === '' || netI(X.val('txtdocno')) === 0, 'Doc No Is Required', 'txtdocno'],
            [X.val('cmbInvoiceNo') === '' || X.val('txtdocno') === '', 'Invoice No Is Required', 'cmbInvoiceNo'],
            [netI(X.val('cmbSupCust')) === 0, 'Customer Is Required', 'cmbSupCust'],
            [netI(X.val('cmbPaymentTermsNew')) === 0, 'Payment Term Is Required', 'cmbPaymentTermsNew'],
            [netI(X.val('cmbdeliverytermnew')) === 0, 'Delivery Term Is Required', 'cmbdeliverytermnew'],
            [netI(X.val('cmbLoadingPort')) === 0, 'Loading Port Is Required', 'cmbLoadingPort'],
            [netI(X.val('cmbDestinationPort')) === 0, 'Destination Port  Is Required', 'cmbDestinationPort'],
            [netI(X.val('cmbcareiertype')) === 0, 'Carrier Type  Is Required', 'cmbcareiertype'],
            [netI(X.val('cmbfcycode')) === 0, 'Fcy Code Is Required', 'cmbfcycode'],
            [netD(X.val('txtfcyAmount')) === 0, 'Fcy Amount  Is Required', 'txtfcyAmount'],
            [netI(X.val('txtNoOfContainer')) === 0, 'No.of Containers Field Is Required', 'txtNoOfContainer'],
            [!(netD(X.val('txtNetWeight')) > 0), 'Net Weight  Is Required', 'txtNetWeight'],
            [!(netD(X.val('txtGrossWeight')) >= netD(X.val('txtNetWeight'))), 'Gross Weight Must Be Equal Or Greater Than Net Weight Thank You', 'txtGrossWeight'],
            [!(netD(X.val('txtExRate')) > 0), 'ExchangeRate Field is Required', 'txtExRate'],
            [!(netD(X.val('txtTotalNetAmount')) > 0), 'Total Amount  Is Required', 'txtTotalNetAmount'],
            [netI(X.val('CmbCreditAccount')) === 0, 'Please select CreditAc', 'CmbCreditAccount']
        ];
        for (var i = 0; i < c.length; i++) if (c[i][0]) { box(c[i][1]); X.focus(c[i][2]); return false; }
        return true;
    }
    function header() {
        return {
            ProjectsId: netI(X.val('cmbproject')), DocCode: netI(X.val('txtdocno')), DocDate: X.val('txtDocDate'), InvoiceNo: X.val('cmbInvoiceNo').trim(),
            SupplierCustomerId: netI(X.val('cmbSupCust')), ConsigneeId: netI(X.val('cmbConsignee')), NotifyParty1: netI(X.val('cmbNotifyParty1')),
            NotifyParty2: netI(X.val('cmbNotifyParty2')), LotNoRef: X.val('txtLotRef'), EFormNo: X.val('txtIformNo').trim(), EFormDate: X.val('txtIformDate'),
            FcurrencyId: netI(X.val('cmbfcycode')), ConversionRate: netD(X.val('txtExRate')), FCurrencyAmount: netD(X.val('txtfcyAmount')),
            EquivalentAmount: netD(X.val('txtLocalAmt')), AddLessAmount: netD(X.val('txtaddlessamount')), TotalAmount: netD(X.val('txtTotalNetAmount')),
            AddLessComments: X.val('txtaddlesscommnets'), PaymentTermId: netI(X.val('cmbPaymentTermsNew')), DeliveryTermId: netI(X.val('cmbdeliverytermnew')),
            PaymentRemarks: X.val('txtPaymentRemarks'), DeliveryRemarks: X.val('txtDeliveryRemarks'), ImporterBankId: netI(X.val('cmbimporterBankNew')),
            ExporteBankId: netI(X.val('cmbexporterBankNew')), LoadingPortId: netI(X.val('cmbLoadingPort')), DestinationPortId: netI(X.val('cmbDestinationPort')),
            CarierType: X.text('cmbcareiertype'), GrossMton: netD(X.val('txtGrossWeight')), NetMton: netD(X.val('txtNetWeight')),
            ExportCompanyId: netI(X.val('CmbExportCompany')), NoOfContainers: netI(X.val('txtNoOfContainer')), Certificate1: X.val('txtcertificate').trim(),
            Certificate2: X.val('txtcertificateOfOrigion').trim(), CreditAccountId: netI(X.val('CmbCreditAccount'))
        };
    }
    /* btnsave_Click */
    function save(btn) {
        try {
            if (S.detail.length <= 0) throw new Error('Grid Record not found');
            if (!ask(S.recId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
            X.setText('txtNetWeight', f3(sum(S.detail, 'QtyMTon')));
            if (!formValid()) return;
            if (!S.pay.length) { selectDetailTab(2); X.focus('dcmbpaymentterm'); throw new Error('Payment Detail Not Found. Please Check! '); }
        } catch (e) { box(e.message); return; }
        var preview = $id('ChkPrintPreview').checked;
        var win = preview && global.CrystalPrint ? global.CrystalPrint.reserve() : null;
        var recId = S.recId;
        return X.busy(btn, function () {
            return X.postJson(API + '/save', { recId: recId, header: header(), rows: S.detail, removed: S.removed, otherItems: S.other, payments: S.pay })
                .then(function (d) {
                    box(d.message || (recId === 0 ? 'Save SuccessFully' : 'Update SuccessFully'));
                    var id = netI(d.id);
                    return formReset().then(function () {
                        if (preview) print521(id, null, win); else if (win) global.CrystalPrint.release(win);
                    });
                }).catch(function (e) { if (win) global.CrystalPrint.release(win); throw e; });
        });
    }
    function update(btn) {
        if (S.recId === 0) { box('Rec Id not found...'); return; }
        return save(btn);
    }
    /* InvoiceSlip521 -> CommonServices.CommercialInvoiceSlip521 */
    function print521(id, btn, win) {
        if (!global.CrystalPrint) { if (win) win.close(); box('Print is not available.'); return; }
        if (netI(id) === 0) { if (win) global.CrystalPrint.release(win); box('No Record Found For Display'); return; }
        return global.CrystalPrint.open('521-exportinvoiceslip', { eximInvoiceId: netI(id) }, btn, win);
    }
    function print(btn) { return print521(S.recId, btn); }
    function attachment() { box('Attachments are not available in the web version.'); }
    function generateInvoiceNos() { box('Generate Export Invoice Nos is not available in the web version.'); }
    function itemDetailLookUp() { box('Commodity Detail Item Customer Wise is not available in the web version.'); }

    // ------------------------------------------------------------------------------ ReadById

    function readById(id) {
        return X.getJson(API + '/by-id?id=' + netI(id)).then(function (d) {
            var h = d.header || {};
            showMode(true);
            S.recId = netI(id);
            X.selectTab('eo', 'eoForm'); tabChanged('eoForm');
            X.setVal('cmbbranches', col(h, 'BranchesId'));
            X.setVal('cmbproject', col(h, 'ProjectsId'));
            X.setText('txtdocno', str(col(h, 'DocCode')));
            X.setText('txtDocDate', X.isoDate(col(h, 'DocDate')));
            X.setText('cmbInvoiceNo', str(col(h, 'InvoiceNo')));
            X.setVal('cmbSupCust', col(h, 'SupplierCustomerId'));
            /* cmbSupCust_Leave - the by-id response carries the customer's FI / consignee / HS lists */
            var c = d.customer || {};
            L.fis = c.fis || [];
            X.bind('CmbFinInstruments', L.fis, 'Id', 'EFormNo');
            X.bind('cmbConsignee', c.consignees || [], 'Id', 'CompanyName');
            bindHs(c.hsCodes || []);
            X.setVal('cmbConsignee', col(h, 'ConsigneeId'));
            X.setVal('cmbNotifyParty1', col(h, 'NotifyParty1'));
            X.setVal('cmbNotifyParty2', col(h, 'NotifyParty2'));
            X.setText('txtLotRef', str(col(h, 'LotNoRef')));
            setT('cmbPaymentTermsNew', col(h, 'PaymentTermId'));
            X.setVal('cmbimporterBankNew', col(h, 'ImporterBankId'));
            X.setVal('cmbexporterBankNew', col(h, 'ExporteBankId'));
            setT('cmbdeliverytermnew', col(h, 'DeliveryTermId'));
            X.setVal('cmbLoadingPort', col(h, 'LoadingPortId'));
            X.setVal('cmbDestinationPort', col(h, 'DestinationPortId'));
            setComboText('cmbcareiertype', col(h, 'CarierType'));
            X.setVal('cmbfcycode', col(h, 'FcurrencyId'));
            setT('txtfcyAmount', f4(col(h, 'FCurrencyAmount')));
            setT('txtExRate', f4(col(h, 'ConversionRate')));
            X.setText('txtLocalAmt', f4(col(h, 'EquivalentAmount')));
            X.setVal('CmbExportCompany', col(h, 'ExportCompanyId'));
            var gm = netD(col(h, 'GrossMton')), nm = netD(col(h, 'NetMton'));
            X.setText('txtGrossWeight', gm > 0 ? f4(gm) : f4(netD(col(h, 'GrossWeight')) / 1000.0));
            setT('txtNetWeight', nm > 0 ? f4(nm) : f4(netD(col(h, 'NetWeight')) / 1000.0));
            X.setText('txtNoOfContainer', String(netI(col(h, 'NoOfContainers'))));
            X.setText('txtIformNo', str(col(h, 'EFormNo')));
            X.setText('txtIformDate', X.isoDate(col(h, 'EFormDate')));
            X.setText('txtcertificate', str(col(h, 'Certificate1')));
            X.setText('txtcertificateOfOrigion', str(col(h, 'Certificate2')));
            X.setText('txtPaymentRemarks', str(col(h, 'PaymentRemarks')));
            X.setText('txtDeliveryRemarks', str(col(h, 'DeliveryRemarks')));
            X.setText('txtaddlesscommnets', str(col(h, 'AddLessComments')));
            setT('txtaddlessamount', X.raw(col(h, 'AddLessAmount')));
            X.setText('txtTotalNetAmount', f4(col(h, 'TotalAmount')));
            X.setVal('CmbCreditAccount', col(h, 'CreditAccountId'));
            var items = [], seen = {};
            S.detail = (d.detail || []).map(function (r) {
                var itemId = netI(col(r, 'ItemId'));
                if (!seen[itemId]) { seen[itemId] = 1; items.push({ Id: itemId, Name: str(col(r, 'ItemName')) }); }
                var rwal = netD(col(r, 'RateWithoutAddLess'));
                return {
                    Id: netI(col(r, 'Id')), ContractId: netI(col(r, 'ContractId')), ContractDetailId: netI(col(r, 'ContractDetailId')),
                    ContractNo: str(col(r, 'PriRefNo')), ContractDate: col(r, 'ProformaDocDate'), ItemId: itemId, ItemName: str(col(r, 'ItemName')),
                    PackTypeId: netI(col(r, 'PackingMaterialTypeId')), PackType: str(col(r, 'PackMaterilaType')), CropYearId: netI(col(r, 'CropYearId')),
                    CropYear: str(col(r, 'CropYear')), JobLotId: netI(col(r, 'JobLotId')), JobLot: str(col(r, 'JobLotDescription')), QtyMTon: netD(col(r, 'MTon')),
                    PackSizeId: netI(col(r, 'OuterQtyUomId')), PackSize: str(col(r, 'OuterUOM')), NoOfBags: netD(col(r, 'OuterQty')),
                    RateWithoutAddLess: rwal > 0 ? rwal : netD(col(r, 'RatePrice')), RateAddLess: netD(col(r, 'RateAddLess')), CostMTon: netD(col(r, 'RatePrice')),
                    ContractRate: netD(col(r, 'ContractRate')), RateUOMId: netI(col(r, 'RateUomId')), RateUOM: str(col(r, 'RateUOM')),
                    RateEquivalent: netD(col(r, 'RateEquivalent')), Amount: netD(col(r, 'FcAmount')), HSCode: str(col(r, 'HsCode')),
                    ItemDetail: str(col(r, 'ItemDescriptionManual')), PackingDetail: str(col(r, 'OuterPackDescription')), OtherItemAmount: 0,
                    ExImFarmingNTradeId: netI(col(r, 'ExImFarmingNTradeId')), FarmingNTrade: str(col(r, 'FarmingNTrade')),
                    ExImFarmingTypeId: netI(col(r, 'ExImFarmingTypeId')), ExImTradeTypeId: netI(col(r, 'ExImTradeTypeId'))
                };
            });
            /* InvoiceUpdateFlag = ExImInvoiceDetail[0].ContractType (index error when there is no detail row, as on the desktop). */
            if (!(d.detail || []).length) throw new Error('Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index');
            S.invoiceUpdateFlag = netI(col(d.detail[0], 'ContractType'));
            if (items.length) { S.items = items; X.bind('combitem', items, 'Id', 'Name', null, '0'); }
            var oitems = [], oseen = {};
            S.other = (d.otherItems || []).map(function (r) {
                var oid = netI(col(r, 'otherItemId'));
                if (!oseen[oid]) { oseen[oid] = 1; oitems.push({ Id: oid, Name: str(col(r, 'ItemName')) }); }
                return { ItemId: oid, ItemName: str(col(r, 'ItemName')), Qty: netD(col(r, 'oItemQty')), Rate: netD(col(r, 'oItemRate')),
                    Amount: netD(col(r, 'oItemAmount')), Remarks: str(col(r, 'OtherItemRemarks')) };
            });
            if (oitems.length) { S.oitems = oitems; X.bind('CmbOtherItem', oitems, 'Id', 'Name', null, '0'); }
            S.pay = (d.payments || []).map(function (r) {
                return { Id: netI(col(r, 'Id')), PaymentTermId: netI(col(r, 'PaymentTermId')), PaymentTerm: str(col(r, 'PaymentTerm')),
                    DocumentTypeId: netI(col(r, 'DocumentTypeId')), ExImEFormRegistrationId: netI(col(r, 'ExImEFormRegistrationId')),
                    FinancialInstrumentNo: str(col(r, 'FinancialInstrumentNo')), PrcntOfTotal: netD(col(r, 'PrcntOfTotal')), FcyAmount: netD(col(r, 'FcyAmount')),
                    DueDays: col(r, 'DueDays'), Remarks: str(col(r, 'PaymentRemarks')) };
            });
            S.removed = [];
            drawOther(); drawPay();
            proportionate();
            detailFormReset();
            X.focus('txtDocDate');
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ history

    function bindHistoryCustomers(rows) { X.bind('CmbCustomerHistory', rows, 'Id', 'name'); }
    function historyReset() {
        X.setText('FromDateHistory', X.daysAgo(3));
        X.setText('ToDateHistory', X.today());
        X.setText('FromDocNoHistory', ''); X.setText('ToDocNoHistory', '');
        X.setVal('CmbCustomerHistory', '0');
        H.rows = []; H.cur = -1; H.detail = [];
        drawHistory(); drawHistoryDetail();
    }
    function historyRefresh(btn) {
        return X.busy(btn, function () { return X.getJson(API + '/history-customers').then(function (rows) { bindHistoryCustomers(rows || []); }); });
    }
    function historyShow(btn) {
        return X.busy(btn, function () {
            return X.postJson(API + '/history', {
                dateKind: X.radio('eoDateKind') || 'doc',
                fromChecked: $id('FromDateHistoryChk').checked, fromDate: X.val('FromDateHistory'),
                toChecked: $id('ToDateHistoryChk').checked, toDate: X.val('ToDateHistory'),
                fromDocNo: netI(X.val('FromDocNoHistory')), toDocNo: netI(X.val('ToDocNoHistory')),
                supplierCustomerId: netI(X.val('CmbCustomerHistory'))
            }).then(function (rows) {
                H.rows = rows || []; H.cur = -1; H.detail = [];
                drawHistory(); drawHistoryDetail();
            });
        });
    }
    function drawHistory() {
        var lead = [];
        if (PERM.Update) lead.push({ t: 'Edit', html: function () { return X.btnHtml('Edit', 'Edit'); } });
        if (PERM.Print) lead.push({ t: 'Slip', html: function () { return X.btnHtml('Slip', '521-Slip'); } });
        X.grid('DataGridHistory', {
            cols: HIST_COLS, rows: H.rows, lead: lead, current: H.cur >= 0 ? H.cur : undefined, empty: 'DataGridHistoryEmpty',
            onRow: function (i) { historySelect(i); },
            onDbl: function (i) { readById(H.rows[i].Id); },
            onButton: function (i, act, b) {
                if (act === 'Slip') print521(H.rows[i].Id, b);
                else if (act === 'Edit') readById(H.rows[i].Id);
            },
            onCode: function (i, k) { if (k === 'NoOfAttachments') attachment(); else { historySelect(i); readById(H.rows[i].Id); } },
            onKey: function (i, e) { if (e.ctrlKey && e.key === 'Enter' && PERM.Update) { e.preventDefault(); readById(H.rows[i].Id); } }
        });
    }
    /* DataGridHistory_SelectionChanged -> GetDetailByHeaderId */
    function historySelect(i) {
        if (H.cur === i) return;
        H.cur = i;
        var row = H.rows[i]; if (!row) return;
        X.getJson(API + '/by-id?id=' + netI(row.Id)).then(function (d) {
            H.detail = (d.detail || []).map(function (r) {
                var rwal = netD(col(r, 'RateWithoutAddLess'));
                return { ContractNo: str(col(r, 'PriRefNo')), ItemName: str(col(r, 'ItemName')), PackType: str(col(r, 'PackMaterilaType')),
                    CropYear: str(col(r, 'CropYear')), JobLot: str(col(r, 'JobLotDescription')), QtyMTon: netD(col(r, 'NetWeight')) / 1000.0,
                    PackSize: str(col(r, 'OuterUOM')), NoOfBags: netD(col(r, 'OuterQty')), RateWithoutAddLess: rwal > 0 ? rwal : netD(col(r, 'RatePrice')),
                    RateAddLess: netD(col(r, 'RateAddLess')), CostMTon: netD(col(r, 'RatePrice')), CostUom: str(col(r, 'RateUOM')),
                    Amount: netD(col(r, 'FcAmount')), HSCode: str(col(r, 'HsCode')), ItemDetail: str(col(r, 'ItemDescriptionManual')),
                    FarmingNTrade: str(col(r, 'FarmingNTrade')), PackingDetail: str(col(r, 'OuterPackDescription')) };
            });
            if (H.detail.length) drawHistoryDetail();
        }).catch(function (e) { box(e.message); });
    }
    function drawHistoryDetail() { X.grid('gridDetailHistory', { cols: histDetailCols(), rows: H.detail }); }

    // ------------------------------------------------------------------------------ loader (LoadSalesContractForExportOpening)

    function loaderOpen(btn) {
        return X.busy(btn, function () {
            LD = { rows: [], main: [], checked: {}, cur: -1 };
            X.grid('ldGrd', { cols: LD_COLS, rows: [] }); X.grid('ldGrdDetail', { cols: LD_DETAIL_COLS, rows: [] });
            $id('ldSelectAll').checked = false;
            X.modal('loaderModal', true);
            return X.getJson(API + '/loader/setup').then(function (d) {
                d = d || {};
                X.setText('ldFromDate', X.isoDate(d.fromDate) || X.today());
                X.setText('ldTodate', X.today());
                loaderCombos(d);
                X.focus('ldFromDate');
                return loaderRows();
            });
        });
    }
    function loaderCombos(d) {
        X.bind('ldCmbPartyName', d.parties || [], 'Id', 'name', null, '0');
        X.bind('ldCmbItemName', d.items || [], 'Id', 'name', null, '0');
        X.bind('ldCmbCurrency', d.currencies || [], 'Id', 'name', null, '0');
    }
    /* ExportInvoicesLoad */
    function loaderRows() {
        return X.postJson(API + '/loader/rows', {
            fromDate: X.val('ldFromDate'), toDate: X.val('ldTodate'), supplierCustomerId: netI(X.val('ldCmbPartyName')),
            itemId: netI(X.val('ldCmbItemName')), fcyId: netI(X.val('ldCmbCurrency'))
        }).then(function (rows) {
            LD.rows = rows || []; LD.checked = {}; LD.cur = -1;
            var seen = {};
            LD.main = [];
            LD.rows.forEach(function (r) {
                var id = netI(col(r, 'Id')); if (seen[id]) return; seen[id] = 1;
                LD.main.push({ Id: id, DocumentTypeId: col(r, 'DocumentTypeId'), DocNo: col(r, 'DocNo'), ContractNo: col(r, 'ProformaNo'),
                    ContractDate: col(r, 'ProformaDate'), OrderDate: col(r, 'SalesContratDate'), ScheduleStatus: col(r, 'ContractScheduleStatus') ? 'Exist' : 'Not Exist',
                    SupplierCustomerId: col(r, 'SupCustId'), PartyName: col(r, 'CustomerName'), SalesMan: col(r, 'SalesMan'), ExporterBank: col(r, 'ExporterBank'),
                    ShipmentStartDate: col(r, 'ShipmentStartDate'), DeliveryDays: col(r, 'DeliveryDays'), LastShipmentDate: col(r, 'LastShipmentDate'),
                    CustomerApprovedDate: col(r, 'CustomerApprovalDate'), CustomerApprovalRemarks: col(r, 'CustomerApprovalRemarks'), FCurrencyId: col(r, 'FCurrencyId'),
                    CurrencyCode: col(r, 'CurrencyCode'), ExchangeRate: col(r, 'ExchangeRate'), NoOfContainers: col(r, 'NoOfContainers'),
                    DeliveryTerm: col(r, 'DeliveryTerm'), PaymentTerm: col(r, 'PaymentTerm'), LoadingPort: col(r, 'LoadingPort'),
                    DestinationPort: col(r, 'DestinationPort'), InsuranceRemarks: col(r, 'InsuranceRemarks'), RemarksHeader: col(r, 'RemarksHeader'),
                    EntryUser: col(r, 'EntryUser'), EntryDate: col(r, 'EntryDate'), ModifyUser: col(r, 'ModifyUser'), ModifyDate: col(r, 'ModifyDate'),
                    NoOfAttachments: col(r, 'NoOfAttachments') });
            });
            $id('ldSelectAll').checked = false;
            drawLoader();
            X.grid('ldGrdDetail', { cols: LD_DETAIL_COLS, rows: [] });
        });
    }
    function drawLoader() {
        X.grid('ldGrd', {
            cols: LD_COLS, rows: LD.main, current: LD.cur >= 0 ? LD.cur : undefined, empty: 'ldGrdEmpty',
            lead: [{ t: 'Select', html: function (r) { return X.checkHtml('sel', !!LD.checked[r.Id]); } }],
            onRow: function (i) { loaderSelect(i); },
            onCode: function () { attachment(); }
        });
        var tb = $id('ldGrd').tBodies[0];
        if (tb) tb.addEventListener('change', function (e) {
            if (e.target.type !== 'checkbox') return;
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            var r = LD.main[parseInt(tr.getAttribute('data-i'), 10)];
            if (e.target.checked) LD.checked[r.Id] = 1; else delete LD.checked[r.Id];
        });
    }
    /* grd_SelectionChanged */
    function loaderSelect(i) {
        LD.cur = i;
        var id = netI(LD.main[i].Id);
        var rows = LD.rows.filter(function (r) { return netI(col(r, 'Id')) === id; }).map(function (r) {
            return { DetailId: col(r, 'ContractDetailId'), ItemId: col(r, 'ExImItemId'), ItemName: col(r, 'ItemName'), PackingType: col(r, 'PackingType'),
                CropYear: col(r, 'CropYear'), CommodityDetail: col(r, 'CommodityDetail'), PackUom: col(r, 'PackUom'), MTon: col(r, 'M_Ton'),
                ShipMTon: col(r, 'ShipMTon'), BalMTon: col(r, 'BalMTon'), NoOfBags: col(r, 'NoOfBags'), ShipBags: col(r, 'ShipBags'),
                BalBags: col(r, 'BalNoofBags'), NetWeight: col(r, 'NetWeight'), ShipWeight: col(r, 'ShipWeight'), BalWeight: col(r, 'BalWeight'),
                ItemRate: col(r, 'RatePrice'), Amount: col(r, 'Amount'), ShipAmount: col(r, 'ShipAmount'), BalAmount: col(r, 'BalShipAmount') };
        });
        X.grid('ldGrdDetail', { cols: LD_DETAIL_COLS, rows: rows });
    }
    function loaderSearch(btn) { return X.busy(btn, function () { return loaderRows(); }); }
    /* btnReset_Click */
    function loaderReset(btn) {
        return X.busy(btn, function () {
            X.focus('ldFromDate');
            return X.getJson(API + '/loader/setup').then(function (d) { loaderCombos(d || {}); return loaderRows(); });
        });
    }
    function loaderClose() { X.modal('loaderModal', false); }
    /* btnLoadOnInvoice_Click_1 */
    function loaderLoad(btn) {
        var checked = LD.main.filter(function (r) { return LD.checked[r.Id]; });
        if (!checked.length) { box('Check the row first'); return; }
        var cust = 0, cur = 0, ids = [];
        for (var i = 0; i < checked.length; i++) {
            var c = netI(checked[i].SupplierCustomerId), f = netI(checked[i].FCurrencyId);
            if (cust === 0) { cust = c; cur = f; }
            if (cust !== c || cur !== f) { box('Sorry! The selected Contract\'s are not of the same Customer or Currency.'); return; }
            ids.push(String(checked[i].Id));
            cust = c; cur = f;
        }
        var data = LD.rows.filter(function (r) { return ids.indexOf(String(netI(col(r, 'Id')))) >= 0; });
        X.modal('loaderModal', false);
        return X.busy(btn, function () { return bindLoaderData(data); });
    }
    /* BindLoaderData */
    function bindLoaderData(contract) {
        if (!contract.length) return Promise.resolve();
        var c0 = contract[0];
        if (S.detail.length > 0 && (netI(col(c0, 'SupCustId')) !== netI(X.val('cmbSupCust')) || netI(col(c0, 'FcurrencyId')) !== netI(X.val('cmbfcycode')))) {
            box('Already Loaded Row\'s Have Different Customer And Currency. So you Can\'t Load Rows Of Different Customer or Currency!');
            return Promise.resolve();
        }
        X.setEnabled('cmbSupCust', false);
        X.setEnabled('cmbfcycode', false);
        X.setVal('cmbSupCust', netI(col(c0, 'SupCustId')));
        return customerLeave().then(function () {
            X.setVal('cmbNotifyParty1', netI(col(c0, 'NotifyPartyId')));
            X.setVal('cmbexporterBankNew', netI(col(c0, 'ExporterBankId')));
            setT('cmbPaymentTermsNew', netI(col(c0, 'PaymentTermId')));
            X.setVal('cmbfcycode', netI(col(c0, 'FcurrencyId')));
            setT('cmbdeliverytermnew', netI(col(c0, 'DeliveryTermId')));
            X.setVal('cmbLoadingPort', netI(col(c0, 'LoadingPortId')));
            X.setVal('cmbDestinationPort', netI(col(c0, 'DestinationPortId')));
            X.setVal('CmbExportCompany', netI(col(c0, 'ExportCompanyId')));
            var items = [], seen = {};
            contract.forEach(function (r) {
                var cd = netI(col(r, 'ContractDetailId'));
                var exists = S.detail.some(function (x) { return netI(x.ContractDetailId) === cd; });
                if (exists) return;
                var itemId = netI(col(r, 'ExImItemId'));
                if (!seen[itemId]) { seen[itemId] = 1; items.push({ Id: itemId, Name: str(col(r, 'ItemName')) }); }
                var rate = netD(col(r, 'RatePrice'));
                S.detail.push({
                    Id: 0, ContractId: netI(col(r, 'Id')), ContractDetailId: cd, ContractNo: str(col(r, 'ProformaNo')), ContractDate: col(r, 'ProformaDate'),
                    ItemId: itemId, ItemName: str(col(r, 'ItemName')), PackTypeId: netI(col(r, 'InvPackingMaterialTypeId')), PackType: str(col(r, 'PackingTYpe')),
                    CropYearId: netI(col(r, 'CropYearId')), CropYear: str(col(r, 'CropYear')), JobLotId: netI(col(r, 'JobLotId')), JobLot: str(col(r, 'JobLotDescription')),
                    QtyMTon: netD(col(r, 'BalMTon')), PackSizeId: netI(col(r, 'PackUomId')), PackSize: str(col(r, 'PackUom')), NoOfBags: netD(col(r, 'BalNoofBags')),
                    RateWithoutAddLess: rate, RateAddLess: 0, CostMTon: rate, ContractRate: rate, RateUOMId: netI(col(r, 'RateUomId')), RateUOM: str(col(r, 'RateUom')),
                    RateEquivalent: netD(col(r, 'RateEquivalent')), Amount: netD(col(r, 'BalShipAmount')), HSCode: '', ItemDetail: str(col(r, 'CommodityDetail')),
                    PackingDetail: '', OtherItemAmount: 0, ExImFarmingNTradeId: netI(col(r, 'ExImFarmingNTradeId')), FarmingNTrade: str(col(r, 'FarmingNTrade')),
                    ExImFarmingTypeId: netI(col(r, 'ExImFarmingTypeId')), ExImTradeTypeId: netI(col(r, 'ExImTradeTypeId'))
                });
            });
            /* combitem is rebound with the newly loaded items only (DDL.BindDDL(dtItem, combitem ...) when dtItem has rows). */
            if (items.length) { S.items = items; X.bind('combitem', items, 'Id', 'Name', null, '0'); }
            var distinct = [];
            contract.forEach(function (r) { var id = String(netI(col(r, 'Id'))); if (distinct.indexOf(id) < 0) distinct.push(id); });
            return X.getJson(API + '/loader/bind?ids=' + encodeURIComponent(distinct.join(',')));
        }).then(function (d) {
            if (!d) return;
            var oitems = [], oseen = {};
            S.other = (d.otherItems || []).map(function (r) {
                var oid = netI(col(r, 'otherItemId'));
                if (!oseen[oid]) { oseen[oid] = 1; oitems.push({ Id: oid, Name: str(col(r, 'ItemName')) }); }
                return { ItemId: oid, ItemName: str(col(r, 'ItemName')), Qty: netD(col(r, 'oItemQty')), Rate: netD(col(r, 'oItemRate')),
                    Amount: netD(col(r, 'oItemAmount')), Remarks: str(col(r, 'OtherItemRemarks')) };
            });
            S.oitems = oitems;
            X.bind('CmbOtherItem', oitems, 'Id', 'Name', null, '0');
            proportionate();
            S.pay = (d.paymentTerms || []).map(function (r) {
                return { Id: 0, PaymentTermId: netI(col(r, 'PaymentTermId')), PaymentTerm: str(col(r, 'LcOrderTerm')), DocumentTypeId: 0, ExImEFormRegistrationId: 0,
                    FinancialInstrumentNo: '', PrcntOfTotal: netD(col(r, 'PrcntOfTotal')), FcyAmount: netD(col(r, 'FcyAmount')), DueDays: col(r, 'DueDays'),
                    Remarks: str(col(r, 'Remarks')) };
            });
            drawPay(); drawOther(); drawDetail();
            setT('txtExRate', X.raw(col(contract[0], 'ExchangeRate')));
            getTotalNet();
        });
    }

    // ------------------------------------------------------------------------------ shortcuts

    var SHORTCUTS = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
        ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'],
        ['Ctrl+L', 'For Load All Records'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowRight', 'For Moving In Detail Tabs'],
        ['Ctrl+ArrowDown', 'For Focus On Selected Tab Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On First Entry of Detail Selected Tab'],
        ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function shortcuts() { X.shortcuts(SHORTCUTS); }
    function gridFocus(id) { var t = $id(id); var b = t && t.querySelector('tbody tr'); if (b) { b.setAttribute('tabindex', '-1'); b.focus(); } }
    /* ImProformaInvoice_KeyDown */
    function keyDown(e) {
        if (!$id('loaderModal').classList.contains('is-hidden')) { if (e.key === 'Escape') loaderClose(); return; }
        var k = (e.key || '').toLowerCase();
        if ((e.ctrlKey && k === 'e') || k === 'escape') { e.preventDefault(); X.cancel(); return; }
        if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); return; }
        if (e.ctrlKey && e.altKey) { shortcuts(); return; }
        if (!onHistory()) {
            if (e.ctrlKey && k === 's' && !S.updateMode) { e.preventDefault(); save($id('btnsave')); }
            else if (e.ctrlKey && k === 'u' && S.updateMode) { e.preventDefault(); save($id('btnupdate')); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew($id('btnNew')); }
            else if (e.ctrlKey && k === 'p' && PERM.Print) { e.preventDefault(); print($id('btnPrint')); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); btnRefresh($id('btnRefresh')); }
            else if (e.ctrlKey && k === 'f10') { e.preventDefault(); attachment(); }
            else if (e.ctrlKey && k === 'f5') { e.preventDefault(); X.focus('txtDocDate'); }
            else if (e.ctrlKey && k === 'arrowdown') { e.preventDefault(); gridFocus(['grdDetail', 'grdotheritems', 'grdpaymentdetail'][detailTab()]); }
            else if (e.ctrlKey && k === 'arrowup') { e.preventDefault(); X.focus(['combpcktype', 'CmbOtherItem', 'dcmbpaymentterm'][detailTab()]); }
            else if (e.ctrlKey && k === 'arrowright') {
                e.preventDefault();
                var t = detailTab();
                if (t === 0) { selectDetailTab(1); X.focus('CmbOtherItem'); } else if (t === 1) { selectDetailTab(2); X.focus('dcmbpaymentterm'); } else { selectDetailTab(0); X.focus('combitem'); }
            }
            return;
        }
        if (e.ctrlKey && k === 's') { e.preventDefault(); historyShow($id('btnShowHistory')); }
        else if (e.ctrlKey && k === 'n') { e.preventDefault(); historyReset(); }
        else if (e.ctrlKey && k === 'r') { e.preventDefault(); historyRefresh($id('btnRefreshHistory')); }
        else if (e.ctrlKey && k === 'f5') { e.preventDefault(); X.focus('FromDateHistory'); }
        else if (e.ctrlKey && (k === 'arrowdown' || k === 'arrowright')) { e.preventDefault(); gridFocus($id('DataGridHistory').contains(document.activeElement) ? 'gridDetailHistory' : 'DataGridHistory'); }
        else if (e.ctrlKey && k === 'arrowup') { e.preventDefault(); X.focus('FromDateHistory'); }
    }

    // ------------------------------------------------------------------------------ wiring

    function wire() {
        X.tabs('eo', tabChanged);
        X.tabs('eoDet');
        X.fullscreen();
        HANDLERS.txtfcyAmount = totalAmountCalculate;                 /* txtfcyAmount_TextChanged -> txtaddlessamount_TextChanged */
        HANDLERS.txtaddlessamount = totalAmountCalculate;
        HANDLERS.txtExRate = calculateLocalAmount;
        HANDLERS.txtQtyMTon = calcWeightAmount;
        HANDLERS.txtCostMTon = calcWeightAmount;
        HANDLERS.CmbPackUom = calcWeightAmount;                       /* combitempck_TextChanged */
        HANDLERS.combpcktype = calcWeightAmount;
        HANDLERS.CmbRateUom = calcWeightAmount;                       /* combrateuom_TextChanged */
        HANDLERS.txtoQty = otherCalc;
        HANDLERS.CmbFinInstruments = fiChanged;
        HANDLERS.dcmbpaymentterm = function () { var t = termDescription(L.paymentTerms, X.val('dcmbpaymentterm')); if (netI(X.val('dcmbpaymentterm')) !== 0 && t) X.setText('txtPaymentRemarksDetail', t); };
        HANDLERS.cmbPaymentTermsNew = function () { var t = termDescription(L.paymentTerms, X.val('cmbPaymentTermsNew')); if (netI(X.val('cmbPaymentTermsNew')) !== 0 && t) X.setText('txtPaymentRemarks', t); };
        HANDLERS.cmbdeliverytermnew = function () { var t = termDescription(L.deliveryTerms, X.val('cmbdeliverytermnew')); if (netI(X.val('cmbdeliverytermnew')) !== 0 && t) X.setText('txtDeliveryRemarks', t); };
        ['txtaddlessamount', 'txtExRate', 'txtQtyMTon', 'txtCostMTon', 'txtoQty'].forEach(function (id) { X.on(id, 'input', function () { fire(id); }); });
        ['CmbPackUom', 'combpcktype', 'CmbRateUom', 'CmbFinInstruments', 'dcmbpaymentterm', 'cmbPaymentTermsNew', 'cmbdeliverytermnew'].forEach(function (id) { X.on(id, 'change', function () { fire(id); }); });
        X.on('dtxtpercentoftotal', 'input', percentCalc);
        X.on('dtxtfcyamount', 'input', fcyPercent);
        X.onLeave('cmbSupCust', customerLeave);
        X.onLeave('combitem', function () { bindRateUomAndItemPackUom().catch(function (e) { box(e.message); }); });
        X.on('ldSelectAll', 'change', function () {
            var on = $id('ldSelectAll').checked;
            LD.checked = {}; if (on) LD.main.forEach(function (r) { LD.checked[r.Id] = 1; });
            drawLoader();
        });
        document.addEventListener('keydown', keyDown);
    }

    global.ExportOpening = {
        btnNew: btnNew, btnRefresh: btnRefresh, save: save, update: update, attachment: attachment, print: print, shortcuts: shortcuts,
        generateInvoiceNos: generateInvoiceNos, itemDetailLookUp: itemDetailLookUp, toggleHistory: toggleHistory,
        btnAddinGrid: btnAddinGrid, btnGridUpdate: btnGridUpdate, btnCancel: btnCancel,
        btnoadd: btnoadd, btnoUpdate: btnoUpdate, btnoCancel: btnoCancel,
        btndadd: btndadd, btndupdate: btndupdate, btndcancel: btndcancel,
        historyReset: historyReset, historyRefresh: historyRefresh, historyShow: historyShow,
        loaderOpen: loaderOpen, loaderClose: loaderClose, loaderSearch: loaderSearch, loaderReset: loaderReset, loaderLoad: loaderLoad
    };
    document.addEventListener('DOMContentLoaded', function () { wire(); load(); });
}(window));
