/* ============================================================================================
 * Screen 963 "Stock In Trust" — Architecture.WinApp.Inventory_Reports.frmGatePassForStockInTrust.
 *
 *   frmGatePassForStockReseved_Load :118   ALlDropDown, IsAcceptFill, BranchesFill, Datetypefill (Rows[5] = Financial Year)
 *   cmbBranchName_Leave :758               branches changed → ALlDropDown again (or the two combos emptied)
 *   cmbDateType_ValueChanged :262          From / To dates by type
 *   btnSearch_Click :299 → GridFill :311   Sp_GatePassInward_StockReservedAsAmanat
 *   grdfrm "Update" :778                   status combo per row → GatePassInwardStatusUpdate
 *   grdfrm GpSrNo link :507                GatePassInwardSlipAndRegisterReport → 251-InvRptInwardGatePassSlip.rpt
 *   btnPrint_Click :540                    253_01-InvRptGatePassInwardRegisterA.rpt with the grid rows
 *   btnReset_Click :563 / toolStripButton1_Click_1 :592 / frmGatePassReport_KeyDown :644 / MakeShortCutKeys :691
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc;
    var api = '/inventory/stock-in-trust/api';
    var st = { init: null, rows: [] };

    function num(v) { var s = String(v === null || v === undefined ? '' : v).replace(/,/g, '').trim(); if (!s) return 0; var n = Number(s); return isNaN(n) ? 0 : n; }
    function fmt(v, p) { return (Number(v) || 0).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: p }); }
    function fail(e) { alert(e && e.message ? e.message : String(e)); }
    function val(k) { return parseInt($id(k).value || '0', 10) || 0; }
    function text(k) { var el = $id(k); return el.selectedIndex >= 0 && el.options.length ? el.options[el.selectedIndex].text : ''; }
    function bind(k, list, defaultRow) {
        var el = $id(k), prev = el.value;
        var h = defaultRow === null ? '' : '<option value="0">' + esc(defaultRow) + '</option>';
        (list || []).forEach(function (r) { h += '<option value="' + esc(r.Id) + '">' + esc(r.Name) + '</option>'; });
        el.innerHTML = h;
        el.value = (list || []).some(function (r) { return String(r.Id) === prev; }) ? prev : (el.options.length ? el.options[0].value : '');
    }

    // ------------------------------------------------------------------------------ branches (checked combo)

    function branchIds() {
        var ids = [];
        $id('cmbBranchNameList').querySelectorAll('input[data-b]:checked').forEach(function (c) { ids.push(c.getAttribute('data-b')); });
        return ids;
    }
    function syncBranchText() {
        var names = [];
        $id('cmbBranchNameList').querySelectorAll('input[data-b]:checked').forEach(function (c) { names.push(c.getAttribute('data-n')); });
        $id('cmbBranchNameText').textContent = names.join(',');
    }
    function renderBranches(list, userBranchId) {
        var h = '<label class="all"><input type="checkbox" id="brAll"> (All)</label>';
        (list || []).forEach(function (b) { h += '<label><input type="checkbox" data-b="' + esc(b.Id) + '" data-n="' + esc(b.Name) + '"' + (b.Id === userBranchId ? ' checked' : '') + '> ' + esc(b.Name) + '</label>'; });
        $id('cmbBranchNameList').innerHTML = h;
        $id('brAll').onchange = function () { $id('cmbBranchNameList').querySelectorAll('input[data-b]').forEach(function (c) { c.checked = $id('brAll').checked; }); syncBranchText(); };
        $id('cmbBranchNameList').querySelectorAll('input[data-b]').forEach(function (c) { c.onchange = syncBranchText; });
        syncBranchText();
    }
    function toggleBranches() { $id('cmbBranchName').classList.toggle('is-open'); }
    /** cmbBranchName_Leave :758. */
    function branchesLeave() {
        if (branchIds().length) allDropDown(); else { $id('cmbSupplierName').innerHTML = ''; $id('cmbgptype').innerHTML = ''; }
    }
    document.addEventListener('click', function (e) {
        var m = $id('cmbBranchName');
        if (m && m.classList.contains('is-open') && !m.contains(e.target)) { m.classList.remove('is-open'); branchesLeave(); }
    });

    // ------------------------------------------------------------------------------ load

    function init() {
        $id('gpFromDate').value = C.today(); $id('gpToDate').value = C.today();
        renderGrid([]);
        C.getJson(api + '/init').then(function (d) {
            st.init = d;
            renderBranches(d.branches, d.userBranchId);                       // BranchesFill :218 — Text = UserAccount.BranchName
            allDropDown();                                                    // ALlDropDown :141
            isAcceptFill();                                                   // IsAcceptFill :194
            dateTypeFill();                                                   // Datetypefill :247
            $id('cmbDateType').focus();
        }).catch(fail);
        document.addEventListener('keydown', keyDown);
    }
    /** ALlDropDown :141 — Supplier / Gate Pass Type for the checked branches, default row, Rows[0] active. */
    function allDropDown() {
        return C.getJson(api + '/dropdowns' + C.qs({ branchIds: branchIds().join(',') })).then(function (d) {
            bind('cmbSupplierName', d.Supplier, 'Supplier Name');
            bind('cmbgptype', d.GatePassType, 'Gate Pass Type');
            $id('cmbSupplierName').selectedIndex = 0; $id('cmbgptype').selectedIndex = 0;
        }).catch(fail);
    }
    /** IsAcceptFill :194 — no default row; nothing activated unless FlagPendingWork. */
    function isAcceptFill() {
        var el = $id('cmbIsAccept');
        el.innerHTML = '<option value=""></option>' + (st.init.statuses || []).map(function (s) { return '<option value="' + s.Id + '">' + esc(s.Name) + '</option>'; }).join('');
        el.value = '';
    }
    /** Datetypefill :247 — CommonServices.DateType() with the default row, Rows[5] ("Financial Year") active. */
    function dateTypeFill() {
        $id('cmbDateType').innerHTML = '<option value="0">Date Type</option><option value="1">This Day</option><option value="2">This Week</option><option value="3">This Month</option><option value="4">This Year</option><option value="5">Financial Year</option>';
        $id('cmbDateType').selectedIndex = 5;
        dateTypeChanged();
    }
    /** cmbDateType_ValueChanged :262. */
    function dateTypeChanged() {
        var v = val('cmbDateType'), today = C.today(), n = new Date();
        if (v === 1) $id('gpFromDate').value = today;
        else if (v === 2) { var d = new Date(n.getFullYear(), n.getMonth(), n.getDate() - 7); $id('gpFromDate').value = d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
        else if (v === 3) { $id('gpFromDate').value = n.getUTCFullYear() + '-' + String(n.getUTCMonth() + 1).padStart(2, '0') + '-01'; $id('gpToDate').value = today; }
        else if (v === 4) { $id('gpFromDate').value = n.getFullYear() + '-01-01'; $id('gpToDate').value = today; }
        else if (v === 5) { if (st.init && st.init.financialYearStart) $id('gpFromDate').value = st.init.financialYearStart; }
    }

    // ------------------------------------------------------------------------------ show

    /** GridFill :311. */
    function show() {
        var ids = branchIds();
        if (!ids.length) { $id('cmbBranchName').focus(); alert('Select Branch First'); return; }
        C.postJson(api + '/show', {
            branchIds: ids.join(','), fromDate: $id('gpFromDate').value, toDate: $id('gpToDate').value,
            fromDocNo: num($id('txtGpFrom').value), toDocNo: num($id('txtGPTo').value), supplierCustomerId: val('cmbSupplierName'),
            status: $id('cmbIsAccept').value ? text('cmbIsAccept') : '', gatepassType: val('cmbgptype') > 0 ? text('cmbgptype') : ''
        }).then(function (list) { st.rows = list || []; renderGrid(st.rows); }).catch(fail);
    }

    /* GrdSetting :399 — Update button at 0, StatusUpdate combo at 1, GpSrNo and NoOfAttachments links, sums on the weights. */
    var COLS = [
        { k: 'BranchName' }, { k: 'OrderType' }, { k: 'GpSrNo', cap: 'Gp No', link: 'gp' }, { k: 'GpDate', date: true }, { k: 'GatePassType' }, { k: 'VehicleType' },
        { k: 'VehicleNo' }, { k: 'BiltyNo' }, { k: 'OrderNo' }, { k: 'SupplierName' }, { k: 'ItemName' }, { k: 'ItemQty', sum: 3 },
        { k: 'SupplierFirstWeight', sum: 3 }, { k: 'SupplierSecondWeight', sum: 3 }, { k: 'SupplierWeight', sum: 3 }, { k: 'FactoryWeight', sum: 3 },
        { k: 'DifferenceWeight', sum: 3 }, { k: 'Freight', sum: 2 }, { k: 'NetPaid', sum: 2 }, { k: 'Status' }, { k: 'CityName' }, { k: 'EntryUser' },
        { k: 'Remarks' }, { k: 'InTime', dt: true }, { k: 'OutTime', dt: true }, { k: 'NoOfAttachments', link: 'att' }
    ];
    function renderGrid(rows) {
        var t = $id('grdfrm');
        t.tHead.innerHTML = '<tr><th>Update</th><th>StatusUpdate</th>' + COLS.map(function (c) { return '<th>' + esc(c.cap || c.k) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = rows.map(function (r, i) {
            return '<tr data-i="' + i + '"><td class="ctr"><button type="button" class="cx-link" data-u="' + i + '">Update</button></td>' +
                '<td><select class="cell" data-s="' + i + '"><option value="0">Vehicle Status</option><option value="1">Accepted</option><option value="2">Rejected</option></select></td>' +
                COLS.map(function (c) {
                    var v = r[c.k];
                    if (c.link === 'gp') return '<td class="ctr"><a data-gp="' + i + '">' + esc(v) + '</a></td>';
                    if (c.link === 'att') return '<td class="lnk" title="Attachments are not ported to the web page">' + esc(v) + '</td>';
                    if (c.date) return '<td>' + esc(C.gridDate(v)) + '</td>';
                    if (c.dt) return '<td>' + esc(C.gridDateTime(v, true)) + '</td>';
                    if (c.sum) return '<td class="num">' + fmt(v, c.sum) + '</td>';
                    return '<td>' + esc(v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        t.tFoot.innerHTML = rows.length ? '<tr><td></td><td></td>' + COLS.map(function (c) { return '<td class="num">' + (c.sum ? fmt(rows.reduce(function (a, r) { return a + num(r[c.k]); }, 0), c.sum) : '') + '</td>'; }).join('') + '</tr>' : '';
        t.tBodies[0].querySelectorAll('[data-u]').forEach(function (b) {
            b.onclick = function () {
                var i = parseInt(b.getAttribute('data-u'), 10), sel = t.tBodies[0].querySelector('[data-s="' + i + '"]');
                if (!sel || sel.value === '0') { alert('Please select vehicle status first...'); return; }                 // :794
                C.postJson(api + '/status', { id: rows[i].Id, status: sel.options[sel.selectedIndex].text }).then(function (res) { alert(res.message); show(); }).catch(fail);
            };
        });
        t.tBodies[0].querySelectorAll('[data-gp]').forEach(function (a) {
            a.onclick = function () { window.printRpt('251-InvRptInwardGatePassSlip.rpt', { id: rows[parseInt(a.getAttribute('data-gp'), 10)].Id }); };   // :509
        });
    }

    /** btnPrint_Click :540 — Reporting.ShowReportWithDataTable(dt, "253_01-InvRptGatePassInwardRegisterA.rpt"). */
    function print() {
        if (!st.rows.length) { alert('Not Record Found For Display'); return; }
        var w = window.open('', '_blank');
        fetch('/reports/print/grid', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json', Accept: 'application/pdf, text/plain' },
            body: JSON.stringify({ rpt: '253_01-InvRptGatePassInwardRegisterA.rpt', title: null, rows: st.rows.map(function (r) { return r.raw; }) }) })
            .then(function (r) {
                if (r.ok && (r.headers.get('Content-Type') || '').indexOf('application/pdf') >= 0) return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (w) w.location.href = u; else window.open(u); });
                return r.text().then(function (x) { if (w) w.close(); alert(x || ('Print failed (' + r.status + ')')); });
            }).catch(function (e) { if (w) w.close(); alert(e.message); });
    }

    /** btnReset_Click :563. */
    function reset() {
        $id('cmbIsAccept').value = ''; $id('cmbSupplierName').value = '0';
        $id('txtGpFrom').value = ''; $id('txtGPTo').value = '';
        $id('cmbDateType').focus();
        dateTypeFill();
    }
    /** toolStripButton1_Click_1 :592. */
    function refresh() { allDropDown().then(isAcceptFill); }

    /** MakeShortCutKeys :691. */
    function shortcutKeys() {
        alert(['Ctrl+S  For Show', 'Ctrl+E  For Close', 'Ctrl+R  For Refresh', 'Ctrl+N  For New', 'Ctrl + P  For print', 'Ctrl+alt  To Show ShortCut Keys Form',
            'Ctrl+ArrowDown  For Focus On Grid', 'Ctrl+ArrowUp  For Focus On DateType in Filter', 'Ctrl+Space  When Focus On Any Grid To Call Function\'s On Button Or Link'].join('\n'));
    }
    /** frmGatePassReport_KeyDown :644. */
    function keyDown(e) {
        if (!e.ctrlKey) return;
        var k = e.key.toLowerCase();
        if (e.altKey) { e.preventDefault(); shortcutKeys(); return; }
        if (k === 'e') { e.preventDefault(); window.location.href = '/app/Inventory?module=137'; }
        else if (k === 'p') { e.preventDefault(); print(); }
        else if (k === 's') { e.preventDefault(); show(); }
        else if (k === 'n') { e.preventDefault(); reset(); }
        else if (k === 'r') { e.preventDefault(); refresh(); }
        else if (k === 'f5' || k === 'arrowup') { e.preventDefault(); $id('cmbDateType').focus(); }
        else if (k === 'arrowdown') { e.preventDefault(); var tr = $id('grdfrm').tBodies[0].querySelector('tr'); if (tr) tr.focus(); }
    }

    document.addEventListener('DOMContentLoaded', init);

    window.SiT = { show: show, reset: reset, refresh: refresh, print: print, shortcutKeys: shortcutKeys, dateTypeChanged: dateTypeChanged, toggleBranches: toggleBranches };
})();
