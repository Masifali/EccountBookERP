/* ============================================================================================
 * countx_adm_org.js - Architecture.WinApp.Templates.DefineOrgnization (ScreenDefinition 402)
 * Each handler follows the desktop method: validation wording and order, the Yes/No confirmations, the messages,
 * what New / Save / Update / Edit / double-click do to the buttons. The server repeats every check.
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/adm-org';
    var recId = 0, updateMode = false, hist = [], cur = -1;

    var COLS = ['OrgCode', 'OrgName', 'OrgContactPerson', 'OrgEmailA', 'OrgEmailB', 'OrgTel', 'OrgMobileA', 'OrgMobileB', 'OrgMobileC',
        'OrgAddress', 'OrgReportingTitle', 'OrgCountry', 'OrgState', 'OrgBaseCurr', 'AllowedLocationsCount', 'ReportingRemarks'];

    /** FormValidation 252: control id, message (desktop order). */
    var REQ = [
        ['txtCode', 'Organization Code Field Required'], ['txtName', 'Organization Name Field Required'],
        ['txtReportingTitle', 'Reporting Title Field Required'], ['txtContactPerson', 'Contact Person Field Required'],
        ['txtEmail', 'Email Field Required'], ['txtEmail02', 'Email II Field Required'], ['txtAdress', 'Address Field Required'],
        ['txtPhoneNumber', 'Phone Number Field Required'], ['txtMobile01', 'Mobile 01 Field Required'],
        ['txtMobile02', 'Mobile 02 Field Required'], ['txtMobile03', 'Mobile 03 Field Required'],
        ['txtCountry', 'Country Field Required'], ['txtState', 'State Field Required'],
        ['txtReportingRemarks', 'Reporting Remarks Field Required']];

    /** Reset 200 clears these; txtCompanyCount and CmbTemplate are not touched. */
    var RESET = ['txtCode', 'txtName', 'txtReportingTitle', 'txtContactPerson', 'txtEmail', 'txtEmail02', 'txtAdress', 'txtPhoneNumber',
        'txtMobile01', 'txtMobile02', 'txtMobile03', 'txtCountry', 'txtState', 'txtReportingRemarks'];

    function $id(id) { return document.getElementById(id); }
    function box(m) { window.alert(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = v === null || v === undefined ? '' : String(v); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function visible(id) { var e = $id(id); return !!e && !e.classList.contains('is-hidden') && !e.disabled; }
    function focus(id) { var e = $id(id); if (e) e.focus(); }
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
    function refreshCombos() { if (window.DesktopCombo) window.DesktopCombo.refresh(); }

    // ------------------------------------------------------------------ TemplateNameFill 170
    /** BindDDLNew(dt, CmbTemplate, "Id", "TemplateName", "Template Name", true); the selection is kept when the id is still listed. */
    function templateFill() {
        return getJson(API + '/templates').then(function (rows) {
            if (!rows || !rows.length) return;                      // dt.Rows.Count <= 0 -> return
            var sel = $id('CmbTemplate'), keep = parseInt(sel.value, 10) || 0;
            var html = '<option value="0">...Select Any Value...</option>';
            rows.forEach(function (r) { html += '<option value="' + esc(r.Id) + '">' + esc(r.TemplateName) + '</option>'; });
            sel.innerHTML = html;
            sel.value = '0';
            if (keep > 0) setTemplate(keep);
            refreshCombos();
        });
    }
    function setTemplate(id) {
        var s = $id('CmbTemplate'), x = String(id);
        for (var i = 0; i < s.options.length; i++) if (s.options[i].value === x) { s.selectedIndex = i; refreshCombos(); return; }
        s.value = '0';
        refreshCombos();
    }

    // ------------------------------------------------------------------ HistoryGridFill 476 / GridSetting 521
    function head() {
        var h = '<tr><th style="width:40px;">Edit</th>';
        COLS.forEach(function (c) { h += '<th>' + esc(c) + '</th>'; });
        $id('grdHistory').querySelector('thead').innerHTML = h + '</tr>';
    }
    function render() {
        var html = [];
        hist.forEach(function (r, i) {
            var t = '<tr data-i="' + i + '"' + (i === cur ? ' class="is-current"' : '') + '><td class="ap-center"><button type="button" class="ap-editbtn" data-edit="' + i + '">Edit</button></td>';
            COLS.forEach(function (c) {
                var v = r[c];
                t += '<td' + (c === 'AllowedLocationsCount' ? ' class="ap-num"' : '') + ' title="' + esc(v) + '">' + esc(v) + '</td>';
            });
            html.push(t + '</tr>');
        });
        $id('grdHistory').querySelector('tbody').innerHTML = html.join('');
    }
    function historyFill() {
        return getJson(API + '/history').then(function (rows) {
            hist = rows || [];
            cur = hist.length ? 0 : -1;
            if (hist.length) head(); else $id('grdHistory').querySelector('thead').innerHTML = '';   // grd.ClearStructure()
            render();
        });
    }

    // ------------------------------------------------------------------ Reset 200 / ReadById 397
    function reset() {
        RESET.forEach(function (id) { setVal(id, ''); });
        show('btnsave', true);
        show('btnupdate', false);
        return historyFill();
    }
    function readById(id) {
        recId = id;
        return getJson(API + '/read?id=' + encodeURIComponent(id)).then(function (o) {
            recId = o.id;
            setVal('txtCode', o.code); setVal('txtName', o.name); setVal('txtReportingTitle', o.reportingTitle);
            setVal('txtContactPerson', o.contactPerson); setVal('txtEmail', o.email); setVal('txtEmail02', o.email02);
            setVal('txtAdress', o.address); setVal('txtReportingRemarks', o.remarks); setVal('txtPhoneNumber', o.phone);
            setVal('txtMobile01', o.mobile01); setVal('txtMobile02', o.mobile02); setVal('txtMobile03', o.mobile03);
            setVal('txtCountry', o.country); setVal('txtState', o.state); setVal('txtCompanyCount', o.companyCount);
            setTemplate(o.templateId);
            show('btnsave', false);
            show('btnupdate', true);
            updateMode = true;
        }).catch(fail);
    }

    // ------------------------------------------------------------------ Insert 341
    function insert() {
        for (var i = 0; i < REQ.length; i++) {                      // FormValidation 252
            if (val(REQ[i][0]).trim() === '') { box(REQ[i][1]); focus(REQ[i][0]); return; }
        }
        if (recId > 0) { if (!window.confirm('Are you sure to Update?')) return; }
        else if (!window.confirm('Are you sure to Save?')) return;
        var body = {
            id: recId, code: val('txtCode'), name: val('txtName'), reportingTitle: val('txtReportingTitle'),
            contactPerson: val('txtContactPerson'), email: val('txtEmail'), email02: val('txtEmail02'), address: val('txtAdress'),
            remarks: val('txtReportingRemarks'), phone: val('txtPhoneNumber'), mobile01: val('txtMobile01'),
            mobile02: val('txtMobile02'), mobile03: val('txtMobile03'), country: val('txtCountry'), state: val('txtState'),
            companyCount: val('txtCompanyCount'), templateId: parseInt(val('CmbTemplate'), 10) || 0
        };
        busy(function () {
            return postJson(API + '/save', body).then(function (r) {
                box(r && r.message ? r.message : (recId > 0 ? 'Update Successfully' : 'Save Successfully'));
                return reset();
            }).catch(fail);
        });
    }
    var working = false;
    function busy(fn) {
        if (working) return;
        working = true;
        var done = function () { working = false; };
        var p;
        try { p = fn(); } catch (e) { done(); throw e; }
        if (p && p.then) p.then(done, done); else done();
    }

    // ------------------------------------------------------------------ toolbar
    function btnsave() { recId = 0; insert(); }                    // btnsave_Click 450
    function btnupdate() {                                         // btnupdate_Click 434
        if (recId === 0) { box('RecId Not Found...'); return; }
        insert();
    }
    function btnnew() { reset().catch(fail); }                     // btnnew_Click 228
    function btnrefresh() { templateFill().catch(fail); }          // btnRefresh_Click 240
    function shortcut() {                                          // MakeShortCutKeys 658
        var rows = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Organization Code'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
            ['Ctrl+ArrowUp', 'For Focus On Entry First Colum'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
            ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
        var m = $id('apModal');
        if (!m) {
            m = document.createElement('div'); m.id = 'apModal'; m.className = 'ap-modal';
            m.innerHTML = '<div class="ap-modal-box"><h4></h4><table><tbody></tbody></table><div class="ap-modal-foot"><button type="button" class="ap-teal-btn" style="width:70px;height:24px;">Close</button></div></div>';
            document.body.appendChild(m);
            m.addEventListener('click', function (e) { if (e.target === m || e.target.tagName === 'BUTTON') m.classList.remove('is-on'); });
        }
        m.querySelector('h4').textContent = 'ShortCut Keys';
        m.querySelector('tbody').innerHTML = rows.map(function (r) { return '<tr><td><b>' + esc(r[0]) + '</b></td><td>' + esc(r[1]) + '</td></tr>'; }).join('');
        m.classList.add('is-on');
    }
    function closeForm() {
        window.close();
        setTimeout(function () { if (!window.closed) { if (history.length > 1) history.back(); else location.href = '/admin/define-company'; } }, 150);
    }

    // ------------------------------------------------------------------ grid events (grd_ColumnButtonClick 560, grd_DoubleClick 542, grd_KeyDown 620)
    function rowOf(e) { var tr = e.target.closest('tr[data-i]'); return tr ? parseInt(tr.getAttribute('data-i'), 10) : -1; }
    function select(i) {
        cur = i;
        var b = $id('grdHistory').querySelector('tbody');
        b.querySelectorAll('tr.is-current').forEach(function (x) { x.classList.remove('is-current'); });
        var tr = b.querySelector('tr[data-i="' + i + '"]'); if (tr) tr.classList.add('is-current');
    }
    function editRow(i) { reset().then(function () { return readById(parseInt(hist[i].Id, 10) || 0); }).catch(fail); }

    document.addEventListener('DOMContentLoaded', function () {
        var body = $id('grdHistory').querySelector('tbody');
        body.addEventListener('click', function (e) {
            var i = rowOf(e); if (i < 0) return;
            select(i);
            if (e.target.closest('button[data-edit]')) editRow(i);
        });
        body.addEventListener('dblclick', function (e) {
            var i = rowOf(e); if (i < 0 || e.target.closest('button')) return;
            readById(parseInt(hist[i].Id, 10) || 0);
        });
        $id('grdHistory').parentNode.addEventListener('keydown', function (e) {
            if (cur < 0 || !hist[cur]) return;
            if (e.key === 'ArrowDown' && !e.ctrlKey) { if (cur + 1 < hist.length) { select(cur + 1); e.preventDefault(); } return; }
            if (e.key === 'ArrowUp' && !e.ctrlKey) { if (cur > 0) { select(cur - 1); e.preventDefault(); } return; }
            if (e.ctrlKey && e.key === 'Enter') { readById(parseInt(hist[cur].Id, 10) || 0); e.preventDefault(); }
            if (e.ctrlKey && e.key === ' ') { readById(parseInt(hist[cur].Id, 10) || 0); e.preventDefault(); }   // Edit column current
        });
        // txtCompanyCount_KeyPress 687: digits and control keys only
        $id('txtCompanyCount').addEventListener('keypress', function (e) { if (e.key.length === 1 && !/\d/.test(e.key)) e.preventDefault(); });
        $id('txtCompanyCount').addEventListener('input', function () { var v = this.value.replace(/\D/g, ''); if (v !== this.value) this.value = v; });
        // Enter moves on like SendKeys.Send("{TAB}")
        document.addEventListener('keydown', function (e) {
            var t = e.target;
            if (e.key === 'Enter' && !e.ctrlKey && !e.shiftKey && !e.altKey && t && t.tagName === 'INPUT' && t.type === 'text' && !t.classList.contains('dtcombo-input')) {
                e.preventDefault();
                var all = Array.prototype.filter.call(document.querySelectorAll('input, button, .dtcombo-input'), function (el) {
                    return !el.disabled && el.offsetParent !== null && el.type !== 'hidden' && el.tabIndex !== -1 && el.tagName !== 'BUTTON';
                });
                var k = all.indexOf(t); if (k >= 0 && k + 1 < all.length) all[k + 1].focus();
            }
            // frmCompanyReport_KeyDown 582
            if (e.ctrlKey && e.altKey && (e.key === 'Alt' || e.key === 'Control')) { shortcut(); return; }
            if (e.key === 'Escape' && !document.querySelector('.dtcombo-pop[style*="block"]') && !document.querySelector('.ap-modal.is-on')) { closeForm(); return; }
            if (!e.ctrlKey) return;
            var k2 = e.key.toLowerCase();
            if (k2 === 'e') { e.preventDefault(); closeForm(); }
            else if (k2 === 's') { e.preventDefault(); if (visible('btnsave')) btnsave(); }
            else if (k2 === 'n') { e.preventDefault(); btnnew(); }
            else if (k2 === 'u') { e.preventDefault(); if (visible('btnupdate') && updateMode) btnupdate(); }
            else if (k2 === 'r') { e.preventDefault(); btnrefresh(); }
            else if (e.key === 'F5') { e.preventDefault(); focus('txtCode'); }
            else if (e.key === 'ArrowUp') { e.preventDefault(); focus('txtCode'); }
            else if (e.key === 'ArrowDown') { var g = $id('grdHistory').parentNode; if (g) { e.preventDefault(); g.focus(); } }
        });

        // frmCompanyReport_Load 151
        show('btnsave', true);
        show('btnupdate', false);
        historyFill().then(templateFill).catch(fail);
    });

    window.AO = { newForm: btnnew, save: btnsave, update: btnupdate, refresh: btnrefresh, shortcut: shortcut };
})();
