package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Date;

import org.springframework.beans.factory.annotation.Value;

/**
 * Row of usp_ReceivablesAndReceiptsSchedule. Closing is CAST to DECIMAL(18,0) by the procedure.
 * In its #Final table the Last*Date columns are NVARCHAR(50) and LastBillsAmount / LastReceiptAmount /
 * ItemQty / BillWeight / SaleAmount / OrderQty / OrderWeight / DispatchQty / DispatchWeight are INT
 * (declared as literal 0), so the UPDATEs truncate the summed amounts. DueBalance / NotYetDue DECIMAL(18,2).
 */
public interface ReceivablesReceiptsScheduleRow {
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
    Double getReceiptsToday();
    String getLastBillDate();
    Integer getLastBillsAmount();
    Integer getBillDays();
    String getLastReceiptDate();
    Integer getLastReceiptAmount();
    Integer getReceiptsDays();
    Integer getSaleQty();
    Integer getSaleWeight();
    Integer getSaleAmount();
    Integer getOrderQty();
    Integer getDispatchQty();
    Integer getBalQty();
    Integer getOrderWeight();
    Integer getDispatchWeight();
    Integer getBalWeight();
    Integer getBranchesId();
    String getBranchName();
    Date getFromDate();
    Date getToDate();
    Date getDueDateFrom();
    Date getDueDateTo();
    Date getSaleFromDate();
    Date getSaleToDate();
}
