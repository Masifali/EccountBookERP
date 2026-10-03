/* countx_export_commission_agent_fcy_ledger.js - CommissionAgentFcyLedger.cs, screen 256 "Comm Agent Fcy Ledger".
 * Data: /api/export/commission-agent-fcy-ledger. */
(function (global) {
    'use strict';
    var H = global.ExRptC, API = '/api/export/commission-agent-fcy-ledger';
    var YEAR_START = '', ROWS = [], CUR = -1;
    var COLS = [
        { key: 'Id', hidden: true }, { key: 'DocDate', cap: 'Doc Date', kind: 'short' }, { key: 'V.Type', cap: 'V.Type' }, { key: 'V.Code', cap: 'V.Code' }, { key: 'InvoiceNo', cap: 'Invoice No' },
        { key: 'InvoiceAmount', cap: 'Invoice Amount', kind: 'q3' }, { key: 'Party', cap: 'Party' }, { key: 'Comm%', cap: 'Comm%', kind: 'q3' }, { key: 'FcyCode', cap: 'Fcy Code' },
        { key: 'FcyDebit', cap: 'Fcy Debit', kind: 'amt', sum: true }, { key: 'FcyCredit', cap: 'Fcy Credit', kind: 'amt', sum: true }, { key: 'FcyBalance', cap: 'Fcy Balance', kind: 'both' },
        { key: 'ExchangeRate', cap: 'Exchange Rate', kind: 'rate' }, { key: 'Debit', cap: 'Debit', kind: 'amt', sum: true }, { key: 'Credit', cap: 'Credit', kind: 'amt', sum: true },
        { key: 'Balance', cap: 'Balance', kind: 'both' }
    ];

    function render() { H.drawGrid('grdShipment', ROWS, COLS, { cur: CUR }); H.show('grdShipmentEmpty', !ROWS.length); }
    function dateType() { H.applyDateType(H.val('CmbDateType'), 'txtdatefrom', 'txtdateto', YEAR_START); }
    function bindAgents(rows) { H.bind('cmbSupplierName', rows || [], 'Id', 'Name'); }
    /** GridBind: "Commission Agent Required!" first (Warning!), then the ledger. */
    function show(btn) {
        if (H.netI(H.val('cmbSupplierName')) <= 0) { H.box('Commission Agent Required!'); return Promise.resolve(); }
        return H.busy(btn, function () {
            return H.postJson(API + '/show', { fromDate: H.val('txtdatefrom'), toDate: H.val('txtdateto'), commissionAgentId: H.netI(H.val('cmbSupplierName')) })
                .then(function (rows) { ROWS = rows || []; CUR = -1; render(); });
        });
    }
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            YEAR_START = d.yearStart || '';
            H.bindDateTypes('CmbDateType', d.dateTypes);
            H.setText('txtdatefrom', H.today()); H.setText('txtdateto', H.today());
            H.setVal('CmbDateType', 3); dateType();
            bindAgents(d.agents);
            H.$id('footerInfo').textContent = 'CommissionAgentFcyLedger  -  Print 568';
            H.focus('CmbDateType');
        }).catch(function (e) { H.box(e.message || 'Error occurred during database call.'); });
    }
    /** btnnew_Click: date type 3, To = now, agent cleared, grid cleared. */
    function reset() {
        H.setVal('CmbDateType', 3); dateType();
        H.setText('txtdateto', H.today());
        H.setVal('cmbSupplierName', 0);
        ROWS = []; CUR = -1; render();
    }
    function refresh(btn) { return H.busy(btn, function () { return H.getJson(API + '/combos').then(bindAgents); }); }
    function print(btn) {
        if (!ROWS.length) { H.box('No Record Found For Display'); return; }
        return H.print('568-CommissionAgentFcyLedger.rpt', { fromDate: H.val('txtdatefrom'), toDate: H.val('txtdateto'), commissionAgentId: H.netI(H.val('cmbSupplierName')) || undefined }, btn);
    }
    /* MakeShortCutKeys: the desktop's table is empty on this form. */
    function shortcuts() { H.shortcuts([]); }
    function toggleHistory() { var b = H.$id('grdShipmentBox'); if (b) b.scrollIntoView({ behavior: 'smooth' }); H.focusGrid('grdShipment'); }

    document.addEventListener('DOMContentLoaded', function () {
        H.wireFullscreen();
        H.$id('CmbDateType').addEventListener('change', dateType);
        H.wireGrid('grdShipment', { select: function (i) { CUR = i; } });
        H.wireKeys({ shortcuts: shortcuts, handle: function (e, k) {
            if (!e.ctrlKey) return;
            if (k === 'n') { e.preventDefault(); reset(); }
            else if (k === 'r') { e.preventDefault(); refresh(H.$id('btnRefresh')); }
            else if (k === 's') { e.preventDefault(); show(H.$id('btnShow')); }
            else if (k === 'p') { e.preventDefault(); print(H.$id('btnRegisterReport')); }
            else if (e.key === 'F5') { e.preventDefault(); H.focus('CmbDateType'); }
            else if (e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grdShipment'); }
        } });
        load();
    });

    global.ExportCommAgentLedger = { show: show, reset: reset, refresh: refresh, print: print, shortcuts: shortcuts, toggleHistory: toggleHistory };
}(window));
