/* ============================================================================================
 * countx_export_invoice_against_forwarding.js - ExportInvoiceAgainstForwarding.cs (Architecture.WinApp.Export)
 * "Export Invoice" (InvSaleInvoice DocumentTypeId 208) and its loader LoadExportForwarding.cs.
 * Events: Load, New, Save, Update, Attechment, Refresh, Load Farwarding (+ loader Search / Load / Reset),
 * 103-Voucher, 319- Slip, 319A-Slip, txtDueDays / txtDocDate change, header Item Rate / Rate Uom TextChanged,
 * grd CellUpdated, party add/less CellUpdated / X / +, tab change, LoadAll, history SelectionChanged /
 * DoubleClick / Voucher / Print / PartySlip / Detail / Edit, form KeyDown - desktop messages and order.
 * Data: /api/export/invoice-against-forwarding.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var G = global.ExportG, S = global.ExportSF;
    var API = '/api/export/invoice-against-forwarding';
    var DOC_TYPE = 208;

    var RIGHTS = { Save: false, Update: false, Print: false };
    var ID = 0, VOUCHER_HEAD_ID = 0, APPROVED = false, UPDATE_MODE = false;
    var ACCOUNTS = [], OTHER_ITEMS = [], HIST = [], HIST_DETAIL = [];
    var SUPPLIER_GL_ID = '';            /* Q4: txtSupplierGLId - never filled by the desktop */
    var LOADER = { rows: [], supply: [], fyStart: '' };

    /* .NET Math.Round(double) - midpoint to even. */
    function roundEven(x) { var r = Math.round(x); if (Math.abs(x % 1) === 0.5) r = 2 * Math.round(x / 2); return r; }
    function hash2(v) { return S.fmtHash(v, 2); }

    var grd = S.editGrid('grd', [
        { key: 'ReferenceNo' }, { key: 'ItemName' }, { key: 'PackingType' }, { key: 'PackUom' },
        { key: 'ItemQty', num: true, dec: 2 }, { key: 'NetBillWeight', num: true, dec: 2 }, { key: 'AddLessWeight', num: true, dec: 2 }, { key: 'GrossWeight', num: true, dec: 2 },
        { key: 'ItemRate', edit: 'num' }, { key: 'RateUom', edit: 'num' }, { key: 'ItemAmount', num: true, dec: 2 },
        { key: 'GpNo' }, { key: 'VehicleNo' }, { key: 'BiltyNo' }, { key: 'CropYear' }, { key: 'WarehouseName' }, { key: 'JobLot' }, { key: 'ItemDescription', edit: 'text' }
    ], { totals: ['ItemQty', 'NetBillWeight', 'AddLessWeight', 'GrossWeight', 'ItemAmount'], onCellUpdated: grdCellUpdated });

    var PARTY_COLUMN = { key: 'AccountId', edit: 'list', list: [] };
    var grdParty = S.editGrid('GrdPartyAddless', [
        PARTY_COLUMN, { key: 'Percentage', edit: 'num' }, { key: 'Qty', edit: 'num' }, { key: 'Rate', edit: 'num' },
        { key: 'Debit', edit: 'num' }, { key: 'Credit', edit: 'num' }, { key: 'Remarks', edit: 'text' }
    ], { withX: true, plus: true, totals: ['Percentage', 'Debit', 'Credit'], onCellUpdated: partyCellUpdated, onDelete: partyDelete, onPlus: function () { addPartyRow(); } });

    var grdHistory = G.grid('grdHistory', [
        { key: 'DocNo', link: true }, { key: 'DocDate', date: true }, { key: 'CustomerName' }, { key: 'DueDate', date: true }, { key: 'ManualBillNo' },
        { key: 'RemarksHeader' }, { key: 'BillAmount', num: true, dec: 2 }, { key: 'EntryDate', date: true }, { key: 'EntryUser' }, { key: 'NoOfAttachments' }
    ], { buttons: [{ key: 'Voucher', text: 'Voucher' }, { key: 'View', text: 'Print' }, { key: 'PartySlip', text: 'PartySlip' }, { key: 'Detail', text: 'Detail' }, { key: 'Edit', text: 'Edit' }],
        totals: ['BillAmount'], emptyId: 'grdHistoryEmpty', onButton: historyButton, onSelect: historySelect, onDblClick: historyDoubleClick,
        onLink: function (i) { historyDoubleClick(i); } });

    var grdDetail = G.grid('grdDetail', [
        { key: 'ForwardingDocNo' }, { key: 'ReferenceNo' }, { key: 'ItemName' }, { key: 'ItemUOM' }, { key: 'PackingType' },
        { key: 'ItemQty', num: true, fmt: function (v) { return G.fmt(v, 0); } }, { key: 'NetBillWeight', num: true, fmt: function (v) { return G.fmt(v, 0); } },
        { key: 'AddLessWeight', num: true, fmt: function (v) { return G.fmt(v, 0); } }, { key: 'GrossWeight', num: true, fmt: function (v) { return G.fmt(v, 0); } },
        { key: 'ItemRate', num: true, fmt: function (v) { return G.fmt(v, 0); } }, { key: 'RateUOM' }, { key: 'ItemAmount', num: true, fmt: function (v) { return G.fmt(v, 0); } },
        { key: 'GpNo' }, { key: 'VehicleNo' }, { key: 'CropYear' }, { key: 'WareHouse' }, { key: 'JobLot' }, { key: 'Description' }
    ], { totals: ['ItemQty', 'NetBillWeight', 'AddLessWeight', 'GrossWeight', 'ItemAmount'] });

    var footerLabel = null;
    var tabs = G.tabs('main', ['tabForm', 'tabHistory'], function (p) { if (footerLabel) footerLabel(); if (p === 'tabHistory') getAll(50); });
    footerLabel = S.footerToggle('btnFooterHistory', tabs, 'tabForm', 'tabHistory');

    // ------------------------------------------------------------------------------ load

    /** InvfrmPurchaseInvoice_Load. */
    function load() {
        G.setText('txtDocDate', G.today()); G.setText('txtDueDate', G.today());
        return G.getJson(API + '/setup').then(function (d) {
            d = d || {};
            RIGHTS = d.rights || RIGHTS;
            ['docNo', 'customers', 'otherItems', 'partyAddLessAccounts'].forEach(function (k) { if (d[k + 'Error']) G.box(d[k + 'Error']); });
            if (G.netI(d.docNo) > 0) G.setText('txtdocno', d.docNo);
            G.$id('btnSave').disabled = !RIGHTS.Save;
            G.$id('btnPrint').disabled = !RIGHTS.Print;
            G.$id('btnUpdate').disabled = !RIGHTS.Update;
            ACCOUNTS = (d.partyAddLessAccounts || []).map(function (a) { return { Id: a.Id, name: a.AccountTitle }; });
            setPartyList();
            OTHER_ITEMS = d.otherItems || [];
            if ((d.customers || []).length) S.bindX('cmbPartyName', d.customers, 'Id', 'CompanyName', []);
            grd.draw([]);
            grdParty.draw([{ AccountId: 0, Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0, Remarks: '' }]);
            G.show('btnSave', true); G.show('btnUpdate', false);
            G.setChecked('ChkBoxSlip', true); G.setChecked('ChkBok', true);
            G.focus('txtDocDate');
        }).catch(function (e) { G.box(e.message); });
    }
    /* the value list of the Party Add/Less column (CoaAllocationAccountTitleByAccountTypeIds(null, "2,11,12,15")) */
    function setPartyList() { PARTY_COLUMN.list = ACCOUNTS; }

    // ------------------------------------------------------------------------------ amounts

    /** BillAmount: Σ ItemAmount - (Σ Debit - Σ Credit), Math.Round, "#,#.##". */
    function billAmount() {
        var items = grd.total('ItemAmount'), dr = grdParty.total('Debit'), cr = grdParty.total('Credit');
        G.setText('txtBillAmount', hash2(roundEven(items - (dr - cr))));
    }
    /** grd_CellUpdated: NetBillWeight = Gross + AddLess, ItemAmount = NetBillWeight / RateUom * ItemRate. */
    function grdCellUpdated(i, key, r) {
        if (key === 'ItemRate' || key === 'RateUom' || key === 'AddLessWeight') {
            r.NetBillWeight = G.netD(r.GrossWeight) + G.netD(r.AddLessWeight);
            r.ItemAmount = G.netD(r.NetBillWeight) / G.netD(r.RateUom) * G.netD(r.ItemRate);
            grd.draw();
        }
        billAmount();
    }
    /** textBox1_TextChanged / textBox1_TextChanged_1: the header Item Rate / Rate Uom into every row. */
    function headerChanged(field) {
        var v = G.netD(G.val(field === 'ItemRate' ? 'txtItemRateHeader' : 'txtRateUomHeader'));
        grd.rows.forEach(function (r) {
            r[field] = v;
            r.NetBillWeight = G.netD(r.GrossWeight) + G.netD(r.AddLessWeight);
            r.ItemAmount = G.netD(r.NetBillWeight) / G.netD(r.RateUom) * G.netD(r.ItemRate);
        });
        if (grd.rows.length) grd.draw();
        billAmount();
        partyAmountAgainstPercent();
    }
    /** PartyAddlessGridAmountAgainstPrcnt: the party grid's current row. */
    function partyAmountAgainstPercent() {
        var item = grdParty.rows[grdParty.current >= 0 ? grdParty.current : 0];
        if (!item) { G.box('Object reference not set to an instance of an object.'); return; }
        if (G.netI(item.Percentage) !== 0) {
            if (Math.abs(G.netI(item.Percentage)) > 100) { item.Percentage = 0; grdParty.draw(); G.box('Percentage mustbe less than 100'); return; }
            applyPercent(item);
        }
        grdParty.draw();
    }
    function applyPercent(item) {
        var tp = grd.total('ItemAmount') / 100.0 * G.netD(item.Percentage);
        if (tp > 0) { item.Credit = roundEven(tp); item.Debit = 0; } else { item.Debit = Math.abs(roundEven(tp)); item.Credit = 0; }
        item.Qty = 0; item.Rate = 0;
    }
    /** GrdPartyAddless_CellUpdated. */
    function partyCellUpdated(i, key, item) {
        if (key === 'Qty' || key === 'Rate') { item.Credit = G.netD(item.Qty) * G.netD(item.Rate); item.Debit = 0; item.Percentage = 0; }
        if (key === 'Percentage') {
            if (Math.abs(G.netI(item.Percentage)) > 100) { item.Percentage = 0; grdParty.draw(); G.box('Percentage mustbe less than 100'); return; }
            applyPercent(item);
        }
        if (key === 'Credit' && G.netD(item.Debit) > 0) item.Debit = 0;
        if (key === 'Debit' && G.netD(item.Credit) > 0) item.Credit = 0;
        if (key === 'AccountId' && G.netI(SUPPLIER_GL_ID) === G.netI(item.AccountId)) {            /* Q4 */
            grdParty.draw();
            G.box('You Can No Select Customer Account Here....');
            item.AccountId = 0;
            grdParty.draw();
            return;
        }
        grdParty.draw();
        billAmount();
    }
    function addPartyRow() { grdParty.rows.push({ AccountId: 0, Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0, Remarks: '' }); grdParty.draw(); }
    /** GrdPartyAddless_ColumnButtonClick "Delete": the row goes, the bill is recomputed, an empty grid gets a fresh row. */
    function partyDelete(i) {
        grdParty.rows.splice(i, 1);
        grdParty.draw();
        billAmount();
        if (grdParty.rows.length === 0) addPartyRow();
    }
    /** DuedaysCalculates (txtDueDays_TextChanged / DocDate_ValueChanged). */
    function dueDays() {
        var t = G.val('txtDueDays').trim();
        G.setText('txtDueDate', t !== '' ? G.addDays(G.val('txtDocDate'), G.netD(t)) : G.today());
    }

    // ------------------------------------------------------------------------------ form

    /** Reset (btnNew_Click). */
    function reset() {
        ID = 0; VOUCHER_HEAD_ID = 0; APPROVED = false; UPDATE_MODE = false;
        ['txtItemRateHeader', 'txtRateUomHeader', 'txtbillno', 'txtRemarks', 'txtDueDays'].forEach(function (id) { G.setText(id, ''); });
        G.setText('txtDueDate', G.today());
        grd.draw([]);
        G.show('btnSave', true); G.show('btnUpdate', false);
        G.setEnabled('cmbPartyName', true); G.setVal('cmbPartyName', '0');
        G.setText('txtBillAmount', '0');
        OTHER_ITEMS.forEach(function (o) { o.Amount = 0; o.Remarks = ''; });
        grdParty.current = -1;
        grdParty.draw([{ AccountId: 0, Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0, Remarks: '' }]);
        G.focus('cmbPartyName');
        return G.getJson(API + '/setup').then(function (d) { if (d && G.netI(d.docNo) > 0) G.setText('txtdocno', d.docNo); }).catch(function (e) { G.box(e.message); });
    }
    /** btnFrmRefresh_Click: SupplierNameFilll. */
    function refreshCustomers(btn) {
        return G.busy(btn, function () {
            return G.getJson(API + '/customers').then(function (rows) { if ((rows || []).length) S.bindX('cmbPartyName', rows, 'Id', 'CompanyName', []); }).catch(function (e) { G.box(e.message); });
        });
    }
    /** Insert(): FormValidation, confirm, then the server; afterwards Reset and the two optional prints. */
    function insert(btn) {
        return G.busy(btn, function () {
            if (G.valI('cmbPartyName') === 0) { G.box('Party Name is Required'); G.focus('cmbPartyName'); return Promise.resolve(); }
            var docNo = G.val('txtdocno').trim();
            if (docNo === '' || docNo === '0') { G.box('Doc No is Required'); G.focus('txtdocno'); return Promise.resolve(); }
            if (!G.ask(ID > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return Promise.resolve();
            for (var i = 0; i < OTHER_ITEMS.length; i++) if (G.netD(OTHER_ITEMS[i].Amount) > 0 && G.netI(OTHER_ITEMS[i].ItemId) === 0) { G.box('Please Select an Item Against Expense First'); return Promise.resolve(); }
            if (grd.rows.length === 0) { G.box('Grid Record Not Found'); return Promise.resolve(); }
            var printVoucher = G.checked('ChkBok'), printSlip = G.checked('ChkBoxSlip');
            return G.postJson(API + '/save', { id: ID, docNo: docNo, docDate: G.val('txtDocDate'), manualBillNo: G.val('txtbillno'), remarks: G.val('txtRemarks'),
                dueDays: G.val('txtDueDays'), dueDate: G.val('txtDueDate'), supplierCustomerId: G.valI('cmbPartyName'), billAmountText: G.val('txtBillAmount'),
                rows: grd.rows, journals: grdParty.rows, expenses: OTHER_ITEMS }).then(function (d) {
                G.box(d && d.message);
                var savedId = G.netI(d && d.id), vh = G.netI(d && d.voucherHeadId);
                return reset().then(function () {
                    if (printVoucher) { if (vh === 0) G.box('No Record Found For Display'); else S.print('acc-103', { id: vh, documentTypeId: DOC_TYPE }); }
                    if (printSlip) S.print('exp-319', { id: savedId });
                });
            }).catch(function (e) { G.box(e.message); });
        });
    }
    function save(btn) { ID = 0; return insert(btn); }
    /** btnUpdate_Click. */
    function update(btn) {
        if (APPROVED) { G.box('Record Not Update because Record has approved'); return Promise.resolve(); }
        if (ID === 0) { G.box('Record Not Update because Record Id not found'); return Promise.resolve(); }
        return insert(btn);
    }
    /** btnPrint_Click: GenerateReportVoucher103 (Print right = btnPrint.Enabled). */
    function voucher103(vhId, btn) {
        if (!vhId) { G.box('No Record Found For Display'); return; }
        S.print('acc-103', { id: vhId, documentTypeId: DOC_TYPE }, btn);
    }
    function slip(key, id, btn) { if (!id) { G.box('No Record Found For Display'); return; } S.print(key, { id: id }, btn); }

    /** ReadById_Update. */
    function readById(id) {
        G.show('btnSave', false); G.show('btnUpdate', true);
        UPDATE_MODE = true;
        ID = id;
        return G.getJson(API + '/by-id?id=' + id).then(function (h) {
            G.setText('txtdocno', h.DocNo);
            G.setText('txtDocDate', h.DocDate);
            tabs.select('tabForm'); if (footerLabel) footerLabel();
            G.setVal('cmbPartyName', h.SupplierCustomerId); G.setEnabled('cmbPartyName', false);
            G.setText('txtbillno', h.ManualBillNo);
            G.setText('txtRemarks', h.RemarksHeader);
            G.setText('txtBillAmount', G.str(h.BillAmount));
            G.setText('txtDueDate', h.DueDate);
            G.setText('txtDueDays', G.str(h.DueDays));
            APPROVED = !!h.IsApproved;
            VOUCHER_HEAD_ID = G.netI(h.VoucherHeadId);
            grd.draw(h.rows || []);
            grdParty.current = -1;
            grdParty.draw(h.journals || []);
            /* Q7: the header boxes from the HISTORY detail grid's current row - and their TextChanged rewrites every row. */
            var cur = HIST_DETAIL[grdDetail.current >= 0 ? grdDetail.current : 0];
            if (!cur) { G.box('Object reference not set to an instance of an object.'); return; }
            G.setText('txtItemRateHeader', G.str(G.netD(cur.ItemRate))); headerChanged('ItemRate');
            G.setText('txtRateUomHeader', G.str(G.netD(cur.RateUOM))); headerChanged('RateUom');
            tabs.select('tabForm'); if (footerLabel) footerLabel();
        }).catch(function (e) { G.box(e.message); });
    }

    // ------------------------------------------------------------------------------ history

    /** GetAll(NoOfRecords). */
    function getAll(n, btn) {
        return G.busy(btn, function () {
            return G.getJson(API + '/history?noOfRecords=' + (n || 0)).then(function (rows) {
                HIST = rows || []; grdHistory.current = -1; grdHistory.draw(HIST);
            }).catch(function (e) { G.box(e.message); });
        });
    }
    /** GetDetailGrdByHeadId (grdHistory_SelectionChanged / Detail button). */
    function loadDetail(i) {
        var r = HIST[i]; if (!r) return Promise.resolve();
        return G.getJson(API + '/by-id?id=' + G.netI(r.Id)).then(function (h) {
            HIST_DETAIL = (h.rows || []).map(function (d) {
                return { ForwardingId: d.InvforwardingId, ForwardingDocNo: d.ForwardingDocNo, ReferenceNo: d.ReferenceNo, ItemId: d.ItemId, ItemName: d.ItemName,
                    ItemUOM: d.PackUom, PackingType: d.PackingType, ItemQty: d.ItemQty, NetBillWeight: d.NetBillWeight, AddLessWeight: d.AddLessWeight,
                    GrossWeight: d.GrossWeight, ItemRate: d.ItemRate, RateUOM: d.RateUom, ItemAmount: d.ItemAmount, GpNo: d.GpNo, VehicleNo: d.VehicleNo,
                    CropYear: d.CropYear, WareHouse: d.WarehouseName, JobLot: d.JobLot, Description: d.ItemDescription };
            });
            grdDetail.current = HIST_DETAIL.length ? 0 : -1;
            grdDetail.draw(HIST_DETAIL);
        }).catch(function (e) { G.box(e.message); });
    }
    function historySelect(i) { loadDetail(i); }
    function historyDoubleClick(i) { var r = HIST[i]; if (r) readById(G.netI(r.Id)); }
    /** grdHistory_ColumnButtonClick. */
    function historyButton(i, key, btn) {
        var r = HIST[i]; if (!r) return;
        if (key === 'Edit') { reset().then(function () { readById(G.netI(r.Id)); }); }
        if (key === 'Voucher') voucher103(G.netI(r.VoucherHeadId), btn);
        if (key === 'View') slip('exp-319', G.netI(r.Id), btn);
        if (key === 'PartySlip') slip('exp-319a', G.netI(r.Id), btn);
        if (key === 'Detail') loadDetail(i);
    }

    // ------------------------------------------------------------------------------ loader dialog (LoadExportForwarding)

    var dlg = S.dialog('loaderModal');
    /** toolStripButton3_Click_1: the loader while Save is available, otherwise Reset. */
    function loadForwarding() {
        if (G.$id('btnSave').classList.contains('is-hidden') || G.$id('btnSave').disabled) { reset(); return; }
        LOADER.supply = [];
        LOADER.rows = [];
        drawLoader();
        G.setText('Todate', G.today());
        dlg.open();
        G.focus('FromDate');
        return G.getJson(API + '/loader/combos').then(function (d) {
            d = d || {};
            ['financialYearStart', 'parties', 'items'].forEach(function (k) { if (d[k + 'Error']) G.box(d[k + 'Error']); });
            LOADER.fyStart = G.str(d.financialYearStart);
            G.setText('FromDate', LOADER.fyStart || G.today());
            if ((d.parties || []).length) G.bind('CmbPartyName', d.parties, 'Id', 'Name');
            if ((d.items || []).length) G.bind('cmbItem', d.items, 'Id', 'Name');
            return loaderSearch(null);
        }).catch(function (e) { G.box(e.message); });
    }
    /** ExportInvoicesLoad (the grid is only re-bound when rows came back). */
    function loaderSearch(btn) {
        return G.busy(btn, function () {
            var q = '?partyId=' + G.valI('CmbPartyName') + '&fromDate=' + encodeURIComponent(G.val('FromDate')) + '&toDate=' + encodeURIComponent(G.val('Todate'));
            return G.getJson(API + '/loader' + q).then(function (rows) {
                rows = rows || [];
                if (rows.length > 0) { LOADER.rows = rows; drawLoader(); }
            }).catch(function (e) { G.box(e.message); });
        });
    }
    function drawLoader() {
        var t = G.$id('loaderGrd'), s = { q: 0, n: 0, a: 0, g: 0, w: 0 };
        t.tBodies[0].innerHTML = LOADER.rows.map(function (r, i) {
            s.q += G.netD(r.Qty); s.n += G.netD(r.NetWeight); s.a += G.netD(r.AdLsWeight); s.g += G.netD(r.GrossWeight); s.w += G.netD(r.StockWeight);
            return '<tr data-i="' + i + '"><td class="ctr"><input type="checkbox" class="win-rowcheck" data-i="' + i + '"/></td><td>' + G.esc(G.ddMMMyyyy(r.DocDate)) + '</td>'
                + '<td>' + G.esc(r.DocNo) + '</td><td>' + G.esc(r.ReferenceNo) + '</td><td>' + G.esc(r.VehicleNo) + '</td><td>' + G.esc(r.BiltyNo) + '</td><td>' + G.esc(r.GpNo) + '</td>'
                + '<td>' + G.esc(r.CustomerName) + '</td><td>' + G.esc(r.ItemName) + '</td><td>' + G.esc(r.ItemDescription) + '</td><td>' + G.esc(r.WareHouseName) + '</td>'
                + '<td>' + G.esc(r.JobLotDescription) + '</td><td>' + G.esc(r.CropYear) + '</td><td>' + G.esc(r.PackingType) + '</td><td>' + G.esc(r.PackUOM) + '</td>'
                + '<td class="num">' + G.esc(G.fmt(r.Qty, 0)) + '</td><td class="num">' + G.esc(G.fmt(r.NetWeight, 0)) + '</td><td class="num">' + G.esc(G.fmt(r.AdLsWeight, 0)) + '</td>'
                + '<td class="num">' + G.esc(G.fmt(r.GrossWeight, 0)) + '</td><td class="num">' + G.esc(G.fmt(r.StockWeight, 0)) + '</td></tr>';
        }).join('');
        t.tFoot.innerHTML = LOADER.rows.length ? '<tr><td colspan="15"></td><td class="num">&Sigma; ' + G.fmt(s.q) + '</td><td class="num">&Sigma; ' + G.fmt(s.n) + '</td><td class="num">&Sigma; '
            + G.fmt(s.a) + '</td><td class="num">&Sigma; ' + G.fmt(s.g) + '</td><td class="num">&Sigma; ' + G.fmt(s.w) + '</td></tr>' : '';
        G.$id('loaderAll').checked = false;
    }
    /** btnLoadOnInvoice_Click_1: the checked rows must be of one customer; dtSupply = their InvforwardingId. */
    function loaderLoad() {
        var checked = [];
        G.$id('loaderGrd').querySelectorAll('input.win-rowcheck:checked').forEach(function (c) { checked.push(LOADER.rows[G.netI(c.getAttribute('data-i'))]); });
        if (checked.length === 0) { G.box('Check the row first...'); return; }
        var j = 0, supply = [];
        for (var k = 0; k < checked.length; k++) {
            var cust = G.netI(checked[k].SupplierCustomerId);
            if (cust === 0) continue;
            if (j === 0) j = cust;
            if (j !== cust) { LOADER.supply = []; G.box('Select Rows Shoulb be Of Same Customer'); return; }
            supply.push(G.netI(checked[k].ExImForwardingId));
            j = cust;
        }
        LOADER.supply = supply;
        dlg.close();
        loadInGridDetail();
    }
    /** LoadInGridDetail: the grid is cleared, then every pending line of the chosen forwardings. */
    function loadInGridDetail() {
        grd.rows = [];
        var ids = '';
        LOADER.supply.forEach(function (id) { ids = ids + ',' + id; });
        var p = ids === '' ? Promise.resolve([]) : G.getJson(API + '/forwarding-rows?ids=' + encodeURIComponent(ids));
        return p.then(function (rows) {
            rows = rows || [];
            if (rows.length > 0) {
                G.setVal('cmbPartyName', G.netI(rows[0].SupplierCustomerId)); G.setEnabled('cmbPartyName', false);
                rows.forEach(function (r) {
                    grd.rows.push({ Id: G.netI(r.Id), InvforwardingId: G.netI(r.ExImForwardingId), ReferenceNo: G.str(r.ReferenceNo), ItemId: G.netI(r.ItemId), ItemName: G.str(r.ItemName),
                        PackingTypeId: G.netI(r.PackingMaterialId), PackingType: G.str(r.PackingType), PackUomId: G.netI(r.UOMScheduleIdOuter), PackUom: G.str(r.PackUOM),
                        ItemQty: G.netD(r.Qty), NetBillWeight: G.netD(r.NetWeight), AddLessWeight: G.netD(r.AdLsWeight), GrossWeight: G.netD(r.GrossWeight),
                        ItemRate: 0, RateUomId: 0, RateUom: 40, ItemAmount: 0, GpNo: G.str(r.GpNo), VehicleNo: G.str(r.VehicleNo), BiltyNo: G.str(r.BiltyNo),
                        CropYear: G.str(r.CropYear), WarehouseId: G.netI(r.WarehouseId), WarehouseName: G.str(r.WareHouseName), JobLotId: G.netI(r.LotJobId),
                        JobLot: G.str(r.JobLotDescription), ItemDescription: G.str(r.ItemDescription) });
                });
            }
            grd.draw();
            billAmount();
        }).catch(function (e) { G.box(e.message); });
    }
    /** btnReset_Click. */
    function loaderReset(btn) {
        G.setVal('CmbPartyName', '0');
        return G.busy(btn, function () {
            return G.getJson(API + '/loader/combos').then(function (d) {
                d = d || {};
                if ((d.parties || []).length) G.bind('CmbPartyName', d.parties, 'Id', 'Name');
                if ((d.items || []).length) G.bind('cmbItem', d.items, 'Id', 'Name');
                return loaderSearch(null);
            }).catch(function (e) { G.box(e.message); });
        });
    }

    // ------------------------------------------------------------------------------ wiring

    function on(id, ev, fn) { var e = G.$id(id); if (e) e.addEventListener(ev, fn); }

    document.addEventListener('DOMContentLoaded', function () {
        G.initFullscreen();
        on('btnNew', 'click', reset);
        on('btnSave', 'click', function () { save(this); });
        on('btnUpdate', 'click', function () { update(this); });
        on('btnAttachment', 'click', function () { G.box('Attachments are not available in the web version.'); });
        on('btnFrmRefresh', 'click', function () { refreshCustomers(this); });
        on('toolStripButton3', 'click', loadForwarding);
        on('btnPrint', 'click', function () { voucher103(VOUCHER_HEAD_ID, this); });
        on('btnSlip220A', 'click', function () { slip('exp-319', ID, this); });
        on('btn220bSummary', 'click', function () { slip('exp-319a', ID, this); });
        on('txtDueDays', 'input', dueDays);
        on('txtDocDate', 'change', dueDays);
        on('txtItemRateHeader', 'input', function () { headerChanged('ItemRate'); });
        on('txtRateUomHeader', 'input', function () { headerChanged('RateUom'); });
        on('btnLoadAll', 'click', function () { getAll(0, this); });
        on('btngrnlod', 'click', function () { loaderSearch(this); });
        on('btnLoadOnInvoice', 'click', loaderLoad);
        on('btnReset', 'click', function () { loaderReset(this); });
        on('loaderClose', 'click', function () { LOADER.supply = []; dlg.close(); loadInGridDetail(); });     /* Q6 */
        on('loaderAll', 'change', function () { var v = this.checked; G.$id('loaderGrd').querySelectorAll('input.win-rowcheck').forEach(function (c) { c.checked = v; }); });
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase(), onForm = tabs.current() === 'tabForm';
            if (dlg.isOpen()) { if (e.key === 'Escape') { e.preventDefault(); LOADER.supply = []; dlg.close(); loadInGridDetail(); } return; }
            if (S.enterAsTab(e)) return;
            if (e.ctrlKey && k === 's' && onForm && !G.$id('btnSave').classList.contains('is-hidden') && !G.$id('btnSave').disabled) { e.preventDefault(); save(G.$id('btnSave')); }
            if (e.ctrlKey && k === 'n' && onForm) { e.preventDefault(); reset(); }
            if (e.ctrlKey && k === 't') { e.preventDefault(); tabs.select(onForm ? 'tabHistory' : 'tabForm'); }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { if (!document.querySelector('.ex-fullscreen')) { e.preventDefault(); G.cancelWindow(); } }
            if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); G.focus('txtDocDate'); }
            if (e.ctrlKey && k === 'u' && !G.$id('btnUpdate').classList.contains('is-hidden') && !G.$id('btnUpdate').disabled) { e.preventDefault(); update(G.$id('btnUpdate')); }
            if (e.ctrlKey && k === 'p' && !G.$id('btnPrint').disabled) { e.preventDefault(); voucher103(VOUCHER_HEAD_ID, G.$id('btnPrint')); }
            if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); G.box('Attachments are not available in the web version.'); }
            if (e.ctrlKey && k === 'l') { e.preventDefault(); loadForwarding(); }
            if (e.ctrlKey && e.key === 'Enter' && !onForm && grdHistory.current >= 0) { e.preventDefault(); readById(G.netI(HIST[grdHistory.current].Id)); }
            if (e.ctrlKey && e.key === 'ArrowDown' && onForm) { e.preventDefault(); G.focus('grd'); }
        });
        load();
    });

    global.ExportInvoiceAgainstForwarding = { reset: reset, save: save, update: update, readById: readById, loadForwarding: loadForwarding };
})(window);
