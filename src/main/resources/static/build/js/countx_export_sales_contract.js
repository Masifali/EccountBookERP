/* ============================================================================================
 * countx_export_sales_contract.js - FrmExportSalesContract.cs (Architecture.WinApp.Export), screen 209
 * "Export Contract", DocumentTypeId 202. Form | History. Every button, Leave/TextChanged, grid
 * double-click, X button, history button and shortcut of the desktop form has its counterpart here
 * with the desktop's messages and order; data comes from /api/export/sales-contract
 * (ExportSalesContractController -> ExportSalesContractService -> the desktop's own procedures).
 * Prints: CrystalPrint keys exp-501 (501-Print / Preview / history Print), exp-501a (501A-Print /
 * history Print501A), exp-501new (history PrintII), exp-523 (history Print523).
 * ============================================================================================ */
(function (global) {
    'use strict';

    var X = global.ExportG;
    var API = '/api/export/sales-contract';
    var $id = X.$id, box = X.box, ask = X.ask, str = X.str, netI = X.netI, netD = X.netD, fmt = X.fmt, fmtFixed = X.fmtFixed, roundAway = X.roundAway;
    var val = X.val, valI = X.valI, valD = X.valD, setVal = X.setVal, setText = X.setText, text = X.text, bind = X.bind, show = X.show, focus = X.focus, checked = X.checked;

    // ------------------------------------------------------------------------------ state

    var PERM = { Save: true, Update: true, Delete: true, Print: true };
    var CFG = { allItemsBindonExportContract: false, pmGridInfoShouldNotCompulsoryOnContract: false, defaultDaysToLessFromHistoryFromDate: 0, itemSearchByCode: false, fcyDecimals: 2 };
    var L = { customers: [], salesPersons: [], currencies: [], deliveryTerms: [], paymentTerms: [], ports: [], banks: { importer: [], exporter: [] }, items: [], packTypes: [], cropYears: [],
        exportCharges: [], otherItems: [], pmItems: [], farmingNTrade: [], exportCompanies: [], customGroups: [], commodityRemarks: [], lastSaved: null, itemPmMap: [], historyCustomers: [] };
    var FEAT = { multi: false, sdf: false };
    var S = { recId: 0, saveAs: false, details: [], removed: [], updateIndex: -1, uoms: [], attachmentsValues: '', customAttachmentsValues: '' };
    var HIST = [];
    var tabsMain, tabs2, tabsH;

    function fcyDec() { return CFG.fcyDecimals > 0 ? CFG.fcyDecimals : 2; }

    // ------------------------------------------------------------------------------ grids

    var detailGrid = X.grid('grdDetails', [
        { key: 'ItemCode' }, { key: 'ItemName' }, { key: 'PackType' }, { key: 'CropYear' }, { key: 'QtyMTon', num: true, dec: 6 }, { key: 'PackSize' },
        { key: 'QtyEquivalent', num: true }, { key: 'NoOfBags', num: true }, { key: 'Rate', num: true, dec: 4 }, { key: 'RateUOM' }, { key: 'Amount', num: true, dec: 4 },
        { key: 'OtherItemAmount', num: true, dec: 4 }, { key: 'CommodityDetail' }, { key: 'FarmingNTrade' }, { key: 'Remarks' }
    ], { withX: true, totals: ['QtyMTon', 'NoOfBags', 'Amount', 'OtherItemAmount'], onDelete: deleteDetail, onDblClick: editDetail });

    var pm = X.pmTab({
        ids: { brand: 'cmbBrandForPmEntry', brandUom: 'CmbBrandUomPm', brandPackType: 'CmbBrandPackingTypePm', brandOuter: 'txtBrandOuterQtyPm', brandInner: 'txtBrandInnerQtyPm',
            pmItem: 'CmbPMItem', pmUom: 'CmbUomPm', pmOuter: 'txtOuterQtyPm', pmInner: 'txtInnerQtyPm', rate: 'txtPackingItemRate', amount: 'txtPackingAmount', remarks: 'txtPackingRemarks',
            add: 'btnPackingAdd', update: 'btnPackingUpdate', cancel: 'btnPackingCancel', grid: 'grdPackingMaterialDetail',
            brandByCode: 'radBrandName', brandByName: 'rabBrandName', pmByCode: 'rdPackingItemCode', pmByName: 'rdPackingItemName' },
        api: { lastRate: API + '/last-rate?itemId=', pmUoms: API + '/uom-by-item?itemId=' },
        fcyDecimals: fcyDec
    });
    var pay = X.paymentTab({
        ids: { term: 'CmbPaymentTermDetail', pct: 'PercntOfTotal', amount: 'txtPaymentDetailFcyAmount', remarks: 'txtPaymentRemarksDetail', add: 'btnAddPayment', update: 'btnUpdatePayment', cancel: 'btnCancelPayment', grid: 'grdPaymentTerm' },
        fcyDecimals: fcyDec, headerFcyAmount: function () { return val('txtFCYAmount'); }, terms: function () { return L.paymentTerms; }
    });
    var other = X.otherItemsTab({
        ids: { item: 'CmbOtherItem', qty: 'txtoQty', rate: 'txtoRate', amount: 'txtoAmount', remarks: 'txtoRemarks', add: 'btnoadd', update: 'btnoUpdate', cancel: 'btnoCancel', grid: 'grdotheritems' },
        onChanged: function () { headerTotals(); proportionate(); }
    });
    var charges = X.otherChargesTab({
        ids: { item: 'CmbChargesNameChargeDetail', add: 'txtAddAmountChargeDetail', less: 'txtLessAmountChargeDetail', remarks: 'txtRemarksChargeDetail', addBtn: 'BtnAddChargeDetail', update: 'BtnUpdateChargeDetail', cancel: 'BtnCancelChargeDetail', grid: 'grdOtherChargesDetail' }
    });
    var histGrid = X.grid('DataGridHistory', [
        { key: 'DocNo', num: true, dec: 0 }, { key: 'DocDate', fmt: X.ddMMMyyyy }, { key: 'OrderNo' }, { key: 'OrderDate', date: true }, { key: 'CustomerName' }, { key: 'LotReference' },
        { key: 'MTons', num: true }, { key: 'NoOfContainers', num: true, dec: 0 }, { key: 'FcyCode' }, { key: 'FcyAmount', num: true, dec: 2 }, { key: 'LastShipmentDate', date: true },
        { key: 'NotifyParty' }, { key: 'DeliveryTerm' }, { key: 'PaymentTerm' }, { key: 'LoadingPort' }, { key: 'DestinationPort' }, { key: 'Commodity_Description' },
        { key: 'EntryUser' }, { key: 'EntryDate', datetime: true }, { key: 'ModifyUser' }, { key: 'ModifyDate', datetime: true }, { key: 'NoOfAttachments', num: true, dec: 0, link: true }
    ], {
        buttons: [{ key: 'Print', text: 'Print' }, { key: 'Print501A', text: 'Print501A' }, { key: 'PrintII', text: 'PrintII' }, { key: 'Print523', text: 'Print523' }, { key: 'Edit', text: 'Edit' }, { key: 'SaveAs', text: 'SaveAs' }],
        totals: ['MTons', 'NoOfContainers', 'FcyAmount'], emptyId: 'scHistEmpty',
        onButton: historyButton, onDblClick: function (i) { historyButton(i, 'Edit'); }, onSelect: historySelected,
        onLink: function () { box('Attachments (DMS) are not available in the web port.'); }
    });
    var hDetail = X.grid('grddetailhistory', X.HIST_COLS.detail, { totals: ['QtyMTon', 'NoOfBags', 'Amount'] });
    var hPay = X.grid('grdPaymentHistory', X.HIST_COLS.payment, { totals: ['PrcntOfTotal', 'FcyAmount'] });
    var hPm = X.grid('grdPMHistory', X.HIST_COLS.pm, { totals: ['PmItemOuterQty', 'PmItemInnerQty', 'Amount'] });
    var hOther = X.grid('grdOtherItemHistory', X.HIST_COLS.other, { totals: ['Amount'] });
    var hCharges = X.grid('grdOtherChargesHistory', X.HIST_COLS.charges, { totals: ['AddAmount', 'LessAmount'] });

    // ------------------------------------------------------------------------------ load

    function applyLists(d, initial) {
        FEAT.multi = !!d.allowExportMultiCompanies; FEAT.sdf = !!d.shipmentDocumentsFeature;
        if (d.config) CFG = d.config;
        ['customers', 'salesPersons', 'currencies', 'deliveryTerms', 'paymentTerms', 'ports', 'items', 'packTypes', 'cropYears', 'exportCharges', 'otherItems', 'pmItems', 'farmingNTrade', 'exportCompanies', 'customGroups', 'commodityRemarks', 'itemPmMap', 'historyCustomers']
            .forEach(function (k) { if (d[k] !== undefined) L[k] = d[k] || []; });
        if (d.banks) L.banks = d.banks;
        if (d.lastSaved !== undefined) L.lastSaved = d.lastSaved;
        bind('CmbCustomer', L.customers, 'Id', 'name'); bind('CmbNotifyParty', L.customers, 'Id', 'name');
        bind('CmbSalePerson', L.salesPersons, 'Id', 'name'); bind('CmbFCYCode', L.currencies, 'Id', 'name');
        bind('CmbDeliveryTerm', L.deliveryTerms, 'Id', 'name'); bind('CmbPaymentTerms', L.paymentTerms, 'Id', 'name'); bind('CmbPaymentTermDetail', L.paymentTerms, 'Id', 'name');
        bind('CmbLoadingPort', L.ports, 'Id', 'name'); bind('CmbDestinationPort', L.ports, 'Id', 'name');
        bind('CmbImporterBank', L.banks.importer, 'Id', 'name'); bind('CmbExporterbank', L.banks.exporter, 'Id', 'name');
        bindItems(); bind('combpcktype', L.packTypes, 'Id', 'name'); bind('CmbCropYear', L.cropYears, 'Id', 'name');
        bind('CmbChargesNameChargeDetail', L.exportCharges, 'Id', 'name'); bind('CmbFarmingNTrade', L.farmingNTrade, 'Id', 'name');
        other.bindItems(L.otherItems); pm.bindPmItems(L.pmItems);
        bind('cmbcomditiyDetailByItemCus', L.commodityRemarks, 'Id', 'name');
        show('exportCompanyWrap', FEAT.multi); if (FEAT.multi) bind('CmbExportCompany', L.exportCompanies, 'Id', 'name');
        show('dcgWrap', FEAT.sdf); if (FEAT.sdf) bind('CmbDocumentCustomGroup', L.customGroups, 'Id', 'name');
        if (d.historyCustomers) bind('CmbCustomerHistory', L.historyCustomers, 'Id', 'name');
        if (initial) {
            X.setChecked(CFG.itemSearchByCode ? 'rdSearchByCode' : 'rdSearchByName', true);
            X.setChecked(CFG.itemSearchByCode ? 'rdPackingItemCode' : 'rdPackingItemName', true);
            bindItems(); pm.bindPmItems(L.pmItems);
            var days = netI(CFG.defaultDaysToLessFromHistoryFromDate);
            setText('FromDateHistory', X.daysAgo(days > 0 ? days : 3)); setText('ToDateHistory', X.today());
        }
        applyLastSaved();
    }
    /* Load / Reset: the last saved record's sales person, currency, crop year and packing type when the sales person box is empty. */
    function applyLastSaved() {
        if (valI('CmbSalePerson') === 0 && L.lastSaved) {
            setVal('CmbSalePerson', L.lastSaved.SalesPersonId); setVal('CmbFCYCode', L.lastSaved.FcurrencyId);
            setVal('CmbCropYear', L.lastSaved.CropYearId); setVal('combpcktype', L.lastSaved.InvPackingMaterialTypeId);
        }
    }
    function bindItems() {
        var byCode = checked('rdSearchByCode');
        bind('combitem', L.items.map(function (r) { return { Id: r.Id, name: byCode ? r.ItemCode : r.ItemName }; }), 'Id', 'name');
    }
    function setup() {
        return X.getJson(API + '/setup').then(function (d) {
            PERM = d.permissions || PERM;
            X.setEnabled('btnsave', !!PERM.Save); X.setEnabled('BtnSaveAs', !!PERM.Save); X.setEnabled('btnUpdate', !!PERM.Update); X.setEnabled('Print', !!PERM.Print);
            var t = X.today();
            ['txtDocDate', 'ApprovalDate', 'datOrderDate', 'datShipmentStartDate', 'txtShipmentDate', 'txtFactoryLoadingDate', 'txtProductionScheduleDate', 'txtInspectionScheduleDate', 'txtPMScheduleDate'].forEach(function (id) { setText(id, t); });
            setText('txtExportSoNo', str(d.docNo));
            applyLists(d, true);
            focus('txtDocDate');
        });
    }

    // ------------------------------------------------------------------------------ header events

    function soNoLeave() {
        var no = val('txtExportSoNo').trim();
        if (!no) return;
        X.getJson(API + '/id-by-doc-no?docNo=' + encodeURIComponent(no)).then(function (r) {
            if (r && netI(r.id) > 0) return readById(netI(r.id), false);
        }).catch(function (e) { box(e.message); });
    }
    function termDesc(comboId, listKey, targetId) {
        if (valI(comboId) === 0) return;
        var r = X.rowOf(L[listKey], 'Id', val(comboId));
        if (r && str(r.desc)) setText(targetId, r.desc);
    }
    /* CmbCustomer_Leave: ItemSpecificationsBind + DocumentcustomGroupBind. */
    function customerLeave() {
        specsBind();
        if (FEAT.sdf) X.getJson(API + '/custom-groups?customerId=' + valI('CmbCustomer')).then(function (rows) { L.customGroups = rows || []; bind('CmbDocumentCustomGroup', L.customGroups, 'Id', 'name'); }).catch(function (e) { box(e.message); });
    }
    function specsBind() {
        return X.getJson(API + '/commodity-remarks?itemId=' + valI('combitem') + '&customerId=' + valI('CmbCustomer')).then(function (rows) { L.commodityRemarks = rows || []; bind('cmbcomditiyDetailByItemCus', L.commodityRemarks, 'Id', 'name'); }).catch(function (e) { box(e.message); });
    }
    function shipmentStartChanged() {
        if (val('datShipmentStartDate') < val('txtDocDate')) { setText('datShipmentStartDate', val('txtDocDate')); box("Shipment Start Date Can't Be Less Than Doc Date"); }
        calcShipmentDate();
    }
    function calcShipmentDate() {
        var days = valI('txtDeliveryDays');
        setText('txtShipmentDate', days === 0 ? val('datShipmentStartDate') : X.addDays(val('datShipmentStartDate'), valD('txtDeliveryDays')));
    }
    function shipmentEndChanged() {
        if (val('txtShipmentDate') < val('datShipmentStartDate')) { setText('txtShipmentDate', val('datShipmentStartDate')); box("Shipment End Date Can't Be Less Than Start Date"); }
    }
    function factoryLoadingChanged() {
        X.getJson(API + '/scheduling-policy').then(function (p) {
            if (!p || p.ProductionInterval === undefined) return;
            var ld = val('txtFactoryLoadingDate');
            setText('txtProductionScheduleDate', X.addDays(ld, -netI(p.ProductionInterval)));
            setText('txtInspectionScheduleDate', X.addDays(ld, -netI(p.InspectionInterval)));
            setText('txtPMScheduleDate', X.addDays(ld, -netI(p.PackMaterialInterval)));
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ detail entry

    function uomRow(id) { return X.rowOf(S.uoms, 'Id', id); }
    /* combitem_Leave_1: bindRateUomAndItemPackUom + ItemSpecificationsBind. */
    function itemLeave(detailId) {
        var packText = text('combitempck'), rateText = text('combrateuom');
        var itemId = valI('combitem');
        return X.getJson(API + '/uom-for-export?itemId=' + itemId).then(function (rows) {
            S.uoms = rows || [];
            if (S.uoms.length > 0) {
                bind('combitempck', S.uoms, 'Id', 'UOMCode'); bind('combrateuom', S.uoms, 'Id', 'UOMCode');
                var pk = S.uoms.filter(function (u) { return str(u.UOMCode) === packText; })[0];
                setVal('combitempck', packText && pk ? pk.Id : 0);
                var rk = S.uoms.filter(function (u) { return str(u.UOMCode) === rateText; })[0];
                setVal('combrateuom', rateText && rk ? rk.Id : 0);
                if (detailId) return;
                if (itemId !== 0 && (rateText !== '1000' || rateText.toUpperCase() !== '1000KG' || valI('combrateuom') === 0)) {
                    var base = S.uoms.filter(function (u) { return X.netB(u.BaseRateUom); })[0];
                    if (base) setVal('combrateuom', base.Id);
                    else { var eq = S.uoms.filter(function (u) { return netD(u.Equivalent) === 1000; })[0]; if (eq) setVal('combrateuom', eq.Id); }
                }
                if (text('combrateuom').toUpperCase() !== '1000KG' && text('combrateuom') !== '1000') setVal('combrateuom', 0);
            } else { bind('combitempck', [], 'Id', 'UOMCode'); bind('combrateuom', [], 'Id', 'UOMCode'); }
            calcWeight(); calcAmount();
        }).then(function () { if (!detailId) return specsBind(); }).catch(function (e) { box(e.message); });
    }
    /* CalculateWeight: M.Ton <-> No Of Bags through the pack uom's Equivalent, driven by the focused box's Tag. */
    function calcWeight() {
        var mton = valD('txtQtyMTon'), bags = valD('txtNoOfBags'), u = uomRow(val('combitempck'));
        if (u) {
            var outer = netD(u.Equivalent), tag = X.activeTag();
            if (tag === 'MTon') setText('txtNoOfBags', mton > 0 && outer > 0 ? fmt(Math.round(mton * 1000 / outer), 2) : '0');
            else if (tag === 'NoofBags') setText('txtQtyMTon', bags > 0 && outer > 0 ? fmt(bags * outer / 1000, 6) : '0');
            else if (mton > 0 && outer > 0) setText('txtNoOfBags', fmt(Math.round(mton * 1000 / outer), 2));
        } else setText('txtNoOfBags', '0');
    }
    /* CalculatAmount: Rate / RateUomEquivalent * NetWeightKgs rounded AwayFromZero to the FCY decimals. */
    function calcAmount() {
        setText('txtAmount', '0');
        var u = uomRow(val('combrateuom'));
        if (u && valI('combrateuom') > 0) {
            var rate = valD('txtCostMTon'), eq = netD(u.Equivalent), nw = valD('txtQtyMTon');
            if (rate > 0 && eq > 0 && nw > 0) setText('txtAmount', fmtFixed(roundAway(rate / eq * (nw * 1000), fcyDec()), fcyDec()));
        }
    }
    function detailValidation() {
        if (valI('combitem') === 0) { box('Item field required'); focus('combitem'); return false; }
        if (valI('combpcktype') === 0) { box('Pack Type field required'); focus('combpcktype'); return false; }
        if (valI('CmbCropYear') === 0) { box('Crop Year field is required'); focus('CmbCropYear'); return false; }
        if (valD('txtQtyMTon') === 0) { box('Qty/M.Ton field required'); focus('txtQtyMTon'); return false; }
        if (valI('combitempck') === 0) { box('Pack Size field required'); focus('combitempck'); return false; }
        if (valD('txtNoOfBags') === 0) { box('No of Bags field required'); focus('txtNoOfBags'); return false; }
        if (valD('txtCostMTon') === 0) { box('Rate field required'); focus('txtCostMTon'); return false; }
        if (valI('combrateuom') === 0) { box('Rate UOM field required'); focus('combrateuom'); return false; }
        if (valD('txtAmount') === 0) { box('Amount field required'); focus('txtAmount'); return false; }
        return true;
    }
    function detailRow(id) {
        var it = X.rowOf(L.items, 'Id', val('combitem')) || {}, f = X.rowOf(L.farmingNTrade, 'Id', val('CmbFarmingNTrade')), pk = uomRow(val('combitempck')) || {};
        return { Id: id || 0, ItemCode: str(it.ItemCode), ItemId: valI('combitem'), ItemName: str(it.ItemName), PackTypeId: valI('combpcktype'), PackType: text('combpcktype'),
            CropYearId: valI('CmbCropYear'), CropYear: text('CmbCropYear'), QtyMTon: valD('txtQtyMTon'), PackSizeId: valI('combitempck'), PackSize: text('combitempck'),
            QtyEquivalent: netD(pk.QtyEquivalent), NoOfBags: valD('txtNoOfBags'), Rate: valD('txtCostMTon'), RateUOMId: valI('combrateuom'), RateUOM: text('combrateuom'),
            Amount: valD('txtAmount'), OtherItemAmount: 0, CommodityDetail: val('txtCommodityDetailIndetail'),
            ExImFarmingNTradeId: f ? netI(f.Id) : 0, FarmingNTrade: f ? str(f.name) : '', ExImFarmingTypeId: f ? netI(f.ExImFarmingTypeId) : 0, ExImTradeTypeId: f ? netI(f.ExImTradeTypeId) : 0,
            Remarks: val('txtRemarks') };
    }
    function resetDetail() {
        S.updateIndex = -1;
        setVal('combitem', 0); bind('combitempck', [], 'Id', 'UOMCode'); bind('combrateuom', [], 'Id', 'UOMCode'); S.uoms = [];
        setText('txtQtyMTon', '0'); setText('txtNoOfBags', '0'); setText('txtCostMTon', '0'); setText('txtAmount', '0');
        setText('txtCommodityDetailIndetail', ''); setText('txtRemarks', ''); setVal('CmbFarmingNTrade', 0); setVal('cmbcomditiyDetailByItemCus', 0);
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
    }
    function addToGrid() {
        if (!detailValidation()) return;
        S.details.push(detailRow(0));
        drawDetails(); resetDetail(); focus('combitem');
    }
    function updateDetail() {
        if (!detailValidation() || S.updateIndex < 0) return;
        var id = netI(S.details[S.updateIndex].Id);
        S.details[S.updateIndex] = detailRow(id);
        drawDetails(); resetDetail(); focus('combitem');
    }
    function editDetail(i) {
        var r = S.details[i]; if (!r) return;
        S.updateIndex = i;
        setVal('combitem', r.ItemId);
        itemLeave(netI(r.Id)).then(function () {
            setVal('combpcktype', r.PackTypeId); setVal('combitempck', r.PackSizeId); setVal('CmbCropYear', r.CropYearId);
            setText('txtQtyMTon', str(r.QtyMTon)); setVal('combrateuom', r.RateUOMId); setText('txtNoOfBags', str(r.NoOfBags)); setText('txtCostMTon', str(r.Rate));
            setText('txtRemarks', r.Remarks); setText('txtCommodityDetailIndetail', r.CommodityDetail); setText('txtAmount', str(r.Amount)); setVal('CmbFarmingNTrade', r.ExImFarmingNTradeId);
            show('btnplus', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
            focus('combitem');
        });
    }
    function deleteDetail(i) {
        if (S.updateIndex !== -1) { box('Reset  the Detail first...'); return; }
        var r = S.details[i]; if (!r) return;
        if (netI(r.Id) > 0) { if (!ask('Are you sure to Delete?')) return; S.removed.push(r); }
        S.details.splice(i, 1);
        drawDetails();
    }
    function drawDetails() { detailGrid.draw(S.details); headerTotals(); proportionate(); }
    /* InsterAmountandQtyMtonInHeader */
    function headerTotals() {
        var total = detailGrid.total('Amount'), otherAmt = other.rows.length ? other.grid.total('Amount') : 0;
        setText('txtMTon', str(detailGrid.total('QtyMTon')));
        setText('txtFCYAmount', fmtFixed(total + otherAmt, fcyDec()));
    }
    /* ProportionateOtherItemAmount */
    function proportionate() {
        if (S.details.length === 0) return;
        var contract = detailGrid.total('Amount'), otherAmt = other.rows.length ? other.grid.total('Amount') : 0;
        S.details.forEach(function (r) { r.OtherItemAmount = otherAmt > 0 ? otherAmt / contract * netD(r.Amount) : 0; });
        detailGrid.draw(S.details);
    }
    function brandsFromDetails() { pm.bindBrands(pm.brandFromDetails(S.details, 'PackSizeId', 'NoOfBags')); }

    // ------------------------------------------------------------------------------ save / read / reset

    function headerBody() {
        return {
            Status: text('cmbStatus'), LcOrderDocNo: valI('txtExportSoNo'), LcOrderDate: val('txtDocDate'), SalesPersonId: valI('CmbSalePerson'), SupCustId: valI('CmbCustomer'),
            ConsigneeId: 0, NotifyPartyId: valI('CmbNotifyParty'), NotifyParty2Id: 0, NotifyParty3Id: 0, LotReference: val('txtLotRefNo'), LcOrderNo: val('txtBuyerOrderNo').trim(),
            SalesContratDate: val('datOrderDate'), ShipmentStartDate: val('datShipmentStartDate'), LastShipmentDate: val('txtShipmentDate'),
            DeliveryTermId: valI('CmbDeliveryTerm'), PaymentTermId: valI('CmbPaymentTerms'), LoadingPortId: valI('CmbLoadingPort'), DestinationPortId: valI('CmbDestinationPort'),
            ImporterBankId: valI('CmbImporterBank'), ExporterBankId: valI('CmbExporterbank'), ExportCompanyId: FEAT.multi ? valI('CmbExportCompany') : 0,
            NoOfContainers: valD('txtNoOfContainers'), FcurrencyId: valI('CmbFCYCode'), FCurrencyAmount: valD('txtFCYAmount'), NetWeightKgs: valD('txtMTon'),
            DeliveryRemarks: val('txtDeliveryRemarks'), PaymentRemarks: val('txtPaymentRemarks'), CommodityDetial: val('txtCommodityDetail'), RemarksHeader: val('txtSpecialInstructions'),
            ProductSpecification: val('txtProductSpecification'), ConversionRate: valD('txtExchangeRate'), DeliveryDays: valI('txtDeliveryDays'),
            CustomerApprovalDate: val('ApprovalDate'), CustomerApprovalRemarks: val('txtApprovalRemarks'), FactoryLoadingDate: val('txtFactoryLoadingDate'),
            CustomGroupId: FEAT.sdf ? valI('CmbDocumentCustomGroup') : 0, OnCommission: checked('ChkOnCommission'),
            AttachmentsValues: S.attachmentsValues, CustomAttachmentsValues: S.customAttachmentsValues
        };
    }
    /* Insert(): the confirm, then the service's FormValidation / grid checks / Save; "Record Save Successfully" + Reset + Preview. */
    function insert(btn, recId) {
        if (!ask(recId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return Promise.resolve();
        var body = { recId: recId, allowExportMultiCompanies: FEAT.multi, header: headerBody(), details: S.details, removedDetails: recId > 0 ? S.removed : [],
            otherItems: other.rows, paymentTerms: pay.rows, pmRows: pm.rows, otherCharges: charges.rows };
        var preview = checked('ChkPreview');
        var win = preview && global.CrystalPrint ? global.CrystalPrint.reserve() : null;
        return X.postJson(API + '/save', body).then(function (r) {
            box(r.message || 'Record Save Successfully');
            return reset().then(function () {
                if (preview && global.CrystalPrint) global.CrystalPrint.open('exp-501', { id: netI(r.id) }, null, win);
                else if (win) global.CrystalPrint.release(win);
            });
        }, function (e) { if (win) global.CrystalPrint.release(win); throw e; });
    }
    function btnSave(btn) { return X.busy(btn, function () { S.recId = 0; return insert(btn, 0); }); }
    function btnUpdate(btn) { return X.busy(btn, function () { if (S.recId === 0) throw new Error('RecId not Found'); return insert(btn, S.recId); }); }
    function btnSaveAs(btn) { return X.busy(btn, function () { S.recId = 0; return insert(btn, 0); }); }

    function readById(id, saveAs) {
        return X.getJson(API + '/read?id=' + id).then(function (d) {
            var h = d.header || {};
            S.recId = id; S.saveAs = !!saveAs; S.removed = [];
            show('btnsave', false); show('btnUpdate', true); show('BtnSaveAs', false);
            tabsMain.select('scForm'); tabs2.select('tabContractDetail');
            if (saveAs) setVal('cmbStatus', 1); else { var st = { Open: 1, Complete: 2, Cancel: 3 }[str(h.Status)] || 0; setVal('cmbStatus', st); }
            setText('txtExportSoNo', str(h.LcOrderDocNo)); setText('txtDocDate', h.LcOrderDate); setVal('CmbSalePerson', h.SalesPersonId); setVal('CmbCustomer', h.SupCustId);
            setVal('CmbNotifyParty', h.NotifyPartyId); setText('txtLotRefNo', h.LotReference); setText('txtBuyerOrderNo', h.LcOrderNo); setText('datOrderDate', h.SalesContratDate);
            setText('datShipmentStartDate', h.ShipmentStartDate); setText('txtShipmentDate', h.LastShipmentDate);
            setVal('CmbDeliveryTerm', h.DeliveryTermId); setVal('CmbPaymentTerms', h.PaymentTermId); setVal('CmbLoadingPort', h.LoadingPortId); setVal('CmbDestinationPort', h.DestinationPortId);
            setVal('CmbImporterBank', h.ImporterBankId); setVal('CmbExporterbank', h.ExporterBankId); if (FEAT.multi) setVal('CmbExportCompany', h.ExportCompanyId);
            setText('txtNoOfContainers', str(h.NoOfContainers)); setVal('CmbFCYCode', h.FcurrencyId); setText('txtFCYAmount', fmtFixed(h.FCurrencyAmount, fcyDec())); setText('txtMTon', str(h.NetWeightKgs));
            setText('txtDeliveryRemarks', h.DeliveryRemarks); setText('txtPaymentRemarks', h.PaymentRemarks); setText('txtCommodityDetail', h.CommodityDetial); setText('txtSpecialInstructions', h.RemarksHeader);
            setText('txtProductSpecification', h.ProductSpecification); setText('txtExchangeRate', fmt(h.ConversionRate, 4)); setText('txtDeliveryDays', str(h.DeliveryDays));
            setText('ApprovalDate', h.CustomerApprovalDate); setText('txtApprovalRemarks', h.CustomerApprovalRemarks); setText('txtFactoryLoadingDate', h.FactoryLoadingDate || X.today());
            X.setChecked('ChkOnCommission', netI(h.ContractType) === 1);
            S.customAttachmentsValues = str(h.CustomAttachmentsValues); S.attachmentsValues = str(h.AttachmentsValues);
            if (FEAT.sdf) X.getJson(API + '/custom-groups?customerId=' + netI(h.SupCustId)).then(function (rows) { L.customGroups = rows || []; bind('CmbDocumentCustomGroup', L.customGroups, 'Id', 'name'); setVal('CmbDocumentCustomGroup', h.CustomGroupId); }).catch(function () { /* keep */ });
            S.details = (d.details || []).map(function (r) { if (saveAs) r.Id = 0; return r; });
            other.rows = d.otherItems || []; pay.rows = d.paymentTerms || [];
            pm.rows = (d.pmRows || []).map(function (r) { if (saveAs) r.Id = 0; return r; });
            charges.rows = (d.otherCharges || []).map(function (r) { if (saveAs) r.Id = 0; return r; });
            detailGrid.draw(S.details); other.draw(); pay.draw(); pm.draw(); charges.draw();
            brandsFromDetails();
            $id('scStatusInfo').textContent = 'Record ' + id + (saveAs ? ' (Save As)' : '') + ' - ' + str(h.Status);
            $id('scFooterInfo').textContent = 'Contract ' + str(h.LcOrderDocNo) + ' / ' + str(h.LcOrderNo);
        });
    }
    function reset() {
        S.recId = 0; S.saveAs = false; S.removed = []; S.details = [];
        X.setChecked('ChkOnCommission', false);
        ['CmbSalePerson', 'CmbCustomer', 'CmbNotifyParty', 'CmbFCYCode', 'CmbDeliveryTerm', 'CmbPaymentTerms', 'CmbLoadingPort', 'CmbDestinationPort', 'CmbImporterBank', 'CmbExporterbank', 'cmbStatus', 'CmbDocumentCustomGroup'].forEach(function (id) { setVal(id, 0); });
        ['txtPaymentRemarks', 'txtDeliveryRemarks', 'txtFCYAmount', 'txtSpecialInstructions', 'txtMTon', 'txtCommodityDetail', 'txtBuyerOrderNo', 'txtLotRefNo', 'txtNoOfContainers', 'txtProductSpecification', 'txtExchangeRate', 'txtDeliveryDays', 'txtApprovalRemarks'].forEach(function (id) { setText(id, ''); });
        resetDetail(); other.rows = []; other.reset(); pay.rows = []; pay.reset(); pm.rows = []; pm.reset(); pm.bindBrands([]); charges.rows = []; charges.reset();
        detailGrid.draw([]); other.draw(); pay.draw(); pm.draw(); charges.draw();
        var dd = val('txtDocDate') > X.today() ? val('txtDocDate') : X.today();
        setText('datShipmentStartDate', dd); setText('txtShipmentDate', dd);
        show('btnsave', true); show('btnUpdate', false); show('BtnSaveAs', false);
        $id('scStatusInfo').textContent = ''; $id('scFooterInfo').textContent = '';
        tabs2.select('tabContractDetail'); focus('txtDocDate');
        return Promise.all([
            X.getJson(API + '/generate-code').then(function (r) { setText('txtExportSoNo', str(r.docNo)); }, function () { setText('txtExportSoNo', '0'); }),
            X.getJson(API + '/last-saved').then(function (r) { L.lastSaved = r.lastSaved; applyLastSaved(); }, function () { /* keep */ })
        ]);
    }
    function btnNew() { reset(); }
    /* toolStripButton1_Click (Refresh / Ctrl+R): every combo again. */
    function btnRefresh(btn) { return X.busy(btn, function () { return X.getJson(API + '/refresh').then(function (d) { applyLists(d, false); }); }); }

    // ------------------------------------------------------------------------------ prints / popups

    function print501(btn) {
        if (S.recId <= 0) { box('No Record found'); return; }
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
        return global.CrystalPrint.open('exp-501', { id: S.recId }, btn);
    }
    function print501A(btn) {
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
        return global.CrystalPrint.open('exp-501a', { id: S.recId }, btn);
    }
    var POPUPS = {
        deliveryTerm: 'The Delivery Term define popup (frmDeliveryTerm) is not ported; define delivery terms in Master Data, then Refresh.',
        paymentTerm: 'The Payment Term define popup (frmPaymentTerms) is not ported; define payment terms in Master Data, then Refresh.',
        seaPort: 'The Sea Port define popup (frmSeaPort) is not ported; define sea ports in Master Data, then Refresh.',
        commodity: 'The Commodity Detail form (frmCommodityDetailItemCustomerWise) is not ported.'
    };
    function popup(key) {
        if (key === 'pmDetail') { global.open('/export/packing-detail-by-contract' + (S.recId > 0 ? '?id=' + S.recId : ''), '_blank'); return; }
        box(POPUPS[key] || 'Not available.');
    }
    function autoMap() { brandsFromDetails(); pm.autoMap(L.itemPmMap); }
    function shortcuts() {
        X.shortcuts([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
            ['Ctrl+F1', 'For Open Delivery Term Popup'], ['Ctrl+F2', 'For Open Payterm Popup'], ['Ctrl+F3', 'For Open Sea Ports Popup'], ['Ctrl+F5', 'For Focus on Doc Date'],
            ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+L', 'For Load All Records'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
            ['Ctrl+ArrowRight', 'For Moving In Detail Grids'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On First Column in Detail for Entry'],
            ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', 'When Focus On Any Grid To Call Function\'s On Button Or Link']]);
    }

    // ------------------------------------------------------------------------------ history

    function historyShow(btn) {
        return X.busy(btn, function () {
            var body = X.contractHistoryBody({ fromChecked: 'chkFromDateHistory', fromDate: 'FromDateHistory', toChecked: 'chkToDateHistory', toDate: 'ToDateHistory', dateBy: 'scHistDateBy', fromDocNo: 'FromDocNoHistory', toDocNo: 'ToDocNoHistory', customer: 'CmbCustomerHistory' });
            return X.postJson(API + '/history', body).then(function (rows) {
                HIST = rows || []; histGrid.current = -1; histGrid.draw(HIST);
                hDetail.draw([]); hPay.draw([]); hPm.draw([]); hOther.draw([]); hCharges.draw([]);
                if (HIST.length === 0) box('No record found');
            });
        });
    }
    function historyNew() {
        var days = netI(CFG.defaultDaysToLessFromHistoryFromDate);
        setText('FromDateHistory', X.daysAgo(days > 0 ? days : 3)); setText('ToDateHistory', X.today());
        setText('FromDocNoHistory', ''); setText('ToDocNoHistory', ''); setVal('CmbCustomerHistory', 0); X.setChecked('drdocdate', true);
        HIST = []; histGrid.draw([]); hDetail.draw([]); hPay.draw([]); hPm.draw([]); hOther.draw([]); hCharges.draw([]);
    }
    function historyRefresh(btn) { return X.busy(btn, function () { return X.getJson(API + '/history-combos').then(function (d) { L.historyCustomers = d.historyCustomers || []; bind('CmbCustomerHistory', L.historyCustomers, 'Id', 'name'); }); }); }
    /* DataGridHistory_SelectionChanged: GetDetailGrdByHeadId. */
    function historySelected(i) {
        var r = HIST[i]; if (!r) return;
        X.getJson(API + '/read?id=' + netI(r.Id)).then(function (d) {
            hDetail.draw(d.details || []); hPay.draw(d.paymentTerms || []); hPm.draw(d.pmRows || []); hOther.draw(d.otherItems || []); hCharges.draw(d.otherCharges || []);
        }).catch(function (e) { box(e.message); });
    }
    function historyButton(i, key, btn) {
        var r = HIST[i]; if (!r) return;
        var id = netI(r.Id);
        if (key === 'Edit') return readById(id, false).catch(function (e) { box(e.message); });
        if (key === 'SaveAs') {
            return readById(id, true).then(function () {
                setText('txtBuyerOrderNo', ''); var t = X.today();
                setText('datShipmentStartDate', t); setText('txtDocDate', t); setText('datOrderDate', t); setText('ApprovalDate', t);
                S.recId = 0;
                show('BtnSaveAs', true); show('btnsave', false); show('btnUpdate', false);
            }).catch(function (e) { box(e.message); });
        }
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
        var map = { Print: 'exp-501', Print501A: 'exp-501a', PrintII: 'exp-501new', Print523: 'exp-523' };
        if (map[key]) global.CrystalPrint.open(map[key], { id: id }, btn);
    }
    function toggleHistory() { if (tabsMain.current() === 'scHistory') tabsMain.select('scForm'); else tabsMain.select('scHistory'); }

    // ------------------------------------------------------------------------------ wiring

    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }
    function wire() {
        tabsMain = X.tabs('sc', ['scForm', 'scHistory'], function (p) { if (p === 'scHistory') { tabsH.select('tabDetailH'); focus('FromDateHistory'); } else focus('txtDocDate'); });
        tabs2 = X.tabs('sc2', ['tabContractDetail', 'tabPaymentTerm', 'tabPackingMaterialDetail', 'tabOtherItems', 'tabOtherCharges'], function (p) {
            if (p === 'tabPackingMaterialDetail') { brandsFromDetails(); focus('cmbBrandForPmEntry'); }
        });
        tabsH = X.tabs('scH', ['tabDetailH', 'tabPaymentDetailH', 'tabPmH', 'tabOtherItemH', 'tabOtherChargesHistory']);
        X.initFullscreen();
        pm.wire(); pay.wire(); other.wire(); charges.wire();
        on('txtExportSoNo', 'change', soNoLeave);
        on('CmbCustomer', 'change', customerLeave);
        on('CmbDeliveryTerm', 'change', function () { termDesc('CmbDeliveryTerm', 'deliveryTerms', 'txtDeliveryRemarks'); });
        on('CmbPaymentTerms', 'change', function () { termDesc('CmbPaymentTerms', 'paymentTerms', 'txtPaymentRemarks'); });
        on('datShipmentStartDate', 'change', shipmentStartChanged);
        on('txtDocDate', 'change', shipmentStartChanged);
        on('txtShipmentDate', 'change', shipmentEndChanged);
        on('txtDeliveryDays', 'input', calcShipmentDate);
        on('datOrderDate', 'change', function () { setText('ApprovalDate', val('datOrderDate')); });
        on('txtFactoryLoadingDate', 'change', factoryLoadingChanged);
        on('combitem', 'change', function () { itemLeave(0); });
        on('cmbcomditiyDetailByItemCus', 'change', function () { setText('txtCommodityDetailIndetail', valI('cmbcomditiyDetailByItemCus') > 0 ? text('cmbcomditiyDetailByItemCus') : ''); });
        ['rdSearchByCode', 'rdSearchByName'].forEach(function (id) { on(id, 'change', bindItems); });
        on('txtQtyMTon', 'input', function () { calcWeight(); calcAmount(); });
        on('txtNoOfBags', 'input', function () { calcWeight(); calcAmount(); });
        on('txtCostMTon', 'input', function () { calcWeight(); calcAmount(); });
        on('combitempck', 'change', function () { calcWeight(); calcAmount(); });
        on('combrateuom', 'change', function () { calcWeight(); calcAmount(); });
        on('btnplus', 'click', addToGrid); on('btnUpdateDetail', 'click', updateDetail); on('btnCancelUpdateDetial', 'click', function () { resetDetail(); });
        ['txtQtyMTon', 'txtNoOfBags', 'txtCostMTon', 'txtExchangeRate', 'FromDocNoHistory', 'ToDocNoHistory'].forEach(function (id) { on(id, 'keypress', X.numericOnly); });
        ['txtDeliveryDays', 'txtNoOfContainers', 'txtExportSoNo'].forEach(function (id) { on(id, 'keypress', X.integerOnly); });
        document.addEventListener('keydown', function (e) {
            var inForm = tabsMain.current() === 'scForm';
            if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'button') {
                var f = Array.prototype.filter.call(document.querySelectorAll('input,select,textarea,button'), function (x) { return !x.disabled && x.tabIndex >= 0 && x.offsetParent !== null; });
                var i = f.indexOf(e.target); if (i >= 0 && i < f.length - 1) { e.preventDefault(); f[i + 1].focus(); }
                return;
            }
            if (!e.ctrlKey) { if (e.key === 'Escape' && !document.querySelector('.ex-fullscreen')) X.cancelWindow(); return; }
            var k = e.key.toLowerCase();
            if (k === 'e') { e.preventDefault(); X.cancelWindow(); }
            else if (k === 't') { e.preventDefault(); if (!inForm) { tabsMain.select('scForm'); tabs2.select('tabContractDetail'); } else tabsMain.select('scHistory'); }
            else if (k === 'n') { e.preventDefault(); if (inForm) btnNew(); else historyNew(); }
            else if (k === 'r') { e.preventDefault(); if (inForm) btnRefresh($id('BtnRefresh')); else historyRefresh($id('btnRefreshHistory')); }
            else if (inForm && k === 's' && !$id('btnsave').classList.contains('is-hidden')) { e.preventDefault(); btnSave($id('btnsave')); }
            else if (inForm && k === 'u' && !$id('btnUpdate').classList.contains('is-hidden') && S.recId > 0) { e.preventDefault(); btnUpdate($id('btnUpdate')); }
            else if (inForm && (k === 'p' || k === '1')) { e.preventDefault(); print501($id('Print')); }
            else if (inForm && e.key === 'F1') { e.preventDefault(); popup('deliveryTerm'); }
            else if (inForm && e.key === 'F2') { e.preventDefault(); popup('paymentTerm'); }
            else if (inForm && e.key === 'F3') { e.preventDefault(); popup('seaPort'); }
            else if (inForm && e.key === 'F5') { e.preventDefault(); focus('txtDocDate'); }
            else if (inForm && e.key === 'F10') { e.preventDefault(); box('Attachments (DMS) are not available in the web port.'); }
            else if (!inForm && k === 'l') { e.preventDefault(); historyShow($id('btnShowHistory')); }
            else if (inForm && e.key === 'ArrowDown') { e.preventDefault(); var g = $id('grdDetails'); if (g) g.focus(); }
            else if (inForm && e.key === 'ArrowUp') { e.preventDefault(); focus('combitem'); }
            else if (inForm && e.key === 'ArrowRight') { e.preventDefault(); var order = ['tabContractDetail', 'tabPaymentTerm', 'tabPackingMaterialDetail', 'tabOtherItems', 'tabOtherCharges']; tabs2.select(order[(order.indexOf(tabs2.current()) + 1) % order.length]); }
            else if (e.altKey) { e.preventDefault(); shortcuts(); }
        });
        setup().then(function () {
            var q = /[?&]id=(\d+)/.exec(location.search);
            if (q) readById(netI(q[1]), false).catch(function (e) { box(e.message); });
        }).catch(function (e) { box(e.message); });
    }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', wire); else wire();

    global.ExportSalesContract = { btnNew: btnNew, btnRefresh: btnRefresh, btnSave: btnSave, btnUpdate: btnUpdate, btnSaveAs: btnSaveAs, print501: print501, print501A: print501A,
        popup: popup, autoMap: autoMap, shortcuts: shortcuts, historyShow: historyShow, historyNew: historyNew, historyRefresh: historyRefresh, toggleHistory: toggleHistory, readById: readById };
})(window);
