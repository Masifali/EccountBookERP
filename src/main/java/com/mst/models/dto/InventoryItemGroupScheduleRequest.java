package com.mst.models.dto;
import java.util.*;
public class InventoryItemGroupScheduleRequest {
    public int id,groupId,unitId;
    public String equivalent;
    public static class Assignment {public int groupId;public List<Integer> itemIds=new ArrayList<>();}
}
