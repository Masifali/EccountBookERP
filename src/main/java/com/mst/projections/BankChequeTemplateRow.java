package com.mst.projections;

/** Row of "Select Id, ChequeTemplete from Bank Where ChartOfAccountId = @AgainstAccountId" in Sp_VoucherDetail_Insert. Native projection. */
public interface BankChequeTemplateRow {
    Integer getId();
    String getChequeTemplete();
}
