package com.mst.projections;

import java.time.LocalDateTime;

/** Row of Sp_ItemTaxSchedule_GetAllMethod (ReadByOrganizationCompanyId) — JPQL projection. */
public interface ItemTaxScheduleReadRow {
    Integer getId();
    Integer getItemId();
    Integer getTaxTypeId();
    LocalDateTime getEffectedDate();
    Boolean getIsActive();
    LocalDateTime getEntryDate();
    Integer getEntryUser();
    LocalDateTime getModifyDate();
    Integer getModifyUser();
    Integer getOrganizationId();
    Integer getCompanyId();
    String getTaxName();
    String getItemName();
}
