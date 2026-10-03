/* ============================================================================================
 * countx_export_pre_shipment_inspection.js - PreShipmentInspection.cs (Architecture.WinApp.Export), screen 188
 * "Pre-Shipment Inspection". Form | History; every button, CmbCustomer_Leave (contracts per customer), the
 * detail + / Update / Cancel / X / double-click flow with InstertMtonInHeader, Save / Update with the desktop's
 * validations, history filters / Show / New / Refresh, Edit / Print buttons, selection detail and the KeyDown
 * shortcuts have their counterpart here; data from /api/export/pre-shipment-inspection.
 * Print -> CommonServices.InvLabPreProductionExportLotInspection_Slip(Id): seeded 513-invlabpreproductionslip (id).
 * ============================================================================================ */
(function (global) {
    'use strict';

    var F = global.ExportF, $id = F.$id, box = F.box, ask = F.ask, str = F.str, netI = F.netI, netD = F.netD, val = F.val, setText = F.setText,
        setVal = F.setVal, bind = F.bind, busy = F.busy, getJson = F.getJson, postJson = F.postJson, show = F.show, focus = F.focus;
    var API = '/api/export/pre-shipment-inspection';

    var PERM = { Save: true, Update: true, Print: true };
    var STATUSES = [], JOBLOTS = [], ITEMS = [], CUSTOMERS = [], CONTRACTS = [], HIST_ITEMS = [];
    var S = { recId: 0, days: 0, updateIndex: -1, attachmentsValues: '', customAttachmentsValues: '' };
    var DET = { rows: [], cur: -1 };
    var HIST = { rows: [], cur: -1 };

    function applyData(d) {
        ['inspectionStatuses', 'jobLots', 'items'].forEach(function (k) { if (d[k + 'Error']) box(d[k + 'Error']); });
        STATUSES = d.inspectionStatuses || []; bind('CmbInspectionStatus', STATUSES, 'Id', 'Name', []);
        JOBLOTS = d.jobLots || []; bind('CmbJobLot', JOBLOTS, 'Id', 'Name', []);
        ITEMS = d.items || []; bind('CmbItemName', ITEMS, 'Id', 'Name', []);
        CUSTOMERS = d.customers || []; bind('CmbCustomer', CUSTOMERS, 'Id', 'Name', []);
    }
    /** InitializeComponentMethod. */
    function load() {
        return getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false, Print: d.permissions.Print !== false };
            $id('btnsave').disabled = !PERM.Save; $id('btnPrint').disabled = !PERM.Print; $id('btnUpdate').disabled = !PERM.Update;
            S.days = netI(d.defaultDaysToLessFromHistoryFromDate);
            if (d.docNoError) box(d.docNoError);
            if (S.recId === 0) setText('txtDocNo', str(d.docNo));
            applyData(d);
            HIST_ITEMS = d.historyItems || []; bind('CmbItemHistory', HIST_ITEMS, 'Id', 'Name', [], true);
            setText('FromDateHistory', F.daysAgo(S.days > 0 ? S.days : 3)); setText('ToDateHistory', F.today());
            setText('txtReportDate', F.today()); setText('txtConfirmationDate', F.today());
            detRender();
            $id('psiFooterInfo').textContent = 'PreShipmentInspection';
            focus('CmbInspectionStatus');
        }).catch(function () { box('Error occurred during database call.'); });
    }
    /** btnRefresh_Click. */
    function refresh(btn) { return busy(btn, function () { return getJson(API + '/refresh').then(applyData).catch(function (e) { box(e.message); }); }); }

    // ------------------------------------------------------------------------------ detail

    /** CmbCustomer_Leave -> ContractNoBind(ContractNoDbCall(CustomerId)). */
    function customerLeave() {
        return getJson(API + '/contracts?customerId=' + netI(val('CmbCustomer'))).then(function (rows) { CONTRACTS = rows || []; bind('cmbContractNo', CONTRACTS, 'Id', 'Name', [], true); if (!CONTRACTS.length) { $id('cmbContractNo').innerHTML = ''; F.refreshCombos(); } }).catch(function (e) { box(e.message); });
    }
    /** DetailFormValidation(). */
    function detailValidation() {
        if (!F.hasSel('CmbCustomer')) { box('Customer field is required'); focus('CmbCustomer'); return false; }
        if (!F.hasSel('cmbContractNo')) { box('Contract No field is required'); focus('cmbContractNo'); return false; }
        if (!val('txtMtons').trim() || netD(val('txtMtons')) === 0) { box('M.Tons field is required'); focus('txtMtons'); return false; }
        if (!val('txtSubLot').trim()) { box('Sub Lot field is required'); focus('txtSubLot'); return false; }
        if (!F.hasSel('CmbStatusDetail')) { box('Status field is required'); focus('CmbStatusDetail'); return false; }
        return true;
    }
    function rowFromFields(r) {
        r.CustomerId = netI(val('CmbCustomer')); r.Customer = F.selText('CmbCustomer');
        r.ContractId = netI(val('cmbContractNo')); r.ContractNo = F.selText('cmbContractNo');
        r.MTons = netD(val('txtMtons')); r.SubLot = val('txtSubLot'); r.Status = F.selText('CmbStatusDetail'); r.Remarks = val('txtDetailRemarks');
        return r;
    }
    /** btnAdd_Click. */
    function addRow() {
        if (!detailValidation()) return;
        DET.rows.push(rowFromFields({ Id: 0 }));
        detRender(); resetDetail(); mtonInHeader();
    }
    /** btnUpdateDetail_Click. */
    function updateRow() {
        if (S.updateIndex === -1) { box('Please select a record to update.'); return; }
        if (!detailValidation()) return;
        rowFromFields(DET.rows[S.updateIndex]);
        detRender(); resetDetail(); mtonInHeader();
    }
    /** grd_DoubleClick. */
    function editRow(i) {
        var r = DET.rows[i]; if (!r) return;
        S.updateIndex = i;
        setVal('CmbCustomer', r.CustomerId);
        customerLeave().then(function () { F.setValOrAdd('cmbContractNo', r.ContractId, r.ContractNo); });
        setText('txtMtons', F.fmt(r.MTons)); setText('txtSubLot', r.SubLot);
        var s = $id('CmbStatusDetail'); Array.prototype.forEach.call(s.options, function (o) { if (o.textContent === r.Status) s.value = o.value; }); F.refreshCombos();
        setText('txtDetailRemarks', r.Remarks);
        show('btnAdd', false); show('btnUpdateDetail', true); show('btnCancelDetial', true);
        focus('CmbCustomer');
    }
    /** grd "X" / Ctrl+Space -> DeleteDetailRow: "Please Reset the Detail" while a row is being edited. */
    function deleteRow(i) {
        if (S.updateIndex !== -1) { box('Please Reset the Detail'); return; }
        DET.rows.splice(i, 1); detRender(); mtonInHeader();
    }
    function cancelRow() { resetDetail(); }
    /** ResetDetail(). */
    function resetDetail() {
        setVal('CmbCustomer', '0'); $id('cmbContractNo').innerHTML = ''; CONTRACTS = []; F.refreshCombos();
        setText('txtMtons', ''); setText('txtSubLot', ''); setText('txtDetailRemarks', '');
        show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelDetial', false);
        S.updateIndex = -1;
        focus('CmbCustomer');
    }
    /** InstertMtonInHeader: Total M.Tons = sum of the grid ("#,##0.###"). */
    function mtonInHeader() { setText('txtHeaderMtons', F.fmt(DET.rows.reduce(function (a, r) { return a + netD(r.MTons); }, 0))); }
    var DCOLS = [{ key: 'Customer' }, { key: 'ContractNo' }, { key: 'MTons', fmt: 'num3', sum: true }, { key: 'Status' }, { key: 'SubLot' }, { key: 'Remarks' }];
    function detRender() {
        F.drawGrid('detBody', 'detFoot', DET.rows, DCOLS, DET.cur, function (r, i) { return '<td class="win-cell-btn"><button type="button" class="win-x" data-del="' + i + '" title="Delete">X</button></td>'; });
    }

    // ------------------------------------------------------------------------------ header

    /** FormValidation(). */
    function formValidation() {
        var dn = val('txtDocNo').trim();
        if (dn === '' || dn === '0') { box('Doc No field is required'); focus('txtDocNo'); return false; }
        if (!F.hasSel('CmbInspectionStatus')) { box('Inspection Status field is required'); focus('CmbInspectionStatus'); return false; }
        if (!F.hasSel('CmbJobLot')) { box('JobLot field is required'); focus('CmbJobLot'); return false; }
        if (!F.hasSel('CmbItemName')) { box('Item Name field is required'); focus('CmbItemName'); return false; }
        if (!F.hasSel('CmbReportStatus')) { box('Report Status field is required'); focus('CmbReportStatus'); return false; }
        return true;
    }
    /** Insert(). */
    function insert(btn) {
        return busy(btn, function () {
            if (!formValidation()) return Promise.resolve();
            if (!ask(S.recId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return Promise.resolve();
            return postJson(API + '/save', {
                recId: S.recId, docNo: val('txtDocNo').trim(), reportDate: val('txtReportDate'), inspectionStatusId: netI(val('CmbInspectionStatus')),
                jobLotId: netI(val('CmbJobLot')), jobLotText: F.selText('CmbJobLot'), itemId: netI(val('CmbItemName')), itemText: F.selText('CmbItemName'),
                headerMtons: netD(val('txtHeaderMtons')), reportStatusId: netI(val('CmbReportStatus')), reportStatusText: F.selText('CmbReportStatus'),
                confirmationDate: val('txtConfirmationDate'), inspectionRemarks: val('txtInspectionRemarks'), requiredAnalysis: val('txtRequiredAnalysis'),
                attachmentsValues: S.attachmentsValues, customAttachmentsValues: S.customAttachmentsValues, details: DET.rows
            }).then(function (d) {
                box((d && d.message) || (S.recId === 0 ? 'Record Saved Successfully' : 'Record Update Successfully'));
                return reset();
            }).catch(function (e) { box(e.message); });
        });
    }
    function save(btn) { S.recId = 0; return insert(btn); }
    function update(btn) { if (S.recId === 0) { box('RecId not Found...'); return Promise.resolve(); } return insert(btn); }
    /** ReadById(ID). */
    function readById(id) {
        return reset().then(function () { return getJson(API + '/by-id?id=' + encodeURIComponent(id)); }).then(function (r) {
            S.recId = netI(r.Id);
            tab('tabPage1');
            show('btnsave', false); show('btnUpdate', true);
            setText('txtDocNo', str(r.ReportDocNo)); setText('txtReportDate', r.ReportDate || F.today());
            F.setValOrAdd('CmbInspectionStatus', r.InspectedByLabId, ''); F.setValOrAdd('CmbJobLot', r.JobLotId, ''); F.setValOrAdd('CmbItemName', r.ItemId, '');
            setText('txtHeaderMtons', F.fmt(r.QtyKgs));
            var s = $id('CmbReportStatus'); s.value = '0'; Array.prototype.forEach.call(s.options, function (o) { if (o.textContent === str(r.ReportStatus)) s.value = o.value; }); F.refreshCombos();
            setText('txtConfirmationDate', r.ConfirmationDate || F.today());
            setText('txtInspectionRemarks', r.InspectionRemarks); setText('txtRequiredAnalysis', r.RequiredAnalysis);
            S.attachmentsValues = str(r.AttachmentsValues); S.customAttachmentsValues = str(r.CustomAttachmentsValues);
            DET.rows = r.details || []; DET.cur = -1; detRender();
            focus('CmbInspectionStatus');
        }).catch(function (e) { box(e.message); });
    }
    /** Reset(). */
    function reset() {
        S.recId = 0; S.attachmentsValues = ''; S.customAttachmentsValues = '';
        show('btnsave', true); show('btnUpdate', false);
        setVal('CmbItemName', '0'); setText('txtHeaderMtons', ''); setVal('CmbInspectionStatus', '0'); setVal('CmbJobLot', '0');
        setText('txtInspectionRemarks', ''); setText('txtRequiredAnalysis', '');
        resetDetail();
        DET.rows = []; DET.cur = -1; detRender();
        setVal('CmbReportStatus', '3');
        focus('CmbInspectionStatus');
        return getJson(API + '/doc-no').then(function (d) { setText('txtDocNo', str(d.docNo)); }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ history

    var HCOLS = [{ key: 'ReportNo', fmt: 'int' }, { key: 'ReportDate', fmt: 'date' }, { key: 'InspectedByName' }, { key: 'JobLot' }, { key: 'ItemName' },
        { key: 'MTons', fmt: 'num3', sum: true }, { key: 'ConfirmationDate', fmt: 'date' }, { key: 'ReportStatus' }, { key: 'InspectionRemarks' }, { key: 'RequiredAnalysis' },
        { key: 'NoOfAttachments', fmt: 'int', link: true }];
    function histRender() {
        F.drawGrid('histBody', 'histFoot', HIST.rows, HCOLS, HIST.cur, function (r, i) {
            return '<td class="win-cell-btn"><button type="button" class="win-edit" data-btn="Print" data-i="' + i + '"' + (PERM.Print ? '' : ' disabled') + '>Print</button></td>' +
                '<td class="win-cell-btn"><button type="button" class="win-edit" data-btn="Edit" data-i="' + i + '"' + (PERM.Update ? '' : ' disabled') + '>Edit</button></td>';
        }, { leadCols: 2 });
        show('histEmpty', HIST.rows.length === 0);
    }
    function historyShow(btn) {
        return busy(btn, function () {
            return postJson(API + '/history', {
                dateBy: (document.querySelector('input[name="dateBy"]:checked') || {}).value || 'doc',
                fromChecked: F.checked('FromDateHistoryChk'), toChecked: F.checked('ToDateHistoryChk'), fromDate: val('FromDateHistory'), toDate: val('ToDateHistory'),
                fromDocNo: netI(val('txtFromDocNoHistory')), toDocNo: netI(val('txtToDocNoHistory')), itemId: netI(val('CmbItemHistory'))
            }).then(function (rows) { HIST.rows = rows || []; HIST.cur = -1; histRender(); F.drawGrid('hdetBody', 'hdetFoot', [], DCOLS, -1, null); }).catch(function (e) { box(e.message); });
        });
    }
    /** DataGridHistory_SelectionChanged. */
    function historySelect(i) {
        HIST.cur = i; var r = HIST.rows[i]; if (!r) return;
        getJson(API + '/details?id=' + r.Id).then(function (rows) { F.drawGrid('hdetBody', 'hdetFoot', rows || [], DCOLS, -1, null); }).catch(function (e) { box(e.message); });
    }
    function historyReset() {
        setText('FromDateHistory', F.today()); setText('ToDateHistory', F.today()); setText('txtFromDocNoHistory', ''); setText('txtToDocNoHistory', '');
        var s = $id('CmbItemHistory'); if (s) s.selectedIndex = -1; F.refreshCombos();
        $id('drdocdate').checked = true;
        HIST.rows = []; HIST.cur = -1; histRender(); F.drawGrid('hdetBody', 'hdetFoot', [], DCOLS, -1, null);
    }
    function historyRefresh(btn) { return busy(btn, function () { return getJson(API + '/history-items').then(function (rows) { HIST_ITEMS = rows || []; bind('CmbItemHistory', HIST_ITEMS, 'Id', 'Name', [], true); }).catch(function (e) { box(e.message); }); }); }
    /** btnPrint_Click -> InvLabPreProductionExportLotInspection_Slip(RecId): 513-invLabPreProductionSlip.rpt ("No Record Found For Display" when 0). */
    function print() { if (!PERM.Print) return; if (S.recId === 0) { box('No Record Found For Display'); return; } F.printSeeded('513-invlabpreproductionslip', { id: S.recId }); }
    function printRow(i) { var r = HIST.rows[i]; if (r) F.printSeeded('513-invlabpreproductionslip', { id: r.Id }); }
    function attachment() { box('Attachments (DMS popup) are not part of the web port.'); }
    function lookup() { box('ExImLookups (LookupId 3, Inspection Status) is a separate definition screen; refresh after defining.'); }
    function defineJobLot() { var w = global.open('/master-data/define-job-lot', '_blank'); var t = setInterval(function () { if (!w || w.closed) { clearInterval(t); refresh(null); } }, 800); }
    function shortcuts() {
        F.shortcutPopup([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
            ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+L', 'For Load All Records'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On Debit Account in Detail Box'],
            ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }
    function tab(id) { F.innerTab('main', id, 'btnPsiFooterHistory', function (p, onHist) { focus(onHist ? 'FromDateHistory' : 'txtReportDate'); }); }
    function toggleHistory() { tab(F.activeInner('main') === 'tabHistory' ? 'tabPage1' : 'tabHistory'); }

    document.addEventListener('DOMContentLoaded', function () {
        F.wireTabs(function (p, onHist) { focus(onHist ? 'FromDateHistory' : 'txtReportDate'); }, 'btnPsiFooterHistory');
        F.on('CmbCustomer', 'change', customerLeave);
        F.wireGrid('detBody', { select: function (i) { DET.cur = i; }, open: editRow, del: deleteRow });
        F.wireGrid('histBody', {
            select: historySelect,
            open: function (i) { readById(HIST.rows[i].Id); },
            btn: function (name, i) { if (name === 'Edit') readById(HIST.rows[i].Id); else if (name === 'Print') printRow(i); },
            link: function (k, i) { box('Attachments of record ' + HIST.rows[i].Id + ': ' + netI(HIST.rows[i].NoOfAttachments)); }
        });
        /* PreShipmentInspection_KeyDown / grd_KeyDown / DataGridHistory_KeyDown */
        document.addEventListener('keydown', function (e) {
            if (F.enterMoves(e)) return;
            var k = (e.key || '').toLowerCase(), onForm = F.activeInner('main') !== 'tabHistory';
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); F.cancel(); return; }
            if (e.ctrlKey && k === 'p' && PERM.Print) { e.preventDefault(); print(); }
            if (e.ctrlKey && k === 's') { e.preventDefault(); if (onForm) { if (!$id('btnsave').classList.contains('is-hidden')) save($id('btnsave')); } else historyShow($id('btnShow')); }
            if (e.ctrlKey && k === 'u' && !$id('btnUpdate').classList.contains('is-hidden') && S.recId > 0) { e.preventDefault(); update($id('btnUpdate')); }
            if (e.ctrlKey && e.key === 'F5' && onForm) { e.preventDefault(); focus('txtReportDate'); }
            if (e.ctrlKey && e.key === 'F10' && onForm) { e.preventDefault(); attachment(); }
            if (e.ctrlKey && k === 'n' && onForm) { e.preventDefault(); reset(); }
            if (e.ctrlKey && k === 'r' && onForm) { e.preventDefault(); refresh($id('btnRefresh')); }
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); }
            if (e.ctrlKey && e.key === 'ArrowUp' && onForm) { e.preventDefault(); focus('txtSubLot'); }
            if (e.ctrlKey && e.key === 'ArrowDown' && onForm) { e.preventDefault(); var tr = $id('detBody').querySelector('tr'); if (tr) tr.scrollIntoView(); }
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); }
            if (onForm && e.ctrlKey && e.key === 'Enter' && DET.cur >= 0) { e.preventDefault(); editRow(DET.cur); }
            if (onForm && e.ctrlKey && e.key === ' ' && DET.cur >= 0) { e.preventDefault(); deleteRow(DET.cur); }
            if (!onForm && e.ctrlKey && e.key === 'Enter' && HIST.cur >= 0 && PERM.Update) { e.preventDefault(); readById(HIST.rows[HIST.cur].Id); }
        });
        load();
    });

    global.ExportPSI = { reset: reset, refresh: refresh, save: save, update: update, attachment: attachment, print: print, lookup: lookup, shortcuts: shortcuts,
        defineJobLot: defineJobLot, addRow: addRow, updateRow: updateRow, cancelRow: cancelRow, historyReset: historyReset, historyRefresh: historyRefresh,
        historyShow: historyShow, toggleHistory: toggleHistory };
}(window));
