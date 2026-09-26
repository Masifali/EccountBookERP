package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.StoreDefineAssetsExtraDto;
import com.mst.repositories.ItemCategoryStoreRepository;
import com.mst.repositories.ItemPmWriter;
import com.mst.repositories.ItemStoreMasterSupport;
import com.mst.repositories.StoreDefineAssetsExtraRepository;
import com.mst.repositories.StoreDefineLookupsRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.ItemStoreMasterSupport.convToBool;
import static com.mst.repositories.ItemStoreMasterSupport.convToInt;
import static com.mst.repositories.ItemStoreMasterSupport.convToString;
import static com.mst.repositories.ItemStoreMasterSupport.desktopContains;
import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;

/**
 * The two forms the Define Asset dialog (frmLookUpDefineAsset) opens with its "+" buttons, as
 * dialogs stacked above it (countx_store_define_lookups.js):
 *
 *   (a) "+" next to Asset Category — frmLookUpDefineAsset.cs:449 btnAssetCategory_Click:
 *       {@code new frmItemCatagoryStore(UserAccount){ FormTypeId = 3 }.Show()}.
 *       Architecture.WinApp.StoreManagement.frmItemCatagoryStore, base.Name "frmItemCatagoryStore",
 *       ClientSize 964 x 566. FormTypeId 3 → ParentCategoryIds "5", ClassGroupIds "6", caption
 *       "Item Category (Fix Assets)" (InvDeffrmItemCatagory_Load:187). API /api/store/define/asset-category.
 *   (b) "+" next to Asset Item — frmLookUpDefineAsset.cs:463 BtnAssetItemDefine_Click:
 *       {@code new frmDefineAssets(UserAccount).Show()}. Architecture.WinApp.FixedAsset.frmDefineAssets,
 *       base.Name "frmDefineAssets", ClientSize 1044 x 561, caption "Add Fixed Assest Item".
 *       API /api/store/define/fixed-asset-item. Its toolbar "Item Category" (btnitmcat_Click:1139) opens
 *       the SAME form in the SAME mode (frmItemCatagoryStore, FormTypeId 3) — the web reuses dialog (a).
 *
 * Neither is a document (no DocumentTypeId). BLL/DAL/Model: see {@link StoreDefineAssetsExtraRepository}.
 *
 * RIGHTS (GoldenAceDb(0509)t.sql, dbo.ScreenDefinition):
 *   (a) "frmItemCatagoryStore" has NO ScreenDefinition row (the only item-category rows are 112
 *       InvDeffrmItemCatagory, 336 ItemCategoryStore, 743 frmInvDefineItemCategory — all
 *       InvDeffrmItemCatagory). The form reads no right. Same rule as StoreDefineLookupsService:
 *       Admin, or View on the whitelisted HOST page that opened the chain, or the matching right on
 *       "frmItemCatagoryStore" (honoured should a row be added).
 *   (b) "frmDefineAssets" HAS a row: 356 "Fixed Asset Items", ModuleId 25, IsActive 1, CompanyIds ",58,64".
 *       The form calls SetRightsValueInRightsObject(base.Name) (InvDefrmAddItem_Load:250):
 *       btnsave.Enabled = Save right, btnupdate.Enabled = Update right, history CanViewAllRecord =
 *       "CanView AllRecord" right. Reads: Admin, or host View, or View on frmDefineAssets. Save / Update:
 *       the frmDefineAssets Save / Update right (Admin short-circuit) — exactly the desktop's
 *       button gate; the host rule does NOT unlock them (see deviation D4).
 *
 * DESKTOP BEHAVIOUR REPRODUCED (not corrected)
 *   (a) frmItemCatagoryStore, FormTypeId 3
 *   1. Parent Category / Class Group are filtered with string Contains ("5".Contains(Id), "6".Contains(Id))
 *      and, after Load / Refresh, the FIRST row is activated (:339, :365) — so they come pre-selected;
 *      New / after save clears them (Reset:812, :818) and "Please Select Parent Category" then fires
 *      until one is picked again.
 *   2. Validation order and texts (formvalidation:456): "Please Insert Code" (Trim()=="" or untrimmed "0"),
 *      "Please Insert Description", "Please Insert Serial From", "Please Insert Serial To",
 *      "Please Select Accumulated Account" (Revenue combo), "Please Select Assets Account" (Inventory combo),
 *      "Please Select Depreciation Account" (CGS combo), "Please Select Parent Category". Class Group,
 *      Expense Maintenance, Depreciation Method, Useful Life and Rate are never validated.
 *   3. The Revenue combo is labelled "Accumulated A/c", Inventory "Assets GL A/c", CGS "Depreciation A/c"
 *      (:192-194). Save sends the same ids twice: RevenueAccountId = accumulatedDepreciationAcId,
 *      InventoryAccountId = CapitalWipAcId, CGSAccountId = DepreciationExpenseAcId (:543-551).
 *   4. Accumulated and Assets GL lists are both AccountTypeId {1}; Depreciation and Expense Maintenance
 *      both {11,12,14,20,21,23} (AssetAccountBind:405, ExpenseAccountFill:424).
 *   5. Serial / Useful-life boxes accept digits only; the rate box decimals (KeyPress). Conversion.ToInt /
 *      ToDecimal of the text: anything unparsable is saved as 0.
 *   6. One CategoryDepreciationSchedule row is ALWAYS written (isActive true, the method may be 0)
 *      by [Asset].[USP_CategoryDepreciationSchedule_Insert] after the category, same transaction.
 *   7. ERP feature 13 (Attributes): only CHECKED rows whose StatusValue is 0 are sent, with
 *      itemCategoryId = RecId — 0 for a new category (the new id is known only after the insert);
 *      isActive is never set (false); unchecking an existing attribute does nothing.
 *   8. Reset (New / after save) keeps "Is Active" and does not clear the attribute checks' source;
 *      ReadById keeps the previous Expense Maintenance / Depreciation Method when the record has 0.
 *   9. Update keeps AssetCategoryId from ReadById (FixedAssetCategoryId); ItemProductionStageId and
 *      ItemVarietyNatureId are sent as 0 (overwrite) on update.
 *  10. Messages "Record Save Successfully." / "Record Update Successfully."; Yes/No "Are you sure to Save?" /
 *      "Are you sure to Update?" asked before the request (client side).
 *   (b) frmDefineAssets
 *  11. Validation order and texts (FormValidationForAddItem:521): "Item Name Field is Required",
 *      "Base Unit Field is Required", "Item Category Field is Required", "Item Type Field is Required",
 *      "Item Class Field is Required", "Asset GL A/c Field is Required", "Depreciation_Expense A/c Field is
 *      Required", "Accumulated_Depreciation A/c Filed is Required", "Expense Maintenance A/c Filed is
 *      Required"; then the confirm; then the allocation grid: "Grid Record Not Found" (no rows),
 *      "Please select Location first" (several rows, none ticked). With ONE company row it is allocated
 *      even when unticked (:650).
 *  12. IsImport = rdLocal.Checked (:634) — "Local" saves IsImport = 1. ReadById never restores the radios.
 *  13. ItemStatus is always true on insert (RecId == 0 || chkstatus.Checked, :615).
 *  14. Update sends a fresh Item: PurchaseGLAC / SaleGLAC / COGSGLAC = 0, ReOrderQty = 0 (txtReOrderQty is
 *      never filled), BranchesId / ProjectsId = 0, ModifyUser = 0, CommOnSale / CommOnPurchase false and
 *      every string the form does not own = NULL — overwriting what the Item screen stored. Min / Max /
 *      ReOrder levels round-trip through the hidden boxes formatted "#,##0.###" (3 decimals).
 *      LeadTimeDay is shown "#,##0"; a value >= 1000 ("1,000") fails Conversion.ToInt and saves 0.
 *  15. Save runs BLL Item.Save(items, AutoInsertCoa: true) → DAL 0436 SetData (ItemPmWriter): with
 *      configuration AutoCoaDefineByItemNameOnInsert on, a new asset item ALSO gets auto-created Stock /
 *      Sale chart-of-account accounts (parent category 5 != 9). Insert writes the base-unit UOM schedule,
 *      one ItemAllocation per allocated company and the FeedMill default price row; Sp_Item_Update
 *      returns 0 so an update writes none of them (allocation changes on update are ignored).
 *  16. Leaving Item Category (only while Save is visible AND enabled) regenerates the Asset Code and
 *      copies the category's four GL accounts (cmbItemCategory_Leave:402); any error → "Record Not Found".
 *      New / after save re-run it for the category still selected.
 *  17. History: first opening of the History tab loads 50 rows (tabCAddItem_SelectedIndexChanged:1017),
 *      its "New" button loads all; CanViewAllRecord from the rights, otherwise only the user's rows.
 *      No rows → the grid structure is cleared.
 *  18. Messages "Save Successfully" / "Update Successfully"; update with RecId 0 →
 *      "Record Not Update because RecId Not Found".
 *
 * DEVIATIONS (web only, with reason)
 *   D1. Tenancy: open / update only a category of the session's organization + company whose parent is
 *       in "5" (what the dialog's history lists), only an item of the session's organization + company
 *       whose category has parent 5; org / company / branch / user / year always from the session.
 *   D2. Combo values are re-checked against the server-built lists (LimitToList parity); a value outside
 *       its list counts as "not selected" and gets the desktop's message. Allocation rows are kept only
 *       for companies of the session's organization.
 *   D3. Asset Code (read-only) is never taken from the request: insert → GenerateCode at save time
 *       (Sp_Item_Insert regenerates it anyway), update → the stored codes. Hidden Min/Max/ReOrder levels
 *       are the stored values (update) or 0 (insert) — what the hidden boxes would carry.
 *   D4. frmDefineAssets Save / Update need that screen's own Save / Update right (desktop button gate);
 *       the "host page may use the dialog" rule applies to reads only.
 *   D5. Pictures: the desktop stores Pic1 / Pic2 as a Windows file path under configuration
 *       "Attachment Folder Path" and File.Copy()s the picked file there from the client machine after
 *       the save; it shows them with Image.FromFile. The web server cannot reach that share: Browse is
 *       disabled, insert sends Pic1 = Pic2 = "" (what the desktop sends when nothing is browsed), and an
 *       update keeps the stored paths (the desktop keeps them when the file still exists, clears when it
 *       does not — existence cannot be checked here).
 *   D6. The account / category lists are read per request; the desktop reads the login-time global
 *       account cache (refreshed by its Refresh button).
 */
