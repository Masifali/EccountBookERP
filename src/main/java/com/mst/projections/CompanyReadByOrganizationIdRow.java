package com.mst.projections;

import java.sql.Timestamp;

/** Row of Sp_Company_GetAllMethod @Activity='ReadByOrganizationId' (native). */
public interface CompanyReadByOrganizationIdRow {
    Integer getId();
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
    Integer getOrgCompanyTypeId();
    String getPictureURL();
    byte[] getCompLogoImage();
    Timestamp getEntryDate();
    Integer getEntryUser();
    Timestamp getModifyDate();
    Integer getModifyUser();
    String getCityName();
    Integer getCompanyTemplateId();
    Integer getAllowedUserCount();
    Integer getCompanyTypeId();
    Boolean getIsHeadOffice();
    String getCompanyNameOtherLanguage();
    String getCompanyAddressOtherLanguage();
    String getCompanyWebsite();
    String getCompanyFaxNo();
    String getCompanyNameOtherLing();
    String getCompanyAddressOtherLing();
    Integer getCompanyId();
}
