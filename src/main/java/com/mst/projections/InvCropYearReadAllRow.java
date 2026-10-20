package com.mst.projections;

import java.time.LocalDateTime;

/** Row of Sp_InvCropYear_GetAllMethod @Activity='ReadAll' (JPQL) */
public interface InvCropYearReadAllRow {
    Integer getId();
    String getCropYear();
    Boolean getIsActive();
    String getEntryUserName();
    LocalDateTime getEntryDate();
    String getModifyUserName();
    LocalDateTime getModifyDate();
}
