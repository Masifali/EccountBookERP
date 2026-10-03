/* ============================================================================================
 * countx_export_pre_invoice_register.js - PreInvoiceRegister.cs, screen 271 "Pre Invoice Register".
 * Data: /api/export/pre-invoice-register (USP_GetDataForDropDownFromExportInvoice '209', USP_ExportDetailByContract_Report).
 * Prints: 543 (button hidden on the desktop, still on Alt+1) / 544 through the keys "543-usp-exportdetailbycontract-reportwithcontainers" /
 * "544-usp-exportdetailbycontract-reportwithoutcontainers"; the row button "523-Print" through "exp-523" { id: ContractId, actionId: 1 }.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptA, N = global.ExportRptN, $id = H.$id, box = H.box;
    var API = '/api/export/pre-invoice-register';

    function u3(v) { return N.fmtUpTo(v, 3); }
    function cols() { /* GridBind dtship + GridSettingForShipmentDetail; Print button column inserted at position 0 */
        var c = [{ key: 'Print', caption: '523-Print', width: 70,
            html: function (v, r, i) { return '<button type="button" class="win-btn-cell" data-btn="Print" data-i="' + i + '">523-Print</button>'; } },
            { key: 'CustomerName', caption: 'CustomerName' }, { key: 'ContractId', hidden: true }, { key: 'ContractNo', caption: 'ContractNo' },
            { key: 'ContractDate', caption: 'ContractDate', fmt: 'd' }, { key: 'InvoiceNo', caption: 'InvoiceNo' }, { key: 'InvoiceDate', caption: 'InvoiceDate', fmt: 'd' },
            { key: 'ItemName', caption: 'ItemName' }];
        ['FclQty', 'NoofBags', 'ShipBags', 'BalBags', 'MTon', 'ShipMTon', 'BalMTon', 'InvoiceWeight', 'DispatchWeight', 'BalWeight'].forEach(function (k) {
            c.push({ key: k, caption: k, num: true, sum: true, fmt: u3 });
        });
        return c;
    }
    function shortcuts() {
        H.shortcuts([['Ctrl+N', 'For New'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Alt+1', 'For Print 543'], ['Alt+2', 'For Print 544'],
            ['Ctrl+F5', 'For Focus on Date Type'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+S', 'For Showing Data'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }
    var P = N.contractRegisterPage({ api: API, grid: 'grdShipmentDetail', empty: 'grdShipmentDetailEmpty', cols: cols, shortcuts: shortcuts,
        extra: function () { return { skipZero: H.checked('ChkSkipZero') }; } });

    /** btnRegisterPrint543_Click / 544: "Not Record Found For Display" while dtForShipmentGird is empty; the last GridBind's filters. */
    function printReg(key, btn) {
        var l = P.st.last;
        if (!P.st.rows.length || !l) { box('Not Record Found For Display'); return; }
        return H.print(key, N.args({ fromDate: l.fromDate, toDate: l.toDate, supplierCustomerId: l.supplierCustomerId, exImLcOrderId: l.exImLcOrderId,
            itemTypeId: l.itemTypeId, itemId: l.itemId, zeroBalanceType: l.skipZero ? 1 : 0 }), btn || null);
    }
    function print543(btn) { return printReg('543-usp-exportdetailbycontract-reportwithcontainers', btn); }
    function print544(btn) { return printReg('544-usp-exportdetailbycontract-reportwithoutcontainers', btn || $id('btnRegisterPrint544')); }

    /** grdShipmentDetail_ColumnButtonClick "Print": ExpRptSalesContractExportRegister_523 (ContractId, ActionId 1); no rows -> message. */
    function rowPrint(i, btn) {
        var r = P.st.rows[i]; if (!r) return;
        if (btn && btn.classList.contains('is-busy')) return;
        var id = H.netI(r.ContractId);
        var w = global.CrystalPrint ? global.CrystalPrint.reserve() : null;
        if (btn) { btn.disabled = true; btn.classList.add('is-busy'); }
        var done = function () { if (btn) { btn.disabled = false; btn.classList.remove('is-busy'); } };
        return H.getJson(API + '/contract-check?contractId=' + id).then(function () {
            done();
            return global.CrystalPrint.open('exp-523', { id: id, actionId: 1 }, btn || null, w);
        }).catch(function (e) { done(); if (global.CrystalPrint) global.CrystalPrint.release(w); box(e.message); });
    }
    document.addEventListener('DOMContentLoaded', function () {
        N.embedFooter('pirFooter');
        H.gridEvents('grdShipmentDetail', { button: function (name, i, b) { if (name === 'Print') rowPrint(i, b); }, ctrlSpace: function (i) { rowPrint(i, null); } });
        P.wire(function (e) {
            if (!e.altKey || e.ctrlKey) return;
            if (e.key === '1' || e.code === 'Digit1' || e.code === 'Numpad1') { e.preventDefault(); print543(); }
            else if (e.key === '2' || e.code === 'Digit2' || e.code === 'Numpad2') { e.preventDefault(); print544(); }
        });
        P.load();
    });
    global.ExportPir = { btnNew: P.reset, refresh: function () { /* btnRefresh_Click: empty on the desktop */ }, show: P.show,
        print543: print543, print544: print544, shortcuts: shortcuts };
}(window));
