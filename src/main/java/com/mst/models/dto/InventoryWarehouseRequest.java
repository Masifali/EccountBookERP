package com.mst.models.dto;

import java.util.List;

public class InventoryWarehouseRequest {
    public int id, warehouseType;
    public String code, name;
    public boolean active = true;
    public static class Allocation {
        public int branchId;
        public List<Integer> warehouseIds;
        public boolean allocate;
    }
    public static class Rack {
        public int id, warehouseId, sortNo;
        public String name;
        public boolean active = true;
    }
    public static class RackItems {
        public int rackId;
        public List<Integer> itemIds;
        public boolean active = true;
    }
}
