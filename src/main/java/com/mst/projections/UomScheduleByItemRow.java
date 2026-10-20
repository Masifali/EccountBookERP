package com.mst.projections;

import java.time.LocalDateTime;

/** Row of Sp_UOMSchedule_GetAllMethod @Activity='ReadByItemID' (JPQL) */
public interface UomScheduleByItemRow {
    Integer getId();
    Integer getItemId();
    Integer getScheduleUnitId();
    Double getEquivalent();
    String getUomCode();
    LocalDateTime getEntryDate();
    Integer getEntryUser();
    LocalDateTime getModifyDate();
    Integer getModifyUser();
    Integer getOrganizationId();
    Integer getCompanyId();
    Double getQtyEquivalent();
    String getItemName();
    String getScheduleUnitName();
    Boolean getBaseRateUom();
    Boolean getBasePackUom();
    Boolean getBaseSecondaryUom();
    Boolean getActive();
}
