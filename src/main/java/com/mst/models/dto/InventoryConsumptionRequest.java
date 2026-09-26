package com.mst.models.dto;
import java.util.List;
public class InventoryConsumptionRequest {
    public List<Row> rows;
    public static class Row { public int id,itemId;public boolean active=true;public String remarks=""; }
}
