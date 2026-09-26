package com.mst.models.dto;

/**
 * Screen 341 "Opening Stock Store" ({@code frmStoreOpeningStockBalancing.cs}, DocumentTypeId 39) —
 * what the page posts: the form's own boxes, one field per control.
 *
 * The numeric boxes arrive as the TEXT the operator typed, because the desktop validates them with
 * {@code double.TryParse(textBox.Text)} ("x must be a non-zero number") before it converts them;
 * the server repeats both steps on the text.
 *
 * Doc No is not here: txtDocNo is ReadOnly on the desktop, so the server takes the number from the
 * generator (new) or the stored header (update). Organization, company, branch, financial year,
 * users and entry/approval dates are the signed-in user's.
 */
public class OpeningStockStoreDto {

    public Integer Id = 0;                 // RECID (0 = Save, > 0 = Update)
    public String  DocDate;                // txtDocdate, "yyyy-MM-dd"
    public String  Remarks;                // txtRemarks

    public Integer ItemId = 0;             // cmbItem
    public Integer WarehouseId = 0;        // cmbWarehouse
    public Integer RackId = 0;             // CmbRackName
    public Integer JobLotId = 0;           // cmbJobLot
    public Integer ItemConditionId = 0;    // CmbItemCondition
    public Integer ItemUomSch = 0;         // cmbUOM ("Pack UOM")
    public String  Qty;                    // txtQty ("Item Qty")

    public Integer SecondaryUomId = 0;     // CmbSecondaryUom
    public String  SecondaryUomQty;        // txtSecondaryUomQty ("Secondary Uom Weight")
    public String  SecondaryUomRate;       // txtSecondaryUomRate
    public String  Rate;                   // txtRate ("Item Rate")
    public Integer RateUomSch = 0;         // cmbRateUOM
    public String  Amount;                 // txtAmount ("Total Amount", read-only, computed)
    public Integer StockCrGLAcId = 0;      // cmbStockCreditAcc

    /** btnAttachment (Attachment form, AT.lst): new files to add ... */
    public java.util.List<InventoryPosItemRequest.Upload> files = new java.util.ArrayList<>();
    /** ... and ids of loaded attachments the operator removed (AT.RemovedAttachmentListInUpdateCase). */
    public java.util.List<Integer> removeAttachmentIds = new java.util.ArrayList<>();
}
