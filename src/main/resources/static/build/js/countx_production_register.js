/* ============================================================================================
 * Production Register — ProductionRegister.cs, ScreenDefinition 310.
 *
 *   six pickers  USP_GetDataForDropDownFromFoodProduction   (one call, split by Activity)
 *   job orders   usp_getJobOrderFromProduction
 *   the register USP_ProductionRegisterWithActivity
 *
 * One procedure returns three different shapes; the Activity picker chooses which, and each has
 * its own columns, its own totals and its own grouping. The server projects each shape, so this
 * file draws what it is given rather than deciding column sets of its own.
 *
 * Two behaviours from the desktop that are easy to lose:
 *   - Branch is MANDATORY. With none ticked, Gridfill throws "Select Branch First" before any
 *     query runs — it does not quietly fall back to every branch.
 *   - RatePerKg is Amount / Weight, computed rather than selected. A zero weight therefore gives
 *     a non-finite value, which is shown as it comes rather than replaced with a 0.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/production/reports/production-register';

    var rows = [];          /* the last result, so CSV cannot disagree with the screen */
    var cols = [];
    var activity = '';
    var lookups = null;

    /* grdsetting:680 — added to the table, then hidden. */
    /* ...and EntryType: the register is grouped on it and grd.HideColumnsWhenGrouped is True (:2305). */
    var REGISTER_HIDDEN = ['InvFoodProductionId', 'ModifyDate', 'ModifyUser', 'MainRemarks', 'EntryType'];
    var K = window.ReportKit;
    var lastArgs = null;        /* the parameters of the last Show - what 553/554/562 print */
    var PRINT_KEYS = { 'Production Register': ['pr-553', '553-ProductionRegisterWithActivity'],
                       'OutPut By Packing Material': ['pr-554', '554-ProductionRegisterWithActivity(OutPut By Packing Material)'],
                       'Production_Summary': ['pr-562', '562-ProductionRegisterWithActivity'] };
    /* grdSummarySetting:785. */
    var SUMMARY_HIDDEN = ['JobOrderId', 'PlantId', 'ItemId'];

    /* Both the register and the summary group on this column. */
    var GROUP_COLUMN = 'EntryType';

    /* grdsetting renames three headers; the underlying column keeps its name. */
    var CAPTIONS = {
        SupplierOtherRef: 'Supplier Reference',
        CropYear: 'Crop',
        Weight: 'Net Weight'
    };

    /* AggregateFunction 2 = Sum, 3 = Average. RatePerKg carries none. */
    var AGGREGATES = {
        'Production Register':        { ItemQty: 'sum', Weight: 'sum', Rate: 'avg', Amount: 'sum' },
        'OutPut By Packing Material': { BrandQty: 'sum', BrandWeight: 'sum', PmQty: 'sum' },
        'Production_Summary':         { Qty: 'sum', Weight: 'sum' }
    };

    /* FormatString from the three grid-setting methods. */
    var FORMATS = {
        ItemQty: 2, Weight: 2, Rate: 3, RatePerKg: 4, Amount: 3,
        BrandQty: 2, BrandWeight: 2, PmQty: 2, Qty: 2
    };

    var DATE_COLUMNS = ['DocDate', 'RefDocDate', 'GrnDate'];

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

    /** "#,##0.##" and friends — group separators, and up to N decimals with no trailing zeroes. */
    function fmt(v, decimals) {
        if (v === null || v === undefined || v === '') return '';
        var n = (typeof v === 'number') ? v : parseFloat(String(v).replace(/,/g, ''));
        if (isNaN(n)) return String(v);
        if (!isFinite(n)) return String(n);      /* a zero weight really did divide by zero */
        var s = n.toFixed(decimals);
        if (s.indexOf('.') >= 0) s = s.replace(/0+$/, '').replace(/\.$/, '');
        var parts = s.split('.');
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return parts.join('.');
    }
    /**
     * The desktop renders every one of these with Conversion.ToDateTime(...).ToShortDateString().
     * The procedure does NOT return them all in one shape: DocDate and EntryDate come back as ISO
     * timestamps, RefDocDate as a plain "yyyy-MM-dd", and GrnDate as the string
     * "Aug 18 2026 12:00AM" - verified against live rows. Parsing only the ISO form left GrnDate
     * showing the raw string where the desktop shows a date, so all three forms are handled.
     */
    function shortDate(v) {
        if (v === null || v === undefined || v === '') return '';
        var s = String(v);
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(s);
        if (m) return m[3] + '/' + m[2] + '/' + m[1];
        var d = new Date(s.replace(/(\d)(AM|PM)$/i, '$1 $2'));
        if (!isNaN(d.getTime())) {
            return String(d.getDate()).padStart(2, '0') + '/'
                 + String(d.getMonth() + 1).padStart(2, '0') + '/' + d.getFullYear();
        }
        return s;
    }
    /** grdsetting:717 — EntryDate carries its own "dd-MMM-yy hh:mm tt". */
    function dateTime(v) {
        if (v === null || v === undefined || v === '') return '';
        var d = new Date(v);
        if (isNaN(d.getTime())) return String(v);
        var M = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
        var h = d.getHours(), ap = h < 12 ? 'AM' : 'PM';
        h = h % 12; if (h === 0) h = 12;
        return String(d.getDate()).padStart(2, '0') + '-' + M[d.getMonth()] + '-'
             + String(d.getFullYear()).slice(-2) + ' ' + String(h).padStart(2, '0') + ':'
             + String(d.getMinutes()).padStart(2, '0') + ' ' + ap;
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

    // ------------------------------------------------------------------ tick-lists

    function tickedValues(hostId) {
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

    function renderTicks(hostId, textId, list, valueKey, nameKey, checkedValue) {
        var host = $id(hostId);
        host.innerHTML = list.map(function (r) {
            var v = ci(r, valueKey), n = ci(r, nameKey);
            var on = (checkedValue !== undefined && String(v) === String(checkedValue)) ? ' checked' : '';
            return '<label class="cx-multi-row">'
                 + '<input type="checkbox" value="' + esc(v) + '" data-name="' + esc(n) + '"' + on + '>'
                 + '<span>' + esc(n) + '</span></label>';
        }).join('');
        host.onchange = function () { $id(textId).value = tickedNames(hostId).join(','); };
        $id(textId).value = tickedNames(hostId).join(',');
    }

    function toggle(id) { $id(id).classList.toggle('is-open'); }

    // ------------------------------------------------------------------ pickers

    /* The job-order dropdown's three visible columns (JobOrderBind:250). */
    if (window.DesktopCombo && window.DesktopCombo.define) {
        window.DesktopCombo.define('prJobOrder', [
            { caption: 'JobOrderNo', flex: 5 },
            { caption: 'StartDate',  flex: 2, key: 'start' },
            { caption: 'EndDate',    flex: 2, key: 'end' }
        ]);
    }
    /* DateTime cell in an UltraCombo: the short date (dd/MM/yyyy). */
    function shortD(v) {
        if (typeof v === 'number') {   /* a Timestamp serialised as epoch millis */
            var d = new Date(v);
            return String(d.getDate()).padStart(2, '0') + '/' + String(d.getMonth() + 1).padStart(2, '0') + '/' + d.getFullYear();
        }
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || ''));
        return m ? m[3] + '/' + m[2] + '/' + m[1] : (v == null ? '' : String(v));
    }

    function fill(selectId, list, valueKey, nameKey) {
        /* ZeroIndex:true on every one of these — the desktop inserts a blank first row so the
           filter can be cleared back to "no restriction". */
        $id(selectId).innerHTML = '<option value=""></option>' + list.map(function (r) {
            return '<option value="' + esc(ci(r, valueKey)) + '">' + esc(ci(r, nameKey)) + '</option>';
        }).join('');
    }

    function loadLookups() {
        return getJson(api + '/lookups').then(function (d) {
            lookups = d || {};
            fill('cmbParentCategory', lookups.parentCategories || [], 'Id', 'Name');
            fill('cmbWarehouse',      lookups.warehouses || [],       'Id', 'Name');
            fill('cmbItem',           lookups.items || [],            'Id', 'Name');
            fill('cmbPlant',          lookups.plants || [],           'Id', 'Name');
            fill('cmbWipAccount',     lookups.wipAccounts || [],      'Id', 'Name');
            fill('cmbStockAccount',   lookups.stockAccounts || [],    'Id', 'Name');
            /* JobOrderBind:250 - DDL.BindDDL shows every column usp_getJobOrderFromProduction returns
               after the hidden Id: JobOrderNo (350 wide), StartDate, EndDate. */
            $id('cmbJobOrder').innerHTML = '<option value=""></option>' + (lookups.jobOrders || []).map(function (r) {
                return '<option value="' + esc(ci(r, 'Id')) + '" data-start="' + esc(shortD(ci(r, 'StartDate')))
                     + '" data-end="' + esc(shortD(ci(r, 'EndDate'))) + '">' + esc(ci(r, 'JobOrderNo')) + '</option>';
            }).join('');

            /* Activityfill:484 activates Rows[1] — row 0 being the blank one, that is the first
               real entry, "Production Register". */
            var acts = lookups.activities || [];
            $id('cmbActivity').innerHTML = '<option value=""></option>' + acts.map(function (a) {
                return '<option value="' + esc(a) + '">' + esc(a) + '</option>';
            }).join('');
            if (acts.length) $id('cmbActivity').value = acts[0];

            renderTicks('branchRows', 'txtBranchName', lookups.branches || [],
                        'BranchId', 'BranchName', lookups.defaultBranchId);
            renderTicks('entryTypeRows', 'txtEntryType', lookups.entryTypes || [],
                        'Id', 'Activity');
        });
    }

    // ------------------------------------------------------------------ the register

    function show_() {
        return busy('btnShow', function () {
            var act = val('cmbActivity');
            if (!act) { box('Select an activity type'); return; }
            /* The desktop's own check, made here too so the click is not spent on a round trip
               that is certain to be refused. The server checks it again. */
            if (!tickedValues('branchRows').length) {
                box('Select Branch First');
                $id('branchBox').classList.add('is-open');
                return;
            }

            var mode = document.querySelector('input[name="dateMode"]:checked');
            var q = ['activity=' + encodeURIComponent(act),
                     'dateMode=' + encodeURIComponent(mode ? mode.value : 'doc'),
                     'branchIds=' + encodeURIComponent(tickedValues('branchRows').join(',')),
                     'entryTypes=' + encodeURIComponent(tickedValues('entryTypeRows').join(',')),
                     'jobOrderId=' + encodeURIComponent(val('cmbJobOrder') || '0'),
                     'parentCategoryId=' + encodeURIComponent(val('cmbParentCategory') || '0'),
                     'warehouseId=' + encodeURIComponent(val('cmbWarehouse') || '0'),
                     'itemId=' + encodeURIComponent(val('cmbItem') || '0'),
                     'plantId=' + encodeURIComponent(val('cmbPlant') || '0'),
                     'wipAccountId=' + encodeURIComponent(val('cmbWipAccount') || '0'),
                     'stockAccountId=' + encodeURIComponent(val('cmbStockAccount') || '0')];
            /* datFromDate carries its own checkbox; datToDate does not. */
            if ($id('chkFromDate').checked && val('datFromDate')) {
                q.push('fromDate=' + encodeURIComponent(val('datFromDate')));
            }
            /* datToDate has no checkbox - @ToDate / @EntryToDate is always sent. */
            if (!val('datToDate')) $id('datToDate').value = iso(new Date());
            q.push('toDate=' + encodeURIComponent(val('datToDate')));

            var entryMode = mode && mode.value === 'entry';
            var args = {
                jobOrderId: +(val('cmbJobOrder') || 0), plantId: +(val('cmbPlant') || 0),
                itemId: +(val('cmbItem') || 0), stockAccountId: +(val('cmbStockAccount') || 0),
                warehouseId: +(val('cmbWarehouse') || 0), parentCategoryId: +(val('cmbParentCategory') || 0),
                wipAccountId: +(val('cmbWipAccount') || 0),
                entryTypeDetail: tickedValues('entryTypeRows').map(function (x) { return ',' + x; }).join(''),
                branchesIds: tickedValues('branchRows').map(function (x) { return ',' + x; }).join('')
            };
            var fromOn = $id('chkFromDate').checked && val('datFromDate');
            if (entryMode) { if (fromOn) args.entryFromDate = val('datFromDate'); args.entryToDate = val('datToDate'); }
            else { if (fromOn) args.fromDate = val('datFromDate'); args.toDate = val('datToDate'); }

            return getJson(api + '?' + q.join('&')).then(function (d) {
                lastArgs = args;
                activity = (d && d.activity) || act;
                rows = (d && d.rows) || [];
                $id('lblGridTitle').textContent = activity;
                /* Groups.Add("EntryType") shows its chip in the group-by box; the packing-material shape is ungrouped. */
                $id('lblGroupBy').textContent = (rows.length && activity !== 'OutPut By Packing Material')
                    ? 'Entry Type' : 'Drag a column header here to group by that column.';
                renderTotals(d && d.totals);
                render();
            }).catch(function (e) {
                rows = []; cols = []; lastArgs = null;
                $id('gridHead').innerHTML = '';
                $id('gridBody').innerHTML = '';
                $id('lblCount').textContent = '';
                show($id('summaryInfoBox'), false);
                box(e.message);
            });
        });
    }

    function renderTotals(t) {
        var on = activity === 'Production_Summary' && !!t;
        show($id('summaryInfoBox'), on);
        if (!on) return;
        /* The desktop formats all six with "#,##0.###". */
        ['totalIssuance', 'totalByProduct', 'totalFinishGoods',
         'totalInput', 'totalOutPut', 'totalDifference'].forEach(function (k) {
            $id(k).textContent = fmt(t[k], 3);
        });
    }

    function hiddenFor(act) {
        if (act === 'Production Register') return REGISTER_HIDDEN;
        if (act === 'Production_Summary') return SUMMARY_HIDDEN.concat(['EntryType']);
        return [];
    }

    function cellText(col, v) {
        if (has(DATE_COLUMNS, col)) return shortDate(v);
        if (col === 'EntryDate') return dateTime(v);
        /* grdSummarySetting:781 - Weight is "#,##0.###" on the summary. */
        if (col === 'Weight' && activity === 'Production_Summary') return fmt(v, 3);
        if (Object.prototype.hasOwnProperty.call(FORMATS, col)) return fmt(v, FORMATS[col]);
        return v;
    }

    function render() {
        var head = $id('gridHead'), body = $id('gridBody');
        if (!rows.length) {
            cols = [];
            head.innerHTML = '';
            body.innerHTML = '<tr><td>No records</td></tr>';
            $id('lblCount').textContent = '0 record(s)';
            return;
        }
        var hidden = hiddenFor(activity);
        cols = Object.keys(rows[0]).filter(function (c) { return !has(hidden, c); });

        head.innerHTML = cols.map(function (c) {
            return '<th>' + esc(CAPTIONS[c] || c) + '</th>';
        }).join('');

        /* Both the register and the summary are grouped by EntryType. The register keeps the
           column visible (grdsetting does not hide it); the summary hides it, since
           grdSummarySetting sets HideWhenGrouped. */
        if (activity === 'Production Register' || activity === 'Production_Summary') {
            body.innerHTML = grouped();
        } else {
            body.innerHTML = rows.map(function (r) {
                return '<tr>' + cols.map(function (c) { return td(c, r[c]); }).join('') + '</tr>';
            }).join('') + grandTotal();
        }
        $id('lblCount').textContent = rows.length + ' record(s)';
        K.filterRow($id('tblRegister'));
    }

    function td(col, v) {
        var agg = AGGREGATES[activity] || {};
        var right = Object.prototype.hasOwnProperty.call(FORMATS, col)
                 || Object.prototype.hasOwnProperty.call(agg, col);
        return '<td' + (right ? ' class="num"' : '') + '>' + esc(cellText(col, v)) + '</td>';
    }

    function aggregateRow(bucket, cls, label) {
        var agg = AGGREGATES[activity] || {};
        return '<tr class="' + cls + '">' + cols.map(function (c, i) {
            if (!Object.prototype.hasOwnProperty.call(agg, c)) {
                return i === 0 ? '<td>' + esc(label) + '</td>' : '<td></td>';
            }
            var t = 0;
            bucket.forEach(function (r) { t += num(r[c]); });
            if (agg[c] === 'avg' && bucket.length) t = t / bucket.length;
            var dec = (c === 'Weight' && activity === 'Production_Summary') ? 3 : (FORMATS[c] === undefined ? 2 : FORMATS[c]);
            return '<td class="num">' + esc(fmt(t, dec)) + '</td>';
        }).join('') + '</tr>';
    }

    function grandTotal() { return aggregateRow(rows, 'cx-grand', 'Total'); }

    /**
     * GridEX's Groups.Add("EntryType") COLLECTS every row of a value under one heading - it does
     * not merely break the list wherever the value changes. The rows arrive in document order,
     * so a break-on-change grouping showed "Material Input" three times and split the register
     * into nine headings where the desktop shows three. Rows are therefore bucketed by value,
     * with each value's heading appearing at the position it was first seen.
     */
    function grouped() {
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
                html.push('<tr>' + cols.map(function (c) { return td(c, r[c]); }).join('') + '</tr>');
            });
            html.push(aggregateRow(bucket, 'cx-total', 'Total'));
        });
        html.push(grandTotal());
        return html.join('');
    }

    // ------------------------------------------------------------------ chrome

    function csvCell(v) {
        var s = (v === null || v === undefined) ? '' : String(v);
        return /[",\r\n]/.test(s) ? '"' + s.replace(/"/g, '""') + '"' : s;
    }

    function exportCsv() {
        if (!rows.length) { box('Nothing to export yet.'); return; }
        var lines = [cols.map(function (c) { return csvCell(CAPTIONS[c] || c); }).join(',')];
        rows.forEach(function (r) {
            lines.push(cols.map(function (c) { return csvCell(cellText(c, r[c])); }).join(','));
        });
        var blob = new Blob(['﻿' + lines.join('\r\n')], { type: 'text/csv;charset=utf-8;' });
        var a = document.createElement('a');
        a.href = URL.createObjectURL(blob);
        a.download = 'production-register.csv';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        setTimeout(function () { URL.revokeObjectURL(a.href); }, 1000);
    }

    /** Reset:820 — clears five pickers only. Branch, entry type and the dates are left alone. */
    function reset() {
        ['cmbParentCategory', 'cmbWarehouse', 'cmbItem', 'cmbJobOrder', 'cmbPlant']
            .forEach(function (id) {
                var el = $id(id);
                el.value = '';
                el.dispatchEvent(new Event('change', { bubbles: true }));
            });
    }

    /** btnRefresh_Click:848 — reload the pickers from the database. */
    /** btnRefresh_Click:830 - AllCombobind + JobOrderBind only, each keeping its selection.
     *  Branch, Entry Type, Activity and the dates are untouched. */
    function refresh() {
        return busy('btnRefresh', function () {
            var keep = {};
            PICKERS.forEach(function (p) { keep[p[0]] = val(p[0]); });
            return getJson(api + '/lookups').then(function (d) {
                d = d || {};
                PICKERS.forEach(function (p) {
                    fill(p[0], d[p[1]] || [], 'Id', p[2]);
                    $id(p[0]).value = keep[p[0]];
                    if ($id(p[0]).value !== keep[p[0]]) $id(p[0]).value = '';
                });
            }).catch(function (e) { box(e.message); });
        });
    }
    var PICKERS = [['cmbParentCategory', 'parentCategories', 'Name'], ['cmbWarehouse', 'warehouses', 'Name'],
                   ['cmbItem', 'items', 'Name'], ['cmbPlant', 'plants', 'Name'],
                   ['cmbWipAccount', 'wipAccounts', 'Name'], ['cmbStockAccount', 'stockAccounts', 'Name'],
                   ['cmbJobOrder', 'jobOrders', 'JobOrderNo']];

    /** btnPrintCurrent_Click:1021 - the last Show's rows into the Activity's report. */
    function print(which) {
        var act = val('cmbActivity');
        var k = PRINT_KEYS[act];
        if (!k) return;
        if (which && which !== k[0]) return;      /* Alt+1 / Alt+2 only act on their own report */
        if (!lastArgs || !rows.length || activity !== act) { box('Record Not Found For Display'); return; }
        return window.CrystalPrint.open(k[0], lastArgs, 'btnPrintCurrent');
    }
    /** PrintButtonManage:991 - the button follows the Activity; hidden when none. */
    function printButtonManage() {
        var k = PRINT_KEYS[val('cmbActivity')];
        $id('btnPrintCurrent').style.display = k ? '' : 'none';
        if (k) $id('lblPrintCurrent').textContent = k[1];
    }

    function all(hostId, textId, on) {
        $id(hostId).querySelectorAll('input[type="checkbox"]').forEach(function (c) { c.checked = on; });
        $id(textId).value = tickedNames(hostId).join(',');
    }

    function shortcuts() {
        K.shortcuts([['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
                     ['Alt+2', 'For print 554-ProductionRegisterWithActivity(OutPut By Packing Material)'],
                     ['Alt+1', 'For Print 553-ProductionRegisterWithActivity'], ['Ctrl+F5', 'For Focus On DateType '],
                     ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
                     ['Ctrl+ArrowUp', 'For Focus On Supplier Customer in Filter']]);
    }

    function toggleFrom() {
        var on = $id('chkFromDate').checked;
        $id('datFromDate').disabled = !on;       /* unticking keeps the date, as the picker does */
    }

    function toggleFullscreen(boxId) {
        var el = $id(boxId);
        if (el) el.classList.toggle('is-fullscreen');
    }

    function iso(d) {
        return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0')
             + '-' + String(d.getDate()).padStart(2, '0');
    }

    function boot() {
        /* ProductionRegister_Load:193 — the From date opens at today minus 15 days. */
        var now = new Date();
        var from = new Date(now.getTime());
        from.setDate(from.getDate() - 15);
        $id('datFromDate').value = iso(from);
        $id('datToDate').value = iso(now);

        /* ProductionRegister_KeyDown:855 (KeyPreview on). */
        K.enterToTab();
        var focusFrom = function () { $id('chkFromDate').focus(); };
        K.keys({
            'ctrl+s': show_, 'ctrl+e': K.close, 'esc': K.close, 'ctrl+r': refresh, 'ctrl+n': reset,
            'alt+1': function () { print('pr-553'); }, 'alt+2': function () { print('pr-554'); },
            'ctrl+arrowdown': function () { var t = $id('tblRegister').querySelector('tbody tr'); if (t) { t.tabIndex = 0; t.focus(); } },
            'ctrl+arrowup': focusFrom, 'ctrl+f5': focusFrom, 'ctrl+alt': shortcuts
        });
        document.addEventListener('click', function (e) {
            ['branchBox', 'entryTypeBox'].forEach(function (id) {
                var b = $id(id);
                if (b && !b.contains(e.target) && b.classList.contains('is-open')) {
                    b.classList.remove('is-open');
                    /* cmbBranchName_Leave:1080 - an empty branch clears six filters. */
                    if (id === 'branchBox' && !tickedValues('branchRows').length) {
                        ['cmbParentCategory', 'cmbWarehouse', 'cmbItem', 'cmbJobOrder', 'cmbPlant'].forEach(function (x) { $id(x).value = ''; });
                        all('entryTypeRows', 'txtEntryType', false);
                    }
                }
            });
        });
        $id('cmbActivity').addEventListener('change', printButtonManage);
        loadLookups().then(printButtonManage).catch(function (e) { box(e.message); });
    }

    window.ProductionRegister = {
        show: show_,
        toggle: toggle,
        toggleFrom: toggleFrom,
        toggleFullscreen: toggleFullscreen,
        exportCsv: exportCsv,
        reset: reset,
        refresh: refresh,
        print: function () { return print(); },
        all: all,
        shortcuts: shortcuts
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
    else boot();
}());
