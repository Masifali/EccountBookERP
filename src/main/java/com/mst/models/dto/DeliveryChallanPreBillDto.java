package com.mst.models.dto;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.fasterxml.jackson.annotation.JsonProperty;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 960 "Delivery Challan Against PreBill" (frmDeliveryChallanAgainstPurchasePreBill.cs,
 * DocumentTypeId 148) — what the page posts to /api/store/delivery-challan-prebill/save.
 *
 * Field names are exactly the JSON keys countx_store_delivery_challan_prebill.js sends (public
 * fields with @JsonProperty so Jackson never re-cases them). Only what the user can actually type
 * or choose is taken from here; every reference id, item, uom, condition, quantity basis and
 * expense value is re-read by the service from the pending loader or the stored document.
 */
@JsonIgnoreProperties(ignoreUnknown = true)
public class DeliveryChallanPreBillDto {

    @JsonProperty("Id")             public Integer Id = 0;             // RecId
    @JsonProperty("DocNo")          public Integer DocNo = 0;          // txtDocNo (display only — never trusted)
    @JsonProperty("DocDate")        public String  DocDate;            // txtDocDate "yyyy-MM-dd"
    /** true when txtDocDate still holds a value set by ReadById (it survives New) — the desktop then saves midnight. */
    @JsonProperty("DocDateFromOpened") public Boolean DocDateFromOpened = false;
    @JsonProperty("DeliveryTermId") public Integer DeliveryTermId = 0; // CmbDeliveryTerm
    @JsonProperty("CityId")         public Integer CityId = 0;         // CmbCityName
    @JsonProperty("VehicleNo")      public String  VehicleNo;          // txtVehicleNo
    @JsonProperty("BiltyNo")        public String  BiltyNo;            // txtBiltyNo
    @JsonProperty("RemarksHeader")  public String  RemarksHeader;      // txtRemarksMain

    /** grd rows, in grid order. */
    @JsonProperty("rows")       public List<Row> rows = new ArrayList<>();
    /** lstRemoveRecord — DeliveryChallanDetailIds of saved rows deleted with the X button. */
    @JsonProperty("removedIds") public List<Integer> removedIds = new ArrayList<>();
    /** grdInvExp rows, in grid order. */
    @JsonProperty("expenses")   public List<Exp> expenses = new ArrayList<>();

    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class Row {
        @JsonProperty("Id")                      public Integer Id = 0;                      // DeliveryChallanDetailId (0 = new)
        @JsonProperty("PurchasePreBillHeaderId") public Integer PurchasePreBillHeaderId = 0;
        @JsonProperty("PurchasePreBillDetailId") public Integer PurchasePreBillDetailId = 0;
        @JsonProperty("ThisQty")                 public Double  ThisQty = 0d;                // editable
        @JsonProperty("Remarks")                 public String  Remarks;                     // editable
    }

    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class Exp {
        @JsonProperty("Id")               public Integer Id = 0;               // DeliveryChallanExpenseDetailId (0 = from loader)
        @JsonProperty("PreBillId")        public Integer PreBillId = 0;
        @JsonProperty("PreBillExpenseId") public Integer PreBillExpenseId = 0;
        @JsonProperty("ItemId")           public Integer ItemId = 0;
        @JsonProperty("Remarks")          public String  Remarks;              // the only editable column
    }
}
