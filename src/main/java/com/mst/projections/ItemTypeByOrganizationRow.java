package com.mst.projections;

import java.sql.Timestamp;

/** Row of Sp_ItemType_GetAllMethod @Activity='ReadByOrganizationCompanyId' (native; aliases kept as in SQL) */
public interface ItemTypeByOrganizationRow {
    Integer getId();
    String getTypeCode();
    String getTypeDescription();
    Integer getType();
    Timestamp getEntryDate();
    Integer getEntryUser();
    Timestamp getModifyDate();
    Integer getModifyUser();
    Timestamp getPostDate();
    Integer getPostUser();
    Boolean getPostState();
    Integer getOrganizationId();
    Integer getCompanyId();
    Integer getItemCategoryId();
    Integer getParentCategoryId();
    String getInvParentCateDescription();
    String getLookupName();
    Boolean getIsMother();
    String getMotherStatus();
}
