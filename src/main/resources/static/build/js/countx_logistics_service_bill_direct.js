/* ============================================================================================
 * countx_logistics_service_bill_direct.js - 233 "Service Bill Direct"
 * (Architecture.WinApp.Service/ExImClearingAgentBillDirect.cs, DocumentTypeId 155). Built on countx_hrm.js.
 *
 * Form tab: InitializeComponentMethod, cmbeximinvoiceno_Leave / BindInvoiceData, ItemCategoryOrMasterItemBind,
 * ItemDtsFillFromGlobal, ItemNameBind, cmbitem_Leave, totalamount, DueDaysCalculate, btnplus / grddetailadd
 * double-click / btnUpdateDetail / DeleteDetailRow, Insert (Save / Update), ReadById, reset, btnDelete, prints.
 * History tab: HistoryGridFill, GridHistorySetting buttons (Slip / Voucher / Add Attachment), selection detail.
 * The server repeats every check and recomputes the amounts.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/logistics/service-bill-direct';
    var DOC_TYPE = 155;
    var P = {};
    window.LgsSB = P;

    if (window.DesktopCombo) {
        /* dtItem: Id, ItemName, ItemCode, MasterItemId (hidden), MasterItem, ItemCategoryId (hidden), ItemCategory. */
        window.DesktopCombo.define('lgsaItem', [
            { caption: 'Item', flex: 4 }, { caption: 'Code / Name', flex: 2, key: 'alt' },
            { caption: 'MasterItem', flex: 2, key: 'master' }, { caption: 'ItemCategory', flex: 2, key: 'cat' }
        ]);
    }

    var RecId = 0, VoucherHeadId = 0, updateDetailIndex = -1, rights = {}, cfg = {};
    var L = { items: [], parties: [], ports: [], currencies: [], accounts: [], invoices: [], deliveredAt: [] };
    var dtItem = [];

    // ------------------------------------------------------------------------ number text (.NET custom formats)
    function roundAway(v, d) { var p = Math.pow(10, d), x = Math.abs(v) * p; var r = Math.floor(x + 0.5 + 1e-9) / p; return v < 0 ? -r : r; }
    /** "#,##0.##" / "#,##0.####": grouped, up to d places, trailing zeros dropped, 0 shows "0". */
    function fmtHash(v, d) {
        var n = roundAway(HRM.num(v), d);
        var s = n.toFixed(d).replace(/\.?0+$/, '');
        var neg = s.charAt(0) === '-'; if (neg) s = s.slice(1);
        var p = s.split('.'); p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return (neg ? '-' : '') + p.join('.');
    }

    // ------------------------------------------------------------------------ combos
    function fill(id, rows, valueKey, textKey, zero, attrs, keepValue) {
        var sel = HRM.$(id); if (!sel) return;
        var keep = keepValue !== undefined ? String(keepValue) : sel.value;
        var html = zero === false ? '' : '<option value="0">' + HRM.esc(zero || '') + '</option>';
        (rows || []).forEach(function (r) {
            var extra = '';
            Object.keys(attrs || {}).forEach(function (a) { extra += ' data-' + a + '="' + HRM.esc(HRM.col(r, attrs[a])) + '"'; });
            html += '<option value="' + HRM.esc(HRM.col(r, valueKey)) + '"' + extra + '>' + HRM.esc(HRM.col(r, textKey)) + '</option>';
        });
        sel.innerHTML = html;
        sel._rows = rows || [];
        if (keep && keep !== '0' && HRM.hasOption(id, keep)) sel.value = keep;
        else if (zero !== false) sel.value = '0';
        else sel.selectedIndex = -1;
        HRM.refreshCombos();
    }
    /** BindAndRetainSelection(insertDefaultRow: false, ActivateRow: false): an empty slot, no default row. */
    function fillNoDefault(id, rows, valueKey, textKey, attrs) { fill(id, rows, valueKey, textKey, '', attrs); }

    function InvoiceNoBind() { fillNoDefault('CmbInvoiceNo', L.invoices, 'Id', 'InvoiceNo'); }
    function BindSupplierName() { fillNoDefault('CmbPartyName', L.parties, 'Id', 'CompanyName', { code: 'PartyCode', city: 'CityName', mobile: 'MobileNo' }); }
    function PortsBind() {
        ['CmbLoadingPortInvoiceData', 'CmbDestinationPortInvoiceData', 'CmbContainerDispatchedAtPortInvoiceData', 'CmbContainerReceivedFromPortInvoiceData']
            .forEach(function (c) { fillNoDefault(c, L.ports, 'Id', 'PortName'); });
    }
    function DeliveredAtBind() { fillNoDefault('CmbDeliveredAtInvoiceData', L.deliveredAt, 'Id', 'Name'); }
    function CurrencyBindFromGlobal() { fillNoDefault('CmbFcyCodeDetail', L.currencies, 'Id', 'CurrencyCode'); }
    function AccountlstDtFillFromGlobal() { fillNoDefault('CmbDebitAc', L.accounts, 'Id', 'AccountTitle', { code: 'AccountCode' }); }

    /** ItemCategoryOrMasterItemBind: distinct categories or master items of the services items. */
    function ItemCategoryOrMasterItemBind() {
        if (!L.items.length) return;
        var cat = HRM.checked('RadCategory'), seen = {}, rows = [];
        L.items.forEach(function (r) {
            var name = cat ? HRM.str(r.ItemCategory) : HRM.str(r.ServicesMasterItem);
            if (!name || seen[name]) return;
            seen[name] = 1;
            rows.push({ Id: cat ? HRM.int(r.ItemCategoryId) : HRM.int(r.ServicesMasterItemId), Description: name });
        });
        fillNoDefault('CmbMasterItem', rows, 'Id', 'Description');
    }
    /** ItemDtsFillFromGlobal(CategoryOrMasterItemId). */
    function ItemDtsFillFromGlobal(id) {
        id = HRM.int(id);
        var cat = HRM.checked('RadCategory'), mas = HRM.checked('RadMasterItem');
        dtItem = L.items.filter(function (r) {
            return id === 0 || (cat && id === HRM.int(r.ItemCategoryId)) || (mas && id === HRM.int(r.ServicesMasterItemId));
        });
    }
    /** ItemNameBind(): value Id, display ItemName or ItemCode by the Name / Code radio; default row. */
    function ItemNameBind() {
        var byName = HRM.checked('RadItemNamePackListDetail');
        fill('CmbItemName', dtItem, 'Id', byName ? 'ItemName' : 'ItemCode', '', { alt: byName ? 'ItemCode' : 'ItemName', master: 'ServicesMasterItem', cat: 'ItemCategory' });
    }
    function itemRow(id) { for (var i = 0; i < L.items.length; i++) if (HRM.int(L.items[i].Id) === HRM.int(id)) return L.items[i]; return null; }

    /** cmbitem_Leave -> ItemUomFromGlobalBind(ItemId): the previous UOM text is kept when the new list has it. */
    var uomSeq = 0;
    function cmbitem_Leave() {
        var seq = ++uomSeq, prevText = HRM.comboText('CmbUomDetail');
        return HRM.get(API + '/uoms', { itemId: HRM.comboVal('CmbItemName') }).then(function (rows) {
            if (seq !== uomSeq) return;
            fill('CmbUomDetail', rows || [], 'Id', 'UOMCode', '');
            if (prevText) HRM.setComboText('CmbUomDetail', prevText);
            totalamount();
        }).catch(HRM.fail);
    }

    // ------------------------------------------------------------------------ calculations
    /** totalamount(): Fcy = rate x qty, Lcy = ex x Fcy, Total = Lcy + other ("#,##0.####"). */
    function totalamount() {
        var rate = HRM.num(HRM.val('txtChargesRateFcyDetail')), ex = HRM.num(HRM.val('txtExchangeRateDetail'));
        var qty = HRM.num(HRM.val('txtQtyDetail')), other = HRM.num(HRM.val('txtOtherChargesFcyDetail'));
        var fcy = rate * qty, lcy = ex * fcy, tot = lcy + other;
        HRM.setVal('txtFcyAmountDetail', fmtHash(fcy, 4));
        HRM.setVal('txtLcyAmountDetail', fmtHash(lcy, 4));
        HRM.setVal('txtTotalLcyAmountDetail', fmtHash(tot, 4));
    }
    /** DueDaysCalculate(): due date = doc date + due days (doc date when not > 0). */
    function DueDaysCalculate() {
        var d = HRM.int(HRM.val('txtduedays').trim()), doc = HRM.val('txtdocdate') || HRM.today();
        HRM.setVal('txtduedate', d > 0 ? HRM.addDays(doc, d) : doc);
    }

    // ------------------------------------------------------------------------ invoice information
    function clearInvoice() {
        ['txtCustomerNameInvoiceData', 'txtForwarderNameInvoiceData', 'txtBlNoInvoiceData', 'txtGdNoInvoiceData', 'txtVesselNameInvoiceData',
            'txtIncoTermInvoiceData', 'txtNoOfContainerInvoiceData', 'txtMtonInvoiceData', 'txtBookingRateInvoiceData'].forEach(function (i) { HRM.setVal(i, ''); });
        fill('CmbContractNoInvoiceData', [], 'LcOrderNoId', 'LcOrderNo', false);
        ['CmbLoadingPortInvoiceData', 'CmbDestinationPortInvoiceData', 'CmbContainerDispatchedAtPortInvoiceData',
            'CmbContainerReceivedFromPortInvoiceData', 'CmbDeliveredAtInvoiceData'].forEach(function (c) { HRM.setCombo(c, 0); });
    }
    /** BindInvoiceData(dt). */
    function BindInvoiceData(d) {
        if (!d || !d.found) { clearInvoice(); return; }
        HRM.setVal('txtCustomerNameInvoiceData', d.CustomerName);
        HRM.setVal('txtForwarderNameInvoiceData', d.ForwarderName);
        fill('CmbContractNoInvoiceData', d.contracts || [], 'LcOrderNoId', 'LcOrderNo', false);     // BindDDLNew + Rows[0].Activate()
        var c = HRM.$('CmbContractNoInvoiceData'); if (c.options.length) { c.selectedIndex = 0; HRM.refreshCombos(); }
        HRM.setVal('txtBlNoInvoiceData', d.BLNumber);
        HRM.setVal('txtGdNoInvoiceData', d.GdNo);
        HRM.setCombo('CmbLoadingPortInvoiceData', d.LoadingPortId);
        HRM.setCombo('CmbDestinationPortInvoiceData', d.DestinationPortId);
        HRM.setCombo('CmbContainerDispatchedAtPortInvoiceData', d.ContainerDispatchedAtPortId);
        HRM.setCombo('CmbContainerReceivedFromPortInvoiceData', d.ContainerReceivedFromPortId);
        HRM.setCombo('CmbDeliveredAtInvoiceData', d.DeliveredAtId);
        HRM.setVal('txtVesselNameInvoiceData', d.VesselName);
        HRM.setVal('txtIncoTermInvoiceData', d.DeliveryTerm);
        HRM.setVal('txtNoOfContainerInvoiceData', d.NoOfContainers);
        HRM.setVal('txtMtonInvoiceData', d.MTon);
        HRM.setVal('txtBookingRateInvoiceData', d.BookingRate);
        if (HRM.int(d.CreditAccountId) > 0) HRM.setCombo('CmbDebitAc', d.CreditAccountId);
    }
    /** cmbeximinvoiceno_Leave: invoice data, then the party list re-bound (selection kept). */
    var invSeq = 0;
    function cmbeximinvoiceno_Leave(preloaded) {
        var seq = ++invSeq, id = HRM.comboVal('CmbInvoiceNo');
        var p = preloaded ? Promise.resolve(preloaded) : (id > 0 ? HRM.get(API + '/invoice', { id: id }) : Promise.resolve({ found: false }));
        return p.then(function (d) {
            if (seq !== invSeq) return;
            BindInvoiceData(d);
            BindSupplierName();
        }).catch(HRM.fail);
    }

    // ------------------------------------------------------------------------ detail grid
    var gridColumns = function (withDelete) {
        var cols = [];
        if (withDelete) cols.push({ key: '__del', caption: '', width: 20, render: function () { return '<button type="button" class="win-btn-action hrm-cell-btn" data-act="del">X</button>'; } });
        cols.push(
            { key: 'Id', caption: 'Id', hidden: true },
            { key: 'ItemId', caption: 'ItemId', hidden: true },
            { key: 'ItemCode', caption: 'ItemCode' },
            { key: 'ItemName', caption: 'ItemName', width: 180 },
            { key: 'Qty', caption: 'Qty', type: 'num', render: function (v) { return HRM.esc(fmtHash(v, 2)); }, align: 'right' },
            { key: 'UomId', caption: 'UomId', hidden: true },
            { key: 'Uom', caption: 'Uom' },
            { key: 'ChargesRate', caption: 'ChargesRate', align: 'right', render: function (v) { return HRM.esc(fmtHash(v, 4)); } },
            { key: 'CurrencyId', caption: 'CurrencyId', hidden: true },
            { key: 'Currency', caption: 'Currency' },
            { key: 'FcyAmount', caption: 'FcyAmount', align: 'right', sum: true, render: function (v) { return HRM.esc(fmtHash(v, 4)); } },
            { key: 'ExchangeRate', caption: 'ExchangeRate', align: 'right', render: function (v) { return HRM.esc(fmtHash(v, 4)); } },
            { key: 'Amount', caption: 'Amount', align: 'right', sum: true, render: function (v) { return HRM.esc(fmtHash(v, 4)); } },
            { key: 'OtherChargesAmount', caption: 'OtherChargesAmount', align: 'right', sum: true, render: function (v) { return HRM.esc(fmtHash(v, 4)); } },
            { key: 'TotalLcyAmount', caption: 'TotalLcyAmount', align: 'right', sum: true, render: function (v) { return HRM.esc(fmtHash(v, 4)); } },
            { key: 'Description', caption: 'Description', width: 200 },
            { key: 'DebitAcId', caption: 'DebitAcId', hidden: true },
            { key: 'DebitAc', caption: 'DebitAc', hidden: !cfg.DebitAccountConfig });
        return cols;
    };
    var grddetailadd = null, gridDetailHistory = null;
    function buildDetailGrids() {                                                       // gridsetting(grid)
        grddetailadd = new HRM.Grid('grddetailadd', { columns: gridColumns(true), totals: true, onDouble: function (r, i) { grddetailadd_DoubleClick(i); } });
        gridDetailHistory = new HRM.Grid('gridDetailHistory', { columns: gridColumns(false), totals: true });
    }
    HRM.$('grddetailadd').addEventListener('click', function (e) {                   // grddetailadd_ColumnButtonClick("Delete")
        var b = e.target.closest('button[data-act="del"]'); if (!b) return;
        var tr = b.closest('tr[data-i]'); if (!tr) return;
        DeleteDetailRow(+tr.getAttribute('data-i'));
    });

    function FormValidationDetail() {
        var rules = [
            [!HRM.comboVal('CmbItemName'), 'Item Name field is required', 'CmbItemName'],
            [HRM.num(HRM.val('txtQtyDetail')) === 0, 'Quantity must be a non-zero number', 'txtQtyDetail'],
            [!HRM.comboVal('CmbUomDetail'), 'Rate UOM field is required', 'CmbUomDetail'],
            [HRM.num(HRM.val('txtChargesRateFcyDetail')) === 0, 'Charges Rate must be a non-zero number', 'txtChargesRateFcyDetail'],
            [!HRM.comboVal('CmbFcyCodeDetail'), 'Currency field is required', 'CmbFcyCodeDetail'],
            [HRM.num(HRM.val('txtFcyAmountDetail')) === 0, 'Fcy Amount must be a non-zero number', 'txtFcyAmountDetail'],
            [HRM.num(HRM.val('txtLcyAmountDetail')) === 0, 'Lcy Amount must be a non-zero number', 'txtLcyAmountDetail'],
            [HRM.num(HRM.val('txtTotalLcyAmountDetail')) === 0, 'Total Lcy Amount must be a non-zero number', 'txtTotalLcyAmountDetail'],
            [!HRM.val('txtdescription').trim(), 'Description field is required', 'txtdescription']
        ];
        if (cfg.DebitAccountConfig) rules.push([!HRM.comboVal('CmbDebitAc'), 'Debit Account field is required', 'CmbDebitAc']);
        for (var i = 0; i < rules.length; i++) if (rules[i][0]) { HRM.box(rules[i][1]); HRM.focus(rules[i][2]); return false; }
        return true;
    }
    /** The row btnplus_Click / btnUpdateDetail_Click write from the entry boxes. */
    function rowFromEntry(id) {
        var it = itemRow(HRM.comboVal('CmbItemName')) || {};
        return {
            Id: id, ItemId: HRM.comboVal('CmbItemName'), ItemCode: HRM.str(it.ItemCode), ItemName: HRM.str(it.ItemName),
            Qty: HRM.num(HRM.val('txtQtyDetail')), UomId: HRM.comboVal('CmbUomDetail'), Uom: HRM.comboText('CmbUomDetail'),
            ChargesRate: HRM.num(HRM.val('txtChargesRateFcyDetail')), CurrencyId: HRM.comboVal('CmbFcyCodeDetail'), Currency: HRM.comboText('CmbFcyCodeDetail'),
            FcyAmount: HRM.num(HRM.val('txtFcyAmountDetail')), ExchangeRate: HRM.num(HRM.val('txtExchangeRateDetail')),
            Amount: HRM.num(HRM.val('txtLcyAmountDetail')), OtherChargesAmount: HRM.num(HRM.val('txtOtherChargesFcyDetail')),
            TotalLcyAmount: HRM.num(HRM.val('txtTotalLcyAmountDetail')), Description: HRM.val('txtdescription'),
            DebitAcId: HRM.comboVal('CmbDebitAc'), DebitAc: HRM.comboText('CmbDebitAc')
        };
    }
    /** resetdetail(). */
    function resetdetail() {
        HRM.setCombo('CmbItemName', 0);
        HRM.setVal('txtQtyDetail', '');
        fill('CmbUomDetail', [], 'Id', 'UOMCode', '');
        HRM.setVal('txtChargesRateFcyDetail', '');
        HRM.setCombo('CmbFcyCodeDetail', 0);
        ['txtFcyAmountDetail', 'txtExchangeRateDetail', 'txtLcyAmountDetail', 'txtOtherChargesFcyDetail', 'txtTotalLcyAmountDetail', 'txtdescription']
            .forEach(function (i) { HRM.setVal(i, ''); });
        HRM.setCombo('CmbDebitAc', 0);
        HRM.show('btnplus', true); HRM.show('btnUpdateDetail', false); HRM.show('btnCancelUpdateDetial', false);
        updateDetailIndex = -1;
        HRM.focus('CmbItemName');
    }
    /** grddetailadd_DoubleClick: the row back into the entry boxes. */
    function grddetailadd_DoubleClick(i) {
        var r = grddetailadd.rows()[i]; if (!r) return;
        updateDetailIndex = i;
        HRM.setCombo('CmbMasterItem', 0);
        ItemDtsFillFromGlobal(0);
        ItemNameBind();
        HRM.setCombo('CmbItemName', r.ItemId);
        cmbitem_Leave().then(function () {
            HRM.setCombo('CmbUomDetail', r.UomId);
            totalamount();
        });
        HRM.setVal('txtQtyDetail', fmtHash(r.Qty, 2));
        HRM.setVal('txtChargesRateFcyDetail', fmtHash(r.ChargesRate, 4));
        HRM.setCombo('CmbFcyCodeDetail', r.CurrencyId);
        HRM.setVal('txtFcyAmountDetail', fmtHash(r.FcyAmount, 4));
        HRM.setVal('txtExchangeRateDetail', fmtHash(r.ExchangeRate, 4));
        HRM.setVal('txtLcyAmountDetail', fmtHash(r.Amount, 4));
        HRM.setVal('txtOtherChargesFcyDetail', fmtHash(r.OtherChargesAmount, 4));
        HRM.setVal('txtTotalLcyAmountDetail', fmtHash(r.TotalLcyAmount, 4));
        HRM.setVal('txtdescription', HRM.str(r.Description));
        HRM.setCombo('CmbDebitAc', r.DebitAcId);
        HRM.show('btnplus', false); HRM.show('btnUpdateDetail', true); HRM.show('btnCancelUpdateDetial', true);
        HRM.focus('CmbItemName');
    }
    /** DeleteDetailRow(r). */
    function DeleteDetailRow(i) {
        var r = grddetailadd.rows()[i]; if (!r) return;
        if (updateDetailIndex !== -1) { HRM.box('Reset Detail First'); return; }
        if (HRM.int(r.Id) > 0 && !HRM.ask('Are you sure to Delete?')) return;
        grddetailadd.remove(i);
    }

    // ------------------------------------------------------------------------ form
    function buttons(mode) {                                                       // reset() / ReadById()
        HRM.show('btnSave', mode === 'new'); HRM.show('btnUpdate', mode === 'edit'); HRM.show('btnDelete', mode === 'edit');
    }
    function GetDocumentCode() {
        return HRM.get(API + '/doc-no').then(function (d) { HRM.setVal('txtdocno', HRM.str(d && d.docNo)); }).catch(HRM.fail);
    }
    /** reset(). */
    function reset() {
        RecId = 0; VoucherHeadId = 0;
        buttons('new');
        HRM.setVal('txtdocno', '');
        HRM.setVal('txtrefbillno', '');
        HRM.setVal('txtduedays', '');
        HRM.setVal('txtdocdate', HRM.today());
        HRM.setVal('txtduedate', HRM.today());
        HRM.setCombo('CmbPartyName', 0);
        HRM.setCombo('CmbInvoiceNo', 0);
        var inv = cmbeximinvoiceno_Leave({ found: false });
        resetdetail();
        grddetailadd.clear();
        HRM.focus('txtdocdate');
        return Promise.all([inv, GetDocumentCode()]);
    }

    function validateHeader() {
        if (!grddetailadd.rows().length) { HRM.box('Detail Record Not Found'); return false; }
        var rules = [
            [!HRM.val('txtdocno').trim(), 'Doc No field is required', 'txtdocno'],
            [!HRM.comboVal('CmbInvoiceNo'), 'Invoice No field is required', 'CmbInvoiceNo'],
            [!HRM.comboVal('CmbPartyName'), 'Party Name field is required', 'CmbPartyName'],
            [!HRM.comboVal('CmbContractNoInvoiceData'), 'Contract No field is required', 'CmbContractNoInvoiceData'],
            [!HRM.comboVal('CmbLoadingPortInvoiceData'), 'Loading Place field is required', 'CmbLoadingPortInvoiceData'],
            [!HRM.comboVal('CmbDestinationPortInvoiceData'), 'Destination Place field is required', 'CmbDestinationPortInvoiceData'],
            [HRM.num(HRM.val('txtNoOfContainerInvoiceData')) === 0, 'No. of Containers must be a non-zero number', 'txtNoOfContainerInvoiceData'],
            [HRM.num(HRM.val('txtMtonInvoiceData')) === 0, 'M.Tons must be a non-zero number', 'txtMtonInvoiceData']
        ];
        for (var i = 0; i < rules.length; i++) if (rules[i][0]) { HRM.box(rules[i][1]); HRM.focus(rules[i][2]); return false; }
        return true;
    }

    /** Insert(). */
    function Insert(btn) {
        if (!validateHeader()) return;
        if (!HRM.ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
        var upd = RecId > 0, preview = HRM.checked('ChkPreview') && rights.print, voucher = HRM.checked('chkVoucher') && rights.print;
        return HRM.busy(btn, function () {
            return HRM.post(API + '/save', {
                id: RecId, docDate: HRM.val('txtdocdate'), dueDays: HRM.val('txtduedays'), refBillNo: HRM.val('txtrefbillno'),
                invoiceId: HRM.comboVal('CmbInvoiceNo'), partyId: HRM.comboVal('CmbPartyName'), rows: grddetailadd.rows()
            }).then(function (d) {
                HRM.box(d && d.message ? d.message : (upd ? 'Update Successfully' : 'Save Successfully'));
                var id = HRM.int(d && d.id);
                return reset().then(function () {
                    if (preview) Slip420(id, null);
                    if (voucher) voucher118(id, null);
                });
            }).catch(function (e) { HRM.fail(e); });
        }, 'sb-save');
    }

    /** ReadById(ID). */
    function ReadById(id) {
        return HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (h) {
            return reset().then(function () {
                RecId = HRM.int(h.Id);
                showTab('tabForm');
                buttons('edit');
                HRM.setVal('txtdocno', HRM.str(h.DocNo));
                HRM.setVal('txtdocdate', HRM.day(h.DocDate));
                HRM.setVal('txtduedays', HRM.str(h.DueDays));
                HRM.setVal('txtduedate', HRM.day(h.DueDate));
                HRM.setVal('txtrefbillno', HRM.str(h.RefBillNo));
                HRM.setCombo('CmbInvoiceNo', h.ExportInvoiceId);
                return cmbeximinvoiceno_Leave(h.invoice).then(function () {
                    HRM.setCombo('CmbPartyName', h.SupCustId);
                    grddetailadd.set(h.rows || []);
                    HRM.focus('txtdocdate');
                });
            });
        }).catch(HRM.fail);
    }

    // ------------------------------------------------------------------------ prints
    /** Slip420(ReportId) -> CommonServices.ServicesBillSlip540. */
    function Slip420(id, btn) {
        var win = window.CrystalPrint ? window.CrystalPrint.reserve() : null;
        return HRM.busy(btn, function () {
            return HRM.get(API + '/slip-check', { id: id }).then(function () {
                if (!window.CrystalPrint) { HRM.box('Report 540 is not available.'); return; }
                return window.CrystalPrint.open('lgsa-540', { id: id }, null, win);
            }).catch(function (e) { if (win) window.CrystalPrint.release(win); HRM.fail(e); });
        }, 'sb-540');
    }
    /** CommonServices.VoucherReport_118(VoucherHeadIdGet(id, 155), 155). */
    function voucher118(id, btn) {
        var win = window.CrystalPrint ? window.CrystalPrint.reserve() : null;
        return HRM.busy(btn, function () {
            return HRM.get(API + '/voucher-id', { id: id }).then(function (d) {
                if (!window.CrystalPrint) { HRM.box('Report 118 is not available.'); return; }
                return window.CrystalPrint.open('acc-118', { id: HRM.int(d.voucherHeadId), documentTypeId: DOC_TYPE }, null, win);
            }).catch(function (e) { if (win) window.CrystalPrint.release(win); HRM.fail(e); });
        }, 'sb-118');
    }

    // ------------------------------------------------------------------------ history
    function btnCell(act, text) { return '<button type="button" class="win-btn-action hrm-cell-btn" data-act="' + act + '">' + text + '</button>'; }
    function stamp(v) { return HRM.esc(HRM.fmtDateTime(v)); }
    var grdhistory = null;
    function buildHistoryGrid() {                                                 // GridHistorySetting
        var cols = [];
        if (rights.print) cols.push({ key: '__slip', caption: 'Slip', width: 50, render: function () { return btnCell('slip', 'Slip'); } });
        if (rights.update) cols.push({ key: '__voucher', caption: 'Voucher', width: 70, render: function () { return btnCell('voucher', 'Voucher'); } });
        cols.push(
            { key: 'Id', caption: 'Id', hidden: true }, { key: 'VoucherHeadId', caption: 'VoucherHeadId', hidden: true },
            { key: 'DocNo', caption: 'DocNo', type: 'int' }, { key: 'DocDate', caption: 'DocDate', type: 'date' },
            { key: 'PartyName', caption: 'PartyName', width: 180 }, { key: 'InvoiceNo', caption: 'InvoiceNo' },
            { key: 'ContractNo', caption: 'ContractNo' }, { key: 'LoadingPort', caption: 'LoadingPort' }, { key: 'DestinationPort', caption: 'DestinationPort' },
            { key: 'NoOfContainer', caption: 'NoOfContainer', type: 'num', decimals: 0, sum: true },
            { key: 'NetBillWeight', caption: 'NetBillWeight', type: 'num', decimals: 3, sum: true },
            { key: 'TotalAmount', caption: 'TotalAmount', type: 'num', decimals: 2, sum: true },
            { key: 'EntryUser', caption: 'EntryUser' }, { key: 'EntryDate', caption: 'EntryDate', render: stamp },
            { key: 'ModifyUser', caption: 'ModifyUser' }, { key: 'ModifyDate', caption: 'ModifyDate', render: stamp },
            { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'code' },
            { key: '__att', caption: 'Add Attachment', width: 110, render: function () { return btnCell('att', 'Add Attachment'); } });
        grdhistory = new HRM.Grid('grdhistory', {
            columns: cols, filterRow: true, totals: true,
            onDouble: function (r) { VoucherHeadId = HRM.int(r.VoucherHeadId); ReadById(HRM.int(r.Id)); },     // grdhistory_DoubleClick
            onCode: function () { HRM.box('Attachments (DMS) are not ported on the web yet.'); },
            onSelect: function (r) { GetDetailGrdByHeadId(HRM.int(r.Id)); }                                     // grdhistory_SelectionChanged
        });
    }
    HRM.$('grdhistory').addEventListener('click', function (e) {                // grdhistory_ColumnButtonClick
        var b = e.target.closest('button[data-act]'); if (!b) return;
        var tr = b.closest('tr[data-i]'); if (!tr || !grdhistory) return;
        var r = grdhistory.rows()[+tr.getAttribute('data-i')]; if (!r) return;
        var act = b.getAttribute('data-act');
        if (act === 'slip') Slip420(HRM.int(r.Id), b);
        else if (act === 'voucher') voucher118(HRM.int(r.Id), b);
        else if (act === 'att') HRM.box('Attachments (DMS) are not ported on the web yet.');
    });
    var detSeq = 0;
    function GetDetailGrdByHeadId(id) {
        var seq = ++detSeq;
        return HRM.get(API + '/history-detail', { id: id }).then(function (rows) { if (seq === detSeq) gridDetailHistory.set(rows || []); }).catch(HRM.fail);
    }
    /** HistoryGridFill(). */
    function HistoryGridFill(btn) {
        var kind = HRM.checked('rdentrydate') ? 'entry' : HRM.checked('rdmodifydate') ? 'modify' : 'doc';
        return HRM.busy(btn || 'btnShowHistory', function () {
            return HRM.get(API + '/history', {
                dateKind: kind,
                fromDate: HRM.checked('chkFromDateHistory') ? HRM.val('FromDateHistory') : null,
                toDate: HRM.checked('chkToDateHistory') ? HRM.val('ToDateHistory') : null,
                fromDocNo: HRM.val('FromDocNoHistory'), toDocNo: HRM.val('ToDocNoHistory')
            }).then(function (rows) { grdhistory.set(rows || []); gridDetailHistory.clear(); }).catch(HRM.fail);
        });
    }

    // ------------------------------------------------------------------------ tabs
    function showTab(id) {
        document.querySelectorAll('.hrm-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
        document.querySelectorAll('.hrm-tab-page').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
        if (id === 'tabHistory') HRM.focus('FromDateHistory');                   // tabControl1_SelectedIndexChanged
    }
    document.querySelectorAll('.hrm-tab').forEach(function (t) { t.addEventListener('click', function () { showTab(t.getAttribute('data-tab')); }); });
    function historyTab() { return HRM.$('tabHistory').classList.contains('is-active'); }

    // ------------------------------------------------------------------------ handlers
    P.btnSave = function (btn) { RecId = 0; return Insert(btn || 'btnSave'); };                          // btnSave_Click
    P.btnUpdate = function (btn) {                                                                      // btnUpdate_Click
        if (RecId === 0) { HRM.box('Record not update because RecId not found'); return; }
        return Insert(btn || 'btnUpdate');
    };
    P.btnNew = function () { return reset(); };
    P.btnDelete = function (btn) {                                                                      // btnDelete_Click
        if (RecId <= 0) { HRM.box('No record found to Delete'); return; }
        if (!HRM.ask('Are you sure to Delete?')) return;
        return HRM.busy(btn || 'btnDelete', function () {
            return HRM.post(API + '/delete?id=' + RecId, {}).then(function (d) {
                HRM.box(d && d.message ? d.message : 'Delete Record Successfully');
                return reset();
            }).catch(HRM.fail);
        });
    };
    P.btnRefresh = function (btn) {                                                                     // btnRefresh_Click
        return HRM.busy(btn || 'btnRefresh', function () {
            return HRM.get(API + '/refresh').then(function (d) {
                applyLists(d);
                return cmbeximinvoiceno_Leave();
            }).catch(HRM.fail);
        });
    };
    P.btnprint = function (btn) { return Slip420(RecId, btn || 'btnprint'); };                          // btnprint_Click
    P.btnVoucher = function (btn) {                                                                     // btnVoucher_Click
        if (RecId <= 0) { HRM.box('Record Not Found For Display'); return; }
        return voucher118(RecId, btn || 'btnVoucher');
    };
    P.btnplus = function () {                                                                           // btnplus_Click
        if (!FormValidationDetail()) return;
        grddetailadd.add(rowFromEntry(0));
        resetdetail();
    };
    P.btnUpdateDetail = function () {                                                                   // btnUpdateDetail_Click
        if (!FormValidationDetail()) return;
        var old = grddetailadd.rows()[updateDetailIndex]; if (!old) return;
        grddetailadd.update(updateDetailIndex, rowFromEntry(old.Id));
        resetdetail();
    };
    P.btnCancelUpdateDetial = function () { resetdetail(); };
    P.btnResetHistory = function () {                                                                   // btnResetHistory_Click
        HRM.setVal('FromDateHistory', HRM.addDays(HRM.today(), -3));
        HRM.setVal('ToDateHistory', HRM.today());
        HRM.setVal('FromDocNoHistory', ''); HRM.setVal('ToDocNoHistory', '');
        grdhistory.clear(); gridDetailHistory.clear();
        HRM.focus('FromDateHistory');
    };
    P.btnShowHistory = function (btn) { return HistoryGridFill(btn); };
    P.attachment = function () { HRM.box('Attachments (DMS) are not ported on the web yet.'); };
    P.defineServices = function () { HRM.open('/logistics/define-services-item'); };                   // btnDefineServices_Click
    P.shortcuts = function () {                                                                         // MakeShortCutKeys
        HRM.box(['Ctrl+S  For Save', 'Ctrl+U  For Update', 'Ctrl+E  For Close', 'Ctrl+R  For Refresh', 'Ctrl+N  For New', 'Ctrl+T  For Tab Transfer',
            'Ctrl+F5  For Focus on Doc Date', 'Ctrl+F10  For Open Attachments', 'Ctrl+alt  To Show ShortCut Keys Form',
            'Ctrl+L  When In History Tab For Load All Records', 'Ctrl+ArrowUp  For Focus On Item', 'Ctrl+ArrowDown  For Focus On Detail Grid',
            'Ctrl+Enter  When Focus On Any Grid For Update Record', 'Ctrl+Space  When Focus On Any Grid To Call Function\'s On Button Or Link'].join('\n'));
    };

    // ------------------------------------------------------------------------ events
    function onLeave(id, fn) {                                                     // UltraCombo.Leave: a pick, or focus leaving the wrap
        HRM.$(id).addEventListener('change', fn);
    }
    onLeave('CmbInvoiceNo', function () { cmbeximinvoiceno_Leave(); });
    onLeave('CmbItemName', function () { cmbitem_Leave(); });
    onLeave('CmbMasterItem', function () { ItemDtsFillFromGlobal(HRM.comboVal('CmbMasterItem')); ItemNameBind(); });      // CmbMasterItem_Leave
    ['RadMasterItem', 'RadCategory'].forEach(function (id) {                     // RadMasterItem_CheckedChanged
        HRM.$(id).addEventListener('change', function () { ItemCategoryOrMasterItemBind(); ItemDtsFillFromGlobal(HRM.comboVal('CmbMasterItem')); ItemNameBind(); });
    });
    ['RadItemNamePackListDetail', 'RadItemCodePackListDetail'].forEach(function (id) {   // RadItemNamePackListDetail_CheckedChanged
        HRM.$(id).addEventListener('change', ItemNameBind);
    });
    ['txtChargesRateFcyDetail', 'txtExchangeRateDetail', 'txtQtyDetail', 'txtOtherChargesFcyDetail'].forEach(function (id) {
        HRM.$(id).addEventListener('input', totalamount);                          // *_TextChanged -> totalamount()
    });
    HRM.$('CmbUomDetail').addEventListener('change', totalamount);               // cmbuom_TextChanged
    HRM.$('txtduedays').addEventListener('input', DueDaysCalculate);             // txtduedays_TextChanged
    HRM.$('txtdocdate').addEventListener('change', DueDaysCalculate);            // txtdocdate_ValueChanged

    HRM.keys({                                                                    // ExImClearingAgentBillDirect_KeyDown
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+t': function () { if (historyTab()) { showTab('tabForm'); HRM.focus('txtdocdate'); } else { showTab('tabHistory'); } },
        'ctrl+alt+control': P.shortcuts, 'ctrl+alt+alt': P.shortcuts,
        'ctrl+s': function () {
            if (historyTab()) { P.btnShowHistory(); return; }
            var b = HRM.$('btnSave'); if (HRM.visible('btnSave') && !b.disabled) P.btnSave();
        },
        'ctrl+u': function () { var b = HRM.$('btnUpdate'); if (!historyTab() && HRM.visible('btnUpdate') && !b.disabled) P.btnUpdate(); },
        'ctrl+delete': function () { var b = HRM.$('btnDelete'); if (!historyTab() && HRM.visible('btnDelete') && !b.disabled) P.btnDelete(); },
        'ctrl+n': function () { if (historyTab()) P.btnResetHistory(); else P.btnNew(); },
        'ctrl+p': function () { if (!historyTab() && rights.print) P.btnprint(); },
        'ctrl+r': function () { if (!historyTab()) P.btnRefresh(); },
        'ctrl+f10': function () { if (!historyTab()) P.attachment(); },
        'ctrl+f5': function () { HRM.focus(historyTab() ? 'FromDateHistory' : 'txtdocdate'); },
        'ctrl+arrowup': function () { if (historyTab()) HRM.focus('FromDateHistory'); },
        'ctrl+arrowdown': function () {
            if (!historyTab()) return;
            var inGrid = document.activeElement && document.activeElement.closest('#grdhistory');
            var tr = document.querySelector((inGrid ? '#gridDetailHistory' : '#grdhistory') + ' tbody tr[data-i]'); if (tr) tr.focus();
        },
        'ctrl+enter': function () {
            var a = document.activeElement;
            if (a && a.closest('#grddetailadd') && grddetailadd.currentIndex() >= 0) { grddetailadd_DoubleClick(grddetailadd.currentIndex()); return; }
            if (a && a.closest('#grdhistory') && grdhistory.current()) {                                  // grdhistory_KeyDown Ctrl+Enter
                if (!rights.update) { HRM.box("You don't have the right to update."); return; }
                var r = grdhistory.current(); VoucherHeadId = HRM.int(r.VoucherHeadId); ReadById(HRM.int(r.Id));
            }
        }
    });
    HRM.footer(function () { showTab('tabHistory'); });

    // ------------------------------------------------------------------------ load
    function applyLists(d) {
        cfg = d.config || cfg;
        L.items = d.items || []; L.parties = d.parties || []; L.ports = d.ports || []; L.currencies = d.currencies || [];
        L.accounts = d.accounts || []; L.invoices = d.invoices || []; L.deliveredAt = d.deliveredAt || [];
        InvoiceNoBind();
        PortsBind();
        BindSupplierName();
        DeliveredAtBind();
        ItemCategoryOrMasterItemBind();
        ItemDtsFillFromGlobal(0);
        ItemNameBind();
        CurrencyBindFromGlobal();
        if (cfg.DebitAccountConfig) { HRM.show('boxDebitAc', true); AccountlstDtFillFromGlobal(); }        // lbldebitac / CmbDebitAc visible
    }

    HRM.setVal('txtdocdate', HRM.today());
    HRM.setVal('txtduedate', HRM.today());
    HRM.loading(HRM.get(API + '/setup')).then(function (d) {                    // InitializeComponentMethod
        rights = d.rights || {};
        cfg = d.config || {};
        buildDetailGrids();
        buildHistoryGrid();
        HRM.applyRights(rights, { save: 'btnSave', 'delete': 'btnDelete', update: 'btnUpdate', print: ['btnprint', 'btnVoucher'] });
        HRM.check('RadItemNamePackListDetail', true);                                                   // both config branches check "Name"
        HRM.show('boxDebitAc', false);
        HRM.setVal('txtdocno', HRM.str(d.docNo));
        applyLists(d);
        var days = HRM.int(cfg.DefaultDaysToLessFromHistoryFromDate);
        HRM.setVal('FromDateHistory', HRM.addDays(HRM.today(), days > 0 ? -days : -3));
        HRM.setVal('ToDateHistory', HRM.today());
        buttons('new');
        HRM.focus('txtdocdate');
        var id = HRM.int(HRM.param('id'));
        if (id > 0) ReadById(id);
    }).catch(HRM.fail);
})();
