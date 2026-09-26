package com.mst.models.dto;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;

public class SaleDeliveryOrderRequest {
    public int id;
    public LocalDate docDate;
    public String deliveryOrderType;
    public int saleTypeId;
    public int toBranchId;
    public int transporterId;
    public String vehicleType;
    public String vehicleNo;
    public String loadingInstructions;
    public boolean stockReserved;
    public List<Line> lines = new ArrayList<>();
    public List<Integer> removedLineIds = new ArrayList<>();

    public static class Line {
        public int id;
        public int supplierCustomerId;
        public int saleOrderId;
        public int saleOrderDetailId;
        public int itemId;
        public int packUomId;
        public int packingTypeId;
        public int warehouseId;
        public int jobLotId;
        public int cropYearId;
        public int refPartyId;
        public int refDocumentTypeId;
        public int refDocIdNo;
        public int refDocSubIdNo;
        public double quantity;
        public double weight;
        public double packingUnit;
        public double packingWeight;
        public double grossWeight;
        public double rate;
        public double rateUom;
        public int rateUomId;
        public String remarks;
    }
}
