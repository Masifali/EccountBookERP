package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_Item_GetAllMethod @Activity='ItemWiseCommissionScheduleHistory' (native) */
public interface ItemCommissionHistoryRow {
    Integer getId();
    Integer getItemCategoryId();
    String getItemCategory();
    Integer getItemTypeId();
    String getItemType();
    Integer getItemId();
    String getItemName();
    BigDecimal getOnSale();
    BigDecimal getOnPurchase();
    Timestamp getEffectedFromDate();
    Timestamp getEntryDate();
    String getEntryUserName();
    Timestamp getModifyDate();
    String getModifyUserName();
}
