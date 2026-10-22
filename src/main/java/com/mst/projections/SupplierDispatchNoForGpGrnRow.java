package com.mst.projections;

import java.math.BigDecimal;

/** Row of USP_SupplierDispatch_GetNoForGpandGrn (native) */
public interface SupplierDispatchNoForGpGrnRow {
    Integer getSupplierDispatchId();
    Integer getSupplierDispatchNo();
    String getVehicleNo();
    Double getAdvanceFreight();
    String getBiltyNo();
    BigDecimal getFreight();
    BigDecimal getSupplierWbWeight();
    Integer getCityId();
}
