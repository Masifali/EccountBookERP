package com.mst.models.dto;

import com.fasterxml.jackson.annotation.JsonProperty;

/**
 * Screen 336 "Item Category Store" (InvDeffrmItemCatagory) — what the page posts on Save/Update.
 *
 * Text boxes are posted as the raw control text (the desktop validates the text, then converts
 * with Conversion.ToInt). Combos post their selected value (0 = nothing selected). Organization,
 * company and users are filled by the service from the session.
 */
public class ItemCategoryStoreDto {

    /** RecId: 0 = Save (btnsave_Click sets RecId = 0), otherwise the opened category (Update). */
    @JsonProperty("id") public Integer id = 0;
    @JsonProperty("categoryCode") public String categoryCode;            // txtItemCategoryCode.Text
    @JsonProperty("categoryDescription") public String categoryDescription; // txtItemCategoryDescription.Text
    @JsonProperty("serialFrom") public String serialFrom;                // txtItemCategorySerialFrom.Text
    @JsonProperty("serialTo") public String serialTo;                    // txtItemCategorySerialTo.Text
    @JsonProperty("revenueAccountId") public Integer revenueAccountId = 0;      // cmbItemCategoryRevenueAccount
    @JsonProperty("inventoryAccountId") public Integer inventoryAccountId = 0;  // cmbItemCategoryInventoryAccount
    @JsonProperty("cgsAccountId") public Integer cgsAccountId = 0;              // cmbItemCategoryCGSAccount
    @JsonProperty("parentCategoryId") public Integer parentCategoryId = 0;      // CmbItemParentCategory
    @JsonProperty("classGroupId") public Integer classGroupId = 0;              // CmbItemClassGroup
    @JsonProperty("productionStageId") public Integer productionStageId = 0;    // CmbProductionStag
    @JsonProperty("varietyNatureId") public Integer varietyNatureId = 0;        // CmbVarityNature
    @JsonProperty("categoryStatus") public Boolean categoryStatus = Boolean.TRUE; // chkstatus (designer: Checked)
}
