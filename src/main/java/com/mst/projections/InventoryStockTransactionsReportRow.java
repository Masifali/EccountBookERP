package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of pcc.[USP-InventoryStockTransactionsReport] (single branch; native-EXEC, ReportCriteria + #Data r.* + Company/Organization) */
public interface InventoryStockTransactionsReportRow {
    String getReportCriteria();
    String getWareHouseName();
    Integer getItemId();
    String getItemCode();
    String getItemName();
    /** UOMCode, or variant ComputedName, or '' */
    String getItemUom();
    String getJobLotDescription();
    String getDocumentTypeDescription();
    Timestamp getDocDate();
    String getDocCodeNo();
    BigDecimal getCurrInQty();
    BigDecimal getCurrOutQty();
    /** running balance: sum(sum(BalQty)) OVER (ORDER BY RecNo) */
    BigDecimal getBalQty();
    BigDecimal getCurrInWeight();
    BigDecimal getCurrOutWeight();
    BigDecimal getCurrInBillWeight();
    BigDecimal getCurrOutBillWeight();
    /** running balance: sum(sum(BalWeight)) OVER (ORDER BY RecNo) */
    BigDecimal getBalWeight();
    BigDecimal getAmountIn();
    BigDecimal getAmountOut();
    String getCompCountry();
    String getCompContactPerson();
    String getCompMobileA();
    String getCompMobileB();
    String getCompMobileC();
    String getCompEmailA();
    String getCompEmailB();
    byte[] getCompLogoImage();
    String getCompanyWebsite();
    String getCompanyFaxNo();
    String getOrgReportingRemarks();
}
