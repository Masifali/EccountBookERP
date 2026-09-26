/* ============================================================================================
 * Contractor Labour Wages - frmwagesBillHeader.cs (Architecture.WinApp.Contractor_Wages),
 * DocumentTypeId 101.
 *
 * The page keeps the form's DataTables in memory exactly as the form does - dtdetail (Regular
 * Wages), dtStiching (Other Wages), the pending list, the previous-entry list - and runs every
 * event the form wires: Load, the three grids' EditingCell / CellUpdated / ColumnButtonClick, the
 * pending grid's Load button, Apply All, New / Refresh / Save / Update / Print, the history tab
 * (Show / Reset / Refresh, SelectionChanged, double-click, Edit / Slip / Voucher), Cancel Records,
 * FormClosing and KeyDown. The server answers the BLL calls and runs Insert() from its data checks
 * onwards (reference-row checks, both grid loops, MakeVoucher, SetData).
 *
 * Entry path (the callers): Production Input (80), Output (112), Consumption (181) and Stock
 * Conversion (66) set obj.RefDocTypeId / RefDocId / GrossWeightTotal and ShowDialog() it. Here
 * those arrive as ?refDocTypeId=&refDocId=&grossWeightTotal=. Inside an iframe the page draws the
 * window caption with its X, and on closing (the X, or the automatic Close() after a save) calls
 * window.parent.P280WagesClosed({saved}).
 * Register links (CommonServices.EditMethodFromLinked, DocumentTypeId 101) open the page with
 * ?id=<header id>: Load, then ReadById(id) - see the end of this file (countx_doc_link.js).
 *
 * Line numbers are frmwagesBillHeader.cs's.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/production/wages-bill';
    var K = window.ReportKit;
    var B = document.body.dataset;

    /* ------------------------------------------------------------------ public fields :65-69 */
    var RefDocTypeId = toInt(B.refDocTypeId);
    var RefDocId = toInt(B.refDocId);
    var GrossWeightTotal = toDouble(B.grossWeightTotal);
    var EMBED = (function () { try { return window.parent && window.parent !== window; } catch (e) { return false; } }());
    var REF = RefDocTypeId > 0 && RefDocId > 0;

    /* ------------------------------------------------------------------ form fields :31-97 */
    var IsRegularOrFreeOfCost = false;
    var RECID = 0;
    var PreviousContractorId = 0;
    var PreviousContractorIdStiching = 0;
    var UpdateMode = false;
    var WagesAmountCalculateOnQty = false;
    var IsReferred = false;
    var StichingWagesCompulsory = false;
    var EnableAddLessOnWagesRegular = false;
    var PercentageForRateAddLess = 0;
    var MultiBranchFeatur = false;
    var CmpGRN = false, CmpGDN = false, CmpFwd = false, CmpST = false, CmpIn = false, CmpCons = false;
    var grdwagesDetailhaveDeletedRecords = false;
    var CFG = {};
    var RIGHTS = { save: false, update: false, print: false };
    var IS_ADMIN = false;
    var dtcus = [];                       // contractors (Id, CompanyName)
    var dtwagesacc = [];                  // Regular grid wages accounts (Id, WagesAccountName)
    var dtStichingItems = [];             // Other grid wages accounts
    var dtWagesTypeIdsAgainstDocumentType = [];
    var dtdetail = [];                    // Regular Wages DataTable (Load:354-386)
    var dtStiching = [];                  // Other Wages DataTable (Clone of dtdetail)
    var regBound = false, othBound = false, retBound = false;
    var regRefLocked = false, othRefLocked = false;   // grdDetailLockEditing / GridStichingLockEditing with IsReferred
    var regCur = -1, othCur = -1, pendCur = -1, histCur = -1, retCur = -1, hdCur = -1;
    var pendingRows = [], pendingChecked = {};
    var retrieveRows = [];                // grdDataRetrieve
    var historyRows = [], historyBound = false, historyDetailRows = [], historyDetailBound = false;
    var cmbRef = { list: [], value: null, enabled: true };  // cmbReferenceDocType
    var TOOL = { btnsave: { visible: true }, btnUpdate: { visible: false } };
    var activeGridTag = '';               // ActiveControl.Tag for Ctrl+D
    var SAVED = false, CLOSED = false;
    var pending = Promise.resolve();

    var NRE = 'Object reference not set to an instance of an object.';
    var OOR = 'Index was out of range. Must be non-negative and less than the size of the collection.\nParameter name: index';

    /* the DataTable columns of Load:354-386, in order */
    var COLS = ['SupplierId', 'ContractorName', 'WagesId', 'WagesAccount', 'WagesType', 'Date', 'packingTypeId', 'packingType',
        'Weight', 'PackSize', 'Quantity', 'WeightCut', 'BillQty', 'BillWeight', 'RateWithoutAddLess', 'RateAddLess', 'Rate',
        'Amount', 'ItemId', 'Item', 'jobLotId', 'jobLot', 'Crop', 'MoveFromId', 'MoveFrom', 'MoveToId', 'MoveTo', 'PurchaseGLAC',
        'WarehouseType', 'RefDocQty', 'RefDocWeight', 'RefLineId', 'WagesScheduleId'];
    /** dtdetail.Rows.Add(values...) - missing trailing values stay null. */
    function newRow(values) {
        var r = {};
        for (var i = 0; i < COLS.length; i++) r[COLS[i]] = i < values.length ? values[i] : null;
        return r;
    }
    function copyRow(r) { var o = {}; for (var k in r) if (Object.prototype.hasOwnProperty.call(r, k)) o[k] = r[k]; return o; }

    /* ---------------------------------------------------------------------------- helpers */
    function $(id) { return document.getElementById(id); }
    function val(id) { var e = $(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function say(m) { var e = $('lblStatus'); if (e) e.textContent = m || ''; }
    function msg(m) { window.alert(m); }
    function ask(m) { return window.confirm(m); }
    function esc(s) { return K.esc(s); }
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var ln = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === ln) return row[k];
        return null;
    }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    /** Conversion.ToDouble - unparsable is 0. */
    function toDouble(v) {
        if (typeof v === 'number') return isFinite(v) ? v : 0;
        if (typeof v === 'boolean') return v ? 1 : 0;
        var n = parseFloat(str(v).replace(/,/g, '').trim());
        return isNaN(n) ? 0 : n;
    }
    /** Conversion.ToInt - a double rounds half to even (Convert.ToInt32); unparsable is 0. */
    function toInt(v) { return Math.trunc(roundN(toDouble(v), 0)); }
    function toBool(v) { if (typeof v === 'boolean') return v; var s = str(v).trim().toLowerCase(); return s === '1' || s === 'true'; }
    /** Math.Round(x, d) - MidpointRounding.ToEven. */
    function roundN(x, d) {
        if (!isFinite(x)) return x;
        var p = Math.pow(10, d), v = Math.abs(x) * p, f = Math.floor(v), diff = v - f, r;
        if (Math.abs(diff - 0.5) < 1e-9) r = (f % 2 === 0) ? f : f + 1; else r = Math.round(v);
        r = r / p;
        return x < 0 ? -r : r;
    }
    function round2(x) { return roundN(x, 2); }
    /** double.ToString() (.NET Framework, 15 significant digits). */
    function netToString(n) {
        if (typeof n !== 'number') n = toDouble(n);
        if (isNaN(n)) return 'NaN';
        if (n === Infinity) return 'Infinity';
        if (n === -Infinity) return '-Infinity';
        if (n === 0) return '0';
        if (Math.abs(n) < 1e15 && Number.isInteger(n)) return String(n);
        return String(Number(n.toPrecision(15))).replace('e+', 'E+').replace('e-', 'E-');
    }
    /** Conversion.ToString of a cell value. */
    function cellText(v) { return typeof v === 'number' ? netToString(v) : str(v); }

    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    /** "yyyy-MM-dd..." or "dd-MMM-yyyy" -> Date parts. */
    function dparts(v) {
        var s = str(v), m = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2}))?/.exec(s);
        if (m) return { y: +m[1], M: +m[2], d: +m[3], h: +(m[4] || 0), mi: +(m[5] || 0) };
        m = /^(\d{1,2})-([A-Za-z]{3})-(\d{4})/.exec(s);
        if (m) {
            var mi = MON.map(function (x) { return x.toLowerCase(); }).indexOf(m[2].toLowerCase());
            if (mi >= 0) return { y: +m[3], M: mi + 1, d: +m[1], h: 0, mi: 0 };
        }
        return null;
    }
    function p2(n) { return (n < 10 ? '0' : '') + n; }
    /** ToString("dd-MMM-yyyy") as LoadDataForWages / ReadById store the row date. */
    function dMMMyyyy(v) { var p = dparts(v); return p ? p2(p.d) + '-' + MON[p.M - 1] + '-' + p.y : str(v); }
    function isoDate(v) { var p = dparts(v); return p ? p.y + '-' + p2(p.M) + '-' + p2(p.d) : ''; }
    /** "dd-MMM-yyyy hh:mm tt" */
    function dMMMyyyyhm(v) {
        var p = dparts(v); if (!p) return str(v);
        var h12 = p.h % 12 === 0 ? 12 : p.h % 12;
        return p2(p.d) + '-' + MON[p.M - 1] + '-' + p.y + ' ' + p2(h12) + ':' + p2(p.mi) + ' ' + (p.h < 12 ? 'AM' : 'PM');
    }
    function today() { var d = new Date(); return d.getFullYear() + '-' + p2(d.getMonth() + 1) + '-' + p2(d.getDate()); }
    /** "#,#" - an integer with separators, and nothing at all for 0. */
    function fmtHash(v) { var n = toDouble(v); var r = Math.round(n); return r === 0 ? '' : K.num(r, 0); }
    function f4(v) { return (v === null || v === undefined || v === '') ? '' : K.num(v, 4); }
    function f2(v) { return (v === null || v === undefined || v === '') ? '' : K.num(v, 2); }
    function f3(v) { return (v === null || v === undefined || v === '') ? '' : K.num(v, 3); }
    function f00(v) { return (v === null || v === undefined || v === '') ? '' : K.pad2(v); }   // "0,0"

    function headers(h) {
        var t = document.querySelector('meta[name="_csrf"]'), hh = document.querySelector('meta[name="_csrf_header"]');
        if (t && hh) h[hh.getAttribute('content')] = t.getAttribute('content');
        return h;
    }
    function readJson(r) {
        return r.text().then(function (t) {
            var body = null;
            try { body = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
            if (!r.ok) throw new Error((body && body.message) || ('Request failed (' + r.status + ')'));
            return body;
        });
    }
    function getJson(url) { return fetch(url, { headers: { 'Accept': 'application/json' }, credentials: 'same-origin' }).then(readJson); }
    function postJson(url, body) {
        return fetch(url, {
            method: 'POST', credentials: 'same-origin',
            headers: headers({ 'Content-Type': 'application/json', 'Accept': 'application/json' }),
            body: JSON.stringify(body)
        }).then(readJson);
    }
    function qs(o) {
        return Object.keys(o).filter(function (k) { return o[k] !== null && o[k] !== undefined; })
            .map(function (k) { return encodeURIComponent(k) + '=' + encodeURIComponent(o[k]); }).join('&');
    }
    /** The button contract: disabled at once, spinner, duplicates ignored, re-enabled on success AND failure. */
    function busy(btn, fn) {
        var b = typeof btn === 'string' ? $(btn) : btn;
        if (b) {
            if (b.classList.contains('is-busy')) return Promise.resolve();
            b.dataset.wbWas = b.disabled ? '1' : '';
            b.disabled = true;
            b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.classList.remove('is-busy'); b.disabled = b.dataset.wbWas === '1'; } };
        var p;
        try { p = Promise.resolve(fn()); } catch (e) { done(); msg(e.message); return Promise.resolve(); }
        return p.then(done, function (e) { done(); if (e && e.message) msg(e.message); });
    }
    /** Grid events run one after another, as the desktop's synchronous handlers do. */
    function after(fn) {
        var p = pending.then(fn, fn);
        pending = p.catch(function () { });
        return p;
    }
    /** try { ... } catch (Exception ex) { MessageBox.Show(ex.Message); } */
    function guarded(fn) {
        return function () {
            var a = arguments;
            return Promise.resolve().then(function () { return fn.apply(null, a); })
                .catch(function (e) { msg(e && e.message ? e.message : String(e)); });
        };
    }

    /* ------------------------------------------------------------------- BLL call wrappers */
    function cmbValue() { return toInt(cmbRef.value); }
    function CheckItemsFreeofcostforWages(date, refDocTypeId, itemId, wagesAccountId) {
        return postJson(API + '/free-of-cost', { rows: [{ date: str(date), refDocumentTypeId: refDocTypeId, itemId: itemId, wagesAccountId: wagesAccountId }] })
            .then(function (r) { return !!(r && r[0]); });
    }
    function GetWagesRate(date, packSize, wagesAccountId, contractorId) {
        return postJson(API + '/wages-rate', { rows: [{ date: str(date), packSize: packSize, wagesAccountId: wagesAccountId, contractorId: contractorId }] })
            .then(function (r) { return r && r[0] ? r[0] : { WagesRate: 0, ScheduleId: 0 }; });
    }
    function GetWagesAccount(ids) {
        return getJson(API + '/wages-accounts' + (ids !== null && ids !== undefined ? '?' + qs({ ids: ids }) : ''))
            .then(function (r) { return r || []; });
    }
    function GetByID(id) { return getJson(API + '/bill/' + id); }
    /** dtWagesTypeIdsAgainstDocumentType.Select("<col> = 'v'") - first row, or null. */
    function wagesTypeRow(colName, v) {
        for (var i = 0; i < dtWagesTypeIdsAgainstDocumentType.length; i++) {
            if (toInt(col(dtWagesTypeIdsAgainstDocumentType[i], colName)) === v) return dtWagesTypeIdsAgainstDocumentType[i];
        }
        return null;
    }
    /** the "comparison by activity" set: 112, 66 and each configured document type. */
    function cmpDoc(v) {
        return v === 112 || v === 66 || (CmpGRN && v === 46) || (CmpGDN && v === 86) || (CmpFwd && v === 205)
            || (CmpST && (v === 68 || v === 806)) || (CmpIn && v === 80) || (CmpCons && v === 181);
    }

    /* ================================================================= tabs and visibility */
    var TABVIS = { tabPage3: true, tabPage4: false, tabPage5: true, tabPage6: false };
    var TABSEL = { tabControl1: 'tabPage3', tabControl2: 'tabPage5' };
    function tabOwner(p) { return (p === 'tabPage3' || p === 'tabPage4') ? 'tabControl1' : 'tabControl2'; }
    function showTab(p, on) {
        TABVIS[p] = !!on;
        var owner = tabOwner(p);
        if (!on && TABSEL[owner] === p) TABSEL[owner] = null;
        if (on && !TABSEL[owner]) TABSEL[owner] = p;
        if (!TABSEL[owner]) {
            var pages = owner === 'tabControl1' ? ['tabPage3', 'tabPage4'] : ['tabPage5', 'tabPage6'];
            for (var i = 0; i < pages.length; i++) if (TABVIS[pages[i]]) { TABSEL[owner] = pages[i]; break; }
        }
        paintTabs();
    }
    function selectTab(p) { if (TABVIS[p]) { TABSEL[tabOwner(p)] = p; paintTabs(); } }
    function paintTabs() {
        ['tabPage3', 'tabPage4', 'tabPage5', 'tabPage6'].forEach(function (p) {
            var head = document.querySelector('.wb-tab[data-tab="' + p + '"]');
            var page = $(p);
            var sel = TABSEL[tabOwner(p)] === p;
            if (head) { head.classList.toggle('is-hidden', !TABVIS[p]); head.classList.toggle('is-active', sel && TABVIS[p]); }
            if (page) page.classList.toggle('is-active', sel && TABVIS[p]);
        });
    }
    Array.prototype.forEach.call(document.querySelectorAll('.wb-tab'), function (t) {
        t.addEventListener('click', function () { selectTab(t.getAttribute('data-tab')); });
    });

    /* tabIncomingLabourwages - Form (0) / History (1) */
    var mainIndex = 0;
    function setMainTab(i) {
        mainIndex = i;
        $('pageForm').classList.toggle('is-active', i === 0);
        $('pageHistory').classList.toggle('is-active', i === 1);
        $('tabForm').classList.toggle('is-active', i === 0);
        $('tabHistory').classList.toggle('is-active', i === 1);
    }
    /** tabControl1_SelectedIndexChanged:3228 (wired to tabIncomingLabourwages). */
    function mainTabChanged(i) {
        setMainTab(i);
        if (REF) { setMainTab(0); return; }
        if (mainIndex === 1) { try { $('FromDateHistory').focus(); } catch (e) { } }
    }
    $('tabForm').addEventListener('click', function () { mainTabChanged(0); });
    $('tabHistory').addEventListener('click', function () { mainTabChanged(1); });

    function paintTools() {
        $('btnsave').classList.toggle('is-hidden', !TOOL.btnsave.visible);
        $('btnUpdate').classList.toggle('is-hidden', !TOOL.btnUpdate.visible);
        if (!$('btnsave').classList.contains('is-busy')) $('btnsave').disabled = !RIGHTS.save;
        if (!$('btnUpdate').classList.contains('is-busy')) $('btnUpdate').disabled = !RIGHTS.update;
        if (!$('Print').classList.contains('is-busy')) $('Print').disabled = !RIGHTS.print;
    }
    function setSaveVisible(on) { TOOL.btnsave.visible = !!on; paintTools(); }
    function setUpdateVisible(on) { TOOL.btnUpdate.visible = !!on; paintTools(); }

    function paintCmbRef() {
        var s = $('cmbReferenceDocType');
        s.innerHTML = cmbRef.list.map(function (o) {
            return '<option value="' + esc(o.Id) + '">' + esc(o.name) + '</option>';
        }).join('');
        s.value = cmbRef.value === null || cmbRef.value === undefined ? '' : String(cmbRef.value);
        s.disabled = !cmbRef.enabled;
    }
    $('cmbReferenceDocType').addEventListener('change', function () { cmbRef.value = this.value; });

    /* ====================================================================== generic grid */
    /**
     * g: { table, nav, cols(), rows(), cur(), setCur(i), editable(col,row,i), commit(key,i,raw),
     *      startEdit(key,i), button(key,i), dbl(i), rowClass(row), totals, filter, frozen, check }
     */
    function renderGrid(g) {
        var t = $(g.table), cols = g.cols(), rows = g.rows();
        var ae = document.activeElement, keepKey = null;
        if (ae && t.tBodies[0] && t.tBodies[0].contains(ae)) {
            var atd = ae.closest('td[data-key]');
            if (atd && atd.querySelector('.cell-edit')) keepKey = atd.getAttribute('data-key');
        }
        var vis = cols.filter(function (c) { return c.visible !== false; });
        if (!g.bound()) {
            t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; if (t.tFoot) t.tFoot.innerHTML = '';
            if (g.nav) $(g.nav).textContent = '';
            return;
        }
        var left = 0;
        t.tHead.innerHTML = '<tr>' + vis.map(function (c, ci) {
            var st = 'min-width:' + (c.w || 80) + 'px;';
            var cls = '';
            if (g.frozen && ci < g.frozen) { cls = ' class="frozen"'; st += 'left:' + left + 'px;'; left += (c.w || 80) + 9; }
            return '<th' + cls + ' style="' + st + '">' + esc(c.cap === undefined ? c.key : c.cap) + '</th>';
        }).join('') + '</tr>';
        var html = [];
        for (var i = 0; i < rows.length; i++) html.push(rowHtml(g, vis, rows[i], i));
        t.tBodies[0].innerHTML = html.join('');
        if (t.tFoot) {
            var any = vis.some(function (c) { return !!c.total; });
            t.tFoot.innerHTML = '<tr>' + vis.map(function (c) {
                if (!c.total || !rows.length) return '<td></td>';
                var s = 0, n = 0;
                rows.forEach(function (r) { var v = r[c.key]; if (v !== null && v !== undefined && v !== '') { s += toDouble(v); n++; } });
                var v = c.total === 'avg' ? (n ? s / n : 0) : s;
                return '<td class="num">' + esc((c.tfmt || c.fmt || netToString)(v)) + '</td>';
            }).join('') + '</tr>';
            t.tFoot.style.display = any ? '' : 'none';
        }
        if (g.filter) K.filterRow(t);
        if (g.nav) $(g.nav).textContent = rows.length ? ('Record ' + (g.cur() + 1) + ' of ' + rows.length) : '';
        if (g.editCombos) enhanceRow(g);
        if (keepKey) focusCell(t, g.cur(), keepKey);
    }
    function rowHtml(g, vis, r, i) {
        var cur = i === g.cur();
        var cls = [];
        if (cur) cls.push('is-current');
        if (g.rowClass) { var rc = g.rowClass(r); if (rc) cls.push(rc); }
        var left = 0;
        return '<tr data-i="' + i + '" class="' + cls.join(' ') + '">' + vis.map(function (c, ci) {
            var st = '', tdc = [];
            if (g.frozen && ci < g.frozen) { tdc.push('frozen'); st = ' style="left:' + left + 'px;"'; left += (c.w || 80) + 9; }
            if (c.align) tdc.push(c.align);
            var inner;
            if (c.type === 'button') {
                inner = '<button type="button" class="cellbtn" data-btn="' + c.key + '">' + esc(c.btn || c.key) + '</button>';
            } else if (c.type === 'check') {
                inner = '<input type="checkbox" data-check="1"' + (g.checked && g.checked(i) ? ' checked' : '') + '/>';
            } else if (c.type === 'link') {
                inner = '<span class="win-doc-link" data-link="' + c.key + '">' + esc(c.fmt ? c.fmt(r[c.key], r) : cellText(r[c.key])) + '</span>';
            } else if (cur && g.editable && g.editable(c, r, i)) {
                if (c.type === 'combo') {
                    var list = c.list(), v = str(r[c.key]), found = false;
                    var opts = list.map(function (o) {
                        var sel = String(o.v) === v;
                        if (sel) found = true;
                        return '<option value="' + esc(o.v) + '"' + (sel ? ' selected' : '') + '>' + esc(o.t) + '</option>';
                    });
                    if (!found) opts.unshift('<option value="' + esc(v) + '" selected>' + esc(c.fmt ? c.fmt(r[c.key], r) : '') + '</option>');
                    inner = '<select class="cell-edit win-combo" data-key="' + c.key + '" data-dtcombo="single" data-dtcombo-caption="'
                        + esc(c.cap) + '">' + opts.join('') + '</select>';
                } else {
                    inner = '<input type="text" class="cell-edit" data-key="' + c.key + '" value="' + esc(cellText(r[c.key])) + '"/>';
                }
            } else {
                inner = esc(c.fmt ? c.fmt(r[c.key], r) : cellText(r[c.key]));
            }
            return '<td' + (tdc.length ? ' class="' + tdc.join(' ') + '"' : '') + st + ' data-key="' + c.key + '">' + inner + '</td>';
        }).join('') + '</tr>';
    }
    /** Focus the editor of one cell - the enhanced combo's own input when there is one. */
    function focusCell(t, i, key) {
        var td = t.querySelector('tbody tr[data-i="' + i + '"] td[data-key="' + key + '"]');
        if (!td) return;
        var ed = td.querySelector('.dtcombo-wrap input, input.cell-edit, select.cell-edit');
        if (ed) { try { ed.focus(); } catch (e) { /* ignore */ } }
    }
    function enhanceRow(g) {
        if (!window.DesktopCombo) return;
        Array.prototype.forEach.call($(g.table).querySelectorAll('tbody select.cell-edit'), function (s) {
            try { window.DesktopCombo.enhance(s); } catch (e) { /* the native select still works */ }
        });
    }
    function wireGrid(g) {
        var t = $(g.table);
        t.addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr'); if (!tr) return;
            var i = +tr.getAttribute('data-i');
            if (i !== g.cur()) {
                g.setCur(i);
                var td = e.target.closest('td'), key = td ? td.getAttribute('data-key') : null;
                renderGrid(g);
                if (key) focusCell(t, i, key);
                if (g.select) g.select(i);
            }
            var b = e.target.closest('[data-btn]');
            if (b && g.button) g.button(b.getAttribute('data-btn'), i, b);
            var l = e.target.closest('[data-link]');
            if (l && g.link) g.link(l.getAttribute('data-link'), i);
            var c = e.target.closest('[data-check]');
            if (c && g.check) g.check(i, c.checked);
        });
        t.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tbody tr'); if (!tr || !g.dbl) return;
            if (e.target.closest('input,select,button')) return;
            g.dbl(+tr.getAttribute('data-i'));
        });
        /* EditingCell - any editor of the current row taking the focus (a plain one or an enhanced combo) */
        t.addEventListener('focusin', function (e) {
            if (!g.startEdit) return;
            var td = e.target.closest('tbody td[data-key]'); if (!td || !td.querySelector('.cell-edit')) return;
            var tr = td.closest('tr'); g.startEdit(td.getAttribute('data-key'), +tr.getAttribute('data-i'));
        });
        t.addEventListener('change', function (e) {
            var ed = e.target.closest('.cell-edit'); if (!ed || !g.commit) return;
            var tr = ed.closest('tr');
            g.commit(ed.getAttribute('data-key'), +tr.getAttribute('data-i'), ed.value);
        });
    }

    /* ---------------------------------------------------------------- value lists */
    function contractorList() { return dtcus.map(function (r) { return { v: toInt(col(r, 'Id')), t: str(col(r, 'CompanyName')) }; }); }
    function accountList(src) { return src.map(function (r) { return { v: toInt(col(r, 'Id')), t: str(col(r, 'WagesAccountName')) }; }); }
    function nameIn(list, v) {
        var n = toInt(v); if (v === null || v === undefined || v === '') return '';
        for (var i = 0; i < list.length; i++) if (list[i].v === n) return list[i].t;
        return n === 0 ? '' : str(v);
    }

    /* ============================================================ grdwagesDetail (Regular) */
    /** DetailGridSettings():682 - the visible columns, captions, widths and formats. */
    function regCols() {
        var dt = cmbValue();
        var addCond = cmpDoc(dt);
        var cols = [
            { key: 'SupplierId', cap: 'Contractor Name', w: 200, type: 'combo', list: contractorList, fmt: function (v) { return nameIn(contractorList(), v); } },
            { key: 'WagesId', cap: 'Labour / Wages Activity', w: 180, type: 'combo', list: function () { return accountList(dtwagesacc); }, fmt: function (v) { return nameIn(accountList(dtwagesacc), v); } },
            { key: 'WagesType', w: 90 },
            { key: 'Date', w: 95 },
            { key: 'packingType', w: 80 },
            { key: 'Weight', w: 90, align: 'num', fmt: f4, total: 'sum', num: true },
            { key: 'PackSize', w: 60 },
            { key: 'Quantity', w: 80, align: 'num', fmt: f4, total: 'sum', num: true },
            { key: 'WeightCut', w: 70, align: 'num' },
            { key: 'BillWeight', w: 90, align: 'num', fmt: f4, total: 'sum' },
            { key: 'RateWithoutAddLess', w: 100, align: 'num', visible: EnableAddLessOnWagesRegular },
            { key: 'RateAddLess', w: 80, align: 'num', fmt: f4, visible: EnableAddLessOnWagesRegular, num: true },
            { key: 'Rate', w: 80, align: 'num' },
            { key: 'Amount', w: 100, align: 'num', fmt: f4, total: 'sum' },
            { key: 'Item', w: 220 },
            { key: 'jobLot', w: 110 },
            { key: 'Crop', w: 70 },
            { key: 'MoveFrom', w: 150 },
            /* :712 "(DocTypeValue != 68 || DocTypeValue != 806)" is always true - MoveTo is always hidden. */
            { key: 'MoveTo', w: 150, visible: false },
            { key: 'RefLineId', cap: 'RowNo', w: 50, align: 'ctr', visible: addCond }
        ];
        if (!IsReferred) {
            cols.push({ key: 'Add', cap: 'Add', w: 40, type: 'button', btn: 'Add' });
            cols.push({ key: 'Delete', cap: 'X', w: 20, type: 'button', btn: 'X' });
        }
        return cols;
    }
    function regEditable(c, r) {
        if (c.key === 'SupplierId') return true;
        if (c.key === 'RateAddLess') return EnableAddLessOnWagesRegular;
        if (c.key === 'WagesId' || c.key === 'Weight' || c.key === 'Quantity' || c.key === 'WeightCut') return !regRefLocked;
        return false;
    }
    var REG = {
        table: 'grdwagesDetail', nav: 'navRegular', totals: true, editCombos: true,
        bound: function () { return regBound; },
        cols: regCols, rows: function () { return regBound ? dtdetail : []; },
        cur: function () { return regCur; }, setCur: function (i) { regCur = i; },
        editable: regEditable,
        rowClass: function (r) { return str(r.WagesType) === 'Free Of Cost' ? 'is-red' : ''; },
        startEdit: function (key, i) { grdwagesDetail_EditingCell(i); },
        commit: function (key, i, raw) { after(function () { return regCommit(key, i, raw); }); },
        button: function (key, i) { after(function () { return grdwagesDetail_ColumnButtonClick(key, i); }); }
    };
    function regRows() { return regBound ? dtdetail : []; }
    function othRows() { return othBound ? dtStiching : []; }

    /** A typed cell rejects text that is not a number (Janus: the value is not accepted). */
    function parseCell(c, raw) {
        if (c && c.num) {
            var t = str(raw).replace(/,/g, '').trim();
            if (t === '') return null;
            if (!/^[+-]?(\d+\.?\d*|\.\d+)([eE][+-]?\d+)?$/.test(t)) throw new Error('Input string was not in a correct format.');
            return parseFloat(t);
        }
        if (c && c.type === 'combo') return raw === '' ? null : toInt(raw);
        return raw;
    }
    function colOf(cols, key) { for (var i = 0; i < cols.length; i++) if (cols[i].key === key) return cols[i]; return null; }

    function regCommit(key, i, raw) {
        var row = dtdetail[i]; if (!row) return;
        var v;
        try { v = parseCell(colOf(regCols(), key), raw); } catch (e) { msg(e.message); renderGrid(REG); return; }
        row[key] = v;
        return guarded(grdwagesDetail_CellUpdated)(key).then(function () { renderGrid(REG); });
    }

    /** grdwagesDetail_EditingCell:1210. */
    function grdwagesDetail_EditingCell(i) {
        var item = dtdetail[i];
        if (item) PreviousContractorId = toInt(item.SupplierId);
    }

    /** grdwagesDetail_CellUpdated:855. */
    async function grdwagesDetail_CellUpdated(key) {
        var rows = regRows(), sel = regCur;
        var WagesRate = 0, WagesScheduleId = 0;
        var AllQtyExceptSelected = 0, AllNetWeightExceptSelected = 0, AllGrossWeightExceptSelected = 0;
        rows.forEach(function (r, j) {
            if (j === sel) return;
            AllQtyExceptSelected += toDouble(r.Quantity);
            AllNetWeightExceptSelected += toDouble(r.BillWeight);
            AllGrossWeightExceptSelected += toDouble(r.Weight);
        });
        var RegularTotalWeight = toDouble(val('txtGrossWeight')) - AllNetWeightExceptSelected;
        var RegularGrossWeight = toDouble(val('txtGrossWeight')) - AllGrossWeightExceptSelected;
        var RegularTotalQty = toDouble(val('txtQty')) - AllQtyExceptSelected;
        var item = rows[sel];
        if (!item) return;
        var DocTypeValue = cmbValue();
        var notCmp = !cmpDoc(DocTypeValue);
        var PreviousWagesRate = toDouble(item.Rate);
        if (key === 'ContractorName' || key === 'WagesId' || key === 'WagesAccount') {
            IsRegularOrFreeOfCost = await CheckItemsFreeofcostforWages(item.Date, cmbValue(), toInt(item.ItemId), toInt(item.WagesId));
            item.WagesType = IsRegularOrFreeOfCost ? 'Free Of Cost' : 'Regular';
        }
        if ((key === 'Quantity' || key === 'WeightCut') && toDouble(item.Quantity) > 0 && toDouble(item.PackSize) > 0) {
            var Qty = toDouble(item.Quantity);
            if (WagesAmountCalculateOnQty && Qty > RegularTotalQty && notCmp) {
                msg('Wages Total Qty can not be Greater than Wages total Qty');
                item.Quantity = round2(RegularTotalQty);
                Qty = round2(RegularTotalQty);
            } else {
                item.Quantity = round2(Qty);
            }
            var WeightCut = toDouble(item.WeightCut);
            var PackSize = toDouble(item.PackSize);
            var Weight = Qty * PackSize;
            if (!WagesAmountCalculateOnQty && Weight > RegularGrossWeight && notCmp) {
                item.Weight = round2(RegularGrossWeight);
                msg('Wages Weight can not be Greater than Wages Gross Weight');
                Weight = round2(RegularGrossWeight);
                Qty = round2(Weight / PackSize);
                item.Quantity = Qty;
            }
            item.Weight = toDouble(netToString(Weight));      // Weight.ToString() into a double column
            var WeightCutTotal = Qty * WeightCut;
            var BillWeight = Weight - Math.abs(WeightCutTotal);
            if (!WagesAmountCalculateOnQty && BillWeight > RegularTotalWeight && notCmp) {
                item.Weight = round2(RegularGrossWeight);
                item.BillWeight = round2(RegularTotalWeight);
                msg('Wages Total Weight can not be Greater than Wages total Weight');
                item.Quantity = round2(Qty * PackSize);        // :926 - Qty * PackSize into Quantity, as the desktop does
            } else {
                item.BillWeight = round2(BillWeight);
            }
        }
        if ((key === 'Weight' || key === 'WeightCut') && toDouble(item.Weight) > 0 && toDouble(item.PackSize) > 0) {
            var a = toDouble(item.Weight);
            if (!WagesAmountCalculateOnQty && a > RegularGrossWeight && notCmp) {
                item.Weight = round2(RegularGrossWeight);
                msg('Wages Weight can not be Greater than Wages Gross Weight');
                a = round2(RegularGrossWeight);
            }
            var b = toDouble(item.PackSize);
            var Qty2 = a / b;
            item.Quantity = round2(Qty2);
            if (WagesAmountCalculateOnQty && Qty2 > RegularTotalQty && notCmp) {
                msg('Wages Total Qty can not be Greater than Wages total Qty');
                item.Quantity = round2(RegularTotalQty);
                Qty2 = round2(RegularTotalQty);
                var PackSize2 = toDouble(item.PackSize);
                item.Weight = round2(Qty2 * PackSize2);
            }
            var num = toDouble(item.Weight);
            var WeightCutTotal2 = toDouble(item.WeightCut) * Qty2;
            var BillWeight2 = num - Math.abs(WeightCutTotal2);
            if (!WagesAmountCalculateOnQty && BillWeight2 > RegularTotalWeight && notCmp) {
                item.Weight = round2(RegularGrossWeight);
                item.BillWeight = round2(RegularTotalWeight);
                msg('Wages Total Weight can not be Greater than Wages total Weight');
            } else {
                item.BillWeight = round2(BillWeight2);
            }
        }
        if (key === 'WagesId' || key === 'SupplierId' || key === 'Quantity' || key === 'Weight' || key === 'WeightCut' || key === 'RateAddLess') {
            if (str(item.WagesType) === 'Free Of Cost' || str(item.WarehouseType) === 'Dryer') {
                item.WagesScheduleId = 0;
                item.Rate = 0;
                item.Amount = 0;
            } else {
                var d = await GetWagesRate(item.Date, toDouble(item.PackSize), toInt(item.WagesId), toInt(item.SupplierId));
                if (d) { WagesRate = toDouble(d.WagesRate); WagesScheduleId = toInt(d.ScheduleId); }
            }
        }
        if (!IsReferred || WagesRate === PreviousWagesRate) {
            if (WagesRate > 0) {
                item.Rate = WagesRate;
                item.WagesScheduleId = WagesScheduleId;
                if (toDouble(item.Rate) > 0 && toDouble(item.BillWeight) > 0) {
                    var Quantity = toDouble(item.Quantity);
                    var Packsize = toDouble(item.PackSize);
                    var BillWeight3 = toDouble(item.BillWeight);
                    var RateWithoutAddLess = toDouble(item.Rate);
                    item.RateWithoutAddLess = RateWithoutAddLess;
                    var RateAddLess = rateAddLessClamp(item, key, RateWithoutAddLess, true);
                    var Rate = RateWithoutAddLess + RateAddLess;
                    item.Rate = Rate;
                    var Amount = !WagesAmountCalculateOnQty ? (BillWeight3 / Packsize * Rate) : (Quantity * Rate);
                    item.Amount = round2(Amount);
                } else {
                    item.Amount = 0;
                }
            } else {
                item.WagesScheduleId = 0;
                item.Rate = 0;
                item.RateWithoutAddLess = 0;
                item.Amount = 0;
            }
        } else {
            item.SupplierId = PreviousContractorId;
            msg('Contractor You are trying Change has different rate than Previous So it Can not be Change');
        }
    }

    /** The RateAddLess block shared by both grids' CellUpdated (with its messages). */
    function rateAddLessClamp(item, key, RateWithoutAddLess, withMessage) {
        var RateAddLess = toDouble(item.RateAddLess);
        if (EnableAddLessOnWagesRegular && (key === 'RateAddLess' || key === null)) {
            if (PercentageForRateAddLess > 0) {
                var byCfg = PercentageForRateAddLess * RateWithoutAddLess / 100;
                var minus = !(RateAddLess > 0);
                if (Math.abs(RateAddLess) > byCfg) {
                    RateAddLess = minus ? toDouble('-' + netToString(byCfg)) : byCfg;
                    item.RateAddLess = RateAddLess;
                    if (withMessage) msg('RateAddLess can not be grater than RateAdLess In config ' + netToString(byCfg));
                }
            } else {
                RateAddLess = 0;
                item.RateAddLess = RateAddLess;
                if (withMessage) msg('Please set RateAddLess Percentage in config first...');
            }
        }
        return RateAddLess;
    }

    /** grdwagesDetail_ColumnButtonClick:1104. */
    function grdwagesDetail_ColumnButtonClick(key, i) {
        var item = dtdetail[i];
        if (!item || IsReferred) return;
        if (key === 'Delete') {
            if (dtdetail.length <= 1) { msg('You Can Not Delete All rows'); return; }
            dtdetail.splice(i, 1);
            if (regCur >= dtdetail.length) regCur = dtdetail.length - 1;
            renderGrid(REG);
        }
        if (key === 'Add') AddRowInGLGrid();
    }

    /** AddRowInGLGrid():1134. */
    function AddRowInGLGrid() {
        try {
            var TotalWeight = toDouble(val('txtGrossWeight').trim());
            var TotalQty = toDouble(val('txtQty').trim());
            var Weight = 0, WeightCut = 0, BillWeight = 0, Qty = 0;
            regRows().forEach(function (r) {
                Weight += toDouble(r.Weight);
                WeightCut = toDouble(r.WeightCut);             // "=" not "+=" - the last row's cut
                BillWeight += toDouble(r.BillWeight);
                Qty += toDouble(r.Quantity);
            });
            var GrossWeight = TotalWeight - Weight;
            var GrossQty = TotalQty - Qty;
            var DocTypeValue = cmbValue();
            var OtherCondition = cmpDoc(DocTypeValue);
            var c = regRows()[regCur];
            if (GrossWeight > 0 || GrossQty > 0 || OtherCondition) {
                if (!c) throw new Error(NRE);
                if (WagesAmountCalculateOnQty) {
                    if (GrossQty > 0) {
                        var ItemWeight = GrossQty * toDouble(c.PackSize);
                        var BillQtyPartal = GrossQty;
                        var Amount = BillQtyPartal * toDouble(c.Rate);
                        dtdetail.push(newRow([c.SupplierId, c.ContractorName, c.WagesId, c.WagesAccount, c.WagesType, c.Date, c.packingTypeId, c.packingType,
                            round2(ItemWeight), c.PackSize, round2(BillQtyPartal), c.WeightCut, c.BillQty, ItemWeight, c.RateWithoutAddLess, c.RateAddLess,
                            c.Rate, round2(Amount), c.ItemId, c.Item, c.jobLotId, c.jobLot, c.Crop, c.MoveFromId, c.MoveFrom, c.MoveToId, c.MoveTo,
                            c.PurchaseGLAC, c.WarehouseType, c.RefDocQty, c.RefDocWeight, c.RefLineId]));
                    } else if (OtherCondition) {
                        dtdetail.push(copy32(c));
                    } else {
                        msg('Please Check Grid Qty and TotalQty');
                    }
                } else if (GrossWeight > 0) {
                    var ItemQty = GrossWeight / toDouble(c.PackSize);
                    var BillWeightPartal = GrossWeight - ItemQty * WeightCut;
                    var Amount2 = BillWeightPartal / toDouble(c.PackSize) * toDouble(c.Rate);
                    dtdetail.push(newRow([c.SupplierId, c.ContractorName, c.WagesId, c.WagesAccount, c.WagesType, c.Date, c.packingTypeId, c.packingType,
                        GrossWeight, c.PackSize, round2(ItemQty), c.WeightCut, c.BillQty, BillWeightPartal, c.RateWithoutAddLess, c.RateAddLess,
                        c.Rate, round2(Amount2), c.ItemId, c.Item, c.jobLotId, c.jobLot, c.Crop, c.MoveFromId, c.MoveFrom, c.MoveToId, c.MoveTo,
                        c.PurchaseGLAC, c.WarehouseType, c.RefDocQty, c.RefDocWeight, c.RefLineId]));
                } else if (OtherCondition) {
                    dtdetail.push(copy32(c));
                } else {
                    msg('Please Check Grid GrossWeight and TotalGrossWeight');
                }
            } else {
                msg('Please Check Grid GrossWeight and TotalGrossWeight');
            }
            renderGrid(REG);
        } catch (e) { msg(e.message); }
    }
    /** dtdetail.Rows.Add of the current row's first 32 cells (WagesScheduleId is not copied). */
    function copy32(c) { var vals = COLS.slice(0, 32).map(function (k) { return c[k]; }); return newRow(vals); }

    /* ================================================================ grdStiching (Other) */
    /** GridStichingSettings():1823. */
    function othCols() {
        var dt = cmbValue();
        var cols = [
            { key: 'SupplierId', cap: 'Contractor Name', w: 200, type: 'combo', list: contractorList, fmt: function (v) { return nameIn(contractorList(), v); } },
            { key: 'WagesId', cap: 'Labour / Wages Activity', w: 180, type: 'combo', list: function () { return accountList(dtStichingItems); }, fmt: function (v) { return nameIn(accountList(dtStichingItems), v); } },
            { key: 'WagesType', w: 90 },
            { key: 'Date', w: 95 },
            { key: 'packingType', w: 80 },
            { key: 'Weight', w: 90, total: 'sum', num: true },
            { key: 'PackSize', w: 60 },
            { key: 'Quantity', w: 80, total: 'sum', num: true },
            { key: 'WeightCut', w: 70 },
            { key: 'BillWeight', w: 90, total: 'sum' },
            { key: 'RateWithoutAddLess', w: 100, visible: EnableAddLessOnWagesRegular },
            { key: 'RateAddLess', w: 80, total: 'sum', visible: EnableAddLessOnWagesRegular, num: true },
            { key: 'Rate', w: 80 },
            { key: 'Amount', w: 100, total: 'sum' },
            { key: 'Item', w: 220 },
            { key: 'jobLot', w: 110 },
            { key: 'Crop', w: 70 },
            { key: 'MoveFrom', w: 150 },
            { key: 'MoveTo', w: 150, visible: false },
            { key: 'RefLineId', cap: 'RowNo', w: 50, visible: dt === 112 || dt === 66 }
        ];
        if (!IsReferred) {
            cols.push({ key: 'Add', cap: 'Add', w: 40, type: 'button', btn: 'Add' });
            cols.push({ key: 'Delete', cap: 'X', w: 20, type: 'button', btn: 'X' });
        }
        return cols;
    }
    function othEditable(c) {
        if (c.key === 'SupplierId' || c.key === 'WeightCut') return true;
        if (c.key === 'RateAddLess') return EnableAddLessOnWagesRegular;
        if (c.key === 'WagesId' || c.key === 'Weight' || c.key === 'Quantity') return !othRefLocked;
        return false;
    }
    var OTH = {
        table: 'grdStiching', nav: 'navOther', totals: true, editCombos: true,
        bound: function () { return othBound; },
        cols: othCols, rows: othRows,
        cur: function () { return othCur; }, setCur: function (i) { othCur = i; },
        editable: othEditable,
        rowClass: function (r) { return str(r.WagesType) === 'Free Of Cost' ? 'is-red' : ''; },
        startEdit: function (key, i) { var it = dtStiching[i]; if (it) PreviousContractorIdStiching = toInt(it.SupplierId); },
        commit: function (key, i, raw) { after(function () { return othCommit(key, i, raw); }); },
        button: function (key, i) { after(function () { return grdStiching_ColumnButtonClick(key, i); }); }
    };
    function othCommit(key, i, raw) {
        var row = dtStiching[i]; if (!row) return;
        var v;
        try { v = parseCell(colOf(othCols(), key), raw); } catch (e) { msg(e.message); renderGrid(OTH); return; }
        row[key] = v;
        return guarded(grdStiching_CellUpdated)(key).then(function () { renderGrid(OTH); });
    }

    /** grdStiching_ColumnButtonClick:1301. */
    function grdStiching_ColumnButtonClick(key, i) {
        var item = dtStiching[i];
        if (!item || IsReferred) return;
        if (key === 'Delete') {
            if (dtStiching.length <= 1) { msg('You Can Not Delete All rows....'); return; }
            dtStiching.splice(i, 1);
            if (othCur >= dtStiching.length) othCur = dtStiching.length - 1;
            renderGrid(OTH);
        }
        if (key === 'Add') AddRowInStichingGrid();
    }

    /** AddRowInStichingGrid():1227 - WagesType comes from grdwagesDetail.CurrentRow, as the desktop reads it. */
    function AddRowInStichingGrid() {
        try {
            var TotalWeight = toDouble(val('txtGrossWeight').trim());
            var TotalQty = toDouble(val('txtQty').trim());
            var Weight = 0, WeightCut = 0, BillWeight = 0, Qty = 0;
            othRows().forEach(function (r) {
                Weight += toDouble(r.Weight);
                WeightCut = toDouble(r.WeightCut);
                BillWeight += toDouble(r.BillWeight);
                Qty += toDouble(r.Quantity);
            });
            var GrossWeight = TotalWeight - Weight;
            var GrossQty = TotalQty - Qty;
            var dt = cmbValue();
            var c = othRows()[othCur];
            var rw = regRows()[regCur];
            function wt() { if (!rw) throw new Error(NRE); return rw.WagesType; }
            function copyAll() {
                return newRow([c.SupplierId, c.ContractorName, c.WagesId, c.WagesAccount, wt(), c.Date, c.packingTypeId, c.packingType,
                    c.Weight, c.PackSize, c.Quantity, c.WeightCut, c.BillQty, c.BillWeight, c.RateWithoutAddLess, c.RateAddLess, c.Rate, c.Amount,
                    c.ItemId, c.Item, c.jobLotId, c.jobLot, c.Crop, c.MoveFromId, c.MoveFrom, c.MoveToId, c.MoveTo, c.PurchaseGLAC,
                    c.WarehouseType, c.RefDocQty, c.RefDocWeight, c.RefLineId]);
            }
            if (GrossWeight > 0 || GrossQty > 0 || dt === 112 || dt === 66) {
                if (!c) throw new Error(NRE);
                if (WagesAmountCalculateOnQty) {
                    if (GrossQty > 0) {
                        var ItemWeight = GrossQty * toDouble(c.PackSize);
                        var BillQtyPartal = GrossQty;
                        var Amount = BillQtyPartal * toDouble(c.Rate);
                        dtStiching.push(newRow([c.SupplierId, c.ContractorName, c.WagesId, c.WagesAccount, wt(), c.Date, c.packingTypeId, c.packingType,
                            ItemWeight, c.PackSize, round2(BillQtyPartal), c.WeightCut, c.BillQty, ItemWeight, c.RateWithoutAddLess, c.RateAddLess,
                            c.Rate, round2(Amount), c.ItemId, c.Item, c.jobLotId, c.jobLot, c.Crop, c.MoveFromId, c.MoveFrom, c.MoveToId, c.MoveTo,
                            c.PurchaseGLAC, c.WarehouseType, c.RefDocQty, c.RefDocWeight, c.RefLineId]));
                    } else if (dt === 112 || dt === 66) {
                        dtStiching.push(copyAll());
                    } else {
                        msg('Please Check Grid Qty and TotalQty');
                    }
                } else if (GrossWeight > 0) {
                    var ItemQty = GrossWeight / toDouble(c.PackSize);
                    var BillWeightPartal = GrossWeight - ItemQty * WeightCut;
                    var Amount2 = BillWeightPartal / toDouble(c.PackSize) * toDouble(c.Rate);
                    dtStiching.push(newRow([c.SupplierId, c.ContractorName, c.WagesId, c.WagesAccount, wt(), c.Date, c.packingTypeId, c.packingType,
                        GrossWeight, c.PackSize, round2(ItemQty), c.WeightCut, c.BillQty, BillWeightPartal, c.RateWithoutAddLess, c.RateAddLess,
                        c.Rate, round2(Amount2), c.ItemId, c.Item, c.jobLotId, c.jobLot, c.Crop, c.MoveFromId, c.MoveFrom, c.MoveToId, c.MoveTo,
                        c.PurchaseGLAC, c.WarehouseType, c.RefDocQty, c.RefDocWeight, c.RefLineId]));
                } else if (dt === 112 || dt === 66) {
                    dtStiching.push(copyAll());
                } else {
                    msg('Please Check Grid GrossWeight and TotalGrossWeight');
                }
            } else {
                msg('Please Check Grid GrossWeight and TotalGrossWeight');
            }
            renderGrid(OTH);
        } catch (e) { msg(e.message); }
    }

    /** the "amount" tail of grdStiching_CellUpdated - Rate &gt; 0 and BillWeight &gt; 0. */
    function othAmount(item, key) {
        if (toDouble(item.Rate) > 0 && toDouble(item.BillWeight) > 0) {
            var Packsize = toDouble(item.PackSize);
            var bw = toDouble(item.BillWeight);
            var RateWithoutAddLess = toDouble(item.Rate);
            item.RateWithoutAddLess = RateWithoutAddLess;
            var RateAddLess = rateAddLessClamp(item, key, RateWithoutAddLess, true);
            var Rate = RateWithoutAddLess + RateAddLess;
            item.Rate = Rate;
            item.Amount = round2(bw / Packsize * Rate);
        } else {
            item.Amount = 0;
        }
    }

    /** grdStiching_CellUpdated:1331. */
    async function grdStiching_CellUpdated(key) {
        var rows = othRows(), sel = othCur;
        var AllQtyExceptSelected = 0, AllNetWeightExceptSelected = 0, AllGrossWeightExceptSelected = 0;
        rows.forEach(function (r, j) {
            if (j === sel) return;
            AllQtyExceptSelected += toDouble(r.Quantity);
            AllNetWeightExceptSelected += toDouble(r.BillWeight);
            AllGrossWeightExceptSelected += toDouble(r.Weight);
        });
        var RegularTotalWeight = toDouble(val('txtGrossWeight')) - AllNetWeightExceptSelected;
        var RegularGrossWeight = toDouble(val('txtGrossWeight')) - AllGrossWeightExceptSelected;
        var RegularTotalQty = toDouble(val('txtQty')) - AllQtyExceptSelected;
        var item = rows[sel];
        if (!item) throw new Error(NRE);
        var dt = cmbValue();
        var not11266 = dt !== 112 && dt !== 66;
        if (key === 'ContractorName' || key === 'WagesId' || key === 'WagesAccount') {
            IsRegularOrFreeOfCost = await CheckItemsFreeofcostforWages(item.Date, cmbValue(), toInt(item.ItemId), toInt(item.WagesId));
            item.WagesType = IsRegularOrFreeOfCost ? 'Free Of Cost' : 'Regular';
        }
        if (key === 'WagesId' || key === 'SupplierId' || key === 'Quantity' || key === 'Weight' || key === 'WeightCut' || key === 'RateAddLess') {
            if (str(item.WagesType) === 'Free Of Cost' || str(item.WarehouseType) === 'Dryer') {
                item.Rate = 0;
                item.Amount = 0;
            } else {
                var PreviousWagesRate = toDouble(item.Rate);
                var WagesRate = 0, WagesScheduleId = 0;
                var d = await GetWagesRate(item.Date, toDouble(item.PackSize), toInt(item.WagesId), toInt(item.SupplierId));
                if (d) { WagesRate = toDouble(d.WagesRate); WagesScheduleId = toInt(d.ScheduleId); }
                var referredSame = IsReferred && WagesRate === PreviousWagesRate;
                if (!IsReferred || referredSame) {
                    /* the IsReferred copy drops the 112/66 exemption from the weight checks (:1483, :1499, :1509) */
                    var ex = IsReferred ? true : not11266;
                    if (WagesRate > 0) {
                        item.WagesScheduleId = WagesScheduleId;
                        item.Rate = WagesRate;
                        if (toDouble(item.Weight) > 0 && toDouble(item.PackSize) > 0) {
                            var Weight = toDouble(item.Weight);
                            if (!WagesAmountCalculateOnQty && Weight > RegularTotalWeight && ex) {
                                msg('Stitching Total Weight can not be Greater than Wages total Weight');
                                item.BillWeight = round2(RegularTotalWeight);
                            } else {
                                item.BillWeight = round2(Weight);
                            }
                        }
                        if (toDouble(item.Quantity) > 0 && toDouble(item.WeightCut) > 0 && toDouble(item.PackSize) > 0 && toDouble(item.Weight) > 0) {
                            var Qty = toDouble(item.Quantity);
                            var BillWeight = toDouble(item.Weight) - Math.abs(toDouble(item.WeightCut) * Qty);
                            if (!WagesAmountCalculateOnQty && BillWeight > RegularTotalWeight && ex) {
                                msg('Stitching Total Weight can not be Greater than Wages total Weight');
                                item.BillWeight = round2(RegularTotalWeight);
                                item.Weight = round2(RegularGrossWeight);
                            } else {
                                item.BillWeight = round2(BillWeight);
                            }
                            if (WagesAmountCalculateOnQty && Qty > RegularTotalQty && ex) {
                                msg('Stitching Total Qty can not be Greater than Wages total Qty');
                                item.Quantity = round2(RegularTotalQty);
                            } else {
                                item.Quantity = round2(Qty);
                            }
                        }
                        othAmount(item, key);
                    } else if (!IsReferred) {
                        item.Rate = 0;
                        item.WagesScheduleId = 0;
                    } else {
                        item.WagesScheduleId = 0;
                        item.Rate = 0;
                        item.RateWithoutAddLess = 0;
                    }
                } else {
                    item.SupplierId = PreviousContractorIdStiching;
                    msg('Contractor You are trying Change has different rate than Previous So it Can not be Change');
                }
            }
        }
        if (key === 'Weight' || key === 'WeightCut') {
            if (toDouble(item.Weight) > 0 && toDouble(item.PackSize) > 0) {
                var Qty3 = toDouble(item.Weight) / toDouble(item.PackSize);
                var Weight3 = toDouble(item.Weight);
                if (!WagesAmountCalculateOnQty && Weight3 > RegularTotalWeight) {
                    msg('Stitching Total Weight can not be Greater than Wages total Weight');
                    item.BillWeight = round2(RegularTotalWeight);
                    item.Weight = round2(RegularGrossWeight);
                } else {
                    item.BillWeight = round2(Weight3);
                }
                if (WagesAmountCalculateOnQty && Qty3 > RegularTotalQty) {
                    msg('Stitching Total Qty can not be Greater than Wages total Qty');
                    item.Quantity = round2(RegularTotalQty);
                } else {
                    item.Quantity = round2(Qty3);
                }
            }
            if (toDouble(item.Quantity) > 0 && toDouble(item.WeightCut) > 0 && toDouble(item.PackSize) > 0 && toDouble(item.Weight) > 0) {
                var Qty4 = toDouble(item.Quantity);
                var BillWeight3 = toDouble(item.Weight) - Math.abs(toDouble(item.WeightCut) * Qty4);
                if (!WagesAmountCalculateOnQty && BillWeight3 > RegularTotalWeight) {
                    msg('Stitching Total Weight can not be Greater than Wages total Weight');
                    item.BillWeight = round2(RegularTotalWeight);
                    item.Weight = round2(RegularGrossWeight);
                } else {
                    item.BillWeight = round2(BillWeight3);
                }
                if (WagesAmountCalculateOnQty && Qty4 > RegularTotalQty) {
                    msg('Stitching Total Qty can not be Greater than Wages total Qty');
                    item.Quantity = round2(RegularTotalQty);
                } else {
                    item.Quantity = round2(Qty4);
                }
            }
            othAmount(item, key);
        }
        if (key === 'Quantity' || key === 'WeightCut') {
            if (toDouble(item.Quantity) > 0 && toDouble(item.PackSize) > 0 && toDouble(item.Weight) > 0) {
                var WeightCut4 = toDouble(item.WeightCut);
                var PackSize = toDouble(item.PackSize);
                var Qty5 = toDouble(item.Quantity);
                var Weight4 = Qty5 * PackSize;
                var BillWeight4 = Weight4 - Math.abs(Qty5 * Math.abs(WeightCut4));
                if (!WagesAmountCalculateOnQty && BillWeight4 > RegularTotalWeight && not11266) {
                    msg('Stitching Total Weight can not be Greater than Wages total Weight');
                    item.BillWeight = round2(RegularTotalWeight);
                    item.Weight = round2(RegularGrossWeight);
                } else {
                    item.Weight = toDouble(netToString(Weight4));
                    item.BillWeight = round2(BillWeight4);
                }
                if (WagesAmountCalculateOnQty && Qty5 > RegularTotalQty && not11266) {
                    msg('Stitching Total Qty can not be Greater than Wages total Qty');
                    item.Quantity = round2(RegularTotalQty);
                } else {
                    item.Quantity = round2(Qty5);
                }
            }
            if (toDouble(item.Quantity) > 0 && toDouble(item.WeightCut) > 0 && toDouble(item.PackSize) > 0 && toDouble(item.Weight) > 0) {
                var Qty6 = toDouble(item.Quantity);
                var BillWeight5 = toDouble(item.Weight) - Math.abs(toDouble(item.WeightCut) * Qty6);
                if (!WagesAmountCalculateOnQty && BillWeight5 > RegularTotalWeight && not11266) {
                    msg('Stitching Total Weight can not be Greater than Wages total Weight');
                    item.BillWeight = round2(RegularTotalWeight);
                    item.Weight = round2(RegularGrossWeight);
                } else {
                    item.BillWeight = round2(BillWeight5);
                }
            }
            /* :1709 - this copy does not refresh RateWithoutAddLess */
            if (toDouble(item.Rate) > 0 && toDouble(item.BillWeight) > 0) {
                var Packsize4 = toDouble(item.PackSize);
                var num11 = toDouble(item.BillWeight);
                var RateWithoutAddLess4 = toDouble(item.Rate);
                var RateAddLess4 = rateAddLessClamp(item, key, RateWithoutAddLess4, true);
                var Rate4 = RateWithoutAddLess4 + RateAddLess4;
                item.Rate = Rate4;
                item.Amount = round2(num11 / Packsize4 * Rate4);
            } else {
                item.Amount = 0;
            }
        }
    }

    /* ============================================================ Apply All :3867-4011 */
    /** AddContractorValuesForAllInGrid():3879. */
    async function AddContractorValuesForAllInGrid(which) {
        var rows = which === 'reg' ? regRows() : othRows();
        var cur = which === 'reg' ? regCur : othCur;
        if (cur < 0 || !rows[cur] || rows.length === 0) return;
        var row = rows[0];
        var ContractorValue = toInt(row.SupplierId);
        var WagesAccountValue = toInt(row.WagesId);
        if (ContractorValue === 0 && WagesAccountValue === 0) { msg('First row contains No values Of Contractor And Wages Account.'); return; }
        var focs = await postJson(API + '/free-of-cost', { rows: rows.map(function (r) {
            return { date: str(r.Date), refDocumentTypeId: cmbValue(), itemId: toInt(r.ItemId), wagesAccountId: WagesAccountValue };
        }) });
        rows.forEach(function (r, j) {
            r.SupplierId = ContractorValue;
            r.WagesId = WagesAccountValue;
            IsRegularOrFreeOfCost = !!(focs && focs[j]);
            r.WagesType = IsRegularOrFreeOfCost ? 'Free Of Cost' : 'Regular';
        });
        var reqs = [], idx = [];
        rows.forEach(function (r, j) {
            if (str(r.WagesType) === 'Free Of Cost' || str(r.WarehouseType) === 'Dryer') return;
            reqs.push({ date: str(r.Date), packSize: toDouble(r.PackSize), wagesAccountId: toInt(r.WagesId), contractorId: toInt(r.SupplierId) });
            idx.push(j);
        });
        var rates = reqs.length ? await postJson(API + '/wages-rate', { rows: reqs }) : [];
        var byRow = {};
        idx.forEach(function (j, k) { byRow[j] = rates[k]; });
        rows.forEach(function (r, j) {
            var WagesRate = 0, WagesScheduleId = 0;
            if (str(r.WagesType) === 'Free Of Cost' || str(r.WarehouseType) === 'Dryer') {
                r.WagesScheduleId = 0; r.Rate = 0; r.Amount = 0;
            } else if (byRow[j]) {
                WagesRate = toDouble(byRow[j].WagesRate); WagesScheduleId = toInt(byRow[j].ScheduleId);
            }
            if (WagesRate > 0) {
                r.WagesScheduleId = WagesScheduleId;
                r.Rate = WagesRate;
                if (toDouble(r.Weight) > 0 && toDouble(r.PackSize) > 0) r.BillWeight = round2(toDouble(r.Weight));
                if (toDouble(r.Quantity) > 0 && toDouble(r.WeightCut) > 0 && toDouble(r.PackSize) > 0 && toDouble(r.Weight) > 0) {
                    r.BillWeight = round2(toDouble(r.Weight) - Math.abs(toDouble(r.WeightCut) * toDouble(r.Quantity)));
                }
                if (toDouble(r.Rate) > 0 && toDouble(r.BillWeight) > 0) {
                    var Packsize = toDouble(r.PackSize), bw = toDouble(r.BillWeight), rwal = toDouble(r.Rate);
                    r.RateWithoutAddLess = rwal;
                    var ral = rateAddLessClamp(r, null, rwal, false);     // no key test and no message here
                    var Rate = rwal + ral;
                    var Amount = !WagesAmountCalculateOnQty ? (bw / Packsize * Rate) : (toDouble(r.Quantity) * Rate);
                    r.Amount = round2(Amount);
                } else {
                    r.Amount = 0;
                }
            } else {
                r.Rate = 0;
                r.WagesScheduleId = 0;
            }
        });
        renderGrid(which === 'reg' ? REG : OTH);
    }

    /* ================================================================ pending / retrieve */
    function pendCols() {
        var c = [];
        if (IS_ADMIN) c.push({ key: '__sel', cap: '', w: 30, type: 'check' });
        c.push({ key: 'Load', cap: 'Load', w: 50, type: 'button', btn: 'Load' });
        c.push({ key: 'DocumentTypeDescription', w: 150 });
        c.push({ key: 'DocDate', w: 85, fmt: dMMMyyyy });
        c.push({ key: 'DocNo', w: 65, align: 'ctr' });
        c.push({ key: 'SupplierCustomer', w: 200 });
        c.push({ key: 'GpNo', w: 65, align: 'ctr' });
        c.push({ key: 'VehicleNo', w: 110 });
        c.push({ key: 'BiltyNo', w: 110 });
        c.push({ key: 'GrossWeight', w: 100, align: 'num', fmt: f3, total: 'sum' });
        c.push({ key: 'TotalQty', w: 100, align: 'num', fmt: f3, total: 'sum' });
        return c;
    }
    var PEND = {
        table: 'gridPendingWagesSlip', nav: 'navPending', filter: true, frozen: 2,
        bound: function () { return pendingRows.length > 0; },
        cols: pendCols, rows: function () { return pendingRows; },
        cur: function () { return pendCur; }, setCur: function (i) { pendCur = i; },
        checked: function (i) { return !!pendingChecked[i]; },
        check: function (i, on) { pendingChecked[i] = on; },
        button: function (key, i, b) {
            if (key !== 'Load') return;
            pendCur = i;
            after(function () { return busy(b, guarded(LoadDataForWages)); });
        }
    };

    function retCols() {
        return [
            { key: 'ContractorName', w: 180 },
            { key: 'WagesAccount', w: 160 },
            { key: 'packingType', w: 80 },
            { key: 'Weight', w: 80, align: 'num', fmt: f00, total: 'sum' },
            { key: 'PackSize', w: 60 },
            { key: 'Quantity', w: 80, align: 'num', fmt: f00, total: 'sum' },
            { key: 'WeightCut', w: 70 },
            { key: 'BillWeight', w: 80, align: 'num', fmt: f00, total: 'sum' },
            { key: 'RateWithoutAddLess', w: 90, align: 'num', visible: EnableAddLessOnWagesRegular },
            { key: 'RateAddLess', w: 80, align: 'num', fmt: f2, total: 'avg', visible: EnableAddLessOnWagesRegular },
            { key: 'Rate', w: 70, align: 'num' },
            { key: 'Amount', w: 90, align: 'num', fmt: f00, total: 'sum' }
        ];
    }
    var RET = {
        table: 'grdDataRetrieve', nav: 'navRetrieve', filter: true,
        bound: function () { return retBound; },
        cols: retCols, rows: function () { return retBound ? retrieveRows : []; },
        cur: function () { return retCur; }, setCur: function (i) { retCur = i; }
    };

    /** PendingGrnAndGdn():1974 / PendingTicket():2040 - the same list shape. */
    async function bindPending() {
        var rows = await getJson(API + '/pending?' + qs({ refDocTypeId: RefDocTypeId, refDocId: RefDocId }));
        pendingRows = rows || [];
        pendingChecked = {};
        pendCur = pendingRows.length ? 0 : -1;
        renderGrid(PEND);
    }
    var PendingGrnAndGdn = guarded(bindPending);
    var PendingTicket = guarded(bindPending);

    function retrievalRow(h, d) {
        return {
            HeaderId: col(d, 'InvContractorWagesBillHeaderId'), ContractorName: col(d, 'CompanyName'), WagesAccount: col(d, 'WagesAccountName'),
            packingType: col(d, 'PackTypeDesc'), Weight: col(d, 'Weight'), PackSize: col(d, 'PackSize'), Quantity: col(d, 'Qty'),
            WeightCut: col(d, 'WeightCut'), BillWeight: col(d, 'BillWeight'),
            RateWithoutAddLess: toDouble(col(d, 'WageRate')) - toDouble(col(d, 'RateAddLess')), RateAddLess: col(d, 'RateAddLess'),
            Rate: col(d, 'WageRate'), Amount: col(d, 'WagesAmount'),
            HeaderWeight: h ? col(h, 'WeightTotal') : col(d, 'WeightTotal'), HeaderQty: h ? col(h, 'QtyTotal') : col(d, 'QtyTotal'),
            RefDocQty: col(d, 'RefDocQty'), RefDocWeight: col(d, 'RefDocWeight'), RefLineId: col(d, 'RefLineId')
        };
    }
    /** RetreivalHistoryDetailGridBind():2291. */
    var RetreivalHistoryDetailGridBind = guarded(async function (Id) {
        var head = await GetByID(Id);
        if (!head) throw new Error(OOR);
        retrieveRows = (col(head, 'invContractWagesBillDateil') || []).map(function (d) { return retrievalRow(head, d); });
        retBound = true; retCur = retrieveRows.length ? 0 : -1;
        renderGrid(RET);
    });
    /** RetreivalDetailGridDeletedData():2332. */
    async function RetreivalDetailGridDeletedData() {
        try {
            var rows = await getJson(API + '/previous-records?' + qs({ refDocTypeId: RefDocTypeId, refDocId: RefDocId }));
            if (rows && rows.length) {
                retrieveRows = rows.map(function (d) { return retrievalRow(null, d); });
                retBound = true; retCur = 0;
                renderGrid(RET);
                grdwagesDetailhaveDeletedRecords = true;
            }
        } catch (e) { msg(e.message); }
        return grdwagesDetailhaveDeletedRecords;
    }

    /* ================================================================== the Load helpers */
    /** accountName(DocumentTypeId):523. */
    async function accountName(DocumentTypeId) {
        DocumentTypeId = DocumentTypeId || 0;
        try {
            var Ids = null;
            var a = [46, 80, 143], b = [86, 112, 210];
            if (a.indexOf(RefDocTypeId) >= 0 || a.indexOf(DocumentTypeId) >= 0) Ids = '34,36,44';
            else if (b.indexOf(RefDocTypeId) >= 0 || b.indexOf(DocumentTypeId) >= 0) Ids = '33,36,44';
            else if (RefDocTypeId === 68 || DocumentTypeId === 68 || RefDocTypeId === 806 || DocumentTypeId === 806) Ids = '33,34';
            var m = wagesTypeRow('DocumentTypeId', DocumentTypeId);
            if (m) Ids = str(col(m, 'WagesActivityIds'));
            dtwagesacc = await GetWagesAccount(Ids);
        } catch (e) { msg(e.message); }
    }
    /** StichingWagesConfig():442. */
    function StichingWagesConfig() {
        var v = cmbValue();
        if (v === 112 || RefDocTypeId === 112) StichingWagesCompulsory = toBool(CFG.StichingWagesCompulsory);
        else if (v === 66 || RefDocTypeId === 66) StichingWagesCompulsory = toBool(CFG.OtherWagesCompulsoryForStockConversion);
        else StichingWagesCompulsory = false;
    }
    /** GenerateDocNo():465. */
    var GenerateDocNo = guarded(async function () {
        var r = await getJson(API + '/next-doc-no');
        var code = toInt(r && r.docNo);
        if (code > 0) setVal('txtdocnumber', String(code));
    });
    /** DocumentTypeFillForCombo():569. */
    async function DocumentTypeFillForCombo() {
        var rows = await getJson(API + '/history-document-types');
        var s = $('CmbDocumentTypeHistory');
        s.innerHTML = '<option value=""></option>' + (rows || []).map(function (r) {
            return '<option value="' + esc(col(r, 'Id')) + '">' + esc(col(r, 'name')) + '</option>';
        }).join('');
    }

    /** LoadDataForWages():2121 - loads the current pending row into both grids. */
    async function LoadDataForWages() {
        if ((!TOOL.btnsave.visible || TOOL.btnUpdate.visible || RECID > 0) && RefDocId === 0) {
            msg('Record Not Loaded Please Reset Form First');
            return;
        }
        showTab('tabPage4', false);
        var pr = pendingRows[pendCur];
        if (!pr) throw new Error(NRE);
        var DocumentTypeId = 0;
        cmbRef.list = [{ Id: col(pr, 'DocumentTypeId'), name: col(pr, 'DocumentTypeDescription') }];
        cmbRef.value = col(pr, 'DocumentTypeId');
        paintCmbRef();
        DocumentTypeId = toInt(col(pr, 'DocumentTypeId'));
        setVal('txtDocNo', cellText(col(pr, 'DocNo')));
        setVal('txtGRNId', cellText(col(pr, 'Id')));
        setVal('txtGpNo', cellText(col(pr, 'GpNo')));
        setVal('txtGrossWeight', cellText(col(pr, 'GrossWeight')));
        setVal('txtQty', cellText(col(pr, 'TotalQty')));
        if ([66, 68, 808, 806].indexOf(DocumentTypeId) >= 0) setVal('txtEntryType', str(col(pr, 'SupplierCustomer')).trim());
        else setVal('txtEntryType', str(col(pr, 'DocumentTypeDescription')).trim());
        dtdetail = [];
        renderGrid(REG);
        var obj = { Id: toInt(col(pr, 'Id')), DocumentTypeId: DocumentTypeId, ReqType: null };
        if (obj.DocumentTypeId === 66 || obj.DocumentTypeId === 808) {
            var et = val('txtEntryType').trim();
            if (et === 'Input' && obj.DocumentTypeId === 808) obj.ReqType = '1';
            else if (et === 'OutPut' && obj.DocumentTypeId === 808) obj.ReqType = '2';
            else obj.ReqType = et;
            var r = await getJson(API + '/id-by-reference?' + qs({ refDocumentTypeId: obj.DocumentTypeId, refDocNoId: obj.Id, refDocument: obj.ReqType }));
            RECID = toInt(r && r.id);
            if (RECID > 0) {
                setUpdateVisible(true);
                setSaveVisible(false);
                var head = await GetByID(RECID);
                if (!head) throw new Error(OOR);
                setVal('txtDocdate', isoDate(col(head, 'DocDate')));
                setVal('txtdocnumber', cellText(col(head, 'DocNo')));
                setVal('txtDocNo', cellText(col(head, 'RefDocNo')));
                setVal('txtGRNId', cellText(col(head, 'RefDocNoId')));
                cmbRef.enabled = false; paintCmbRef();
                setVal('txtRemarks', str(col(head, 'OtherRemarks')));
                setVal('txtGpNo', cellText(col(head, 'ScaleSlipNo')));
                setVal('txtEntryType', str(col(head, 'RefDocument')));
                $('chkisapprove').checked = toBool(col(head, 'IsAproved'));
            } else {
                setUpdateVisible(false);
                setSaveVisible(true);
            }
        }
        var dtGrnDetail = await getJson(API + '/reference-detail?' + qs({ id: obj.Id, documentTypeId: obj.DocumentTypeId, reqType: obj.ReqType }));
        dtGrnDetail = dtGrnDetail || [];
        if (dtGrnDetail.length > 0) {
            dtStiching = [];
            var focs = await postJson(API + '/free-of-cost', { rows: dtGrnDetail.map(function (g) {
                return { date: str(col(g, 'DocDate')), refDocumentTypeId: cmbValue(), itemId: toInt(col(g, 'ItemId')), wagesAccountId: 0 };
            }) });
            if (WagesAmountCalculateOnQty) {
                var GrossWeight = 0, Qty = 0;
                dtGrnDetail.forEach(function (g, i) {
                    IsRegularOrFreeOfCost = !!(focs && focs[i]);
                    var q = toDouble(col(g, 'Qty')), eq = toDouble(col(g, 'Equivalent'));
                    dtdetail.push(newRow([0, 0, 0, 0, IsRegularOrFreeOfCost ? 'Free Of Cost' : 'Regular', dMMMyyyy(col(g, 'DocDate')),
                        col(g, 'PackingTypeId'), col(g, 'PackTypeCode'), q * eq, col(g, 'Equivalent'), q, 0, 0, q * eq, 0, 0, 0, 0,
                        col(g, 'ItemId'), col(g, 'ItemName'), col(g, 'JlId'), col(g, 'JobLotDescription'), col(g, 'CropYear'),
                        col(g, 'WhFId'), col(g, 'WarehouseFrom'), col(g, 'WhTId'), col(g, 'WareHouseTo'), col(g, 'PurchaseGLAC'),
                        col(g, 'WarehouseType'), q, q * eq, i + 1]));
                    GrossWeight += toDouble(col(g, 'GrossWeight'));
                    Qty += q;
                });
                setVal('txtGrossWeight', netToString(GrossWeight));
                setVal('txtQty', netToString(Qty));
            } else {
                dtGrnDetail.forEach(function (g, j) {
                    IsRegularOrFreeOfCost = !!(focs && focs[j]);
                    var gw = toDouble(col(g, 'GrossWeight')), eq = toDouble(col(g, 'Equivalent'));
                    dtdetail.push(newRow([0, 0, 0, 0, IsRegularOrFreeOfCost ? 'Free Of Cost' : 'Regular', dMMMyyyy(col(g, 'DocDate')),
                        col(g, 'PackingTypeId'), col(g, 'PackTypeCode'), gw, col(g, 'Equivalent'), gw / eq, 0, 0, gw, 0, 0, 0, 0,
                        col(g, 'ItemId'), col(g, 'ItemName'), col(g, 'JlId'), col(g, 'JobLotDescription'), col(g, 'CropYear'),
                        col(g, 'WhFId'), col(g, 'WarehouseFrom'), col(g, 'WhTId'), col(g, 'WareHouseTo'), col(g, 'PurchaseGLAC'),
                        col(g, 'WarehouseType'), gw / eq, gw, j + 1]));
                });
            }
            if (RefDocTypeId === 112 || DocumentTypeId === 112 || RefDocTypeId === 66 || DocumentTypeId === 66) {
                var wagesId = '35';
                var m = wagesTypeRow('DocumentTypeId', DocumentTypeId);
                if (m) wagesId = str(col(m, 'WagesActivityIds'));
                dtStichingItems = await GetWagesAccount(wagesId);
                dtdetail.forEach(function (dr) { if (toDouble(dr.PackSize) < 100) dtStiching.push(copyRow(dr)); });
                if (dtStiching.length > 0) showTab('tabPage4', true);
            }
            if ((RefDocTypeId === 68 || RefDocTypeId === 806 || DocumentTypeId === 68 || DocumentTypeId === 806) && val('txtEntryType') === 'MoveOrder') {
                showTab('tabPage4', true);
                var wagesId2 = '34';
                var m2 = wagesTypeRow('DocumentTypeId', DocumentTypeId);
                if (m2) wagesId2 = str(col(m2, 'WagesActivityIds'));
                dtStichingItems = await GetWagesAccount(wagesId2);
                dtdetail.forEach(function (dr) { dtStiching.push(copyRow(dr)); });
            }
            await accountName(DocumentTypeId);
            setVal('txtDocdate', isoDate(dtdetail[0].Date));
            regBound = true; regRefLocked = false; regCur = dtdetail.length ? 0 : -1;
            renderGrid(REG);
            othBound = true; othRefLocked = false; othCur = dtStiching.length ? 0 : -1;
            renderGrid(OTH);
        }
        StichingWagesConfig();
    }

    /* ===================================================================== New / Refresh */
    /** grdDetailLockEditing():608 / GridStichingLockEditing():1753. */
    function grdDetailLockEditing() { if (regRows().length > 0) regRefLocked = IsReferred; renderGrid(REG); }
    function GridStichingLockEditing() { if (othRows().length > 0) othRefLocked = IsReferred; renderGrid(OTH); }

    /** FormRest():2459. */
    async function FormRest() {
        try {
            RECID = 0;
            PreviousContractorId = 0;
            setVal('txtDocNo', '');
            cmbRef.list = []; cmbRef.value = null;
            setVal('txtGpNo', '');
            setVal('txtRemarks', '');
            setVal('txtQty', '');
            setVal('txtGrossWeight', '');
            setVal('txtGRNId', '');
            await GenerateDocNo();
            dtdetail = [];
            regBound = false; regCur = -1;          // grdwagesDetail.ClearStructure() - grdStiching keeps its rows
            renderGrid(REG);
            setSaveVisible(true);
            setUpdateVisible(false);
            cmbRef.enabled = true;
            paintCmbRef();
            if (REF) await PendingGrnAndGdn(); else await PendingTicket();
            showTab('tabPage4', false);
            showTab('tabPage6', false);
            IsReferred = false;
            grdwagesDetailhaveDeletedRecords = false;
            GridStichingLockEditing();
            grdDetailLockEditing();
        } catch (e) { msg(e.message); }
    }

    /** ValidationOnformClose(ResetOrClose):2597. */
    async function ValidationOnformClose(ResetOrClose) {
        if (regRows().length > 0 && (retBound ? retrieveRows.length : 0) > 0 && !grdwagesDetailhaveDeletedRecords) {
            var CurrentQty = toDouble(val('txtQty'));
            var CurrentWeight = toDouble(val('txtGrossWeight'));
            var PreviousWeight = toDouble(retrieveRows[0].HeaderWeight);
            var PreviousQty = toDouble(retrieveRows[0].HeaderQty);
            var text = function (what) {
                return 'Current ' + what + ' Does not match with Previous ' + what + '\nIf You ' + ResetOrClose
                    + ' the Form Previous Saved Record Will Be Deleted\nAre you Sure to Do So???';
            };
            if (WagesAmountCalculateOnQty) {
                if (CurrentQty !== PreviousQty) {
                    if (!ask(text('Qty'))) return false;
                    await postJson(API + '/delete-by-reference', { refDocTypeId: RefDocTypeId, refDocId: RefDocId });
                }
            } else if (CurrentWeight !== PreviousWeight) {
                if (!ask(text('Weight'))) return false;
                /* :2626 - the weight branch passes RECID, not RefDocId */
                await postJson(API + '/delete-by-reference', { refDocTypeId: RefDocTypeId, refDocId: RECID });
            }
        }
        return true;
    }

    /** Close() -> frmwagesBillHeader_FormClosing:2549. */
    async function closeForm() {
        var ok;
        try { ok = await ValidationOnformClose('Close'); } catch (e) { msg(e.message); return false; }
        if (!ok) return false;
        CLOSED = true;
        if (EMBED) {
            try { if (window.parent.P280WagesClosed) window.parent.P280WagesClosed({ saved: SAVED }); } catch (e) { /* cross-origin */ }
        } else {
            K.close();
        }
        return true;
    }
    window.P280WagesRequestClose = function () { return after(closeForm); };

    /** btnRefresh_Click:2531. */
    async function btnRefresh_Click() {
        StichingWagesConfig();
        dtcus = (await getJson(API + '/contractors')) || [];
        await accountName();
        renderGrid(REG);                       // WagesItemAndContractorRefresh
        grdDetailLockEditing();
        GridStichingLockEditing();
    }

    /* ======================================================================== Save / Update */
    /** FormValidation():2568. */
    function FormValidation() {
        var dn = val('txtdocnumber').trim();
        if (dn === '' || dn === '0') { msg('document Number Field Required'); $('txtdocnumber').focus(); return false; }
        var s = $('cmbReferenceDocType');
        var text = s.selectedIndex >= 0 && s.options[s.selectedIndex] ? s.options[s.selectedIndex].textContent.trim() : '';
        if (text === '' || cmbValue() === 0) { msg('ReferenceDocType Field Required'); return false; }
        var d = val('txtDocNo').trim();
        if (d === '' || d === '0') { msg('Doc No Field Required'); return false; }
        var g = val('txtGRNId').trim();
        if (g === '' || g === '0') { msg('DocNoId Field Required'); return false; }
        return true;
    }
    function gridRowsForSave(rows, list, lookup) {
        return rows.map(function (r) {
            var o = copyRow(r);
            o.SupplierText = nameIn(contractorList(), r.SupplierId);
            o.WagesText = nameIn(accountList(list), r.WagesId);
            return o;
        });
    }
    /** Insert():2702. */
    async function Insert() {
        if (!FormValidation()) return;
        var cmb = cmbValue();
        if (!WagesAmountCalculateOnQty && (cmb === 68 || cmb === 806)) {
            var tot = 0; othRows().forEach(function (r) { tot += toDouble(r.Weight); });
            if (toDouble(netToString(tot)) !== toDouble(val('txtGrossWeight'))) {
                msg('OutPut Grid Total Weight and GrossWeight not equal please check!');
                return;
            }
        }
        if (RECID > 0) { if (!ask('Are you sure to Update?')) return; }
        else if (!ask('Are you sure to Save?')) return;
        var names = {};
        dtwagesacc.forEach(function (a) { names[String(toInt(col(a, 'Id')))] = str(col(a, 'WagesAccountName')); });
        var body = {
            recId: RECID,
            refDocTypeId: RefDocTypeId, refDocId: RefDocId,
            docNo: val('txtdocnumber'), docDate: val('txtDocdate'),
            refDocumentTypeId: cmb, refDocNoId: val('txtGRNId'), refDocNo: val('txtDocNo'),
            grossWeight: val('txtGrossWeight'), qty: val('txtQty'), remarks: val('txtRemarks'),
            gpNo: val('txtGpNo'), entryType: val('txtEntryType'), isApproved: $('chkisapprove').checked,
            regular: gridRowsForSave(regRows(), dtwagesacc),
            other: gridRowsForSave(othRows(), dtStichingItems),
            wagesAccountNames: names
        };
        var r;
        try { r = await postJson(API + '/save', body); }
        catch (e) { msg(e.message); return; }
        msg(r.message);
        SAVED = true;
        if (REF && RefDocTypeId !== 66) {
            if (await closeForm()) return;
        }
        await FormRest();
    }

    /* ============================================================================ history */
    function histCols() {
        return [
            { key: 'Edit', cap: 'Edit', w: 40, type: 'button', btn: 'Edit' },
            { key: 'Slip', cap: 'Slip', w: 40, type: 'button', btn: 'Slip' },
            { key: 'Voucher', cap: 'Voucher', w: 60, type: 'button', btn: 'Voucher' },
            { key: 'DocumentType', w: 150 },
            { key: 'DocDate', w: 95, fmt: dMMMyyyy },
            { key: 'DocNo', w: 70, align: 'ctr', type: 'link' },
            { key: 'RefDocNo', w: 70, align: 'ctr' },
            { key: 'QtyTotal', w: 80, align: 'num', fmt: fmtHash, total: 'sum' },
            { key: 'WeightTotal', w: 90, align: 'num', fmt: fmtHash, total: 'sum' },
            { key: 'WagesAmount', w: 100, align: 'num', fmt: fmtHash, total: 'sum' },
            { key: 'OtherRemarks', w: 250 },
            { key: 'JobOrderNo', w: 100 },
            { key: 'EntryDate', w: 140, fmt: dMMMyyyyhm },
            { key: 'EntryUser', w: 120 },
            { key: 'ModifyDate', w: 140, fmt: dMMMyyyyhm },
            { key: 'ModifyUser', w: 120 }
        ];
    }
    var HIS = {
        table: 'DataGridHistory', nav: 'navHistory', filter: true, frozen: 3,
        bound: function () { return historyBound; },
        cols: histCols, rows: function () { return historyBound ? historyRows : []; },
        cur: function () { return histCur; }, setCur: function (i) { histCur = i; },
        select: function (i) { after(function () { return HistoryDetailGridBind(toInt(historyRows[i].Id)); }); },
        dbl: function (i) { after(function () { return editFromHistory(i); }); },
        link: function (key, i) { after(function () { return editFromHistory(i); }); },
        button: function (key, i, b) {
            if (key === 'Voucher') return voucherPrint(i, b);
            if (key === 'Slip') return SlipPrint_002(toInt(historyRows[i].Id), b);
            if (key === 'Edit') after(function () { return editFromHistory(i); });
        }
    };
    function hdCols() {
        return [
            { key: 'ContractorName', w: 150 },
            { key: 'WagesAccount', cap: 'Labour / Wages Activity', w: 180 },
            { key: 'packingType', w: 80 },
            { key: 'Weight', w: 90, align: 'num', fmt: f00, total: 'sum' },
            { key: 'PackSize', w: 60 },
            { key: 'Quantity', w: 80, align: 'num', fmt: f00, total: 'sum' },
            { key: 'WeightCut', w: 70 },
            { key: 'BillWeight', w: 90, align: 'num', fmt: f00, total: 'sum' },
            { key: 'RateWithoutAddLess', w: 100, visible: EnableAddLessOnWagesRegular },
            { key: 'RateAddLess', w: 80, align: 'num', fmt: f2, total: 'sum', visible: EnableAddLessOnWagesRegular },
            { key: 'Rate', w: 80, align: 'num' },
            { key: 'Amount', w: 100, align: 'num', fmt: f00, total: 'sum' }
        ];
    }
    var HISD = {
        table: 'GrdHistoryDetail', nav: 'navHistoryDetail', filter: true,
        bound: function () { return historyDetailBound; },
        cols: hdCols, rows: function () { return historyDetailBound ? historyDetailRows : []; },
        cur: function () { return hdCur; }, setCur: function (i) { hdCur = i; }
    };

    /** bindHistory():3247. */
    async function bindHistory() {
        var p = {
            from: $('FromDateHistoryChk').checked ? val('FromDateHistory') : null,
            to: $('ToDateHistoryChk').checked ? val('ToDateHistory') : null,
            fromDocNo: val('FromDocNoHistory'), toDocNo: val('ToDocNoHistory'),
            refDocumentTypeId: toInt(val('CmbDocumentTypeHistory'))
        };
        var dt = await getJson(API + '/history?' + qs(p));
        if (dt && dt.length) {
            historyRows = dt.map(function (r) {
                return {
                    Id: col(r, 'Id'), DocumentTypeId: col(r, 'DocumentTypeId'), RefDocumentTypeId: col(r, 'RefDocumentTypeId'),
                    RefDocNoId: col(r, 'RefDocNoId'), DocumentType: col(r, 'DocumentTypeDescription'), DocDate: col(r, 'DocDate'),
                    DocNo: col(r, 'DocNo'), RefDocNo: col(r, 'RefDocNo'), QtyTotal: col(r, 'QtyTotal'), WeightTotal: col(r, 'WeightTotal'),
                    WagesAmount: col(r, 'WagesAmount'), OtherRemarks: col(r, 'OtherRemarks'), JobOrderId: col(r, 'JobOrderId'),
                    JobOrderNo: col(r, 'JobOrderNo'), EntryDate: col(r, 'EntryDate'), EntryUser: col(r, 'UserName'),
                    ModifyDate: col(r, 'ModifyDate'), ModifyUser: col(r, 'ModifyUser'), IsReferred: col(r, 'IsReferred')
                };
            });
            historyBound = true; histCur = 0;
            renderGrid(HIS);
            await HistoryDetailGridBind(toInt(historyRows[0].Id));   // SelectionChanged on bind
        } else {
            historyRows = []; historyBound = false; histCur = -1;
            renderGrid(HIS);
        }
    }
    /** HistoryDetailGridBind():3445. */
    var HistoryDetailGridBind = guarded(async function (Id) {
        var head = await GetByID(Id);
        if (!head) throw new Error(OOR);
        historyDetailRows = (col(head, 'invContractWagesBillDateil') || []).map(function (d) {
            var r = retrievalRow(head, d);
            return {
                HeaderId: r.HeaderId, ContractorName: r.ContractorName, WagesAccount: r.WagesAccount, packingType: r.packingType,
                Weight: r.Weight, PackSize: r.PackSize, Quantity: r.Quantity, WeightCut: r.WeightCut, BillWeight: r.BillWeight,
                RateWithoutAddLess: r.RateWithoutAddLess, RateAddLess: r.RateAddLess, Rate: r.Rate, Amount: r.Amount
            };
        });
        historyDetailBound = true; hdCur = historyDetailRows.length ? 0 : -1;
        renderGrid(HISD);
    });
    /** DataGridHistory_DoubleClick:3383 / "Edit":3426. */
    function editFromHistory(i) {
        var h = historyRows[i]; if (!h) return;
        IsReferred = toInt(h.IsReferred) === 1;
        return guarded(ReadById)(toInt(h.Id));
    }

    /** ReadById(ID):3054. */
    async function ReadById(ID) {
        RECID = ID;
        var head = await GetByID(RECID);
        if (!head) throw new Error(OOR);
        var det = col(head, 'invContractWagesBillDateil') || [];
        if (det.length <= 0) return;
        setSaveVisible(false);
        setUpdateVisible(true);
        UpdateMode = true;
        showTab('tabPage6', false);
        showTab('tabPage4', false);
        setMainTab(0);
        cmbRef.list = [{ Id: col(head, 'RefDocumentTypeId'), name: col(head, 'DocumentTypeDescription') }];
        cmbRef.value = col(head, 'RefDocumentTypeId');
        setVal('txtDocdate', isoDate(col(head, 'DocDate')));
        setVal('txtdocnumber', cellText(col(head, 'DocNo')));
        setVal('txtDocNo', cellText(col(head, 'RefDocNo')));
        setVal('txtGRNId', cellText(col(head, 'RefDocNoId')));
        cmbRef.enabled = false;
        paintCmbRef();
        setVal('txtQty', cellText(toDouble(col(head, 'QtyTotal'))));
        setVal('txtGrossWeight', cellText(toDouble(col(head, 'WeightTotal'))));
        setVal('txtRemarks', str(col(head, 'OtherRemarks')));
        setVal('txtGpNo', cellText(col(head, 'ScaleSlipNo')));
        setVal('txtEntryType', str(col(head, 'RefDocument')));
        $('chkisapprove').checked = toBool(col(head, 'IsAproved'));
        var refType = toInt(col(head, 'RefDocumentTypeId'));
        await accountName(refType);
        dtdetail = [];
        dtStiching = [];
        var GrossWeight = 0;
        var stichingLoaded = false;
        for (var i = 0; i < det.length; i++) {
            var d = det[i];
            var type2 = toInt(col(d, 'WagesTypeId')) === 2;
            if (type2 && !stichingLoaded) {
                var wagesId = '35';
                var m = wagesTypeRow('WagesTypeId', 2);
                if (m) wagesId = str(col(m, 'WagesActivityIds'));
                dtStichingItems = await GetWagesAccount(wagesId);
                stichingLoaded = true;
            }
            var values = [col(d, 'ContractorId'), col(d, 'CompanyName'), col(d, 'InvConractorWagesAccountsId'), col(d, 'WagesTypeId'),
                toBool(col(d, 'FreeOfCost')) ? 'Free Of Cost' : 'Regular', dMMMyyyy(col(d, 'RefDocDate')), col(d, 'InvPackingTypeId'),
                col(d, 'PackTypeDesc'), col(d, 'Weight'), col(d, 'PackSize'), col(d, 'Qty'), col(d, 'WeightCut'), col(d, 'BillQty'),
                col(d, 'BillWeight'), toDouble(col(d, 'WageRate')) - toDouble(col(d, 'RateAddLess')), col(d, 'RateAddLess'),
                col(d, 'WageRate'), col(d, 'WagesAmount'), col(d, 'ItemId'), col(d, 'ItemName'), col(d, 'JobLotId'),
                col(d, 'JobLotDescription'), col(d, 'Crop'), col(d, 'WareHouseFromId'), col(d, 'WareHouseFrom'), col(d, 'WareHouseToId'),
                col(d, 'WareHouseTo'), col(d, 'PurchaseGLAC'), type2 ? col(d, 'WarehouseType') : '', col(d, 'RefDocQty'),
                col(d, 'RefDocWeight'), col(d, 'RefLineId'), col(d, 'InvContractorWagesScheduleId')];
            if (type2) {
                dtStiching.push(newRow(values));
            } else {
                dtdetail.push(newRow(values));
                GrossWeight += toDouble(col(d, 'Weight'));
            }
        }
        setVal('txtGrossWeight', netToString(GrossWeight));
        regBound = true; regRefLocked = false; regCur = dtdetail.length ? 0 : -1;
        renderGrid(REG);
        var cmb = cmbValue();
        function fillFromDetail(checkPack) {
            dtdetail.forEach(function (r) {
                var exists = dtStiching.some(function (s) { return toInt(s.RefLineId) === toInt(r.RefLineId); });
                if (!exists && (!checkPack || toDouble(r.PackSize) < 100)) {
                    dtStiching.push(newRow([0, '', 0, '', r.WagesType, r.Date, r.packingTypeId, r.packingType, r.Weight, r.PackSize, r.Quantity,
                        r.WeightCut, r.BillQty, r.BillWeight, 0, 0, 0, 0, r.ItemId, r.Item, r.jobLotId, r.jobLot, r.Crop, r.MoveFromId,
                        r.MoveFrom, r.MoveToId, r.MoveTo, r.PurchaseGLAC, r.WarehouseType, r.RefDocQty, r.RefDocWeight, r.RefLineId]));
                }
            });
        }
        if (dtStiching.length === 0 && ((cmb === 66 && val('txtEntryType') !== 'Issue') || cmb === 112)) {
            var wagesId3 = '34';
            var m3 = wagesTypeRow('DocumentTypeId', refType);
            if (m3) wagesId3 = str(col(m3, 'WagesActivityIds'));
            dtStichingItems = await GetWagesAccount(wagesId3);
            fillFromDetail(true);
        } else if (dtStiching.length === 0 && (cmb === 68 || cmb === 806)) {
            var wagesId4 = '34';
            var m4 = wagesTypeRow('DocumentTypeId', refType);
            if (m4) wagesId4 = str(col(m4, 'WagesActivityIds'));
            dtStichingItems = await GetWagesAccount(wagesId4);
            fillFromDetail(false);
        }
        if (dtStiching.length > 0) showTab('tabPage4', true);
        othBound = true; othRefLocked = false; othCur = dtStiching.length ? 0 : -1;
        renderGrid(OTH);
        grdDetailLockEditing();
        GridStichingLockEditing();
        StichingWagesConfig();
    }

    /** ReadOnlyMasterById(ID):3185. */
    async function ReadOnlyMasterById(ID) {
        try {
            setSaveVisible(false);
            setUpdateVisible(true);
            RECID = ID;
            var head = await GetByID(RECID);
            setMainTab(0);
            if (head) {
                cmbRef.list = [{ Id: col(head, 'RefDocumentTypeId'), name: col(head, 'DocumentTypeDescription') }];
                cmbRef.value = col(head, 'RefDocumentTypeId');
            }
            if (!head) throw new Error(OOR);
            setVal('txtDocdate', isoDate(col(head, 'DocDate')));
            setVal('txtdocnumber', cellText(col(head, 'DocNo')));
            setVal('txtDocNo', cellText(col(head, 'RefDocNo')));
            setVal('txtGRNId', cellText(col(head, 'RefDocNoId')));
            cmbRef.enabled = false;
            paintCmbRef();
            setVal('txtGrossWeight', cellText(toDouble(col(head, 'WeightTotal'))));
            setVal('txtRemarks', str(col(head, 'OtherRemarks')));
            setVal('txtGpNo', cellText(col(head, 'ScaleSlipNo')));
            setVal('txtEntryType', str(col(head, 'RefDocument')));
            $('chkisapprove').checked = toBool(col(head, 'IsAproved'));
            await PendingGrnAndGdn();
            await guarded(LoadDataForWages)();
        } catch (e) { msg(e.message); }
    }

    /* ============================================================================== prints */
    /** SlipPrint_002(PrintId):3829 -> CommonServices.ContractorWagesBill_SlipandRegister_002. */
    function SlipPrint_002(PrintId, btn) {
        if (!(PrintId > 0)) { msg('No Record Found For Display'); return; }
        window.CrystalPrint.open('wages-002', { id: PrintId }, btn || null);
    }
    /** "Voucher":3420 - VoucherReport_118(VoucherHeadIdGet(Id, 101)). */
    function voucherPrint(i, b) {
        var win = window.CrystalPrint.reserve();
        return busy(b, function () {
            return getJson(API + '/voucher-head-id?' + qs({ id: toInt(historyRows[i].Id) })).then(function (r) {
                var vh = toInt(r && r.voucherHeadId);
                if (vh === 0) { window.CrystalPrint.release(win); msg('VoucherId Not Found'); return; }
                window.CrystalPrint.open('acc-118', { id: vh, documentTypeId: 0 }, null, win);
            }, function (e) { window.CrystalPrint.release(win); throw e; });
        });
    }

    /* ========================================================================= Cancel Records */
    /** BtnCancelPendingRecords_Click:4037. */
    async function BtnCancelPendingRecords_Click() {
        var rows = [];
        pendingRows.forEach(function (r, i) { if (pendingChecked[i]) rows.push({ documentTypeId: toInt(r.DocumentTypeId), id: toInt(r.Id) }); });
        if (rows.length === 0) { msg('Please select check box first'); return; }
        if (!ask('Are you sure to Cancel Selected Pending Records?')) return;
        var r = await postJson(API + '/cancel-pending', { rows: rows });
        msg(r.message);
        await FormRest();
    }

    /* ======================================================================== wiring */
    wireGrid(REG); wireGrid(OTH); wireGrid(PEND); wireGrid(RET); wireGrid(HIS); wireGrid(HISD);
    ['boxRegular', 'boxOther'].forEach(function (id) {
        $(id).addEventListener('focusin', function () { activeGridTag = $(id).getAttribute('data-grid-tag'); });
    });
    document.addEventListener('focusin', function (e) {
        if (!e.target.closest('#boxRegular, #boxOther')) activeGridTag = '';
    });
    Array.prototype.forEach.call(document.querySelectorAll('[data-full]'), function (b) {
        b.addEventListener('click', function () { $(b.getAttribute('data-full')).classList.toggle('is-fullscreen'); });
    });

    $('btnnew').addEventListener('click', function () {
        busy('btnnew', function () { return after(async function () { if (await ValidationOnformClose('Reset')) await FormRest(); }); });
    });
    $('btnRefresh').addEventListener('click', function () { busy('btnRefresh', function () { return after(btnRefresh_Click); }); });
    $('btnsave').addEventListener('click', function () {
        busy('btnsave', function () { return after(function () { RECID = 0; return Insert(); }); });
    });
    $('btnUpdate').addEventListener('click', function () { busy('btnUpdate', function () { return after(Insert); }); });
    $('Print').addEventListener('click', function () { SlipPrint_002(RECID, 'Print'); });
    $('btnWagesSchedule').addEventListener('click', function () {
        msg('Wages Schedule (frmContractWagesSchedule) is a separate screen. Open it from the Contractor Wages menu.');
    });
    $('btnWagesExempt').addEventListener('click', function () {
        msg('Wages Exempt Item (WagesExemptItemSchedule) is a separate screen. Open it from the Contractor Wages menu.');
    });
    $('btnAddContractorValuesForAllDetailWages').addEventListener('click', function () {
        busy('btnAddContractorValuesForAllDetailWages', function () { return after(guarded(function () { return AddContractorValuesForAllInGrid('reg'); })); });
    });
    $('btnAddContractorValuesForAllSticingWages').addEventListener('click', function () {
        busy('btnAddContractorValuesForAllSticingWages', function () { return after(guarded(function () { return AddContractorValuesForAllInGrid('oth'); })); });
    });
    $('BtnCancelPendingRecords').addEventListener('click', function () {
        busy('BtnCancelPendingRecords', function () { return after(BtnCancelPendingRecords_Click); });
    });
    $('btnShow').addEventListener('click', function () { busy('btnShow', function () { return after(bindHistory); }); });
    function btnResetHistory_Click() {
        setVal('FromDateHistory', today());
        setVal('ToDateHistory', today());
        setVal('FromDocNoHistory', '');
        setVal('ToDocNoHistory', '');
        setVal('CmbDocumentTypeHistory', '');
        historyRows = []; historyBound = false; histCur = -1; renderGrid(HIS);
        historyDetailRows = []; historyDetailBound = false; hdCur = -1; renderGrid(HISD);
    }
    $('btnResetHistory').addEventListener('click', btnResetHistory_Click);
    $('btnRefreshHistory').addEventListener('click', function () {
        busy('btnRefreshHistory', function () { return after(DocumentTypeFillForCombo); });
    });
    /* FromDocNoHistory_KeyPress / ToDocNoHistory_KeyPress - CommonServices.OnlytextNumberFunction */
    ['FromDocNoHistory', 'ToDocNoHistory'].forEach(function (id) {
        $(id).addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[0-9]/.test(e.key)) e.preventDefault(); });
        $(id).addEventListener('input', function () { var v = this.value.replace(/[^0-9]/g, ''); if (v !== this.value) this.value = v; });
    });
    $('btnCloseDialog').addEventListener('click', function () { after(closeForm); });

    /* frmwagesBillHeader_KeyDown:3706 (KeyPreview) */
    K.enterToTab();
    function formTab() { return mainIndex === 0; }
    K.keys({
        'ctrl+s': function () {
            if (formTab()) { if (TOOL.btnsave.visible && !$('btnsave').disabled) $('btnsave').click(); }
            else $('btnShow').click();
        },
        'ctrl+n': function () { if (formTab()) $('btnnew').click(); else btnResetHistory_Click(); },
        'ctrl+t': function () {
            if (mainIndex === 1) { mainTabChanged(0); try { $('cmbReferenceDocType').focus(); } catch (e) { } }
            else { mainTabChanged(1); if (mainIndex === 1) try { $('FromDateHistory').focus(); } catch (e) { } }
        },
        'ctrl+e': function () { if (!REF) after(closeForm); },
        'esc': function () { if (!REF) after(closeForm); },
        'ctrl+u': function () { if (TOOL.btnUpdate.visible && !$('btnUpdate').disabled) $('btnUpdate').click(); },
        'ctrl+p': function () { if (!$('Print').disabled) $('Print').click(); },
        'ctrl+d': function () {
            if (!formTab() || IsReferred) return;
            if (activeGridTag === 'WagesGrid') after(AddRowInGLGrid);
            if (activeGridTag === 'StichingGrid') after(AddRowInStichingGrid);
        },
        'ctrl+arrowdown': function () {
            var t = formTab() ? $('gridPendingWagesSlip') : $('DataGridHistory');
            var f = t.querySelector('tbody button, tbody input'); if (f) f.focus();
        },
        'ctrl+f5': function () { if (formTab()) try { $('txtDocdate').focus(); } catch (e) { } },
        'ctrl+l': function () {
            if (!formTab()) return;
            after(guarded(LoadDataForWages)).then(function () { var f = $('grdwagesDetail').querySelector('.cell-edit'); if (f) f.focus(); });
        },
        'ctrl+arrowup': function () {
            if (formTab()) { var f = $('grdwagesDetail').querySelector('.cell-edit'); if (f) f.focus(); }
            else try { $('FromDateHistory').focus(); } catch (e) { }
        }
    });

    /* ================================================================ frmwagesBillHeader_Load:321 */
    async function Load() {
        try {
            if (EMBED) $('wbCaption').classList.remove('is-hidden');
            setVal('FromDateHistory', today());
            setVal('ToDateHistory', today());
            var s = await getJson(API + '/state?' + qs({ refDocTypeId: RefDocTypeId, refDocId: RefDocId }));
            CFG = s.config || {};
            EnableAddLessOnWagesRegular = toBool(CFG.EnableAddLessOnWagesRegular);
            PercentageForRateAddLess = toDouble(CFG.PercentageForRateAddLess);
            CmpGRN = toBool(CFG.ContractorWageComparisonbyActivityForGRN);
            CmpGDN = toBool(CFG.ContractorWageComparisonbyActivityForGDN);
            CmpFwd = toBool(CFG.ContractorWageComparisonbyActivityForForwarding);
            CmpST = toBool(CFG.ContractorWageComparisonbyActivityForStockTransfer);
            CmpIn = toBool(CFG.ContractorWageComparisonbyActivityForProductionInput);
            CmpCons = toBool(CFG.ContractorWageComparisonbyActivityForProductionConsumption);
            showTab('tabPage4', false);
            showTab('tabPage6', false);
            if (RefDocTypeId === 112 || RefDocTypeId === 66) showTab('tabPage4', true);
            RIGHTS = s.rights || RIGHTS;
            IS_ADMIN = !!s.isAdmin;
            paintTools();
            MultiBranchFeatur = !!s.multiBranch;
            dtWagesTypeIdsAgainstDocumentType = s.wagesTypeIds || [];
            WagesAmountCalculateOnQty = toBool(CFG.WagesAmountCalculateOnQty);
            dtcus = s.contractors || [];
            await accountName();
            if (toInt(s.docNo) > 0) setVal('txtdocnumber', String(toInt(s.docNo)));
            if (REF) {
                RECID = toInt(s.recId);
                if (RECID > 0) {
                    await ReadOnlyMasterById(RECID);
                    if (GrossWeightTotal > 0) setVal('txtGrossWeight', netToString(GrossWeightTotal));
                    if (RefDocTypeId !== 66) showTab('tabPage5', false);
                    showTab('tabPage6', true);
                    await RetreivalHistoryDetailGridBind(RECID);
                } else {
                    await PendingGrnAndGdn();
                    await guarded(LoadDataForWages)();
                    if (await RetreivalDetailGridDeletedData()) {
                        if (GrossWeightTotal > 0) setVal('txtGrossWeight', netToString(GrossWeightTotal));
                        if (RefDocTypeId !== 66) showTab('tabPage5', false);
                        showTab('tabPage6', true);
                    }
                }
            } else {
                await PendingTicket();
            }
            await DocumentTypeFillForCombo();
            StichingWagesConfig();
            $('BtnCancelPendingRecords').classList.toggle('is-hidden', !(IS_ADMIN && !REF));
        } catch (e) {
            msg(e && e.message ? e.message : String(e));
        }
        paintCmbRef();
        renderGrid(REG); renderGrid(OTH); renderGrid(RET);
    }

    paintTabs();
    setMainTab(0);
    paintTools();
    after(Load);

    /* Opened from a register link: CommonServices.EditMethodFromLinked(101, Id, ...) (:3328) does
       new frmwagesBillHeader(userAccount); Show() - Load runs - then ReadById(num), num =
       DocumentTypeIdSrNo > 0 ? DocumentTypeIdSrNo : Id. Here that arrives as ?id= (DocLink). The
       history-grid Edit's IsReferred assignment is NOT part of that path, so IsReferred keeps its
       field default. ReadById's own try/catch -> MessageBox is guarded(). */
    (function () {
        var linkedId = 0;
        try { linkedId = toInt(new URLSearchParams(window.location.search).get('id')); } catch (e) { linkedId = 0; }
        if (linkedId > 0) after(function () { return guarded(ReadById)(linkedId); });
    }());
}());
