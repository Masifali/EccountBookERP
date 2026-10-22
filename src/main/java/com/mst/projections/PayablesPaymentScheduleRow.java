package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Date;

import org.springframework.beans.factory.annotation.Value;

/**
 * Row of usp_PayablesAndPaymentSchedule (first result set). In the procedure's #Final table the
 * Last*Date columns are NVARCHAR(50) and LastBillsAmount / LastPaymentAmount / ItemQty / BillWeight /
 * PurchaseAmount / OrderQty / OrderWeight / ReceivedQty / ReceivedWeight are INT (declared as literal 0),
 * so the UPDATEs truncate the summed amounts to integers. DueBalance / NotYetDue are DECIMAL(18,2).
 */
public interface PayablesPaymentScheduleRow {
    Integer getAccountId();
    Integer getAccountTypeId();
    String getAccountType();
    String getClassName();
    String getParentAccount();
    String getAccountCode();
    String getAccountTitle();
    BigDecimal getOpening();
    BigDecimal getCurrDebit();
    BigDecimal getCurrCredit();
    BigDecimal getClosing();
    /** SQL alias [Short/Excess] kept verbatim. */
    @Value("#{target['Short/Excess']}")
    BigDecimal getShortExcess();
    BigDecimal getDueBalance();
    BigDecimal getNotYetDue();
    Double getPayToday();
    String getLastBillDate();
    Integer getLastBillsAmount();
    Integer getBillDays();
    String getLastPaymentDate();
    Integer getLastPaymentAmount();
    Integer getPaymentDays();
    Integer getPurchaseQty();
    Integer getPurchaseWeight();
    Integer getPurchaseAmount();
    Integer getOrderQty();
    Integer getReceivedQty();
    Integer getBalQty();
    Integer getOrderWeight();
    Integer getReceivedWeight();
    Integer getBalWeight();
    Integer getBranchesId();
    String getBranchName();
    Date getFromDate();
    Date getToDate();
    Date getDueDateFrom();
    Date getDueDateTo();
    Date getPurchaseFromDate();
    Date getPurchaseToDate();
}
