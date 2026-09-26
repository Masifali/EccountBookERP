/* ============================================================================================
 * "Wages Register" - Architecture.WinApp.Inventory_Reports/frmEvaulationDetailWagesReports.cs (2,351 lines).
 *
 * Screen 280 opens it from the Settlement tab's Over Heads grid (frmProductionSettlement
 * .grdOverHeadSettlement_LinkClicked:972, DocumentTypeId 80 / 112):
 *     obj2 = new frmEvaulationDetailWagesReports(UserAccount);
 *     obj2.Show();                     -> Load runs (below)
 *     obj2.CmbJobOrderNo.Value = Id;   -> ?jobOrderId=Id
 *     obj2.GridFill();
 * The page replays exactly that after its own Load.
 *
 * Line numbers below are frmEvaulationDetailWagesReports.cs's. Each server call is one desktop
 * method; the per-activity projection of dtGrid into the grid tables runs here as it runs in the form.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/production/reports/evaluation-detail-wages';
    var K = window.ReportKit;

    /* ---------------------------------------------------------------- form fields */
    var IsPartyProcessingFeatureOn = false;
    var BranchFeature = false;
    var dtBranch = [];
    var branchText = '';            /* CmbBranchName.Text */
    var branchChecked = {};         /* the "Selected" column */
    var branchActiveRow = null;     /* CmbBranchName.ActiveRow */
    var fyStart = null;
    var userBranchName = '';        /* UserAccount.BranchName */
    var fmt = { amount: 0, rate: 2 };
    var dtGridCount = 0;            /* dtGrid.Rows.Count (cleared at the top of every GridFill) */
    var lastPrintArgs = null;       /* the parameters behind dtGrid */
    var printText = '&Print';       /* btnPrint.Text */
    var gridTable = null;           /* { cols, rows, group, register } - DataGridHistory's structure, null = cleared */
    var fromTime = timePart(nowIso()), toTime = timePart(nowIso());
    var cur = -1;

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
    function toD(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return v;
        var n = Number(String(v).trim().replace(/,/g, ''));
        return isNaN(n) ? 0 : n;
    }
    function toI(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isFinite(v) ? Math.round(v) : 0;
        var s = String(v).trim();
        return /^[+-]?\d+$/.test(s) ? parseInt(s, 10) : 0;
    }
    function str15(x) {
        if (x === null || x === undefined || x === '') return '';
        var n = typeof x === 'number' ? x : Number(x);
        if (isNaN(n)) return String(x);
        return String(Number(n.toPrecision(15)));
    }
    function cssEsc(v) { return window.CSS && CSS.escape ? CSS.escape(v) : String(v).replace(/"/g, '\\"'); }

    /* ------------------------------------------------------------------ MessageBox */
    function msg(text) {
        return new Promise(function (resolve) {
            var m = document.createElement('div');
            m.className = 'wr-modal';
            m.innerHTML = '<div class="wr-dlg" role="dialog"><div class="cap"></div><div class="msg">' + esc(text)
                + '</div><div class="btns"><button type="button">OK</button></div></div>';
            document.body.appendChild(m);
            var b = m.querySelector('button');
            b.focus();
            function done() { document.removeEventListener('keydown', key, true); m.remove(); resolve(); }
            function key(e) { if (e.key === 'Escape') { e.preventDefault(); e.stopPropagation(); done(); } }
            document.addEventListener('keydown', key, true);
            b.addEventListener('click', done);
        });
    }
    function modalOpen() { return !!document.querySelector('.wr-modal, .rk-modal.is-open'); }

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
        /* DDL.BindDDL(dtJob, CmbJobOrderNo, "Id", "JobOrderNo", "Job Order No"): Id hidden, the
           procedure's third column (RefDocumentTypeId) stays visible. */
        window.DesktopCombo.define('evalJobOrder', [
            { caption: 'Job Order No',      flex: 3 },
            { caption: 'RefDocumentTypeId', flex: 2, key: 'refdoc', type: 'num' }
        ]);
    }
    /** DDL.BindDDL(dt, combo, "Id", "name", caption, ZeroIndex:false) - a fresh bind drops the old value. */
    function bind(sel, rows, idKey, textKey, extra) {
        var h = '<option value=""></option>';
        (rows || []).forEach(function (r) {
            h += '<option value="' + esc(col(r, idKey)) + '"' + (extra ? extra(r) : '') + '>' + esc(col(r, textKey)) + '</option>';
        });
        sel.innerHTML = h;
        sel.value = '';
    }
    /** Text = ""; DataSource = null. */
    function unbind(sel) { sel.innerHTML = '<option value=""></option>'; sel.value = ''; }
    function setValue(sel, v) {
        var s = String(v === null || v === undefined ? '' : v);
        Array.prototype.forEach.call(sel.querySelectorAll('option[data-orphan]'), function (o) { o.remove(); });
        if (s === '' || s === '0') { sel.value = ''; return; }
        if (!sel.querySelector('option[value="' + cssEsc(s) + '"]')) {
            var o = document.createElement('option');
            o.value = s; o.textContent = s; o.setAttribute('data-orphan', '1');
            sel.appendChild(o);
        }
        sel.value = s;
    }
    function activeRow(sel) {
        var o = sel.options[sel.selectedIndex];
        return o && o.value !== '' && !o.hasAttribute('data-orphan') ? o : null;
    }
    function comboText(sel) {
        var o = sel.options[sel.selectedIndex];
        return o && o.value !== '' ? o.textContent : '';
    }

    /* ------------------------------------------------------------ CmbBranchName (checked list) */
    function renderBranchPop() {
        var pop = $id('CmbBranchNamePop');
        var all = dtBranch.length > 0 && dtBranch.every(function (r) { return branchChecked[String(col(r, 'BranchId'))]; });
        pop.innerHTML = '<table><thead><tr><th><input type="checkbox" data-all' + (all ? ' checked' : '') + '></th><th>Branch Name</th></tr></thead><tbody>'
            + dtBranch.map(function (r, i) {
                var id = String(col(r, 'BranchId'));
                return '<tr data-i="' + i + '"' + (branchActiveRow === i ? ' class="act"' : '') + '><td><input type="checkbox" data-id="' + esc(id) + '"'
                    + (branchChecked[id] ? ' checked' : '') + '></td><td>' + esc(col(r, 'BranchName')) + '</td></tr>';
            }).join('') + '</tbody></table>';
    }
    function branchTextFromChecks() {
        branchText = dtBranch.filter(function (r) { return branchChecked[String(col(r, 'BranchId'))]; })
            .map(function (r) { return String(col(r, 'BranchName')); }).join(',');
        $id('CmbBranchNameText').value = branchText;
    }
    function wireBranchCombo() {
        var box = $id('CmbBranchName'), pop = $id('CmbBranchNamePop'), txt = $id('CmbBranchNameText');
        box.querySelector('.bc-drop').addEventListener('click', function () {
            if (pop.classList.contains('is-hidden')) { renderBranchPop(); pop.classList.remove('is-hidden'); }
            else pop.classList.add('is-hidden');
            txt.focus();
        });
        txt.addEventListener('input', function () { branchText = txt.value; });
        txt.addEventListener('keydown', function (e) {
            if (e.altKey && e.key === 'ArrowDown') { e.preventDefault(); renderBranchPop(); pop.classList.remove('is-hidden'); }
        });
        pop.addEventListener('mousedown', function (e) { e.preventDefault(); });   /* keep focus in the combo */
        pop.addEventListener('click', function (e) {
            var all = e.target.closest('input[data-all]');
            if (all) {
                dtBranch.forEach(function (r) { branchChecked[String(col(r, 'BranchId'))] = all.checked; });
                branchTextFromChecks(); renderBranchPop(); return;
            }
            var tr = e.target.closest('tbody tr');
            if (!tr) return;
            var i = toI(tr.getAttribute('data-i'));
            branchActiveRow = i;
            var id = String(col(dtBranch[i], 'BranchId'));
            var cb = e.target.closest('input[data-id]');
            branchChecked[id] = cb ? cb.checked : !branchChecked[id];
            branchTextFromChecks();
            renderBranchPop();
        });
        /* CmbBranchName_Leave:437 */
        box.addEventListener('focusout', function () {
            setTimeout(function () {
                if (box.contains(document.activeElement)) return;
                pop.classList.add('is-hidden');
                CmbBranchName_Leave();
            }, 0);
        });
    }

    /* ================================================== frmEvaulationDetailSalesReports_Load:151 */
    function Load() {
        return getJson(API + '/load').then(function (d) {
            IsPartyProcessingFeatureOn = !!d.partyProcessing;
            ['lblrdIsCompany', 'lblrdIsThirdParty', 'lblrdAll'].forEach(function (id) {
                $id(id).classList.toggle('is-hidden', !IsPartyProcessingFeatureOn);
            });
            BranchFeature = !!d.branchFeature;
            $id('lblchkBranchWise').classList.toggle('is-hidden', !BranchFeature);
            fyStart = d.financialYearStart;
            if (d.amountDecimals !== undefined) fmt.amount = d.amountDecimals;
            if (d.rateDecimals !== undefined) fmt.rate = d.rateDecimals;
            userBranchName = d.userBranchName || '';
            BranchesFill(d.branches, userBranchName);
            return ComboFill();
        }).then(JobOrderFill).then(function () {
            WagesTypesFill();
            ActivityFill();
            return GridFill();
        }).then(function () {
            if (fyStart) { $id('txtDateFrom').value = String(fyStart).substring(0, 10); fromTime = timePart(String(fyStart).length >= 19 ? String(fyStart) : ''); }
            PrintButtonManage();
        }).catch(function (e) { return msg(e.message); });
    }

    /** BranchesFill:250 - the ticks start empty; Text is the user's own branch name. */
    function BranchesFill(rows, userBranchName) {
        dtBranch = [];
        branchChecked = {};
        branchActiveRow = null;
        if (rows && rows.length > 0) {
            dtBranch = rows;
            branchText = userBranchName || '';
            $id('CmbBranchNameText').value = branchText;
        }
    }

    /** ComboFill:280 - throws "Select Branch First" when the branch text is empty. */
    function ComboFill() {
        if (branchText === '') {
            $id('CmbBranchNameText').focus();
            return Promise.reject(new Error('Select Branch First'));
        }
        return postJson(API + '/combos', { branchText: branchText }).then(function (d) {
            if (!d || !d.bound) return;
            bind($id('cmbDocumentType'), d.documentTypes, 'Id', 'name');
            bind($id('CmbWagesAccount'), d.wagesAccounts, 'Id', 'name');
            bind($id('CmbContractorName'), d.contractors, 'Id', 'name');
            bind($id('CmbStockParty'), d.stockParties, 'Id', 'name');
            bind($id('CmbWagesVoucherAccount'), d.debitAccounts, 'Id', 'name');
        });
    }

    /** JobOrderFill:363 */
    function JobOrderFill() {
        if (branchText === '') {
            $id('CmbBranchNameText').focus();
            return Promise.reject(new Error('Select Branch First'));
        }
        return postJson(API + '/job-orders', { branchText: branchText }).then(function (d) {
            if (!d || !d.bound) return;
            bind($id('CmbJobOrderNo'), d.rows, 'Id', 'JobOrderNo', function (r) {
                return ' data-refdoc="' + esc(col(r, 'RefDocumentTypeId')) + '"';
            });
        });
    }

    /** WagesTypesFill:399 - hard-coded on the desktop too. */
    function WagesTypesFill() {
        bind($id('cmbWagesType'), [{ Id: 1, WagesType: 'Regular' }, { Id: 2, WagesType: 'Free of Cost' }], 'Id', 'WagesType');
    }
    /** ActivityFill:416 - hard-coded; Rows[0].Activate(). */
    var ACTIVITIES = ['Contractor Wages Register', 'Summary By Contractor and Document Type', 'Summary By Document Type',
                      'Summary By Contractor', 'Summary By Contractor And JobOrder'];
    function ActivityFill() {
        bind($id('cmbActivity'), ACTIVITIES.map(function (n, i) { return { Id: i + 1, Name: n }; }), 'Id', 'Name');
        $id('cmbActivity').value = '1';
    }

    /** CmbBranchName_Leave:437 */
    function CmbBranchName_Leave() {
        if (branchActiveRow !== null) {
            return ComboFill().then(JobOrderFill).catch(function (e) { return msg(e.message); });
        }
        unbind($id('cmbDocumentType'));
        unbind($id('CmbWagesAccount'));
        unbind($id('CmbContractorName'));
        unbind($id('CmbStockParty'));
        unbind($id('CmbJobOrderNo'));
    }

    /* ================================================================================ GridFill:482 */
    function dateArg(id, time) { var v = $id(id).value; return v ? v + time : null; }
    function GridFill() {
        dtGridCount = 0;                                   /* dtGrid.Rows.Clear() */
        var jo = $id('CmbJobOrderNo');
        var body = {
            fromDate: dateArg('txtDateFrom', fromTime),
            toDate: dateArg('txtToDate', toTime),
            documentTypeId: toI($id('cmbDocumentType').value),
            activity: comboText($id('cmbActivity')),
            wagesType: toI($id('cmbWagesType').value),
            party: (document.querySelector('input[name="party"]:checked') || {}).value || 'all',
            branchWise: $id('chkBranchWise').checked,
            contractorId: toI($id('CmbContractorName').value),
            wagesAccountId: toI($id('CmbWagesAccount').value),
            stockPartyId: toI($id('CmbStockParty').value),
            jobOrderId: toI(jo.value),
            debitAccountId: toI($id('CmbWagesVoucherAccount').value),
            branchText: branchText
        };
        if (body.jobOrderId !== 0) {
            /* CmbJobOrderNo.SelectedRow.Cells["RefDocumentTypeId"] - a Value with no row is a null SelectedRow. */
            var row = activeRow(jo);
            if (!row) return msg('Object reference not set to an instance of an object.');
            body.jobOrderRefDocumentTypeId = toI(row.getAttribute('data-refdoc'));
        }
        if (branchText === '') {
            $id('CmbBranchNameText').focus();
            return msg('Select Branch First');
        }
        return postJson(API + '/grid', body).then(function (d) {
            var rows = (d && d.rows) || [];
            dtGridCount = rows.length;
            lastPrintArgs = d ? d.printArgs : null;
            if (rows.length > 0 && activeRow($id('cmbActivity'))) project(comboText($id('cmbActivity')), rows);
            else gridTable = null;                          /* ClearStructure() */
            cur = -1;
            renderGrid();
            say(rows.length ? rows.length + ' record(s)' : '');
        }).catch(function (e) { return msg(e.message); });
    }

    /* ---- the per-activity tables (GridFill:553-640) and DataGridHistorySetting:662 */
    /* Constants.InventoryConstants widths are not in the recovered source; these follow
       GridColumnSettings' own defaults (int 60, date 80, double 90, name 150). */
    var W = { Date: 80, DocNo: 60, Qty: 90, Weight: 100, ExRate: 90, Amount: 100, Branch: 120, Uom: 60, DocumentType: 120 };
    function project(activity, g) {
        var t = null;
        var d = function (k) { return { k: k, t: 'dbl' }; };
        if (activity === 'Contractor Wages Register') {
            t = { register: true, group: 'WagesAccount', cols: [
                { k: 'Id', t: 'int', hide: true }, { k: 'DocumentTypeId', t: 'int', hide: true },
                { k: 'DocDate', t: 'date', w: W.Date }, { k: 'DocNo', t: 'int', w: W.DocNo, link: true },
                { k: 'DocumentType', w: 80 }, { k: 'RefDocumentTypeId', t: 'int', hide: true },
                { k: 'RefDocumentType', w: 110, hide: true, link: true }, { k: 'RefDocNo', w: 53, link: true },
                { k: 'WagesAccount', w: 250 }, { k: 'PackSize', t: 'dbl', w: 50 }, d('WageRate'), d('Qty'), d('Weight'),
                d('WeightCut'), d('WagesAmount'), { k: 'WagesPer40Kg', t: 'dbl', w: 70, cap: 'Wages / 40Kg', raw: true },
                { k: 'JobOrder', w: 110 }, { k: 'ItemName', w: 110 }, { k: 'Crop', w: 60 }, { k: 'JobLot', w: 70 },
                { k: 'ContractorName', w: 110 }, d('BillQty'), d('BillWeight'), { k: 'RefDocId', t: 'int', hide: true },
                { k: 'BranchName' }, { k: 'BaseDocumentTypeId', t: 'int', hide: true }
            ], rows: g.map(function (r) {
                var dd = col(r, 'DocDate');
                return { Id: col(r, 'Id'), DocumentTypeId: col(r, 'DocumentTypeId'), DocDate: dd ? String(dd).substring(0, 10) : dd,
                         DocNo: col(r, 'DocNo'), DocumentType: col(r, 'DocumentTypeDescription'),
                         RefDocumentTypeId: col(r, 'RefDocumentTypeId'), RefDocumentType: col(r, 'RefDocumentTypeDescription'),
                         RefDocNo: col(r, 'RefDocNo'), WagesAccount: col(r, 'WagesAccountName'), PackSize: col(r, 'PackSize'),
                         WageRate: col(r, 'WageRate'), Qty: col(r, 'Qty'), Weight: col(r, 'Weight'), WeightCut: col(r, 'Weightcut'),
                         WagesAmount: col(r, 'WagesAmount'), WagesPer40Kg: col(r, 'WagesPer40Kg'), JobOrder: col(r, 'JobOrderNo'),
                         ItemName: col(r, 'ItemName'), Crop: col(r, 'Crop'), JobLot: col(r, 'JobLot'),
                         ContractorName: col(r, 'ContractorName'), BillQty: col(r, 'BillQty'), BillWeight: col(r, 'BillWeight'),
                         RefDocId: col(r, 'RefDocId'), BranchName: col(r, 'BranchName'), BaseDocumentTypeId: col(r, 'BaseDocumentTypeId') };
            }) };
        } else if (activity === 'Summary By Contractor') {
            t = { cols: [{ k: 'ContractorName', w: 250 }, d('Qty'), d('BillQty'), d('Weight'), d('WeightCut'), d('BillWeight'),
                         d('WageRate'), d('WagesAmount'), { k: 'BranchName' }],
                  rows: g.map(function (r) { return pick(r, ['ContractorName', 'Qty', 'BillQty', 'Weight', 'WeightCut', 'BillWeight', 'WageRate', 'WagesAmount', 'BranchName']); }) };
        } else if (activity === 'Summary By Document Type') {
            t = { cols: [{ k: 'DocumentType', w: 150 }, { k: 'PackSize', w: W.Uom }, d('Qty'), d('BillQty'), d('Weight'), d('WeightCut'),
                         d('BillWeight'), d('WageRate'), d('WagesAmount'), { k: 'BranchName' }],
                  rows: g.map(function (r) {
                      var o = pick(r, ['PackSize', 'Qty', 'BillQty', 'Weight', 'WeightCut', 'BillWeight', 'WageRate', 'WagesAmount', 'BranchName']);
                      o.DocumentType = col(r, 'DocumentTypeDescription');
                      o.PackSize = o.PackSize === null || o.PackSize === undefined ? '' : str15(o.PackSize);   /* untyped column: text */
                      return o;
                  }) };
        } else if (activity === 'Summary By Contractor and Document Type') {
            t = { cols: [{ k: 'ContractorName', w: 120 }, { k: 'DocumentType', w: W.DocumentType }, { k: 'PackSize', w: W.Uom },
                         d('Qty'), d('BillQty'), d('Weight'), d('WeightCut'), d('BillWeight'), d('WageRate'), d('WagesAmount'), { k: 'BranchName' }],
                  rows: g.map(function (r) {
                      var o = pick(r, ['ContractorName', 'PackSize', 'Qty', 'BillQty', 'Weight', 'WeightCut', 'BillWeight', 'WageRate', 'WagesAmount', 'BranchName']);
                      o.DocumentType = col(r, 'DocumentTypeDescription');
                      o.PackSize = o.PackSize === null || o.PackSize === undefined ? '' : str15(o.PackSize);
                      return o;
                  }) };
        } else if (activity === 'Summary By Contractor And JobOrder') {
            t = { cols: [{ k: 'ContractorName', w: 180 }, { k: 'JobOrder', w: 100 }, d('Qty'), d('BillQty'), d('Weight'), d('WeightCut'),
                         d('BillWeight'), d('WageRate'), d('WagesAmount'), { k: 'BranchName' }],
                  rows: g.map(function (r) {
                      var o = pick(r, ['ContractorName', 'Qty', 'BillQty', 'Weight', 'WeightCut', 'BillWeight', 'WageRate', 'WagesAmount', 'BranchName']);
                      o.JobOrder = col(r, 'JobOrderNo');
                      return o;
                  }) };
        }
        if (!t) { return; }               /* an activity text outside the five: the grid keeps what it had */
        /* DataGridHistorySetting: BillQty / BillWeight hidden, fixed widths, Link columns and the
           Print button (register only). */
        t.cols.forEach(function (c) {
            if (c.k === 'BillQty' || c.k === 'BillWeight') c.hide = true;
            if (c.k === 'Qty') c.w = W.Qty;
            if (c.k === 'Weight') c.w = W.Weight - 10;
            if (c.k === 'WeightCut') c.w = 65;
            if (c.k === 'WageRate') c.w = W.ExRate;
            if (c.k === 'WagesAmount') c.w = W.Amount;
            if (c.k === 'BranchName') c.w = W.Branch;
            if (!c.w) c.w = c.t === 'int' ? 60 : c.t === 'date' ? 80 : c.t === 'dbl' ? 90 : /name|description/i.test(c.k) ? 150 : 100;
        });
        gridTable = t;
    }
    function pick(r, keys) { var o = {}; keys.forEach(function (k) { o[k] = col(r, k); }); return o; }

    /** GridColumnSettings(col, 2): Amount -> stringFormatsingle (Sum), Rate -> DecimalRateFormate,
        other doubles "#,##0.##" (Sum); WagesPer40Kg is excluded from the settings. */
    function fmtCell(c, v) {
        if (v === null || v === undefined || v === '') return '';
        if (c.t === 'date') return K.dMMMyy(v);
        if (c.t !== 'dbl') return String(v);
        if (c.raw) return str15(v);
        if (c.k.indexOf('Amount') >= 0) return K.fixed(toD(v), fmt.amount);
        if (c.k.indexOf('Rate') >= 0) return K.fixed(toD(v), fmt.rate);
        return K.num(v, 2);
    }
    function sums(c) { return c.t === 'dbl' && !c.raw && c.k.indexOf('Rate') < 0; }
    function renderGrid() {
        var tb = $id('DataGridHistory');
        if (!gridTable) { tb.innerHTML = ''; return; }
        var cols = gridTable.cols.filter(function (c) { return !c.hide; });
        var pr = !!gridTable.register;
        var h = '<colgroup>' + (pr ? '<col style="width:45px">' : '') + cols.map(function (c) { return '<col style="width:' + c.w + 'px">'; }).join('')
            + '</colgroup><thead><tr>' + (pr ? '<th></th>' : '') + cols.map(function (c) { return '<th>' + esc(c.cap || c.k) + '</th>'; }).join('')
            + '</tr></thead><tbody>';
        var n = cols.length + (pr ? 1 : 0);
        function line(r) {
            var i = gridTable.rows.indexOf(r);
            return '<tr data-i="' + i + '"' + (i === cur ? ' class="cur"' : '') + '>' + (pr ? '<td><button type="button" class="cb" data-print>Print</button></td>' : '')
                + cols.map(function (c) {
                    var t = esc(fmtCell(c, r[c.k]));
                    if (c.link && t && pr) t = '<a class="lnk" data-link="' + c.k + '">' + t + '</a>';
                    var cls = c.t === 'dbl' ? ' class="num"' : c.t === 'int' ? ' class="ctr"' : '';
                    return '<td' + cls + ' title="' + esc(fmtCell(c, r[c.k])) + '">' + t + '</td>';
                }).join('') + '</tr>';
        }
        if (gridTable.group) {
            /* RootTable.Groups.Add("WagesAccount"): every row of a value under one heading, headings ascending. */
            var order = [], buckets = {};
            gridTable.rows.forEach(function (r) {
                var k = r[gridTable.group] === null || r[gridTable.group] === undefined ? '' : String(r[gridTable.group]);
                if (!Object.prototype.hasOwnProperty.call(buckets, k)) { buckets[k] = []; order.push(k); }
                buckets[k].push(r);
            });
            order.sort(function (a, b) { return a.toLowerCase() < b.toLowerCase() ? -1 : a.toLowerCase() > b.toLowerCase() ? 1 : 0; });
            order.forEach(function (k) {
                h += '<tr class="grp"><td colspan="' + n + '">WagesAccount: ' + esc(k) + ' (' + buckets[k].length + ')</td></tr>';
                buckets[k].forEach(function (r) { h += line(r); });
            });
        } else {
            gridTable.rows.forEach(function (r) { h += line(r); });
        }
        h += '</tbody><tfoot><tr>' + (pr ? '<td></td>' : '') + cols.map(function (c) {
            if (!sums(c)) return '<td></td>';
            var s = 0;
            gridTable.rows.forEach(function (r) { s += toD(r[c.k]); });
            return '<td class="num">' + esc(fmtCell(c, s)) + '</td>';
        }).join('') + '</tr></tfoot>';
        tb.innerHTML = h;
    }

    /* ========================================================================== PrintButtonManage:884 */
    var PRINT = {
        'Contractor Wages Register': '160-WagesRegister',
        'Summary By Contractor and Document Type': '161-WagesbyContractor&DocumentType',
        'Summary By Contractor': '162-WagesByContractor',
        'Summary By Document Type': '163-WagesByDocumentType',
        'Summary By Contractor And JobOrder': '164-WagesSummaryByContractorAndJobOrder'
    };
    var PRINT_KEY = {
        '160-WagesRegister': 'wr-160', '161-WagesbyContractor&DocumentType': 'wr-161', '162-WagesByContractor': 'wr-162',
        '163-WagesByDocumentType': 'wr-163', '164-WagesSummaryByContractorAndJobOrder': 'wr-164'
    };
    function PrintButtonManage() {
        if (toI($id('cmbActivity').value) > 0) {
            var t = PRINT[comboText($id('cmbActivity'))];
            if (t) { printText = t; $id('btnPrint').textContent = t; }
        } else {
            $id('btnPrint').classList.add('is-hidden');      /* never shown again */
        }
    }
    /** btnPrint_Click:813 - the report named by the button's text, over the dtGrid of the last Show. */
    function btnPrint_Click() {
        var key = PRINT_KEY[printText];
        if (!key) return;
        if (dtGridCount === 0) return msg('Record Not Found For Display');
        return window.CrystalPrint.open(key, lastPrintArgs || {}, 'btnPrint');
    }

    /* ============================================================================ grid buttons / links */
    /** DataGridHistory_ColumnButtonClick:697 - CommonServices.ContractorWagesBill_SlipandRegister_002(Id). */
    function printSlip(r, btn) {
        var id = toI(r.Id);
        if (!(id > 0)) return msg('No Record Found For Display');
        return window.CrystalPrint.open('wages-002', { id: id }, btn);
    }
    /**
     * DataGridHistory_LinkClicked:725 -> CommonServices.EditMethodFromLinked (the desktop's one
     * dispatcher; web: countx_doc_link.js, window.DocLink):
     *   DocNo    -> EditMethodFromLinked(DocumentTypeId, Id, Id)                                   (:743)
     *   RefDocNo -> EditMethodFromLinked(RefDocumentTypeId, RefDocId, RefDocId, BaseDocumentTypeId) (:754)
     *               only when RefDocId > 0
     * DocumentTypeId is the row's own cell (101 for this register's rows). RefDocumentTypeId is never
     * passed as the dispatcher's 5th argument, so the 401 (pre-costing) branches are never taken.
     * RefDocumentType -> CommonServices.GetSlipsByDocumentTypeId (:749) is not ported; the column is
     * hidden, as DataGridHistorySetting leaves it.
     */
    function link(r, key) {
        if (!window.DocLink) return msg('countx_doc_link.js is not loaded.');
        if (key === 'DocNo') {
            return window.DocLink.open(toI(r.DocumentTypeId), toI(r.Id), { srNo: toI(r.Id), message: msg });
        }
        if (key === 'RefDocNo' && toI(r.RefDocId) > 0) {
            return window.DocLink.open(toI(r.RefDocumentTypeId), toI(r.RefDocId),
                { srNo: toI(r.RefDocId), baseDocumentTypeId: toI(r.BaseDocumentTypeId), message: msg });
        }
    }

    /* ============================================================================== menu buttons */
    /** btnNew_Click:753 */
    function btnNew_Click() {
        if (fyStart) { $id('txtDateFrom').value = String(fyStart).substring(0, 10); fromTime = timePart(String(fyStart).length >= 19 ? String(fyStart) : ''); }
        var n = nowIso();
        $id('txtToDate').value = n.substring(0, 10); toTime = timePart(n);
        $id('cmbDocumentType').value = '';
        $id('cmbActivity').value = '1';
        gridTable = null;
        renderGrid();
    }
    /** btnRefresh_Click:769 */
    function btnRefresh_Click() {
        return getJson(API + '/branches').then(function (rows) {
            BranchesFill(rows, userBranchName);
        }).then(ComboFill).then(JobOrderFill).catch(function (e) { return msg(e.message); });
    }
    function MakeShortCutKeys() {
        K.shortcuts([['Ctrl+E', 'For Close'], ['Ctrl+S', 'For Show'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'],
                     ['Ctrl+P', 'For print'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
                     ['Ctrl+ArrowUp', 'For Focus On From Date in Filter']]);
    }
    function btnShow_Click() {
        return busy('btnShow', function () { return Promise.resolve(GridFill()).then(PrintButtonManage); });
    }
    function closeForm() {
        if (window.opener && !window.opener.closed) { window.close(); return; }
        K.close();
    }

    /* ==================================================================================== events */
    function wire() {
        wireBranchCombo();
        $id('btnShow').addEventListener('click', btnShow_Click);
        $id('btnNew').addEventListener('click', btnNew_Click);
        $id('btnRefresh').addEventListener('click', function () { busy('btnRefresh', btnRefresh_Click); });
        $id('btnPrint').addEventListener('click', btnPrint_Click);
        $id('btnShortCut').addEventListener('click', MakeShortCutKeys);

        var g = $id('DataGridHistory');
        g.addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr[data-i]');
            if (!tr || !gridTable) return;
            cur = toI(tr.getAttribute('data-i'));
            Array.prototype.forEach.call(g.querySelectorAll('tbody tr'), function (x) { x.classList.toggle('cur', x === tr); });
            var r = gridTable.rows[cur];
            var b = e.target.closest('button[data-print]');
            if (b) { printSlip(r, b); return; }
            var a = e.target.closest('a[data-link]');
            if (a) link(r, a.getAttribute('data-link'));
        });

        /* frmEvaulationDetailWagesReports_KeyDown:922 (KeyPreview) - independent ifs, as on the desktop. */
        K.enterToTab();
        document.addEventListener('keydown', function (e) {
            if (e.defaultPrevented || modalOpen()) return;
            var k = e.key;
            if (e.ctrlKey && (k === 's' || k === 'S')) { e.preventDefault(); btnShow_Click(); }
            if ((e.ctrlKey && (k === 'e' || k === 'E')) || k === 'Escape') { e.preventDefault(); closeForm(); }
            if (e.ctrlKey && (k === 'r' || k === 'R')) { e.preventDefault(); busy('btnRefresh', btnRefresh_Click); }
            if (e.ctrlKey && (k === 'n' || k === 'N')) { e.preventDefault(); btnNew_Click(); }
            if (e.ctrlKey && (k === 'p' || k === 'P')) { e.preventDefault(); btnPrint_Click(); }
            if (e.ctrlKey && k === 'ArrowDown') { e.preventDefault(); $id('wrapGrid').focus(); }
            if (e.ctrlKey && (k === 'ArrowUp' || k === 'F5')) { e.preventDefault(); $id('txtDateFrom').focus(); }
            if ((e.ctrlKey && k === 'Alt') || (e.altKey && k === 'Control')) { e.preventDefault(); MakeShortCutKeys(); }
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        var n = nowIso();
        $id('txtDateFrom').value = n.substring(0, 10);      /* designer default: DateTime.Now */
        $id('txtToDate').value = n.substring(0, 10);
        ['cmbDocumentType', 'cmbWagesType', 'CmbWagesAccount', 'CmbJobOrderNo', 'CmbContractorName', 'CmbStockParty',
         'CmbWagesVoucherAccount', 'cmbActivity'].forEach(function (id) { unbind($id(id)); });
        wire();
        Load().then(function () {
            /* frmProductionSettlement.grdOverHeadSettlement_LinkClicked:989 - after Show(). */
            var q = new URLSearchParams(window.location.search);
            var id = toI(q.get('jobOrderId'));
            if (id > 0) {
                setValue($id('CmbJobOrderNo'), id);
                return GridFill();
            }
        });
    });
}());
