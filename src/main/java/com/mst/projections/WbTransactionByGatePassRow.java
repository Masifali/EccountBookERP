package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_WbTransation_GetAllMethod @Activity='GetWbTransactionByGatePassId' for RefDocumentTypeId 51 / 91 (native) */
public interface WbTransactionByGatePassRow {
    Integer getId();
    Integer getTicketNo();
    Integer getGpSrNo();
    BigDecimal getFirstWeight();
    BigDecimal getSecondWeight();
    Timestamp getEntryDate();
    String getFirstName();
    String getVehicleNo();
    String getOtherRemarks();
    BigDecimal getItemQty();
}
