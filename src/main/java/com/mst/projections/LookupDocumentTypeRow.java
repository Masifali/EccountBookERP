package com.mst.projections;

/** Row of Sp_Item_GetAllMethod @Activity='GetDocumentTypeFromInventoryStocksEvaluations' / 'DocumentTypeGetFromInventoryTransactionsForStore' (JPQL) */
public interface LookupDocumentTypeRow {
    Integer getRefDocumentTypeId();
    String getDocumentTypeDescription();
}
