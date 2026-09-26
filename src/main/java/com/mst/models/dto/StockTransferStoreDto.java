package com.mst.models.dto;

import com.fasterxml.jackson.annotation.JsonProperty;
import java.util.ArrayList;
import java.util.List;

/**
 * Screen 505 "Stock Transfer Store" (frmStockTransferStore.cs, DocumentTypeId 807) - what the page posts.
 * Row fields carry the desktop grid's column keys ("table", PurchsaeOrder_Load:364). DocNo, users, dates,
 * company and DocumentTypeId are never read from here; ItemAmount and Expense are recomputed.
 */
public class StockTransferStoreDto {
    @JsonProperty("Id")            public Integer Id = 0;
    @JsonProperty("DocDate")       public String DocDate;
    @JsonProperty("RemarksHeader") public String RemarksHeader;
    @JsonProperty("FromBranchId")  public Integer FromBranchId = 0;
    @JsonProperty("ToBranchId")    public Integer ToBranchId = 0;
    @JsonProperty("rows")          public List<Row> rows = new ArrayList<>();
    @JsonProperty("expenses")      public List<Expense> expenses = new ArrayList<>();

    public static class Row {
        @JsonProperty("Id")                public Integer Id = 0;
        @JsonProperty("ItemId")            public Integer ItemId = 0;
        @JsonProperty("WareHouseFromId")   public Integer WareHouseFromId = 0;
        @JsonProperty("RackFromId")        public Integer RackFromId = 0;
        @JsonProperty("WareHouseToId")     public Integer WareHouseToId = 0;
        @JsonProperty("RackToId")          public Integer RackToId = 0;
        @JsonProperty("ItemConditionId")   public Integer ItemConditionId = 0;
        @JsonProperty("PackUOMId")         public Integer PackUOMId = 0;
        @JsonProperty("QTY")               public Double QTY = 0d;
        @JsonProperty("ItemRate")          public Double ItemRate = 0d;
        @JsonProperty("RefDocumentTypeId") public Integer RefDocumentTypeId = 0;
        @JsonProperty("RefDocId")          public Integer RefDocId = 0;
        @JsonProperty("RefDocInvoiceId")   public Integer RefDocInvoiceId = 0;
        @JsonProperty("Remarks")           public String Remarks;
    }

    public static class Expense {
        @JsonProperty("Account")    public Integer Account = 0;
        @JsonProperty("Percentage") public Double Percentage = 0d;
        @JsonProperty("Qty")        public Double Qty = 0d;
        @JsonProperty("Rate")       public Double Rate = 0d;
        @JsonProperty("Amount")     public Double Amount = 0d;
        @JsonProperty("Remarks")    public String Remarks;
    }
}
