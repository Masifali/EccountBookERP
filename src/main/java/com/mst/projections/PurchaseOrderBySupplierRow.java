package com.mst.projections;

/** Row of Sp_PurchaseOrder_GetAllMethod @Activity='GetOrdersBySupplierId' (JPQL). docDate is java.util.Date (existing entity mapping). */
public interface PurchaseOrderBySupplierRow {
    Integer getId();
    Integer getDocNo();
    java.util.Date getDocDate();
    Integer getOrderSupCustId();
}
