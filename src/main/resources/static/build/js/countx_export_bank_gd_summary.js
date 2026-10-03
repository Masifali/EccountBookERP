/* ============================================================================================
 * countx_export_bank_gd_summary.js - BankGdSummary.cs, screen 272 "Bank Gd Summary".
 * Data: /api/export/bank-gd-summary (USP_BankGdsSummary). Print 289 through CrystalPrint key "exp-289-bankgd"
 * (ExportReportsNPrints). GdNo link -> frmGDBreakUp popup (USP_GetGdBreakUpsByGdId).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptA, N = global.ExportRptN, $id = H.$id, box = H.box;
    var API = '/api/export/bank-gd-summary';
    var S = { rows: [], dec: 0, lastBalance: 0 };

    function single(v) { return N.fmtSingle(v, S.dec); }
    function cols() { /* GridFill dtcol + grdsetting (stringFormatsingle on the amounts; Mtons is a text column) */
        return [
            { key: 'GdId', hidden: true }, { key: 'DocumentTypeId', hidden: true },
            { key: 'CustomerName', caption: 'CustomerName', width: 300 }, { key: 'InvoiceNo', caption: 'InvoiceNo', width: 120 },
            { key: 'InvoiceDate', caption: 'InvoiceDate', width: 120 },
            { key: 'InvoiceAmount', caption: 'InvoiceAmount', num: true, sum: true, fmt: single, width: 150 },
            { key: 'Mtons', caption: 'Mtons', num: true, width: 120 },
            { key: 'GdNo', caption: 'GdNo', link: true, width: 220 },
            { key: 'GdAmount', caption: 'GdAmount', num: true, sum: true, fmt: single, width: 120 },
            { key: 'CommAmount', caption: 'CommAmount', num: true, sum: true, fmt: single, width: 120 },
            { key: 'RealizedAmount', caption: 'RealizedAmount', num: true, sum: true, fmt: single, width: 120 },
            { key: 'CommRealizeTotal', caption: 'CommRealizeTotal', num: true, sum: true, fmt: single, width: 120 },
            { key: 'GdBalance', caption: 'GdBalance', num: true, sum: true, fmt: single, width: 150 }
        ];
    }

    /** btnShow_Click -> GridFill: Amount = Conversion.ToDouble(txtzerBalance.Text); rows -> grid, none -> ClearStructure. */
    function gridFill() {
        var bal = H.netD(H.val('txtzerBalance'));
        return H.postJson(API + '/show', { fromBalance: bal }).then(function (d) {
            S.rows = (d && d.rows) || [];
            S.lastBalance = bal;
            if (S.rows.length) H.drawGrid('grdfrm', cols(), S.rows, {});
            else { var t = $id('grdfrm'); t.querySelector('thead').innerHTML = ''; H.clearGrid('grdfrm'); }
            H.show('grdfrmEmpty', !S.rows.length);
        }).catch(function (e) { box(e.message); });
    }
    function show(btn) { return H.busy(btn || 'btnShow', gridFill); }
    /** btnnew_Click -> Reset: txtzerBalance cleared (the grid stays as it is). */
    function btnNew() { H.setText('txtzerBalance', ''); }
    /** btnPrint_Click: "Not Record Found For Display" while dtGrid is empty; 289 prints the last GridFill's query. */
    function print(btn) {
        if (!S.rows.length) { box('Not Record Found For Display'); return; }
        return H.print('exp-289-bankgd', N.args({ fromToBalance: S.lastBalance }), btn || $id('btnPrint'));
    }
    function shortcuts() {
        H.shortcuts([['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on From Balance'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus On Zero Balance'],
            ['Ctrl+Enter', 'When Focus On Any Grid For Update Record']]);
    }
    /** grdfrm_LinkClicked GdNo -> frmGDBreakUp { GdId, DocumentTypeId }. */
    function link(key, i) { var r = S.rows[i]; if (r && key === 'GdNo') N.gdBreakUp(H.netI(r.GdId), H.netI(r.DocumentTypeId)); }

    document.addEventListener('DOMContentLoaded', function () {
        N.embedFooter('bgdFooter');
        H.fullscreenButtons();
        var t = $id('txtzerBalance');
        /* textBox1_KeyPress = OnlytextNumberFunction (digits and control keys); ShortcutsEnabled = false (no paste). */
        t.addEventListener('keypress', function (e) { if (e.key && e.key.length === 1 && !/\d/.test(e.key)) e.preventDefault(); });
        t.addEventListener('paste', function (e) { e.preventDefault(); });
        H.gridEvents('grdfrm', { link: link, ctrlSpace: function (i) { link('GdNo', i); } });
        document.addEventListener('keydown', function (e) {
            if (H.baseKeys(e, shortcuts)) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'p') { e.preventDefault(); print(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); show(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew(); }
            else if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); H.focus('txtzerBalance'); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grdfrm'); }
        });
        /* BankGdSummary_Load: focus txtzerBalance, btnShow_Click. */
        H.focus('txtzerBalance');
        H.getJson(API + '/setup').then(function (d) { S.dec = H.netI(d && d.decimalsAmount); })
            .catch(function (e) { box(e.message); })
            .then(function () { return show($id('btnShow')); });
    });
    global.ExportBgd = { btnNew: btnNew, show: show, print: print, shortcuts: shortcuts };
}(window));
