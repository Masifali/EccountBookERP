/* ============================================================================================
 * countx_export_rptn_common.js - helpers shared by the Export Reports group N pages
 * (237, 248, 259, 264, 268, 271, 272). Builds on window.ExportRptA (countx_export_rpta_common.js:
 * busy, getJson/postJson with CSRF, bind, drawGrid, gridEvents, fullscreen, baseKeys, dateTypeChanged, print)
 * and adds the GridEX format strings these forms use, the date-type filter block of 259 / 271,
 * and the frmGDBreakUp popup of 272. Exposes window.ExportRptN.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var A = global.ExportRptA;

    function grp(intPart) { return intPart.replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
    /* .NET "0,0": at least two integer digits, grouped, no decimals (5 -> "05", 1234.6 -> "1,235"). */
    function fmt00(v) {
        var n = A.netD(v), neg = n < 0, s = Math.round(Math.abs(n)).toString();
        if (s.length < 2) s = '0' + s;
        return (neg ? '-' : '') + grp(s);
    }
    /* "#,##0." + N zeros (stringFormatsingle); N = 0 gives "#,##0." -> no decimals. */
    function fmtSingle(v, dec) { return A.fmtFixed(v, dec || 0); }
    /* stringFormatboth: "#,##0.00;(0,0.00); 0" - negatives in brackets, zero as " 0". */
    function fmtBoth(v, dec) {
        var n = A.netD(v);
        if (n === 0) return ' 0';
        var s = A.fmtFixed(Math.abs(n), dec || 0);
        if (n < 0) { var parts = s.split('.'); if (parts[0].length < 2) parts[0] = '0' + parts[0]; return '(' + parts.join('.') + ')'; }
        return s;
    }
    /* "#,#0." + N zeros (DecimalRateFormate / DecimalFCYRateFormate). */
    function fmtRate(v, dec) { return A.fmtFixed(v, dec || 0); }
    /* "#,##0.###" style (up to N decimals). */
    function fmtUpTo(v, dec) { return A.fmtOpt(v, dec); }
    /* A format fn whose Σ row (row === null) uses another format - GridEX FormatString vs TotalFormatString. */
    function cellTotal(cell, total) { return function (v, row) { return row === null ? total(v) : cell(v); }; }

    /* ------------------------------------------------------------------ date-type filter block (259 / 271)
     * Load: ParameterFill, ComboFill, Rows[0].Activate() (This Day -> From = today), GridBind. */
    function dateTypeBlock(cmbId, fromId, toId, getYearStart) {
        var s = A.$id(cmbId);
        if (s) s.addEventListener('change', function () { A.dateTypeChanged(cmbId, fromId, toId, getYearStart()); });
    }

    /* ------------------------------------------------------------------ frmGDBreakUp popup (272 GdNo link)
     * GainAndLossBreakUp_Load -> BindGdbreakUp: USP_GetGdBreakUpsByGdId(GdId, DocumentTypeId); RetrieveStructure,
     * Id hidden, FcGrossAmount / RealizedAmount / CommAmount / TotalRealized summed, "#,#.###". */
    function gdBreakUp(gdId, documentTypeId) {
        var m = A.$id('rptnGdBreakUp');
        if (!m) {
            m = document.createElement('div');
            m.id = 'rptnGdBreakUp';
            m.className = 'rpta-modal is-hidden';
            m.innerHTML = '<div class="rpta-modal-box">'
                + '<div class="pd-caption pd-caption-teal" style="height:28px;justify-content:space-between;"><span>GD BreakUp</span><button type="button" class="ex-fs-btn" id="rptnGdClose">&times;</button></div>'
                + '<div class="ex-grid-scroll ex-h400"><table class="win-grid" id="rptnGdGrid"><thead></thead><tbody></tbody><tfoot></tfoot></table></div>'
                + '<div class="ex-grid-empty is-hidden" id="rptnGdEmpty">No record found.</div>'
                + '</div>';
            document.body.appendChild(m);
            A.$id('rptnGdClose').addEventListener('click', function () { m.classList.add('is-hidden'); });
            m.addEventListener('click', function (e) { if (e.target === m) m.classList.add('is-hidden'); });
            document.addEventListener('keydown', function (e) { if (e.key === 'Escape' && !m.classList.contains('is-hidden')) { e.stopPropagation(); m.classList.add('is-hidden'); } }, true);
        }
        m.classList.remove('is-hidden');
        A.clearGrid('rptnGdGrid');
        A.show('rptnGdEmpty', false);
        return A.getJson('/api/export/bank-gd-summary/gd-break-up?gdId=' + (gdId || 0) + '&documentTypeId=' + (documentTypeId || 0)).then(function (d) {
            var rows = (d && d.rows) || [];
            var h3 = function (v) { return A.fmtHash(v, 3); };
            A.drawGrid('rptnGdGrid', [
                { key: 'Id', hidden: true }, { key: 'DocDate', caption: 'DocDate', fmt: 'd' }, { key: 'DocNo', caption: 'DocNo' },
                { key: 'InvoiceNo', caption: 'InvoiceNo' }, { key: 'GDNO', caption: 'GDNO' }, { key: 'CustomerName', caption: 'CustomerName' },
                { key: 'DebitAccount', caption: 'DebitAccount' }, { key: 'BankFbpNo', caption: 'BankFbpNo' },
                { key: 'FcGrossAmount', caption: 'FcGrossAmount', num: true, sum: true, fmt: h3 }, { key: 'CurrencyCode', caption: 'CurrencyCode' },
                { key: 'ExchangeRate', caption: 'ExchangeRate', num: true, fmt: h3 },
                { key: 'RealizedAmount', caption: 'RealizedAmount', num: true, sum: true, fmt: h3 },
                { key: 'CommAmount', caption: 'CommAmount', num: true, sum: true, fmt: h3 },
                { key: 'TotalRealized', caption: 'TotalRealized', num: true, sum: true, fmt: h3 }
            ], rows, {});
            A.show('rptnGdEmpty', !rows.length);
        }).catch(function (e) { A.box(e.message); });
    }

    /* ------------------------------------------------------------------ 259 / 271 page shell
     * Both forms share frmExportShipingLineBookingRpt_Load: ParameterFill (CommonServices.DateType, BindDDL ZeroIndex false),
     * ComboFill (GetDataForDropDownFromExportInvoice split by ActivityType; each combo bound only when it has rows),
     * Rows[0].Activate() when the date type is empty (This Day -> Date From = today; Date To keeps today), GridBind.
     * Reset (btnnew): Contract / Customer / Item Type / Item Name cleared, focus Date From, GridBind (dates and date type kept).
     * btnShow_Leave: focus back on Date Type unless the button was left with Shift+Tab (btnShow_PreviewKeyDown).
     * cfg: { api, grid, empty, cols(), extra() -> extra show body, onRows(rows) } */
    function contractRegisterPage(cfg) {
        var st = { rows: [], last: null, yearStart: '', shiftTab: false };
        function filters() {
            var f = { fromDate: A.val('txtdatefrom'), toDate: A.val('txtdateto'), exImLcOrderId: intVal('cmbcontractNo'),
                supplierCustomerId: intVal('cmbSupplierName'), itemTypeId: intVal('CmbItemType'), itemId: intVal('CmbItemName') };
            if (cfg.extra) { var x = cfg.extra(); Object.keys(x).forEach(function (k) { f[k] = x[k]; }); }
            return f;
        }
        function gridBind() {
            var f = filters();
            st.rows = [];
            return A.postJson(cfg.api + '/show', f).then(function (d) {
                st.rows = (d && d.rows) || [];
                st.last = f;
                if (st.rows.length) A.drawGrid(cfg.grid, cfg.cols(), st.rows, {});
                else { var t = A.$id(cfg.grid); t.querySelector('thead').innerHTML = ''; A.clearGrid(cfg.grid); }
                A.show(cfg.empty, !st.rows.length);
            }).catch(function (e) { A.box(e.message); });
        }
        function show(btn) { return A.busy(btn || 'btnShow', gridBind); }
        function reset(btn) {
            return A.busy(btn || 'btnnew', function () {
                ['cmbcontractNo', 'cmbSupplierName', 'CmbItemType', 'CmbItemName'].forEach(function (id) { A.setVal(id, '0'); });
                A.focus('txtdatefrom');
                return gridBind();
            });
        }
        function load() {
            A.setText('txtdatefrom', A.today());
            A.setText('txtdateto', A.today());
            return A.getJson(cfg.api + '/setup').then(function (d) {
                d = d || {};
                st.yearStart = d.yearStart || '';
                if (d.combosError) A.box(d.combosError);
                A.bind('cmbperemeter', d.dateTypes || [], 'Id', 'name', false);
                if ((d.itemTypes || []).length) A.bind('CmbItemType', d.itemTypes, 'Id', 'name', false);
                if ((d.items || []).length) A.bind('CmbItemName', d.items, 'Id', 'name', false);
                if ((d.contracts || []).length) A.bind('cmbcontractNo', d.contracts, 'Id', 'name', false);
                if ((d.customers || []).length) A.bind('cmbSupplierName', d.customers, 'Id', 'name', false);
                /* BindDDL ZeroIndex:false leaves the combos without a selection; the web list keeps a blank first entry so a
                   filter can be cleared again (UltraCombo.Text = ""). */
                ['CmbItemType', 'CmbItemName', 'cmbcontractNo', 'cmbSupplierName'].forEach(function (id) {
                    var s = A.$id(id); if (s && (!s.options.length || s.options[0].value !== '0')) { s.insertAdjacentHTML('afterbegin', '<option value="0"></option>'); s.value = '0'; }
                });
                A.refreshCombos();
                if (cfg.onSetup) cfg.onSetup(d);
                if (A.netI(A.val('cmbperemeter')) === 0 || A.$id('cmbperemeter').selectedIndex <= 0) {
                    A.$id('cmbperemeter').selectedIndex = 0; A.refreshCombos();
                }
                A.dateTypeChanged('cmbperemeter', 'txtdatefrom', 'txtdateto', st.yearStart);
            }).catch(function (e) { A.box(e.message); }).then(function () { return show(A.$id('btnShow')); });
        }
        function wire(keysExtra) {
            dateTypeBlock('cmbperemeter', 'txtdatefrom', 'txtdateto', function () { return st.yearStart; });
            var b = A.$id('btnShow');
            b.addEventListener('keydown', function (e) { st.shiftTab = e.shiftKey && e.key === 'Tab'; });
            b.addEventListener('blur', function () { if (!st.shiftTab) setTimeout(function () { A.focus('cmbperemeter'); }, 0); st.shiftTab = false; });
            A.fullscreenButtons();
            document.addEventListener('keydown', function (e) {
                if (A.baseKeys(e, cfg.shortcuts)) return;
                var k = (e.key || '').toLowerCase();
                if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
                else if (e.ctrlKey && k === 'r') { e.preventDefault(); /* btnRefresh_Click is empty on the desktop */ }
                else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); A.focus('cmbperemeter'); }
                else if (e.ctrlKey && k === 's') { e.preventDefault(); show(); }
                else if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); A.focusGrid(cfg.grid); }
                else if (keysExtra) keysExtra(e);
            });
        }
        return { st: st, show: show, reset: reset, load: load, wire: wire, filters: filters };
    }

    /* Combo value as an int (UltraCombo.Value -> Conversion.ToInt). */
    function intVal(id) { return A.netI(A.val(id)); }
    /* GUARDED print args: 0 / '' are left out so the contract omits them, exactly as the BLL "!= 0" guards. */
    function args(o) { var r = {}; Object.keys(o).forEach(function (k) { var v = o[k]; if (v !== 0 && v !== '' && v !== null && v !== undefined) r[k] = v; }); return r; }
    function embedFooter(id) { if (/[?&]embed=1/.test(location.search)) { var f = A.$id(id); if (f) f.classList.add('is-hidden'); } }

    global.ExportRptN = {
        fmt00: fmt00, fmtSingle: fmtSingle, fmtBoth: fmtBoth, fmtRate: fmtRate, fmtUpTo: fmtUpTo, cellTotal: cellTotal,
        dateTypeBlock: dateTypeBlock, gdBreakUp: gdBreakUp, intVal: intVal, args: args, embedFooter: embedFooter,
        contractRegisterPage: contractRegisterPage
    };
}(window));
