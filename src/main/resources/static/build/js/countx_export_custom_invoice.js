/* ============================================================================================
 * countx_export_custom_invoice.js - frmCustomInvoice.cs (Architecture.WinApp.Export), screen 951
 * "Custom / Bank Invoice" (DocumentTypeId 214). Every desktop event has its counterpart here with the
 * desktop's texts and order; the arithmetic of the header and the five detail tabs (CalculateWeight,
 * CalculatAmount, getTotalnetWeightFromGrid, ProportionateOtherItemAmount, ProportionateAddLessAmount,
 * TotalAmountCalculate, CalculateLocalAmount, PaymentDetailPercentCalculate, CalculateCommissionAmount,
 * RecalculateCommissionGridLocalAmount, UpdateAddLessAmountInHeaderFromChargesGrid) is reproduced verbatim.
 * Data through /api/export/custom-invoice (ExportBankGdController -> ExportCustomInvoiceService).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var X = global.ExBG, $id = X.$id, box = X.box, ask = X.ask, val = X.val, setText = X.setText, netD = X.netD, netI = X.netI, fmt = X.fmt, str = X.str, col = X.col;
    var API = '/api/export/custom-invoice';

    var PERM = { Save: true, Update: true, Print: true };
    var CFG = { flagFin: false, allowExportMultiCompanies: false, saleCommissionTabFeatureWise: false, defaultDays: 0 };
    var C = {};                                    /* combo tables from /setup */
    var CI_INV = [], UOMS = [], HS = [], FIS = [], CONSIGNEES = [];
    var DT = [], REMOVED = [], OTHER = [], PAY = [], COMM = [], CHARGES = [];
    var RecId = 0, UpdateMode = false, ExportVoucherId = 0;
    var uIdx = { detail: -1, other: -1, pay: -1, comm: -1, charge: -1 };
    var CUR = { detail: -1, other: -1, pay: -1, comm: -1, charge: -1, hist: -1 };
    var HIST = [], HIST_DET = [], commRateUomTag = '', commAddEnabled = true;
    var CARRIER = [{ Id: 1, Type: 'By Sea' }, { Id: 2, Type: 'By Air' }, { Id: 3, Type: 'By Road' }];
    var COMM_TYPES = [{ Id: 1, CommissionType: 'Fixed Commission Amount' }, { Id: 2, CommissionType: 'Commission % of Value' }, { Id: 3, CommissionType: 'Commission By Weight Kg' }];

    // ------------------------------------------------------------------ load / combos
    function load() {
        return X.getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false, Print: d.permissions.Print !== false };
            $id('btnsave').disabled = !PERM.Save; $id('btnupdate').disabled = !PERM.Update; $id('btnPrint').disabled = !PERM.Print;
            CFG.flagFin = !!d.flagFin; CFG.allowExportMultiCompanies = !!d.allowExportMultiCompanies; CFG.saleCommissionTabFeatureWise = !!d.saleCommissionTabFeatureWise;
            CFG.defaultDays = netI(d.defaultDaysToLessFromHistoryFromDate);
            Object.keys(d).forEach(function (k) { if (/Error$/.test(k) && d[k]) box(d[k]); });
            X.show('rowExportCompany', CFG.allowExportMultiCompanies);
            X.show('tabCommission', CFG.saleCommissionTabFeatureWise);
            bindAll(d);
            if (d.historyCombos) bindHistoryCombos(d.historyCombos);
            X.bind('cmbcareiertype', CARRIER, 'Id', 'Type', []); X.setVal('cmbcareiertype', 1);
            X.bind('cmbCommissionType', COMM_TYPES, 'Id', 'CommissionType', []); X.setVal('cmbCommissionType', 3);
            setText('FromDateHistory', X.daysAgo(CFG.defaultDays > 0 ? CFG.defaultDays : 3)); setText('ToDateHistory', X.today());
            setText('txtDocDate', X.today()); setText('txtIformDate', X.today());
            renderAll();
            $id('ciFooterInfo').textContent = 'frmCustomInvoice  -  Document Type 214';
            X.focus('txtDocDate');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    /* BindData: every combo keeps its current value when the row is still in the new list (BindDDLNew / SetComboValue). */
    function bindAll(d) {
        C = d;
        X.bind('cmbbranches', d.branches || [], 'Id', 'Name', []); if ((d.branches || []).length) X.setVal('cmbbranches', d.branches[0].Id);
        X.bind('cmbproject', d.projects || [], 'Id', 'Name', []); if ((d.projects || []).length) X.setVal('cmbproject', d.projects[0].Id);
        if (netI(d.docNo) > 0) setText('txtdocno', d.docNo);
        setText('txtLastInvoiceNumber', str(d.lastInvoiceNumber));
        CI_INV = d.commercialInvoices || []; bindCiInvoices(); ciInvoiceLeave();
        X.bind('cmbSupCust', d.customers || [], 'Id', 'Name', []);
        X.bind('cmbNotifyParty1', d.customers || [], 'Id', 'Name', []);
        X.bind('cmbNotifyParty2', d.customers || [], 'Id', 'Name', []);
        X.bind('CmbFarmingNTrade', d.farmingNTrade || [], 'Id', 'FarmingNTrade', []);
        X.bind('CmbExportCompany', d.exportCompanies || [], 'Id', 'Name', []);
        X.bind('cmbdeliverytermnew', d.deliveryTerms || [], 'Id', 'Code', ['Description']);
        X.bind('cmbPaymentTermsNew', d.paymentTerms || [], 'Id', 'lcOrderTerm', ['Description']);
        X.bind('dcmbpaymentterm', d.paymentTerms || [], 'Id', 'lcOrderTerm', ['Description']);
        X.bind('cmbLoadingPort', (d.ports || []).filter(function (p) { return p.PortType === 'Loading'; }), 'Id', 'PortName', []);
        X.bind('cmbDestinationPort', (d.ports || []).filter(function (p) { return p.PortType === 'Destination'; }), 'Id', 'PortName', []);
        var home = (d.banks || []).filter(function (b) { return b.IsHomeland === 'Home Country'; }), foreign = (d.banks || []).filter(function (b) { return b.IsHomeland === 'Foreign Country'; });
        X.bind('cmbexporterBankNew', home, 'Id', 'BranchName', []); X.bind('cmbimporterBankNew', foreign, 'Id', 'BranchName', []);
        if (home.length && !X.hasSel('cmbexporterBankNew')) X.setVal('cmbexporterBankNew', home[0].Id);   /* Rows[1].Activate() = the first data row */
        X.bind('CmbCreditAccount', d.creditAccounts || [], 'Id', 'Name', []);
        X.bind('combitem', d.items || [], 'Id', 'ItemName', ['ItemCode', 'ItemCategory', 'ItemType']);
        FIS = d.fis || [];
        X.bind('CmbOtherItem', d.otherItems || [], 'Id', 'Name', []);
        X.bind('cmbfcycode', d.currencies || [], 'Id', 'Name', []);
        X.bind('CmbCropYear', d.cropYears || [], 'Id', 'Name', []);
        X.bind('CmbJobLot', d.jobLots || [], 'Id', 'Name', []);
        X.bind('combpcktype', d.packTypes || [], 'Id', 'Name', []);
        X.bind('CmbChargesNameChargeDetail', d.exportCharges || [], 'Id', 'Name', []);
        if (CFG.saleCommissionTabFeatureWise) X.bind('CmbSalePerson', d.salesPersons || [], 'Id', 'Name', []);
    }
    function bindCiInvoices() { X.bind('CmbInvoiceNoCI', CI_INV, 'ExImInvoiceId', 'InvoiceNo', ['InvoiceDate', 'ContractNo', 'CreditAccount', 'CustomerName', 'DestinationPort', 'BalWeight', 'BalAmount']); }
    function ciRow() { return X.findRow(CI_INV, val('CmbInvoiceNoCI'), 'ExImInvoiceId'); }
    /* CmbInvoiceNoCI_Leave: the master invoice's weights and its credit account. */
    function ciInvoiceLeave() {
        setText('txtMasterInvoiceNetWeight', '0'); setText('txtMasterInvoiceDispatchWeight', '0'); setText('txtMasterInvoiceBalWeight', '0');
        var r = ciRow(), creditAccountId = 0;
        if (r && netI(val('CmbInvoiceNoCI')) > 0) {
            creditAccountId = netI(col(r, 'CreditAccountId'));
            setText('txtMasterInvoiceNetWeight', str(col(r, 'InvoiceWeight'))); setText('txtMasterInvoiceDispatchWeight', str(col(r, 'UsedWeight'))); setText('txtMasterInvoiceBalWeight', str(col(r, 'BalWeight')));
        }
        X.setVal('CmbCreditAccount', creditAccountId);
    }
    function bindHistoryCombos(hc) { X.bind('CmbCustomerHistory', hc.customers || [], 'Id', 'Name', []); X.bind('CmbInvoiceNoHistory', hc.invoices || [], 'Id', 'Name', []); }
    /* cmbSupCust_Leave: consignees of the customer + HS codes. */
    function customerLeave() {
        var cust = netI(val('cmbSupCust'));
        var p = cust > 0 ? X.getJson(API + '/consignees?customerId=' + cust) : Promise.resolve([]);
        return p.then(function (rows) { CONSIGNEES = rows || []; X.bind('cmbConsignee', CONSIGNEES, 'Id', 'Name', []); }).catch(function (e) { box(e.message); }).then(hsCodeBind);
    }
    /* HSCodeBind: CommodityDetailItemCustomerWise.GetRemarks(item, customer); first row active. */
    function hsCodeBind() {
        return X.getJson(API + '/hs-codes?itemId=' + netI(val('combitem')) + '&customerId=' + netI(val('cmbSupCust'))).then(function (rows) {
            HS = rows || [];
            var keep = val('CmbHSCode');
            X.bind('CmbHSCode', HS, 'Id', 'HSCode', []);
            if (HS.length) { if (!(netI(keep) > 0 && X.findRow(HS, keep))) X.setVal('CmbHSCode', HS[0].Id); }
        }).catch(function (e) { box(e.message); });
    }
    /* combitem_Leave: bindRateUomAndItemPackUom + HSCodeBind (the previous UOM texts are kept when still in the list). */
    function itemLeave() {
        var packText = X.selText('CmbPackUom'), rateText = X.selText('CmbRateUom');
        return X.getJson(API + '/uoms?itemId=' + netI(val('combitem'))).then(function (rows) {
            UOMS = rows || [];
            X.bind('CmbPackUom', UOMS, 'Id', 'UOMCode', ['Equivalent'], '0'); X.bind('CmbRateUom', UOMS, 'Id', 'UOMCode', ['Equivalent'], '0');
            var p = UOMS.filter(function (u) { return u.UOMCode === packText; })[0], r = UOMS.filter(function (u) { return u.UOMCode === rateText; })[0];
            if (packText && p) X.setVal('CmbPackUom', p.Id);
            if (rateText && r) X.setVal('CmbRateUom', r.Id);
        }).catch(function (e) { box(e.message); }).then(hsCodeBind);
    }
    /* BindFinancialInstrument (dcmbpaymentterm_Leave with term 1). */
    function bindFi() { X.bind('CmbFinInstruments', FIS, 'Id', 'FINo', ['DocumentTypeId', 'FIBalance']); fiChanged(); }
    /* dcmbfino_ValueChanged: a FI forces Payment Term 1 and shows its balance. */
    function fiChanged() {
        var f = X.findRow(FIS, val('CmbFinInstruments'));
        if (f && netI(val('CmbFinInstruments')) > 0) { X.setVal('dcmbpaymentterm', 1); setText('txtBalanceFI', str(f.FIBalance)); }
        else setText('txtBalanceFI', '0');
    }
    function paymentTermDetailLeave() {
        if (netI(val('dcmbpaymentterm')) === 1) bindFi();
        else { X.bind('CmbFinInstruments', [], 'Id', 'FINo', []); }
    }
    /* CmbPaymentTermDetail / CmbDeliveryTerm / CmbPaymentTerms TextChanged: the description into the remarks box. */
    function descInto(comboId, tableKey, textId) {
        var r = X.findRow(C[tableKey] || [], val(comboId)); if (!r) return;
        var d = str(r.Description); if (d) setText(textId, d);
    }
    function refreshFis() { return X.getJson(API + '/fis?recId=' + RecId).then(function (rows) { FIS = rows || []; }).catch(function (e) { box(e.message); }); }

    // ------------------------------------------------------------------ header arithmetic
    function calculateLocalAmount() {
        var f = netD(val('txtTotalNetAmount')), ex = netD(val('txtExRate'));
        setText('txtLocalAmt', f > 0 && ex > 0 ? fmt(f * ex) : '0');
    }
    /* TotalAmountCalculate: Total = Fcy + AddLess + other items; payment rows re-proportioned by their %. */
    function totalAmountCalculate() {
        var other = X.sum(OTHER, 'Amount'), addLess = netD(val('txtaddlessamount')), fcy = netD(val('txtfcyAmount'));
        if (fcy > 0) setText('txtTotalNetAmount', X.fmtHash(fcy + addLess + other, 3));
        if (netD(val('txtTotalNetAmount')) > 0 && PAY.length > 0) {
            var total = netD(val('txtTotalNetAmount'));
            PAY.forEach(function (r) { r.FcyAmount = netD(r.PrcntOfTotal) * total / 100; });
            payRender();
        }
        calculateLocalAmount();
    }
    /* ProportionateAddLessAmount: AddLess spread over the detail rows by weight. */
    function proportionateAddLess() {
        var netW = X.sum(DT, 'QtyMTon'), addLess = netD(val('txtaddlessamount'));
        DT.forEach(function (r) { r.AddLessAmount = addLess !== 0 ? addLess / netW * netD(r.QtyMTon) : 0; });
        detailRender();
    }
    /* ProportionateOtherItemAmount: other items total spread by amount. */
    function proportionateOtherItems() {
        var contractAmount = X.sum(DT, 'Amount'), otherAmount = X.sum(OTHER, 'Amount');
        DT.forEach(function (r) { r.OtherItemAmount = otherAmount > 0 ? otherAmount / contractAmount * netD(r.Amount) : 0; });
        detailRender();
    }
    /* getTotalnetWeightFromGrid: Net MTon's + Fcy-Amount from the detail grid, then the payment balance. */
    function totalsFromGrid() {
        setText('txtNetWeight', fmt(X.sum(DT, 'QtyMTon'), 5));
        setText('txtfcyAmount', fmt(X.sum(DT, 'Amount')));
        onFcyAmount();
        payBalance();
    }
    /* txtaddlessamount_TextChanged / txtfcyAmount_TextChanged */
    function onAddLess() { totalAmountCalculate(); proportionateAddLess(); }
    function onFcyAmount() { onAddLess(); }
    /* txtExRate_TextChanged */
    function onExRate() { setText('txtCommissionExchangeRate', val('txtExRate')); calculateLocalAmount(); recalcCommissionLcy(); }
    /* PaymentGridBalanceAmountFromInvoiceAmount */
    function payBalance() { setText('txtBalanceInvoiceAmtPaymentGrid', fmt(netD(val('txtTotalNetAmount')) - X.sum(PAY, 'FcyAmount'))); }

    // ------------------------------------------------------------------ Invoice Detail
    function calculateWeight() {
        setText('txtNoOfBags', '0');
        var u = X.findRow(UOMS, val('CmbPackUom')); if (!u) return;
        var mton = netD(val('txtQtyMTon')), outer = netD(u.Equivalent);
        if (mton > 0 && outer > 0) setText('txtNoOfBags', String(Math.round(mton * 1000 / outer)));
    }
    function calculatAmount() {
        setText('txtAmount', '0');
        var u = X.findRow(UOMS, val('CmbRateUom')); if (!u) return;
        var rate = netD(val('txtCostMTon')), rateUom = netD(u.Equivalent), qty = netD(val('txtQtyMTon'));
        if (rate > 0 && rateUom > 0 && qty > 0) setText('txtAmount', fmt(rate / rateUom * (qty * 1000)));
    }
    function onQtyRate() { calculateWeight(); calculatAmount(); }
    var DET_COLS = [
        { key: '_x', html: function (v, r, i) { return '<button type="button" class="win-x" data-del="' + i + '" title="Delete">X</button>'; }, cls: 'win-cell-btn' },
        { key: '_a', html: function (v, r, i) { return '<button type="button" class="win-x" style="color:#060" data-act="dup" data-i="' + i + '" title="Add">+</button>'; }, cls: 'win-cell-btn' },
        { key: 'ItemName', link: true }, 'PackType', 'CropYear', 'JobLot', { key: 'QtyMTon', num: true, sum: true, fmt: fmt }, 'PackSize', { key: 'NoOfBags', num: true, sum: true, fmt: fmt },
        { key: 'CostMTon', num: true, fmt: fmt }, 'RateUOM', { key: 'Amount', num: true, sum: true, fmt: fmt }, 'HSCode', 'ItemDetail', 'PackingDetail',
        { key: 'OtherItemAmount', num: true, sum: true, fmt: fmt }, { key: 'AddLessAmount', num: true, sum: true, fmt: X.fmt4 }, 'FarmingNTrade'
    ];
    var DET_HIST_COLS = DET_COLS.slice(2);
    function detailRender() { X.drawGrid('ciDetBody', 'ciDetFoot', DT, DET_COLS, { cur: CUR.detail }); }
    function detailValidation() {
        if (netI(val('combitem')) === 0) { box('Item Name field required'); X.focus('combitem'); return false; }
        if (!X.hasSel('combpcktype')) { box('Packing Type field required'); X.focus('combpcktype'); return false; }
        if (!X.hasSel('CmbCropYear')) { box('Crop Year field required'); X.focus('CmbCropYear'); return false; }
        if (!X.hasSel('CmbJobLot')) { box('Job Lot field required'); X.focus('CmbJobLot'); return false; }
        if (!X.hasSel('CmbPackUom')) { box('Pack Size field required'); X.focus('CmbPackUom'); return false; }
        if (!val('txtQtyMTon').trim() || !(netD(val('txtQtyMTon')) > 0)) { box('Qty/M.Ton field required'); X.focus('txtQtyMTon'); return false; }
        if (!val('txtNoOfBags').trim() || !(netD(val('txtNoOfBags')) > 0)) { box('No of Bags field required'); X.focus('txtNoOfBags'); return false; }
        if (!val('txtCostMTon').trim() || !(netD(val('txtCostMTon')) > 0)) { box('Cost/M.Ton field required'); X.focus('txtCostMTon'); return false; }
        if (!X.hasSel('CmbRateUom')) { box('Rate UOM field required'); X.focus('CmbRateUom'); return false; }
        if (!val('txtAmount').trim() || !(netD(val('txtAmount')) > 0)) { box('Amount field required'); X.focus('txtAmount'); return false; }
        return true;
    }
    function detailFromFields(base) {
        var r = base || { Id: 0, OtherItemAmount: 0, AddLessAmount: 0 };
        var f = X.findRow(C.farmingNTrade || [], val('CmbFarmingNTrade')), ru = X.findRow(UOMS, val('CmbRateUom'));
        r.ItemId = netI(val('combitem')); r.ItemName = X.selText('combitem');
        r.PackTypeId = netI(val('combpcktype')); r.PackType = X.selText('combpcktype');
        r.CropYearId = netI(val('CmbCropYear')); r.CropYear = X.selText('CmbCropYear');
        r.JobLotId = netI(val('CmbJobLot')); r.JobLot = X.selText('CmbJobLot');
        r.QtyMTon = netD(val('txtQtyMTon')); r.PackSizeId = netI(val('CmbPackUom')); r.PackSize = X.selText('CmbPackUom');
        r.NoOfBags = netD(val('txtNoOfBags')); r.CostMTon = netD(val('txtCostMTon'));
        r.RateUOMId = netI(val('CmbRateUom')); r.RateUOM = X.selText('CmbRateUom'); r.RateEquivalent = ru ? netD(ru.Equivalent) : 0;
        r.Amount = netD(val('txtAmount')); r.HSCode = netI(val('CmbHSCode')) > 0 ? X.selText('CmbHSCode') : '';
        r.ItemDetail = val('txtitemdetail'); r.PackingDetail = val('txtpackingdetail');
        r.ExImFarmingNTradeId = f ? netI(f.Id) : 0; r.FarmingNTrade = f ? str(f.FarmingNTrade) : ''; r.ExImFarmingTypeId = f ? netI(f.ExImFarmingTypeId) : 0; r.ExImTradeTypeId = f ? netI(f.ExImTradeTypeId) : 0;
        return r;
    }
    /* btnAddinGrid_Click */
    function detailAdd() {
        if (!detailValidation()) return;
        DT.push(detailFromFields()); CUR.detail = -1;
        totalsFromGrid(); proportionateOtherItems(); detailFormReset(); calculateLocalAmount(); X.focus('combitem');
    }
    /* btnGridUpdate_Click */
    function detailUpdate() {
        if (!detailValidation()) return;
        var r = DT[uIdx.detail]; if (!r) return;
        detailFromFields(r);
        totalsFromGrid(); proportionateOtherItems();
        X.show('btnAddinGrid', true); X.show('btnGridUpdate', false); X.show('btnCancel', false);
        detailFormReset(); calculateLocalAmount(); X.focus('combitem');
    }
    function detailCancel() { X.show('btnAddinGrid', true); X.show('btnGridUpdate', false); X.show('btnCancel', false); detailFormReset(); }
    /* grdDetail_DoubleClick */
    function detailEdit(i) {
        var item = DT[i]; if (!item) return;
        uIdx.detail = i; CUR.detail = i;
        X.setVal('combitem', item.ItemId);
        itemLeave().then(function () {
            X.setVal('combpcktype', item.PackTypeId); X.setVal('CmbPackUom', item.PackSizeId); X.setVal('CmbCropYear', item.CropYearId); X.setVal('CmbJobLot', item.JobLotId);
            setText('txtQtyMTon', str(item.QtyMTon)); X.setVal('CmbRateUom', item.RateUOMId);
            setText('txtNoOfBags', str(item.NoOfBags)); setText('txtCostMTon', str(item.CostMTon)); setText('txtAmount', str(item.Amount));
            setText('txtitemdetail', item.ItemDetail); setText('txtpackingdetail', item.PackingDetail);
            var h = HS.filter(function (x) { return x.HSCode === item.HSCode; })[0]; X.setVal('CmbHSCode', h ? h.Id : '0');
            X.setVal('CmbFarmingNTrade', item.ExImFarmingNTradeId);
            X.show('btnAddinGrid', false); X.show('btnGridUpdate', true); X.show('btnCancel', true);
            X.focus('combitem'); detailRender();
        });
    }
    /* grdDetail_ColumnButtonClick "Delete": only while more than one row; saved rows are remembered (ActionTypeId 3). */
    function detailDelete(i) {
        try {
            var r = DT[i]; if (!r) return;
            if (uIdx.detail !== -1) throw new Error('Reset the Detail First');
            if (DT.length > 1) {
                if (netI(r.Id) > 0) { if (!ask('Are you sure to Delete?')) return; REMOVED.push(X.copy(r)); }
                DT.splice(i, 1);
            }
            CUR.detail = -1; totalsFromGrid(); proportionateOtherItems();
        } catch (e) { box(e.message); }
    }
    /* grdDetail_ColumnButtonClick "Add": a copy of the row with Id 0. */
    function detailDup(i) { var r = DT[i]; if (!r) return; var c = X.copy(r); c.Id = 0; DT.push(c); totalsFromGrid(); proportionateOtherItems(); }
    /* DetailFormReset */
    function detailFormReset() {
        uIdx.detail = -1;
        X.setVal('combitem', '0'); X.setVal('combpcktype', '0'); X.setVal('CmbJobLot', '0');
        setText('txtQtyMTon', ''); X.setVal('CmbPackUom', '0'); setText('txtNoOfBags', ''); setText('txtCostMTon', ''); X.setVal('CmbRateUom', '0'); setText('txtAmount', '');
        setText('txtitemdetail', ''); setText('txtpackingdetail', ''); X.setVal('CmbHSCode', '0'); X.setVal('CmbFarmingNTrade', '0');
        X.show('btnAddinGrid', true); X.show('btnGridUpdate', false); X.show('btnCancel', false);
        X.subTab('ciDetail', 'detail');
        return refreshFis();
    }

    // ------------------------------------------------------------------ Other Items
    var OTHER_COLS = ['ItemName', { key: 'Qty', num: true, sum: true, fmt: X.fmt2 }, { key: 'Rate', num: true, fmt: X.fmt4 }, { key: 'Amount', num: true, sum: true, fmt: X.fmt2 }, 'Remarks'];
    function otherRender() { X.drawGrid('ciOtherBody', 'ciOtherFoot', OTHER, OTHER_COLS, { cur: CUR.other }); }
    function otherCalc() { var q = netD(val('txtoQty')), r = netD(val('txtoRate')); setText('txtoAmount', q > 0 && r > 0 ? String(q * r) : '0'); }
    /* CmbOtherItem_TextChanged: in update mode a three-column source fills the rate from its third column. */
    function otherItemChanged() {
        if (!($id('btnupdate').classList.contains('is-hidden') === false && !$id('btnupdate').disabled)) return;
        var r = X.findRow(C.otherItems || [], val('CmbOtherItem'));
        if (r && netI(r.ColumnCount) === 3) setText('txtoRate', str(r.ThirdColumn));
    }
    function otherValidation() {
        if (netI(val('CmbOtherItem')) === 0) { box('Item field required'); X.focus('CmbOtherItem'); return false; }
        if (netD(val('txtoRate')) === 0) { box('Rate field required'); X.focus('txtoRate'); return false; }
        return true;
    }
    function otherAdd() {
        if (!otherValidation()) return;
        OTHER.push({ ItemId: netI(val('CmbOtherItem')), ItemName: X.selText('CmbOtherItem'), Qty: netD(val('txtoQty')), Rate: netD(val('txtoRate')), Amount: netD(val('txtoAmount')), Remarks: val('txtoRemarks').trim() });
        CUR.other = -1; otherRender(); otherReset(); totalAmountCalculate(); proportionateOtherItems();
    }
    function otherEdit(i) {
        var it = OTHER[i]; if (!it) return;
        uIdx.other = i; CUR.other = i;
        X.setVal('CmbOtherItem', it.ItemId); setText('txtoQty', str(it.Qty)); setText('txtoRate', str(it.Rate)); setText('txtoAmount', str(it.Amount)); setText('txtoRemarks', it.Remarks);
        X.show('btnoadd', false); X.show('btnoUpdate', true); X.show('btnoCancel', true); X.focus('CmbOtherItem'); otherRender();
    }
    function otherUpdate() {
        if (!otherValidation()) return;
        var it = OTHER[uIdx.other]; if (!it) return;
        it.ItemId = netI(val('CmbOtherItem')); it.ItemName = X.selText('CmbOtherItem'); it.Qty = netD(val('txtoQty')); it.Rate = netD(val('txtoRate')); it.Amount = netD(val('txtoAmount')); it.Remarks = val('txtoRemarks');
        otherRender(); X.show('btnoadd', true); X.show('btnoUpdate', false); X.show('btnoCancel', false); otherReset(); X.focus('txtoQty'); totalAmountCalculate(); proportionateOtherItems();
    }
    function otherCancel() { X.show('btnoadd', true); X.show('btnoUpdate', false); X.show('btnoCancel', false); otherReset(); uIdx.other = -1; }
    function otherReset() { uIdx.other = -1; X.setVal('CmbOtherItem', '0'); setText('txtoQty', ''); setText('txtoRate', ''); setText('txtoAmount', ''); setText('txtoRemarks', ''); }

    // ------------------------------------------------------------------ Payment Detail
    var PAY_COLS = [
        { key: '_x', html: function (v, r, i) { return '<button type="button" class="win-x" data-del="' + i + '" title="Delete">X</button>'; }, cls: 'win-cell-btn' },
        'PaymentTerm', 'FinancialInstrumentNo', { key: 'PrcntOfTotal', num: true, sum: true, fmt: function (v) { return X.fmtHash(v, 2); } }, { key: 'FcyAmount', num: true, sum: true, fmt: X.fmt2 }, { key: 'DueDays', num: true, fmt: function (v) { return str(netI(v)); } }, 'Remarks'
    ];
    function payRender() { X.drawGrid('ciPayBody', 'ciPayFoot', PAY, PAY_COLS, { cur: CUR.pay }); }
    /* dtxtpercentoftotal_TextChanged -> PaymentDetailPercentCalculate */
    function payPercentCalc() {
        try {
            if (netD(val('txtTotalNetAmount')) > 0 && netD(val('dtxtpercentoftotal')) > 0) {
                if (netD(val('dtxtpercentoftotal')) > 100) { setText('dtxtpercentoftotal', '0'); throw new Error('Percent cannot be greater than 100 Please check'); }
                setText('dtxtfcyamount', X.fmtHash(netD(val('txtTotalNetAmount')) * netD(val('dtxtpercentoftotal')) / 100, 3));
            }
        } catch (e) { box(e.message); }
    }
    /* dtxtfcyamount_TextChanged */
    function payAmountChanged() {
        try {
            if (netD(val('txtTotalNetAmount')) > 0 && netD(val('dtxtfcyamount')) > 0) {
                var pct = netD(val('dtxtfcyamount')) / netD(val('txtTotalNetAmount')) * 100;
                setText('dtxtpercentoftotal', String(pct));
                if (pct > 100) { setText('dtxtpercentoftotal', '0'); throw new Error('Percent cannot be greater than 100 Please check'); }
            }
        } catch (e) { box(e.message); }
    }
    function payValidation() {
        if (netI(val('dcmbpaymentterm')) === 0) { box('Payment Term field required'); X.focus('dcmbpaymentterm'); return false; }
        if (netI(val('dcmbpaymentterm')) === 1 && !X.hasSel('CmbFinInstruments')) throw new Error('Financial Instrument field is required against Advance');
        if (netD(val('dtxtpercentoftotal')) === 0 || !val('dtxtpercentoftotal')) { box('Percent Field Required'); X.focus('dtxtpercentoftotal'); return false; }
        if (netD(val('dtxtfcyamount')) === 0 || !val('dtxtfcyamount')) { box('Fcy Amount  Field Required'); X.focus('dtxtfcyamount'); return false; }
        return true;
    }
    function fiDocType() { var f = X.findRow(FIS, val('CmbFinInstruments')); return f ? netI(f.DocumentTypeId) : 0; }
    function payAdd() {
        try {
            if (!payValidation()) return;
            var fiId = netI(val('CmbFinInstruments')), dt = fiId > 0 ? fiDocType() : 0;
            for (var i = 0; i < PAY.length; i++) if (fiId > 0 && netI(PAY[i].ExImEFormRegistrationId) === fiId && netI(PAY[i].DocumentTypeId) === dt) throw new Error('Financial Instrument No already add in Grid Please Check');
            PAY.push({ Id: 0, PaymentTermId: netI(val('dcmbpaymentterm')), PaymentTerm: X.selText('dcmbpaymentterm'), DocumentTypeId: dt, ExImEFormRegistrationId: fiId,
                FinancialInstrumentNo: fiId > 0 ? X.selText('CmbFinInstruments') : '', PrcntOfTotal: netD(val('dtxtpercentoftotal')), FcyAmount: netD(val('dtxtfcyamount')), DueDays: netD(val('dtxtdueday')), Remarks: val('txtPaymentRemarksDetail').trim() });
            CUR.pay = -1; payRender(); payReset(); X.focus('dcmbpaymentterm'); payBalance();
        } catch (e) { box(e.message); }
    }
    function payEdit(i) {
        var it = PAY[i]; if (!it) return;
        uIdx.pay = i; CUR.pay = i;
        X.setVal('dcmbpaymentterm', it.PaymentTermId); if (netI(it.PaymentTermId) === 1) bindFi();
        X.setVal('CmbFinInstruments', it.ExImEFormRegistrationId);
        setText('dtxtpercentoftotal', str(it.PrcntOfTotal)); setText('dtxtfcyamount', str(it.FcyAmount)); setText('dtxtdueday', str(it.DueDays)); setText('txtPaymentRemarksDetail', it.Remarks);
        X.show('btndadd', false); X.show('btndupdate', true); X.show('btndcancel', true); X.focus('dcmbpaymentterm'); payRender();
    }
    function payUpdate() {
        try {
            if (!payValidation()) return;
            var fiId = netI(val('CmbFinInstruments')), dt = fiId > 0 ? fiDocType() : 0;
            for (var i = 0; i < PAY.length; i++) if (fiId > 0 && uIdx.pay !== i && netI(PAY[i].ExImEFormRegistrationId) === fiId && netI(PAY[i].DocumentTypeId) === dt) throw new Error('Financial Instrument No already add in Grid Please Check');
            var it = PAY[uIdx.pay]; if (!it) return;
            it.PaymentTermId = netI(val('dcmbpaymentterm')); it.PaymentTerm = X.selText('dcmbpaymentterm'); it.DocumentTypeId = dt; it.ExImEFormRegistrationId = fiId;
            it.FinancialInstrumentNo = fiId > 0 ? X.selText('CmbFinInstruments') : ''; it.PrcntOfTotal = netD(val('dtxtpercentoftotal')); it.FcyAmount = netD(val('dtxtfcyamount')); it.DueDays = netD(val('dtxtdueday')); it.Remarks = val('txtPaymentRemarksDetail');
            payRender(); X.show('btndadd', true); X.show('btndupdate', false); X.show('btndcancel', false); payReset(); X.focus('dcmbpaymentterm'); payBalance();
        } catch (e) { box(e.message); }
    }
    function payCancel() { X.show('btndadd', true); X.show('btndupdate', false); X.show('btndcancel', false); payReset(); }
    function payDelete(i) {
        try { if (uIdx.pay !== -1) throw new Error('Reset Detail First...'); PAY.splice(i, 1); CUR.pay = -1; payRender(); payBalance(); } catch (e) { box(e.message); }
    }
    /* ResePaymentDetails */
    function payReset() { uIdx.pay = -1; X.setVal('dcmbpaymentterm', '0'); X.setVal('CmbFinInstruments', '0'); setText('dtxtpercentoftotal', '0'); setText('dtxtfcyamount', '0'); setText('dtxtdueday', '0'); }

    // ------------------------------------------------------------------ Commission Info
    var COMM_COLS = [
        { key: '_x', html: function (v, r, i) { return '<button type="button" class="win-x" data-del="' + i + '" title="Delete">X</button>'; }, cls: 'win-cell-btn' },
        'SalesPerson', 'CommissionType', { key: 'Rate', num: true, fmt: X.fmt4 }, 'RateUom', { key: 'FcyAmount', num: true, sum: true, fmt: X.fmt2 }, { key: 'ExchangeRate', num: true, fmt: X.fmt4 }, { key: 'LcyAmount', num: true, sum: true, fmt: X.fmt2 }, 'Remarks'
    ];
    function commRender() { X.drawGrid('ciCommBody', 'ciCommFoot', COMM, COMM_COLS, { cur: CUR.comm }); }
    /* CalculateCommissionAmount */
    function calcCommission() {
        try {
            var typeId = netI(val('cmbCommissionType')), CommRate = netD(val('txtCommRate')), FcyAmount = netD(val('txtfcyAmount'));
            if (typeId > 0 && CommRate > 0 && FcyAmount > 0) {
                var GridAmount = 0, GridRate = 0, NetWeightKgs = netD(val('txtNetWeight')) * 1000, CommAmt = 0;
                COMM.forEach(function (r, i) { if (uIdx.comm !== i) { GridAmount += netD(r.FcyAmount); GridRate += netD(r.Rate); } });
                if (typeId === 1) {
                    if (GridAmount + CommRate > FcyAmount) { var rem = FcyAmount - GridAmount; CommRate = rem >= 0 ? rem : 0; }
                    CommAmt = CommRate;
                } else if (typeId === 2) {
                    if (CommRate + GridRate > 99) { CommAmt = FcyAmount - GridAmount; CommRate = CommAmt * 100 / FcyAmount; setText('txtCommRate', X.fmt4(CommRate)); }
                    CommAmt = FcyAmount * CommRate / 100;
                    if (CommAmt + GridAmount > FcyAmount) { var rem2 = FcyAmount - GridAmount; CommAmt = !(rem2 > 0) ? 0 : rem2 * CommRate / 100; }
                } else if (typeId === 3) {
                    if (NetWeightKgs > 0) {
                        var UOM = netD(val('txtCommRateUom'));
                        CommAmt = NetWeightKgs / UOM * CommRate;
                        if (CommAmt + GridAmount > FcyAmount) {
                            var rem3 = FcyAmount - GridAmount;
                            if (rem3 > 0) { CommAmt = rem3; CommRate = CommAmt * 100 / FcyAmount; setText('txtCommRate', X.fmt4(CommRate)); CommAmt = NetWeightKgs / UOM * CommRate; }
                            else CommAmt = 0;
                        }
                    } else CommAmt = 0;
                }
                setText('txtCommissionAmount', X.fmt4(CommAmt));
                var ex = netD(val('txtCommissionExchangeRate'));
                setText('txtCommissionLcyAmount', X.fmt4(ex > 0 ? ex * CommAmt : 0));
            } else { setText('txtCommissionAmount', '0'); setText('txtCommissionLcyAmount', '0'); }
        } catch (e) { box(e.message); }
    }
    /* cmbCommissionType_TextChanged */
    function commTypeChanged() {
        var id = netI(val('cmbCommissionType')), u = $id('txtCommRateUom');
        if (id === 1) { u.value = 'Lump Sum'; u.disabled = true; }
        else if (id === 2) { u.value = '%'; u.disabled = true; }
        else if (id === 3) { if (commRateUomTag === '') u.value = '1000'; u.disabled = false; }
        else { u.value = ''; u.disabled = false; }
        calcCommission();
    }
    /* RecalculateCommissionGridLocalAmount */
    function recalcCommissionLcy() {
        var ex = netD(val('txtExRate'));
        COMM.forEach(function (r) { var f = netD(r.FcyAmount); if (f > 0 && ex > 0) { r.ExchangeRate = ex; r.LcyAmount = f * ex; } else { r.ExchangeRate = 0; r.LcyAmount = 0; } });
        commRender();
    }
    function commValidation() {
        if (netI(val('CmbSalePerson')) === 0) { box('Sales Person field is required'); X.focus('CmbSalePerson'); return false; }
        if (netI(val('cmbCommissionType')) === 0) { box('Commission Type field is required'); X.focus('cmbCommissionType'); return false; }
        if (netD(val('txtCommRate')) === 0) { box('Commission Rate Field is Required'); X.focus('txtCommRate'); return false; }
        if (netI(val('cmbCommissionType')) === 3 && val('txtCommRateUom') === '') { box('Commission Rate Uom Field is Required'); X.focus('txtCommRateUom'); return false; }
        if (netD(val('txtCommissionAmount')) === 0 || val('txtCommissionAmount') === '') { box('Commission Fcy Amount Field is Required'); X.focus('txtCommissionAmount'); return false; }
        return true;
    }
    function commAdd() {
        try {
            if (!commAddEnabled) return;
            if (!commValidation()) return;
            var sp = netI(val('CmbSalePerson'));
            for (var i = 0; i < COMM.length; i++) if (sp > 0 && netI(COMM[i].SalesPersonId) === sp) throw new Error('Sale person already add in Grid Please select another sale person!');
            COMM.push({ Id: 0, SalesPersonId: sp, SalesPerson: X.selText('CmbSalePerson'), CommissionTypeId: netI(val('cmbCommissionType')), CommissionType: X.selText('cmbCommissionType'),
                Rate: netD(val('txtCommRate')), RateUom: val('txtCommRateUom').trim(), FcyAmount: netD(val('txtCommissionAmount')), ExchangeRate: netD(val('txtCommissionExchangeRate')), LcyAmount: netD(val('txtCommissionLcyAmount')), Remarks: val('txtCommissionRemarks').trim() });
            CUR.comm = -1; commRender(); commReset(); X.focus('CmbSalePerson');
        } catch (e) { box(e.message); }
    }
    function commEdit(i) {
        var it = COMM[i]; if (!it) return;
        uIdx.comm = i; CUR.comm = i;
        X.setVal('CmbSalePerson', it.SalesPersonId); X.setVal('cmbCommissionType', it.CommissionTypeId); commTypeChanged();
        setText('txtCommRate', str(it.Rate)); setText('txtCommRateUom', it.RateUom); setText('txtCommissionAmount', fmt(it.FcyAmount)); setText('txtCommissionExchangeRate', fmt(it.ExchangeRate)); setText('txtCommissionLcyAmount', fmt(it.LcyAmount)); setText('txtCommissionRemarks', it.Remarks);
        X.show('btnAddCommission', false); X.show('btnUpdateCommision', true); X.show('btnCancelCommission', true); X.focus('CmbSalePerson'); commRender();
    }
    function commUpdate() {
        try {
            if (!commAddEnabled) return;
            if (!commValidation()) return;
            var sp = netI(val('CmbSalePerson'));
            for (var i = 0; i < COMM.length; i++) if (sp > 0 && uIdx.comm !== i && netI(COMM[i].SalesPersonId) === sp) throw new Error('Sales person already add in Grid Please selected another SalesPerson!');
            var it = COMM[uIdx.comm]; if (!it) return;
            it.SalesPersonId = sp; it.SalesPerson = X.selText('CmbSalePerson'); it.CommissionTypeId = netI(val('cmbCommissionType')); it.CommissionType = X.selText('cmbCommissionType');
            it.Rate = netD(val('txtCommRate')); it.RateUom = val('txtCommRateUom').trim(); it.FcyAmount = netD(val('txtCommissionAmount')); it.ExchangeRate = netD(val('txtCommissionExchangeRate')); it.LcyAmount = netD(val('txtCommissionLcyAmount')); it.Remarks = val('txtCommissionRemarks');
            commRender(); X.show('btnAddCommission', true); X.show('btnUpdateCommision', false); X.show('btnCancelCommission', false); commReset(); X.focus('CmbSalePerson');
        } catch (e) { box(e.message); }
    }
    function commCancel() { X.show('btnAddCommission', true); X.show('btnUpdateCommision', false); X.show('btnCancelCommission', false); commReset(); }
    function commDelete(i) { try { if (uIdx.comm !== -1) throw new Error('Reset Detail First...'); if (commAddEnabled) { COMM.splice(i, 1); CUR.comm = -1; commRender(); } } catch (e) { box(e.message); } }
    function commReset() { uIdx.comm = -1; X.setVal('CmbSalePerson', '0'); X.setVal('cmbCommissionType', '0'); setText('txtCommRate', '0'); setText('txtCommRateUom', '0'); commRateUomTag = ''; setText('txtCommissionAmount', '0'); setText('txtCommissionLcyAmount', '0'); }

    // ------------------------------------------------------------------ Other Charges
    var CHARGE_COLS = [
        { key: 'ChargesItem', link: true },
        { key: '_x', html: function (v, r, i) { return '<button type="button" class="win-x" data-del="' + i + '" title="Delete">X</button>'; }, cls: 'win-cell-btn' },
        { key: 'AddAmount', num: true, sum: true, fmt: fmt }, { key: 'LessAmount', num: true, sum: true, fmt: fmt }, 'Remarks'
    ];
    function chargeRender() { X.drawGrid('ciChargeBody', 'ciChargeFoot', CHARGES, CHARGE_COLS, { cur: CUR.charge }); }
    function chargeValidation() {
        if (!X.hasSel('CmbChargesNameChargeDetail')) { box('Charges Item field is required'); X.focus('CmbChargesNameChargeDetail'); return false; }
        if (netD(val('txtLessAmountChargeDetail')) === 0 && netD(val('txtAddAmountChargeDetail')) === 0) { box('Add Or Less Amount is required'); X.focus('txtLessAmountChargeDetail'); return false; }
        if (netD(val('txtLessAmountChargeDetail')) > 0 && netD(val('txtAddAmountChargeDetail')) > 0) { box("You Can Only Enter 'Add Or Less Amount'"); X.focus('txtLessAmountChargeDetail'); return false; }
        return true;
    }
    /* UpdateAddLessAmountInHeaderFromChargesGrid -> txtaddlessamount ("#,#.###") -> TotalAmountCalculate + ProportionateAddLessAmount */
    function chargesToHeader() { setText('txtaddlessamount', X.fmtHash(X.sum(CHARGES, 'AddAmount') - X.sum(CHARGES, 'LessAmount'), 3)); onAddLess(); }
    function chargeAdd() {
        if (!chargeValidation()) return;
        CHARGES.push({ Id: 0, ContractId: 0, ContractOtherChargesDetailId: 0, ChargesItemId: netI(val('CmbChargesNameChargeDetail')), ChargesItem: X.selText('CmbChargesNameChargeDetail'), AddAmount: netD(val('txtAddAmountChargeDetail')), LessAmount: netD(val('txtLessAmountChargeDetail')), Remarks: val('txtRemarksChargeDetail').trim() });
        CUR.charge = -1; chargeRender(); chargeReset(); chargesToHeader();
    }
    function chargeEdit(i) {
        var it = CHARGES[i]; if (!it) return;
        uIdx.charge = i; CUR.charge = i;
        X.setVal('CmbChargesNameChargeDetail', it.ChargesItemId); setText('txtAddAmountChargeDetail', str(it.AddAmount)); setText('txtLessAmountChargeDetail', str(it.LessAmount)); setText('txtRemarksChargeDetail', it.Remarks);
        X.show('BtnAddChargeDetail', false); X.show('BtnUpdateChargeDetail', true); X.show('BtnCancelChargeDetail', true); X.focus('CmbChargesNameChargeDetail'); chargeRender();
    }
    function chargeUpdate() {
        if (!chargeValidation()) return;
        var it = CHARGES[uIdx.charge]; if (!it) return;
        it.ChargesItemId = netI(val('CmbChargesNameChargeDetail')); it.ChargesItem = X.selText('CmbChargesNameChargeDetail'); it.AddAmount = netD(val('txtAddAmountChargeDetail')); it.LessAmount = netD(val('txtLessAmountChargeDetail')); it.Remarks = val('txtRemarksChargeDetail');
        chargeRender(); chargeReset(); chargesToHeader();
    }
    function chargeCancel() { chargeReset(); }
    function chargeDelete(i) { try { if (uIdx.charge !== -1) throw new Error('Reset Detail First...'); CHARGES.splice(i, 1); CUR.charge = -1; chargeRender(); chargesToHeader(); } catch (e) { box(e.message); } }
    function chargeReset() { uIdx.charge = -1; X.setVal('CmbChargesNameChargeDetail', '0'); setText('txtAddAmountChargeDetail', ''); setText('txtLessAmountChargeDetail', ''); setText('txtRemarksChargeDetail', ''); X.show('BtnAddChargeDetail', true); X.show('BtnUpdateChargeDetail', false); X.show('BtnCancelChargeDetail', false); X.focus('CmbChargesNameChargeDetail'); }

    function renderAll() { detailRender(); otherRender(); payRender(); commRender(); chargeRender(); }

    // ------------------------------------------------------------------ form
    /* FormReset */
    function formReset() {
        REMOVED = []; X.show('btnsave', true); X.show('btnupdate', false); UpdateMode = false; RecId = 0; ExportVoucherId = 0;
        var p = X.getJson(API + '/generate-code').then(function (d) { if (d && netI(d.docNo) > 0) setText('txtdocno', d.docNo); setText('txtLastInvoiceNumber', str(d && d.lastInvoiceNumber)); })
            .then(function () { return X.getJson(API + '/commercial-invoices?recId=0'); }).then(function (rows) { CI_INV = rows || []; bindCiInvoices(); ciInvoiceLeave(); }).catch(function (e) { box(e.message); });
        if ((C.branches || []).length) X.setVal('cmbbranches', C.branches[0].Id);
        if ((C.projects || []).length) X.setVal('cmbproject', C.projects[0].Id);
        X.setVal('cmbcareiertype', 1);
        setText('txtDocDate', X.today());
        X.setVal('cmbSupCust', '0'); customerLeave();
        ['cmbConsignee', 'cmbNotifyParty1', 'cmbNotifyParty2', 'cmbimporterBankNew', 'cmbdeliverytermnew', 'cmbLoadingPort', 'cmbDestinationPort', 'cmbexporterBankNew', 'cmbfcycode'].forEach(function (id) { X.setVal(id, '0'); });
        ['txtLotRef', 'txtfcyAmount', 'txtExRate', 'txtLocalAmt', 'txtGrossWeight', 'txtNetWeight', 'txtNoOfContainer', 'txtIformNo', 'txtaddlesscommnets', 'txtaddlessamount', 'txtTotalNetAmount', 'txtCustomerContractNos', 'cmbInvoiceNo'].forEach(function (id) { setText(id, ''); });
        DT = []; PAY = []; OTHER = []; COMM = []; CHARGES = [];
        chargeReset(); uIdx = { detail: -1, other: -1, pay: -1, comm: -1, charge: -1 }; CUR = { detail: -1, other: -1, pay: -1, comm: -1, charge: -1, hist: -1 };
        X.setVal('combitem', '0'); setText('txtIformDate', X.today()); X.focus('txtDocDate');
        renderAll();
        detailFormReset(); payReset(); commAddEnabled = true; commReset();
        return p;
    }
    function newClick() { return formReset().then(function () { return detailFormReset(); }); }
    /* btnRefresh_Click: every combo re-read, values kept. */
    function refresh(btn) { return X.busy(btn, function () { return X.getJson(API + '/refresh?recId=' + RecId).then(function (d) { Object.keys(d || {}).forEach(function (k) { if (/Error$/.test(k) && d[k]) box(d[k]); }); var keepDoc = val('txtdocno'); bindAll(d || {}); if (keepDoc) setText('txtdocno', keepDoc); hsCodeBind(); }).catch(function (e) { box(e.message); }); }); }
    /* formvalidation (desktop order) - the server repeats it. */
    function formValidation() {
        if (CFG.allowExportMultiCompanies && !X.hasSel('CmbExportCompany')) { box('Export Company Is required'); X.focus('CmbExportCompany'); return false; }
        if (!val('txtdocno') || netI(val('txtdocno')) === 0) { box('Doc No Is Required'); X.focus('txtdocno'); return false; }
        if (!X.hasSel('CmbInvoiceNoCI')) { box('Invoice No Is Required'); X.focus('CmbInvoiceNoCI'); return false; }
        if (!val('cmbInvoiceNo') || !val('txtdocno')) { box('Custom Invoice No Is Required'); X.focus('cmbInvoiceNo'); return false; }
        if (netI(val('cmbSupCust')) === 0) { box('Customer Is Required'); X.focus('cmbSupCust'); return false; }
        if (CFG.flagFin && netI(val('CmbCreditAccount')) === 0) { if (!ask('Are you sure to Credit Value charge to Sale Account?')) { box('Please select CreditAc'); X.focus('CmbCreditAccount'); return false; } creditToSaleConfirmed = true; }
        if (netI(val('cmbdeliverytermnew')) === 0) { box('Delivery Term Is Required'); X.focus('cmbdeliverytermnew'); return false; }
        if (netI(val('cmbLoadingPort')) === 0) { box('Loading Port Is Required'); X.focus('cmbLoadingPort'); return false; }
        if (netI(val('cmbDestinationPort')) === 0) { box('Destination Port  Is Required'); X.focus('cmbDestinationPort'); return false; }
        if (netI(val('cmbcareiertype')) === 0) { box('Carrier Type  Is Required'); X.focus('cmbcareiertype'); return false; }
        if (netI(val('cmbfcycode')) === 0) { box('Fcy Code Is Required'); X.focus('cmbfcycode'); return false; }
        if (netD(val('txtfcyAmount')) === 0) { box('Fcy Amount  Is Required'); X.focus('txtfcyAmount'); return false; }
        if (netI(val('txtNoOfContainer')) === 0) { box('No.of Containers Field Is Required'); X.focus('txtNoOfContainer'); return false; }
        if (!(netD(val('txtNetWeight')) > 0)) { box('Net Weight  Is Required'); X.focus('txtNetWeight'); return false; }
        if (!(netD(val('txtGrossWeight')) >= netD(val('txtNetWeight')))) { box('Gross Weight Must Be Equal Or Greater Than Net Weight Thank You'); X.focus('txtGrossWeight'); return false; }
        var r = ciRow(), balKg = (r ? netD(col(r, 'BalWeight')) : 0) / 1000;
        if (netD(val('txtNetWeight')) > balKg) { box('Net MTon:' + netD(val('txtNetWeight')) + ' must be equal to or Less than Balance MTon of Selected Invoice :' + balKg + '. Thank you.'); X.subTab('ciDetail', 'detail'); return false; }
        if (!(netD(val('txtExRate')) > 0)) { box('ExchangeRate Field is Required'); X.focus('txtExRate'); return false; }
        if (!(netD(val('txtTotalNetAmount')) > 0)) { box('Total Amount  Is Required'); X.focus('txtTotalNetAmount'); return false; }
        return true;
    }
    var creditToSaleConfirmed = false;
    /* btnsave_Click */
    function save(btn) {
        return X.busy(btn, function () {
            if (DT.length <= 0) { box('Grid Record not found'); return Promise.resolve(); }
            if (!ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return Promise.resolve();
            setText('txtNetWeight', fmt(X.sum(DT, 'QtyMTon')));
            if (netD(val('txtaddlessamount')) > 0 && CHARGES.length === 0 && !ask("Are you sure to proceed because add less amount not zero but Charges grid don't have any record?")) { X.subTab('ciDetail', 'charges'); X.focus('CmbChargesNameChargeDetail'); return Promise.resolve(); }
            creditToSaleConfirmed = false;
            if (!formValidation()) return Promise.resolve();
            if (PAY.length === 0) { X.subTab('ciDetail', 'payment'); X.focus('dcmbpaymentterm'); box('Payment Detail Not Found. Please Check! '); return Promise.resolve(); }
            var r = ciRow() || {};
            var header = {
                allowExportMultiCompanies: CFG.allowExportMultiCompanies, creditToSaleConfirmed: creditToSaleConfirmed,
                ProjectsId: netI(val('cmbproject')), DocCode: netI(val('txtdocno')), DocDate: val('txtDocDate') || X.today(), CommercialInvoiceId: netI(val('CmbInvoiceNoCI')),
                InvoiceNo: val('cmbInvoiceNo').trim(), SupplierCustomerId: netI(val('cmbSupCust')), ConsigneeId: netI(val('cmbConsignee')), NotifyParty1: netI(val('cmbNotifyParty1')), NotifyParty2: netI(val('cmbNotifyParty2')),
                LotNoRef: val('txtLotRef'), EFormNo: val('txtIformNo').trim(), EFormDate: val('txtIformDate') || X.today(), FcurrencyId: netI(val('cmbfcycode')), ConversionRate: netD(val('txtExRate')),
                FCurrencyAmount: netD(val('txtfcyAmount')), EquivalentAmount: netD(val('txtLocalAmt')), AddLessAmount: netD(val('txtaddlessamount')), TotalAmount: netD(val('txtTotalNetAmount')), AddLessComments: val('txtaddlesscommnets'),
                DeliveryTermId: netI(val('cmbdeliverytermnew')), PaymentRemarks: val('txtPaymentRemarks'), DeliveryRemarks: val('txtDeliveryRemarks'), ImporterBankId: netI(val('cmbimporterBankNew')), ExporteBankId: netI(val('cmbexporterBankNew')),
                LoadingPortId: netI(val('cmbLoadingPort')), DestinationPortId: netI(val('cmbDestinationPort')), CarierTypeId: netI(val('cmbcareiertype')), CarierType: X.selText('cmbcareiertype'),
                GrossMton: netD(val('txtGrossWeight')), NetMton: netD(val('txtNetWeight')), ExportCompanyId: netI(val('CmbExportCompany')), NoOfContainers: netI(val('txtNoOfContainer')), CreditAccountId: netI(val('CmbCreditAccount')),
                CustomerContractNos: val('txtCustomerContractNos'), InvoiceBalWeight: netD(col(r, 'BalWeight')), AttachmentsValues: AttachmentsValues, CustomAttachmentsValues: CustomAttachmentsValues
            };
            return X.postJson(API + '/save', { recId: RecId, header: header, detail: DT, removed: REMOVED, otherItems: OTHER, paymentTerms: PAY, commissions: CFG.saleCommissionTabFeatureWise ? COMM : COMM, otherCharges: CHARGES }).then(function (d) {
                box((d && d.message) || (RecId === 0 ? 'Save SuccessFully' : 'Update SuccessFully'));
                var id = d && d.id;
                return formReset().then(function () { if ($id('ChkPrintPreview').checked && id) X.print('exp-521', { id: id, eximInvoiceId: id }); });
            }).catch(function (e) { box(e.message); });
        });
    }
    var AttachmentsValues = '', CustomAttachmentsValues = '';
    function update(btn) { if (RecId === 0) { box('Rec Id not found...'); return Promise.resolve(); } return save(btn); }
    /* ReadById(Id) */
    function readById(id) {
        REMOVED = []; X.show('btnsave', false); X.show('btnupdate', true); RecId = id;
        return X.getJson(API + '/by-id?id=' + id).then(function (o) {
            o = o || {};
            X.innerTab('ci', 'ciForm', 'btnCiFooterHistory');
            X.setVal('cmbbranches', o.BranchesId); X.setVal('cmbproject', o.ProjectsId);
            setText('txtdocno', str(o.DocCode)); setText('txtDocDate', X.isoDate(o.DocDate));
            return X.getJson(API + '/commercial-invoices?recId=' + RecId).then(function (rows) { CI_INV = rows || []; bindCiInvoices(); }).then(function () {
                X.setVal('CmbInvoiceNoCI', o.CommercialInvoiceId); ciInvoiceLeave();
                setText('cmbInvoiceNo', o.InvoiceNo);
                X.setVal('cmbSupCust', o.SupplierCustomerId);
                return customerLeave();
            }).then(function () {
                X.setVal('cmbConsignee', o.ConsigneeId); X.setVal('cmbNotifyParty1', o.NotifyParty1); X.setVal('cmbNotifyParty2', o.NotifyParty2);
                setText('txtLotRef', o.LotNoRef); X.setVal('cmbimporterBankNew', o.ImporterBankId); X.setVal('cmbexporterBankNew', o.ExporteBankId); X.setVal('cmbdeliverytermnew', o.DeliveryTermId);
                X.setVal('cmbLoadingPort', o.LoadingPortId); X.setVal('cmbDestinationPort', o.DestinationPortId);
                var ct = CARRIER.filter(function (c) { return c.Type === o.CarierType; })[0]; X.setVal('cmbcareiertype', ct ? ct.Id : '0');
                X.setVal('cmbfcycode', o.FcurrencyId);
                setText('txtfcyAmount', X.fmt4(o.FCurrencyAmount)); setText('txtLocalAmt', X.fmt4(o.EquivalentAmount)); X.setVal('CmbExportCompany', o.ExportCompanyId);
                setText('txtGrossWeight', netD(o.GrossMton) > 0 ? X.fmt4(o.GrossMton) : X.fmt4(netD(o.GrossWeight) / 1000));
                setText('txtNetWeight', netD(o.NetMton) > 0 ? X.fmt4(o.NetMton) : X.fmt4(netD(o.NetWeight) / 1000));
                setText('txtNoOfContainer', str(o.NoOfContainers)); setText('txtIformNo', o.EFormNo); setText('txtIformDate', X.isoDate(o.EFormDate));
                setText('txtPaymentRemarks', o.PaymentRemarks); setText('txtDeliveryRemarks', o.DeliveryRemarks); setText('txtaddlesscommnets', o.AddLessComments);
                setText('txtaddlessamount', str(o.AddLessAmount)); setText('txtTotalNetAmount', X.fmt4(o.TotalAmount)); X.setVal('CmbCreditAccount', o.CreditAccountId);
                setText('txtCustomerContractNos', o.CustomerContractNos);
                AttachmentsValues = str(o.AttachmentsValues); CustomAttachmentsValues = str(o.CustomAttachmentsValues);
                ExportVoucherId = netI(o.ExportVoucherId);
                commAddEnabled = !(ExportVoucherId > 0); $id('btnAddCommission').disabled = !commAddEnabled; $id('btnUpdateCommision').disabled = !commAddEnabled;
                DT = (o.detail || []).map(X.copy); REMOVED = [];
                OTHER = (o.otherItems || []).map(X.copy); PAY = (o.paymentTerms || []).map(X.copy); COMM = (o.commissions || []).map(X.copy); CHARGES = (o.otherCharges || []).map(X.copy);
                renderAll();
                chargesToHeader();
                proportionateOtherItems();
                detailFormReset();
                UpdateMode = true; X.focus('txtDocDate');
                setText('txtExRate', X.fmt4(o.ConversionRate)); onExRate();
                proportionateAddLess();
                payBalance();
            });
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ prints / popups
    function print521(btn) { return X.print('exp-521', { id: RecId, eximInvoiceId: RecId }, btn); }
    function print529(btn) { return X.print('exp-529', { id: RecId }, btn); }
    function print501B(btn) { return X.print('501b-exprptsalescontractexport', { contractId: 0, invoiceId: RecId }, btn); }
    function attachment() { box('Attachments (Attachment form) are not part of the web port.'); }
    function popup(name) { box(name + ' is not available on the web yet.'); }
    function shortcuts() {
        X.shortcutPopup([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Doc Date'],
            ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+L', 'For Load All Records'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowRight', 'For Moving In Detail Tabs'],
            ['Ctrl+ArrowDown', 'For Focus On Selected Tab Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On First Entry of Detail Selected Tab'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    // ------------------------------------------------------------------ history
    function btn(act, i, text) { return '<button type="button" class="win-edit" data-act="' + act + '" data-i="' + i + '">' + text + '</button>'; }
    var HIST_COLS = [
        { key: '_e', html: function (v, r, i) { return PERM.Update ? '<button type="button" class="win-edit" data-edit="' + i + '">Edit</button>' : ''; }, cls: 'win-cell-btn' },
        { key: '_gp', html: function (v, r, i) { return PERM.Print ? btn('gp', i, 'Gp Print') : ''; }, cls: 'win-cell-btn' },
        { key: '_do', html: function (v, r, i) { return PERM.Print ? btn('do', i, 'Do Print') : ''; }, cls: 'win-cell-btn' },
        { key: '_pl', html: function (v, r, i) { return PERM.Print ? btn('pl', i, 'Print Packing List') : ''; }, cls: 'win-cell-btn' },
        { key: '_s', html: function (v, r, i) { return PERM.Print ? btn('slip', i, '521-Slip') : ''; }, cls: 'win-cell-btn' },
        { key: 'InvoiceNo', link: true }, { key: 'DocCode', num: true, fmt: function (v) { return str(netI(v)); } }, { key: 'DocDate', fmt: X.ddMMMyyyy }, 'CustomerName', 'ContractNos',
        { key: 'NoOfContainers', num: true, sum: true, fmt: fmt }, { key: 'GrossWeight', num: true, sum: true, fmt: fmt }, { key: 'NetWeight', num: true, sum: true, fmt: fmt },
        { key: 'FcyAmount', num: true, sum: true, fmt: fmt }, { key: 'AddLess', num: true, sum: true, fmt: fmt }, { key: 'TotalAmount', num: true, sum: true, fmt: fmt }, 'JobLots', 'LoadingPort', 'DestinationPort',
        'EntryUser', { key: 'EntryDate', fmt: X.ddMMyyyyHm }, 'ModifyUser', { key: 'ModifyDate', fmt: X.ddMMyyyyHm }, { key: 'NoOfAttachments', num: true, fmt: function (v) { return str(netI(v)); } },
        { key: '_att', html: function (v, r, i) { return btn('att', i, 'Add Attachment'); }, cls: 'win-cell-btn' }
    ];
    function histRender() {
        X.show('ciHistEditTh', PERM.Update);
        document.querySelectorAll('.ciHistPrintTh').forEach(function (th) { th.classList.toggle('is-hidden', !PERM.Print); });
        X.drawGrid('ciHistBody', 'ciHistFoot', HIST, HIST_COLS, { cur: CUR.hist, empty: 'ciHistEmpty' });
    }
    /* HistoryGridFill */
    function historyShow(b) {
        return X.busy(b, function () {
            var body = { dateKind: (document.querySelector('input[name="ciDateKind"]:checked') || {}).value || 'doc', fromChecked: $id('FromDateHistoryChk').checked, fromDate: val('FromDateHistory'),
                toChecked: $id('ToDateHistoryChk').checked, toDate: val('ToDateHistory'), fromDocNo: netI(val('FromDocNoHistory')), toDocNo: netI(val('ToDocNoHistory')),
                customerId: netI(val('CmbCustomerHistory')), invoiceId: netI(val('CmbInvoiceNoHistory')), actionId: netI((document.querySelector('input[name="ciReferred"]:checked') || {}).value) };
            return X.postJson(API + '/history', body).then(function (rows) { HIST = rows || []; CUR.hist = -1; histRender(); HIST_DET = []; X.drawGrid('ciHistDetBody', 'ciHistDetFoot', HIST_DET, DET_HIST_COLS, {}); }).catch(function (e) { box(e.message); });
        });
    }
    function histSelect(i) {
        CUR.hist = i; var r = HIST[i]; if (!r) return;
        X.getJson(API + '/history-detail?id=' + netI(col(r, 'Id'))).then(function (rows) { HIST_DET = rows || []; X.drawGrid('ciHistDetBody', 'ciHistDetFoot', HIST_DET, DET_HIST_COLS, {}); }).catch(function (e) { box(e.message); });
    }
    function histAct(a, i) {
        var r = HIST[i]; if (!r) return; var id = netI(col(r, 'Id'));
        if (a === 'slip') X.print('exp-521', { id: id, eximInvoiceId: id });
        else if (a === 'pl') X.print('exp-529', { id: id });
        else if (a === 'do') X.print('exp-do-84', { invoiceId: id, documentTypeId: 84 });
        else if (a === 'gp') X.print('295-invrptoutwardgatepassslipwithitems', { invoiceId: id, id: 0 });
        else if (a === 'att') box('Attachments (AttachmentAddingFromHistory) are not part of the web port.');
    }
    function historyReset() { setText('FromDateHistory', X.daysAgo(3)); setText('ToDateHistory', X.today()); setText('FromDocNoHistory', ''); setText('ToDocNoHistory', ''); X.setVal('CmbCustomerHistory', '0'); HIST = []; CUR.hist = -1; histRender(); X.show('ciHistEmpty', false); HIST_DET = []; X.drawGrid('ciHistDetBody', 'ciHistDetFoot', HIST_DET, DET_HIST_COLS, {}); }
    function historyRefresh(b) { return X.busy(b, function () { return X.getJson(API + '/history-combos').then(bindHistoryCombos).catch(function (e) { box(e.message); }); }); }
    function tab(group, panelId) { X.innerTab(group, panelId, 'btnCiFooterHistory', function (p, onHist) { X.focus(onHist ? 'FromDateHistory' : 'txtDocDate'); }); }
    function toggleHistory() { tab('ci', X.activeInner('ci') === 'ciHistory' ? 'ciForm' : 'ciHistory'); }

    // ------------------------------------------------------------------ wiring
    document.addEventListener('DOMContentLoaded', function () {
        X.wireTabs(tab);
        X.on('CmbInvoiceNoCI', 'change', ciInvoiceLeave);
        X.on('cmbSupCust', 'change', customerLeave);
        X.on('combitem', 'change', itemLeave);
        X.on('cmbdeliverytermnew', 'change', function () { descInto('cmbdeliverytermnew', 'deliveryTerms', 'txtDeliveryRemarks'); });
        X.on('cmbPaymentTermsNew', 'change', function () { descInto('cmbPaymentTermsNew', 'paymentTerms', 'txtPaymentRemarks'); });
        X.on('dcmbpaymentterm', 'change', function () { descInto('dcmbpaymentterm', 'paymentTerms', 'txtPaymentRemarksDetail'); paymentTermDetailLeave(); });
        X.on('CmbFinInstruments', 'change', fiChanged);
        X.on('txtQtyMTon', 'input', onQtyRate); X.on('CmbPackUom', 'change', onQtyRate); X.on('txtCostMTon', 'input', onQtyRate); X.on('CmbRateUom', 'change', onQtyRate);
        X.on('txtExRate', 'input', onExRate);
        X.on('txtoQty', 'input', otherCalc); X.on('txtoRate', 'input', otherCalc); X.on('CmbOtherItem', 'change', otherItemChanged);
        X.on('dtxtpercentoftotal', 'input', payPercentCalc); X.on('dtxtfcyamount', 'input', payAmountChanged);
        X.on('cmbCommissionType', 'change', commTypeChanged); X.on('txtCommRate', 'input', calcCommission); X.on('txtCommRateUom', 'input', calcCommission);
        X.on('txtCommRateUom', 'keydown', function () { commRateUomTag = val('txtCommRateUom'); });
        X.wireGrid('ciDetBody', { del: detailDelete, open: detailEdit, select: function (i) { CUR.detail = i; }, act: function (a, i) { if (a === 'dup') detailDup(i); } });
        X.wireGrid('ciOtherBody', { open: otherEdit, select: function (i) { CUR.other = i; } });
        X.wireGrid('ciPayBody', { del: payDelete, open: payEdit, select: function (i) { CUR.pay = i; } });
        X.wireGrid('ciCommBody', { del: commDelete, open: commEdit, select: function (i) { CUR.comm = i; } });
        X.wireGrid('ciChargeBody', { del: chargeDelete, open: chargeEdit, select: function (i) { CUR.charge = i; } });
        X.wireGrid('ciHistBody', { open: function (i) { var r = HIST[i]; if (r) readById(netI(col(r, 'Id'))); }, select: histSelect, act: histAct });
        /* ImProformaInvoice_KeyDown */
        var SUBS = ['detail', 'other', 'payment'];
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (X.enterMovesOn(e)) return;
            var onForm = X.activeInner('ci') === 'ciForm';
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); X.cancel(); return; }
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); return; }
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
            if (onForm) {
                if (e.ctrlKey && k === 's' && !UpdateMode) { e.preventDefault(); save($id('btnsave')); }
                if (e.ctrlKey && k === 'u' && UpdateMode) { e.preventDefault(); save($id('btnupdate')); }
                if (e.ctrlKey && k === 'n') { e.preventDefault(); newClick(); }
                if (e.ctrlKey && k === 'p' && PERM.Print) { e.preventDefault(); print521($id('btnPrint')); }
                if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh($id('btnRefresh')); }
                if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); attachment(); }
                if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); X.focus('txtDocDate'); }
                if (e.ctrlKey && e.key === 'ArrowRight') { e.preventDefault(); var s = X.activeSub('ciDetail'), n = SUBS[(SUBS.indexOf(s) + 1) % SUBS.length]; X.subTab('ciDetail', n); X.focus(n === 'detail' ? 'combitem' : n === 'other' ? 'CmbOtherItem' : 'dcmbpaymentterm'); }
                if (e.ctrlKey && e.key === 'ArrowUp') { e.preventDefault(); var s2 = X.activeSub('ciDetail'); X.focus(s2 === 'detail' ? 'combpcktype' : s2 === 'other' ? 'CmbOtherItem' : 'dcmbpaymentterm'); }
                if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); var s3 = X.activeSub('ciDetail'); var tb = $id(s3 === 'detail' ? 'ciDetBody' : s3 === 'other' ? 'ciOtherBody' : 'ciPayBody').querySelector('tr'); if (tb) tb.scrollIntoView(); }
                return;
            }
            if (e.ctrlKey && k === 's') { e.preventDefault(); historyShow($id('btnShowHistory')); }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); historyReset(); }
            if (e.ctrlKey && k === 'r') { e.preventDefault(); historyRefresh($id('btnRefreshHistory')); }
            if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); X.focus('FromDateHistory'); }
            if (e.ctrlKey && (e.key === 'ArrowDown' || e.key === 'ArrowRight')) { e.preventDefault(); var t = $id('ciHistBody').querySelector('tr'); if (t) t.scrollIntoView(); }
            if (e.ctrlKey && e.key === 'Enter' && CUR.hist >= 0) { e.preventDefault(); var hr = HIST[CUR.hist]; if (hr) readById(netI(col(hr, 'Id'))); }
        });
        load();
    });

    global.ExportCustomInvoice = {
        newClick: newClick, refresh: refresh, save: save, update: update, attachment: attachment, print521: print521, print529: print529, print501B: print501B, popup: popup, shortcuts: shortcuts,
        detailAdd: detailAdd, detailUpdate: detailUpdate, detailCancel: detailCancel, otherAdd: otherAdd, otherUpdate: otherUpdate, otherCancel: otherCancel,
        payAdd: payAdd, payUpdate: payUpdate, payCancel: payCancel, commAdd: commAdd, commUpdate: commUpdate, commCancel: commCancel,
        chargeAdd: chargeAdd, chargeUpdate: chargeUpdate, chargeCancel: chargeCancel,
        historyShow: historyShow, historyReset: historyReset, historyRefresh: historyRefresh, toggleHistory: toggleHistory
    };
}(window));
