/* ============================================================================================
 * Production Summary Report — ProductionSummaryReport.cs, ScreenDefinition 309.
 *
 *   branches   [dbo].[USP_GetBranchsAllocatedToUserFromProduction]
 *   job orders [dbo].[USP_InvProductionJobOrder_GetJobOrderNoWithInfo]   DocumentTypeId 403
 *   plants     Sp_InvProductionJobOrder_GetAllMethod @Activity='GetPlantByJobOrderId'
 *   values     Sp_InvFoodProduction_GetSummeryValues
 *   schedule   USP_InvProductionJobOrderAndOrderAllocation_SummaryByJobOrderId
 *   recovery   Sp_InvFoodProduction_Summery_Rpt
 *   doc-wise   [dbo].[USP_FoodProduction_DocWiseSummeryReport]
 *
 * btnShow_Click:477 fills all four grids from one click, so one request returns all four; the
 * grids are only consistent with each other when they were read for the same filters.
 *
 * Rate and amount columns are decided on the server from the screen's "Rate" right. The page
 * draws whatever columns it is given, so a column that is not granted is absent from the
 * response rather than hidden in the DOM.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/production/reports/production-summary';

    /* The last result, kept so CSV export cannot disagree with what is on screen. */
    var data = { values: [], schedule: [], recovery: [], docWise: [] };
    var branches = [];          /* [{BranchId, BranchName}] from the user's own allocation */
    var jobOrders = [];         /* rows of the job-order picker, with their info columns */

    /* Columns the desktop hides after adding them (ScheduleGridSetting:660). They stay in the
       payload so a row keeps its identity; they are simply not drawn. */
    var SCHEDULE_HIDDEN = ['ContractId', 'ScheduleId', 'SupCustId'];

    /* Grouped grids: the desktop adds the group and then hides the grouped column itself
       (SummeryHistoryGridSetting:757, grdDocWiseSummerySetting:845). */
    var GROUP_COLUMN = 'TransactionType';

    /* Right-aligned like the desktop's numeric columns. */
    var NUMERIC = ['SortNo', 'WeightPrcnt', 'NetQty', 'NetWeight', 'Amount', 'AvgRate',
                   'AvgRateWoExp', 'OverHeadAmount', 'PackingMaterialAmount', 'QtyTotal',
                   'NetWeightTotal', 'TransactionRate', 'NetRate', 'TranRate', 'DocNo'];

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
    function numeric(col) {
        for (var i = 0; i < NUMERIC.length; i++) if (NUMERIC[i] === col) return true;
        return false;
    }
    function num(v) {
        var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, ''));
        return isNaN(n) ? 0 : n;
    }
    /* Dates arrive as ISO timestamps; the desktop shows ToShortDateString. */
    function shortDate(v) {
        if (v === null || v === undefined || v === '') return '';
        var s = String(v);
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(s);
        return m ? (m[3] + '/' + m[2] + '/' + m[1]) : s;
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

    // ------------------------------------------------------------------ branch tick-list

    function checkedBranchIds() {
        var out = [];
        var boxes = $id('branchRows').querySelectorAll('input[type="checkbox"]');
        for (var i = 0; i < boxes.length; i++) if (boxes[i].checked) out.push(boxes[i].value);
        return out;
    }

    function branchCsv() { return checkedBranchIds().join(','); }

    function renderBranchText() {
        var names = [];
        var boxes = $id('branchRows').querySelectorAll('input[type="checkbox"]');
        for (var i = 0; i < boxes.length; i++) {
            if (boxes[i].checked) names.push(boxes[i].getAttribute('data-name'));
        }
        /* The desktop's ListSeparator is "," and the text box holds the checked names. */
        $id('txtBranchName').value = names.join(',');
    }

    function toggleBranches() { $id('branchBox').classList.toggle('is-open'); }

    /* cmbBranchName_Leave:229 — closing the list rebuilds the job-order picker. */
    function closeBranches() {
        var b = $id('branchBox');
        if (!b.classList.contains('is-open')) return;
        b.classList.remove('is-open');
        loadJobOrders();
    }

    function renderBranches(defaultBranchId) {
        var host = $id('branchRows');
        host.innerHTML = branches.map(function (b) {
            var id = ci(b, 'BranchId'), name = ci(b, 'BranchName');
            var on = String(id) === String(defaultBranchId) ? ' checked' : '';
            return '<label class="cx-multi-row">'
                 + '<input type="checkbox" value="' + esc(id) + '" data-name="' + esc(name) + '"' + on + '>'
                 + '<span>' + esc(name) + '</span></label>';
        }).join('');
        host.onchange = renderBranchText;
        renderBranchText();
    }

    // ------------------------------------------------------------------ cascading pickers

    function loadBranches() {
        return getJson(api + '/lookups').then(function (d) {
            branches = (d && d.branches) || [];
            show($id('noRateNote'), !(d && d.canSeeRateAndAmount));
            renderBranches(d && d.defaultBranchId);
            return loadJobOrders();
        });
    }

    /** JobOrderNoFillForSummery:262 — rebuilt whenever the branch selection changes. */
    function loadJobOrders() {
        var keep = val('cmbJobOrderNo');
        return getJson(api + '/job-orders?branchIds=' + encodeURIComponent(branchCsv()))
            .then(function (rows) {
                jobOrders = rows || [];
                var sel = $id('cmbJobOrderNo');
                /* insertDefaultRow:false — the desktop adds no blank row to this picker. */
                /* Every column the procedure returns, because the desktop binds this picker
                   with AllColumns:true. The list was confirmed against a live response. */
                sel.innerHTML = jobOrders.map(function (r) {
                    return '<option value="' + esc(ci(r, 'Id')) + '"'
                         + ' data-order-date="' + esc(shortDate(ci(r, 'JobOrderDate'))) + '"'
                         + ' data-doc-no="' + esc(ci(r, 'JobOrderDocNo')) + '"'
                         + ' data-start-date="' + esc(shortDate(ci(r, 'JobStartDate'))) + '"'
                         + ' data-job-status="' + esc(ci(r, 'JobOrderStatus')) + '"'
                         + ' data-settled-status="' + esc(ci(r, 'SettledStatus')) + '"'
                         + ' data-settlement-date="' + esc(shortDate(ci(r, 'SettlementDate'))) + '"'
                         + ' data-approval-status="' + esc(ci(r, 'ApprovalStatus')) + '"'
                         + ' data-approved-date="' + esc(shortDate(ci(r, 'ApprovedDate'))) + '"'
                         + ' data-lot-code="' + esc(ci(r, 'LotCode')) + '">'
                         + esc(ci(r, 'JobOrderNo')) + '</option>';
                }).join('');
                /* BindAndRetainSelection — keep the previous pick when it survived the rebuild. */
                sel.value = keep;
                if (sel.value !== keep) sel.selectedIndex = -1;
                jobOrderChanged();
            }).catch(function (e) {
                jobOrders = [];
                $id('cmbJobOrderNo').innerHTML = '';
                box(e.message);
            });
    }

    function selectedJobOrder() {
        var id = val('cmbJobOrderNo');
        if (!id) return null;
        for (var i = 0; i < jobOrders.length; i++) {
            if (String(ci(jobOrders[i], 'Id')) === String(id)) return jobOrders[i];
        }
        return null;
    }

    /** cmbsummeryJobOrderNo_TextChanged:291 plus SummaryJobOrderInformationFill:588. */
    function jobOrderChanged() {
        fillJobOrderInfo();
        var id = val('cmbJobOrderNo');
        var plant = $id('cmbPlantName');
        if (!id) { plant.innerHTML = ''; return; }
        return getJson(api + '/plants?jobOrderId=' + encodeURIComponent(id)
                       + '&branchIds=' + encodeURIComponent(branchCsv()))
            .then(function (rows) {
                var list = rows || [];
                /* BindDDLNew — one visible column, and no blank row is added here either. */
                plant.innerHTML = list.map(function (r) {
                    return '<option value="' + esc(ci(r, 'PlantId')) + '">'
                         + esc(ci(r, 'PlantName')) + '</option>';
                }).join('');
                if (!list.length) plant.innerHTML = '';
            }).catch(function (e) { plant.innerHTML = ''; box(e.message); });
    }

    /** The information panel is the selected dropdown row — no second query, as in the desktop. */
    function fillJobOrderInfo() {
        var r = selectedJobOrder();
        if (!r) {
            $id('txtJobOrderNo').value = '';
            $id('txtJobOrderDocNo').value = '';
            $id('txtJobStartedDate').value = '';
            $id('txtJobOrderStatus').value = '';
            $id('txtSettlementStatus').value = '';
            $id('txtLotCode').value = '';
            show($id('fldStartDate'), false);
            show($id('fldSettlementDate'), false);
            return;
        }
        show($id('fldStartDate'), true);
        $id('txtJobOrderNo').value = ci(r, 'JobOrderNo');
        $id('txtJobOrderDocNo').value = ci(r, 'JobOrderDocNo');
        $id('txtJobStartedDate').value = shortDate(ci(r, 'JobStartDate'));
        $id('txtJobOrderStatus').value = ci(r, 'JobOrderStatus');
        var settled = ci(r, 'SettledStatus');
        $id('txtSettlementStatus').value = settled;
        /* :607 — the settlement date shows only for "Settled". */
        var isSettled = String(settled).trim() === 'Settled';
        show($id('fldSettlementDate'), isSettled);
        $id('txtSettlementDate').value = isSettled ? shortDate(ci(r, 'ApprovedDate')) : '';
        $id('txtLotCode').value = ci(r, 'LotCode');
    }

    function toggleDate(which) {
        var on = $id('chk' + which + 'Date').checked;
        var d = $id('dat' + which + 'Date');
        d.disabled = !on;
        if (!on) d.value = '';
    }

    // ------------------------------------------------------------------ the four grids

    function show_() {
        return busy('btnShow', function () {
            var id = val('cmbJobOrderNo');
            if (!id) { box('Select a job order'); return; }

            var q = ['jobOrderId=' + encodeURIComponent(id),
                     'plantId=' + encodeURIComponent(val('cmbPlantName') || '0'),
                     'branchIds=' + encodeURIComponent(branchCsv())];
            if ($id('chkFromDate').checked && val('datFromDate')) {
                q.push('fromDate=' + encodeURIComponent(val('datFromDate')));
            }
            if ($id('chkToDate').checked && val('datToDate')) {
                q.push('toDate=' + encodeURIComponent(val('datToDate')));
            }

            $id('lblStatus').textContent = '';
            return getJson(api + '?' + q.join('&')).then(function (d) {
                data = {
                    values:   (d && d.values)   || [],
                    schedule: (d && d.schedule) || [],
                    recovery: (d && d.recovery) || [],
                    docWise:  (d && d.docWise)  || []
                };
                show($id('noRateNote'), !(d && d.canSeeRateAndAmount));
                renderFlat('values', data.values, null);
                renderFlat('schedule', data.schedule, SCHEDULE_HIDDEN);
                renderGrouped('recovery', data.recovery);
                renderGrouped('docWise', data.docWise);
                $id('lblStatus').textContent = data.values.length + ' summary row(s)';
            }).catch(function (e) {
                data = { values: [], schedule: [], recovery: [], docWise: [] };
                ['values', 'schedule', 'recovery', 'docWise'].forEach(function (k) {
                    $id(k + 'Head').innerHTML = '';
                    $id(k + 'Body').innerHTML = '';
                });
                box(e.message);
            });
        });
    }

    function visibleColumns(rows, hidden) {
        if (!rows.length) return [];
        return Object.keys(rows[0]).filter(function (c) {
            return !(hidden || []).some(function (h) { return h.toLowerCase() === c.toLowerCase(); });
        });
    }

    function cell(col, v) {
        var text = (col === 'DocDate') ? shortDate(v) : v;
        return '<td' + (numeric(col) ? ' class="num"' : '') + '>' + esc(text) + '</td>';
    }

    function renderFlat(key, rows, hidden) {
        var head = $id(key + 'Head'), body = $id(key + 'Body');
        if (!rows.length) {
            head.innerHTML = '';
            body.innerHTML = '<tr><td>No records</td></tr>';
            return;
        }
        var cols = visibleColumns(rows, hidden);
        head.innerHTML = cols.map(function (c) {
            return '<th' + (numeric(c) ? ' class="num"' : '') + '>' + esc(c) + '</th>';
        }).join('');
        body.innerHTML = rows.map(function (r) {
            return '<tr>' + cols.map(function (c) { return cell(c, r[c]); }).join('') + '</tr>';
        }).join('');
    }

    /**
     * The desktop groups these two by TransactionType with GroupTotals on, then hides the grouped
     * column. So the group header carries the value, the column itself is not drawn, and each
     * group ends with a totals line over its numeric columns.
     */
    function renderGrouped(key, rows) {
        var head = $id(key + 'Head'), body = $id(key + 'Body');
        if (!rows.length) {
            head.innerHTML = '';
            body.innerHTML = '<tr><td>No records</td></tr>';
            return;
        }
        var cols = visibleColumns(rows, [GROUP_COLUMN]);
        head.innerHTML = cols.map(function (c) {
            return '<th' + (numeric(c) ? ' class="num"' : '') + '>' + esc(c) + '</th>';
        }).join('');

        /* GridEX's Groups.Add COLLECTS every row of a value under one heading rather than
           breaking wherever the value changes, and these rows do not arrive sorted by it. Rows
           are bucketed by value, each heading appearing where its value was first seen. */
        var order = [], buckets = {};
        rows.forEach(function (r) {
            var k = String(r[GROUP_COLUMN]);
            if (!Object.prototype.hasOwnProperty.call(buckets, k)) { buckets[k] = []; order.push(k); }
            buckets[k].push(r);
        });

        var html = [];
        order.forEach(function (k) {
            var bucket = buckets[k];
            html.push('<tr class="cx-group"><td colspan="' + cols.length + '">'
                    + esc(k) + ' &nbsp;(' + bucket.length + ')</td></tr>');
            bucket.forEach(function (r) {
                html.push('<tr>' + cols.map(function (c) { return cell(c, r[c]); }).join('') + '</tr>');
            });
            html.push('<tr class="cx-total">' + cols.map(function (c, i) {
                if (i === 0) return '<td>Total</td>';
                if (!numeric(c)) return '<td></td>';
                var t = 0;
                bucket.forEach(function (r) { t += num(r[c]); });
                return '<td class="num">' + esc(t.toFixed(2)) + '</td>';
            }).join('') + '</tr>');
        });
        body.innerHTML = html.join('');
    }

    // ------------------------------------------------------------------ export / chrome

    function csvCell(v) {
        var s = (v === null || v === undefined) ? '' : String(v);
        return /[",\r\n]/.test(s) ? '"' + s.replace(/"/g, '""') + '"' : s;
    }

    /** Exports all four grids, from the same arrays they were drawn from. */
    function exportCsv() {
        if (!data.values.length && !data.recovery.length && !data.docWise.length
            && !data.schedule.length) {
            box('Nothing to export yet.');
            return;
        }
        var lines = [];
        var block = function (title, rows, hidden) {
            if (!rows.length) return;
            lines.push(title);
            var cols = visibleColumns(rows, hidden);
            lines.push(cols.map(csvCell).join(','));
            rows.forEach(function (r) {
                lines.push(cols.map(function (c) {
                    return csvCell(c === 'DocDate' ? shortDate(r[c]) : r[c]);
                }).join(','));
            });
            lines.push('');
        };
        block('Summary Values', data.values, null);
        block('Export Schedule Information', data.schedule, SCHEDULE_HIDDEN);
        block('Recovery Summary', data.recovery, null);
        block('Document-wise Summary', data.docWise, null);

        var blob = new Blob(['﻿' + lines.join('\r\n')], { type: 'text/csv;charset=utf-8;' });
        var a = document.createElement('a');
        a.href = URL.createObjectURL(blob);
        a.download = 'production-summary.csv';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        setTimeout(function () { URL.revokeObjectURL(a.href); }, 1000);
    }

    function toggleFullscreen(boxId) {
        var el = $id(boxId);
        if (el) el.classList.toggle('is-fullscreen');
    }

    function boot() {
        /* ProductionSummaryReport_KeyDown:313 — Enter moves on rather than submitting. */
        document.addEventListener('keydown', function (e) {
            if (e.key === 'Enter' && e.target && e.target.tagName !== 'BUTTON'
                && e.target.tagName !== 'TEXTAREA') {
                e.preventDefault();
            }
        });
        document.addEventListener('click', function (e) {
            var b = $id('branchBox');
            if (b && !b.contains(e.target)) closeBranches();
        });
        toggleDate('From');
        toggleDate('To');
        loadBranches().catch(function (e) { box(e.message); });
    }

    window.ProductionSummary = {
        show: show_,
        jobOrderChanged: jobOrderChanged,
        toggleBranches: toggleBranches,
        toggleDate: toggleDate,
        exportCsv: exportCsv,
        toggleFullscreen: toggleFullscreen
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
    else boot();
}());
