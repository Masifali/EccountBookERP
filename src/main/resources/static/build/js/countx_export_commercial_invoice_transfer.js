/* ============================================================================================
 * countx_export_commercial_invoice_transfer.js - CommiercialInvoiceAgainstPreInvoiceTransfer.cs (Architecture.WinApp.Export),
 * screen 202 "Commercial Invoice" (ExImInvoice, DocumentTypeId 211) with the loader popup LoadForwardingForCommercialInvoice.
 * API /api/export/commercial-invoice-transfer.
 * The desktop's TextChanged / ValueChanged chains that fire when the code itself sets a box are replayed by setFire() /
 * setCombo(), which - like the WinForms controls - only fire when the text / value really changes; the Leave events are
 * the select 'change' / input 'blur' events of the page.
 * Quirks kept on purpose:
 *   - "+" only answers "Sorry Cannot Add New Record Only Update Detail Record : Thank You" (rows come from Load Forwarding);
 *   - closing the loader without "Load" still runs LoadInGridDetail with an empty table: the Other Items are cleared;
 *   - New (FormReset) hides the Payment Detail rows but keeps them, so they come back with the next "+"; the Other Items stay;
 *   - one shared edit index for Other Items and Payment Detail ("Reset Detail First..." after editing an other item);
 *   - the Other Items rate is only taken from the item list in update mode (btnupdate visible and enabled);
 *   - getTotalnetWeightFromGrid only runs while the grid has a row, so deleting the last row keeps the header totals;
 *   - Voucher-103 prints the VoucherHeadId of the last history double-click (never reset by New);
 *   - ReadById fills the HSCode column with the Bank HS Code; a document without detail rows stops ReadById half way.
 * Button contract: disabled + spinner while a request runs, no duplicates, re-enabled on success and failure.
 * ============================================================================================ */
