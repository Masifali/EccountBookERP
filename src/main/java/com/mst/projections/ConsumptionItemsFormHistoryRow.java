package com.mst.projections;

import java.time.LocalDateTime;

/** Row of USP_ConsumptionItems_FormHistory — JPQL projection. */
public interface ConsumptionItemsFormHistoryRow {
    Integer getId();
    Integer getItemId();
    String getItemName();
    Boolean getIsActive();
    String getRemarks();
    LocalDateTime getEntryDate();
    String getUserName();
}
