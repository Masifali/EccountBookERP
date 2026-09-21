package com.mst.models.dto;

import java.time.LocalDate;
import lombok.Data;

@Data
public class InventoryTransactionsRequest {
    private LocalDate fromDate, toDate;
    private Integer supplierCustomerId, documentTypeId, itemId, warehouseId, jobLotId,
            itemClassGroupId, parentCategoryId, itemCategoryId, itemTypeId;
    private String itemTypeIds, warehouseIds;
}
