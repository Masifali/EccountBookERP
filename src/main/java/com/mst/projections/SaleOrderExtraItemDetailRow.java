package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_SaleOrder_GetAllMethod (ReadBySaleOrderId_ExtraItemDetail) - native projection. */
public interface SaleOrderExtraItemDetailRow {
    Integer getId();
    Integer getSaleOrderId();
    Integer getMasterItemId();
    String getMasterItemName();
    String getMasterItemCode();
    Integer getMasterPackUomId();
    String getMasterPackUomCode();
    String getMasterPackUomEquivalent();
    Integer getItemId();
    String getItemName();
    String getItemCode();
    Integer getPackUomId();
    String getPackUomCode();
    String getPackUomEquivalent();
    String getItemDiscription();
    String getRemarks();
    Integer getJobLotId();
    String getJobLotDescription();
    Integer getPackingTypeId();
    String getPackingType();
    BigDecimal getItemQty();
    BigDecimal getNetWeight();
}
