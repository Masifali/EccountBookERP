package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ItemStoreDto;
import com.mst.repositories.ItemPmRepository;
import com.mst.repositories.ItemPmWriter;
import com.mst.repositories.ItemStoreRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Isolation;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Objects;
import java.util.Set;

/**
 * Store Management (ModuleId 24) - ScreenDefinition 329 "Item Store", ScreenName (base.Name)
 * {@code AddItemStore}, desktop form {@code Architecture.WinApp.StoreManagement.AddItemStore}
 * (AddItemStore.cs, 3,869 lines). A master screen: no DocumentTypeId (none - item definition), no
 * voucher, no stock posting. Page /store/item-store, API /api/store/item-store.
 *
 * <p>BLL/DAL used: BLL 0583 Architecture.BLL.Inventory.Item (Save :15, GetByID :49, FormHistory
 * :468, GenerateCode :581, GetGLAccountbyItemCategoryId :619); DAL 0436
 * Architecture.DAL.Inventory.Item.SetData (:23-606) incl. SaveCoa (:608-723); BLL 0584
 * ItemCategory.Getall (:96); BLL 0593 ItemType.Getall (:92); UOM.Getall, TaxesTypes.Getall,
 * CommonServices.CoaAllocationGetForComboServiceBind / CompanyServiceBind / StaticColumnsService /
 * GetERPFeatureById, GlobalServicesMethods.GetWarehouseRacks (usp_getRackswithWarehouse).
 *
 * <p>Reuse (read-only) of the screen-496 port, because AddItemStore.cs and AddItemPM.cs are the
 * same code there: {@link ItemPmRepository} for UOM / COA / taxes / racks / pack sizes / companies
 * / code generation / GL by category / configuration / features / images, and {@link ItemPmWriter}
 * for Item.Save -> DAL SetData (the DAL is one method for both forms; only the Item property bag
 * and the AutoCoaInsertIsOn switch differ, and both are built here).
 *
 * <p>What differs from AddItemPM.cs (diffed line by line) and is implemented here:
 * category list @Ids '8' (not '7'); type list @Type 17 / '8' (not 14 / '7'); no Master Item, no
 * Empty Bag Weight, no Master-Item update button, no HistoryComboBind (the history combos are
 * bound from the SAME tables as the entry combos); the switch is config
 * "AutoCoaDefineByItemNameOnInsertStore" (:318); FormValidationForAddItem checks Rack AFTER
 * Min/Max (:732-737, PM checks it before party processing); refresh() does not clear the Rack (:1160);
 * history sends @NoOfRecords and parent 8 and has no Master Item filter (:1254); the history grid
 * is not grouped and has no editable columns (:1312).
 *
 * <p>DESKTOP BEHAVIOUR REPRODUCED (not corrected):
 * <ol>
 *   <li>Update never reads txtReOrderQty, rdLocal/rdImport or cmbTaxType (:928-956): ReOrderQty 0,
 *       IsImport false, TaxTypeId 0 go to Sp_Item_Update. EntryUser is also 0 on update.</li>
 *   <li>IsImport = rdLocal.Checked (:815) - "Local" stores IsImport = true.</li>
 *   <li>Opening an item (grdhistory_DoubleClick :1010) does not restore ReOrderQty, Local/Import or
 *       Tax Type - they keep whatever they showed before.</li>
 *   <li>cmbItemType_Leave (:1690) regenerates the Item Code with no mode check, so changing the
 *       Item Type of an opened item replaces its code.</li>
 *   <li>The toolbar Refresh (toolStripButton1_Click :1220) reloads the auto-COA switch from
 *       "AutoCoaDefineByItemNameOnInsertPm" - the PM switch - for the rest of the session.</li>
 *   <li>Rack: RackNameFillFromGlobal (:445) inserts and activates a "-- Select --" row (Id 0), so
 *       "Rack Name Field is Required" only fires when the combo has no active row; saving with
 *       "-- Select --" stores RackId 0.</li>
 *   <li>LeadTimeDay is Conversion.ToInt(text): an opened value shown as "1,000" (:1044 "#,##0") is
 *       not an Int32 literal and goes back as 0 on update.</li>
 *   <li>BranchesId is never set on the Item, so the COA allocation, reorder schedule and chart of
 *       account rows the DAL writes get BranchId 0.</li>
 *   <li>Pack Size id: Rows[0].Activate() only at load (:670); New/refresh do not reset it.</li>
 *   <li>Save and Update messages only when Item.Save returns &gt;= 1; the form is reset after any
 *       save that did not throw.</li>
 * </ol>
 *
 * <p>DEVIATIONS (web-only):
 * <ol>
 *   <li>Rights come from StoreScreenRights("AddItemStore") on every call; Save/Update are refused
 *       server-side without the right (the desktop only disables the buttons).</li>
 *   <li>Every combo value is checked server-side against the list the desktop binds (the desktop
 *       can only pick listed rows); the opened item must belong to the user's company and to
 *       parent category 8; Item Category cannot change on update (desktop sets it ReadOnly :1034).</li>
 *   <li>Images: a kept image keeps its stored ImagePath. The desktop re-sends picbox.Tag, which
 *       it set to the FileName (:1058/:1069), as ImagePath - after any desktop update the image row
 *       points at the original file name instead of the stored GUID name; and a new picture
 *       browsed into a slot that already had one is not uploaded (DAL :380 keeps a non-empty
 *       ImagePath). The web uploads the new picture. Opening an item clears both picture slots
 *       first (the desktop leaves the previous item's picture in a slot the opened item has no
 *       image for, and would re-save it under this item). Removing a picture does not physically
 *       delete the stored file (DAL :544 ItemImagesDeletelist).</li>
 *   <li>ItemCodingEnable and the auto-COA switches are read per request, not once per form load.</li>
 *   <li>Item Code / Item Code New (read-only boxes) are not trusted from the request: on insert the
 *       server generates them for (category, type); on update the posted pair is kept only when it
 *       equals the stored codes or the pair generated now for (category, type), else the stored
 *       codes are kept.</li>
 *   <li>With no racks at all the desktop binds nothing (no "-- Select --" row, no active row), so
 *       "Rack Name Field is Required"; the server refuses RackId 0 in that case too.</li>
 *   <li>FinancialYearId for the auto-created chart of account is the session's financial year
 *       (desktop: clsGlobalVariables.ActiveYr).</li>
 * </ol>
 *
 * <p>NOT PORTED: attachments (btnattachment, AttachmentsList/DeleteAttachmentsList, the
 * NoOfAttachments link), saved grid layouts (ctrlGrdBar1), the sub-form launchers (Item Category,
 * Item Type, Barcode Print, Item Uom, Item Uom Schedule, Attribute Allocation, Rack Allocation To
 * Item), VPS/FTP image transport (the shared DesktopAttachmentStore decides where files live).
 * No print exists on this form.
 */
