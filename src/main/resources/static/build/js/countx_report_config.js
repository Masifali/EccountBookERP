/* frmReportConfig.cs, method for method: Reset, GridFill, Insert, RetrivedData,
 * grdhistory_DoubleClick, tabControl1_SelectedIndexChanged, frmReportConfig_KeyDown. */
(function () {
    'use strict';

    var rows = [], view = [], current = -1;
    var RecId = 0;                 // frmReportConfig.RecId
    var UpdateMode = false;        // frmReportConfig.UpdateMode
    var token = null;
    var tab = 'form';              // tabControl1.SelectedIndex: 0 Form, 1 History
    var busy = false;

    /* Sp_ReportConfig_GetAllMethod @Activity='ReadAll' returns these seventeen, in this order.
       bit columns are drawn as checkboxes, the way Janus GridEX draws them on the desktop. */
    var COLS = [
        { k: 'ReportConfigId',      t: 'Report Config Id', type: 'code' },
        { k: 'ReportTitle',         t: 'Report Title',      type: 'wrap' },
        { k: 'ReportShortName',     t: 'Report Short Name', type: 'wrap' },
        { k: 'ReportFolder',        t: 'Report Folder',     type: 'wrap' },
        { k: 'ReportFileName',      t: 'Report File Name',  type: 'wrap' },
        { k: 'ReportSeqNo',         t: 'Report Seq No',     type: 'num'  },
        { k: 'ReportIconURL',       t: 'Report Icon URL',   type: 'wrap' },
        { k: 'ReportProcedureName', t: 'Report Procedure Name', type: 'wrap' },
        { k: 'TargetSource',        t: 'Target Source',     type: 'wrap' },
        { k: 'TargetFunctionName',  t: 'Target Function Name', type: 'wrap' },
        { k: 'IsSubReport',         t: 'Is Sub Report',     type: 'chk'  },
        { k: 'IsActive',            t: 'Is Active',         type: 'chk'  },
        { k: 'CreatedById',         t: 'Created By Id',     type: 'num'  },
        { k: 'CreatedOn',           t: 'Created On',        type: 'wrap' },
        { k: 'AlteredById',         t: 'Altered By Id',     type: 'num'  },
        { k: 'AlteredOn',           t: 'Altered On',        type: 'wrap' },
        { k: 'ActionTypeId',        t: 'Action Type Id',    type: 'num'  }
    ];

    function $(id) { return document.getElementById(id); }
    function esc(v) { return v === null || v === undefined ? '' : String(v); }
    function truthy(v) { return v === true || v === 1 || v === '1' || String(v).toLowerCase() === 'true'; }

    /* ------------------------------------------------------------- MessageBox.Show */
    function messageBox(text, buttons) {
        return new Promise(function (resolve) {
            $('mbBody').textContent = text;
            var box = $('mbBtns');
            box.innerHTML = '';
            (buttons || ['OK']).forEach(function (b) {
                var btn = document.createElement('button');
                btn.type = 'button'; btn.textContent = b;
                btn.addEventListener('click', function () { $('mbMask').style.display = 'none'; resolve(b); });
                box.appendChild(btn);
            });
            $('mbMask').style.display = 'flex';
            var f = box.querySelector('button'); if (f) { f.focus(); }
        });
    }

    /* Button guard: disable, loader, no duplicate request, re-enable on either outcome. */
    function setBusy(btn, on) {
        busy = on;
        ['btnnew', 'btnsave', 'btnupdate', 'btnPrint', 'btnRefresh', 'ReportAllocatToCompany']
            .forEach(function (id) { $(id).disabled = on; });
        if (btn) { btn.classList.toggle('busy', on); }
    }

    /* ---------------------------------------------------------------------- Reset() */
    function Reset() {
        $('txtReportTitle').value = '';
        $('txtReportShortName').value = '';
        $('txtReportFolder').value = '';
        $('txtReportFileName').value = '';
        $('txtReportProcedureName').value = '';
        $('IsSubReport').checked = false;
        $('IsActive').checked = false;
        $('txtReportTitle').focus();
        /* Reset() does NOT touch RecId, btnsave or btnupdate on the desktop - only RetrivedData
           and btnsave_Click move those - so neither is touched here. */
    }

    /* ------------------------------------------------------------------- the grid */
    function buildHead() {
        var h = $('grdHead'), f = $('grdFilter');
        h.innerHTML = ''; f.innerHTML = '';
        COLS.forEach(function (c) {
            var th = document.createElement('th'); th.textContent = c.t; h.appendChild(th);
            var tf = document.createElement('th');
            var i = document.createElement('input'); i.dataset.col = c.k;
            i.addEventListener('input', applyFilter);
            tf.appendChild(i); f.appendChild(tf);
        });
    }

    function applyFilter() {
        var terms = {};
        $('grdFilter').querySelectorAll('input').forEach(function (i) {
            var v = (i.value || '').trim().toLowerCase();
            if (v) { terms[i.dataset.col] = v; }
        });
        view = rows.filter(function (r) {
            for (var c in terms) { if (esc(r[c]).toLowerCase().indexOf(terms[c]) === -1) { return false; } }
            return true;
        });
        paint();
    }

    function paint() {
        var tb = $('grdBody');
        tb.innerHTML = '';
        view.forEach(function (r, i) {
            var tr = document.createElement('tr');
            COLS.forEach(function (c) {
                var td = document.createElement('td');
                if (c.type === 'chk') {
                    td.className = 'chk';
                    var cb = document.createElement('input');
                    cb.type = 'checkbox'; cb.checked = truthy(r[c.k]); cb.disabled = true;
                    td.appendChild(cb);
                } else if (c.type === 'code') {
                    /* The brief: show the code on every row and make it open the record. */
                    var a = document.createElement('a');
                    a.className = 'code'; a.textContent = esc(r[c.k]);
                    a.addEventListener('click', function (e) { e.stopPropagation(); RetrivedData(r.ReportConfigId); });
                    td.appendChild(a);
                } else {
                    td.className = c.type === 'num' ? 'num' : 'wrap';
                    td.textContent = esc(r[c.k]);
                }
                tr.appendChild(td);
            });
            tr.addEventListener('click', function () { select(i); });
            /* grdhistory_DoubleClick */
            tr.addEventListener('dblclick', function () { RetrivedData(r.ReportConfigId); });
            tb.appendChild(tr);
        });
        current = view.length ? 0 : -1;
        select(current);
    }

    function select(i) {
        var tb = $('grdBody');
        var s = tb.querySelector('tr.sel'); if (s) { s.classList.remove('sel'); }
        current = i;
        if (i >= 0 && tb.rows[i]) { tb.rows[i].classList.add('sel'); tb.rows[i].scrollIntoView({ block: 'nearest' }); }
        $('navPos').textContent = (current >= 0 ? current + 1 : 0) + ' of ' + view.length;
    }

    /* -------------------------------------------------------------------- GridFill() */
    function GridFill() {
        return fetch('/configurations/define-reports/api/history', { credentials: 'same-origin' })
            .then(function (r) { return r.json().then(function (b) { return { ok: r.ok, b: b }; }); })
            .then(function (x) {
                if (!x.ok) { return messageBox(x.b.message || 'Could not load.'); }
                token = x.b.token || null;
                rows = x.b.rows || [];
                applyFilter();
            })
            .catch(function (e) { return messageBox(e.message); });
    }

    /* --------------------------------------------------------------- RetrivedData(Id) */
    function RetrivedData(id) {
        return fetch('/configurations/define-reports/api/' + encodeURIComponent(id), { credentials: 'same-origin' })
            .then(function (r) { return r.json().then(function (b) { return { ok: r.ok, b: b }; }); })
            .then(function (x) {
                if (!x.ok) { return messageBox(x.b.message || 'Could not load the report.'); }
                var o = x.b;
                RecId = o.ReportConfigId;
                $('txtReportTitle').value         = esc(o.ReportTitle);
                $('txtReportShortName').value     = esc(o.ReportShortName);
                $('txtReportFolder').value        = esc(o.ReportFolder);
                $('txtReportFileName').value      = esc(o.ReportFileName);
                $('txtReportProcedureName').value = esc(o.ReportProcedureName);
                $('IsSubReport').checked          = truthy(o.IsSubReport);
                $('IsActive').checked             = truthy(o.IsActive);
                showTab('form');                       // tabControl1.SelectedIndex = 0
                $('btnsave').style.display = 'none';
                $('btnupdate').style.display = '';
                UpdateMode = true;
            })
            .catch(function (e) { return messageBox(e.message); });
    }

    /* --------------------------------------------------------------------- Insert() */
    function Insert(btn) {
        if (busy) { return; }
        return messageBox(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', ['Yes', 'No'])
            .then(function (a) {
                if (a !== 'Yes') { return; }            // DialogResult.No -> return
                setBusy(btn, true);
                return fetch('/configurations/define-reports/api/save', {
                    method: 'POST',
                    credentials: 'same-origin',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        token: token,
                        id: RecId,
                        reportTitle:         $('txtReportTitle').value,
                        reportShortName:     $('txtReportShortName').value,
                        reportFolder:        $('txtReportFolder').value,
                        reportFileName:      $('txtReportFileName').value,
                        reportProcedureName: $('txtReportProcedureName').value,
                        isSubReport:         $('IsSubReport').checked,
                        isActive:            $('IsActive').checked
                    })
                })
                    .then(function (r) { return r.json().then(function (b) { return { ok: r.ok, b: b }; }); })
                    .then(function (x) {
                        if (!x.ok) { return messageBox(x.b.message || 'Save failed.'); }
                        return messageBox(x.b.message).then(function () {
                            rows = x.b.rows || rows;
                            applyFilter();
                            Reset();                     // Insert() ends with Reset() either way
                        });
                    })
                    .catch(function (e) { return messageBox(e.message); })
                    .then(function () { setBusy(btn, false); });
            });
    }

    /* --------------------------------------------------- tabControl1_SelectedIndexChanged */
    function showTab(which) {
        tab = which;
        $('formPane').style.display    = which === 'form' ? '' : 'none';
        $('historyPane').style.display = which === 'history' ? '' : 'none';
        document.querySelectorAll('#tabstrip .tab').forEach(function (t) {
            t.classList.toggle('on', t.dataset.tab === which);
        });
        if (which === 'history') { GridFill(); }        // SelectedIndex == 1 -> GridFill()
    }

    /* ----------------------------------------------------------- frmReportConfig_KeyDown */
    function keyDown(e) {
        if (e.key === 'Enter' && !e.ctrlKey) {           // Keys.Return -> SendKeys "{TAB}"
            var f = Array.prototype.filter.call(
                document.querySelectorAll('#panel4 input, .toolstrip button'),
                function (el) { return !el.disabled && el.offsetParent !== null; });
            var i = f.indexOf(document.activeElement);
            if (i > -1) { e.preventDefault(); f[(i + 1) % f.length].focus(); }
            return;
        }
        if ((e.ctrlKey && e.key.toLowerCase() === 'e') || e.key === 'Escape') {
            e.preventDefault(); window.location.href = '/admin-panel'; return;
        }
        if (!e.ctrlKey) { return; }
        var k = e.key.toLowerCase();
        var saveShown   = $('btnsave').style.display !== 'none';
        var updateShown = $('btnupdate').style.display !== 'none';
        if (k === 's' && saveShown && !$('btnsave').disabled && tab === 'form')      { e.preventDefault(); RecId = 0; Insert($('btnsave')); }
        if (k === 'n' && tab === 'form')                                             { e.preventDefault(); Reset(); }
        if (k === 'u' && updateShown && !$('btnupdate').disabled && tab === 'form' && UpdateMode) { e.preventDefault(); Insert($('btnupdate')); }
    }

    document.addEventListener('DOMContentLoaded', function () {
        if (!$('btnsave')) { return; }                  // the Admin condition refused the form

        buildHead();

        document.querySelectorAll('#tabstrip .tab').forEach(function (t) {
            t.addEventListener('click', function () { showTab(t.dataset.tab); });
        });

        $('btnnew').addEventListener('click', Reset);
        /* btnsave_Click sets RecId = 0 first, so Save always inserts. */
        $('btnsave').addEventListener('click', function () { RecId = 0; Insert($('btnsave')); });
        $('btnupdate').addEventListener('click', function () { Insert($('btnupdate')); });
        /* ReportAllocatToCompany_Click -> new frmCompanyReport(UserAccount).Show()
           .Show() is modeless on the desktop, so the form opens alongside this one rather than
           replacing it - a new window here, not a navigation. */
        $('ReportAllocatToCompany').addEventListener('click', function () {
            window.open('/configurations/report-allocate-to-company',
                        'frmCompanyReport', 'width=915,height=615,resizable=yes,scrollbars=yes');
        });

        $('grdFull').addEventListener('click', function () { $('gridWrap').classList.toggle('full'); });
        $('grdExport').addEventListener('click', function () {
            var csv = [COLS.map(function (c) { return '"' + c.t + '"'; }).join(',')];
            view.forEach(function (r) {
                csv.push(COLS.map(function (c) { return '"' + esc(r[c.k]).replace(/"/g, '""') + '"'; }).join(','));
            });
            var a = document.createElement('a');
            a.href = URL.createObjectURL(new Blob([csv.join('\n')], { type: 'text/csv' }));
            a.download = 'ReportConfig.csv';
            a.click();
        });

        $('navigator').addEventListener('click', function (e) {
            var n = e.target.dataset && e.target.dataset.nav;
            if (!n || !view.length) { return; }
            if (n === 'first') { select(0); }
            if (n === 'prev')  { select(Math.max(0, current - 1)); }
            if (n === 'next')  { select(Math.min(view.length - 1, current + 1)); }
            if (n === 'last')  { select(view.length - 1); }
        });

        document.addEventListener('keydown', keyDown);   // base.KeyPreview = true
        $('txtReportTitle').focus();                     // frmReportConfig_Load
    });
})();
