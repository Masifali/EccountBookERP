/* ============================================================================================
 * Screen 877 "Daily Plant Consumed Hours".
 * Desktop: Architecture.WinApp.Production/frmDailyPlantConsumedHours.cs (1,757 lines).
 * Opened from screen 280's Output tab (frmProductionOutput.btnDailyPlantConsumedHours_Click:3221,
 * `new frmDailyPlantConsumedHours(UserAccount).Show()` - its own window).
 *
 * Line numbers below are frmDailyPlantConsumedHours.cs's. The form state lives here as it lives in
 * the desktop controls; every database step (and every refusal Insert() makes) runs on the server.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/production/daily-plant-consumed-hours';
    var K = window.ReportKit;

    /* ---------------------------------------------------------------- form fields */
    var RecId = 0;
    var FormInitialized = false;
    var dtHistoryGrid = null;       /* grdfrm.DataSource (null = no DataSource) */
    var lastHistory = null;         /* dtHistoryFromDb: { count, args } of the last successful BindGrid */
    var docTime = timePart(nowIso());   /* txtDocdate.Value's time of day (DateTimePicker keeps it) */
    var histTime = timePart(nowIso());  /* FromDateHistory / ToDateHistory time of day */
    var current = -1;

    function $id(id) { return document.getElementById(id); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function say(m) { $id('lblStatus').textContent = m || ''; }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function nowIso() {
        var d = new Date();
        return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()) + 'T'
            + pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds());
    }
    function timePart(iso) { return iso && iso.length >= 19 ? iso.substring(10, 19) : 'T00:00:00'; }
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return null;
    }

    /* ------------------------------------------------------ Architecture.Common.Conversion */
    function toD(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isFinite(v) ? v : 0;
        var n = Number(String(v).trim().replace(/,/g, ''));
        return isNaN(n) || !isFinite(n) ? 0 : n;
    }
    function toI(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isFinite(v) ? Math.round(v) : 0;
        var s = String(v).trim();
        return /^[+-]?\d+$/.test(s) ? parseInt(s, 10) : 0;
    }
    /** double.ToString() (15 significant digits). */
    function str15(x) {
        if (x === null || x === undefined || x === '') return '';
        if (typeof x !== 'number') x = Number(x);
        if (isNaN(x)) return '';
        return String(Number(x.toPrecision(15)));
    }

    /**
     * Conversion.ToDecimal(text) as a scaled integer: decimal.Parse keeps the scale the text has,
     * so "8.50" - "5" is 3.50 and ToString() prints "3.50". Unparsable -> 0.
     */
    function dec(text) {
        var s = String(text === null || text === undefined ? '' : text).trim();
        var m = /^([+-]?)(\d*)(?:\.(\d*))?$/.exec(s);
        if (!s || !m || (m[2] === '' && (m[3] === undefined || m[3] === ''))) return { v: BigInt(0), s: 0 };
        var frac = m[3] || '';
        var v = BigInt((m[2] || '0') + frac);
        return { v: m[1] === '-' ? -v : v, s: frac.length };
    }
    function decSub(a, b) {
        var s = Math.max(a.s, b.s);
        var av = a.v * BigInt(Math.pow(10, s - a.s)), bv = b.v * BigInt(Math.pow(10, s - b.s));
        return { v: av - bv, s: s };
    }
    function decText(d) {
        var neg = d.v < 0, t = (neg ? -d.v : d.v).toString();
        if (d.s > 0) {
            while (t.length <= d.s) t = '0' + t;
            t = t.slice(0, t.length - d.s) + '.' + t.slice(t.length - d.s);
        }
        return (neg ? '-' : '') + t;
    }

    /* ------------------------------------------------------------------ MessageBox */
    var ICON = { error: 'fa-times-circle" style="color:#c00', question: 'fa-question-circle" style="color:#1565c0' };
    function dialog(text, caption, icon, buttons) {
        return new Promise(function (resolve) {
            var m = document.createElement('div');
            m.className = 'dp-modal';
            m.innerHTML = '<div class="dp-dlg" role="dialog"><div class="cap"><span>' + esc(caption || '') + '</span></div>'
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
                else if (e.key === 'Enter') { e.stopPropagation(); }
            }
            document.addEventListener('keydown', key, true);
            m.addEventListener('click', function (e) {
                var b = e.target.closest('button[data-i]');
                if (b) done(parseInt(b.getAttribute('data-i'), 10));
            });
        });
    }
    function msg(text, caption, icon) { return dialog(text, caption, icon, ['OK']); }
    function ask(text, caption) {
        return dialog(text, caption, null, ['Yes', 'No']).then(function (i) { return i === 0; });
    }
    function modalOpen() { return !!document.querySelector('.dp-modal, .rk-modal.is-open'); }

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
            body: JSON.stringify(body) }).then(handle);
    }
    function qs(o) {
        return Object.keys(o).filter(function (k) { return o[k] !== null && o[k] !== undefined && o[k] !== ''; })
            .map(function (k) { return encodeURIComponent(k) + '=' + encodeURIComponent(o[k]); }).join('&');
    }
    /** Button contract: disabled + spinner while running, duplicates ignored, restored on both outcomes. */
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
    /**
     * InfragisticsHelper.BindAndRetainSelection: rebinding keeps the previous Value when it is
     * still a row of the new list. An UltraCombo can also hold a Value that is NOT a row (ReadById
     * assigns one): that is kept as an "orphan" option - shown, but with no ActiveRow, which is
     * what ValidateControls tests.
     */
    function fill(sel, rows, idKey, textKey, keep) {
        var prev = keep !== undefined ? String(keep) : '';
        var h = '<option value=""></option>';
        (rows || []).forEach(function (r) {
            h += '<option value="' + esc(col(r, idKey)) + '">' + esc(col(r, textKey)) + '</option>';
        });
        sel.innerHTML = h;
        sel.value = (prev && prev !== '0' && sel.querySelector('option[value="' + cssEsc(prev) + '"]:not([data-orphan])')) ? prev : '';
    }
    function cssEsc(v) { return window.CSS && CSS.escape ? CSS.escape(v) : String(v).replace(/"/g, '\\"'); }
    /** combo.Value = v (UltraCombo): selects the row, or holds v with no ActiveRow. */
    function setValue(sel, v) {
        var s = String(v === null || v === undefined ? '' : v);
        Array.prototype.forEach.call(sel.querySelectorAll('option[data-orphan]'), function (o) { o.remove(); });
        if (s === '' ) { sel.value = ''; return; }
        if (!sel.querySelector('option[value="' + cssEsc(s) + '"]')) {
            var o = document.createElement('option');
            o.value = s; o.textContent = s; o.setAttribute('data-orphan', '1');
            sel.appendChild(o);
        }
        sel.value = s;
    }
    function clearCombo(sel) { sel.innerHTML = '<option value=""></option>'; sel.value = ''; }
    function activeRow(sel) {
        var o = sel.options[sel.selectedIndex];
        return o && o.value !== '' && !o.hasAttribute('data-orphan') ? o : null;
    }
    function comboValue(sel) { return toI(sel.value); }
    function focusCombo(sel) {
        var w = sel.closest('.dtcombo-wrap');
        var inp = w ? w.querySelector('input, [tabindex]') : null;
        (inp || sel).focus();
    }
    /** Control.Leave for a combo that the desktop-combo component wraps. */
    function onLeave(sel, fn) {
        function wire() {
            var wrap = sel.closest('.dtcombo-wrap') || sel;
            wrap.addEventListener('focusout', function () {
                setTimeout(function () {
                    var a = document.activeElement;
                    if (a && (wrap.contains(a) || (a.closest && a.closest('.dtcombo-pop')))) return;
                    fn();
                }, 0);
            });
        }
        setTimeout(wire, 0);
    }

    /* ============================================================ InitializeComponentMethod:157 */
    function load() {
        return getJson(API + '/load').then(function (d) {
            JobOrderNoBind(d.jobOrders);
            DownTimeReasonFill(d.reasons);
            FormInitialized = true;
            $id('btnSave').classList.remove('is-hidden');
            $id('btnupdate').classList.add('is-hidden');
            $id('btnAdd').textContent = 'Save';
        }).catch(function () {
            return msg('Error occurred during database call.');
        });
    }

    /** JobOrderNoBind:210 - both job order combos, retaining their values, then the plants. */
    function JobOrderNoBind(rows) {
        fill($id('CmbJobOrder'), rows, 'Id', 'PlanCode', comboValue($id('CmbJobOrder')));
        fill($id('CmbInvoiceNoHistory'), rows, 'Id', 'PlanCode', comboValue($id('CmbInvoiceNoHistory')));
        return PlantBindByJobOrderId(comboValue($id('CmbJobOrder')));
    }

    /** PlantBindByJobOrderId:224 - cleared when 0, and the procedure still runs. */
    function PlantBindByJobOrderId(jobOrderId) {
        var sel = $id('CmbPlant');
        if (jobOrderId === 0) clearCombo(sel);
        var keep = sel.value;
        return getJson(API + '/plants?jobOrderId=' + jobOrderId).then(function (rows) {
            fill(sel, rows, 'PlantId', 'PlantName', keep);
        }).catch(function (e) {
            return msg(e.message, 'Error Message', 'error');
        });
    }

    /** DownTimeReasonFill:268 - both reason combos, retaining their values. */
    function DownTimeReasonFill(rows) {
        fill($id('CmbReason'), rows, 'Id', 'LookupName', comboValue($id('CmbReason')));
        fill($id('CmbReasonHistory'), rows, 'Id', 'LookupName', comboValue($id('CmbReasonHistory')));
    }

    /* ============================================================================ Insert():286 */
    function reportDateIso() {
        var v = $id('txtDocdate').value;
        return v ? v + docTime : '';
    }
    function focusAndMsg(el, text) {
        return msg(text).then(function () { if (el.tagName === 'SELECT') focusCombo(el); else el.focus(); });
    }
    function Insert() {
        var cJob = $id('CmbJobOrder'), cPlant = $id('CmbPlant'), cReason = $id('CmbReason');
        var tCons = $id('txtConsumedHours'), tTotal = $id('txtTotalWorkingHours');
        /* FormHelper.ValidateControls - the list's order and wording. */
        if (!activeRow(cJob) || comboValue(cJob) === 0) return focusAndMsg(cJob, 'Job Order No field is required');
        if (!activeRow(cPlant) || comboValue(cPlant) === 0) return focusAndMsg(cPlant, 'Plant field is required');
        if (!tCons.value.trim()) return focusAndMsg(tCons, 'Occupied Time (hours) field is required');
        if (!tTotal.value.trim()) return focusAndMsg(tTotal, 'Total Working Hour field is required');
        if (!$id('txtDocdate').value) return focusAndMsg($id('txtDocdate'), 'Report Date is not valid');
        /* The four hour rules; each throw focuses txtConsumedHours (or CmbReason) first. */
        if (toD(tCons.value) > toD(tTotal.value)) {
            tCons.focus(); return msg('Occupied Time (hours) cannot be greater than Total Working Hours');
        }
        if (toD(tTotal.value) > 24.0) {
            tCons.focus(); return msg('Total Working Hours cannot be greater than 24 hours');
        }
        if (toD(tCons.value) > 24.0) {
            tCons.focus(); return msg('Occupied Time (hours) cannot be greater than 24 hours');
        }
        if (toD($id('txtDowntime').value) > 0.0 && comboValue(cReason) === 0) {
            focusCombo(cReason); return msg('DownTime Reason field is required');
        }
        return ask(RecId > 0 ? 'Are you sure you want to update?' : 'Are you sure you want to save?', 'Confirm').then(function (yes) {
            if (!yes) return;
            return postJson(API + '/save', {
                id: RecId,
                jobOrderId: comboValue(cJob),
                plantId: comboValue(cPlant),
                reasonId: comboValue(cReason),
                totalWorkingHours: tTotal.value,
                consumedHours: tCons.value,
                downtime: $id('txtDowntime').value,
                reportDate: reportDateIso()
            }).then(function () {
                return msg(RecId > 0 ? 'Record updated successfully.' : 'Record saved successfully.').then(ResetForm);
            });
        }).catch(function (e) { return msg(e.message); });
    }

    /** btnSave_Click:365 - RecId = 0 first, so the menu Save always inserts. */
    function btnSave_Click() { return busy('btnSave', function () { RecId = 0; return Insert(); }); }
    /** btnupdate_Click:378 / btnAdd_Click:418 - Insert() with the current RecId. */
    function btnupdate_Click() { return busy('btnupdate', Insert); }
    function btnAdd_Click() { return busy('btnAdd', Insert); }

    /* =========================================================================== ReadById:390 */
    function ReadById(i) {
        var r = dtHistoryGrid && dtHistoryGrid[i];
        if (!r) return;
        RecId = toI(r.Id);
        setValue($id('CmbJobOrder'), toI(r.JobOrderId));      /* no Leave: the plant list is NOT rebound */
        setValue($id('CmbPlant'), toI(r.PlantId));
        setValue($id('CmbReason'), toI(r.ReasonId) || '');
        $id('txtTotalWorkingHours').value = str15(r.TotalWorkingHours);
        DownTimeCalculate();
        $id('txtConsumedHours').value = str15(r.ConsumedHours);
        DownTimeCalculate();
        $id('txtDowntime').value = str15(r.Downtime);
        var rd = r.ReportDate ? String(r.ReportDate) : '';
        if (rd) { $id('txtDocdate').value = rd.substring(0, 10); docTime = timePart(rd.length >= 19 ? rd : rd.substring(0, 10) + 'T00:00:00'); }
        $id('btnSave').classList.add('is-hidden');
        $id('btnupdate').classList.remove('is-hidden');
        $id('btnAdd').textContent = 'Update';
        $id('txtConsumedHours').focus();
    }

    /* ============================================================================ ResetForm:572 */
    function ResetForm() {
        RecId = 0;
        $id('CmbJobOrder').value = '';
        Array.prototype.forEach.call(document.querySelectorAll('option[data-orphan]'), function (o) { o.remove(); });
        clearCombo($id('CmbPlant'));
        $id('CmbReason').value = '';
        $id('txtTotalWorkingHours').value = ''; DownTimeCalculate();
        $id('txtConsumedHours').value = ''; DownTimeCalculate();
        $id('txtDowntime').value = '';
        $id('btnSave').classList.remove('is-hidden');
        $id('btnupdate').classList.add('is-hidden');
        $id('btnAdd').textContent = 'Save';
    }

    /* ===================================================================== DownTimeCalculate:770 */
    function DownTimeCalculate() {
        $id('txtDowntime').value = '0';
        var d = decSub(dec($id('txtTotalWorkingHours').value.trim()), dec($id('txtConsumedHours').value.trim()));
        if (d.v > 0) $id('txtDowntime').value = decText(d);
    }

    /* ============================================================================ BindGrid:435 */
    function histDate(chk, inp) {
        return $id(chk).checked && $id(inp).value ? $id(inp).value + histTime : null;
    }
    function BindGrid() {
        var type = document.querySelector('input[name="histDate"]:checked');
        var dateType = type ? type.value : '';
        var from = histDate('chkFromDateHistory', 'FromDateHistory');
        var to = histDate('chkToDateHistory', 'ToDateHistory');
        var q = { dateType: dateType, fromDate: from, toDate: to,
                  jobOrderId: comboValue($id('CmbInvoiceNoHistory')), reasonId: comboValue($id('CmbReasonHistory')) };
        return getJson(API + '/history?' + qs(q)).then(function (rows) {
            /* dtHistoryFromDb - what Print-666 hands to the report, whatever the count. */
            var args = { jobOrderId: q.jobOrderId, reasonId: q.reasonId };
            if (dateType === 'doc') { args.fromDate = from; args.toDate = to; }
            else if (dateType === 'entry') { args.entryFromDate = from; args.entryToDate = to; }
            else if (dateType === 'modify') { args.modifyFromDate = from; args.modifyToDate = to; }
            lastHistory = { count: rows.length, args: args };
            if (rows.length > 0) {
                dtHistoryGrid = rows.map(function (dr) {
                    return { Id: col(dr, 'Id'), JobOrderId: col(dr, 'JobOrderId'), JobOrderNo: col(dr, 'JobOrderNo'),
                             PlantId: col(dr, 'PlantId'), PlantName: col(dr, 'PlantName'),
                             TotalWorkingHours: col(dr, 'TotalWorkingHours'), ConsumedHours: col(dr, 'ConsumedHours'),
                             Downtime: col(dr, 'Downtime'), ReasonId: col(dr, 'ReasonId'), DownTimeReason: col(dr, 'DownTimeReason'),
                             ReportDate: col(dr, 'ReportDate'), EntryUser: col(dr, 'EntryUserName'), EntryDate: col(dr, 'EntryDate'),
                             ModifyUser: col(dr, 'ModifyUserName'), ModifyDate: col(dr, 'ModifyDate') };
                });
            } else {
                dtHistoryGrid = null;
            }
            current = -1;
            render();
            say(rows.length ? rows.length + ' record(s)' : '');
        }).catch(function (e) { return msg(e.message); });
    }

    /* The columns RetrieveStructure produces, with Id / JobOrderId / PlantId / ReasonId hidden,
       "ConsumedHours" captioned "Actual Hours" and the three date formats (:507-514). Doubles are
       right-aligned "#,##0.##" (GridWrappingAndColumnSettings, 2 header lines). */
    var COLS = [
        { k: 'JobOrderNo', link: true }, { k: 'PlantName' },
        { k: 'TotalWorkingHours', num: true }, { k: 'ConsumedHours', cap: 'Actual Hours', num: true },
        { k: 'Downtime', num: true }, { k: 'DownTimeReason' },
        { k: 'ReportDate', date: 'dd-MM-yyyy' }, { k: 'EntryUser' }, { k: 'EntryDate', date: 'dd-MM-yyyy hh:mm tt' },
        { k: 'ModifyUser' }, { k: 'ModifyDate', date: 'dd-MM-yyyy hh:mm tt' }
    ];
    function cellText(c, v) {
        if (v === null || v === undefined || v === '') return '';
        if (c.num) return K.num(v, 2);
        if (c.date === 'dd-MM-yyyy') { var s = String(v); return s.length >= 10 ? s.substring(8, 10) + '-' + s.substring(5, 7) + '-' + s.substring(0, 4) : s; }
        if (c.date) return K.dmyhm(v);
        return String(v);
    }
    function render() {
        var t = $id('grdfrm');
        if (!dtHistoryGrid) { t.innerHTML = ''; return; }
        var h = '<thead><tr>' + COLS.map(function (c) { return '<th>' + esc(c.cap || c.k) + '</th>'; }).join('') + '</tr></thead><tbody>';
        dtHistoryGrid.forEach(function (r, i) {
            h += '<tr data-i="' + i + '"' + (i === current ? ' class="cur"' : '') + '>' + COLS.map(function (c) {
                var txt = esc(cellText(c, r[c.k]));
                if (c.link && txt) txt = '<a class="lnk" data-open="' + i + '">' + txt + '</a>';
                return '<td' + (c.num ? ' class="num"' : '') + '>' + txt + '</td>';
            }).join('') + '</tr>';
        });
        t.innerHTML = h + '</tbody>';
    }
    function setCurrent(i) {
        current = i;
        Array.prototype.forEach.call($id('grdfrm').querySelectorAll('tbody tr'), function (tr) {
            tr.classList.toggle('cur', toI(tr.getAttribute('data-i')) === i);
        });
    }

    /* ======================================================================== Print-666:711 */
    function btnPrint_Click() {
        if (!lastHistory || lastHistory.count === 0) return msg('No Record Found For Display');
        return window.CrystalPrint.open('dpch-666', lastHistory.args, 'btnPrint');
    }

    /* ======================================================================== the other menus */
    function refresh() {
        /* refreshToolStripMenuItem_Click:559 */
        return Promise.all([getJson(API + '/job-orders'), getJson(API + '/reasons')]).then(function (a) {
            DownTimeReasonFill(a[1]);
            return JobOrderNoBind(a[0]);
        }).catch(function (e) { return msg(e.message); });
    }
    function ShowShortcutKeys() {
        K.shortcuts([
            ['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'],
            ['Ctrl+N', 'For New'], ['Ctrl+F5', 'For Focus on BrandCode'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
            ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus On BrandCode'],
            ['Ctrl+Enter', 'When Focus On Any Grid For Update Record']
        ]);
    }
    /** btnDefineRasons_Click:801 - new frmInvLookup(UserAccount).Show(); cmbProfileName.Value = 21. */
    function btnDefineRasons_Click() {
        var w = window.open('/inventory/lookup-definitions?lookupTypeId=21', '_blank');
        if (!w) msg('The browser blocked the new window.');
    }
    /** Close() - this form is its own window (Show()), so the window closes. */
    function closeForm() {
        if (window.opener && !window.opener.closed) { window.close(); return; }
        K.close();
    }

    /* ============================================================================== events */
    function wire() {
        onLeave($id('CmbJobOrder'), function () { PlantBindByJobOrderId(comboValue($id('CmbJobOrder'))); });

        $id('btnnew').addEventListener('click', ResetForm);
        $id('refreshToolStripMenuItem').addEventListener('click', function () { busy('refreshToolStripMenuItem', refresh); });
        $id('btnSave').addEventListener('click', btnSave_Click);
        $id('btnupdate').addEventListener('click', btnupdate_Click);
        $id('btnAdd').addEventListener('click', btnAdd_Click);
        $id('btnPrint').addEventListener('click', btnPrint_Click);
        $id('btnShortCutKeys').addEventListener('click', ShowShortcutKeys);
        $id('btnDefineRasons').addEventListener('click', btnDefineRasons_Click);
        $id('btnShowHistory').addEventListener('click', function () { busy('btnShowHistory', BindGrid); });

        ['txtTotalWorkingHours', 'txtConsumedHours'].forEach(function (id) {
            $id(id).addEventListener('input', DownTimeCalculate);          /* TextChanged */
        });
        ['chkFromDateHistory', 'chkToDateHistory'].forEach(function (id) {
            var inp = $id(id === 'chkFromDateHistory' ? 'FromDateHistory' : 'ToDateHistory');
            $id(id).addEventListener('change', function () { inp.disabled = !$id(id).checked; });
        });

        var grid = $id('grdfrm');
        grid.addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr');
            if (tr) setCurrent(toI(tr.getAttribute('data-i')));
            var a = e.target.closest('a[data-open]');
            if (a) ReadById(toI(a.getAttribute('data-open')));
        });
        grid.addEventListener('dblclick', function (e) {           /* grdfrm_DoubleClick:528 */
            var tr = e.target.closest('tbody tr');
            if (tr) ReadById(toI(tr.getAttribute('data-i')));
        });
        $id('wrapGrid').addEventListener('keydown', function (e) {  /* grdfrm_KeyDown:694 */
            if (!dtHistoryGrid) return;
            if (e.key === 'ArrowDown' && !e.ctrlKey) { e.preventDefault(); setCurrent(Math.min(dtHistoryGrid.length - 1, current + 1)); }
            else if (e.key === 'ArrowUp' && !e.ctrlKey) { e.preventDefault(); setCurrent(Math.max(0, current - 1)); }
            else if (e.ctrlKey && e.key === 'Enter' && current >= 0) { e.preventDefault(); ReadById(current); }
        });

        /* frmDailyPlantConsumedHours_KeyDown:604 (KeyPreview). */
        K.enterToTab();
        document.addEventListener('keydown', function (e) {
            if (e.defaultPrevented || modalOpen()) return;
            var k = e.key;
            if (e.ctrlKey && (k === 's' || k === 'S')) {
                e.preventDefault();
                if (!$id('btnSave').classList.contains('is-hidden') && !$id('btnSave').disabled) btnSave_Click();
            } else if (e.ctrlKey && (k === 'u' || k === 'U')) {
                e.preventDefault();
                if (!$id('btnupdate').classList.contains('is-hidden') && !$id('btnupdate').disabled) btnupdate_Click();
            } else if (e.ctrlKey && (k === 'n' || k === 'N')) { e.preventDefault(); ResetForm(); }
            else if (e.ctrlKey && (k === 'r' || k === 'R')) { e.preventDefault(); busy('refreshToolStripMenuItem', refresh); }
            else if ((e.ctrlKey && (k === 'e' || k === 'E')) || k === 'Escape') { e.preventDefault(); closeForm(); }
            else if (e.ctrlKey && (k === 'F5' || k === 'ArrowUp')) { e.preventDefault(); focusCombo($id('CmbJobOrder')); }
            else if (e.ctrlKey && k === 'ArrowDown') { e.preventDefault(); $id('wrapGrid').focus(); }
            else if ((e.ctrlKey && k === 'Alt') || (e.altKey && k === 'Control')) { e.preventDefault(); ShowShortcutKeys(); }
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        var today = nowIso().substring(0, 10);
        $id('txtDocdate').value = today;          /* DateTimePicker.Value = DateTime.Now */
        $id('FromDateHistory').value = today;     /* ShowCheckBox pickers: Checked, today */
        $id('ToDateHistory').value = today;
        $id('CmbJobOrder').innerHTML = '<option value=""></option>';
        clearCombo($id('CmbPlant'));
        wire();
        load();
    });
}());
