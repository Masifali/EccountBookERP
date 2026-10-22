package com.mst.projections;

import java.time.LocalDate;
import java.time.LocalDateTime;

/** Row of Proc_InvLookup_ReadById and Sp_InvLookup_GetAllMethod @Activity='ReadByInvlookTypeId' (JPQL) */
public interface InvLookUpRow {
    Integer getId();
    Integer getInvLookupTypeId();
    Integer getCode();
    String getLookupName();
    Integer getEntryUser();
    LocalDate getEntryDate();
    Integer getModifyUser();
    String getModifyDate();
    LocalDateTime getPostDate();
    Integer getPostUser();
    Boolean getPostState();
    Integer getOrganizationId();
    Integer getCompanyId();
    String getProfileTypeName();
}
