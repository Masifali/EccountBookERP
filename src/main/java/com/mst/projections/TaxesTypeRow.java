package com.mst.projections;

import java.time.LocalDateTime;

/** Row of Sp_TaxesTypes_GetAllMethod @Activity='ReadByOrganizationCompanyId' (JPQL). */
public interface TaxesTypeRow {
    Integer getId();
    String getTaxName();
    String getTaxDescription();
    Integer getTaxGLAccountId();
    Integer getType();
    LocalDateTime getEntryDate();
    Integer getEntryUser();
    LocalDateTime getModifyDate();
    Integer getModifyUser();
    Integer getOrganizationId();
    Integer getCompanyId();
    String getAccountTitle();
}
