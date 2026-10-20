package com.mst.projections;

/** Row of InvPurchaseInvoiceDetailRepository.findUnmatchedDetails (JPQL): #TmpUnmatchedIds of usp_PurchaseInvoiceDetailRemoveByDetailIdsAndValidations. */
public interface UnmatchedDetailRow {
    Integer getDetailId();
    Integer getInvPurchaseInvoiceId();
    Integer getDocumentTypeId();
}
