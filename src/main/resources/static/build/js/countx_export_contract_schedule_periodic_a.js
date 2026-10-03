/* ============================================================================================
 * countx_export_contract_schedule_periodic_a.js - frmExportContractSchedulePeriodic.cs, screen 237 "5002 Contract Schedule Report"
 * (Export Contract Scheduling Reports). Data: /api/export/contract-schedule-periodic-a
 * (USp_ExImLcOrderShipmentSchedule_GetAllMethod 'GetLatestAttentiveLoadDate', Usp_ExportContractSchedulePeriodicA - 5 tables).
 * The desktop print button is hidden and has no handler: nothing to print.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptA, N = global.ExportRptN, $id = H.$id, box = H.box;
    var API = '/api/export/contract-schedule-periodic-a';
    var S = { months: [], last: null, suspend: false };
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    /* Fcl / MTons: FormatString "0,0", TotalFormatString "#,##0.##", Σ on both. */
    var CELL = N.cellTotal(N.fmt00, function (v) { return N.fmtUpTo(v, 2); });
    function three(key, caption, width) {
        return [{ key: key, caption: caption || key, width: width }, { key: 'Fcl', caption: 'Fcl', num: true, sum: true, fmt: CELL, width: 50 },
            { key: 'MTons', caption: 'MTons', num: true, sum: true, fmt: CELL, width: 70 }];
    }
    var GRIDS = [['byDate', 'grdByDate', three('Date', 'Date', 170)], ['byParty', 'grdByParty', three('Customer', 'Customer', 155)],
        ['byPorts', 'grdByPorts', three('Port', 'Port', 180)], ['byItem', 'grdByItem', three('Item', 'Item', 220)], ['byMonths', 'grdByMonths', three('Month', 'Month')]];

    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function ymd(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function monthText(d) { return MON[d.getMonth()] + ' ' + d.getFullYear(); }
    function parseIso(s) {
        var m = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2})(?::(\d{2}))?)?/.exec(H.str(s));
        return m ? new Date(+m[1], +m[2] - 1, +m[3], +(m[4] || 0), +(m[5] || 0), +(m[6] || 0)) : null;
    }
    /* DateTime.ParseExact(text, "MMM yyyy") - null when it would throw. */
    function parseMonth(t) {
        var m = /^([A-Z][a-z]{2}) (\d{4})$/.exec(H.str(t).trim());
        if (!m) return null;
        var i = MON.indexOf(m[1]);
        return i < 0 ? null : new Date(+m[2], i, 1);
    }
    function monthListed(t) { return S.months.some(function (r) { return r.name === t; }); }
    function setMonth(id, t) { if (monthListed(t)) H.setVal(id, t); }

    /**
     * Load / btnNew_Click: FromMonth = ToMonth = Now; Last > Now -> tztAddDays = (Last - Now).Days (its TextChanged moves To Date
     * to From Date + days); otherwise From Date = Last - 7 and FromMonth = Last; then To Date = Last, ToMonth = Last; each month
     * combo takes its "MMM yyyy" text only when the month is listed; focus From Month.
     */
    function applyLastDate() {
        var now = new Date(), last = S.last || now, fromMonth = now;
        if (last > now) {
            H.setText('tztAddDays', String(Math.floor((last - now) / 86400000)));
            addDaysChanged();
        } else {
            fromMonth = last;
            H.setText('txtDateFrom', ymd(new Date(last.getFullYear(), last.getMonth(), last.getDate() - 7)));
        }
        H.setText('txtToDate', ymd(last));
        if (S.months.length) { setMonth('cmbFromMonth', monthText(fromMonth)); setMonth('CmbToMonth', monthText(last)); }
        H.focus('cmbFromMonth');
    }
    function load() {
        H.setText('txtDateFrom', H.today());
        H.setText('txtToDate', H.today());
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            S.months = d.months || [];
            if (S.months.length) { H.bind('cmbFromMonth', S.months, 'Id', 'name'); H.bind('CmbToMonth', S.months, 'Id', 'name'); }
            if (d.lastScheduleLoadingDateError) { box(d.lastScheduleLoadingDateError); return; }
            S.last = parseIso(d.lastScheduleLoadingDate) || new Date();
            applyLastDate();
        }).catch(function (e) { box(e.message); });
    }
    /** FillAllGrids: each table bound when it has rows, otherwise the grid is emptied. */
    function fill(d) {
        GRIDS.forEach(function (g) {
            var rows = (d && d[g[0]]) || [];
            if (rows.length) H.drawGrid(g[1], g[2], rows, {});
            else { $id(g[1]).querySelector('thead').innerHTML = ''; H.clearGrid(g[1]); }
        });
    }
    /** btnShow_Click -> ShowDataByDates (From / To date pickers). */
    function showByDates(btn) {
        return H.busy(btn || 'BtnShowbyDates', function () {
            return H.postJson(API + '/show', { mode: 'dates', fromDate: H.val('txtDateFrom'), toDate: H.val('txtToDate') }).then(fill).catch(function (e) { box(e.message); });
        });
    }
    /** btnShowByMonths_Click -> ShowDataByMonths (first day of From Month .. last day of To Month). */
    function showByMonths(btn) {
        return H.busy(btn || 'btnShowByMonths', function () {
            return H.postJson(API + '/show', { mode: 'months', fromMonth: H.selText('cmbFromMonth'), toMonth: H.selText('CmbToMonth') }).then(fill).catch(function (e) { box(e.message); });
        });
    }
    /** tztAddDays_TextChanged: To Date = From Date + Conversion.ToInt(text). */
    function addDaysChanged() {
        if (S.suspend) return;
        var f = parseIso(H.val('txtDateFrom')); if (!f) return;
        f.setDate(f.getDate() + H.netI(H.val('tztAddDays')));
        H.setText('txtToDate', ymd(f));
    }
    /** txtAddMonths_TextChanged: To Month = From Month + months when listed, else "Selected month does not exist in the List.". */
    function addMonthsChanged() {
        var f = parseMonth(H.selText('cmbFromMonth'));
        if (!f) { box('String was not recognized as a valid DateTime.'); return; }
        var t = monthText(new Date(f.getFullYear(), f.getMonth() + H.netI(H.val('txtAddMonths')), 1));
        if (monthListed(t)) H.setVal('CmbToMonth', t);
        else box('Selected month does not exist in the List.');
    }
    function shortcuts() {
        H.shortcuts([['Ctrl+S', 'For Focus On From Date combo in First Filter Box'], ['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+F5', 'For Focus on Combo From Date'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus On From Date combo in First Filter Box'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }
    document.addEventListener('DOMContentLoaded', function () {
        N.embedFooter('scaFooter');
        H.fullscreenButtons();
        GRIDS.forEach(function (g) { H.gridEvents(g[1], {}); });
        ['tztAddDays', 'txtAddMonths'].forEach(function (id) {   /* KeyPress = OnlytextNumberFunction */
            $id(id).addEventListener('keypress', function (e) { if (e.key && e.key.length === 1 && !/\d/.test(e.key)) e.preventDefault(); });
        });
        $id('tztAddDays').addEventListener('input', addDaysChanged);
        $id('txtAddMonths').addEventListener('input', addMonthsChanged);
        document.addEventListener('keydown', function (e) {
            if (H.baseKeys(e, shortcuts)) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 's') { e.preventDefault(); showByMonths(); }
            else if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); H.focus('cmbFromMonth'); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); applyLastDate(); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grdByMonths'); }
        });
        load();
    });
    global.ExportSca = { btnNew: applyLastDate, showByDates: showByDates, showByMonths: showByMonths, shortcuts: shortcuts };
}(window));
