package com.mst.models.dto;

import com.fasterxml.jackson.annotation.JsonProperty;

/**
 * Screen 337 "Item Type Store" (InvDeffrmItemType) — what the page posts on Save/Update.
 * Organization, company and users are filled by the service from the session.
 */
public class ItemTypeStoreDto {

    /** RecId: 0 = Save (btnsave_Click sets RecId = 0), otherwise the opened type (Update). */
    @JsonProperty("id") public Integer id = 0;
    @JsonProperty("typeCode") public String typeCode;               // txtItemTypeCode.Text
    @JsonProperty("typeDescription") public String typeDescription; // txtItemTypeDescription.Text
    @JsonProperty("type") public Integer type = 0;                  // cmbItemTypeType.Value
    @JsonProperty("parentCategoryId") public Integer parentCategoryId = 0; // CmbParentCategory.Value
    @JsonProperty("isMother") public Boolean isMother = Boolean.FALSE;     // ChkIsMother.Checked
}