@Service
public class StoreDefineAssetsExtraService {

    public static final String SCREEN_CATEGORY = "frmItemCatagoryStore";
    public static final String SCREEN_ASSET_ITEM = "frmDefineAssets";

    /** FormTypeId 3 (InvDeffrmItemCatagory_Load:187). */
    static final String PARENT_CATEGORY_IDS = "5";
    static final String CLASS_GROUP_IDS = "6";
    static final int[] ASSET_ACCOUNT_TYPES = {1};
    static final int[] EXPENSE_ACCOUNT_TYPES = {11, 12, 14, 20, 21, 23};

    private final StoreDefineAssetsExtraRepository repo;
    private final ItemCategoryStoreRepository categoryRepo;
    private final ItemStoreMasterSupport support;
    private final StoreDefineLookupsRepository lookupsRepo;
    private final ItemPmWriter itemWriter;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public StoreDefineAssetsExtraService(StoreDefineAssetsExtraRepository repo, ItemCategoryStoreRepository categoryRepo,
                                         ItemStoreMasterSupport support, StoreDefineLookupsRepository lookupsRepo,
                                         ItemPmWriter itemWriter, StoreScreenRights rights, CurrentUserContext ctx) {
        this.repo = repo;
        this.categoryRepo = categoryRepo;
        this.support = support;
        this.lookupsRepo = lookupsRepo;
        this.itemWriter = itemWriter;
        this.rights = rights;
        this.ctx = ctx;
    }

