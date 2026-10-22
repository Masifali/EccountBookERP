package com.mst.projections;

import java.sql.Timestamp;

/** Row of Sp_UOM_GetAllMethod @Activity='ReadById' / 'ReadByOrganizationCompanyId' (native; aliases kept as in SQL) */
public interface UomRow {
    Integer getId();
    String getUOMCode();
    String getUOMDescription();
    String getUOMSymbol();
    Boolean getUOMStatus();
    Timestamp getEntryDate();
    Integer getEntryUser();
    Timestamp getModifyDate();
    Integer getModifyUser();
    Integer getOrganizationId();
    Integer getCompanyId();
    Double getEquivalent();
    Double getQtyEquivalent();
    Integer getParentUOMId();
    String getParentUomCode();
}
