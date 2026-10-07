/* ============================================================================================
 * Screen 75 "1008 Payables & Receivable Forecast" - Architecture.WinApp.Account_Reports.DueDateAnalysisPayablesAndReceivablesForcast
 * Page: templates/accounts/reports/acrpt2_due_date_forecast.html (route /accounts/reports/due-date-analysis-forecast).
 * Every desktop handler, in desktop order (DueDateAnalysisPayablesAndReceivablesForcast.cs):
 *   _Load                       -> ctrlGrdBar1: Field Chooser / Save Layout / Remove Layout hidden; fromdate = Now (disabled); txtIntervalDays "8";
 *                                  todate unchecked; fromdate.Focus().
 *   btnshow_Click / FillData(0) -> usp_shorttermDueDateAnalysisPayablesAndReceivablesForcast (FromDate, ToDate only when checked, SortNo, DaysInterval);
 *                                  Tables[0] -> grd (payables negated), cards, the six summary boxes, todate = MaxDueDate;
 *                                  Tables[1] -> grdDueWise (only when SortNo = 0), payables negated.
 *   GridSettings                -> HeaderLines 3, the four running/net captions, widths 130 (AmountForReportConstant + 20) / 73 (dates),
 *                                  stringFormatboth, sums on every numeric column except the three running ones, TotalRow.
 *   grdItemWise_FormattingRow   -> RunningReceivables < 0: red + bold.   grdDueWise_FormattingRow -> CumulativeBalance < 0: red + bold.
 *   grdDueWise_DoubleClick      -> FillData(SortNo of the row): grd + cards + summary boxes only; grdDueWise is left as it is.
 *   BtnNewSummary_Click         -> fromdate = Now - 30 days, todate = Now, fromdate.Focus(), grd cleared, txtNetReceivable / txtOverDueReceivables /
 *                                  txtNotYetPayables = "" (the other boxes keep their text).
 *   btnPrintSumm_Click          -> no rows: "Record Not Found For Display"; else 840_DueDateAnalysisPayablesAndReceivablesForcast.rpt over Tables[0].
 *   btnShortcut_Click / Ctrl+Alt-> ShortCutKeyPopUp (MakeShortCutKeys).     txtIntervalDays_KeyPress -> OnlytextNumberFunction (digits).
 *   _KeyDown                    -> Enter = Tab, Ctrl+S Show, Ctrl+T switches the tab, Ctrl+N New, Ctrl+F5 / Ctrl+Up fromdate, Ctrl+Down grid,
 *                                  Ctrl+E / Esc close, Ctrl+Alt shortcut keys.
 *   grd_KeyDown                 -> Ctrl+Space does nothing (empty body); tabControl1_SelectedIndexChanged, grdItemWise_ColumnButtonClick /
 *                                  LinkClicked are empty on the desktop.
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt2, el = A.el;
    var API = '/api/accounts/acrpt2/forecast';
    var RPT = '840_DueDateAnalysisPayablesAndReceivablesForcast.rpt';
    var look = { amountDecimals: 0 };
    var dtFilteredData = [];          // ds.Tables[0] of the last FillData (what Print hands to the .rpt)
    var seq = 0;
    var SHORT = '#,#;(#,#);0';        // the "#,#;(#,#);0" string the form uses for the cards, boxes and grdDueWise

    function bothFmt() {              // clsGlobalVariables.stringFormatboth
        var z = new Array(look.amountDecimals + 1).join('0');
        return '#,##0.' + z + ';(0,0.' + z + '); 0';
    }
    function negate(v) { var n = 0 - Number(v == null || v === '' ? 0 : v); return n === 0 ? 0 : n; }   // 0.0 - Conversion.ToDouble(...)
    function dbl(v) { var n = Number(v == null || v === '' ? 0 : v); return isFinite(n) ? n : 0; }

    var grd, grdDue;
    function initGrids() {
        grd = A.createGrid({ table: el('grd'), scroller: el('gridMain'), nav: el('navMain'), navText: el('navTextMain'), headerLines: 3,
            totalRow: true, filterRow: true,
            cellStyle: function (c, r, raw) { return c.key === 'RunningReceivables' && dbl(raw) < 0 ? { color: '#ff0000', bold: true } : null; } });
        grdDue = A.createGrid({ table: el('grdDueWise'), scroller: el('gridDue'), nav: el('navDue'), navText: el('navTextDue'), headerLines: 2,
            totalRow: true, filterRow: false,
            cellStyle: function (c, r, raw) { return c.key === 'CumulativeBalance' && dbl(raw) < 0 ? { color: '#ff0000', bold: true } : null; },
            onRowDblClick: function (r) { fillData(A.intArg(A.ci(r, 'SortNo'))); } });
        grd.clear(); grdDue.clear();
    }

    /* GridSettings() */
    function mainColumns() {
        var f = bothFmt(), W = 130;
        function amt(key, caption, sum) { return { key: key, caption: caption || key, width: W, type: 'num', fmt: f, totalFmt: f, sum: !!sum }; }
        return [
            { key: 'DueDate', caption: 'DueDate', width: 73, type: 'date' },
            amt('SaleReceivable', null, true), amt('ExportReceivable', null, true), amt('TotalReceivables', null, true),
            amt('RunningTotalReceivables', 'Cumulative Receivables', false),
            amt('PurchasePayable', null, true), amt('ImportPayable', null, true), amt('TotalPayables', null, true),
            amt('RunningTotalPayables', 'Cumulative Payables', false),
            amt('ReceivablesMinusPayables', 'Net Balance (Receivables - Payables)', true),
            amt('RunningReceivables', 'Cumulative Balance', false)
        ];
    }
    /* grdDueWise settings in FillData */
    function dueColumns() {
        function amt(key, width, sum) { return { key: key, caption: key, width: width, type: 'num', fmt: SHORT, totalFmt: SHORT, sum: !!sum }; }
        return [
            { key: 'SortNo', caption: 'SortNo', hidden: true, width: 60 },
            { key: 'Description', caption: 'Description', width: 325 },
            amt('SaleReceivable', 110, true), amt('ExportReceivable', 110, true), amt('TotalReceivables', 110, true),
            amt('CumulativeReceivables', 110, false),
            amt('PurchasePayable', 110, true), amt('ImportPayable', 110, true), amt('TotalPayables', 110, true),
            amt('CumulativePayables', 110, false),
            amt('NetBalance', 110, true), amt('CumulativeBalance', 130, false)
        ];
    }

    // ------------------------------------------------------------------ cards (DueDateAnalysisReceivableAndPayableCard)
    var CARD_ROWS = [
        ['Sale Receivable :', 'SaleReceivable', 29, 27, 100], ['Export Receivable :', 'ExportReceivable', 47, 45, 114],
        ['Total Receivables :', 'TotalReceivables', 65, 63, 109], ['Cumulative Receivables :', 'RunningTotalReceivables', 83, 81, 145],
        ['Purchase Payable :', 'PurchasePayable', 101, 99, 108], ['Import Payable :', 'ImportPayable', 119, 117, 97],
        ['Total Payables :', 'TotalPayables', 137, 135, 90], ['Cumulative Payables :', 'RunningTotalPayables', 155, 153, 126],
        ['Net Balance (Receivables - Payables) :', 'ReceivablesMinusPayables', 171, 177, 139, 30],
        ['Cumulative Balance:', 'RunningReceivables', 202, 200, 119]
    ];
    function buildCards(rows) {
        var host = el('cards'); host.innerHTML = '';
        rows.forEach(function (r) {
            var c = d.createElement('div'); c.className = 'card';
            var dt = d.createElement('div'); dt.className = 'dt'; dt.textContent = A.fmtDate(A.ci(r, 'DueDate')); c.appendChild(dt);
            CARD_ROWS.forEach(function (cr) {
                var k = d.createElement('span'); k.className = 'k'; k.textContent = cr[0]; k.style.top = cr[2] + 'px'; k.style.width = cr[4] + 'px'; if (cr[5]) k.style.height = cr[5] + 'px';
                var n = d.createElement('span'); n.className = 'n'; n.style.top = cr[3] + 'px';
                var txt = A.netFmt(dbl(A.ci(r, cr[1])), SHORT); n.textContent = txt;
                if (cr[1] === 'RunningReceivables' && txt.indexOf('(') >= 0 && txt.indexOf(')') >= 0) n.classList.add('red');   // card _Load
                c.appendChild(k); c.appendChild(n);
            });
            host.appendChild(c);
        });
    }

    // ------------------------------------------------------------------ date boxes
    function setTo(v) { el('toDate').value = v; el('toChk').checked = true; el('toWrap').classList.remove('off'); }   // setting Value ticks the box

    // ------------------------------------------------------------------ FillData
    function fillData(sortNo) {
        sortNo = sortNo || 0;
        var q = { fromDate: el('fromDate').value || A.today(), intervalDays: A.intArg(el('interval').value), sortNo: sortNo };
        var qs = new URLSearchParams(q);
        if (el('toChk').checked && el('toDate').value) qs.set('toDate', el('toDate').value);
        var token = ++seq;
        return A.getJson(API + '/data?' + qs.toString()).then(function (ds) {
            if (token !== seq) return;
            dtFilteredData = ds.table0 || [];
            var dtDueWise = ds.table1 || [];
            if (dtFilteredData.length) {
                var dtcol = dtFilteredData.map(function (r) {
                    return {
                        DueDate: A.ci(r, 'DueDate'), SaleReceivable: A.ci(r, 'SaleReceivable'), ExportReceivable: A.ci(r, 'ExportReceivable'),
                        TotalReceivables: A.ci(r, 'TotalReceivables'), RunningTotalReceivables: A.ci(r, 'RunningTotalReceivables'),
                        PurchasePayable: negate(A.ci(r, 'PurchasePayable')), ImportPayable: negate(A.ci(r, 'ImportPayable')),
                        TotalPayables: negate(A.ci(r, 'TotalPayables')), RunningTotalPayables: negate(A.ci(r, 'RunningTotalPayables')),
                        ReceivablesMinusPayables: A.ci(r, 'ReceivablesMinusPayables'), RunningReceivables: A.ci(r, 'RunningReceivables')
                    };
                });
                grd.setData(mainColumns(), dtcol);
                buildCards(dtcol);
                var f0 = dtFilteredData[0];
                var mx = A.ci(f0, 'MaxDueDate'); if (mx) setTo(String(mx).slice(0, 10));          // todate.Value = MaxDueDate
                el('vNetRcv').textContent = A.netFmt(dbl(A.ci(f0, 'NetReceivables')), SHORT);
                el('vOverRcv').textContent = A.netFmt(dbl(A.ci(f0, 'ReceivablesOverDue')), SHORT);
                el('vNotRcv').textContent = A.netFmt(-dbl(A.ci(f0, 'ReceivablesNotYetDue')), SHORT);
                el('vNetPay').textContent = A.netFmt(-dbl(A.ci(f0, 'NetPayables')), SHORT);
                el('vOverPay').textContent = A.netFmt(-dbl(A.ci(f0, 'PayablesOverDue')), SHORT);
                el('vNotPay').textContent = A.netFmt(-dbl(A.ci(f0, 'PayablesNotYetDue')), SHORT);
            } else {
                grd.clear(); el('cards').innerHTML = '';                                           // grd.DataSource = null; ItemWiseflowlayout.Controls.Clear()
                ['vNetRcv', 'vOverRcv', 'vNotRcv', 'vNetPay', 'vOverPay', 'vNotPay'].forEach(function (id) { el(id).textContent = '0'; });
            }
            if (dtDueWise.length) {
                if (sortNo !== 0) return;                                                           // grdDueWise stays as it is
                var dt = dtDueWise.map(function (r) {
                    return {
                        SortNo: A.ci(r, 'SortNo'), Description: A.ci(r, 'Description'),
                        SaleReceivable: A.ci(r, 'SaleReceivable'), ExportReceivable: A.ci(r, 'ExportReceivable'),
                        TotalReceivables: A.ci(r, 'TotalReceivables'), CumulativeReceivables: A.ci(r, 'ComulativeReceivables'),
                        PurchasePayable: negate(A.ci(r, 'PurchasePayable')), ImportPayable: negate(A.ci(r, 'ImportPayable')),
                        TotalPayables: negate(A.ci(r, 'TotalPayables')), CumulativePayables: negate(A.ci(r, 'ComulativePayables')),
                        NetBalance: A.ci(r, 'NetBalance'), CumulativeBalance: A.ci(r, 'ComulativeBalance')
                    };
                });
                grdDue.setData(dueColumns(), dt);
            } else grdDue.clear();                                                                  // grdDueWise.ClearStructure()
        });
    }

    function show() {                                  // btnshow_Click
        var b = el('show'); if (b.disabled) return;
        A.busy(b, true);
        fillData(0).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }

    // ------------------------------------------------------------------ New / Print / shortcuts
    function newSummary() {                            // BtnNewSummary_Click
        el('fromDate').value = A.addDays(A.today(), -30);
        setTo(A.today());
        grd.clear();
        el('vNetRcv').textContent = ''; el('vOverRcv').textContent = ''; el('vNotPay').textContent = '';
        try { el('fromDate').focus(); } catch (e) { }
    }
    function print() {                                 // btnPrintSumm_Click
        var b = el('print'); if (b.disabled) return;
        if (!dtFilteredData || !dtFilteredData.length) { alert('Record Not Found For Display'); return; }
        A.postGrid(RPT, dtFilteredData, null, b);
    }
    var SHORTCUTS = [
        ['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
        ['Ctrl+F5', 'For Focus On FromDate '], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
        ['Ctrl+ArrowUp', 'For Focus On FromDate'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]
    ];
    function closeForm() { w.location.href = '/accounts/dashboard'; }

    // ------------------------------------------------------------------ tabs
    var tabIndex = 0;
    function tab(i) {
        tabIndex = i;
        el('pageTab').hidden = i !== 0; el('pageCard').hidden = i !== 1;
        el('tabTab').classList.toggle('on', i === 0); el('tabCard').classList.toggle('on', i === 1);
    }

    // ------------------------------------------------------------------ events
    el('show').addEventListener('click', show);
    el('new').addEventListener('click', newSummary);
    el('print').addEventListener('click', print);
    el('shortcuts').addEventListener('click', function () { A.shortcuts(SHORTCUTS); });
    el('closeShortcuts').addEventListener('click', function () { el('shortcutDialog').close(); });
    el('tabTab').addEventListener('click', function () { tab(0); });
    el('tabCard').addEventListener('click', function () { tab(1); });
    el('toChk').addEventListener('change', function () { el('toWrap').classList.toggle('off', !this.checked); });
    /* txtIntervalDays_KeyPress = OnlytextNumberFunction: digits and control keys only */
    el('interval').addEventListener('keypress', function (e) { if (e.key.length === 1 && !/\d/.test(e.key) && !e.ctrlKey) e.preventDefault(); });
    el('interval').addEventListener('input', function () { var v = this.value.replace(/\D/g, ''); if (v !== this.value) this.value = v; });

    function focusables() { return ['toDate', 'interval', 'show'].map(A.focusOf); }
    /* DueDateAnalysisPayablesAndReceivablesForcast_KeyDown (KeyPreview) */
    d.addEventListener('keydown', function (e) {
        var dlg = el('shortcutDialog'); if (dlg.open) return;
        if (e.ctrlKey && e.altKey && (e.key === 'Control' || e.key === 'Alt')) { e.preventDefault(); A.shortcuts(SHORTCUTS); return; }
        if (e.key === 'Enter' && !e.ctrlKey && !e.altKey && !e.shiftKey && e.target.tagName !== 'BUTTON') {      // SendKeys "{TAB}"
            var list = focusables(), i = list.indexOf(d.activeElement);
            if (i >= 0 && i < list.length - 1) { e.preventDefault(); list[i + 1].focus(); return; }
        }
        if (e.key === 'Escape') { e.preventDefault(); closeForm(); return; }
        if (!e.ctrlKey || e.altKey) return;
        var k = e.key.toLowerCase();
        if (k === 's') { e.preventDefault(); show(); }
        else if (k === 't') { e.preventDefault(); tab(tabIndex === 1 ? 0 : 1); }
        else if (k === 'n') { e.preventDefault(); newSummary(); }
        else if (k === 'f5' || k === 'arrowup') { e.preventDefault(); try { el('fromDate').focus(); } catch (x) { } }
        else if (k === 'arrowdown') { e.preventDefault(); tab(0); grd.focus(); }
        else if (k === 'e') { e.preventDefault(); closeForm(); }
    });

    // ------------------------------------------------------------------ start (_Load)
    initGrids();
    el('fromDate').value = A.today();                 // fromdate.Value = DateTime.Now; fromdate.Enabled = false
    el('toDate').value = A.today(); el('toChk').checked = false; el('toWrap').classList.add('off');
    el('interval').value = '8';
    A.getJson(API + '/lookups').then(function (l) { look = l || look; if (grd.hasStructure()) grd.render(); }).catch(function () { });
}(window, document));
