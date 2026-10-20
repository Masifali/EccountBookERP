package com.mst.projections;

import java.sql.Timestamp;

/** Row of SP_City_GetAllMethod @MethodType='ReadById' (native; the city name is the un-aliased Description column). */
public interface CityReadByIdRow {
    Integer getId();
    String getCode();
    String getDescription();
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
