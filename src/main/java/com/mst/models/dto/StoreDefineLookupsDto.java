package com.mst.models.dto;

import com.fasterxml.jackson.annotation.JsonProperty;

/**
 * What the two desktop lookup dialogs post (Architecture.WinApp.Lookups):
 * {@code Define_Department} and {@code frmLookUpDefineAsset}. Field names are exactly the JSON keys
 * countx_store_define_lookups.js sends. Everything the server owns — organization, company, branch,
 * users, entry / approval / purchase / expiry dates — is filled by the service from the session.
 */
public class StoreDefineLookupsDto {

    /** Define_Department.btnsave_Click:147 / btnUpdate_Click:111 — txtDepartmentName + RecId. */
    public static class Department {
        @JsonProperty("Id") public Integer Id = 0;                         // RecId (update only)
        @JsonProperty("DepartmentName") public String DepartmentName;       // txtDepartmentName.Text (untrimmed)
        @JsonProperty("host") public String host;                           // ScreenName of the page that opened the dialog
    }

    /** frmLookUpDefineAsset.Insert():255 — the eight inputs of panel5 + RecId. */
    public static class Asset {
        @JsonProperty("Id") public Integer Id = 0;                          // RecId (update only)
        @JsonProperty("FixedAssetsCategoryId") public Integer FixedAssetsCategoryId = 0;  // CmbAssetCategory.Value
        @JsonProperty("ItemId") public Integer ItemId = 0;                  // CmbAssetItem.Value
        @JsonProperty("Brand") public String Brand;                         // txtBrandName
        @JsonProperty("AssetName") public String AssetName;                 // txtAssetName
        @JsonProperty("ModelDesc") public String ModelDesc;                 // txtModelDesc
        @JsonProperty("AssetCondition") public String AssetCondition;       // txtAssetCondition
        @JsonProperty("AssetsDepartmentId") public Integer AssetsDepartmentId = 0;         // cmbDepartment.Value
        @JsonProperty("AssetLocationId") public String AssetLocationId;     // txtAssetLocation
        @JsonProperty("host") public String host;
    }
}
