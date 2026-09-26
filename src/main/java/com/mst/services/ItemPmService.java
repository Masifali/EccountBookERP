package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ItemPmRequest;
import com.mst.repositories.InventoryOpeningRepository;
import com.mst.repositories.ItemPmRepository;
import com.mst.repositories.ItemPmWriter;
import com.mst.repositories.support.ProcExec;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.math.BigDecimal;
import java.sql.Timestamp;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Isolation;
import org.springframework.transaction.annotation.Transactional;

/**
 * Screen 496 "Item PM" (Packing Material module 54) - {@code AddItemPM.cs}.
 *
 * Rights: the desktop reads them with {@code CommonServices.SetRightsValueInRightsObject("AddItemPM")}
 * (:339) and enables Save / Update from them; here every endpoint re-checks against the real
 * CompanyRights + ScreenRights + tblUserRights chain for ScreenDefinition 496.
 *
 * Validation: {@code FormValidationForAddItem} (:787) in the desktop's order and with its own
 * strings, run on the server so a request that does not come from the page is refused the same
 * way. Save-time refusals from BtnSave_Click (:941-976) and the DAL follow.
 */
@Service
public class ItemPmService {

    public static final int SCREEN_ID = 496;

    private final ItemPmRepository repo;
    private final ItemPmWriter writer;
    private final InventoryOpeningRepository shared;
    private final DesktopAttachmentStore store;
    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public ItemPmService(ItemPmRepository repo, ItemPmWriter writer, InventoryOpeningRepository shared,
                         DesktopAttachmentStore store, JdbcTemplate jdbc,
                         CurrentUserContext context, DesktopReportRights rights) {
        this.repo = repo; this.writer = writer; this.shared = shared; this.store = store;
        this.jdbc = jdbc; this.context = context; this.rights = rights;
    }

    private UserAccount user(String action) {
        UserAccount u = context.requireAccountingUser();
        rights.require(u, SCREEN_ID, action);
        return u;
    }

    private boolean allowed(UserAccount u, String action) {
        try { rights.require(u, SCREEN_ID, action); return true; } catch (AccessDeniedException e) { return false; }
    }

    // ------------------------------------------------------------------ load

    /** InvDefrmAddItem_Load :332 - everything the form binds, plus rights, switches and features. */
    public Map<String, Object> lookups() {
        UserAccount u = user("View");
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("categories", repo.categories(u));
        r.put("types", repo.types(u));
        r.put("masterItems", repo.masterItems(u));
        r.put("companies", repo.companies(u));
        r.put("accounts", repo.accounts(u));
        r.put("racks", repo.racks(u));
        r.put("taxes", repo.taxes(u));
        r.put("units", repo.units(u));
        r.put("packSizes", repo.packSizes());
        r.put("history", repo.historyCombos(u));

        Map<String, Boolean> cfg = new LinkedHashMap<>();
        for (String k : List.of("AutoCoaDefineByItemNameOnInsertPm", "IsVpsAttachmentsServiceOn", "ItemCodingEnable")) {
            cfg.put(k, repo.configuration(u, k));
        }
        r.put("configuration", cfg);

        Map<String, Boolean> features = new LinkedHashMap<>();
        features.put("partyProcessing", repo.feature(u, 8));       // :361
        features.put("attributesForItem", repo.feature(u, 13));    // :364
        r.put("features", features);

        Map<String, Boolean> perms = new LinkedHashMap<>();
        for (String a : List.of("Save", "Update", "CanView AllRecord")) perms.put(a, allowed(u, a));
        r.put("permissions", perms);
        return r;
    }

