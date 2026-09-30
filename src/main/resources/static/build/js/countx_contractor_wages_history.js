/* ============================================================================================
 * "Wages Report" / "Contractor Wages History" -
 * Architecture.WinApp.Contractor_Wages/frmStockContractorWagesHistory.cs (1,999 lines).
 * Line numbers below are that file's. Each server call is one desktop method.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/accounts/reports/contractor-wages-history';
    var K = window.ReportKit;

    var IsPartyProcessingFeatureOn = false;
    var BranchFeature = false;
    var dtBranch = [];
    var branchText = '';
    var branchChecked = {};
    var branchActiveRow = null;
    var userBranchName = '';
    var fyStart = null;
    var fmt = { amount: 0, rate: 2 };
    var dtGridCount = 0;
    var lastPrintArgs = null;
    var printText = '001-Print';        /* Print.Text */
    var gridTable = null;               /* { name, cols, rows, groups } - null = ClearStructure() */
    var fromTime = timePart(nowIso()), toTime = timePart(nowIso());
    var cur = -1;
    var order = [];                     /* data rows in display order (for the navigator) */

    function $id(id) { return document.getElementById(id); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
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
    /** GridEX default DateTime text: the short date (dd/MM/yyyy on the desktop's locale). */
    function shortDate(v) {
        if (v === null || v === undefined || v === '') return '';
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v));
        return m ? m[3] + '/' + m[2] + '/' + m[1] : String(v);
    }

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
            function key(e) { if (e.key === 'Escape' || e.key === 'Enter') { e.preventDefault(); e.stopPropagation(); done(); } }
            document.addEventListener('keydown', key, true);
            b.addEventListener('click', done);
        });
    }
    function modalOpen() { return !!document.querySelector('.wr-modal, .rk-modal.is-open'); }

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
    function getJson(url) { return fetch(url, { credentials: 'same-origin', headers: { 'Accept': 'application/json' } }).then(handle); }
    function postJson(url, body) {
        return fetch(url, { method: 'POST', credentials: 'same-origin',
            headers: headers({ 'Content-Type': 'application/json', 'Accept': 'application/json' }),
            body: JSON.stringify(body) }).then(handle);
    }
    function busy(id, fn) {
        var b = $id(id);
        if (b && b.classList.contains('is-busy')) return Promise.resolve();
        var was = b ? b.disabled : false;
        if (b) { b.classList.add('is-busy'); b.disabled = true; }
        var done = function () { if (b) { b.classList.remove('is-busy'); b.disabled = was; } };
        var p;
        try { p = Promise.resolve(fn()); } catch (e) { p = Promise.reject(e); }
        return p.then(function (v) { done(); return v; }, function (e) { done(); throw e; });
    }

    /* ------------------------------------------------------------------ combos */
    /** DDL.BindDDL(dt, combo, "Id", "name", caption, ZeroIndex:false). */
    function bind(sel, rows, idKey, textKey) {
        var h = '<option value=""></option>';
        (rows || []).forEach(function (r) {
            h += '<option value="' + esc(col(r, idKey)) + '">' + esc(col(r, textKey)) + '</option>';
        });
        sel.innerHTML = h;
        sel.value = '';
    }
    function unbind(sel) { sel.innerHTML = '<option value=""></option>'; sel.value = ''; }
    function fire(sel) { sel.dispatchEvent(new Event('change', { bubbles: true })); }

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
        pop.addEventListener('mousedown', function (e) { e.preventDefault(); });
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
        /* CmbBranchName_Leave:318 */
        box.addEventListener('focusout', function () {
            setTimeout(function () {
                if (box.contains(document.activeElement)) return;
                pop.classList.add('is-hidden');
                CmbBranchName_Leave();
            }, 0);
        });
    }

    /* ======================================================================= frmPendingGrnRpt_Load:143 */
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
            BranchesFill(d.branches, d.userBranchName || '');
            WagesTypesFill();
            ReportTypeBind();
        }).catch(function (e) { return msg(e.message); });
    }
    /** BranchesFill:166 - Text = UserAccount.BranchName. An UltraCombo whose Text matches a list item
        makes that item its ActiveRow, so the Leave that follows fills the other combos. */
    function BranchesFill(rows, name) {
        userBranchName = name;
        dtBranch = [];
        branchChecked = {};
        branchActiveRow = null;
        if (rows && rows.length > 0) {
            dtBranch = rows;
            branchText = name || '';
            $id('CmbBranchNameText').value = branchText;
            dtBranch.forEach(function (r, i) {
                if (String(col(r, 'BranchName')).toLowerCase() === branchText.toLowerCase()) branchActiveRow = i;
            });
        }
    }
    /** WagesTypesFill:196 - hard-coded on the desktop. */
    function WagesTypesFill() {
        bind($id('cmbWagesType'), [{ Id: 1, WagesType: 'Regular' }, { Id: 2, WagesType: 'Free of Cost' }], 'Id', 'WagesType');
    }
    /** ReportTypeBind:285 - Detail / Summary, Rows[0].Activate(). */
    function ReportTypeBind() {
        bind($id('CmbReportType'), [{ Id: 1, name: 'Detail' }, { Id: 2, name: 'Summary' }], 'Id', 'name');
        $id('CmbReportType').value = '1';
        fire($id('CmbReportType'));
        CmbReportType_TextChanged();
    }
    /** ComboFill:224 */
    function ComboFill() {
        if (branchText === '') {
            $id('CmbBranchNameText').focus();
            return Promise.reject(new Error('Select Branch First'));
        }
        return postJson(API + '/combos', { branchText: branchText }).then(function (d) {
            if (!d || !d.bound) return;
            bind($id('cmbDocumentType'), d.documentTypes, 'Id', 'name');
            bind($id('CmbWagesAccount'), d.wagesAccounts, 'Id', 'name');
            bind($id('cmbSupplierCustomer'), d.contractors, 'Id', 'name');
            bind($id('CmbStockParty'), d.stockParties, 'Id', 'name');
        });
    }
    /** CmbBranchName_Leave:318 */
    function CmbBranchName_Leave() {
        if (branchActiveRow !== null) return ComboFill().catch(function (e) { return msg(e.message); });
        ['cmbDocumentType', 'CmbWagesAccount', 'cmbSupplierCustomer', 'CmbStockParty'].forEach(function (id) { unbind($id(id)); });
    }
    /** CmbReportType_TextChanged:855 */
    function CmbReportType_TextChanged() {
        ['lblchkGroupOnDocType', 'lblchkGroupOnDocDate', 'lblChkGroupOnContractor', 'lblchkGroupOnwagesActivity', 'lblchkGroupOnItem']
            .forEach(function (id) { $id(id).classList.remove('is-hidden'); });
        if (toI($id('CmbReportType').value) === 2) {
            $id('chkGroupOnDocType').checked = true;
            $id('lblchkGroupOnDocDate').classList.add('is-hidden');
            $id('lblChkGroupOnContractor').classList.add('is-hidden');
            $id('lblchkGroupOnItem').classList.add('is-hidden');
        }
    }

    /* ================================================================================ GridBind:359 */
    function dateArg(id, time) { var v = $id(id).value; return v ? v + time : null; }
    function GridBind() {
        $id('BtnPrintNew').classList.add('is-hidden');
        var reportType = toI($id('CmbReportType').value);
        var body = {
            reportType: reportType,
            documentTypeId: toI($id('cmbDocumentType').value),
            fromDate: dateArg('txtFromDate', fromTime),
            toDate: dateArg('txtToDate', toTime),
            contractorId: toI($id('cmbSupplierCustomer').value),
            wagesAccountId: toI($id('CmbWagesAccount').value),
            stockPartyId: toI($id('CmbStockParty').value),
            branchWise: $id('chkBranchWise').checked,
            wagesType: toI($id('cmbWagesType').value),
            party: (document.querySelector('input[name="party"]:checked') || {}).value || 'all',
            branchText: branchText
        };
        if (branchText === '') {
            $id('CmbBranchNameText').focus();
            return msg('Select Branch First');
        }
        if (reportType !== 1 && reportType !== 2) return Promise.resolve();
        return postJson(API + '/grid', body).then(function (d) {
            var rows = (d && d.rows) || [];
            dtGridCount = rows.length;
            lastPrintArgs = d ? d.printArgs : null;
            if (rows.length > 0) {
                if (reportType === 1) {
                    setPrintText('001-Print');
                    $id('BtnPrintNew').classList.remove('is-hidden');
                    gridTable = detail(rows);
                } else {
                    setPrintText('003-Print');
                    gridTable = summary(rows);
                }
            } else {
                gridTable = null;                          /* grdfrm.ClearStructure() */
            }
            cur = -1;
            renderGrid();
        }).catch(function (e) { return msg(e.message); });
    }
    function setPrintText(t) { printText = t; $id('PrintText').textContent = t; }

    /* ---- GridBind :421-455 (dt) + GridSetting:543 */
    function detail(g) {
        var c = function (k, t, w, hide) { return { k: k, t: t || 's', w: w, hide: !!hide }; };
        var cols = [
            c('Id', 'i', 60, true), c('BranchName', 's', 110, !BranchFeature), c('DocumentTypeId', 'i', 60, true),
            c('RefDocumentTypeId', 'i', 60, true), c('D.Type', 's', 120, true), c('RefNo', 'i', 60), c('RefDate', 'd', 80, true),
            c('DocNo', 'i', 60), c('DocDate', 'd', 70, true), c('Contractor', 's', 120, true), c('WagesActivity', 's', 120, true),
            c('ItemName', 's', 120, true), c('CropYear', 's', 80), c('JobLot', 's', 120), c('PackingType', 's', 80),
            c('Qty', 'n', 60), c('PackSize', 's', 70), c('WeightCut', 'n', 80), c('BillWeight', 'n', 80), c('WageRate', 'n', 80),
            c('WagesAmount', 'n', 100), c('WagesTypeId', 'i', 60, true), c('WagesType', 's', 80), c('WarehouseFrom', 's', 110),
            c('WarehouseTo', 's', 100), c('EntryUser', 's', 120), c('EntryDate', 'd', 80), c('ModifyUser', 's', 100, true),
            c('ModifyDate', 'd', 80, true), c('ApprovedUser', 's', 100), c('ApprovedDate', 'd', 80, true),
            c('OtherRemarks', 's', 150), c('RemarksDetail', 's', 150)
        ];
        var rows = g.map(function (r) {
            return {
                'Id': col(r, 'Id'), 'BranchName': col(r, 'BranchName'), 'DocumentTypeId': col(r, 'DocumentTypeId'),
                'RefDocumentTypeId': col(r, 'RefDocumentTypeId'), 'D.Type': col(r, 'DocumentTypeDescription'),
                'RefNo': col(r, 'RefDocNo'), 'RefDate': col(r, 'RefDocDate'), 'DocNo': col(r, 'DocNo'), 'DocDate': col(r, 'DocDate'),
                'Contractor': col(r, 'ContractorName'), 'WagesActivity': col(r, 'WagesAccountName'), 'ItemName': col(r, 'ItemName'),
                'CropYear': col(r, 'Crop'), 'JobLot': col(r, 'JobLotDescription'), 'PackingType': col(r, 'PackTypeDesc'),
                'Qty': col(r, 'Qty'), 'PackSize': col(r, 'PackSize'), 'WeightCut': col(r, 'WeightCut'), 'BillWeight': col(r, 'BillWeight'),
                'WageRate': col(r, 'WageRate'), 'WagesAmount': col(r, 'WagesAmount'), 'WagesTypeId': col(r, 'WagesTypeId'),
                'WagesType': col(r, 'WagesType'), 'WarehouseFrom': col(r, 'WareHouseFrom'), 'WarehouseTo': col(r, 'WareHouseNameTo'),
                'EntryUser': col(r, 'EntryUserName'), 'EntryDate': col(r, 'EntryDate'), 'ModifyUser': col(r, 'ModifyUserName'),
                'ModifyDate': col(r, 'ModifyDate'), 'ApprovedUser': col(r, 'ApprovedUserName'), 'ApprovedDate': col(r, 'ApprovedDate'),
                'OtherRemarks': col(r, 'OtherRemarks'), 'RemarksDetail': col(r, 'RemarksDetail')
            };
        });
        /* Groups in the order GridSetting adds them; the five columns stay hidden either way (:569-589). */
        var groups = [];
        if ($id('chkGroupOnDocType').checked) groups.push('D.Type');
        if ($id('chkGroupOnDocDate').checked) groups.push('DocDate');
        if ($id('ChkGroupOnContractor').checked) groups.push('Contractor');
        if ($id('chkGroupOnwagesActivity').checked) groups.push('WagesActivity');
        if ($id('chkGroupOnItem').checked) groups.push('ItemName');
        return { name: 'WagesDetailGrid', cols: cols, rows: rows, groups: groups };
    }
    /* ---- GridBind :479-492 (dt2) + GridSettingForSummary:622 */
    function summary(g) {
        var byDoc = $id('chkGroupOnDocType').checked, byAct = $id('chkGroupOnwagesActivity').checked;
        var cols = [
            { k: 'BranchName', t: 's', hide: !BranchFeature }, { k: 'Description', t: 's', hide: byDoc },
            { k: 'WagesType', t: 's' }, { k: 'WagesAccount', t: 's', hide: byAct },
            { k: 'Qty', t: 'n' }, { k: 'Weight', t: 'n' }, { k: 'AvgRate', t: 'n' }, { k: 'Amount', t: 'n' }
        ];
        var rows = g.map(function (r) {
            return { BranchName: col(r, 'BranchName'), Description: col(r, 'Description'), WagesType: col(r, 'WagesType'),
                     WagesAccount: col(r, 'WagesAccountName'), Qty: col(r, 'Qty'), Weight: col(r, 'Weight'),
                     AvgRate: col(r, 'AvgRate'), Amount: col(r, 'Amount') };
        });
        /* CommonServices.GridAutoAdjustmentNew: widths fitted to the content. */
        cols.forEach(function (c) {
            var n = c.k.length;
            rows.forEach(function (r) { var s = fmtCell(c, r[c.k]); if (s.length > n) n = s.length; });
            c.w = Math.min(Math.max(60, n * 7 + 12), 360);
        });
        var groups = [];
        if (byDoc) groups.push('Description');
        if (byAct) groups.push('WagesAccount');
        return { name: 'WagesSummaryGrid', cols: cols, rows: rows, groups: groups };
    }

    /** ConfigureNumericalColumn:517 - Amount: stringFormatsingle + Sum; Rate: DecimalRateFormate, no Sum;
        any other double: "#,##0.##" + Sum. */
    function fmtCell(c, v) {
        if (v === null || v === undefined || v === '') return '';
        if (c.t === 'd') return shortDate(v);
        if (c.t !== 'n') return String(v);
        if (c.k.indexOf('Amount') >= 0) return K.fixed(toD(v), fmt.amount);
        if (c.k.indexOf('Rate') >= 0) return K.fixed(toD(v), fmt.rate);
        return K.num(v, 2);
    }
    function sums(c) { return c.t === 'n' && c.k.indexOf('Rate') < 0; }
    function groupKey(k, v) {
        if (v === null || v === undefined) return '';
        return k === 'DocDate' ? shortDate(v) : String(v);
    }

    function renderGrid() {
        var tb = $id('grdfrm');
        order = [];
        if (!gridTable) { tb.innerHTML = ''; nav(); return; }
        var cols = gridTable.cols.filter(function (c) { return !c.hide; });
        var n = cols.length;
        var h = '<colgroup>' + cols.map(function (c) { return '<col style="width:' + c.w + 'px">'; }).join('')
            + '</colgroup><thead><tr>' + cols.map(function (c) { return '<th>' + esc(c.k) + '</th>'; }).join('')
            + '</tr></thead><tbody>';
        function totals(rows, cls) {
            return '<tr class="' + cls + '">' + cols.map(function (c) {
                if (!sums(c)) return '<td></td>';
                var s = 0; rows.forEach(function (r) { s += toD(r[c.k]); });
                return '<td class="num">' + esc(fmtCell(c, s)) + '</td>';
            }).join('') + '</tr>';
        }
        function line(r) {
            var i = gridTable.rows.indexOf(r);
            order.push(i);
            return '<tr data-i="' + i + '"' + (i === cur ? ' class="cur"' : '') + '>' + cols.map(function (c) {
                var t = fmtCell(c, r[c.k]);
                return '<td' + (c.t === 'n' ? ' class="num"' : '') + ' title="' + esc(t) + '">' + esc(t) + '</td>';
            }).join('') + '</tr>';
        }
        function level(rows, depth) {
            if (depth >= gridTable.groups.length) { rows.forEach(function (r) { h += line(r); }); return; }
            var k = gridTable.groups[depth], keys = [], b = {};
            rows.forEach(function (r) {
                var v = groupKey(k, r[k]);
                if (!Object.prototype.hasOwnProperty.call(b, v)) { b[v] = []; keys.push(v); }
                b[v].push(r);
            });
            keys.sort(function (a, c) {
                if (k === 'DocDate') { a = a.split('/').reverse().join(''); c = c.split('/').reverse().join(''); }
                a = a.toLowerCase(); c = c.toLowerCase();
                return a < c ? -1 : a > c ? 1 : 0;
            });
            keys.forEach(function (v) {
                h += '<tr class="grp rk-keep"><td colspan="' + n + '" style="padding-left:' + (4 + depth * 16) + 'px">&#8863; '
                    + esc(k) + ': ' + esc(v) + '</td></tr>';
                level(b[v], depth + 1);
                h += totals(b[v], 'gt rk-keep');            /* GroupTotals.Always */
            });
        }
        level(gridTable.rows, 0);
        h += '</tbody><tfoot>' + totals(gridTable.rows, 'tt') + '</tfoot>';
        tb.innerHTML = h;
        if (K.filterRow) K.filterRow(tb);                   /* FilterMode.Automatic */
        nav();
    }

    /* RecordNavigator */
    function nav() {
        $id('navCount').textContent = order.length;
        $id('navPos').value = cur >= 0 ? (order.indexOf(cur) + 1) : 0;
    }
    function goTo(pos) {
        if (!order.length) return;
        pos = Math.max(0, Math.min(order.length - 1, pos));
        cur = order[pos];
        Array.prototype.forEach.call($id('grdfrm').querySelectorAll('tbody tr[data-i]'), function (x) {
            var on = toI(x.getAttribute('data-i')) === cur;
            x.classList.toggle('cur', on);
            if (on) x.scrollIntoView({ block: 'nearest' });
        });
        nav();
    }

    /* ============================================================================== menu buttons */
    /** btnnew_Click:658 */
    function btnnew_Click() {
        if (fyStart) { $id('txtFromDate').value = String(fyStart).substring(0, 10); fromTime = timePart(String(fyStart).length >= 19 ? String(fyStart) : ''); }
        $id('cmbSupplierCustomer').value = ''; fire($id('cmbSupplierCustomer'));
        $id('cmbDocumentType').value = ''; fire($id('cmbDocumentType'));
        $id('CmbWagesAccount').value = ''; fire($id('CmbWagesAccount'));
        branchText = ''; branchActiveRow = null; $id('CmbBranchNameText').value = '';
        $id('txtFromDate').focus();
    }
    /** btnRefresh_Click:673 */
    function btnRefresh_Click() {
        return getJson(API + '/branches').then(function (rows) { BranchesFill(rows, userBranchName); })
            .then(ComboFill).catch(function (e) { return msg(e.message); });
    }
    /** Print_Click:697 */
    function Print_Click() {
        if (dtGridCount === 0) return msg('Record Not Found For Display');
        return window.CrystalPrint.open(printText === '001-Print' ? 'wh-001' : 'wh-003', lastPrintArgs || {}, 'Print');
    }
    /** BtnPrintNew_Click:722 */
    function BtnPrintNew_Click() {
        if (dtGridCount === 0) return msg('Record Not Found For Display');
        return window.CrystalPrint.open('wh-003A', lastPrintArgs || {}, 'BtnPrintNew');
    }
    function MakeShortCutKeys() {
        K.shortcuts([['Ctrl+E', 'For Close'], ['Ctrl+S', 'For Show'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'],
                     ['Ctrl+P', 'For print'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
                     ['Ctrl+ArrowUp', 'For Focus On From Date in Filter']]);
    }
    function btnshow_Click() { return busy('btnshow', GridBind); }
    function closeForm() {
        if (window.opener && !window.opener.closed) { window.close(); return; }
        window.location.href = '/accounts/vouchers/contractor-wages-dashboard';
    }

    function wire() {
        wireBranchCombo();
        $id('btnshow').addEventListener('click', btnshow_Click);
        $id('btnnew').addEventListener('click', btnnew_Click);
        $id('btnRefresh').addEventListener('click', function () { busy('btnRefresh', btnRefresh_Click); });
        $id('Print').addEventListener('click', Print_Click);
        $id('BtnPrintNew').addEventListener('click', BtnPrintNew_Click);
        $id('btnShortCut').addEventListener('click', MakeShortCutKeys);
        $id('CmbReportType').addEventListener('change', CmbReportType_TextChanged);
        $id('txtFromDate').addEventListener('change', function () { fromTime = timePart(nowIso()); });
        $id('txtToDate').addEventListener('change', function () { toTime = timePart(nowIso()); });

        $id('grdfrm').addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr[data-i]');
            if (!tr) return;
            goTo(order.indexOf(toI(tr.getAttribute('data-i'))));
        });
        $id('recordNavigator').addEventListener('click', function (e) {
            var b = e.target.closest('button[data-nav]');
            if (!b || !order.length) return;
            var p = cur >= 0 ? order.indexOf(cur) : -1, a = b.getAttribute('data-nav');
            goTo(a === 'first' ? 0 : a === 'last' ? order.length - 1 : a === 'prev' ? p - 1 : p + 1);
        });
        $id('navPos').addEventListener('change', function () { goTo(toI(this.value) - 1); });

        /* frmStockContractorWagesHistory_KeyDown:769 (KeyPreview) */
        K.enterToTab();
        document.addEventListener('keydown', function (e) {
            if (e.defaultPrevented || modalOpen()) return;
            var k = e.key;
            if (e.ctrlKey && (k === 's' || k === 'S')) { e.preventDefault(); btnshow_Click(); }
            if ((e.ctrlKey && (k === 'e' || k === 'E')) || k === 'Escape') { e.preventDefault(); closeForm(); }
            if (e.ctrlKey && (k === 'r' || k === 'R')) { e.preventDefault(); busy('btnRefresh', btnRefresh_Click); }
            if (e.ctrlKey && (k === 'n' || k === 'N')) { e.preventDefault(); btnnew_Click(); }
            if (e.ctrlKey && (k === 'p' || k === 'P')) { e.preventDefault(); Print_Click(); }
            if (e.ctrlKey && k === 'ArrowDown') { e.preventDefault(); $id('wrapGrid').focus(); }
            if (e.ctrlKey && (k === 'ArrowUp' || k === 'F5')) { e.preventDefault(); $id('txtFromDate').focus(); }
            if ((e.ctrlKey && k === 'Alt') || (e.altKey && k === 'Control')) { e.preventDefault(); MakeShortCutKeys(); }
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        var n = nowIso();
        $id('txtFromDate').value = n.substring(0, 10);      /* designer default: DateTime.Now */
        $id('txtToDate').value = n.substring(0, 10);
        ['cmbDocumentType', 'cmbWagesType', 'CmbWagesAccount', 'cmbSupplierCustomer', 'CmbStockParty', 'CmbReportType']
            .forEach(function (id) { unbind($id(id)); });
        wire();
        Load().then(function () {
            /* The branch combo's Text is the user's own branch (an ActiveRow), so its first Leave fills
               the four lists; fill them now so they are ready before the user tabs out. */
            if (branchActiveRow !== null) return ComboFill().catch(function (e) { return msg(e.message); });
        });
    });
}());
