/* ============================================================================================
 * Production Comparison Report — FoodProductionComparisonRpt.cs, ScreenDefinition 308.
 *
 *   production types  Sp_ProductionType_GetAllMethod          (no parameters at all)
 *   both tabs' lists  USP_GetDataForDropDownFromFoodProduction
 *   pack uom          Sp_UOMSchedule_GetAllMethod @Activity='ReadByItemID'
 *   comparison        SpInvFoodProductionComparisons_Rpt
 *   input detail      Sp_InvFoodProductionIssuanceGrnWiseByJobOrderId_rpt
 *   gain / loss       SpProduction_GainLossSummary_Rpt
 *
 * Two tabs, each with its own branch tick-list and its own pickers, sharing one grid — the same
 * arrangement as the desktop's tabControl1.
 *
 * The Summary tab's Job Order picker is EMPTY on purpose. ComboBindSummary builds a table for it
 * but its splitting loop only ever matches "Plant" and "ItemName", so the desktop's picker is
 * always empty too and both summary reports run with no job-order filter. Filling it here would
 * return different rows from the desktop.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/production/reports/production-comparison';

    var rows = [];
    var cols = [];
    var shape = 'comparison';      /* comparison | detail | gainloss */
    var branches = [];

    /* GridSetting:691 — added and then hidden. */
    var COMPARISON_HIDDEN = ['Id'];

    var NUMERIC = ['Input', 'BP_Output', 'FG_Output', 'Short_Gain', 'BP_Recovery', 'FG_Recovery',
                   'Total_Recovery', 'OrderNo', 'GrnNo', 'InvoiceNo', 'IssueQty', 'IssueWeight',
                   'ItemQty', 'GrossWeight', 'EBWPerUnit', 'EBWTotal', 'AdLsWeight',
                   'StockWeight', 'NetBillWeight', 'ItemAmount', 'ItemNetAmount',
                   'CarriageAmount', 'ProductionQty', 'ProductionWeight'];

    /* AggregateFunction 2 = Sum. grdInputSetting:1040, grdGainLossSummarySetting. */
    var AGGREGATES = {
        comparison: {},
        detail:   { IssueQty: 'sum', IssueWeight: 'sum', ItemAmount: 'sum', ItemNetAmount: 'sum' },
        gainloss: { IssueQty: 'sum', IssueWeight: 'sum', ProductionQty: 'sum', ProductionWeight: 'sum' }
    };

    var DATE_COLUMNS = ['PlanDate', 'GpDate', 'GrnDate'];

    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function box(m) { window.alert(m); }
    function show(el, on) { if (el) el.classList[on ? 'remove' : 'add']('cx-hidden'); }

    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function ci(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        for (var k in row) {
            if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === name.toLowerCase()) {
                return row[k];
            }
        }
        return '';
    }
    function has(list, s) {
        for (var i = 0; i < list.length; i++) if (list[i].toLowerCase() === s.toLowerCase()) return true;
        return false;
    }
    function num(v) {
        var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, ''));
        return isNaN(n) ? 0 : n;
    }
    function int(v) {
        var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/[^0-9-]/g, ''), 10);
        return isNaN(n) ? 0 : n;
    }
    function fmt(v, decimals) {
        if (v === null || v === undefined || v === '') return '';
        var n = (typeof v === 'number') ? v : parseFloat(String(v).replace(/,/g, ''));
        if (isNaN(n)) return String(v);
        if (!isFinite(n)) return String(n);
        var s = n.toFixed(decimals);
        if (s.indexOf('.') >= 0) s = s.replace(/0+$/, '').replace(/\.$/, '');
        var parts = s.split('.');
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return parts.join('.');
    }
    function shortDate(v) {
        if (v === null || v === undefined || v === '') return '';
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v));
        return m ? (m[3] + '/' + m[2] + '/' + m[1]) : String(v);
    }
    function isoDate(v) {
        if (!v) return '';
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v));
        return m ? m[0] : '';
    }

    /* Disable → spinner → ignore repeat clicks → re-enable on success AND failure. */
    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) {
            if (b.disabled || b.classList.contains('is-busy')) return;
            b.disabled = true;
            b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } };
        var p;
        try { p = fn(); } catch (e) { done(); throw e; }
        if (p && typeof p.then === 'function') p.then(done, done); else done();
        return p;
    }

    function getJson(url) {
        return fetch(url, { headers: { 'Accept': 'application/json' }, credentials: 'same-origin' })
            .then(function (r) {
                return r.text().then(function (t) {
                    var body = null;
                    try { body = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
                    if (!r.ok) throw new Error((body && body.message) || ('Request failed (' + r.status + ')'));
                    return body;
                });
            });
    }

    // ------------------------------------------------------------------ tick-lists / tabs

    function ticked(hostId) {
        var out = [];
        var boxes = $id(hostId).querySelectorAll('input[type="checkbox"]');
        for (var i = 0; i < boxes.length; i++) if (boxes[i].checked) out.push(boxes[i].value);
        return out;
    }
    function tickedNames(hostId) {
        var out = [];
        var boxes = $id(hostId).querySelectorAll('input[type="checkbox"]');
        for (var i = 0; i < boxes.length; i++) {
            if (boxes[i].checked) out.push(boxes[i].getAttribute('data-name'));
        }
        return out;
    }
    function renderTicks(hostId, textId, defaultId, onClose) {
        var host = $id(hostId);
        host.innerHTML = branches.map(function (b) {
            var id = ci(b, 'BranchId'), name = ci(b, 'BranchName');
            var on = String(id) === String(defaultId) ? ' checked' : '';
            return '<label class="cx-multi-row">'
                 + '<input type="checkbox" value="' + esc(id) + '" data-name="' + esc(name) + '"' + on + '>'
                 + '<span>' + esc(name) + '</span></label>';
        }).join('');
        host.onchange = function () { $id(textId).value = tickedNames(hostId).join(','); };
        $id(textId).value = tickedNames(hostId).join(',');
        host.setAttribute('data-onclose', onClose);
    }
    function toggle(id) { $id(id).classList.toggle('is-open'); }

    function tab(which) {
        var cmp = which === 'comparison';
        show($id('tabComparison'), cmp);
        show($id('tabSummary'), !cmp);
        $id('tabComparisonBtn').classList[cmp ? 'add' : 'remove']('is-active');
        $id('tabSummaryBtn').classList[cmp ? 'remove' : 'add']('is-active');
        /* tabControl1_SelectedIndexChanged:608 — switching tabs clears the shared grid, since
           the two tabs' grids have nothing in common. */
        rows = []; cols = [];
        $id('gridHead').innerHTML = '';
        $id('gridBody').innerHTML = '';
        $id('lblCount').textContent = '';
        show($id('panelValues'), false);
        show($id('noRateNote'), false);
        $id('lblGridTitle').textContent = cmp ? 'Comparison' : 'Summary';
    }

    // ------------------------------------------------------------------ pickers

    function fill(selectId, list, valueKey, nameKey, blankFirst) {
        var head = blankFirst ? '<option value=""></option>' : '';
        $id(selectId).innerHTML = head + (list || []).map(function (r) {
            return '<option value="' + esc(ci(r, valueKey)) + '">' + esc(ci(r, nameKey)) + '</option>';
        }).join('');
    }

    function loadForm() {
        return getJson(api + '/lookups').then(function (d) {
            branches = (d && d.branches) || [];
            renderTicks('cmpBranchRows', 'txtCmpBranch', d && d.defaultBranchId, 'comparison');
            renderTicks('sumBranchRows', 'txtSumBranch', d && d.defaultBranchId, 'summary');

            /* ProductionTypeBind uses BindDDLNew - one visible column, and NO blank row. The
               form then activates Rows[1], so the second entry is the opening selection. */
            fill('cmbProductionType', d && d.productionTypes, 'Id', 'ProductionTypeDescription', false);
            var pt = $id('cmbProductionType');
            if (pt.options.length > 1) pt.selectedIndex = 1;

            /* frmGPOutward_Load:216 - From opens at the active financial year's start. */
            var start = isoDate(d && d.financialYearStart);
            if (start) $id('datSumFrom').value = start;

            return Promise.all([loadComparisonLists(), loadSummaryLists()]);
        });
    }

    function loadComparisonLists() {
        return getJson(api + '/comparison-lookups?branchIds='
                       + encodeURIComponent(ticked('cmpBranchRows').join(',')))
            .then(function (d) {
                fill('cmbCmpJobOrder', d && d.jobOrders, 'Id', 'Name', true);
                fill('cmbCmpPlant',    d && d.plants,    'Id', 'Name', true);
            });
    }

    function loadSummaryLists() {
        return getJson(api + '/summary-lookups?branchIds='
                       + encodeURIComponent(ticked('sumBranchRows').join(',')))
            .then(function (d) {
                /* Always empty on the desktop — see the header note. */
                fill('cmbSumJobOrder', d && d.jobOrders, 'Id', 'Name', true);
                fill('cmbSumPlant',    d && d.plants,    'Id', 'Name', true);
                fill('cmbSumItem',     d && d.items,     'Id', 'Name', true);
                $id('cmbSumPackUom').innerHTML = '';
            });
    }

    /** CmbItemName_Leave:461 — the pack UOM list follows the item. */
    function itemChanged() {
        var id = val('cmbSumItem');
        var uom = $id('cmbSumPackUom');
        if (!id) { uom.innerHTML = ''; return; }
        return getJson(api + '/pack-uoms?itemId=' + encodeURIComponent(id))
            .then(function (list) {
                /* ZeroIndex:false — this one has no blank row. */
                fill('cmbSumPackUom', list, 'Id', 'UOMCode', false);
                if (!(list || []).length) uom.innerHTML = '';
            }).catch(function (e) { uom.innerHTML = ''; box(e.message); });
    }

    // ------------------------------------------------------------------ the two tabs' reports

    function showComparison() {
        return busy('btnShowComparison', function () {
            var pt = $id('cmbProductionType');
            var type = pt.selectedIndex >= 0 ? pt.options[pt.selectedIndex].text : '';
            if (!type) { box('Select a production type'); return; }

            /* The procedure takes the production type's CAPTION, not its id. */
            var q = ['productionType=' + encodeURIComponent(type),
                     'docNoFrom=' + int(val('txtDocNoFrom')),
                     'docNoTo=' + int(val('txtDocNoTo')),
                     'plantId=' + encodeURIComponent(val('cmbCmpPlant') || '0'),
                     'jobOrderId=' + encodeURIComponent(val('cmbCmpJobOrder') || '0')];

            return getJson(api + '?' + q.join('&')).then(function (data) {
                shape = 'comparison';
                rows = data || [];
                $id('lblGridTitle').textContent = 'Comparison';
                show($id('panelValues'), false);
                show($id('noRateNote'), false);
                render(COMPARISON_HIDDEN);
            }).catch(fail);
        });
    }

    function showSummary() {
        return busy('btnShowSummary', function () {
            var mode = document.querySelector('input[name="sumMode"]:checked');
            var m = mode ? mode.value : 'detail';
            var q = ['mode=' + encodeURIComponent(m),
                     'jobOrderId=' + encodeURIComponent(val('cmbSumJobOrder') || '0'),
                     'plantId=' + encodeURIComponent(val('cmbSumPlant') || '0'),
                     'itemId=' + encodeURIComponent(val('cmbSumItem') || '0'),
                     'packUomId=' + encodeURIComponent(val('cmbSumPackUom') || '0')];
            /* The desktop sets both dates unconditionally on this tab. */
            if (val('datSumFrom')) q.push('fromDate=' + encodeURIComponent(val('datSumFrom')));
            if (val('datSumTo'))   q.push('toDate=' + encodeURIComponent(val('datSumTo')));

            return getJson(api + '/summary?' + q.join('&')).then(function (d) {
                shape = (d && d.mode) === 'gainloss' ? 'gainloss' : 'detail';
                rows = (d && d.rows) || [];
                $id('lblGridTitle').textContent =
                    shape === 'gainloss' ? 'Gain / Loss Summary' : 'Input Detail';
                show($id('noRateNote'),
                     shape === 'detail' && !(d && d.canSeeRateAndAmount));
                renderGainLoss(shape === 'gainloss' ? (d && d.totals) : null);
                render([]);
            }).catch(fail);
        });
    }

    function fail(e) {
        rows = []; cols = [];
        $id('gridHead').innerHTML = '';
        $id('gridBody').innerHTML = '';
        $id('lblCount').textContent = '';
        show($id('panelValues'), false);
        box(e.message);
    }

    /**
     * Production_GainLossSummary:926 — the sign of the difference decides the word and its
     * colour, and the percentage is only computed when the difference is not zero.
     */
    function renderGainLoss(t) {
        /* LablesStatus(false) when the report came back empty. */
        var on = !!t && rows.length > 0;
        show($id('panelValues'), on);
        if (!on) return;
        $id('lblSumofTotalIssue').textContent = fmt(t.totalIssueWeight, 0);
        $id('lblSumOfOutPut').textContent = fmt(t.totalProductionWeight, 0);
        /* "#,#;(#,#);0" — a negative difference is shown in brackets. */
        var d = num(t.difference);
        $id('lblGainLossDiff').textContent = d === 0 ? '0'
            : (d < 0 ? '(' + fmt(Math.abs(d), 0) + ')' : fmt(d, 0));

        var st = $id('lblGainLossStatus'), pc = $id('lblPercent');
        st.textContent = t.status || '';
        st.className = 'cx-status ' + (t.status === 'Loss' ? 'cx-loss'
                                     : t.status === 'Gain' ? 'cx-gain' : 'cx-nill');
        pc.className = st.className;
        pc.textContent = (t.percent === undefined || t.percent === null) ? '' : (t.percent + ' %');
    }

    function render(hidden) {
        var head = $id('gridHead'), body = $id('gridBody');
        if (!rows.length) {
            cols = [];
            head.innerHTML = '';
            body.innerHTML = '<tr><td>No records</td></tr>';
            $id('lblCount').textContent = '0 record(s)';
            return;
        }
        cols = Object.keys(rows[0]).filter(function (c) { return !has(hidden || [], c); });
        head.innerHTML = cols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('');
        body.innerHTML = rows.map(function (r) {
            return '<tr>' + cols.map(function (c) { return td(c, r[c]); }).join('') + '</tr>';
        }).join('') + grandTotal();
        $id('lblCount').textContent = rows.length + ' record(s)';
    }

    function cellText(col, v) {
        if (has(DATE_COLUMNS, col)) return shortDate(v);
        if (has(NUMERIC, col)) return fmt(v, 2);
        return v;
    }

    function td(col, v) {
        return '<td' + (has(NUMERIC, col) ? ' class="num"' : '') + '>'
             + esc(cellText(col, v)) + '</td>';
    }

    function grandTotal() {
        var agg = AGGREGATES[shape] || {};
        var any = false;
        for (var k in agg) { if (Object.prototype.hasOwnProperty.call(agg, k)) { any = true; break; } }
        if (!any) return '';
        return '<tr class="cx-grand">' + cols.map(function (c, i) {
            if (!Object.prototype.hasOwnProperty.call(agg, c)) {
                return i === 0 ? '<td>Total</td>' : '<td></td>';
            }
            var t = 0;
            rows.forEach(function (r) { t += num(r[c]); });
            return '<td class="num">' + esc(fmt(t, 2)) + '</td>';
        }).join('') + '</tr>';
    }

    // ------------------------------------------------------------------ chrome

    function csvCell(v) {
        var s = (v === null || v === undefined) ? '' : String(v);
        return /[",\r\n]/.test(s) ? '"' + s.replace(/"/g, '""') + '"' : s;
    }

    function exportCsv() {
        if (!rows.length) { box('Nothing to export yet.'); return; }
        var lines = [cols.map(csvCell).join(',')];
        rows.forEach(function (r) {
            lines.push(cols.map(function (c) { return csvCell(cellText(c, r[c])); }).join(','));
        });
        var blob = new Blob(['﻿' + lines.join('\r\n')], { type: 'text/csv;charset=utf-8;' });
        var a = document.createElement('a');
        a.href = URL.createObjectURL(blob);
        a.download = 'production-comparison-' + shape + '.csv';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        setTimeout(function () { URL.revokeObjectURL(a.href); }, 1000);
    }

    /** BtnRefreshComparison_Click:595 / BtnRefreshSummary_Click:582. */
    function refresh() {
        return busy(null, function () {
            return loadForm().catch(function (e) { box(e.message); });
        });
    }

    function toggleFullscreen(boxId) {
        var el = $id(boxId);
        if (el) el.classList.toggle('is-fullscreen');
    }

    /** GpsNoFrom_KeyPress:1261 / GpsNoTo_KeyPress:1273 — digits only in the doc-no boxes. */
    function digitsOnly(el) {
        el.addEventListener('keypress', function (e) {
            if (e.key.length === 1 && !/[0-9]/.test(e.key)) e.preventDefault();
        });
        el.addEventListener('input', function () {
            var c = el.value.replace(/[^0-9]/g, '');
            if (c !== el.value) el.value = c;
        });
    }

    function boot() {
        digitsOnly($id('txtDocNoFrom'));
        digitsOnly($id('txtDocNoTo'));

        var now = new Date();
        $id('datSumTo').value = now.getFullYear() + '-'
            + String(now.getMonth() + 1).padStart(2, '0') + '-'
            + String(now.getDate()).padStart(2, '0');

        document.addEventListener('keydown', function (e) {
            if (e.key === 'Enter' && e.target && e.target.tagName !== 'BUTTON'
                && e.target.tagName !== 'TEXTAREA') {
                e.preventDefault();
            }
        });
        /* cmbBranchNameComparison_Leave:483 / CmbBranchNameSummary_Leave:503 — closing a branch
           list rebuilds that tab's pickers. */
        document.addEventListener('click', function (e) {
            [['cmpBranchBox', loadComparisonLists], ['sumBranchBox', loadSummaryLists]]
                .forEach(function (pair) {
                    var b = $id(pair[0]);
                    if (b && b.classList.contains('is-open') && !b.contains(e.target)) {
                        b.classList.remove('is-open');
                        pair[1]().catch(function (err) { box(err.message); });
                    }
                });
        });

        loadForm().catch(function (e) { box(e.message); });
    }

    window.ProductionComparison = {
        tab: tab,
        toggle: toggle,
        itemChanged: itemChanged,
        showComparison: showComparison,
        showSummary: showSummary,
        exportCsv: exportCsv,
        refresh: refresh,
        toggleFullscreen: toggleFullscreen
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
    else boot();
}());
