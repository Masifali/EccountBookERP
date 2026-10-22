package com.mst.projections;

import java.sql.Timestamp;

/** Row of Sp_Company_GetAllMethod @Activity='GetHistory' (native). */
public interface CompanyGetHistoryRow {
    Integer getId();
    Integer getOrgCompanyTypeId();
    String getCompCode();
    String getCompName();
    String getCompContactPerson();
    String getCompEmailA();
    String getCompEmailB();
    String getCompTel();
    String getCompMobileA();
    String getCompMobileB();
    String getCompMobileC();
    String getCompAddress();
    String getCompReportingTitle();
    String getCompLogo();
    String getCompCountry();
    String getCompState();
    String getCompBaseCurr();
    String getCompType();
    Integer getEntryUser();
    Timestamp getEntryDate();
    Integer getModifyUser();
    Timestamp getModifyDate();
    Integer getAllowedUserCount();
    String getPictureURL();
    Integer getCompanyId();
}
