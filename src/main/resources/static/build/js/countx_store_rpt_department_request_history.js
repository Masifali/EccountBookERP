/* ============================================================================================
 * 455 "Department Request History" - DepartmentRequestHistory.cs (ScreenName DepartmentRequestHistory)
 * API /api/store/reports/department-request-history. Rules and notes: StoreReportsBService.
 * ============================================================================================ */
(function () {
    'use strict';

    var C = window.StoreCommon, K = window.StoreRptB, $id = C.$id;
    var API = '/api/store/reports/department-request-history';
    var dt = [];                   /* dt - the procedure rows of the last Show; 450-Register prints them */
    var yearStart = null;

    function val(id) { return $id(id).value; }
    function num(id) { return C.intOf(val(id)); }

    /* frmGatePassReport_Load:198 */
    function load() {
        C.getJson(API + '/lookups').then(function (d) {
            yearStart = d.fromDate;
            if (yearStart) $id('FromDate').value = yearStart;              /* :203 */
            $id('ToDate').value = d.toDate || C.today();                    /* :204 */
            fillDropDowns(d);                                               /* :205 */
            C.fillSelect('CmbApproved', d.approved, 'Name', 'Name', false); /* :172 - Text is what GridFill reads */
            $id('CmbApproved').value = d.approvedDefault;                   /* :173 Rows[1].Activate() */
            K.guardDate('FromDate'); K.guardDate('ToDate');                 /* a DateTimePicker is never empty */
            $id('FromDate').focus();
        }).catch(function (e) { alert(e.message); });
    }

    /* FillAllDropDowns:103 - nothing returned leaves the combos as they are (:116); only the
       Activity groups present in the answer are re-bound (:138-150), so an absent group (an empty
       list here) leaves its combo as it was. BindDDL ZeroIndex false - no default row. */
    function fillDropDowns(d) {
        if (d.empty) return;
        [['CmbDepartmentName', d.departments], ['CmbItemName', d.items], ['CmbAssetName', d.assets],
         ['cmbItemCondition', d.itemConditions]].forEach(function (x) {
            if (x[1] && x[1].length) C.fillSelect(x[0], x[1], 'Id', 'Name');
        });
    }

    /* btnRefresh_Click:407 */
    function refresh() {
        return C.getJson(API + '/refresh').then(fillDropDowns).catch(function (e) { alert(e.message); });
    }

    /* Reset:182 - reachable on the desktop only through the form's Ctrl+N (toolStripButton1_Click),
       and neither fires: the New button has no Click handler and KeyPreview is off. Kept for
       reference, not wired to the page (desktop-behaviour note in StoreReportsBService). */
    function reset() {
        if (yearStart) $id('FromDate').value = yearStart;
        $id('ToDate').value = C.today();
        $id('CmbDepartmentName').value = '0';
        K.clear('grdfrm');
        $id('FromDate').focus();
    }

    /* GridFill:214 */
    function show() {
        var q = C.qs({
            fromDate: val('FromDate'), toDate: val('ToDate'),
            departmentId: num('CmbDepartmentName'), itemId: num('CmbItemName'), assetId: num('CmbAssetName'),
            approved: val('CmbApproved')
        });
        return C.getJson(API + q).then(function (d) {
            dt = d.raw || [];
            var rows = d.rows || [];
            if (!rows.length) {                                             /* :269-270 */
                K.clear('grdfrm');
                alert(d.message || 'Record Not found For Display');
                return;
            }
            K.render('grdfrm', columns(), rows, { frozen: 1 });
        }).catch(function (e) { alert(e.message); });
    }

    /* RetrieveStructure of the :243-259 table + gridSetting:278 (GridWrappingAndColumnSettings(grdfrm, 2, 3, hidden)) */
    function columns() {
        var intCol = function (k) { return { key: k, caption: K.caption(k), align: 'center' }; };
        var txt = function (k) { return { key: k, caption: K.caption(k) }; };
        var q = function (v) { return K.fmt(v, '#,##0.###'); };
        return [
            { caption: 'Print', button: 'Print', onButton: printSlip },                             /* :300 AddButton position 0 */
            { key: 'DocDate', caption: K.caption('DocDate'), format: K.dMMMyyyy },                  /* :283 dd-MMM-yyyy */
            intCol('DocNo'),
            txt('DepartmentNameFrom'), txt('ToDepartmentName'),
            { key: 'RequestedQty', caption: K.caption('RequestedQty'), align: 'right', agg: 'sum', format: q, total: q },
            txt('ItemName'), txt('UOMCode'), txt('ItemCondition'), txt('AssetName'),
            { key: 'EntryDate', caption: K.caption('EntryDate'), format: K.dMMMyyyyhm },            /* :284 */
            txt('EntryUser'),
            { key: 'ModifyDate', caption: K.caption('ModifyDate'), format: K.dMMMyyyyhm },          /* :285 */
            txt('ModifyUser'),
            intCol('NoOfAttachments')
        ];
    }

    /* grdfrm_ColumnButtonClick:363 -> CommonServices.DepartmentSlip451 (451-RptDepartmentRequestSlip.rpt) */
    function printSlip(row) {
        return C.getJson(API + '/' + C.intOf(C.ci(row, 'Id')) + '/slip').then(function (rows) {
            if (!K.printRows(rows, '451-RptDepartmentRequestSlip')) alert('No Record Found For Display');
        }).catch(function (e) { alert(e.message); });
    }

    /* btnPrint_Click:340 (450-RptDepartmentRequestRegister.rpt) */
    function printRegister() {
        if (!dt.length) { alert('Record Not Found For Display'); return; }
        K.printRows(dt, '450-RptDepartmentRequestRegister');
    }

    /* Footer History button (web): the history grid is below the filters, as on the desktop. */
    function gotoHistory() { $id('historySection').scrollIntoView({ behavior: 'smooth', block: 'start' }); }

    window.RptDR = { show: show, reset: reset, refresh: refresh, printRegister: printRegister, gotoHistory: gotoHistory };
    document.addEventListener('DOMContentLoaded', load);
})();