    // ================================================================================== rights

    private boolean admin() { return "Admin".equals(ctx.currentRoleName()); }

    private boolean hostView(String host) {
        return host != null && StoreDefineLookupsService.HOSTS.contains(host) && rights.has(host, "view");
    }

    /** Same rule as StoreDefineLookupsService.require. */
    private void require(String ownScreen, String host, String right) {
        if (admin()) return;
        if (hostView(host)) return;
        if (rights.has(ownScreen, right)) return;
        throw new IllegalStateException("You do not have the rights to use this screen.");
    }

    // =============================================================== (a) Item Category (Fix Assets)

    /** InvDeffrmItemCatagory_Load:166 with FormTypeId 3 (also btnRefresh_Click:856 — the page re-binds with retain). */
    public Map<String, Object> categoryLookups(String host) {
        require(SCREEN_CATEGORY, host, "view");
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        boolean multiLanguage = support.erpFeature(u, 7);                         // :200
        boolean attributes = support.erpFeature(u, 13);                           // :202
        out.put("multiLanguage", multiLanguage);
        out.put("attributesFeature", attributes);
        out.put("attributes", attributes ? repo.attributesForCategory(u, 0) : new ArrayList<>());   // AttributeGridFill(0)
        out.put("parentCategories", parentCategories());                          // ItemParentCategoryFill:316
        out.put("classGroups", classGroups());                                    // GetItemClassGroup:350
        out.put("history", categoryHistory(u));                                   // gridfill:687
        List<Map<String, Object>> assets = repo.accountsByTypes(u, ASSET_ACCOUNT_TYPES);   // AssetAccountBind:405
        List<Map<String, Object>> expense = repo.accountsByTypes(u, EXPENSE_ACCOUNT_TYPES); // ExpenseAccountFill:424
        out.put("assetAccounts", assets);
        out.put("expenseAccounts", expense);
        out.put("depreciationMethods", repo.depreciationMethods(u));              // DepreciatonMethodNameBind:443
        return out;
    }

    /** gridfill:687 — reloaded after ReadById / Reset. */
    public List<Map<String, Object>> categoryHistory(String host) {
        require(SCREEN_CATEGORY, host, "view");
        return categoryHistory(ctx.requireAccountingUser());
    }

    /** AttributeGridFill(CategoryId):234. */
    public List<Map<String, Object>> categoryAttributes(int categoryId, String host) {
        require(SCREEN_CATEGORY, host, "view");
        UserAccount u = ctx.requireAccountingUser();
        if (categoryId != 0) ownedCategory(u, categoryId);
        return repo.attributesForCategory(u, categoryId);
    }

    /** grdfrm_DoubleClick:779 → ReadById:634. */
    public Map<String, Object> category(int id, String host) {
        require(SCREEN_CATEGORY, host, "view");
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> c = ownedCategory(u, id);
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", toInt(ci(c, "Id")));
        o.put("CategoryCode", convToString(ci(c, "CategoryCode")));
        o.put("CategoryDescription", convToString(ci(c, "CategoryDescription")));
        o.put("SerialFrom", String.valueOf(toInt(ci(c, "SerialFrom"))));
        o.put("SerialTo", String.valueOf(toInt(ci(c, "SerialTo"))));
        o.put("RevenueAccountId", toInt(ci(c, "RevenueAccountId")));
        o.put("CGSAccountId", toInt(ci(c, "CGSAccountId")));
        o.put("InventoryAccountId", toInt(ci(c, "InventoryAccountId")));
        o.put("accumulatedDepreciationAcId", toInt(ci(c, "accumulatedDepreciationAcId")));
        o.put("CapitalWipAcId", toInt(ci(c, "CapitalWipAcId")));
        o.put("DepreciationExpenseAcId", toInt(ci(c, "DepreciationExpenseAcId")));
        o.put("ExpenseMaintenanceAccountId", toInt(ci(c, "ExpenseMaintenanceAccountId")));
        o.put("depreciationMethodScheduleId", toInt(ci(c, "depreciationMethodScheduleId")));
        o.put("usefullLifeInMonths", String.valueOf(toInt(ci(c, "usefullLifeInMonths"))));      // Conversion.ToString(int)
        o.put("depriciationrate", decimalText(ci(c, "depriciationrate")));                      // Conversion.ToString(decimal)
        o.put("InventoryParentCategoriesId", toInt(ci(c, "InventoryParentCategoriesId")));
        o.put("ItemClassGroupId", toInt(ci(c, "ItemClassGroupId")));
        o.put("CategoryStatus", convToBool(ci(c, "CategoryStatus")));
        return o;
    }

