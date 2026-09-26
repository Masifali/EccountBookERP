package com.mst.models.dto;

import java.math.BigDecimal;
import java.time.LocalDate;

/** Editable controls in frmOpeningStockBlancing; tenant and audit values are server-owned. */
public class InventoryOpeningRequest {
    public int id, projectsId, warehouseId, itemId, itemUomSch, jobLotId, packingTypeId, stockCrGLAcId, rateUomSch;
    public BigDecimal qty, weightKgs, itemRate, itemAmount;
    public String cropYear, remarks, transactionType;
    public java.util.List<InventoryPosItemRequest.Upload> files=new java.util.ArrayList<>();
    public java.util.List<Integer> removeAttachmentIds=new java.util.ArrayList<>();
    public static class History {
        public LocalDate fromDate, toDate;
        public Integer fromDocNo, toDocNo, accountId, itemId, itemStockAccountId;
    }
}
