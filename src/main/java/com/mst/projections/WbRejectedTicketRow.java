package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_WbTransation_GetAllMethod @Activity='GetWeighBridgeDataForRejectedTicketNos' (native) */
public interface WbRejectedTicketRow {
    Integer getId();
    Timestamp getDocDate();
    Integer getTicketNo();
    String getDocumentTypeDescription();
    BigDecimal getItemQty();
    BigDecimal getFirstWeight();
    BigDecimal getSecondWeight();
    BigDecimal getNetWbWeight();
    String getVehicleNo();
    String getBiltyNo();
    String getItemDescription();
    String getWbRemarks();
    BigDecimal getWbCharges();
}