    /** btnsave_Click:609 (Id 0) / btnUpdate_Click:622 → Insert:512 → BLL ItemCategory.Save → DAL 0437 SetData (one transaction). */
    @Transactional
    public Map<String, Object> saveCategory(StoreDefineAssetsExtraDto.AssetCategory d) {
        String host = d == null ? null : d.host;
        int recId = d == null || d.Id == null ? 0 : d.Id;
        require(SCREEN_CATEGORY, host, recId > 0 ? "update" : "save");
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> stored = recId > 0 ? ownedCategory(u, recId) : null;          // D1

        List<Map<String, Object>> assets = repo.accountsByTypes(u, ASSET_ACCOUNT_TYPES);
        List<Map<String, Object>> expense = repo.accountsByTypes(u, EXPENSE_ACCOUNT_TYPES);
        String code = nz(d.CategoryCode), desc = nz(d.CategoryDescription);
        String sFrom = nz(d.SerialFrom), sTo = nz(d.SerialTo);
        int revenue = inList(assets, "Id", nz(d.RevenueAccountId));
        int inventory = inList(assets, "Id", nz(d.InventoryAccountId));
        int cgs = inList(expense, "Id", nz(d.CGSAccountId));
        int parent = inList(parentCategories(), "Id", nz(d.ParentCategoryId));
        int classGroup = inList(classGroups(), "Id", nz(d.ClassGroupId));
        int expenseMaint = inList(expense, "Id", nz(d.ExpenseMaintenanceAccountId));
        int method = inList(repo.depreciationMethods(u), "depreciationMethodScheduleId", nz(d.DepreciationMethodScheduleId));

        // ---- formvalidation:456, same order and texts (FormTypeId 3 wording)
        if (code.trim().isEmpty() || code.equals("0")) throw new IllegalStateException("Please Insert Code");
        if (desc.trim().isEmpty() || desc.trim().equals("0")) throw new IllegalStateException("Please Insert Description");
        if (sFrom.trim().isEmpty() || sFrom.trim().equals("0")) throw new IllegalStateException("Please Insert Serial From");
        if (sTo.trim().isEmpty() || sTo.trim().equals("0")) throw new IllegalStateException("Please Insert Serial To");
        if (revenue == 0) throw new IllegalStateException("Please Select Accumulated Account");
        if (inventory == 0) throw new IllegalStateException("Please Select Assets Account");
        if (cgs == 0) throw new IllegalStateException("Please Select Depreciation Account");
        if (parent == 0) throw new IllegalStateException("Please Select Parent Category");

        int months = convToInt(d.UseFullLifeMonths);                                    // :554
        BigDecimal rate = convToDecimal(d.DepreciationRate);                             // :555
        int assetCategoryId = stored == null ? 0 : toInt(ci(stored, "AssetCategoryId"));  // FixedAssetCategoryId (:565, ReadById:672)

        // ---- Model.Inventory.ItemCategory, property order (GenericProvider.SetProc); BLL Save stamps the dates
        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("CategoryStatus", Boolean.TRUE.equals(d.CategoryStatus));
        m.put("EntryDate", now);
        m.put("ModifyDate", now);
        m.put("CGSAccountId", cgs);
        m.put("CompanyId", u.getCompanyId());
        m.put("EntryUser", u.getId());
        m.put("Id", recId);
        m.put("InventoryAccountId", inventory);
        m.put("ModifyUser", u.getId());
        m.put("OrganizationId", u.getOrganizationId());
        m.put("RevenueAccountId", revenue);
        m.put("SerialFrom", convToInt(sFrom));
        m.put("SerialTo", convToInt(sTo));
        m.put("InventoryParentCategoriesId", parent);
        m.put("accumulatedDepreciationAcId", revenue);                                   // :549
        m.put("ExpenseMaintenanceAccountId", expenseMaint);                              // :552
        m.put("DepreciationExpenseAcId", cgs);                                           // :551
        m.put("CapitalWipAcId", inventory);                                              // :550
        m.put("ItemClassGroupId", classGroup);
        m.put("CategoryCode", code);                                                     // untrimmed (:539)
        m.put("CategoryDescription", desc);
        m.put("ItemProductionStageId", 0);
        m.put("ItemVarietyNatureId", 0);
        m.put("usefullLifeInMonths", months);
        m.put("depriciationrate", rate);
        m.put("AssetCategoryId", assetCategoryId);
        m.put("depreciationMethodScheduleId", method);

        int num = categoryRepo.setProc(recId == 0 ? "Sp_ItemCategory_Insert" : "Sp_ItemCategory_Update", m);
        int categoryId = recId == 0 ? num : recId;                                       // DAL: if (obj.Id == 0) obj.Id = num

        if (support.erpFeature(u, 13)) {                                                  // DAL feature-13 branch
            /* Insert:571 — AttributesForItemFeature is the same feature flag read at Load. */
            Set<Long> checked = new HashSet<>(d.CheckedAttributeIds == null ? new ArrayList<>() : d.CheckedAttributeIds);
            for (Map<String, Object> a : repo.attributesForCategory(u, recId)) {       // the rows the grid showed for RecId
                long attributeId = ((Number) a.get("attributeId")).longValue();
                if (!checked.contains(attributeId) || toInt(a.get("StatusValue")) != 0) continue;
                Map<String, Object> ca = new LinkedHashMap<>();                         // Model 0906 order
                ca.put("isActive", false);                                              // never set (:578-588)
                ca.put("EntryDate", now);
                ca.put("ModifyDate", now);
                ca.put("companyId", u.getCompanyId());
                ca.put("EntryUserId", u.getId());
                ca.put("itemCategoryId", recId);                                        // RecId — 0 on insert (note 7)
                ca.put("ModifyUserId", u.getId());
                ca.put("organizationId", u.getOrganizationId());
                ca.put("seqNo", 0);
                ca.put("attributeId", attributeId);
                ca.put("CategoryAttributeId", ((Number) a.get("CategoryAttributeId")).longValue());
                repo.saveCategoryAttribute(ca);
            }
            categoryRepo.itemAttributesByCategory(u, categoryId);
        }

        Map<String, Object> s = new LinkedHashMap<>();                                   // :556-561, Model 0740 order
        s.put("isActive", true);
        s.put("rateofDepreciation", rate);
        s.put("categoryDepreciationScheduleId", 0);
        s.put("categoryId", categoryId);
        s.put("depreciationMethodScheduleId", method);
        s.put("sortNo", 0);
        s.put("usefullLifeInMonths", months);
        repo.saveCategoryDepreciationSchedule(s);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", categoryId);
        out.put("message", recId > 0 ? "Record Update Successfully." : "Record Save Successfully.");
        return out;
    }

