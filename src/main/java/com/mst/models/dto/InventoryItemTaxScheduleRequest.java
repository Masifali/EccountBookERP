package com.mst.models.dto;

import java.time.LocalDateTime;

/** Values edited by InvDeffrmItemTaxSchdule; scope and audit come from the session. */
public class InventoryItemTaxScheduleRequest {
    public int id;
    public int sourceId;
    public int itemId;
    public int taxTypeId;
    public LocalDateTime effectedDate;
    public boolean active;
}
