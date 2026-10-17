/* ============================================================================================
 * Screen 97 "Stock Close Run" — Architecture.WinApp.Inventory_Definition.Stock_Closing.StockCloseRun
 * (base.Name "StockCloseRun", DocumentTypeId 300). Line refs ":NNN" are StockCloseRun.cs.
 *
 *   MonthlyStockClosing_Load :101   rights + StockCreditAccountBind                   (init)
 *   btnShow_Click :154              GetAvailableStockForMonthlyClosing :172           (show)
 *   grd_CellUpdated :531            OpeningRate → OpeningAmount = Weight / PackEquivalent * OpeningRate
 *   CalculateDifferenceAmount :555  Closing / Opening / Difference labels
 *   btnSave_Click :405              precheck → "Are you sure to Save/Update?" → save → slip if Print Preview
 *   btnnew_Click :575               clear account, remarks, grid, hide AmountsPanel
 *   btnSlip_Click :590 / btnprint_Click :514   300-StockMonthlyClosingSlip.rpt / 103-AcRptPurchaseSalesVoucherSlip.rpt
 *   MonthlyStockClosing_KeyDown :358 / MakeShortCutKeys :618
 * The sibling page (screen 103, MonthlyStockClosing) is the same form with the OpeningRate / RateUom /
 * OpeningAmount columns visible (:261-264 hide them here); data-variant on #frmRoot selects the column set.
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc;
    var root = $id('frmRoot');
    var api = root.getAttribute('data-api');
    var variant = root.getAttribute('data-variant');
    var SLIP_RPT = '300-StockMonthlyClosingSlip.rpt';         // CommonServices.StockMonthlyClosing_Slip :16460
    var VOUCHER_RPT = '103-AcRptPurchaseSalesVoucherSlip.rpt'; // CommonServices.AcRptPurchaseSalesVoucherSlip_103 :5717

    var rights = {};
    var recId = 0;             // RecId — set by show() from row 0's HeaderId, never cleared by New/Save (:230)
    var rows = [];             // dtData
    var filters = {};

    // ------------------------------------------------------------------------------ helpers
    function num(v) { var s = String(v === null || v === undefined ? '' : v).replace(/,/g, '').trim(); if (!s) return 0; var n = Number(s); return isNaN(n) ? 0 : n; }
    function fmt(v, places) { return (Number(v) || 0).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: places }); }
    function fail(e) { alert(e && e.message ? e.message : String(e)); }
    function sum(key) { return rows.reduce(function (a, r) { return a + num(r[key]); }, 0); }

    /* dtData columns :183-206; Visible=false list :251-264; formats :301-320; widths :321-330.
       OpeningRate is the only cell left editable (:265-285 set EditType NoEdit on every other column). */
    var COLS = [
        { key: 'ClosingDate', cap: 'ClosingDate', w: 85 },
        { key: 'ItemName', cap: 'ItemName', w: 200 },
        { key: 'PackUom', cap: 'PackUom', w: 75 },
        { key: 'WareHouseName', cap: 'WareHouseName' },
        { key: 'CropBatch', cap: 'CropBatch' },
        { key: 'JobLotDescription', cap: 'JobLotDescription' },
        { key: 'PackingType', cap: 'PackingType' },
        { key: 'Qty', cap: 'Qty', w: 90, n: 3, total: true },
        { key: 'Weight', cap: 'Weight', w: 100, n: 3, total: true },
        { key: 'Value', cap: 'Value', w: 110, n: 4, total: true },
        { key: 'ClosingRate', cap: 'ClosingRate', w: 95, n: 4 },
        { key: 'OpeningRate', cap: 'OpeningRate', w: 100, n: 4, edit: true, monthlyOnly: true },
        { key: 'RateUom', cap: 'RateUom', w: 75, monthlyOnly: true },
        { key: 'OpeningAmount', cap: 'OpeningAmount', w: 110, n: 4, total: true, monthlyOnly: true }
    ].filter(function (c) { return variant === 'MONTHLY_STOCK_CLOSING' || !c.monthlyOnly; });

    // ------------------------------------------------------------------------------ load
    function init() {
        $id('datAsOnDate').value = C.today();
        renderGrid();
        C.getJson(api + '/lookups').then(function (d) {
            rights = d.rights || {};
            $id('btnSave').disabled = !rights.save;          // :106
            $id('btnprint').disabled = !rights.print;        // :107
            $id('btnSlip').disabled = !rights.print;         // :108
            bindAccounts(d.accounts || []);
            $id('datAsOnDate').focus();                       // :109
        }).catch(fail);
        $id('btnShow').addEventListener('click', show);
        $id('btnSave').addEventListener('click', save);
        $id('btnnew').addEventListener('click', newClick);
        $id('btnSlip').addEventListener('click', slipClick);
        $id('btnprint').addEventListener('click', voucherClick);
        $id('BtnShortCutkeys').addEventListener('click', shortcutKeys);
        document.addEventListener('keydown', keyDown);
    }

    /* StockCreditAccountBind :119 — BindDDL(..., "Id", "AccountTitle", "Difference Account", ZeroIndex true) */
    function bindAccounts(list) {
        var el = $id('CmbDifferenceAccount'), keep = el.value;
        el.innerHTML = '<option value="0"></option>';
        list.forEach(function (a) { el.appendChild(new Option(String(a.AccountTitle), String(a.Id))); });
        if (keep && list.some(function (a) { return String(a.Id) === keep; })) el.value = keep; else el.value = '0';
    }

    // ------------------------------------------------------------------------------ show
    function show() {
        var d = $id('datAsOnDate').value;
        C.withBusy($id('btnShow'), function () {
            return C.getJson(api + '/show' + C.qs({ asOnDate: d })).then(function (res) {
                rows = res.rows || [];
                if (rows.length && num(rows[0].HeaderId) > 0) {                       // :228-233 (row 0 of an existing closing)
                    recId = res.RecId;
                    $id('CmbDifferenceAccount').value = String(res.AccountId);
                    $id('txtRemarks').value = res.Remarks || '';
                }
                filters = {};
                renderGrid();
                $id('AmountsPanel').classList.toggle('is-hidden', rows.length === 0);  // :239-247
                if (rows.length) calculateDifferenceAmount();
            });
        }).catch(fail);
    }

    // ------------------------------------------------------------------------------ grid
    function visibleRows() {
        var keys = Object.keys(filters).filter(function (k) { return filters[k]; });
        return rows.map(function (r, i) { return { r: r, i: i }; }).filter(function (x) {
            return keys.every(function (k) { return String(x.r[k] === null || x.r[k] === undefined ? '' : x.r[k]).toLowerCase().indexOf(filters[k].toLowerCase()) >= 0; });
        });
    }
    function cell(c, v) {
        if (c.n !== undefined) return fmt(v, c.n);
        return v === null || v === undefined ? '' : String(v);
    }
    function renderGrid() {
        var t = $id('grd');
        t.tHead.innerHTML = '<tr>' + COLS.map(function (c) { return '<th' + (c.w ? ' style="min-width:' + c.w + 'px"' : '') + '>' + esc(c.cap) + '</th>'; }).join('') + '</tr>' +
            '<tr class="flt">' + COLS.map(function (c) { return '<td><input type="text" data-f="' + c.key + '" value="' + esc(filters[c.key] || '') + '" placeholder="contains"></td>'; }).join('') + '</tr>';
        var vis = visibleRows();
        t.tBodies[0].innerHTML = vis.map(function (x) {
            return '<tr data-i="' + x.i + '">' + COLS.map(function (c) {
                var v = x.r[c.key];
                if (c.edit) return '<td class="n"><input type="text" class="ed" data-e="' + x.i + '" data-k="' + c.key + '" value="' + esc(v === null || v === undefined ? '' : v) + '"></td>';
                return '<td' + (c.n !== undefined ? ' class="n"' : '') + '>' + esc(cell(c, v)) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        t.tFoot.innerHTML = rows.length ? '<tr>' + COLS.map(function (c) { return '<td>' + (c.total ? fmt(sum(c.key), c.n) : '') + '</td>'; }).join('') + '</tr>' : '';
        $id('lblNavigator').textContent = rows.length ? vis.length + ' of ' + rows.length + ' rows' : '';
        t.tHead.querySelectorAll('[data-f]').forEach(function (inp) {
            inp.oninput = function () { filters[inp.getAttribute('data-f')] = inp.value; var keep = inp; renderGrid(); var again = t.tHead.querySelector('[data-f="' + keep.getAttribute('data-f') + '"]'); if (again) { again.focus(); again.setSelectionRange(again.value.length, again.value.length); } };
        });
        t.tBodies[0].querySelectorAll('[data-e]').forEach(function (inp) {
            inp.onchange = function () { cellUpdated(parseInt(inp.getAttribute('data-e'), 10), inp.getAttribute('data-k'), inp.value); };
        });
        t.tBodies[0].querySelectorAll('tr[data-i]').forEach(function (tr) {
            tr.onclick = function () { t.tBodies[0].querySelectorAll('tr.sel').forEach(function (x) { x.classList.remove('sel'); }); tr.classList.add('sel'); };
        });
    }

    /* grd_CellUpdated :531 — only the OpeningRate column recomputes OpeningAmount. */
    function cellUpdated(i, key, value) {
        var r = rows[i];
        if (!r) return;
        r[key] = num(value);
        if (key === 'OpeningRate') {
            var equivalent = num(r.PackEquivalent);
            r.OpeningAmount = num(r.Weight) / equivalent * num(r.OpeningRate);   // :543 (Infinity/NaN when Equivalent is 0, as on the desktop)
        }
        renderGrid();
        calculateDifferenceAmount();
    }

    /* CalculateDifferenceAmount :555 — "#,##0.####" */
    function calculateDifferenceAmount() {
        if (!rows.length) return;
        var closing = sum('Value'), opening = sum('OpeningAmount');
        $id('txtClosingAmount').textContent = fmt(closing, 4);
        $id('txtOpeningAmount').textContent = fmt(opening, 4);
        $id('txtDifferenceAmount').textContent = fmt(closing - opening, 4);
    }

    // ------------------------------------------------------------------------------ save
    function body() {
        return {
            AsOnDate: $id('datAsOnDate').value,
            AccountId: parseInt($id('CmbDifferenceAccount').value || '0', 10) || 0,
            Remarks: $id('txtRemarks').value,
            rows: rows.map(function (r) {
                var o = {};
                Object.keys(r).forEach(function (k) { o[k] = r[k]; });
                return o;
            })
        };
    }

    /* btnSave_Click :405 */
    function save() {
        if (!rights.save) return;
        var b = body();
        C.postJson(api + '/precheck', b).then(function (pre) {
            if (!confirm(pre.update ? 'Are you sure to Update?' : 'Are you sure to Save?')) return null;   // :432/:438
            return C.postJson(api + '/save', b).then(function (res) {
                alert(res.message);                                              // :494/:498
                rows = []; filters = {};
                renderGrid();                                                    // grd.ClearStructure()
                $id('CmbDifferenceAccount').value = '0';                         // :501
                $id('txtRemarks').value = '';                                    // :502
                if ($id('chkPrintPreview').checked) printSlip(true);             // :503-506
            });
        }).catch(fail);
    }

    /* btnnew_Click :575 */
    function newClick() {
        $id('CmbDifferenceAccount').value = '0';
        $id('txtRemarks').value = '';
        rows = []; filters = {};
        renderGrid();
        $id('AmountsPanel').classList.add('is-hidden');
    }

    // ------------------------------------------------------------------------------ print
    function printGrid(rpt, title, data) {
        var w = window.open('', '_blank');
        fetch('/reports/print/grid', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json', Accept: 'application/pdf, text/plain' }, body: JSON.stringify({ rpt: rpt, title: title, rows: data }) })
            .then(function (r) {
                if (r.ok && (r.headers.get('Content-Type') || '').indexOf('application/pdf') >= 0) return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (w) w.location.href = u; else window.open(u); });
                return r.text().then(function (x) { if (w) w.close(); alert(x || ('Print failed (' + r.status + ')')); });
            }).catch(function (e) { if (w) w.close(); fail(e); });
    }
    /* CommonServices.StockMonthlyClosing_Slip(datAsOnDate.Value) :16432 */
    function printSlip(afterSave) {
        C.getJson(api + '/slip' + C.qs({ recId: recId, asOnDate: $id('datAsOnDate').value, afterSave: afterSave ? 'true' : 'false' }))
            .then(function (data) { printGrid(SLIP_RPT, 'Stock Monthly Closing Slip', data); }).catch(fail);
    }
    function slipClick() { if (rights.print) printSlip(false); }                 // :590
    /* btnprint_Click :514 → AcRptPurchaseSalesVoucherSlip_103(VoucherHeadIdGet(RecId, 300), 300) */
    function voucherClick() {
        if (!rights.print) return;
        C.getJson(api + '/voucher' + C.qs({ recId: recId }))
            .then(function (data) { printGrid(VOUCHER_RPT, 'Voucher Slip', data); }).catch(fail);
    }

    // ------------------------------------------------------------------------------ keys
    /* MakeShortCutKeys :618 */
    function shortcutKeys() {
        alert(['Ctrl+E\tFor Close', 'Ctrl+N\tFor New', 'Ctrl+R\tFor Refresh', 'Ctrl+S\tFor Save', 'Alt+1\tFor Print Slip', 'Alt+2\tFor Print Voucher',
            'Ctrl+F5\tFor Focus on AsOnDate', 'Ctrl+alt\tTo Show ShortCut Keys Form', 'Ctrl+ArrowDown\tFor Focus On Detail Grid', 'Ctrl+ArrowUp\tFor Focus On Difference Account'].join('\n'));
    }
    /* MonthlyStockClosing_KeyDown :358 */
    function keyDown(e) {
        var k = (e.key || '').toLowerCase();
        if ((e.ctrlKey && k === 'e') || k === 'escape') { e.preventDefault(); window.location.href = '/modules'; return; }
        if (e.ctrlKey && e.shiftKey && k === 'g') { e.preventDefault(); var gb = $id('gbMount'); if (gb) gb.style.visibility = 'visible'; return; }
        if (e.ctrlKey && k === 'arrowdown') { e.preventDefault(); var f = $id('grd').querySelector('input, tbody tr'); if (f) f.focus(); return; }
        if (e.ctrlKey && k === 'f5') { e.preventDefault(); $id('datAsOnDate').focus(); return; }
        if (e.ctrlKey && k === 'arrowup') { e.preventDefault(); $id('CmbDifferenceAccount').focus(); return; }
        if (e.ctrlKey && k === 's' && rights.save) { e.preventDefault(); save(); return; }
        if (e.altKey && (k === '1') && rights.print) { e.preventDefault(); slipClick(); return; }
        if (e.altKey && (k === '2') && rights.print) { e.preventDefault(); voucherClick(); return; }
        if (e.ctrlKey && e.altKey && k === 'alt') { e.preventDefault(); shortcutKeys(); }
    }

    init();
})();
