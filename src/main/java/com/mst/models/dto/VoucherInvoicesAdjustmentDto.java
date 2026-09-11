package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Save/Update/Delete payload for the Voucher Invoices Adjustment screen - ditto
 * frmInvoicesAdjustmentVoucher.cs's Insert()/btnDelete_Click(). TransactionTypeId is NOT
 * carried here: the real desktop constructor hardcodes it to 1 for this specific screen
 * (the Payment-side outstanding-vs-Purchase-Invoice adjustment form), and the Java service
 * does the same rather than accept a value this UI never actually varies.
 */
public class VoucherInvoicesAdjustmentDto {
    private Integer recId;         // 0/null = new voucher; >0 = editing (equals voucherHeadId of the loaded record)
    private Integer voucherHeadId; // CmbVoucher.Value - the outstanding VoucherHead being adjusted
    private Integer voucherCode;
    private String voucherDate;
    private Double voucherAmount;
    private Integer paymentTypeId;
    private Integer supplierCustomerId;
    private Integer partyGlId;
    private List<VoucherInvoicesAdjustmentLineDto> lines = new ArrayList<>();
    private List<Integer> removedLineIds = new ArrayList<>();

    public Integer getRecId() { return recId; }
    public void setRecId(Integer recId) { this.recId = recId; }

    public Integer getVoucherHeadId() { return voucherHeadId; }
    public void setVoucherHeadId(Integer voucherHeadId) { this.voucherHeadId = voucherHeadId; }

    public Integer getVoucherCode() { return voucherCode; }
    public void setVoucherCode(Integer voucherCode) { this.voucherCode = voucherCode; }

    public String getVoucherDate() { return voucherDate; }
    public void setVoucherDate(String voucherDate) { this.voucherDate = voucherDate; }

    public Double getVoucherAmount() { return voucherAmount; }
    public void setVoucherAmount(Double voucherAmount) { this.voucherAmount = voucherAmount; }

    public Integer getPaymentTypeId() { return paymentTypeId; }
    public void setPaymentTypeId(Integer paymentTypeId) { this.paymentTypeId = paymentTypeId; }

    public Integer getSupplierCustomerId() { return supplierCustomerId; }
    public void setSupplierCustomerId(Integer supplierCustomerId) { this.supplierCustomerId = supplierCustomerId; }

    public Integer getPartyGlId() { return partyGlId; }
    public void setPartyGlId(Integer partyGlId) { this.partyGlId = partyGlId; }

    public List<VoucherInvoicesAdjustmentLineDto> getLines() { return lines; }
    public void setLines(List<VoucherInvoicesAdjustmentLineDto> lines) { this.lines = lines; }

    public List<Integer> getRemovedLineIds() { return removedLineIds; }
    public void setRemovedLineIds(List<Integer> removedLineIds) { this.removedLineIds = removedLineIds; }
}
