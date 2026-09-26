/* ============================================================================================
 * Screen 280 "Production Against (Job Order)" - INPUT tab.
 * Desktop: Architecture.WinApp.Production/frmProductionInput.cs (DocumentTypeId 80) and the three
 * dialogs it opens (LoadavailableTransactionsForIssuance, LoadOutPutPendingforRates "Issue",
 * frmPendingMoveOrderDocuments), ported event by event. Line numbers are frmProductionInput.cs's.
 *
 * The form's DataTable `table` (Load:407-441) is TABLE below - one object per row, keyed by the
 * same 35 column names, so every desktop expression that reads r.Cells["X"] reads row.X here.
 *
 * Desktop MessageBox texts are reproduced exactly (including their spelling). Server messages are
 * the desktop's own ex.Message (SQL RAISERROR text included).
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/production/production-against-job-order/input';
    var K = window.ReportKit;

    /* ------------------------------------------------------------------------ form state */
    var S = null;                 // /state
    var R = {};                   // rights (SetRightsValueInRightsObject)
    var FIFO = false;             // FIFOCGSFlag
    var JOCWR = false;            // JobOrderCreatewithoutRates
    var WAGES_STATUS = false;     // WagesCompulsoryOnProduction
    var WAGES_ACTIVE = false;     // WagesActiveOrInActiveForInput
    var RECID = 0, VOUCHERHEADID = 0, REMOVE_IDS = '', UPDATE_INDEX = -1;
    var TABLE = [];               // the form's `table`
    var UOMLIST = [];             // dtuomlst (UOMSchedule.Getall)
    var ITEM_UOMS = [];           // dtUom of the chosen item (Id, PackUom, Equivalent)
    var JO_ITEMS = [];            // dtJobOrderInputItems
    var JO_ITEM_IDS = '';         // JoborderInputitems
    var JOB_LOTS = [], CROP_YEARS = [];   // dtJobInPut / dtCropInPut - filled ONCE at Load
    var HIST_ROWS = [], HIST_CURRENT = -1;
    var CURRENT_ROW = -1;
    var suppressRate = false, rateSeq = 0;
    var PAGE_TIME = timeNow();    // an untouched DateTimePicker keeps the time it was created with

    /* =========================================================================== helpers */
    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function msg(m) { window.alert(m); }
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return null;
    }
    /** Conversion.ToDouble - Convert.ToDouble accepts thousands separators; bad/empty -> 0. */
    function num(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isFinite(v) ? v : 0;
        if (typeof v === 'boolean') return v ? 1 : 0;
        var n = parseFloat(String(v).replace(/,/g, '').trim());
        return isNaN(n) || !isFinite(n) ? 0 : n;
    }
    /** Conversion.ToInt - Convert.ToInt32: numbers round half-to-even, strings must be whole numbers. */
    function int(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isFinite(v) ? roundEven(v, 0) : 0;
        if (typeof v === 'boolean') return v ? 1 : 0;
        var s = String(v).trim();
        return /^[+-]?\d+$/.test(s) ? parseInt(s, 10) : 0;
    }
    /** Math.Round(x, d) - .NET default MidpointRounding.ToEven. */
    function roundEven(x, d) {
        var m = Math.pow(10, d || 0), y = x * m, f = Math.floor(y), diff = y - f;
        var r;
        if (Math.abs(diff - 0.5) < 1e-9) r = (f % 2 === 0) ? f : f + 1;
        else r = Math.round(y);
        return r / m;
    }
    /** Math.Round(x, d, MidpointRounding.AwayFromZero) */
    function roundAway(x, d) {
        var m = Math.pow(10, d || 0);
        return (x < 0 ? -1 : 1) * Math.round(Math.abs(x) * m + 1e-9) / m;
    }
    function f3(v) { return K.num(v, 3); }              // "#,##0.###"
    function f4(v) { return K.num(v, 4); }              // "#,##0.####"
    function f2(v) { return K.num(v, 2); }              // "#,##0.##"
    /** "#,##" / "#,#" - whole number, and an EMPTY string for zero. */
    function fHash(v) {
        var n = roundAway(num(v), 0);
        if (n === 0) return '';
        return K.fixed(n, 0);
    }
    function amountDec() { return S && S.amountDecimals !== undefined ? int(S.amountDecimals) : 2; }
    function rateDec() { return S && S.rateDecimals !== undefined ? int(S.rateDecimals) : 2; }
    function fAmt(v) { return K.fixed(v, amountDec()); }  // clsGlobalVariables.stringFormatsingle
    function fRate(v) { return K.fixed(v, rateDec()); }   // DecimalRateFormate
    /** .NET Framework double.ToString(): 15 significant digits, no trailing zeros. */
    function netG(d) {
        if (isNaN(d)) return 'NaN';
        if (!isFinite(d)) return d > 0 ? 'Infinity' : '-Infinity';
        if (d === Math.round(d) && Math.abs(d) < 1e15) return String(d);
        return String(parseFloat(d.toPrecision(15)));
    }
    /** double.ToString("#,##0.###") of a non-finite value, as the text box shows it. */
    function fmtOrNet(v, fn) { return isFinite(v) ? fn(v) : netG(v); }

    function pad(n) { return String(n).padStart(2, '0'); }
    function timeNow() { var d = new Date(); return pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds()); }
    function isoDate(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function addDays(n) { var d = new Date(); d.setDate(d.getDate() + n); return d; }
    /** A DateTimePicker: the date input plus the time of day the picker carries (data-time). */
    function setDT(id, v) {
        var e = $id(id); if (!e) return;
        if (v instanceof Date) { e.value = isoDate(v); e.dataset.time = pad(v.getHours()) + ':' + pad(v.getMinutes()) + ':' + pad(v.getSeconds()); return; }
        var m = /^(\d{4}-\d{2}-\d{2})(?:[T ](\d{2}:\d{2}(?::\d{2})?))?/.exec(String(v || ''));
        if (!m) return;
        e.value = m[1];
        e.dataset.time = m[2] ? (m[2].length === 5 ? m[2] + ':00' : m[2]) : '00:00:00';
    }
    function getDT(id) {
        var e = $id(id); if (!e || !e.value) return '';
        return e.value + 'T' + (e.dataset.time || PAGE_TIME);
    }
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function parseDate(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2}))?/.exec(String(v || ''));
        return m ? { y: +m[1], M: +m[2], d: +m[3], h: +(m[4] || 0), mi: +(m[5] || 0) } : null;
    }
    function dMMMyy(v) { var p = parseDate(v); return p ? pad(p.d) + '-' + MON[p.M - 1] + '-' + String(p.y).slice(2) : (v == null ? '' : String(v)); }
    function dMMMyyyy(v) { var p = parseDate(v); return p ? pad(p.d) + '-' + MON[p.M - 1] + '-' + p.y : (v == null ? '' : String(v)); }
    function dMMMyyhmtt(v) {
        var p = parseDate(v); if (!p) return v == null ? '' : String(v);
        var h12 = p.h % 12 === 0 ? 12 : p.h % 12;
        return dMMMyy(v) + ' ' + pad(h12) + ':' + pad(p.mi) + ' ' + (p.h < 12 ? 'AM' : 'PM');
    }

    function csrf(h) {
        var t = document.querySelector('meta[name="_csrf"]'), hd = document.querySelector('meta[name="_csrf_header"]');
        if (t && hd) h[hd.getAttribute('content')] = t.getAttribute('content');
        return h;
    }
    function handle(r) {
        return r.text().then(function (t) {
            var body = null;
            try { body = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
            if (!r.ok) {
                var e = new Error((body && body.message) || ('Request failed (' + r.status + ')'));
                e.status = r.status;
                throw e;
            }
            return body;
        });
    }
    function getJson(path, q) {
        var url = API + path;
        if (q) {
            var parts = [];
            Object.keys(q).forEach(function (k) {
                if (q[k] === undefined || q[k] === null) return;
                parts.push(encodeURIComponent(k) + '=' + encodeURIComponent(q[k]));
            });
            if (parts.length) url += '?' + parts.join('&');
        }
        return fetch(url, { headers: { Accept: 'application/json' }, credentials: 'same-origin' }).then(handle);
    }
    function postJson(path, body) {
        return fetch(API + path, {
            method: 'POST', credentials: 'same-origin',
            headers: csrf({ 'Content-Type': 'application/json', Accept: 'application/json' }),
            body: JSON.stringify(body)
        }).then(handle);
    }
    /* The button contract: disabled with a spinner while running, duplicates ignored, re-enabled on
       success AND failure. */
    function busy(btn, fn) {
        var b = typeof btn === 'string' ? $id(btn) : btn;
        if (b) {
            if (b.classList.contains('is-busy')) return Promise.resolve();
            b.dataset.wasDisabled = b.disabled ? '1' : '';
            b.disabled = true;
            b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.classList.remove('is-busy'); b.disabled = b.dataset.wasDisabled === '1'; } };
        var p;
        try { p = fn(); } catch (e) { done(); msg(e.message); return Promise.resolve(); }
        return Promise.resolve(p).then(done, function (e) { done(); if (e) msg(e.message || String(e)); });
    }

    /* ------------------------------------------------------------------ combos (UltraCombo) */
    /** DDL.BindDDL(..., ZeroIndex:false): no "0" row; the blank option is the web's empty text. The
     *  previous value is re-selected when it is still in the list (keep=true). */
    function fill(id, rows, valueKey, textKey, keep, attrs) {
        var sel = $id(id); if (!sel) return;
        var prev = sel.value;
        var html = '<option value=""></option>';
        (rows || []).forEach(function (r) {
            var extra = '';
            if (attrs) Object.keys(attrs).forEach(function (a) { extra += ' data-' + a + '="' + esc(col(r, attrs[a])) + '"'; });
            html += '<option value="' + esc(col(r, valueKey)) + '"' + extra + '>' + esc(col(r, textKey)) + '</option>';
        });
        sel.innerHTML = html;
        if (keep && prev !== '' && Array.prototype.some.call(sel.options, function (o) { return o.value === prev; })) sel.value = prev;
        else sel.value = '';
    }
    function clearCombo(id) { var sel = $id(id); if (sel) { sel.innerHTML = '<option value=""></option>'; sel.value = ''; } }
    function selText(id) { var sel = $id(id); if (!sel || sel.selectedIndex < 0 || sel.value === '') return ''; return sel.options[sel.selectedIndex].textContent; }
    function selAttr(id, a) { var sel = $id(id); if (!sel || sel.selectedIndex < 0 || sel.value === '') return null; return sel.options[sel.selectedIndex].getAttribute('data-' + a); }
    function setByText(id, text) {
        var sel = $id(id); if (!sel) return;
        var hit = Array.prototype.find.call(sel.options, function (o) { return o.value !== '' && o.textContent === text; });
        sel.value = hit ? hit.value : '';
    }
    function hasValue(id, v) { var sel = $id(id); return !!sel && Array.prototype.some.call(sel.options, function (o) { return o.value === String(v); }); }
    function setCombo(id, v) { var sel = $id(id); if (!sel) return; sel.value = hasValue(id, v) && String(v) !== '' ? String(v) : ''; }
    function activateFirst(id) { var sel = $id(id); if (sel && sel.options.length > 1) sel.value = sel.options[1].value; }
    function focusCombo(id) {
        var sel = $id(id); if (!sel) return;
        var wrap = sel.closest('.dtcombo-wrap');
        var inp = wrap ? wrap.querySelector('.dtcombo-input') : null;
        (inp || sel).focus();
    }

    /** A UltraCombo Leave: the change, or focus leaving the control. De-duplicated. */
    function onLeave(id, fn) {
        var sel = $id(id); if (!sel) return;
        var last = { v: null, t: 0 };
        function fire() {
            var now = Date.now();
            if (last.v === sel.value && now - last.t < 600) return;
            last = { v: sel.value, t: now };
            fn();
        }
        sel.addEventListener('change', fire);
        var box = sel.closest('.pi-cell, .pi-fld') || sel.parentNode;
        box.addEventListener('focusout', function () {
            setTimeout(function () {
                if (box.contains(document.activeElement)) return;
                var pop = document.querySelector('.dtcombo-pop[style*="block"]');
                if (pop && pop.contains(document.activeElement)) return;
                fire();
            }, 180);
        });
    }

    /* ======================================================================= the grids */
    /**
     * Minimal GridEX: cols [{key, caption, num, fmt, total, edit, btn, link, check, hidden}].
     * TotalRow at the bottom where a column has total.
     */
    function renderGrid(tbl, cols, rows, o) {
        o = o || {};
        var vis = cols.filter(function (c) { return !c.hidden; });
        var head = '<tr>' + vis.map(function (c) {
            if (c.check && c.headerSelector) return '<th style="width:30px;"><input type="checkbox" data-head-check="1"></th>';
            return '<th' + (c.width ? ' style="min-width:' + c.width + 'px"' : '') + '>' + esc(c.caption === undefined ? c.key : c.caption) + '</th>';
        }).join('') + '</tr>';
        tbl.tHead.innerHTML = head;
        var body = rows.map(function (r, i) {
            var cls = (o.current === i ? 'is-current' : '');
            return '<tr data-i="' + i + '" class="' + cls + '">' + vis.map(function (c) {
                var v = c.get ? c.get(r) : r[c.key];
                if (c.btn) return '<td><button type="button" class="' + (c.btnClass || 'pi-cellbtn') + '" data-btn="' + esc(c.key) + '" data-i="' + i + '">' + esc(c.btn) + '</button></td>';
                if (c.check) return '<td style="text-align:center;"><input type="checkbox" data-check="' + i + '"' + (r.__checked ? ' checked' : '') + '></td>';
                var text = c.fmt ? c.fmt(v, r) : (v === null || v === undefined ? '' : String(v));
                if (c.edit) return '<td class="edit"><input type="text" data-edit="' + esc(c.key) + '" data-i="' + i + '" data-guard="decimal" value="' + esc(text) + '"></td>';
                if (c.link) return '<td class="' + (c.num ? 'num' : '') + '"><span class="pi-link" data-link="' + esc(c.key) + '" data-i="' + i + '">' + esc(text) + '</span></td>';
                return '<td class="' + (c.num ? 'num' : '') + '">' + esc(text) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        tbl.tBodies[0].innerHTML = body;
        var foot = '';
        if (vis.some(function (c) { return c.total; })) {
            foot = '<tr>' + vis.map(function (c) {
                if (!c.total) return '<td></td>';
                var s = 0;
                rows.forEach(function (r) { s += num(c.get ? c.get(r) : r[c.key]); });
                return '<td class="num">' + esc((c.totalFmt || c.fmt || String)(s)) + '</td>';
            }).join('') + '</tr>';
        }
        tbl.tFoot.innerHTML = foot;
        if (K && K.filterRow && o.filter !== false) K.filterRow(tbl);
    }
    function rowIndexOf(ev) { var tr = ev.target.closest('tr[data-i]'); return tr ? int(tr.getAttribute('data-i')) : -1; }

    /** ctrlGrdBar itself is the shared GridBar (countx_grid_bar.js, data-gridbar on the tables).
     *  This span keeps only the optional web Full screen button (not on the desktop). */
    function gridBar(span) {
        if (!span) return;
        var tblId = span.getAttribute('data-grid');
        span.innerHTML = '<button type="button" data-a="full" title="Full screen"><i class="fa fa-arrows-alt"></i></button>';
        span.addEventListener('click', function (e) {
            var b = e.target.closest('button'); if (!b) return;
            var tbl = $id(tblId);
            if (b.getAttribute('data-a') === 'full') tbl.closest('.win-grid-container, .pi-modal-box').classList.toggle('is-fullscreen');
        });
    }

    /* ======================================================== grdStockConversionProductionDetail */
    /* gridsettings:1983 - hidden columns, the four editable ones, the Delete "X" at position 0 */
    var HIDDEN = ['Id', 'ItemId', 'WareHouseId', 'RefDocumentTypeId', 'RefDocIdNo', 'RefDocSubIdNo', 'SupplierCustomerId',
        'JobLotId', 'WarehouseId', 'PackingTypeId', 'ItemUOMId', 'RateUOMId', 'RateForCheck', 'RateUOMForCheck', 'ItemEquivalent'];
    var TABLE_COLS = ['Id', 'RefDocumentTypeId', 'RefDocumentType', 'RefDocIdNo', 'RefDocSubIdNo', 'SupplierCustomerId', 'PartyName',
        'DocDate', 'DocNo', 'WareHouseId', 'WareHouse', 'ItemId', 'Item', 'ItemUOMId', 'ItemUOM', 'ItemEquivalent', 'CropYear',
        'JobLotId', 'JobLot', 'PackingTypeId', 'PackingType', 'BalanceQty', 'BalanceWeight', 'Quantity', 'Weight', 'EbUnit',
        'EbTotal', 'GrossWeight', 'Rate', 'RateForCheck', 'RateUOMId', 'RateUOM', 'RateUOMForCheck', 'ItemAmount', 'Remarks'];
    var QTY_COLS = { BalanceQty: 1, BalanceWeight: 1, Quantity: 1, Weight: 1, EbUnit: 1, EbTotal: 1, GrossWeight: 1 };
    var EDIT_COLS = { Quantity: 1, Weight: 1, EbUnit: 1, EbTotal: 1 };

    function detailCols() {
        var hidden = HIDDEN.slice();
        if (!R.rateAndAmount) hidden.push('Rate', 'RateUOM', 'ItemAmount');
        var cols = [{ key: 'Delete', caption: '', btn: 'X', btnClass: 'pi-x' }];
        TABLE_COLS.forEach(function (k) {
            var c = { key: k, hidden: hidden.indexOf(k) >= 0 };
            if (QTY_COLS[k]) { c.num = true; c.fmt = function (v) { return fmtOrNet(num(v), f3); }; c.total = true; }
            if (k === 'Rate') { c.num = true; c.fmt = function (v) { return fRate(num(v)); }; }
            if (k === 'ItemAmount') { c.num = true; c.fmt = function (v) { return fAmt(num(v)); }; c.total = true; }
            if (EDIT_COLS[k]) c.edit = true;
            if (k === 'WareHouse') c.width = 150;
            if (k === 'Item') c.width = 300;
            cols.push(c);
        });
        return cols;
    }
    function renderDetail() {
        renderGrid($id('tblDetail'), detailCols(), TABLE, { current: CURRENT_ROW });
    }

    /* ============================================================================ Load (:351) */
    function load() {
        return getJson('/state').then(function (d) {
            S = d;
            R = d.rights || {};
            FIFO = !!d.fifoCgs;
            JOCWR = !!d.jobOrderCreateWithoutRates;
            WAGES_STATUS = !!d.wagesCompulsoryOnProduction;
            WAGES_ACTIVE = !!d.wagesActiveForInput;
            /* :365 */
            show('PanelInPutManualEntry', !d.isManualEntryOnInputNotAllowed);
            /* :377-380 */
            $id('btnsave').disabled = !R.save;
            $id('btnUpdate').disabled = !R.update;
            $id('Print').disabled = !R.print;
            $id('btnVoucherReport').disabled = !R.print;
            /* :392-400 - label83 is the "Plant Name" caption; FIFO shows it and hides Pending For Rates */
            show('label83', FIFO);
            if (FIFO) show('btnPendingForRateInput', false);

            UOMLIST = d.uomSchedules || [];
            JOB_LOTS = d.jobLots || [];
            CROP_YEARS = d.cropYears || [];
            if (int(d.docNumber) > 0) setVal('txtdocnumber', d.docNumber);
            fill('cmbLotInput', JOB_LOTS, 'Id', 'JobLotDescription', true);
            fill('cmbCropYearInput', CROP_YEARS, 'Id', 'CropYear', true);
            fill('cmbPackingTypeInput', d.packingTypes, 'Id', 'PackTypeDesc', true);
            bindJobOrders(d.jobOrders);
            fill('cmbWareHouseInput', d.warehouses, 'Id', 'WareHouseName', true);
            fill('CmbProductionDepartment', d.departments, 'Id', 'WareHouseName', true);

            /* :452-466 */
            show('btnUpdateDetailInput', false);
            show('btnCancelInput', false);
            $id('wrapRemarksInput').style.width = '527px';
            if (!R.rateAndAmount) {
                $id('wrapRemarksInput').style.width = '793px';
                ['txtRateInput', 'lblRateInput', 'lblRateUom', 'txtAmountInput', 'lblAmountInput'].forEach(function (x) {
                    var e = $id(x); if (e) (e.closest('.pi-cell') || e).classList.add('is-hidden');
                });
                show('BtnGenerateRates', false);
                show('btnVoucherReport', false);
            }
            /* :467 */
            setDT('txtFromdateHistory', addDays(-3));
            setDT('txtToDateHistory', new Date());
            bindHistoryJobOrders(d.historyJobOrders);
            /* :469-478 - Rate UOM is disabled either way; Rate only under FIFO */
            $id('txtRateInput').disabled = FIFO;
            $id('cmbRateuomInput').disabled = true;
            renderDetail();
            gridBar(document.querySelector('.pi-gridbar[data-grid=tblDetail]'));
            gridBar(document.querySelector('.pi-gridbar[data-grid=tblHistory]'));
            gridBar(document.querySelector('.pi-gridbar[data-grid=tblIssuance]'));
            /* :381-390 - only ctrlGrdBar3 (grdStockConversionProductionDetail) is gated; the other bars stay enabled. */
            if (window.GridBar) {
                window.GridBar.attach($id('tblDetail'), null, {
                    canSaveLayout: R.saveLayout !== false, canPrint: !!R.gridPrint, canExport: !!R.gridExport,
                    canGroupCollapse: R.groupCollapse !== false, canGroupExpand: R.groupExpand !== false,
                    canChooseFields: !!R.rateAndAmount
                });
            }
            $id('txtDocdate').focus();
        }).catch(function (e) { msg(e.message); });
    }

    /* JobOrderNoFill:634 - Id, PlanCode, DocumentTypeId (hidden column, used by Save:1423). */
    function bindJobOrders(rows) {
        if (!rows || !rows.length) return;
        fill('CmbJobOrderNo', rows, 'Id', 'PlanCode', true, { 'document-type-id': 'DocumentTypeId' });
    }
    function jobOrderNoFill() {
        return getJson('/refresh').then(function (d) { WAGES_ACTIVE = !!d.wagesActiveForInput; bindJobOrders(d.jobOrders); });
    }

    /* GenerateDocNumberInput:1045 */
    function generateDocNumber() {
        return getJson('/serial').then(function (d) { if (int(d.docNumber) > 0) setVal('txtdocnumber', d.docNumber); })
            .catch(function (e) { msg(e.message); });
    }

    /* ================================================================ CmbJobOrderNo_Leave:1130 */
    function jobOrderLeave() {
        var id = int(val('CmbJobOrderNo'));
        return getJson('/job-order', { jobOrderId: id }).then(function (d) {
            var gl = d.glAccounts || [];
            if (gl.length) {
                fill('CmbWorkInProcess', gl, 'WorkInProccessAcId', 'WorkInProcessAc', false);
                fill('CmbWIPItem', gl, 'WipItemId', 'ItemName', false);
                fill('CmbWipWarehouse', gl, 'WipWareHouseId', 'WareHouseName', false);
                activateFirst('CmbWorkInProcess'); activateFirst('CmbWIPItem'); activateFirst('CmbWipWarehouse');
            } else {
                clearCombo('CmbWorkInProcess'); clearCombo('CmbWIPItem'); clearCombo('CmbWipWarehouse');
            }
            /* GetPlantFeeder:749 */
            var keepPlant = val('CmbPlantFeeder');
            var plants = d.plants || [];
            if (plants.length) {
                if (val('CmbJobOrderNo') !== '') {
                    fill('CmbPlantFeeder', plants, 'PlantId', 'PlantName', false);
                    if (plants.length === 1) activateFirst('CmbPlantFeeder');
                }
                if (int(keepPlant) > 0) setCombo('CmbPlantFeeder', keepPlant);
            } else {
                clearCombo('CmbPlantFeeder');
            }
            /* getJobOrderItems:796 */
            if (JOCWR) {
                JO_ITEMS = d.jobOrderItems || [];
                JO_ITEM_IDS = JO_ITEMS.map(function (x) { return int(x.ItemId); }).join(',');
                if (JO_ITEMS.length) fill('cmbItemInput', JO_ITEMS, 'ItemId', 'ItemName', true);
                else clearCombo('cmbItemInput');
            }
        }).catch(function (e) { msg(e.message); });
    }

    /* ======================================================= detail-entry events (:899-1243) */
    function docDate() { return getDT('txtDocdate'); }

    /* GetAvailableStockForInput:899 */
    function getAvailableStock() {
        return getJson('/stock', {
            warehouseId: int(val('cmbWareHouseInput')), itemId: int(val('cmbItemInput')), jobLotId: int(val('cmbLotInput')),
            cropYear: selText('cmbCropYearInput').trim(), docDate: docDate()
        }).then(function (d) {
            if (d && d.availableQty !== undefined) {
                setVal('txtBalanceQty', f3(num(d.availableQty)));
                setVal('txtBalanceWeight', f4(num(d.availableStock)));
            } else {
                setVal('txtBalanceQty', '0');
                setVal('txtBalanceWeight', '0');
            }
        }).catch(function (e) { msg(e.message); });
    }

    /* GetAvgRateByItemAndJoblot:1194 - Rate right only; the text is rate x 40, "#,##0.###", or "0". */
    function getAvgRate() {
        if (!R.rateAndAmount || suppressRate) return Promise.resolve();
        var seq = ++rateSeq;
        return getJson('/avg-rate', {
            itemId: int(val('cmbItemInput')), docDate: docDate(), recId: RECID, jobLotId: int(val('cmbLotInput')),
            cropYearId: int(val('cmbCropYearInput')), cropYear: selText('cmbCropYearInput'), warehouseId: int(val('cmbWareHouseInput')),
            stockUom: int(val('cmbItemUomInput')), packingTypeId: int(val('cmbPackingTypeInput')),
            qty: num(val('txtQtyInput')), netWeight: num(val('txtNetWeightInput'))
        }).then(function (d) {
            if (seq !== rateSeq || !d || d.skipped) return;
            var rate = num(d.rate);
            setText('txtRateInput', rate > 0 ? f3(rate * 40) : '0', amountInputCalu);
        }).catch(function (e) { if (seq === rateSeq) msg(e.message); });
    }

    /** Assign a text box and raise its TextChanged handler only when the text changed. */
    function setText(id, text, handler) {
        var e = $id(id); if (!e) return;
        if (e.value === text) return;
        e.value = text;
        if (handler) handler();
    }

    function uomEquivalent(selectId) {
        var id = val(selectId);
        for (var i = 0; i < ITEM_UOMS.length; i++) if (String(ITEM_UOMS[i].Id) === id) return num(ITEM_UOMS[i].Equivalent);
        return 0;
    }

    /* AmountInputCalu:935 */
    function amountInputCalu() {
        var nw = num(val('txtNetWeightInput')), rateUom = int(val('cmbRateuomInput')), rate = num(val('txtRateInput'));
        if (nw > 0 && rateUom > 0 && rate > 0) {
            var amount = nw / uomEquivalent('cmbRateuomInput') * rate;
            setVal('txtAmountInput', amount > 0 ? fmtOrNet(amount, fAmt) : '0');
        } else {
            setVal('txtAmountInput', '0');
        }
    }

    /* CalculateWeight:2770 - the ActiveControl's Tag decides which of EbUnit / EbTotal is derived. */
    var inCalc = 0;
    function calculateWeight() {
        if (inCalc > 4) return;
        inCalc++;
        try {
            if (int(val('cmbItemUomInput')) > 0) {
                var eq = uomEquivalent('cmbItemUomInput'), qty = num(val('txtQtyInput'));
                var weight = eq * qty;
                setText('txtNetWeightInput', fmtOrNet(weight, f3), netWeightChanged);
                var ebUnit = num(val('txtEbUnit')), ebTotal = num(val('txtEbTotal'));
                var tag = document.activeElement && document.activeElement.getAttribute('data-tag');
                if (tag === 'EbUnit') {
                    ebTotal = ebUnit * qty;
                    setText('txtEbTotal', fmtOrNet(ebTotal, f3), calculateWeight);
                } else if (tag === 'EbTotal') {
                    ebUnit = ebTotal / qty;
                    setText('txtEbUnit', fmtOrNet(ebUnit, f4), calculateWeight);
                } else {
                    ebUnit = qty > 0 ? ebTotal / qty : 0;
                    setText('txtEbUnit', fmtOrNet(ebUnit, f4), calculateWeight);
                }
                setVal('txtGrossWeight', fmtOrNet(weight + ebTotal, f3));
            } else {
                setText('txtNetWeightInput', '0', netWeightChanged);
                setVal('txtGrossWeight', '0');
            }
        } finally { inCalc--; }
    }

    /* txtNetWeightInput_TextChanged:965 */
    function netWeightChanged() { getAvgRate(); amountInputCalu(); }
    /* txtQtyInput_TextChanged:2811 */
    function qtyChanged() { getAvgRate(); calculateWeight(); }
    /* cmbItemUomInput_TextChanged:2757 */
    function itemUomChanged() { calculateWeight(); getAvgRate(); }

    /* bindUomInput:1075 - the item's rows of dtuomlst; Pack UOM text kept if present; Rate UOM =
       the first row whose Equivalent is 40. */
    function bindUomInput() {
        if (!UOMLIST.length) return;
        var packUom = selText('cmbItemUomInput');
        $id('cmbItemUomInput').value = '';
        $id('cmbRateuomInput').value = '';
        var item = int(val('cmbItemInput'));
        var rows = UOMLIST.filter(function (r) { return int(r.ItemId) === item; })
            .map(function (r) { return { Id: r.Id, PackUom: r.UOMCode, Equivalent: num(r.Equivalent) }; });
        if (rows.length) {
            ITEM_UOMS = rows;
            fill('cmbItemUomInput', rows, 'Id', 'PackUom', false);
            fill('cmbRateuomInput', rows, 'Id', 'PackUom', false);
            if (rows.some(function (r) { return r.PackUom === packUom; })) setByText('cmbItemUomInput', packUom);
            var forty = rows.filter(function (r) { return r.Equivalent === 40; });
            if (forty.length) $id('cmbRateuomInput').value = String(forty[0].Id);
        }
        itemUomChanged();
        amountInputCalu();
    }

    /* cmbItemInput_Leave:1180 */
    function itemLeave() {
        var p = getAvailableStock();
        bindUomInput();
        getAvgRate();
        return p;
    }
    /* cmbWareHouseInput_Leave / cmbCropYearInput_Leave / cmbLotInput_Leave */
    function stockAndRateLeave() { getAvailableStock(); getAvgRate(); }

    /* cmbWareHouseInput_ValueChanged:1165 - the item list is the warehouse's, unless the job order
       decides the items (JobOrderCreatewithoutRates). */
    function warehouseValueChanged() {
        if (val('cmbWareHouseInput') === '' || JOCWR) return Promise.resolve();
        return getJson('/items', { warehouseId: int(val('cmbWareHouseInput')) }).then(function (rows) {
            if (rows && rows.length) fill('cmbItemInput', rows, 'ItemId', 'ItemName', true);
            else clearCombo('cmbItemInput');
        }).catch(function (e) { msg(e.message); });
    }

    /* FormValidationOfDetailPortionInPut:1285 */
    function detailValidation() {
        function no(m, f) { msg(m); if (f) { if ($id(f).tagName === 'SELECT') focusCombo(f); else $id(f).focus(); } return false; }
        if (val('cmbWareHouseInput') === '') return no('Ware house Field Required', 'cmbWareHouseInput');
        if (val('cmbItemInput') === '') return no('ItemName Field Required', 'cmbItemInput');
        if (int(val('cmbItemUomInput')) <= 0) return no('UOM Field Required', 'cmbItemUomInput');
        if (val('cmbCropYearInput') === '') return no('CropYear Field Required', 'cmbCropYearInput');
        if (val('cmbLotInput') === '') return no('JobLot Field Required', 'cmbLotInput');
        if (val('cmbPackingTypeInput') === '') return no('Bag Type Field Required', 'cmbPackingTypeInput');
        if (val('txtQtyInput').trim() === '' || num(val('txtQtyInput').trim()) === 0) return no('Bag Quantity Field Required', 'txtQtyInput');
        if (R.rateAndAmount) {
            if (val('txtRateInput').trim() === '' || num(val('txtRateInput').trim()) === 0) return no('Rate Field Required', 'txtRateInput');
            if (val('cmbRateuomInput') === '' || num(val('cmbRateuomInput')) === 0) {
                return no('RateUom Field Required! ' + selText('cmbItemInput') + '40Kg Uom not Defined', 'cmbRateuomInput');
            }
            if (val('txtAmountInput').trim() === '' || num(val('txtAmountInput').trim()) === 0) return no('Amount Field Required', 'txtAmountInput');
        }
        if (val('txtNetWeightInput').trim() === '' || num(val('txtNetWeightInput').trim()) === 0) return no('NetWeight Field Required', 'txtNetWeightInput');
        if (val('txtGrossWeight').trim() === '' || num(val('txtGrossWeight').trim()) === 0) return no('GrossWeight Field Required', 'txtGrossWeight');
        return true;
    }

    /* btnAddInput_Click:2028 */
    function btnAddInput() {
        if (!detailValidation()) return;
        var rateUomEq = int(val('cmbRateuomInput')) > 0 ? int(uomEquivalent('cmbRateuomInput')) : 0;
        TABLE.push({
            Id: 0, RefDocumentTypeId: 0, RefDocumentType: '', RefDocIdNo: 0, RefDocSubIdNo: 0, SupplierCustomerId: 0,
            PartyName: '', DocDate: 0, DocNo: 0,
            WareHouseId: val('cmbWareHouseInput'), WareHouse: selText('cmbWareHouseInput'),
            ItemId: val('cmbItemInput'), Item: selText('cmbItemInput'),
            ItemUOMId: val('cmbItemUomInput'), ItemUOM: selText('cmbItemUomInput'),
            ItemEquivalent: uomEquivalent('cmbItemUomInput'),
            CropYear: selText('cmbCropYearInput'), JobLotId: val('cmbLotInput'), JobLot: selText('cmbLotInput'),
            PackingTypeId: val('cmbPackingTypeInput'), PackingType: selText('cmbPackingTypeInput'),
            BalanceQty: num(val('txtBalanceQty')), BalanceWeight: num(val('txtBalanceWeight')),
            Quantity: num(val('txtQtyInput')), Weight: num(val('txtNetWeightInput')),
            EbUnit: num(val('txtEbUnit')), EbTotal: num(val('txtEbTotal')), GrossWeight: num(val('txtGrossWeight')),
            Rate: num(val('txtRateInput')), RateForCheck: num(val('txtRateInput')),
            RateUOMId: val('cmbRateuomInput'), RateUOM: selText('cmbRateuomInput'), RateUOMForCheck: rateUomEq,
            ItemAmount: num(val('txtAmountInput')), Remarks: val('txtRemarksInput')
        });
        renderDetail();
        resetInputFields();
        focusCombo('cmbWareHouseInput');
    }

    /* btnUpdateDetailInput_Click:2048 */
    function btnUpdateDetailInput() {
        if (!detailValidation()) return;
        var r = TABLE[UPDATE_INDEX];
        if (!r) { msg('There is no row at position ' + UPDATE_INDEX + '.'); return; }
        r.WareHouseId = val('cmbWareHouseInput'); r.WareHouse = selText('cmbWareHouseInput').trim();
        r.JobLotId = val('cmbLotInput'); r.JobLot = selText('cmbLotInput').trim();
        r.PackingTypeId = val('cmbPackingTypeInput'); r.PackingType = selText('cmbPackingTypeInput').trim();
        r.ItemId = val('cmbItemInput'); r.Item = selText('cmbItemInput').trim();
        r.ItemUOMId = val('cmbItemUomInput'); r.ItemUOM = selText('cmbItemUomInput').trim();
        r.ItemEquivalent = uomEquivalent('cmbItemUomInput');
        r.CropYear = selText('cmbCropYearInput').trim();
        r.Quantity = num(val('txtQtyInput').trim());
        r.BalanceQty = num(val('txtBalanceQty').trim());
        r.BalanceWeight = num(val('txtBalanceWeight').trim());
        r.Weight = num(val('txtNetWeightInput').trim());
        r.EbUnit = num(val('txtEbUnit').trim());
        r.EbTotal = num(val('txtEbTotal').trim());
        r.GrossWeight = num(val('txtGrossWeight').trim());
        r.Rate = num(val('txtRateInput').trim());
        r.RateForCheck = num(val('txtRateInput').trim());
        r.RateUOM = selText('cmbRateuomInput').trim();
        r.RateUOMForCheck = netG(uomEquivalent('cmbRateuomInput'));
        r.RateUOMId = val('cmbRateuomInput');
        r.ItemAmount = num(val('txtAmountInput').trim());
        r.Remarks = val('txtRemarksInput').trim();
        resetInputFields();
        focusCombo('cmbWareHouseInput');
        renderDetail();
    }

    /* btnCancelInput_Click:2091 */
    function btnCancelInput() {
        show('btnAddInput', true); show('btnUpdateDetailInput', false); show('btnCancelInput', false);
        resetInputFields();
    }

    /* ResetInputFields:1704 - EbUnit, EbTotal and Gross Weight are NOT cleared (the Gross text ends
       at "0" because clearing the Pack UOM runs CalculateWeight). */
    function resetInputFields() {
        suppressRate = true;
        try {
            $id('cmbWareHouseInput').value = '';
            clearCombo('cmbItemInput');
            clearCombo('cmbItemUomInput');
            ITEM_UOMS = [];
            itemUomChanged();
            $id('cmbCropYearInput').value = '';
            $id('cmbLotInput').value = '';
            $id('cmbPackingTypeInput').value = '';
            setVal('txtBalanceQty', '');
            setVal('txtBalanceWeight', '');
            setText('txtQtyInput', '', qtyChanged);
            setText('txtNetWeightInput', '', netWeightChanged);
            setVal('txtRemarksInput', '');
            setText('txtRateInput', '', amountInputCalu);
            clearCombo('cmbRateuomInput');
            setVal('txtAmountInput', '');
        } finally { suppressRate = false; rateSeq++; }
        show('btnAddInput', true); show('btnUpdateDetailInput', false); show('btnCancelInput', false);
    }

    /* ===================================================== grid events (:1751-1981) */
    function isLoaderRow(r) { return int(r.RefDocumentTypeId) > 0 || int(r.RefDocIdNo) > 0 || int(r.RefDocSubIdNo) > 0; }
    function allRefsPositive(r) { return int(r.RefDocumentTypeId) > 0 && int(r.RefDocIdNo) > 0 && int(r.RefDocSubIdNo) > 0; }
    function noRefs(r) { return int(r.RefDocumentTypeId) <= 0 && int(r.RefDocIdNo) <= 0 && int(r.RefDocSubIdNo) <= 0; }
    function decimalCell(v) {
        if (!isFinite(v)) throw new Error('Value was either too large or too small for a Decimal.');
        return v;
    }

    function fifoRowRate(item) {
        return getJson('/fifo-rate', {
            docDate: docDate(), warehouseId: int(item.WareHouseId), itemId: int(item.ItemId), jobLotId: int(item.JobLotId),
            packingTypeId: int(item.PackingTypeId), cropYear: item.CropYear == null ? '' : String(item.CropYear),
            qty: num(item.Quantity), netWeight: num(item.Weight)
        }).then(function (d) { return num(d.rate); });
    }

    /* grdStockConversionProductionDetail_CellUpdated:1751 */
    function cellUpdated(i, key, text) {
        var item = TABLE[i];
        if (!item) return Promise.resolve();
        item[key] = num(text);
        var chain = Promise.resolve();
        if (key === 'Quantity') {
            chain = chain.then(function () {
                var qty = num(item.Quantity), uom = num(item.ItemEquivalent), weight = 0;
                if (num(item.BalanceQty) > 0 && num(item.BalanceQty) < qty && isLoaderRow(item)) {
                    item.Quantity = 0; item.Weight = 0; qty = 0;
                    msg('Qty cannot greater than BalanceQty Please check!');
                }
                if (allRefsPositive(item)) {
                    weight = num(item.BalanceWeight) / num(item.BalanceQty) * qty;
                    if (num(item.BalanceWeight) < weight) {
                        weight = 0; item.Weight = 0;
                        msg('Weight cannot greater than Balance Weight Please check!');
                    }
                } else {
                    weight = qty * uom;
                }
                item.Weight = weight;
                var p = Promise.resolve();
                if (FIFO && noRefs(item)) p = fifoRowRate(item).then(function (rate) { item.Rate = rate * 40.0; });
                return p.then(function () {
                    var amount = weight / num(item.RateUOMForCheck) * num(item.Rate);
                    item.ItemAmount = decimalCell(amount);
                    item.GrossWeight = weight + num(item.EbTotal);
                });
            });
        }
        if (key === 'Weight') {
            chain = chain.then(function () {
                var weight2 = num(item.Weight);
                if (num(item.BalanceWeight) < weight2 && isLoaderRow(item)) {
                    item.Weight = 0; weight2 = 0;
                    msg('Weight cannot greater than BalanceWeight Please check!');
                }
                var p = Promise.resolve();
                if (FIFO && noRefs(item)) p = fifoRowRate(item).then(function (rate) { item.Rate = rate * 40.0; });
                return p.then(function () {
                    var amount2 = weight2 / num(item.RateUOMForCheck) * num(item.Rate);
                    item.ItemAmount = decimalCell(isFinite(amount2) ? roundAway(amount2, amountDec()) : amount2);
                    item.GrossWeight = weight2 + num(item.EbTotal);
                });
            });
        }
        if (key === 'Rate' || key === 'RateUOM') {
            chain = chain.then(function () {
                if (int(item.RefDocumentTypeId) > 0 || int(item.RefDocIdNo) > 0) {
                    item.RateUOM = item.RateUOMForCheck;
                    item.Rate = item.RateForCheck;
                    throw new Error('The item rate or rate unit of measure (RateUom) is not editable because this entry is loaded from the loader.');
                }
                var w3 = num(item.Weight), ru3 = num(item.RateUOM), r3 = num(item.Rate);
                item.ItemAmount = decimalCell((ru3 !== 0 && r3 !== 0) ? w3 / ru3 * r3 : 0);
            });
        }
        if (key === 'EbUnit') {
            chain = chain.then(function () {
                var ebTotal3 = num(item.EbUnit) * num(item.Quantity);
                item.EbTotal = ebTotal3;
                item.GrossWeight = num(item.Weight) + ebTotal3;
            });
        }
        if (key === 'EbTotal') {
            chain = chain.then(function () {
                var ebTotal4 = num(item.EbTotal), qty3 = num(item.Quantity);
                item.EbUnit = qty3 > 0 ? ebTotal4 / qty3 : 0;
                item.GrossWeight = num(item.Weight) + ebTotal4;
            });
        }
        return chain.catch(function (e) { msg(e.message); }).then(renderDetail);
    }

    /* grdStockConversionProductionDetail_ColumnButtonClick:1908 */
    function deleteRow(i) {
        var item = TABLE[i];
        if (!item) return;
        if (!window.confirm('Are you sure to Delete?')) return;
        if (int(item.Id) !== 0) REMOVE_IDS = REMOVE_IDS + ',' + String(item.Id);
        TABLE.splice(i, 1);
        if (CURRENT_ROW >= TABLE.length) CURRENT_ROW = TABLE.length - 1;
        renderDetail();
    }

    /* grdStockConversionProductionDetail_DoubleClick:1941 - the setters run their TextChanged
       handlers in the desktop's order; the synchronous rate lookups those raise are overwritten by
       the row's own Rate a few lines later, so they are not issued here. */
    function rowDoubleClick(i) {
        var item = TABLE[i];
        if (!item) return;
        if (isLoaderRow(item)) { msg('The record was not updated as it was loaded directly from the Loader.'); return; }
        UPDATE_INDEX = i;
        $id('cmbWareHouseInput').value = hasValue('cmbWareHouseInput', item.WareHouseId) ? String(item.WareHouseId) : '';
        warehouseValueChanged().then(function () {
            setCombo('cmbItemInput', item.ItemId);
            suppressRate = true;
            return getAvailableStock().then(function () {
                try {
                    bindUomInput();
                    $id('cmbItemUomInput').value = hasValue('cmbItemUomInput', int(item.ItemUOMId)) ? String(int(item.ItemUOMId)) : '';
                    itemUomChanged();
                    setByText('cmbCropYearInput', String(item.CropYear == null ? '' : item.CropYear));
                    setCombo('cmbLotInput', item.JobLotId);
                    setCombo('cmbPackingTypeInput', item.PackingTypeId);
                    setVal('txtBalanceQty', f3(num(item.BalanceQty)));
                    setVal('txtBalanceWeight', f3(num(item.BalanceWeight)));
                    setText('txtQtyInput', f3(num(item.Quantity)), qtyChanged);
                    setText('txtNetWeightInput', f3(num(item.Weight)), netWeightChanged);
                    setText('txtEbUnit', f3(num(item.EbUnit)), calculateWeight);
                    setText('txtEbTotal', f3(num(item.EbTotal)), calculateWeight);
                    setVal('txtGrossWeight', f3(num(item.GrossWeight)));
                    setText('txtRateInput', netG(num(item.Rate)), amountInputCalu);
                    $id('cmbRateuomInput').value = hasValue('cmbRateuomInput', item.RateUOMId) ? String(item.RateUOMId) : '';
                    amountInputCalu();
                    setVal('txtAmountInput', fAmt(num(item.ItemAmount)));
                    setVal('txtRemarksInput', item.Remarks == null ? '' : String(item.Remarks));
                } finally { suppressRate = false; rateSeq++; }
                show('btnAddInput', false); show('btnUpdateDetailInput', true); show('btnCancelInput', true);
                focusCombo('cmbWareHouseInput');
            });
        }).catch(function (e) { suppressRate = false; msg(e.message); });
    }

    /* LoadDataDetailfromPurchaseInvoivce:2127 - a row already in the grid (same RefDocumentTypeId,
       RefDocIdNo, RefDocSubIdNo) only has its BALANCE increased; its Quantity/Weight stay. */
    function mergeIssuance(rows) {
        if (!rows || !rows.length) return;
        rows.forEach(function (d) {
            var found = false;
            TABLE.forEach(function (item) {
                if (int(d.RefDocumentTypeId) === int(item.RefDocumentTypeId) && int(d.RefDocIdNo) === int(item.RefDocIdNo)
                    && int(d.RefDocSubIdNo) === int(item.RefDocSubIdNo)) {
                    item.BalanceWeight = num(item.BalanceWeight) + num(d.Weight);
                    item.BalanceQty = num(item.BalanceQty) + num(d.ItemQty);
                    found = true;
                }
            });
            if (!found) {
                TABLE.push({
                    Id: 0, RefDocumentTypeId: d.RefDocumentTypeId, RefDocumentType: d.RefDocumentType, RefDocIdNo: d.RefDocIdNo,
                    RefDocSubIdNo: d.RefDocSubIdNo, SupplierCustomerId: d.SupplierCustomerId, PartyName: d.PartyName,
                    DocDate: dMMMyyyy(d.DocDate), DocNo: d.DocNo, WareHouseId: d.WareHouseId, WareHouse: d.WareHouse,
                    ItemId: d.ItemId, Item: d.ItemName, ItemUOMId: d.ItemUOMId, ItemUOM: d.PackUom, ItemEquivalent: num(d.ItemUOM),
                    CropYear: d.CropYear, JobLotId: d.JobLotId, JobLot: d.JobLotCode, PackingTypeId: d.PackingTypeId,
                    PackingType: d.PackTypeCode, BalanceQty: num(d.ItemQty), BalanceWeight: num(d.Weight),
                    Quantity: num(d.ItemQty), Weight: num(d.Weight), EbUnit: 0, EbTotal: 0, GrossWeight: num(d.Weight),
                    Rate: num(d.ItemRate), RateForCheck: num(d.ItemRate), RateUOMId: d.RateUOMId, RateUOM: d.RateUOM,
                    RateUOMForCheck: d.Equivalent, ItemAmount: num(d.ItemAmount), Remarks: ''
                });
            }
        });
        renderDetail();
    }

    /* ============================================================ reset / refresh (:1672, :1735) */
    function reset() {
        REMOVE_IDS = ''; RECID = 0; VOUCHERHEADID = 0;
        setVal('txtRemarks', '');
        show('btnsave', true); show('btnUpdate', false);
        $id('CmbJobOrderNo').disabled = false;
        TABLE = []; CURRENT_ROW = -1; UPDATE_INDEX = -1;
        renderDetail();
        generateDocNumber();
        clearCombo('CmbJobOrderNo');
        jobOrderNoFill().catch(function (e) { msg(e.message); });
        $id('CmbPlantFeeder').value = '';
        clearCombo('CmbMoveOrderTicket');
        setVal('txtMoveOrderNetWeight', ''); setVal('txtMoveOrderAlreadUsedWeight', ''); setVal('txtMoveOrderBalWeight', '');
        resetInputFields();
    }

    function btnRefresh() {
        return jobOrderNoFill().then(function () {
            /* CropYearInput / JobLotInput re-bind the tables cached at Load - they are not re-read. */
            fill('cmbCropYearInput', CROP_YEARS, 'Id', 'CropYear', true);
            fill('cmbLotInput', JOB_LOTS, 'Id', 'JobLotDescription', true);
        });
    }

    /* ================================================================== Save / Update (:1385) */
    function formValidation() {
        function no(m, f) { msg(m); if (f) { if ($id(f).tagName === 'SELECT') focusCombo(f); else $id(f).focus(); } return false; }
        var dn = val('txtdocnumber').trim();
        if (dn === '' || val('txtdocnumber') === '0') return no('document Number Field Required', 'txtdocnumber');
        if (val('CmbJobOrderNo') === '') return no('Job Order Number Field Required', 'CmbJobOrderNo');
        if (val('CmbPlantFeeder') === '') return no('Plant Name Field Required', 'CmbPlantFeeder');
        if (val('CmbWorkInProcess') === '') return no('WorkInProcess Field Required', 'CmbWorkInProcess');
        if (val('CmbWIPItem') === '') return no('WIP Item Field Required', 'CmbWIPItem');
        if (val('CmbWipWarehouse') === '' || int(val('CmbWipWarehouse')) === 0) return no('WIP WareHouse Field Required', 'CmbWipWarehouse');
        return true;
    }
    /* ValidateMoveOrder:1365 */
    function validateMoveOrder() {
        if (int(val('CmbMoveOrderTicket')) > 0) {
            var total = 0;
            TABLE.forEach(function (r) { total += num(r.GrossWeight); });
            var available = num(val('txtMoveOrderBalWeight'));
            if (total > available) {
                throw new Error("The total weight in the grid '" + netG(total) + "' exceeds the available Move Order balance weight '" + netG(available) + "'.");
            }
        }
    }
    function insertInput(btn) {
        if (!formValidation()) return Promise.resolve();
        try { validateMoveOrder(); } catch (e) { msg(e.message); return Promise.resolve(); }
        if (RECID > 0) { if (!window.confirm('Are you sure to Update?')) return Promise.resolve(); }
        else if (!window.confirm('Are you sure to Save?')) return Promise.resolve();
        if (!TABLE.length) { msg('Enter Detail First ...'); return Promise.resolve(); }
        var body = {
            id: RECID, docNumber: val('txtdocnumber'), docDate: docDate(), remarks: val('txtRemarks'),
            wipAccountId: int(val('CmbWorkInProcess')), wipItemId: int(val('CmbWIPItem')), wipWarehouseId: int(val('CmbWipWarehouse')),
            departmentId: int(val('CmbProductionDepartment')), jobOrderId: int(val('CmbJobOrderNo')),
            jobOrderText: selText('CmbJobOrderNo').trim(), jobOrderDocumentTypeId: int(selAttr('CmbJobOrderNo', 'document-type-id')),
            plantId: int(val('CmbPlantFeeder')), moveOrderId: int(val('CmbMoveOrderTicket')),
            inputDetailRowsRemoveIds: REMOVE_IDS,
            rows: TABLE.map(function (r) { var o = {}; TABLE_COLS.forEach(function (k) { o[k] = r[k]; }); return o; })
        };
        return busy(btn, function () {
            return postJson('/save', body).then(function (d) {
                msg(d.message);
                if (!d.success) return;
                if (d.openWages) {
                    if (d.wagesMessage) msg(d.wagesMessage);
                    openWages(int(d.id), num(d.grossWeightTotal));
                } else {
                    reset();
                }
            });
        });
    }
    function btnSave() { RECID = 0; return insertInput('btnsave'); }
    function btnUpdate() { return insertInput('btnUpdate'); }

    /* frmwagesBillHeader(RefDocTypeId 80, RefDocId, GrossWeightTotal) - the Labour Wages page, modal. */

    /* frmwagesBillHeader as a modal (ShowDialog): /production/wages-bill in an overlay iframe; the
       caller continues (reset) only after the dialog closes, as the desktop blocks until then. */
    function p280OpenWages(refDocTypeId, refDocId, grossWeightTotal, onClosed) {
        var ov = document.createElement('div');
        ov.style.cssText = 'position:fixed;inset:0;z-index:9800;background:rgba(0,0,0,.35);';
        var fr = document.createElement('iframe');
        fr.src = '/production/wages-bill?' + new URLSearchParams({ refDocTypeId: refDocTypeId, refDocId: refDocId,
                                                                  grossWeightTotal: grossWeightTotal || 0 });
        fr.style.cssText = 'position:absolute;inset:12px;width:calc(100% - 24px);height:calc(100% - 24px);border:1px solid #555;background:#fff;';
        ov.appendChild(fr); document.body.appendChild(ov);
        window.P280WagesClosed = function (r) { ov.remove(); window.P280WagesClosed = null; if (onClosed) onClosed(r); };
    }
    function openWages(id, gross) {
        p280OpenWages(80, id, gross, function () { reset(); });
        return;
        $id('wagesFrame').src = '/accounts/vouchers/labour-wages?refDocTypeId=80&refDocId=' + encodeURIComponent(id)
            + '&grossWeightTotal=' + encodeURIComponent(gross);
        openModal('dlgWages');
        $id('dlgWages').dataset.afterClose = 'reset';
    }

    /* ======================================================================= ReadByIdInput:1575 */
    function readByIdInput(id) {
        RECID = int(id);
        show('btnsave', false); show('btnUpdate', true);
        REMOVE_IDS = '';
        return getJson('/' + RECID).then(function (d) {
            if (!d) return;
            var h = d.header || {};
            if (col(h, 'IsApproved') === true || col(h, 'IsApproved') === 1) throw new Error("You Can't Update Approved Record");
            var status = col(h, 'PlanStatus') == null ? '' : String(col(h, 'PlanStatus'));
            if (status !== 'In Process') throw new Error("You Can't Update Record with " + status + ' JobPlanStatus');
            setVal('txtdocnumber', col(h, 'DocCode') == null ? '' : String(col(h, 'DocCode')));
            setDT('txtDocdate', String(col(h, 'DocDate') || '').replace(' ', 'T'));
            setVal('txtRemarks', col(h, 'MainRemarks') == null ? '' : String(col(h, 'MainRemarks')));
            setCombo('CmbProductionDepartment', col(h, 'EBDepartmentId'));
            setCombo('CmbJobOrderNo', col(h, 'InvJobOrderId'));
            var details = d.details || [];
            if (details.length) {
                var first = details[0];
                bindMoveOrderTicket(int(col(first, 'MoveOrderDocId')), int(col(first, 'WbTicketNo')));
                setVal('txtMoveOrderNetWeight', fHash(col(first, 'TotalWbWeight')));
                setVal('txtMoveOrderAlreadUsedWeight', fHash(col(first, 'UsedWbWeight')));
                setVal('txtMoveOrderBalWeight', fHash(col(first, 'AvailableWbWeight')));
            }
            return jobOrderLeave().then(function () {
                setCombo('CmbPlantFeeder', col(h, 'PlantId'));
                TABLE = [];
                var anyIssue = false;
                details.forEach(function (x) {
                    if (col(x, 'EntryType') !== 'Issue') return;
                    anyIssue = true;
                    TABLE.push({
                        Id: col(x, 'Id'), RefDocumentTypeId: col(x, 'RefDocumentTypeId'), RefDocumentType: col(x, 'RefDocumentType'),
                        RefDocIdNo: col(x, 'RefDocNoId'), RefDocSubIdNo: col(x, 'RefDocSubIdNo'),
                        SupplierCustomerId: col(x, 'SupplierCustomerId'), PartyName: col(x, 'PartyName'),
                        DocDate: dMMMyy(col(x, 'RefDocDate')), DocNo: col(x, 'RefDocNo'),
                        WareHouseId: col(x, 'WarehouseId'), WareHouse: col(x, 'WareHouseName'),
                        ItemId: col(x, 'ItemId'), Item: col(x, 'ItemName'), ItemUOMId: col(x, 'ItemUomId'), ItemUOM: col(x, 'PackUom'),
                        ItemEquivalent: num(col(x, 'Equivalent')), CropYear: col(x, 'CropBatch'), JobLotId: col(x, 'JobLotId'),
                        JobLot: col(x, 'JobLotDescription'), PackingTypeId: col(x, 'PackingtypeId'), PackingType: col(x, 'PackTypeDesc'),
                        /* :1621 - BalanceQty / BalanceWeight are filled with the row's own Qty / Weight */
                        BalanceQty: num(col(x, 'Qty')), BalanceWeight: num(col(x, 'Weight')),
                        Quantity: num(col(x, 'Qty')), Weight: num(col(x, 'Weight')),
                        EbUnit: num(col(x, 'EbUnit')), EbTotal: num(col(x, 'EbTotal')), GrossWeight: num(col(x, 'GrossWeight')),
                        Rate: roundEven(num(col(x, 'Rate')), 2), RateForCheck: roundEven(num(col(x, 'Rate')), 2),
                        RateUOMId: col(x, 'RateUOMId'), RateUOM: col(x, 'RateUOM'), RateUOMForCheck: col(x, 'RateEquivalent'),
                        ItemAmount: num(col(x, 'Amount')), Remarks: col(x, 'Remarks')
                    });
                });
                if (anyIssue) showView('form');
                CURRENT_ROW = -1;
                renderDetail();
                VOUCHERHEADID = int(d.voucherHeadId);
            });
        }).catch(function (e) { msg(e.message); });
    }

    /* BindMoveOrderTicket:2979 - one row (Id, TicketNo). */
    function bindMoveOrderTicket(id, ticket) {
        fill('CmbMoveOrderTicket', [{ Id: id, TicketNo: ticket }], 'Id', 'TicketNo', false);
        $id('CmbMoveOrderTicket').value = String(id);
    }

    /* ======================================================================= prints (:2168, :2585) */
    function print601() {
        if (RECID === 0) { msg('No Record Found For Display'); return; }
        CrystalPrint.open('prod-601', { id: RECID }, 'Print');
    }
    function voucher118() {
        if (VOUCHERHEADID === 0) { msg('VoucherId Not Found'); return; }
        CrystalPrint.open('acc-118', { id: VOUCHERHEADID }, 'btnVoucherReport');
    }

    /* BtnGenerateRates_Click -> AvgRateUpdateOnDocDateChange:2836 - stops at the first row that
       came from the loader (the desktop's `break`). */
    function generateRates() {
        if (!TABLE.length) return Promise.resolve();
        var i = 0;
        function next() {
            if (i >= TABLE.length) return Promise.resolve();
            var r = TABLE[i];
            if (int(r.RefDocumentTypeId) !== 0 || int(r.RefDocIdNo) !== 0 || int(r.RefDocSubIdNo) !== 0) return Promise.resolve();
            return getJson('/generate-rate', {
                docDate: docDate(), recId: RECID, itemId: int(r.ItemId), itemUomId: int(r.ItemUOMId), packingTypeId: int(r.PackingTypeId),
                warehouseId: int(r.WareHouseId), cropYear: r.CropYear == null ? '' : String(r.CropYear), jobLotId: int(r.JobLotId),
                qty: num(r.Quantity), weight: num(r.Weight)
            }).then(function (d) {
                var rate = num(d.rate), weight = num(r.Weight), eq = num(r.RateUOMForCheck);
                if (d.fifo) {
                    if (rate > 0) { r.Rate = rate; r.ItemAmount = decimalCell(weight / eq * rate); }
                    else { r.Rate = 0; r.ItemAmount = 0; }
                } else if (rate > 0) {
                    r.Rate = rate; r.ItemAmount = decimalCell(weight / eq * rate);
                }
                i++;
                return next();
            });
        }
        return next().then(renderDetail, function (e) { renderDetail(); throw e; });
    }

    /* =================================================================== History (tabPage4) */
    function bindHistoryJobOrders(rows) {
        if (rows && rows.length) fill('CmbJobOrderInputHistory', rows, 'Id', 'ReferenceName', false);
        else clearCombo('CmbJobOrderInputHistory');
    }
    var HIST_COLS = [
        { key: 'WagesPrint', caption: 'WagesPrint', btn: 'WagesPrint' },
        { key: 'Edit', caption: 'Edit', btn: 'Edit' },
        { key: 'DocNo', link: true },
        { key: 'WagesNo' },
        { key: 'DocDate', fmt: dMMMyyyy },
        { key: 'JobOrderNo' },
        { key: 'PlantName' },
        { key: 'WbTicketNo' },
        { key: 'TotalWbWeight', num: true, fmt: function (v) { return f2(num(v)); }, total: true },
        { key: 'EntryType' },
        { key: 'Remarks', width: 300 },
        { key: 'IsApproved' },
        { key: 'StartDate', fmt: dMMMyyyy },
        { key: 'EndDate', fmt: dMMMyyyy },
        { key: 'EntryDate', fmt: dMMMyyhmtt },
        { key: 'EntryUser' },
        { key: 'ModifyDate', fmt: dMMMyyhmtt },
        { key: 'ModifyUser' },
        { key: 'PlanStatus' }
    ];
    /* InputGridHistory:2301 - only DocumentTypeId 80 rows are kept. */
    function inputGridHistory() {
        return getJson('/history', {
            fromDate: getDT('txtFromdateHistory'), toDate: getDT('txtToDateHistory'),
            docNoFrom: int(val('txtFromNoHistory')), docNoTo: int(val('txtToDocNoHistory')),
            jobOrderId: int(val('CmbJobOrderInputHistory'))
        }).then(function (rows) {
            HIST_ROWS = [];
            (rows || []).forEach(function (x) {
                if (int(col(x, 'DocumentTypeId')) !== 80) return;
                HIST_ROWS.push({
                    Id: col(x, 'Id'), DocNo: col(x, 'DocCode'), WagesNo: col(x, 'WagesNo'), DocDate: col(x, 'DocDate'),
                    JobOrderNo: col(x, 'InvJobOrderNo'), PlantName: col(x, 'PlantName'), WbTicketNo: col(x, 'WbTicketNo'),
                    TotalWbWeight: col(x, 'TotalWbWeight'), EntryType: col(x, 'EntryType'), Remarks: col(x, 'MainRemarks'),
                    IsApproved: col(x, 'JobOrderApprovedStatus'), StartDate: col(x, 'StartDate'), EndDate: col(x, 'EndDate'),
                    EntryDate: col(x, 'EntryDate'), EntryUser: col(x, 'EntryUser'), ModifyDate: col(x, 'ModifyDate'),
                    ModifyUser: col(x, 'ModifyUser'), PlanStatus: col(x, 'PlanStatus'), WagesId: col(x, 'WagesId')
                });
            });
            HIST_CURRENT = -1;
            if (rows && rows.length) renderGrid($id('tblHistory'), HIST_COLS, HIST_ROWS, { current: HIST_CURRENT });
            else { $id('tblHistory').tHead.innerHTML = ''; $id('tblHistory').tBodies[0].innerHTML = ''; $id('tblHistory').tFoot.innerHTML = ''; }
        });
    }
    /* grdinputhistory_ColumnButtonClick:2408 (and a click on the DocNo link) */
    function historyEdit(i) {
        var r = HIST_ROWS[i]; if (!r) return;
        if (String(r.EntryType) !== 'Input') return;
        if (String(r.IsApproved) === 'Approved') { msg("You Can't Update Approved Record"); return; }
        if (String(r.PlanStatus == null ? '' : r.PlanStatus) !== 'In Process') {
            msg("You Can't Update Record with " + (r.PlanStatus == null ? '' : r.PlanStatus) + ' JobPlanStatus'); return;
        }
        RECID = int(r.Id);
        readByIdInput(RECID);
    }
    function historyWagesPrint(i) {
        var r = HIST_ROWS[i]; if (!r) return;
        if (int(r.WagesId) <= 0) { msg('No Record Found For Display'); return; }
        CrystalPrint.open('wages-002', { id: int(r.WagesId) });
    }
    /* grdinputhistory_SelectionChanged -> InputDetailByHeaderId:2609 */
    var HD_COLS_BASE = ['RefDocumentType', 'DocDate', 'DocNo', 'PartyName', 'WareHouseName', 'ItemName', 'PackUom', 'CropYear', 'JobLot',
        'PackingType', 'Qty', 'Weight', 'Rate', 'RateUom', 'Amount', 'Remarks'];
    function historyDetail(i) {
        var r = HIST_ROWS[i]; if (!r) return;
        getJson('/' + int(r.Id)).then(function (d) {
            var rows = [];
            ((d && d.details) || []).forEach(function (x) {
                if (col(x, 'EntryType') !== 'Issue') return;
                rows.push({
                    RefDocumentType: col(x, 'RefDocumentType'), DocDate: dMMMyy(col(x, 'RefDocDate')), DocNo: col(x, 'RefDocNo'),
                    PartyName: col(x, 'PartyName'), WareHouseName: col(x, 'WareHouseName'), ItemName: col(x, 'ItemName'),
                    PackUom: col(x, 'PackUom'), CropYear: col(x, 'CropBatch'), JobLot: col(x, 'JobLotDescription'),
                    PackingType: col(x, 'PackTypeDesc'), Qty: num(col(x, 'Qty')), Weight: num(col(x, 'Weight')),
                    Rate: roundEven(num(col(x, 'Rate')), 2), RateUom: col(x, 'RateUOM'), Amount: num(col(x, 'Amount')),
                    Remarks: col(x, 'Remarks')
                });
            });
            var cols = HD_COLS_BASE.map(function (k) {
                var c = { key: k };
                if (k === 'Qty' || k === 'Weight' || k === 'Amount') { c.num = true; c.fmt = fHash; c.total = true; }
                if (k === 'Rate') { c.num = true; c.fmt = function (v) { return netG(num(v)); }; }
                if (!R.rateAndAmount && (k === 'Rate' || k === 'RateUom' || k === 'Amount')) c.hidden = true;
                return c;
            });
            renderGrid($id('tblHistoryDetail'), cols, rows, {});
        }).catch(function (e) { msg(e.message); });
    }
    /* ResetInputhistory:2261 */
    function resetInputHistory() {
        setVal('txtFromNoHistory', ''); setVal('txtToDocNoHistory', '');
        $id('CmbJobOrderInputHistory').value = '';
        if (S && S.financialYearStart) setDT('txtFromdateHistory', String(S.financialYearStart).replace(' ', 'T'));
        $id('txtFromdateHistory').focus();
    }

    /* ======================================================= tabControl2 (Alignment Bottom) */
    function showView(which) {
        var form = which === 'form';
        show('viewForm', form); show('viewHistory', !form);
        $id('tabForm').classList.toggle('is-active', form);
        $id('tabHistory').classList.toggle('is-active', !form);
        /* tabControl2_SelectedIndexChanged:2242 */
        if (form) $id('txtDocdate').focus(); else $id('txtFromdateHistory').focus();
    }

    /* ======================================================= modal plumbing */
    function openModal(id) { $id(id).classList.add('is-open'); }
    function closeModal(id) {
        var m = $id(id);
        if (!m.classList.contains('is-open')) return;
        m.classList.remove('is-open');
        var cb = m._onClose; m._onClose = null;
        if (id === 'dlgWages') { $id('wagesFrame').src = 'about:blank'; if (m.dataset.afterClose === 'reset') { m.dataset.afterClose = ''; reset(); } }
        if (cb) cb();
    }
    function topModal() {
        var open = Array.prototype.filter.call(document.querySelectorAll('.pi-modal.is-open'), function () { return true; });
        return open.length ? open[open.length - 1].id : null;
    }

    /* =================================================== LoadavailableTransactionsForIssuance */
    var ISSUE_ROWS = [], ISSUE_RESULT = null, ISSUE_BROKEN = false, ISSUE_SHELL_RATE = false;
    var ISSUE_HIDDEN = ['RefDocumentTypeId', 'CropYearId', 'RefDocIdNo', 'RefDocSubIdNo', 'ItemId', 'ItemUom', 'JobLotId', 'InvPackingTypeId',
        'WarehouseId', 'RateUomId', 'ReserveWeight', 'PackSize', 'Equivalent', 'SupplierCustomerId', 'StockWeightOut'];
    var ISSUE_COLS = ['RefDocumentTypeId', 'RefDocIdNo', 'RefDocSubIdNo', 'RefDocumentType', 'DocDate', 'DocCodeNo', 'ManualNo', 'GrnNo',
        'SupplierCustomerId', 'SupplierCustomerName', 'VehicleNo', 'BiltyNo', 'GpNo', 'WarehouseId', 'WareHouseCode', 'RefWarehouse', 'ItemId',
        'ItemName', 'ItemCode', 'CropYearId', 'CropBatch', 'JobLotId', 'JobLotCode', 'InvPackingTypeId', 'PackingType', 'ItemUom', 'PackUom',
        'PackSize', 'QtyIn', 'QtyOut', 'QtyBalance', 'WeightIn', 'WeightOut', 'WeightBalance', 'ReserveWeight', 'StockWeightOut', 'AVgRate',
        'RateUom', 'Equivalent', 'RateUomId', 'ItemAmount', 'Remarks'];
    var ISSUE_DECIMAL = { PackSize: 1, QtyIn: 1, QtyOut: 1, QtyBalance: 1, WeightIn: 1, WeightOut: 1, WeightBalance: 1, ReserveWeight: 1,
        StockWeightOut: 1, AVgRate: 1, Equivalent: 1, ItemAmount: 1 };

    /* btnLoadInvoices_Click:2106 */
    function btnLoadInvoices() {
        if (JOCWR && (val('CmbJobOrderNo') === '' || int(val('CmbJobOrderNo')) === 0)) { msg('Job Order Select First'); return Promise.resolve(); }
        ISSUE_ROWS = []; ISSUE_RESULT = []; ISSUE_BROKEN = false;
        renderIssuance();
        setVal('ldSelectedStock', ''); setVal('ldSelectedQty', '');
        $id('dlgIssuance')._onClose = function () { mergeIssuance(ISSUE_RESULT); };
        openModal('dlgIssuance');
        /* LoadInvoices_Load:128 */
        return getJson('/loader/lookups').then(function (d) {
            ISSUE_SHELL_RATE = !!d.shellRateRight;
            if (d.rateRowMissing) {
                /* lstRights.Where(Rate).ToList()[0] on an empty list - Load stops here on the desktop */
                ISSUE_BROKEN = true;
                msg('Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index');
                return;
            }
            if (d.financialYearStart) setDT('ldFromDate', String(d.financialYearStart).replace(' ', 'T'));
            setDT('ldToDate', new Date());
            var L = d.lists || {};
            fill('ldParentCategory', L.ParentCategories, 'Id', 'name', false);
            fill('ldItemCategory', L.ItemCategories, 'Id', 'name', false);
            fill('ldItemType', L.ItemTypes, 'Id', 'name', false);
            fill('ldJobLot', L.JobLot, 'Id', 'name', false);
            fill('ldCropYear', L.CropYear, 'Id', 'name', false);
            fill('ldWarehouse', L.Warehouse, 'Id', 'name', false);
            fill('ldRefDocument', L.DocumentType, 'Id', 'name', false);
            fill('ldSupplier', L.Supplier_Customer, 'Id', 'name', false);
            /* StockComboFill:274-289 - the job order's items replace the "Items" list when there are any */
            if (JO_ITEMS.length) fill('ldItemName', JO_ITEMS.map(function (x) { return { Id: x.ItemId, name: x.ItemName }; }), 'Id', 'name', false);
            else fill('ldItemName', L.Items, 'Id', 'name', false);
            return issuanceSearch();
        }).then(function () { $id('ldFromDate').focus(); }).catch(function (e) { msg(e.message); });
    }
    /* PendingInventoryTransactionsForIssuanceLoad:319 */
    function issuanceSearch() {
        return getJson('/loader/search', {
            fromDate: getDT('ldFromDate'), toDate: getDT('ldToDate'), parentCategory: int(val('ldParentCategory')),
            itemCategory: int(val('ldItemCategory')), itemType: int(val('ldItemType')), jobLotId: int(val('ldJobLot')),
            cropYear: selText('ldCropYear'), warehouseId: int(val('ldWarehouse')), refDocumentTypeId: int(val('ldRefDocument')),
            supplierId: int(val('ldSupplier')), itemId: int(val('ldItemName')), itemIds: JO_ITEM_IDS
        }).then(function (rows) {
            ISSUE_ROWS = (rows || []).map(function (x) {
                var o = {};
                ISSUE_COLS.forEach(function (k) { o[k] = col(x, k === 'Remarks' ? 'TranRemarks' : k); });
                var t = int(o.RefDocumentTypeId);
                if (t === 112 || t === 80) o.GrnNo = 0;
                return o;
            });
            renderIssuance();
        }).catch(function (e) { msg(e.message); });
    }
    function renderIssuance() {
        var hidden = ISSUE_HIDDEN.slice();
        if (!ISSUE_SHELL_RATE) hidden.push('ItemAmount', 'AVgRate');
        var cols = [{ key: '__sel', check: true, headerSelector: true }];
        ISSUE_COLS.forEach(function (k) {
            var c = { key: k, hidden: hidden.indexOf(k) >= 0 };
            if (ISSUE_DECIMAL[k]) {
                c.num = true;
                if (k.indexOf('Amount') >= 0) { c.fmt = function (v) { return fAmt(num(v)); }; c.total = true; }
                else if (k.indexOf('Rate') >= 0) { c.fmt = function (v) { return fRate(num(v)); }; }
                else { c.fmt = function (v) { return f2(num(v)); }; c.total = true; }
            }
            if (k === 'DocDate') c.fmt = dMMMyy;
            cols.push(c);
        });
        if (!ISSUE_ROWS.length) { var t = $id('tblIssuance'); t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; return; }
        renderGrid($id('tblIssuance'), cols, ISSUE_ROWS, {});
    }
    /* SelectedWeightCalculation:636 - Math.Round(...).ToString("0,0") */
    function selectedWeightCalculation() {
        var w = 0, q = 0;
        ISSUE_ROWS.forEach(function (r) { if (r.__checked) { w += num(r.WeightBalance); q += num(r.QtyBalance); } });
        setVal('ldSelectedStock', K.pad2(roundEven(w, 0)));
        setVal('ldSelectedQty', K.pad2(roundEven(q, 0)));
    }
    /* btnLoadOnInvoice_Click_1:566 */
    function issuanceLoad() {
        if (ISSUE_BROKEN) { msg('Input array is longer than the number of columns in this table.'); return; }
        var checked = ISSUE_ROWS.filter(function (r) { return r.__checked; });
        if (!checked.length) { msg('Please Select Row first'); return; }
        checked.forEach(function (r) {
            ISSUE_RESULT.push({
                RefDocumentTypeId: String(r.RefDocumentTypeId), RefDocumentType: String(r.RefDocumentType == null ? '' : r.RefDocumentType),
                RefDocIdNo: String(r.RefDocIdNo), RefDocSubIdNo: String(r.RefDocSubIdNo), SupplierCustomerId: int(r.SupplierCustomerId),
                PartyName: String(r.SupplierCustomerName == null ? '' : r.SupplierCustomerName), DocDate: r.DocDate, DocNo: int(r.DocCodeNo),
                WareHouseId: int(r.WarehouseId), WareHouse: String(r.WareHouseCode == null ? '' : r.WareHouseCode),
                ItemId: int(r.ItemId), ItemName: String(r.ItemName == null ? '' : r.ItemName), ItemCode: r.ItemCode,
                ItemUOMId: int(r.ItemUom), PackUom: r.PackUom, ItemUOM: num(r.PackSize), CropYearId: int(r.CropYearId),
                CropYear: String(r.CropBatch == null ? '' : r.CropBatch), JobLotId: int(r.JobLotId), JobLotCode: String(r.JobLotCode == null ? '' : r.JobLotCode),
                PackingTypeId: int(r.InvPackingTypeId), PackTypeCode: String(r.PackingType == null ? '' : r.PackingType),
                BalQty: num(r.QtyBalance), BalWeight: num(r.WeightBalance), ItemQty: num(r.QtyBalance), Weight: num(r.WeightBalance),
                ItemRate: num(r.AVgRate), RateUOMId: int(r.RateUomId), RateUOM: r.RateUom, Equivalent: netG(num(r.Equivalent)),
                ItemAmount: num(r.ItemAmount), Remarks: ''
            });
        });
        closeModal('dlgIssuance');
    }
    function issuanceReset() {
        $id('ldSupplier').value = ''; $id('ldItemName').value = '';
        ISSUE_ROWS = []; renderIssuance();
        focusCombo('ldParentCategory');
    }

    /* ================================================== LoadOutPutPendingforRates ("Issue") */
    var PEND_ROWS = [], PEND_RAW = [], PEND_DOC = 0;
    function btnPendingForRate() {
        PEND_DOC = 0;
        $id('dlgPending')._onClose = function () {
            var p = PEND_DOC > 0 ? readByIdInput(PEND_DOC) : Promise.resolve();
            p.then(updateUomForPendingRateEntries);
        };
        openModal('dlgPending');
        $id('tblPending').tHead.innerHTML = ''; $id('tblPending').tBodies[0].innerHTML = ''; $id('tblPending').tFoot.innerHTML = '';
        $id('tblPendingDetail').tHead.innerHTML = ''; $id('tblPendingDetail').tBodies[0].innerHTML = ''; $id('tblPendingDetail').tFoot.innerHTML = '';
        if (S && S.financialYearStart) setDT('prFromDate', String(S.financialYearStart).replace(' ', 'T'));
        setDT('prToDate', new Date());
        $id('prFromDate').focus();
        /* LoadInvoices_Load: JobOrderFill then OutputGridHistory */
        return getJson('/pending-rates/job-orders').then(function (rows) {
            if (rows && rows.length) fill('prJobOrder', rows, 'Id', 'PlanCode', false);
            return pendingSearch();
        }).catch(function (e) { msg(e.message); });
    }
    /* UpdateUomForLoadingPendingForRateEntriesInPut:2716 - Cells["RateUom"] is the RateUOM TEXT; a
       non-numeric code converts to 0 and is overwritten with 40. Runs even when nothing was loaded. */
    function updateUomForPendingRateEntries() {
        TABLE.forEach(function (r) { if (int(r.RateUOM) === 0) r.RateUOM = 40; });
        renderDetail();
    }
    /* OutputGridHistory:190 - grouped by Id: TotalQty = Sum(Qty), TotalWeight = AVERAGE(Weight). */
    function pendingSearch() {
        return getJson('/pending-rates', {
            fromDate: getDT('prFromDate'), toDate: getDT('prToDate'), jobOrderId: int(val('prJobOrder')), entryType: 'Issue'
        }).then(function (rows) {
            if (!rows || !rows.length) return;
            var groups = {}, order = [];
            rows.forEach(function (x) {
                var id = String(col(x, 'Id'));
                if (!groups[id]) { groups[id] = []; order.push(id); }
                groups[id].push(x);
            });
            PEND_ROWS = order.map(function (id) {
                var g = groups[id], f = g[0], q = 0, w = 0;
                g.forEach(function (x) { q += num(col(x, 'Qty')); w += num(col(x, 'Weight')); });
                return {
                    Id: col(f, 'Id'), JobOrderNo: col(f, 'JobOrderNo'), DocDate: col(f, 'DocDate'), DocCode: int(col(f, 'DocCode')),
                    MainRemarks: col(f, 'MainRemarks'), TotalQty: q, TotalWeight: w / g.length, EntryDate: col(f, 'EntryDate'),
                    EntryUser: col(f, 'EntryUser'), ModifyDate: col(f, 'ModifyDate'), ModifyUser: col(f, 'ModifyUser')
                };
            });
            renderGrid($id('tblPending'), [
                { key: 'Load', caption: 'Load', btn: 'Load' }, { key: 'Id', hidden: true }, { key: 'JobOrderNo' },
                { key: 'DocDate', fmt: dMMMyy }, { key: 'DocCode' }, { key: 'MainRemarks' },
                { key: 'TotalQty', num: true, fmt: function (v) { return netG(num(v)); } },
                { key: 'TotalWeight', num: true, fmt: function (v) { return netG(num(v)); } },
                { key: 'EntryDate', fmt: dMMMyy }, { key: 'EntryUser' }, { key: 'ModifyDate', fmt: dMMMyy }, { key: 'ModifyUser' }
            ], PEND_ROWS, {});
        }).catch(function (e) { msg(e.message); });
    }
    /* grd_SelectionChanged - the detail read passes NO EntryType. */
    function pendingDetail(i) {
        var r = PEND_ROWS[i]; if (!r) return;
        getJson('/pending-rates', { fromDate: getDT('prFromDate'), toDate: getDT('prToDate'), jobOrderId: int(val('prJobOrder')) })
            .then(function (rows) {
                if (!rows || !rows.length) return;
                var out = rows.filter(function (x) { return int(col(x, 'Id')) === int(r.Id); }).map(function (x) {
                    return { DetailId: col(x, 'DetailId'), EntryType: col(x, 'EntryType'), ItemName: col(x, 'ItemName'), Cropyear: col(x, 'CropBatch'),
                        WareHouseName: col(x, 'WareHouseName'), JobLotDescription: col(x, 'JobLotDescription'), PackType: col(x, 'PackTypeDesc'),
                        PackUom: col(x, 'PackUom'), Qty: num(col(x, 'Qty')), Weight: num(col(x, 'Weight')) };
                });
                renderGrid($id('tblPendingDetail'), [{ key: 'DetailId', hidden: true }, { key: 'EntryType' }, { key: 'ItemName' },
                    { key: 'Cropyear' }, { key: 'WareHouseName' }, { key: 'JobLotDescription' }, { key: 'PackType' }, { key: 'PackUom' },
                    { key: 'Qty', num: true, fmt: function (v) { return netG(num(v)); } },
                    { key: 'Weight', num: true, fmt: function (v) { return netG(num(v)); } }], out, {});
            }).catch(function (e) { msg(e.message); });
    }
    /* btnReset_Click: OutputGridHistory first, THEN the fields and both grids are cleared. */
    function pendingReset() {
        return pendingSearch().then(function () {
            if (S && S.financialYearStart) setDT('prFromDate', String(S.financialYearStart).replace(' ', 'T'));
            $id('prFromDate').focus();
            $id('prJobOrder').value = '';
            ['tblPending', 'tblPendingDetail'].forEach(function (t) { var x = $id(t); x.tHead.innerHTML = ''; x.tBodies[0].innerHTML = ''; x.tFoot.innerHTML = ''; });
            PEND_ROWS = [];
        });
    }

    /* ======================================================= frmPendingMoveOrderDocuments */
    var MO_ROWS = [], MO_RESULT = null;
    var MO_COLS = ['Id', 'MoveOrderDocumentTypeId', 'DocumentTypeId', 'WbDocumentType', 'DocDate', 'TicketNo', 'WorkingReportNo', 'VehicleNo',
        'ItemQty', 'FirstWeight', 'SecondWeight', 'NetWbWeight', 'UsedWeight', 'BalWeight', 'ItemDescription', 'WbRemarks', 'Status',
        'EntryDate', 'EntryUserName', 'ModifyDate', 'ModifyUserName'];
    var MO_NUM = { ItemQty: 1, FirstWeight: 1, SecondWeight: 1, NetWbWeight: 1, UsedWeight: 1, BalWeight: 1 };
    function btnLoadMoveOrder() {
        MO_ROWS = []; MO_RESULT = null;
        renderMoveOrders();
        $id('dlgMoveOrder')._onClose = function () { loadInGridDetail(MO_RESULT); };
        setDT('moFromDate', addDays(-7));
        setDT('moToDate', new Date());
        openModal('dlgMoveOrder');
        $id('moFromDate').focus();
        return moveOrderShow();
    }
    function moveOrderShow() {
        return getJson('/move-orders', {
            fromDate: getDT('moFromDate'), toDate: getDT('moToDate'), fromDocNo: int(val('moDocFrom')), toDocNo: int(val('moDocTo')),
            vehicleNo: val('moVehicle'), biltyNo: val('moBilty')
        }).then(function (rows) {
            MO_ROWS = (rows || []).map(function (x) { var o = {}; MO_COLS.forEach(function (k) { o[k] = col(x, k); }); return o; });
            renderMoveOrders();
        }).catch(function (e) { msg(e.message); });
    }
    function renderMoveOrders() {
        var t = $id('tblMoveOrder');
        if (!MO_ROWS.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; return; }
        var cols = [{ key: '__sel', check: true }];
        MO_COLS.forEach(function (k) {
            var c = { key: k, hidden: k === 'Id' || k === 'MoveOrderDocumentTypeId' || k === 'DocumentTypeId' };
            if (MO_NUM[k]) { c.num = true; c.fmt = function (v) { return f3(num(v)); }; c.total = true; }
            if (k === 'DocDate' || k === 'EntryDate' || k === 'ModifyDate') c.fmt = dMMMyy;
            cols.push(c);
        });
        renderGrid(t, cols, MO_ROWS, {});
    }
    /* btnLoadOnInvoice_Click_1 (move order) */
    function moveOrderLoad() {
        var checked = MO_ROWS.filter(function (r) { return r.__checked; });
        if (!checked.length) { msg('Check the row first'); return; }
        var firstId = int(checked[0].Id), ids = [];
        for (var i = 0; i < checked.length; i++) {
            if (int(checked[i].Id) !== firstId) { msg('Sorry! You can Only Check rows which have the same DocNo'); return; }
            if (String(checked[i].Status == null ? '' : checked[i].Status) !== 'Accepted') { msg('Sorry! You can Only Load Accepted Record'); return; }
            ids.push(int(checked[i].Id));
        }
        MO_RESULT = MO_ROWS.filter(function (r) { return ids.indexOf(int(r.Id)) >= 0; });
        closeModal('dlgMoveOrder');
    }
    function moveOrderReset() {
        $id('moFromDate').focus();
        if (S && S.financialYearStart) setDT('moFromDate', String(S.financialYearStart).replace(' ', 'T'));
        setVal('moDocFrom', ''); setVal('moDocTo', ''); setVal('moVehicle', ''); setVal('moBilty', '');
        return moveOrderShow();
    }
    /* LoadInGridDetail:2961 */
    function loadInGridDetail(rows) {
        if (!rows || !rows.length) return;
        var incoming = int(rows[0].Id), current = int(val('CmbMoveOrderTicket'));
        if (current <= 0 || current === incoming
            || window.confirm('There is already a ticket selected. Are you sure you want to clear it and proceed with a new one?')) {
            var ticket = int(rows[0].TicketNo);
            setVal('txtMoveOrderNetWeight', fHash(rows[0].NetWbWeight));
            setVal('txtMoveOrderAlreadUsedWeight', fHash(rows[0].UsedWeight));
            setVal('txtMoveOrderBalWeight', fHash(rows[0].BalWeight));
            bindMoveOrderTicket(incoming, ticket);
        }
    }
    /* BtnResetMoveOrder_Click:3024 */
    function btnResetMoveOrder() {
        clearCombo('CmbMoveOrderTicket');
        setVal('txtMoveOrderNetWeight', ''); setVal('txtMoveOrderAlreadUsedWeight', ''); setVal('txtMoveOrderBalWeight', '');
    }

    /* ============================================================================ wiring */
    function wire() {
        $id('btnnew').addEventListener('click', function () { reset(); });
        $id('btnRefresh').addEventListener('click', function () { busy('btnRefresh', btnRefresh); });
        $id('btnsave').addEventListener('click', btnSave);
        $id('btnUpdate').addEventListener('click', btnUpdate);
        $id('Print').addEventListener('click', print601);
        $id('btnVoucherReport').addEventListener('click', voucher118);
        $id('btnJobOrderDefine').addEventListener('click', function () { window.open('/production/job-order', '_blank'); });
        $id('BtnGenerateRates').addEventListener('click', function () { busy('BtnGenerateRates', generateRates); });
        $id('btnPendingForRateInput').addEventListener('click', function () { busy('btnPendingForRateInput', btnPendingForRate); });
        $id('btnLoadInvoices').addEventListener('click', function () { busy('btnLoadInvoices', btnLoadInvoices); });
        $id('BtnLoadMoveOrder').addEventListener('click', function () { busy('BtnLoadMoveOrder', btnLoadMoveOrder); });
        $id('BtnResetMoveOrder').addEventListener('click', btnResetMoveOrder);
        $id('btnAddInput').addEventListener('click', btnAddInput);
        $id('btnUpdateDetailInput').addEventListener('click', btnUpdateDetailInput);
        $id('btnCancelInput').addEventListener('click', btnCancelInput);

        onLeave('CmbJobOrderNo', jobOrderLeave);
        $id('cmbWareHouseInput').addEventListener('change', warehouseValueChanged);
        onLeave('cmbWareHouseInput', stockAndRateLeave);
        onLeave('cmbCropYearInput', stockAndRateLeave);
        onLeave('cmbLotInput', stockAndRateLeave);
        onLeave('cmbItemInput', itemLeave);
        $id('cmbItemUomInput').addEventListener('change', itemUomChanged);
        onLeave('cmbPackingTypeInput', getAvgRate);
        $id('cmbRateuomInput').addEventListener('change', amountInputCalu);

        var qtyTimer = null, nwTimer = null;
        $id('txtQtyInput').addEventListener('input', function () {
            calculateWeight();
            clearTimeout(qtyTimer); qtyTimer = setTimeout(getAvgRate, 250);
        });
        $id('txtQtyInput').addEventListener('blur', function () { if (FIFO) getAvgRate(); });
        $id('txtNetWeightInput').addEventListener('input', function () {
            amountInputCalu();
            clearTimeout(nwTimer); nwTimer = setTimeout(getAvgRate, 250);
        });
        $id('txtNetWeightInput').addEventListener('blur', function () { if (FIFO) getAvgRate(); });
        $id('txtRateInput').addEventListener('input', amountInputCalu);
        $id('txtEbUnit').addEventListener('input', calculateWeight);
        $id('txtEbTotal').addEventListener('input', calculateWeight);

        /* the detail grid */
        var tbl = $id('tblDetail');
        tbl.addEventListener('click', function (e) {
            var b = e.target.closest('button[data-btn]');
            if (b && b.getAttribute('data-btn') === 'Delete') { deleteRow(int(b.getAttribute('data-i'))); return; }
            var i = rowIndexOf(e);
            if (i >= 0 && CURRENT_ROW !== i) {
                CURRENT_ROW = i;
                Array.prototype.forEach.call(tbl.tBodies[0].rows, function (tr) { tr.classList.toggle('is-current', int(tr.getAttribute('data-i')) === i); });
            }
        });
        tbl.addEventListener('dblclick', function (e) {
            if (e.target.closest('input, button')) return;
            var i = rowIndexOf(e); if (i >= 0) rowDoubleClick(i);
        });
        tbl.addEventListener('change', function (e) {
            var inp = e.target.closest('input[data-edit]');
            if (!inp) return;
            cellUpdated(int(inp.getAttribute('data-i')), inp.getAttribute('data-edit'), inp.value);
        });

        /* History */
        $id('tabForm').addEventListener('click', function () { showView('form'); });
        $id('tabHistory').addEventListener('click', function () { showView('history'); });
        $id('toolStripButton4').addEventListener('click', resetInputHistory);
        $id('BtnRefereshInputHistory').addEventListener('click', function () {
            busy('BtnRefereshInputHistory', function () { return getJson('/history-job-orders').then(bindHistoryJobOrders); });
        });
        $id('BtnShowInputHistory').addEventListener('click', function () { busy('BtnShowInputHistory', inputGridHistory); });
        var ht = $id('tblHistory');
        ht.addEventListener('click', function (e) {
            var b = e.target.closest('button[data-btn]');
            var l = e.target.closest('[data-link]');
            var i = rowIndexOf(e);
            if (i >= 0 && i !== HIST_CURRENT) {
                HIST_CURRENT = i;
                Array.prototype.forEach.call(ht.tBodies[0].rows, function (tr) { tr.classList.toggle('is-current', int(tr.getAttribute('data-i')) === i); });
                historyDetail(i);
            }
            if (b && b.getAttribute('data-btn') === 'Edit') historyEdit(i);
            if (b && b.getAttribute('data-btn') === 'WagesPrint') historyWagesPrint(i);
            if (l) historyEdit(i);
        });

        /* modals */
        document.addEventListener('click', function (e) {
            var c = e.target.closest('[data-close]');
            if (c) closeModal(c.getAttribute('data-close'));
        });
        $id('ldSearch').addEventListener('click', function () { busy('ldSearch', issuanceSearch); });
        $id('ldLoad').addEventListener('click', issuanceLoad);
        $id('ldReset').addEventListener('click', issuanceReset);
        var it = $id('tblIssuance');
        it.addEventListener('change', function (e) {
            var cb = e.target.closest('input[data-check]');
            if (cb) { ISSUE_ROWS[int(cb.getAttribute('data-check'))].__checked = cb.checked; selectedWeightCalculation(); return; }
            var hc = e.target.closest('input[data-head-check]');
            if (hc) {
                var visible = Array.prototype.filter.call(it.tBodies[0].rows, function (tr) { return tr.style.display !== 'none'; });
                visible.forEach(function (tr) { var i = int(tr.getAttribute('data-i')); ISSUE_ROWS[i].__checked = hc.checked; });
                Array.prototype.forEach.call(it.querySelectorAll('input[data-check]'), function (x) { x.checked = !!ISSUE_ROWS[int(x.getAttribute('data-check'))].__checked; });
                selectedWeightCalculation();
            }
        });
        $id('prSearch').addEventListener('click', function () { busy('prSearch', pendingSearch); });
        $id('prReset').addEventListener('click', function () { busy('prReset', pendingReset); });
        var pt = $id('tblPending');
        pt.addEventListener('click', function (e) {
            var i = rowIndexOf(e);
            var b = e.target.closest('button[data-btn]');
            if (b && b.getAttribute('data-btn') === 'Load') { PEND_DOC = int(PEND_ROWS[i] && PEND_ROWS[i].Id); closeModal('dlgPending'); return; }
            if (i >= 0) {
                Array.prototype.forEach.call(pt.tBodies[0].rows, function (tr) { tr.classList.toggle('is-current', int(tr.getAttribute('data-i')) === i); });
                pendingDetail(i);
            }
        });
        $id('moShow').addEventListener('click', function () { busy('moShow', moveOrderShow); });
        $id('moNew').addEventListener('click', function () { busy('moNew', moveOrderReset); });
        $id('moRefresh').addEventListener('click', function () { /* btnRefresh_Click is empty on the desktop */ });
        $id('moLoad').addEventListener('click', moveOrderLoad);
        $id('moShortcuts').addEventListener('click', function () {
            K.shortcuts([['Ctrl+S', 'Show'], ['Ctrl+L', 'Load'], ['Ctrl+N', 'New'], ['Ctrl+R', 'Refresh'], ['Ctrl+F5', 'Date From'],
                ['Ctrl+Down', 'Grid'], ['Ctrl+E / Esc', 'Close']]);
        });
        var mt = $id('tblMoveOrder');
        mt.addEventListener('change', function (e) {
            var cb = e.target.closest('input[data-check]');
            if (cb) MO_ROWS[int(cb.getAttribute('data-check'))].__checked = cb.checked;
        });

        /* frmFoodProduction_KeyDown:837 (form tab only) and the dialogs' own keys */
        document.addEventListener('keydown', function (e) {
            var m = topModal();
            var key = e.key, ctrl = e.ctrlKey || e.metaKey;
            if (m) {
                if (key === 'Escape' || (ctrl && (key === 'e' || key === 'E'))) { e.preventDefault(); closeModal(m); return; }
                if (m === 'dlgIssuance') {
                    if (ctrl && (key === 'l' || key === 'L')) { e.preventDefault(); issuanceLoad(); }
                    else if (ctrl && (key === 's' || key === 'S')) { e.preventDefault(); busy('ldSearch', issuanceSearch); }
                    else if (ctrl && key === 'ArrowUp') { e.preventDefault(); $id('ldFromDate').focus(); }
                    else if (ctrl && key === 'F5') { e.preventDefault(); focusCombo('ldParentCategory'); }
                } else if (m === 'dlgMoveOrder') {
                    if (ctrl && (key === 's' || key === 'S')) { e.preventDefault(); busy('moShow', moveOrderShow); }
                    else if (ctrl && (key === 'l' || key === 'L')) { e.preventDefault(); moveOrderLoad(); }
                    else if (ctrl && (key === 'n' || key === 'N')) { e.preventDefault(); busy('moNew', moveOrderReset); }
                    else if (ctrl && key === 'F5') { e.preventDefault(); $id('moFromDate').focus(); }
                }
                return;
            }
            if (key === 'Escape' || (ctrl && (key === 'e' || key === 'E'))) {
                /* Close(): standalone, back to the hub; framed, the shell owns the window. */
                if (window.top === window) { e.preventDefault(); K.close(); }
                return;
            }
            if ($id('viewForm').classList.contains('is-hidden')) return;
            if (ctrl && (key === 's' || key === 'S')) { e.preventDefault(); if (!$id('btnsave').classList.contains('is-hidden') && !$id('btnsave').disabled) btnSave(); }
            else if (ctrl && (key === 'u' || key === 'U')) { e.preventDefault(); if (!$id('btnUpdate').classList.contains('is-hidden') && !$id('btnUpdate').disabled) btnUpdate(); }
            else if (ctrl && (key === 'l' || key === 'L')) { e.preventDefault(); busy('btnLoadInvoices', btnLoadInvoices); }
            else if (ctrl && key === 'F5') { e.preventDefault(); $id('txtDocdate').focus(); }
            else if (ctrl && key === 'ArrowDown') { e.preventDefault(); var f = $id('tblDetail').querySelector('input, button'); if (f) f.focus(); }
            else if (ctrl && (key === 'n' || key === 'N')) { e.preventDefault(); reset(); }
            else if (ctrl && (key === 'r' || key === 'R')) { e.preventDefault(); busy('btnRefresh', btnRefresh); }
        });
        K.enterToTab();
    }

    /* Shell -> this page: the Transaction History tab's Edit (FoodProductionWithValues:1109-1110)
       disables the job order and calls ReadByIdInput. */
    window.P280ReadById = function (id) {
        showView('form');
        $id('CmbJobOrderNo').disabled = true;
        return readByIdInput(id);
    };

    document.addEventListener('DOMContentLoaded', function () {
        setDT('txtDocdate', new Date());
        wire();
        if (window.DesktopCombo && window.DesktopCombo.refresh) window.DesktopCombo.refresh();
        load();
    });
}());
