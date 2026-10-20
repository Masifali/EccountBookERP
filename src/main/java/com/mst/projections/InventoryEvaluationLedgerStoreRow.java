package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of USP_InventoryEvaluationLedgerStore (native-EXEC; final grouped SELECT from #AllData) */
public interface InventoryEvaluationLedgerStoreRow {
    String getReportCriteria();
    Integer getDocumentTypeId();
    String getDocumentTypeDescription();
    Timestamp getTranDate();
    Integer getId();
    Integer getDocNo();
    Integer getSupplierCustomerId();
    String getDescription();
    Integer getItemId();
    String getWareHouseCode();
    Integer getRackId();
    String getRackName();
    String getCategoryDescription();
    String getTypeDescription();
    String getItemName();
    BigDecimal getPackSize();
    Integer getItemconditionId();
    String getItemCondition();
    BigDecimal getItemRate();
    BigDecimal getOp_Qty();
    BigDecimal getQtyIn();
    BigDecimal getQtyOut();
    BigDecimal getBalQty();
    BigDecimal getQtyRunBal();
    BigDecimal getOp_Weight();
    BigDecimal getWeightIn();
    BigDecimal getWeightOut();
    BigDecimal getBalWeight();
    BigDecimal getWeightRunBal();
    BigDecimal getOp_Amount();
    BigDecimal getAmountIn();
    BigDecimal getAmountOut();
    BigDecimal getBalAmount();
    BigDecimal getAmountRunBal();
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
    Timestamp getEntryDate();
    String getBranchName();
}
