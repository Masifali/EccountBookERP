package com.mst.projections;

/** Row of InvPurchaseInvoiceDetailRepository.findStockEvaluationReferences (JPQL): DocType / CodeNo of the stock-evaluation record blocking the delete (usp_PurchaseInvoiceDetailRemoveByDetailIdsAndValidations). */
public interface StockEvaluationReferenceRow {
    String getDocType();
    Integer getCodeNo();
}
