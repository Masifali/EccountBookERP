/* ============================================================================================
 * DocLink - the web side of CommonServices.EditMethodFromLinked
 * (Architecture.WinApp.Common.CommonServices.cs:2533-3922).
 *
 *   EditMethodFromLinked(int DocumentTypeId, int Id, int DocumentTypeIdSrNo = 0,
 *                        int BaseDocumentTypeId = 0, int RefDocumentTypeId = 0)
 *
 * On the desktop every register / history link goes through that one dispatcher. It resolves
 * the document type to a form class (text), a ScreenName tag (text2), a read method (text3) and
 * the id handed to it (num), then - Admin, or CheckScreenRight(text2) - creates the form,
 * Show()s it maximised and invokes text3(num) (after the form's Action field / "initialized"
 * flag, else straight after Show(), i.e. after Load). Accounting vouchers (1-10, 17, 34, 35)
 * are opened in dedicated if-blocks before the switch.
 *
 *   DocLink.open(documentTypeId, id, { srNo, baseDocumentTypeId, refDocumentTypeId, message })
 *
 * resolve() below is that dispatcher, case for case (including its unreachable duplicates and
 * the cases that open nothing). WEB then says where each resolved desktop form lives on the web
 * and how that page is told which record to read. A WEB entry exists ONLY where both the route
 * and the page's own "open by id" were read in the page's source:
 *   query  - the page reads ?id= from location.search itself on load
 *   hook   - the page exposes a function that runs its ReadById (the P280ReadById contract,
 *            or the page's global object); DocLink opens the page, waits until it has loaded
 *            and gone quiet (its Load requests finished), then calls it once.
 * A desktop form that is resolved but has no WEB entry shows the same message the Wages
 * Register showed before: "Document type N (Form) does not have a web page to open yet."
 * A type the dispatcher itself does not handle opens nothing, exactly as on the desktop (its
 * text/text2/text3 stay empty and it returns without a word).
 *
 * Rights: CheckScreenRight(text2) reads clsGlobalVariables.ScreenViewReights (tblUserRights
 * view right per ScreenName). The web grants those view rights as Spring authorities at sign-in
 * (DesktopScreenRightsService.viewableScreens); no endpoint returns them to the browser by
 * ScreenName, so this file does not re-check them. The route itself is what the server gates.
 * ============================================================================================ */
