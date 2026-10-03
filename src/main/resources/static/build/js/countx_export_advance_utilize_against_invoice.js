/* ============================================================================================
 * countx_export_advance_utilize_against_invoice.js - FIOrAdvanceUtilizedAgainstInvoice.cs (Architecture.WinApp.Export),
 * screen 915 "Advance Utilize Against Invoice" ("Financial Instruments Utilized Against Shipment"). Form | History;
 * every button, CmbInvoiceNo_Leave, dcmbfino_ValueChanged (payment term + FI balance), % <-> Fcy Amount cross
 * calculation with the desktop's warnings, the payment-detail + / Update / Cancel / double-click flow (duplicate FI
 * check, Payment Term locked when Received > 0), the FcyAmount cell edit (Received / Balance / % recalculated),
 * Save / Update, history Show / New / Refresh, Edit / double-click and the KeyDown shortcuts; data from
 * /api/export/advance-utilize-against-invoice. No print on this form.
 * ============================================================================================ */
(function (global) {
    'use strict';

    var F = global.ExportF, $id = F.$id, box = F.box, ask = F.ask, str = F.str, netI = F.netI, netD = F.netD, val = F.val, setText = F.setText,
        setVal = F.setVal, bind = F.bind, busy = F.busy, getJson = F.getJson, postJson = F.postJson, show = F.show, focus = F.focus;
    var API = '/api/export/advance-utilize-against-invoice';

    var PERM = { Save: true, Update: true };
    var INVOICES = [], TERMS = [], FIS = [], HIST_INV = [], HIST_FI = [];
    var S = { recId: 0, updateMode: false, updateIndex: -1 };
    var DET = { rows: [], cur: -1 };
    var HIST = { rows: [], cur: -1 };

    function bindInvoices() { bind('CmbInvoiceNo', INVOICES, 'Id', 'InvoiceNo', ['BankAmount']); }
    function bindTerms() { bind('dcmbpaymentterm', TERMS, 'Id', 'Name', []); }
    function bindFis() { bind('CmbFinInstruments', FIS, 'Id', 'EFormNo', []); }

    /** ImProformaInvoice_Load. */
    function load() {
        return getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false };
            $id('btnsave').disabled = !PERM.Save; $id('btnupdate').disabled = !PERM.Update;
            ['invoices', 'paymentTerms', 'historyInvoices', 'historyFis'].forEach(function (k) { if (d[k + 'Error']) box(d[k + 'Error']); });
            INVOICES = d.invoices || []; bindInvoices(); setVal('CmbInvoiceNo', '0');
            TERMS = d.paymentTerms || []; bindTerms();
            HIST_INV = d.historyInvoices || []; bind('CmbInvoiceHistory', HIST_INV, 'Id', 'Name', [], true);
            HIST_FI = d.historyFis || []; bind('CmbFINoHistory', HIST_FI, 'Id', 'FinancialInstrumentNo', [], true);
            detRender();
            $id('fiuFooterInfo').textContent = 'FIOrAdvanceUtilizedAgainstInvoice';
            F.setEnabled('CmbInvoiceNo', true);
            focus('CmbInvoiceNo');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    /** btnrefersh_Click: InvoiceNoFill + PaymentTermsFill. */
    function refresh(btn) {
        return busy(btn, function () {
            return getJson(API + '/refresh?recId=' + S.recId).then(function (d) { INVOICES = d.invoices || []; bindInvoices(); TERMS = d.paymentTerms || []; bindTerms(); }).catch(function (e) { box(e.message); });
        });
    }
    function invoice() { return F.findRow(INVOICES, val('CmbInvoiceNo')); }

    /** CmbInvoiceNo_Leave. */
    function invoiceLeave() {
        var inv = invoice();
        if (inv && netI(inv.Id) !== 0) {
            return getJson(API + '/invoice-leave?invoiceId=' + inv.Id + '&supplierCustomerId=' + netI(inv.SupplierCustomerId)).then(function (d) {
                DET.rows = d.rows || []; DET.cur = -1; detRender();
                FIS = d.fis || []; bindFis();
                setText('txtInvoiceBalanceAmt', F.fmt(inv.BankAmount)); setText('txtInvoiceAmount', F.fmt(inv.TotalAmount));
            }).catch(function (e) { box(e.message); });
        }
        setText('txtInvoiceBalanceAmt', F.fmt(0)); setText('txtInvoiceAmount', F.fmt(0));
        FIS = []; bindFis(); DET.rows = []; detRender();
        return Promise.resolve();
    }
    /** dcmbfino_ValueChanged: payment term from the FI row, then GetFinancialInstrumentsBalance. */
    function fiChanged() {
        var fi = F.findRow(FIS, val('CmbFinInstruments'));
        if (!fi || netI(fi.Id) <= 0) return;
        setVal('dcmbpaymentterm', fi.PaymenttermId);
        getJson(API + '/fi-balance?documentTypeId=' + netI(fi.DocumentTypeId) + '&id=' + netI(fi.Id)).then(function (d) { setText('txtBalanceFI', str(d.balance)); }).catch(function (e) { box(e.message); });
    }
    /** dtxtfcyamount_TextChanged. */
    function fcyChanged() {
        var total = netD(val('txtInvoiceAmount')), amt = netD(val('dtxtfcyamount'));
        if (total > 0 && amt > 0) {
            setText('dtxtpercentoftotal', F.fmt(amt / total * 100));
            if (netD(val('dtxtpercentoftotal')) > 100) { setText('dtxtpercentoftotal', '0'); box('Percent cannot be greater than 100 Please check'); }
        }
    }
    /** dtxtpercentoftotal_TextChanged -> PaymentDetailPercentCalculate. */
    function percentChanged() {
        if (!invoice() || netI(val('CmbInvoiceNo')) <= 0 || netD(val('dtxtpercentoftotal')) <= 0) return;
        if (netD(val('dtxtpercentoftotal')) > 100) { setText('dtxtpercentoftotal', '0'); box('Percent cannot be greater than 100 Please check'); return; }
        var total = netD(val('txtInvoiceAmount')), pct = netD(val('dtxtpercentoftotal'));
        if (total > 0 && pct > 0) setText('dtxtfcyamount', F.fmt(total * pct / 100));
    }

    /** PaymentDetailFormValidation(). */
    function detailValidation() {
        if (!F.hasSel('CmbInvoiceNo')) { box('Please Select Invoice Number First...'); focus('CmbInvoiceNo'); return false; }
        if (netI(val('dcmbpaymentterm')) === 1 && !F.hasSel('CmbFinInstruments')) { box('Financial Instrument No field required'); focus('CmbFinInstruments'); return false; }
        if (!F.hasSel('dcmbpaymentterm')) { box('Payment Term field required'); focus('dcmbpaymentterm'); return false; }
        if (netD(val('dtxtpercentoftotal')) === 0 || val('dtxtpercentoftotal') === '') { box('Percent Field Required'); focus('dtxtpercentoftotal'); return false; }
        if (netD(val('dtxtfcyamount')) === 0) { box('Fcy Amount Field is Required'); focus('dtxtfcyamount'); return false; }
        return true;
    }
    function fiRow() { return F.findRow(FIS, val('CmbFinInstruments')) || {}; }
    function dupFi(skip) {
        var adv = netI(val('dcmbpaymentterm')) === 1, fi = fiRow();
        return DET.rows.some(function (r, i) { return i !== skip && adv && netI(r.ExImEFormRegistrationId) === netI(val('CmbFinInstruments')) && netI(r.DocumentTypeId) === netI(fi.DocumentTypeId); });
    }
    /** btndadd_Click. */
    function addRow() {
        if (!detailValidation()) return;
        if (dupFi(-1)) { box('Financial Instrument No already add in Grid Please Check'); return; }
        var adv = netI(val('dcmbpaymentterm')) === 1, fi = fiRow(), amt = netD(val('dtxtfcyamount'));
        DET.rows.push({ Id: 0, DocumentTypeId: adv ? netI(fi.DocumentTypeId) : 0, ExImEFormRegistrationId: adv ? netI(val('CmbFinInstruments')) : 0,
            FinancialInstrumentNo: adv ? F.selText('CmbFinInstruments') : '', PaymentTermId: netI(val('dcmbpaymentterm')), PaymentTerm: F.selText('dcmbpaymentterm'),
            PrcntOfTotal: netD(val('dtxtpercentoftotal')), FcyAmount: amt, DueDays: netD(val('dtxtdueday')), Received: 0, Balance: amt });
        percentOfTotalCalculate(); detRender(); resetPaymentDetails();
    }
    /** btndupdate_Click. */
    function updateRow() {
        if (!detailValidation()) return;
        if (dupFi(S.updateIndex)) { box('Financial Instrument No already add in Grid Please Check'); return; }
        var r = DET.rows[S.updateIndex]; if (!r) return;
        var adv = netI(val('dcmbpaymentterm')) === 1, fi = fiRow();
        r.DocumentTypeId = adv ? netI(fi.DocumentTypeId) : 0; r.ExImEFormRegistrationId = adv ? netI(val('CmbFinInstruments')) : 0;
        r.FinancialInstrumentNo = adv ? F.selText('CmbFinInstruments') : ''; r.PaymentTermId = netI(val('dcmbpaymentterm')); r.PaymentTerm = F.selText('dcmbpaymentterm');
        r.PrcntOfTotal = netD(val('dtxtpercentoftotal')); r.FcyAmount = netD(val('dtxtfcyamount')); r.DueDays = netI(val('dtxtdueday'));
        r.Balance = netD(r.FcyAmount) - netD(r.Received);
        percentOfTotalCalculate(); detRender(); resetPaymentDetails();
        F.setEnabled('dcmbpaymentterm', true); focus('CmbFinInstruments');
    }
    function cancelRow() { show('btndadd', true); show('btndupdate', false); show('btndcancel', false); F.setEnabled('dcmbpaymentterm', true); resetPaymentDetails(); }
    /** grdpaymentdetail_DoubleClick. */
    function editRow(i) {
        var r = DET.rows[i]; if (!r) return;
        F.setEnabled('dcmbpaymentterm', true);
        S.updateIndex = i;
        F.setValOrAdd('CmbFinInstruments', r.ExImEFormRegistrationId, r.FinancialInstrumentNo);
        setVal('dcmbpaymentterm', r.PaymentTermId);
        setText('dtxtpercentoftotal', F.fmt(r.PrcntOfTotal)); setText('dtxtfcyamount', F.fmt(r.FcyAmount)); setText('dtxtdueday', str(r.DueDays));
        if (netD(r.Received) > 0) F.setEnabled('dcmbpaymentterm', false);
        show('btndadd', false); show('btndupdate', true); show('btndcancel', true);
    }
    /** grdpaymentdetail_CellUpdated (FcyAmount): Received > FcyAmount -> FcyAmount = Received + warning; Balance / % recalculated. */
    function cellUpdated(i, k, v) {
        var r = DET.rows[i]; if (!r || k !== 'FcyAmount') return;
        r.FcyAmount = netD(v);
        var total = netD(val('txtInvoiceAmount'));
        if (netD(r.Received) > r.FcyAmount) { r.FcyAmount = netD(r.Received); detRender(); box('Received Amount cannot be greater than FcyAmount.please check ....'); return; }
        r.Balance = r.FcyAmount - netD(r.Received);
        r.PrcntOfTotal = (r.FcyAmount > 0 && total > 0) ? r.FcyAmount / total * 100 : 0;
        percentOfTotalCalculate(); detRender();
    }
    function percentOfTotalCalculate() {
        var total = netD(val('txtInvoiceAmount'));
        DET.rows.forEach(function (r) { r.PrcntOfTotal = (netD(r.FcyAmount) > 0 && total > 0) ? netD(r.FcyAmount) / total * 100 : 0; });
    }
    /** ResePaymentDetails(). */
    function resetPaymentDetails() {
        S.updateIndex = -1;
        setVal('CmbFinInstruments', '0'); setVal('dcmbpaymentterm', '0');
        setText('dtxtpercentoftotal', '0'); setText('dtxtfcyamount', '0'); setText('dtxtdueday', '0');
        show('btndadd', true); show('btndupdate', false); show('btndcancel', false);
        focus('CmbFinInstruments');
    }
    var DCOLS = [{ key: 'FinancialInstrumentNo' }, { key: 'PaymentTerm' }, { key: 'PrcntOfTotal', fmt: 'num3', sum: true }, { key: 'FcyAmount', fmt: 'num3', sum: true, edit: 'text' },
        { key: 'DueDays', fmt: 'int' }, { key: 'Received', fmt: 'num3', sum: true }, { key: 'Balance', fmt: 'num3', sum: true }];
    function detRender() { F.drawGrid('detBody', 'detFoot', DET.rows, DCOLS, DET.cur, null); }

    /** INSERT(). */
    function insert(btn) {
        return busy(btn, function () {
            if (DET.rows.length <= 0) { box('Grid Record not found'); return Promise.resolve(); }
            if (!F.hasSel('CmbInvoiceNo')) { box('Please Select Invoice Number First...'); focus('CmbInvoiceNo'); return Promise.resolve(); }
            if (!ask(S.recId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return Promise.resolve();
            var inv = invoice() || {};
            return postJson(API + '/save', { recId: S.recId, invoiceId: netI(val('CmbInvoiceNo')), invoiceDocumentTypeId: netI(inv.DocumentTypeId),
                totalInvoiceAmount: netD(val('txtInvoiceAmount')), rows: DET.rows }).then(function (d) {
                box((d && d.message) || (S.recId === 0 ? 'Save SuccessFully' : 'Update SuccessFully'));
                return formReset();
            }).catch(function (e) { box(e.message); });
        });
    }
    function save(btn) { S.recId = 0; return insert(btn); }
    function update(btn) { if (S.recId === 0) { box('RecId Not found...'); return Promise.resolve(); } return insert(btn); }
    /** ReadById(Id). */
    function readById(id) {
        show('btnsave', false); show('btnupdate', true);
        S.recId = netI(id);
        return getJson(API + '/by-id?id=' + id).then(function (d) {
            tab('tabPage1');
            INVOICES = d.invoices || INVOICES; bindInvoices();
            F.setValOrAdd('CmbInvoiceNo', d.Id, '');
            DET.rows = d.rows || []; DET.cur = -1; detRender();
            S.updateMode = true;
        }).catch(function (e) { box(e.message); }).then(function () { return invoiceLeave(); });
    }
    /** FormReset(). */
    function formReset() {
        S.recId = 0;
        setVal('CmbInvoiceNo', '0'); setText('txtInvoiceBalanceAmt', ''); setText('txtInvoiceAmount', ''); setText('txtBalanceFI', '');
        DET.rows = []; DET.cur = -1; detRender();
        resetPaymentDetails();
        show('btnsave', true); show('btnupdate', false); S.updateMode = false;
        focus('CmbInvoiceNo');
        return getJson(API + '/refresh?recId=0').then(function (d) { INVOICES = d.invoices || []; bindInvoices(); setVal('CmbInvoiceNo', '0'); }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ history

    var HCOLS = [{ key: 'InvoiceNo' }, { key: 'FinancialInstrumentNo' }, { key: 'PaymentTerm' }, { key: 'PrcntOfTotal', fmt: 'num3', sum: true }, { key: 'FcyAmount', fmt: 'num3', sum: true },
        { key: 'DueDays', fmt: 'int' }, { key: 'SortNo', fmt: 'int' }, { key: 'PaymentRemarks' }, { key: 'EntryUserName' }, { key: 'EntryDate', fmt: 'datetime', mmm: false },
        { key: 'ModifyUserName' }, { key: 'ModifyDate', fmt: 'datetime', mmm: false }];
    function histRender() {
        F.drawGrid('histBody', 'histFoot', HIST.rows, HCOLS, HIST.cur, function (r, i) { return '<td class="win-cell-btn"><button type="button" class="win-edit" data-btn="Edit" data-i="' + i + '">Edit</button></td>'; });
        show('histEmpty', HIST.rows.length === 0);
    }
    function historyShow(btn) {
        return busy(btn, function () {
            var fi = F.findRow(HIST_FI, val('CmbFINoHistory'));
            return postJson(API + '/history', { invoiceId: netI(val('CmbInvoiceHistory')), fiId: fi ? netI(fi.Id) : 0, fiDocumentTypeId: fi ? netI(fi.DocumentTypeId) : 0 })
                .then(function (rows) { HIST.rows = rows || []; HIST.cur = -1; histRender(); }).catch(function (e) { box(e.message); });
        });
    }
    function historyReset() { ['CmbInvoiceHistory', 'CmbFINoHistory'].forEach(function (id) { var s = $id(id); if (s) s.selectedIndex = -1; }); F.refreshCombos(); HIST.rows = []; HIST.cur = -1; histRender(); }
    function historyRefresh(btn) {
        return busy(btn, function () {
            return getJson(API + '/history-combos').then(function (d) { HIST_INV = d.historyInvoices || []; bind('CmbInvoiceHistory', HIST_INV, 'Id', 'Name', [], true); HIST_FI = d.historyFis || []; bind('CmbFINoHistory', HIST_FI, 'Id', 'FinancialInstrumentNo', [], true); }).catch(function (e) { box(e.message); });
        });
    }
    function shortcuts() {
        F.shortcutPopup([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+F5', 'For Focus on Invoice No'],
            ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowRight', 'For Moving In Detail Tabs'], ['Ctrl+ArrowDown', 'For Focus On Selected Tab Detail Grid'],
            ['Ctrl+ArrowUp', 'For Focus On First Entry of Detail Selected Tab'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }
    function tab(id) { F.innerTab('main', id, 'btnFiuFooterHistory', function (p, onHist) { focus(onHist ? 'CmbInvoiceHistory' : 'CmbInvoiceNo'); }); }
    function toggleHistory() { tab(F.activeInner('main') === 'tabHistory' ? 'tabPage1' : 'tabHistory'); }

    document.addEventListener('DOMContentLoaded', function () {
        F.wireTabs(function (p, onHist) { if (p === 'tabPage5') return; focus(onHist ? 'CmbInvoiceHistory' : 'CmbInvoiceNo'); }, 'btnFiuFooterHistory');
        F.on('CmbInvoiceNo', 'change', invoiceLeave);
        F.on('CmbFinInstruments', 'change', fiChanged);
        F.on('dtxtfcyamount', 'input', fcyChanged);
        F.on('dtxtpercentoftotal', 'input', percentChanged);
        F.wireGrid('detBody', { select: function (i) { DET.cur = i; }, open: editRow, cell: cellUpdated });
        F.wireGrid('histBody', { select: function (i) { HIST.cur = i; }, open: function (i) { readById(HIST.rows[i].ExImInvoiceId); }, btn: function (name, i) { if (name === 'Edit') readById(HIST.rows[i].ExImInvoiceId); } });
        /* ImProformaInvoice_KeyDown */
        document.addEventListener('keydown', function (e) {
            if (F.enterMoves(e)) return;
            var k = (e.key || '').toLowerCase(), onForm = F.activeInner('main') !== 'tabHistory';
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); F.cancel(); return; }
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); return; }
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
            if (onForm) {
                if (e.ctrlKey && k === 's' && !S.updateMode) { e.preventDefault(); save($id('btnsave')); }
                if (e.ctrlKey && k === 'u' && S.updateMode) { e.preventDefault(); save($id('btnsave')); }     /* desktop: Ctrl+U calls btnsave_Click */
                if (e.ctrlKey && k === 'n') { e.preventDefault(); formReset(); }
                if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh($id('btnrefersh')); }
                if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); focus('CmbInvoiceNo'); }
                if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); var tr = $id('detBody').querySelector('tr'); if (tr) tr.scrollIntoView(); }
                if (e.ctrlKey && e.key === 'ArrowUp') { e.preventDefault(); focus('CmbFinInstruments'); }
                if (e.ctrlKey && e.key === 'Enter' && DET.cur >= 0) { e.preventDefault(); editRow(DET.cur); }
            } else {
                if (e.ctrlKey && k === 's') { e.preventDefault(); historyShow($id('btnShowHistory')); }
                if (e.ctrlKey && k === 'n') { e.preventDefault(); historyReset(); }
                if (e.ctrlKey && k === 'r') { e.preventDefault(); historyRefresh($id('btnRefreshHistory')); }
                if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); focus('CmbInvoiceHistory'); }
                if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); var tr2 = $id('histBody').querySelector('tr'); if (tr2) tr2.scrollIntoView(); }
                if (e.ctrlKey && (e.key === 'Enter' || e.key === ' ') && HIST.cur >= 0 && PERM.Update) { e.preventDefault(); readById(HIST.rows[HIST.cur].Id); }   /* desktop passes the detail Id */
            }
        });
        load();
    });

    global.ExportFIU = { formReset: formReset, save: save, update: update, refresh: refresh, shortcuts: shortcuts, addRow: addRow, updateRow: updateRow, cancelRow: cancelRow,
        historyReset: historyReset, historyRefresh: historyRefresh, historyShow: historyShow, toggleHistory: toggleHistory };
}(window));
