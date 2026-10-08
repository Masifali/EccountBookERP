/* ============================================================================================
 * Screen 840 SaleInvoiceRegister - Architecture.WinApp.Pcc.Reports.SaleInvoiceRegister (.cs)
 * Page: templates/sale/reports/sarpt1_sale_invoice_register.html (route /sale/reports/sale-invoice-register).
 *   Load :170-236          ComboFill, ParameterFill (Rows[1] = This Week), StatusFill (Rows[2] = All), ReportTypeFill (Rows[1])
 *   ComboFill :262-490     [pcc].[USP_DropDownFillFromInvSaleInvoice] by Activity (ParentCategory, ItemCategory, ItemType, Item, JobLot, District, City, Customer, ReferenceParty,
 *                          RegisteredReferenceParty (checked list, Sales Person), RefSalesMan, CustomerGroup), ZeroIndex false
 *   CmbPartyGroup_Leave :516  the customers of the chosen party group (CustomerGroupId; 0 = all)
 *   ReportTypeFill :490    ten report types (Id = the long name, display = the short name), Rows[1]
 *   BtnShowSumm_Click :700 no report type -> "Please Select Activity First..."; GridSummaryFill :719 + PrintButtonManage :1366
 *   GridSummaryFill :719   InvSaleInvoice.SaleInvoiceSummaryRegister (pcc.USP_SaleInvoiceSummaryRegister): dates, doc nos, category, type, party group, customer, item, job lot, city, district,
 *                          sale man, ReferencePartyName (the text), RefPartyIds ("id,id,"), ReportType, IsApproved / All, average rate (only > 0, else "Average Rate Always Greater Than Zero"),
 *                          Add City, Add Varient; "City & Varient" with neither ticked -> "At least one checkbox (AddCity or Varient) must be checked."
 *                          (the parent category is filled in the parameter object but never read by the BLL, so it is not sent)
 *   DataGridHistorySetting :818  49 columns, HeaderLines 3, hidden Id / ItemId / SupplierCustomerId / DocumentTypeId, links CustomerName and DocNo, Variant columns with Add Varient,
 *                          CityName with Add City, widths from InventoryConstants, decimal columns by CommonServices.GridColumnSettings, per report type hidden columns (:890-1222)
 *   DataGridHistory_LinkClicked :1235  DocNo of a 1856 / 1861 invoice opens its concrete form; CustomerName opens the general ledger of the customer for the form dates
 *   btnPrintSumm_Click :1337  no rows -> "Record Not Found For Display"; blank name -> "Invalid report name."; <1861_NN-name>.rpt over the grid rows
 *   BtnNewSummary_Click :649 (clears the filters, Rows[1] of the report type, Show, focus From), btnRefreshSummary (ComboFill), KeyDown :585
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, el = A.el;
    var API = '/api/sale/sarpt1/sale-invoice-register';
    var lookup = { amountDecimals: 0, rateDecimals: 0, yearStart: null };
    var combos = {}, rows = [], lastArgs = null, lastTemplate = '', seq = 0;
    var SHORTCUTS = [['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+L', 'To Press Load Button'], ['Ctrl+S', 'For Show'], ['Ctrl+P', 'For Print'],
        ['Ctrl+F5', 'For Focus on From Date'], ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus On From Date'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];

    var REPORT_TYPES = [
        ['Sales Register', 'Sales Register'], ['Sales Summary DocNo_Customer & SalesMan', 'Sales Summary DocNo_Customer & SalesMan'],
        ['Sales Summary By Item_Varient & City', 'Sales Summary By Item'], ['Sales Summary By City & Varient', 'Sales Summary By City & Variant'],
        ['Sales Summary By Customer_Varient & City', 'Sales Summary By Customer'], ['Sales Summary By Customer_Item_Varient & City', 'Sales Summary By Customer_Item'],
        ['Sales Summary By ReferenceParty_Varient & City', 'Sales Summary By ReferenceParty'], ['Sales Summary By ReferenceParty_Customer_Varient & City', 'Sales Summary By ReferenceParty_Customer'],
        ['Sales Summary By ReferenceParty_Item_Varient & City', 'Sales Summary By ReferenceParty_Item'], ['Sales Summary By ReferenceParty_Customer_Item_Varient & City', 'Sales Summary By ReferenceParty_Customer_Item']
    ];
    // DataGridHistorySetting :890-1222 - the columns hidden by each report type
    var HIDE = {"Sales Register":["ItemNetAmount","AvgRate","PrcntOfTotalQty","PrcntOfTotalAmount"],"Sales Summary DocNo_Customer & SalesMan":["VisitedByName","DueDays","DueDate","DeliveryTerm","WareHouseName","ItemCode","ItemName","Variant","VariantEquivalent","VariantQty","CityName","ItemAmount","ItemDiscountType","ItemDiscountRate","ItemAmountWithDisc","ItemNetAmount"],"Sales Summary By Item_Varient & City":["DocDate","DocNo","DocumentType","CustomerName","ReferenceNo","ManualBillNo","VisitedByName","DueDays","DueDate","DeliveryTerm","BuildingStorey","BuildingHeight","BuildingArea","ReferencePartyName","ReferencePartyCellNo","ReferencePartyAddress","RefSalesMan","CommissionType","CommissionRate","ExFactoryAmount","NetDiscountAmount","LabourAmount","CarriageAmount","CommissionAmount","BillAmount","TransporterName","WareHouseName","ItemAmount","ItemDiscountType","ItemDiscountRate","ItemAmountWithDisc","CurrentGlBalance"],"Sales Summary By City & Varient":["DocDate","DocNo","DocumentType","CustomerName","ReferenceNo","ManualBillNo","VisitedByName","DueDays","DueDate","DeliveryTerm","BuildingStorey","BuildingHeight","BuildingArea","ReferencePartyName","ReferencePartyCellNo","ReferencePartyAddress","RefSalesMan","CommissionType","CommissionRate","ExFactoryAmount","NetDiscountAmount","LabourAmount","CarriageAmount","CommissionAmount","BillAmount","TransporterName","WareHouseName","ItemCode","ItemName","ItemAmount","ItemDiscountType","ItemDiscountRate","ItemAmountWithDisc","CurrentGlBalance"],"Sales Summary By Customer_Varient & City":["DocDate","DocNo","DocumentType","ReferenceNo","ManualBillNo","VisitedByName","DueDays","DueDate","DeliveryTerm","BuildingStorey","BuildingHeight","BuildingArea","ReferencePartyName","ReferencePartyCellNo","ReferencePartyAddress","RefSalesMan","CommissionType","CommissionRate","TransporterName","CommissionAmount","WareHouseName","ItemCode","ItemName","ItemAmount","ItemDiscountType","ItemDiscountRate","ItemAmountWithDisc"],"Sales Summary By Customer_Item_Varient & City":["DocDate","DocNo","DocumentType","ReferenceNo","ManualBillNo","VisitedByName","DueDays","DueDate","DeliveryTerm","BuildingStorey","BuildingHeight","BuildingArea","ReferencePartyName","ReferencePartyCellNo","ReferencePartyAddress","RefSalesMan","CommissionType","CommissionRate","CommissionAmount","TransporterName","WareHouseName","ItemAmount","ItemDiscountType","ItemDiscountRate","ItemAmountWithDisc"],"Sales Summary By ReferenceParty_Customer_Item_Varient & City":["DocDate","DocNo","DocumentType","ReferenceNo","ManualBillNo","VisitedByName","DueDays","DueDate","DeliveryTerm","BuildingStorey","BuildingHeight","BuildingArea","RefSalesMan","CommissionType","CommissionRate","CommissionAmount","TransporterName","WareHouseName","ItemQty","NetWeight","ItemAmount","ItemDiscountType","ItemDiscountRate","ItemAmountWithDisc","ItemNetAmount","AvgRate","PrcntOfTotalQty","PrcntOfTotalAmount"],"Sales Summary By ReferenceParty_Item_Varient & City":["DocDate","DocNo","DocumentType","CustomerName","ReferenceNo","ManualBillNo","VisitedByName","DueDays","DueDate","DeliveryTerm","BuildingStorey","BuildingHeight","BuildingArea","RefSalesMan","CommissionType","CommissionRate","CommissionAmount","TransporterName","WareHouseName","ItemQty","NetWeight","ItemAmount","ItemDiscountType","ItemDiscountRate","ItemAmountWithDisc","ItemNetAmount","AvgRate","PrcntOfTotalQty","PrcntOfTotalAmount","CurrentGlBalance"],"Sales Summary By ReferenceParty_Customer_Varient & City":["DocDate","DocNo","DocumentType","ReferenceNo","ManualBillNo","VisitedByName","DueDays","DueDate","DeliveryTerm","BuildingStorey","BuildingHeight","BuildingArea","RefSalesMan","CommissionType","CommissionRate","CommissionAmount","TransporterName","WareHouseName","ItemCode","ItemName","ItemQty","NetWeight","ItemAmount","ItemDiscountType","ItemDiscountRate","ItemAmountWithDisc","ItemNetAmount","AvgRate","PrcntOfTotalQty","PrcntOfTotalAmount"],"Sales Summary By ReferenceParty_Varient & City":["DocDate","DocNo","DocumentType","CustomerName","ReferenceNo","ManualBillNo","VisitedByName","ReferencePartyAddress","ReferencePartyCellNo","DueDays","DueDate","DeliveryTerm","BuildingStorey","BuildingHeight","BuildingArea","RefSalesMan","CommissionType","CommissionRate","CommissionAmount","TransporterName","WareHouseName","ItemCode","ItemName","ItemQty","NetWeight","ItemAmount","ItemDiscountType","ItemDiscountRate","ItemAmountWithDisc","ItemNetAmount","AvgRate","PrcntOfTotalQty","PrcntOfTotalAmount","CurrentGlBalance"]};

    // PrintButtonManage :1366 - [both, varient only, city only, none] -> 1861_NN-SalesSummaryBy<name>
    var PRINT = {
        'Sales Register': '1861_01-SalesRegister',
        'Sales Summary DocNo_Customer & SalesMan': '1861_17-SalesSummaryDocNo_Customer&SalesMan',
        'Sales Summary By Item_Varient & City': ['16-ItemCity&Varient', '02-Item&Varient', '15-Item&City', '03-Item'],
        'Sales Summary By City & Varient': ['04-City&Varient', '05-Varient', '06-City', '04-City&Varient'],
        'Sales Summary By Customer_Varient & City': ['07-CustomerVarient&City', '08-CustomerVarient', '09-CustomerCity', '13-Customer'],
        'Sales Summary By Customer_Item_Varient & City': ['10-CustomerItemVarient&City', '11-CustomerItemVarient', '12-CustomerItem&City', '14-CustomerItem'],
        'Sales Summary By ReferenceParty_Customer_Item_Varient & City': ['18-ReferencePartyCustomerItemVarient&City', '19-ReferencePartyCustomerItemVarient', '20-ReferencePartyCustomerItem&City', '21-ReferencePartyCustomerItem'],
        'Sales Summary By ReferenceParty_Item_Varient & City': ['22-ReferencePartyItemVarient&City', '23-ReferencePartyItemVarient', '24-ReferencePartyItem&City', '25-ReferencePartyItem'],
        'Sales Summary By ReferenceParty_Customer_Varient & City': ['26-ReferencePartyCustomerVarient&City', '27-ReferencePartyCustomerVarient', '28-ReferencePartyCustomer&City', '29-ReferencePartyCustomer'],
        'Sales Summary By ReferenceParty_Varient & City': ['30-ReferencePartyVarient&City', '31-ReferencePartyVarient', '32-ReferenceParty&City', '33-ReferenceParty']
    };
    function templateOf(type, city, varient) {
        var p = PRINT[type];
        if (!p) return '';
        if (typeof p === 'string') return p;
        var i = city && varient ? 0 : varient ? 1 : city ? 2 : 3;
        return '1861_' + p[i].replace(/^(\d+)-/, '$1-SalesSummaryBy');
    }

    // ------------------------------------------------------------------ columns (InventoryConstants widths, CommonServices.GridColumnSettings)
    // kind: s string | i int | d date | amt "Amount" decimal | rate "Rate" decimal | n other decimal
    var COLS = [
        ['Id', 0, 'i'], ['DocDate', 73, 'd'], ['DocNo', 60, 'i'], ['DocumentTypeId', 0, 's'], ['DocumentType', 150, 's'], ['SupplierCustomerId', 0, 'i'], ['CustomerName', 170, 's'],
        ['ReferenceNo', 60, 's'], ['ManualBillNo', 60, 's'], ['VisitedByName', 170, 's'], ['DueDays', 60, 'i'], ['DueDate', 73, 'd'], ['DeliveryTerm', 70, 's'],
        ['BuildingStorey', 80, 's'], ['BuildingHeight', 80, 's'], ['BuildingArea', 80, 's'], ['ReferencePartyName', 170, 's'], ['ReferencePartyCellNo', 100, 's'],
        ['ReferencePartyAddress', 150, 's'], ['RefSalesMan', 170, 's'], ['CommissionType', 115, 's'], ['CommissionRate', 70, 'rate'], ['ExFactoryAmount', 110, 'amt'],
        ['NetDiscountAmount', 110, 'amt'], ['LabourAmount', 110, 'amt'], ['TransporterName', 150, 's'], ['CarriageAmount', 110, 'amt'], ['CommissionAmount', 110, 'amt'],
        ['BillAmount', 110, 'amt'], ['WareHouseName', 150, 's'], ['ItemId', 0, 'i'], ['ItemCode', 70, 's'], ['ItemName', 150, 's'], ['Variant', 65, 's'], ['VariantEquivalent', 100, 'n'],
        ['VariantQty', 70, 'n'], ['CityName', 90, 's'], ['ItemQty', 70, 'n'], ['NetWeight', 80, 'n'], ['ItemAmount', 90, 'amt'], ['ItemDiscountType', 80, 's'],
        ['ItemDiscountRate', 70, 'rate'], ['ItemAmountWithDisc', 110, 'amt'], ['ItemNetAmount', 110, 'amt'], ['AvgRate', 70, 'rate'], ['PrcntOfTotalQty', 55, 'n'],
        ['PrcntOfTotalAmount', 55, 'amt'], ['CurrentGlBalance', 110, 'n']
    ];
    var BASE_HIDDEN = { Id: 1, ItemId: 1, SupplierCustomerId: 1, DocumentTypeId: 1 };
    function columns(type, city, varient) {
        var hide = {}; (HIDE[type] || []).forEach(function (k) { hide[k] = 1; });
        var single = S.fmtSingle(lookup.amountDecimals), rate = S.fmtRate(lookup.rateDecimals);
        return COLS.map(function (c) {
            var k = c[0], o = { key: k, caption: k, width: c[1] || undefined };
            o.hidden = !!(BASE_HIDDEN[k] || hide[k] || (!varient && (k === 'Variant' || k === 'VariantEquivalent' || k === 'VariantQty')) || (!city && k === 'CityName'));
            if (c[2] === 'd') o.date = 'dd-MMM-yy';
            else if (c[2] === 'amt') { o.fmt = single; o.totalFmt = single; o.sum = true; o.num = true; }
            else if (c[2] === 'rate') { o.fmt = rate; o.num = true; }
            else if (c[2] === 'n') { o.fmt = '#,##0.##'; o.totalFmt = '#,##0.##'; o.sum = true; o.num = true; }
            if (k === 'CustomerName' || k === 'DocNo') o.link = true;
            return o;
        });
    }
    function mapRow(r, varient) {
        var c = A.ci;
        return { Id: c(r, 'Id'), DocDate: c(r, 'DocDate'), DocNo: c(r, 'DocNo'), DocumentTypeId: c(r, 'DocumentTypeId'), DocumentType: c(r, 'DocumentTypeCode'),
            SupplierCustomerId: c(r, 'SupplierCustomerId'), CustomerName: c(r, 'CustomerName'), ReferenceNo: c(r, 'ReferenceNo'), ManualBillNo: c(r, 'ManualBillNo'),
            VisitedByName: c(r, 'VisitedByName'), DueDays: c(r, 'DueDays'), DueDate: c(r, 'DueDate'), DeliveryTerm: c(r, 'DeliveryTerm'), BuildingStorey: c(r, 'BuildingStorey'),
            BuildingHeight: c(r, 'BuildingHeight'), BuildingArea: c(r, 'BuildingArea'), ReferencePartyName: c(r, 'ReferencPartyName'), ReferencePartyCellNo: c(r, 'ReferencPartyCellNo'),
            ReferencePartyAddress: c(r, 'ReferencPartyAddress'), RefSalesMan: c(r, 'RefSalesMan'), CommissionType: c(r, 'CommissionType'), CommissionRate: c(r, 'CommissionRate'),
            ExFactoryAmount: c(r, 'ExFactoryAmountHeader'), NetDiscountAmount: c(r, 'NetDiscountAmountHeader'), LabourAmount: c(r, 'LabourAmountHeader'),
            TransporterName: c(r, 'TransporterName'), CarriageAmount: c(r, 'CarriageAmount'), CommissionAmount: c(r, 'CommissionAmount'), BillAmount: c(r, 'BillAmount'),
            WareHouseName: c(r, 'WareHouseName'), ItemId: c(r, 'ItemId'), ItemCode: c(r, 'ItemCode'), ItemName: c(r, 'ItemName'), Variant: varient ? c(r, 'Varient') : null,
            VariantEquivalent: c(r, 'VarientEquivalent'), VariantQty: c(r, 'VarientQty'), CityName: c(r, 'CityName'), ItemQty: c(r, 'ItemQty'), NetWeight: c(r, 'NetWeight'),
            ItemAmount: c(r, 'ItemAmount'), ItemDiscountType: c(r, 'ItemDiscountType'), ItemDiscountRate: c(r, 'ItemDiscountRate'), ItemAmountWithDisc: c(r, 'ItemAmountWithDisc'),
            ItemNetAmount: c(r, 'ItemNetAmount'), AvgRate: c(r, 'AvgRate'), PrcntOfTotalQty: c(r, 'PrcntOfTotalQty'), PrcntOfTotalAmount: c(r, 'PrcntOfTotalAmount'),
            CurrentGlBalance: c(r, 'CurrentGlBalance') };
    }

    var grid = new S.Grid({ tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', headerLines: 3, onLink: onLink });
    function onLink(row, col) {                                                // DataGridHistory_LinkClicked
        if (!col) return;
        var id = A.toInt(row.Id), dt = A.toInt(row.DocumentTypeId);
        if (col.key === 'DocNo') {
            if (dt === 1856 || dt === 1861) { if (w.DocLink) w.DocLink.open(dt, id); else alert('The invoice page is not available.'); }
        } else if (col.key === 'CustomerName') {
            A.getJson(API + '/gl-account?' + S.qs({ supplierCustomerId: A.toInt(row.SupplierCustomerId) })).then(function (o) {
                var acc = A.toInt(o && o.glAccountId);
                if (acc <= 0) { alert('No ledger account is linked to this customer.'); return; }
                w.open('/accounts/reports/general-ledger?' + S.qs({ accountId: acc, fromDate: el('txtFromDateSumm').value, toDate: el('txtToDateSumm').value }), '_blank');
            }).catch(function (e) { alert(e.message); });
        }
    }

    // ------------------------------------------------------------------ lists
    function list(k) { return combos[k] || []; }
    function bindCustomers() {                                                 // CmbPartyGroup_Leave
        var g = S.selInt('CmbPartyGroup'), all = list('Customer');
        var rs = all.filter(function (r) { return g === 0 || A.toInt(A.ci(r, 'CustomerGroupId')) === g; });
        if (rs.length) S.fill(el('cmbCustomerSumm'), rs, false, true);
    }
    function bindCombos() {
        S.fill(el('cmbParentcategorySumm'), list('ParentCategory'), false, true);
        S.fill(el('CmbCategorySumm'), list('ItemCategory'), false, true);
        S.fill(el('cmbItemtypeSumm'), list('ItemType'), false, true);
        S.fill(el('cmbItemNameSumm'), list('Item'), false, true);
        S.fill(el('cmbJobLotSumm'), list('JobLot'), false, true);
        S.fill(el('cmbDistrictSumm'), list('District'), false, true);
        S.fill(el('cmbcitySumm'), list('City'), false, true);
        S.fill(el('cmbCustomerSumm'), list('Customer'), false, true);
        S.fill(el('cmbRefPartyDetailSumm'), list('ReferenceParty'), false, true);
        S.fill(el('CmbRegisteredRefParty'), list('RegisteredReferenceParty'), false, true);
        S.fill(el('cmbsaleman'), list('RefSalesMan'), false, true);
        S.fill(el('CmbPartyGroup'), list('CustomerGroup'), false, true);
    }
    function comboFill() { return A.getJson(API + '/lookups').then(function (data) { lookup = data || lookup; combos = (data && data.combos) || {}; bindCombos(); }); }
    function parameterFill() { S.fill(el('CmbDateTypeSumm'), S.dateTypeRows(false), false); S.activate('CmbDateTypeSumm', 1, false); }
    function statusFill() { S.fill(el('CmbApprovalStatusSumm'), [{ Id: 1, name: 'UnApproved' }, { Id: 2, name: 'Approve' }, { Id: 3, name: 'All' }], false); S.activate('CmbApprovalStatusSumm', 2, false); }
    function reportTypeFill() { S.fill(el('cmbReportType'), REPORT_TYPES.map(function (r) { return { Id: r[0], name: r[1] }; }), false); S.activate('cmbReportType', 1, false); }
    el('CmbDateTypeSumm').addEventListener('change', function () {
        S.dateRule(A.toInt(this.value), el('txtFromDateSumm'), el('txtToDateSumm'), lookup.yearStart, { to5: true });
    });
    S.onLeave('CmbPartyGroup', bindCustomers);

    // ------------------------------------------------------------------ Show / print
    function printLabel(t) { el('printLabel').textContent = t || 'Print'; }
    function show() {
        var b = el('show'); if (b.disabled) return Promise.resolve();
        var type = el('cmbReportType').value;
        if (!type) { alert('Please Select Activity First...'); S.focus('cmbReportType'); return Promise.resolve(); }
        var city = el('chkCity').checked, varient = el('chkvarient').checked;
        var a = { fromDate: el('txtFromDateSumm').value, toDate: el('txtToDateSumm').value, fromDocNo: A.toIntText(el('txtFromDocNoSumm').value), toDocNo: A.toIntText(el('txtToDocNoSumm').value),
                  itemCategoryId: S.selInt('CmbCategorySumm'), itemTypeId: S.selInt('cmbItemtypeSumm'), customerGroupId: S.selInt('CmbPartyGroup'),
                  supplierCustomerId: S.selInt('cmbCustomerSumm'), itemId: S.selInt('cmbItemNameSumm'), jobLotId: S.selInt('cmbJobLotSumm'), cityId: S.selInt('cmbcitySumm'),
                  districtId: S.selInt('cmbDistrictSumm'), refSaleManId: S.selInt('cmbsaleman'), referencePartyName: S.selText('cmbRefPartyDetailSumm'),
                  reportType: type, statusId: S.selInt('CmbApprovalStatusSumm') };
        var ids = ''; A.checkedValues(el('CmbRegisteredRefParty')).forEach(function (v) { ids += v + ','; });
        if (ids) a.refPartyIds = ids;
        var rate = A.toDouble(el('txtavgrate').value);
        if (rate > 0) a.avgRate = rate; else alert('Average Rate Always Greater Than Zero');
        if (city) a.cityAdd = 1;
        if (varient) a.varientAdd = 1;
        if (type === 'Sales Summary By City & Varient' && !city && !varient) {
            rows = []; lastArgs = null; grid.clear(); printLabel(templateOf(type, city, varient));
            alert('At least one checkbox (AddCity or Varient) must be checked.'); return Promise.resolve();
        }
        var token = ++seq;
        A.busy(b, true);
        return A.getJson(API + '/rows?' + S.qs(a)).then(function (rs) {
            if (token !== seq) return;
            rows = rs || [];
            if (!rows.length) { lastArgs = null; grid.clear(); }
            else { lastArgs = a; grid.setData(columns(type, city, varient), rows.map(function (r) { return mapRow(r, varient); })); }
            lastTemplate = templateOf(type, city, varient); printLabel(lastTemplate);           // PrintButtonManage
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }
    function print() {
        var b = el('print'); if (b.disabled) return;
        var name = lastTemplate;
        if (!rows.length || !lastArgs) { alert('Record Not Found For Display'); return; }
        if (!name || !name.trim()) { alert('Invalid report name.'); return; }
        var q = {}; Object.keys(lastArgs).forEach(function (k) { q[k] = lastArgs[k]; }); q.template = name;
        A.busy(b, true);
        A.openPdf('/sale/reports/sarpt1/print/sale-invoice-register?' + S.qs(q)).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function neu() {                                                           // BtnNewSummary_Click
        ['cmbCustomerSumm', 'CmbCategorySumm', 'cmbParentcategorySumm', 'cmbItemNameSumm', 'cmbcitySumm', 'cmbDistrictSumm', 'CmbApprovalStatusSumm', 'cmbJobLotSumm'].forEach(function (id) {
            var s = el(id); s.value = ''; s.dispatchEvent(new Event('change', { bubbles: true }));
        });
        el('txtFromDocNoSumm').value = ''; el('txtToDocNoSumm').value = '';
        S.activate('cmbReportType', 1, false);
        show().then(function () { S.focus('txtFromDateSumm'); });
    }
    function refresh() {
        var b = el('refresh'); if (b.disabled) return;
        S.focus('txtFromDateSumm'); A.busy(b, true);
        comboFill().catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function closeForm() { w.location.href = '/sale/reports'; }

    S.digitsOnly('txtFromDocNoSumm'); S.digitsOnly('txtToDocNoSumm'); S.digitsOnly('txtavgrate', true);
    el('show').addEventListener('click', show);
    el('reset').addEventListener('click', neu);
    el('refresh').addEventListener('click', refresh);
    el('print').addEventListener('click', print);
    el('shortcuts').addEventListener('click', function () { A.shortcuts(SHORTCUTS); });
    S.closeShortcuts();
    S.keys({ s: show, l: show, e: closeForm, n: neu, r: refresh, p: print, F5: function () { S.focus('txtFromDateSumm'); }, ArrowUp: function () { S.focus('txtFromDateSumm'); },
             ArrowDown: function () { grid.focus(); }, alt: function () { A.shortcuts(SHORTCUTS); } }, closeForm);

    // ------------------------------------------------------------------ start (Load)
    el('txtFromDateSumm').value = A.today(); el('txtToDateSumm').value = A.today();
    grid.render();
    S.focus('CmbDateTypeSumm');
    comboFill().then(function () { parameterFill(); statusFill(); reportTypeFill(); }).catch(function (e) { alert(e.message); });
}(window, document));
