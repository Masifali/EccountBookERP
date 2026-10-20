package com.mst.projections;

/** Row (native) of dbo.CheckRecordReferedInRefRefIdsStockEvalautionDetail / ...InventoryTransactionsPartyProcessing (Sp_InvPurchaseInvoiceDetail_Insert, Sp_InvoicesVouchersandStocksDelete). RefId = 1 means referenced. */
public interface RefRecordCheckRow {
    Integer getRefId();
    Integer getCodeNo();
    String getDocType();
}
