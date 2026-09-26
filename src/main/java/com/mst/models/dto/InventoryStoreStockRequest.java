package com.mst.models.dto;
import java.time.LocalDate;
import java.util.*;
public class InventoryStoreStockRequest {
 public LocalDate fromDate,toDate;
 public List<Integer> branchIds=new ArrayList<>(),parentIds=new ArrayList<>();
 public int categoryId,typeId,warehouseId,itemId,accountId,conditionId,rackId;
 public String reportType="ItemStockSummary";
 public boolean itemWise,skipZero,saleValue;
 public List<Rate> rates=new ArrayList<>();
 public static class Rate { public int itemId,packUomId,packingTypeId,conditionId;public java.math.BigDecimal rate; }
}
