package com.mst.models.dto;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;

public class SaleDeliveryOrderRequest {
    public int id;
    public LocalDate docDate;
    public String deliveryOrderType;
    // Desktop Branch From is saved as BranchesId. Null preserves it for older clients.
    public Integer branchesId;
    public int saleTypeId;
    public int toBranchId;
    public int transporterId;
    public String vehicleType;
    public String vehicleNo;
    public String loadingInstructions;
    public boolean stockReserved;
    public List<Line> lines = new ArrayList<>();
    public List<Integer> removedLineIds = new ArrayList<>();
    // null means an older client did not submit the expense grid; [] deliberately removes its rows.
    public List<Expense> expenses;

    public static class Expense {
        public int id;
        public int saleOrderId;
        public int saleOrderCustomerExpId;
        public int itemId;
        public double quantity;
        public String remarks;
    }

    public static class Line {
        public int id;
        public int supplierCustomerId;
        public int saleOrderId;
        public int saleOrderDetailId;
        // Null preserves a saved schedule reference; zero explicitly clears it.
        public Integer deliveryScheduleId;
        public Integer deliveryScheduleDetailId;
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
