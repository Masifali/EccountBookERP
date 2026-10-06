package com.mst.models.dto;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;
import lombok.Data;

/** Desktop InventoryEvaluationItemLedger.btnshow_Click (screen 293 "Transaction Report (With Value)"). */
@Data
public class InventoryTransactionsWithValueRequest {
    private LocalDate fromDate, toDate;
    /** cmbBranchName checked items (BranchId) and cmbparentCategory checked items (Id). */
    private List<Integer> branchIds = new ArrayList<>();
    private List<Integer> parentIds = new ArrayList<>();
    private Integer itemId, itemClassGroupId, warehouseId, jobLotId, itemStockAc,
            itemCategoryId, itemTypeId, documentTypeId;
    /** CmbCropYear.Text - the desktop passes the crop year text, not its id. */
    private String cropYear;
    /** RdSaleAmount.Checked -> @SaleValue = 1. */
    private boolean saleValue;
}
