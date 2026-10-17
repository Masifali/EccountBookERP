/* ============================================================================================
 * 906 "Production FOH".
 * Desktop: Architecture.WinApp.Production.Regular/frmProductionFohReportCreate.cs (DocumentTypeId 922).
 * Tabs "FOH Allocation" (BindGrid:339) and "FOH Expense" (Form: Run:586, InsertFohExpense:860, Delete:1011;
 * History: HistoryFill:1188). Line numbers below are that file's.
 * The grid arithmetic (Run, grdFohExpense_CellUpdated:765, PlantWiseSummaryGridFillFohExpense:800) runs here as it runs in
 * the desktop grid; every database step and every refusal InsertFohExpense makes before its write runs on the server.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/production/production-foh';
    var K = window.ReportKit;

    var rights = { save: false, update: false, delete: false, canViewAllRecord: false };
    var amtDec = 0, defDays = 0, today = '';
    var RecId = 0;
    var dtDetail = [];          /* id, month, accountId, jobOrderId, jobOrderNo, plantId, plantName, consumedHours, pct, amount, remarks */
    var dtSummary = [];
    var amountEditable = false; /* grdSettingsFohExpense:737 - fixed at bind time from the Method Type then */
    var allocShownMonth = '', allocShownRows = 0;
    var histRows = null, histDetail = [];
    var hcur = -1, fcur = -1;

    function $id(id) { return document.getElementById(id); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function ymd(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return null;
    }
    function toD(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isFinite(v) ? v : 0;
        var n = Number(String(v).trim().replace(/,/g, ''));
        return isNaN(n) ? 0 : n;
    }
    function toI(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isFinite(v) ? Math.round(v) : 0;
        var s = String(v).trim();
        return /^[+-]?\d+$/.test(s) ? parseInt(s, 10) : 0;
    }
    function toS(v) { return v === null || v === undefined ? '' : String(v); }

    /* ---- .NET custom numeric formats; round half away from zero on the 15-digit value ---- */
    function roundTo(x, dec) {
        var neg = x < 0, a = Math.abs(Number(x.toPrecision(15)));
        var r = Number(Math.round(Number(a + 'e' + dec)) + 'e-' + dec);
        return neg && r !== 0 ? -r : r;
    }
    function grp(s) { return s.replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
    function fmtFixed(x, dec) {
        if (!isFinite(x)) return String(x);
        var r = roundTo(x, dec), neg = r < 0, t = Math.abs(r).toFixed(dec).split('.');
        return (neg ? '-' : '') + grp(t[0]) + (t.length > 1 ? '.' + t[1] : '');
    }
    /* clsGlobalVariables.stringFormatsingle = "#,##0." + zeros */
    function fmtAmt(x) { return fmtFixed(x, amtDec); }
    function readNum(id) { return toD($id(id).value.trim()); }
    /* Math.Round(x, 2) - round half to even */
    function bankers2(x) {
        var s = x * 100, f = Math.floor(s), d = s - f, r;
        if (Math.abs(d - 0.5) < 1e-9) r = (f % 2 === 0) ? f : f + 1; else r = Math.round(s);
        return r / 100;
    }
    function say(m) { $id('lblStatus').textContent = m || ''; }

    /* ------------------------------------------------------------------ MessageBox */
    var ICON = { error: 'fa-times-circle" style="color:#c00' };
    function dialog(text, caption, icon, buttons) {
        return new Promise(function (resolve) {
            var m = document.createElement('div');
            m.className = 'pa-modal pa-msg';
            m.innerHTML = '<div class="pa-dlg" role="dialog"><div class="cap"><span>' + esc(caption || '') + '</span></div>'
                + '<div class="msg">' + (icon ? '<i class="fa ' + ICON[icon] + '"></i>' : '') + '<div>' + esc(text) + '</div></div>'
                + '<div class="btns">' + buttons.map(function (b, i) {
                    return '<button type="button" data-i="' + i + '">' + esc(b) + '</button>';
                }).join('') + '</div></div>';
            document.body.appendChild(m);
            var first = m.querySelector('button');
            if (first) first.focus();
            function done(i) { document.removeEventListener('keydown', key, true); m.remove(); resolve(i); }
            function key(e) {
                if (e.key === 'Escape') { e.preventDefault(); e.stopPropagation(); done(buttons.length === 1 ? 0 : 1); }
            }
            document.addEventListener('keydown', key, true);
            m.addEventListener('click', function (e) {
                var b = e.target.closest('button[data-i]');
                if (b) done(parseInt(b.getAttribute('data-i'), 10));
            });
        });
    }
    function msg(text, caption, icon) { return dialog(text, caption, icon, ['OK']); }
    function ask(text, caption) { return dialog(text, caption, null, ['Yes', 'No']).then(function (i) { return i === 0; }); }
    function modalOpen() { return !!document.querySelector('.pa-msg, .pa-modal:not(.is-hidden), .rk-modal.is-open'); }

    /* ------------------------------------------------------------------ requests */
    function headers(h) {
        var t = document.querySelector('meta[name="_csrf"]'), hn = document.querySelector('meta[name="_csrf_header"]');
        if (t && hn && t.getAttribute('content')) h[hn.getAttribute('content')] = t.getAttribute('content');
        return h;
    }
    function handle(r) {
        return r.text().then(function (t) {
            var body = null;
            try { body = t ? JSON.parse(t) : null; } catch (e) { body = null; }
            if (!r.ok) throw new Error(body && body.message ? body.message : (t || ('Request failed (' + r.status + ')')));
            return body;
        });
    }
    function getJson(url) {
        return fetch(url, { credentials: 'same-origin', headers: { 'Accept': 'application/json' } }).then(handle);
    }
    function postJson(url, body) {
        return fetch(url, { method: 'POST', credentials: 'same-origin',
            headers: headers({ 'Content-Type': 'application/json', 'Accept': 'application/json' }),
            body: JSON.stringify(body || {}) }).then(handle);
    }
    function qs(o) {
        return Object.keys(o).filter(function (k) { return o[k] !== null && o[k] !== undefined && o[k] !== ''; })
            .map(function (k) { return encodeURIComponent(k) + '=' + encodeURIComponent(o[k]); }).join('&');
    }
    function busy(btn, fn) {
        var b = typeof btn === 'string' ? $id(btn) : btn;
        if (b && b.classList.contains('is-busy')) return Promise.resolve();
        var was = b ? b.disabled : false;
        if (b) { b.classList.add('is-busy'); b.disabled = true; }
        var done = function () { if (b) { b.classList.remove('is-busy'); b.disabled = was; } };
        var p;
        try { p = Promise.resolve(fn()); } catch (e) { p = Promise.reject(e); }
        return p.then(function (v) { done(); return v; }, function (e) { done(); throw e; });
    }

    /* ------------------------------------------------------------------ combos */
    if (window.DesktopCombo) {
        /* AccountsBindByMonthDateDbCall:516 - BindAndRetainSelection(CmbAccountFohExpense, dt, "AccountId", "AccountTitle", "Account Title", AllColumns) */
        window.DesktopCombo.define('w3cFohAccount', [
            { caption: 'Account Title', flex: 4 },
            { caption: 'AccountCode', flex: 2, key: 'code' },
            { caption: 'NetAmount', flex: 2, key: 'net', type: 'num' },
            { caption: 'MethodType', flex: 2, key: 'mt' },
            { caption: 'Status', flex: 2, key: 'st' }
        ]);
    }
    function cssEsc(v) { return window.CSS && CSS.escape ? CSS.escape(v) : String(v).replace(/"/g, '\\"'); }
    function setValue(sel, v) {
        var s = String(v === null || v === undefined ? '' : v);
        Array.prototype.forEach.call(sel.querySelectorAll('option[data-orphan]'), function (o) { o.remove(); });
        if (s === '' || s === '0') { sel.value = ''; syncText(sel); return; }
        if (!sel.querySelector('option[value="' + cssEsc(s) + '"]')) {
            var o = document.createElement('option');
            o.value = s; o.textContent = s; o.setAttribute('data-orphan', '1');
            sel.appendChild(o);
        }
        sel.value = s;
        syncText(sel);
    }
    function syncText(sel) {
        var w = sel.closest('.dtcombo-wrap'), inp = w ? w.querySelector('input') : null;
        if (inp) { var o = sel.options[sel.selectedIndex]; inp.value = o && sel.value !== '' ? o.textContent : ''; }
    }
    function activeRow(sel) {
        var o = sel.options[sel.selectedIndex];
        return o && o.value !== '' && !o.hasAttribute('data-orphan') ? o : null;
    }
    function comboText(sel) {
        var o = sel.options[sel.selectedIndex];
        return o && sel.value !== '' ? o.textContent : '';
    }
    function focusCombo(sel) {
        var w = sel.closest('.dtcombo-wrap');
        var inp = w ? w.querySelector('input') : null;
        (inp || sel).focus();
    }
    function onLeave(sel, fn) {
        setTimeout(function () {
            var wrap = sel.closest('.dtcombo-wrap') || sel;
            wrap.addEventListener('focusout', function () {
                setTimeout(function () {
                    var a = document.activeElement;
                    if (a && (wrap.contains(a) || (a.closest && a.closest('.dtcombo-pop')))) return;
                    fn();
                }, 0);
            });
        }, 0);
    }
    function fillSimple(sel, items, keep) {
        var h = '<option value=""></option>';
        items.forEach(function (it) { h += '<option value="' + esc(it[0]) + '">' + esc(it[1]) + '</option>'; });
        sel.innerHTML = h;
        setValue(sel, keep);
    }
    function fillMonths(months) {
        var items = (months || []).map(function (m) { return [m, m]; });
        fillSimple($id('CmbMonthDateFohExpense'), items, $id('CmbMonthDateFohExpense').value);
        fillSimple($id('CmbMonthHistoryFohExpense'), items, $id('CmbMonthHistoryFohExpense').value);
    }
    function fillMethods(list) {
        fillSimple($id('CmbMethodTypeFohExpense'), (list || []).map(function (m) { return [m.id, m.name]; }), $id('CmbMethodTypeFohExpense').value);
    }
    function fillAccounts(rows) {
        var sel = $id('CmbAccountFohExpense'), keep = sel.value;
        var h = '<option value=""></option>';
        (rows || []).forEach(function (r) {
            h += '<option value="' + esc(col(r, 'AccountId')) + '" data-net="' + esc(col(r, 'NetAmount')) + '" data-code="' + esc(col(r, 'AccountCode'))
                + '" data-mt="' + esc(col(r, 'MethodType')) + '" data-st="' + esc(col(r, 'Status')) + '">' + esc(col(r, 'AccountTitle')) + '</option>';
        });
        sel.innerHTML = h;
        setValue(sel, keep);
    }

    /* ============================================================ InitializeComponentMethod:277 */
    function load() {
        return getJson(API + '/init').then(function (d) {
            rights = d.rights || rights;
            amtDec = toI(d.amountDecimals); defDays = toI(d.defaultHistoryDays);
            today = d.today || ymd(new Date());
            $id('BtnSaveFohExpense').disabled = !rights.save;
            $id('btnUpdateFohExpense').disabled = !rights.update;
            $id('BtnDeleteFohExpense').disabled = !rights.delete;
            if (window.GridBar) {
                ['grdFohExpense', 'grdHistoryFohExpense'].forEach(function (id) {
                    window.GridBar.attach($id(id), null, {
                        canChooseFields: rights.fieldChooser !== false, canSaveLayout: rights.saveLayout !== false,
                        canPrint: !!rights.gridPrint, canExport: !!rights.gridExport,
                        canGroupCollapse: rights.groupCollapse !== false, canGroupExpand: rights.groupExpand !== false
                    });
                });
            }
            fillMonths(d.months); fillMethods(d.methodTypes);
            $id('txtMonthDateFohAllocation').value = today.substring(0, 7);
            $id('FromDateHistoryFohExpense').value = today;
            $id('ToDateHistoryFohExpense').value = today;
            renderAll();
        }).catch(function (e) { return msg(e.message || 'Error occurred during database call.'); });
    }
    /* BtnRefreshFohExpense_Click:1099 / btnRefreshHistoryFohExpense_Click:1164 */
    function refresh() {
        return getJson(API + '/refresh').then(function (d) {
            if (d.defaultHistoryDays !== undefined) defDays = toI(d.defaultHistoryDays);
            fillMonths(d.months); fillMethods(d.methodTypes);
        }, function (e) { return msg(e.message); });
    }

    /* ============================================================ FOH Allocation: BindGrid:339 */
    function monthDateValue() {
        var v = $id('txtMonthDateFohAllocation').value;
        return v ? v + '-01' : '';
    }
    function allocationShow() {
        var md = monthDateValue();
        if (!md) return msg('Invalid date');
        return busy('btnShowFohAllocation', function () {
            return getJson(API + '/allocation?' + qs({ monthDate: md })).then(function (d) {
                allocShownMonth = md; allocShownRows = toI(d.rows);
                var jp = d.jobPlant || [], pl = d.plant || [];
                $id('grdJobPlantFohAllocation').innerHTML = jp.length ? '<thead><tr><th style="min-width:140px">JobOrderNo</th><th style="min-width:110px">PlantName</th><th style="min-width:100px">ConsumedHours</th><th style="min-width:75px">%OfTotalHours</th></tr></thead><tbody>'
                    + jp.map(function (r) {
                        return '<tr><td>' + esc(r.jobOrderNo) + '</td><td>' + esc(r.plantName) + '</td><td class="num">' + fmtFixed(toD(r.consumedHours), 2)
                            + '</td><td class="num">' + fmtFixed(toD(r.percentOfTotalHours), 2) + '</td></tr>';
                    }).join('') + '</tbody>' : '';
                $id('grdPlantFohAllocation').innerHTML = pl.length ? '<thead><tr><th style="min-width:110px">PlantName</th><th style="min-width:100px">ConsumedHours</th><th style="min-width:75px">%OfTotalHours</th></tr></thead><tbody>'
                    + pl.map(function (r) {
                        return '<tr><td>' + esc(r.plantName) + '</td><td class="num">' + fmtFixed(toD(r.consumedHours), 2)
                            + '</td><td class="num">' + fmtFixed(toD(r.percentOfTotalHours), 2) + '</td></tr>';
                    }).join('') + '</tbody>' : '';
                say(jp.length + ' record(s)');
            }, function (e) { return msg(e.message); });
        });
    }
    /* BtnNewFohAloocation_Click:466 */
    function allocationNew() {
        $id('grdJobPlantFohAllocation').innerHTML = ''; $id('grdPlantFohAllocation').innerHTML = '';
        allocShownRows = 0; $id('txtMonthDateFohAllocation').focus();
    }
    /* btnPrint_Click:480 */
    function allocationPrint() {
        if (!allocShownRows) return msg('No Record Found For Display');
        var w = window.CrystalPrint.reserve();
        return Promise.resolve(window.CrystalPrint.open('w3c-666_01', { monthDate: allocShownMonth }, null, w));
    }

    /* ============================================================ FOH Expense: pickers */
    /* txtMonthDateFohExpense_Leave:503 / AccountsBindByMonthDateDbCall:516 */
    function monthLeave() {
        var m = $id('CmbMonthDateFohExpense').value;
        if (!m) { accountLeave(); return Promise.resolve(); }
        return getJson(API + '/accounts?' + qs({ month: m })).then(function (rows) {
            fillAccounts(rows); accountLeave();
        }, function (e) { return msg(e.message); });
    }
    /* CmbAccountFohExpense_Leave:535 */
    function accountLeave() {
        var sel = $id('CmbAccountFohExpense'), o = activeRow(sel), id = toI(sel.value), net = 0;
        if (o && id > 0) net = toD(o.getAttribute('data-net'));
        $id('txtAccountNetAmountFohExpense').value = fmtAmt(net);
    }

    /* ============================================================ rendering */
    function renderDetail() {
        var t = $id('grdFohExpense');
        if (!dtDetail.length) { t.innerHTML = ''; return; }
        var h = '<thead><tr><th style="min-width:110px">JobOrderNo</th><th style="min-width:100px">ConsumedHours</th><th style="min-width:95px">%OfTotalHours</th><th style="min-width:110px">ConsumedFohAmount</th><th style="min-width:200px">Remarks</th></tr></thead><tbody>';
        var order = [], byPlant = {};
        dtDetail.forEach(function (r, i) {
            var k = r.plantName;
            if (!byPlant.hasOwnProperty(k)) { byPlant[k] = []; order.push(k); }
            byPlant[k].push(i);
        });
        order.forEach(function (k) {
            var hrs = 0, amt = 0;
            byPlant[k].forEach(function (i) { hrs += toD(dtDetail[i].consumedHours); amt += toD(dtDetail[i].amount); });
            h += '<tr class="grp"><td colspan="5">PlantName : ' + esc(k) + '</td></tr>';
            byPlant[k].forEach(function (i) {
                var r = dtDetail[i];
                h += '<tr data-i="' + i + '"' + (i === fcur ? ' class="cur"' : '') + '><td>' + esc(r.jobOrderNo) + '</td><td class="num">' + fmtFixed(toD(r.consumedHours), 2)
                    + '</td><td class="num">' + fmtFixed(toD(r.pct), 2) + '</td>';
                if (amountEditable) h += '<td class="num"><input class="edit" data-amt="' + i + '" value="' + esc(String(toD(r.amount))) + '"></td>';
                else h += '<td class="num">' + fmtAmt(toD(r.amount)) + '</td>';
                h += '<td><input class="edit left" data-rem="' + i + '" value="' + esc(r.remarks) + '"></td></tr>';
            });
            h += '<tr class="grptot"><td>Total</td><td>' + fmtFixed(hrs, 2) + '</td><td></td><td>' + fmtAmt(amt) + '</td><td></td></tr>';
        });
        t.innerHTML = h + '</tbody>';
    }
    function renderSummary() {
        var t = $id('grdPlantWiseSummaryFohExpense');
        if (!dtSummary.length) { t.innerHTML = ''; return; }
        t.innerHTML = '<thead><tr><th style="min-width:110px">PlantName</th><th style="min-width:100px">ConsumedHours</th><th style="min-width:95px">%OfTotalHours</th><th style="min-width:110px">ConsumedFohAmount</th></tr></thead><tbody>'
            + dtSummary.map(function (r) {
                return '<tr><td>' + esc(r.plantName) + '</td><td class="num">' + fmtFixed(toD(r.consumedHours), 2) + '</td><td class="num">'
                    + fmtFixed(toD(r.pct), 2) + '</td><td class="num">' + fmtAmt(toD(r.amount)) + '</td></tr>';
            }).join('') + '</tbody>';
    }
    function histCols() {
        return [['FromDateMonth', 'FromDateMonth', 'd'], ['ToDateMonth', 'ToDateMonth', 'd'], ['Month', 'Month', 'l'], ['AccountTitle', 'AccountTitle', 'l'],
            ['NetAmount', 'NetAmount', 'a'], ['MethodType', 'MethodType', 'l'], ['Status', 'Status', 'l'], ['RemarksHeader', 'RemarksHeader', 'l'],
            ['EntryDate', 'EntryDate', 't'], ['EntryUser', 'EntryUser', 'l'], ['ModifyDate', 'ModifyDate', 't'], ['ModifyUser', 'ModifyUser', 'l'],
            ['ApprovedDate', 'ApprovedDate', 't'], ['ApprovedUser', 'ApprovedUser', 'l'], ['NoOfAttachments', 'NoOfAttachments', 'n']];
    }
    function renderHistory() {
        var t = $id('grdHistoryFohExpense');
        if (!histRows || !histRows.length) { t.innerHTML = ''; return; }
        var C = histCols();
        var h = '<thead><tr><th>Edit</th><th>Print</th>' + C.map(function (c) { return '<th>' + esc(c[0]) + '</th>'; }).join('') + '</tr></thead><tbody>';
        histRows.forEach(function (r, i) {
            h += '<tr data-i="' + i + '"' + (i === hcur ? ' class="cur"' : '') + '><td><button type="button" class="cb" data-act="edit">Edit</button></td><td><button type="button" class="cb" data-act="print">Print</button></td>';
            C.forEach(function (c) {
                var v = col(r, c[1]);
                if (c[2] === 'a') h += '<td class="num">' + fmtAmt(toD(v)) + '</td>';
                else if (c[2] === 'n') h += '<td class="num">' + esc(toS(v)) + '</td>';
                else if (c[2] === 't') h += '<td>' + esc(v ? K.dmyhm(v) : '') + '</td>';
                else if (c[2] === 'd') h += '<td>' + esc(v ? K.dMMMyy(v) : '') + '</td>';
                else h += '<td>' + esc(v) + '</td>';
            });
            h += '</tr>';
        });
        t.innerHTML = h + '</tbody>';
    }
    function renderHistDetail() {
        var t = $id('grdDetailHistoryFohExpense');
        if (!histDetail.length) { t.innerHTML = ''; return; }
        t.innerHTML = '<thead><tr><th>Month</th><th>AccountId</th><th>JobOrderNo</th><th>PlantName</th><th>ConsumedHours</th><th>%OfTotalHours</th><th>ConsumedFohAmount</th><th>Remarks</th></tr></thead><tbody>'
            + histDetail.map(function (r) {
                return '<tr><td>' + esc(r.month) + '</td><td class="num">' + esc(r.accountId) + '</td><td>' + esc(r.jobOrderNo) + '</td><td>' + esc(r.plantName)
                    + '</td><td class="num">' + fmtFixed(toD(r.consumedHours), 2) + '</td><td class="num">' + fmtFixed(toD(r.percentOfTotalHours), 2)
                    + '</td><td class="num">' + fmtAmt(toD(r.consumedFohAmount)) + '</td><td>' + esc(r.remarks) + '</td></tr>';
            }).join('') + '</tbody>';
    }
    function renderAll() { renderDetail(); renderSummary(); renderHistory(); renderHistDetail(); }

    /* ============================================================ PlantWiseSummaryGridFillFohExpense:800 */
    function summaryFill() {
        if (!dtDetail.length) { dtSummary = []; return; }
        var order = [], m = {};
        dtDetail.forEach(function (r) {
            var k = r.plantId + '|' + r.plantName;
            if (!m[k]) { m[k] = { plantId: r.plantId, plantName: r.plantName, consumedHours: 0, amount: 0 }; order.push(k); }
            m[k].consumedHours += toD(r.consumedHours); m[k].amount += toD(r.amount);
        });
        var total = 0;
        order.forEach(function (k) { total += m[k].consumedHours; });
        dtSummary = order.map(function (k) { var p = m[k]; p.pct = total > 0 ? p.consumedHours / total * 100 : 0; return p; });
    }
    function bindGrids() {
        amountEditable = toI($id('CmbMethodTypeFohExpense').value) === 2;
        renderDetail(); renderSummary();
    }

    /* ============================================================ BtnShowFohExpense_Click:586 */
    function validateControls() {
        if ($id('CmbMonthDateFohExpense').value === '') return msg('Month field is required').then(function () { return false; });
        if (toI($id('CmbAccountFohExpense').value) === 0) return msg('Account field is required').then(function () { return false; });
        if (toI($id('CmbMethodTypeFohExpense').value) === 0) return msg('Method Type field is required').then(function () { return false; });
        return Promise.resolve(true);
    }
    function run() {
        return validateControls().then(function (ok) {
            if (!ok) return;
            var net = readNum('txtAccountNetAmountFohExpense');
            if (!dtDetail.length) {
                /* FillForFohExpenseData:663 */
                var month = $id('CmbMonthDateFohExpense').value;
                return busy('BtnShowFohExpense', function () {
                    return getJson(API + '/expense-hours?' + qs({ month: month })).then(function (rows) {
                        dtDetail = [];
                        if (rows.length) {
                            var plantHours = {}, total = 0;
                            rows.forEach(function (r) { total += toD(r.consumedHours); plantHours[r.plantId] = (plantHours[r.plantId] || 0) + toD(r.consumedHours); });
                            var accId = toI($id('CmbAccountFohExpense').value), mText = comboText($id('CmbMonthDateFohExpense'));
                            rows.forEach(function (r) {
                                var pt = plantHours[r.plantId], h = toD(r.consumedHours);
                                dtDetail.push({ id: 0, month: mText, accountId: accId, jobOrderId: r.jobOrderId, jobOrderNo: r.jobOrderNo,
                                    plantId: r.plantId, plantName: r.plantName, consumedHours: h, pct: pt > 0 ? h / pt * 100 : 0,
                                    amount: net * (total > 0 ? h / total : 0), remarks: '' });
                            });
                        }
                        summaryFill(); bindGrids();
                    }, function (e) { return msg(e.message); });
                });
            }
            /* UpdateFohExpenseCalculations:638 */
            var total2 = 0;
            dtDetail.forEach(function (r) { total2 += toD(r.consumedHours); });
            dtDetail.forEach(function (r) { r.amount = net * (total2 > 0 ? toD(r.consumedHours) / total2 : 0); });
            summaryFill(); bindGrids();
        });
    }

    /* grdFohExpense_CellUpdated:765 */
    function amountEdited(i, text) {
        var r = dtDetail[i];
        if (!r) return;
        r.amount = toD(text);
        if (r.amount < 0) {
            var net = readNum('txtAccountNetAmountFohExpense'), total = 0;
            dtDetail.forEach(function (x) { total += toD(x.consumedHours); });
            r.amount = total > 0 ? bankers2(net * toD(r.consumedHours) / total) : 0;
            renderDetail();
            msg('Consumed FOH Amount cannot be negative');
            return;
        }
        renderDetail();
    }

    /* ============================================================ ResetFohExpense:1114 */
    function resetExpense() {
        RecId = 0;
        $id('BtnSaveFohExpense').classList.remove('is-hidden');
        $id('btnUpdateFohExpense').classList.add('is-hidden');
        $id('BtnDeleteFohExpense').classList.add('is-hidden');
        fillAccounts([]);
        setValue($id('CmbAccountFohExpense'), '');
        $id('txtAccountNetAmountFohExpense').value = '';
        dtDetail = []; dtSummary = []; fcur = -1;
        bindGrids();
        showMain(1); showSub(0);
        focusCombo($id('CmbMonthDateFohExpense'));
    }

    /* ============================================================ ReadByIdFohExpense:1044 */
    function readById(id) {
        resetExpense();
        RecId = id;
        return getJson(API + '/read?' + qs({ id: id, withAccounts: true })).then(function (d) {
            showSub(0);
            $id('BtnSaveFohExpense').classList.add('is-hidden');
            $id('btnUpdateFohExpense').classList.remove('is-hidden');
            $id('BtnDeleteFohExpense').classList.remove('is-hidden');
            setValue($id('CmbMonthDateFohExpense'), d.month);
            fillAccounts(d.accounts || []);
            setValue($id('CmbAccountFohExpense'), d.accountId);
            $id('txtAccountNetAmountFohExpense').value = fmtAmt(toD(d.netAmount));
            setValue($id('CmbMethodTypeFohExpense'), d.methodTypeId);
            dtDetail = (d.rows || []).map(function (r) {
                return { id: r.id, month: r.month, accountId: r.accountId, jobOrderId: r.jobOrderId, jobOrderNo: r.jobOrderNo, plantId: r.plantId,
                    plantName: r.plantName, consumedHours: r.consumedHours, pct: r.percentOfTotalHours, amount: r.consumedFohAmount, remarks: r.remarks };
            });
            summaryFill(); bindGrids();
        }, function (e) { RecId = 0; return msg(e.message); });
    }

    /* ============================================================ InsertFohExpense:860 */
    function insertExpense() {
        if (!dtDetail.length) return msg('Grid Record Not Found', 'Error!', 'error');
        return validateControls().then(function (ok) {
            if (!ok) return;
            return ask(RecId === 0 ? 'Are you sure to Save' : 'Are you sure to Update', 'Confirm').then(function (yes) {
                if (!yes) return;
                var sumAmt = 0;
                dtDetail.forEach(function (r) { sumAmt += toD(r.amount); });
                if (bankersInt(readNum('txtAccountNetAmountFohExpense')) !== bankersInt(sumAmt))
                    return msg('Grid Amount not equal To Net Amount', 'Error!', 'error');
                var monthSel = $id('CmbMonthDateFohExpense'), accSel = $id('CmbAccountFohExpense');
                var selectedMonth = comboText(monthSel), selectedAccountId = toI(accSel.value);
                var gridAccountId = 0, gridMonth = '';
                for (var i = 0; i < dtDetail.length; i++) {
                    gridAccountId = toI(dtDetail[i].accountId); gridMonth = toS(dtDetail[i].month);
                    if (gridMonth.trim() !== '' && gridAccountId > 0) break;
                }
                var chain = Promise.resolve(true);
                if (selectedMonth.toLowerCase() !== gridMonth.toLowerCase()) {
                    chain = chain.then(function () {
                        return ask("The selected month does not match the month in the detail data.\nDo you want to update the selected month to match the detail data's month value?", 'Month Mismatch')
                            .then(function (y) { if (y) setValue(monthSel, gridMonth); return y; });
                    });
                }
                chain = chain.then(function (cont) {
                    if (!cont) return false;
                    if (selectedAccountId !== gridAccountId) {
                        return ask("The selected account does not match the account in the detail data.\nDo you want to update the selected account to match the detail data's account value?", 'Account Mismatch')
                            .then(function (y) { if (y) setValue(accSel, gridAccountId); return y; });
                    }
                    return true;
                });
                return chain.then(function (cont) {
                    if (!cont) return;
                    var body = {
                        recId: RecId, month: comboText(monthSel) || monthSel.value, accountId: toI(accSel.value),
                        netAmount: readNum('txtAccountNetAmountFohExpense'), methodTypeId: toI($id('CmbMethodTypeFohExpense').value),
                        rows: dtDetail.map(function (r) {
                            return { id: RecId !== 0 ? r.id : 0, jobOrderId: r.jobOrderId, plantId: r.plantId, consumedHours: r.consumedHours,
                                consumedFohAmount: r.amount, remarks: r.remarks };
                        })
                    };
                    return busy(RecId > 0 ? 'btnUpdateFohExpense' : 'BtnSaveFohExpense', function () {
                        return postJson(API + '/save', body).then(function () {
                            var wasNew = RecId === 0;
                            return msg(wasNew ? 'Record Save Successfully' : 'Record Update Successfully').then(function () { resetExpense(); });
                        }, function (e) { return msg(e.message, 'Error!', 'error'); });
                    });
                });
            });
        });
    }
    /* Math.Round(double) - banker's rounding to an integer */
    function bankersInt(x) {
        var f = Math.floor(x), d = x - f;
        if (Math.abs(d - 0.5) < 1e-9) return f % 2 === 0 ? f : f + 1;
        return Math.round(x);
    }
    function saveClick() { RecId = 0; return insertExpense(); }
    function updateClick() {
        if (RecId === 0) return msg('RecId not Found');
        return insertExpense();
    }
    /* BtnDeleteFohExpense_Click:1011 */
    function deleteClick() {
        if (!(RecId > 0)) return msg('RecordId Not Found.....');
        return ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
            if (!yes) return;
            return busy('BtnDeleteFohExpense', function () {
                return postJson(API + '/delete?' + qs({ id: RecId }), {}).then(function () {
                    return msg('Delete Record Successfully').then(function () { resetExpense(); });
                }, function (e) { return msg(e.message); });
            });
        });
    }

    /* ============================================================ Print-922 */
    function print922(id) {
        var w = window.CrystalPrint.reserve();
        return getJson(API + '/print-rows?' + qs({ id: id })).then(function (d) {
            if (!d || !(d.rows > 0)) { window.CrystalPrint.release(w); return msg('No Record Found For Display'); }
            return window.CrystalPrint.open('w3c-922', { id: id }, null, w);
        }, function (e) { window.CrystalPrint.release(w); return msg(e.message); });
    }

    /* ============================================================ History */
    function histBasis() {
        var el = document.querySelector('input[name="histBasis"]:checked');
        return el ? el.value : 'entry';
    }
    function historyShow() {
        var p = { basis: histBasis(), month: $id('CmbMonthHistoryFohExpense').value };
        if ($id('chkFromDateHistory').checked) p.fromDate = $id('FromDateHistoryFohExpense').value;
        if ($id('chkToDateHistory').checked) p.toDate = $id('ToDateHistoryFohExpense').value;
        return busy('btnShowHistoryFohExpense', function () {
            return getJson(API + '/history?' + qs(p)).then(function (rows) {
                histRows = rows || []; hcur = -1; histDetail = [];
                renderHistory(); renderHistDetail();
                say(histRows.length + ' record(s)');
            }, function (e) { return msg(e.message); });
        });
    }
    /* btnResetHistoryFohExpense_Click:1148 */
    function historyReset() {
        var d = new Date(today + 'T00:00:00'); d.setDate(d.getDate() - 3);
        $id('FromDateHistoryFohExpense').value = ymd(d); $id('ToDateHistoryFohExpense').value = today;
        setValue($id('CmbMonthHistoryFohExpense'), '');
        histRows = null; histDetail = []; hcur = -1;
        renderHistory(); renderHistDetail();
    }
    /* grdHistoryFohExpense_SelectionChanged:1350 / GetDetailGrdByHeadId:1367 */
    function historySelect(i) {
        hcur = i;
        Array.prototype.forEach.call($id('grdHistoryFohExpense').querySelectorAll('tbody tr'), function (tr) {
            tr.classList.toggle('cur', toI(tr.getAttribute('data-i')) === i);
        });
        var r = histRows && histRows[i];
        if (!r) return;
        getJson(API + '/read?' + qs({ id: toI(col(r, 'Id')), withAccounts: false })).then(function (d) {
            histDetail = d.rows || []; renderHistDetail();
        }, function (e) { histDetail = []; renderHistDetail(); msg(e.message); });
    }

    /* ============================================================ tabs / keys */
    function showMain(n) {
        $id('tabFohAllocation').classList.toggle('is-hidden', n !== 0);
        $id('tabFohExpense').classList.toggle('is-hidden', n !== 1);
        Array.prototype.forEach.call($id('tabMain').querySelectorAll('button'), function (b) { b.classList.toggle('on', toI(b.getAttribute('data-main')) === n); });
    }
    function showSub(n) {
        $id('tabFormFohExpense').classList.toggle('is-hidden', n !== 0);
        $id('tabHistoryFohExpense').classList.toggle('is-hidden', n !== 1);
        Array.prototype.forEach.call($id('tabControl1').querySelectorAll('button'), function (b) { b.classList.toggle('on', toI(b.getAttribute('data-sub')) === n); });
    }
    function shortcutKeys() {
        var rows = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Month'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
            ['Ctrl+ArrowRight', 'For Moving In Detail Grids'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'],
            ['Ctrl+ArrowUp', 'For Focus On First Column in Detail for Entry'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
        $id('tblKeys').innerHTML = '<tr><th>KeyCombination</th><th>Description</th></tr>' + rows.map(function (r) {
            return '<tr><td>' + esc(r[0]) + '</td><td>' + esc(r[1]) + '</td></tr>';
        }).join('');
        $id('dlgKeys').classList.remove('is-hidden');
    }

    function wire() {
        Array.prototype.forEach.call($id('tabMain').querySelectorAll('button'), function (b) {
            b.addEventListener('click', function () { showMain(toI(b.getAttribute('data-main'))); });
        });
        Array.prototype.forEach.call($id('tabControl1').querySelectorAll('button'), function (b) {
            b.addEventListener('click', function () { showSub(toI(b.getAttribute('data-sub'))); });
        });
        Array.prototype.forEach.call(document.querySelectorAll('#dlgKeys [data-close]'), function (b) {
            b.addEventListener('click', function () { b.closest('.pa-modal').classList.add('is-hidden'); });
        });
        /* allocation */
        $id('btnShowFohAllocation').addEventListener('click', allocationShow);
        $id('BtnNewFohAloocation').addEventListener('click', allocationNew);
        $id('btnPrintFohallocation').addEventListener('click', allocationPrint);
        $id('btnShortCutKeyFohAllocation').addEventListener('click', shortcutKeys);
        /* expense */
        $id('BtnNewFohExpense').addEventListener('click', resetExpense);
        $id('BtnRefreshFohExpense').addEventListener('click', refresh);
        $id('BtnSaveFohExpense').addEventListener('click', saveClick);
        $id('btnUpdateFohExpense').addEventListener('click', updateClick);
        $id('BtnDeleteFohExpense').addEventListener('click', deleteClick);
        $id('BtnPrintFohExpense').addEventListener('click', function () { print922(RecId); });
        $id('BtnShortCutKeyFohExpense').addEventListener('click', function () { /* empty handler on the desktop (BtnShortCutKeyFohExpense_Click:1547) */ });
        $id('BtnShowFohExpense').addEventListener('click', run);
        var monthSel = $id('CmbMonthDateFohExpense'), accSel = $id('CmbAccountFohExpense');
        onLeave(monthSel, monthLeave);
        onLeave(accSel, accountLeave);
        accSel.addEventListener('change', accountLeave);
        $id('grdFohExpense').addEventListener('change', function (e) {
            var a = e.target.getAttribute && e.target.getAttribute('data-amt'), m = e.target.getAttribute && e.target.getAttribute('data-rem');
            if (a !== null && a !== undefined) amountEdited(toI(a), e.target.value);
            else if (m !== null && m !== undefined && dtDetail[toI(m)]) dtDetail[toI(m)].remarks = e.target.value;
        });
        /* history */
        $id('btnResetHistoryFohExpense').addEventListener('click', historyReset);
        $id('btnRefreshHistoryFohExpense').addEventListener('click', refresh);
        $id('btnShowHistoryFohExpense').addEventListener('click', historyShow);
        $id('grdHistoryFohExpense').addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr'); if (!tr) return;
            var i = toI(tr.getAttribute('data-i')), b = e.target.closest('button[data-act]');
            historySelect(i);
            if (!b) return;
            var id = toI(col(histRows[i], 'Id'));
            if (b.getAttribute('data-act') === 'print') print922(id); else readById(id);
        });
        $id('grdHistoryFohExpense').addEventListener('dblclick', function (e) {
            if (e.target.closest('button')) return;
            var tr = e.target.closest('tbody tr'); if (!tr) return;
            readById(toI(col(histRows[toI(tr.getAttribute('data-i'))], 'Id')));
        });
        /* keys (frmProductionFohReportCreate_KeyDown:1424) - note Ctrl+S / Ctrl+P / Ctrl+N act on the Allocation tab */
        document.addEventListener('keydown', function (e) {
            if (modalOpen()) return;
            var k = e.key;
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcutKeys(); return; }
            if (e.ctrlKey && (k === 'e' || k === 'E')) { e.preventDefault(); if (K && K.close) K.close(); else window.location.href = '/production'; }
            else if (e.ctrlKey && (k === 't' || k === 'T')) { e.preventDefault(); if ($id('tabFohExpense').classList.contains('is-hidden')) showMain(1); else { showMain(0); $id('txtMonthDateFohAllocation').focus(); } }
            else if (e.ctrlKey && (k === 's' || k === 'S')) { e.preventDefault(); allocationShow(); }
            else if (e.ctrlKey && (k === 'p' || k === 'P' || k === '1')) { e.preventDefault(); allocationPrint(); }
            else if (e.ctrlKey && k === 'F5') { e.preventDefault(); $id('txtMonthDateFohAllocation').focus(); }
            else if (e.ctrlKey && (k === 'n' || k === 'N')) { e.preventDefault(); allocationNew(); }
            else if (e.ctrlKey && k === 'ArrowUp') { e.preventDefault(); $id('txtMonthDateFohAllocation').focus(); }
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        wire();
        load();
    });
})();
