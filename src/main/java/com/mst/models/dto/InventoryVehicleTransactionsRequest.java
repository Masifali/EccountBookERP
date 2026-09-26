package com.mst.models.dto;

import java.time.LocalDate;
import lombok.Data;

@Data
public class InventoryVehicleTransactionsRequest {
    private LocalDate fromDate,toDate;
    private Integer parentCategoryId,itemId,supplierCustomerId,warehouseId,jobLotId,packingTypeId,itemUomId;
    private String cropYear,documentTypeIds;
}
