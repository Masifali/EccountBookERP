/* ============================================================================================
 * Screen 562 SalesWages_Register - Architecture.WinApp.Pcc.Reports.SalesWages_Register (.cs, 1723 lines)
 * Page: templates/sale/reports/pcc_sales_wages_register.html (route /sale/reports/pcc/sales-wages-register).
 *   VoucherValidation_Load :121     ComboFill, ServiceActivityAndItem, DateTypeFill, ActivityFill, cmbDateType.Focus()
 *   DateTypeFill :140               CommonServices.DateType, BindDDL(ZeroIndex false), Rows[1].Activate() ("This Week", From = today - 7)
 *   cmbDateType_ValueChanged :154   1 From = today | 2 today - 7 | 3 first of UTC month, To = today | 4 1 Jan, To = today | 5 Start_Period
 *   ComboFill :191                  InvSaleInvoice.DropDownFillFromInvSaleInvoice (RefDocumentTypeId 1 -> @IsFromWages) = [pcc].[USP_DropDownFillFromInvSaleInvoice], by Activity:
 *                                   Customer -> cmbsupplier, Contractor -> cmbcontractor (Id / ReferenceName), bound only when the list has rows
 *   ServiceActivityAndItem :238     ContractorWagesActivity.GetDataForDropDownFromWagesSchedule = [pcc].[USP_GetDataForDropDownFromWagesSchedule] (@IsFromWages), by Activity:
 *                                   GroupName -> cmbitemname ("Item Name"), Service Activity -> cmbServiceActivity
 *   ActivityFill :291               1 Detail, 2 Summary (BindDDLNew, Rows[0] activated)
 *   btnshow_Click :325              both print buttons hidden; Activity 0 -> "Please Select Activity First..."; 1 -> print (1865) visible, SaleOrder.salewagesregister =
 *                                   [pcc].[usp_SalesWages_Register]: rows copied to DocNo, DocDate, DocumentType, Customer, Contractor, ReferencePartyName, ReferencePartyAddress,
 *                                   Value_01..Value_10 (caption = Parms_01..10 of the first row) and Total = their sum; 2 -> btnPrint1865_01 visible, SaleOrder.SaleWagesRegisterSummary =
 *                                   [pcc].[usp_SalesWages_Register_Summary]: the same seven columns + Amount; no rows -> grid cleared
 *   GridSettings :450 / GridSettingsTwo :470   GridEX_Helper.GridWrappingAndColumnSettings(grd, 3 | 2, 2): header lines 3 | 2; numeric columns "#,##0.##" summed (Amount:
 *                                   stringFormatsingle summed), widths int 60, date 80, double 90, string with "name" 150, else 100
 *   print_Click_1 :483 / btnPrint1865_01_Click :506   "Not Record Found For Display" or 1865-SalesWages_Register.rpt / 1865_01_SalesWages_SummaryRegister.rpt over the last Show
 *   reset :632 (btnNew :652)             clears doc from / to, contractor, item name, customer, service activity, grid, DateTypeFill, focus Date Type
 *   btnRefresh_Click :688 (ComboFill + ServiceActivityAndItem), KeyDown :529, MakeShortCutKeys :588, btnshow_Leave :310
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, el = A.el;
    var API = '/api/sale/salepcc/sales-wages-register';
    var lookup = { amountDecimals: 0, rateDecimals: 0, yearStart: null };
    var lastArgs = null, hasRows = false, lastActivity = 0, seq = 0;
    var SHORTCUTS = [['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl + P', 'For print'],
        ['Ctrl+F5', 'For Focus On Date Type'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
        ['Ctrl+ArrowUp', 'For Focus On Date Type in Filter']];

    var grid = new S.Grid({ tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', headerLines: 3, frozen: 0, autosize: false });

    // ------------------------------------------------------------------ grids
    function detailColumns(first) {
        var cols = [
            { key: 'DocNo', caption: 'DocNo', width: 60, align: 'c' }, { key: 'DocDate', caption: 'DocDate', width: 80, date: 'dd/MM/yyyy' },
            { key: 'DocumentType', caption: 'DocumentType', width: 100 }, { key: 'Customer', caption: 'Customer', width: 100 }, { key: 'Contractor', caption: 'Contractor', width: 100 },
            { key: 'ReferencePartyName', caption: 'ReferencePartyName', width: 150 }, { key: 'ReferencePartyAddress', caption: 'ReferencePartyAddress', width: 100 }
        ];
        for (var i = 1; i <= 10; i++) {                                      // "Value_0" + i, so the tenth is Value_010 (as the procedure and the desktop)
            var pc = A.ci(first, 'Parms_0' + i);
            cols.push({ key: 'Value_0' + i, caption: pc == null ? '' : String(pc), width: 90, fmt: '#,##0.##', totalFmt: '#,##0.##', sum: true, num: true });
        }
        cols.push({ key: 'Total', caption: 'Total', width: 90, fmt: '#,##0.##', totalFmt: '#,##0.##', sum: true, num: true });
        return cols;
    }
    function detailRows(rows) {
        return rows.map(function (r) {
            var m = { DocNo: A.toInt(A.ci(r, 'DocNo')), DocDate: A.ci(r, 'DocDate'), DocumentType: A.ci(r, 'DocumentTypeDescription'), Customer: A.ci(r, 'CustomerName'),
                      Contractor: A.ci(r, 'ContractorName'), ReferencePartyName: A.ci(r, 'ReferencPartyName'), ReferencePartyAddress: A.ci(r, 'ReferencPartyAddress') };
            var total = 0;
            for (var j = 1; j <= 10; j++) { var v = A.toDouble(A.ci(r, 'Value_0' + j)); m['Value_0' + j] = v; total += v; }
            m.Total = total;
            return m;
        });
    }
    function summaryColumns() {
        var single = S.fmtSingle(lookup.amountDecimals);
        return [
            { key: 'DocNo', caption: 'DocNo', width: 60, align: 'c' }, { key: 'DocDate', caption: 'DocDate', width: 80, date: 'dd/MM/yyyy' },
            { key: 'DocumentType', caption: 'DocumentType', width: 100 }, { key: 'Customer', caption: 'Customer', width: 100 }, { key: 'Contractor', caption: 'Contractor', width: 100 },
            { key: 'ReferencePartyName', caption: 'ReferencePartyName', width: 150 }, { key: 'ReferencePartyAddress', caption: 'ReferencePartyAddress', width: 100 },
            { key: 'Amount', caption: 'Amount', width: 90, fmt: single, totalFmt: single, sum: true, num: true }
        ];
    }
    function summaryRows(rows) {
        return rows.map(function (r) {
            return { DocNo: A.toInt(A.ci(r, 'DocNo')), DocDate: A.ci(r, 'DocDate'), DocumentType: A.ci(r, 'DocumentTypeDescription'), Customer: A.ci(r, 'CustomerName'),
                     Contractor: A.ci(r, 'ContractorName'), ReferencePartyName: A.ci(r, 'ReferencPartyName'), ReferencePartyAddress: A.ci(r, 'ReferencPartyAddress'), Amount: A.ci(r, 'Amount') };
        });
    }

    // ------------------------------------------------------------------ lists
    function comboFill(data) {                                               // ComboFill: bound only when the procedure gives rows
        var c = (data && data.invoiceCombos) || {};
        if (!Object.keys(c).length) return;
        if ((c.Customer || []).length) S.fill(el('cmbsupplier'), c.Customer, false);
        if ((c.Contractor || []).length) S.fill(el('cmbcontractor'), c.Contractor, false);
    }
    function serviceActivityAndItem(data) {                                  // ServiceActivityAndItem
        var c = (data && data.wagesCombos) || {};
        if (!Object.keys(c).length) return;
        if ((c.GroupName || []).length) S.fill(el('cmbitemname'), c.GroupName, false);
        if ((c['Service Activity'] || []).length) S.fill(el('cmbServiceActivity'), c['Service Activity'], false);
    }
    function dateTypeFill() { S.fill(el('cmbDateType'), S.dateTypeRows(false), false); S.activate('cmbDateType', 1, false); }
    function activityFill() { S.fill(el('CmbActivity'), [{ Id: 1, name: 'Detail' }, { Id: 2, name: 'Summary' }], false); S.activate('CmbActivity', 0, false); }
    el('cmbDateType').addEventListener('change', function () {               // cmbDateType_ValueChanged
        S.dateRule(A.toInt(this.value), el('fromDate'), el('todate'), lookup.yearStart);
    });

    // ------------------------------------------------------------------ Show / print
    function show() {                                                        // btnshow_Click
        var b = el('show'); if (b.disabled) return Promise.resolve();
        el('print1865_01').hidden = true; el('print1865').hidden = true;
        hasRows = false;
        var activity = S.selInt('CmbActivity');
        if (activity === 0) { alert('Please Select Activity First...'); return Promise.resolve(); }
        var a = { fromDate: el('fromDate').value, toDate: el('todate').value, fromDocNo: A.toIntText(el('txtFromDoc').value), toDocNo: A.toIntText(el('txtDocTo').value),
                  serviceActivityId: S.selInt('cmbServiceActivity'), itemId: S.selInt('cmbitemname'), supplierCustomerId: S.selInt('cmbsupplier'),
                  contractorId: S.selInt('cmbcontractor'), activity: activity }, token = ++seq;
        if (activity === 1) el('print1865').hidden = false; else if (activity === 2) el('print1865_01').hidden = false;
        A.busy(b, true);
        return A.getJson(API + '/rows?' + S.qs(a)).then(function (rows) {
            if (token !== seq) return;
            lastArgs = a; lastActivity = activity; hasRows = !!(rows && rows.length);
            if (!hasRows) { grid.clear(); return; }                          // grd.DataSource = null
            if (activity === 1) { grid.cfg.headerLines = 3; grid.setData(detailColumns(rows[0]), detailRows(rows)); }
            else { grid.cfg.headerLines = 2; grid.setData(summaryColumns(), summaryRows(rows)); }
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }
    function print(btnId) {                                                  // print_Click_1 / btnPrint1865_01_Click
        var b = el(btnId); if (b.disabled) return;
        if (!hasRows || !lastArgs) { alert('Not Record Found For Display'); return; }
        A.busy(b, true);
        var q = {}; Object.keys(lastArgs).forEach(function (k) { q[k] = lastArgs[k]; }); q.kind = btnId === 'print1865' ? '1865' : '1865_01';   // the button decides the template, not the activity
        A.openPdf('/sale/reports/pcc/print/sales-wages-register?' + S.qs(q)).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function reset() {                                                       // reset
        el('txtFromDoc').value = ''; el('txtDocTo').value = '';
        ['cmbcontractor', 'cmbitemname', 'cmbsupplier', 'cmbServiceActivity'].forEach(function (id) {
            var s = el(id); s.value = ''; s.dispatchEvent(new Event('change', { bubbles: true }));
        });
        grid.clear(); hasRows = false;
        dateTypeFill();
        S.focus('cmbDateType');
    }
    function refresh() {                                                     // btnRefresh_Click
        var b = el('refresh'); if (b.disabled) return;
        A.busy(b, true);
        A.getJson(API + '/lookups').then(function (data) { lookup = data || lookup; comboFill(data); serviceActivityAndItem(data); }).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function closeForm() { w.location.href = '/sale/reports'; }

    S.digitsOnly('txtFromDoc'); S.digitsOnly('txtDocTo');
    el('show').addEventListener('click', show);
    el('reset').addEventListener('click', reset);
    el('refresh').addEventListener('click', refresh);
    el('print1865').addEventListener('click', function () { print('print1865'); });
    el('print1865_01').addEventListener('click', function () { print('print1865_01'); });
    el('shortcuts').addEventListener('click', function () { A.shortcuts(SHORTCUTS); });
    S.closeShortcuts();
    el('show').addEventListener('blur', function (e) { if (e.relatedTarget && el('grid').contains(e.relatedTarget)) S.focus('cmbDateType'); });   // btnshow_Leave
    S.keys({ s: show, e: closeForm, n: reset, p: function () { print('print1865'); }, F5: function () { S.focus('cmbDateType'); }, ArrowUp: function () { S.focus('cmbDateType'); },
             ArrowDown: function () { grid.focus(); }, alt: function () { A.shortcuts(SHORTCUTS); } }, closeForm);

    // ------------------------------------------------------------------ start (Load)
    el('fromDate').value = A.today(); el('todate').value = A.today();
    grid.render();
    dateTypeFill(); activityFill();
    A.getJson(API + '/lookups').then(function (data) {
        lookup = data || lookup;
        comboFill(data); serviceActivityAndItem(data);                       // ComboFill, ServiceActivityAndItem
        S.activate('cmbDateType', 1, false);                                 // the year start is known now: re-apply the date type rule
        S.focus('cmbDateType');
    }).catch(function (e) { alert(e.message); });
}(window, document));
