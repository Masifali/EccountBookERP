/* ============================================================================================
 * countx_export_contract_iii.js - frmExportContractIII.cs (Architecture.WinApp.Export), screen 879
 * "Export Contract (III)", DocumentTypeId 223. Form | History. Every button, Leave/TextChanged, grid
 * double-click, X button, history button and shortcut of the desktop form has its counterpart here;
 * data comes from /api/export/contract-iii (ExportContractIIIController -> ExportContractIIIService).
 * Prints: exp-501-sub (501-Print / Preview / history Print), exp-501a (523A-Print / history Print501A),
 * exp-501new (history PrintII), exp-501b (history PrintIII), exp-523 (history Print523).
 * ============================================================================================ */
(function (global) {
    'use strict';

    var X = global.ExportG;
    var API = '/api/export/contract-iii';
    var $id = X.$id, box = X.box, ask = X.ask, str = X.str, netI = X.netI, netD = X.netD, fmt = X.fmt, fmtFixed = X.fmtFixed, roundAway = X.roundAway;
    var val = X.val, valI = X.valI, valD = X.valD, setVal = X.setVal, setText = X.setText, text = X.text, bind = X.bind, show = X.show, focus = X.focus, checked = X.checked;

    var PERM = { Save: true, Update: true, Delete: true, Print: true };
    var CFG = { allItemsBindonExportContract: false, pmGridInfoShouldNotCompulsoryOnContract: false, defaultDaysToLessFromHistoryFromDate: 0, itemSearchByCode: false, specialInstructionsForExportContract: '', defaultCropYearId: 0, defaultPackingTypeId: 0, fcyDecimals: 2 };
    var L = { contractNos: [], customers: [], salesPersons: [], subParties: [], banks: { importer: [], exporter: [] }, ports: [], deliveryTerms: [], currencies: [], customGroups: [], items: [], pmItems: [],
        packTypes: [], brands: [], cropYears: [], commodity: [], pmDetail: [], farmingNTrade: [], paymentTerms: [], exportCharges: [], exportCompanies: [], lastSaved: null, historyCustomers: [] };
    var FEAT = { multi: false, sdf: false };
    var S = { recId: 0, details: [], removed: [], updateIndex: -1, uoms: [], attachmentsValues: '', customAttachmentsValues: '' };
    var HIST = [];
    var tabsMain, tabs2, tabsH;
    function fcyDec() { return CFG.fcyDecimals > 0 ? CFG.fcyDecimals : 2; }

    // ------------------------------------------------------------------------------ grids / shared tabs

    var detailGrid = X.grid('grdDetails', [
        { key: 'ItemCode' }, { key: 'ItemName' }, { key: 'PackType' }, { key: 'PmItemCode' }, { key: 'PmItemName' }, { key: 'BrandCode' }, { key: 'BrandName' }, { key: 'CropYear' },
        { key: 'QtyMTon', num: true, dec: 6 }, { key: 'OuterUom' }, { key: 'OuterQty', num: true }, { key: 'InnerQty', num: true }, { key: 'InnerUom' }, { key: 'PalletQty', num: true },
        { key: 'Rate', num: true, dec: 4 }, { key: 'RateUOM' }, { key: 'Amount', num: true, dec: 4 }, { key: 'OtherItemAmount', num: true, dec: 4 },
        { key: 'CommodityDetail' }, { key: 'PackingDetail' }, { key: 'FarmingNTrade' }, { key: 'Remarks' }
    ], { withX: true, totals: ['QtyMTon', 'OuterQty', 'InnerQty', 'PalletQty', 'Amount', 'OtherItemAmount'], onDelete: deleteDetail, onDblClick: editDetail });

    var pm = X.pmTab({
        ids: { brand: 'CmbBrandPmDetail', brandUom: 'CmbBrandUomPmDetail', brandPackType: 'CmbBrandPackingTypePmDetail', brandOuter: 'txtBrandOuterQtyPmDetail', brandInner: 'txtBrandInnerQtyPmDetail',
            pmItem: 'CmbItemPmDetail', pmUom: 'CmbUomPmDetail', pmOuter: 'txtOuterQtyPmDetail', pmInner: 'txtInnerQtyPmDetail', rate: 'txtRatePmDetail', amount: 'txtAmountPmDetail', remarks: 'txtRemarksPmDetail',
            add: 'btnPackingAdd', update: 'btnPackingUpdate', cancel: 'btnPackingCancel', grid: 'grdPackingMaterialDetail',
            brandByCode: 'radBrandName', brandByName: 'rabBrandName', pmByCode: 'rdPackingItemCode', pmByName: 'rdPackingItemName' },
        api: { lastRate: API + '/last-rate?itemId=', pmUoms: API + '/global-uoms?itemId=' },
        fcyDecimals: fcyDec, messagesIII: true
    });
    var pay = X.paymentTab({
        ids: { term: 'CmbPaymentTermPT', pct: 'txtPercntOfTotalPT', amount: 'txtlFcyAmountPT', remarks: 'txtRemarksPT', add: 'btnAddPayment', update: 'btnUpdatePayment', cancel: 'btnCancelPayment', grid: 'grdPaymentTerm' },
        fcyDecimals: fcyDec, headerFcyAmount: function () { return val('txtFCYAmount'); }, terms: function () { return L.paymentTerms; }, messagesIII: true
    });
    var other = X.otherItemsTab({
        ids: { item: 'CmbItemOtherDetail', qty: 'txtQtyOtherDetail', rate: 'txtRateOtherDetail', amount: 'txtAmountOtherDetail', remarks: 'txtRemarksOtherDetail', add: 'btnaddOtherDetail', update: 'btnUpdateOtherDetail', cancel: 'btnCancelOtherDetail', grid: 'grdotheritems', byCode: 'RadItemCodeOtherDetail', byName: 'RadItemNameOtherDetail' },
        onChanged: function () { headerTotals(); proportionate(); }, messagesIII: true
    });
    var charges = X.otherChargesTab({
        ids: { item: 'CmbChargesNameChargeDetail', add: 'txtAddAmountChargeDetail', less: 'txtLessAmountChargeDetail', remarks: 'txtRemarksChargeDetail', addBtn: 'BtnAddChargeDetail', update: 'BtnUpdateChargeDetail', cancel: 'BtnCancelChargeDetail', grid: 'grdOtherChargesDetail' }
    });
    var histGrid = X.grid('DataGridHistory', [
        { key: 'OrderNo' }, { key: 'OrderDate', date: true }, { key: 'CustomerName' }, { key: 'CustomerOrderNo' }, { key: 'DocDate', date: true }, { key: 'LotReference' },
        { key: 'MTons', num: true }, { key: 'NoOfContainers', num: true, dec: 0 }, { key: 'FcyCode' }, { key: 'FcyAmount', num: true, dec: 2 }, { key: 'LastShipmentDate', date: true },
        { key: 'NotifyParty' }, { key: 'DeliveryTerm' }, { key: 'PaymentTerm' }, { key: 'LoadingPort' }, { key: 'DestinationPort' }, { key: 'Commodity_Description' },
        { key: 'EntryUser' }, { key: 'EntryDate', datetime: true }, { key: 'ModifyUser' }, { key: 'ModifyDate', datetime: true }, { key: 'NoOfAttachments', num: true, dec: 0, link: true }
    ], {
        buttons: [{ key: 'Print', text: 'Print' }, { key: 'Print501A', text: 'Print501A' }, { key: 'PrintII', text: 'PrintII' }, { key: 'PrintIII', text: 'PrintIII' }, { key: 'Print523', text: 'Print523' }, { key: 'Edit', text: 'Edit' }, { key: 'SaveAs', text: 'SaveAs' }],
        totals: ['MTons', 'NoOfContainers', 'FcyAmount'], emptyId: 'c3HistEmpty',
        onButton: historyButton, onDblClick: function (i) { historyButton(i, 'Edit'); }, onSelect: historySelected,
        onLink: function () { box('Attachments (DMS) are not available in the web port.'); }
    });
    var hDetail = X.grid('grddetailhistory', [
        { key: 'ItemCode' }, { key: 'ItemName' }, { key: 'PackType' }, { key: 'PmItemName' }, { key: 'BrandName' }, { key: 'CropYear' }, { key: 'QtyMTon', num: true, dec: 6 }, { key: 'OuterUom' },
        { key: 'OuterQty', num: true }, { key: 'InnerQty', num: true }, { key: 'InnerUom' }, { key: 'PalletQty', num: true }, { key: 'Rate', num: true, dec: 4 }, { key: 'RateUOM' }, { key: 'Amount', num: true, dec: 4 },
        { key: 'CommodityDetail' }, { key: 'PackingDetail' }, { key: 'FarmingNTrade' }, { key: 'Remarks' }], { totals: ['QtyMTon', 'OuterQty', 'Amount'] });
    var hPay = X.grid('grdPaymentHistory', X.HIST_COLS.payment, { totals: ['PrcntOfTotal', 'FcyAmount'] });
    var hPm = X.grid('grdPMHistory', X.HIST_COLS.pm, { totals: ['PmItemOuterQty', 'PmItemInnerQty', 'Amount'] });
    var hOther = X.grid('grdOtherItemHistory', X.HIST_COLS.other, { totals: ['Amount'] });
    var hCharges = X.grid('grdOtherChargesHistory', X.HIST_COLS.charges, { totals: ['AddAmount', 'LessAmount'] });

    /* ReadById / detail rows arrive with the 209 column names; the III grid carries Outer* names. */
    function toIII(r) {
        r.OuterQty = r.OuterQty !== undefined ? r.OuterQty : r.NoOfBags; r.OuterUomId = r.OuterUomId !== undefined ? r.OuterUomId : r.PackSizeId;
        r.OuterUom = r.OuterUom !== undefined ? r.OuterUom : r.PackSize; r.OuterQtyEquivalent = r.OuterQtyEquivalent !== undefined ? r.OuterQtyEquivalent : r.QtyEquivalent;
        r.QtyEquivalent = r.OuterQtyEquivalent;
        return r;
    }

    // ------------------------------------------------------------------------------ load

    function applyLists(d, initial) {
        FEAT.multi = !!d.allowExportMultiCompanies; FEAT.sdf = !!d.shipmentDocumentsFeature;
        if (d.config) CFG = d.config;
        ['contractNos', 'ports', 'deliveryTerms', 'currencies', 'customGroups', 'packTypes', 'brands', 'cropYears', 'farmingNTrade', 'paymentTerms', 'exportCharges', 'exportCompanies', 'historyCustomers'].forEach(function (k) { if (d[k] !== undefined) L[k] = d[k] || []; });
        if (d.parties) { L.customers = d.parties.customers || []; L.salesPersons = d.parties.salesPersons || []; L.subParties = d.parties.subParties || []; }
        if (d.banks) L.banks = d.banks;
        if (d.items) { L.items = d.items.items || []; L.pmItems = d.items.pmItems || []; }
        if (d.commodityRemarks) { L.commodity = d.commodityRemarks.commodity || []; L.pmDetail = d.commodityRemarks.pmDetail || []; }
        if (d.lastSaved !== undefined) L.lastSaved = d.lastSaved;
        show('exportCompanyWrap', FEAT.multi); if (FEAT.multi) bind('CmbExportCompany', L.exportCompanies, 'Id', 'name');
        bind('CmbContractNo', L.contractNos, 'Id', 'name');
        bind('CmbSalePerson', L.salesPersons, 'Id', 'name'); bind('CmbCustomer', L.customers, 'Id', 'name'); subParties();
        bind('CmbImporterBank', L.banks.importer, 'Id', 'name'); bind('CmbExporterbank', L.banks.exporter, 'Id', 'name');
        bind('CmbLoadingPort', L.ports, 'Id', 'name'); bind('CmbDestinationPort', L.ports, 'Id', 'name');
        bind('CmbDeliveryTerm', L.deliveryTerms, 'Id', 'name'); bind('CmbFCYCode', L.currencies, 'Id', 'name');
        show('dcgWrap', FEAT.sdf); if (FEAT.sdf) bind('CmbDocumentCustomGroup', L.customGroups, 'Id', 'name');
        categoryBind(); itemsBind(); bind('CmbPackingTypeInDetail', L.packTypes, 'Id', 'name'); pmItemsInDetailBind(); brandsBind(); bind('CmbCropYearInDetail', L.cropYears, 'Id', 'name');
        bind('CmbComditiyDetailByItemCusInDetail', L.commodity, 'Id', 'name'); bind('CmbPmDetailByItemCusInDetail', L.pmDetail, 'Id', 'name');
        bind('CmbFarmingNTradeInDetail', L.farmingNTrade, 'Id', 'name'); bind('CmbPaymentTermPT', L.paymentTerms, 'Id', 'name');
        other.bindItems(L.pmItems); pm.bindPmItems(L.pmItems);
        bind('CmbChargesNameChargeDetail', L.exportCharges, 'Id', 'name');
        if (d.historyCustomers) bind('CmbCustomerHistory', L.historyCustomers, 'Id', 'name');
        if (initial) {
            X.setChecked(CFG.itemSearchByCode ? 'rdSearchByCode' : 'rdSearchByName', true);
            X.setChecked(CFG.itemSearchByCode ? 'rdPackingItemCode' : 'rdPackingItemName', true);
            itemsBind(); pm.bindPmItems(L.pmItems);
            var days = netI(CFG.defaultDaysToLessFromHistoryFromDate);
            setText('FromDateHistory', X.daysAgo(days > 0 ? days : 3)); setText('ToDateHistory', X.today());
        }
        configDefaults(); applyLastSaved();
    }
    /* GetConfigurationsFromGlobalAndBindValuesInColumns */
    function configDefaults() {
        if (netI(CFG.defaultCropYearId) !== 0) setVal('CmbCropYearInDetail', CFG.defaultCropYearId);
        if (netI(CFG.defaultPackingTypeId) !== 0) setVal('CmbPackingTypeInDetail', CFG.defaultPackingTypeId);
        if (str(CFG.specialInstructionsForExportContract).trim() && !val('txtSpecialInstructions').trim()) setText('txtSpecialInstructions', CFG.specialInstructionsForExportContract);
    }
    function applyLastSaved() {
        if (valI('CmbSalePerson') === 0 && L.lastSaved) {
            setVal('CmbSalePerson', L.lastSaved.SalesPersonId); setVal('CmbFCYCode', L.lastSaved.FcurrencyId);
            setVal('CmbCropYearInDetail', L.lastSaved.CropYearId); setVal('CmbPackingTypeInDetail', L.lastSaved.InvPackingMaterialTypeId);
        }
    }
    /* ItemCategoryOrTypeBind: distinct Item Category / Item Type of the item list. */
    function categoryBind() {
        var byCat = checked('RadCategory'), seen = {}, rows = [];
        L.items.forEach(function (r) {
            var id = byCat ? r.ItemCategoryId : r.ItemTypeId, nm = byCat ? r.ItemCategory : r.ItemType;
            if (!str(nm) || seen[nm]) return; seen[nm] = true; rows.push({ Id: id, name: nm });
        });
        bind('CmbCategoryInDetail', rows, 'Id', 'name');
    }
    /* ItemDtsFillFromGlobal(CategoryOrTypeId) + ItemNameBind */
    function itemsBind() {
        var byCode = checked('rdSearchByCode'), byCat = checked('RadCategory'), f = valI('CmbCategoryInDetail');
        var rows = L.items.filter(function (r) { return f === 0 || (byCat ? netI(r.ItemCategoryId) === f : netI(r.ItemTypeId) === f); });
        bind('CmbItemNameInDetail', rows.map(function (r) { return { Id: r.Id, name: byCode ? r.ItemCode : r.ItemName }; }), 'Id', 'name');
    }
    function pmItemsInDetailBind() {
        var byCode = checked('RadPmItemCodeInDetail');
        bind('CmbPmItemInDetail', L.pmItems.map(function (r) { return { Id: r.Id, name: byCode ? r.ItemCode : r.ItemName }; }), 'Id', 'name');
    }
    function brandsBind() {
        var byCode = checked('RadBrandCodeInDetail');
        bind('CmbBrandInDetail', L.brands.map(function (r) { return { Id: r.Id, name: byCode ? r.BrandCode : r.BrandName }; }), 'Id', 'name');
    }
    /* ConsigneeAgainstCustomerBind: the customer's sub parties into Consignee / Notify Party 1-3. */
    function subParties() {
        var c = valI('CmbCustomer');
        var rows = c > 0 ? L.subParties.filter(function (r) { return netI(r.ParentsSupCustId) === c; }) : [];
        ['CmbConsignee', 'CmbNotifyParty1', 'CmbNotifyParty2', 'CmbNotifyParty3'].forEach(function (id) { bind(id, rows, 'Id', 'name'); });
    }
    function setup() {
        return X.getJson(API + '/setup').then(function (d) {
            PERM = d.permissions || PERM;
            X.setEnabled('btnsave', !!PERM.Save); X.setEnabled('BtnSaveAs', !!PERM.Save); X.setEnabled('btnUpdate', !!PERM.Update); X.setEnabled('Print', !!PERM.Print);
            var t = X.today();
            ['txtDocDate', 'txtCustomerOrderDate', 'ApprovalDate', 'datShipmentStartDate', 'txtShipmentDate', 'txtFactoryLoadingDate', 'txtProductionScheduleDate', 'txtInspectionScheduleDate', 'txtPMScheduleDate'].forEach(function (id) { setText(id, t); });
            applyLists(d, true);
            focus('txtDocDate');
        });
    }

    // ------------------------------------------------------------------------------ header events

    function customerLeave() {
        commodityBind();
        if (FEAT.sdf) X.getJson(API + '/custom-groups?customerId=' + valI('CmbCustomer')).then(function (rows) { L.customGroups = rows || []; bind('CmbDocumentCustomGroup', L.customGroups, 'Id', 'name'); }).catch(function (e) { box(e.message); });
        subParties();
    }
    function commodityBind() {
        return X.getJson(API + '/commodity-remarks?itemId=' + valI('CmbItemNameInDetail')).then(function (d) {
            L.commodity = d.commodity || []; L.pmDetail = d.pmDetail || [];
            bind('CmbComditiyDetailByItemCusInDetail', L.commodity, 'Id', 'name'); bind('CmbPmDetailByItemCusInDetail', L.pmDetail, 'Id', 'name');
        }).catch(function (e) { box(e.message); });
    }
    function termDesc() { if (valI('CmbDeliveryTerm') === 0) return; var r = X.rowOf(L.deliveryTerms, 'Id', val('CmbDeliveryTerm')); if (r && str(r.desc)) setText('txtDeliveryRemarks', r.desc); }
    /* CmbFCYCode_TextChanged: the currency's exchange rate when the box is empty / 0. */
    function fcyChanged() {
        var r = X.rowOf(L.currencies, 'Id', val('CmbFCYCode')), rate = r && valI('CmbFCYCode') > 0 ? netD(r.ExchangeRate) : 0;
        if (!val('txtExchangeRate') || valD('txtExchangeRate') === 0) setText('txtExchangeRate', fmt(rate, 2));
    }
    function shipmentStartChanged() {
        if (val('datShipmentStartDate') < val('txtDocDate')) { setText('datShipmentStartDate', val('txtDocDate')); box("Shipment Start Date Can't Be Less Than Doc Date"); }
        calcShipmentDate();
    }
    function calcShipmentDate() { var days = valI('txtDeliveryDays'); setText('txtShipmentDate', days === 0 ? val('datShipmentStartDate') : X.addDays(val('datShipmentStartDate'), valD('txtDeliveryDays'))); }
    function shipmentEndChanged() { if (val('txtShipmentDate') < val('datShipmentStartDate')) { setText('txtShipmentDate', val('datShipmentStartDate')); box("Shipment End Date Can't Be Less Than Start Date"); } }
    function factoryLoadingChanged() {
        X.getJson(API + '/scheduling-policy').then(function (p) {
            if (!p || p.ProductionInterval === undefined) return;
            var ld = val('txtFactoryLoadingDate');
            setText('txtProductionScheduleDate', X.addDays(ld, -netI(p.ProductionInterval))); setText('txtInspectionScheduleDate', X.addDays(ld, -netI(p.InspectionInterval))); setText('txtPMScheduleDate', X.addDays(ld, -netI(p.PackMaterialInterval)));
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ detail entry

    function uomRow(id) { return X.rowOf(S.uoms, 'Id', id); }
    function itemLeave(detailId) {
        var packT = text('CmbOuterUomInDetail').trim(), innerT = text('CmbInnerUomInDetail').trim(), rateT = text('CmbRateUomInDetail').trim();
        var itemId = valI('CmbItemNameInDetail');
        if (itemId === 0) { S.uoms = []; ['CmbOuterUomInDetail', 'CmbInnerUomInDetail', 'CmbRateUomInDetail'].forEach(function (id) { bind(id, [], 'Id', 'UOMCode'); }); return detailId ? Promise.resolve() : commodityBind(); }
        return X.getJson(API + '/uom-for-export?itemId=' + itemId).then(function (rows) {
            S.uoms = rows || [];
            if (S.uoms.length === 0) { ['CmbOuterUomInDetail', 'CmbInnerUomInDetail', 'CmbRateUomInDetail'].forEach(function (id) { bind(id, [], 'Id', 'UOMCode'); }); return; }
            function keep(id, t) { bind(id, S.uoms, 'Id', 'UOMCode'); var m = t ? S.uoms.filter(function (u) { return str(u.UOMCode) === t; })[0] : null; setVal(id, m ? m.Id : 0); }
            keep('CmbOuterUomInDetail', packT); keep('CmbInnerUomInDetail', innerT); keep('CmbRateUomInDetail', rateT);
            if (detailId) return;
            var sel = text('CmbRateUomInDetail').toUpperCase();
            if (rateT && (sel === '1000KG' || sel === '1000' || valI('CmbRateUomInDetail') !== 0)) return;
            var base = S.uoms.filter(function (u) { return X.netB(u.BaseRateUom); })[0];
            if (base) { setVal('CmbRateUomInDetail', base.Id); return; }
            var eq = S.uoms.filter(function (u) { return netD(u.Equivalent) === 1000; })[0];
            if (eq) setVal('CmbRateUomInDetail', eq.Id);
        }).then(function () { calcWeight(); calcAmount(); if (!detailId) return commodityBind(); }).catch(function (e) { box(e.message); });
    }
    function innerQtyCalc() {
        var u = uomRow(val('CmbInnerUomInDetail')), inner = u ? netD(u.Equivalent) : 0, mton = valD('txtQtyMTonInDetail');
        setText('txtQtyInnerInDetail', fmt(Math.round(mton > 0 && inner > 0 ? mton * 1000 / inner : 0), 2));
    }
    function calcWeight() {
        var mton = valD('txtQtyMTonInDetail'), bags = valD('txtOuterQtyInDetail'), u = uomRow(val('CmbOuterUomInDetail'));
        if (!u) return;
        var outer = netD(u.Equivalent), tag = X.activeTag();
        if (tag === 'MTon') setText('txtOuterQtyInDetail', mton > 0 && outer > 0 ? fmt(Math.round(mton * 1000 / outer), 2) : '0');
        else if (tag === 'NoofBags') setText('txtQtyMTonInDetail', bags > 0 && outer > 0 ? fmt(bags * outer / 1000, 6) : '0');
        else if (mton > 0 && outer > 0) setText('txtOuterQtyInDetail', fmt(Math.round(mton * 1000 / outer), 2));
        innerQtyCalc();
    }
    function calcAmount() {
        setText('txtAmountInDetail', '0');
        var u = uomRow(val('CmbRateUomInDetail'));
        if (u && valI('CmbRateUomInDetail') > 0) {
            var rate = valD('txtRateInDetail'), eq = netD(u.Equivalent), nw = valD('txtQtyMTonInDetail');
            if (rate > 0 && eq > 0 && nw > 0) setText('txtAmountInDetail', fmtFixed(roundAway(rate / eq * (nw * 1000), fcyDec()), fcyDec()));
        }
    }
    /* FormHelper.ValidateControls texts. */
    function detailValidation() {
        var checks = [['CmbItemNameInDetail', 'Item', 'int'], ['CmbPackingTypeInDetail', 'Pack Type', 'int'], ['CmbCropYearInDetail', 'Crop Year', 'int'], ['txtQtyMTonInDetail', 'Qty/M.Ton', 'dbl'],
            ['CmbOuterUomInDetail', 'Outer Uom', 'int'], ['txtOuterQtyInDetail', 'Outer Qty', 'dbl'], ['txtRateInDetail', 'Rate', 'dbl'], ['CmbRateUomInDetail', 'Rate UOM', 'int'], ['txtAmountInDetail', 'Amount', 'dbl']];
        for (var i = 0; i < checks.length; i++) {
            var c = checks[i];
            if (c[2] === 'int' && valI(c[0]) === 0) { box(c[1] + ' field is required'); focus(c[0]); return false; }
            if (c[2] === 'dbl' && valD(c[0]) === 0) { box(c[1] + ' must be a non-zero number'); focus(c[0]); return false; }
        }
        return true;
    }
    function detailRow(id) {
        var it = X.rowOf(L.items, 'Id', val('CmbItemNameInDetail')) || {}, pmi = X.rowOf(L.pmItems, 'Id', val('CmbPmItemInDetail')), br = X.rowOf(L.brands, 'Id', val('CmbBrandInDetail'));
        var f = X.rowOf(L.farmingNTrade, 'Id', val('CmbFarmingNTradeInDetail')), ou = uomRow(val('CmbOuterUomInDetail')) || {}, iu = uomRow(val('CmbInnerUomInDetail')) || {}, ru = uomRow(val('CmbRateUomInDetail')) || {};
        return { Id: id || 0, ItemId: valI('CmbItemNameInDetail'), ItemCode: str(it.ItemCode), ItemName: str(it.ItemName), PackTypeId: valI('CmbPackingTypeInDetail'), PackType: text('CmbPackingTypeInDetail'),
            PmItemId: pmi ? netI(pmi.Id) : 0, PmItemCode: pmi ? str(pmi.ItemCode) : '', PmItemName: pmi ? str(pmi.ItemName) : '',
            BrandId: br ? netI(br.Id) : 0, BrandCode: br ? str(br.BrandCode) : '', BrandName: br ? str(br.BrandName) : '',
            CropYearId: valI('CmbCropYearInDetail'), CropYear: text('CmbCropYearInDetail'), QtyMTon: valD('txtQtyMTonInDetail'),
            OuterUomId: valI('CmbOuterUomInDetail'), OuterUom: text('CmbOuterUomInDetail'), OuterEquivalent: netD(ou.Equivalent), OuterQtyEquivalent: netD(ou.QtyEquivalent), QtyEquivalent: netD(ou.QtyEquivalent),
            OuterQty: valD('txtOuterQtyInDetail'), InnerQty: valD('txtQtyInnerInDetail'), InnerUomId: valI('CmbInnerUomInDetail'), InnerUom: text('CmbInnerUomInDetail'), InnerEquivalent: netD(iu.Equivalent),
            PalletQty: valD('txtPalletQtyInDetail'), Rate: valD('txtRateInDetail'), RateUOMId: valI('CmbRateUomInDetail'), RateUOM: text('CmbRateUomInDetail'), RateEquivalent: netD(ru.Equivalent),
            Amount: valD('txtAmountInDetail'), OtherItemAmount: 0, CommodityDetail: val('txtCommodityDetailInDetail'), PackingDetail: val('txtPmDetailInDetail'),
            ExImFarmingNTradeId: f ? netI(f.Id) : 0, FarmingNTrade: f ? str(f.name) : '', ExImFarmingTypeId: f ? netI(f.ExImFarmingTypeId) : 0, ExImTradeTypeId: f ? netI(f.ExImTradeTypeId) : 0,
            Remarks: val('txtRemarksInDetail') };
    }
    function resetDetail() {
        S.updateIndex = -1; S.uoms = [];
        ['CmbItemNameInDetail', 'CmbPmItemInDetail', 'CmbBrandInDetail', 'CmbFarmingNTradeInDetail', 'CmbComditiyDetailByItemCusInDetail', 'CmbPmDetailByItemCusInDetail'].forEach(function (id) { setVal(id, 0); });
        ['CmbOuterUomInDetail', 'CmbInnerUomInDetail', 'CmbRateUomInDetail'].forEach(function (id) { bind(id, [], 'Id', 'UOMCode'); });
        ['txtQtyMTonInDetail', 'txtOuterQtyInDetail', 'txtQtyInnerInDetail', 'txtPalletQtyInDetail', 'txtRateInDetail', 'txtAmountInDetail'].forEach(function (id) { setText(id, '0'); });
        ['txtCommodityDetailInDetail', 'txtPmDetailInDetail', 'txtRemarksInDetail'].forEach(function (id) { setText(id, ''); });
        configDefaults();
        show('BtnAddInDetail', true); show('btnUpdateInDetail', false); show('btnCancelInDetail', false);
    }
    function addToGrid() { if (!detailValidation()) return; S.details.push(detailRow(0)); drawDetails(); resetDetail(); focus('CmbItemNameInDetail'); }
    function updateDetail() { if (!detailValidation() || S.updateIndex < 0) return; var id = netI(S.details[S.updateIndex].Id); S.details[S.updateIndex] = detailRow(id); drawDetails(); resetDetail(); focus('CmbItemNameInDetail'); }
    function editDetail(i) {
        var r = S.details[i]; if (!r) return;
        setVal('CmbCategoryInDetail', 0); itemsBind();
        S.updateIndex = i;
        setVal('CmbItemNameInDetail', r.ItemId);
        itemLeave(netI(r.Id)).then(function () {
            setVal('CmbPackingTypeInDetail', r.PackTypeId); setVal('CmbPmItemInDetail', r.PmItemId); setVal('CmbBrandInDetail', r.BrandId); setVal('CmbCropYearInDetail', r.CropYearId);
            setText('txtQtyMTonInDetail', str(r.QtyMTon)); setVal('CmbOuterUomInDetail', r.OuterUomId); setText('txtOuterQtyInDetail', str(r.OuterQty)); setText('txtQtyInnerInDetail', str(r.InnerQty));
            setVal('CmbInnerUomInDetail', r.InnerUomId); setText('txtPalletQtyInDetail', str(r.PalletQty)); setText('txtRateInDetail', str(r.Rate)); setVal('CmbRateUomInDetail', r.RateUOMId);
            setText('txtAmountInDetail', str(r.Amount)); setText('txtCommodityDetailInDetail', r.CommodityDetail); setText('txtPmDetailInDetail', r.PackingDetail);
            if (netI(r.ExImFarmingNTradeId) > 0) setVal('CmbFarmingNTradeInDetail', r.ExImFarmingNTradeId);
            setText('txtRemarksInDetail', r.Remarks);
            show('BtnAddInDetail', false); show('btnUpdateInDetail', true); show('btnCancelInDetail', true);
            focus('CmbItemNameInDetail');
        });
    }
    function deleteDetail(i) {
        if (S.updateIndex !== -1) { box('Reset  the Detail first...'); return; }
        var r = S.details[i]; if (!r) return;
        if (netI(r.Id) > 0) { if (!ask('Are you sure to Delete?')) return; S.removed.push(r); }
        S.details.splice(i, 1); drawDetails();
    }
    function drawDetails() { detailGrid.draw(S.details); headerTotals(); proportionate(); }
    function headerTotals() {
        var total = detailGrid.total('Amount'), otherAmt = other.rows.length ? other.grid.total('Amount') : 0;
        setText('txtMTon', str(detailGrid.total('QtyMTon'))); setText('txtFCYAmount', fmtFixed(total + otherAmt, fcyDec()));
    }
    function proportionate() {
        if (S.details.length === 0) return;
        var contract = detailGrid.total('Amount'), otherAmt = other.rows.length ? other.grid.total('Amount') : 0;
        S.details.forEach(function (r) { r.OtherItemAmount = otherAmt > 0 ? otherAmt / contract * netD(r.Amount) : 0; });
        detailGrid.draw(S.details);
    }
    function brandsFromDetails() { pm.bindBrands(pm.brandFromDetails(S.details, 'OuterUomId', 'OuterQty')); }

    // ------------------------------------------------------------------------------ save / read / reset

    function headerBody() {
        return {
            Status: text('cmbStatus'), ContractNoId: valI('CmbContractNo'), LcOrderNo: text('CmbContractNo').trim(), LcOrderDate: val('txtDocDate'), CustomerOrderNo: val('txtCustomerOrderNo').trim(),
            SalesContratDate: val('txtCustomerOrderDate'), SalesPersonId: valI('CmbSalePerson'), SupCustId: valI('CmbCustomer'), ConsigneeId: valI('CmbConsignee'),
            NotifyPartyId: valI('CmbNotifyParty1'), NotifyParty2Id: valI('CmbNotifyParty2'), NotifyParty3Id: valI('CmbNotifyParty3'), LotReference: val('txtLotRefNo'),
            ShipmentStartDate: val('datShipmentStartDate'), LastShipmentDate: val('txtShipmentDate'), DeliveryTermId: valI('CmbDeliveryTerm'), PaymentTermId: 0,
            LoadingPortId: valI('CmbLoadingPort'), DestinationPortId: valI('CmbDestinationPort'), ImporterBankId: valI('CmbImporterBank'), ExporterBankId: valI('CmbExporterbank'),
            ExportCompanyId: FEAT.multi ? valI('CmbExportCompany') : 0, NoOfContainers: valD('txtNoOfContainers'), FcurrencyId: valI('CmbFCYCode'), FCurrencyAmount: valD('txtFCYAmount'), NetWeightKgs: valD('txtMTon'),
            DeliveryRemarks: val('txtDeliveryRemarks'), PaymentRemarks: '', CommodityDetial: val('txtCommodityDetail'), RemarksHeader: val('txtSpecialInstructions'), ProductSpecification: val('txtProductSpecification'),
            ConversionRate: valD('txtExchangeRate'), DeliveryDays: valI('txtDeliveryDays'), CustomerApprovalDate: val('ApprovalDate'), CustomerApprovalRemarks: val('txtApprovalRemarks'),
            FactoryLoadingDate: val('txtFactoryLoadingDate'), CustomGroupId: FEAT.sdf ? valI('CmbDocumentCustomGroup') : 0,
            AttachmentsValues: S.attachmentsValues, CustomAttachmentsValues: S.customAttachmentsValues
        };
    }
    function insert(recId) {
        if (!ask(recId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return Promise.resolve();
        var body = { recId: recId, allowExportMultiCompanies: FEAT.multi, header: headerBody(), details: S.details, removedDetails: recId > 0 ? S.removed : [],
            otherItems: other.rows, paymentTerms: pay.rows, pmRows: pm.rows, otherCharges: charges.rows };
        var preview = checked('ChkPreview'), win = preview && global.CrystalPrint ? global.CrystalPrint.reserve() : null;
        return X.postJson(API + '/save', body).then(function (r) {
            box(r.message || 'Record Save Successfully');
            return reset().then(function () {
                if (preview && global.CrystalPrint) global.CrystalPrint.open('exp-501-sub', { id: netI(r.id) }, null, win);
                else if (win) global.CrystalPrint.release(win);
            });
        }, function (e) { if (win) global.CrystalPrint.release(win); throw e; });
    }
    function btnSave(btn) { return X.busy(btn, function () { S.recId = 0; return insert(0); }); }
    function btnUpdate(btn) { return X.busy(btn, function () { if (S.recId === 0) throw new Error('RecId not Found'); return insert(S.recId); }); }
    function btnSaveAs(btn) { return X.busy(btn, function () { S.recId = 0; return insert(0); }); }

    function readById(id) {
        return X.getJson(API + '/read?id=' + id).then(function (d) {
            var h = d.header || {};
            S.recId = id; S.removed = [];
            show('btnsave', false); show('btnUpdate', true); show('BtnSaveAs', false);
            tabsMain.select('c3Form'); tabs2.select('tabContractDetail');
            setVal('cmbStatus', { Open: 1, Complete: 2, Cancel: 3 }[str(h.Status)] || 0);
            if (FEAT.multi) setVal('CmbExportCompany', h.ExportCompanyId);
            setText('txtDocDate', h.LcOrderDate);
            L.contractNos = d.contractNos || L.contractNos; bind('CmbContractNo', L.contractNos, 'Id', 'name');
            var cn = L.contractNos.filter(function (r) { return str(r.name) === str(h.LcOrderNo); })[0];
            if (cn) setVal('CmbContractNo', cn.Id); else { var s = $id('CmbContractNo'); if (s) { s.insertAdjacentHTML('beforeend', '<option value="-1">' + X.esc(h.LcOrderNo) + '</option>'); s.value = '-1'; X.refreshCombos(); } }
            setText('txtCustomerOrderNo', h.CustomerOrderNo); setText('txtCustomerOrderDate', h.SalesContratDate);
            setVal('CmbSalePerson', h.SalesPersonId); setVal('CmbCustomer', h.SupCustId); subParties();
            setVal('CmbConsignee', h.ConsigneeId); setVal('CmbNotifyParty1', h.NotifyPartyId); setVal('CmbNotifyParty2', h.NotifyParty2Id); setVal('CmbNotifyParty3', h.NotifyParty3Id);
            setText('txtLotRefNo', h.LotReference); setText('datShipmentStartDate', h.ShipmentStartDate); setText('txtShipmentDate', h.LastShipmentDate);
            setVal('CmbDeliveryTerm', h.DeliveryTermId); setVal('CmbLoadingPort', h.LoadingPortId); setVal('CmbDestinationPort', h.DestinationPortId);
            setVal('CmbImporterBank', h.ImporterBankId); setVal('CmbExporterbank', h.ExporterBankId);
            setText('txtNoOfContainers', str(h.NoOfContainers)); setVal('CmbFCYCode', h.FcurrencyId); setText('txtFCYAmount', fmtFixed(h.FCurrencyAmount, fcyDec())); setText('txtMTon', str(h.NetWeightKgs));
            setText('txtDeliveryRemarks', h.DeliveryRemarks); setText('txtCommodityDetail', h.CommodityDetial); setText('txtSpecialInstructions', h.RemarksHeader); setText('txtProductSpecification', h.ProductSpecification);
            setText('txtExchangeRate', fmt(h.ConversionRate, 4)); setText('txtDeliveryDays', str(h.DeliveryDays)); setText('ApprovalDate', h.CustomerApprovalDate); setText('txtApprovalRemarks', h.CustomerApprovalRemarks);
            setText('txtFactoryLoadingDate', h.FactoryLoadingDate || X.today());
            S.customAttachmentsValues = str(h.CustomAttachmentsValues); S.attachmentsValues = str(h.AttachmentsValues);
            if (FEAT.sdf) X.getJson(API + '/custom-groups?customerId=' + netI(h.SupCustId)).then(function (rows) { L.customGroups = rows || []; bind('CmbDocumentCustomGroup', L.customGroups, 'Id', 'name'); setVal('CmbDocumentCustomGroup', h.CustomGroupId); }).catch(function () { /* keep */ });
            S.details = (d.details || []).map(toIII);
            other.rows = d.otherItems || []; pay.rows = d.paymentTerms || []; pm.rows = d.pmRows || []; charges.rows = d.otherCharges || [];
            detailGrid.draw(S.details); other.draw(); pay.draw(); pm.draw(); charges.draw();
            brandsFromDetails();
            $id('c3StatusInfo').textContent = 'Record ' + id + ' - ' + str(h.Status);
            $id('c3FooterInfo').textContent = 'Contract ' + str(h.LcOrderNo);
        });
    }
    function reset() {
        S.recId = 0; S.removed = []; S.details = [];
        ['CmbContractNo', 'CmbSalePerson', 'CmbCustomer', 'CmbConsignee', 'CmbNotifyParty1', 'CmbNotifyParty2', 'CmbNotifyParty3', 'CmbFCYCode', 'CmbDeliveryTerm', 'CmbLoadingPort', 'CmbDestinationPort', 'CmbImporterBank', 'CmbExporterbank', 'cmbStatus', 'CmbDocumentCustomGroup'].forEach(function (id) { setVal(id, 0); });
        ['txtCustomerOrderNo', 'txtDeliveryRemarks', 'txtFCYAmount', 'txtSpecialInstructions', 'txtMTon', 'txtCommodityDetail', 'txtLotRefNo', 'txtNoOfContainers', 'txtProductSpecification', 'txtExchangeRate', 'txtDeliveryDays', 'txtApprovalRemarks'].forEach(function (id) { setText(id, ''); });
        var dd = val('txtDocDate') > X.today() ? val('txtDocDate') : X.today();
        setText('datShipmentStartDate', dd); setText('txtShipmentDate', dd);
        resetDetail(); other.rows = []; other.reset(); pay.rows = []; pay.reset(); pm.rows = []; pm.reset(); pm.bindBrands([]); charges.rows = []; charges.reset();
        detailGrid.draw([]); other.draw(); pay.draw(); pm.draw(); charges.draw();
        show('btnsave', true); show('btnUpdate', false); show('BtnSaveAs', false);
        $id('c3StatusInfo').textContent = ''; $id('c3FooterInfo').textContent = '';
        tabs2.select('tabContractDetail'); focus('txtDocDate');
        configDefaults();
        return Promise.all([
            X.getJson(API + '/contract-nos').then(function (r) { L.contractNos = r.contractNos || []; bind('CmbContractNo', L.contractNos, 'Id', 'name'); }, function () { /* keep */ }),
            X.getJson(API + '/last-saved?documentTypeId=0').then(function (r) { L.lastSaved = r.lastSaved; applyLastSaved(); }, function () { /* keep */ })
        ]);
    }
    function btnNew() { reset(); }
    function btnRefresh(btn) { return X.busy(btn, function () { return X.getJson(API + '/refresh').then(function (d) { applyLists(d, false); }); }); }

    // ------------------------------------------------------------------------------ prints / popups

    function print501(btn) {
        if (S.recId <= 0) { box('No Record found'); return; }
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
        return global.CrystalPrint.open('exp-501-sub', { id: S.recId }, btn);
    }
    function print501A(btn) { if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; } return global.CrystalPrint.open('exp-501a', { id: S.recId }, btn); }
    var POPUPS = {
        generateNos: 'The Generate Contract Nos form (frmGenerateExportContractNos) is not ported; generate the numbers on the desktop, then Refresh.',
        packingType: 'The Packing Type define popup is not ported; define packing types in Master Data, then Refresh.',
        deliveryTerm: 'The Delivery Term define popup is not ported; define delivery terms in Master Data, then Refresh.',
        paymentTerm: 'The Payment Term define popup is not ported; define payment terms in Master Data, then Refresh.',
        seaPort: 'The Sea Port define popup is not ported; define sea ports in Master Data, then Refresh.',
        bank: 'The bank definition popup is not ported; define banks in Master Data, then Refresh.',
        commodity: 'The Commodity Detail form (frmCommodityDetailItemCustomerWise) is not ported.'
    };
    function popup(key) {
        if (key === 'generateNos') { global.open('/export/generate-contract-nos', '_blank'); return; }
        if (key === 'pmDetail') { global.open('/export/packing-detail-by-contract' + (S.recId > 0 ? '?id=' + S.recId : ''), '_blank'); return; }
        box(POPUPS[key] || 'Not available.');
    }
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
            var body = X.contractHistoryBody({ fromChecked: 'chkFromDateHistory', fromDate: 'FromDateHistory', toChecked: 'chkToDateHistory', toDate: 'ToDateHistory', dateBy: 'c3HistDateBy', fromDocNo: 'FromDocNoHistory', toDocNo: 'ToDocNoHistory', customer: 'CmbCustomerHistory' });
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
    function historySelected(i) {
        var r = HIST[i]; if (!r) return;
        X.getJson(API + '/read?id=' + netI(r.Id)).then(function (d) {
            hDetail.draw((d.details || []).map(toIII)); hPay.draw(d.paymentTerms || []); hPm.draw(d.pmRows || []); hOther.draw(d.otherItems || []); hCharges.draw(d.otherCharges || []);
        }).catch(function (e) { box(e.message); });
    }
    function historyButton(i, key, btn) {
        var r = HIST[i]; if (!r) return;
        var id = netI(r.Id);
        if (key === 'Edit') return readById(id).catch(function (e) { box(e.message); });
        if (key === 'SaveAs') {
            /* HistoryGridSaveAsButtonClick: ReadById, RecId = 0, all contract nos, today's dates, Save As visible. */
            return readById(id).then(function () {
                S.recId = 0;
                S.details.forEach(function (d) { d.Id = 0; }); pm.rows.forEach(function (d) { d.Id = 0; }); charges.rows.forEach(function (d) { d.Id = 0; });
                return X.getJson(API + '/contract-nos').then(function (c) { L.contractNos = c.contractNos || []; bind('CmbContractNo', L.contractNos, 'Id', 'name'); }, function () { /* keep */ });
            }).then(function () {
                var t = X.today();
                setText('datShipmentStartDate', t); setText('txtDocDate', t); setText('txtCustomerOrderDate', t); setText('ApprovalDate', t);
                show('BtnSaveAs', true); show('btnsave', false); show('btnUpdate', false);
            }).catch(function (e) { box(e.message); });
        }
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
        var map = { Print: 'exp-501-sub', Print501A: 'exp-501a', PrintII: 'exp-501new', PrintIII: 'exp-501b', Print523: 'exp-523' };
        if (map[key]) global.CrystalPrint.open(map[key], { id: id }, btn);
    }
    function toggleHistory() { if (tabsMain.current() === 'c3History') tabsMain.select('c3Form'); else tabsMain.select('c3History'); }

    // ------------------------------------------------------------------------------ wiring

    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }
    function wire() {
        tabsMain = X.tabs('c3', ['c3Form', 'c3History'], function (p) { if (p === 'c3History') { tabsH.select('tabDetailH'); focus('FromDateHistory'); } else focus('txtDocDate'); });
        tabs2 = X.tabs('c32', ['tabContractDetail', 'tabPaymentTerm', 'tabPackingMaterialDetail', 'tabOtherItems', 'tabOtherCharges'], function (p) {
            if (p === 'tabPackingMaterialDetail') { brandsFromDetails(); focus('CmbBrandPmDetail'); }
        });
        tabsH = X.tabs('c3H', ['tabDetailH', 'tabPaymentDetailH', 'tabPmH', 'tabOtherItemH', 'tabOtherchargesHistory']);
        X.initFullscreen();
        pm.wire(); pay.wire(); other.wire(); charges.wire();
        on('CmbCustomer', 'change', customerLeave);
        on('CmbDeliveryTerm', 'change', termDesc);
        on('CmbFCYCode', 'change', fcyChanged);
        on('datShipmentStartDate', 'change', shipmentStartChanged); on('txtDocDate', 'change', shipmentStartChanged);
        on('txtShipmentDate', 'change', shipmentEndChanged); on('txtDeliveryDays', 'input', calcShipmentDate);
        on('txtCustomerOrderDate', 'change', function () { setText('ApprovalDate', val('txtCustomerOrderDate')); });
        on('txtFactoryLoadingDate', 'change', factoryLoadingChanged);
        ['RadCategory', 'RadType'].forEach(function (id) { on(id, 'change', function () { categoryBind(); itemsBind(); }); });
        on('CmbCategoryInDetail', 'change', itemsBind);
        ['rdSearchByCode', 'rdSearchByName'].forEach(function (id) { on(id, 'change', itemsBind); });
        ['RadPmItemCodeInDetail', 'RadPmItemNameInDetail'].forEach(function (id) { on(id, 'change', pmItemsInDetailBind); });
        ['RadBrandCodeInDetail', 'RadBrandNameInDetail'].forEach(function (id) { on(id, 'change', brandsBind); });
        on('CmbItemNameInDetail', 'change', function () { itemLeave(0); });
        on('CmbComditiyDetailByItemCusInDetail', 'change', function () { if (!val('txtCommodityDetailInDetail').trim() && valI('CmbComditiyDetailByItemCusInDetail') > 0) setText('txtCommodityDetailInDetail', text('CmbComditiyDetailByItemCusInDetail')); });
        on('CmbPmDetailByItemCusInDetail', 'change', function () { if (!val('txtPmDetailInDetail').trim() && valI('CmbPmDetailByItemCusInDetail') > 0) setText('txtPmDetailInDetail', text('CmbPmDetailByItemCusInDetail')); });
        on('txtQtyMTonInDetail', 'input', function () { calcWeight(); calcAmount(); });
        on('txtOuterQtyInDetail', 'input', function () { calcWeight(); });
        on('txtRateInDetail', 'input', function () { calcWeight(); calcAmount(); });
        on('CmbOuterUomInDetail', 'change', function () { calcWeight(); calcAmount(); });
        on('CmbRateUomInDetail', 'change', function () { calcWeight(); calcAmount(); });
        on('CmbInnerUomInDetail', 'change', innerQtyCalc);
        on('BtnAddInDetail', 'click', addToGrid); on('btnUpdateInDetail', 'click', updateDetail); on('btnCancelInDetail', 'click', function () { resetDetail(); });
        ['txtQtyMTonInDetail', 'txtOuterQtyInDetail', 'txtRateInDetail', 'txtExchangeRate', 'txtNoOfContainers', 'txtPalletQtyInDetail', 'FromDocNoHistory', 'ToDocNoHistory'].forEach(function (id) { on(id, 'keypress', X.numericOnly); });
        on('txtDeliveryDays', 'keypress', X.integerOnly);
        document.addEventListener('keydown', function (e) {
            var inForm = tabsMain.current() === 'c3Form';
            if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'button') {
                var f = Array.prototype.filter.call(document.querySelectorAll('input,select,textarea,button'), function (x) { return !x.disabled && x.tabIndex >= 0 && x.offsetParent !== null; });
                var i = f.indexOf(e.target); if (i >= 0 && i < f.length - 1) { e.preventDefault(); f[i + 1].focus(); }
                return;
            }
            if (!e.ctrlKey) { if (e.key === 'Escape' && !document.querySelector('.ex-fullscreen')) X.cancelWindow(); return; }
            var k = e.key.toLowerCase();
            if (k === 'e') { e.preventDefault(); X.cancelWindow(); }
            else if (k === 't') { e.preventDefault(); if (!inForm) { tabsMain.select('c3Form'); tabs2.select('tabContractDetail'); } else tabsMain.select('c3History'); }
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
            else if (inForm && e.key === 'ArrowUp') { e.preventDefault(); focus('CmbItemNameInDetail'); }
            else if (inForm && e.key === 'ArrowRight') { e.preventDefault(); var order = ['tabContractDetail', 'tabPaymentTerm', 'tabPackingMaterialDetail', 'tabOtherItems', 'tabOtherCharges']; tabs2.select(order[(order.indexOf(tabs2.current()) + 1) % order.length]); }
            else if (e.altKey) { e.preventDefault(); shortcuts(); }
        });
        setup().then(function () {
            var q = /[?&]id=(\d+)/.exec(location.search);
            if (q) readById(netI(q[1])).catch(function (e) { box(e.message); });
        }).catch(function (e) { box(e.message); });
    }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', wire); else wire();

    global.ExportContractIII = { btnNew: btnNew, btnRefresh: btnRefresh, btnSave: btnSave, btnUpdate: btnUpdate, btnSaveAs: btnSaveAs, print501: print501, print501A: print501A,
        popup: popup, shortcuts: shortcuts, historyShow: historyShow, historyNew: historyNew, historyRefresh: historyRefresh, toggleHistory: toggleHistory, readById: readById };
})(window);
