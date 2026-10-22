package com.mst.projections;

import java.sql.Timestamp;

/** Row of Sp_UOMSchedule_GetAllMethod @Activity='ReadByOrganizationCompanyId' (native; aliases kept as in SQL) */
public interface UomScheduleByOrganizationRow {
    Integer getId();
    Integer getItemId();
    String getItemCode();
    Integer getScheduleUnitId();
    Double getEquivalent();
    Timestamp getEntryDate();
    Integer getEntryUser();
    Timestamp getModifyDate();
    Integer getModifyUser();
    Integer getOrganizationId();
    Integer getCompanyId();
    String getUOMDescription();
    String getItemName();
    String getUOMCode();
    Double getQtyEquivalent();
    Boolean getBaseRateUom();
    Boolean getBasePackUom();
    Boolean getBaseSecondaryUom();
    Boolean getActive();
}
