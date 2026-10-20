package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_WbTransation_GetAllMethod @Activity='GetWbTransactionByGatePassId' for RefDocumentTypeId 74 (native; TicketNo is selected twice in the proc, WbRemarks replaces OtherRemarks) */
public interface WbTransactionByGatePassStRow {
    Integer getId();
    Integer getTicketNo();
    BigDecimal getFirstWeight();
    BigDecimal getSecondWeight();
    Timestamp getEntryDate();
    String getFirstName();
    String getVehicleNo();
    String getWbRemarks();
    BigDecimal getItemQty();
}
