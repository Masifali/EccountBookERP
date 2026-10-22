package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='GrnGhallaMandiLoadForInvoice' */
public interface PurchaseInvoiceGhallaMandiGrnLoadRow {
    Integer getInvGrnDetailId();
    Integer getInvGrnId();
    Timestamp getDocDate();
    Integer getDocNo();
    Integer getGhallaMandiId();
    String getGhallaMandi();
    Integer getSupplierCustomerId();
    String getLoadingContractor();
    String getAccountTitle();
    BigDecimal getCarriageAmount();
    String getRemarksHeader();
    Integer getItemId();
    String getItemCode();
    String getItemName();
    String getCropYear();
    Integer getItemUOMId();
    String getUOMCodeItem();
    BigDecimal getItemQty();
    Integer getWarehouseId();
    String getWareHouseName();
    Integer getPackingTypeId();
    String getLabAnalisysNo();
    BigDecimal getGrossWeight();
    BigDecimal getEBWPerUnit();
    BigDecimal getEBTotalWt();
    BigDecimal getWtCut();
    BigDecimal getWeightCutTotal();
    BigDecimal getAdLsWeight();
    BigDecimal getScaleKart();
    BigDecimal getNetBillWeight();
    BigDecimal getNetStockWeight();
    String getAreaCity();
    BigDecimal getItemRate();
    Integer getUomScheduleIdRate();
    BigDecimal getEquivalentPoRate();
    BigDecimal getItemAmount();
    Integer getJobLotId();
    String getCommentsDetail();
    String getDeliveryTerm();
    Integer getPurchaseGLAC();
    Integer getGpNo();
    String getVehicleNo();
    Integer getInwardGatePassId();
    BigDecimal getDeductionWeight();
    BigDecimal getDeductionRate();
}
