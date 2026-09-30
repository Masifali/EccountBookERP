package com.mst.models.dto;

import java.util.List;

public class ItemPmMapDto {
    private int id;
    private int itemId;
    private int baseUomId;
    private String remarks;
    private boolean active = true;
    private List<Detail> details;

    public static class Detail {
        private int id;
        private int pmItemId;
        private int baseUomId;
        private double pmQty;
        private double weightCapacity;
        private String remarks;

        public int getId() { return id; }
        public void setId(int id) { this.id = id; }
        public int getPmItemId() { return pmItemId; }
        public void setPmItemId(int pmItemId) { this.pmItemId = pmItemId; }
        public int getBaseUomId() { return baseUomId; }
        public void setBaseUomId(int baseUomId) { this.baseUomId = baseUomId; }
        public double getPmQty() { return pmQty; }
        public void setPmQty(double pmQty) { this.pmQty = pmQty; }
        public double getWeightCapacity() { return weightCapacity; }
        public void setWeightCapacity(double weightCapacity) { this.weightCapacity = weightCapacity; }
        public String getRemarks() { return remarks; }
        public void setRemarks(String remarks) { this.remarks = remarks; }
    }

    public int getId() { return id; }
    public void setId(int id) { this.id = id; }
    public int getItemId() { return itemId; }
    public void setItemId(int itemId) { this.itemId = itemId; }
    public int getBaseUomId() { return baseUomId; }
    public void setBaseUomId(int baseUomId) { this.baseUomId = baseUomId; }
    public String getRemarks() { return remarks; }
    public void setRemarks(String remarks) { this.remarks = remarks; }
    public boolean isActive() { return active; }
    public void setActive(boolean active) { this.active = active; }
    public List<Detail> getDetails() { return details; }
    public void setDetails(List<Detail> details) { this.details = details; }
}