    /**
     * cmbItemCategory_Leave :628 / cmbItemType_Leave :1913 - the code, and (category leave only)
     * the three GL accounts. {@code ItemCodingEnable} decides the ItemCodeNew format (:679).
     */
    public Map<String, Object> defaults(int category, int type, boolean withAccounts) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> code = repo.generateCode(u, category, type);
        if (!code.isEmpty()) {
            String itemCode = Objects.toString(ItemPmRepository.col(code, "ItemCode"), "");
            String newCode = Objects.toString(ItemPmRepository.col(code, "ItemCodeNew"), "");
            out.put("ItemCode", itemCode);
            out.put("ItemCodeNew", repo.configuration(u, "ItemCodingEnable")
                    ? newCode + "-" + ItemPmRepository.intOf(itemCode)
                    : newCode);
        }
        if (withAccounts) {
            Map<String, Object> gl = repo.glAccounts(u, category);
            if (!gl.isEmpty()) {
                out.put("PurchaseGLAC", ItemPmRepository.col(gl, "InventoryAccountId"));
                out.put("SaleGLAC", ItemPmRepository.col(gl, "RevenueAccountId"));
                out.put("COGSGLAC", ItemPmRepository.col(gl, "CGSAccountId"));
            }
        }
        return out;
    }

    public List<Map<String, Object>> history(int category, int type, int master) {
        UserAccount u = user("View");
        return repo.history(u, allowed(u, "CanView AllRecord"), category, type, master);
    }

    public Map<String, Object> record(int id) {
        return repo.details(user("View"), id);
    }

    // ------------------------------------------------------------------ save / update

    @Transactional(isolation = Isolation.SERIALIZABLE)
    public Map<String, Object> save(ItemPmRequest r) {
        if (r == null || r.Id < 0) throw new IllegalArgumentException("Invalid item");
        boolean insert = r.Id == 0;
        UserAccount u = user(insert ? "Save" : "Update");

        Map<String, Object> old = null;
        if (!insert) {
            @SuppressWarnings("unchecked")
            Map<String, Object> rec = (Map<String, Object>) repo.details(u, r.Id).get("item");
            old = rec;
        }

        boolean partyProcessing = repo.feature(u, 8);

        // ------------------------------------------------ FormValidationForAddItem :787
        if (r.ItemName == null || r.ItemName.trim().isEmpty()) throw new IllegalArgumentException("Item Name Field is Required");
        Map<String, Object> unit = pick(repo.units(u), r.BaseUnitId, "Base Unit Field is Required");
        pick(repo.categories(u), r.ItemCategoryId, "Item Category Field is Required");
        // The entry combo can hold either list: ItemTypeFill binds it first and HistoryComboBind
        // (:409) then re-binds it with the types of existing PM items. Both are real rows.
        List<Map<String, Object>> typeRows = new ArrayList<>(repo.types(u));
        typeRows.addAll(repo.historyCombos(u).get("types"));
        pick(typeRows, r.ItemTypeId, "Item Type Field is Required");
        pick(repo.racks(u), r.RackId, "Rack Name Field is Required");
        if (partyProcessing && !r.IsCompany && !r.IsThirdParty) {
            throw new IllegalArgumentException("Please Check of the Check box ('Is Company' Or 'Is third Party') ");
        }
        if (dbl(r.MinStockLevel) > dbl(r.MaxStockLevel)) {
            throw new IllegalArgumentException("Min stock level cannot be greater than max stock level.");
        }
        List<Map<String, Object>> accounts = repo.accounts(u);
        pick(accounts, r.PurchaseGLAC, "Stock A/c Field is Required");
        pick(accounts, r.SaleGLAC, "Sale A/c Field is Required");
        pick(accounts, r.COGSGLAC, "CGS A/c Filed is Required");

        // Selections that are optional on the desktop but must still be real rows.
        if (r.MasterItemId > 0) pick(repo.masterItems(u), r.MasterItemId, "Select a Master Item from the list");
        if (r.PackSizeId > 0) pick(repo.packSizes(), r.PackSizeId, "Select a Pack Size from the list");
        if (r.ApplyGST && r.TaxTypeId > 0) pick(repo.taxes(u), r.TaxTypeId, "Select a Tax Type from the list");
        if (!insert && ItemPmRepository.intOf(old.get("ItemCategoryId")) != r.ItemCategoryId) {
            // grdhistory_DoubleClick :1129 makes the category read-only once an item is opened.
            throw new IllegalArgumentException("Item Category cannot change after the item is saved");
        }
        length(r.ItemName, 100, "Item Name");
        length(r.BarcodeNo, 50, "Barcode No");

        // ------------------------------------------------ the Item property bag
        Map<String, Object> item = new LinkedHashMap<>();
        item.put("Id", insert ? 0 : r.Id);
        item.put("ItemCode", trim(r.ItemCode));
        item.put("ItemCodeNew", trim(r.ItemCodeNew));
        item.put("BarcodeNo", trim(r.BarcodeNo));
        item.put("ItemName", r.ItemName.trim());
        item.put("BaseUnitId", r.BaseUnitId);
        double equivalent = dbl(ItemPmRepository.col(unit, "Equivalent"));
        item.put("Equivalent", equivalent);
        item.put("ItemCategoryId", r.ItemCategoryId);
        item.put("ItemTypeId", r.ItemTypeId);
        item.put("MasterItemId", r.MasterItemId);
        item.put("RackId", r.RackId);
        item.put("EmptyBagWeight", dbl(r.EmptyBagWeight));
        item.put("ItemClassId", 1);                                             // :888 / :1034
        item.put("MinStockLevel", dbl(r.MinStockLevel));
        item.put("MaxStockLevel", dbl(r.MaxStockLevel));
        item.put("ReorderLevel", dbl(r.ReorderLevel));
        item.put("LeadTimeDay", r.LeadTimeDay);
        item.put("PackSize", dbl(r.PackSize));
        item.put("PackSizeId", r.PackSizeId);
        item.put("AllowMultiUom", r.AllowMultiUom);
        item.put("PurchaseGLAC", r.PurchaseGLAC);
        item.put("SaleGLAC", r.SaleGLAC);
        item.put("COGSGLAC", r.COGSGLAC);
        item.put("OrganizationId", u.getOrganizationId());
        item.put("CompanyId", u.getCompanyId());
        if (partyProcessing) { item.put("IsThirdParty", r.IsThirdParty); item.put("IsCompany", r.IsCompany); }
        else { item.put("IsThirdParty", false); item.put("IsCompany", true); }  // :915-919 / :1057-1061
        if (insert) {
            item.put("ItemStatus", true);                                        // :889
            item.put("ReOrderQty", dbl(r.ReOrderQty));                           // :893 (Save only)
            item.put("ItemGroupId", 0);                                          // :903
            item.put("EntryUser", u.getId());                                    // :906
            item.put("IsImport", r.LocalSelected);                               // :909 - IsImport = rdLocal.Checked
            if (r.ApplyGST) { item.put("ApplyGST", true); item.put("TaxTypeId", r.TaxTypeId); }   // :920-924
        } else {
            item.put("ItemStatus", r.ItemStatus);                                // :1035
            item.put("ApplyGST", r.ApplyGST);                                    // :1046 - TaxTypeId is not read on update
            item.put("ModifyUser", u.getId());                                   // :1049
        }

        // ------------------------------------------------ images
        List<Map<String, Object>> images = images(u, r, insert);
        item.put("Pic1", "");                                                   // Path.GetFileName("") when a slot is empty
        item.put("Pic2", "");
        for (Map<String, Object> img : images) {
            int slot = ItemPmRepository.intOf(img.get("SortNo"));
            item.put("Pic" + slot, img.get("FileName"));
        }

        // ------------------------------------------------ allocation grid :941-976 (Save only)
        List<ItemPmWriter.Allocation> allocations = new ArrayList<>();
        if (insert) {
            List<Map<String, Object>> companies = repo.companies(u);
            if (companies.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
            Set<Integer> ticked = new HashSet<>(r.companies == null ? List.of() : r.companies);
            for (Integer id : ticked) {
                if (id == null || companies.stream().noneMatch(c -> ItemPmRepository.intOf(c.get("Id")) == id)) {
                    throw new IllegalArgumentException("Select a Location from this organization");
                }
            }
            int falseCount = 0;
            for (Map<String, Object> c : companies) {
                int id = ItemPmRepository.intOf(c.get("Id"));
                if (companies.size() == 1) {
                    allocations.add(new ItemPmWriter.Allocation(id, true, u.getBranchesId(), u.getOrganizationId()));
                } else if (ticked.contains(id)) {
                    allocations.add(new ItemPmWriter.Allocation(id, true, u.getBranchesId(), u.getOrganizationId()));
                } else {
                    falseCount++;
                }
            }
            if (companies.size() > 1 && companies.size() == falseCount) {
                throw new IllegalArgumentException("Please select Location first");
            }
        }

        var years = shared.years(u);
        if (insert && years.isEmpty()) throw new IllegalArgumentException("No active financial year allocated to this company");

        ItemPmWriter.Save s = new ItemPmWriter.Save();
        s.insert = insert;
        s.id = r.Id;
        s.item = item;
        s.itemCategoryId = r.ItemCategoryId;
        s.itemTypeId = r.ItemTypeId;
        s.itemName = r.ItemName.trim();
        s.itemCode = trim(r.ItemCode);
        s.equivalent = equivalent;
        s.applyGst = Boolean.TRUE.equals(item.get("ApplyGST"));
        s.taxTypeId = ItemPmRepository.intOf(item.get("TaxTypeId"));
        s.reorderLevel = dbl(r.ReorderLevel);
        s.minStockLevel = dbl(r.MinStockLevel);
        s.maxStockLevel = dbl(r.MaxStockLevel);
        s.allocations = allocations;
        s.images = images;
        s.autoCoaInsertIsOn = repo.configuration(u, "AutoCoaDefineByItemNameOnInsertPm");
        s.financialYearId = years.isEmpty() ? 0 : ItemPmRepository.intOf(years.get(0).get("Id"));

        int id;
        try {
            id = writer.save(u, s);
        } catch (IllegalStateException e) {
            throw new IllegalArgumentException(e.getMessage());
        } catch (org.springframework.dao.DataAccessException e) {
            // Sp_Item_Insert / Sp_Item_Update RAISERROR text - duplicate Item Name or Item Code,
            // a control (non-Detail) GL account, a duplicate Barcode - reaches the operator as-is.
            Throwable c = e.getMostSpecificCause();
            throw new IllegalArgumentException(c == null ? e.getMessage() : c.getMessage());
        }
        attachments(u, id, r.ItemTypeId, r);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("message", insert ? "Save Successfully" : "Update Successfully");   // :979 / :1094
        return out;
    }

    // ------------------------------------------------------------------ Master Items + EB weight

    /** btnMasterItemsUpdate_Click :1855 - rows with a Master Item > 0 or EB weight > 0 only. */
    @Transactional(isolation = Isolation.SERIALIZABLE)
    public Map<String, Object> updateMasterItems(List<ItemPmRequest.MasterItemRow> rows) {
        UserAccount u = user("Update");
        if (rows == null) rows = List.of();
        Set<Integer> masters = new HashSet<>();
        for (var m : repo.masterItems(u)) masters.add(ItemPmRepository.intOf(ItemPmRepository.col(m, "Id")));
        List<Map<String, Object>> out = new ArrayList<>();
        Set<Integer> seen = new HashSet<>();
        for (var row : rows) {
            if (row == null) continue;
            double eb = dbl(row.emptyBagWeight);
            if (!(row.masterItemId > 0 || eb > 0.0)) continue;
            if (!seen.add(row.itemId)) throw new IllegalArgumentException("Each item may appear once");
            repo.details(u, row.itemId);                                     // tenancy + PM check
            if (row.masterItemId > 0 && !masters.contains(row.masterItemId)) {
                throw new IllegalArgumentException("Select a Master Item from the list");
            }
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ItemId", row.itemId);
            m.put("MasterItemId", row.masterItemId);
            m.put("EmptyBagWeight", eb);
            out.add(m);
        }
        try {
            writer.saveMasterItemAllocations(u, out);
        } catch (org.springframework.dao.DataAccessException e) {
            Throwable c = e.getMostSpecificCause();
            throw new IllegalArgumentException(c == null ? e.getMessage() : c.getMessage());
        }
        return Map.of("message", "Record Updated successfully...", "rows", out.size());
    }

    // ------------------------------------------------------------------ files

    /**
     * Brand Images, slots 1 and 2 (:867-875 on Save, :1003-1011 on Update). A newly browsed file is
     * stored and gets a new ImagePath; an image kept from the record keeps its FileName and
     * ImagePath (DAL :380-384 - "ImagePath already set, keep as is").
     */
    private List<Map<String, Object>> images(UserAccount u, ItemPmRequest r, boolean insert) {
        List<Map<String, Object>> existing = insert ? List.of() : repo.images(r.Id);
        List<Map<String, Object>> out = new ArrayList<>();
        if (r.images == null) return out;
        Set<Integer> slots = new HashSet<>();
        for (ItemPmRequest.Image img : r.images) {
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

    /** Attachments against ScreenName "AddItemPM", RefAccountId = ItemTypeId (DAL :526-538). */
    private void attachments(UserAccount u, int item, int type, ItemPmRequest r) {
        if (r.files == null) r.files = List.of();
        if (r.removeAttachmentIds == null) r.removeAttachmentIds = List.of();
        if (r.files.size() > 10) throw new IllegalArgumentException("At most ten attachments may be uploaded at once");
        var existing = repo.attachments(u, item);
        for (Integer removed : new LinkedHashSet<>(r.removeAttachmentIds)) {
            if (removed == null || existing.stream().noneMatch(a -> ItemPmRepository.intOf(a.get("Id")) == removed)) {
                throw new IllegalArgumentException("Attachment does not belong to this item");
            }
            ProcExec.call(jdbc, "EXEC dbo.Sp_DMSAttachments_GetAllMethod @Id=?, @Activity=?", removed, "AttachmentDeleteById");
        }
        for (var file : r.files) {
            byte[] bytes = DesktopInventoryItemFileService.decode(file);
            String stored = store.store(u, file.name, bytes);
            Timestamp now = new Timestamp(System.currentTimeMillis());
            Object[] v = {0, type, 0, 0, item, file.name, now, u.getId(), now, u.getId(), u.getOrganizationId(),
                    u.getCompanyId(), 0, ItemPmRepository.SCREEN, false, stored, bytes.length / 1048576d, 0};
            ProcExec.run(jdbc, "EXEC dbo.Proc_DMSAttachments_Insert @Id=?, @RefAccountId=?, @DMSFoldersLabelsId=?, @RefDocumentTypeId=?, @RefDocumentNo=?, @Attachment=?, @EntryDate=?, @EntryUser=?, @ModifyDate=?, @ModifyUser=?, @OrganizationId=?, @CompanyId=?, @BranchId=?, @ScreenName=?, @DetailWiseAttachment=?, @UploadedFileCustomName=?, @UploadedFileSizeMb=?, @LineId=?", v);
        }
    }

    public DesktopInventoryItemFileService.Download image(int item, int image) {
        UserAccount u = user("View");
        repo.details(u, item);
        var row = repo.images(item).stream().filter(r -> ItemPmRepository.intOf(r.get("Id")) == image).findFirst()
                .orElseThrow(() -> new org.springframework.web.server.ResponseStatusException(org.springframework.http.HttpStatus.NOT_FOUND, "Image not found"));
        String path = basename(Objects.toString(row.get("ImagePath"), ""));
        byte[] bytes = store.read(u, path);
        return new DesktopInventoryItemFileService.Download(basename(Objects.toString(row.get("FileName"), path)), bytes,
                DesktopInventoryItemFileService.imageType(bytes));
    }

    public DesktopInventoryItemFileService.Download attachment(int item, int attachment) {
        UserAccount u = user("View");
        repo.details(u, item);
        var row = repo.attachments(u, item).stream().filter(r -> ItemPmRepository.intOf(r.get("Id")) == attachment).findFirst()
                .orElseThrow(() -> new org.springframework.web.server.ResponseStatusException(org.springframework.http.HttpStatus.NOT_FOUND, "Attachment not found"));
        String stored = Objects.toString(row.get("UploadedFileCustomName"), "");
        if (stored.isBlank()) stored = Objects.toString(row.get("Attachment"), "");
        return new DesktopInventoryItemFileService.Download(basename(Objects.toString(row.get("Attachment"), stored)),
                store.read(u, basename(stored)), "application/octet-stream");
    }

    // ------------------------------------------------------------------ helpers

    private static Map<String, Object> pick(List<Map<String, Object>> rows, int id, String message) {
        if (id <= 0) throw new IllegalArgumentException(message);
        return rows.stream().filter(r -> ItemPmRepository.intOf(ItemPmRepository.col(r, "Id")) == id).findFirst()
                .orElseThrow(() -> new IllegalArgumentException(message));
    }

    private static double dbl(Object v) {
        if (v == null) return 0d;
        if (v instanceof BigDecimal b) return b.doubleValue();
        if (v instanceof Number n) return n.doubleValue();
        try { return Double.parseDouble(v.toString().replace(",", "").trim()); } catch (NumberFormatException e) { return 0d; }
    }

    private static String trim(String s) { return s == null ? "" : s.trim(); }

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
