/* ============================================================================================
 * countx_imports_import_invoice.js - 783 Import Invoice, ported from Architecture.WinApp.Import.frmImportInvoice.
 * Built on countx_hrm.js (HRM) and countx_imports_impb_core.js (ImpB); exports window.ImpBInvoice.
 * Each handler names the form method it follows. Desktop quirks kept on purpose are marked "(desktop)".
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/import/import-invoice';
    var P = {}; window.ImpBInvoice = P;
    var RecId = 0, UpdateMode = false, rights = {}, branchFeature = false, financialActive = false, multi = false, userBranchId = 0, historyDays = 0;
    var dtPerformaNo = [], dtInvoiceAndLcNo = [], dtFiList = [], dtUom = [], AT = ImpB.attachmentState();
    var updateDetailIndex = -1, updateDetailIndexOtherItem = -1, updateDetailIndexPT = -1;
    var LstRemoveRecordDetail = [], LstRemoveRecordOtherItem = [], LstRemoveRecordPaymentTerm = [];

    function btnCell(act, text) { return '<button type="button" class="win-btn-action impb-cell-btn" data-act="' + act + '">' + text + '</button>'; }
    function n(key, caption, dec, sum) { return { key: key, caption: caption, type: 'num', sum: !!sum, decimals: dec, render: function (v) { return HRM.esc(ImpB.fmt(v, dec, 0)); } }; }
    function del() { return { key: 'Delete', caption: 'X', width: 20, render: function () { return btnCell('delete', 'X'); } }; }
    function onGridButton(tableId, grid, fn) {
        HRM.$(tableId).addEventListener('click', function (e) {
            var b = e.target.closest('button[data-act]'); if (!b) return;
            var tr = b.closest('tr[data-i]'); if (!tr) return;
            var i = +tr.getAttribute('data-i'); grid.select(i); fn(i, b.getAttribute('data-act'), b);
        });
    }
    function onGridKeys(tableId, grid, dbl, remove) {
        HRM.$(tableId).addEventListener('keydown', function (e) {
            var i = grid.currentIndex(); if (i < 0) return;
            if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); dbl(grid.rows()[i], i); }
            else if (e.ctrlKey && e.key === 'Delete') { e.preventDefault(); remove(i); }
        });
    }

    // ============================================================ Invoice Detail
    var grd = new HRM.Grid('grdDetail', {
        columns: [del(), { key: 'Id', hidden: true }, { key: 'ItemId', hidden: true }, { key: 'ItemName', caption: 'ItemName' }, { key: 'ItemCode', caption: 'ItemCode' },
            { key: 'ItemDetail', caption: 'ItemDetail' }, { key: 'JobLotId', hidden: true }, { key: 'JobLot', caption: 'JobLot' }, n('QtyMTon', 'Qty/M.Ton', 3, true),
            { key: 'PackSizeId', hidden: true }, { key: 'PackSize', caption: 'PackSize' }, { key: 'PackEquivalent', hidden: true }, n('NoOfBags', 'NoOfBags', 3, true),
            n('NetWeight', 'NetWeight', 3, true), n('CostMTon', 'Cost/M.Ton', 4, false), { key: 'RateUOMId', hidden: true }, { key: 'RateUOM', caption: 'RateUOM' },
            { key: 'RateEquivalent', hidden: true }, n('Amount', 'Amount', 3, true), { key: 'Remarks', caption: 'Remarks' }, n('OtherItemAmount', 'OtherItemAmount', 3, true),
            { key: 'RowVersionLong', hidden: true }],
        totals: true, onDouble: function (r, i) { grdDetail_DoubleClick(r, i); }
    });
    onGridButton('grdDetail', grd, function (i) { DeleteDetailGridRow(i); getTotalnetWeightFromGrid(); ProportionateOtherItemAmount(); });
    onGridKeys('grdDetail', grd, function (r, i) { grdDetail_DoubleClick(r, i); }, function (i) { DeleteDetailGridRow(i); getTotalnetWeightFromGrid(); ProportionateOtherItemAmount(); });
    function DeleteDetailGridRow(i) {
        if (updateDetailIndex !== -1) { HRM.box('Reset the Detail First'); return; }
        if (grd.rows().length <= 1) return;
        var r = grd.rows()[i];
        if (HRM.int(HRM.col(r, 'Id')) > 0) { if (!HRM.ask('Are you sure to Delete?')) return; LstRemoveRecordDetail.push(r); }
        grd.remove(i);
    }
    /** getTotalnetWeightFromGrid(): Net MTon's = Σ Qty/M.Ton, No.Of Packages = Σ NoOfBags, Invoice Value = Σ Amount. */
    function getTotalnetWeightFromGrid() {
        if (grd.rows().length > 0) {
            HRM.setVal('txtNetWeightHeader', ImpB.fmt(grd.sum('QtyMTon'), 5, 0));
            HRM.setVal('txtNoOfPackagesHeader', ImpB.fmt(grd.sum('NoOfBags'), 5, 0));
            setFcy(ImpB.fmt(grd.sum('Amount'), 3, 0));
        } else { HRM.setVal('txtNetWeightHeader', ''); setFcy(''); }
    }
    /** txtfcyAmountHeader.Text = v -> txtfcyAmount_TextChanged: every payment row's FcyAmount = %OfTotal × Invoice Value / 100. */
    function setFcy(v) {
        HRM.setVal('txtfcyAmountHeader', v);
        var total = HRM.num(v);
        if (total > 0 && pay.rows().length > 0) { pay.rows().forEach(function (r) { r.FcyAmount = HRM.num(r.PctOfTotal) * total / 100; }); pay.draw(); }
    }
    /** ProportionateOtherItemAmount(): OtherItemAmount = OtherItems total / contract total × row amount (0 without other items). */
    function ProportionateOtherItemAmount() {
        var contract = grd.sum('Amount'), other = oth.sum('Amount');
        grd.rows().forEach(function (r) { r.OtherItemAmount = other > 0 ? other / contract * HRM.num(r.Amount) : 0; });
        grd.draw();
    }

    var calc = ImpB.qtyEntry({ uoms: function () { return dtUom; }, importInvoice: true });
    function bindRateUomAndItemPackUom() {
        var pack = HRM.comboText('CmbPackUom'), rate = HRM.comboText('CmbRateUom');
        return HRM.get(API + '/uoms', { itemId: HRM.comboVal('CmbItemName') }).then(function (rows) {
            dtUom = rows || [];
            HRM.fill('CmbPackUom', dtUom, 'Id', 'UOMCode', { zero: '' }); HRM.fill('CmbRateUom', dtUom, 'Id', 'UOMCode', { zero: '' });
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
        r.ItemDetail = HRM.val('txtitemDescription').trim(); r.JobLotId = HRM.comboVal('CmbJobLot'); r.JobLot = HRM.comboText('CmbJobLot').trim();
        r.QtyMTon = HRM.num(HRM.val('txtQtyMTon')); r.PackSizeId = HRM.comboVal('CmbPackUom'); r.PackSize = HRM.comboText('CmbPackUom').trim(); r.PackEquivalent = uomEq(r.PackSizeId);
        r.NoOfBags = HRM.num(HRM.val('txtNoOfBags')); r.NetWeight = HRM.num(HRM.val('txtWeightDetail')); r.CostMTon = HRM.num(HRM.val('txtCostMTon'));
        r.RateUOMId = HRM.comboVal('CmbRateUom'); r.RateUOM = HRM.comboText('CmbRateUom').trim(); r.RateEquivalent = uomEq(r.RateUOMId);
        r.Amount = HRM.num(HRM.val('txtAmount'));
        if (withRemarks) r.Remarks = HRM.val('txtRemarksDetail').trim();           // btnGridUpdate leaves Remarks (desktop)
        return r;
    }
    function DetailFormReset() {
        updateDetailIndex = -1;
        HRM.setCombo('CmbItemName', 0); HRM.setVal('txtQtyMTon', ''); HRM.setCombo('CmbPackUom', 0); HRM.setVal('txtNoOfBags', ''); HRM.setVal('txtCostMTon', '');
        HRM.setCombo('CmbJobLot', 0); HRM.setCombo('CmbRateUom', 0); HRM.setVal('txtAmount', ''); HRM.setVal('txtitemDescription', '');
        HRM.show('btnAddinGrid', true); HRM.show('btnGridUpdate', false); HRM.show('btnCancel', false);
        ImpB.showTab('tc2', 'tabPage3'); calc.reset();
    }
    function grdDetail_DoubleClick(r, i) {
        updateDetailIndex = i;
        HRM.setCombo('CmbItemName', HRM.int(HRM.col(r, 'ItemId')));
        bindRateUomAndItemPackUom().then(function () {
            HRM.setCombo('CmbPackUom', HRM.int(HRM.col(r, 'PackSizeId'))); HRM.setCombo('CmbRateUom', HRM.int(HRM.col(r, 'RateUOMId')));
            HRM.setVal('txtQtyMTon', HRM.str(HRM.col(r, 'QtyMTon'))); calc.weight();
            HRM.setVal('txtNoOfBags', HRM.str(HRM.col(r, 'NoOfBags'))); HRM.setVal('txtCostMTon', HRM.str(HRM.col(r, 'CostMTon'))); HRM.setVal('txtAmount', HRM.str(HRM.col(r, 'Amount')));
        });
        HRM.setVal('txtitemDescription', HRM.str(HRM.col(r, 'ItemDetail'))); HRM.setCombo('CmbJobLot', HRM.int(HRM.col(r, 'JobLotId')));
        HRM.show('btnAddinGrid', false); HRM.show('btnGridUpdate', true); HRM.show('btnCancel', true);
    }
    P.btnAddinGrid = function () {
        if (!DetailFormValidation()) return;
        grd.add(entry({ Id: 0, OtherItemAmount: 0, RowVersionLong: 0 }, true));
        getTotalnetWeightFromGrid(); ProportionateOtherItemAmount(); DetailFormReset();
    };
    P.btnGridUpdate = function () {
        if (!DetailFormValidation() || updateDetailIndex < 0) return;
        grd.update(updateDetailIndex, entry(grd.rows()[updateDetailIndex], false));
        getTotalnetWeightFromGrid(); ProportionateOtherItemAmount(); DetailFormReset();
    };
    P.btnCancel = function () { DetailFormReset(); };

    // ============================================================ Other Items
    var oth = new HRM.Grid('grdotheritems', {
        columns: [del(), { key: 'Id', hidden: true }, { key: 'ItemId', hidden: true }, { key: 'ItemName', caption: 'ItemName', width: 350 },
            { key: 'Qty', caption: 'Qty', type: 'num', sum: true, decimals: 2, render: function (v) { return HRM.esc(ImpB.fmtInt(v)); } },
            { key: 'Rate', caption: 'Rate', type: 'num', render: function (v) { return HRM.esc(ImpB.fmtInt(v)); } },
            { key: 'Amount', caption: 'Amount', type: 'num', sum: true, decimals: 2, render: function (v) { return HRM.esc(ImpB.fmtInt(v)); } },
            { key: 'Remarks', caption: 'Remarks', width: 350 }, { key: 'RowVersionLong', hidden: true }],
        totals: true, onDouble: function (r, i) { grdotheritems_DoubleClick(r, i); }
    });
    onGridButton('grdotheritems', oth, function (i) { DeleteOtherItemDetailGridRow(i); TotalAmountCalculate(); ProportionateOtherItemAmount(); });
    onGridKeys('grdotheritems', oth, function (r, i) { grdotheritems_DoubleClick(r, i); }, function (i) { DeleteOtherItemDetailGridRow(i); });
    function DeleteOtherItemDetailGridRow(i) {
        if (updateDetailIndexOtherItem !== -1) { HRM.box('Reset the Detail First'); return; }
        var r = oth.rows()[i];
        if (HRM.int(HRM.col(r, 'Id')) > 0) { if (!HRM.ask('Are you sure to Delete?')) return; LstRemoveRecordOtherItem.push(r); }
        oth.remove(i);
    }
    /** TotalAmountCalculate(): Invoice Value += Σ other items when it is > 0 (added again on every call - desktop), then the payment rows. */
    function TotalAmountCalculate() {
        var fcy = HRM.num(HRM.val('txtfcyAmountHeader')), other = oth.sum('Amount');
        if (fcy > 0) setFcy(ImpB.fmt(fcy + other, 3, 0)); else setFcy(HRM.val('txtfcyAmountHeader'));
    }
    function OtherItemsCalculations() {
        var q = HRM.num(HRM.val('txtQtyOtherItem')), r = HRM.num(HRM.val('txtRateOtherItem'));
        HRM.setVal('txtAmountOtherItem', q > 0 && r > 0 ? String(q * r) : '0');
    }
    function OtherItemFormValidation() {
        if (!HRM.comboVal('CmbItemOtherItem')) { HRM.box('Item field required'); HRM.focus('CmbItemOtherItem'); return false; }
        if (HRM.num(HRM.val('txtRateOtherItem')) === 0) { HRM.box('Rate field required'); HRM.focus('txtRateOtherItem'); return false; }
        return true;
    }
    function ResetOtherItems() {
        updateDetailIndexOtherItem = -1;
        HRM.setCombo('CmbItemOtherItem', 0); HRM.setVal('txtQtyOtherItem', ''); HRM.setVal('txtRateOtherItem', ''); HRM.setVal('txtAmountOtherItem', ''); HRM.setVal('txtRemarksOtherItem', '');
        HRM.show('BtnAddOtherItem', true); HRM.show('BtnUpdateOtherItem', false); HRM.show('BtnCancelOtherItem', false);
    }
    function otherEntry(r) {
        r.ItemId = HRM.comboVal('CmbItemOtherItem'); r.ItemName = HRM.comboText('CmbItemOtherItem');
        r.Qty = HRM.num(HRM.val('txtQtyOtherItem')); r.Rate = HRM.num(HRM.val('txtRateOtherItem')); r.Amount = HRM.num(HRM.val('txtAmountOtherItem'));
        r.Remarks = HRM.val('txtRemarksOtherItem').trim();
        return r;
    }
    function grdotheritems_DoubleClick(r, i) {
        updateDetailIndexOtherItem = i;
        HRM.setCombo('CmbItemOtherItem', HRM.int(HRM.col(r, 'ItemId')));
        HRM.setVal('txtQtyOtherItem', HRM.str(HRM.col(r, 'Qty'))); HRM.setVal('txtRateOtherItem', HRM.str(HRM.col(r, 'Rate')));
        HRM.setVal('txtAmountOtherItem', HRM.str(HRM.col(r, 'Amount'))); HRM.setVal('txtRemarksOtherItem', HRM.str(HRM.col(r, 'Remarks')));
        HRM.show('BtnAddOtherItem', false); HRM.show('BtnUpdateOtherItem', true); HRM.show('BtnCancelOtherItem', true);
        HRM.focus('CmbItemOtherItem');
    }
    P.btnoadd = function () {
        if (!OtherItemFormValidation()) return;
        oth.add(otherEntry({ Id: 0, RowVersionLong: 0 }));
        ResetOtherItems(); TotalAmountCalculate(); ProportionateOtherItemAmount();
    };
    P.btnoUpdate = function () {
        if (!OtherItemFormValidation() || updateDetailIndexOtherItem < 0) return;
        oth.update(updateDetailIndexOtherItem, otherEntry(oth.rows()[updateDetailIndexOtherItem]));
        ResetOtherItems(); HRM.focus('txtQtyOtherItem'); TotalAmountCalculate(); ProportionateOtherItemAmount();
    };
    P.btnoCancel = function () { ResetOtherItems(); };

    // ============================================================ Payment Detail
    var pay = new HRM.Grid('grdpaymentdetail', {
        columns: [del(), { key: 'Id', hidden: true }, { key: 'PaymentTermId', hidden: true }, { key: 'PaymentTerm', caption: 'PaymentTerm', width: 350 },
            { key: 'DocumentTypeId', hidden: true }, { key: 'ExImEFormRegistrationId', hidden: true }, { key: 'FinancialInstrumentNo', caption: 'FinancialInstrumentNo', width: 350 },
            n('PctOfTotal', '%OfTotal', 2, true), n('FcyAmount', 'FcyAmount', 2, true), { key: 'DueDays', caption: 'DueDays' }, { key: 'Remarks', caption: 'Remarks' },
            { key: 'RowVersionLong', hidden: true }],
        totals: true, onDouble: function (r, i) { grdpaymentdetail_DoubleClick(r, i); }
    });
    onGridButton('grdpaymentdetail', pay, function (i) { DeletePaymentItemDetailGridRow(i); });
    onGridKeys('grdpaymentdetail', pay, function (r, i) { grdpaymentdetail_DoubleClick(r, i); }, function (i) { DeletePaymentItemDetailGridRow(i); });
    /** DeletePaymentItemDetailGridRow: the desktop deletes dtOtherItem.Rows[i] here - only the payment row is removed on the web (see the report). */
    function DeletePaymentItemDetailGridRow(i) {
        if (updateDetailIndexPT !== -1) { HRM.box('Reset the Detail First'); return; }
        if (pay.rows().length <= 1) return;
        var r = pay.rows()[i];
        if (HRM.int(HRM.col(r, 'Id')) > 0) { if (!HRM.ask('Are you sure to Delete?')) return; r.SortNo = i + 1; LstRemoveRecordPaymentTerm.push(r); }
        pay.remove(i);
    }
    function fiRow() { var id = HRM.comboVal('CmbFinInstrumentsPaymentDetail'), hit = null; dtFiList.forEach(function (r) { if (HRM.int(HRM.col(r, 'Id')) === id) hit = r; }); return hit; }
    function PaymentDetailFormValidation() {
        var pt = HRM.comboVal('CmbPaymentTermPaymentDetail');
        if (!pt) { HRM.box('Payment Term field required'); HRM.focus('CmbPaymentTermPaymentDetail'); return false; }
        if (pt === 1 && !HRM.comboVal('CmbFinInstrumentsPaymentDetail')) { HRM.box('Financial Instrument field is required against Advance'); return false; }
        if (HRM.num(HRM.val('dtxtpercentoftotal')) === 0 || HRM.val('dtxtpercentoftotal') === '') { HRM.box('Percent Field Required'); HRM.focus('dtxtpercentoftotal'); return false; }
        if (HRM.num(HRM.val('txtFcyAmountPaymentDetail')) === 0 || HRM.val('txtFcyAmountPaymentDetail') === '') { HRM.box('Fcy Amount  Field Required'); HRM.focus('txtFcyAmountPaymentDetail'); return false; }
        return true;
    }
    function duplicateFi(skip) {
        var fi = HRM.comboVal('CmbFinInstrumentsPaymentDetail'), row = fiRow();
        if (!fi || !row) return false;
        return pay.rows().some(function (r, i) { return i !== skip && HRM.int(r.ExImEFormRegistrationId) === fi && HRM.int(r.DocumentTypeId) === HRM.int(HRM.col(row, 'DocumnetTypeId')); });
    }
    function payEntry(r) {
        var fi = HRM.comboVal('CmbFinInstrumentsPaymentDetail'), row = fiRow();
        r.PaymentTermId = HRM.comboVal('CmbPaymentTermPaymentDetail'); r.PaymentTerm = HRM.comboText('CmbPaymentTermPaymentDetail');
        r.DocumentTypeId = fi > 0 && row ? HRM.int(HRM.col(row, 'DocumnetTypeId')) : 0;
        r.ExImEFormRegistrationId = fi; r.FinancialInstrumentNo = fi > 0 ? HRM.comboText('CmbFinInstrumentsPaymentDetail') : '';
        r.PctOfTotal = HRM.num(HRM.val('dtxtpercentoftotal')); r.FcyAmount = HRM.num(HRM.val('txtFcyAmountPaymentDetail'));
        r.DueDays = HRM.str(HRM.num(HRM.val('txtDueDaysPaymentDetail'))); r.Remarks = HRM.val('txtRemarksPaymentDetail').trim();
        return r;
    }
    function ResePaymentDetails() {                                                   // Remarks is not cleared (desktop)
        updateDetailIndexPT = -1;
        HRM.setCombo('CmbPaymentTermPaymentDetail', 0); HRM.setCombo('CmbFinInstrumentsPaymentDetail', 0);
        HRM.setVal('dtxtpercentoftotal', '0'); HRM.setVal('txtFcyAmountPaymentDetail', '0'); HRM.setVal('txtDueDaysPaymentDetail', '0');
        HRM.show('BtnAddPaymentDetail', true); HRM.show('BtnUpdatePaymentDetail', false); HRM.show('btnCancelPaymentDetail', false);
    }
    function grdpaymentdetail_DoubleClick(r, i) {
        updateDetailIndexPT = i;
        HRM.setCombo('CmbPaymentTermPaymentDetail', HRM.int(r.PaymentTermId)); HRM.setCombo('CmbFinInstrumentsPaymentDetail', HRM.int(r.ExImEFormRegistrationId));
        HRM.setVal('dtxtpercentoftotal', HRM.str(r.PctOfTotal)); HRM.setVal('txtFcyAmountPaymentDetail', HRM.str(r.FcyAmount));
        HRM.setVal('txtDueDaysPaymentDetail', HRM.str(r.DueDays)); HRM.setVal('txtRemarksPaymentDetail', HRM.str(r.Remarks));
        HRM.show('BtnAddPaymentDetail', false); HRM.show('BtnUpdatePaymentDetail', true); HRM.show('btnCancelPaymentDetail', true);
        HRM.focus('CmbPaymentTermPaymentDetail');
    }
    P.btndadd = function () {
        if (!PaymentDetailFormValidation()) return;
        if (duplicateFi(-1)) { HRM.box('Financial Instrument No already add in Grid Please Check'); return; }
        pay.add(payEntry({ Id: 0, RowVersionLong: 0 }));
        ResePaymentDetails(); HRM.focus('CmbPaymentTermPaymentDetail');
    };
    P.btndupdate = function () {
        if (!PaymentDetailFormValidation() || updateDetailIndexPT < 0) return;
        if (duplicateFi(updateDetailIndexPT)) { HRM.box('Financial Instrument No already add in Grid Please Check'); return; }
        pay.update(updateDetailIndexPT, payEntry(pay.rows()[updateDetailIndexPT]));
        ResePaymentDetails(); HRM.focus('CmbPaymentTermPaymentDetail');
    };
    P.btndcancel = function () { ResePaymentDetails(); };
    /** dcmbfino_ValueChanged: Payment Term = the FI's PaymenttermId; GetFinancialInstrumentsBalance ("0,0"). */
    function dcmbfino_ValueChanged() {
        var row = fiRow(); if (!row) return;
        HRM.setCombo('CmbPaymentTermPaymentDetail', HRM.int(HRM.col(row, 'PaymenttermId')));
        HRM.get(API + '/fi-balance', { documentTypeId: HRM.int(HRM.col(row, 'DocumnetTypeId')), id: HRM.int(HRM.col(row, 'Id')) })
            .then(function (d) { HRM.setVal('txtBalanceFIPaymentDetail', ImpB.fmtInt(d && d.balance)); }).catch(HRM.fail);
    }
    /** PaymentDetailPercentCalculate() (dtxtpercentoftotal Leave): Fcy Amount = Invoice Value × % / 100 ("#,#.###"). */
    function PaymentDetailPercentCalculate() {
        var fcy = HRM.num(HRM.val('txtfcyAmountHeader')), pct = HRM.num(HRM.val('dtxtpercentoftotal'));
        if (fcy > 0 && pct > 0) {
            if (pct > 100) { HRM.setVal('dtxtpercentoftotal', '0'); HRM.box('Percent cannot be greater than 100 Please check'); return; }
            HRM.setVal('txtFcyAmountPaymentDetail', ImpB.fmt(fcy * pct / 100, 3, 0));
        }
    }
    /** dtxtfcyamount_TextChanged (Fcy Amount Leave): % = Fcy Amount / Invoice Value × 100. */
    function dtxtfcyamount() {
        var fcy = HRM.num(HRM.val('txtfcyAmountHeader')), amt = HRM.num(HRM.val('txtFcyAmountPaymentDetail'));
        if (fcy > 0 && amt > 0) {
            var pct = amt / fcy * 100;
            HRM.setVal('dtxtpercentoftotal', String(pct));
            if (pct > 100) { HRM.setVal('dtxtpercentoftotal', '0'); HRM.box('Percent cannot be greater than 100 Please check'); }
        }
    }

    // ============================================================ header combos
    function fillCombos(d) {
        if (d.branches) { HRM.fill('CmbBranchName', d.branches, 'BranchId', 'BranchName', { zero: '', keep: true }); if (!HRM.comboVal('CmbBranchName')) HRM.setCombo('CmbBranchName', userBranchId); }
        if (d.proformaNos) { dtPerformaNo = d.proformaNos; HRM.fill('CmbPerformaNo', dtPerformaNo, 'Id', 'ProformaNo', { zero: '', keep: true }); }
        if (d.invoiceAndLc) InvoiceAndLcOrderNoFill(d.invoiceAndLc);
        if (d.suppliers) { HRM.fill('CmbImporterName', d.suppliers, 'Id', 'CompanyName', { zero: '', keep: true }); HRM.fill('CmbExporterName', d.suppliers, 'Id', 'CompanyName', { zero: '', keep: true }); }
        if (d.salePersons) HRM.fill('CmbSalesPerson', d.salePersons, 'Id', 'CompanyName', { zero: '', keep: true });
        if (d.incoTerms) HRM.fill('CmbIncomeTerm', d.incoTerms, 'Id', 'Name', { zero: '', keep: true });
        if (d.paymentTerms) { HRM.fill('CmbPaymentTermPaymentDetail', d.paymentTerms, 'Id', 'Name', { zero: '', keep: true }); HRM.fill('CmbPaymentTermsNew', d.paymentTerms, 'Id', 'Name', { zero: '', keep: true }); }
        if (d.ports) { HRM.fill('cmbLoadingPort', d.ports, 'Id', 'Name', { zero: '', keep: true }); HRM.fill('cmbDestinationPort', d.ports, 'Id', 'Name', { zero: '', keep: true }); }
        if (d.currencies) HRM.fill('CmbFcyCode', d.currencies, 'Id', 'CurrencyName', { zero: '', keep: true });
        if (d.items) HRM.fill('CmbItemName', d.items, 'Id', 'ItemName', { zero: '', keep: true });
        if (d.jobLots) HRM.fill('CmbJobLot', d.jobLots, 'Id', 'JobLotDescription', { zero: '', keep: true });
        if (d.debitAccounts) HRM.fill('CmbDabitAccount', d.debitAccounts, 'Id', 'AccountTitle', { zero: '', keep: true });
        if (d.importerBanks) HRM.fill('cmbimporterBankNew', d.importerBanks, 'Id', 'BranchName', { zero: '', keep: true });
        if (d.exporterBanks) HRM.fill('cmbexporterBankNew', d.exporterBanks, 'Id', 'BranchName', { zero: '', keep: true });
        if (d.otherItems) HRM.fill('CmbItemOtherItem', d.otherItems, 'Id', 'ItemName', { zero: '', keep: true });
        if (d.exportCompanies && multi) HRM.fill('CmbImportCompany', d.exportCompanies, 'Id', 'CompName', { zero: '', keep: true });
    }
    function InvoiceAndLcOrderNoFill(rows) {
        dtInvoiceAndLcNo = rows || [];
        HRM.fill('CmbContractNo', dtInvoiceAndLcNo, 'ContractId', 'ContractNo', { zero: '', keep: true });
        HRM.fill('cmbInvoiceNo', dtInvoiceAndLcNo, 'InvoiceId', 'InvoiceNo', { zero: '', keep: true });
    }
    function lcRow(key, id) { var hit = null; dtInvoiceAndLcNo.forEach(function (r) { if (!hit && HRM.int(HRM.col(r, key)) === id) hit = r; }); return hit; }
    function CmbContractNo_ValueChanged() {
        var r = lcRow('ContractId', HRM.comboVal('CmbContractNo'));
        if (r) { HRM.setVal('txtContractDate', HRM.day(HRM.col(r, 'ContractDate'))); HRM.setCombo('cmbInvoiceNo', HRM.int(HRM.col(r, 'InvoiceId'))); HRM.setVal('txtInvoiceDate', HRM.day(HRM.col(r, 'InvoiceDate'))); }
        else { HRM.setCombo('cmbInvoiceNo', 0); HRM.setVal('txtContractDate', HRM.today()); HRM.setVal('txtInvoiceDate', HRM.today()); }
    }
    function cmbInvoiceNo_ValueChanged() {
        var r = lcRow('InvoiceId', HRM.comboVal('cmbInvoiceNo'));
        if (r) { HRM.setVal('txtInvoiceDate', HRM.day(HRM.col(r, 'InvoiceDate'))); HRM.setCombo('CmbContractNo', HRM.int(HRM.col(r, 'ContractId'))); HRM.setVal('txtContractDate', HRM.day(HRM.col(r, 'ContractDate'))); }
        else { HRM.setCombo('CmbContractNo', 0); HRM.setVal('txtContractDate', HRM.today()); HRM.setVal('txtInvoiceDate', HRM.today()); }
    }
    function CmbPerformaNo_ValueChanged() {
        var id = HRM.comboVal('CmbPerformaNo'), d = '';
        dtPerformaNo.forEach(function (r) { if (HRM.int(HRM.col(r, 'Id')) === id) d = HRM.day(HRM.col(r, 'ProformaDate')); });
        HRM.setVal('txtPerformaDate', id > 0 ? d : HRM.today());
    }
    /** cmbSupCust_Leave: BindFinancialInstrument() + bindConsigneeAgainstCustomer() for the Exporter. */
    function cmbSupCust_Leave() {
        var exp = HRM.comboVal('CmbExporterName');
        if (!exp) { dtFiList = []; HRM.setCombo('CmbFinInstrumentsPaymentDetail', 0); HRM.setCombo('cmbConsignee', 0); return Promise.resolve(); }
        return HRM.get(API + '/by-exporter', { exporterId: exp }).then(applyExporter).catch(HRM.fail);
    }
    function applyExporter(d) {
        dtFiList = (d && d.financialInstruments) || [];
        if (dtFiList.length) HRM.fill('CmbFinInstrumentsPaymentDetail', dtFiList, 'Id', 'EFormNo', { zero: '', keep: true });
        var cons = (d && d.consignees) || [];
        if (cons.length) HRM.fill('cmbConsignee', cons, 'Id', 'CompanyName', { zero: '', keep: true }); else HRM.setCombo('cmbConsignee', HRM.comboVal('cmbConsignee'));
    }

    // ============================================================ FormReset / ReadById
    function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnupdate', update); }
    function FormReset() {
        AT = ImpB.attachmentState(); LstRemoveRecordDetail = []; LstRemoveRecordPaymentTerm = []; LstRemoveRecordOtherItem = [];
        buttons(false); UpdateMode = false; RecId = 0;
        HRM.fill('cmbStatus', [{ Id: 1, Status: 'Open' }, { Id: 2, Status: 'Complete' }, { Id: 3, Status: 'Cancel' }], 'Id', 'Status', { zero: false });
        HRM.enable('cmbStatus', false);
        HRM.setCombo('CmbImporterName', 0); HRM.setCombo('CmbExporterName', 0); cmbSupCust_Leave(); HRM.setCombo('cmbConsignee', 0);
        HRM.setCombo('CmbPerformaNo', 0); HRM.setVal('txtPerformaDate', HRM.today());
        HRM.setCombo('CmbContractNo', 0); HRM.setVal('txtContractDate', HRM.today()); HRM.setCombo('cmbInvoiceNo', 0); HRM.setVal('txtInvoiceDate', HRM.today());
        HRM.setVal('txtLotRef', '');
        HRM.enable('CmbPerformaNo', true); HRM.enable('CmbContractNo', true); HRM.enable('cmbInvoiceNo', true);
        HRM.setCombo('CmbIncomeTerm', 0); HRM.setCombo('CmbPaymentTermsNew', 0); HRM.setCombo('CmbFcyCode', 0);
        HRM.setVal('txtExRate', ''); HRM.setVal('txtfcyAmountHeader', ''); HRM.setVal('txtNoOfPackagesHeader', ''); HRM.setVal('txtNoOfContainerHeader', '');
        HRM.setVal('txtGrossWeightHeader', ''); HRM.setVal('txtNetWeightHeader', '');
        HRM.setCombo('cmbimporterBankNew', 0); HRM.setCombo('cmbexporterBankNew', 0); HRM.setCombo('cmbLoadingPort', 0); HRM.setCombo('cmbDestinationPort', 0);
        HRM.setVal('txtBLNo', ''); HRM.setVal('txtBlDate', HRM.today()); HRM.setVal('txtGdNo', ''); HRM.setVal('txtGdDate', HRM.today()); HRM.setVal('txtGdValue', '');
        grd.clear(); pay.clear(); oth.clear();
        DetailFormReset(); ResePaymentDetails(); ResetOtherItems();
        HRM.focus(branchFeature ? 'CmbBranchName' : 'CmbExporterName');
        return HRM.get(API + '/reset').then(function (d) { HRM.setVal('txtDocNo', HRM.str(d.docNo)); }).catch(HRM.fail);
    }
    function ReadById(id) {
        return FormReset().then(function () {
            buttons(true); RecId = id;
            return HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (d) {
                var h = d.header || {};
                ImpB.showTab('tc1', 'tabPage1');
                dtPerformaNo = d.proformaNos || []; HRM.fill('CmbPerformaNo', dtPerformaNo, 'Id', 'ProformaNo', { zero: '' });
                InvoiceAndLcOrderNoFill(d.invoiceAndLc);
                HRM.enable('CmbPerformaNo', false); HRM.enable('CmbContractNo', false); HRM.enable('cmbInvoiceNo', false);
                HRM.setVal('txtDocNo', HRM.str(HRM.col(h, 'docNo')));
                HRM.setCombo('CmbImporterName', HRM.int(HRM.col(h, 'importerId'))); HRM.setCombo('CmbExporterName', HRM.int(HRM.col(h, 'exporterId')));
                applyExporter(d);
                HRM.setCombo('CmbSalesPerson', HRM.int(HRM.col(h, 'salePersonId'))); HRM.setCombo('cmbConsignee', HRM.int(HRM.col(h, 'ConsigneeId')));
                HRM.setCombo('CmbPerformaNo', HRM.int(HRM.col(h, 'proformaMasterId'))); HRM.setVal('txtPerformaDate', HRM.day(HRM.col(h, 'proformaMasterDate')));
                HRM.setCombo('cmbInvoiceNo', HRM.int(HRM.col(h, 'invoiceMasterId'))); HRM.setVal('txtInvoiceDate', HRM.day(HRM.col(h, 'docDate')));
                var det = d.details || [];
                if (det.length) {
                    HRM.setCombo('CmbBranchName', HRM.int(det[0].locationBranchId));
                    HRM.setCombo('CmbContractNo', HRM.int(det[0].LocOrderMasterId));
                    if (det[0].locOrderDate) HRM.setVal('txtContractDate', HRM.day(det[0].locOrderDate));
                }
                HRM.setVal('txtLotRef', HRM.str(HRM.col(h, 'importerRefNo')));
                HRM.setCombo('CmbIncomeTerm', HRM.int(HRM.col(h, 'incoTermId'))); HRM.setCombo('CmbPaymentTermsNew', HRM.int(HRM.col(h, 'payementTermId')));
                HRM.setCombo('CmbFcyCode', HRM.int(HRM.col(h, 'currencyId')));
                HRM.setVal('txtExRate', ImpB.fmt(HRM.col(h, 'exchangeRate'), 4, 0));
                HRM.setVal('txtNoOfPackagesHeader', ImpB.fmt(HRM.col(h, 'NoOfPackages'), 4, 0));
                HRM.setVal('txtNoOfContainerHeader', HRM.str(HRM.col(h, 'fclTotal')));
                HRM.setVal('txtGrossWeightHeader', ImpB.fmt(HRM.col(h, 'grossWeightTotal'), 4, 0));
                HRM.setVal('txtNetWeightHeader', ImpB.fmt(HRM.col(h, 'netWeightTotal'), 4, 0));
                HRM.setCombo('cmbimporterBankNew', HRM.int(HRM.col(h, 'bankIdImporter'))); HRM.setCombo('cmbexporterBankNew', HRM.int(HRM.col(h, 'bankIdExporter')));
                HRM.setCombo('cmbLoadingPort', HRM.int(HRM.col(h, 'loadingPortId'))); HRM.setCombo('cmbDestinationPort', HRM.int(HRM.col(h, 'destinationPortId')));
                HRM.setVal('txtBLNo', HRM.str(HRM.col(h, 'BillOfLadingNo'))); HRM.setVal('txtBlDate', HRM.day(HRM.col(h, 'BillOfLadingDate')));
                HRM.setVal('txtGdNo', HRM.str(HRM.col(h, 'GoodsDeclarationNo'))); HRM.setVal('txtGdDate', HRM.day(HRM.col(h, 'GoodsDeclarationDate')));
                HRM.setVal('txtGdValue', ImpB.fmt(HRM.col(h, 'GoodsDeclarationValue'), 4, 0));
                if (financialActive) HRM.setCombo('CmbDabitAccount', HRM.int(HRM.col(h, 'DabitAccountId')));
                HRM.setCombo('CmbImportCompany', HRM.int(HRM.col(h, 'legalEntityId')));
                grd.set(det); oth.set(d.otherItems || []); pay.set(d.paymentTerms || []);
                HRM.setVal('txtfcyAmountHeader', ImpB.fmt(HRM.col(h, 'fcyAmountTotal'), 4, 0));
                AT = ImpB.attachmentState(); AT.existing = d.attachments || [];
                ProportionateOtherItemAmount();
                UpdateMode = true;
                HRM.focus('CmbImporterName');
            });
        }).catch(HRM.fail);
    }

    // ============================================================ Insert
    function formvalidation() {
        if (branchFeature && !HRM.comboVal('CmbBranchName')) { HRM.box('BranchName Is required'); HRM.focus('CmbBranchName'); return false; }
        if (HRM.val('txtDocNo') === '' || HRM.int(HRM.val('txtDocNo')) === 0) { HRM.box('Doc No Is Required'); HRM.focus('txtDocNo'); return false; }
        var st = HRM.comboText('cmbStatus');
        if ((HRM.visible('btnupdate') && st === '') || st === '0') { HRM.box('Status Is Required'); HRM.focus('cmbStatus'); return false; }
        if (!HRM.comboVal('CmbImporterName')) { HRM.box('Importer Is Required'); HRM.focus('CmbImporterName'); return false; }
        if (!HRM.comboVal('CmbExporterName')) { HRM.box('Exporter Is Required'); HRM.focus('CmbExporterName'); return false; }
        if (HRM.comboText('CmbContractNo') === '') { HRM.box('ContractNo No Is Required'); HRM.focus('CmbContractNo'); return false; }
        if (HRM.comboText('cmbInvoiceNo') === '') { HRM.box('Invoice No Is Required'); HRM.focus('cmbInvoiceNo'); return false; }
        if (!HRM.comboVal('cmbConsignee')) { HRM.box('Consignee Is Required'); HRM.focus('cmbConsignee'); return false; }
        if (!HRM.comboVal('CmbPaymentTermsNew')) { HRM.box('Payment Term Is Required'); HRM.focus('CmbPaymentTermsNew'); return false; }
        if (!HRM.comboVal('cmbLoadingPort')) { HRM.box('Loading Port Is Required'); HRM.focus('cmbLoadingPort'); return false; }
        if (!HRM.comboVal('cmbDestinationPort')) { HRM.box('Destination Port  Is Required'); HRM.focus('cmbDestinationPort'); return false; }
        if (!HRM.comboVal('CmbFcyCode')) { HRM.box('Fcy Code Is Required'); HRM.focus('CmbFcyCode'); return false; }
        if (HRM.num(HRM.val('txtfcyAmountHeader')) === 0) { HRM.box('Fcy Amount  Is Required'); HRM.focus('txtfcyAmountHeader'); return false; }
        if (HRM.int(HRM.val('txtNoOfContainerHeader')) === 0) { HRM.box('No.of Containers Field Is Required'); HRM.focus('txtNoOfContainerHeader'); return false; }
        if (!(HRM.num(HRM.val('txtNetWeightHeader')) > 0)) { HRM.box('Net Weight  Is Required'); HRM.focus('txtNetWeightHeader'); return false; }
        if (!(HRM.num(HRM.val('txtGrossWeightHeader')) >= HRM.num(HRM.val('txtNetWeightHeader')))) { HRM.box('Gross Weight Must Be Equal Or Greater Than Net Weight Thank You'); HRM.focus('txtGrossWeightHeader'); return false; }
        if (!(HRM.num(HRM.val('txtExRate')) > 0)) { HRM.box('ExchangeRate Field is Required'); HRM.focus('txtExRate'); return false; }
        if (financialActive && !HRM.comboVal('CmbDabitAccount') && !HRM.ask('You have not Selected Debit Account,Are you sure to Debit Value to Purchase Account?')) {
            HRM.box('Please select Debit Ac'); HRM.focus('CmbDabitAccount'); return false;
        }
        if (multi && !HRM.comboVal('CmbImportCompany')) { HRM.box('Export Company Is required'); HRM.focus('CmbImportCompany'); return false; }
        return true;
    }
    function Insert(btn) {
        if (!formvalidation()) return;
        if (!grd.rows().length) { ImpB.showTab('tc2', 'tabPage3'); HRM.box('Detail Grid Record Not Found. Please Check! '); return; }
        if (!pay.rows().length) { ImpB.showTab('tc2', 'tabPage5'); HRM.focus('CmbPaymentTermPaymentDetail'); HRM.box('Payment Detail Not Found. Please Check! '); return; }
        if (!HRM.ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
        var fcy = HRM.num(HRM.val('txtfcyAmountHeader')), used = 0;
        pay.rows().forEach(function (r) { used += HRM.num(r.FcyAmount); });
        if (ImpB.roundEven(fcy, 0) !== ImpB.roundEven(used, 0)) { HRM.box('Total Invoice Amount and Total Payment Utilize Amount Not equal Please Check'); return; }
        var body = {
            update: RecId > 0, recId: RecId, attachments: ImpB.attachmentPayload(AT),
            rows: grd.rows(), otherItems: oth.rows(), paymentTerms: pay.rows(),
            removedRows: LstRemoveRecordDetail, removedOtherItems: LstRemoveRecordOtherItem, removedPaymentTerms: LstRemoveRecordPaymentTerm,
            header: {
                branchId: HRM.comboVal('CmbBranchName'), docNo: HRM.val('txtDocNo'), docStatus: HRM.comboText('cmbStatus'),
                importerId: HRM.comboVal('CmbImporterName'), exporterId: HRM.comboVal('CmbExporterName'), salePersonId: HRM.comboVal('CmbSalesPerson'),
                ConsigneeId: HRM.comboVal('cmbConsignee'), proformaMasterId: HRM.comboVal('CmbPerformaNo'), proformaMasterDate: HRM.val('txtPerformaDate'),
                contractId: HRM.comboVal('CmbContractNo'), invoiceMasterId: HRM.comboVal('cmbInvoiceNo'), docDate: HRM.val('txtInvoiceDate'),
                importerRefNo: HRM.val('txtLotRef'), incoTermId: HRM.comboVal('CmbIncomeTerm'), payementTermId: HRM.comboVal('CmbPaymentTermsNew'),
                currencyId: HRM.comboVal('CmbFcyCode'), exchangeRate: HRM.val('txtExRate'), fcyAmountTotal: HRM.val('txtfcyAmountHeader'),
                detailNoOfBags: HRM.val('txtNoOfBags'), fclTotal: HRM.val('txtNoOfContainerHeader'), grossWeightTotal: HRM.val('txtGrossWeightHeader'),
                netWeightTotal: HRM.val('txtNetWeightHeader'), bankIdImporter: HRM.comboVal('cmbimporterBankNew'), bankIdExporter: HRM.comboVal('cmbexporterBankNew'),
                loadingPortId: HRM.comboVal('cmbLoadingPort'), destinationPortId: HRM.comboVal('cmbDestinationPort'),
                BillOfLadingDate: HRM.val('txtBlDate'), BillOfLadingNo: HRM.val('txtBLNo'), GoodsDeclarationDate: HRM.val('txtGdDate'),
                GoodsDeclarationNo: HRM.val('txtGdNo'), GoodsDeclarationValue: HRM.val('txtGdValue'), DabitAccountId: HRM.comboVal('CmbDabitAccount'),
                debitConfirmed: true, legalEntityId: HRM.comboVal('CmbImportCompany')
            }
        };
        var preview = HRM.checked('ChkPrintPreview');
        return HRM.busy(btn, function () {
            return HRM.post(API + '/save', body).then(function (d) {
                HRM.box(d && d.message);
                var id = HRM.int(d && d.id);
                return FormReset().then(function () { if (preview) InvoiceSlip(id); });
            }).catch(HRM.fail);
        }, 'invoice-save');
    }
    function InvoiceSlip(id, btn) { return ImpB.print(API, id, btn); }                   // CommonServices.ImportInvoiceSlip902(PrintId)

    // ============================================================ History
    var hist = new HRM.Grid('DataGridHistory', {
        columns: [
            { key: 'Edit', caption: 'Edit', width: 70, hidden: true, render: function () { return btnCell('edit', 'Edit'); } },
            { key: 'Slip', caption: 'Slip', width: 70, hidden: true, render: function () { return btnCell('slip', 'Slip'); } },
            { key: 'Id', hidden: true }, { key: 'DocNo', caption: 'DocNo' }, { key: 'InvoiceDate', caption: 'InvoiceDate', type: 'date' },
            { key: 'InvoiceNo', caption: 'InvoiceNo' }, { key: 'ProformaMasterId', hidden: true }, { key: 'ProformaNo', caption: 'ProformaNo' },
            { key: 'ProformaDate', caption: 'ProformaDate', type: 'date' }, { key: 'LcOrderMasterId', hidden: true }, { key: 'LcOrderNo', caption: 'LcOrderNo' },
            { key: 'LcOrderDate', caption: 'LcOrderDate', type: 'date' }, n('ExchangeRate', 'ExchangeRate', 4, false), n('FcyAmountTotal', 'FcyAmountTotal', 3, true),
            { key: 'NoOfPackages', caption: 'NoOfPackages', type: 'int' }, { key: 'FclTotal', caption: 'FclTotal', type: 'int' },
            n('GrossWeightTotal', 'GrossWeightTotal', 3, true), n('NetWeightTotal', 'NetWeightTotal', 3, true), { key: 'BLNo', caption: 'BLNo' },
            { key: 'BLDate', caption: 'BLDate', type: 'date' }, { key: 'GDNo', caption: 'GDNo' }, { key: 'GDDate', caption: 'GDDate', type: 'date' },
            n('GDValue', 'GDValue', 3, false), { key: 'DocStatus', caption: 'DocStatus' }, { key: 'RemarksHeader', caption: 'RemarksHeader' },
            { key: 'EntryDate', caption: 'EntryDate', type: 'datetime' }, { key: 'EntryUser', caption: 'EntryUser' }, { key: 'ModifyDate', caption: 'ModifyDate', type: 'datetime' },
            { key: 'ModifyUser', caption: 'ModifyUser' }, { key: 'IsApproved', caption: 'IsApproved', type: 'check' }, { key: 'ApprovedDate', caption: 'ApprovedDate', type: 'datetime' },
            { key: 'ApprovedUser', caption: 'ApprovedUser' }, { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'code' }, { key: 'RowVersionLong', hidden: true }
        ],
        filterRow: true, totals: true,
        onDouble: function (r) { ReadById(HRM.int(HRM.col(r, 'Id'))); },
        onCode: function (r) { showAttachments(HRM.int(HRM.col(r, 'Id'))); },
        onSelect: function (r) { GetDetailByHeaderId(HRM.int(HRM.col(r, 'Id'))); }
    });
    onGridButton('DataGridHistory', hist, function (i, act, b) {
        var id = HRM.int(HRM.col(hist.rows()[i], 'Id'));
        if (act === 'slip') InvoiceSlip(id, b); else ReadById(id);
    });
    HRM.$('DataGridHistory').addEventListener('keydown', function (e) {
        var r = hist.current(); if (!r) return;
        if (e.ctrlKey && e.key === 'Enter' && rights.update) { e.preventDefault(); ReadById(HRM.int(HRM.col(r, 'Id'))); }
    });
    var histDetail = new HRM.Grid('gridDetailHistory', {
        columns: [{ key: 'Id', hidden: true }, { key: 'ItemId', hidden: true }, { key: 'ItemName', caption: 'ItemName' }, { key: 'ItemCode', caption: 'ItemCode' },
            { key: 'ItemDetail', caption: 'ItemDetail' }, { key: 'JobLotId', hidden: true }, { key: 'JobLot', caption: 'JobLot' }, n('QtyMTon', 'Qty/M.Ton', 3, true),
            { key: 'PackSizeId', hidden: true }, { key: 'PackSize', caption: 'PackSize' }, { key: 'PackEquivalent', hidden: true }, n('NoOfBags', 'NoOfBags', 3, true),
            n('NetWeight', 'NetWeight', 3, true), n('CostMTon', 'Cost/M.Ton', 4, false), { key: 'RateUOMId', hidden: true }, { key: 'RateUOM', caption: 'RateUOM' },
            { key: 'RateEquivalent', hidden: true }, n('Amount', 'Amount', 3, true), { key: 'Remarks', caption: 'Remarks' }],
        totals: true
    });
    var detSeq = 0;
    function GetDetailByHeaderId(id) {
        var s = ++detSeq;
        return HRM.get(API + '/history-detail', { id: id }).then(function (rows) { if (s === detSeq && (rows || []).length) histDetail.set(rows); }).catch(HRM.fail);
    }
    function showAttachments(id) {
        HRM.get(API + '/attachments', { id: id }).then(function (rows) {
            var st = ImpB.attachmentState(); st.existing = rows || [];
            ImpB.attachments({ state: st, fileUrl: function (a) { return API + '/attachment-file?id=' + id + '&attachmentId=' + HRM.int(HRM.col(a, 'Id')); } });
        }).catch(HRM.fail);
    }
    function HistoryGridFill() {
        var f = ImpB.historyFilter('h');
        f.fromDocNo = HRM.val('FromDocNoHistory'); f.toDocNo = HRM.val('ToDocNoHistory'); f.supplierCustomerId = HRM.comboVal('CmbCustomerHistory');
        return HRM.post(API + '/history', f).then(function (rows) { hist.set(rows || []); if (!(rows || []).length) histDetail.clear(); }).catch(HRM.fail);
    }

    // ============================================================ toolbar
    P.btnNew = function () { return FormReset(); };
    P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };
    P.btnupdate = function (btn) { if (RecId === 0) { HRM.box('Rec Id not found...'); return; } return Insert(btn || 'btnupdate'); };
    P.btnRefresh = function (btn) {
        return HRM.busy(btn || 'btnRefresh', function () { return HRM.get(API + '/refresh', { recId: RecId }).then(fillCombos).then(cmbSupCust_Leave).catch(HRM.fail); });
    };
    P.btnAttachment = function () { ImpB.attachments({ state: AT, fileUrl: function (a) { return API + '/attachment-file?id=' + RecId + '&attachmentId=' + HRM.int(HRM.col(a, 'Id')); } }); };
    P.btnPrint = function (btn) { return InvoiceSlip(RecId, btn || 'btnPrint'); };
    P.BtnDefinePerformaNo = function () { ImpB.serialDialog({ kind: 'proforma', api: API + '/proforma-serials' }); };      // frmProformaDocumentSerial.Show()
    P.BtnDefineInvoiceNo = function () { ImpB.serialDialog({ kind: 'master', api: API + '/master-serials' }); };          // frmMasterDocumentSerial.Show()
    P.BtnDefineContractNo = P.BtnDefineInvoiceNo;
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

    // ============================================================ events
    ImpB.tabs();
    ImpB.onLeave('CmbItemName', bindRateUomAndItemPackUom);
    ImpB.onLeave('CmbExporterName', cmbSupCust_Leave);
    ImpB.onLeave('CmbImporterName', cmbSupCust_Leave);
    ImpB.onLeave('CmbContractNo', CmbContractNo_ValueChanged);
    ImpB.onLeave('cmbInvoiceNo', cmbInvoiceNo_ValueChanged);
    HRM.$('CmbPerformaNo').addEventListener('change', CmbPerformaNo_ValueChanged);
    HRM.$('CmbFinInstrumentsPaymentDetail').addEventListener('change', dcmbfino_ValueChanged);
    HRM.$('dtxtpercentoftotal').addEventListener('blur', PaymentDetailPercentCalculate);
    HRM.$('txtFcyAmountPaymentDetail').addEventListener('blur', dtxtfcyamount);
    HRM.$('txtQtyOtherItem').addEventListener('input', OtherItemsCalculations);
    HRM.$('txtRateOtherItem').addEventListener('input', OtherItemsCalculations);
    ImpB.wireDateChecks('h');
    function onForm() { return ImpB.activeTab('tc1') === 'tabPage1'; }
    var TABS = ['tabPage3', 'tabPage4', 'tabPage5'], FIRST = { tabPage3: 'CmbItemName', tabPage4: 'CmbItemOtherItem', tabPage5: 'CmbPaymentTermPaymentDetail' };
    HRM.keys({                                                                                      // ImProformaInvoice_KeyDown
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+alt+alt': P.btnShortCutKey, 'ctrl+alt+control': P.btnShortCutKey,
        'ctrl+t': function () { if (onForm()) { ImpB.showTab('tc1', 'tabPage2'); HRM.focus('hFromDate'); } else { ImpB.showTab('tc1', 'tabPage1'); HRM.focus(branchFeature ? 'CmbBranchName' : 'CmbExporterName'); } },
        'ctrl+s': function () { if (!onForm()) { P.btnShowHistory(); return; } if (!UpdateMode && !HRM.$('btnsave').disabled) P.btnsave(); },
        // Ctrl+U on the desktop calls btnsave_Click (RecId = 0): every line would be inserted again - it goes to Update here (see the report)
        'ctrl+u': function () { if (onForm() && UpdateMode && !HRM.$('btnupdate').disabled) P.btnupdate(); },
        'ctrl+n': function () { if (onForm()) P.btnNew(); else P.btnResetHistory(); },
        'ctrl+p': function () { if (onForm() && rights.print) P.btnPrint(); },
        'ctrl+r': function () { if (onForm()) P.btnRefresh(); else P.btnRefreshHistory(); },
        'ctrl+f10': function () { if (onForm()) P.btnAttachment(); },
        'ctrl+f5': function () { if (onForm()) HRM.focus(branchFeature ? 'CmbBranchName' : 'CmbExporterName'); else HRM.focus('hFromDate'); },
        'ctrl+arrowup': function () { if (onForm()) HRM.focus(FIRST[ImpB.activeTab('tc2')] || 'CmbItemName'); else HRM.focus('hFromDate'); },
        'ctrl+arrowright': function () {
            if (!onForm()) return;
            var t = TABS[(TABS.indexOf(ImpB.activeTab('tc2')) + 1) % 3]; ImpB.showTab('tc2', t); HRM.focus(FIRST[t]);
        }
    });
    HRM.footer(function (b) { ImpB.showTab('tc1', 'tabPage2'); return HRM.busy(b, HistoryGridFill); });

    // ============================================================ InitializeComponentMethod / Load
    ['txtPerformaDate', 'txtContractDate', 'txtInvoiceDate', 'txtBlDate', 'txtGdDate'].forEach(function (id) { HRM.setVal(id, HRM.today()); });
    HRM.fill('cmbStatus', [{ Id: 1, Status: 'Open' }, { Id: 2, Status: 'Complete' }, { Id: 3, Status: 'Cancel' }], 'Id', 'Status', { zero: false });
    HRM.loading(HRM.get(API + '/setup')).then(function (d) {
        rights = d.rights || {};
        HRM.applyRights(rights, { save: 'btnsave', update: 'btnupdate', print: 'btnPrint' });
        hist.columnOf('Edit').hidden = !rights.update; hist.columnOf('Slip').hidden = !rights.print; hist._build();   // HistoryGridSettings
        branchFeature = !!d.branchFeature; financialActive = !!d.financialActive; multi = !!d.multiCompanies;
        userBranchId = HRM.int(d.userBranchId); historyDays = d.historyDays;
        HRM.show('boxBranch', branchFeature); HRM.show('boxDebit', financialActive); HRM.show('boxCompany', multi);
        HRM.setVal('txtDocNo', HRM.str(d.docNo));
        fillCombos(d);
        if ((d.exporterBanks || []).length) HRM.setCombo('cmbexporterBankNew', HRM.int(HRM.col(d.exporterBanks[0], 'Id')));   // Rows[1].Activate()
        HRM.fill('CmbCustomerHistory', d.historyExporters, 'Id', 'name', { zero: '', keep: true });
        ImpB.historyDefaults('h', historyDays);
        DetailFormReset(); ResePaymentDetails(); ResetOtherItems();
        buttons(false);
        HRM.focus(branchFeature ? 'CmbBranchName' : 'CmbImporterName');
        var id = HRM.int(HRM.param('id'));
        if (id > 0) ReadById(id);
    }).catch(HRM.fail);
})();