    private List<Map<String, Object>> parentCategories() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : support.inventoryParentCategories()) {
            if (desktopContains(PARENT_CATEGORY_IDS, r.get("Id"))) out.add(r);
        }
        return out;
    }

    private List<Map<String, Object>> classGroups() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : categoryRepo.classGroups()) {
            if (desktopContains(CLASS_GROUP_IDS, r.get("Id"))) out.add(r);
        }
        return out;
    }

    /** gridfill:687 — FormHistory(CategoryCode = ParentCategoryIds "5"), columns :705-726. */
    private List<Map<String, Object>> categoryHistory(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : categoryRepo.formHistory(u, PARENT_CATEGORY_IDS)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("CategoryCode", str(ci(r, "categoryCode")));
            o.put("CategoryDescription", str(ci(r, "CategoryDescription")));
            o.put("SerialFrom", toInt(ci(r, "SerialFrom")));
            o.put("SerialTo", toInt(ci(r, "SerialTo")));
            o.put("CategoryStatus", str(ci(r, "CategoryStatus")));
            o.put("ParentCategory", str(ci(r, "InvParentCateDescription")));
            o.put("AccumulatedDepreciationAccount", str(ci(r, "AccumulatedDepreciationAccount")));
            o.put("AssetGlAccount", str(ci(r, "CapitalWipAccount")));
            o.put("DepreciationExpenseAccount", str(ci(r, "DepreciationExpenseAccount")));
            o.put("ExpenseMaintenanceAccount", str(ci(r, "ExpenseMaintenanceAccount")));
            o.put("ClassGroupName", str(ci(r, "ClassGroupName")));
            o.put("DepreciationMethodName", str(ci(r, "depreciationMethodName")));
            o.put("UsefullLifeInMonths", toInt(ci(r, "usefullLifeInMonths")));
            o.put("DepreciationRate", toDouble(ci(r, "depriciationrate")));
            out.add(o);
        }
        return out;
    }

    /** D1 — own org + company, parent category in the "5" history set. */
    private Map<String, Object> ownedCategory(UserAccount u, int id) {
        Map<String, Object> c = categoryRepo.readById(id);
        if (c == null
                || toInt(ci(c, "OrganizationId")) != nz(u.getOrganizationId())
                || toInt(ci(c, "CompanyId")) != nz(u.getCompanyId())
                || toInt(ci(c, "InventoryParentCategoriesId")) != 5) {
            throw new IllegalStateException("Record Not Found");
        }
        return c;
    }

    // ================================================================== (b) Add Fixed Assest Item

    /** InvDefrmAddItem_Load:243 (and BtnRefresh_Click:871 — the page re-binds with retain). */
    public Map<String, Object> assetItemLookups(String host) {
        requireAssetItemRead(host);
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Boolean> r = rights.of(SCREEN_ASSET_ITEM);                             // :250-252
        Map<String, Object> rr = new LinkedHashMap<>();
        rr.put("save", r.get("save"));
        rr.put("update", r.get("update"));
        rr.put("viewAll", r.get("viewAll"));
        out.put("rights", rr);
        out.put("categories", assetCategories(u));                                         // ItemCatagoryHistoryFill:308
        out.put("itemTypes", repo.itemTypes(u));                                           // ItemTypeFill:373
        out.put("companies", repo.companies(u));                                           // CompaniesBindInGrid:443
        out.put("assetAccounts", repo.accountsByTypes(u, ASSET_ACCOUNT_TYPES));            // AssetAccountBind:270
        out.put("expenseAccounts", repo.accountsByTypes(u, EXPENSE_ACCOUNT_TYPES));        // ExpenseAccountFill:289
        out.put("uoms", repo.uoms(u));                                                     // BaseUnitFill:341
        out.put("classes", repo.itemClasses());                                            // ItemClassFill:488
        return out;
    }

    /** ItemCatagoryHistoryFill:308 alone — re-read after the nested Item Category dialog closes. */
    public List<Map<String, Object>> assetItemCategories(String host) {
        requireAssetItemRead(host);
        return assetCategories(ctx.requireAccountingUser());
    }

    /** cmbItemCategory_Leave:402 — GenerateCode + GetGLAccountbyItemCategoryIdForFixedAssets. */
    public Map<String, Object> assetItemCategoryLeave(int categoryId, String host) {
        requireAssetItemRead(host);
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        try {
            Map<String, Object> code = repo.generateItemCode(u, categoryId);
            if (code != null) {
                out.put("ItemCode", str(ci(code, "ItemCode")));
                out.put("ItemCodeNew", str(ci(code, "ItemCodeNew")));
            }
            Map<String, Object> gl = repo.glAccountsByCategory(u, categoryId);
            if (gl != null) {
                out.put("accumulatedDepreciationAcId", toInt(ci(gl, "accumulatedDepreciationAcId")));
                out.put("DepreciationExpenseAcId", toInt(ci(gl, "DepreciationExpenseAcId")));
                out.put("CapitalWipAcId", toInt(ci(gl, "CapitalWipAcId")));
                out.put("ExpenseMaintenanceAccountId", toInt(ci(gl, "ExpenseMaintenanceAccountId")));
            }
        } catch (Exception e) {
            throw new IllegalStateException("Record Not Found");                            // :439
        }
        return out;
    }

    /** grdfrmfill(NoOfRecords):925 — 50 on the tab's first selection, 0 (all) from its "New" button. */
    public List<Map<String, Object>> assetItemHistory(int noOfRecords, String host) {
        requireAssetItemRead(host);
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = rights.has(SCREEN_ASSET_ITEM, "viewAll");                         // formrights.DoHaveCanViewAllRecordRights
        SimpleDateFormat f = new SimpleDateFormat("yyyy-MM-dd");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.fixedAssetHistory(u, viewAll, noOfRecords)) {    // :961-964
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("AssetCode", str(ci(r, "AssetCode")));
            o.put("AssetCodeNew", str(ci(r, "ItemCodeNew")));
            o.put("AssetName", str(ci(r, "AssetName")));
            o.put("ItemType", str(ci(r, "AssetType")));
            o.put("ItemCategory", str(ci(r, "AssetCategory")));
            o.put("ParentCategory", str(ci(r, "InvParentCateDescription")));
            o.put("ClassGroupName", str(ci(r, "ClassGroupName")));
            o.put("ItemStatus", netBoolText(ci(r, "ItemStatus")));                         // Conversion.ToString(bool) → "True"/"False"
            o.put("Assets_GL", str(ci(r, "Assets_GL")));
            o.put("Accumulated_Depreciation", str(ci(r, "Accumulated_Depreciation")));
            o.put("Depreciation_Expense", str(ci(r, "Depreciation_Expense")));
            Object ed = ci(r, "EntryDate");                                                 // ToShortDateString() → midnight
            o.put("EntryDate", ed instanceof java.util.Date ? f.format((java.util.Date) ed) : str(ed));
            o.put("EntryUserName", str(ci(r, "UserName")));
            o.put("NoOfAttachments", toInt(ci(r, "NoOfAttachments")));
            o.put("LeadTimeDay", toInt(ci(r, "LeadTimeDay")));
            out.add(o);
        }
        return out;
    }

    /** grdhistory_DoubleClick:901 → ReadById:740. */
    public Map<String, Object> assetItem(int id, String host) {
        requireAssetItemRead(host);
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> i = ownedItem(u, id);
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", toInt(ci(i, "Id")));
        o.put("ItemCode", convToString(ci(i, "ItemCode")));
        o.put("ItemCodeNew", convToString(ci(i, "ItemCodeNew")));
        o.put("BarcodeNo", convToString(ci(i, "BarcodeNo")));
        o.put("ItemName", convToString(ci(i, "ItemName")));
        o.put("BaseUnitId", toInt(ci(i, "BaseUnitId")));
        o.put("ItemCategoryId", toInt(ci(i, "ItemCategoryId")));
        o.put("ItemTypeId", toInt(ci(i, "ItemTypeId")));
        o.put("ItemStatus", convToBool(ci(i, "ItemStatus")));
        o.put("CapitalWipAcId", toInt(ci(i, "CapitalWipAcId")));
        o.put("DepreciationExpenseAcId", toInt(ci(i, "DepreciationExpenseAcId")));
        o.put("accumulatedDepreciationAcId", toInt(ci(i, "accumulatedDepreciationAcId")));
        o.put("ExpenseMaintenanceAccountId", toInt(ci(i, "ExpenseMaintenanceAccountId")));
        o.put("ItemClassId", toInt(ci(i, "ItemClassId")));
        o.put("LeadTimeDay", String.format(java.util.Locale.US, "%,d", toInt(ci(i, "LeadTimeDay"))));   // "#,##0"
        o.put("Pic1", convToString(ci(i, "Pic1")));
        o.put("Pic2", convToString(ci(i, "Pic2")));
        return o;
    }

    /** BtnSave_Click:711 (Id 0) / btnupdate_Click:724 → Insert:580 → BLL Item.Save(items, true) → DAL 0436 SetData. */
    @Transactional
    public Map<String, Object> saveAssetItem(StoreDefineAssetsExtraDto.FixedAssetItem d) {
        String host = d == null ? null : d.host;
        int recId = d == null || d.Id == null ? 0 : d.Id;
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_ASSET_ITEM, recId > 0 ? "update" : "save")) {                // D4 / :251-252
            throw new IllegalStateException(recId > 0 ? "You do not have the Update right for this screen."
                    : "You do not have the Save right for this screen.");
        }
        Map<String, Object> stored = recId > 0 ? ownedItem(u, recId) : null;               // D1

        List<Map<String, Object>> uoms = repo.uoms(u);
        List<Map<String, Object>> assets = repo.accountsByTypes(u, ASSET_ACCOUNT_TYPES);
        List<Map<String, Object>> expense = repo.accountsByTypes(u, EXPENSE_ACCOUNT_TYPES);
        String name = nz(d.ItemName);
        int baseUnit = inList(uoms, "Id", nz(d.BaseUnitId));
        int category = inList(assetCategories(u), "Id", nz(d.ItemCategoryId));
        int itemType = inList(repo.itemTypes(u), "Id", nz(d.ItemTypeId));
        int itemClass = inList(repo.itemClasses(), "ClassId", nz(d.ItemClassId));
        int assetGl = inList(assets, "Id", nz(d.CapitalWipAcId));
        int deprGl = inList(expense, "Id", nz(d.DepreciationExpenseAcId));
        int accum = inList(assets, "Id", nz(d.accumulatedDepreciationAcId));
        int expMaint = inList(expense, "Id", nz(d.ExpenseMaintenanceAccountId));

        // ---- FormValidationForAddItem:521, same order and texts
        if (name.trim().isEmpty()) throw new IllegalStateException("Item Name Field is Required");
        if (baseUnit == 0) throw new IllegalStateException("Base Unit Field is Required");
        if (category == 0) throw new IllegalStateException("Item Category Field is Required");
        if (itemType == 0) throw new IllegalStateException("Item Type Field is Required");
        if (itemClass == 0) throw new IllegalStateException("Item Class Field is Required");
        if (assetGl == 0) throw new IllegalStateException("Asset GL A/c Field is Required");
        if (deprGl == 0) throw new IllegalStateException("Depreciation_Expense A/c Field is Required");
        if (accum == 0) throw new IllegalStateException("Accumulated_Depreciation A/c Filed is Required");
        if (expMaint == 0) throw new IllegalStateException("Expense Maintenance A/c Filed is Required");

        // ---- allocation grid (:641-676), D2: only companies of the session's organization
        Set<Integer> orgCompanies = new HashSet<>();
        for (Map<String, Object> c : repo.companies(u)) orgCompanies.add(toInt(c.get("Id")));
        List<StoreDefineAssetsExtraDto.Allocation> rows = new ArrayList<>();
        if (d.Allocations != null) {
            for (StoreDefineAssetsExtraDto.Allocation a : d.Allocations) {
                if (a != null && a.Id != null && orgCompanies.contains(a.Id)) rows.add(a);
            }
        }
        int count = rows.size();
        if (count == 0) throw new IllegalStateException("Grid Record Not Found");
        List<ItemPmWriter.Allocation> allocations = new ArrayList<>();
        int falseCount = 0;
        for (StoreDefineAssetsExtraDto.Allocation a : rows) {
            boolean v = Boolean.TRUE.equals(a.Value);
            if (count == 1) allocations.add(new ItemPmWriter.Allocation(a.Id, true, nz(u.getBranchesId()), nz(u.getOrganizationId())));
            else if (v) allocations.add(new ItemPmWriter.Allocation(a.Id, true, nz(u.getBranchesId()), nz(u.getOrganizationId())));
            else falseCount++;
        }
        if (count > 1 && count == falseCount) throw new IllegalStateException("Please select Location first");

        // ---- D3: codes and hidden levels
        String itemCode, itemCodeNew, pic1, pic2;
        double minStock = 0, maxStock = 0, reorder = 0;
        if (stored == null) {
            Map<String, Object> g = repo.generateItemCode(u, category);
            itemCode = g == null ? "" : str(ci(g, "ItemCode")).trim();
            itemCodeNew = g == null ? "" : str(ci(g, "ItemCodeNew")).trim();
            pic1 = "";                                                                       // D5
            pic2 = "";
        } else {
            itemCode = convToString(ci(stored, "ItemCode")).trim();
            itemCodeNew = convToString(ci(stored, "ItemCodeNew")).trim();
            pic1 = convToString(ci(stored, "Pic1"));
            pic2 = convToString(ci(stored, "Pic2"));
            minStock = round3(toDouble(ci(stored, "MinStockLevel")));                        // ReadById:771 "#,##0.###"
            maxStock = round3(toDouble(ci(stored, "MaxStockLevel")));
            reorder = round3(toDouble(ci(stored, "ReorderLevel")));
        }
        double equivalent = 0;
        for (Map<String, Object> r : uoms) if (toInt(r.get("Id")) == baseUnit) equivalent = toDouble(r.get("Equivalent"));   // SelectedRow.Cells[2]

        // ---- the Item properties Insert():607-636 assigns (the rest keep their CLR defaults)
        Map<String, Object> item = new LinkedHashMap<>();
        item.put("Id", recId);
        item.put("ItemCode", itemCode);
        item.put("ItemCodeNew", itemCodeNew);
        item.put("BarcodeNo", nz(d.BarcodeNo).trim());
        item.put("ItemName", name.trim());
        item.put("BaseUnitId", baseUnit);
        item.put("ItemCategoryId", category);
        item.put("ItemTypeId", itemType);
        item.put("ItemStatus", recId == 0 || Boolean.TRUE.equals(d.ItemStatus));            // note 13
        item.put("MinStockLevel", minStock);
        item.put("MaxStockLevel", maxStock);
        item.put("ReorderLevel", reorder);
        item.put("ReOrderQty", 0d);                                                          // note 14
        item.put("LeadTimeDay", convToInt(d.LeadTimeDay));
        item.put("Pic1", pic1);
        item.put("Pic2", pic2);
        item.put("DepreciationExpenseAcId", deprGl);
        item.put("accumulatedDepreciationAcId", accum);
        item.put("CapitalWipAcId", assetGl);
        item.put("ExpenseMaintenanceAccountId", expMaint);
        item.put("ItemClassId", itemClass);
        item.put("OrganizationId", u.getOrganizationId());
        item.put("CompanyId", u.getCompanyId());
        item.put("EntryUser", u.getId());
        item.put("IsImport", !Boolean.FALSE.equals(d.Local));                                // note 12: IsImport = rdLocal.Checked
        item.put("IsThirdParty", false);
        item.put("IsCompany", true);

        ItemPmWriter.Save s = new ItemPmWriter.Save();
        s.insert = recId == 0;
        s.id = recId;
        s.item = item;
        s.itemCategoryId = category;
        s.itemTypeId = itemType;
        s.itemName = name.trim();
        s.itemCode = itemCode;
        s.equivalent = equivalent;
        s.applyGst = false;
        s.taxTypeId = 0;
        s.reorderLevel = reorder;
        s.minStockLevel = minStock;
        s.maxStockLevel = maxStock;
        s.allocations = allocations;
        s.autoCoaInsertIsOn = true;                                                           // Item.Save(items, true)
        s.financialYearId = ctx.currentFinancialYearId();
        int id = itemWriter.save(u, s);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("message", recId == 0 ? "Save Successfully" : "Update Successfully");
        return out;
    }

    private void requireAssetItemRead(String host) {
        require(SCREEN_ASSET_ITEM, host, "view");
    }

    /** ItemCategory.Getall(CategoryCode "5") → Id, CategoryDescription (same call as frmLookUpDefineAsset.ItemCategory). */
    private List<Map<String, Object>> assetCategories(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : lookupsRepo.assetCategories(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("CategoryDescription", str(ci(r, "CategoryDescription")));
            out.add(o);
        }
        return out;
    }

    /** D1 — own org + company, category under parent 5. */
    private Map<String, Object> ownedItem(UserAccount u, int id) {
        Map<String, Object> i = repo.itemById(id);
        if (i == null
                || toInt(ci(i, "OrganizationId")) != nz(u.getOrganizationId())
                || toInt(ci(i, "CompanyId")) != nz(u.getCompanyId())) {
            throw new IllegalStateException("Record Not Found");
        }
        Map<String, Object> c = repo.categoryById(toInt(ci(i, "ItemCategoryId")));
        if (c == null || toInt(ci(c, "InventoryParentCategoriesId")) != 5) throw new IllegalStateException("Record Not Found");
        return i;
    }

    // ================================================================================= helpers

    private static int inList(List<Map<String, Object>> rows, String key, int id) {
        if (id == 0) return 0;
        for (Map<String, Object> r : rows) if (toInt(r.get(key)) == id) return id;
        return 0;
    }

    private static int nz(Integer v) { return v == null ? 0 : v; }

    private static String nz(String s) { return s == null ? "" : s; }

    private static double round3(double v) { return Math.round(v * 1000d) / 1000d; }

    /** Conversion.ToDecimal(text): Convert.ToDecimal (thousands separators allowed), 0 on failure. */
    static BigDecimal convToDecimal(String s) {
        if (s == null) return BigDecimal.ZERO;
        String t = s.trim().replace(",", "");
        if (t.isEmpty()) return BigDecimal.ZERO;
        try { return new BigDecimal(t); } catch (Exception e) { return BigDecimal.ZERO; }
    }

    /** Conversion.ToString(decimal) — invariant text of the stored numeric(18,2), e.g. "10.00". */
    private static String decimalText(Object v) {
        if (v == null) return "0";
        if (v instanceof BigDecimal) return ((BigDecimal) v).toPlainString();
        return String.valueOf(v);
    }

    /** .NET bool.ToString(): "True" / "False". */
    private static String netBoolText(Object v) {
        if (v == null) return "";
        return convToBool(v) ? "True" : "False";
    }
}
