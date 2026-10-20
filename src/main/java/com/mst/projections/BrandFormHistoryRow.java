package com.mst.projections;

import java.sql.Timestamp;

/** Row of USP_Brand_GetAllMethod @Activity='FormHistory' (native; aliases kept as in SQL) */
public interface BrandFormHistoryRow {
    Integer getId();
    String getBrandCode();
    String getBrandName();
    Boolean getIsActive();
    Timestamp getEntryDate();
    Integer getEntryUserId();
    String getEntryUserName();
    Timestamp getModifyDate();
    Integer getModifyUserId();
    String getModifyUserName();
    Integer getOrganizationId();
    Integer getCompanyId();
}
