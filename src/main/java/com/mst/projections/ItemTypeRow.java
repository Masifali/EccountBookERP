package com.mst.projections;

import java.time.LocalDateTime;

/** Row of Sp_ItemType_GetAllMethod @Activity='ReadById' (JPQL) */
public interface ItemTypeRow {
    Integer getId();
    String getTypeCode();
    String getTypeDescription();
    Integer getType();
    LocalDateTime getEntryDate();
    Integer getEntryUser();
    LocalDateTime getModifyDate();
    Integer getModifyUser();
    LocalDateTime getPostDate();
    Integer getPostUser();
    Boolean getPostState();
    Integer getOrganizationId();
    Integer getCompanyId();
    Integer getParentCategoryId();
    Boolean getIsMother();
}
