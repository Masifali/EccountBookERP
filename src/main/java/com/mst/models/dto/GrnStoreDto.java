package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 324 "Grn Store" (GrnStore.cs, DocumentTypeId 48) — what the page posts on Save / Update.
 *
 * Field names are the desktop's own: the header fields are the InvGrn properties Insert()
 * (GrnStore.cs:1094-1136) fills from controls, the rows are the columns of dtdetail
 * (InitializeComponentCustom, GrnStore.cs:340-376). Public fields, so Jackson binds the JSON keys
 * exactly as spelled here (the JS posts the same casing).
 *
 * Everything the server owns — organization, company, branch, financial year, users, entry and
 * modify dates, Doc No, attachments values — is filled by the service, never taken from here.
 */
public class GrnStoreDto {

    public Integer Id = 0;                    // Id (0 = Save, >0 = Update)
    public String  DocDate;                   // "yyyy-MM-dd" — DocDate picker
    public Integer SupplierCustomerId = 0;    // CmbBillToParty.Value
    public String  ReferenceDocNo;            // txtReferenceNo
    public Integer InwardGatePassId = 0;      // GPID (set only by a loader or ReadById)
    public String  GpNo;                      // CmbGpNo.Text (a TextBox on the desktop)
    public String  VehicleType;               // CmbVehicleType.Text
    public String  VehicleNo;                 // txtVehicleNo
    public String  BiltyNo;                   // txtBiltyNo
    public String  CarriageAmount;            // txtFreightAmount.Text (Conversion.ToDouble on the server)
    public Integer TransporterId = 0;         // CmbTransporter.Value
    public Integer TransporterSupCustId = 0;  // CmbTransporter.SelectedRow.Cells[3] (feature 4 only)
    public String  RemarksHeader;             // txtRemarksHeader
    public Integer BaseDocumentTypeId = 0;    // 0 none, 1 PO, 2 Demand, 3 Delivery Challan

    public List<Row> rows = new ArrayList<>();

    /** One dtdetail row (GrnStore.cs:340-376). */
    public static class Row {
        public Integer Id = 0;
        public Integer OrderId = 0;
        public Integer OrderDetailId = 0;
        public Integer OrderNo = 0;
        public Integer DemandId = 0;
        public Integer DemandDetailId = 0;
        public Integer DemandNo = 0;
        public Integer PurchasePreBillId = 0;
        public Integer PurchasePreBillDetailId = 0;
        public Integer PurchasePreBillNo = 0;
        public Integer DeliveryChallanId = 0;
        public Integer DeliveryChallanDetailId = 0;
        public Integer DeliveryChallanNo = 0;
        public Integer ItemId = 0;
        public String  ItemCode;
        public String  ItemName;
        public Integer WarehouseId = 0;
        public String  WarehouseName;
        public Integer RackId = 0;
        public String  RackName;
        public Integer ItemConditionId = 0;
        public Integer UOMId = 0;
        public String  UOM;
        public Double  UomEquivalent = 0d;
        public Double  TotalOrderQty = 0d;
        public Double  UsedOrderQty = 0d;
        public Double  BalOrderQty = 0d;
        public Double  TotalDemandQty = 0d;
        public Double  UsedDemandQty = 0d;
        public Double  BalDemandQty = 0d;
        public Double  PurchasePreBillQty = 0d;
        public Double  TotalDeliveryChallanQty = 0d;
        public Double  UsedDeliveryChallanQty = 0d;
        public Double  BalDeliveryChallanQty = 0d;
        public Double  ThisQty = 0d;
        public Double  AvailableStockQty = 0d;
        public String  Remarks;
    }
}
