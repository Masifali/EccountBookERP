package com.mst.projections;

import java.math.BigDecimal;

/** Row (native) of InvContractorWagesBillHeader lookups: Sp_InvPurchaseInvoiceDetail_Insert (wagesBillHeaderByRefDoc) and Sp_InvoicesVouchersandStocksDelete #TmpWagesData (findWagesBillsOfDocument). */
public interface ContractorWagesBillHeaderRow {
    Integer getId();
    Integer getDocumentTypeId();
    Integer getDocNo();
    BigDecimal getQtyTotal();
    BigDecimal getWeightTotal();
}
