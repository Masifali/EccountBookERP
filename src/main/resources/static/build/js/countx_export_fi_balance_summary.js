/* countx_export_fi_balance_summary.js - FIBalanceSummary.cs (+ popup FIBalanceDetailByFI), screen 246.
 * Data: /api/export/fi-balance-summary. */
(function (global) {
    'use strict';
    var H = global.ExRptC, API = '/api/export/fi-balance-summary';
    var ROWS = [], CUR = -1, DETAIL = [];
    var COLS = [
        { key: 'Id', hidden: true }, { key: 'EFormNo', cap: 'EForm No', link: true }, { key: 'PaymentTermId', hidden: true }, { key: 'DocumentTypeId', hidden: true },
        { key: 'FcyAmount', cap: 'Fcy Amount', kind: 'amt', sum: true }, { key: 'UtilizeAmountInInvoice', cap: 'Utilize Amount In Invoice', kind: 'amt', sum: true },
        { key: 'AdvanceUtilizeAgainstGDs', cap: 'Advance Utilize Against GDs', kind: 'amt', sum: true }, { key: 'BalFIAmount', cap: 'Bal FI Amount', kind: 'amt', sum: true }
    ];
    /* FIBalanceDetailByFI.BindGdbreakUp: Id / RefDocumentTypeId / EximInvoiceId / FIId / GDId hidden, UtilizeAmount summed "#,#.###". */
    var DCOLS = [
        { key: 'Id', hidden: true }, { key: 'RefDocumentTypeId', hidden: true }, { key: 'GDId', hidden: true }, { key: 'EximInvoiceId', hidden: true },
        { key: 'InvoiceNo', cap: 'Invoice No' }, { key: 'FIId', hidden: true }, { key: 'FINo', cap: 'FI No' }, { key: 'GdNo', cap: 'Gd No' },
        { key: 'UtilizeAmount', cap: 'Utilize Amount', kind: 'q3', sum: true }
    ];

    function render() { H.drawGrid('grdfrm', ROWS, COLS, { cur: CUR }); H.show('grdfrmEmpty', !ROWS.length); }
    function show(btn) {
        return H.busy(btn, function () {
            return H.postJson(API + '/show', { skipZero: H.checked('ChkSkipZero') }).then(function (rows) { ROWS = rows || []; CUR = -1; render(); });
        });
    }
    /** FIBalanceSummary_Load: focus Skip Zero, btnShow_Click. */
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            ROWS = (d && d.rows) || []; render();
            H.$id('footerInfo').textContent = 'FIBalanceSummary  -  Print 300_01';
            H.focus('ChkSkipZero');
        }).catch(function (e) { H.box(e.message || 'Error occurred during database call.'); });
    }
    /** Reset(): grid cleared, focus Skip Zero. */
    function reset() { ROWS = []; CUR = -1; render(); H.focus('ChkSkipZero'); }
    function print(btn) {
        if (!ROWS.length) { H.box('Not Record Found For Display'); return; }
        return H.print('300_01_FISummaryRegister.rpt', { skipZero: H.checked('ChkSkipZero') ? 1 : undefined }, btn);
    }
    /** grdfrm_LinkClicked "EFormNo": the FI Balance Detail popup for the row's Id. */
    function openDetail(i) {
        var r = ROWS[i]; if (!r) return;
        return H.getJson(API + '/detail?id=' + H.netI(H.col(r, 'Id'))).then(function (rows) {
            DETAIL = rows || [];
            H.drawGrid('grdDetail', DETAIL, DCOLS, {});
            H.show('grdDetailEmpty', !DETAIL.length);
            H.show('fiDetailModal', true);
        }).catch(function (e) { H.box(e.message); });
    }
    function closeDetail() { H.show('fiDetailModal', false); }
    function shortcuts() {
        H.shortcuts([['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on From Balance'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus On Zero Balance'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record']]);
    }
    function toggleHistory() { var b = H.$id('grdfrmBox'); if (b) b.scrollIntoView({ behavior: 'smooth' }); H.focusGrid('grdfrm'); }

    document.addEventListener('DOMContentLoaded', function () {
        H.wireFullscreen();
        H.wireGrid('grdfrm', { select: function (i) { CUR = i; }, link: openDetail });
        H.$id('fiDetailModal').addEventListener('click', function (e) { if (e.target === H.$id('fiDetailModal')) closeDetail(); });
        H.wireKeys({ shortcuts: shortcuts, handle: function (e, k) {
            if (e.key === 'Escape' && !H.$id('fiDetailModal').classList.contains('is-hidden')) { closeDetail(); return; }
            if (!e.ctrlKey) return;
            if (k === 'p') { e.preventDefault(); print(H.$id('btnPrint')); }
            else if (k === 's') { e.preventDefault(); show(H.$id('btnShow')); }
            else if (k === 'n') { e.preventDefault(); reset(); }
            else if (e.key === 'F5' || e.key === 'ArrowUp') { e.preventDefault(); H.focus('ChkSkipZero'); }
            else if (e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grdfrm'); }
            else if (e.key === 'Enter' && CUR >= 0) { e.preventDefault(); openDetail(CUR); }
        } });
        load();
    });

    global.ExportFiBalance = { show: show, reset: reset, print: print, shortcuts: shortcuts, toggleHistory: toggleHistory, closeDetail: closeDetail };
}(window));
