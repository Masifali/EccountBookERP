package com.mst.projections;

import java.time.LocalDateTime;

/** Row of InvPurchaseInvoiceRepository.readState / InvoicesVouchersStocksDeleteRepository.readPurchaseInvoiceState (JPQL): approval state read by Sp_InvPurchaseInvoice_Update and Sp_InvoicesVouchersandStocksDelete. */
public interface InvPurchaseInvoiceStateRow {
    Boolean getIsApproved();
    LocalDateTime getPostDate();
    Double getBillAmount();
    Integer getDocNo();
    Integer getFinancialYearId();
}
