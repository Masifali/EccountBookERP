package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/**
 * Row of Sp_Accounts_Receivables_Rpt. LastBillDate / LastRcvdDate come straight from the
 * V_AcReceiablesMaxDateAndTotal* views (VoucherHead.VoucherDate, datetime); FollowupDate / PromiseDate
 * from AccountsPayRecFollowUpHistory.
 */
public interface ReceivablesRptRow {
    Integer getAccountId();
    Integer getId();
    String getAccountCode();
    String getAccountTitle();
    String getClassName();
    String getAccountType();
    String getParentAccount();
    BigDecimal getOpening();
    BigDecimal getObDebit();
    BigDecimal getObCredit();
    BigDecimal getCurrDebit();
    BigDecimal getCurrCredit();
    BigDecimal getClDebit();
    Timestamp getLastBillDate();
    BigDecimal getLastBillsAmount();
    Integer getBillDays();
    Timestamp getLastRcvdDate();
    BigDecimal getLastRcvdAmount();
    Integer getRcvdDays();
    String getCityName();
    String getMobilePersonal();
    String getTitle();
    Timestamp getFollowupDate();
    Timestamp getPromiseDate();
    byte[] getCompLogoImage();
    Integer getDecimalPointsAmount();
    Integer getBranchesId();
    String getBranchName();
}
