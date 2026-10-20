package com.mst.projections;

/** Row of usp_getAllUomsByCompanyId (JPQL) */
public interface UomsByCompanyRow {
    Integer getId();
    String getUomCode();
    Double getEquivalent();
    Boolean getBaseRateUom();
    Integer getItemId();
    Boolean getBasePackUom();
    Boolean getBaseSecondaryUom();
    Double getQtyEquivalent();
    String getUomDescription();
    Boolean getActive();
    String getItemName();
    String getItemCode();
}
