/* ============================================================================================
 * Screen 933 "Goods Receipts Audit" - Architecture.WinApp.Inventory_Reports.frmGrnAudit_History
 * Page: templates/accounts/reports/acrpt4_grn_audit_history.html (route /inventory/reports/grn-audit-history).
 * Every desktop handler, in desktop order (frmGrnAudit_History.cs):
 *   frmGRNHistory_Load       -> AllDropDownBind(), ParameterFill() (DateType 1..5), cmbperemeter.Focus(); date type 0 -> Rows[0] ("This Day").
 *                               (RequestedByOtherDocument is false on the web: the grid is NOT filled on open.)
 *   AllDropDownBind          -> InvGrn.GetDataForDropDownFromGrn (USP_GetDataForDropDownFromGrn, DocumentTypeIds "46", branch, year) split by Activity
 *                               PurchaseType / ParentCategory / PurchaseOrderNo / Supplier / City (Id, ReferenceName), each kept on its previous selection.
 *   cmbperemeter_ValueChanged -> 1 From = today | 2 today - 7 | 3 first of month + To = today | 4 1 Jan + To = today | 5 FY start.
 *   btnshow_Click / gridHisory -> InvGrn.GetGrnDataForAudit (usp_getDataForGrnAudit): dates, purchase type, parent category, order, supplier, city,
 *                               FreightDiscountAmount 1.0 / ActionId 1 from the two check boxes. rows > 0 -> the 49 typed columns of dtHisGrid,
 *                               GridSetting() (hidden ids, links, grouped by Variety with GroupTotals Always, widths, captions, "#,##0.##" sums);
 *                               no rows -> DataSource = null.
 *   DataGridHistory_LinkClicked -> DocNo: GrnSlipWithSubReports(Id) (211) | GpSrNo: GatePassInwardSlipAndRegisterReport (251) | OrderNo: ActivityDetails
 *                               window | FactoryWeight: WbTransactionSlipByGpId(gp, 51) (280) | NetPaid: FreightVoucherSlip241 (241); each only when its id > 0.
 *   btn334Register_Click     -> dt (the raw procedure rows) -> 334_01-GrnAuditReport.rpt, else "Not Record Found For Display".
 *   btnNew_Click / Reset     -> GRNfromdate.Focus(), Supplier text cleared.      btnRefresh_Click -> AllDropDownBind().
 *   frmGRNHistory_KeyDown    -> Enter = Tab, Ctrl+E / Esc close, Ctrl+S show, Ctrl+F5 date type, Ctrl+N new, Alt+1 / Alt+NumPad1 334 register, Ctrl+R refresh,
 *                               Ctrl+Down grid, Ctrl+Up date type, Ctrl+Alt shortcut list (Alt+2 "335-Register" is only listed on the desktop, it has no handler).
 *   btnshow_Leave            -> only when the grid becomes the active control straight from Show; the tab order after Show is the filter boxes, so
 *                               it cannot be reached by keyboard here.
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt4, el = A.el, API = '/api/accounts/acrpt4/grn', seq = 0;
    var RPT334 = '334_01-GrnAuditReport.rpt';
    var dt = [];                                       // the raw procedure rows
    var decimals = 0, yearStart = null;
    var DATE_TYPES = [{ Id: 1, Parameters: 'This Day' }, { Id: 2, Parameters: 'This Week' }, { Id: 3, Parameters: 'This Month' },
                      { Id: 4, Parameters: 'This Year' }, { Id: 5, Parameters: 'Financial Year' }];
    var SEL = { PurchaseType: 'CmbPurchaseType', ParentCategory: 'cmbParentCategory', PurchaseOrderNo: 'CmbPoNo', Supplier: 'CmbSupplier', City: 'CmbCityName' };

    function ci(r, k) { return A.ci(r, k); }
    function toInt(v) { var s = String(v == null ? '' : v).trim(); return /^[-+]?\d+$/.test(s) ? parseInt(s, 10) : (isFinite(Number(s)) && s !== '' ? Math.trunc(Number(s)) : 0); }
    function nInt(v) { return v == null ? null : toInt(v); }

    function stringFormatsingle() { return '#,##0.' + (['', '0', '00', '000', '0000'][decimals] || ''); }

    // ------------------------------------------------------------------ grid columns (dtHisGrid + GridWrappingAndColumnSettings + GridSetting)
    function columns() {
        var D2 = '#,##0.##';
        function txt(key, width, extra) { return Object.assign({ key: key, caption: key, width: width, hAlign: 'center' }, extra || {}); }
        function int(key, width, extra) { return Object.assign({ key: key, caption: key, width: width, hAlign: 'center', type: 'int', align: 'center' }, extra || {}); }
        function num(key, width, fmt, extra) {          // a double column: right aligned, Sum
            return Object.assign({ key: key, caption: key, width: width, hAlign: 'center', type: 'num', align: 'right', sum: true, fmt: fmt, totalFmt: fmt }, extra || {});
        }
        return [
            int('Id', 60, { hidden: true }),
            int('DocNo', 50, { caption: 'Grn No', link: true }),
            txt('DocDate', 73, { type: 'date', dateFmt: 'yy' }),
            txt('VehicleNo', 80), txt('BiltyNo', 45),
            int('InwardGatePassId', 60, { hidden: true }),
            int('GpSrNo', 45, { link: true }),
            int('OrderId', 60, { hidden: true }),
            int('OrderNo', 45, { link: true }),
            txt('TransType', 100),
            int('SupplierCustomerId', 60, { hidden: true }),
            txt('SupplierName', 200),
            num('SupplierWeight', 75, D2),
            num('FactoryWeight', 75, D2, { link: true }),
            num('WeightDiff', 63, D2),
            txt('ExcessShort', 68),
            num('WeightBridgeTolerance', 70, D2),
            num('NoofBags', 60, D2, { caption: 'No of Bags', type: 'int' }),
            txt('PolicyName', 70, { caption: 'Shortage Allow Policy' }),
            num('ShortWeight', 70, D2, { caption: 'Total Short Weight' }),
            num('AllowShortage', 70, D2, { caption: 'Against Policy Discount Allowed Weight' }),
            num('ApplyShortage', 70, D2, { caption: 'Short Weight Deducted' }),
            num('GrossBillWeight', 75, D2),
            txt('BagType', 68), txt('DeliveryTerm', 62),
            txt('Variety', 55, { hidden: true }),
            txt('EBTerm', 90, { caption: 'EB Term' }),
            num('EBWeightCut', 65, D2, { caption: 'EB Weight Cut' }),
            num('QualityCutTouch', 65, D2, { caption: 'Quality Cut / Touch' }),
            num('AdLsWeight', 65, D2, { caption: 'Add Less Weight' }),
            num('BillWeight', 75, D2, { caption: 'Net Bill Weight' }),
            num('GrossStockWeight', 75, D2),
            num('LsEBWStock', 65, D2, { caption: 'Less EB Weight Stock' }),
            num('StockWeight', 75, D2, { caption: 'Net Stock Weight' }),
            num('BillStockDiff', 80, D2, { caption: 'Bill And Stock Weight Diff' }),
            txt('CityName', 62),
            num('Freight/40KG', 70, stringFormatsingle()),
            num('BiltyFreight', 73, D2), num('ScaleCharges', 73, D2), num('OtherDeduction', 73, D2),
            num('AdvanceByParty', 73, D2), num('AdvanceByFactory', 73, D2),
            num('ShortageAmount', 70, D2, { caption: 'Shortage Applied Amount' }),
            num('DiscountAmount', 70, D2, { caption: 'Freight Discount' }),
            num('ChargeToPartyAmount', 70, D2, { caption: 'Shortage Charge To Supplier' }),
            num('FreightDeductionAmount', 70, D2, { caption: 'Deduction From Freight' }),
            num('TotalPayableAmount', 73, stringFormatsingle()),
            int('FreightVoucherId', 60, { hidden: true }),
            num('NetPaid', 73, D2, { caption: 'Net Payable Amount', link: true })
        ];
    }

    function mapRows(rows) {
        return rows.map(function (r) {
            return {
                'Id': nInt(ci(r, 'Id')), 'DocNo': nInt(ci(r, 'DocNo')), 'DocDate': ci(r, 'DocDate'), 'VehicleNo': ci(r, 'VehicleNo'), 'BiltyNo': ci(r, 'BiltyNo'),
                'InwardGatePassId': nInt(ci(r, 'InwardGatePassId')), 'GpSrNo': nInt(ci(r, 'GpSrNo')), 'OrderId': nInt(ci(r, 'OrderId')), 'OrderNo': nInt(ci(r, 'OrderNo')),
                'TransType': ci(r, 'TransType'), 'SupplierCustomerId': nInt(ci(r, 'SupplierCustomerId')), 'SupplierName': ci(r, 'SupplierName'),
                'SupplierWeight': ci(r, 'PartyWeight'), 'FactoryWeight': ci(r, 'FactoryWeight'), 'WeightDiff': ci(r, 'WeightDiff'),
                'ExcessShort': ci(r, 'ExcessShort'), 'WeightBridgeTolerance': ci(r, 'WeightBridgeTolerance'), 'NoofBags': ci(r, 'NoofBags'),
                'PolicyName': ci(r, 'PolicyName'), 'ShortWeight': ci(r, 'ShortWeight'), 'AllowShortage': ci(r, 'AllowShortage'), 'ApplyShortage': ci(r, 'ApplyShortage'),
                'GrossBillWeight': ci(r, 'GrossBillWeight'), 'BagType': ci(r, 'BagType'), 'DeliveryTerm': ci(r, 'DeliveryTerm'), 'Variety': ci(r, 'Variety'),
                'EBTerm': ci(r, 'EBTerm'), 'EBWeightCut': ci(r, 'EBWeightCut'), 'QualityCutTouch': ci(r, 'QualityCut/Touch'), 'AdLsWeight': ci(r, 'AdLsWeight'),
                'BillWeight': ci(r, 'BillWeight'), 'GrossStockWeight': ci(r, 'FactoryWeight'), 'LsEBWStock': ci(r, 'LsEBWStock'), 'StockWeight': ci(r, 'StockWeight'),
                'BillStockDiff': ci(r, 'BillStockDiff'), 'CityName': ci(r, 'CityName'), 'Freight/40KG': ci(r, 'FreightPer40Kg'), 'BiltyFreight': ci(r, 'BiltyFreight'),
                'ScaleCharges': ci(r, 'ScaleCharges'), 'OtherDeduction': ci(r, 'OtherDeduction'), 'AdvanceByParty': ci(r, 'AdvanceByParty'),
                'AdvanceByFactory': ci(r, 'AdvanceByFactory'), 'ShortageAmount': ci(r, 'ShortageAmount'), 'DiscountAmount': ci(r, 'DiscountAmount'),
                'ChargeToPartyAmount': ci(r, 'ChargeToPartyAmount'), 'FreightDeductionAmount': ci(r, 'FreightDeductionAmount'),
                'TotalPayableAmount': ci(r, 'TotalPayableAmount'), 'FreightVoucherId': nInt(ci(r, 'FreightVoucherId')), 'NetPaid': ci(r, 'InvoiceFreight')
            };
        });
    }

    var grid = A.createGrid({ table: el('grd'), scroller: el('gridWrap'), nav: el('nav'), navText: el('navText'), groupBoxEl: el('gbox1'),
        headerLines: 3, groupBy: 'Variety', groupTotals: true, totalRow: true, onLink: onLink });
    grid.clear();

    // ------------------------------------------------------------------ AllDropDownBind
    var items = [];
    function allDropDownBind() {
        return A.getJson(API + '/init').then(function (init) {
            decimals = init.amountDecimals || 0; yearStart = init.yearStart || yearStart;
            items = init.items || [];
            if (!items.length) return;
            Object.keys(SEL).forEach(function (act) {
                var rows = items.filter(function (r) { return String(ci(r, 'Activity')) === act; });
                A.fillSelect(el(SEL[act]), rows, 'Id', 'ReferenceName', { keep: true });          // BindAndRetainSelection, no default row
            });
        });
    }

    // ------------------------------------------------------------------ gridHisory
    function gridHistory() {
        var q = new URLSearchParams({
            fromDate: el('GRNfromdate').value || A.today(), toDate: el('GRNToDate').value || A.today(),
            purchaseTypeId: A.intArg(el('CmbPurchaseType').value), parentCategoryId: A.intArg(el('cmbParentCategory').value),
            orderId: A.intArg(el('CmbPoNo').value), supplierId: A.intArg(el('CmbSupplier').value), cityId: A.intArg(el('CmbCityName').value),
            onlyDiscounted: el('chkOnlyDiscountedRows').checked ? 'true' : 'false', freightByCity: el('chkFreightAuditbyCity').checked ? 'true' : 'false'
        });
        var token = ++seq;
        return A.getJson(API + '/data?' + q.toString()).then(function (data) {
            if (token !== seq) return;
            dt = (data && data.rows) || [];
            if (dt.length) grid.setData(columns(), mapRows(dt));
            else grid.clear();                                      // DataGridHistory.DataSource = null
        });
    }
    function show() {                                               // btnshow_Click
        var b = el('show'); if (b.disabled) return Promise.resolve();
        A.busy(b, true);
        return gridHistory().catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function refresh() {                                            // btnRefresh_Click -> AllDropDownBind()
        var b = el('btnRefresh'); if (b.disabled) return Promise.resolve();
        A.busy(b, true);
        return allDropDownBind().catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function reset() { el('GRNfromdate').focus(); A.clearSelect(el('CmbSupplier')); }     // Reset()

    // ------------------------------------------------------------------ links
    function notReady() { alert('Printing is not available on this page.'); }
    function onLink(row, col) {                                     // DataGridHistory_LinkClicked (CurrentRow)
        var id = toInt(ci(row, 'Id')), gp = toInt(ci(row, 'InwardGatePassId')), order = toInt(ci(row, 'OrderId')), fv = toInt(ci(row, 'FreightVoucherId'));
        switch (col.key) {
            case 'DocNo':                                           // GrnSlipWithSubReports(Id): DocumentTypeId 46 when none is given
                if (id > 0) { if (typeof w.printRpt !== 'function') return notReady(); w.printRpt('211-InvRptGoodsReceiptsNotesRiceSlip.rpt', { id: id, documentTypeId: 46 }); }
                break;
            case 'GpSrNo':                                          // GatePassInwardSlipAndRegisterReport(InwardGatePassId) -> DocumentTypeId 51
                if (gp > 0) { if (typeof w.printRpt !== 'function') return notReady(); w.printRpt('251-InvRptInwardGatePassSlip.rpt', { id: gp }); }
                break;
            case 'OrderNo':                                         // new ActivityDetails(UserAccount, orderId).Show()
                if (order > 0) alert('Activity Details (purchase order activity window) is not on the web yet.');
                break;
            case 'FactoryWeight':                                   // WbTransactionSlipByGpId(gp, 51) -> 280-InvRptWeighBridgeSlip.rpt over the slip rows
                if (gp > 0) A.getJson(API + '/wb-slip?gpId=' + gp).then(function (data) {
                    if (!data || !data.rows || !data.rows.length) { alert('No Record Found For Display'); return; }
                    A.postGrid('280-InvRptWeighBridgeSlip.rpt', data.rows, null, null);
                }).catch(function (e) { alert(e.message); });
                break;
            case 'NetPaid':                                         // FreightVoucherSlip241(FreightVoucherId)
                if (fv > 0) { if (typeof w.printRpt !== 'function') return notReady(); w.printRpt('241-FreightVoucherSlip.rpt', { id: fv }); }
                break;
        }
    }

    function print334() {                                           // btn334Register_Click
        if (!dt.length) { alert('Not Record Found For Display'); return; }
        A.postGrid(RPT334, dt, null, el('btn334'));
    }

    function shortcuts() {                                          // MakeShortCutKeys
        A.shortcuts([['Ctrl+S', 'For Shoe Record'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Alt+1', 'For Print 334-Register'], ['Alt+2', 'For Print 335-Register'], ['Ctrl+F5', 'For Focus on Combo Date'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid History'],
            ['Ctrl+ArrowUp', 'For Focus On Date combo in Filters Box'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }
    function closeForm() { w.location.href = '/accounts/dashboard'; }

    // ------------------------------------------------------------------ date type
    function dateType() {                                           // cmbperemeter_ValueChanged
        var v = A.intArg(el('cmbperemeter').value), now = new Date();
        if (v === 1) el('GRNfromdate').value = A.today();
        else if (v === 2) el('GRNfromdate').value = A.addDays(A.today(), -7);
        else if (v === 3) { el('GRNfromdate').value = A.ymd(new Date(now.getUTCFullYear(), now.getUTCMonth(), 1)); el('GRNToDate').value = A.today(); }
        else if (v === 4) { el('GRNfromdate').value = now.getFullYear() + '-01-01'; el('GRNToDate').value = A.today(); }
        else if (v === 5) { if (yearStart) el('GRNfromdate').value = yearStart; }
    }

    // ------------------------------------------------------------------ events
    el('show').addEventListener('click', show);
    el('btnNew').addEventListener('click', reset);
    el('btnRefresh').addEventListener('click', refresh);
    el('btn334').addEventListener('click', print334);
    el('btnShortCutKeys').addEventListener('click', shortcuts);
    el('cmbperemeter').addEventListener('change', dateType);

    function focusables() {
        return ['cmbperemeter', 'GRNfromdate', 'GRNToDate', 'CmbSupplier', 'show', 'CmbPurchaseType', 'cmbParentCategory', 'CmbPoNo', 'CmbCityName',
                'chkOnlyDiscountedRows', 'chkFreightAuditbyCity'].map(A.focusOf);
    }
    d.addEventListener('keydown', function (e) {                    // frmGRNHistory_KeyDown
        var onGrid = e.target && e.target.closest && e.target.closest('#gridWrap');
        if (e.key === 'Enter' && !e.ctrlKey && !e.altKey && !e.shiftKey && !A.comboOpen() && e.target.tagName !== 'BUTTON' && !onGrid) {   // SendKeys "{TAB}"
            var list = focusables(), i = list.indexOf(d.activeElement);
            if (i >= 0 && i < list.length - 1) { e.preventDefault(); list[i + 1].focus(); return; }
        }
        if (e.key === 'Escape' && !A.comboOpen() && !(el('shortcutDialog') && el('shortcutDialog').open)) { e.preventDefault(); closeForm(); return; }
        if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
        if (e.altKey && !e.ctrlKey && (e.key === '1' || e.code === 'Numpad1' || e.code === 'Digit1')) { e.preventDefault(); print334(); return; }
        if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); A.focusOf('cmbperemeter').focus(); return; }
        if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); grid.focus(); return; }
        if (e.ctrlKey && e.key === 'ArrowUp') { e.preventDefault(); A.focusOf('cmbperemeter').focus(); return; }
        if (!e.ctrlKey || e.altKey) return;
        var k = e.key.toLowerCase();
        if (k === 'e') { e.preventDefault(); closeForm(); }
        else if (k === 's') { e.preventDefault(); show(); }
        else if (k === 'n') { e.preventDefault(); reset(); }
        else if (k === 'r') { e.preventDefault(); refresh(); }
    });

    // ------------------------------------------------------------------ Load
    el('GRNfromdate').value = el('GRNToDate').value = A.today();
    A.fillSelect(el('cmbperemeter'), DATE_TYPES, 'Id', 'Parameters', { keep: false, empty: false });
    allDropDownBind().catch(function (e) { alert(e.message); }).then(function () {
        A.focusOf('cmbperemeter').focus();
        var s = el('cmbperemeter'); s.value = '1'; s.dispatchEvent(new Event('change', { bubbles: true }));   // Rows[0].Activate()
    });
}(window, document));
