package com.mst.models.dto;

import com.fasterxml.jackson.annotation.JsonProperty;

import java.util.ArrayList;
import java.util.List;

/**
 * What the two dialogs opened by the Define Asset dialog's "+" buttons post
 * (countx_store_define_lookups.js, StoreDefine.openAssetCategory / openFixedAssetItem).
 * Field names are exactly the JSON keys the JS sends. Organization, company, branch, users and
 * dates are filled by the service from the session; read-only boxes (Asset Code) are not posted.
 */
public class StoreDefineAssetsExtraDto {

    /** frmItemCatagoryStore (FormTypeId 3) Insert():512 — the controls it reads, by control. */
    public static class AssetCategory {
        @JsonProperty("Id") public Integer Id = 0;                                        // RecId (0 = Save)
        @JsonProperty("CategoryCode") public String CategoryCode;                          // txtItemCategoryCode
        @JsonProperty("CategoryDescription") public String CategoryDescription;            // txtItemCategoryDescription
        @JsonProperty("SerialFrom") public String SerialFrom;                              // txtItemCategorySerialFrom (text)
        @JsonProperty("SerialTo") public String SerialTo;                                  // txtItemCategorySerialTo (text)
        @JsonProperty("RevenueAccountId") public Integer RevenueAccountId = 0;             // cmbItemCategoryRevenueAccount ("Accumulated A/c")
        @JsonProperty("InventoryAccountId") public Integer InventoryAccountId = 0;         // cmbItemCategoryInventoryAccount ("Assets GL A/c")
        @JsonProperty("CGSAccountId") public Integer CGSAccountId = 0;                     // cmbItemCategoryCGSAccount ("Depreciation A/c")
        @JsonProperty("ExpenseMaintenanceAccountId") public Integer ExpenseMaintenanceAccountId = 0;
        @JsonProperty("ParentCategoryId") public Integer ParentCategoryId = 0;             // CmbItemParentCategory
        @JsonProperty("ClassGroupId") public Integer ClassGroupId = 0;                     // CmbItemClassGroup
        @JsonProperty("DepreciationMethodScheduleId") public Integer DepreciationMethodScheduleId = 0;
        @JsonProperty("UseFullLifeMonths") public String UseFullLifeMonths;                // txtUseFullLifeMonth (text)
        @JsonProperty("DepreciationRate") public String DepreciationRate;                  // txtDepreciationRatePercent (text)
        @JsonProperty("CategoryStatus") public Boolean CategoryStatus = Boolean.FALSE;     // chkstatus ("Is Active")
        @JsonProperty("CheckedAttributeIds") public List<Long> CheckedAttributeIds = new ArrayList<>();  // grdAttributes checked rows
        @JsonProperty("host") public String host;
    }

    /** frmDefineAssets Insert():580 — the visible controls it reads. */
    public static class FixedAssetItem {
        @JsonProperty("Id") public Integer Id = 0;                                        // RecId (0 = Save)
        @JsonProperty("ItemCategoryId") public Integer ItemCategoryId = 0;                 // cmbItemCategory
        @JsonProperty("ItemTypeId") public Integer ItemTypeId = 0;                         // cmbItemType
        @JsonProperty("ItemName") public String ItemName;                                  // txtItemName
        @JsonProperty("BaseUnitId") public Integer BaseUnitId = 0;                         // cmbBaseUnit
        @JsonProperty("BarcodeNo") public String BarcodeNo;                                // txtBarcodeNo
        @JsonProperty("ItemClassId") public Integer ItemClassId = 0;                       // cmbAssetsClass
        @JsonProperty("CapitalWipAcId") public Integer CapitalWipAcId = 0;                 // CmbAssetGL
        @JsonProperty("accumulatedDepreciationAcId") public Integer accumulatedDepreciationAcId = 0;   // cmbAccumolativeACC
        @JsonProperty("DepreciationExpenseAcId") public Integer DepreciationExpenseAcId = 0;           // cmbDepriGL
        @JsonProperty("ExpenseMaintenanceAccountId") public Integer ExpenseMaintenanceAccountId = 0;   // CmbExpenseMaintenanceAccount
        @JsonProperty("LeadTimeDay") public String LeadTimeDay;                            // txtLeadTimeDay (text)
        @JsonProperty("ItemStatus") public Boolean ItemStatus = Boolean.TRUE;              // chkstatus ("Status")
        @JsonProperty("Local") public Boolean Local = Boolean.TRUE;                        // rdLocal.Checked
        @JsonProperty("Allocations") public List<Allocation> Allocations = new ArrayList<>();          // grdAllocation rows
        @JsonProperty("host") public String host;
    }

    /** One grdAllocation row: hidden Id (company) + the "Value" checkbox. */
    public static class Allocation {
        @JsonProperty("Id") public Integer Id = 0;
        @JsonProperty("Value") public Boolean Value = Boolean.FALSE;
    }
}
