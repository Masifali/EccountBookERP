package com.mst.projections;

import java.sql.Timestamp;

/** Row of SP_City_GetAllMethod @MethodType='GetAll' (native). */
public interface CityRow {
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
    Integer getTehsilId();
    String getCountryName();
    String getTehsil();
}