(function (global) {
    'use strict';

    function toI(v) { var n = parseInt(v, 10); return isNaN(n) ? 0 : n; }
    function pick(srNo, id) { return srNo > 0 ? srNo : id; }   /* num = DocumentTypeIdSrNo > 0 ? DocumentTypeIdSrNo : Id */

    /* ------------------------------------------------------------------------------------------
     * EditMethodFromLinked, resolution only. Returns the list of forms the desktop would open
     * (usually one; the Payment voucher block can open two). Each: { cls, tag, method, num }.
     * ------------------------------------------------------------------------------------------ */
    function resolve(t, id, srNo, base, ref) {
        var out = [];
        function f(cls, tag, method, num) { out.push({ cls: cls, tag: tag, method: method, num: num }); }
        var AD = 'Account_Definition.', VT = 'Account_Definition.VouchersWithTax.';

        /* :2543 / :2599 - two independent ifs, then the if/else-if chain from :2655. */
        if ((t === 1 || t === 2) && base === 0) f(AD + 'PaymentVoucherNew', t === 1 ? 'frmCashPaymentVoucher' : 'frmBankPaymentVoucher', 'ReadById', id);
        if ((t === 1 || t === 2) && base === 1) f(VT + 'PaymentVoucherNew', t === 1 ? 'frmCashPaymentVoucherTax' : 'frmBankPaymentVoucherTax', 'ReadById', id);
        if ((t === 1 && srNo === 1) || (t === 2 && srNo === 2)) { f(AD + 'PaymentByInvoiceVoucherNew', 'PaymentByInvoiceVoucherNew', 'HistoryGridFill', id); return out; }
        if ((t === 3 || t === 4) && base === 0) { f(AD + 'ReceiptsVoucherNew', t === 3 ? 'frmCashReceiptVoucher' : 'frmBankReceiptVoucher', 'ReadById', id); return out; }
        if ((t === 3 || t === 4) && base === 2) { f(VT + 'ReceiptsVoucherNew', t === 3 ? 'frmCashReceiptVoucherTax' : 'frmBankReceiptVoucherTax', 'ReadById', id); return out; }
        if (t === 5) {
            if (base === 0) f(AD + 'VoucherEntry', 'VoucherEntry', 'ReadById', id);
            else if (base === 4) f(AD + 'JournalVoucher', 'JournalVoucher', 'ReadById', id);
            return out;
        }
        if (t === 6) { f(AD + 'frmBillsPayables', 'frmBillsPayables', 'ReadById', id); return out; }
        if (t === 7) { f(AD + 'frmBillsReceivables', 'frmBillsReceivables', 'ReadById', id); return out; }
        if (t === 34) { f(AD + 'frmPartyReceiptVoucher', 'frmPartyReceiptVoucher', 'ReadById', id); return out; }
        if (t === 35) { f(AD + 'frmPartyPaymentVoucher', 'frmPartyPaymentVoucher', 'ReadById', id); return out; }
        if (t === 8) { f(AD + 'frmDayBook', 'frmDayBook', 'ReadById', id); return out; }
        if (t === 9) { f(AD + 'DayBook', 'DayBook', 'ReadById', id); return out; }
        if (t === 10 && base === 0) { f(AD + 'ContraVoucher', 'ContraVoucher', 'ReadById', id); return out; }
        if (t === 10 && base === 3) { f(VT + 'ContraVoucher', 'ContraVoucherTax', 'ReadById', id); return out; }
        if (t === 17 && base === 0) { f(AD + 'JournalVoucher_New', 'JournalVoucher_New', 'ReadById', id); return out; }

        /* :3114 - the switch. PRE = RefDocumentTypeId 401 and not packing material (base 2). */
        var P = pick(srNo, id), PRE = ref === 401 && base !== 2, R = 'ReadById';
        switch (t) {
            case 14: f(AD + 'PaymentByInvoiceAccount', 'PaymentByInvoiceAccount', R, P); break;
            case 16: f(AD + 'AcfrmDefPdcManagment', 'AcfrmDefPdcManagment', R, P); break;
            case 24: f(AD + 'PostDatedCheqPaymentVouchers', 'PostDatedCheqPaymentVouchers', R, id); break;
            case 26: if (base === 0) f(AD + 'ExpenseVoucher', 'ExpenseVoucher', R, id); else f(VT + 'ExpenseVoucherNew', 'ExpenseVoucherNew', R, id); break;
            case 27: f(AD + 'frmExportInvoiceVoucher', 'frmExportInvoiceVoucher', R, P); break;
            case 155: f('Service.ExImClearingAgentBillDirect', 'ExImClearingAgentBillDirect', R, P); break;
            case 29: f(AD + 'ReceiptByContract', 'ReceiptByContract', R, P); break;
            case 39: f('Inventory_Definition.frmStoreOpeningStockBalancing', base !== 2 ? 'frmStoreOpeningStockBalancing' : 'frmPackingMaterialOpeningStockBalance', R, P); break;
            case 40: f('Inventory_Definition.frmOpeningStockBlancing', 'frmOpeningStockBlancing', R, P); break;
            case 41: f('Purchase.PurchsaeOrder', 'PurchsaeOrder', R, id); break;
            case 46: f('Purchase.InvFrmGRN', 'InvFrmGRN', R, P); break;
            case 137: f('Purchase.InvFrmGRNDirect', 'InvFrmGRNDirect', R, P); break;
            case 51: f('Purchase.InwardGatePass', 'InwardGatePass', R, id); break;
            case 56: f('Purchase.InvfrmPurchaseInvoice', 'InvfrmPurchaseInvoice', R, P); break;
            case 57: f('Purchase.InvfrmPurchasedirectInvoice', 'InvfrmPurchasedirectInvoice', R, P); break;
            case 58: case 64: f('StoreManagement.PurchaseInvoiceStoreManagement', 'PurchaseInvoiceStoreManagement', R, P); break;
            case 59: f('Purchase.InvfrmInvPurchaseInvoiceReturn', 'InvfrmPurchaseReturn', R, P); break;
            case 61: case 131: f('StoreManagement.frmPurchaseInvoiceDirectStore', 'frmPurchaseInvoiceDirectStore', R, P); break;
            case 70: f('StoreManagement.frmStockAdjustment', 'frmStockAdjustment', R, P); break;
            case 80: if (PRE) f('Production.ProductionAgainstPreCostingJobOrder', 'ProductionAgainstPreCostingJobOrder', 'ReadByIdInput', P);
                     else f('Production.frmProductionInput', 'FoodProductionWithValues', 'ReadByIdInput', P); break;
            case 126: f('StoreManagement.InvfrmInvSaleInvoiceDirectPackingMaterial', base === 1 ? 'frmSaleInvoiceDirectStore' : 'FrmSaleInvoiceDirectPackingMaterial', R, P); break;
            case 81: f('Sale.SaleOrder', 'SaleOrder', R, id); break;
            case 84: f('Sale.DeliveryOrder', 'DeliveryOrder', R, id); break;
            case 85: f('StoreManagement.DeliveryOrderPackingMaterial', 'DeliveryOrderPackingMaterial', R, id); break;
            case 86: f('Sale.InvFrmGDN', 'InvFrmGDN', R, P); break;
            case 91: f('Sale.OutwardGatePass', 'OutwardGatePass', R, id); break;
            case 95: f('Sale.InvfrmSaleInvoice', 'InvfrmSaleInvoice', R, P); break;
            case 98: f('Sale.InvfrmSaleInvoiceReturn', 'InvfrmSaleInvoiceReturn', R, P); break;
            case 99: if (base === 0) f('Sale.InvfrmInvSaleInvoiceDirect', 'InvfrmInvSaleInvoiceDirect', R, P); break;
            case 101: f('Contractor_Wages.frmwagesBillHeader', 'frmwagesBillHeader', R, P); break;
            case 112: if (PRE) f('Production.ProductionAgainstPreCostingJobOrder', 'ProductionAgainstPreCostingJobOrder', 'ReadByIdOutPut', P);
                      else f('Production.frmProductionOutput', 'FoodProductionWithValues', 'ReadByIdOutPut', P); break;
            case 181: if (PRE) f('Production.ProductionAgainstPreCostingJobOrder', 'ProductionAgainstPreCostingJobOrder', 'ReadByIdConsumption', P);
                      else f('Production.FoodProductionWithValues', 'FoodProductionWithValues', 'ReadByIdConsumption', P); break;
            case 111: if (PRE) f('Production.ProductionAgainstPreCostingJobOrder', 'ProductionAgainstPreCostingJobOrder', 'ReadByIdPackingMaterial', P);
                      else f('Production.frmProductionPackingMaterial', 'FoodProductionWithValues', 'ReadByIdPackingMaterial', P); break;
            /* :3382 - num = InvFoodProduction.JobOrderIdByOverHeadId(DocumentTypeIdSrNo): a BLL call. */
            case 110: if (PRE) f('Production.ProductionAgainstPreCostingJobOrder', 'ProductionAgainstPreCostingJobOrder', 'ReadByIdOverHeadJobOrderWise', null);
                      else f('Production.frmProductionOverhead', 'FoodProductionWithValues', 'ReadByIdOverHeadJobOrderWise', null); break;
            case 124: f('Sale.frmGdnDirect', 'frmGdnDirect', R, id); break;
            case 170: f('Sale.frmGdnAgainstSaleOrder', 'frmGdnAgainstSaleOrder', R, P); break;
            case 66: f('Production.invfrmStockConversionProduction', 'invfrmStockConversionProduction', R, P); break;
            case 68: f('StoreManagement.frmStockTransfer', 'frmStockTransfer', R, P); break;
            case 806: f('StoreManagement.frmStockTransferManual', 'frmStockTransferManual', R, P); break;
            case 11: case 12: case 97: break;                                     /* :3780 - nothing */
            default:
                switch (t) {                                                      /* :3429 (126 here is unreachable) */
                    case 145:
                        if (base === 1) f('StoreManagement.PurchaseInvoiceReturn_Store', 'PurchaseInvoiceReturn_Store', R, srNo);
                        else if (base === 2) f('StoreManagement.PurchaseInvoiceReturn_Store', 'PurchaseInvoiceReturnPM', R, srNo);
                        break;
                    case 127: f('Sale.BookingOrder', 'BookingOrder', R, id); break;
                    case 138: f('Purchase.frmPurchaseInvoiceAgaintGrnDirect', 'PurchaseInvoiceAgainstGrnDirect', R, P); break;
                    case 139: f('Sale.frmSaleInvoiceDirect139', 'frmSaleInvoiceDirect139', R, P); break;
                    case 162: f('CommissionAgent.CommissionAgentTradeBill_162', 'CommissionAgentTradeBill_162', R, P); break;
                    case 1056: f('Cmagt.frmCommissionAgentTradeBillAgainstGdn', 'frmCommissionAgentTradeBillAgainstGdn', R, P); break;
                    case 166: f('Paddy_Purchase.frmPurchaseInvoiceForGate', 'frmPurchaseInvoiceForGate', R, P); break;
                    case 171: f('Sale.frmSaleInvoiceAgainstGdnWithoutWb', 'frmSaleInvoiceAgainstGdnWithoutWb', R, P); break;
                    case 172: f('Purchase.PurchaseInvoiceAgainstGrnOrder', 'PurchaseInvoiceAgainstGrnOrder', R, P); break;
                    case 203: f(AD + 'Acfrmfcbankreceipt', 'Acfrmfcbankreceipt', R, P); break;
                    case 204: f('Export.ExImCommercialInvoice', 'EximInvoice', R, P); break;
                    case 205: f('Export.EximForwarding', 'EximForwarding', R, P); break;   /* the frmForwardingNew 205 at :3531 is unreachable */
                    default:
                        switch (t) {                                              /* :3529 */
                            case 452:
                                var tag = base === 0 ? 'StoreIssuanceDirect' : base === 1 ? 'StoreIssuanceDirectStore' : base === 2 ? 'StoreIssuanceDirectPM' : '';
                                if (tag) f('StoreManagement.StoreIssuanceDirect', tag, R, P);   /* empty text2 -> returns */
                                break;
                            case 810: f('Contractor_Wages.frmWagesBillManual', 'frmWagesBillManual', R, P); break;
                            case 702: f('PackingMaterial_Store.PurchaseInvoicePackingMaterial', 'PurchaseInvoicePackingMaterial', R, P); break;
                            case 1304: f('Service.lgstcm.frmLogisticPurchaseServicesBill', 'frmLogisticPurchaseServicesBill', R, P); break;
                            case 1500: f('Steel.Purchase.PurchsaeOrder_St', 'PurchsaeOrder_St', R, P); break;
                            case 1501: f('Steel.Purchase.InvFrmGRN_St', 'InvFrmGRN_St', R, P); break;
                            case 1502: f('Steel.Purchase.InvfrmPurchaseInvoice_St', 'InvfrmPurchaseInvoice_St', R, P); break;
                            case 1503: f('Steel.Purchase.PurchaseInvoiceDirect_St', 'PurchaseInvoiceDirect_St', R, P); break;
                            case 1505: f('Steel.Sale.SaleOrder_St', 'SaleOrder_St', R, P); break;
                            case 1506: f('Steel.Sale.DeliveryOrder_St', 'DeliveryOrder_St', R, P); break;
                            case 1507: f('Steel.Sale.OutwardGatePass_St', 'OutwardGatePass_St', R, P); break;
                            case 1508: f('Steel.Sale.InvFrmGDN_St', 'InvFrmGDN_St', R, P); break;
                            case 1509: f('Steel.Sale.SaleInvoice_St', 'SaleInvoice_St', R, P); break;
                            case 1510: f('Steel.Sale.SaleInvoiceDirect_St', 'SaleInvoiceDirect_St', R, P); break;
                            case 1600: f('PurchaseTrading.PurchsaeOrderWithQty', 'PurchsaeOrderWithQty', R, id); break;
                            case 1602: f('PurchaseTrading.InvFrmGRN_Qty', 'InvFrmGRN_Qty', R, id); break;
                            case 1603: f('PurchaseTrading.PurchaseInvoiceTradingWithTax', 'PurchaseInvoiceTradingWithTax', R, P); break;
                            case 1604: f('PurchaseTrading.PurchaseInvoiceTradePro', 'PurchaseInvoiceTradePro', R, P); break;
                            case 1605: f('SaleTrading.SaleOrderWithQty', 'SaleOrderWithQty', R, id); break;
                            case 1656: f('Mfg.Sale.frmSaleOrderEngr', 'frmSaleOrderEngr', R, id); break;
                            case 1606: f('SaleTrading.DeliveryOrder_Engr', 'DeliveryOrder_Engr', R, id); break;
                            case 1657: f('Mfg.frmDeliveryOrderEngr', 'frmDeliveryOrderEngr', R, id); break;
                            case 1608: f('SaleTrading.SaleInvoiceQtyWithTax', 'SaleInvoiceQtyWithTax', R, P); break;
                            case 1609: f('SaleTrading.SaleInvoiceTrading_Engr', 'SaleInvoiceTrading_Engr', R, P); break;
                            case 1611: f('SaleTrading.frmSaleInvoiceReturn_Engr', 'frmSaleInvoiceReturn_Engr', R, P); break;
                            case 1654: f('Mfg.Purchase.frmPurchaseInvoiceDirectEngr', 'frmPurchaseInvoiceDirectEngr', R, P); break;
                            case 1660: f('Mfg.Sale.frmSaleInvoiceEngr', 'frmSaleInvoiceEngr', R, P); break;
                            case 1661: f('Mfg.Sale.frmSaleInvoiceDirectEngr', 'frmSaleInvoiceDirectEngr', R, P); break;
                            case 1662: f('Mfg.Sale.frmSaleInvoiceReturnEngr', 'frmSaleInvoiceReturnEngr', R, P); break;
                            case 1850: f('pcc.frmConcreteProduction', 'frmConcreteProduction', R, P); break;
                            case 1856: f('pcc.Sale.SaleInvoiceDirectConcrete', 'SaleInvoiceDirectConcrete', R, P); break;
                            case 1859: f('pcc.purchase.PurchaseInvoiceDirectConcrete', 'PurchaseInvoiceDirectConcrete', R, P); break;
                            case 1861: f('pcc.Sale.SaleInvoiceAgainstGDNConcrete', 'SaleInvoiceAgainstGDNConcrete', R, P); break;
                            case 1862: f('pcc.Sale.SaleInvoiceReturnConcrete', 'SaleInvoiceReturnConcrete', R, P); break;
                            case 1865: f('pcc.purchase.PurchaseInvoiceReturn', 'PurchaseInvoiceReturn', R, P); break;
                            case 1000: f('HRM.PayRoll.frmPayrollPosting', 'PayrollPosting', R, P); break;
                            case 1001: f('HRM.LoanManagement.frmEmployeeLoan', 'EmployeeLoan', R, P); break;
                            case 1104: f('FeedMill.Purchase.PurchaseInvoiceDirect', 'PurchaseInvoiceDirect', R, P); break;
                        }
                }
        }
        return out;
    }

    /* ------------------------------------------------------------------------------------------
     * Where the web has the form, keyed "<class after Architecture.WinApp.>|<ScreenName tag>".
     *   q    : route whose page reads ?id= itself        h : route + hook(w, num) on the page
     *   ready: optional extra test (w) before the hook is called
     * Each entry names the file its open-by-id was read in.
     * ------------------------------------------------------------------------------------------ */
    function hookCall(w, fn, args) {
        var r = fn.apply(null, args);
        if (r && typeof r.then === 'function') r.then(null, function (e) { try { w.alert(e && e.message ? e.message : String(e)); } catch (x) { /* closed */ } });
        return r;
    }
    var PAJO = '/production/production-against-job-order';
    var WEB = {
        /* accounts/vouchers/*.html: URLSearchParams id|Id -> load...VoucherForEdit(id). One web page per
           voucher type; its source cites PaymentVoucherNew.cs / ReceiptsVoucherNew.cs / JournalVoucher.cs. */
        'Account_Definition.PaymentVoucherNew|frmCashPaymentVoucher': { q: '/accounts/vouchers/cash-payment' },
        'Account_Definition.PaymentVoucherNew|frmBankPaymentVoucher': { q: '/accounts/vouchers/bank-payment' },
        'Account_Definition.VouchersWithTax.PaymentVoucherNew|frmCashPaymentVoucherTax': { q: '/accounts/vouchers/cash-payment' },
        'Account_Definition.VouchersWithTax.PaymentVoucherNew|frmBankPaymentVoucherTax': { q: '/accounts/vouchers/bank-payment' },
        'Account_Definition.ReceiptsVoucherNew|frmCashReceiptVoucher': { q: '/accounts/vouchers/cash-receipt' },
        'Account_Definition.ReceiptsVoucherNew|frmBankReceiptVoucher': { q: '/accounts/vouchers/bank-receipt' },
        'Account_Definition.VouchersWithTax.ReceiptsVoucherNew|frmCashReceiptVoucherTax': { q: '/accounts/vouchers/cash-receipt' },
        'Account_Definition.VouchersWithTax.ReceiptsVoucherNew|frmBankReceiptVoucherTax': { q: '/accounts/vouchers/bank-receipt' },
        'Account_Definition.JournalVoucher|JournalVoucher': { q: '/accounts/vouchers/journal' },

        /* countx_store_opening_stock_store.js:595 - /[?&]id=(\d+)/ -> open(id) after lookups + history. */
        'Inventory_Definition.frmStoreOpeningStockBalancing|frmStoreOpeningStockBalancing': { q: '/store/opening-stock-store' },
        /* countx_purchase_order_full.js:63 - loadSelectedOrder(id, 'edit'). */
        'Purchase.PurchsaeOrder|PurchsaeOrder': { q: '/purchase/purchase-order' },
        /* countx_market_grn.js:78 - loadRecord(id). */
        'Purchase.InvFrmGRN|InvFrmGRN': { q: '/purchase/goods-receipt-notes' },
        /* countx_inward_gate_pass.js:28 - loadRecordAndEdit(id). */
        'Purchase.InwardGatePass|InwardGatePass': { q: '/purchase/inward-gate-pass' },
        /* countx_purchase_invoice.js:82 - load(id). */
        'Purchase.InvfrmPurchaseInvoice|InvfrmPurchaseInvoice': { q: '/purchase/purchase-invoice' },
        /* countx_direct_invoice.js:61 - load(id). */
        'Purchase.InvfrmPurchasedirectInvoice|InvfrmPurchasedirectInvoice': { q: '/purchase/purchase-invoice-direct' },
        /* sale/sale_invoice.js:1258 - readById(id). */
        'Sale.InvfrmSaleInvoice|InvfrmSaleInvoice': { q: '/sale/sale-invoice' },
        /* sale/sale_invoice_gdn_no_wb.js:936 - readById(id). */
        'Sale.frmSaleInvoiceAgainstGdnWithoutWb|frmSaleInvoiceAgainstGdnWithoutWb': { q: '/sale/sale-invoice-gdn-no-wb' },
        /* countx_wages_bill.js - ?id= -> ReadById(id) after Load (frmwagesBillHeader.ReadById:3054). */
        'Contractor_Wages.frmwagesBillHeader|frmwagesBillHeader': { q: '/production/wages-bill' },

        /* countx_store_purchase_invoice_store_management.js:957 - PiStoreMgmt.open = readById. */
        'StoreManagement.PurchaseInvoiceStoreManagement|PurchaseInvoiceStoreManagement': {
            h: '/store/purchase-invoice-store-management', hook: function (w, n) { return w.PiStoreMgmt && w.PiStoreMgmt.open && function () { return hookCall(w, w.PiStoreMgmt.open, [n]); }; } },
        /* countx_store_purchase_invoice_direct_store.js:681 - PiDirectStore.open = openFromHistory (update-rights check, then readById). */
        'StoreManagement.frmPurchaseInvoiceDirectStore|frmPurchaseInvoiceDirectStore': {
            h: '/store/purchase-invoice-direct-store', hook: function (w, n) { return w.PiDirectStore && w.PiDirectStore.open && function () { return hookCall(w, w.PiDirectStore.open, [n]); }; } },
        /* countx_store_stock_transfer.js:979 - StockTr.readById (ReadById:2186). */
        'StoreManagement.frmStockTransfer|frmStockTransfer': {
            h: '/store/stock-transfer', hook: function (w, n) { return w.StockTr && w.StockTr.readById && function () { return hookCall(w, w.StockTr.readById, [n]); }; } },
        /* countx_store_stock_transfer_manual.js:800 - StockTrM.readById (ReadById:1143, no Reset - the double-click path). */
        'StoreManagement.frmStockTransferManual|frmStockTransferManual': {
            h: '/store/stock-transfer-manual', hook: function (w, n) { return w.StockTrM && w.StockTrM.readById && function () { return hookCall(w, w.StockTrM.readById, [n]); }; } },
        /* countx_store_issuance_direct.js:451 - IssDirect.open = readById (ReadById:1590); the page is ScreenName
           "StoreIssuanceDirect", BaseDocumentTypeId 0 (StoreIssuanceService.Screen.DIRECT). */
        'StoreManagement.StoreIssuanceDirect|StoreIssuanceDirect': {
            h: '/store/store-issuance-direct', hook: function (w, n) { return w.IssDirect && w.IssDirect.open && function () { return hookCall(w, w.IssDirect.open, [n]); }; } },
        /* countx_store_purchase_invoice_return_store.js:1108 - PiReturnStore.open = readById; BaseDocumentTypeId 1 page. */
        'StoreManagement.PurchaseInvoiceReturn_Store|PurchaseInvoiceReturn_Store': {
            h: '/store/purchase-invoice-return-store', hook: function (w, n) { return w.PiReturnStore && w.PiReturnStore.open && function () { return hookCall(w, w.PiReturnStore.open, [n]); }; } },
        /* countx_stock_conversion.js:2463 - StockConversion.load = readById (ReadById:4820; runs its own refreshForm first). */
        'Production.invfrmStockConversionProduction|invfrmStockConversionProduction': {
            h: '/production/stock-conversion', hook: function (w, n) { return w.StockConversion && w.StockConversion.load && function () { return hookCall(w, w.StockConversion.load, [n]); }; } },
        /* countx_p280_input.js:1658 - P280ReadById = showView('form'); CmbJobOrderNo.disabled = true; readByIdInput.
           The dispatcher calls ReadByIdInput alone and never disables CmbJobOrderNo (that is the shell's
           Transaction History Edit, FoodProductionWithValues:1109), so it is re-enabled right after. */
        'Production.frmProductionInput|FoodProductionWithValues': {
            h: PAJO + '/input', hook: function (w, n) {
                return typeof w.P280ReadById === 'function' && function () {
                    var r = hookCall(w, w.P280ReadById, [n]);
                    var c = w.document.getElementById('CmbJobOrderNo'); if (c) c.disabled = false;
                    return r;
                };
            } },
        /* countx_p280_output.js:1006 - P280ReadById = guard(ReadByIdOutPut). */
        'Production.frmProductionOutput|FoodProductionWithValues': {
            h: PAJO + '/output', hook: function (w, n) { return typeof w.P280ReadById === 'function' && function () { return hookCall(w, w.P280ReadById, [n]); }; } },
        /* countx_p280_packing_material.js:1034 - P280ReadById (queued until its Load has finished). */
        'Production.frmProductionPackingMaterial|FoodProductionWithValues': {
            h: PAJO + '/packing-material', hook: function (w, n) { return typeof w.P280ReadById === 'function' && function () { return hookCall(w, w.P280ReadById, [n]); }; } },
        /* countx_p280_consumption.js:1612 - P280Consumption.readById (ReadByIdConsumption:2341; selects tab 2
           itself when the record has rows, as the desktop does inside its row loop). On the shell page. */
        'Production.FoodProductionWithValues|FoodProductionWithValues': {
            h: PAJO, hook: function (w, n) {
                return w.P280 && w.P280.ready && w.P280Consumption && w.P280Consumption.readById && function () {
                    return w.P280.ready.then(function () { return w.P280Consumption.readById(n); });
                };
            } }
    };

    function shortName(cls) { var p = cls.split('.'); return p[p.length - 1]; }

    /** Opens `url` and calls the page's hook once it has loaded and its Load requests have settled. */
    function openWithHook(url, entry, num, say) {
        var w = global.open(url, '_blank');
        if (!w) { say('The browser blocked the new window.'); return; }
        var started = Date.now(), lastCount = -1, quietSince = 0, done = false;
        var t = setInterval(function () {
            if (done) return;
            try {
                if (w.closed) { clearInterval(t); return; }
                if (!w.document || w.document.readyState !== 'complete') return;
                var run = entry.hook(w, num);
                if (!run) {
                    if (Date.now() - started > 30000) { clearInterval(t); say('The page ' + url + ' did not become ready.'); }
                    return;
                }
                /* "Load has finished": no new resource (fetch/XHR) entries for 600 ms, or 15 s passed. */
                var n = w.performance && w.performance.getEntriesByType ? w.performance.getEntriesByType('resource').length : 0;
                if (n !== lastCount) { lastCount = n; quietSince = Date.now(); if (Date.now() - started < 15000) return; }
                if (Date.now() - quietSince < 600 && Date.now() - started < 15000) return;
                done = true; clearInterval(t);
                run();
            } catch (e) { /* the new window is still navigating */ }
        }, 150);
    }

    /**
     * EditMethodFromLinked(DocumentTypeId, Id, DocumentTypeIdSrNo, BaseDocumentTypeId, RefDocumentTypeId).
     * opts: { srNo, baseDocumentTypeId, refDocumentTypeId, message(text) }.
     */
    function open(documentTypeId, id, opts) {
        opts = opts || {};
        var say = typeof opts.message === 'function' ? opts.message : function (m) { global.alert(m); };
        var t = toI(documentTypeId), rid = toI(id);
        var forms = resolve(t, rid, toI(opts.srNo), toI(opts.baseDocumentTypeId), toI(opts.refDocumentTypeId));
        if (!forms.length) return false;                   /* :3786 - nothing resolved: the desktop returns silently */
        forms.forEach(function (fm) {
            var e = WEB[fm.cls + '|' + fm.tag];
            if (!e) {
                say('Document type ' + t + ' (' + shortName(fm.cls) + ') does not have a web page to open yet.');
                return;
            }
            if (e.q) {
                var w = global.open(e.q + '?id=' + encodeURIComponent(fm.num), '_blank');
                if (!w) say('The browser blocked the new window.');
                return;
            }
            openWithHook(e.h, e, fm.num, say);
        });
        return true;
    }

    global.DocLink = {
        open: open,
        /** For inspection: what the desktop dispatcher resolves, and the web target when there is one. */
        resolve: function (documentTypeId, id, opts) {
            opts = opts || {};
            return resolve(toI(documentTypeId), toI(id), toI(opts.srNo), toI(opts.baseDocumentTypeId), toI(opts.refDocumentTypeId))
                .map(function (fm) {
                    var e = WEB[fm.cls + '|' + fm.tag] || null;
                    return { form: fm.cls, screenName: fm.tag, method: fm.method, num: fm.num,
                             route: e ? (e.q || e.h) : null, by: e ? (e.q ? 'query ?id=' : 'page hook') : null };
                });
        }
    };
}(window));
