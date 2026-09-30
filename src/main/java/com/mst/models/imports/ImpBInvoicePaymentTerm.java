package com.mst.models.imports;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.Import.invoicePaymentTerm (model 0635) - non-virtual properties bound to
 * ImEx.usp_Set_invoicePaymentTerm. PaymentTerm / FinancialInstrumentNo are virtual and not sent.
 */
public class ImpBInvoicePaymentTerm extends DesktopModel {
    public boolean isApproved;
    public LocalDateTime approvedOn;
    public LocalDateTime createdOn;
    public LocalDateTime lastModifiedOn;
    public double FcyAmount;
    public double pctOfTotal;
    public int appActionId;
    public int approvedUserId;
    public int createdUserId;
    public int DueDays;
    public long invoiceMasterId;
    public long InvoicePaymentTermId;
    public int locationBranchId;
    public int paymentTermId;
    public int DocumentTypeId;
    public int SortNo;
    public int FinancialInstrumentId;
    public long lastModifiedUserId;
    public long RowVersionLong;
    public String Remarks;
}
