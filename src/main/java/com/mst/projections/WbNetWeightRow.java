package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_WbTransation_GetAllMethod @Activity='GetNetWeightFromWbTransactions' (native) */
public interface WbNetWeightRow {
    Integer getId();
    Integer getEntryNo();
    Integer getTicketNo();
    Integer getReferenceDocTypeId();
    Integer getReferenceDocNoId();
    String getDocumentTypeDescription();
    BigDecimal getFirstWeight();
    BigDecimal getSecondWeight();
    Timestamp getDocDate();
    String getWbRemarks();
    String getContainerNo();
    BigDecimal getNetWbWeight();
    BigDecimal getLoadWeight();
    BigDecimal getTearWeight();
    BigDecimal getSupplierWbNetWeight();
}
