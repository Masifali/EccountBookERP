/* ============================================================================================
 * countx_export_contract_schedule_periodic.js - frmExportContractSchedulePeriodicB.cs, screen 236
 * "Export Contract Scheduling Reports". Data: /api/export/contract-schedule-periodic
 * (Usp_ExportContractSchedulePeriodicB, 4 tables). Prints 551_02 / 551_03 through the seeded CrystalPrint
 * keys "551-02-exportcontractscheduleloadingdateitemwise-slip" / "551-03-...customeritemwise-slip".
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptA, $id = H.$id, box = H.box;
    var API = '/api/export/contract-schedule-periodic';
    var S = { last: '', t1: [], t2: [], periodic: [], sortNo: 0, suspend: false };

    var COLS1 = [ /* dtLoadingDateItem: GroupTitle, ItemId, LoadingDate hidden; grouped by LoadingDate */
        { key: 'GroupTitle', hidden: true }, { key: 'ItemId', hidden: true }, { key: 'LoadingDate', hidden: true },
        { key: 'ItemName', caption: 'Item Name', width: 380 }, { key: 'Fcl', caption: 'Fcl', num: true, sum: true, fmt: 'o2' }, { key: 'MTons', caption: 'M Tons', num: true, sum: true, fmt: 'o3' }
    ];
    function refCls(v) { return H.str(v) === 'Referred' ? 'dkgreen' : 'red'; }
    var COLS2 = [ /* dtLoadingDateItemCustomer; grouped by GroupCaption */
        { key: 'GroupTitle', hidden: true }, { key: 'ScheduleId', hidden: true }, { key: 'ExImLcOrderId', hidden: true }, { key: 'LoadingDate', hidden: true },
        { key: 'ScheduleCode', caption: 'Schedule Code', link: true }, { key: 'SupplierCustomerId', hidden: true }, { key: 'Customer', caption: 'Customer' },
        { key: 'CustomerContractNo', caption: 'Customer Contract No' }, { key: 'ItemId', hidden: true }, { key: 'ItemName', caption: 'Item Name', width: 275 },
        { key: 'PackSize', caption: 'Pack Size' }, { key: 'NoOfBags', caption: 'No Of Bags', num: true, sum: true, fmt: 'o3' },
        { key: 'MTons', caption: 'M Tons', num: true, sum: true, fmt: 'o3' }, { key: 'Fcl', caption: 'Fcl', num: true, sum: true, fmt: 'o2' },
        { key: 'ReferredStatus', caption: 'Referred Status', cls: refCls }, { key: 'DestinationPort', caption: 'Destination Port' },
        { key: 'NoOfAttachments', caption: 'No Of Attachments', num: true, link: true }, { key: 'GroupCaption', hidden: true }
    ];
    var COLS3 = [{ key: 'SortNo', hidden: true }, { key: 'PeriodDescription', caption: 'Period Description', width: 325 },
        { key: 'FCL', caption: 'FCL', num: true, sum: true, fmt: 'n2' }, { key: 'MTons', caption: 'M.Tons', num: true, sum: true, fmt: 'n2' }];

    /** Load: From = today, Last = GetLatestAttentiveLoadDate; Last > today -> Add Days = days, else From = Last; To = Last. */
    function applyLastDate() {
        var today = H.today();
        S.suspend = true;
        H.setText('txtDateFrom', today);
        if (S.last && S.last > today) {
            var d = Math.floor((new Date(S.last) - new Date(today)) / 86400000);
            H.setText('tztAddDays', String(d));
        } else if (S.last) {
            H.setText('txtDateFrom', S.last);
        }
        H.setText('txtToDate', S.last || today);
        S.suspend = false;
    }
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.lastScheduleLoadingDateError) box(d.lastScheduleLoadingDateError);
            S.last = d.lastScheduleLoadingDate || H.today();
            applyLastDate();
            $id('txtDateFromChk').checked = false;
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    /** ShowDataByDates(SortNo) -> FillAllGrids. */
    function showData(sortNo) {
        S.sortNo = sortNo || 0;
        return H.postJson(API + '/show', {
            perFclTon: H.val('txtprcntfclton'), fromChecked: H.checked('txtDateFromChk'), fromDate: H.val('txtDateFrom'),
            toChecked: H.checked('txtToDateChk'), toDate: H.val('txtToDate'), intervalDays: H.netI(H.val('txtIntervalDays')),
            notReferred: H.checked('chkNotReferredStatus'), sortNo: S.sortNo
        }).then(function (d) {
            d = d || {};
            H.setText('txtprcntfclton', d.perFclTon || '24');
            S.t1 = d.loadingDateItem || []; S.t2 = d.loadingDateItemCustomer || [];
            H.drawGrid('grdLoadingDateItem', COLS1, S.t1, { group: 'LoadingDate' });
            H.show('grdLoadingDateItemEmpty', !S.t1.length);
            H.drawGrid('grdLoadingDateItemCustomer', COLS2, S.t2, { group: 'GroupCaption' });
            H.show('grdLoadingDateItemCustomerEmpty', !S.t2.length);
            if (d.periodic) { S.periodic = d.periodic; H.drawGrid('grdDueWise', COLS3, S.periodic, {}); }
        }).catch(function (e) { box(e.message); });
    }
    function show(btn) { return H.busy(btn, function () { return showData(0); }); }
    /** btnNew_Click: the Last-date arithmetic again, grids cleared. */
    function btnNew() {
        applyLastDate();
        S.t1 = []; S.t2 = []; S.periodic = [];
        ['grdLoadingDateItem', 'grdLoadingDateItemCustomer', 'grdDueWise'].forEach(H.clearGrid);
        H.show('grdLoadingDateItemEmpty', false); H.show('grdLoadingDateItemCustomerEmpty', false);
    }
    /** tztAddDays_TextChanged: To = From + days. */
    function addDaysChanged() { if (S.suspend) return; H.setText('txtToDate', H.addDays(H.val('txtDateFrom'), H.netI(H.val('tztAddDays')))); }
    function printArgs() {
        return {
            branchesId: 0, fromDate: H.checked('txtDateFromChk') ? H.val('txtDateFrom') : '', toDate: H.checked('txtToDateChk') ? H.val('txtToDate') : '',
            netWeight: H.netI(H.val('txtprcntfclton') || '24'), intervalDays: H.netI(H.val('txtIntervalDays')), sortNo: S.sortNo,
            statusId: H.checked('chkNotReferredStatus') ? 1 : 0
        };
    }
    /** btnPrint02_Click / btnPrint03_Click: "Not Record Found For Display" while the table is empty (dtXxxFromDb starts as an empty DataTable). */
    function print02(btn) { if (!S.t1.length) { box('Not Record Found For Display'); return; } var a = printArgs(); a.flagForReport = 2; return H.print('551-02-exportcontractscheduleloadingdateitemwise-slip', a, btn); }
    function print03(btn) { if (!S.t2.length) { box('Not Record Found For Display'); return; } var a = printArgs(); a.flagForReport = 3; return H.print('551-03-exportcontractscheduleloadingdatecustomeritemwise-slip', a, btn); }
    function shortcuts() {
        H.shortcuts([['Ctrl+S', 'For Focus On From Date combo in First Filter Box'], ['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+F5', 'For Focus on Combo From Date'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus On From Date combo in First Filter Box'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }
    function tab(id) {
        document.querySelectorAll('.win-tabs[data-tabs="sch"] .win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === id); });
        ['tabPage1', 'tabPage2'].forEach(function (p) { $id(p).classList.toggle('is-active', p === id); });
    }
    /** grdLoadingDateItemCustomer_LinkClicked: ScheduleCode -> frmSaleContractSchedule.ReadById (no web page yet); NoOfAttachments -> attachments (not on web). */
    function link(key, i) {
        var r = S.t2[i]; if (!r) return;
        if (key === 'ScheduleCode') box('Sale Contract Schedule ' + H.str(r.ScheduleCode) + ' (contract id ' + H.netI(r.ExImLcOrderId) + ') - the FrmExportSalesContractSchedule form has no web page yet.');
        else if (key === 'NoOfAttachments') box('Attachments of schedule ' + H.netI(r.ScheduleId) + ' (document type 244) - the attachment viewer has no web page yet.');
    }
    document.addEventListener('DOMContentLoaded', function () {
        if (/[?&]embed=1/.test(location.search)) { var f = $id('schFooter'); if (f) f.classList.add('is-hidden'); }
        H.fullscreenButtons();
        $id('tztAddDays').addEventListener('input', addDaysChanged);
        H.gridEvents('grdLoadingDateItemCustomer', { link: link, ctrlSpace: function (i) { link('ScheduleCode', i); } });
        H.gridEvents('grdDueWise', { dblclick: function (i) { var r = S.periodic[i]; if (r) showData(H.netI(r.SortNo)); } });
        document.addEventListener('keydown', function (e) {
            if (H.baseKeys(e, shortcuts)) return;
            var k = (e.key || '').toLowerCase();
            /* the desktop's KeyDown handles only Enter, Ctrl+E/Esc and Ctrl+Alt; the ShortCutKeyPopUp list is documentation there. */
            if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew(); }
            if (e.ctrlKey && (k === 's' || e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); H.focus('txtDateFrom'); }
            if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid($id('tabPage1').classList.contains('is-active') ? 'grdLoadingDateItemCustomer' : 'grdLoadingDateItem'); }
        });
        load();
    });
    global.ExportSch = { btnNew: btnNew, show: show, print02: print02, print03: print03, shortcuts: shortcuts, tab: tab };
}(window));
