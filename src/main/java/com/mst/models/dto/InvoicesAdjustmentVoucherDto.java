package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Save / Delete payload of screens 861 "Invoices Adjustment Voucher"
 * (frmInvoicesAdjustmentVoucher, TransactionTypeId 1) and 863 "Receipt Invoices Adjustment
 * Voucher" (frmReceiptInvoicesAdjustmentVoucher, TransactionTypeId 2). The transaction type,
 * screen name, users and entry dates are fixed by the server; the client posts what the form
 * holds: the header combos and EVERY row of the Detail grid (the server applies the desktop's
 * own "Id > 0 or AdjustmentAmount > 0" filter and reports row numbers against the full grid).
 */
public class InvoicesAdjustmentVoucherDto {
    public Integer recId = 0;            // RecId: 0 = Save, the VoucherHeadId being edited = Update
    public Integer voucherHeadId = 0;    // CmbVoucher.Value
    public String voucherCode;           // CmbVoucher.Text
    public String voucherDate;           // txtVoucherDate.Value
    public String voucherAmountText;     // txtVoucherAmountRegular.Text ("#,##.##")
    public Integer paymentTypeId = 0;    // CmbPaymentType.Value
    public Integer supplierCustomerId = 0;   // CmbSupplier row "SupplierCustomerId"
    public Integer partyGlId = 0;            // CmbSupplier row "AccountId"
    public Integer partyValue = 0;           // CmbSupplier.Value (SupplierCustomerId or AccountId, per the feature)
    public List<Line> lines = new ArrayList<>();

    /** One row of grd (dtGrid). */
    public static class Line {
        public Integer id = 0;
        public Integer refDocumentTypeId = 0;
        public Integer invoiceId = 0;
        public Double invoiceAmount = 0d;
        public Double adjustmentAmount = 0d;
        public String remarks;
    }
}
