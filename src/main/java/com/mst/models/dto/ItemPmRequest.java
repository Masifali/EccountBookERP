package com.mst.models.dto;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

/**
 * Screen 496 "Item PM" - {@code Architecture.WinApp.StoreManagement.AddItemPM}.
 *
 * Exactly the controls BtnSave_Click (:851) and btnupdate_Click (:989) read. Everything the form
 * hard-codes (ItemClassId = 1, ItemGroupId = 0, ItemStatus = true on insert, and the
 * party-processing fallback) is applied on the server, never taken from the request.
 */
public class ItemPmRequest {
    public int Id;

    /** txtItemCode - hidden, read-only; filled by GenerateItemCode. */
    public String ItemCode = "";
    /** txtItemCodeNew - read-only, the visible "Item Code". */
    public String ItemCodeNew = "";
    public String BarcodeNo = "";
    public String ItemName = "";

    public int BaseUnitId;
    public int ItemCategoryId;
    public int ItemTypeId;
    public int MasterItemId;
    public int RackId;

    public BigDecimal EmptyBagWeight = BigDecimal.ZERO;
    public BigDecimal MinStockLevel = BigDecimal.ZERO;
    public BigDecimal MaxStockLevel = BigDecimal.ZERO;
    public BigDecimal ReorderLevel = BigDecimal.ZERO;
    /** Sent on Save only. btnupdate_Click does not read txtReOrderQty. */
    public BigDecimal ReOrderQty = BigDecimal.ZERO;
    /** txtLeadTimeDay - digits only (OnlytextNumberFunction), Conversion.ToInt. */
    public int LeadTimeDay;
    public BigDecimal PackSize = BigDecimal.ZERO;
    public int PackSizeId;

    /** chkAllowMultiUom - Visible=false on the desktop, so always its designer default, false. */
    public boolean AllowMultiUom;
    /** chkstatus - read on Update only; Save forces true (:889). */
    public boolean ItemStatus = true;
    /** rdLocal.Checked - the desktop assigns IsImport = rdLocal.Checked (:909), Save only. */
    public boolean LocalSelected = true;

    public boolean IsCompany = true;
    public boolean IsThirdParty = true;

    /** chbGST */
    public boolean ApplyGST;
    public int TaxTypeId;

    public int PurchaseGLAC;
    public int SaleGLAC;
    public int COGSGLAC;

    /** grdAllocation - the Value column, one entry per company row that is ticked. */
    public List<Integer> companies = new ArrayList<>();

    /** Brand Images: picture slot 1 and 2. */
    public List<Image> images = new ArrayList<>();

    /** Attachments (AT). */
    public List<InventoryPosItemRequest.Upload> files = new ArrayList<>();
    public List<Integer> removeAttachmentIds = new ArrayList<>();

    public static class Image {
        /** 1 or 2 - picbox1 / picbox2. */
        public int sortNo;
        /** Existing ItemImage row being kept, or 0. */
        public int id;
        /** A newly browsed file, or null when an existing image is kept. */
        public InventoryPosItemRequest.Upload upload;
    }

    /** btnMasterItemsUpdate_Click (:1855) - one row per history-grid row with a Master Item or EB weight. */
    public static class MasterItemRow {
        public int itemId;
        public int masterItemId;
        public BigDecimal emptyBagWeight = BigDecimal.ZERO;
    }
}
