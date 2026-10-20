package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Date;
import java.sql.Timestamp;

/**
 * Row of Sp_Accounts_GeneralLedger_Rpt (single result set, native EXEC).
 * Column types follow the procedure's #GeneralLedger temp table: the opening row UNIONs literal '' / 0 with
 * the voucher columns, so CheqDate and DueDate are datetime (the '' literal becomes 1900-01-01) and
 * IsApproved is int (1 UNION bit). CompLogoImage is the literal '' (string) in this report.
 */
public interface GeneralLedgerRptRow {
    String getReportCriteria();
    String getPrintedBy();
    Date getFromDate();
    Date getToDate();
    Integer getAccountId();
    Integer getVoucherHeadId();
    Integer getDocumentTypeId();
    Integer getBaseDocumentTypeId();
    Integer getDetailId();
    Integer getDocumentTypeSrNo();
    Timestamp getVoucherDate();
    String getDocTypeCode();
    Integer getVoucherCode();
    String getParentAccountTitle();
    String getAccountCode();
    String getAccountTitle();
    String getOffsetAccountTitle();
    String getSubsidiaryAccount();
    String getMannualNo();
    String getRefInvoiceNo();
    String getCheqNoDetail();
    String getComments();
    String getCheqNo();
    Timestamp getCheqDate();
    String getPayeeTitle();
    BigDecimal getDebitAmount();
    BigDecimal getCreditAmount();
    BigDecimal getQtyIn();
    BigDecimal getQtyOut();
    BigDecimal getItemRate();
    Integer getBookmarkStatus();
    String getBookmarkRemarks();
    BigDecimal getBalance();
    Integer getPostDatedNoOfCheqs();
    BigDecimal getPostDatedCheqAmount();
    Integer getNoOfAttachments();
    Integer getAttachmentsCount();
    Integer getDecimalPointOfAmount();
    Integer getDecimalPointOfRate();
    String getMobilePersonal();
    String getAddress();
    String getCityName();
    String getPersonName();
    String getEmail();
    Integer getIsApproved();
    String getJobLot();
    BigDecimal getBillAmount();
    Timestamp getDueDate();
    Integer getDueDays();
    String getVehcileNo();
    String getRefParty();
    Integer getBranchesId();
    String getBranchName();
    String getStockReservedStatus();
    BigDecimal getOpeningBalance();
    BigDecimal getTotalDebit();
    BigDecimal getTotalCredit();
    BigDecimal getClosingBalance();
    Long getRow_count();
    String getCurrencyName();
    String getAccountClass();
    String getAccountType();
    String getCompCode();
    String getCompName();
    String getCompAddress();
    String getCompCountry();
    String getCompContactPerson();
    String getCompMobileA();
    String getCompMobileB();
    String getCompMobileC();
    String getCompEmailA();
    String getCompEmailB();
    String getCompLogoImage();
    String getCompanyWebsite();
    String getCompanyFaxNo();
    String getOrgReportingRemarks();
    String getPhoneNo();
}
