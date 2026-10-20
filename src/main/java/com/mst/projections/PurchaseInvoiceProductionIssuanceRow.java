package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_InvPurchaseInvoice_GetAllMethod @Activity='GetInvoicesDetailForProductionIssence' */
public interface PurchaseInvoiceProductionIssuanceRow {
    Integer getId();
    Timestamp getDocDate();
    Integer getDocNo();
    Integer getSupplierCustomerId();
    String getSupplierName();
    String getManualBillNo();
    Integer getItemId();
    Integer getItemUOMId();
    BigDecimal getUOMCodeItem();
    String getCropYear();
    Integer getJobLotId();
    String getJobLotCode();
    Integer getPackingTypeId();
    String getPackTypeCode();
    String getWareHouseName();
    Integer getWarehouseId();
    String getItemName();
    Integer getRateUOMId();
    BigDecimal getRateUOM();
    BigDecimal getItemQty();
    BigDecimal getNetBillWeight();
    BigDecimal getItemRate();
    BigDecimal getItemAmount();
    Integer getCurrencyId();
    BigDecimal getExchangeRate();
    BigDecimal getFcyAmount();
}
