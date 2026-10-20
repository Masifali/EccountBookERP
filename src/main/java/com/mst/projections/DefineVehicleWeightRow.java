package com.mst.projections;

import java.math.BigDecimal;

/** Row of Sp_WbTransation_GetAllMethod @Activity='GetVehicleWeight' (native, DefineVehicleWeight) */
public interface DefineVehicleWeightRow {
    Integer getId();
    String getVehicleType();
    String getVehicleNo();
    BigDecimal getVehicleNetWeight();
}
