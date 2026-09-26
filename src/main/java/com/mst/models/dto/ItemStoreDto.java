package com.mst.models.dto;

import com.fasterxml.jackson.annotation.JsonProperty;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 329 "Item Store" ({@code AddItemStore.cs}) - what the page posts for BtnSave_Click (:759)
 * and btnupdate_Click (:896).
 *
 * Field names are the Item model property each control is copied into. Organization, company,
 * users, branch and financial year are never read from here - the service fills them from the
 * signed-in user. {@code RackId} is nullable: null means "the Rack combo has no active row"
 * (FormValidationForAddItem :731), 0 is the "-- Select --" row the desktop inserts and activates.
 */
public class ItemStoreDto {

    @JsonProperty("Id") public Integer Id = 0;                         // RecId
    @JsonProperty("ItemCode") public String ItemCode;                  // txtItemCode (hidden, read-only)
    @JsonProperty("ItemCodeNew") public String ItemCodeNew;            // txtItemCodeNew (read-only)
    @JsonProperty("BarcodeNo") public String BarcodeNo;
    @JsonProperty("ItemName") public String ItemName;
    @JsonProperty("BaseUnitId") public Integer BaseUnitId = 0;
    @JsonProperty("ItemCategoryId") public Integer ItemCategoryId = 0;
    @JsonProperty("ItemTypeId") public Integer ItemTypeId = 0;
    @JsonProperty("RackId") public Integer RackId;
    @JsonProperty("MinStockLevel") public String MinStockLevel;        // text boxes: Conversion.ToDouble(text)
    @JsonProperty("MaxStockLevel") public String MaxStockLevel;
    @JsonProperty("ReorderLevel") public String ReorderLevel;
    @JsonProperty("ReOrderQty") public String ReOrderQty;
    @JsonProperty("LeadTimeDay") public String LeadTimeDay;
    @JsonProperty("PackSize") public String PackSize;
    @JsonProperty("PackSizeId") public Integer PackSizeId = 0;
    @JsonProperty("AllowMultiUom") public boolean AllowMultiUom;
    @JsonProperty("ItemStatus") public boolean ItemStatus = true;      // chkstatus
    @JsonProperty("LocalSelected") public boolean LocalSelected = true; // rdLocal.Checked
    @JsonProperty("IsCompany") public boolean IsCompany = true;
    @JsonProperty("IsThirdParty") public boolean IsThirdParty = true;
    @JsonProperty("ApplyGST") public boolean ApplyGST;                 // chbGST
    @JsonProperty("TaxTypeId") public Integer TaxTypeId = 0;
    @JsonProperty("PurchaseGLAC") public Integer PurchaseGLAC = 0;     // Stock A/C
    @JsonProperty("SaleGLAC") public Integer SaleGLAC = 0;
    @JsonProperty("COGSGLAC") public Integer COGSGLAC = 0;
    /**
     * True once the toolbar "Refresh" (toolStripButton1_Click :1216) has run in this page session:
     * it reloads AutoCoaDefineByItemNameOnInsert from the PM switch, not the Store one.
     */
    @JsonProperty("RefreshedLookups") public boolean RefreshedLookups;

    /** Location ids ticked in grdAllocation (Save only). */
    @JsonProperty("companies") public List<Integer> companies = new ArrayList<>();
    /** Brand Images, slots 1 and 2. */
    @JsonProperty("images") public List<Image> images = new ArrayList<>();

    public static class Image {
        @JsonProperty("sortNo") public int sortNo;
        /** An ItemImage row of this item kept as it is (0 when a new file is uploaded). */
        @JsonProperty("id") public int id;
        @JsonProperty("upload") public InventoryPosItemRequest.Upload upload;
    }
}
