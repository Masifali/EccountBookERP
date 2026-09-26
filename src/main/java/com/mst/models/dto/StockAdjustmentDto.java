package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 320 "Stock Adjustment" (frmStockAdjustment.cs, DocumentTypeId 70) — what the page posts.
 *
 * Row fields are the desktop grid's DataTable columns (frmStockAdjustment_Load:303-332), named
 * exactly as the page sends them. Organization, company, branch, financial year, users, DocNo and
 * every date except DocDate are filled by the service; they are never taken from the request.
 */
public class StockAdjustmentDto {

    public Integer Id = 0;                  // RecId
    public String  DocDate;                 // "yyyy-MM-dd"
    public String  Remarks;                 // txtremarks
    public Integer AdjustmentTypeId = 0;    // CmbEntryType.Value (1 Gain, 2 Loss)

    public List<Row> rows = new ArrayList<>();

    public static class Row {
        public Integer Id = 0;
        public Integer RefDocumentTypeId = 0;
        public Integer RefDocNoId = 0;
        public Integer RefDocSubIdNo = 0;
        public String  RefDocumentType;
        public String  RefDocDate;
        public Integer RefDocNo = 0;
        public Integer WareHouseId = 0;
        public String  WareHouse;
        public Integer ItemId = 0;
        public String  Item;
        public String  ItemCode;
        public Integer CropYearId = 0;
        public String  CropYear;
        public Integer JobLotId = 0;
        public String  JobLot;
        public Integer PackingTypeId = 0;
        public String  PackingType;
        public Integer ItemUOMId = 0;
        public String  ItemUOM;
        public Double  ItemUOMEquivalent = 0d;
        public Double  ItemQty = 0d;
        public Double  Weight = 0d;
        public Double  ItemRate = 0d;
        public Integer RateUomId = 0;
        public String  RateUom;
        public Double  RateUomEquivalent = 0d;
        public Double  ItemAmount = 0d;
        public Integer AccountId = 0;
        public String  Comments;
    }
}
