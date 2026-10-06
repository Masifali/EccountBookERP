package com.mst.models.dto;

import java.time.LocalDate;
import lombok.Data;

/** StockEvaluationReportVehicleWise (screen 292) Show filters - ReportsParameters as the desktop fills it. */
@Data
public class InventoryStockEvaluationVehicleWiseRequest {
    private LocalDate fromDate,toDate;
    private Integer classGroupId,itemId,supplierCustomerId,warehouseId,jobLotId,packingTypeId,itemUomId;
    private String cropYear,refDocumentTypeIds,parentCategoryIds,itemCategoryIds,itemTypeIds;
    private Boolean skipZero;
}
