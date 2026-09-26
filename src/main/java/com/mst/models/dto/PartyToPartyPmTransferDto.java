package com.mst.models.dto;

import com.fasterxml.jackson.annotation.JsonProperty;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 335 "Packing Material Transfer (Party to Party)"
 * ({@code PartyToPartyPackingMaterialTransfer.cs}, DocumentTypeId 125) — what the page posts.
 *
 * Row fields are the columns of the form's grid table (Form_Load:257-270). Doc No is not here:
 * txtVoucherCode is ReadOnly on the desktop, so the server takes the generator's number (new) or
 * the stored header's (update). Organization, company, users and the entry/modify/approved dates
 * are the signed-in user's and the server clock's.
 */
public class PartyToPartyPmTransferDto {

    @JsonProperty("Id")            public Integer Id = 0;          // RecId (0 = btnSave, > 0 = btnUpdate)
    @JsonProperty("DocDate")       public String  DocDate;         // datDocDate, "yyyy-MM-dd"
    @JsonProperty("RemarksHeader") public String  RemarksHeader;   // txtRemarksHeader

    @JsonProperty("rows")          public List<Row> rows = new ArrayList<>();

    public static class Row {
        @JsonProperty("Id")              public Integer Id = 0;               // detail id (loaded rows)
        @JsonProperty("ItemId")          public Integer ItemId = 0;
        @JsonProperty("ItemCode")        public String  ItemCode;
        @JsonProperty("ItemName")        public String  ItemName;
        @JsonProperty("PackUomId")       public Integer PackUomId = 0;
        @JsonProperty("PackUom")         public String  PackUom;
        @JsonProperty("ItemConditionId") public Integer ItemConditionId = 0;
        @JsonProperty("ItemCondition")   public String  ItemCondition;
        @JsonProperty("Qty")             public String  Qty;                  // the cell text (txtItemQty.Text)
        @JsonProperty("Remarks")         public String  Remarks;
        @JsonProperty("SupplierFromId")  public Integer SupplierFromId = 0;
        @JsonProperty("SupplierFrom")    public String  SupplierFrom;
        @JsonProperty("SupplierToId")    public Integer SupplierToId = 0;
        @JsonProperty("SupplierTo")      public String  SupplierTo;
    }
}
