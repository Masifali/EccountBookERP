package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_InventoryTransactions_GenerateStocksReport_Store_Rpt (single branch; final SELECT from #s) */
public interface InventoryStocksReportStoreRow {
    Integer getItemId();
    String getDocumentTypeDescription();
    Timestamp getDocDate();
    String getDocCodeNo();
    String getWareHouseName();
    String getRackName();
    String getItemCode();
    String getItemName();
    String getUOMCode();
    BigDecimal getInQty();
    BigDecimal getOutQty();
    /** running balance: sum(sum(BalQty)) OVER (ORDER BY RecNo) */
    BigDecimal getBalQty();
    String getCompCountry();
    String getCompContactPerson();
    String getCompMobileA();
    String getCompMobileB();
    String getCompMobileC();
    String getCompEmailA();
    String getCompEmailB();
    /** the procedure emits CAST('' AS NVARCHAR(MAX)) AS CompLogoImage, so this is a String here (always empty) */
    String getCompLogoImage();
    String getCompanyWebsite();
    String getCompanyFaxNo();
    String getOrgReportingRemarks();
    String getBranchName();
    Integer getItemConditionId();
    String getItemCondition();
}