@Service
public class ItemStoreService {

    public static final int SCREEN_ID = 329;

    private final ItemStoreRepository repo;
    private final ItemPmRepository pm;
    private final ItemPmWriter writer;
    private final DesktopAttachmentStore store;
    private final CurrentUserContext ctx;
    private final StoreScreenRights rights;

    public ItemStoreService(ItemStoreRepository repo, ItemPmRepository pm, ItemPmWriter writer,
                            DesktopAttachmentStore store, CurrentUserContext ctx, StoreScreenRights rights) {
        this.repo = repo; this.pm = pm; this.writer = writer; this.store = store; this.ctx = ctx; this.rights = rights;
    }

    // ------------------------------------------------------------------ InvDefrmAddItem_Load :307

    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("categories", repo.categories(u));          // ItemCatagoryHistoryFill :347 (entry + history)
        r.put("types", repo.types(u));                    // ItemTypeFill :414 (entry + history)
        r.put("racks", pm.racks(u));                      // RackNameFillFromGlobal :445
        r.put("companies", pm.companies(u));              // CompaniesBindInGrid :615
        r.put("accounts", pm.accounts(u));                // PurchaseGLAcFill :476
        r.put("taxes", pm.taxes(u));                      // CmbTaxTypeFill :497 (Id != 1)
        r.put("units", pm.units(u));                      // BaseUnitFill :382
        r.put("packSizes", pm.packSizes());               // PackSizeIdForItem :660
        Map<String, Boolean> features = new LinkedHashMap<>();
        features.put("partyProcessing", pm.feature(u, 8));      // :334-336
        features.put("attributesForItem", pm.feature(u, 13));   // :337-338
        r.put("features", features);
        r.put("rights", rights.of(ItemStoreRepository.SCREEN)); // :314-316
        return r;
    }

    /**
     * cmbItemCategory_Leave :536 (code + GL accounts, only while Save is the active button - the
     * page decides that) and cmbItemType_Leave :1690 (code only, always).
     */
    public Map<String, Object> defaults(int category, int type, boolean withAccounts) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        String[] code = generated(u, category, type);                           // GenerateItemCode :579
        if (code != null) {
            out.put("ItemCode", code[0]);
            out.put("ItemCodeNew", code[1]);
        }
        if (withAccounts) {
            Map<String, Object> gl = pm.glAccounts(u, category);                // SetGLAccounts :596
            if (!gl.isEmpty()) {
                out.put("PurchaseGLAC", ItemPmRepository.col(gl, "InventoryAccountId"));
                out.put("SaleGLAC", ItemPmRepository.col(gl, "RevenueAccountId"));
                out.put("COGSGLAC", ItemPmRepository.col(gl, "CGSAccountId"));
            }
        }
        return out;
    }

    /** grdfrmfill :1254 - the 16-column table the form builds (:1276-1295). */
    public List<Map<String, Object>> history(int noOfRecords, int category, int type) {
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = rights.has(ItemStoreRepository.SCREEN, "viewAll");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.history(u, viewAll, Math.max(0, noOfRecords), category, type)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ItemPmRepository.intOf(ItemPmRepository.col(r, "Id")));
            m.put("ItemCode", str(ItemPmRepository.col(r, "ItemCode")));
            m.put("ItemCodeNew", str(ItemPmRepository.col(r, "ItemCodeNew")));
            m.put("ItemName", str(ItemPmRepository.col(r, "ItemName")));
            m.put("TypeDescription", str(ItemPmRepository.col(r, "TypeDescription")));
            m.put("CategoryDescription", str(ItemPmRepository.col(r, "CategoryDescription")));
            m.put("ParentCategory", str(ItemPmRepository.col(r, "InvParentCateDescription")));
            m.put("ClassGroupName", str(ItemPmRepository.col(r, "ClassGroupName")));
            m.put("ItemStatus", boolText(ItemPmRepository.col(r, "ItemStatus")));
            m.put("StockAc", str(ItemPmRepository.col(r, "GlStockAccountTitle")));
            m.put("SaleAc", str(ItemPmRepository.col(r, "GlSaleAccountTitle")));
            m.put("CgsAc", str(ItemPmRepository.col(r, "GlCgsAccountTitle")));
            m.put("EntryDate", ItemPmRepository.col(r, "EntryDate"));
            m.put("EntryUserName", str(ItemPmRepository.col(r, "EntryUserName")));
            m.put("NoOfAttachments", ItemPmRepository.intOf(ItemPmRepository.col(r, "NoOfAttachments")));
            m.put("LeadTimeDay", ItemPmRepository.intOf(ItemPmRepository.col(r, "LeadTimeDay")));
            out.add(m);
        }
        return out;
    }

    /** grdhistory_DoubleClick :1010. */
    public Map<String, Object> record(int id) {
        return repo.details(ctx.requireAccountingUser(), id);
    }

    public DesktopInventoryItemFileService.Download image(int item, int image) {
        UserAccount u = ctx.requireAccountingUser();
        repo.details(u, item);
        Map<String, Object> row = pm.images(item).stream()
                .filter(r -> ItemPmRepository.intOf(r.get("Id")) == image).findFirst()
                .orElseThrow(() -> new ResponseStatusException(HttpStatus.NOT_FOUND, "Image not found"));
        String path = basename(Objects.toString(row.get("ImagePath"), ""));
        byte[] bytes = store.read(u, path);
        return new DesktopInventoryItemFileService.Download(basename(Objects.toString(row.get("FileName"), path)), bytes,
                DesktopInventoryItemFileService.imageType(bytes));
    }

    // ------------------------------------------------------------------ BtnSave_Click :759 / btnupdate_Click :896

    @Transactional(isolation = Isolation.SERIALIZABLE)
    public Map<String, Object> save(ItemStoreDto r) {
        if (r == null || r.Id == null || r.Id < 0) throw new IllegalArgumentException("Invalid item");
        boolean insert = r.Id == 0;
        UserAccount u = ctx.requireAccountingUser();
        if (insert && !rights.has(ItemStoreRepository.SCREEN, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (!insert && !rights.has(ItemStoreRepository.SCREEN, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");

        Map<String, Object> old = null;
        if (!insert) {
            @SuppressWarnings("unchecked")
            Map<String, Object> rec = (Map<String, Object>) repo.details(u, r.Id).get("item");
            old = rec;
        }
        boolean partyProcessing = pm.feature(u, 8);

        // ------------------------------------------------ FormValidationForAddItem :695 (this order)
        if (r.ItemName == null || r.ItemName.trim().isEmpty()) throw new IllegalArgumentException("Item Name Field is Required");
        Map<String, Object> unit = pick(pm.units(u), nz(r.BaseUnitId), "Base Unit Field is Required");
        pick(repo.categories(u), nz(r.ItemCategoryId), "Item Category Field is Required");
        pick(repo.types(u), nz(r.ItemTypeId), "Item Type Field is Required");
        if (partyProcessing && !r.IsCompany && !r.IsThirdParty) {
            throw new IllegalArgumentException("Please Check of the Check box ('Is Company' Or 'Is third Party') ");
        }
        if (dbl(r.MinStockLevel) > dbl(r.MaxStockLevel)) {
            throw new IllegalArgumentException("Min stock level cannot be greater than max stock level.");
        }
        // :732-737 - the "-- Select --" row (Id 0) is an active row; only "no row" is refused.
        if (r.RackId == null || r.RackId < 0) throw new IllegalArgumentException("Rack Name Field is Required");
        List<Map<String, Object>> racks = pm.racks(u);
        // BindAndRetainSelection with an empty source binds nothing (no "-- Select --" row, no active row).
        if (racks.isEmpty()) throw new IllegalArgumentException("Rack Name Field is Required");
        if (r.RackId > 0) pick(racks, r.RackId, "Rack Name Field is Required");
        List<Map<String, Object>> accounts = pm.accounts(u);
        pick(accounts, nz(r.PurchaseGLAC), "Stock A/c Field is Required");
        pick(accounts, nz(r.SaleGLAC), "Sale A/c Field is Required");
        pick(accounts, nz(r.COGSGLAC), "CGS A/c Filed is Required");

        // Selections the desktop does not validate, but can only pick from its lists.
        if (nz(r.PackSizeId) > 0) pick(pm.packSizes(), r.PackSizeId, "Select a Pack Size from the list");
        if (insert && r.ApplyGST && nz(r.TaxTypeId) > 0) pick(pm.taxes(u), r.TaxTypeId, "Select a Tax Type from the list");
        if (!insert && ItemPmRepository.intOf(ItemPmRepository.col(old, "ItemCategoryId")) != nz(r.ItemCategoryId)) {
            throw new IllegalArgumentException("Item Category cannot change after the item is saved");   // ReadOnly :1034
        }
        length(r.ItemName, 100, "Item Name");
        length(r.BarcodeNo, 50, "Barcode No");

        // ------------------------------------------------ the Item property bag
        double equivalent = dbl(ItemPmRepository.col(unit, "Equivalent"));   // SelectedRow.Cells[2] :790 / :935
        Map<String, Object> item = new LinkedHashMap<>();
        item.put("Id", insert ? 0 : r.Id);
        // txtItemCode / txtItemCodeNew are ReadOnly (:785-786 / :930-931) and only ever filled by
        // GenerateItemCode (:579) or by opening the item (:1028-1029); never taken from the request as is.
        String[] code = itemCodes(u, insert, old, nz(r.ItemCategoryId), nz(r.ItemTypeId), r.ItemCode, r.ItemCodeNew);
        item.put("ItemCode", code[0]);
        item.put("ItemCodeNew", code[1]);
        item.put("BarcodeNo", trim(r.BarcodeNo));
        item.put("ItemName", r.ItemName.trim());
        item.put("BaseUnitId", nz(r.BaseUnitId));
        item.put("Equivalent", equivalent);
        item.put("ItemCategoryId", nz(r.ItemCategoryId));
        item.put("ItemTypeId", nz(r.ItemTypeId));
        item.put("ItemClassId", 1);                                                 // :793 / :938
        item.put("MinStockLevel", dbl(r.MinStockLevel));
        item.put("MaxStockLevel", dbl(r.MaxStockLevel));
        item.put("ReorderLevel", dbl(r.ReorderLevel));
        item.put("LeadTimeDay", desktopInt(r.LeadTimeDay));
        item.put("PackSize", dbl(r.PackSize));
        item.put("PackSizeId", nz(r.PackSizeId));
        item.put("AllowMultiUom", r.AllowMultiUom);
        item.put("RackId", r.RackId);
        item.put("PurchaseGLAC", nz(r.PurchaseGLAC));
        item.put("SaleGLAC", nz(r.SaleGLAC));
        item.put("COGSGLAC", nz(r.COGSGLAC));
        item.put("OrganizationId", u.getOrganizationId());
        item.put("CompanyId", u.getCompanyId());
        if (partyProcessing) { item.put("IsThirdParty", r.IsThirdParty); item.put("IsCompany", r.IsCompany); }
        else { item.put("IsThirdParty", false); item.put("IsCompany", true); }         // :816-825 / :957-966
        if (insert) {
            item.put("ItemStatus", true);                                           // :794
            item.put("ReOrderQty", dbl(r.ReOrderQty));                              // :798 (Save only)
            item.put("ItemGroupId", 0);                                             // :809
            item.put("EntryUser", u.getId());                                       // :812
            item.put("IsImport", r.LocalSelected);                                  // :815 IsImport = rdLocal.Checked
            if (r.ApplyGST) { item.put("ApplyGST", true); item.put("TaxTypeId", nz(r.TaxTypeId)); }   // :826-830
        } else {
            item.put("ItemStatus", r.ItemStatus);                                   // :939 chkstatus
            item.put("ApplyGST", r.ApplyGST);                                       // :951 - TaxTypeId not read
            item.put("ModifyUser", u.getId());                                      // :954
        }

        // ------------------------------------------------ Brand Images (:776-783 / :911-927, :967-968)
        List<Map<String, Object>> images = images(u, r, insert);
        item.put("Pic1", "");
        item.put("Pic2", "");
        for (Map<String, Object> img : images) {
            item.put("Pic" + ItemPmRepository.intOf(img.get("SortNo")), img.get("FileName"));
        }

        // ------------------------------------------------ grdAllocation :832-867 (Save only)
        List<ItemPmWriter.Allocation> allocations = new ArrayList<>();
        if (insert) {
            List<Map<String, Object>> companies = pm.companies(u);
            if (companies.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
            Set<Integer> ticked = new HashSet<>(r.companies == null ? List.of() : r.companies);
            for (Integer id : ticked) {
                if (id == null || companies.stream().noneMatch(c -> ItemPmRepository.intOf(c.get("Id")) == id)) {
                    throw new IllegalArgumentException("Select a Location from this organization");
                }
            }
            int branch = u.getBranchesId() == null ? 0 : u.getBranchesId();
            int falseCount = 0;
            for (Map<String, Object> c : companies) {
                int id = ItemPmRepository.intOf(c.get("Id"));
                if (companies.size() == 1 || ticked.contains(id)) {
                    allocations.add(new ItemPmWriter.Allocation(id, true, branch, u.getOrganizationId()));
                } else {
                    falseCount++;
                }
            }
            if (companies.size() > 1 && companies.size() == falseCount) {
                throw new IllegalArgumentException("Please select Location first");
            }
        }

        ItemPmWriter.Save s = new ItemPmWriter.Save();
        s.insert = insert;
        s.id = r.Id;
        s.item = item;
        s.itemCategoryId = nz(r.ItemCategoryId);
        s.itemTypeId = nz(r.ItemTypeId);
        s.itemName = r.ItemName.trim();
        s.itemCode = code[0];
        s.equivalent = equivalent;
        s.applyGst = Boolean.TRUE.equals(item.get("ApplyGST"));
        s.taxTypeId = ItemPmRepository.intOf(item.get("TaxTypeId"));
        s.reorderLevel = dbl(r.ReorderLevel);
        s.minStockLevel = dbl(r.MinStockLevel);
        s.maxStockLevel = dbl(r.MaxStockLevel);
        s.allocations = allocations;
        s.images = images;
        // :318 "AutoCoaDefineByItemNameOnInsertStore"; after the toolbar Refresh :1220 the PM key.
        s.autoCoaInsertIsOn = pm.configuration(u, r.RefreshedLookups
                ? "AutoCoaDefineByItemNameOnInsertPm" : "AutoCoaDefineByItemNameOnInsertStore");
        s.financialYearId = ctx.currentFinancialYearId();

        int id;
        try {
            id = writer.save(u, s);
        } catch (IllegalStateException e) {
            throw new IllegalArgumentException(e.getMessage());
        } catch (org.springframework.dao.DataAccessException e) {
            Throwable c = e.getMostSpecificCause();
            throw new IllegalArgumentException(c == null ? e.getMessage() : c.getMessage());
        }

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("message", id >= 1 ? (insert ? "Save Successfully" : "Update Successfully") : "");   // :884-886 / :997-999
        return out;
    }

    /**
     * ItemImageslist. New file -> stored, FileName = its name, ImagePath = the stored name. Kept
     * image -> its FileName and stored ImagePath (see DEVIATIONS 3). SortNo 1 is the profile image.
     */
    private List<Map<String, Object>> images(UserAccount u, ItemStoreDto r, boolean insert) {
        List<Map<String, Object>> existing = insert ? List.of() : pm.images(r.Id);
        List<Map<String, Object>> out = new ArrayList<>();
        if (r.images == null) return out;
        Set<Integer> slots = new HashSet<>();
        for (ItemStoreDto.Image img : r.images) {
            if (img == null || (img.sortNo != 1 && img.sortNo != 2) || !slots.add(img.sortNo)) {
                throw new IllegalArgumentException("Select one image for each slot");
            }
            String fileName, path;
            if (img.upload != null) {
                byte[] bytes = DesktopInventoryItemFileService.decode(img.upload);
                DesktopInventoryItemFileService.imageType(bytes);
                fileName = img.upload.name;
                path = store.store(u, fileName, bytes);
            } else {
                Map<String, Object> keep = existing.stream()
                        .filter(e -> ItemPmRepository.intOf(e.get("Id")) == img.id).findFirst()
                        .orElseThrow(() -> new IllegalArgumentException("Image does not belong to this item"));
                fileName = Objects.toString(keep.get("FileName"), "");
                path = Objects.toString(keep.get("ImagePath"), "");
            }
            Map<String, Object> row = new LinkedHashMap<>();
            row.put("ImagePath", path);
            row.put("FileName", fileName);
            row.put("SortNo", img.sortNo);
            row.put("IsProfileImage", img.sortNo == 1);
            row.put("EffetedDate", new Timestamp(System.currentTimeMillis()));
            row.put("IsActive", false);
            out.add(row);
        }
        return out;
    }

    // ------------------------------------------------------------------ item code

    /** GenerateItemCode :579-594 - {ItemCode, ItemCodeNew}, or null when the procedure returns no row. */
    private String[] generated(UserAccount u, int category, int type) {
        Map<String, Object> code = pm.generateCode(u, category, type);
        if (code.isEmpty()) return null;
        String itemCode = Objects.toString(ItemPmRepository.col(code, "ItemCode"), "");
        String newCode = Objects.toString(ItemPmRepository.col(code, "ItemCodeNew"), "");
        return new String[] { itemCode, pm.configuration(u, "ItemCodingEnable")          // :586
                ? newCode + "-" + desktopInt(itemCode) : newCode };
    }

    /**
     * The codes the read-only boxes can legitimately hold. Insert: the pair generated now for
     * (category, type). Update: the posted pair when it equals the stored codes (opened item) or
     * the pair generated now for (category, type) - cmbItemType_Leave :1690 regenerates on an
     * opened item too; anything else keeps the stored codes.
     */
    private String[] itemCodes(UserAccount u, boolean insert, Map<String, Object> old, int category, int type,
                               String postedCode, String postedNew) {
        String[] gen = generated(u, category, type);
        if (insert) return gen == null ? new String[] { "", "" } : gen;
        String[] stored = { str(ItemPmRepository.col(old, "ItemCode")).trim(), str(ItemPmRepository.col(old, "ItemCodeNew")).trim() };
        String[] posted = { trim(postedCode), trim(postedNew) };
        if (gen != null && posted[0].equals(gen[0].trim()) && posted[1].equals(gen[1].trim())) return new String[] { posted[0], posted[1] };
        return stored;
    }

    // ------------------------------------------------------------------ helpers

    private static Map<String, Object> pick(List<Map<String, Object>> rows, int id, String message) {
        if (id <= 0) throw new IllegalArgumentException(message);
        return rows.stream().filter(x -> ItemPmRepository.intOf(ItemPmRepository.col(x, "Id")) == id).findFirst()
                .orElseThrow(() -> new IllegalArgumentException(message));
    }

    private static int nz(Integer v) { return v == null ? 0 : v; }

    /** Conversion.ToDouble(text): Convert.ToDouble accepts thousands separators; anything else is 0. */
    static double dbl(Object v) {
        if (v == null) return 0d;
        if (v instanceof BigDecimal b) return b.doubleValue();
        if (v instanceof Number n) return n.doubleValue();
        String s = v.toString().replace(",", "").trim();
        if (s.isEmpty()) return 0d;
        try {
            double d = Double.parseDouble(s);
            return Double.isInfinite(d) || Double.isNaN(d) ? 0d : d;
        } catch (NumberFormatException e) { return 0d; }
    }

    /** Conversion.ToInt(text) = Convert.ToInt32(string): an Int32 literal or 0 ("1,000" -> 0). */
    static int desktopInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number n) return n.intValue();
        String s = v.toString().trim();
        if (!s.matches("[+-]?\\d+")) return 0;
        try { return Integer.parseInt(s); } catch (NumberFormatException e) { return 0; }
    }

    private static String trim(String s) { return s == null ? "" : s.trim(); }

    private static String str(Object o) { return o == null ? "" : o.toString(); }

    /** Conversion.ToString of a bit column: "True" / "False", as the grid shows it. */
    private static String boolText(Object o) {
        if (o == null) return "";
        if (o instanceof Boolean b) return b ? "True" : "False";
        return o.toString();
    }

    private static void length(String v, int max, String name) {
        if (v != null && v.trim().length() > max) throw new IllegalArgumentException(name + " cannot exceed " + max + " characters");
    }

    private static String basename(String name) {
        String n = name.replace('\\', '/');
        String r = n.substring(n.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(r);
        return r;
    }
}
