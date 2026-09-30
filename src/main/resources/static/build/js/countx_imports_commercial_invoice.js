/* ============================================================================================
 * countx_imports_commercial_invoice.js - screen 221 "IMPORT INVOICE" (Architecture.WinApp.Import/ImCommercialInvoice.cs).
 * <body data-mode> is the form's DocumentTypeId (233 against a purchase order, 237 against a contract).
 * The desktop's Sp_ImInvoice_GetAllMethod (doc no / history / edit) and Sp_ImInvoicePackingDetail_Insert (save) do not exist
 * in the database: those actions show the server's message. API /api/import/commercial-invoice/*. Exports window.ImpACi.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/import/commercial-invoice';
    var P = {};
    window.ImpACi = P;

    var S = {}, MODE = 233, RecId = 0;
    var grdDetail, grdShipmentExpenses, DataGridHistory, tabMain;

    function $(id) { return HRM.$(id); }
    function val(id) { return HRM.val(id); }
    function set(id, v) { HRM.setVal(id, v === null || v === undefined ? '' : v); }
    function num(v) { return HRM.num(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); }
    function box(m) { HRM.box(m); }
    function q(extra) { return Object.assign({ mode: MODE }, extra || {}); }

    // ------------------------------------------------------------------------ grids (grdSettings / GrdSettingShipmentExpenses)
    function buildGrids() {
        var whOpts = [['0', '']].concat((S.warehouses || []).map(function (w) { return [String(w.Id), HRM.str(w.WareHouseName)]; }));
        var jlOpts = [['0', '']].concat((S.jobLots || []).map(function (j) { return [String(j.Id), HRM.str(j.JobLotDescription)]; }));
        var c237 = MODE === 237;
        var cols = [];
        if (c237) cols.push(ImpA.btnCol('Delete', 'X', 26));                          // AllowDelete (237 only)
        cols = cols.concat([
            ImpA.hid('ItemId'), { key: 'ItemName', caption: 'ItemName', width: 220 },
            ImpA.hid('PackTypeId'), { key: 'PackType', caption: 'PackType', width: 150 },
            c237 ? { key: 'Qty/M.Ton', caption: 'Qty/M.Ton', type: 'edit-num', width: 90, onChange: grdDetail_CellUpdated }
                 : ImpA.numCol('Qty/M.Ton', 'Qty/M.Ton', '#,#.###', true),
            ImpA.hid('PackSizeId'), { key: 'PackSize', caption: 'PackSize' },
            ImpA.numCol('NoOfBags', 'NoOfBags', '0,0', true), ImpA.numCol('Cost/M.Ton', 'Cost/M.Ton', '0,0', false),
            ImpA.hid('RateUOMId'), { key: 'RateUOM', caption: 'RateUOM' },
            ImpA.numCol('Amount', 'Amount', '0,0', true),
            { key: 'ItemDetail', caption: 'ItemDetail', type: 'edit', width: 140 },
            c237 ? { key: 'WareHouseId', caption: 'WareHouseName', type: 'select', options: whOpts, width: 150 } : ImpA.hid('WareHouseId'),
            c237 ? ImpA.hid('WareHouseName') : { key: 'WareHouseName', caption: 'WareHouseName' },
            c237 ? { key: 'JobLotId', caption: 'JobLot', type: 'select', options: jlOpts, width: 180 } : ImpA.hid('JobLotId'),
            c237 ? ImpA.hid('JobLot') : { key: 'JobLot', caption: 'JobLot' },
            ImpA.numCol('AddLssAmount', 'AddLssAmount', '0,0', true), ImpA.numCol('Expenses', 'Expenses', '#,#.##', true)
        ]);
        grdDetail = new HRM.Grid('grdDetail', {
            columns: cols, totals: true, emptyText: '',
            onDraw: function (g) {
                ImpA.footer(g, {
                    'Qty/M.Ton': function (rs) { return ImpA.fmt(ImpA.sumOf(rs, 'Qty/M.Ton'), '#,#.###'); },
                    NoOfBags: function (rs) { return ImpA.fmt(ImpA.sumOf(rs, 'NoOfBags'), '#,##0.##'); },
                    Amount: function (rs) { return ImpA.fmt(ImpA.sumOf(rs, 'Amount'), '#,##0.##'); },
                    AddLssAmount: function (rs) { return ImpA.fmt(ImpA.sumOf(rs, 'AddLssAmount'), '#,##0.##'); },
                    Expenses: function (rs) { return ImpA.fmt(ImpA.sumOf(rs, 'Expenses'), '#,#.##'); }
                });
            }
        });
        if (c237) ImpA.buttons(grdDetail, 'grdDetail', function (r, act, i) {
            if (act === 'Delete') { grdDetail.remove(i); grdDetail_RecordsDeleted(); }
        });

        var accOpts = [['0', '']].concat((S.accounts || []).map(function (a) { return [String(a.Id), HRM.str(a.AccountTitle)]; }));
        grdShipmentExpenses = new HRM.Grid('grdShipmentExpenses', {
            columns: [
                { key: 'AccountId', caption: 'Account', type: 'select', options: accOpts, width: 250, onChange: function () { BillAmount(); } },
                { key: 'Remarks', caption: 'Remarks', type: 'edit', width: 300 },
                { key: 'FcAmount', caption: 'FcAmount', type: 'edit-num', width: 110, sum: true, onChange: function (r) { expenseUpdated(r, 'FcAmount'); } },
                { key: 'LocalAmount', caption: 'LocalAmount', type: 'edit-num', width: 110, sum: true, onChange: function (r) { expenseUpdated(r, 'LocalAmount'); } },
                ImpA.btnCol('Delete', 'X', 26), ImpA.btnCol('Add', '+', 26)
            ],
            totals: true, emptyText: '',
            onDraw: function (g) { ImpA.footer(g, { FcAmount: function (rs) { return ImpA.fmt(ImpA.sumOf(rs, 'FcAmount'), '#,#'); },
                LocalAmount: function (rs) { return ImpA.fmt(ImpA.sumOf(rs, 'LocalAmount'), '#,#'); } }); }
        });
        ImpA.buttons(grdShipmentExpenses, 'grdShipmentExpenses', function (r, act, i) { grdShipmentExpenses_ColumnButtonClick(act, i); });

        DataGridHistory = new HRM.Grid('DataGridHistory', {                             // HistoryGridSettings
            columns: [ImpA.hid('Id'), { key: 'InvoiceNo', caption: 'InvoiceNo', width: 100 }, { key: 'DocCode', caption: 'DocCode' },
                ImpA.dateCol('DocDate', 'DocDate'), { key: 'ContractNo', caption: 'ContractNo' }, { key: 'ContractDocNo', caption: 'ContractDocNo' },
                { key: 'Supplier', caption: 'Supplier' }, ImpA.numCol('NoOfContainers', 'NoOfContainers', '0,0', true),
                ImpA.numCol('GrossWeight', 'GrossWeight', '0,0', true), ImpA.numCol('NetWeight', 'NetWeight', '0,0', true),
                ImpA.numCol('FcyAmount', 'FcyAmount', '0,0', true), { key: 'Add/Less', caption: 'Add/Less' },
                ImpA.numCol('TotalAmount', 'TotalAmount', '0,0', true), { key: 'LoadingPort', caption: 'LoadingPort' },
                { key: 'DestinationPort', caption: 'DestinationPort' }, ImpA.btnCol('Print', 'Print', 50)],
            filterRow: true, totals: true, emptyText: 'No record found.'
        });
    }

    // ------------------------------------------------------------------------ load
    function fillCombos() {
        HRM.fill('cmbbranches', S.branches, 'Id', 'BranchName', { zero: false });        // branchesGetAll: Rows[0].Activate
        HRM.fill('cmbproject', S.projects, 'Id', 'ProjectName', { zero: false });        // projectGetAll
        HRM.fill('cmbSupCust', S.suppliers, 'Id', 'CompanyName', { zero: '' });           // CustomerGetAll
        HRM.fill('cmbNotifyParty1', S.suppliers, 'Id', 'CompanyName', { zero: '' });
        HRM.fill('cmbNotifyParty2', S.suppliers, 'Id', 'CompanyName', { zero: '' });
        HRM.fill('cmbdeliverytermnew', S.deliveryTerms, 'Id', 'Code', { zero: '' });     // DeliveryTermFill
        HRM.fill('cmbPaymentTermsNew', S.paymentTerms, 'Id', 'LcOrderTerm', { zero: '' }); // PaymentTermsFill
        HRM.fill('cmbLoadingPort', S.ports, 'Id', 'PortName', { zero: '' });             // LoadingPortFill
        HRM.fill('cmbDestinationPort', S.ports, 'Id', 'PortName', { zero: '' });
        HRM.fill('cmbfcycode', S.currencies, 'Id', 'CurrencyCode', { zero: '' });        // MultiCurrencyfill
        HRM.fill('CmbHomeCurrency', S.currencies, 'Id', 'CurrencyCode', { zero: '' });
        HRM.fill('cmbexporterBankNew', S.exporterBanks, 'Id', 'BranchName', { zero: '' }); // ImporterandExportBankFill
        HRM.fill('cmbimporterBankNew', S.importerBanks, 'Id', 'BranchName', { zero: '' });
        HRM.fillFixed('cmbcareiertype', [['1', 'By Sea'], ['2', 'By Air'], ['3', 'By Road']]); // CarierType: Rows[0]
        HRM.fill('cmbLcContractNo', [], 'Id', 'OrderNo', { zero: '' });
    }
    function load() {
        MODE = HRM.int(document.body.getAttribute('data-mode')) === 237 ? 237 : 233;
        HRM.text('lblPoNo', MODE === 233 ? 'Purchase Order' : 'Contract No');
        HRM.enable('CmbHomeCurrency', MODE !== 233);
        return HRM.loading(HRM.get(API + '/setup', q())).then(function (d) {
            S = d || {};
            buildGrids();
            fillCombos();
            set('txtDocDate', HRM.today()); set('txtIformDate', HRM.today());
            grdDetail.set([]);
            grdShipmentExpenses.set([]); AddRowInShipmentExpensesGrid();
            HRM.text('lblStatus', S.docNoError || '');
            if (S.docNoError) box(S.docNoError);                                        // generateCode's MessageBox
            HRM.focus('txtinvoiceno');
        }).catch(HRM.fail);
    }

    // ------------------------------------------------------------------------ supplier / order
    function supplierGl() {
        var r = HRM.comboRow('cmbSupCust', 'Id');
        return r ? HRM.int(r.GlAccountId) : -1;                   // SelectedRow.Cells[2] (GlAccountId); no supplier -> matches nothing
    }
    /** cmbSupCust_Leave_1: the orders of the supplier into the Purchase Order / Contract No combo. */
    function cmbSupCust_Leave() {
        var sup = HRM.comboVal('cmbSupCust');
        if (MODE === 237 && sup <= 0) return;
        HRM.fill('cmbLcContractNo', [], 'Id', 'OrderNo', { zero: '' });
        if (sup <= 0) return;
        HRM.get(API + '/orders', q({ supplierId: sup })).then(function (rows) {
            HRM.fill('cmbLcContractNo', rows || [], 'Id', 'OrderNo', { zero: '' });
        }).catch(HRM.fail);
    }
    /** cmbLcContractNo_Leave: header boxes and detail rows of the picked order / contract. */
    function cmbLcContractNo_Leave() {
        var id = HRM.comboVal('cmbLcContractNo');
        if (id <= 0) {
            ['cmbPaymentTermsNew', 'cmbdeliverytermnew', 'cmbexporterBankNew', 'cmbimporterBankNew', 'cmbLoadingPort', 'cmbDestinationPort', 'cmbfcycode']
                .forEach(function (c) { HRM.setCombo(c, 0); });
            grdDetail.set([]);
            return;
        }
        HRM.loading(HRM.get(API + '/order', q({ supplierId: HRM.comboVal('cmbSupCust'), orderId: id }))).then(function (d) {
            var h = d.header;
            HRM.setCombo('cmbPaymentTermsNew', h.PaymentTermId); HRM.setCombo('cmbdeliverytermnew', h.DeliveryTermId);
            HRM.setCombo('cmbexporterBankNew', h.ExporterBankId); HRM.setCombo('cmbimporterBankNew', h.ImporterBankId);
            HRM.setCombo('cmbLoadingPort', h.LoadingPortId); HRM.setCombo('cmbDestinationPort', h.DestinationPortId);
            HRM.setCombo('cmbfcycode', h.FcyId); HRM.setCombo('CmbHomeCurrency', h.HomeCurrencyId);
            set('txtfcyAmount', h.FcyAmount); set('txtExRate', h.ExchangeRate); set('txtLocalAmt', h.Amount);
            set('txtGrossWeight', h.GrossWeight); set('txtNetWeight', h.NetWeight); set('txtNoOfContainer', h.Containers);
            txtaddlessamount_TextChanged();
            grdDetail.set(d.details || []);
            if ((d.details || []).length) { getTotalnetWeightFromGrid(); ParporationHomeCurrencyAmount(); }
        }).catch(HRM.fail);
    }

    // ------------------------------------------------------------------------ calculations
    /** getTotalnetWeightFromGrid(): Net (and Gross for 237) = sum(Qty/M.Ton) * 1000, Fcy = sum(Amount), all "0,0". */
    function getTotalnetWeightFromGrid() {
        var kg = grdDetail.sum('Qty/M.Ton') * 1000, amt = grdDetail.sum('Amount');
        set('txtNetWeight', ImpA.fmt(kg, '0,0'));
        if (MODE === 237) set('txtGrossWeight', ImpA.fmt(kg, '0,0'));
        set('txtfcyAmount', ImpA.fmt(amt, '0,0'));
        txtaddlessamount_TextChanged();                               // txtfcyAmount_TextChanged
    }
    /** CalculateLocalAmount(): Local = Total * ExchangeRate ("#,#") when both > 0. */
    function CalculateLocalAmount() {
        var t = num(val('txtTotalNetAmount')), ex = num(val('txtExRate'));
        if (t > 0 && ex > 0) set('txtLocalAmt', ImpA.fmt(t * ex, '#,#'));
    }
    /** txtaddlessamount_TextChanged (also txtfcyAmount_TextChanged). */
    function txtaddlessamount_TextChanged() {
        var add = num(val('txtaddlessamount')), fcy = num(val('txtfcyAmount')), exp = 0, gl = supplierGl();
        if (fcy > 0) {
            grdShipmentExpenses.rows().forEach(function (r) {
                if (HRM.int(r.AccountId) > 0 && num(r.FcAmount) > 0 && gl === HRM.int(r.AccountId)) exp += num(r.FcAmount);
            });
            set('txtTotalNetAmount', ImpA.plain(fcy + exp + add));
        }
        CalculateLocalAmount();
        ParporationHomeCurrencyAmount();
    }
    /** ParporationHomeCurrencyAmount(): each row's AddLssAmount = AddLess / NetWeight * row kg. */
    function ParporationHomeCurrencyAmount() {
        var t = num(val('txtTotalNetAmount')), nw = num(val('txtNetWeight')), add = num(val('txtaddlessamount'));
        grdDetail.rows().forEach(function (r) { r.AddLssAmount = t > 0 && nw > 0 ? add / nw * (num(r['Qty/M.Ton']) * 1000) : 0; });
        grdDetail.draw();
    }
    /** BillAmount(). */
    function BillAmount() {
        var item = Math.round(grdDetail.sum('Amount')), credit = 0, gl = supplierGl();
        grdShipmentExpenses.rows().forEach(function (r) { if (HRM.int(r.AccountId) > 0 && gl === HRM.int(r.AccountId)) credit += num(r.FcAmount); });
        var bill = item + credit + num(val('txtaddlessamount'));
        set('txtTotalNetAmount', ImpA.fmt(ImpA.roundAway(bill, 0), '#,#'));
        CalculateLocalAmount();
        ShipmentExpensesProportion();
    }
    /** ShipmentExpensesProportion(): each row's Expenses = round(total expense Fc) / total M.Ton * row M.Ton. */
    function ShipmentExpensesProportion() {
        var nw = grdDetail.sum('Qty/M.Ton'), credit = grdShipmentExpenses.sum('FcAmount');
        grdDetail.rows().forEach(function (r) { r.Expenses = credit > 0 ? ImpA.roundAway(credit, 0) / nw * num(r['Qty/M.Ton']) : 0; });
        grdDetail.draw();
    }
    /** grdDetail_CellUpdated (Qty/M.Ton, 237 only). */
    function grdDetail_CellUpdated(r) {
        var mton = num(r['Qty/M.Ton']), pack = num(r.PackSize), ru = num(r.RateUOM), rate = num(r['Cost/M.Ton']);
        r.NoOfBags = mton * pack;
        r.Amount = mton > 0 && ru > 0 && rate > 0 ? mton * 1000 / ru * rate : 0;
        getTotalnetWeightFromGrid();
        grdDetail.draw();
    }
    function grdDetail_RecordsDeleted() { getTotalnetWeightFromGrid(); ShipmentExpensesProportion(); ParporationHomeCurrencyAmount(); }
    /** grdShipmentExpenses_CellUpdated. */
    function expenseUpdated(r, key) {
        var ex = num(val('txtExRate'));
        if (ex > 0) {
            if (key === 'FcAmount' && HRM.str(r.FcAmount) !== '') r.LocalAmount = num(r.FcAmount) * ex;
            if (key === 'LocalAmount' && HRM.str(r.LocalAmount) !== '') r.FcAmount = num(r.LocalAmount) / ex;
        }
        grdShipmentExpenses.draw();
        BillAmount();
    }
    function AddRowInShipmentExpensesGrid() { grdShipmentExpenses.add({ AccountId: 0, Remarks: '', FcAmount: 0, LocalAmount: 0 }); }
    function grdShipmentExpenses_ColumnButtonClick(act, i) {
        if (act === 'Delete') { grdShipmentExpenses.remove(i); if (!grdShipmentExpenses.rows().length) AddRowInShipmentExpensesGrid(); }
        if (act === 'Add') AddRowInShipmentExpensesGrid();
        BillAmount();
    }

    // ------------------------------------------------------------------------ New / Save
    function FormReset() {
        HRM.show('btnsave', true); HRM.show('btnupdate', false);
        RecId = 0;
        set('txtdocno', '');
        HRM.get(API + '/doc-no').catch(function (e) { HRM.text('lblStatus', e.message); box(e.message); });   // generateCode()
        HRM.fill('cmbbranches', S.branches, 'Id', 'BranchName', { zero: false });
        HRM.fill('cmbproject', S.projects, 'Id', 'ProjectName', { zero: false });
        HRM.fillFixed('cmbcareiertype', [['1', 'By Sea'], ['2', 'By Air'], ['3', 'By Road']]);
        set('txtDocDate', HRM.today());
        ['txtinvoiceno', 'txtcertificate', 'txtcertificateOfOrigion', 'txtLotRef', 'txtfcyAmount', 'txtExRate', 'txtLocalAmt', 'txtGrossWeight',
            'txtNetWeight', 'txtNoOfContainer', 'txtIformNo', 'txtaddlesscommnets', 'txtaddlessamount', 'txtTotalNetAmount'].forEach(function (id) { set(id, ''); });
        ['cmbNotifyParty1', 'cmbNotifyParty2', 'cmbSupCust', 'cmbimporterBankNew', 'cmbdeliverytermnew', 'cmbLoadingPort', 'cmbDestinationPort',
            'CmbHomeCurrency', 'cmbexporterBankNew', 'cmbfcycode'].forEach(function (id) { HRM.setCombo(id, 0); });
        HRM.fill('cmbLcContractNo', [], 'Id', 'OrderNo', { zero: '' });
        grdDetail.set([]);
        grdShipmentExpenses.set([]); AddRowInShipmentExpensesGrid();
        HRM.focus('txtinvoiceno');
    }
    P.btnNew = function () { tabMain.show('pgForm'); FormReset(); };
    function bodyOf() {
        return {
            id: RecId, branchesId: HRM.comboVal('cmbbranches'), projectsId: HRM.comboVal('cmbproject'), docNo: val('txtdocno'), docDate: val('txtDocDate'),
            invoiceNo: val('txtinvoiceno'), supplierId: HRM.comboVal('cmbSupCust'), orderId: HRM.comboVal('cmbLcContractNo'),
            notifyParty1: HRM.comboVal('cmbNotifyParty1'), notifyParty2: HRM.comboVal('cmbNotifyParty2'), lotRef: val('txtLotRef'),
            paymentTermId: HRM.comboVal('cmbPaymentTermsNew'), importerBankId: HRM.comboVal('cmbimporterBankNew'), exporterBankId: HRM.comboVal('cmbexporterBankNew'),
            deliveryTermId: HRM.comboVal('cmbdeliverytermnew'), loadingPortId: HRM.comboVal('cmbLoadingPort'), destinationPortId: HRM.comboVal('cmbDestinationPort'),
            carierType: HRM.comboText('cmbcareiertype'), fcyId: HRM.comboVal('cmbfcycode'), homeCurrencyId: HRM.comboVal('CmbHomeCurrency'),
            fcyAmount: val('txtfcyAmount'), exchangeRate: val('txtExRate'), localAmount: val('txtLocalAmt'), grossWeight: val('txtGrossWeight'),
            netWeight: val('txtNetWeight'), noOfContainers: val('txtNoOfContainer'), iFormNo: val('txtIformNo'), iFormDate: val('txtIformDate'),
            certificate1: val('txtcertificate'), certificate2: val('txtcertificateOfOrigion'), addLessComments: val('txtaddlesscommnets'),
            addLessAmount: val('txtaddlessamount'), totalAmount: val('txtTotalNetAmount'),
            details: grdDetail.rows().map(function (r) {
                return { itemId: HRM.int(r.ItemId), packTypeId: HRM.int(r.PackTypeId), qtyMTon: String(r['Qty/M.Ton']), packSizeId: HRM.int(r.PackSizeId),
                    noOfBags: String(r.NoOfBags), costMTon: String(r['Cost/M.Ton']), rateUomId: HRM.int(r.RateUOMId), amount: String(r.Amount),
                    wareHouseId: HRM.int(r.WareHouseId), jobLotId: HRM.int(r.JobLotId), itemDetail: HRM.str(r.ItemDetail),
                    addLessAmount: String(r.AddLssAmount), expenses: String(r.Expenses) };
            }),
            expenses: grdShipmentExpenses.rows().map(function (r) {
                return { accountId: HRM.int(r.AccountId), remarks: HRM.str(r.Remarks), fcAmount: String(r.FcAmount), localAmount: String(r.LocalAmount) };
            })
        };
    }
    /** btnsave_Click. */
    P.btnsave = function (b) {
        if (grdDetail.rows().length === 0) { box('Grid Record not found'); return; }
        if (!HRM.ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
        return HRM.busy(b, function () {
            return HRM.post(API + '/save?mode=' + MODE, bodyOf()).then(function (d) {
                box(d && d.message ? d.message : 'Save SuccessFully'); FormReset();
            }).catch(HRM.fail);
        });
    };
    P.btnupdate = P.btnsave;

    // ------------------------------------------------------------------------ History / Print
    function HistoryGridFill() {
        return HRM.loading(HRM.get(API + '/history')).then(function (rows) { DataGridHistory.set(rows || []); }).catch(HRM.fail);
    }
    /** btnPrint_Click: ImInvoiceSlip_806(RecId) -> 806-ImInvoiceSlip.rpt. */
    P.btnPrint = function (b) { return ImpA.checkedPrint(b, API + '/print-check', { id: RecId }, 'impa-806', { id: RecId }); };

    // ------------------------------------------------------------------------ wiring
    function wire() {
        tabMain = ImpA.tabs('tabMain', function (id) { if (id === 'pgHistory') HistoryGridFill(); });
        ImpA.tabs('tabDetail');
        HRM.footer(function () { tabMain.show('pgHistory'); });
        $('cmbSupCust').addEventListener('change', cmbSupCust_Leave);
        $('cmbLcContractNo').addEventListener('change', cmbLcContractNo_Leave);
        $('txtExRate').addEventListener('input', CalculateLocalAmount);
        $('txtaddlessamount').addEventListener('input', txtaddlessamount_TextChanged);
        ImpA.guard('txtExRate', true);
        HRM.keys({
            'ctrl+s': function () { if (tabMain.current === 'pgForm' && HRM.visible('btnsave') && RecId === 0) P.btnsave($('btnsave')); },
            'ctrl+u': function () { if (HRM.visible('btnupdate') && RecId > 0) P.btnsave($('btnupdate')); },
            'ctrl+n': function () { P.btnNew(); },
            'ctrl+t': function () { tabMain.show(tabMain.current === 'pgHistory' ? 'pgForm' : 'pgHistory'); },
            'ctrl+e': HRM.close, 'esc': HRM.close
        });
    }

    document.addEventListener('DOMContentLoaded', function () { wire(); load(); });
})();
