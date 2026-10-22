package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='InvPurchaseInvoiceExpense_ReadByPurchaseInvoiceID' */
public interface PurchaseInvoiceExpenseRow {
    Integer getId();
    Integer getGdnId();
    Integer getGdnExpId();
    Integer getGdnNo();
    Integer getPurchaseOrderId();
    Integer getPurchaseOrderSupplierExpId();
    Integer getInvPurchaseInvoiceId();
    Integer getInvRevExpItemId();
    String getOtherItemName();
    BigDecimal getQty();
    BigDecimal getRate();
    BigDecimal getAmount();
    String getRemarks();
    Integer getStockGLAcId();
    String getAccountTitleStock();
    Integer getSaleGLAcId();
    String getAccountTitleSales();
    Integer getCogsGLAcId();
    String getAccountTitleCgs();
    String getCustomRemarks();
    Integer getGrnId();
    Integer getGrnNo();
    Integer getSupplierDispatchId();
    Integer getSupplierDispatchExpenseId();
}
