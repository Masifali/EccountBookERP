package com.mst.projections;

import java.time.LocalDateTime;

/** Row of Sp_UOMSchedule_GetAllMethod @Activity='ReadById' (JPQL) */
public interface UomScheduleReadByIdRow {
    Integer getId();
    Integer getItemId();
    Integer getScheduleUnitId();
    Double getEquivalent();
    LocalDateTime getEntryDate();
    Integer getEntryUser();
    LocalDateTime getModifyDate();
    Integer getModifyUser();
    Integer getOrganizationId();
    Integer getCompanyId();
    Double getQtyEquivalent();
    String getUomCode();
    String getScheduleUnitName();
    Boolean getBaseRateUom();
    Boolean getBasePackUom();
    Boolean getBaseSecondaryUom();
    Boolean getActive();
}