(function (global) {
    'use strict';

    var X = global.ExR2;
    var $id = X.$id, box = X.box, esc = X.esc, str = X.str, netI = X.netI, netD = X.netD, clr = X.clr;
    var API = '/api/export/commercial-invoice-transfer';
    var DOC_TYPE = 211;
    var CARIER = [{ Id: 1, Type: 'By Sea' }, { Id: 2, Type: 'By Air' }, { Id: 3, Type: 'By Road' }];
    var PERM = { Save: true, Update: true, Print: true };
    var FMT = { rate: '#,#0.00', fcyRate: '#,#0.00', fcyAmount: '#,##0.00' };
    var S = {
        recId: 0, updateMode: false, rows: [], removed: [], updateIdx: -1, cur: -1, invoiceUpdateFlag: 0, voucherHeadId: 0,
        commissionDebitAcId: 0, dtIN: [], other: [], sharedIdx: -1, pay: [], payHidden: false, uoms: [], fis: [], otherItemList: [], defaultDays: 0
    };
    var HIST = [], HCUR = -1;
    var LD = { raw: [], open: false };

    function fmt(v, p) { return X.fmtNet(v, p); }

    // ------------------------------------------------------------------------------ tabs

    var TABS = ['tabPage1', 'tabPage2', 'tabPage7', 'GDHistory', 'InvoicesAllocationToGd'];
    function tabIndex() { for (var i = 0; i < TABS.length; i++) if ($id(TABS[i]).classList.contains('is-active')) return i; return 0; }
    function tab(panelId) {
        document.querySelectorAll('.win-tabs[data-tabs="ecit"] .win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === panelId); });
        TABS.forEach(function (p) { $id(p).classList.toggle('is-active', p === panelId); });
        var onHist = panelId === 'tabPage2';
        var fb = $id('btnEcitFooterHistory');
        fb.querySelector('span').textContent = onHist ? 'Form' : 'History';
        fb.querySelector('i').className = onHist ? 'fa fa-file-text-o' : 'fa fa-history';
        /* tabControl1_SelectedIndexChanged */
        if (onHist) X.focus('FromDateHistory'); else if (panelId === 'tabPage1') X.focus('txtDocDate');
    }
    function toggleHistory() { tab(tabIndex() === 1 ? 'tabPage1' : 'tabPage2'); }
    function subtab(id) {
        document.querySelectorAll('[data-subtabs="ecit"] .win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-subtab') === id); });
        document.querySelectorAll('[data-subpanel="ecit"]').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
    }

    // ------------------------------------------------------------------------------ change replay

    var HANDLERS = {};
    function setFire(id, v) {
        var e = $id(id); if (!e) return;
        v = str(v);
        if (e.value === v) return;
        e.value = v;
        if (HANDLERS[id]) HANDLERS[id]();
    }
    /** UltraCombo.Value = v (or Text = "" for v ''): fires the combo's TextChanged / ValueChanged handler when it changed. */
    function setCombo(id, v) {
        var s = $id(id); if (!s) return;
        var before = s.value;
        X.setVal(id, v === null || v === undefined ? '' : String(v));
        if (s.value !== before && HANDLERS[id]) HANDLERS[id]();
    }
    function setComboText(id, t) {
        var s = $id(id); if (!s) return;
        var before = s.value;
        X.setByText(id, t);
        if (s.value !== before && HANDLERS[id]) HANDLERS[id]();
    }
    function guard(fn) { return function () { try { fn(); } catch (e) { box(e.message); } }; }

    // ------------------------------------------------------------------------------ calculations

    function uomEq(id) { var v = X.val(id); if (v === '') return null; var u = X.find(S.uoms, 'Id', v); return u ? netD(u.Equivalent) : 0; }

    /** CalculateWeight. */
    function calculateWeight() {
        try {
            var c = netD(X.val('txtNoOfContainersDetail')), b = netD(X.val('txtNoOfBagsPerCntnr'));
            if (c > 0 && b > 0) {
                var total = c * b, check = netD(X.val('txtqtycheck'));
                if (total > check) {
                    setFire('txtNoOfBagsPerCntnr', '0');
                    X.setText('txtNoOfBags', fmt(check, '#,##0.###'));
                    throw new Error("Total No Of Bags Can't Be Greater Than Balance No. Of Bags Which Is " + clr(check));
                }
                X.setText('txtNoOfBags', clr(total));
                var pw = netD(X.val('txtPackingWeight'));
                X.setText('txttotalpackingweight', fmt(total * pw, '#,##0.###'));
                var eq = uomEq('CmbPackUom');
                if (eq !== null) {
                    X.setText('txtQtyMTon', fmt(total * eq / 1000.0, '#,##0.####'));
                    X.setText('txtNetWeightdetail', fmt(total * eq, '#,##0.###'));
                    X.setText('txtgrossWeightdetail', fmt(total * eq + total * pw, '#,##0.###'));
                } else {
                    X.setText('txtNoOfBags', '0');
                    X.setText('txtQtyMTon', '0');
                }
            }
        } catch (e) { box(e.message); }
    }
    /** CalculatAmount. */
    function calculatAmount() {
        X.setText('txtAmount', '0');
        var eq = uomEq('CmbRateUom');
        if (eq !== null) {
            var rate = netD(X.val('txtCostMTon')), other = netD(X.val('txtOtherRateDetail')), qty = netD(X.val('txtQtyMTon'));
            if (rate > 0 && eq > 0 && qty > 0) {
                var kg = qty * 1000.0;
                X.setText('txtAmount', fmt(rate / eq * kg, FMT.fcyAmount));
                X.setText('txtOtherAmountDetil', fmt(other / eq * kg, FMT.fcyAmount));
            }
        } else {
            X.setText('txtAmount', '0');
            X.setText('txtOtherAmountDetil', '0');
        }
    }
    function weightAndAmount() { calculateWeight(); calculatAmount(); }
    /** txtOtherRateDetail_TextChanged: + decimal.Parse(Bank Amount).ToString("#,##0.00"). */
    function bankRateChanged() {
        calculateWeight(); calculatAmount();
        var s = X.val('txtOtherAmountDetil').replace(/,/g, '').trim();
        if (!/^[-+]?(\d+\.?\d*|\.\d+)$/.test(s)) { box('Input string was not in a correct format.'); return; }
        X.setText('txtOtherAmountDetil', fmt(parseFloat(s), '#,##0.00'));
    }
    /** CalculateNetRate. */
    function calculateNetRate() {
        var al = netD(X.val('txtAddLessRate'));
        if (al !== 0) setFire('txtCostMTon', fmt(netD(X.val('txtRateWithoutAddLess')) + al, FMT.rate));
        else setFire('txtCostMTon', fmt(netD(X.val('txtRateWithoutAddLess')), FMT.rate));
    }
    function sum(list, c) { var s = 0; list.forEach(function (r) { s += netD(r[c]); }); return s; }
    /** txtCommPercent_TextChanged. */
    function commPercentChanged() {
        var r = netD(X.val('txtCommPercent'));
        if (r > 100) { box('Commission % Can not Greater than 100...'); setFire('txtCommPercent', '100'); return; }
        if (S.rows.length) {
            var total = sum(S.rows, 'Amount');
            X.setText('txtCommAmount', isNaN(total) ? '0' : clr(X.roundEven(total * r / 100.0, 0)));
        }
    }
    /** CalculateLocalAmount. */
    function calculateLocalAmount() {
        var f = netD(X.val('txtTotalNetAmount')), ex = netD(X.val('txtExRate'));
        X.setText('txtLocalAmt', f > 0 && ex > 0 ? fmt(f * ex, '0,0') : '0');
    }
    function visiblePay() { return S.payHidden ? [] : S.pay; }
    /** TotalAmountCalculate. */
    function totalAmountCalculate() {
        var other = sum(S.other, 'Amount'), addless = netD(X.val('txtaddlessamount')), fcy = netD(X.val('txtfcyAmount'));
        if (fcy > 0) X.setText('txtTotalNetAmount', fmt(fcy + addless + other, '#,#.###'));
        var pay = visiblePay();
        if (netD(X.val('txtTotalNetAmount')) > 0 && pay.length) {
            var t = netD(X.val('txtTotalNetAmount'));
            pay.forEach(function (p) { p.FcyAmount = netD(p.PrcntOfTotal) * t / 100.0; });
            renderPay();
        }
        calculateLocalAmount();
    }
    /** getTotalnetWeightFromGrid (only while the grid has a current row). */
    function getTotalnetWeightFromGrid() {
        if (!S.rows.length) return;
        X.setText('txtNetWeight', fmt(sum(S.rows, 'NetWeight'), '#,#.##'));
        X.setText('txtGrossWeight', fmt(sum(S.rows, 'GrossWeight'), '#,#.##'));
        setFire('txtfcyAmount', fmt(sum(S.rows, 'Amount'), '#,#.##'));
        X.setText('txtNoOfContainer', clr(sum(S.rows, 'NoOfContainers')));
    }
    /** OtherItemsCalculations. */
    function otherItemsCalculations() {
        var q = netD(X.val('txtoQty')), r = netD(X.val('txtoRate'));
        X.setText('txtoAmount', q > 0 && r > 0 ? clr(q * r) : '0');
    }
    /** CmbOtherItem_TextChanged: the item's rate, in update mode only. */
    function otherItemChanged() {
        if (X.visible('btnupdate') && !$id('btnupdate').disabled && X.val('CmbOtherItem') !== '') {
            var it = X.find(S.otherItemList, 'ItemId', X.val('CmbOtherItem'));
            setFire('txtoRate', it ? clr(it.oItemRate) : '');
        }
    }
    /** PaymentDetailPercentCalculate (Leave of % of Total and Balance). */
    function paymentPercentCalculate() {
        if (S.rows.length && netD(X.val('dtxtpercentoftotal')) > 0) {
            if (netD(X.val('dtxtpercentoftotal')) > 100) { X.setText('dtxtpercentoftotal', '0'); box('Percent cannot be greater than 100 Please check'); return; }
            var total = sum(S.rows, 'OtherAmount'), pct = netD(X.val('dtxtpercentoftotal'));
            if (total > 0 && pct > 0) X.setText('dtxtfcyamount', fmt(total * pct / 100.0, FMT.fcyAmount));
        }
    }
    /** dtxtfcyamount Leave. */
    function fcyAmountLeave() {
        var total = sum(S.rows, 'OtherAmount'), f = netD(X.val('dtxtfcyamount'));
        if (total > 0 && f > 0) {
            X.setText('dtxtpercentoftotal', fmt(f / total * 100.0, '#,##0.###'));
            if (netD(X.val('dtxtpercentoftotal')) > 100) { X.setText('dtxtpercentoftotal', '0'); box('Percent cannot be greater than 100 Please check'); }
        }
    }
    function selectedFi() { var v = X.val('CmbFinInstruments'); return v === '' ? null : X.find(S.fis, 'Id', v); }
    /** dcmbfino_ValueChanged -> payment term + GetFinancialInstrumentsBalance. */
    function fiChanged() {
        var fi = selectedFi();
        if (fi && netI(fi.Id) > 0) {
            X.setVal('dcmbpaymentterm', netI(fi.PaymenttermId));
            X.setText('txtBalanceFI', str(fi.BalFIAmount));
        } else if (!fi) X.setText('txtBalanceFI', '0');
    }

    HANDLERS.txtPackingWeight = weightAndAmount;
    HANDLERS.txtNoOfContainersDetail = weightAndAmount;
    HANDLERS.txtNoOfBagsPerCntnr = weightAndAmount;
    HANDLERS.txtCostMTon = weightAndAmount;
    HANDLERS.CmbPackUom = weightAndAmount;
    HANDLERS.CmbRateUom = weightAndAmount;
    HANDLERS.txtOtherRateDetail = bankRateChanged;
    HANDLERS.txtAddLessRate = calculateNetRate;
    HANDLERS.txtRateWithoutAddLess = calculateNetRate;
    HANDLERS.txtCommPercent = commPercentChanged;
    HANDLERS.txtExRate = calculateLocalAmount;
    HANDLERS.txtaddlessamount = totalAmountCalculate;
    HANDLERS.txtfcyAmount = totalAmountCalculate;
    HANDLERS.txtoQty = otherItemsCalculations;
    HANDLERS.CmbOtherItem = otherItemChanged;
    HANDLERS.CmbFinInstruments = fiChanged;

    // ------------------------------------------------------------------------------ load / combos

    function activateFirst(id) { var s = $id(id); if (s && s.options.length > 1) { s.selectedIndex = 1; X.refreshCombos(); } }
    function clearCombo(id) { X.bind(id, [], 'Id', 'Id'); }
    function bindKeep(id, rows, v, t) { X.bind(id, rows, v, t, { keep: true }); }
    function bindInvoiceNos(rows) { S.dtIN = rows || []; if (S.dtIN.length) bindKeep('Cmbinvoiceno', S.dtIN, 'Id', 'InvoiceNo'); }
    function bindFis(rows) { S.fis = rows || []; if (S.fis.length) bindKeep('CmbFinInstruments', S.fis, 'Id', 'EFormNo'); }
    function applyLookups(d, initial) {
        if (d.branches && d.branches.length) { X.bind('cmbbranches', d.branches, 'Id', 'BranchName'); activateFirst('cmbbranches'); }
        bindInvoiceNos(d.invoiceNos);
        if (d.prefixTypes && d.prefixTypes.length) bindKeep('cmbPrefixType', d.prefixTypes, 'Id', 'PrefixDescription'); else if (d.prefixTypes) { X.setVal('cmbPrefixType', ''); clearCombo('cmbPrefixType'); }
        if (d.projects && d.projects.length) { X.bind('cmbproject', d.projects, 'Id', 'ProjectName'); activateFirst('cmbproject'); }
        if (d.customers && d.customers.length)
            ['cmbSupCust', 'cmbNotifyParty1', 'cmbOtherCustomer', 'cmbNotifyParty2', 'cmbCommissionAgent'].forEach(function (id) { bindKeep(id, d.customers, 'Id', 'CompanyName'); });
        if (d.deliveryTerms && d.deliveryTerms.length) bindKeep('cmbdeliverytermnew', d.deliveryTerms, 'Id', 'Code');
        if (d.paymentTerms && d.paymentTerms.length) { bindKeep('cmbPaymentTermsNew', d.paymentTerms, 'Id', 'LcOrderTerm'); bindKeep('dcmbpaymentterm', d.paymentTerms, 'Id', 'LcOrderTerm'); }
        if (d.ports && d.ports.length) ['cmbLoadingPort', 'cmbDestinationPort', 'cmbOtherDestinationPort'].forEach(function (id) { bindKeep(id, d.ports, 'Id', 'PortName'); });
        if (d.currencies && d.currencies.length) bindKeep('cmbfcycode', d.currencies, 'Id', 'CurrencyName');
        if (d.banks) {
            if (d.banks.exporter && d.banks.exporter.length) bindKeep('cmbexporterBankNew', d.banks.exporter, 'Id', 'BranchName');
            if (d.banks.importer && d.banks.importer.length) bindKeep('cmbimporterBankNew', d.banks.importer, 'Id', 'BranchName');
        }
        X.bind('cmbcareiertype', CARIER, 'Id', 'Type'); activateFirst('cmbcareiertype');
        if (d.packTypes && d.packTypes.length) bindKeep('combpcktype', d.packTypes, 'Id', 'PackTypeDesc');
        if (d.cropYears && d.cropYears.length) bindKeep('CmbCropYear', d.cropYears, 'Id', 'CropYear');
    }

    /** ImProformaInvoice_Load. */
    function load() {
        return X.getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false, Print: d.permissions.Print !== false };
            if (d.formats) FMT = d.formats;
            $id('btnsave').disabled = !PERM.Save;
            $id('btnupdate').disabled = !PERM.Update;
            if (d.docNo) X.setText('txtdocno', d.docNo);
            if (d.preInvoices && d.preInvoices.length) X.bind('CmbPreInvoice', d.preInvoices, 'Id', 'InvoiceNo');
            applyLookups(d, true);
            if (d.commissionDebitAccounts && d.commissionDebitAccounts.length) X.bind('CmbCommissionDebitAccount', d.commissionDebitAccounts, 'Id', 'AccountTitle');
            S.commissionDebitAcId = netI(d.lastCommissionDebitAcId);
            if (S.commissionDebitAcId > 0) X.setVal('CmbCommissionDebitAccount', S.commissionDebitAcId);
            X.setText('txtDocDate', X.today()); X.setText('txtIformDate', X.today());
            S.defaultDays = netI(d.defaultDays);
            X.bind('CmbCustomerHistory', d.historyCustomers, 'Id', 'Customer');
            X.setText('FromDateHistory', X.addDays(X.today(), S.defaultDays > 0 ? -S.defaultDays : -3));
            X.setText('ToDateHistory', X.today());
            showEntry(false);
            subtab('tabPage3');
            render(); renderOther(); renderPay();
            $id('ecitFooterInfo').textContent = 'CommiercialInvoiceAgainstPreInvoiceTransfer  -  Document Type 211';
            X.focus('CmbPreInvoice');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }

    function generateCode() {
        return X.getJson(API + '/new-code').then(function (d) { if (d && d.docNo) X.setText('txtdocno', d.docNo); }).catch(function (e) { box(e.message); });
    }
    /** BindCommercialInvoiceNofromlookup(InvoiceNo). */
    function bindCommercialInvoiceNos(invoiceNo) {
        return X.getJson(API + '/invoice-nos' + (invoiceNo ? '?invoiceNo=' + encodeURIComponent(invoiceNo) : '')).then(bindInvoiceNos).catch(function (e) { box(e.message); });
    }
    /** BindFinancialInstrument. */
    function bindFinancialInstrument() {
        return X.getJson(API + '/fis').then(bindFis).catch(function (e) { box(e.message); });
    }

    /** cmbPrefixType_TextChanged (Leave). */
    function prefixChanged() {
        if (X.val('cmbPrefixType') !== '' || netI(X.val('cmbPrefixType')) > 0) {
            X.setVal('Cmbinvoiceno', '');
            var p = netI(X.val('cmbPrefixType'));
            X.bind('Cmbinvoiceno', S.dtIN.filter(function (r) { return netI(r.PrefixTypeId) === p; }), 'Id', 'InvoiceNo');
        } else {
            bindKeep('Cmbinvoiceno', S.dtIN, 'Id', 'InvoiceNo');
        }
    }

    function clearRaw(id) { var s = $id(id); if (!s) return; s.querySelectorAll('option[data-raw]').forEach(function (o) { o.parentNode.removeChild(o); }); }
    /** UltraCombo.Text / Value that is not in the list: the text stays as typed. */
    function setRawText(id, t) {
        clearRaw(id);
        if (X.setByText(id, t) || str(t) === '') return;
        var s = $id(id), o = document.createElement('option');
        o.value = '__raw'; o.text = str(t); o.setAttribute('data-raw', '1'); s.appendChild(o); s.value = '__raw'; X.refreshCombos();
    }
    function setRawValue(id, v) {
        clearRaw(id);
        var s = $id(id); v = str(v);
        X.setVal(id, v);
        if (s.value === v || v === '' || v === '0') return;
        var o = document.createElement('option');
        o.value = v; o.text = v; o.setAttribute('data-raw', '1'); s.appendChild(o); s.value = v; X.refreshCombos();
    }

    /** cmbSupCust_Leave_1 / CmbPreInvoice Leave: BindPreInvoice, BindFinancialInstrument, BindHeaderInformPreInvoiceByPreInvoiceId. */
    function supCustLeave() {
        var cust = netI(X.val('cmbSupCust')), pre = netI(X.val('CmbPreInvoice'));
        return X.getJson(API + '/customer-leave?customerId=' + cust + '&preInvoiceId=' + pre).then(function (d) {
            var list = d.preInvoices || [];
            if (list.length) bindKeep('CmbPreInvoice', list, 'Id', 'InvoiceNo');
            else { X.setVal('CmbPreInvoice', ''); clearCombo('CmbPreInvoice'); }
            bindFis(d.fis);
            var h = d.preInvoiceHeader;
            if (h && netI(X.val('CmbPreInvoice')) === pre && pre > 0) {
                X.setVal('cmbSupCust', netI(h.SupplierCustomerId));
                X.setVal('cmbfcycode', netI(h.FCurrencyId));
                setFire('txtExRate', clr(h.ConversionRate));
                X.setVal('cmbLoadingPort', netI(h.LoadingPortId));
                X.setVal('cmbPaymentTermsNew', netI(h.PaymentTermId));
                X.setVal('cmbDestinationPort', netI(h.DestinationPortId));
                X.setVal('cmbdeliverytermnew', netI(h.DeliveryTermId));
                X.setVal('cmbOtherCustomer', netI(h.OtherCustomerId));
                X.setVal('cmbOtherDestinationPort', netI(h.OtherDestinationPortId));
                if (netI(h.NotifyParty1) > 0) X.setVal('cmbNotifyParty1', netI(h.NotifyParty1));
                if (netI(h.NotifyParty2) > 0) X.setVal('cmbNotifyParty2', netI(h.NotifyParty2));
                X.setText('txtNoOfContainer', clr(h.NoOfContainers));
                X.setText('txtLotRef', str(h.LotReference));
                X.setVal('cmbCommissionAgent', netI(h.CommissionAgentId));
                setFire('txtCommPercent', clr(h.CommRate));
                X.setText('txtCommAmount', clr(h.CommAmount));
                X.setText('txtoRemarks', str(h.RemarksHeader));
            }
        }).catch(function (e) { box(e.message); });
    }

    /** bindRateUomAndItemPackUom (combitem Leave). */
    function bindRateUom(itemId) {
        return X.getJson(API + '/uoms?itemId=' + netI(itemId)).then(function (rows) {
            S.uoms = rows || [];
            if (S.uoms.length) { X.bind('CmbPackUom', S.uoms, 'Id', 'UOMCode'); X.bind('CmbRateUom', S.uoms, 'Id', 'UOMCode'); }
            else { clearCombo('CmbPackUom'); clearCombo('CmbRateUom'); }
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ detail grid

    var COLS = [
        { key: 'ItemName' }, { key: 'HSCode' }, { key: 'PackType' }, { key: 'PackingWeight', fmt: '#,##0.###', sum: '#,##0.###' }, { key: 'CropYear' },
        { key: 'NoOfContainers', num: true }, { key: 'NoOfBagsCntnr', num: true }, { key: 'PackSize' },
        { key: 'QtyMTon', fmt: '#,##0.###', sum: '#,##0.###' }, { key: 'NoOfBags', fmt: '#,##0.###', sum: '#,##0.###' },
        { key: 'NetWeight', fmt: '#,##0.###', sum: '#,##0.###' }, { key: 'TotalPackingWeight', fmt: '#,##0.###', sum: '#,##0.###' },
        { key: 'GrossWeight', fmt: '#,##0.###', sum: '#,##0.###' },
        { key: 'RateWithoutAddLess', fmtKey: 'fcyRate' }, { key: 'AddLessRate', fmtKey: 'fcyRate' }, { key: 'CostMTon', fmtKey: 'fcyRate' }, { key: 'RateUOM' },
        { key: 'Amount', fmtKey: 'fcyAmount', sumKey: 'fcyAmount' }, { key: 'OtherRate', fmtKey: 'fcyRate' }, { key: 'OtherAmount', fmtKey: 'fcyAmount', sumKey: 'fcyAmount' },
        { key: 'OtherHSCode' }, { key: 'PackingExpiryDate' }, { key: 'ProductionNo' }, { key: 'ItemCommodityDetail' }, { key: 'HealthPermitNoDetail' }
    ];
    function cols() {
        return COLS.map(function (c) {
            var o = {}; for (var k in c) o[k] = c[k];
            if (c.fmtKey) o.fmt = FMT[c.fmtKey];
            if (c.sumKey) o.sum = FMT[c.sumKey];
            return o;
        });
    }
    function render() {
        X.drawGrid('ecitBody', 'ecitFoot', S.rows, cols(), {
            current: S.cur, leadCount: 1,
            lead: function (r, i) { return '<td class="win-cell-btn"><button type="button" class="win-x" data-del="' + i + '">X</button></td>'; }
        });
    }
    function showEntry(editing) { X.show('btnAddinGrid', !editing); X.show('btnGridUpdate', editing); X.show('btnCancel', editing); }

    /** btnAddinGrid_Click. */
    function btnAddinGrid() { box('Sorry Cannot Add New Record Only Update Detail Record : Thank You'); }

    /** grdDetail_DoubleClick. */
    function rowToEntry(i) {
        var r = S.rows[i]; if (!r) return Promise.resolve();
        X.setVal('combitem', ''); clearCombo('combitem');
        X.setVal('CmbPackUom', ''); clearCombo('CmbPackUom');
        X.setVal('CmbRateUom', ''); clearCombo('CmbRateUom');
        S.updateIdx = i; S.cur = i; render();
        X.bind('combitem', [{ ItemId: r.ItemId, ItemName: r.ItemName }], 'ItemId', 'ItemName');
        X.setVal('combitem', r.ItemId);
        return bindRateUom(r.ItemId).then(function () {
            X.setText('txtItemHsCodeDetail', str(r.HSCode));
            X.setVal('combpcktype', r.PackTypeId);
            setFire('txtPackingWeight', fmt(r.PackingWeight, '#,##0.###'));
            X.setVal('CmbCropYear', r.CropYearId);
            X.setByText('CmbCropYear', r.CropYear);
            X.setText('txtqtycheck', clr(r.ValidateBags));
            setFire('txtNoOfContainersDetail', clr(r.NoOfContainers));
            setFire('txtNoOfBagsPerCntnr', clr(r.NoOfBagsCntnr));
            setCombo('CmbPackUom', r.PackSizeId);
            X.setText('txtQtyMTon', fmt(r.QtyMTon, '#,##0.####'));
            X.setText('txtNoOfBags', fmt(r.NoOfBags, '#,##0.###'));
            X.setText('txtNetWeightdetail', fmt(r.NetWeight, '#,##0.###'));
            X.setText('txtweightcheck', clr(r.ValidateNetWeight));
            X.setText('txttotalpackingweight', fmt(r.TotalPackingWeight, '#,##0.###'));
            X.setText('txtgrossWeightdetail', fmt(r.GrossWeight, '#,##0.###'));
            setCombo('CmbRateUom', r.RateUOMId);
            setFire('txtRateWithoutAddLess', fmt(r.RateWithoutAddLess, FMT.rate));
            setFire('txtAddLessRate', fmt(r.AddLessRate, FMT.rate));
            setFire('txtCostMTon', fmt(r.CostMTon, FMT.rate));
            X.setText('txtAmount', fmt(r.Amount, FMT.fcyAmount));
            setFire('txtOtherRateDetail', clr(r.OtherRate));
            X.setText('txtOtherAmountDetil', fmt(r.OtherAmount, FMT.fcyAmount));
            X.setText('txtOtherHSCodeDetail', str(r.OtherHSCode));
            X.setText('txtPackingExpiryDate', str(r.PackingExpiryDate));
            X.setText('txtProductionNo', str(r.ProductionNo));
            X.setText('txtItemCommodityDetail', str(r.ItemCommodityDetail));
            X.setText('txtHealthPermitnodetail', str(r.HealthPermitNoDetail));
            showEntry(true);
            /* txtCostMTon.ReadOnly = InvoiceUpdateFlag == 0 (the box is disabled on the form anyway) */
            calculatAmount();
            calculateLocalAmount();
        });
    }

    /** DetailFormValidation. */
    function detailValidation() {
        var checks = [
            [X.val('combitem') === '', 'Item field required', 'combitem'],
            [X.val('CmbPackUom') === '', 'Pack Type field required', 'CmbPackUom'],
            [X.val('txtPackingWeight').trim() === '' || !(netD(X.val('txtPackingWeight')) > 0), 'PackingWeight field required', 'txtPackingWeight'],
            [X.val('txtNoOfContainersDetail').trim() === '' || !(netD(X.val('txtNoOfContainersDetail')) > 0), 'NoOfContainer field required', 'txtNoOfContainersDetail'],
            [X.val('txtNoOfBagsPerCntnr').trim() === '' || !(netD(X.val('txtNoOfBagsPerCntnr')) > 0), 'NoOfBagsPerCntnr field required', 'txtNoOfBagsPerCntnr'],
            [X.val('txtQtyMTon').trim() === '' || !(netD(X.val('txtQtyMTon')) > 0), 'Qty/M.Ton field required', 'txtQtyMTon'],
            [X.val('txtNoOfBags').trim() === '' || !(netD(X.val('txtNoOfBags')) > 0), 'No of Bags field required', 'txtNoOfBags'],
            [X.val('txtNetWeightdetail').trim() === '' || !(netD(X.val('txtNetWeightdetail')) > 0), 'Net Weight field required', 'txtNetWeightdetail'],
            [X.val('txttotalpackingweight').trim() === '' || !(netD(X.val('txttotalpackingweight')) > 0), 'Total Packing Weight field required', 'txttotalpackingweight'],
            [X.val('txtgrossWeightdetail').trim() === '' || !(netD(X.val('txtgrossWeightdetail')) > 0), 'Gross Weight field required', 'txtgrossWeightdetail'],
            [X.val('txtCostMTon').trim() === '' || !(netD(X.val('txtCostMTon')) > 0), 'Cost/M.Ton field required', 'txtCostMTon'],
            [X.val('CmbRateUom') === '', 'Rate UOM field required', 'CmbRateUom'],
            [X.val('txtAmount').trim() === '' || !(netD(X.val('txtAmount')) > 0), 'Amount field required', 'txtAmount'],
            [X.val('txtTotalNetAmount').trim() === '' || !(netD(X.val('txtTotalNetAmount')) > 0), 'Total Net Amount  field required', 'txtTotalNetAmount']
        ];
        for (var i = 0; i < checks.length; i++) if (checks[i][0]) { box(checks[i][1]); X.focus(checks[i][2]); return false; }
        return true;
    }

    /** btnGridUpdate_Click. */
    function btnGridUpdate() {
        if (!detailValidation()) return;
        if (X.visible('btnsave') && !$id('btnsave').disabled && netD(X.val('txtNetWeightdetail')) > netD(X.val('txtweightcheck'))) {
            box('Net Weight Can not be greater than Bal Weight...\n Balance Net Weight Is ' + X.val('txtweightcheck'));
            X.focus('txtNoOfContainersDetail');
            return;
        }
        var r = S.rows[S.updateIdx]; if (!r) return;
        r.ItemId = netI(X.val('combitem')); r.ItemName = X.txt('combitem'); r.HSCode = X.val('txtItemHsCodeDetail');
        r.PackTypeId = netI(X.val('combpcktype')); r.PackType = X.txt('combpcktype'); r.PackingWeight = netD(X.val('txtPackingWeight'));
        r.CropYearId = netI(X.val('CmbCropYear')); r.CropYear = X.txt('CmbCropYear');
        r.NoOfContainers = netD(X.val('txtNoOfContainersDetail')); r.NoOfBagsCntnr = netD(X.val('txtNoOfBagsPerCntnr'));
        r.PackSizeId = netI(X.val('CmbPackUom')); r.PackSize = X.txt('CmbPackUom');
        r.QtyMTon = netD(X.val('txtQtyMTon')); r.NoOfBags = netD(X.val('txtNoOfBags')); r.NetWeight = netD(X.val('txtNetWeightdetail'));
        r.TotalPackingWeight = netD(X.val('txttotalpackingweight')); r.GrossWeight = netD(X.val('txtgrossWeightdetail'));
        r.RateWithoutAddLess = netD(X.val('txtRateWithoutAddLess')); r.AddLessRate = netD(X.val('txtAddLessRate')); r.CostMTon = netD(X.val('txtCostMTon'));
        r.RateUOMId = netI(X.val('CmbRateUom')); r.RateUOM = X.txt('CmbRateUom'); r.Amount = netD(X.val('txtAmount'));
        r.OtherRate = netD(X.val('txtOtherRateDetail')); r.OtherAmount = netD(X.val('txtOtherAmountDetil')); r.OtherHSCode = X.val('txtOtherHSCodeDetail');
        r.PackingExpiryDate = X.val('txtPackingExpiryDate'); r.ProductionNo = X.val('txtProductionNo');
        r.ItemCommodityDetail = X.val('txtItemCommodityDetail'); r.HealthPermitNoDetail = X.val('txtHealthPermitnodetail');
        render();
        getTotalnetWeightFromGrid();
        showEntry(false);
        detailFormReset();
        getTotalnetWeightFromGrid();
        calculateLocalAmount();
        commPercentChanged();
        X.focus('combitem');
    }
    function btnCancel() { showEntry(false); detailFormReset(); }

    /** grdDetail_ColumnButtonClick (X). */
    function deleteRow(i) {
        var r = S.rows[i]; if (!r) return;
        if (netI(r.Id) > 0) {
            if (!X.ask('Are you sure to Delete?')) return;
            S.removed.push(r);
        }
        S.rows.splice(i, 1);
        S.cur = -1;
        render();
        getTotalnetWeightFromGrid();
    }

    /** DetailFormReset (the TextChanged chains of its text clears replay in the desktop order). */
    function detailFormReset() {
        setComboText('combitem', '');
        setComboText('combpcktype', '');
        X.setText('txtQtyMTon', '');
        setComboText('CmbPackUom', '');
        X.setText('txtNoOfBags', '');
        setFire('txtRateWithoutAddLess', '');
        setFire('txtAddLessRate', '');
        setFire('txtCostMTon', '');
        setComboText('CmbRateUom', '');
        X.setText('txtAmount', '');
        X.setText('txtOtherHSCodeDetail', '');
        setFire('txtOtherRateDetail', '');
        X.setText('txtOtherAmountDetil', '');
        ['txtPackingExpiryDate', 'txtItemHsCodeDetail', 'txtHealthPermitnodetail', 'txtItemCommodityDetail'].forEach(function (id) { X.setText(id, ''); });
        setFire('txtNoOfContainersDetail', '');
        setFire('txtNoOfBagsPerCntnr', '');
        X.setText('txtweightcheck', ''); X.setText('txtqtycheck', '');
        X.setText('txttotalpackingweight', '');
        setFire('txtPackingWeight', '');
        X.setText('txtNetWeightdetail', ''); X.setText('txtgrossWeightdetail', ''); X.setText('txtProductionNo', '');
        showEntry(false);
        subtab('tabPage3');
        if (S.commissionDebitAcId > 0) X.setVal('CmbCommissionDebitAccount', S.commissionDebitAcId);
    }

    // ------------------------------------------------------------------------------ other items

    var OCOLS = [{ key: 'ItemName' }, { key: 'Qty', fmt: '0,0', sum: '#,##0.##' }, { key: 'Rate', fmt: '0,0' }, { key: 'Amount', fmt: '0,0', sum: '#,##0.##' }, { key: 'Remarks' }];
    function renderOther() { X.drawGrid('ecitOtherBody', 'ecitOtherFoot', S.other, OCOLS); }
    function showOtherButtons(editing) { X.show('btnoadd', !editing); X.show('btnoUpdate', editing); X.show('btnoCancel', editing); }
    function otherValidation() {
        if (X.val('CmbOtherItem') === '') { box('Item field required'); X.focus('CmbOtherItem'); return false; }
        if (netD(X.val('txtoRate').trim()) === 0) { box('Rate field required'); X.focus('txtoRate'); return false; }
        return true;
    }
    function resetOtherItems() { X.setVal('CmbOtherItem', ''); setFire('txtoQty', ''); X.setText('txtoRate', ''); X.setText('txtoAmount', ''); X.setText('txtoRemarks', ''); }
    function btnoadd() {
        if (!otherValidation()) return;
        S.other.push({ ItemId: X.val('CmbOtherItem'), ItemName: X.txt('CmbOtherItem'), Qty: netD(X.val('txtoQty').trim()), Rate: X.val('txtoRate'),
            Amount: netD(X.val('txtoAmount').trim()), Remarks: X.val('txtoRemarks').trim() });
        renderOther(); resetOtherItems(); totalAmountCalculate();
    }
    function otherToEntry(i) {
        var r = S.other[i]; if (!r) return;
        S.sharedIdx = i;
        setCombo('CmbOtherItem', r.ItemId);
        setFire('txtoQty', clr(r.Qty));
        X.setText('txtoRate', str(r.Rate));
        X.setText('txtoAmount', clr(r.Amount));
        X.setText('txtoRemarks', str(r.Remarks));
        showOtherButtons(true);
        X.focus('txtoQty');
    }
    function btnoUpdate() {
        if (!otherValidation()) return;
        var r = S.other[S.sharedIdx]; if (!r) return;
        r.ItemId = X.val('CmbOtherItem'); r.ItemName = X.txt('CmbOtherItem'); r.Qty = netD(X.val('txtoQty')); r.Rate = X.val('txtoRate');
        r.Amount = netD(X.val('txtoAmount')); r.Remarks = X.val('txtoRemarks');
        renderOther(); showOtherButtons(false); resetOtherItems(); X.focus('txtoQty'); totalAmountCalculate();
    }
    function btnoCancel() { showOtherButtons(false); resetOtherItems(); }

    // ------------------------------------------------------------------------------ payment detail

    var PCOLS = [{ key: 'FinancialInstrumentNo' }, { key: 'PaymentTerm' }, { key: 'PrcntOfTotal', fmt: '#,#.##', sum: '#,#.##' }, { key: 'FcyAmount', fmt: '#,#.##', sum: '#,#.##' }, { key: 'DueDays', num: true }];
    function renderPay() {
        X.drawGrid('ecitPayBody', 'ecitPayFoot', visiblePay(), PCOLS, {
            leadCount: 1, lead: function (r, i) { return '<td class="win-cell-btn"><button type="button" class="win-x" data-pdel="' + i + '">X</button></td>'; }
        });
    }
    function showPayButtons(editing) { X.show('btndadd', !editing); X.show('btndupdate', editing); X.show('btndcancel', editing); }
    function payValidation() {
        if (X.val('CmbFinInstruments') === '') { box('Financial Instrument No field required'); X.focus('CmbFinInstruments'); return false; }
        if (X.val('dcmbpaymentterm') === '') { box('Payment Term field required'); X.focus('dcmbpaymentterm'); return false; }
        if (netD(X.val('dtxtpercentoftotal').trim()) === 0 || X.val('dtxtpercentoftotal') === '') { box('Percent Field Required'); X.focus('dtxtpercentoftotal'); return false; }
        if (netD(X.val('dtxtfcyamount').trim()) === 0 || X.val('dtxtfcyamount') === '') { box('Fcy Amount  Field Required'); X.focus('dtxtfcyamount'); return false; }
        return true;
    }
    function resetPay() {
        S.sharedIdx = -1;
        setCombo('CmbFinInstruments', '');
        X.setVal('dcmbpaymentterm', '');
        X.setText('dtxtpercentoftotal', '0'); X.setText('dtxtfcyamount', '0'); X.setText('dtxtdueday', '0');
        X.focus('CmbFinInstruments');
    }
    function btndadd() {
        if (!payValidation()) return;
        var fi = selectedFi() || {};
        var rows = visiblePay();
        for (var i = 0; i < rows.length; i++) if (netI(rows[i].ExImEFormRegistrationId) === netI(X.val('CmbFinInstruments')) && netI(rows[i].DocumentTypeId) === netI(fi.DocumentTypeId)) {
            box('Financial Instrument No already add in Grid Please Check'); return;
        }
        S.payHidden = false;            /* grdpaymentdetail.DataSource = dtPaymentTerm brings hidden rows back */
        S.pay.push({ Id: 0, DocumentTypeId: netI(fi.DocumentTypeId), ExImEFormRegistrationId: X.val('CmbFinInstruments'), FinancialInstrumentNo: X.txt('CmbFinInstruments'),
            PaymentTermId: X.val('dcmbpaymentterm'), PaymentTerm: X.txt('dcmbpaymentterm'), PrcntOfTotal: netD(X.val('dtxtpercentoftotal').trim()),
            FcyAmount: netD(X.val('dtxtfcyamount').trim()), DueDays: clr(netD(X.val('dtxtdueday').trim())) });
        renderPay(); resetPay();
    }
    function payToEntry(i) {
        var r = visiblePay()[i]; if (!r) return;
        S.sharedIdx = i;
        setCombo('CmbFinInstruments', r.ExImEFormRegistrationId);
        X.setVal('dcmbpaymentterm', r.PaymentTermId);
        X.setText('dtxtpercentoftotal', fmt(r.PrcntOfTotal, '#,##0.###'));
        X.setText('dtxtfcyamount', fmt(r.FcyAmount, FMT.fcyAmount));
        X.setText('dtxtdueday', str(r.DueDays));
        showPayButtons(true);
        X.focus('txtoQty');
    }
    function btndupdate() {
        if (!payValidation()) return;
        var fi = selectedFi() || {};
        var rows = visiblePay();
        for (var i = 0; i < rows.length; i++) if (S.sharedIdx !== i && netI(rows[i].ExImEFormRegistrationId) === netI(X.val('CmbFinInstruments')) && netI(rows[i].DocumentTypeId) === netI(fi.DocumentTypeId)) {
            box('Financial Instrument No already add in Grid Please Check'); return;
        }
        var r = rows[S.sharedIdx]; if (!r) { box('There is no row at position ' + S.sharedIdx + '.'); return; }
        r.DocumentTypeId = netI(fi.DocumentTypeId); r.ExImEFormRegistrationId = X.val('CmbFinInstruments'); r.FinancialInstrumentNo = X.txt('CmbFinInstruments');
        r.PaymentTermId = X.val('dcmbpaymentterm'); r.PaymentTerm = X.txt('dcmbpaymentterm');
        r.PrcntOfTotal = netD(X.val('dtxtpercentoftotal').trim()); r.FcyAmount = netD(X.val('dtxtfcyamount').trim()); r.DueDays = X.val('dtxtdueday');
        renderPay(); showPayButtons(false); resetPay(); X.focus('CmbFinInstruments');
    }
    function btndcancel() { showPayButtons(false); resetPay(); }
    function deletePay(i) {
        var r = visiblePay()[i]; if (!r) return;
        if (S.sharedIdx !== -1) { box('Reset Detail First...'); return; }
        if (netI(r.Id) > 0) { box('You can not Delete this Row...'); return; }
        S.pay.splice(S.pay.indexOf(r), 1);
        renderPay();
    }

    // ------------------------------------------------------------------------------ header

    /** FormReset. */
    function formReset() {
        X.show('btnsave', true); X.show('btnupdate', false);
        $id('Cmbinvoiceno').disabled = false;
        bindCommercialInvoiceNos(null);
        X.setVal('cmbPrefixType', '');
        S.updateMode = false; S.recId = 0;
        generateCode();
        activateFirst('cmbcareiertype');
        X.setText('txtDocDate', X.today());
        clearRaw('CmbPreInvoice'); X.setVal('CmbPreInvoice', '');
        clearRaw('Cmbinvoiceno'); X.setVal('Cmbinvoiceno', '');
        ['cmbNotifyParty1', 'cmbNotifyParty2', 'cmbSupCust', 'cmbimporterBankNew', 'cmbdeliverytermnew', 'cmbCommissionAgent', 'cmbLoadingPort', 'cmbDestinationPort']
            .forEach(function (id) { X.setVal(id, ''); });
        X.setText('txtcertificate', '');
        setFire('txtCommPercent', '');
        X.setText('txtCommAmount', '');
        X.setText('txtcertificateOfOrigion', ''); X.setText('txtLotRef', '');
        X.setVal('cmbexporterBankNew', ''); X.setVal('cmbfcycode', '');
        setFire('txtfcyAmount', '');
        setFire('txtExRate', '');
        X.setText('txtLocalAmt', ''); X.setText('txtGrossWeight', ''); X.setText('txtNetWeight', ''); X.setText('txtNoOfContainer', ''); X.setText('txtGdNo', '');
        X.setText('txtaddlesscommnets', '');
        setFire('txtaddlessamount', '');
        X.setText('txtTotalNetAmount', '');
        S.rows = []; S.cur = -1; S.updateIdx = -1;
        render();
        S.payHidden = true; renderPay();         /* grdpaymentdetail.ClearStructure(): dtPaymentTerm keeps its rows */
        S.sharedIdx = -1;
        X.setVal('combitem', ''); clearCombo('combitem');
        X.focus('cmbSupCust');
        detailFormReset();
        resetPay();
        S.removed = [];
    }
    function btnNew() { formReset(); detailFormReset(); }

    /** btnrefersh_Click. */
    function btnrefersh(btn) {
        return X.busy(btn || 'btnrefersh', function () { return X.getJson(API + '/refresh').then(function (d) { applyLookups(d || {}, false); }); });
    }

    /** formvalidation. */
    function formValidation() {
        var checks = [
            [X.val('txtdocno') === '' || netI(X.val('txtdocno')) === 0, 'Doc No Is Required', 'txtdocno'],
            [X.val('CmbPreInvoice') === '', 'PreInvoice Is Required', 'CmbPreInvoice'],
            [X.val('cmbPrefixType') === '' || netI(X.val('cmbPrefixType')) <= 0, 'Prefix Type Is Required', 'cmbPrefixType'],
            [X.txt('Cmbinvoiceno') === '' || X.txt('Cmbinvoiceno').trim() === '0', 'Invoice No Is Required', 'Cmbinvoiceno'],
            [X.val('cmbSupCust') === '', 'Customer Is Required', 'cmbSupCust'],
            [X.val('cmbPaymentTermsNew') === '', 'Payment Term Is Required', 'cmbPaymentTermsNew'],
            [X.val('cmbdeliverytermnew') === '', 'Delivery Term Is Required', 'cmbdeliverytermnew'],
            [X.val('cmbLoadingPort') === '', 'Loading Port Is Required', 'cmbLoadingPort'],
            [X.val('cmbDestinationPort') === '', 'Destination Port  Is Required', 'cmbDestinationPort'],
            [X.txt('cmbDestinationPort').trim() === X.txt('cmbLoadingPort').trim(), 'Destination Port and Loaing Port cannot be same...', 'cmbDestinationPort'],
            [X.val('cmbcareiertype') === '', 'Carier Type  Is Required', 'cmbcareiertype'],
            [X.val('txtGdNo') === '' || X.val('txtGdNo') === '0', 'GD_number Is Required', 'txtGdNo'],
            [X.val('cmbfcycode') === '', 'Fcy Code Is Required', 'cmbfcycode'],
            [netD(X.val('txtfcyAmount')) === 0, 'Fcy Amount  Is Required', 'txtfcyAmount'],
            [!(netD(X.val('txtNetWeight')) > 0), 'Net Weight  Is Required', 'txtNetWeight'],
            [!(netD(X.val('txtGrossWeight')) >= netD(X.val('txtNetWeight'))), 'Gross Weight Must Be Equal Or Greater Than Net Weight Thank You', 'txtGrossWeight'],
            [netD(X.val('txtCommPercent')) > 0 && X.val('cmbCommissionAgent') === '', "As you have entered 'Commission %', the 'Commission Agent' field is required. ", 'cmbCommissionAgent'],
            [netD(X.val('txtCommPercent')) === 0 && X.val('cmbCommissionAgent') !== '', "As you have selected 'Commission agent', please enter the commission percentage.", 'txtCommPercent'],
            [netD(X.val('txtCommAmount')) > 0 && X.val('cmbCommissionAgent') !== '' && X.val('CmbCommissionDebitAccount') === '', 'Commission DebitAccount Is Required', 'CmbCommissionDebitAccount'],
            [!(netD(X.val('txtExRate')) > 0), 'ExchangeRate Field is Required', 'txtExRate'],
            [!(netD(X.val('txtTotalNetAmount')) > 0), 'Total Amount  Is Required', 'txtTotalNetAmount']
        ];
        for (var i = 0; i < checks.length; i++) if (checks[i][0]) { box(checks[i][1]); X.focus(checks[i][2]); return false; }
        return true;
    }

    function headerBody() {
        var ids = ['cmbbranches', 'cmbproject', 'txtdocno', 'txtDocDate', 'cmbPrefixType', 'CmbPreInvoice', 'cmbSupCust', 'cmbNotifyParty1', 'cmbNotifyParty2',
            'txtLotRef', 'cmbPaymentTermsNew', 'cmbimporterBankNew', 'cmbexporterBankNew', 'cmbdeliverytermnew', 'cmbLoadingPort', 'cmbDestinationPort',
            'cmbOtherDestinationPort', 'cmbOtherCustomer', 'txtCommAmount', 'cmbCommissionAgent', 'txtCommPercent', 'CmbCommissionDebitAccount', 'cmbfcycode',
            'txtfcyAmount', 'txtExRate', 'txtLocalAmt', 'txtGrossWeight', 'txtNoOfContainer', 'txtGdNo', 'txtIformDate', 'txtcertificateOfOrigion',
            'txtcertificate', 'txtaddlesscommnets', 'txtaddlessamount', 'txtTotalNetAmount', 'txtoRemarks'];
        var h = {};
        ids.forEach(function (id) { h[id] = X.val(id); });
        if (h.CmbPreInvoice === '__raw') h.CmbPreInvoice = '';
        h.CmbinvoicenoText = X.txt('Cmbinvoiceno');
        h.cmbcareiertypeText = X.txt('cmbcareiertype');
        h.cmbLoadingPortText = X.txt('cmbLoadingPort');
        h.cmbDestinationPortText = X.txt('cmbDestinationPort');
        return h;
    }

    /** btnsave_Click (btnupdate_Click calls it). */
    function btnsave(btn) {
        if (!S.rows.length) { box('Grid Record not found'); return; }
        if (S.recId === 0) { if (!X.ask('Are you sure to Save?')) return; }
        else if (!X.ask('Are you sure to Update?')) return;
        X.setText('txtNetWeight', clr(sum(S.rows, 'QtyMTon') * 1000.0));
        if (!formValidation()) return;
        var body = { recId: S.recId, header: headerBody(), rows: S.rows, removed: S.removed, otherItems: S.other, paymentTerms: visiblePay() };
        var p549 = X.checked('ChkPrintPreview'), p548 = X.checked('chk548Print'), wasUpdate = S.recId > 0;
        return X.busy(btn || 'btnsave', function () {
            return X.postJson(API + '/save', body).then(function (r) {
                if (r && r.success) {
                    box(r.message || (wasUpdate ? 'Update SuccessFully' : 'Save SuccessFully'));
                    if (p549) printSlip('exp-549', r.id);
                    if (p548) printSlip('exp-548-bank', r.id);
                    formReset();
                }
            });
        });
    }

    // ------------------------------------------------------------------------------ prints

    function crystal(key, args, btn) {
        if (!global.CrystalPrint) { box('Print is not available.'); return; }
        return global.CrystalPrint.open(key, args, btn);
    }
    /** CommonServices.ExportPreCommercialInvoiceSlip549 / 548: "No Record Found For Display" when the id is 0. */
    function printSlip(key, id, btn) {
        if (netI(id) === 0) { box('No Record Found For Display'); return; }
        return crystal(key, { id: netI(id) }, btn);
    }
    function btnPrint(btn) { return printSlip('exp-549', S.recId, btn); }
    function btnPrint548(btn) { return printSlip('exp-548-bank', S.recId, btn); }
    /** CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 211). */
    function voucher103(id, btn) {
        if (netI(id) === 0) { box('No Record Found For Display'); return; }
        return crystal('acc-103', { id: netI(id), documentTypeId: DOC_TYPE }, btn);
    }
    function btnVoucherPrint(btn) { return voucher103(S.voucherHeadId, btn); }
    /** btnSlipHistory_Click / btnBankSlipHistory_Click: ExportPdfReport.ExportPreInvoiceSlip by doc no range. */
    function rangeArgs() {
        var a = {};
        if (netI(X.val('FromDocNoHistory').trim()) > 0) a.fromDocNo = netI(X.val('FromDocNoHistory').trim());
        if (netI(X.val('ToDocNoHistory').trim()) > 0) a.toDocNo = netI(X.val('ToDocNoHistory').trim());
        return a;
    }
    function btnSlipHistory(btn) { return crystal('exp-549a', rangeArgs(), btn); }
    function btnBankSlipHistory(btn) { return crystal('exp-548a', rangeArgs(), btn); }

    // ------------------------------------------------------------------------------ read

    /** ReadById. */
    function readById(id, voucherHeadId) {
        X.show('btnsave', false); X.show('btnupdate', true);
        S.removed = [];
        S.recId = netI(id);
        return X.getJson(API + '/by-id?id=' + encodeURIComponent(id)).then(function (d) {
            var h = d.header;
            tab('tabPage1');
            X.setVal('cmbbranches', h.BranchesId);
            X.setVal('cmbproject', h.ProjectsId);
            X.setText('txtdocno', clr(h.DocCode));
            X.setText('txtDocDate', h.DocDate);
            return bindCommercialInvoiceNos(h.InvoiceNo).then(function () {
                X.setVal('cmbPrefixType', h.PrefixTypeId);
                setRawText('Cmbinvoiceno', h.InvoiceNo);
                setRawValue('CmbPreInvoice', h.PreInvoiceId);
                X.setVal('cmbSupCust', h.SupplierCustomerId);
                X.setVal('cmbCommissionAgent', h.CommissionAgentId);
                X.setVal('CmbCommissionDebitAccount', h.CommissionDebitAcId);
                setFire('txtCommPercent', h.CommissionPercentage);
                X.setText('txtCommAmount', h.CommissionAmount);
                X.setVal('cmbDestinationPort', h.DestinationPortId);
                X.setVal('cmbOtherCustomer', h.OtherCustomerId);
                X.setVal('cmbOtherDestinationPort', h.OtherDestinationPortId);
                if (h.NotifyParty1 > 0) X.setVal('cmbNotifyParty1', h.NotifyParty1);
                if (h.NotifyParty2 > 0) X.setVal('cmbNotifyParty2', h.NotifyParty2);
                if (h.ImporterBankId > 0) X.setVal('cmbimporterBankNew', h.ImporterBankId);
                if (h.ExporteBankId > 0) X.setVal('cmbexporterBankNew', h.ExporteBankId);
                X.setText('txtLotRef', h.LotNoRef);
                X.setVal('cmbPaymentTermsNew', h.PaymentTermId);
                X.setVal('cmbdeliverytermnew', h.DeliveryTermId);
                X.setVal('cmbLoadingPort', h.LoadingPortId);
                X.setVal('cmbDestinationPort', h.DestinationPortId);
                X.setByText('cmbcareiertype', h.CarierType);
                X.setVal('cmbfcycode', h.FcurrencyId);
                setFire('txtfcyAmount', h.FCurrencyAmount);
                setFire('txtExRate', h.ConversionRate);
                X.setText('txtLocalAmt', h.EquivalentAmount);
                X.setText('txtGrossWeight', h.GrossWeight);
                X.setText('txtNetWeight', h.NetWeight);
                X.setText('txtNoOfContainer', h.NoOfContainers);
                X.setText('txtGdNo', h.EFormNo);
                X.setText('txtIformDate', h.EFormDate);
                X.setText('txtcertificate', h.Certificate1);
                X.setText('txtcertificateOfOrigion', h.Certificate2);
                X.setText('txtaddlesscommnets', h.AddLessComments);
                setFire('txtaddlessamount', h.AddLessAmount);
                X.setText('txtTotalNetAmount', h.TotalAmount);
                S.otherItemList = d.otherItemList || [];
                if (S.otherItemList.length) X.bind('CmbOtherItem', S.otherItemList, 'ItemId', 'ItemName');
                S.rows = []; S.cur = -1; render();
                if (!(d.rows || []).length) throw new Error('Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index');
                S.rows = d.rows;
                S.invoiceUpdateFlag = netI(d.invoiceUpdateFlag);
                var items = [];
                S.rows.forEach(function (r) { items.push({ Id: r.ItemId, Name: r.ItemName }); });
                if (items.length) X.bind('combitem', items, 'Id', 'Name');
                render();
                S.other = d.otherItems || []; renderOther();
                S.pay = d.paymentTerms || []; S.payHidden = false; renderPay();
                detailFormReset();
                S.updateMode = true;
            }).catch(function (e) { box(e.message); }).then(function () {
                bindFis(d.fis);                         /* BindFinancialInstrument after the try / catch */
                if (voucherHeadId !== undefined) S.voucherHeadId = netI(voucherHeadId);
            });
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ history

    var HCOLS = [
        { key: 'InvoiceNo', render: function (r, i) { return '<a href="#" class="win-code" data-i="' + i + '">' + esc(r.InvoiceNo) + '</a>'; } },
        { key: 'DocCode', num: true }, { key: 'DocDate' }, { key: 'CustomerName' },
        { key: 'NoOfContainers', fmt: '0,0', sum: '0,0' }, { key: 'GrossWeight', fmt: '#,#.###', sum: '#,#.###' }, { key: 'NetWeight', fmt: '#,#.###', sum: '#,#.###' },
        { key: 'FcyAmount', fmt: '#,#.###', sum: '#,#.###' }, { key: 'AddLess', fmt: '0,0', sum: '0,0' }, { key: 'TotalAmount', fmt: '#,#.###', sum: '#,#.###' },
        { key: 'LoadingPort' }, { key: 'DestinationPort' }, { key: 'EntryUser' }, { key: 'EntryDate' }, { key: 'ModifyUser' }, { key: 'ModifyDate' },
        { key: 'ApprovedStatus' }, { key: 'ApprovedUser' }, { key: 'ApprovedDate' },
        { key: 'NoOfAttachments', render: function (r) { return '<span title="Attachments are not part of this web screen" style="text-decoration:underline;">' + esc(r.NoOfAttachments) + '</span>'; } }
    ];
    function renderHistory() {
        X.drawGrid('ecitHistBody', 'ecitHistFoot', HIST, HCOLS, {
            current: HCUR, leadCount: 3,
            lead: function (r, i) {
                return '<td class="win-cell-btn"><button type="button" class="win-btn-small" data-hslip="' + i + '">549-Slip</button></td>'
                    + '<td class="win-cell-btn"><button type="button" class="win-btn-small" data-hbank="' + i + '">Bank Slip</button></td>'
                    + '<td class="win-cell-btn"><button type="button" class="win-btn-small" data-hvoucher="' + i + '">Voucher</button></td>';
            }
        });
        X.show('ecitHistEmpty', !HIST.length);
    }
    /** HistoryGridFill. */
    function btnShowHistory(btn) {
        var body = {
            fromChecked: X.checked('FromDateHistoryChk'), fromDate: X.val('FromDateHistory'), toChecked: X.checked('ToDateHistoryChk'), toDate: X.val('ToDateHistory'),
            dateMode: (document.querySelector('input[name="ecitDateBy"]:checked') || {}).value || 'doc',
            fromDocNo: netI(X.val('FromDocNoHistory')), toDocNo: netI(X.val('ToDocNoHistory')), customerId: netI(X.val('CmbCustomerHistory'))
        };
        return X.busy(btn || 'btnShowHistory', function () {
            return X.postJson(API + '/history', body).then(function (rows) {
                HIST = rows || []; HCUR = -1; renderHistory();
                if (!HIST.length) { $id('ecitHistDetailBody').innerHTML = ''; $id('ecitHistDetailFoot').innerHTML = ''; }
            });
        });
    }
    var DCOLS = [
        { key: 'ItemName' }, { key: 'HSCode' }, { key: 'PackingType' }, { key: 'PackingWeight', fmt: '#,##0.###', sum: '#,##0.###' }, { key: 'CropYear' },
        { key: 'NoOfContainers', fmt: '#,##0.###', sum: '#,##0.###' }, { key: 'NoOfBagsCntnr', fmt: '#,##0.###', sum: '#,##0.###' }, { key: 'PackUom' },
        { key: 'MTon', fmt: '#,##0.###', sum: '#,##0.###' }, { key: 'NoOfBags', fmt: '#,##0.###', sum: '#,##0.###' }, { key: 'NetWeight', fmt: '#,##0.###', sum: '#,##0.###' },
        { key: 'TotalPackingWeight', fmt: '#,##0.###', sum: '#,##0.###' }, { key: 'GrossWeight', fmt: '#,##0.###', sum: '#,##0.###' },
        { key: 'RateMTon', fmtKey: 'fcyRate' }, { key: 'RateUOM' }, { key: 'Amount', fmtKey: 'fcyAmount', sumKey: 'fcyAmount' }, { key: 'BankRate', fmtKey: 'fcyRate' },
        { key: 'BankAmount', fmtKey: 'fcyAmount', sumKey: 'fcyAmount' }, { key: 'BankHSCode' }, { key: 'PackingExpiryDate' }, { key: 'ItemCommodityDetail' }, { key: 'HealthPermitNoDetail' }
    ];
    /** GetDetailByHeaderId (DataGridHistory_SelectionChanged). */
    function historySelect(i) {
        HCUR = i;
        var r = HIST[i]; if (!r) return;
        X.getJson(API + '/history-detail?id=' + r.Id).then(function (rows) {
            if (HCUR !== i || !rows || !rows.length) return;
            X.drawGrid('ecitHistDetailBody', 'ecitHistDetailFoot', rows, DCOLS.map(function (c) {
                var o = {}; for (var k in c) o[k] = c[k]; if (c.fmtKey) o.fmt = FMT[c.fmtKey]; if (c.sumKey) o.sum = FMT[c.sumKey]; return o;
            }));
        }).catch(function (e) { box(e.message); });
    }
    /** btnResetHistory_Click. */
    function btnResetHistory() {
        X.setText('FromDateHistory', X.addDays(X.today(), -3));
        X.setText('ToDateHistory', X.today());
        X.setText('FromDocNoHistory', ''); X.setText('ToDocNoHistory', '');
        X.setVal('CmbCustomerHistory', '');
        HIST = []; HCUR = -1;
        $id('ecitHistBody').innerHTML = ''; $id('ecitHistFoot').innerHTML = '';
        $id('ecitHistDetailBody').innerHTML = ''; $id('ecitHistDetailFoot').innerHTML = '';
        X.show('ecitHistEmpty', false);
    }
    function btnRefreshHistory(btn) {
        return X.busy(btn || 'btnRefreshHistory', function () { return X.getJson(API + '/history-combos').then(function (rows) { X.bind('CmbCustomerHistory', rows, 'Id', 'Customer'); }); });
    }

    // ------------------------------------------------------------------------------ loader popup (LoadForwardingForCommercialInvoice)

    var LCOLS = [
        { key: 'CustomerName' }, { key: 'ProformaNo' }, { key: 'CommissionAgent' }, { key: 'CommRate', num: true }, { key: 'CurrencyCode' }, { key: 'ItemName' },
        { key: 'PackingType' }, { key: 'CropYear' }, { key: 'PackUom' }, { key: 'EbUnit', num: true }, { key: 'EbTotal', num: true }, { key: 'GrossWeight', num: true },
        { key: 'M_Ton', fmt: '#,##0', sum: '#,##0' }, { key: 'ShipMton', fmt: '#,##0', sum: '#,##0' }, { key: 'BalMTon', fmt: '#,##0', sum: '#,##0' },
        { key: 'NoOfBags', fmt: '#,##0', sum: '#,##0' }, { key: 'ShipBags', fmt: '#,##0', sum: '#,##0' }, { key: 'BalNoofBags', fmt: '#,##0', sum: '#,##0' },
        { key: 'NetWeight', fmt: '#,##0.###', sum: '#,##0.###' }, { key: 'ShipWeight', fmt: '#,##0.###', sum: '#,##0.###' }, { key: 'BalWeight', fmt: '#,##0.###', sum: '#,##0.###' },
        { key: 'RatePrice', num: true }, { key: 'RateUom' }, { key: 'Amount', fmtKey: 'fcyAmount', sumKey: 'fcyAmount' }, { key: 'ShipAmount', fmtKey: 'fcyAmount', sumKey: 'fcyAmount' },
        { key: 'BalShipAmount', fmtKey: 'fcyAmount', sumKey: 'fcyAmount' }, { key: 'OtherRate', num: true }, { key: 'PackingExpiryDate' }, { key: 'ProductionNo' }
    ];
    function renderLoader() {
        X.drawGrid('ldBody', 'ldFoot', LD.raw, LCOLS.map(function (c) {
            var o = {}; for (var k in c) o[k] = c[k]; if (c.fmtKey) o.fmt = FMT[c.fmtKey]; if (c.sumKey) o.sum = FMT[c.sumKey]; return o;
        }), { leadCount: 1, lead: function (r, i) { return '<td class="win-cell-chk"><input type="checkbox" data-ldchk="' + i + '"/></td>'; } });
        $id('ldSelectAll').checked = false;
    }
    /** ExportPreInvoicesLoad. */
    function loaderSearch(btn) {
        var f = {
            customerId: netI(X.val('ldCmbPartyName')), itemId: netI(X.val('ldcmbItemName')), fcyId: netI(X.val('ldcmbCurrency')), skipZero: X.checked('ldchkSkipZero'),
            packingExpiryDate: X.val('ldCmbPackingExpiryDate'), productionNo: X.val('ldCmbProductionNo'), proformaId: netI(X.val('ldCmbProformaNo'))
        };
        return X.busy(btn || 'ldbtngrnlod', function () { return X.postJson(API + '/loader/search', f).then(function (rows) { LD.raw = rows || []; renderLoader(); }); });
    }
    /** btnLoaderForm_Click: a new LoadForwardingForCommercialInvoice every time (LoadInvoices_Load). */
    function btnLoaderForm() {
        LD.raw = []; renderLoader();
        X.show('ldchkSkipZeroWrap', false); X.setChecked('ldchkSkipZero', true);
        X.setText('ldFromDate', X.today()); X.setText('ldTodate', X.today());
        X.show('ecitLoader', true); LD.open = true;
        return X.busy('btnLoaderForm', function () {
            return X.getJson(API + '/loader/setup').then(function (d) {
                d = d || {};
                X.bind('ldCmbProformaNo', d.proformas, 'Id', 'Name');
                X.bind('ldCmbPartyName', d.parties, 'Id', 'Name');
                X.bind('ldcmbItemName', d.items, 'Id', 'Name');
                X.bind('ldcmbCurrency', d.currencies, 'Id', 'CurrencyName');
                X.bind('ldCmbPackingExpiryDate', d.expiryDates, 'Id', 'ExpiryDate');
                X.bind('ldCmbProductionNo', d.productionNos, 'Id', 'ProductionNo');
                X.focus('ldFromDate');
                return loaderSearch($id('ldbtngrnlod'));
            });
        });
    }
    /** btnReset_Click: hides Skip Zero and clears the party only. */
    function loaderReset() { X.show('ldchkSkipZeroWrap', false); X.focus('ldFromDate'); X.setVal('ldCmbPartyName', ''); }
    /** btnLoadOnInvoice_Click_1. */
    function loaderLoad() {
        var picked = [];
        document.querySelectorAll('#ldBody input[data-ldchk]:checked').forEach(function (c) { picked.push(LD.raw[+c.getAttribute('data-ldchk')]); });
        if (!picked.length) { box('Check the row first'); return; }
        var cust = 0, cur = 0;
        for (var i = 0; i < picked.length; i++) {
            var c = netI(picked[i].SupplierCustomerId), f = netI(picked[i].FCurrencyId);
            if (cust === 0) { cust = c; cur = f; }
            if (cust !== c || cur !== f) { LD.supply = []; box("Sorry! The selected PreInvoice's are not of the same Customer/Importer or Currency."); return; }
        }
        closeLoader(picked);
    }
    /** The popup closes (Load hides it; the window X leaves dtSupply as it was) -> LoadInGridDetail + cmbSupCust_Leave_1. */
    function closeLoader(rows) {
        X.show('ecitLoader', false); LD.open = false;
        loadInGridDetail(rows || LD.supply || []);
        LD.supply = [];
        supCustLeave();
    }
    function loaderClose() { closeLoader(null); }

    /** LoadInGridDetail. */
    function loadInGridDetail(dt) {
        try {
            if (dt.length) {
                var d0 = dt[0];
                if (X.val('cmbSupCust') !== '' && S.rows.length && netI(X.val('cmbSupCust')) !== netI(d0.SupplierCustomerId)) throw new Error("You Can't Load Data Of Different Customer");
                if (X.val('cmbfcycode') !== '' && S.rows.length && netI(X.val('cmbfcycode')) !== netI(d0.FCurrencyId)) throw new Error("You Can't Load Data Of Different Currency");
                X.setVal('cmbSupCust', netI(d0.SupplierCustomerId));
                X.setVal('cmbfcycode', netI(d0.FCurrencyId));
                X.setVal('cmbCommissionAgent', netI(d0.CommissionAgentId));
                setFire('txtCommPercent', fmt(d0.CommRate, FMT.fcyRate));
                dt.forEach(function (r) {
                    var dup = S.rows.some(function (g) {
                        return netI(g.ItemId) === netI(r.ItemId) && netI(g.PackTypeId) === netI(r.PackingMaterialId) && netI(g.CropYearId) === netI(r.CropYearId)
                            && netI(g.PackSizeId) === netI(r.PackUomId) && netI(g.RateUOMId) === netI(r.RateUomId) && Math.round(netD(g.CostMTon)) === Math.round(netD(r.RatePrice))
                            && Math.round(netD(g.OtherRate)) === Math.round(netD(r.OtherRate)) && str(g.PackingExpiryDate) === str(r.PackingExpiryDate)
                            && str(g.ProductionNo) === str(r.ProductionNo);
                    });
                    if (dup) return;
                    S.rows.push({
                        Id: 0, PerformaId: netI(r.ContractId), PerformaDetailId: netI(r.ContractDetailId), PerformaDate: X.isoDate(r.ProformaDate), PriRefNo: str(r.ProformaNo),
                        ItemId: netI(r.ItemId), ItemName: str(r.ItemName), HSCode: '', PackTypeId: netI(r.PackingMaterialId), PackType: str(r.PackingType),
                        PackingWeight: netD(r.EbUnit), CropYearId: netI(r.CropYearId), CropYear: str(r.CropYear), NoOfContainers: 0, NoOfBagsCntnr: 0,
                        PackSizeId: netI(r.PackUomId), PackSize: str(r.PackUom), QtyMTon: netD(r.BalMTon), NoOfBags: netD(r.BalNoofBags), ValidateBags: netD(r.BalNoofBags),
                        NetWeight: netD(r.BalWeight), ValidateNetWeight: netD(r.BalWeight), TotalPackingWeight: netD(r.EbTotal), GrossWeight: netD(r.GrossWeight),
                        RateWithoutAddLess: netD(r.RatePrice), AddLessRate: 0, CostMTon: netD(r.RatePrice), ContractRate: netD(r.RatePrice), RateUOMId: netI(r.RateUomId),
                        RateUOM: str(r.RateUom), Amount: netD(r.BalShipAmount), OtherRate: netD(r.OtherRate), OtherAmount: 0, OtherHSCode: '0',
                        PackingExpiryDate: str(r.PackingExpiryDate), ProductionNo: str(r.ProductionNo), ItemCommodityDetail: '', HealthPermitNoDetail: ''
                    });
                });
                render();
            }
            S.other = []; renderOther();
            calculatAmount();
            calculateLocalAmount();
            calculateWeight();
            totalAmountCalculate();
            getTotalnetWeightFromGrid();
        } catch (e) { box('System.Exception: ' + e.message); }
    }

    // ------------------------------------------------------------------------------ wiring

    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.win-tabs[data-tabs="ecit"] .win-tab').forEach(function (b) { b.addEventListener('click', function () { tab(b.getAttribute('data-tab')); }); });
        document.querySelectorAll('[data-subtabs="ecit"] .win-tab').forEach(function (b) { b.addEventListener('click', function () { subtab(b.getAttribute('data-subtab')); }); });
        document.querySelectorAll('button[data-open]').forEach(function (b) { b.addEventListener('click', function () { global.open(b.getAttribute('data-open'), '_blank'); }); });
        X.wireFullscreen();
        /* TextChanged / ValueChanged of boxes the user types in */
        ['txtPackingWeight', 'txtNoOfContainersDetail', 'txtNoOfBagsPerCntnr', 'txtOtherRateDetail', 'txtAddLessRate', 'txtCommPercent', 'txtExRate', 'txtaddlessamount', 'txtoQty']
            .forEach(function (id) { X.on(id, 'input', function () { HANDLERS[id](); }); });
        ['CmbOtherItem', 'CmbFinInstruments'].forEach(function (id) { X.on(id, 'change', function () { HANDLERS[id](); }); });
        /* Leave events */
        X.on('combitem', 'change', function () { bindRateUom(X.val('combitem')); });
        X.on('CmbPreInvoice', 'change', function () { clearRaw('CmbPreInvoice'); supCustLeave(); });
        X.on('cmbPrefixType', 'change', prefixChanged);
        X.on('Cmbinvoiceno', 'change', function () { if (X.val('Cmbinvoiceno') !== '__raw') clearRaw('Cmbinvoiceno'); });
        X.on('dtxtpercentoftotal', 'blur', paymentPercentCalculate);
        X.on('txtBalanceFI', 'blur', paymentPercentCalculate);
        X.on('dtxtfcyamount', 'blur', fcyAmountLeave);
        var gb = $id('ecitBody');
        gb.addEventListener('click', function (e) {
            var d = e.target.closest('button[data-del]'); if (d) { deleteRow(+d.getAttribute('data-del')); return; }
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            S.cur = +tr.getAttribute('data-i');
            gb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
        });
        gb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('button')) rowToEntry(+tr.getAttribute('data-i')); });
        $id('ecitOtherBody').addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) otherToEntry(+tr.getAttribute('data-i')); });
        var pb = $id('ecitPayBody');
        pb.addEventListener('click', function (e) { var d = e.target.closest('button[data-pdel]'); if (d) deletePay(+d.getAttribute('data-pdel')); });
        pb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('button')) payToEntry(+tr.getAttribute('data-i')); });
        var hb = $id('ecitHistBody');
        hb.addEventListener('click', function (e) {
            var s = e.target.closest('button[data-hslip]'); if (s) { printSlip('exp-549', HIST[+s.getAttribute('data-hslip')].Id, s); return; }
            var bk = e.target.closest('button[data-hbank]'); if (bk) { printSlip('exp-548-bank', HIST[+bk.getAttribute('data-hbank')].Id, bk); return; }
            var v = e.target.closest('button[data-hvoucher]'); if (v) { voucher103(HIST[+v.getAttribute('data-hvoucher')].VoucherHeadId, v); return; }
            var a = e.target.closest('a.win-code'); if (a) { e.preventDefault(); var r = HIST[+a.getAttribute('data-i')]; readById(r.Id, r.VoucherHeadId); return; }
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            hb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
            historySelect(+tr.getAttribute('data-i'));
        });
        hb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (!tr || e.target.closest('button')) return; var r = HIST[+tr.getAttribute('data-i')]; readById(r.Id, r.VoucherHeadId); });
        $id('ldSelectAll').addEventListener('change', function () { var on = this.checked; document.querySelectorAll('#ldBody input[data-ldchk]').forEach(function (c) { c.checked = on; }); });
        /* ImProformaInvoice_KeyDown (+ LoadForwardingForCommercialInvoice_KeyDown while the popup is open) */
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (LD.open) {
                if (e.ctrlKey && e.altKey && k === 'l') { e.preventDefault(); X.show('ldchkSkipZeroWrap', true); return; }
                if (e.key === 'Escape') { e.preventDefault(); loaderClose(); return; }
                X.enterAsTab(e);
                return;
            }
            if (X.enterAsTab(e)) return;
            var t = tabIndex();
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); X.closeForm(); return; }
            if (e.ctrlKey && k === 'n' && t === 0) { e.preventDefault(); btnNew(); return; }
            if (e.ctrlKey && k === 'r' && t === 0) { e.preventDefault(); btnrefersh($id('btnrefersh')); return; }
            if (e.ctrlKey && k === 's' && t === 0 && !S.updateMode) { e.preventDefault(); btnsave($id('btnsave')); return; }
            if (e.ctrlKey && k === 'u' && t === 0 && S.updateMode) { e.preventDefault(); btnsave($id('btnupdate')); return; }
            if (e.ctrlKey && k === 't') { e.preventDefault(); tab(TABS[(t + 1) % TABS.length]); }
        });
        load();
    });

    global.ExportEcit = {
        btnNew: btnNew, btnsave: btnsave, btnrefersh: btnrefersh, btnLoaderForm: btnLoaderForm, btnVoucherPrint: btnVoucherPrint, btnPrint548: btnPrint548,
        btnPrint: btnPrint, toggleHistory: toggleHistory, btnAddinGrid: btnAddinGrid, btnGridUpdate: btnGridUpdate, btnCancel: btnCancel,
        btnoadd: btnoadd, btnoUpdate: btnoUpdate, btnoCancel: btnoCancel, btndadd: btndadd, btndupdate: btndupdate, btndcancel: btndcancel,
        btnShowHistory: btnShowHistory, btnResetHistory: btnResetHistory, btnRefreshHistory: btnRefreshHistory, btnSlipHistory: btnSlipHistory,
        btnBankSlipHistory: btnBankSlipHistory, loaderSearch: loaderSearch, loaderLoad: loaderLoad, loaderReset: loaderReset, loaderClose: loaderClose
    };
}(window));
