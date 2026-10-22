package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Date;

/** Row of usp_ItemLedgerFromStockEvaluations (native-EXEC). Qty/Weight/Amount/Rate columns are SQL float - exposed as BigDecimal per the naming rule (Spring converts). */
public interface ItemLedgerRow {
    String getTrancDate();
    String getDocNo();
    Integer getRefDocumentTypeId();
    String getDocType();
    Integer getSupplierCustomerId();
    String getPartyName();
    Integer getWarehouseId();
    String getWareHouseName();
    String getCropYear();
    Integer getJobLotId();
    String getJobLot();
    Integer getPackingTypeId();
    String getPackTypeDesc();
    String getStockHoldStatus();
    Integer getItemId();
    String getItemName();
    Integer getItemUomId();
    String getPackUom();
    BigDecimal getQtyIn();
    BigDecimal getQtyOut();
    BigDecimal getBalQty();
    BigDecimal getRateIn();
    String getRateUom();
    BigDecimal getWeightIn();
    BigDecimal getWeightOut();
    BigDecimal getBalWeight();
    BigDecimal getRateOut();
    BigDecimal getAmountIn();
    BigDecimal getAmountOut();
    BigDecimal getBalAmount();
    BigDecimal getAvgRate();
    Date getFromDate();
    Date getToDate();
    Long getRowNum();
}
