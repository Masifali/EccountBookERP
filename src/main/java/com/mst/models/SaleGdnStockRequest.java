package com.mst.models;

import java.time.LocalDate;
import java.util.List;

/** Read-only stock request for InvFrmGDN.btnGenerateAvailableStock_Click. */
public record SaleGdnStockRequest(LocalDate docDate, List<Line> lines) {
    public record Line(int itemId, int warehouseId, String cropYear, int jobLotId,
                       int packingTypeId, int itemUomId) {}
}
