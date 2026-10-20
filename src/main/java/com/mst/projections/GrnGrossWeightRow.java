package com.mst.projections;

/** Row of InvPurchaseInvoiceDetailRepository.grnGrossWeightByGrn (JPQL): SUM(InvGrnDetail.GrossWeight) + InvGrn.DocNo (Sp_InvPurchaseInvoiceDetail_Insert). */
public interface GrnGrossWeightRow {
    Double getGrnGrossWeight();
    Integer getGrnNo();
}
