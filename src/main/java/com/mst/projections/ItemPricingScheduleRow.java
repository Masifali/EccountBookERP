package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_Item_GetAllMethod @Activity='GetItemsFromItemPricingSchedule' (native) */
public interface ItemPricingScheduleRow {
    Long getRecordNo();
    Integer getId();
    String getScheduleDesc();
    Timestamp getEffectedDate();
    Integer getItemId();
    String getItemName();
    Integer getPriceTypeId();
    BigDecimal getItemPrice();
}
