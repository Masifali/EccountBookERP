/* ============================================================================================
 * countx_imports_lc_order.js - screen 525 "Import : Lc Order" (Architecture.WinApp.Import/ImLcOrder.cs).
 * Every handler follows the form's method of the same name: validation order and wording, what New / Save /
 * Update / Edit / Save As / Delete do to the buttons and grids, the detail and payment calculations.
 * API /api/import/lc-order/*. Exports window.ImpALc.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/import/lc-order';
    var P = {};
    window.ImpALc = P;

    var S = {};                        // setup: rights, formats, combos
    var F = { fcyDecimals: 0, amountDecimals: 0, rateDecimals: 2, fcyRound: 0 };
    var RecId = 0, UpdateMode = false;
    var updateDetailIndex = -1, updateDetailIndexPaymentDetail = -1;
    var lstRemoveRecord = [];
    var uomRows = [];                  // GetUomScheduleByItemId of the item in the detail box
    var grdDetail, grdPaymentTerm, DataGridHistory, grdHistoryDetail, tabMain, tabDetail;

    function $(id) { return HRM.$(id); }
    function val(id) { return HRM.val(id); }
    function set(id, v) { HRM.setVal(id, v === null || v === undefined ? '' : v); }
    function num(v) { return HRM.num(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); }
    function fcyFmt(v) { return ImpA.fixed(v, F.fcyDecimals); }        // clsGlobalVariables.stringFormatsingleForFcy
    function lcyFmt(v) { return ImpA.fixed(v, F.amountDecimals); }     // stringFormatsingle
    function box(m) { HRM.box(m); }
    function status(t) { HRM.text('lblStatus', t || ''); }

    // ------------------------------------------------------------------------ combos (the *Fill methods)
    var ZERO = '';
    function fillKeep(id, rows, v, t, first) {
        var prev = HRM.comboVal(id);
        HRM.fill(id, rows, v, t, { zero: ZERO });
        if (first && rows && rows.length) HRM.setCombo(id, HRM.col(rows[0], v));       // Rows[1].Activate()
        if (prev > 0) HRM.setCombo(id, HRM.hasOption(id, prev) ? prev : 0);
    }
    function fixedRows(pairs) { return pairs.map(function (p) { return { Id: p[0], Name: p[1] }; }); }
    var LEGAL = fixedRows([[1, 'Required'], [2, 'Non-Required']]);
    var STATUS = fixedRows([[1, 'Open'], [2, 'Complete'], [3, 'Cancel']]);

    function itemText(r) { return HRM.checked('rdSearchByName') ? HRM.col(r, 'ItemName') : HRM.col(r, 'ItemCode'); }

    function fillAll(d) {
        fillKeep('cmbSupplier', d.suppliers, 'Id', 'CompanyName');                 // SupplierFill
        fillKeep('cmbNotifyParty', d.suppliers, 'Id', 'CompanyName');
        fillKeep('cmbShippedTo', d.suppliers, 'Id', 'CompanyName');
        fillKeep('cmbSalesPerson', d.salesPersons, 'Id', 'CompanyName');           // salesPersonFill
        fillKeep('cmbLoadingPort', d.ports, 'Id', 'PortName', true);               // LoadingPortFill (Rows[1].Activate)
        fillKeep('cmbDestinationPort', d.ports, 'Id', 'PortName', true);
        fillKeep('CmbImporterBank', d.importerBanks, 'Id', 'BranchName');          // ImporterandExportBankFill
        fillKeep('cmbExporterBanks', d.exporterBanks, 'Id', 'BranchName');
        HRM.fill('cmbLegalizationRequired', LEGAL, 'Id', 'Name', { zero: ZERO });  // LegalizationFill: Rows[1]
        HRM.setCombo('cmbLegalizationRequired', 1);
        HRM.fill('cmbInspectionRequired', LEGAL, 'Id', 'Name', { zero: ZERO });    // InspectionFill: Rows[1]
        HRM.setCombo('cmbInspectionRequired', 1);
        fillKeep('cmbPaymenTerm', d.paymentTerms, 'Id', 'lcOrderTerm');            // PaymentTermsFill
        fillKeep('CmbPaymentTermDetail', d.paymentTerms, 'Id', 'lcOrderTerm');
        fillKeep('cmbCurreny', d.currencies, 'Id', 'CurrencyCode', true);          // MultiCurrencyfill (Rows[1])
        fillKeep('cmbDeliveryTerm', d.deliveryTerms, 'Id', 'Code', true);          // DeliveryTermFill (Rows[1])
        HRM.fill('cmbOrderStatus', STATUS, 'Id', 'Name', { zero: ZERO });          // StatusFill: Rows[1]
        HRM.setCombo('cmbOrderStatus', 1);
        fillKeep('cmbItem', d.items, 'Id', itemText);                               // ItemDetailFill
        fillKeep('cmbPackingType', d.packingTypes, 'Id', 'Description');            // ItemPacktype
        S.items = d.items; S.suppliers = d.suppliers; S.paymentTerms = d.paymentTerms;
    }

    // ------------------------------------------------------------------------ grids
    function buildGrids() {
        grdDetail = new HRM.Grid('grdDetail', {                                      // GrdSetting
            columns: [
                ImpA.btnCol('Edit', 'Edit', 50), ImpA.btnCol('Delete', 'X', 30),
                ImpA.hid('Id'), ImpA.hid('ItemId'),
                { key: 'ItemName', caption: 'ItemName', width: 200 }, { key: 'ItemCode', caption: 'ItemCode', width: 90 },
                { key: 'Description', caption: 'Description', width: 160 },
                ImpA.hid('PackingTypeId'), { key: 'PackingType', caption: 'PackingType', width: 110 },
                ImpA.hid('ItemUomId'), { key: 'ItemUom', caption: 'ItemUom', width: 70 },
                ImpA.numCol('Qty', 'Qty', '#,##.##', true), ImpA.numCol('NetWeight', 'NetWeight', '#,##.##', true),
                { key: 'ItemRate', caption: 'ItemRate', type: 'num', align: 'right', sum: true, render: function (v) { return HRM.esc(ImpA.fixed(v, F.rateDecimals)); } },
                ImpA.hid('RateUomId'), { key: 'RateUom', caption: 'RateUom', width: 70 },
                { key: 'FcyAmount', caption: 'FcyAmount', type: 'num', align: 'right', sum: true, render: function (v) { return HRM.esc(fcyFmt(v)); } },
                { key: 'LcyAmount', caption: 'LcyAmount', type: 'num', align: 'right', sum: true, render: function (v) { return HRM.esc(lcyFmt(v)); } },
                { key: 'RemarksDetail', caption: 'RemarksDetail', width: 160 }
            ],
            totals: true, emptyText: '',
            onDouble: function (r, i) { grdDetail_DoubleClick(i); },
            onDraw: function (g) {
                ImpA.footer(g, {
                    Qty: function (rs) { return ImpA.fmt(ImpA.sumOf(rs, 'Qty'), '#,##.##'); },
                    NetWeight: function (rs) { return ImpA.fmt(ImpA.sumOf(rs, 'NetWeight'), '#,##.##'); },
                    ItemRate: function (rs) { return ImpA.fixed(ImpA.avgOf(rs, 'ItemRate'), F.rateDecimals); },
                    FcyAmount: function (rs) { return fcyFmt(ImpA.sumOf(rs, 'FcyAmount')); },
                    LcyAmount: function (rs) { return lcyFmt(ImpA.sumOf(rs, 'LcyAmount')); }
                });
            }
        });
        ImpA.buttons(grdDetail, 'grdDetail', function (r, act, i) { grdDetail_ColumnButtonClick(act, i); });

        grdPaymentTerm = new HRM.Grid('grdPaymentTerm', {                           // grdSettingsPaymentTerm
            columns: [
                ImpA.btnCol('Delete', 'X', 30), ImpA.hid('Id'), ImpA.hid('PaymentTermId'),
                { key: 'PaymentTerm', caption: 'PaymentTerm', width: 220 },
                ImpA.numCol('%ofTotal', '%ofTotal', '#,##0.###', false),
                { key: 'FcyAmount', caption: 'FcyAmount', type: 'num', align: 'right', sum: true, render: function (v) { return HRM.esc(fcyFmt(v)); } }
            ],
            totals: true, emptyText: '',
            onDouble: function (r, i) { grdPaymentTerm_DoubleClick(i); },
            onDraw: function (g) { ImpA.footer(g, { FcyAmount: function (rs) { return fcyFmt(ImpA.sumOf(rs, 'FcyAmount')); } }); }
        });
        ImpA.buttons(grdPaymentTerm, 'grdPaymentTerm', function (r, act, i) {
            if (act === 'Delete') paymentDelete(i);
        });

        var R = S.rights || {};
        var hcols = [];
        if (R.update) hcols.push(ImpA.btnCol('Edit', 'Edit', 50));                   // ConfigureUserRightsColumns
        if (R.save) hcols.push(ImpA.btnCol('SaveAs', 'SaveAs', 55));
        if (R.print) hcols.push(ImpA.btnCol('Print', 'Print', 50));
        hcols = hcols.concat([
            ImpA.hid('RecordNo'), ImpA.hid('Id'), ImpA.hid('DocumentTypeId'),
            { key: 'DocNo', caption: 'DocNo', width: 60 }, ImpA.dateCol('DocDate', 'DocDate'),
            { key: 'SupOrderNo', caption: 'SupOrderNo', width: 90 }, ImpA.dateCol('SupOrderDate', 'SupOrderDate'),
            ImpA.hid('SupplierCustomerId'), { key: 'SupplierName', caption: 'SupplierName', width: 180 },
            { key: 'NotifyParty', caption: 'NotifyParty', width: 150 }, ImpA.dateCol('ExpiryDate', 'ExpiryDate'),
            { key: 'ExpiryPlace', caption: 'ExpiryPlace' }, { key: 'PartialShipment', caption: 'PartialShipment' },
            { key: 'TransShipment', caption: 'TransShipment' }, ImpA.dateCol('LastShipmentDate', 'LastShipmentDate'),
            { key: 'QuotReference', caption: 'QuotReference' }, { key: 'InquiryReference', caption: 'InquiryReference' },
            { key: 'ImporterBank', caption: 'ImporterBank' }, { key: 'ExporterBank', caption: 'ExporterBank' },
            { key: 'LoadingPort', caption: 'LoadingPort' }, { key: 'DestinationPort', caption: 'DestinationPort' },
            { key: 'PaymentTerm', caption: 'PaymentTerm' },
            { key: 'FcyAmount', caption: 'FcyAmount', type: 'num', align: 'right', sum: true, render: function (v) { return HRM.esc(fcyFmt(v)); } },
            { key: 'CurrencyCode', caption: 'CurrencyCode' },
            ImpA.numCol('ExchangeRate', 'ExchangeRate', '#,##0.####', false),
            { key: 'LcyAmount', caption: 'LcyAmount', type: 'num', align: 'right', sum: true, render: function (v) { return HRM.esc(lcyFmt(v)); } },
            ImpA.numCol('GrossWeightKgs', 'GrossWeightKgs', '#,##0.###', true), ImpA.numCol('NetWeightKgs', 'NetWeightKgs', '#,##0.###', true),
            ImpA.numCol('NoOfContainers', 'NoOfContainers', '#,##0.###', true),
            { key: 'Status', caption: 'Status' }, { key: 'CommodityDetial', caption: 'CommodityDetial' },
            { key: 'RemarksHeader', caption: 'RemarksHeader' }, { key: 'EntryDate', caption: 'EntryDate', type: 'datetime' },
            { key: 'EntryUserName', caption: 'EntryUserName' }, { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'int' }
        ]);
        DataGridHistory = new HRM.Grid('DataGridHistory', {
            columns: hcols, filterRow: true, totals: true, emptyText: 'No record found.',
            onDouble: function (r) { DataGridHistory_DoubleClick(r); },
            onSelect: function (r) { GridHistoryDetailBind(HRM.int(r.Id)); },
            onDraw: function (g) {
                ImpA.footer(g, {
                    FcyAmount: function (rs) { return fcyFmt(ImpA.sumOf(rs, 'FcyAmount')); },
                    LcyAmount: function (rs) { return lcyFmt(ImpA.sumOf(rs, 'LcyAmount')); },
                    GrossWeightKgs: function (rs) { return ImpA.fmt(ImpA.sumOf(rs, 'GrossWeightKgs'), '#,##0.###'); },
                    NetWeightKgs: function (rs) { return ImpA.fmt(ImpA.sumOf(rs, 'NetWeightKgs'), '#,##0.###'); },
                    NoOfContainers: function (rs) { return ImpA.fmt(ImpA.sumOf(rs, 'NoOfContainers'), '#,##0.###'); }
                });
            }
        });
        ImpA.buttons(DataGridHistory, 'DataGridHistory', function (r, act, i, b) { DataGridHistory_ColumnButtonClick(r, act, b); });

        grdHistoryDetail = new HRM.Grid('grdHistoryDetail', {                        // grddetailsetting
            columns: [
                ImpA.hid('Id'), ImpA.hid('ImLcOrderId'), ImpA.hid('ImItemId'),
                { key: 'ItemName', caption: 'ItemName', width: 200 }, { key: 'ItemCode', caption: 'ItemCode' },
                { key: 'ItemDescription', caption: 'ItemDescription' }, { key: 'PackingType', caption: 'PackingType' },
                { key: 'PackUom', caption: 'PackUom' },
                ImpA.numCol('Qty', 'Qty', '#,##0.###', true), ImpA.numCol('NetWeight', 'NetWeight', '#,##0.###', true),
                { key: 'RateUom', caption: 'RateUom' },
                { key: 'ItemRate', caption: 'ItemRate', type: 'num', align: 'right', sum: true, render: function (v) { return HRM.esc(ImpA.fixed(v, F.rateDecimals)); } },
                { key: 'FcyAmount', caption: 'FcyAmount', type: 'num', align: 'right', sum: true, render: function (v) { return HRM.esc(fcyFmt(v)); } },
                { key: 'LcyAmount', caption: 'LcyAmount', type: 'num', align: 'right', sum: true, render: function (v) { return HRM.esc(lcyFmt(v)); } },
                { key: 'RemarksDetail', caption: 'RemarksDetail' }
            ],
            totals: true, emptyText: '',
            onDraw: function (g) {
                ImpA.footer(g, {
                    Qty: function (rs) { return ImpA.fmt(ImpA.avgOf(rs, 'Qty'), '#,##0.###'); },          // AggregateFunction 3 (Average)
                    NetWeight: function (rs) { return ImpA.fmt(ImpA.avgOf(rs, 'NetWeight'), '#,##0.###'); },
                    ItemRate: function (rs) { return ImpA.fixed(ImpA.avgOf(rs, 'ItemRate'), F.rateDecimals); },
                    FcyAmount: function (rs) { return fcyFmt(ImpA.sumOf(rs, 'FcyAmount')); },
                    LcyAmount: function (rs) { return lcyFmt(ImpA.sumOf(rs, 'LcyAmount')); }
                });
            }
        });
    }

    // ------------------------------------------------------------------------ load
    function load() {
        return HRM.loading(HRM.get(API + '/setup')).then(function (d) {
            S = d || {};
            F = Object.assign(F, S.formats || {});
            if (S.itemSearchByCode) HRM.check('rdSearchByCode', true); else HRM.check('rdSearchByName', true);
            buildGrids();
            fillAll(S);
            set('txtdocno', S.docNo || '');
            var today = HRM.today();
            ['txtdocdate', 'txtSupLcOrderDate', 'txtExpiryDate', 'txtLastShipmentDate'].forEach(function (id) { set(id, today); });
            var R = S.rights || {};
            HRM.applyRights(R, { save: ['btnSave', 'BtnSaveAs'], update: 'btnUpdate', print: 'Print' });
            grdDetail.set([]); grdPaymentTerm.set([]);
            HRM.focus('txtdocdate');
        }).catch(HRM.fail);
    }

    // ------------------------------------------------------------------------ New / Reset
    function FormReset() {
        RecId = 0; UpdateMode = false;
        HRM.show('btnSave', true); HRM.show('btnUpdate', false); HRM.show('BtnSaveAs', false); HRM.show('btnDelete', false);
        var today = HRM.today();
        ['txtFcyAmountDetail', 'txtCommodityDescMain', 'txtSupLcOrderNo', 'txtItemDescription', 'txtdocno', 'txtExchangeRate', 'txtExpiryPlace',
            'txtGrossWeightMain', 'txtInquiryRef', 'txtLegalizationDesc', 'txtLcyAmountMain', 'txtNetWeightMain', 'txtNoOfContainerMain',
            'txtQuorationRef', 'txtItemRate', 'txtRemarksHeader', 'txtShippingMarks', 'txtTotalFcyAmountMain', 'txtWeightDetail', 'txtQtyDetail',
            'txtInspectionDesc', 'cmbTransShipment', 'cmbPartialShipment'].forEach(function (id) { set(id, ''); });
        set('txtdocdate', today); set('txtSupLcOrderDate', today);
        ['cmbSupplier', 'cmbSalesPerson', 'cmbShippedTo', 'cmbNotifyParty', 'cmbDeliveryTerm', 'cmbDestinationPort', 'cmbExporterBanks', 'cmbCurreny',
            'CmbImporterBank', 'cmbPackingType', 'cmbItemUom', 'cmbInspectionRequired', 'cmbPaymenTerm', 'cmbLegalizationRequired', 'cmbLoadingPort',
            'cmbOrderStatus'].forEach(function (id) { HRM.setCombo(id, 0); });
        FormResetDetail();
        grdDetail.set([]);
        lstRemoveRecord = [];
        ResetPaymentDetail();
        grdPaymentTerm.set([]);
        DocNo();
        HRM.focus('txtdocdate');
    }
    function DocNo() {
        return HRM.get(API + '/doc-no').then(function (d) { if (d && HRM.int(d.docNo) !== 0) set('txtdocno', d.docNo); }).catch(HRM.fail);
    }
    function FormResetDetail() {
        updateDetailIndex = -1;
        HRM.setCombo('cmbItem', 0); set('txtItemDescription', ''); HRM.setCombo('cmbPackingType', 0); set('txtQtyDetail', '');
        HRM.setCombo('cmbItemUom', 0); set('txtWeightDetail', ''); set('txtItemRate', ''); HRM.setCombo('cmbRateUom', 0); set('txtFcyAmountDetail', '');
        HRM.show('btnaddnew', true); HRM.show('btnGridUpdate', false); HRM.show('btnCancel', false);
    }
    function ResetPaymentDetail() {
        updateDetailIndexPaymentDetail = -1;
        HRM.setCombo('CmbPaymentTermDetail', 0); set('PercntOfTotal', ''); set('txtPaymentDetailFcyAmount', '');
        HRM.show('btnAddPayment', true); HRM.show('btnUpdatePayment', false); HRM.show('btnCancelPayment', false);
    }

    P.btnNew = function () { FormReset(); FormResetDetail(); };
    P.btnRefresh = function (b) {
        return HRM.busy(b, function () { return HRM.get(API + '/refresh').then(fillAll).catch(HRM.fail); });
    };

    // ------------------------------------------------------------------------ detail box
    /** bindRateUomAndItemPackUom(): the item's UOM schedule into Uom / Rate Uom, keeping the picked ones when still listed. */
    function bindRateUomAndItemPackUom(keepPack, keepRate) {
        var pack = keepPack !== undefined ? keepPack : HRM.comboVal('cmbItemUom');
        var rate = keepRate !== undefined ? keepRate : HRM.comboVal('cmbRateUom');
        var item = HRM.comboVal('cmbItem');
        if (!item) { uomRows = []; HRM.fill('cmbItemUom', [], 'Id', 'UOMCode', { zero: ZERO }); HRM.fill('cmbRateUom', [], 'Id', 'UOMCode', { zero: ZERO }); return Promise.resolve(); }
        return HRM.get(API + '/uoms', { itemId: item }).then(function (rows) {
            uomRows = rows || [];
            HRM.fill('cmbItemUom', uomRows, 'Id', 'UOMCode', { zero: ZERO });
            HRM.fill('cmbRateUom', uomRows, 'Id', 'UOMCode', { zero: ZERO });
            if (pack > 0) HRM.setCombo('cmbItemUom', pack);
            if (rate > 0) HRM.setCombo('cmbRateUom', rate);
            CalculateWeight();
        }).catch(HRM.fail);
    }
    function uomEq(id) {
        var v = HRM.comboVal(id), hit = null;
        uomRows.forEach(function (r) { if (HRM.int(r.Id) === v) hit = r; });
        return hit ? num(hit.Equivalent) : 0;
    }
    /** CalculateWeight(): Equivalent(Uom) * Qty as "##,#.##". */
    function CalculateWeight() {
        var eq = HRM.comboVal('cmbItemUom') > 0 ? uomEq('cmbItemUom') : 0, qty = num(val('txtQtyDetail'));
        set('txtWeightDetail', eq > 0 && qty > 0 ? ImpA.fmt(eq * qty, '##,#.##') : ImpA.fmt(0, '##,#.##'));
        CalculateAmount();
    }
    /** CalculateAmount(): Rate / Equivalent(Rate Uom) * Weight (decimal.ToString()). */
    function CalculateAmount() {
        var req = HRM.comboVal('cmbRateUom') > 0 ? uomEq('cmbRateUom') : 0;
        var w = num(val('txtWeightDetail')), rate = num(val('txtItemRate'));
        if (HRM.comboVal('cmbRateUom') > 0 && w > 0 && req > 0 && rate > 0) set('txtFcyAmountDetail', ImpA.plain(rate / req * w));
        else set('txtFcyAmountDetail', ImpA.fmt(0, '##,#.##'));
    }
    function DetailFormValidation() {
        if (!HRM.comboVal('cmbItem')) return fail('Item Field is Required!', 'cmbItem');
        if (!HRM.comboVal('cmbPackingType')) return fail('Packing Type Field is Required!', 'cmbPackingType');
        if (val('txtQtyDetail') === '' || num(val('txtQtyDetail')) === 0) return fail('Inner Qty Field is Required!', 'txtQtyDetail');
        if (!HRM.comboVal('cmbItemUom')) return fail('Pack UOM Field is Required!', 'cmbItemUom');
        if (num(val('txtWeightDetail')) === 0) return fail('Net Weight Field is Required!', 'txtWeightDetail');
        if (val('txtItemRate') === '' || num(val('txtItemRate')) === 0) return fail('Rate Field is Required!', 'txtItemRate');
        if (!HRM.comboVal('cmbRateUom')) return fail('Rate UOM Field is Required!', 'cmbRateUom');
        if (val('txtFcyAmountDetail') === '' || num(val('txtFcyAmountDetail')) === 0) return fail('Amount Field is Required!', 'txtFcyAmountDetail');
        return true;
    }
    function fail(msg, focus) { box(msg); if (focus) HRM.focus(focus); return false; }
    function detailRowFromBox(r) {
        var item = HRM.comboRow('cmbItem', 'Id') || {};
        r = r || { Id: 0 };
        r.ItemId = HRM.comboVal('cmbItem'); r.ItemName = HRM.str(item.ItemName); r.ItemCode = HRM.str(item.ItemCode);
        r.Description = val('txtItemDescription');
        r.PackingTypeId = HRM.comboVal('cmbPackingType'); r.PackingType = HRM.comboText('cmbPackingType');
        r.ItemUomId = HRM.comboVal('cmbItemUom'); r.ItemUom = HRM.comboText('cmbItemUom');
        r.Qty = num(val('txtQtyDetail')); r.NetWeight = num(val('txtWeightDetail')); r.ItemRate = num(val('txtItemRate'));
        r.RateUomId = HRM.comboVal('cmbRateUom'); r.RateUom = HRM.comboText('cmbRateUom');
        r.FcyAmount = num(val('txtFcyAmountDetail'));
        if (r.LcyAmount === undefined) r.LcyAmount = r.FcyAmount;
        r.RemarksDetail = val('txtRemarksDetail');
        return r;
    }
    P.btnaddnew = function () {
        if (!DetailFormValidation()) return;
        grdDetail.add(detailRowFromBox());
        FormResetDetail(); HRM.focus('cmbItem');
        CalculateLcyAmountInDetail(); CalculateNetWeightAndFcyAmountInMain();
    };
    function grdDetail_DoubleClick(i) {
        var r = grdDetail.rows()[i]; if (!r) return;
        updateDetailIndex = i;
        HRM.setCombo('cmbItem', r.ItemId);
        set('txtItemDescription', r.Description);
        HRM.setCombo('cmbPackingType', r.PackingTypeId);
        set('txtQtyDetail', ImpA.fmt(r.Qty, '#,##.##'));
        set('txtItemRate', ImpA.fixed(r.ItemRate, F.rateDecimals));
        set('txtRemarksDetail', r.RemarksDetail);
        bindRateUomAndItemPackUom(HRM.int(r.ItemUomId), HRM.int(r.RateUomId)).then(function () {
            set('txtWeightDetail', ImpA.fmt(r.NetWeight, '#,##.##'));
            set('txtFcyAmountDetail', fcyFmt(r.FcyAmount));
        });
        HRM.show('btnaddnew', false); HRM.show('btnGridUpdate', true); HRM.show('btnCancel', true);
        HRM.focus('cmbItem');
    }
    P.btnGridUpdate = function () {
        if (!DetailFormValidation()) return;
        var r = grdDetail.rows()[updateDetailIndex]; if (!r) return;
        grdDetail.update(updateDetailIndex, detailRowFromBox(r));
        FormResetDetail();
        CalculateLcyAmountInDetail(); CalculateNetWeightAndFcyAmountInMain();
        HRM.focus('cmbItem');
    };
    P.btnCancel = function () { FormResetDetail(); };
    function grdDetail_ColumnButtonClick(act, i) {
        if (act === 'Edit') grdDetail_DoubleClick(i);
        if (act === 'Delete') {
            if (updateDetailIndex > -1) { box('Please Reset Detail Box Data If You want to Delete Any row from the Grid....'); return; }
            var r = grdDetail.rows()[i];
            if (HRM.int(r.Id) > 0) {
                if (!HRM.ask('Are you sure to Delete?')) return;
                lstRemoveRecord.push(r);
            }
            grdDetail.remove(i);
        }
        CalculateLcyAmountInDetail(); CalculateNetWeightAndFcyAmountInMain();
    }

    // ------------------------------------------------------------------------ calculations
    /** CalculateLcyAmountInDetail(): LcyAmount = Fcy * ExchangeRate (Fcy when the rate is 0). */
    function CalculateLcyAmountInDetail() {
        var ex = num(val('txtExchangeRate'));
        grdDetail.rows().forEach(function (r) { var f = num(r.FcyAmount); r.LcyAmount = f !== 0 ? (ex > 0 ? f * ex : f) : 0; });
        grdDetail.draw();
    }
    /** CalculateNetWeightAndFcyAmountInMain(): the grid totals into Fcy Amount / Net Weight. */
    function CalculateNetWeightAndFcyAmountInMain() {
        if (grdDetail.rows().length > 0) {
            set('txtTotalFcyAmountMain', fcyFmt(grdDetail.sum('FcyAmount')));
            set('txtNetWeightMain', ImpA.fmt(grdDetail.sum('NetWeight'), '##,#.##'));
        } else {
            set('txtTotalFcyAmountMain', fcyFmt(0));
            set('txtNetWeightMain', ImpA.fmt(0, '##,#.##'));
        }
        CalculateLocalAmount(true);
    }
    /** CalculateLocalAmount(): Lcy = Fcy * ExchangeRate when both > 0 (the text is left alone otherwise). */
    function CalculateLocalAmount(skipTotals) {
        if (!skipTotals) {
            if (grdDetail.rows().length > 0) {
                set('txtTotalFcyAmountMain', fcyFmt(grdDetail.sum('FcyAmount')));
                set('txtNetWeightMain', ImpA.fmt(grdDetail.sum('NetWeight'), '##,#.##'));
            }
        }
        var f = num(val('txtTotalFcyAmountMain')), ex = num(val('txtExchangeRate'));
        if (f > 0 && ex > 0) set('txtLcyAmountMain', ImpA.plain(f * ex));
    }

    // ------------------------------------------------------------------------ payment tab
    function PaymentFormValidation() {
        if (!HRM.comboVal('CmbPaymentTermDetail')) return fail('Payment Term field required', 'CmbPaymentTermDetail');
        if (num(val('PercntOfTotal')) === 0) return fail('% of Total field required', 'PercntOfTotal');
        if (num(val('txtPaymentDetailFcyAmount')) === 0) return fail('Fcy Amount field required', 'txtPaymentDetailFcyAmount');
        return true;
    }
    /** PercntOfTotal_TextChanged. */
    function PercntOfTotal_TextChanged() {
        var p = num(val('PercntOfTotal')), f = num(val('txtTotalFcyAmountMain'));
        if (p > 100) { set('PercntOfTotal', '0'); set('txtPaymentDetailFcyAmount', '0'); box("%age Can't be Greater Than 100"); }
        else if (f > 0) set('txtPaymentDetailFcyAmount', lcyFmt(ImpA.roundAway(p * f / 100, F.fcyRound)));
        else { set('txtPaymentDetailFcyAmount', '0'); box('Fcy Amount is Zero'); }
    }
    P.btnAddPayment = function () {
        if (!PaymentFormValidation()) return;
        var p = num(val('PercntOfTotal'));
        if (p > 100) return fail("Percent can't be Greater than 100%", 'PercntOfTotal');
        var rows = grdPaymentTerm.rows();
        if (rows.length) {
            for (var i = 0; i < rows.length; i++) if (HRM.int(rows[i].PaymentTermId) === HRM.comboVal('CmbPaymentTermDetail')) return fail("You Can't Add Same Payment Term", 'CmbPaymentTermDetail');
            var t = grdPaymentTerm.sum('%ofTotal');
            if (t + p > 100) { box("Toatl Percent can't be Greater than 100%.Remaing " + (100 - t)); set('PercntOfTotal', String(100 - t)); PercntOfTotal_TextChanged(); return; }
        }
        grdPaymentTerm.add({ Id: 0, PaymentTermId: HRM.comboVal('CmbPaymentTermDetail'), PaymentTerm: HRM.comboText('CmbPaymentTermDetail'),
            '%ofTotal': p, FcyAmount: num(val('txtPaymentDetailFcyAmount')) });
        ResetPaymentDetail(); HRM.focus('CmbPaymentTermDetail');
    };
    P.btnUpdatePayment = function () {
        if (!PaymentFormValidation()) return;
        var p = num(val('PercntOfTotal'));
        if (p > 100) return fail("Percent can't be Greater than 100%", 'PercntOfTotal');
        var rows = grdPaymentTerm.rows(), cur = rows[updateDetailIndexPaymentDetail];
        if (!cur) return;
        for (var i = 0; i < rows.length; i++) {
            if (i !== updateDetailIndexPaymentDetail && HRM.int(rows[i].PaymentTermId) === HRM.comboVal('CmbPaymentTermDetail')) { box("You Can't Add Same Payment Term Twice"); return; }
        }
        var t = Math.abs(grdPaymentTerm.sum('%ofTotal') - num(cur['%ofTotal']));
        if (t + p > 100) { box("Toatl Percent can't be Greater than 100%.Remaing " + (100 - t)); set('PercntOfTotal', String(100 - t)); PercntOfTotal_TextChanged(); return; }
        cur.PaymentTermId = HRM.comboVal('CmbPaymentTermDetail'); cur.PaymentTerm = HRM.comboText('CmbPaymentTermDetail');
        cur['%ofTotal'] = p; cur.FcyAmount = num(val('txtPaymentDetailFcyAmount'));
        grdPaymentTerm.draw();
        ResetPaymentDetail(); HRM.focus('CmbPaymentTermDetail');
    };
    P.btnCancelPayment = function () { ResetPaymentDetail(); };
    function grdPaymentTerm_DoubleClick(i) {
        var r = grdPaymentTerm.rows()[i]; if (!r) return;
        if (RecId > 0 && HRM.int(r.Id) > 0) { box('This saved payment term row cannot be changed: USP_ImLcOrderPaymnetTerm_Insert only adds rows.'); return; }
        updateDetailIndexPaymentDetail = i;
        HRM.setCombo('CmbPaymentTermDetail', r.PaymentTermId);
        set('PercntOfTotal', ImpA.fmt(r['%ofTotal'], '#,##0.###'));
        set('txtPaymentDetailFcyAmount', fcyFmt(r.FcyAmount));
        HRM.show('btnAddPayment', false); HRM.show('btnUpdatePayment', true); HRM.show('btnCancelPayment', true);
        HRM.focus('CmbPaymentTermDetail');
    }
    function paymentDelete(i) {
        var r = grdPaymentTerm.rows()[i]; if (!r) return;
        if (RecId > 0 && HRM.int(r.Id) > 0) { box('This saved payment term row cannot be removed: no procedure deletes ImLcOrderPaymnetTerm rows.'); return; }
        grdPaymentTerm.remove(i);
    }

    // ------------------------------------------------------------------------ Save / Update / Save As
    function FormValidation() {
        if (val('txtdocno') === '' || HRM.int(val('txtdocno')) === 0) return fail('Doc No Field is Required!', 'txtdocno');
        if (!HRM.comboVal('cmbSupplier')) return fail('Supplier Field is Required!', 'cmbSupplier');
        if (val('txtSupLcOrderNo') === '' || val('txtSupLcOrderNo') === '0') return fail('Supplier Order No Field is Required!', 'cmbSupplier');
        if (!HRM.comboVal('cmbSalesPerson')) return fail('SalesPerson Field is Required!', 'cmbSalesPerson');
        if (!HRM.comboVal('cmbLoadingPort')) return fail('Loading Port Field is Required!', 'cmbLoadingPort');
        if (!HRM.comboVal('cmbDestinationPort')) return fail('Destination Port Field is Required!', 'cmbDestinationPort');
        if (!HRM.comboVal('CmbImporterBank')) return fail('Importer Bank Field is Required!', 'CmbImporterBank');
        if (!HRM.comboVal('cmbExporterBanks')) return fail('Exporter Bank Field is Required!', 'cmbExporterBanks');
        if (!HRM.comboVal('cmbPaymenTerm')) return fail('Payment Term Field is Required!', 'cmbPaymenTerm');
        if (val('txtTotalFcyAmountMain') === '' || num(val('txtTotalFcyAmountMain')) === 0) return fail('Fcy Amount Field is Required!', 'txtTotalFcyAmountMain');
        if (!HRM.comboVal('cmbCurreny')) return fail('Fcy Code Field is Required!', 'cmbCurreny');
        if (val('txtExchangeRate') === '' || num(val('txtExchangeRate')) === 0) return fail('Exchange Rate Field is Required!', 'txtExchangeRate');
        if (val('txtLcyAmountMain') === '' || num(val('txtLcyAmountMain')) === 0) return fail('Local Amount Field is Required!', 'txtLcyAmountMain');
        if (!HRM.comboVal('cmbDeliveryTerm')) return fail('Delivery Term Field is Required!', 'cmbDeliveryTerm');
        if (val('txtGrossWeightMain') === '' || num(val('txtGrossWeightMain')) === 0) return fail('Gross Weight Field is Required!', 'txtGrossWeightMain');
        if (val('txtNetWeightMain') === '' || num(val('txtNetWeightMain')) === 0) return fail('Net Weight Field is Required!', 'txtNetWeightMain');
        if (strictInt(val('txtGrossWeightMain')) < strictInt(val('txtNetWeightMain'))) return fail('Gross Weight Must Be Greater then Net Weight');
        if (!HRM.comboVal('cmbOrderStatus')) return fail('Status Field is Required!', 'cmbOrderStatus');
        return true;
    }
    /** Conversion.ToInt(TextBox.Text): only a plain whole number parses. */
    function strictInt(s) { s = String(s || '').trim(); return /^[+-]?\d+$/.test(s) ? parseInt(s, 10) : 0; }

    function body(saveAs) {
        var detail = function (r) {
            return { id: HRM.int(r.Id), itemId: HRM.int(r.ItemId), description: HRM.str(r.Description), packingTypeId: HRM.int(r.PackingTypeId),
                itemUomId: HRM.int(r.ItemUomId), qty: String(r.Qty), itemRate: String(r.ItemRate), rateUomId: HRM.int(r.RateUomId), remarks: HRM.str(r.RemarksDetail) };
        };
        return {
            id: RecId, saveAs: !!saveAs, docNo: val('txtdocno'), docDate: val('txtdocdate'), supplierId: HRM.comboVal('cmbSupplier'),
            lcOrderNo: val('txtSupLcOrderNo'), lcOrderDate: val('txtSupLcOrderDate'), salesPersonId: HRM.comboVal('cmbSalesPerson'),
            notifyPartyId: HRM.comboVal('cmbNotifyParty'), shippedToId: HRM.comboVal('cmbShippedTo'),
            partialShipment: val('cmbPartialShipment'), transShipment: val('cmbTransShipment'), lastShipmentDate: val('txtLastShipmentDate'),
            quotReference: val('txtQuorationRef'), inquiryReference: val('txtInquiryRef'), expiryDate: val('txtExpiryDate'), expiryPlace: val('txtExpiryPlace'),
            loadingPortId: HRM.comboVal('cmbLoadingPort'), destinationPortId: HRM.comboVal('cmbDestinationPort'),
            legalizationRequired: HRM.comboText('cmbLegalizationRequired'), inspectionRequired: HRM.comboText('cmbInspectionRequired'),
            importerBankId: HRM.comboVal('CmbImporterBank'), exporterBankId: HRM.comboVal('cmbExporterBanks'), paymentTermId: HRM.comboVal('cmbPaymenTerm'),
            currencyId: HRM.comboVal('cmbCurreny'), exchangeRate: val('txtExchangeRate'), deliveryTermId: HRM.comboVal('cmbDeliveryTerm'),
            grossWeight: val('txtGrossWeightMain'), noOfContainers: val('txtNoOfContainerMain'), status: HRM.comboText('cmbOrderStatus'),
            remarksHeader: val('txtRemarksHeader'), commodity: val('txtCommodityDescMain'), legalizationDescription: val('txtLegalizationDesc'),
            inspectionDescription: val('txtInspectionDesc'), shippingMarks: val('txtShippingMarks'),
            details: grdDetail.rows().map(detail), removed: lstRemoveRecord.map(detail),
            payments: grdPaymentTerm.rows().map(function (r) {
                return { id: HRM.int(r.Id), paymentTermId: HRM.int(r.PaymentTermId), percent: String(r['%ofTotal']), fcyAmount: String(r.FcyAmount) };
            })
        };
    }
    /** Insert(). */
    function Insert(btn, saveAs) {
        if (grdDetail.rows().length === 0) { box('Please Check Detail Grid'); return; }
        if (!FormValidation()) return;
        if (!HRM.ask(RecId === 0 || saveAs ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
        if (grdPaymentTerm.rows().length === 0) { box('Payment Term Grid record not found'); return; }
        var t = num(val('txtTotalFcyAmountMain')), pg = grdPaymentTerm.sum('FcyAmount');
        if (Math.abs(t - pg) > 1e-9) { box("Payment Term Grid Total Amount not Equal To Total FcyAmount.. PaymentGridAmount is = '" + pg + "' and TotalFcyAmount is = '" + t + "'"); return; }
        CalculateLcyAmountInDetail(); CalculateLocalAmount();
        var wasUpdate = UpdateMode && !saveAs;
        return HRM.busy(btn, function () {
            return HRM.post(API + '/save', body(saveAs)).then(function (d) {
                box(d && d.message ? d.message : (wasUpdate ? 'Update SuccessFully' : 'Save SuccessFully'));
                FormReset();
            }).catch(HRM.fail);
        });
    }
    P.btnSave = function (b) { RecId = 0; return Insert(b, false); };
    P.btnUpdate = function (b) { return Insert(b, false); };
    P.BtnSaveAs = function (b) { return Insert(b, true); };

    // ------------------------------------------------------------------------ ReadById / Delete
    function ReadById(id, after) {
        return HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (d) {
            RecId = id;
            var h = d.header;
            tabMain.show('pgForm');
            set('txtdocno', HRM.str(h.DocNo));
            set('txtdocdate', ImpA.day(h.DocDate));
            HRM.setCombo('cmbSupplier', h.SupplierCustomerId);
            set('txtSupLcOrderNo', h.LcOrderNo); set('txtSupLcOrderDate', ImpA.day(h.LcOrderDate));
            HRM.setCombo('cmbSalesPerson', h.SalesPersonId); HRM.setCombo('cmbNotifyParty', h.NotifyPartyId); HRM.setCombo('cmbShippedTo', h.ShipedToId);
            set('cmbPartialShipment', h.PartialShipment); set('cmbTransShipment', h.TransShipment);
            set('txtLastShipmentDate', ImpA.day(h.LastShipmentDate)); set('txtQuorationRef', h.QuotReference); set('txtInquiryRef', h.InquiryReference);
            set('txtExpiryDate', ImpA.day(h.ExpiryDate)); set('txtExpiryPlace', h.ExpiryPlace);
            HRM.setCombo('cmbLoadingPort', h.LoadingPortId); HRM.setCombo('cmbDestinationPort', h.DestinationPortId);
            HRM.setCombo('cmbInspectionRequired', 0); HRM.setComboText('cmbInspectionRequired', HRM.str(h.InsepctionRequired));
            HRM.setCombo('cmbLegalizationRequired', 0); HRM.setComboText('cmbLegalizationRequired', HRM.str(h.LegalizationRequired));
            HRM.setCombo('CmbImporterBank', h.ImporterBankId); HRM.setCombo('cmbExporterBanks', h.ExporterBankId);
            HRM.setCombo('cmbPaymenTerm', h.PaymentTermId); HRM.setCombo('cmbCurreny', h.FcurrencyId);
            set('txtTotalFcyAmountMain', ImpA.plain(h.FcyAmount)); set('txtExchangeRate', ImpA.plain(h.ExchangeRate)); set('txtLcyAmountMain', ImpA.plain(h.LcyAmount));
            HRM.setCombo('cmbDeliveryTerm', h.DeliveryTermId);
            set('txtGrossWeightMain', ImpA.plain(h.GrossWeightKgs)); set('txtNetWeightMain', ImpA.plain(h.NetWeightKgs)); set('txtNoOfContainerMain', HRM.str(h.NoOfContainers));
            HRM.setCombo('cmbOrderStatus', 0); HRM.setComboText('cmbOrderStatus', HRM.str(h.Status));
            set('txtRemarksHeader', h.RemarksHeader); set('txtCommodityDescMain', h.CommodityDetial); set('txtInspectionDesc', h.InsepctionDescription);
            set('txtLegalizationDesc', h.LegalizationDescription); set('txtShippingMarks', h.ShippingMarks);
            lstRemoveRecord = [];
            FormResetDetail();
            grdDetail.set(d.details || []);
            ResetPaymentDetail();
            if ((d.payments || []).length) grdPaymentTerm.set(d.payments);
            CalculateLcyAmountInDetail(); CalculateLocalAmount();
            UpdateMode = true;
            HRM.show('btnSave', false); HRM.show('BtnSaveAs', false); HRM.show('btnUpdate', true); HRM.show('btnDelete', true);
            if (after) after();
            HRM.focus('txtdocdate');
        }).catch(HRM.fail);
    }
    P.btnDelete = function (b) {
        if (RecId <= 0) { box('No record found to Delete'); return; }
        if (!HRM.ask('Are you sure to Delete?')) return;
        return HRM.busy(b, function () {
            return HRM.post(API + '/delete', { id: RecId }).then(function (d) { box(d && d.message ? d.message : 'Delete Record Seccessfully'); FormReset(); }).catch(HRM.fail);
        });
    };

    // ------------------------------------------------------------------------ History
    function GridHistoryFill(all, btn) {
        var run = function () {
            return HRM.get(API + '/history', { all: !!all }).then(function (rows) {
                DataGridHistory.set(rows || []); grdHistoryDetail.set([]);
                status((rows || []).length + ' record(s)');
            }).catch(HRM.fail);
        };
        return btn ? HRM.busy(btn, run) : HRM.loading(run());
    }
    P.btnLoadAllHistory = function (b) { return GridHistoryFill(true, b); };
    function GridHistoryDetailBind(id) {
        if (!id) return;
        HRM.get(API + '/history-detail', { id: id }).then(function (rows) { if ((rows || []).length) grdHistoryDetail.set(rows); }).catch(HRM.fail);
    }
    function DataGridHistory_DoubleClick(r) {
        if ((S.rights || {}).update) ReadById(HRM.int(r.Id));
        else box("you Don't Have Update right....");
    }
    function DataGridHistory_ColumnButtonClick(r, act, b) {
        var R = S.rights || {};
        if (act === 'Edit') {
            if (!R.update && !R['delete']) { box("you Don't Have Update right...."); return; }
            ReadById(HRM.int(r.Id), function () { HRM.show('btnDelete', true); });
        }
        if (act === 'SaveAs') {
            if (!R.save && !R['delete']) { box("you Don't Have Update right...."); return; }
            ReadById(HRM.int(r.Id), function () {
                HRM.show('BtnSaveAs', true); HRM.show('btnDelete', true); HRM.show('btnSave', false); HRM.show('btnUpdate', false);
            });
        }
        if (act === 'Print' && R.print) GenerateSlip(HRM.int(r.Id), b);
    }

    // ------------------------------------------------------------------------ print / misc buttons
    function GenerateSlip(id, btn) {
        return ImpA.checkedPrint(btn, API + '/print-check', { id: id }, 'impa-231', { id: id });
    }
    P.Print = function (b) { return GenerateSlip(RecId, b); };
    P.btnAttachment = function () { box('Attachments are not available on the web page (the desktop copies files to a local attachment folder).'); };
    P.btnSeaPortDefine = function () { HRM.open('/master-data/sea-ports'); };
    P.btnPaymentTerms = function () { box('The Payment Terms definition form is not available on the web.'); };
    P.btnDeliveryTerm = function () { box('The Delivery Terms definition form is not available on the web.'); };
    var SHORTCUTS = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'],
        ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'], ['F12', 'For Save As'],
        ['Ctrl+F1', 'For Sea Port Define'], ['Ctrl+F2', 'For Payment Term Define'], ['Ctrl+F3', 'For Delivery Term Define'], ['Ctrl+T', 'For Tab Transfer'],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+D', 'For Adding an row in Focused Grid'], ['Ctrl+Delete', 'For Deleting an row of Focused Grid'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On Item in Detail Box'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
        ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    P.BtnShortCutkeys = function () { ImpA.shortcuts(SHORTCUTS); };

    // ------------------------------------------------------------------------ wiring
    function onFormTab(fn) { return function () { if (tabMain.current === 'pgForm') fn(); }; }
    function visibleEnabled(id) { var e = $(id); return e && !e.disabled && HRM.visible(id); }
    function wire() {
        tabMain = ImpA.tabs('tabMain', function (id) { if (id === 'pgHistory') GridHistoryFill(false); });   // tabControl1_SelectedIndexChanged(1) -> 50
        tabDetail = ImpA.tabs('tabDetail');
        HRM.footer(function () { tabMain.show('pgHistory'); });
        $('cmbItem').addEventListener('change', function () { bindRateUomAndItemPackUom(); });                 // cmbitem_Leave
        $('cmbItemUom').addEventListener('change', CalculateWeight);                                              // cmbItemUom_ValueChanged
        $('txtQtyDetail').addEventListener('input', CalculateWeight);                                             // txtQtyDetail_TextChanged
        $('txtItemRate').addEventListener('input', CalculateAmount);                                              // txtrate_TextChanged
        $('cmbRateUom').addEventListener('change', CalculateAmount);                                              // cmbrateuom_TextChanged
        $('txtExchangeRate').addEventListener('input', function () { CalculateLocalAmount(); });                  // txtexchangerate_TextChanged
        $('PercntOfTotal').addEventListener('input', PercntOfTotal_TextChanged);
        ['txtQtyDetail', 'txtExchangeRate', 'txtGrossWeightMain', 'txtItemRate', 'PercntOfTotal'].forEach(function (id) { ImpA.guard(id, true); });
        ImpA.guard('txtNoOfContainerMain', false);
        HRM.keys({
            'ctrl+t': function () { if (tabMain.current === 'pgHistory') { tabMain.show('pgForm'); HRM.focus('txtdocdate'); } else tabMain.show('pgHistory'); },
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+shift+delete': onFormTab(function () { if (visibleEnabled('btnDelete')) P.btnDelete($('btnDelete')); }),
            'ctrl+s': onFormTab(function () { if (visibleEnabled('btnSave')) P.btnSave($('btnSave')); }),
            'ctrl+f12': onFormTab(function () { if (visibleEnabled('BtnSaveAs')) P.BtnSaveAs($('BtnSaveAs')); }),
            'ctrl+u': onFormTab(function () { if (visibleEnabled('btnUpdate') && UpdateMode) P.btnUpdate($('btnUpdate')); }),
            'ctrl+n': onFormTab(function () { P.btnNew(); }),
            'ctrl+r': onFormTab(function () { P.btnRefresh($('btnRefresh')); }),
            'ctrl+f5': onFormTab(function () { HRM.focus('txtdocdate'); }),
            'ctrl+f10': onFormTab(P.btnAttachment),
            'ctrl+f1': onFormTab(P.btnSeaPortDefine),
            'ctrl+arrowup': onFormTab(function () { HRM.focus(tabDetail.current === 'pgDetail' ? 'cmbItem' : 'CmbPaymentTermDetail'); }),
            'ctrl+arrowright': onFormTab(function () { tabDetail.show(tabDetail.current === 'pgDetail' ? 'pgPayment' : 'pgDetail'); })
        });
    }

    document.addEventListener('DOMContentLoaded', function () { wire(); load(); });
})();
