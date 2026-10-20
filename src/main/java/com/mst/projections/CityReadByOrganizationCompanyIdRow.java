package com.mst.projections;

import java.sql.Timestamp;

/** Row of SP_City_GetAllMethod @MethodType='ReadByOrganizationCompanyId' (native; no TehsilId column in this branch). */
public interface CityReadByOrganizationCompanyIdRow {
    Integer getId();
    String getCode();
    String getCityName();
    String getCityNameOtherLingo();
    Timestamp getEntryDate();
    Integer getEntryUser();
    Timestamp getModifyDate();
    Integer getModifyUser();
    Timestamp getPostDate();
    Integer getPostUser();
    Boolean getPostState();
    Integer getOrganizationId();
    Integer getCompanyId();
    Integer getCountryId();
    String getCountryName();
    String getTehsil();
}
