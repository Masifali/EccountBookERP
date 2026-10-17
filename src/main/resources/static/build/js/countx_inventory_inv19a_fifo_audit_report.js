/* frmFiFoAuditReport (Inventory_Reports/frmFiFoAuditReport.cs) - ScreenDefinition 290 "FIFO Audit Report".
   Line refs ":NNN" are that file. API: /inventory/reports/fifo-audit-report/api. */
(function () {
    'use strict';
    var SC = window.StoreCommon, X = window.Inv19a, $id = SC.$id, num = SC.num, API = '/inventory/reports/fifo-audit-report/api';
    var F = { dec: 0, paren: true, negDec: 2 };    // "#,#;(#,#.##);0"

    var st = { dt: [] };

    /* gridSetting :131 - RefDocumentTypeId/RefDocIdNo/RefDocSubIdNo hidden, six numerics summed, extra "TranDetail" button column (width 80). */
    var grid = new X.Grid('grdfrm', [
        { key: 'RefDocumentTypeId', hidden: true }, { key: 'RefDocIdNo', hidden: true }, { key: 'RefDocSubIdNo', hidden: true },
        { key: 'DocumentTypeCode', caption: 'Document Type Code', width: 110 },
        { key: 'DocCodeNo', caption: 'Doc Code No', width: 90 },
        { key: 'ReferedDocNos', caption: 'Refered Doc Nos', width: 260 },
        { key: 'InQty', caption: 'In Qty', width: 80, num: F, sum: true },
        { key: 'OutQty', caption: 'Out Qty', width: 80, num: F, sum: true },
        { key: 'BalQty', caption: 'Bal Qty', width: 80, num: F, sum: true },
        { key: 'InWeight', caption: 'In Weight', width: 90, num: F, sum: true },
        { key: 'OutWeight', caption: 'Out Weight', width: 90, num: F, sum: true },
        { key: 'BalWeight', caption: 'Bal Weight', width: 90, num: F, sum: true },
        { key: 'TranDetail', caption: 'TranDetail', width: 80, button: 'TranDetail' }
    ], { navigator: '#lblNavigator', onButton: grdfrm_ColumnButtonClick });

    /* GridFill :85 - dtcol = the twelve columns of dt :99-114; empty -> grdfrm.ClearStructure() */
    function GridFill() {
        return SC.getJson(API + '/show').then(function (rows) {
            st.dt = rows || [];
            grid.setRows(st.dt.map(function (r) {
                var g = function (k) { return SC.ci(r, k); };
                return { RefDocumentTypeId: num(g('RefDocumentTypeId')), RefDocIdNo: num(g('RefDocIdNo')), RefDocSubIdNo: num(g('RefDocSubIdNo')),
                    DocumentTypeCode: g('DocumentTypeCode'), DocCodeNo: g('DocCodeNo'), ReferedDocNos: g('ReferedDocNos'),
                    InQty: num(g('InQty')), OutQty: num(g('OutQty')), BalQty: num(g('BalQty')), InWeight: num(g('InWeight')), OutWeight: num(g('OutWeight')), BalWeight: num(g('BalWeight')) };
            }));
        }).catch(function (e) { alert(e.message); });
    }

    /* grdfrm_ColumnButtonClick :273 - StockTransactionDetail(RefDocumentTypeId, RefDocId = RefDocIdNo, RefDocDetailId = RefDocSubIdNo).Show() */
    function grdfrm_ColumnButtonClick(key, r) {
        if (key !== 'TranDetail' || !r) return;
        SC.getJson(API + '/transaction-detail' + SC.qs({ refDocumentTypeId: r.RefDocumentTypeId, refDocIdNo: r.RefDocIdNo, refDocSubIdNo: r.RefDocSubIdNo }))
            .then(function (rows) { X.modal('Stock Transaction Detail - ' + r.DocumentTypeCode + '-' + r.DocCodeNo, X.rowsTable(rows)); })
            .catch(function (e) { alert(e.message); });
    }

    /* BtnPrintRegister_Click :207 - dt -> 390_GetFIFODataForAudit.rpt */
    function BtnPrintRegister_Click() { X.printRows('390_GetFIFODataForAudit.rpt', st.dt, 'Record Not Found For Display'); }

    document.addEventListener('DOMContentLoaded', function () {
        $id('btnRefresh').addEventListener('click', function () { SC.withBusy($id('btnRefresh'), GridFill); });   // btnNew_Click :177
        $id('BtnPrintRegister').addEventListener('click', BtnPrintRegister_Click);
        document.addEventListener('keydown', function (e) {   // IssuanceHistory_KeyDown :230
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'p') { e.preventDefault(); BtnPrintRegister_Click(); }
            else if ((e.ctrlKey && k === 'e') || k === 'escape') { e.preventDefault(); X.closeForm(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); GridFill(); }
        });
        GridFill();   // frmGatePassReport_Load :58
    });
})();
