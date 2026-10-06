package com.mst.models.dto;

import lombok.Data;

/** Desktop frmRptItemList.ShowReport (screen 171 "Item List"): the filter combos and the "Is Active" check box. */
@Data
public class InventoryItemListRequest {
    /** cmbItemCategory / cmbItemTypes / cmbItemClass / cmbPurchaseGLAccount / cmbSaleGLAccount / cmbCGSGLAccount values (0 or null = not chosen). */
    private Integer itemCategoryId, itemTypeId, itemClassId, purchaseGLAC, saleGLAC, cogsGLAC;
    /** Status.Checked -> @ItemStatus = 1 (only passed when checked). */
    private boolean itemStatus;
}
