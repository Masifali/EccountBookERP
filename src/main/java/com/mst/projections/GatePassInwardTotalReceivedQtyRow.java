package com.mst.projections;

import java.util.Date;

/** Row of Sp_GatePassInward_GetAllMethod @Activity='TotalReceivedQtyByPurchaseOrderId' (native). */
public interface GatePassInwardTotalReceivedQtyRow {
    Integer getPurchaseOrderId();
    Integer getOrderItemId();
    Integer getVehicleNo();
    Double getOrderItemQty();
    Integer getNoOfPackages();
    Double getBalQty();
    Date getGpDate();
}
