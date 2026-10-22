package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Date;

/**
 * Row of usp_SupplierAgingReport_DocumentWise (single branch; native CTE query and the KEEP-EXEC fallback share it).
 * Same as CustomerAgingDocumentWiseRow plus ManualNo and PartyBillDate.
 * Native projection: SQL Server DATE columns arrive as java.sql.Date, DECIMAL as BigDecimal.
 */
public interface SupplierAgingDocumentWiseRow {
    Integer getId();
    Integer getDocumentTypeId();
    String getDocumentType();
    String getInvoiceNo();
    String getManualNo();
    Date getInvoiceDate();
    Date getInvoiceDueDate();
    Date getPartyBillDate();
    Integer getAccountId();
    String getParentAccountCode();
    String getParentAccount();
    String getClassName();
    String getAccountCode();
    /** CONCAT(C.AccountTitle,'  ',R.CurrentGlBalance) */
    String getPartyName();
    BigDecimal getDebitAmount();
    BigDecimal getCreditAmount();
    String getRemarks();
    /** SUM(DebitAmount - CreditAmount) OVER(PARTITION BY AccountId) */
    BigDecimal getPartyBalance();
    BigDecimal getDueAmount();
    BigDecimal getCurrentGlBalance();
    /** alias [1stInterval] */
    BigDecimal get1stInterval();
    /** alias [2ndInterval] */
    BigDecimal get2ndInterval();
    /** alias [3rdInterval] */
    BigDecimal get3rdInterval();
    /** alias [Above] */
    BigDecimal getAbove();
    String getFirstIntervalCaption();
    String getSecondIntervalCaption();
    String getThirdIntervalCaption();
    String getAboveIntervalCaption();
    Date getAsOnDate();
}
