/* ============================================================================================
 * countx_imports_proforma_invoice.js - 782 Proforma Invoice, ported from Architecture.WinApp.Import.frmProformaInvoice.
 * Built on countx_hrm.js (HRM) and countx_imports_impb_core.js (ImpB); exports window.ImpBProforma.
 * Each handler names the form method it follows (validation order and wording, grid behaviour, New / Save / Update).
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/import/proforma-invoice';
    var P = {}; window.ImpBProforma = P;
    var RecId = 0, UpdateMode = false, rights = {}, branchFeature = false, userBranchId = 0, historyDays = 0;
    var dtPerformaNo = [], dtUom = [], updateDetailIndex = -1, LstRemoveRecordDetail = [], AT = ImpB.attachmentState();

    function btnCell(act, text) { return '<button type="button" class="win-btn-action impb-cell-btn" data-act="' + act + '">' + text + '</button>'; }
    function n(key, caption, dec, sum) { return { key: key, caption: caption, type: 'num', sum: !!sum, decimals: dec, render: function (v) { return HRM.esc(ImpB.fmt(v, dec, 0)); } }; }

    // ------------------------------------------------------------------ detail grid (grdSettings)
    var grd = new HRM.Grid('grdDetail', {
        columns: [
            { key: 'Delete', caption: 'X', width: 20, render: function () { return btnCell('delete', 'X'); } },
            { key: 'Id', hidden: true }, { key: 'ItemId', hidden: true }, { key: 'ItemCode', caption: 'ItemCode' }, { key: 'ItemName', caption: 'ItemName' },
            { key: 'ItemDetail', caption: 'ItemDetail' }, { key: 'JobLotId', hidden: true }, { key: 'JobLot', caption: 'JobLot' },
            n('QtyMTon', 'Qty/M.Ton', 3, true), { key: 'PackSizeId', hidden: true }, { key: 'PackSize', caption: 'PackSize' },
            { key: 'PackEquivalent', hidden: true }, n('NoOfBags', 'NoOfBags', 3, true), n('NetWeight', 'NetWeight', 3, true),
            n('CostMTon', 'Cost/M.Ton', 4, false), { key: 'RateUOMId', hidden: true }, { key: 'RateUOM', caption: 'RateUOM' },
            { key: 'RateEquivalent', hidden: true }, n('Amount', 'Amount', 3, true), { key: 'Remarks', caption: 'Remarks' },
            { key: 'RowVersionLong', hidden: true }
        ],
        totals: true,
        onDouble: function (r, i) { grdDetail_DoubleClick(r, i); }
    });
    HRM.$('grdDetail').addEventListener('click', function (e) {                          // grdDetail_ColumnButtonClick
        var b = e.target.closest('button[data-act="delete"]'); if (!b) return;
        var tr = b.closest('tr[data-i]'); if (!tr) return;
        DeleteDetailGridRow(+tr.getAttribute('data-i'));
        getTotalnetWeightFromGrid();
    });
    HRM.$('grdDetail').addEventListener('keydown', function (e) {                        // grdDetail_KeyDown
        var i = grd.currentIndex(); if (i < 0) return;
        if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); grdDetail_DoubleClick(grd.rows()[i], i); }
        else if (e.ctrlKey && e.key === 'Delete') { e.preventDefault(); DeleteDetailGridRow(i); getTotalnetWeightFromGrid(); }
    });
    function DeleteDetailGridRow(i) {
        if (updateDetailIndex !== -1) { HRM.box('Reset the Detail First'); return; }
        if (grd.rows().length <= 1) return;
        var r = grd.rows()[i];
        if (HRM.int(HRM.col(r, 'Id')) > 0) {
            if (!HRM.ask('Are you sure to Delete?')) return;
            LstRemoveRecordDetail.push(r);
        }
        grd.remove(i);
    }
    /** getTotalnetWeightFromGrid(): Net Weight = Σ Qty/M.Ton × 1000 ("#,##0.#####"), Amount = Σ Amount ("#,##0.###"). */
    function getTotalnetWeightFromGrid() {
        HRM.setVal('txtNetWeightHeader', ImpB.fmt(grd.sum('QtyMTon') * 1000, 5, 0));
        HRM.setVal('txtfcyAmountHeader', ImpB.fmt(grd.sum('Amount'), 3, 0));
    }

    // ------------------------------------------------------------------ detail entry
    var calc = ImpB.qtyEntry({ uoms: function () { return dtUom; }, importInvoice: false });
    function bindRateUomAndItemPackUom() {                                               // combitem_Leave
        var pack = HRM.comboText('CmbPackUom'), rate = HRM.comboText('CmbRateUom');
        return HRM.get(API + '/uoms', { itemId: HRM.comboVal('CmbItemName') }).then(function (rows) {
            dtUom = rows || [];
            HRM.fill('CmbPackUom', dtUom, 'Id', 'UOMCode', { zero: '' });
            HRM.fill('CmbRateUom', dtUom, 'Id', 'UOMCode', { zero: '' });
            if (pack) HRM.setComboText('CmbPackUom', pack);
            if (rate) HRM.setComboText('CmbRateUom', rate);
        }).catch(HRM.fail);
    }
    function uomEq(id) { var e = 0; dtUom.forEach(function (u) { if (HRM.int(HRM.col(u, 'Id')) === id) e = HRM.num(HRM.col(u, 'Equivalent')); }); return e; }
    function DetailFormValidation() {
        if (!HRM.comboVal('CmbItemName')) { HRM.box('Item field required'); HRM.focus('CmbItemName'); return false; }
        if (!HRM.comboVal('CmbJobLot')) { HRM.box('JobLot field required'); HRM.focus('CmbJobLot'); return false; }
        if (!HRM.comboVal('CmbPackUom')) { HRM.box('Pack uom field required'); HRM.focus('CmbPackUom'); return false; }
        if (HRM.val('txtQtyMTon').trim() === '' || !(HRM.num(HRM.val('txtQtyMTon')) > 0)) { HRM.box('Qty/M.Ton field required'); HRM.focus('txtQtyMTon'); return false; }
        if (HRM.val('txtNoOfBags').trim() === '' || !(HRM.num(HRM.val('txtNoOfBags')) > 0)) { HRM.box('No of Bags field required'); HRM.focus('txtNoOfBags'); return false; }
        if (HRM.val('txtCostMTon').trim() === '' || !(HRM.num(HRM.val('txtCostMTon')) > 0)) { HRM.box('Cost/M.Ton field required'); HRM.focus('txtCostMTon'); return false; }
        if (!HRM.comboVal('CmbRateUom')) { HRM.box('Rate UOM field required'); HRM.focus('CmbRateUom'); return false; }
        if (HRM.val('txtAmount').trim() === '' || !(HRM.num(HRM.val('txtAmount')) > 0)) { HRM.box('Amount field required'); HRM.focus('txtAmount'); return false; }
        return true;
    }
    function entry(r, withRemarks) {
        var item = HRM.comboRow('CmbItemName', 'Id') || {};
        r.ItemId = HRM.comboVal('CmbItemName'); r.ItemCode = HRM.str(HRM.col(item, 'ItemCode')); r.ItemName = HRM.str(HRM.col(item, 'ItemName'));
        r.ItemDetail = HRM.val('txtitemDescription').trim();
        r.JobLotId = HRM.comboVal('CmbJobLot'); r.JobLot = HRM.comboText('CmbJobLot').trim();
        r.QtyMTon = HRM.num(HRM.val('txtQtyMTon'));
        r.PackSizeId = HRM.comboVal('CmbPackUom'); r.PackSize = HRM.comboText('CmbPackUom').trim(); r.PackEquivalent = uomEq(r.PackSizeId);
        r.NoOfBags = HRM.num(HRM.val('txtNoOfBags')); r.NetWeight = HRM.num(HRM.val('txtWeightDetail'));
        r.CostMTon = HRM.num(HRM.val('txtCostMTon'));
        r.RateUOMId = HRM.comboVal('CmbRateUom'); r.RateUOM = HRM.comboText('CmbRateUom').trim(); r.RateEquivalent = uomEq(r.RateUOMId);
        r.Amount = HRM.num(HRM.val('txtAmount'));
        if (withRemarks) r.Remarks = HRM.val('txtRemarksDetail').trim();                 // btnGridUpdate does not touch Remarks
        return r;
    }
    function DetailFormReset() {                                                        // Remarks and Weight(kg) are not cleared by the form
        updateDetailIndex = -1;
        HRM.setCombo('CmbItemName', 0); HRM.setVal('txtQtyMTon', ''); HRM.setCombo('CmbPackUom', 0); HRM.setVal('txtNoOfBags', '');
        HRM.setVal('txtCostMTon', ''); HRM.setCombo('CmbJobLot', 0); HRM.setCombo('CmbRateUom', 0); HRM.setVal('txtAmount', ''); HRM.setVal('txtitemDescription', '');
        HRM.show('btnAddinGrid', true); HRM.show('btnGridUpdate', false); HRM.show('btnCancel', false);
        calc.reset();
    }
    function grdDetail_DoubleClick(r, i) {
        updateDetailIndex = i;
        HRM.setCombo('CmbItemName', HRM.int(HRM.col(r, 'ItemId')));
        bindRateUomAndItemPackUom().then(function () {
            HRM.setCombo('CmbPackUom', HRM.int(HRM.col(r, 'PackSizeId')));
            HRM.setCombo('CmbRateUom', HRM.int(HRM.col(r, 'RateUOMId')));
            HRM.setVal('txtQtyMTon', HRM.str(HRM.col(r, 'QtyMTon'))); calc.weight();
            HRM.setVal('txtNoOfBags', HRM.str(HRM.col(r, 'NoOfBags')));
            HRM.setVal('txtCostMTon', HRM.str(HRM.col(r, 'CostMTon')));
            HRM.setVal('txtAmount', HRM.str(HRM.col(r, 'Amount')));
        });
        HRM.setVal('txtitemDescription', HRM.str(HRM.col(r, 'ItemDetail')));
        HRM.setCombo('CmbJobLot', HRM.int(HRM.col(r, 'JobLotId')));
        HRM.show('btnAddinGrid', false); HRM.show('btnGridUpdate', true); HRM.show('btnCancel', true);
    }
    P.btnAddinGrid = function () {                                                      // btnAddinGrid_Click
        if (!DetailFormValidation()) return;
        grd.add(entry({ Id: 0, RowVersionLong: 0 }, true));
        getTotalnetWeightFromGrid();
        DetailFormReset();
    };
    P.btnGridUpdate = function () {                                                     // btnGridUpdate_Click
        if (!DetailFormValidation() || updateDetailIndex < 0) return;
        grd.update(updateDetailIndex, entry(grd.rows()[updateDetailIndex], false));
        getTotalnetWeightFromGrid();
        DetailFormReset();
    };
    P.btnCancel = function () { DetailFormReset(); };

    // ------------------------------------------------------------------ header combos
    function ProformaNoFill(rows) {
        dtPerformaNo = rows || [];
        HRM.fill('CmbPerformaNo', dtPerformaNo, 'Id', 'ProformaNo', { zero: '', keep: true });
    }
    function fillCombos(d) {
        if (d.branches) BranchFill(d.branches);
        if (d.proformaNos) ProformaNoFill(d.proformaNos);
        if (d.suppliers) { HRM.fill('CmbImporterName', d.suppliers, 'Id', 'CompanyName', { zero: '', keep: true }); HRM.fill('CmbExporterName', d.suppliers, 'Id', 'CompanyName', { zero: '', keep: true }); }
        if (d.incoTerms) HRM.fill('CmbIncomeTerm', d.incoTerms, 'Id', 'Name', { zero: '', keep: true });
        if (d.paymentTerms) HRM.fill('CmbPaymentTermsNew', d.paymentTerms, 'Id', 'Name', { zero: '', keep: true });
        if (d.ports) { HRM.fill('cmbLoadingPort', d.ports, 'Id', 'Name', { zero: '', keep: true }); HRM.fill('cmbDestinationPort', d.ports, 'Id', 'Name', { zero: '', keep: true }); }
        if (d.currencies) HRM.fill('CmbFcyCode', d.currencies, 'Id', 'CurrencyName', { zero: '', keep: true });
        if (d.items) HRM.fill('CmbItemName', d.items, 'Id', 'ItemName', { zero: '', keep: true });
        if (d.jobLots) HRM.fill('CmbJobLot', d.jobLots, 'Id', 'JobLotDescription', { zero: '', keep: true });
        if (d.countries) HRM.fill('CmbGoodsOrigin', d.countries, 'Id', 'Description', { zero: '', keep: true });
    }
    function BranchFill(rows) {                                                        // BranchFill(): the user's branch when nothing chosen
        var keep = HRM.comboVal('CmbBranchName');
        HRM.fill('CmbBranchName', rows, 'BranchId', 'BranchName', { zero: '' });
        HRM.setCombo('CmbBranchName', keep && HRM.hasOption('CmbBranchName', keep) ? keep : userBranchId);
    }
    function CmbPerformaNo_ValueChanged() {                                             // the chosen Proforma No's date
        var id = HRM.comboVal('CmbPerformaNo'), d = '';
        dtPerformaNo.forEach(function (r) { if (HRM.int(HRM.col(r, 'Id')) === id) d = HRM.day(HRM.col(r, 'ProformaDate')); });
        HRM.setVal('txtPerformaDate', id > 0 ? d : HRM.today());
    }

    // ------------------------------------------------------------------ FormReset / ReadById
    function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnupdate', update); }
    function FormReset() {
        AT = ImpB.attachmentState(); LstRemoveRecordDetail = [];
        buttons(false); UpdateMode = false; RecId = 0;
        HRM.setCombo('CmbImporterName', 0); HRM.setCombo('CmbExporterName', 0); HRM.setCombo('CmbPerformaNo', 0);
        HRM.setVal('txtPerformaDate', HRM.today());
        HRM.setCombo('CmbGoodsOrigin', 0); HRM.setCombo('cmbLoadingPort', 0); HRM.setCombo('cmbDestinationPort', 0); HRM.setCombo('CmbIncomeTerm', 0);
        HRM.setVal('txtNetWeightHeader', ''); HRM.setVal('txtGrossWeightHeader', ''); HRM.setVal('txtNoOfContainerHeader', '');
        HRM.setCombo('CmbPaymentTermsNew', 0); HRM.setCombo('CmbFcyCode', 0); HRM.setVal('txtfcyAmountHeader', '');
        HRM.enable('CmbPerformaNo', true);
        grd.clear();
        DetailFormReset();
        HRM.focus(branchFeature ? 'CmbBranchName' : 'datValidityUpto');
        return HRM.get(API + '/reset').then(function (d) { HRM.setVal('DocNo', HRM.str(d.docNo)); ProformaNoFill(d.proformaNos); }).catch(HRM.fail);
    }
    function ReadById(id) {
        buttons(true);
        RecId = id;
        return HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (d) {
            var h = d.header || {};
            ImpB.showTab('tc1', 'tabPage1');
            HRM.setCombo('CmbBranchName', HRM.int(HRM.col(h, 'branchId')));
            ProformaNoFill(d.proformaNos);
            HRM.setVal('DocNo', HRM.str(HRM.col(h, 'docNo')));
            HRM.setVal('datValidityUpto', HRM.day(HRM.col(h, 'validityUpto')));
            HRM.setCombo('CmbImporterName', HRM.int(HRM.col(h, 'importerId')));
            HRM.setCombo('CmbExporterName', HRM.int(HRM.col(h, 'exporterId')));
            HRM.setCombo('CmbPerformaNo', HRM.int(HRM.col(h, 'proformaMasterId')));
            HRM.setVal('txtPerformaDate', HRM.day(HRM.col(h, 'docDate')));
            HRM.enable('CmbPerformaNo', false); HRM.enable('txtPerformaDate', false);
            HRM.setCombo('CmbGoodsOrigin', HRM.int(HRM.col(h, 'GoodsOriginId')));
            HRM.setCombo('cmbLoadingPort', HRM.int(HRM.col(h, 'loadingPortId')));
            HRM.setCombo('cmbDestinationPort', HRM.int(HRM.col(h, 'destinationPortId')));
            HRM.setCombo('CmbIncomeTerm', HRM.int(HRM.col(h, 'incoTermId')));
            HRM.setVal('txtGrossWeightHeader', ImpB.fmt(HRM.col(h, 'grossWeightTotal'), 4, 0));
            HRM.setVal('txtNetWeightHeader', ImpB.fmt(HRM.col(h, 'netWeightTotal'), 4, 0));
            HRM.setVal('txtNoOfContainerHeader', HRM.str(HRM.col(h, 'fclTotal')));
            HRM.setCombo('CmbPaymentTermsNew', HRM.int(HRM.col(h, 'paymentTermId')));
            HRM.setCombo('CmbFcyCode', HRM.int(HRM.col(h, 'currencyId')));
            HRM.setVal('txtfcyAmountHeader', ImpB.fmt(HRM.col(h, 'fcyAmountTotal'), 4, 0));
            HRM.setVal('txtTermsAndConditions', HRM.str(HRM.col(h, 'TermsConditions')));
            grd.set(d.details || []);
            AT = ImpB.attachmentState(); AT.existing = d.attachments || [];
            LstRemoveRecordDetail = [];
            UpdateMode = true;
            HRM.focus('datValidityUpto');
        }).catch(HRM.fail);
    }

    // ------------------------------------------------------------------ Insert
    function formvalidation() {
        if (branchFeature && !HRM.comboVal('CmbBranchName')) { HRM.box('BranchName Is required'); HRM.focus('CmbBranchName'); return false; }
        if (HRM.val('DocNo') === '' || HRM.int(HRM.val('DocNo')) === 0) { HRM.box('Doc No Is Required'); HRM.focus('DocNo'); return false; }
        if (!HRM.comboVal('CmbImporterName')) { HRM.box('Importer Is Required'); HRM.focus('CmbImporterName'); return false; }
        if (!HRM.comboVal('CmbExporterName')) { HRM.box('Exporter Is Required'); HRM.focus('CmbExporterName'); return false; }
        if (!HRM.comboVal('CmbPerformaNo')) { HRM.box('Proforma No Is Required'); HRM.focus('CmbPerformaNo'); return false; }
        if (!HRM.comboVal('CmbGoodsOrigin')) { HRM.box('Goods Origin Is Required'); HRM.focus('CmbGoodsOrigin'); return false; }
        if (!HRM.comboVal('cmbLoadingPort')) { HRM.box('Loading Port Is Required'); HRM.focus('cmbLoadingPort'); return false; }
        if (!HRM.comboVal('cmbDestinationPort')) { HRM.box('Destination Port Is Required'); HRM.focus('cmbDestinationPort'); return false; }
        if (!HRM.comboVal('CmbIncomeTerm')) { HRM.box('Inco Term Is Required'); HRM.focus('CmbIncomeTerm'); return false; }
        if (!(HRM.num(HRM.val('txtNetWeightHeader')) > 0)) { HRM.box('Net Weight  Is Required'); HRM.focus('txtNetWeightHeader'); return false; }
        if (!(HRM.num(HRM.val('txtGrossWeightHeader')) >= HRM.num(HRM.val('txtNetWeightHeader')))) { HRM.box('Gross Weight Must Be Equal Or Greater Than Net Weight Thank You'); HRM.focus('txtGrossWeightHeader'); return false; }
        if (HRM.num(HRM.val('txtNoOfContainerHeader')) === 0) { HRM.box('Fcl Qty Field Is Required'); HRM.focus('txtNoOfContainerHeader'); return false; }
        if (!HRM.comboVal('CmbPaymentTermsNew')) { HRM.box('Payment Term Is Required'); HRM.focus('CmbPaymentTermsNew'); return false; }
        if (!HRM.comboVal('CmbFcyCode')) { HRM.box('Currency Code Is Required'); HRM.focus('CmbFcyCode'); return false; }
        if (HRM.num(HRM.val('txtfcyAmountHeader')) === 0) { HRM.box('Fcy Amount  Is Required'); HRM.focus('txtfcyAmountHeader'); return false; }
        return true;
    }
    function Insert(btn) {
        if (!formvalidation()) return;
        if (!grd.rows().length) { HRM.box('Detail Grid Record Not Found. Please Check! '); return; }
        if (!HRM.ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
        HRM.setVal('txtNetWeightHeader', ImpB.fmt(grd.sum('QtyMTon') * 1000, 3, 0));
        var body = {
            update: RecId > 0, recId: RecId, attachments: ImpB.attachmentPayload(AT), removed: LstRemoveRecordDetail, rows: grd.rows(),
            header: {
                branchId: HRM.comboVal('CmbBranchName'), docNo: HRM.val('DocNo'), validityUpto: HRM.val('datValidityUpto'),
                importerId: HRM.comboVal('CmbImporterName'), exporterId: HRM.comboVal('CmbExporterName'), proformaMasterId: HRM.comboVal('CmbPerformaNo'),
                docDate: HRM.val('txtPerformaDate'), GoodsOriginId: HRM.comboVal('CmbGoodsOrigin'), loadingPortId: HRM.comboVal('cmbLoadingPort'),
                destinationPortId: HRM.comboVal('cmbDestinationPort'), incoTermId: HRM.comboVal('CmbIncomeTerm'),
                grossWeightTotal: HRM.val('txtGrossWeightHeader'), fclTotal: HRM.val('txtNoOfContainerHeader'), paymentTermId: HRM.comboVal('CmbPaymentTermsNew'),
                currencyId: HRM.comboVal('CmbFcyCode'), TermsConditions: HRM.val('txtTermsAndConditions')
            }
        };
        var preview = HRM.checked('ChkPrintPreview');
        return HRM.busy(btn, function () {
            return HRM.post(API + '/save', body).then(function (d) {
                HRM.box(d && d.message);
                var id = HRM.int(d && d.id);
                return FormReset().then(function () { if (preview) InvoiceSlip(id); });
            }).catch(HRM.fail);
        }, 'proforma-save');
    }
    function InvoiceSlip(id, btn) { return ImpB.print(API, id, btn); }                // CommonServices.ProformaInvoiceSlip900(PrintId)

    // ------------------------------------------------------------------ history
    var hist = new HRM.Grid('DataGridHistory', {
        columns: [
            { key: 'Edit', caption: 'Edit', width: 35, hidden: true, render: function () { return btnCell('edit', 'Edit'); } },
            { key: 'Slip', caption: 'Slip', width: 60, hidden: true, render: function () { return btnCell('slip', '900-Slip'); } },
            { key: 'Id', hidden: true }, { key: 'DocNo', caption: 'DocNo' }, { key: 'ValidityUpto', caption: 'ValidityUpto', type: 'date' },
            { key: 'Importer', caption: 'Importer' }, { key: 'Exporter', caption: 'Exporter' }, { key: 'ProformaNo', caption: 'ProformaNo' },
            { key: 'ProformaDate', caption: 'ProformaDate', type: 'date' }, { key: 'GoodsOrigin', caption: 'GoodsOrigin' },
            { key: 'LoadingPort', caption: 'LoadingPort' }, { key: 'DestinationPort', caption: 'DestinationPort' }, { key: 'IncoTerm', caption: 'IncoTerm' },
            n('GrossWeight', 'GrossWeight', 3, true), n('NetWeight', 'NetWeight', 3, true), { key: 'FclQty', caption: 'FclQty', type: 'int' },
            { key: 'PaymentTerm', caption: 'PaymentTerm' }, { key: 'Currency', caption: 'Currency' }, n('Amount', 'Amount', 3, true),
            { key: 'TermsAndConditions', caption: 'TermsAndConditions' }, { key: 'EntryDate', caption: 'EntryDate', type: 'datetime' },
            { key: 'EntryUser', caption: 'EntryUser' }, { key: 'ModifyDate', caption: 'ModifyDate', type: 'datetime' }, { key: 'ModifyUser', caption: 'ModifyUser' },
            { key: 'IsApproved', caption: 'IsApproved', type: 'check' }, { key: 'ApprovedDate', caption: 'ApprovedDate', type: 'datetime' },
            { key: 'ApprovedUser', caption: 'ApprovedUser' }, { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'code' }
        ],
        filterRow: true, totals: true,
        onDouble: function (r) { ReadById(HRM.int(HRM.col(r, 'Id'))); },                                // DataGridHistory_DoubleClick
        onCode: function (r) { showAttachments(HRM.int(HRM.col(r, 'Id'))); },                            // DataGridHistory_LinkClicked
        onSelect: function (r) { GetDetailByHeaderId(HRM.int(HRM.col(r, 'Id'))); }                       // DataGridHistory_SelectionChanged
    });
    HRM.$('DataGridHistory').addEventListener('click', function (e) {                                     // DataGridHistory_ColumnButtonClick
        var b = e.target.closest('button[data-act]'); if (!b) return;
        var tr = b.closest('tr[data-i]'); if (!tr) return;
        var id = HRM.int(HRM.col(hist.rows()[+tr.getAttribute('data-i')], 'Id'));
        if (b.getAttribute('data-act') === 'slip') InvoiceSlip(id, b); else ReadById(id);
    });
    HRM.$('DataGridHistory').addEventListener('keydown', function (e) {                                   // DataGridHistory_KeyDown
        var r = hist.current(); if (!r) return;
        if (e.ctrlKey && e.key === 'Enter' && rights.update) { e.preventDefault(); ReadById(HRM.int(HRM.col(r, 'Id'))); }
    });
    var histDetail = new HRM.Grid('gridDetailHistory', {
        columns: [
            { key: 'Id', hidden: true }, { key: 'ItemCode', caption: 'ItemCode' }, { key: 'ItemName', caption: 'ItemName' }, { key: 'ItemDetail', caption: 'ItemDetail' },
            { key: 'JobLot', caption: 'JobLot' }, n('QtyMTon', 'Qty/M.Ton', 3, true), { key: 'PackSize', caption: 'PackSize' },
            n('NoOfBags', 'NoOfBags', 3, true), n('NetWeight', 'NetWeight', 3, true), n('CostMTon', 'Cost/M.Ton', 4, false),
            { key: 'RateUOM', caption: 'RateUom' }, n('Amount', 'Amount', 3, true), { key: 'Remarks', caption: 'Remarks' }
        ],
        totals: true
    });
    var detSeq = 0;
    function GetDetailByHeaderId(id) {
        var s = ++detSeq;
        return HRM.get(API + '/history-detail', { id: id }).then(function (rows) { if (s === detSeq && (rows || []).length) histDetail.set(rows); }).catch(HRM.fail);
    }
    function showAttachments(id) {                                       // CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 902): this record's attachments
        HRM.get(API + '/attachments', { id: id }).then(function (rows) {
            var st = ImpB.attachmentState(); st.existing = rows || [];
            ImpB.attachments({ state: st, fileUrl: function (a) { return API + '/attachment-file?id=' + id + '&attachmentId=' + HRM.int(HRM.col(a, 'Id')); } });
        }).catch(HRM.fail);
    }
    function HistoryGridFill() {
        var f = ImpB.historyFilter('h');
        f.fromDocNo = HRM.val('FromDocNoHistory'); f.toDocNo = HRM.val('ToDocNoHistory'); f.importerId = HRM.comboVal('CmbCustomerHistory');
        return HRM.post(API + '/history', f).then(function (rows) {
            hist.set(rows || []);
            if (!(rows || []).length) histDetail.clear();
        }).catch(HRM.fail);
    }

    // ------------------------------------------------------------------ toolbar
    P.btnNew = function () { return FormReset(); };
    P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };
    P.btnupdate = function (btn) { if (RecId === 0) { HRM.box('Rec Id not found...'); return; } return Insert(btn || 'btnupdate'); };
    P.btnRefresh = function (btn) {
        return HRM.busy(btn || 'btnRefresh', function () { return HRM.get(API + '/refresh', { recId: RecId }).then(fillCombos).catch(HRM.fail); });
    };
    P.btnAttachment = function () { ImpB.attachments({ state: AT, fileUrl: function (a) { return API + '/attachment-file?id=' + RecId + '&attachmentId=' + HRM.int(HRM.col(a, 'Id')); } }); };
    P.btnPrint = function (btn) { return InvoiceSlip(RecId, btn || 'btnPrint'); };
    P.BtnDefinePerformaNo = function () {                                                  // frmProformaDocumentSerial.ShowDialog(); ProformaNoFill(BindProformaNo())
        ImpB.serialDialog({ kind: 'proforma', api: API + '/proforma-serials', onClose: function () {
            HRM.get(API + '/refresh', { recId: RecId }).then(function (d) { ProformaNoFill(d.proformaNos); }).catch(HRM.fail);
        } });
    };
    P.btnShowHistory = function (btn) { return HRM.busy(btn || 'btnShowHistory', HistoryGridFill); };
    P.btnResetHistory = function () {
        HRM.setVal('hFromDate', HRM.addDays(HRM.today(), -3)); HRM.setVal('hToDate', HRM.today());
        HRM.setVal('FromDocNoHistory', ''); HRM.setVal('ToDocNoHistory', ''); HRM.setCombo('CmbCustomerHistory', 0);
        hist.clear(); histDetail.clear();
    };
    P.btnRefreshHistory = function (btn) {
        return HRM.busy(btn || 'btnRefreshHistory', function () {
            return HRM.get(API + '/history-combos').then(function (rows) { HRM.fill('CmbCustomerHistory', rows, 'Id', 'name', { zero: '', keep: true }); }).catch(HRM.fail);
        });
    };
    P.btnShortCutKey = function () {
        ImpB.shortcuts([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'],
            ['Ctrl+L', 'For Load All Records'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowRight', 'For Moving In Detail Tabs'],
            ['Ctrl+ArrowDown', 'For Focus On Selected Tab Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On First Entry of Detail Selected Tab'],
            ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    };

    // ------------------------------------------------------------------ events
    ImpB.tabs();
    ImpB.onLeave('CmbItemName', bindRateUomAndItemPackUom);
    HRM.$('CmbPerformaNo').addEventListener('change', CmbPerformaNo_ValueChanged);
    ImpB.wireDateChecks('h');
    function onForm() { return ImpB.activeTab('tc1') === 'tabPage1'; }
    HRM.keys({                                                                                      // ImProformaInvoice_KeyDown
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+alt+alt': P.btnShortCutKey, 'ctrl+alt+control': P.btnShortCutKey,
        'ctrl+t': function () { if (onForm()) { ImpB.showTab('tc1', 'tabPage2'); HRM.focus('hFromDate'); } else { ImpB.showTab('tc1', 'tabPage1'); HRM.focus(branchFeature ? 'CmbBranchName' : 'CmbExporterName'); } },
        'ctrl+s': function () { if (!onForm()) { P.btnShowHistory(); return; } if (!UpdateMode && !HRM.$('btnsave').disabled) P.btnsave(); },
        // Ctrl+U on the desktop calls btnsave_Click (RecId = 0) and re-inserts every detail line - it goes to Update here (see the report)
        'ctrl+u': function () { if (onForm() && UpdateMode && !HRM.$('btnupdate').disabled) P.btnupdate(); },
        'ctrl+n': function () { if (onForm()) P.btnNew(); else P.btnResetHistory(); },
        'ctrl+p': function () { if (onForm() && rights.print) P.btnPrint(); },
        'ctrl+r': function () { if (onForm()) P.btnRefresh(); else P.btnRefreshHistory(); },
        'ctrl+f10': function () { if (onForm()) P.btnAttachment(); },
        'ctrl+f5': function () { if (onForm()) HRM.focus(branchFeature ? 'CmbBranchName' : 'CmbExporterName'); else HRM.focus('hFromDate'); },
        'ctrl+arrowup': function () { if (onForm()) HRM.focus('CmbItemName'); else HRM.focus('hFromDate'); },
        'ctrl+arrowright': function () { if (onForm()) HRM.focus('CmbItemName'); }
    });
    HRM.footer(function (b) { ImpB.showTab('tc1', 'tabPage2'); return HRM.busy(b, HistoryGridFill); });

    // ------------------------------------------------------------------ InitializeComponentMethod / Load
    HRM.setVal('datValidityUpto', HRM.today()); HRM.setVal('txtPerformaDate', HRM.today());
    HRM.loading(HRM.get(API + '/setup')).then(function (d) {
        rights = d.rights || {};
        HRM.applyRights(rights, { save: 'btnsave', update: 'btnupdate', print: 'btnPrint' });
        hist.columnOf('Edit').hidden = !rights.update; hist.columnOf('Slip').hidden = !rights.print; hist._build();   // HistoryGridSettings: Edit / Slip by right
        branchFeature = !!d.branchFeature; userBranchId = HRM.int(d.userBranchId); historyDays = d.historyDays;
        HRM.setVal('DocNo', HRM.str(d.docNo));
        fillCombos(d);
        HRM.fill('CmbCustomerHistory', d.historyImporters, 'Id', 'name', { zero: '', keep: true });
        ImpB.historyDefaults('h', historyDays);
        buttons(false);
        var id = HRM.int(HRM.param('id'));
        if (id > 0) ReadById(id);
    }).catch(HRM.fail);
})();
