/* ============================================================================================
 * Production Comparison Report - FoodProductionComparisonRpt.cs, ScreenDefinition 308.
 *
 *   production types  Sp_ProductionType_GetAllMethod          (no parameters at all)
 *   both tabs' lists  USP_GetDataForDropDownFromFoodProduction (split on Activity)
 *   pack uom          Sp_UOMSchedule_GetAllMethod @Activity='ReadByItemID'
 *   comparison        SpInvFoodProductionComparisons_Rpt       -> 604 print, row Print -> 602
 *   input detail      Sp_InvFoodProductionIssuanceGrnWiseByJobOrderId_rpt -> 607 print
 *   gain / loss       SpProduction_GainLossSummary_Rpt
 *
 * Two tabs, each with its OWN toolbar, pickers and grid (DataGridHistory / grdInput), so a tab
 * switch keeps the other tab's result - as the desktop does.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/production/reports/production-comparison';
    var K = window.ReportKit;

    var cmp = { rows: [], cols: [], args: null };             /* DataGridHistory / dtReg */
    var sum = { rows: [], cols: [], args: null, shape: 'detail' };  /* grdInput / dtInput / dtGainLoss */
    var branches = [];

    /* Column formats (GridSetting:688-715, grdInputSetting:992-1037, gain/loss :1066):
       p2  = .NET "0,0" (whole, thousands, two digits minimum)   n2 = "#,##0.##"
       raw = no FormatString - shown as it comes, left aligned. */
    var FMT = {
        comparison: { Input: 'p2', BP_Output: 'p2', FG_Output: 'p2', Short_Gain: 'p2', BP_Recovery: 'p2',
                      FG_Recovery: 'p2', Total_Recovery: 'p2' },
        detail:     { ItemQty: 'p2', GrossWeight: 'n2', EBWPerUnit: 'n2', EBWTotal: 'n2', AdLsWeight: 'n2',
                      StockWeight: 'n2', IssueWeight: 'n2', IssueQty: 'n2', ItemAmount: 'n2', ItemNetAmount: 'n2' },
        gainloss:   { IssueQty: 'p2', IssueWeight: 'n2', ProductionQty: 'n2', ProductionWeight: 'n2' }
    };
    /* Columns carrying a Sum (totals "#,##0.##"). */
    var SUMS = {
        comparison: ['Input', 'BP_Output', 'FG_Output', 'Short_Gain', 'BP_Recovery', 'FG_Recovery', 'Total_Recovery'],
        detail:     ['ItemQty', 'GrossWeight', 'EBWPerUnit', 'EBWTotal', 'AdLsWeight', 'StockWeight', 'IssueWeight',
                     'IssueQty', 'ItemAmount', 'ItemNetAmount'],
        gainloss:   ['IssueQty', 'IssueWeight', 'ProductionQty', 'ProductionWeight']
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
        for (var i = 0; i < boxes.length; i++) if (boxes[i].checked) out.push(boxes[i].getAttribute('data-name'));
        return out;
    }
    function renderTicks(hostId, textId, defaultId) {
        var host = $id(hostId);
        var keep = ticked(hostId);
        host.innerHTML = branches.map(function (b) {
            var id = ci(b, 'BranchId'), name = ci(b, 'BranchName');
            var on = (keep.length ? keep.indexOf(String(id)) >= 0 : String(id) === String(defaultId)) ? ' checked' : '';
            return '<label class="cx-multi-row">'
                 + '<input type="checkbox" value="' + esc(id) + '" data-name="' + esc(name) + '"' + on + '>'
                 + '<span>' + esc(name) + '</span></label>';
        }).join('');
        host.onchange = function () { $id(textId).value = tickedNames(hostId).join(','); };
        $id(textId).value = tickedNames(hostId).join(',');
    }
    function all(hostId, textId, on) {
        $id(hostId).querySelectorAll('input[type="checkbox"]').forEach(function (c) { c.checked = on; });
        $id(textId).value = tickedNames(hostId).join(',');
    }
    function toggle(id) { $id(id).classList.toggle('is-open'); }

    /** tabControl1_SelectedIndexChanged:592 - only entering Summary does anything: focus From,
     *  InPut_Detail radio, totals panel hidden. Neither grid is cleared. */
    function tab(which) {
        var isCmp = which === 'comparison';
        show($id('tabComparison'), isCmp);
        show($id('tabSummary'), !isCmp);
        $id('tabComparisonBtn').classList[isCmp ? 'add' : 'remove']('is-active');
        $id('tabSummaryBtn').classList[isCmp ? 'remove' : 'add']('is-active');
        if (!isCmp) {
            setMode('detail');
            show($id('panelValues'), false);
            $id('datSumFrom').focus();
        }
        $id('lblCount').textContent = (isCmp ? cmp.rows.length : sum.rows.length) + ' record(s)';
    }
    function currentTab() { return $id('tabSummary').classList.contains('cx-hidden') ? 'comparison' : 'summary'; }

    function setMode(m) {
        var r = document.querySelector('input[name="sumMode"][value="' + m + '"]');
        if (r) r.checked = true;
    }

    // ------------------------------------------------------------------ pickers

    function fill(selectId, list, valueKey, nameKey, blankFirst) {
        var el = $id(selectId), keep = el.value;
        var head = blankFirst ? '<option value=""></option>' : '';
        el.innerHTML = head + (list || []).map(function (r) {
            return '<option value="' + esc(ci(r, valueKey)) + '">' + esc(ci(r, nameKey)) + '</option>';
        }).join('');
        el.value = keep;
        if (el.value !== keep) el.value = blankFirst ? '' : (el.options.length ? el.options[0].value : '');
    }

    function loadForm() {
        return getJson(api + '/lookups').then(function (d) {
            branches = (d && d.branches) || [];
            renderTicks('cmpBranchRows', 'txtCmpBranch', d && d.defaultBranchId);
            renderTicks('sumBranchRows', 'txtSumBranch', d && d.defaultBranchId);
            /* ProductionTypeBind: no blank row; Rows[1] is activated on load. */
            fill('cmbProductionType', d && d.productionTypes, 'Id', 'ProductionTypeDescription', false);
            var pt = $id('cmbProductionType');
            if (pt.options.length > 1) pt.selectedIndex = 1;
            var start = isoDate(d && d.financialYearStart);
            if (start) $id('datSumFrom').value = start;
            return Promise.all([loadComparisonLists(), loadSummaryLists()]);
        });
    }

    function loadComparisonLists() {
        return getJson(api + '/comparison-lookups?branchIds=' + encodeURIComponent(ticked('cmpBranchRows').join(',')))
            .then(function (d) {
                fill('cmbCmpJobOrder', d && d.jobOrders, 'Id', 'Name', true);
                fill('cmbCmpPlant',    d && d.plants,    'Id', 'Name', true);
            });
    }

    function loadSummaryLists() {
        return getJson(api + '/summary-lookups?branchIds=' + encodeURIComponent(ticked('sumBranchRows').join(',')))
            .then(function (d) {
                /* ComboBindSummary:385 fills the Job Order picker too (Activity "JobOrder"). */
                fill('cmbSumJobOrder', d && d.jobOrders, 'Id', 'Name', true);
                fill('cmbSumPlant',    d && d.plants,    'Id', 'Name', true);
                fill('cmbSumItem',     d && d.items,     'Id', 'Name', true);
            });
    }

    /** CmbItemName_Leave:461 - the pack UOM list follows the item. ZeroIndex:false and no value
     *  set, so nothing is selected and @PackUomId is not sent until one is picked. */
    function itemChanged() {
        var id = val('cmbSumItem');
        var uom = $id('cmbSumPackUom');
        if (!id) { uom.innerHTML = ''; return; }
        return getJson(api + '/pack-uoms?itemId=' + encodeURIComponent(id)).then(function (list) {
            uom.innerHTML = '<option value=""></option>' + (list || []).map(function (r) {
                return '<option value="' + esc(ci(r, 'Id')) + '">' + esc(ci(r, 'UOMCode')) + '</option>';
            }).join('');
            uom.value = '';
        }).catch(function (e) { uom.innerHTML = ''; box(e.message); });
    }

    // ------------------------------------------------------------------ Comparison tab

    function showComparison() {
        return busy('btnShowComparison', function () { return runComparison(); });
    }
    function runComparison() {
        var pt = $id('cmbProductionType');
        var type = pt.selectedIndex >= 0 ? pt.options[pt.selectedIndex].text : '';
        if (!type) { box('Select a production type'); return; }
        var args = { productionType: type, docNoFrom: int(val('txtDocNoFrom')), docNoTo: int(val('txtDocNoTo')),
                     plantId: int(val('cmbCmpPlant')), jobOrderId: int(val('cmbCmpJobOrder')) };
        /* The procedure takes the production type's CAPTION, not its id. */
        var q = ['productionType=' + encodeURIComponent(type), 'docNoFrom=' + args.docNoFrom,
                 'docNoTo=' + args.docNoTo, 'plantId=' + args.plantId, 'jobOrderId=' + args.jobOrderId];
        return getJson(api + '?' + q.join('&')).then(function (data) {
            cmp.rows = data || []; cmp.args = args;
            renderGrid('cmp', 'comparison', cmp, ['Id'], true);
        }).catch(function (e) { cmp.rows = []; cmp.args = null; renderGrid('cmp', 'comparison', cmp, ['Id'], true); box(e.message); });
    }

    /** reset:511 - doc numbers and plant cleared, Production Type back to Rows[1], and the
     *  grid re-run (a desktop quirk, kept). */
    function resetComparison() {
        setVal('txtDocNoFrom', ''); setVal('txtDocNoTo', ''); setVal('cmbCmpPlant', '');
        var pt = $id('cmbProductionType');
        if (pt.options.length > 1) pt.selectedIndex = 1;
        return busy('btnShowComparison', function () { return runComparison(); });
    }

    /** BtnRefreshComparison_Click:582 - BranchesFill + ComboBindComparison. */
    function refreshComparison() {
        return busy('btnRefreshComparison', function () {
            return getJson(api + '/lookups').then(function (d) {
                branches = (d && d.branches) || [];
                renderTicks('cmpBranchRows', 'txtCmpBranch', d && d.defaultBranchId);
                return loadComparisonLists();
            }).catch(function (e) { box(e.message); });
        });
    }

    /** toolStripButton1_Click:1162 - dtReg into 604. */
    function print604() {
        if (!cmp.args || !cmp.rows.length) { box('Record Not Found For Display'); return; }
        return window.CrystalPrint.open('pc-604', cmp.args, 'btnPrint604');
    }

    /** DataGridHistory_ColumnButtonClick:730 - the row Print column. */
    function printRow(i) {
        var r = cmp.rows[i];
        if (!r) return;
        return window.CrystalPrint.open('pc-602-row', { id: int(ci(r, 'Id')) });
    }

    // ------------------------------------------------------------------ Summary tab

    function mode() {
        var m = document.querySelector('input[name="sumMode"]:checked');
        return m ? m.value : 'detail';
    }

    function showSummary() {
        return busy('btnShowSummary', function () {
            var m = mode();
            var args = { id: int(val('cmbSumJobOrder')), jobOrderId: int(val('cmbSumJobOrder')),
                         plantId: int(val('cmbSumPlant')), itemId: int(val('cmbSumItem')),
                         packUomId: int(val('cmbSumPackUom')),
                         fromDate: val('datSumFrom'), toDate: val('datSumTo') };
            var q = ['mode=' + encodeURIComponent(m), 'jobOrderId=' + args.jobOrderId, 'plantId=' + args.plantId,
                     'itemId=' + args.itemId, 'packUomId=' + args.packUomId];
            /* Both dates are set unconditionally on this tab. */
            if (args.fromDate) q.push('fromDate=' + encodeURIComponent(args.fromDate));
            if (args.toDate) q.push('toDate=' + encodeURIComponent(args.toDate));
            return getJson(api + '/summary?' + q.join('&')).then(function (d) {
                sum.shape = (d && d.mode) === 'gainloss' ? 'gainloss' : 'detail';
                sum.rows = (d && d.rows) || [];
                sum.args = args;
                show($id('noRateNote'), sum.shape === 'detail' && !(d && d.canSeeRateAndAmount));
                renderGainLoss(sum.shape === 'gainloss' ? (d && d.totals) : null);
                renderGrid('sum', sum.shape, sum, [], false);
            }).catch(function (e) {
                sum.rows = []; sum.args = null;
                show($id('panelValues'), false);
                renderGrid('sum', sum.shape, sum, [], false);
                box(e.message);
            });
        });
    }

    /** btnNewInput_Click:539 - job order cleared, grid cleared, InPut_Detail, totals hidden. */
    function resetSummary() {
        setVal('cmbSumJobOrder', '');
        clearSummaryGrid();
        setMode('detail');
        show($id('panelValues'), false);
    }
    function clearSummaryGrid() {
        sum.rows = []; sum.args = null;
        $id('sumHead').innerHTML = ''; $id('sumBody').innerHTML = ''; $id('sumFoot').innerHTML = '';
        if (currentTab() === 'summary') $id('lblCount').textContent = '';
    }

    /** BtnRefreshSummary_Click:568 - BranchesFill + ComboBindSummary. */
    function refreshSummary() {
        return busy('btnRefreshSummary', function () {
            return getJson(api + '/lookups').then(function (d) {
                branches = (d && d.branches) || [];
                renderTicks('sumBranchRows', 'txtSumBranch', d && d.defaultBranchId);
                return loadSummaryLists();
            }).catch(function (e) { box(e.message); });
        });
    }

    /** btnPrint607Register_Click:1181 - dtInput into 607. */
    function print607() {
        if (sum.shape !== 'detail' || !sum.args || !sum.rows.length) { box('Record Not Found For Display'); return; }
        var a = sum.args;
        return window.CrystalPrint.open('prod-607', { id: a.jobOrderId, plantId: a.plantId, itemId: a.itemId,
                                                      packUomId: a.packUomId, fromDate: a.fromDate, toDate: a.toDate },
                                        'btnPrint607');
    }

    /** Production_GainLossSummary:926 - word, colour and percentage from the difference. */
    function renderGainLoss(t) {
        var on = !!t && sum.rows.length > 0;
        show($id('panelValues'), on);
        if (!on) return;
        $id('lblSumofTotalIssue').textContent = fmt(t.totalIssueWeight, 0);
        $id('lblSumOfOutPut').textContent = fmt(t.totalProductionWeight, 0);
        var d = num(t.difference);
        $id('lblGainLossDiff').textContent = d === 0 ? '0' : (d < 0 ? '(' + fmt(Math.abs(d), 0) + ')' : fmt(d, 0));
        var st = $id('lblGainLossStatus'), pc = $id('lblPercent');
        st.textContent = t.status || '';
        st.className = 'cx-status ' + (t.status === 'Loss' ? 'cx-loss' : t.status === 'Gain' ? 'cx-gain' : 'cx-nill');
        pc.className = st.className;
        pc.textContent = (t.percent === undefined || t.percent === null) ? '' : (t.percent + ' %');
    }

    // ------------------------------------------------------------------ grids

    function cellText(shape, col, v) {
        if (has(DATE_COLUMNS, col)) return shortDate(v);
        var f = (FMT[shape] || {})[col];
        if (f === 'p2') return K.pad2(v);
        if (f === 'n2') return K.num(v, 2);
        return v;
    }

    function renderGrid(prefix, shape, st, hidden, printColumn) {
        var head = $id(prefix + 'Head'), body = $id(prefix + 'Body'), foot = $id(prefix + 'Foot');
        foot.innerHTML = '';
        if (!st.rows.length) {
            st.cols = [];
            head.innerHTML = '';
            body.innerHTML = '<tr><td>No records</td></tr>';
            $id('lblCount').textContent = '0 record(s)';
            return;
        }
        st.cols = Object.keys(st.rows[0]).filter(function (c) { return !has(hidden || [], c); });
        var fmts = FMT[shape] || {};
        head.innerHTML = st.cols.map(function (c) {
            return '<th' + (fmts[c] ? ' class="num"' : '') + '>' + esc(c) + '</th>';
        }).join('') + (printColumn ? '<th>Print</th>' : '');
        body.innerHTML = st.rows.map(function (r, i) {
            return '<tr>' + st.cols.map(function (c) {
                return '<td' + (fmts[c] ? ' class="num"' : '') + '>' + esc(cellText(shape, c, r[c])) + '</td>';
            }).join('') + (printColumn ? '<td><button type="button" class="cx-cellbtn" data-print="' + i + '">Print</button></td>' : '') + '</tr>';
        }).join('');
        var sums = SUMS[shape] || [];
        foot.innerHTML = '<tr class="cx-grand">' + st.cols.map(function (c, i) {
            if (sums.indexOf(c) < 0) return i === 0 ? '<td>Total</td>' : '<td></td>';
            var t = 0;
            st.rows.forEach(function (r) { t += num(r[c]); });
            return '<td class="num">' + esc(K.num(t, 2)) + '</td>';
        }).join('') + (printColumn ? '<td></td>' : '') + '</tr>';
        $id('lblCount').textContent = st.rows.length + ' record(s)';
        K.filterRow($id(prefix === 'cmp' ? 'tblCmp' : 'tblSum'));
    }

    // ------------------------------------------------------------------ chrome

    function csvCell(v) {
        var s = (v === null || v === undefined) ? '' : String(v);
        return /[",\r\n]/.test(s) ? '"' + s.replace(/"/g, '""') + '"' : s;
    }

    function exportCsv(which) {
        var st = which === 'cmp' ? cmp : sum, shape = which === 'cmp' ? 'comparison' : sum.shape;
        if (!st.rows.length) { box('Nothing to export yet.'); return; }
        var lines = [st.cols.map(csvCell).join(',')];
        st.rows.forEach(function (r) {
            lines.push(st.cols.map(function (c) { return csvCell(cellText(shape, c, r[c])); }).join(','));
        });
        var blob = new Blob(['\ufeff' + lines.join('\r\n')], { type: 'text/csv;charset=utf-8;' });
        var a = document.createElement('a');
        a.href = URL.createObjectURL(blob);
        a.download = 'production-comparison-' + shape + '.csv';
        document.body.appendChild(a); a.click(); document.body.removeChild(a);
        setTimeout(function () { URL.revokeObjectURL(a.href); }, 1000);
    }

    function toggleFullscreen(boxId) { var el = $id(boxId); if (el) el.classList.toggle('is-fullscreen'); }
    function setVal(id, v) { var e = $id(id); if (e) e.value = v; }

    /** GpsNoFrom_KeyPress / GpsNoTo_KeyPress - digits only. */
    function digitsOnly(el) {
        el.addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[0-9]/.test(e.key)) e.preventDefault(); });
        el.addEventListener('input', function () { var c = el.value.replace(/[^0-9]/g, ''); if (c !== el.value) el.value = c; });
    }

    function boot() {
        digitsOnly($id('txtDocNoFrom'));
        digitsOnly($id('txtDocNoTo'));
        var now = new Date();
        $id('datSumTo').value = now.getFullYear() + '-' + String(now.getMonth() + 1).padStart(2, '0') + '-'
                              + String(now.getDate()).padStart(2, '0');

        /* KeyDown:1243 - Ctrl+P 604, Ctrl+N comparison reset, Ctrl+E / Esc close. */
        K.enterToTab();
        K.keys({ 'ctrl+p': print604, 'ctrl+n': resetComparison, 'ctrl+e': K.close, 'esc': K.close });

        /* RdInputDetail / RdGainLoss CheckedChanged:1087 - every toggle clears grdInput. */
        document.querySelectorAll('input[name="sumMode"]').forEach(function (r) {
            r.addEventListener('change', function () {
                clearSummaryGrid();
                if (mode() === 'detail') show($id('panelValues'), false);
            });
        });
        $id('cmpBody').addEventListener('click', function (e) {
            var b = e.target.closest('button[data-print]');
            if (b) printRow(+b.getAttribute('data-print'));
        });
        /* cmbBranchNameComparison_Leave:476 / CmbBranchNameSummary_Leave:496 - closing a
           branch list rebuilds that tab's pickers. */
        document.addEventListener('click', function (e) {
            [['cmpBranchBox', loadComparisonLists], ['sumBranchBox', loadSummaryLists]].forEach(function (pair) {
                var b = $id(pair[0]);
                if (b && b.classList.contains('is-open') && !b.contains(e.target)) {
                    b.classList.remove('is-open');
                    pair[1]().catch(function (err) { box(err.message); });
                }
            });
            var dd = $id('ddGainLoss');
            if (dd && !dd.contains(e.target)) dd.classList.remove('is-open');
        });

        loadForm().catch(function (e) { box(e.message); });
    }

    window.ProductionComparison = {
        tab: tab, toggle: toggle, all: all, itemChanged: itemChanged,
        showComparison: showComparison, resetComparison: resetComparison, refreshComparison: refreshComparison,
        print604: print604,
        showSummary: showSummary, resetSummary: resetSummary, refreshSummary: refreshSummary, print607: print607,
        exportCsv: exportCsv, toggleFullscreen: toggleFullscreen
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
    else boot();
}());
