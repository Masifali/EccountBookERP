package com.mst.projections;

/** Row of InvPurchaseInvoiceDetailRepository.grnWagesTotals (JPQL) / gdnWagesTotals (native): DocNo, SUM(GrossWeight), SUM(ItemQty) for the labour-wages comparison (Sp_InvPurchaseInvoiceDetail_Insert). */
public interface GrnWagesTotalsRow {
    Integer getDocNo();
    Double getGrnWagesWeight();
    Double getGrnQty();
}
