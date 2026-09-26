/* frmCompanyReport.cs, method for method: frmCompanyReport_Load, CompanyFill, ReportHeaderFill,
 * grdFill, grdSetting, Reset, Insert, RetrivedData, grdhistory_DoubleClick,
 * tabControl1_SelectedIndexChanged, frmCompanyReport_KeyDown. */
(function () {
    'use strict';

    var reports = [], view = [], checked = {};     // grd rows + the Select column's state
    var history = [], hview = [], hcols = [];
    var RecId = 0;                                 // frmCompanyReport.RecId
    var UpdateMode = false;                        // frmCompanyReport.UpdateMode
    var token = null;
    var tab = 'form';                              // tabControl1.SelectedIndex: 0 Form, 1 History
    var busy = false;

    /* grdFill copies exactly these six columns out of Sp_ReportConfig_GetAllMethod into the
       grid's own DataTable. The other eleven the procedure returns are read and dropped. */
    var COLS = [
        { k: 'ReportConfigId',      t: 'Report Config Id',     type: 'num'  },
        { k: 'ReportTitle',         t: 'Report Title',         type: 'wrap' },
        { k: 'ReportShortName',     t: 'Report Short Name',    type: 'wrap' },
        { k: 'ReportFolder',        t: 'Report Folder',        type: 'wrap' },
        { k: 'ReportFileName',      t: 'Report File Name',     type: 'wrap' },
        { k: 'ReportProcedureName', t: 'Report Procedure Name', type: 'wrap' }
    ];

    function $(id) { return document.getElementById(id); }
    function esc(v) { return v === null || v === undefined ? '' : String(v); }

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
        ['btnnew', 'btnsave', 'btnupdate', 'btnPrint', 'btnRefresh'].forEach(function (id) {
            var el = $(id); if (el) { el.disabled = on; }
        });
        if (btn) { btn.classList.toggle('busy', on); }
    }

    /* ---------------------------------------------------------- CompanyFill / ReportHeaderFill
       DDL.BindDDLNew(dt, cmb, valueMember, displayMember, caption, false) - the trailing false is
       insertDefaultRow, so NO blank row is inserted. The combo simply starts empty because
       Reset()/Load set .Text = string.Empty without selecting anything. Reproduced with a blank
       first option that carries no value. */
    function bindDDL(sel, rows, valueMember, displayMember) {
        sel.innerHTML = '';
        var blank = document.createElement('option');
        blank.value = ''; blank.textContent = '';
        sel.appendChild(blank);
        (rows || []).forEach(function (r) {
            var o = document.createElement('option');
            o.value = esc(pick(r, valueMember));
            o.textContent = esc(pick(r, displayMember));
            sel.appendChild(o);
        });
        sel.value = '';
    }

    function pick(row, key) {
        if (row == null) { return ''; }
        if (Object.prototype.hasOwnProperty.call(row, key)) { return row[key]; }
        var lower = String(key).toLowerCase();
        for (var k in row) {
            if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === lower) { return row[k]; }
        }
        return '';
    }

    /* ---------------------------------------------------------------------- Reset()
       The desktop clears the two combos and refills the grid. It deliberately does NOT clear
       RecId and does NOT swap Update back to Save - only RetrivedData moves those. Copied as-is,
       so the record badge stays visible until the page is reloaded, exactly like the form. */
    function Reset() {
        $('cmbCompany').value = '';
        $('cmbReportHeader').value = '';
        return grdFill();
    }

    /* ---------------------------------------------------------------------- grdFill() */
    function grdFill() {
        return api('/configurations/report-allocate-to-company/api/load').then(function (b) {
            reports = b.reports || [];
            token = b.token || token;
            bindDDL($('cmbCompany'), b.companies, 'Id', 'CompName');
            bindDDL($('cmbReportHeader'), b.headers, 'ReportHeaderId', 'HeaderPrefix');
            checked = {};
            buildHead();
            applyFilter();
        });
    }

    function api(url, opts) {
        return fetch(url, Object.assign({ credentials: 'same-origin' }, opts || {}))
            .then(function (r) { return r.json().then(function (b) { return { ok: r.ok, b: b }; }); })
            .then(function (x) {
                if (!x.ok || x.b.success === false) {
                    throw new Error(x.b && x.b.message ? x.b.message : 'Request failed.');
                }
                return x.b;
            });
    }

    /* grdSetting(): Columns["Select"].ActAsSelector = true, UseHeaderSelector = true -> the
       header cell carries a checkbox that ticks every row currently shown. */
    function buildHead() {
        var h = $('grdHead'), f = $('grdFilter');
        h.innerHTML = ''; f.innerHTML = '';

        var th = document.createElement('th');
        th.className = 'sel';
        var all = document.createElement('input');
        all.type = 'checkbox'; all.id = 'selAll';
        all.addEventListener('change', function () {
            view.forEach(function (r) { checked[pick(r, 'ReportConfigId')] = all.checked; });
            paint();
        });
        th.appendChild(all);
        h.appendChild(th);
        f.appendChild(document.createElement('th')).className = 'sel';

        COLS.forEach(function (c) {
            var t = document.createElement('th');
            t.textContent = c.t;
            h.appendChild(t);
            var ft = document.createElement('th');
            var i = document.createElement('input');
            i.setAttribute('data-k', c.k);
            i.addEventListener('input', applyFilter);
            ft.appendChild(i);
            f.appendChild(ft);
        });
    }

    function applyFilter() {
        var terms = {};
        $('grdFilter').querySelectorAll('input').forEach(function (i) {
            var v = i.value.trim().toLowerCase();
            if (v) { terms[i.getAttribute('data-k')] = v; }
        });
        view = reports.filter(function (r) {
            for (var k in terms) {
                if (esc(pick(r, k)).toLowerCase().indexOf(terms[k]) < 0) { return false; }
            }
            return true;
        });
        paint();
    }

    function paint() {
        var b = $('grdBody');
        b.innerHTML = '';
        var on = 0;
        view.forEach(function (r) {
            var id = pick(r, 'ReportConfigId');
            var tr = document.createElement('tr');

            var td = document.createElement('td');
            td.className = 'sel';
            var cb = document.createElement('input');
            cb.type = 'checkbox';
            cb.checked = !!checked[id];
            if (cb.checked) { on++; }
            cb.addEventListener('change', function () {
                checked[id] = cb.checked;
                updateCount();
            });
            td.appendChild(cb);
            tr.appendChild(td);

            COLS.forEach(function (c) {
                var cell = document.createElement('td');
                cell.className = c.type;
                cell.textContent = esc(pick(r, c.k));
                tr.appendChild(cell);
            });
            b.appendChild(tr);
        });
        var all = $('selAll');
        if (all) { all.checked = view.length > 0 && on === view.length; }
        updateCount();
    }

    function checkedIds() {
        return Object.keys(checked).filter(function (k) { return checked[k]; })
                     .map(function (k) { return parseInt(k, 10); })
                     .filter(function (n) { return n > 0; });
    }

    function updateCount() {
        $('grdCount').textContent = view.length + ' report(s), ' + checkedIds().length + ' checked';
    }

    /* ---------------------------------------------------------------------- Insert()
       btnsave_Click sets RecId = 0 first, so Save always inserts one row per checked report.
       btnupdate_Click does NOT, so an update sends the SAME CompanyReportId for every checked
       row and the record keeps only the last one. The desktop does this silently; here the
       confirm says so first. Nothing is refused and nothing written differs. */
    function Insert(btn) {
        if (busy) { return Promise.resolve(); }

        var ids = checkedIds();
        if (ids.length === 0) {
            return messageBox('Please Check the Reports to save');
        }

        var ask = RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?';
        if (RecId > 0 && ids.length > 1) {
            ask += '\n\nWarning: the desktop sends the same CompanyReportId for every checked '
                 + 'report on an update, so record ' + RecId + ' will be written ' + ids.length
                 + ' times and keep only the last one.';
        }

        return messageBox(ask, ['Yes', 'No']).then(function (a) {
            if (a !== 'Yes') { return; }
            setBusy(btn, true);
            return api('/configurations/report-allocate-to-company/api/save', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    token: token,
                    id: RecId,
                    companyId: $('cmbCompany').value,
                    reportHeaderId: $('cmbReportHeader').value,
                    reportConfigIds: ids
                })
            }).then(function (b) {
                history = b.rows || history;
                if (tab === 'history') { paintHistory(); }
                var text = b.message + (b.warning ? '\n\n' + b.warning : '');
                return messageBox(text).then(function () { return Reset(); });
            });
        }).catch(function (e) {
            return messageBox(e.message);
        }).then(function () {
            setBusy(btn, false);
        });
    }

    /* ---------------------------------------------------------------- RetrivedData(Id)
       The desktop sets RecId, moves to the Form tab and swaps Save for Update - and fills
       NOTHING. Neither combo is set and no row is ticked. Reproduced: the record id is shown so
       it is at least visible which record an Update would write to. */
    function RetrivedData(id) {
        return api('/configurations/report-allocate-to-company/api/' + encodeURIComponent(id))
            .then(function () {
                RecId = parseInt(id, 10) || 0;
                UpdateMode = true;
                $('btnsave').style.display = 'none';
                $('btnupdate').style.display = '';
                $('recBadge').textContent = 'Update mode - CompanyReportId ' + RecId
                        + ' (the desktop fills no field here)';
                showTab('form');
            })
            .catch(function (e) { return messageBox(e.message); });
    }

    /* ------------------------------------------------------------------- History grid */
    function loadHistory() {
        return api('/configurations/report-allocate-to-company/api/history').then(function (b) {
            history = b.rows || [];
            paintHistory();
        }).catch(function (e) { return messageBox(e.message); });
    }

    /* Sp_CompanyReport_GetAllMethod's column list is not known from the C# - the desktop binds
       the DataTable straight to the grid. Columns are therefore taken from the first row rather
       than invented. */
    function paintHistory() {
        hcols = history.length ? Object.keys(history[0]) : [];
        var h = $('hstHead'), f = $('hstFilter');
        h.innerHTML = ''; f.innerHTML = '';
        hcols.forEach(function (k) {
            h.appendChild(document.createElement('th')).textContent = k;
            var ft = document.createElement('th');
            var i = document.createElement('input');
            i.setAttribute('data-k', k);
            i.addEventListener('input', paintHistoryRows);
            ft.appendChild(i);
            f.appendChild(ft);
        });
        paintHistoryRows();
    }

    function paintHistoryRows() {
        var terms = {};
        $('hstFilter').querySelectorAll('input').forEach(function (i) {
            var v = i.value.trim().toLowerCase();
            if (v) { terms[i.getAttribute('data-k')] = v; }
        });
        hview = history.filter(function (r) {
            for (var k in terms) {
                if (esc(r[k]).toLowerCase().indexOf(terms[k]) < 0) { return false; }
            }
            return true;
        });

        var b = $('hstBody');
        b.innerHTML = '';
        hview.forEach(function (r) {
            var tr = document.createElement('tr');
            hcols.forEach(function (k) {
                var td = document.createElement('td');
                if (k.toLowerCase() === 'companyreportid') {
                    var a = document.createElement('a');
                    a.className = 'code'; a.textContent = esc(r[k]);
                    a.addEventListener('click', function (e) { e.stopPropagation(); RetrivedData(r[k]); });
                    td.appendChild(a);
                } else {
                    td.className = 'wrap';
                    td.textContent = esc(r[k]);
                }
                tr.appendChild(td);
            });
            /* grdhistory_DoubleClick -> RetrivedData(CurrentRow.Cells["CompanyReportId"]) */
            tr.addEventListener('dblclick', function () { RetrivedData(pick(r, 'CompanyReportId')); });
            b.appendChild(tr);
        });
        $('hstCount').textContent = hview.length + ' row(s)';
    }

    /* -------------------------------------------- tabControl1_SelectedIndexChanged
       The desktop handler's body is `_ = tabControl1.SelectedIndex; _ = 1;` - it does nothing.
       Unlike frmReportConfig it does NOT refill on tab change, so the History grid is loaded
       once when the tab is first opened rather than on every switch. */
    var historyLoaded = false;
    function showTab(name) {
        tab = name;
        document.querySelectorAll('#tabstrip .tab').forEach(function (t) {
            t.classList.toggle('on', t.getAttribute('data-tab') === name);
        });
        $('formPane').style.display = name === 'form' ? '' : 'none';
        $('historyPane').style.display = name === 'history' ? '' : 'none';
        if (name === 'history' && !historyLoaded) { historyLoaded = true; loadHistory(); }
    }

    /* --------------------------------------------------------------------- wiring */
    document.querySelectorAll('#tabstrip .tab').forEach(function (t) {
        t.addEventListener('click', function () { showTab(t.getAttribute('data-tab')); });
    });

    $('btnnew').addEventListener('click', function () { Reset(); });
    $('btnsave').addEventListener('click', function () { RecId = 0; Insert($('btnsave')); });
    $('btnupdate').addEventListener('click', function () { Insert($('btnupdate')); });
    /* btnPrint, btnRefresh, toolStripButton1 and toolStripButton4 have no Click handler in the
       desktop designer. They stay inert here rather than being given invented behaviour. */

    $('grdFull').addEventListener('click', function () { $('grdWrap').classList.toggle('full'); });
    $('hstFull').addEventListener('click', function () { $('historyWrap').classList.toggle('full'); });

    /* frmCompanyReport_KeyDown, condition for condition. */
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Enter' && !(e.target && e.target.tagName === 'BUTTON')) {
            e.preventDefault();
            var f = Array.prototype.slice.call(
                document.querySelectorAll('select, input:not([type=checkbox]), button'))
                .filter(function (el) { return !el.disabled && el.offsetParent !== null; });
            var i = f.indexOf(e.target);
            if (i >= 0 && i + 1 < f.length) { f[i + 1].focus(); }
            return;
        }
        var save = $('btnsave'), upd = $('btnupdate');
        if (save.style.display !== 'none' && !save.disabled && tab === 'form' && e.ctrlKey && e.key.toLowerCase() === 's') {
            e.preventDefault(); RecId = 0; Insert(save);
        }
        if (e.ctrlKey && e.key.toLowerCase() === 'n' && tab === 'form') { e.preventDefault(); Reset(); }
        if ((e.ctrlKey && e.key.toLowerCase() === 'e') || e.key === 'Escape') { window.close(); }
        if (upd.style.display !== 'none' && !upd.disabled && tab === 'form'
            && e.ctrlKey && e.key.toLowerCase() === 'u' && UpdateMode) {
            e.preventDefault(); Insert(upd);
        }
    });

    /* frmCompanyReport_Load: grdFill(), CompanyFill(), ReportHeaderFill(), cmbCompany.Focus(). */
    grdFill().then(function () { $('cmbCompany').focus(); })
             .catch(function (e) { return messageBox(e.message); });

})();
