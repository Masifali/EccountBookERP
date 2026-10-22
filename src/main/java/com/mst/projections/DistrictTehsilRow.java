package com.mst.projections;

/** Row of SP_City_GetAllMethod @MethodType='GetDistrictAndTehsilByCityId' (native). */
public interface DistrictTehsilRow {
    Integer getDistrictId();
    String getDistrict();
    Integer getTehsilId();
    String getTehsil();
}
