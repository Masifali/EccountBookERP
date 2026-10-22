package com.mst.projections;

import java.time.LocalDateTime;

/** Row of InvPurchaseInvoiceDetailRepository.readGrnInfo (JPQL): InvGrn DocDate / GrnTypeId / DocumentTypeId / AddWages / DocNo (Sp_InvPurchaseInvoiceDetail_Insert). */
public interface InvGrnInfoRow {
    LocalDateTime getDocDate();
    Integer getGrnTypeId();
    Integer getDocumentTypeId();
    Integer getAddWages();
    Integer getDocNo();
}
