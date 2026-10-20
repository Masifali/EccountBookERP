package com.mst.projections;

import java.math.BigDecimal;

import org.springframework.beans.factory.annotation.Value;

/**
 * Row of SpAccounts_TradeDebtorsAndCreditors_Report (first result set, native EXEC).
 * Getter = output alias exactly as the procedure emits it. LastBillDate / LastRcvdDate are NVARCHAR(50)
 * columns in the procedure's #Final table (date converted to text); LastBillsAmount / LastRcvdAmount are
 * INT columns there (the UPDATE truncates the decimal amount).
 */
public interface TradeDebtorsCreditorsRow {
    Integer getAccountClassId();
    String getLvl03_Code();
    String getLvl03_Title();
    Integer getParentAccountId();
    Integer getAccountId();
    String getAccountCode();
    String getAccountTitle();
    String getAccountTitleEnglish();
    BigDecimal getOpening();
    BigDecimal getCurrDebit();
    BigDecimal getCurrCredit();
    BigDecimal getClosing();
    /** SQL alias is [Increase/Decrease] (kept verbatim); read through the tuple map. */
    @Value("#{target['Increase/Decrease']}")
    BigDecimal getIncreaseDecrease();
    String getLastBillDate();
    Integer getLastBillsAmount();
    Integer getBillDays();
    String getLastRcvdDate();
    Integer getLastRcvdAmount();
    Integer getRcvdDays();
    Integer getSubsidiaryAccountId();
    String getPartyCode();
    String getSubsidiaryAccountTitle();
    String getCityName();
    String getMobilePersonal();
    String getTitle();
    String getAccountType();
    String getClassName();
    Integer getBranchesId();
    String getBranchName();
    String getReportCriteria();
    BigDecimal getTotalOpening();
    BigDecimal getTotalDebit();
    BigDecimal getTotalCredit();
    BigDecimal getTotalClosing();
    Long getRow_count();
    BigDecimal getParentTotalOpening();
    BigDecimal getParentTotalDebit();
    BigDecimal getParentTotalCredit();
    BigDecimal getParentTotalClosing();
    Long getParentrow_count();
    Integer getDecimalPointsAmount();
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
    String getSupplierBankNames();
}
