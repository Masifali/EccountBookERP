package com.mst.models.dto;

import com.fasterxml.jackson.annotation.JsonProperty;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 339 "Stock Transfer" (frmStockTransfer.cs, DocumentTypeId 68) — what the page posts.
 *
 * Row fields carry the desktop grid's column keys ("table", frmStockTransfer_Load:391), except
 * "Ad/LsWeight", which travels as AdLsWeight. Public fields with @JsonProperty so Jackson keeps the
 * exact casing the page sends. Organization, company, branch, financial year, users, entry dates,
 * DocNo and DocumentTypeId are never read from here — the service fills them from the session.
 */
public class StoreStockTransferDto {

    @JsonProperty("Id")               public Integer Id = 0;                 // frmStockTransfer.Id (RECID)
    @JsonProperty("DocDate")          public String  DocDate;                // DocDate picker, "yyyy-MM-dd"
    @JsonProperty("TransferType")     public String  TransferType;           // CmbTransferType.Text
    @JsonProperty("GatePassId")       public Integer GatePassId = 0;         // cmbGatePass.Value
    @JsonProperty("WbTicketId")       public Integer WbTicketId = 0;         // cmbTicketNo.Value
    @JsonProperty("WbNetWeight")      public Double  WbNetWeight = 0d;       // txtHeadNetWeight
    @JsonProperty("OtherWeight")      public Double  OtherWeight = 0d;       // txtOtherWeight
    @JsonProperty("NoOfBranches")     public Integer NoOfBranches = 0;       // txtNoofBranches (hidden)
    @JsonProperty("RefDocumentTypeId") public Integer RefDocumentTypeId = 0; // form field RefDocumentTypeId
    @JsonProperty("RemarksHeader")    public String  RemarksHeader;          // txtRemarksHead

    @JsonProperty("rows")     public List<Row> rows = new ArrayList<>();
    @JsonProperty("expenses") public List<Expense> expenses = new ArrayList<>();

    /** One row of "table" (grd). */
    public static class Row {
        @JsonProperty("FlagForSplitAndDelete") public Boolean FlagForSplitAndDelete = false;
        @JsonProperty("Id")               public Integer Id = 0;
        @JsonProperty("RefDocumentTypeId") public Integer RefDocumentTypeId = 0;
        @JsonProperty("RefDocNoId")       public Integer RefDocNoId = 0;
        @JsonProperty("RefDocSubIdNo")    public Integer RefDocSubIdNo = 0;
        @JsonProperty("ItemId")           public Integer ItemId = 0;
        @JsonProperty("ItemName")         public String  ItemName;
        @JsonProperty("PackTypeId")       public Integer PackTypeId = 0;
        @JsonProperty("PackType")         public String  PackType;
        @JsonProperty("CropYear")         public String  CropYear;
        @JsonProperty("JobLotId")         public Integer JobLotId = 0;
        @JsonProperty("JobLot")           public String  JobLot;
        @JsonProperty("JobLotIdTo")       public Integer JobLotIdTo = 0;
        @JsonProperty("JobLotTo")         public String  JobLotTo;
        @JsonProperty("QTY")              public Double  QTY = 0d;
        @JsonProperty("PackUOMId")        public Integer PackUOMId = 0;
        @JsonProperty("PackUOM")          public String  PackUOM;
        @JsonProperty("GrossWeight")      public Double  GrossWeight = 0d;
        @JsonProperty("EbUnit")           public Double  EbUnit = 0d;
        @JsonProperty("EbTotal")          public Double  EbTotal = 0d;
        @JsonProperty("AdLsWeight")       public Double  AdLsWeight = 0d;        // grid key "Ad/LsWeight"
        @JsonProperty("NetWeight")        public Double  NetWeight = 0d;
        @JsonProperty("BalQty")           public Double  BalQty = 0d;
        @JsonProperty("BalWeight")        public Double  BalWeight = 0d;
        @JsonProperty("WareHouseFromId")  public Integer WareHouseFromId = 0;
        @JsonProperty("WareHouseFrom")    public String  WareHouseFrom;
        @JsonProperty("WareHouseToId")    public Integer WareHouseToId = 0;
        @JsonProperty("WareHouseTo")      public String  WareHouseTo;
        @JsonProperty("Remarks")          public String  Remarks;
        @JsonProperty("ItemRate")         public Double  ItemRate = 0d;
        @JsonProperty("RateUOMId")        public Integer RateUOMId = 0;
        @JsonProperty("RateUOM")          public Double  RateUOM = 0d;
        @JsonProperty("ItemAmount")       public Double  ItemAmount = 0d;
        @JsonProperty("Expense")          public Double  Expense = 0d;
        @JsonProperty("TransferDocumentTypeId") public Integer TransferDocumentTypeId = 0;
        @JsonProperty("TransferId")       public Integer TransferId = 0;
        @JsonProperty("TransferDetailId") public Integer TransferDetailId = 0;
    }

    /** One row of dtExp (grdOtherCharges, "Other Charges" tab). */
    public static class Expense {
        @JsonProperty("Account")    public Integer Account = 0;
        @JsonProperty("Percentage") public Double  Percentage = 0d;
        @JsonProperty("Qty")        public Double  Qty = 0d;
        @JsonProperty("Rate")       public Double  Rate = 0d;
        @JsonProperty("Amount")     public Double  Amount = 0d;
        @JsonProperty("Remarks")    public String  Remarks;
    }
}
