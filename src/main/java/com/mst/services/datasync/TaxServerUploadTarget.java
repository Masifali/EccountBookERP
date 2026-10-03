package com.mst.services.datasync;

import org.springframework.stereotype.Component;

/**
 * Where the desktop's DataSyncing forms write an "uploaded" document: the TAX SERVER database.
 *
 * All five Architecture.WinApp.DataSyncing forms (frmPendingPurchaseInvoiceForUpload, frmPendingSaleInvoiceForUpload,
 * frmPendingStoreIssuanceForUpload, frmPendingExportVoucherForUpload, frmPendingVouchersForUpload) share one pattern
 * (they share no base class): list the local documents a USP_*PendingForUpload returns, map each checked one with
 * Architecture.WinApp.DataSyncing.Mapping.*, save the mapped object through Architecture.BLL.TaxProject.* and, only
 * when that save succeeded, write the local upload status (USP_*_UpdateUploadStatus).
 *
 * Every Architecture.DAL.TaxProject.* class (e.g. DAL 0239 InvPurchaseInvoice.SetData :29, DAL 0243 CommonServices
 * via GenericProvider.GetDataTableProcForServer) opens ConnectionObject.SQLForServer() - the connection string
 * "TPSSDb_Server" of the desktop's App.config, stored encrypted and read by Services.GetTaxServerConnectionString()
 * (architecture.common 0007 :29; prefixed with "Data Source=" + AppSettings["DbServerIP"] when that is set). Neither
 * the App.config nor its value is in the desktop sources, and the web application has no such connection: the server,
 * database and credentials cannot be known from the source, so none is invented here.
 *
 * The write chain behind that connection (BLL.TaxProject.Inventory.InvPurchaseInvoice.Save -> PurchaseInvoiceFinancial.
 * MakeVoucherForPurchaseInvoice -> DAL.TaxProject SetData: Sp_InvPurchaseInvoice_Insert, Sp_InvPurchaseInvoiceDetail_Insert,
 * ... Sp_InvPurchaseInvoiceEmptyBags_Insert, voucher and stock-evaluation procedures, all on the tax server) therefore
 * cannot run from the web. This class is the single point where that write would happen; it refuses, so the caller
 * stops before the local status write-back exactly as the desktop does when Save throws.
 */
@Component
public class TaxServerUploadTarget {

    /** The refusal text shown to the operator (and in the migration report). */
    public static String notConfigured(String what) {
        return what + " was not uploaded. The desktop saves it to the tax server database (App.config connection string "
                + "\"TPSSDb_Server\", ConnectionObject.SQLForServer). That connection is not in the desktop sources and is not "
                + "configured in the web application, so the upload stops here and the upload status was not changed.";
    }

    /** BLL.TaxProject.Inventory.InvPurchaseInvoice.Save(obj2) (frmPendingPurchaseInvoiceForUpload.cs:579). */
    public int savePurchaseInvoice(PurchaseInvoiceTaxMapping.TaxPurchaseInvoice invoice, int localDocNo) {
        throw new IllegalArgumentException(notConfigured("Selected Invoice : " + localDocNo));
    }
}
