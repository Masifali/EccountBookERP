package com.mst.projections;

import java.sql.Timestamp;

/** Row of Sp_Company_GetAllMethod @Activity='FormHistory' (native). */
public interface CompanyFormHistoryRow {
    Integer getId();
    Integer getOrgCompanyTypeId();
    String getOrgName();
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
    String getPictureURL();
    Integer getEntryUser();
    Integer getModifyUser();
    Timestamp getEntryDate();
    Timestamp getModifyDate();
    Integer getAllowedUserCount();
    byte[] getCompLogoImage();
    Integer getCompanyTypeId();
    Boolean getIsHeadOffice();
    String getCompanyNameOtherLanguage();
    String getCompanyAddressOtherLanguage();
    String getCompanyWebsite();
    String getCompanyFaxNo();
    String getCompanyNameOtherLing();
    String getCompanyAddressOtherLing();
    Integer getCompanyTemplateId();
    String getCityName();
    String getFirstName();
    String getLastName();
    Integer getCompanyId();
}
