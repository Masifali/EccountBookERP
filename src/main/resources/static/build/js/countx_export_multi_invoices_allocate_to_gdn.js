/* ============================================================================================
 * countx_export_multi_invoices_allocate_to_gdn.js - frmMultiInvoicesAllocateToGdn.cs (Architecture.WinApp.Export),
 * screen 907 "Multi Invoices Allocate To Gdn" (DocumentTypeId 2). Form | History; every button, the header
 * TextChanged chain (GD Value / Freight -> FOB -> FTT % -> FTT Amount -> Net Amount with the desktop's warnings),
 * CmbInvoiceNoDetail_TextChanged, the detail + / Update / Cancel / X / double-click flow with the duplicate-invoice
 * check, Save / Save As / Update, Preview after save, history filters / Show / New / Refresh, Edit / Print / SaveAs
 * grid buttons, selection detail and KeyDown shortcuts; data from /api/export/multi-invoices-allocate-to-gdn.
 * Print -> GenerateReport(Id): seeded 02-gdbreakupheader-slip (id).
 * ============================================================================================ */
(function (global) {
    'use strict';

    var F = global.ExportF, $id = F.$id, box = F.box, ask = F.ask, str = F.str, netI = F.netI, netD = F.netD, val = F.val, setText = F.setText,
        setVal = F.setVal, bind = F.bind, busy = F.busy, getJson = F.getJson, postJson = F.postJson, show = F.show, focus = F.focus;
    var API = '/api/export/multi-invoices-allocate-to-gdn';

    var PERM = { Save: true, Update: true, Delete: true, Print: true };
    var BANKS = [], INVOICES = [], HIST_BANKS = [];
    var S = { recId: 0, days: 0, updateIndex: -1, removed: [], attachmentsValues: '', customAttachmentsValues: '' };
    var DET = { rows: [], cur: -1 };
    var HIST = { rows: [], cur: -1 };

    function bindBanks() { bind('CmbBankMultiInvoiceForm', BANKS, 'Id', 'BranchName', ['BankIBANNo'], true); }
    function bindInvoices() { bind('CmbInvoiceNoDetail', INVOICES, 'Id', 'InvoiceNo', ['BankInvoiceAmount'], true); invoiceChanged(); }
    function bindHistBanks() { bind('CmbBankMultiInvoiceAgainstGdHistory', HIST_BANKS, 'Id', 'Name', [], true); }

    /** InitializeComponentMethod. */
    function load() {
        return getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false, Delete: d.permissions.Delete !== false, Print: d.permissions.Print !== false };
            $id('btnsave').disabled = !PERM.Save; $id('BtnSaveAs').disabled = !PERM.Save; $id('btnUpdate').disabled = !PERM.Update; $id('BtnDelete').disabled = !PERM.Delete; $id('Print').disabled = !PERM.Print;
            ['banks', 'invoices', 'historyBanks'].forEach(function (k) { if (d[k + 'Error']) box(d[k + 'Error']); });
            S.days = netI(d.defaultDaysToLessFromHistoryFromDate);
            BANKS = d.banks || []; bindBanks(); setVal('CmbBankMultiInvoiceForm', '0');
            INVOICES = d.invoices || []; bindInvoices(); setVal('CmbInvoiceNoDetail', '0'); invoiceChanged();
            HIST_BANKS = d.historyBanks || []; bindHistBanks(); setVal('CmbBankMultiInvoiceAgainstGdHistory', '0');
            setText('FromDateHistory', F.daysAgo(S.days > 0 ? S.days : 3)); setText('ToDateHistory', F.today());
            setText('GdDateMultiInvoiceForm', F.today()); setText('BankInvoiceDateMultiInvoiceForm', F.today());
            detRender();
            $id('miaFooterInfo').textContent = 'frmMultiInvoicesAllocateToGdn  -  Document Type 2';
            focus('GdDateMultiInvoiceForm');
        }).catch(function () { box('Error occurred during database call.'); });
    }
    /** btnRefresh_Click. */
    function refresh(btn) {
        return busy(btn, function () {
            return getJson(API + '/refresh?recId=' + S.recId).then(function (d) {
                S.days = netI(d.defaultDaysToLessFromHistoryFromDate);
                BANKS = d.banks || []; bindBanks();
                INVOICES = d.invoices || []; bindInvoices();
            }).catch(function (e) { box(e.message); });
        });
    }

    // ------------------------------------------------------------------------------ header calculations

    /** CalculateFobValueMultiInvoiceForm: Freight > GD Value -> warning + Freight 0; FOB = GD - Freight. */
    function calcFob() {
        var gd = netD(val('txtGdValueMultiInvoiceForm')), fr = netD(val('txtFreightMultiInvoiceForm'));
        if (gd > 0 && fr > 0 && fr > gd) { box("Freight Can't be Greater Than GD Value"); setText('txtFreightMultiInvoiceForm', '0'); }
        setText('txtFobValueMultiInvoiceForm', F.fmt(netD(val('txtGdValueMultiInvoiceForm')) - netD(val('txtFreightMultiInvoiceForm'))));
        calcFtt();
    }
    /** CalculateFTTAmountMultiInvoiceForm. */
    function calcFtt() {
        if (netD(val('FttPrcntMultiInvoiceForm')) > 100) { setText('FttPrcntMultiInvoiceForm', '10'); box('Percent Cant be Greater Than 100'); }
        setText('FttAmountMultiInvoiceForm', F.fmt(netD(val('txtFobValueMultiInvoiceForm')) * netD(val('FttPrcntMultiInvoiceForm')) / 100));
        calcNet();
    }
    /** CalculateNetAmountMultiInvoiceForm: GD > 0 and FTT Amount > 0 -> GD - FTT Amount, else 0. */
    function calcNet() {
        var gd = netD(val('txtGdValueMultiInvoiceForm')), ftt = netD(val('FttAmountMultiInvoiceForm'));
        setText('txtNetAmountMultiInvoiceForm', (gd > 0 && ftt > 0) ? F.fmt(gd - ftt) : F.fmt(0));
    }

    // ------------------------------------------------------------------------------ detail

    /** CmbInvoiceNoDetail_TextChanged: Invoice Amount = the invoice row's BankInvoiceAmount, else "0". */
    function invoiceChanged() {
        var r = F.findRow(INVOICES, val('CmbInvoiceNoDetail'));
        setText('txtInvoiceAmountDetail', r ? F.fmt(r.BankInvoiceAmount, 4) : '0');
    }
    function detailValidation() {
        if (!F.hasSel('CmbInvoiceNoDetail')) { box('Invoice No field is required'); focus('CmbInvoiceNoDetail'); return false; }
        if (netD(val('txtInvoiceAmountDetail')) === 0) { box('Invoice Amount must be a non-zero number'); focus('txtInvoiceAmountDetail'); return false; }
        return true;
    }
    function dup(invoiceId, skip) { return DET.rows.some(function (r, i) { return i !== skip && netI(r.InvoiceId) === invoiceId; }); }
    /** btnAdd_Click. */
    function addRow() {
        if (!detailValidation()) return;
        var id = netI(val('CmbInvoiceNoDetail'));
        if (dup(id, -1)) { box('Invoice ' + F.selText('CmbInvoiceNoDetail') + " Already in Detail Grid.So you can't select this invoice again."); return; }
        DET.rows.push({ Id: 0, InvoiceId: id, InvoiceNo: F.selText('CmbInvoiceNoDetail'), InvoiceAmount: netD(val('txtInvoiceAmountDetail')), Remarks: val('txtRemarksDetail') });
        detRender(); resetDetail();
    }
    /** grd_DoubleClick. */
    function editRow(i) {
        var r = DET.rows[i]; if (!r) return;
        S.updateIndex = i;
        F.setValOrAdd('CmbInvoiceNoDetail', r.InvoiceId, r.InvoiceNo);
        setText('txtInvoiceAmountDetail', F.fmt(r.InvoiceAmount)); setText('txtRemarksDetail', r.Remarks);
        show('btnAdd', false); show('btnUpdateDetail', true); show('btnCancelDetail', true);
        focus('CmbInvoiceNoDetail');
    }
    /** btnUpdateDetail_Click. */
    function updateRow() {
        if (!detailValidation()) return;
        var id = netI(val('CmbInvoiceNoDetail'));
        if (dup(id, S.updateIndex)) { box('Invoice ' + F.selText('CmbInvoiceNoDetail') + " Already in Detail Grid.So you can't select this invoice again."); return; }
        var r = DET.rows[S.updateIndex]; if (!r) return;
        r.InvoiceId = id; r.InvoiceNo = F.selText('CmbInvoiceNoDetail'); r.InvoiceAmount = netD(val('txtInvoiceAmountDetail')); r.Remarks = val('txtRemarksDetail');
        detRender(); resetDetail();
    }
    function cancelRow() { resetDetail(); }
    /** DeleteDetailRow: "Reset Detail First" while editing; saved rows are confirmed and kept as ActionTypeId 3. */
    function deleteRow(i) {
        var r = DET.rows[i]; if (!r) return;
        if (S.updateIndex !== -1) { box('Reset Detail First'); return; }
        if (netI(r.Id) > 0) { if (!ask('Are you sure to Delete?')) return; S.removed.push({ Id: r.Id, InvoiceId: r.InvoiceId, InvoiceNo: r.InvoiceNo, InvoiceAmount: r.InvoiceAmount, Remarks: r.Remarks }); }
        DET.rows.splice(i, 1); detRender();
    }
    function resetDetail() {
        S.updateIndex = -1;
        setVal('CmbInvoiceNoDetail', '0'); setText('txtInvoiceAmountDetail', ''); setText('txtRemarksDetail', '');
        show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelDetail', false);
        focus('CmbInvoiceNoDetail');
    }
    var DCOLS = [{ key: 'InvoiceNo' }, { key: 'InvoiceAmount', fmt: 'num3', sum: true }, { key: 'Remarks', edit: 'text' }];
    function detRender() {
        F.drawGrid('detBody', 'detFoot', DET.rows, DCOLS, DET.cur, function (r, i) { return '<td class="win-cell-btn"><button type="button" class="win-x" data-del="' + i + '" title="Delete">X</button></td>'; });
    }

    // ------------------------------------------------------------------------------ save / read / reset

    /** Insert(). */
    function insert(btn) {
        return busy(btn, function () {
            if (DET.rows.length === 0) { box('Grid Record Not Found'); return Promise.resolve(); }
            if (!val('GdNoMultiInvoiceForm').trim()) { box('Gd No field is required'); focus('GdNoMultiInvoiceForm'); return Promise.resolve(); }
            if (!val('BankInvoiceNoMultiInvoiceForm').trim()) { box('Bank Invoice No field is required'); focus('BankInvoiceNoMultiInvoiceForm'); return Promise.resolve(); }
            if (!F.hasSel('CmbBankMultiInvoiceForm')) { box('Bank field is required'); focus('CmbBankMultiInvoiceForm'); return Promise.resolve(); }
            if (netI(val('txtDueDaysMultiInvoiceForm')) === 0) { box('Due Days must be a non-zero number'); focus('txtDueDaysMultiInvoiceForm'); return Promise.resolve(); }
            if (netD(val('txtGdValueMultiInvoiceForm')) === 0) { box('GD Value must be a non-zero number'); focus('txtGdValueMultiInvoiceForm'); return Promise.resolve(); }
            if (netD(val('txtFobValueMultiInvoiceForm')) === 0) { box('FOB Value must be a non-zero number'); focus('txtFobValueMultiInvoiceForm'); return Promise.resolve(); }
            if (netD(val('txtNetAmountMultiInvoiceForm')) === 0) { box('Net Amount must be a non-zero number'); focus('txtNetAmountMultiInvoiceForm'); return Promise.resolve(); }
            if (!ask(S.recId === 0 ? 'Are you sure to Save' : 'Are you sure to Update')) return Promise.resolve();
            var total = DET.rows.reduce(function (a, r) { return a + netD(r.InvoiceAmount); }, 0);
            if (netD(val('txtGdValueMultiInvoiceForm')) !== total) { box("Invoice's Amount must be equal to Total GDValue Please Check"); return Promise.resolve(); }
            var preview = F.checked('ChkPreview');
            return postJson(API + '/save', {
                recId: S.recId, gdNo: val('GdNoMultiInvoiceForm'), gdDate: val('GdDateMultiInvoiceForm'), bankInvoiceNo: val('BankInvoiceNoMultiInvoiceForm'),
                bankInvoiceDate: val('BankInvoiceDateMultiInvoiceForm'), bankId: netI(val('CmbBankMultiInvoiceForm')), dueDays: netI(val('txtDueDaysMultiInvoiceForm')),
                exchangeRate: netD(val('txtExchangeRateMultiInvoiceForm')), gdValue: netD(val('txtGdValueMultiInvoiceForm')), freight: netD(val('txtFreightMultiInvoiceForm')),
                fobValue: netD(val('txtFobValueMultiInvoiceForm')), fttPercent: netD(val('FttPrcntMultiInvoiceForm')), fttAmount: netD(val('FttAmountMultiInvoiceForm')),
                netAmount: netD(val('txtNetAmountMultiInvoiceForm')), remarks: val('txtRemarksMultiInvoiceForm'), details: DET.rows, removed: S.removed,
                attachmentsValues: S.attachmentsValues, customAttachmentsValues: S.customAttachmentsValues
            }).then(function (d) {
                box((d && d.message) || (S.recId === 0 ? 'Record Save Successfully' : 'Record Update Successfully'));
                var id = d && d.id;
                return reset().then(function () { if (preview) generateReport(id); });
            }).catch(function (e) { box(e.message); });
        });
    }
    function save(btn) { S.recId = 0; return insert(btn); }
    function saveAs(btn) { S.recId = 0; return insert(btn); }
    function update(btn) { if (S.recId === 0) { box('RecId not Found'); return Promise.resolve(); } return insert(btn); }
    /** ReadById(ID). */
    function readById(id) {
        return reset().then(function () { return getJson(API + '/by-id?id=' + encodeURIComponent(id)); }).then(function (r) {
            S.recId = netI(r.Id);
            tab('tabPage1');
            show('btnsave', false); show('btnUpdate', true); show('BtnSaveAs', false);
            setText('GdNoMultiInvoiceForm', r.GDNO); setText('GdDateMultiInvoiceForm', r.GDDate || F.today());
            setText('BankInvoiceNoMultiInvoiceForm', r.BankInvoiceNo); setText('BankInvoiceDateMultiInvoiceForm', r.BankInvoiceDate || F.today());
            F.setValOrAdd('CmbBankMultiInvoiceForm', r.BankId, '');
            setText('txtDueDaysMultiInvoiceForm', str(r.DueDays)); setText('txtExchangeRateMultiInvoiceForm', F.fmt(r.ExchangeRate, 4));
            setText('txtGdValueMultiInvoiceForm', F.fmt(r.GDValue, 4)); setText('txtFreightMultiInvoiceForm', F.fmt(r.Freight, 4));
            setText('txtFobValueMultiInvoiceForm', F.fmt(r.FobValue, 4)); setText('FttPrcntMultiInvoiceForm', F.fmt(r.CommPercent, 4));
            setText('FttAmountMultiInvoiceForm', F.fmt(r.CommAmount, 4)); setText('txtNetAmountMultiInvoiceForm', F.fmt(r.NetToBeRealized, 4));
            setText('txtRemarksMultiInvoiceForm', r.Remarks);
            S.attachmentsValues = str(r.AttachmentsValues); S.customAttachmentsValues = str(r.CustomAttachmentsValues);
            DET.rows = r.details || []; DET.cur = -1; detRender();
            INVOICES = r.invoices || INVOICES; bindInvoices();
        }).catch(function (e) { box(e.message); });
    }
    /** Reset(). */
    function reset() {
        S.removed = []; S.attachmentsValues = ''; S.customAttachmentsValues = ''; S.recId = 0;
        show('btnsave', true); show('btnUpdate', false); show('BtnDelete', false); show('BtnSaveAs', false);
        setText('GdNoMultiInvoiceForm', ''); setText('BankInvoiceNoMultiInvoiceForm', ''); setVal('CmbBankMultiInvoiceForm', '0');
        ['txtDueDaysMultiInvoiceForm', 'txtExchangeRateMultiInvoiceForm', 'txtGdValueMultiInvoiceForm', 'txtFreightMultiInvoiceForm', 'txtFobValueMultiInvoiceForm',
            'FttAmountMultiInvoiceForm', 'txtNetAmountMultiInvoiceForm', 'txtRemarksMultiInvoiceForm'].forEach(function (id) { setText(id, ''); });
        setText('FttPrcntMultiInvoiceForm', '10');
        resetDetail();
        DET.rows = []; DET.cur = -1; detRender();
        tab('tabPage1');
        focus('GdDateMultiInvoiceForm');
        return Promise.resolve();
    }
    function newClick() { reset(); resetDetail(); }

    // ------------------------------------------------------------------------------ history

    var HCOLS = [{ key: 'InvoiceNos' }, { key: 'GDNo' }, { key: 'GDDate', fmt: 'date' }, { key: 'BankInvoiceNo' }, { key: 'BankInvoiceDate', fmt: 'date' }, { key: 'BankName' },
        { key: 'DueDays', fmt: 'int' }, { key: 'ExchangeRate', fmt: 'rate' }, { key: 'GDValue', fmt: 'num3', sum: true }, { key: 'Freight', fmt: 'num3', sum: true },
        { key: 'FobValue', fmt: 'num3', sum: true }, { key: 'FttPercent', fmt: 'num3' }, { key: 'FttAmount', fmt: 'num3', sum: true }, { key: 'NetAmount', fmt: 'num3', sum: true },
        { key: 'EntryUser' }, { key: 'EntryDate', fmt: 'date' }, { key: 'ModifyUser' }, { key: 'ModifyDate', fmt: 'date' }, { key: 'Remarks' }, { key: 'NoOfAttachments', fmt: 'int', link: true }];
    function histRender() {
        F.drawGrid('histBody', 'histFoot', HIST.rows, HCOLS, HIST.cur, function (r, i) {
            return '<td class="win-cell-btn"><button type="button" class="win-edit" data-btn="Edit" data-i="' + i + '">Edit</button></td>' +
                '<td class="win-cell-btn"><button type="button" class="win-edit" data-btn="Print" data-i="' + i + '">Print</button></td>';
        }, { leadCols: 2 });
        show('histEmpty', HIST.rows.length === 0);
    }
    function historyShow(btn) {
        return busy(btn, function () {
            return postJson(API + '/history', {
                dateBy: (document.querySelector('input[name="dateBy"]:checked') || {}).value || 'doc',
                fromChecked: F.checked('FromDateHistoryChk'), toChecked: F.checked('ToDateHistoryChk'), fromDate: val('FromDateHistory'), toDate: val('ToDateHistory'),
                bankId: netI(val('CmbBankMultiInvoiceAgainstGdHistory'))
            }).then(function (rows) { HIST.rows = rows || []; HIST.cur = -1; histRender(); F.drawGrid('hdetBody', 'hdetFoot', [], DCOLS.map(function (c) { return { key: c.key, fmt: c.fmt, sum: c.sum }; }), -1, null); }).catch(function (e) { box(e.message); });
        });
    }
    function historySelect(i) {
        HIST.cur = i; var r = HIST.rows[i]; if (!r) return;
        getJson(API + '/details?id=' + r.Id).then(function (rows) { F.drawGrid('hdetBody', 'hdetFoot', rows || [], [{ key: 'InvoiceNo' }, { key: 'InvoiceAmount', fmt: 'num3', sum: true }, { key: 'Remarks' }], -1, null); }).catch(function (e) { box(e.message); });
    }
    function historyReset() {
        setText('FromDateHistory', F.daysAgo(3)); setText('ToDateHistory', F.today()); setVal('CmbBankMultiInvoiceAgainstGdHistory', '0');
        HIST.rows = []; HIST.cur = -1; histRender(); $id('hdetBody').innerHTML = ''; $id('hdetFoot').innerHTML = '';
    }
    function historyRefresh(btn) { return busy(btn, function () { return getJson(API + '/history-banks').then(function (rows) { HIST_BANKS = rows || []; bindHistBanks(); }).catch(function (e) { box(e.message); }); }); }
    /** GenerateReport(PrintId): [dbo].[USP_GDBreakUpHeader_Slip] @Id -> 02_GDBreakUpHeader_Slip.rpt. */
    function generateReport(id) { if (netI(id) === 0) { box('No Record Found For Display'); return; } F.printSeeded('02-gdbreakupheader-slip', { id: id }); }
    function print() { if (S.recId > 0) generateReport(S.recId); else box('No Record found'); }
    function attachment() { box('Attachments (DMS popup) are not part of the web port.'); }
    function shortcuts() {
        F.shortcutPopup([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P / Ctrl+1', 'For Print'],
            ['Ctrl+F5', 'For Focus on GD Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form']]);
    }
    function tab(id) { F.innerTab('main', id, 'btnMiaFooterHistory', function (p, onHist) { focus(onHist ? 'FromDateHistory' : 'GdDateMultiInvoiceForm'); }); }
    function toggleHistory() { tab(F.activeInner('main') === 'tabHistory' ? 'tabPage1' : 'tabHistory'); }

    document.addEventListener('DOMContentLoaded', function () {
        F.wireTabs(function (p, onHist) { focus(onHist ? 'FromDateHistory' : 'GdDateMultiInvoiceForm'); }, 'btnMiaFooterHistory');
        F.on('CmbInvoiceNoDetail', 'change', invoiceChanged);
        F.on('txtGdValueMultiInvoiceForm', 'input', calcFob);
        F.on('txtFreightMultiInvoiceForm', 'input', calcFob);
        F.on('FttPrcntMultiInvoiceForm', 'input', calcFtt);
        F.wireGrid('detBody', { select: function (i) { DET.cur = i; }, open: editRow, del: deleteRow, cell: function (i, k, v) { var r = DET.rows[i]; if (r) r[k] = v; } });
        F.wireGrid('histBody', {
            select: historySelect,
            open: function (i) { readById(HIST.rows[i].Id); },
            btn: function (name, i) { if (name === 'Edit') readById(HIST.rows[i].Id); else if (name === 'Print') generateReport(HIST.rows[i].Id); },
            link: function (k, i) { box('Attachments of record ' + HIST.rows[i].Id + ': ' + netI(HIST.rows[i].NoOfAttachments)); }
        });
        /* frmMultiInvoicesAllocateToGdn_KeyDown */
        document.addEventListener('keydown', function (e) {
            if (F.enterMoves(e)) return;
            var k = (e.key || '').toLowerCase(), onForm = F.activeInner('main') !== 'tabHistory';
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); F.cancel(); return; }
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); return; }
            if (onForm) {
                if (e.ctrlKey && k === 's' && !$id('btnsave').classList.contains('is-hidden')) { e.preventDefault(); save($id('btnsave')); }
                if (e.ctrlKey && k === 'u' && !$id('btnUpdate').classList.contains('is-hidden') && S.recId > 0) { e.preventDefault(); update($id('btnUpdate')); }
                if (e.ctrlKey && (k === 'p' || e.key === '1')) { e.preventDefault(); print(); }
                if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); focus('GdDateMultiInvoiceForm'); }
                if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); attachment(); }
                if (e.ctrlKey && k === 'n') { e.preventDefault(); newClick(); }
                if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh($id('BtnRefresh')); }
            } else {
                if (e.ctrlKey && k === 's') { e.preventDefault(); historyShow($id('btnShowHistory')); }
                if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); focus('FromDateHistory'); }
                if (e.ctrlKey && k === 'n') { e.preventDefault(); historyReset(); }
                if (e.ctrlKey && k === 'r') { e.preventDefault(); historyRefresh($id('btnRefreshHistory')); }
                if (e.ctrlKey && e.key === 'Enter' && HIST.cur >= 0) { e.preventDefault(); readById(HIST.rows[HIST.cur].Id); }
            }
        });
        load();
    });

    global.ExportMIA = { newClick: newClick, refresh: refresh, save: save, saveAs: saveAs, update: update, attachment: attachment, print: print, shortcuts: shortcuts,
        addRow: addRow, updateRow: updateRow, cancelRow: cancelRow, historyReset: historyReset, historyRefresh: historyRefresh, historyShow: historyShow, toggleHistory: toggleHistory };
}(window));
