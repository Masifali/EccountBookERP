/* ============================================================================================
 * Screen 932 "5016 Export Shipment Weight Audit" - Architecture.WinApp.Audit_Dashboard.frmAuditByWeightReport
 * Page: templates/accounts/reports/acrpt4_export_weight_audit.html (route /audit/reports/export-shipment-weight-audit).
 * Every desktop handler, in desktop order (frmAuditByWeightReport.cs):
 *   frmExportShipingLineBookingRpt_Load -> ParameterFill() (CommonServices.DateType 1..5, "Parameters"), ComboFill(), date type 0 -> Rows[1] ("This Week").
 *   ComboFill                -> InvDeliveryOrder.GetDataForDropDownFromDeliveryOrder (OrganizationId, CompanyId) split by Activity "InvoiceNo" / "ContractNo"
 *                               (Id, ReferenceName) -> CmbInvoiceNo "Invoice No" / CmbContractNo "Contract No", ZeroIndex true, previous selection kept.
 *   cmbperemeter_ValueChanged -> 1 From = today | 2 From = today - 7 | 3 From = first of month, To = today | 4 From = 1 Jan, To = today | 5 From = FY start.
 *   btnShow_Click / GridBind -> ExImInvoice.ExportSalesAuditByWeight (SpEximInvoice_DoWeightWbWeightDiff_Rpt): invoice id, contract id, From / To, status
 *                               (In Process / Completed, none for All). dtHistory = every gate-pass row. rows > 0 -> main grid = first row of each distinct
 *                               InvoiceNo (GpCount = rows of that invoice) + GridSetting() + GridColumnsColourChange(); no rows -> grd.DataSource = null.
 *   grd_SelectionChanged     -> grdDetail = the dtHistory rows of the current InvoiceNo + GridDetailSetting().
 *   grd_LinkClicked          -> InvoiceNo: ExportSalesAuditByWeight(invoice only) -> 618-DoWeightWbWeightDiff.rpt, else "Not Record Found For Display";
 *                               DoGrossWeight (> 0): DeliveryOrderHistory (DateTypeId 5, Export, FY start .. To, InvoiceId, second weight).
 *   grdDetail_LinkClicked    -> GpNo: OutwardGatePassWithWb (258) | DONo: ExportDeliveryOrderSlip396(DOId, 84).
 *   btnPrint (618-Print) / btnPrint618_01 -> dtHistory rows -> 618-DoWeightWbWeightDiff.rpt / 618_01_DoWeightWbWeightDiff.rpt, else "Not Record Found For Display".
 *   btnnew_Click / Reset     -> txtdatefrom.Focus(), GridBind() (the filters are NOT cleared).
 *   btnShortCutKey / Ctrl+Alt -> MakeShortCutKeys (ShortCutKeyPopUp).
 *   KeyDown (KeyPreview)     -> Enter = Tab, Ctrl+E / Esc close, Ctrl+N reset, Ctrl+F5 date type, Ctrl+S show, Ctrl+P 618 print, Ctrl+Down grid, Ctrl+Alt shortcuts.
 *   grd_KeyDown              -> Enter = Tab; Ctrl+Space calls the link of the current row (Janus).
 *   btnShow_Leave            -> only fires for an unnamed active control; every control after Show is named, so it never applies here.
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt4, el = A.el, API = '/api/accounts/acrpt4/weight', seq = 0;
    var RPT618 = '618-DoWeightWbWeightDiff.rpt', RPT618_01 = '618_01_DoWeightWbWeightDiff.rpt';
    var hist = [];                                   // dtHistory
    var yearStart = null;
    var W0 = '#,##0;(0,0);0', W3 = '#,##0.###;(0,0.###);0';
    var DATE_TYPES = [{ Id: 1, Parameters: 'This Day' }, { Id: 2, Parameters: 'This Week' }, { Id: 3, Parameters: 'This Month' },
                      { Id: 4, Parameters: 'This Year' }, { Id: 5, Parameters: 'Financial Year' }];

    function ci(r, k) { return A.ci(r, k); }
    function toInt(v) { var s = String(v == null ? '' : v).trim(); return /^[-+]?\d+$/.test(s) ? parseInt(s, 10) : (isFinite(Number(s)) && s !== '' ? Math.trunc(Number(s)) : 0); }
    function str(v) { return v == null ? '' : String(v); }

    // ------------------------------------------------------------------ grids
    function sumCol(key, caption, width, fmt) {
        return { key: key, caption: caption, width: width, type: 'num', align: 'right', hAlign: 'center', sum: true, fmt: fmt, totalFmt: fmt };
    }
    function mainColumns() {
        function c(key, caption, width, extra) { return Object.assign({ key: key, caption: caption, width: width, hAlign: 'center' }, extra || {}); }
        return [
            c('SupplierCustomerId', 'SupplierCustomerId', 100, { hidden: true, type: 'int', align: 'right' }),
            c('CustomerName', 'CustomerName', 170),
            c('ContractNo', 'Contract Number', 130),
            c('InvoiceId', 'InvoiceId', 100, { hidden: true, type: 'int', align: 'right' }),
            c('InvoiceNo', 'InvoiceNo', 100, { link: true }),
            c('InvoiceDate', 'InvoiceDate', 73, { type: 'date', dateFmt: 'yy' }),
            Object.assign(sumCol('Containers', 'FCL', 50, '#,##0'), { type: 'int', hAlign: 'center' }),
            sumCol('InvoiceGrossWeight', 'InvoiceGrossWeight', 90, W0),
            Object.assign(sumCol('DoGrossWeight', 'DoGrossWeight', 90, W0), { link: true }),
            sumCol('WbWeight', 'Weigh Bridge Weight', 90, W0),
            sumCol('ExcessWeight', 'Less / Excess DO - WB', 90, W3),
            c('LessExcess', 'LessExcess', 60),
            sumCol('AvgLessExcess', 'Avg Ls/Exc Per FCL', 80, '#,##0'),
            sumCol('LessExcessAmount', 'LessExcessAmount', 90, '#,##0'),
            sumCol('InvoiceNetWeight', 'InvoiceNetWeight', 90, W0),
            sumCol('DoNetWeight', 'DoNetWeight', 90, W0),
            sumCol('DiffWeight', 'Less / Excess Invoice - DO', 90, W0),
            Object.assign(sumCol('GpCount', 'No Of GatePass', 75, '#,##0'), { type: 'int' }),
            c('InvoiceStatus', 'Dispatch Status', 100)
        ];
    }
    function detailColumns() {
        function c(key, caption, width, extra) { return Object.assign({ key: key, caption: caption, width: width, hAlign: 'center' }, extra || {}); }
        return [
            c('GpDate', 'GpDate', 73, { type: 'date', dateFmt: 'yy' }),
            c('GpId', 'GpId', 100, { hidden: true, type: 'int', align: 'right' }),
            c('GpNo', 'GpNo', 40, { type: 'int', align: 'right', link: true }),
            c('DOId', 'DOId', 100, { hidden: true, type: 'int', align: 'right' }),
            c('DONo', 'DO No', 40, { type: 'int', align: 'right', link: true }),
            c('Container1', 'Container1', 115),
            c('Container2', 'Container2', 115),
            sumCol('DoNetWeight', 'DoNetWeight', 90, W3),
            sumCol('DoPackingWeight', 'DoPackingWeight', 90, W3),
            sumCol('DoGrossWeight', 'DoGrossWeight', 90, W3),
            sumCol('DoOtherWeight', 'DoOtherWeight', 90, W3),
            sumCol('WbWeight', 'WbWeight', 90, W3),
            sumCol('Less/Excess', 'WB-DO Difference', 90, W3),
            c('LessExcess', 'LessExcess', 100)
        ];
    }

    /* GridColumnsColourChange: InvoiceStatus "Completed" -> green bold, "In Process" -> red bold. */
    function cellStyle(col, row) {
        if (col.key !== 'InvoiceStatus') return null;
        var v = str(ci(row, 'InvoiceStatus'));
        if (v === 'Completed') return { color: 'green', bold: true };
        if (v === 'In Process') return { color: 'red', bold: true };
        return null;
    }

    var detail = A.createGrid({ table: el('grdDetail'), scroller: el('detWrap'), nav: el('detNav'), navText: el('detNavText'),
        headerLines: 2, groupTotals: true, totalRow: true, onLink: detailLink });          // GroupByBoxVisible = false
    var main = A.createGrid({ table: el('grd'), scroller: el('gridWrap'), nav: el('nav'), navText: el('navText'), groupBoxEl: el('gbox1'),
        headerLines: 2, groupTotals: true, totalRow: true, autoSelect: true, onLink: mainLink, onSelect: selectionChanged, cellStyle: cellStyle });
    main.clear(); detail.clear();

    // ------------------------------------------------------------------ GridBind
    function buildMain() {
        var seen = {}, out = [];
        hist.forEach(function (r) {
            var inv = str(ci(r, 'InvoiceNo'));
            if (seen[inv]) return;
            seen[inv] = true;
            var cnt = hist.filter(function (x) { return str(ci(x, 'InvoiceNo')) === inv; }).length;
            out.push({
                SupplierCustomerId: toInt(ci(r, 'SupplierCustomerId')), CustomerName: ci(r, 'CustomerName'), ContractNo: ci(r, 'LcOrderNo'),
                InvoiceId: toInt(ci(r, 'InvoiceId')), InvoiceNo: ci(r, 'InvoiceNo'), InvoiceDate: ci(r, 'InvoiceDate'),
                Containers: toInt(ci(r, 'Containers')), InvoiceGrossWeight: ci(r, 'InvoiceGwTotal'), DoGrossWeight: ci(r, 'DoGwTotal'),
                WbWeight: ci(r, 'WbGwTotal'), ExcessWeight: ci(r, 'ExcessWtTotal'), LessExcess: ci(r, 'LessExcess'),
                AvgLessExcess: ci(r, 'AvgLessExcess'), LessExcessAmount: ci(r, 'LessExcessAmount'),
                InvoiceNetWeight: ci(r, 'InvoiceNwTotal'), DoNetWeight: ci(r, 'DoNwTotal'), DiffWeight: ci(r, 'InvNwDoNwDiff'),
                GpCount: cnt, InvoiceStatus: ci(r, 'InvoiceStatus')
            });
        });
        return out;
    }
    function status() { return el('RdInProcess').checked ? 'In Process' : (el('rdCompleted').checked ? 'Completed' : ''); }
    function gridBind() {
        var q = new URLSearchParams({ invoiceId: A.intArg(el('CmbInvoiceNo').value), contractId: A.intArg(el('CmbContractNo').value),
            fromDate: el('txtdatefrom').value || A.today(), toDate: el('txtdateto').value || A.today(), status: status() });
        var token = ++seq;
        hist = [];
        return A.getJson(API + '/data?' + q.toString()).then(function (data) {
            if (token !== seq) return;
            hist = (data && data.rows) || [];
            if (hist.length) main.setData(mainColumns(), buildMain());           // setData selects the first record -> grd_SelectionChanged
            else { main.clear(); }                                               // grd.DataSource = null
            if (!hist.length) detail.clear();
        });
    }
    function show() {                                                            // btnShow_Click
        var b = el('show'); if (b.disabled) return Promise.resolve();
        A.busy(b, true);
        return gridBind().catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function reset() { el('txtdatefrom').focus(); return show(); }               // Reset(): Focus + GridBind

    // ------------------------------------------------------------------ grd_SelectionChanged
    function selectionChanged(row) {
        var inv = str(ci(row, 'InvoiceNo'));
        var rows = hist.filter(function (r) { return str(ci(r, 'InvoiceNo')) === inv; }).map(function (r) {
            return {
                GpDate: ci(r, 'GpDate'), GpId: toInt(ci(r, 'GpId')), GpNo: toInt(ci(r, 'GpSrNo')), DOId: toInt(ci(r, 'DOId')), DONo: toInt(ci(r, 'DONo')),
                Container1: ci(r, 'Container1'), Container2: ci(r, 'Container2'), DoNetWeight: ci(r, 'doNetWeight'), DoPackingWeight: ci(r, 'doPackingWeight'),
                DoGrossWeight: ci(r, 'doGrossWeight'), DoOtherWeight: ci(r, 'doOtherWeight'), WbWeight: ci(r, 'WbWeight'),
                'Less/Excess': ci(r, 'ExcessWt'), LessExcess: ci(r, 'LessExcessDoWise')
            };
        });
        detail.setData(detailColumns(), rows);
    }

    // ------------------------------------------------------------------ prints / links
    function printHistory(rpt, btn) {
        if (!hist.length) { alert('Not Record Found For Display'); return; }
        A.postGrid(rpt, hist, null, btn);
    }
    function print618() { printHistory(RPT618, el('btnPrint')); }
    function print618_01() { printHistory(RPT618_01, el('btnPrint618_01')); }

    function mainLink(row, col) {                                                // grd_LinkClicked (uses grd.CurrentRow)
        var invoiceId = toInt(ci(row, 'InvoiceId'));
        if (col.key === 'InvoiceNo') {
            A.getJson(API + '/data?' + new URLSearchParams({ invoiceId: invoiceId }).toString()).then(function (data) {
                if (!data || !data.rows || !data.rows.length) { alert('Not Record Found For Display'); return; }
                A.postGrid(RPT618, data.rows, null, null);
            }).catch(function (e) { alert(e.message); });
        } else if (col.key === 'DoGrossWeight' && Number(ci(row, 'DoGrossWeight') || 0) > 0) {
            /* DeliveryOrderHistory: DateTypeId 5, DOType Export, FY start .. Date To, InvoiceId, RadWithSecondWeight (screen 250). */
            w.open('/export/delivery-order-report?dateTypeId=5&doType=Export&invoiceId=' + invoiceId + '&fromDate=' + (yearStart || A.today())
                + '&toDate=' + (el('txtdateto').value || A.today()) + '&weight=second', '_blank');
        }
    }
    function detailLink(row, col) {                                              // grdDetail_LinkClicked
        if (typeof w.printRpt !== 'function') { alert('Printing is not available on this page.'); return; }
        if (col.key === 'GpNo') w.printRpt('258-OutwardGatePassWithWbAndLabSlip.rpt', { id: toInt(ci(row, 'GpId')) });
        else if (col.key === 'DONo') w.printRpt('396-ExportDeliveryOrderSlip.rpt', { id: toInt(ci(row, 'DOId')), documentTypeId: 84 });
    }

    function shortcuts() {                                                       // MakeShortCutKeys
        A.shortcuts([['Ctrl+N', 'For New'], ['Ctrl+E', 'For Close'], ['Ctrl+F5', 'For Focus on Date Type'], ['Ctrl+S', 'For Showing Data'],
            ['Ctrl+P', 'For Print'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }
    function closeForm() { w.location.href = '/accounts/dashboard'; }

    // ------------------------------------------------------------------ date type
    function dateType() {                                                        // cmbperemeter_ValueChanged
        var v = A.intArg(el('cmbperemeter').value), now = new Date();
        if (v === 1) el('txtdatefrom').value = A.today();
        else if (v === 2) el('txtdatefrom').value = A.addDays(A.today(), -7);
        else if (v === 3) { el('txtdatefrom').value = A.ymd(new Date(now.getUTCFullYear(), now.getUTCMonth(), 1)); el('txtdateto').value = A.today(); }
        else if (v === 4) { el('txtdatefrom').value = now.getFullYear() + '-01-01'; el('txtdateto').value = A.today(); }
        else if (v === 5) { if (yearStart) el('txtdatefrom').value = yearStart; }
    }

    // ------------------------------------------------------------------ events
    el('show').addEventListener('click', show);
    el('btnnew').addEventListener('click', reset);
    el('btnPrint').addEventListener('click', print618);
    el('btnPrint618_01').addEventListener('click', print618_01);
    el('btnShortCutKey').addEventListener('click', shortcuts);
    el('cmbperemeter').addEventListener('change', dateType);

    function focusables() {
        return ['cmbperemeter', 'txtdatefrom', 'txtdateto', 'CmbContractNo', 'CmbInvoiceNo', 'show', 'RdInProcess', 'rdCompleted', 'rdAll'].map(A.focusOf);
    }
    d.addEventListener('keydown', function (e) {                                 // frmExportShipingLineBookingRpt_KeyDown
        var onGrid = e.target && e.target.closest && (e.target.closest('#gridWrap') || e.target.closest('#detWrap'));
        if (e.key === 'Enter' && !e.ctrlKey && !e.altKey && !e.shiftKey && !A.comboOpen() && e.target.tagName !== 'BUTTON' && !onGrid) {   // SendKeys "{TAB}"
            var list = focusables(), i = list.indexOf(d.activeElement);
            if (i >= 0 && i < list.length - 1) { e.preventDefault(); list[i + 1].focus(); return; }
        }
        if (e.key === 'Escape' && !A.comboOpen() && !(el('shortcutDialog') && el('shortcutDialog').open)) { e.preventDefault(); closeForm(); return; }
        if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
        if (e.key === 'F5' && e.ctrlKey) { e.preventDefault(); A.focusOf('cmbperemeter').focus(); return; }
        if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); main.focus(); return; }
        if (!e.ctrlKey || e.altKey) return;
        var k = e.key.toLowerCase();
        if (k === 'e') { e.preventDefault(); closeForm(); }
        else if (k === 'n') { e.preventDefault(); reset(); }
        else if (k === 's') { e.preventDefault(); show(); }
        else if (k === 'p') { e.preventDefault(); print618(); }
    });

    // ------------------------------------------------------------------ Load
    var today = A.today();
    el('txtdatefrom').value = today; el('txtdateto').value = today;
    A.fillSelect(el('cmbperemeter'), DATE_TYPES, 'Id', 'Parameters', { keep: false, empty: false });
    A.getJson(API + '/init').then(function (init) {
        yearStart = init.yearStart || null;
        var inv = [], con = [];
        (init.items || []).forEach(function (r) {
            var act = str(ci(r, 'Activity'));
            if (act === 'InvoiceNo') inv.push({ Id: ci(r, 'Id'), InvoiceNo: ci(r, 'ReferenceName') });
            if (act === 'ContractNo') con.push({ Id: ci(r, 'Id'), ContractNo: ci(r, 'ReferenceName') });
        });
        var z = { keep: true, emptyText: '...Select Any Value...', emptyValue: 0 };
        if (inv.length) A.fillSelect(el('CmbInvoiceNo'), inv, 'Id', 'InvoiceNo', z);
        if (con.length) A.fillSelect(el('CmbContractNo'), con, 'Id', 'ContractNo', z);
    }).catch(function (e) { alert(e.message); }).then(function () {
        // the date type has no value -> Rows[1] ("This Week") is activated, which fires cmbperemeter_ValueChanged
        var s = el('cmbperemeter'); s.value = '2'; s.dispatchEvent(new Event('change', { bubbles: true }));
    });
}(window, document));
