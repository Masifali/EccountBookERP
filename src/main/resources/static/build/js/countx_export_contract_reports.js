/* ============================================================================================
 * countx_export_contract_reports.js - ExportContractReports.cs "Export Contract Reports" (tab launcher, no ScreenDefinition row).
 * /api/export/contract-reports/tabs returns the tabs ExportContractReports_Load would add from ScreenViewReights.
 * "Contract Register" embeds /export/contract-register (ExImSaleContractRegister, screen 251). "Contracts Current Status"
 * (frmContractCurrentStatusReport) has no web port: its tab says so.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptO, $id = H.$id, box = H.box;
    var T = null;
    function build(tabs) {
        var strip = $id('tabControl1'), panels = $id('tabPanels'), sh = '', ph = '';
        tabs.forEach(function (t, i) {
            sh += '<button type="button" class="win-tab' + (i === 0 ? ' is-active' : '') + '" data-tab="ecrTab' + i + '">' + H.esc(t.title) + '</button>';
            ph += '<div class="win-tab-panel' + (i === 0 ? ' is-active' : '') + '" id="ecrTab' + i + '">'
                + (t.url ? '<iframe class="rpto-embed" title="' + H.esc(t.title) + '" data-src="' + H.esc(t.url) + '"></iframe>'
                         : '<div class="rpto-empty-tabs">' + H.esc(t.form) + ' is not available on the web yet.</div>')
                + '</div>';
        });
        strip.innerHTML = sh; panels.innerHTML = ph;
        T = H.tabs('tabControl1', function (i) { loadFrame(i); });
        loadFrame(0);
    }
    /* the embedded form is created once (TopLevel = false, Visible = true): load each iframe on first show */
    function loadFrame(i) {
        var f = document.querySelector('#ecrTab' + i + ' iframe');
        if (f && !f.getAttribute('src')) f.setAttribute('src', f.getAttribute('data-src'));
    }
    function load() {
        return H.getJson('/api/export/contract-reports/tabs').then(function (d) {
            var tabs = (d && d.tabs) || [];
            if (!tabs.length) {
                /* ExportContractReports_Load returns without adding a tab: the desktop shows an empty TabControl */
                var n = $id('noTabs'); n.classList.remove('is-hidden');
                n.textContent = (d && d.moduleRight) ? 'No contract report is assigned to this user (SaleContractHistory / frmContractCurrentStatusReport).'
                    : 'No "Export Reports" rights for this user.';
                return;
            }
            build(tabs);
        }).catch(function (e) { box(e.message); });
    }
    document.addEventListener('DOMContentLoaded', function () {
        document.addEventListener('keydown', function (e) {
            if (e.key === 'Enter' && H.baseKeys(e, null)) return;          /* Enter = {TAB}; the form has no Esc / Ctrl+E close */
            /* ShiftToNextTab: Ctrl+Right, (SelectedIndex + 1) % Count when there is more than one tab */
            if (e.ctrlKey && e.key === 'ArrowRight' && T && T.count > 1 && !/^(INPUT|SELECT|TEXTAREA)$/.test((e.target || {}).tagName || '')) {
                e.preventDefault(); var n = (T.index() + 1) % T.count; T.select(n);
            }
        });
        load();
    });
}(window));
