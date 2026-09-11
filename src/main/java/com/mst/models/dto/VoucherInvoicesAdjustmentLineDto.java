package com.mst.models.dto;

/**
 * One detail row of a Voucher Invoices Adjustment save/update payload - ditto the fields
 * frmInvoicesAdjustmentVoucher.cs's Insert()/DeleteDetailRow() read off each grid row
 * (r.Cells["RefDocumentTypeId"]/["InvoiceId"]/["InvoiceAmount"]/["AdjustmentAmount"]/["Remarks"]),
 * mapped 1:1 onto USP_VoucherInvoicesAdjustment_InsertUpdateDelete's own real parameter names
 * (confirmed by reading the stored procedure body directly - no schema/param guessing).
 */
public class VoucherInvoicesAdjustmentLineDto {
    private Integer id; // existing VoucherInvoicesAdjustment.Id when editing; 0/null for a new row
    private Integer refDocumentTypeId;
    private Integer refDocNoId; // the outstanding invoice's own Id (InvPurchaseInvoice.Id etc.)
    private Double invoiceAmount;
    private Double adjustmentAmount;
    private String remarks;

    public Integer getId() { return id; }
    public void setId(Integer id) { this.id = id; }

    public Integer getRefDocumentTypeId() { return refDocumentTypeId; }
    public void setRefDocumentTypeId(Integer refDocumentTypeId) { this.refDocumentTypeId = refDocumentTypeId; }

    public Integer getRefDocNoId() { return refDocNoId; }
    public void setRefDocNoId(Integer refDocNoId) { this.refDocNoId = refDocNoId; }

    public Double getInvoiceAmount() { return invoiceAmount; }
    public void setInvoiceAmount(Double invoiceAmount) { this.invoiceAmount = invoiceAmount; }

    public Double getAdjustmentAmount() { return adjustmentAmount; }
    public void setAdjustmentAmount(Double adjustmentAmount) { this.adjustmentAmount = adjustmentAmount; }

    public String getRemarks() { return remarks; }
    public void setRemarks(String remarks) { this.remarks = remarks; }
}
