/* ============================================================================================
 * Production PackingMaterial Consumption Register —
 * frmProductionPackingMaterialConsumptionRegister.cs, ScreenDefinition 306.
 *
 *   two pickers  USP_GetDataForDropDownFromFoodProductionPM   (one call, split by Activity)
 *   the register Sp_InvProductionCumPackMaterialConsumption_Register
 *
 * The desktop binds the result set STRAIGHT to the grid — no intermediate table, so no fixed
 * column list. The headers here are built from what the procedure returns, with Id hidden and
 * Qty right-aligned and totalled, exactly as grdSettings does.
 *
 * Branch is MANDATORY: with none ticked, GridBind focuses the branch box and throws
 * "Select Branch First" before any query runs.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/production/reports/packing-material-consumption';

    var rows = [];
    var cols = [];
    var branches = [];

    /* grdSettings:326 — added to the grid, then hidden. */
    var HIDDEN = ['Id'];
    /* The one column grdSettings gives an aggregate, an alignment and a format. */
    var QTY = 'Qty';

    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function box(m) { window.alert(m); }

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
    /** .NET "0,0" - thousands separated, no decimals, at least two digits (5 -> "05"). */
    function fmtQty(v) {
        if (v === null || v === undefined || v === '') return '';
        return window.ReportKit.pad2(num(v));
    }
    var K = window.ReportKit;
    var lastArgs = null;
    var fyStart = '';
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
    /** Only DocDate is a date column here (a CalendarDropDown filter, :308); EntryDate has no
     *  format on the desktop, so it keeps its time. */
    function isDate(col) { return col === 'DocDate'; }

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

    function ticked() {
        var out = [];
        var boxes = $id('branchRows').querySelectorAll('input[type="checkbox"]');
        for (var i = 0; i < boxes.length; i++) if (boxes[i].checked) out.push(boxes[i].value);
        return out;
    }
    function tickedNames() {
        var out = [];
        var boxes = $id('branchRows').querySelectorAll('input[type="checkbox"]');
        for (var i = 0; i < boxes.length; i++) {
            if (boxes[i].checked) out.push(boxes[i].getAttribute('data-name'));
        }
        return out;
    }
    function renderBranches(defaultId) {
        var host = $id('branchRows');
        host.innerHTML = branches.map(function (b) {
            var id = ci(b, 'BranchId'), name = ci(b, 'BranchName');
            var on = String(id) === String(defaultId) ? ' checked' : '';
            return '<label class="cx-multi-row">'
                 + '<input type="checkbox" value="' + esc(id) + '" data-name="' + esc(name) + '"' + on + '>'
                 + '<span>' + esc(name) + '</span></label>';
        }).join('');
        host.onchange = function () { $id('txtBranchName').value = tickedNames().join(','); };
        $id('txtBranchName').value = tickedNames().join(',');
    }
    function toggle() { $id('branchBox').classList.toggle('is-open'); }

    /** cmbBranchName_Leave:430 does nothing - ComboBind runs only at Load (:97). */
    function closeBranches() {
        var b = $id('branchBox');
        if (!b.classList.contains('is-open')) return;
        b.classList.remove('is-open');
    }
    function all(on) {
        $id('branchRows').querySelectorAll('input[type="checkbox"]').forEach(function (c) { c.checked = on; });
        $id('txtBranchName').value = tickedNames().join(',');
    }

    // ------------------------------------------------------------------ pickers

    function fill(selectId, list) {
        /* ZeroIndex:true on both — a blank first row so the filter can be cleared. */
        $id(selectId).innerHTML = '<option value=""></option>' + (list || []).map(function (r) {
            return '<option value="' + esc(ci(r, 'Id')) + '">' + esc(ci(r, 'Name')) + '</option>';
        }).join('');
    }

    function loadLookups(keepBranches) {
        return getJson(api + '/lookups?branchIds=' + encodeURIComponent(ticked().join(',')))
            .then(function (d) {
                fill('cmbItem', d && d.items);
                fill('cmbPmItem', d && d.pmItems);
                if (!keepBranches) {
                    branches = (d && d.branches) || [];
                    renderBranches(d && d.defaultBranchId);
                    fyStart = isoDate(d && d.financialYearStart);
                    $id('datFromDate').value = fyStart || todayIso();
                }
            });
    }

    // ------------------------------------------------------------------ the register

    function show_() {
        return busy('btnShow', function () {
            /* The desktop's own check, made here too; the server checks it again. */
            if (!ticked().length) {
                box('Select Branch First');
                $id('branchBox').classList.add('is-open');
                return;
            }
            var q = ['branchIds=' + encodeURIComponent(ticked().join(',')),
                     'itemId=' + encodeURIComponent(val('cmbItem') || '0'),
                     'pmItemId=' + encodeURIComponent(val('cmbPmItem') || '0')];
            /* The desktop sets both dates unconditionally (:262-263). */
            if (!val('datFromDate') || !val('datToDate')) { box('From Date and To Date are required'); return; }
            q.push('fromDate=' + encodeURIComponent(val('datFromDate')));
            q.push('toDate=' + encodeURIComponent(val('datToDate')));
            var args = { fromDate: val('datFromDate'), toDate: val('datToDate'),
                         itemId: +(val('cmbItem') || 0), pmItemId: +(val('cmbPmItem') || 0),
                         branchesIds: ticked().map(function (x) { return ',' + x; }).join('') };

            return getJson(api + '?' + q.join('&')).then(function (data) {
                rows = data || [];
                lastArgs = args;
                render();
            }).catch(function (e) {
                rows = []; cols = []; lastArgs = null;
                $id('gridHead').innerHTML = '';
                $id('gridBody').innerHTML = '';
                $id('lblCount').textContent = '';
                box(e.message);
            });
        });
    }

    function cellText(col, v) {
        if (col === QTY) return fmtQty(v);
        if (isDate(col)) return shortDate(v);
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
        cols = Object.keys(rows[0]).filter(function (c) { return !has(HIDDEN, c); });
        head.innerHTML = cols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('');

        var total = 0;
        body.innerHTML = rows.map(function (r) {
            total += num(r[QTY]);
            return '<tr>' + cols.map(function (c) {
                return '<td' + (c === QTY ? ' class="num"' : '') + '>'
                     + esc(cellText(c, r[c])) + '</td>';
            }).join('') + '</tr>';
        }).join('')
        + '<tr class="cx-grand">' + cols.map(function (c, i) {
            if (c === QTY) return '<td class="num">' + esc(fmtQty(total)) + '</td>';
            return i === 0 ? '<td>Total</td>' : '<td></td>';
        }).join('') + '</tr>';

        $id('lblCount').textContent = rows.length + ' record(s)';
        K.filterRow($id('tblRegister'));
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
        a.download = 'packing-material-consumption.csv';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        setTimeout(function () { URL.revokeObjectURL(a.href); }, 1000);
    }

    /** Reset:342 - From = financial-year start, To = today, both item pickers cleared, focus From. */
    function reset() {
        $id('datFromDate').value = fyStart || todayIso();
        $id('datToDate').value = todayIso();
        ['cmbItem', 'cmbPmItem'].forEach(function (id) {
            var el = $id(id);
            el.value = '';
            el.dispatchEvent(new Event('change', { bubbles: true }));
        });
        $id('datFromDate').focus();
    }
    function todayIso() {
        var n = new Date();
        return n.getFullYear() + '-' + String(n.getMonth() + 1).padStart(2, '0') + '-' + String(n.getDate()).padStart(2, '0');
    }

    /** PrintRegister:321 - the Show result into 812 ("Not Record Found For Display" when empty). */
    function print() {
        if (!lastArgs || !rows.length) { box('Not Record Found For Display'); return; }
        return window.CrystalPrint.open('pm-812', lastArgs, 'btnPrint812');
    }

    function toggleFullscreen(boxId) {
        var el = $id(boxId);
        if (el) el.classList.toggle('is-fullscreen');
    }

    function boot() {
        var now = new Date();
        $id('datToDate').value = now.getFullYear() + '-'
            + String(now.getMonth() + 1).padStart(2, '0') + '-'
            + String(now.getDate()).padStart(2, '0');

        /* KeyDown:358 - Ctrl+P print, Ctrl+E/Esc close, Ctrl+N reset, Ctrl+S show. */
        K.enterToTab();
        K.keys({ 'ctrl+p': print, 'ctrl+e': K.close, 'esc': K.close, 'ctrl+n': reset, 'ctrl+s': show_ });
        document.addEventListener('click', function (e) {
            var b = $id('branchBox');
            if (b && !b.contains(e.target)) closeBranches();
        });

        /* Load:97-100 - ComboBind, then GridBind straight away. */
        loadLookups(false).then(show_).catch(function (e) { box(e.message); });
    }

    window.PackingMaterialConsumption = {
        show: show_,
        toggle: toggle,
        exportCsv: exportCsv,
        reset: reset,
        print: print,
        all: all,
        toggleFullscreen: toggleFullscreen
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
    else boot();
}());
