package com.mst.projections;

/**
 * Row of Usp_ReceivablesByDueDates. Amount columns are FLOAT in the procedure's #TmpResult table.
 */
public interface ReceivablesByDueDatesRow {
    Integer getAccountId();
    String getAccountCode();
    String getAccountTitle();
    String getParentAccount();
    String getAccountType();
    Double getOpening();
    Double getDebit();
    Double getCredit();
    Double getClosing();
    Double getNotYetDue();
    Double getOverDueReceivables();
    Double getNetReceivablesTotal();
    Double getNotYetDueReceivablesTotal();
    Double getOverDueReceivablesTotal();
    String getCompCountry();
    String getCompContactPerson();
    String getCompMobileA();
    String getCompMobileB();
    String getCompMobileC();
    String getCompEmailA();
    String getCompEmailB();
    byte[] getCompLogoImage();
    String getCompanyWebsite();
    String getCompanyFaxNo();
    String getOrgReportingRemarks();
}
