/* ============================================================================================
 * Screen 559 frmGPOutwardRegister - Architecture.WinApp.Steel.Reports.SalesReports.frmGPOutwardRegister (.cs)
 * Page: templates/sale/steel/rpt_gp_outward_register_st.html (route /sale/reports/steel/gp-outward-register).
 *   frmGPOutward_Load :98        SupplierCustomer, StatusFill, gridHistory, fromdate = ActiveYr.Start_Period (set AFTER the first gridHistory, as the desktop does)
 *   SupplierCustomer :123        CommonServices.SupplierCustomerGetforComboServiceBind (Sp_SupplierCustomer_GetAllMethod ReadByOrganizationIdCompanyIdForBinding), Id / CompanyName, ZeroIndex false
 *   StatusFill :139              1 Open, 2 Accepted, 3 Rejected (ZeroIndex false, nothing activated)
 *   gridHistory :163             GatePassOutwardReports.GatePassOutwardSlipandRegisterForSteel = [ST].[USp_GatePassOutward_SlipAndRegisterSteel_Rpt]; Status = the combo TEXT;
 *                                40 column table (Id hidden); no rows -> DataSource = null
 *   GridSetting :241             Id hidden, NoOfAttachtment and GpSrNo are links, GridAutoAdjustment, Print button column (40) at position 0, FrozenColumns 1
 *   DataGridHistory_ColumnButtonClick :265 / LinkClicked :277   Print button and the GpSrNo link -> GatePassOutwardSlipandRegisterForSteel(Id) (1512 slip);
 *                                the attachment link compares its key with "NoOfAttachments" (the column is "NoOfAttachtment") so it never fires - kept as is
 *   btnNew :109 / reset :114     doc numbers cleared, customer and status text cleared, gridHistory
 *   toolStripButton1 :332        "Record Not Found For Display" else 1520-InvRptGatePassOutwardRegister.rpt over dtReg
 *   GpsNoFrom / GpsNoTo KeyPress: digits only.   KeyDown: Ctrl+P register, Ctrl+E / Esc close, Ctrl+N refresh
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, el = A.el;
    var API = '/api/sale/steel-reports/gp-outward-register';
    var PRINT = '/sale/reports/steel/print/';
    var lastArgs = null, hasRows = false, seq = 0;

    var grid = new S.Grid({ tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', headerLines: 2, autosize: true, frozen: 1, onLink: onLink, onButton: onButton });

    function cols() {
        var D = 'dd-MMM-yy', DT = 'dd-MMM-yy hh:mm tt', R = 'r';
        return [
            { key: '_Print', caption: 'Print', width: 40, button: 'Print' },
            { key: 'Id', caption: 'Id', hidden: true }, { key: 'GpSrNo', caption: 'GpSrNo', link: true, align: R }, { key: 'GpDate', caption: 'GpDate', date: D },
            { key: 'GatepassType', caption: 'GatepassType' }, { key: 'CustomerName', caption: 'CustomerName' }, { key: 'OrderType', caption: 'OrderType' },
            { key: 'OrderNo', caption: 'OrderNo', align: R }, { key: 'OrderQty', caption: 'OrderQty', align: R }, { key: 'OrderGrossWeight', caption: 'OrderGrossWeight', align: R },
            { key: 'PackUOM', caption: 'PackUOM' }, { key: 'GpQty', caption: 'GpQty', align: R }, { key: 'PackingType', caption: 'PackingType' },
            { key: 'VehicleType', caption: 'VehicleType' }, { key: 'VehicleNo', caption: 'VehicleNo' }, { key: 'BiltyNo', caption: 'BiltyNo' },
            { key: 'InDateTime', caption: 'InDateTime', date: DT }, { key: 'OutDateTime', caption: 'OutDateTime', date: DT }, { key: 'CityName', caption: 'CityName' },
            { key: 'Freight', caption: 'Freight', align: R }, { key: 'SupplierWeight', caption: 'SupplierWeight', align: R }, { key: 'FactoryWeight', caption: 'FactoryWeight', align: R },
            { key: 'DifferenceWeight', caption: 'DifferenceWeight', align: R }, { key: 'NetWeightWb', caption: 'NetWeightWb', align: R }, { key: 'ContainerNo', caption: 'ContainerNo' },
            { key: 'WareHouseName', caption: 'WareHouseName' }, { key: 'VarietyName', caption: 'VarietyName' }, { key: 'ItemName', caption: 'ItemName' }, { key: 'JobLot', caption: 'JobLot' },
            { key: 'EntryDate', caption: 'EntryDate', date: DT }, { key: 'EntryUser', caption: 'EntryUser' }, { key: 'ModifyDate', caption: 'ModifyDate', date: DT },
            { key: 'ModifyUser', caption: 'ModifyUser' }, { key: 'ApprovedUser', caption: 'ApprovedUser' }, { key: 'ApprovedDate', caption: 'ApprovedDate', date: DT },
            { key: 'IsApproved', caption: 'IsApproved' }, { key: 'GPStatus', caption: 'GPStatus' }, { key: 'OtherRemarks', caption: 'OtherRemarks' },
            { key: 'GPRemarks', caption: 'GPRemarks', align: R }, { key: 'WtDiffRemarks', caption: 'WtDiffRemarks' }, { key: 'NoOfAttachtment', caption: 'NoOfAttachtment', link: true }
        ];
    }

    // ------------------------------------------------------------------ lists
    function statusFill() { S.fill(el('cmbStatus'), [{ Id: 1, name: 'Open' }, { Id: 2, name: 'Accepted' }, { Id: 3, name: 'Rejected' }], false); }
    function supplierCustomer() {
        return A.getJson(API + '/lookups').then(function (data) {
            S.fill(el('CmbSupplier'), (data.customers || []).map(function (r) { return { Id: A.ci(r, 'Id'), name: A.ci(r, 'CompanyName') }; }), false);
            return data;
        });
    }

    // ------------------------------------------------------------------ Show
    function gridHistory() {
        var b = el('btnshow'); if (b.disabled) return Promise.resolve();
        var a = { fromDate: el('fromdate').value, toDate: el('ToDate').value, fromDocNo: A.toIntText(el('GpsNoFrom').value), toDocNo: A.toIntText(el('GpsNoTo').value),
                  supplierCustomerId: S.selInt('CmbSupplier'), status: S.selText('cmbStatus') };
        var token = ++seq;
        A.busy(b, true);
        return A.getJson(API + '/rows?' + S.qs(a)).then(function (rows) {
            if (token !== seq) return;
            lastArgs = a; hasRows = !!(rows && rows.length);
            if (!hasRows) { grid.clear(); return; }
            grid.setData(cols(), rows);
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }

    // ------------------------------------------------------------------ Print button / GpSrNo link: GatePassOutwardSlipandRegisterForSteel(Id)
    function slip(row) {
        if (!row) return;
        A.openPdf(PRINT + 'gp-outward-slip?id=' + A.toInt(A.ci(row, 'Id'))).catch(function (e) { alert(e.message); });
    }
    function onButton(row) { slip(row); }
    function onLink(row, col) {
        if (!row || !col) return;
        if (col.key === 'GpSrNo') slip(row);
        // NoOfAttachtment: the desktop tests e.Column.Key == "NoOfAttachments" (a different key), so the click does nothing
    }

    // ------------------------------------------------------------------ toolbar
    function blank(id) { var s = el(id); s.value = ''; s.dispatchEvent(new Event('change', { bubbles: true })); }
    function reset() {                                                              // btnNew_Click -> reset()
        el('GpsNoFrom').value = ''; el('GpsNoTo').value = '';
        blank('CmbSupplier'); blank('cmbStatus');
        gridHistory();
    }
    function register() {                                                           // toolStripButton1_Click
        if (!hasRows || !lastArgs) { alert('Record Not Found For Display'); return; }
        A.openPdf(PRINT + 'gp-outward-register?' + S.qs(lastArgs)).catch(function (e) { alert(e.message); });
    }
    function closeForm() { w.location.href = '/sale/reports'; }

    el('btnshow').addEventListener('click', gridHistory);
    el('reset').addEventListener('click', reset);
    el('register').addEventListener('click', register);
    S.digitsOnly('GpsNoFrom'); S.digitsOnly('GpsNoTo');
    S.keys({ n: reset, p: register, e: closeForm }, closeForm);

    // ------------------------------------------------------------------ start (Load)
    el('fromdate').value = A.today(); el('ToDate').value = A.today();
    grid.render();
    supplierCustomer().then(function (data) {
        statusFill();
        return gridHistory().then(function () {
            if (data.yearStart) el('fromdate').value = S.isoDay(data.yearStart);    // fromdate = ActiveYr.Start_Period (after the first fill, as the desktop)
        });
    }).catch(function (e) { alert(e.message); });
}(window, document));
