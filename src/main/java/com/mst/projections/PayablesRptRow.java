package com.mst.projections;

import java.math.BigDecimal;

import org.springframework.beans.factory.annotation.Value;

/**
 * Row of Sp_Accounts_Payables_Rpt. LastBillDate / LastPaidDate are NVARCHAR(50) in the procedure's
 * #TmpResult (date converted to text), LastBillsAmount / LastPidAmount are INT there (truncated amounts).
 * The procedure emits CityName twice; the projection exposes it once (same value).
 */
public interface PayablesRptRow {
    Integer getAccountId();
    String getAccountCode();
    String getAccountTitle();
    Integer getAccountClass();
    Integer getAccountTypeId();
    String getAccountGroup();
    BigDecimal getOpening();
    BigDecimal getObDebit();
    BigDecimal getObCredit();
    BigDecimal getCurrDebit();
    BigDecimal getCurrCredit();
    BigDecimal getClDebit();
    BigDecimal getClCredit();
    BigDecimal getClosing();
    /** SQL alias [Increase/Decrease_Debit] kept verbatim. */
    @Value("#{target['Increase/Decrease_Debit']}")
    BigDecimal getIncreaseDecreaseDebit();
    /** SQL alias [Increase/Decrease_Credit] kept verbatim. */
    @Value("#{target['Increase/Decrease_Credit']}")
    BigDecimal getIncreaseDecreaseCredit();
    /** SQL alias [Increase/Decrease] kept verbatim. */
    @Value("#{target['Increase/Decrease']}")
    BigDecimal getIncreaseDecrease();
    Integer getOrganizationId();
    Integer getCompanyId();
    Integer getFinancialYearId();
    String getCityName();
    Integer getDecimalPointsAmount();
    byte[] getCompLogoImage();
    Integer getParentCodeId();
    String getParentAccountTitle();
    String getAccountType();
    String getClassName();
    Integer getBranchesId();
    String getBranchName();
    Integer getSupplierCustomerId();
    Integer getCityId();
    String getMobilePersonal();
    String getTitle();
    Integer getCustomerGroupId();
    String getLastBillDate();
    Integer getLastBillsAmount();
    Integer getBillDays();
    String getLastPaidDate();
    Integer getLastPidAmount();
    Integer getPaidDays();
}
