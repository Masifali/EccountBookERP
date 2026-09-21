package com.mst.models.dto;

import java.time.LocalDate;
import lombok.Data;

@Data
public class InventoryItemLedgerRequest {
    private Integer itemId;
    private LocalDate fromDate;
    private LocalDate toDate;
}
