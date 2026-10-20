package com.mst.projections;

/** Row (native) of dbo.CheckMinAndMaxRateOfItems(...) — InvPurchaseInvoiceDetailRepository.checkMinAndMaxRate (Sp_InvPurchaseInvoiceDetail_Insert). */
public interface MinMaxRateCheckRow {
    Integer getIsError();
    String getErrorMessage();
}
