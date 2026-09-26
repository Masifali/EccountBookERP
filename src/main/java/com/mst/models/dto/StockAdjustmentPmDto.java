package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/** Screen 492 "Stock Adjustment For PM" - what Insert() (StockAdjustmentForPM.cs :933) reads. */
public class StockAdjustmentPmDto {

    public int Id;                          // RecId
    public String DocDate;                  // yyyy-MM-dd
    public int AdjustmentTypeId;            // CmbEntryType (1 Gain, 2 Loss)
    public String RemarksHeader = "";

    public List<Row> rows = new ArrayList<>();

    /** One grd row (table columns of frmStockAdjustment_Load :255). */
    public static class Row {
        public int Id;
        public int ItemId;
        public int WareHouseId;
        public int RackId;
        public int ItemConditionId;
        public double ItemQty;
        public double ItemRate;
        public double ItemAmount;
        public int AccountId;
        public String Comments = "";
    }
}
