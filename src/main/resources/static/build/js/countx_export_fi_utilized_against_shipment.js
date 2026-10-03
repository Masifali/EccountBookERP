/* ============================================================================================
 * countx_export_fi_utilized_against_shipment.js - FinancialInstrumentsUtilizedAgainstShipment.cs
 * (Architecture.WinApp.Export) "Financial Instruments Utilized Against Shipment".
 * Events: Load, CmbInvoiceNo_Leave, CmbFinInstruments ValueChanged, % of Total / Fcy Amount / Balance Leave,
 * + / Update / Cancel, grid DoubleClick / X / KeyDown, New, Save, Update, Refresh, ShortCut Keys,
 * history New / Refresh / Show / DoubleClick / Edit / KeyDown, form KeyDown - desktop messages and order.
 * Data: /api/export/fi-utilized-against-shipment (rights of ScreenDefinition 202).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var G = global.ExportG, S = global.ExportSF;
    var API = '/api/export/fi-utilized-against-shipment';

    var RIGHTS = { Save: false, Update: false };
    var RECID = 0, UPDATE_MODE = false, UPDATE_INDEX = -1, FCY_DEC = 0;
    var INVOICES = [], FIS = [], TERMS = [], HIST = [], HIST_FIS = [];

    /* stringFormatsingleForFcy = "#,##0." + N zeros (N from DefaultNoOfDecimalPointsForFcyAmount). */
    function fcyFmt(v) { return G.fmtFixed(v, FCY_DEC); }
    /* "#,#.##" for the %OfTotal / FcyAmount grid columns (0 prints empty). */
    function hash2(v) { return S.fmtHash(v, 2); }

    var grd = G.grid('grdpaymentdetail', [
        { key: 'FinancialInstrumentNo' }, { key: 'PaymentTerm' }, { key: 'OfTotal', num: true, fmt: hash2 }, { key: 'FcyAmount', num: true, fmt: hash2 }, { key: 'DueDays', num: true, dec: 0 }
    ], { withX: true, totals: ['OfTotal', 'FcyAmount'], onDelete: deleteRow, onDblClick: editRow });

    var grdHistory = G.grid('DataGridHistory', [
        { key: 'InvoiceNo', link: true }, { key: 'FinancialInstrumentNo' }, { key: 'PaymentTerm' }, { key: 'PrcntOfTotal', num: true }, { key: 'FcyAmount', num: true },
        { key: 'DueDays', num: true, dec: 0 }, { key: 'SortNo', num: true, dec: 0 }, { key: 'PaymentRemarks' }, { key: 'EntryUserName' },
        { key: 'EntryDate', datetime: true }, { key: 'ModifyUserName' }, { key: 'ModifyDate', datetime: true }
    ], { buttons: [{ key: 'Edit', text: 'Edit' }], emptyId: 'DataGridHistoryEmpty',
        onButton: function (i) { var r = HIST[i]; if (r) readById(G.netI(r.ExImInvoiceId)); },
        onDblClick: function (i) { var r = HIST[i]; if (r) readById(G.netI(r.ExImInvoiceId)); },
        onLink: function (i) { var r = HIST[i]; if (r) readById(G.netI(r.ExImInvoiceId)); } });

    var footerLabel = null;
    var tabs = G.tabs('main', ['tabForm', 'tabHistory'], function (p) {
        if (footerLabel) footerLabel();
        if (p === 'tabHistory') G.focus('CmbInvoiceHistory'); else G.focus('CmbInvoiceNo');      /* tabControl1_SelectedIndexChanged */
    });
    footerLabel = S.footerToggle('btnFooterHistory', tabs, 'tabForm', 'tabHistory');

    function rowOf(list, id) { return G.rowOf(list, 'Id', id); }
    function selText(id) { var s = G.$id(id); return s && s.selectedIndex > 0 ? s.options[s.selectedIndex].textContent : ''; }

    // ------------------------------------------------------------------------------ binds

    /** InvoiceNoFill: DocumentTypeId hidden; value kept when still listed. */
    function bindInvoices(rows, keep) {
        INVOICES = rows || [];
        S.bindX('CmbInvoiceNo', INVOICES, 'Id', 'InvoiceNo', ['BankAmount', 'SupplierCustomerId', 'TotalAmount'], keep);
    }
    function bindFis(rows) { FIS = rows || []; S.bindX('CmbFinInstruments', FIS, 'Id', 'EFormNo', ['BalFIAmount']); }
    function bindTerms(rows) { TERMS = rows || []; S.bindX('dcmbpaymentterm', TERMS, 'Id', 'LcOrderTerm', []); }
    function applyHistoryCombos(d) {
        ['historyInvoices', 'historyFis', 'historyFisFromInvoice'].forEach(function (k) { if (d[k + 'Error']) G.box(d[k + 'Error']); });
        if ((d.historyInvoices || []).length) G.bind('CmbInvoiceHistory', d.historyInvoices, 'Id', 'name');
        if ((d.historyFisFromInvoice || []).length) { HIST_FIS = []; G.bind('CmbFINoHistory', d.historyFisFromInvoice, 'Id', 'name'); }
        if ((d.historyFis || []).length) { HIST_FIS = d.historyFis; G.bind('CmbFINoHistory', HIST_FIS, 'Id', 'FinancialInstrumentNo'); }
    }

    // ------------------------------------------------------------------------------ load

    function load() {
        return G.getJson(API + '/setup').then(function (d) {
            d = d || {};
            RIGHTS = d.rights || RIGHTS;
            FCY_DEC = G.netI(d.fcyDecimals);
            G.$id('btnsave').disabled = !RIGHTS.Save;
            G.$id('btnupdate').disabled = !RIGHTS.Update;
            ['invoices', 'fis', 'paymentTerms'].forEach(function (k) { if (d[k + 'Error']) G.box(d[k + 'Error']); });
            bindInvoices(d.invoices); bindFis(d.fis); bindTerms(d.paymentTerms);
            applyHistoryCombos(d);
            grd.draw([]);
            G.setEnabled('CmbInvoiceNo', true);
        }).catch(function (e) { G.box(e.message); });
    }

    // ------------------------------------------------------------------------------ entry events

    /** CmbInvoiceNo_Leave: BankAmount "#,##.###" (0 prints empty). */
    function invoiceLeave() {
        var r = rowOf(INVOICES, G.val('CmbInvoiceNo'));
        G.setText('txtInvoiceBalanceAmt', r && G.valI('CmbInvoiceNo') !== 0 ? S.fmtHash(r.BankAmount, 3) : '');
    }
    /** dcmbfino_ValueChanged: the FI's payment term, then GetFinancialInstrumentsBalance. */
    function fiChanged() {
        var r = rowOf(FIS, G.val('CmbFinInstruments'));
        if (!r || G.valI('CmbFinInstruments') <= 0) return Promise.resolve();
        G.setVal('dcmbpaymentterm', G.netI(r.PaymenttermId));
        return G.getJson(API + '/fi-balance?documentTypeId=' + G.netI(r.DocumentTypeId) + '&id=' + G.netI(r.Id))
            .then(function (d) { G.setText('txtBalanceFI', d ? d.balance : '0'); }).catch(function (e) { G.box(e.message); });
    }
    /** PaymentDetailPercentCalculate (Leave of % of Total and of Balance). */
    function percentCalc() {
        if (G.valI('CmbInvoiceNo') > 0 && G.netD(G.val('dtxtpercentoftotal')) > 0) {
            if (G.netD(G.val('dtxtpercentoftotal')) > 100) { G.setText('dtxtpercentoftotal', '0'); G.box('Percent cannot be greater than 100 Please check'); return; }
            var total = G.netD(G.val('txtInvoiceBalanceAmt')), pct = G.netD(G.val('dtxtpercentoftotal').trim());
            if (total > 0 && pct > 0) G.setText('dtxtfcyamount', fcyFmt(total * pct / 100.0));
        }
    }
    /** dtxtfcyamount_TextChanged (wired to Leave). */
    function fcyLeave() {
        var total = G.netD(G.val('txtInvoiceBalanceAmt'));
        if (total > 0 && G.netD(G.val('dtxtfcyamount')) > 0) {
            var pct = G.netD(G.val('dtxtfcyamount').trim()) / total * 100.0;
            G.setText('dtxtpercentoftotal', G.fmt(pct, 3));
            if (G.netD(G.val('dtxtpercentoftotal')) > 100) { G.setText('dtxtpercentoftotal', '0'); G.box('Percent cannot be greater than 100 Please check'); }
        }
    }

    // ------------------------------------------------------------------------------ payment detail grid

    /** PaymentDetailFormValidation. */
    function detailValidation() {
        if (G.valI('CmbInvoiceNo') === 0) { G.box('Please Select Invoice Number First...'); G.focus('CmbInvoiceNo'); return false; }
        if (G.valI('CmbFinInstruments') === 0) { G.box('Financial Instrument No field required'); G.focus('CmbFinInstruments'); return false; }
        if (G.valI('dcmbpaymentterm') === 0) { G.box('Payment Term field required'); G.focus('dcmbpaymentterm'); return false; }
        if (G.netD(G.val('dtxtpercentoftotal').trim()) === 0 || G.val('dtxtpercentoftotal') === '') { G.box('Percent Field Required'); G.focus('dtxtpercentoftotal'); return false; }
        if (G.netD(G.val('dtxtfcyamount').trim()) === 0 || G.val('dtxtfcyamount') === '') { G.box('Fcy Amount  Field Required'); G.focus('dtxtfcyamount'); return false; }
        return true;
    }
    function fiDocType() { var r = rowOf(FIS, G.val('CmbFinInstruments')); return r ? G.netI(r.DocumentTypeId) : 0; }
    function duplicate(except) {
        var fi = G.valI('CmbFinInstruments'), dt = fiDocType();
        return grd.rows.some(function (r, i) { return i !== except && G.netI(r.ExImEFormRegistrationId) === fi && G.netI(r.DocumentTypeId) === dt; });
    }
    /** btndadd_Click. */
    function addRow() {
        if (!detailValidation()) return;
        if (duplicate(-1)) { G.box('Financial Instrument No already add in Grid Please Check'); return; }
        grd.rows.push({ Id: 0, DocumentTypeId: fiDocType(), ExImEFormRegistrationId: G.valI('CmbFinInstruments'), FinancialInstrumentNo: selText('CmbFinInstruments'),
            PaymentTermId: G.valI('dcmbpaymentterm'), PaymentTerm: selText('dcmbpaymentterm'), OfTotal: G.netD(G.val('dtxtpercentoftotal').trim()),
            FcyAmount: G.netD(G.val('dtxtfcyamount').trim()), DueDays: G.netD(G.val('dtxtdueday').trim()) });
        grd.draw(grd.rows);
        resetDetails();
    }
    /** btndupdate_Click. */
    function updateRow() {
        if (!detailValidation()) return;
        if (duplicate(UPDATE_INDEX)) { G.box('Financial Instrument No already add in Grid Please Check'); return; }
        var r = grd.rows[UPDATE_INDEX]; if (!r) return;
        r.DocumentTypeId = fiDocType(); r.ExImEFormRegistrationId = G.valI('CmbFinInstruments'); r.FinancialInstrumentNo = selText('CmbFinInstruments');
        r.PaymentTermId = G.valI('dcmbpaymentterm'); r.PaymentTerm = selText('dcmbpaymentterm');
        r.OfTotal = G.netD(G.val('dtxtpercentoftotal').trim()); r.FcyAmount = G.netD(G.val('dtxtfcyamount').trim()); r.DueDays = G.netI(G.val('dtxtdueday'));
        grd.draw(grd.rows);
        resetDetails();
        G.focus('CmbFinInstruments');
    }
    /** grdpaymentdetail_DoubleClick. */
    function editRow(i) {
        var r = grd.rows[i]; if (!r) return;
        UPDATE_INDEX = i;
        G.setVal('CmbFinInstruments', r.ExImEFormRegistrationId);
        G.setVal('dcmbpaymentterm', r.PaymentTermId);
        G.setText('dtxtpercentoftotal', G.fmt(r.OfTotal, 3));
        G.setText('dtxtfcyamount', fcyFmt(r.FcyAmount));
        G.setText('dtxtdueday', G.str(r.DueDays));
        G.show('btndadd', false); G.show('btndupdate', true); G.show('btndcancel', true);
    }
    /** grdpaymentdetail_ColumnButtonClick "Delete". */
    function deleteRow(i) {
        var r = grd.rows[i]; if (!r) return;
        if (UPDATE_INDEX !== -1) { G.box('Reset Detail First...'); return; }
        if (G.netI(r.Id) > 0) { G.box('You can not Delete this Row...'); return; }
        grd.rows.splice(i, 1); grd.draw(grd.rows);
    }
    /** ResePaymentDetails. */
    function resetDetails() {
        UPDATE_INDEX = -1;
        G.setVal('CmbFinInstruments', '0'); G.setVal('dcmbpaymentterm', '0');
        G.setText('dtxtpercentoftotal', '0'); G.setText('dtxtfcyamount', '0'); G.setText('dtxtdueday', '0');
        G.show('btndadd', true); G.show('btndupdate', false); G.show('btndcancel', false);
        G.focus('CmbFinInstruments');
    }

    // ------------------------------------------------------------------------------ form

    function invoiceNoFill(keep) {
        return G.getJson(API + '/invoices?recId=' + RECID).then(function (rows) {
            if ((rows || []).length > 0) bindInvoices(rows, keep);
            else { INVOICES = []; S.bindX('CmbInvoiceNo', [], 'Id', 'InvoiceNo', []); }
        }).catch(function (e) { G.box(e.message); });
    }
    /** FormReset (btnNew_Click). */
    function formReset() {
        RECID = 0;
        G.setVal('CmbInvoiceNo', '0'); G.setText('txtInvoiceBalanceAmt', '');
        grd.draw([]);
        resetDetails();
        G.show('btnsave', true); G.show('btnupdate', false);
        UPDATE_MODE = false;
        return invoiceNoFill().then(function () { G.focus('CmbInvoiceNo'); });
    }
    /** btnrefersh_Click. */
    function refresh(btn) {
        return G.busy(btn, function () {
            return G.getJson(API + '/refresh?recId=' + RECID).then(function (d) {
                d = d || {};
                ['invoices', 'fis', 'paymentTerms'].forEach(function (k) { if (d[k + 'Error']) G.box(d[k + 'Error']); });
                bindInvoices(d.invoices); bindFis(d.fis); bindTerms(d.paymentTerms);
            }).catch(function (e) { G.box(e.message); });
        });
    }
    /** INSERT(). */
    function insert(btn) {
        return G.busy(btn, function () {
            if (grd.rows.length <= 0) { G.box('Grid Record not found'); return Promise.resolve(); }
            if (G.valI('CmbInvoiceNo') === 0) { G.box('Please Select Invoice Number First...'); G.focus('CmbInvoiceNo'); return Promise.resolve(); }
            if (!G.ask(RECID === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return Promise.resolve();
            var inv = rowOf(INVOICES, G.val('CmbInvoiceNo'));
            return G.postJson(API + '/save', { recId: RECID, invoiceId: G.valI('CmbInvoiceNo'), invoiceDocumentTypeId: inv ? G.netI(inv.DocumentTypeId) : 0,
                invoiceBalanceText: G.val('txtInvoiceBalanceAmt'), rows: grd.rows }).then(function (d) {
                G.box(d && d.message);
                return formReset();
            }).catch(function (e) { G.box(e.message); });
        });
    }
    function save(btn) { RECID = 0; return insert(btn); }
    function update(btn) {
        if (RECID === 0) { G.box('RecId Not found...'); return Promise.resolve(); }
        return insert(btn);
    }
    /** ReadById(Id): Update mode and RecId are set first; the combo fills run whatever the read did. */
    function readById(id) {
        G.show('btnsave', false); G.show('btnupdate', true);
        RECID = id;
        return G.getJson(API + '/by-id?id=' + id).then(function (h) {
            tabs.select('tabForm'); if (footerLabel) footerLabel();
            G.setVal('CmbInvoiceNo', h.Id);
            grd.draw(h.rows || []);
            UPDATE_MODE = true;
            return h.Id;
        }).catch(function (e) { G.box(e.message); return 0; }).then(function (keep) {
            return invoiceNoFill(keep || G.val('CmbInvoiceNo')).then(function () {
                invoiceLeave();
                return G.getJson(API + '/refresh?recId=' + RECID).then(function (d) { bindFis((d || {}).fis); });
            });
        });
    }

    // ------------------------------------------------------------------------------ history

    function historyReset() { G.setVal('CmbInvoiceHistory', '0'); G.setVal('CmbFINoHistory', '0'); HIST = []; grdHistory.draw([]); }
    function historyRefresh(btn) {
        return G.busy(btn, function () { return G.getJson(API + '/history-combos').then(function (d) { applyHistoryCombos(d || {}); }).catch(function (e) { G.box(e.message); }); });
    }
    /** HistoryGridFill: @EFormRegistrationId = Conversion.ToInt(the FI number text), @DocumentTypeId = its DocumentTypeId. */
    function historyShow(btn) {
        return G.busy(btn, function () {
            var eForm = 0, dt = 0;
            var fi = rowOf(HIST_FIS, G.val('CmbFINoHistory'));
            if (fi && G.valI('CmbFINoHistory') > 0) { eForm = G.netI(fi.FinancialInstrumentNo); dt = G.netI(fi.DocumentTypeId); }
            return G.getJson(API + '/history?invoiceId=' + G.valI('CmbInvoiceHistory') + '&eFormRegistrationId=' + eForm + '&documentTypeId=' + dt).then(function (rows) {
                HIST = rows || []; grdHistory.current = -1; grdHistory.draw(HIST);
            }).catch(function (e) { G.box(e.message); });
        });
    }
    function shortcuts() {
        G.shortcuts([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+F5', 'For Focus on Invoice No'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
            ['Ctrl+ArrowRight', 'For Moving In Detail Tabs'], ['Ctrl+ArrowDown', 'For Focus On Selected Tab Detail Grid'],
            ['Ctrl+ArrowUp', 'For Focus On First Entry of Detail Selected Tab'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    // ------------------------------------------------------------------------------ wiring

    function on(id, ev, fn) { var e = G.$id(id); if (e) e.addEventListener(ev, fn); }

    document.addEventListener('DOMContentLoaded', function () {
        G.initFullscreen();
        on('btnNew', 'click', formReset);
        on('btnsave', 'click', function () { save(this); });
        on('btnupdate', 'click', function () { update(this); });
        on('btnrefersh', 'click', function () { refresh(this); });
        on('btnShortCutKey', 'click', shortcuts);
        on('btndadd', 'click', addRow);
        on('btndupdate', 'click', updateRow);
        on('btndcancel', 'click', resetDetails);
        on('CmbInvoiceNo', 'change', invoiceLeave);
        on('CmbFinInstruments', 'change', fiChanged);
        on('dtxtpercentoftotal', 'blur', percentCalc);
        on('txtBalanceFI', 'blur', percentCalc);
        on('dtxtfcyamount', 'blur', fcyLeave);
        on('btnResetHistory', 'click', historyReset);
        on('btnRefreshHistory', 'click', function () { historyRefresh(this); });
        on('btnShowHistory', 'click', function () { historyShow(this); });
        /* DataGridHistory_KeyDown: Ctrl+Enter with the Update right reads the row's own Id (as the desktop does). */
        G.$id('DataGridHistoryBox').addEventListener('keydown', function (e) {
            if (!(e.ctrlKey && e.key === 'Enter')) return;
            e.preventDefault(); e.stopPropagation();
            if (grdHistory.current >= 0 && RIGHTS.Update) readById(G.netI(HIST[grdHistory.current].Id));
        }, true);
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase(), onForm = tabs.current() === 'tabForm';
            if (S.enterAsTab(e)) return;
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { if (!document.querySelector('.ex-fullscreen')) { e.preventDefault(); G.cancelWindow(); } return; }
            if (e.ctrlKey && k === 't') { e.preventDefault(); tabs.select(onForm ? 'tabHistory' : 'tabForm'); return; }
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
            if (onForm) {
                if (e.ctrlKey && k === 's' && !UPDATE_MODE) { e.preventDefault(); save(G.$id('btnsave')); }
                if (e.ctrlKey && k === 'u' && UPDATE_MODE) { e.preventDefault(); save(G.$id('btnsave')); }     /* Q3: btnsave_Click */
                if (e.ctrlKey && k === 'n') { e.preventDefault(); formReset(); }
                if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh(G.$id('btnrefersh')); }
                if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); G.focus('CmbInvoiceNo'); }
                if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); G.focus('grdpaymentdetail'); }
                if (e.ctrlKey && e.key === 'ArrowUp') { e.preventDefault(); G.focus('CmbFinInstruments'); }
            } else {
                if (e.ctrlKey && k === 's') { e.preventDefault(); historyShow(G.$id('btnShowHistory')); }
                if (e.ctrlKey && k === 'n') { e.preventDefault(); historyReset(); }
                if (e.ctrlKey && k === 'r') { e.preventDefault(); historyRefresh(G.$id('btnRefreshHistory')); }
                if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); G.focus('CmbInvoiceHistory'); }
                if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); G.focus('DataGridHistory'); }
            }
        });
        load();
    });

    global.ExportFiUtilized = { formReset: formReset, save: save, update: update, readById: readById };
})(window);
