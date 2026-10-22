package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Timestamp;

/** Row of Sp_Item_GetAllMethod @Activity='FormHistoryForFixedAssets' (native) */
public interface ItemFixedAssetsFormHistoryRow {
    Long getRecordNo();
    Integer getId();
    String getItemCodeNew();
    String getAssetCode();
    String getAssetName();
    String getAssetCategory();
    String getAssetType();
    Timestamp getEntryDate();
    String getUserName();
    Long getNoOfAttachments();
    String getAccumulated_Depreciation();
    String getDepreciation_Expense();
    Integer getItemQcGradeId();
    String getItemQcGrade();
    String getClassGroupName();
    Integer getMaxMeaurementUnitId();
    BigDecimal getWeightKgs();
    BigDecimal getMaxMeaurement();
    String getInvParentCateDescription();
    Boolean getItemStatus();
    String getAssets_GL();
    Boolean getIsCompany();
    Boolean getIsThirdParty();
    Double getReOrderQty();
    Boolean getIsImport();
    Integer getLeadTimeDay();
    Boolean getAllowMultiUom();
    Integer getPackSizeId();
    Double getPackSize();
    Integer getMotherItemId();
}
