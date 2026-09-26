package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/** Screen 506 "Delivery Order Packing Material" - what Insert() (DeliveryOrderPackingMaterial.cs :582) reads. */
public class DeliveryOrderPmDto {

    public int Id;
    public String DocDate;
    public int BranchesId;
    public int ProjectsId;
    public String Remarks = "";          // txtremarks → LoadingInstructions
    public String VehicleType = "";      // cmbvehicletype.Text
    public String VehicleNo = "";

    public List<Row> rows = new ArrayList<>();
    public List<Integer> removedIds = new ArrayList<>();   // grd_ColumnButtonClick "Delete" on a saved row (update mode)

    /** One grd row (table columns of PurchsaeOrder_Load :287). */
    public static class Row {
        public int Id;
        public int BagTypeId;
        public int SupplierCustomerId;
        public int OrderId;
        public int ItemId;
        public int WareHouseId;
        public int ItemUOMId;
        public double QTY;
        public String Remarks = "";
    }
}
