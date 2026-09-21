package com.mst.models.dto;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;
public class InventoryMinMaxRequest {
    public LocalDateTime effectedDate;
    public List<Row> rows;
    public static class Row {
        public int itemId, rateUomId;
        public BigDecimal rateUom, minRate, maxRate;
    }
    public static class Filter {
        public int parent, category, type, item;
        public LocalDate date;
    }
    public static class Delete { public List<Integer> ids; }
}
