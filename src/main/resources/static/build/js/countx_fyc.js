/* countx_fyc.js - Architecture.WinApp.Account_Definition.frmFinancialYearClose (399), Admin Panel hub / System Admin.
   Handlers follow the desktop: btnShow_Click, btnSave_Click, btnUpdate_Click, Insert (check order, the two Yes/No
   confirmations, the messages), Reset, btnNew_Click, tabControl1_SelectedIndexChanged -> BindHistory, the Edit column
   of GridHistory -> GetDataById. The server repeats the checks and re-reads the rows; nothing sent here is trusted. */
(function () {
    'use strict';
    var API = '/api/adm-panel/fyc';
    var recId = 0;                 // RecId
    var configured = true;         // UnAppropriatedProfitAndLossAccount <> 0
    var gridRows = [];             // what grd holds
    var editMode = false;          // GetDataById shows only AccountTitle/Debit/Credit (grdSettings(Value: true))

    function $id(id) { return document.getElementById(id); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function box(m) { window.alert(m); }
    function ask(m) { return window.confirm(m); }
    function parse(r) {
        return r.text().then(function (t) {
            var b = null;
            try { b = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
            if (!r.ok) throw new Error((b && b.message) || ('Request failed (' + r.status + ')'));
            return b;
        });
    }
    function getJson(url) { return fetch(url, { headers: { 'Accept': 'application/json' }, credentials: 'same-origin' }).then(parse); }
    function postJson(url, data) {
        var h = { 'Accept': 'application/json', 'Content-Type': 'application/json' };
        var t = document.querySelector('meta[name="_csrf"]'), n = document.querySelector('meta[name="_csrf_header"]');
        if (t && n && t.getAttribute('content') && n.getAttribute('content')) h[n.getAttribute('content')] = t.getAttribute('content');
        return fetch(url, { method: 'POST', headers: h, credentials: 'same-origin', body: JSON.stringify(data) }).then(parse);
    }
    function fail(e) { box(e && e.message ? e.message : String(e)); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function fmt(v) {                       // ToString("##0,##.##")
        var n = Number(v) || 0;
        if (n === 0) return '0';
        return n.toLocaleString('en-US', { maximumFractionDigits: 2 });
    }
    function money(v) { return fmt(v); }
    function today() { var d = new Date(), m = d.getMonth() + 1, dd = d.getDate(); return d.getFullYear() + '-' + (m < 10 ? '0' : '') + m + '-' + (dd < 10 ? '0' : '') + dd; }
    function refreshCombos() { if (window.DesktopCombo) window.DesktopCombo.refresh(); }

    // ---------------------------------------------------------------- grd

    var COLS_RUN = [   // grdSettings(Value:false): AccountId + Account_Level hidden; widths from the form
        ['AccountCode', 'AccountCode', 90, ''], ['AccountTitle', 'AccountTitle', 300, ''], ['AccountType', 'AccountType', 150, ''],
        ['NoteTitle', 'NoteTitle', 200, ''], ['Debit', 'Debit', 120, 'fyc-num'], ['Credit', 'Credit', 120, 'fyc-num']];
    var COLS_EDIT = [['AccountTitle', 'AccountTitle', 300, ''], ['Debit', 'Debit', 120, 'fyc-num'], ['Credit', 'Credit', 120, 'fyc-num']];

    function drawGrid() {
        var cols = editMode ? COLS_EDIT : COLS_RUN, t = $id('grd'), h = '<tr>', b = '';
        if (!gridRows.length) { t.querySelector('thead').innerHTML = ''; t.querySelector('tbody').innerHTML = ''; return; }
        cols.forEach(function (c) { h += '<th style="width:' + c[2] + 'px;">' + esc(c[1]) + '</th>'; });
        gridRows.forEach(function (r) {
            b += '<tr>';
            cols.forEach(function (c) {
                var v = r[c[0]];
                b += '<td class="' + c[3] + '">' + esc(c[3] ? money(v) : v) + '</td>';
            });
            b += '</tr>';
        });
        t.querySelector('thead').innerHTML = h + '</tr>';
        t.querySelector('tbody').innerHTML = b;
        t.style.width = cols.reduce(function (a, c) { return a + c[2]; }, 0) + 'px';
    }
    function clearGrid() { gridRows = []; drawGrid(); }

    // ---------------------------------------------------------------- buttons visible state

    function setEditing(on) { show('btnSave', !on); show('btnUpdate', on); }

    // ---------------------------------------------------------------- load / Reset

    function bindBranches(rows, defId) {
        var sel = $id('cmbBranchName'), html = '<option value="0"></option>';
        (rows || []).forEach(function (r) { html += '<option value="' + esc(r.BranchId) + '">' + esc(r.BranchName) + '</option>'; });
        sel.innerHTML = html;
        var ok = false;
        for (var i = 0; i < sel.options.length; i++) if (sel.options[i].value === String(defId)) { sel.selectedIndex = i; ok = true; }
        if (!ok) sel.value = '0';
        refreshCombos();
    }
    function loadInit() {
        return getJson(API + '/init').then(function (r) {
            configured = !!r.unAppropriatedConfigured;
            if (recId === 0) $id('txtdocno').value = r.docNo || '';
            bindBranches(r.branches, r.defaultBranchId);
            return r;
        }).catch(function (e) { box('Error occurred during database call.'); throw e; });
    }
    function reset() {                       // Reset()
        $id('txtdocno').value = '';
        $id('txtPlAmount').value = '';
        clearGrid();
        setEditing(false);
        recId = 0; editMode = false;
        return getJson(API + '/init').then(function (r) { $id('txtdocno').value = r.docNo || ''; configured = !!r.unAppropriatedConfigured; }).catch(fail);
    }

    // ---------------------------------------------------------------- btnShow_Click

    function onShow() {
        postJson(API + '/show', { closingDate: $id('ClosingDate').value, branchId: parseInt($id('cmbBranchName').value, 10) || 0 })
            .then(function (r) {
                if (r.rows && r.rows.length > 0) {
                    editMode = false; gridRows = r.rows; drawGrid();
                    $id('txtPlAmount').value = fmt(r.plAmount);
                } else {
                    clearGrid(); $id('txtPlAmount').value = '0';
                }
            }).catch(fail);
    }

    // ---------------------------------------------------------------- Insert()

    function insert() {
        if ($id('txtdocno').value === '') { box('Document No Is Required'); $id('txtdocno').focus(); return; }
        if (!configured) { box('Un-Appropriated Profit And Loss Account Configuration Not Found'); return; }
        if (recId > 0) { if (!ask('Are you sure to Update?')) return; }
        else if (!ask('Are you sure to Save?')) return;
        if (!gridRows.length) { box('Grid Record Not Found'); return; }
        var wasUpdate = recId > 0;
        postJson(API + '/save', {
            id: recId, docNo: $id('txtdocno').value, closingDate: $id('ClosingDate').value,
            branchId: parseInt($id('cmbBranchName').value, 10) || 0, confirmed: true
        }).then(function (r) {
            box(r.message);
            $id('txtPlAmount').value = '';
            clearGrid(); setEditing(false); recId = 0; editMode = false;
            $id('txtdocno').value = r.docNo || '';
        }).catch(fail);
    }
    function onSave() { recId = 0; insert(); }
    function onUpdate() {
        if (recId === 0) { box('RecordId not found'); return; }
        insert();
    }

    // ---------------------------------------------------------------- tabs / history

    function selectTab(i) {
        $id('tabClosing').classList.toggle('is-active', i === 0);
        $id('tabHistory').classList.toggle('is-active', i === 1);
        $id('pageClosing').classList.toggle('is-active', i === 0);
        $id('pageHistory').classList.toggle('is-active', i === 1);
        bindHistory();                       // tabControl1_SelectedIndexChanged
    }
    function bindHistory() {
        getJson(API + '/history').then(function (r) {
            var rows = r.rows || [];
            if (!rows.length) return;        // desktop leaves the grid untouched when nothing is returned
            var t = $id('GridHistory'), h = '<tr><th style="width:40px;">Edit</th><th style="width:110px;">VoucherCode</th><th style="width:110px;">VoucherDate</th>'
                + '<th style="width:120px;">VoucherAmount</th><th style="width:110px;">NoOfAttachments</th></tr>', b = '';
            rows.forEach(function (x) {
                b += '<tr><td class="fyc-center"><button type="button" class="fyc-editbtn" data-id="' + esc(x.Id) + '">Edit</button></td>'
                    + '<td>' + esc(x.VoucherCode) + '</td><td>' + esc(x.VoucherDate) + '</td><td>' + esc(x.VoucherAmount) + '</td>'
                    + '<td class="fyc-center"><input type="checkbox" disabled' + (x.NoOfAttachments > 0 ? ' checked' : '') + '></td></tr>';
            });
            t.querySelector('thead').innerHTML = h;
            t.querySelector('tbody').innerHTML = b;
            t.style.width = '490px';
        }).catch(fail);
    }
    function getDataById(id) {
        getJson(API + '/get?id=' + encodeURIComponent(id)).then(function (r) {
            selectTab(0);
            recId = r.id; setEditing(true);
            $id('txtdocno').value = r.docNo;
            if (r.closingDate) $id('ClosingDate').value = r.closingDate;
            editMode = true; gridRows = r.rows || []; drawGrid();
            $id('txtPlAmount').value = fmt(r.plAmount);
        }).catch(fail);
    }

    // ---------------------------------------------------------------- wiring

    document.addEventListener('DOMContentLoaded', function () {
        $id('ClosingDate').value = today();
        $id('btnNew').addEventListener('click', function () { reset(); });
        $id('btnShow').addEventListener('click', onShow);
        $id('btnSave').addEventListener('click', onSave);
        $id('btnUpdate').addEventListener('click', onUpdate);
        $id('btnattachment').addEventListener('click', function () {
            box('Attachments are not available in the web version of this screen yet.');
        });
        $id('tabClosing').addEventListener('click', function () { selectTab(0); });
        $id('tabHistory').addEventListener('click', function () { selectTab(1); });
        $id('GridHistory').addEventListener('click', function (e) {
            var b = e.target.closest ? e.target.closest('button[data-id]') : null;
            if (b) getDataById(b.getAttribute('data-id'));
        });
        // New / Update / Save shortcuts as the &-mnemonics (Alt+N, Alt+U, Alt+S, Alt+A)
        document.addEventListener('keydown', function (e) {
            if (!e.altKey) return;
            var k = (e.key || '').toLowerCase(), map = { n: 'btnNew', r: 'btnRefresh', u: 'btnUpdate', s: 'btnSave', a: 'btnattachment' };
            if (map[k] && $id(map[k]) && !$id(map[k]).classList.contains('is-hidden')) { e.preventDefault(); $id(map[k]).click(); }
        });
        loadInit().then(function () { var inp = document.querySelector('#cmbBranchName ~ .dtcombo-input, .dtcombo-wrap .dtcombo-input'); if (inp) inp.focus(); }).catch(function () {});
    });
})();
