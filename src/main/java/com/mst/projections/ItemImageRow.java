package com.mst.projections;

import java.time.LocalDateTime;

/** Row of Sp_Item_GetAllMethod @Activity='ItemImagesByItemId' (JPQL) */
public interface ItemImageRow {
    Integer getId();
    Integer getItemId();
    String getImagePath();
    LocalDateTime getEffetedDate();
    Boolean getIsActive();
    String getFileName();
    Boolean getIsProfileImage();
    Integer getSortNo();
}
